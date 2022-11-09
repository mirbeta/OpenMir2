using System.IO;
using SystemModule.Extensions;

namespace SystemModule.Packet.ClientPackets
{
    public class UserEntryAdd : Packets
    {
        public string Quiz2;
        public string Answer2;
        public string BirthDay;
        public string MobilePhone;
        public string Memo;
        public string Memo2;

        protected override void ReadPacket(BinaryReader reader)
        {
            throw new System.NotImplementedException();
        }

        protected override void WritePacket(BinaryWriter writer)
        {
            writer.WriteAsciiString(Quiz2, 20);
            writer.WriteAsciiString(Answer2, 12);
            writer.WriteAsciiString(BirthDay, 10);
            writer.WriteAsciiString(MobilePhone, 13);
            writer.WriteAsciiString(Memo, 20);
            writer.WriteAsciiString(Memo2, 20);
        }
    }
}