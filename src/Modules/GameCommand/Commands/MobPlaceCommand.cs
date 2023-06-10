using SystemModule;
using SystemModule.Enums;

namespace CommandModule.Commands
{
    /// <summary>
    /// 设定怪物集中点
    /// </summary>
    [Command("MobPlace", "设定怪物集中点", "X  Y 怪物名称 怪物数量", 10)]
    public class MobPlaceCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (@params == null)
            {
                return;
            }
            var sX = @params.Length > 0 ? @params[0] : "";
            var sY = @params.Length > 1 ? @params[1] : "";
            var sMonName = @params.Length > 2 ? @params[2] : "";
            var sCount = @params.Length > 3 ? @params[3] : "";
            var nCount = HUtil32._MIN(500, HUtil32.StrToInt(sCount, 0));
            var nX = HUtil32.StrToInt16(sX, 0);
            var nY = HUtil32.StrToInt16(sY, 0);
            IActor mon = null;
            nCount = HUtil32._MIN(500, HUtil32.StrToInt(sCount, 0));
            nX = HUtil32.StrToInt16(sX, 0);
            nY = HUtil32.StrToInt16(sY, 0);
            if (nX <= 0 || nY <= 0 || string.IsNullOrEmpty(sMonName) || nCount <= 0)
            {
                PlayerActor.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var mEnvir = SystemShare.MapMgr.FindMap(SystemShare.MissionMap);
            if (!SystemShare.BoMission || mEnvir == null)
            {
                PlayerActor.SysMsg("还没有设定怪物集中点!!!", MsgColor.Red, MsgType.Hint);
                PlayerActor.SysMsg("请先用命令" + this.Command.Name + "设置怪物的集中点。", MsgColor.Red, MsgType.Hint);
                return;
            }
            for (var i = 0; i < nCount; i++)
            {
                mon = SystemShare.WorldEngine.RegenMonsterByName(SystemShare.MissionMap, nX, nY, sMonName);
                if (mon != null)
                {
                    mon.Mission = true;
                    mon.MissionX = SystemShare.MissionX;
                    mon.MissionY = SystemShare.MissionY;
                }
                else
                {
                    break;
                }
            }
            if (mon?.Race != 136)
            {
                PlayerActor.SysMsg(nCount + " 只 " + sMonName + " 已正在往地图 " + SystemShare.MissionMap + " " + SystemShare.MissionX + ":" + SystemShare.MissionY + " 集中。", MsgColor.Green, MsgType.Hint);
            }
        }
    }
}