using GameSvr.Player;
using SystemModule;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 移动地图指定座标(需要戴传送装备)
    /// </summary>
    [GameCommand("UserMoveXY", "移动地图指定座标(需要戴传送装备)", 0)]
    public class UserMoveXYCommand : BaseCommond
    {
        [DefaultCommand]
        public void UserMoveXY(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sX = @Params.Length > 0 ? @Params[0] : "";
            var sY = @Params.Length > 1 ? @Params[1] : "";
            if (PlayObject.Teleport)
            {
                short nX = (short)HUtil32.Str_ToInt(sX, -1);
                short nY = (short)HUtil32.Str_ToInt(sY, -1);
                if (!PlayObject.Envir.Flag.boNOPOSITIONMOVE)
                {
                    if (PlayObject.Envir.CanWalkOfItem(nX, nY, M2Share.Config.boUserMoveCanDupObj, M2Share.Config.boUserMoveCanOnItem))
                    {
                        if ((HUtil32.GetTickCount() - PlayObject.TeleportTick) > M2Share.Config.dwUserMoveTime * 1000)
                        {
                            PlayObject.TeleportTick = HUtil32.GetTickCount();
                            PlayObject.SendRefMsg(Grobal2.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
                            PlayObject.SpaceMove(PlayObject.MapName, nX, nY, 0);
                        }
                        else
                        {
                            PlayObject.SysMsg(M2Share.Config.dwUserMoveTime - (HUtil32.GetTickCount() - PlayObject.TeleportTick) / 1000 + "秒之后才可以再使用此功能!!!", MsgColor.Red, MsgType.Hint);
                        }
                    }
                    else
                    {
                        PlayObject.SysMsg(string.Format(GameCommandConst.GameCommandPositionMoveCanotMoveToMap, PlayObject.MapName, sX, sY), MsgColor.Green, MsgType.Hint);
                    }
                }
                else
                {
                    PlayObject.SysMsg("此地图禁止使用此命令!!!", MsgColor.Red, MsgType.Hint);
                }
            }
            else
            {
                PlayObject.SysMsg("您现在还无法使用此功能!!!", MsgColor.Red, MsgType.Hint);
            }
        }
    }
}