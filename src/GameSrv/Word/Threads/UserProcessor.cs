using NLog;
using SystemModule;

namespace M2Server.World.Threads
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
                M2Share.WorldEngine.ProcessHumans();
            }
            catch (Exception ex)
            {
                M2Share.Logger.Error("[Exception] UserProcessor::ExecuteInternal");
                M2Share.Logger.Error(ex);
            }
            return Task.CompletedTask;
        }
    }
}