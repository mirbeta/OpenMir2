using SystemModule;
using SystemModule.Actors;
using SystemModule.Enums;

namespace CommandModule.Commands
{
    /// <summary>
    /// 取指定地图玩家数量
    /// </summary>
    [Command("HumanCount", "取指定地图玩家数量", CommandHelp.GameCommandHumanCountHelpMsg, 10)]
    public class HumanCountCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (@params == null)
            {
                return;
            }
            var sMapName = @params.Length > 0 ? @params[0] : "";
            if (string.IsNullOrEmpty(sMapName))
            {
                PlayerActor.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var envir = SystemShare.MapMgr.FindMap(sMapName);
            if (envir == null)
            {
                PlayerActor.SysMsg(CommandHelp.GameCommandMobCountMapNotFound, MsgColor.Red, MsgType.Hint);
                return;
            }
            PlayerActor.SysMsg(string.Format(CommandHelp.GameCommandMobCountMonsterCount, SystemShare.WorldEngine.GetMapHuman(sMapName)), MsgColor.Green, MsgType.Hint);
        }
    }
}