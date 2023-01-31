using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 取指定地图怪物数量
    /// </summary>
    [Command("MobCount", "取指定地图怪物数量", CommandHelp.GameCommandMobCountHelpMsg, 10)]
    public class MobCountCommand : Command
    {
        [ExecuteCommand]
        public void MobCount(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            string sMapName = @Params.Length > 0 ? @Params[0] : "";
            if (string.IsNullOrEmpty(sMapName))
            {
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            Maps.Envirnoment envirnoment = M2Share.MapMgr.FindMap(sMapName);
            if (envirnoment == null)
            {
                PlayObject.SysMsg(CommandHelp.GameCommandMobCountMapNotFound, MsgColor.Red, MsgType.Hint);
                return;
            }
            PlayObject.SysMsg(string.Format(CommandHelp.GameCommandMobCountMonsterCount, M2Share.WorldEngine.GetMapMonster(envirnoment, null)), MsgColor.Green, MsgType.Hint);
        }
    }
}