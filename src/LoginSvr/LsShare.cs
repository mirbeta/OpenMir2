using LoginSvr.Conf;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using SystemModule.Common;
using SystemModule.Packet.ClientPackets;

namespace LoginSvr
{
    public class LsShare
    {
        public static bool ShowLog = true;
        public static int OnlineCountMin = 0;
        public static int OnlineCountMax = 0;
        public static int SessionIdx = 0;
        public static readonly int VersionDate = 20011006;
        public static IList<GateInfo> Gates = new List<GateInfo>();
        public static IList<CertUser> CertList = new List<CertUser>();
        public static string[] ServerAddr = new string[200];

        /// <summary>
        /// 检查账号是否符合规则
        /// </summary>
        /// <returns></returns>
        public static bool CheckAccountName(string account)
        {
            if (string.IsNullOrEmpty(account))
            {
                return false;
            }
            var result = true;
            var nLen = account.Length;
            var i = 0;
            while (true)
            {
                if (i >= nLen)
                {
                    break;
                }
                if ((account[i] < '0') || (account[i] > 'z'))
                {
                    result = false;
                    if ((account[i] >= '°') && (account[i] <= 'è'))
                    {
                        i++;
                        if (i <= nLen)
                        {
                            if ((account[i] >= '?') && (account[i] <= 't'))
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

        public static int GetSessionId()
        {
            SessionIdx++;
            if (SessionIdx >= int.MaxValue)
            {
                SessionIdx = 2;
            }
            return SessionIdx;
        }

        public static void SaveGateConfig(Config Config)
        {
            int n8;
            string s10;
            string sC;
            StringList SaveList = new StringList();
            SaveList.Add(";No space allowed");
            SaveList.Add(GenSpaceString(";Server", 15) + GenSpaceString("Title", 15) + GenSpaceString("Remote", 17) + GenSpaceString("Public", 17) + "Gate...");
            for (var i = 0; i < Config.nRouteCount; i++)
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
            SaveList.SaveToFile("addrtable.txt");
            SaveList = null;
        }

        public static string GetGatePublicAddr(Config Config, string sGateIP)
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

        private static string GenSpaceString(string sStr, int nSpaceCOunt)
        {
            string result = sStr + " ";
            for (var i = 0; i <= nSpaceCOunt - sStr.Length; i++)
            {
                result = result + " ";
            }
            return result;
        }

        public static void Initialization()
        {
            SessionIdx = 1;
        }
    }

    public struct UserSessionData
    {
        public UserInfo UserInfo;
        public string Msg;
    }

    public struct AccountQuick
    {
        public string Account;
        public int Index;

        public AccountQuick(string account, int index)
        {
            Account = account;
            Index = index;
        }
    }

    public class AccountRecord
    {
        public int AccountId;
        public int ErrorCount;
        public int ActionTick;
        public UserEntry UserEntry;
        public UserEntryAdd UserEntryAdd;
    }

    public struct TGateNet
    {
        public string sIPaddr;
        public int nPort;
        public bool boEnable;
    }

    public class TGateRoute
    {
        public string sServerName;
        public string sTitle;
        public string sRemoteAddr;
        public string sPublicAddr;
        public int nSelIdx;
        public TGateNet[] Gate;

        public TGateRoute()
        {
            Gate = new TGateNet[10];
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

    public class GateInfo
    {
        public Socket Socket;
        public string sIPaddr;
        public IList<UserInfo> UserList;
        public long dwKeepAliveTick;
    }
    
    public struct CertUser
    {
        public string LoginID;
        public string Addr;
        public string ServerName;
        public bool FreeMode;
        public int Certification;
        public long OpenTime;
        public bool Closing;
        public byte AvailableType;
        public long IDDay;
        public long IDHour;
        public long IPDay;
        public long IPHour;
    }

    public class UserInfo
    {
        public string Account;
        public string UserIPaddr;
        public string GateIPaddr;
        public string SockIndex;
        public int nVersionDate;
        public bool boCertificationOK;
        public int SessionID;
        /// <summary>
        /// 付费账号
        /// </summary>
        public bool PayCost;
        /// <summary>
        /// 0 : 体验版
        /// 1 : 收费
        /// 2 : 免费（创建时间）
        /// </summary>
        public byte PayMode;
        public int nClientVersion;
        public int nCertification;
        public long nPassFailTime;
        public int nPassFailCount;
        public bool bVersionAccept;
        public bool bSelServerOk;
        public long dwValidFrom;
        public long dwValidUntil;
        public long dwSeconds;
        public long dwIpValidFrom;
        public long dwIpValidUntil;
        public long dwIpSeconds;
        public long dwStopUntil;
        public long dwMakeTime;
        public long dwOpenTime;
        public byte nAvailableType;
        public bool bFreeMode;
        public int nServerID;
        public int nParentCheck;
        public DateTime AccountMakeDate;
        public string SocData;
        public long dwLatestCmdTime;
        
        /// <summary>
        /// 剩余游戏时间
        /// </summary>
        public int PlayTime;
        /// <summary>
        /// 授权游戏时间
        /// </summary>
        public int AuthorizedTime;
        /// <summary>
        /// 剩余多少天
        /// </summary>
        public int IDDay;
        /// <summary>
        /// 剩余多少小时
        /// </summary>
        public int IDHour;
        /// <summary>
        /// IP剩余多少天
        /// </summary>
        public int IPDay;
        /// <summary>
        /// IP剩余多少小时
        /// </summary>
        public int IPHour;
        public DateTime dtDateTime;
        public bool SelServer;
        public Socket Socket;
        public long ClientTick;
        public GateInfo Gate;
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