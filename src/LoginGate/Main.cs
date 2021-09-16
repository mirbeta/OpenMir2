using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using SystemModule;
using SystemModule.Common;
using SystemModule.Sockets;
using SystemModule.Sockets.AsyncSocketClient;
using SystemModule.Sockets.AsyncSocketServer;

namespace LoginGate
{
    public partial class TFrmMain
    {
        private long dwShowMainLogTick = 0;
        private bool boShowLocked = false;
        private ArrayList TempLogList = null;
        private int nSessionCount = 0;
        private ArrayList StringList30C = null;
        private long dwSendKeepAliveTick = 0;
        private bool boServerReady = false;
        private ArrayList StringList318 = null;
        private long dwDecodeMsgTime = 0;
        private long dwReConnectServerTick = 0;
        private TUserSession[] g_SessionArray;
        private IList<string> ClientSockeMsgList;
        private ISocketServer ServerSocket;
        private IClientScoket ClientSocket;
        private string sProcMsg = string.Empty;

        public TFrmMain()
        {
            TempLogList = new ArrayList();
            StringList30C = new ArrayList();
            StringList318 = new ArrayList();
            dwDecodeMsgTime = 0;
            IniUserSessionArray();

            ClientSocket = new IClientScoket();
            ClientSocket.OnConnected += ClientSocketConnect;
            ClientSocket.OnDisconnected += ClientSocketDisconnect;
            ClientSocket.ReceivedDatagram += ClientSocketRead;
            ClientSocket.OnError += ClientSocketError;

            ServerSocket = new ISocketServer(ushort.MaxValue, 1024);
            ServerSocket.OnClientConnect += ServerSocketClientConnect;
            ServerSocket.OnClientDisconnect += ServerSocketClientDisconnect;
            ServerSocket.OnClientRead += ServerSocketClientRead;
            ServerSocket.OnClientError += ServerSocketClientError;
            ServerSocket.Init();
        }

        public void ServerSocketClientConnect(object sender, AsyncUserToken e)
        {
            TUserSession UserSession;
            string sLocalIPaddr;
            TSockaddr IPaddr;
            string sRemoteIPaddr = e.RemoteIPaddr;
            if (GateShare.g_boDynamicIPDisMode)
            {
                sLocalIPaddr = e.RemoteIPaddr;
            }
            else
            {
                sLocalIPaddr = IPAddress.Any.AddressFamily.ToString();
            }
            if (IsBlockIP(sRemoteIPaddr))
            {
                MainOutMessage("过滤连接: " + sRemoteIPaddr, 1);
                e.Socket.Close();
                return;
            }
            if (IsConnLimited(sRemoteIPaddr))
            {
                switch (GateShare.BlockMethod)
                {
                    case TBlockIPMethod.mDisconnect:
                        e.Socket.Close();
                        break;
                    case TBlockIPMethod.mBlock:
                        IPaddr = new TSockaddr();
                        IPaddr.nIPaddr = HUtil32.IpToInt(sRemoteIPaddr);
                        GateShare.TempBlockIPList.Add(IPaddr);
                        CloseConnect(sRemoteIPaddr);
                        break;
                    case TBlockIPMethod.mBlockList:
                        IPaddr = new TSockaddr();
                        IPaddr.nIPaddr = HUtil32.IpToInt(sRemoteIPaddr);
                        GateShare.BlockIPList.Add(IPaddr);
                        CloseConnect(sRemoteIPaddr);
                        break;
                }
                MainOutMessage("端口攻击: " + sRemoteIPaddr, 1);
                return;
            }
            if (GateShare.boGateReady)
            {
                for (var nSockIndex = 0; nSockIndex < GateShare.GATEMAXSESSION; nSockIndex++)
                {
                    UserSession = g_SessionArray[nSockIndex];
                    if (UserSession.Socket == null)
                    {
                        UserSession.Socket = e.Socket;
                        UserSession.sRemoteIPaddr = sRemoteIPaddr;
                        UserSession.nSendMsgLen = 0;
                        UserSession.bo0C = false;
                        UserSession.dw10Tick = HUtil32.GetTickCount();
                        UserSession.dwConnctCheckTick = HUtil32.GetTickCount();
                        UserSession.boSendAvailable = true;
                        UserSession.boSendCheck = false;
                        UserSession.nCheckSendLength = 0;
                        UserSession.n20 = 0;
                        UserSession.dwUserTimeOutTick = HUtil32.GetTickCount();
                        UserSession.SocketHandle = (int)e.Socket.Handle;
                        UserSession.sIP = sRemoteIPaddr;
                        UserSession.MsgList.Clear();
                        GateShare.socketMap.TryAdd(e.ConnectionId, nSockIndex);
                        nSessionCount++;
                        break;
                    }
                }
                if (!string.IsNullOrEmpty(e.ConnectionId))
                {
                    ClientSocket.SendText("%O" + (int)e.Socket.Handle + "/" + sRemoteIPaddr + "/" + sLocalIPaddr + "$");
                    MainOutMessage("Connect: " + sRemoteIPaddr, 5);
                }
                else
                {
                    e.Socket.Close();
                    MainOutMessage("Kick Off: " + sRemoteIPaddr, 1);
                }
            }
            else
            {
                e.Socket.Close();
                MainOutMessage("Kick Off: " + sRemoteIPaddr, 1);
            }
        }

