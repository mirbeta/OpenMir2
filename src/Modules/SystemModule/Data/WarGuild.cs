using SystemModule.Castles;

namespace SystemModule.Data
{
    public record struct WarGuild
    {
        public IGuild Guild;
        public int WarTick;
        public int WarTime;
    }
}
