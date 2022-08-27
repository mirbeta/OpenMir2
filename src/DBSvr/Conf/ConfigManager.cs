using System;
using System.IO;
using SystemModule.Common;

namespace DBSvr.Conf
{
    public class ConfigManager : IniFile
    {
        private readonly DBConfig _config;
        private static readonly string ConfitFile = Path.Combine(AppContext.BaseDirectory, "dbsvr.conf");

        private static readonly ConfigManager instance = new ConfigManager(ConfitFile);

        public static DBConfig GetConfig()
        {
            return Instance._config;
        }

        public static ConfigManager Instance => instance;

        public ConfigManager(string fileName) : base(fileName)
        {
            _config = new DBConfig();
            Load();
        }

        public void LoadConfig()
        {
            _config.ShowDebugLog = ReadBool("Setup", "ShowDebugLog", _config.ShowDebugLog);
            _config.DBConnection = ReadString("DataBase", "ConnctionString", _config.DBConnection);
            _config.ServerPort = ReadInteger("Setup", "ServerPort", _config.ServerPort);
            _config.ServerAddr = ReadString("Setup", "ServerAddr", _config.ServerAddr);
            _config.GatePort = ReadInteger("Setup", "GatePort", _config.GatePort);
            _config.GateAddr = ReadString("Setup", "GateAddr", _config.GateAddr);
            _config.LoginServerAddr = ReadString("Server", "IDSAddr", _config.LoginServerAddr);
            _config.LoginServerPort = ReadInteger("Server", "IDSPort", _config.LoginServerPort);
            _config.ServerName = ReadString("Setup", "ServerName", _config.ServerName);
            _config.boDenyChrName = ReadBool("Setup", "DenyChrName", _config.boDenyChrName);
            _config.nDELMaxLevel = ReadInteger("Setup", "DELMaxLevel", _config.nDELMaxLevel);
            _config.Interval = Read<int>("DBClear", "Interval", _config.Interval);
            var dynamicIpMode = ReadInteger("Setup", "DynamicIPMode", -1);
            if (dynamicIpMode < 0)
            {
                WriteBool("Setup", "DynamicIPMode", _config.DynamicIpMode);
            }
            else
            {
                _config.DynamicIpMode = dynamicIpMode == 1;
            }
            _config.EnglishNames = ReadBool("Setup", "EnglishNameOnly", _config.EnglishNames);
            _config.MapFile = ReadString("Setup", "MapFile", _config.MapFile);
        }
    }
}