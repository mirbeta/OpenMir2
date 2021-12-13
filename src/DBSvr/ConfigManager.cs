using SystemModule.Common;

namespace DBSvr
{
    public class ConfigManager : IniFile
    {
        public ConfigManager(string fileName) : base(fileName)
        {
            Load();
        }

        public void LoadConfig()
        {
            DBShare.sLogPath = ReadString("DB", "LogDir", DBShare.sLogPath);
            DBShare.nServerPort = ReadInteger("Setup", "ServerPort", DBShare.nServerPort);
            DBShare.sServerAddr = ReadString("Setup", "ServerAddr", DBShare.sServerAddr);
            DBShare.g_nGatePort = ReadInteger("Setup", "GatePort", DBShare.g_nGatePort);
            DBShare.g_sGateAddr = ReadString("Setup", "GateAddr", DBShare.g_sGateAddr);
            DBShare.sIDServerAddr = ReadString("Server", "IDSAddr", DBShare.sIDServerAddr);
            DBShare.nIDServerPort = ReadInteger("Server", "IDSPort", DBShare.nIDServerPort);
            DBShare.sServerName = ReadString("Setup", "ServerName", DBShare.sServerName);
            DBShare.boDenyChrName = ReadBool("Setup", "DenyChrName", DBShare.boDenyChrName);
            DBShare.nDELMaxLevel = ReadInteger("Setup", "DELMaxLevel", DBShare.nDELMaxLevel);
            DBShare.dwInterval = Read<int>("DBClear", "Interval", DBShare.dwInterval);
            var LoadInteger = ReadInteger("Setup", "DynamicIPMode", -1);
            if (LoadInteger < 0)
            {
                WriteBool("Setup", "DynamicIPMode", DBShare.g_boDynamicIPMode);
            }
            else
            {
                DBShare.g_boDynamicIPMode = LoadInteger == 1;
            }
            DBShare.g_boEnglishNames = ReadBool("Setup", "EnglishNameOnly", DBShare.g_boEnglishNames);
        }
    }
}