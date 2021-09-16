using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using SystemModule;
using SystemModule.Common;
using SystemModule.Sockets.AsyncSocketServer;
using SystemModule.Sockets;

namespace LoginSvr
{
    public class MasSocService
    {
        public readonly IList<TMsgServerInfo> m_ServerList = null;
        private readonly ISocketServer serverSocket;
        private readonly LoginSvrService loginSvr;

        public MasSocService()
        {
            m_ServerList = new List<TMsgServerInfo>();
            serverSocket = new ISocketServer(ushort.MaxValue, 1024);
            serverSocket.OnClientConnect += MSocketClientConnect;
            serverSocket.OnClientDisconnect += MSocketClientDisconnect;
            serverSocket.OnClientError += MSocketClientError;
            serverSocket.OnClientRead += MSocketClientRead;
            serverSocket.Init();
        }

        public void Start()
        {
            TConfig Config = LSShare.g_Config;
            serverSocket.Start(Config.sServerAddr, Config.nServerPort);
            LoadServerAddr();
            LoadUserLimit();
        }

        private void MSocketClientConnect(object sender, SystemModule.Sockets.AsyncUserToken e)
        {
            string sRemoteAddr = e.RemoteIPaddr;
            bool boAllowed = false;
            TMsgServerInfo MsgServer;
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
                MsgServer = new TMsgServerInfo();
                MsgServer.sReceiveMsg = "";
                MsgServer.Socket = e.Socket;
                m_ServerList.Add(MsgServer);
            }
            else
            {
                LSShare.MainOutMessage("非法地址连接:" + sRemoteAddr);
                e.Socket.Close();
            }
        }

        private void MSocketClientDisconnect(object sender, SystemModule.Sockets.AsyncUserToken e)
        {
            TMsgServerInfo MsgServer;
            for (var i = 0; i < m_ServerList.Count; i++)
            {
                MsgServer = m_ServerList[i];
                if (MsgServer.Socket == e.Socket)
                {
                    MsgServer = null;
                    m_ServerList.RemoveAt(i);
                    break;
                }
            }
        }

        private void MSocketClientError(object sender, SystemModule.Sockets.AsyncSocketErrorEventArgs e)
        {

        }

        private void MSocketClientRead(object sender, SystemModule.Sockets.AsyncUserToken e)
        {
            TMsgServerInfo MsgServer;
            string sReviceMsg = string.Empty;
            string sMsg = string.Empty;
            string sCode = string.Empty;
            string sAccount = string.Empty;
            string sServerName = string.Empty;
            string sIndex = string.Empty;
            string sOnlineCount = string.Empty;
            int nCode;
            TConfig Config;
            Config = LSShare.g_Config;
            for (var i = 0; i < m_ServerList.Count; i++)
            {
                MsgServer = m_ServerList[i];
                if (MsgServer.Socket == e.Socket)
                {
                    var nReviceLen = e.BytesReceived;
                    var data = new byte[nReviceLen];
                    Buffer.BlockCopy(e.ReceiveBuffer, e.Offset, data, 0, nReviceLen);
                    sReviceMsg = MsgServer.sReceiveMsg + HUtil32.GetString(data, 0, data.Length);
                    while ((sReviceMsg.IndexOf(")", StringComparison.OrdinalIgnoreCase) > 0))
                    {
                        sReviceMsg = HUtil32.ArrestStringEx(sReviceMsg, "(", ")", ref sMsg);
                        if (sMsg == "")
                        {
                            break;
                        }
                        sMsg = HUtil32.GetValidStr3(sMsg, ref sCode, new string[] { "/" });
                        nCode = HUtil32.Str_ToInt(sCode, -1);
                        switch (nCode)
                        {
                            case Grobal2.SS_SOFTOUTSESSION:
                                sMsg = HUtil32.GetValidStr3(sMsg, ref sAccount, new string[] { "/" });
                                loginSvr.CloseUser(Config, sAccount, HUtil32.Str_ToInt(sMsg, 0));
                                break;
                            case Grobal2.SS_SERVERINFO:
                                sMsg = HUtil32.GetValidStr3(sMsg, ref sServerName, new string[] { "/" });
                                sMsg = HUtil32.GetValidStr3(sMsg, ref sIndex, new string[] { "/" });
                                sMsg = HUtil32.GetValidStr3(sMsg, ref sOnlineCount, new string[] { "/" });
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
                                SendServerMsgA(Grobal2.SS_KEEPALIVE, (LSShare.nOnlineCountMin).ToString());
                                RefServerLimit(sServerName);
                                break;
                            case Grobal2.UNKNOWMSG:
                                SendServerMsgA(Grobal2.UNKNOWMSG, sMsg);
                                break;
                            default:
                                Console.WriteLine((nCode).ToString());
                                break;
                        }
                    }
                }
                MsgServer.sReceiveMsg = sReviceMsg;
            }
        }

