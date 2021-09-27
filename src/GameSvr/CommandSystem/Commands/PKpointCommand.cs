using SystemModule;
using System;
using GameSvr.CommandSystem;

namespace GameSvr
{
    /// <summary>
    /// 查看指定玩家PK值
    /// </summary>
    [GameCommand("PKpoint", "查看指定玩家PK值", 10)]
    public class PKpointCommand : BaseCommond
    {
        [DefaultCommand]
        public void PKpoint(string[] @Params, TPlayObject PlayObject)
        {
            var sHumanName = @Params.Length > 0 ? @Params[0] : "";
            if (!string.IsNullOrEmpty(sHumanName) && sHumanName[1] == '?')
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandParamUnKnow, this.Attributes.Name, M2Share.g_sGameCommandPKPointHelpMsg), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            var m_PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (m_PlayObject == null)
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumanName), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandPKPointMsg, sHumanName, m_PlayObject.m_nPkPoint), TMsgColor.c_Green, TMsgType.t_Hint);
        }
    }
}