using GameSvr.Npc;

namespace GameSvr.Script {

    public class TScript {
        public bool boQuest;
        public TScriptQuestInfo[] QuestInfo;
        public Dictionary<string, SayingRecord> RecordList;
        public int nQuest;
    }

    public struct TScriptQuestInfo {
        public short wFlag;
        public byte btValue;
        public int nRandRage;
    }
}