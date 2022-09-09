using GameSvr.Player;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 取指定地图怪物数量
    /// </summary>
    [GameCommand("MobCount", "取指定地图怪物数量", GameCommandConst.GameCommandMobCountHelpMsg, 10)]
    public class MobCountCommand : BaseCommond
    {
        [DefaultCommand]
        public void MobCount(string[] @Params, PlayObject PlayObject)
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
            var envirnoment = M2Share.MapMgr.FindMap(sMapName);
            if (envirnoment == null)
            {
                PlayObject.SysMsg(GameCommandConst.GameCommandMobCountMapNotFound, MsgColor.Red, MsgType.Hint);
                return;
            }
            PlayObject.SysMsg(string.Format(GameCommandConst.GameCommandMobCountMonsterCount, M2Share.UserEngine.GetMapMonster(envirnoment, null)), MsgColor.Green, MsgType.Hint);
        }
    }
}