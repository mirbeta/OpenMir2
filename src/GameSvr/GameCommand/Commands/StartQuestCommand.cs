using GameSvr.Player;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.GameCommand.Commands
{
    [Command("StartQuest", "", "问答名称", 10)]
    public class StartQuestCommand : GameCommand
    {
        [ExecuteCommand]
        public void StartQuest(string[] @params, PlayObject PlayObject)
        {
            if (@params == null)
            {
                return;
            }
            string sQuestName = @params.Length > 0 ? @params[0] : "";
            if (sQuestName == "")
            {
                PlayObject.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            M2Share.WorldEngine.SendQuestMsg(sQuestName);
        }
    }
}