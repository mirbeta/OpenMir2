using SystemModule;
using SystemModule.Enums;

namespace CommandSystem
{
    /// <summary>
    /// 查看行会战的得分数
    /// </summary>
    [Command("ContestPoint", "查看行会战的得分数", "行会名称", 10)]
    public class ContestPointCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor) {
            if (@params == null) {
                return;
            }
            var sGuildName = @params.Length > 0 ? @params[0] : "";
            if (string.IsNullOrEmpty(sGuildName) || !string.IsNullOrEmpty(sGuildName) && sGuildName[0] == '?') {
                PlayerActor.SysMsg("查看行会战的得分数。", MsgColor.Red, MsgType.Hint);
                PlayerActor.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var guild = SystemShare.GuildMgr.FindGuild(sGuildName);
            if (guild != null) {
                PlayerActor.SysMsg($"{sGuildName} 的得分为: {guild.ContestPoint}", MsgColor.Green, MsgType.Hint);
            }
            else {
                PlayerActor.SysMsg($"行会: {sGuildName} 不存在!!!", MsgColor.Green, MsgType.Hint);
            }
        }
    }
}