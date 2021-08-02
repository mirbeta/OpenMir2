using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using SystemModule;
using SystemModule.Packages;
using SystemModule.Sockets;

namespace RunGate
{
    public class ServerApp
    {
        private long dwShowMainLogTick = 0;
        private bool boShowLocked = false;
        private ArrayList TempLogList = null;
        private long dwCheckClientTick = 0;
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
        private readonly UserClientService _userClient;
        private readonly ServerService _serverService;
        
        public ServerApp(UserClientService userClient, ServerService serverService)
        {
            _userClient = userClient;
            _serverService = serverService;
            TempLogList = new ArrayList();
            dwLoopCheckTick = HUtil32.GetTickCount();
        }

        public async Task StartProcessMessageService()
        {
            var gTasks = new Task[2];
            var consumerTask1 =Task.Factory.StartNew(ProcessReviceMessage);
            gTasks[0] = consumerTask1;

            var consumerTask2 = Task.Factory.StartNew(ProcessSendMessage);
            gTasks[1] = consumerTask2;
            
            await Task.WhenAll(gTasks);
        }

        private async Task ProcessReviceMessage()
        {
            try
            {
                //long dwProcessReviceMsgLimiTick = 0;
                while (await GateShare.ReviceMsgList.Reader.WaitToReadAsync())
                {
                    if (GateShare.ReviceMsgList.Reader.Count <= 0)
                    {
                        break;
                    }
                    if (GateShare.ReviceMsgList.Reader.TryRead(out var message))
                    {
                        //dwProcessReviceMsgLimiTick = HUtil32.GetTickCount();
                        ProcessUserPacket(message);
                        // if (HUtil32.GetTickCount() - dwProcessReviceMsgLimiTick > GateShare.dwProcessReviceMsgTimeLimit)
                        // {
                        //     continue;
                        // }
                    }
                }
            }
            catch (Exception E)
            {
                GateShare.AddMainLogMsg("[Exception] DecodeTimerTImer->ProcessUserPacket", 1);
            }
        }

        private async Task ProcessSendMessage()
        {
            try
            {
                //long dwProcessReviceMsgLimiTick = 0;
                while (await GateShare.SendMsgList.Reader.WaitToReadAsync())
                {
                    //dwProcessReviceMsgLimiTick = HUtil32.GetTickCount();
                    if (GateShare.SendMsgList.Reader.Count <= 0)
                    {
                        break;
                    }
                    if (GateShare.SendMsgList.Reader.TryRead(out var message))
                    {
                        ProcessPacket(message);
                        // if (HUtil32.GetTickCount() - dwProcessReviceMsgLimiTick > GateShare.dwProcessSendMsgTimeLimit)
                        // {
                        //     break;
                        // }
                    }
                }
            }
            catch (Exception E)
            {
                GateShare.AddMainLogMsg("[Exception] DecodeTimerTImer->ProcessPacket", 1);
            }
        }

