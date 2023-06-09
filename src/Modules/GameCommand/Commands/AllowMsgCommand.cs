using SystemModule;
using SystemModule.Enums;

namespace CommandSystem {
    [Command("Allowmsg", "", "")]
    public class AllowMsgCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(IPlayerActor PlayerActor) {
            PlayerActor.HearWhisper = !PlayerActor.HearWhisper;
            if (PlayerActor.HearWhisper) {
                PlayerActor.SysMsg(CommandHelp.EnableHearWhisper, MsgColor.Green, MsgType.Hint);
            }
            else {
                PlayerActor.SysMsg(CommandHelp.DisableHearWhisper, MsgColor.Green, MsgType.Hint);
            }
        }
    }
}