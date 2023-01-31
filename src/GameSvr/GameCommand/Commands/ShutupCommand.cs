using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 将指定人物禁言(支持权限分配)
    /// </summary>
    [Command("Shutup", "将指定人物禁言", CommandHelp.GameCommandShutupHelpMsg, 10)]
    public class ShutupCommand : Command
    {
        [ExecuteCommand]
        public void Shutup(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            string sHumanName = @Params.Length > 0 ? @Params[0] : "";
            string sTime = @Params.Length > 1 ? @Params[1] : "";
            if (sTime == "" || string.IsNullOrEmpty(sHumanName) ||
                !string.IsNullOrEmpty(sHumanName) && sHumanName[1] == '?')
            {
                PlayObject.SysMsg(string.Format(CommandHelp.GameCommandParamUnKnow, this.GameCommand.Name, CommandHelp.GameCommandShutupHelpMsg), MsgColor.Red, MsgType.Hint);
                return;
            }
            uint dwTime = (uint)HUtil32.StrToInt(sTime, 5);
            HUtil32.EnterCriticalSection(M2Share.DenySayMsgList);
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
                HUtil32.LeaveCriticalSection(M2Share.DenySayMsgList);
            }
            PlayObject.SysMsg(string.Format(CommandHelp.GameCommandShutupHumanMsg, sHumanName, dwTime), MsgColor.Red, MsgType.Hint);
        }
    }
}