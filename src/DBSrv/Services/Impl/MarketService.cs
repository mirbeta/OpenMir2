using DBSrv.Conf;
using DBSrv.Storage;
using NLog;
using System;
using System.Linq;
using System.Net;
using SystemModule;
using SystemModule.CoreSocket.Sockets.Common;
using SystemModule.DataHandlingAdapters;
using SystemModule.Extensions;
using SystemModule.Packets.ServerPackets;
using TouchSocket.Core;
using TouchSocket.Sockets;

namespace DBSrv.Services.Impl
{
    /// <summary>
    /// 拍卖行数据存储服务
    /// GameSrv-> DBSrv
    /// </summary>
    public class MarketService : IService
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly ICacheStorage _cacheStorage;
        private readonly IMarketStorage _marketStorage;
        private readonly TcpService _socketServer;
        private readonly SettingConf _setting;

        public MarketService(SettingConf setting, ICacheStorage cacheStorage, IMarketStorage marketStorage)
        {
            _setting = setting;
            _cacheStorage = cacheStorage;
            _marketStorage = marketStorage;
            _socketServer = new TcpService();
            _socketServer.Connected += Connecting;
            _socketServer.Disconnected += Disconnected;
            _socketServer.Received += Received;
        }

        public void Initialize()
        {
            var touchSocketConfig = new TouchSocketConfig();
            touchSocketConfig.SetListenIPHosts(new IPHost[1]
            {
                new IPHost(IPAddress.Parse(_setting.MarketServerAddr), _setting.MarketServerPort)
            }).SetDataHandlingAdapter(() => new ServerPacketFixedHeaderDataHandlingAdapter());
            _socketServer.Setup(touchSocketConfig);
        }

        public void Start()
        {
            _socketServer.Start();
            _logger.Info($"拍卖行数据库服务[{_setting.MarketServerAddr}:{_setting.MarketServerPort}]已启动.等待链接...");
        }

        public void Stop()
        {
            _socketServer.Stop();
        }
        
        public void PushMarketData()
        {
            //todo 根据服务器分组推送到各个GameSrv或者推送到所有GameSrv
            byte groupId = 0;//GroupID为0时查询所有区服的拍卖行数据
            var marketItems = _marketStorage.QueryMarketItems(groupId);
            if (!marketItems.Any())
            {
                _logger.Info("拍卖行数据为空,跳过推送拍卖行数据.");
                return;
            }
            var socketList = _socketServer.GetClients();
            foreach (var client in socketList)
            {
                if (_socketServer.SocketClientExist(client.ID))
                {
                    _socketServer.Send(client.ID, Array.Empty<byte>());//推送拍卖行数据
                }
            }
            _logger.Info($"推送拍卖行数据成功.当前拍卖行物品数据:[{marketItems.Count()}],在线服务器:[{socketList.Length}]");
        }
        
        private void Received(object sender, ByteBlock byteBlock, IRequestInfo requestInfo)
        {
            if (requestInfo is not ServerDataMessageFixedHeaderRequestInfo fixedHeader)
                return;
            var client = (SocketClient)sender;
            try
            {
                if (fixedHeader.Header.PacketCode != Grobal2.PacketCode)
                {
                    _logger.Error($"解析寄售行封包出现异常封包...");
                    return;
                }
                var messageData = SerializerUtil.Deserialize<ServerRequestData>(fixedHeader.Message);
                ProcessMessagePacket(client.ID, messageData);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
            }
        }

        private void Connecting(object sender, TouchSocketEventArgs e)
        {
            var client = (SocketClient)sender;
            var remoteIp = client.MainSocket.RemoteEndPoint.GetIP();
            if (!DBShare.CheckServerIP(remoteIp))
            {
                _logger.Warn("非法服务器连接: " + remoteIp);
                client.Close();
                return;
            }
            _logger.Info("拍卖行客户端(GameSrv)连接 " + client.MainSocket.RemoteEndPoint);
        }

        private void Disconnected(object sender, DisconnectEventArgs e)
        {
            var client = (SocketClient)sender;
            _logger.Info("拍卖行客户端(GameSrv)断开连接 " + client.MainSocket.RemoteEndPoint);
        }

