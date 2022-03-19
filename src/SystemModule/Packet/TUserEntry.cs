using System.IO;

namespace SystemModule.Packet
{
    public class TUserEntry
    {
        public string sAccount;
        public string sPassword;
        public string sUserName;
        public string sSSNo;
        public string sPhone;
        public string sQuiz;
        public string sAnswer;
        public string sEMail;
    }

    public class UserFullEntry : Packets
    {
        public TUserEntry UserEntry;
        public TUserEntryAdd UserEntryAdd;

        public UserFullEntry()
        {
            UserEntry = new TUserEntry();
            UserEntryAdd = new TUserEntryAdd();
        }

        public UserFullEntry(byte[] buff) : base(buff)
        {
            UserEntry = new TUserEntry();
            UserEntry.sAccount = ReadPascalString(10);
            UserEntry.sPassword = ReadPascalString(10);
            UserEntry.sUserName = ReadPascalString(20);
            UserEntry.sSSNo = ReadPascalString(14);
            UserEntry.sPhone = ReadPascalString(14);
            UserEntry.sQuiz = ReadPascalString(20);
            UserEntry.sAnswer = ReadPascalString(12);
            UserEntry.sEMail = ReadPascalString(40);

            UserEntryAdd = new TUserEntryAdd();
            UserEntryAdd.sQuiz2 = ReadPascalString(20);
            UserEntryAdd.sAnswer2 = ReadPascalString(12);
            UserEntryAdd.sBirthDay = ReadPascalString(10);
            UserEntryAdd.sMobilePhone = ReadPascalString(13);
            UserEntryAdd.sMemo = ReadPascalString(20);
            UserEntryAdd.sMemo2 = ReadPascalString(20);
        }

        protected override void ReadPacket(BinaryReader reader)
        {
            throw new System.NotImplementedException();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.Write(UserEntry.sAccount, 10);
            writer.Write(UserEntry.sPassword, 10);
            writer.Write(UserEntry.sUserName, 20);
            writer.Write(UserEntry.sSSNo, 14);
            writer.Write(UserEntry.sPhone, 14);
            writer.Write(UserEntry.sQuiz, 20);
            writer.Write(UserEntry.sAnswer, 12);
            writer.Write(UserEntry.sEMail, 40);

            writer.Write(UserEntryAdd.sQuiz2, 20);
            writer.Write(UserEntryAdd.sAnswer2, 12);
            writer.Write(UserEntryAdd.sBirthDay, 10);
            writer.Write(UserEntryAdd.sMobilePhone, 13);
            writer.Write(UserEntryAdd.sMemo, 20);
            writer.Write(UserEntryAdd.sMemo2, 20);
        }
    }
}