using GameSrv.Actor;
using GameSrv.Maps;

namespace GameSrv.Event {
    public class EventInfo : IDisposable {
        /// <summary>
        /// 事件唯一ID
        /// </summary>
        public readonly int Id;
        public VisibleFlag VisibleFlag = 0;
        public Envirnoment Envir;
        public short nX;
        public short nY;
        public int EventType;
        public int EventParam;
        protected int OpenStartTick;
        /// <summary>
        /// 持续时间
        /// </summary>
        private readonly int ContinueTime;
        /// <summary>
        /// 关闭时间
        /// </summary>
        public int CloseTick;
        public int Damage;
        public BaseObject OwnBaseObject;
        /// <summary>
        /// 开始运行时间
        /// </summary>
        public int RunStart;
        /// <summary>
        /// 运行间隔
        /// </summary>
        public int RunTick;
        /// <summary>
        /// 是否关闭
        /// </summary>
        public bool Closed;
        /// <summary>
        /// 是否可见
        /// </summary>
        public bool Visible;
        /// <summary>
        /// 是否激活
        /// </summary>
        public bool Active;
        public bool IsDispose;

        public EventInfo(Envirnoment envir, short ntX, short ntY, byte nType, int dwETime, bool boVisible) {
            Id = M2Share.ActorMgr.GetNextIdentity();
            OpenStartTick = HUtil32.GetTickCount();
            EventType = nType;
            EventParam = 0;
            ContinueTime = dwETime;
            Visible = boVisible;
            Closed = false;
            Envir = envir;
            nX = ntX;
            nY = ntY;
            Active = true;
            Damage = 0;
            OwnBaseObject = null;
            RunStart = HUtil32.GetTickCount();
            RunTick = 500;
            if (Envir != null && Visible) {
                Envir.AddToMap(nX, nY, CellType.Event, this.Id, this);
            }
            else {
                Visible = false;
            }
        }

        /// <summary>
        /// 为了防止忘记显式的调用Dispose方法
        /// </summary>
        ~EventInfo() {
            //必须为false
            Dispose(false);
        }

        public virtual void Run() {
            if ((HUtil32.GetTickCount() - OpenStartTick) > ContinueTime) {
                Closed = true;
                Close();
            }
            if (OwnBaseObject != null && (OwnBaseObject.Ghost || OwnBaseObject.Death)) {
                OwnBaseObject = null;
            }
        }

        public void Close() {
            CloseTick = HUtil32.GetTickCount();
            if (!Visible) return;
            Visible = false;
            if (Envir != null) {
                Envir.DeleteFromMap(nX, nY, CellType.Event, this.Id, null);
            }
            Envir = null;
        }

        /// <summary>执行与释放或重置非托管资源关联的应用程序定义的任务。</summary>
        public void Dispose() {
            //必须为true
            Dispose(true);
            //通知垃圾回收器不再调用终结器
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 非密封类可重写的Dispose方法，方便子类继承时可重写
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing) {
            if (IsDispose) {
                return;
            }
            //清理托管资源
            if (disposing) {

            }
            //告诉自己已经被释放
            IsDispose = true;
        }
    }
}