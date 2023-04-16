using GameSrv.Actor;

namespace GameSrv.Event.Events
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