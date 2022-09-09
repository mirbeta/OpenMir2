using System.IO;
using SystemModule.Extensions;

namespace SystemModule.Packet.ClientPackets
{
    public class UserEntry: Packets
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
        public UserEntry UserEntry;
        public UserEntryAdd UserEntryAdd;

        public UserFullEntry()
        {
            UserEntry = new UserEntry();
            UserEntryAdd = new UserEntryAdd();
        }

        protected override void ReadPacket(BinaryReader reader)
        {
            UserEntry = new UserEntry();
            UserEntry.sAccount = reader.ReadPascalString(10);
            UserEntry.sPassword = reader.ReadPascalString(10);
            UserEntry.sUserName = reader.ReadPascalString(20);
            UserEntry.sSSNo = reader.ReadPascalString(14);
            UserEntry.sPhone = reader.ReadPascalString(14);
            UserEntry.sQuiz = reader.ReadPascalString(20);
            UserEntry.sAnswer = reader.ReadPascalString(12);
            UserEntry.sEMail = reader.ReadPascalString(40);

            UserEntryAdd = new UserEntryAdd();
            UserEntryAdd.sQuiz2 = reader.ReadPascalString(20);
            UserEntryAdd.sAnswer2 = reader.ReadPascalString(12);
            UserEntryAdd.sBirthDay = reader.ReadPascalString(10);
            UserEntryAdd.sMobilePhone = reader.ReadPascalString(13);
            UserEntryAdd.sMemo = reader.ReadPascalString(20);
            UserEntryAdd.sMemo2 =reader. ReadPascalString(20);
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.WriteAsciiString(UserEntry.sAccount, 10);
            writer.WriteAsciiString(UserEntry.sPassword, 10);
            writer.WriteAsciiString(UserEntry.sUserName, 20);
            writer.WriteAsciiString(UserEntry.sSSNo, 14);
            writer.WriteAsciiString(UserEntry.sPhone, 14);
            writer.WriteAsciiString(UserEntry.sQuiz, 20);
            writer.WriteAsciiString(UserEntry.sAnswer, 12);
            writer.WriteAsciiString(UserEntry.sEMail, 40);

            writer.WriteAsciiString(UserEntryAdd.sQuiz2, 20);
            writer.WriteAsciiString(UserEntryAdd.sAnswer2, 12);
            writer.WriteAsciiString(UserEntryAdd.sBirthDay, 10);
            writer.WriteAsciiString(UserEntryAdd.sMobilePhone, 13);
            writer.WriteAsciiString(UserEntryAdd.sMemo, 20);
            writer.WriteAsciiString(UserEntryAdd.sMemo2, 20);
        }
    }
}