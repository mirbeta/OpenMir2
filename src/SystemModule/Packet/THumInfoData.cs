using System;
using System.IO;

namespace SystemModule
{
    public class THumInfoData : Packets
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
            var bagItemBuff = ReadBytes(24 * BagItems.Length);

            for (var i = 0; i < BagItems.Length; i++)
            {
                var itemBuff = new byte[24];
                Array.Copy(bagItemBuff, i * 24, itemBuff, 0, 24);
                BagItems[i] = new TUserItem(itemBuff);
            }

            this.Magic = new TMagicRcd[20];
            var hubMagicBuff = ReadBytes(Magic.Length * 8);
            for (var i = 0; i < Magic.Length; i++)
            {
                var itemBuff = new byte[8];
                Array.Copy(hubMagicBuff, i * 8, itemBuff, 0, 8);
                Magic[i] = new TMagicRcd(itemBuff);
            }

            this.StorageItems = new TUserItem[50];
            // var storageBuff = ReadBytes(24 * StorageItems.Length);
            // for (var i = 0; i < StorageItems.Length; i++)
            // {
            //     var itemBuff = new byte[24];
            //     Array.Copy(storageBuff, i * 24, itemBuff, 0, 24);
            //     StorageItems[i] = new TUserItem(itemBuff);
            // }
        }

        protected override void ReadPacket(BinaryReader reader)
        {
            throw new NotImplementedException();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(sCharName.ToByte(15));
            writer.Write(sCurMap.ToByte(17));

            writer.Write(wCurX);
            writer.Write(wCurY);
            writer.Write(btDir);
            writer.Write(btHair);
            writer.Write(btSex);
            writer.Write(btJob);
            writer.Write(nGold);
            writer.Write(Abil.GetPacket());//50
            for (var i = 0; i < wStatusTimeArr.Length; i++)
            {
                writer.Write(wStatusTimeArr[i]); //24
            }
            writer.Write(sHomeMap.ToByte(17));
            writer.Write(wHomeX);
            writer.Write(wHomeY);
            writer.Write(BonusAbil.GetPacket()); //24
            writer.Write(nBonusPoint);
            writer.Write(btCreditPoint);
            writer.Write(btReLevel);
            writer.Write(sMasterName.ToByte(15));
            writer.Write(boMaster);
            writer.Write(sDearName.ToByte(15));
            writer.Write(sStoragePwd.ToByte(11));
            writer.Write(nGameGold);
            writer.Write(nGamePoint);
            writer.Write(nPayMentPoint);
            writer.Write(nPKPoint);
            writer.Write(btAllowGroup);
            writer.Write(btF9);
            writer.Write(btAttatckMode);
            writer.Write(btIncHealth);
            writer.Write(btIncSpell);
            writer.Write(btIncHealing);
            writer.Write(btFightZoneDieCount);
            writer.Write(btEE);
            writer.Write(btEF);
            writer.Write(sAccount.ToByte(17));
            writer.Write(boLockLogon);
            writer.Write(wContribution);
            writer.Write(nHungerStatus);
            writer.Write(boAllowGuildReCall);
            writer.Write(wGroupRcallTime);
            writer.Write(dBodyLuck);
            writer.Write(boAllowGroupReCall);
            writer.Write(QuestUnitOpen);
            writer.Write(QuestUnit);
            writer.Write(QuestFlag);
            writer.Write(btMarryCount);
            var nullItem = new TUserItem();
            var nullBuffer = nullItem.GetPacket();
            for (var i = 0; i < HumItems.Length; i++)
            {
                if (HumItems[i] == null)
                {
                    writer.Write(nullBuffer);
                }
                else
                {
                    writer.Write(HumItems[i].GetPacket());
                }
            }
            for (var i = 0; i < BagItems.Length; i++)
            {
                if (BagItems[i] == null)
                {
                    writer.Write(nullBuffer);
                }
                else
                {
                    writer.Write(BagItems[i].GetPacket());
                }
            }
            var userMagic = new TMagicRcd();
            for (var i = 0; i < Magic.Length; i++)
            {
                if (Magic[i] == null)
                {
                    writer.Write(userMagic.GetPacket());
                }
                else
                {
                    writer.Write(Magic[i].GetPacket());//16
                }
            }
            for (var i = 0; i < StorageItems.Length; i++)
            {
                if (StorageItems[i] == null)
                {
                    writer.Write(nullBuffer);
                }
                else
                {
                    writer.Write(StorageItems[i].GetPacket());
                }
            }
            nullItem = null;
        }
    }
}