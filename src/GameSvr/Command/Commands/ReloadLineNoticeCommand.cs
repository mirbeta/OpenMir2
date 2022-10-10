using GameSvr.Player;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 重新加载游戏公告
    /// </summary>
    [Command("ReloadLineNotice", "重新加载游戏公告", 10)]
    public class ReloadLineNoticeCommand : Commond
    {
        [ExecuteCommand]
        public void ReloadLineNotice(PlayObject PlayObject)
        {
            if (M2Share.LoadLineNotice(Path.Combine(M2Share.BasePath, M2Share.Config.NoticeDir, "LineNotice.txt")))
            {
                PlayObject.SysMsg(CommandHelp.GameCommandReloadLineNoticeSuccessMsg, MsgColor.Green, MsgType.Hint);
            }
            else
            {
                PlayObject.SysMsg(CommandHelp.GameCommandReloadLineNoticeFailMsg, MsgColor.Red, MsgType.Hint);
            }
        }
    }
}