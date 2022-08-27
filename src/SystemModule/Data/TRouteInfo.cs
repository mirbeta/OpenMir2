namespace SystemModule.Data;

public class TRouteInfo
{
    public int nServerIdx;
    public int nGateCount;
    public string sSelGateIP;
    public string[] sGameGateIP;
    public int[] nGameGatePort;

    public TRouteInfo()
    {
        sGameGateIP = new string[16];
        nGameGatePort = new int[16];
    }
}