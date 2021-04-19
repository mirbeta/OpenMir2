using SystemModule;
using System;
using M2Server.CommandSystem;

namespace M2Server
{
    /// <summary>
    /// 清除指定玩家PK值
    /// </summary>
    [GameCommand("FreePenalty", "清除指定玩家PK值", 10)]
    public class FreePenaltyCommand : BaseCommond
    {
        [DefaultCommand]
        public void FreePenalty(string[] @Params, TPlayObject PlayObject)
        {
            var sHumanName = @Params.Length > 0 ? @Params[0] : "";

            if (sHumanName != "" && sHumanName[0] == '?')
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandParamUnKnow, this.Attributes.Name, M2Share.g_sGameCommandFreePKHelpMsg), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            var m_PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (m_PlayObject == null)
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumanName), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            m_PlayObject.m_nPkPoint = 0;
            m_PlayObject.RefNameColor();
            m_PlayObject.SysMsg(M2Share.g_sGameCommandFreePKHumanMsg, TMsgColor.c_Green, TMsgType.t_Hint);
            PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandFreePKMsg, sHumanName), TMsgColor.c_Green, TMsgType.t_Hint);
        }
    }
}