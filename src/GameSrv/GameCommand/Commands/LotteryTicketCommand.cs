using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    [Command("LotteryTicket", "", 10)]
    public class LotteryTicketCommandL : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject playObject) {
            playObject.SysMsg(string.Format(CommandHelp.GameCommandLotteryTicketMsg, GameShare.Config.WinLotteryCount,
                GameShare.Config.NoWinLotteryCount, GameShare.Config.WinLotteryLevel1, GameShare.Config.WinLotteryLevel2,
                GameShare.Config.WinLotteryLevel3, GameShare.Config.WinLotteryLevel4, GameShare.Config.WinLotteryLevel5,
                GameShare.Config.WinLotteryLevel6), MsgColor.Green, MsgType.Hint);
        }
    }
}