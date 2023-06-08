using M2Server.Actor;

namespace M2Server.Event.Events
{
    public class MagicEvent
    {
        public IList<BaseObject> ObjectList;
        public int StartTick;
        public int Time;
        public MapEvent[] Events;

        public MagicEvent()
        {
            Events = new MapEvent[8];
        }
    }
}