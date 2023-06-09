namespace SystemModule
{
    public struct AttackerInfo : IDisposable
    {
        public DateTime AttackDate;
        public string sGuildName;
        public IGuild Guild;

        /// <summary>执行与释放或重置非托管资源关联的应用程序定义的任务。</summary>
        public void Dispose()
        {
            //通知垃圾回收器不再调用终结器
            GC.SuppressFinalize(this);
        }
    }
}
