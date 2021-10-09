using SystemModule;
using GameSvr.CommandSystem;

namespace GameSvr
{
    /// <summary>
    /// 调整当前玩家管理模式
    /// </summary>
    [GameCommand("ChangeAdminMode", "调整当前玩家管理模式", 10)]
    public class ChangeAdminModeCommand : BaseCommond
    {
        public void ChangeAdminMode(string[] @Params, TPlayObject PlayObject)
        {
            var sParam1 = @Params.Length > 0 ? Params[0] : "";
            var boFlag = !PlayObject.m_boAdminMode;
            if (sParam1 != "" && sParam1[0] == '?')
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandParamUnKnow, this.Attributes.Name, ""), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            PlayObject.m_boAdminMode = boFlag;
            PlayObject.SysMsg(PlayObject.m_boAdminMode ? M2Share.sGameMasterMode : M2Share.sReleaseGameMasterMode,
                TMsgColor.c_Green, TMsgType.t_Hint);
        }
    }
}