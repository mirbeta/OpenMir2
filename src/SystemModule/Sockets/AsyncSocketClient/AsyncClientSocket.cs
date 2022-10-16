using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
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
        public event DSCClientOnDataInHandler ReceivedDatagram;
        /// <summary>
        /// 断开连接事件
        /// </summary>
        public event DSCClientOnDisconnectedHandler OnDisconnected;

        /// <summary>
        /// 接受缓存数组
        /// </summary>
        private byte[] recvBuff = null;
        /// <summary>
        /// 发送缓存数组
        /// </summary>
        private byte[] sendBuff = null;
        /// <summary>
        /// 连接socket
        /// </summary>
        private Socket connectSocket = null;
        private IPEndPoint EndPoint;
        /// <summary>
        /// 用于发送数据的SocketAsyncEventArgs
        /// </summary>
        private SocketAsyncEventArgs sendEventArg = null;
        /// <summary>
        /// 用于接收数据的SocketAsyncEventArgs
        /// </summary>
        private SocketAsyncEventArgs recvEventArg = null;
        //tcp服务器ip
        private string ip = "";
        //tcp服务器端口
        private int port = 0;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="bufferSize">用于socket发送和接受的缓存区大小</param>
        public AsyncClientSocket(int bufferSize)
        {
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
        }

        /// <summary>
        /// 开启tcp客户端，连接tcp服务器
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        public void Start(string ip, int port)
        {
            if (string.IsNullOrEmpty(ip))
                throw new ArgumentNullException("ip cannot be null");
            if (port < 1 || port > 65535)
                throw new ArgumentOutOfRangeException("port is out of range");

            this.ip = ip;
            this.port = port;

            try
            {
                connectSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress address = IPAddress.Parse(ip);
                IPEndPoint endpoint = new IPEndPoint(address, port);

                //连接tcp服务器
                SocketAsyncEventArgs connectEventArg = new SocketAsyncEventArgs();
                connectEventArg.Completed += ConnectEventArgs_Completed;
                connectEventArg.RemoteEndPoint = endpoint;//设置要连接的tcp服务器地址
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
        /// <param name="message">要发送的数据</param>
        public void SendMessage(Memory<byte> buffer)
        {
            if (buffer.Length <= 0)
                throw new ArgumentNullException("buffer cannot be null");

            if (connectSocket == null)
                throw new Exception("socket cannot be null");

            sendEventArg.SetBuffer(buffer);
            bool willRaiseEvent = connectSocket.SendAsync(sendEventArg);
            if (!willRaiseEvent)
            {
                ProcessSend(sendEventArg);
            }
        }

        /// <summary>
        /// 关闭tcp客户端
        /// </summary>
        public void CloseSocket()
        {
            if (connectSocket == null)
                return;

            try
            {
                //关闭socket时，单独使用socket.close()通常会造成资源提前被释放，应该在关闭socket之前，先使用shutdown进行接受或者发送的禁用，再使用socket进行释放
                connectSocket.Shutdown(SocketShutdown.Both);
            }
            catch { }

            try
            {
                connectSocket.Close();
            }
            catch { }
        }

        /// <summary>
        /// 重启tcp客户端，重新连接tcp服务器
        /// </summary>
        public void Restart()
        {
            CloseSocket();
            Start(ip, port);
        }

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
                    OnError(e.RemoteEndPoint, new DSCClientErrorEventArgs(e.RemoteEndPoint, (int)e.SocketError, e.ConnectByNameError));
                }
                return;
            }
            ProcessConnect(e);
            if (null != OnConnected)
            {
                EndPoint = (IPEndPoint)e.RemoteEndPoint;
                OnConnected(this, new DSCClientConnectedEventArgs(e.ConnectSocket)); //引发连接成功事件
            }
        }

        private void ProcessConnect(SocketAsyncEventArgs e)
        {
            StartRecv();
        }

        /// <summary>
        /// tcp客户端开始接受tcp服务器发送的数据
        /// </summary>
        private void StartRecv()
        {
            bool willRaiseEvent = connectSocket.ReceiveAsync(recvEventArg);
            if (!willRaiseEvent)
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
            AsyncUserToken token = (AsyncUserToken)e.UserToken;
            if (e.BytesTransferred > 0 && e.SocketError == SocketError.Success)
            {
                ReceivedDatagram?.Invoke(this, new DSCClientDataInEventArgs(e.ConnectSocket, e.Buffer, e.BytesTransferred)); //引发接收数据事件
                StartRecv();
            }
            else
            {
                Restart();
            }
        }

        /// <summary>
        /// 处理tcp客户端发送的结果
        /// </summary>
        /// <param name="e"></param>
        private void ProcessSend(SocketAsyncEventArgs e)
        {
            AsyncUserToken token = (AsyncUserToken)e.UserToken;
            if (e.SocketError == SocketError.Success)
            {
                if (sendResultEvent != null)
                    sendResultEvent(e.BytesTransferred);
            }
            else
            {
                if (sendResultEvent != null)
                    sendResultEvent(-1);
                Restart();
            }
        }
    }
}