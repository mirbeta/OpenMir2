
namespace GameSrv.World.Threads
{
    public class MerchantProcessor : TimerBase
    {
        public MerchantProcessor() : base(200, "MerchantProcessor")
        {

        }

        protected override void OnStartAsync()
        {
            Console.Write("启动");
            base.OnStartAsync();
        }

        protected override bool OnElapseAsync()
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

            return true;
        }
    }
}