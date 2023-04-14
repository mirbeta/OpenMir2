using GameSrv.Maps;

namespace GameSrv.Event.Events {
    /// <summary>
    /// 安全区光环
    /// </summary>
    public class SafeEvent : MapEvent {
        public SafeEvent(Envirnoment envir, short nX, short nY, byte nType) : base(envir, nX, nY, nType, HUtil32.GetTickCount(), true) {

        }

        public override void Run() {
            OpenStartTick = HUtil32.GetTickCount();
            base.Run();
        }
    }
}

