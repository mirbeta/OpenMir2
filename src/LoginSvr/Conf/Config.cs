using System.Collections.Generic;

namespace LoginSvr.Conf
{
    public class AccountOption
    {
        
    }

    public class Config
    {
        /// <summary>
        /// 账号付费模式(0:免费 1:收费)
        /// 收费模式(0:免费 1:点卡 2:月卡 3:试玩 4:永久免费)
        /// </summary>
        public int PayMode;
        /// <summary>
        /// 是否显示调试日志
        /// </summary>
        public bool ShowDebugLog;
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
        public int ReadyServers;
        public IList<TConnInfo> SessionList;
        public IList<string> ServerNameList;
        public int RouteCount;
        public TGateRoute[] GateRoute;
        public string ConnctionString;

        public Config()
        {
            PayMode = 0;
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
            ReadyServers = 0;
            GateRoute = new TGateRoute[60];
            ConnctionString = "server=127.0.0.1;uid=root;pwd=;database=mir2_account;";
            ShowDebugLog = false;
            ShowLogLevel = 1;
        }
    }
}

