using ProtoBuf;
using SystemModule.Packet.ClientPackets;

namespace SystemModule.Packet.ServerPackets
{
    [ProtoContract]
    public class HumDataInfo
    {
        [ProtoMember(1)] 
        public RecordHeader Header { get; set; }
        [ProtoMember(2)] 
        public HumInfoData Data { get; set; }

        public HumDataInfo()
        {
            Header = new RecordHeader();
            Data = new HumInfoData();
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
        public HumDataInfo HumDataInfo { get; set; }
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
    public class HumInfoData
    {
        [ProtoMember(1)]
        public byte ServerIndex;
        [ProtoMember(2)]
        public string sChrName;
        [ProtoMember(3)]
        public string sCurMap;
        [ProtoMember(4)]
        public short CurX;
        [ProtoMember(5)]
        public short CurY;
        [ProtoMember(6)]
        public byte Dir;
        [ProtoMember(7)]
        public byte btHair;
        [ProtoMember(8)]
        public byte Sex;
        [ProtoMember(9)]
        public byte Job;
        [ProtoMember(10)]
        public int nGold;
        [ProtoMember(11)]
        public Ability Abil;
        [ProtoMember(12)]
        public ushort[] StatusTimeArr;
        [ProtoMember(13)]
        public string sHomeMap;
        [ProtoMember(14)]
        public short wHomeX;
        [ProtoMember(15)]
        public short wHomeY;
        [ProtoMember(16)]
        public NakedAbility BonusAbil;
        [ProtoMember(17)]
        public int nBonusPoint;
        [ProtoMember(18)]
        public byte btCreditPoint;
        [ProtoMember(19)]
        public byte btReLevel;
        [ProtoMember(20)]
        public string sMasterName;
        [ProtoMember(21)]
        public bool boMaster;
        [ProtoMember(22)]
        public string sDearName;
        [ProtoMember(23)]
        public string sStoragePwd;
        [ProtoMember(24)]
        public int nGameGold;
        [ProtoMember(25)]
        public int nGamePoint;
        [ProtoMember(26)]
        public int nPayMentPoint;
        [ProtoMember(27)]
        public int nPKPoint;
        [ProtoMember(28)]
        public byte btAllowGroup;
        [ProtoMember(29)]
        public byte btF9;
        [ProtoMember(30)]
        public byte btAttatckMode;
        [ProtoMember(31)]
        public byte btIncHealth;
        [ProtoMember(32)]
        public byte btIncSpell;
        [ProtoMember(33)]
        public byte btIncHealing;
        [ProtoMember(34)]
        public byte btFightZoneDieCount;
        [ProtoMember(35)]
        public byte btEE;
        [ProtoMember(36)]
        public byte btEF;
        [ProtoMember(37)]
        public string Account;
        [ProtoMember(38)]
        public bool boLockLogon;
        [ProtoMember(39)]
        public short wContribution;
        [ProtoMember(40)]
        public int nHungerStatus;
        [ProtoMember(41)]
        public bool boAllowGuildReCall;
        [ProtoMember(42)]
        public short wGroupRcallTime;
        [ProtoMember(43)]
        public double dBodyLuck;
        [ProtoMember(44)]
        public bool boAllowGroupReCall;
        [ProtoMember(45)]
        public byte[] QuestUnitOpen;
        [ProtoMember(46)]
        public byte[] QuestUnit;
        [ProtoMember(47)]
        public byte[] QuestFlag;
        [ProtoMember(48)]
        public byte MarryCount;
        [ProtoMember(49)]
        public UserItem[] HumItems;
        [ProtoMember(50)]
        public UserItem[] BagItems;
        [ProtoMember(51)]
        public UserItem[] StorageItems;
        [ProtoMember(52)]
        public MagicRcd[] Magic;

        public HumInfoData()
        {
            QuestUnitOpen = new byte[128];
            QuestUnit = new byte[128];
            QuestFlag = new byte[128];
            HumItems = new UserItem[13];
            BagItems = new UserItem[46];
            StorageItems = new UserItem[50];
            Magic = new MagicRcd[20];
            Abil = new Ability();
            BonusAbil = new NakedAbility();
            StatusTimeArr = new ushort[15];
        }
    }
}