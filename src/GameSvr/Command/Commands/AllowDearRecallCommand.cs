using GameSvr.Player;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 此命令允许或禁止夫妻传送
    /// </summary>
    [GameCommand("AllowDearRecall", "", 10)]
    public class AllowDearRecallCommand : BaseCommond
    {
        [DefaultCommand]
        public void AllowDearRecall(string[] @Params, PlayObject PlayObject)
        {
            PlayObject.m_boCanDearRecall = !PlayObject.m_boCanDearRecall;
            if (PlayObject.m_boCanDearRecall)
            {
                PlayObject.SysMsg(GameCommandConst.EnableDearRecall, MsgColor.Blue, MsgType.Hint);
            }
            else
            {
                PlayObject.SysMsg(GameCommandConst.DisableDearRecall, MsgColor.Blue, MsgType.Hint);
            }
        }
    }
}