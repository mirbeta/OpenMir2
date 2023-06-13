using MemoryPack;
using System.Runtime.Serialization;
using SystemModule.Data;
using SystemModule.Packets.ClientPackets;

namespace SystemModule.Packets.ServerPackets
{
    [MemoryPackable]
    public partial class PlayerDataInfo
    {
        public RecordHeader Header { get; set; }
        public PlayerInfoData Data { get; set; }

        public PlayerDataInfo()
        {
            Header = new RecordHeader();
            Data = new PlayerInfoData();
        }
    }

    [MemoryPackable]
    public partial class SavePlayerDataMessage
    {
        public string Account { get; set; }
        public string ChrName { get; set; }
        public PlayerDataInfo HumDataInfo { get; set; }

        public SavePlayerDataMessage(string account, string chrName, PlayerDataInfo humDataInfo)
        {
            Account = account;
            ChrName = chrName;
            HumDataInfo = humDataInfo;
        }
    }

    [MemoryPackable]
    public partial record struct LoadPlayerDataMessage
    {
        public string Account { get; set; }
        public string ChrName { get; set; }
        public string UserAddr { get; set; }
        public int SessionID { get; set; }
    }

    [MemoryPackable]
    public partial class PlayerInfoData
    {
        public byte ServerIndex { get; set; }
        public string ChrName { get; set; }
        public string CurMap { get; set; }
        public short CurX { get; set; }
        public short CurY { get; set; }
        public byte Dir { get; set; }
        public byte Hair { get; set; }
        public byte Sex { get; set; }
        public byte Job { get; set; }
        public int Gold { get; set; }
        public Ability Abil { get; set; }
        public ushort[] StatusTimeArr { get; set; }
        public string HomeMap { get; set; }
        public short HomeX { get; set; }
        public short HomeY { get; set; }
        public NakedAbility BonusAbil { get; set; }
        public int BonusPoint { get; set; }
        public byte CreditPoint { get; set; }
        public byte ReLevel { get; set; }
        public string MasterName { get; set; }
        public bool IsMaster { get; set; }
        public string DearName { get; set; }
        public string StoragePwd { get; set; }
        public int GameGold { get; set; }
        public int GamePoint { get; set; }
        public int PayMentPoint { get; set; }
        public int PKPoint { get; set; }
        public byte AllowGroup { get; set; }
        public byte btF9 { get; set; }
        public byte AttatckMode { get; set; }
        public byte IncHealth { get; set; }
        public byte IncSpell { get; set; }
        public byte IncHealing { get; set; }
        public byte FightZoneDieCount { get; set; }
        public byte btEE { get; set; }
        public byte btEF { get; set; }
        public string Account { get; set; }
        public bool LockLogon { get; set; }
        public short Contribution { get; set; }
        public int HungerStatus { get; set; }
        public bool AllowGuildReCall { get; set; }
        public short GroupRcallTime { get; set; }
        public double BodyLuck { get; set; }
        public bool AllowGroupReCall { get; set; }
        public byte[] QuestUnitOpen { get; set; }
        public byte[] QuestUnit { get; set; }
        public byte[] QuestFlag { get; set; }
        public byte MarryCount { get; set; }
        public ServerUserItem[] HumItems;
        public ServerUserItem[] BagItems;
        public ServerUserItem[] StorageItems;
        public MagicRcd[] Magic { get; set; }

        public PlayerInfoData()
        {
            QuestUnitOpen = new byte[128];
            QuestUnit = new byte[128];
            QuestFlag = new byte[128];
            HumItems = new ServerUserItem[13];
            BagItems = new ServerUserItem[46];
            StorageItems = new ServerUserItem[50];
            Magic = new MagicRcd[20];
            Abil = new Ability();
            BonusAbil = new NakedAbility();
            StatusTimeArr = new ushort[15];
        }

        [OnSerializing]
        public void Serialized(StreamingContext streamingContext)
        {
            HumItems ??= new ServerUserItem[13];
            BagItems ??= new ServerUserItem[46];
            StorageItems ??= new ServerUserItem[50];
            Magic ??= new MagicRcd[20];
            for (int i = 0; i < HumItems.Length; i++)
            {
                if (HumItems[i] == null)
                {
                    HumItems[i] = new ServerUserItem();
                }
            }
            for (int i = 0; i < BagItems.Length; i++)
            {
                if (BagItems[i] == null)
                {
                    BagItems[i] = new ServerUserItem();
                }
            }
            for (int i = 0; i < StorageItems.Length; i++)
            {
                if (StorageItems[i] == null)
                {
                    StorageItems[i] = new ServerUserItem();
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