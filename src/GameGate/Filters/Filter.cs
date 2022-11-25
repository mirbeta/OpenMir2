using System.Collections;
using SystemModule;

namespace GameGate.Filters
{
    public class Filter
    {
        public readonly HardwareFilter g_HWIDFilter = null;
        public static object g_ConnectOfIPLock = null;
        public static ArrayList g_ConnectOfIPList = null;
        private static readonly ArrayList g_BlockIPList = null;
        private static readonly ArrayList g_TempBlockIPList = null;
        public static ArrayList g_BlockIPAreaList = null;

        public Filter(HardwareFilter hwidFilter)
        {
            g_HWIDFilter = hwidFilter;
        }

        public static void LoadBlockIPList()
        {
            //int nIP;
            //ArrayList sList = new ArrayList();
            //if (!File.Exists(Protocol.Units.Protocol._STR_BLOCK_FILE))
            //{
            //    sList.SaveToFile(Protocol.Units.Protocol._STR_BLOCK_FILE);
            //}
            //g_BlockIPList.Clear();
            //sList.LoadFromFile(Protocol.Units.Protocol._STR_BLOCK_FILE);
            //for (var i = 0; i < sList.Count; i++)
            //{
            //    if (sList[i] == "")
            //    {
            //        continue;
            //    }
            //    nIP = inet_addr((sList[i] as string));
            //    if (nIP == INADDR_NONE)
            //    {
            //        continue;
            //    }
            //    g_BlockIPList.Add(sList[i], ((nIP) as Object));
            //}
            //sList.Free;
        }

        public static void SaveBlockIPList()
        {
            //ArrayList sList = new ArrayList();
            //for (var i = 0; i < g_BlockIPList.Count; i++)
            //{
            //    if (g_BlockIPList[i] == "")
            //    {
            //        continue;
            //    }
            //    sList.Add(g_BlockIPList[i]);
            //}
            //sList.SaveToFile(Protocol.Units.Protocol._STR_BLOCK_FILE);
            //sList.Free;
        }

        public static void AddToBlockIPList(string szIP)
        {
            var nIP = 0L;
            if (g_BlockIPList.IndexOf(szIP) < 0)
            {
                nIP = HUtil32.IpToInt(szIP);
                if (nIP != 0)
                {
                    //g_BlockIPList.Add(szIP, nIP);
                }
            }
        }

        public static void AddToBlockIPList(int nIP)
        {
            //string pszIP;
            //bool fExists = false;
            //for (var i = 0; i < g_BlockIPList.Count; i++)
            //{
            //    if (((int)g_BlockIPList.Values[i]) == nIP)
            //    {
            //        fExists = true;
            //        break;
            //    }
            //}
            //if (!fExists)
            //{
            //    pszIP = inet_ntoa(TInAddr(nIP));
            //    if (pszIP != null)
            //    {
            //        g_BlockIPList.Add(pszIP, ((nIP) as Object));
            //    }
            //}
        }

        public static void AddToTempBlockIPList(string szIP)
        {
            var nIP = 0L;
            if (g_TempBlockIPList.IndexOf(szIP) < 0)
            {
                nIP = HUtil32.IpToInt(szIP);
                if (nIP != 0)
                {
                    //g_TempBlockIPList.Add(szIP, ((nIP) as Object));
                }
            }
        }

        public static void AddToTempBlockIPList(int nIP)
        {
            //string pszIP;
            //bool fExists = false;
            //for (var i = 0; i < g_TempBlockIPList.Count; i++)
            //{
            //    if (((int)g_TempBlockIPList.Values[i]) == nIP)
            //    {
            //        fExists = true;
            //        break;
            //    }
            //}
            //if (!fExists)
            //{
            //    pszIP = inet_ntoa(TInAddr(nIP));
            //    if (pszIP != null)
            //    {
            //        g_TempBlockIPList.Add(pszIP, ((nIP) as Object));
            //    }
            //}
        }

        public static bool IsBlockIP(int nRemoteIP)
        {
            bool result = false;
            //if (g_BlockIPList.Count > 0)
            //{
            //    for (var i = 0; i < g_BlockIPList.Count; i++)
            //    {
            //        if (nRemoteIP == ((int)g_BlockIPList.Values[i]))
            //        {
            //            result = true;
            //            return result;
            //        }
            //    }
            //}
            //if (g_TempBlockIPList.Count > 0)
            //{
            //    for (var i = 0; i < g_TempBlockIPList.Count; i++)
            //    {
            //        if (nRemoteIP == ((int)g_TempBlockIPList.Values[i]))
            //        {
            //            result = true;
            //            break;
            //        }
            //    }
            //}
            return result;
        }

        public static bool OverConnectOfIP(long Addr)
        {
            bool result = false;
            //TPerIPAddr PerIPAddr;
            //if (!ConfigManager.Units.ConfigManager.g_pConfig.m_fCheckNullSession)
            //{
            //    return result;
            //}
            //EnterCriticalSection(g_ConnectOfIPLock);
            //try {
            //    for (var i = 0; i < g_ConnectOfIPList.Count; i++)
            //    {
            //        PerIPAddr = ((TPerIPAddr)(g_ConnectOfIPList[i]));
            //        if (PerIPAddr.IPaddr == Addr)
            //        {
            //            if (PerIPAddr.Count + 1 > ConfigManager.Units.ConfigManager.g_pConfig.m_nMaxConnectOfIP)
            //            {
            //                result = true;
            //            }
            //            else
            //            {
            //                PerIPAddr.Count++;
            //            }
            //            return result;
            //        }
            //    }
            //    PerIPAddr = new TPerIPAddr();
            //    PerIPAddr.IPaddr = Addr;
            //    PerIPAddr.Count = 1;
            //    g_ConnectOfIPList.Add(PerIPAddr);
            //} finally {
            //    LeaveCriticalSection(g_ConnectOfIPLock);
            //}
            return result;
        }

