using M2Server;
using NLog;

namespace GameSrv.Word.Threads
{
    public class RobotProcessor : TimerScheduledService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        public int ProcessedMonsters { get; private set; }

        public RobotProcessor() : base(TimeSpan.FromMilliseconds(100), "RobotProcessor")
        {

        }

        public override void Initialize(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        protected override void Startup(CancellationToken stoppingToken)
        {
            logger.Info("机器人管理线程初始化完成...");
        }

        protected override void Stopping(CancellationToken stoppingToken)
        {
            logger.Info("机器人管理线程停止ֹ...");
        }

        protected override Task ExecuteInternal(CancellationToken stoppingToken)
        {
            try
            {
                ProcessedMonsters = 0;
                M2Share.WorldEngine.ProcessRobotPlayData();
                /*foreach (var map in Kernel.MapMgr.GameMaps.Values)
                    ProcessedMonsters += await map.OnTimerAsync();
                await Kernel.RoleManager.OnRoleTimerAsync();*/
            }
            catch (Exception ex)
            {
                M2Share.Logger.Error("[Exception] RobotProcessor::ExecuteInternal");
                logger.Error(ex);
            }
            return Task.CompletedTask;
        }
    }
}