using SystemModule;
using SystemModule.Enums;

namespace CommandModule.Commands
{
    [Command("Banguildchat", "", "")]
    public class BanGuildChatCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(IPlayerActor PlayerActor)
        {
            PlayerActor.BanGuildChat = !PlayerActor.BanGuildChat;
            if (PlayerActor.BanGuildChat)
            {
                PlayerActor.SysMsg(CommandHelp.EnableGuildChat, MsgColor.Green, MsgType.Hint);
            }
            else
            {
                PlayerActor.SysMsg(CommandHelp.DisableGuildChat, MsgColor.Green, MsgType.Hint);
            }
        }
    }
}