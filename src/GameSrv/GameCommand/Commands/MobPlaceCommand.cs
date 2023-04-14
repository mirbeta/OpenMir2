using GameSrv.Actor;
using GameSrv.Maps;
using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 设定怪物集中点
    /// </summary>
    [Command("MobPlace", "设定怪物集中点", "X  Y 怪物名称 怪物数量", 10)]
    public class MobPlaceCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null) {
                return;
            }
            var sX = @params.Length > 0 ? @params[0] : "";
            var sY = @params.Length > 1 ? @params[1] : "";
            var sMonName = @params.Length > 2 ? @params[2] : "";
            var sCount = @params.Length > 3 ? @params[3] : "";
            var nCount = HUtil32._MIN(500, HUtil32.StrToInt(sCount, 0));
            var nX = HUtil32.StrToInt16(sX, 0);
            var nY = HUtil32.StrToInt16(sY, 0);
            BaseObject mon = null;
            nCount = HUtil32._MIN(500, HUtil32.StrToInt(sCount, 0));
            nX = HUtil32.StrToInt16(sX, 0);
            nY = HUtil32.StrToInt16(sY, 0);
            if (nX <= 0 || nY <= 0 || string.IsNullOrEmpty(sMonName) || nCount <= 0) {
                playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var mEnvir = M2Share.MapMgr.FindMap(M2Share.MissionMap);
            if (!M2Share.BoMission || mEnvir == null) {
                playObject.SysMsg("还没有设定怪物集中点!!!", MsgColor.Red, MsgType.Hint);
                playObject.SysMsg("请先用命令" + this.Command.Name + "设置怪物的集中点。", MsgColor.Red, MsgType.Hint);
                return;
            }
            for (var i = 0; i < nCount; i++) {
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
                playObject.SysMsg(nCount + " 只 " + sMonName + " 已正在往地图 " + M2Share.MissionMap + " " + M2Share.MissionX + ":" + M2Share.MissionY + " 集中。", MsgColor.Green, MsgType.Hint);
            }
        }
    }
}