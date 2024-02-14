namespace OpenMir2.Data
{
    public class TRouteInfo
    {
        public int ServerIdx;
        public int GateCount;
        public string SelGateIP;
        public string[] GameGateIP;
        public int[] GameGatePort;

        public TRouteInfo()
        {
            GameGateIP = new string[16];
            GameGatePort = new int[16];
        }
    }
}