using SystemModule;
using System;
using GameSvr.CommandSystem;

namespace GameSvr
{
    /// <summary>
    /// 移动到某地图XY坐标处
    /// </summary>
    [GameCommand("PositionMove", "移动到某地图XY坐标处", M2Share.g_sGameCommandPositionMoveHelpMsg, 10)]
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
                    PlayObject.SysMsg(CommandAttribute.CommandHelp(), TMsgColor.c_Red, TMsgType.t_Hint);
                    return;
                }
                if (PlayObject.m_btPermission >= this.CommandAttribute.nPermissionMin || M2Share.CanMoveMap(sMapName))
                {
                    Envir = M2Share.g_MapManager.FindMap(sMapName);
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
                            PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandPositionMoveCanotMoveToMap, sMapName, sX, sY), TMsgColor.c_Green, TMsgType.t_Hint);
                        }
                    }
                }
                else
                {
                    PlayObject.SysMsg(string.Format(M2Share.g_sTheMapDisableMove, sMapName, Envir.sMapDesc), TMsgColor.c_Red, TMsgType.t_Hint);
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