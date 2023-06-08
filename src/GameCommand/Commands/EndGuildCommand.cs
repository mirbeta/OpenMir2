using M2Server.Player;
using M2Server.World;
using SystemModule;
using SystemModule.Enums;

namespace M2Server.GameCommand.Commands {
    /// <summary>
    /// 退出行会
    /// </summary>
    [Command("EndGuild", "退出行会")]
    public class EndGuildCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject playObject) {
            if (playObject.MyGuild != null) {
                if (playObject.GuildRankNo > 1) {
                    if (playObject.MyGuild.IsMember(playObject.ChrName) && playObject.MyGuild.DelMember(playObject.ChrName)) {
                        WorldServer.SendServerGroupMsg(Messages.SS_207, M2Share.ServerIndex, playObject.MyGuild.GuildName);
                        playObject.MyGuild = null;
                        playObject.RefRankInfo(0, "");
                        playObject.RefShowName();
                        playObject.SysMsg("你已经退出行会。", MsgColor.Green, MsgType.Hint);
                    }
                }
                else {
                    playObject.SysMsg("行会掌门人不能这样退出行会!!!", MsgColor.Red, MsgType.Hint);
                }
            }
            else {
                playObject.SysMsg("你都没加入行会!!!", MsgColor.Red, MsgType.Hint);
            }
        }
    }
}