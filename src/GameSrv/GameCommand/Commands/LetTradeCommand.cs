using GameSrv.Player;
using SystemModule.Enums;

namespace GameSrv.GameCommand.Commands {
    [Command("Lettrade", "", "")]
    public class LetTradeCommand : GameCommand {
        [ExecuteCommand]
        public void Execute(PlayObject playObject) {
            playObject.AllowDeal = !playObject.AllowDeal;
            if (playObject.AllowDeal) {
                playObject.SysMsg(CommandHelp.EnableDealMsg, MsgColor.Green, MsgType.Hint);
            }
            else {
                playObject.SysMsg(CommandHelp.DisableDealMsg, MsgColor.Green, MsgType.Hint);
            }
        }
    }
}