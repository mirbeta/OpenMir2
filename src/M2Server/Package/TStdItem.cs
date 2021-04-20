using System;
using System.IO;

namespace M2Server
{
    public class TStdItem
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
        public short Looks;
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

        public byte[] ToByte()
        {
            using (var memoryStream = new MemoryStream())
            {
                var backingStream = new BinaryWriter(memoryStream);
                var nameLen = 0;
                var nameBuff = HUtil32.StringToByteAry(Name, out nameLen);
                nameBuff[0] = (byte)nameLen;
                Array.Resize(ref nameBuff, 21);
                backingStream.Write(nameBuff);
                backingStream.Write(StdMode);
                backingStream.Write(Shape);
                backingStream.Write(Weight);
                backingStream.Write(AniCount);
                backingStream.Write(Source);
                backingStream.Write(reserved);
                backingStream.Write(NeedIdentify);
                backingStream.Write(Looks);
                backingStream.Write(DuraMax);
                backingStream.Write(AC);
                backingStream.Write(MAC);
                backingStream.Write(DC);
                backingStream.Write(MC);
                backingStream.Write(SC);
                backingStream.Write(Need);
                backingStream.Write(NeedLevel);
                backingStream.Write(Price);
                backingStream.Write((byte)0);
                backingStream.Write((byte)0);

                var stream = backingStream.BaseStream as MemoryStream;
                return stream.ToArray();
            }
        }
    }
}

