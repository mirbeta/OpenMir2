/// <summary>
/// 网关客户端(LoginGate-LoginSrv)
/// </summary>
public class ClientThread
{
    /// <summary>
    /// 用户会话
    /// </summary>
    public readonly TSessionInfo[] SessionArray;
    /// <summary>
    /// Socket
    /// </summary>
    private readonly TcpClient _clientSocket;
    /// <summary>
    /// Client管理
    /// </summary>
    private readonly ClientManager _clientManager;
    /// <summary>
    /// Session管理
    /// </summary>
    private readonly SessionManager _sessionManager;
    private GameGateInfo gameGateInfo;
    /// <summary>
    /// 数据缓冲区
    /// </summary>
    private readonly byte[] DataBuff;
    /// <summary>
    /// 缓存缓冲长度
    /// </summary>
    private int DataLen;

    public ClientThread(ClientManager clientManager, SessionManager sessionManager)
    {
        _clientManager = clientManager;
        _sessionManager = sessionManager;
        _clientSocket = new TcpClient();
        _clientSocket.Connected += ClientSocketConnect;
        _clientSocket.Disconnected += ClientSocketDisconnect;
        _clientSocket.Received += ClientSocketRead;
        SessionArray = new TSessionInfo[GateShare.MaxSession];
        DataBuff = new byte[2048 * 10];
    }

    public bool IsConnected => _clientSocket.Online;

    public EndPoint EndPoint => _clientSocket.RemoteIPHost.EndPoint;

    public void Initialize(GameGateInfo gateInfo)
    {
        gameGateInfo = gateInfo;
        var config = new TouchSocketConfig()
        .SetRemoteIPHost(new IPHost(IPAddress.Parse(gateInfo.LoginServer), gateInfo.LoginPort)).ConfigureContainer(a =>
        {
            a.AddConsoleLogger();
        });
        _clientSocket.Setup(config);
    }

    public void Start()
    {
        try
        {
            if (_clientSocket.Online)
            {
                return;
            }
            _clientSocket.Connect();
        }
        catch (TimeoutException)
        {
            LogService.Error($"链接登录服务器[{gameGateInfo.LoginServer}:{gameGateInfo.LoginPort}]超时.");
        }
        catch (Exception)
        {
            LogService.Error($"链接登录服务器[{gameGateInfo.LoginServer}:{gameGateInfo.LoginPort}]失败.");
        }
    }

    public void Stop()
    {
        _clientSocket.Close();
    }

    public bool SessionIsFull()
    {
        for (int i = 0; i < GateShare.MaxSession; i++)
        {
            TSessionInfo userSession = SessionArray[i];
            if (userSession == null)
            {
                return false;
            }
        }

        return true;
    }

    public TSessionInfo[] GetSession()
    {
        return SessionArray;
    }

    private Task ClientSocketConnect(ITcpClient client, ConnectedEventArgs e)
    {
        ConnectState = true;
        RestSessionArray();
        KeepAliveTick = HUtil32.GetTickCount();
        CheckServerTick = HUtil32.GetTickCount();
        CheckServerFailCount = 1;
        //_clientManager.AddClientThread(e.SocketHandle, this);
        LogService.Info($"账号服务器[{((TcpClientBase)client).GetIPPort()}]链接成功.");
        LogService.Debug($"线程[{Guid.NewGuid():N}]连接 {((TcpClientBase)client).GetIPPort()} 成功...");
        return Task.CompletedTask;
    }

    private Task ClientSocketDisconnect(ITcpClientBase client, DisconnectEventArgs e)
    {
        for (int i = 0; i < GateShare.MaxSession; i++)
        {
            TSessionInfo userSession = SessionArray[i];
            if (userSession == null)
            {
                continue;
            }

            SessionArray[i] = null;
            LogService.Debug("账号服务器断开Socket");
        }

        RestSessionArray();
        ConnectState = false;
        //_clientManager.DeleteClientThread(e.SocketHandle);
        LogService.Info($"账号服务器[{((SocketClient)client).GetIPPort()}]断开链接.");
        return Task.CompletedTask;
    }

    /// <summary>
    /// 收到登录服务器消息 直接发送给客户端
    /// </summary>
    private Task ClientSocketRead(IClient client, ReceivedDataEventArgs e)
    {
        int nMsgLen = e.ByteBlock.Len;
        if (nMsgLen <= 0)
        {
            return Task.CompletedTask;
        }

        if (DataLen > 0)
        {
            MemoryCopy.BlockCopy(e.ByteBlock.Buffer, 0, DataBuff, DataLen, nMsgLen);
            ProcessServerData(DataBuff, DataLen + nMsgLen);
        }
        else
        {
            ProcessServerData(e.ByteBlock.Buffer, nMsgLen);
        }

        ReceiveBytes += nMsgLen;
        return Task.CompletedTask;
    }

