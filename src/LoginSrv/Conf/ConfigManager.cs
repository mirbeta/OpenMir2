namespace LoginSrv.Conf
{
    public class ConfigManager : ConfigFile
    {
        private const string SectionServer = "Server";
        private const string DB = "DataBase";
        private const string IdentDBServer = "DBServer";
        private const string IdentFeeServer = "FeeServer";
        private const string IdentLogServer = "LogServer";
        private const string IdentGateAddr = "GateAddr";
        private const string IdentGatePort = "GatePort";
        private const string IdentServerAddr = "ServerAddr";
        private const string IdentServerPort = "ServerPort";
        private const string IdentMonAddr = "MonAddr";
        private const string IdentMonPort = "MonPort";
        private const string IdentDBSPort = "DBSPort";
        private const string IdentFeePort = "FeePort";
        private const string IdentLogPort = "LogPort";
        private const string IdentTestServer = "TestServer";
        private const string IdentDynamicIPMode = "DynamicIPMode";

        public readonly Config Config;

        public ConfigManager(string fileName) : base(fileName)
        {
            Load();
            Config = new Config();
            Config.ServerNameList = new List<string>();
        }

        public void LoadConfig()
        {
            Config.sDBServer = LoadConfigString(SectionServer, IdentDBServer, Config.sDBServer);
            Config.sFeeServer = LoadConfigString(SectionServer, IdentFeeServer, Config.sFeeServer);
            Config.sLogServer = LoadConfigString(SectionServer, IdentLogServer, Config.sLogServer);
            Config.sGateAddr = LoadConfigString(SectionServer, IdentGateAddr, Config.sGateAddr);
            Config.nGatePort = LoadConfigInteger(SectionServer, IdentGatePort, Config.nGatePort);
            Config.sServerAddr = LoadConfigString(SectionServer, IdentServerAddr, Config.sServerAddr);
            Config.nServerPort = LoadConfigInteger(SectionServer, IdentServerPort, Config.nServerPort);
            Config.sMonAddr = LoadConfigString(SectionServer, IdentMonAddr, Config.sMonAddr);
            Config.nMonPort = LoadConfigInteger(SectionServer, IdentMonPort, Config.nMonPort);
            Config.nDBSPort = LoadConfigInteger(SectionServer, IdentDBSPort, Config.nDBSPort);
            Config.nFeePort = LoadConfigInteger(SectionServer, IdentFeePort, Config.nFeePort);
            Config.nLogPort = LoadConfigInteger(SectionServer, IdentLogPort, Config.nLogPort);
            Config.EnableMakingID = LoadConfigBoolean(SectionServer, IdentTestServer, Config.EnableMakingID);
            Config.DynamicIPMode = LoadConfigBoolean(SectionServer, IdentDynamicIPMode, Config.DynamicIPMode);
            Config.ConnctionString = ReadWriteString(DB, "ConnctionString", Config.ConnctionString);
            Config.ShowLogLevel = ReadWriteInteger("Server", "ShowLogLevel", Config.ShowLogLevel);
            Config.ShowDebug = ReadWriteBool("Server", "ShowDebug", Config.ShowDebug);
        }

        private string LoadConfigString(string sSection, string sIdent, string sDefault)
        {
            string result;
            string sString = ReadWriteString(sSection, sIdent, "");
            if (string.IsNullOrEmpty(sString))
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
            int nLoadInteger = ReadWriteInteger(sSection, sIdent, -1);
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
            int nLoadInteger = ReadWriteInteger(sSection, sIdent, -1);
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
            using StringList LoadList = new StringList();
            if (File.Exists(sFileName))
            {
                LoadList.LoadFromFile(sFileName);
                nRouteIdx = 0;
                for (int i = 0; i < LoadList.Count; i++)
                {
                    sLineText = LoadList[i];
                    if (!string.IsNullOrEmpty(sLineText) && !sLineText.StartsWith(";"))
                    {
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sServerName, ' ');
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sTitle, ' ');
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sRemote, ' ');
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sPublic, ' ');
                        sLineText = sLineText.Trim();
                        if (!string.IsNullOrEmpty(sTitle) && !string.IsNullOrEmpty(sRemote) && !string.IsNullOrEmpty(sPublic) && nRouteIdx < 60)
                        {
                            Config.GateRoute[nRouteIdx] = new GateRoute();
                            Config.GateRoute[nRouteIdx].ServerName = sServerName;
                            Config.GateRoute[nRouteIdx].Title = sTitle;
                            Config.GateRoute[nRouteIdx].RemoteAddr = sRemote;
                            Config.GateRoute[nRouteIdx].PublicAddr = sPublic;
                            nSelGateIdx = 0;
                            while (!string.IsNullOrEmpty(sLineText))
                            {
                                if (nSelGateIdx > 9)
                                {
                                    break;
                                }
                                sLineText = HUtil32.GetValidStr3(sLineText, ref sGate, ' ');
                                if (!string.IsNullOrEmpty(sGate))
                                {
                                    if (sGate[0] == '*')
                                    {
                                        sGate = sGate[1..];
                                        Config.GateRoute[nRouteIdx].Gate[nSelGateIdx].boEnable = false;
                                    }
                                    else
                                    {
                                        Config.GateRoute[nRouteIdx].Gate[nSelGateIdx].boEnable = true;
                                    }
                                    sGatePort = HUtil32.GetValidStr3(sGate, ref sGate, ':');
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
            GenServerNameList(Config);
        }

        private void GenServerNameList(Config Config)
        {
            Config.ServerNameList.Clear();
            for (int i = 0; i < Config.RouteCount; i++)
            {
                bool boD = true;
                for (int j = 0; j < Config.ServerNameList.Count; j++)
                {
                    if (Config.ServerNameList[j] == Config.GateRoute[i].ServerName)
                    {
                        boD = false;
                    }
                }
                if (boD)
                {
                    Config.ServerNameList.Add(Config.GateRoute[i].ServerName);
                }
            }
        }
    }
}