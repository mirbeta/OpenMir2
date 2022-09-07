namespace GameGate.Auth
{
    public class SetupCode
    {
        public string Account { get; internal set; }
        public string AccountSecretKey { get; internal set; }
        public string ManualEntryKey { get; internal set; }
    }
}