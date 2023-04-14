using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 移动地图指定座标(需要戴传送装备)
    /// </summary>
    [Command("UserMoveXY", "移动地图指定座标(需要戴传送装备)")]
    public class UserMoveXyCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null) {
                return;
            }
            var sX = @params.Length > 0 ? @params[0] : "";
            var sY = @params.Length > 1 ? @params[1] : "";
            if (playObject.Teleport) {
                var nX = HUtil32.StrToInt16(sX, -1);
                var nY = HUtil32.StrToInt16(sY, -1);
                if (!playObject.Envir.Flag.boNOPOSITIONMOVE) {
                    if (playObject.Envir.CanWalkOfItem(nX, nY, M2Share.Config.boUserMoveCanDupObj, M2Share.Config.boUserMoveCanOnItem)) {
                        if ((HUtil32.GetTickCount() - playObject.TeleportTick) > M2Share.Config.dwUserMoveTime * 1000) {
                            playObject.TeleportTick = HUtil32.GetTickCount();
                            playObject.SendRefMsg(Messages.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
                            playObject.SpaceMove(playObject.MapName, nX, nY, 0);
                        }
                        else {
                            playObject.SysMsg(M2Share.Config.dwUserMoveTime - (HUtil32.GetTickCount() - playObject.TeleportTick) / 1000 + "秒之后才可以再使用此功能!!!", MsgColor.Red, MsgType.Hint);
                        }
                    }
                    else {
                        playObject.SysMsg(string.Format(CommandHelp.GameCommandPositionMoveCanotMoveToMap, playObject.MapName, sX, sY), MsgColor.Green, MsgType.Hint);
                    }
                }
                else {
                    playObject.SysMsg("此地图禁止使用此命令!!!", MsgColor.Red, MsgType.Hint);
                }
            }
            else {
                playObject.SysMsg("您现在还无法使用此功能!!!", MsgColor.Red, MsgType.Hint);
            }
        }
    }
}