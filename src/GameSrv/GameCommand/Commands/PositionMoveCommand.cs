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
        public void Execute(string[] @Params, PlayObject PlayObject) {
            if (@Params == null) {
                return;
            }
            try {
                string sMapName = @Params.Length > 0 ? @Params[0] : "";
                string sX = @Params.Length > 1 ? @Params[1] : "";
                string sY = @Params.Length > 2 ? @Params[2] : "";
                Envirnoment Envir = null;
                if (string.IsNullOrEmpty(sMapName) || string.IsNullOrEmpty(sX) || string.IsNullOrEmpty(sY) || !string.IsNullOrEmpty(sMapName) && sMapName[0] == '?')
                {
                    PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                    return;
                }
                if (PlayObject.Permission >= this.Command.PermissionMin || M2Share.CanMoveMap(sMapName)) {
                    Envir = M2Share.MapMgr.FindMap(sMapName);
                    if (Envir != null) {
                        short nX = HUtil32.StrToInt16(sX, 0);
                        short nY = HUtil32.StrToInt16(sY, 0);
                        if (Envir.CanWalk(nX, nY, true)) {
                            PlayObject.SpaceMove(sMapName, nX, nY, 0);
                        }
                        else {
                            PlayObject.SysMsg(string.Format(CommandHelp.GameCommandPositionMoveCanotMoveToMap, sMapName, sX, sY), MsgColor.Green, MsgType.Hint);
                        }
                    }
                }
                else {
                    PlayObject.SysMsg(string.Format(CommandHelp.TheMapDisableMove, sMapName, Envir.MapDesc), MsgColor.Red, MsgType.Hint);
                }
            }
            catch (Exception e) {
                M2Share.Logger.Error("[Exceptioin] TPlayObject.CmdPositionMove");
                M2Share.Logger.Error(e.Message);
            }
        }
    }
}