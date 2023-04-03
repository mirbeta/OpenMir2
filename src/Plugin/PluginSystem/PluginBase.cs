namespace PluginSystem
{
    /// <summary>
    /// PluginBase
    /// </summary>
    public class PluginBase : DisposableObject, IPlugin
    {
        /// <inheritdoc/>
        public int Order { get; set; }
    }

    /// <summary>
    /// 具有释放的对象。
    /// 并未实现析构函数相关。
    /// </summary>
    public class DisposableObject : IDisposable
    {
        /// <summary>
        /// 判断是否已释放。
        /// </summary>
        private volatile bool m_disposedValue;

        /// <summary>
        /// 标识该对象是否已被释放
        /// </summary>
        public bool DisposedValue { get => m_disposedValue; }

        /// <summary>
        /// 调用释放，切换释放状态。
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            m_disposedValue = true;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}