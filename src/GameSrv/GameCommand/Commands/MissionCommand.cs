using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
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
            GameShare.BoMission = true;
            GameShare.MissionMap = playObject.MapName;
            GameShare.MissionX = nX;
            GameShare.MissionY = nY;
            playObject.SysMsg("怪物集中目标已设定为: " + playObject.MapName + '(' + GameShare.MissionX + ':' + GameShare.MissionY + ')', MsgColor.Green, MsgType.Hint);
        }
    }
}