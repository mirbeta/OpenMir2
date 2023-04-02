using System.Collections.Generic;
using System.Net.Sockets;

namespace SystemModule.Sockets;

/// <summary>
/// 这个类创建一个可以被划分和分配给SocketAsyncEventArgs对象每次操作可以使用的单独的打的缓冲区
/// 这可以使缓冲区很容易地被重复使用并且防止在内从中堆积碎片。
/// BufferManager 类暴露的操作不是线程安全的(需要做线程安全处理)
/// </summary>
internal class BufferManager
{
    /// <summary>
    /// 被缓冲区池管理的字节总数
    /// </summary>
    private readonly int _numBytes;
    /// <summary>
    /// 被BufferManager维持的基础字节数组
    /// </summary>
    private byte[] _buffer;
    /// <summary>
    /// 释放的索引池
    /// </summary>
    private readonly Stack<int> _freeIndexPool;
    /// <summary>
    /// 当前索引
    /// </summary>
    private int _currentIndex;
    /// <summary>
    /// 缓冲区大小
    /// </summary>
    private readonly int _bufferSize;

    /// <summary>
    /// 初始化缓冲区管理对象
    /// </summary>
    /// <param name="totalBytes">缓冲区管理对象管理的字节总数</param>
    /// <param name="bufferSize">每个缓冲区大小</param>
    public BufferManager(int totalBytes, int bufferSize)
    {
        _numBytes = totalBytes;
        _currentIndex = 0;
        _bufferSize = bufferSize;
        _freeIndexPool = new Stack<int>();
    }

    /// <summary>
    /// 分配被缓冲区池使用的缓冲区空间
    /// </summary>
    public void InitBuffer()
    {
        // 创建一个大的大缓冲区并且划分给每一个SocketAsyncEventArgs对象
        _buffer = new byte[_numBytes];
    }

    /// <summary>
    /// 从缓冲区池中分配一个缓冲区给指定的SocketAsyncEventArgs对象
    /// </summary>
    /// <returns>如果缓冲区被成功设置返回真否则返回假</returns>
    public void SetBuffer(SocketAsyncEventArgs args)
    {
        if (_freeIndexPool.Count > 0)
        {
            args.SetBuffer(_buffer, _freeIndexPool.Pop(), _bufferSize);
        }
        else
        {
            if ((_numBytes - _bufferSize) < _currentIndex)
            {
                return;
            }
            args.SetBuffer(_buffer, _currentIndex, _bufferSize);
            _currentIndex += _bufferSize;
        }
    }

    /// <summary>
    /// 从缓冲区池中分配一个缓冲区给指定的SocketAsyncEventArgs对象
    /// </summary>
    /// <returns>如果缓冲区被成功设置返回真否则返回假</returns>
    public void SetBuffer(SocketAsyncEventArgs args, bool tr)
    {
        if (_freeIndexPool.Count > 0)
        {
            args.SetBuffer(_buffer, _freeIndexPool.Pop(), _bufferSize);
        }
        else
        {
            args.SetBuffer(_buffer, _currentIndex, _bufferSize);
            _currentIndex += _bufferSize;
        }
    }

    /// <summary>
    /// 从一个SocketAsyncEventArgs对象上删除缓冲区，这将把缓冲区释放回缓冲区池
    /// </summary>
    public void FreeBuffer(SocketAsyncEventArgs args)
    {
        _freeIndexPool.Push(args.Offset);
        args.SetBuffer(null, 0, 0);
    }
}