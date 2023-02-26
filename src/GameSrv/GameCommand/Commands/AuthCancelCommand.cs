using GameSrv.Player;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 此命令允许公会取消联盟
    /// </summary>
    [Command("AuthCancel", "", 0)]
    public class AuthCancelCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject PlayObject) {
            if (@params == null || @params.Length <= 0) {
                return;
            }
            if (PlayObject.IsGuildMaster()) {
                PlayObject.ClientGuildBreakAlly(@params[0]);
            }
        }
    }
}