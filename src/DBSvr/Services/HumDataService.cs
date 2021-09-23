using System;
using System.Collections.Generic;
using System.Net.Sockets;
using SystemModule;
using SystemModule.Packages;
using SystemModule.Sockets;

namespace DBSvr
{
    public class HumDataService
    {
        private IList<TServerInfo> ServerList = null;
        private IList<THumSession> HumSessionList = null;
        private TDefaultMessage m_DefMsg;
        private string s34C;
        private readonly MySqlHumDB HumDB;
        private readonly ISocketServer serverSocket;
        private readonly LoginSocService _LoginSoc;

        public HumDataService(LoginSocService frmIdSoc)
        {
            _LoginSoc = frmIdSoc;
            ServerList = new List<TServerInfo>();
            HumSessionList = new List<THumSession>();
            serverSocket = new ISocketServer(ushort.MaxValue, 1024);
            serverSocket.OnClientConnect += ServerSocketClientConnect;
            serverSocket.OnClientDisconnect += ServerSocketClientDisconnect;
            serverSocket.OnClientRead += ServerSocketClientRead;
            serverSocket.OnClientError += ServerSocketClientError;
            serverSocket.Init();
        }

        public void Start()
        {
            serverSocket.Start(DBShare.sServerAddr, DBShare.nServerPort);
            DBShare.MainOutMessage("数据库服务器启动.");
        }

        private void ServerSocketClientConnect(object sender, AsyncUserToken e)
        {
            TServerInfo ServerInfo;
            string sIPaddr = e.RemoteIPaddr;
            if (!DBShare.CheckServerIP(sIPaddr))
            {
                DBShare.OutMainMessage("非法服务器连接: " + sIPaddr);
                e.Socket.Close();
                return;
            }
            if (DBShare.boOpenDBBusy)
            {
                ServerInfo = new TServerInfo();
                ServerInfo.bo08 = true;
                ServerInfo.nSckHandle = (int)e.Socket.Handle;
                ServerInfo.sStr = "";
                ServerInfo.Socket = e.Socket;
                ServerList.Add(ServerInfo);
            }
            else
            {
                e.Socket.Close();
            }
        }

        private void ServerSocketClientDisconnect(object sender, AsyncUserToken e)
        {
            TServerInfo ServerInfo;
            for (var i = 0; i < ServerList.Count; i++)
            {
                ServerInfo = ServerList[i];
                if (ServerInfo.nSckHandle == (int)e.Socket.Handle)
                {
                    ServerInfo = null;
                    ServerList.RemoveAt(i);
                    ClearSocket(e.Socket);
                    break;
                }
            }
        }

        private void ServerSocketClientError(object sender, AsyncSocketErrorEventArgs e)
        {

        }

        private void ServerSocketClientRead(object sender, AsyncUserToken e)
        {
            TServerInfo ServerInfo;
            string s10;
            for (var i = 0; i < ServerList.Count; i++)
            {
                ServerInfo = ServerList[i];
                if (ServerInfo.nSckHandle == (int)e.Socket.Handle)
                {
                    var nReviceLen = e.BytesReceived;
                    var data = new byte[nReviceLen];
                    Buffer.BlockCopy(e.ReceiveBuffer, e.Offset, data, 0, nReviceLen);
                    s10 = HUtil32.GetString(data, 0, data.Length);
                    if (!string.IsNullOrEmpty(s10))
                    {
                        ServerInfo.sStr = ServerInfo.sStr + s10;
                        if (s10.IndexOf("!", StringComparison.Ordinal) > 0)
                        {
                            ProcessServerPacket(ServerInfo);
                            break;
                        }
                        else
                        {
                            if (ServerInfo.sStr.Length > 81920)
                            {
                                ServerInfo.sStr = "";
                            }
                        }
                    }
                    break;
                }
            }
        }

