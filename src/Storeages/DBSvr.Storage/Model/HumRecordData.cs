using SystemModule.Data;
using SystemModule.Packet.ServerPackets;

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
