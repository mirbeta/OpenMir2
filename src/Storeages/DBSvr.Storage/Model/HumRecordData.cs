using SystemModule.Packet.ClientPackets;

namespace DBSvr.Storage.Model
{
    public struct HumRecordData
    {
        public int Id;
        public TRecordHeader Header;
        public string sChrName;
        public string sAccount;
        public bool boDeleted;
        public byte boSelected;
    }
}
