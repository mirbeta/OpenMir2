using Microsoft.Extensions.Logging;
using System;
using SystemModule;
using SystemModule.Sockets;
using SystemModule.Sockets.AsyncSocketServer;

namespace RunGate
{
    /// <summary>
    /// 客户端服务端(Mir2-RunGate)
    /// </summary>
    public class ServerService
    {
        private ISocketServer ServerSocket;
        public int nReviceMsgSize = 0;
        public long dwProcessClientMsgTime = 0;
        private readonly UserClientService _userClient;
        private readonly ILogger<AppService> _logger;
        
        public ServerService(ILogger<AppService> logger,UserClientService userClient)
        {
            _logger = logger;
            _userClient = userClient;
            ServerSocket = new ISocketServer(20, 2048);
            ServerSocket.OnClientConnect += ServerSocketClientConnect;
            ServerSocket.OnClientDisconnect += ServerSocketClientDisconnect;
            ServerSocket.OnClientRead += ServerSocketClientRead;
            ServerSocket.OnClientError += ServerSocketClientError;
            ServerSocket.Init();
        }

        public void Start()
        {
            ServerSocket.Start(GateShare.GateAddr, GateShare.GatePort);
        }

        public void Stop()
        {
            ServerSocket.Shutdown();
        }

        /// <summary>
        /// 新玩家链接
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="e"></param>
        private void ServerSocketClientConnect(object Sender,AsyncUserToken e)
        {
            TSessionInfo UserSession;
            var sRemoteAddress = e.RemoteIPaddr;
            var nSockIdx = 0;
            if (GateShare.boGateReady)
            {
                try
                {
                    for (var nIdx = 0; nIdx < GateShare.GATEMAXSESSION; nIdx++)
                    {
                        UserSession = GateShare.SessionArray[nIdx];
                        if (UserSession.Socket == null)
                        {
                            UserSession.Socket = e.Socket;
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
                            UserSession.sRemoteAddr = sRemoteAddress;
                            UserSession.boOverNomSize = false;
                            UserSession.nOverNomSizeCount = 0;
                            UserSession.dwSayMsgTick = HUtil32.GetTickCount();
                            UserSession.nSckHandle = (int)e.Socket.Handle;
                            nSockIdx = nIdx;
                            GateShare.SessionIndex.TryAdd(e.ConnectionId, nIdx);
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
                    _userClient.SendServerMsg(Grobal2.GM_OPEN, nSockIdx, (int)e.Socket.Handle, 0, e.RemoteIPaddr.Length, e.RemoteIPaddr);//通知M2有新玩家进入游戏
                    GateShare.AddMainLogMsg("开始连接: " + sRemoteAddress, 5);
                }
                else
                {
                    GateShare.DelSocketIndex(e.ConnectionId);
                    e.Socket.Close();
                    GateShare.AddMainLogMsg("禁止连接: " + sRemoteAddress, 1);
                }
            }
            else
            {
                GateShare.DelSocketIndex(e.ConnectionId);
                e.Socket.Close();
                GateShare.AddMainLogMsg("禁止连接: " + sRemoteAddress, 1);
            }
        }

        private void ServerSocketClientDisconnect(object Sender, AsyncUserToken e)
        {
            TSessionInfo UserSession;
            var sRemoteAddr = e.RemoteIPaddr;
            var nSockIndex = GateShare.GetSocketIndex(e.ConnectionId);
            if (nSockIndex >= 0 && nSockIndex < GateShare.GATEMAXSESSION)
            {
                UserSession = GateShare.SessionArray[nSockIndex];
                UserSession.Socket = null;
                UserSession.nSckHandle = -1;
                UserSession.sSocData = "";
                UserSession.sSendData = "";
                GateShare.SessionCount -= 1;
                if (GateShare.boGateReady)
                {
                    _userClient.SendServerMsg(Grobal2.GM_CLOSE, 0, (int)e.Socket.Handle, 0, 0, "");//发送消息给M2断开链接
                    GateShare.AddMainLogMsg("断开连接: " + sRemoteAddr, 5);
                }
                GateShare.DelSocketIndex(e.ConnectionId);
            }
        }

        private void ServerSocketClientError(object Sender, AsyncSocketErrorEventArgs e)
        {
            Console.WriteLine("客户端链接错误.");
        }

        /// <summary>
        /// 收到客户端消息
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="token"></param>
        private void ServerSocketClientRead(object Sender, AsyncUserToken token)
        {
            long dwProcessMsgTick = 0;
            long dwProcessMsgTime= 0;
            var nReviceLen= 0;
            var sReviceMsg = string.Empty;
            var sRemoteAddress = string.Empty;
            var nSocketIndex = GateShare.GetSocketIndex(token.ConnectionId);
            var nPos= 0;
            TSendUserData UserData = null;
            var nMsgCount= 0;
            TSessionInfo UserSession = null;
            try
            {
                dwProcessMsgTick = HUtil32.GetTickCount();
                sRemoteAddress = token.RemoteIPaddr;
                var data = new byte[token.BytesReceived];
                Buffer.BlockCopy(token.ReceiveBuffer, token.Offset, data, 0, token.BytesReceived);
                sReviceMsg = HUtil32.GetString(data, 0, data.Length);
                nReviceLen = token.BytesReceived;
                Console.WriteLine("nSocketIndex:" + nSocketIndex);
                if (nSocketIndex is >= 0 and < GateShare.GATEMAXSESSION && !string.IsNullOrEmpty(sReviceMsg) && GateShare.boServerReady)
                {
                    if (nReviceLen > GateShare.nNomClientPacketSize)
                    {
                        nMsgCount = HUtil32.TagCount(sReviceMsg, '!');
                        if (nMsgCount > GateShare.nMaxClientMsgCount || nReviceLen > GateShare.nMaxClientPacketSize)
                        {
                            if (GateShare.bokickOverPacketSize)
                            {
                                switch (GateShare.BlockMethod)
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
                                GateShare.AddMainLogMsg("踢除连接: IP(" + sRemoteAddress + "),信息数量(" + nMsgCount + "),数据包长度(" + nReviceLen + ")", 1);
                                token.Socket.Close();
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
                    if (UserSession.Socket == token.Socket)
                    {
                        nPos = sReviceMsg.IndexOf("*", StringComparison.Ordinal);
                        if (nPos > -1)
                        {
                            UserSession.boSendAvailable = true;
                            UserSession.boSendCheck = false;
                            UserSession.nCheckSendLength = 0;
                            UserSession.dwReceiveTick = HUtil32.GetTickCount();
                            sReviceMsg = sReviceMsg.Substring(0, nPos);
                            //sReviceMsg = sReviceMsg.Substring(nPos + 1, sReviceMsg.Length);
                        }
                        if (sReviceMsg != "" && GateShare.boGateReady && !GateShare.boCheckServerFail)
                        {
                            UserData = new TSendUserData();
                            UserData.nSocketIdx = nSocketIndex;
                            UserData.nSocketHandle = (int)token.Socket.Handle;
                            UserData.sMsg = sReviceMsg;
                            GateShare.ReviceMsgList.Writer.TryWrite(UserData);
                        }
                    }
                }
                dwProcessMsgTime = HUtil32.GetTickCount() - dwProcessMsgTick;
                if (dwProcessMsgTime > dwProcessClientMsgTime)
                {
                    dwProcessClientMsgTime = dwProcessMsgTime;
                }
            }
            catch
            {
                GateShare.AddMainLogMsg("[Exception] ClientRead", 1);
            }
        }

        private void CloseConnect(string sIPaddr)
        {
            var userSocket = ServerSocket.GetSocket(sIPaddr);
            userSocket?.Socket.Close();
        }
    }
}