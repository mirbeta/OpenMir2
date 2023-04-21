using GameSrv.Guild;
using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 查看行会战的得分数
    /// </summary>
    [Command("ContestPoint", "查看行会战的得分数", "行会名称", 10)]
    public class ContestPointCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null) {
                return;
            }
            var sGuildName = @params.Length > 0 ? @params[0] : "";
            if (string.IsNullOrEmpty(sGuildName) || !string.IsNullOrEmpty(sGuildName) && sGuildName[0] == '?') {
                playObject.SysMsg("查看行会战的得分数。", MsgColor.Red, MsgType.Hint);
                playObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var guild = GameShare.GuildMgr.FindGuild(sGuildName);
            if (guild != null) {
                playObject.SysMsg($"{sGuildName} 的得分为: {guild.ContestPoint}", MsgColor.Green, MsgType.Hint);
            }
            else {
                playObject.SysMsg($"行会: {sGuildName} 不存在!!!", MsgColor.Green, MsgType.Hint);
            }
        }
    }
}