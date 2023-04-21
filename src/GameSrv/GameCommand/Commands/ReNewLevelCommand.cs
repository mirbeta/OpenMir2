using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 调整指定玩家转生等级
    /// </summary>
    [Command("ReNewLevel", "调整指定玩家转生等级", "人物名称 点数(为空则查看)", 10)]
    public class ReNewLevelCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null) {
                return;
            }
            var sHumanName = @params.Length > 0 ? @params[0] : "";
            var sLevel = @params.Length > 1 ? @params[1] : "";
            if (string.IsNullOrEmpty(sHumanName) || !string.IsNullOrEmpty(sHumanName) && sHumanName[0] == '?') {
                playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var nLevel = HUtil32.StrToInt(sLevel, -1);
            var mPlayObject = GameShare.WorldEngine.GetPlayObject(sHumanName);
            if (mPlayObject != null) {
                if (nLevel >= 0 && nLevel <= 255) {
                    mPlayObject.ReLevel = (byte)nLevel;
                    mPlayObject.RefShowName();
                }
                playObject.SysMsg(sHumanName + " 的转生等级为 " + playObject.ReLevel, MsgColor.Green, MsgType.Hint);
            }
            else {
                playObject.SysMsg(sHumanName + " 没在线上!!!", MsgColor.Red, MsgType.Hint);
            }
        }
    }
}