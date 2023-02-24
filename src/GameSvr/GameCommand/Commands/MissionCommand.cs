using GameSvr.Player;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands {
    /// <summary>
    /// 设置怪物集中目标
    /// </summary>
    [Command("Mission", "设置怪物集中目标", " X Y", 10)]
    public class MissionCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @Params, PlayObject PlayObject) {
            if (@Params == null) {
                return;
            }
            string sX = @Params.Length > 0 ? @Params[0] : "";
            string sY = @Params.Length > 1 ? @Params[1] : "";
            if (sX == "" || sY == "") {
                PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            short nX = HUtil32.StrToInt16(sX, 0);
            short nY = HUtil32.StrToInt16(sY, 0);
            M2Share.BoMission = true;
            M2Share.MissionMap = PlayObject.MapName;
            M2Share.MissionX = nX;
            M2Share.MissionY = nY;
            PlayObject.SysMsg("怪物集中目标已设定为: " + PlayObject.MapName + '(' + M2Share.MissionX + ':' + M2Share.MissionY + ')', MsgColor.Green, MsgType.Hint);
        }
    }
}