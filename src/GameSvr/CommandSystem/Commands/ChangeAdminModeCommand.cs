using SystemModule;
using GameSvr.CommandSystem;

namespace GameSvr
{
    /// <summary>
    /// 调整当前玩家管理模式
    /// </summary>
    [GameCommand("ChangeAdminMode", "进入/退出管理员模式(进入模式后不会受到任何角色攻击)", 10)]
    public class ChangeAdminModeCommand : BaseCommond
    {
        public void ChangeAdminMode(string[] @Params, TPlayObject PlayObject)
        {
            if (Params != null && Params.Length > 0)
            {
                var sParam1 = @Params.Length > 0 ? Params[0] : "";
                if (sParam1 != "" && sParam1[0] == '?')
                {
                    PlayObject.SysMsg(CommandAttribute.CommandHelp(), TMsgColor.c_Red, TMsgType.t_Hint);
                    return;
                }
            }
            var boFlag = !PlayObject.m_boAdminMode;
            PlayObject.m_boAdminMode = boFlag;
            PlayObject.SysMsg(PlayObject.m_boAdminMode ? M2Share.sGameMasterMode : M2Share.sReleaseGameMasterMode,
                TMsgColor.c_Green, TMsgType.t_Hint);
        }
    }
}