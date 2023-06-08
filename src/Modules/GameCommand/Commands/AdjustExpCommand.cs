using M2Server.Player;

namespace M2Server.GameCommand.Commands {
    /// <summary>
    /// 暂时不清楚干啥的
    /// </summary>
    [Command("AdjustExp", "", 10)]
    public class AdjustExpCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @params, PlayObject playObject) {
            if (playObject.Permission < 6) {
                return;
            }
        }
    }
}