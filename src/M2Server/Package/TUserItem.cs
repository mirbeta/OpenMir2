using System.IO;

namespace M2Server
{
    public class TUserItem : Package
    {
        public int MakeIndex;
        public short wIndex;
        public short Dura;
        public short DuraMax;
        public byte[] btValue;

        public TUserItem()
        {
            btValue = new byte[14];
        }

        public TUserItem(byte[] buffer)
            : base(buffer)
        {
            this.MakeIndex = ReadInt32();
            this.wIndex = ReadInt16();
            this.Dura = ReadInt16();
            this.DuraMax = ReadInt16();
            this.btValue = ReadBytes(14);
        }

        public byte[] ToByte()
        {
            using (var memoryStream = new MemoryStream())
            {
                var backingStream = new BinaryWriter(memoryStream);

                backingStream.Write(MakeIndex);
                backingStream.Write(wIndex);
                backingStream.Write(Dura);
                backingStream.Write(DuraMax);
                backingStream.Write(btValue);

                return (backingStream.BaseStream as MemoryStream).ToArray();
            }
        }
    }
}

