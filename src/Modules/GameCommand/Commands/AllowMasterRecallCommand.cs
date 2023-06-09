using SystemModule;
using SystemModule.Enums;

namespace CommandModule.Commands
{
    /// <summary>
    /// 此命令用于允许或禁止师徒传送
    /// </summary>
    [Command("AllowMasterRecall", "此命令用于允许或禁止师徒传送")]
    public class AllowMasterRecallCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(IPlayerActor PlayerActor)
        {
            PlayerActor.CanMasterRecall = !PlayerActor.CanMasterRecall;
            if (PlayerActor.CanMasterRecall)
            {
                PlayerActor.SysMsg(CommandHelp.EnableMasterRecall, MsgColor.Blue, MsgType.Hint);
            }
            else
            {
                PlayerActor.SysMsg(CommandHelp.DisableMasterRecall, MsgColor.Blue, MsgType.Hint);
            }
        }
    }
}