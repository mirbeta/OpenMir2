using SystemModule;
using SystemModule.Enums;

namespace CommandSystem {
    [Command("Letshout", "", "")]
    public class LetShoutCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(IPlayerActor PlayerActor) {
            PlayerActor.SysMsgBanShout = !PlayerActor.SysMsgBanShout;
            if (PlayerActor.SysMsgBanShout) {
                PlayerActor.SysMsg(CommandHelp.EnableShoutMsg, MsgColor.Green, MsgType.Hint);
            }
            else {
                PlayerActor.SysMsg(CommandHelp.DisableShoutMsg, MsgColor.Green, MsgType.Hint);
            }
        }
    }
}
