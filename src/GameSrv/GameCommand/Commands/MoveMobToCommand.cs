using GameSrv.Actor;
using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 将指定坐标的怪物移动到新坐标，名称为ALL则移动该坐标所有怪物
    /// </summary>
    [Command("MoveMobTo", "将指定坐标的怪物移动到新坐标", "怪物名称 原地图 原X 原Y 新地图 新X 新Y", 10)]
    public class MoveMobToCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @Params, PlayObject PlayObject) {
            if (@Params == null) {
                return;
            }
            string sMonName = @Params[0];
            string OleMap = @Params[1];
            string NewMap = @Params[2];
            short nX = (short)(@Params[3] == null ? 0 : HUtil32.StrToInt(@Params[3],0));
            short nY = (short)(@Params[4] == null ? 0 : HUtil32.StrToInt(@Params[4],0));
            short OnX = (short)(@Params[5] == null ? 0 : HUtil32.StrToInt(@Params[5],0));
            short OnY = (short)(@Params[6] == null ? 0 : HUtil32.StrToInt(@Params[6],0));
            BaseObject MoveMon;
            if (string.IsNullOrEmpty(sMonName) || string.IsNullOrEmpty(OleMap) || string.IsNullOrEmpty(NewMap) || !string.IsNullOrEmpty(sMonName) && sMonName[0] == '?') {
                PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            bool boMoveAll = false;
            if (sMonName == "ALL") {
                boMoveAll = true;
            }
            if (nX < 0) {
                nX = 0;
            }
            if (nY < 0) {
                nY = 0;
            }
            if (OnX < 0) {
                OnX = 0;
            }
            if (OnY < 0) {
                OnY = 0;
            }
            Maps.Envirnoment SrcEnvir = M2Share.MapMgr.FindMap(OleMap);// 原地图
            Maps.Envirnoment DenEnvir = M2Share.MapMgr.FindMap(NewMap);// 新地图
            if (SrcEnvir == null || DenEnvir == null) {
                return;
            }
            IList<BaseObject> MonList = new List<BaseObject>();
            if (!boMoveAll)// 指定名称的怪移动
            {
                M2Share.WorldEngine.GetMapRangeMonster(SrcEnvir, OnX, OnY, 10, MonList);// 查指定XY范围内的怪
                if (MonList.Count > 0) {
                    for (int i = 0; i < MonList.Count; i++) {
                        MoveMon = MonList[i];
                        if (MoveMon != PlayObject) {
                            if (string.Compare(MoveMon.ChrName, sMonName, StringComparison.OrdinalIgnoreCase) == 0) // 是否是指定名称的怪
                            {
                                MoveMon.SpaceMove(NewMap, nX, nY, 0);
                            }
                        }
                    }
                }
            }
            else {
                // 所有怪移动
                M2Share.WorldEngine.GetMapRangeMonster(SrcEnvir, OnX, OnY, 1000, MonList);// 查指定XY范围内的怪
                for (int i = 0; i < MonList.Count; i++) {
                    MoveMon = MonList[i];
                    if (MoveMon != PlayObject) {
                        MoveMon.SpaceMove(NewMap, nX, nY, 0);
                    }
                }
            }
        }
    }
}