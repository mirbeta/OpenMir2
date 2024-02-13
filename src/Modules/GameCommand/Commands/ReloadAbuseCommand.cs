using SystemModule.Actors;

namespace CommandModule.Commands
{
    [Command("ReloadAbuse", "无用", 10)]
    public class ReloadAbuseCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(IPlayerActor PlayerActor)
        {

        }
    }
}