        public void ServerSocketClientDisconnect(object sender, AsyncUserToken e)
        {
            TUserSession UserSession;
            TSockaddr IPaddr;
            string sRemoteIPaddr = e.RemoteIPaddr;
            long nIPaddr = HUtil32.IpToInt(sRemoteIPaddr);
            int nSockIndex = GateShare.socketMap[e.ConnectionId];
            for (var i = 0; i < GateShare.CurrIPaddrList.Count; i++)
            {
                IPaddr = GateShare.CurrIPaddrList[i];
                if (IPaddr.nIPaddr == nIPaddr)
                {
                    IPaddr.nCount -= 1;
                    if (IPaddr.nCount <= 0)
                    {
                        IPaddr = null;
                        GateShare.CurrIPaddrList.RemoveAt(i);
                    }
                    break;
                }
            }
            if ((nSockIndex >= 0) && (nSockIndex < GateShare.GATEMAXSESSION))
            {
                UserSession = g_SessionArray[nSockIndex];
                UserSession.Socket = null;
                UserSession.sRemoteIPaddr = "";
                UserSession.SocketHandle = -1;
                UserSession.MsgList.Clear();
                nSessionCount -= 1;
                if (GateShare.boGateReady)
                {
                    ClientSocket.SendText("%X" + e.Socket.Handle + "$");
                    MainOutMessage("DisConnect: " + sRemoteIPaddr, 5);
                }
            }
        }

        public void ServerSocketClientError(object sender, AsyncSocketErrorEventArgs e)
        {

        }

        public void ServerSocketClientRead(object sender, AsyncUserToken e)
        {
            TUserSession UserSession;
            string sReviceMsg;
            string s10;
            string s1C;
            int nPos;
            int nMsgLen;
            if (!GateShare.socketMap.TryGetValue(e.ConnectionId, out var nSockIndex))
            {
                return;
            }
            if ((nSockIndex >= 0) && (nSockIndex < GateShare.GATEMAXSESSION))
            {
                UserSession = g_SessionArray[nSockIndex];
                var nReviceLen = e.BytesReceived;
                var data = new byte[nReviceLen];
                Buffer.BlockCopy(e.ReceiveBuffer, e.Offset, data, 0, nReviceLen);
                sReviceMsg = HUtil32.GetString(data, 0, data.Length);
                if ((sReviceMsg != "") && boServerReady)
                {
                    nPos = sReviceMsg.IndexOf("*");
                    if (nPos > 0)
                    {
                        UserSession.boSendAvailable = true;
                        UserSession.boSendCheck = false;
                        UserSession.nCheckSendLength = 0;
                        s10 = sReviceMsg.Substring(1 - 1, nPos - 1);
                        s1C = sReviceMsg.Substring(nPos + 1 - 1, sReviceMsg.Length - nPos);
                        sReviceMsg = s10 + s1C;
                    }
                    nMsgLen = sReviceMsg.Length;
                    if ((sReviceMsg != "") && (GateShare.boGateReady) && (!GateShare.boKeepAliveTimcOut))
                    {
                        UserSession.dwConnctCheckTick = HUtil32.GetTickCount();
                        if ((HUtil32.GetTickCount() - UserSession.dwUserTimeOutTick) < 1000)
                        {
                            UserSession.n20 += nMsgLen;
                        }
                        else
                        {
                            UserSession.n20 = nMsgLen;
                        }
                        ClientSocket.SendText("%A" + (int)e.Socket.Handle + "/" + sReviceMsg + "$");
                    }
                }
            }
        }

