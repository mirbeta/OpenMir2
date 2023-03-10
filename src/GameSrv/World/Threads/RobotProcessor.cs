using NLog;

namespace GameSrv.World.Threads
{
    public class RobotProcessor : TimerScheduledService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        public int ProcessedMonsters { get; private set; }

        public RobotProcessor() : base(TimeSpan.FromMilliseconds(100), "RobotProcessor")
        {

        }

        protected override Task ExecuteInternal(CancellationToken stoppingToken)
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
            return Task.CompletedTask;
        }

        protected override void Startup(CancellationToken stoppingToken)
        {
            logger.Info("机器人管理线程初始化完成...");
        }

        protected override void Stopping(CancellationToken stoppingToken)
        {
            logger.Info("机器人管理线程停止ֹ...");
        }
    }
}