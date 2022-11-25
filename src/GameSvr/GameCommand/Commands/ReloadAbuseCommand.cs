using GameSvr.Player;

namespace GameSvr.GameCommand.Commands
{
    [Command("ReloadAbuse", "无用", 10)]
    public class ReloadAbuseCommand : Command
    {
        [ExecuteCommand]
        public void ReloadAbuse(PlayObject PlayObject)
        {

        }
    }
}