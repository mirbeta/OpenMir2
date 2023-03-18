using System;
using System.Collections.Generic;
using DBSrv.Conf;
using DBSrv.Storage;
using NLog;
using SystemModule;
using SystemModule.Packets.ServerPackets;
using SystemModule.Sockets;
using SystemModule.Sockets.AsyncSocketServer;

namespace DBSrv.Services
{
    /// <summary>
    /// 玩家数据服务
    /// DBSrv->GameSvr
    /// </summary>
    public class PlayerDataService
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly IList<ServerDataInfo> _serverList;
        private readonly IPlayDataStorage _playDataStorage;
        private readonly ICacheStorage _cacheStorage;
        private readonly SocketServer _serverSocket;
        private readonly LoginSessionService _loginService;
        private readonly DbSrvConf _conf;
        private IList<THumSession> PlaySessionList { get; set; }

        public PlayerDataService(DbSrvConf conf, LoginSessionService loginService, IPlayDataStorage playDataStorage, ICacheStorage cacheStorage)
        {
            _loginService = loginService;
            _playDataStorage = playDataStorage;
            _cacheStorage = cacheStorage;
            _serverList = new List<ServerDataInfo>();
            _serverSocket = new SocketServer(byte.MaxValue, 1024);
            _serverSocket.OnClientConnect += ServerSocketClientConnect;
            _serverSocket.OnClientDisconnect += ServerSocketClientDisconnect;
            _serverSocket.OnClientRead += ServerSocketClientRead;
            _serverSocket.OnClientError += ServerSocketClientError;
            _conf = conf;
        }

        public void Start()
        {
            PlaySessionList = new List<THumSession>();
            _serverSocket.Init();
            _serverSocket.Start(_conf.ServerAddr, _conf.ServerPort);
            _playDataStorage.LoadQuickList();
            _logger.Info($"玩家数据存储服务[{_conf.ServerAddr}:{_conf.ServerPort}]已启动.等待链接...");
        }

        private void ServerSocketClientConnect(object sender, AsyncUserToken e)
        {
            string sIPaddr = e.RemoteIPaddr;
            if (!DBShare.CheckServerIP(sIPaddr))
            {
                _logger.Warn("非法服务器连接: " + sIPaddr);
                e.Socket.Close();
                return;
            }
            var serverInfo = new ServerDataInfo();
            serverInfo.Data = new byte[1024 * 20];
            serverInfo.ConnectionId = e.ConnectionId;
            _serverList.Add(serverInfo);
        }

        private void ServerSocketClientDisconnect(object sender, AsyncUserToken e)
        {
            for (var i = 0; i < _serverList.Count; i++)
            {
                var serverInfo = _serverList[i];
                if (serverInfo.ConnectionId == e.ConnectionId)
                {
                    ClearSocket(e.ConnectionId);
                    _serverList.Remove(serverInfo);
                    break;
                }
            }
        }

        private void ServerSocketClientError(object sender, AsyncSocketErrorEventArgs e)
        {

        }

