using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    [Command("Letguild", "加入公会", "", 0)]
    public class LetGuildCommand : Command
    {
        [ExecuteCommand]
        public static void Letguild(PlayObject playObject)
        {
            playObject.AllowGuild = !playObject.AllowGuild;
            if (playObject.AllowGuild)
            {
                playObject.SysMsg(CommandHelp.EnableJoinGuild, MsgColor.Green, MsgType.Hint);
            }
            else
            {
                playObject.SysMsg(CommandHelp.DisableJoinGuild, MsgColor.Green, MsgType.Hint);
            }
            return;
        }
    }
}