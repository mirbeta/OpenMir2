using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    [Command("LotteryTicket", "", 10)]
    public class LotteryTicketCommandL : Command
    {
        [ExecuteCommand]
        public static void LotteryTicket(PlayObject PlayObject)
        {
            PlayObject.SysMsg(string.Format(CommandHelp.GameCommandLotteryTicketMsg, M2Share.Config.WinLotteryCount,
                M2Share.Config.NoWinLotteryCount, M2Share.Config.WinLotteryLevel1, M2Share.Config.WinLotteryLevel2,
                M2Share.Config.WinLotteryLevel3, M2Share.Config.WinLotteryLevel4, M2Share.Config.WinLotteryLevel5,
                M2Share.Config.WinLotteryLevel6), MsgColor.Green, MsgType.Hint);
        }
    }
}