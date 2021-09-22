using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using SystemModule;
using SystemModule.Common;
using SystemModule.Packages;
using SystemModule.Sockets;

namespace DBSvr
{
    public class TFrmUserSoc
    {
        private IList<TGateInfo> GateList = null;
        private TGateInfo CurGate = null;
        private Dictionary<string, int> MapList = null;
        private readonly THumDB HumDB;
        private readonly TFileHumDB HumChrDB;
        private readonly ISocketServer UserSocket;
        private Timer userSocTimer;
        private readonly TFrmIDSoc _frmIdSoc;

        public TFrmUserSoc(TFrmIDSoc frmIdSoc, TFileHumDB humChrDb, THumDB humDb)
        {
            _frmIdSoc = frmIdSoc;
            HumChrDB = humChrDb;
            HumDB = humDb;
            //CS_GateSession = new TCriticalSection();
            GateList = new List<TGateInfo>();
            MapList = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
            UserSocket = new ISocketServer(ushort.MaxValue,1024);
            UserSocket.OnClientConnect += UserSocketClientConnect;
            UserSocket.OnClientDisconnect += UserSocketClientDisconnect;
            UserSocket.OnClientRead += UserSocketClientRead;
            UserSocket.OnClientError += UserSocketClientError;
            UserSocket.Init();
            LoadServerInfo();
            LoadChrNameList("DenyChrName.txt");
            LoadClearMakeIndexList("ClearMakeIndex.txt");
        }

        public void Start()
        {
            UserSocket.Start(DBShare.g_sGateAddr, DBShare.g_nGatePort);
            DBShare.OutMainMessage("角色网关监听.");
            userSocTimer = new Timer(Timer1Timer, null, 1000, 10000);
        }

        public void Stop()
        {
            TGateInfo GateInfo;
            TUserInfo UserInfo;
            for (var i = 0; i < GateList.Count; i++)
            {
                GateInfo = GateList[i];
                if (GateInfo != null)
                {
                    //for (var ii = 0; ii < GateInfo.UserList.Count; ii++)
                    //{
                    //    UserInfo = GateInfo.UserList[ii];
                    //    this.Dispose(UserInfo);
                    //}
                    //GateInfo.UserList.Free;
                }
                GateList.RemoveAt(i);
                break;
            }
            GateList = null;
            MapList = null;
        }

        private void UserSocketClientConnect(object sender, AsyncUserToken e)
        {
            TGateInfo GateInfo;
            string sIPaddr = e.RemoteIPaddr;
            const string sGateOpen = "角色网关[{0}]({1}:{2})已打开...";
            if (!DBShare.CheckServerIP(sIPaddr))
            {
                DBShare.OutMainMessage("非法网关连接: " + sIPaddr);
                e.Socket.Close();
                return;
            }
            if (DBShare.boOpenDBBusy)
            {
                GateInfo = new TGateInfo();
                GateInfo.Socket = e.Socket;
                GateInfo.sGateaddr = sIPaddr;
                GateInfo.sText = "";
                GateInfo.UserList = new List<TUserInfo>();
                GateInfo.dwTick10 = HUtil32.GetTickCount();
                GateInfo.nGateID = DBShare.GetGateID(sIPaddr);
                GateList.Add(GateInfo);
                DBShare.MainOutMessage(string.Format(sGateOpen, 0, e.RemoteIPaddr, e.RemotePort));
            }
            else
            {
                e.Socket.Close();
            }
        }

        private void UserSocketClientDisconnect(object sender, AsyncUserToken e)
        {
            TGateInfo GateInfo;
            TUserInfo UserInfo;
            const string sGateClose = "角色网关[{0}]({1}:{2})已关闭...";
            for (var i = 0; i < GateList.Count; i++)
            {
                GateInfo = GateList[i];
                if (GateInfo != null)
                {
                    for (var ii = 0; ii < GateInfo.UserList.Count; ii++)
                    {
                        UserInfo = GateInfo.UserList[ii];
                        UserInfo = null;
                    }
                    GateInfo.UserList = null;
                }
                DBShare.MainOutMessage(string.Format(sGateClose, i, e.RemoteIPaddr, e.RemotePort));
                GateList.RemoveAt(i);
                break;
            }
        }

        private void UserSocketClientError(object sender, AsyncSocketErrorEventArgs e)
        {

        }

