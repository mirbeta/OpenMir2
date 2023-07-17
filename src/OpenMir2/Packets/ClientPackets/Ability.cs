using MemoryPack;
using System.Runtime.InteropServices;

namespace SystemModule.Packets.ClientPackets
{
    [MemoryPackable(SerializeLayout.Sequential)]
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Size = 40)]
    public partial class Ability
    {
        public byte Level { get; set; }
        public byte Reserved1 { get; set; }
        /// <summary>
        /// 防御力
        /// </summary>
        public ushort AC { get; set; }
        /// <summary>
        /// 魔防力
        /// </summary>
        public ushort MAC { get; set; }
        /// <summary>
        /// 攻击力
        /// </summary>
        public ushort DC { get; set; }
        /// <summary>
        /// 魔法力
        /// </summary>
        public ushort MC { get; set; }
        /// <summary>
        /// 道术
        /// </summary>
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
        public short Reserved2 { get; set; }
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
    }

    public class AbilityFormatter : MemoryPackFormatter<Ability>
    {
        public override void Deserialize(ref MemoryPackReader reader, scoped ref Ability? value)
        {

        }

        public override void Serialize<TBufferWriter>(ref MemoryPackWriter<TBufferWriter> writer, scoped ref Ability? value)
        {
            writer.WriteObjectHeader(22);
            writer.WriteValue<byte>(value.Level);
            writer.WriteValue<byte>(value.Reserved1);
            writer.WriteValue<ushort>(value.AC);
            writer.WriteValue<ushort>(value.MAC);
            writer.WriteValue<ushort>(value.DC);
            writer.WriteValue<ushort>(value.MC);
            writer.WriteValue<ushort>(value.SC);
            writer.WriteValue<ushort>(value.HP);
            writer.WriteValue<ushort>(value.MP);
            writer.WriteValue<ushort>(value.MaxHP);
            writer.WriteValue<ushort>(value.MaxMP);
            writer.WriteValue<byte>(value.ExpCount);
            writer.WriteValue<byte>(value.ExpMaxCount);
            writer.WriteValue<int>(value.Exp);
            writer.WriteValue<int>(value.MaxExp);
            writer.WriteValue<ushort>(value.Weight);
            writer.WriteValue<ushort>(value.MaxWeight);
            writer.WriteValue<byte>(value.WearWeight);
            writer.WriteValue<byte>(value.MaxWearWeight);
            writer.WriteValue<byte>(value.HandWeight);
            writer.WriteValue<byte>(value.MaxHandWeight);
            writer.WriteValue<short>(value.Reserved2);
        }
    }
}