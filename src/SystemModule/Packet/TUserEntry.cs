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

        public byte[] GetPacket()
        {
            using var memoryStream = new MemoryStream();
            var backingStream = new BinaryWriter(memoryStream);
            backingStream.Write(sAccount.ToByte(11));
            backingStream.Write(sPassword.ToByte(11));
            backingStream.Write(sUserName.ToByte(21));
            backingStream.Write(sSSNo.ToByte(15));
            backingStream.Write(sPhone.ToByte(15));
            backingStream.Write(sQuiz.ToByte(21));
            backingStream.Write(sAnswer.ToByte(13));
            backingStream.Write(sEMail.ToByte(41));
            var stream = backingStream.BaseStream as MemoryStream;
            return stream?.ToArray();
        }
    }
}