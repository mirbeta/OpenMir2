using System;
using System.Collections.Generic;
using System.Linq;
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
    /// 拍卖行数据存储服务
    /// GameSrv-> DBSrv
    /// </summary>
    public class MarketService
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly ICacheStorage _cacheStorage;
        private readonly IMarketStorage _marketStorage;
        private readonly SocketServer _socketServer;
        private readonly SettingConf _setting;
        private readonly IList<ServerDataInfo> serverList;

        public MarketService(SettingConf setting, ICacheStorage cacheStorage, IMarketStorage marketStorage)
        {
            _setting = setting;
            _cacheStorage = cacheStorage;
            _marketStorage = marketStorage;
            _socketServer = new SocketServer(byte.MaxValue, 1024);
            _socketServer.OnClientConnect += ServerSocketClientConnect;
            _socketServer.OnClientDisconnect += ServerSocketClientDisconnect;
            _socketServer.OnClientRead += ServerSocketClientRead;
            _socketServer.OnClientError += ServerSocketClientError;
            serverList = new List<ServerDataInfo>();
        }

        public void Start()
        {
            _socketServer.Init();
            _socketServer.Start(_setting.MarketServerAddr, _setting.MarketServerPort);
            _logger.Info($"拍卖行数据库服务[{_setting.MarketServerAddr}:{_setting.MarketServerPort}]已启动.等待链接...");
        }

        public void Stop()
        {
            _socketServer.Shutdown();
        }
        
        public void PushMarketData()
        {
            //todo 根据服务器分组推送到各个GameSrv或者推送到所有GameSrv
            byte groupId = 0;//GroupID为0时查询所有区服的拍卖行数据
            var marketItems = _marketStorage.QueryMarketItems(groupId);
            if (!marketItems.Any())
            {
                _logger.Info("当前服务器分组拍卖行数据为空,推送拍卖行数据失败.");
                return;
            }
            var socketList = _socketServer.GetSockets();
            foreach (var client in socketList)
            {
                if (_socketServer.IsOnline(client.ConnectionId))
                {
                    _socketServer.Send(client.ConnectionId, Array.Empty<byte>());//推送拍卖行数据
                }
            }
            _logger.Info($"推送拍卖行数据成功.当前拍卖行物品数据:[{marketItems.Count()}],在线服务器:[{socketList.Count}]");
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
            serverList.Add(serverInfo);
            _logger.Info("新的客户端连接 " + e.RemoteIPaddr);
        }

        private void ServerSocketClientRead(object sender, AsyncUserToken e)
        {
            for (var i = 0; i < serverList.Count; i++)
            {
                var serverInfo = serverList[i];
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

        private void ServerSocketClientDisconnect(object sender, AsyncUserToken e)
        {

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
                    SendFailMessage(nQueryId, serverInfo.ConnectionId);
                    return;
                }
                if (requestData.Sgin.Length <= 0)
                {
                    SendFailMessage(nQueryId, serverInfo.ConnectionId);
                    return;
                }
                var signatureBuff = BitConverter.GetBytes(queryId);
                var signatureId = BitConverter.ToInt16(signatureBuff);
                var sginBuff = EDCode.DecodeBuff(requestData.Sgin);
                var sgin = BitConverter.ToInt16(sginBuff);
                if (sgin == signatureId)
                {
                    ProcessMarketPacket(nQueryId, requestMessage, sData, serverInfo.ConnectionId);
                    return;
                }
                _socketServer.CloseSocket(serverInfo.ConnectionId);
                _logger.Error($"关闭错误的任务{nQueryId}查询请求.");
                return;
            }
            SendFailMessage(nQueryId, serverInfo.ConnectionId);
        }

        private void ProcessMarketPacket(int nQueryId, ServerRequestMessage packet, byte[] sData, string connectionId)
        {
            switch (packet.Ident)
            {
                case Messages.DB_LOADMARKET: //GameSrv主动拉取拍卖行数据
                    LoadMarketList(nQueryId, sData, connectionId);
                    break;
                case Messages.DB_SAVEMARKET: //GameSrv保存拍卖行数据
                    SaveMarketItem(nQueryId, sData, connectionId);
                    break;
                case Messages.DB_SEARCHMARKET: //GameSrv搜索拍卖行数据
                    SearchMarketItem(nQueryId, sData, connectionId);
                    break;
            }
        }

        private void SearchMarketItem(int nQueryId, byte[] sData, string connectionId)
        {
            var marKetReqInfo = SerializerUtil.Deserialize<MarketSearchMessage>(sData);
            if (marKetReqInfo.GroupId == 0)
            {
                return;
            }
            var searchItems = _marketStorage.SearchMarketItems(marKetReqInfo.GroupId,marKetReqInfo.MarketName, marKetReqInfo.SearchItem, marKetReqInfo.SearchWho, marKetReqInfo.ItemType, marKetReqInfo.ItemSet);
            var marketItemMessgae = new MarketDataMessage();
            marketItemMessgae.List = searchItems.ToList();
            marketItemMessgae.TotalCount = searchItems.Count();
            SendSuccessMessage(connectionId, Messages.DB_SEARCHMARKETSUCCESS, marketItemMessgae);
        }
        
        private void LoadMarketList(int nQueryId, byte[] sData, string connectionId)
        {
            var marketMessage = SerializerUtil.Deserialize<MarketRegisterMessage>(sData);
            if (string.IsNullOrEmpty(marketMessage.Token) || string.IsNullOrEmpty(marketMessage.ServerName))
            {
                _logger.Warn($"SocketId:{connectionId} QueryId:[{nQueryId}] 非法获取拍卖行数据...");
                return;
            }
            var marketItems = _marketStorage.QueryMarketItems(marketMessage.GroupId);
            if (!marketItems.Any())
            {
                _logger.Info("当前服务器分组拍卖行数据为空,读取拍卖行数据失败.");
                return;
            }
            var marketItemMessgae = new MarketDataMessage();
            marketItemMessgae.List = marketItems.ToList();
            marketItemMessgae.TotalCount = marketItems.Count();
            SendSuccessMessage(connectionId, Messages.DB_LOADMARKETSUCCESS, marketItemMessgae);
            _logger.Info($"服务器组[{marketMessage.GroupId}] [{marketMessage.ServerName}]读取拍卖行数据成功.当前拍卖行物品数据:[{marketItemMessgae.TotalCount}]");
        }

        private void SaveMarketItem(int nQueryId, byte[] sData, string connectionId)
        {
            var saveMessage = SerializerUtil.Deserialize<MarketSaveDataItem>(sData);
            if (saveMessage.GroupId == 0 || string.IsNullOrEmpty(saveMessage.ServerName))
            {
                _logger.Warn($"任务[{nQueryId}]非法获取拍卖行数据...");
                return;
            }
            var marketItems = _marketStorage.SaveMarketItem(saveMessage.Item, saveMessage.GroupId, saveMessage.ServerIndex);
            if (!marketItems)
            {
                _logger.Info("当前服务器分组拍卖行数据为空,推送拍卖行数据失败.");
                return;
            }
            var marketItemsCount = _marketStorage.QueryMarketItems(saveMessage.GroupId);
            SendSuccessMessage(connectionId, Messages.DB_SAVEMARKETSUCCESS, saveMessage);
            _logger.Info($"服务器组[{saveMessage.GroupId}] [{saveMessage.ServerName}]保存拍卖行数据成功.当前拍卖行物品数据:[{marketItemsCount}]");
        }

        private void SendSuccessMessage<T>(string connectionId, byte messageId, T data)
        {
            var responsePack = new ServerRequestData();
            var messagePacket = new ServerRequestMessage(messageId, 0, 0, 0, 0);
            responsePack.Message = EDCode.EncodeBuffer(SerializerUtil.Serialize(messagePacket));
            responsePack.Packet = EDCode.EncodeBuffer(SerializerUtil.Serialize(data));
            SendRequest(connectionId, 0, responsePack);
        }

        private void SendFailMessage(int nQueryId, string connectionId)
        {
            var responsePack = new ServerRequestData();
            var messagePacket = new ServerRequestMessage(Messages.DBR_FAIL, 0, 0, 0, 0);
            responsePack.Message = EDCode.EncodeBuffer(SerializerUtil.Serialize(messagePacket));
            SendRequest(connectionId, nQueryId, responsePack);
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
            _socketServer.Send(connectionId, data);
        }
    }
}