using System;
using System.Net;
using System.Net.Sockets;

namespace SystemModule.Sockets
{
    /// <summary>
    /// 这个类被设计用来作为被分配给SocketAsyncEventArgs.UserToken属性的类.
    /// </summary>
    public class AsyncUserToken : EventArgs
    {
        private Socket m_socket;//Socket
        private int m_connectionId;//内部连接ID
        private readonly int m_connectionIdx;//内部链接数自增
        private IPEndPoint m_endPoint;//终结点
        private byte[] m_receiveBuffer;//缓冲区
        private int m_count;
        private int m_offset;//偏移量
        private int m_bytesReceived;//已经接收到的字节数
        private SocketAsyncEventArgs m_readEventArgs;// SocketAsyncEventArgs读对象
        private object m_operation;

        public AsyncUserToken()
            : this(null)
        {

        }

        /// <summary>
        /// 预留的操作标志 用于发送某些操作数据后的成功反馈(建议用法 使用自定义枚举来表示操作)
        /// </summary>
        public object Operation
        {
            set { m_operation = value; }
            get { return m_operation; }
        }

        /// <summary>
        /// 获取缓冲区
        /// </summary>
        public byte[] ReceiveBuffer => m_receiveBuffer;

        /// <summary>
        /// 获取相对缓冲区偏移量
        /// </summary>
        public int Offset => m_offset;

        /// <summary>
        /// 获取接收数据字节数
        /// </summary>
        public int BytesReceived => m_bytesReceived;

        /// <summary>
        /// 获取或设置SocketAsyncEventArgs读对象
        /// </summary>
        public SocketAsyncEventArgs ReadEventArgs
        {
            set { m_readEventArgs = value; }
            get { return m_readEventArgs; }
        }

        /// <summary>
        /// 携带的Scoket上下文
        /// </summary>
        /// <param name="socket">Socket上下文</param>
        public AsyncUserToken(Socket socket)
        {
            m_readEventArgs = new SocketAsyncEventArgs();
            m_readEventArgs.UserToken = this;
            if (null != socket)
            {
                m_socket = socket;
                this.m_endPoint = (IPEndPoint)socket.RemoteEndPoint;
            }
        }

        /// <summary>
        /// 获取或设置携带的Socket上下文
        /// </summary>
        public Socket Socket
        {
            get { return m_socket; }
            set
            {
                if (value != null)
                {
                    m_socket = value;
                    m_endPoint = (IPEndPoint)m_socket.RemoteEndPoint;
                }
            }
        }

        public int SocHandle
        {
            get
            {
                if (Socket == null)
                {
                    return 0;
                }
                return (int)Socket.Handle;
            }
        }

        /// <summary>
        /// 获取或设置通讯中使用的连接ID号
        /// </summary>
        public int ConnectionId//内部连接ID
        {
            get { return this.m_connectionId; }
            set { this.m_connectionId = value; }
        }

        /// <summary>
        /// 获取正在连接的对端客户端终结点
        /// </summary>
        public IPEndPoint EndPoint => this.m_endPoint; //对端终结点

        /// <summary>
        /// 远程IP
        /// </summary>
        public string RemoteIPaddr => EndPoint?.Address.ToString();

        /// <summary>
        /// 远程端口
        /// </summary>
        public int RemotePort
        {
            get
            {
                if (EndPoint == null)
                {
                    return 0;
                }
                return EndPoint.Port;
            }
        }

        /// <summary>
        /// 设置需要通知外部类接收到的数据缓冲区位置
        /// </summary>
        /// <param name="bytesReceived">接收到的字节数</param>
        public void SetBytesReceived(int bytesReceived)
        {
            m_bytesReceived = bytesReceived;
        }

        /// <summary>
        /// 设置需要通知外部类接收到的数据缓冲区位置
        /// </summary>
        /// <param name="buffer">接收到的数据缓冲区位置</param>
        /// <param name="offset">相对于缓冲区的偏移量</param>
        /// <param name="bytesReceived">接收到的字节数</param>
        public void SetBuffer(byte[] buffer, int offset, int count)
        {
            m_receiveBuffer = buffer;
            m_offset = offset;
            m_count = count;
            m_bytesReceived = 0;
        }
    }
}