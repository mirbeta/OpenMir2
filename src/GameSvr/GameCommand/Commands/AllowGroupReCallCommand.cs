using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    /// <summary>
    /// 此命令用于允许或禁止编组传送功能
    /// </summary>
    [Command("AllowGroupReCall", "此命令用于允许或禁止编组传送功能", 0)]
    public class AllowGroupReCallCommand : Command
    {
        [ExecuteCommand]
        public void AllowGroupReCall(PlayObject PlayObject)
        {
            PlayObject.AllowGroupReCall = !PlayObject.AllowGroupReCall;
            if (PlayObject.AllowGroupReCall)
            {
                PlayObject.SysMsg(CommandHelp.EnableGroupRecall, MsgColor.Green, MsgType.Hint);
            }
            else
            {
                PlayObject.SysMsg(CommandHelp.DisableGroupRecall, MsgColor.Green, MsgType.Hint);
            }
        }
    }
}