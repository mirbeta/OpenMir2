using System;
using System.IO;

namespace SystemModule
{
    public class TStdItem : Packets
    {
        /// <summary>
        /// 物品名称
        /// </summary>
        public string Name;

        /// <summary>
        /// 物品种类
        /// </summary>
        public byte StdMode;

        /// <summary>
        /// 书的类别
        /// </summary>
        public byte Shape;

        /// <summary>
        /// 重量
        /// </summary>
        public byte Weight;

        public byte AniCount;

        /// <summary>
        /// 武器神圣值
        /// </summary>
        public sbyte Source;

        public byte reserved;

        /// <summary>
        /// 武器升级后标记
        /// </summary>
        public byte NeedIdentify;

        /// <summary>
        /// 外观，即Items.WIL中的图片索引
        /// </summary>
        public ushort Looks;

        /// <summary>
        /// 持久力
        /// </summary>
        public int DuraMax;

        /// <summary>
        /// 防御 高位：武器准确 低位：武器幸运
        /// </summary>
        public int AC;

        /// <summary>
        /// 防魔 高位：武器速度 低位：武器诅咒
        /// </summary>
        public int MAC;

        /// <summary>
        /// 攻击
        /// </summary>
        public int DC;

        /// <summary>
        /// 魔法
        /// </summary>
        public int MC;

        /// <summary>
        /// 道术
        /// </summary>
        public int SC;

        /// <summary>
        /// 其他要求 0：等级 1：攻击力 2：魔法力 3：精神力
        /// </summary>
        public int Need;

        /// <summary>
        /// Need要求数值
        /// </summary>
        public int NeedLevel;

        /// <summary>
        /// 价格
        /// </summary>
        public uint Price;

        protected override void ReadPacket(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            var nameBuff = HUtil32.StringToByteAry(Name, out int nameLen);
            nameBuff[0] = (byte)nameLen;
            Array.Resize(ref nameBuff, 21);
            writer.Write(nameBuff);
            writer.Write(StdMode);
            writer.Write(Shape);
            writer.Write(Weight);
            writer.Write(AniCount);
            writer.Write(Source);
            writer.Write(reserved);
            writer.Write(NeedIdentify);
            writer.Write(Looks);
            writer.Write(DuraMax);
            writer.Write(AC);
            writer.Write(MAC);
            writer.Write(DC);
            writer.Write(MC);
            writer.Write(SC);
            writer.Write(Need);
            writer.Write(NeedLevel);
            writer.Write(Price);
            writer.Write((byte)0);
            writer.Write((byte)0);
        }
    }
}