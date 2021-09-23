using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace SystemModule.Sockets
{
    public class IClientScoket
    {
        private readonly int buffersize = 1024;//缓冲区大小
        private Socket cli = null;//客户端Socket
        private readonly byte[] databuffer;//缓冲区
        public bool IsConnected;//连接是否成功
        public string Address = string.Empty;
        public int Port = 0;
        public bool IsBusy = false;

        public event DSCClientOnConnectedHandler OnConnected;//连接成功事件

        public event DSCClientOnErrorHandler OnError;//错误事件

        public event DSCClientOnDataInHandler ReceivedDatagram;//接收到数据事件

        public event DSCClientOnDisconnectedHandler OnDisconnected;//断开连接事件

        public IClientScoket()
        {
            this.databuffer = new byte[this.buffersize];//创建缓冲区
        }

        public void Connect()
        {
            if (!string.IsNullOrEmpty(Address) && Port > 0)
            {
                Connect(Address, Port);
            }
            else
            {
                new Exception("IP地址或端口号错误");
            }
        }

        public void Connect(string ip, int port)//连接到终结点
        {
            this.cli = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint remoteEP = new IPEndPoint(IPAddress.Parse(ip), port);
            try
            {
                this.cli.BeginConnect(remoteEP, this.HandleConnect, this.cli);//开始异步连接
            }
            catch (ObjectDisposedException)
            {
                RaiseDisconnectedEvent();
            }
            catch (SocketException exception)
            {
                if (exception.ErrorCode == (int)SocketError.ConnectionReset)
                {
                    this.RaiseDisconnectedEvent();//引发断开连接事件
                }
                this.RaiseErrorEvent(exception);//引发错误事件
            }
        }

        private void HandleConnect(IAsyncResult iar)
        {
            Socket asyncState = (Socket)iar.AsyncState;
            try
            {
                asyncState.EndConnect(iar);//结束异步连接
                if (null != this.OnConnected)
                {
                    this.OnConnected(this, new DSCClientConnectedEventArgs(this.cli));//引发连接成功事件
                    IsConnected = true;
                }
                this.StartWaitingForData(asyncState);//开始接收数据
            }
            catch (ObjectDisposedException ex)
            {
                RaiseDisconnectedEvent();
                IsConnected = false;
            }
            catch (SocketException exception)
            {
                if (exception.ErrorCode == (int)SocketError.ConnectionReset)
                {
                    this.RaiseDisconnectedEvent();//引发断开连接事件
                }
                this.RaiseErrorEvent(exception);//引发错误事件
            }
        }

        private void StartWaitingForData(Socket soc)
        {
            try
            {
                //开始异步接收数据
                soc.BeginReceive(this.databuffer, 0, this.buffersize, SocketFlags.None, this.HandleIncomingData, soc);
            }
            catch (ObjectDisposedException)
            {
                RaiseDisconnectedEvent();
            }
            catch (SocketException exception)
            {
                if (exception.ErrorCode == (int)SocketError.ConnectionReset)
                {
                    this.RaiseDisconnectedEvent();//引发断开连接事件
                }
                this.RaiseErrorEvent(exception);//引发错误事件
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
                    this.RaiseDisconnectedEvent();//引发断开连接事件
                }
                else
                {
                    var destinationArray = new byte[length];//目的字节数组
                    Buffer.BlockCopy(this.databuffer, 0, destinationArray, 0, length);
                    ReceivedDatagram?.Invoke(this, new DSCClientDataInEventArgs(this.cli, destinationArray)); //引发接收数据事件
                    this.StartWaitingForData(asyncState);//继续接收数据
                }
            }
            catch (ObjectDisposedException)
            {
                this.RaiseDisconnectedEvent();//引发断开连接事件
            }
            catch (SocketException exception)
            {
                if (exception.ErrorCode == (int)SocketError.ConnectionReset)
                {
                    this.RaiseDisconnectedEvent();//引发断开连接事件
                }
                this.RaiseErrorEvent(exception);//引发错误事件
            }
        }

        public void SendText(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return;
            }
            var buffer = System.Text.Encoding.GetEncoding("gb2312").GetBytes(str);
            try
            {
                //开始异步发送数据
                this.cli.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, this.HandleSendFinished, this.cli);
            }
            catch (ObjectDisposedException)
            {
                this.RaiseDisconnectedEvent();//引发断开连接事件
            }
            catch (SocketException exception)
            {
                if (exception.ErrorCode == (int)SocketError.ConnectionReset)
                {
                    this.RaiseDisconnectedEvent();//引发断开连接事件
                }
                this.RaiseErrorEvent(exception);//引发错误事件
            }
        }

        public void Send(byte[] buffer)
        {
            try
            {
                //开始异步发送数据
                this.cli.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, this.HandleSendFinished, this.cli);
            }
            catch (ObjectDisposedException)
            {
                this.RaiseDisconnectedEvent();//引发断开连接事件
            }
            catch (SocketException exception)
            {
                if (exception.ErrorCode == (int)SocketError.ConnectionReset)
                {
                    this.RaiseDisconnectedEvent();//引发断开连接事件
                }
                this.RaiseErrorEvent(exception);//引发错误事件
            }
        }

        public void SendBuff(byte[] buffer)
        {
            this.cli.Send(buffer);
        }

        private void HandleSendFinished(IAsyncResult parameter)
        {
            try
            {
                ((Socket)parameter.AsyncState).EndSend(parameter);//结束异步发送数据
            }
            catch (ObjectDisposedException)
            {
                this.RaiseDisconnectedEvent();
            }
            catch (SocketException exception)
            {
                if (exception.ErrorCode == (int)SocketError.ConnectionReset)
                {
                    this.RaiseDisconnectedEvent();//引发断开连接事件
                }
                this.RaiseErrorEvent(exception);
            }
            catch (Exception exception_debug)
            {
                Debug.WriteLine("调试：" + exception_debug.Message);
            }
        }

        private void RaiseDisconnectedEvent()
        {
            if (null != this.OnDisconnected)
            {
                this.OnDisconnected(this, new DSCClientConnectedEventArgs(this.cli));
            }
            IsConnected = false;
        }

        private void RaiseErrorEvent(SocketException error)
        {
            if (null != this.OnError)
            {
                this.OnError(this.cli.RemoteEndPoint, new DSCClientErrorEventArgs(Address, Port, error));
            }
        }

        public void Disconnect()
        {
            try
            {
                this.cli.Shutdown(SocketShutdown.Both);
                this.cli.Close();
            }
            catch (Exception) { }
        }
    }
}