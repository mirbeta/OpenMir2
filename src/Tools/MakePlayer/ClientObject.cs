namespace MakePlayer
{
    public class ClientObject
    {
        public const int MaxBagItemCL = 40;
    }

    public struct UserCharacterInfo
    {
        public string sName;
        public byte btJob;
        public byte btHair;
        public ushort wLevel;
        public byte btSex;
    }

    public struct SelChar
    {
        public bool boValid;
        public UserCharacterInfo UserChr;
        public bool boSelected;
        public bool boFreezeState;
        public bool boUnfreezing;
        public bool boFreezing;
        public int nAniIndex;
        public int nDarkLevel;
        public int nEffIndex;
        public long dwStartTime;
        public long dwMoretime;
        public long dwStartefftime;
    }

    public enum ConnectionStep : byte
    {
        Connect,
        ConnectFail,
        NewAccount,
        QueryServer,
        SelServer,
        Login,
        NewChr,
        QueryChr,
        SelChr,
        ReSelChr,
        Play
    }

    public enum ConnectionStatus : byte
    {
        Success,
        Connect,
        Failure
    }
}