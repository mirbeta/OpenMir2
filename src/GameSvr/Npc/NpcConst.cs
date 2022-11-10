using SystemModule.Packet.ClientPackets;

namespace GameSvr.Npc
{
    /// <summary>
    /// 沙巴克武器升级信息
    /// </summary>
    public class WeaponUpgradeInfo
    {
        public string UserName;
        public UserItem UserItem;
        public byte Dc;
        public byte Sc;
        public byte Mc;
        public byte Dura;
        public int n2C;
        public DateTime UpgradeTime;
        public int GetBackTick;
        public int n3C;
    }

    public struct ItemPrice
    {
        public ushort wIndex;
        public double nPrice;
    }

    public struct Goods
    {
        public string ItemName;
        public int Count;
        public int RefillTime;
        public int RefillTick;
    }

    public class QuestActionInfo
    {
        public int nCmdCode;
        public string sParam1;
        public int nParam1;
        public string sParam2;
        public int nParam2;
        public string sParam3;
        public int nParam3;
        public string sParam4;
        public int nParam4;
        public string sParam5;
        public int nParam5;
        public string sParam6;
        public int nParam6;
        public string sOpName;
        public string sOpHName;
    }

    public class QuestConditionInfo
    {
        public int CmdCode;
        public string sParam1;
        public int nParam1;
        public string sParam2;
        public int nParam2;
        public string sParam3;
        public int nParam3;
        public string sParam4;
        public int nParam4;
        public string sParam5;
        public int nParam5;
        public string sParam6;
        public int nParam6;
        public string sOpName;
        public string sOpHName;
    }

    public class SayingProcedure
    {
        public IList<QuestConditionInfo> ConditionList;
        public IList<QuestActionInfo> ActionList;
        public string sSayMsg;
        public IList<string> SayOldLabelList;
        public IList<string> SayNewLabelList;
        public IList<QuestActionInfo> ElseActionList;
        public string sElseSayMsg;
        public IList<string> ElseSayOldLabelList;
        public IList<string> ElseSayNewLabelList;

        public SayingProcedure()
        {
            ConditionList = new List<QuestConditionInfo>();
            ActionList = new List<QuestActionInfo>();
            SayOldLabelList = new List<string>();
            SayNewLabelList = new List<string>();
            ElseActionList = new List<QuestActionInfo>();
            ElseSayOldLabelList = new List<string>();
            ElseSayNewLabelList = new List<string>();
        }
    }

    public class SayingRecord
    {
        public string sLabel;
        public IList<SayingProcedure> ProcedureList;
        public bool boExtJmp;
    }

    public class ScriptParams
    {
        public string sParams;
        public int nParams;
    }
}

