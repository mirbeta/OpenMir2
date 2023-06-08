using M2Server.Maps;
using M2Server.Player;
using SystemModule.Enums;

namespace M2Server.GameCommand.Commands
{
    /// <summary>
    /// 取指定地图玩家数量
    /// </summary>
    [Command("HumanCount", "取指定地图玩家数量", CommandHelp.GameCommandHumanCountHelpMsg, 10)]
    public class HumanCountCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject)
        {
            if (@params == null)
            {
                return;
            }
            var sMapName = @params.Length > 0 ? @params[0] : "";
            if (string.IsNullOrEmpty(sMapName))
            {
                playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var envir = M2Share.MapMgr.FindMap(sMapName);
            if (envir == null)
            {
                playObject.SysMsg(CommandHelp.GameCommandMobCountMapNotFound, MsgColor.Red, MsgType.Hint);
                return;
            }
            playObject.SysMsg(string.Format(CommandHelp.GameCommandMobCountMonsterCount, M2Share.WorldEngine.GetMapHuman(sMapName)), MsgColor.Green, MsgType.Hint);
        }
    }
}