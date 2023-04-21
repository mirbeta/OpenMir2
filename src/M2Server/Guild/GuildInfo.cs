namespace M2Server
{
    public interface GuildInfo
    {
        int Stability { get; set; }
        int ChiefItemCount { get; set; }
        int Flourishing { get; set; }
        int BuildPoint { get; set; }
        int Aurae { get; set; }
        string GuildName { get; set; }

        string GetChiefName();
    }
}