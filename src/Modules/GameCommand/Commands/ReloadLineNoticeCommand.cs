using SystemModule;
using SystemModule.Enums;

namespace CommandSystem {
    /// <summary>
    /// 重新加载游戏公告
    /// </summary>
    [Command("ReloadLineNotice", "重新加载游戏公告", 10)]
    public class ReloadLineNoticeCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(IPlayerActor PlayerActor) {
            if (SystemShare.LoadLineNotice(SystemShare.GetNoticeFilePath("LineNotice.txt"))) {
                PlayerActor.SysMsg(CommandHelp.GameCommandReloadLineNoticeSuccessMsg, MsgColor.Green, MsgType.Hint);
            }
            else {
                PlayerActor.SysMsg(CommandHelp.GameCommandReloadLineNoticeFailMsg, MsgColor.Red, MsgType.Hint);
            }
        }
    }
}