using GameSvr.Maps;
using GameSvr.Player;
using SystemModule;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 移动到某地图XY坐标处
    /// </summary>
    [Command("PositionMove", "移动到某地图XY坐标处", CommandHelp.GameCommandPositionMoveHelpMsg, 10)]
    public class PositionMoveCommand : Commond
    {
        [ExecuteCommand]
        public void PositionMove(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            try
            {
                var sMapName = @Params.Length > 0 ? @Params[0] : "";
                var sX = @Params.Length > 1 ? @Params[1] : "";
                var sY = @Params.Length > 2 ? @Params[2] : "";
                Envirnoment Envir = null;
                if (sMapName == "" || sX == "" || sY == "" || sMapName != "" && sMapName[0] == '?')
                {
                    PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                    return;
                }
                if (PlayObject.Permission >= this.GameCommand.nPermissionMin || M2Share.CanMoveMap(sMapName))
                {
                    Envir = M2Share.MapMgr.FindMap(sMapName);
                    if (Envir != null)
                    {
                        var nX = (short)HUtil32.StrToInt(sX, 0);
                        var nY = (short)HUtil32.StrToInt(sY, 0);
                        if (Envir.CanWalk(nX, nY, true))
                        {
                            PlayObject.SpaceMove(sMapName, nX, nY, 0);
                        }
                        else
                        {
                            PlayObject.SysMsg(string.Format(CommandHelp.GameCommandPositionMoveCanotMoveToMap, sMapName, sX, sY), MsgColor.Green, MsgType.Hint);
                        }
                    }
                }
                else
                {
                    PlayObject.SysMsg(string.Format(CommandHelp.TheMapDisableMove, sMapName, Envir.MapDesc), MsgColor.Red, MsgType.Hint);
                }
            }
            catch (Exception e)
            {
                M2Share.Log.Error("[Exceptioin] TPlayObject.CmdPositionMove");
                M2Share.Log.Error(e.Message);
            }
        }
    }
}