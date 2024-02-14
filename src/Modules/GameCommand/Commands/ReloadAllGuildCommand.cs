using SystemModule;
using SystemModule.Actors;
using SystemModule.Enums;

namespace CommandModule.Commands
{
    /// <summary>
    /// 重新读取所有行会
    /// </summary>
    [Command("ReloadAllGuild", "重新读取所有行会", 10)]
    public class ReloadAllGuildCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(IPlayerActor PlayerActor)
        {
            if (SystemShare.ServerIndex != 0)
            {
                PlayerActor.SysMsg(CommandHelp.GameCommandReloadGuildOnMasterserver, MsgColor.Red, MsgType.Hint);
                return;
            }
            SystemShare.GuildMgr.LoadGuildInfo();
            PlayerActor.SysMsg("重新加载行会信息完成.", MsgColor.Red, MsgType.Hint);
        }
    }
}