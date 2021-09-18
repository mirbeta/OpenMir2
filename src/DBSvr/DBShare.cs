using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using SystemModule;
using SystemModule.Common;

namespace DBSvr
{
    public class TQuickID
    {
        public int nSelectID;
        public string sAccount;
        public int nIndex;
        public string sChrName;
    }

    public class THumInfo
    {
        public TRecordHeader Header;
        public string sChrName;
        public string sAccount;
        public bool boDeleted;
        public bool boSelected;
        public DateTime dModDate;
        public byte btCount;
        public byte[] unknown2;
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
        public string sText;
        public IList<TUserInfo> UserList;
        public long dwTick10;
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
        public string s2C;
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

namespace DBSvr
{
    public class DBShare
    {
        public static string sHumDBFilePath = ".\\FDB\\";
        public static string sDataDBFilePath = ".\\FDB\\";
        public static string sFeedPath = ".\\FDB\\";
        public static string sBackupPath = ".\\FDB\\";
        public static string sConnectPath = ".\\Connects\\";
        public static string sLogPath = ".\\Log\\";
        public static int nServerPort = 6000;
        public static string sServerAddr = "0.0.0.0";
        public static int g_nGatePort = 5100;
        public static string g_sGateAddr = "0.0.0.0";
        public static int nIDServerPort = 5600;
        public static string sIDServerAddr = "127.0.0.1";
        public static bool g_boEnglishNames = false;
        public static bool boViewHackMsg = false;
        public static object HumDB_CS = null;
        public static int n4ADAE4 = 0;
        public static int n4ADAE8 = 0;
        public static int n4ADAEC = 0;
        public static int n4ADAF0 = 0;
        public static bool boDataDBReady = false;
        public static int n4ADAFC = 0;
        public static int n4ADB00 = 0;
        public static int n4ADB04 = 0;
        public static bool boHumDBReady = false;
        public static int n4ADBF4 = 0;
        public static int n4ADBF8 = 0;
        public static int n4ADBFC = 0;
        public static int n4ADC00 = 0;
        public static int n4ADC04 = 0;
        public static bool boAutoClearDB = false;
        public static int g_nQueryChrCount = 0;
        public static int nHackerNewChrCount = 0;
        public static int nHackerDelChrCount = 0;
        public static int nHackerSelChrCount = 0;
        public static int n4ADC1C = 0;
        public static int n4ADC20 = 0;
        public static int n4ADC24 = 0;
        public static int n4ADC28 = 0;
        public static int n4ADC2C = 0;
        public static int n4ADB10 = 0;
        public static int n4ADB14 = 0;
        public static int n4ADB18 = 0;
        public static int n4ADBB8 = 0;
        public static bool bo4ADB1C = false;
        public static string sServerName = "SKY引擎";
        public static string sDBName = "HeroDB";
        public static string sConfFileName = ".\\Dbsrc.ini";
        public static string sGateConfFileName = ".\\!ServerInfo.txt";
        public static string sServerIPConfFileNmae = ".\\!AddrTable.txt";
        public static string sGateIDConfFileName = ".\\SelectID.txt";
        public static string sMapFile = String.Empty;
        public static StringList DenyChrNameList = null;
        public static StringList ServerIPList = null;
        public static ArrayList GateIDList = null;
        public static long dwInterval = 3000;
        // 清理时间间隔长度
        public static int nLevel1 = 1;
        // 清理等级 1
        public static int nLevel2 = 7;
        // 清理等级 2
        public static int nLevel3 = 14;
        // 清理等级 3
        public static int nDay1 = 14;
        // 清理未登录天数 1
        public static int nDay2 = 62;
        // 清理未登录天数 2
        public static int nDay3 = 124;
        // 清理未登录天数 3
        public static int nMonth1 = 0;
        // 清理未登录月数 1
        public static int nMonth2 = 0;
        // 清理未登录月数 2
        public static int nMonth3 = 0;
        // 清理未登录月数 3
        public static int g_nClearRecordCount = 0;
        public static int g_nClearIndex = 0;
        public static int g_nClearCount = 0;
        public static int g_nClearItemIndexCount = 0;
        public static bool boOpenDBBusy = false;
        public static long g_dwGameCenterHandle = 0;
        public static bool g_boDynamicIPMode = false;
        public static StringList g_ClearMakeIndex = null;
        public static TRouteInfo[] g_RouteInfo = new TRouteInfo[20];
        public static bool boDenyChrName = true;
        public static int nDELMaxLevel = 30;// 禁示删除大于指定等级的人物
        public static string DBConnection = "server=10.10.0.199;uid=root;pwd=123456;database=Mir2;";


        public static void LoadGateID()
        {
            StringList LoadList;
            string sLineText;
            string sID = string.Empty;
            string sIPaddr = string.Empty;
            int nID;
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
                    nID = HUtil32.Str_ToInt(sID, -1);
                    if (nID < 0)
                    {
                        continue;
                    }
                    //GateIDList.Add(sIPaddr, ((nID) as Object));
                }
                //LoadList.Free;
            }
        }

        public static short GetGateID(string sIPaddr)
        {
            short result = 0;
            for (var I = 0; I < GateIDList.Count; I++)
            {
                if (GateIDList[I] == sIPaddr)
                {
                    //result = ((int)GateIDList.Values[I]);
                    break;
                }
            }
            return result;
        }

