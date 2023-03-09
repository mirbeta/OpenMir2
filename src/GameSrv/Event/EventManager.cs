using GameSrv.Maps;
using NLog;

namespace GameSrv.Event
{
    public class EventManager
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IList<EventInfo> _eventList;
        private readonly IList<EventInfo> _closedEventList;
        private readonly Thread _worldEventThread;

        public EventManager()
        {
            _eventList = new List<EventInfo>();
            _closedEventList = new List<EventInfo>();
            _worldEventThread = new Thread(Run) { IsBackground = true };
        }

        public void Start()
        {
            _worldEventThread.Start();
            _logger.Info("事件管理线程初始化完成...");
        }

        public void Stop() {
            _worldEventThread.Interrupt();
            _logger.Info("事件管理线程停止ֹ...");
        }

        private void Run()
        {
            while (M2Share.StartReady)
            {
                EventInfo executeEvent;
                for (int i = _eventList.Count - 1; i >= 0; i--)
                {
                    executeEvent = _eventList[i];
                    if (executeEvent.Active && (HUtil32.GetTickCount() - executeEvent.RunStart) > executeEvent.RunTick)
                    {
                        executeEvent.RunStart = HUtil32.GetTickCount();
                        executeEvent.Run();
                        if (executeEvent.Closed)
                        {
                            _closedEventList.Add(executeEvent);
                            _eventList.RemoveAt(i);
                        }
                    }
                }

                for (int i = _closedEventList.Count - 1; i >= 0; i--)
                {
                    executeEvent = _closedEventList[i];
                    if ((HUtil32.GetTickCount() - executeEvent.CloseTick) > 5 * 60 * 1000)
                    {
                        _closedEventList[i] = null;
                        _closedEventList.RemoveAt(i);
                    }
                }

                Thread.SpinWait(20);
            }
        }

        public EventInfo GetEvent(Envirnoment Envir, int nX, int nY, int nType)
        {
            for (int i = _eventList.Count - 1; i >= 0; i--)
            {
                EventInfo currentEvent = _eventList[i];
                if (currentEvent.EventType == nType)
                {
                    if (currentEvent.Envir == Envir && currentEvent.nX == nX && currentEvent.nY == nY)
                    {
                        return currentEvent;
                    }
                }
            }
            return null;
        }

        public void AddEvent(EventInfo @event)
        {
            _eventList.Add(@event);
        }
    }
}