using System.Collections.Generic;

namespace GameSvr
{
    public class TMagicEvent
    {
        public IList<TBaseObject> BaseObjectList;
        public int dwStartTick;
        public int dwTime;
        public TEvent[] Events;

        public TMagicEvent()
        {
            Events = new TEvent[8];
        }
    }
}

