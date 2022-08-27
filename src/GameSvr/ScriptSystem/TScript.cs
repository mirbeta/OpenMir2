using GameSvr.Npc;

namespace GameSvr.ScriptSystem
{
    public class TScript
    {
        public bool boQuest;
        public TScriptQuestInfo[] QuestInfo;
        public Dictionary<string, TSayingRecord> RecordList;
        public int nQuest;
    }
}