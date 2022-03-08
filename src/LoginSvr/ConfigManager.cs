using SystemModule.Common;

namespace LoginSvr
{
    public class ConfigManager : IniFile
    {
        public ConfigManager(string fileName) : base(fileName)
        {
            Load();
        }

        public void LoadConfig(TConfig Config)
        {
            const string sSectionServer = "Server";
            const string sSectionDB = "DB";
            const string sIdentDBServer = "DBServer";
            const string sIdentFeeServer = "FeeServer";
            const string sIdentLogServer = "LogServer";
            const string sIdentGateAddr = "GateAddr";
            const string sIdentGatePort = "GatePort";
            const string sIdentServerAddr = "ServerAddr";
            const string sIdentServerPort = "ServerPort";
            const string sIdentMonAddr = "MonAddr";
            const string sIdentMonPort = "MonPort";
            const string sIdentDBSPort = "DBSPort";
            const string sIdentFeePort = "FeePort";
            const string sIdentLogPort = "LogPort";
            const string sIdentReadyServers = "ReadyServers";
            const string sIdentTestServer = "TestServer";
            const string sIdentDynamicIPMode = "DynamicIPMode";
            const string sIdentFeedIDList = "FeedIDList";
            const string sIdentFeedIPList = "FeedIPList";
            Config.sDBServer = LoadConfig_LoadConfigString(sSectionServer, sIdentDBServer, Config.sDBServer);
            Config.sFeeServer = LoadConfig_LoadConfigString(sSectionServer, sIdentFeeServer, Config.sFeeServer);
            Config.sLogServer = LoadConfig_LoadConfigString(sSectionServer, sIdentLogServer, Config.sLogServer);
            Config.sGateAddr = LoadConfig_LoadConfigString(sSectionServer, sIdentGateAddr, Config.sGateAddr);
            Config.nGatePort = LoadConfig_LoadConfigInteger(sSectionServer, sIdentGatePort, Config.nGatePort);
            Config.sServerAddr = LoadConfig_LoadConfigString(sSectionServer, sIdentServerAddr, Config.sServerAddr);
            Config.nServerPort = LoadConfig_LoadConfigInteger(sSectionServer, sIdentServerPort, Config.nServerPort);
            Config.sMonAddr = LoadConfig_LoadConfigString(sSectionServer, sIdentMonAddr, Config.sMonAddr);
            Config.nMonPort = LoadConfig_LoadConfigInteger(sSectionServer, sIdentMonPort, Config.nMonPort);
            Config.nDBSPort = LoadConfig_LoadConfigInteger(sSectionServer, sIdentDBSPort, Config.nDBSPort);
            Config.nFeePort = LoadConfig_LoadConfigInteger(sSectionServer, sIdentFeePort, Config.nFeePort);
            Config.nLogPort = LoadConfig_LoadConfigInteger(sSectionServer, sIdentLogPort, Config.nLogPort);
            Config.nReadyServers = LoadConfig_LoadConfigInteger(sSectionServer, sIdentReadyServers, Config.nReadyServers);
            Config.boEnableMakingID = LoadConfig_LoadConfigBoolean(sSectionServer, sIdentTestServer, Config.boEnableMakingID);
            Config.boDynamicIPMode = LoadConfig_LoadConfigBoolean(sSectionServer, sIdentDynamicIPMode, Config.boDynamicIPMode);
            Config.sFeedIDList = LoadConfig_LoadConfigString(sSectionDB, sIdentFeedIDList, Config.sFeedIDList);
            Config.sFeedIPList = LoadConfig_LoadConfigString(sSectionDB, sIdentFeedIPList, Config.sFeedIPList);
        }

        private string LoadConfig_LoadConfigString(string sSection, string sIdent, string sDefault)
        {
            string result;
            string sString = ReadString(sSection, sIdent, "");
            if (sString == "")
            {
                WriteString(sSection, sIdent, sDefault);
                result = sDefault;
            }
            else
            {
                result = sString;
            }
            return result;
        }

        private int LoadConfig_LoadConfigInteger(string sSection, string sIdent, int nDefault)
        {
            int result;
            int nLoadInteger;
            nLoadInteger = ReadInteger(sSection, sIdent, -1);
            if (nLoadInteger < 0)
            {
                WriteInteger(sSection, sIdent, nDefault);
                result = nDefault;
            }
            else
            {
                result = nLoadInteger;
            }
            return result;
        }

        private bool LoadConfig_LoadConfigBoolean(string sSection, string sIdent, bool boDefault)
        {
            bool result;
            int nLoadInteger;
            nLoadInteger = ReadInteger(sSection, sIdent, -1);
            if (nLoadInteger < 0)
            {
                WriteBool(sSection, sIdent, boDefault);
                result = boDefault;
            }
            else
            {
                result = nLoadInteger == 1;
            }
            return result;
        }
    }
}