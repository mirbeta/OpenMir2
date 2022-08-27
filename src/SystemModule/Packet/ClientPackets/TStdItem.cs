using System;
using System.IO;

namespace SystemModule.Packet.ClientPackets
{
    public class TStdItem:Packets
    {
        public string Name; // 酒捞袍 捞抚 (玫窍力老八)
        public byte StdMode; //
        public byte Shape; // 屈怕喊 捞抚 (枚八)
        public byte Weight; // 公霸
        public byte AniCount; // 1焊促 农搁 局聪皋捞记 登绰 酒捞袍 (促弗 侩档肺 腹捞 静烙)
        public short SpecialPwr; // +捞搁 积拱傍拜+瓷仿, -捞搁 攫单靛傍拜+
        //1~10 碍档
        //-50~-1 攫单靛 瓷仿摹 氢惑
        //-100~-51 攫单靛 瓷仿摹 皑家
        public byte ItemDesc ; //$01 IDC_UNIDENTIFIED  (酒捞错萍颇捞 救 等 巴, 努扼捞攫飘俊辑父 荤侩凳)
        //$02 IDC_UNABLETAKEOFF (颊俊辑 冻绢瘤瘤 臼澜, 固瘤荐 荤侩 啊瓷)
        //$04 IDC_NEVERTAKEOFF  (颊俊辑 冻绢瘤瘤 臼澜, 固瘤荐 荤侩 阂啊瓷)
        //$08 IDC_DIEANDBREAK   (馒侩酒捞袍俊辑 磷栏搁 柄瘤绰 加己)
        //$10 IDC_NEVERLOSE     (馒侩酒捞袍俊辑 磷绢档 冻绢瘤瘤 臼澜)
        public ushort Looks; // 弊覆 锅龋
        public ushort DuraMax;
        public ushort AC; // 规绢仿
        public ushort MAC; // 付亲仿
        public ushort DC; // 单固瘤
        public ushort MC; // 贱荤狼 付过 颇况
        public ushort SC; // 档荤狼 沥脚仿
        public byte Need; // 0:Level, 1:DC, 2:MC, 3:SC
        public byte NeedLevel; // 1..60 level value...
        public byte NeedIdentify;
        public int Price; // 啊拜
        public int Stock; // 焊蜡樊
        public byte AtkSpd; // 傍拜加档
        public byte Agility; // 刮酶
        public byte Accurate; // 沥犬
        public byte MgAvoid; // 付过雀乔 -> 付过历亲(sonmg)
        public byte Strong; // 碍档
        public byte Undead; // 荤磊
        public int HpAdd; // 眠啊HP
        public int MpAdd; // 眠啊MP
        public int ExpAdd; // 眠啊 版氰摹
        public byte EffType1; // 瓤苞辆幅1
        public byte EffRate1; // 瓤苞犬伏1
        public byte EffValue1; // 瓤苞蔼1
        public byte EffType2; // 瓤苞辆幅2
        public byte EffRate2; // 瓤苞犬伏2
        public byte EffValue2; // 瓤苞蔼2
        public byte Slowdown; // 敌拳
        public byte Tox; // 吝刀
        public byte ToxAvoid; // 吝刀历亲
        public byte UniqueItem; // 蜡聪农加己
        // 蜡聪农 --- $01:力访/诀弊饭捞靛 救凳
        // 蜡聪农 --- $02:荐府阂啊
        // 蜡聪农 --- $04:滚府搁荤扼咙(啊规芒俊辑 冻备瘤 臼澜)
        // 蜡聪农 --- $08:背券阂啊(12=4+8 : 背券阂啊,冻崩阂啊)
        public byte OverlapItem; // 吝汗倾侩
        public byte light; // 蝴阑郴绰 酒捞袍
        public byte ItemType; // 酒捞袍狼 备盒
        public ushort ItemSet; // 悸飘 酒捞袍 备盒
        public string Reference; // 曼炼 巩磊凯
        protected override void ReadPacket(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            var nameBuff = HUtil32.StringToByteAry(Name, out int nameLen);
            nameBuff[0] = (byte) nameLen;
            Array.Resize(ref nameBuff, 15);
            writer.Write(nameBuff);
            writer.Write(StdMode);
            writer.Write(Shape);
            writer.Write(Weight);
            writer.Write(AniCount);
            writer.Write(SpecialPwr);
            writer.Write(ItemDesc);
            writer.Write(Looks);
            writer.Write(DuraMax);
            writer.Write(AC);
            writer.Write(MAC);
            writer.Write(DC);
            writer.Write(MC);
            writer.Write(SC);
            writer.Write(Need);
            writer.Write(NeedLevel);
            writer.Write(NeedIdentify);
            writer.Write(Price);
            writer.Write(Stock);
            writer.Write(AtkSpd);
            writer.Write(Agility);
            writer.Write(Accurate);
            writer.Write(MgAvoid);
            writer.Write(Strong);
            writer.Write(Undead);
            writer.Write(HpAdd);
            writer.Write(MpAdd);
            writer.Write(ExpAdd);
            writer.Write(EffType1);
            writer.Write(EffRate1);
            writer.Write(EffValue1);
            writer.Write(EffType2);
            writer.Write(EffRate2);
            writer.Write(EffValue2);
            writer.Write(Slowdown);
            writer.Write(Tox);
            writer.Write(ToxAvoid);
            writer.Write(UniqueItem);
            writer.Write(OverlapItem);
            writer.Write(light);
            writer.Write(ItemType);
            writer.Write(ItemSet);
            var referenceBuff = HUtil32.StringToByteAry(Name, out nameLen);
            referenceBuff[0] = (byte) nameLen;
            Array.Resize(ref referenceBuff, 15);
            writer.Write(referenceBuff);
        }
    }

    public class TOldStdItem : Packets
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
            Array.Resize(ref nameBuff, 15);
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