using System;
using System.IO;
namespace M2Server
{
    public class TMagicRcd
    {
        /// <summary>
        /// 技能ID
        /// </summary>
        public ushort wMagIdx;
        /// <summary>
        /// 等级
        /// </summary>
        public byte btLevel;
        /// <summary>
        /// 技能快捷键
        /// </summary>
        public byte btKey;
        /// <summary>
        /// 当前修练值
        /// </summary>
        public int nTranPoint;

        public byte[] ToByte()
        {
            using (var memoryStream = new MemoryStream())
            {
                var backingStream = new BinaryWriter(memoryStream);

                backingStream.Write(wMagIdx);
                backingStream.Write(btLevel);
                backingStream.Write(btKey);
                backingStream.Write(nTranPoint);

                return (backingStream.BaseStream as MemoryStream).ToArray();
            }
        }

        public TMagicRcd() { }

        public TMagicRcd(byte[] buff)
        {
            this.wMagIdx = BitConverter.ToUInt16(buff, 0);
            this.btLevel = buff[2];
            this.btKey = buff[3];
            this.nTranPoint = BitConverter.ToInt16(buff, 4);
        }
    }
}