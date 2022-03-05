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

        protected override void ReadPacket(BinaryReader reader)
        {
            throw new System.NotImplementedException();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(sQuiz2.ToByte(21));
            writer.Write(sAnswer2.ToByte(13));
            writer.Write(sBirthDay.ToByte(11));
            writer.Write(sMobilePhone.ToByte(14));
            writer.Write(sMemo.ToByte(21));
            writer.Write(sMemo2.ToByte(21));
        }
    }
}