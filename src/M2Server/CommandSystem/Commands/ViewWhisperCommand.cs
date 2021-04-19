using SystemModule;
using System;
using M2Server.CommandSystem;

namespace M2Server
{
    /// <summary>
    /// 监听指定玩家私聊信息
    /// </summary>
    [GameCommand("ViewWhisper", "监听指定玩家私聊信息", 10)]
    public class ViewWhisperCommand : BaseCommond
    {
        [DefaultCommand]
        public void ViewWhisper(string[] @Params, TPlayObject PlayObject)
        {
            var sCharName = @Params.Length > 0 ? @Params[0] : "";
            var sParam2 = @Params.Length > 1 ? @Params[1] : "";
            if ((sCharName == "") || ((sCharName != "") && (sCharName[1] == '?')))
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandParamUnKnow, this.Attributes.Name, M2Share.g_sGameCommandViewWhisperHelpMsg), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            var m_PlayObject = M2Share.UserEngine.GetPlayObject(sCharName);
            if (m_PlayObject != null)
            {
                if (m_PlayObject.m_GetWhisperHuman == PlayObject)
                {
                    m_PlayObject.m_GetWhisperHuman = null;
                    PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandViewWhisperMsg1, sCharName), TMsgColor.c_Green, TMsgType.t_Hint);
                }
                else
                {
                    m_PlayObject.m_GetWhisperHuman = PlayObject;
                    PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandViewWhisperMsg2, sCharName), TMsgColor.c_Green, TMsgType.t_Hint);
                }
            }
            else
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sNowNotOnLineOrOnOtherServer, sCharName), TMsgColor.c_Red, TMsgType.t_Hint);
            }
        }
    }
}