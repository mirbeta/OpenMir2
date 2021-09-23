using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DBSvr
{
    public class AppService : BackgroundService
    {
        private readonly ILogger<AppService> _logger;
        private readonly ServerApp _serverApp;
        private readonly UserSocService _userSoc;
        private readonly LoginSocService _LoginSoc;
        private readonly HumDataService _dataService;
        private Timer _threadTimer;

        public AppService(ILogger<AppService> logger, ServerApp serverApp, UserSocService userSoc, LoginSocService idSoc, HumDataService dataService)
        {
            _logger = logger;
            _serverApp = serverApp;
            _userSoc = userSoc;
            _LoginSoc = idSoc;
            _dataService = dataService;
            _threadTimer = new Timer(ThreadServerTimer, null, 1000, 5000);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.Register(() => _logger.LogDebug($"DBSvr is stopping."));
            _serverApp.Start();
            _userSoc.Start();
            _LoginSoc.Start();
            _dataService.Start();
            return Task.CompletedTask;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug($"DBSvr is starting.");
            _serverApp.StartService();
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug($"DBSvr is stopping.");
            _serverApp.StopService();
            return base.StopAsync(cancellationToken);
        }

        private void ThreadServerTimer(object obj)
        {
            var userCount = _userSoc.GetUserCount();
            _LoginSoc.SendKeepAlivePacket(userCount);
            _LoginSoc.CheckConnection();
            _dataService.ClearTimeoutSession();
            //ServerState(userCount);
        }
        
        private void ServerState(int userCount)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"UserCount:{userCount}");
            sb.AppendLine(DBShare.g_nClearIndex + "/(" + DBShare.g_nClearCount + "/" + DBShare.g_nClearItemIndexCount + ")/" + DBShare.g_nClearRecordCount);
            sb.AppendLine($"H-QyChr:{DBShare.g_nQueryChrCount} H-NwChr:{DBShare.nHackerNewChrCount} H-DlChr:{DBShare.nHackerDelChrCount} Dubb -Sl:{DBShare.nHackerSelChrCount}");
            sb.AppendLine($"H-Er-P1:{DBShare.n4ADC1C} Dubl-P2:{DBShare.n4ADC20} Dubl-P3:{DBShare.n4ADC24} Dubl-P4:{DBShare.n4ADC28}");
            DBShare.OutMainMessage(sb.ToString());
        }
    }
}