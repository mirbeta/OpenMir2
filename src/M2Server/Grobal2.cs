using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using SystemModule;

namespace M2Server
{

    public enum MessageType
    {
        Success = 0,
        Error = 1
    }

    public enum MessageLevel
    {
        Hihg = 3,
        Low = 2,
        None = 1
    }

    public struct TDefaultMessage
    {
        public int Recog;
        public short Ident;
        public short Param;
        public short Tag;
        public short Series;

        public byte[] ToByte()
        {
            using (var memoryStream = new MemoryStream())
            {
                var backingStream = new BinaryWriter(memoryStream);
                backingStream.Write(Recog);
                backingStream.Write(Ident);
                backingStream.Write(Param);
                backingStream.Write(Tag);
                backingStream.Write(Series);

                var stream = backingStream.BaseStream as MemoryStream;
                return stream.ToArray();
            }
        }

    }

    public struct TChrMsg
    {
        public int Ident;
        public int X;
        public int Y;
        public int Dir;
        public int State;
        public int feature;
        public string saying;
        public int Sound;
    }

    public class TRouteInfo
    {
        public int nServerIdx;
        public int nGateCount;
        public string sSelGateIP;
        public string[] sGameGateIP;
        public int[] nGameGatePort;

        public TRouteInfo()
        {
            sGameGateIP = new string[16];
            nGameGatePort = new int[16];
        }
    }

    public struct TOStdItem
    {
        public string Name;
        public byte StdMode;
        public byte Shape;
        public byte Weight;
        public byte AniCount;
        public byte Source;
        public byte Reserved;
        public byte NeedIdentify;
        public short Looks;
        public short DuraMax;
        public short AC;
        public short MAC;
        public short DC;
        public short MC;
        public short SC;
        public byte Need;
        public byte NeedLevel;
        public int Price;

        public byte[] ToByte()
        {
            using (var memoryStream = new MemoryStream())
            {
                var backingStream = new BinaryWriter(memoryStream);
                backingStream.Write(Name.ToByte(15));
                backingStream.Write(StdMode);
                backingStream.Write(Shape);
                backingStream.Write(Weight);
                backingStream.Write(AniCount);
                backingStream.Write(Source);
                backingStream.Write(Reserved);
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

                var stream = backingStream.BaseStream as MemoryStream;
                return stream.ToArray();
            }
        }

    }

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

    public class TClientItem
    {
        public TStdItem S;
        public int MakeIndex;
        public short Dura;
        public short DuraMax;

        public TClientItem()
        {
            S = new TStdItem();
        }

        public byte[] ToByte()
        {
            using (var memoryStream = new MemoryStream())
            {
                var backingStream = new BinaryWriter(memoryStream);

                backingStream.Write(S.ToByte());
                backingStream.Write(MakeIndex);
                backingStream.Write(Dura);
                backingStream.Write(DuraMax);

                var stream = backingStream.BaseStream as MemoryStream;
                return stream.ToArray();
            }
        }

    }

    public class TOClientItem
    {
        public TOStdItem S;
        public int MakeIndex;
        public short Dura;
        public short DuraMax;

        public byte[] ToByte()
        {
            using (var memoryStream = new MemoryStream())
            {
                var backingStream = new BinaryWriter(memoryStream);

                backingStream.Write(S.ToByte());
                backingStream.Write(MakeIndex);
                backingStream.Write(Dura);
                backingStream.Write(DuraMax);

                var stream = backingStream.BaseStream as MemoryStream;
                return stream.ToArray();
            }
        }
    }

    public class TUserStateInfo
    {
        public int Feature;
        public string UserName;
        public string GuildName;
        public string GuildRankName;
        public short NameColor;
        public TClientItem[] UseItems;

        public TUserStateInfo()
        {
            UseItems = new TClientItem[13];
        }

        public byte[] ToByte()
        {
            using (var memoryStream = new MemoryStream())
            {
                var backingStream = new BinaryWriter(memoryStream);

                backingStream.Write(Feature);

                var StrLen = 0;
                var NameBuff = HUtil32.StringToByteAry(UserName, out StrLen);
                NameBuff[0] = (byte)StrLen;
                Array.Resize(ref NameBuff, 20);
                backingStream.Write(NameBuff, 0, NameBuff.Length);

                backingStream.Write(NameColor);

                NameBuff = HUtil32.StringToByteAry(GuildName, out StrLen);
                NameBuff[0] = (byte)StrLen;
                Array.Resize(ref NameBuff, 15);
                backingStream.Write(NameBuff, 0, NameBuff.Length);

                NameBuff = HUtil32.StringToByteAry(GuildRankName, out StrLen);
                NameBuff[0] = (byte)StrLen;
                Array.Resize(ref NameBuff, 15);
                backingStream.Write(NameBuff, 0, NameBuff.Length);

                for (var i = 0; i < UseItems.Length; i++)
                {
                    backingStream.Write(UseItems[i].ToByte());
                }

                var stream = backingStream.BaseStream as MemoryStream;
                return stream.ToArray();
            }
        }
    }

    public class TOUserStateInfo
    {
        public int Feature;
        public string UserName;
        public string GuildName;
        public string GuildRankName;
        public short NameColor;
        public TOClientItem[] UseItems;
    }

    public class TMagic
    {
        public short wMagicID;
        public string sMagicName;
        public byte btEffectType;
        public byte btEffect;
        public short wSpell;
        public short wPower;
        public byte[] TrainLevel;
        public int[] MaxTrain;
        public byte btTrainLv;
        public byte btJob;
        public int dwDelayTime;
        public byte btDefSpell;
        public byte btDefPower;
        public short wMaxPower;
        public byte btDefMaxPower;
        public string sDescr;

        public TMagic()
        {
            TrainLevel = new byte[4];
            MaxTrain = new int[4];
        }

        public byte[] ToByte()
        {
            using (var memoryStream = new MemoryStream())
            {
                var backingStream = new BinaryWriter(memoryStream);
                backingStream.Write(wMagicID);
                backingStream.Write(sMagicName.ToByte(13));
                backingStream.Write(btEffectType);
                backingStream.Write(btEffect);
                backingStream.Write((byte)0);
                backingStream.Write(wSpell);
                backingStream.Write(wPower);
                backingStream.Write(TrainLevel);
                backingStream.Write((byte)0);
                backingStream.Write((byte)0);
                backingStream.Write(MaxTrain[0]);
                backingStream.Write(MaxTrain[1]);
                backingStream.Write(MaxTrain[2]);
                backingStream.Write(MaxTrain[3]);
                backingStream.Write(btTrainLv);
                backingStream.Write(btJob);
                backingStream.Write((byte)0);
                backingStream.Write((byte)0);
                backingStream.Write(dwDelayTime);
                backingStream.Write(btDefSpell);
                backingStream.Write(btDefPower);
                backingStream.Write(wMaxPower);
                backingStream.Write(btDefMaxPower);
                backingStream.Write(sDescr.ToByte(19));

                var stream = backingStream.BaseStream as MemoryStream;
                return stream.ToArray();
            }
        }

    }

    public class TOMagic
    {
        public short wMagicID;
        public byte btEffectType;
        public byte btEffect;
        public short wSpell;
        public short wPower;
        public byte btTrainLv;
        public byte btJob;
        public int dwDelayTime;
        public byte btDefSpell;
        public byte btDefPower;
        public short wMaxPower;
        public byte btDefMaxPower;

        public TOMagic()
        {
        }

        public byte[] ToByte()
        {
            using (var memoryStream = new MemoryStream())
            {
                var backingStream = new BinaryWriter(memoryStream);
                backingStream.Write(wMagicID);
                backingStream.Write(btEffectType);
                backingStream.Write(btEffect);
                backingStream.Write(wSpell);
                backingStream.Write(wPower);
                backingStream.Write(btTrainLv);
                backingStream.Write(btJob);
                backingStream.Write(dwDelayTime);
                backingStream.Write(btDefSpell);
                backingStream.Write(btDefPower);
                backingStream.Write(wMaxPower);
                backingStream.Write(btDefMaxPower);

                var stream = backingStream.BaseStream as MemoryStream;
                return stream.ToArray();
            }
        }

    }

    public class TClientMagic
    {
        public char Key;
        public byte Level;
        public int CurTrain;
        public TMagic Def;

        public TClientMagic()
        {
            Def = new TMagic();
        }

        public byte[] ToByte()
        {
            using (var memoryStream = new MemoryStream())
            {
                var backingStream = new BinaryWriter(memoryStream);

                backingStream.Write(Key);
                backingStream.Write(Level);
                backingStream.Write((byte)0);
                backingStream.Write((byte)0);
                backingStream.Write(CurTrain);
                backingStream.Write(Def.ToByte());

                var stream = backingStream.BaseStream as MemoryStream;
                return stream.ToArray();
            }
        }
    }

    public class TNakedAbility : Package
    {
        public short DC;
        public short MC;
        public short SC;
        public short AC;
        public short MAC;
        public short HP;
        public short MP;
        public byte Hit;
        public int Speed;
        public byte X2;

        public TNakedAbility() { }

        public TNakedAbility(byte[] buff)
            : base(buff)
        {
            DC = ReadInt16(); //BitConverter.ToInt16(buff, 0);
            MC = ReadInt16();//BitConverter.ToInt16(buff, 2);
            SC = ReadInt16();//BitConverter.ToInt16(buff, 4);
            AC = ReadInt16();//BitConverter.ToInt16(buff, 6);
            MAC = ReadInt16();//BitConverter.ToInt16(buff, 8);
            HP = ReadInt16();//BitConverter.ToInt16(buff, 10);
            MP = ReadInt16();//BitConverter.ToInt16(buff, 12);
            Hit = ReadByte(); //buff[13];
            Speed = ReadInt32(); //BitConverter.ToInt32(buff, 14);
            X2 = ReadByte(); //buff[20];
        }

        public byte[] ToByte()
        {
            using (var memoryStream = new MemoryStream())
            {
                var backingStream = new BinaryWriter(memoryStream);
                backingStream.Write(DC);
                backingStream.Write(MC);
                backingStream.Write(SC);
                backingStream.Write(AC);
                backingStream.Write(MAC);
                backingStream.Write(HP);
                backingStream.Write(MP);
                backingStream.Write(Hit);
                backingStream.Write(Speed);
                backingStream.Write(X2);
                backingStream.Write(0);

                var stream = backingStream.BaseStream as MemoryStream;
                return stream.ToArray();
            }
        }
    }

    public class TOAbility
    {
        public short Level;
        public short AC;
        public short MAC;
        public short DC;
        public short MC;
        public short SC;
        public short HP;
        public short MP;
        public short MaxHP;
        public short MaxMP;
        public int dw1AC;
        public int Exp;
        public int MaxExp;
        public short Weight;
        public short MaxWeight;
        public byte WearWeight;
        public byte MaxWearWeight;
        public byte HandWeight;
        public byte MaxHandWeight;

        public byte[] ToByte()
        {
            using (var memoryStream = new MemoryStream())
            {
                var backingStream = new BinaryWriter(memoryStream);
                backingStream.Write(Level);
                backingStream.Write(AC);
                backingStream.Write(MAC);
                backingStream.Write(DC);
                backingStream.Write(MC);
                backingStream.Write(SC);
                backingStream.Write(HP);
                backingStream.Write(MP);
                backingStream.Write(MaxHP);
                backingStream.Write(MaxMP);
                backingStream.Write(Exp);
                backingStream.Write(MaxExp);
                backingStream.Write(Weight);
                backingStream.Write(MaxWeight);
                backingStream.Write(WearWeight);
                backingStream.Write(MaxWearWeight);
                backingStream.Write(HandWeight);
                backingStream.Write(MaxHandWeight);

                var stream = backingStream.BaseStream as MemoryStream;
                return stream.ToArray();
            }
        }

    }

    public class TShortMessage
    {
        public short Ident;
        public short wMsg;

        public byte[] ToByte()
        {
            using (var memoryStream = new MemoryStream())
            {
                var backingStream = new BinaryWriter(memoryStream);

                backingStream.Write(Ident);
                backingStream.Write(wMsg);

                var stream = backingStream.BaseStream as MemoryStream;
                return stream.ToArray();
            }
        }
    }

    public class TMessageBodyW
    {
        public short Param1;
        public short Param2;
        public short Tag1;
        public short Tag2;

        public byte[] ToByte()
        {
            using (var memoryStream = new MemoryStream())
            {
                var backingStream = new BinaryWriter(memoryStream);

                backingStream.Write(Param1);
                backingStream.Write(Param2);
                backingStream.Write(Tag1);
                backingStream.Write(Tag2);

                var stream = backingStream.BaseStream as MemoryStream;
                return stream.ToArray();
            }
        }
    }

    public class TMessageBodyWL
    {
        public int lParam1;
        public int lParam2;
        public int lTag1;
        public int lTag2;

        public byte[] ToByte()
        {
            using (var memoryStream = new MemoryStream())
            {
                var backingStream = new BinaryWriter(memoryStream);

                backingStream.Write(lParam1);
                backingStream.Write(lParam2);
                backingStream.Write(lTag1);
                backingStream.Write(lTag2);

                var stream = backingStream.BaseStream as MemoryStream;
                return stream.ToArray();
            }
        }
    }

