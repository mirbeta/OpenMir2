using GameSvr.Player;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    [GameCommand("Letshout", "", "", 0)]
    public class LetShoutCommand : BaseCommond
    {
        [DefaultCommand]
        public void Letshout(PlayObject playObject)
        {
            playObject.BanShout = !playObject.BanShout;
            if (playObject.BanShout)
            {
                playObject.SysMsg(M2Share.g_sEnableShoutMsg, MsgColor.Green, MsgType.Hint);
            }
            else
            {
                playObject.SysMsg(M2Share.g_sDisableShoutMsg, MsgColor.Green, MsgType.Hint);
            }
        }
    }
}
