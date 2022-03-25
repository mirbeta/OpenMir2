using ProtoBuf;

namespace SystemModule
{
    [ProtoContract]
    public class THumDataInfo
    {
        [ProtoMember(1)] public TRecordHeader Header { get; set; }
        [ProtoMember(2)] public THumInfoData Data { get; set; }

        public THumDataInfo()
        {
            Header = new TRecordHeader();
            Data = new THumInfoData();
        }
    }

    [ProtoContract]
    public class SaveHumDataPacket : CmdPacket
    {
        [ProtoMember(1)]
        public string sAccount { get; set; }
        [ProtoMember(2)]
        public string sCharName { get; set; }
        [ProtoMember(3)]
        public THumDataInfo HumDataInfo { get; set; }
    }
    
    [ProtoContract]
    public class LoadHumDataPacket : CmdPacket
    {
        [ProtoMember(1)] 
        public string sAccount { get; set; }
        [ProtoMember(2)] 
        public string sChrName { get; set; }
        [ProtoMember(3)] 
        public string sUserAddr { get; set; }
        [ProtoMember(4)] 
        public int nSessionID { get; set; }
    }
}