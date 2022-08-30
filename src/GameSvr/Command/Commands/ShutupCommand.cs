using GameSvr.Player;
using SystemModule;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 将指定人物禁言(支持权限分配)
    /// </summary>
    [GameCommand("Shutup", "将指定人物禁言", GameCommandConst.g_sGameCommandShutupHelpMsg, 10)]
    public class ShutupCommand : BaseCommond
    {
        [DefaultCommand]
        public void Shutup(string[] @Params, TPlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sHumanName = @Params.Length > 0 ? @Params[0] : "";
            var sTime = @Params.Length > 1 ? @Params[1] : "";
            if (sTime == "" || string.IsNullOrEmpty(sHumanName) ||
                !string.IsNullOrEmpty(sHumanName) && sHumanName[1] == '?')
            {
                PlayObject.SysMsg(string.Format(GameCommandConst.g_sGameCommandParamUnKnow, this.GameCommand.Name, GameCommandConst.g_sGameCommandShutupHelpMsg), MsgColor.Red, MsgType.Hint);
                return;
            }
            var dwTime = (uint)HUtil32.Str_ToInt(sTime, 5);
            HUtil32.EnterCriticalSection(M2Share.g_DenySayMsgList);
            try
            {
                //if (M2Share.g_DenySayMsgList.ContainsKey(sHumanName))
                //{
                //    M2Share.g_DenySayMsgList[sHumanName] = HUtil32.GetTickCount() + dwTime * 60 * 1000;
                //}
                //else
                //{
                //    M2Share.g_DenySayMsgList.Add(sHumanName, HUtil32.GetTickCount() + dwTime * 60 * 1000);
                //}

                //nIndex = M2Share.g_DenySayMsgList.GetIndex(sHumanName);
                //if (nIndex >= 0)
                //{
                //    M2Share.g_DenySayMsgList[nIndex] = ((HUtil32.GetTickCount() + dwTime * 60 * 1000) as Object);
                //}
                //else
                //{
                //    M2Share.g_DenySayMsgList.AddRecord(sHumanName, HUtil32.GetTickCount() + dwTime * 60 * 1000);
                //}
            }
            finally
            {
                HUtil32.LeaveCriticalSection(M2Share.g_DenySayMsgList);
            }
            PlayObject.SysMsg(string.Format(GameCommandConst.g_sGameCommandShutupHumanMsg, sHumanName, dwTime), MsgColor.Red, MsgType.Hint);
        }
    }
}