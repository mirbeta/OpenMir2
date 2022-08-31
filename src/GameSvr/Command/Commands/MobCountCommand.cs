using GameSvr.Player;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 取指定地图怪物数量
    /// </summary>
    [GameCommand("MobCount", "取指定地图怪物数量", GameCommandConst.g_sGameCommandMobCountHelpMsg, 10)]
    public class MobCountCommand : BaseCommond
    {
        [DefaultCommand]
        public void MobCount(string[] @Params, TPlayObject PlayObject)
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
            var envirnoment = M2Share.MapManager.FindMap(sMapName);
            if (envirnoment == null)
            {
                PlayObject.SysMsg(GameCommandConst.g_sGameCommandMobCountMapNotFound, MsgColor.Red, MsgType.Hint);
                return;
            }
            PlayObject.SysMsg(string.Format(GameCommandConst.g_sGameCommandMobCountMonsterCount, M2Share.UserEngine.GetMapMonster(envirnoment, null)), MsgColor.Green, MsgType.Hint);
        }
    }
}