using SystemModule;
using SystemModule.Enums;

namespace CommandModule.Commands
{
    /// <summary>
    /// 创建行会
    /// </summary>
    [Command("AddGuild", "新建一个行会", "行会名称 掌门人名称", 10)]
    public class AddGuildCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (@params == null)
            {
                return;
            }
            var sGuildName = @params.Length > 0 ? @params[0] : "";
            var sGuildChief = @params.Length > 1 ? @params[1] : "";
            if (ModuleShare.ServerIndex != 0)
            {
                PlayerActor.SysMsg("这个命令只能使用在主服务器上", MsgColor.Red, MsgType.Hint);
                return;
            }
            if (string.IsNullOrEmpty(sGuildName) || string.IsNullOrEmpty(sGuildChief))
            {
                PlayerActor.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var boAddState = false;
            var chiefObject = ModuleShare.WorldEngine.GetPlayObject(sGuildChief);
            if (chiefObject == null)
            {
                PlayerActor.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sGuildChief), MsgColor.Red, MsgType.Hint);
                return;
            }
            if (ModuleShare.GuildMgr.MemberOfGuild(sGuildChief) == null)
            {
                if (ModuleShare.GuildMgr.AddGuild(sGuildName, sGuildChief))
                {
                    ModuleShare.WorldEngine.SendServerGroupMsg(Messages.SS_205, ModuleShare.ServerIndex, sGuildName + '/' + sGuildChief);
                    PlayerActor.SysMsg("行会名称: " + sGuildName + " 掌门人: " + sGuildChief, MsgColor.Green, MsgType.Hint);
                    boAddState = true;
                }
            }
            if (boAddState)
            {
                //chiefObject.MyGuild = ModuleShare.GuildMgr.MemberOfGuild(chiefObject.ChrName);
                //if (chiefObject.MyGuild != null)
                //{
                //    chiefObject.GuildRankName = chiefObject.MyGuild.GetRankName(PlayerActor, ref chiefObject.GuildRankNo);
                //    chiefObject.RefShowName();
                //}
            }
        }
    }
}