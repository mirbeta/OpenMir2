using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace SystemModule.Sockets.AsyncSocketServer
{
    /// <summary>
    /// 异步Socket通讯服务器类
    /// </summary>
    public class SocketServer
    {
        /// <summary>
        /// 缓冲区同步对象
        /// </summary>
        private readonly object _bufferLock = new object();
        /// <summary>
        /// 读对象池同步对象
        /// </summary>
        private readonly object _readPoolLock = new object();
        /// <summary>
        /// 写对象池同步对象
        /// </summary>
        private readonly object _writePoolLock = new object();

        /// <summary>
        /// 接收数据事件对象集合
        /// </summary>
        private readonly ConcurrentDictionary<string, AsyncUserToken> _mTokens;
        /// <summary>
        /// 设计同时处理的连接最大数
        /// </summary>
        private readonly int _numConnections;
        /// <summary>
        /// 用于每一个Socket I/O操作使用的缓冲区大小
        /// </summary>
        private readonly int _bufferSize;
        /// <summary>
        /// 为所有Socket操作准备的一个大的可重用的缓冲区集合
        /// </summary>
        private readonly BufferManager _bufferManager;
        /// <summary>
        /// 需要分配空间的操作数目:为 读、写、接收等 操作预分配空间(接受操作可以不分配)
        /// </summary>
        private const int OpsToPreAlloc = 2;
        /// <summary>
        /// 用来侦听到达的连接请求的Socket
        /// </summary>
        private Socket _listenSocket;
        /// <summary>
        /// 读Socket操作的SocketAsyncEventArgs可重用对象池
        /// </summary>
        private readonly SocketAsyncEventArgsPool _readPool;
        /// <summary>
        /// 写Socket操作的SocketAsyncEventArgs可重用对象池
        /// </summary>
        private readonly SocketAsyncEventArgsPool _writePool;
        private bool _isActive = false;
        /// <summary>
        /// 服务器接收到的总字节数计数器
        /// </summary>
        private long _totalBytesRead;
        /// <summary>
        /// 服务器发送的字节总数
        /// </summary>
        private long _totalBytesWrite;
        /// <summary>
        /// 连接到服务器的Socket总数
        /// </summary>
        private readonly long _numConnectedSockets;
        /// <summary>
        /// 最大接受请求数信号量
        /// </summary>
        private readonly Semaphore _maxNumberAcceptedClients;
        private readonly ReaderWriterLock rwl = new ReaderWriterLock();
        /// <summary>
        /// 获取已经连接的Socket总数
        /// </summary>
        public long NumConnectedSockets
        {
            get { return _numConnectedSockets; }
        }
        /// <summary>
        /// 获取接收到的字节总数
        /// </summary>
        public long TotalBytesRead
        {
            get { return _totalBytesRead; }
        }
        /// <summary>
        /// 获取发送的字节总数
        /// </summary>
        public long TotalBytesWrite
        {
            get { return _totalBytesWrite; }
        }
        /// <summary>
        /// 客户端已经连接事件
        /// </summary>
        public event EventHandler<AsyncUserToken> OnClientConnect;
        /// <summary>
        /// 客户端错误事件
        /// </summary>
        public event EventHandler<AsyncSocketErrorEventArgs> OnClientError;
        /// <summary>
        /// 接收到数据事件
        /// </summary>
        public event EventHandler<AsyncUserToken> OnClientRead;
        /// <summary>
        /// 数据发送完成
        /// </summary>
        public event EventHandler<AsyncUserToken> OnDataSendCompleted;
        /// <summary>
        /// 客户端断开连接事件
        /// </summary>
        public event EventHandler<AsyncUserToken> OnClientDisconnect;

        /// <summary>
        /// 客户端是否在线
        /// </summary>
        /// <param name="connectionId"></param>
        /// <returns>在线返回true，否则返回false</returns>
        public bool IsOnline(string connectionId)
        {
            if (!this._mTokens.ContainsKey(connectionId))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public IList<AsyncUserToken> GetSockets()
        {
            return this._mTokens.Values.ToList();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="numConnections">服务器允许连接的客户端总数</param>
        /// <param name="bufferSize">接收缓冲区大小</param>
        public SocketServer(int numConnections, int bufferSize)//构造函数
        {
            // 重置接收和发送字节总数
            _totalBytesRead = 0;
            _totalBytesWrite = 0;
            _numConnectedSockets = 0;
            // 数据库设计连接容量
            this._numConnections = numConnections;
            // 接收缓冲区大小
            this._bufferSize = bufferSize;

            // 为最大数目Socket 同时能拥有高性能的读、写通讯表现而分配缓冲区空间

            _bufferManager = new BufferManager(bufferSize * numConnections * OpsToPreAlloc, bufferSize);

            // 读写池
            _readPool = new SocketAsyncEventArgsPool(numConnections);
            _writePool = new SocketAsyncEventArgsPool(numConnections);

            // 接收数据事件参数对象集合
            _mTokens = new ConcurrentDictionary<string, AsyncUserToken>();

            // 初始信号量
            _maxNumberAcceptedClients = new Semaphore(numConnections, numConnections);
        }

        /// <summary>
        /// 用预分配的可重用缓冲区和上下文对象初始化服务器.        
        /// </summary>
        public void Init()
        {
            // 为所有的I/O操作分配一个大的字节缓冲区.目的是防止内存碎片的产生             
            _bufferManager.InitBuffer();

            // 可重用的SocketAsyncEventArgs,用于重复接受客户端使用
            SocketAsyncEventArgs readWriteEventArg;
            AsyncUserToken token;
            // 预分配一个 SocketAsyncEventArgs 对象池

            // 预分配SocketAsyncEventArgs读对象池
            for (int i = 0; i < _numConnections; i++)
            {
                // 预分配一个 可重用的SocketAsyncEventArgs 对象
                //readWriteEventArg = new SocketAsyncEventArgs();
                // 创建token时同时创建了一个和token关联的用于读数据的SocketAsyncEventArgs对象,
                // 并且创建的SocketAsyncEventArgs对象的UserToken属性值为该token
                token = new AsyncUserToken();
                //token.ReadEventArgs = readWriteEventArg;
                //readWriteEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(IO_Completed);
                // 注册一个SocketAsyncEventArgs完成事件
                token.ReadEventArgs.Completed += IO_Completed;
                //readWriteEventArg.UserToken = new AsyncUserToken();

                // 从缓冲区池中分配一个字节缓冲区给 SocketAsyncEventArg 对象
                //m_bufferManager.SetBuffer(readWriteEventArg);
                _bufferManager.SetBuffer(token.ReadEventArgs);
                //((AsyncUserToken)readWriteEventArg.UserToken).SetReceivedBytes(readWriteEventArg.Buffer, readWriteEventArg.Offset, 0);
                //设定接收缓冲区及偏移量
                token.SetBuffer(token.ReadEventArgs.Buffer, token.ReadEventArgs.Offset, token.ReadEventArgs.Count);
                // 添加一个 SocketAsyncEventArg 到池中
                //m_readPool.Push(readWriteEventArg);
                _readPool.Push(token.ReadEventArgs);
            }
            // 预分配SocketAsyncEventArgs写对象池
            for (int i = 0; i < _numConnections * 2; i++)
            {
                // 预分配一个 可重用的SocketAsyncEventArgs 对象
                readWriteEventArg = new SocketAsyncEventArgs();
                readWriteEventArg.Completed += IO_Completed;
                readWriteEventArg.UserToken = null;

                // 从缓冲区池中分配一个字节缓冲区给 SocketAsyncEventArg 对象
                _bufferManager.SetBuffer(readWriteEventArg);
                //readWriteEventArg.SetBuffer(null, 0, 0);

                // 添加一个 SocketAsyncEventArg 到池中
                _writePool.Push(readWriteEventArg);
            }
        }

        /// <summary>
        /// 启动异步Socket服务
        /// </summary>
        /// <param name="endPoint"></param>
        public void Start(IPEndPoint endPoint)
        {
            StartSocket(endPoint);
            _isActive = true;
        }

        /// <summary>
        /// 启动异步Socket服务
        /// 支持指定IP和端口
        /// </summary>
        public void Start(string ip, int port)
        {
            if (ip == "*" || ip == "all")
            {
                Start(port);
            }
            else
            {
                StartSocket(new IPEndPoint(IPAddress.Parse(ip), port));
                _isActive = true;
            }
        }

        /// <summary>
        /// 启动异步Scoket服务
        /// 绑定本机所有IP并指定端口
        /// </summary>
        /// <param name="port"></param>
        public void Start(int port)
        {
            StartSocket(new IPEndPoint(IPAddress.Any, port));
            _isActive = true;
        }

        public bool Active
        {
            get { return _isActive; }
        }

        /// <summary>
        /// 启动异步Socket服务器
        /// </summary>
        /// <param name="localEndPoint">要绑定的本地终结点</param>
        private void StartSocket(IPEndPoint localEndPoint)// 启动
        {
            try
            {
                // 创建一个侦听到达连接的Socket
                if (null != _listenSocket)
                {
                    _listenSocket.Close();
                }
                else
                {
                    _listenSocket = new Socket(localEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    _listenSocket.Bind(localEndPoint);
                }
                // 用一个侦听1000个连接的队列启动服务器
                _listenSocket.NoDelay = false;
                _listenSocket.ReceiveBufferSize = 4096;
                _listenSocket.SendBufferSize = 4096;
                _listenSocket.ReceiveTimeout = 20000;
                _listenSocket.SendTimeout = 10000;
                _listenSocket.Listen(1000);
            }
            catch (ObjectDisposedException)
            {

            }
            catch (SocketException ex)
            {
                //RaiseErrorEvent(null, exception);
                // 启动失败抛出启动失败异常
                if (ex.SocketErrorCode == SocketError.AddressNotAvailable)
                {
                    throw new AsyncSocketException("IP地址不可用", ex);
                }
                if (ex.ErrorCode == 48)
                {
                    throw new AsyncSocketException("Socket端口被占用", AsyncSocketErrorCode.ServerStartFailure);
                }
                else
                {
                    throw new AsyncSocketException("服务器启动失败", AsyncSocketErrorCode.ServerStartFailure);
                }
            }
            catch (Exception exceptionDebug)
            {
                Debug.WriteLine("调试：" + exceptionDebug.Message);
                throw;
            }
            // 在侦听Socket上抛出接受委托.      

            StartAccept(null); // (第一次不采用可重用的SocketAsyncEventArgs对象来接受请求的Socket的Accept方式)

            Debug.WriteLine("服务器启动成功....");
        }

        /// <summary>
        /// 开始接受连接请求
        /// </summary>
        /// <param name="acceptEventArg"></param>
        private void StartAccept(SocketAsyncEventArgs acceptEventArg)
        {
            if (acceptEventArg == null)
            {
                acceptEventArg = new SocketAsyncEventArgs();
                acceptEventArg.Completed += AcceptEventArg_Completed;
            }
            else
            {
                // 由于上下文对象正在被使用Socket必须被清理
                acceptEventArg.AcceptSocket = null;
            }
            try
            {
                //m_maxNumberAcceptedClients.WaitOne(TimeSpan.FromMilliseconds(200));// 对信号量进行一次P操作

                bool willRaiseEvent = _listenSocket.AcceptAsync(acceptEventArg);
                if (!willRaiseEvent)
                {
                    ProcessAccept(acceptEventArg);
                }
            }
            catch (ObjectDisposedException)
            {
            }
            catch (SocketException socketException)
            {
                RaiseErrorEvent(null, new AsyncSocketException("服务器接受客户端请求发生一次异常", socketException));
                // 接受客户端发生异常
                //throw new AsyncSocketException("服务器接受客户端请求发生一次异常", AsyncSocketErrorCode.ServerAcceptFailure);
            }
            catch (Exception exceptionDebug)
            {
                Debug.WriteLine("调试：" + exceptionDebug.Message);
                throw;
            }
        }

        /// <summary>
        /// 这个方法是关联异步接受操作的回调方法并且当接收操作完成时被调用        
        /// </summary>
        private void AcceptEventArg_Completed(object sender, SocketAsyncEventArgs e)
        {
            ProcessAccept(e);
        }

        private void ProcessAccept(SocketAsyncEventArgs e)
        {
            AsyncUserToken token = null;
            //Interlocked.Increment(ref m_numConnectedSockets);
            //Debug.WriteLine($"客户端连接请求被接受. 有 {m_numConnectedSockets} 个客户端连接到服务器");
            SocketAsyncEventArgs readEventArg;
            // 获得已经接受的客户端连接Socket并把它放到ReadEventArg对象的user token中
            try
            {
                rwl.AcquireReaderLock(10);
                readEventArg = _readPool.Pop();

                token = (AsyncUserToken)readEventArg.UserToken;
                // 把它放到ReadEventArg对象的user token中
                token.Socket = e.AcceptSocket;
                // 获得一个新的Guid 32位 "xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx"
                token.ConnectionId = Guid.NewGuid().ToString("N");
                if (!this._mTokens.TryAdd(token.ConnectionId, token)) // 添加到集合中
                {
                    Console.WriteLine("Socket链接异常");
                    return;
                }
                EventHandler<AsyncUserToken> handler = OnClientConnect;
                // 如果订户事件将为空(null)
                handler?.Invoke(this, token);// 抛出客户端连接事件
                                             // 客户端一连接上就抛出一个接收委托给连接的Socket开始接收数据
                bool willRaiseEvent = token.Socket.ReceiveAsync(readEventArg);
                if (!willRaiseEvent)
                {
                    ProcessReceive(readEventArg);
                }
            }
            catch (ObjectDisposedException)
            {
                RaiseDisconnectedEvent(token);
            }
            catch (SocketException socketException)
            {
                if (socketException.ErrorCode == (int)SocketError.ConnectionReset)// 10054一个建立的连接被远程主机强行关闭
                {
                    RaiseDisconnectedEvent(token);// 引发断开连接事件
                }
                else
                {
                    RaiseErrorEvent(token, new AsyncSocketException("在SocketAsyncEventArgs对象上执行异步接收数据操作时发生SocketException异常", socketException));
                }
            }
            catch (Exception exceptionDebug)
            {
                Debug.WriteLine("调试：" + exceptionDebug.Message);
            }
            finally
            {
                rwl.ReleaseReaderLock();
                // 接受下一个连接请求
                StartAccept(e);
            }
        }

        /// <summary>
        /// 这个方法在一个Socket读或者发送操作完成时被调用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">和完成的接受操作关联的SocketAsyncEventArg对象</param>
        private void IO_Completed(object sender, SocketAsyncEventArgs e)
        {
            // 确定刚刚完成的操作类型并调用关联的句柄
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Receive:
                    ProcessReceive(e);
                    break;
                case SocketAsyncOperation.Send:
                    ProcessSend(e);
                    break;
                default:
                    throw new ArgumentException("最后一次在Socket上的操作不是接收或者发送操作");
            }
        }

        /// <summary>
        /// 这个方法在异步接收操作完成时调用.  
        /// 如果远程主机关闭连接Socket将关闭        
        /// </summary>
        private void ProcessReceive(SocketAsyncEventArgs e)
        {
            AsyncUserToken token = (AsyncUserToken)e.UserToken;
            // 检查远程主机是否关闭连接
            if (e.BytesTransferred > 0 && e.SocketError == SocketError.Success)
            {
                // 增加接收到的字节总数
                Interlocked.Add(ref _totalBytesRead, e.BytesTransferred);
                //Debug.WriteLine($"服务器读取字节总数:{BytesToReadableValue(_totalBytesRead)}");
                //byte[] destinationArray = new byte[e.BytesTransferred];// 目的字节数组
                //Array.Copy(e.Buffer, 0, destinationArray, 0, e.BytesTransferred);
                token.SetBytesReceived(e.BytesTransferred);

                EventHandler<AsyncUserToken> handler = OnClientRead;
                // 如果订户事件将为空(null)
                handler?.Invoke(this, token);// 抛出接收到数据事件                                                   

                try
                {
                    // 继续接收数据
                    var willRaiseEvent = token.Socket.ReceiveAsync(e);
                    if (!willRaiseEvent)
                    {
                        ProcessReceive(e);
                    }
                }
                catch (ObjectDisposedException)
                {
                    RaiseDisconnectedEvent(token);
                }
                catch (SocketException socketException)
                {
                    if (socketException.ErrorCode == (int)SocketError.ConnectionReset)//10054一个建立的连接被远程主机强行关闭
                    {
                        RaiseDisconnectedEvent(token);//引发断开连接事件
                    }
                    else
                    {
                        RaiseErrorEvent(token, new AsyncSocketException("在SocketAsyncEventArgs对象上执行异步接收数据操作时发生SocketException异常", socketException));
                    }
                }
                catch (Exception exceptionDebug)
                {
                    Debug.WriteLine("调试：" + exceptionDebug.Message);
                    throw;
                }
            }
            else
            {
                RaiseDisconnectedEvent(token);
            }
        }

        public void Send(string connectionId, byte[] buffer)
        {
            AsyncUserToken token;
            if (!this._mTokens.TryGetValue(connectionId, out token))
            {
                throw new AsyncSocketException($"客户端:{connectionId}已经关闭或者未连接", AsyncSocketErrorCode.ClientSocketNoExist);
            }
            try
            {
                rwl.AcquireReaderLock(3);
                var writeEventArgs = _writePool.Pop();// 分配一个写SocketAsyncEventArgs对象
                writeEventArgs.UserToken = token;
                if (buffer.Length <= _bufferSize)
                {
                    writeEventArgs.SetBuffer(buffer, writeEventArgs.Offset, buffer.Length);
                    //Array.Copy(buffer, 0, writeEventArgs.Buffer, writeEventArgs.Offset, buffer.Length);
                    //Array.Copy(buffer, 0, writeEventArgs.Buffer, writeEventArgs.Offset, buffer.Length);
                    //writeEventArgs.SetBuffer(writeEventArgs.Buffer, writeEventArgs.Offset, buffer.Length);
                }
                else
                {
                    lock (_bufferLock)
                    {
                        _bufferManager.FreeBuffer(writeEventArgs);
                    }
                    writeEventArgs.SetBuffer(buffer);
                }
                // 异步发送数据
                var willRaiseEvent = token.Socket.SendAsync(writeEventArgs);
                if (!willRaiseEvent)
                {
                    ProcessSend(writeEventArgs);
                }
            }
            catch (ObjectDisposedException)
            {
                RaiseDisconnectedEvent(token);
            }
            catch (SocketException socketException)
            {
                if (socketException.ErrorCode == (int)SocketError.ConnectionReset)
                {
                    RaiseDisconnectedEvent(token);//引发断开连接事件
                }
                else
                {
                    RaiseErrorEvent(token, new AsyncSocketException("在SocketAsyncEventArgs对象上执行异步发送数据操作时发生SocketException异常", socketException));
                }
            }
            catch (Exception exceptionDebug)
            {
                Debug.WriteLine("调试：" + exceptionDebug.Message);
                throw;
            }
            finally
            {
                rwl.ReleaseReaderLock();
            }
        }


        /// <summary>
        /// 发送数据,可以携带操作类型(使用自定义枚举来表示)
        /// </summary>
        /// <param name="connectionId">连接的ID号</param>
        /// <param name="buffer">缓冲区大小</param>
        /// <param name="operation">操作自定义枚举</param>
        public void SendAsync(string connectionId, byte[] buffer, object operation)
        {
            AsyncUserToken token;
            //SocketAsyncEventArgs token;
            //if (buffer.Length <= m_receiveSendBufferSize)
            //{
            //    throw new ArgumentException("数据包长度超过缓冲区大小", "buffer");
            //}
            if (!this._mTokens.TryGetValue(connectionId, out token))
            {
                throw new AsyncSocketException($"客户端:{connectionId}已经关闭或者未连接", AsyncSocketErrorCode.ClientSocketNoExist);
                //return;
            }
            //writeEventArgs.SetBuffer(buffer, 0, buffer.Length);
            try
            {
                SocketAsyncEventArgs writeEventArgs;
                rwl.AcquireReaderLock(10);
                writeEventArgs = _writePool.Pop();// 分配一个写SocketAsyncEventArgs对象
                writeEventArgs.UserToken = token;
                token.Operation = operation;// 设置操作标志
                if (buffer.Length <= _bufferSize)
                {
                    Array.Copy(buffer, 0, writeEventArgs.Buffer, writeEventArgs.Offset, buffer.Length);
                }
                else
                {
                    lock (_bufferLock)
                    {
                        _bufferManager.FreeBuffer(writeEventArgs);
                    }
                    writeEventArgs.SetBuffer(buffer, 0, buffer.Length);
                }

                // 异步发送数据
                bool willRaiseEvent = token.Socket.SendAsync(writeEventArgs);
                if (!willRaiseEvent)
                {
                    ProcessSend(writeEventArgs);
                }
            }
            catch (ObjectDisposedException)
            {
                RaiseDisconnectedEvent(token);
            }
            catch (SocketException socketException)
            {
                if (socketException.ErrorCode == (int)SocketError.ConnectionReset)//10054一个建立的连接被远程主机强行关闭
                {
                    RaiseDisconnectedEvent(token);//引发断开连接事件
                }
                else
                {
                    RaiseErrorEvent(token, new AsyncSocketException("在SocketAsyncEventArgs对象上执行异步发送数据操作时发生SocketException异常", socketException));
                }
            }
            catch (Exception exceptionDebug)
            {
                Debug.WriteLine("调试：" + exceptionDebug.Message);
                throw;
            }
            finally
            {
                rwl.ReleaseReaderLock();
            }
        }

        /// <summary>
        /// 这个方法当一个异步发送操作完成时被调用.         
        /// </summary>
        /// <param name="e"></param>
        private void ProcessSend(SocketAsyncEventArgs e)
        {
            try
            {
                //SocketAsyncEventArgs token;
                AsyncUserToken token = (AsyncUserToken)e.UserToken;
                // 增加发送计数器
                Interlocked.Add(ref _totalBytesWrite, e.BytesTransferred);
                if (e.Count > _bufferSize)
                {
                    lock (_bufferLock)
                    {
                        _bufferManager.SetBuffer(e);// 恢复默认大小缓冲区
                    }
                    //e.SetBuffer(null, 0, 0);// 清除发送缓冲区
                }

                // 回收SocketAsyncEventArgs以备再次被利用
                lock (_writePool)
                {
                    _writePool.Push(e);
                }

                // 清除UserToken对象引用  
                e.UserToken = null;

                if (e.SocketError == SocketError.Success)
                {
                    //Debug.WriteLine($"发送总字节数:{BytesToReadableValue(e.BytesTransferred)}");
                    //lock (((ICollection)this.m_tokens).SyncRoot)
                    //{
                    //    if (!this.m_tokens.TryGetValue(token.ConnectionId, out token))
                    //    {
                    //        RaiseErrorEvent(token,new AsyncSocketException(string.Format("客户端:{0}或者已经关闭或者未连接", token.ConnectionId), AsyncSocketErrorCode.ClientSocketNoExist));
                    //        //throw new AsyncSocketException(string.Format("客户端:{0}或者已经关闭或者未连接", token.ConnectionId), AsyncSocketErrorCode.ClientSocketNoExist);
                    //        //return;
                    //    }
                    //}
                    EventHandler<AsyncUserToken> handler = OnDataSendCompleted;
                    // 如果订户事件将为空(null)
                    if (handler != null)
                    {
                        handler(this, token);//抛出客户端发送完成事件
                    }

                    //try
                    //{
                    //    // 读取下一块从客户端发送来的数据
                    //    bool willRaiseEvent = ((AsyncUserToken)connection.UserToken).Socket.ReceiveAsync(connection);
                    //    if (!willRaiseEvent)
                    //    {
                    //        ProcessReceive(connection);
                    //    }
                    //}
                    //catch (ObjectDisposedException)
                    //{
                    //    RaiseDisconnectedEvent(connection);
                    //}
                    //catch (SocketException exception)
                    //{
                    //    if (exception.ErrorCode == (int)SocketError.ConnectionReset)//10054一个建立的连接被远程主机强行关闭
                    //    {
                    //        RaiseDisconnectedEvent(connection);//引发断开连接事件
                    //    }
                    //    else
                    //    {
                    //        RaiseErrorEvent(connection, exception);//引发断开连接事件
                    //    }
                    //}
                    //catch (Exception exception_debug)
                    //{
                    //    Debug.WriteLine("调试：" + exception_debug.Message);
                    //    throw exception_debug;
                    //}
                }
                else
                {
                    //lock (((ICollection)this.m_tokens).SyncRoot)
                    //{
                    //    if (!this.m_tokens.TryGetValue(token.ConnectionId, out token))
                    //    {
                    //        throw new AsyncSocketException(string.Format("客户端:{0}或者已经关闭或者未连接", token.ConnectionId), AsyncSocketErrorCode.ClientSocketNoExist);
                    //        //return;
                    //    }
                    //}

                    RaiseDisconnectedEvent(token);//引发断开连接事件
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        public void Disconnect(string connectionId)//断开连接(形参 连接ID)
        {
            AsyncUserToken token;
            if (!this._mTokens.TryGetValue(connectionId, out token))
            {
                throw new AsyncSocketException($"客户端:{connectionId}已经关闭或者未连接", AsyncSocketErrorCode.ClientSocketNoExist);
                //return;//不存在该ID客户端
            }
            RaiseDisconnectedEvent(token);//抛出断开连接事件            
        }

        private void RaiseDisconnectedEvent(AsyncUserToken token)//引发断开连接事件
        {
            if (null != token)
            {
                if (this._mTokens.ContainsKey(token.ConnectionId))
                {
                    this._mTokens.TryRemove(token.ConnectionId, out token);
                    CloseClientSocket(token);
                    EventHandler<AsyncUserToken> handler = OnClientDisconnect;
                    // 如果订户事件将为空(null)
                    if ((handler != null) && (null != token))
                    {
                        handler(this, token);//抛出连接断开事件
                    }
                }
            }
        }

        private void CloseClientSocket(AsyncUserToken token)
        {
            //AsyncUserToken token = e.UserToken as AsyncUserToken;
            if (token == null)
            {
                return;
            }

            // 关闭相关联的客户端
            try
            {
                token.Socket.Shutdown(SocketShutdown.Both);
                token.Socket.Close();
            }
            // 抛出客户处理已经被关闭
            catch (ObjectDisposedException)
            {
            }
            catch (SocketException)
            {
                token.Socket.Close();
            }
            catch (Exception exceptionDebug)
            {
                token.Socket.Close();
                Debug.WriteLine("调试:" + exceptionDebug.Message);
                throw;
            }
            finally
            {
                // 减少连接到服务器客户端总数的计数器的值
                //Interlocked.Decrement(ref m_numConnectedSockets);
                //m_maxNumberAcceptedClients.Release();
                //Debug.WriteLine($"一个客户端被从服务器断开. 有 {m_numConnectedSockets} 个客户端连接到服务器");
                _readPool.Push(token.ReadEventArgs);
            }
        }

        private void RaiseErrorEvent(AsyncUserToken token, AsyncSocketException exception)
        {
            EventHandler<AsyncSocketErrorEventArgs> handler = OnClientError;
            // 如果订户事件将为空(null)
            if (handler != null)
            {
                if (null != token)
                {
                    handler(token, new AsyncSocketErrorEventArgs(exception));//抛出客户端错误事件
                }
                else
                {
                    handler(null, new AsyncSocketErrorEventArgs(exception));//抛出服务器错误事件
                }
            }
        }

        public void Shutdown()
        {
            if (null != this._listenSocket)
            {
                this._listenSocket.Close();//停止侦听
            }
            foreach (AsyncUserToken token in this._mTokens.Values)
            {
                try
                {
                    CloseClientSocket(token);
                    EventHandler<AsyncUserToken> handler = OnClientDisconnect;
                    // 如果订户事件将为空(null)
                    if ((handler != null) && (null != token))
                    {
                        handler(this, token);//抛出连接断开事件
                    }
                }
                // 编译时打开注释调试时关闭
                //catch(Exception){ }
                // 编译时关闭调试时打开
                catch (Exception exceptionDebug)
                {
                    Debug.WriteLine("调试:" + exceptionDebug.Message);
                }
            }
            this._mTokens.Clear();
            _isActive = false;
        }

        /// <summary>
        /// 获取文件大小的显示字符串
        /// </summary>
        /// <returns></returns>
        private string BytesToReadableValue(long length)
        {
            int byteConversion = 1024;
            double bytes = Convert.ToDouble(length);
            // 超过EB的单位已经没有实际转换意义了, 太大了, 忽略不用
            if (bytes >= Math.Pow(byteConversion, 6)) // EB
            {
                return string.Concat(Math.Round(bytes / Math.Pow(byteConversion, 6), 2), " EB");
            }
            if (bytes >= Math.Pow(byteConversion, 5)) // PB
            {
                return string.Concat(Math.Round(bytes / Math.Pow(byteConversion, 5), 2), " PB");
            }
            if (bytes >= Math.Pow(byteConversion, 4)) // TB
            {
                return string.Concat(Math.Round(bytes / Math.Pow(byteConversion, 4), 2), " TB");
            }
            if (bytes >= Math.Pow(byteConversion, 3)) // GB
            {
                return string.Concat(Math.Round(bytes / Math.Pow(byteConversion, 3), 2), " GB");
            }
            if (bytes >= Math.Pow(byteConversion, 2)) // MB
            {
                return string.Concat(Math.Round(bytes / Math.Pow(byteConversion, 2), 2), " MB");
            }
            if (bytes >= byteConversion) // KB
            {
                return string.Concat(Math.Round(bytes / byteConversion, 2), " KB");
            }
            return string.Concat(bytes, " Bytes");// Bytes
        }
    }
}