    public struct TCharDesc
    {
        public int Feature;
        public int Status;

        public byte[] ToByte()
        {
            using (var memoryStream = new MemoryStream())
            {
                var backingStream = new BinaryWriter(memoryStream);
                backingStream.Write(Feature);
                backingStream.Write(Status);
                var stream = backingStream.BaseStream as MemoryStream;
                return stream.ToArray();
            }
        }
    }

    public struct TClientGoods
    {
        public string Name;
        public int SubMenu;
        public int Price;
        public int Stock;
        public int Grade;
    }

    public class TAbility
    {
        public short Level;
        public int AC;
        public int MAC;
        public int DC;
        public int MC;
        public int SC;
        /// <summary>
        /// 生命值
        /// </summary>
        public short HP;
        /// <summary>
        /// 魔法值
        /// </summary>
        public short MP;
        public short MaxHP;
        public short MaxMP;
        /// <summary>
        /// 当前经验
        /// </summary>
        public int Exp;
        /// <summary>
        /// 最大经验
        /// </summary>
        public int MaxExp;
        /// <summary>
        /// 背包重
        /// </summary>
        public short Weight;
        /// <summary>
        /// 背包最大重量
        /// </summary>
        public short MaxWeight;
        /// <summary>
        /// 当前负重
        /// </summary>
        public short WearWeight;
        /// <summary>
        /// 最大负重
        /// </summary>
        public short MaxWearWeight;
        /// <summary>
        /// 腕力
        /// </summary>
        public short HandWeight;
        /// <summary>
        /// 最大腕力
        /// </summary>
        public short MaxHandWeight;

        public TAbility() { }

        public TAbility(byte[] buff)
        {
            Level = BitConverter.ToInt16(buff, 0);
            AC = BitConverter.ToUInt16(buff, 2);
            MAC = BitConverter.ToUInt16(buff, 6);
            DC = BitConverter.ToUInt16(buff, 10);
            MC = BitConverter.ToUInt16(buff, 14);
            SC = BitConverter.ToUInt16(buff, 18);
            HP = BitConverter.ToInt16(buff, 22);
            MP = BitConverter.ToInt16(buff, 24);
            MaxHP = BitConverter.ToInt16(buff, 26);
            MaxMP = BitConverter.ToInt16(buff, 28);
            Exp = BitConverter.ToInt32(buff, 30);
            MaxExp = BitConverter.ToInt32(buff, 34);
            Weight = BitConverter.ToInt16(buff, 38);
            MaxWeight = BitConverter.ToInt16(buff, 40);
            WearWeight = BitConverter.ToInt16(buff, 42);
            MaxWearWeight = BitConverter.ToInt16(buff, 44);
            HandWeight = BitConverter.ToInt16(buff, 46);
            MaxHandWeight = BitConverter.ToInt16(buff, 48);
        }


        public byte[] ToByte()
        {
            using (var memoryStream = new MemoryStream())
            {
                var backingStream = new BinaryWriter(memoryStream);
                backingStream.Write(Level);
                backingStream.Write(AC);
                backingStream.Write(MAC);
                backingStream.Write(DC);
                backingStream.Write(MC);
                backingStream.Write(SC);
                backingStream.Write(HP);
                backingStream.Write(MP);
                backingStream.Write(MaxHP);
                backingStream.Write(MaxMP);
                backingStream.Write(Exp);
                backingStream.Write(MaxExp);
                backingStream.Write(Weight);
                backingStream.Write(MaxWeight);
                backingStream.Write(WearWeight);
                backingStream.Write(MaxWearWeight);
                backingStream.Write(HandWeight);
                backingStream.Write(MaxHandWeight);
                var stream = backingStream.BaseStream as MemoryStream;
                return stream.ToArray();
            }

        }
    }

    public class TRecordHeader : Package
    {
        public string sAccount;
        public string sName;
        public int nSelectID;
        public double dCreateDate;
        public bool boDeleted;
        public double UpdateDate;
        public double CreateDate;

        public TRecordHeader() { }

        public TRecordHeader(byte[] buff)
            : base(buff)
        {
            this.sAccount = ReadPascalString(16);//BitConverter.ToString(buff, 0, 16);
            this.sName = ReadPascalString(20);//BitConverter.ToString(buff, 17, 37);
            this.nSelectID = ReadInt32();//BitConverter.ToInt32(buff, 38);
            this.dCreateDate = ReadDouble();//BitConverter.ToDouble(buff, 39);
            this.boDeleted = ReadBoolean(); //BitConverter.ToBoolean(buff, 48);
            this.UpdateDate = ReadDouble();//BitConverter.ToDouble(buff, 49);
            this.CreateDate = ReadDouble();//BitConverter.ToDouble(buff, 59);
        }

        public byte[] ToByte()
        {
            using (var memoryStream = new MemoryStream())
            {
                var backingStream = new BinaryWriter(memoryStream);

                backingStream.Write(sAccount.ToByte(17));
                backingStream.Write(sName.ToByte(21));
                backingStream.Write(nSelectID);
                backingStream.Write(dCreateDate);
                backingStream.Write(boDeleted);
                backingStream.Write(UpdateDate);
                backingStream.Write(CreateDate);

                var stream = backingStream.BaseStream as MemoryStream;
                return stream.ToArray();
            }
        }
    }

    public struct THumInfo
    {
        public bool boDeleted;
        public bool boSelected;
        public string[] sAccount;
        public DateTime dModDate;
        public string[] sChrName;
        public byte btCount;
        public TRecordHeader Header;
    }

    public class TUserItem : Package
    {
        public int MakeIndex;
        public short wIndex;
        public short Dura;
        public short DuraMax;
        public byte[] btValue;

        public TUserItem()
        {
            btValue = new byte[14];
        }

        public TUserItem(byte[] buffer)
            : base(buffer)
        {
            this.MakeIndex = ReadInt32();
            this.wIndex = ReadInt16();
            this.Dura = ReadInt16();
            this.DuraMax = ReadInt16();
            this.btValue = ReadBytes(14);
        }

        public byte[] ToByte()
        {
            using (var memoryStream = new MemoryStream())
            {
                var backingStream = new BinaryWriter(memoryStream);

                backingStream.Write(MakeIndex);
                backingStream.Write(wIndex);
                backingStream.Write(Dura);
                backingStream.Write(DuraMax);
                backingStream.Write(btValue);

                return (backingStream.BaseStream as MemoryStream).ToArray();
            }
        }
    }

    public class TMagicRcd
    {
        /// <summary>
        /// 技能ID
        /// </summary>
        public short wMagIdx;
        /// <summary>
        /// 等级
        /// </summary>
        public byte btLevel;
        /// <summary>
        /// 技能快捷键
        /// </summary>
        public byte btKey;
        /// <summary>
        /// 当前修练值
        /// </summary>
        public int nTranPoint;

        public byte[] ToByte()
        {
            using (var memoryStream = new MemoryStream())
            {
                var backingStream = new BinaryWriter(memoryStream);

                backingStream.Write(wMagIdx);
                backingStream.Write(btLevel);
                backingStream.Write(btKey);
                backingStream.Write(nTranPoint);

                return (backingStream.BaseStream as MemoryStream).ToArray();
            }
        }

        public TMagicRcd() { }

