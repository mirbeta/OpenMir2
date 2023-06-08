using M2Server.Player;
using SystemModule;
using SystemModule.Enums;

namespace M2Server.GameCommand.Commands {
    /// <summary>
    /// 设置怪物集中目标
    /// </summary>
    [Command("Mission", "设置怪物集中目标", " X Y", 10)]
    public class MissionCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null) {
                return;
            }
            var sX = @params.Length > 0 ? @params[0] : "";
            var sY = @params.Length > 1 ? @params[1] : "";
            if (string.IsNullOrEmpty(sX) || string.IsNullOrEmpty(sY)) {
                playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var nX = HUtil32.StrToInt16(sX, 0);
            var nY = HUtil32.StrToInt16(sY, 0);
            M2Share.BoMission = true;
            M2Share.MissionMap = playObject.MapName;
            M2Share.MissionX = nX;
            M2Share.MissionY = nY;
            playObject.SysMsg("怪物集中目标已设定为: " + playObject.MapName + '(' + M2Share.MissionX + ':' + M2Share.MissionY + ')', MsgColor.Green, MsgType.Hint);
        }
    }
}