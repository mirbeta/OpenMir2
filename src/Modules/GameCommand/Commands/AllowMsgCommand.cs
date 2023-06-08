using M2Server.Player;
using SystemModule.Enums;

namespace M2Server.GameCommand.Commands {
    [Command("Allowmsg", "", "")]
    public class AllowMsgCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject playObject) {
            playObject.HearWhisper = !playObject.HearWhisper;
            if (playObject.HearWhisper) {
                playObject.SysMsg(CommandHelp.EnableHearWhisper, MsgColor.Green, MsgType.Hint);
            }
            else {
                playObject.SysMsg(CommandHelp.DisableHearWhisper, MsgColor.Green, MsgType.Hint);
            }
        }
    }
}