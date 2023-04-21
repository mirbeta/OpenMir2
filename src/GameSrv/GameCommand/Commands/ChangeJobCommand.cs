using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 调整指定玩家职业
    /// </summary>
    [Command("ChangeJob", "调整指定玩家职业", CommandHelp.GameCommandChangeJobHelpMsg, 10)]
    public class ChangeJobCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null) {
                return;
            }
            var sHumanName = @params.Length > 0 ? @params[0] : "";
            var sJobName = @params.Length > 1 ? @params[1] : "";
            if (string.IsNullOrEmpty(sHumanName) || string.IsNullOrEmpty(sJobName)) {
                playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var mPlayObject = GameShare.WorldEngine.GetPlayObject(sHumanName);
            if (mPlayObject != null) {
                if (string.Compare(sJobName, "Warr", StringComparison.OrdinalIgnoreCase) == 0) {
                    mPlayObject.Job = PlayJob.Warrior;
                }
                if (string.Compare(sJobName, "Wizard", StringComparison.OrdinalIgnoreCase) == 0) {
                    mPlayObject.Job = PlayJob.Wizard;
                }
                if (string.Compare(sJobName, "Taos", StringComparison.OrdinalIgnoreCase) == 0) {
                    mPlayObject.Job = PlayJob.Taoist;
                }
                mPlayObject.HasLevelUp(1);
                mPlayObject.SysMsg(CommandHelp.GameCommandChangeJobHumanMsg, MsgColor.Green, MsgType.Hint);
                playObject.SysMsg(string.Format(CommandHelp.GameCommandChangeJobMsg, sHumanName), MsgColor.Green, MsgType.Hint);
            }
            else {
                playObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sHumanName), MsgColor.Red, MsgType.Hint);
            }
        }
    }
}