using System;
using System.IO;

namespace SystemModule
{
    public class TMagicRcd : Packets
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

        public TMagicRcd() { }

        public TMagicRcd(byte[] buff)
        {
            this.wMagIdx = BitConverter.ToUInt16(buff, 0);
            this.btLevel = buff[2];
            this.btKey = buff[3];
            this.nTranPoint = BitConverter.ToInt16(buff, 4);
        }

        protected override void ReadPacket(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(wMagIdx);
            writer.Write(btLevel);
            writer.Write(btKey);
            writer.Write(nTranPoint);
        }
    }
}