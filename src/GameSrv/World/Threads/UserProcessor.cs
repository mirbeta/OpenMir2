using NLog;

namespace GameSrv.World.Threads
{
    public class UserProcessor : TimerScheduledService
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public UserProcessor() : base(TimeSpan.FromMilliseconds(50), "UserProcessor")
        {

        }

        public override void Initialize(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        
        protected override void Startup(CancellationToken stoppingToken)
        {
            _logger.Info("玩家管理线程初始化完成...");
        }

        protected override void Stopping(CancellationToken stoppingToken)
        {
            _logger.Info("玩家管理线程停止ֹ...");
        }

        protected override Task ExecuteInternal(CancellationToken stoppingToken)
        {
            try
            {
                GameShare.WorldEngine.ProcessHumans();
            }
            catch (Exception ex)
            {
                GameShare.Logger.Error("[Exception] UserProcessor::ExecuteInternal");
                GameShare.Logger.Error(ex);
            }
            return Task.CompletedTask;
        }
    }
}