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
            for (var i = 0; i < GameGates.Length; i++)
            {
                GameGates[i].LoginAdress = "127.0.0.1";
                GameGates[i].LoginPort = 5500;
                GameGates[i].GateAddress = "127.0.0.1";
                GameGates[i].GatePort = 7000 + i;
            }
        }

        public void LoadConfig()
        {
            GetConfig.ClientTimeOutTime = ReadInteger("Integer", "ClientTimeOutTime", GetConfig.ClientTimeOutTime);
            if (GetConfig.ClientTimeOutTime < 10 * 1000)
            {
                GetConfig.ClientTimeOutTime = 10 * 1000;
                WriteInteger("Integer", "ClientTimeOutTime", GetConfig.ClientTimeOutTime);
            }
            GetConfig.m_nMaxConnectOfIP = ReadInteger("Integer", "MaxConnectOfIP", GetConfig.m_nMaxConnectOfIP);
            GetConfig.m_nCheckNewIDOfIP = ReadInteger("Integer", "CheckNewIDOfIP", GetConfig.m_nCheckNewIDOfIP);
            GetConfig.ClientTimeOutTime = ReadInteger("Integer", "ClientTimeOutTime", GetConfig.ClientTimeOutTime);
            GetConfig.NomClientPacketSize = ReadInteger("Integer", "NomClientPacketSize", GetConfig.NomClientPacketSize);
            GetConfig.MaxClientPacketCount = ReadInteger("Integer", "MaxClientPacketCount", GetConfig.MaxClientPacketCount);
            GetConfig.m_fCheckNewIDOfIP = ReadBool("Switch", "CheckNewIDOfIP", GetConfig.m_fCheckNewIDOfIP);
            GetConfig.CheckNullSession = ReadBool("Switch", "CheckNullSession", GetConfig.CheckNullSession);
            GetConfig.OverSpeedSendBack = ReadBool("Switch", "OverSpeedSendBack", GetConfig.OverSpeedSendBack);
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
            WriteBool("LoginGate", "ShowDebugLog", GetConfig.ShowDebugLog);
            for (var i = 0; i < GetConfig.GateCount; i++)
            {
                WriteString("LoginGate", "ServerAddr" + i, GameGates[i].LoginAdress);
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