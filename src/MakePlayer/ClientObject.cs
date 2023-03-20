namespace MakePlayer
{
    public class ClientObject
    {
        public const int MAXBAGITEMCL = 40;
    }

    public struct TUserCharacterInfo
    {
        public string sName;
        public byte btJob;
        public byte btHair;
        public ushort wLevel;
        public byte btSex;
    }

    public struct TSelChar
    {
        public bool boValid;
        public TUserCharacterInfo UserChr;
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

    public enum TConnectionStep
    {
        cnsConnect,
        cnsNewAccount,
        cnsQueryServer,
        cnsSelServer,
        cnsLogin,
        cnsNewChr,
        cnsQueryChr,
        cnsSelChr,
        cnsReSelChr,
        cnsPlay
    }

    public enum ConnectionStatus
    {
        Success,
        Failure
    }
}

