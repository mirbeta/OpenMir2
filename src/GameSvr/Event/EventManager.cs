using GameSvr.Maps;
using SystemModule;

namespace GameSvr.Event
{
    public class EventManager
    {
        private readonly IList<MirEvent> _eventList = null;
        private readonly IList<MirEvent> _closedEventList = null;
        
        public EventManager()
        {
            _eventList = new List<MirEvent>();
            _closedEventList = new List<MirEvent>();
        }
        
        public void Run()
        {
            MirEvent executeEvent;
            for (var i = _eventList.Count - 1; i >= 0; i--)
            {
                executeEvent = _eventList[i];
                if (executeEvent.Active && (HUtil32.GetTickCount() - executeEvent.m_dwRunStart) > executeEvent.m_dwRunTick)
                {
                    executeEvent.m_dwRunStart = HUtil32.GetTickCount();
                    executeEvent.Run();
                    if (executeEvent.Closed)
                    {
                        _closedEventList.Add(executeEvent);
                        _eventList.RemoveAt(i);
                    }
                }
            }

            for (var i = _closedEventList.Count - 1; i >= 0; i--)
            {
                executeEvent = _closedEventList[i];
                if ((HUtil32.GetTickCount() - executeEvent.CloseTick) > 5 * 60 * 1000)
                {
                    _closedEventList.RemoveAt(i);
                    executeEvent = null;
                }
            }
        }

        public MirEvent GetEvent(Envirnoment Envir, int nX, int nY, int nType)
        {
            for (var i = _eventList.Count - 1; i >= 0; i--)
            {
                MirEvent currentEvent = _eventList[i];
                if (currentEvent.EventType == nType)
                {
                    if (currentEvent.m_Envir == Envir && currentEvent.m_nX == nX && currentEvent.m_nY == nY)
                    {
                        return currentEvent;
                    }
                }
            }
            return null;
        }

        public void AddEvent(MirEvent @event)
        {
            _eventList.Add(@event);
        }
    }
}