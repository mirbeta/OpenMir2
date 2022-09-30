using ProtoBuf;
using System;
using System.IO;

namespace SystemModule.Packet.ClientPackets
{
    [ProtoContract]
    [Serializable]
    public class Ability : Packets, ICloneable
    {
        [ProtoMember(1)]
        public byte Level;
        [ProtoMember(2)]
        public byte Reserved1;
        [ProtoMember(3)]
        public ushort AC;
        [ProtoMember(4)]
        public ushort MAC;
        [ProtoMember(5)]
        public ushort DC;
        [ProtoMember(6)]
        public ushort MC;
        [ProtoMember(7)]
        public ushort SC;
        /// <summary>
        /// 生命值
        /// </summary>
        [ProtoMember(8)]
        public ushort HP;
        /// <summary>
        /// 魔法值
        /// </summary>
        [ProtoMember(9)]
        public ushort MP;
        /// <summary>
        /// 最大血量
        /// </summary>
        [ProtoMember(10)]
        public ushort MaxHP;
        /// <summary>
        /// 最大魔法值
        /// </summary>
        [ProtoMember(11)]
        public ushort MaxMP;
        [ProtoMember(12)]
        public byte ExpCount;
        [ProtoMember(13)]
        public byte ExpMaxCount;
        /// <summary>
        /// 当前经验
        /// </summary>
        [ProtoMember(14)]
        public int Exp;
        /// <summary>
        /// 最大经验
        /// </summary>
        [ProtoMember(15)]
        public int MaxExp;
        /// <summary>
        /// 背包重
        /// </summary>
        [ProtoMember(16)]
        public ushort Weight;
        /// <summary>
        /// 背包最大重量
        /// </summary>
        [ProtoMember(17)]
        public ushort MaxWeight;
        /// <summary>
        /// 当前负重
        /// </summary>
        [ProtoMember(18)]
        public byte WearWeight;
        /// <summary>
        /// 最大负重
        /// </summary>
        [ProtoMember(19)]
        public byte MaxWearWeight;
        /// <summary>
        /// 腕力
        /// </summary>
        [ProtoMember(20)]
        public byte HandWeight;
        /// <summary>
        /// 最大腕力
        /// </summary>
        [ProtoMember(21)]
        public byte MaxHandWeight;

        public Ability() { }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        protected override void ReadPacket(BinaryReader reader)
        {
            Level = reader.ReadByte();
            Reserved1 = reader.ReadByte();
            AC = reader.ReadUInt16();
            MAC = reader.ReadUInt16();
            DC = reader.ReadUInt16();
            MC = reader.ReadUInt16();
            SC = reader.ReadUInt16();
            HP = reader.ReadUInt16();
            MP = reader.ReadUInt16();
            MaxHP = reader.ReadUInt16();
            MaxMP = reader.ReadUInt16();
            ExpCount = reader.ReadByte();
            ExpMaxCount = reader.ReadByte();
            Exp = reader.ReadInt32();
            MaxExp = reader.ReadInt32();
            Weight = reader.ReadUInt16();
            MaxWeight = reader.ReadUInt16();
            WearWeight = reader.ReadByte();
            MaxWearWeight = reader.ReadByte();
            HandWeight = reader.ReadByte();
            MaxHandWeight = reader.ReadByte();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Level);
            writer.Write(Reserved1);
            writer.Write(AC);
            writer.Write(MAC);
            writer.Write(DC);
            writer.Write(MC);
            writer.Write(SC);
            writer.Write(HP);
            writer.Write(MP);
            writer.Write(MaxHP);
            writer.Write(MaxMP);
            writer.Write(ExpCount);
            writer.Write(ExpMaxCount);
            writer.Write((byte)0);
            writer.Write((byte)0);
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