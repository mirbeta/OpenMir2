namespace LoginSrv.Conf
{
    public class Config
    {
        /// <summary>
        /// 是否显示调试日志
        /// </summary>
        public bool ShowDebug;
        /// <summary>
        /// 日志等级
        /// </summary>
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
        /// <summary>
        /// 测试
        /// </summary>
        public bool TestServer;
        /// <summary>
        /// 是否允许创建账号
        /// </summary>
        public bool EnableMakingID;
        public bool DynamicIPMode;
        public IList<string> ServerNameList;
        public int RouteCount;
        public GateRoute[] GateRoute;
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
            TestServer = true;
            EnableMakingID = true;
            DynamicIPMode = false;
            GateRoute = new GateRoute[60];
            ConnctionString = "server=127.0.0.1;uid=root;pwd=;database=mir2_account;";
            ShowDebug = false;
            ShowLogLevel = 1;
        }
    }
}

