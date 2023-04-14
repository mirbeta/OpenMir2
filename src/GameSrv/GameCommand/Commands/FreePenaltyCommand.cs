using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 清除指定玩家PK值
    /// </summary>
    [Command("FreePenalty", "清除指定玩家PK值", "人物名称", 10)]
    public class FreePenaltyCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null) {
                return;
            }
            var sHumanName = @params.Length > 0 ? @params[0] : "";
            if (!string.IsNullOrEmpty(sHumanName) && sHumanName[0] == '?') {
                playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var mPlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
            if (mPlayObject == null) {
                playObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                return;
            }
            mPlayObject.PkPoint = 0;
            mPlayObject.RefNameColor();
            mPlayObject.SysMsg(CommandHelp.GameCommandFreePKHumanMsg, MsgColor.Green, MsgType.Hint);
            playObject.SysMsg(string.Format(CommandHelp.GameCommandFreePKMsg, sHumanName), MsgColor.Green, MsgType.Hint);
        }
    }
}