namespace SystemModule.Data
{
    public class LoadDBInfo
    {
        public int nGateIdx;
        public int nSocket;
        public string Account;
        public string ChrName;
        public int nSessionID;
        public string sIPaddr;
        public int nSoftVersionDate;
        public int nPayMent;
        public int nPayMode;
        /// <summary>
        /// 账号剩余游戏时间
        /// </summary>
        public long PlayTime;
        public ushort nGSocketIdx;
        public int dwNewUserTick;
        public object PlayObject;
        public int nReLoadCount;
    }
}