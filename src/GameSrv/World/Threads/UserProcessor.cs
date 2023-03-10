namespace GameSrv.World.Threads
{
    public class UserProcessor : TimerBase
    {

        public UserProcessor() : base(100, "UserProcessor")
        {

        }

        protected override bool OnElapseAsync()
        {
            try
            {
                M2Share.WorldEngine.ProcessHumans();
            }
            catch (Exception ex)
            {
                M2Share.Logger.Error("[“Ï≥£] UserProcessor::OnElapseAsync error");
                M2Share.Logger.Error(ex);
            }
            return true;
        }
    }
}