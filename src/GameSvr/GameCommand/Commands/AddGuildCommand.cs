using GameSvr.Player;
using SystemModule;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 创建行会
    /// </summary>
    [Command("AddGuild", "新建一个行会", "行会名称 掌门人名称", 10)]
    public class AddGuildCommand : Command
    {
        [ExecuteCommand]
        public void AddGuild(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sGuildName = @Params.Length > 0 ? @Params[0] : "";
            var sGuildChief = @Params.Length > 1 ? @Params[1] : "";
            if (M2Share.ServerIndex != 0)
            {
                PlayObject.SysMsg("这个命令只能使用在主服务器上", MsgColor.Red, MsgType.Hint);
                return;
            }
            if (string.IsNullOrEmpty(sGuildName) || sGuildChief == "")
            {
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var boAddState = false;
            var Human = M2Share.WorldEngine.GetPlayObject(sGuildChief);
            if (Human == null)
            {
                PlayObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sGuildChief), MsgColor.Red, MsgType.Hint);
                return;
            }
            if (M2Share.GuildMgr.MemberOfGuild(sGuildChief) == null)
            {
                if (M2Share.GuildMgr.AddGuild(sGuildName, sGuildChief))
                {
                    M2Share.WorldEngine.SendServerGroupMsg(Grobal2.SS_205, M2Share.ServerIndex, sGuildName + '/' + sGuildChief);
                    PlayObject.SysMsg("行会名称: " + sGuildName + " 掌门人: " + sGuildChief, MsgColor.Green, MsgType.Hint);
                    boAddState = true;
                }
            }
            if (boAddState)
            {
                Human.MyGuild = M2Share.GuildMgr.MemberOfGuild(Human.ChrName);
                if (Human.MyGuild != null)
                {
                    Human.GuildRankName = Human.MyGuild.GetRankName(PlayObject, ref Human.GuildRankNo);
                    Human.RefShowName();
                }
            }
        }
    }
}