using NLog;

namespace GameSrv.World.Threads
{
    public class StorageProcessor : TimerScheduledService
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public StorageProcessor() : base(TimeSpan.FromMilliseconds(20), "PlayerDataProcessor")
        {

        }

        protected override Task ExecuteInternal(CancellationToken stoppingToken)
        {
            const string sExceptionMsg = "[Exception] StorageProcessor::ExecuteInternal";
            try
            {
                M2Share.FrontEngine.ProcessGameDate();
                M2Share.FrontEngine.GetGameTime();
            }
            catch (Exception ex)
            {
                M2Share.Logger.Error(sExceptionMsg);
                M2Share.Logger.Error(ex.StackTrace);
            }
            return Task.CompletedTask;
        }

        protected override void Startup(CancellationToken stoppingToken)
        {
            _logger.Info("人物数据引擎启动成功...");
        }

        protected override void Stopping(CancellationToken stoppingToken)
        {
            _logger.Info("人物数据引擎停止...");
        }
    }
}