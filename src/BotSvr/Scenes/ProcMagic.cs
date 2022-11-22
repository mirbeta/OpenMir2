using BotSvr.Objects;
using SystemModule.Packets.ClientPackets;

namespace BotSvr.Scenes
{
    public class ProcMagic
    {
        public short NTargetX;
        public short NTargetY;
        public TActor XTarget;
        public ClientMagic XMagic;
        public bool FReacll;
        public bool FContinue;
        public bool FUnLockMagic;
        public long DwTick;
    }
}