using SystemModule;
using SystemModule.Enums;

namespace CommandModule.Commands
{
    [Command("LotteryTicket", "", 10)]
    public class LotteryTicketCommandL : GameCommand
    {
        [ExecuteCommand]
        public void Execute(IPlayerActor PlayerActor)
        {
            PlayerActor.SysMsg(string.Format(CommandHelp.GameCommandLotteryTicketMsg, ModuleShare.Config.WinLotteryCount,
                ModuleShare.Config.NoWinLotteryCount, ModuleShare.Config.WinLotteryLevel1, ModuleShare.Config.WinLotteryLevel2,
                ModuleShare.Config.WinLotteryLevel3, ModuleShare.Config.WinLotteryLevel4, ModuleShare.Config.WinLotteryLevel5,
                ModuleShare.Config.WinLotteryLevel6), MsgColor.Green, MsgType.Hint);
        }
    }
}