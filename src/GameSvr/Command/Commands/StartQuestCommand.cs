using SystemModule;
using GameSvr.CommandSystem;

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
                PlayObject.SysMsg(CommandAttribute.CommandHelp(), TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            M2Share.UserEngine.SendQuestMsg(sQuestName);
        }
    }
}