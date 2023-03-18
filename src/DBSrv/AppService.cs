using System.Threading;
using System.Threading.Tasks;
using DBSrv.Services;
using Microsoft.Extensions.Hosting;
using NLog;

namespace DBSrv
{
    public class AppService : BackgroundService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly UserService _userSocService;
        private readonly LoginSessionService _loginSvrService;
        private readonly PlayerDataService _dataService;

        public AppService( UserService userSoc, LoginSessionService loginSession, PlayerDataService dataService)
        {
            _userSocService = userSoc;
            _loginSvrService = loginSession;
            _dataService = dataService;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.Register(() => logger.Debug("DBSrv is stopping."));
            _userSocService.Start(stoppingToken);
            _loginSvrService.Start();
            _dataService.Start();
            return Task.CompletedTask;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            logger.Debug("DBSrv is starting.");
            DBShare.LoadConfig();
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            logger.Debug("DBSrv is stopping.");
            _userSocService.Stop();
            return base.StopAsync(cancellationToken);
        }
    }
}