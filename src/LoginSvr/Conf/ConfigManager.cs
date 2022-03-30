using System;
using System.Collections.Generic;
using System.IO;
using SystemModule;
using SystemModule.Common;

namespace LoginSvr
{
    public class ConfigManager : IniFile
    {
        private static string fileName = Path.Combine(AppContext.BaseDirectory, "Logsrv.conf");
        private static readonly ConfigManager instance = new ConfigManager(fileName);

        public static ConfigManager Instance
        {
            get { return instance; }
        }

        const string sSectionServer = "Server";
        const string sSectionDB = "DB";
        private const string sDB = "DataBase";
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

        public Config Config;

        public ConfigManager(string fileName) : base(fileName)
        {
            Load();
            Config = new Config();
            Config.SessionList = new List<TConnInfo>();
            Config.ServerNameList = new List<string>();
            Config.AccountCostList = new Dictionary<string, int>();
            Config.IPaddrCostList = new Dictionary<string, int>();
        }

        public void LoadConfig()
        {
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
            Config.ConnctionString = LoadConfig_LoadConfigString(sDB, "ConnctionString", Config.ConnctionString);
            Config.ShowDetailMsg = LoadConfig_LoadConfigBoolean(sSectionServer, "ShowDetailMsg", Config.ShowDetailMsg);
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

        public void LoadAddrTable()
        {
            int nRouteIdx;
            int nSelGateIdx;
            string sLineText = string.Empty;
            string sTitle = string.Empty;
            string sServerName = string.Empty;
            string sGate = string.Empty;
            string sRemote = string.Empty;
            string sPublic = string.Empty;
            string sGatePort = string.Empty;
            string sFileName = "!AddrTable.txt";
            StringList LoadList = new StringList();
            if (File.Exists(sFileName))
            {
                LoadList.LoadFromFile(sFileName);
                nRouteIdx = 0;
                for (var i = 0; i < LoadList.Count; i++)
                {
                    sLineText = LoadList[i];
                    if (sLineText != "" && sLineText[0] != ';')
                    {
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sServerName, new string[] { " " });
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sTitle, new string[] { " " });
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sRemote, new string[] { " " });
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sPublic, new string[] { " " });
                        sLineText = sLineText.Trim();
                        if (sTitle != "" && sRemote != "" && sPublic != "" && nRouteIdx < 60)
                        {
                            Config.GateRoute[nRouteIdx] = new TGateRoute();
                            Config.GateRoute[nRouteIdx].sServerName = sServerName;
                            Config.GateRoute[nRouteIdx].sTitle = sTitle;
                            Config.GateRoute[nRouteIdx].sRemoteAddr = sRemote;
                            Config.GateRoute[nRouteIdx].sPublicAddr = sPublic;
                            nSelGateIdx = 0;
                            while (sLineText != "")
                            {
                                if (nSelGateIdx > 9)
                                {
                                    break;
                                }
                                sLineText = HUtil32.GetValidStr3(sLineText, ref sGate, new string[] { " " });
                                if (sGate != "")
                                {
                                    if (sGate[0] == '*')
                                    {
                                        sGate = sGate.Substring(1, sGate.Length - 1);
                                        Config.GateRoute[nRouteIdx].Gate[nSelGateIdx].boEnable = false;
                                    }
                                    else
                                    {
                                        Config.GateRoute[nRouteIdx].Gate[nSelGateIdx].boEnable = true;
                                    }
                                    sGatePort = HUtil32.GetValidStr3(sGate, ref sGate, new string[] { ":" });
                                    Config.GateRoute[nRouteIdx].Gate[nSelGateIdx].sIPaddr = sGate;
                                    Config.GateRoute[nRouteIdx].Gate[nSelGateIdx].nPort = HUtil32.Str_ToInt(sGatePort, 0);
                                    Config.GateRoute[nRouteIdx].nSelIdx = 0;
                                    nSelGateIdx++;
                                }
                                sLineText = sLineText.Trim();
                            }
                            nRouteIdx++;
                        }
                    }
                }
                Config.nRouteCount = nRouteIdx;
            }
            LoadList = null;
            GenServerNameList(Config);
        }

        private void GenServerNameList(Config Config)
        {
            Config.ServerNameList.Clear();
            for (var i = 0; i < Config.nRouteCount; i++)
            {
                bool boD = true;
                for (var j = 0; j < Config.ServerNameList.Count; j++)
                {
                    if (Config.ServerNameList[j] == Config.GateRoute[i].sServerName)
                    {
                        boD = false;
                    }
                }
                if (boD)
                {
                    Config.ServerNameList.Add(Config.GateRoute[i].sServerName);
                }
            }
        }
    }
}