        private void UserSocketClientRead(object sender, AsyncUserToken e)
        {
            string sReviceMsg;
            TGateInfo GateInfo;
            for (var i = 0; i < GateList.Count; i++)
            {
                GateInfo = GateList[i];
                if (GateInfo.Socket == e.Socket)
                {
                    CurGate = GateInfo;
                    var nReviceLen = e.BytesReceived;
                    var data = new byte[nReviceLen];
                    Buffer.BlockCopy(e.ReceiveBuffer, e.Offset, data, 0, nReviceLen);
                    sReviceMsg = HUtil32.GetString(data, 0, data.Length);
                    GateInfo.sText = GateInfo.sText + sReviceMsg;
                    if (GateInfo.sText.Length < 81920)
                    {
                        if (GateInfo.sText.IndexOf("$", StringComparison.OrdinalIgnoreCase) > 1)
                        {
                            ProcessGateMsg(ref GateInfo);
                        }
                    }
                    else
                    {
                        GateInfo.sText = "";
                    }
                }
            }
        }

        public void Timer1Timer(object obj)
        {
            int n8 = DBShare.g_nQueryChrCount + DBShare.nHackerNewChrCount + DBShare.nHackerDelChrCount + DBShare.nHackerSelChrCount + DBShare.n4ADC1C + DBShare.n4ADC20 + DBShare.n4ADC24 + DBShare.n4ADC28;
            if (DBShare.n4ADBB8 != n8)
            {
                DBShare.n4ADBB8 = n8;
                DBShare.OutMainMessage("H-QyChr=" + (DBShare.g_nQueryChrCount).ToString() + " " + "H-NwChr=" + (DBShare.nHackerNewChrCount).ToString() + " " + "H-DlChr=" + (DBShare.nHackerDelChrCount).ToString() + " " + "Dubl-Sl=" + (DBShare.nHackerSelChrCount).ToString() + " " + "H-Er-P1=" + (DBShare.n4ADC1C).ToString() + " " + "Dubl-P2=" + (DBShare.n4ADC20).ToString() + " " + "Dubl-P3=" + (DBShare.n4ADC24).ToString() + " " + "Dubl-P4=" + (DBShare.n4ADC28).ToString());
            }
        }

        public int GetUserCount()
        {
            TGateInfo GateInfo;
            int nUserCount = 0;
            for (var i = 0; i < GateList.Count; i++)
            {
                GateInfo = GateList[i];
                nUserCount += GateInfo.UserList.Count;
            }
            return nUserCount;
        }

        private bool NewChrData(string sChrName, int nSex, int nJob, int nHair)
        {
            bool result = false;
            THumDataInfo ChrRecord;
            try
            {
                if (HumDB.Open() && (HumDB.Index(sChrName) == -1))
                {
                    ChrRecord = new THumDataInfo();
                    ChrRecord.Header = new TRecordHeader();
                    ChrRecord.Data = new THumInfoData();
                    ChrRecord.Header.sName = sChrName;
                    ChrRecord.Data.sChrName = sChrName;
                    ChrRecord.Data.btSex = (byte)nSex;
                    ChrRecord.Data.btJob = (byte)nJob;
                    ChrRecord.Data.btHair = (byte)nHair;
                    HumDB.Add(ref ChrRecord);
                    result = true;
                }
            }
            finally
            {
                HumDB.Close();
            }
            return result;
        }

        private void LoadServerInfo()
        {
            StringList LoadList;
            string sLineText = string.Empty;
            string sSelGateIPaddr = string.Empty;
            string sGameGateIPaddr = string.Empty;
            string sGameGate = string.Empty;
            string sGameGatePort = string.Empty;
            string sMapName = string.Empty;
            string sMapInfo = string.Empty;
            string sServerIndex = string.Empty;
            IniFile Conf;
            try
            {
                LoadList = new StringList();
                LoadList.LoadFromFile(DBShare.sGateConfFileName);
                int nRouteIdx = 0;
                int nGateIdx = 0;
                for (var i = 0; i < LoadList.Count; i++)
                {
                    sLineText = LoadList[i].Trim();
                    if ((sLineText != "") && (sLineText[0] != ';'))
                    {
                        sGameGate = HUtil32.GetValidStr3(sLineText, ref sSelGateIPaddr, new string[] { " ", "\09" });
                        if ((sGameGate == "") || (sSelGateIPaddr == ""))
                        {
                            continue;
                        }
                        DBShare.g_RouteInfo[nRouteIdx] = new TRouteInfo();
                        DBShare.g_RouteInfo[nRouteIdx].sSelGateIP = sSelGateIPaddr.Trim();
                        DBShare.g_RouteInfo[nRouteIdx].nGateCount = 0;
                        nGateIdx = 0;
                        while ((sGameGate != ""))
                        {
                            sGameGate = HUtil32.GetValidStr3(sGameGate, ref sGameGateIPaddr, new string[] { " ", "\09" });
                            sGameGate = HUtil32.GetValidStr3(sGameGate, ref sGameGatePort, new string[] { " ", "\09" });
                            DBShare.g_RouteInfo[nRouteIdx].sGameGateIP[nGateIdx] = sGameGateIPaddr.Trim();
                            DBShare.g_RouteInfo[nRouteIdx].nGameGatePort[nGateIdx] = HUtil32.Str_ToInt(sGameGatePort, 0);
                            nGateIdx++;
                        }
                        DBShare.g_RouteInfo[nRouteIdx].nGateCount = nGateIdx;
                        nRouteIdx++;
                    }
                }
                Conf = new IniFile(DBShare.sConfFileName);
                DBShare.sMapFile = Conf.ReadString("Setup", "MapFile", DBShare.sMapFile);
                //Conf.Free;
                MapList.Clear();
                if (File.Exists(DBShare.sMapFile))
                {
                    LoadList.Clear();
                    LoadList.LoadFromFile(DBShare.sMapFile);
                    for (var i = 0; i < LoadList.Count; i++)
                    {
                        sLineText = LoadList[i];
                        if ((sLineText != "") && (sLineText[0] == '['))
                        {
                            sLineText = HUtil32.ArrestStringEx(sLineText, "[", "]", ref sMapName);
                            sMapInfo = HUtil32.GetValidStr3(sMapName, ref sMapName, new string[] { " ", "\09" });
                            sServerIndex = HUtil32.GetValidStr3(sMapInfo, ref sMapInfo, new string[] { " ", "\09" }).Trim();
                            int nServerIndex = HUtil32.Str_ToInt(sServerIndex, 0);
                            MapList.Add(sMapName, nServerIndex);
                        }
                    }
                }
                //LoadList.Free;
            }
            finally
            {
            }
        }

