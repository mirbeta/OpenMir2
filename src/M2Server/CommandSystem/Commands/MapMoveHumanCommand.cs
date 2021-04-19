﻿using SystemModule;
using System;
using System.Collections.Generic;
using M2Server.CommandSystem;
using System.Collections;

namespace M2Server
{
    /// <summary>
    /// 将指定地图所有玩家随机移动
    /// </summary>
    [GameCommand("MapMoveHuman", "将指定地图所有玩家随机移动", 10)]
    public class MapMoveHumanCommand : BaseCommond
    {
        [DefaultCommand]
        public void MapMoveHuman(string[] @Params, TPlayObject PlayObject)
        {
            var sSrcMap = @Params.Length > 0 ? @Params[0] : "";
            var sDenMap = @Params.Length > 1 ? @Params[1] : "";
            ArrayList HumanList;
            TPlayObject MoveHuman;
            if ((sDenMap == "") || (sSrcMap == "") || ((sSrcMap != "") && (sSrcMap[0] == '?')))
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandParamUnKnow, this.Attributes.Name, M2Share.g_sGameCommandMapMoveHelpMsg), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            var SrcEnvir = M2Share.g_MapManager.FindMap(sSrcMap);
            var DenEnvir = M2Share.g_MapManager.FindMap(sDenMap);
            if (SrcEnvir == null)
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandMapMoveMapNotFound, sSrcMap), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (DenEnvir == null)
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandMapMoveMapNotFound, sDenMap), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            HumanList = new ArrayList();
            M2Share.UserEngine.GetMapRageHuman(SrcEnvir, SrcEnvir.wWidth / 2, SrcEnvir.wHeight / 2, 1000, HumanList);
            for (var i = 0; i < HumanList.Count; i++)
            {
                MoveHuman = HumanList[i] as TPlayObject;
                if (MoveHuman != PlayObject)
                {
                    MoveHuman.MapRandomMove(sDenMap, 0);
                }
            }
            HumanList = null;
        }
    }
}