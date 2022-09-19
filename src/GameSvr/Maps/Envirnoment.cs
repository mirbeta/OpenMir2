using Collections.Pooled;
using GameSvr.Actor;
using GameSvr.Event;
using GameSvr.Event.Events;
using GameSvr.Npc;
using GameSvr.World;
using System.Buffers;
using SystemModule;
using SystemModule.Common;
using SystemModule.Data;

namespace GameSvr.Maps
{
    public class Envirnoment
    {
        /// <summary>
        /// 怪物数量
        /// </summary>
        public int MonCount => _monCount;
        /// <summary>
        /// 玩家数量
        /// </summary>
        public int HumCount => _humCount;

        public int ThreadId;
        public short Width;
        public short Height;
        public string MapFileName = string.Empty;
        public string MapName = string.Empty;
        public string MapDesc = string.Empty;
        private Memory<MapCellInfo> _cellArray;
        private readonly MemoryPool<MapCellInfo> _cellPool;
        public int MinMap;
        public int ServerIndex;
        /// <summary>
        /// 进入本地图所需等级
        /// </summary>
        public readonly int RequestLevel = 0;
        public TMapFlag Flag;
        public bool Bo2C;
        /// <summary>
        /// 门
        /// </summary>
        public readonly IList<TDoorInfo> DoorList;
        public Merchant QuestNpc = null;
        /// <summary>
        /// 任务
        /// </summary>
        private readonly IList<TMapQuestInfo> _questList;
        private int WhisperTick;
        private int _monCount;
        private int _humCount;
        public readonly IList<PointInfo> PointList;

        //multithread vars
        readonly object _locker = new object();
        public MonsterThread[] MobThreads;
        private readonly Thread[] MobThreading;
        public int SpawnMultiplier = 1;//set this to 2 if you want double spawns (warning this can easily lag your server far beyond what you imagine)

        public Envirnoment()
        {
            ServerIndex = 0;
            MinMap = 0;
            Flag = new TMapFlag();
            _monCount = 0;
            _humCount = 0;
            DoorList = new List<TDoorInfo>();
            _questList = new List<TMapQuestInfo>();
            WhisperTick = 0;
            _cellPool = MemoryPool<MapCellInfo>.Shared;
            PointList = new List<PointInfo>();
            ThreadId = M2Share.RandomNumber.Random(M2Share.Config.ProcessMonsterMultiThreadLimit);
            MobThreads = new MonsterThread[M2Share.Config.ProcessMonsterMultiThreadLimit];
            MobThreading = new Thread[M2Share.Config.ProcessMonsterMultiThreadLimit];
        }

