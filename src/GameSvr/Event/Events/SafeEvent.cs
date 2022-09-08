using GameSvr.Maps;
using SystemModule;

namespace GameSvr.Event.Events
{
    /// <summary>
    /// 安全区光环
    /// </summary>
    public class SafeEvent : MirEvent
    {
        public SafeEvent(Envirnoment Envir, int nX, int nY, int nType) : base(Envir, nX, nY, nType, HUtil32.GetTickCount(), true)
        {

        }

        public override void Run()
        {
            OpenStartTick = HUtil32.GetTickCount();
            base.Run();
        }
    }
}

