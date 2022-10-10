using GameSvr.Player;

namespace GameSvr.Command.Commands
{
    [Command("Training", "", 10)]
    public class TrainingCommand : Command
    {
        [ExecuteCommand]
        public void Training(PlayObject PlayObject)
        {
            if (PlayObject.Permission < 6)
            {
                return;
            }
        }
    }
}