using SystemModule;
using System;
using System.Collections.Generic;
using GameSvr.CommandSystem;
using System.Collections;

namespace GameSvr
{
    /// <summary>
    /// 将指定地图所有玩家随机移动
    /// </summary>
    [GameCommand("MapMoveHuman", "将指定地图所有玩家随机移动", M2Share.g_sGameCommandMapMoveHelpMsg, 10)]
    public class MapMoveHumanCommand : BaseCommond
    {
        [DefaultCommand]
        public void MapMoveHuman(string[] @Params, TPlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sSrcMap = @Params.Length > 0 ? @Params[0] : "";
            var sDenMap = @Params.Length > 1 ? @Params[1] : "";
            IList<TBaseObject> HumanList;
            TPlayObject MoveHuman;
            if (sDenMap == "" || sSrcMap == "" || sSrcMap != "" && sSrcMap[0] == '?')
            {
                PlayObject.SysMsg(CommandAttribute.CommandHelp(), MsgColor.Red, MsgType.Hint);
                return;
            }
            var SrcEnvir = M2Share.g_MapManager.FindMap(sSrcMap);
            var DenEnvir = M2Share.g_MapManager.FindMap(sDenMap);
            if (SrcEnvir == null)
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandMapMoveMapNotFound, sSrcMap), MsgColor.Red, MsgType.Hint);
                return;
            }
            if (DenEnvir == null)
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandMapMoveMapNotFound, sDenMap), MsgColor.Red, MsgType.Hint);
                return;
            }
            HumanList = new List<TBaseObject>();
            M2Share.UserEngine.GetMapRageHuman(SrcEnvir, SrcEnvir.wWidth / 2, SrcEnvir.wHeight / 2, 1000, HumanList);
            for (var i = 0; i < HumanList.Count; i++)
            {
                MoveHuman = (TPlayObject)HumanList[i];
                if (MoveHuman != PlayObject)
                {
                    MoveHuman.MapRandomMove(sDenMap, 0);
                }
            }
            HumanList = null;
        }
    }
}