using System;

namespace M2Server
{
    public struct THumInfo
    {
        public bool boDeleted;
        public bool boSelected;
        public string[] sAccount;
        public DateTime dModDate;
        public string[] sChrName;
        public byte btCount;
        public TRecordHeader Header;
    }
}