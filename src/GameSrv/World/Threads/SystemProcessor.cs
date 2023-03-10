namespace GameSrv.World.Threads
{
    public class SystemProcessor : TimerBase
    {

        public SystemProcessor() : base(20, "SystemThread")
        {

        }
    }
}