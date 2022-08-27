using GameSvr.Npc;
using GameSvr.Player;

namespace GameSvr.ScriptSystem
{
    public class ScriptEngine
    {
        public delegate void ScriptCondition(TPlayObject PlayObject, TQuestConditionInfo QuestConditionInfo, ref bool Result);
        public delegate void ScriptAction(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo, ref bool Result);


        public ScriptEngine()
        {

        }

        public void Condition(NormNpc normNpc, TPlayObject playObject, TQuestConditionInfo questConditionInfo, ref bool result)
        {

        }

        public void Action(NormNpc normNpc, TPlayObject playObject, TQuestActionInfo questActionInfo, ref bool result)
        {

        }
    }
}