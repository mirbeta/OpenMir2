using M2Server;
using NLog;
using OpenMir2;
using SystemModule;

namespace GameSrv.Word.Threads
{
    public class ActorBuffProcessor: TimerScheduledService
    {
        

        public ActorBuffProcessor() : base(TimeSpan.FromMilliseconds(50), "ActorBuffProcessor")
        {
            
        }

        public override void Initialize(CancellationToken cancellationToken)
        {
            LogService.Info("技能Buff处理器初始化...");
        }

        protected override Task ExecuteInternal(CancellationToken cancellationToken)
        {
            try
            {
                M2Share.ActorBuffSystem.DoWork(null);
            }
            catch (Exception e)
            {
                LogService.Error(e.StackTrace);
            }
            return Task.CompletedTask;
        }

        protected override void Startup(CancellationToken cancellationToken)
        {
            LogService.Info("技能Buff处理器启动...");
        }

        protected override void Stopping(CancellationToken cancellationToken)
        {
            LogService.Info("技能Buff处理器停止...");
        }
    }
}