using NLog;

namespace GameSrv.World.Threads
{
    public class MerchantProcessor : TimerScheduledService
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public MerchantProcessor() : base(TimeSpan.FromMilliseconds(200), "MerchantProcessor")
        {

        }

        public override void Initialize(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        protected override void Startup(CancellationToken stoppingToken)
        {
            _logger.Info("商人管理线程初始化完成...");
        }

        protected override void Stopping(CancellationToken stoppingToken)
        {
            _logger.Info("商人管理线程停止ֹ...");
        }
        
        protected override Task ExecuteInternal(CancellationToken stoppingToken)
        {
            try
            {
                GameShare.WorldEngine.ProcessNpcs();
                GameShare.WorldEngine.ProcessMerchants();
            }
            catch (Exception ex)
            {
                GameShare.Logger.Error("[异常] MerchantProcessor::OnElapseAsync error");
                GameShare.Logger.Error(ex);
            }
            return Task.CompletedTask;
        }
    }
}