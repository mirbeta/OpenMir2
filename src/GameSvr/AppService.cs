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

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (M2Share.boStartReady)
            {
                M2Share.GateManager.Start();
                await M2Share.GateManager.StartMessageQueue(stoppingToken);
            }
            while (!stoppingToken.IsCancellationRequested)
            {
                _mirApp.Run();
                await Task.Delay(TimeSpan.FromMilliseconds(10), stoppingToken);
            }
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            M2Share.MainOutMessage("正在读取配置信息...");
            _mirApp.InitializeServer();
            M2Share.MainOutMessage("读取配置信息完成...");
            _mirApp.Initialize();
            _mirApp.StartEngine();
            _mirApp.StartService();
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
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
            var sIPaddr = string.Empty;
            var nPort = 0;

            var isMultiServer = M2Share.GetMultiServerAddrPort(0, ref sIPaddr, ref nPort); //如果有可用服务器，那就切换过去
            if (isMultiServer)
            {
                var playerCount = M2Share.UserEngine.PlayObjects.Count();
                if (playerCount > 0)
                {
                    Task.Run(() =>
                    {
                        var shutdownSeconds = 120;
                        while (playerCount > 0)
                        {
                            foreach (var playObject in M2Share.UserEngine.PlayObjects)
                            {
                                var closeStr = $"服务器关闭倒计时 [{shutdownSeconds}].";
                                playObject.SysMsg(closeStr, MsgColor.Red, MsgType.Notice);
                                Console.WriteLine(closeStr);
                                shutdownSeconds--;
                            }
                            if (shutdownSeconds > 0)
                            {
                                Thread.Sleep(TimeSpan.FromSeconds(1));
                            }
                            else
                            {
                                foreach (var playObject in M2Share.UserEngine.PlayObjects)
                                {
                                    if (playObject.m_boGhost || playObject.m_boDeath) //死亡或者下线的玩家不进行转移
                                    {
                                        playerCount--;
                                        continue;
                                    }
                                    playObject.ChangeSnapsServer(sIPaddr, nPort);
                                    playerCount--;
                                }
                                break;
                            }
                        }
                        M2Share.boStartReady = false;
                        _mirApp.Stop();
                    }, cancellationToken);
                }
            }
            else
            {
                M2Share.boStartReady = false;
                _mirApp.Stop();
            }
            return base.StopAsync(cancellationToken);
        }
    }
}