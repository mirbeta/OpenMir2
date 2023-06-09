using SystemModule.Data;
using SystemModule.Enums;

namespace M2Server.Actor
{
    public class ActorEntity : IDisposable
    {
        /// <summary>
        /// 对象唯一ID
        /// </summary>
        public int ActorId { get; set; }
        /// <summary>
        /// 消息列表
        /// </summary>
        protected readonly PriorityQueue<SendMessage, byte> MsgQueue;

        public ActorEntity()
        {
            ActorId = M2Share.ActorMgr.GetNextIdentity();
            MsgQueue = new PriorityQueue<SendMessage, byte>();
        }

        public void AddMessage(SendMessage sendMessage)
        {
            MsgQueue.Enqueue(sendMessage, (byte)MessagePriority.Normal);
        }

        /// <summary>
        /// 保证多次调用Dispose方式不会抛出异常
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// 为了防止忘记显式的调用Dispose方法
        /// </summary>
        ~ActorEntity()
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
        /// 非密封类可重写的Dispose方法，方便子类继承时可重写
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
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