using SystemModule;
using SystemModule.Actors;
using SystemModule.Enums;

namespace CommandModule.Commands
{
    [Command("LotteryTicket", "", 10)]
    public class LotteryTicketCommandL : GameCommand
    {
        [ExecuteCommand]
        public void Execute(IPlayerActor PlayerActor)
        {
            PlayerActor.SysMsg(string.Format(CommandHelp.GameCommandLotteryTicketMsg, SystemShare.Config.WinLotteryCount,
                SystemShare.Config.NoWinLotteryCount, SystemShare.Config.WinLotteryLevel1, SystemShare.Config.WinLotteryLevel2,
                SystemShare.Config.WinLotteryLevel3, SystemShare.Config.WinLotteryLevel4, SystemShare.Config.WinLotteryLevel5,
                SystemShare.Config.WinLotteryLevel6), MsgColor.Green, MsgType.Hint);
        }
    }
}