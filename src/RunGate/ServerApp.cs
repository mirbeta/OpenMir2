using System;
using System.Collections;
using System.Net.Mime;
using System.Net.Sockets;
using SystemModule;
using SystemModule.Sockets;
using SystemModule.Sockets.AsyncSocketClient;
using SystemModule.Sockets.AsyncSocketServer;

namespace RunGate
{
    public class ServerApp
    {
        private long dwShowMainLogTick = 0;
        private bool boShowLocked = false;
        private ArrayList TempLogList = null;
        private long dwCheckClientTick = 0;
        private long dwProcessPacketTick = 0;
        private bool boServerReady = false;
        private long dwLoopCheckTick = 0;
        private long dwLoopTime = 0;
        private long dwProcessServerMsgTime = 0;
        private long dwProcessClientMsgTime = 0;
        private long dwReConnectServerTime = 0;
        private long dwRefConsolMsgTick = 0;
        private int nBufferOfM2Size = 0;
        private long dwRefConsoleMsgTick = 0;
        private int nReviceMsgSize = 0;
        private int nDeCodeMsgSize = 0;
        private int nSendBlockSize = 0;
        private int nProcessMsgSize = 0;
        private int nHumLogonMsgSize = 0;
        private int nHumPlayMsgSize = 0;
        private IClientScoket ClientSocket;
        private ISocketServer ServerSocket;
        
        private void SendSocket(string SendBuffer, int nLen)
        {
            if (ClientSocket.IsConnected)
            {
                ClientSocket.SendText(SendBuffer);
            }
        }

        private void SendServerMsg(short nIdent, short wSocketIndex, int nSocket, short nUserListIndex, int nLen, string Data)
        {
            TMsgHeader GateMsg;
            string SendBuffer;
            int nBuffLen;
            GateMsg.dwCode = Grobal2.RUNGATECODE;
            GateMsg.nSocket = nSocket;
            GateMsg.wGSocketIdx = wSocketIndex;
            GateMsg.wIdent = nIdent;
            GateMsg.wUserListIndex = nUserListIndex;
            GateMsg.nLength = nLen;
            nBuffLen = nLen + sizeof(TMsgHeader);
            GetMem(SendBuffer, nBuffLen);
            Move(GateMsg, SendBuffer, sizeof(TMsgHeader));
            if (Data != null)
            {
                Move(Data, SendBuffer[sizeof(TMsgHeader)], nLen);
            }
            SendSocket(SendBuffer, nBuffLen);
        }

