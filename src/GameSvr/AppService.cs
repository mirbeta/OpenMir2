using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SystemModule.Data;

namespace GameSvr
{
    public class AppService : IHostedService, IDisposable
    {
        private readonly ILogger<AppService> _logger;
        private readonly GameApp _mirApp;
        private readonly TimeSpan DelayTime = TimeSpan.FromMilliseconds(10);
        private Timer _timer;

        public AppService(GameApp serverApp, ILogger<AppService> logger)
        {
            _mirApp = serverApp;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("正在读取配置信息...");
            _mirApp.Initialize();
            _logger.LogInformation("读取配置信息完成...");
            _mirApp.StartEngine();
            _mirApp.StartService();
            if (M2Share.boStartReady)
            {
                M2Share.GateMgr.Start(stoppingToken);
            }
            _timer = new Timer(DoWork, null, TimeSpan.Zero, DelayTime);
            return Task.CompletedTask;
        }

        private void DoWork(object state)
        {
            _mirApp.Run();
        }

        public async Task StopAsync(CancellationToken stoppingToken)
        {
            if (M2Share.UserEngine.PlayObjectCount > 0)
            {
                _logger.LogInformation("保存玩家数据");
                foreach (var item in M2Share.UserEngine.PlayObjects)
                {
                    M2Share.UserEngine.SaveHumanRcd(item);
                }
                _logger.LogInformation("数据保存完毕.");
            }

            _logger.LogInformation("检查是否有其他可用服务器.");
            //如果有多机负载转移在线玩家到新服务器
            var sIPaddr = string.Empty;
            var nPort = 0;
            var isMultiServer = M2Share.GetMultiServerAddrPort(M2Share.ServerIndex, ref sIPaddr, ref nPort); //如果有可用服务器，那就切换过去
            if (isMultiServer)
            {
                _logger.LogInformation($"转移到新服务器[{sIPaddr}:{nPort}]");
                var playerCount = M2Share.UserEngine.PlayObjects.Count();
                if (playerCount > 0)
                {
                    await Task.Factory.StartNew(async () =>
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
                    }, stoppingToken);
                }
            }
            else
            {
                _logger.LogInformation("没有可用服务器，即将关闭游戏服务器.");
                M2Share.boStartReady = false;
                _mirApp.Stop();
            }
            _timer?.Change(Timeout.Infinite, 0);
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}