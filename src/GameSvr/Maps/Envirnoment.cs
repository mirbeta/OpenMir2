using GameSvr.Actor;
using GameSvr.Castle;
using GameSvr.Event;
using GameSvr.Event.Events;
using GameSvr.Npc;
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
        public int MonCount => _mNMonCount;
        /// <summary>
        /// 玩家数量
        /// </summary>
        public int HumCount => _mNHumCount;
        public short WWidth;
        public short WHeight;
        public string MSMapFileName = string.Empty;
        public string SMapName = string.Empty;
        public string SMapDesc = string.Empty;
        private MapCellinfo[] _mapCellArray;
        public int NMinMap = 0;
        public int NServerIndex = 0;
        /// <summary>
        /// 进入本地图所需等级
        /// </summary>
        public int NRequestLevel = 0;
        public TMapFlag Flag = null;
        public bool Bo2C = false;
        /// <summary>
        /// 门
        /// </summary>
        public IList<TDoorInfo> MDoorList = null;
        public object QuestNpc = null;
        /// <summary>
        /// 任务
        /// </summary>
        public IList<TMapQuestInfo> MQuestList = null;
        public int MDwWhisperTick = 0;
        private int _mNMonCount = 0;
        private int _mNHumCount = 0;
        public IList<PointInfo> MPointList;

        public Envirnoment()
        {
            SMapName = string.Empty;
            NServerIndex = 0;
            NMinMap = 0;
            Flag = new TMapFlag();
            _mNMonCount = 0;
            _mNHumCount = 0;
            MDoorList = new List<TDoorInfo>();
            MQuestList = new List<TMapQuestInfo>();
            MDwWhisperTick = 0;
        }

        ~Envirnoment()
        {
            _mapCellArray = null;
        }

        public bool AllowMagics(string magicName)
        {
            return true;
        }

        /// <summary>
        /// 判断地图是否禁用技能
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
        public object AddToMap(int nX, int nY, CellType btType, object pRemoveObject)
        {
            object result = null;
            MapCellinfo mapCellInfo;
            CellObject osObject;
            MapItem mapItem;
            int nGoldCount;
            const string sExceptionMsg = "[Exception] TEnvirnoment::AddToMap";
            try
            {
                var bo1E = false;
                var mapCell = false;
                mapCellInfo = GetMapCellInfo(nX, nY, ref mapCell);
                if (mapCell && mapCellInfo.Valid)
                {
                    if (mapCellInfo.ObjList == null)
                    {
                        mapCellInfo.ObjList = new List<CellObject>();
                    }
                    else
                    {
                        if (btType == CellType.OS_ITEMOBJECT)
                        {
                            if (((MapItem)pRemoveObject).Name == Grobal2.sSTRING_GOLDNAME)
                            {
                                for (var i = 0; i < mapCellInfo.Count; i++)
                                {
                                    osObject = mapCellInfo.ObjList[i];
                                    if (osObject.CellType == CellType.OS_ITEMOBJECT)
                                    {
                                        mapItem = (MapItem)mapCellInfo.ObjList[i].CellObj;
                                        if (mapItem.Name == Grobal2.sSTRING_GOLDNAME)
                                        {
                                            nGoldCount = mapItem.Count + ((MapItem)pRemoveObject).Count;
                                            if (nGoldCount <= 2000)
                                            {
                                                mapItem.Count = nGoldCount;
                                                mapItem.Looks = M2Share.GetGoldShape(nGoldCount);
                                                mapItem.AniCount = 0;
                                                mapItem.Reserved = 0;
                                                osObject.dwAddTime = HUtil32.GetTickCount();
                                                result = mapItem;
                                                bo1E = true;
                                            }
                                        }
                                    }
                                }
                            }
                            if (!bo1E && mapCellInfo.Count >= 5)
                            {
                                result = null;
                                bo1E = true;
                            }
                        }
                    }
                    if (!bo1E)
                    {
                        osObject = new CellObject
                        {
                            CellType = btType,
                            CellObj = pRemoveObject,
                            dwAddTime = HUtil32.GetTickCount()
                        };
                        mapCellInfo.Add(osObject);
                        result = pRemoveObject;
                        if (btType == CellType.OS_MOVINGOBJECT && !((TBaseObject)pRemoveObject).m_boAddToMaped)
                        {
                            ((TBaseObject)pRemoveObject).m_boDelFormMaped = false;
                            ((TBaseObject)pRemoveObject).m_boAddToMaped = true;
                            AddObject(pRemoveObject);
                        }
                    }
                }
            }
            catch
            {
                M2Share.ErrorMessage(sExceptionMsg);
            }
            return result;
        }

        public void AddDoorToMap()
        {
            TDoorInfo door;
            for (var i = 0; i < MDoorList.Count; i++)
            {
                door = MDoorList[i];
                AddToMap(door.nX, door.nY, CellType.OS_DOOR, door);
            }
        }

        public bool GetMapCellInfo(int nX, int nY, ref MapCellinfo mapCellInfo)
        {
            bool result;
            if (nX >= 0 && nX < WWidth && nY >= 0 && nY < WHeight)
            {
                mapCellInfo = _mapCellArray[nX * WHeight + nY];
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }

        public MapCellinfo GetMapCellInfo(int nX, int nY, ref bool success)
        {
            if (nX >= 0 && nX < WWidth && nY >= 0 && nY < WHeight)
            {
                MapCellinfo mapCell = _mapCellArray[nX * WHeight + nY];
                if (mapCell == null)
                {
                    success = false;
                    return default(MapCellinfo);
                }
                success = true;
                return mapCell;
            }
            success = false;
            return default(MapCellinfo);
        }

        public int MoveToMovingObject(int nCx, int nCy, TBaseObject cert, int nX, int nY, bool boFlag)
        {
            MapCellinfo mapCellInfo;
            bool mapCell = false;
            TBaseObject baseObject;
            CellObject osObject;
            bool bo1A;
            const string sExceptionMsg = "[Exception] TEnvirnoment::MoveToMovingObject";
            var result = 0;
            try
            {
                bo1A = true;
                mapCellInfo = GetMapCellInfo(nX, nY, ref mapCell);
                if (!boFlag && mapCell)
                {
                    if (mapCellInfo.Valid)
                    {
                        if (mapCellInfo.ObjList != null)
                        {
                            for (var i = 0; i < mapCellInfo.Count; i++)
                            {
                                if (mapCellInfo.ObjList[i].CellType == CellType.OS_MOVINGOBJECT)
                                {
                                    baseObject = (TBaseObject)mapCellInfo.ObjList[i].CellObj;
                                    if (baseObject != null)
                                    {
                                        if (!baseObject.m_boGhost && baseObject.bo2B9 && !baseObject.m_boDeath && !baseObject.m_boFixedHideMode && !baseObject.m_boObMode)
                                        {
                                            bo1A = false;
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
                        bo1A = false;
                    }
                }
                if (bo1A)
                {
                    if (GetMapCellInfo(nX, nY, ref mapCellInfo) && mapCellInfo.Attribute != 0)
                    {
                        result = -1;
                    }
                    else
                    {
                        if (GetMapCellInfo(nCx, nCy, ref mapCellInfo) && mapCellInfo.ObjList != null)
                        {
                            var i = 0;
                            while (true)
                            {
                                if (mapCellInfo.Count <= i)
                                {
                                    break;
                                }
                                osObject = mapCellInfo.ObjList[i];
                                if (osObject.CellType == CellType.OS_MOVINGOBJECT)
                                {
                                    if ((TBaseObject)osObject.CellObj == cert)
                                    {
                                        mapCellInfo.Remove(i);
                                        osObject = null;
                                        if (mapCellInfo.Count > 0)
                                        {
                                            continue;
                                        }
                                        mapCellInfo.Dispose();
                                        break;
                                    }
                                }
                                i++;
                            }
                        }
                        if (GetMapCellInfo(nX, nY, ref mapCellInfo))
                        {
                            if (mapCellInfo.ObjList == null)
                            {
                                mapCellInfo.ObjList = new List<CellObject>();
                            }
                            osObject = new CellObject
                            {
                                CellType = CellType.OS_MOVINGOBJECT,
                                CellObj = cert,
                                dwAddTime = HUtil32.GetTickCount()
                            };
                            mapCellInfo.Add(osObject);
                            result = 1;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                M2Share.ErrorMessage(sExceptionMsg);
                M2Share.ErrorMessage(e.Message);
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
            CellObject osObject;
            TBaseObject baseObject;
            var result = false;
            var mapCell = false;
            MapCellinfo mapCellInfo = GetMapCellInfo(nX, nY, ref mapCell);
            if (mapCell && mapCellInfo.Valid)
            {
                if (boFlag)
                {
                    return true;
                }
                result = true;
                if (!boFlag && mapCellInfo.ObjList != null)
                {
                    for (var i = 0; i < mapCellInfo.Count; i++)
                    {
                        osObject = mapCellInfo.ObjList[i];
                        if (osObject.CellType == CellType.OS_MOVINGOBJECT)
                        {
                            baseObject = (TBaseObject)osObject.CellObj;
                            if (baseObject != null)
                            {
                                if (!baseObject.m_boGhost && baseObject.bo2B9 && !baseObject.m_boDeath && !baseObject.m_boFixedHideMode && !baseObject.m_boObMode)
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
            var mapCell = false;
            CellObject osObject;
            TBaseObject baseObject;
            var result = true;
            MapCellinfo mapCellInfo = GetMapCellInfo(nX, nY, ref mapCell);
            if (mapCell && mapCellInfo.Valid)
            {
                if (mapCellInfo.ObjList != null)
                {
                    for (var i = 0; i < mapCellInfo.Count; i++)
                    {
                        osObject = mapCellInfo.ObjList[i];
                        if (!boFlag && osObject.CellType == CellType.OS_MOVINGOBJECT)
                        {
                            baseObject = (TBaseObject)osObject.CellObj;
                            if (baseObject != null)
                            {
                                if (!baseObject.m_boGhost && baseObject.bo2B9 && !baseObject.m_boDeath && !baseObject.m_boFixedHideMode && !baseObject.m_boObMode)
                                {
                                    result = false;
                                    break;
                                }
                            }
                        }
                        if (!boItem && osObject.CellType == CellType.OS_ITEMOBJECT)
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
            CellObject osObject;
            TBaseObject baseObject;
            TUserCastle castle;
            var result = false;
            var mapCell = false;
            MapCellinfo mapCellInfo = GetMapCellInfo(nX, nY, ref mapCell);
            if (mapCell && mapCellInfo.Valid)
            {
                result = true;
                if (!boFlag && mapCellInfo.ObjList != null)
                {
                    for (var i = 0; i < mapCellInfo.Count; i++)
                    {
                        osObject = mapCellInfo.ObjList[i];
                        if (osObject.CellType == CellType.OS_MOVINGOBJECT)
                        {
                            baseObject = (TBaseObject)osObject.CellObj;
                            if (baseObject != null)
                            {
                                castle = M2Share.CastleManager.InCastleWarArea(baseObject);
                                if (M2Share.g_Config.boWarDisHumRun && castle != null && castle.m_boUnderWar)
                                {
                                }
                                else
                                {
                                    if (baseObject.m_btRaceServer == Grobal2.RC_PLAYOBJECT)
                                    {
                                        if (M2Share.g_Config.boRunHuman || Flag.boRUNHUMAN)
                                        {
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        if (baseObject.m_btRaceServer == Grobal2.RC_NPC)
                                        {
                                            if (M2Share.g_Config.boRunNpc)
                                            {
                                                continue;
                                            }
                                        }
                                        else
                                        {
                                            if (baseObject.m_btRaceServer == Grobal2.RC_GUARD || baseObject.m_btRaceServer == Grobal2.RC_ARCHERGUARD)
                                            {
                                                if (M2Share.g_Config.boRunGuard)
                                                {
                                                    continue;
                                                }
                                            }
                                            else
                                            {
                                                if (M2Share.g_Config.boRunMon || Flag.boRUNMON)
                                                {
                                                    continue;
                                                }
                                            }
                                        }
                                    }
                                }
                                if (!baseObject.m_boGhost && baseObject.bo2B9 && !baseObject.m_boDeath && !baseObject.m_boFixedHideMode && !baseObject.m_boObMode)
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

        public int DeleteFromMap(int nX, int nY, CellType cellType, object pRemoveObject)
        {
            CellObject osObject;
            int n18;
            const string sExceptionMsg1 = "[Exception] TEnvirnoment::DeleteFromMap -> Except 1 ** %d";
            const string sExceptionMsg2 = "[Exception] TEnvirnoment::DeleteFromMap -> Except 2 ** %d";
            var result = -1;
            var mapCell = false;
            try
            {
                MapCellinfo mapCellInfo = GetMapCellInfo(nX, nY, ref mapCell);
                if (mapCell)
                {
                    if (mapCell)
                    {
                        try
                        {
                            if (mapCellInfo.ObjList != null)
                            {
                                n18 = 0;
                                while (true)
                                {
                                    if (mapCellInfo.Count <= n18)
                                    {
                                        break;
                                    }
                                    osObject = mapCellInfo.ObjList[n18];
                                    if (osObject != null)
                                    {
                                        if (osObject.CellType == cellType && osObject.CellObj == pRemoveObject)
                                        {
                                            mapCellInfo.Remove(n18);
                                            osObject = null;
                                            result = 1;
                                            // 减地图人物怪物计数
                                            if (cellType == CellType.OS_MOVINGOBJECT && !((TBaseObject)pRemoveObject).m_boDelFormMaped)
                                            {
                                                ((TBaseObject)pRemoveObject).m_boDelFormMaped = true;
                                                ((TBaseObject)pRemoveObject).m_boAddToMaped = false;
                                                DelObjectCount(pRemoveObject);
                                            }
                                            if (mapCellInfo.Count > 0)
                                            {
                                                continue;
                                            }
                                            mapCellInfo.Dispose();
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        mapCellInfo.Remove(n18);
                                        if (mapCellInfo.Count > 0)
                                        {
                                            continue;
                                        }
                                        mapCellInfo.Dispose();
                                        break;
                                    }
                                    n18++;
                                }
                            }
                            else
                            {
                                result = -2;
                            }
                        }
                        catch
                        {
                            osObject = null;
                            M2Share.MainOutMessage(string.Format(sExceptionMsg1, cellType));
                        }
                    }
                    else
                    {
                        result = -3;
                    }
                }
                else
                {
                    result = 0;
                }
            }
            catch
            {
                M2Share.MainOutMessage(string.Format(sExceptionMsg2, cellType));
            }
            return result;
        }

        public MapItem GetItem(int nX, int nY)
        {
            CellObject osObject;
            TBaseObject baseObject;
            MapItem result = null;
            Bo2C = false;
            var mapCell = false;
            MapCellinfo mapCellInfo = GetMapCellInfo(nX, nY, ref mapCell);
            if (mapCell && mapCellInfo.Valid)
            {
                Bo2C = true;
                if (mapCellInfo.ObjList != null)
                {
                    for (var i = 0; i < mapCellInfo.Count; i++)
                    {
                        osObject = mapCellInfo.ObjList[i];
                        if (osObject.CellType == CellType.OS_ITEMOBJECT)
                        {
                            result = (MapItem)osObject.CellObj;
                            return result;
                        }
                        if (osObject.CellType == CellType.OS_GATEOBJECT)
                        {
                            Bo2C = false;
                        }
                        if (osObject.CellType == CellType.OS_MOVINGOBJECT)
                        {
                            baseObject = (TBaseObject)osObject.CellObj;
                            if (!baseObject.m_boDeath)
                            {
                                Bo2C = false;
                            }
                        }
                    }
                }
            }
            return result;
        }

        public bool IsCheapStuff()
        {
            bool result;
            if (MQuestList.Count > 0)
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }

        public object AddToMapItemEvent(int nX, int nY, CellType nType, object @event)
        {
            object result = null;
            var mapCell = false;
            MapCellinfo mapCellInfo = GetMapCellInfo(nX, nY, ref mapCell);
            if (mapCell && mapCellInfo.Valid)
            {
                if (mapCellInfo.ObjList == null)
                {
                    mapCellInfo.ObjList = new List<CellObject>();
                }
                if (nType == CellType.OS_EVENTOBJECT)
                {
                    var osObject = new CellObject();
                    osObject.CellType = nType;
                    osObject.CellObj = @event;
                    osObject.dwAddTime = HUtil32.GetTickCount();
                    mapCellInfo.Add(osObject);
                    result = osObject;
                }
            }
            return result;
        }

        /// <summary>
        /// 添加矿石到地图上
        /// </summary>
        /// <param name="nX"></param>
        /// <param name="nY"></param>
        /// <param name="nType"></param>
        /// <param name="stoneMineEvent"></param>
        /// <returns></returns>
        public object AddToMapMineEvent(int nX, int nY, CellType nType, StoneMineEvent stoneMineEvent)
        {
            MapCellinfo mc = new MapCellinfo();
            const string sExceptionMsg = "[Exception] TEnvirnoment::AddToMapMineEvent ";
            var mapCell = false;
            try
            {
                MapCellinfo mapCellInfo = GetMapCellInfo(nX, nY, ref mapCell);
                var bo1A = false;
                if (mapCell && mapCellInfo.Attribute != 0)
                {
                    var isSpace = false;// 人物可以走到的地方才放上矿藏
                    for (var x = nX - 1; x <= nX + 1; x++)
                    {
                        for (var y = nY - 1; y <= nY + 1; y++)
                        {
                            if (GetMapCellInfo(x, y, ref mc))
                            {
                                if ((mc.Valid))
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
                        if (mapCellInfo.ObjList == null)
                        {
                            mapCellInfo.ObjList = new List<CellObject>();
                        }
                        if (!bo1A)
                        {
                            var osObject = new CellObject
                            {
                                CellType = nType,
                                CellObj = stoneMineEvent,
                                dwAddTime = HUtil32.GetTickCount()
                            };
                            mapCellInfo.Add(osObject);
                            return stoneMineEvent;
                        }
                    }
                }
            }
            catch
            {
                M2Share.ErrorMessage(sExceptionMsg);
            }
            return null;
        }

        /// <summary>
        /// 刷新在地图上位置的时间
        /// </summary>
        /// <param name="nX"></param>
        /// <param name="nY"></param>
        /// <param name="baseObject"></param>
        public void VerifyMapTime(int nX, int nY, object baseObject)
        {
            CellObject osObject;
            bool boVerify;
            var mapCell = false;
            const string sExceptionMsg = "[Exception] TEnvirnoment::VerifyMapTime";
            try
            {
                boVerify = false;
                MapCellinfo mapCellInfo = GetMapCellInfo(nX, nY, ref mapCell);
                if (mapCell && mapCellInfo.ObjList != null)
                {
                    for (var i = 0; i < mapCellInfo.Count; i++)
                    {
                        osObject = mapCellInfo.ObjList[i];
                        if (osObject.CellType == CellType.OS_MOVINGOBJECT && osObject.CellObj == baseObject)
                        {
                            osObject.dwAddTime = HUtil32.GetTickCount();
                            boVerify = true;
                            break;
                        }
                    }
                }
                if (!boVerify)
                {
                    AddToMap(nX, nY, CellType.OS_MOVINGOBJECT, baseObject);
                }
            }
            catch
            {
                M2Share.ErrorMessage(sExceptionMsg);
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
                    WWidth = BitConverter.ToInt16(bytData, 0);
                    WHeight = BitConverter.ToInt16(bytData, 2);

                    Initialize(WWidth, WHeight);

                    var nMapSize = WWidth * muiSize * WHeight;
                    buffer = new byte[nMapSize];
                    binReader.Read(buffer, 0, nMapSize);
                    var buffIndex = 0;

                    for (var nW = 0; nW < WWidth; nW++)
                    {
                        n24 = nW * WHeight;
                        for (var nH = 0; nH < WHeight; nH++)
                        {
                            // wBkImg High
                            if ((buffer[buffIndex + 1] & 0x80) != 0)
                            {
                                _mapCellArray[n24 + nH] = MapCellinfo.HighWall;
                            }
                            // wFrImg High
                            if ((buffer[buffIndex + 5] & 0x80) != 0)
                            {
                                _mapCellArray[n24 + nH] = MapCellinfo.LowWall;
                            }
                            if (_mapCellArray[n24 + nH] == null)
                            {
                                _mapCellArray[n24 + nH] = new MapCellinfo();
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
                                    for (var i = 0; i < MDoorList.Count; i++)
                                    {
                                        if (Math.Abs(MDoorList[i].nX - door.nX) <= 10)
                                        {
                                            if (Math.Abs(MDoorList[i].nY - door.nY) <= 10)
                                            {
                                                if (MDoorList[i].n08 == point)
                                                {
                                                    door.Status = MDoorList[i].Status;
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
                                    MDoorList.Add(door);
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

                var pointFileName = Path.Combine(M2Share.sConfigPath, M2Share.g_Config.sEnvirDir, "Point", $"{sMapFile}.txt");
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
                        if (nX >= 0 && nY >= 0 && nX < WWidth && nY < WHeight)
                        {
                            MPointList.Add(new PointInfo(nX, nY));
                        }
                    }
                }
            }
            catch (Exception)
            {
                M2Share.MainOutMessage("[Exception] TEnvirnoment.LoadMapData");
            }
            return result;
        }

        private void Initialize(short nWidth, short nHeight)
        {
            if (nWidth > 1 && nHeight > 1)
            {
                if (_mapCellArray != null)
                {
                    for (var nW = 0; nW < WWidth; nW++)
                    {
                        for (var nH = 0; nH < WHeight; nH++)
                        {
                            if (_mapCellArray[nW * WHeight + nH].ObjList != null)
                            {
                                _mapCellArray[nW * WHeight + nH].Dispose();
                            }
                        }
                    }
                    _mapCellArray = null;
                }
                WWidth = nWidth;
                WHeight = nHeight;
                _mapCellArray = new MapCellinfo[nWidth * nHeight];
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
                m_sMapName = "0",
                m_nCurrX = 0,
                m_nCurrY = 0,
                m_sCharName = sQuest,
                m_nFlag = 0,
                m_wAppr = 0,
                m_sFilePath = "MapQuest_def",
                m_boIsHide = true,
                m_boIsQuest = false
            };
            M2Share.UserEngine.QuestNPCList.Add(mapMerchant);
            mapQuest.NPC = mapMerchant;
            MQuestList.Add(mapQuest);
            result = true;
            return result;
        }

        public int GetXyObjCount(int nX, int nY)
        {
            var result = 0;
            CellObject osObject;
            TBaseObject baseObject;
            var mapCell = false;
            MapCellinfo mapCellInfo = GetMapCellInfo(nX, nY, ref mapCell);
            if (mapCell && mapCellInfo.ObjList != null)
            {
                for (var i = 0; i < mapCellInfo.Count; i++)
                {
                    osObject = mapCellInfo.ObjList[i];
                    if (osObject.CellType == CellType.OS_MOVINGOBJECT)
                    {
                        baseObject = (TBaseObject)osObject.CellObj;
                        if (baseObject != null)
                        {
                            if (!baseObject.m_boGhost && baseObject.bo2B9 && !baseObject.m_boDeath && !baseObject.m_boFixedHideMode && !baseObject.m_boObMode)
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
                    if (sny < WWidth - nFlag)
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
                    if (snx < WWidth - nFlag)
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
                    if (snx > nFlag - 1 && sny < WHeight - nFlag)
                    {
                        snx += (short)nFlag;
                        sny -= (short)nFlag;
                    }
                    break;
                case Grobal2.DR_DOWNLEFT:
                    if (snx < WWidth - nFlag && sny > nFlag - 1)
                    {
                        snx -= (short)nFlag;
                        sny += (short)nFlag;
                    }
                    break;
                case Grobal2.DR_DOWNRIGHT:
                    if (snx < WWidth - nFlag && sny < WHeight - nFlag)
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
            CellObject osObject;
            var mapCell = false;
            MapCellinfo mapCellInfo = GetMapCellInfo(nX, nY, ref mapCell);
            if (mapCell && mapCellInfo.ObjList != null)
            {
                for (var i = mapCellInfo.Count - 1; i >= 0; i--)
                {
                    osObject = mapCellInfo.ObjList[i];
                    if (osObject.CellType == CellType.OS_EVENTOBJECT)
                    {
                        if (((MirEvent)osObject.CellObj).Damage > 0)
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
            TDoorInfo door;
            for (var i = 0; i < MDoorList.Count; i++)
            {
                door = MDoorList[i];
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
            CellObject osObject;
            TBaseObject baseObject;
            var mapCell = false;
            MapCellinfo mapCellInfo = GetMapCellInfo(nX, nY, ref mapCell);
            if (mapCell && mapCellInfo.ObjList != null)
            {
                for (var i = 0; i < mapCellInfo.Count; i++)
                {
                    osObject = mapCellInfo.ObjList[i];
                    if (osObject.CellType == CellType.OS_MOVINGOBJECT)
                    {
                        baseObject = (TBaseObject)osObject.CellObj;
                        if (baseObject != null && !baseObject.m_boGhost && baseObject.bo2B9 && (!boFlag || !baseObject.m_boDeath))
                        {
                            result = baseObject;
                            break;
                        }
                    }
                }
            }
            return result;
        }

        public object GetQuestNpc(TBaseObject baseObject, string sCharName, string sItem, bool boFlag)
        {
            object result = null;
            TMapQuestInfo mapQuestFlag;
            int nFlagValue;
            bool bo1D;
            for (var i = 0; i < MQuestList.Count; i++)
            {
                mapQuestFlag = MQuestList[i];
                nFlagValue = baseObject.GetQuestFalgStatus(mapQuestFlag.nFlag);
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
                            result = mapQuestFlag.NPC;
                            break;
                        }
                    }
                }
            }
            return result;
        }

        public object GetItemEx(int nX, int nY, ref int nCount)
        {
            object result = null;
            CellObject osObject;
            TBaseObject baseObject;
            nCount = 0;
            Bo2C = false;
            var mapCell = false;
            MapCellinfo mapCellInfo = GetMapCellInfo(nX, nY, ref mapCell);
            if (mapCell && mapCellInfo.Valid)
            {
                Bo2C = true;
                if (mapCellInfo.ObjList != null)
                {
                    for (var i = 0; i < mapCellInfo.Count; i++)
                    {
                        osObject = mapCellInfo.ObjList[i];
                        if (osObject.CellType == CellType.OS_ITEMOBJECT)
                        {
                            result = osObject.CellObj;
                            nCount++;
                        }
                        if (osObject.CellType == CellType.OS_GATEOBJECT)
                        {
                            Bo2C = false;
                        }
                        if (osObject.CellType == CellType.OS_MOVINGOBJECT)
                        {
                            baseObject = (TBaseObject)osObject.CellObj;
                            if (!baseObject.m_boDeath)
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
            TDoorInfo door;
            TDoorInfo result = null;
            for (var i = 0; i < MDoorList.Count; i++)
            {
                door = MDoorList[i];
                if (door.nX == nX && door.nY == nY)
                {
                    result = door;
                    return result;
                }
            }
            return result;
        }

        public bool IsValidObject(int nX, int nY, int nRage, object baseObject)
        {
            MapCellinfo mapCellInfo;
            CellObject osObject;
            var result = false;
            for (var nXx = nX - nRage; nXx <= nX + nRage; nXx++)
            {
                for (var nYy = nY - nRage; nYy <= nY + nRage; nYy++)
                {
                    var mapCell = false;
                    mapCellInfo = GetMapCellInfo(nX, nY, ref mapCell);
                    if (mapCell && mapCellInfo.ObjList != null)
                    {
                        for (var i = 0; i < mapCellInfo.Count; i++)
                        {
                            osObject = mapCellInfo.ObjList[i];
                            if (osObject.CellObj == baseObject)
                            {
                                result = true;
                                return result;
                            }
                        }
                    }
                }
            }
            return result;
        }

        public int GetRangeBaseObject(int nX, int nY, int nRage, bool boFlag, IList<TBaseObject> baseObjectList)
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

        public bool GetMapBaseObjects(short nX, short nY, int nRage, IList<TBaseObject> baseObjectList, CellType btType = CellType.OS_MOVINGOBJECT)
        {
            if (baseObjectList.Count == 0)
            {
                return false;
            }
            var nStartX = nX - nRage;
            var nEndX = nX + nRage;
            var nStartY = nY - nRage;
            var nEndY = nY + nRage;
            Console.WriteLine("todo GetMapBaseObjects");
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
        public int GetBaseObjects(int nX, int nY, bool boFlag, IList<TBaseObject> baseObjectList)
        {
            int result;
            CellObject osObject;
            TBaseObject baseObject;
            var mapCell = false;
            MapCellinfo mapCellInfo = GetMapCellInfo(nX, nY, ref mapCell);
            if (mapCell && mapCellInfo.ObjList != null)
            {
                for (var i = 0; i < mapCellInfo.Count; i++)
                {
                    osObject = mapCellInfo.ObjList[i];
                    if (osObject.CellType == CellType.OS_MOVINGOBJECT)
                    {
                        baseObject = (TBaseObject)osObject.CellObj;
                        if (baseObject != null)
                        {
                            if (!baseObject.m_boGhost && baseObject.bo2B9)
                            {
                                if (!boFlag || !baseObject.m_boDeath)
                                {
                                    baseObjectList.Add(baseObject);
                                }
                            }
                        }
                    }
                }
            }
            result = baseObjectList.Count;
            return result;
        }

        public object GetEvent(int nX, int nY)
        {
            CellObject osObject;
            object result = null;
            Bo2C = false;
            var mapCell = false;
            MapCellinfo mapCellInfo = GetMapCellInfo(nX, nY, ref mapCell);
            if (mapCell && mapCellInfo.ObjList != null)
            {
                for (var i = 0; i < mapCellInfo.Count; i++)
                {
                    osObject = mapCellInfo.ObjList[i];
                    if (osObject.CellType == CellType.OS_EVENTOBJECT)
                    {
                        result = osObject.CellObj;
                    }
                }
            }
            return result;
        }

        public void SetMapXyFlag(int nX, int nY, bool boFlag)
        {
            var mapcell = false;
            MapCellinfo mapCellInfo = GetMapCellInfo(nX, nY, ref mapcell);
            if (mapcell)
            {
                if (boFlag)
                {
                    mapCellInfo.Attribute = CellAttribute.Walk;
                }
                else
                {
                    mapCellInfo.Attribute = CellAttribute.LowWall;
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

        public bool GetXyHuman(int nMapX, int nMapY)
        {
            var mapCell = false;
            CellObject osObject;
            TBaseObject baseObject;
            var result = false;
            MapCellinfo mapCellInfo = GetMapCellInfo(nMapX, nMapY, ref mapCell);
            if (mapCell && mapCellInfo.ObjList != null)
            {
                for (var i = 0; i < mapCellInfo.Count; i++)
                {
                    osObject = mapCellInfo.ObjList[i];
                    if (osObject.CellType == CellType.OS_MOVINGOBJECT)
                    {
                        baseObject = (TBaseObject)osObject.CellObj;
                        if (baseObject.m_btRaceServer == Grobal2.RC_PLAYOBJECT)
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
            var mapCell = false;
            MapCellinfo mapCellInfo = GetMapCellInfo(nX, nY, ref mapCell);
            if (mapCell && mapCellInfo.Attribute == CellAttribute.LowWall)
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
            var result = string.Format(sMsg, SMapName, SMapDesc, HUtil32.BoolToStr(Flag.boDayLight), HUtil32.BoolToStr(Flag.boDarkness), HUtil32.BoolToStr(Flag.boSAFE), HUtil32.BoolToStr(Flag.boFightZone),
                HUtil32.BoolToStr(Flag.boFight3Zone), HUtil32.BoolToStr(Flag.boQUIZ), HUtil32.BoolToStr(Flag.boNORECONNECT), Flag.sNoReConnectMap, HUtil32.BoolToStr(Flag.boMUSIC), Flag.nMUSICID, HUtil32.BoolToStr(Flag.boEXPRATE),
                Flag.nEXPRATE / 100, HUtil32.BoolToStr(Flag.boPKWINLEVEL), Flag.nPKWINLEVEL, HUtil32.BoolToStr(Flag.boPKLOSTLEVEL), Flag.nPKLOSTLEVEL, HUtil32.BoolToStr(Flag.boPKWINEXP), Flag.nPKWINEXP, HUtil32.BoolToStr(Flag.boPKLOSTEXP),
                Flag.nPKLOSTEXP, HUtil32.BoolToStr(Flag.boDECHP), Flag.nDECHPTIME, Flag.nDECHPPOINT, HUtil32.BoolToStr(Flag.boINCHP), Flag.nINCHPTIME, Flag.nINCHPPOINT, HUtil32.BoolToStr(Flag.boDECGAMEGOLD), Flag.nDECGAMEGOLDTIME,
                Flag.nDECGAMEGOLD, HUtil32.BoolToStr(Flag.boINCGAMEGOLD), Flag.nINCGAMEGOLDTIME, Flag.nINCGAMEGOLD, HUtil32.BoolToStr(Flag.boINCGAMEPOINT), Flag.nINCGAMEPOINTTIME, Flag.nINCGAMEPOINT, HUtil32.BoolToStr(Flag.boRUNHUMAN),
                HUtil32.BoolToStr(Flag.boRUNMON), HUtil32.BoolToStr(Flag.boNEEDHOLE), HUtil32.BoolToStr(Flag.boNORECALL), HUtil32.BoolToStr(Flag.boNOGUILDRECALL), HUtil32.BoolToStr(Flag.boNODEARRECALL), HUtil32.BoolToStr(Flag.boNOMASTERRECALL),
                HUtil32.BoolToStr(Flag.boNODRUG), HUtil32.BoolToStr(Flag.boMINE), HUtil32.BoolToStr(Flag.boMINE2), HUtil32.BoolToStr(Flag.boNODROPITEM), HUtil32.BoolToStr(Flag.boNOTHROWITEM), HUtil32.BoolToStr(Flag.boNOPOSITIONMOVE),
                HUtil32.BoolToStr(Flag.boNOHORSE), HUtil32.BoolToStr(Flag.boNOHUMNOMON), HUtil32.BoolToStr(Flag.boNOCHAT));
            return result;
        }

        public void AddObject(object baseObject)
        {
            var btRaceServer = ((TBaseObject)baseObject).m_btRaceServer;
            if (btRaceServer == Grobal2.RC_PLAYOBJECT)
            {
                _mNHumCount++;
            }
            if (btRaceServer >= Grobal2.RC_ANIMAL)
            {
                _mNMonCount++;
            }
        }

        public void DelObjectCount(object baseObject)
        {
            var btRaceServer = ((TBaseObject)baseObject).m_btRaceServer;
            if (btRaceServer == Grobal2.RC_PLAYOBJECT)
            {
                _mNHumCount--;
            }
            if (btRaceServer >= Grobal2.RC_ANIMAL)
            {
                _mNMonCount--;
            }
        }
    }
}

