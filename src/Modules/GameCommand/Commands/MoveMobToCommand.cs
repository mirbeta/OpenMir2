using SystemModule;
using SystemModule.Enums;

namespace CommandSystem.Commands
{
    /// <summary>
    /// 将指定坐标的怪物移动到新坐标，名称为ALL则移动该坐标所有怪物
    /// </summary>
    [Command("MoveMobTo", "将指定坐标的怪物移动到新坐标", "怪物名称 原地图 原X 原Y 新地图 新X 新Y", 10)]
    public class MoveMobToCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (@params == null)
            {
                return;
            }
            var sMonName = @params[0];
            var oleMap = @params[1];
            var newMap = @params[2];
            var nX = (short)(@params[3] == null ? 0 : HUtil32.StrToInt(@params[3], 0));
            var nY = (short)(@params[4] == null ? 0 : HUtil32.StrToInt(@params[4], 0));
            var x = (short)(@params[5] == null ? 0 : HUtil32.StrToInt(@params[5], 0));
            var y = (short)(@params[6] == null ? 0 : HUtil32.StrToInt(@params[6], 0));
            if (string.IsNullOrEmpty(sMonName) || string.IsNullOrEmpty(oleMap) || string.IsNullOrEmpty(newMap) || !string.IsNullOrEmpty(sMonName) && sMonName[0] == '?')
            {
                PlayerActor.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var boMoveAll = sMonName == "ALL";
            if (nX < 0)
            {
                nX = 0;
            }
            if (nY < 0)
            {
                nY = 0;
            }
            if (x < 0)
            {
                x = 0;
            }
            if (y < 0)
            {
                y = 0;
            }
            var srcEnvir = SystemShare.MapMgr.FindMap(oleMap);// 原地图
            var denEnvir = SystemShare.MapMgr.FindMap(newMap);// 新地图
            if (srcEnvir == null || denEnvir == null)
            {
                return;
            }
            IList<IActor> monList = new List<IActor>();
            //if (!boMoveAll)// 指定名称的怪移动
            //{
            //    M2Share.WorldEngine.GetMapRangeMonster(srcEnvir, x, y, 10, monList);// 查指定XY范围内的怪
            //    if (monList.Count > 0)
            //    {
            //        for (var i = 0; i < monList.Count; i++)
            //        {
            //            var moveMon = monList[i];
            //            if (moveMon != IPlayerActor)
            //            {
            //                if (string.Compare(moveMon.ChrName, sMonName, StringComparison.OrdinalIgnoreCase) == 0)// 是否是指定名称的怪
            //                {
            //                    moveMon.SpaceMove(newMap, nX, nY, 0);
            //                }
            //            }
            //        }
            //    }
            //}
            //else
            //{
            //    // 所有怪移动
            //    M2Share.WorldEngine.GetMapRangeMonster(srcEnvir, x, y, 1000, monList);// 查指定XY范围内的怪
            //    for (var i = 0; i < monList.Count; i++)
            //    {
            //        var moveMon = monList[i];
            //        if (moveMon != IPlayerActor)
            //        {
            //            moveMon.SpaceMove(newMap, nX, nY, 0);
            //        }
            //    }
            //}
        }
    }
}