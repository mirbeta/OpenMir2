using System.IO;

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
            throw new System.NotImplementedException();
        }
    }
}