using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 调整当前玩家管理模式
    /// </summary>
    [Command("ChangeAdminMode", "进入/退出管理员模式(进入模式后不会受到任何角色攻击)", 10)]
    public class ChangeAdminModeCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject PlayObject) {
            PlayObject.AdminMode = !PlayObject.AdminMode;
            PlayObject.SysMsg(PlayObject.AdminMode ? Settings.GameMasterMode : Settings.ReleaseGameMasterMode, MsgColor.Green, MsgType.Hint);
        }
    }
}