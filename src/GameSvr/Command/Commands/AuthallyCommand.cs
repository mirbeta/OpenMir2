using GameSvr.Player;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    [GameCommand("Authally", "", "", 0)]
    internal class AuthallyCommand : BaseCommond
    {
        [DefaultCommand]
        public void Authally(PlayObject playObject)
        {
            if (playObject.IsGuildMaster())
            {
                playObject.MyGuild.m_boEnableAuthAlly = !playObject.MyGuild.m_boEnableAuthAlly;
                if (playObject.MyGuild.m_boEnableAuthAlly)
                {
                    playObject.SysMsg(M2Share.g_sEnableAuthAllyGuild, MsgColor.Green, MsgType.Hint);
                }
                else
                {
                    playObject.SysMsg(M2Share.g_sDisableAuthAllyGuild, MsgColor.Green, MsgType.Hint);
                }
            }
            return;
        }
    }
}