        public static void LoadIPTable()
        {
            ServerIPList.Clear();
            try
            {
                ServerIPList.LoadFromFile(sServerIPConfFileNmae);
            }
            catch
            {
                OutMainMessage("加载IP列表文件 " + sServerIPConfFileNmae + " 出错！！！");
            }
        }

        public static void LoadConfig()
        {
            int LoadInteger;
            IniFile Conf = new IniFile(sConfFileName);
            if (Conf != null)
            {
                sDataDBFilePath = Conf.ReadString("DB", "Dir", sDataDBFilePath);
                sHumDBFilePath = Conf.ReadString("DB", "HumDir", sHumDBFilePath);
                sFeedPath = Conf.ReadString("DB", "FeeDir", sFeedPath);
                sBackupPath = Conf.ReadString("DB", "Backup", sBackupPath);
                sConnectPath = Conf.ReadString("DB", "ConnectDir", sConnectPath);
                sLogPath = Conf.ReadString("DB", "LogDir", sLogPath);
                nServerPort = Conf.ReadInteger("Setup", "ServerPort", nServerPort);
                sServerAddr = Conf.ReadString("Setup", "ServerAddr", sServerAddr);
                g_nGatePort = Conf.ReadInteger("Setup", "GatePort", g_nGatePort);
                g_sGateAddr = Conf.ReadString("Setup", "GateAddr", g_sGateAddr);
                sIDServerAddr = Conf.ReadString("Server", "IDSAddr", sIDServerAddr);
                nIDServerPort = Conf.ReadInteger("Server", "IDSPort", nIDServerPort);
                boViewHackMsg = Conf.ReadBool("Setup", "ViewHackMsg", boViewHackMsg);
                sServerName = Conf.ReadString("Setup", "ServerName", sServerName);
                sDBName = Conf.ReadString("Setup", "DBName", sDBName);
                boDenyChrName = Conf.ReadBool("Setup", "DenyChrName", boDenyChrName);
                nDELMaxLevel = Conf.ReadInteger("Setup", "DELMaxLevel", nDELMaxLevel);// 禁示删除大于指定等级的人物
                dwInterval = Conf.ReadInteger<long>("DBClear", "Interval", dwInterval);
                nLevel1 = Conf.ReadInteger("DBClear", "Level1", nLevel1);
                nLevel2 = Conf.ReadInteger("DBClear", "Level2", nLevel2);
                nLevel3 = Conf.ReadInteger("DBClear", "Level3", nLevel3);
                nDay1 = Conf.ReadInteger("DBClear", "Day1", nDay1);
                nDay2 = Conf.ReadInteger("DBClear", "Day2", nDay2);
                nDay3 = Conf.ReadInteger("DBClear", "Day3", nDay3);
                nMonth1 = Conf.ReadInteger("DBClear", "Month1", nMonth1);
                nMonth2 = Conf.ReadInteger("DBClear", "Month2", nMonth2);
                nMonth3 = Conf.ReadInteger("DBClear", "Month3", nMonth3);
                LoadInteger = Conf.ReadInteger("Setup", "DynamicIPMode", -1);
                if (LoadInteger < 0)
                {
                    Conf.WriteBool("Setup", "DynamicIPMode", g_boDynamicIPMode);
                }
                else
                {
                    g_boDynamicIPMode = LoadInteger == 1;
                }
                g_boEnglishNames = Conf.ReadBool("Setup", "EnglishNameOnly", g_boEnglishNames);
                //Conf.Free;
            }
            LoadIPTable();
            LoadGateID();
        }

        public static bool CheckChrName(string sChrName)
        {
            bool result;
            char Chr;
            bool boIsTwoByte;
            char FirstChr;
            result = true;
            boIsTwoByte = false;
            FirstChr = '\0';
            for (var i = 0; i <= sChrName.Length; i++)
            {
                Chr = sChrName[i];
                if (boIsTwoByte)
                {
                    if (!((FirstChr <= '÷') && (Chr >= '@') && (Chr <= 't')))
                    {
                        if (!((FirstChr > '÷') && (Chr >= '@') && (Chr <= '?')))
                        {
                            result = false;
                        }
                    }
                    boIsTwoByte = false;
                }
                else
                {
                    if ((Chr >= '?') && (Chr <= 't'))
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

        public static void OutMainMessage(string sMsg)
        {
            WriteLogMsg(sMsg);
        }

        public static void WriteLogMsg(string sMsg)
        {
            Console.WriteLine(sMsg);
        }

        public static bool CheckServerIP(string sIP)
        {
            bool result;
            int i;
            result = false;
            for (i = 0; i < ServerIPList.Count; i++)
            {
                if ((sIP).ToLower().CompareTo((ServerIPList[i]).ToLower()) == 0)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        public static void MainOutMessage(string sMsg)
        {
            Console.WriteLine(sMsg);
        }

        public void initialization()
        {
            //InitializeCriticalSection(HumDB_CS);
            //DenyChrNameList = new object();
            //ServerIPList = new object();
            //GateIDList = new object();
            //g_ClearMakeIndex = new object();
        }

        public void finalization()
        {
            //DenyChrNameList.Free;
            //ServerIPList.Free;
            //GateIDList.Free;
            //g_ClearMakeIndex.Free;
        }

    }
}