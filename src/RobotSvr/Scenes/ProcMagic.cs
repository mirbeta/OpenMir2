using SystemModule;

namespace RobotSvr
{
    public struct ProcMagic
    {
        public int NTargetX;
        public int NTargetY;
        public TActor XTarget;
        public TClientMagic XMagic;
        public bool FReacll;
        public bool FContinue;
        public bool FUnLockMagic;
        public long DwTick;
    }
}