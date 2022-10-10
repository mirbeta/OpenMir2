using GameSvr.Player;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    [Command("Letshout", "", "", 0)]
    public class LetShoutCommand : Commond
    {
        [ExecuteCommand]
        public void Letshout(PlayObject playObject)
        {
            playObject.BanShout = !playObject.BanShout;
            if (playObject.BanShout)
            {
                playObject.SysMsg(CommandHelp.EnableShoutMsg, MsgColor.Green, MsgType.Hint);
            }
            else
            {
                playObject.SysMsg(CommandHelp.DisableShoutMsg, MsgColor.Green, MsgType.Hint);
            }
        }
    }
}
