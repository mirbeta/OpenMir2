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
            PlayObject.SysMsg(string.Format(GameCommandConst.g_sGameCommandLotteryTicketMsg, M2Share.g_Config.nWinLotteryCount,
                M2Share.g_Config.nNoWinLotteryCount, M2Share.g_Config.nWinLotteryLevel1, M2Share.g_Config.nWinLotteryLevel2,
                M2Share.g_Config.nWinLotteryLevel3, M2Share.g_Config.nWinLotteryLevel4, M2Share.g_Config.nWinLotteryLevel5,
                M2Share.g_Config.nWinLotteryLevel6), MsgColor.Green, MsgType.Hint);
        }
    }
}