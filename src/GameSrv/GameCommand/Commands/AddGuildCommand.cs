using GameSrv.Player;
using GameSrv.World;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 创建行会
    /// </summary>
    [Command("AddGuild", "新建一个行会", "行会名称 掌门人名称", 10)]
    public class AddGuildCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null) {
                return;
            }
            var sGuildName = @params.Length > 0 ? @params[0] : "";
            var sGuildChief = @params.Length > 1 ? @params[1] : "";
            if (GameShare.ServerIndex != 0) {
                playObject.SysMsg("这个命令只能使用在主服务器上", MsgColor.Red, MsgType.Hint);
                return;
            }
            if (string.IsNullOrEmpty(sGuildName) || string.IsNullOrEmpty(sGuildChief)) {
                playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var boAddState = false;
            var chiefObject = GameShare.WorldEngine.GetPlayObject(sGuildChief);
            if (chiefObject == null) {
                playObject.SysMsg(string.Format(CommandHelp.NowNotOnLineOrOnOtherServer, sGuildChief), MsgColor.Red, MsgType.Hint);
                return;
            }
            if (GameShare.GuildMgr.MemberOfGuild(sGuildChief) == null) {
                if (GameShare.GuildMgr.AddGuild(sGuildName, sGuildChief)) {
                    WorldServer.SendServerGroupMsg(Messages.SS_205, GameShare.ServerIndex, sGuildName + '/' + sGuildChief);
                    playObject.SysMsg("行会名称: " + sGuildName + " 掌门人: " + sGuildChief, MsgColor.Green, MsgType.Hint);
                    boAddState = true;
                }
            }
            if (boAddState) {
                chiefObject.MyGuild = GameShare.GuildMgr.MemberOfGuild(chiefObject.ChrName);
                if (chiefObject.MyGuild != null) {
                    chiefObject.GuildRankName = chiefObject.MyGuild.GetRankName(playObject, ref chiefObject.GuildRankNo);
                    chiefObject.RefShowName();
                }
            }
        }
    }
}