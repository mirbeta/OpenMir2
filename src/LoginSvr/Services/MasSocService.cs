using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using SystemModule;
using SystemModule.Common;
using SystemModule.Sockets;

namespace LoginSvr
{
    public class MasSocService : IService
    {
        private readonly LogQueue _logQueue;
        private readonly IList<TMsgServerInfo> m_ServerList = null;
        private readonly ISocketServer serverSocket;
        private readonly ConfigManager _configManager;

        public MasSocService(LogQueue logQueue, ConfigManager configManager)
        {
            m_ServerList = new List<TMsgServerInfo>();
            serverSocket = new ISocketServer(ushort.MaxValue, 1024);
            serverSocket.OnClientConnect += SocketClientConnect;
            serverSocket.OnClientDisconnect += SocketClientDisconnect;
            serverSocket.OnClientError += SocketClientError;
            serverSocket.OnClientRead += SocketClientRead;
            _configManager = configManager;
            _logQueue = logQueue;
        }

        public IList<TMsgServerInfo> ServerList => m_ServerList;

        public void Start()
        {
            var config = _configManager.Config;
            serverSocket.Init();
            serverSocket.Start(config.sServerAddr, config.nServerPort);
            LoadServerAddr();
            LoadUserLimit();
            _logQueue.Enqueue($"账号数据服务[{config.sServerAddr}:{config.nServerPort}]已启动.");
        }

        private void SocketClientConnect(object sender, AsyncUserToken e)
        {
            string sRemoteAddr = e.RemoteIPaddr;
            bool boAllowed = false;
            for (var i = LSShare.ServerAddr.GetLowerBound(0); i <= LSShare.ServerAddr.GetUpperBound(0); i++)
            {
                if (sRemoteAddr == LSShare.ServerAddr[i])
                {
                    boAllowed = true;
                    break;
                }
            }
            if (boAllowed)
            {
                TMsgServerInfo MsgServer = new TMsgServerInfo();
                MsgServer.sReceiveMsg = string.Empty;
                MsgServer.Socket = e.Socket;
                m_ServerList.Add(MsgServer);
            }
            else
            {
                _logQueue.Enqueue("非法地址连接:" + sRemoteAddr);
                e.Socket.Close();
            }
        }

        private void SocketClientDisconnect(object sender, AsyncUserToken e)
        {
            for (var i = 0; i < m_ServerList.Count; i++)
            {
                TMsgServerInfo MsgServer = m_ServerList[i];
                if (MsgServer.Socket == e.Socket)
                {
                    if (MsgServer.nServerIndex == 99)
                    {
                        _logQueue.Enqueue($"[{MsgServer.sServerName}]数据库服务器[{e.RemoteIPaddr}:{e.RemotePort}]断开链接.");
                    }
                    else
                    {
                        _logQueue.Enqueue($"[{MsgServer.sServerName}]游戏服务器[{e.RemoteIPaddr}:{e.RemotePort}]断开链接.");
                    }
                    MsgServer = null;
                    m_ServerList.RemoveAt(i);
                    break;
                }
            }
        }

        private void SocketClientError(object sender, AsyncSocketErrorEventArgs e)
        {

        }

