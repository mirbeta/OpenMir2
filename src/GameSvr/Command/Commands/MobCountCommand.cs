using SystemModule;
using System;
using GameSvr.CommandSystem;

namespace GameSvr
{
    /// <summary>
    /// 取指定地图怪物数量
    /// </summary>
    [GameCommand("MobCount", "取指定地图怪物数量", M2Share.g_sGameCommandMobCountHelpMsg, 10)]
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
                PlayObject.SysMsg(CommandAttribute.CommandHelp(), MsgColor.Red, MsgType.Hint);
                return;
            }
            var FindEnvir = M2Share.g_MapManager.FindMap(sMapName);
            if (FindEnvir == null)
            {
                PlayObject.SysMsg(M2Share.g_sGameCommandMobCountMapNotFound, MsgColor.Red, MsgType.Hint);
                return;
            }
            PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandMobCountMonsterCount, M2Share.UserEngine.GetMapMonster(FindEnvir, null)), MsgColor.Green, MsgType.Hint);
        }
    }
}