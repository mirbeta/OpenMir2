using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 重新读取所有行会
    /// </summary>
    [Command("ReloadAllGuild", "重新读取所有行会", 10)]
    public class ReloadAllGuildCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject playObject) {
            if (GameShare.ServerIndex != 0) {
                playObject.SysMsg(CommandHelp.GameCommandReloadGuildOnMasterserver, MsgColor.Red, MsgType.Hint);
                return;
            }
            GameShare.GuildMgr.LoadGuildInfo();
            playObject.SysMsg("重新加载行会信息完成.", MsgColor.Red, MsgType.Hint);
        }
    }
}