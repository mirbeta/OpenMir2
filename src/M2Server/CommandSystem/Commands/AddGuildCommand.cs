using SystemModule;
using System;
using M2Server.CommandSystem;

namespace M2Server
{
    /// <summary>
    /// 创建行会
    /// </summary>
    [GameCommand("AddGuild", "创建行会", 10)]
    public class AddGuildCommand : BaseCommond
    {
        [DefaultCommand]
        public void AddGuild(string[] @Params, TPlayObject PlayObject)
        {
            TPlayObject Human;
            bool boAddState;
            var sGuildName = @Params.Length > 0 ? @Params[0] : "";
            var sGuildChief = @Params.Length > 1 ? @Params[1] : "";
            if (M2Share.nServerIndex != 0)
            {
                PlayObject.SysMsg("这个命令只能使用在主服务器上", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if ((sGuildName == "") || (sGuildChief == ""))
            {
                PlayObject.SysMsg("命令格式: @" + this.Attributes.Name + " 行会名称 掌门人名称", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            boAddState = false;
            Human = M2Share.UserEngine.GetPlayObject(sGuildChief);
            if (Human == null)
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sNowNotOnLineOrOnOtherServer, sGuildChief), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (M2Share.GuildManager.MemberOfGuild(sGuildChief) == null)
            {
                if (M2Share.GuildManager.AddGuild(sGuildName, sGuildChief))
                {
                    // UserEngine.SendServerGroupMsg(SS_205, nServerIndex, sGuildName + '/' + sGuildChief);
                    PlayObject.SysMsg("行会名称: " + sGuildName + " 掌门人: " + sGuildChief, TMsgColor.c_Green, TMsgType.t_Hint);
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