using BotSvr.Objects;
using SystemModule.Packet.ClientPackets;

namespace BotSvr.Scenes
{
    public class ProcMagic
    {
        public int NTargetX;
        public int NTargetY;
        public TActor XTarget;
        public ClientMagic XMagic;
        public bool FReacll;
        public bool FContinue;
        public bool FUnLockMagic;
        public long DwTick;
    }
}