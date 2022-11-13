using DBSvr.Conf;
using DBSvr.Storage;
using System;
using System.Collections.Generic;
using SystemModule;
using SystemModule.Logger;
using SystemModule.Packets;
using SystemModule.Packets.ServerPackets;
using SystemModule.Sockets;
using SystemModule.Sockets.AsyncSocketServer;

namespace DBSvr.Services
{
    /// <summary>
    /// 玩家数据服务
    /// DBSvr->GameSvr
    /// </summary>
    public class PlayerDataService
    {
        private readonly MirLogger _logger;
        private readonly IList<ServerDataInfo> _serverList;
        private readonly IPlayDataStorage _playDataStorage;
        private readonly ICacheStorage _cacheStorage;
        private readonly SocketServer _serverSocket;
        private readonly LoginSessionServer _loginSvrService;
        private readonly DBSvrConf _conf;
        private IList<THumSession> PlaySessionList { get; set; }

        public PlayerDataService(MirLogger logger, DBSvrConf conf, LoginSessionServer loginService, IPlayDataStorage playDataStorage, ICacheStorage cacheStorage)
        {
            _logger = logger;
            _loginSvrService = loginService;
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
            _logger.LogInformation($"数据库角色服务[{_conf.ServerAddr}:{_conf.ServerPort}]已启动.等待链接...");
        }