        public void DecodeTimerTimer(System.Object Sender, System.EventArgs _e1)
        {
            long dwLoopProcessTime;
            long dwProcessReviceMsgLimiTick;
            TSendUserData UserData= null;
            TSendUserData tUserData = null;
            TSessionInfo UserSession= null;
            ShowMainLogMsg();
            if (!GateShare.boDecodeMsgLock)
            {
                try
                {
                    if ((HUtil32.GetTickCount() - dwRefConsoleMsgTick) >= 1000)
                    {
                        dwRefConsoleMsgTick = HUtil32.GetTickCount();
                        if (!GateShare.boShowBite)
                        {
                            // LabelReviceMsgSize.Text = "接收: " + (nReviceMsgSize / 1024).ToString() + " KB";
                            // LabelBufferOfM2Size.Text = "服务器通讯: " + (nBufferOfM2Size / 1024).ToString() + " KB";
                            // LabelProcessMsgSize.Text = "编码: " + (nProcessMsgSize / 1024).ToString() + " KB";
                            // LabelLogonMsgSize.Text = "登录: " + (nHumLogonMsgSize / 1024).ToString() + " KB";
                            // LabelPlayMsgSize.Text = "普通: " + (nHumPlayMsgSize / 1024).ToString() + " KB";
                            // LabelDeCodeMsgSize.Text = "解码: " + (nDeCodeMsgSize / 1024).ToString() + " KB";
                            // LabelSendBlockSize.Text = "发送: " + (nSendBlockSize / 1024).ToString() + " KB";
                        }
                        else
                        {
                            // LabelReviceMsgSize.Text = "接收: " + (nReviceMsgSize).ToString() + " B";
                            // LabelBufferOfM2Size.Text = "服务器通讯: " + (nBufferOfM2Size).ToString() + " B";
                            // LabelSelfCheck.Text = "通讯自检: " + (GateShare.dwCheckServerTimeMin).ToString() + "/" + (GateShare.dwCheckServerTimeMax).ToString();
                            // LabelProcessMsgSize.Text = "编码: " + (nProcessMsgSize).ToString() + " B";
                            // LabelLogonMsgSize.Text = "登录: " + (nHumLogonMsgSize).ToString() + " B";
                            // LabelPlayMsgSize.Text = "普通: " + (nHumPlayMsgSize).ToString() + " B";
                            // LabelDeCodeMsgSize.Text = "解码: " + (nDeCodeMsgSize).ToString() + " B";
                            // LabelSendBlockSize.Text = "发送: " + (nSendBlockSize).ToString() + " B";
                            if (GateShare.dwCheckServerTimeMax > 1)
                            {
                                GateShare.dwCheckServerTimeMax -= 1;
                            }
                        }
                        nBufferOfM2Size = 0;
                        nReviceMsgSize = 0;
                        nDeCodeMsgSize = 0;
                        nSendBlockSize = 0;
                        nProcessMsgSize = 0;
                        nHumLogonMsgSize = 0;
                        nHumPlayMsgSize = 0;
                    }
                    try
                    {
                        dwProcessReviceMsgLimiTick = HUtil32.GetTickCount();
                        while (true)
                        {
                            if (GateShare.ReviceMsgList.Count <= 0)
                            {
                                break;
                            }
                            UserData = GateShare.ReviceMsgList[0];
                            GateShare.ReviceMsgList.RemoveAt(0);
                            ProcessUserPacket(UserData);
                            UserData = null;
                            if ((HUtil32.GetTickCount() - dwProcessReviceMsgLimiTick) > GateShare.dwProcessReviceMsgTimeLimit)
                            {
                                break;
                            }
                        }
                    }
                    catch (Exception E)
                    {
                        GateShare.AddMainLogMsg("[Exception] DecodeTimerTImer->ProcessUserPacket", 1);
                    }
                    try
                    {
                        dwProcessReviceMsgLimiTick = HUtil32.GetTickCount();
                        while (true)
                        {
                            if (GateShare.SendMsgList.Count <= 0)
                            {
                                break;
                            }
                            UserData = GateShare.SendMsgList[0];
                            GateShare.SendMsgList.RemoveAt(0);
                            ProcessPacket(UserData);
                            UserData = null;
                            if ((HUtil32.GetTickCount() - dwProcessReviceMsgLimiTick) > GateShare.dwProcessSendMsgTimeLimit)
                            {
                                break;
                            }
                        }
                    }
                    catch (Exception E)
                    {
                        GateShare.AddMainLogMsg("[Exception] DecodeTimerTImer->ProcessPacket", 1);
                    }
                    try
                    {
                        dwProcessReviceMsgLimiTick = HUtil32.GetTickCount();
                        if ((HUtil32.GetTickCount() - dwProcessPacketTick) > 300)
                        {
                            dwProcessPacketTick = HUtil32.GetTickCount();
                            if (GateShare.ReviceMsgList.Count > 0)
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
                            if (GateShare.SendMsgList.Count > 0)
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
                                if ((UserSession.Socket != null) && (UserSession.sSendData != ""))
                                {
                                    tUserData.nSocketIdx = i;
                                    tUserData.nSocketHandle = UserSession.nSckHandle;
                                    tUserData.sMsg = "";
                                    ProcessPacket(tUserData);
                                    if ((HUtil32.GetTickCount() - dwProcessReviceMsgLimiTick) > 20)
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
                    if ((HUtil32.GetTickCount() - dwCheckClientTick) > 2000)
                    {
                        dwCheckClientTick = HUtil32.GetTickCount();
                        if (GateShare.boGateReady)
                        {
                            SendServerMsg(Grobal2.GM_CHECKCLIENT, 0, 0, 0, 0, null);
                        }
                        if ((HUtil32.GetTickCount() - GateShare.dwCheckServerTick) > GateShare.dwCheckServerTimeOutTime)
                        {
                            GateShare.boCheckServerFail = true;
                            ClientSocket.Close();
                        }
                        if (dwLoopTime > 30)
                        {
                            dwLoopTime -= 20;
                        }
                        if (dwProcessServerMsgTime > 1)
                        {
                            dwProcessServerMsgTime -= 1;
                        }
                        if (dwProcessClientMsgTime > 1)
                        {
                            dwProcessClientMsgTime -= 1;
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
                if ((HUtil32.GetTickCount() - dwRefConsolMsgTick) > 1000)
                {
                    dwRefConsolMsgTick = HUtil32.GetTickCount();
                    // LabelLoopTime.Text = (dwLoopTime).ToString();
                    // LabelReviceLimitTime.Text = "接收处理限制: " + (GateShare.dwProcessReviceMsgTimeLimit).ToString();
                    // LabelSendLimitTime.Text = "发送处理限制: " + (GateShare.dwProcessSendMsgTimeLimit).ToString();
                    // LabelReceTime.Text = "接收: " + (dwProcessClientMsgTime);
                    // LabelSendTime.Text = "发送: " + (dwProcessServerMsgTime);
                }
            }
        }

        private void ProcessUserPacket(TSendUserData UserData)
        {
            string sMsg;
            string sData;
            string sDefMsg;
            string sDataMsg;
            string sDataText;
            string sHumName;
            string Buffer;
            int nOPacketIdx;
            int nPacketIdx;
            int nDataLen;
            int n14;
            TDefaultMessage DefMsg;
            try
            {
                n14 = 0;
                nProcessMsgSize += UserData.sMsg.Length;
                if ((UserData.nSocketIdx >= 0) && (UserData.nSocketIdx < GateShare.GATEMAXSESSION))
                {
                    if ((UserData.nSocketHandle == GateShare.SessionArray[UserData.nSocketIdx].nSckHandle) && (GateShare.SessionArray[UserData.nSocketIdx].nPacketErrCount < 10))
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
                                nPacketIdx = HUtil32.Str_ToInt(sData[1], 99); // 将数据名第一位的序号取出
                                if (GateShare.SessionArray[UserData.nSocketIdx].nPacketIdx == nPacketIdx)
                                {
                                    // 如果序号重复则增加错误计数
                                    GateShare.SessionArray[UserData.nSocketIdx].nPacketErrCount++;
                                }
                                else
                                {
                                    nOPacketIdx = GateShare.SessionArray[UserData.nSocketIdx].nPacketIdx;
                                    GateShare.SessionArray[UserData.nSocketIdx].nPacketIdx = nPacketIdx;
                                    sData = sData.Substring(2 - 1, sData.Length - 1);
                                    nDataLen = sData.Length;
                                    if ((nDataLen >= Grobal2.DEFBLOCKSIZE))
                                    {
                                        if (GateShare.SessionArray[UserData.nSocketIdx].boStartLogon)// 第一个人物登录数据包
                                        {
                                            nHumLogonMsgSize += sData.Length;
                                            GateShare.SessionArray[UserData.nSocketIdx].boStartLogon = false;
                                            sData = "#" + (nPacketIdx).ToString() + sData + "!";
                                            GetMem(Buffer, sData.Length + 1);
                                            Move(sData[1], Buffer, sData.Length + 1);
                                            SendServerMsg(Grobal2.GM_DATA, UserData.nSocketIdx, GateShare.SessionArray[UserData.nSocketIdx].Socket.SocketHandle, GateShare.SessionArray[UserData.nSocketIdx].nUserListIndex, sData.Length + 1, Buffer);
                                            FreeMem(Buffer);
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
                                                sDefMsg = sData.Substring(1 - 1, Grobal2.DEFBLOCKSIZE);
                                                sDataMsg = sData.Substring(Grobal2.DEFBLOCKSIZE + 1 - 1, sData.Length - Grobal2.DEFBLOCKSIZE);
                                            }
                                            DefMsg = EDcode.Units.EDcode.DecodeMessage(sDefMsg);
                                            // 检查数据
                                            if (sDataMsg != "")
                                            {
                                                if (DefMsg.Ident == Grobal2.CM_SAY)
                                                {
                                                    // 控制发言间隔时间
                                                    sDataText = EDcode.Units.EDcode.DecodeString(sDataMsg);
                                                    if (sDataText != "")
                                                    {
                                                        if (sDataText[1] == '/')
                                                        {
                                                            sDataText = HUtil32.GetValidStr3(sDataText, ref sHumName, new string[] { " " });
                                                            // 限制最长可发字符长度
                                                            FilterSayMsg(ref sDataText);
                                                            sDataText = sHumName + " " + sDataText;
                                                        }
                                                        else
                                                        {
                                                            if (sDataText[1] != '@')
                                                            {
                                                                // 限制最长可发字符长度
                                                                FilterSayMsg(ref sDataText);
                                                            }
                                                        }
                                                    }
                                                    sDataMsg = EDcode.Units.EDcode.EncodeString(sDataText);
                                                }
                                                GetMem(Buffer, sDataMsg.Length + sizeof(TDefaultMessage) + 1);
                                                Move(DefMsg, Buffer, sizeof(TDefaultMessage));
                                                Move(sDataMsg[1], Buffer[sizeof(TDefaultMessage)], sDataMsg.Length + 1);
                                                SendServerMsg(Grobal2.GM_DATA, UserData.nSocketIdx, GateShare.SessionArray[UserData.nSocketIdx].Socket.SocketHandle, GateShare.SessionArray[UserData.nSocketIdx].nUserListIndex, sDataMsg.Length + sizeof(TDefaultMessage) + 1, Buffer);
                                                FreeMem(Buffer);
                                            }
                                            else
                                            {
                                                GetMem(Buffer, sizeof(TDefaultMessage));
                                                Move(DefMsg, Buffer, sizeof(TDefaultMessage));
                                                SendServerMsg(Grobal2.GM_DATA, UserData.nSocketIdx, GateShare.SessionArray[UserData.nSocketIdx].Socket.SocketHandle, GateShare.SessionArray[UserData.nSocketIdx].nUserListIndex, sizeof(TDefaultMessage), Buffer);
                                                FreeMem(Buffer);
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
                if ((UserData.nSocketIdx >= 0) && (UserData.nSocketIdx < GateShare.GATEMAXSESSION))
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
            if ((UserData.nSocketIdx >= 0) && (UserData.nSocketIdx < GateShare.GATEMAXSESSION))
            {
                UserSession = GateShare.SessionArray[UserData.nSocketIdx];
                if (UserSession.nSckHandle == UserData.nSocketHandle)
                {
                    nDeCodeMsgSize += UserData.sMsg.Length;
                    sData = UserSession.sSendData + UserData.sMsg;
                    while (sData != "")
                    {
                        if (sData.Length > GateShare.nClientSendBlockSize)
                        {
                            sSendBlock = sData.Substring(1 - 1 ,GateShare.nClientSendBlockSize);
                            sData = sData.Substring(GateShare.nClientSendBlockSize + 1 - 1 ,sData.Length - GateShare.nClientSendBlockSize);
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
                            if ((UserSession.Socket != null) && (UserSession.Socket.Connected))
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
                GateShare.CS_FilterMsg.Enter;
                for (var i = 0; i < GateShare.AbuseList.Count; i++)
                {
                    sFilterText = GateShare.AbuseList[i];
                    sReplaceText = "";
                    if ((sMsg.IndexOf(sFilterText) != -1))
                    {
                        for (nLen = 1; nLen <= sFilterText.Length; nLen++)
                        {
                            sReplaceText = sReplaceText + GateShare.sReplaceWord;
                        }

                        sMsg = sMsg.Replace(sFilterText, sReplaceText);
                    }
                }
            }
            finally
            {
                GateShare.CS_FilterMsg.Leave;
            }
        }

        public void ClientSocketError(Object Sender, Socket Socket)
        {
            Socket.Close();
            boServerReady = false;
        }
        
        private void StartService()
        {
            try
            {
                GateShare.AddMainLogMsg("正在启动服务...", 2);
                GateShare.boServiceStart = true;
                GateShare.boGateReady = false;
                GateShare.boCheckServerFail = false;
                GateShare.boSendHoldTimeOut = false;
                GateShare.SessionCount = 0;
                LoadConfig();
                this.Text = GateShare.GateName + " - " + GateShare.TitleName;
                RestSessionArray();
                GateShare.dwProcessReviceMsgTimeLimit = 50;
                GateShare.dwProcessSendMsgTimeLimit = 50;
                boServerReady = false;
                dwReConnectServerTime = HUtil32.GetTickCount() - 25000;
                dwRefConsolMsgTick = HUtil32.GetTickCount();
                
                //ServerSocket.Active = false;
                //ServerSocket.Address = GateShare.GateAddr;
                //ServerSocket.Port = GateShare.GatePort;
                ServerSocket = new ISocketServer(20,ushort.MaxValue);
                ServerSocket.Init();
                ServerSocket.Start(GateShare.GateAddr, GateShare.GatePort);
                //ServerSocket.Active = true;
                
                //ClientSocket.Active = false;
                ClientSocket.Address = GateShare.ServerAddr;
                ClientSocket.Port = GateShare.ServerPort;
                //ClientSocket.Active = true;
                
                SendTimer.Enabled = true;
                
                GateShare.AddMainLogMsg("服务已启动成功...", 2);
                // GateShare.AddMainLogMsg("欢迎使用翎风系列游戏软件...",0);
                // GateShare.AddMainLogMsg("网站:http://www.gameofmir.com",0);
                // GateShare.AddMainLogMsg("论坛:http://bbs.gameofmir.com",0);
            }
            catch (Exception E)
            {
                GateShare.AddMainLogMsg(E.Message, 0);
            }
        }

        private void StopService()
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
            ServerSocket.Close();
            ClientSocket.Close();
            GateShare.AddMainLogMsg("服务停止成功...", 2);
        }

        private void LoadConfig()
        {
            GateShare.AddMainLogMsg("正在加载配置信息...", 3);
            if (GateShare.Conf != null)
            {
                // GateShare.TitleName = GateShare.Conf.ReadString(GateShare.GateClass, "Title", GateShare.TitleName);
                // GateShare.ServerAddr = GateShare.Conf.ReadString(GateShare.GateClass, "ServerAddr", GateShare.ServerAddr);
                // GateShare.ServerPort = GateShare.Conf.ReadInteger(GateShare.GateClass, "ServerPort", GateShare.ServerPort);
                // GateShare.GateAddr = GateShare.Conf.ReadString(GateShare.GateClass, "GateAddr", GateShare.GateAddr);
                // GateShare.GatePort = GateShare.Conf.ReadInteger(GateShare.GateClass, "GatePort", GateShare.GatePort);
                // GateShare.nShowLogLevel = GateShare.Conf.ReadInteger(GateShare.GateClass, "ShowLogLevel", GateShare.nShowLogLevel);
                // GateShare.boShowBite = GateShare.Conf.ReadBool(GateShare.GateClass, "ShowBite", GateShare.boShowBite);
                // GateShare.nMaxConnOfIPaddr = GateShare.Conf.ReadInteger(GateShare.GateClass, "MaxConnOfIPaddr", GateShare.nMaxConnOfIPaddr);
                // GateShare.BlockMethod = ((Grobal2.TBlockIPMethod)(GateShare.Conf.ReadInteger(GateShare.GateClass, "BlockMethod", ((int)GateShare.BlockMethod))));
                // GateShare.nMaxClientPacketSize = GateShare.Conf.ReadInteger(GateShare.GateClass, "MaxClientPacketSize", GateShare.nMaxClientPacketSize);
                // GateShare.nNomClientPacketSize = GateShare.Conf.ReadInteger(GateShare.GateClass, "NomClientPacketSize", GateShare.nNomClientPacketSize);
                // GateShare.nMaxClientMsgCount = GateShare.Conf.ReadInteger(GateShare.GateClass, "MaxClientMsgCount", GateShare.nMaxClientMsgCount);
                // GateShare.bokickOverPacketSize = GateShare.Conf.ReadBool(GateShare.GateClass, "kickOverPacket", GateShare.bokickOverPacketSize);
                // GateShare.dwCheckServerTimeOutTime = GateShare.Conf.ReadInteger(GateShare.GateClass, "ServerCheckTimeOut", GateShare.dwCheckServerTimeOutTime);
                // GateShare.nClientSendBlockSize = GateShare.Conf.ReadInteger(GateShare.GateClass, "ClientSendBlockSize", GateShare.nClientSendBlockSize);
                // GateShare.dwClientTimeOutTime = GateShare.Conf.ReadInteger(GateShare.GateClass, "ClientTimeOutTime", GateShare.dwClientTimeOutTime);
                // GateShare.dwSessionTimeOutTime = GateShare.Conf.ReadInteger(GateShare.GateClass, "SessionTimeOutTime", GateShare.dwSessionTimeOutTime);
                // GateShare.nSayMsgMaxLen = GateShare.Conf.ReadInteger(GateShare.GateClass, "SayMsgMaxLen", GateShare.nSayMsgMaxLen);
                // GateShare.dwSayMsgTime = GateShare.Conf.ReadInteger(GateShare.GateClass, "SayMsgTime", GateShare.dwSayMsgTime);
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
                    MemoLog.Lines.Add(TempLogList[i]);
                }
                TempLogList.Clear();
            }
            finally
            {
                boShowLocked = false;
            }
        }

        public void FormCreate(System.Object Sender, System.EventArgs _e1)
        {
            TempLogList = new ArrayList();
            dwLoopCheckTick = HUtil32.GetTickCount();
        }

        public void FormDestroy(Object Sender)
        {
            GateShare.BlockIPList.SaveToFile(".\\BlockIPList.txt");
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

        private void RestSessionArray()
        {
            TSessionInfo tSession;
            for (var i = 0; i < GateShare.GATEMAXSESSION; i ++ )
            {
                tSession = GateShare.SessionArray[i];
                tSession.Socket = null;
                tSession.sSocData = "";
                tSession.sSendData = "";
                tSession.nUserListIndex = 0;
                tSession.nPacketIdx =  -1;
                tSession.nPacketErrCount = 0;
                tSession.boStartLogon = true;
                tSession.boSendLock = false;
                tSession.boOverNomSize = false;
                tSession.nOverNomSizeCount = 0;
                tSession.dwSendLatestTime = HUtil32.GetTickCount();
                tSession.boSendAvailable = true;
                tSession.boSendCheck = false;
                tSession.nCheckSendLength = 0;
                tSession.nReceiveLength = 0;
                tSession.dwReceiveTick = HUtil32.GetTickCount();
                tSession.nSckHandle =  -1;
                tSession.dwSayMsgTick = HUtil32.GetTickCount();
            }
        }

        public void ServerSocketClientConnect(Object Sender, Socket Socket)
        {
            int nSockIdx;
            TSessionInfo UserSession;
            //Socket.nIndex =  -1;
            string sRemoteAddress = Socket.RemoteAddress;
            if (GateShare.boGateReady)
            {
                try
                {
                    for (nSockIdx = 0; nSockIdx < GateShare.GATEMAXSESSION; nSockIdx++)
                    {
                        UserSession = GateShare.SessionArray[nSockIdx];
                        if (UserSession.Socket == null)
                        {
                            UserSession.Socket = Socket;
                            UserSession.sSocData = "";
                            UserSession.sSendData = "";
                            UserSession.nUserListIndex = 0;
                            UserSession.nPacketIdx = -1;
                            UserSession.nPacketErrCount = 0;
                            UserSession.boStartLogon = true;
                            UserSession.boSendLock = false;
                            UserSession.dwSendLatestTime = HUtil32.GetTickCount();
                            UserSession.boSendAvailable = true;
                            UserSession.boSendCheck = false;
                            UserSession.nCheckSendLength = 0;
                            UserSession.nReceiveLength = 0;
                            UserSession.dwReceiveTick = HUtil32.GetTickCount();
                            //UserSession.nSckHandle = Socket.SocketHandle;
                            UserSession.sRemoteAddr = sRemoteAddress;
                            UserSession.boOverNomSize = false;
                            UserSession.nOverNomSizeCount = 0;
                            UserSession.dwSayMsgTick = HUtil32.GetTickCount();
                            //Socket.nIndex = nSockIdx;
                            GateShare.SessionCount++;
                            break;
                        }
                    }
                }
                finally
                {
                }
                if (nSockIdx < GateShare.GATEMAXSESSION)
                {
                    SendServerMsg(Grobal2.GM_OPEN, nSockIdx, Socket.SocketHandle, 0, Socket.RemoteAddress.Length + 1, (Socket.RemoteAddress as string));
                    //Socket.nIndex = nSockIdx;
                    GateShare.AddMainLogMsg("开始连接: " + sRemoteAddress, 5);
                }
                else
                {
                    //Socket.nIndex =  -1;
                    Socket.Close();
                    GateShare.AddMainLogMsg("禁止连接: " + sRemoteAddress, 1);
                }
            }
            else
            {
                //Socket.nIndex =  -1;
                Socket.Close();
                GateShare.AddMainLogMsg("禁止连接: " + sRemoteAddress, 1);
            }
        }

        public void ServerSocketClientDisconnect(Object Sender, Socket Socket)
        {
            int nSockIndex;
            TSessionInfo UserSession;
            string sRemoteAddr = Socket.RemoteAddress;
            nSockIndex = Socket.nIndex;
            if ((nSockIndex >= 0) && (nSockIndex < GateShare.GATEMAXSESSION))
            {
                UserSession = GateShare.SessionArray[nSockIndex];
                UserSession.Socket = null;
                UserSession.nSckHandle =  -1;
                UserSession.sSocData = "";
                UserSession.sSendData = "";
                Socket.nIndex =  -1;
                GateShare.SessionCount -= 1;
                if (GateShare.boGateReady)
                {
                    SendServerMsg(Grobal2.GM_CLOSE, 0, Socket.SocketHandle, 0, 0, null);
                    GateShare.AddMainLogMsg("断开连接: " + Socket.RemoteAddress, 5);
                }
            }
        }

        public void ServerSocketClientError(Object Sender, Socket Socket)
        {
            Socket.Close();
        }

        public void ServerSocketClientRead(Object Sender, Socket Socket)
        {
            long dwProcessMsgTick;
            long dwProcessMsgTime;
            int nReviceLen;
            string sReviceMsg;
            string sRemoteAddress;
            int nSocketIndex;
            int nPos;
            TSendUserData UserData;
            int nMsgCount;
            TSessionInfo UserSession;
            try {
                dwProcessMsgTick = HUtil32.GetTickCount();
                sRemoteAddress = Socket.RemoteAddress;
                nSocketIndex = Socket.nIndex;
                sReviceMsg = Socket.ReceiveText;
                nReviceLen = sReviceMsg.Length;
                if ((nSocketIndex >= 0) && (nSocketIndex < GateShare.GATEMAXSESSION) && (sReviceMsg != "") && boServerReady)
                {
                    if (nReviceLen > GateShare.nNomClientPacketSize)
                    {
                        nMsgCount = HUtil32.TagCount(sReviceMsg, '!');
                        if ((nMsgCount > GateShare.nMaxClientMsgCount) || (nReviceLen > GateShare.nMaxClientPacketSize))
                        {
                            if (GateShare.bokickOverPacketSize)
                            {
                                switch(GateShare.BlockMethod)
                                {
                                    case TBlockIPMethod.mDisconnect:
                                        break;
                                    case TBlockIPMethod.mBlock:
                                        GateShare.TempBlockIPList.Add(sRemoteAddress);
                                        CloseConnect(sRemoteAddress);
                                        break;
                                    case TBlockIPMethod.mBlockList:
                                        GateShare.BlockIPList.Add(sRemoteAddress);
                                        CloseConnect(sRemoteAddress);
                                        break;
                                }
                                GateShare.AddMainLogMsg("踢除连接: IP(" + sRemoteAddress + "),信息数量(" + (nMsgCount).ToString() + "),数据包长度(" + (nReviceLen).ToString() + ")", 1);
                                Socket.Close();
                            }
                            return;
                        }
                    }
                    nReviceMsgSize += sReviceMsg.Length;
                    if (GateShare.boShowSckData)
                    {
                        GateShare.AddMainLogMsg(sReviceMsg, 0);
                    }
                    UserSession = GateShare.SessionArray[nSocketIndex];
                    if (UserSession.Socket == Socket)
                    {
                        nPos = sReviceMsg.IndexOf("*");
                        if (nPos > 0)
                        {
                            UserSession.boSendAvailable = true;
                            UserSession.boSendCheck = false;
                            UserSession.nCheckSendLength = 0;
                            UserSession.dwReceiveTick = HUtil32.GetTickCount();
                            sReviceMsg = sReviceMsg.Substring(1 - 1 ,nPos - 1) + sReviceMsg.Substring(nPos + 1 - 1 ,sReviceMsg.Length);
                        }
                        if ((sReviceMsg != "") && GateShare.boGateReady && !GateShare.boCheckServerFail)
                        {
                            UserData = new TSendUserData();
                            UserData.nSocketIdx = nSocketIndex;
                            UserData.nSocketHandle = Socket.SocketHandle;
                            UserData.sMsg = sReviceMsg;
                            GateShare.ReviceMsgList.Add(UserData);
                        }
                    }
                }
                dwProcessMsgTime = HUtil32.GetTickCount() - dwProcessMsgTick;
                if (dwProcessMsgTime > dwProcessClientMsgTime)
                {
                    dwProcessClientMsgTime = dwProcessMsgTime;
                }
            }
            catch {
                GateShare.AddMainLogMsg("[Exception] ClientRead", 1);
            }
        }

        public void SendTimerTimer(System.Object Sender, System.EventArgs _e1)
        {
            TSessionInfo UserSession;
            if (( HUtil32.GetTickCount() - GateShare.dwSendHoldTick) > 3000)
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
                        if (( HUtil32.GetTickCount() - UserSession.dwReceiveTick) > GateShare.dwSessionTimeOutTime)
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
                StatusBar.Panels[1].Text = "未连接";
                StatusBar.Panels[3].Text = "????";
                POPMENU_CHECKTICK.Text = "????";
                if ((( HUtil32.GetTickCount() - dwReConnectServerTime) > 1000) && GateShare.boServiceStart)
                {
                    dwReConnectServerTime = HUtil32.GetTickCount();
                    ClientSocket.Active = false;
                    ClientSocket.Address = GateShare.ServerAddr;
                    ClientSocket.Port = GateShare.ServerPort;
                    ClientSocket.Active = true;
                }
            }
            else
            {
                if (GateShare.boCheckServerFail)
                {
                    //StatusBar.Panels[1].Text = "超时";
                }
                else
                {
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

        public void ClientSocketConnect(Socket Socket)
        {
            GateShare.boGateReady = true;
            GateShare.dwCheckServerTick = HUtil32.GetTickCount();
            GateShare.dwCheckRecviceTick = HUtil32.GetTickCount();
            RestSessionArray();
            boServerReady = true;
            GateShare.dwCheckServerTimeMax = 0;
            GateShare.dwCheckServerTimeMax = 0;
        }

        public void ClientSocketDisconnect(Object Sender, Socket Socket)
        {
            TSessionInfo UserSession;
            for (var i = 0; i < GateShare.GATEMAXSESSION; i ++ )
            {
                UserSession = GateShare.SessionArray[i];
                if (UserSession.Socket != null)
                {
                    UserSession.Socket.Close();
                    UserSession.Socket = null;
                    UserSession.nSckHandle =  -1;
                }
            }
            RestSessionArray();
            if (GateShare.SocketBuffer != null)
            {
               // FreeMem(GateShare.SocketBuffer);
            }
            GateShare.SocketBuffer = null;
            for (var i = 0; i < GateShare.List_45AA58.Count; i ++ )
            {
                
            }
            GateShare.List_45AA58.Clear();
            GateShare.boGateReady = false;
            boServerReady = false;
        }

        public void ClientSocketRead(Object Sender, Socket Socket)
        {
            long dwTime10;
            long dwTick14;
            int nMsgLen;
            string tBuffer;
            try
            {
                dwTick14 = HUtil32.GetTickCount();
                nMsgLen = Socket.ReceiveLength;
                GetMem(tBuffer, nMsgLen);
                Socket.ReceiveBuf(tBuffer, nMsgLen);
                ProcReceiveBuffer(tBuffer, nMsgLen);
                nBufferOfM2Size += nMsgLen;
                dwTime10 = HUtil32.GetTickCount() - dwTick14;
                if (dwProcessServerMsgTime < dwTime10)
                {
                    dwProcessServerMsgTime = dwTime10;
                }
            }
            catch (Exception E)
            {
                GateShare.AddMainLogMsg("[Exception] ClientSocketRead", 1);
            }
        }

        private void ProcReceiveBuffer(string tBuffer, int nMsgLen)
        {
            int nLen;
            string Buff;
            TMsgHeader pMsg;
            string MsgBuff;
            string TempBuff;
            try
            {
                ReallocMem(GateShare.SocketBuffer, GateShare.nBuffLen + nMsgLen);
                Move(tBuffer, GateShare.SocketBuffer[GateShare.nBuffLen], nMsgLen);
                FreeMem(tBuffer);
                nLen = GateShare.nBuffLen + nMsgLen;
                Buff = GateShare.SocketBuffer;
                if (nLen >= sizeof(TMsgHeader))
                {
                    while (true)
                    {
                        pMsg = ((TMsgHeader) (Buff));
                        if (pMsg.dwCode == Grobal2.RUNGATECODE)
                        {
                            if ((Math.Abs(pMsg.nLength) + sizeof(TMsgHeader)) > nLen)
                            {
                                break;
                            }
                            MsgBuff = Ptr((long) Buff + sizeof(TMsgHeader));
                            switch (pMsg.wIdent)
                            {
                                case Grobal2.GM_CHECKSERVER:
                                    GateShare.boCheckServerFail = false;
                                    GateShare.dwCheckServerTick = HUtil32.GetTickCount();
                                    break;
                                case Grobal2.GM_SERVERUSERINDEX:
                                    if ((pMsg.wGSocketIdx < GateShare.GATEMAXSESSION) && (pMsg.nSocket ==
                                        GateShare.SessionArray[pMsg.wGSocketIdx].nSckHandle))
                                    {
                                        GateShare.SessionArray[pMsg.wGSocketIdx].nUserListIndex = pMsg.wUserListIndex;
                                    }
                                    break;
                                case Grobal2.GM_RECEIVE_OK:
                                    GateShare.dwCheckServerTimeMin = HUtil32.GetTickCount() - GateShare.dwCheckRecviceTick;
                                    if (GateShare.dwCheckServerTimeMin > GateShare.dwCheckServerTimeMax)
                                    {
                                        GateShare.dwCheckServerTimeMax = GateShare.dwCheckServerTimeMin;
                                    }
                                    GateShare.dwCheckRecviceTick = HUtil32.GetTickCount();
                                    SendServerMsg(Grobal2.GM_RECEIVE_OK, 0, 0, 0, 0, null);
                                    break;
                                case Grobal2.GM_DATA:
                                    ProcessMakeSocketStr(pMsg.nSocket, pMsg.wGSocketIdx, MsgBuff, pMsg.nLength);
                                    break;
                                case Grobal2.GM_TEST:
                                    break;
                            }
                            Buff = Buff[sizeof(TMsgHeader) + Math.Abs(pMsg.nLength)];
                            nLen = nLen - (Math.Abs(pMsg.nLength) + sizeof(TMsgHeader));
                        }
                        else
                        {
                            Buff++;
                            nLen -= 1;
                        }
                        if (nLen < sizeof(TMsgHeader))
                        {
                            break;
                        }
                    }
                }
                if (nLen > 0)
                {
                    GetMem(TempBuff, nLen);
                    Move(Buff, TempBuff, nLen);
                    FreeMem(GateShare.SocketBuffer);
                    GateShare.SocketBuffer = TempBuff;
                    GateShare.nBuffLen = nLen;
                }
                else
                {
                    FreeMem(GateShare.SocketBuffer);
                    GateShare.SocketBuffer = null;
                    GateShare.nBuffLen = 0;
                }
            }
            catch (Exception E)
            {
                GateShare.AddMainLogMsg("[Exception] ProcReceiveBuffer", 1);
            }
        }

        private void ProcessMakeSocketStr(int nSocket, int nSocketIndex, string Buffer, int nMsgLen)
        {
            string sSendMsg;
            TDefaultMessage pDefMsg;
            TSendUserData UserData;
            try
            {
                sSendMsg = "";
                if (nMsgLen < 0)
                {
                    sSendMsg = "#" + (Buffer as string) + "!";
                }
                else
                {
                    if ((nMsgLen >= sizeof(TDefaultMessage)))
                    {
                        pDefMsg = ((TDefaultMessage) (Buffer));
                        if (nMsgLen > sizeof(TDefaultMessage))
                        {
                            sSendMsg = "#" + EDcode.Units.EDcode.EncodeMessage(pDefMsg) + ((Buffer[sizeof(TDefaultMessage)] as string) as string) + "!";
                        }
                        else
                        {
                            sSendMsg = "#" + EDcode.Units.EDcode.EncodeMessage(pDefMsg) + "!";
                        }
                    }
                }
                if ((nSocketIndex >= 0) && (nSocketIndex < GateShare.GATEMAXSESSION) &&
                    (sSendMsg != ""))
                {
                    UserData = new TSendUserData();
                    UserData.nSocketIdx = nSocketIndex;
                    UserData.nSocketHandle = nSocket;
                    UserData.sMsg = sSendMsg;
                    GateShare.SendMsgList.Add(UserData);
                }
            }
            catch (Exception E)
            {
                GateShare.AddMainLogMsg("[Exception] ProcessMakeSocketStr", 1);
            }
        }

        private bool IsBlockIP(string sIPaddr)
        {
            bool result= false;
            string sBlockIPaddr;
            for (var i = 0; i < GateShare.TempBlockIPList.Count; i ++ )
            {
                sBlockIPaddr = GateShare.TempBlockIPList[i];
                if ((sIPaddr).ToLower().CompareTo((sBlockIPaddr).ToLower()) == 0)
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
            for (var i = 0; i < ServerSocket.Socket.ActiveConnections; i ++ )
            {
                if ((sIPaddr).ToLower().CompareTo((ServerSocket.Socket.Connections[i].RemoteAddress).ToLower()) == 0)
                {
                    nCount ++;
                }
            }
            if (nCount > GateShare.nMaxConnOfIPaddr)
            {
                result = true;
            }
            return result;
        }

        public void CloseConnect(string sIPaddr)
        {
            int i;
            bool boCheck;
            if (ServerSocket.Active)
            {
                while (true)
                {
                    boCheck = false;
                    for (i = 0; i < ServerSocket.Socket.ActiveConnections; i ++ )
                    {
                        if (sIPaddr == ServerSocket.Socket.Connections[i].RemoteAddress)
                        {
                            ServerSocket.Socket.Connections[i].Close;
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

        private bool CheckDefMsg(TDefaultMessage DefMsg, TSessionInfo SessionInfo)
        {
            bool result;
            result = true;
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

