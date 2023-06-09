using M2Server.Maps;
using SystemModule;
using SystemModule.Enums;

namespace CommandSystem {
    /// <summary>
    /// 取指定地图怪物数量
    /// </summary>
    [Command("MobCount", "取指定地图怪物数量", CommandHelp.GameCommandMobCountHelpMsg, 10)]
    public class MobCountCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor) {
            if (@params == null) {
                return;
            }
            var sMapName = @params.Length > 0 ? @params[0] : "";
            if (string.IsNullOrEmpty(sMapName)) {
                PlayerActor.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var envirnoment = ModuleShare.MapMgr.FindMap(sMapName);
            if (envirnoment == null) {
                PlayerActor.SysMsg(CommandHelp.GameCommandMobCountMapNotFound, MsgColor.Red, MsgType.Hint);
                return;
            }
            PlayerActor.SysMsg(string.Format(CommandHelp.GameCommandMobCountMonsterCount, ModuleShare.WorldEngine.GetMapMonster(envirnoment, null)), MsgColor.Green, MsgType.Hint);
        }
    }
}