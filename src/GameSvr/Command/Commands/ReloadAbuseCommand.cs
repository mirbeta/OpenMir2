using GameSvr.Player;

namespace GameSvr.Command.Commands
{
    [GameCommand("ReloadAbuse", "无用", 10)]
    public class ReloadAbuseCommand : BaseCommond
    {
        [DefaultCommand]
        public void ReloadAbuse(PlayObject PlayObject)
        {

        }
    }
}