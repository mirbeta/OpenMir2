using M2Server;
using NLog;

namespace GameSrv.Word.Threads
{
    public class EventProcessor : TimerScheduledService
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public EventProcessor() : base(TimeSpan.FromMilliseconds(20), "EventProcessor")
        {

        }

        public override void Initialize(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        protected override void Startup(CancellationToken stoppingToken)
        {
            _logger.Info("事件管理线程初始化完成...");
        }

        protected override void Stopping(CancellationToken stoppingToken)
        {
            _logger.Info("事件管理线程停止ֹ...");
        }

        protected override Task ExecuteInternal(CancellationToken stoppingToken)
        {
            var eventList = SystemShare.EventMgr.Events;
            var closedEventList = SystemShare.EventMgr.ClosedEvents;
            for (var i = eventList.Count - 1; i >= 0; i--)
            {
                var executeEvent = eventList[i];
                if (executeEvent == null)
                {
                    continue;
                }
                if (executeEvent.Active && (HUtil32.GetTickCount() - executeEvent.RunStart) > executeEvent.RunTick)
                {
                    executeEvent.RunStart = HUtil32.GetTickCount();
                    executeEvent.Run();
                    if (executeEvent.Closed)
                    {
                        closedEventList.Add(executeEvent);
                        eventList.RemoveAt(i);
                    }
                }
            }

            for (var i = closedEventList.Count - 1; i >= 0; i--)
            {
                var executeEvent = closedEventList[i];
                if ((HUtil32.GetTickCount() - executeEvent.CloseTick) > 5 * 60 * 1000)
                {
                    closedEventList[i] = null;
                    closedEventList.RemoveAt(i);
                }
            }
            return Task.CompletedTask;
        }
    }
}