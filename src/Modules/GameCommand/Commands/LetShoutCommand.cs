using SystemModule;
using SystemModule.Enums;

namespace CommandModule.Commands
{
    [Command("Letshout", "", "")]
    public class LetShoutCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(IPlayerActor PlayerActor)
        {
            PlayerActor.BanShout = !PlayerActor.BanShout;
            if (PlayerActor.BanShout)
            {
                PlayerActor.SysMsg(CommandHelp.EnableShoutMsg, MsgColor.Green, MsgType.Hint);
            }
            else
            {
                PlayerActor.SysMsg(CommandHelp.DisableShoutMsg, MsgColor.Green, MsgType.Hint);
            }
        }
    }
}
