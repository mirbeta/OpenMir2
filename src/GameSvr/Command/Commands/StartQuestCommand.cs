using GameSvr.CommandSystem;
using SystemModule;

namespace GameSvr
{
    [GameCommand("StartQuest", "", "问答名称", 10)]
    public class StartQuestCommand : BaseCommond
    {
        [DefaultCommand]
        public void StartQuest(string[] @params, TPlayObject PlayObject)
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