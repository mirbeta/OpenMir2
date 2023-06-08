using M2Server.Player;
using SystemModule.Enums;

namespace M2Server.GameCommand.Commands {
    /// <summary>
    /// 此命令允许或禁止夫妻传送
    /// </summary>
    [Command("AllowDearRecall", "", 10)]
    public class AllowDearRecallCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            playObject.CanDearRecall = !playObject.CanDearRecall;
            if (playObject.CanDearRecall) {
                playObject.SysMsg(CommandHelp.EnableDearRecall, MsgColor.Blue, MsgType.Hint);
            }
            else {
                playObject.SysMsg(CommandHelp.DisableDearRecall, MsgColor.Blue, MsgType.Hint);
            }
        }
    }
}