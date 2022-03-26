using System;
using System.IO;
using ProtoBuf;

namespace SystemModule
{
    [ProtoContract]
    public class TAbility : Packets
    {
        [ProtoMember(1)]
        public ushort Level;
        [ProtoMember(2)]
        public int AC;
        [ProtoMember(3)]
        public int MAC;
        [ProtoMember(4)]
        public int DC;
        [ProtoMember(5)]
        public int MC;
        [ProtoMember(6)]
        public int SC;
        /// <summary>
        /// 生命值
        /// </summary>
        [ProtoMember(7)]
        public ushort HP;
        /// <summary>
        /// 魔法值
        /// </summary>
        [ProtoMember(8)]
        public ushort MP;
        [ProtoMember(9)]
        public ushort MaxHP;
        [ProtoMember(10)]
        public ushort MaxMP;
        /// <summary>
        /// 当前经验
        /// </summary>
        [ProtoMember(11)]
        public int Exp;
        /// <summary>
        /// 最大经验
        /// </summary>
        [ProtoMember(12)]
        public int MaxExp;
        /// <summary>
        /// 背包重
        /// </summary>
        [ProtoMember(13)]
        public ushort Weight;
        /// <summary>
        /// 背包最大重量
        /// </summary>
        [ProtoMember(14)]
        public ushort MaxWeight;
        /// <summary>
        /// 当前负重
        /// </summary>
        [ProtoMember(15)]
        public ushort WearWeight;
        /// <summary>
        /// 最大负重
        /// </summary>
        [ProtoMember(16)]
        public ushort MaxWearWeight;
        /// <summary>
        /// 腕力
        /// </summary>
        [ProtoMember(17)]
        public ushort HandWeight;
        /// <summary>
        /// 最大腕力
        /// </summary>
        [ProtoMember(18)]
        public ushort MaxHandWeight;

        public TAbility() { }

        public TAbility(byte[] buff)
        {
            Level = BitConverter.ToUInt16(buff, 0);
            AC = BitConverter.ToUInt16(buff, 2);
            MAC = BitConverter.ToUInt16(buff, 6);
            DC = BitConverter.ToUInt16(buff, 10);
            MC = BitConverter.ToUInt16(buff, 14);
            SC = BitConverter.ToUInt16(buff, 18);
            HP = BitConverter.ToUInt16(buff, 22);
            MP = BitConverter.ToUInt16(buff, 24);
            MaxHP = BitConverter.ToUInt16(buff, 26);
            MaxMP = BitConverter.ToUInt16(buff, 28);
            Exp = BitConverter.ToInt32(buff, 30);
            MaxExp = BitConverter.ToInt32(buff, 34);
            Weight = BitConverter.ToUInt16(buff, 38);
            MaxWeight = BitConverter.ToUInt16(buff, 40);
            WearWeight = BitConverter.ToUInt16(buff, 42);
            MaxWearWeight = BitConverter.ToUInt16(buff, 44);
            HandWeight = BitConverter.ToUInt16(buff, 46);
            MaxHandWeight = BitConverter.ToUInt16(buff, 48);
        }

        protected override void ReadPacket(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Level);
            writer.Write(AC);
            writer.Write(MAC);
            writer.Write(DC);
            writer.Write(MC);
            writer.Write(SC);
            writer.Write(HP);
            writer.Write(MP);
            writer.Write(MaxHP);
            writer.Write(MaxMP);
            writer.Write(Exp);
            writer.Write(MaxExp);
            writer.Write(Weight);
            writer.Write(MaxWeight);
            writer.Write(WearWeight);
            writer.Write(MaxWearWeight);
            writer.Write(HandWeight);
            writer.Write(MaxHandWeight);
        }
    }
}