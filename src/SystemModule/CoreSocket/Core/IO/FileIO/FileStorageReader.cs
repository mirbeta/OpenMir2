namespace SystemModule.CoreSocket;

/// <summary>
/// 文件读取器
/// </summary>
[IntelligentCoder.AsyncMethodPoster(Flags = IntelligentCoder.MemberFlags.Public)]
public partial class FileStorageReader : DisposableObject
{
    private FileStorage m_fileStorage;

    private long m_position;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="fileStorage"></param>
    public FileStorageReader(FileStorage fileStorage)
    {
        m_fileStorage = fileStorage ?? throw new System.ArgumentNullException(nameof(fileStorage));
    }

    /// <summary>
    /// 析构函数
    /// </summary>
    ~FileStorageReader()
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
    /// 读取数据到缓存区
    /// </summary>
    /// <param name="buffer"></param>
    /// <param name="offset"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public int Read(byte[] buffer, int offset, int length)
    {
        int r = m_fileStorage.Read(m_position, buffer, offset, length);
        m_position += r;
        return r;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="disposing"></param>
    protected override void Dispose(bool disposing)
    {
        FilePool.TryReleaseFile(m_fileStorage.Path);
        m_fileStorage = null;
        base.Dispose(disposing);
    }
}