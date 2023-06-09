using M2Server.Maps;
using SystemModule;
using SystemModule;
using SystemModule.Enums;

namespace CommandSystem {
    /// <summary>
    /// 移动到某地图XY坐标处
    /// </summary>
    [Command("PositionMove", "移动到某地图XY坐标处", CommandHelp.GameCommandPositionMoveHelpMsg, 10)]
    public class PositionMoveCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor) {
            if (@params == null) {
                return;
            }
            try {
                var sMapName = @params.Length > 0 ? @params[0] : "";
                var sX = @params.Length > 1 ? @params[1] : "";
                var sY = @params.Length > 2 ? @params[2] : "";
                IEnvirnoment envir = null;
                if (string.IsNullOrEmpty(sMapName) || string.IsNullOrEmpty(sX) || string.IsNullOrEmpty(sY) || !string.IsNullOrEmpty(sMapName) && sMapName[0] == '?')
                {
                    PlayerActor.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                    return;
                }
                if (PlayerActor.Permission >= this.Command.PermissionMin || SystemShare.CanMoveMap(sMapName)) {
                    envir = SystemShare.MapMgr.FindMap(sMapName);
                    if (envir != null) {
                        var nX = HUtil32.StrToInt16(sX, 0);
                        var nY = HUtil32.StrToInt16(sY, 0);
                        if (envir.CanWalk(nX, nY, true)) {
                            PlayerActor.SpaceMove(sMapName, nX, nY, 0);
                        }
                        else {
                            PlayerActor.SysMsg(string.Format(CommandHelp.GameCommandPositionMoveCanotMoveToMap, sMapName, sX, sY), MsgColor.Green, MsgType.Hint);
                        }
                    }
                }
                else {
                    PlayerActor.SysMsg(string.Format(CommandHelp.TheMapDisableMove, sMapName, envir.MapDesc), MsgColor.Red, MsgType.Hint);
                }
            }
            catch (Exception e) {
                SystemShare.Logger.Error("[Exceptioin] PlayerActor.SysMsgCmdPositionMove");
                SystemShare.Logger.Error(e.Message);
            }
        }
    }
}