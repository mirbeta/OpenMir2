namespace GameSvr.Command.Commands
{

    [Command("ShowUnit", "", "", 10)]
    public class ShowUnitCommand : Commond
    {
        [ExecuteCommand()]
        public void ShowUnit()
        {

        }
    }
}