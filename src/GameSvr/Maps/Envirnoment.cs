using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using SystemModule;
using SystemModule.Common;

namespace GameSvr
{
    public class Envirnoment
    {
        /// <summary>
        /// 怪物数量
        /// </summary>
        public int MonCount => m_nMonCount;
        /// <summary>
        /// 玩家数量
        /// </summary>
        public int HumCount => m_nHumCount;
        public short wWidth;
        public short wHeight;
        public string m_sMapFileName = string.Empty;
        public string sMapName = string.Empty;
        public string sMapDesc = string.Empty;
        private MapCellinfo[] MapCellArray;
        public int nMinMap = 0;
        public int nServerIndex = 0;
        /// <summary>
        /// 进入本地图所需等级
        /// </summary>
        public int nRequestLevel = 0;
        public TMapFlag Flag = null;
        public bool bo2C = false;
        /// <summary>
        /// 门
        /// </summary>
        public IList<TDoorInfo> m_DoorList = null;
        public object QuestNPC = null;
        /// <summary>
        /// 任务
        /// </summary>
        public IList<TMapQuestInfo> m_QuestList = null;
        public int m_dwWhisperTick = 0;
        private int m_nMonCount = 0;
        private int m_nHumCount = 0;
        public IList<PointInfo> m_PointList;

        public Envirnoment()
        {
            sMapName = string.Empty;
            nServerIndex = 0;
            nMinMap = 0;
            Flag = new TMapFlag();
            m_nMonCount = 0;
            m_nHumCount = 0;
            m_DoorList = new List<TDoorInfo>();
            m_QuestList = new List<TMapQuestInfo>();
            m_dwWhisperTick = 0;
        }

