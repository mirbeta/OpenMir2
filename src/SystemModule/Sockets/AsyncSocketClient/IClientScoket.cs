using System;
using System.Buffers;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using SystemModule.Sockets.Event;

namespace SystemModule.Sockets.AsyncSocketClient
{
    public class ClientScoket
    {
        /// <summary>
        /// 缓冲区大小
        /// </summary>
        private const int Buffersize = 1024;
        /// <summary>
        /// 客户端Socket
        /// </summary>
        private Socket _cli = null;
        /// <summary>
        /// 缓冲区
        /// </summary>
        private readonly byte[] _databuffer;
        /// <summary>
        /// 连接是否成功
        /// </summary>
        public bool IsConnected;
        public string Host = string.Empty;
        public int Port = 0;
        public bool IsBusy = false;
        public IPEndPoint EndPoint;
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
        public event DSCClientOnReceiveHandler ReceivedDatagram;
        /// <summary>
        /// 断开连接事件
        /// </summary>
        public event DSCClientOnDisconnectedHandler OnDisconnected;

        public ClientScoket()
        {
            _databuffer = new byte[Buffersize];
        }

        public void Connect()
        {
            if (!string.IsNullOrEmpty(Host) && Port > 0)
            {
                Connect(Host, Port);
                return;
            }
            throw new Exception("IP地址或端口号错误");
        }

        public void Connect(IPEndPoint endPoint)//连接到终结点
        {
            try
            {
                _cli = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IsBusy = true;
                Host = endPoint.Address.ToString();
                Port = endPoint.Port;
                EndPoint = endPoint;
                _cli.BeginConnect(endPoint, HandleConnect, _cli);//开始异步连接
            }
            catch (ObjectDisposedException)
            {
                RaiseDisconnectedEvent();
            }
            catch (SocketException exception)
            {
                if (exception.ErrorCode == (int)SocketError.ConnectionReset)
                {
                    RaiseDisconnectedEvent();//引发断开连接事件
                }
                RaiseErrorEvent(exception);//引发错误事件
            }
        }
        
        public void Connect(string ip, int port)//连接到终结点
        {
            try
            {
                this._cli = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                EndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
                IsBusy = true;
                _cli.BeginConnect(EndPoint, HandleConnect, _cli);//开始异步连接
            }
            catch (ObjectDisposedException)
            {
                RaiseDisconnectedEvent();
            }
            catch (SocketException exception)
            {
                if (exception.ErrorCode == (int)SocketError.ConnectionReset)
                {
                    RaiseDisconnectedEvent();//引发断开连接事件
                }
                RaiseErrorEvent(exception);//引发错误事件
            }
        }

        private void HandleConnect(IAsyncResult iar)
        {
            Socket asyncState = (Socket)iar.AsyncState;
            try
            {
                asyncState.EndConnect(iar); //结束异步连接
                if (null != OnConnected)
                {
                    IsConnected = true;
                    EndPoint = (IPEndPoint)_cli.RemoteEndPoint;
                    OnConnected(this, new DSCClientConnectedEventArgs(_cli)); //引发连接成功事件
                }
                StartWaitingForData(asyncState); //开始接收数据
            }
            catch (ObjectDisposedException)
            {
                RaiseDisconnectedEvent();
                IsConnected = false;
            }
            catch (SocketException exception)
            {
                if (exception.ErrorCode == (int)SocketError.ConnectionReset)
                {
                    RaiseDisconnectedEvent(); //引发断开连接事件
                }
                RaiseErrorEvent(exception); //引发错误事件
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void StartWaitingForData(Socket soc)
        {
            try
            {
                //开始异步接收数据
                soc.BeginReceive(_databuffer, 0, Buffersize, SocketFlags.None, HandleIncomingData, soc);
            }
            catch (ObjectDisposedException)
            {
                RaiseDisconnectedEvent();
            }
            catch (SocketException exception)
            {
                if (exception.ErrorCode == (int)SocketError.ConnectionReset)
                {
                    RaiseDisconnectedEvent();//引发断开连接事件
                }
                RaiseErrorEvent(exception);//引发错误事件
            }
        }

        private void HandleIncomingData(IAsyncResult parameter)
        {
            var asyncState = (Socket)parameter.AsyncState;
            try
            {
                var length = asyncState.EndReceive(parameter);//结束异步接收数据
                if (0 == length)
                {
                    RaiseDisconnectedEvent();//引发断开连接事件
                }
                else
                {
                    Span<byte> destinationArray = stackalloc byte[length];//目的字节数组
                    for (var i = 0; i < length; i++)
                    {
                        destinationArray[i] = _databuffer[i];
                    }
                    ReceivedDatagram?.Invoke(this, new DSCClientDataInEventArgs(_cli, destinationArray.ToArray(), length)); //引发接收数据事件
                    StartWaitingForData(asyncState);//继续接收数据
                }
            }
            catch (ObjectDisposedException)
            {
                RaiseDisconnectedEvent();//引发断开连接事件
            }
            catch (SocketException exception)
            {
                if (exception.ErrorCode == (int)SocketError.ConnectionReset)
                {
                    RaiseDisconnectedEvent();//引发断开连接事件
                }
                RaiseErrorEvent(exception);//引发错误事件
            }
        }

        public void SendText(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return;
            }
            var buffer = System.Text.Encoding.GetEncoding("gb2312").GetBytes(str);
            Send(buffer);
        }
        
        public void Send(byte[] buffer)
        {
            try
            {
                //开始异步发送数据
                _cli.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, HandleSendFinished, _cli);
            }
            catch (ObjectDisposedException)
            {
                RaiseDisconnectedEvent();//引发断开连接事件
            }
            catch (SocketException exception)
            {
                if (exception.ErrorCode == (int)SocketError.ConnectionReset)
                {
                    RaiseDisconnectedEvent();//引发断开连接事件
                }
                RaiseErrorEvent(exception);//引发错误事件
            }
        }

        private void HandleSendFinished(IAsyncResult parameter)
        {
            try
            {
                ((Socket)parameter.AsyncState).EndSend(parameter);//结束异步发送数据
            }
            catch (ObjectDisposedException)
            {
                RaiseDisconnectedEvent();
            }
            catch (SocketException exception)
            {
                if (exception.ErrorCode == (int)SocketError.ConnectionReset)
                {
                    RaiseDisconnectedEvent();//引发断开连接事件
                }
                RaiseErrorEvent(exception);
            }
            catch (Exception exception_debug)
            {
                Debug.WriteLine("调试：" + exception_debug.Message);
            }
        }

        private void RaiseDisconnectedEvent()
        {
            IsConnected = false;
            if (null != OnDisconnected)
            {
                OnDisconnected(this, new DSCClientConnectedEventArgs(_cli));
            }
        }

        private void RaiseErrorEvent(SocketException error)
        {
            if (null != OnError)
            {
                OnError(_cli.RemoteEndPoint, new DSCClientErrorEventArgs(_cli.RemoteEndPoint, error.SocketErrorCode, error));
            }
        }

        public void SendBuffer(byte[] data)
        {
            Send(data);
        }

        public void Close()
        {
            if (_cli != null)
            {
                _cli.Shutdown(SocketShutdown.Both);
                _cli.Disconnect(true);//scoket 复用
                _cli.Close();
            }
        }

        public void Disconnect()
        {
            try
            {
                if (_cli != null)
                {
                    _cli.Shutdown(SocketShutdown.Both);
                    _cli.Disconnect(true);//scoket 复用
                    _cli.Close();
                }
            }
            catch (Exception) { }
        }
    }
}