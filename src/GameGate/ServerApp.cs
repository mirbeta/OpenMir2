using System;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SystemModule;

namespace GameGate
{
    public class ServerApp
    {
        private long dwShowMainLogTick = 0;
        private bool boShowLocked = false;
        private ArrayList TempLogList = null;
        private long dwProcessPacketTick = 0;
        private long dwLoopCheckTick = 0;
        private long dwLoopTime = 0;
        private long dwProcessServerMsgTime = 0;
        private long dwReConnectServerTime = 0;
        private long dwRefConsolMsgTick = 0;
        private long dwRefConsoleMsgTick = 0;
        private int nDeCodeMsgSize = 0;
        private int nSendBlockSize = 0;
        private int nProcessMsgSize = 0;
        private int nHumLogonMsgSize = 0;
        private int nHumPlayMsgSize = 0;
        private Timer decodeTimer;
        private Timer sendTime;
        private readonly ServerService _serverService;
        private readonly RunGateClient _runGateClient;
        private readonly SessionManager _sessionManager;

        public ServerApp(ServerService serverService, RunGateClient runGateClient, SessionManager sessionManager)
        {
            _serverService = serverService;
            _runGateClient = runGateClient;
            _sessionManager = sessionManager;
            TempLogList = new ArrayList();
            dwLoopCheckTick = HUtil32.GetTickCount();
        }

        public async Task Start()
        {
            var gTasks = new Task[2];
            var consumerTask1 = Task.Factory.StartNew(ProcessReviceMessage);
            gTasks[0] = consumerTask1;

            var consumerTask2 = Task.Factory.StartNew(ProcessSendMessage);
            gTasks[1] = consumerTask2;

            await Task.WhenAll(gTasks);
        }

        /// <summary>
        /// 处理客户端发过来的消息
        /// </summary>
        private async Task ProcessReviceMessage()
        {
            while (await GateShare.ReviceMsgList.Reader.WaitToReadAsync())
            {
                if (GateShare.ReviceMsgList.Reader.TryRead(out var message))
                {
                    var clientSession = _sessionManager.GetSession(message.UserCientId);
                    clientSession?.HandleUserPacket(message);
                }
            }
        }

        /// <summary>
        /// 处理M2发过来的消息
        /// </summary>
        private async Task ProcessSendMessage()
        {
            while (await GateShare.SendMsgList.Reader.WaitToReadAsync())
            {
                if (GateShare.SendMsgList.Reader.TryRead(out var message))
                {
                    //todo 需要处理多个网关的数据
                    ProcessPacket(message);
                }
            }
        }

