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

        public byte[] GetPacket()
        {
            using (var memoryStream = new MemoryStream())
            {
                var backingStream = new BinaryWriter(memoryStream);

                backingStream.Write(Key);
                backingStream.Write(Level);
                backingStream.Write((byte)0);
                backingStream.Write((byte)0);
                backingStream.Write(CurTrain);
                backingStream.Write(Def.GetPacket());

                var stream = backingStream.BaseStream as MemoryStream;
                return stream.ToArray();
            }
        }
    }
}