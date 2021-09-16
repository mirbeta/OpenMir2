using System;
using SystemModule;

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

    public class TUserEntry : Package
    {
        public string sAccount;
        public string sPassword;
        public string sUserName;
        public string sSSNo;
        public string sPhone;
        public string sQuiz;
        public string sAnswer;
        public string sEMail;

        public TUserEntry()
        {
            
        }

        public TUserEntry(byte[] buff)
            : base(buff)
        {
            this.sAccount = ReadPascalString(10); 
            this.sPassword = ReadPascalString(10); 
            this.sUserName = ReadPascalString(20);
            this.sSSNo = ReadPascalString(14);
            this.sPhone = ReadPascalString(14);
            this.sQuiz = ReadPascalString(20);
            this.sAnswer = ReadPascalString(12);
            this.sEMail = ReadPascalString(40); 
        }
    }

    public class TUserEntryAdd: Package
    {
        public string sQuiz2;
        public string sAnswer2;
        public string sBirthDay;
        public string sMobilePhone;
        public string sMemo;
        public string sMemo2;

        public TUserEntryAdd()
        {
            
        }

        public TUserEntryAdd(byte[] buff)
            : base(buff)
        {
            this.sQuiz2 = ReadPascalString(20); 
            this.sAnswer2 = ReadPascalString(12); 
            this.sBirthDay = ReadPascalString(10);
            this.sMobilePhone = ReadPascalString(13);
            this.sMemo = ReadPascalString(20);
            this.sMemo2 = ReadPascalString(20);
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