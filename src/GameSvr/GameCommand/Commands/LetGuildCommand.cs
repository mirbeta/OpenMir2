using GameSvr.Player;
using SystemModule.Data;

namespace GameSvr.GameCommand.Commands
{
    [Command("Letguild", "加入公会", "", 0)]
    public class LetGuildCommand : Command
    {
        [ExecuteCommand]
        public void Letguild(PlayObject playObject)
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