using System.IO;

namespace SystemModule.Packet
{
    public class TUserEntry: Packets
    {
        public string sAccount;
        public string sPassword;
        public string sUserName;
        public string sSSNo;
        public string sPhone;
        public string sQuiz;
        public string sAnswer;
        public string sEMail;
        
        protected override void ReadPacket(BinaryReader reader)
        {
            throw new System.NotImplementedException();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            throw new System.NotImplementedException();
        }
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

        protected override void ReadPacket(BinaryReader reader)
        {
            UserEntry = new TUserEntry();
            UserEntry.sAccount = reader.ReadPascalString(10);
            UserEntry.sPassword = reader.ReadPascalString(10);
            UserEntry.sUserName = reader.ReadPascalString(20);
            UserEntry.sSSNo = reader.ReadPascalString(14);
            UserEntry.sPhone = reader.ReadPascalString(14);
            UserEntry.sQuiz = reader.ReadPascalString(20);
            UserEntry.sAnswer = reader.ReadPascalString(12);
            UserEntry.sEMail = reader.ReadPascalString(40);

            UserEntryAdd = new TUserEntryAdd();
            UserEntryAdd.sQuiz2 = reader.ReadPascalString(20);
            UserEntryAdd.sAnswer2 = reader.ReadPascalString(12);
            UserEntryAdd.sBirthDay = reader.ReadPascalString(10);
            UserEntryAdd.sMobilePhone = reader.ReadPascalString(13);
            UserEntryAdd.sMemo = reader.ReadPascalString(20);
            UserEntryAdd.sMemo2 =reader. ReadPascalString(20);
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