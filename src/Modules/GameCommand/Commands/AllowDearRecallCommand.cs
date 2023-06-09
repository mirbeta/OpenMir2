using SystemModule;
using SystemModule.Enums;

namespace CommandSystem {
    /// <summary>
    /// 此命令允许或禁止夫妻传送
    /// </summary>
    [Command("AllowDearRecall", "", 10)]
    public class AllowDearRecallCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor) {
            PlayerActor.CanDearRecall = !PlayerActor.CanDearRecall;
            if (PlayerActor.CanDearRecall) {
                PlayerActor.SysMsg(CommandHelp.EnableDearRecall, MsgColor.Blue, MsgType.Hint);
            }
            else {
                PlayerActor.SysMsg(CommandHelp.DisableDearRecall, MsgColor.Blue, MsgType.Hint);
            }
        }
    }
}