using System;
using SystemModule.Packet;
using SystemModule.Packet.ClientPackets;

namespace LoginSvr
{
    public struct UserSessionData
    {
        public UserInfo UserInfo;
        public string Msg;
    }

    public class AccountQuick
    {
        public string sAccount;
        public int nIndex;

        public AccountQuick(string account, int index)
        {
            sAccount = account;
            nIndex = index;
        }
    }

    public class RecordHeader
    {
        public string sAccount;
        public string sName;
        public int nSelectID;
        public DateTime dCreateDate;
        public bool boDeleted;
        public DateTime UpdateDate;
        public DateTime CreateDate;
    }

    public class AccountRecord
    {
        public RecordHeader Header;
        public int nErrorCount;
        public double dwActionTick;
        public UserEntry UserEntry;
        public UserEntryAdd UserEntryAdd;
    }
}