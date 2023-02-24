using GameSvr.Player;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands {
    /// <summary>
    /// 移动地图指定座标(需要戴传送装备)
    /// </summary>
    [Command("UserMoveXY", "移动地图指定座标(需要戴传送装备)", 0)]
    public class UserMoveXYCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @Params, PlayObject PlayObject) {
            if (@Params == null) {
                return;
            }
            string sX = @Params.Length > 0 ? @Params[0] : "";
            string sY = @Params.Length > 1 ? @Params[1] : "";
            if (PlayObject.Teleport) {
                short nX = HUtil32.StrToInt16(sX, -1);
                short nY = HUtil32.StrToInt16(sY, -1);
                if (!PlayObject.Envir.Flag.boNOPOSITIONMOVE) {
                    if (PlayObject.Envir.CanWalkOfItem(nX, nY, M2Share.Config.boUserMoveCanDupObj, M2Share.Config.boUserMoveCanOnItem)) {
                        if ((HUtil32.GetTickCount() - PlayObject.TeleportTick) > M2Share.Config.dwUserMoveTime * 1000) {
                            PlayObject.TeleportTick = HUtil32.GetTickCount();
                            PlayObject.SendRefMsg(Messages.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
                            PlayObject.SpaceMove(PlayObject.MapName, nX, nY, 0);
                        }
                        else {
                            PlayObject.SysMsg(M2Share.Config.dwUserMoveTime - (HUtil32.GetTickCount() - PlayObject.TeleportTick) / 1000 + "秒之后才可以再使用此功能!!!", MsgColor.Red, MsgType.Hint);
                        }
                    }
                    else {
                        PlayObject.SysMsg(string.Format(CommandHelp.GameCommandPositionMoveCanotMoveToMap, PlayObject.MapName, sX, sY), MsgColor.Green, MsgType.Hint);
                    }
                }
                else {
                    PlayObject.SysMsg("此地图禁止使用此命令!!!", MsgColor.Red, MsgType.Hint);
                }
            }
            else {
                PlayObject.SysMsg("您现在还无法使用此功能!!!", MsgColor.Red, MsgType.Hint);
            }
        }
    }
}