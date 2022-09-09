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
                playObject.SysMsg(GameCommandConst.EnableGuildRecall, MsgColor.Green, MsgType.Hint);
            }
            else
            {
                playObject.SysMsg(GameCommandConst.DisableGuildRecall, MsgColor.Green, MsgType.Hint);
            }
        }
    }
}