        private void ProcessServerPacket(TServerInfo ServerInfo)
        {
            bool bo25;
            string sC = string.Empty;
            string s1C = string.Empty;
            string s20 = string.Empty;
            string s24 = string.Empty;
            int n14;
            int n18;
            int wE;
            int w10;
            if (DBShare.boOpenDBBusy)
            {
                return;
            }
            try
            {
                bo25 = false;
                s1C = ServerInfo.sStr;
                ServerInfo.sStr = "";
                s20 = "";
                s1C = HUtil32.ArrestStringEx(s1C, "#", "!", ref s20);
                if (s20 != "")
                {
                    s20 = HUtil32.GetValidStr3(s20, ref s24, new string[] { "/" });
                    n14 = s20.Length;
                    if ((n14 >= Grobal2.DEFBLOCKSIZE) && (s24 != ""))
                    {
                        wE = HUtil32.Str_ToInt(s24, 0) ^ 170;
                        w10 = n14;
                        n18 = HUtil32.MakeLong(wE, w10);
                        var by = new byte[sizeof(int)];
                        unsafe
                        {
                            fixed (byte* pb = by)
                            {
                                *(int*)pb = n18;
                            }
                        }
                        sC = EDcode.EncodeBuffer(@by, @by.Length);
                        s34C = s24;
                        if (HUtil32.CompareBackLStr(s20, sC, sC.Length))
                        {
                            ProcessServerMsg(s20, n14, ServerInfo.Socket);
                            bo25 = true;
                        }
                    }
                }
                if (s1C != "")
                {
                    //Label4.Text = "Error " + (DBShare.n4ADC00).ToString();
                }
                if (!bo25)
                {
                    m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.DBR_FAIL, 0, 0, 0, 0);
                    SendSocket(ServerInfo.Socket, EDcode.EncodeMessage(m_DefMsg));
                    //Label4.Text = "Error " + (DBShare.n4ADC00).ToString();
                }
            }
            finally
            {
            }
        }

        private void SendSocket(Socket Socket, string sMsg)
        {
            int n10 = HUtil32.MakeLong(HUtil32.Str_ToInt(s34C, 0) ^ 170, sMsg.Length + 6);
            var by = new byte[sizeof(int)];
            unsafe
            {
                fixed (byte* pb = by)
                {
                    *(int*)pb = n10;
                }
            }
            string s18 = EDcode.EncodeBuffer(@by, @by.Length);
            Socket.SendText("#" + s34C + "/" + sMsg + s18 + "!");
        }

        /// <summary>
        /// 清理超时会话
        /// </summary>
        public void ClearTimeoutSession()
        {
            THumSession HumSession;
            int i = 0;
            while (true)
            {
                if (HumSessionList.Count <= i)
                {
                    break;
                }
                HumSession = HumSessionList[i];
                if (!HumSession.bo24)
                {
                    if (HumSession.bo2C)
                    {
                        if ((HUtil32.GetTickCount() - HumSession.dwTick30) > 20 * 1000)
                        {
                            HumSession = null;
                            HumSessionList.RemoveAt(i);
                            continue;
                        }
                    }
                    else
                    {
                        if ((HUtil32.GetTickCount() - HumSession.dwTick30) > 2 * 60 * 1000)
                        {
                            HumSession = null;
                            HumSessionList.RemoveAt(i);
                            continue;
                        }
                    }
                }
                if ((HUtil32.GetTickCount() - HumSession.dwTick30) > 40 * 60 * 1000)
                {
                    HumSession = null;
                    HumSessionList.RemoveAt(i);
                    continue;
                }
                i++;
            }
        }

