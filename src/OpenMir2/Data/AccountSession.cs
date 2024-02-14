namespace OpenMir2.Data
{
    public class AccountSession
    {
        public int SessionId;
        public string Account;
        public string IPaddr;
        public int PayMent;
        public int PayMode;
        /// <summary>
        /// 账号剩余游戏时间
        /// </summary>
        public long PlayTime;
        public int SessionStatus;
        public int StartTick;
        public int ActiveTick;
        public int RefCount;
        public int Socket;
        public int GateIdx;
        public int SocketIdx;
        public int NewUserTick;
        public int SoftVersionDate;
    }
}