        private void DecodeTimer(object obj)
        {
            ShowMainLogMsg();
            if (!GateShare.boDecodeMsgLock)
            {
                if ((HUtil32.GetTickCount() - dwRefConsoleMsgTick) >= 10000)
                {
                    dwRefConsoleMsgTick = HUtil32.GetTickCount();
                    if (!GateShare.boShowBite)
                    {
                        Debug.WriteLine("接收: " + _serverService.NReviceMsgSize / 1024 + " KB");
                        //Debug.WriteLine( "服务器通讯: " + _userClient.nBufferOfM2Size / 1024 + " KB");
                        Debug.WriteLine("编码: " + nProcessMsgSize / 1024 + " KB");
                        Debug.WriteLine("登录: " + nHumLogonMsgSize / 1024 + " KB");
                        Debug.WriteLine("普通: " + nHumPlayMsgSize / 1024 + " KB");
                        Debug.WriteLine("解码: " + nDeCodeMsgSize / 1024 + " KB");
                        Debug.WriteLine("发送: " + nSendBlockSize / 1024 + " KB");
                    }
                    else
                    {
                        Debug.WriteLine("接收: " + _serverService.NReviceMsgSize + " B");
                        //Debug.WriteLine( "服务器通讯: " + _userClient.nBufferOfM2Size + " B");
                        Debug.WriteLine("通讯自检: " + GateShare.dwCheckServerTimeMin + "/" + GateShare.dwCheckServerTimeMax);
                        Debug.WriteLine("编码: " + nProcessMsgSize + " B");
                        Debug.WriteLine("登录: " + nHumLogonMsgSize + " B");
                        Debug.WriteLine("普通: " + nHumPlayMsgSize + " B");
                        Debug.WriteLine("解码: " + nDeCodeMsgSize + " B");
                        Debug.WriteLine("发送: " + nSendBlockSize + " B");
                        if (GateShare.dwCheckServerTimeMax > 1)
                        {
                            GateShare.dwCheckServerTimeMax -= 1;
                        }
                    }
                    //_userClient.nBufferOfM2Size = 0;
                    _serverService.NReviceMsgSize = 0;
                    nDeCodeMsgSize = 0;
                    nSendBlockSize = 0;
                    nProcessMsgSize = 0;
                    nHumLogonMsgSize = 0;
                    nHumPlayMsgSize = 0;
                }
                try
                {
                    var dwProcessReviceMsgLimiTick = HUtil32.GetTickCount();
                    if ((HUtil32.GetTickCount() - dwProcessPacketTick) > 300)
                    {
                        dwProcessPacketTick = HUtil32.GetTickCount();
                        if (GateShare.ReviceMsgList.Reader.Count > 0)
                        {
                            if (GateShare.dwProcessReviceMsgTimeLimit < 300)
                            {
                                GateShare.dwProcessReviceMsgTimeLimit++;
                            }
                        }
                        else
                        {
                            if (GateShare.dwProcessReviceMsgTimeLimit > 30)
                            {
                                GateShare.dwProcessReviceMsgTimeLimit -= 1;
                            }
                        }
                        if (GateShare.SendMsgList.Reader.Count > 0)
                        {
                            if (GateShare.dwProcessSendMsgTimeLimit < 300)
                            {
                                GateShare.dwProcessSendMsgTimeLimit++;
                            }
                        }
                        else
                        {
                            if (GateShare.dwProcessSendMsgTimeLimit > 30)
                            {
                                GateShare.dwProcessSendMsgTimeLimit -= 1;
                            }
                        }
                        //处理ClientSession中的延时消息
                        var sessionList = _sessionManager.GetAllSession();
                        for (var i = 0; i < sessionList.Count(); i++)
                        {
                            if (string.IsNullOrEmpty(sessionList[i].sSendData))
                            {
                                return;
                            }
                            sessionList[i].SeneMessage();
                            if ((HUtil32.GetTickCount() - dwProcessReviceMsgLimiTick) > 20)
                            {
                                break;
                            }
                        }
                    }
                }
                catch (Exception E)
                {
                    GateShare.AddMainLogMsg($"[Exception] ProcessPacket :{E.Message}", 1);
                }
                GateShare.boDecodeMsgLock = false;
                var dwLoopProcessTime = HUtil32.GetTickCount() - dwLoopCheckTick;
                dwLoopCheckTick = HUtil32.GetTickCount();
                if (dwLoopTime < dwLoopProcessTime)
                {
                    dwLoopTime = dwLoopProcessTime;
                }
                if ((HUtil32.GetTickCount() - dwRefConsolMsgTick) > 10000)
                {
                    dwRefConsolMsgTick = HUtil32.GetTickCount();
                    // LabelLoopTime.Text = (dwLoopTime).ToString();
                    //Console.WriteLine("接收处理限制: " + GateShare.dwProcessReviceMsgTimeLimit);
                    //Console.WriteLine("发送处理限制: " + GateShare.dwProcessSendMsgTimeLimit);
                    //Console.WriteLine("接收: " + _serverService.dwProcessClientMsgTime);
                    //Console.WriteLine("发送: " + dwProcessServerMsgTime);
                }
            }
        }

        private void ProcessPacket(TSendUserData UserData)
        {
            ClientSession userSession = null;
            if (string.IsNullOrEmpty(UserData.UserCientId))
            {
                userSession = _sessionManager.GetSession(UserData.SocketIndex);
            }
            else
            {
                userSession = _sessionManager.GetSession(UserData.UserCientId);
            }
            if (userSession == null)
            {
                return;
            }
            var sMsg = HUtil32.GetString(UserData.Buffer, 0, UserData.BufferLen);
            userSession.SeneMessage(sMsg);
        }