        public TMagicRcd(byte[] buff)
        {
            this.wMagIdx = BitConverter.ToInt16(buff, 0);
            this.btLevel = buff[2];
            this.btKey = buff[3];
            this.nTranPoint = BitConverter.ToInt16(buff, 4);
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public class TUserMagic
    {
        public TMagic MagicInfo;
        public byte btLevel;
        public short wMagIdx;
        public int nTranPoint;
        public byte btKey;

        public TUserMagic()
        {
            MagicInfo = new TMagic();
        }

        public byte[] ToByte()
        {
            using (var memoryStream = new MemoryStream())
            {
                var backingStream = new BinaryWriter(memoryStream);

                backingStream.Write(MagicInfo.ToByte());//76
                backingStream.Write(btLevel);
                backingStream.Write(wMagIdx);
                backingStream.Write(nTranPoint);
                backingStream.Write(btKey);

                return (backingStream.BaseStream as MemoryStream).ToArray();
            }
        }

    }

    /// <summary>
    /// Reads primitive data types from an array of binary data.
    /// </summary>
    public abstract class Package 
    {
        public readonly BinaryReader binaryReader;

        public Package() { }

        public Package(byte[] segment)
        {
            if (segment == null)
            {
                throw new Exception("segment is null");
            }
            binaryReader = new BinaryReader(new MemoryStream(segment));
        }

        public string ReadPascalString(int size)
        {
            var packegeLen = binaryReader.ReadByte();
            var strbuff = binaryReader.ReadBytes(size);
            return Encoding.Default.GetString(strbuff, 0, packegeLen);
        }

        public int ReadInt32()
        {
            return binaryReader.ReadInt32();
        }

        public short ReadInt16()
        {
            return binaryReader.ReadInt16();
        }

        public double ReadDouble()
        {
            return binaryReader.ReadDouble();
        }

        public byte ReadByte()
        {
            return binaryReader.ReadByte();
        }

        public bool ReadBoolean()
        {
            return binaryReader.ReadBoolean();
        }

        public byte[] ReadBytes(int size)
        {
            return binaryReader.ReadBytes(size);
        }

        public ushort[] ReadUInt16(int size)
        {
            var shortarr = new ushort[size];
            for (var i = 0; i < shortarr.Length; i++)
            {
                shortarr[i] = binaryReader.ReadUInt16();
            }
            return shortarr;
        }
    }

    public class THumInfoData : Package
    {
        public string sChrName;
        public string sCurMap;
        public short wCurX;
        public short wCurY;
        public byte btDir;
        public byte btHair;
        public byte btSex;
        public byte btJob;
        public int nGold;
        public TAbility Abil;
        public ushort[] wStatusTimeArr;
        public string sHomeMap;
        public short wHomeX;
        public short wHomeY;
        public TNakedAbility BonusAbil;
        public int nBonusPoint;
        public byte btCreditPoint;
        public byte btReLevel;
        public string sMasterName;
        public bool boMaster;
        public string sDearName;
        public string sStoragePwd;
        public int nGameGold;
        public int nGamePoint;
        public int nPayMentPoint;
        public int nPKPoint;
        public byte btAllowGroup;
        public byte btF9;
        public byte btAttatckMode;
        public byte btIncHealth;
        public byte btIncSpell;
        public byte btIncHealing;
        public byte btFightZoneDieCount;
        public byte btEE;
        public byte btEF;
        public string sAccount;
        public bool boLockLogon;
        public short wContribution;
        public int nHungerStatus;
        public bool boAllowGuildReCall;
        public short wGroupRcallTime;
        public double dBodyLuck;
        public bool boAllowGroupReCall;
        public byte[] QuestUnitOpen;
        public byte[] QuestUnit;
        public byte[] QuestFlag;
        public byte btMarryCount;
        public TUserItem[] HumItems;
        public TUserItem[] BagItems;
        public TUserItem[] StorageItems;
        public TMagicRcd[] Magic;

        public THumInfoData()
        {
            QuestUnitOpen = new byte[128];
            QuestUnit = new byte[128];
            QuestFlag = new byte[128];
            HumItems = new TUserItem[13];
            BagItems = new TUserItem[43];
            StorageItems = new TUserItem[50];
            Magic = new TMagicRcd[20];
            Abil = new TAbility();
            BonusAbil = new TNakedAbility();
        }

        public THumInfoData(byte[] buffer) 
            : base(buffer)
        {
            this.sChrName = ReadPascalString(14);
            this.sCurMap = ReadPascalString(16);
            this.wCurX = ReadInt16();
            this.wCurY = ReadInt16();
            this.btDir = ReadByte();
            this.btHair = ReadByte();
            this.btSex = ReadByte();
            this.btJob = ReadByte();
            this.nGold = ReadInt32();
            this.Abil = new TAbility(ReadBytes(50));
            this.wStatusTimeArr = ReadUInt16(12);
            this.sHomeMap = ReadPascalString(16);
            this.wHomeX = ReadInt16();
            this.wHomeY = ReadInt16();
            this.BonusAbil = new TNakedAbility(ReadBytes(24));
            this.nBonusPoint = ReadInt32();
            this.btCreditPoint = ReadByte();
            this.btReLevel = ReadByte();
            this.sMasterName = ReadPascalString(14);
            this.boMaster = ReadBoolean();
            this.sDearName = ReadPascalString(14);
            this.sStoragePwd = ReadPascalString(10);
            this.nGameGold = ReadInt32();
            this.nGamePoint = ReadInt32();
            this.nPayMentPoint = ReadInt32();
            this.nPKPoint = ReadInt32();
            this.btAllowGroup = ReadByte();
            this.btF9 = ReadByte();
            this.btAttatckMode = ReadByte();
            this.btIncHealth = ReadByte();
            this.btIncSpell = ReadByte();
            this.btIncHealing = ReadByte();
            this.btFightZoneDieCount = ReadByte();
            this.btEE = ReadByte();
            this.btEF = ReadByte();
            this.sAccount = ReadPascalString(16);
            this.boLockLogon = ReadBoolean();
            this.wContribution = ReadInt16();
            this.nHungerStatus = ReadInt32();
            this.boAllowGuildReCall = ReadBoolean();
            this.wGroupRcallTime = ReadInt16();
            this.dBodyLuck = ReadDouble();
            this.boAllowGroupReCall = ReadBoolean();
            this.QuestUnitOpen = ReadBytes(128);
            this.QuestUnit = ReadBytes(128);
            this.QuestFlag = ReadBytes(128);
            this.btMarryCount = ReadByte();

            this.HumItems = new TUserItem[13];
            var humItemBuff = ReadBytes(312);
            for (var i = 0; i < HumItems.Length; i++)
            {
                var itemBuff = new byte[24];
                Array.Copy(humItemBuff, i * 24, itemBuff, 0, itemBuff.Length);
                HumItems[i] = new TUserItem(itemBuff);
            }

            this.BagItems = new TUserItem[43];
            var bagItemBuff = ReadBytes(1032);

            for (var i = 0; i < BagItems.Length; i++)
            {
                var itemBuff = new byte[24];
                Buffer.BlockCopy(bagItemBuff, i * 24, itemBuff, 0, itemBuff.Length);
                BagItems[i] = new TUserItem(itemBuff);
            }

            this.Magic = new TMagicRcd[20];
            var hubMagicBuff = ReadBytes(160);
            for (var i = 0; i < Magic.Length; i++)
            {
                var itemBuff = new byte[8];
                Buffer.BlockCopy(hubMagicBuff, i * 8, itemBuff, 0, itemBuff.Length);
                Magic[i] = new TMagicRcd(itemBuff);
            }

            this.StorageItems = new TUserItem[50];
            var storageBuff = ReadBytes(1200);
            for (var i = 0; i < StorageItems.Length; i++)
            {
                var itemBuff = new byte[24];
                Buffer.BlockCopy(storageBuff, i * 24, itemBuff, 0, itemBuff.Length);
                StorageItems[i] = new TUserItem(itemBuff);
            }
        }

        public byte[] ToByte()
        {
            using (var memoryStream = new MemoryStream())
            {
                var backingStream = new BinaryWriter(memoryStream);

                backingStream.Write(sChrName.ToByte(15));
                backingStream.Write(sCurMap.ToByte(17));

                backingStream.Write(wCurX);
                backingStream.Write(wCurY);
                backingStream.Write(btDir);
                backingStream.Write(btHair);
                backingStream.Write(btSex);
                backingStream.Write(btJob);
                backingStream.Write(nGold);
                backingStream.Write(Abil.ToByte());//50
                for (var i = 0; i < wStatusTimeArr.Length; i++)
                {
                    backingStream.Write(wStatusTimeArr[i]); //24
                }
                backingStream.Write(sHomeMap.ToByte(17));
                backingStream.Write(wHomeX);
                backingStream.Write(wHomeY);
                backingStream.Write(BonusAbil.ToByte()); //24
                backingStream.Write(nBonusPoint);
                backingStream.Write(btCreditPoint);
                backingStream.Write(btReLevel);
                backingStream.Write(sMasterName.ToByte(15));
                backingStream.Write(boMaster);
                backingStream.Write(sDearName.ToByte(15));
                backingStream.Write(sStoragePwd.ToByte(11));
                backingStream.Write(nGameGold);
                backingStream.Write(nGamePoint);
                backingStream.Write(nPayMentPoint);
                backingStream.Write(nPKPoint);
                backingStream.Write(btAllowGroup);
                backingStream.Write(btF9);
                backingStream.Write(btAttatckMode);
                backingStream.Write(btIncHealth);
                backingStream.Write(btIncSpell);
                backingStream.Write(btIncHealing);
                backingStream.Write(btFightZoneDieCount);
                backingStream.Write(btEE);
                backingStream.Write(btEF);
                backingStream.Write(sAccount.ToByte(17));
                backingStream.Write(boLockLogon);
                backingStream.Write(wContribution);
                backingStream.Write(nHungerStatus);
                backingStream.Write(boAllowGuildReCall);
                backingStream.Write(wGroupRcallTime);
                backingStream.Write(dBodyLuck);
                backingStream.Write(boAllowGroupReCall);
                backingStream.Write(QuestUnitOpen);
                backingStream.Write(QuestUnit);
                backingStream.Write(QuestFlag);
                backingStream.Write(btMarryCount);
                var userItem = new TUserItem();
                for (var i = 0; i < HumItems.Length; i++)
                {
                    if (HumItems[i] == null)
                    {
                        backingStream.Write(userItem.ToByte());
                    }
                    else
                    {
                        backingStream.Write(HumItems[i].ToByte());
                    }
                }
                for (var i = 0; i < BagItems.Length; i++)
                {
                    if (BagItems[i] == null)
                    {
                        backingStream.Write(userItem.ToByte());
                    }
                    else
                    {
                        backingStream.Write(BagItems[i].ToByte());
                    }
                }
                var userMagic = new TMagicRcd();
                for (var i = 0; i < Magic.Length; i++)
                {
                    if (Magic[i] == null)
                    {
                        backingStream.Write(userMagic.ToByte());
                    }
                    else
                    {
                        backingStream.Write(Magic[i].ToByte());//16
                    }
                }
                for (var i = 0; i < StorageItems.Length; i++)
                {
                    if (StorageItems[i] == null)
                    {
                        backingStream.Write(userItem.ToByte());
                    }
                    else
                    {
                        backingStream.Write(StorageItems[i].ToByte());
                    }
                }
                var stream = backingStream.BaseStream as MemoryStream;
                return stream.ToArray();
            }
        }

    }


    public class THumDataInfo : Package
    {
        public TRecordHeader Header;
        public THumInfoData Data;

        public THumDataInfo()
        {
            Header = new TRecordHeader();
            Data = new THumInfoData();
        }

        public THumDataInfo(byte[] buff)
            : base(buff)
        {
            var hederBuff = ReadBytes(67);
            Header = new TRecordHeader(hederBuff);

            var bodyBuff = ReadBytes(3361);
            Data = new THumInfoData(bodyBuff);
        }

        public byte[] ToByte()
        {
            using (var memoryStream = new MemoryStream())
            {
                var backingStream = new BinaryWriter(memoryStream);

                backingStream.Write(Header.ToByte());
                backingStream.Write(Data.ToByte());

                var stream = backingStream.BaseStream as MemoryStream;
                return stream.ToArray();
            }
        }
    }

    public struct TMsgHeader
    {
        public uint dwCode;
        public int nSocket;
        public short wGSocketIdx;
        public short wIdent;
        public short wUserListIndex;
        public int nLength;
    }

    public class TMapItem
    {
        public int Id;
        public string Name;
        public short Looks;
        public byte AniCount;
        public int Reserved;
        public int Count;
        public object DropBaseObject;
        public object OfBaseObject;
        public int dwCanPickUpTick;
        public TUserItem UserItem;
    }

    public class TDoorStatus
    {
        public bool bo01;
        public int n04;
        public bool boOpened;
        public int dwOpenTick;
        public int nRefCount;
    }

    public class TDoorInfo
    {
        public int nX;
        public int nY;
        public TDoorStatus Status;
        public int n08;
    }

    public class TMapFlag
    {
        public bool boSAFE;
        public int nL;
        public int nNEEDSETONFlag;
        public int nNeedONOFF;
        public int nMUSICID;
        public bool boDarkness;
        public bool boDayLight;
        public bool boFightZone;
        public bool boFight3Zone;
        public bool boQUIZ;
        public bool boNORECONNECT;
        public string sNoReConnectMap;
        public bool boMUSIC;
        public bool boEXPRATE;
        public int nEXPRATE;
        public bool boPKWINLEVEL;
        public int nPKWINLEVEL;
        public bool boPKWINEXP;
        public int nPKWINEXP;
        public bool boPKLOSTLEVEL;
        public int nPKLOSTLEVEL;
        public bool boPKLOSTEXP;
        public int nPKLOSTEXP;
        public bool boDECHP;
        public int nDECHPPOINT;
        public int nDECHPTIME;
        public bool boINCHP;
        public int nINCHPPOINT;
        public int nINCHPTIME;
        public bool boDECGAMEGOLD;
        public int nDECGAMEGOLD;
        public int nDECGAMEGOLDTIME;
        public bool boDECGAMEPOINT;
        public int nDECGAMEPOINT;
        public int nDECGAMEPOINTTIME;
        public bool boINCGAMEGOLD;
        public int nINCGAMEGOLD;
        public int nINCGAMEGOLDTIME;
        public bool boINCGAMEPOINT;
        public int nINCGAMEPOINT;
        public int nINCGAMEPOINTTIME;
        public bool boRUNHUMAN;
        public bool boRUNMON;
        public bool boNEEDHOLE;
        public bool boNORECALL;
        public bool boNOGUILDRECALL;
        public bool boNODEARRECALL;
        public bool boNOMASTERRECALL;
        public bool boNORANDOMMOVE;
        public bool boNODRUG;
        public bool boMINE;
        public bool boMINE2;
        public bool boNOPOSITIONMOVE;
        public bool boNODROPITEM;
        public bool boNOTHROWITEM;
        public bool boNOHORSE;
        public bool boNOCHAT;
        public bool boKILLFUNC;
        public int nKILLFUNCNO;
        public bool boNOHUMNOMON;
    } 

    public class TAddAbility
    {
        public short wHP;
        public short wMP;
        public short wHitPoint;
        public short wSpeedPoint;
        public int wAC;
        public int wMAC;
        public int wDC;
        public int wMC;
        public int wSC;
        public short wAntiPoison;
        public short wPoisonRecover;
        public short wHealthRecover;
        public short wSpellRecover;
        public short wAntiMagic;
        public byte btLuck;
        public byte btUnLuck;
        public byte btWeaponStrong;
        public short nHitSpeed;
        public byte btUndead;
        public short Weight;
        public short WearWeight;
        public short HandWeight;

        public TAddAbility()
        { }
    } 

    public class TProcessMessage
    {
        public int wIdent;
        public int wParam;
        public int nParam1;
        public int nParam2;
        public int nParam3;
        public int dwDeliveryTime;
        public int BaseObject;
        public bool boLateDelivery;
        public string sMsg;
    }

    public class TSessInfo
    {
        public int nSessionID;
        public string sAccount;
        public string sIPaddr;
        public int nPayMent;
        public int nPayMode;
        public int nSessionStatus;
        public int dwStartTick;
        public int dwActiveTick;
        public int nRefCount;
        public int nSocket;
        public int nGateIdx;
        public int nGSocketIdx;
        public int dwNewUserTick;
        public int nSoftVersionDate;
    }

    public struct TScriptQuestInfo
    {
        public short wFlag;
        public byte btValue;
        public int nRandRage;
    } 

    public class TScript
    {
        public bool boQuest;
        public TScriptQuestInfo[] QuestInfo;
        //public IList<TSayingRecord> RecordList;
        public Dictionary<string, TSayingRecord> RecordList;
        public int nQuest;
    }

    public struct TGameCmd
    {
        public string sCmd;
        public int nPerMissionMin;
        public int nPerMissionMax;
    }

    public class TLoadDBInfo
    {
        public int nGateIdx;
        public int nSocket;
        public string sAccount;
        public string sCharName;
        public int nSessionID;
        public string sIPaddr;
        public int nSoftVersionDate;
        public int nPayMent;
        public int nPayMode;
        public int nGSocketIdx;
        public int dwNewUserTick;
        public object PlayObject;
        public int nReLoadCount;
    }

    public class TGoldChangeInfo
    {
        public string sGameMasterName;
        public string sGetGoldUser;
        public int nGold;
    }

    public class TSaveRcd
    {
        public string sAccount;
        public string sChrName;
        public int nSessionID;
        public TPlayObject PlayObject;
        public THumDataInfo HumanRcd;
        public int nReTryCount;

        public TSaveRcd()
        {
            HumanRcd = new THumDataInfo();
        }
    }

    public class TStartPoint
    {
        public string m_sMapName;
        public short m_nCurrX;
        public short m_nCurrY;
        public bool m_boNotAllowSay;
        public int m_nRange;
        public int m_nType;
        public int m_nPkZone;
        public int m_nPkFire;
        public byte m_btShape;
    } 

    public class TMonGenInfo
    {
        public string sMapName;
        public int nX;
        public int nY;
        public string sMonName;
        public int nRange;
        public int nCount;
        public int dwZenTime;
        public int nMissionGenRate;
        public IList<TBaseObject> CertList;
        public int CertCount;
        public object Envir;
        public int nRace;
        public int dwStartTick;
    }

    public struct TMonInfo
    {
        public IList<TMonItem> ItemList;
        public string sName;
        public byte btRace;
        public byte btRaceImg;
        public short wAppr;
        public short wLevel;
        public byte btLifeAttrib;
        public short wCoolEye;
        public int dwExp;
        public short wHP;
        public short wMP;
        public short wAC;
        public short wMAC;
        public short wDC;
        public short wMaxDC;
        public short wMC;
        public short wSC;
        public short wSpeed;
        public short wHitPoint;
        public short wWalkSpeed;
        public short wWalkStep;
        public short wWalkWait;
        public short wAttackSpeed;
        public short wAntiPush;
        public bool boAggro;
        public bool boTame;
    } 

    public class TMonItem
    {
        public int MaxPoint;
        public int SelPoint;
        public string ItemName;
        public int Count;
    } 

    public struct TUnbindInfo
    {
        public int nUnbindCode;
        public string sItemName;
    }

    public struct TSlaveInfo
    {
        public string sSlaveName;
        public byte btSlaveLevel;
        public int dwRoyaltySec;
        public int nKillCount;
        public byte btSalveLevel;
        public byte btSlaveExpLevel;
        public int nHP;
        public int nMP;
    }

    public class TSwitchDataInfo
    {
        public string sMap;
        public short wX;
        public short wY;
        public TAbility Abil;
        public string sChrName;
        public int nCode;
        public bool boC70;
        public bool boBanShout;
        public bool boHearWhisper;
        public bool boBanGuildChat;
        public bool boAdminMode;
        public bool boObMode;
        public IList<string> BlockWhisperArr;
        public TSlaveInfo[] SlaveArr;
        public short[] StatusValue;
        public int[] StatusTimeOut;
        public int dwWaitTime;
    }

    public struct TIPaddr
    {
        public string sIpaddr;
        public string dIPaddr;
    }

    public class TGateInfo
    {
        public bool boUsed;
        public Socket Socket;
        public string SocketId;
        public string sAddr;
        public int nPort;
        public int n520;
        public IList<TGateUserInfo> UserList;
        public int nUserCount;
        public IntPtr Buffer;
        public int nBuffLen;
        public IList<IntPtr> BufferList;
        public bool boSendKeepAlive;
        public int nSendChecked;
        public int nSendBlockCount;
        public int dwStartTime;
        public int nSendMsgCount;
        public int nSendRemainCount;
        public int dwSendTick;
        public int nSendMsgBytes;
        public int nSendBytesCount;
        public int nSendedMsgCount;
        public int nSendCount;
        public int dwSendCheckTick;
    } 

    public class TGateUserInfo
    {
        public object PlayObject;
        public int nSessionID;
        public string sAccount;
        public int nGSocketIdx;
        public string sIPaddr;
        public bool boCertification;
        public string sCharName;
        public int nClientVersion;
        public TSessInfo SessInfo;
        public int nSocket;
        public object FrontEngine;
        public object UserEngine;
        public int dwNewUserTick;
    } 

    public struct TClientConf
    {
        public bool boClientCanSet;
        public bool boRunHuman;
        public bool boRunMon;
        public bool boRunNpc;
        public bool boWarRunAll;
        public byte btDieColor;
        public int wSpellTime;
        public int wHitIime;
        public int wItemFlashTime;
        public byte btItemSpeed;
        public bool boCanStartRun;
        public bool boParalyCanRun;
        public bool boParalyCanWalk;
        public bool boParalyCanHit;
        public bool boParalyCanSpell;
        public bool boShowRedHPLable;
        public bool boShowHPNumber;
        public bool boShowJobLevel;
        public bool boDuraAlert;
        public bool boMagicLock;
        public bool boAutoPuckUpItem;

        public byte[] ToByte()
        {
            using (var memoryStream = new MemoryStream())
            {
                var backingStream = new BinaryWriter(memoryStream);

                backingStream.Write(boClientCanSet);
                backingStream.Write(boRunHuman);
                backingStream.Write(boRunMon);
                backingStream.Write(boRunNpc);
                backingStream.Write(boWarRunAll);
                backingStream.Write(btDieColor);
                backingStream.Write(wSpellTime);
                backingStream.Write(wHitIime);
                backingStream.Write(wItemFlashTime);
                backingStream.Write(btItemSpeed);
                backingStream.Write(boCanStartRun);
                backingStream.Write(boParalyCanRun);
                backingStream.Write(boParalyCanWalk);
                backingStream.Write(boParalyCanHit);
                backingStream.Write(boParalyCanSpell);
                backingStream.Write(boShowRedHPLable);
                backingStream.Write(boShowHPNumber);
                backingStream.Write(boShowJobLevel);
                backingStream.Write(boDuraAlert);
                backingStream.Write(boMagicLock);
                backingStream.Write(boAutoPuckUpItem);

                var stream = backingStream.BaseStream as MemoryStream;
                return stream.ToArray();
            }
        }
    } 

    public struct TRecallMigic
    {
        public int nHumLevel;
        public string sMonName;
        public int nCount;
        public int nLevel;
    }

    public class TGameCommand
    {
        public TGameCmd DATA;
        public TGameCmd PRVMSG;
        public TGameCmd ALLOWMSG;
        public TGameCmd LETSHOUT;
        public TGameCmd LETTRADE;
        public TGameCmd LETGUILD;
        public TGameCmd ENDGUILD;
        public TGameCmd BANGUILDCHAT;
        public TGameCmd AUTHALLY;
        public TGameCmd AUTH;
        public TGameCmd AUTHCANCEL;
        public TGameCmd DIARY;
        public TGameCmd USERMOVE;
        public TGameCmd SEARCHING;
        public TGameCmd ALLOWGROUPCALL;
        public TGameCmd GROUPRECALLL;
        public TGameCmd ALLOWGUILDRECALL;
        public TGameCmd GUILDRECALLL;
        public TGameCmd UNLOCKSTORAGE;
        public TGameCmd UNLOCK;
        public TGameCmd __LOCK;
        public TGameCmd PASSWORDLOCK;
        public TGameCmd SETPASSWORD;
        public TGameCmd CHGPASSWORD;
        public TGameCmd CLRPASSWORD;
        public TGameCmd UNPASSWORD;
        public TGameCmd MEMBERFUNCTION;
        public TGameCmd MEMBERFUNCTIONEX;
        public TGameCmd DEAR;
        public TGameCmd ALLOWDEARRCALL;
        public TGameCmd DEARRECALL;
        public TGameCmd MASTER;
        public TGameCmd ALLOWMASTERRECALL;
        public TGameCmd MASTERECALL;
        public TGameCmd ATTACKMODE;
        public TGameCmd REST;
        public TGameCmd TAKEONHORSE;
        public TGameCmd TAKEOFHORSE;
        public TGameCmd HUMANLOCAL;
        public TGameCmd MOVE;
        public TGameCmd POSITIONMOVE;
        public TGameCmd INFO;
        public TGameCmd MOBLEVEL;
        public TGameCmd MOBCOUNT;
        public TGameCmd HUMANCOUNT;
        public TGameCmd MAP;
        public TGameCmd KICK;
        public TGameCmd TING;
        public TGameCmd SUPERTING;
        public TGameCmd MAPMOVE;
        public TGameCmd SHUTUP;
        public TGameCmd RELEASESHUTUP;
        public TGameCmd SHUTUPLIST;
        public TGameCmd GAMEMASTER;
        public TGameCmd OBSERVER;
        public TGameCmd SUEPRMAN;
        public TGameCmd LEVEL;
        public TGameCmd SABUKWALLGOLD;
        public TGameCmd RECALL;
        public TGameCmd REGOTO;
        public TGameCmd SHOWFLAG;
        public TGameCmd SHOWOPEN;
        public TGameCmd SHOWUNIT;
        public TGameCmd ATTACK;
        public TGameCmd MOB;
        public TGameCmd MOBNPC;
        public TGameCmd DELNPC;
        public TGameCmd NPCSCRIPT;
        public TGameCmd RECALLMOB;
        public TGameCmd LUCKYPOINT;
        public TGameCmd LOTTERYTICKET;
        public TGameCmd RELOADGUILD;
        public TGameCmd RELOADLINENOTICE;
        public TGameCmd RELOADABUSE;
        public TGameCmd BACKSTEP;
        public TGameCmd BALL;
        public TGameCmd FREEPENALTY;
        public TGameCmd PKPOINT;
        public TGameCmd INCPKPOINT;
        public TGameCmd CHANGELUCK;
        public TGameCmd HUNGER;
        public TGameCmd HAIR;
        public TGameCmd TRAINING;
        public TGameCmd DELETESKILL;
        public TGameCmd CHANGEJOB;
        public TGameCmd CHANGEGENDER;
        public TGameCmd NAMECOLOR;
        public TGameCmd MISSION;
        public TGameCmd MOBPLACE;
        public TGameCmd TRANSPARECY;
        public TGameCmd DELETEITEM;
        public TGameCmd LEVEL0;
        public TGameCmd CLEARMISSION;
        public TGameCmd SETFLAG;
        public TGameCmd SETOPEN;
        public TGameCmd SETUNIT;
        public TGameCmd RECONNECTION;
        public TGameCmd DISABLEFILTER;
        public TGameCmd CHGUSERFULL;
        public TGameCmd CHGZENFASTSTEP;
        public TGameCmd CONTESTPOINT;
        public TGameCmd STARTCONTEST;
        public TGameCmd ENDCONTEST;
        public TGameCmd ANNOUNCEMENT;
        public TGameCmd OXQUIZROOM;
        public TGameCmd GSA;
        public TGameCmd CHANGEITEMNAME;
        public TGameCmd DISABLESENDMSG;
        public TGameCmd ENABLESENDMSG;
        public TGameCmd DISABLESENDMSGLIST;
        public TGameCmd KILL;
        public TGameCmd MAKE;
        public TGameCmd SMAKE;
        public TGameCmd BONUSPOINT;
        public TGameCmd DELBONUSPOINT;
        public TGameCmd RESTBONUSPOINT;
        public TGameCmd FIREBURN;
        public TGameCmd TESTFIRE;
        public TGameCmd TESTSTATUS;
        public TGameCmd DELGOLD;
        public TGameCmd ADDGOLD;
        public TGameCmd DELGAMEGOLD;
        public TGameCmd ADDGAMEGOLD;
        public TGameCmd GAMEGOLD;
        public TGameCmd GAMEPOINT;
        public TGameCmd CREDITPOINT;
        public TGameCmd TESTGOLDCHANGE;
        public TGameCmd REFINEWEAPON;
        public TGameCmd RELOADADMIN;
        public TGameCmd RELOADNPC;
        public TGameCmd RELOADMANAGE;
        public TGameCmd RELOADROBOTMANAGE;
        public TGameCmd RELOADROBOT;
        public TGameCmd RELOADMONITEMS;
        public TGameCmd RELOADDIARY;
        public TGameCmd RELOADITEMDB;
        public TGameCmd RELOADMAGICDB;
        public TGameCmd RELOADMONSTERDB;
        public TGameCmd RELOADMINMAP;
        public TGameCmd REALIVE;
        public TGameCmd ADJUESTLEVEL;
        public TGameCmd ADJUESTEXP;
        public TGameCmd ADDGUILD;
        public TGameCmd DELGUILD;
        public TGameCmd CHANGESABUKLORD;
        public TGameCmd FORCEDWALLCONQUESTWAR;
        public TGameCmd ADDTOITEMEVENT;
        public TGameCmd ADDTOITEMEVENTASPIECES;
        public TGameCmd ITEMEVENTLIST;
        public TGameCmd STARTINGGIFTNO;
        public TGameCmd DELETEALLITEMEVENT;
        public TGameCmd STARTITEMEVENT;
        public TGameCmd ITEMEVENTTERM;
        public TGameCmd ADJUESTTESTLEVEL;
        public TGameCmd TRAININGSKILL;
        public TGameCmd OPDELETESKILL;
        public TGameCmd CHANGEWEAPONDURA;
        public TGameCmd RELOADGUILDALL;
        public TGameCmd WHO;
        public TGameCmd TOTAL;
        public TGameCmd TESTGA;
        public TGameCmd MAPINFO;
        public TGameCmd SBKDOOR;
        public TGameCmd CHANGEDEARNAME;
        public TGameCmd CHANGEMASTERNAME;
        public TGameCmd STARTQUEST;
        public TGameCmd SETPERMISSION;
        public TGameCmd CLEARMON;
        public TGameCmd RENEWLEVEL;
        public TGameCmd DENYIPLOGON;
        public TGameCmd DENYACCOUNTLOGON;
        public TGameCmd DENYCHARNAMELOGON;
        public TGameCmd DELDENYIPLOGON;
        public TGameCmd DELDENYACCOUNTLOGON;
        public TGameCmd DELDENYCHARNAMELOGON;
        public TGameCmd SHOWDENYIPLOGON;
        public TGameCmd SHOWDENYACCOUNTLOGON;
        public TGameCmd SHOWDENYCHARNAMELOGON;
        public TGameCmd VIEWWHISPER;
        public TGameCmd SPIRIT;
        public TGameCmd SPIRITSTOP;
        public TGameCmd SETMAPMODE;
        public TGameCmd SHOWMAPMODE;
        public TGameCmd TESTSERVERCONFIG;
        public TGameCmd SERVERSTATUS;
        public TGameCmd TESTGETBAGITEM;
        public TGameCmd CLEARBAG;
        public TGameCmd SHOWUSEITEMINFO;
        public TGameCmd BINDUSEITEM;
        public TGameCmd MOBFIREBURN;
        public TGameCmd TESTSPEEDMODE;
        public TGameCmd LOCKLOGON;

        public TGameCommand()
        {
    //        {
    //            { "Date", 0, 10} , 
    //{ "PrvMsg", 0, 10} , 
    //{ "AllowMsg", 0, 10} , 
    //{ "LetShout", 0, 10} , 
    //{ "LetTrade", 0, 10} , 
    //{ "LetGuild", 0, 10} , 
    //{ "EndGuild", 0, 10} , 
    //{ "BanGuildChat", 0, 10} , 
    //{ "AuthAlly", 0, 10} , 
    //{ "联盟", 0, 10} , 
    //{ "取消联盟", 0, 10} , 
    //{ "Diary", 0, 10} , 
    //{ "Move", 0, 10} , 
    //{ "Searching", 0, 10} , 
    //{ "AllowGroupRecall", 0, 10} , 
    //{ "GroupRecall", 0, 10} , 
    //{ "AllowGuildRecall", 0, 10} , 
    //{ "GuildRecall", 0, 10} , 
    //{ "UnLockStorage", 0, 10} , 
    //{ "UnLock", 0, 10} , 
    //{ "Lock", 0, 10} , 
    //{ "PasswordLock", 0, 10} , 
    //{ "SetPassword", 0, 10} , 
    //{ "ChgPassword", 0, 10} , 
    //{ "ClrPassword", 10, 10} , 
    //{ "UnPassword", 0, 10} , 
    //{ "MemberFunc", 0, 10} , 
    //{ "MemberFuncEx", 0, 10} , 
    //{ "Dear", 0, 10} , 
    //{ "AllowDearRecall", 0, 10} , 
    //{ "DearRecall", 0, 10} , 
    //{ "Master", 0, 10} , 
    //{ "AllowMasterRecall", 0, 10} , 
    //{ "MasterRecall", 0, 10} , 
    //{ "AttackMode", 0, 10} , 
    //{ "Rest", 0, 10} , 
    //{ "OnHorse", 0, 10} , 
    //{ "OffHorse", 0, 10} , 
    //{ "HumanLocal", 3, 10} , 
    //{ "Move", 3, 6} , 
    //{ "PositionMove", 3, 6} , 
    //{ "Info", 3, 10} , 
    //{ "MobLevel", 3, 10} , 
    //{ "MobCount", 3, 10} , 
    //{ "HumanCount", 3, 10} , 
    //{ "Map", 3, 10} , 
    //{ "Kick", 10, 10} , 
    //{ "Ting", 10, 10} , 
    //{ "SuperTing", 10, 10} , 
    //{ "MapMove", 10, 10} , 
    //{ "Shutup", 10, 10} , 
    //{ "ReleaseShutup", 10, 10} , 
    //{ "ShutupList", 10, 10} , 
    //{ "GameMaster", 10, 10} , 
    //{ "Observer", 10, 10} , 
    //{ "Superman", 10, 10} , 
    //{ "Level", 10, 10} , 
    //{ "SabukWallGold", 10, 10} , 
    //{ "Recall", 10, 10} , 
    //{ "ReGoto", 10, 10} , 
    //{ "showflag", 10, 10} , 
    //{ "showopen", 10, 10} , 
    //{ "showunit", 10, 10} , 
    //{ "Attack", 10, 10} , 
    //{ "Mob", 10, 10} , 
    //{ "MobNpc", 10, 10} , 
    //{ "DelNpc", 10, 10} , 
    //{ "NpcScript", 10, 10} , 
    //{ "RecallMob", 10, 10} , 
    //{ "LuckyPoint", 10, 10} , 
    //{ "LotteryTicket", 10, 10} , 
    //{ "ReloadGuild", 10, 10} , 
    //{ "ReloadLineNotice", 10, 10} , 
    //{ "ReloadAbuse", 10, 10} , 
    //{ "Backstep", 10, 10} , 
    //{ "Ball", 10, 10} , 
    //{ "FreePK", 10, 10} , 
    //{ "PKpoint", 10, 10} , 
    //{ "IncPkPoint", 10, 10} , 
    //{ "ChangeLuck", 10, 10} , 
    //{ "Hunger", 10, 10} , 
    //{ "hair", 10, 10} , 
    //{ "Training", 10, 10} , 
    //{ "DeleteSkill", 10, 10} , 
    //{ "ChangeJob", 10, 10} , 
    //{ "ChangeGender", 10, 10} , 
    //{ "NameColor", 10, 10} , 
    //{ "Mission", 10, 10} , 
    //{ "MobPlace", 10, 10} , 
    //{ "Transparency", 10, 10} , 
    //{ "DeleteItem", 10, 10} , 
    //{ "Level0", 10, 10} , 
    //{ "ClearMission", 10, 10} , 
    //{ "setflag", 10, 10} , 
    //{ "setopen", 10, 10} , 
    //{ "setunit", 10, 10} , 
    //{ "Reconnection", 10, 10} , 
    //{ "DisableFilter", 10, 10} , 
    //{ "CHGUSERFULL", 10, 10} , 
    //{ "CHGZENFASTSTEP", 10, 10} , 
    //{ "ContestPoint", 10, 10} , 
    //{ "StartContest", 10, 10} , 
    //{ "EndContest", 10, 10} , 
    //{ "Announcement", 10, 10} , 
    //{ "OXQuizRoom", 10, 10} , 
    //{ "gsa", 10, 10} , 
    //{ "ChangeItemName", 10, 10} , 
    //{ "DisableSendMsg", 10, 10} , 
    //{ "EnableSendMsg", 10, 10} , 
    //{ "DisableSendMsgList", 10, 10} , 
    //{ "Kill", 10, 10} , 
    //{ "make", 10, 10} , 
    //{ "Supermake", 10, 10} , 
    //{ "BonusPoint", 10, 10} , 
    //{ "DelBonusPoint", 10, 10} , 
    //{ "RestBonusPoint", 10, 10} , 
    //{ "FireBurn", 10, 10} , 
    //{ "TestFire", 10, 10} , 
    //{ "TestStatus", 10, 10} , 
    //{ "DelGold", 10, 10} , 
    //{ "AddGold", 10, 10} , 
    //{ "DelGamePoint", 10, 10} , 
    //{ "AddGamePoint", 10, 10} , 
    //{ "GameGold", 10, 10} , 
    //{ "GamePoint", 10, 10} , 
    //{ "CreditPoint", 10, 10} , 
    //{ "Test_GOLD_Change", 10, 10} , 
    //{ "RefineWeapon", 10, 10} , 
    //{ "ReloadAdmin", 10, 10} , 
    //{ "ReloadNpc", 10, 10} , 
    //{ "ReloadManage", 10, 10} , 
    //{ "ReloadRobotManage", 10, 10} , 
    //{ "ReloadRobot", 10, 10} , 
    //{ "ReloadMonItems", 10, 10} , 
    //{ "ReloadDiary", 10, 10} , 
    //{ "ReloadItemDB", 10, 10} , 
    //{ "ReloadMagicDB", 10, 10} , 
    //{ "ReloadMonsterDB", 10, 10} , 
    //{ "ReLoadMinMap", 10, 10} , 
    //{ "ReAlive", 10, 10} , 
    //{ "AdjustLevel", 10, 10} , 
    //{ "AdjustExp", 10, 10} , 
    //{ "AddGuild", 10, 10} , 
    //{ "DelGuild", 10, 10} , 
    //{ "ChangeSabukLord", 10, 10} , 
    //{ "ForcedWallconquestWar", 10, 10} , 
    //{ "AddToItemEvent", 10, 10} , 
    //{ "AddToItemEventAsPieces", 10, 10} , 
    //{ "ItemEventList", 10, 10} , 
    //{ "StartingGiftNo", 10, 10} , 
    //{ "DeleteAllItemEvent", 10, 10} , 
    //{ "StartItemEvent", 10, 10} , 
    //{ "ItemEventTerm", 10, 10} , 
    //{ "AdjustTestLevel", 10, 10} , 
    //{ "TrainingSkill", 10, 10} , 
    //{ "OPDeleteSkill", 10, 10} , 
    //{ "ChangeWeaponDura", 10, 10} , 
    //{ "ReloadGuildAll", 10, 10} , 
    //{ "Who ", 3, 10} , 
    //{ "Total ", 5, 10} , 
    //{ "Testga", 10, 10} , 
    //{ "MapInfo", 10, 10} , 
    //{ "SbkDoor", 10, 10} , 
    //{ "DearName", 10, 10} , 
    //{ "MasterName", 10, 10} , 
    //{ "StartQuest", 10, 10} , 
    //{ "SetPermission", 10, 10} , 
    //{ "ClearMon", 10, 10} , 
    //{ "ReNewLevel", 10, 10} , 
    //{ "DenyIPLogon", 10, 10} , 
    //{ "DenyAccountLogon", 10, 10} , 
    //{ "DenyCharNameLogon", 10, 10} , 
    //{ "DelDenyIPLogon", 10, 10} , 
    //{ "DelDenyAccountLogon", 10, 10} , 
    //{ "DelDenyCharNameLogon", 10, 10} , 
    //{ "ShowDenyIPLogon", 10, 10} , 
    //{ "ShowDenyAccountLogon", 10, 10} , 
    //{ "ShowDenyCharNameLogon", 10, 10} , 
    //{ "ViewWhisper", 10, 10} , 
    //{ "祈祷生效", 10, 10} , 
    //{ "停止叛变", 10, 10} , 
    //{ "SetMapMode", 10, 10} , 
    //{ "ShowMapMode", 10, 10} , 
    //{ "TestServerConfig", 10, 10} , 
    //{ "ServerStatus", 10, 10} , 
    //{ "TestGetBagItem", 10, 10} , 
    //{ "ClearBag", 10, 10} , 
    //{ "ShowUseItemInfo", 10, 10} , 
    //{ "BindUseItem", 10, 10} , 
    //{ "MobFireBurn", 10, 10} , 
    //{ "TestSpeedMode", 10, 10} , 
    //{ "LockLogin", 0, 0}
    //        }
        }
    }
        
    public struct TAdminInfo
    {
        public int nLv;
        public string sChrName;
        public string sIPaddr;
    } 

    public class TMonDrop
    {
        public string sItemName;
        public int nDropCount;
        public int nNoDropCount;
        public int nCountLimit;
    }

    public struct TMonSayMsg
    {
        public TMonStatus State;
        public TMsgColor Color;
        public int nRate;
        public string sSayMsg;
    } 

    public class TDynamicVar
    {
        public string sName;
        public TVarType VarType;
        public int nInternet;
        public string sString;
    }

    public class TItemName
    {
        public int nMakeIndex;
        public int nItemIndex;
        public string sItemName = string.Empty;
    }

    public class TLoadHuman
    {
        public string sAccount;
        public string sChrName;
        public string sUserAddr;
        public int nSessionID;

        public byte[] ToByte()
        {
            using (var memoryStream = new MemoryStream())
            {
                var backingStream = new BinaryWriter(memoryStream);
                backingStream.Write(sAccount.ToByte(17));
                backingStream.Write(sChrName.ToByte(21));
                backingStream.Write(sUserAddr.ToByte(18));
                backingStream.Write(nSessionID);
                var stream = backingStream.BaseStream as MemoryStream;
                return stream.ToArray();
            }
        }
    }

    public struct TSrvNetInfo
    {
        public string sIPaddr;
        public int nPort;
    }

    public class TUserOpenInfo
    {
        public string sChrName;
        public TLoadDBInfo LoadUser;
        public THumDataInfo HumanRcd;
    }

    public class TGateObj
    {
        public TEnvirnoment DEnvir;
        public short nDMapX;
        public short nDMapY;
        public bool boFlag;
    }

    public struct TMapQuestInfo
    {
        public int nFlag;
        public int nValue;
        public string sMonName;
        public string sItemName;
        public bool boGrouped;
        public object NPC;
    } 

    public enum TMsgColor
    {
        c_Red,
        c_Green,
        c_Blue,
        c_White
    }

    public enum TMsgType
    {
        t_System,
        t_Notice,
        t_Hint,
        t_Say,
        t_Castle,
        t_Cust,
        t_GM,
        t_Mon
    }

    public enum TMonStatus
    {
        s_KillHuman,
        s_UnderFire,
        s_Die,
        s_MonGen
    }

    public enum TVarType
    {
        vNone,
        VInteger,
        VString
    }
}

namespace M2Server
{
    public class grobal2
    {
        public const string VERSION_NUMBER_STR = "当前版本：20161001";
        public const int VERSION_NUMBER = 20020522;
        public const int CLIENT_VERSION_NUMBER = 120040918;
        public const int CM_POWERBLOCK = 0;
        // Damian
        public const int MapNameLen = 16;
        public const int ActorNameLen = 14;
        public const int DR_UP = 0;
        public const int DR_UPRIGHT = 1;
        public const int DR_RIGHT = 2;
        public const int DR_DOWNRIGHT = 3;
        public const int DR_DOWN = 4;
        public const int DR_DOWNLEFT = 5;
        public const int DR_LEFT = 6;
        public const int DR_UPLEFT = 7;
        /// <summary>
        /// 衣服
        /// </summary>
        public const int U_DRESS = 0;
        /// <summary>
        /// 武器
        /// </summary>
        public const int U_WEAPON = 1;
        public const int U_RIGHTHAND = 2;
        public const int U_NECKLACE = 3;
        public const int U_HELMET = 4;
        public const int U_ARMRINGL = 5;
        public const int U_ARMRINGR = 6;
        public const int U_RINGL = 7;
        public const int U_RINGR = 8;
        public const int U_BUJUK = 9;
        public const int U_BELT = 10;
        public const int U_BOOTS = 11;
        public const int U_CHARM = 12;
        public const int DEFBLOCKSIZE = 16;
        public const int BUFFERSIZE = 10000;
        public const int LOGICALMAPUNIT = 40;
        public const int UNITX = 48;
        public const int UNITY = 32;
        public const int HALFX = 24;
        public const int HALFY = 16;
        public const int MAXBAGITEM = 52;
        public const int HOWMANYMAGICS = 20;
        public const int USERITEMMAX = 46;
        // 用户最大的物品
        public const int MaxSkillLevel = 3;
        public const int MAX_STATUS_ATTRIBUTE = 12;
        // 物品类型(物品属性读取)
        public const int ITEM_WEAPON = 0;
        // 武器
        public const int ITEM_ARMOR = 1;
        // 装备
        public const int ITEM_ACCESSORY = 2;
        // 辅助物品
        public const int ITEM_ETC = 3;
        // 其它物品
        public const int ITEM_LEECHDOM = 4;
        // 药水
        public const int ITEM_GOLD = 10;
        // 金币
        public const int POISON_DECHEALTH = 0;
        // 中毒类型 - 绿毒
        public const int POISON_DAMAGEARMOR = 1;
        // 中毒类型 - 红毒
        public const int POISON_LOCKSPELL = 2;
        public const int POISON_DONTMOVE = 4;
        public const int POISON_STONE = 5;
        public const int POISON_68 = 68;
        public const int STATE_TRANSPARENT = 8;
        public const int STATE_DEFENCEUP = 9;
        public const int STATE_MAGDEFENCEUP = 10;
        public const int STATE_BUBBLEDEFENCEUP = 11;
        public const int STATE_STONE_MODE = 0x00000001;
        public const int STATE_OPENHEATH = 0x00000002;
        // 眉仿 傍俺惑怕
        public const int ET_DIGOUTZOMBI = 1;
        // 粱厚啊 顶颇绊 唱柯 如利
        public const int ET_MINE = 2;
        // 堡籍捞 概厘登绢 乐澜
        public const int ET_PILESTONES = 3;
        // 倒公歹扁
        public const int ET_HOLYCURTAIN = 4;
        // 搬拌
        public const int ET_FIRE = 5;
        public const int ET_SCULPEICE = 6;
        // 林付空狼 倒柄柳 炼阿
        public const int RCC_MERCHANT = 50;
        public const int RCC_GUARD = 12;
        public const int RCC_USERHUMAN = 0;
        public const int CM_QUERYUSERSTATE = 82;
        public const int CM_QUERYUSERNAME = 80;
        public const int CM_QUERYBAGITEMS = 81;
        public const int CM_QUERYCHR = 100;
        public const int CM_NEWCHR = 101;
        public const int CM_DELCHR = 102;
        public const int CM_SELCHR = 103;
        public const int CM_SELECTSERVER = 104;
        public const int CM_OPENDOOR = 1002;
        public const int CM_SOFTCLOSE = 1009;
        public const int CM_DROPITEM = 1000;
        public const int CM_PICKUP = 1001;
        public const int CM_TAKEONITEM = 1003;
        public const int CM_TAKEOFFITEM = 1004;
        public const int CM_1005 = 1005;
        public const int CM_EAT = 1006;
        public const int CM_BUTCH = 1007;
        public const int CM_MAGICKEYCHANGE = 1008;
        public const int CM_CLICKNPC = 1010;
        public const int CM_MERCHANTDLGSELECT = 1011;
        public const int CM_MERCHANTQUERYSELLPRICE = 1012;
        public const int CM_USERSELLITEM = 1013;
        public const int CM_USERBUYITEM = 1014;
        public const int CM_USERGETDETAILITEM = 1015;
        public const int CM_DROPGOLD = 1016;
        public const int CM_1017 = 1017;
        public const int CM_LOGINNOTICEOK = 1018;
        public const int CM_GROUPMODE = 1019;
        public const int CM_CREATEGROUP = 1020;
        public const int CM_ADDGROUPMEMBER = 1021;
        public const int CM_DELGROUPMEMBER = 1022;
        public const int CM_USERREPAIRITEM = 1023;
        public const int CM_MERCHANTQUERYREPAIRCOST = 1024;
        public const int CM_DEALTRY = 1025;
        public const int CM_DEALADDITEM = 1026;
        public const int CM_DEALDELITEM = 1027;
        public const int CM_DEALCANCEL = 1028;
        public const int CM_DEALCHGGOLD = 1029;
        public const int CM_DEALEND = 1030;
        public const int CM_USERSTORAGEITEM = 1031;
        public const int CM_USERTAKEBACKSTORAGEITEM = 1032;
        public const int CM_WANTMINIMAP = 1033;
        public const int CM_USERMAKEDRUGITEM = 1034;
        public const int CM_OPENGUILDDLG = 1035;
        public const int CM_GUILDHOME = 1036;
        public const int CM_GUILDMEMBERLIST = 1037;
        public const int CM_GUILDADDMEMBER = 1038;
        public const int CM_GUILDDELMEMBER = 1039;
        public const int CM_GUILDUPDATENOTICE = 1040;
        public const int CM_GUILDUPDATERANKINFO = 1041;
        public const int CM_1042 = 1042;
        public const int CM_ADJUST_BONUS = 1043;
        public const int CM_GUILDALLY = 1044;
        public const int CM_GUILDBREAKALLY = 1045;
        public const int CM_SPEEDHACKUSER = 10430;
        // ??
        public const int CM_PROTOCOL = 2000;
        public const int CM_IDPASSWORD = 2001;
        public const int CM_ADDNEWUSER = 2002;
        public const int CM_CHANGEPASSWORD = 2003;
        public const int CM_UPDATEUSER = 2004;
        public const int CM_THROW = 3005;
        public const int CM_TURN = 3010;
        public const int CM_WALK = 3011;
        public const int CM_SITDOWN = 3012;
        public const int CM_RUN = 3013;
        public const int CM_HIT = 3014;
        public const int CM_HEAVYHIT = 3015;
        public const int CM_BIGHIT = 3016;
        public const int CM_SPELL = 3017;
        public const int CM_POWERHIT = 3018;
        public const int CM_LONGHIT = 3019;
        public const int CM_WIDEHIT = 3024;
        public const int CM_FIREHIT = 3025;
        public const int CM_SAY = 3030;
        public const int SM_41 = 4;
        public const int SM_THROW = 5;
        public const int SM_RUSH = 6;
        public const int SM_RUSHKUNG = 7;
        public const int SM_FIREHIT = 8;
        // 烈火
        public const int SM_BACKSTEP = 9;
        public const int SM_TURN = 10;
        public const int SM_WALK = 11;
        // 走
        public const int SM_SITDOWN = 12;
        public const int SM_RUN = 13;
        public const int SM_HIT = 14;
        // 砍
        public const int SM_HEAVYHIT = 15;
        public const int SM_BIGHIT = 16;
        public const int SM_SPELL = 17;
        // 使用魔法
        public const int SM_POWERHIT = 18;
        public const int SM_LONGHIT = 19;
        // 刺杀
        public const int SM_DIGUP = 20;
        public const int SM_DIGDOWN = 21;
        public const int SM_FLYAXE = 22;
        public const int SM_LIGHTING = 23;
        public const int SM_WIDEHIT = 24;
        public const int SM_CRSHIT = 25;
        public const int SM_TWINHIT = 26;
        public const int SM_ALIVE = 27;
        public const int SM_MOVEFAIL = 28;
        public const int SM_HIDE = 29;
        public const int SM_DISAPPEAR = 30;
        public const int SM_STRUCK = 31;
        // 弯腰
        public const int SM_DEATH = 32;
        public const int SM_SKELETON = 33;
        public const int SM_NOWDEATH = 34;
        public const int SM_HEAR = 40;
        public const int SM_FEATURECHANGED = 41;
        public const int SM_USERNAME = 42;
        public const int SM_43 = 43;
        public const int SM_WINEXP = 44;
        public const int SM_LEVELUP = 45;
        public const int SM_DAYCHANGING = 46;
        public const int SM_LOGON = 50;
        public const int SM_NEWMAP = 51;
        public const int SM_ABILITY = 52;
        public const int SM_HEALTHSPELLCHANGED = 53;
        public const int SM_MAPDESCRIPTION = 54;
        public const int SM_SPELL2 = 117;
        public const int SM_SYSMESSAGE = 100;
        public const int SM_GROUPMESSAGE = 101;
        public const int SM_CRY = 102;
        public const int SM_WHISPER = 103;
        public const int SM_GUILDMESSAGE = 104;
        public const int SM_ADDITEM = 200;
        public const int SM_BAGITEMS = 201;
        public const int SM_DELITEM = 202;
        public const int SM_UPDATEITEM = 203;
        public const int SM_ADDMAGIC = 210;
        public const int SM_SENDMYMAGIC = 211;
        public const int SM_DELMAGIC = 212;
        public const int SM_ATTACKMODE = 213;
        // 攻击模式
        public const int SM_CERTIFICATION_SUCCESS = 500;
        public const int SM_CERTIFICATION_FAIL = 501;
        public const int SM_ID_NOTFOUND = 502;
        public const int SM_PASSWD_FAIL = 503;
        public const int SM_NEWID_SUCCESS = 504;
        public const int SM_NEWID_FAIL = 505;
        public const int SM_CHGPASSWD_SUCCESS = 506;
        public const int SM_CHGPASSWD_FAIL = 507;
        public const int SM_QUERYCHR = 520;
        public const int SM_NEWCHR_SUCCESS = 521;
        public const int SM_NEWCHR_FAIL = 522;
        public const int SM_DELCHR_SUCCESS = 523;
        public const int SM_DELCHR_FAIL = 524;
        public const int SM_STARTPLAY = 525;
        public const int SM_STARTFAIL = 526;
        // SM_USERFULL
        public const int SM_QUERYCHR_FAIL = 527;
        public const int SM_OUTOFCONNECTION = 528;
        // ?
        public const int SM_PASSOK_SELECTSERVER = 529;
        public const int SM_SELECTSERVER_OK = 530;
        public const int SM_NEEDUPDATE_ACCOUNT = 531;
        public const int SM_UPDATEID_SUCCESS = 532;
        public const int SM_UPDATEID_FAIL = 533;
        public const int SM_DROPITEM_SUCCESS = 600;
        public const int SM_DROPITEM_FAIL = 601;
        public const int SM_ITEMSHOW = 610;
        public const int SM_ITEMHIDE = 611;
        public const int SM_OPENDOOR_OK = 612;
        public const int SM_OPENDOOR_LOCK = 613;
        public const int SM_CLOSEDOOR = 614;
        public const int SM_TAKEON_OK = 615;
        public const int SM_TAKEON_FAIL = 616;
        public const int SM_TAKEOFF_OK = 619;
        public const int SM_TAKEOFF_FAIL = 620;
        public const int SM_SENDUSEITEMS = 621;
        public const int SM_WEIGHTCHANGED = 622;
        public const int SM_CLEAROBJECTS = 633;
        public const int SM_CHANGEMAP = 634;
        public const int SM_EAT_OK = 635;
        public const int SM_EAT_FAIL = 636;
        public const int SM_BUTCH = 637;
        public const int SM_MAGICFIRE = 638;
        public const int SM_MAGICFIRE_FAIL = 639;
        public const int SM_MAGIC_LVEXP = 640;
        public const int SM_DURACHANGE = 642;
        public const int SM_MERCHANTSAY = 643;
        public const int SM_MERCHANTDLGCLOSE = 644;
        public const int SM_SENDGOODSLIST = 645;
        public const int SM_SENDUSERSELL = 646;
        public const int SM_SENDBUYPRICE = 647;
        public const int SM_USERSELLITEM_OK = 648;
        public const int SM_USERSELLITEM_FAIL = 649;
        public const int SM_BUYITEM_SUCCESS = 650;
        // ?
        public const int SM_BUYITEM_FAIL = 651;
        // ?
        public const int SM_SENDDETAILGOODSLIST = 652;
        public const int SM_GOLDCHANGED = 653;
        public const int SM_CHANGELIGHT = 654;
        public const int SM_LAMPCHANGEDURA = 655;
        public const int SM_CHANGENAMECOLOR = 656;
        public const int SM_CHARSTATUSCHANGED = 657;
        public const int SM_SENDNOTICE = 658;
        public const int SM_GROUPMODECHANGED = 659;
        public const int SM_CREATEGROUP_OK = 660;
        public const int SM_CREATEGROUP_FAIL = 661;
        public const int SM_GROUPADDMEM_OK = 662;
        public const int SM_GROUPDELMEM_OK = 663;
        public const int SM_GROUPADDMEM_FAIL = 664;
        public const int SM_GROUPDELMEM_FAIL = 665;
        public const int SM_GROUPCANCEL = 666;
        public const int SM_GROUPMEMBERS = 667;
        public const int SM_SENDUSERREPAIR = 668;
        public const int SM_USERREPAIRITEM_OK = 669;
        public const int SM_USERREPAIRITEM_FAIL = 670;
        public const int SM_SENDREPAIRCOST = 671;
        public const int SM_DEALMENU = 673;
        public const int SM_DEALTRY_FAIL = 674;
        public const int SM_DEALADDITEM_OK = 675;
        public const int SM_DEALADDITEM_FAIL = 676;
        public const int SM_DEALDELITEM_OK = 677;
        public const int SM_DEALDELITEM_FAIL = 678;
        public const int SM_DEALCANCEL = 681;
        public const int SM_DEALREMOTEADDITEM = 682;
        public const int SM_DEALREMOTEDELITEM = 683;
        public const int SM_DEALCHGGOLD_OK = 684;
        public const int SM_DEALCHGGOLD_FAIL = 685;
        public const int SM_DEALREMOTECHGGOLD = 686;
        public const int SM_DEALSUCCESS = 687;
        public const int SM_SENDUSERSTORAGEITEM = 700;
        public const int SM_STORAGE_OK = 701;
        public const int SM_STORAGE_FULL = 702;
        public const int SM_STORAGE_FAIL = 703;
        public const int SM_SAVEITEMLIST = 704;
        public const int SM_TAKEBACKSTORAGEITEM_OK = 705;
        public const int SM_TAKEBACKSTORAGEITEM_FAIL = 706;
        public const int SM_TAKEBACKSTORAGEITEM_FULLBAG = 707;
        public const int SM_AREASTATE = 766;
        public const int SM_MYSTATUS = 708;
        public const int SM_DELITEMS = 709;
        public const int SM_READMINIMAP_OK = 710;
        public const int SM_READMINIMAP_FAIL = 711;
        public const int SM_SENDUSERMAKEDRUGITEMLIST = 712;
        public const int SM_MAKEDRUG_SUCCESS = 713;
        public const int SM_MAKEDRUG_FAIL = 714;
        public const int SM_716 = 716;
        public const int SM_CHANGEGUILDNAME = 750;
        public const int SM_SENDUSERSTATE = 751;
        public const int SM_SUBABILITY = 752;
        public const int SM_OPENGUILDDLG = 753;
        public const int SM_OPENGUILDDLG_FAIL = 754;
        public const int SM_SENDGUILDMEMBERLIST = 756;
        public const int SM_GUILDADDMEMBER_OK = 757;
        public const int SM_GUILDADDMEMBER_FAIL = 758;
        public const int SM_GUILDDELMEMBER_OK = 759;
        public const int SM_GUILDDELMEMBER_FAIL = 760;
        public const int SM_GUILDRANKUPDATE_FAIL = 761;
        public const int SM_BUILDGUILD_OK = 762;
        public const int SM_BUILDGUILD_FAIL = 763;
        public const int SM_DONATE_OK = 764;
        public const int SM_DONATE_FAIL = 765;
        public const int SM_MENU_OK = 767;
        // ?
        public const int SM_GUILDMAKEALLY_OK = 768;
        public const int SM_GUILDMAKEALLY_FAIL = 769;
        public const int SM_GUILDBREAKALLY_OK = 770;
        // ?
        public const int SM_GUILDBREAKALLY_FAIL = 771;
        // ?
        public const int SM_DLGMSG = 772;
        // Jacky
        public const int SM_SPACEMOVE_HIDE = 800;
        public const int SM_SPACEMOVE_SHOW = 801;
        public const int SM_RECONNECT = 802;
        public const int SM_GHOST = 803;
        public const int SM_SHOWEVENT = 804;
        public const int SM_HIDEEVENT = 805;
        public const int SM_SPACEMOVE_HIDE2 = 806;
        public const int SM_SPACEMOVE_SHOW2 = 807;
        public const int SM_TIMECHECK_MSG = 810;
        public const int SM_ADJUST_BONUS = 811;
        // ?
        public const int SM_OPENHEALTH = 1100;
        public const int SM_CLOSEHEALTH = 1101;
        public const int SM_CHANGEFACE = 1104;
        public const int SM_BREAKWEAPON = 1102;
        public const int SM_INSTANCEHEALGUAGE = 1103;
        // ??
        public const int SM_VERSION_FAIL = 1106;
        public const int SM_ITEMUPDATE = 1500;
        public const int SM_MONSTERSAY = 1501;
        public const int SM_EXCHGTAKEON_OK = 65023;
        public const int SM_EXCHGTAKEON_FAIL = 65024;
        public const int SM_TEST = 65037;
        public const int SM_ACTION_MIN = 65070;
        public const int SM_ACTION_MAX = 65071;
        public const int SM_ACTION2_MIN = 65072;
        public const int SM_ACTION2_MAX = 65073;
        public const int CM_SERVERREGINFO = 65074;
        // -------------------------------------
        public const int CM_GETGAMELIST = 5001;
        public const int SM_SENDGAMELIST = 5002;
        public const int CM_GETBACKPASSWORD = 5003;
        public const int SM_GETBACKPASSWD_SUCCESS = 5005;
        public const int SM_GETBACKPASSWD_FAIL = 5006;
        public const int SM_SERVERCONFIG = 5007;
        public const int SM_GAMEGOLDNAME = 5008;
        public const int SM_PASSWORD = 5009;
        public const int SM_HORSERUN = 5010;
        public const int UNKNOWMSG = 199;
        // 以下几个正确
        public const int SS_OPENSESSION = 100;
        public const int SS_CLOSESESSION = 101;
        public const int SS_KEEPALIVE = 104;
        public const int SS_KICKUSER = 111;
        public const int SS_SERVERLOAD = 113;
        public const int SS_200 = 200;
        public const int SS_201 = 201;
        public const int SS_202 = 202;
        public const int SS_203 = 203;
        public const int SS_204 = 204;
        public const int SS_205 = 205;
        public const int SS_206 = 206;
        public const int SS_207 = 207;
        public const int SS_208 = 208;
        public const int SS_209 = 209;
        public const int SS_210 = 210;
        public const int SS_211 = 211;
        public const int SS_212 = 212;
        public const int SS_213 = 213;
        public const int SS_214 = 214;
        public const int SS_WHISPER = 299;
        // ?????
        // 不正确
        // Damian
        public const int SS_SERVERINFO = 103;
        public const int SS_SOFTOUTSESSION = 102;
        public const int SS_LOGINCOST = 30002;
        // Damian
        public const int DBR_FAIL = 2000;
        public const int DB_LOADHUMANRCD = 100;
        public const int DB_SAVEHUMANRCD = 101;
        public const int DB_SAVEHUMANRCDEX = 102;
        // ?
        public const int DBR_LOADHUMANRCD = 1100;
        public const int DBR_SAVEHUMANRCD = 1102;
        // ?
        public const int SG_FORMHANDLE = 32001;
        public const int SG_STARTNOW = 32002;
        public const int SG_STARTOK = 32003;
        public const int SG_CHECKCODEADDR = 32004;
        public const int SG_USERACCOUNT = 32005;
        public const int SG_USERACCOUNTCHANGESTATUS = 32006;
        public const int SG_USERACCOUNTNOTFOUND = 32007;
        public const int GS_QUIT = 32101;
        public const int GS_USERACCOUNT = 32102;
        public const int GS_CHANGEACCOUNTINFO = 32103;
        public const int WM_SENDPROCMSG = 32104;
        // Damian
        public const uint RUNGATECODE = 0xAA55AA55;
        public const int GM_OPEN = 1;
        public const int GM_CLOSE = 2;
        public const int GM_CHECKSERVER = 3;
        // Send check signal to Server
        public const int GM_CHECKCLIENT = 4;
        // Send check signal to Client
        public const int GM_DATA = 5;
        public const int GM_SERVERUSERINDEX = 6;
        public const int GM_RECEIVE_OK = 7;
        public const int GM_TEST = 20;
        // M2Server
        public const int GROUPMAX = 11;
        public const int CM_42HIT = 42;
        public const int CM_PASSWORD = 2001;
        public const int CM_CHGPASSWORD = 2002;
        public const int CM_SETPASSWORD = 2004;
        public const int CM_HORSERUN = 3035;
        // ------------未知消息码
        public const int CM_CRSHIT = 3036;
        // ------------未知消息码
        public const int CM_3037 = 3037;
        public const int CM_TWINHIT = 3038;
        public const int CM_QUERYUSERSET = 3040;
        // Damian
        public const int SM_PLAYDICE = 8001;
        public const int SM_PASSWORDSTATUS = 8002;
        public const int SM_NEEDPASSWORD = 8003;
        public const int SM_GETREGINFO = 8004;
        public const int DATA_BUFSIZE = 1024;
        public const int RUNGATEMAX = 8;
        // MAX_STATUS_ATTRIBUTE = 13;
        public const int MAXMAGIC = 54;
        public const string PN_GETRGB = "GetRGB";
        public const string PN_GAMEDATALOG = "GameDataLog";
        public const string PN_SENDBROADCASTMSG = "SendBroadcastMsg";
        public const string sSTRING_GOLDNAME = "金币";
        public const short MAXLEVEL = short.MaxValue;
        public const int MAXCHANGELEVEL = 1000;
        public const int SLAVEMAXLEVEL = 50;
        public const int LOG_GAMEGOLD = 1;
        public const int LOG_GAMEPOINT = 2;
        public const int RC_PLAYOBJECT = 0;
        public const int RC_GUARD = 11;
        public const int RC_PEACENPC = 15;
        public const int RC_ANIMAL = 50;
        public const int RC_EXERCISE = 55;
        // 练功师
        public const int RC_PLAYCLONE = 60;
        // 人型怪物
        public const int RC_MONSTER = 80;
        public const int RC_NPC = 10;
        public const int RC_ARCHERGUARD = 112;
        public const int RC_135 = 135;
        // 魔王岭弓箭手
        public const int RC_136 = 136;
        // 魔王岭弓箭手
        public const int RC_153 = 153;
        // 任务怪物
        public const int RM_TURN = 10001;
        public const int RM_WALK = 10002;
        public const int RM_HORSERUN = 50003;
        public const int RM_RUN = 10003;
        public const int RM_HIT = 10004;
        public const int RM_BIGHIT = 10006;
        public const int RM_HEAVYHIT = 10007;
        public const int RM_SPELL = 10008;
        public const int RM_SPELL2 = 10009;
        public const int RM_MOVEFAIL = 10010;
        public const int RM_LONGHIT = 10011;
        public const int RM_WIDEHIT = 10012;
        public const int RM_FIREHIT = 10014;
        public const int RM_CRSHIT = 10015;
        public const int RM_DEATH = 10021;
        public const int RM_SKELETON = 10024;
        public const int RM_LOGON = 10050;
        public const int RM_ABILITY = 10051;
        public const int RM_HEALTHSPELLCHANGED = 10052;
        public const int RM_DAYCHANGING = 10053;
        public const int RM_10101 = 10101;
        public const int RM_WEIGHTCHANGED = 10115;
        public const int RM_FEATURECHANGED = 10116;
        public const int RM_BUTCH = 10119;
        public const int RM_MAGICFIRE = 10120;
        public const int RM_MAGICFIREFAIL = 10121;
        public const int RM_SENDMYMAGIC = 10122;
        public const int RM_MAGIC_LVEXP = 10123;
        public const int RM_DURACHANGE = 10125;
        public const int RM_MERCHANTDLGCLOSE = 10127;
        public const int RM_SENDGOODSLIST = 10128;
        public const int RM_SENDUSERSELL = 10129;
        public const int RM_SENDBUYPRICE = 10130;
        public const int RM_USERSELLITEM_OK = 10131;
        public const int RM_USERSELLITEM_FAIL = 10132;
        public const int RM_BUYITEM_SUCCESS = 10133;
        public const int RM_BUYITEM_FAIL = 10134;
        public const int RM_SENDDETAILGOODSLIST = 10135;
        public const int RM_GOLDCHANGED = 10136;
        public const int RM_CHANGELIGHT = 10137;
        public const int RM_LAMPCHANGEDURA = 10138;
        public const int RM_CHARSTATUSCHANGED = 10139;
        public const int RM_GROUPCANCEL = 10140;
        public const int RM_SENDUSERREPAIR = 10141;
        public const int RM_SENDUSERSREPAIR = 50142;
        public const int RM_SENDREPAIRCOST = 10142;
        public const int RM_USERREPAIRITEM_OK = 10143;
        public const int RM_USERREPAIRITEM_FAIL = 10144;
        public const int RM_USERSTORAGEITEM = 10146;
        public const int RM_USERGETBACKITEM = 10147;
        public const int RM_SENDDELITEMLIST = 10148;
        public const int RM_USERMAKEDRUGITEMLIST = 10149;
        public const int RM_MAKEDRUG_SUCCESS = 10150;
        public const int RM_MAKEDRUG_FAIL = 10151;
        public const int RM_ALIVE = 10153;
        public const int RM_10155 = 10155;
        public const int RM_DIGUP = 10200;
        public const int RM_DIGDOWN = 10201;
        public const int RM_FLYAXE = 10202;
        public const int RM_LIGHTING = 10204;
        public const int RM_10205 = 10205;
        public const int RM_CHANGEGUILDNAME = 10301;
        public const int RM_SUBABILITY = 10302;
        public const int RM_BUILDGUILD_OK = 10303;
        public const int RM_BUILDGUILD_FAIL = 10304;
        public const int RM_DONATE_OK = 10305;
        public const int RM_DONATE_FAIL = 10306;
        public const int RM_MENU_OK = 10309;
        public const int RM_RECONNECTION = 10332;
        public const int RM_HIDEEVENT = 10333;
        public const int RM_SHOWEVENT = 10334;
        public const int RM_10401 = 10401;
        public const int RM_OPENHEALTH = 10410;
        public const int RM_CLOSEHEALTH = 10411;
        public const int RM_BREAKWEAPON = 10413;
        public const int RM_10414 = 10414;
        public const int RM_CHANGEFACE = 10415;
        public const int RM_PASSWORD = 10416;
        public const int RM_PLAYDICE = 10500;
        public const int RM_HEAR = 11001;
        public const int RM_WHISPER = 11002;
        public const int RM_CRY = 11003;
        public const int RM_SYSMESSAGE = 11004;
        public const int RM_GROUPMESSAGE = 11005;
        public const int RM_SYSMESSAGE2 = 11006;
        public const int RM_GUILDMESSAGE = 11007;
        public const int RM_SYSMESSAGE3 = 11008;
        public const int RM_MERCHANTSAY = 11009;
        public const int RM_ZEN_BEE = 8020;
        public const int RM_DELAYMAGIC = 8021;
        public const int RM_STRUCK = 8018;
        public const int RM_MAGSTRUCK_MINE = 8030;
        public const int RM_MAGHEALING = 8034;
        public const int RM_POISON = 8037;
        public const int RM_DOOPENHEALTH = 8040;
        public const int RM_SPACEMOVE_FIRE2 = 8042;
        public const int RM_DELAYPUSHED = 8043;
        public const int RM_MAGSTRUCK = 8044;
        public const int RM_TRANSPARENT = 8045;
        public const int RM_DOOROPEN = 8046;
        public const int RM_DOORCLOSE = 8047;
        public const int RM_DISAPPEAR = 8061;
        public const int RM_SPACEMOVE_FIRE = 8062;
        public const int RM_SENDUSEITEMS = 8074;
        public const int RM_WINEXP = 8075;
        public const int RM_ADJUST_BONUS = 8078;
        public const int RM_ITEMSHOW = 8082;
        public const int RM_GAMEGOLDCHANGED = 8084;
        public const int RM_ITEMHIDE = 8085;
        public const int RM_LEVELUP = 8086;
        public const int RM_CHANGENAMECOLOR = 8090;
        public const int RM_PUSH = 8092;
        public const int RM_CLEAROBJECTS = 8097;
        public const int RM_CHANGEMAP = 8098;
        public const int RM_SPACEMOVE_SHOW2 = 8099;
        public const int RM_SPACEMOVE_SHOW = 8100;
        public const int RM_USERNAME = 8101;
        public const int RM_MYSTATUS = 8102;
        public const int RM_STRUCK_MAG = 8103;
        public const int RM_RUSH = 8104;
        public const int RM_RUSHKUNG = 8105;
        public const int RM_PASSWORDSTATUS = 8106;
        public const int RM_POWERHIT = 8107;
        public const int RM_41 = 9041;
        public const int RM_TWINHIT = 9042;
        public const int RM_43 = 9043;

