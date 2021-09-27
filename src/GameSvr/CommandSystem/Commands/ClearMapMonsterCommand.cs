using SystemModule;
using System.Collections.Generic;
using GameSvr.CommandSystem;
using System.Collections;

namespace GameSvr
{
    /// <summary>
    /// 清楚指定地图怪物
    /// </summary>
    [GameCommand("ClearMapMonster", "清楚指定地图怪物", 10)]
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
            IList<TBaseObject> MonList;
            TEnvirnoment Envir;
            int nMonCount;
            bool boKillAll;
            bool boKillAllMap;
            bool boNotItem;
            TBaseObject BaseObject;
            if (sMapName == "" || sMonName == "" || sItems == "")
            {
                PlayObject.SysMsg("命令格式: @" + this.Attributes.Name + " 地图号(* 为所有) 怪物名称(* 为所有) 掉物品(0,1)", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            boKillAll = false;
            boKillAllMap = false;
            boNotItem = true;
            nMonCount = 0;
            Envir = null;
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
            MonList = new List<TBaseObject>();
            try
            {
                for (var i = 0; i < M2Share.g_MapManager.Maps.Count; i++)
                {
                    Envir = M2Share.g_MapManager.Maps[i];
                    if (Envir != null)
                    {
                        if (boKillAllMap || Envir.sMapName.ToLower().CompareTo(sMapName.ToLower()) == 0)
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
                                        if (boKillAll || sMonName.ToLower().CompareTo(BaseObject.m_sCharName.ToLower()) == 0)
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
                PlayObject.SysMsg("输入的地图不存在!!!", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            PlayObject.SysMsg("已清除怪物数: " + nMonCount, TMsgColor.c_Red, TMsgType.t_Hint);
        }
    }
}