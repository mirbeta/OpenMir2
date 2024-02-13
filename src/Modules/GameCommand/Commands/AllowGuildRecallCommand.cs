using SystemModule.Actors;
using SystemModule.Enums;

namespace CommandModule.Commands
{
    [Command("Allowguildrecall", "", "")]
    public class AllowGuildRecallCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(IPlayerActor PlayerActor)
        {
            PlayerActor.AllowGuildReCall = !PlayerActor.AllowGuildReCall;
            if (PlayerActor.AllowGuildReCall)
            {
                PlayerActor.SysMsg(CommandHelp.EnableGuildRecall, MsgColor.Green, MsgType.Hint);
            }
            else
            {
                PlayerActor.SysMsg(CommandHelp.DisableGuildRecall, MsgColor.Green, MsgType.Hint);
            }
        }
    }
}
