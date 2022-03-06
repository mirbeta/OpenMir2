using SystemModule;
using System;
using GameSvr.CommandSystem;

namespace GameSvr
{
    /// <summary>
    /// 查看行会战的得分数
    /// </summary>
    [GameCommand("ContestPoint", "查看行会战的得分数", "行会名称", 10)]
    public class ContestPointCommand : BaseCommond
    {
        [DefaultCommand]
        public void ContestPoint(string[] @Params, TPlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sGuildName = @Params.Length > 0 ? @Params[0] : "";
            if (sGuildName == "" || sGuildName != "" && sGuildName[0] == '?')
            {
                PlayObject.SysMsg("查看行会战的得分数。", MsgColor.Red, MsgType.Hint);
                PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            var Guild = M2Share.GuildManager.FindGuild(sGuildName);
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