using System.Collections.Concurrent;
using System.Threading;

namespace TouchSocket.Core;

/// <summary>
/// 智能安全队列
/// </summary>
/// <typeparam name="T"></typeparam>
public class IntelligentConcurrentQueue<T> : ConcurrentQueue<T>
{
    private int m_count;

    private readonly int m_maxCount;

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="maxCount"></param>
    public IntelligentConcurrentQueue(int maxCount)
    {
        m_maxCount = maxCount;
    }

    /// <summary>
    /// 允许的最大长度
    /// </summary>
    public int MaxCount => m_maxCount;

    /// <summary>
    /// 长度
    /// </summary>
    public new int Count => m_count;

    /// <summary>
    /// 入队
    /// </summary>
    /// <param name="item"></param>
    public new void Enqueue(T item)
    {
        SpinWait.SpinUntil(Check);
        Interlocked.Increment(ref m_count);
        base.Enqueue(item);
    }

    /// <summary>
    /// 出队
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    public new bool TryDequeue(out T result)
    {
        if (base.TryDequeue(out result))
        {
            Interlocked.Decrement(ref m_count);
            return true;
        }
        return false;
    }

    private bool Check()
    {
        return m_count < m_maxCount;
    }
}