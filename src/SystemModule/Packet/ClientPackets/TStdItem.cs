using System;
using System.IO;
using SystemModule.Extensions;

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
            Eva = new TEvaluation();
            SvrSet = new TStdItemExt();
        }

        protected override void ReadPacket(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Name, PacketConst.ItemNameLen);
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
}