using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using SystemModule;
using SystemModule.Common;

namespace DBSvr
{
    public class DBShare
    {
        public static int nServerPort = 6000;
        public static string sServerAddr = "*";
        public static int g_nGatePort = 5100;
        public static string g_sGateAddr = "*";
        public static int nIDServerPort = 5600;
        public static string sIDServerAddr = "127.0.0.1";
        public static bool g_boEnglishNames = false;
        public static string sServerName = "热血传奇";
        public static string sGateConfFileName = "!ServerInfo.txt";
        private static string sServerIPConfFileNmae = "!AddrTable.txt";
        private static string sGateIDConfFileName = "SelectID.txt";
        public static string sMapFile = string.Empty;
        public static StringList DenyChrNameList = null;
        private static Hashtable ServerIPList = null;
        private static Dictionary<string, short> GateIDList = null;
        public static int dwInterval = 3000;
        public static bool g_boDynamicIPMode = false;
        public static StringList g_ClearMakeIndex = null;
        public static TRouteInfo[] g_RouteInfo = new TRouteInfo[20];
        /// <summary>
        /// 是否禁止检测玩家名字
        /// </summary>
        public static bool boDenyChrName = true;
        public static int nDELMaxLevel = 30;
        public static string DBConnection = "server=127.0.0.1;uid=root;pwd=;database=Mir2;";

        private static void LoadGateID()
        {
            StringList LoadList;
            string sLineText;
            string sID = string.Empty;
            string sIPaddr = string.Empty;
            GateIDList.Clear();
            if (File.Exists(sGateIDConfFileName))
            {
                LoadList = new StringList();
                LoadList.LoadFromFile(sGateIDConfFileName);
                for (var i = 0; i < LoadList.Count; i++)
                {
                    sLineText = LoadList[i];
                    if ((sLineText == "") || (sLineText[0] == ';'))
                    {
                        continue;
                    }
                    sLineText = HUtil32.GetValidStr3(sLineText, ref sID, new string[] { " ", "\09" });
                    sLineText = HUtil32.GetValidStr3(sLineText, ref sIPaddr, new string[] { " ", "\09" });
                    int nID = HUtil32.Str_ToInt(sID, -1);
                    if (nID < 0)
                    {
                        continue;
                    }
                    GateIDList.Add(sIPaddr, (short)nID);
                }
                LoadList = null;
            }
        }

        public static short GetGateID(string sIPaddr)
        {
            short result = 0;
            if (GateIDList.ContainsKey(sIPaddr))
            {
                return GateIDList[sIPaddr];
            }
            return result;
        }

        private static void LoadIPTable()
        {
            ServerIPList.Clear();
            try
            {
                var stringList = new StringList();
                stringList.LoadFromFile(sServerIPConfFileNmae);
                for (var i = 0; i < stringList.Count; i++)
                {
                    if (ServerIPList.ContainsKey(stringList[i]))
                    {
                        continue;
                    }
                    ServerIPList.Add(stringList[i], stringList[i]);
                }
                stringList = null;
            }
            catch
            {
                MainOutMessage("加载IP列表文件 " + sServerIPConfFileNmae + " 出错!!!");
            }
        }

        public static void LoadConfig()
        {
            LoadIPTable();
            LoadGateID();
        }

        public static bool CheckChrName(string sChrName)
        {
            try
            {
                char Chr;
                bool result = true;
                bool boIsTwoByte = false;
                char FirstChr = '\0';
                var bytesCount = 0;
                for (int i = 0; i < sChrName.Length; i++)
                {
                    var bytes = HUtil32.GetByteCount(sChrName[i]);
                    switch (bytes)
                    {
                        case 1:
                            bytesCount++;
                            break;
                        case 2:
                            bytesCount += 2;
                            if (i == sChrName.Length)
                            {
                                result = false;
                            }
                            break;
                    }
                }

                if (bytesCount != sChrName.Length)
                {
                    return false;
                }

                for (var i = 0; i < sChrName.Length; i++)
                {
                    Chr = sChrName[i];
                    if (boIsTwoByte)
                    {
                        if (!((FirstChr <= 0xF7) && (Chr >= 0x40) && (Chr <= 0xFE)))
                        {
                            if (!((FirstChr > 0xF7) && (Chr >= 0x40) && (Chr <= 0xA0)))
                            {
                                result = false;
                            }
                        }
                        boIsTwoByte = false;
                    }
                    else
                    {
                        if ((Chr >= 0x81) && (Chr <= 0xFE))
                        {
                            boIsTwoByte = true;
                            FirstChr = Chr;
                        }
                        else
                        {
                            if (!((Chr >= '0') && (Chr <= '9')) && !((Chr >= 'a') && (Chr <= 'z')) && !((Chr >= 'A') && (Chr <= 'Z')))
                            {
                                result = false;
                            }
                        }
                    }
                    if (!result)
                    {
                        break;
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine($"CheckChrName {sChrName} 异常信息:" + e.Message);
                return false;
            }
        }

        public static bool InClearMakeIndexList(int nIndex)
        {
            bool result = false;
            for (var i = 0; i < g_ClearMakeIndex.Count; i++)
            {
                //if (nIndex == ((int)g_ClearMakeIndex.Values[i]))
                //{
                //    result = true;
                //    break;
                //}
            }
            return result;
        }

        public static bool CheckServerIP(string sIP)
        {
            bool result = false;
            if (ServerIPList.ContainsKey(sIP))
            {
                return true;
            }
            return result;
        }

        public static void MainOutMessage(string sMsg)
        {
            Console.WriteLine(sMsg);
        }

        public static void Initialization()
        {
            DenyChrNameList = new StringList();
            ServerIPList = new Hashtable();
            GateIDList = new Dictionary<string, short>();
        }
    }

    public class TServerInfo
    {
        public int nSckHandle;
        public byte[] Data;
        public int DataLen;
        public Socket Socket;
    }

    public class THumSession
    {
        public string sChrName;
        public int nIndex;
        public Socket Socket;
        public bool bo24;
        public bool bo2C;
        public long lastSessionTick;
    }

    public struct HumRecordData
    {
        public int Id;
        public TRecordHeader Header;
        public string sChrName;
        public string sAccount;
        public bool boDeleted;
        public byte boSelected;
    }

    public class TGlobaSessionInfo
    {
        public string sAccount;
        public string sIPaddr;
        public int nSessionID;
        public long dwAddTick;
        public DateTime dAddDate;
        public bool boLoadRcd;
        public bool boStartPlay;
    }

    public class TGateInfo
    {
        public Socket Socket;
        public string sGateaddr;
        public IList<TUserInfo> UserList;
        public long dwTick10;
        /// <summary>
        /// 网关ID
        /// </summary>
        public short nGateID;
    }

    public class TUserInfo
    {
        public string sAccount;
        public string sUserIPaddr;
        public string sGateIPaddr;
        public string sConnID;
        public int nSessionID;
        public Socket Socket;
        public string sText;
        public bool boChrSelected;
        public bool boChrQueryed;
        public long dwTick34;
        public long dwChrTick;
        public short nSelGateID;
    }

    public class TRouteInfo
    {
        public int nGateCount;
        public string sSelGateIP;
        public string[] sGameGateIP;
        public int[] nGameGatePort;

        public TRouteInfo()
        {
            sGameGateIP = new string[8];
            nGameGatePort = new int[8];
        }
    }

    public class TQueryChr
    {
        public byte btJob;
        public byte btHair;
        public byte btSex;
        public ushort wLevel;
        public string sName;
    }
}