using System;
using SystemModule;
using SystemModule.Packet;

namespace LoginSvr
{
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
    
    public class TRecordHeader
    {
        public string sAccount;
        public string sName;
        public int nSelectID;
        public DateTime dCreateDate;
        public bool boDeleted;
        public DateTime UpdateDate;
        public DateTime CreateDate;
    }
    
    public class TAccountDBRecord
    {
        public TRecordHeader Header;
        public int nErrorCount;
        public double dwActionTick;
        public TUserEntry UserEntry;
        public TUserEntryAdd UserEntryAdd;
    } 
}