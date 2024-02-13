using DBSrv.Conf;
using DBSrv.Storage;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using OpenMir2;
using OpenMir2.DataHandlingAdapters;
using OpenMir2.Packets.ServerPackets;
using TouchSocket.Core;
using TouchSocket.Sockets;

namespace DBSrv.Services.Impl
{
    /// <summary>
    /// 玩家数据服务
    /// DBSrv->GameSvr
    /// </summary>
    public class DataService : IService
    {
        private readonly IPlayDataStorage _playDataStorage;
        private readonly ICacheStorage _cacheStorage;
        private readonly TcpService _serverSocket;
        private readonly ClientSession _loginService;
        private readonly SettingConf _setting;
        private IList<THumSession> PlaySessionList { get; set; }

        public DataService(SettingConf conf, ClientSession loginService, IPlayDataStorage playDataStorage, ICacheStorage cacheStorage)
        {
            _setting = conf;
            _loginService = loginService;
            _playDataStorage = playDataStorage;
            _cacheStorage = cacheStorage;
            _serverSocket = new TcpService();
            _serverSocket.Connected += Connecting;
            _serverSocket.Disconnected += Disconnected;
            _serverSocket.Received += Received;
            PlaySessionList = new List<THumSession>();
        }

        public void Initialize()
        {
            var touchSocketConfig = new TouchSocketConfig();
            touchSocketConfig.SetListenIPHosts(new IPHost[1]
            {
                new IPHost(IPAddress.Parse(_setting.ServerAddr), _setting.ServerPort)
            }).SetTcpDataHandlingAdapter(() => new PlayerDataFixedHeaderDataHandlingAdapter());
            _serverSocket.Setup(touchSocketConfig);
        }

        public void Start()
        {
            _serverSocket.Start();
            _playDataStorage.LoadQuickList();
            LogService.Info($"玩家数据存储服务[{_setting.ServerAddr}:{_setting.ServerPort}]已启动.等待链接...");
        }

        public void Stop()
        {
            _serverSocket.Stop();
        }

        private Task Received(IClient client, ReceivedDataEventArgs e)
        {
            if (e.RequestInfo is not PlayerDataMessageFixedHeaderRequestInfo fixedHeader)
                return Task.CompletedTask;
            if (fixedHeader.Header.PacketCode != Grobal2.PacketCode)
            {
                LogService.Error("验证玩家数据封包头出现异常...");
                return Task.CompletedTask;
            }
            var clientSoc = (SocketClient)client;
            var messageData = SerializerUtil.Deserialize<ServerRequestData>(fixedHeader.Message);
            ProcessMessagePacket(clientSoc.Id, messageData);
            return Task.CompletedTask;
        }

        private Task Connecting(IClient client, ConnectedEventArgs e)
        {
            var clientSoc = (SocketClient)client;
            var remoteIp = clientSoc.MainSocket.RemoteEndPoint.GetIP();
            if (!DBShare.CheckServerIP(remoteIp))
            {
                LogService.Warn("非法服务器连接: " + remoteIp);
                clientSoc.Close();
            }
            LogService.Info("服务器连接: " + remoteIp);
            return Task.CompletedTask;
        }

        private Task Disconnected(IClient client, DisconnectEventArgs e)
        {
            var clientSoc = (SocketClient)client;
            ClearSocket(clientSoc.Id);
            return Task.CompletedTask;
        }

