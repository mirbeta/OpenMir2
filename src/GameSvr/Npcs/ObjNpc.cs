using SystemModule;

namespace GameSvr
{
    public class TUpgradeInfo
    {
        public string sUserName;
        public TUserItem UserItem;
        public byte btDc;
        public byte btSc;
        public byte btMc;
        public byte btDura;
        public int n2C;
        public DateTime dtTime;
        public int dwGetBackTick;
        public int n3C;
    }

    public struct TItemPrice
    {
        public short wIndex;
        public double nPrice;
    }

    public struct TGoods
    {
        public string sItemName;
        public int nCount;
        public int dwRefillTime;
        public int dwRefillTick;
    }

    public class TQuestActionInfo
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
    }

    public class TQuestConditionInfo
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
    }

    public class TSayingProcedure
    {
        public IList<TQuestConditionInfo> ConditionList;
        public IList<TQuestActionInfo> ActionList;
        public string sSayMsg;
        public IList<string> SayOldLabelList;
        public IList<string> SayNewLabelList;
        public IList<TQuestActionInfo> ElseActionList;
        public string sElseSayMsg;
        public IList<string> ElseSayOldLabelList;
        public IList<string> ElseSayNewLabelList;

        public TSayingProcedure()
        {
            this.ConditionList = new List<TQuestConditionInfo>();
            this.ActionList = new List<TQuestActionInfo>();
            this.SayOldLabelList = new List<string>();
            this.SayNewLabelList = new List<string>();
            this.ElseActionList = new List<TQuestActionInfo>();
            this.ElseSayOldLabelList = new List<string>();
            this.ElseSayNewLabelList = new List<string>();
        }
    }

    public class TSayingRecord
    {
        public string sLabel;
        public IList<TSayingProcedure> ProcedureList;
        public bool boExtJmp;
    }

    public class TScriptParams
    {
        public string sParams;
        public int nParams;
    }
}

