using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    [Command("Banguildchat", "", "", 0)]
    public class BanGuildChatCommand : Command
    {
        [ExecuteCommand]
        public static void Banguildchat(PlayObject playObject)
        {
            playObject.BanGuildChat = !playObject.BanGuildChat;
            if (playObject.BanGuildChat)
            {
                playObject.SysMsg(CommandHelp.EnableGuildChat, MsgColor.Green, MsgType.Hint);
            }
            else
            {
                playObject.SysMsg(CommandHelp.DisableGuildChat, MsgColor.Green, MsgType.Hint);
            }
            return;
        }
    }
}