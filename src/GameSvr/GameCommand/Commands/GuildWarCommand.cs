using GameSvr.Player;

namespace GameSvr.Command.Commands
{
    [Command("GuildWar", "", 10)]
    public class GuildWarCommand : Command
    {
        [ExecuteCommand]
        public void GuildWar(PlayObject PlayObject)
        {
            if (PlayObject.Permission < 6)
            {
                return;
            }
        }
    }
}