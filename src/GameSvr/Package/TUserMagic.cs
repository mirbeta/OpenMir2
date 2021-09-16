using System.IO;
using System.Runtime.InteropServices;
using SystemModule;

namespace GameSvr
{
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class TUserMagic
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

        public byte[] ToByte()
        {
            using (var memoryStream = new MemoryStream())
            {
                var backingStream = new BinaryWriter(memoryStream);

                backingStream.Write(MagicInfo.ToByte());//76
                backingStream.Write(btLevel);
                backingStream.Write(wMagIdx);
                backingStream.Write(nTranPoint);
                backingStream.Write(btKey);

                return (backingStream.BaseStream as MemoryStream).ToArray();
            }
        }

    }
}

