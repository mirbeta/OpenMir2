using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    [Command("Allowmsg", "", "", 0)]
    public class AllowMsgCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(PlayObject playObject)
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