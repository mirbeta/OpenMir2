using System.IO;
using SystemModule;

namespace GameSvr
{
    public class TClientMagic : Packets
    {
        public char Key;
        public byte Level;
        public int CurTrain;
        public TMagic Def;

        public TClientMagic()
        {
            Def = new TMagic();
        }

        protected override void ReadPacket(BinaryReader reader)
        {
            throw new System.NotImplementedException();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Key);
            writer.Write(Level);
            writer.Write((byte)0);
            writer.Write((byte)0);
            writer.Write(CurTrain);
            writer.Write(Def.GetBuffer());
        }
    }
}