        ~Envirnoment()
        {
            _cellArray = null;
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
        public object AddToMap(int nX, int nY, CellType cellType, EntityId mapObject)
        {
            object result = null;
            const string sExceptionMsg = "[Exception] TEnvirnoment::AddToMap";
            try
            {
                var bo1E = false;
                var cellsuccess = false;
                var cellInfo = GetCellInfo(nX, nY, ref cellsuccess);
                if (cellsuccess && cellInfo.Valid)
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
                                    var osObject = cellInfo.ObjList[i];
                                    if (osObject.CellType == CellType.Item)
                                    {
                                        var mapItem = (MapItem)M2Share.CellObjectSystem.Get(osObject.CellObjId);
                                        if (mapItem.Name == Grobal2.sSTRING_GOLDNAME)
                                        {
                                            var nGoldCount = mapItem.Count + ((MapItem)mapObject).Count;
                                            if (nGoldCount <= 2000)
                                            {
                                                mapItem.Count = nGoldCount;
                                                mapItem.Looks = M2Share.GetGoldShape(nGoldCount);
                                                mapItem.AniCount = 0;
                                                mapItem.Reserved = 0;
                                                osObject.AddTime = HUtil32.GetTickCount();
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
                        var osObject = new CellObject
                        {
                            CellType = cellType,
                            CellObjId = mapObject.ActorId,
                            AddTime = HUtil32.GetTickCount()
                        };
                        if (cellType is CellType.Play or CellType.Monster or CellType.Merchant)
                        {
                            var baseObject = (BaseObject)mapObject;
                            if (!baseObject.AddToMaped)
                            {
                                baseObject.DelFormMaped = false;
                                baseObject.AddToMaped = true;
                                osObject.ActorObject = true;
                                AddObject(baseObject);
                            }
                        }
                        cellInfo.Add(osObject, mapObject);
                        result = mapObject;
                    }
                }
            }
            catch
            {
                M2Share.Log.Error(sExceptionMsg);
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
                cellInfo = _cellArray.Span[nX * Height + nY];
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
                MapCellInfo cellInfo = _cellArray.Span[nX * Height + nY];
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
            bool cellsuccess = false;
            BaseObject baseObject;
            CellObject osObject;
            bool moveSuccess;
            const string sExceptionMsg = "[Exception] TEnvirnoment::MoveToMovingObject";
            var result = 0;
            try
            {
                moveSuccess = true;
                var cellInfo = GetCellInfo(nX, nY, ref cellsuccess);
                if (!boFlag && cellsuccess)
                {
                    if (cellInfo.Valid)
                    {
                        if (cellInfo.IsAvailable)
                        {
                            for (var i = 0; i < cellInfo.Count; i++)
                            {
                                var OSObject = cellInfo.ObjList[i];
                                if (OSObject.ActorObject)
                                {
                                    baseObject = M2Share.ActorMgr.Get(OSObject.CellObjId);
                                    if (baseObject != null)
                                    {
                                        if (!baseObject.Ghost && baseObject.Bo2B9 && !baseObject.Death && !baseObject.FixedHideMode && !baseObject.ObMode)
                                        {
                                            moveSuccess = false;
                                            break;
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
                                osObject = cellInfo.ObjList[i];
                                if (osObject.ActorObject)
                                {
                                    if (osObject.CellObjId == cert.ActorId)
                                    {
                                        cellInfo.Remove(osObject);
                                        if (cellInfo.Count > 0)
                                        {
                                            continue;
                                        }
                                        cellInfo.Dispose();
                                        break;
                                    }
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
                            osObject = new CellObject
                            {
                                CellType = cert.MapCell,
                                CellObjId = cert.ActorId,
                                AddTime = HUtil32.GetTickCount()
                            };
                            if (osObject.CellType == CellType.Play || osObject.CellType == CellType.Monster || osObject.CellType == CellType.Merchant)
                            {
                                osObject.ActorObject = true;
                            }
                            cellInfo.Add(osObject, cert);
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
            var cellsuccess = false;
            MapCellInfo cellInfo = GetCellInfo(nX, nY, ref cellsuccess);
            if (cellsuccess && cellInfo.Valid)
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
                        var osObject = cellInfo.ObjList[i];
                        if (osObject.ActorObject)
                        {
                            var baseObject = M2Share.ActorMgr.Get(osObject.CellObjId);
                            if (baseObject != null)
                            {
                                if (!baseObject.Ghost && baseObject.Bo2B9 && !baseObject.Death && !baseObject.FixedHideMode && !baseObject.ObMode)
                                {
                                    result = false;
                                    break;
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
            var cellsuccess = false;
            var result = true;
            MapCellInfo cellInfo = GetCellInfo(nX, nY, ref cellsuccess);
            if (cellsuccess && cellInfo.Valid)
            {
                if (cellInfo.IsAvailable)
                {
                    for (var i = 0; i < cellInfo.Count; i++)
                    {
                        var osObject = cellInfo.ObjList[i];
                        if (!boFlag && osObject.ActorObject)
                        {
                            var baseObject = M2Share.ActorMgr.Get(osObject.CellObjId);;
                            if (baseObject != null)
                            {
                                if (!baseObject.Ghost && baseObject.Bo2B9 && !baseObject.Death && !baseObject.FixedHideMode && !baseObject.ObMode)
                                {
                                    result = false;
                                    break;
                                }
                            }
                        }
                        if (!boItem && osObject.CellType == CellType.Item)
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
            var cellsuccess = false;
            MapCellInfo cellInfo = GetCellInfo(nX, nY, ref cellsuccess);
            if (cellsuccess && cellInfo.Valid)
            {
                result = true;
                if (!boFlag && cellInfo.IsAvailable)
                {
                    for (var i = 0; i < cellInfo.Count; i++)
                    {
                        var osObject = cellInfo.ObjList[i];
                        if (osObject.ActorObject)
                        {
                            var baseObject = M2Share.ActorMgr.Get(osObject.CellObjId);;
                            if (baseObject != null)
                            {
                                var castle = M2Share.CastleMgr.InCastleWarArea(baseObject);
                                if (M2Share.Config.boWarDisHumRun && castle != null && castle.UnderWar)
                                {
                                }
                                else
                                {
                                    if (baseObject.Race == Grobal2.RC_PLAYOBJECT)
                                    {
                                        if (M2Share.Config.boRunHuman || Flag.boRUNHUMAN)
                                        {
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        if (baseObject.Race == Grobal2.RC_NPC)
                                        {
                                            if (M2Share.Config.boRunNpc)
                                            {
                                                continue;
                                            }
                                        }
                                        else
                                        {
                                            if (baseObject.Race == Grobal2.RC_GUARD || baseObject.Race == Grobal2.RC_ARCHERGUARD)
                                            {
                                                if (M2Share.Config.boRunGuard)
                                                {
                                                    continue;
                                                }
                                            }
                                            else
                                            {
                                                if (M2Share.Config.boRunMon || Flag.boRUNMON)
                                                {
                                                    continue;
                                                }
                                            }
                                        }
                                    }
                                }
                                if (!baseObject.Ghost && baseObject.Bo2B9 && !baseObject.Death && !baseObject.FixedHideMode && !baseObject.ObMode)
                                {
                                    result = false;
                                    break;
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
        public int DeleteFromMap(int nX, int nY, CellType cellType, EntityId pRemoveObject)
        {
            const string sExceptionMsg1 = "[Exception] TEnvirnoment::DeleteFromMap -> Except 1 ** {0}";
            const string sExceptionMsg2 = "[Exception] TEnvirnoment::DeleteFromMap -> Except 2 ** {0}";
            var result = -1;
            var cellSuccess = false;
            MapCellInfo cellInfo = GetCellInfo(nX, nY, ref cellSuccess);
            if (cellSuccess && cellInfo.IsAvailable)
            {
                try
                {
                    for (var i = 0; i < cellInfo.Count; i++)
                    {
                        var osObject = cellInfo.ObjList[i];
                        if (osObject != null)
                        {
                            if (osObject.CellType == cellType && osObject.CellObjId == pRemoveObject.ActorId)
                            {
                                cellInfo.Remove(osObject);
                                result = 1;
                                if (osObject.ActorObject && !((BaseObject)pRemoveObject).DelFormMaped)
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
            var cellsuccess = false;
            MapCellInfo cellInfo = GetCellInfo(nX, nY, ref cellsuccess);
            if (cellsuccess && cellInfo.Valid)
            {
                Bo2C = true;
                if (cellInfo.IsAvailable)
                {
                    for (var i = 0; i < cellInfo.Count; i++)
                    {
                        var osObject = cellInfo.ObjList[i];
                        if (osObject.CellType == CellType.Item)
                        {
                            return (MapItem)M2Share.CellObjectSystem.Get(osObject.CellObjId);;
                        }
                        if (osObject.CellType == CellType.Route)
                        {
                            Bo2C = false;
                        }
                        if (osObject.CellType == CellType.Monster || osObject.CellType == CellType.Play)
                        {
                            var baseObject = M2Share.ActorMgr.Get(osObject.CellObjId);;
                            if (!baseObject.Death)
                            {
                                Bo2C = false;
                            }
                        }
                    }
                }
            }
            return null;
        }

        public bool IsCheapStuff()
        {
            bool result;
            if (_questList.Count > 0)
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
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
                    var osObject = new CellObject();
                    osObject.CellType = nType;
                    osObject.CellObjId = stoneMineEvent.Id;
                    osObject.AddTime = HUtil32.GetTickCount();
                    cellInfo.Add(osObject, stoneMineEvent);
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
            var cellsuccess = false;
            try
            {
                MapCellInfo cellInfo = GetCellInfo(nX, nY, ref cellsuccess);
                var bo1A = false;
                if (cellsuccess && cellInfo.Attribute != CellAttribute.Walk)
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
                            var osObject = new CellObject
                            {
                                CellType = nType,
                                CellObjId = stoneMineEvent.Id,
                                AddTime = HUtil32.GetTickCount()
                            };
                            cellInfo.Add(osObject, stoneMineEvent);
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
            CellObject osObject;
            bool boVerify = false;
            var cellsuccess = false;
            const string sExceptionMsg = "[Exception] TEnvirnoment::VerifyMapTime";
            try
            {
                MapCellInfo cellInfo = GetCellInfo(nX, nY, ref cellsuccess);
                if (cellsuccess && cellInfo.IsAvailable)
                {
                    for (var i = 0; i < cellInfo.Count; i++)
                    {
                        osObject = cellInfo.ObjList[i];
                        if (osObject.ActorObject && osObject.CellObjId == baseObject.ActorId)
                        {
                            osObject.AddTime = HUtil32.GetTickCount();
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
            byte[] buffer;
            int point;
            TDoorInfo door;
            var muiSize = 12;//固定大小
            try
            {
                if (File.Exists(sMapFile))
                {
                    using var fileStream = new FileStream(sMapFile, FileMode.Open, FileAccess.Read);
                    using var binReader = new BinaryReader(fileStream);

                    var bytData = new byte[52];
                    binReader.Read(bytData, 0, bytData.Length);
                    Width = BitConverter.ToInt16(bytData, 0);
                    Height = BitConverter.ToInt16(bytData, 2);

                    Initialize(Width, Height);

                    var nMapSize = Width * muiSize * Height;
                    buffer = new byte[nMapSize];
                    binReader.Read(buffer, 0, nMapSize);
                    var buffIndex = 0;

                    for (var nW = 0; nW < Width; nW++)
                    {
                        n24 = nW * Height;
                        for (var nH = 0; nH < Height; nH++)
                        {
                            // wBkImg High
                            if ((buffer[buffIndex + 1] & 0x80) != 0)
                            {
                                _cellArray.Span[n24 + nH] = MapCellInfoConst.HighWall;
                            }
                            // wFrImg High
                            if ((buffer[buffIndex + 5] & 0x80) != 0)
                            {
                                _cellArray.Span[n24 + nH] = MapCellInfoConst.LowWall;
                            }
                            if (_cellArray.Span[n24 + nH] == null)
                            {
                                _cellArray.Span[n24 + nH] = new MapCellInfo();
                            }
                            // btDoorIndex
                            if ((buffer[buffIndex + 6] & 0x80) != 0)
                            {
                                point = buffer[buffIndex + 6] & 0x7F;
                                if (point > 0)
                                {
                                    door = new TDoorInfo
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
                                        door.Status = new TDoorStatus
                                        {
                                            boOpened = false,
                                            bo01 = false,
                                            n04 = 0,
                                            dwOpenTick = 0,
                                            nRefCount = 1
                                        };
                                    }
                                    DoorList.Add(door);
                                }
                            }
                            buffIndex += muiSize;
                        }
                    }
                    binReader.Close();
                    binReader.Dispose();
                    fileStream.Close();
                    fileStream.Dispose();
                    buffer = null;
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
                        line = HUtil32.GetValidStr3(line, ref sX, new[] { ",", "\t" });
                        line = HUtil32.GetValidStr3(line, ref sY, new[] { ",", "\t" });
                        var nX = (short)HUtil32.Str_ToInt(sX, -1);
                        var nY = (short)HUtil32.Str_ToInt(sY, -1);
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
                if (!_cellArray.IsEmpty)
                {
                    for (var nW = 0; nW < Width; nW++)
                    {
                        for (var nH = 0; nH < Height; nH++)
                        {
                            if (_cellArray.Span[nW * Height + nH].ObjList != null)
                            {
                                _cellArray.Span[nW * Height + nH].Dispose();
                            }
                        }
                    }
                    _cellArray = null;
                }
                Width = nWidth;
                Height = nHeight;
                //_cellArray = new MapCellInfo[nWidth * nHeight];
                _cellArray = _cellPool.Rent(nWidth * nHeight).Memory;
            }
        }

        public bool CreateQuest(int nFlag, int nValue, string sMonName, string sItem, string sQuest, bool boGrouped)
        {
            TMapQuestInfo mapQuest;
            Merchant mapMerchant;
            var result = false;
            if (nFlag < 0)
            {
                return result;
            }
            mapQuest = new TMapQuestInfo
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
            mapMerchant = new Merchant
            {
                MapName = "0",
                CurrX = 0,
                CurrY = 0,
                CharName = sQuest,
                m_nFlag = 0,
                Appr = 0,
                m_sFilePath = "MapQuest_def",
                m_boIsHide = true,
                m_boIsQuest = false
            };
            M2Share.WorldEngine.QuestNpcList.Add(mapMerchant);
            mapQuest.NPC = mapMerchant;
            _questList.Add(mapQuest);
            result = true;
            return result;
        }

        public int GetXyObjCount(int nX, int nY)
        {
            var result = 0;
            var cellsuccess = false;
            MapCellInfo cellInfo = GetCellInfo(nX, nY, ref cellsuccess);
            if (cellsuccess && cellInfo.IsAvailable)
            {
                for (var i = 0; i < cellInfo.Count; i++)
                {
                    var osObject = cellInfo.ObjList[i];
                    if (osObject == null)
                    {
                        continue;
                    }
                    if (osObject.ActorObject)
                    {
                        var baseObject = M2Share.ActorMgr.Get(osObject.CellObjId);;
                        if (baseObject != null)
                        {
                            if (!baseObject.Ghost && baseObject.Bo2B9 && !baseObject.Death && !baseObject.FixedHideMode && !baseObject.ObMode)
                            {
                                result++;
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
            var cellsuccess = false;
            MapCellInfo cellInfo = GetCellInfo(nX, nY, ref cellsuccess);
            if (cellsuccess && cellInfo.IsAvailable)
            {
                for (var i = 0; i < cellInfo.Count; i++)
                {
                    var osObject = cellInfo.ObjList[i];
                    if (osObject.CellType == CellType.Event)
                    {
                        var owinEvent = (MirEvent)M2Share.CellObjectSystem.Get(osObject.CellObjId);
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
                    if (!door.Status.boOpened)
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
            var cellsuccess = false;
            MapCellInfo cellInfo = GetCellInfo(nX, nY, ref cellsuccess);
            if (cellsuccess && cellInfo.IsAvailable)
            {
                for (var i = 0; i < cellInfo.Count; i++)
                {
                    var osObject = cellInfo.ObjList[i];
                    if (osObject.ActorObject)
                    {
                        var baseObject = M2Share.ActorMgr.Get(osObject.CellObjId); ;
                        if (baseObject != null && !baseObject.Ghost && baseObject.Bo2B9 && (!boFlag || !baseObject.Death))
                        {
                            result = baseObject;
                            break;
                        }
                    }
                }
            }
            return result;
        }

        public Merchant GetQuestNpc(BaseObject baseObject, string sCharName, string sItem, bool boFlag)
        {
            bool bo1D;
            for (var i = 0; i < _questList.Count; i++)
            {
                var mapQuestFlag = _questList[i];
                var nFlagValue = baseObject.GetQuestFalgStatus(mapQuestFlag.nFlag);
                if (nFlagValue == mapQuestFlag.nValue)
                {
                    if (boFlag == mapQuestFlag.boGrouped || !boFlag)
                    {
                        bo1D = false;
                        if (!string.IsNullOrEmpty(mapQuestFlag.sMonName) && !string.IsNullOrEmpty(mapQuestFlag.sItemName))
                        {
                            if (mapQuestFlag.sMonName == sCharName && mapQuestFlag.sItemName == sItem)
                            {
                                bo1D = true;
                            }
                        }
                        if (!string.IsNullOrEmpty(mapQuestFlag.sMonName) && string.IsNullOrEmpty(mapQuestFlag.sItemName))
                        {
                            if (mapQuestFlag.sMonName == sCharName && string.IsNullOrEmpty(sItem))
                            {
                                bo1D = true;
                            }
                        }
                        if (string.IsNullOrEmpty(mapQuestFlag.sMonName) && !string.IsNullOrEmpty(mapQuestFlag.sItemName))
                        {
                            if (mapQuestFlag.sItemName == sItem)
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
            var cellsuccess = false;
            MapCellInfo cellInfo = GetCellInfo(nX, nY, ref cellsuccess);
            if (cellsuccess && cellInfo.Valid)
            {
                Bo2C = true;
                if (cellInfo.IsAvailable)
                {
                    for (var i = 0; i < cellInfo.Count; i++)
                    {
                        var osObject = cellInfo.ObjList[i];
                        if (osObject.CellType == CellType.Item)
                        {
                            result = osObject.CellObjId;
                            nCount++;
                        }
                        if (osObject.CellType == CellType.Route)
                        {
                            Bo2C = false;
                        }
                        if (osObject.CellType == CellType.Monster || osObject.CellType == CellType.Play)
                        {
                            var baseObject = M2Share.ActorMgr.Get(osObject.CellObjId);
                            if (!baseObject.Death)
                            {
                                Bo2C = false;
                            }
                        }
                    }
                }
            }
            return result;
        }

        public TDoorInfo GetDoor(int nX, int nY)
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
                    var cellsuccess = false;
                    var cellInfo = GetCellInfo(nX, nY, ref cellsuccess);
                    if (cellsuccess && cellInfo.IsAvailable)
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
            var cellsuccess = false;
            MapCellInfo cellInfo = GetCellInfo(nX, nY, ref cellsuccess);
            if (cellsuccess && cellInfo.IsAvailable)
            {
                for (var i = 0; i < cellInfo.Count; i++)
                {
                    var osObject = cellInfo.ObjList[i];
                    if (osObject.ActorObject)
                    {
                        var baseObject = M2Share.ActorMgr.Get(osObject.CellObjId);;
                        if (baseObject != null)
                        {
                            if (!baseObject.Ghost && baseObject.Bo2B9 && !boFlag || !baseObject.Death)
                            {
                                baseObjectList.Add(baseObject);
                            }
                        }
                    }
                }
            }
            return baseObjectList.Count;
        }

        public MirEvent GetEvent(int nX, int nY)
        {
            MirEvent result = null;
            Bo2C = false;
            var cellsuccess = false;
            MapCellInfo cellInfo = GetCellInfo(nX, nY, ref cellsuccess);
            if (cellsuccess && cellInfo.IsAvailable)
            {
                for (var i = 0; i < cellInfo.Count; i++)
                {
                    var osObject = cellInfo.ObjList[i];
                    if (osObject.CellType == CellType.Event)
                    {
                        result = (MirEvent)M2Share.CellObjectSystem.Get(osObject.CellObjId);;
                    }
                }
            }
            return result;
        }

        public void SetMapXyFlag(int nX, int nY, bool boFlag)
        {
            var cellsuccess = false;
            MapCellInfo cellInfo = GetCellInfo(nX, nY, ref cellsuccess);
            if (cellsuccess)
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
        /// <param name="nMapX"></param>
        /// <param name="nMapY"></param>
        /// <returns></returns>
        public bool GetXyHuman(int nMapX, int nMapY)
        {
            var cellsuccess = false;
            var result = false;
            MapCellInfo cellInfo = GetCellInfo(nMapX, nMapY, ref cellsuccess);
            if (cellsuccess && cellInfo.IsAvailable)
            {
                for (var i = 0; i < cellInfo.Count; i++)
                {
                    var osObject = cellInfo.ObjList[i];
                    if (osObject.CellType == CellType.Play)
                    {
                        var baseObject = M2Share.ActorMgr.Get(osObject.CellObjId);;
                        if (baseObject.Race == Grobal2.RC_PLAYOBJECT)
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
            var cellsuccess = false;
            MapCellInfo cellInfo = GetCellInfo(nX, nY, ref cellsuccess);
            if (cellsuccess && cellInfo.Attribute == CellAttribute.LowWall)
            {
                return false;
            }
            return true;
        }

        public string GetEnvirInfo()
        {
            string sMsg;
            sMsg = "Map:%s(%s) DAY:%s DARK:%s SAFE:%s FIGHT:%s FIGHT3:%s QUIZ:%s NORECONNECT:%s(%s) MUSIC:%s(%d) EXPRATE:%s(%f) PKWINLEVEL:%s(%d) PKLOSTLEVEL:%s(%d) PKWINEXP:%s(%d) PKLOSTEXP:%s(%d) DECHP:%s(%d/%d) INCHP:%s(%d/%d)";
            sMsg = sMsg + " DECGAMEGOLD:%s(%d/%d) INCGAMEGOLD:%s(%d/%d) INCGAMEPOINT:%s(%d/%d) RUNHUMAN:%s RUNMON:%s NEEDHOLE:%s NORECALL:%s NOGUILDRECALL:%s NODEARRECALL:%s NOMASTERRECALL:%s NODRUG:%s MINE:%s MINE2:%s NODROPITEM:%s";
            sMsg = sMsg + " NOTHROWITEM:%s NOPOSITIONMOVE:%s NOHORSE:%s NOHUMNOMON:%s NOCHAT:%s ";
            var result = string.Format(sMsg, MapName, MapDesc, HUtil32.BoolToStr(Flag.boDayLight), HUtil32.BoolToStr(Flag.boDarkness), HUtil32.BoolToStr(Flag.boSAFE), HUtil32.BoolToStr(Flag.boFightZone),
                HUtil32.BoolToStr(Flag.boFight3Zone), HUtil32.BoolToStr(Flag.boQUIZ), HUtil32.BoolToStr(Flag.boNORECONNECT), Flag.sNoReConnectMap, HUtil32.BoolToStr(Flag.boMUSIC), Flag.nMUSICID, HUtil32.BoolToStr(Flag.boEXPRATE),
                Flag.nEXPRATE / 100, HUtil32.BoolToStr(Flag.boPKWINLEVEL), Flag.nPKWINLEVEL, HUtil32.BoolToStr(Flag.boPKLOSTLEVEL), Flag.nPKLOSTLEVEL, HUtil32.BoolToStr(Flag.boPKWINEXP), Flag.nPKWINEXP, HUtil32.BoolToStr(Flag.boPKLOSTEXP),
                Flag.nPKLOSTEXP, HUtil32.BoolToStr(Flag.boDECHP), Flag.nDECHPTIME, Flag.nDECHPPOINT, HUtil32.BoolToStr(Flag.boINCHP), Flag.nINCHPTIME, Flag.nINCHPPOINT, HUtil32.BoolToStr(Flag.boDECGAMEGOLD), Flag.nDECGAMEGOLDTIME,
                Flag.nDECGAMEGOLD, HUtil32.BoolToStr(Flag.boINCGAMEGOLD), Flag.nINCGAMEGOLDTIME, Flag.nINCGAMEGOLD, HUtil32.BoolToStr(Flag.boINCGAMEPOINT), Flag.nINCGAMEPOINTTIME, Flag.nINCGAMEPOINT, HUtil32.BoolToStr(Flag.boRUNHUMAN),
                HUtil32.BoolToStr(Flag.boRUNMON), HUtil32.BoolToStr(Flag.boNEEDHOLE), HUtil32.BoolToStr(Flag.boNORECALL), HUtil32.BoolToStr(Flag.boNOGUILDRECALL), HUtil32.BoolToStr(Flag.boNODEARRECALL), HUtil32.BoolToStr(Flag.boNOMASTERRECALL),
                HUtil32.BoolToStr(Flag.boNODRUG), HUtil32.BoolToStr(Flag.boMINE), HUtil32.BoolToStr(Flag.boMINE2), HUtil32.BoolToStr(Flag.boNODROPITEM), HUtil32.BoolToStr(Flag.boNOTHROWITEM), HUtil32.BoolToStr(Flag.boNOPOSITIONMOVE),
                HUtil32.BoolToStr(Flag.boNOHORSE), HUtil32.BoolToStr(Flag.boNOHUMNOMON), HUtil32.BoolToStr(Flag.boNOCHAT));
            return result;
        }

        public void AddObject(BaseObject baseObject)
        {
            var btRaceServer = baseObject.Race;
            if (btRaceServer == Grobal2.RC_PLAYOBJECT)
            {
                _humCount++;
            }
            if (btRaceServer >= Grobal2.RC_ANIMAL)
            {
                _monCount++;
            }
        }

        public void DelObjectCount(BaseObject baseObject)
        {
            var btRaceServer = baseObject.Race;
            if (btRaceServer == Grobal2.RC_PLAYOBJECT)
            {
                _humCount--;
            }
            if (btRaceServer >= Grobal2.RC_ANIMAL)
            {
                _monCount--;
            }
        }
    }
}

