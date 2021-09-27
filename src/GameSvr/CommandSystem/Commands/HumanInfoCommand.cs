using SystemModule;
using System;
using GameSvr.CommandSystem;

namespace GameSvr
{
    /// <summary>
    /// 查看指定玩家信息
    /// </summary>
    [GameCommand("HumanInfo", "查看指定玩家信息", 10)]
    public class HumanInfoCommand : BaseCommond
    {
        [DefaultCommand]
        public void HumanInfo(string[] @Params, TPlayObject PlayObject)
        {
            var sHumanName = @Params.Length > 0 ? @Params[0] : "";
            if (string.IsNullOrEmpty(sHumanName) || !string.IsNullOrEmpty(sHumanName) && sHumanName[1] == '?')
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandParamUnKnow, this.Attributes.Name, M2Share.g_sGameCommandInfoHelpMsg), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            var m_PlayObject = M2Share.UserEngine.GetPlayObject(sHumanName);
            if (m_PlayObject == null)
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sNowNotOnLineOrOnOtherServer, sHumanName), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            PlayObject.SysMsg(PlayObject.GeTBaseObjectInfo(), TMsgColor.c_Green, TMsgType.t_Hint);
        }
    }
}