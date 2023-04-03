using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using SystemModule.Core.Run.Action;
using SystemModule.CoreSocket;
using SystemModule.Dependency;
using SystemModule.Extensions;
using SystemModule.Sockets.Common;
using SystemModule.Sockets.Common.Options;
using SystemModule.Sockets.DataAdapter;
using SystemModule.Sockets.Enum;
using SystemModule.Sockets.Exceptions;
using SystemModule.Sockets.Extensions;
using SystemModule.Sockets.Interface;
using SystemModule.Sockets.SocketEventArgs;

namespace SystemModule.Sockets.Components.TCP
{
    /// <summary>
    /// 简单TCP客户端
    /// </summary>
    public class TcpClient : TcpClientBase
    {
        /// <summary>
        /// 接收到数据
        /// </summary>
        public ReceivedEventHandler<TcpClient> Received { get; set; }

        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="byteBlock"></param>
        /// <param name="requestInfo"></param>
        protected override void HandleReceivedData(ByteBlock byteBlock, IRequestInfo requestInfo)
        {
            Received?.Invoke(this, byteBlock, requestInfo);
            base.HandleReceivedData(byteBlock, requestInfo);
        }
    }

    /// <summary>
    /// TCP客户端
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("{IP}:{Port}")]
    public class TcpClientBase : BaseSocket, ITcpClient
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public TcpClientBase()
        {
            Protocol = Protocol.TCP;
        }

        #region 变量

        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private DelaySender m_delaySender;
        private bool m_useDelaySender;
        private Stream m_workStream;

        #endregion 变量

        #region 事件

        /// <summary>
        /// 成功连接到服务器
        /// </summary>
        public MessageEventHandler<ITcpClient> Connected { get; set; }

        /// <summary>
        /// 准备连接的时候，此时已初始化Socket，但是并未建立Tcp连接
        /// </summary>
        public ConnectingEventHandler<ITcpClient> Connecting { get; set; }

        /// <summary>
        /// 断开连接。在客户端未设置连接状态时，不会触发
        /// </summary>
        public DisconnectEventHandler<ITcpClientBase> Disconnected { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public DisconnectEventHandler<ITcpClientBase> Disconnecting { get; set; }

        private void PrivateOnConnected(MsgEventArgs e)
        {
            OnConnected(e);
        }

        /// <summary>
        /// 已经建立Tcp连接
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnConnected(MsgEventArgs e)
        {
            try
            {
                Connected?.Invoke(this, e);
            }
            catch (Exception ex)
            {
                _logger.Error($"在事件{nameof(Connected)}中发生错误。{0}", ex);
            }
        }

        private void PrivateOnConnecting(ConnectingEventArgs e)
        {
            LastReceivedTime = DateTime.Now;
            LastSendTime = DateTime.Now;
            if (CanSetDataHandlingAdapter)
            {
                SetDataHandlingAdapter(Config.GetValue(TouchSocketConfigExtension.DataHandlingAdapterProperty).Invoke());
            }
            OnConnecting(e);
        }

        /// <summary>
        /// 准备连接的时候，此时已初始化Socket，但是并未建立Tcp连接
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnConnecting(ConnectingEventArgs e)
        {
            try
            {
                Connecting?.Invoke(this, e);
            }
            catch (Exception ex)
            {
                _logger.Error($"在事件{nameof(this.OnConnecting)}中发生错误。{0}", ex);
            }
        }

        private void PrivateOnDisconnected(DisconnectEventArgs e)
        {
            OnDisconnected(e);
        }

        /// <summary>
        /// 断开连接。在客户端未设置连接状态时，不会触发
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnDisconnected(DisconnectEventArgs e)
        {
            try
            {
                Disconnected?.Invoke(this, e);
            }
            catch (Exception ex)
            {
                _logger.Error($"在事件{nameof(Disconnected)}中发生错误。{0}", ex);
            }
        }

        private void PrivateOnDisconnecting(DisconnectEventArgs e)
        {
            OnDisconnecting(e);
        }

        /// <summary>
        /// 即将断开连接(仅主动断开时有效)。
        /// <para>
        /// 当主动调用Close断开时，可通过<see cref="TouchSocketEventArgs.IsPermitOperation"/>终止断开行为。
        /// </para>
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnDisconnecting(DisconnectEventArgs e)
        {
            try
            {
                Disconnecting?.Invoke(this, e);
            }
            catch (Exception ex)
            {
                _logger.Error($"在事件{nameof(Disconnecting)}中发生错误。{0}", ex);
            }
        }
        #endregion 事件

        #region 属性

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public DateTime LastReceivedTime { get; private set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public DateTime LastSendTime { get; private set; }

        /// <summary>
        /// 处理未经过适配器的数据。返回值表示是否继续向下传递。
        /// </summary>
        public Func<ByteBlock, bool> OnHandleRawBuffer { get; set; }

        /// <summary>
        /// 处理经过适配器后的数据。返回值表示是否继续向下传递。
        /// </summary>
        public Func<ByteBlock, IRequestInfo, bool> OnHandleReceivedData { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public IContainer Container => Config?.Container;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public virtual bool CanSetDataHandlingAdapter => true;

        /// <summary>
        /// 客户端配置
        /// </summary>
        public TouchSocketConfig Config { get; private set; }

        /// <summary>
        /// 数据处理适配器
        /// </summary>
        public DataHandlingAdapter DataHandlingAdapter { get; private set; }

        /// <summary>
        /// IP地址
        /// </summary>
        public string IP { get; private set; }

        /// <summary>
        /// 主通信器
        /// </summary>
        public Socket MainSocket { get; private set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool CanSend { get; private set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool Online => CanSend;

        /// <summary>
        /// 端口号
        /// </summary>
        public int Port { get; private set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public ReceiveType ReceiveType { get; private set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public bool UseSsl { get; private set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public Protocol Protocol { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public IPHost RemoteIPHost { get; private set; }

        /// <inheritdoc/>
        public bool IsClient => true;

        #endregion 属性

        #region 断开操作

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public virtual void Close()
        {
            Close($"{nameof(Close)}主动断开");
        }

        /// <summary>
        /// 中断终端，传递中断消息。
        /// </summary>
        /// <param name="msg"></param>
        public virtual void Close(string msg)
        {
            if (CanSend)
            {
                DisconnectEventArgs args = new DisconnectEventArgs(true, msg)
                {
                    IsPermitOperation = true
                };
                PrivateOnDisconnecting(args);
                if (DisposedValue || args.IsPermitOperation)
                {
                    BreakOut(msg, true);
                }
            }
        }

        private void BreakOut(string msg, bool manual)
        {
            lock (SyncRoot)
            {
                if (CanSend)
                {
                    CanSend = false;
                    this.TryShutdown();
                    MainSocket.SafeDispose();
                    m_delaySender.SafeDispose();
                    m_workStream.SafeDispose();
                    DataHandlingAdapter.SafeDispose();
                    PrivateOnDisconnected(new DisconnectEventArgs(manual, msg));
                }
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (CanSend)
            {
                DisconnectEventArgs args = new DisconnectEventArgs(true, $"{nameof(Dispose)}主动断开");
                PrivateOnDisconnecting(args);
            }
            Config = default;
            DataHandlingAdapter.SafeDispose();
            DataHandlingAdapter = default;
            BreakOut($"{nameof(Dispose)}主动断开", true);
            base.Dispose(disposing);
        }

        #endregion 断开操作

        /// <summary>
        /// 建立Tcp的连接。
        /// </summary>
        /// <param name="timeout"></param>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        /// <exception cref="TimeoutException"></exception>
        protected void TcpConnect(int timeout)
        {
            lock (SyncRoot)
            {
                if (CanSend)
                {
                    return;
                }
                if (DisposedValue)
                {
                    throw new ObjectDisposedException(GetType().FullName);
                }
                if (Config == null)
                {
                    throw new ArgumentNullException("配置文件不能为空。");
                }
                IPHost iPHost = Config.GetValue(TouchSocketConfigExtension.RemoteIPHostProperty);
                if (iPHost == null)
                {
                    throw new ArgumentNullException("iPHost不能为空。");
                }
                if (MainSocket != null)
                {
                    MainSocket.Dispose();
                }
                MainSocket = CreateSocket(iPHost);
                ConnectingEventArgs args = new ConnectingEventArgs(MainSocket);
                PrivateOnConnecting(args);
                if (timeout == 5000)
                {
                    MainSocket.Connect(iPHost.EndPoint);
                    CanSend = true;
                    LoadSocketAndReadIpPort();
                    if (Config.GetValue(TouchSocketConfigExtension.DelaySenderProperty) is DelaySenderOption senderOption)
                    {
                        m_useDelaySender = true;
                        m_delaySender.SafeDispose();
                        m_delaySender = new DelaySender(MainSocket, senderOption.QueueLength, OnDelaySenderError)
                        {
                            DelayLength = senderOption.DelayLength
                        };
                    }
                    BeginReceive();
                    PrivateOnConnected(new MsgEventArgs("连接成功"));
                }
                else
                {
                    IAsyncResult result = MainSocket.BeginConnect(iPHost.EndPoint, null, null);
                    if (result.AsyncWaitHandle.WaitOne(timeout))
                    {
                        if (MainSocket.Connected)
                        {
                            MainSocket.EndConnect(result);
                            CanSend = true;
                            LoadSocketAndReadIpPort();
                            if (Config.GetValue(TouchSocketConfigExtension.DelaySenderProperty) is DelaySenderOption senderOption)
                            {
                                m_useDelaySender = true;
                                m_delaySender.SafeDispose();
                                m_delaySender = new DelaySender(MainSocket, senderOption.QueueLength, OnDelaySenderError)
                                {
                                    DelayLength = senderOption.DelayLength
                                };
                            }
                            BeginReceive();
                            PrivateOnConnected(new MsgEventArgs("连接成功"));
                            return;
                        }
                        else
                        {
                            MainSocket.SafeDispose();
                            throw new Exception("异步已完成，但是socket并未在连接状态，可能发生了绑定端口占用的错误。");
                        }
                    }
                    MainSocket.SafeDispose();
                    throw new TimeoutException();
                }
            }
        }

        /// <summary>
        /// 请求连接到服务器。
        /// </summary>
        public virtual ITcpClient Connect(int timeout = 5000)
        {
            TcpConnect(timeout);
            return this;
        }

        /// <summary>
        /// 异步连接服务器
        /// </summary>
        public Task<ITcpClient> ConnectAsync(int timeout = 5000)
        {
            return EasyTask.Run(() => Connect(timeout));
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        public Stream GetStream()
        {
            if (m_workStream == null)
            {
                m_workStream = new NetworkStream(MainSocket, true);
            }
            return m_workStream;
        }

        /// <summary>
        /// 设置数据处理适配器
        /// </summary>
        /// <param name="adapter"></param>
        public virtual void SetDataHandlingAdapter(DataHandlingAdapter adapter)
        {
            if (!CanSetDataHandlingAdapter)
            {
                throw new Exception($"不允许自由调用{nameof(SetDataHandlingAdapter)}进行赋值。");
            }

            SetAdapter(adapter);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="ipHost"></param>
        /// <returns></returns>
        public ITcpClient Setup(string ipHost)
        {
            TouchSocketConfig config = new TouchSocketConfig();
            config.SetRemoteIPHost(new IPHost(ipHost));
            return Setup(config);
        }

        /// <summary>
        /// 配置服务器
        /// </summary>
        /// <param name="config"></param>
        /// <exception cref="Exception"></exception>
        public ITcpClient Setup(TouchSocketConfig config)
        {
            Config = config;
            LoadConfig(Config);
            return this;
        }

        private void PrivateHandleReceivedData(ByteBlock byteBlock, IRequestInfo requestInfo)
        {
            if (OnHandleReceivedData?.Invoke(byteBlock, requestInfo) == false)
            {
                return;
            }
            HandleReceivedData(byteBlock, requestInfo);
        }

        /// <summary>
        /// 处理已接收到的数据。
        /// </summary>
        /// <param name="byteBlock">以二进制流形式传递</param>
        /// <param name="requestInfo">以解析的数据对象传递</param>
        protected virtual void HandleReceivedData(ByteBlock byteBlock, IRequestInfo requestInfo)
        {
        }

        /// <summary>
        /// 当即将发送时，如果覆盖父类方法，则不会触发插件。
        /// </summary>
        /// <param name="buffer">数据缓存区</param>
        /// <param name="offset">偏移</param>
        /// <param name="length">长度</param>
        /// <returns>返回值表示是否允许发送</returns>
        protected virtual bool HandleSendingData(byte[] buffer, int offset, int length)
        {
            return true;
        }

        /// <summary>
        /// 当即将发送时，如果覆盖父类方法，则不会触发插件。
        /// </summary>
        /// <param name="buffer">数据缓存区</param>
        /// <param name="offset">偏移</param>
        /// <param name="length">长度</param>
        /// <returns>返回值表示是否允许发送</returns>
        protected virtual bool HandleSendingData(ReadOnlyMemory<byte> buffer, int offset, int length)
        {
            return true;
        }

        /// <summary>
        /// 加载配置
        /// </summary>
        /// <param name="config"></param>
        protected virtual void LoadConfig(TouchSocketConfig config)
        {
            if (config == null)
            {
                throw new Exception("配置文件为空");
            }
            RemoteIPHost = config.GetValue(TouchSocketConfigExtension.RemoteIPHostProperty);
            BufferLength = config.GetValue(TouchSocketConfigExtension.BufferLengthProperty);
            ReceiveType = config.GetValue(TouchSocketConfigExtension.ReceiveTypeProperty);
            if (config.GetValue(TouchSocketConfigExtension.SslOptionProperty) != null)
            {
                UseSsl = true;
            }
        }

        /// <summary>
        /// 在延迟发生错误
        /// </summary>
        /// <param name="ex"></param>
        protected virtual void OnDelaySenderError(Exception ex)
        {
            _logger.Error("发送错误{0}", ex);
        }

        /// <summary>
        /// 设置适配器，该方法不会检验<see cref="CanSetDataHandlingAdapter"/>的值。
        /// </summary>
        /// <param name="adapter"></param>
        protected void SetAdapter(DataHandlingAdapter adapter)
        {
            if (adapter is null)
            {
                throw new ArgumentNullException(nameof(adapter));
            }

            if (Config != null)
            {
                if (Config.GetValue(TouchSocketConfigExtension.MaxPackageSizeProperty) is int v1)
                {
                    adapter.MaxPackageSize = v1;
                }
                if (Config.GetValue(TouchSocketConfigExtension.CacheTimeoutProperty) != TimeSpan.Zero)
                {
                    adapter.CacheTimeout = Config.GetValue(TouchSocketConfigExtension.CacheTimeoutProperty);
                }
                if (Config.GetValue(TouchSocketConfigExtension.CacheTimeoutEnableProperty) is bool v2)
                {
                    adapter.CacheTimeoutEnable = v2;
                }
                if (Config.GetValue(TouchSocketConfigExtension.UpdateCacheTimeWhenRevProperty) is bool v3)
                {
                    adapter.UpdateCacheTimeWhenRev = v3;
                }
            }

            adapter.OnLoaded(this);
            adapter.ReceivedCallBack = PrivateHandleReceivedData;
            adapter.SendCallBack = DefaultSend;
            DataHandlingAdapter = adapter;
        }

        private void BeginReceive()
        {
            m_workStream.SafeDispose();
            if (UseSsl)
            {
                ClientSslOption sslOption = (ClientSslOption)Config.GetValue(TouchSocketConfigExtension.SslOptionProperty);
                SslStream sslStream = sslOption.CertificateValidationCallback != null ? new SslStream(new NetworkStream(MainSocket, false), false, sslOption.CertificateValidationCallback) : new SslStream(new NetworkStream(MainSocket, false), false);
                if (sslOption.ClientCertificates == null)
                {
                    sslStream.AuthenticateAsClient(sslOption.TargetHost);
                }
                else
                {
                    sslStream.AuthenticateAsClient(sslOption.TargetHost, sslOption.ClientCertificates, sslOption.SslProtocols, sslOption.CheckCertificateRevocation);
                }
                m_workStream = sslStream;
                if (ReceiveType != ReceiveType.None)
                {
                    BeginSsl();
                }
            }
            else
            {
                if (ReceiveType == ReceiveType.Auto)
                {
                    SocketAsyncEventArgs eventArgs = new SocketAsyncEventArgs();
                    eventArgs.Completed += EventArgs_Completed;

                    ByteBlock byteBlock = BytePool.Default.GetByteBlock(BufferLength);
                    eventArgs.UserToken = byteBlock;
                    eventArgs.SetBuffer(byteBlock.Buffer, 0, byteBlock.Buffer.Length);
                    if (!MainSocket.ReceiveAsync(eventArgs))
                    {
                        ProcessReceived(eventArgs);
                    }
                }
            }
        }

        private void BeginSsl()
        {
            ByteBlock byteBlock = new ByteBlock(BufferLength);
            try
            {
                m_workStream.BeginRead(byteBlock.Buffer, 0, byteBlock.Capacity, EndSsl, byteBlock);
            }
            catch (Exception ex)
            {
                byteBlock.Dispose();
                BreakOut(ex.Message, false);
            }
        }

        private void EndSsl(IAsyncResult result)
        {
            ByteBlock byteBlock = (ByteBlock)result.AsyncState;
            try
            {
                int r = m_workStream.EndRead(result);
                if (r == 0)
                {
                    BreakOut("远程终端主动关闭", false);
                }
                byteBlock.SetLength(r);
                HandleBuffer(byteBlock);
                BeginSsl();
            }
            catch (Exception ex)
            {
                byteBlock.Dispose();
                BreakOut(ex.Message, false);
            }
        }

        private Socket CreateSocket(IPHost iPHost)
        {
            Socket socket = new Socket(iPHost.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socket.ReceiveBufferSize = BufferLength;
            socket.SendBufferSize = BufferLength;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                KeepAliveValue keepAliveValue = Config.GetValue(TouchSocketConfigExtension.KeepAliveValueProperty);
                if (keepAliveValue.Enable)
                {
                    socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.KeepAlive, true);
                    socket.IOControl(IOControlCode.KeepAliveValues, keepAliveValue.KeepAliveTime, null);
                }
            }
            socket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, Config.GetValue(TouchSocketConfigExtension.NoDelayProperty));
            if (Config.GetValue(TouchSocketConfigExtension.BindIPHostProperty) != null)
            {
                if (Config.GetValue(TouchSocketConfigExtension.ReuseAddressProperty))
                {
                    socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                }
                socket.Bind(Config.GetValue(TouchSocketConfigExtension.BindIPHostProperty).EndPoint);
            }
            return socket;
        }

        private void EventArgs_Completed(object sender, SocketAsyncEventArgs e)
        {
            try
            {
                ProcessReceived(e);
            }
            catch (Exception ex)
            {
                e.Dispose();
                BreakOut(ex.Message, false);
            }
        }

        /// <summary>
        /// 处理数据
        /// </summary>
        private void HandleBuffer(ByteBlock byteBlock)
        {
            try
            {
                LastReceivedTime = DateTime.Now;
                if (OnHandleRawBuffer?.Invoke(byteBlock) == false)
                {
                    return;
                }
                if (DisposedValue)
                {
                    return;
                }
                if (DataHandlingAdapter == null)
                {
                    _logger.Error(TouchSocketStatus.NullDataAdapter.GetDescription());
                    return;
                }
                DataHandlingAdapter.ReceivedInput(byteBlock);
            }
            catch (Exception ex)
            {
                _logger.Error("在处理数据时发生错误 {0}", ex);
            }
            finally
            {
                byteBlock.Dispose();
            }
        }

        #region 发送

        #region 同步发送

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="requestInfo"></param>
        /// <exception cref="NotConnectedException"></exception>
        /// <exception cref="OverlengthException"></exception>
        /// <exception cref="Exception"></exception>
        public void Send(IRequestInfo requestInfo)
        {
            if (DisposedValue)
            {
                return;
            }
            if (DataHandlingAdapter == null)
            {
                throw new ArgumentNullException(nameof(DataHandlingAdapter), TouchSocketStatus.NullDataAdapter.GetDescription());
            }
            if (!DataHandlingAdapter.CanSendRequestInfo)
            {
                throw new NotSupportedException($"当前适配器不支持对象发送。");
            }
            DataHandlingAdapter.SendInput(requestInfo);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="buffer"><inheritdoc/></param>
        /// <param name="offset"><inheritdoc/></param>
        /// <param name="length"><inheritdoc/></param>
        /// <exception cref="NotConnectedException"><inheritdoc/></exception>
        /// <exception cref="OverlengthException"><inheritdoc/></exception>
        /// <exception cref="Exception"><inheritdoc/></exception>
        public virtual void Send(byte[] buffer, int offset, int length)
        {
            if (DataHandlingAdapter == null)
            {
                throw new ArgumentNullException(nameof(DataHandlingAdapter), TouchSocketStatus.NullDataAdapter.GetDescription());
            }
            DataHandlingAdapter.SendInput(buffer, offset, length);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="buffer"><inheritdoc/></param>
        /// <param name="offset"><inheritdoc/></param>
        /// <param name="length"><inheritdoc/></param>
        /// <exception cref="NotConnectedException"><inheritdoc/></exception>
        /// <exception cref="OverlengthException"><inheritdoc/></exception>
        /// <exception cref="Exception"><inheritdoc/></exception>
        public virtual void Send(ReadOnlyMemory<byte> buffer, int offset, int length)
        {
            if (DataHandlingAdapter == null)
            {
                throw new ArgumentNullException(nameof(DataHandlingAdapter), TouchSocketStatus.NullDataAdapter.GetDescription());
            }
            DataHandlingAdapter.SendInput(buffer, offset, length);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="transferBytes"><inheritdoc/></param>
        /// <exception cref="NotConnectedException"><inheritdoc/></exception>
        /// <exception cref="OverlengthException"><inheritdoc/></exception>
        /// <exception cref="Exception"><inheritdoc/></exception>
        public virtual void Send(IList<ArraySegment<byte>> transferBytes)
        {
            if (DataHandlingAdapter == null)
            {
                throw new ArgumentNullException(nameof(DataHandlingAdapter), TouchSocketStatus.NullDataAdapter.GetDescription());
            }

            if (DataHandlingAdapter.CanSplicingSend)
            {
                DataHandlingAdapter.SendInput(transferBytes);
            }
            else
            {
                ByteBlock byteBlock = BytePool.Default.GetByteBlock(BufferLength);
                try
                {
                    foreach (ArraySegment<byte> item in transferBytes)
                    {
                        byteBlock.Write(item.Array, item.Offset, item.Count);
                    }
                    DataHandlingAdapter.SendInput(byteBlock.Buffer, 0, byteBlock.Len);
                }
                finally
                {
                    byteBlock.Dispose();
                }
            }
        }

        #endregion 同步发送

        #region 异步发送

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="buffer"><inheritdoc/></param>
        /// <param name="offset"><inheritdoc/></param>
        /// <param name="length"><inheritdoc/></param>
        /// <exception cref="NotConnectedException"><inheritdoc/></exception>
        /// <exception cref="OverlengthException"><inheritdoc/></exception>
        /// <exception cref="Exception"><inheritdoc/></exception>
        public virtual Task SendAsync(byte[] buffer, int offset, int length)
        {
            return EasyTask.Run(() =>
            {
                Send(buffer, offset, length);
            });
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="requestInfo"></param>
        /// <exception cref="NotConnectedException"></exception>
        /// <exception cref="OverlengthException"></exception>
        /// <exception cref="Exception"></exception>
        public virtual Task SendAsync(IRequestInfo requestInfo)
        {
            return EasyTask.Run(() =>
            {
                Send(requestInfo);
            });
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="transferBytes"><inheritdoc/></param>
        /// <exception cref="NotConnectedException"><inheritdoc/></exception>
        /// <exception cref="OverlengthException"><inheritdoc/></exception>
        /// <exception cref="Exception"><inheritdoc/></exception>
        public virtual Task SendAsync(IList<ArraySegment<byte>> transferBytes)
        {
            return EasyTask.Run(() =>
            {
                Send(transferBytes);
            });
        }

        #endregion 异步发送

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="buffer"><inheritdoc/></param>
        /// <param name="offset"><inheritdoc/></param>
        /// <param name="length"><inheritdoc/></param>
        /// <exception cref="NotConnectedException"><inheritdoc/></exception>
        /// <exception cref="OverlengthException"><inheritdoc/></exception>
        /// <exception cref="Exception"><inheritdoc/></exception>
        public void DefaultSend(byte[] buffer, int offset, int length)
        {
            if (!CanSend)
            {
                throw new NotConnectedException(TouchSocketStatus.NotConnected.GetDescription());
            }
            if (HandleSendingData(buffer, offset, length))
            {
                if (UseSsl)
                {
                    m_workStream.Write(buffer, offset, length);
                }
                else
                {
                    if (m_useDelaySender && length < TouchSocketUtility.BigDataBoundary)
                    {
                        m_delaySender.Send(new QueueDataBytes(buffer, offset, length));
                    }
                    else
                    {
                        MainSocket.AbsoluteSend(buffer, offset, length);
                    }
                }

                LastSendTime = DateTime.Now;
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="buffer"><inheritdoc/></param>
        /// <param name="offset"><inheritdoc/></param>
        /// <param name="length"><inheritdoc/></param>
        /// <exception cref="NotConnectedException"><inheritdoc/></exception>
        /// <exception cref="OverlengthException"><inheritdoc/></exception>
        /// <exception cref="Exception"><inheritdoc/></exception>
        public void DefaultSend(ReadOnlyMemory<byte> buffer, int offset, int length)
        {
            if (!CanSend)
            {
                throw new NotConnectedException(TouchSocketStatus.NotConnected.GetDescription());
            }
            if (HandleSendingData(buffer, offset, length))
            {
                if (UseSsl)
                {
                    //this.m_workStream.Write(buffer, offset, length);
                }
                else
                {
                    if (m_useDelaySender && length < TouchSocketUtility.BigDataBoundary)
                    {
                        m_delaySender.Send(new QueueDataBytes(buffer, offset, length));
                    }
                    else
                    {
                        MainSocket.AbsoluteSend(buffer, offset, length);
                    }
                }

                LastSendTime = DateTime.Now;
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="buffer"><inheritdoc/></param>
        /// <param name="offset"><inheritdoc/></param>
        /// <param name="length"><inheritdoc/></param>
        /// <exception cref="NotConnectedException"><inheritdoc/></exception>
        /// <exception cref="OverlengthException"><inheritdoc/></exception>
        /// <exception cref="Exception"><inheritdoc/></exception>
        public Task DefaultSendAsync(byte[] buffer, int offset, int length)
        {
            return EasyTask.Run(() =>
            {
                DefaultSend(buffer, offset, length);
            });
        }

        #endregion 发送

        private void LoadSocketAndReadIpPort()
        {
            if (MainSocket == null)
            {
                IP = null;
                Port = -1;
                return;
            }

            IPEndPoint endPoint;
            if (MainSocket.Connected && MainSocket.RemoteEndPoint != null)
            {
                endPoint = (IPEndPoint)MainSocket.RemoteEndPoint;
            }
            else if (MainSocket.IsBound && MainSocket.LocalEndPoint != null)
            {
                endPoint = (IPEndPoint)MainSocket.LocalEndPoint;
            }
            else
            {
                return;
            }

            IP = endPoint.Address.ToString();
            Port = endPoint.Port;
        }

        private void ProcessReceived(SocketAsyncEventArgs e)
        {
            if (!CanSend)
            {
                e.Dispose();
                return;
            }
            if (e.SocketError == SocketError.Success && e.BytesTransferred > 0)
            {
                ByteBlock byteBlock = (ByteBlock)e.UserToken;
                byteBlock.SetLength(e.BytesTransferred);
                HandleBuffer(byteBlock);
                try
                {
                    ByteBlock newByteBlock = BytePool.Default.GetByteBlock(BufferLength);
                    e.UserToken = newByteBlock;
                    e.SetBuffer(newByteBlock.Buffer, 0, newByteBlock.Buffer.Length);

                    if (!MainSocket.ReceiveAsync(e))
                    {
                        ProcessReceived(e);
                    }
                }
                catch (Exception ex)
                {
                    e.Dispose();
                    BreakOut(ex.Message, false);
                }
            }
            else
            {
                e.Dispose();
                BreakOut("远程终端主动关闭", false);
            }
        }
    }
}