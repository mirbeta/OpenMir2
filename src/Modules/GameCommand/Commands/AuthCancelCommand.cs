using M2Server.Player;

namespace M2Server.GameCommand.Commands {
    /// <summary>
    /// 此命令允许公会取消联盟
    /// </summary>
    [Command("AuthCancel", "")]
    public class AuthCancelCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (@params == null || @params.Length <= 0) {
                return;
            }
            if (playObject.IsGuildMaster()) {
                playObject.ClientGuildBreakAlly(@params[0]);
            }
        }
    }
}