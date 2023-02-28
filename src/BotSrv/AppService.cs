using System;
using System.Threading;
using System.Threading.Tasks;
using BotSrv.Player;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SystemModule;

namespace BotSrv
{
    public class AppService : BackgroundService
    {
        private readonly RobotOptions _options;
        /// <summary>
        /// 登录序号
        /// </summary>
        private static int g_nLoginIndex = 0;
        /// <summary>
        /// 登录间隔
        /// </summary>
        private static long g_dwLogonTick = 0;

        public AppService(IOptions<RobotOptions> options, ClientManager clientManager)
        {
            BotShare.ClientMgr = clientManager;
            _options = options.Value;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            MShare.g_sGameIPaddr = _options.Address;
            MShare.g_nGamePort = _options.Port;
            BotShare.ClientMgr.Start(stoppingToken);
            Start();
            return Task.CompletedTask;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _options.ChrCount = HUtil32._MIN(_options.ChrCount, _options.TotalChrCount);
            g_dwLogonTick = HUtil32.GetTickCount() - 1000 * _options.ChrCount;
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            BotShare.ClientMgr.Stop(cancellationToken);
            return base.StopAsync(cancellationToken);
        }

        private void Start()
        {
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
                            playClient.ServerName = _options.ServerName;
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
