using SystemModule;
using SystemModule.Enums;

namespace CommandSystem.Commands
{
    /// <summary>
    /// 将指定人物禁言(支持权限分配)
    /// </summary>
    [Command("Shutup", "将指定人物禁言", CommandHelp.GameCommandShutupHelpMsg, 10)]
    public class ShutupCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (@params == null)
            {
                return;
            }
            var sHumanName = @params.Length > 0 ? @params[0] : "";
            var sTime = @params.Length > 1 ? @params[1] : "";
            if (string.IsNullOrEmpty(sTime) || string.IsNullOrEmpty(sHumanName) ||
                !string.IsNullOrEmpty(sHumanName) && sHumanName[1] == '?')
            {
                PlayerActor.SysMsg(string.Format(CommandHelp.GameCommandParamUnKnow, this.Command.Name, CommandHelp.GameCommandShutupHelpMsg), MsgColor.Red, MsgType.Hint);
                return;
            }
            var dwTime = (uint)HUtil32.StrToInt(sTime, 5);
            HUtil32.EnterCriticalSection(SystemShare.DenySayMsgList);
            try
            {
                //if (Settings.g_DenySayMsgList.ContainsKey(sHumanName))
                //{
                //    Settings.g_DenySayMsgList[sHumanName] = HUtil32.GetTickCount() + dwTime * 60 * 1000;
                //}
                //else
                //{
                //    Settings.g_DenySayMsgList.Add(sHumanName, HUtil32.GetTickCount() + dwTime * 60 * 1000);
                //}

                //nIndex = Settings.g_DenySayMsgList.GetIndex(sHumanName);
                //if (nIndex >= 0)
                //{
                //    Settings.g_DenySayMsgList[nIndex] = ((HUtil32.GetTickCount() + dwTime * 60 * 1000) as Object);
                //}
                //else
                //{
                //    Settings.g_DenySayMsgList.AddRecord(sHumanName, HUtil32.GetTickCount() + dwTime * 60 * 1000);
                //}
            }
            finally
            {
                HUtil32.LeaveCriticalSection(SystemShare.DenySayMsgList);
            }
            PlayerActor.SysMsg(string.Format(CommandHelp.GameCommandShutupHumanMsg, sHumanName, dwTime), MsgColor.Red, MsgType.Hint);
        }
    }
}