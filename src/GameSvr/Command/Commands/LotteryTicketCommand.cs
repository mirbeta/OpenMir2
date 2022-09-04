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
            PlayObject.SysMsg(string.Format(GameCommandConst.g_sGameCommandLotteryTicketMsg, M2Share.Config.nWinLotteryCount,
                M2Share.Config.nNoWinLotteryCount, M2Share.Config.nWinLotteryLevel1, M2Share.Config.nWinLotteryLevel2,
                M2Share.Config.nWinLotteryLevel3, M2Share.Config.nWinLotteryLevel4, M2Share.Config.nWinLotteryLevel5,
                M2Share.Config.nWinLotteryLevel6), MsgColor.Green, MsgType.Hint);
        }
    }
}