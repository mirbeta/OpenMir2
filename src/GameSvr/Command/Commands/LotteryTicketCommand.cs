using GameSvr.Player;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    [Command("LotteryTicket", "", 10)]
    public class LotteryTicketCommandL : Commond
    {
        [ExecuteCommand]
        public void LotteryTicket(PlayObject PlayObject)
        {
            PlayObject.SysMsg(string.Format(CommandHelp.GameCommandLotteryTicketMsg, M2Share.Config.WinLotteryCount,
                M2Share.Config.NoWinLotteryCount, M2Share.Config.WinLotteryLevel1, M2Share.Config.WinLotteryLevel2,
                M2Share.Config.WinLotteryLevel3, M2Share.Config.WinLotteryLevel4, M2Share.Config.WinLotteryLevel5,
                M2Share.Config.WinLotteryLevel6), MsgColor.Green, MsgType.Hint);
        }
    }
}