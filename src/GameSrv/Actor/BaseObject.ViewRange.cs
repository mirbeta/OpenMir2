using GameSrv.Maps;
using SystemModule.Enums;

namespace GameSrv.Actor
{
    public partial class BaseObject
    {
        protected virtual void UpdateVisibleGay(BaseObject baseObject)
        {
            bool boIsVisible = false;
            VisibleBaseObject visibleBaseObject;
            if ((baseObject.Race == ActorRace.Play) || (baseObject.Master != null))// 如果是人物或宝宝则置TRUE
            {
                IsVisibleActive = true;
            }
            for (int i = 0; i < VisibleActors.Count; i++)
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
                VisibleFlag = VisibleFlag.Show,
                BaseObject = baseObject
            };
            VisibleActors.Add(visibleBaseObject);
        }

        public bool IsVisibleHuman()
        {
            bool result = false;
            for (int i = 0; i < VisibleActors.Count; i++)
            {
                VisibleBaseObject visibleBaseObject = VisibleActors[i];
                if ((visibleBaseObject.BaseObject.Race == ActorRace.Play) || (visibleBaseObject.BaseObject.Master != null))
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
                M2Share.Logger.Error("SearchViewRange nil PEnvir");
                return;
            }
            int n24 = 0;
            IsVisibleActive = false;// 先置为FALSE
            for (int i = 0; i < VisibleActors.Count; i++)
            {
                VisibleActors[i].VisibleFlag = VisibleFlag.Hidden;
            }
            short nStartX = (short)(CurrX - ViewRange);
            short nEndX = (short)(CurrX + ViewRange);
            short nStartY = (short)(CurrY - ViewRange);
            short nEndY = (short)(CurrY + ViewRange);
            try
            {
                for (short n18 = nStartX; n18 <= nEndX; n18++)
                {
                    for (short n1C = nStartY; n1C <= nEndY; n1C++)
                    {
                        ref MapCellInfo cellInfo = ref Envir.GetCellInfo(n18, n1C, out bool cellSuccess);
                        if (cellSuccess && cellInfo.IsAvailable)
                        {
                            n24 = 1;
                            var nIdx = 0;
                            while (true)
                            {
                                if (cellInfo.Count <= nIdx)
                                {
                                    break;
                                }
                                CellObject cellObject = cellInfo.ObjList[nIdx];
                                if (cellObject.ActorObject)
                                {
                                    if ((HUtil32.GetTickCount() - cellObject.AddTime) >= 60 * 1000)
                                    {
                                        cellInfo.Remove(nIdx);
                                        if (cellInfo.Count > 0)
                                        {
                                            continue;
                                        }
                                        cellInfo.Clear();
                                        break;
                                    }
                                    BaseObject baseObject = M2Share.ActorMgr.Get(cellObject.CellObjId);
                                    if (baseObject != null)
                                    {
                                        if (!baseObject.Death && !baseObject.Invisible)
                                        {
                                            if (!baseObject.Ghost && !baseObject.FixedHideMode && !baseObject.ObMode)
                                            {
                                                if ((Master != null) || CrazyMode || NastyMode || WantRefMsg || ((baseObject.Master != null) && (Math.Abs(baseObject.CurrX - CurrX) <= 3) && (Math.Abs(baseObject.CurrY - CurrY) <= 3)) || (baseObject.Race == ActorRace.Play))
                                                {
                                                    UpdateVisibleGay(baseObject);
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
                M2Share.Logger.Error(Format(sExceptionMsg, n24, ChrName, MapName, CurrX, CurrY));
                M2Share.Logger.Error(e.Message);
                KickException();
            }
            n24 = 2;
            try
            {
                int n18 = 0;
                while (true)
                {
                    if (VisibleActors.Count <= n18)
                    {
                        break;
                    }
                    VisibleBaseObject visibleBaseObject = VisibleActors[n18];
                    if (visibleBaseObject.VisibleFlag == VisibleFlag.Hidden)
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
                M2Share.Logger.Error(Format(sExceptionMsg, n24, ChrName, MapName, CurrX, CurrY));
                KickException();
            }
        }

        public void SearchViewRangeDeath()
        {
            if (Envir == null)
            {
                return;
            }
            if (VisibleActors.Count <= 0)
            {
                return;
            }
            IsVisibleActive = false;
            for (int i = 0; i < VisibleActors.Count; i++)
            {
                if (VisibleActors[i] == null) {
                    continue;
                }
                VisibleActors[i].VisibleFlag = VisibleFlag.Hidden;
            }
            short nStartX = (short)(CurrX - ViewRange);
            short nEndX = (short)(CurrX + ViewRange);
            short nStartY = (short)(CurrY - ViewRange);
            short nEndY = (short)(CurrY + ViewRange);
            for (short n18 = nStartX; n18 <= nEndX; n18++)
            {
                for (short n1C = nStartY; n1C <= nEndY; n1C++)
                {
                    ref MapCellInfo cellInfo = ref Envir.GetCellInfo(n18, n1C, out bool cellSuccess);
                    if (cellSuccess && cellInfo.IsAvailable)
                    {
                        for (int i = 0; i < cellInfo.ObjList.Count; i++)
                        {
                            CellObject cellObject = cellInfo.ObjList[i];
                            if (cellObject.CellObjId > 0)
                            {
                                if (cellObject.ActorObject)
                                {
                                    if ((HUtil32.GetTickCount() - cellObject.AddTime) >= 60 * 1000)
                                    {
                                        cellInfo.Remove(i);
                                        if (cellInfo.Count > 0)
                                        {
                                            continue;
                                        }
                                        cellInfo.Clear();
                                        break;
                                    }
                                }
                                if ((cellObject.CellType == CellType.Item) && !Death && (Race > ActorRace.Monster))
                                {
                                    if ((HUtil32.GetTickCount() - cellObject.AddTime) > M2Share.Config.ClearDropOnFloorItemTime)
                                    {
                                        cellInfo.Remove(i);
                                        if (cellInfo.Count > 0)
                                        {
                                            continue;
                                        }
                                        cellInfo.Clear();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            VisibleActors.Clear();
        }
    }
}