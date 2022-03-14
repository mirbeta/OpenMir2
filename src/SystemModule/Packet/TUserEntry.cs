using System.IO;

namespace SystemModule.Packet
{
    public class TUserEntry : Packets
    {
        public string sAccount;
        public string sPassword;
        public string sUserName;
        public string sSSNo;
        public string sPhone;
        public string sQuiz;
        public string sAnswer;
        public string sEMail;

        public const int PacketSize = 198;

        public TUserEntry()
        {

        }

        public TUserEntry(byte[] buff)
            : base(buff)
        {
            this.sAccount = ReadPascalString(10);
            this.sPassword = ReadPascalString(10);
            this.sUserName = ReadPascalString(20);
            this.sSSNo = ReadPascalString(14);
            this.sPhone = ReadPascalString(14);
            this.sQuiz = ReadPascalString(20);
            this.sAnswer = ReadPascalString(12);
            this.sEMail = ReadPascalString(40);
        }

        protected override void ReadPacket(BinaryReader reader)
        {
            throw new System.NotImplementedException();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(sAccount, 10);
            writer.Write(sPassword, 10);
            writer.Write(sUserName, 20);
            writer.Write(sSSNo, 14);
            writer.Write(sPhone, 14);
            writer.Write(sQuiz, 20);
            writer.Write(sAnswer, 12);
            writer.Write(sEMail, 40);
        }
    }
}