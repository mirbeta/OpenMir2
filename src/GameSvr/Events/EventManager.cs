using SystemModule;

namespace GameSvr
{
    public class EventManager
    {
        private readonly IList<Event> _eventList = null;
        private readonly IList<Event> _closedEventList = null;

        public void Run()
        {
            Event executeEvent;
            for (var i = _eventList.Count - 1; i >= 0; i--)
            {
                executeEvent = _eventList[i];
                if (executeEvent.m_boActive && (HUtil32.GetTickCount() - executeEvent.m_dwRunStart) > executeEvent.m_dwRunTick)
                {
                    executeEvent.m_dwRunStart = HUtil32.GetTickCount();
                    executeEvent.Run();
                    if (executeEvent.m_boClosed)
                    {
                        _closedEventList.Add(executeEvent);
                        _eventList.RemoveAt(i);
                    }
                }
            }

            for (var i = _closedEventList.Count - 1; i >= 0; i--)
            {
                executeEvent = _closedEventList[i];
                if ((HUtil32.GetTickCount() - executeEvent.m_dwCloseTick) > 5 * 60 * 1000)
                {
                    _closedEventList.RemoveAt(i);
                    executeEvent = null;
                }
            }
        }

        public Event GetEvent(Envirnoment Envir, int nX, int nY, int nType)
        {
            Event result = null;
            for (var i = _eventList.Count - 1; i >= 0; i--)
            {
                Event currentEvent = _eventList[i];
                if (currentEvent.m_nEventType == nType)
                {
                    if (currentEvent.m_Envir == Envir && currentEvent.m_nX == nX && currentEvent.m_nY == nY)
                    {
                        result = currentEvent;
                        break;
                    }
                }
            }
            return result;
        }

        public void AddEvent(Event @event)
        {
            _eventList.Add(@event);
        }

        public EventManager()
        {
            _eventList = new List<Event>();
            _closedEventList = new List<Event>();
        }
    }
}