        private void ProcessMessagePacket(string connectionId, ServerRequestData requestData)
        {
            int nQueryId = requestData.QueryId;
            var requestMessage = SerializerUtil.Deserialize<ServerRequestMessage>(EDCode.DecodeBuff(requestData.Message));
            var packetLen = requestData.Message.Length + requestData.Packet.Length + ServerDataPacket.FixedHeaderLen;
            if (packetLen >= Messages.DefBlockSize && nQueryId > 0 && requestData.Packet != null && requestData.Sgin != null)
            {
                var sData = EDCode.DecodeBuff(requestData.Packet);
                var queryId = HUtil32.MakeLong((ushort)(nQueryId ^ 170), (ushort)packetLen);
                if (queryId <= 0)
                {
                    SendFailMessage(nQueryId, connectionId, new ServerRequestMessage(Messages.DBR_FAIL, 0, 0, 0, 0));
                    return;
                }
                if (requestData.Sgin.Length <= 0)
                {
                    SendFailMessage(nQueryId, connectionId, new ServerRequestMessage(Messages.DBR_FAIL, 0, 0, 0, 0));
                    return;
                }
                var signatureBuff = BitConverter.GetBytes(queryId);
                var signatureId = BitConverter.ToInt16(signatureBuff);
                var sginBuff = EDCode.DecodeBuff(requestData.Sgin);
                var sgin = BitConverter.ToInt16(sginBuff);
                if (sgin == signatureId)
                {
                    ProcessMarketPacket(nQueryId, requestMessage, sData, connectionId);
                    return;
                }
                _socketServer.TryGetSocketClient(connectionId, out var client);
                client.Close();
                _logger.Error($"关闭错误的任务{nQueryId}查询请求.");
                return;
            }
            SendFailMessage(nQueryId, connectionId, new ServerRequestMessage(Messages.DBR_FAIL, 0, 0, 0, 0));
        }

        private void ProcessMarketPacket(int nQueryId, ServerRequestMessage packet, byte[] sData, string connectionId)
        {
            switch (packet.Ident)
            {
                case Messages.DB_LOADMARKET://GameSrv主动拉取拍卖行数据
                    LoadMarketList(nQueryId, sData, connectionId);
                    break;
                case Messages.DB_SAVEMARKET://GameSrv保存拍卖行数据
                    SaveMarketItem(nQueryId, packet.Recog, sData, connectionId);
                    break;
                case Messages.DB_SEARCHMARKET://GameSrv搜索拍卖行数据
                    SearchMarketItem(nQueryId, packet.Recog, sData, connectionId);
                    break;
                case Messages.DB_LOADUSERMARKET://GameSrv拉取玩家拍卖行数据
                    QueryMarketUserLoad(nQueryId, packet.Recog, sData, connectionId);
                    break;
            }
        }

        private void QueryMarketUserLoad(int nQueryId, int actorId, byte[] sData, string connectionId)
        {
            var userMarket = SerializerUtil.Deserialize<MarketSearchMessage>(sData);
            if (userMarket.GroupId == 0)
            {
                var messagePacket = new ServerRequestMessage(Messages.DB_SRARCHMARKETFAIL, 0, 0, 0, 0);
                SendFailMessage(nQueryId, connectionId, messagePacket);
                _logger.Info($"服务器组[{userMarket.GroupId}]拍卖行数据为空,搜索拍卖行数据失败.");
                return;
            }
            var userItemLoad = _marketStorage.QueryMarketItemsCount(userMarket.GroupId, userMarket.SearchWho);
            var marketLoadMessgae = new MarkerUserLoadMessage();
            marketLoadMessgae.SellCount = userItemLoad;
            marketLoadMessgae.IsBusy = 0;
            marketLoadMessgae.MarketNPC = userMarket.MarketNPC;
            SendSuccessMessage(connectionId, actorId, Messages.DB_LOADUSERMARKETSUCCESS, marketLoadMessgae);
            _logger.Info($"获取服务器组[{userMarket.GroupId}] 用户[{userMarket.SearchWho}]个人拍卖行数据...");
        }

