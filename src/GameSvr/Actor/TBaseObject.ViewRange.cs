using GameSvr.Event;
using GameSvr.Maps;
using SystemModule;

namespace GameSvr.Actor
{
    public partial class TBaseObject
    {
        protected virtual void UpdateVisibleGay(TBaseObject baseObject)
        {
            bool boIsVisible = false;
            TVisibleBaseObject visibleBaseObject;
            if ((baseObject.m_btRaceServer == Grobal2.RC_PLAYOBJECT) || (baseObject.m_Master != null))// 如果是人物或宝宝则置TRUE
            {
                m_boIsVisibleActive = true;
            }
            for (var i = 0; i < m_VisibleActors.Count; i++)
            {
                visibleBaseObject = m_VisibleActors[i];
                if (visibleBaseObject.BaseObject == baseObject)
                {
                    visibleBaseObject.nVisibleFlag = 1;
                    boIsVisible = true;
                    break;
                }
            }
            if (boIsVisible)
            {
                return;
            }
            visibleBaseObject = new TVisibleBaseObject
            {
                nVisibleFlag = 2,
                BaseObject = baseObject
            };
            m_VisibleActors.Add(visibleBaseObject);
        }

        protected void UpdateVisibleItem(int wX, int wY, MapItem MapItem)
        {
            VisibleMapItem visibleMapItem;
            bool boIsVisible = false;
            for (int i = 0; i < m_VisibleItems.Count; i++)
            {
                visibleMapItem = m_VisibleItems[i];
                if (visibleMapItem.MapItem == MapItem)
                {
                    visibleMapItem.nVisibleFlag = 1;
                    boIsVisible = true;
                    break;
                }
            }
            if (boIsVisible)
            {
                return;
            }
            visibleMapItem = new VisibleMapItem
            {
                nVisibleFlag = 2,
                nX = wX,
                nY = wY,
                MapItem = MapItem,
                sName = MapItem.Name,
                wLooks = MapItem.Looks
            };
            m_VisibleItems.Add(visibleMapItem);
        }

        protected void UpdateVisibleEvent(int wX, int wY, MirEvent MapEvent)
        {
            bool boIsVisible = false;
            for (var i = 0; i < m_VisibleEvents.Count; i++)
            {
                var mapEvent = m_VisibleEvents[i];
                if (mapEvent == MapEvent)
                {
                    mapEvent.VisibleFlag = 1;
                    boIsVisible = true;
                    break;
                }
            }
            if (boIsVisible)
            {
                return;
            }
            MapEvent.VisibleFlag = 2;
            MapEvent.m_nX = wX;
            MapEvent.m_nY = wY;
            m_VisibleEvents.Add(MapEvent);
        }

        public bool IsVisibleHuman()
        {
            bool result = false;
            for (int i = 0; i < m_VisibleActors.Count; i++)
            {
                var visibleBaseObject = m_VisibleActors[i];
                if ((visibleBaseObject.BaseObject.m_btRaceServer == Grobal2.RC_PLAYOBJECT) || (visibleBaseObject.BaseObject.m_Master != null))
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        public virtual void SearchViewRange()
        {
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
                        var MapCellInfo = m_PEnvir.GetMapCellInfo(n18, n1C, ref mapCell);
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
                                        var BaseObject = OSObject.CellObj as TBaseObject;
                                        if (BaseObject != null)
                                        {
                                            if (!BaseObject.m_boDeath && !BaseObject.m_boInvisible)
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
                M2Share.ErrorMessage(format(sExceptionMsg2, n24, m_sCharName, m_sMapName, m_nCurrX, m_nCurrY));
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
                    var VisibleBaseObject = m_VisibleActors[n18];
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

        public void SearchViewRangeDeath()
        {
            if (m_PEnvir == null)
            {
                return;
            }
            m_boIsVisibleActive = false;
            for (var i = 0; i < m_VisibleActors.Count; i++)
            {
                m_VisibleActors[i].nVisibleFlag = 0;
            }
            var nStartX = m_nCurrX - m_nViewRange;
            var nEndX = m_nCurrX + m_nViewRange;
            var nStartY = m_nCurrY - m_nViewRange;
            var nEndY = m_nCurrY + m_nViewRange;
            for (var n18 = nStartX; n18 <= nEndX; n18++)
            {
                for (var n1C = nStartY; n1C <= nEndY; n1C++)
                {
                    var mapCell = false;
                    var mapCellInfo = m_PEnvir.GetMapCellInfo(n18, n1C, ref mapCell);
                    if (mapCell && (mapCellInfo.ObjList != null))
                    {
                        var nIdx = 0;
                        while (true)
                        {
                            if (mapCellInfo.Count <= nIdx)
                            {
                                break;
                            }
                            var OSObject = mapCellInfo.ObjList[nIdx];
                            if (OSObject != null)
                            {
                                if (OSObject.CellType == CellType.OS_MOVINGOBJECT)
                                {
                                    if ((HUtil32.GetTickCount() - OSObject.dwAddTime) >= 60 * 1000)
                                    {
                                        OSObject = null;
                                        mapCellInfo.Remove(nIdx);
                                        if (mapCellInfo.Count > 0)
                                        {
                                            continue;
                                        }
                                        mapCellInfo.Dispose();
                                        break;
                                    }
                                }
                                if ((OSObject.CellType == CellType.OS_ITEMOBJECT) && !m_boDeath && (m_btRaceServer > Grobal2.RC_MONSTER))
                                {
                                    if ((HUtil32.GetTickCount() - OSObject.dwAddTime) > M2Share.g_Config.dwClearDropOnFloorItemTime)
                                    {
                                        Dispose(OSObject.CellObj);
                                        Dispose(OSObject);
                                        mapCellInfo.Remove(nIdx);
                                        if (mapCellInfo.Count > 0)
                                        {
                                            continue;
                                        }
                                        mapCellInfo.Dispose();
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
                    if (m_VisibleActors.Count <= n17)
                    {
                        break;
                    }
                    if (m_VisibleActors[n17].nVisibleFlag == 0)
                    {
                        m_VisibleActors.RemoveAt(n17);
                        continue;
                    }
                    n17++;
                }
            }
        }
    }
}