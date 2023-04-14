using GameSrv.Maps;

namespace GameSrv.Event.Events {
    public class PileStones : MapEvent {
        public PileStones(Envirnoment envir, int nX, int nY, int nType, int nTime) : base(envir, (short)nX, (short)nY, (byte)nType, nTime, true) {
            EventParam = 1;
        }

        public void AddEventParam() {
            if (EventParam < 5) {
                EventParam++;
            }
        }
    }
}

