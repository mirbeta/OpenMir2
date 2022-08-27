namespace SystemModule.Packet.ClientPackets
{
    public class TSaveRcd
    {
        public string sAccount;
        public string sChrName;
        public int nSessionID;
        public object PlayObject;
        public THumDataInfo HumanRcd;
        public int nReTryCount;

        public TSaveRcd()
        {
            HumanRcd = new THumDataInfo();
        }
    }
}