        private void RefServerLimit(string sServerName)
        {
            int nCount = 0;
            TMsgServerInfo MsgServer;
            try
            {
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
            catch
            {
                LSShare.MainOutMessage("TFrmMasSoc.RefServerLimit");
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
            int nC;
            int n10;
            int n14;
            TMsgServerInfo MsgServerSort;
            TMsgServerInfo MsgServer;
            int nNewIndex;
            try
            {
                if (m_ServerList.Count <= nIndex)
                {
                    return;
                }
                MsgServerSort = m_ServerList[nIndex];
                m_ServerList.RemoveAt(nIndex);
                for (nC = 0; nC < m_ServerList.Count; nC++)
                {
                    MsgServer = m_ServerList[nC];
                    if (MsgServer.sServerName == MsgServerSort.sServerName)
                    {
                        if (MsgServer.nServerIndex < MsgServerSort.nServerIndex)
                        {
                            m_ServerList.Insert(nC, MsgServerSort);
                            return;
                        }
                        else
                        {
                            nNewIndex = nC + 1;
                            if (nNewIndex < m_ServerList.Count)
                            {
                                for (n10 = nNewIndex; n10 < m_ServerList.Count; n10++)
                                {
                                    MsgServer = m_ServerList[n10];
                                    if (MsgServer.sServerName == MsgServerSort.sServerName)
                                    {
                                        if (MsgServer.nServerIndex < MsgServerSort.nServerIndex)
                                        {
                                            m_ServerList.Insert(n10, MsgServerSort);
                                            for (n14 = n10 + 1; n14 < m_ServerList.Count; n14++)
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
            catch
            {
                LSShare.MainOutMessage("TFrmMasSoc.SortServerList");
            }
        }

        public void SendServerMsg(short wIdent, string sServerName, string sMsg)
        {
            TMsgServerInfo MsgServer;
            string sSendMsg;
            string s18;
            const string sFormatMsg = "({0}/{1})";
            try
            {
                s18 = LimitName(sServerName);
                sSendMsg = string.Format(sFormatMsg, wIdent, sMsg);
                for (var i = 0; i < m_ServerList.Count; i++)
                {
                    MsgServer = m_ServerList[i];
                    if (MsgServer.Socket.Connected)
                    {
                        if ((s18 == "") || (MsgServer.sServerName == "") || (string.Compare(MsgServer.sServerName, s18, StringComparison.OrdinalIgnoreCase) == 0) || (MsgServer.nServerIndex == 99))
                        {
                            MsgServer.Socket.SendText(sSendMsg);
                        }
                    }
                }
            }
            catch
            {
                LSShare.MainOutMessage("TFrmMasSoc.SendServerMsg");
            }
        }

        private void LoadServerAddr()
        {
            StringList LoadList;
            int nServerIdx = 0;
            string sLineText;
            string sFileName = "!ServerAddr.txt";
            if (File.Exists(sFileName))
            {
                LoadList = new StringList();
                LoadList.LoadFromFile(sFileName);
                for (var i = 0; i < LoadList.Count; i++)
                {
                    sLineText = LoadList[i].Trim();
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
            catch
            {
                LSShare.MainOutMessage("TFrmMasSoc.GetOnlineHumCount");
            }
            return result;
        }

        public bool CheckReadyServers()
        {
            bool result = false;
            TConfig Config = LSShare.g_Config;
            if (m_ServerList.Count >= Config.nReadyServers)
            {
                result = true;
            }
            return result;
        }

        private void SendServerMsgA(short wIdent, string sMsg)
        {
            string sSendMsg;
            TMsgServerInfo MsgServer;
            const string sFormatMsg = "({0}/{1})";
            try
            {
                sSendMsg = string.Format(sFormatMsg, wIdent, sMsg);
                for (var i = 0; i < m_ServerList.Count; i++)
                {
                    MsgServer = m_ServerList[i];
                    if (MsgServer.Socket.Connected)
                    {
                        MsgServer.Socket.SendText(sSendMsg);
                    }
                }
            }
            catch (Exception e)
            {
                LSShare.MainOutMessage("TFrmMasSoc.SendServerMsgA");
                LSShare.MainOutMessage(e.Message);
            }
        }

        private string LimitName(string sServerName)
        {
            string result = string.Empty;
            try
            {
                for (var i = MasSock.UserLimit.GetLowerBound(0); i <= MasSock.UserLimit.GetUpperBound(0); i++)
                {
                    if (string.Compare(MasSock.UserLimit[i].sServerName, sServerName, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        result = MasSock.UserLimit[i].sName;
                        break;
                    }
                }
            }
            catch
            {
                LSShare.MainOutMessage("TFrmMasSoc.LimitName");
            }
            return result;
        }

        private void LoadUserLimit()
        {
            StringList LoadList;
            int nC = 0;
            string sLineText = string.Empty;
            string sServerName = string.Empty;
            string s10 = string.Empty;
            string s14 = string.Empty;
            string sFileName = "!UserLimit.txt";
            if (File.Exists(sFileName))
            {
                LoadList = new StringList();
                LoadList.LoadFromFile(sFileName);
                for (var i = 0; i < LoadList.Count; i++)
                {
                    sLineText = LoadList[i];
                    sLineText = HUtil32.GetValidStr3(sLineText, ref sServerName, new string[] { " ", "\09" });
                    sLineText = HUtil32.GetValidStr3(sLineText, ref s10, new string[] { " ", "\09" });
                    sLineText = HUtil32.GetValidStr3(sLineText, ref s14, new string[] { " ", "\09" });
                    if (sServerName != "")
                    {
                        MasSock.UserLimit[nC].sServerName = sServerName;
                        MasSock.UserLimit[nC].sName = s10;
                        MasSock.UserLimit[nC].nLimitCountMax = HUtil32.Str_ToInt(s14, 3000);
                        MasSock.UserLimit[nC].nLimitCountMin = 0;
                        nC++;
                    }
                }
                LSShare.UserLimit = nC;
                LoadList = null;
            }
            else
            {
                LSShare.MainOutMessage("[Critical Failure] file not found. .\\!UserLimit.txt");
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
            catch
            {
                LSShare.MainOutMessage("TFrmMasSoc.ServerStatus");
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
        public long dwKeepAliveTick;
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