        private void SocketClientRead(object sender, AsyncUserToken e)
        {
            TMsgServerInfo MsgServer;
            string sReviceMsg = string.Empty;
            string sMsg = string.Empty;
            string sCode = string.Empty;
            string sAccount = string.Empty;
            string sServerName = string.Empty;
            string sIndex = string.Empty;
            string sOnlineCount = string.Empty;
            for (var i = 0; i < m_ServerList.Count; i++)
            {
                MsgServer = m_ServerList[i];
                if (MsgServer.Socket == e.Socket)
                {
                    var nReviceLen = e.BytesReceived;
                    var data = new byte[nReviceLen];
                    Array.Copy(e.ReceiveBuffer, e.Offset, data, 0, nReviceLen);
                    sReviceMsg = MsgServer.sReceiveMsg + HUtil32.GetString(data, 0, data.Length);
                    while (sReviceMsg.IndexOf(")", StringComparison.Ordinal) > 0)
                    {
                        sReviceMsg = HUtil32.ArrestStringEx(sReviceMsg, "(", ")", ref sMsg);
                        if (string.IsNullOrEmpty(sMsg))
                        {
                            break;
                        }
                        sMsg = HUtil32.GetValidStr3(sMsg, ref sCode, new[] { "/" });
                        int nCode = HUtil32.Str_ToInt(sCode, -1);
                        switch (nCode)
                        {
                            case Grobal2.SS_SOFTOUTSESSION:
                                sMsg = HUtil32.GetValidStr3(sMsg, ref sAccount, new[] { "/" });
                                CloseUser(sAccount, HUtil32.Str_ToInt(sMsg, 0));
                                break;
                            case Grobal2.SS_SERVERINFO:
                                sMsg = HUtil32.GetValidStr3(sMsg, ref sServerName, new[] { "/" });
                                sMsg = HUtil32.GetValidStr3(sMsg, ref sIndex, new[] { "/" });
                                sMsg = HUtil32.GetValidStr3(sMsg, ref sOnlineCount, new[] { "/" });
                                MsgServer.sServerName = sServerName;
                                MsgServer.nServerIndex = HUtil32.Str_ToInt(sIndex, 0);
                                MsgServer.nOnlineCount = HUtil32.Str_ToInt(sOnlineCount, 0);
                                MsgServer.dwKeepAliveTick = HUtil32.GetTickCount();
                                SortServerList(i);
                                LSShare.nOnlineCountMin = GetOnlineHumCount();
                                if (LSShare.nOnlineCountMin > LSShare.nOnlineCountMax)
                                {
                                    LSShare.nOnlineCountMax = LSShare.nOnlineCountMin;
                                }
                                SendServerMsgA(Grobal2.SS_KEEPALIVE, LSShare.nOnlineCountMin.ToString());
                                RefServerLimit(sServerName);
                                break;
                            case Grobal2.UNKNOWMSG:
                                SendServerMsgA(Grobal2.UNKNOWMSG, sMsg);
                                break;
                            default:
                                Console.WriteLine(nCode);
                                break;
                        }
                    }
                }
                MsgServer.sReceiveMsg = sReviceMsg;
            }
        }

        private void CloseUser(string sAccount, int nSessionID)
        {
            var Config = _configManager.Config;
            for (var i = Config.SessionList.Count - 1; i >= 0; i--)
            {
                TConnInfo ConnInfo = Config.SessionList[i];
                if ((ConnInfo.sAccount == sAccount) || (ConnInfo.nSessionID == nSessionID))
                {
                    SendServerMsg(Grobal2.SS_CLOSESESSION, ConnInfo.sServerName, ConnInfo.sAccount + "/" + ConnInfo.nSessionID);
                    ConnInfo = null;
                    Config.SessionList.RemoveAt(i);
                }
            }
        }

        private void RefServerLimit(string sServerName)
        {
            int nCount = 0;
            TMsgServerInfo MsgServer;
            for (var i = 0; i < m_ServerList.Count; i++)
            {
                MsgServer = m_ServerList[i];
                if ((MsgServer.nServerIndex != 99) && (MsgServer.sServerName == sServerName))
                {
                    nCount += MsgServer.nOnlineCount;
                }
            }
            for (var i = MasSock.UserLimit.GetLowerBound(0); i <= MasSock.UserLimit.GetUpperBound(0); i++)
            {
                if (MasSock.UserLimit[i].sServerName == sServerName)
                {
                    MasSock.UserLimit[i].nLimitCountMin = nCount;
                    break;
                }
            }
        }

