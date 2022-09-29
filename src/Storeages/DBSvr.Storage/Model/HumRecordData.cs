using SystemModule.Packet.ClientPackets;
using SystemModule.Packet.ServerPackets;

namespace DBSvr.Storage.Model
{
    public struct HumRecordData
    {
        public int Id;
        public RecordHeader Header;
        public string sChrName;
        public string sAccount;
        public bool Deleted;
        public byte Selected;
    }
}
