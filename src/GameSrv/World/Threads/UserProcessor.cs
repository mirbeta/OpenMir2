namespace GameSrv.World.Threads
{
    public class UserProcessor : TimerBase
    {

        public UserProcessor() : base(100, "UserProcessor")
        {
            
        }
    }
}