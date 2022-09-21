using DBSvr.Conf;
using DBSvr.Storage;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using SystemModule;
using SystemModule.Packet;
using SystemModule.Packet.ServerPackets;
using SystemModule.Sockets;
using SystemModule.Sockets.AsyncSocketServer;

namespace DBSvr.Services
{
    /// <summary>
    /// 玩家数据查询保存
    /// DBSvr->GameSvr
    /// </summary>
    public class HumDataService
    {
        private readonly MirLog _logger;
        private readonly IList<TServerInfo> _serverList;
        private readonly IList<THumSession> _playSessionList;
        private readonly IPlayDataStorage _playDataStorage;
        private readonly SocketServer _serverSocket;
        private readonly LoginSvrService _loginSvrService;
        private readonly SvrConf _conf;

        public HumDataService(MirLog logger, LoginSvrService loginSvrService, IPlayDataStorage playDataStorage, SvrConf conf)
        {
            _logger = logger;
            _loginSvrService = loginSvrService;
            _playDataStorage = playDataStorage;
            _serverList = new List<TServerInfo>();
            _playSessionList = new List<THumSession>();
            _serverSocket = new SocketServer(byte.MaxValue, 1024);
            _serverSocket.OnClientConnect += ServerSocketClientConnect;
            _serverSocket.OnClientDisconnect += ServerSocketClientDisconnect;
            _serverSocket.OnClientRead += ServerSocketClientRead;
            _serverSocket.OnClientError += ServerSocketClientError;
            _conf = conf;
        }

        public void Start()
        {
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
            var serverInfo = new TServerInfo();
            serverInfo.nSckHandle = (int)e.Socket.Handle;
            serverInfo.Data = null;
            serverInfo.Socket = e.Socket;
            _serverList.Add(serverInfo);
        }

