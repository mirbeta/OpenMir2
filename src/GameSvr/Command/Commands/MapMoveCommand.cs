using GameSvr.Player;
using SystemModule;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 在指定地图随机移动
    /// </summary>
    [GameCommand("MapMove", "在指定地图随机移动", GameCommandConst.g_sGameCommandMoveHelpMsg, 10)]
    public class MapMoveCommand : BaseCommond
    {
        [DefaultCommand]
        public void MapMove(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sMapName = @Params.Length > 0 ? @Params[0] : "";
            if (string.IsNullOrEmpty(sMapName))
            {
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var Envir = M2Share.MapManager.FindMap(sMapName);
            if (Envir == null)
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sTheMapNotFound, sMapName), MsgColor.Red, MsgType.Hint);
                return;
            }
            if (PlayObject.Permission >= this.GameCommand.nPermissionMin || M2Share.CanMoveMap(sMapName))
            {
                PlayObject.SendRefMsg(Grobal2.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
                PlayObject.MapRandomMove(sMapName, 0);
            }
            else
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sTheMapDisableMove, sMapName, Envir.MapDesc), MsgColor.Red, MsgType.Hint);//不允许传送
            }
        }
    }
}