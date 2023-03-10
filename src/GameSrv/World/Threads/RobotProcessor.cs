using NLog;

namespace GameSrv.World.Threads
{
    public class RobotProcessor : TimerBase
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        public int ProcessedMonsters { get; private set; }

        public RobotProcessor() : base(500, "RobotProcessor")
        {

        }

        

        protected override bool OnElapseAsync()
        {
            try
            {
                ProcessedMonsters = 0;
                M2Share.WorldEngine.ProcessRobotPlayData();
                /*foreach (var map in Kernel.MapManager.GameMaps.Values)
                    ProcessedMonsters += await map.OnTimerAsync();
                await Kernel.RoleManager.OnRoleTimerAsync();*/
            }
            catch (Exception ex)
            {
                logger.Error($"RobotProcessor::OnElapseAsync error");
                logger.Error(ex);
            }

            return true;
        }
    }
}