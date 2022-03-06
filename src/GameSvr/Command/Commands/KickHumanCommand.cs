using SystemModule;
using System;
using GameSvr.CommandSystem;

namespace GameSvr
{
    /// <summary>
    /// 剔除指定玩家下线
    /// </summary>
    [GameCommand("KickHuman", "剔除指定玩家下线", M2Share.g_sGameCommandHumanLocalHelpMsg, 10)]
    public class KickHumanCommand : BaseCommond
    {
        [DefaultCommand]
        public void KickHuman(string[] @params, TPlayObject PlayObject)
        {
            if (@params == null)
            {
                return;
            }
            var sHumName = @params.Length > 0 ? @params[0] : "";
            if (sHumName == "")
            {
                return;
            }
            var m_PlayObject = M2Share.UserEngine.GetPlayObject(sHumName);
            if (m_PlayObject != null)
            {
                m_PlayObject.m_boKickFlag = true;
                m_PlayObject.m_boEmergencyClose = true;
                //m_PlayObject.m_boPlayOffLine = false;
                //m_PlayObject.m_boNotOnlineAddExp = false;
            }
            else
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumName), MsgColor.Red, MsgType.Hint);
            }
        }
    }
}