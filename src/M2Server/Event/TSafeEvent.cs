namespace M2Server
{
    /// <summary>
    /// 安全区光环
    /// </summary>
    public class TSafeEvent : TEvent
    {
        public TSafeEvent(TEnvirnoment Envir, int nX, int nY, int nType) : base(Envir, nX, nY, nType, HUtil32.GetTickCount(), true)
        {

        }

        public override void Run()
        {
            m_dwOpenStartTick = HUtil32.GetTickCount();
            base.Run();
        }
    }
}

