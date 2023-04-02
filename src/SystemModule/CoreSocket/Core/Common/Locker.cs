using System;
using System.Threading;

namespace TouchSocket.Core;

/// <summary>
/// 读取锁
/// </summary>
public struct ReadLock : IDisposable
{
    private readonly ReaderWriterLockSlim m_locks;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="locks"></param>
    public ReadLock(ReaderWriterLockSlim locks)
    {
        m_locks = locks;
        m_locks.EnterReadLock();
    }

    /// <summary>
    /// 释放
    /// </summary>
    public void Dispose()
    {
        m_locks.ExitReadLock();
    }
}

/// <summary>
/// 写入锁
/// </summary>
public struct WriteLock : IDisposable
{
    private readonly ReaderWriterLockSlim m_locks;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="locks"></param>
    public WriteLock(ReaderWriterLockSlim locks)
    {
        m_locks = locks;
        m_locks.EnterWriteLock();
    }

    /// <summary>
    /// 释放
    /// </summary>
    public void Dispose()
    {
        m_locks.ExitWriteLock();
    }
}