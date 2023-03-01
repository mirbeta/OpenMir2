using BotSrv.Objects;
using SystemModule.Packets.ClientPackets;

namespace BotSrv.Data
{
    public class ProcMagic
    {
        public short NTargetX;
        public short NTargetY;
        public Actor XTarget;
        public ClientMagic XMagic;
        public bool FReacll;
        public bool FContinue;
        public bool FUnLockMagic;
        public long DwTick;
    }
}