using System.IO;
using SystemModule.Packet.ServerPackets;

namespace SystemModule.Packet.ClientPackets
{
    public class UserMagic : Packets
    {
        public MagicInfo MagicInfo;
        public byte btLevel;
        public ushort wMagIdx;
        public int nTranPoint;
        public byte btKey;

        public UserMagic()
        {
            MagicInfo = new MagicInfo();
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
