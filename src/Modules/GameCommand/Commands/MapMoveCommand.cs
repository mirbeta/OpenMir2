using OpenMir2;
using SystemModule;
using SystemModule.Actors;
using SystemModule.Enums;

namespace CommandModule.Commands
{
    /// <summary>
    /// 在指定地图随机移动
    /// </summary>
    [Command("MapMove", "在指定地图随机移动", CommandHelp.GameCommandMoveHelpMsg, 10)]
    public class MapMoveCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (@params == null)
            {
                return;
            }
            string sMapName = @params.Length > 0 ? @params[0] : "";
            if (string.IsNullOrEmpty(sMapName))
            {
                PlayerActor.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            SystemModule.Maps.IEnvirnoment envir = SystemShare.MapMgr.FindMap(sMapName);
            if (envir == null)
            {
                PlayerActor.SysMsg(string.Format(CommandHelp.TheMapNotFound, sMapName), MsgColor.Red, MsgType.Hint);
                return;
            }
            if (PlayerActor.Permission >= this.Command.PermissionMin || SystemShare.CanMoveMap(sMapName))
            {
                PlayerActor.SendRefMsg(Messages.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
                PlayerActor.MapRandomMove(sMapName, 0);
            }
            else
            {
                PlayerActor.SysMsg(string.Format(CommandHelp.TheMapDisableMove, sMapName, envir.MapDesc), MsgColor.Red, MsgType.Hint);//不允许传送
            }
        }
    }
}