using System;
using SystemModule.Common;

namespace LoginGate.Conf
{
    public class ConfigManager : IniFile
    {
        public readonly GateConfig GetConfig;
        public readonly GameGateInfo[] GameGates;
        
        public ConfigManager(string szFileName) : base(szFileName)
        {
            Load();
            GetConfig = new GateConfig();
            GameGates = new GameGateInfo[32];
            for (var i = GameGates.GetLowerBound(0); i <= GameGates.GetUpperBound(0); i++)
            {
                GameGates[i].LoginAdress = "127.0.0.1";
                GameGates[i].LoginPort = 5500;
                GameGates[i].GateAddress = "127.0.0.1";
                GameGates[i].GatePort = 7000 + i;
            }
        }

        public void LoadConfig()
        {
            GetConfig.m_nClientTimeOutTime = ReadInteger("Integer", "ClientTimeOutTime", GetConfig.m_nClientTimeOutTime);
            if (GetConfig.m_nClientTimeOutTime < 10 * 1000)
            {
                GetConfig.m_nClientTimeOutTime = 10 * 1000;
                WriteInteger("Integer", "ClientTimeOutTime", GetConfig.m_nClientTimeOutTime);
            }
            GetConfig.m_nMaxConnectOfIP = ReadInteger("Integer", "MaxConnectOfIP", GetConfig.m_nMaxConnectOfIP);
            GetConfig.m_nCheckNewIDOfIP = ReadInteger("Integer", "CheckNewIDOfIP", GetConfig.m_nCheckNewIDOfIP);
            GetConfig.m_nClientTimeOutTime = ReadInteger("Integer", "ClientTimeOutTime", GetConfig.m_nClientTimeOutTime);
            GetConfig.m_nNomClientPacketSize = ReadInteger("Integer", "NomClientPacketSize", GetConfig.m_nNomClientPacketSize);
            GetConfig.m_nMaxClientPacketCount = ReadInteger("Integer", "MaxClientPacketCount", GetConfig.m_nMaxClientPacketCount);
            GetConfig.m_fCheckNewIDOfIP = ReadBool("Switch", "CheckNewIDOfIP", GetConfig.m_fCheckNewIDOfIP);
            GetConfig.m_fCheckNullSession = ReadBool("Switch", "CheckNullSession", GetConfig.m_fCheckNullSession);
            GetConfig.m_fOverSpeedSendBack = ReadBool("Switch", "OverSpeedSendBack", GetConfig.m_fOverSpeedSendBack);
            GetConfig.DefenceCCPacket = ReadBool("Switch", "DefenceCCPacket", GetConfig.DefenceCCPacket);
            GetConfig.m_fKickOverSpeed = ReadBool("Switch", "KickOverSpeed", GetConfig.m_fKickOverSpeed);
            GetConfig.m_fKickOverPacketSize = ReadBool("Switch", "KickOverPacketSize", GetConfig.m_fKickOverPacketSize);
            GetConfig.m_tBlockIPMethod = Enum.Parse<TBlockIPMethod>(ReadString("Method", "BlockIPMethod", GetConfig.m_tBlockIPMethod.ToString()));
            GetConfig.GateCount = ReadInteger("LoginGate", "Count", GetConfig.GateCount);
            GetConfig.ShowLogLevel = ReadInteger("LoginGate", "ShowLogLevel", GetConfig.ShowLogLevel);
            GetConfig.ShowDebugLog = ReadBool("LoginGate", "ShowDebugLog", GetConfig.ShowDebugLog);
            for (var i = 0; i < GetConfig.GateCount; i++)
            {
                GameGates[i].LoginAdress = ReadString("LoginGate", "ServerAddr" + i, GameGates[i].LoginAdress);
                GameGates[i].LoginPort = ReadInteger("LoginGate", "ServerPort" + i, GameGates[i].LoginPort);
                GameGates[i].GateAddress = ReadString("LoginGate", "GateAddr" + i, GameGates[i].GateAddress);
                GameGates[i].GatePort = ReadInteger("LoginGate", "GatePort" + i, GameGates[i].GatePort);
            }
        }
    }

    public struct GameGateInfo
    {
        /// <summary>
        /// 服务器地址
        /// </summary>
        public string LoginAdress;
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
}