    private void ProcessServerData(byte[] data, int nLen)
    {
        int srcOffset = 0;
        Span<byte> dataBuff = data;
        while (nLen > ServerDataPacket.FixedHeaderLen)
        {
            Span<byte> packetHead = dataBuff[..ServerDataPacket.FixedHeaderLen];
            ServerDataPacket message = SerializerUtil.Deserialize<ServerDataPacket>(packetHead);
            if (message.PacketCode != Grobal2.PacketCode)
            {
                srcOffset++;
                dataBuff = dataBuff.Slice(srcOffset, ServerDataPacket.FixedHeaderLen);
                nLen -= 1;
                LogService.Debug($"解析封包出现异常封包，PacketLen:[{dataBuff.Length}] Offset:[{srcOffset}].");
                continue;
            }

            int nCheckMsgLen = Math.Abs(message.PacketLen + ServerDataPacket.FixedHeaderLen);
            if (nCheckMsgLen > nLen)
            {
                break;
            }

            ServerDataMessage messageData =
                SerializerUtil.Deserialize<ServerDataMessage>(dataBuff[ServerDataPacket.FixedHeaderLen..]);
            switch (messageData.Type)
            {
                case ServerDataType.KeepAlive:
                    KeepAliveTick = HUtil32.GetTickCount();
                    break;
                case ServerDataType.Leave:
                    _sessionManager.CloseSession(messageData.SocketId);
                    break;
                case ServerDataType.Data:
                    _clientManager.SendQueue(messageData);
                    break;
            }

            nLen -= nCheckMsgLen;
            if (nLen <= 0)
            {
                break;
            }

            dataBuff = dataBuff.Slice(nCheckMsgLen, nLen);
            DataLen = nLen;
            srcOffset = 0;
            if (nLen < ServerDataPacket.FixedHeaderLen)
            {
                break;
            }
        }

        if (nLen > 0) //有部分数据被处理,需要把剩下的数据拷贝到接收缓冲的头部
        {
            MemoryCopy.BlockCopy(dataBuff, 0, DataBuff, 0, nLen);
            DataLen = nLen;
        }
        else
        {
            DataLen = 0;
        }
    }

    private void RestSessionArray()
    {
        for (int i = 0; i < GateShare.MaxSession; i++)
        {
            if (SessionArray[i] != null)
            {
                SessionArray[i].ReceiveTick = HUtil32.GetTickCount();
                SessionArray[i].ConnectionId = string.Empty;
                SessionArray[i].ClientIP = string.Empty;
            }
        }
    }

    public void SendClientPacket(ServerDataMessage packet)
    {
        if (!IsConnected)
        {
            return;
        }

        SendMessage(SerializerUtil.Serialize(packet));
    }

    private void SendMessage(byte[] sendBuffer)
    {
        ServerDataPacket serverMessage = new ServerDataPacket
        {
            PacketCode = Grobal2.PacketCode,
            PacketLen = (ushort)sendBuffer.Length
        };
        byte[] dataBuff = SerializerUtil.Serialize(serverMessage);
        byte[] data = new byte[ServerDataPacket.FixedHeaderLen + sendBuffer.Length];
        MemoryCopy.BlockCopy(dataBuff, 0, data, 0, data.Length);
        MemoryCopy.BlockCopy(sendBuffer, 0, data, dataBuff.Length, sendBuffer.Length);
        _clientSocket.Send(data);
        SendBytes += data.Length;
    }

    private string SendStatistics()
    {
        string sendStr = SendBytes switch
        {
            > 1024 * 1000 => $"↑{SendBytes / (1024 * 1000)}M",
            > 1024 => $"↑{SendBytes / 1024}K",
            _ => $"↑{SendBytes}B"
        };
        SendBytes = 0;
        return sendStr;
    }

    private string ReceiveStatistics()
    {
        string receiveStr = ReceiveBytes switch
        {
            > 1024 * 1000 => $"↓{ReceiveBytes / (1024 * 1000)}M",
            > 1024 => $"↓{ReceiveBytes / 1024}K",
            _ => $"↓{ReceiveBytes}B"
        };
        ReceiveBytes = 0;
        return receiveStr;
    }

    private string GetSessionCount()
    {
        int count = 0;
        for (int i = 0; i < SessionArray.Length; i++)
        {
            if (SessionArray[i] != null)
            {
                count++;
            }
        }

        if (count > Counter)
        {
            Counter = count;
        }

        return count + "/" + Counter;
    }

    private string GetConnected()
    {
        return IsConnected ? "[green]Connected[/]" : "[red]Not Connected[/]";
    }

    public (string remoteendpoint, string status, string sessionCount, string reviceTotal, string sendTotal, string
        threadCount) GetStatus()
    {
        return (EndPoint?.ToString(), GetConnected(), GetSessionCount(), SendStatistics(), ReceiveStatistics(), "1");
    }

    /// <summary>
    /// 网关编号（初始化的时候进行分配）
    /// </summary>
    public readonly int ClientId = 0;
    /// <summary>
    ///  网关游戏服务器之间检测是否失败（超时）
    /// </summary>
    public bool CheckServerFail = false;
    /// <summary>
    /// 网关游戏服务器之间检测是否失败次数
    /// </summary>
    public int CheckServerFailCount = 1;
    /// <summary>
    /// 服务器之间的检查间隔
    /// </summary>
    public int CheckServerTick = 0;
    /// <summary>
    /// 网关是否就绪
    /// </summary>
    public bool ConnectState = false;
    /// <summary>
    /// 上次心跳链接时间
    /// </summary>
    public int KeepAliveTick;
    /// <summary>
    /// 历史最高在线人数
    /// </summary>
    private int Counter = 0;
    /// <summary>
    /// 发送总字节数
    /// </summary>
    public int SendBytes;
    /// <summary>
    /// 接收总字节数
    /// </summary>
    public int ReceiveBytes;
}