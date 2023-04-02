using SystemModule.Packets.ServerPackets;

namespace SystemModule.Data;

public class SavePlayerRcd
{
    public string Account;
    public string ChrName;
    public int SessionID;
    public object PlayObject;
    public PlayerDataInfo HumanRcd;
    public int ReTryCount;
    public bool IsSaveing;
    public int QueryId;

    public SavePlayerRcd()
    {
        HumanRcd = new PlayerDataInfo();
    }
}