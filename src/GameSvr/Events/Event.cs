using System;
using SystemModule;

namespace GameSvr
{
    public class Event : IDisposable
    {
        /// <summary>
        /// 事件ID
        /// </summary>
        public int Id;
        public int nVisibleFlag = 0;
        public Envirnoment m_Envir = null;
        public int m_nX = 0;
        public int m_nY = 0;
        public int m_nEventType = 0;
        public int m_nEventParam = 0;
        protected int m_dwOpenStartTick = 0;
        /// <summary>
        /// 持续时间
        /// </summary>
        private int m_dwContinueTime = 0;
        /// <summary>
        /// 关闭时间
        /// </summary>
        public int m_dwCloseTick = 0;
        /// <summary>
        /// 是否关闭
        /// </summary>
        public bool m_boClosed = false;
        public int m_nDamage = 0;
        public TBaseObject m_OwnBaseObject = null;
        /// <summary>
        /// 开始运行时间
        /// </summary>
        public int m_dwRunStart = 0;
        /// <summary>
        /// 运行间隔
        /// </summary>
        public int m_dwRunTick = 0;
        /// <summary>
        /// 是否可见
        /// </summary>
        public bool m_boVisible = false;
        /// <summary>
        /// 是否激活
        /// </summary>
        public bool m_boActive = false;

        public Event(Envirnoment envir, int ntX, int ntY, int nType, int dwETime, bool boVisible)
        {
            Id = HUtil32.Sequence();
            m_dwOpenStartTick = HUtil32.GetTickCount();
            m_nEventType = nType;
            m_nEventParam = 0;
            m_dwContinueTime = dwETime;
            m_boVisible = boVisible;
            m_boClosed = false;
            m_Envir = envir;
            m_nX = ntX;
            m_nY = ntY;
            m_boActive = true;
            m_nDamage = 0;
            m_OwnBaseObject = null;
            m_dwRunStart = HUtil32.GetTickCount();
            m_dwRunTick = 500;
            if (m_Envir != null && m_boVisible)
            {
                m_Envir.AddToMap(m_nX, m_nY, CellType.OS_EVENTOBJECT, this);
            }
            else
            {
                m_boVisible = false;
            }
        }

        public virtual void Run()
        {
            if ((HUtil32.GetTickCount() - m_dwOpenStartTick) > m_dwContinueTime)
            {
                m_boClosed = true;
                Close();
            }
            if (m_OwnBaseObject != null && (m_OwnBaseObject.m_boGhost || m_OwnBaseObject.m_boDeath))
            {
                m_OwnBaseObject = null;
            }
        }

        public void Close()
        {
            m_dwCloseTick = HUtil32.GetTickCount();
            if (!m_boVisible) return;
            m_boVisible = false;
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