        private bool LoadChrNameList(string sFileName)
        {
            bool result = false;
            int i;
            if (File.Exists(sFileName))
            {
                DBShare.DenyChrNameList.LoadFromFile(sFileName);
                i = 0;
                while (true)
                {
                    if (DBShare.DenyChrNameList.Count <= i)
                    {
                        break;
                    }
                    if (DBShare.DenyChrNameList[i].Trim() == "")
                    {
                        DBShare.DenyChrNameList.RemoveAt(i);
                        continue;
                    }
                    i++;
                }
                result = true;
            }
            return result;
        }

        private bool LoadClearMakeIndexList(string sFileName)
        {
            bool result = false;
            int i;
            int nIndex;
            string sLineText;
            if (File.Exists(sFileName))
            {
                DBShare.g_ClearMakeIndex.LoadFromFile(sFileName);
                i = 0;
                while (true)
                {
                    if (DBShare.g_ClearMakeIndex.Count <= i)
                    {
                        break;
                    }
                    sLineText = DBShare.g_ClearMakeIndex[i];
                    nIndex = HUtil32.Str_ToInt(sLineText, -1);
                    if (nIndex < 0)
                    {
                        DBShare.g_ClearMakeIndex.RemoveAt(i);
                        continue;
                    }
                    DBShare.g_ClearMakeIndex[i] = nIndex.ToString();
                    i++;
                }
                result = true;
            }
            return result;
        }

        private void ProcessGateMsg(ref TGateInfo GateInfo)
        {
            string s0C = string.Empty;
            string s10 = string.Empty;
            char s19;
            TUserInfo UserInfo;
            while (true)
            {
                if (GateInfo.sText.IndexOf("$", StringComparison.Ordinal) <= 0)
                {
                    break;
                }
                GateInfo.sText = HUtil32.ArrestStringEx(GateInfo.sText, "%", "$", ref s10);
                if (s10 != "")
                {
                    s19 = s10[0];
                    s10 = s10.Substring(1, s10.Length - 1);
                    switch (s19)
                    {
                        case '-':
                            SendKeepAlivePacket(GateInfo.Socket);
                            //dwKeepAliveTick = HUtil32.GetTickCount();
                            break;
                        case 'A':
                            s10 = HUtil32.GetValidStr3(s10, ref s0C, new string[] { "/" });
                            for (var i = 0; i < GateInfo.UserList.Count; i++)
                            {
                                UserInfo = GateInfo.UserList[i];
                                if (UserInfo != null)
                                {
                                    if (UserInfo.sConnID == s0C)
                                    {
                                        UserInfo.s2C = UserInfo.s2C + s10;
                                        if (s10.IndexOf("!", StringComparison.OrdinalIgnoreCase) < 1)
                                        {
                                            continue;
                                        }
                                        ProcessUserMsg(ref UserInfo);
                                        break;
                                    }
                                }
                            }
                            break;
                        case 'O':
                            s10 = HUtil32.GetValidStr3(s10, ref s0C, new string[] { "/" });
                            OpenUser(s0C, s10, ref GateInfo);
                            break;
                        case 'X':
                            CloseUser(s10, ref GateInfo);
                            break;
                    }
                }
            }
        }

