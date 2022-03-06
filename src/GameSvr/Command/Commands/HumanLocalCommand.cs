using SystemModule;
using System;
using GameSvr.CommandSystem;

namespace GameSvr
{
    /// <summary>
    /// 查看指定玩家所在IP地址
    /// </summary>
    [GameCommand("HumanLocal", "查看指定玩家所在IP地址", M2Share.g_sGameCommandHumanLocalHelpMsg, 10)]
    public class HumanLocalCommand : BaseCommond
    {
        [DefaultCommand]
        public void HumanLocal(string[] @Params, TPlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sHumanName = @Params.Length > 0 ? @Params[0] : "";
            var m_sIPLocal = "";
            if (string.IsNullOrEmpty(sHumanName) || !string.IsNullOrEmpty(sHumanName))
            {
                PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var m_PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (m_PlayObject == null)
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                return;
            }
            // GetIPLocal(PlayObject.m_sIPaddr)
            PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandHumanLocalMsg, sHumanName, m_sIPLocal), MsgColor.Green, MsgType.Hint);
        }
    }
}