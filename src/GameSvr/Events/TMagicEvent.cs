using System.Collections.Generic;

namespace GameSvr
{
    public class TMagicEvent
    {
        public IList<TBaseObject> BaseObjectList;
        public double dwStartTick;
        public double dwTime;
        public TEvent[] Events;
    }
}

