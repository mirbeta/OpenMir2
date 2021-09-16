using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using SystemModule;
using SystemModule.Common;

namespace LoginGate
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
        public static object CS_MainLog = null;
        public static object CS_FilterMsg = null;
        public static IList<string> MainLogMsgList = null;
        public static IList<TSockaddr> BlockIPList = null;
        public static IList<TSockaddr> TempBlockIPList = null;
        public static IList<TSockaddr> CurrIPaddrList = null;
        public static int nIPCountLimit1 = 20;
        public static int nIPCountLimit2 = 40;
        public static int nShowLogLevel = 3;
        public static IList<string> StringList456A14 = null;
        public static string GateClass = "LoginGate";
        public static string GateName = "登录网关";
        public static string TitleName = "SKY引擎";
        public static int ServerPort = 5500;
        public static string ServerAddr = "127.0.0.1";
        public static int GatePort = 7000;
        public static string GateAddr = "0.0.0.0";
        public static bool boGateReady = false;
        public static bool boShowMessage = false;
        public static bool boStarted = false;
        public static bool boClose = false;
        public static bool boServiceStart = false;
        public static long dwKeepAliveTick = 0;
        public static bool boKeepAliveTimcOut = false;
        public static int nSendMsgCount = 0;
        public static int n456A2C = 0;
        public static int n456A30 = 0;
        public static bool boSendHoldTimeOut = false;
        public static long dwSendHoldTick = 0;
        public static bool boDecodeLock = false;
        public static int nMaxConnOfIPaddr = 10;
        public static TBlockIPMethod BlockMethod = TBlockIPMethod.mDisconnect;
        public static long dwKeepConnectTimeOut = 60 * 1000;
        /// <summary>
        /// 用于动态IP，分机放置登录网关用，打开此模式后，网关将会把连接登录服务器的IP地址，当为服务器IP，发给登录服务器，客户端将直接使用此IP连接角色网关
        /// </summary>
        public static bool g_boDynamicIPDisMode = false;
        public static long g_dwGameCenterHandle = 0;
        public static string g_sNowStartGate = "正在启动登录前置服务器...";
        public static string g_sNowStartOK = "启动登录前置服务器完成...";
        public static int GATEMAXSESSION = 10000;
        public static ConcurrentDictionary<string, int> socketMap = null;
        
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

        public void initialization()
        {
            CS_MainLog = new object();
            CS_FilterMsg = new object();
            StringList456A14 = new List<string>();
            MainLogMsgList = new List<string>();
            socketMap = new ConcurrentDictionary<string, int>();
        }
    } 
}

