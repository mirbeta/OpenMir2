namespace GameSrv.World.Threads
{
    public class UserProcessor : TimerScheduledService
    {
        public UserProcessor() : base(TimeSpan.FromMilliseconds(50), "UserProcessor")
        {

        }

        protected override Task ExecuteInternal(CancellationToken stoppingToken)
        {
            try
            {
                M2Share.WorldEngine.ProcessHumans();
            }
            catch (Exception ex)
            {
                M2Share.Logger.Error("[�쳣] UserProcessor::OnElapseAsync error");
                M2Share.Logger.Error(ex);
            }
            return Task.CompletedTask;
        }
    }
}