        public static void DeleteConnectOfIP(long Addr)
        {
            //int i;
            //TPerIPAddr PerIPAddr;
            //if (!ConfigManager.Units.ConfigManager.g_pConfig.m_fCheckNullSession)
            //{
            //    return;
            //}
            //EnterCriticalSection(g_ConnectOfIPLock);
            //try {
            //    for (i = 0; i < g_ConnectOfIPList.Count; i++)
            //    {
            //        PerIPAddr = ((TPerIPAddr)(g_ConnectOfIPList[i]));
            //        if (PerIPAddr.IPaddr == Addr)
            //        {
            //            if (PerIPAddr.Count > 0)
            //            {
            //                PerIPAddr.Count -= 1;
            //            }
            //            if (PerIPAddr.Count <= 0)
            //            {
            //                Dispose(PerIPAddr);
            //                g_ConnectOfIPList.RemoveAt(i);
            //            }
            //            break;
            //        }
            //    }
            //} finally {
            //    LeaveCriticalSection(g_ConnectOfIPLock);
            //}
        }

        public static void ClearConnectOfIP()
        {
            //if (!ConfigManager.Units.ConfigManager.g_pConfig.m_fCheckNullSession)
            //{
            //    return;
            //}
            //EnterCriticalSection(g_ConnectOfIPLock);
            //try {
            //    for (var i = 0; i < g_ConnectOfIPList.Count; i++)
            //    {
            //        Dispose(((TPerIPAddr)(g_ConnectOfIPList[i])));
            //    }
            //    g_ConnectOfIPList.Clear();
            //} finally {
            //    LeaveCriticalSection(g_ConnectOfIPLock);
            //}
        }

        public static void LoadBlockIPAreaList()
        {
            //int i;
            //double dwIPLow;
            //double dwIPHigh;
            //double dwtmp;
            //string szIPArea;
            //string szIPLow;
            //string szIPHigh;
            //TIPArea pIPArea;
            //ArrayList sList;
            //sList = new ArrayList();
            //if (!File.Exists(Protocol.Units.Protocol._STR_BLOCK_AREA_FILE))
            //{
            //    sList.SaveToFile(Protocol.Units.Protocol._STR_BLOCK_AREA_FILE);
            //}
            //g_BlockIPAreaList.Clear();
            //sList.LoadFromFile(Protocol.Units.Protocol._STR_BLOCK_AREA_FILE);
            //for (i = 0; i < sList.Count; i++)
            //{
            //    szIPArea = sList[i];
            //    if (szIPArea == "")
            //    {
            //        continue;
            //    }
            //    szIPHigh = HUtil32.Units.HUtil32.GetValidStr3(szIPArea, ref szIPLow, new string[] { "-" });
            //    dwIPLow = Misc.ReverseIP(inet_addr((szIPLow as string)));
            //    dwIPHigh = Misc.ReverseIP(inet_addr((szIPHigh as string)));
            //    if ((dwIPLow == INADDR_NONE))
            //    {
            //        continue;
            //    }
            //    if ((dwIPHigh == INADDR_NONE))
            //    {
            //        continue;
            //    }
            //    if (dwIPLow > dwIPHigh)
            //    {
            //        dwtmp = dwIPLow;
            //        dwIPLow = dwIPHigh;
            //        dwIPHigh = dwtmp;
            //    }
            //    pIPArea = new TIPArea();
            //    pIPArea.Low = dwIPLow;
            //    pIPArea.High = dwIPHigh;
            //    g_BlockIPAreaList.Add(szIPArea, ((pIPArea) as Object));
            //}
        }

        public static void SaveBlockIPAreaList()
        {
            //ArrayList sList = new ArrayList();
            //for (var i = 0; i < g_BlockIPAreaList.Count; i++)
            //{
            //    if (g_BlockIPAreaList[i] == "")
            //    {
            //        continue;
            //    }
            //    sList.Add(g_BlockIPAreaList[i]);
            //}
            //sList.SaveToFile(Protocol.Units.Protocol._STR_BLOCK_AREA_FILE);
        }

        public static bool IsBlockIPArea(int nRemoteIP)
        {
            bool result = false;
            //TIPArea pIPArea;
            //double dwReverseIP;
            //if (g_BlockIPAreaList.Count > 0)
            //{
            //    dwReverseIP = Misc.ReverseIP((double)nRemoteIP);
            //    for (var i = 0; i < g_BlockIPAreaList.Count; i++)
            //    {
            //        pIPArea = ((TIPArea)(g_BlockIPAreaList.Values[i]));
            //        if ((dwReverseIP >= pIPArea.Low) && (dwReverseIP <= pIPArea.High))
            //        {
            //            result = true;
            //            return result;
            //        }
            //    }
            //}
            return result;
        }

        public void initialization()
        {
            //g_ConnectOfIPLock = new object();
            //g_ConnectOfIPList = new object();
            //g_BlockIPList = new object();
            //g_TempBlockIPList = new object();
            //g_BlockIPAreaList = new object();
        }
    }

    public class HardwareCnt
    {
        public byte[] HWID;
        public int Count;
    }
}
