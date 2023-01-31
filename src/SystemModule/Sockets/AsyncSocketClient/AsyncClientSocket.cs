using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using SystemModule.Sockets.Event;

namespace SystemModule.Sockets.AsyncSocketClient
{
    public class AsyncClientSocket
    {
        /// <summary>
        /// 连接成功事件
        /// </summary>
        public event DSCClientOnConnectedHandler OnConnected;
        /// <summary>
        /// 错误事件
        /// </summary>
        public event DSCClientOnErrorHandler OnError;
        /// <summary>
        /// 接收到数据事件
        /// </summary>
        public event DSCClientOnReceiveHandler OnReceivedData;
        /// <summary>
        /// 数据发送事件
        /// </summary>
        public event DSCClientOnSendHandler OnSendDataCompleted;
        /// <summary>
        /// 断开连接事件
        /// </summary>
        public event DSCClientOnDisconnectedHandler OnDisconnected;
        public bool IsConnected = false;
        /// <summary>
        /// 服务器接收到的总字节数计数器
        /// </summary>
        private long _totalBytesRead;
        /// <summary>
        /// 服务器发送的字节总数
        /// </summary>
        private long _totalBytesWrite;
        /// <summary>
        /// 接受缓存数组
        /// </summary>
        private readonly byte[] recvBuff = null;
        /// <summary>
        /// 发送缓存数组
        /// </summary>
        private readonly byte[] sendBuff = null;
        /// <summary>
        /// 连接socket
        /// </summary>
        private Socket connectSocket = null;
        public IPEndPoint EndPoint;
        public IPEndPoint RemoteEndpoint;
        /// <summary>
        /// 用于发送数据的SocketAsyncEventArgs
        /// </summary>
        private readonly SocketAsyncEventArgs sendEventArg = null;
        /// <summary>
        /// 用于接收数据的SocketAsyncEventArgs
        /// </summary>
        private readonly SocketAsyncEventArgs recvEventArg = null;
        //tcp服务器ip
        public string Host = "";
        //tcp服务器端口
        public int Port = 0;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="bufferSize">用于socket发送和接受的缓存区大小</param>
        public AsyncClientSocket(string ip, int port, int bufferSize)
        {
            if (string.IsNullOrEmpty(ip))
                throw new ArgumentNullException("ip cannot be null");
            if (port < 1 || port > 65535)
                throw new ArgumentOutOfRangeException("port is out of range");

            //设置用于发送数据的SocketAsyncEventArgs
            sendBuff = new byte[bufferSize];
            sendEventArg = new SocketAsyncEventArgs();
            sendEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
            sendEventArg.SetBuffer(sendBuff, 0, bufferSize);
            //设置用于接受数据的SocketAsyncEventArgs
            recvBuff = new byte[bufferSize];
            recvEventArg = new SocketAsyncEventArgs();
            recvEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
            recvEventArg.SetBuffer(recvBuff, 0, bufferSize);
            this.Host = ip;
            this.Port = port;
        }

