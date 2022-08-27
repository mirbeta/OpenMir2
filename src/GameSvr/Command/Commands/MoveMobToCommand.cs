using GameSvr.Actor;
using GameSvr.Maps;
using GameSvr.Player;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 将指定坐标的怪物移动到新坐标，名称为ALL则移动该坐标所有怪物
    /// </summary>
    [GameCommand("MoveMobTo", "将指定坐标的怪物移动到新坐标", "怪物名称 原地图 原X 原Y 新地图 新X 新Y", 10)]
    public class MoveMobToCommand : BaseCommond
    {
        [DefaultCommand]
        public void MoveMobTo(string[] @Params, TPlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sMonName = @Params[0];
            var OleMap = @Params[1];
            var NewMap = @Params[2];
            var nX = @Params[3] == null ? (short)0 : System.Convert.ToInt16(@Params[3]);
            var nY = @Params[4] == null ? (short)0 : System.Convert.ToInt16(@Params[4]);
            var OnX = @Params[5] == null ? (short)0 : System.Convert.ToInt16(@Params[5]);
            var OnY = @Params[6] == null ? (short)0 : System.Convert.ToInt16(@Params[6]);
            Envirnoment SrcEnvir;
            Envirnoment DenEnvir;
            IList<TBaseObject> MonList;
            TBaseObject MoveMon;
            if (sMonName == "" || OleMap == "" || NewMap == "" || sMonName != "" && sMonName[0] == '?')
            {
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var boMoveAll = false;
            if (sMonName == "ALL")
            {
                boMoveAll = true;
            }
            if (nX < 0)
            {
                nX = 0;
            }
            if (nY < 0)
            {
                nY = 0;
            }
            if (OnX < 0)
            {
                OnX = 0;
            }
            if (OnY < 0)
            {
                OnY = 0;
            }
            SrcEnvir = M2Share.MapManager.FindMap(OleMap);// 原地图
            DenEnvir = M2Share.MapManager.FindMap(NewMap);// 新地图
            if (SrcEnvir == null || DenEnvir == null)
            {
                return;
            }
            MonList = new List<TBaseObject>();
            if (!boMoveAll)// 指定名称的怪移动
            {
                M2Share.UserEngine.GetMapRangeMonster(SrcEnvir, OnX, OnY, 10, MonList);// 查指定XY范围内的怪
                if (MonList.Count > 0)
                {
                    for (var i = 0; i < MonList.Count; i++)
                    {
                        MoveMon = MonList[i];
                        if (MoveMon != PlayObject)
                        {
                            if (string.Compare(MoveMon.m_sCharName, sMonName, StringComparison.Ordinal) == 0) // 是否是指定名称的怪
                            {
                                MoveMon.SpaceMove(NewMap, nX, nY, 0);
                            }
                        }
                    }
                }
            }
            else
            {
                // 所有怪移动
                M2Share.UserEngine.GetMapRangeMonster(SrcEnvir, OnX, OnY, 1000, MonList);// 查指定XY范围内的怪
                for (var i = 0; i < MonList.Count; i++)
                {
                    MoveMon = MonList[i];
                    if (MoveMon != PlayObject)
                    {
                        MoveMon.SpaceMove(NewMap, nX, nY, 0);
                    }
                }
            }
            MonList = null;
        }
    }
}