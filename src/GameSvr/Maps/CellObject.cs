namespace GameSvr.Maps
{
    /// <summary>
    /// 地图上的对象
    /// </summary>
    public sealed class CellObject : IDisposable
    {
        /// <summary>
        /// 唯一ID
        /// </summary>
        public int CellObjId;
        /// <summary>
        /// Cell类型
        /// </summary>
        public CellType CellType;
        /// <summary>
        /// 精灵对象（玩家 怪物 商人）
        /// </summary>
        public bool ActorObject;
        /// <summary>
        /// 添加时间
        /// </summary>
        public int AddTime;
        /// <summary>
        /// 对象释放已释放
        /// </summary>
        public bool IsDispose;

        /// <summary>
        /// 保证多次调用Dispose方式不会抛出异常
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// 为了防止忘记显式的调用Dispose方法
        /// </summary>
        ~CellObject()
        {
            //必须为false
            Dispose(false);
        }

        /// <summary>执行与释放或重置非托管资源关联的应用程序定义的任务。</summary>
        public void Dispose()
        {
            //必须为true
            Dispose(true);
            //通知垃圾回收器不再调用终结器
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 非必需的，只是为了更符合其他语言的规范，如C++、java
        /// </summary>
        public void Close()
        {
            Dispose();
        }

        /// <summary>
        /// 非密封类可重写的Dispose方法，方便子类继承时可重写
        /// </summary>
        /// <param name="disposing"></param>
        private void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }
            //清理托管资源
            if (disposing)
            {

            }
            //告诉自己已经被释放
            disposed = true;
        }
    }
}