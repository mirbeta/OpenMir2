using GameSvr.Player;
using SystemModule.Data;

namespace GameSvr.Command.Commands
{
    /// <summary>
    /// 将指定人物召唤到身边(支持权限分配)
    /// </summary>
    [Command("RecallHuman", "将指定人物召唤到身边(支持权限分配)", CommandHelp.GameCommandPrvMsgHelpMsg, 10)]
    public class RecallHumanCommand : Commond
    {
        [ExecuteCommand]
        public void RecallHuman(string[] @Params, PlayObject PlayObject)
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