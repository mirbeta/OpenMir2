using SystemModule;
using System;
using GameSvr.CommandSystem;

namespace GameSvr
{
    /// <summary>
    /// 监听指定玩家私聊信息
    /// </summary>
    [GameCommand("ViewWhisper", "监听指定玩家私聊信息", M2Share.g_sGameCommandViewWhisperHelpMsg, 10)]
    public class ViewWhisperCommand : BaseCommond
    {
        [DefaultCommand]
        public void ViewWhisper(string[] @Params, TPlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sCharName = @Params.Length > 0 ? @Params[0] : "";
            if (sCharName == "" || sCharName != "" && sCharName[1] == '?')
            {
                PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var m_PlayObject = M2Share.UserEngine.GetPlayObject(sCharName);
            if (m_PlayObject != null)
            {
                if (m_PlayObject.m_GetWhisperHuman == PlayObject)
                {
                    m_PlayObject.m_GetWhisperHuman = null;
                    PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandViewWhisperMsg1, sCharName), MsgColor.Green, MsgType.Hint);
                }
                else
                {
                    m_PlayObject.m_GetWhisperHuman = PlayObject;
                    PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandViewWhisperMsg2, sCharName), MsgColor.Green, MsgType.Hint);
                }
            }
            else
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sNowNotOnLineOrOnOtherServer, sCharName), MsgColor.Red, MsgType.Hint);
            }
        }
    }
}