        ~Envirnoment()
        {
            MapCellArray = null;
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
            MapCellinfo MapCellInfo;
            CellObject OSObject;
            MapItem MapItem;
            int nGoldCount;
            const string sExceptionMsg = "[Exception] TEnvirnoment::AddToMap";
            try
            {
                var bo1E = false;
                var mapCell = false;
                MapCellInfo = GetMapCellInfo(nX, nY, ref mapCell);
                if (mapCell && MapCellInfo.Valid)
                {
                    if (MapCellInfo.ObjList == null)
                    {
                        MapCellInfo.ObjList = new List<CellObject>();
                    }
                    else
                    {
                        if (btType == CellType.OS_ITEMOBJECT)
                        {
                            if (((MapItem)pRemoveObject).Name == Grobal2.sSTRING_GOLDNAME)
                            {
                                for (var i = 0; i < MapCellInfo.Count; i++)
                                {
                                    OSObject = MapCellInfo.ObjList[i];
                                    if (OSObject.CellType == CellType.OS_ITEMOBJECT)
                                    {
                                        MapItem = (MapItem)MapCellInfo.ObjList[i].CellObj;
                                        if (MapItem.Name == Grobal2.sSTRING_GOLDNAME)
                                        {
                                            nGoldCount = MapItem.Count + ((MapItem)pRemoveObject).Count;
                                            if (nGoldCount <= 2000)
                                            {
                                                MapItem.Count = nGoldCount;
                                                MapItem.Looks = M2Share.GetGoldShape(nGoldCount);
                                                MapItem.AniCount = 0;
                                                MapItem.Reserved = 0;
                                                OSObject.dwAddTime = HUtil32.GetTickCount();
                                                result = MapItem;
                                                bo1E = true;
                                            }
                                        }
                                    }
                                }
                            }
                            if (!bo1E && MapCellInfo.Count >= 5)
                            {
                                result = null;
                                bo1E = true;
                            }
                        }
                    }
                    if (!bo1E)
                    {
                        OSObject = new CellObject
                        {
                            CellType = btType,
                            CellObj = pRemoveObject,
                            dwAddTime = HUtil32.GetTickCount()
                        };
                        MapCellInfo.Add(OSObject);
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
            TDoorInfo Door;
            for (var i = 0; i < m_DoorList.Count; i++)
            {
                Door = m_DoorList[i];
                AddToMap(Door.nX, Door.nY, CellType.OS_DOOR, Door);
            }
        }

        public bool GetMapCellInfo(int nX, int nY, ref MapCellinfo MapCellInfo)
        {
            bool result;
            if (nX >= 0 && nX < wWidth && nY >= 0 && nY < wHeight)
            {
                MapCellInfo = MapCellArray[nX * wHeight + nY];
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
            if (nX >= 0 && nX < wWidth && nY >= 0 && nY < wHeight)
            {
                MapCellinfo mapCell = MapCellArray[nX * wHeight + nY];
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

        public int MoveToMovingObject(int nCX, int nCY, TBaseObject Cert, int nX, int nY, bool boFlag)
        {
            MapCellinfo MapCellInfo;
            bool mapCell = false;
            TBaseObject BaseObject;
            CellObject OSObject;
            bool bo1A;
            const string sExceptionMsg = "[Exception] TEnvirnoment::MoveToMovingObject";
            var result = 0;
            try
            {
                bo1A = true;
                MapCellInfo = GetMapCellInfo(nX, nY, ref mapCell);
                if (!boFlag && mapCell)
                {
                    if (MapCellInfo.Valid)
                    {
                        if (MapCellInfo.ObjList != null)
                        {
                            for (var i = 0; i < MapCellInfo.Count; i++)
                            {
                                if (MapCellInfo.ObjList[i].CellType == CellType.OS_MOVINGOBJECT)
                                {
                                    BaseObject = (TBaseObject)MapCellInfo.ObjList[i].CellObj;
                                    if (BaseObject != null)
                                    {
                                        if (!BaseObject.m_boGhost && BaseObject.bo2B9 && !BaseObject.m_boDeath && !BaseObject.m_boFixedHideMode && !BaseObject.m_boObMode)
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
                    if (GetMapCellInfo(nX, nY, ref MapCellInfo) && MapCellInfo.Attribute != 0)
                    {
                        result = -1;
                    }
                    else
                    {
                        if (GetMapCellInfo(nCX, nCY, ref MapCellInfo) && MapCellInfo.ObjList != null)
                        {
                            var i = 0;
                            while (true)
                            {
                                if (MapCellInfo.Count <= i)
                                {
                                    break;
                                }
                                OSObject = MapCellInfo.ObjList[i];
                                if (OSObject.CellType == CellType.OS_MOVINGOBJECT)
                                {
                                    if ((TBaseObject)OSObject.CellObj == Cert)
                                    {
                                        MapCellInfo.Remove(i);
                                        OSObject = null;
                                        if (MapCellInfo.Count > 0)
                                        {
                                            continue;
                                        }
                                        MapCellInfo.Dispose();
                                        break;
                                    }
                                }
                                i++;
                            }
                        }
                        if (GetMapCellInfo(nX, nY, ref MapCellInfo))
                        {
                            if (MapCellInfo.ObjList == null)
                            {
                                MapCellInfo.ObjList = new List<CellObject>();
                            }
                            OSObject = new CellObject
                            {
                                CellType = CellType.OS_MOVINGOBJECT,
                                CellObj = Cert,
                                dwAddTime = HUtil32.GetTickCount()
                            };
                            MapCellInfo.Add(OSObject);
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
            CellObject OSObject;
            TBaseObject BaseObject;
            var result = false;
            var mapCell = false;
            MapCellinfo MapCellInfo = GetMapCellInfo(nX, nY, ref mapCell);
            if (mapCell && MapCellInfo.Valid)
            {
                result = true;
                if (!boFlag && MapCellInfo.ObjList != null)
                {
                    for (var i = 0; i < MapCellInfo.Count; i++)
                    {
                        OSObject = MapCellInfo.ObjList[i];
                        if (OSObject.CellType == CellType.OS_MOVINGOBJECT)
                        {
                            BaseObject = (TBaseObject)OSObject.CellObj;
                            if (BaseObject != null)
                            {
                                if (!BaseObject.m_boGhost && BaseObject.bo2B9 && !BaseObject.m_boDeath && !BaseObject.m_boFixedHideMode && !BaseObject.m_boObMode)
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
            CellObject OSObject;
            TBaseObject BaseObject;
            var result = true;
            MapCellinfo MapCellInfo = GetMapCellInfo(nX, nY, ref mapCell);
            if (mapCell && MapCellInfo.Valid)
            {
                if (MapCellInfo.ObjList != null)
                {
                    for (var i = 0; i < MapCellInfo.Count; i++)
                    {
                        OSObject = MapCellInfo.ObjList[i];
                        if (!boFlag && OSObject.CellType == CellType.OS_MOVINGOBJECT)
                        {
                            BaseObject = (TBaseObject)OSObject.CellObj;
                            if (BaseObject != null)
                            {
                                if (!BaseObject.m_boGhost && BaseObject.bo2B9 && !BaseObject.m_boDeath && !BaseObject.m_boFixedHideMode && !BaseObject.m_boObMode)
                                {
                                    result = false;
                                    break;
                                }
                            }
                        }
                        if (!boItem && OSObject.CellType == CellType.OS_ITEMOBJECT)
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
            CellObject OSObject;
            TBaseObject BaseObject;
            TUserCastle Castle;
            var result = false;
            var mapCell = false;
            MapCellinfo MapCellInfo = GetMapCellInfo(nX, nY, ref mapCell);
            if (mapCell && MapCellInfo.Valid)
            {
                result = true;
                if (!boFlag && MapCellInfo.ObjList != null)
                {
                    for (var i = 0; i < MapCellInfo.Count; i++)
                    {
                        OSObject = MapCellInfo.ObjList[i];
                        if (OSObject.CellType == CellType.OS_MOVINGOBJECT)
                        {
                            BaseObject = (TBaseObject)OSObject.CellObj;
                            if (BaseObject != null)
                            {
                                Castle = M2Share.CastleManager.InCastleWarArea(BaseObject);
                                if (M2Share.g_Config.boWarDisHumRun && Castle != null && Castle.m_boUnderWar)
                                {
                                }
                                else
                                {
                                    if (BaseObject.m_btRaceServer == Grobal2.RC_PLAYOBJECT)
                                    {
                                        if (M2Share.g_Config.boRunHuman || Flag.boRUNHUMAN)
                                        {
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        if (BaseObject.m_btRaceServer == Grobal2.RC_NPC)
                                        {
                                            if (M2Share.g_Config.boRunNpc)
                                            {
                                                continue;
                                            }
                                        }
                                        else
                                        {
                                            if(BaseObject.m_btRaceServer==Grobal2.RC_GUARD || BaseObject.m_btRaceServer== Grobal2.RC_ARCHERGUARD)
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
                                if (!BaseObject.m_boGhost && BaseObject.bo2B9 && !BaseObject.m_boDeath && !BaseObject.m_boFixedHideMode && !BaseObject.m_boObMode)
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
            CellObject OSObject;
            int n18;
            const string sExceptionMsg1 = "[Exception] TEnvirnoment::DeleteFromMap -> Except 1 ** %d";
            const string sExceptionMsg2 = "[Exception] TEnvirnoment::DeleteFromMap -> Except 2 ** %d";
            var result = -1;
            var mapCell = false;
            try
            {
                MapCellinfo MapCellInfo = GetMapCellInfo(nX, nY, ref mapCell);
                if (mapCell)
                {
                    if (mapCell)
                    {
                        try
                        {
                            if (MapCellInfo.ObjList != null)
                            {
                                n18 = 0;
                                while (true)
                                {
                                    if (MapCellInfo.Count <= n18)
                                    {
                                        break;
                                    }
                                    OSObject = MapCellInfo.ObjList[n18];
                                    if (OSObject != null)
                                    {
                                        if (OSObject.CellType == cellType && OSObject.CellObj == pRemoveObject)
                                        {
                                            MapCellInfo.Remove(n18);
                                            OSObject = null;
                                            result = 1;
                                            // 减地图人物怪物计数
                                            if (cellType == CellType.OS_MOVINGOBJECT && !((TBaseObject)pRemoveObject).m_boDelFormMaped)
                                            {
                                                ((TBaseObject)pRemoveObject).m_boDelFormMaped = true;
                                                ((TBaseObject)pRemoveObject).m_boAddToMaped = false;
                                                DelObjectCount(pRemoveObject);
                                            }
                                            if (MapCellInfo.Count > 0)
                                            {
                                                continue;
                                            }
                                            MapCellInfo.Dispose();
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        MapCellInfo.Remove(n18);
                                        if (MapCellInfo.Count > 0)
                                        {
                                            continue;
                                        }
                                        MapCellInfo.Dispose();
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
                            OSObject = null;
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
            CellObject OSObject;
            TBaseObject BaseObject;
            MapItem result = null;
            bo2C = false;
            var mapCell = false;
            MapCellinfo MapCellInfo = GetMapCellInfo(nX, nY, ref mapCell);
            if (mapCell && MapCellInfo.Valid)
            {
                bo2C = true;
                if (MapCellInfo.ObjList != null)
                {
                    for (var i = 0; i < MapCellInfo.Count; i++)
                    {
                        OSObject = MapCellInfo.ObjList[i];
                        if (OSObject.CellType == CellType.OS_ITEMOBJECT)
                        {
                            result = (MapItem)OSObject.CellObj;
                            return result;
                        }
                        if (OSObject.CellType == CellType.OS_GATEOBJECT)
                        {
                            bo2C = false;
                        }
                        if (OSObject.CellType == CellType.OS_MOVINGOBJECT)
                        {
                            BaseObject = (TBaseObject)OSObject.CellObj;
                            if (!BaseObject.m_boDeath)
                            {
                                bo2C = false;
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
            if (m_QuestList.Count > 0)
            {
                result = true;
            }
            else
            {
                result = false;
            }
            return result;
        }

        public object AddToMapItemEvent(int nX, int nY, CellType nType, object __Event)
        {
            object result = null;
            var mapCell = false;
            MapCellinfo MapCellInfo = GetMapCellInfo(nX, nY, ref mapCell);
            if (mapCell && MapCellInfo.Valid)
            {
                if (MapCellInfo.ObjList == null)
                {
                    MapCellInfo.ObjList = new List<CellObject>();
                }
                if (nType == CellType.OS_EVENTOBJECT)
                {
                    var OSObject = new CellObject();
                    OSObject.CellType = nType;
                    OSObject.CellObj = __Event;
                    OSObject.dwAddTime = HUtil32.GetTickCount();
                    MapCellInfo.Add(OSObject);
                    result = OSObject;
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
            MapCellinfo Mc = new MapCellinfo();
            const string sExceptionMsg = "[Exception] TEnvirnoment::AddToMapMineEvent ";
            var mapCell = false;
            try
            {
                MapCellinfo MapCellInfo = GetMapCellInfo(nX, nY, ref mapCell);
                var bo1A = false;
                if (mapCell && MapCellInfo.Attribute != 0)
                {
                    var isSpace = false;// 人物可以走到的地方才放上矿藏
                    for (var X = nX - 1; X <= nX + 1; X ++ )
                    {
                        for (var Y = nY - 1; Y <= nY + 1; Y ++ )
                        {
                            if (GetMapCellInfo(X, Y, ref Mc))
                            {
                                if ((Mc.Valid))
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
                        if (MapCellInfo.ObjList == null)
                        {
                            MapCellInfo.ObjList = new List<CellObject>();
                        }
                        if (!bo1A)
                        {
                            var OSObject = new CellObject
                            {
                                CellType = nType,
                                CellObj = stoneMineEvent,
                                dwAddTime = HUtil32.GetTickCount()
                            };
                            MapCellInfo.Add(OSObject);
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
        /// <param name="BaseObject"></param>
        public void VerifyMapTime(int nX, int nY, object BaseObject)
        {
            CellObject OSObject;
            bool boVerify;
            var mapCell = false;
            const string sExceptionMsg = "[Exception] TEnvirnoment::VerifyMapTime";
            try
            {
                boVerify = false;
                MapCellinfo MapCellInfo = GetMapCellInfo(nX, nY, ref mapCell);
                if (mapCell && MapCellInfo.ObjList != null)
                {
                    for (var i = 0; i < MapCellInfo.Count; i++)
                    {
                        OSObject = MapCellInfo.ObjList[i];
                        if (OSObject.CellType == CellType.OS_MOVINGOBJECT && OSObject.CellObj == BaseObject)
                        {
                            OSObject.dwAddTime = HUtil32.GetTickCount();
                            boVerify = true;
                            break;
                        }
                    }
                }
                if (!boVerify)
                {
                    AddToMap(nX, nY, CellType.OS_MOVINGOBJECT, BaseObject);
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
            int Point;
            TDoorInfo Door;
            var muiSize = 12;//固定大小
            try
            {
                if (File.Exists(sMapFile))
                {
                    using var fileStream = new FileStream(sMapFile, FileMode.Open, FileAccess.Read);
                    using var binReader = new BinaryReader(fileStream);

                    var bytData = new byte[52];
                    binReader.Read(bytData, 0, bytData.Length);
                    wWidth = BitConverter.ToInt16(bytData, 0);
                    wHeight = BitConverter.ToInt16(bytData, 2);

                    Initialize(wWidth, wHeight);

                    var nMapSize = wWidth * muiSize * wHeight;
                    buffer = new byte[nMapSize];
                    binReader.Read(buffer, 0, nMapSize);
                    var buffIndex = 0;

                    for (var nW = 0; nW < wWidth; nW++)
                    {
                        n24 = nW * wHeight;
                        for (var nH = 0; nH < wHeight; nH++)
                        {
                            // wBkImg High
                            if ((buffer[buffIndex + 1] & 0x80) != 0)
                            {
                                MapCellArray[n24 + nH] = MapCellinfo.HighWall;
                            }
                            // wFrImg High
                            if ((buffer[buffIndex + 5] & 0x80) != 0)
                            {
                                MapCellArray[n24 + nH] = MapCellinfo.LowWall;
                            }
                            if (MapCellArray[n24 + nH] == null)
                            {
                                MapCellArray[n24 + nH] = new MapCellinfo();
                            }
                            // btDoorIndex
                            if ((buffer[buffIndex + 6] & 0x80) != 0)
                            {
                                Point = buffer[buffIndex + 6] & 0x7F;
                                if (Point > 0)
                                {
                                    Door = new TDoorInfo
                                    {
                                        nX = nW,
                                        nY = nH,
                                        n08 = Point,
                                        Status = null
                                    };
                                    for (var i = 0; i < m_DoorList.Count; i++)
                                    {
                                        if (Math.Abs(m_DoorList[i].nX - Door.nX) <= 10)
                                        {
                                            if (Math.Abs(m_DoorList[i].nY - Door.nY) <= 10)
                                            {
                                                if (m_DoorList[i].n08 == Point)
                                                {
                                                    Door.Status = m_DoorList[i].Status;
                                                    Door.Status.nRefCount++;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    if (Door.Status == null)
                                    {
                                        Door.Status = new TDoorStatus
                                        {
                                            boOpened = false,
                                            bo01 = false,
                                            n04 = 0,
                                            dwOpenTick = 0,
                                            nRefCount = 1
                                        };
                                    }
                                    m_DoorList.Add(Door);
                                }
                            }
                            buffIndex += muiSize;
                        }
                    }
                    binReader.Close();
                    binReader.Dispose();
                    fileStream.Close();
                    fileStream.Dispose();
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
                        if (nX >= 0 && nY >= 0 && nX < wWidth && nY < wHeight)
                        {
                            m_PointList.Add(new PointInfo(nX,nY));
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                M2Share.MainOutMessage("[Exception] TEnvirnoment.LoadMapData");
            }
            return result;
        }

        private void Initialize(short nWidth, short nHeight)
        {
            if (nWidth > 1 && nHeight > 1)
            {
                if (MapCellArray != null)
                {
                    for (var nW = 0; nW < wWidth; nW++)
                    {
                        for (var nH = 0; nH < wHeight; nH++)
                        {
                            if (MapCellArray[nW * wHeight + nH].ObjList != null)
                            {
                                MapCellArray[nW * wHeight + nH].Dispose();
                            }
                        }
                    }
                    MapCellArray = null;
                }
                wWidth = nWidth;
                wHeight = nHeight;
                MapCellArray = new MapCellinfo[nWidth * nHeight];
            }
        }

        public bool CreateQuest(int nFlag, int nValue, string sMonName, string sItem, string sQuest, bool boGrouped)
        {
            TMapQuestInfo MapQuest;
            Merchant MapMerchant;
            var result = false;
            if (nFlag < 0)
            {
                return result;
            }
            MapQuest = new TMapQuestInfo
            {
                nFlag = nFlag
            };
            if (nValue > 1)
            {
                nValue = 1;
            }
            MapQuest.nValue = nValue;
            if (sMonName == "*")
            {
                sMonName = "";
            }
            MapQuest.sMonName = sMonName;
            if (sItem == "*")
            {
                sItem = "";
            }
            MapQuest.sItemName = sItem;
            if (sQuest == "*")
            {
                sQuest = "";
            }
            MapQuest.boGrouped = boGrouped;
            MapMerchant = new Merchant
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
            M2Share.UserEngine.QuestNPCList.Add(MapMerchant);
            MapQuest.NPC = MapMerchant;
            m_QuestList.Add(MapQuest);
            result = true;
            return result;
        }

        public int GetXYObjCount(int nX, int nY)
        {
            var result = 0;
            CellObject OSObject;
            TBaseObject BaseObject;
            var mapCell = false;
            MapCellinfo MapCellInfo = GetMapCellInfo(nX, nY, ref mapCell);
            if (mapCell && MapCellInfo.ObjList != null)
            {
                for (var i = 0; i < MapCellInfo.Count; i++)
                {
                    OSObject = MapCellInfo.ObjList[i];
                    if (OSObject.CellType == CellType.OS_MOVINGOBJECT)
                    {
                        BaseObject = (TBaseObject)OSObject.CellObj;
                        if (BaseObject != null)
                        {
                            if (!BaseObject.m_boGhost && BaseObject.bo2B9 && !BaseObject.m_boDeath && !BaseObject.m_boFixedHideMode && !BaseObject.m_boObMode)
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
                    if (sny < wWidth - nFlag)
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
                    if (snx < wWidth - nFlag)
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
                    if (snx > nFlag - 1 && sny < wHeight - nFlag)
                    {
                        snx += (short)nFlag;
                        sny -= (short)nFlag;
                    }
                    break;
                case Grobal2.DR_DOWNLEFT:
                    if (snx < wWidth - nFlag && sny > nFlag - 1)
                    {
                        snx -= (short)nFlag;
                        sny += (short)nFlag;
                    }
                    break;
                case Grobal2.DR_DOWNRIGHT:
                    if (snx < wWidth - nFlag && sny < wHeight - nFlag)
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
            CellObject OSObject;
            var mapCell = false;
            MapCellinfo MapCellInfo = GetMapCellInfo(nX, nY, ref mapCell);
            if (mapCell && MapCellInfo.ObjList != null)
            {
                for (var i = MapCellInfo.Count - 1; i >= 0; i--)
                {
                    OSObject = MapCellInfo.ObjList[i];
                    if (OSObject.CellType == CellType.OS_EVENTOBJECT)
                    {
                        if (((Event)OSObject.CellObj).m_nDamage > 0)
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
            TDoorInfo Door;
            for (var i = 0; i < m_DoorList.Count; i++)
            {
                Door = m_DoorList[i];
                if (Math.Abs(Door.nX - nX) <= 1 && Math.Abs(Door.nY - nY) <= 1)
                {
                    if (!Door.Status.boOpened)
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
            CellObject OSObject;
            TBaseObject BaseObject;
            var mapCell = false;
            MapCellinfo MapCellInfo = GetMapCellInfo(nX, nY, ref mapCell);
            if (mapCell && MapCellInfo.ObjList != null)
            {
                for (var i = 0; i < MapCellInfo.Count; i++)
                {
                    OSObject = MapCellInfo.ObjList[i];
                    if (OSObject.CellType == CellType.OS_MOVINGOBJECT)
                    {
                        BaseObject = (TBaseObject)OSObject.CellObj;
                        if (BaseObject != null && !BaseObject.m_boGhost && BaseObject.bo2B9 && (!boFlag || !BaseObject.m_boDeath))
                        {
                            result = BaseObject;
                            break;
                        }
                    }
                }
            }
            return result;
        }

        public object GetQuestNPC(TBaseObject BaseObject, string sCharName, string sItem, bool boFlag)
        {
            object result = null;
            TMapQuestInfo MapQuestFlag;
            int nFlagValue;
            bool bo1D;
            for (var i = 0; i < m_QuestList.Count; i++)
            {
                MapQuestFlag = m_QuestList[i];
                nFlagValue = BaseObject.GetQuestFalgStatus(MapQuestFlag.nFlag);
                if (nFlagValue == MapQuestFlag.nValue)
                {
                    if (boFlag == MapQuestFlag.boGrouped || !boFlag)
                    {
                        bo1D = false;
                        if (!string.IsNullOrEmpty(MapQuestFlag.sMonName) && !string.IsNullOrEmpty(MapQuestFlag.sItemName))
                        {
                            if (MapQuestFlag.sMonName == sCharName && MapQuestFlag.sItemName == sItem)
                            {
                                bo1D = true;
                            }
                        }
                        if (!string.IsNullOrEmpty(MapQuestFlag.sMonName) && string.IsNullOrEmpty(MapQuestFlag.sItemName))
                        {
                            if (MapQuestFlag.sMonName == sCharName && string.IsNullOrEmpty(sItem))
                            {
                                bo1D = true;
                            }
                        }
                        if (string.IsNullOrEmpty(MapQuestFlag.sMonName) && !string.IsNullOrEmpty(MapQuestFlag.sItemName))
                        {
                            if (MapQuestFlag.sItemName == sItem)
                            {
                                bo1D = true;
                            }
                        }
                        if (bo1D)
                        {
                            result = MapQuestFlag.NPC;
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
            CellObject OSObject;
            TBaseObject BaseObject;
            nCount = 0;
            bo2C = false;
            var mapCell = false;
            MapCellinfo MapCellInfo = GetMapCellInfo(nX, nY, ref mapCell);
            if (mapCell && MapCellInfo.Valid)
            {
                bo2C = true;
                if (MapCellInfo.ObjList != null)
                {
                    for (var i = 0; i < MapCellInfo.Count; i++)
                    {
                        OSObject = MapCellInfo.ObjList[i];
                        if (OSObject.CellType == CellType.OS_ITEMOBJECT)
                        {
                            result = OSObject.CellObj;
                            nCount++;
                        }
                        if (OSObject.CellType == CellType.OS_GATEOBJECT)
                        {
                            bo2C = false;
                        }
                        if (OSObject.CellType == CellType.OS_MOVINGOBJECT)
                        {
                            BaseObject = (TBaseObject)OSObject.CellObj;
                            if (!BaseObject.m_boDeath)
                            {
                                bo2C = false;
                            }
                        }
                    }
                }
            }
            return result;
        }

        public TDoorInfo GetDoor(int nX, int nY)
        {
            TDoorInfo Door;
            TDoorInfo result = null;
            for (var i = 0; i < m_DoorList.Count; i++)
            {
                Door = m_DoorList[i];
                if (Door.nX == nX && Door.nY == nY)
                {
                    result = Door;
                    return result;
                }
            }
            return result;
        }

        public bool IsValidObject(int nX, int nY, int nRage, object BaseObject)
        {
            MapCellinfo MapCellInfo;
            CellObject OSObject;
            var result = false;
            for (var nXX = nX - nRage; nXX <= nX + nRage; nXX++)
            {
                for (var nYY = nY - nRage; nYY <= nY + nRage; nYY++)
                {
                    var mapCell = false;
                    MapCellInfo = GetMapCellInfo(nX, nY, ref mapCell);
                    if (mapCell && MapCellInfo.ObjList != null)
                    {
                        for (var i = 0; i < MapCellInfo.Count; i++)
                        {
                            OSObject = MapCellInfo.ObjList[i];
                            if (OSObject.CellObj == BaseObject)
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

        public int GetRangeBaseObject(int nX, int nY, int nRage, bool boFlag, IList<TBaseObject> BaseObjectList)
        {
            for (var nXX = nX - nRage; nXX <= nX + nRage; nXX++)
            {
                for (var nYY = nY - nRage; nYY <= nY + nRage; nYY++)
                {
                    GetBaseObjects(nXX, nYY, boFlag, BaseObjectList);
                }
            }
            return BaseObjectList.Count;
        }

        public bool GetMapBaseObjects(short nX,short nY,int nRage, IList<TBaseObject> BaseObjectList, CellType btType = CellType.OS_MOVINGOBJECT)
        {
            if (BaseObjectList.Count == 0)
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
        /// <param name="BaseObjectList"></param>
        /// <returns></returns>
        public int GetBaseObjects(int nX, int nY, bool boFlag, IList<TBaseObject> BaseObjectList)
        {
            int result;
            CellObject OSObject;
            TBaseObject BaseObject;
            var mapCell = false;
            MapCellinfo MapCellInfo = GetMapCellInfo(nX, nY, ref mapCell);
            if (mapCell && MapCellInfo.ObjList != null)
            {
                for (var i = 0; i < MapCellInfo.Count; i++)
                {
                    OSObject = MapCellInfo.ObjList[i];
                    if (OSObject.CellType == CellType.OS_MOVINGOBJECT)
                    {
                        BaseObject = (TBaseObject)OSObject.CellObj;
                        if (BaseObject != null)
                        {
                            if (!BaseObject.m_boGhost && BaseObject.bo2B9)
                            {
                                if (!boFlag || !BaseObject.m_boDeath)
                                {
                                    BaseObjectList.Add(BaseObject);
                                }
                            }
                        }
                    }
                }
            }
            result = BaseObjectList.Count;
            return result;
        }

        public object GetEvent(int nX, int nY)
        {
            CellObject OSObject;
            object result = null;
            bo2C = false;
            var mapCell = false;
            MapCellinfo MapCellInfo = GetMapCellInfo(nX, nY, ref mapCell);
            if (mapCell && MapCellInfo.ObjList != null)
            {
                for (var i = 0; i < MapCellInfo.Count; i++)
                {
                    OSObject = MapCellInfo.ObjList[i];
                    if (OSObject.CellType == CellType.OS_EVENTOBJECT)
                    {
                        result = OSObject.CellObj;
                    }
                }
            }
            return result;
        }

        public void SetMapXYFlag(int nX, int nY, bool boFlag)
        {
            var mapcell = false;
            MapCellinfo MapCellInfo = GetMapCellInfo(nX, nY, ref mapcell);
            if (mapcell)
            {
                if (boFlag)
                {
                    MapCellInfo.Attribute = CellAttribute.Walk;
                }
                else
                {
                    MapCellInfo.Attribute = CellAttribute.LowWall;
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

        public bool GetXYHuman(int nMapX, int nMapY)
        {
            var mapCell = false;
            CellObject OSObject;
            TBaseObject BaseObject;
            var result = false;
            MapCellinfo MapCellInfo = GetMapCellInfo(nMapX, nMapY, ref mapCell);
            if (mapCell && MapCellInfo.ObjList != null)
            {
                for (var i = 0; i < MapCellInfo.Count; i++)
                {
                    OSObject = MapCellInfo.ObjList[i];
                    if (OSObject.CellType == CellType.OS_MOVINGOBJECT)
                    {
                        BaseObject = (TBaseObject)OSObject.CellObj;
                        if (BaseObject.m_btRaceServer == Grobal2.RC_PLAYOBJECT)
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
            MapCellinfo MapCellInfo = GetMapCellInfo(nX, nY, ref mapCell);
            if (mapCell && MapCellInfo.Attribute == CellAttribute.LowWall)
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
            var result = string.Format(sMsg, sMapName, sMapDesc, HUtil32.BoolToStr(Flag.boDayLight), HUtil32.BoolToStr(Flag.boDarkness), HUtil32.BoolToStr(Flag.boSAFE), HUtil32.BoolToStr(Flag.boFightZone), 
                HUtil32.BoolToStr(Flag.boFight3Zone), HUtil32.BoolToStr(Flag.boQUIZ), HUtil32.BoolToStr(Flag.boNORECONNECT), Flag.sNoReConnectMap, HUtil32.BoolToStr(Flag.boMUSIC), Flag.nMUSICID, HUtil32.BoolToStr(Flag.boEXPRATE),
                Flag.nEXPRATE / 100, HUtil32.BoolToStr(Flag.boPKWINLEVEL), Flag.nPKWINLEVEL, HUtil32.BoolToStr(Flag.boPKLOSTLEVEL), Flag.nPKLOSTLEVEL, HUtil32.BoolToStr(Flag.boPKWINEXP), Flag.nPKWINEXP, HUtil32.BoolToStr(Flag.boPKLOSTEXP), 
                Flag.nPKLOSTEXP, HUtil32.BoolToStr(Flag.boDECHP), Flag.nDECHPTIME, Flag.nDECHPPOINT, HUtil32.BoolToStr(Flag.boINCHP), Flag.nINCHPTIME, Flag.nINCHPPOINT, HUtil32.BoolToStr(Flag.boDECGAMEGOLD), Flag.nDECGAMEGOLDTIME, 
                Flag.nDECGAMEGOLD, HUtil32.BoolToStr(Flag.boINCGAMEGOLD), Flag.nINCGAMEGOLDTIME, Flag.nINCGAMEGOLD, HUtil32.BoolToStr(Flag.boINCGAMEPOINT), Flag.nINCGAMEPOINTTIME, Flag.nINCGAMEPOINT, HUtil32.BoolToStr(Flag.boRUNHUMAN), 
                HUtil32.BoolToStr(Flag.boRUNMON), HUtil32.BoolToStr(Flag.boNEEDHOLE), HUtil32.BoolToStr(Flag.boNORECALL), HUtil32.BoolToStr(Flag.boNOGUILDRECALL), HUtil32.BoolToStr(Flag.boNODEARRECALL), HUtil32.BoolToStr(Flag.boNOMASTERRECALL),
                HUtil32.BoolToStr(Flag.boNODRUG), HUtil32.BoolToStr(Flag.boMINE), HUtil32.BoolToStr(Flag.boMINE2), HUtil32.BoolToStr(Flag.boNODROPITEM), HUtil32.BoolToStr(Flag.boNOTHROWITEM), HUtil32.BoolToStr(Flag.boNOPOSITIONMOVE), 
                HUtil32.BoolToStr(Flag.boNOHORSE), HUtil32.BoolToStr(Flag.boNOHUMNOMON), HUtil32.BoolToStr(Flag.boNOCHAT));
            return result;
        }

        public void AddObject(object BaseObject)
        {
            var btRaceServer = ((TBaseObject)BaseObject).m_btRaceServer;
            if (btRaceServer == Grobal2.RC_PLAYOBJECT)
            {
                m_nHumCount++;
            }
            if (btRaceServer >= Grobal2.RC_ANIMAL)
            {
                m_nMonCount++;
            }
        }

        public void DelObjectCount(object BaseObject)
        {
            var btRaceServer = ((TBaseObject)BaseObject).m_btRaceServer;
            if (btRaceServer == Grobal2.RC_PLAYOBJECT)
            {
                m_nHumCount--;
            }
            if (btRaceServer >= Grobal2.RC_ANIMAL)
            {
                m_nMonCount--;
            }
        }
    }
}

