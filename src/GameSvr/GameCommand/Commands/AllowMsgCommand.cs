using GameSvr.Player;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    [Command("Allowmsg", "", "", 0)]
    public class AllowMsgCommand : Command
    {
        [ExecuteCommand]
        public void AllowMsg(PlayObject playObject)
        {
            playObject.HearWhisper = !playObject.HearWhisper;
            if (playObject.HearWhisper)
            {
                playObject.SysMsg(CommandHelp.EnableHearWhisper, MsgColor.Green, MsgType.Hint);
            }
            else
            {
                playObject.SysMsg(CommandHelp.DisableHearWhisper, MsgColor.Green, MsgType.Hint);
            }
        }
    }
}