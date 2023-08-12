using NLog;
using System.Net;
using System.Net.Sockets;
using SystemModule;
using SystemModule.ByteManager;
using SystemModule.Core.Config;
using SystemModule.Data;
using SystemModule.DataHandlingAdapters;
using SystemModule.Enums;
using SystemModule.Packets.ServerPackets;
using SystemModule.Sockets.Common;
using SystemModule.Sockets.Config;
using SystemModule.Sockets.Interface;
using SystemModule.Sockets.SocketEventArgs;
using TcpClient = SystemModule.Sockets.Components.TCP.TcpClient;

namespace MarketSystem
{
    public class MarketService : IMarketService
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly TcpClient _clientScoket;
        private readonly MarketManager _marketManager;
        private readonly Dictionary<int, MarketUser> _marketUsers;
        private Thread _thread;
        private bool IsFirstData = false;

        public MarketService()
        {
            _clientScoket = new TcpClient();
            _clientScoket.Connected += MarketScoketConnected;
            _clientScoket.Disconnected += MarketScoketDisconnected;
            _clientScoket.Received += MarketSocketRead;
            _marketManager = new MarketManager();
            _marketUsers = new Dictionary<int, MarketUser>();
        }

        public void Start()
        {
            if (_thread == null)
            {
                _thread = new Thread(CheckConnected);
                _thread.IsBackground = true;
            }
            var config = new TouchSocketConfig();
            config.SetRemoteIPHost(new IPHost(IPAddress.Parse(SystemShare.Config.MarketSrvAddr), SystemShare.Config.MarketSrvPort))
                .SetBufferLength(4096);
            config.SetDataHandlingAdapter(() => new ServerPacketFixedHeaderDataHandlingAdapter());
            _clientScoket.Setup(config);
            try
            {
                _clientScoket.Connect();
            }
            catch (SocketException ex)
            {
                MarketSocketError(ex);
            }
        }

        public void Stop()
        {
            _clientScoket.Close();
        }

        public bool IsConnected => _clientScoket.Online;

        public void CheckConnected()
        {
            if (IsConnected)
            {
                return;
            }
            _clientScoket.Connect();
        }

        public bool SendRequest<T>(int queryId, ServerRequestMessage message, T packet)
        {
            if (!IsConnected)
            {
                return false;
            }
            var requestPacket = new ServerRequestData();
            requestPacket.QueryId = queryId;
            requestPacket.Message = EDCode.EncodeBuffer(SerializerUtil.Serialize(message));
            requestPacket.Packet = EDCode.EncodeBuffer(SerializerUtil.Serialize(packet));
            var signId = HUtil32.MakeLong((ushort)(queryId ^ 170), (ushort)(requestPacket.Message.Length + requestPacket.Packet.Length + ServerDataPacket.FixedHeaderLen));
            requestPacket.Sign = EDCode.EncodeBuffer(BitConverter.GetBytes(signId));
            SendMessage(SerializerUtil.Serialize(requestPacket));
            return true;
        }

