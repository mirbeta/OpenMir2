using GameSvr.Player;
using SystemModule;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 取用户任务状态
    /// </summary>
    [GameCommand("ShowHumanFlag", "取用户任务状态", GameCommandConst.g_sGameCommandShowHumanFlagHelpMsg, 10)]
    public class ShowHumanFlagCommand : BaseCommond
    {
        [DefaultCommand]
        public void ShowHumanFlag(string[] @Params, TPlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sHumanName = @Params.Length > 0 ? @Params[0] : "";
            var sFlag = @Params.Length > 1 ? @Params[1] : "";
            if (string.IsNullOrEmpty(sHumanName) || !string.IsNullOrEmpty(sHumanName) && sHumanName[0] == '?')
            {
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var m_PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (m_PlayObject == null)
            {
                PlayObject.SysMsg(string.Format(GameCommandConst.g_sNowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                return;
            }
            var nFlag = HUtil32.Str_ToInt(sFlag, 0);
            if (m_PlayObject.GetQuestFalgStatus(nFlag) == 1)
            {
                PlayObject.SysMsg(string.Format(GameCommandConst.g_sGameCommandShowHumanFlagONMsg, m_PlayObject.m_sCharName, nFlag), MsgColor.Green, MsgType.Hint);
            }
            else
            {
                PlayObject.SysMsg(string.Format(GameCommandConst.g_sGameCommandShowHumanFlagOFFMsg, m_PlayObject.m_sCharName, nFlag), MsgColor.Green, MsgType.Hint);
            }
        }
    }
}