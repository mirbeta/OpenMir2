using M2Server.Maps;

namespace M2Server.Event.Events {
    public class PileStones : MapEvent {
        public PileStones(Envirnoment envir, short nX, short nY, byte nType, int nTime) : base(envir, nX, nY, nType, nTime, true) {
            EventParam = 1;
        }

        public void AddEventParam() {
            if (EventParam < 5) {
                EventParam++;
            }
        }
    }
}