        private void ServerSocketClientDisconnect(object sender, AsyncUserToken e)
        {
            for (var i = 0; i < _serverList.Count; i++)
            {
                if (_serverList[i].nSckHandle == (int)e.Socket.Handle)
                {
                    _serverList[i] = null;
                    _serverList.RemoveAt(i);
                    ClearSocket(e.Socket);
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
                if (serverInfo.nSckHandle == (int)e.Socket.Handle)
                {
                    var nReviceLen = e.BytesReceived;
                    var data = new byte[nReviceLen];
                    Buffer.BlockCopy(e.ReceiveBuffer, e.Offset, data, 0, nReviceLen);
                    if (serverInfo.DataLen == 0 && data[0] == (byte)'#')
                    {
                        serverInfo.DataLen = BitConverter.ToInt32(data[1..5]);
                    }
                    if (serverInfo.Data != null && serverInfo.Data.Length > 0)
                    {
                        var tempBuff = new byte[serverInfo.Data.Length + nReviceLen];
                        Buffer.BlockCopy(serverInfo.Data, 0, tempBuff, 0, serverInfo.Data.Length);
                        Buffer.BlockCopy(data, 0, tempBuff, serverInfo.Data.Length, data.Length);
                        serverInfo.Data = tempBuff;
                    }
                    else
                    {
                        serverInfo.Data = data;
                    }
                    var len = serverInfo.Data.Length - serverInfo.DataLen;
                    if (len > 0)
                    {
                        var tempBuff = new byte[len];
                        Buffer.BlockCopy(serverInfo.Data, serverInfo.DataLen, tempBuff, 0, len);
                        data = serverInfo.Data[..serverInfo.DataLen];
                        serverInfo.Data = tempBuff;
                        ProcessServerPacket(serverInfo, data);
                        serverInfo.DataLen = tempBuff.Length;
                    }
                    else if (len == 0)
                    {
                        ProcessServerPacket(serverInfo, serverInfo.Data);
                        serverInfo.Data = null;
                        serverInfo.DataLen = 0;
                    }
                    break;
                }
            }
        }

        private void ProcessServerPacket(TServerInfo serverInfo, byte[] data)
        {
            var nQueryId = 0;
            if (data.Length > 0)
            {
                var requestPacket = Packets.ToPacket<RequestServerPacket>(data);
                if (requestPacket == null)
                {
                    return;
                }
                nQueryId = requestPacket.QueryId;
                var packet = ProtoBufDecoder.DeSerialize<ServerMessagePacket>(EDCode.DecodeBuff(requestPacket.Message));
                var packetLen = requestPacket.Message.Length + requestPacket.Packet.Length + 6;
                if (packetLen >= Grobal2.DEFBLOCKSIZE && nQueryId > 0)
                {
                    var queryId = HUtil32.MakeLong(nQueryId ^ 170, packetLen);
                    var sginBuff = EDCode.DecodeBuff(requestPacket.Sgin);
                    var sgin = BitConverter.ToInt16(sginBuff);
                    if (sgin == queryId)
                    {
                        ProcessServerMsg(nQueryId, packet, requestPacket.Packet, serverInfo.Socket);
                        return;
                    }
                    serverInfo.Socket.Close();
                    Console.WriteLine("关闭错误的查询请求.");
                    return;
                }
            }
            var responsePack = new RequestServerPacket();
            responsePack.QueryId = nQueryId;
            var messagePacket = new ServerMessagePacket(Grobal2.DBR_FAIL, 0, 0, 0, 0);
            responsePack.Message = EDCode.EncodeBuffer(ProtoBufDecoder.Serialize(messagePacket));
            SendRequest(serverInfo.Socket, responsePack);
            serverInfo.Data = null;
        }

        private void SendRequest(Socket socket, RequestServerPacket requestPacket)
        {
            var queryPart = 0;
            if (requestPacket.Packet != null)
            {
                queryPart = HUtil32.MakeLong(requestPacket.QueryId ^ 170, requestPacket.Message.Length + requestPacket.Packet.Length + 6);
            }
            else
            {
                requestPacket.Packet = Array.Empty<byte>();
                queryPart = HUtil32.MakeLong(requestPacket.QueryId ^ 170, requestPacket.Message.Length + 6);
            }
            var nCheckCode = BitConverter.GetBytes(queryPart);
            requestPacket.Sgin = EDCode.EncodeBuffer(nCheckCode);
            var pk = requestPacket.GetBuffer();
            socket.Send(pk, pk.Length, SocketFlags.None);
        }

        private void SendRequest<T>(Socket socket, RequestServerPacket requestPacket, T packet) where T : class, new()
        {
            if (packet != null)
            {
                requestPacket.Packet = EDCode.EncodeBuffer(ProtoBufDecoder.Serialize(packet));
            }
            var s = HUtil32.MakeLong(requestPacket.QueryId ^ 170, requestPacket.Message.Length + requestPacket.Packet.Length + 6);
            requestPacket.Sgin = EDCode.EncodeBuffer(BitConverter.GetBytes(s));
            var pk = requestPacket.GetBuffer();
            socket.Send(pk, pk.Length, SocketFlags.None);
        }

        /// <summary>
        /// 清理超时会话
        /// </summary>
        public void ClearTimeoutSession()
        {
            int i = 0;
            while (true)
            {
                if (_playSessionList.Count <= i)
                {
                    break;
                }
                THumSession HumSession = _playSessionList[i];
                if (!HumSession.bo24)
                {
                    if (HumSession.bo2C)
                    {
                        if ((HUtil32.GetTickCount() - HumSession.lastSessionTick) > 20 * 1000)
                        {
                            HumSession = null;
                            _playSessionList.RemoveAt(i);
                            continue;
                        }
                    }
                    else
                    {
                        if ((HUtil32.GetTickCount() - HumSession.lastSessionTick) > 2 * 60 * 1000)
                        {
                            HumSession = null;
                            _playSessionList.RemoveAt(i);
                            continue;
                        }
                    }
                }
                if ((HUtil32.GetTickCount() - HumSession.lastSessionTick) > 40 * 60 * 1000)
                {
                    HumSession = null;
                    _playSessionList.RemoveAt(i);
                    continue;
                }
                i++;
            }
        }

        public bool CopyHumData(string sSrcChrName, string sDestChrName, string sUserID)
        {
            bool result = false;
            bool find = false;
            try
            {
                int chrIndex = _playDataStorage.Index(sSrcChrName);
                THumDataInfo HumanRCD = null;
                if ((chrIndex >= 0) && (_playDataStorage.Get(sSrcChrName, ref HumanRCD)))
                {
                    find = true;
                }
                if (find)
                {
                    chrIndex = _playDataStorage.Index(sDestChrName);
                    if ((chrIndex >= 0))
                    {
                        HumanRCD.Header.sName = sDestChrName;
                        HumanRCD.Data.sCharName = sDestChrName;
                        HumanRCD.Data.sAccount = sUserID;
                        _playDataStorage.Update(chrIndex, ref HumanRCD);
                        result = true;
                    }
                }
            }
            finally
            {

            }
            return result;
        }

        private void ProcessServerMsg(int nQueryId, ServerMessagePacket packet, byte[] sData, Socket Socket)
        {
            sData = EDCode.DecodeBuff(sData);
            switch (packet.Ident)
            {
                case Grobal2.DB_LOADHUMANRCD:
                    LoadHumanRcd(nQueryId, sData, Socket);
                    break;
                case Grobal2.DB_SAVEHUMANRCD:
                    SaveHumanRcd(nQueryId, packet.Recog, sData, Socket);
                    break;
                case Grobal2.DB_SAVEHUMANRCDEX:
                    SaveHumanRcdEx(nQueryId, sData, packet.Recog, Socket);
                    break;
                default:
                    var responsePack = new RequestServerPacket();
                    responsePack.QueryId = nQueryId;
                    var messagePacket = new ServerMessagePacket(Grobal2.DBR_FAIL, 0, 0, 0, 0);
                    responsePack.Message = EDCode.EncodeBuffer(ProtoBufDecoder.Serialize(messagePacket));
                    SendRequest(Socket, responsePack);
                    break;
            }
        }

        private void LoadHumanRcd(int queryId, byte[] data, Socket Socket)
        {
            var loadHumanPacket = ProtoBufDecoder.DeSerialize<LoadHumDataPacket>(data);
            if (loadHumanPacket == null)
            {
                return;
            }
            THumDataInfo HumanRCD = null;
            bool boFoundSession = false;
            int nCheckCode = -1;
            if ((!string.IsNullOrEmpty(loadHumanPacket.sAccount)) && (!string.IsNullOrEmpty(loadHumanPacket.sChrName)))
            {
                nCheckCode = _loginSvrService.CheckSessionLoadRcd(loadHumanPacket.sAccount, loadHumanPacket.sUserAddr, loadHumanPacket.nSessionID, ref boFoundSession);
                if ((nCheckCode < 0) || !boFoundSession)
                {
                    _logger.LogWarning("[非法请求] " + "帐号: " + loadHumanPacket.sAccount + " IP: " + loadHumanPacket.sUserAddr + " 标识: " + loadHumanPacket.nSessionID);
                }
            }
            if ((nCheckCode == 1) || boFoundSession)
            {
                int nIndex = _playDataStorage.Index(loadHumanPacket.sChrName);
                if (nIndex >= 0)
                {
                    if (!_playDataStorage.Get(loadHumanPacket.sChrName, ref HumanRCD))
                    {
                        nCheckCode = -2;
                    }
                }
                else
                {
                    nCheckCode = -3;
                }
            }
            var responsePack = new RequestServerPacket();
            responsePack.QueryId = queryId;
            if ((nCheckCode == 1) || boFoundSession)
            {
                var loadHumData = new LoadHumanRcdResponsePacket();
                loadHumData.sChrName = EDCode.EncodeString(loadHumanPacket.sChrName);
                loadHumData.HumDataInfo = HumanRCD;
                var messagePacket = new ServerMessagePacket(Grobal2.DBR_LOADHUMANRCD, 1, 0, 0, 1);
                responsePack.Message = EDCode.EncodeBuffer(ProtoBufDecoder.Serialize(messagePacket));
                SendRequest(Socket, responsePack, loadHumData);
            }
            else
            {
                var messagePacket = new ServerMessagePacket(Grobal2.DBR_LOADHUMANRCD, nCheckCode, 0, 0, 0);
                responsePack.Message = EDCode.EncodeBuffer(ProtoBufDecoder.Serialize(messagePacket));
                SendRequest(Socket, responsePack);
            }
        }

        private void SaveHumanRcd(int queryId, int nRecog, byte[] sMsg, Socket Socket)
        {
            var saveHumDataPacket = ProtoBufDecoder.DeSerialize<SaveHumDataPacket>(sMsg);
            if (saveHumDataPacket == null)
            {
                Console.WriteLine("保存玩家数据出错.");
                return;
            }
            var sUserID = saveHumDataPacket.sAccount;
            var sChrName = saveHumDataPacket.sCharName;
            var humanRcd = saveHumDataPacket.HumDataInfo;
            bool bo21 = humanRcd == null;
            if (!bo21)
            {
                bo21 = true;
                int nIndex = _playDataStorage.Index(sChrName);
                if (nIndex < 0)
                {
                    humanRcd.Header.sName = sChrName;
                    _playDataStorage.Add(ref humanRcd);
                    nIndex = _playDataStorage.Index(sChrName);
                }
                if (nIndex >= 0)
                {
                    humanRcd.Header.sName = sChrName;
                    _playDataStorage.Update(nIndex, ref humanRcd);
                    bo21 = false;
                }
                _loginSvrService.SetSessionSaveRcd(sUserID);
            }
            var responsePack = new RequestServerPacket();
            responsePack.QueryId = queryId;
            if (!bo21)
            {
                for (var i = 0; i < _playSessionList.Count; i++)
                {
                    THumSession HumSession = _playSessionList[i];
                    if ((HumSession.sChrName == sChrName) && (HumSession.nIndex == nRecog))
                    {
                        HumSession.lastSessionTick = HUtil32.GetTickCount();
                        break;
                    }
                }
                var messagePacket = new ServerMessagePacket(Grobal2.DBR_SAVEHUMANRCD, 1, 0, 0, 0);
                responsePack.Message = EDCode.EncodeBuffer(ProtoBufDecoder.Serialize(messagePacket));
                SendRequest(Socket, responsePack);
            }
            else
            {
                var messagePacket = new ServerMessagePacket(Grobal2.DBR_LOADHUMANRCD, 0, 0, 0, 0);
                responsePack.Message = EDCode.EncodeBuffer(ProtoBufDecoder.Serialize(messagePacket));
                SendRequest(Socket, responsePack);
            }
        }

        private void SaveHumanRcdEx(int nQueryId, byte[] sMsg, int nRecog, Socket Socket)
        {
            var saveHumDataPacket = ProtoBufDecoder.DeSerialize<SaveHumDataPacket>(sMsg);
            if (saveHumDataPacket == null)
            {
                Console.WriteLine("保存玩家数据出错.");
                return;
            }
            var sChrName = saveHumDataPacket.sCharName;
            for (var i = 0; i < _playSessionList.Count; i++)
            {
                THumSession HumSession = _playSessionList[i];
                if ((HumSession.sChrName == sChrName) && (HumSession.nIndex == nRecog))
                {
                    HumSession.bo24 = false;
                    HumSession.Socket = Socket;
                    HumSession.bo2C = true;
                    HumSession.lastSessionTick = HUtil32.GetTickCount();
                    break;
                }
            }
            SaveHumanRcd(nQueryId, nRecog, sMsg, Socket);
        }

        private void ClearSocket(Socket Socket)
        {
            THumSession HumSession;
            int nIndex = 0;
            while (true)
            {
                if (_playSessionList.Count <= nIndex)
                {
                    break;
                }
                HumSession = _playSessionList[nIndex];
                if (HumSession.Socket == Socket)
                {
                    HumSession = null;
                    _playSessionList.RemoveAt(nIndex);
                    continue;
                }
                nIndex++;
            }
        }
    }
}