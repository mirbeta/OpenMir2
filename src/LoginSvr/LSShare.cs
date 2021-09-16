using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using SystemModule.Common;

namespace LoginSvr
{
    public class LSShare
    {
        public static TConfig g_Config;
        public static IList<long> StringList_0 = null;
        public static int nOnlineCountMin = 0;
        public static int nOnlineCountMax = 0;
        public static IList<string> g_MainMsgList = null;
        public static int nSessionIdx = 0;
        public static bool g_boDataDBReady = false;
        public static bool bo470D20 = false;
        public static int nVersionDate = 20011006;
        public static string[] ServerAddr = new string[16];
        public static string DBConnection = "server=10.10.0.199;uid=root;pwd=123456;database=Mir2;";
        public static int UserLimit = ushort.MaxValue;

        public static int GetCodeMsgSize(double X)
        {
            int result;
            if (Convert.ToInt32(X) < X)
            {
                result = Convert.ToInt32(X) + 1;
            }
            else
            {
                result = Convert.ToInt32(X);
            }

            return result;
        }

        public static bool CheckAccountName(string sName)
        {
            bool result = false;
            if (string.IsNullOrEmpty(sName))
            {
                return result;
            }
            result = true;
            int nLen = sName.Length;
            var i = 0;
            while (true)
            {
                if (i >= nLen)
                {
                    break;
                }
                if ((sName[i] < '0') || (sName[i] > 'z'))
                {
                    result = false;
                    if ((sName[i] >= '°') && (sName[i] <= 'è'))
                    {
                        i++;
                        if (i <= nLen)
                        {
                            if ((sName[i] >= '?') && (sName[i] <= 't'))
                            {
                                result = true;
                            }
                        }
                    }
                    if (!result)
                    {
                        break;
                    }
                }
                i++;
            }
            return result;
        }

        public static int GetSessionID()
        {
            int result;
            nSessionIdx++;
            if (nSessionIdx >= Int32.MaxValue)
            {
                nSessionIdx = 2;
            }

            result = nSessionIdx;
            return result;
        }

        public static void SaveGateConfig(TConfig Config)
        {
            StringList SaveList;
            int i;
            int n8;
            string s10;
            string sC;
            SaveList = new StringList();
            SaveList.Add(";No space allowed");
            SaveList.Add(GenSpaceString(";Server", 15) + GenSpaceString("Title", 15) + GenSpaceString("Remote", 17) +
                         GenSpaceString("Public", 17) + "Gate...");
            for (i = 0; i < Config.nRouteCount; i++)
            {
                sC = GenSpaceString(Config.GateRoute[i].sServerName, 15) +
                     GenSpaceString(Config.GateRoute[i].sTitle, 15) +
                     GenSpaceString(Config.GateRoute[i].sRemoteAddr, 17) +
                     GenSpaceString(Config.GateRoute[i].sPublicAddr, 17);
                n8 = 0;
                while (true)
                {
                    s10 = Config.GateRoute[i].Gate[n8].sIPaddr;
                    if (s10 == "")
                    {
                        break;
                    }
                    if (!Config.GateRoute[i].Gate[n8].boEnable)
                    {
                        s10 = "*" + s10;
                    }
                    s10 = s10 + ":" + (Config.GateRoute[i].Gate[n8].nPort).ToString();
                    sC = sC + GenSpaceString(s10, 17);
                    n8++;
                    if (n8 >= 10)
                    {
                        break;
                    }
                }
                SaveList.Add(sC);
            }
            SaveList.SaveToFile(".\\!addrtable.txt");
            SaveList = null;
        }

        public static string GetGatePublicAddr(TConfig Config, string sGateIP)
        {
            string result = sGateIP;
            for (var i = 0; i < Config.nRouteCount; i++)
            {
                if (Config.GateRoute[i].sRemoteAddr == sGateIP)
                {
                    result = Config.GateRoute[i].sPublicAddr;
                    break;
                }
            }

            return result;
        }

        public static string GenSpaceString(string sStr, int nSpaceCOunt)
        {
            string result = sStr + " ";
            for (var i = 1; i <= nSpaceCOunt - sStr.Length; i++)
            {
                result = result + " ";
            }

            return result;
        }

