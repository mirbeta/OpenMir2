using OpenMir2;
using SystemModule;
using SystemModule.Actors;
using SystemModule.Enums;

namespace CommandModule.Commands
{
    /// <summary>
    /// 移动地图指定座标(需要戴传送装备)
    /// </summary>
    [Command("UserMoveXY", "移动地图指定座标(需要戴传送装备)")]
    public class UserMoveXyCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (@params == null)
            {
                return;
            }
            string sX = @params.Length > 0 ? @params[0] : "";
            string sY = @params.Length > 1 ? @params[1] : "";
            if (PlayerActor.Teleport)
            {
                short nX = HUtil32.StrToInt16(sX, -1);
                short nY = HUtil32.StrToInt16(sY, -1);
                if (!PlayerActor.Envir.Flag.boNOPOSITIONMOVE)
                {
                    if (PlayerActor.Envir.CanWalkOfItem(nX, nY, SystemShare.Config.boUserMoveCanDupObj, SystemShare.Config.boUserMoveCanOnItem))
                    {
                        if ((HUtil32.GetTickCount() - PlayerActor.TeleportTick) > SystemShare.Config.dwUserMoveTime * 1000)
                        {
                            PlayerActor.TeleportTick = HUtil32.GetTickCount();
                            PlayerActor.SendRefMsg(Messages.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
                            PlayerActor.SpaceMove(PlayerActor.MapName, nX, nY, 0);
                        }
                        else
                        {
                            PlayerActor.SysMsg(SystemShare.Config.dwUserMoveTime - (HUtil32.GetTickCount() - PlayerActor.TeleportTick) / 1000 + "秒之后才可以再使用此功能!!!", MsgColor.Red, MsgType.Hint);
                        }
                    }
                    else
                    {
                        PlayerActor.SysMsg(string.Format(CommandHelp.GameCommandPositionMoveCanotMoveToMap, PlayerActor.MapName, sX, sY), MsgColor.Green, MsgType.Hint);
                    }
                }
                else
                {
                    PlayerActor.SysMsg("此地图禁止使用此命令!!!", MsgColor.Red, MsgType.Hint);
                }
            }
            else
            {
                PlayerActor.SysMsg("您现在还无法使用此功能!!!", MsgColor.Red, MsgType.Hint);
            }
        }
    }
}