        public bool IsNotUserFull(string sServerName)
        {
            bool result = true;
            for (var i = MasSock.UserLimit.GetLowerBound(0); i <= MasSock.UserLimit.GetUpperBound(0); i++)
            {
                if (MasSock.UserLimit[i].sServerName == sServerName)
                {
                    if (MasSock.UserLimit[i].nLimitCountMin > MasSock.UserLimit[i].nLimitCountMax)
                    {
                        result = false;
                    }
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// 服务器排序
        /// </summary>
        /// <param name="nIndex"></param>
        private void SortServerList(int nIndex)
        {
            try
            {
                if (m_ServerList.Count <= nIndex)
                {
                    return;
                }
                TMsgServerInfo MsgServerSort = m_ServerList[nIndex];
                m_ServerList.RemoveAt(nIndex);
                for (var nC = 0; nC < m_ServerList.Count; nC++)
                {
                    TMsgServerInfo MsgServer = m_ServerList[nC];
                    if (MsgServer.sServerName == MsgServerSort.sServerName)
                    {
                        if (MsgServer.nServerIndex < MsgServerSort.nServerIndex)
                        {
                            m_ServerList.Insert(nC, MsgServerSort);
                            return;
                        }
                        else
                        {
                            int nNewIndex = nC + 1;
                            if (nNewIndex < m_ServerList.Count)
                            {
                                for (var n10 = nNewIndex; n10 < m_ServerList.Count; n10++)
                                {
                                    MsgServer = m_ServerList[n10];
                                    if (MsgServer.sServerName == MsgServerSort.sServerName)
                                    {
                                        if (MsgServer.nServerIndex < MsgServerSort.nServerIndex)
                                        {
                                            m_ServerList.Insert(n10, MsgServerSort);
                                            for (var n14 = n10 + 1; n14 < m_ServerList.Count; n14++)
                                            {
                                                MsgServer = m_ServerList[n14];
                                                if ((MsgServer.sServerName == MsgServerSort.sServerName) && (MsgServer.nServerIndex == MsgServerSort.nServerIndex))
                                                {
                                                    m_ServerList.RemoveAt(n14);
                                                    return;
                                                }
                                            }
                                            return;
                                        }
                                        else
                                        {
                                            nNewIndex = n10 + 1;
                                        }
                                    }
                                }
                                m_ServerList.Insert(nNewIndex, MsgServerSort);
                                return;
                            }
                        }
                    }
                }
                m_ServerList.Add(MsgServerSort);
            }
            catch (Exception ex)
            {
                _logQueue.Enqueue(ex.StackTrace);
            }
        }

        public void SendServerMsg(short wIdent, string sServerName, string sMsg)
        {
            const string sFormatMsg = "({0}/{1})";
            try
            {
                string tempName = LimitName(sServerName);
                string sSendMsg = string.Format(sFormatMsg, wIdent, sMsg);
                for (var i = 0; i < m_ServerList.Count; i++)
                {
                    TMsgServerInfo MsgServer = m_ServerList[i];
                    if (MsgServer.Socket.Connected)
                    {
                        if ((string.IsNullOrEmpty(tempName)) || (string.IsNullOrEmpty(MsgServer.sServerName)) || (string.Compare(MsgServer.sServerName, tempName, StringComparison.OrdinalIgnoreCase) == 0)
                            || (MsgServer.nServerIndex == 99))
                        {
                            MsgServer.Socket.SendText(sSendMsg);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logQueue.Enqueue(ex.StackTrace);
            }
        }

        private void LoadServerAddr()
        {
            StringList LoadList;
            int nServerIdx = 0;
            string sFileName = "!ServerAddr.txt";
            if (File.Exists(sFileName))
            {
                LoadList = new StringList();
                LoadList.LoadFromFile(sFileName);
                for (var i = 0; i < LoadList.Count; i++)
                {
                    string sLineText = LoadList[i].Trim();
                    if ((sLineText != "") && (sLineText[i] != ';'))
                    {
                        if (HUtil32.TagCount(sLineText, '.') == 3)
                        {
                            LSShare.ServerAddr[nServerIdx] = sLineText;
                            nServerIdx++;
                            if (nServerIdx >= 100)
                            {
                                break;
                            }
                        }
                    }
                }
                LoadList = null;
            }
        }

        private int GetOnlineHumCount()
        {
            int result = 0;
            int nCount = 0;
            TMsgServerInfo MsgServer;
            try
            {
                for (var i = 0; i < m_ServerList.Count; i++)
                {
                    MsgServer = m_ServerList[i];
                    if (MsgServer.nServerIndex != 99)
                    {
                        nCount += MsgServer.nOnlineCount;
                    }
                }
                result = nCount;
            }
            catch (Exception ex)
            {
                _logQueue.Enqueue(ex.StackTrace);
            }
            return result;
        }

        public bool CheckReadyServers()
        {
            return m_ServerList.Count >= _configManager.Config.nReadyServers;
        }

        private void SendServerMsgA(short wIdent, string sMsg)
        {
            const string sFormatMsg = "({0}/{1})";
            try
            {
                string sSendMsg = string.Format(sFormatMsg, wIdent, sMsg);
                for (var i = 0; i < m_ServerList.Count; i++)
                {
                    if (m_ServerList[i].Socket.Connected)
                    {
                        m_ServerList[i].Socket.SendText(sSendMsg);
                    }
                }
            }
            catch (Exception e)
            {
                _logQueue.Enqueue(e.StackTrace);
            }
        }

        private string LimitName(string sServerName)
        {
            string result = string.Empty;
            for (var i = MasSock.UserLimit.GetLowerBound(0); i <= MasSock.UserLimit.GetUpperBound(0); i++)
            {
                if (string.Compare(MasSock.UserLimit[i].sServerName, sServerName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    result = MasSock.UserLimit[i].sName;
                    break;
                }
            }
            return result;
        }

        private void LoadUserLimit()
        {
            int nC = 0;
            string sLineText = string.Empty;
            string sServerName = string.Empty;
            string s10 = string.Empty;
            string s14 = string.Empty;
            string sFileName = "!UserLimit.txt";
            if (File.Exists(sFileName))
            {
                StringList LoadList = new StringList();
                LoadList.LoadFromFile(sFileName);
                for (var i = 0; i < LoadList.Count; i++)
                {
                    sLineText = LoadList[i];
                    sLineText = HUtil32.GetValidStr3(sLineText, ref sServerName, new[] { " ", "\09" });
                    sLineText = HUtil32.GetValidStr3(sLineText, ref s10, new[] { " ", "\09" });
                    sLineText = HUtil32.GetValidStr3(sLineText, ref s14, new[] { " ", "\09" });
                    if (!string.IsNullOrEmpty(sServerName))
                    {
                        MasSock.UserLimit[nC].sServerName = sServerName;
                        MasSock.UserLimit[nC].sName = s10;
                        MasSock.UserLimit[nC].nLimitCountMax = HUtil32.Str_ToInt(s14, 3000);
                        MasSock.UserLimit[nC].nLimitCountMin = 0;
                        nC++;
                    }
                }
                LoadList = null;
            }
            else
            {
                _logQueue.Enqueue("[Critical Failure] file not found. !UserLimit.txt");
            }
        }

        /// <summary>
        /// 获取服务器状态
        /// </summary>
        /// <param name="sServerName"></param>
        /// <returns></returns>
        public int ServerStatus(string sServerName)
        {
            int result = 0;
            int nStatus = 0;
            TMsgServerInfo MsgServer;
            bool boServerOnLine = false;
            try
            {
                for (var i = 0; i < m_ServerList.Count; i++)
                {
                    MsgServer = m_ServerList[i];
                    if ((MsgServer.nServerIndex != 99) && (MsgServer.sServerName == sServerName))
                    {
                        boServerOnLine = true;
                    }
                }
                if (!boServerOnLine)
                {
                    return result;
                }
                for (var i = MasSock.UserLimit.GetLowerBound(0); i <= MasSock.UserLimit.GetUpperBound(0); i++)
                {
                    if (MasSock.UserLimit[i].sServerName == sServerName)
                    {
                        if (MasSock.UserLimit[i].nLimitCountMin <= MasSock.UserLimit[i].nLimitCountMax / 2)
                        {
                            nStatus = 1;// 空闲
                            break;
                        }
                        if (MasSock.UserLimit[i].nLimitCountMin <= MasSock.UserLimit[i].nLimitCountMax - (MasSock.UserLimit[i].nLimitCountMax / 5))
                        {
                            nStatus = 2;// 良好
                            break;
                        }
                        if (MasSock.UserLimit[i].nLimitCountMin < MasSock.UserLimit[i].nLimitCountMax)
                        {
                            nStatus = 3;// 繁忙
                            break;
                        }
                        if (MasSock.UserLimit[i].nLimitCountMin >= MasSock.UserLimit[i].nLimitCountMax)
                        {
                            nStatus = 4;// 满员
                            break;
                        }
                    }
                }
                result = nStatus;
            }
            catch (Exception ex)
            {
                _logQueue.Enqueue(ex.StackTrace);
            }
            return result;
        }
    }

    public class TMsgServerInfo
    {
        public string sReceiveMsg;
        public Socket Socket;
        public string sServerName;
        public int nServerIndex;
        public int nOnlineCount;
        public int nSelectID;
        public int dwKeepAliveTick;
        public string sIPaddr;
    }

    public struct TLimitServerUserInfo
    {
        public string sServerName;
        public string sName;
        public int nLimitCountMin;
        public int nLimitCountMax;
    }
}

namespace LoginSvr
{
    public class MasSock
    {
        public static TLimitServerUserInfo[] UserLimit = new TLimitServerUserInfo[100];
    }
}