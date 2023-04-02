using SystemModule.Packets.ServerPackets;

namespace SystemModule.Data;

public class UserOpenInfo
{
    /// <summary>
    /// 查询Id
    /// </summary>
    public int QueryId;
    /// <summary>
    /// 失败次数
    /// </summary>
    public int FailCount;
    public string ChrName;
    public LoadDBInfo LoadUser;
    public PlayerDataInfo HumanRcd;
}