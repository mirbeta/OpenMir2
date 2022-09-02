using GameSvr.Npc;
using GameSvr.Player;

namespace GameSvr.Script
{
    public class ScriptEngine
    {
        public delegate void ScriptCondition(PlayObject PlayObject, TQuestConditionInfo QuestConditionInfo, ref bool Result);
        public delegate void ScriptAction(PlayObject PlayObject, TQuestActionInfo QuestActionInfo, ref bool Result);


        public ScriptEngine()
        {

        }

        public void Condition(NormNpc normNpc, PlayObject playObject, TQuestConditionInfo questConditionInfo, ref bool result)
        {

        }

        public void Action(NormNpc normNpc, PlayObject playObject, TQuestActionInfo questActionInfo, ref bool result)
        {

        }
    }
}