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
            writer.Write(sAccount.ToByte(11));
            writer.Write(sPassword.ToByte(11));
            writer.Write(sUserName.ToByte(21));
            writer.Write(sSSNo.ToByte(15));
            writer.Write(sPhone.ToByte(15));
            writer.Write(sQuiz.ToByte(21));
            writer.Write(sAnswer.ToByte(13));
            writer.Write(sEMail.ToByte(41));
        }
    }
}