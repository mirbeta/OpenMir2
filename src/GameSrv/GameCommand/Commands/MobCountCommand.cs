using GameSrv.Maps;
using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 取指定地图怪物数量
    /// </summary>
    [Command("MobCount", "取指定地图怪物数量", CommandHelp.GameCommandMobCountHelpMsg, 10)]
    public class MobCountCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null) {
                return;
            }
            var sMapName = @params.Length > 0 ? @params[0] : "";
            if (string.IsNullOrEmpty(sMapName)) {
                playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var envirnoment = M2Share.MapMgr.FindMap(sMapName);
            if (envirnoment == null) {
                playObject.SysMsg(CommandHelp.GameCommandMobCountMapNotFound, MsgColor.Red, MsgType.Hint);
                return;
            }
            playObject.SysMsg(string.Format(CommandHelp.GameCommandMobCountMonsterCount, M2Share.WorldEngine.GetMapMonster(envirnoment, null)), MsgColor.Green, MsgType.Hint);
        }
    }
}