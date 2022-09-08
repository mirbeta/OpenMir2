using GameSvr.Player;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    [GameCommand("LotteryTicket", "", 10)]
    public class LotteryTicketCommandL : BaseCommond
    {
        [DefaultCommand]
        public void LotteryTicket(PlayObject PlayObject)
        {
            PlayObject.SysMsg(string.Format(GameCommandConst.g_sGameCommandLotteryTicketMsg, M2Share.Config.WinLotteryCount,
                M2Share.Config.NoWinLotteryCount, M2Share.Config.WinLotteryLevel1, M2Share.Config.WinLotteryLevel2,
                M2Share.Config.WinLotteryLevel3, M2Share.Config.WinLotteryLevel4, M2Share.Config.WinLotteryLevel5,
                M2Share.Config.WinLotteryLevel6), MsgColor.Green, MsgType.Hint);
        }
    }
}