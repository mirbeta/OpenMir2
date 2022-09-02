using GameSvr.Player;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    [GameCommand("StartQuest", "", "问答名称", 10)]
    public class StartQuestCommand : BaseCommond
    {
        [DefaultCommand]
        public void StartQuest(string[] @params, PlayObject PlayObject)
        {
            if (@params == null)
            {
                return;
            }
            var sQuestName = @params.Length > 0 ? @params[0] : "";
            if (sQuestName == "")
            {
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            M2Share.UserEngine.SendQuestMsg(sQuestName);
        }
    }
}