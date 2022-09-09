using GameSvr.Player;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    [GameCommand("Allowguildrecall", "", "", 0)]
    public class AllowGuildRecallCommand : BaseCommond
    {
        [DefaultCommand]
        public void AllowGuildRecall(PlayObject playObject)
        {
            playObject.AllowGuildReCall = !playObject.AllowGuildReCall;
            if (playObject.AllowGuildReCall)
            {
                playObject.SysMsg(M2Share.g_sEnableGuildRecall, MsgColor.Green, MsgType.Hint);
            }
            else
            {
                playObject.SysMsg(M2Share.g_sDisableGuildRecall, MsgColor.Green, MsgType.Hint);
            }
            return;
        }
    }
}
