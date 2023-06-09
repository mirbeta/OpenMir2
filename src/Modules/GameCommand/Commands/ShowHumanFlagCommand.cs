using SystemModule;
using SystemModule;
using SystemModule.Enums;

namespace CommandSystem {
    /// <summary>
    /// 取用户任务状态
    /// </summary>
    [Command("ShowHumanFlag", "取用户任务状态", CommandHelp.GameCommandShowHumanFlagHelpMsg, 10)]
    public class ShowHumanFlagCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor) {
            if (@params == null) {
                return;
            }
            var sHumanName = @params.Length > 0 ? @params[0] : "";
            var sFlag = @params.Length > 1 ? @params[1] : "";
            if (string.IsNullOrEmpty(sHumanName) || !string.IsNullOrEmpty(sHumanName) && sHumanName[0] == '?') {
                PlayerActor.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var mIPlayerActor = SystemShare.WorldEngine.GetPlayObject(sHumanName);
            if (mIPlayerActor == null) {
                PlayerActor.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
                return;
            }
            var nFlag = HUtil32.StrToInt(sFlag, 0);
            if (mIPlayerActor.GetQuestFalgStatus(nFlag) == 1) {
                PlayerActor.SysMsg(string.Format(CommandHelp.GameCommandShowHumanFlagONMsg, mIPlayerActor.ChrName, nFlag), MsgColor.Green, MsgType.Hint);
            }
            else {
                PlayerActor.SysMsg(string.Format(CommandHelp.GameCommandShowHumanFlagOFFMsg, mIPlayerActor.ChrName, nFlag), MsgColor.Green, MsgType.Hint);
            }
        }
    }
}