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
            var sMapName = @Params.Length > 0 ? @Params[0] : "";
            if (sMapName == "" || sMapName != "" && sMapName[1] == '?')
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandParamUnKnow, this.Attributes.Name, M2Share.g_sGameCommandMobCountHelpMsg), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            var Envir = M2Share.g_MapManager.FindMap(sMapName);
            if (Envir == null)
            {
                PlayObject.SysMsg(M2Share.g_sGameCommandMobCountMapNotFound, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandMobCountMonsterCount, M2Share.UserEngine.GetMapMonster(Envir, null)), TMsgColor.c_Green, TMsgType.t_Hint);
        }
    }
}