        private void ProcessMessagePacket(string connectionId, ServerRequestData requestData)
        {
            var nQueryId = requestData.QueryId;
            var requestMessage = SerializerUtil.Deserialize<ServerRequestMessage>(EDCode.DecodeBuff(requestData.Message));
            var packetLen = requestData.Message.Length + requestData.Packet.Length + ServerDataPacket.FixedHeaderLen;
            if (packetLen >= Messages.DefBlockSize && nQueryId > 0 && requestData.Packet != null && requestData.Sign != null)
            {
                var sData = EDCode.DecodeBuff(requestData.Packet);
                var queryId = HUtil32.MakeLong((ushort)(nQueryId ^ 170), (ushort)packetLen);
                if (queryId <= 0)
                {
                    ProcessServerMsg(nQueryId, requestMessage, sData, connectionId);
                    return;
                }
                if (requestData.Sign.Length <= 0)
                {
                    ProcessServerMsg(nQueryId, requestMessage, sData, connectionId);
                    return;
                }
                var signatureBuff = BitConverter.GetBytes(queryId);
                var signatureId = BitConverter.ToInt16(signatureBuff);
                var signBuff = EDCode.DecodeBuff(requestData.Sign);
                var signId = BitConverter.ToInt16(signBuff);
                if (signId == signatureId)
                {
                    ProcessServerMsg(nQueryId, requestMessage, sData, connectionId);
                    return;
                }
                if (_serverSocket.TryGetSocketClient(connectionId, out var client))
                {
                    client.Close();
                }
                LogService.Error($"关闭错误的任务{nQueryId}查询请求.");
                return;
            }
            var responsePack = new ServerRequestData();
            var messagePacket = new ServerRequestMessage(Messages.DBR_FAIL, 0, 0, 0, 0);
            responsePack.Message = EDCode.EncodeBuffer(SerializerUtil.Serialize(messagePacket));
            SendRequest(connectionId, nQueryId, responsePack);
        }

