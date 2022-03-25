using ProtoBuf;

namespace SystemModule
{
    [ProtoContract]
    public class TRecordHeader
    {
        [ProtoMember(1)]
        public string sAccount;
        [ProtoMember(2)]
        public string sName;
        [ProtoMember(3)]
        public int nSelectID;
        [ProtoMember(4)]
        public double dCreateDate;
        [ProtoMember(5)]
        public bool boDeleted;
        [ProtoMember(6)]
        public double UpdateDate;
        [ProtoMember(7)]
        public double CreateDate;
    }
}
