using SystemModule;
using System;
using GameSvr.CommandSystem;

namespace GameSvr
{
    /// <summary>
    /// 调整当前玩家进入无敌模式
    /// </summary>
    [GameCommand("ChangeSuperManMode", "调整当前玩家进入无敌模式", 10)]
    public class ChangeSuperManModeCommand : BaseCommond
    {
        [DefaultCommand]
        public void ChangeSuperManMode(string[] @Params, TPlayObject PlayObject)
        {
            if (Params != null && Params.Length > 0)
            {
                var sParam1 = @Params.Length > 0 ? @Params[0] : "";
                if (!string.IsNullOrEmpty(sParam1) && sParam1[0] == '?')
                {
                    PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandParamUnKnow, this.Attributes.Name, ""), TMsgColor.c_Red, TMsgType.t_Hint);
                    return;
                }
            }
            var boFlag = !PlayObject.m_boSuperMan;
            PlayObject.m_boSuperMan = boFlag;
            if (PlayObject.m_boSuperMan)
            {
                PlayObject.SysMsg(M2Share.sSupermanMode, TMsgColor.c_Green, TMsgType.t_Hint);
            }
            else
            {
                PlayObject.SysMsg(M2Share.sReleaseSupermanMode, TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }
    }
}