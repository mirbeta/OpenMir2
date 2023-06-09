using SystemModule;
using SystemModule.Enums;

namespace CommandSystem {
    /// <summary>
    /// 此命令用于允许或禁止编组传送功能
    /// </summary>
    [Command("AllowGroupReCall", "此命令用于允许或禁止编组传送功能")]
    public class AllowGroupReCallCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(IPlayerActor PlayerActor) {
            PlayerActor.AllowGroupReCall = !PlayerActor.AllowGroupReCall;
            if (PlayerActor.AllowGroupReCall) {
                PlayerActor.SysMsg(CommandHelp.EnableGroupRecall, MsgColor.Green, MsgType.Hint);
            }
            else {
                PlayerActor.SysMsg(CommandHelp.DisableGroupRecall, MsgColor.Green, MsgType.Hint);
            }
        }
    }
}