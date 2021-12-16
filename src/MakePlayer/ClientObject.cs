using System;

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
        // TRUE:倔篮惑怕 FALSE:踌篮惑怕
        public bool boUnfreezing;
        // 踌绊 乐绰 惑怕牢啊?
        public bool boFreezing;
        // 倔绊 乐绰 惑怕?
        public int nAniIndex;
        // 踌绰(绢绰) 局聪皋捞记
        public int nDarkLevel;
        public int nEffIndex;
        // 瓤苞 局聪皋捞记
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

    public enum TConnectionStatus
    {
        cns_Success,
        cns_Failure
    } 
}

