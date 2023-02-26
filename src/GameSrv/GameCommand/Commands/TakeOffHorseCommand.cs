using GameSrv.Player;

namespace GameSrv.GameCommand.Commands {
    [Command("TakeOffHorse", desc: "下马命令，在骑马状态输入此命令下马。", 10)]
    public class TakeOffHorseCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject PlayObject) {
            if (!PlayObject.OnHorse) {
                return;
            }
            PlayObject.OnHorse = false;
            PlayObject.FeatureChanged();
        }
    }
}