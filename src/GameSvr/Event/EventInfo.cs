using GameSvr.Actor;
using GameSvr.Maps;

namespace GameSvr.Event
{
    public class EventInfo : ActorEntity, IDisposable
    {
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

        public EventInfo(Envirnoment envir, short ntX, short ntY, byte nType, int dwETime, bool boVisible)
        {
            Id = HUtil32.Sequence();
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
            if (Envir != null && Visible)
            {
                Envir.AddToMap(nX, nY, CellType.Event, this);
            }
            else
            {
                Visible = false;
            }
        }

        public virtual void Run()
        {
            if ((HUtil32.GetTickCount() - OpenStartTick) > ContinueTime)
            {
                Closed = true;
                Close();
            }
            if (OwnBaseObject != null && (OwnBaseObject.Ghost || OwnBaseObject.Death))
            {
                OwnBaseObject = null;
            }
        }

        public void Close()
        {
            CloseTick = HUtil32.GetTickCount();
            if (!Visible) return;
            Visible = false;
            if (Envir != null)
            {
                Envir.DeleteFromMap(nX, nY, CellType.Event, this);
            }
            Envir = null;
        }
    }
}