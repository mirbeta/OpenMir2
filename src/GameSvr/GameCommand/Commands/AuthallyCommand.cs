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
                playObject.MyGuild.m_boEnableAuthAlly = !playObject.MyGuild.m_boEnableAuthAlly;
                if (playObject.MyGuild.m_boEnableAuthAlly)
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