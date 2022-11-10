using SystemModule.Data;

namespace DBSvr.Storage.Model
{
    public class PlayerRecordData
    {
        public int Id;
        public RecordHeader Header;
        public string sChrName;
        public string sAccount;
        public bool Deleted;
        public byte Selected;
    }
}
