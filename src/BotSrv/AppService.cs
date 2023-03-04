using System;
using System.Threading;
using System.Threading.Tasks;
using BotSrv.Player;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using NLog;
using SystemModule;

namespace BotSrv
{
    public class AppService : IHostedService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly RobotOptions _options;
        /// <summary>
        /// 登录序号
        /// </summary>
        private static int g_nLoginIndex;
        /// <summary>
        /// 登录间隔
        /// </summary>
        private static long g_dwLogonTick;
        private readonly Thread runThread;

        public AppService(IOptions<RobotOptions> options, ClientManager clientManager)
        {
            BotShare.ClientMgr = clientManager;
            _options = options.Value;
            _options.ChrCount = HUtil32._MIN(_options.ChrCount, _options.TotalChrCount);
            g_dwLogonTick = HUtil32.GetTickCount() - 1000 * _options.ChrCount;
            MShare.g_sGameIPaddr = _options.Address;
            MShare.g_nGamePort = _options.Port;
            runThread = new Thread(Run) { IsBackground = true };
            logger.Info("服务器名称:{0} 服务器IP:{1} 服务器端口:{2}", _options.ServerName, _options.Address, _options.Port);
            logger.Info("初始化机器人数量:{0} 登录机器人总数量:{1}", _options.ChrCount, _options.TotalChrCount);
        }

        public async Task StartAsync(CancellationToken stoppingToken)
        {
            logger.Info("机器人服务启动...");
            runThread.Start();
            await BotShare.ClientMgr.Start(stoppingToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.Info("机器人服务停止...");
            BotShare.ClientMgr.Stop(cancellationToken);
            return Task.CompletedTask;
        }

        private void Run()
        {
            logger.Debug("工作线程开始执行...");
            while (true)
            {
                if (_options.TotalChrCount > 0)
                {
                    if (((HUtil32.GetTickCount() - g_dwLogonTick) > 1000 * _options.ChrCount))
                    {
                        g_dwLogonTick = HUtil32.GetTickCount();
                        if (_options.TotalChrCount >= _options.ChrCount)
                        {
                            _options.TotalChrCount -= _options.ChrCount;
                        }
                        else
                        {
                            _options.TotalChrCount = 0;
                        }
                        for (var i = 0; i < _options.ChrCount; i++)
                        {
                            var playClient = new RobotPlayer();
                            playClient.NewAccount = _options.NewAccount;
                            playClient.LoginID = string.Concat(_options.LoginAccount, g_nLoginIndex);
                            playClient.LoginPasswd = playClient.LoginID;
                            playClient.ChrName = playClient.LoginID;
                            playClient.ConnectTick = HUtil32.GetTickCount() + (i + 1) * 3000;
                            BotShare.ClientMgr.AddClient(playClient.SessionId, playClient);
                            g_nLoginIndex++;
                        }
                    }
                }
                BotShare.ClientMgr.Run();
                Thread.Sleep(TimeSpan.FromMilliseconds(50));
            }
        }
    }
}
