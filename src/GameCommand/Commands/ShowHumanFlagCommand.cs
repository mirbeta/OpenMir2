using M2Server.Player;
using SystemModule;
using SystemModule.Enums;

namespace M2Server.GameCommand.Commands {
    /// <summary>
    /// 取用户任务状态
    /// </summary>
    [Command("ShowHumanFlag", "取用户任务状态", CommandHelp.GameCommandShowHumanFlagHelpMsg, 10)]
    public class ShowHumanFlagCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null) {
                return;
            }
            var sHumanName = @params.Length > 0 ? @params[0] : "";
            var sFlag = @params.Length > 1 ? @params[1] : "";
            if (string.IsNullOrEmpty(sHumanName) || !string.IsNullOrEmpty(sHumanName) && sHumanName[0] == '?') {
                playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var mPlayObject = M2Share.WorldEngine.GetPlayObject(sHumanName);
            if (mPlayObject == null) {
                playObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                return;
            }
            var nFlag = HUtil32.StrToInt(sFlag, 0);
            if (mPlayObject.GetQuestFalgStatus(nFlag) == 1) {
                playObject.SysMsg(string.Format(CommandHelp.GameCommandShowHumanFlagONMsg, mPlayObject.ChrName, nFlag), MsgColor.Green, MsgType.Hint);
            }
            else {
                playObject.SysMsg(string.Format(CommandHelp.GameCommandShowHumanFlagOFFMsg, mPlayObject.ChrName, nFlag), MsgColor.Green, MsgType.Hint);
            }
        }
    }
}