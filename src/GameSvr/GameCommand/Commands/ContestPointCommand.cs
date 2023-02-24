using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 查看行会战的得分数
    /// </summary>
    [Command("ContestPoint", "查看行会战的得分数", "行会名称", 10)]
    public class ContestPointCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            string sGuildName = @Params.Length > 0 ? @Params[0] : "";
            if (sGuildName == "" || !string.IsNullOrEmpty(sGuildName) && sGuildName[0] == '?')
            {
                PlayObject.SysMsg("查看行会战的得分数。", MsgColor.Red, MsgType.Hint);
                PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            Guild.GuildInfo Guild = M2Share.GuildMgr.FindGuild(sGuildName);
            if (Guild != null)
            {
                PlayObject.SysMsg($"{sGuildName} 的得分为: {Guild.nContestPoint}", MsgColor.Green, MsgType.Hint);
            }
            else
            {
                PlayObject.SysMsg($"行会: {sGuildName} 不存在!!!", MsgColor.Green, MsgType.Hint);
            }
        }
    }
}