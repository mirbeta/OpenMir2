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
                playObject.SysMsg(M2Share.g_sEnableDealMsg, MsgColor.Green, MsgType.Hint);
            }
            else
            {
                playObject.SysMsg(M2Share.g_sDisableDealMsg, MsgColor.Green, MsgType.Hint);
            }
            return;
        }
    }
}