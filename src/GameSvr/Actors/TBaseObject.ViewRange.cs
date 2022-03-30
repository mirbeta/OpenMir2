using System;
using SystemModule;

namespace GameSvr
{
    public partial class TBaseObject
    {
        protected virtual void UpdateVisibleGay(TBaseObject BaseObject)
        {
            bool boIsVisible = false;
            TVisibleBaseObject VisibleBaseObject;
            if ((BaseObject.m_btRaceServer == Grobal2.RC_PLAYOBJECT) || (BaseObject.m_Master != null))// 如果是人物或宝宝则置TRUE
            {
                m_boIsVisibleActive = true;
            }
            for (var i = 0; i < m_VisibleActors.Count; i++)
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

        protected void UpdateVisibleItem(int wX, int wY, MapItem MapItem)
        {
            VisibleMapItem VisibleMapItem;
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
            VisibleMapItem = new VisibleMapItem
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

        protected void UpdateVisibleEvent(int wX, int wY, Event MapEvent)
        {
            bool boIsVisible = false;
            Event __Event;
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
            MapCellinfo MapCellInfo;
            CellObject OSObject;
            TBaseObject BaseObject;
            TVisibleBaseObject VisibleBaseObject;
            const string sExceptionMsg1 = "[Exception] TBaseObject::SearchViewRange";
            const string sExceptionMsg2 = "[Exception] TBaseObject::SearchViewRange 1-{0} {1} {2} {3} {4} {5}";
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
                        var mapCell = false;
                        MapCellInfo = m_PEnvir.GetMapCellInfo(n18, n1C, ref mapCell);
                        if (mapCell && (MapCellInfo.ObjList != null))
                        {
                            n24 = 1;
                            var nIdx = 0;
                            while (true)
                            {
                                if (MapCellInfo.Count <= nIdx)
                                {
                                    break;
                                }
                                OSObject = MapCellInfo.ObjList[nIdx];
                                if (OSObject != null)
                                {
                                    if (OSObject.CellType == CellType.OS_MOVINGOBJECT)
                                    {
                                        if ((HUtil32.GetTickCount() - OSObject.dwAddTime) >= 60 * 1000)
                                        {
                                            OSObject = null;
                                            MapCellInfo.Remove(nIdx);
                                            if (MapCellInfo.Count > 0)
                                            {
                                                continue;
                                            }
                                            MapCellInfo.Dispose();
                                            break;
                                        }
                                        BaseObject = OSObject.CellObj as TBaseObject;
                                        if (BaseObject != null)
                                        {
                                            if ((BaseObject != null) && !BaseObject.m_boDeath && !BaseObject.m_boInvisible)
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

        public virtual void SearchViewRange_Death()
        {
            if (m_PEnvir == null)
            {
                return;
            }
            m_boIsVisibleActive = false;
            for (int i = 0; i < m_VisibleActors.Count; i++)
            {
                m_VisibleActors[i].nVisibleFlag = 0;
            }
            var nStartX = m_nCurrX - m_nViewRange;
            var nEndX = m_nCurrX + m_nViewRange;
            var nStartY = m_nCurrY - m_nViewRange;
            var nEndY = m_nCurrY + m_nViewRange;
            MapCellinfo MapCellInfo;
            for (var n18 = nStartX; n18 <= nEndX; n18++)
            {
                for (var n1C = nStartY; n1C <= nEndY; n1C++)
                {
                    var mapCell = false;
                    MapCellInfo = m_PEnvir.GetMapCellInfo(n18, n1C, ref mapCell);
                    if (mapCell && (MapCellInfo.ObjList != null))
                    {
                        var nIdx = 0;
                        while (true)
                        {
                            if (MapCellInfo.Count <= nIdx)
                            {
                                break;
                            }
                            var OSObject = MapCellInfo.ObjList[nIdx];
                            if (OSObject != null)
                            {
                                if (OSObject.CellType == CellType.OS_MOVINGOBJECT)
                                {
                                    if ((HUtil32.GetTickCount() - OSObject.dwAddTime) >= 60 * 1000)
                                    {
                                        OSObject = null;
                                        MapCellInfo.Remove(nIdx);
                                        if (MapCellInfo.Count > 0)
                                        {
                                            continue;
                                        }
                                        MapCellInfo.Dispose();
                                        break;
                                    }
                                }
                                if ((OSObject.CellType == CellType.OS_ITEMOBJECT) && !m_boDeath && (m_btRaceServer > Grobal2.RC_MONSTER))
                                {
                                    if ((HUtil32.GetTickCount() - OSObject.dwAddTime) > M2Share.g_Config.dwClearDropOnFloorItemTime)
                                    {
                                        Dispose(OSObject.CellObj);
                                        Dispose(OSObject);
                                        MapCellInfo.Remove(nIdx);
                                        if (MapCellInfo.Count > 0)
                                        {
                                            continue;
                                        }
                                        MapCellInfo.Dispose();
                                    }
                                }
                            }
                            nIdx++;
                        }
                    }
                }
            }

            var n17 = 0;
            if (m_VisibleActors.Count > 0)
            {
                while (true)
                {
                    if (m_VisibleActors.Count < n17)
                    {
                        if (m_VisibleActors[n17].nVisibleFlag == 0)
                        {
                            m_VisibleActors.RemoveAt(n17);
                            m_VisibleActors[n17] = null;
                            continue;
                        }
                        n17++;
                    }
                }
            }
        }
    }
}