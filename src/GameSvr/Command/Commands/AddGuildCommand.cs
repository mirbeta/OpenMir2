using GameSvr.Player;
using SystemModule;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 创建行会
    /// </summary>
    [GameCommand("AddGuild", "新建一个行会", "行会名称 掌门人名称", 10)]
    public class AddGuildCommand : BaseCommond
    {
        [DefaultCommand]
        public void AddGuild(string[] @Params, TPlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            TPlayObject Human;
            bool boAddState;
            var sGuildName = @Params.Length > 0 ? @Params[0] : "";
            var sGuildChief = @Params.Length > 1 ? @Params[1] : "";
            if (M2Share.nServerIndex != 0)
            {
                PlayObject.SysMsg("这个命令只能使用在主服务器上", MsgColor.Red, MsgType.Hint);
                return;
            }
            if (string.IsNullOrEmpty(sGuildName) || sGuildChief == "")
            {
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            boAddState = false;
            Human = M2Share.UserEngine.GetPlayObject(sGuildChief);
            if (Human == null)
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sNowNotOnLineOrOnOtherServer, sGuildChief), MsgColor.Red, MsgType.Hint);
                return;
            }
            if (M2Share.GuildManager.MemberOfGuild(sGuildChief) == null)
            {
                if (M2Share.GuildManager.AddGuild(sGuildName, sGuildChief))
                {
                    M2Share.UserEngine.SendServerGroupMsg(Grobal2.SS_205, M2Share.nServerIndex, sGuildName + '/' + sGuildChief);
                    PlayObject.SysMsg("行会名称: " + sGuildName + " 掌门人: " + sGuildChief, MsgColor.Green, MsgType.Hint);
                    boAddState = true;
                }
            }
            if (boAddState)
            {
                Human.m_MyGuild = M2Share.GuildManager.MemberOfGuild(Human.m_sCharName);
                if (Human.m_MyGuild != null)
                {
                    Human.m_sGuildRankName = Human.m_MyGuild.GetRankName(PlayObject, ref Human.m_nGuildRankNo);
                    Human.RefShowName();
                }
            }
        }
    }
}