        private void SearchMarketItem(int nQueryId, int actorId, byte[] sData, string connectionId)
        {
            var marketSearch = SerializerUtil.Deserialize<MarketSearchMessage>(sData);
            if (marketSearch.GroupId == 0)
            {
                var messagePacket = new ServerRequestMessage(Messages.DB_SRARCHMARKETFAIL, 0, 0, 0, 0);
                SendFailMessage(nQueryId, connectionId, messagePacket);
                _logger.Info($"服务器组[{marketSearch.GroupId}]拍卖行数据为空,搜索拍卖行数据失败.");
                return;
            }
            var searchItems = _marketStorage.SearchMarketItems(marketSearch.GroupId,marketSearch.MarketName, marketSearch.SearchItem, marketSearch.SearchWho, marketSearch.ItemType, marketSearch.ItemSet);
            if (!searchItems.Any())
            {
                var messagePacket = new ServerRequestMessage(Messages.DB_SRARCHMARKETFAIL, 0, 1, 0, 0);
                SendFailMessage(nQueryId, connectionId, messagePacket);
                _logger.Info($"服务器组[{marketSearch.GroupId}]拍卖行数据为空,搜索拍卖行数据失败.");
                return;
            }
            var marketItemMessgae = new MarketDataMessage();
            marketItemMessgae.List = searchItems.ToList();
            marketItemMessgae.TotalCount = searchItems.Count();
            SendSuccessMessage(connectionId, actorId, Messages.DB_SEARCHMARKETSUCCESS, marketItemMessgae);
            _logger.Info($"服务器组[{marketSearch.GroupId}]搜索拍卖行数据...");
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
                var messagePacket = new ServerRequestMessage(Messages.DB_LOADMARKETFAIL, 0, 1, 0, 0);
                SendFailMessage(nQueryId, connectionId, messagePacket);
                _logger.Info($"当前服务器组[{marketMessage.GroupId}]拍卖行数据为空,读取拍卖行数据失败.");
                return;
            }
            var marketItemMessgae = new MarketDataMessage();
            marketItemMessgae.List = marketItems.ToList();
            marketItemMessgae.TotalCount = marketItems.Count();
            SendSuccessMessage(connectionId, 0, Messages.DB_LOADMARKETSUCCESS, marketItemMessgae);
            _logger.Info($"服务器组[{marketMessage.GroupId}] [{marketMessage.ServerName}]读取拍卖行数据成功.当前拍卖行物品数据:[{marketItemMessgae.TotalCount}]");
        }

        private void SaveMarketItem(int nQueryId, int actorId, byte[] sData, string connectionId)
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
            SendSuccessMessage(connectionId, actorId, Messages.DB_SAVEMARKETSUCCESS, saveMessage);
            _logger.Info($"服务器组[{saveMessage.GroupId}] [{saveMessage.ServerName}]保存拍卖行数据成功.当前拍卖行物品数据:[{marketItemsCount}]");
        }

        private void SendSuccessMessage<T>(string connectionId, int actorId, byte messageId, T data)
        {
            var responsePack = new ServerRequestData();
            var messagePacket = new ServerRequestMessage(messageId, actorId, 0, 0, 0);
            responsePack.Message = EDCode.EncodeBuffer(SerializerUtil.Serialize(messagePacket));
            responsePack.Packet = EDCode.EncodeBuffer(SerializerUtil.Serialize(data));
            SendRequest(connectionId, 1, responsePack);
        }

        private void SendFailMessage(int nQueryId, string connectionId, ServerRequestMessage messagePacket)
        {
            var responsePack = new ServerRequestData();
            responsePack.Message = EDCode.EncodeBuffer(SerializerUtil.Serialize(messagePacket));
            SendRequest(connectionId, nQueryId, responsePack);
        }

        private void SendRequest(string connectionId, int queryId, ServerRequestData requestPacket)
        {
            requestPacket.QueryId = queryId;
            var queryPart = 0;
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
            requestPacket.Sgin = EDCode.EncodeBuffer(nCheckCode);
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
            _socketServer.Send(connectionId, data);
        }
    }
}