        private void SendKeepAlivePacket(Socket Socket)
        {
            if (Socket.Connected)
            {
                Socket.SendText("%++$");
            }
        }

        private void ProcessUserMsg(ref TUserInfo UserInfo)
        {
            string s10 = string.Empty;
            int nC = 0;
            while (true)
            {
                if (HUtil32.TagCount(UserInfo.s2C, '!') <= 0)
                {
                    break;
                }
                UserInfo.s2C = HUtil32.ArrestStringEx(UserInfo.s2C, "#", "!", ref s10);
                if (s10 != "")
                {
                    s10 = s10.Substring(1, s10.Length - 1);
                    if (s10.Length >= Grobal2.DEFBLOCKSIZE)
                    {
                        DeCodeUserMsg(s10, ref UserInfo);
                    }
                    else
                    {
                        DBShare.n4ADC20++;
                    }
                }
                else
                {
                    DBShare.n4ADC1C++;
                    if (nC >= 1)
                    {
                        UserInfo.s2C = "";
                    }
                    nC++;
                }
            }
        }

        private void OpenUser(string sID, string sIP, ref TGateInfo GateInfo)
        {
            TUserInfo UserInfo;
            string sUserIPaddr = string.Empty;
            string sGateIPaddr = HUtil32.GetValidStr3(sIP, ref sUserIPaddr, new string[] { "/" });
            for (var i = 0; i < GateInfo.UserList.Count; i++)
            {
                UserInfo = GateInfo.UserList[i];
                if ((UserInfo != null) && (UserInfo.sConnID == sID))
                {
                    return;
                }
            }
            UserInfo = new TUserInfo();
            UserInfo.sAccount = "";
            UserInfo.sUserIPaddr = sUserIPaddr;
            UserInfo.sGateIPaddr = sGateIPaddr;
            UserInfo.sConnID = sID;
            UserInfo.nSessionID = 0;
            UserInfo.Socket = GateInfo.Socket;
            UserInfo.s2C = "";
            UserInfo.dwTick34 = HUtil32.GetTickCount();
            UserInfo.dwChrTick = HUtil32.GetTickCount();
            UserInfo.boChrSelected = false;
            UserInfo.boChrQueryed = false;
            UserInfo.nSelGateID = GateInfo.nGateID;
            GateInfo.UserList.Add(UserInfo);
        }

        private void CloseUser(string sID, ref TGateInfo GateInfo)
        {
            TUserInfo UserInfo;
            for (var i = 0; i < GateInfo.UserList.Count; i++)
            {
                UserInfo = GateInfo.UserList[i];
                if ((UserInfo != null) && (UserInfo.sConnID == sID))
                {
                    if (!_frmIdSoc.GetGlobaSessionStatus(UserInfo.nSessionID))
                    {
                        _frmIdSoc.SendSocketMsg(Grobal2.SS_SOFTOUTSESSION, UserInfo.sAccount + "/" + (UserInfo.nSessionID).ToString());
                        _frmIdSoc.CloseSession(UserInfo.sAccount, UserInfo.nSessionID);
                    }
                    UserInfo = null;
                    GateInfo.UserList.RemoveAt(i);
                    break;
                }
            }
        }

