using SystemModule.Common;

namespace SelGate
{
    public class ConfigManager : IniFile
    {
        public ConfigManager(string fileName) : base(fileName)
        {
            Load();
        }

        public void LoadConfig()
        {
            GateShare.ServerPort = ReadInteger(GateShare.GateClass, "ServerPort", GateShare.ServerPort);
            GateShare.ServerAddr = ReadString(GateShare.GateClass, "ServerAddr", GateShare.ServerAddr);
            GateShare.GatePort = ReadInteger(GateShare.GateClass, "GatePort", GateShare.GatePort);
            GateShare.GateAddr = ReadString(GateShare.GateClass, "GateAddr", GateShare.GateAddr);
            GateShare.nShowLogLevel = ReadInteger(GateShare.GateClass, "ShowLogLevel", GateShare.nShowLogLevel);
            GateShare.BlockMethod = ((TBlockIPMethod)(ReadInteger(GateShare.GateClass, "BlockMethod", (int)GateShare.BlockMethod)));
            if (ReadInteger(GateShare.GateClass, "KeepConnectTimeOut", -1) <= 0)
            {
                WriteInteger(GateShare.GateClass, "KeepConnectTimeOut", GateShare.dwKeepConnectTimeOut);
            }
            GateShare.nMaxConnOfIPaddr = ReadInteger(GateShare.GateClass, "MaxConnOfIPaddr", GateShare.nMaxConnOfIPaddr);
            GateShare.dwKeepConnectTimeOut = Read<long>(GateShare.GateClass, "KeepConnectTimeOut", GateShare.dwKeepConnectTimeOut);
            GateShare.g_boDynamicIPDisMode = ReadBool(GateShare.GateClass, "DynamicIPDisMode", GateShare.g_boDynamicIPDisMode);
            GateShare.LoadBlockIPFile();
        }
    }
}