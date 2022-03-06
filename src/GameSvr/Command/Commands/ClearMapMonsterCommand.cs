using System;
using SystemModule;
using System.Collections.Generic;
using GameSvr.CommandSystem;

namespace GameSvr
{
    /// <summary>
    /// 清楚指定地图怪物
    /// </summary>
    [GameCommand("ClearMapMonster", "清楚指定地图怪物", "地图号(* 为所有) 怪物名称(* 为所有) 掉物品(0,1)", 10)]
    public class ClearMapMonsterCommand : BaseCommond
    {
        [DefaultCommand]
        public void ClearMapMonster(string[] @Params, TPlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sMapName = @Params.Length > 0 ? @Params[0] : "";
            var sMonName = @Params.Length > 1 ? @Params[1] : "";
            var sItems = @Params.Length > 2 ? @Params[2] : "";
            TBaseObject BaseObject;
            if (sMapName == "" || sMonName == "" || sItems == "")
            {
                PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var boKillAll = false;
            var boKillAllMap = false;
            var boNotItem = true;
            var nMonCount = 0;
            Envirnoment Envir = null;
            if (sMonName == "*")
            {
                boKillAll = true;
            }
            if (sMapName == "*")
            {
                boKillAllMap = true;
            }
            if (sItems == "1")
            {
                boNotItem = false;
            }
            IList<TBaseObject> MonList = new List<TBaseObject>();
            try
            {
                for (var i = 0; i < M2Share.g_MapManager.Maps.Count; i++)
                {
                    Envir = M2Share.g_MapManager.Maps[i];
                    if (Envir != null)
                    {
                        if (boKillAllMap || string.Compare(Envir.sMapName, sMapName, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            M2Share.UserEngine.GetMapMonster(Envir, MonList);
                            if (MonList.Count > 0)
                            {
                                for (var j = 0; j < MonList.Count; j++)
                                {
                                    BaseObject = MonList[j] as TBaseObject;
                                    if (BaseObject != null)
                                    {
                                        if (BaseObject.m_Master != null && BaseObject.m_btRaceServer != 135)// 除135怪外，其它宝宝不清除
                                        {
                                            if (BaseObject.m_Master.m_btRaceServer == Grobal2.RC_PLAYOBJECT)
                                            {
                                                continue;
                                            }
                                        }
                                        if (boKillAll || string.Compare(sMonName, BaseObject.m_sCharName, StringComparison.OrdinalIgnoreCase) == 0)
                                        {
                                            BaseObject.m_boNoItem = boNotItem;
                                            BaseObject.m_WAbil.HP = 0;
                                            nMonCount++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            finally
            {
                MonList = null;
            }
            if (Envir == null)
            {
                PlayObject.SysMsg("输入的地图不存在!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            PlayObject.SysMsg("已清除怪物数: " + nMonCount, MsgColor.Red, MsgType.Hint);
        }
    }
}