        private void SendMessage(byte[] sendBuffer)
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
            _clientScoket.Send(data, 0, data.Length);
        }

        private void MarketScoketDisconnected(object sender, DisconnectEventArgs e)
        {
            var client = (TcpClient)sender;
            _logger.Error("数据库(寄售行)服务器[" + client.MainSocket.RemoteEndPoint + "]断开连接...");
        }

        private void MarketScoketConnected(object sender, MsgEventArgs e)
        {
            var client = (TcpClient)sender;
            _logger.Info("数据库(寄售行)服务器[" + client.MainSocket.RemoteEndPoint + "]连接成功...");
            SendFirstMessage();// 链接成功后进行第一次主动拉取拍卖行数据
            if (_thread != null)
            {
                _thread.Interrupt();
            }
            _thread.Start();
        }

        private void MarketSocketError(SocketException e)
        {
            switch (e.SocketErrorCode)
            {
                case SocketError.ConnectionRefused:
                    _logger.Error("数据库(寄售行)服务器[" + SystemShare.Config.MarketSrvAddr + ":" + SystemShare.Config.MarketSrvPort + "]拒绝链接...");
                    break;
                case SocketError.ConnectionReset:
                    _logger.Error("数据库(寄售行)服务器[" + SystemShare.Config.MarketSrvAddr + ":" + SystemShare.Config.MarketSrvPort + "]关闭连接...");
                    break;
                case SocketError.TimedOut:
                    _logger.Error("数据库(寄售行)服务器[" + SystemShare.Config.MarketSrvAddr + ":" + SystemShare.Config.MarketSrvPort + "]链接超时...");
                    break;
            }
        }

        public void SendUserMarket(INormNpc normNpc, IPlayerActor playerActor, short ItemType, byte UserMode)
        {
            switch (UserMode)
            {
                case MarketConst.USERMARKET_MODE_BUY:
                case MarketConst.USERMARKET_MODE_INQUIRY:
                    RequireLoadUserMarket(playerActor, SystemShare.Config.ServerName + '_' + normNpc.ChrName, ItemType, UserMode, "", "");
                    break;
                case MarketConst.USERMARKET_MODE_SELL:
                    SendUserMarketSellReady(normNpc, playerActor);
                    break;
            }
        }

        private void RequireLoadUserMarket(IPlayerActor user, string MarketName, short ItemType, byte UserMode, string OtherName, string ItemName)
        {
            var IsOk = false;
            var marKetReqInfo = new MarKetReqInfo();
            marKetReqInfo.UserName = user.ChrName;
            marKetReqInfo.MarketName = MarketName;
            marKetReqInfo.SearchWho = OtherName;
            marKetReqInfo.SearchItem = ItemName;
            marKetReqInfo.ItemType = ItemType;
            marKetReqInfo.ItemSet = 0;
            marKetReqInfo.UserMode = UserMode;

            switch (ItemType)
            {
                case MarketConst.USERMARKET_TYPE_ALL:
                case MarketConst.USERMARKET_TYPE_WEAPON:
                case MarketConst.USERMARKET_TYPE_NECKLACE:
                case MarketConst.USERMARKET_TYPE_RING:
                case MarketConst.USERMARKET_TYPE_BRACELET:
                case MarketConst.USERMARKET_TYPE_CHARM:
                case MarketConst.USERMARKET_TYPE_HELMET:
                case MarketConst.USERMARKET_TYPE_BELT:
                case MarketConst.USERMARKET_TYPE_SHOES:
                case MarketConst.USERMARKET_TYPE_ARMOR:
                case MarketConst.USERMARKET_TYPE_DRINK:
                case MarketConst.USERMARKET_TYPE_JEWEL:
                case MarketConst.USERMARKET_TYPE_BOOK:
                case MarketConst.USERMARKET_TYPE_MINERAL:
                case MarketConst.USERMARKET_TYPE_QUEST:
                case MarketConst.USERMARKET_TYPE_ETC:
                case MarketConst.USERMARKET_TYPE_OTHER:
                case MarketConst.USERMARKET_TYPE_ITEMNAME:
                    IsOk = true;
                    break;
                case MarketConst.USERMARKET_TYPE_SET:
                    marKetReqInfo.ItemSet = 1;
                    IsOk = true;
                    break;
                case MarketConst.USERMARKET_TYPE_MINE:
                    marKetReqInfo.SearchWho = user.ChrName;
                    IsOk = true;
                    break;
            }
            if (IsOk)
            {
                if (RequestLoadPageUserMarket(user.ActorId, marKetReqInfo))
                {
                    SendUserMarketCloseMsg(user);
                }
            }
        }

        private void SendUserMarketCloseMsg(IPlayerActor user)
        {
            user.SendMsg(user, Messages.RM_MARKET_RESULT, 0, 0, MarketConst.UMResult_MarketNotReady, 0);
            user.SendMsg(user, Messages.RM_MENU_OK, 0, user.ActorId, 0, 0, "你不能使用寄售商人功能。");
        }

        private void SendUserMarketSellReady(INormNpc normNpc, IPlayerActor user)
        {
            if (!SystemShare.Config.EnableMarket)
            {
                user.SysMsg("寄售商人功能无法使用。", MsgColor.Red, MsgType.Hint);
            }
            SendUserMarketSellReady(user.ActorId, user.ChrName, normNpc.ActorId);
        }

        /// <summary>
        /// 发送拍卖行注册信息
        /// </summary>
        public void SendFirstMessage()
        {
            if (IsFirstData)
            {
                return;
            }
            var request = new ServerRequestMessage(Messages.DB_LOADMARKET, 0, 0, 0, 0);
            var requestData = new MarketRegisterMessage() { ServerIndex = SystemShare.ServerIndex, ServerName = SystemShare.Config.ServerName, GroupId = 1, Token = SystemShare.Config.MarketToken };
            SendRequest(1, request, requestData);
            IsFirstData = true;
        }

        public bool RequestLoadPageUserMarket(int actorId, MarKetReqInfo marKetReqInfo)
        {
            var request = new ServerRequestMessage(Messages.DB_SEARCHMARKET, actorId, 0, 0, 0);
            var requestData = new MarketSearchMessage
            {
                UserName = marKetReqInfo.UserName,
                MarketName = marKetReqInfo.MarketName,
                SearchWho = marKetReqInfo.SearchWho,
                SearchItem = marKetReqInfo.SearchItem,
                ItemType = marKetReqInfo.ItemType,
                ItemSet = marKetReqInfo.ItemSet,
                UserMode = marKetReqInfo.UserMode
            };
            SendRequest(1, request, requestData);
            return true;
        }

        public bool SendUserMarketSellReady(int actorId, string chrName, int marketNpc)
        {
            var request = new ServerRequestMessage(Messages.DB_LOADUSERMARKET, actorId, 0, 0, 0);
            var requestData = new MarketSearchMessage
            {
                UserName = chrName,
                MarketNPC = marketNpc
            };
            SendRequest(1, request, requestData);
            _logger.Info($"发送用户[{chrName}]个人拍卖行数据请求");
            return true;
        }

        private void MarketSocketRead(object sender, ByteBlock byteBlock, IRequestInfo requestInfo)
        {
            if (requestInfo is not DataMessageFixedHeaderRequestInfo fixedHeader)
                return;
            try
            {
                if (fixedHeader.Header.PacketCode != Grobal2.PacketCode)
                {
                    _logger.Debug($"解析寄售行封包出现异常封包...");
                    return;
                }
                var messageData = SerializerUtil.Deserialize<ServerRequestData>(fixedHeader.Message);
                ProcessServerData(messageData);
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
            }
        }

        private void ProcessServerData(ServerRequestData responsePacket)
        {
            if (responsePacket != null)
            {
                var respCheckCode = responsePacket.QueryId;
                var nLen = responsePacket.Message.Length + responsePacket.Packet.Length + ServerDataPacket.FixedHeaderLen;
                if (nLen >= 12)
                {
                    var queryId = HUtil32.MakeLong((ushort)(respCheckCode ^ 170), (ushort)nLen);
                    if (queryId <= 0 || responsePacket.Sign.Length <= 0)
                    {
                        SystemShare.Config.nLoadDBErrorCount++;
                        return;
                    }
                    var signatureBuff = BitConverter.GetBytes(queryId);
                    var signBuff = EDCode.DecodeBuff(responsePacket.Sign);
                    if (BitConverter.ToInt16(signatureBuff) == BitConverter.ToInt16(signBuff))
                    {
                        var commandMessage = SerializerUtil.Deserialize<ServerRequestMessage>(responsePacket.Message, true);
                        switch (commandMessage.Ident)
                        {
                            case Messages.DB_LOADMARKETSUCCESS:
                                _marketManager.OnMsgReadData(SerializerUtil.Deserialize<MarketDataMessage>(responsePacket.Packet));
                                _logger.Info("加载拍卖行数据成功...");
                                break;
                            case Messages.DB_LOADMARKETFAIL:
                                if (commandMessage.Param == 1)
                                {
                                    _logger.Info("拍卖行物品列表为空...");
                                }
                                else
                                {
                                    _logger.Info("获取拍卖行数据失败...");
                                }
                                _marketManager.OnMsgReadData();
                                break;
                            case Messages.DB_LOADUSERMARKETSUCCESS:
                                var user = SystemShare.ActorMgr.Get<IPlayerActor>(commandMessage.Recog);
                                if (user != null)
                                {
                                    ReadyToSellUserMarket(user, SerializerUtil.Deserialize<MarkerUserLoadMessage>(responsePacket.Packet));
                                }
                                else
                                {
                                    _logger.Debug("玩家不在线,拍卖行数据无需返回给玩家");
                                }
                                break;
                            case Messages.DB_SEARCHMARKETSUCCESS:
                                if (commandMessage.Recog > 0) // 搜索数据需要返回给玩家
                                {
                                    var searchUser = SystemShare.ActorMgr.Get<IPlayerActor>(commandMessage.Recog);
                                    if (searchUser != null)
                                    {
                                        SendUserMarketList(searchUser, 0, SerializerUtil.Deserialize<MarketDataMessage>(responsePacket.Packet));
                                    }
                                    else
                                    {
                                        _logger.Debug("玩家不在线,拍卖行数据无需返回给玩家");
                                    }
                                }
                                _logger.Info("搜索拍卖行数据成功...");
                                break;
                            case Messages.DB_SRARCHMARKETFAIL:
                                if (commandMessage.Recog > 0) // 搜索数据需要返回给玩家
                                {
                                    var searchUser = SystemShare.ActorMgr.Get<IPlayerActor>(commandMessage.Recog);
                                    if (searchUser != null && commandMessage.Param <= 1)
                                    {
                                        //searchUser.SendUserMarketList(0, SerializerUtil.Deserialize<MarketDataMessage>(responsePacket.Packet));
                                        searchUser.SysMsg("未找到相关物品.", MsgColor.Green, MsgType.Hint);
                                    }
                                    else
                                    {
                                        _logger.Debug("玩家不在线,拍卖行数据无需返回给玩家");
                                    }
                                }
                                _logger.Info("搜索拍卖行数据失败...");
                                break;
                            case Messages.DB_SAVEMARKETSUCCESS:// 保存结果需要返回给玩家
                                var saveUser = SystemShare.ActorMgr.Get<IPlayerActor>(commandMessage.Recog);
                                if (saveUser != null)
                                {
                                    if (commandMessage.Tag == 1)
                                    {
                                        saveUser.SysMsg("物品上架成功...由于缓存原因需要等待几分钟后才能正常显示.", MsgColor.Green, MsgType.Hint);
                                    }
                                    else
                                    {
                                        //todo 把物品还给玩家
                                        saveUser.SysMsg("物品上架失败.", MsgColor.Green, MsgType.Hint);
                                    }
                                }
                                else
                                {
                                    //todo 把物品还给玩家，或者需要通过邮件发送给玩家
                                    _logger.Debug("玩家不在线,拍卖行数据无法返回给玩家");
                                }
                                _logger.Info("保存拍卖行数据成功...");
                                break;
                        }
                    }
                    else
                    {
                        _logger.Warn("非法拍卖行数据封包，解析失败...");
                        SystemShare.Config.nLoadDBErrorCount++;
                    }
                }
            }
            else
            {
                _logger.Error("错误的封包数据");
            }
        }

        private void SendUserMarketList(IPlayerActor user, int nextPage, MarketDataMessage marketData)
        {
            if (marketData.TotalCount <= 0)
            {
                return;
            }
            var page = 0;
            byte bFirstSend = 0;
            if (nextPage == 0)
            {
                page = 1;
                bFirstSend = 1;
            }
            var marketUser = _marketUsers[user.ActorId];
            if (nextPage == 1)
            {
                page = marketUser.CurrPage + 1;
            }
            marketUser.CurrPage = page;
            var maxpage = (int)Math.Ceiling(marketData.TotalCount / (double)MarketConst.MAKET_ITEMCOUNT_PER_PAGE);
            var marketItems = marketData.List.Skip((page - 1) * MarketConst.MAKET_ITEMCOUNT_PER_PAGE).Take(MarketConst.MAKET_ITEMCOUNT_PER_PAGE).ToList();
            var sendMsg = string.Empty;
            var cnt = 0;
            if (marketItems.Count > 0)
            {
                for (int i = 0; i < marketItems.Count; i++)
                {
                    cnt++;
                    sendMsg = sendMsg + EDCode.EncodeBuffer(marketData.List[i]) + '/';
                }
            }
            sendMsg = cnt + '/' + page + '/' + maxpage + '/' + sendMsg;
            user.SendMsg(Messages.RM_MARKET_LIST, 0, marketUser.UserMode, marketUser.ItemType, bFirstSend, sendMsg);
        }

        private void ReadyToSellUserMarket(IPlayerActor user, MarkerUserLoadMessage readyItem)
        {
            if (readyItem.IsBusy != MarketConst.UMRESULT_SUCCESS) return;
            if (readyItem.SellCount < MarketConst.MARKET_MAX_SELL_COUNT)
            {
                user.SendMsg(Messages.RM_MARKET_RESULT, 0, readyItem.MarketNPC, MarketConst.UMResult_ReadyToSell, 0);
            }
            else
            {
                user.SendMsg(Messages.RM_MARKET_RESULT, 0, 0, MarketConst.UMResult_OverSellCount, 0);
            }
            // FlagReadyToSellCheck = true;
        }
    }
}