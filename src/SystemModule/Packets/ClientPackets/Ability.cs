using MemoryPack;
using System;
using System.Runtime.InteropServices;

namespace SystemModule.Packets.ClientPackets
{
    [MemoryPackable]
    [StructLayout(LayoutKind.Sequential,CharSet = CharSet.Ansi, Size =40)]
    public partial struct Ability : ICloneable
    {
        [MemoryPackOrder(0)]
        public byte Level { get; set; }
        [MemoryPackOrder(1)]
        public byte Reserved1 { get; set; }
        /// <summary>
        /// 防御力
        /// </summary>
        [MemoryPackOrder(2)]
        public ushort AC { get; set; }
        /// <summary>
        /// 魔防力
        /// </summary>
        [MemoryPackOrder(3)]
        public ushort MAC { get; set; }
        /// <summary>
        /// 攻击力
        /// </summary>
        [MemoryPackOrder(4)]
        public ushort DC { get; set; }
        /// <summary>
        /// 魔法力
        /// </summary>
        [MemoryPackOrder(5)]
        public ushort MC { get; set; }
        /// <summary>
        /// 道术
        /// </summary>
        [MemoryPackOrder(6)]
        public ushort SC { get; set; }
        /// <summary>
        /// 生命值
        /// </summary>
        [MemoryPackOrder(7)]
        public ushort HP { get; set; }
        /// <summary>
        /// 魔法值
        /// </summary>
        [MemoryPackOrder(8)]
        public ushort MP { get; set; }
        /// <summary>
        /// 最大血量
        /// </summary>
        [MemoryPackOrder(9)]
        public ushort MaxHP { get; set; }
        /// <summary>
        /// 最大魔法值
        /// </summary>
        [MemoryPackOrder(10)]
        public ushort MaxMP { get; set; }
        [MemoryPackOrder(11)]
        public byte ExpCount { get; set; }
        [MemoryPackOrder(12)]
        public byte ExpMaxCount { get; set; }
        /// <summary>
        /// 当前经验
        /// </summary>
        [MemoryPackOrder(13)]
        public int Exp { get; set; }
        /// <summary>
        /// 最大经验
        /// </summary>
        [MemoryPackOrder(14)]
        public int MaxExp { get; set; }
        /// <summary>
        /// 背包重
        /// </summary>
        [MemoryPackOrder(15)]
        public ushort Weight { get; set; }
        /// <summary>
        /// 背包最大重量
        /// </summary>
        [MemoryPackOrder(16)]
        public ushort MaxWeight { get; set; }
        /// <summary>
        /// 当前负重
        /// </summary>
        [MemoryPackOrder(17)]
        public byte WearWeight { get; set; }
        /// <summary>
        /// 最大负重
        /// </summary>
        [MemoryPackOrder(18)]
        public byte MaxWearWeight { get; set; }
        /// <summary>
        /// 腕力
        /// </summary>
        [MemoryPackOrder(19)]
        public byte HandWeight { get; set; }
        /// <summary>
        /// 最大腕力
        /// </summary>
        [MemoryPackOrder(20)]
        public byte MaxHandWeight { get; set; }

        public Ability() { }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public void Test(int dw)
        {
            Exp += dw;
        }

        //protected override void ReadPacket(BinaryReader reader)
        //{
        //    Level = reader.ReadByte();
        //    Reserved1 = reader.ReadByte();
        //    AC = reader.ReadUInt16();
        //    MAC = reader.ReadUInt16();
        //    DC = reader.ReadUInt16();
        //    MC = reader.ReadUInt16();
        //    SC = reader.ReadUInt16();
        //    HP = reader.ReadUInt16();
        //    MP = reader.ReadUInt16();
        //    MaxHP = reader.ReadUInt16();
        //    MaxMP = reader.ReadUInt16();
        //    ExpCount = reader.ReadByte();
        //    ExpMaxCount = reader.ReadByte();
        //    Exp = reader.ReadInt32();
        //    MaxExp = reader.ReadInt32();
        //    Weight = reader.ReadUInt16();
        //    MaxWeight = reader.ReadUInt16();
        //    WearWeight = reader.ReadByte();
        //    MaxWearWeight = reader.ReadByte();
        //    HandWeight = reader.ReadByte();
        //    MaxHandWeight = reader.ReadByte();
        //}

        //protected override void WritePacket(BinaryWriter writer)
        //{
        //    writer.Write(Level);
        //    writer.Write(Reserved1);
        //    writer.Write(AC);
        //    writer.Write(MAC);
        //    writer.Write(DC);
        //    writer.Write(MC);
        //    writer.Write(SC);
        //    writer.Write(HP);
        //    writer.Write(MP);
        //    writer.Write(MaxHP);
        //    writer.Write(MaxMP);
        //    writer.Write(ExpCount);
        //    writer.Write(ExpMaxCount);
        //    writer.Write((byte)0);
        //    writer.Write((byte)0);
        //    writer.Write(Exp);
        //    writer.Write(MaxExp);
        //    writer.Write(Weight);
        //    writer.Write(MaxWeight);
        //    writer.Write(WearWeight);
        //    writer.Write(MaxWearWeight);
        //    writer.Write(HandWeight);
        //    writer.Write(MaxHandWeight);
        //}
    }
}
