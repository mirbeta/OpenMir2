using SystemModule;
using GameSvr.CommandSystem;

namespace GameSvr
{
    [GameCommand("StartQuest", "", 10)]
    public class StartQuestCommand : BaseCommond
    {
        [DefaultCommand]
        public void StartQuest(string[] @params, TPlayObject PlayObject)
        {
            var sQuestName = @params.Length > 0 ? @params[0] : "";
            if (sQuestName == "")
            {
                PlayObject.SysMsg("命令格式: @" + this.Attributes.Name + " 问答名称", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            M2Share.UserEngine.SendQuestMsg(sQuestName);
        }
    }
}