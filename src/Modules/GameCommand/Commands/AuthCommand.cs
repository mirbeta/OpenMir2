using SystemModule;

namespace CommandSystem.Commands
{
    /// <summary>
    /// 此命令允许或禁止公会联盟
    /// </summary>
    [Command("Auth", "")]
    public class AuthCommand : GameCommand
    {
        [ExecuteCommand]
        public void Execute(IPlayerActor PlayerActor)
        {
            if (PlayerActor.IsGuildMaster())
            {
                PlayerActor.ClientGuildAlly();
            }
        }
    }
}