using SystemModule.MagicEvent;

namespace SystemModule
{
    public interface IEventSystem
    {
        IList<MapEvent> ClosedEvents { get; }

        IList<MapEvent> Events { get; }

        void AddEvent(MapEvent @event);

        MapEvent GetEvent(IEnvirnoment envir, int nX, int nY, int nType);
    }
}
