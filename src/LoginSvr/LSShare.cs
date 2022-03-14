using System;
using System.Collections.Generic;
using System.Net.Sockets;
using SystemModule.Common;

namespace LoginSvr
{
    public class LSShare
    {
        public static int nOnlineCountMin = 0;
        public static int nOnlineCountMax = 0;
        public static int nSessionIdx = 0;
        public static int nVersionDate = 20011006;
        public static string[] ServerAddr = new string[200];

        /// <summary>
        /// 检查账号是否符合规则
        /// </summary>
        /// <param name="sName"></param>
        /// <returns></returns>
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
            nSessionIdx++;
            if (nSessionIdx >= Int32.MaxValue)
            {
                nSessionIdx = 2;
            }
            return nSessionIdx;
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
            SaveList.SaveToFile(".\\!addrtable.txt");
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

        public static string GenSpaceString(string sStr, int nSpaceCOunt)
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
            nSessionIdx = 1;
        }
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

    public class TGateInfo
    {
        public Socket Socket;
        public string sIPaddr;
        public IList<TUserInfo> UserList;
        public long dwKeepAliveTick;
    }

    public class TUserInfo
    {
        public string sAccount;
        public string sUserIPaddr;
        public string sGateIPaddr;
        public int sSockIndex;
        public int nVersionDate;
        public bool boCertificationOK;
        public int nSessionID;
        public bool boPayCost;
        public int nIDDay;
        public int nIDHour;
        public int nIPDay;
        public int nIPHour;
        public DateTime dtDateTime;
        public bool boSelServer;
        public Socket Socket;
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