        private void DecodeTimer(object obj)
        {
            long dwLoopProcessTime;
            long dwProcessReviceMsgLimiTick;
            TSendUserData tUserData = null;
            TSessionInfo UserSession= null;
            ShowMainLogMsg();
            if (!GateShare.boDecodeMsgLock)
            {
                try
                {
                    if (HUtil32.GetTickCount() - dwRefConsoleMsgTick >= 10000)
                    {
                        dwRefConsoleMsgTick = HUtil32.GetTickCount();
                        Console.WriteLine("待处理消息:" + GateShare.ReviceMsgList.Reader.Count);

                        if (!GateShare.boShowBite)
                        {
                           //Debug.WriteLine( "接收: " + _serverService.nReviceMsgSize / 1024 + " KB");
                           //Debug.WriteLine( "服务器通讯: " + _userClient.nBufferOfM2Size / 1024 + " KB");
                           //Debug.WriteLine( "编码: " + nProcessMsgSize / 1024 + " KB");
                           //Debug.WriteLine( "登录: " + nHumLogonMsgSize / 1024 + " KB");
                           //Debug.WriteLine("普通: " + nHumPlayMsgSize / 1024 + " KB");
                           //Debug.WriteLine( "解码: " + nDeCodeMsgSize / 1024 + " KB");
                           //Debug.WriteLine( "发送: " + nSendBlockSize / 1024 + " KB");
                        }
                        else
                        {
                            //Debug.WriteLine( "接收: " + _serverService.nReviceMsgSize + " B");
                            //Debug.WriteLine( "服务器通讯: " + _userClient.nBufferOfM2Size + " B");
                            //Debug.WriteLine("通讯自检: " + GateShare.dwCheckServerTimeMin + "/" + GateShare.dwCheckServerTimeMax);
                            //Debug.WriteLine( "编码: " + nProcessMsgSize + " B");
                            //Debug.WriteLine("登录: " + nHumLogonMsgSize + " B");
                            //Debug.WriteLine( "普通: " + nHumPlayMsgSize + " B");
                            //Debug.WriteLine( "解码: " + nDeCodeMsgSize + " B");
                            //Debug.WriteLine( "发送: " + nSendBlockSize + " B");
                            if (GateShare.dwCheckServerTimeMax > 1)
                            {
                                GateShare.dwCheckServerTimeMax -= 1;
                            }
                        }
                        _userClient.nBufferOfM2Size = 0;
                        _serverService.nReviceMsgSize = 0;
                        nDeCodeMsgSize = 0;
                        nSendBlockSize = 0;
                        nProcessMsgSize = 0;
                        nHumLogonMsgSize = 0;
                        nHumPlayMsgSize = 0;
                    }
                    try
                    {
                        dwProcessReviceMsgLimiTick = HUtil32.GetTickCount();
                        if (HUtil32.GetTickCount() - dwProcessPacketTick > 300)
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
                            for (var i = 0; i < GateShare.GATEMAXSESSION; i++)
                            {
                                UserSession = GateShare.SessionArray[i];
                                if (UserSession.Socket != null && UserSession.sSendData != "")
                                {
                                    tUserData = new TSendUserData();
                                    tUserData.nSocketIdx = i;
                                    tUserData.nSocketHandle = UserSession.nSckHandle;
                                    tUserData.sMsg = "";
                                    ProcessPacket(tUserData);
                                    if (HUtil32.GetTickCount() - dwProcessReviceMsgLimiTick > 20)
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception E)
                    {
                        GateShare.AddMainLogMsg("[Exception] DecodeTimerTImer->ProcessPacket 2", 1);
                    }
                    // 每二秒向游戏服务器发送一个检查信号
                    if (HUtil32.GetTickCount() - dwCheckClientTick > 20000)
                    {
                        dwCheckClientTick = HUtil32.GetTickCount();
                        if (GateShare.boGateReady)
                        {
                            _userClient.SendServerMsg(Grobal2.GM_CHECKCLIENT, 0, 0, 0, 0, "");
                        }
                        if (HUtil32.GetTickCount() - GateShare.dwCheckServerTick > GateShare.dwCheckServerTimeOutTime)
                        {
                            GateShare.boCheckServerFail = true;
                            //ClientSocket.Disconnect();
                        }
                        if (dwLoopTime > 30)
                        {
                            dwLoopTime -= 20;
                        }
                        if (dwProcessServerMsgTime > 1)
                        {
                            dwProcessServerMsgTime -= 1;
                        }
                        if (_serverService.dwProcessClientMsgTime > 1)
                        {
                            _serverService.dwProcessClientMsgTime -= 1;
                        }
                    }
                    GateShare.boDecodeMsgLock = false;
                }
                catch (Exception E)
                {
                    GateShare.AddMainLogMsg("[Exception] DecodeTimer", 1);
                    GateShare.boDecodeMsgLock = false;
                }
                dwLoopProcessTime = HUtil32.GetTickCount() - dwLoopCheckTick;
                dwLoopCheckTick = HUtil32.GetTickCount();
                if (dwLoopTime < dwLoopProcessTime)
                {
                    dwLoopTime = dwLoopProcessTime;
                }
                if (HUtil32.GetTickCount() - dwRefConsolMsgTick > 10000)
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

        private void ProcessUserPacket(TSendUserData UserData)
        {
            string sMsg = string.Empty;
            string sData = string.Empty;
            string sDefMsg = string.Empty;
            string sDataMsg = string.Empty;
            string sDataText = string.Empty;
            string sHumName = string.Empty;
            byte[] DataBuffer = null;
            int nOPacketIdx;
            int nPacketIdx;
            int nDataLen;
            int n14;
            TDefaultMessage DefMsg;
            try
            {
                n14 = 0;
                nProcessMsgSize += UserData.sMsg.Length;
                //Console.WriteLine("处理游戏引擎封包:" + nProcessMsgSize);
                if (UserData.nSocketIdx >= 0 && UserData.nSocketIdx < GateShare.GATEMAXSESSION)
                {
                    if (UserData.nSocketHandle == GateShare.SessionArray[UserData.nSocketIdx].nSckHandle && GateShare.SessionArray[UserData.nSocketIdx].nPacketErrCount < 10)
                    {
                        if (GateShare.SessionArray[UserData.nSocketIdx].sSocData.Length > GateShare.MSGMAXLENGTH)
                        {
                            GateShare.SessionArray[UserData.nSocketIdx].sSocData = "";
                            GateShare.SessionArray[UserData.nSocketIdx].nPacketErrCount = 99;
                            UserData.sMsg = "";
                        }
                        sMsg = GateShare.SessionArray[UserData.nSocketIdx].sSocData + UserData.sMsg;
                        while (true)
                        {
                            sData = "";
                            sMsg = HUtil32.ArrestStringEx(sMsg, "#", "!", ref sData);
                            if (sData.Length > 2)
                            {
                                nPacketIdx = HUtil32.Str_ToInt(sData[0].ToString(), 99); // 将数据名第一位的序号取出
                                if (GateShare.SessionArray[UserData.nSocketIdx].nPacketIdx == nPacketIdx)
                                {
                                    // 如果序号重复则增加错误计数
                                    GateShare.SessionArray[UserData.nSocketIdx].nPacketErrCount++;
                                }
                                else
                                {
                                    nOPacketIdx = GateShare.SessionArray[UserData.nSocketIdx].nPacketIdx;
                                    GateShare.SessionArray[UserData.nSocketIdx].nPacketIdx = nPacketIdx;
                                    sData = sData.Substring(1, sData.Length - 1);
                                    nDataLen = sData.Length;
                                    if (nDataLen >= Grobal2.DEFBLOCKSIZE)
                                    {
                                        if (GateShare.SessionArray[UserData.nSocketIdx].boStartLogon)// 第一个人物登录数据包
                                        {
                                            nHumLogonMsgSize += sData.Length;
                                            GateShare.SessionArray[UserData.nSocketIdx].boStartLogon = false;
                                            sData = "#" + nPacketIdx + sData + "!";
                                            _userClient.SendServerMsg(Grobal2.GM_DATA, UserData.nSocketIdx, (int)GateShare.SessionArray[UserData.nSocketIdx].Socket.Handle,
                                                GateShare.SessionArray[UserData.nSocketIdx].nUserListIndex, sData.Length, sData);
                                        }
                                        else
                                        {
                                            // 普通数据包
                                            nHumPlayMsgSize += sData.Length;
                                            if (nDataLen == Grobal2.DEFBLOCKSIZE)
                                            {
                                                sDefMsg = sData;
                                                sDataMsg = "";
                                            }
                                            else
                                            {
                                                sDefMsg = sData.Substring(0, Grobal2.DEFBLOCKSIZE);
                                                sDataMsg = sData.Substring(Grobal2.DEFBLOCKSIZE, sData.Length - Grobal2.DEFBLOCKSIZE);
                                            }
                                            DefMsg = EDcode.DecodeMessage(sDefMsg); // 检查数据
                                            if (!string.IsNullOrEmpty(sDataMsg))
                                            {
                                                if (DefMsg.Ident == Grobal2.CM_SAY) // 控制发言间隔时间
                                                {
                                                    sDataText = EDcode.DeCodeString(sDataMsg);
                                                    if (sDataText != "")
                                                    {
                                                        if (sDataText[1] == '/')
                                                        {
                                                            sDataText = HUtil32.GetValidStr3(sDataText, ref sHumName, new string[] { " " }); // 限制最长可发字符长度
                                                            FilterSayMsg(ref sDataText);
                                                            sDataText = sHumName + " " + sDataText;
                                                        }
                                                        else
                                                        {
                                                            if (sDataText[1] != '@')
                                                            {
                                                                FilterSayMsg(ref sDataText);// 限制最长可发字符长度
                                                            }
                                                        }
                                                    }
                                                    sDataMsg = EDcode.EncodeString(sDataText);
                                                }
                                                DataBuffer = new byte[sDataMsg.Length + 12 + 1]; //GetMem(Buffer, sDataMsg.Length + 12 + 1);
                                                Buffer.BlockCopy(DefMsg.ToByte(), 0, DataBuffer, 0, 12);//Move(DefMsg, Buffer, 12);
                                                var msgBuff = HUtil32.GetBytes(sDataMsg);
                                                Buffer.BlockCopy(msgBuff, 0, DataBuffer, 12, msgBuff.Length); //Move(sDataMsg[1], Buffer[12], sDataMsg.Length + 1);
                                                _userClient.SendServerMsg(Grobal2.GM_DATA, UserData.nSocketIdx, (int)GateShare.SessionArray[UserData.nSocketIdx].Socket.Handle, 
                                                    GateShare.SessionArray[UserData.nSocketIdx].nUserListIndex, DataBuffer.Length, DataBuffer);
                                            }
                                            else
                                            {
                                                DataBuffer = DefMsg.ToByte();
                                                _userClient.SendServerMsg(Grobal2.GM_DATA, UserData.nSocketIdx, (int)GateShare.SessionArray[UserData.nSocketIdx].Socket.Handle,
                                                    GateShare.SessionArray[UserData.nSocketIdx].nUserListIndex, 12, DataBuffer);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (n14 >= 1)
                                {
                                    sMsg = "";
                                }
                                else
                                {
                                    n14++;
                                }
                            }
                            if (HUtil32.TagCount(sMsg, '!') < 1)
                            {
                                break;
                            }
                        }
                        GateShare.SessionArray[UserData.nSocketIdx].sSocData = sMsg;
                    }
                    else
                    {
                        GateShare.SessionArray[UserData.nSocketIdx].sSocData = "";
                    }
                }
            }
            catch
            {
                if (UserData.nSocketIdx >= 0 && UserData.nSocketIdx < GateShare.GATEMAXSESSION)
                {
                    sData = "[" + GateShare.SessionArray[UserData.nSocketIdx].sRemoteAddr + "]";
                }
                GateShare.AddMainLogMsg("[Exception] ProcessUserPacket" + sData, 1);
            }
        }

        private void ProcessPacket(TSendUserData UserData)
        {
            string sData;
            string sSendBlock;
            TSessionInfo UserSession;
            if (UserData.nSocketIdx >= 0 && UserData.nSocketIdx < GateShare.GATEMAXSESSION)
            {
                UserSession = GateShare.SessionArray[UserData.nSocketIdx];
                if (UserSession.nSckHandle == UserData.nSocketHandle)
                {
                    nDeCodeMsgSize += UserData.sMsg.Length;
                    sData = UserSession.sSendData + UserData.sMsg;
                    while (!string.IsNullOrEmpty(sData))
                    {
                        if (sData.Length > GateShare.nClientSendBlockSize)
                        {
                            sSendBlock = sData.Substring(0 ,GateShare.nClientSendBlockSize);
                            sData = sData.Substring(GateShare.nClientSendBlockSize ,sData.Length - GateShare.nClientSendBlockSize);
                        }
                        else
                        {
                            sSendBlock = sData;
                            sData = "";
                        }
                        if (!UserSession.boSendAvailable)
                        {
                            if (HUtil32.GetTickCount() > UserSession.dwTimeOutTime)
                            {
                                UserSession.boSendAvailable = true;
                                UserSession.nCheckSendLength = 0;
                                GateShare.boSendHoldTimeOut = true;
                                GateShare.dwSendHoldTick = HUtil32.GetTickCount();
                            }
                        }
                        if (UserSession.boSendAvailable)
                        {
                            if (UserSession.nCheckSendLength >= GateShare.SENDCHECKSIZE)
                            {
                                if (!UserSession.boSendCheck)
                                {
                                    UserSession.boSendCheck = true;
                                    sSendBlock = "*" + sSendBlock;
                                }
                                if (UserSession.nCheckSendLength >= GateShare.SENDCHECKSIZEMAX)
                                {
                                    UserSession.boSendAvailable = false;
                                    UserSession.dwTimeOutTime = HUtil32.GetTickCount() + GateShare.dwClientCheckTimeOut;
                                }
                            }
                            if (UserSession.Socket != null && UserSession.Socket.Connected)
                            {
                                nSendBlockSize += sSendBlock.Length;
                                UserSession.Socket.SendText(sSendBlock);
                            }
                            UserSession.nCheckSendLength += sSendBlock.Length;
                        }
                        else
                        {
                            sData = sSendBlock + sData;
                            break;
                        }
                    }
                    UserSession.sSendData = sData;
                }
            }
        }

        private void FilterSayMsg(ref string sMsg)
        {
            int nLen;
            string sReplaceText;
            string sFilterText;
            try
            {
                HUtil32.EnterCriticalSection(GateShare.CS_FilterMsg);
                for (var i = 0; i < GateShare.AbuseList.Count; i++)
                {
                    sFilterText = GateShare.AbuseList[i];
                    sReplaceText = "";
                    if (sMsg.IndexOf(sFilterText) != -1)
                    {
                        for (nLen = 0; nLen <= sFilterText.Length; nLen++)
                        {
                            sReplaceText = sReplaceText + GateShare.sReplaceWord;
                        }
                        sMsg = sMsg.Replace(sFilterText, sReplaceText);
                    }
                }
            }
            finally
            {
                HUtil32.LeaveCriticalSection(GateShare.CS_FilterMsg);
            }
        }
        
        public void StartService()
        {
            try
            {
                GateShare.Initialization();
                GateShare.AddMainLogMsg("正在启动服务...", 2);
                GateShare.boServiceStart = true;
                GateShare.boGateReady = false;
                GateShare.boCheckServerFail = false;
                GateShare.boSendHoldTimeOut = false;
                GateShare.SessionCount = 0;
                LoadConfig();
                _userClient.RestSessionArray();
                GateShare.dwProcessReviceMsgTimeLimit = 50;
                GateShare.dwProcessSendMsgTimeLimit = 50;
                GateShare.boServerReady = false;
                dwReConnectServerTime = HUtil32.GetTickCount() - 25000;
                dwRefConsolMsgTick = HUtil32.GetTickCount();
                
                _serverService.Start();
                _userClient.Start();
                
                sendTime = new Timer(SendTimerTimer, null, 3000, 3000);
                decodeTimer = new Timer(DecodeTimer, null, 3000, 200);

                
                GateShare.AddMainLogMsg("服务已启动成功...", 2);
                GateShare.AddMainLogMsg("欢迎使用翎风系列游戏软件...",0);
                GateShare.AddMainLogMsg("网站:http://www.gameofmir.com",0);
                GateShare.AddMainLogMsg("论坛:http://bbs.gameofmir.com",0);
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
            for (var nSockIdx = 0; nSockIdx < GateShare.GATEMAXSESSION; nSockIdx ++ )
            {
                if (GateShare.SessionArray[nSockIdx].Socket != null)
                {
                    GateShare.SessionArray[nSockIdx].Socket.Close();
                }
            }
            _serverService.Stop();
            _userClient.Stop();
            GateShare.AddMainLogMsg("服务停止成功...", 2);
        }

        private void LoadConfig()
        {
            GateShare.AddMainLogMsg("正在加载配置信息...", 3);
            if (GateShare.Conf != null)
            {
                 GateShare.TitleName = GateShare.Conf.ReadString(GateShare.GateClass, "Title", GateShare.TitleName);
                 GateShare.ServerAddr = GateShare.Conf.ReadString(GateShare.GateClass, "ServerAddr", GateShare.ServerAddr);
                 GateShare.ServerPort = GateShare.Conf.ReadInteger(GateShare.GateClass, "ServerPort", GateShare.ServerPort);
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
                 GateShare.dwCheckServerTimeOutTime = GateShare.Conf.ReadInteger<long>(GateShare.GateClass, "ServerCheckTimeOut", GateShare.dwCheckServerTimeOutTime);
                 GateShare.nClientSendBlockSize = GateShare.Conf.ReadInteger(GateShare.GateClass, "ClientSendBlockSize", GateShare.nClientSendBlockSize);
                 GateShare.dwClientTimeOutTime = GateShare.Conf.ReadInteger<long>(GateShare.GateClass, "ClientTimeOutTime", GateShare.dwClientTimeOutTime);
                 GateShare.dwSessionTimeOutTime = GateShare.Conf.ReadInteger<long>(GateShare.GateClass, "SessionTimeOutTime", GateShare.dwSessionTimeOutTime);
                 GateShare.nSayMsgMaxLen = GateShare.Conf.ReadInteger(GateShare.GateClass, "SayMsgMaxLen", GateShare.nSayMsgMaxLen);
                 GateShare.dwSayMsgTime = GateShare.Conf.ReadInteger<long>(GateShare.GateClass, "SayMsgTime", GateShare.dwSayMsgTime);
            }
            GateShare.AddMainLogMsg("配置信息加载完成...", 3);
            GateShare.LoadAbuseFile();
            GateShare.LoadBlockIPFile();
        }

        private void ShowMainLogMsg()
        {
            if (HUtil32.GetTickCount() - dwShowMainLogTick < 200)
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
        
        public void ShowLogMsg(bool boFlag)
        {
            // int nHeight;
            // switch(boFlag)
            // {
            //     case true:
            //         nHeight = Panel.Height;
            //         Panel.Height = 0;
            //         MemoLog.Height = nHeight;
            //         MemoLog.Top = Panel.Top;
            //         break;
            //     case false:
            //         nHeight = MemoLog.Height;
            //         MemoLog.Height = 0;
            //         Panel.Height = nHeight;
            //         break;
            // }
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
                //MENU_VIEW_LOGMSGClick(Sender);
                GateShare.boStarted = true;
                //StartTimer.Enabled = false;
                StartService();
            }
        }

        public void TimerTimer(System.Object Sender, System.EventArgs _e1)
        {
            // if (ServerSocket.Active)
            // {
            //     StatusBar.Panels[0].Text = (ServerSocket.Port).ToString();
            //     POPMENU_PORT.Text = (ServerSocket.Port).ToString();
            //     if (GateShare.boSendHoldTimeOut)
            //     {
            //         StatusBar.Panels[2].Text = (GateShare.SessionCount).ToString() + "/#" + (ServerSocket.Socket.ActiveConnections).ToString();
            //         POPMENU_CONNCOUNT.Text = (GateShare.SessionCount).ToString() + "/#" + (ServerSocket.Socket.ActiveConnections).ToString();
            //     }
            //     else
            //     {
            //         StatusBar.Panels[2].Text = (GateShare.SessionCount).ToString() + "/" + (ServerSocket.Socket.ActiveConnections).ToString();
            //         POPMENU_CONNCOUNT.Text = (GateShare.SessionCount).ToString() + "/" + (ServerSocket.Socket.ActiveConnections).ToString();
            //     }
            // }
            // else
            // {
            //     StatusBar.Panels[0].Text = "????";
            //     StatusBar.Panels[2].Text = "????";
            //     POPMENU_CONNCOUNT.Text = "????";
            // }
        }

        public void SendTimerTimer(object obj)
        {
            TSessionInfo UserSession;
            if (HUtil32.GetTickCount() - GateShare.dwSendHoldTick > 3000)
            {
                GateShare.boSendHoldTimeOut = false;
            }
            if (GateShare.boGateReady && !GateShare.boCheckServerFail)
            {
                for (var i = 0; i < GateShare.GATEMAXSESSION; i ++ )
                {
                    UserSession = GateShare.SessionArray[i];
                    if (UserSession.Socket != null)
                    {
                        if (HUtil32.GetTickCount() - UserSession.dwReceiveTick > GateShare.dwSessionTimeOutTime)
                        {
                            UserSession.Socket.Close();
                            UserSession.Socket = null;
                            UserSession.nSckHandle =  -1;
                        }
                    }
                }
            }
            if (!GateShare.boGateReady)
            {
                //StatusBar.Panels[1].Text = "未连接";
                //StatusBar.Panels[3].Text = "????";
                //POPMENU_CHECKTICK.Text = "????";
                if (HUtil32.GetTickCount() - dwReConnectServerTime > 1000 && GateShare.boServiceStart)
                {
                    dwReConnectServerTime = HUtil32.GetTickCount();
                    _userClient.Start();
                }
            }
            else
            {
                if (GateShare.boCheckServerFail)
                {
                    //StatusBar.Panels[1].Text = "超时";
                    Debug.WriteLine("链接游戏数据处理引擎超时");
                }
                else
                {
                    Debug.WriteLine("游戏数据处理引擎链接正常");
                    //StatusBar.Panels[1].Text = "已连接";
                    //LbLack.Text = (GateShare.n45AA84).ToString() + "/" + (GateShare.n45AA80).ToString();
                }
                GateShare.dwCheckServerTimeMin =  HUtil32.GetTickCount() - GateShare.dwCheckServerTick;
                if (GateShare.dwCheckServerTimeMax < GateShare.dwCheckServerTimeMin)
                {
                    GateShare.dwCheckServerTimeMax = GateShare.dwCheckServerTimeMin;
                }
               // StatusBar.Panels[3].Text = (GateShare.dwCheckServerTimeMin).ToString() + "/" + (GateShare.dwCheckServerTimeMax).ToString();
                //POPMENU_CHECKTICK.Text = (GateShare.dwCheckServerTimeMin).ToString() + "/" + (GateShare.dwCheckServerTimeMax).ToString();
            }
        }

        private bool IsBlockIP(string sIPaddr)
        {
            bool result= false;
            string sBlockIPaddr;
            for (var i = 0; i < GateShare.TempBlockIPList.Count; i ++ )
            {
                sBlockIPaddr = GateShare.TempBlockIPList[i];
                if (sIPaddr.ToLower().CompareTo(sBlockIPaddr.ToLower()) == 0)
                {
                    result = true;
                    break;
                }
            }
            for (var i = 0; i < GateShare.BlockIPList.Count; i ++ )
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
            bool result= false;
            int nCount= 0;
            // for (var i = 0; i < ServerSocket.Socket.ActiveConnections; i ++ )
            // {
            //     if ((sIPaddr).ToLower().CompareTo((ServerSocket.Connections[i].RemoteAddress).ToLower()) == 0)
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

        private bool CheckDefMsg(TDefaultMessage DefMsg, TSessionInfo SessionInfo)
        {
            var result = true;
            switch(DefMsg.Ident)
            {
                case Grobal2.CM_WALK:
                case Grobal2.CM_RUN:
                    break;
                case Grobal2.CM_TURN:
                    break;
                case Grobal2.CM_HIT:
                case Grobal2.CM_HEAVYHIT:
                case Grobal2.CM_BIGHIT:
                case Grobal2.CM_POWERHIT:
                case Grobal2.CM_LONGHIT:
                case Grobal2.CM_WIDEHIT:
                case Grobal2.CM_FIREHIT:
                    break;
                case Grobal2.CM_SPELL:
                    break;
                case Grobal2.CM_DROPITEM:
                    break;
                case Grobal2.CM_PICKUP:
                    break;
            }
            return result;
        }

        private void CloseAllUser()
        {
            for (var nSockIdx = 0; nSockIdx < GateShare.GATEMAXSESSION; nSockIdx ++ )
            {
                if (GateShare.SessionArray[nSockIdx].Socket != null)
                {
                    GateShare.SessionArray[nSockIdx].Socket.Close();
                }
            }
        }
    } 
}

