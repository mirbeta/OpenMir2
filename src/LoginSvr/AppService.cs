using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LoginSvr
{
    public class AppService : BackgroundService
    {
        private readonly ILogger<AppService> _logger;
        private readonly AppServer _serverApp;
        private readonly MasSocService _masSoc;
        private readonly MonSocService _monSoc;
        private readonly LoginService _loginService;
        private readonly Thread _appThread;
        private Timer _monitorTimer;

        public AppService(ILogger<AppService> logger, AppServer serverApp, MasSocService masSoc, MonSocService monSoc, LoginService loginService)
        {
            _logger = logger;
            _serverApp = serverApp;
            _masSoc = masSoc;
            _monSoc = monSoc;
            _loginService = loginService;
            _appThread = new Thread(LoginProcessThread);
            _appThread.IsBackground = true;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.Register(() => _logger.LogDebug($"GameGate is stopping."));
            _serverApp.Start();
            _masSoc.Start();
            _monSoc.Start();
            _appThread.Start();
            _monitorTimer = new Timer(_serverApp.MonitorTimer, null, 0, 5000);
            return Task.CompletedTask;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug($"GameGate is starting.");
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug($"GameGate is stopping.");
            //_serverApp.StopService();
            return base.StopAsync(cancellationToken);
        }
        
        private void LoginProcessThread(object obj)
        {
            while (true)
            {
                TConfig Config = LSShare.g_Config;
                for (var i = 0; i < LSShare.g_MainMsgList.Count; i++)
                {
                    Console.WriteLine(LSShare.g_MainMsgList[i]);
                }
                LSShare.g_MainMsgList.Clear();
                _loginService.ProceDataTimer();
                _loginService.SessionClearKick(Config);
                _loginService.SessionClearNoPayMent(Config);
                Thread.Sleep(1);
            }
        }
       
    }
}
