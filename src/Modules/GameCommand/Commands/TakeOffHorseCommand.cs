using M2Server.Player;

namespace M2Server.GameCommand.Commands {
    [Command("TakeOffHorse", desc: "下马命令，在骑马状态输入此命令下马。", 10)]
    public class TakeOffHorseCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject playObject) {
            if (!playObject.OnHorse) {
                return;
            }
            playObject.OnHorse = false;
            playObject.FeatureChanged();
        }
    }
}