        // -------Start Inter Server Msg-------
        public const int ISM_GROUPSERVERHEART = 100;
        public const int ISM_USERSERVERCHANGE = 200;
        public const int ISM_USERLOGON = 201;
        public const int ISM_USERLOGOUT = 202;
        public const int ISM_WHISPER = 203;
        public const int ISM_SYSOPMSG = 204;
        public const int ISM_ADDGUILD = 205;
        public const int ISM_DELGUILD = 206;
        public const int ISM_RELOADGUILD = 207;
        public const int ISM_GUILDMSG = 208;
        public const int ISM_CHATPROHIBITION = 209;
        public const int ISM_CHATPROHIBITIONCANCEL = 210;
        public const int ISM_CHANGECASTLEOWNER = 211;
        public const int ISM_RELOADCASTLEINFO = 212;
        public const int ISM_RELOADADMIN = 213;
        // -------End Inter Server Msg-------
        // Friend System -------------
        public const int ISM_FRIEND_INFO = 214;
        public const int ISM_FRIEND_DELETE = 215;
        public const int ISM_FRIEND_OPEN = 216;
        public const int ISM_FRIEND_CLOSE = 217;
        public const int ISM_FRIEND_RESULT = 218;
        // Tag System ----------------
        public const int ISM_TAG_SEND = 219;
        public const int ISM_TAG_RESULT = 220;
        // User System --------------
        public const int ISM_USER_INFO = 221;
        public const int ISM_CHANGESERVERRECIEVEOK = 222;
        public const int ISM_RELOADCHATLOG = 223;
        public const int ISM_MARKETOPEN = 224;
        public const int ISM_MARKETCLOSE = 225;
        // relationship --------------
        public const int ISM_LM_DELETE = 226;
        public const int ISM_RELOADMAKEITEMLIST = 227;
        public const int ISM_GUILDMEMBER_RECALL = 228;
        public const int ISM_RELOADGUILDAGIT = 229;
        public const int ISM_LM_WHISPER = 230;
        public const int ISM_GMWHISPER = 231;
        public const int ISM_LM_LOGIN = 232;
        public const int ISM_LM_LOGOUT = 233;
        public const int ISM_REQUEST_RECALL = 234;
        public const int ISM_RECALL = 235;
        public const int ISM_LM_LOGIN_REPLY = 236;
        public const int ISM_LM_KILLED_MSG = 237;
        public const int ISM_REQUEST_LOVERRECALL = 238;
        public const int ISM_STANDARDTICKREQ = 239;
        public const int ISM_STANDARDTICK = 240;
        public const int ISM_GUILDWAR = 241;


