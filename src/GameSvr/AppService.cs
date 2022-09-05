using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SystemModule.Data;

namespace GameSvr
{
    public class AppService : BackgroundService
    {
        private readonly ILogger<AppService> _logger;
        private readonly GameApp _mirApp;
        private readonly TimeSpan DelayTime = TimeSpan.FromMilliseconds(10);

        public AppService(GameApp serverApp, ILogger<AppService> logger)
        {
            _mirApp = serverApp;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (M2Share.boStartReady)
            {
                M2Share.GateMgr.Start(stoppingToken);
            }
            while (!stoppingToken.IsCancellationRequested)
            {
                _mirApp.Run();
                await Task.Delay(DelayTime, stoppingToken);
            }
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("正在读取配置信息...");
            _mirApp.InitializeServer();
            _logger.LogInformation("读取配置信息完成...");
            _mirApp.Initialize();
            _mirApp.StartEngine();
            _mirApp.StartService();
            return base.StartAsync(cancellationToken);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            if (M2Share.UserEngine.PlayObjectCount > 0)
            {
                _logger.LogDebug("开始玩家数据保存");
                foreach (var item in M2Share.UserEngine.PlayObjects)
                {
                    M2Share.UserEngine.SaveHumanRcd(item);
                }
                _logger.LogDebug("玩家数据保存完毕.");
            }

            //如果有多机负载转移在线玩家到新服务器
            var sIPaddr = string.Empty;
            var nPort = 0;
            var isMultiServer = M2Share.GetMultiServerAddrPort(M2Share.ServerIndex, ref sIPaddr, ref nPort); //如果有可用服务器，那就切换过去
            if (isMultiServer)
            {
                var playerCount = M2Share.UserEngine.PlayObjects.Count();
                if (playerCount > 0)
                {
                    await Task.Run(async () =>
                    {
                        var shutdownSeconds = 120;
                        while (playerCount > 0)
                        {
                            foreach (var playObject in M2Share.UserEngine.PlayObjects)
                            {
                                var closeStr = $"服务器关闭倒计时 [{shutdownSeconds}].";
                                playObject.SysMsg(closeStr, MsgColor.Red, MsgType.Notice);
                                _logger.LogInformation(closeStr);
                                shutdownSeconds--;
                            }
                            if (shutdownSeconds > 0)
                            {
                                await Task.Delay(TimeSpan.FromSeconds(1));
                            }
                            else
                            {
                                foreach (var playObject in M2Share.UserEngine.PlayObjects)
                                {
                                    if (playObject.Ghost || playObject.Death)//死亡或者下线的玩家不进行转移
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
        }
    }
}