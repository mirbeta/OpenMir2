using ProtoBuf;
using System.IO;

namespace SystemModule.Packet.ClientPackets
{
    [ProtoContract]
    public class Ability : Packets
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
        /// <summary>
        /// 最大血量
        /// </summary>
        [ProtoMember(9)]
        public ushort MaxHP;
        /// <summary>
        /// 最大魔法值
        /// </summary>
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
        public short WearWeight;
        /// <summary>
        /// 最大负重
        /// </summary>
        [ProtoMember(16)]
        public short MaxWearWeight;
        /// <summary>
        /// 腕力
        /// </summary>
        [ProtoMember(17)]
        public short HandWeight;
        /// <summary>
        /// 最大腕力
        /// </summary>
        [ProtoMember(18)]
        public short MaxHandWeight;

        public Ability() { }

        protected override void ReadPacket(BinaryReader reader)
        {
            Level = reader.ReadUInt16();
            AC = reader.ReadInt32();
            MAC = reader.ReadInt32();
            DC = reader.ReadInt32();
            MC = reader.ReadInt32();
            SC = reader.ReadInt32();
            HP = reader.ReadUInt16();
            MP = reader.ReadUInt16();
            MaxHP = reader.ReadUInt16();
            MaxMP = reader.ReadUInt16();
            Exp = reader.ReadInt32();
            MaxExp = reader.ReadInt32();
            Weight = reader.ReadUInt16();
            MaxWeight = reader.ReadUInt16();
            WearWeight = reader.ReadInt16();
            MaxWearWeight = reader.ReadInt16();
            HandWeight = reader.ReadInt16();
            MaxHandWeight = reader.ReadInt16();
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