using ProtoBuf;

namespace SystemModule
{
    [ProtoContract]
    public class THumDataInfo
    {
        [ProtoMember(1)] public TRecordHeader Header { get; set; }
        [ProtoMember(2)] public THumInfoData Data { get; set; }

        public THumDataInfo()
        {
            Header = new TRecordHeader();
            Data = new THumInfoData();
        }
    }

    [ProtoContract]
    public class SaveHumDataPacket : CmdPacket
    {
        [ProtoMember(1)]
        public string sAccount { get; set; }
        [ProtoMember(2)]
        public string sCharName { get; set; }
        [ProtoMember(3)]
        public THumDataInfo HumDataInfo { get; set; }
    }
    
    [ProtoContract]
    public class LoadHumDataPacket : CmdPacket
    {
        [ProtoMember(1)] 
        public string sAccount { get; set; }
        [ProtoMember(2)] 
        public string sChrName { get; set; }
        [ProtoMember(3)] 
        public string sUserAddr { get; set; }
        [ProtoMember(4)] 
        public int nSessionID { get; set; }
    }
    
    [ProtoContract]
    public class THumInfoData
    {
        [ProtoMember(1)]
        public string sCharName;
        [ProtoMember(2)]
        public string sCurMap;
        [ProtoMember(3)]
        public short wCurX;
        [ProtoMember(4)]
        public short wCurY;
        [ProtoMember(5)]
        public byte btDir;
        [ProtoMember(6)]
        public byte btHair;
        [ProtoMember(7)]
        public byte btSex;
        [ProtoMember(8)]
        public byte btJob;
        [ProtoMember(9)]
        public int nGold;
        [ProtoMember(10)]
        public TAbility Abil;
        [ProtoMember(11)]
        public ushort[] wStatusTimeArr;
        [ProtoMember(12)]
        public string sHomeMap;
        [ProtoMember(13)]
        public short wHomeX;
        [ProtoMember(14)]
        public short wHomeY;
        [ProtoMember(15)]
        public TNakedAbility BonusAbil;
        [ProtoMember(16)]
        public int nBonusPoint;
        [ProtoMember(17)]
        public byte btCreditPoint;
        [ProtoMember(18)]
        public byte btReLevel;
        [ProtoMember(19)]
        public string sMasterName;
        [ProtoMember(20)]
        public bool boMaster;
        [ProtoMember(21)]
        public string sDearName;
        [ProtoMember(22)]
        public string sStoragePwd;
        [ProtoMember(23)]
        public int nGameGold;
        [ProtoMember(24)]
        public int nGamePoint;
        [ProtoMember(25)]
        public int nPayMentPoint;
        [ProtoMember(26)]
        public int nPKPoint;
        [ProtoMember(27)]
        public byte btAllowGroup;
        [ProtoMember(28)]
        public byte btF9;
        [ProtoMember(29)]
        public byte btAttatckMode;
        [ProtoMember(30)]
        public byte btIncHealth;
        [ProtoMember(31)]
        public byte btIncSpell;
        [ProtoMember(32)]
        public byte btIncHealing;
        [ProtoMember(33)]
        public byte btFightZoneDieCount;
        [ProtoMember(34)]
        public byte btEE;
        [ProtoMember(35)]
        public byte btEF;
        [ProtoMember(36)]
        public string sAccount;
        [ProtoMember(37)]
        public bool boLockLogon;
        [ProtoMember(38)]
        public short wContribution;
        [ProtoMember(39)]
        public int nHungerStatus;
        [ProtoMember(40)]
        public bool boAllowGuildReCall;
        [ProtoMember(41)]
        public short wGroupRcallTime;
        [ProtoMember(42)]
        public double dBodyLuck;
        [ProtoMember(43)]
        public bool boAllowGroupReCall;
        [ProtoMember(44)]
        public byte[] QuestUnitOpen;
        [ProtoMember(45)]
        public byte[] QuestUnit;
        [ProtoMember(46)]
        public byte[] QuestFlag;
        [ProtoMember(47)]
        public byte MarryCount;
        [ProtoMember(48)]
        public TUserItem[] HumItems;
        [ProtoMember(49)]
        public TUserItem[] BagItems;
        [ProtoMember(50)]
        public TUserItem[] StorageItems;
        [ProtoMember(51)]
        public TMagicRcd[] Magic;

        public THumInfoData()
        {

        }

        public void Initialization()
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
    }
}