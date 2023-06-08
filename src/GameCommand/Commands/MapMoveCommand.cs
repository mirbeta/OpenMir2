using M2Server.Maps;
using M2Server.Player;
using SystemModule;
using SystemModule.Enums;

namespace M2Server.GameCommand.Commands {
    /// <summary>
    /// 在指定地图随机移动
    /// </summary>
    [Command("MapMove", "在指定地图随机移动", CommandHelp.GameCommandMoveHelpMsg, 10)]
    public class MapMoveCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null) {
                return;
            }
            var sMapName = @params.Length > 0 ? @params[0] : "";
            if (string.IsNullOrEmpty(sMapName)) {
                playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var envir = M2Share.MapMgr.FindMap(sMapName);
            if (envir == null) {
                playObject.SysMsg(string.Format(CommandHelp.TheMapNotFound, sMapName), MsgColor.Red, MsgType.Hint);
                return;
            }
            if (playObject.Permission >= this.Command.PermissionMin || M2Share.CanMoveMap(sMapName)) {
                playObject.SendRefMsg(Messages.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
                playObject.MapRandomMove(sMapName, 0);
            }
            else {
                playObject.SysMsg(string.Format(CommandHelp.TheMapDisableMove, sMapName, envir.MapDesc), MsgColor.Red, MsgType.Hint);//不允许传送
            }
        }
    }
}