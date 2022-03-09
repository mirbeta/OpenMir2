using System.IO;

namespace SystemModule.Packet
{
    public class TUserEntryAdd : Packets
    {
        public string sQuiz2;
        public string sAnswer2;
        public string sBirthDay;
        public string sMobilePhone;
        public string sMemo;
        public string sMemo2;

        public const int PacketSize = 135;

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
            writer.Write(sQuiz2.ToByte(sQuiz2.Length + 1, 21));
            writer.Write(sAnswer2.ToByte(sAnswer2.Length + 1, 13));
            writer.Write(sBirthDay.ToByte(sBirthDay.Length + 1, 11));
            writer.Write(sMobilePhone.ToByte(sMobilePhone.Length + 1, 14));
            writer.Write(sMemo.ToByte(sMemo.Length + 1, 21));
            writer.Write(sMemo2.ToByte(sMemo2.Length + 1, 21));
        }
    }
}