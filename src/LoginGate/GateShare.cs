using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using SystemModule;
using SystemModule.Common;

namespace LoginGate
{
    public class GateShare
    {
        public static IList<string> MainLogMsgList = null;
        public static IList<TSockaddr> BlockIPList = null;
        public static IList<TSockaddr> TempBlockIPList = null;
        public static IList<TSockaddr> CurrIPaddrList = null;
        public static int nIPCountLimit1 = 20;
        public static int nIPCountLimit2 = 40;
        public static string GateClass = "LoginGate";
        public static int ServerPort = 5500;
        public static string ServerAddr = "127.0.0.1";
        public static int GatePort = 7000;
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
        public static bool g_boDynamicIPDisMode = false;
        public static int GATEMAXSESSION = 10000;
        public static ConcurrentDictionary<int, int> socketMap = null;
        public static TUserSession[] g_SessionArray;
        public static int nSessionCount = 0;
        public static bool boServerReady = false;
        public static IList<string> ClientSockeMsgList;


        public static void LoadBlockIPFile()
        {
            StringList LoadList;
            string sIPaddr;
            long nIPaddr;
            string sFileName = ".\\BlockIPList.txt";
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
                    nIPaddr = HUtil32.IpToInt(sIPaddr);
                    if (nIPaddr == 0)
                    {
                        continue;
                    }
                    var IPaddr = new TSockaddr();
                    IPaddr.nIPaddr = nIPaddr;
                    BlockIPList.Add(IPaddr);
                }
                LoadList = null;
            }
        }

        public static void MainOutMessage(string sMsg, int nMsgLevel)
        {
            string tMsg = "[" + DateTime.Now.ToString() + "] " + sMsg;
            MainLogMsgList.Add(tMsg);
        }

        public static void SaveBlockIPList()
        {
            StringList SaveList = new StringList();
            for (var i = 0; i < BlockIPList.Count; i++)
            {
                //SaveList.Add(inet_ntoa(TInAddr(((BlockIPList[i]) as TSockaddr).nIPaddr)));
            }
            SaveList.SaveToFile(".\\BlockIPList.txt");
            SaveList = null;
        }

        public static void Initialization()
        {
            g_SessionArray = new TUserSession[GATEMAXSESSION];
            MainLogMsgList = new List<string>();
            socketMap = new ConcurrentDictionary<int, int>();
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
}

