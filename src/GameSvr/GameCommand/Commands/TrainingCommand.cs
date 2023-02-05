using GameSvr.Player;

namespace GameSvr.GameCommand.Commands
{
    [Command("Training", "", 10)]
    public class TrainingCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(PlayObject PlayObject)
        {
            if (PlayObject.Permission < 6)
            {
                return;
            }
        }
    }
}