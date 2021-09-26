using System.IO;

namespace SystemModule
{
    public class TUserItem : Packets
    {
        public int MakeIndex;
        public ushort wIndex;
        public ushort Dura;
        public ushort DuraMax;
        public byte[] btValue;

        public TUserItem()
        {
            btValue = new byte[14];
        }

        public TUserItem(byte[] buffer)
            : base(buffer)
        {
            this.MakeIndex = ReadInt32();
            this.wIndex = ReadUInt16();
            this.Dura = ReadUInt16();
            this.DuraMax = ReadUInt16();
            this.btValue = ReadBytes(14);
        }

        public byte[] GetPacket()
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