        public bool CopyHumData(string sSrcChrName, string sDestChrName, string sUserID)
        {
            THumDataInfo HumanRCD = null;
            bool result = false;
            bool bo15 = false;
            try
            {
                if (HumDB.Open())
                {
                    int n14 = HumDB.Index(sSrcChrName);
                    if ((n14 >= 0) && (HumDB.Get(n14, ref HumanRCD) >= 0))
                    {
                        bo15 = true;
                    }
                    if (bo15)
                    {
                        n14 = HumDB.Index(sDestChrName);
                        if ((n14 >= 0))
                        {
                            HumanRCD.Header.sName = sDestChrName;
                            HumanRCD.Data.sChrName = sDestChrName;
                            HumanRCD.Data.sAccount = sUserID;
                            HumDB.Update(n14, ref HumanRCD);
                            result = true;
                        }
                    }
                }
            }
            finally
            {
                HumDB.Close();
            }
            return result;
        }

        private void ProcessServerMsg(string sMsg, int nLen, Socket Socket)
        {
            string sDefMsg;
            string sData;
            TDefaultMessage DefMsg;
            if (nLen == Grobal2.DEFBLOCKSIZE)
            {
                sDefMsg = sMsg;
                sData = "";
            }
            else
            {
                sDefMsg = sMsg.Substring(0, Grobal2.DEFBLOCKSIZE);
                sData = sMsg.Substring(Grobal2.DEFBLOCKSIZE, sMsg.Length - Grobal2.DEFBLOCKSIZE - 6);
            }
            DefMsg = EDcode.DecodeMessage(sDefMsg);
            switch (DefMsg.Ident)
            {
                case Grobal2.DB_LOADHUMANRCD:
                    LoadHumanRcd(sData, Socket);
                    break;
                case Grobal2.DB_SAVEHUMANRCD:
                    SaveHumanRcd(DefMsg.Recog, sData, Socket);
                    break;
                case Grobal2.DB_SAVEHUMANRCDEX:
                    SaveHumanRcdEx(sData, DefMsg.Recog, Socket);
                    break;
                default:
                    m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.DBR_FAIL, 0, 0, 0, 0);
                    SendSocket(Socket, EDcode.EncodeMessage(m_DefMsg));
                    break;
            }
        }

        private void LoadHumanRcd(string sMsg, Socket Socket)
        {
            int nIndex;
            THumDataInfo HumanRCD = null;
            bool boFoundSession = false;
            TLoadHuman LoadHuman = new TLoadHuman(EDcode.DecodeBuffer(sMsg));
            string sAccount = LoadHuman.sAccount;
            string sHumName = LoadHuman.sChrName;
            string sIPaddr = LoadHuman.sUserAddr;
            int nSessionID = LoadHuman.nSessionID;
            int nCheckCode = -1;
            if ((sAccount != "") && (sHumName != ""))
            {
                nCheckCode = _LoginSoc.CheckSessionLoadRcd(sAccount, sIPaddr, nSessionID, ref boFoundSession);
                if ((nCheckCode < 0) || !boFoundSession)
                {
                    DBShare.OutMainMessage("[非法请求] " + "帐号: " + sAccount + " IP: " + sIPaddr + " 标识: " + (nSessionID).ToString());
                }
            }
            if ((nCheckCode == 1) || boFoundSession)
            {
                try
                {
                    if (HumDB.Open())
                    {
                        nIndex = HumDB.Index(sHumName);
                        if (nIndex >= 0)
                        {
                            if (HumDB.Get(nIndex, ref HumanRCD) < 0)
                            {
                                nCheckCode = -2;
                            }
                        }
                        else
                        {
                            nCheckCode = -3;
                        }
                    }
                    else
                    {
                        nCheckCode = -4;
                    }
                }
                finally
                {
                    HumDB.Close();
                }
            }
            if ((nCheckCode == 1) || boFoundSession)
            {
                m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.DBR_LOADHUMANRCD, 1, 0, 0, 1);
                SendSocket(Socket, EDcode.EncodeMessage(m_DefMsg) + EDcode.EncodeString(sHumName) + "/" + EDcode.EncodeBuffer(HumanRCD));
            }
            else
            {
                m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.DBR_LOADHUMANRCD, nCheckCode, 0, 0, 0);
                SendSocket(Socket, EDcode.EncodeMessage(m_DefMsg));
            }
        }

        private void SaveHumanRcd(int nRecog, string sMsg, Socket Socket)
        {
            string sChrName = string.Empty;
            string sUserID = string.Empty;
            bool bo21;
            THumDataInfo HumanRCD = null;
            string sHumanRCD = HUtil32.GetValidStr3(sMsg, ref sUserID, new string[] { "/" });
            sHumanRCD = HUtil32.GetValidStr3(sHumanRCD, ref sChrName, new string[] { "/" });
            sUserID = EDcode.DeCodeString(sUserID);
            sChrName = EDcode.DeCodeString(sChrName);
            bo21 = false;
            if (sHumanRCD.Length == 10 * 4 / 3)
            {
                HumanRCD = new THumDataInfo(EDcode.DecodeBuffer(sHumanRCD));
            }
            else
            {
                bo21 = true;
            }
            if (!bo21)
            {
                bo21 = true;
                try
                {
                    if (HumDB.Open())
                    {
                        int nIndex = HumDB.Index(sChrName);
                        if (nIndex < 0)
                        {
                            HumanRCD.Header.sName = sChrName;
                            HumDB.Add(ref HumanRCD);
                            nIndex = HumDB.Index(sChrName);
                        }
                        if (nIndex >= 0)
                        {
                            HumanRCD.Header.sName = sChrName;
                            HumDB.Update(nIndex, ref HumanRCD);
                            bo21 = false;
                        }
                    }
                }
                finally
                {
                    HumDB.Close();
                }
                _LoginSoc.SetSessionSaveRcd(sUserID);
            }
            if (!bo21)
            {
                for (var i = 0; i < HumSessionList.Count; i++)
                {
                    THumSession HumSession = HumSessionList[i];
                    if ((HumSession.sChrName == sChrName) && (HumSession.nIndex == nRecog))
                    {
                        HumSession.dwTick30 = HUtil32.GetTickCount();
                        break;
                    }
                }
                m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.DBR_SAVEHUMANRCD, 1, 0, 0, 0);
                SendSocket(Socket, EDcode.EncodeMessage(m_DefMsg));
            }
            else
            {
                m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.DBR_LOADHUMANRCD, 0, 0, 0, 0);
                SendSocket(Socket, EDcode.EncodeMessage(m_DefMsg));
            }
        }

        private void SaveHumanRcdEx(string sMsg, int nRecog, Socket Socket)
        {
            string sChrName = string.Empty;
            string sUserID = string.Empty;
            string sHumanRCD = HUtil32.GetValidStr3(sMsg, ref sUserID, new string[] { "/" });
            sHumanRCD = HUtil32.GetValidStr3(sHumanRCD, ref sChrName, new string[] { "/" });
            sUserID = EDcode.DeCodeString(sUserID);
            sChrName = EDcode.DeCodeString(sChrName);
            for (var i = 0; i < HumSessionList.Count; i++)
            {
                THumSession HumSession = HumSessionList[i];
                if ((HumSession.sChrName == sChrName) && (HumSession.nIndex == nRecog))
                {
                    HumSession.bo24 = false;
                    HumSession.Socket = Socket;
                    HumSession.bo2C = true;
                    HumSession.dwTick30 = HUtil32.GetTickCount();
                    break;
                }
            }
            SaveHumanRcd(nRecog, sMsg, Socket);
        }

        private void ClearSocket(Socket Socket)
        {
            THumSession HumSession;
            int nIndex = 0;
            while (true)
            {
                if (HumSessionList.Count <= nIndex)
                {
                    break;
                }
                HumSession = HumSessionList[nIndex];
                if (HumSession.Socket == Socket)
                {
                    HumSession = null;
                    HumSessionList.RemoveAt(nIndex);
                    continue;
                }
                nIndex++;
            }
        }
    }
}