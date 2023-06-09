using SystemModule;
using SystemModule.Enums;

namespace CommandModule.Commands
{
    /// <summary>
    /// 将指定人物召唤到身边(支持权限分配)
    /// </summary>
    [Command("RecallHuman", "将指定人物召唤到身边(支持权限分配)", CommandHelp.GameCommandPrvMsgHelpMsg, 10)]
    public class RecallHumanCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (@params == null)
            {
                return;
            }
            var sHumanName = @params.Length > 0 ? @params[0] : "";
            if (string.IsNullOrEmpty(sHumanName) || !string.IsNullOrEmpty(sHumanName) && sHumanName[1] == '?')
            {
                PlayerActor.SysMsg(Command.CommandHelp, MsgColor.Red, MsgType.Hint);
                return;
            }
            PlayerActor.RecallHuman(sHumanName);
        }
    }
}