using GameSrv.Guild;
using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 重新读取行会
    /// </summary>
    [Command("ReloadGuild", "重新读取指定行会", 10)]
    public class ReloadGuildCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null) {
                return;
            }
            var sParam1 = string.Empty;
            if (@params.Length > 0) {
                sParam1 = @params.Length > 0 ? @params[0] : "";
                if (string.IsNullOrEmpty(sParam1)) {
                    playObject.SysMsg(string.Format(CommandHelp.GameCommandParamUnKnow, this.Command.Name, CommandHelp.GameCommandReloadGuildHelpMsg), MsgColor.Red, MsgType.Hint);
                    return;
                }
            }
            if (M2Share.ServerIndex != 0) {
                playObject.SysMsg(CommandHelp.GameCommandReloadGuildOnMasterserver, MsgColor.Red, MsgType.Hint);
                return;
            }
            var guild = M2Share.GuildMgr.FindGuild(sParam1);
            if (guild == null) {
                playObject.SysMsg(string.Format(CommandHelp.GameCommandReloadGuildNotFoundGuildMsg, sParam1), MsgColor.Red, MsgType.Hint);
                return;
            }
            guild.LoadGuild();
            playObject.SysMsg(string.Format(CommandHelp.GameCommandReloadGuildSuccessMsg, sParam1), MsgColor.Red, MsgType.Hint);
            // WorldEngine.SendServerGroupMsg(SS_207, nServerIndex, sParam1);
        }
    }
}