        public void FormCloseQuery(System.Object Sender, System.ComponentModel.CancelEventArgs _e1)
        {
            //if (GateShare.boClose)
            //{
            //    return;
            //}
            //if (Application.MessageBox("是否确认退出服务器？", "提示信息", MB_YESNO + MB_ICONQUESTION) == IDYES)
            //{
            //    if (GateShare.boServiceStart)
            //    {
            //        StartTimer.Enabled = true;
            //        CanClose = false;
            //    }
            //}
            //else
            //{
            //    CanClose = false;
            //}
        }

        public void ClientSocketConnect(object sender, SystemModule.Sockets.Event.DSCClientConnectedEventArgs e)
        {
            GateShare.boGateReady = true;
            nSessionCount = 0;
            GateShare.dwKeepAliveTick = HUtil32.GetTickCount();
            ResUserSessionArray();
            boServerReady = true;
        }

        public void ClientSocketDisconnect(object sender, SystemModule.Sockets.Event.DSCClientConnectedEventArgs e)
        {
            TUserSession UserSession;
            for (var nIndex = 0; nIndex < GateShare.GATEMAXSESSION; nIndex++)
            {
                UserSession = g_SessionArray[nIndex];
                if (UserSession.Socket != null)
                {
                    UserSession.Socket.Close();
                }
                UserSession.Socket = null;
                UserSession.sRemoteIPaddr = "";
                UserSession.SocketHandle = -1;
            }
            ResUserSessionArray();
            ClientSockeMsgList.Clear();
            GateShare.boGateReady = false;
            nSessionCount = 0;
        }

        public void ClientSocketError(object sender, SystemModule.Sockets.Event.DSCClientErrorEventArgs e)
        {
            boServerReady = false;
        }

        public void ClientSocketRead(object sender, SystemModule.Sockets.Event.DSCClientDataInEventArgs e)
        {
            var sReviceMsg = HUtil32.GetString(e.Buff, 0, e.Buff.Length);
            ClientSockeMsgList.Add(sReviceMsg);
        }

        public void SendTimerTimer(System.Object Sender, System.EventArgs _e1)
        {
            int nIndex;
            TUserSession UserSession;
            //if (ServerSocket.Active)
            //{
            //    GateShare.n456A30 = ServerSocket.Socket.ActiveConnections;
            //}
            //if (GateShare.boSendHoldTimeOut)
            //{
            //    LbHold.Text = (GateShare.n456A30).ToString() + "#";
            //    if ((HUtil32.GetTickCount() - GateShare.dwSendHoldTick) > 3 * 1000)
            //    {
            //        GateShare.boSendHoldTimeOut = false;
            //    }
            //}
            //else
            //{
            //    LbHold.Text = (GateShare.n456A30).ToString();
            //}
            if (GateShare.boGateReady && (!GateShare.boKeepAliveTimcOut))
            {
                for (nIndex = 0; nIndex < GateShare.GATEMAXSESSION; nIndex++)
                {
                    UserSession = g_SessionArray[nIndex];
                    if (UserSession.Socket != null)
                    {
                        if ((HUtil32.GetTickCount() - UserSession.dwUserTimeOutTick) > 60 * 60 * 1000)
                        {
                            UserSession.Socket.Close();
                            UserSession.Socket = null;
                            UserSession.SocketHandle = -1;
                            UserSession.MsgList.Clear();
                            UserSession.sRemoteIPaddr = "";
                        }
                    }
                }
            }
            if (!GateShare.boGateReady && (GateShare.boServiceStart))
            {
                if ((HUtil32.GetTickCount() - dwReConnectServerTick) > 1000)// 30 * 1000
                {
                    dwReConnectServerTick = HUtil32.GetTickCount();
                    ClientSocket.Connect(GateShare.ServerAddr, GateShare.ServerPort);
                }
            }
        }

