using NLog;

namespace GameSrv.Word.Threads
{
    public class TimedRobotProcessor : TimerScheduledService
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public TimedRobotProcessor() : base(TimeSpan.FromMilliseconds(20), "TimedRobotProcessor")
        {
            
        }
        
        public override void Initialize(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        
        protected override void Startup(CancellationToken stoppingToken)
        {
            _logger.Info("脚本机器人线程启动...");
        }

        protected override void Stopping(CancellationToken stoppingToken)
        {
            _logger.Info("脚本机器人线程停止...");
        }

        protected override Task ExecuteInternal(CancellationToken stoppingToken)
        {
            const string sExceptionMsg = "[Exception] TimedRobotProcessor::ExecuteInternal";
            try
            {
                //var robotHumanList = M2Share.RobotMgr.Robots;
                //for (var i = robotHumanList.Count - 1; i >= 0; i--)
                //{
                //    robotHumanList[i].Run();
                //}
            }
            catch (Exception e)
            {
                _logger.Error(sExceptionMsg);
                _logger.Error(e.Message);
            }
            return Task.CompletedTask;
        }
    }
}