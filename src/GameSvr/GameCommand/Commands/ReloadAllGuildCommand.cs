using GameSvr.Player;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands {
    /// <summary>
    /// 重新读取所有行会
    /// </summary>
    [Command("ReloadAllGuild", "重新读取所有行会", 10)]
    public class ReloadAllGuildCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject PlayObject) {
            if (M2Share.ServerIndex != 0) {
                PlayObject.SysMsg(CommandHelp.GameCommandReloadGuildOnMasterserver, MsgColor.Red, MsgType.Hint);
                return;
            }
            M2Share.GuildMgr.LoadGuildInfo();
            PlayObject.SysMsg("重新加载行会信息完成.", MsgColor.Red, MsgType.Hint);
        }
    }
}