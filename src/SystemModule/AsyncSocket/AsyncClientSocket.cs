using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SystemModule.AsyncSocket
{
    public class AsyncClientSocket : IOCPBase, IDisposable
    {
        #region Fields

        /// <summary>
        /// 连接服务器的socket
        /// </summary>
        private Socket _clientSocket;

        /// <summary>
        /// 服务器监听地址
        /// </summary>
        private readonly IPEndPoint _remoteEndPoint;

        /// <summary>
        /// 本地监听地址
        /// </summary>
        private readonly IPEndPoint _localEndPoint;

        /// <summary>  
        /// 用于每个I/O Socket操作的缓冲区大小  
        /// </summary>  
        private int _bufferSize = 1024;

        /// <summary>
        /// 连接超时
        /// </summary>
        private readonly int _timeOut;

        private int _state;

        private const int _none = 0;

        private const int _connecting = 1;

        private const int _connected = 2;

        private const int _disconnecting = 3;

        private const int _disconnected = 4;

        private const int _closed = 5;
        /// <summary>  
        /// 缓冲区管理  
        /// </summary>  
        private readonly BufferManager _bufferManager;

        /// <summary>  
        /// 对象池  
        /// </summary>  
        private readonly UserTokenPool _objectPool;
        private const int opsToPreAlloc = 2;

        private readonly int _maxClient = 10;

        #endregion

        #region Properties

        public bool Connected
        {
            get
            {
                return _clientSocket != null && _clientSocket.Connected;
            }
        }

        public IPEndPoint RemoteEndPoint
        {
            get
            {
                return Connected ? (IPEndPoint)_clientSocket.RemoteEndPoint : _remoteEndPoint;
            }
        }

        public IPEndPoint LocalEndPoint
        {
            get
            {
                return Connected ? (IPEndPoint)_clientSocket.LocalEndPoint : _localEndPoint;
            }
        }

        public Encoding Encoding { get; set; }

        public override int BufferSize
        {
            get
            {
                return _bufferSize;
            }

            protected set
            {
                if (_bufferSize != value && value > 0)
                {
                    _bufferSize = value;
                }
            }
        }
        public Socket ConnectSocket
        {
            get
            {
                return _clientSocket;
            }
        }

        #endregion

        #region Ctors

        public AsyncClientSocket(IPEndPoint remote) : this(null, remote, Encoding.UTF8)
        {

        }

        public AsyncClientSocket(IPEndPoint local, IPEndPoint remote, Encoding encoding, int timeOut = 120)
        {
            if (remote == null)
                throw new ArgumentNullException("remote");
            _remoteEndPoint = remote;
            _localEndPoint = local;
            this.Encoding = encoding;
            this._timeOut = timeOut;
            _bufferManager = new BufferManager(_bufferSize * _maxClient * opsToPreAlloc, _bufferSize);
            _objectPool = new UserTokenPool();
            Init();
        }

        #endregion

        #region Init

        private void Init()
        {
            // Allocates one large byte buffer which all I/O operations use a piece of.  This gaurds   
            // against memory fragmentation  
            _bufferManager.InitBuffer();

            // preallocate pool of SocketAsyncEventArgs objects  
            SocketAsyncEventArgs readWriteEventArg;
            UserToken userToken;
            for (int i = 0; i < _maxClient; i++)
            {
                //Pre-allocate a set of reusable SocketAsyncEventArgs  
                readWriteEventArg = new SocketAsyncEventArgs();
                readWriteEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(OnIOCompleted);

                // assign a byte buffer from the buffer pool to the SocketAsyncEventArg object  
                _bufferManager.SetBuffer(readWriteEventArg);
                userToken = new UserToken();
                userToken.ReceiveArgs = readWriteEventArg;
                readWriteEventArg.UserToken = userToken;
                // add SocketAsyncEventArg to the pool  
                _objectPool.Push(userToken);
            }
        }

        #endregion

        #region Async Function

        #region Connect

        public Task<SocketResult> ConnectAsync(int timeOut = -1)
        {
            int origin = Interlocked.Exchange(ref _state, _connecting);
            if (!(origin == _none || origin == _closed))
            {
                CloseClientSocket(null);
                throw new InvalidOperationException("This tcp socket client is in invalid state when connecting.");
            }
            _clientSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            SetSocketOptions(_clientSocket);
            if (_localEndPoint != null)
            {
                _clientSocket.Bind(_localEndPoint);
            }
            var userToken = _objectPool.Pop();
            userToken.ConnectSocket = _clientSocket;
            userToken.ReceiveArgs.RemoteEndPoint = _remoteEndPoint;
            userToken.TimeOut = timeOut;
            userToken.ReceiveArgs.SetBuffer(userToken.ReceiveArgs.Offset, 0);
            return _clientSocket.ConnectAsync(this, userToken)
                .ContinueWith(t =>
                {
                    if (Interlocked.CompareExchange(ref _state, _connected, _connecting) != _connecting)
                    {
                        CloseClientSocket(null);
                        throw new InvalidOperationException("This tcp socket client is in invalid state when connected.");
                    }
                    if (t.Result.SocketError != SocketError.Success)
                    {
                        CloseClientSocket(null);
                    }
                    return t.Result;
                }, TaskContinuationOptions.ExecuteSynchronously);
        }

        #endregion

        #region Disconnect

        public override Task<SocketResult> DisconnectAsync(Socket socket, int timeOut = -1)
        {
            int origin = Interlocked.Exchange(ref _state, _disconnecting);
            if (origin != _connected)
            {
                throw new InvalidOperationException("This tcp socket client is in invalid state when disconnecting.");
            }
            var userToken = _objectPool.Pop();
            userToken.ConnectSocket = socket;
            userToken.TimeOut = timeOut;
            userToken.ReceiveArgs.SetBuffer(userToken.ReceiveArgs.Offset, 0);
            return socket.DisconnectAsync(this, userToken)
                .ContinueWith(t =>
                {
                    if (Interlocked.CompareExchange(ref _state, _disconnected, _disconnecting) != _disconnecting)
                    {
                        throw new InvalidOperationException("This tcp socket client is in invalid state when disconnected");
                    }
                    return t.Result;
                }, TaskContinuationOptions.ExecuteSynchronously);
        }

        #endregion

        #endregion

        #region Close

        public override void CloseClientSocket(Socket socket)
        {
            if (Interlocked.Exchange(ref _state, _closed) == _closed)
            {
                return;
            }
            if (socket == null)
            {
                CleanClientSocket();
                return;
            }
            if (socket.IsDisposed())
                return;
            if (socket != null && socket.Connected)
            {
                socket.Shutdown(SocketShutdown.Both);
            }
            if (socket != null)
            {
                socket.Close();
            }
        }

        private void CleanClientSocket()
        {
            if (_clientSocket != null && _clientSocket.Connected)
            {
                _clientSocket.Shutdown(SocketShutdown.Both);
            }
            if (_clientSocket != null)
            {
                _clientSocket.Close();
                _clientSocket = null;
            }
        }

        protected override void RecycleToken(UserToken token)
        {
            _bufferManager.ResetBuffer(token);
            _objectPool.Reture(token);
        }

        #endregion

        #region IDisposable Support

        private bool disposedValue = false;// 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _clientSocket?.Close();
                    _objectPool.Clear();
                }
                disposedValue = true;
            }
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public override Task<SocketResult> ReceiveAsync(Socket socket, int timeOut = -1)
        {
            if (socket == null || !socket.Connected)
                return Task.FromResult(SocketResult.NotSocket);
            var userToken = _objectPool.Pop();
            userToken.ConnectSocket = socket;
            userToken.ReceiveArgs.RemoteEndPoint = _remoteEndPoint;
            userToken.TimeOut = timeOut;
            return socket.ReceiveAsync(this, userToken);
        }

        public override Task<SocketResult> SendAsync(Socket socket, byte[] data, int timeOut = -1)
        {
            if (socket == null || !socket.Connected)
                return Task.FromResult(SocketResult.NotSocket);
            var userToken = _objectPool.Pop();
            userToken.ConnectSocket = socket;
            userToken.ReceiveArgs.RemoteEndPoint = _remoteEndPoint;
            userToken.TimeOut = timeOut;
            return socket.SendAsync(data, this, userToken);
        }

        public override Task<SocketResult> SendFileAsync(Socket socket, string fileName, int timeOut = -1)
        {
            if (socket == null || !socket.Connected)
                return Task.FromResult(SocketResult.NotSocket);
            var userToken = _objectPool.Pop();
            userToken.ConnectSocket = socket;
            userToken.ReceiveArgs.RemoteEndPoint = _remoteEndPoint;
            userToken.TimeOut = timeOut;
            return socket.SendFileAsync(fileName, this, userToken);
        }

        #endregion
    }
}