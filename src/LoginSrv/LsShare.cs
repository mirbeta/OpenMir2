using LoginSrv.Conf;
using System.Collections.Generic;
using System.Net.Sockets;
using SystemModule.Common;
using SystemModule.Packets.ClientPackets;

namespace LoginSrv
{
    public static class LsShare
    {
        public static bool ShowLog = true;
        public static int OnlineCountMin = 0;
        public static int OnlineCountMax = 0;
        public static int SessionIdx = 0;
        public static readonly int VersionDate = 20011006;
        public static IList<CertUser> CertList = new List<CertUser>();
        public static string[] ServerAddr = new string[200];

        /// <summary>
        /// 验证账号是否符合规则
        /// </summary>
        /// <returns></returns>
        public static bool VerifyAccountRule(string account)
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
            for (var i = 0; i < Config.RouteCount; i++)
            {
                sC = GenSpaceString(Config.GateRoute[i].ServerName, 15) +
                     GenSpaceString(Config.GateRoute[i].Title, 15) +
                     GenSpaceString(Config.GateRoute[i].RemoteAddr, 17) +
                     GenSpaceString(Config.GateRoute[i].PublicAddr, 17);
                n8 = 0;
                while (true)
                {
                    s10 = Config.GateRoute[i].Gate[n8].sIPaddr;
                    if (string.IsNullOrEmpty(s10))
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
            for (var i = 0; i < Config.RouteCount; i++)
            {
                if (Config.GateRoute[i].RemoteAddr == sGateIP)
                {
                    result = Config.GateRoute[i].PublicAddr;
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
        public int SessionId;
        public string SoketId;
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
        public int PayModel;
        public long PlayTime;
        public UserEntry UserEntry;
        public UserEntryAdd UserEntryAdd;
    }

    public struct TGateNet
    {
        public string sIPaddr;
        public int nPort;
        public bool boEnable;
    }

    public struct GateRoute
    {
        public string ServerName;
        public string Title;
        public string RemoteAddr;
        public string PublicAddr;
        public int nSelIdx;
        public TGateNet[] Gate;

        public GateRoute()
        {
            Gate = new TGateNet[10];
        }
    }

    public class SessionConnInfo
    {
        public string Account;
        public string IPaddr;
        public string ServerName;
        public int SessionID;
        public bool boPayCost;
        public bool IsPayMent;
        public byte PayMenMode;
        public long KickTick;
        public long StartTick;
        public bool Kicked;
        public int nLockCount;
    }

    public class LoginGateInfo
    {
        public Socket Socket;
        public string ConnectionId;
        public string sIPaddr;
        public byte[] Data;
        public int DataLen;
        public IList<UserInfo> UserList;
        public long KeepAliveTick;

        public LoginGateInfo()
        {
            Data = new byte[1024 * 10];
        }
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
        public int Certification;
        public long Seconds;
        public byte AvailableType;
        public bool SelServer;
        public Socket Socket;
        public long ClientTick;
        public long LastCreateAccountTick;
        public long LastUpdatePwdTick;
        public long LastGetBackPwdTick;
        public long LastUpdateAccountTick;
    }
}