using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DBSvr
{
    public class AppService : BackgroundService
    {
        private readonly ILogger<AppService> _logger;
        private readonly UserSocService _userSocService;
        private readonly LoginSvrService _loginSvrService;
        private readonly HumDataService _dataService;
        private readonly ConfigManager _configManager;

        public AppService(ILogger<AppService> logger,  UserSocService userSoc, LoginSvrService idSoc, HumDataService dataService, ConfigManager configManager)
        {
            _logger = logger;
            _userSocService = userSoc;
            _loginSvrService = idSoc;
            _dataService = dataService;
            _configManager = configManager;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.Register(() => _logger.LogDebug($"DBSvr is stopping."));
            _userSocService.Start();
            _loginSvrService.Start();
            _dataService.Start();
            await _userSocService.StartConsumer();
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug($"DBSvr is starting.");
            DBShare.Initialization();
            _configManager.LoadConfig();
            DBShare.LoadConfig();
            DBShare.MainOutMessage("服务器已启动...");
            return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug($"DBSvr is stopping.");
            return base.StopAsync(cancellationToken);
        }
    }
}