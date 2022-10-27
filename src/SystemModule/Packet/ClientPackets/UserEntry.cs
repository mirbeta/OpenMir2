using System.IO;
using SystemModule.Extensions;

namespace SystemModule.Packet.ClientPackets
{
    public class UserEntry: Packets
    {
        public string Account;
        public string Password;
        public string UserName;
        public string SSNo;
        public string Phone;
        public string Quiz;
        public string Answer;
        public string EMail;

        public const byte Size = 140 + 8;
        
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
            UserEntry.Account = reader.ReadPascalString(10);
            UserEntry.Password = reader.ReadPascalString(10);
            UserEntry.UserName = reader.ReadPascalString(20);
            UserEntry.SSNo = reader.ReadPascalString(14);
            UserEntry.Phone = reader.ReadPascalString(14);
            UserEntry.Quiz = reader.ReadPascalString(20);
            UserEntry.Answer = reader.ReadPascalString(12);
            UserEntry.EMail = reader.ReadPascalString(40);
            UserEntryAdd = new UserEntryAdd();
            UserEntryAdd.Quiz2 = reader.ReadPascalString(20);
            UserEntryAdd.Answer2 = reader.ReadPascalString(12);
            UserEntryAdd.BirthDay = reader.ReadPascalString(10);
            UserEntryAdd.MobilePhone = reader.ReadPascalString(13);
            UserEntryAdd.Memo = reader.ReadPascalString(20);
            UserEntryAdd.Memo2 =reader. ReadPascalString(20);
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.WriteAsciiString(UserEntry.Account, 10);
            writer.WriteAsciiString(UserEntry.Password, 10);
            writer.WriteAsciiString(UserEntry.UserName, 20);
            writer.WriteAsciiString(UserEntry.SSNo, 14);
            writer.WriteAsciiString(UserEntry.Phone, 14);
            writer.WriteAsciiString(UserEntry.Quiz, 20);
            writer.WriteAsciiString(UserEntry.Answer, 12);
            writer.WriteAsciiString(UserEntry.EMail, 40);
            writer.WriteAsciiString(UserEntryAdd.Quiz2, 20);
            writer.WriteAsciiString(UserEntryAdd.Answer2, 12);
            writer.WriteAsciiString(UserEntryAdd.BirthDay, 10);
            writer.WriteAsciiString(UserEntryAdd.MobilePhone, 13);
            writer.WriteAsciiString(UserEntryAdd.Memo, 20);
            writer.WriteAsciiString(UserEntryAdd.Memo2, 20);
        }
    }
}