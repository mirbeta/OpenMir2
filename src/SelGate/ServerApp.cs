using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using SystemModule;
using SystemModule.Common;

namespace SelGate
{
    public class ServerApp
    {
        private long dwShowMainLogTick = 0;
        private ArrayList TempLogList = null;
        private long dwReConnectServerTick = 0;
        private Timer logTimer;

        public ServerApp(GateServer gateServer, GateClient gateClient)
        {
            GateShare.Initialization();
            TempLogList = new ArrayList();
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

        public void SendTimerTimer(System.Object Sender, System.EventArgs _e1)
        {
            int nIndex;
            TUserSession UserSession;
            // if (ServerSocket.Active)
            // {
            //    GateShare.n456A30 = ServerSocket.Socket.ActiveConnections;
            // }
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
            //清理超时则会话
            if (GateShare.boGateReady && (!GateShare.boKeepAliveTimcOut))
            {
                for (nIndex = 0; nIndex < GateShare.GATEMAXSESSION; nIndex++)
                {
                    UserSession = GateShare.g_SessionArray[nIndex];
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

        private void LoadConfig()
        {
            IniFile Conf;
            string sConfigFileName;
            sConfigFileName = ".\\Config.ini";
            Conf = new IniFile(sConfigFileName);
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

        public void StartService()
        {
            try
            {
                logTimer = new Timer(ShowMainLogMsg, null, 0, 100);
                GateShare.MainOutMessage("欢迎使用翎风系统软件...", 0);
                GateShare.MainOutMessage("网站:http://www.gameofmir.com", 0);
                GateShare.MainOutMessage("论坛:http://bbs.gameofmir.com", 0);
                GateShare.MainOutMessage("正在启动服务...", 3);
                GateShare.boServiceStart = true;
                GateShare.boGateReady = false;
                GateShare.boServerReady = false;
                GateShare.nSessionCount = 0;
                dwReConnectServerTick = HUtil32.GetTickCount() - 25 * 1000;
                GateShare.boKeepAliveTimcOut = false;
                GateShare.nSendMsgCount = 0;
                //dwSendKeepAliveTick = HUtil32.GetTickCount();
                GateShare.boSendHoldTimeOut = false;
                GateShare.dwSendHoldTick = HUtil32.GetTickCount();
                GateShare.CurrIPaddrList = new List<TSockaddr>();
                GateShare.BlockIPList = new List<TSockaddr>();
                GateShare.TempBlockIPList = new List<TSockaddr>();
                LoadConfig();
                GateShare.MainOutMessage("启动服务完成...", 3);
            }
            catch (Exception E)
            {
                GateShare.MainOutMessage(E.Message, 0);
            }
        }

        public void StopService()
        {
            //int I;
            //int nSockIdx;
            //TSockaddr IPaddr;
            //MainOutMessage("正在停止服务...", 3);
            //GateShare.boServiceStart = false;
            //GateShare.boGateReady = false;
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

        private void IniUserSessionArray()
        {
            TUserSession UserSession;
            GateShare.g_SessionArray = new TUserSession[GateShare.GATEMAXSESSION];
            for (var nIndex = 0; nIndex < GateShare.GATEMAXSESSION; nIndex++)
            {
                UserSession = new TUserSession();
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
                GateShare.g_SessionArray[nIndex] = UserSession;
            }
        }

        public void Start()
        {
            IniUserSessionArray();
        }

        private void ShowMainLogMsg(object obj)
        {
            if ((HUtil32.GetTickCount() - dwShowMainLogTick) < 200)
            {
                return;
            }
            dwShowMainLogTick = HUtil32.GetTickCount();
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
        }
    }
}