        public void TimerTimer(System.Object Sender, System.EventArgs _e1)
        {
            //if (ServerSocket.Active)
            //{
            //    StatusBar.Panels[0].Text = (ServerSocket.Port).ToString();
            //    if (GateShare.boSendHoldTimeOut)
            //    {
            //        StatusBar.Panels[2].Text = (nSessionCount).ToString() + "/#" + (ServerSocket.Socket.ActiveConnections).ToString();
            //    }
            //    else
            //    {
            //        StatusBar.Panels[2].Text = (nSessionCount).ToString() + "/" + (ServerSocket.Socket.ActiveConnections).ToString();
            //    }
            //}
            //else
            //{
            //    StatusBar.Panels[0].Text = "????";
            //    StatusBar.Panels[2].Text = "????";
            //}
            //Label2.Text = (dwDecodeMsgTime).ToString();
            //if (!GateShare.boGateReady)
            //{
            //    StatusBar.Panels[1].Text = "---]    [---";
            //}
            //else
            //{
            //    if (GateShare.boKeepAliveTimcOut)
            //    {
            //        StatusBar.Panels[1].Text = "---]$$$$[---";
            //    }
            //    else
            //    {
            //        StatusBar.Panels[1].Text = "-----][-----";
            //        LbLack.Text = (GateShare.n456A2C).ToString() + "/" + (GateShare.nSendMsgCount).ToString();
            //    }
            //}
        }

