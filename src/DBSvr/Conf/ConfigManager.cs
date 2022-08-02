using System.IO;
using System;
using SystemModule.Common;

namespace DBSvr
{
    public class ConfigManager : IniFile
    {
        public DBConfig Config;
        private static string sConfitFile = Path.Combine(AppContext.BaseDirectory, "config.conf");

        private static readonly ConfigManager instance = new ConfigManager(sConfitFile);

        public static DBConfig GetConfig()
        {
            return Instance.Config;
        }

        public static ConfigManager Instance => instance;

        public ConfigManager(string fileName) : base(fileName)
        {
            Config = new DBConfig();
            Load();
        }

        public void LoadConfig()
        {
            Config.DBConnection = ReadString("DataBase", "ConnctionString", Config.DBConnection);
            Config.nServerPort = ReadInteger("Setup", "ServerPort", Config.nServerPort);
            Config.sServerAddr = ReadString("Setup", "ServerAddr", Config.sServerAddr);
            Config.g_nGatePort = ReadInteger("Setup", "GatePort", Config.g_nGatePort);
            Config.g_sGateAddr = ReadString("Setup", "GateAddr", Config.g_sGateAddr);
            Config.sIDServerAddr = ReadString("Server", "IDSAddr", Config.sIDServerAddr);
            Config.nIDServerPort = ReadInteger("Server", "IDSPort", Config.nIDServerPort);
            Config.sServerName = ReadString("Setup", "ServerName", Config.sServerName);
            Config.boDenyChrName = ReadBool("Setup", "DenyChrName", Config.boDenyChrName);
            Config.nDELMaxLevel = ReadInteger("Setup", "DELMaxLevel", Config.nDELMaxLevel);
            Config.dwInterval = Read<int>("DBClear", "Interval", Config.dwInterval);
            var LoadInteger = ReadInteger("Setup", "DynamicIPMode", -1);
            if (LoadInteger < 0)
            {
                WriteBool("Setup", "DynamicIPMode", Config.g_boDynamicIPMode);
            }
            else
            {
                Config.g_boDynamicIPMode = LoadInteger == 1;
            }
            Config.g_boEnglishNames = ReadBool("Setup", "EnglishNameOnly", Config.g_boEnglishNames);
            Config.sMapFile = ReadString("Setup", "MapFile", Config.sMapFile);
        }
    }
}