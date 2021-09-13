using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using SystemModule;

namespace GameSvr
{
    public class AppService : BackgroundService
    {
        private readonly GameApp _mirApp;
        
        public AppService(GameApp serverApp)
        {
            _mirApp = serverApp;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Task.Run(async () =>
            {
                await M2Share.RunSocket.StartConsumer();
            }, stoppingToken);
            return Task.CompletedTask;
        }
        
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("GameSvr is starting.");
            M2Share.MainOutMessage("正在读取配置信息...");
            _mirApp.InitializeServer();
            M2Share.MainOutMessage("读取配置信息完成...");
            _mirApp.StartServer(cancellationToken);
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("GameSvr is stopping.");

            //转移在线玩家到新服务器
            if (M2Share.UserEngine.PlayObjects.Any())
            {
                Task.Run(() =>
                {
                    var shutdownSeconds = 30;
                    while (true)
                    {
                        foreach (var playObject in M2Share.UserEngine.PlayObjects)
                        {
                            playObject.SysMsg($"服务器关闭倒计时 [{shutdownSeconds}].", TMsgColor.c_Red, TMsgType.t_Notice);
                            shutdownSeconds--;
                        }
                        if (shutdownSeconds > 0)
                        {
                            Thread.Sleep(TimeSpan.FromSeconds(1));
                        }
                        else
                        {
                            var sIPaddr = string.Empty;
                            var nPort = 0;
                            if (M2Share.GetMultiServerAddrPort(0, ref sIPaddr, ref nPort)) //如果有可用服务器，那就切换过去
                            {
                                foreach (var playObject in M2Share.UserEngine.PlayObjects)
                                {
                                    playObject.CrossGroupServer(sIPaddr, nPort);
                                }
                            }
                            break;
                        }
                    }
                }, cancellationToken);
            }
            return base.StopAsync(cancellationToken);
        }
    }
}