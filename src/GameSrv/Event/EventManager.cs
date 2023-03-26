using GameSrv.Maps;
using NLog;

namespace GameSrv.Event
{
    public class EventManager
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IList<EventInfo> _eventList;
        private readonly IList<EventInfo> _closedEventList;

        public EventManager()
        {
            _eventList = new List<EventInfo>();
            _closedEventList = new List<EventInfo>();
        }

        public IList<EventInfo> Events => _eventList;
        public IList<EventInfo> ClosedEvents => _closedEventList;
        
        public EventInfo GetEvent(Envirnoment envir, int nX, int nY, int nType)
        {
            for (int i = _eventList.Count - 1; i >= 0; i--)
            {
                EventInfo currentEvent = _eventList[i];
                if (currentEvent.EventType == nType)
                {
                    if (currentEvent.Envir == envir && currentEvent.nX == nX && currentEvent.nY == nY)
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