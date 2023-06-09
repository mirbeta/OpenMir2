using SystemModule;

namespace CommandModule.Commands
{
    /// <summary>
    /// 此命令允许公会取消联盟
    /// </summary>
    [Command("AuthCancel", "")]
    public class AuthCancelCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(string[] @params, IPlayerActor PlayerActor)
        {
            if (@params == null || @params.Length <= 0)
            {
                return;
            }
            if (PlayerActor.IsGuildMaster())
            {
                PlayerActor.ClientGuildBreakAlly(@params[0]);
            }
        }
    }
}