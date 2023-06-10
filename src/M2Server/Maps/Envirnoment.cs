using M2Server.Actor;
using M2Server.Castle;
using M2Server.Monster.Monsters;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using SystemModule;
using SystemModule.Common;
using SystemModule.Data;
using SystemModule.Enums;
using SystemModule.Events;
using SystemModule.NativeList.Utils;

namespace M2Server
{
    public class Envirnoment : IEnvirnoment, IDisposable
    {
        /// <summary>
        /// 怪物数量
        /// </summary>
        public int MonCount => monCount;
        /// <summary>
        /// 玩家数量
        /// </summary>
        public int HumCount => humCount;
        public short Width { get; set; }
        public short Height { get; set; }
        public string MapFileName { get; set; }
        public string MapName { get; set; }
        public string MapDesc { get; set; }
        private MapCellInfo[] _cellArray;
        public short MinMap { get; set; }
        public byte ServerIndex { get; set; }
        /// <summary>
        /// 进入本地图所需等级
        /// </summary>
        public int EnterLevel { get; }
        public MapInfoFlag Flag { get; set; }
        public bool ChFlag { get; set; }
        /// <summary>
        /// 门
        /// </summary>
        public IList<MapDoor> DoorList { get; set; }
        /// <summary>
        /// 任务
        /// </summary>
        private readonly IList<MapQuestInfo> QuestList;
        private int monCount;
        private int humCount;
        public IList<PointInfo> PointList { get; set; }

        public Envirnoment()
        {
            ServerIndex = 0;
            MinMap = 0;
            monCount = 0;
            humCount = 0;
            Flag = new MapInfoFlag();
            DoorList = new List<MapDoor>();
            QuestList = new List<MapQuestInfo>();
            PointList = new List<PointInfo>();
        }

        ~Envirnoment()
        {
            Dispose(false);
        }

        public static bool AllowMagics(string magicName)
        {
            return true;
        }

        /// <summary>
        /// 检测地图是否禁用技能
        /// </summary>
        /// <returns></returns>
        public static bool AllowMagics(short magicId, int type)
        {
            return true;
        }

