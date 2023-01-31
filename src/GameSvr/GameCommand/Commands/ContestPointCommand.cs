using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 查看行会战的得分数
    /// </summary>
    [Command("ContestPoint", "查看行会战的得分数", "行会名称", 10)]
    public class ContestPointCommand : Command
    {
        [ExecuteCommand]
        public void ContestPoint(string[] @Params, PlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            string sGuildName = @Params.Length > 0 ? @Params[0] : "";
            if (sGuildName == "" || sGuildName != "" && sGuildName[0] == '?')
            {
                PlayObject.SysMsg("查看行会战的得分数。", MsgColor.Red, MsgType.Hint);
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
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