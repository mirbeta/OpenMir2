using System.IO;

namespace M2Server
{
    public class TClientMagic : Package
    {
        public char Key;
        public byte Level;
        public int CurTrain;
        public TMagic Def;

        public TClientMagic()
        {
            Def = new TMagic();
        }

        public byte[] ToByte()
        {
            using (var memoryStream = new MemoryStream())
            {
                var backingStream = new BinaryWriter(memoryStream);

                backingStream.Write(Key);
                backingStream.Write(Level);
                backingStream.Write((byte)0);
                backingStream.Write((byte)0);
                backingStream.Write(CurTrain);
                backingStream.Write(Def.ToByte());

                var stream = backingStream.BaseStream as MemoryStream;
                return stream.ToArray();
            }
        }
    }
}