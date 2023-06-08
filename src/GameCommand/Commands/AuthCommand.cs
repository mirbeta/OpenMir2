using M2Server.Player;

namespace M2Server.GameCommand.Commands {
    /// <summary>
    /// 此命令允许或禁止公会联盟
    /// </summary>
    [Command("Auth", "")]
    public class AuthCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject playObject) {
            if (playObject.IsGuildMaster()) {
                playObject.ClientGuildAlly();
            }
        }
    }
}