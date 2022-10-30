using System;
using System.Collections.Generic;
using System.IO;
using SystemModule;
using SystemModule.Common;

namespace LoginSvr.Conf
{
    public class ConfigManager : IniFile
    {
        const string sSectionServer = "Server";
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

        public readonly Config Config;

        public ConfigManager(string fileName) : base(fileName)
        {
            Load();
            Config = new Config();
            Config.SessionList = new List<TConnInfo>();
            Config.ServerNameList = new List<string>();
        }

        public void LoadConfig()
        {
            Config.sDBServer = LoadConfigString(sSectionServer, sIdentDBServer, Config.sDBServer);
            Config.sFeeServer = LoadConfigString(sSectionServer, sIdentFeeServer, Config.sFeeServer);
            Config.sLogServer = LoadConfigString(sSectionServer, sIdentLogServer, Config.sLogServer);
            Config.sGateAddr = LoadConfigString(sSectionServer, sIdentGateAddr, Config.sGateAddr);
            Config.nGatePort = LoadConfigInteger(sSectionServer, sIdentGatePort, Config.nGatePort);
            Config.sServerAddr = LoadConfigString(sSectionServer, sIdentServerAddr, Config.sServerAddr);
            Config.nServerPort = LoadConfigInteger(sSectionServer, sIdentServerPort, Config.nServerPort);
            Config.sMonAddr = LoadConfigString(sSectionServer, sIdentMonAddr, Config.sMonAddr);
            Config.nMonPort = LoadConfigInteger(sSectionServer, sIdentMonPort, Config.nMonPort);
            Config.nDBSPort = LoadConfigInteger(sSectionServer, sIdentDBSPort, Config.nDBSPort);
            Config.nFeePort = LoadConfigInteger(sSectionServer, sIdentFeePort, Config.nFeePort);
            Config.nLogPort = LoadConfigInteger(sSectionServer, sIdentLogPort, Config.nLogPort);
            Config.ReadyServers = LoadConfigInteger(sSectionServer, sIdentReadyServers, Config.ReadyServers);
            Config.EnableMakingID = LoadConfigBoolean(sSectionServer, sIdentTestServer, Config.EnableMakingID);
            Config.DynamicIPMode = LoadConfigBoolean(sSectionServer, sIdentDynamicIPMode, Config.DynamicIPMode);
            Config.ConnctionString = LoadConfigString(sDB, "ConnctionString", Config.ConnctionString);
            Config.ShowLogLevel = ReadInteger("Server", "ShowLogLevel", Config.ShowLogLevel);
            Config.ShowDebugLog = ReadBool("Server", "ShowDebugLog", Config.ShowDebugLog);
            Config.PayMode = ReadInteger("Server", "PayMode", Config.PayMode);
        }

        private string LoadConfigString(string sSection, string sIdent, string sDefault)
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

        private int LoadConfigInteger(string sSection, string sIdent, int nDefault)
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

        private bool LoadConfigBoolean(string sSection, string sIdent, bool boDefault)
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
            string sFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AddrTable.txt");
            StringList LoadList = new StringList();
            if (File.Exists(sFileName))
            {
                LoadList.LoadFromFile(sFileName);
                nRouteIdx = 0;
                for (var i = 0; i < LoadList.Count; i++)
                {
                    sLineText = LoadList[i];
                    if (!string.IsNullOrEmpty(sLineText) && !sLineText.StartsWith(";"))
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
                                    Config.GateRoute[nRouteIdx].Gate[nSelGateIdx].nPort = HUtil32.StrToInt(sGatePort, 0);
                                    Config.GateRoute[nRouteIdx].nSelIdx = 0;
                                    nSelGateIdx++;
                                }
                                sLineText = sLineText.Trim();
                            }
                            nRouteIdx++;
                        }
                    }
                }
                Config.RouteCount = nRouteIdx;
            }
            LoadList = null;
            GenServerNameList(Config);
        }

        private void GenServerNameList(Config Config)
        {
            Config.ServerNameList.Clear();
            for (var i = 0; i < Config.RouteCount; i++)
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