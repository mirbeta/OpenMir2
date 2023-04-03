using SystemModule.Core.Common;

namespace SystemModule.Core.IO.FileIO
{
    /// <summary>
    /// 文件写入器。
    /// </summary>
    [IntelligentCoder.AsyncMethodPoster(Flags = IntelligentCoder.MemberFlags.Public)]
    public partial class FileStorageWriter : DisposableObject, IWrite
    {
        private readonly FileStorage m_fileStorage;
        private long m_position;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fileStorage"></param>
        public FileStorageWriter(FileStorage fileStorage)
        {
            m_fileStorage = fileStorage ?? throw new System.ArgumentNullException(nameof(fileStorage));
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <param name="buffer"></param>
        public virtual void Write(byte[] buffer)
        {
            Write(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~FileStorageWriter()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: false);
        }

        /// <summary>
        /// 文件存储器
        /// </summary>
        public FileStorage FileStorage => m_fileStorage;

        /// <summary>
        /// 游标位置
        /// </summary>
        public int Pos
        {
            get => (int)m_position;
            set => m_position = value;
        }

        /// <summary>
        /// 游标位置
        /// </summary>
        public long Position
        {
            get => m_position;
            set => m_position = value;
        }

        /// <summary>
        /// 移动Pos到流末尾
        /// </summary>
        /// <returns></returns>
        public long SeekToEnd()
        {
            return Position = FileStorage.Length;
        }

        /// <summary>
        /// 读取数据到缓存区
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public void Write(byte[] buffer, int offset, int length)
        {
            m_fileStorage.Write(m_position, buffer, offset, length);
            m_position += length;
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