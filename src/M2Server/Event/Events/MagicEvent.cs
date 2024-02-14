using SystemModule.Actors;
using SystemModule.MagicEvent;

namespace M2Server.Event.Events
{
    public class MagicEvent
    {
        public IList<IActor> ObjectList;
        public int StartTick;
        public int Time;
        public MapEvent[] Events;

        public MagicEvent()
        {
            Events = new MapEvent[8];
        }
    }
}