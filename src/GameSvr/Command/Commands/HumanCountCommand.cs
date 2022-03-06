using SystemModule;
using System;
using GameSvr.CommandSystem;

namespace GameSvr
{
    /// <summary>
    /// 取指定地图玩家数量
    /// </summary>
    [GameCommand("HumanCount", "取指定地图玩家数量", M2Share.g_sGameCommandHumanCountHelpMsg, 10)]
    public class HumanCountCommand : BaseCommond
    {
        [DefaultCommand]
        public void HumanCount(string[] @Params, TPlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sMapName = @Params.Length > 0 ? @Params[0] : "";
            if (string.IsNullOrEmpty(sMapName))
            {
                PlayObject.SysMsg(CommandAttribute.CommandHelp(), MsgColor.Red, MsgType.Hint);
                return;
            }
            var Envir = M2Share.g_MapManager.FindMap(sMapName);
            if (Envir == null)
            {
                PlayObject.SysMsg(M2Share.g_sGameCommandMobCountMapNotFound, MsgColor.Red, MsgType.Hint);
                return;
            }
            PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandMobCountMonsterCount, M2Share.UserEngine.GetMapHuman(sMapName)), MsgColor.Green, MsgType.Hint);
        }
    }
}