        public void DecodeTimerTimer(System.Object Sender, System.EventArgs _e1)
        {
            string sProcessMsg = string.Empty;
            string sSocketMsg = string.Empty;
            string sSocketHandle = string.Empty;
            int nSocketIndex = 0;
            int nMsgCount = 0;
            int nSendRetCode = 0;
            int nSocketHandle = 0;
            long dwDecodeTick = 0;
            long dwDecodeTime = 0;
            string sRemoteIPaddr = string.Empty;
            TUserSession UserSession;
            TSockaddr IPaddr;
            ShowMainLogMsg();
            if (GateShare.boDecodeLock || (!GateShare.boGateReady))
            {
                return;
            }
            try
            {
                dwDecodeTick = HUtil32.GetTickCount();
                GateShare.boDecodeLock = true;
                while (true)
                {
                    if (ClientSockeMsgList.Count <= 0)
                    {
                        break;
                    }
                    sProcessMsg = sProcMsg + ClientSockeMsgList[0];
                    sProcMsg = "";
                    ClientSockeMsgList.RemoveAt(0);
                    while (true)
                    {
                        if (HUtil32.TagCount(sProcessMsg, '$') < 1)
                        {
                            break;
                        }
                        sProcessMsg = HUtil32.ArrestStringEx(sProcessMsg, "%", "$", ref sSocketMsg);
                        if (sSocketMsg == "")
                        {
                            break;
                        }
                        if (sSocketMsg[0] == '+')
                        {
                            if (sSocketMsg[1] == '-')
                            {
                                CloseSocket(HUtil32.Str_ToInt(sSocketMsg.Substring(2, sSocketMsg.Length - 2), 0));
                                continue;
                            }
                            else
                            {
                                GateShare.dwKeepAliveTick = HUtil32.GetTickCount();
                                GateShare.boKeepAliveTimcOut = false;
                                continue;
                            }
                        }
                        sSocketMsg = HUtil32.GetValidStr3(sSocketMsg, ref sSocketHandle, new string[] { "/" });
                        nSocketHandle = HUtil32.Str_ToInt(sSocketHandle, -1);
                        if (nSocketHandle < 0)
                        {
                            continue;
                        }
                        for (nSocketIndex = 0; nSocketIndex < GateShare.GATEMAXSESSION; nSocketIndex++)
                        {
                            if (g_SessionArray[nSocketIndex].SocketHandle == nSocketHandle)
                            {
                                g_SessionArray[nSocketIndex].MsgList.Add(sSocketMsg);
                                break;
                            }
                        }
                    }
                }
                if (sProcessMsg != "")
                {
                    sProcMsg = sProcessMsg;
                }
                GateShare.nSendMsgCount = 0;
                GateShare.n456A2C = 0;
                StringList318.Clear();
                for (nSocketIndex = 0; nSocketIndex < GateShare.GATEMAXSESSION; nSocketIndex++)
                {
                    if (g_SessionArray[nSocketIndex].SocketHandle <= -1)
                    {
                        continue;
                    }
                    if ((HUtil32.GetTickCount() - g_SessionArray[nSocketIndex].dwConnctCheckTick) > GateShare.dwKeepConnectTimeOut)// 踢除超时无数据传输连接
                    {
                        sRemoteIPaddr = g_SessionArray[nSocketIndex].sRemoteIPaddr;
                        switch (GateShare.BlockMethod)
                        {
                            case TBlockIPMethod.mDisconnect:
                                g_SessionArray[nSocketIndex].Socket.Close();
                                break;
                            case TBlockIPMethod.mBlock:
                                IPaddr = new TSockaddr();
                                IPaddr.nIPaddr = HUtil32.IpToInt((sRemoteIPaddr as string));
                                GateShare.TempBlockIPList.Add(IPaddr);
                                CloseConnect(sRemoteIPaddr);
                                break;
                            case TBlockIPMethod.mBlockList:
                                IPaddr = new TSockaddr();
                                IPaddr.nIPaddr = HUtil32.IpToInt((sRemoteIPaddr as string));
                                GateShare.BlockIPList.Add(IPaddr);
                                CloseConnect(sRemoteIPaddr);
                                break;
                        }
                        MainOutMessage("端口空连接攻击: " + sRemoteIPaddr, 1);
                        continue;
                    }
                    while (true)
                    {
                        if (g_SessionArray[nSocketIndex].MsgList.Count <= 0)
                        {
                            break;
                        }
                        UserSession = g_SessionArray[nSocketIndex];
                        nSendRetCode = SendUserMsg(UserSession, UserSession.MsgList[0]);
                        if ((nSendRetCode >= 0))
                        {
                            if (nSendRetCode == 1)
                            {
                                UserSession.dwConnctCheckTick = HUtil32.GetTickCount();
                                UserSession.MsgList.RemoveAt(0);
                                continue;
                            }
                            if (UserSession.MsgList.Count > 100)
                            {
                                nMsgCount = 0;
                                while (nMsgCount != 51)
                                {
                                    UserSession.MsgList.RemoveAt(0);
                                    nMsgCount++;
                                }
                            }
                            GateShare.n456A2C += UserSession.MsgList.Count;
                            MainOutMessage(UserSession.sIP + " : " + (UserSession.MsgList.Count).ToString(), 5);
                            GateShare.nSendMsgCount++;
                        }
                        else
                        {
                            UserSession.SocketHandle = -1;
                            UserSession.Socket = null;
                            UserSession.MsgList.Clear();
                        }
                    }
                }
                if ((HUtil32.GetTickCount() - dwSendKeepAliveTick) > 2 * 1000)
                {
                    dwSendKeepAliveTick = HUtil32.GetTickCount();
                    if (GateShare.boGateReady)
                    {
                        ClientSocket.SendText("%--$");
                    }
                }
                if ((HUtil32.GetTickCount() - GateShare.dwKeepAliveTick) > 10 * 1000)
                {
                    GateShare.boKeepAliveTimcOut = true;
                    ClientSocket.Disconnect();
                }
            }
            finally
            {
                GateShare.boDecodeLock = false;
            }
            dwDecodeTime = HUtil32.GetTickCount() - dwDecodeTick;
            if (dwDecodeMsgTime < dwDecodeTime)
            {
                dwDecodeMsgTime = dwDecodeTime;
            }
            if (dwDecodeMsgTime > 50)
            {
                dwDecodeMsgTime -= 50;
            }
        }

        private void CloseSocket(int nSocketHandle)
        {
            int nIndex;
            TUserSession UserSession;
            for (nIndex = 0; nIndex < GateShare.GATEMAXSESSION; nIndex++)
            {
                UserSession = g_SessionArray[nIndex];
                if ((UserSession.Socket != null) && (UserSession.SocketHandle == nSocketHandle))
                {
                    UserSession.Socket.Close();
                    break;
                }
            }
        }

