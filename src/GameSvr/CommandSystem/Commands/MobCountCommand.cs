using SystemModule;
using System;
using GameSvr.CommandSystem;

namespace GameSvr
{
    /// <summary>
    /// 取指定地图怪物数量
    /// </summary>
    [GameCommand("MobCount", "取指定地图怪物数量", 10)]
    public class MobCountCommand : BaseCommond
    {
        [DefaultCommand]
        public void MobCount(string[] @Params, TPlayObject PlayObject)
        {
            if (@Params ==null)
            {
                return;
            }
            var sMapName = @Params.Length > 0 ? @Params[0] : "";
            if (sMapName == "" || sMapName != "" && sMapName[1] == '?')
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandParamUnKnow, this.Attributes.Name, M2Share.g_sGameCommandMobCountHelpMsg), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            var FindEnvir = M2Share.g_MapManager.FindMap(sMapName);
            if (FindEnvir == null)
            {
                PlayObject.SysMsg(M2Share.g_sGameCommandMobCountMapNotFound, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandMobCountMonsterCount, M2Share.UserEngine.GetMapMonster(FindEnvir, null)), TMsgColor.c_Green, TMsgType.t_Hint);
        }
    }
}