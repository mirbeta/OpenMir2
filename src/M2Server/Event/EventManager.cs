using SystemModule;
using SystemModule.MagicEvent;
using SystemModule.Maps;
using SystemModule.SubSystem;

namespace M2Server.Event
{
    public class EventManager : IEventSystem
    {
        private readonly IList<MapEvent> _eventList;
        private readonly IList<MapEvent> _closedEventList;

        public EventManager()
        {
            _eventList = new List<MapEvent>();
            _closedEventList = new List<MapEvent>();
        }

        public IList<MapEvent> Events => _eventList;
        public IList<MapEvent> ClosedEvents => _closedEventList;

        public MapEvent GetEvent(IEnvirnoment envir, int nX, int nY, int nType)
        {
            for (int i = _eventList.Count - 1; i >= 0; i--)
            {
                MapEvent currentEvent = _eventList[i];
                if (currentEvent.EventType == nType)
                {
                    if (currentEvent.Envirnoment == envir && currentEvent.nX == nX && currentEvent.nY == nY)
                    {
                        return currentEvent;
                    }
                }
            }
            return null;
        }

        public void AddEvent(MapEvent @event)
        {
            _eventList.Add(@event);
        }
    }
}