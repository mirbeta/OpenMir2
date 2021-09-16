using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using SystemModule;
using SystemModule.Common;
using SystemModule.Sockets;

namespace SelGate
{
    public class TFrmMain
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
        public static TUserSession[] g_SessionArray;
        public static ArrayList ClientSockeMsgList = null;
        public static string sProcMsg = String.Empty;
        public const int GATEMAXSESSION = 10000;
        private readonly IClientScoket ClientSocket;
        private readonly ISocketServer ServerSocket;


        public TFrmMain()
        {

        }

        public static void MainOutMessage(string sMsg, int nMsgLevel)
        {
            string tMsg;
            if (nMsgLevel <= GateShare.nShowLogLevel)
            {
                tMsg = "[" + DateTime.Now.ToString() + "] " + sMsg;
                GateShare.MainLogMsgList.Add(tMsg);
            }
        }

        public void ServerSocketClientConnect(Object Sender, Socket Socket)
        {
            TUserSession UserSession;
            string sRemoteIPaddr = string.Empty;
            string sLocalIPaddr = string.Empty;
            int nSockIndex;
            TSockaddr IPaddr;
            //Socket.nIndex = -1;
            //sRemoteIPaddr = Socket.RemoteAddress;
            //if (GateShare.g_boDynamicIPDisMode)
            //{
            //    sLocalIPaddr = ClientSocket.Socket.RemoteAddress;
            //}
            //else
            //{
            //    sLocalIPaddr = Socket.LocalAddress;
            //}
            if (IsBlockIP(sRemoteIPaddr))
            {
                MainOutMessage("过滤连接: " + sRemoteIPaddr, 1);
                Socket.Close();
                return;
            }
            if (IsConnLimited(sRemoteIPaddr))
            {
                switch (GateShare.BlockMethod)
                {
                    case TBlockIPMethod.mDisconnect:
                        Socket.Close();
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
                for (nSockIndex = 0; nSockIndex < GATEMAXSESSION; nSockIndex++)
                {
                    UserSession = g_SessionArray[nSockIndex];
                    if (UserSession.Socket == null)
                    {
                        UserSession.Socket = Socket;
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
                        UserSession.SocketHandle = (int)Socket.Handle;
                        UserSession.sIP = sRemoteIPaddr;
                        UserSession.MsgList.Clear();
                        //Socket.nIndex = nSockIndex;
                        nSessionCount++;
                        break;
                    }
                }
                //if (Socket.nIndex >= 0)
                //{
                //    ClientSocket.SendText("%O" + (int)Socket.Handle + "/" + sRemoteIPaddr + "/" + sLocalIPaddr + "$");
                //    MainOutMessage("Connect: " + sRemoteIPaddr, 5);
                //}
                //else
                //{
                //    Socket.Close();
                //    MainOutMessage("Kick Off: " + sRemoteIPaddr, 1);
                //}
            }
            else
            {
                Socket.Close();
                MainOutMessage("Kick Off: " + sRemoteIPaddr, 1);
            }
        }

        public void ServerSocketClientDisconnect(Object Sender, Socket Socket)
        {
            TUserSession UserSession;
            int nSockIndex = 0;
            string sRemoteIPaddr = string.Empty;
            TSockaddr IPaddr = null;
            //sRemoteIPaddr = Socket.RemoteAddress;
            long nIPaddr = HUtil32.IpToInt(sRemoteIPaddr);
            //nSockIndex = Socket.nIndex;
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
            if ((nSockIndex >= 0) && (nSockIndex < GATEMAXSESSION))
            {
                UserSession = g_SessionArray[nSockIndex];
                UserSession.Socket = null;
                UserSession.sRemoteIPaddr = "";
                UserSession.SocketHandle = -1;
                UserSession.MsgList.Clear();
                nSessionCount -= 1;
                if (GateShare.boGateReady)
                {
                    ClientSocket.SendText("%X" + (int)Socket.Handle + "$");
                    MainOutMessage("DisConnect: " + sRemoteIPaddr, 5);
                }
            }
        }

        public void ServerSocketClientError(Object Sender, Socket Socket)
        {
 
        }

        public void ServerSocketClientRead(Object Sender, Socket Socket)
        {
            TUserSession UserSession;
            int nSockIndex = 0;
            string sReviceMsg = string.Empty;
            string s10;
            string s1C;
            int nPos;
            int nMsgLen;
           // nSockIndex = Socket.nIndex;
            if ((nSockIndex >= 0) && (nSockIndex < GATEMAXSESSION))
            {
                UserSession = g_SessionArray[nSockIndex];
                //sReviceMsg = Socket.ReceiveText;
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
                        ClientSocket.SendText("%A" + (int)Socket.Handle + "/" + sReviceMsg + "$");
                    }
                }
            }
        }

        public void FormDestroy(Object Sender)
        {
            //int nIndex;
            //StringList30C.Free;
            //TempLogList.Free;
            //for (nIndex = 0; nIndex < GATEMAXSESSION; nIndex++)
            //{
            //    g_SessionArray[nIndex].MsgList.Free;
            //}
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

        public void ClientSocketConnect(Object Sender, Socket Socket)
        {
            GateShare.boGateReady = true;
            nSessionCount = 0;
            GateShare.dwKeepAliveTick = HUtil32.GetTickCount();
            ResUserSessionArray();
            boServerReady = true;
        }

        public void ClientSocketDisconnect(Object Sender, Socket Socket)
        {
            TUserSession UserSession;
            int nIndex;
            for (nIndex = 0; nIndex < GATEMAXSESSION; nIndex++)
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

        public void ClientSocketError(Object Sender, Socket Socket)
        {
            boServerReady = false;
        }

        public void ClientSocketRead(Object Sender, Socket Socket)
        {
            //string sRecvMsg;
            //sRecvMsg = Socket.ReceiveText;
            //ClientSockeMsgList.Add(sRecvMsg);
        }

        public void SendTimerTimer(System.Object Sender, System.EventArgs _e1)
        {
            int nIndex;
            TUserSession UserSession;
            if (ServerSocket.Active)
            {
               // GateShare.n456A30 = ServerSocket.Socket.ActiveConnections;
            }
            if (GateShare.boSendHoldTimeOut)
            {
                //LbHold.Text = (GateShare.n456A30).ToString() + "#";
                if ((HUtil32.GetTickCount() - GateShare.dwSendHoldTick) > 3 * 1000)
                {
                    GateShare.boSendHoldTimeOut = false;
                }
            }
            else
            {
                //LbHold.Text = (GateShare.n456A30).ToString();
            }
            if (GateShare.boGateReady && (!GateShare.boKeepAliveTimcOut))
            {
                for (nIndex = 0; nIndex < GATEMAXSESSION; nIndex++)
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
                    //ClientSocket.Port = GateShare.ServerPort;
                    //ClientSocket.Host = GateShare.ServerAddr;
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
            //    StatusBar.Panels[1].Text = "未连接";
            //}
            //else
            //{
            //    if (GateShare.boKeepAliveTimcOut)
            //    {
            //        StatusBar.Panels[1].Text = "超时";
            //    }
            //    else
            //    {
            //        StatusBar.Panels[1].Text = "已连接";
            //        LbLack.Text = (GateShare.n456A2C).ToString() + "/" + (GateShare.nSendMsgCount).ToString();
            //    }
            //}
        }

        public void DecodeTimerTimer(System.Object Sender, System.EventArgs _e1)
        {
            string sProcessMsg = string.Empty;
            string sSocketMsg = string.Empty;
            string sSocketHandle = string.Empty;
            int nSocketIndex;
            int nMsgCount;
            int nSendRetCode;
            int nSocketHandle;
            long dwDecodeTick;
            long dwDecodeTime;
            string sRemoteIPaddr;
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
                sProcessMsg = "";
                while (true)
                {
                    if (ClientSockeMsgList.Count <= 0)
                    {
                        break;
                    }
                    sProcessMsg = sProcMsg + ClientSockeMsgList[0];
                    sProcMsg = "";
                    ClientSockeMsgList.Remove(0);
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
                                CloseSocket(HUtil32.Str_ToInt(sSocketMsg.Substring(3 - 1, sSocketMsg.Length - 2), 0));
                                continue;
                            }
                            else
                            {
                                // 0x004521B7
                                GateShare.dwKeepAliveTick = HUtil32.GetTickCount();
                                GateShare.boKeepAliveTimcOut = false;
                                continue;
                            }
                        }
                        // 0x004521CD
                        sSocketMsg = HUtil32.GetValidStr3(sSocketMsg, ref sSocketHandle, new string[] { "/" });
                        nSocketHandle = HUtil32.Str_ToInt(sSocketHandle, -1);
                        if (nSocketHandle < 0)
                        {
                            continue;
                        }
                        for (nSocketIndex = 0; nSocketIndex < GATEMAXSESSION; nSocketIndex++)
                        {
                            if (g_SessionArray[nSocketIndex].SocketHandle == nSocketHandle)
                            {
                                g_SessionArray[nSocketIndex].MsgList.Add(sSocketMsg);
                                break;
                            }
                        }
                    }
                    // 0x00452246
                }
                // 0x452252
                // if sProcessMsg <> '' then ClientSockeMsgList.Add(sProcessMsg);
                if (sProcessMsg != "")
                {
                    sProcMsg = sProcessMsg;
                }
                GateShare.nSendMsgCount = 0;
                GateShare.n456A2C = 0;
                StringList318.Clear();
                for (nSocketIndex = 0; nSocketIndex < GATEMAXSESSION; nSocketIndex++)
                {
                    if (g_SessionArray[nSocketIndex].SocketHandle <= -1)// 踢除超时无数据传输连接
                    {
                        continue;
                    }
                    if ((HUtil32.GetTickCount() - g_SessionArray[nSocketIndex].dwConnctCheckTick) > GateShare.dwKeepConnectTimeOut)
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
                            // 0x004523A4
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
                    //ClientSocket.Close();
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
            for (nIndex = 0; nIndex < GATEMAXSESSION; nIndex++)
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
            //Conf.Free;
            GateShare.LoadBlockIPFile();
        }

        private void StartService()
        {
            try
            {
                // MainOutMessage('欢迎使用翎风系统软件...',0);
                // MainOutMessage('网站:http://www.gameofmir.com',0);
                // MainOutMessage('论坛:http://bbs.gameofmir.com',0);
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
                ClientSockeMsgList = new ArrayList();
                ResUserSessionArray();
                LoadConfig();
                //ClientSocket.Active = false;
                //ClientSocket.Host = GateShare.ServerAddr;
                //ClientSocket.Port = GateShare.ServerPort;
                //ClientSocket.Active = true;
                //ServerSocket.Active = false;
                //ServerSocket.Address = GateShare.GateAddr;
                //ServerSocket.Port = GateShare.GatePort;
                //ServerSocket.Active = true;
                //SendTimer.Enabled = true;
                MainOutMessage("启动服务完成...", 3);
            }
            catch (Exception E)
            {
                MainOutMessage(E.Message, 0);
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
            for (nIndex = 0; nIndex < GATEMAXSESSION; nIndex++)
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
            int nIndex;
            for (nIndex = 0; nIndex < GATEMAXSESSION; nIndex++)
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
                //this.Close();
            }
            else
            {
                GateShare.boStarted = true;
                //StartTimer.Enabled = false;
                StartService();
            }
        }

        public void FormCreate(System.Object Sender, System.EventArgs _e1)
        {
            TempLogList = new ArrayList();
            StringList30C = new ArrayList();
            StringList318 = new ArrayList();
            dwDecodeMsgTime = 0;
            IniUserSessionArray();
        }

        public void CloseConnect(string sIPaddr)
        {
            int i;
            bool boCheck;
            if (ServerSocket.Active)
            {
                while (true)
                {
                    //boCheck = false;
                    //for (i = 0; i < ServerSocket.Socket.ActiveConnections; i++)
                    //{
                    //    if (sIPaddr == ServerSocket.Socket.Connections[i].RemoteAddress)
                    //    {
                    //        ServerSocket.Socket.Connections[i].Close();
                    //        boCheck = true;
                    //        break;
                    //    }
                    //}
                    //if (!boCheck)
                    //{
                    //    break;
                    //}
                }
            }
        }

        private bool IsBlockIP(string sIPaddr)
        {
            bool result = false;
            TSockaddr IPaddr;
            long nIPaddr = HUtil32.IpToInt(sIPaddr);
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
            bool result;
            TSockaddr IPaddr;
            bool boDenyConnect;
            result = false;
            boDenyConnect = false;
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
            int i;
            if ((HUtil32.GetTickCount() - dwShowMainLogTick) < 200)
            {
                return;
            }
            dwShowMainLogTick = HUtil32.GetTickCount();
            try
            {
                boShowLocked = true;
                for (i = 0; i < GateShare.MainLogMsgList.Count; i++)
                {
                    TempLogList.Add(GateShare.MainLogMsgList[i]);
                }
                GateShare.MainLogMsgList.Clear();
                for (i = 0; i < TempLogList.Count; i++)
                {
                    Console.WriteLine(TempLogList[i]);
                }
                TempLogList.Clear();
            }
            finally
            {
                boShowLocked = false;
            }
        }
    }

    public struct TUserSession
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