        private int SendUserMsg(TUserSession UserSession, string sSendMsg)
        {
            int result;
            result = -1;
            if (UserSession.Socket != null)
            {
                if (!UserSession.bo0C)
                {
                    if (!UserSession.boSendAvailable && (HUtil32.GetTickCount() > UserSession.dwSendLockTimeOut))
                    {
                        UserSession.boSendAvailable = true;
                        UserSession.nCheckSendLength = 0;
                        GateShare.boSendHoldTimeOut = true;
                        GateShare.dwSendHoldTick = HUtil32.GetTickCount();
                    }
                    if (UserSession.boSendAvailable)
                    {
                        if (UserSession.nCheckSendLength >= 250)
                        {
                            if (!UserSession.boSendCheck)
                            {
                                UserSession.boSendCheck = true;
                                sSendMsg = "*" + sSendMsg;
                            }
                            if (UserSession.nCheckSendLength >= 512)
                            {
                                UserSession.boSendAvailable = false;
                                UserSession.dwSendLockTimeOut = HUtil32.GetTickCount() + 3 * 1000;
                            }
                        }
                        UserSession.Socket.SendText(sSendMsg);
                        UserSession.nSendMsgLen += sSendMsg.Length;
                        UserSession.nCheckSendLength += sSendMsg.Length;
                        result = 1;
                    }
                    else
                    {
                        result = 0;
                    }
                }
                else
                {
                    result = 0;
                }
            }
            return result;
        }

        private void LoadConfig()
        {
            IniFile Conf;
            string sConfigFileName;
            sConfigFileName = ".\\Config.ini";
            Conf = new IniFile(sConfigFileName);
            GateShare.TitleName = Conf.ReadString(GateShare.GateClass, "Title", GateShare.TitleName);
            GateShare.ServerPort = Conf.ReadInteger(GateShare.GateClass, "ServerPort", GateShare.ServerPort);
            GateShare.ServerAddr = Conf.ReadString(GateShare.GateClass, "ServerAddr", GateShare.ServerAddr);
            GateShare.GatePort = Conf.ReadInteger(GateShare.GateClass, "GatePort", GateShare.GatePort);
            GateShare.GateAddr = Conf.ReadString(GateShare.GateClass, "GateAddr", GateShare.GateAddr);
            GateShare.nShowLogLevel = Conf.ReadInteger(GateShare.GateClass, "ShowLogLevel", GateShare.nShowLogLevel);
            GateShare.BlockMethod = ((TBlockIPMethod)(Conf.ReadInteger(GateShare.GateClass, "BlockMethod", ((int)GateShare.BlockMethod))));
            if (Conf.ReadInteger(GateShare.GateClass, "KeepConnectTimeOut", -1) <= 0)
            {
                Conf.WriteInteger(GateShare.GateClass, "KeepConnectTimeOut", GateShare.dwKeepConnectTimeOut);
            }
            GateShare.nMaxConnOfIPaddr = Conf.ReadInteger(GateShare.GateClass, "MaxConnOfIPaddr", GateShare.nMaxConnOfIPaddr);
            GateShare.dwKeepConnectTimeOut = Conf.ReadInteger<long>(GateShare.GateClass, "KeepConnectTimeOut", GateShare.dwKeepConnectTimeOut);
            GateShare.g_boDynamicIPDisMode = Conf.ReadBool(GateShare.GateClass, "DynamicIPDisMode", GateShare.g_boDynamicIPDisMode);
            Conf = null;
            GateShare.LoadBlockIPFile();
        }

        private void StartService()
        {
            try
            {
                MainOutMessage("欢迎使用翎风系统软件...",0);
                MainOutMessage("网站:http://www.gameofmir.com",0);
                MainOutMessage("论坛:http://bbs.gameofmir.com",0);
                MainOutMessage("正在启动服务...", 3);
                GateShare.boServiceStart = true;
                GateShare.boGateReady = false;
                boServerReady = false;
                nSessionCount = 0;
                dwReConnectServerTick = HUtil32.GetTickCount() - 25 * 1000;
                GateShare.boKeepAliveTimcOut = false;
                GateShare.nSendMsgCount = 0;
                GateShare.n456A2C = 0;
                dwSendKeepAliveTick = HUtil32.GetTickCount();
                GateShare.boSendHoldTimeOut = false;
                GateShare.dwSendHoldTick = HUtil32.GetTickCount();
                GateShare.CurrIPaddrList = new List<TSockaddr>();
                GateShare.BlockIPList = new List<TSockaddr>();
                GateShare.TempBlockIPList = new List<TSockaddr>();
                ClientSockeMsgList = new List<string>();
                ResUserSessionArray();
                LoadConfig();

                ClientSocket.Connect(GateShare.ServerAddr, GateShare.ServerPort);
                ServerSocket.Start(GateShare.GateAddr, GateShare.GatePort);
                //SendTimer.Enabled = true;
                MainOutMessage("启动服务完成...", 3);
            }
            catch (Exception E)
            {
                MainOutMessage(E.Message, 0);
            }
        }

