using System.IO;

namespace SystemModule.Packet.ClientPackets
{
    public class UserEntryAdd : Packets
    {
        public string sQuiz2;
        public string sAnswer2;
        public string sBirthDay;
        public string sMobilePhone;
        public string sMemo;
        public string sMemo2;

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