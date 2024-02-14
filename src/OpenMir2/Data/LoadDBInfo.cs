namespace OpenMir2.Data
{
    public struct LoadDBInfo
    {
        public int GateIdx;
        public int SocketId;
        public string Account;
        public string ChrName;
        public int SessionID;
        public string sIPaddr;
        public int SoftVersionDate;
        public int PayMent;
        public int PayMode;
        /// <summary>
        /// 账号剩余游戏时间
        /// </summary>
        public long PlayTime;
        public ushort GSocketIdx;
        public int NewUserTick;
    }
}