namespace GameSrv.Word.Threads
{
    public class TimedRobotProcessor : TimerScheduledService
    {


        public TimedRobotProcessor() : base(TimeSpan.FromMilliseconds(20), "TimedRobotProcessor")
        {

        }

        public override void Initialize(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        protected override void Startup(CancellationToken stoppingToken)
        {
            LogService.Info("脚本机器人线程启动...");
        }

        protected override void Stopping(CancellationToken stoppingToken)
        {
            LogService.Info("脚本机器人线程停止...");
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
                LogService.Error(sExceptionMsg);
                LogService.Error(e.Message);
            }
            return Task.CompletedTask;
        }
    }
}