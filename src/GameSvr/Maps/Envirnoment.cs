using Collections.Pooled;
using GameSvr.Actor;
using GameSvr.Event;
using GameSvr.Event.Events;
using GameSvr.Monster.Monsters;
using GameSvr.Npc;
using GameSvr.Player;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using SystemModule;
using SystemModule.Common;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.Maps
{
    public class Envirnoment : IDisposable
    {
        /// <summary>
        /// 怪物数量
        /// </summary>
        public int MonCount => _monCount;
        /// <summary>
        /// 玩家数量
        /// </summary>
        public int HumCount => _humCount;
        public short Width;
        public short Height;
        public string MapFileName = string.Empty;
        public string MapName = string.Empty;
        public string MapDesc = string.Empty;
        private MapCellInfo[] _cellArray;
        public int MinMap;
        public byte ServerIndex;
        /// <summary>
        /// 进入本地图所需等级
        /// </summary>
        public readonly int RequestLevel = 0;
        public MapInfoFlag Flag;
        public bool Bo2C;
        /// <summary>
        /// 门
        /// </summary>
        public readonly IList<DoorInfo> DoorList;
        public Merchant QuestNpc = null;
        /// <summary>
        /// 任务
        /// </summary>
        private readonly IList<MapQuestInfo> QuestList;
        private int _monCount;
        private int _humCount;
        public readonly IList<PointInfo> PointList;
        public readonly ConcurrentDictionary<int, ActorEntity> CellMap = new ConcurrentDictionary<int, ActorEntity>();

        public Envirnoment()
        {
            ServerIndex = 0;
            MinMap = 0;
            _monCount = 0;
            _humCount = 0;
            Flag = new MapInfoFlag();
            DoorList = new List<DoorInfo>();
            QuestList = new List<MapQuestInfo>();
            PointList = new List<PointInfo>();
        }

        ~Envirnoment()
        {
            Dispose(false);
        }

        public bool AllowMagics(string magicName)
        {
            return true;
        }

        /// <summary>
        /// 检测地图是否禁用技能
        /// </summary>
        /// <returns></returns>
        public bool AllowMagics(short magicId, int type)
        {
            return true;
        }

        /// <summary>
        /// 添加对象到地图
        /// </summary>
        /// <returns></returns>
        public object AddToMap(int nX, int nY, CellType cellType, ActorEntity mapObject)
        {
            object result = null;
            const string sExceptionMsg = "[Exception] TEnvirnoment::AddToMap";
            try
            {
                var bo1E = false;
                var cellSuccess = false;
                var cellInfo = GetCellInfo(nX, nY, ref cellSuccess);
                if (cellSuccess && cellInfo.Valid)
                {
                    if (cellInfo.ObjList == null)
                    {
                        cellInfo.ObjList = new PooledList<CellObject>();
                    }
                    else
                    {
                        if (cellType == CellType.Item)
                        {
                            if (string.Compare(((MapItem)mapObject).Name, Grobal2.sSTRING_GOLDNAME, StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                for (var i = 0; i < cellInfo.Count; i++)
                                {
                                    var cellObject = cellInfo.ObjList[i];
                                    if (cellObject.CellType == CellType.Item)
                                    {
                                        var mapItem = (MapItem)M2Share.CellObjectMgr.Get(cellObject.CellObjId);
                                        if (mapItem.Name == Grobal2.sSTRING_GOLDNAME)
                                        {
                                            var nGoldCount = mapItem.Count + ((MapItem)mapObject).Count;
                                            if (nGoldCount <= 2000)
                                            {
                                                mapItem.Count = nGoldCount;
                                                mapItem.Looks = M2Share.GetGoldShape(nGoldCount);
                                                mapItem.AniCount = 0;
                                                mapItem.Reserved = 0;
                                                cellObject.AddTime = HUtil32.GetTickCount();
                                                result = mapItem;
                                                bo1E = true;
                                            }
                                        }
                                    }
                                }
                            }
                            if (!bo1E && cellInfo.Count >= 5)
                            {
                                result = null;
                                bo1E = true;
                            }
                        }
                    }
                    if (!bo1E)
                    {
                        var cellObject = new CellObject
                        {
                            CellType = cellType,
                            CellObjId = mapObject.ActorId,
                            AddTime = HUtil32.GetTickCount()
                        };
                        if (cellObject.CellObjId == 0)
                        {
                            return null;
                        }
                        if (cellType is CellType.Play or CellType.Monster or CellType.Merchant)
                        {
                            var baseObject = (BaseObject)mapObject;
                            if (!baseObject.AddToMaped)
                            {
                                baseObject.DelFormMaped = false;
                                baseObject.AddToMaped = true;
                                AddObject(baseObject);
                            }
                            cellObject.ActorObject = true;
                        }
                        cellInfo.Add(cellObject, mapObject);
                        result = mapObject;
                    }
                }
            }
            catch (Exception ex)
            {
                M2Share.Log.Error(sExceptionMsg);
                M2Share.Log.Error(ex);
            }
            return result;
        }

        public void AddDoorToMap()
        {
            for (var i = 0; i < DoorList.Count; i++)
            {
                var door = DoorList[i];
                AddToMap(door.nX, door.nY, CellType.Door, door);
            }
        }

        private bool GetCellInfo(int nX, int nY, ref MapCellInfo cellInfo)
        {
            bool result;
            if (nX >= 0 && nX < Width && nY >= 0 && nY < Height)
            {
                cellInfo = _cellArray[nX * Height + nY];
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }

        public MapCellInfo GetCellInfo(int nX, int nY, ref bool success)
        {
            if (nX >= 0 && nX < Width && nY >= 0 && nY < Height)
            {
                MapCellInfo cellInfo = _cellArray[nX * Height + nY];
                if (cellInfo == null)
                {
                    success = false;
                    return null;
                }
                success = true;
                return cellInfo;
            }
            success = false;
            return null;
        }

        public int MoveToMovingObject(int nCx, int nCy, BaseObject cert, int nX, int nY, bool boFlag)
        {
            bool cellSuccess = false;
            bool moveSuccess = true;
            const string sExceptionMsg = "[Exception] TEnvirnoment::MoveToMovingObject";
            var result = 0;
            try
            {
                var cellInfo = GetCellInfo(nX, nY, ref cellSuccess);
                if (!boFlag && cellSuccess)
                {
                    if (cellInfo.Valid)
                    {
                        if (cellInfo.IsAvailable)
                        {
                            for (var i = 0; i < cellInfo.Count; i++)
                            {
                                var cellObject = cellInfo.ObjList[i];
                                if (cellObject.ActorObject)
                                {
                                    var baseObject = M2Share.ActorMgr.Get(cellObject.CellObjId);
                                    if (baseObject != null)
                                    {
                                        if (baseObject.MapCell == CellType.CastleDoor)
                                        {
                                            if (!baseObject.Ghost && ((CastleDoor)baseObject).HoldPlace && !baseObject.Death && !baseObject.FixedHideMode && !baseObject.ObMode)
                                            {
                                                moveSuccess = false;
                                                break;
                                            }
                                        }
                                        else
                                        {
                                            if (!baseObject.Ghost && !baseObject.Death && !baseObject.FixedHideMode && !baseObject.ObMode)
                                            {
                                                moveSuccess = false;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        result = -1;
                        moveSuccess = false;
                    }
                }
                if (moveSuccess)
                {
                    if (GetCellInfo(nX, nY, ref cellInfo) && cellInfo.Attribute != CellAttribute.Walk)
                    {
                        result = -1;
                    }
                    else
                    {
                        if (GetCellInfo(nCx, nCy, ref cellInfo) && cellInfo.IsAvailable)
                        {
                            var i = 0;
                            while (true)
                            {
                                if (cellInfo.Count <= i)
                                {
                                    break;
                                }
                                var cellObject = cellInfo.ObjList[i];
                                if (cellObject.ActorObject && cellObject.CellObjId == cert.ActorId)
                                {
                                    cellInfo.Remove(cellObject);
                                    if (cellInfo.Count > 0)
                                    {
                                        continue;
                                    }
                                    cellInfo.Dispose();
                                    break;
                                }
                                i++;
                            }
                        }
                        if (GetCellInfo(nX, nY, ref cellInfo))
                        {
                            if (cellInfo.ObjList == null)
                            {
                                cellInfo.ObjList = new PooledList<CellObject>();
                            }
                            var cellObject = new CellObject
                            {
                                CellType = cert.MapCell,
                                CellObjId = cert.ActorId,
                                AddTime = HUtil32.GetTickCount()
                            };
                            switch (cert.MapCell)
                            {
                                case CellType.Play:
                                case CellType.Monster:
                                case CellType.Merchant:
                                    cellObject.ActorObject = true;
                                    break;
                                default:
                                    cellObject.ActorObject = false;
                                    break;
                            }
                            cellInfo.Add(cellObject, cert);
                            result = 1;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                M2Share.Log.Error(sExceptionMsg);
                M2Share.Log.Error(e.StackTrace);
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
        public bool CanWalk(int nX, int nY, bool boFlag)
        {
            var result = false;
            var cellSuccess = false;
            MapCellInfo cellInfo = GetCellInfo(nX, nY, ref cellSuccess);
            if (cellSuccess && cellInfo.Valid)
            {
                if (boFlag)
                {
                    return true;
                }
                result = true;
                if (cellInfo.IsAvailable)
                {
                    for (var i = 0; i < cellInfo.Count; i++)
                    {
                        var cellObject = cellInfo.ObjList[i];
                        if (cellObject.ActorObject)
                        {
                            var baseObject = M2Share.ActorMgr.Get(cellObject.CellObjId);
                            if (baseObject != null)
                            {
                                if (baseObject.MapCell == CellType.CastleDoor)
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
        /// 检查地图指定座标是否可以移动
        /// </summary>
        /// <param name="nX"></param>
        /// <param name="nY"></param>
        /// <param name="boFlag">如果为TRUE 则忽略座标上是否有角色</param>
        /// <param name="boItem"></param>
        /// <returns>返回值 True 为可以移动，False 为不可以移动</returns>
        public bool CanWalkOfItem(int nX, int nY, bool boFlag, bool boItem)
        {
            var cellSuccess = false;
            var result = true;
            MapCellInfo cellInfo = GetCellInfo(nX, nY, ref cellSuccess);
            if (cellSuccess && cellInfo.Valid)
            {
                if (cellInfo.IsAvailable)
                {
                    for (var i = 0; i < cellInfo.Count; i++)
                    {
                        var cellObject = cellInfo.ObjList[i];
                        if (!boFlag && cellObject.ActorObject)
                        {
                            var baseObject = M2Share.ActorMgr.Get(cellObject.CellObjId); ;
                            if (baseObject != null)
                            {
                                if (baseObject.MapCell == CellType.CastleDoor)
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
            var result = false;
            var cellSuccess = false;
            MapCellInfo cellInfo = GetCellInfo(nX, nY, ref cellSuccess);
            if (cellSuccess && cellInfo.Valid)
            {
                result = true;
                if (!boFlag && cellInfo.IsAvailable)
                {
                    for (var i = 0; i < cellInfo.Count; i++)
                    {
                        var cellObject = cellInfo.ObjList[i];
                        if (cellObject.ActorObject)
                        {
                            var baseObject = M2Share.ActorMgr.Get(cellObject.CellObjId);
                            if (baseObject != null)
                            {
                                var castle = M2Share.CastleMgr.InCastleWarArea(baseObject);
                                if (M2Share.Config.boWarDisHumRun && castle != null && castle.UnderWar)
                                {

                                }
                                else
                                {
                                    switch (baseObject.Race)
                                    {
                                        case ActorRace.Play:
                                            {
                                                if (M2Share.Config.boRunHuman || Flag.boRUNHUMAN)
                                                {
                                                    continue;
                                                }
                                                break;
                                            }
                                        case ActorRace.NPC:
                                            {
                                                if (M2Share.Config.boRunNpc)
                                                {
                                                    continue;
                                                }
                                                break;
                                            }
                                        case ActorRace.Guard:
                                        case ActorRace.ArcherGuard:
                                            {
                                                if (M2Share.Config.boRunGuard)
                                                {
                                                    continue;
                                                }
                                                break;
                                            }
                                        default:
                                            {
                                                if (M2Share.Config.boRunMon || Flag.boRUNMON)
                                                {
                                                    continue;
                                                }
                                                break;
                                            }
                                    }
                                }
                                if (baseObject.MapCell == CellType.CastleDoor)
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
        public int DeleteFromMap(int nX, int nY, CellType cellType, ActorEntity pRemoveObject)
        {
            const string sExceptionMsg1 = "[Exception] TEnvirnoment::DeleteFromMap -> Except {0}";
            var result = -1;
            var cellSuccess = false;
            MapCellInfo cellInfo = GetCellInfo(nX, nY, ref cellSuccess);
            if (cellSuccess && cellInfo.IsAvailable)
            {
                try
                {
                    for (var i = 0; i < cellInfo.Count; i++)
                    {
                        var cellObject = cellInfo.ObjList[i];
                        if (cellObject != null)
                        {
                            if (cellObject.CellType == cellType && cellObject.CellObjId == pRemoveObject.ActorId)
                            {
                                cellInfo.Remove(cellObject);
                                result = 1;
                                if (cellObject.ActorObject && !((BaseObject)pRemoveObject).DelFormMaped)
                                {
                                    ((BaseObject)pRemoveObject).DelFormMaped = true;
                                    ((BaseObject)pRemoveObject).AddToMaped = false;
                                    DelObjectCount(((BaseObject)pRemoveObject));// 减地图人物怪物计数
                                }
                                if (cellInfo.Count > 0)
                                {
                                    continue;
                                }
                                cellInfo.Dispose();
                                break;
                            }
                        }
                        else
                        {
                            cellInfo.ObjList.RemoveAt(i);
                            if (cellInfo.Count > 0)
                            {
                                continue;
                            }
                            cellInfo.Dispose();
                            break;
                        }
                    }
                }
                catch
                {
                    M2Share.Log.Error(string.Format(sExceptionMsg1, cellType));
                }
            }
            else
            {
                result = 0;
            }
            return result;
        }

        public MapItem GetItem(int nX, int nY)
        {
            Bo2C = false;
            var cellSuccess = false;
            MapCellInfo cellInfo = GetCellInfo(nX, nY, ref cellSuccess);
            if (cellSuccess && cellInfo.Valid)
            {
                Bo2C = true;
                if (cellInfo.IsAvailable)
                {
                    for (var i = 0; i < cellInfo.Count; i++)
                    {
                        var cellObject = cellInfo.ObjList[i];
                        switch (cellObject.CellType)
                        {
                            case CellType.Item:
                                return (MapItem)M2Share.CellObjectMgr.Get(cellObject.CellObjId);
                            case CellType.Route:
                                Bo2C = false;
                                break;
                            case CellType.Play:
                            case CellType.Monster:
                            case CellType.Merchant:
                                var baseObject = M2Share.ActorMgr.Get(cellObject.CellObjId); ;
                                if (!baseObject.Death)
                                {
                                    Bo2C = false;
                                }
                                break;
                        }
                    }
                }
            }
            return null;
        }

        public bool IsCheapStuff()
        {
            return QuestList.Any();
        }

        public bool AddToMapItemEvent(int nX, int nY, CellType nType, StoneMineEvent stoneMineEvent)
        {
            var cellSuccess = false;
            MapCellInfo cellInfo = GetCellInfo(nX, nY, ref cellSuccess);
            if (cellSuccess && cellInfo.Valid)
            {
                if (cellInfo.ObjList == null)
                {
                    cellInfo.ObjList = new PooledList<CellObject>();
                }
                if (nType == CellType.Event)
                {
                    var cellObject = new CellObject();
                    cellObject.CellType = nType;
                    cellObject.CellObjId = stoneMineEvent.Id;
                    cellObject.AddTime = HUtil32.GetTickCount();
                    cellInfo.Add(cellObject, stoneMineEvent);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 添加矿石到地图上
        /// </summary>
        /// <returns></returns>
        public object AddToMapMineEvent(int nX, int nY, CellType nType, StoneMineEvent stoneMineEvent)
        {
            MapCellInfo mc = null;
            const string sExceptionMsg = "[Exception] TEnvirnoment::AddToMapMineEvent ";
            var cellSuccess = false;
            try
            {
                MapCellInfo cellInfo = GetCellInfo(nX, nY, ref cellSuccess);
                var bo1A = false;
                if (cellSuccess && cellInfo.Attribute != CellAttribute.Walk)
                {
                    var isSpace = false;// 人物可以走到的地方才放上矿藏
                    for (var x = nX - 1; x <= nX + 1; x++)
                    {
                        for (var y = nY - 1; y <= nY + 1; y++)
                        {
                            if (GetCellInfo(x, y, ref mc))
                            {
                                if (mc.Valid)
                                {
                                    isSpace = true;
                                }
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
                        if (cellInfo.ObjList == null)
                        {
                            cellInfo.ObjList = new PooledList<CellObject>();
                        }
                        if (!bo1A)
                        {
                            var cellObject = new CellObject
                            {
                                CellType = nType,
                                CellObjId = stoneMineEvent.Id,
                                AddTime = HUtil32.GetTickCount()
                            };
                            cellInfo.Add(cellObject, stoneMineEvent);
                            return stoneMineEvent;
                        }
                    }
                }
            }
            catch
            {
                M2Share.Log.Error(sExceptionMsg);
            }
            return null;
        }

        /// <summary>
        /// 刷新在地图上位置的时间
        /// </summary>
        public void VerifyMapTime(int nX, int nY, BaseObject baseObject)
        {
            bool boVerify = false;
            var cellSuccess = false;
            const string sExceptionMsg = "[Exception] TEnvirnoment::VerifyMapTime";
            try
            {
                MapCellInfo cellInfo = GetCellInfo(nX, nY, ref cellSuccess);
                if (cellSuccess && cellInfo.IsAvailable)
                {
                    for (var i = 0; i < cellInfo.Count; i++)
                    {
                        CellObject cellObject = cellInfo.ObjList[i];
                        if (cellObject.ActorObject && cellObject.CellObjId == baseObject.ActorId)
                        {
                            cellObject.AddTime = HUtil32.GetTickCount();
                            boVerify = true;
                            break;
                        }
                    }
                }
                if (!boVerify)
                {
                    AddToMap(nX, nY, baseObject.MapCell, baseObject);
                }
            }
            catch
            {
                M2Share.Log.Error(sExceptionMsg);
            }
        }

        public bool LoadMapData(string sMapFile)
        {
            var result = false;
            int n24;
            int point;
            DoorInfo door;
            try
            {
                if (File.Exists(sMapFile))
                {
                    using var fileStream = new FileStream(sMapFile, FileMode.Open, FileAccess.Read);
                    if (fileStream.Length <= 52)
                    {
                        fileStream.Close();
                        fileStream.Dispose();
                        return false;
                    }
                    using var binReader = new BinaryReader(fileStream);
                    Width = binReader.ReadInt16();
                    Height = binReader.ReadInt16();
                    Initialize(Width, Height);
                    fileStream.Position = 52;

                    MapUnitInfo mapUnitInfo = new MapUnitInfo();
                    if (Flag.boMINE || Flag.boMINE2)
                    {
                        for (var nW = 0; nW < Width; nW++)
                        {
                            n24 = nW * Height;
                            for (var nH = 0; nH < Height; nH++)
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
                                    _cellArray[n24 + nH] = new MapCellInfo() { Attribute = CellAttribute.HighWall };
                                }
                                if ((mapUnitInfo.wFrImg & 0x8000) != 0)// wFrImg High
                                {
                                    _cellArray[n24 + nH] = new MapCellInfo() { Attribute = CellAttribute.LowWall };
                                }
                                if (_cellArray[n24 + nH] == null)
                                {
                                    _cellArray[n24 + nH] = new MapCellInfo()
                                    {
                                        ObjList = new PooledList<CellObject>(),
                                        Attribute = CellAttribute.Walk
                                    };
                                }
                                if ((mapUnitInfo.btDoorIndex & 0x80) != 0)
                                {
                                    point = mapUnitInfo.btDoorIndex & 0x7F;
                                    if (point > 0)
                                    {
                                        door = new DoorInfo
                                        {
                                            nX = nW,
                                            nY = nH,
                                            n08 = point,
                                            Status = null
                                        };
                                        for (var i = 0; i < DoorList.Count; i++)
                                        {
                                            if (Math.Abs(DoorList[i].nX - door.nX) <= 10)
                                            {
                                                if (Math.Abs(DoorList[i].nY - door.nY) <= 10)
                                                {
                                                    if (DoorList[i].n08 == point)
                                                    {
                                                        door.Status = DoorList[i].Status;
                                                        door.Status.nRefCount++;
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
                                                bo01 = false,
                                                n04 = 0,
                                                OpenTick = 0,
                                                nRefCount = 1
                                            };
                                        }
                                        DoorList.Add(door);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        for (var nW = 0; nW < Width; nW++)
                        {
                            n24 = nW * Height;
                            for (var nH = 0; nH < Height; nH++)
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
                                    _cellArray[n24 + nH] = MapCellInfo.HighWall;
                                }
                                if ((mapUnitInfo.wFrImg & 0x8000) != 0)// wFrImg High
                                {
                                    _cellArray[n24 + nH] = MapCellInfo.LowWall;
                                }
                                if (_cellArray[n24 + nH] == null)
                                {
                                    _cellArray[n24 + nH] = new MapCellInfo()
                                    {
                                        ObjList = new PooledList<CellObject>(),
                                        Attribute = CellAttribute.Walk
                                    };
                                }
                                if ((mapUnitInfo.btDoorIndex & 0x80) != 0)
                                {
                                    point = mapUnitInfo.btDoorIndex & 0x7F;
                                    if (point > 0)
                                    {
                                        door = new DoorInfo
                                        {
                                            nX = nW,
                                            nY = nH,
                                            n08 = point,
                                            Status = null
                                        };
                                        for (var i = 0; i < DoorList.Count; i++)
                                        {
                                            if (Math.Abs(DoorList[i].nX - door.nX) <= 10)
                                            {
                                                if (Math.Abs(DoorList[i].nY - door.nY) <= 10)
                                                {
                                                    if (DoorList[i].n08 == point)
                                                    {
                                                        door.Status = DoorList[i].Status;
                                                        door.Status.nRefCount++;
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
                                                bo01 = false,
                                                n04 = 0,
                                                OpenTick = 0,
                                                nRefCount = 1
                                            };
                                        }
                                        DoorList.Add(door);
                                    }
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

                var pointFileName = Path.Combine(M2Share.BasePath, M2Share.Config.EnvirDir, "Point", $"{sMapFile}.txt");
                if (File.Exists(pointFileName))
                {
                    var loadList = new StringList();
                    loadList.LoadFromFile(pointFileName);
                    string sX = string.Empty;
                    string sY = string.Empty;
                    for (int i = 0; i < loadList.Count; i++)
                    {
                        var line = loadList[i];
                        if (string.IsNullOrEmpty(line) || line.StartsWith(";"))
                        {
                            continue;
                        }
                        line = HUtil32.GetValidStr3(line, ref sX, new[] { ',', '\t' });
                        line = HUtil32.GetValidStr3(line, ref sY, new[] { ',', '\t' });
                        var nX = (short)HUtil32.StrToInt(sX, -1);
                        var nY = (short)HUtil32.StrToInt(sY, -1);
                        if (nX >= 0 && nY >= 0 && nX < Width && nY < Height)
                        {
                            PointList.Add(new PointInfo(nX, nY));
                        }
                    }
                }
            }
            catch (Exception)
            {
                M2Share.Log.Error("[Exception] TEnvirnoment.LoadMapData");
            }
            return result;
        }

        private void Initialize(short nWidth, short nHeight)
        {
            if (nWidth > 1 && nHeight > 1)
            {
                if (_cellArray != null)
                {
                    for (var nW = 0; nW < Width; nW++)
                    {
                        for (var nH = 0; nH < Height; nH++)
                        {
                            if (_cellArray[nW * Height + nH].ObjList != null)
                            {
                                _cellArray[nW * Height + nH].Dispose();
                            }
                        }
                    }
                    _cellArray = null;
                }
                Width = nWidth;
                Height = nHeight;
                _cellArray = new MapCellInfo[nWidth * nHeight];
            }
        }

        public bool CreateQuest(int nFlag, int nValue, string sMonName, string sItem, string sQuest, bool boGrouped)
        {
            if (nFlag < 0)
            {
                return false;
            }
            var mapQuest = new MapQuestInfo
            {
                nFlag = nFlag
            };
            if (nValue > 1)
            {
                nValue = 1;
            }
            mapQuest.nValue = nValue;
            if (sMonName == "*")
            {
                sMonName = "";
            }
            mapQuest.sMonName = sMonName;
            if (sItem == "*")
            {
                sItem = "";
            }
            mapQuest.sItemName = sItem;
            if (sQuest == "*")
            {
                sQuest = "";
            }
            mapQuest.boGrouped = boGrouped;
            var mapMerchant = new Merchant
            {
                MapName = "0",
                CurrX = 0,
                CurrY = 0,
                ChrName = sQuest,
                m_nFlag = 0,
                Appr = 0,
                m_sFilePath = "MapQuest_def",
                m_boIsHide = true,
                m_boIsQuest = false
            };
            M2Share.WorldEngine.QuestNpcList.Add(mapMerchant);
            mapQuest.NPC = mapMerchant;
            QuestList.Add(mapQuest);
            return true;
        }

        public int GetXyObjCount(int nX, int nY)
        {
            var result = 0;
            var cellSuccess = false;
            MapCellInfo cellInfo = GetCellInfo(nX, nY, ref cellSuccess);
            if (cellSuccess && cellInfo.IsAvailable)
            {
                for (var i = 0; i < cellInfo.Count; i++)
                {
                    var cellObject = cellInfo.ObjList[i];
                    if (cellObject == null)
                    {
                        continue;
                    }
                    if (cellObject.ActorObject)
                    {
                        var baseObject = M2Share.ActorMgr.Get(cellObject.CellObjId); ;
                        if (baseObject != null)
                        {
                            if (baseObject.MapCell == CellType.CastleDoor)
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

        public bool GetNextPosition(short sx, short sy, int ndir, int nFlag, ref short snx, ref short sny)
        {
            bool result;
            snx = sx;
            sny = sy;
            switch (ndir)
            {
                case Grobal2.DR_UP:
                    if (sny > nFlag - 1)
                    {
                        sny -= (short)nFlag;
                    }
                    break;
                case Grobal2.DR_DOWN:
                    if (sny < Width - nFlag)
                    {
                        sny += (short)nFlag;
                    }
                    break;
                case Grobal2.DR_LEFT:
                    if (snx > nFlag - 1)
                    {
                        snx -= (short)nFlag;
                    }
                    break;
                case Grobal2.DR_RIGHT:
                    if (snx < Width - nFlag)
                    {
                        snx += (short)nFlag;
                    }
                    break;
                case Grobal2.DR_UPLEFT:
                    if (snx > nFlag - 1 && sny > nFlag - 1)
                    {
                        snx -= (short)nFlag;
                        sny -= (short)nFlag;
                    }
                    break;
                case Grobal2.DR_UPRIGHT:
                    if (snx > nFlag - 1 && sny < Height - nFlag)
                    {
                        snx += (short)nFlag;
                        sny -= (short)nFlag;
                    }
                    break;
                case Grobal2.DR_DOWNLEFT:
                    if (snx < Width - nFlag && sny > nFlag - 1)
                    {
                        snx -= (short)nFlag;
                        sny += (short)nFlag;
                    }
                    break;
                case Grobal2.DR_DOWNRIGHT:
                    if (snx < Width - nFlag && sny < Height - nFlag)
                    {
                        snx += (short)nFlag;
                        sny += (short)nFlag;
                    }
                    break;
            }
            if (snx == sx && sny == sy)
            {
                result = false;
            }
            else
            {
                result = true;
            }
            return result;
        }

        public bool CanSafeWalk(int nX, int nY)
        {
            var result = true;
            var cellSuccess = false;
            MapCellInfo cellInfo = GetCellInfo(nX, nY, ref cellSuccess);
            if (cellSuccess && cellInfo.IsAvailable)
            {
                for (var i = 0; i < cellInfo.Count; i++)
                {
                    var cellObject = cellInfo.ObjList[i];
                    if (cellObject.CellType == CellType.Event)
                    {
                        var owinEvent = (EventInfo)M2Share.CellObjectMgr.Get(cellObject.CellObjId);
                        if (owinEvent.Damage > 0)
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
            var result = true;
            for (var i = 0; i < DoorList.Count; i++)
            {
                var door = DoorList[i];
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

        public object GetMovingObject(short nX, short nY, bool boFlag)
        {
            object result = null;
            var cellSuccess = false;
            MapCellInfo cellInfo = GetCellInfo(nX, nY, ref cellSuccess);
            if (cellSuccess && cellInfo.IsAvailable)
            {
                for (var i = 0; i < cellInfo.Count; i++)
                {
                    var cellObject = cellInfo.ObjList[i];
                    if (cellObject.ActorObject)
                    {
                        var baseObject = M2Share.ActorMgr.Get(cellObject.CellObjId);
                        if (baseObject != null && !baseObject.Ghost)
                        {
                            if (baseObject.MapCell == CellType.CastleDoor)
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

        public Merchant GetQuestNpc(PlayObject baseObject, string sChrName, string itemName, bool boFlag)
        {
            for (var i = 0; i < QuestList.Count; i++)
            {
                var mapQuestFlag = QuestList[i];
                var nFlagValue = baseObject.GetQuestFalgStatus(mapQuestFlag.nFlag);
                if (nFlagValue == mapQuestFlag.nValue)
                {
                    if (boFlag == mapQuestFlag.boGrouped || !boFlag)
                    {
                        var bo1D = false;
                        if (!string.IsNullOrEmpty(mapQuestFlag.sMonName) && !string.IsNullOrEmpty(mapQuestFlag.sItemName))
                        {
                            if (mapQuestFlag.sMonName == sChrName && mapQuestFlag.sItemName == itemName)
                            {
                                bo1D = true;
                            }
                        }
                        if (!string.IsNullOrEmpty(mapQuestFlag.sMonName) && string.IsNullOrEmpty(mapQuestFlag.sItemName))
                        {
                            if (mapQuestFlag.sMonName == sChrName && string.IsNullOrEmpty(itemName))
                            {
                                bo1D = true;
                            }
                        }
                        if (string.IsNullOrEmpty(mapQuestFlag.sMonName) && !string.IsNullOrEmpty(mapQuestFlag.sItemName))
                        {
                            if (mapQuestFlag.sItemName == itemName)
                            {
                                bo1D = true;
                            }
                        }
                        if (bo1D)
                        {
                            return (Merchant)mapQuestFlag.NPC;
                        }
                    }
                }
            }
            return null;
        }

        public int GetItemEx(int nX, int nY, ref int nCount)
        {
            int result = 0;
            nCount = 0;
            Bo2C = false;
            var cellSuccess = false;
            MapCellInfo cellInfo = GetCellInfo(nX, nY, ref cellSuccess);
            if (cellSuccess && cellInfo.Valid)
            {
                Bo2C = true;
                if (cellInfo.IsAvailable)
                {
                    for (var i = 0; i < cellInfo.Count; i++)
                    {
                        var cellObject = cellInfo.ObjList[i];
                        switch (cellObject.CellType)
                        {
                            case CellType.Item:
                                result = cellObject.CellObjId;
                                nCount++;
                                break;
                            case CellType.Route:
                                Bo2C = false;
                                break;
                            case CellType.Monster:
                            case CellType.Play:
                                {
                                    var baseObject = M2Share.ActorMgr.Get(cellObject.CellObjId);
                                    if (!baseObject.Death)
                                    {
                                        Bo2C = false;
                                    }
                                    break;
                                }
                        }
                    }
                }
            }
            return result;
        }

        public DoorInfo GetDoor(int nX, int nY)
        {
            for (var i = 0; i < DoorList.Count; i++)
            {
                var door = DoorList[i];
                if (door.nX == nX && door.nY == nY)
                {
                    return door;
                }
            }
            return null;
        }

        public bool IsValidObject(int nX, int nY, int nRage, BaseObject baseObject)
        {
            for (var nXx = nX - nRage; nXx <= nX + nRage; nXx++)
            {
                for (var nYy = nY - nRage; nYy <= nY + nRage; nYy++)
                {
                    var cellSuccess = false;
                    var cellInfo = GetCellInfo(nX, nY, ref cellSuccess);
                    if (cellSuccess && cellInfo.IsAvailable)
                    {
                        for (var i = 0; i < cellInfo.Count; i++)
                        {
                            var cellObject = cellInfo.ObjList[i];
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

        public int GetRangeBaseObject(int nX, int nY, int nRage, bool boFlag, IList<BaseObject> baseObjectList)
        {
            for (var nXx = nX - nRage; nXx <= nX + nRage; nXx++)
            {
                for (var nYy = nY - nRage; nYy <= nY + nRage; nYy++)
                {
                    GetBaseObjects(nXx, nYy, boFlag, baseObjectList);
                }
            }
            return baseObjectList.Count;
        }

        public bool GetMapBaseObjects(short nX, short nY, int nRage, IList<BaseObject> baseObjectList, CellType btType = CellType.Monster)
        {
            if (baseObjectList.Count == 0)
            {
                return false;
            }
            var nStartX = nX - nRage;
            var nEndX = nX + nRage;
            var nStartY = nY - nRage;
            var nEndY = nY + nRage;
            M2Share.Log.Error("todo GetMapBaseObjects");
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
        public int GetBaseObjects(int nX, int nY, bool boFlag, IList<BaseObject> baseObjectList)
        {
            var cellSuccess = false;
            MapCellInfo cellInfo = GetCellInfo(nX, nY, ref cellSuccess);
            if (cellSuccess && cellInfo.IsAvailable)
            {
                for (var i = 0; i < cellInfo.Count; i++)
                {
                    var cellObject = cellInfo.ObjList[i];
                    if (cellObject.ActorObject)
                    {
                        var baseObject = M2Share.ActorMgr.Get(cellObject.CellObjId); ;
                        if (baseObject != null)
                        {
                            if (baseObject.MapCell == CellType.CastleDoor)
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
            return baseObjectList.Count;
        }

        public EventInfo GetEvent(int nX, int nY)
        {
            EventInfo result = null;
            Bo2C = false;
            var cellSuccess = false;
            MapCellInfo cellInfo = GetCellInfo(nX, nY, ref cellSuccess);
            if (cellSuccess && cellInfo.IsAvailable)
            {
                for (var i = 0; i < cellInfo.Count; i++)
                {
                    var cellObject = cellInfo.ObjList[i];
                    if (cellObject.CellType == CellType.Event)
                    {
                        result = (EventInfo)M2Share.CellObjectMgr.Get(cellObject.CellObjId); ;
                    }
                }
            }
            return result;
        }

        public void SetMapXyFlag(int nX, int nY, bool boFlag)
        {
            var cellSuccess = false;
            MapCellInfo cellInfo = GetCellInfo(nX, nY, ref cellSuccess);
            if (cellSuccess)
            {
                if (boFlag)
                {
                    cellInfo.Attribute = CellAttribute.Walk;
                }
                else
                {
                    cellInfo.Attribute = CellAttribute.LowWall;
                }
            }
        }

        public bool CanFly(int nsX, int nsY, int ndX, int ndY)
        {
            int n18;
            int n1C;
            var result = true;
            var r28 = (ndX - nsX) / 1.0e1;
            var r30 = (ndY - ndX) / 1.0e1;
            var n14 = 0;
            while (true)
            {
                n18 = HUtil32.Round(nsX + r28);
                n1C = HUtil32.Round(nsY + r30);
                if (!CanWalk(n18, n1C, true))
                {
                    result = false;
                    break;
                }
                n14++;
                if (n14 >= 10)
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
            var cellSuccess = false;
            var result = false;
            MapCellInfo cellInfo = GetCellInfo(nMapX, nMapY, ref cellSuccess);
            if (cellSuccess && cellInfo.IsAvailable)
            {
                for (var i = 0; i < cellInfo.Count; i++)
                {
                    var cellObject = cellInfo.ObjList[i];
                    if (cellObject.ActorObject)
                    {
                        var baseObject = M2Share.ActorMgr.Get(cellObject.CellObjId); ;
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
            var cellSuccess = false;
            MapCellInfo cellInfo = GetCellInfo(nX, nY, ref cellSuccess);
            if (cellSuccess && cellInfo.Attribute == CellAttribute.LowWall)
            {
                return false;
            }
            return true;
        }

        public string GetEnvirInfo()
        {
            var messgae = new StringBuilder();
            messgae.AppendFormat("Map:{0}({1}) DAY:{2} DARK:{3} SAFE:{4} FIGHT:{5} FIGHT3:{6} QUIZ:{7} NORECONNECT:{8}({9}) MUSIC:{10}({11}) EXPRATE:{12}({13}) PKWINLEVEL:{14}({15}) PKLOSTLEVEL:{16}({17}) PKWINEXP:{18}({19}) ",
                MapName, MapDesc, HUtil32.BoolToStr(Flag.boDayLight), HUtil32.BoolToStr(Flag.boDarkness), HUtil32.BoolToStr(Flag.boSAFE), HUtil32.BoolToStr(Flag.boFightZone),
                HUtil32.BoolToStr(Flag.boFight3Zone), HUtil32.BoolToStr(Flag.boQUIZ), HUtil32.BoolToStr(Flag.boNORECONNECT), Flag.sNoReConnectMap, HUtil32.BoolToStr(Flag.boMUSIC), Flag.nMUSICID, HUtil32.BoolToStr(Flag.boEXPRATE),
                Flag.nEXPRATE / 100, HUtil32.BoolToStr(Flag.boPKWINLEVEL), Flag.nPKWINLEVEL, HUtil32.BoolToStr(Flag.boPKLOSTLEVEL), Flag.nPKLOSTLEVEL, HUtil32.BoolToStr(Flag.boPKWINEXP), Flag.nPKWINEXP);
            messgae.AppendFormat("PKLOSTEXP:{0}({1}) DECHP:{2}({3}/{4}) INCHP:{5}({6}/{7}) DECGAMEGOLD:{8}({9}/{10}) INCGAMEGOLD:{11}({12}/{13}) INCGAMEPOINT:{14}({15}/{16}) RUNHUMAN:{17} RUNMON:{18} NEEDHOLE:{19} NORECALL:{20} ",
                HUtil32.BoolToStr(Flag.boPKLOSTEXP), Flag.nPKLOSTEXP, HUtil32.BoolToStr(Flag.boDECHP), Flag.nDECHPTIME, Flag.nDECHPPOINT, HUtil32.BoolToStr(Flag.boINCHP), Flag.nINCHPTIME, Flag.nINCHPPOINT, HUtil32.BoolToStr(Flag.boDECGAMEGOLD),
                Flag.nDECGAMEGOLDTIME, Flag.nDECGAMEGOLD, HUtil32.BoolToStr(Flag.boINCGAMEGOLD), Flag.nINCGAMEGOLDTIME, Flag.nINCGAMEGOLD, HUtil32.BoolToStr(Flag.boINCGAMEPOINT), Flag.nINCGAMEPOINTTIME, Flag.nINCGAMEPOINT,
                HUtil32.BoolToStr(Flag.boRUNHUMAN), HUtil32.BoolToStr(Flag.boRUNMON), HUtil32.BoolToStr(Flag.boNEEDHOLE), HUtil32.BoolToStr(Flag.boNORECALL));
            messgae.AppendFormat("NOGUILDRECALL:{0} NODEARRECALL:{1} NOMASTERRECALL:{2} NODRUG:{3} MINE:{4} MINE2:{5} NODROPITEM:{6} NOTHROWITEM:{7} NOPOSITIONMOVE:{8} NOHORSE:{9} NOHUMNOMON:{10} NOCHAT:{11}",
                HUtil32.BoolToStr(Flag.boNOGUILDRECALL), HUtil32.BoolToStr(Flag.boNODEARRECALL), HUtil32.BoolToStr(Flag.boNOMASTERRECALL),
                HUtil32.BoolToStr(Flag.boNODRUG), HUtil32.BoolToStr(Flag.boMINE), HUtil32.BoolToStr(Flag.boMINE2), HUtil32.BoolToStr(Flag.boNODROPITEM), HUtil32.BoolToStr(Flag.boNOTHROWITEM), HUtil32.BoolToStr(Flag.boNOPOSITIONMOVE),
                HUtil32.BoolToStr(Flag.boNOHORSE), HUtil32.BoolToStr(Flag.boNOHUMNOMON), HUtil32.BoolToStr(Flag.boNOCHAT));
            return messgae.ToString();
        }

        public void AddObject(BaseObject baseObject)
        {
            switch (baseObject.Race)
            {
                case ActorRace.Play:
                    _humCount++;
                    break;
                case >= ActorRace.Animal:
                    _monCount++;
                    break;
            }
        }

        public void DelObjectCount(BaseObject baseObject)
        {
            switch (baseObject.Race)
            {
                case ActorRace.Play:
                    _humCount--;
                    break;
                case >= ActorRace.Animal:
                    _monCount--;
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

        private struct MapUnitInfo
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