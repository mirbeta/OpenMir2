namespace GameSvr.GameCommand.Commands
{

    [Command("ShowUnit", "", "", 10)]
    public class ShowUnitCommand : Command
    {
        [ExecuteCommand()]
        public void ShowUnit()
        {

        }
    }
}