using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 此命令允许或禁止夫妻传送
    /// </summary>
    [Command("AllowDearRecall", "", 10)]
    public class AllowDearRecallCommand : Command
    {
        [ExecuteCommand]
        public static void AllowDearRecall(string[] @Params, PlayObject PlayObject)
        {
            PlayObject.CanDearRecall = !PlayObject.CanDearRecall;
            if (PlayObject.CanDearRecall)
            {
                PlayObject.SysMsg(CommandHelp.EnableDearRecall, MsgColor.Blue, MsgType.Hint);
            }
            else
            {
                PlayObject.SysMsg(CommandHelp.DisableDearRecall, MsgColor.Blue, MsgType.Hint);
            }
        }
    }
}