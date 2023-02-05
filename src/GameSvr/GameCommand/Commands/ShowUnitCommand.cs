namespace GameSvr.GameCommand.Commands
{

    [Command("ShowUnit", "", "", 10)]
    public class ShowUnitCommand : GameCommand
    {
        [ExecuteCommand()]
        public static void ShowUnit()
        {

        }
    }
}