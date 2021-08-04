using System.Collections.Generic;

namespace M2Server
{
    public class TScript
    {
        public bool boQuest;
        public TScriptQuestInfo[] QuestInfo;
        public Dictionary<string, TSayingRecord> RecordList;
        public int nQuest;
    }
}