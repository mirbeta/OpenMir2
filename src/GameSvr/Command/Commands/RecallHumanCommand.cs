using GameSvr.CommandSystem;
using SystemModule;

namespace GameSvr
{
    /// <summary>
    /// 将指定人物召唤到身边(支持权限分配)
    /// </summary>
    [GameCommand("RecallHuman", "将指定人物召唤到身边(支持权限分配)", M2Share.g_sGameCommandPrvMsgHelpMsg, 10)]
    public class RecallHumanCommand : BaseCommond
    {
        [DefaultCommand]
        public void RecallHuman(string[] @Params, TPlayObject PlayObject)
        {
            if (@Params == null)
            {
                return;
            }
            var sHumanName = @Params.Length > 0 ? @Params[0] : "";
            if (string.IsNullOrEmpty(sHumanName) || !string.IsNullOrEmpty(sHumanName) && sHumanName[1] == '?')
            {
                PlayObject.SysMsg(GameCommand.ShowHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            PlayObject.RecallHuman(sHumanName);
        }
    }
}