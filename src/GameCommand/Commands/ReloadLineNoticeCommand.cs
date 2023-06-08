using M2Server.Player;
using SystemModule.Enums;

namespace M2Server.GameCommand.Commands {
    /// <summary>
    /// 重新加载游戏公告
    /// </summary>
    [Command("ReloadLineNotice", "重新加载游戏公告", 10)]
    public class ReloadLineNoticeCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject playObject) {
            if (M2Share.LoadLineNotice(M2Share.GetNoticeFilePath("LineNotice.txt"))) {
                playObject.SysMsg(CommandHelp.GameCommandReloadLineNoticeSuccessMsg, MsgColor.Green, MsgType.Hint);
            }
            else {
                playObject.SysMsg(CommandHelp.GameCommandReloadLineNoticeFailMsg, MsgColor.Red, MsgType.Hint);
            }
        }
    }
}