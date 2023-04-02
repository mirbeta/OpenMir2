using System;
using System.Collections.Concurrent;

namespace SystemModule.CoreSocket;

/// <summary>
/// 依赖对象接口
/// </summary>
public interface IDependencyObject : IDisposable
{
    /// <summary>
    /// 获取依赖注入的值
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="dp"></param>
    /// <returns></returns>
    public TValue GetValue<TValue>(IDependencyProperty<TValue> dp);

    /// <summary>
    /// 是否有值。
    /// </summary>
    /// <param name="dp"></param>
    /// <returns></returns>
    public bool HasValue<TValue>(IDependencyProperty<TValue> dp);

    /// <summary>
    /// 重置属性值。
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="dp"></param>
    /// <returns></returns>
    public DependencyObject RemoveValue<TValue>(IDependencyProperty<TValue> dp);

    /// <summary>
    /// 设置依赖注入的值
    /// </summary>
    /// <param name="dp"></param>
    /// <param name="value"></param>
    public DependencyObject SetValue<TValue>(IDependencyProperty<TValue> dp, TValue value);
}

/// <summary>
/// 依赖项对象.
/// 线程安全。
/// </summary>
public class DependencyObject : DisposableObject, IDependencyObject, System.IDisposable
{
    [System.Diagnostics.DebuggerBrowsable(System.Diagnostics.DebuggerBrowsableState.Never)]
    private readonly ConcurrentDictionary<object, object> m_dp;

    /// <summary>
    /// 构造函数
    /// </summary>
    public DependencyObject()
    {
        m_dp = new ConcurrentDictionary<object, object>();
    }

    /// <summary>
    /// 获取依赖注入的值
    /// </summary>
    /// <param name="dp"></param>
    /// <returns></returns>
    public TValue GetValue<TValue>(IDependencyProperty<TValue> dp)
    {
        if (m_dp.TryGetValue(dp, out object value))
        {
            return (TValue)value;
        }
        else
        {
            return dp.DefauleValue;
        }
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="dp"></param>
    /// <returns></returns>
    public bool HasValue<TValue>(IDependencyProperty<TValue> dp)
    {
        return m_dp.ContainsKey(dp);
    }

    /// <summary>
    /// 移除设定值。
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="dp"></param>
    /// <returns></returns>
    public DependencyObject RemoveValue<TValue>(IDependencyProperty<TValue> dp)
    {
        m_dp.TryRemove(dp, out _);
        return this;
    }

    /// <summary>
    /// 设置依赖注入的值
    /// </summary>
    /// <param name="dp"></param>
    /// <param name="value"></param>
    public DependencyObject SetValue<TValue>(IDependencyProperty<TValue> dp, TValue value)
    {
        m_dp.AddOrUpdate(dp, value, (k, v) => v);
        return this;
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    /// <param name="disposing"></param>
    protected override void Dispose(bool disposing)
    {
        m_dp.Clear();
        base.Dispose(disposing);
    }
}