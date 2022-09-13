using GameSvr.Player;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 取指定地图玩家数量
    /// </summary>
    [GameCommand("HumanCount", "取指定地图玩家数量", GameCommandConst.GameCommandHumanCountHelpMsg, 10)]
    public class HumanCountCommand : BaseCommond
    {
        [DefaultCommand]
        public void HumanCount(string[] @Params, PlayObject PlayObject)
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
                PlayObject.SysMsg(GameCommandConst.GameCommandMobCountMapNotFound, MsgColor.Red, MsgType.Hint);
                return;
            }
            PlayObject.SysMsg(string.Format(GameCommandConst.GameCommandMobCountMonsterCount, M2Share.WorldEngine.GetMapHuman(sMapName)), MsgColor.Green, MsgType.Hint);
        }
    }
}