        private void ServerSocketClientConnect(object sender, AsyncUserToken e)
        {
            string sIPaddr = e.RemoteIPaddr;
            if (!DBShare.CheckServerIP(sIPaddr))
            {
                _logger.LogWarning("非法服务器连接: " + sIPaddr);
                e.Socket.Close();
                return;
            }
            var serverInfo = new ServerDataInfo();
            serverInfo.Data = null;
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

        private void ServerSocketClientRead(object sender, AsyncUserToken e)
        {
            for (var i = 0; i < _serverList.Count; i++)
            {
                var serverInfo = _serverList[i];
                if (serverInfo.ConnectionId == e.ConnectionId)
                {
                    var nReviceLen = e.BytesReceived;
                    var data = new byte[nReviceLen];
                    Buffer.BlockCopy(e.ReceiveBuffer, e.Offset, data, 0, nReviceLen);
                    if (serverInfo.PacketLen == 0 && data[0] == (byte)'#')
                    {
                        serverInfo.PacketLen = BitConverter.ToInt32(data.AsSpan()[1..5]);
                        serverInfo.Data = new byte[serverInfo.PacketLen];
                        Buffer.BlockCopy(data, 0, serverInfo.Data, 0, nReviceLen);
                    }
                    else if (serverInfo.Data != null && serverInfo.Data.Length > 0)
                    {
                        Buffer.BlockCopy(data, 0, serverInfo.Data, serverInfo.ReviceLen, nReviceLen);
                    }
                    if (serverInfo.Data == null || serverInfo.Data.Length <= 0)
                    {
                        continue;
                    }
                    var len = serverInfo.ReviceLen - serverInfo.PacketLen;
                    if (len > 0)
                    {
                        data = serverInfo.Data[..serverInfo.PacketLen];
                        ProcessServerPacket(serverInfo, data);
                        var dataBuff = serverInfo.Data[len..];
                        if (dataBuff[0] == (byte)'#')
                        {
                            Buffer.BlockCopy(dataBuff, 0, serverInfo.Data, 0, dataBuff.Length);
                            serverInfo.PacketLen = BitConverter.ToInt32(data.AsSpan()[1..5]);
                            serverInfo.ReviceLen += dataBuff.Length;
                            continue;
                        }
                        serverInfo.PacketLen = 0;
                        serverInfo.ReviceLen = 0;
                        break;
                    }
                    if (len == 0)
                    {
                        ProcessServerPacket(serverInfo, serverInfo.Data);
                        serverInfo.PacketLen = 0;
                        serverInfo.ReviceLen = 0;
                        break;
                    }
                    serverInfo.ReviceLen += nReviceLen;
                    break;
                }
            }
        }

        private void ProcessServerPacket(ServerDataInfo serverInfo, byte[] data)
        {
            var nQueryId = 0;
            if (data.Length > 0)
            {
                var requestPacket = Packets.ToPacket<ServerRequestData>(data);
                if (requestPacket == null)
                {
                    return;
                }
                nQueryId = requestPacket.QueryId;
                var packet = ProtoBufDecoder.DeSerialize<ServerRequestMessage>(EDCode.DecodeBuff(requestPacket.Message));
                var packetLen = requestPacket.Message.Length + requestPacket.Packet.Length + 6;
                if (packetLen >= Grobal2.DEFBLOCKSIZE && nQueryId > 0)
                {
                    var queryId = HUtil32.MakeLong((ushort)(nQueryId ^ 170), (ushort)packetLen);
                    if (queryId <= 0)
                    {
                        ProcessServerMsg(nQueryId, packet, requestPacket.Packet, serverInfo.ConnectionId);
                        return;
                    }
                    if (requestPacket.Sgin.Length <= 0)
                    {
                        ProcessServerMsg(nQueryId, packet, requestPacket.Packet, serverInfo.ConnectionId);
                        return;
                    }
                    var signatureBuff = BitConverter.GetBytes(queryId);
                    var signatureId = BitConverter.ToInt16(signatureBuff);
                    var sginBuff = EDCode.DecodeBuff(requestPacket.Sgin);
                    var sgin = BitConverter.ToInt16(sginBuff);
                    if (sgin == signatureId)
                    {
                        ProcessServerMsg(nQueryId, packet, requestPacket.Packet, serverInfo.ConnectionId);
                        return;
                    }
                    _serverSocket.CloseSocket(serverInfo.ConnectionId);
                    _logger.LogWarning("关闭错误的查询请求.");
                    return;
                }
            }
            var responsePack = new ServerRequestData();
            responsePack.QueryId = nQueryId;
            var messagePacket = new ServerRequestMessage(Grobal2.DBR_FAIL, 0, 0, 0, 0);
            responsePack.Message = EDCode.EncodeBuffer(ProtoBufDecoder.Serialize(messagePacket));
            SendRequest(serverInfo.ConnectionId, responsePack);
        }

        private void SendRequest(string connectionId, ServerRequestData requestPacket)
        {
            var queryPart = 0;
            if (requestPacket.Packet != null)
            {
                queryPart = HUtil32.MakeLong((ushort)(requestPacket.QueryId ^ 170), (ushort)(requestPacket.Message.Length + requestPacket.Packet.Length + 6));
            }
            else
            {
                requestPacket.Packet = Array.Empty<byte>();
                queryPart = HUtil32.MakeLong((ushort)(requestPacket.QueryId ^ 170), (ushort)(requestPacket.Message.Length + 6));
            }
            var nCheckCode = BitConverter.GetBytes(queryPart);
            requestPacket.Sgin = EDCode.EncodeBuffer(nCheckCode);
            var pk = requestPacket.GetBuffer();
            _serverSocket.Send(connectionId, pk);
        }

        private void SendRequest<T>(string connectionId, ServerRequestData requestPacket, T packet) where T : class, new()
        {
            if (packet != null)
            {
                requestPacket.Packet = EDCode.EncodeBuffer(ProtoBufDecoder.Serialize(packet));
            }
            var s = HUtil32.MakeLong((ushort)(requestPacket.QueryId ^ 170), (ushort)(requestPacket.Message.Length + requestPacket.Packet.Length + 6));
            requestPacket.Sgin = EDCode.EncodeBuffer(BitConverter.GetBytes(s));
            var pk = requestPacket.GetBuffer();
            _serverSocket.Send(connectionId, pk);
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
            sData = EDCode.DecodeBuff(sData);
            switch (packet.Ident)
            {
                case Grobal2.DB_LOADHUMANRCD:
                    LoadHumanRcd(nQueryId, sData, connectionId);
                    break;
                case Grobal2.DB_SAVEHUMANRCD:
                    SaveHumanRcd(nQueryId, packet.Recog, sData, connectionId);
                    break;
                case Grobal2.DB_SAVEHUMANRCDEX:
                    SaveHumanRcdEx(nQueryId, sData, packet.Recog, connectionId);
                    break;
                default:
                    var responsePack = new ServerRequestData();
                    responsePack.QueryId = nQueryId;
                    var messagePacket = new ServerRequestMessage(Grobal2.DBR_FAIL, 0, 0, 0, 0);
                    responsePack.Message = EDCode.EncodeBuffer(ProtoBufDecoder.Serialize(messagePacket));
                    SendRequest(connectionId, responsePack);
                    break;
            }
        }

        private void LoadHumanRcd(int queryId, byte[] data, string connectionId)
        {
            var loadHumanPacket = ProtoBufDecoder.DeSerialize<LoadPlayerDataMessage>(data);
            if (loadHumanPacket == null)
            {
                return;
            }
            PlayerDataInfo HumanRCD = null;
            bool boFoundSession = false;
            int nCheckCode = -1;
            if ((!string.IsNullOrEmpty(loadHumanPacket.Account)) && (!string.IsNullOrEmpty(loadHumanPacket.ChrName)))
            {
                nCheckCode = _loginSvrService.CheckSessionLoadRcd(loadHumanPacket.Account, loadHumanPacket.UserAddr, loadHumanPacket.SessionID, ref boFoundSession);
                if ((nCheckCode < 0) || !boFoundSession)
                {
                    _logger.LogWarning("[非法请求] " + "帐号: " + loadHumanPacket.Account + " IP: " + loadHumanPacket.UserAddr + " 标识: " + loadHumanPacket.SessionID);
                }
            }
            if ((nCheckCode == 1) || boFoundSession)
            {
                int nIndex = _playDataStorage.Index(loadHumanPacket.ChrName);
                if (nIndex >= 0)
                {
                    HumanRCD = _cacheStorage.Get(loadHumanPacket.ChrName);
                    if (HumanRCD == null)
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
            responsePack.QueryId = queryId;
            if ((nCheckCode == 1) || boFoundSession)
            {
                var loadHumData = new LoadPlayerDataPacket();
                loadHumData.ChrName = EDCode.EncodeString(loadHumanPacket.ChrName);
                loadHumData.HumDataInfo = HumanRCD;
                var messagePacket = new ServerRequestMessage(Grobal2.DBR_LOADHUMANRCD, 1, 0, 0, 1);
                responsePack.Message = EDCode.EncodeBuffer(ProtoBufDecoder.Serialize(messagePacket));
                SendRequest(connectionId, responsePack, loadHumData);
                _logger.DebugLog($"获取玩家[{loadHumanPacket.ChrName}]数据成功");
            }
            else
            {
                var messagePacket = new ServerRequestMessage(Grobal2.DBR_LOADHUMANRCD, nCheckCode, 0, 0, 0);
                responsePack.Message = EDCode.EncodeBuffer(ProtoBufDecoder.Serialize(messagePacket));
                SendRequest(connectionId, responsePack);
            }
        }

        private void SaveHumanRcd(int queryId, int nRecog, byte[] sMsg, string connectionId)
        {
            try
            {
                var saveHumDataPacket = ProtoBufDecoder.DeSerialize<SavePlayerDataMessage>(sMsg);
                if (saveHumDataPacket == null)
                {
                    _logger.LogError("保存玩家数据出错.");
                    return;
                }
                var sUserID = saveHumDataPacket.Account;
                var sChrName = saveHumDataPacket.ChrName;
                var humanRcd = saveHumDataPacket.HumDataInfo;
                bool bo21 = humanRcd == null;
                if (!bo21)
                {
                    bo21 = true;
                    var nIndex = _playDataStorage.Index(sChrName);
                    if (nIndex < 0)
                    {
                        humanRcd.Header.Name = sChrName;
                        _playDataStorage.Add(humanRcd);
                        nIndex = _playDataStorage.Index(sChrName);
                    }
                    if (nIndex >= 0)
                    {
                        humanRcd.Header.Name = sChrName;
                        _cacheStorage.Add(sChrName, humanRcd);
                        _playDataStorage.Update(sChrName, humanRcd);
                        bo21 = false;
                    }
                    _loginSvrService.SetSessionSaveRcd(sUserID);
                }
                var responsePack = new ServerRequestData();
                responsePack.QueryId = queryId;
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
                    var messagePacket = new ServerRequestMessage(Grobal2.DBR_SAVEHUMANRCD, 1, 0, 0, 0);
                    responsePack.Message = EDCode.EncodeBuffer(ProtoBufDecoder.Serialize(messagePacket));
                    SendRequest(connectionId, responsePack);
                }
                else
                {
                    var messagePacket = new ServerRequestMessage(Grobal2.DBR_LOADHUMANRCD, 0, 0, 0, 0);
                    responsePack.Message = EDCode.EncodeBuffer(ProtoBufDecoder.Serialize(messagePacket));
                    SendRequest(connectionId, responsePack);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e);
            }
        }

        private void SaveHumanRcdEx(int nQueryId, byte[] sMsg, int nRecog, string connectionId)
        {
            var saveHumDataPacket = ProtoBufDecoder.DeSerialize<SavePlayerDataMessage>(sMsg);
            if (saveHumDataPacket == null)
            {
                _logger.LogError("保存玩家数据出错.");
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
    }
}