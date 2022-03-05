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
        private Timer _connectTimer;
        private int _checkIntervalTime;
        private int _saveIntervalTime;
        private int _clearIntervalTime;

        public AppService(GameApp serverApp)
        {
            _mirApp = serverApp;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _checkIntervalTime = HUtil32.GetTickCount();
            _saveIntervalTime = HUtil32.GetTickCount();
            _clearIntervalTime = HUtil32.GetTickCount();
            if (M2Share.boStartReady)
            {
                _connectTimer = new Timer(ServiceTimer, null, 1000, 3000);
                await M2Share.RunSocket.StartConsumer(stoppingToken);
            }
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            M2Share.MainOutMessage("GameSvr is starting.");
            M2Share.MainOutMessage("正在读取配置信息...");
            _mirApp.InitializeServer();
            M2Share.MainOutMessage("读取配置信息完成...");
            _mirApp.StartServer(cancellationToken);
            _mirApp.StartEngine();
            _mirApp.Start();
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("GameSvr is stopping.");

            //todo 需要立即保存所有玩家数据，防止数据不对

            if (M2Share.UserEngine.PlayObjectCount > 0)
            {
                Console.WriteLine("开始玩家数据保存");
                foreach (var item in M2Share.UserEngine.PlayObjects)
                {
                    M2Share.UserEngine.SaveHumanRcd(item);
                }
                Console.WriteLine("玩家数据保存完毕.");
            }

            //如果有多机负载转移在线玩家到新服务器
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
                                    playObject.ChangeSnapsServer(sIPaddr, nPort);
                                }
                            }
                            break;
                        }
                    }
                }, cancellationToken);
            }

            M2Share.boStartReady = false;
            _mirApp.Stop();

            return base.StopAsync(cancellationToken);
        }

        private void ServiceTimer(object obj)
        {
            if ((HUtil32.GetTickCount() - _checkIntervalTime) > 3000) //3s一次检查链接
            {
                M2Share.DataServer.CheckConnected();
                IdSrvClient.Instance.CheckConnected();
                InterMsgClient.Instance.CheckConnected();
                _checkIntervalTime = HUtil32.GetTickCount();
            }
            if ((HUtil32.GetTickCount() - _saveIntervalTime) > 50000) //保存游戏变量等
            {
                _mirApp.SaveItemNumber();
                _saveIntervalTime = HUtil32.GetTickCount();
            }
            if ((HUtil32.GetTickCount() - _clearIntervalTime) > 60000)
            {
                M2Share.ObjectSystem.ClearGhost();
                _clearIntervalTime = HUtil32.GetTickCount();
            }
        }
    }
}