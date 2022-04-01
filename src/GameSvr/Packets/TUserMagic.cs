using System.IO;
using SystemModule;

namespace GameSvr
{
    public class TUserMagic : Packets
    {
        public TMagic MagicInfo;
        public byte btLevel;
        public ushort wMagIdx;
        public int nTranPoint;
        public byte btKey;

        public TUserMagic()
        {
            MagicInfo = new TMagic();
        }

        protected override void ReadPacket(BinaryReader reader)
        {
            throw new System.NotImplementedException();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(MagicInfo.GetBuffer());
            writer.Write(btLevel);
            writer.Write(wMagIdx);
            writer.Write(nTranPoint);
            writer.Write(btKey);
        }
    }
}
