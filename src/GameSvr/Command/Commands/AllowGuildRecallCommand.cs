using GameSvr.Player;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    [Command("Allowguildrecall", "", "", 0)]
    public class AllowGuildRecallCommand : Commond
    {
        [ExecuteCommand]
        public void AllowGuildRecall(PlayObject playObject)
        {
            playObject.AllowGuildReCall = !playObject.AllowGuildReCall;
            if (playObject.AllowGuildReCall)
            {
                playObject.SysMsg(CommandHelp.EnableGuildRecall, MsgColor.Green, MsgType.Hint);
            }
            else
            {
                playObject.SysMsg(CommandHelp.DisableGuildRecall, MsgColor.Green, MsgType.Hint);
            }
        }
    }
}
