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
                    playObject.SysMsg(GameCommandConst.EnableAuthAllyGuild, MsgColor.Green, MsgType.Hint);
                }
                else
                {
                    playObject.SysMsg(GameCommandConst.DisableAuthAllyGuild, MsgColor.Green, MsgType.Hint);
                }
            }
            return;
        }
    }
}