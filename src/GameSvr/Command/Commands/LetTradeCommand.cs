using GameSvr.Player;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    [GameCommand("Lettrade", "", "", 0)]
    public class LetTradeCommand : BaseCommond
    {
        [DefaultCommand]
        public void Lettrade(PlayObject playObject)
        {
            playObject.AllowDeal = !playObject.AllowDeal;
            if (playObject.AllowDeal)
            {
                playObject.SysMsg(GameCommandConst.EnableDealMsg, MsgColor.Green, MsgType.Hint);
            }
            else
            {
                playObject.SysMsg(GameCommandConst.DisableDealMsg, MsgColor.Green, MsgType.Hint);
            }
        }
    }
}