using GameSrv.Script;

namespace ScriptEngine
{
    public class ScriptInfo
    {
        public bool IsQuest;
        public ScriptQuestInfo[] QuestInfo;
        public Dictionary<string, SayingRecord> RecordList;
        public int QuestCount;
    }
    
    public struct QuestActionInfo {
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

    public struct QuestConditionInfo {
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
        public string sParam7;
        public int nParam7;
        public string sOpName;
        public string sOpHName;
    }

    public class SayingProcedure {
        public IList<QuestConditionInfo> ConditionList;
        public IList<QuestActionInfo> ActionList;
        public IList<QuestActionInfo> ElseActionList;
        public string sSayMsg;
        public string sElseSayMsg;

        public SayingProcedure() {
            ConditionList = new List<QuestConditionInfo>();
            ActionList = new List<QuestActionInfo>();
            ElseActionList = new List<QuestActionInfo>();
        }
    }

    public class SayingRecord {
        public string sLabel;
        public IList<SayingProcedure> ProcedureList;
        public bool boExtJmp;

        public SayingRecord() {
            ProcedureList = new List<SayingProcedure>();
        }
    }

    public class ScriptParams {
        public string sParams;
        public int nParams;
    }
}