        public bool AddItemToMap(int nX, int nY, MapItem mapItem)
        {
            if (mapItem.ItemId == 0)
            {
                return false;
            }
            if (!CellMatch(nX, nY))
            {
                return false;
            }
            const string sExceptionMsg = "[Exception] Envirnoment::AddItemToMap";
            bool result = false;
            try
            {
                bool addSuccess = false;
                ref MapCellInfo cellInfo = ref GetCellInfo(nX, nY, out bool cellSuccess);
                if (cellSuccess && cellInfo.Valid)
                {
                    if (cellInfo.ObjList == null)
                    {
                        cellInfo.ObjList = new NativeList<CellObject>();
                    }
                    if (cellInfo.IsAvailable)
                    {
                        if (string.Compare(mapItem.Name, Grobal2.StringGoldName, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            for (int i = 0; i < cellInfo.ObjList.Count; i++)
                            {
                                CellObject cellObject = cellInfo.ObjList[i];
                                if (cellObject.CellType == CellType.Item)
                                {
                                    MapItem cellItem = M2Share.CellObjectMgr.Get<MapItem>(cellObject.CellObjId);
                                    if (cellItem.ItemId == 0)
                                    {
                                        continue;
                                    }
                                    if (string.Compare(mapItem.Name, Grobal2.StringGoldName, StringComparison.OrdinalIgnoreCase) == 0)
                                    {
                                        int nGoldCount = mapItem.Count + mapItem.Count;
                                        if (nGoldCount <= 2000)
                                        {
                                            mapItem.Count = nGoldCount;
                                            mapItem.Looks = M2Share.GetGoldShape(nGoldCount);
                                            mapItem.AniCount = 0;
                                            mapItem.Reserved = 0;
                                            result = true;
                                            cellInfo.Update(i, ref cellObject);
                                            addSuccess = true;
                                        }
                                    }
                                }
                            }
                        }
                        if (!addSuccess && cellInfo.Count >= 5)
                        {
                            result = false;
                            addSuccess = true;
                        }
                    }
                    if (!addSuccess)
                    {
                        CellObject cellObject = new CellObject
                        {
                            CellType = CellType.Item,
                            CellObjId = mapItem.ItemId,
                            AddTime = HUtil32.GetTickCount()
                        };
                        cellInfo.Add(cellObject);
                        M2Share.CellObjectMgr.Add(cellObject.CellObjId, mapItem);
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                M2Share.Logger.Error(sExceptionMsg);
                M2Share.Logger.Error(ex);
            }
            return result;
        }

        public void AddMapRoute(int nX, int nY, MapRouteItem mapRoute)
        {
            if (!CellMatch(nX, nY))
            {
                return;
            }
            ref MapCellInfo cellInfo = ref GetCellInfo(nX, nY, out bool cellSuccess);
            if (cellSuccess && cellInfo.Valid)
            {
                if (cellInfo.ObjList == null)
                {
                    cellInfo.ObjList = new NativeList<CellObject>();
                }
                CellObject cellObject = new CellObject
                {
                    CellType = CellType.MapRoute,
                    CellObjId = mapRoute.RouteId,
                    AddTime = HUtil32.GetTickCount()
                };
                cellInfo.Add(cellObject);
                M2Share.CellObjectMgr.Add(cellObject.CellObjId, mapRoute);
            }
        }

        public void AddMapDoor(int nX, int nY, MapDoor mapDoor)
        {
            if (!CellMatch(nX, nY))
            {
                return;
            }
            ref MapCellInfo cellInfo = ref GetCellInfo(nX, nY, out bool cellSuccess);
            if (cellSuccess && cellInfo.Valid)
            {
                if (cellInfo.ObjList == null)
                {
                    cellInfo.ObjList = new NativeList<CellObject>();
                }
                CellObject cellObject = new CellObject
                {
                    CellType = CellType.Door,
                    CellObjId = mapDoor.DoorId,
                    AddTime = HUtil32.GetTickCount()
                };
                cellInfo.Add(cellObject);
                if (cellObject.CellType is CellType.Door or CellType.Event)
                {
                    M2Share.CellObjectMgr.Add(cellObject.CellObjId, mapDoor);
                }
            }
        }

        public void AddMapEvent(int nX, int nY, MapEvent mapEvent)
        {
            if (mapEvent == null)
            {
                return;
            }
            if (!CellMatch(nX, nY))
            {
                return;
            }
            ref MapCellInfo cellInfo = ref GetCellInfo(nX, nY, out bool cellSuccess);
            if (cellSuccess && cellInfo.Valid)
            {
                if (cellInfo.ObjList == null)
                {
                    cellInfo.ObjList = new NativeList<CellObject>();
                }
                CellObject cellObject = new CellObject
                {
                    CellType = CellType.Event,
                    CellObjId = mapEvent.Id,
                    AddTime = HUtil32.GetTickCount()
                };
                cellInfo.Add(cellObject);
                M2Share.CellObjectMgr.Add(cellObject.CellObjId, mapEvent);
            }
        }

        /// <summary>
        /// 添加对象到地图
        /// </summary>
        /// <returns></returns>
        public bool AddMapObject(int nX, int nY, CellType cellType, int cellId, IActor mapObject)
        {
            if (mapObject == null)
            {
                return false;
            }
            if (!CellMatch(nX, nY))
            {
                return false;
            }
            const string sExceptionMsg = "[Exception] Envirnoment::AddToMap";
            bool result = false;
            try
            {
                ref MapCellInfo cellInfo = ref GetCellInfo(nX, nY, out bool cellSuccess);
                if (cellSuccess && cellInfo.Valid)
                {
                    if (cellInfo.ObjList == null)
                    {
                        cellInfo.ObjList = new NativeList<CellObject>();
                    }
                    CellObject cellObject = new CellObject
                    {
                        CellType = cellType,
                        CellObjId = cellId,
                        AddTime = HUtil32.GetTickCount()
                    };
                    if (!mapObject.AddToMaped)
                    {
                        mapObject.DelFormMaped = false;
                        mapObject.AddToMaped = true;
                        AddObject(mapObject);
                    }
                    cellObject.ActorObject = true;
                    cellInfo.Add(cellObject);
                    result = true;
                }
            }
            catch (Exception ex)
            {
                M2Share.Logger.Error(sExceptionMsg);
                M2Share.Logger.Error(ex);
            }
            return result;
        }

        public void AddDoorToMap()
        {
            for (int i = 0; i < DoorList.Count; i++)
            {
                MapDoor door = DoorList[i];
                AddMapDoor(door.nX, door.nY, door);
            }
        }

        public bool CellValid(int nX, int nY)
        {
            if (nX >= 0 && nX < Width && nY >= 0 && nY < Height)
            {
                return _cellArray[nX * Height + nY].Valid;
            }
            return true;
        }

        public bool CellMatch(int nX, int nY)
        {
            return nX >= 0 && nX < Width && nY >= 0 && nY < Height;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref MapCellInfo GetCellInfo(int nX, int nY, out bool success)
        {
            if (nX >= 0 && nX < Width && nY >= 0 && nY < Height)
            {
                ref MapCellInfo cellInfo = ref _cellArray[nX * Height + nY];
                if (cellInfo.Valid)
                {
                    success = true;
                    return ref cellInfo;
                }
            }
            success = false;
            return ref _cellArray[0];
        }

        public bool MoveToMovingObject(int nCx, int nCy, IActor cert, int nX, int nY, bool boFlag)
        {
            if (!CellMatch(nX, nY))
            {
                return false;
            }
            const string sExceptionMsg = "[Exception] TEnvirnoment::MoveToMovingObject";
            bool canMove = true;
            bool result = false;
            try
            {
                ref MapCellInfo cellInfo = ref GetCellInfo(nX, nY, out bool cellSuccess);
                if (!boFlag && cellSuccess)
                {
                    if (cellInfo.Valid && cellInfo.IsAvailable)
                    {
                        for (int i = 0; i < cellInfo.ObjList.Count; i++)
                        {
                            CellObject cellObject = cellInfo.ObjList[i];
                            if (cellObject.ActorObject)
                            {
                                IActor baseObject = M2Share.ActorMgr.Get(cellObject.CellObjId);
                                if (baseObject != null)
                                {
                                    if (!baseObject.Ghost && !baseObject.Death && !baseObject.FixedHideMode && !baseObject.ObMode)
                                    {
                                        if (baseObject.CellType == CellType.CastleDoor && ((CastleDoor)baseObject).HoldPlace)
                                        {
                                            canMove = false;
                                            break;
                                        }
                                        canMove = false;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        canMove = true;
                    }
                }
                if (canMove && cellInfo.Valid)
                {
                    CellObject moveObject = default;
                    ref var oldCellInfo = ref GetCellInfo(nCx, nCy, out bool oldSuccess);
                    if (oldSuccess && oldCellInfo.IsAvailable)
                    {
                        for (int i = 0; i < oldCellInfo.ObjList.Count; i++)
                        {
                            moveObject = oldCellInfo.ObjList[i];
                            if (moveObject.CellObjId == cert.ActorId && moveObject.ActorObject)
                            {
                                oldCellInfo.Remove(i);
                                if (oldCellInfo.Count > 0)
                                {
                                    continue;
                                }
                                oldCellInfo.Clear();
                                break;
                            }
                        }
                    }
                    if (cellSuccess)
                    {
                        cellInfo.ObjList ??= new NativeList<CellObject>();
                        if (moveObject.CellObjId == 0)
                        {
                            moveObject.CellType = cert.CellType;
                            moveObject.CellObjId = cert.ActorId;
                            switch (cert.CellType)
                            {
                                case CellType.Play:
                                case CellType.Monster:
                                case CellType.Merchant:
                                    moveObject.ActorObject = true;
                                    break;
                                default:
                                    moveObject.ActorObject = false;
                                    break;
                            }
                        }
                        moveObject.AddTime = HUtil32.GetTickCount();
                        cellInfo.Add(moveObject);
                        result = true;
                    }
                }
            }
            catch (Exception e)
            {
                M2Share.Logger.Error(sExceptionMsg);
                M2Share.Logger.Error(e.StackTrace);
            }
            return result;
        }

        /// <summary>
        /// 检查地图指定座标是否可以移动
        /// </summary>
        /// <param name="nX"></param>
        /// <param name="nY"></param>
        /// <param name="boFlag">如果为TRUE 则忽略座标上是否有角色</param>
        /// <returns> 返回值 True 为可以移动，False 为不可以移动</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool CanWalk(int nX, int nY, bool boFlag)
        {
            bool result = false;
            ref MapCellInfo cellInfo = ref GetCellInfo(nX, nY, out var cellSuccess);
            if (cellSuccess && cellInfo.Valid)
            {
                if (boFlag)
                {
                    return true;
                }
                result = true;
                if (cellInfo.IsAvailable)
                {
                    for (int i = 0; i < cellInfo.ObjList.Count; i++)
                    {
                        CellObject cellObject = cellInfo.ObjList[i];
                        if (cellObject.ActorObject)
                        {
                            IActor baseObject = M2Share.ActorMgr.Get(cellObject.CellObjId);
                            if (baseObject != null)
                            {
                                if (baseObject.CellType == CellType.CastleDoor)
                                {
                                    if (!baseObject.Ghost && ((CastleDoor)baseObject).HoldPlace && !baseObject.Death && !baseObject.FixedHideMode && !baseObject.ObMode)
                                    {
                                        result = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    if (!baseObject.Ghost && !baseObject.Death && !baseObject.FixedHideMode && !baseObject.ObMode)
                                    {
                                        result = false;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        public bool CanWalk(int nX, int nY)
        {
            return CanWalk(nX, nY, false);
        }

        /// <summary>
        /// 检查地图指定座标上是否有物品
        /// </summary>
        /// <param name="nX"></param>
        /// <param name="nY"></param>
        /// <param name="boFlag">如果为TRUE 则忽略座标上是否有角色</param>
        /// <param name="boItem"></param>
        /// <returns>返回值 True 为可以移动，False 为不可以移动</returns>
        public bool CanWalkOfItem(int nX, int nY, bool boFlag, bool boItem)
        {
            bool result = true;
            ref MapCellInfo cellInfo = ref GetCellInfo(nX, nY, out var cellSuccess);
            if (cellSuccess && cellInfo.Valid)
            {
                if (cellInfo.IsAvailable)
                {
                    for (int i = 0; i < cellInfo.ObjList.Count; i++)
                    {
                        CellObject cellObject = cellInfo.ObjList[i];
                        if (!boFlag && cellObject.CellObjId > 0 && cellObject.ActorObject)
                        {
                            IActor baseObject = M2Share.ActorMgr.Get(cellObject.CellObjId);
                            if (baseObject != null)
                            {
                                if (baseObject.CellType == CellType.CastleDoor)
                                {
                                    if (!baseObject.Ghost && ((CastleDoor)baseObject).HoldPlace && !baseObject.Death && !baseObject.FixedHideMode && !baseObject.ObMode)
                                    {
                                        result = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    if (!baseObject.Ghost && !baseObject.Death && !baseObject.FixedHideMode && !baseObject.ObMode)
                                    {
                                        result = false;
                                        break;
                                    }
                                }
                            }
                        }
                        if (!boItem && cellObject.CellType == CellType.Item)
                        {
                            result = false;
                            break;
                        }
                    }
                }
            }
            return result;
        }

        public bool CanWalkEx(int nX, int nY, bool boFlag)
        {
            bool result = false;
            ref MapCellInfo cellInfo = ref GetCellInfo(nX, nY, out var cellSuccess);
            if (cellSuccess && cellInfo.Valid)
            {
                result = true;
                if (!boFlag && cellInfo.IsAvailable)
                {
                    for (int i = 0; i < cellInfo.ObjList.Count; i++)
                    {
                        CellObject cellObject = cellInfo.ObjList[i];
                        if (cellObject.ActorObject)
                        {
                            IActor baseObject = M2Share.ActorMgr.Get(cellObject.CellObjId);
                            if (baseObject != null)
                            {
                                IUserCastle castle = M2Share.CastleMgr.InCastleWarArea(baseObject);
                                if (SystemShare.Config.boWarDisHumRun && castle != null && castle.UnderWar)
                                {

                                }
                                else
                                {
                                    switch (baseObject.Race)
                                    {
                                        case ActorRace.Play:
                                            {
                                                if (SystemShare.Config.boRunHuman || Flag.RunHuman)
                                                {
                                                    continue;
                                                }
                                                break;
                                            }
                                        case ActorRace.NPC:
                                            {
                                                if (SystemShare.Config.boRunNpc)
                                                {
                                                    continue;
                                                }
                                                break;
                                            }
                                        case ActorRace.Guard:
                                        case ActorRace.ArcherGuard:
                                            {
                                                if (SystemShare.Config.boRunGuard)
                                                {
                                                    continue;
                                                }
                                                break;
                                            }
                                        default:
                                            {
                                                if (SystemShare.Config.boRunMon || Flag.RunMon)
                                                {
                                                    continue;
                                                }
                                                break;
                                            }
                                    }
                                }
                                if (baseObject.CellType == CellType.CastleDoor)
                                {
                                    if (!baseObject.Ghost && ((CastleDoor)baseObject).HoldPlace && !baseObject.Death && !baseObject.FixedHideMode && !baseObject.ObMode)
                                    {
                                        result = false;
                                        break;
                                    }
                                }
                                else
                                {
                                    if (!baseObject.Ghost && !baseObject.Death && !baseObject.FixedHideMode && !baseObject.ObMode)
                                    {
                                        result = false;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 从地图指定坐标上删除对象
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int DeleteFromMap(int nX, int nY, CellType cellType, int cellId, IActor mapObject)
        {
            const string sExceptionMsg = "[Exception] TEnvirnoment::DeleteFromMap -> Except {0}";
            int result = -1;
            ref MapCellInfo cellInfo = ref GetCellInfo(nX, nY, out bool cellSuccess);
            if (cellSuccess && cellInfo.IsAvailable)
            {
                try
                {
                    var nIdx = 0;
                    while (true)
                    {
                        if (cellInfo.Count <= nIdx)
                        {
                            break;
                        }
                        CellObject cellObject = cellInfo.ObjList[nIdx];
                        if (cellObject.CellObjId > 0)
                        {
                            if (cellObject.CellType == cellType && cellObject.CellObjId == cellId)
                            {
                                cellInfo.Remove(nIdx);
                                result = 1;
                                if (cellObject.ActorObject && mapObject != null && !mapObject.DelFormMaped)
                                {
                                    mapObject.DelFormMaped = true;
                                    mapObject.AddToMaped = false;
                                    DelObjectCount(mapObject);// 减地图人物怪物计数
                                }
                                if (cellType != CellType.Monster)
                                {
                                    M2Share.CellObjectMgr.Remove(cellId);//删除物品
                                }
                                if (cellInfo.Count > 0)
                                {
                                    continue;
                                }
                                cellInfo.Clear();
                                break;
                            }
                        }
                        else
                        {
                            if (nIdx < cellInfo.Count)
                            {
                                cellInfo.Remove(nIdx);
                            }
                            if (cellInfo.Count > 0)
                            {
                                continue;
                            }
                            cellInfo.Clear();
                            break;
                        }
                        nIdx++;
                    }
                }
                catch
                {
                    M2Share.Logger.Error(string.Format(sExceptionMsg, cellType));
                }
            }
            else
            {
                result = 0;
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool GetItem(int nX, int nY, ref MapItem mapItem)
        {
            ChFlag = false;
            ref MapCellInfo cellInfo = ref GetCellInfo(nX, nY, out bool cellSuccess);
            if (cellSuccess && cellInfo.IsAvailable)
            {
                ChFlag = true;
                for (int i = 0; i < cellInfo.ObjList.Count; i++)
                {
                    CellObject cellObject = cellInfo.ObjList[i];
                    switch (cellObject.CellType)
                    {
                        case CellType.Item:
                            mapItem = M2Share.CellObjectMgr.Get<MapItem>(cellObject.CellObjId);
                            return true;
                        case CellType.MapRoute:
                            ChFlag = false;
                            break;
                        case CellType.Play:
                        case CellType.Monster:
                        case CellType.Merchant:
                            IActor baseObject = M2Share.ActorMgr.Get(cellObject.CellObjId);
                            if (baseObject != null && !baseObject.Death)
                            {
                                ChFlag = false;
                            }
                            break;
                    }
                }
            }
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetItemEx(int nX, int nY, ref int nCount)
        {
            int result = 0;
            nCount = 0;
            ChFlag = false;
            ref var cellInfo = ref GetCellInfo(nX, nY, out bool cellSuccess);
            if (cellSuccess && cellInfo.Valid)
            {
                ChFlag = true;
                if (cellInfo.IsAvailable)
                {
                    for (int i = 0; i < cellInfo.ObjList.Count; i++)
                    {
                        CellObject cellObject = cellInfo.ObjList[i];
                        switch (cellObject.CellType)
                        {
                            case CellType.Item:
                                result = cellObject.CellObjId;
                                nCount++;
                                break;
                            case CellType.MapRoute:
                                ChFlag = false;
                                break;
                            case CellType.Monster:
                            case CellType.Merchant:
                            case CellType.Play:
                                {
                                    IActor baseObject = M2Share.ActorMgr.Get(cellObject.CellObjId);
                                    if (baseObject != null && !baseObject.Death)
                                    {
                                        ChFlag = false;
                                    }
                                    break;
                                }
                        }
                    }
                }
            }
            return result;
        }

        public bool IsCheapStuff()
        {
            return QuestList.Any();
        }

        public bool AddMineToEvent<T>(int nX, int nY, T stoneMineEvent) where T : StoneMineEvent
        {
            ref MapCellInfo cellInfo = ref GetCellInfo(nX, nY, out bool cellSuccess);
            if (cellSuccess && cellInfo.Valid)
            {
                cellInfo.ObjList ??= new NativeList<CellObject>();
                CellObject cellObject = new CellObject();
                cellObject.CellType = CellType.Event;
                cellObject.CellObjId = stoneMineEvent.Id;
                cellObject.AddTime = HUtil32.GetTickCount();
                cellInfo.Add(cellObject);
                M2Share.CellObjectMgr.Add(cellObject.CellObjId, stoneMineEvent);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 添加矿石到地图上
        /// </summary>
        /// <returns></returns>
        public bool AddToMapMineEvent<T>(int nX, int nY, T stoneMineEvent) where T : StoneMineEvent
        {
            const string sExceptionMsg = "[Exception] Envirnoment::AddToMapMineEvent ";
            try
            {
                ref MapCellInfo cellInfo = ref GetCellInfo(nX, nY, out bool cellSuccess);
                if (cellSuccess && !cellInfo.Valid) //不动走动的地方才允许放矿
                {
                    bool isSpace = false;// 人物可以走到的地方才放上矿
                    for (int x = nX - 1; x <= nX + 1; x++)
                    {
                        for (int y = nY - 1; y <= nY + 1; y++)
                        {
                            if (CellValid(x, y))
                            {
                                isSpace = true;
                            }
                            if (isSpace)
                            {
                                break;
                            }
                        }
                        if (isSpace)
                        {
                            break;
                        }
                    }
                    if (isSpace)
                    {
                        cellInfo.ObjList ??= new NativeList<CellObject>();
                        CellObject cellObject = new CellObject
                        {
                            CellType = CellType.Event,
                            CellObjId = stoneMineEvent.Id,
                            AddTime = HUtil32.GetTickCount()
                        };
                        cellInfo.Add(cellObject);
                        M2Share.CellObjectMgr.Add(cellObject.CellObjId, stoneMineEvent);
                        return true;
                    }
                }
            }
            catch
            {
                M2Share.Logger.Error(sExceptionMsg);
            }
            return false;
        }

        /// <summary>
        /// 刷新在地图上位置的时间
        /// </summary>
        public void VerifyMapTime(int nX, int nY, IActor baseObject)
        {
            bool boVerify = false;
            const string sExceptionMsg = "[Exception] TEnvirnoment::VerifyMapTime";
            try
            {
                ref MapCellInfo cellInfo = ref GetCellInfo(nX, nY, out bool cellSuccess);
                if (cellSuccess && cellInfo.IsAvailable)
                {
                    for (int i = 0; i < cellInfo.ObjList.Count; i++)
                    {
                        CellObject cellObject = cellInfo.ObjList[i];
                        if ((HUtil32.GetTickCount() - cellObject.AddTime) >= 60 * 1000)
                        {
                            boVerify = true;
                            cellInfo.Remove(i);
                            if (cellInfo.Count > 0)
                            {
                                continue;
                            }
                            cellInfo.Clear();
                            break;
                        }
                        else
                        {
                            if (cellObject.ActorObject && cellObject.CellObjId == baseObject.ActorId)
                            {
                                cellInfo.Update(i, ref cellObject);
                                boVerify = true;
                                break;
                            }
                        }
                    }
                }
                if (!boVerify)
                {
                    AddMapObject(nX, nY, baseObject.CellType, baseObject.ActorId, baseObject);
                }
            }
            catch
            {
                M2Share.Logger.Error(sExceptionMsg);
            }
        }

        public bool LoadMapData(string sMapFile)
        {
            bool result = false;
            int n24;
            int point;
            MapDoor door;
            try
            {
                if (File.Exists(sMapFile))
                {
                    using FileStream fileStream = new FileStream(sMapFile, FileMode.Open, FileAccess.Read);
                    if (fileStream.Length <= 52)
                    {
                        fileStream.Close();
                        fileStream.Dispose();
                        return false;
                    }
                    using BinaryReader binReader = new BinaryReader(fileStream);
                    Width = binReader.ReadInt16();
                    Height = binReader.ReadInt16();
                    Initialize(Width, Height);
                    fileStream.Position = 52;
                    bool isInitialize = true;

                    MapUnitInfo mapUnitInfo = new MapUnitInfo();
                    for (int nW = 0; nW < Width; nW++)
                    {
                        n24 = nW * Height;
                        for (int nH = 0; nH < Height; nH++)
                        {
                            mapUnitInfo.wBkImg = binReader.ReadUInt16();
                            mapUnitInfo.wMidImg = binReader.ReadUInt16();
                            mapUnitInfo.wFrImg = binReader.ReadUInt16();
                            mapUnitInfo.btDoorIndex = binReader.ReadByte();
                            mapUnitInfo.btDoorOffset = binReader.ReadByte();
                            mapUnitInfo.btAniFrame = binReader.ReadByte();
                            mapUnitInfo.btAniTick = binReader.ReadByte();
                            mapUnitInfo.btArea = binReader.ReadByte();
                            mapUnitInfo.btLight = binReader.ReadByte();
                            if ((mapUnitInfo.wBkImg & 0x8000) != 0)// wBkImg High
                            {
                                _cellArray[n24 + nH].Attribute = CellAttribute.HighWall;
                                isInitialize = false;
                            }
                            if ((mapUnitInfo.wFrImg & 0x8000) != 0)// wFrImg High
                            {
                                _cellArray[n24 + nH].Attribute = CellAttribute.LowWall;
                                isInitialize = false;
                            }
                            if (isInitialize)
                            {
                                _cellArray[n24 + nH].ObjList = new NativeList<CellObject>();
                            }
                            if ((mapUnitInfo.btDoorIndex & 0x80) != 0)
                            {
                                point = mapUnitInfo.btDoorIndex & 0x7F;
                                if (point > 0)
                                {
                                    door = new MapDoor
                                    {
                                        nX = (short)nW,
                                        nY = (short)nH,
                                        n08 = point,
                                        Status = null
                                    };
                                    for (int i = 0; i < DoorList.Count; i++)
                                    {
                                        if (Math.Abs(DoorList[i].nX - door.nX) <= 10)
                                        {
                                            if (Math.Abs(DoorList[i].nY - door.nY) <= 10)
                                            {
                                                if (DoorList[i].n08 == point)
                                                {
                                                    door.Status = DoorList[i].Status;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    if (door.Status == null)
                                    {
                                        door.Status = new DoorStatus
                                        {
                                            Opened = false,
                                            OpenTick = 0
                                        };
                                    }
                                    DoorList.Add(door);
                                }
                            }
                        }
                    }

                    binReader.Close();
                    binReader.Dispose();
                    fileStream.Close();
                    fileStream.Dispose();
                    result = true;
                }

                string pointFileName = M2Share.GetEnvirFilePath("Point", $"{sMapFile}.txt");
                if (File.Exists(pointFileName))
                {
                    StringList loadList = new StringList();
                    loadList.LoadFromFile(pointFileName);
                    string sX = string.Empty;
                    string sY = string.Empty;
                    for (int i = 0; i < loadList.Count; i++)
                    {
                        string line = loadList[i];
                        if (string.IsNullOrEmpty(line) || line.StartsWith(";"))
                        {
                            continue;
                        }
                        line = HUtil32.GetValidStr3(line, ref sX, new[] { ',', '\t' });
                        line = HUtil32.GetValidStr3(line, ref sY, new[] { ',', '\t' });
                        short nX = HUtil32.StrToInt16(sX, -1);
                        short nY = HUtil32.StrToInt16(sY, -1);
                        if (nX >= 0 && nY >= 0 && nX < Width && nY < Height)
                        {
                            PointList.Add(new PointInfo(nX, nY));
                        }
                    }
                }
            }
            catch (Exception)
            {
                M2Share.Logger.Error("[Exception] TEnvirnoment.LoadMapData");
            }
            return result;
        }

        private void Initialize(short nWidth, short nHeight)
        {
            if (nWidth > 1 && nHeight > 1)
            {
                if (_cellArray != null)
                {
                    for (int nW = 0; nW < Width; nW++)
                    {
                        for (int nH = 0; nH < Height; nH++)
                        {
                            if (_cellArray[nW * Height + nH].ObjList != null)
                            {
                                _cellArray[nW * Height + nH] = default;
                            }
                        }
                    }
                    _cellArray = null;
                }
                Width = nWidth;
                Height = nHeight;
                //_cellArray = new MapCellInfo[nWidth * nHeight];
                //_cellArray = GC.AllocateUninitializedArray<MapCellInfo>(nWidth * nHeight);
                _cellArray = GC.AllocateArray<MapCellInfo>(nWidth * nHeight);
            }
        }

        public int GetXYObjCount(int nX, int nY)
        {
            int result = 0;
            ref MapCellInfo cellInfo = ref GetCellInfo(nX, nY, out var cellSuccess);
            if (cellSuccess && cellInfo.IsAvailable)
            {
                for (int i = 0; i < cellInfo.ObjList.Count; i++)
                {
                    CellObject cellObject = cellInfo.ObjList[i];
                    if (cellObject.ActorObject)
                    {
                        IActor baseObject = M2Share.ActorMgr.Get(cellObject.CellObjId);
                        if (baseObject != null)
                        {
                            if (baseObject.CellType == CellType.CastleDoor)
                            {
                                if (!baseObject.Ghost && ((CastleDoor)baseObject).HoldPlace && !baseObject.Death && !baseObject.FixedHideMode && !baseObject.ObMode)
                                {
                                    result++;
                                }
                            }
                            else
                            {
                                if (!baseObject.Ghost && !baseObject.Death && !baseObject.FixedHideMode && !baseObject.ObMode)
                                {
                                    result++;
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        public bool GetNextPosition(short sx, short sy, byte ndir, int nFlag, ref short snx, ref short sny)
        {
            snx = sx;
            sny = sy;
            switch (ndir)
            {
                case Direction.Up:
                    if (sny > nFlag - 1)
                    {
                        sny -= (short)nFlag;
                    }
                    break;
                case Direction.Down:
                    if (sny < Width - nFlag)
                    {
                        sny += (short)nFlag;
                    }
                    break;
                case Direction.Left:
                    if (snx > nFlag - 1)
                    {
                        snx -= (short)nFlag;
                    }
                    break;
                case Direction.Right:
                    if (snx < Width - nFlag)
                    {
                        snx += (short)nFlag;
                    }
                    break;
                case Direction.UpLeft:
                    if (snx > nFlag - 1 && sny > nFlag - 1)
                    {
                        snx -= (short)nFlag;
                        sny -= (short)nFlag;
                    }
                    break;
                case Direction.UpRight:
                    if (snx > nFlag - 1 && sny < Height - nFlag)
                    {
                        snx += (short)nFlag;
                        sny -= (short)nFlag;
                    }
                    break;
                case Direction.DownLeft:
                    if (snx < Width - nFlag && sny > nFlag - 1)
                    {
                        snx -= (short)nFlag;
                        sny += (short)nFlag;
                    }
                    break;
                case Direction.DownRight:
                    if (snx < Width - nFlag && sny < Height - nFlag)
                    {
                        snx += (short)nFlag;
                        sny += (short)nFlag;
                    }
                    break;
            }
            return (snx == sx && sny == sy) ? false : true;
        }

        public bool CanSafeWalk(int nX, int nY)
        {
            bool result = true;
            ref MapCellInfo cellInfo = ref GetCellInfo(nX, nY, out var cellSuccess);
            if (cellSuccess)
            {
                if (cellInfo.ObjList == null)
                {
                    return true;
                }
                for (int i = 0; i < cellInfo.ObjList.Count; i++)
                {
                    CellObject cellObject = cellInfo.ObjList[i];
                    if (cellObject.CellType == CellType.Event)
                    {
                        MapEvent owinEvent = M2Share.CellObjectMgr.Get<MapEvent>(cellObject.CellObjId);
                        if (owinEvent?.Damage > 0)
                        {
                            result = false;
                        }
                    }
                }
            }
            return result;
        }

        public bool ArroundDoorOpened(int nX, int nY)
        {
            bool result = true;
            for (int i = 0; i < DoorList.Count; i++)
            {
                MapDoor door = DoorList[i];
                if (Math.Abs(door.nX - nX) <= 1 && Math.Abs(door.nY - nY) <= 1)
                {
                    if (!door.Status.Opened)
                    {
                        result = false;
                        break;
                    }
                }
            }
            return result;
        }

        public IActor GetMovingObject(short nX, short nY, bool boFlag)
        {
            IActor result = null;
            ref MapCellInfo cellInfo = ref GetCellInfo(nX, nY, out bool cellSuccess);
            if (cellSuccess && cellInfo.IsAvailable)
            {
                for (int i = 0; i < cellInfo.ObjList.Count; i++)
                {
                    CellObject cellObject = cellInfo.ObjList[i];
                    if (cellObject.CellObjId > 0 && cellObject.ActorObject)
                    {
                        IActor baseObject = M2Share.ActorMgr.Get(cellObject.CellObjId);
                        if (baseObject != null && !baseObject.Ghost)
                        {
                            if (baseObject.CellType == CellType.CastleDoor)
                            {
                                if (((CastleDoor)baseObject).HoldPlace && (!boFlag || !baseObject.Death))
                                {
                                    result = baseObject;
                                    break;
                                }
                            }
                            else
                            {
                                if ((!boFlag || !baseObject.Death))
                                {
                                    result = baseObject;
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }

        public bool GetDoor(int nX, int nY, ref MapDoor door)
        {
            for (int i = 0; i < DoorList.Count; i++)
            {
                if (DoorList[i].nX == nX && DoorList[i].nY == nY)
                {
                    door = DoorList[i];
                    return true;
                }
            }
            return false;
        }

        public bool IsValidObject(int nX, int nY, int nRage, IActor baseObject)
        {
            for (int nXx = nX - nRage; nXx <= nX + nRage; nXx++)
            {
                for (int nYy = nY - nRage; nYy <= nY + nRage; nYy++)
                {
                    ref MapCellInfo cellInfo = ref GetCellInfo(nX, nY, out var cellSuccess);
                    if (cellSuccess && cellInfo.IsAvailable)
                    {
                        for (int i = 0; i < cellInfo.ObjList.Count; i++)
                        {
                            CellObject cellObject = cellInfo.ObjList[i];
                            if (cellObject.CellObjId == baseObject.ActorId)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        public int GetRangeBaseObject(int nX, int nY, int nRage, bool boFlag, IList<IActor> baseObjectList)
        {
            for (int nXx = nX - nRage; nXx <= nX + nRage; nXx++)
            {
                for (int nYy = nY - nRage; nYy <= nY + nRage; nYy++)
                {
                    GetBaseObjects(nXx, nYy, boFlag, ref baseObjectList);
                }
            }
            return baseObjectList.Count;
        }

        public static bool GetMapBaseObjects(short nX, short nY, int nRage, IList<BaseObject> baseObjectList, CellType btType = CellType.Monster)
        {
            if (baseObjectList.Count == 0)
            {
                return false;
            }
            int nStartX = nX - nRage;
            int nEndX = nX + nRage;
            int nStartY = nY - nRage;
            int nEndY = nY + nRage;
            M2Share.Logger.Error("todo GetMapBaseObjects");
            return true;
        }

        /// <summary>
        /// 获取指定坐标上的所有游戏对象
        /// </summary>
        /// <param name="nX"></param>
        /// <param name="nY"></param>
        /// <param name="boFlag">是否包括死亡对象 FALSE 包括死亡对象 TRUE  不包括死亡对象</param>
        /// <param name="baseObjectList"></param>
        /// <returns></returns>
        public void GetBaseObjects(int nX, int nY, bool boFlag, ref IList<IActor> baseObjectList)
        {
            ref MapCellInfo cellInfo = ref GetCellInfo(nX, nY, out var cellSuccess);
            if (cellSuccess && cellInfo.IsAvailable)
            {
                for (int i = 0; i < cellInfo.ObjList.Count; i++)
                {
                    CellObject cellObject = cellInfo.ObjList[i];
                    if (cellObject.CellObjId > 0 && cellObject.ActorObject)
                    {
                        IActor baseObject = M2Share.ActorMgr.Get(cellObject.CellObjId);
                        if (baseObject != null)
                        {
                            if (baseObject.CellType == CellType.CastleDoor)
                            {
                                if (!baseObject.Ghost && ((CastleDoor)baseObject).HoldPlace && !boFlag || !baseObject.Death)
                                {
                                    baseObjectList.Add(baseObject);
                                }
                            }
                            else
                            {
                                if (!baseObject.Ghost && !boFlag || !baseObject.Death)
                                {
                                    baseObjectList.Add(baseObject);
                                }
                            }
                        }
                    }
                }
            }
        }

        public MapEvent GetEvent(int nX, int nY)
        {
            ChFlag = false;
            ref MapCellInfo cellInfo = ref GetCellInfo(nX, nY, out var cellSuccess);
            if (cellSuccess && cellInfo.IsAvailable)
            {
                for (int i = 0; i < cellInfo.ObjList.Count; i++)
                {
                    CellObject cellObject = cellInfo.ObjList[i];
                    if (cellObject.CellType == CellType.Event)
                    {
                        return M2Share.CellObjectMgr.Get<MapEvent>(cellObject.CellObjId); ;
                    }
                }
            }
            return null;
        }

        public void SetMapXyFlag(int nX, int nY, bool boFlag)
        {
            ref MapCellInfo cellInfo = ref GetCellInfo(nX, nY, out var cellSuccess);
            if (cellSuccess)
            {
                cellInfo.SetAttribute(boFlag ? CellAttribute.Walk : CellAttribute.LowWall);
            }
        }

        public bool CanFly(int nsX, int nsY, int ndX, int ndY)
        {
            bool result = true;
            double r28 = (ndX - nsX) / 1.0e1;
            double r30 = (ndY - ndX) / 1.0e1;
            int tryCount = 0;
            while (true)
            {
                int flyX = HUtil32.Round(nsX + r28);
                int flyY = HUtil32.Round(nsY + r30);
                if (!CanWalk(flyX, flyY, true))
                {
                    result = false;
                    break;
                }
                tryCount++;
                if (tryCount >= 10)
                {
                    break;
                }
            }
            return result;
        }

        /// <summary>
        /// 获取指定坐标上的玩家
        /// </summary>
        /// <returns></returns>
        public bool GetXyHuman(int nMapX, int nMapY)
        {
            bool result = false;
            ref MapCellInfo cellInfo = ref GetCellInfo(nMapX, nMapY, out var cellSuccess);
            if (cellSuccess && cellInfo.IsAvailable)
            {
                for (int i = 0; i < cellInfo.ObjList.Count; i++)
                {
                    CellObject cellObject = cellInfo.ObjList[i];
                    if (cellObject.CellObjId > 0 && cellObject.ActorObject)
                    {
                        IActor baseObject = M2Share.ActorMgr.Get(cellObject.CellObjId); ;
                        if (baseObject.Race == ActorRace.Play)
                        {
                            result = true;
                            break;
                        }
                    }
                }
            }
            return result;
        }

        public bool IsValidCell(int nX, int nY)
        {
            ref MapCellInfo cellInfo = ref GetCellInfo(nX, nY, out var cellSuccess);
            if (cellSuccess && cellInfo.Attribute == CellAttribute.LowWall)
            {
                return false;
            }
            return true;
        }

        public string GetEnvirInfo()
        {
            StringBuilder messgae = new StringBuilder();
            messgae.AppendFormat("Map:{0}({1}) DAY:{2} DARK:{3} SAFE:{4} FIGHT:{5} FIGHT3:{6} QUIZ:{7} NORECONNECT:{8}({9}) MUSIC:{10}({11}) EXPRATE:{12}({13}) PKWINLEVEL:{14}({15}) PKLOSTLEVEL:{16}({17}) PKWINEXP:{18}({19}) ",
                MapName, MapDesc, HUtil32.BoolToStr(Flag.DayLight), HUtil32.BoolToStr(Flag.boDarkness), HUtil32.BoolToStr(Flag.SafeArea), HUtil32.BoolToStr(Flag.FightZone),
                HUtil32.BoolToStr(Flag.Fight3Zone), HUtil32.BoolToStr(Flag.boQUIZ), HUtil32.BoolToStr(Flag.boNORECONNECT), Flag.sNoReConnectMap, HUtil32.BoolToStr(Flag.Music), Flag.MusicId, HUtil32.BoolToStr(Flag.boEXPRATE),
                Flag.ExpRate / 100, HUtil32.BoolToStr(Flag.boPKWINLEVEL), Flag.nPKWINLEVEL, HUtil32.BoolToStr(Flag.boPKLOSTLEVEL), Flag.nPKLOSTLEVEL, HUtil32.BoolToStr(Flag.boPKWINEXP), Flag.nPKWINEXP);
            messgae.AppendFormat("PKLOSTEXP:{0}({1}) DECHP:{2}({3}/{4}) INCHP:{5}({6}/{7}) DECGAMEGOLD:{8}({9}/{10}) INCGAMEGOLD:{11}({12}/{13}) INCGAMEPOINT:{14}({15}/{16}) RUNHUMAN:{17} RUNMON:{18} NEEDHOLE:{19} NORECALL:{20} ",
                HUtil32.BoolToStr(Flag.boPKLOSTEXP), Flag.nPKLOSTEXP, HUtil32.BoolToStr(Flag.boDECHP), Flag.nDECHPTIME, Flag.nDECHPPOINT, HUtil32.BoolToStr(Flag.boINCHP), Flag.nINCHPTIME, Flag.nINCHPPOINT, HUtil32.BoolToStr(Flag.boDECGAMEGOLD),
                Flag.nDECGAMEGOLDTIME, Flag.nDECGAMEGOLD, HUtil32.BoolToStr(Flag.boINCGAMEGOLD), Flag.nINCGAMEGOLDTIME, Flag.nINCGAMEGOLD, HUtil32.BoolToStr(Flag.boINCGAMEPOINT), Flag.nINCGAMEPOINTTIME, Flag.nINCGAMEPOINT,
                HUtil32.BoolToStr(Flag.RunHuman), HUtil32.BoolToStr(Flag.RunMon), HUtil32.BoolToStr(Flag.boNEEDHOLE), HUtil32.BoolToStr(Flag.NoReCall));
            messgae.AppendFormat("NOGUILDRECALL:{0} NODEARRECALL:{1} NOMASTERRECALL:{2} NODRUG:{3} MINE:{4} MINE2:{5} NODROPITEM:{6} NOTHROWITEM:{7} NOPOSITIONMOVE:{8} NOHORSE:{9} NOHUMNOMON:{10} NOCHAT:{11}",
                HUtil32.BoolToStr(Flag.NoGuildReCall), HUtil32.BoolToStr(Flag.boNODEARRECALL), HUtil32.BoolToStr(Flag.MasterReCall),
                HUtil32.BoolToStr(Flag.boNODRUG), HUtil32.BoolToStr(Flag.Mine), HUtil32.BoolToStr(Flag.boMINE2), HUtil32.BoolToStr(Flag.NoDropItem), HUtil32.BoolToStr(Flag.NoThrowItem), HUtil32.BoolToStr(Flag.boNOPOSITIONMOVE),
                HUtil32.BoolToStr(Flag.NoHorse), HUtil32.BoolToStr(Flag.boNOHUMNOMON), HUtil32.BoolToStr(Flag.boNOCHAT));
            return messgae.ToString();
        }

        public void AddObject(IActor baseObject)
        {
            switch (baseObject.Race)
            {
                case ActorRace.Play:
                    humCount++;
                    break;
                case >= ActorRace.Animal:
                    monCount++;
                    break;
            }
        }

        public void DelObjectCount(IActor baseObject)
        {
            switch (baseObject.Race)
            {
                case ActorRace.Play:
                    humCount--;
                    break;
                case >= ActorRace.Animal:
                    monCount--;
                    break;
            }
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _cellArray = null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private record struct MapUnitInfo
        {
            public ushort wBkImg;
            public ushort wMidImg;
            public ushort wFrImg;
            public byte btDoorIndex;
            public byte btDoorOffset;
            public byte btAniFrame;
            public byte btAniTick;
            public byte btArea;
            public byte btLight;
        }
    }
}