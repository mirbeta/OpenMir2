using System.IO;

namespace SystemModule.CoreSocket
{
    /// <summary>
    /// FileStorageStream。非线程安全。
    /// </summary>
    [IntelligentCoder.AsyncMethodPoster(Flags = IntelligentCoder.MemberFlags.Public)]
    public partial class FileStorageStream : Stream
    {
        private readonly FileStorage m_fileStorage;
        private long m_position;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fileStorage"></param>
        public FileStorageStream(FileStorage fileStorage)
        {
            m_fileStorage = fileStorage ?? throw new System.ArgumentNullException(nameof(fileStorage));
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~FileStorageStream()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: false);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override bool CanRead => m_fileStorage.FileStream.CanRead;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override bool CanSeek => m_fileStorage.FileStream.CanSeek;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override bool CanWrite => m_fileStorage.FileStream.CanWrite;

        /// <summary>
        /// 文件存储器
        /// </summary>
        public FileStorage FileStorage => m_fileStorage;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override long Length => m_fileStorage.FileStream.Length;

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public override long Position { get => m_position; set => m_position = value; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        [IntelligentCoder.AsyncMethodIgnore]
        public override void Flush()
        {
            m_fileStorage.Flush();
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public override int Read(byte[] buffer, int offset, int count)
        {
            int r = m_fileStorage.Read(m_position, buffer, offset, count);
            m_position += r;
            return r;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="origin"></param>
        /// <returns></returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    m_position = offset;
                    break;

                case SeekOrigin.Current:
                    m_position += offset;
                    break;

                case SeekOrigin.End:
                    m_position = Length + offset;
                    break;
            }
            return m_position;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="value"></param>
        public override void SetLength(long value)
        {
            m_fileStorage.FileStream.SetLength(value);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="count"></param>
        public override void Write(byte[] buffer, int offset, int count)
        {
            m_fileStorage.Write(m_position, buffer, offset, count);
            m_position += count;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            FilePool.TryReleaseFile(m_fileStorage.Path);
            base.Dispose(disposing);
        }
    }
}