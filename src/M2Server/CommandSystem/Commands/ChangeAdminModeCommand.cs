using SystemModule;
using M2Server.CommandSystem;

namespace M2Server
{
    /// <summary>
    /// 调整当前玩家管理模式
    /// </summary>
    [GameCommand("ChangeAdminMode", "调整当前玩家管理模式", 10)]
    public class ChangeAdminModeCommand : BaseCommond
    {
        public void ChangeAdminMode(string[] @Params, TPlayObject PlayObject)
        {
            var nPermission = @Params.Length > 0 ? int.Parse(Params[0]) : 0;
            var sParam1 = @Params.Length > 1 ? Params[1] : "";
            var boFlag = @Params.Length > 2 && bool.Parse(Params[2]);
            if (PlayObject.m_btPermission < nPermission)
            {
                PlayObject.SysMsg(M2Share.g_sGameCommandPermissionTooLow, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if ((sParam1 != "") && (sParam1[0] == '?'))
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