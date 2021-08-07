using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using SystemModule;

namespace M2Server
{
    public class AppService : BackgroundService
    {
        private readonly MirApp _mirApp;
        
        public AppService(MirApp serverApp)
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
            Console.WriteLine("M2Server is starting.");
            _mirApp.StartServer(cancellationToken);
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("M2Server is stopping.");
            
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
                            Task.Delay(TimeSpan.FromSeconds(1));
                        }
                        else
                        {
                            var crossGroupServer = "";//随机抽取分组可用服务器
                            foreach (var playObject in M2Share.UserEngine.PlayObjects)
                            {
                                playObject.CrossGroupServer("10.10.0.188", 7200);
                            }
                            break;
                        }
                    }
                });
            }
            return base.StopAsync(cancellationToken);
        }
    }
}