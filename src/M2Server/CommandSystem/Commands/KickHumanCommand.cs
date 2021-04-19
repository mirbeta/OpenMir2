﻿using SystemModule;
using System;
using M2Server.CommandSystem;

namespace M2Server
{
    /// <summary>
    /// 剔除指定玩家下线
    /// </summary>
    [GameCommand("KickHuman", "剔除指定玩家下线", 10)]
    public class KickHumanCommand : BaseCommond
    {
        [DefaultCommand]
        public void KickHuman(string[] @params, TPlayObject PlayObject)
        {
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
                PlayObject.SysMsg(string.Format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumName), TMsgColor.c_Red, TMsgType.t_Hint);
            }
        }
    }
}