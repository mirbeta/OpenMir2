using GameSvr.Npc;
using GameSvr.Player;

namespace GameSvr.Script
{
    public class ScriptEngine
    {
        public delegate void ScriptCondition(PlayObject PlayObject, QuestConditionInfo QuestConditionInfo, ref bool Result);
        public delegate void ScriptAction(PlayObject PlayObject, QuestActionInfo QuestActionInfo, ref bool Result);


        public ScriptEngine()
        {

        }

        public static void Condition(NormNpc normNpc, PlayObject playObject, QuestConditionInfo questConditionInfo, ref bool result)
        {

        }

        public static void Action(NormNpc normNpc, PlayObject playObject, QuestActionInfo questActionInfo, ref bool result)
        {

        }
    }
}