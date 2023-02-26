using GameSrv.Player;

namespace GameSrv.GameCommand.Commands {
    /// <summary>
    /// 暂时不清楚干啥的
    /// </summary>
    [Command("AdjustExp", "", 10)]
    public class AdjustExpCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(string[] @Params, PlayObject PlayObject) {
            if (PlayObject.Permission < 6) {
                return;
            }
        }
    }
}