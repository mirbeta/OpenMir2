namespace GameSrv.Word.Threads
{
    public class UserProcessor : TimerScheduledService
    {


        public UserProcessor() : base(TimeSpan.FromMilliseconds(50), "UserProcessor")
        {

        }

        public override void Initialize(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        protected override void Startup(CancellationToken stoppingToken)
        {
            LogService.Info("玩家管理线程初始化完成...");
        }

        protected override void Stopping(CancellationToken stoppingToken)
        {
            LogService.Info("玩家管理线程停止ֹ...");
        }

        protected override Task ExecuteInternal(CancellationToken stoppingToken)
        {
            try
            {
                SystemShare.WorldEngine.ProcessHumans();
            }
            catch (Exception ex)
            {
                LogService.Error("[Exception] UserProcessor::ExecuteInternal");
                LogService.Error(ex);
            }
            return Task.CompletedTask;
        }
    }
}