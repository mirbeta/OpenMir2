using GameSvr.Player;

namespace GameSvr.GameCommand.Commands
{
    [Command("GuildWar", "", 10)]
    public class GuildWarCommand : Command
    {
        [ExecuteCommand]
        public static void GuildWar(PlayObject PlayObject)
        {
            if (PlayObject.Permission < 6)
            {
                return;
            }
        }
    }
}