using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using SystemModule;
using SystemModule.Common;

namespace DBSvr
{
    public class DBShare
    {
        public static readonly string GateConfFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ServerInfo.txt");
        private static readonly string ServerIpConfFileNmae = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AddrTable.txt");
        private static readonly string GateIdConfFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "SelectID.txt");
        public static StringList DenyChrNameList = null;
        private static Hashtable _serverIpList = null;
        private static Dictionary<string, short> _gateIdList = null;
        public static readonly StringList ClearMakeIndex = null;
        public static readonly TRouteInfo[] RouteInfo = new TRouteInfo[20];
        public static bool ShowLog = true;

        private static void LoadGateID()
        {
            string sID = string.Empty;
            string sIPaddr = string.Empty;
            _gateIdList.Clear();
            if (File.Exists(GateIdConfFileName))
            {
                var LoadList = new StringList();
                LoadList.LoadFromFile(GateIdConfFileName);
                for (var i = 0; i < LoadList.Count; i++)
                {
                    var sLineText = LoadList[i];
                    if ((sLineText == "") || (sLineText[0] == ';'))
                    {
                        continue;
                    }
                    sLineText = HUtil32.GetValidStr3(sLineText, ref sID, new string[] { " ", "\09" });
                    sLineText = HUtil32.GetValidStr3(sLineText, ref sIPaddr, new string[] { " ", "\09" });
                    int nID = HUtil32.StrToInt(sID, -1);
                    if (nID < 0)
                    {
                        continue;
                    }
                    _gateIdList.Add(sIPaddr, (short)nID);
                }
                LoadList = null;
            }
        }

        public static short GetGateID(string sIPaddr)
        {
            short result = 0;
            if (_gateIdList.ContainsKey(sIPaddr))
            {
                return _gateIdList[sIPaddr];
            }
            return result;
        }

        private static void LoadIPTable()
        {
            _serverIpList.Clear();
            try
            {
                var stringList = new StringList();
                stringList.LoadFromFile(ServerIpConfFileNmae);
                for (var i = 0; i < stringList.Count; i++)
                {
                    if (_serverIpList.ContainsKey(stringList[i]))
                    {
                        continue;
                    }
                    _serverIpList.Add(stringList[i], stringList[i]);
                }
                stringList = null;
            }
            catch
            {
                //MainOutMessage("加载IP列表文件 " + ServerIpConfFileNmae + " 出错!!!");
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
            for (var i = 0; i < ClearMakeIndex.Count; i++)
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
            if (_serverIpList.ContainsKey(sIP))
            {
                return true;
            }
            return result;
        }

        public static void Initialization()
        {
            DenyChrNameList = new StringList();
            _serverIpList = new Hashtable();
            _gateIdList = new Dictionary<string, short>();
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

    public class GlobaSessionInfo
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
        public EndPoint RemoteEndPoint;
        public EndPoint LoclEndPoint;
        public IList<TUserInfo> UserList;
        /// <summary>
        /// 网关ID
        /// </summary>
        public short nGateID;
        public int ConnectTick;

        public (string serverIp, string Status, string playCount, string reviceTotal, string sendTotal, string queueCount) GetStatus()
        {
            return (RemoteEndPoint.ToString(), GetConnected(), UserList.Count.ToString(), "", "", "");
        }

        private string GetConnected()
        {
            return Socket.Connected ? "[green]Connected[/]" : "[red]Not Connected[/]";
        }
    }

    public class TUserInfo
    {
        public string sAccount;
        public string sUserIPaddr;
        public string sGateIPaddr;
        public int sConnID;
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
}