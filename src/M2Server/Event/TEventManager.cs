using System.Collections.Generic;

namespace M2Server
{
    public class EventManager
    {
        private readonly IList<TEvent> _eventList = null;
        private readonly IList<TEvent> _closedEventList = null;

        public void Run()
        {
            TEvent executeEvent;
            for (var i = _eventList.Count - 1; i >= 0; i--)
            {
                executeEvent = _eventList[i];
                if (executeEvent.m_boActive && HUtil32.GetTickCount() - executeEvent.m_dwRunStart > executeEvent.m_dwRunTick)
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
                if (HUtil32.GetTickCount() - executeEvent.m_dwCloseTick > 5 * 60 * 1000)
                {
                    _closedEventList.RemoveAt(i);
                    executeEvent = null;
                }
            }
        }

        public TEvent GetEvent(TEnvirnoment Envir, int nX, int nY, int nType)
        {
            TEvent result = null;
            TEvent currentEvent = null;
            for (var i = _eventList.Count - 1; i >= 0; i--)
            {
                currentEvent = _eventList[i];
                if (currentEvent.m_Envir == Envir && currentEvent.m_nX == nX && currentEvent.m_nY == nY &&
                    currentEvent.m_nEventType == nType)
                {
                    result = currentEvent;
                    break;
                }
            }
            return result;
        }

        public void AddEvent(TEvent @event)
        {
            _eventList.Add(@event);
        }

        public EventManager()
        {
            _eventList = new List<TEvent>();
            _closedEventList = new List<TEvent>();
        }
    }
}