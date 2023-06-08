using M2Server.Player;
using SystemModule.Enums;

namespace M2Server.GameCommand.Commands {
    /// <summary>
    /// 此命令用于允许或禁止编组传送功能
    /// </summary>
    [Command("AllowGroupReCall", "此命令用于允许或禁止编组传送功能")]
    public class AllowGroupReCallCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject playObject) {
            playObject.AllowGroupReCall = !playObject.AllowGroupReCall;
            if (playObject.AllowGroupReCall) {
                playObject.SysMsg(CommandHelp.EnableGroupRecall, MsgColor.Green, MsgType.Hint);
            }
            else {
                playObject.SysMsg(CommandHelp.DisableGroupRecall, MsgColor.Green, MsgType.Hint);
            }
        }
    }
}