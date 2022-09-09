using GameSvr.Player;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 重新加载游戏公告
    /// </summary>
    [GameCommand("ReloadLineNotice", "重新加载游戏公告", 10)]
    public class ReloadLineNoticeCommand : BaseCommond
    {
        [DefaultCommand]
        public void ReloadLineNotice(PlayObject PlayObject)
        {
            if (M2Share.LoadLineNotice(Path.Combine(M2Share.BasePath, M2Share.Config.NoticeDir, "LineNotice.txt")))
            {
                PlayObject.SysMsg(GameCommandConst.GameCommandReloadLineNoticeSuccessMsg, MsgColor.Green, MsgType.Hint);
            }
            else
            {
                PlayObject.SysMsg(GameCommandConst.GameCommandReloadLineNoticeFailMsg, MsgColor.Red, MsgType.Hint);
            }
        }
    }
}