        /// <summary>
        /// 开启tcp客户端，连接tcp服务器
        /// </summary>
        public void Start()
        {
            try
            {
                if (RemoteEndpoint == null)
                {
                    // 重置接收和发送字节总数
                    _totalBytesRead = 0;
                    _totalBytesWrite = 0;
                    connectSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    IPAddress address = IPAddress.Parse(Host);
                    RemoteEndpoint = new IPEndPoint(address, Port);
                }

                //连接tcp服务器
                SocketAsyncEventArgs connectEventArg = new SocketAsyncEventArgs();
                connectEventArg.Completed += ConnectEventArgs_Completed;
                connectEventArg.RemoteEndPoint = RemoteEndpoint;//设置要连接的tcp服务器地址
                bool willRaiseEvent = connectSocket.ConnectAsync(connectEventArg);
                if (!willRaiseEvent)
                    ProcessConnect(connectEventArg);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 发送数据到tcp服务器
        /// </summary>
        /// <param name="buffer">要发送的数据</param>
        public void Send(byte[] buffer)
        {
            if (buffer.Length <= 0)
                throw new ArgumentNullException("buffer cannot be null");

            if (connectSocket == null)
                throw new Exception("socket cannot be null");

            sendEventArg.SetBuffer(buffer, 0, buffer.Length);
            if (!connectSocket.SendAsync(sendEventArg))
            {
                ProcessSend(sendEventArg);
            }
        }

        /// <summary>
        /// 关闭tcp客户端
        /// </summary>
        public void CloseSocket()
        {
            IsConnected = false;
            RemoteEndpoint = null;
            if (connectSocket == null)
                return;
            try
            {
                //关闭socket时，单独使用socket.close()通常会造成资源提前被释放，应该在关闭socket之前，先使用shutdown进行接受或者发送的禁用，再使用socket进行释放
                connectSocket.Shutdown(SocketShutdown.Both);
            }
            catch
            {
                connectSocket.Close();
            }
        }

        /// <summary>
        /// 重启tcp客户端，重新连接tcp服务器
        /// </summary>
        public void Restart()
        {
            IsConnected = false;
            CloseSocket();
            Start();
        }

        /// <summary>
        /// 服务器发送的字节总数
        /// </summary>
        public long TotalBytesWrite => _totalBytesWrite;
        /// <summary>
        /// 服务器接收到的总字节数计数器
        /// </summary>
        public long TotalBytesRead => _totalBytesRead;
        
        /// <summary>
        /// Socket.ConnectAsync完成回调函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConnectEventArgs_Completed(object sender, SocketAsyncEventArgs e)
        {
            if (e.ConnectByNameError != null)
            {
                if (null != OnError)
                {
                    OnError(e.RemoteEndPoint, new DSCClientErrorEventArgs(e.RemoteEndPoint, e.SocketError, e.ConnectByNameError));
                }
                return;
            }
            if (e.ConnectSocket == null || !e.ConnectSocket.Connected)
            {
                if (null != OnError)
                {
                    OnError(e.RemoteEndPoint, new DSCClientErrorEventArgs(e.RemoteEndPoint, e.SocketError, e.ConnectByNameError));
                }
                return;
            }
            IsConnected = true;
            ProcessConnect(e);
            if (null != OnConnected)
            {
                EndPoint = (IPEndPoint)e.RemoteEndPoint;
                OnConnected(this, new DSCClientConnectedEventArgs(e.ConnectSocket)); //引发连接成功事件
            }
        }

        private void ProcessConnect(SocketAsyncEventArgs e)
        {
            StartWaitingForData();
        }

        /// <summary>
        /// tcp客户端开始接受tcp服务器发送的数据
        /// </summary>
        private void StartWaitingForData()
        {
            if (!connectSocket.ReceiveAsync(recvEventArg))
            {
                ProcessReceive(recvEventArg);
            }
        }

        /// <summary>
        /// socket.sendAsync和socket.recvAsync的完成回调函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IO_Completed(object sender, SocketAsyncEventArgs e)
        {
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Receive:
                    ProcessReceive(e);
                    break;
                case SocketAsyncOperation.Send:
                    ProcessSend(e);
                    break;
                default:
                    throw new ArgumentException("The last operation completed on the socket was not a receive or send");
            }
        }

        /// <summary>
        /// 处理接受到的tcp服务器数据
        /// </summary>
        /// <param name="e"></param>
        private void ProcessReceive(SocketAsyncEventArgs e)
        {
            try
            {
                AsyncUserToken token = (AsyncUserToken)e.UserToken;
                // 增加发送计数器
                Interlocked.Add(ref _totalBytesRead, e.BytesTransferred);
                if (0 == e.BytesTransferred)
                {
                    RaiseDisconnectedEvent(e.ConnectSocket);//引发断开连接事件
                    return;
                }
                if (e.BytesTransferred > 0 && e.SocketError == SocketError.Success)
                {
                    OnReceivedData?.Invoke(this, new DSCClientDataInEventArgs(e.ConnectSocket, e.Buffer, e.BytesTransferred)); //引发接收数据事件
                    StartWaitingForData();//继续接收数据
                }
            }
            catch (ObjectDisposedException)
            {
                RaiseDisconnectedEvent(e.ConnectSocket);//引发断开连接事件
            }
            catch (SocketException exception)
            {
                if (exception.ErrorCode == (int)SocketError.ConnectionReset)
                {
                    RaiseDisconnectedEvent(e.ConnectSocket);//引发断开连接事件
                    return;
                }
                RaiseErrorEvent(exception);//引发错误事件
            }
        }

        /// <summary>
        /// 处理tcp客户端发送的结果
        /// </summary>
        /// <param name="e"></param>
        private void ProcessSend(SocketAsyncEventArgs e)
        {
            try
            {
                // 增加发送到的字节总数
                Interlocked.Add(ref _totalBytesWrite, e.BytesTransferred);
                if (e.SocketError == SocketError.Success)
                {
                    OnSendDataCompleted?.Invoke(this, new DSCClientSendDataEventArgs(e.ConnectSocket, e.BytesTransferred));
                }
                else
                {
                    OnSendDataCompleted?.Invoke(this, new DSCClientSendDataEventArgs(e.ConnectSocket, -1));
                }
            }
            catch (ObjectDisposedException)
            {
                RaiseDisconnectedEvent(e.ConnectSocket);
            }
            catch (SocketException exception)
            {
                if (exception.ErrorCode == (int)SocketError.ConnectionReset)
                {
                    RaiseDisconnectedEvent(e.ConnectSocket);//引发断开连接事件
                    return;
                }
                RaiseErrorEvent(exception);
            }
            catch (Exception exception_debug)
            {
                Debug.WriteLine("调试：" + exception_debug.Message);
            }
        }

        private void RaiseErrorEvent(SocketException error)
        {
            if (null != OnError)
            {
                OnError(this, new DSCClientErrorEventArgs(EndPoint, error.SocketErrorCode, error));
            }
        }

        private void RaiseDisconnectedEvent(Socket socket)
        {
            IsConnected = false;
            if (null != OnDisconnected)
            {
                OnDisconnected(this, new DSCClientConnectedEventArgs(socket));
            }
        }
    }
}