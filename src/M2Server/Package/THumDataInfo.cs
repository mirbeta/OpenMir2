using System.IO;

namespace M2Server
{
    public class THumDataInfo : Package
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

        public byte[] ToByte()
        {
            using (var memoryStream = new MemoryStream())
            {
                var backingStream = new BinaryWriter(memoryStream);

                backingStream.Write(Header.ToByte());
                backingStream.Write(Data.ToByte());

                var stream = backingStream.BaseStream as MemoryStream;
                return stream.ToArray();
            }
        }
    }
}

