namespace DBSvr
{
    public class DBConfig
    {
        public bool ShowDebugLog = false;
        public int ShowLogLevel = 1;
        public int nServerPort = 6000;
        public string sServerAddr = "*";
        public int g_nGatePort = 5100;
        public string g_sGateAddr = "*";
        public int nIDServerPort = 5600;
        public string sIDServerAddr = "127.0.0.1";
        public bool g_boEnglishNames = false;
        public string sServerName = "热血传奇";
        /// <summary>
        /// 是否禁止检测玩家名字
        /// </summary>
        public bool boDenyChrName = true;
        public int nDELMaxLevel = 30;
        public string DBConnection = "server=127.0.0.1;uid=root;pwd=;database=Mir2;";
        public int dwInterval = 3000;
        public bool g_boDynamicIPMode = false;
        public string sMapFile = string.Empty;
    }
}