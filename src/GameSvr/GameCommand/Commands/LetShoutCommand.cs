using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    [Command("Letshout", "", "", 0)]
    public class LetShoutCommand : Command
    {
        [ExecuteCommand]
        public static void Letshout(PlayObject playObject)
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
