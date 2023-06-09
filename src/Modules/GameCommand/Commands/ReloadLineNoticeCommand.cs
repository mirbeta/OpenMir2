using SystemModule;
using SystemModule.Enums;

namespace CommandModule.Commands
{
    /// <summary>
    /// 重新加载游戏公告
    /// </summary>
    [Command("ReloadLineNotice", "重新加载游戏公告", 10)]
    public class ReloadLineNoticeCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(IPlayerActor PlayerActor)
        {
            if (ModuleShare.LoadLineNotice(ModuleShare.GetNoticeFilePath("LineNotice.txt")))
            {
                PlayerActor.SysMsg(CommandHelp.GameCommandReloadLineNoticeSuccessMsg, MsgColor.Green, MsgType.Hint);
            }
            else
            {
                PlayerActor.SysMsg(CommandHelp.GameCommandReloadLineNoticeFailMsg, MsgColor.Red, MsgType.Hint);
            }
        }
    }
}