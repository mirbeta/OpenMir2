using SystemModule;
using SystemModule;
using SystemModule.Enums;

namespace CommandSystem {
    /// <summary>
    /// 设置怪物集中目标
    /// </summary>
    [Command("Mission", "设置怪物集中目标", " X Y", 10)]
    public class MissionCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor) {
            if (@params == null) {
                return;
            }
            var sX = @params.Length > 0 ? @params[0] : "";
            var sY = @params.Length > 1 ? @params[1] : "";
            if (string.IsNullOrEmpty(sX) || string.IsNullOrEmpty(sY)) {
                PlayerActor.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var nX = HUtil32.StrToInt16(sX, 0);
            var nY = HUtil32.StrToInt16(sY, 0);
            ModuleShare.BoMission = true;
            ModuleShare.MissionMap = PlayerActor.MapName;
            ModuleShare.MissionX = nX;
            ModuleShare.MissionY = nY;
            PlayerActor.SysMsg("怪物集中目标已设定为: " + PlayerActor.MapName + '(' + ModuleShare.MissionX + ':' + ModuleShare.MissionY + ')', MsgColor.Green, MsgType.Hint);
        }
    }
}