using System;
using System.IO;

namespace SystemModule.Packet.ClientPackets
{

    public class TStdItem : Packets
    {
        public string Name;
        public byte StdMode;
        public byte Shape;
        public byte Weight;
        public byte AniCount;
        public short Source;
        public byte Reserved;
        public byte NeedIdentify;
        public ushort Looks;
        public ushort DuraMax;
        public int AC;
        public int MAC;
        public int DC;
        public int MC;
        public int SC;
        public int Need;
        public int NeedLevel;
        public int Price;
        public byte UniqueItem;
        public byte Overlap;
        public byte ItemType;
        public short ItemSet;
        public byte Binded;
        public byte[] Reserve;
        public byte[] AddOn;
        public TEvaluation Eva;
        public TStdItemExt SvrSet;

        public TStdItem()
        {
            Reserve = new byte[9];
            AddOn = new byte[10];
        }

        protected override void ReadPacket(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            var nameBuff = HUtil32.StringToByteAry(Name, out int nameLen);
            nameBuff[0] = (byte)nameLen;
            Array.Resize(ref nameBuff, PacketConst.ItemNameLen);
            writer.Write(nameBuff);
            writer.Write(StdMode);
            writer.Write(Shape);
            writer.Write(Weight);
            writer.Write(AniCount);
            writer.Write(Source);
            writer.Write(Reserved);
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
            writer.Write(UniqueItem);
            writer.Write(Overlap);
            writer.Write(ItemType);
            writer.Write(ItemSet);
            writer.Write(Binded);
            writer.Write(Reserve);
            writer.Write(AddOn);
            writer.Write(Eva.GetBuffer());
            writer.Write(SvrSet.GetBuffer());
        }
    }

    public class TEvaAbil : Packets
    {
        public byte btType;
        public byte btValue;

        public TEvaAbil()
        {
            btType = 0;
            btValue = 0;
        }

        protected override void ReadPacket(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(btType);
            writer.Write(btValue);
        }
    }

    public class TEvaluation : Packets
    {
        public byte EvaTimes;
        public byte EvaTimesMax;
        public byte AdvAbil;
        public byte AdvAbilMax;
        public byte Spirit;
        public byte SpiritMax;
        public TEvaAbil[] Abil;
        public byte BaseMax;
        public byte Quality;
        public byte SpiritQ;
        public byte SpSkill;

        public TEvaluation()
        {
            Abil = new TEvaAbil[8];
            for (int i = 0; i < Abil.Length; i++)
            {
                Abil[i] = new TEvaAbil();
            }
        }

        protected override void ReadPacket(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(EvaTimes);
            writer.Write(EvaTimesMax);
            writer.Write(AdvAbil);
            writer.Write(AdvAbilMax);
            writer.Write(Spirit);
            writer.Write(SpiritMax);
            for (int i = 0; i < Abil.Length; i++)
            {
                writer.Write(Abil[i].GetBuffer());
            }
            writer.Write(BaseMax);
            writer.Write(Quality);
            writer.Write(SpiritQ);
            writer.Write(SpSkill);
        }
    }
    
    public class TStdItemExt : Packets
    {
        public bool boHeroPickup;
        public byte btRefSuiteCount;
        public byte[] aSuiteWhere;
        public byte[] aSuiteIndex;
        public int nGetRate;
        public int nBind;

        public TStdItemExt()
        {
            aSuiteWhere = new byte[255];
            aSuiteIndex = new byte[255];
        }

        protected override void ReadPacket(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(boHeroPickup);
            writer.Write(btRefSuiteCount);
            writer.Write(aSuiteWhere);
            writer.Write(aSuiteIndex);
            writer.Write(nGetRate);
            writer.Write(nBind);
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
            Array.Resize(ref nameBuff, PacketConst.ItemNameLen);
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