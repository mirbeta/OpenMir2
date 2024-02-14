using OpenMir2.Data;

namespace DBSrv.Storage.Model
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
