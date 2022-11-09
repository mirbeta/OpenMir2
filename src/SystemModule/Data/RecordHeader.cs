using ProtoBuf;

namespace SystemModule.Data
{
    [ProtoContract]
    public class RecordHeader
    {
        [ProtoMember(1)]
        public string sAccount;
        [ProtoMember(2)]
        public string Name;
        [ProtoMember(3)]
        public int SelectID;
        [ProtoMember(4)]
        public double dCreateDate;
        [ProtoMember(5)]
        public bool Deleted;
        [ProtoMember(6)]
        public double UpdateDate;
        [ProtoMember(7)]
        public double CreateDate;
    }
}
