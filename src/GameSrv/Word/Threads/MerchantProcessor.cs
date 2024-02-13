using M2Server;
using NLog;
using OpenMir2;
using SystemModule;

namespace GameSrv.Word.Threads
{
    public class MerchantProcessor : TimerScheduledService
    {
        

        public MerchantProcessor() : base(TimeSpan.FromMilliseconds(200), "MerchantProcessor")
        {

        }

        public override void Initialize(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        protected override void Startup(CancellationToken stoppingToken)
        {
            LogService.Info("商人管理线程初始化完成...");
        }

        protected override void Stopping(CancellationToken stoppingToken)
        {
            LogService.Info("商人管理线程停止ֹ...");
        }

        protected override Task ExecuteInternal(CancellationToken stoppingToken)
        {
            try
            {
                SystemShare.WorldEngine.ProcessNpcs();
                SystemShare.WorldEngine.ProcessMerchants();
            }
            catch (Exception ex)
            {
                LogService.Error("[异常] MerchantProcessor::OnElapseAsync error");
                LogService.Error(ex);
            }
            return Task.CompletedTask;
        }
    }
}