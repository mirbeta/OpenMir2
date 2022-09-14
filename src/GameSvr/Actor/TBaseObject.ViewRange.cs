using GameSvr.Event;
using GameSvr.Maps;
using SystemModule;

namespace GameSvr.Actor
{
    public partial class BaseObject
    {
        protected virtual void UpdateVisibleGay(BaseObject baseObject)
        {
            bool boIsVisible = false;
            VisibleBaseObject visibleBaseObject;
            if ((baseObject.Race == Grobal2.RC_PLAYOBJECT) || (baseObject.Master != null))// 如果是人物或宝宝则置TRUE
            {
                IsVisibleActive = true;
            }
            for (var i = 0; i < VisibleActors.Count; i++)
            {
                visibleBaseObject = VisibleActors[i];
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
            VisibleActors.Add(visibleBaseObject);
        }

        protected void UpdateVisibleItem(short wX, short wY, MapItem MapItem)
        {
            VisibleMapItem visibleMapItem;
            bool boIsVisible = false;
            for (int i = 0; i < VisibleItems.Count; i++)
            {
                visibleMapItem = VisibleItems[i];
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
            VisibleItems.Add(visibleMapItem);
        }

        protected void UpdateVisibleEvent(short wX, short wY, MirEvent MapEvent)
        {
            bool boIsVisible = false;
            for (var i = 0; i < VisibleEvents.Count; i++)
            {
                var mapEvent = VisibleEvents[i];
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
            MapEvent.nX = wX;
            MapEvent.nY = wY;
            VisibleEvents.Add(MapEvent);
        }

        public bool IsVisibleHuman()
        {
            bool result = false;
            for (int i = 0; i < VisibleActors.Count; i++)
            {
                var visibleBaseObject = VisibleActors[i];
                if ((visibleBaseObject.BaseObject.Race == Grobal2.RC_PLAYOBJECT) || (visibleBaseObject.BaseObject.Master != null))
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        public virtual void SearchViewRange()
        {
            const string sExceptionMsg = "[Exception] TBaseObject::SearchViewRange {0} {1} {2} {3} {4}";
            if (Envir == null)
            {
                M2Share.Log.Error("SearchViewRange nil PEnvir");
                return;
            }
            var n24 = 0;
            IsVisibleActive = false;// 先置为FALSE
            for (var i = 0; i < VisibleActors.Count; i++)
            {
                VisibleActors[i].VisibleFlag = 0;
            }
            var nStartX = (short)(CurrX - ViewRange);
            var nEndX = (short)(CurrX + ViewRange);
            var nStartY =  (short)(CurrY - ViewRange);
            var nEndY =  (short)(CurrY + ViewRange);

            //当前坐标X，Y + 视野范围大小 看看有没有玩家

            try
            {
                for (var n18 = nStartX; n18 <= nEndX; n18++)
                {
                    for (var n1C = nStartY; n1C <= nEndY; n1C++)
                    {
                        var cellsuccess = false;
                        var cellInfo = Envir.GetCellInfo(n18, n1C, ref cellsuccess);
                        if (cellsuccess && cellInfo.IsAvailable)
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
                                        var baseObject = M2Share.ActorMgr.Get(osObject.CellObjId);
                                        if (baseObject != null)
                                        {
                                            if (!baseObject.Death && !baseObject.Invisible)
                                            {
                                                if (!baseObject.Ghost && !baseObject.FixedHideMode && !baseObject.ObMode)
                                                {
                                                    if ((Race < Grobal2.RC_ANIMAL) || (Master != null) || CrazyMode || NastyMode || WantRefMsg || ((baseObject.Master != null) && (Math.Abs(baseObject.CurrX - CurrX) <= 3) && (Math.Abs(baseObject.CurrY - CurrY) <= 3)) ||
                                                        (baseObject.Race == Grobal2.RC_PLAYOBJECT))
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
                M2Share.Log.Error(Format(sExceptionMsg, n24, CharName, MapName, CurrX, CurrY));
                M2Share.Log.Error(e.Message);
                KickException();
            }
            n24 = 2;
            try
            {
                var n18 = 0;
                while (true)
                {
                    if (VisibleActors.Count <= n18)
                    {
                        break;
                    }
                    var visibleBaseObject = VisibleActors[n18];
                    if (visibleBaseObject.VisibleFlag == VisibleFlag.Visible)
                    {
                        VisibleActors.RemoveAt(n18);
                        Dispose(visibleBaseObject);
                        continue;
                    }
                    n18++;
                }
            }
            catch
            {
                M2Share.Log.Error(Format(sExceptionMsg, n24, CharName, MapName, CurrX, CurrY));
                KickException();
            }
        }

        public void SearchViewRangeDeath()
        {
            if (Envir == null)
            {
                return;
            }
            IsVisibleActive = false;
            for (var i = 0; i < VisibleActors.Count; i++)
            {
                VisibleActors[i].VisibleFlag = 0;
            }
            var nStartX = (short)(CurrX - ViewRange);
            var nEndX = (short)(CurrX + ViewRange);
            var nStartY = (short)(CurrY - ViewRange);
            var nEndY = (short)(CurrY + ViewRange);
            for (var n18 = nStartX; n18 <= nEndX; n18++)
            {
                for (var n1C = nStartY; n1C <= nEndY; n1C++)
                {
                    var cellsuccess = false;
                    var cellInfo = Envir.GetCellInfo(n18, n1C, ref cellsuccess);
                    if (cellsuccess && cellInfo.IsAvailable)
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
                                    if ((OSObject.CellType == CellType.ItemObject) && !Death && (Race > Grobal2.RC_MONSTER))
                                    {
                                        if ((HUtil32.GetTickCount() - OSObject.AddTime) > M2Share.Config.ClearDropOnFloorItemTime)
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
                            M2Share.Log.Error(e.StackTrace);
                        }
                    }
                }
            }

            var n17 = 0;
            if (VisibleActors.Count > 0)
            {
                while (true)
                {
                    if (VisibleActors.Count <= n17)
                    {
                        break;
                    }
                    if (VisibleActors[n17].VisibleFlag == 0)
                    {
                        VisibleActors.RemoveAt(n17);
                        continue;
                    }
                    n17++;
                }
            }
        }
    }
}