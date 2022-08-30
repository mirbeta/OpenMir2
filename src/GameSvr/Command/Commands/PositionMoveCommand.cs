using GameSvr.Maps;
using GameSvr.Player;
using SystemModule;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 移动到某地图XY坐标处
    /// </summary>
    [GameCommand("PositionMove", "移动到某地图XY坐标处", GameCommandConst.g_sGameCommandPositionMoveHelpMsg, 10)]
    public class PositionMoveCommand : BaseCommond
    {
        [DefaultCommand]
        public void PositionMove(string[] @Params, TPlayObject PlayObject)
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
                if (PlayObject.m_btPermission >= this.GameCommand.nPermissionMin || M2Share.CanMoveMap(sMapName))
                {
                    Envir = M2Share.MapManager.FindMap(sMapName);
                    if (Envir != null)
                    {
                        var nX = (short)HUtil32.Str_ToInt(sX, 0);
                        var nY = (short)HUtil32.Str_ToInt(sY, 0);
                        if (Envir.CanWalk(nX, nY, true))
                        {
                            PlayObject.SpaceMove(sMapName, nX, nY, 0);
                        }
                        else
                        {
                            PlayObject.SysMsg(string.Format(GameCommandConst.g_sGameCommandPositionMoveCanotMoveToMap, sMapName, sX, sY), MsgColor.Green, MsgType.Hint);
                        }
                    }
                }
                else
                {
                    PlayObject.SysMsg(string.Format(M2Share.g_sTheMapDisableMove, sMapName, Envir.SMapDesc), MsgColor.Red, MsgType.Hint);
                }
            }
            catch (Exception e)
            {
                M2Share.ErrorMessage("[Exceptioin] TPlayObject.CmdPositionMove");
                M2Share.ErrorMessage(e.Message);
            }
        }
    }
}