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
            writer.Write(sAccount.ToByte(sAccount.Length + 1, 11));
            writer.Write(sPassword.ToByte(sPassword.Length + 1, 11));
            writer.Write(sUserName.ToByte(sUserName.Length + 1,21));
            writer.Write(sSSNo.ToByte(sSSNo.Length + 1, 15));
            writer.Write(sPhone.ToByte(sPhone.Length + 1, 15));
            writer.Write(sQuiz.ToByte(sQuiz.Length + 1, 21));
            writer.Write(sAnswer.ToByte(sAnswer.Length + 1, 13));
            writer.Write(sEMail.ToByte(sEMail.Length + 1, 41));
        }
    }
}