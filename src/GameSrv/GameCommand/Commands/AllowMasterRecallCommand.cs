using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 此命令用于允许或禁止师徒传送
    /// </summary>
    [Command("AllowMasterRecall", "此命令用于允许或禁止师徒传送")]
    public class AllowMasterRecallCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject playObject) {
            playObject.CanMasterRecall = !playObject.CanMasterRecall;
            if (playObject.CanMasterRecall) {
                playObject.SysMsg(CommandHelp.EnableMasterRecall, MsgColor.Blue, MsgType.Hint);
            }
            else {
                playObject.SysMsg(CommandHelp.DisableMasterRecall, MsgColor.Blue, MsgType.Hint);
            }
        }
    }
}