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

        public byte[] GetPacket()
        {
            using (var memoryStream = new MemoryStream())
            {
                var backingStream = new BinaryWriter(memoryStream);

                backingStream.Write(Header.GetPacket());
                backingStream.Write(Data.GetPacket());

                var stream = backingStream.BaseStream as MemoryStream;
                return stream.ToArray();
            }
        }
    }
}