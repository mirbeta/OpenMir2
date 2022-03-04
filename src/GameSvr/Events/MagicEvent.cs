using System.Collections.Generic;

namespace GameSvr
{
    public class MagicEvent
    {
        public IList<TBaseObject> BaseObjectList;
        public int dwStartTick;
        public int dwTime;
        public Event[] Events;

        public MagicEvent()
        {
            Events = new Event[8];
        }
    }
}

