using System.Threading.Tasks;
using SystemModule.Extensions;

namespace SystemModule.CoreSocket
{

    /// <summary>
    /// Pipeline读取管道
    /// </summary>
    public abstract class Pipeline : BlockReadStream, IRequestInfo
    {
        /// <summary>
        /// Pipeline读取管道
        /// </summary>
        /// <param name="client"></param>
        protected Pipeline(ITcpClientBase client)
        {
            Client = client;
        }

        /// <summary>
        /// 当前支持此管道的客户端。
        /// </summary>
        public ITcpClientBase Client { get; set; }
    }

    /// <summary>
    /// 管道数据处理适配器。
    /// 使用该适配器后，<see cref="IRequestInfo"/>将为<see cref="Pipeline"/>.
    /// </summary>
    public class PipelineDataHandlingAdapter : NormalDataHandlingAdapter
    {
        private byte[] m_buffer;
        private InternalPipeline m_pipeline;

        private Task m_task;

        /// <summary>
        /// 管道数据处理适配器。
        /// 使用该适配器后，<see cref="IRequestInfo"/>将为<see cref="Pipeline"/>.
        /// </summary>
        public PipelineDataHandlingAdapter()
        {
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            m_pipeline.SafeDispose();
            base.Dispose(disposing);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="byteBlock"></param>
        protected override void PreviewReceived(ByteBlock byteBlock)
        {
            if (m_pipeline == null || !m_pipeline.Enable)
            {
                m_task?.Wait();
                m_pipeline = new InternalPipeline(Client);
                m_task = EasyTask.Run(() =>
                {
                    try
                    {
                        GoReceived(default, m_pipeline);
                        if (m_pipeline.CanReadLen > 0)
                        {
                            m_buffer = new byte[m_pipeline.CanReadLen];
                            m_pipeline.Read(m_buffer, 0, m_buffer.Length);
                        }
                    }
                    catch
                    {
                    }
                    finally
                    {
                        m_pipeline.SafeDispose();
                    }
                });
            }
            if (m_buffer != null)
            {
                m_pipeline.InternalInput(m_buffer, 0, m_buffer.Length);
                m_buffer = null;
            }
            m_pipeline.InternalInput(byteBlock.Buffer, 0, byteBlock.Len);
        }
    }

    internal class InternalPipeline : Pipeline
    {
        private readonly object m_locker = new object();

        private bool m_disposedValue;

        /// <summary>
        /// Pipeline读取管道
        /// </summary>
        /// <param name="client"></param>
        public InternalPipeline(ITcpClientBase client) : base(client)
        {
            ReadTimeout = 60 * 1000;
        }

        public override bool CanRead => Enable;

        public override bool CanWrite => Client.CanSend;

        public bool Enable
        {
            get
            {
                lock (m_locker)
                {
                    return !m_disposedValue;
                }
            }
        }

        public override void Flush()
        {
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            Client.DefaultSend(buffer, offset, count);
        }

        internal void InternalInput(byte[] buffer, int offset, int length)
        {
            Input(buffer, offset, length);
        }

        protected override void Dispose(bool disposing)
        {
            lock (m_locker)
            {
                m_disposedValue = true;
                base.Dispose(disposing);
            }
        }
    }
}