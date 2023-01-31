using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 在指定地图随机移动
    /// </summary>
    [Command("MapMove", "在指定地图随机移动", CommandHelp.GameCommandMoveHelpMsg, 10)]
    public class MapMoveCommand : Command
    {
        [ExecuteCommand]
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
            var Envir = M2Share.MapMgr.FindMap(sMapName);
            if (Envir == null)
            {
                PlayObject.SysMsg(string.Format(CommandHelp.TheMapNotFound, sMapName), MsgColor.Red, MsgType.Hint);
                return;
            }
            if (PlayObject.Permission >= this.GameCommand.nPermissionMin || M2Share.CanMoveMap(sMapName))
            {
                PlayObject.SendRefMsg(Grobal2.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
                PlayObject.MapRandomMove(sMapName, 0);
            }
            else
            {
                PlayObject.SysMsg(string.Format(CommandHelp.TheMapDisableMove, sMapName, Envir.MapDesc), MsgColor.Red, MsgType.Hint);//不允许传送
            }
        }
    }
}