using GameSvr.Actor;
using GameSvr.Maps;
using GameSvr.Player;
using SystemModule;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 清楚指定地图怪物
    /// </summary>
    [Command("ClearMapMonster", "清楚指定地图怪物", "地图号(* 为所有) 怪物名称(* 为所有) 掉物品(0,1)", 10)]
    public class ClearMapMonsterCommand : Command
    {
        [ExecuteCommand]
        public void ClearMapMonster(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sMapName = @Params.Length > 0 ? @Params[0] : "";
            var sMonName = @Params.Length > 1 ? @Params[1] : "";
            var sItems = @Params.Length > 2 ? @Params[2] : "";
            if (sMapName == "" || sMonName == "" || sItems == "")
            {
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
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
            IList<BaseObject> MonList = new List<BaseObject>();
            try
            {
                for (var i = 0; i < M2Share.MapMgr.Maps.Count; i++)
                {
                    Envir = M2Share.MapMgr.Maps[i];
                    if (Envir != null)
                    {
                        if (boKillAllMap || string.Compare(Envir.MapName, sMapName, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            var monsterCount = M2Share.WorldEngine.GetMapMonster(Envir, MonList);
                            if (monsterCount > 0)
                            {
                                for (var j = 0; j < monsterCount; j++)
                                {
                                    var BaseObject = MonList[j];
                                    if (BaseObject != null)
                                    {
                                        if (BaseObject.Master != null && BaseObject.Race != 135)// 除135怪外，其它宝宝不清除
                                        {
                                            if (BaseObject.Master.Race == ActorRace.Play)
                                            {
                                                continue;
                                            }
                                        }
                                        if (boKillAll || string.Compare(sMonName, BaseObject.ChrName, StringComparison.OrdinalIgnoreCase) == 0)
                                        {
                                            BaseObject.NoItem = boNotItem;
                                            BaseObject.WAbil.HP = 0;
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