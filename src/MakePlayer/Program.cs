using System.Text;
using SystemModule;

namespace MakePlayer
{
    class Program
    {
        private static ClientManager _clientManager;
        /// <summary>
        /// 服务器名称
        /// </summary>
        private static string g_sServerName = String.Empty;
        /// <summary>
        /// 游戏服务器IP地址
        /// </summary>
        private static string g_sGameIPaddr = String.Empty;
        /// <summary>
        /// 服务器端口号
        /// </summary>
        private static int g_nGamePort = 0;
        /// <summary>
        /// 账号前缀
        /// </summary>
        private static string g_sAccount = String.Empty;
        /// <summary>
        /// 同时登录人数
        /// </summary>
        private static int g_nChrCount = 1;
        /// <summary>
        /// 登录总人数
        /// </summary>
        private static int g_nTotalChrCount = 1;
        /// <summary>
        /// 是否创建帐号
        /// </summary>
        private static bool g_boNewAccount = false;
        /// <summary>
        /// 登录序号
        /// </summary>
        private static int g_nLoginIndex = 0;
        /// <summary>
        /// 登录间隔
        /// </summary>
        private static long g_dwLogonTick = 0;
        private static Thread _playTimer;

        static void Main(string[] args)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            _clientManager = new ClientManager();

            g_sServerName = "热血传奇";
            g_sGameIPaddr = "127.0.0.1";
            g_nGamePort = 7000;
            g_boNewAccount = false;

            g_nChrCount = HUtil32._MIN(g_nChrCount, g_nTotalChrCount);
            g_dwLogonTick = HUtil32.GetTickCount() - 1000 * g_nChrCount;
            g_sAccount = "mplay";

            _playTimer = new Thread(start: Start);
            _playTimer.Start();

            while (true)
            {
                var line = Console.ReadLine();
                switch (line)
                {
                    case "exit":

                        break;
                }
            }
        }

        static void Start(object? obj)
        {
            while (true)
            {
                if (g_nTotalChrCount > 0)
                {
                    if (((HUtil32.GetTickCount() - g_dwLogonTick) > 1000 * g_nChrCount))
                    {
                        g_dwLogonTick = HUtil32.GetTickCount();
                        if (g_nTotalChrCount >= g_nChrCount)
                        {
                            g_nTotalChrCount -= g_nChrCount;
                        }
                        else
                        {
                            g_nTotalChrCount = 0;
                        }
                        for (var i = 0; i < g_nChrCount; i++)
                        {
                            var ObjClient = new TObjClient();
                            ObjClient.m_boNewAccount = g_boNewAccount;
                            ObjClient.m_sLoginAccount = string.Concat(g_sAccount, g_nLoginIndex);
                            ObjClient.m_sLoginPasswd = ObjClient.m_sLoginAccount;
                            ObjClient.m_sCharName = ObjClient.m_sLoginAccount;
                            ObjClient.m_sServerName = g_sServerName;
                            ObjClient.ClientSocket.Address = g_sGameIPaddr;
                            ObjClient.ClientSocket.Port = g_nGamePort;
                            ObjClient.m_dwConnectTick = HUtil32.GetTickCount() + (i + 1) * 3000;
                            _clientManager.AddClient(ObjClient);
                            g_nLoginIndex++;
                        }
                    }
                }
                _clientManager.Run();
                Thread.Sleep(1);
            }
        }
    }
}