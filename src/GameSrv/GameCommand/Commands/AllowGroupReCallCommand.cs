using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 此命令用于允许或禁止编组传送功能
    /// </summary>
    [Command("AllowGroupReCall", "此命令用于允许或禁止编组传送功能", 0)]
    public class AllowGroupReCallCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject PlayObject) {
            PlayObject.AllowGroupReCall = !PlayObject.AllowGroupReCall;
            if (PlayObject.AllowGroupReCall) {
                PlayObject.SysMsg(CommandHelp.EnableGroupRecall, MsgColor.Green, MsgType.Hint);
            }
            else {
                PlayObject.SysMsg(CommandHelp.DisableGroupRecall, MsgColor.Green, MsgType.Hint);
            }
        }
    }
}