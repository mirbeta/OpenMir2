
namespace GameSrv.World.Threads
{
    public class MerchantProcessor : TimerScheduledService
    {
        public MerchantProcessor() : base(TimeSpan.FromMilliseconds(200), "MerchantProcessor")
        {

        }

        protected override Task ExecuteInternal(CancellationToken stoppingToken)
        {
            try
            {
                M2Share.WorldEngine.ProcessNpcs();
                M2Share.WorldEngine.ProcessMerchants();
            }
            catch (Exception ex)
            {
                M2Share.Logger.Error("[异常] MerchantProcessor::OnElapseAsync error");
                M2Share.Logger.Error(ex);
            }
            return Task.CompletedTask;
        }
    }
}