        private void DeCodeUserMsg(string sData, ref TUserInfo UserInfo)
        {
            string sDefMsg = sData.Substring(0, Grobal2.DEFBLOCKSIZE);
            string s18 = sData.Substring(Grobal2.DEFBLOCKSIZE, sData.Length - Grobal2.DEFBLOCKSIZE);
            TDefaultMessage Msg = EDcode.DecodeMessage(sDefMsg);
            switch (Msg.Ident)
            {
                case Grobal2.CM_QUERYCHR:
                    if (!UserInfo.boChrQueryed || ((HUtil32.GetTickCount() - UserInfo.dwChrTick) > 200))
                    {
                        UserInfo.dwChrTick = HUtil32.GetTickCount();
                        if (QueryChr(s18, ref UserInfo))
                        {
                            UserInfo.boChrQueryed = true;
                        }
                    }
                    else
                    {
                        DBShare.g_nQueryChrCount++;
                        DBShare.OutMainMessage("[Hacker Attack] QUERYCHR " + UserInfo.sUserIPaddr);
                    }
                    break;
                case Grobal2.CM_NEWCHR:
                    if ((HUtil32.GetTickCount() - UserInfo.dwChrTick) > 1000)
                    {
                        UserInfo.dwChrTick = HUtil32.GetTickCount();
                        if ((UserInfo.sAccount != "") && _frmIdSoc.CheckSession(UserInfo.sAccount, UserInfo.sUserIPaddr, UserInfo.nSessionID))
                        {
                            NewChr(s18, ref UserInfo);
                            UserInfo.boChrQueryed = false;
                        }
                        else
                        {
                            OutOfConnect(UserInfo);
                        }
                    }
                    else
                    {
                        DBShare.nHackerNewChrCount++;
                        DBShare.OutMainMessage("[Hacker Attack] NEWCHR " + UserInfo.sAccount + "/" + UserInfo.sUserIPaddr);
                    }
                    break;
                case Grobal2.CM_DELCHR:
                    if ((HUtil32.GetTickCount() - UserInfo.dwChrTick) > 1000)
                    {
                        UserInfo.dwChrTick = HUtil32.GetTickCount();
                        if ((UserInfo.sAccount != "") && _frmIdSoc.CheckSession(UserInfo.sAccount, UserInfo.sUserIPaddr, UserInfo.nSessionID))
                        {
                            DelChr(s18, ref UserInfo);
                            UserInfo.boChrQueryed = false;
                        }
                        else
                        {
                            OutOfConnect(UserInfo);
                        }
                    }
                    else
                    {
                        DBShare.nHackerDelChrCount++;
                        DBShare.OutMainMessage("[Hacker Attack] DELCHR " + UserInfo.sAccount + "/" + UserInfo.sUserIPaddr);
                    }
                    break;
                case Grobal2.CM_SELCHR:
                    if (!UserInfo.boChrQueryed)
                    {
                        if ((UserInfo.sAccount != "") && _frmIdSoc.CheckSession(UserInfo.sAccount, UserInfo.sUserIPaddr, UserInfo.nSessionID))
                        {
                            if (SelectChr(s18, ref UserInfo))
                            {
                                UserInfo.boChrSelected = true;
                            }
                        }
                        else
                        {
                            OutOfConnect(UserInfo);
                        }
                    }
                    else
                    {
                        DBShare.nHackerSelChrCount++;
                        DBShare.OutMainMessage("Double send SELCHR " + UserInfo.sAccount + "/" + UserInfo.sUserIPaddr);
                    }
                    break;
                default:
                    DBShare.n4ADC24++;
                    break;
            }
        }

        /// <summary>
        /// 查询角色
        /// </summary>
        /// <param name="sData"></param>
        /// <param name="UserInfo"></param>
        /// <returns></returns>
        private bool QueryChr(string sData, ref TUserInfo UserInfo)
        {
            bool result;
            string sAccount = string.Empty;
            string sSessionID = string.Empty;
            int nSessionID;
            int nChrCount;
            IList<TQuickID> ChrList;
            int I;
            int nIndex;
            THumDataInfo ChrRecord = null;
            THumInfo HumRecord = null;
            TQuickID QuickID;
            byte btSex;
            string sChrName = string.Empty;
            string sJob = string.Empty;
            string sHair = string.Empty;
            string sLevel = string.Empty;
            string s40 = string.Empty;
            result = false;
            sSessionID = HUtil32.GetValidStr3(EDcode.DeCodeString(sData), ref sAccount, new string[] { "/" });
            nSessionID = HUtil32.Str_ToInt(sSessionID, -2);
            UserInfo.nSessionID = nSessionID;
            nChrCount = 0;
            if (_frmIdSoc.CheckSession(sAccount, UserInfo.sUserIPaddr, nSessionID))
            {
                _frmIdSoc.SetGlobaSessionNoPlay(nSessionID);
                UserInfo.sAccount = sAccount;
                ChrList = new List<TQuickID>();
                try
                {
                    if (HumDB.Open() && (HumChrDB.FindByAccount(sAccount, ref ChrList) >= 0))
                    {
                        try
                        {
                            if (HumDB.OpenEx())
                            {
                                for (var i = 0; i < ChrList.Count; i++)
                                {
                                    QuickID = ((TQuickID)(ChrList[i]));
                                    if (QuickID.nSelectID != UserInfo.nSelGateID) // 如果选择ID不对,则跳过
                                    {
                                        continue;
                                    }
                                    if (HumChrDB.GetBy(QuickID.nIndex, ref HumRecord) && !HumRecord.boDeleted)
                                    {
                                        sChrName = QuickID.sChrName;
                                        nIndex = HumDB.Index(sChrName);
                                        if ((nIndex < 0) || (nChrCount >= 2))
                                        {
                                            continue;
                                        }
                                        if (HumDB.Get(nIndex, ref ChrRecord) >= 0)
                                        {
                                            btSex = ChrRecord.Data.btSex;
                                            sJob = (ChrRecord.Data.btJob).ToString();
                                            sHair = (ChrRecord.Data.btHair).ToString();
                                            sLevel = (ChrRecord.Data.Abil.Level).ToString();
                                            if (HumRecord.boSelected)
                                            {
                                                s40 = s40 + "*";
                                            }
                                            s40 = s40 + sChrName + "/" + sJob + "/" + sHair + "/" + sLevel + "/" + (btSex).ToString() + "/";
                                            nChrCount++;
                                        }
                                    }
                                }
                            }
                        }
                        finally
                        {
                            HumDB.Close();
                        }
                    }
                }
                finally
                {
                    //HumDB.Close();
                }
                //ChrList.Free;
                SendUserSocket(UserInfo.Socket, UserInfo.sConnID, EDcode.EncodeMessage(Grobal2.MakeDefaultMsg(Grobal2.SM_QUERYCHR, nChrCount, 0, 1, 0)) + EDcode.EncodeString(s40));
            }
            else
            {
                SendUserSocket(UserInfo.Socket, UserInfo.sConnID, EDcode.EncodeMessage(Grobal2.MakeDefaultMsg(Grobal2.SM_QUERYCHR_FAIL, nChrCount, 0, 1, 0)));
                CloseUser(UserInfo.sConnID, ref CurGate);
            }
            return result;
        }

