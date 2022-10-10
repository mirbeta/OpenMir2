using System;
using System.IO;
using SystemModule.Common;

namespace DBSvr.Conf
{
    public class ConfigManager : IniFile
    {
        private readonly DBSvrConf _config;
        private static readonly string ConfitFile = Path.Combine(AppContext.BaseDirectory, "dbsvr.conf");

        public ConfigManager() : base(ConfitFile)
        {
            _config = new DBSvrConf();
            Load();
        }

        public DBSvrConf GetConfig => _config;

        public void LoadConfig()
        {
            _config.ConnctionString = ReadString("DataBase", "ConnctionString", _config.ConnctionString);
            _config.StoreageType = ReadString("DataBase", "Storeage", _config.StoreageType);
            _config.ShowDebugLog = ReadBool("Setup", "ShowDebugLog", _config.ShowDebugLog);
            _config.ServerPort = ReadInteger("Setup", "ServerPort", _config.ServerPort);
            _config.ServerAddr = ReadString("Setup", "ServerAddr", _config.ServerAddr);
            _config.GatePort = ReadInteger("Setup", "GatePort", _config.GatePort);
            _config.GateAddr = ReadString("Setup", "GateAddr", _config.GateAddr);
            _config.LoginServerAddr = ReadString("Server", "IDSAddr", _config.LoginServerAddr);
            _config.LoginServerPort = ReadInteger("Server", "IDSPort", _config.LoginServerPort);
            _config.ServerName = ReadString("Setup", "ServerName", _config.ServerName);
            _config.boDenyChrName = ReadBool("Setup", "DenyChrName", _config.boDenyChrName);
            _config.DeleteMinLevel = ReadInteger("Setup", "DELMaxLevel", _config.DeleteMinLevel);
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