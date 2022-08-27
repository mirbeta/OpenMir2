using GameSvr.Actor;
using GameSvr.Maps;
using SystemModule;

namespace GameSvr.Event
{
    public class MirEvent : IDisposable
    {
        /// <summary>
        /// 事件唯一ID
        /// </summary>
        public readonly int Id;
        public int VisibleFlag = 0;
        public Envirnoment m_Envir = null;
        public int m_nX = 0;
        public int m_nY = 0;
        public int EventType = 0;
        public int m_nEventParam = 0;
        protected int m_dwOpenStartTick = 0;
        /// <summary>
        /// 持续时间
        /// </summary>
        private readonly int ContinueTime = 0;
        /// <summary>
        /// 关闭时间
        /// </summary>
        public int CloseTick = 0;
        public int Damage = 0;
        public TBaseObject OwnBaseObject = null;
        /// <summary>
        /// 开始运行时间
        /// </summary>
        public int m_dwRunStart = 0;
        /// <summary>
        /// 运行间隔
        /// </summary>
        public int m_dwRunTick = 0;
        /// <summary>
        /// 是否关闭
        /// </summary>
        public bool Closed = false;
        /// <summary>
        /// 是否可见
        /// </summary>
        public bool Visible = false;
        /// <summary>
        /// 是否激活
        /// </summary>
        public bool Active = false;

        public MirEvent(Envirnoment envir, int ntX, int ntY, int nType, int dwETime, bool boVisible)
        {
            Id = HUtil32.Sequence();
            m_dwOpenStartTick = HUtil32.GetTickCount();
            EventType = nType;
            m_nEventParam = 0;
            ContinueTime = dwETime;
            Visible = boVisible;
            Closed = false;
            m_Envir = envir;
            m_nX = ntX;
            m_nY = ntY;
            Active = true;
            Damage = 0;
            OwnBaseObject = null;
            m_dwRunStart = HUtil32.GetTickCount();
            m_dwRunTick = 500;
            if (m_Envir != null && Visible)
            {
                m_Envir.AddToMap(m_nX, m_nY, CellType.OS_EVENTOBJECT, this);
            }
            else
            {
                Visible = false;
            }
        }

        public virtual void Run()
        {
            if ((HUtil32.GetTickCount() - m_dwOpenStartTick) > ContinueTime)
            {
                Closed = true;
                Close();
            }
            if (OwnBaseObject != null && (OwnBaseObject.m_boGhost || OwnBaseObject.m_boDeath))
            {
                OwnBaseObject = null;
            }
        }

        public void Close()
        {
            CloseTick = HUtil32.GetTickCount();
            if (!Visible) return;
            Visible = false;
            if (m_Envir != null)
            {
                m_Envir.DeleteFromMap(m_nX, m_nY, CellType.OS_EVENTOBJECT, this);
            }
            m_Envir = null;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}