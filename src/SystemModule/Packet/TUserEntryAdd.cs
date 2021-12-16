using System.IO;

namespace SystemModule.Packet
{
    public class TUserEntryAdd: Packets
    {
        public string sQuiz2;
        public string sAnswer2;
        public string sBirthDay;
        public string sMobilePhone;
        public string sMemo;
        public string sMemo2;

        public TUserEntryAdd()
        {
            
        }

        public TUserEntryAdd(byte[] buff)
            : base(buff)
        {
            this.sQuiz2 = ReadPascalString(20); 
            this.sAnswer2 = ReadPascalString(12); 
            this.sBirthDay = ReadPascalString(10);
            this.sMobilePhone = ReadPascalString(13);
            this.sMemo = ReadPascalString(20);
            this.sMemo2 = ReadPascalString(20);
        }
        
        public byte[] GetPacket()
        {
            using var memoryStream = new MemoryStream();
            var backingStream = new BinaryWriter(memoryStream);
            backingStream.Write(sQuiz2.ToByte(21));
            backingStream.Write(sAnswer2.ToByte(13));
            backingStream.Write(sBirthDay.ToByte(11));
            backingStream.Write(sMobilePhone.ToByte(14));
            backingStream.Write(sMemo.ToByte(21));
            backingStream.Write(sMemo2.ToByte(21));
            var stream = backingStream.BaseStream as MemoryStream;
            return stream?.ToArray();
        }
    }
}