        public static void MainOutMessage(string sMsg)
        {
            g_MainMsgList.Add(sMsg);
        }

        public static void initialization()
        {
            g_MainMsgList = new List<string>();
            g_Config = new TConfig();
        }

        public void finalization()
        {
            g_MainMsgList = null;
        }

    }

    public struct TGateNet
    {
        public string sIPaddr;
        public int nPort;
        public bool boEnable;
    }

    public struct TGateRoute
    {
        public string sServerName;
        public string sTitle;
        public string sRemoteAddr;
        public string sPublicAddr;
        public int nSelIdx;
        public TGateNet[] Gate;
    }

    public class TConfig
    {
        public IniFile IniConf;
        public bool boRemoteClose;
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
        public string sIdDir;
        public string sWebLogDir;
        public string sFeedIDList;
        public string sFeedIPList;
        public string sCountLogDir;
        public string sChrLogDir;
        public bool boTestServer;
        /// <summary>
        /// 是否允许创建账号
        /// </summary>
        public bool boEnableMakingID;
        public bool boDynamicIPMode;
        public int nReadyServers;
        public object GateCriticalSection;
        public IList<TGateInfo> GateList;
        public IList<TConnInfo> SessionList;
        public IList<string> ServerNameList;
        public Dictionary<string, int> AccountCostList;
        public Dictionary<string, int> IPaddrCostList;
        public bool boShowDetailMsg;
        public int dwProcessGateTick;
        public int dwProcessGateTime;
        public int nRouteCount;
        public TGateRoute[] GateRoute;

        public TConfig()
        {
            boRemoteClose = false;
            sDBServer = "10.10.0.168";
            nDBSPort = 16300;
            sFeeServer = "10.10.0.168";
            nFeePort = 16301;
            sLogServer = "10.10.0.168";
            nLogPort = 16301;
            sGateAddr = "10.10.0.168";
            nGatePort = 5500;
            sServerAddr = "10.10.0.168";
            nServerPort = 5600;
            sMonAddr = "10.10.0.168";
            nMonPort = 3000;
            sIdDir = ".\\DB\\";
            sWebLogDir = ".\\Share\\";
            sFeedIDList = ".\\FeedIDList.txt";
            sFeedIPList = ".\\FeedIPList.txt";
            sCountLogDir = ".\\CountLog\\";
            sChrLogDir = ".\\ChrLog\\";
            boTestServer = true;
            boEnableMakingID = true;
            boDynamicIPMode = false;
            nReadyServers = 0;
            boShowDetailMsg = false;
        }
    }

    public class TConnInfo
    {
        public string sAccount;
        public string sIPaddr;
        public string sServerName;
        public int nSessionID;
        public bool boPayCost;
        public bool bo11;
        public long dwKickTick;
        public long dwStartTick;
        public bool boKicked;
        public int nLockCount;
    }

    public class TGateInfo
    {
        public Socket Socket;
        public string sIPaddr;
        public string sReceiveMsg;
        public IList<TUserInfo> UserList;
        public long dwKeepAliveTick;
    }

    public class TUserInfo
    {
        public string sAccount;
        public string sUserIPaddr;
        public string sGateIPaddr;
        public string sSockIndex;
        public int nVersionDate;
        public bool boCertificationOK;
        public bool bo29;
        public bool bo2A;
        public bool bo2B;
        public int nSessionID;
        public bool boPayCost;
        public int nIDDay;
        public int nIDHour;
        public int nIPDay;
        public int nIPHour;
        public DateTime dtDateTime;
        public bool boSelServer;
        public bool bo51;
        public Socket Socket;
        public string sReceiveMsg;
        public long dwTime5C;
        public bool bo60;
        public bool bo61;
        public bool bo62;
        public bool bo63;
        public long dwClientTick;
        public TGateInfo Gate;
    }

    public class AccountConst
    {
        public string s1C;
        public int nC;

        public AccountConst(string s1c, int nC)
        {
            this.s1C = s1c;
            this.nC = nC;
        }
    }

}

