using System;
using System.Collections;
using System.Collections.Generic;

namespace M2Server
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
        public int nPrice;
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
        public List<TQuestConditionInfo> ConditionList;
        public List<TQuestActionInfo> ActionList;
        public string sSayMsg;
        public List<string> SayOldLabelList;
        public List<string> SayNewLabelList;
        public List<TQuestActionInfo> ElseActionList;
        public string sElseSayMsg;
        public List<string> ElseSayOldLabelList;
        public List<string> ElseSayNewLabelList;

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
}

