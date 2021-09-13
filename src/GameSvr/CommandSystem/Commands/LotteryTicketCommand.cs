using SystemModule;
using System;
using GameSvr.CommandSystem;

namespace GameSvr
{
    [GameCommand("LotteryTicket", "", 10)]
    public class LotteryTicketCommandL : BaseCommond
    {
        [DefaultCommand]
        public void LotteryTicket(string[] @Params, TPlayObject PlayObject)
        {
            var sParam1 = @Params.Length > 0 ? @Params[0] : "";
            if (sParam1 == "" || sParam1 != "" && sParam1[1] == '?')
            {
                PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandParamUnKnow, this.Attributes.Name, ""), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            PlayObject.SysMsg(string.Format(M2Share.g_sGameCommandLotteryTicketMsg, M2Share.g_Config.nWinLotteryCount,
                M2Share.g_Config.nNoWinLotteryCount, M2Share.g_Config.nWinLotteryLevel1, M2Share.g_Config.nWinLotteryLevel2,
                M2Share.g_Config.nWinLotteryLevel3, M2Share.g_Config.nWinLotteryLevel4, M2Share.g_Config.nWinLotteryLevel5,
                M2Share.g_Config.nWinLotteryLevel6), TMsgColor.c_Green, TMsgType.t_Hint);
        }
    }
}