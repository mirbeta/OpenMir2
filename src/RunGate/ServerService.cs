using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
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
        private readonly GateService _gateService;
        
        public ServerService(ILogger<AppService> logger,UserClientService userClient, GateService gateService)
        {
            _logger = logger;
            _userClient = userClient;
            _gateService = gateService;
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
        private void ServerSocketClientConnect(object Sender, AsyncUserToken e)
        {
            TSessionInfo UserSession;
            var sRemoteAddress = e.RemoteIPaddr;
            var nSockIdx = 0;
            //todo 新玩家链接的时候要随机分配一个可用网关客户端
            //根据配置文件有三种模式
            //1.轮询分配
            //2.总是分配到最小资源 即网关在线人数最小的那个
            //3.一直分配到一个 直到当前玩家达到配置上线，则开始分配到其他可用网关
            
            //从全局服务获取可用网关服务进行分配 
            
            //需要记录socket会话ID和链接网关

            var gateclient = _gateService.GetClientService();

            for (var nIdx = 0; nIdx < gateclient.SessionArray.Length; nIdx++)
            {
                UserSession = gateclient.SessionArray[nIdx];
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
                    UserSession.boSendAvailable = true;
                    UserSession.boSendCheck = false;
                    UserSession.nCheckSendLength = 0;
                    UserSession.dwReceiveTick = HUtil32.GetTickCount();
                    UserSession.sRemoteAddr = sRemoteAddress;
                    UserSession.dwSayMsgTick = HUtil32.GetTickCount();
                    UserSession.nSckHandle = (int) e.Socket.Handle;
                    nSockIdx = nIdx;
                    GateShare.SessionIndex.TryAdd(e.ConnectionId, nIdx);
                    GateShare.SessionCount++;
                    break;
                }
            }
            if (nSockIdx < gateclient.GetMaxSession())
            {
                _userClient.SendServerMsg(Grobal2.GM_OPEN, nSockIdx, (int) e.Socket.Handle, 0, e.RemoteIPaddr.Length,
                    e.RemoteIPaddr); //通知M2有新玩家进入游戏
                GateShare.AddMainLogMsg("开始连接: " + sRemoteAddress, 5);
                GateShare._ClientGateMap.TryAdd(e.ConnectionId, gateclient);//链接成功后建立对应关系
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
            var userClinet = GateShare.GetUserClient(e.ConnectionId);
            if (userClinet != null)
            {
                if (nSockIndex >= 0 && nSockIndex < userClinet.GetMaxSession())
                {
                    UserSession = userClinet.SessionArray[nSockIndex];
                    UserSession.Socket = null;
                    UserSession.nSckHandle = -1;
                    UserSession.sSocData = "";
                    UserSession.sSendData = "";
                    GateShare.SessionCount -= 1;
                    if (GateShare.boGateReady)
                    {
                        _userClient.SendServerMsg(Grobal2.GM_CLOSE, 0, (int) e.Socket.Handle, 0, 0, ""); //发送消息给M2断开链接
                        GateShare.AddMainLogMsg("断开连接: " + sRemoteAddr, 5);
                    }
                    GateShare.DelSocketIndex(e.ConnectionId);
                    GateShare.DeleteUserClient(e.ConnectionId);
                }
            }
            else
            {
                GateShare.DelSocketIndex(e.ConnectionId);
                GateShare.DeleteUserClient(e.ConnectionId);
                GateShare.AddMainLogMsg("断开链接: " + sRemoteAddr, 5);
                Debug.WriteLine($"获取用户对应网关失败 RemoteAddr:[{sRemoteAddr}] ConnectionId:[{e.ConnectionId}]");
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
            var nSocketIndex = GateShare.GetSocketIndex(token.ConnectionId);
            var userClinet = GateShare.GetUserClient(token.ConnectionId);
            string sRemoteAddress = token.RemoteIPaddr;
            if (userClinet == null)
            {
                GateShare.AddMainLogMsg("非法攻击: " + token.RemoteIPaddr, 5);
                Debug.WriteLine($"获取用户对应网关失败 RemoteAddr:[{sRemoteAddress}] ConnectionId:[{token.ConnectionId}]");
                return;
            }
            try
            {
                long dwProcessMsgTick = HUtil32.GetTickCount();
                var data = new byte[token.BytesReceived];
                Buffer.BlockCopy(token.ReceiveBuffer, token.Offset, data, 0, token.BytesReceived);
                string sReviceMsg = HUtil32.GetString(data, 0, data.Length);
                int nReviceLen = token.BytesReceived;

                if (nSocketIndex > 0 && nSocketIndex < userClinet.GetMaxSession() && !string.IsNullOrEmpty(sReviceMsg) && GateShare.boServerReady)
                {
                    if (nReviceLen > GateShare.nNomClientPacketSize)
                    {
                        int nMsgCount = HUtil32.TagCount(sReviceMsg, '!');
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
                    TSessionInfo UserSession = userClinet.SessionArray[nSocketIndex];
                    if (UserSession.Socket == token.Socket)
                    {
                        int nPos = sReviceMsg.IndexOf("*", StringComparison.Ordinal);
                        if (nPos > -1)
                        {
                            UserSession.boSendAvailable = true;
                            UserSession.boSendCheck = false;
                            UserSession.nCheckSendLength = 0;
                            UserSession.dwReceiveTick = HUtil32.GetTickCount();
                            sReviceMsg = sReviceMsg.Substring(0, nPos);
                            //sReviceMsg = sReviceMsg.Substring(nPos + 1, sReviceMsg.Length);
                        }
                        if (!string.IsNullOrEmpty(sReviceMsg) && GateShare.boGateReady) //&& !GateShare.boCheckServerFail
                        {
                            var UserData = new TSendUserData();
                            UserData.nSocketIdx = nSocketIndex;
                            UserData.nSocketHandle = (int)token.Socket.Handle;
                            UserData.sMsg = sReviceMsg;
                            UserData.UserClient = userClinet;
                            GateShare.ReviceMsgList.Writer.TryWrite(UserData);
                        }
                    }
                }
                var dwProcessMsgTime = HUtil32.GetTickCount() - dwProcessMsgTick;
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