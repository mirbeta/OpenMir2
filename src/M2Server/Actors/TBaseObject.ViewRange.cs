using System;
using SystemModule;

namespace M2Server
{
    public partial class TBaseObject
    {
        public virtual void UpdateVisibleGay(TBaseObject BaseObject)
        {
            bool boIsVisible = false;
            TVisibleBaseObject VisibleBaseObject;
            if ((BaseObject.m_btRaceServer == Grobal2.RC_PLAYOBJECT) || (BaseObject.m_Master != null))// 如果是人物或宝宝则置TRUE
            {
                m_boIsVisibleActive = true;
            }
            for (int i = 0; i < m_VisibleActors.Count; i++)
            {
                VisibleBaseObject = m_VisibleActors[i];
                if (VisibleBaseObject.BaseObject == BaseObject)
                {
                    VisibleBaseObject.nVisibleFlag = 1;
                    boIsVisible = true;
                    break;
                }
            }
            if (boIsVisible)
            {
                return;
            }
            VisibleBaseObject = new TVisibleBaseObject
            {
                nVisibleFlag = 2,
                BaseObject = BaseObject
            };
            m_VisibleActors.Add(VisibleBaseObject);
        }

        public void UpdateVisibleItem(int wX, int wY, TMapItem MapItem)
        {
            TVisibleMapItem VisibleMapItem;
            bool boIsVisible = false;
            for (int i = 0; i < m_VisibleItems.Count; i++)
            {
                VisibleMapItem = m_VisibleItems[i];
                if (VisibleMapItem.MapItem == MapItem)
                {
                    VisibleMapItem.nVisibleFlag = 1;
                    boIsVisible = true;
                    break;
                }
            }
            if (boIsVisible)
            {
                return;
            }
            VisibleMapItem = new TVisibleMapItem
            {
                nVisibleFlag = 2,
                nX = wX,
                nY = wY,
                MapItem = MapItem,
                sName = MapItem.Name,
                wLooks = MapItem.Looks
            };
            m_VisibleItems.Add(VisibleMapItem);
        }

        public void UpdateVisibleEvent(int wX, int wY, TEvent MapEvent)
        {
            bool boIsVisible = false;
            TEvent __Event;
            for (int i = 0; i < m_VisibleEvents.Count; i++)
            {
                __Event = m_VisibleEvents[i];
                if (__Event == MapEvent)
                {
                    __Event.nVisibleFlag = 1;
                    boIsVisible = true;
                    break;
                }
            }
            if (boIsVisible)
            {
                return;
            }
            MapEvent.nVisibleFlag = 2;
            MapEvent.m_nX = wX;
            MapEvent.m_nY = wY;
            m_VisibleEvents.Add(MapEvent);
        }

        public bool IsVisibleHuman()
        {
            bool result = false;
            TVisibleBaseObject VisibleBaseObject;
            for (int i = 0; i < m_VisibleActors.Count; i++)
            {
                VisibleBaseObject = m_VisibleActors[i];
                if ((VisibleBaseObject.BaseObject.m_btRaceServer == Grobal2.RC_PLAYOBJECT) || (VisibleBaseObject.BaseObject.m_Master != null))
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        public virtual void SearchViewRange()
        {
            TMapCellinfo MapCellInfo = null;
            TOSObject OSObject;
            TBaseObject BaseObject;
            TVisibleBaseObject VisibleBaseObject;
            const string sExceptionMsg1 = "[Exception] TBaseObject::SearchViewRange";
            const string sExceptionMsg2 = "[Exception] TBaseObject::SearchViewRange 1-%d %s %s %d %d %d";
            if (m_PEnvir == null)
            {
                M2Share.ErrorMessage("SearchViewRange nil PEnvir");
                return;
            }
            var n24 = 0;
            m_boIsVisibleActive = false;// 先置为FALSE
            try
            {
                for (var i = 0; i < m_VisibleActors.Count; i++)
                {
                    m_VisibleActors[i].nVisibleFlag = 0;
                }
            }
            catch
            {
                M2Share.ErrorMessage(sExceptionMsg1);
                KickException();
            }
            var nStartX = m_nCurrX - m_nViewRange;
            var nEndX = m_nCurrX + m_nViewRange;
            var nStartY = m_nCurrY - m_nViewRange;
            var nEndY = m_nCurrY + m_nViewRange;
            try
            {
                for (var n18 = nStartX; n18 <= nEndX; n18++)
                {
                    for (var n1C = nStartY; n1C <= nEndY; n1C++)
                    {
                        if (m_PEnvir.GetMapCellInfo(n18, n1C, ref MapCellInfo) && (MapCellInfo.ObjList != null))
                        {
                            n24 = 1;
                            var nIdx = 0;
                            while (true)
                            {
                                if (MapCellInfo.ObjList.Count <= nIdx)
                                {
                                    break;
                                }
                                OSObject = MapCellInfo.ObjList[nIdx];
                                if (OSObject != null)
                                {
                                    if (OSObject.btType == Grobal2.OS_MOVINGOBJECT)
                                    {
                                        if ((HUtil32.GetTickCount() - OSObject.dwAddTime) >= 60 * 1000)
                                        {
                                            OSObject = null;
                                            MapCellInfo.ObjList.RemoveAt(nIdx);
                                            if (MapCellInfo.ObjList.Count > 0)
                                            {
                                                continue;
                                            }
                                            MapCellInfo.ObjList = null;
                                            break;
                                        }
                                        BaseObject = OSObject.CellObj as TBaseObject;
                                        if (BaseObject != null)
                                        {
                                            if (!BaseObject.m_boGhost && !BaseObject.m_boFixedHideMode && !BaseObject.m_boObMode)
                                            {
                                                if ((m_btRaceServer < Grobal2.RC_ANIMAL) || (m_Master != null) || m_boCrazyMode || m_boNastyMode || m_boWantRefMsg || ((BaseObject.m_Master != null) && (Math.Abs(BaseObject.m_nCurrX - m_nCurrX) <= 3) && (Math.Abs(BaseObject.m_nCurrY - m_nCurrY) <= 3)) || (BaseObject.m_btRaceServer == Grobal2.RC_PLAYOBJECT))
                                                {
                                                    UpdateVisibleGay(BaseObject);
                                                }
                                            }
                                        }
                                    }
                                }
                                nIdx++;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                M2Share.ErrorMessage(format(sExceptionMsg2, new object[] { n24, m_sCharName, m_sMapName, m_nCurrX, m_nCurrY }));
                M2Share.ErrorMessage(e.Message);
                KickException();
            }

            n24 = 2;
            try
            {
                var n18 = 0;
                while (true)
                {
                    if (m_VisibleActors.Count <= n18)
                    {
                        break;
                    }
                    VisibleBaseObject = m_VisibleActors[n18];
                    if (VisibleBaseObject.nVisibleFlag == 0)
                    {
                        m_VisibleActors.RemoveAt(n18);
                        Dispose(VisibleBaseObject);
                        continue;
                    }
                    n18++;
                }
            }
            catch
            {
                M2Share.ErrorMessage(format(sExceptionMsg2, new object[] { n24, m_sCharName, m_sMapName, m_nCurrX, m_nCurrY }));
                KickException();
            }
        }
    }
}