        public static void MainOutMessage(string sMsg, int nMsgLevel)
        {
            if (nMsgLevel <= GateShare.nShowLogLevel)
            {
                string tMsg = "[" + DateTime.Now.ToString() + "] " + sMsg;
                GateShare.MainLogMsgList.Add(tMsg);
            }
        }

        private void StopService()
        {
            //int I;
            //int nSockIdx;
            //TSockaddr IPaddr;
            //MainOutMessage("正在停止服务...", 3);
            //GateShare.boServiceStart = false;
            //GateShare.boGateReady = false;
            //MENU_CONTROL_START.Enabled = true;
            //MENU_CONTROL_STOP.Enabled = false;
            //SendTimer.Enabled = false;
            //for (nSockIdx = 0; nSockIdx < GATEMAXSESSION; nSockIdx++)
            //{
            //    if (g_SessionArray[nSockIdx].Socket != null)
            //    {
            //        g_SessionArray[nSockIdx].Socket.Close;
            //    }
            //}
            //GateShare.SaveBlockIPList();
            //ServerSocket.Close;
            //ClientSocket.Close;
            //ClientSockeMsgList.Free;
            //for (I = 0; I < GateShare.CurrIPaddrList.Count; I++)
            //{
            //    IPaddr = GateShare.CurrIPaddrList[I];
            //    this.Dispose(IPaddr);
            //}
            //GateShare.CurrIPaddrList.Free;
            //for (I = 0; I < GateShare.BlockIPList.Count; I++)
            //{
            //    IPaddr = GateShare.BlockIPList[I];
            //    this.Dispose(IPaddr);
            //}
            //GateShare.BlockIPList.Free;
            //for (I = 0; I < GateShare.TempBlockIPList.Count; I++)
            //{
            //    IPaddr = GateShare.TempBlockIPList[I];
            //    this.Dispose(IPaddr);
            //}
            //GateShare.TempBlockIPList.Free;
            //MainOutMessage("停止服务完成...", 3);
        }

        private void ResUserSessionArray()
        {
            TUserSession UserSession;
            int nIndex;
            for (nIndex = 0; nIndex < GateShare.GATEMAXSESSION; nIndex++)
            {
                UserSession = g_SessionArray[nIndex];
                UserSession.Socket = null;
                UserSession.sRemoteIPaddr = "";
                UserSession.SocketHandle = -1;
                UserSession.MsgList.Clear();
            }
        }

        private void IniUserSessionArray()
        {
            TUserSession UserSession;
            for (var nIndex = 0; nIndex < GateShare.GATEMAXSESSION; nIndex++)
            {
                UserSession = g_SessionArray[nIndex];
                UserSession.Socket = null;
                UserSession.sRemoteIPaddr = "";
                UserSession.nSendMsgLen = 0;
                UserSession.bo0C = false;
                UserSession.dw10Tick = HUtil32.GetTickCount();
                UserSession.boSendAvailable = true;
                UserSession.boSendCheck = false;
                UserSession.nCheckSendLength = 0;
                UserSession.n20 = 0;
                UserSession.dwUserTimeOutTick = HUtil32.GetTickCount();
                UserSession.SocketHandle = -1;
                UserSession.MsgList = new List<string>();
            }
        }

        public void StartTimerTimer(System.Object Sender, System.EventArgs _e1)
        {
            if (GateShare.boStarted)
            {
                //StartTimer.Enabled = false;
                StopService();
                GateShare.boClose = true;
            }
            else
            {
                GateShare.boStarted = true;
                // StartTimer.Enabled = false;
                StartService();
            }
        }