        public const int OS_EVENTOBJECT = 1;
        public const int OS_MOVINGOBJECT = 2;
        public const int OS_ITEMOBJECT = 3;
        public const int OS_GATEOBJECT = 4;
        public const int OS_MAPEVENT = 5;
        public const int OS_DOOR = 6;
        public const int OS_ROON = 7;
        // 技能编号（正确）
        /// <summary>
        /// 火球术
        /// </summary>
        public const int SKILL_FIREBALL = 1;
        /// <summary>
        /// 大火球
        /// </summary>
        public const int SKILL_HEALLING = 2;
        public const int SKILL_ONESWORD = 3;
        public const int SKILL_ILKWANG = 4;
        public const int SKILL_FIREBALL2 = 5;
        /// <summary>
        /// 施毒术
        /// </summary>
        public const int SKILL_AMYOUNSUL = 6;
        public const int SKILL_YEDO = 7;
        /// <summary>
        /// 抗拒火环
        /// </summary>
        public const int SKILL_FIREWIND = 8;
        /// <summary>
        /// 地狱火
        /// </summary>
        public const int SKILL_FIRE = 9;
        /// <summary>
        /// 疾光电影
        /// </summary>
        public const int SKILL_SHOOTLIGHTEN = 10;
        /// <summary>
        /// 雷电术
        /// </summary>
        public const int SKILL_LIGHTENING = 11;
        /// <summary>
        /// 刺杀剑法
        /// </summary>
        public const int SKILL_ERGUM = 12;
        /// <summary>
        /// 灵魂火符
        /// </summary>
        public const int SKILL_FIRECHARM = 13;
        /// <summary>
        /// 幽灵盾
        /// </summary>
        public const int SKILL_HANGMAJINBUB = 14;
        /// <summary>
        /// 神圣战甲术
        /// </summary>
        public const int SKILL_DEJIWONHO = 15;
        /// <summary>
        /// 捆魔咒
        /// </summary>
        public const int SKILL_HOLYSHIELD = 16;
        /// <summary>
        /// 召唤骷髅
        /// </summary>
        public const int SKILL_SKELLETON = 17;
        /// <summary>
        /// 隐身术
        /// </summary>
        public const int SKILL_CLOAK = 18;
        /// <summary>
        /// 集体隐身术
        /// </summary>
        public const int SKILL_BIGCLOAK = 19;
        /// <summary>
        /// 诱惑之光
        /// </summary>
        public const int SKILL_TAMMING = 20;
        /// <summary>
        /// 瞬息移动
        /// </summary>
        public const int SKILL_SPACEMOVE = 21;
        /// <summary>
        /// 火墙
        /// </summary>
        public const int SKILL_EARTHFIRE = 22;
        /// <summary>
        /// 爆裂火焰
        /// </summary>
        public const int SKILL_FIREBOOM = 23;
        /// <summary>
        /// 地狱雷光
        /// </summary>
        public const int SKILL_LIGHTFLOWER = 24;
        /// <summary>
        /// 半月弯刀
        /// </summary>
        public const int SKILL_BANWOL = 25;
        /// <summary>
        /// 烈火剑法
        /// </summary>
        public const int SKILL_FIRESWORD = 26;
        /// <summary>
        /// 野蛮冲撞
        /// </summary>
        public const int SKILL_MOOTEBO = 27;
        /// <summary>
        /// 心灵启示
        /// </summary>
        public const int SKILL_SHOWHP = 28;
        /// <summary>
        /// 群体治疗术
        /// </summary>
        public const int SKILL_BIGHEALLING = 29;
        /// <summary>
        /// 召唤神兽
        /// </summary>
        public const int SKILL_SINSU = 30;
        /// <summary>
        /// 魔法盾
        /// </summary>
        public const int SKILL_SHIELD = 31;
        /// <summary>
        /// 圣言术
        /// </summary>
        public const int SKILL_KILLUNDEAD = 32;
        /// <summary>
        /// 冰咆哮
        /// </summary>
        public const int SKILL_SNOWWIND = 33;
        /// <summary>
        /// 解毒术
        /// </summary>
        public const int SKILL_UNAMYOUNSUL = 40;
        // Purification
        public const int SKILL_WINDTEBO = 35;
        /// <summary>
        /// 冰焰
        /// </summary>
        public const int SKILL_MABE = 50;
        /// <summary>
        /// 群体雷电术
        /// </summary>
        public const int SKILL_GROUPLIGHTENING = 42;
        /// <summary>
        /// 群体施毒术
        /// </summary>
        public const int SKILL_GROUPAMYOUNSUL = 48;
        /// <summary>
        /// 地钉
        /// </summary>
        public const int SKILL_GROUPDEDING = 39;
        public const int SKILL_CROSSMOON = 34;
        // CHM
        public const int SKILL_ANGEL = 41;
        public const int SKILL_TWINBLADE = 38;
        public const int SKILL_43 = 43;
        public const int SKILL_44 = 44;
        /// <summary>
        /// 灭天火
        /// </summary>
        public const int SKILL_45 = 45;
        /// <summary>
        /// 分身术
        /// </summary>
        public const int SKILL_46 = 46;
        /// <summary>
        /// 火龙气焰
        /// </summary>
        public const int SKILL_47 = 47;
        /// <summary>
        /// 气功波
        /// </summary>
        public const int SKILL_ENERGYREPULSOR = 37;
        /// <summary>
        /// 净化术
        /// </summary>
        public const int SKILL_49 = 49;
        /// <summary>
        /// 无极真气
        /// </summary>
        public const int SKILL_UENHANCER = 36;
        public const int SKILL_51 = 51;
        public const int SKILL_52 = 52;
        public const int SKILL_53 = 53;
        public const int SKILL_54 = 54;
        public const int SKILL_55 = 55;
        public const int SKILL_REDBANWOL = 56;
        public const int SKILL_57 = 57;
        public const int SKILL_58 = 58;
        public const int SKILL_59 = 59;
        public const int LA_UNDEAD = 1;
        public const string sENCYPTSCRIPTFLAG = "不知道是什么字符串";
        public const string sSTATUS_FAIL = "+FAIL/";
        public const string sSTATUS_GOOD = "+GOOD/";

        public static TDefaultMessage MakeDefaultMsg(int msg, int Recog, int param, int tag, int series)
        {
            var result = new TDefaultMessage();
            result.Ident = (short)msg;
            result.Param = (short)param;
            result.Tag = (short)tag;
            result.Series = (short)series;
            result.Recog = Recog;
            return result;
        }

        public static int MakeMonsterFeature(byte btRaceImg, byte btWeapon, short wAppr)
        {
            int result;
            result = HUtil32.MakeLong(HUtil32.MakeWord(btRaceImg, btWeapon), wAppr);
            return result;
        }

        public static int MakeHumanFeature(byte btRaceImg, byte btDress, byte btWeapon, byte btHair)
        {
            return HUtil32.MakeLong(HUtil32.MakeWord(btRaceImg, btWeapon), HUtil32.MakeWord(btHair, btDress));
        }
    }
}

