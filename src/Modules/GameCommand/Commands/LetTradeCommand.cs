using SystemModule;
using SystemModule.Enums;

namespace CommandSystem
{
    [Command("Lettrade", "", "")]
    public class LetTradeCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(IPlayerActor PlayerActor)
        {
            PlayerActor.SysMsgAllowDeal = !PlayerActor.SysMsgAllowDeal;
            if (PlayerActor.SysMsgAllowDeal)
            {
                PlayerActor.SysMsg(CommandHelp.EnableDealMsg, MsgColor.Green, MsgType.Hint);
            }
            else
            {
                PlayerActor.SysMsg(CommandHelp.DisableDealMsg, MsgColor.Green, MsgType.Hint);
            }
        }
    }
}