        private void CloseConnect(string sIPaddr)
        {
            bool boCheck;
            if (ServerSocket.Active)
            {
                while (true)
                {
                    boCheck = false;
                    var socketList = ServerSocket.GetSockets();
                    for (var i = 0; i < socketList.Count; i++)
                    {
                        if (sIPaddr == socketList[i].RemoteIPaddr)
                        {
                            socketList[i].Socket.Close();
                            boCheck = true;
                            break;
                        }
                    }
                    if (!boCheck)
                    {
                        break;
                    }
                }
            }
        }

        private bool IsBlockIP(string sIPaddr)
        {
            bool result = false;
            var nIPaddr = HUtil32.IpToInt((sIPaddr as string));
            TSockaddr IPaddr = null;
            for (var i = 0; i < GateShare.TempBlockIPList.Count; i++)
            {
                IPaddr = GateShare.TempBlockIPList[i];
                if (IPaddr.nIPaddr == nIPaddr)
                {
                    result = true;
                    return result;
                }
            }
            for (var i = 0; i < GateShare.BlockIPList.Count; i++)
            {
                IPaddr = GateShare.BlockIPList[i];
                if (IPaddr.nIPaddr == nIPaddr)
                {
                    result = true;
                    return result;
                }
            }
            return result;
        }

        private bool IsConnLimited(string sIPaddr)
        {
            bool result = false;
            TSockaddr IPaddr;
            bool boDenyConnect = false;
            long nIPaddr = HUtil32.IpToInt(sIPaddr);
            for (var i = 0; i < GateShare.CurrIPaddrList.Count; i++)
            {
                IPaddr = GateShare.CurrIPaddrList[i];
                if (IPaddr.nIPaddr == nIPaddr)
                {
                    IPaddr.nCount++;
                    if (HUtil32.GetTickCount() - IPaddr.dwIPCountTick1 < 1000)
                    {
                        IPaddr.nIPCount1++;
                        if (IPaddr.nIPCount1 >= GateShare.nIPCountLimit1)
                        {
                            boDenyConnect = true;
                        }
                    }
                    else
                    {
                        IPaddr.dwIPCountTick1 = HUtil32.GetTickCount();
                        IPaddr.nIPCount1 = 0;
                    }
                    if (HUtil32.GetTickCount() - IPaddr.dwIPCountTick2 < 3000)
                    {
                        IPaddr.nIPCount2++;
                        if (IPaddr.nIPCount2 >= GateShare.nIPCountLimit2)
                        {
                            boDenyConnect = true;
                        }
                    }
                    else
                    {
                        IPaddr.dwIPCountTick2 = HUtil32.GetTickCount();
                        IPaddr.nIPCount2 = 0;
                    }
                    if (IPaddr.nCount > GateShare.nMaxConnOfIPaddr)
                    {
                        boDenyConnect = true;
                    }
                    result = boDenyConnect;
                    return result;
                }
            }
            IPaddr = new TSockaddr();
            IPaddr.nIPaddr = nIPaddr;
            IPaddr.nCount = 1;
            GateShare.CurrIPaddrList.Add(IPaddr);
            return result;
        }

        private void ShowMainLogMsg()
        {
            if ((HUtil32.GetTickCount() - dwShowMainLogTick) < 200)
            {
                return;
            }
            dwShowMainLogTick = HUtil32.GetTickCount();
            boShowLocked = true;
            for (var i = 0; i < GateShare.MainLogMsgList.Count; i++)
            {
                TempLogList.Add(GateShare.MainLogMsgList[i]);
            }
            GateShare.MainLogMsgList.Clear();
            for (var i = 0; i < TempLogList.Count; i++)
            {
                Console.WriteLine(TempLogList[i]);
            }
            TempLogList.Clear();
            boShowLocked = false;
        }
    }

    public class TUserSession
    {
        public Socket Socket;
        public string sRemoteIPaddr;
        public int nSendMsgLen;
        public bool bo0C;
        public long dw10Tick;
        public int nCheckSendLength;
        public bool boSendAvailable;
        public bool boSendCheck;
        public long dwSendLockTimeOut;
        public int n20;
        public long dwUserTimeOutTick;
        public int SocketHandle;
        public string sIP;
        public IList<string> MsgList;
        public long dwConnctCheckTick;
    }
}