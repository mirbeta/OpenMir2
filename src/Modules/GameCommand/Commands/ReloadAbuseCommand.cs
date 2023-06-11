using SystemModule;

namespace CommandSystem.Commands
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