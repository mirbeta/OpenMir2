using SystemModule;
using System;
using GameSvr.CommandSystem;

namespace GameSvr
{
    /// <summary>
    /// 复活指定玩家
    /// </summary>
    [GameCommand("ReAlive", "复活指定玩家", 10)]
    public class ReAliveCommand : BaseCommond
    {
        [DefaultCommand]
        public void ReAlive(string[] @params, TPlayObject PlayObject)
        {
            var sHumanName = @params.Length > 0 ? @params[0] : "";
            if (sHumanName == "")
            {
                PlayObject.SysMsg("命令格式不正确", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            var m_PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (m_PlayObject == null)
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumanName), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            m_PlayObject.ReAlive();
            m_PlayObject.m_WAbil.HP = m_PlayObject.m_WAbil.MaxHP;
            m_PlayObject.SendMsg(m_PlayObject, Grobal2.RM_ABILITY, 0, 0, 0, 0, "");
            PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandReAliveMsg, sHumanName), TMsgColor.c_Green, TMsgType.t_Hint);
            PlayObject.SysMsg(sHumanName + " 已获重生。", TMsgColor.c_Green, TMsgType.t_Hint);
        }
    }
}