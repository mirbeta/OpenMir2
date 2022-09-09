using GameSvr.Player;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    [GameCommand("Allowmsg", "", "", 0)]
    public class AllowMsgCommand : BaseCommond
    {
        [DefaultCommand]
        public void AllowMsg(PlayObject playObject)
        {
            playObject.HearWhisper = !playObject.HearWhisper;
            if (playObject.HearWhisper)
            {
                playObject.SysMsg(M2Share.g_sEnableHearWhisper, MsgColor.Green, MsgType.Hint);
            }
            else
            {
                playObject.SysMsg(M2Share.g_sDisableHearWhisper, MsgColor.Green, MsgType.Hint);
            }
        }
    }
}