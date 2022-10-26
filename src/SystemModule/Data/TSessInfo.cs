namespace SystemModule.Data
{
    public class TSessInfo
    {
        public int nSessionID;
        public string sAccount;
        public string sIPaddr;
        public int PayMent;
        public int PayMode;
        /// <summary>
        /// 账号剩余游戏时间
        /// </summary>
        public long PlayTime;
        public int SessionStatus;
        public int dwStartTick;
        public int ActiveTick;
        public int nRefCount;
        public int nSocket;
        public int nGateIdx;
        public int nGSocketIdx;
        public int dwNewUserTick;
        public int nSoftVersionDate;
    }
}