        private void ProcessServerData(byte[] data, int nLen, ref ServerDataInfo serverInfo)
        {
            var srcOffset = 0;
            Span<byte> dataBuff = data;
            try
            {
                while (nLen >= ServerDataPacket.FixedHeaderLen)
                {
                    var packetHead = dataBuff[..ServerDataPacket.FixedHeaderLen];
                    var message = ServerPacket.ToPacket<ServerDataPacket>(packetHead);
                    if (message.PacketCode != Grobal2.RunGateCode)
                    {
                        srcOffset++;
                        dataBuff = dataBuff.Slice(srcOffset, ServerDataPacket.FixedHeaderLen);
                        nLen -= 1;
                        _logger.Error($"解析封包出现异常封包，PacketLen:[{dataBuff.Length}] Offset:[{srcOffset}].");
                        continue;
                    }
                    var nCheckMsgLen = Math.Abs(message.PacketLen + ServerDataPacket.FixedHeaderLen);
                    if (nCheckMsgLen > nLen)
                    {
                        break;
                    }
                    var messageData = SerializerUtil.Deserialize<ServerRequestData>(dataBuff[ServerDataPacket.FixedHeaderLen..]);
                    ProcessMessagePacket(serverInfo, messageData);
                    nLen -= nCheckMsgLen;
                    if (nLen <= 0)
                    {
                        break;
                    }
                    dataBuff = dataBuff.Slice(nCheckMsgLen, nLen);
                    serverInfo.DataLen = nLen;
                    srcOffset = 0;
                    if (nLen < ServerDataPacket.FixedHeaderLen)
                    {
                        break;
                    }
                }
                if (nLen > 0)//有部分数据被处理,需要把剩下的数据拷贝到接收缓冲的头部
                {
                    MemoryCopy.BlockCopy(dataBuff, 0, serverInfo.Data, 0, nLen);
                    serverInfo.DataLen = nLen;
                }
                else
                {
                    serverInfo.DataLen = 0;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }
        
        private void ProcessMessagePacket(ServerDataInfo serverInfo, ServerRequestData requestData)
        {
            int nQueryId = requestData.QueryId;
            var requestMessage = SerializerUtil.Deserialize<ServerRequestMessage>(EDCode.DecodeBuff(requestData.Message));
            var packetLen = requestData.Message.Length + requestData.Packet.Length + 6;
            if (packetLen >= Messages.DefBlockSize && nQueryId > 0 && requestData.Packet != null && requestData.Sgin != null)
            {
                var sData = EDCode.DecodeBuff(requestData.Packet);
                var queryId = HUtil32.MakeLong((ushort)(nQueryId ^ 170), (ushort)packetLen);
                if (queryId <= 0)
                {
                    ProcessServerMsg(nQueryId, requestMessage, sData, serverInfo.ConnectionId);
                    return;
                }
                if (requestData.Sgin.Length <= 0)
                {
                    ProcessServerMsg(nQueryId, requestMessage, sData, serverInfo.ConnectionId);
                    return;
                }
                var signatureBuff = BitConverter.GetBytes(queryId);
                var signatureId = BitConverter.ToInt16(signatureBuff);
                var sginBuff = EDCode.DecodeBuff(requestData.Sgin);
                var sgin = BitConverter.ToInt16(sginBuff);
                if (sgin == signatureId)
                {
                    ProcessServerMsg(nQueryId, requestMessage, sData, serverInfo.ConnectionId);
                    return;
                }
                _serverSocket.CloseSocket(serverInfo.ConnectionId);
                _logger.Error($"关闭错误的任务{nQueryId}查询请求.");
                return;
            }
            var responsePack = new ServerRequestData();
            var messagePacket = new ServerRequestMessage(Messages.DBR_FAIL, 0, 0, 0, 0);
            responsePack.Message = EDCode.EncodeBuffer(SerializerUtil.Serialize(messagePacket));
            SendRequest(serverInfo.ConnectionId, nQueryId, responsePack);
        }

        private void ServerSocketClientRead(object sender, AsyncUserToken e)
        {
            for (var i = 0; i < _serverList.Count; i++)
            {
                var serverInfo = _serverList[i];
                if (serverInfo.ConnectionId == e.ConnectionId)
                {
                    var nMsgLen = e.BytesReceived;
                    if (serverInfo.DataLen > 0)
                    {
                        var packetData = new byte[e.BytesReceived];
                        Buffer.BlockCopy(e.ReceiveBuffer, e.Offset, packetData, 0, nMsgLen);
                        MemoryCopy.BlockCopy(packetData, 0, serverInfo.Data, serverInfo.DataLen, packetData.Length);
                        ProcessServerData(serverInfo.Data, serverInfo.DataLen + nMsgLen, ref serverInfo);
                    }
                    else
                    {
                        Buffer.BlockCopy(e.ReceiveBuffer, e.Offset, serverInfo.Data, 0, nMsgLen);
                        ProcessServerData(serverInfo.Data, nMsgLen, ref serverInfo);
                    }
                }
            }
        }
        
        /// <summary>
        /// 清理超时会话
        /// </summary>
        public void ClearTimeoutSession()
        {
            int i = 0;
            while (true)
            {
                if (PlaySessionList.Count <= i)
                {
                    break;
                }
                THumSession HumSession = PlaySessionList[i];
                if (!HumSession.bo24)
                {
                    if (HumSession.bo2C)
                    {
                        if ((HUtil32.GetTickCount() - HumSession.lastSessionTick) > 20 * 1000)
                        {
                            HumSession = null;
                            PlaySessionList.RemoveAt(i);
                            continue;
                        }
                    }
                    else
                    {
                        if ((HUtil32.GetTickCount() - HumSession.lastSessionTick) > 2 * 60 * 1000)
                        {
                            HumSession = null;
                            PlaySessionList.RemoveAt(i);
                            continue;
                        }
                    }
                }
                if ((HUtil32.GetTickCount() - HumSession.lastSessionTick) > 40 * 60 * 1000)
                {
                    HumSession = null;
                    PlaySessionList.RemoveAt(i);
                    continue;
                }
                i++;
            }
        }

        private void ProcessServerMsg(int nQueryId, ServerRequestMessage packet, byte[] sData, string connectionId)
        {
            switch (packet.Ident)
            {
                case Messages.DB_LOADHUMANRCD:
                    LoadHumanRcd(nQueryId, sData, connectionId);
                    break;
                case Messages.DB_SAVEHUMANRCD:
                    SaveHumanRcd(nQueryId, packet.Recog, sData, connectionId);
                    break;
                case Messages.DB_SAVEHUMANRCDEX:
                    SaveHumanRcdEx(nQueryId, sData, packet.Recog, connectionId);
                    break;
                default:
                    var responsePack = new ServerRequestData();
                    var messagePacket = new ServerRequestMessage(Messages.DBR_FAIL, 0, 0, 0, 0);
                    responsePack.Message = EDCode.EncodeBuffer(SerializerUtil.Serialize(messagePacket));
                    SendRequest(connectionId, nQueryId, responsePack);
                    break;
            }
        }

        private void LoadHumanRcd(int queryId, byte[] data, string connectionId)
        {
            var loadHumanPacket = SerializerUtil.Deserialize<LoadPlayerDataMessage>(data);
            if (loadHumanPacket.SessionID <= 0)
            {
                return;
            }
            PlayerDataInfo HumanRCD = null;
            bool boFoundSession = false;
            int nCheckCode = -1;
            if ((!string.IsNullOrEmpty(loadHumanPacket.Account)) && (!string.IsNullOrEmpty(loadHumanPacket.ChrName)))
            {
                nCheckCode = _loginService.CheckSessionLoadRcd(loadHumanPacket.Account, loadHumanPacket.UserAddr, loadHumanPacket.SessionID, ref boFoundSession);
                if ((nCheckCode < 0) || !boFoundSession)
                {
                    _logger.Warn("[非法请求] " + "帐号: " + loadHumanPacket.Account + " IP: " + loadHumanPacket.UserAddr + " 标识: " + loadHumanPacket.SessionID);
                }
            }
            if ((nCheckCode == 1) || boFoundSession)
            {
                int nIndex = _playDataStorage.Index(loadHumanPacket.ChrName);
                if (nIndex >= 0)
                {
                    HumanRCD = _cacheStorage.Get(loadHumanPacket.ChrName, out bool isExist);
                    if (!isExist)
                    {
                        if (!_playDataStorage.Get(loadHumanPacket.ChrName, ref HumanRCD))
                        {
                            nCheckCode = -2;
                        }
                    }
                }
                else
                {
                    nCheckCode = -3;
                }
            }
            var responsePack = new ServerRequestData();
            if ((nCheckCode == 1) || boFoundSession)
            {
                var loadHumData = new LoadPlayerDataPacket();
                loadHumData.ChrName = EDCode.EncodeString(loadHumanPacket.ChrName);
                loadHumData.HumDataInfo = HumanRCD;
                var messagePacket = new ServerRequestMessage(Messages.DBR_LOADHUMANRCD, 1, 0, 0, 1);
                responsePack.Message = EDCode.EncodeBuffer(SerializerUtil.Serialize(messagePacket));
                SendRequest(connectionId, queryId, responsePack, loadHumData);
                _logger.Debug($"获取玩家[{loadHumanPacket.ChrName}]数据成功");
            }
            else
            {
                var messagePacket = new ServerRequestMessage(Messages.DBR_LOADHUMANRCD, nCheckCode, 0, 0, 0);
                responsePack.Message = EDCode.EncodeBuffer(SerializerUtil.Serialize(messagePacket));
                SendRequest(connectionId, queryId, responsePack);
            }
        }

        private void SaveHumanRcd(int queryId, int nRecog, byte[] sMsg, string connectionId)
        {
            try
            {
                var saveHumDataPacket = SerializerUtil.Deserialize<SavePlayerDataMessage>(sMsg);
                if (saveHumDataPacket == null)
                {
                    _logger.Error("保存玩家数据出错.");
                    return;
                }
                var sUserID = saveHumDataPacket.Account;
                var sChrName = saveHumDataPacket.ChrName;
                var humanRcd = saveHumDataPacket.HumDataInfo;
                bool bo21 = humanRcd == null;
                if (!bo21)
                {
                    bo21 = true;
                    humanRcd.Header.SetName(sChrName);
                    var nIndex = _playDataStorage.Index(sChrName);
                    if (nIndex < 0)
                    {
                        _playDataStorage.Add(humanRcd);
                        nIndex = _playDataStorage.Index(sChrName);
                    }
                    if (nIndex >= 0)
                    {
                        _cacheStorage.Add(sChrName, humanRcd);
                        _playDataStorage.Update(sChrName, humanRcd);
                        bo21 = false;
                    }
                    _loginService.SetSessionSaveRcd(sUserID);
                }
                var responsePack = new ServerRequestData();
                if (!bo21)
                {
                    for (var i = 0; i < PlaySessionList.Count; i++)
                    {
                        THumSession HumSession = PlaySessionList[i];
                        if ((HumSession.sChrName == sChrName) && (HumSession.nIndex == nRecog))
                        {
                            HumSession.lastSessionTick = HUtil32.GetTickCount();
                            break;
                        }
                    }
                    var messagePacket = new ServerRequestMessage(Messages.DBR_SAVEHUMANRCD, 1, 0, 0, 0);
                    responsePack.Message = EDCode.EncodeBuffer(SerializerUtil.Serialize(messagePacket));
                    SendRequest(connectionId, queryId, responsePack);
                }
                else
                {
                    var messagePacket = new ServerRequestMessage(Messages.DBR_LOADHUMANRCD, 0, 0, 0, 0);
                    responsePack.Message = EDCode.EncodeBuffer(SerializerUtil.Serialize(messagePacket));
                    SendRequest(connectionId, queryId, responsePack);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e);
            }
        }

        private void SaveHumanRcdEx(int nQueryId, byte[] sMsg, int nRecog, string connectionId)
        {
            var saveHumDataPacket = SerializerUtil.Deserialize<SavePlayerDataMessage>(sMsg);
            if (saveHumDataPacket == null)
            {
                _logger.Error("保存玩家数据出错.");
                return;
            }
            var sChrName = saveHumDataPacket.ChrName;
            for (var i = 0; i < PlaySessionList.Count; i++)
            {
                THumSession HumSession = PlaySessionList[i];
                if ((HumSession.sChrName == sChrName) && (HumSession.nIndex == nRecog))
                {
                    HumSession.bo24 = false;
                    HumSession.ConnectionId = connectionId;
                    HumSession.bo2C = true;
                    HumSession.lastSessionTick = HUtil32.GetTickCount();
                    break;
                }
            }
            SaveHumanRcd(nQueryId, nRecog, sMsg, connectionId);
        }

        private void ClearSocket(string connectionId)
        {
            THumSession HumSession;
            int nIndex = 0;
            while (true)
            {
                if (PlaySessionList.Count <= nIndex)
                {
                    break;
                }
                HumSession = PlaySessionList[nIndex];
                if (HumSession.ConnectionId == connectionId)
                {
                    HumSession = null;
                    PlaySessionList.RemoveAt(nIndex);
                    continue;
                }
                nIndex++;
            }
        }
           
        private void SendRequest(string connectionId, int queryId, ServerRequestData requestPacket)
        {
            requestPacket.QueryId = queryId;
            var queryPart = 0;
            if (requestPacket.Packet != null)
            {
                queryPart = HUtil32.MakeLong((ushort)(queryId ^ 170), (ushort)(requestPacket.Message.Length + requestPacket.Packet.Length + 6));
            }
            else
            {
                requestPacket.Packet = Array.Empty<byte>();
                queryPart = HUtil32.MakeLong((ushort)(queryId ^ 170), (ushort)(requestPacket.Message.Length + 6));
            }
            var nCheckCode = BitConverter.GetBytes(queryPart);
            requestPacket.Sgin = EDCode.EncodeBuffer(nCheckCode);
            SendMessage(connectionId, SerializerUtil.Serialize(requestPacket));
        }

        private void SendRequest<T>(string connectionId, int queryId, ServerRequestData requestPacket, T packet) where T : new()
        {
            requestPacket.QueryId = queryId;
            if (packet != null)
            {
                requestPacket.Packet = EDCode.EncodeBuffer(SerializerUtil.Serialize(packet));
            }
            var s = HUtil32.MakeLong((ushort)(queryId ^ 170), (ushort)(requestPacket.Message.Length + requestPacket.Packet.Length + 6));
            requestPacket.Sgin = EDCode.EncodeBuffer(BitConverter.GetBytes(s));
            SendMessage(connectionId, SerializerUtil.Serialize(requestPacket));
        }

        private void SendMessage(string connectionId, byte[] sendBuffer)
        {
            var serverMessage = new ServerDataPacket
            {
                PacketCode = Grobal2.RunGateCode,
                PacketLen = (ushort)sendBuffer.Length
            };
            var dataBuff = serverMessage.GetBuffer();
            var data = new byte[ServerDataPacket.FixedHeaderLen + sendBuffer.Length];
            MemoryCopy.BlockCopy(dataBuff, 0, data, 0, data.Length);
            MemoryCopy.BlockCopy(sendBuffer, 0, data, dataBuff.Length, sendBuffer.Length);
            _serverSocket.Send(connectionId, data);
        }
    }
}