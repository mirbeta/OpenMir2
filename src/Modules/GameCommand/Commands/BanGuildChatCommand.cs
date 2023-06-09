using SystemModule;
using SystemModule.Enums;

namespace CommandSystem
{
    [Command("Banguildchat", "", "")]
    public class BanGuildChatCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(IPlayerActor PlayerActor)
        {
            PlayerActor.SysMsgBanGuildChat = !PlayerActor.SysMsgBanGuildChat;
            if (PlayerActor.SysMsgBanGuildChat)
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