using System.Collections.Generic;

namespace M2Server
{
    public class TEventManager
    {
        private readonly IList<TEvent> m_EventList = null;
        private readonly IList<TEvent> m_ClosedEventList = null;

        public void Run()
        {
            TEvent executeEvent;
            for (var i = m_EventList.Count - 1; i >= 0; i--)
            {
                executeEvent = m_EventList[i];
                if (executeEvent.m_boActive &&
                    ((HUtil32.GetTickCount() - executeEvent.m_dwRunStart) > executeEvent.m_dwRunTick))
                {
                    executeEvent.m_dwRunStart = HUtil32.GetTickCount();
                    executeEvent.Run();
                    if (executeEvent.m_boClosed)
                    {
                        m_ClosedEventList.Add(executeEvent);
                        m_EventList.RemoveAt(i);
                    }
                }
            }

            for (var i = m_ClosedEventList.Count - 1; i >= 0; i--)
            {
                executeEvent = m_ClosedEventList[i];
                if ((HUtil32.GetTickCount() - executeEvent.m_dwCloseTick) > 5 * 60 * 1000)
                {
                    m_ClosedEventList.RemoveAt(i);
                    executeEvent = null;
                }
            }
        }

        public TEvent GetEvent(TEnvirnoment Envir, int nX, int nY, int nType)
        {
            TEvent result = null;
            TEvent currentEvent = null;
            for (var i = m_EventList.Count - 1; i >= 0; i--)
            {
                currentEvent = m_EventList[i];
                if ((currentEvent.m_Envir == Envir) && (currentEvent.m_nX == nX) && (currentEvent.m_nY == nY) &&
                    (currentEvent.m_nEventType == nType))
                {
                    result = currentEvent;
                    break;
                }
            }

            return result;
        }

        public void AddEvent(TEvent __Event)
        {
            m_EventList.Add(__Event);
        }

        public TEventManager()
        {
            m_EventList = new List<TEvent>();
            m_ClosedEventList = new List<TEvent>();
        }
    }
}