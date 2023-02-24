using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    [Command("Authally", "", "", 0)]
    internal class AuthallyCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(PlayObject playObject)
        {
            if (playObject.IsGuildMaster())
            {
                playObject.MyGuild.MBoEnableAuthAlly = !playObject.MyGuild.MBoEnableAuthAlly;
                if (playObject.MyGuild.MBoEnableAuthAlly)
                {
                    playObject.SysMsg(CommandHelp.EnableAuthAllyGuild, MsgColor.Green, MsgType.Hint);
                }
                else
                {
                    playObject.SysMsg(CommandHelp.DisableAuthAllyGuild, MsgColor.Green, MsgType.Hint);
                }
            }
            return;
        }
    }
}