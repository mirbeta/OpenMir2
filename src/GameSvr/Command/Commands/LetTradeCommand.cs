using GameSvr.Player;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    [Command("Lettrade", "", "", 0)]
    public class LetTradeCommand : Commond
    {
        [ExecuteCommand]
        public void Lettrade(PlayObject playObject)
        {
            playObject.AllowDeal = !playObject.AllowDeal;
            if (playObject.AllowDeal)
            {
                playObject.SysMsg(CommandHelp.EnableDealMsg, MsgColor.Green, MsgType.Hint);
            }
            else
            {
                playObject.SysMsg(CommandHelp.DisableDealMsg, MsgColor.Green, MsgType.Hint);
            }
        }
    }
}