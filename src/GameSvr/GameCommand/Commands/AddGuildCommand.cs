using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 创建行会
    /// </summary>
    [Command("AddGuild", "新建一个行会", "行会名称 掌门人名称", 10)]
    public class AddGuildCommand : GameCommand
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
                PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            bool boAddState = false;
            var chiefObject = M2Share.WorldEngine.GetPlayObject(sGuildChief);
            if (chiefObject == null)
            {
                PlayObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sGuildChief), MsgColor.Red, MsgType.Hint);
                return;
            }
            if (M2Share.GuildMgr.MemberOfGuild(sGuildChief) == null)
            {
                if (M2Share.GuildMgr.AddGuild(sGuildName, sGuildChief))
                {
                    World.WorldServer.SendServerGroupMsg(Messages.SS_205, M2Share.ServerIndex, sGuildName + '/' + sGuildChief);
                    PlayObject.SysMsg("行会名称: " + sGuildName + " 掌门人: " + sGuildChief, MsgColor.Green, MsgType.Hint);
                    boAddState = true;
                }
            }
            if (boAddState)
            {
                chiefObject.MyGuild = M2Share.GuildMgr.MemberOfGuild(chiefObject.ChrName);
                if (chiefObject.MyGuild != null)
                {
                    chiefObject.GuildRankName = chiefObject.MyGuild.GetRankName(PlayObject, ref chiefObject.GuildRankNo);
                    chiefObject.RefShowName();
                }
            }
        }
    }
}