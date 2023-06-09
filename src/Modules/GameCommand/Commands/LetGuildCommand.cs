using SystemModule;
using SystemModule.Enums;

namespace CommandSystem {
    [Command("Letguild", "加入公会", "")]
    public class LetGuildCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(IPlayerActor PlayerActor) {
            PlayerActor.SysMsgAllowGuild = !PlayerActor.SysMsgAllowGuild;
            if (PlayerActor.SysMsgAllowGuild) {
                PlayerActor.SysMsg(CommandHelp.EnableJoinGuild, MsgColor.Green, MsgType.Hint);
            }
            else {
                PlayerActor.SysMsg(CommandHelp.DisableJoinGuild, MsgColor.Green, MsgType.Hint);
            }
            return;
        }
    }
}