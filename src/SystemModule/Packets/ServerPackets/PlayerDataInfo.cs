using ProtoBuf;
using System.Runtime.Serialization;
using SystemModule.Data;
using SystemModule.Packets.ClientPackets;

namespace SystemModule.Packets.ServerPackets
{
    [ProtoContract]
    public class PlayerDataInfo
    {
        [ProtoMember(1)]
        public RecordHeader Header { get; set; }
        [ProtoMember(2)]
        public PlayerInfoData Data { get; set; }

        public PlayerDataInfo()
        {
            Header = new RecordHeader();
            Data = new PlayerInfoData();
        }
    }

    [ProtoContract]
    public class SavePlayerDataMessage : RequestPacket
    {
        [ProtoMember(1)]
        public string Account { get; set; }
        [ProtoMember(2)]
        public string ChrName { get; set; }
        [ProtoMember(3)]
        public PlayerDataInfo HumDataInfo { get; set; }
    }

    [ProtoContract]
    public class LoadPlayerDataMessage : RequestPacket
    {
        [ProtoMember(1)]
        public string Account { get; set; }
        [ProtoMember(2)]
        public string ChrName { get; set; }
        [ProtoMember(3)]
        public string UserAddr { get; set; }
        [ProtoMember(4)]
        public int SessionID { get; set; }
    }

    [ProtoContract]
    public class PlayerInfoData
    {
        [ProtoMember(1)]
        public byte ServerIndex;
        [ProtoMember(2)]
        public string ChrName;
        [ProtoMember(3)]
        public string CurMap;
        [ProtoMember(4)]
        public short CurX;
        [ProtoMember(5)]
        public short CurY;
        [ProtoMember(6)]
        public byte Dir;
        [ProtoMember(7)]
        public byte Hair;
        [ProtoMember(8)]
        public byte Sex;
        [ProtoMember(9)]
        public byte Job;
        [ProtoMember(10)]
        public int Gold;
        [ProtoMember(11)]
        public Ability Abil;
        [ProtoMember(12)]
        public ushort[] StatusTimeArr;
        [ProtoMember(13)]
        public string HomeMap;
        [ProtoMember(14)]
        public short HomeX;
        [ProtoMember(15)]
        public short HomeY;
        [ProtoMember(16)]
        public NakedAbility BonusAbil;
        [ProtoMember(17)]
        public int BonusPoint;
        [ProtoMember(18)]
        public byte CreditPoint;
        [ProtoMember(19)]
        public byte ReLevel;
        [ProtoMember(20)]
        public string MasterName;
        [ProtoMember(21)]
        public bool IsMaster;
        [ProtoMember(22)]
        public string DearName;
        [ProtoMember(23)]
        public string StoragePwd;
        [ProtoMember(24)]
        public int GameGold;
        [ProtoMember(25)]
        public int GamePoint;
        [ProtoMember(26)]
        public int PayMentPoint;
        [ProtoMember(27)]
        public int PKPoint;
        [ProtoMember(28)]
        public byte AllowGroup;
        [ProtoMember(29)]
        public byte btF9;
        [ProtoMember(30)]
        public byte AttatckMode;
        [ProtoMember(31)]
        public byte IncHealth;
        [ProtoMember(32)]
        public byte IncSpell;
        [ProtoMember(33)]
        public byte IncHealing;
        [ProtoMember(34)]
        public byte FightZoneDieCount;
        [ProtoMember(35)]
        public byte btEE;
        [ProtoMember(36)]
        public byte btEF;
        [ProtoMember(37)]
        public string Account;
        [ProtoMember(38)]
        public bool LockLogon;
        [ProtoMember(39)]
        public short Contribution;
        [ProtoMember(40)]
        public int HungerStatus;
        [ProtoMember(41)]
        public bool AllowGuildReCall;
        [ProtoMember(42)]
        public short GroupRcallTime;
        [ProtoMember(43)]
        public double BodyLuck;
        [ProtoMember(44)]
        public bool AllowGroupReCall;
        [ProtoMember(45)]
        public byte[] QuestUnitOpen;
        [ProtoMember(46)]
        public byte[] QuestUnit;
        [ProtoMember(47)]
        public byte[] QuestFlag;
        [ProtoMember(48)]
        public byte MarryCount;
        [ProtoMember(49, OverwriteList = true)]
        public UserItem[] HumItems;
        [ProtoMember(50, OverwriteList = true)]
        public UserItem[] BagItems;
        [ProtoMember(51, OverwriteList = true)]
        public UserItem[] StorageItems;
        [ProtoMember(52, OverwriteList = true)]
        public MagicRcd[] Magic;

        public PlayerInfoData()
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

        [OnSerializing]
        public void Serialized(StreamingContext streamingContext)
        {
            HumItems ??= new UserItem[13];
            BagItems ??= new UserItem[46];
            StorageItems ??= new UserItem[50];
            Magic ??= new MagicRcd[20];
            for (int i = 0; i < HumItems.Length; i++)
            {
                if (HumItems[i] == null)
                {
                    HumItems[i] = new UserItem();
                }
            }
            for (int i = 0; i < BagItems.Length; i++)
            {
                if (BagItems[i] == null)
                {
                    BagItems[i] = new UserItem();
                }
            }
            for (int i = 0; i < StorageItems.Length; i++)
            {
                if (StorageItems[i] == null)
                {
                    StorageItems[i] = new UserItem();
                }
            }
            for (int i = 0; i < Magic.Length; i++)
            {
                if (Magic[i] == null)
                {
                    Magic[i] = new MagicRcd();
                }
            }
        }
    }
}