        public void StartService()
        {
            try
            {
                GateShare.Initialization();
                GateShare.AddMainLogMsg("正在启动服务...", 2);
                GateShare.boServiceStart = true;
                GateShare.boGateReady = false;
                //GateShare.boCheckServerFail = false;
                GateShare.boSendHoldTimeOut = false;
                LoadConfig();
                GateShare.dwProcessReviceMsgTimeLimit = 50;
                GateShare.dwProcessSendMsgTimeLimit = 50;
                GateShare.boServerReady = false;
                dwReConnectServerTime = HUtil32.GetTickCount() - 25000;
                dwRefConsolMsgTick = HUtil32.GetTickCount();

                _serverService.Start();
                _runGateClient.LoadConfig();
                _runGateClient.Start();
                GateShare.boServerReady = true;
                decodeTimer = new Timer(DecodeTimer, null, 0, 1);
                sendTime = new Timer(SendTimerTimer, null, 3000, 3000);

                GateShare.AddMainLogMsg("服务已启动成功...", 2);
                GateShare.AddMainLogMsg("欢迎使用翎风系列游戏软件...", 0);
                GateShare.AddMainLogMsg("网站:http://www.gameofmir.com", 0);
                GateShare.AddMainLogMsg("论坛:http://bbs.gameofmir.com", 0);
                GateShare.AddMainLogMsg("智能反外挂程序已启动...", 0);
                GateShare.AddMainLogMsg("智能反外挂程序云端已连接...", 0);
                //GateShare.AddMainLogMsg("网关集群模式已启动,当前运行[随机分配]...", 0);
            }
            catch (Exception E)
            {
                GateShare.AddMainLogMsg(E.Message, 0);
            }
        }

        public void StopService()
        {
            GateShare.AddMainLogMsg("正在停止服务...", 2);
            GateShare.boServiceStart = false;
            GateShare.boGateReady = false;
            // for (var nSockIdx = 0; nSockIdx < GateShare.GATEMAXSESSION; nSockIdx ++ )
            // {
            //     if (GateShare.SessionArray[nSockIdx].Socket != null)
            //     {
            //         GateShare.SessionArray[nSockIdx].Socket.Close();
            //     }
            // }
            _serverService.Stop();
            _runGateClient.Stop();
            GateShare.AddMainLogMsg("服务停止成功...", 2);
        }

        private void LoadConfig()
        {
            GateShare.AddMainLogMsg("正在加载配置信息...", 3);
            if (GateShare.Conf != null)
            {
                GateShare.GateAddr = GateShare.Conf.ReadString(GateShare.GateClass, "GateAddr", GateShare.GateAddr);
                GateShare.GatePort = GateShare.Conf.ReadInteger(GateShare.GateClass, "GatePort", GateShare.GatePort);
                GateShare.nShowLogLevel = GateShare.Conf.ReadInteger(GateShare.GateClass, "ShowLogLevel", GateShare.nShowLogLevel);
                GateShare.boShowBite = GateShare.Conf.ReadBool(GateShare.GateClass, "ShowBite", GateShare.boShowBite);
                GateShare.nMaxConnOfIPaddr = GateShare.Conf.ReadInteger(GateShare.GateClass, "MaxConnOfIPaddr", GateShare.nMaxConnOfIPaddr);
                GateShare.BlockMethod = (TBlockIPMethod)GateShare.Conf.ReadInteger(GateShare.GateClass, "BlockMethod", (int)GateShare.BlockMethod);
                GateShare.nMaxClientPacketSize = GateShare.Conf.ReadInteger(GateShare.GateClass, "MaxClientPacketSize", GateShare.nMaxClientPacketSize);
                GateShare.nNomClientPacketSize = GateShare.Conf.ReadInteger(GateShare.GateClass, "NomClientPacketSize", GateShare.nNomClientPacketSize);
                GateShare.nMaxClientMsgCount = GateShare.Conf.ReadInteger(GateShare.GateClass, "MaxClientMsgCount", GateShare.nMaxClientMsgCount);
                GateShare.bokickOverPacketSize = GateShare.Conf.ReadBool(GateShare.GateClass, "kickOverPacket", GateShare.bokickOverPacketSize);
                GateShare.dwCheckServerTimeOutTime = GateShare.Conf.ReadInteger<int>(GateShare.GateClass, "ServerCheckTimeOut", GateShare.dwCheckServerTimeOutTime);
                GateShare.nClientSendBlockSize = GateShare.Conf.ReadInteger(GateShare.GateClass, "ClientSendBlockSize", GateShare.nClientSendBlockSize);
                GateShare.dwClientTimeOutTime = GateShare.Conf.ReadInteger<int>(GateShare.GateClass, "ClientTimeOutTime", GateShare.dwClientTimeOutTime);
                GateShare.dwSessionTimeOutTime = GateShare.Conf.ReadInteger<int>(GateShare.GateClass, "SessionTimeOutTime", GateShare.dwSessionTimeOutTime);
                GateShare.nSayMsgMaxLen = GateShare.Conf.ReadInteger(GateShare.GateClass, "SayMsgMaxLen", GateShare.nSayMsgMaxLen);
                GateShare.dwSayMsgTime = GateShare.Conf.ReadInteger<int>(GateShare.GateClass, "SayMsgTime", GateShare.dwSayMsgTime);
            }
            GateShare.AddMainLogMsg("配置信息加载完成...", 3);
            GateShare.LoadAbuseFile();
            GateShare.LoadBlockIPFile();
        }

