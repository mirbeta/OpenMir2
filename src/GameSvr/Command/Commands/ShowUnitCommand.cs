namespace GameSvr.Command.Commands
{

    [GameCommand("ShowUnit", "", "", 10)]
    public class ShowUnitCommand : BaseCommond
    {
        [DefaultCommand()]
        public void ShowUnit()
        {

        }
    }
}