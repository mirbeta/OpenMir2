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
            VisibleBaseObject visibleBaseObject;
            if ((baseObject.m_btRaceServer == Grobal2.RC_PLAYOBJECT) || (baseObject.m_Master != null))// 如果是人物或宝宝则置TRUE
            {
                m_boIsVisibleActive = true;
            }
            for (var i = 0; i < m_VisibleActors.Count; i++)
            {
                visibleBaseObject = m_VisibleActors[i];
                if (visibleBaseObject.BaseObject == baseObject)
                {
                    visibleBaseObject.VisibleFlag = VisibleFlag.Invisible;
                    boIsVisible = true;
                    break;
                }
            }
            if (boIsVisible)
            {
                return;
            }
            visibleBaseObject = new VisibleBaseObject
            {
                VisibleFlag = VisibleFlag.Hidden,
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
                    visibleMapItem.VisibleFlag = VisibleFlag.Invisible;
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
                VisibleFlag = VisibleFlag.Hidden,
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
                    mapEvent.VisibleFlag = VisibleFlag.Invisible;
                    boIsVisible = true;
                    break;
                }
            }
            if (boIsVisible)
            {
                return;
            }
            MapEvent.VisibleFlag = VisibleFlag.Hidden;
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
            const string sExceptionMsg2 = "[Exception] TBaseObject::SearchViewRange {0} {1} {2} {3} {4}";
            if (m_PEnvir == null)
            {
                M2Share.ErrorMessage("SearchViewRange nil PEnvir");
                return;
            }
            var n24 = 0;
            m_boIsVisibleActive = false;// 先置为FALSE
            for (var i = 0; i < m_VisibleActors.Count; i++)
            {
                m_VisibleActors[i].VisibleFlag = 0;
            }
            var nStartX = (short)(m_nCurrX - m_nViewRange);
            var nEndX = (short)(m_nCurrX + m_nViewRange);
            var nStartY =  (short)(m_nCurrY - m_nViewRange);
            var nEndY =  (short)(m_nCurrY + m_nViewRange);
            try
            {
                for (var n18 = nStartX; n18 <= nEndX; n18++)
                {
                    for (var n1C = nStartY; n1C <= nEndY; n1C++)
                    {
                        var cellsuccess = false;
                        var cellInfo = m_PEnvir.GetCellInfo(n18, n1C, ref cellsuccess);
                        if (cellsuccess && (cellInfo.ObjList != null))
                        {
                            n24 = 1;
                            var nIdx = 0;
                            while (true)
                            {
                                if (cellInfo.Count <= nIdx)
                                {
                                    break;
                                }
                                var osObject = cellInfo.ObjList[nIdx];
                                if (osObject != null)
                                {
                                    if (osObject.CellType == CellType.MovingObject)
                                    {
                                        if ((HUtil32.GetTickCount() - osObject.AddTime) >= 60 * 1000)
                                        {
                                            cellInfo.Remove(osObject);
                                            if (cellInfo.Count > 0)
                                            {
                                                continue;
                                            }
                                            cellInfo.Dispose();
                                            break;
                                        }
                                        var baseObject = M2Share.ActorManager.Get(osObject.CellObjId);
                                        if (baseObject != null)
                                        {
                                            if (!baseObject.m_boDeath && !baseObject.m_boInvisible)
                                            {
                                                if (!baseObject.m_boGhost && !baseObject.m_boFixedHideMode && !baseObject.m_boObMode)
                                                {
                                                    if ((m_btRaceServer < Grobal2.RC_ANIMAL) || (m_Master != null) || m_boCrazyMode || m_boNastyMode || m_boWantRefMsg || 
                                                        ((baseObject.m_Master != null) && (Math.Abs(baseObject.m_nCurrX - m_nCurrX) <= 3) && (Math.Abs(baseObject.m_nCurrY - m_nCurrY) <= 3)) || 
                                                        (baseObject.m_btRaceServer == Grobal2.RC_PLAYOBJECT))
                                                    {
                                                        UpdateVisibleGay(baseObject);
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
                    var visibleBaseObject = m_VisibleActors[n18];
                    if (visibleBaseObject.VisibleFlag == 0)
                    {
                        m_VisibleActors.RemoveAt(n18);
                        Dispose(visibleBaseObject);
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
                m_VisibleActors[i].VisibleFlag = 0;
            }
            var nStartX = (short)(m_nCurrX - m_nViewRange);
            var nEndX = (short)(m_nCurrX + m_nViewRange);
            var nStartY = (short)(m_nCurrY - m_nViewRange);
            var nEndY = (short)(m_nCurrY + m_nViewRange);
            for (var n18 = nStartX; n18 <= nEndX; n18++)
            {
                for (var n1C = nStartY; n1C <= nEndY; n1C++)
                {
                    var cellsuccess = false;
                    var cellInfo = m_PEnvir.GetCellInfo(n18, n1C, ref cellsuccess);
                    if (cellsuccess && (cellInfo.ObjList != null))
                    {
                        try
                        {
                            for (var i = 0; i < cellInfo.Count; i++)
                            {
                                var OSObject = cellInfo.ObjList[i];
                                if (OSObject != null)
                                {
                                    if (OSObject.CellType == CellType.MovingObject)
                                    {
                                        if ((HUtil32.GetTickCount() - OSObject.AddTime) >= 60 * 1000)
                                        {
                                            cellInfo.Remove(OSObject);
                                            if (cellInfo.Count > 0)
                                            {
                                                continue;
                                            }
                                            cellInfo.Dispose();
                                            break;
                                        }
                                    }
                                    if ((OSObject.CellType == CellType.ItemObject) && !m_boDeath && (m_btRaceServer > Grobal2.RC_MONSTER))
                                    {
                                        if ((HUtil32.GetTickCount() - OSObject.AddTime) > M2Share.g_Config.dwClearDropOnFloorItemTime)
                                        {
                                            cellInfo.Remove(OSObject);
                                            if (cellInfo.Count > 0)
                                            {
                                                continue;
                                            }
                                            cellInfo.Dispose();
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e);
                            throw;
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
                    if (m_VisibleActors[n17].VisibleFlag == 0)
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