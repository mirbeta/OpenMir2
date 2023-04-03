using MemoryPack;
using System;
using System.IO;

namespace SystemModule.Packets.ClientPackets;

[MemoryPackable]
public partial class Ability : ClientPacket, ICloneable
{
    public byte Level { get; set; }
    public byte Reserved1 { get; set; }
    public ushort AC { get; set; }
    public ushort MAC { get; set; }
    public ushort DC { get; set; }
    public ushort MC { get; set; }
    public ushort SC { get; set; }
    /// <summary>
    /// 生命值
    /// </summary>
    public ushort HP { get; set; }
    /// <summary>
    /// 魔法值
    /// </summary>
    public ushort MP { get; set; }
    /// <summary>
    /// 最大血量
    /// </summary>
    public ushort MaxHP { get; set; }
    /// <summary>
    /// 最大魔法值
    /// </summary>
    public ushort MaxMP { get; set; }
    public byte ExpCount { get; set; }
    public byte ExpMaxCount { get; set; }
    /// <summary>
    /// 当前经验
    /// </summary>
    public int Exp { get; set; }
    /// <summary>
    /// 最大经验
    /// </summary>
    public int MaxExp { get; set; }
    /// <summary>
    /// 背包重
    /// </summary>
    public ushort Weight { get; set; }
    /// <summary>
    /// 背包最大重量
    /// </summary>
    public ushort MaxWeight { get; set; }
    /// <summary>
    /// 当前负重
    /// </summary>
    public byte WearWeight { get; set; }
    /// <summary>
    /// 最大负重
    /// </summary>
    public byte MaxWearWeight { get; set; }
    /// <summary>
    /// 腕力
    /// </summary>
    public byte HandWeight { get; set; }
    /// <summary>
    /// 最大腕力
    /// </summary>
    public byte MaxHandWeight { get; set; }

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