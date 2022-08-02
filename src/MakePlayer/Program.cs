using System.Text;
using SystemModule;
using SystemModule.Packet;
using SystemModule.Sockets;

namespace MakePlayer
{
    class Program
    {
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
        private static int g_nTotalChrCount = 3000;
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

            //AccountCrateTest();

            g_sServerName = "热血传奇";
            g_sGameIPaddr = "10.10.0.102";
            g_nGamePort = 7000;
            g_boNewAccount = false;

            g_nChrCount = HUtil32._MIN(g_nChrCount, g_nTotalChrCount);
            g_dwLogonTick = HUtil32.GetTickCount() - 1000 * g_nChrCount;
            g_sAccount = "winpalay";

            _playTimer = new Thread(start: Start);
            _playTimer.IsBackground = true;
            _playTimer.Start();

            ClientManager.Start();

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

        private static IClientScoket ClientSocket;

        static void AccountCrateTest()
        {
            ClientSocket = new IClientScoket();
            ClientSocket.Host = "10.10.0.112";
            ClientSocket.Port = 7000;
            ClientSocket.Connect();

            UserFullEntry ue = new UserFullEntry();
            var sAccount = "mplay0";
            var sPassword = "mplay0";
            ue.UserEntry.sAccount = sAccount;
            ue.UserEntry.sPassword = sPassword;
            ue.UserEntry.sUserName = sAccount;
            ue.UserEntry.sSSNo = "650101-1455111";
            ue.UserEntry.sQuiz = sAccount;
            ue.UserEntry.sAnswer = sAccount;
            ue.UserEntry.sPhone = "";
            ue.UserEntry.sEMail = "";
            ue.UserEntryAdd.sQuiz2 = sAccount;
            ue.UserEntryAdd.sAnswer2 = sAccount;
            ue.UserEntryAdd.sBirthDay = "1978/01/01";
            ue.UserEntryAdd.sMobilePhone = "";
            ue.UserEntryAdd.sMemo = "";
            ue.UserEntryAdd.sMemo2 = "";
            var Msg = Grobal2.MakeDefaultMsg(Grobal2.CM_ADDNEWUSER, 0, 0, 0, 0);
            ClientSocket.IsConnected = true;
            SendSocket(EDcode.EncodeMessage(Msg) + EDcode.EncodeBuffer(ue));
        }

        private static void SendSocket(string sText)
        {
            if (ClientSocket.IsConnected)
            {
                var sSendText = "#" + 0 + sText + "!";
                ClientSocket.SendText(sSendText);
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
                            var playClient = new PlayClient();
                            playClient.SessionId = Guid.NewGuid().ToString("N");
                            playClient.m_boNewAccount = g_boNewAccount;
                            playClient.m_sLoginAccount = string.Concat(g_sAccount, g_nLoginIndex);
                            playClient.m_sLoginPasswd = playClient.m_sLoginAccount;
                            playClient.m_sCharName = playClient.m_sLoginAccount;
                            playClient.m_sServerName = g_sServerName;
                            playClient.ClientSocket.Close();
                            playClient.ClientSocket.Host = g_sGameIPaddr;
                            playClient.ClientSocket.Port = g_nGamePort;
                            playClient.m_dwConnectTick = HUtil32.GetTickCount() + (i + 1) * 3000;
                            ClientManager.AddClient(playClient.SessionId, playClient);
                            g_nLoginIndex++;
                        }
                    }
                }
                ClientManager.Run();
                Thread.Sleep(TimeSpan.FromMilliseconds(50));
            }
        }
    }
}