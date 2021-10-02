using System.IO;
using SystemModule;

namespace GameSvr
{
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

        public byte[] GetPacket()
        {
            using MemoryStream memoryStream = new MemoryStream();
            var backingStream = new BinaryWriter(memoryStream);

            backingStream.Write(MagicInfo.GetPacket());//76
            backingStream.Write(btLevel);
            backingStream.Write(wMagIdx);
            backingStream.Write(nTranPoint);
            backingStream.Write(btKey);

            return (backingStream.BaseStream as MemoryStream).ToArray();
        }

    }
}