        private void OutOfConnect(TUserInfo UserInfo)
        {
            TDefaultMessage Msg;
            string sMsg;
            Msg = Grobal2.MakeDefaultMsg(Grobal2.SM_OUTOFCONNECTION, 0, 0, 0, 0);
            sMsg = EDcode.EncodeMessage(Msg);
            SendUserSocket(UserInfo.Socket, sMsg, UserInfo.sConnID);
        }

        public int DelChr_snametolevel(string sName)
        {
            int result;
            int nIndex;
            THumDataInfo ChrRecord = null;
            result = 0;
            try
            {
                if (HumDB.OpenEx())
                {
                    nIndex = HumDB.Index(sName);
                    if (nIndex >= 0)
                    {
                        HumDB.Get(nIndex, ref ChrRecord);
                        result = ChrRecord.Data.Abil.Level;
                    }
                }
            }
            finally
            {
                HumDB.Close();
            }
            return result;
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="sData"></param>
        /// <param name="UserInfo"></param>
        private void DelChr(string sData, ref TUserInfo UserInfo)
        {
            TDefaultMessage Msg;
            string sMsg;
            int n10;
            THumInfo HumRecord = null;
            int nckr;
            int nIndex;
            var sChrName = EDcode.DeCodeString(sData);
            var boCheck = false;
            try
            {
                if (HumChrDB.Open())
                {
                    n10 = HumChrDB.Index(sChrName);
                    if (n10 >= 0)
                    {
                        HumChrDB.Get(n10, ref HumRecord);
                        nIndex = DelChr_snametolevel(sChrName);
                        if (HumRecord.sAccount == UserInfo.sAccount)
                        {
                            if (nIndex < DBShare.nDELMaxLevel)
                            {
                                HumRecord.boDeleted = true;
                                HumRecord.dModDate = DateTime.Now;
                                boCheck = HumChrDB.Update(n10, ref HumRecord);
                            }
                            else
                            {
                                nckr = 1;
                            }
                        }
                    }
                }
            }
            finally
            {
                HumDB.Close();
            }
            if (boCheck)
            {
                Msg = Grobal2.MakeDefaultMsg(Grobal2.SM_DELCHR_SUCCESS, 0, 0, 0, 0);
            }
            else
            {
                Msg = Grobal2.MakeDefaultMsg(Grobal2.SM_DELCHR_FAIL, 0, 0, 0, 0);
            }
            sMsg = EDcode.EncodeMessage(Msg);
            SendUserSocket(UserInfo.Socket, UserInfo.sConnID, sMsg);
        }

        /// <summary>
        /// 新建角色
        /// </summary>
        /// <param name="sData"></param>
        /// <param name="UserInfo"></param>
        private void NewChr(string sData, ref TUserInfo UserInfo)
        {
            string Data = string.Empty;
            string sAccount = string.Empty;
            string sChrName = string.Empty;
            string sHair = string.Empty;
            string sJob = string.Empty;
            string sSex = string.Empty;
            TDefaultMessage Msg;
            string sMsg;
            THumInfo HumRecord;
            int i;
            var nCode = -1;
            Data = EDcode.DeCodeString(sData);
            Data = HUtil32.GetValidStr3(Data, ref sAccount, new string[] { "/" });
            Data = HUtil32.GetValidStr3(Data, ref sChrName, new string[] { "/" });
            Data = HUtil32.GetValidStr3(Data, ref sHair, new string[] { "/" });
            Data = HUtil32.GetValidStr3(Data, ref sJob, new string[] { "/" });
            Data = HUtil32.GetValidStr3(Data, ref sSex, new string[] { "/" });
            if (Data.Trim() != "")
            {
                nCode = 0;
            }
            sChrName = sChrName.Trim();
            if (sChrName.Length < 3)
            {
                nCode = 0;
            }
            if (DBShare.g_boEnglishNames && !HUtil32.IsEnglishStr(sChrName))
            {
                nCode = 0;
            }
            if (!CheckDenyChrName(sChrName))
            {
                nCode = 2;
            }
            if (!DBShare.boDenyChrName)
            {
                if (!DBShare.CheckChrName(sChrName))
                {
                    nCode = 0;
                }
                for (i = 0; i <= sChrName.Length; i++)
                {
                    if ((sChrName[i] == '?') || (sChrName[i] == ' ') || (sChrName[i] == '/') || (sChrName[i] == '@') || (sChrName[i] == '?') || (sChrName[i] == '\'') ||
                        (sChrName[i] == '\'') || (sChrName[i] == '\\') || (sChrName[i] == '.') || (sChrName[i] == ',') || (sChrName[i] == ':') || (sChrName[i] == ';') ||
                        (sChrName[i] == '`') || (sChrName[i] == '~') || (sChrName[i] == '!') || (sChrName[i] == '#') || (sChrName[i] == '$') || (sChrName[i] == '%') ||
                        (sChrName[i] == '^') || (sChrName[i] == '&') || (sChrName[i] == '*') || (sChrName[i] == '(') || (sChrName[i] == ')') || (sChrName[i] == '-') ||
                        (sChrName[i] == '_') || (sChrName[i] == '+') || (sChrName[i] == '=') || (sChrName[i] == '|') || (sChrName[i] == '[') || (sChrName[i] == '{') ||
                        (sChrName[i] == ']') || (sChrName[i] == '}'))
                    {
                        nCode = 0;
                    }
                }
            }
            if (nCode == -1)
            {
                try
                {
                    HumDB.__Lock();
                    if (HumDB.Index(sChrName) >= 0)
                    {
                        nCode = 2;
                    }
                }
                finally
                {
                    HumDB.UnLock();
                }
                try
                {
                    if (HumChrDB.Open())
                    {
                        if (HumChrDB.ChrCountOfAccount(sAccount) < 2)
                        {
                            HumRecord = new THumInfo();
                            HumRecord.sChrName = sChrName;
                            HumRecord.sAccount = sAccount;
                            HumRecord.boDeleted = false;
                            HumRecord.btCount = 0;
                            HumRecord.Header.sName = sChrName;
                            HumRecord.Header.nSelectID = UserInfo.nSelGateID;
                            if (HumRecord.Header.sName != "")
                            {
                                if (!HumChrDB.Add(HumRecord))
                                {
                                    nCode = 2;
                                }
                            }
                        }
                        else
                        {
                            nCode = 3;
                        }
                    }
                }
                finally
                {
                    HumDB.Close();
                }
                if (nCode == -1)
                {
                    if (NewChrData(sChrName, HUtil32.Str_ToInt(sSex, 0), HUtil32.Str_ToInt(sJob, 0), HUtil32.Str_ToInt(sHair, 0)))
                    {
                        nCode = 1;
                    }
                }
                else
                {
                    //BSMain.FrmDBSrv.DelHum(sChrName); //删除人物
                    nCode = 4;
                }
            }
            if (nCode == 1)
            {
                Msg = Grobal2.MakeDefaultMsg(Grobal2.SM_NEWCHR_SUCCESS, 0, 0, 0, 0);
            }
            else
            {
                Msg = Grobal2.MakeDefaultMsg(Grobal2.SM_NEWCHR_FAIL, nCode, 0, 0, 0);
            }
            sMsg = EDcode.EncodeMessage(Msg);
            SendUserSocket(UserInfo.Socket, UserInfo.sConnID, sMsg);
        }

        /// <summary>
        /// 选择角色
        /// </summary>
        /// <param name="sData"></param>
        /// <param name="UserInfo"></param>
        /// <returns></returns>
        private bool SelectChr(string sData, ref TUserInfo UserInfo)
        {
            string sAccount = string.Empty;
            IList<TQuickID> ChrList;
            THumInfo HumRecord = null;
            int nIndex;
            int nMapIndex;
            TQuickID QuickID;
            THumDataInfo ChrRecord = null;
            string sCurMap = string.Empty;
            bool boDataOK;
            string sDefMsg = string.Empty;
            string sRouteMsg = string.Empty;
            string sRouteIP = string.Empty;
            int nRoutePort = 0;
            var result = false;
            var sChrName = HUtil32.GetValidStr3(EDcode.DeCodeString(sData, true), ref sAccount, new string[] { "/" });
            boDataOK = false;
            if (UserInfo.sAccount == sAccount)
            {
                try
                {
                    if (HumChrDB.Open())
                    {
                        ChrList = new List<TQuickID>();
                        if (HumChrDB.FindByAccount(sAccount, ref ChrList) >= 0)
                        {
                            for (var i = 0; i < ChrList.Count; i++)
                            {
                                nIndex = ChrList[i].nIndex;
                                //if (HumChrDB.GetBy(nIndex, ref HumRecord))
                                //{
                                //    if (HumRecord.sChrName == sChrName)
                                //    {
                                //        HumRecord.boSelected = true;
                                //        HumChrDB.UpdateBy(nIndex, ref HumRecord);
                                //    }
                                //    else
                                //    {
                                //        if (HumRecord.boSelected)
                                //        {
                                //            HumRecord.boSelected = false;
                                //            HumChrDB.UpdateBy(nIndex, ref HumRecord);
                                //        }
                                //    }
                                //}
                            }
                        }
                        //ChrList.Free;
                    }
                }
                finally
                {
                    HumDB.Close();
                }
                try
                {
                    if (HumDB.OpenEx())
                    {
                        nIndex = HumDB.Index(sChrName);
                        if (nIndex >= 0)
                        {
                            HumDB.Get(nIndex, ref ChrRecord);
                            sCurMap = ChrRecord.Data.sCurMap;
                            boDataOK = true;
                        }
                    }
                }
                finally
                {
                    HumDB.Close();
                }
            }
            if (boDataOK)
            {
                nMapIndex = GetMapIndex(sCurMap);
                sDefMsg = EDcode.EncodeMessage(Grobal2.MakeDefaultMsg(Grobal2.SM_STARTPLAY, 0, 0, 0, 0));
                sRouteIP = GateRouteIP(CurGate.sGateaddr, ref nRoutePort);
                if (DBShare.g_boDynamicIPMode)// 使用动态IP
                {
                    sRouteIP = UserInfo.sGateIPaddr;
                }
                sRouteMsg = EDcode.EncodeString(sRouteIP + "/" + nRoutePort + nMapIndex);
                SendUserSocket(UserInfo.Socket, UserInfo.sConnID, sDefMsg + sRouteMsg);
                _frmIdSoc.SetGlobaSessionPlay(UserInfo.nSessionID);
                result = true;
            }
            else
            {
                SendUserSocket(UserInfo.Socket, UserInfo.sConnID, EDcode.EncodeMessage(Grobal2.MakeDefaultMsg(Grobal2.SM_STARTFAIL, 0, 0, 0, 0)));
            }
            return result;
        }

        private int GateRoutePort(string sGateIP)
        {
            return 7200;
        }

        private string GateRouteIP_GetRoute(TRouteInfo RouteInfo, ref int nGatePort)
        {
            var nGateIndex = (new System.Random(RouteInfo.nGateCount)).Next();
            var result = RouteInfo.sGameGateIP[nGateIndex];
            nGatePort = RouteInfo.nGameGatePort[nGateIndex];
            return result;
        }

        private string GateRouteIP(string sGateIP, ref int nPort)
        {
            string result = string.Empty;
            TRouteInfo RouteInfo;
            nPort = 0;
            for (var i = DBShare.g_RouteInfo.GetLowerBound(0); i <= DBShare.g_RouteInfo.GetUpperBound(0); i++)
            {
                RouteInfo = DBShare.g_RouteInfo[i];
                if (RouteInfo.sSelGateIP == sGateIP)
                {
                    result = GateRouteIP_GetRoute(RouteInfo, ref nPort);
                    break;
                }
            }
            return result;
        }

        private int GetMapIndex(string sMap)
        {
            if (MapList.ContainsKey(sMap))
            {
                return MapList[sMap];
            }
            return 0;
        }

        private void SendUserSocket(Socket Socket, string sSessionID, string sSendMsg)
        {
            Socket.SendText("%" + sSessionID + "/#" + sSendMsg + "!$");
        }

        private bool CheckDenyChrName(string sChrName)
        {
            bool result = true;
            for (var i = 0; i < DBShare.DenyChrNameList.Count; i++)
            {
                if (string.Compare((sChrName).ToLower(), (DBShare.DenyChrNameList[i]).ToLower(), StringComparison.OrdinalIgnoreCase) == 0)
                {
                    result = false;
                    break;
                }
            }
            return result;
        }
    }
}

namespace DBSvr
{
    public class UsrSoc
    {
        public static TFrmUserSoc FrmUserSoc = null;
    } 
}

