using GameSrv.Player;
using GameSrv.World;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 退出行会
    /// </summary>
    [Command("EndGuild", "退出行会", 0)]
    public class EndGuildCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject PlayObject) {
            if (PlayObject.MyGuild != null) {
                if (PlayObject.GuildRankNo > 1) {
                    if (PlayObject.MyGuild.IsMember(PlayObject.ChrName) && PlayObject.MyGuild.DelMember(PlayObject.ChrName)) {
                        WorldServer.SendServerGroupMsg(Messages.SS_207, M2Share.ServerIndex, PlayObject.MyGuild.GuildName);
                        PlayObject.MyGuild = null;
                        PlayObject.RefRankInfo(0, "");
                        PlayObject.RefShowName();
                        PlayObject.SysMsg("你已经退出行会。", MsgColor.Green, MsgType.Hint);
                    }
                }
                else {
                    PlayObject.SysMsg("行会掌门人不能这样退出行会!!!", MsgColor.Red, MsgType.Hint);
                }
            }
            else {
                PlayObject.SysMsg("你都没加入行会!!!", MsgColor.Red, MsgType.Hint);
            }
        }
    }
}