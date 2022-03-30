using ProtoBuf;
using System;
using System.IO;

namespace SystemModule
{
    [ProtoContract]
    public class TMagicRcd : Packets
    {
        /// <summary>
        /// 技能ID
        /// </summary>
        [ProtoMember(1)]
        public ushort wMagIdx;
        /// <summary>
        /// 等级
        /// </summary>
        [ProtoMember(2)]
        public byte btLevel;
        /// <summary>
        /// 技能快捷键
        /// </summary>
        [ProtoMember(3)]
        public byte btKey;
        /// <summary>
        /// 当前修练值
        /// </summary>
        [ProtoMember(4)]
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