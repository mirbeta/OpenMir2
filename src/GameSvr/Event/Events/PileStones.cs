using GameSvr.Maps;

namespace GameSvr.Event.Events {
    public class PileStones : EventInfo {
        public PileStones(Envirnoment Envir, int nX, int nY, int nType, int nTime) : base(Envir, (short)nX, (short)nY, (byte)nType, nTime, true) {
            EventParam = 1;
        }

        public void AddEventParam() {
            if (EventParam < 5) {
                EventParam++;
            }
        }
    }
}

