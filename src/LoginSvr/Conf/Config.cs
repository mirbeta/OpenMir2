using System.Collections.Generic;

namespace LoginSvr.Conf
{
    public class Config
    {
        public bool ShowDebugLog;
        public int ShowLogLevel;
        public string sDBServer;
        public int nDBSPort;
        public string sFeeServer;
        public int nFeePort;
        public string sLogServer;
        public int nLogPort;
        public string sGateAddr;
        public int nGatePort;
        public string sServerAddr;
        public int nServerPort;
        public string sMonAddr;
        public int nMonPort;
        public string sGateIPaddr;
        public string sFeedIDList;
        public string sFeedIPList;
        public bool TestServer;
        /// <summary>
        /// 是否允许创建账号
        /// </summary>
        public bool boEnableMakingID;
        public bool boDynamicIPMode;
        public int nReadyServers;
        public IList<TConnInfo> SessionList;
        public IList<string> ServerNameList;
        public Dictionary<string, int> AccountCostList;
        public Dictionary<string, int> IPaddrCostList;
        public int nRouteCount;
        public TGateRoute[] GateRoute;
        public string ConnctionString;

        public Config()
        {
            sDBServer = "127.0.0.1";
            nDBSPort = 16300;
            sFeeServer = "127.0.0.1";
            nFeePort = 16301;
            sLogServer = "127.0.0.1";
            nLogPort = 16301;
            sGateAddr = "*";
            nGatePort = 5500;
            sServerAddr = "*";
            nServerPort = 5600;
            sMonAddr = "*";
            nMonPort = 3000;
            sFeedIDList = "FeedIDList.txt";
            sFeedIPList = "FeedIPList.txt";
            TestServer = true;
            boEnableMakingID = true;
            boDynamicIPMode = false;
            nReadyServers = 0;
            GateRoute = new TGateRoute[60];
            ConnctionString = "server=127.0.0.1;uid=root;pwd=;database=mir2;";
            ShowDebugLog = false;
            ShowLogLevel = 1;
        }
    }
}

