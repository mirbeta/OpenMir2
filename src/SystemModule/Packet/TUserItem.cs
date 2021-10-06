using System.IO;

namespace SystemModule
{
    public class TUserItem : Packets
    {
        /// <summary>
        /// 唯一ID
        /// </summary>
        public int MakeIndex;
        /// <summary>
        /// 物品ID
        /// </summary>
        public ushort wIndex;
        /// <summary>
        /// 当前持久值
        /// </summary>
        public ushort Dura;
        /// <summary>
        /// 最大持久值
        /// </summary>
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

        public TUserItem(TUserItem userItem)
        {
            this.MakeIndex = userItem.MakeIndex;
            this.wIndex = userItem.wIndex;
            this.Dura = userItem.Dura;
            this.DuraMax = userItem.DuraMax;
            this.btValue = userItem.btValue;
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

