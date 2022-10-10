using GameSvr.Player;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 调整当前玩家管理模式
    /// </summary>
    [Command("ChangeAdminMode", "进入/退出管理员模式(进入模式后不会受到任何角色攻击)", 10)]
    public class ChangeAdminModeCommand : Command
    {
        [ExecuteCommand]
        public void ChangeAdminMode(PlayObject PlayObject)
        {
            PlayObject.AdminMode = !PlayObject.AdminMode;
            PlayObject.SysMsg(PlayObject.AdminMode ? M2Share.sGameMasterMode : M2Share.sReleaseGameMasterMode, MsgColor.Green, MsgType.Hint);
        }
    }
}