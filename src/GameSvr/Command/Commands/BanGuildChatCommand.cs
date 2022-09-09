using GameSvr.Player;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    [GameCommand("Banguildchat", "", "", 0)]
    public class BanGuildChatCommand : BaseCommond
    {
        [DefaultCommand]
        public void Banguildchat(PlayObject playObject)
        {
            playObject.BanGuildChat = !playObject.BanGuildChat;
            if (playObject.BanGuildChat)
            {
                playObject.SysMsg(M2Share.g_sEnableGuildChat, MsgColor.Green, MsgType.Hint);
            }
            else
            {
                playObject.SysMsg(M2Share.g_sDisableGuildChat, MsgColor.Green, MsgType.Hint);
            }
            return;
        }
    }
}