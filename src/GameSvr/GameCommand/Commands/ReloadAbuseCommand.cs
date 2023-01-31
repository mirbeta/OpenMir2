using GameSvr.Player;

namespace GameSvr.GameCommand.Commands
{
    [Command("ReloadAbuse", "无用", 10)]
    public class ReloadAbuseCommand : Command
    {
        [ExecuteCommand]
        public static void ReloadAbuse(PlayObject PlayObject)
        {

        }
    }
}