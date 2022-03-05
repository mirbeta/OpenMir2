using System.IO;

namespace SystemModule
{
    public class THumDataInfo : Packets
    {
        public TRecordHeader Header;
        public THumInfoData Data;

        public THumDataInfo()
        {
            Header = new TRecordHeader();
            Data = new THumInfoData();
        }

        public THumDataInfo(byte[] buff)
            : base(buff)
        {
            var hederBuff = ReadBytes(67);
            Header = new TRecordHeader(hederBuff);

            var bodyBuff = ReadBytes(3433);
            Data = new THumInfoData(bodyBuff);
        }

        protected override void ReadPacket(BinaryReader reader)
        {
            throw new System.NotImplementedException();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(Header.GetPacket());
            writer.Write(Data.GetPacket());
        }
    }
}