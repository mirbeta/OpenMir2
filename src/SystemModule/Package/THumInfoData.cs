using System;
using System.IO;

namespace SystemModule
{
    public class THumInfoData : Package
    {
        public string sCharName;
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
            BagItems = new TUserItem[46];
            StorageItems = new TUserItem[50];
            Magic = new TMagicRcd[20];
            Abil = new TAbility();
            BonusAbil = new TNakedAbility();
            wStatusTimeArr = new ushort[12];
        }

        public THumInfoData(byte[] buffer) 
            : base(buffer)
        {
            this.sCharName = ReadPascalString(14);
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
                Array.Copy(humItemBuff, i * 24, itemBuff, 0, 24);
                HumItems[i] = new TUserItem(itemBuff);
            }

            this.BagItems = new TUserItem[46];
            var bagItemBuff = ReadBytes(24 * 46);

            for (var i = 0; i < BagItems.Length; i++)
            {
                var itemBuff = new byte[24];
                Buffer.BlockCopy(bagItemBuff, i * 24, itemBuff, 0, 24);
                BagItems[i] = new TUserItem(itemBuff);
            }

            this.Magic = new TMagicRcd[20];
            var hubMagicBuff = ReadBytes(160);
            for (var i = 0; i < Magic.Length; i++)
            {
                var itemBuff = new byte[8];
                Buffer.BlockCopy(hubMagicBuff, i * 8, itemBuff, 0, 8);
                Magic[i] = new TMagicRcd(itemBuff);
            }

            this.StorageItems = new TUserItem[50];
            var storageBuff = ReadBytes(1200);
            for (var i = 0; i < StorageItems.Length; i++)
            {
                var itemBuff = new byte[24];
                Buffer.BlockCopy(storageBuff, i * 24, itemBuff, 0, 24);
                StorageItems[i] = new TUserItem(itemBuff);
            }
        }

        public byte[] ToByte()
        {
            using (var memoryStream = new MemoryStream())
            {
                var backingStream = new BinaryWriter(memoryStream);

                backingStream.Write(sCharName.ToByte(15));
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
}

