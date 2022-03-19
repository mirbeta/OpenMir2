using GameSvr.CommandSystem;
using SystemModule;

namespace GameSvr
{
    /// <summary>
    /// 在指定地图随机移动
    /// </summary>
    [GameCommand("MapMove", "在指定地图随机移动", M2Share.g_sGameCommandMoveHelpMsg, 10)]
    public class MapMoveCommand : BaseCommond
    {
        [DefaultCommand]
        public void MapMove(string[] @Params, TPlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sMapName = @Params.Length > 0 ? @Params[0] : "";
            if (string.IsNullOrEmpty(sMapName))
            {
                PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var Envir = M2Share.g_MapManager.FindMap(sMapName);
            if (Envir == null)
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sTheMapNotFound, sMapName), MsgColor.Red, MsgType.Hint);
                return;
            }
            if (PlayObject.m_btPermission >= this.Command.nPermissionMin || M2Share.CanMoveMap(sMapName))
            {
                PlayObject.SendRefMsg(Grobal2.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
                PlayObject.MapRandomMove(sMapName, 0);
            }
            else
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sTheMapDisableMove, sMapName, Envir.sMapDesc), MsgColor.Red, MsgType.Hint);//不允许传送
            }
        }
    }
}