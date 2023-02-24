using GameSvr.Actor;

namespace GameSvr.Event.Events {
    public class MagicEvent {
        public IList<BaseObject> BaseObjectList;
        public int dwStartTick;
        public int dwTime;
        public EventInfo[] Events;

        public MagicEvent() {
            Events = new EventInfo[8];
        }
    }
}