        /// <summary>
        /// 清理超时会话
        /// </summary>
        public void ClearTimeoutSession()
        {
            var i = 0;
            while (true)
            {
                if (PlaySessionList.Count <= i)
                {
                    break;
                }
                var humSession = PlaySessionList[i];
                if (!humSession.bo24)
                {
                    if (humSession.bo2C)
                    {
                        if ((HUtil32.GetTickCount() - humSession.lastSessionTick) > 20 * 1000)
                        {
                            humSession = null;
                            PlaySessionList.RemoveAt(i);
                            continue;
                        }
                    }
                    else
                    {
                        if ((HUtil32.GetTickCount() - humSession.lastSessionTick) > 2 * 60 * 1000)
                        {
                            humSession = null;
                            PlaySessionList.RemoveAt(i);
                            continue;
                        }
                    }
                }
                if ((HUtil32.GetTickCount() - humSession.lastSessionTick) > 40 * 60 * 1000)
                {
                    humSession = null;
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
            var loadHumanPacket = SerializerUtil.Deserialize<LoadCharacterData>(data);
            if (loadHumanPacket.SessionID <= 0)
            {
                return;
            }
            CharacterDataInfo humanRcd = null;
            var boFoundSession = false;
            var nCheckCode = -1;
            if ((!string.IsNullOrEmpty(loadHumanPacket.Account)) && (!string.IsNullOrEmpty(loadHumanPacket.ChrName)))
            {
                nCheckCode = _loginService.CheckSessionLoadRcd(loadHumanPacket.Account, loadHumanPacket.UserAddr, loadHumanPacket.SessionID, ref boFoundSession);
                if ((nCheckCode < 0) || !boFoundSession)
                {
                    LogService.Warn("[非法请求] " + "帐号: " + loadHumanPacket.Account + " IP: " + loadHumanPacket.UserAddr + " 标识: " + loadHumanPacket.SessionID);
                }
            }
            if ((nCheckCode == 1) || boFoundSession)
            {
                var nIndex = _playDataStorage.Index(loadHumanPacket.ChrName);
                if (nIndex >= 0)
                {
                    humanRcd = _cacheStorage.Get(loadHumanPacket.ChrName, out var isExist);
                    if (!isExist)
                    {
                        if (!_playDataStorage.Get(loadHumanPacket.ChrName, ref humanRcd))
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
                loadHumData.HumDataInfo = humanRcd;
                var messagePacket = new ServerRequestMessage(Messages.DBR_LOADHUMANRCD, 1, 0, 0, 1);
                responsePack.Message = EDCode.EncodeBuffer(SerializerUtil.Serialize(messagePacket));
                SendRequest(connectionId, queryId, responsePack, loadHumData);
                LogService.Debug($"获取玩家[{loadHumanPacket.ChrName}]数据成功");
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
                var saveHumDataPacket = SerializerUtil.Deserialize<SaveCharacterData>(sMsg);
                if (saveHumDataPacket == null)
                {
                    LogService.Error("保存玩家数据出错.");
                    return;
                }
                var sUserId = saveHumDataPacket.Account;
                var sChrName = saveHumDataPacket.ChrName;
                var humanRcd = saveHumDataPacket.CharacterData;
                var bo21 = humanRcd == null;
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
                    _loginService.SetSessionSaveRcd(sUserId);
                }
                var responsePack = new ServerRequestData();
                if (!bo21)
                {
                    for (var i = 0; i < PlaySessionList.Count; i++)
                    {
                        var humSession = PlaySessionList[i];
                        if ((string.Compare(humSession.sChrName, sChrName, StringComparison.OrdinalIgnoreCase) == 0) && (humSession.nIndex == nRecog))
                        {
                            humSession.lastSessionTick = HUtil32.GetTickCount();
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
                LogService.Error(e);
            }
        }

        private void SaveHumanRcdEx(int nQueryId, byte[] sMsg, int nRecog, string connectionId)
        {
            var saveHumDataPacket = SerializerUtil.Deserialize<SaveCharacterData>(sMsg);
            if (saveHumDataPacket == null)
            {
                LogService.Error("保存玩家数据出错.");
                return;
            }
            var sChrName = saveHumDataPacket.ChrName;
            for (var i = 0; i < PlaySessionList.Count; i++)
            {
                var humSession = PlaySessionList[i];
                if ((string.Compare(humSession.sChrName, sChrName, StringComparison.OrdinalIgnoreCase) == 0) && (humSession.nIndex == nRecog))
                {
                    humSession.bo24 = false;
                    humSession.ConnectionId = connectionId;
                    humSession.bo2C = true;
                    humSession.lastSessionTick = HUtil32.GetTickCount();
                    break;
                }
            }
            SaveHumanRcd(nQueryId, nRecog, sMsg, connectionId);
        }

        private void ClearSocket(string connectionId)
        {
            var nIndex = 0;
            while (true)
            {
                if (PlaySessionList.Count <= nIndex)
                {
                    break;
                }
                var humSession = PlaySessionList[nIndex];
                if (humSession.ConnectionId == connectionId)
                {
                    humSession = null;
                    PlaySessionList.RemoveAt(nIndex);
                    continue;
                }
                nIndex++;
            }
        }

        private void SendRequest(string connectionId, int queryId, ServerRequestData requestPacket)
        {
            requestPacket.QueryId = queryId;
            int queryPart;
            if (requestPacket.Packet != null)
            {
                queryPart = HUtil32.MakeLong((ushort)(queryId ^ 170), (ushort)(requestPacket.Message.Length + requestPacket.Packet.Length + ServerDataPacket.FixedHeaderLen));
            }
            else
            {
                requestPacket.Packet = Array.Empty<byte>();
                queryPart = HUtil32.MakeLong((ushort)(queryId ^ 170), (ushort)(requestPacket.Message.Length + ServerDataPacket.FixedHeaderLen));
            }
            var nCheckCode = BitConverter.GetBytes(queryPart);
            requestPacket.Sign = EDCode.EncodeBuffer(nCheckCode);
            SendMessage(connectionId, SerializerUtil.Serialize(requestPacket));
        }

        private void SendRequest<T>(string connectionId, int queryId, ServerRequestData requestPacket, T packet) where T : new()
        {
            requestPacket.QueryId = queryId;
            if (packet != null)
            {
                requestPacket.Packet = EDCode.EncodeBuffer(SerializerUtil.Serialize(packet));
            }
            var signId = HUtil32.MakeLong((ushort)(queryId ^ 170), (ushort)(requestPacket.Message.Length + requestPacket.Packet.Length + ServerDataPacket.FixedHeaderLen));
            requestPacket.Sign = EDCode.EncodeBuffer(BitConverter.GetBytes(signId));
            SendMessage(connectionId, SerializerUtil.Serialize(requestPacket));
        }

        private void SendMessage(string connectionId, byte[] sendBuffer)
        {
            var serverMessage = new ServerDataPacket
            {
                PacketCode = Grobal2.PacketCode,
                PacketLen = (ushort)sendBuffer.Length
            };
            var dataBuff = SerializerUtil.Serialize(serverMessage);
            var data = new byte[ServerDataPacket.FixedHeaderLen + sendBuffer.Length];
            MemoryCopy.BlockCopy(dataBuff, 0, data, 0, data.Length);
            MemoryCopy.BlockCopy(sendBuffer, 0, data, dataBuff.Length, sendBuffer.Length);
            _serverSocket.Send(connectionId, data);
        }
    }
}