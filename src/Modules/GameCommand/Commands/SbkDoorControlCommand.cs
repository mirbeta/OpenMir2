using SystemModule;

namespace CommandSystem.Commands
{
    [Command("SbkDoorControl", "", 10)]
    public class SbkDoorControlCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(IPlayerActor PlayerActor)
        {
        }
    }
}