namespace GameSvr.GameCommand.Commands
{

    [Command("ShowUnit", "", "", 10)]
    public class ShowUnitCommand : Command
    {
        [ExecuteCommand()]
        public static void ShowUnit()
        {

        }
    }
}