using OpenMir2.Common;

namespace LoginGate.Conf;

public class ConfigManager : ConfigFile
{
    public readonly GateConfig GetConfig;
    public readonly GameGateInfo[] GameGates;

    public ConfigManager(string szFileName) : base(szFileName)
    {
        Load();
        GetConfig = new GateConfig();
        GameGates = new GameGateInfo[32];
        for (int i = 0; i < GameGates.Length; i++)
        {
            GameGates[i].LoginServer = "127.0.0.1";
            GameGates[i].LoginPort = 5500;
            GameGates[i].GateAddress = "127.0.0.1";
            GameGates[i].GatePort = 7000 + i;
        }
    }

    public void LoadConfig()
    {
        GetConfig.ClientTimeOutTime = ReadWriteInteger("Integer", "ClientTimeOutTime", GetConfig.ClientTimeOutTime);
        if (GetConfig.ClientTimeOutTime < 10 * 1000)
        {
            GetConfig.ClientTimeOutTime = 10 * 1000;
            WriteInteger("Integer", "ClientTimeOutTime", GetConfig.ClientTimeOutTime);
        }

        GetConfig.m_nMaxConnectOfIP = ReadWriteInteger("Integer", "MaxConnectOfIP", GetConfig.m_nMaxConnectOfIP);
        GetConfig.m_nCheckNewIDOfIP = ReadWriteInteger("Integer", "CheckNewIDOfIP", GetConfig.m_nCheckNewIDOfIP);
        GetConfig.ClientTimeOutTime = ReadWriteInteger("Integer", "ClientTimeOutTime", GetConfig.ClientTimeOutTime);
        GetConfig.NomClientPacketSize =
            ReadWriteInteger("Integer", "NomClientPacketSize", GetConfig.NomClientPacketSize);
        GetConfig.MaxClientPacketCount =
            ReadWriteInteger("Integer", "MaxClientPacketCount", GetConfig.MaxClientPacketCount);
        GetConfig.m_fCheckNewIDOfIP = ReadWriteBool("Switch", "CheckNewIDOfIP", GetConfig.m_fCheckNewIDOfIP);
        GetConfig.CheckNullSession = ReadWriteBool("Switch", "CheckNullSession", GetConfig.CheckNullSession);
        GetConfig.OverSpeedSendBack = ReadWriteBool("Switch", "OverSpeedSendBack", GetConfig.OverSpeedSendBack);
        GetConfig.DefenceCCPacket = ReadWriteBool("Switch", "DefenceCCPacket", GetConfig.DefenceCCPacket);
        GetConfig.m_fKickOverSpeed = ReadWriteBool("Switch", "KickOverSpeed", GetConfig.m_fKickOverSpeed);
        GetConfig.m_fKickOverPacketSize =
            ReadWriteBool("Switch", "KickOverPacketSize", GetConfig.m_fKickOverPacketSize);
        GetConfig.m_tBlockIPMethod =
            Enum.Parse<TBlockIPMethod>(
                ReadWriteString("Method", "BlockIPMethod", GetConfig.m_tBlockIPMethod.ToString()));
        GetConfig.GateCount = ReadWriteInteger("LoginGate", "Count", GetConfig.GateCount);
        GetConfig.ShowLogLevel = ReadWriteInteger("LoginGate", "ShowLogLevel", GetConfig.ShowLogLevel);
        GetConfig.ShowDebug = ReadWriteBool("LoginGate", "ShowDebug", GetConfig.ShowDebug);
        for (int i = 0; i < GetConfig.GateCount; i++)
        {
            GameGates[i].LoginServer = ReadWriteString("LoginGate", "ServerAddr" + i, GameGates[i].LoginServer);
            GameGates[i].LoginPort = ReadWriteInteger("LoginGate", "ServerPort" + i, GameGates[i].LoginPort);
            GameGates[i].GateAddress = ReadWriteString("LoginGate", "GateAddr" + i, GameGates[i].GateAddress);
            GameGates[i].GatePort = ReadWriteInteger("LoginGate", "GatePort" + i, GameGates[i].GatePort);
        }

        SaveConfig();
    }

    private void SaveConfig()
    {
        WriteInteger("Integer", "ClientTimeOutTime", GetConfig.ClientTimeOutTime);
        WriteInteger("Integer", "MaxConnectOfIP", GetConfig.m_nMaxConnectOfIP);
        WriteInteger("Integer", "CheckNewIDOfIP", GetConfig.m_nCheckNewIDOfIP);
        WriteInteger("Integer", "ClientTimeOutTime", GetConfig.ClientTimeOutTime);
        WriteInteger("Integer", "NomClientPacketSize", GetConfig.NomClientPacketSize);
        WriteInteger("Integer", "MaxClientPacketCount", GetConfig.MaxClientPacketCount);
        WriteBool("Switch", "CheckNewIDOfIP", GetConfig.m_fCheckNewIDOfIP);
        WriteBool("Switch", "CheckNullSession", GetConfig.CheckNullSession);
        WriteBool("Switch", "OverSpeedSendBack", GetConfig.OverSpeedSendBack);
        WriteBool("Switch", "DefenceCCPacket", GetConfig.DefenceCCPacket);
        WriteBool("Switch", "KickOverSpeed", GetConfig.m_fKickOverSpeed);
        WriteBool("Switch", "KickOverPacketSize", GetConfig.m_fKickOverPacketSize);
        WriteInteger("Method", "BlockIPMethod", (int)GetConfig.m_tBlockIPMethod);
        WriteInteger("LoginGate", "Count", GetConfig.GateCount);
        WriteInteger("LoginGate", "ShowLogLevel", GetConfig.ShowLogLevel);
        WriteBool("LoginGate", "ShowDebug", GetConfig.ShowDebug);
        for (int i = 0; i < GetConfig.GateCount; i++)
        {
            WriteString("LoginGate", "ServerAddr" + i, GameGates[i].LoginServer);
            WriteInteger("LoginGate", "ServerPort" + i, GameGates[i].LoginPort);
            WriteString("LoginGate", "GateAddr" + i, GameGates[i].GateAddress);
            WriteInteger("LoginGate", "GatePort" + i, GameGates[i].GatePort);
        }

        Save();
    }
}

public struct GameGateInfo
{
    /// <summary>
    /// 服务器地址
    /// </summary>
    public string LoginServer;

    /// <summary>
    /// 服务器端口 默认值：5500
    /// </summary>
    public int LoginPort;

    /// <summary>
    /// 网关地址
    /// </summary>
    public string GateAddress;

    /// <summary>
    /// 网关端口 默认值：7000
    /// </summary>
    public int GatePort;
}