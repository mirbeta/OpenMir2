using GameSrv.Npc;
using GameSrv.Player;

namespace GameSrv.ScriptSystem
{
    /// <summary>
    /// 脚本命令执行处理模块
    /// </summary>
    internal class ExecutionProcessingSys
    {
        /// <summary>
        /// 全局变量消息处理列表
        /// </summary>
        private static Dictionary<int, HandleExecutionMessage> ProcessExecutionMessage;

        public delegate void HandleExecutionMessage(PlayObject PlayObject, QuestActionInfo QuestActionInfo, ref bool Success);

        public void Initialize()
        {
            ProcessExecutionMessage = new Dictionary<int, HandleExecutionMessage>();
            ProcessExecutionMessage[ExecutionCodeDef.nSC_RANDOMMOVE] = ActionOfRandomMove;
        }

        public bool IsRegister(int cmdCode)
        {
            return ProcessExecutionMessage.ContainsKey(cmdCode);
        }

        public void Execute(PlayObject playObject, QuestActionInfo questConditionInfo, ref bool success)
        {
            if (ProcessExecutionMessage.ContainsKey(questConditionInfo.nCmdCode))
            {
                ProcessExecutionMessage[questConditionInfo.nCmdCode](playObject, questConditionInfo, ref success);
            }
        }

        internal void ActionOfRandomMove(PlayObject PlayObject, QuestActionInfo QuestActionInfo, ref bool Success)
        {
            Success = true;
        }
    }
}