        private void ShowMainLogMsg()
        {
            if ((HUtil32.GetTickCount() - dwShowMainLogTick) < 200)
            {
                return;
            }
            dwShowMainLogTick = HUtil32.GetTickCount();
            try
            {
                boShowLocked = true;
                try
                {
                    HUtil32.EnterCriticalSection(GateShare.CS_MainLog);
                    for (var i = 0; i < GateShare.MainLogMsgList.Count; i++)
                    {
                        TempLogList.Add(GateShare.MainLogMsgList[i]);
                    }
                    GateShare.MainLogMsgList.Clear();
                }
                finally
                {
                    HUtil32.LeaveCriticalSection(GateShare.CS_MainLog);
                }
                for (var i = 0; i < TempLogList.Count; i++)
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

        private void SendTimerTimer(object obj)
        {
            TSessionInfo UserSession;
            if ((HUtil32.GetTickCount() - GateShare.dwSendHoldTick) > 3000)
            {
                GateShare.boSendHoldTimeOut = false;
            }
            if (GateShare.boGateReady) //清理超时用户会话   && !GateShare.boCheckServerFail
            {
                var clientList = _runGateClient.GetAllClient();
                for (var i = 0; i < clientList.Count; i++)
                {
                    if (clientList[i] == null)
                    {
                        continue;
                    }
                    for (var j = 0; j < clientList[i].MaxSession; j++)
                    {
                        UserSession = clientList[i].SessionArray[j];
                        if (UserSession.Socket != null)
                        {
                            if ((HUtil32.GetTickCount() - UserSession.dwReceiveTick) > GateShare.dwSessionTimeOutTime)
                            {
                                UserSession.Socket.Close();
                                UserSession.Socket = null;
                                UserSession.nSckHandle = -1;
                            }
                        }
                    }
                }
            }
            if (!GateShare.boGateReady)
            {
                if (HUtil32.GetTickCount() - dwReConnectServerTime > 1000 && GateShare.boServiceStart)
                {
                    dwReConnectServerTime = HUtil32.GetTickCount();
                    //_runGateClient.Start();
                    //判断是否连接然后在开始
                }
            }
            else
            {
                GateShare.dwCheckServerTimeMin = HUtil32.GetTickCount() - GateShare.dwCheckServerTick;
                if (GateShare.dwCheckServerTimeMax < GateShare.dwCheckServerTimeMin)
                {
                    GateShare.dwCheckServerTimeMax = GateShare.dwCheckServerTimeMin;
                }
            }
        }

        private bool IsBlockIP(string sIPaddr)
        {
            bool result = false;
            string sBlockIPaddr;
            for (var i = 0; i < GateShare.TempBlockIPList.Count; i++)
            {
                sBlockIPaddr = GateShare.TempBlockIPList[i];
                if (string.Compare(sIPaddr, sBlockIPaddr, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    result = true;
                    break;
                }
            }
            for (var i = 0; i < GateShare.BlockIPList.Count; i++)
            {
                sBlockIPaddr = GateShare.BlockIPList[i];
                if (HUtil32.CompareLStr(sIPaddr, sBlockIPaddr, sBlockIPaddr.Length))
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        private bool IsConnLimited(string sIPaddr)
        {
            bool result = false;
            int nCount = 0;
            // for (var i = 0; i < ServerSocket.Socket.ActiveConnections; i ++ )
            // {
            //     if ((sIPaddr).CompareTo((ServerSocket.Connections[i].RemoteAddress)) == 0)
            //     {
            //         nCount ++;
            //     }
            // }
            if (nCount > GateShare.nMaxConnOfIPaddr)
            {
                result = true;
            }
            return result;
        }
    }
}