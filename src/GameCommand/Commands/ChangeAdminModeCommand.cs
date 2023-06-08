using M2Server.Player;
using M2Server;
using SystemModule.Enums;

namespace M2Server.GameCommand.Commands {
    /// <summary>
    /// 调整当前玩家管理模式
    /// </summary>
    [Command("ChangeAdminMode", "进入/退出管理员模式(进入模式后不会受到任何角色攻击)", 10)]
    public class ChangeAdminModeCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject playObject) {
            playObject.AdminMode = !playObject.AdminMode;
            playObject.SysMsg(playObject.AdminMode ? Settings.GameMasterMode : Settings.ReleaseGameMasterMode, MsgColor.Green, MsgType.Hint);
        }
    }
}