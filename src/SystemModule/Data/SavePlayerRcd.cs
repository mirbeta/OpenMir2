using SystemModule.Packet.ServerPackets;

namespace SystemModule.Data
{
    public class SavePlayerRcd
    {
        public string sAccount;
        public string sChrName;
        public int nSessionID;
        public object PlayObject;
        public PlayerDataInfo HumanRcd;
        public int nReTryCount;

        public SavePlayerRcd()
        {
            HumanRcd = new PlayerDataInfo();
        }
    }
}