using GameSrv.Maps;
using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 移动到某地图XY坐标处
    /// </summary>
    [Command("PositionMove", "移动到某地图XY坐标处", CommandHelp.GameCommandPositionMoveHelpMsg, 10)]
    public class PositionMoveCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null) {
                return;
            }
            try {
                string sMapName = @params.Length > 0 ? @params[0] : "";
                string sX = @params.Length > 1 ? @params[1] : "";
                string sY = @params.Length > 2 ? @params[2] : "";
                Envirnoment envir = null;
                if (string.IsNullOrEmpty(sMapName) || string.IsNullOrEmpty(sX) || string.IsNullOrEmpty(sY) || !string.IsNullOrEmpty(sMapName) && sMapName[0] == '?')
                {
                    playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                    return;
                }
                if (playObject.Permission >= this.Command.PermissionMin || M2Share.CanMoveMap(sMapName)) {
                    envir = M2Share.MapMgr.FindMap(sMapName);
                    if (envir != null) {
                        short nX = HUtil32.StrToInt16(sX, 0);
                        short nY = HUtil32.StrToInt16(sY, 0);
                        if (envir.CanWalk(nX, nY, true)) {
                            playObject.SpaceMove(sMapName, nX, nY, 0);
                        }
                        else {
                            playObject.SysMsg(string.Format(CommandHelp.GameCommandPositionMoveCanotMoveToMap, sMapName, sX, sY), MsgColor.Green, MsgType.Hint);
                        }
                    }
                }
                else {
                    playObject.SysMsg(string.Format(CommandHelp.TheMapDisableMove, sMapName, envir.MapDesc), MsgColor.Red, MsgType.Hint);
                }
            }
            catch (Exception e) {
                M2Share.Logger.Error("[Exceptioin] TPlayObject.CmdPositionMove");
                M2Share.Logger.Error(e.Message);
            }
        }
    }
}