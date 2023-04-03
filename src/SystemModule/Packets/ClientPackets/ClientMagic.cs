using System.IO;
using SystemModule.Data;

namespace SystemModule.Packets.ClientPackets
{
    public class ClientMagic : ClientPacket
    {
        public char Key;
        public byte Level;
        public int CurTrain;
        public MagicInfo Def;

        public ClientMagic()
        {
            Def = new MagicInfo();
        }

        protected override void ReadPacket(BinaryReader reader)
        {
            throw new System.NotImplementedException();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write((byte)Key);
            writer.Write(Level);
            writer.Write((byte)0);
            writer.Write((byte)0);
            writer.Write(CurTrain);
            writer.Write(Def.GetBuffer());
        }
    }
}