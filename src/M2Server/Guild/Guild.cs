using SystemModule;

namespace M2Server.Guild
{
    public record struct WarGuild
    {
        public IGuild Guild;
        public int dwWarTick;
        public int dwWarTime;
    }
}

