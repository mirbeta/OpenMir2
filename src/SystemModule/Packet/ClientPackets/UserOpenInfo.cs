using SystemModule.Data;
using SystemModule.Packet.ServerPackets;

namespace SystemModule.Packet.ClientPackets
{
    public class UserOpenInfo
    {
        public string sChrName;
        public TLoadDBInfo LoadUser;
        public THumDataInfo HumanRcd;
    }
}