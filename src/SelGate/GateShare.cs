using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using SystemModule.Common;

namespace SelGate
{
    public class TSockaddr
    {
        public long nIPaddr;
        public int nCount;
        public long dwIPCountTick1;
        public int nIPCount1;
        public long dwIPCountTick2;
        public int nIPCount2;
        public long dwDenyTick;
        public int nIPDenyCount;
    }

    public enum TBlockIPMethod
    {
        mDisconnect,
        mBlock,
        mBlockList
    }

    public class GateShare
    {
        public static IList<string> MainLogMsgList = null;
        public static IList<TSockaddr> BlockIPList = null;
        public static IList<TSockaddr> TempBlockIPList = null;
        public static IList<TSockaddr> CurrIPaddrList = null;
        public static int nIPCountLimit1 = 20;
        public static int nIPCountLimit2 = 40;
        public static int nShowLogLevel = 3;
        public static string GateClass = "SelGate";
        public static int ServerPort = 5100;
        public static string ServerAddr = "10.10.0.168";
        public static int GatePort = 7100;
        public static string GateAddr = "*";
        public static bool boGateReady = false;
        public static bool boServiceStart = false;
        public static long dwKeepAliveTick = 0;
        public static bool boKeepAliveTimcOut = false;
        public static int nSendMsgCount = 0;
        public static bool boSendHoldTimeOut = false;
        public static long dwSendHoldTick = 0;
        public static bool boDecodeLock = false;
        public static int nMaxConnOfIPaddr = 10;
        public static TBlockIPMethod BlockMethod = TBlockIPMethod.mDisconnect;
        public static long dwKeepConnectTimeOut = 60 * 1000;
        /// <summary>
        /// 用于动态IP，分机放置登录网关用，打开此模式后，网关将会把连接登录服务器的IP地址，
        /// 当为服务器IP，发给登录服务器，客户端将直接使用此IP连接角色网关
        /// </summary>
        public static bool g_boDynamicIPDisMode = false;
        public static bool boServerReady = false;
        public const int GATEMAXSESSION = 10000;
        public static int nSessionCount = 0;
        public static TUserSession[] g_SessionArray;
        public static ConcurrentDictionary<string, int> _sessionMap = new ConcurrentDictionary<string, int>();
        public static IList<string> ClientSockeMsgList = null;

        public static void LoadBlockIPFile()
        {
            string sFileName;
            StringList LoadList;
            string sIPaddr;
            int nIPaddr;
            TSockaddr IPaddr;
            sFileName = ".\\BlockIPList.txt";
            if (File.Exists(sFileName))
            {
                LoadList = new StringList();
                LoadList.LoadFromFile(sFileName);
                for (var i = 0; i < LoadList.Count; i++)
                {
                    sIPaddr = LoadList[0].Trim();
                    if (sIPaddr == "")
                    {
                        continue;
                    }
                    //nIPaddr = inet_addr((sIPaddr as string));
                    //if (nIPaddr == INADDR_NONE)
                    //{
                    //    continue;
                    //}
                    IPaddr = new TSockaddr();
                    //IPaddr.nIPaddr = nIPaddr;
                    BlockIPList.Add(IPaddr);
                }
                //LoadList.Free;
            }
        }

        public static void SaveBlockIPList()
        {
            StringList SaveList;
            SaveList = new StringList();
            for (var i = 0; i < BlockIPList.Count; i++)
            {
               // SaveList.Add(inet_ntoa(TInAddr(((BlockIPList[I]) as TSockaddr).nIPaddr)));
            }
            SaveList.SaveToFile(".\\BlockIPList.txt");
            //SaveList.Free;
        }
        
        public static void MainOutMessage(string sMsg, int nMsgLevel)
        {
            string tMsg;
            if (nMsgLevel <= nShowLogLevel)
            {
                tMsg = "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "] " + sMsg;
                MainLogMsgList.Add(tMsg);
            }
        }

        public static void Initialization()
        {
            ClientSockeMsgList = new List<string>();
            MainLogMsgList = new List<string>();
        }
    }
    
    public class TUserSession
    {
        public Socket Socket;
        public string sRemoteIPaddr;
        public int nSendMsgLen;
        public bool bo0C;
        public long dw10Tick;
        public int nCheckSendLength;
        public bool boSendAvailable;
        public bool boSendCheck;
        public long dwSendLockTimeOut;
        public int n20;
        public long dwUserTimeOutTick;
        public int SocketHandle;
        public string sIP;
        public IList<string> MsgList;
        public long dwConnctCheckTick;
    }
}