using System.IO;
using SystemModule.Packet.ServerPackets;

namespace SystemModule.Packet.ClientPackets
{
    public class UserMagic : Packets
    {
        public MagicInfo Magic;
        public ushort MagIdx;
        public byte Level;
        public char Key;
        public int TranPoint;

        public UserMagic()
        {
            Magic = new MagicInfo();
        }

        protected override void ReadPacket(BinaryReader reader)
        {
            throw new System.NotImplementedException();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Magic.GetBuffer());
            writer.Write(MagIdx);
            writer.Write(Level);
            writer.Write(Key);
            writer.Write(TranPoint);
        }
    }
}
