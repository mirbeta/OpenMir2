using GameSvr.Actor;

namespace GameSvr.Event.Events
{
    public class MagicEvent
    {
        public IList<TBaseObject> BaseObjectList;
        public int dwStartTick;
        public int dwTime;
        public MirEvent[] Events;

        public MagicEvent()
        {
            Events = new MirEvent[8];
        }
    }
}

