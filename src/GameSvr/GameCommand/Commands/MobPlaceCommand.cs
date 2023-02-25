using GameSvr.Actor;
using GameSvr.Player;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands {
    /// <summary>
    /// 设定怪物集中点
    /// </summary>
    [Command("MobPlace", "设定怪物集中点", "X  Y 怪物名称 怪物数量", 10)]
    public class MobPlaceCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @Params, PlayObject PlayObject) {
            if (@Params == null) {
                return;
            }
            string sX = @Params.Length > 0 ? @Params[0] : "";
            string sY = @Params.Length > 1 ? @Params[1] : "";
            string sMonName = @Params.Length > 2 ? @Params[2] : "";
            string sCount = @Params.Length > 3 ? @Params[3] : "";
            int nCount = HUtil32._MIN(500, HUtil32.StrToInt(sCount, 0));
            short nX = HUtil32.StrToInt16(sX, 0);
            short nY = HUtil32.StrToInt16(sY, 0);
            BaseObject mon = null;
            nCount = HUtil32._MIN(500, HUtil32.StrToInt(sCount, 0));
            nX = HUtil32.StrToInt16(sX, 0);
            nY = HUtil32.StrToInt16(sY, 0);
            if (nX <= 0 || nY <= 0 || string.IsNullOrEmpty(sMonName) || nCount <= 0) {
                PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            Maps.Envirnoment MEnvir = M2Share.MapMgr.FindMap(M2Share.MissionMap);
            if (!M2Share.BoMission || MEnvir == null) {
                PlayObject.SysMsg("还没有设定怪物集中点!!!", MsgColor.Red, MsgType.Hint);
                PlayObject.SysMsg("请先用命令" + this.Command.Name + "设置怪物的集中点。", MsgColor.Red, MsgType.Hint);
                return;
            }
            for (int i = 0; i < nCount; i++) {
                mon = M2Share.WorldEngine.RegenMonsterByName(M2Share.MissionMap, nX, nY, sMonName);
                if (mon != null) {
                    mon.Mission = true;
                    mon.MissionX = M2Share.MissionX;
                    mon.MissionY = M2Share.MissionY;
                }
                else {
                    break;
                }
            }
            if (mon?.Race != 136) {
                PlayObject.SysMsg(nCount + " 只 " + sMonName + " 已正在往地图 " + M2Share.MissionMap + " " + M2Share.MissionX + ":" + M2Share.MissionY + " 集中。", MsgColor.Green, MsgType.Hint);
            }
        }
    }
}