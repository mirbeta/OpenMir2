using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace M2Server
{
    public class TEnvirnoment
    {
        public int MonCount{get{return m_nMonCount;}}
        public int HumCount {get{return m_nHumCount;}}
        public short wWidth;
        public short wHeight;
        public string m_sMapFileName = string.Empty;
        public string sMapName = string.Empty;
        public string sMapDesc = string.Empty;
        public TMapCellinfo[] MapCellArray;
        public int nMinMap = 0;
        public int nServerIndex = 0;
        /// <summary>
        /// 进入本地图所需等级
        /// </summary>
        public int nRequestLevel = 0;
        public TMapFlag Flag = null;
        public bool bo2C = false;
        public IList<TDoorInfo> m_DoorList = null;
        public object QuestNPC = null;
        public IList<TMapQuestInfo> m_QuestList = null;
        public int m_dwWhisperTick = 0;
        private int m_nMonCount = 0;
        private int m_nHumCount = 0;

        public object AddToMap(int nX, int nY, byte btType, object pRemoveObject)
        {
            object result = null;
            TMapCellinfo MapCellInfo = null;
            TOSObject OSObject;
            TMapItem MapItem;
            int nGoldCount;
            const string sExceptionMsg = "[Exception] TEnvirnoment::AddToMap";
            try
            {
                var bo1E = false;
                if (GetMapCellInfo(nX, nY, ref MapCellInfo) && MapCellInfo.chFlag == 0)
                {
                    if (MapCellInfo.ObjList == null)
                    {
                        MapCellInfo.ObjList = new List<TOSObject>();
                    }
                    else
                    {
                        if (btType == grobal2.OS_ITEMOBJECT)
                        {
                            if (((TMapItem)pRemoveObject).Name == grobal2.sSTRING_GOLDNAME)
                            {
                                for (var i = 0; i < MapCellInfo.ObjList.Count; i++)
                                {
                                    OSObject = MapCellInfo.ObjList[i];
                                    if (OSObject.btType == grobal2.OS_ITEMOBJECT)
                                    {
                                        MapItem = (TMapItem)MapCellInfo.ObjList[i].CellObj;
                                        if (MapItem.Name == grobal2.sSTRING_GOLDNAME)
                                        {
                                            nGoldCount = MapItem.Count + ((TMapItem)pRemoveObject).Count;
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
                            if (!bo1E && MapCellInfo.ObjList.Count >= 5)
                            {
                                result = null;
                                bo1E = true;
                            }
                        }
                    }
                    if (!bo1E)
                    {
                        OSObject = new TOSObject
                        {
                            btType = btType,
                            CellObj = pRemoveObject,
                            dwAddTime = HUtil32.GetTickCount()
                        };
                        MapCellInfo.ObjList.Add(OSObject);
                        result = pRemoveObject;
                        if (btType == grobal2.OS_MOVINGOBJECT && !((TBaseObject)pRemoveObject).m_boAddToMaped)
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
                AddToMap(Door.nX, Door.nY, grobal2.OS_DOOR, Door);
            }
        }

        public bool GetMapCellInfo(int nX, int nY, ref TMapCellinfo MapCellInfo)
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

        public int MoveToMovingObject(int nCX, int nCY, TBaseObject Cert, int nX, int nY, bool boFlag)
        {
            TMapCellinfo MapCellInfo = null;
            TBaseObject BaseObject;
            TOSObject OSObject;
            bool bo1A;
            const string sExceptionMsg = "[Exception] TEnvirnoment::MoveToMovingObject";
            var result = 0;
            try
            {
                bo1A = true;
                if (!boFlag && GetMapCellInfo(nX, nY, ref MapCellInfo))
                {
                    if (MapCellInfo.chFlag == 0)
                    {
                        if (MapCellInfo.ObjList != null)
                        {
                            for (var i = 0; i < MapCellInfo.ObjList.Count; i++)
                            {
                                if (MapCellInfo.ObjList[i].btType == grobal2.OS_MOVINGOBJECT)
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
                    if (GetMapCellInfo(nX, nY, ref MapCellInfo) && MapCellInfo.chFlag != 0)
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
                                if (MapCellInfo.ObjList.Count <= i)
                                {
                                    break;
                                }
                                OSObject = MapCellInfo.ObjList[i];
                                if (OSObject.btType == grobal2.OS_MOVINGOBJECT)
                                {
                                    if ((TBaseObject)OSObject.CellObj == Cert)
                                    {
                                        MapCellInfo.ObjList.RemoveAt(i);
                                        OSObject = null;
                                        if (MapCellInfo.ObjList.Count > 0)
                                        {
                                            continue;
                                        }
                                        MapCellInfo.ObjList = null;
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
                                MapCellInfo.ObjList = new List<TOSObject>();
                            }
                            OSObject = new TOSObject
                            {
                                btType = grobal2.OS_MOVINGOBJECT,
                                CellObj = Cert,
                                dwAddTime = HUtil32.GetTickCount()
                            };
                            MapCellInfo.ObjList.Add(OSObject);
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
            TMapCellinfo MapCellInfo = null;
            TOSObject OSObject;
            TBaseObject BaseObject;
            var result = false;
            if (GetMapCellInfo(nX, nY, ref MapCellInfo) && MapCellInfo.chFlag == 0)
            {
                result = true;
                if (!boFlag && MapCellInfo.ObjList != null)
                {
                    for (var i = 0; i < MapCellInfo.ObjList.Count; i++)
                    {
                        OSObject = MapCellInfo.ObjList[i];
                        if (OSObject.btType == grobal2.OS_MOVINGOBJECT)
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
            TMapCellinfo MapCellInfo = null;
            TOSObject OSObject;
            TBaseObject BaseObject;
            var result = true;
            if (GetMapCellInfo(nX, nY, ref MapCellInfo) && MapCellInfo.chFlag == 0)
            {
                if (MapCellInfo.ObjList != null)
                {
                    for (var i = 0; i < MapCellInfo.ObjList.Count; i++)
                    {
                        OSObject = MapCellInfo.ObjList[i];
                        if (!boFlag && OSObject.btType == grobal2.OS_MOVINGOBJECT)
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
                        if (!boItem && OSObject.btType == grobal2.OS_ITEMOBJECT)
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
            TMapCellinfo MapCellInfo = null;
            TOSObject OSObject;
            TBaseObject BaseObject;
            TUserCastle Castle;
            var result = false;
            if (GetMapCellInfo(nX, nY, ref MapCellInfo) && MapCellInfo.chFlag == 0)
            {
                result = true;
                if (!boFlag && MapCellInfo.ObjList != null)
                {
                    for (var i = 0; i < MapCellInfo.ObjList.Count; i++)
                    {
                        OSObject = MapCellInfo.ObjList[i];
                        if (OSObject.btType == grobal2.OS_MOVINGOBJECT)
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
                                    if (BaseObject.m_btRaceServer == grobal2.RC_PLAYOBJECT)
                                    {
                                        if (M2Share.g_Config.boRunHuman || Flag.boRUNHUMAN)
                                        {
                                            continue;
                                        }
                                    }
                                    else
                                    {
                                        if (BaseObject.m_btRaceServer == grobal2.RC_NPC)
                                        {
                                            if (M2Share.g_Config.boRunNpc)
                                            {
                                                continue;
                                            }
                                        }
                                        else
                                        {
                                            if (new ArrayList(new[] { grobal2.RC_GUARD, grobal2.RC_ARCHERGUARD }).Contains(BaseObject.m_btRaceServer))
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

        public int DeleteFromMap(int nX, int nY, byte btType, object pRemoveObject)
        {
            TMapCellinfo MapCellInfo = null;
            TOSObject OSObject;
            int n18;
            const string sExceptionMsg1 = "[Exception] TEnvirnoment::DeleteFromMap -> Except 1 ** %d";
            const string sExceptionMsg2 = "[Exception] TEnvirnoment::DeleteFromMap -> Except 2 ** %d";
            var result = -1;
            try
            {
                if (GetMapCellInfo(nX, nY, ref MapCellInfo))
                {
                    if (MapCellInfo != null)
                    {
                        try
                        {
                            if (MapCellInfo.ObjList != null)
                            {
                                n18 = 0;
                                while (true)
                                {
                                    if (MapCellInfo.ObjList.Count <= n18)
                                    {
                                        break;
                                    }
                                    OSObject = MapCellInfo.ObjList[n18];
                                    if (OSObject != null)
                                    {
                                        if (OSObject.btType == btType && OSObject.CellObj == pRemoveObject)
                                        {
                                            MapCellInfo.ObjList.RemoveAt(n18);
                                            OSObject = null;
                                            result = 1;
                                            // 减地图人物怪物计数
                                            if (btType == grobal2.OS_MOVINGOBJECT && !((TBaseObject)pRemoveObject).m_boDelFormMaped)
                                            {
                                                ((TBaseObject)pRemoveObject).m_boDelFormMaped = true;
                                                ((TBaseObject)pRemoveObject).m_boAddToMaped = false;
                                                DelObjectCount(pRemoveObject);
                                            }
                                            if (MapCellInfo.ObjList.Count > 0)
                                            {
                                                continue;
                                            }
                                            MapCellInfo.ObjList = null;
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        MapCellInfo.ObjList.RemoveAt(n18);
                                        if (MapCellInfo.ObjList.Count > 0)
                                        {
                                            continue;
                                        }
                                        MapCellInfo.ObjList = null;
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
                            M2Share.MainOutMessage(string.Format(sExceptionMsg1, new byte[] { btType }));
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
                M2Share.MainOutMessage(string.Format(sExceptionMsg2, new byte[] { btType }));
            }
            return result;
        }

        public TMapItem GetItem(int nX, int nY)
        {
            TMapCellinfo MapCellInfo = null;
            TOSObject OSObject;
            TBaseObject BaseObject;
            TMapItem result = null;
            bo2C = false;
            if (GetMapCellInfo(nX, nY, ref MapCellInfo) && MapCellInfo.chFlag == 0)
            {
                bo2C = true;
                if (MapCellInfo.ObjList != null)
                {
                    for (var i = 0; i < MapCellInfo.ObjList.Count; i++)
                    {
                        OSObject = MapCellInfo.ObjList[i];
                        if (OSObject.btType == grobal2.OS_ITEMOBJECT)
                        {
                            result = (TMapItem)OSObject.CellObj;
                            return result;
                        }
                        if (OSObject.btType == grobal2.OS_GATEOBJECT)
                        {
                            bo2C = false;
                        }
                        if (OSObject.btType == grobal2.OS_MOVINGOBJECT)
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

        public object AddToMapMineEvent(int nX, int nY, int nType, object __Event)
        {
            TMapCellinfo MapCellInfo = null;
            const string sExceptionMsg = "[Exception] TEnvirnoment::AddToMapMineEvent ";
            object result = null;
            try
            {
                var bo19 = GetMapCellInfo(nX, nY, ref MapCellInfo);
                var bo1A = false;
                if (bo19 && MapCellInfo.chFlag != 0)
                {
                    if (MapCellInfo.ObjList == null)
                    {
                        MapCellInfo.ObjList = new List<TOSObject>();
                    }
                    if (!bo1A)
                    {
                        var OSObject = new TOSObject
                        {
                            btType = (byte)nType,
                            CellObj = __Event,
                            dwAddTime = HUtil32.GetTickCount()
                        };
                        MapCellInfo.ObjList.Add(OSObject);
                        result = __Event;
                    }
                }
            }
            catch
            {
                M2Share.ErrorMessage(sExceptionMsg);
            }
            return result;
        }

        /// <summary>
        /// 刷新在地图上位置的时间
        /// </summary>
        /// <param name="nX"></param>
        /// <param name="nY"></param>
        /// <param name="BaseObject"></param>
        public void VerifyMapTime(int nX, int nY, object BaseObject)
        {
            TMapCellinfo MapCellInfo = null;
            TOSObject OSObject;
            bool boVerify;
            const string sExceptionMsg = "[Exception] TEnvirnoment::VerifyMapTime";
            try
            {
                boVerify = false;
                if (GetMapCellInfo(nX, nY, ref MapCellInfo) && MapCellInfo != null && MapCellInfo.ObjList != null)
                {
                    for (var i = 0; i < MapCellInfo.ObjList.Count; i++)
                    {
                        OSObject = MapCellInfo.ObjList[i];
                        if (OSObject.btType == grobal2.OS_MOVINGOBJECT && OSObject.CellObj == BaseObject)
                        {
                            OSObject.dwAddTime = HUtil32.GetTickCount();
                            boVerify = true;
                            break;
                        }
                    }
                }
                if (!boVerify)
                {
                    AddToMap(nX, nY, grobal2.OS_MOVINGOBJECT, BaseObject);
                }
            }
            catch
            {
                M2Share.ErrorMessage(sExceptionMsg);
            }
        }

        public TEnvirnoment()
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
                            MapCellArray[n24 + nH] = new TMapCellinfo();
                            // wBkImg High
                            if ((buffer[buffIndex + 1] & 0x80) != 0)
                            {
                                MapCellArray[n24 + nH].chFlag = 1;
                            }
                            // wFrImg High
                            if ((buffer[buffIndex + 5] & 0x80) != 0)
                            {
                                MapCellArray[n24 + nH].chFlag = 2;
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
                                MapCellArray[nW * wHeight + nH].ObjList = null;
                            }
                        }
                    }
                    MapCellArray = null;
                }
                wWidth = nWidth;
                wHeight = nHeight;
                MapCellArray = new TMapCellinfo[nWidth * nHeight];
            }
        }

        public bool CreateQuest(int nFlag, int nValue, string sMonName, string sItem, string sQuest, bool boGrouped)
        {
            TMapQuestInfo MapQuest;
            TMerchant MapMerchant;
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
            MapMerchant = new TMerchant
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
            TMapCellinfo MapCellInfo = null;
            TOSObject OSObject;
            TBaseObject BaseObject;
            if (GetMapCellInfo(nX, nY, ref MapCellInfo) && MapCellInfo.ObjList != null)
            {
                for (var i = 0; i < MapCellInfo.ObjList.Count; i++)
                {
                    OSObject = MapCellInfo.ObjList[i];
                    if (OSObject.btType == grobal2.OS_MOVINGOBJECT)
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
                case grobal2.DR_UP:
                    if (sny > nFlag - 1)
                    {
                        sny -= (short)nFlag;
                    }
                    break;
                case grobal2.DR_DOWN:
                    if (sny < wWidth - nFlag)
                    {
                        sny += (short)nFlag;
                    }
                    break;
                case grobal2.DR_LEFT:
                    if (snx > nFlag - 1)
                    {
                        snx -= (short)nFlag;
                    }
                    break;
                case grobal2.DR_RIGHT:
                    if (snx < wWidth - nFlag)
                    {
                        snx += (short)nFlag;
                    }
                    break;
                case grobal2.DR_UPLEFT:
                    if (snx > nFlag - 1 && sny > nFlag - 1)
                    {
                        snx -= (short)nFlag;
                        sny -= (short)nFlag;
                    }
                    break;
                case grobal2.DR_UPRIGHT:
                    if (snx > nFlag - 1 && sny < wHeight - nFlag)
                    {
                        snx += (short)nFlag;
                        sny -= (short)nFlag;
                    }
                    break;
                case grobal2.DR_DOWNLEFT:
                    if (snx < wWidth - nFlag && sny > nFlag - 1)
                    {
                        snx -= (short)nFlag;
                        sny += (short)nFlag;
                    }
                    break;
                case grobal2.DR_DOWNRIGHT:
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
            TMapCellinfo MapCellInfo = null;
            TOSObject OSObject;
            if (GetMapCellInfo(nX, nY, ref MapCellInfo) && MapCellInfo.ObjList != null)
            {
                for (var i = MapCellInfo.ObjList.Count - 1; i >= 0; i--)
                {
                    OSObject = MapCellInfo.ObjList[i];
                    if (OSObject.btType == grobal2.OS_EVENTOBJECT)
                    {
                        if (((TEvent)OSObject.CellObj).m_nDamage > 0)
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
            const string sExceptionMsg = "[Exception] TEnvirnoment::ArroundDoorOpened ";
            try
            {
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
            }
            catch
            {
                M2Share.ErrorMessage(sExceptionMsg);
            }
            return result;
        }

        public object GetMovingObject(short nX, short nY, bool boFlag)
        {
            object result = null;
            TMapCellinfo MapCellInfo = null;
            TOSObject OSObject;
            TBaseObject BaseObject;
            if (GetMapCellInfo(nX, nY, ref MapCellInfo) && MapCellInfo.ObjList != null)
            {
                for (var i = 0; i < MapCellInfo.ObjList.Count; i++)
                {
                    OSObject = MapCellInfo.ObjList[i];
                    if (OSObject.btType == grobal2.OS_MOVINGOBJECT)
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

        public object GetQuestNPC(object BaseObject, string sCharName, string sItem, bool boFlag)
        {
            object result = null;
            TMapQuestInfo MapQuestFlag;
            int nFlagValue;
            bool bo1D;
            for (var i = 0; i < m_QuestList.Count; i++)
            {
                MapQuestFlag = m_QuestList[i];
                nFlagValue = ((TBaseObject)BaseObject).GetQuestFalgStatus(MapQuestFlag.nFlag);
                if (nFlagValue == MapQuestFlag.nValue)
                {
                    if (boFlag == MapQuestFlag.boGrouped || !boFlag)
                    {
                        bo1D = false;
                        if (MapQuestFlag.sMonName != "" && MapQuestFlag.sItemName != "")
                        {
                            if (MapQuestFlag.sMonName == sCharName && MapQuestFlag.sItemName == sItem)
                            {
                                bo1D = true;
                            }
                        }
                        if (MapQuestFlag.sMonName != "" && MapQuestFlag.sItemName == "")
                        {
                            if (MapQuestFlag.sMonName == sCharName && sItem == "")
                            {
                                bo1D = true;
                            }
                        }
                        if (MapQuestFlag.sMonName == "" && MapQuestFlag.sItemName != "")
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
            TMapCellinfo MapCellInfo = null;
            TOSObject OSObject;
            TBaseObject BaseObject;
            nCount = 0;
            bo2C = false;
            if (GetMapCellInfo(nX, nY, ref MapCellInfo) && MapCellInfo.chFlag == 0)
            {
                bo2C = true;
                if (MapCellInfo.ObjList != null)
                {
                    for (var i = 0; i < MapCellInfo.ObjList.Count; i++)
                    {
                        OSObject = MapCellInfo.ObjList[i];
                        if (OSObject.btType == grobal2.OS_ITEMOBJECT)
                        {
                            result = OSObject.CellObj;
                            nCount++;
                        }
                        if (OSObject.btType == grobal2.OS_GATEOBJECT)
                        {
                            bo2C = false;
                        }
                        if (OSObject.btType == grobal2.OS_MOVINGOBJECT)
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
            TMapCellinfo MapCellInfo = null;
            TOSObject OSObject;
            var result = false;
            for (var nXX = nX - nRage; nXX <= nX + nRage; nXX++)
            {
                for (var nYY = nY - nRage; nYY <= nY + nRage; nYY++)
                {
                    if (GetMapCellInfo(nX, nY, ref MapCellInfo) && MapCellInfo.ObjList != null)
                    {
                        for (var i = 0; i < MapCellInfo.ObjList.Count; i++)
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
            int result;
            for (var nXX = nX - nRage; nXX <= nX + nRage; nXX++)
            {
                for (var nYY = nY - nRage; nYY <= nY + nRage; nYY++)
                {
                    GetBaseObjects(nXX, nYY, boFlag, BaseObjectList);
                }
            }
            result = BaseObjectList.Count;
            return result;
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
            TMapCellinfo MapCellInfo = null;
            TOSObject OSObject;
            TBaseObject BaseObject;
            if (GetMapCellInfo(nX, nY, ref MapCellInfo) && MapCellInfo.ObjList != null)
            {
                for (var i = 0; i < MapCellInfo.ObjList.Count; i++)
                {
                    OSObject = MapCellInfo.ObjList[i];
                    if (OSObject.btType == grobal2.OS_MOVINGOBJECT)
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
            TMapCellinfo MapCellInfo = null;
            TOSObject OSObject;
            object result = null;
            bo2C = false;
            if (GetMapCellInfo(nX, nY, ref MapCellInfo) && MapCellInfo.ObjList != null)
            {
                for (var i = 0; i < MapCellInfo.ObjList.Count; i++)
                {
                    OSObject = MapCellInfo.ObjList[i];
                    if (OSObject.btType == grobal2.OS_EVENTOBJECT)
                    {
                        result = OSObject.CellObj;
                    }
                }
            }
            return result;
        }

        public void SetMapXYFlag(int nX, int nY, bool boFlag)
        {
            TMapCellinfo MapCellInfo = null;
            if (GetMapCellInfo(nX, nY, ref MapCellInfo))
            {
                if (boFlag)
                {
                    MapCellInfo.chFlag = 0;
                }
                else
                {
                    MapCellInfo.chFlag = 2;
                }
            }
        }

        public bool CanFly(int nsX, int nsY, int ndX, int ndY)
        {
            bool result;
            double r28;
            double r30;
            int n14;
            int n18;
            int n1C;
            result = true;
            r28 = (ndX - nsX) / 1.0e1;
            r30 = (ndY - ndX) / 1.0e1;
            n14 = 0;
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
            TMapCellinfo MapCellInfo = null;
            TOSObject OSObject;
            TBaseObject BaseObject;
            var result = false;
            if (GetMapCellInfo(nMapX, nMapY, ref MapCellInfo) && MapCellInfo.ObjList != null)
            {
                for (var i = 0; i < MapCellInfo.ObjList.Count; i++)
                {
                    OSObject = MapCellInfo.ObjList[i];
                    if (OSObject.btType == grobal2.OS_MOVINGOBJECT)
                    {
                        BaseObject = (TBaseObject)OSObject.CellObj;
                        if (BaseObject.m_btRaceServer == grobal2.RC_PLAYOBJECT)
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
            TMapCellinfo MapCellInfo = null;
            var result = true;
            if (GetMapCellInfo(nX, nY, ref MapCellInfo) && MapCellInfo.chFlag == 2)
            {
                result = false;
            }
            return result;
        }

        public string GetEnvirInfo()
        {
            string result;
            string sMsg;
            sMsg = "Map:%s(%s) DAY:%s DARK:%s SAFE:%s FIGHT:%s FIGHT3:%s QUIZ:%s NORECONNECT:%s(%s) MUSIC:%s(%d) EXPRATE:%s(%f) PKWINLEVEL:%s(%d) PKLOSTLEVEL:%s(%d) PKWINEXP:%s(%d) PKLOSTEXP:%s(%d) DECHP:%s(%d/%d) INCHP:%s(%d/%d)";
            sMsg = sMsg + " DECGAMEGOLD:%s(%d/%d) INCGAMEGOLD:%s(%d/%d) INCGAMEPOINT:%s(%d/%d) RUNHUMAN:%s RUNMON:%s NEEDHOLE:%s NORECALL:%s NOGUILDRECALL:%s NODEARRECALL:%s NOMASTERRECALL:%s NODRUG:%s MINE:%s MINE2:%s NODROPITEM:%s";
            sMsg = sMsg + " NOTHROWITEM:%s NOPOSITIONMOVE:%s NOHORSE:%s NOHUMNOMON:%s NOCHAT:%s ";
            result = string.Format(sMsg, sMapName, sMapDesc, HUtil32.BoolToStr(Flag.boDayLight), HUtil32.BoolToStr(Flag.boDarkness), HUtil32.BoolToStr(Flag.boSAFE), HUtil32.BoolToStr(Flag.boFightZone), HUtil32.BoolToStr(Flag.boFight3Zone), HUtil32.BoolToStr(Flag.boQUIZ), HUtil32.BoolToStr(Flag.boNORECONNECT), Flag.sNoReConnectMap, HUtil32.BoolToStr(Flag.boMUSIC), Flag.nMUSICID, HUtil32.BoolToStr(Flag.boEXPRATE), Flag.nEXPRATE / 100, HUtil32.BoolToStr(Flag.boPKWINLEVEL), Flag.nPKWINLEVEL, HUtil32.BoolToStr(Flag.boPKLOSTLEVEL), Flag.nPKLOSTLEVEL, HUtil32.BoolToStr(Flag.boPKWINEXP), Flag.nPKWINEXP, HUtil32.BoolToStr(Flag.boPKLOSTEXP), Flag.nPKLOSTEXP, HUtil32.BoolToStr(Flag.boDECHP), Flag.nDECHPTIME, Flag.nDECHPPOINT, HUtil32.BoolToStr(Flag.boINCHP), Flag.nINCHPTIME, Flag.nINCHPPOINT, HUtil32.BoolToStr(Flag.boDECGAMEGOLD), Flag.nDECGAMEGOLDTIME, Flag.nDECGAMEGOLD, HUtil32.BoolToStr(Flag.boINCGAMEGOLD), Flag.nINCGAMEGOLDTIME, Flag.nINCGAMEGOLD, HUtil32.BoolToStr(Flag.boINCGAMEPOINT), Flag.nINCGAMEPOINTTIME, Flag.nINCGAMEPOINT, HUtil32.BoolToStr(Flag.boRUNHUMAN), HUtil32.BoolToStr(Flag.boRUNMON), HUtil32.BoolToStr(Flag.boNEEDHOLE), HUtil32.BoolToStr(Flag.boNORECALL), HUtil32.BoolToStr(Flag.boNOGUILDRECALL), HUtil32.BoolToStr(Flag.boNODEARRECALL), HUtil32.BoolToStr(Flag.boNOMASTERRECALL), HUtil32.BoolToStr(Flag.boNODRUG), HUtil32.BoolToStr(Flag.boMINE), HUtil32.BoolToStr(Flag.boMINE2), HUtil32.BoolToStr(Flag.boNODROPITEM), HUtil32.BoolToStr(Flag.boNOTHROWITEM), HUtil32.BoolToStr(Flag.boNOPOSITIONMOVE), HUtil32.BoolToStr(Flag.boNOHORSE), HUtil32.BoolToStr(Flag.boNOHUMNOMON), HUtil32.BoolToStr(Flag.boNOCHAT));
            return result;
        }

        public void AddObject(object BaseObject)
        {
            var btRaceServer = ((TBaseObject)BaseObject).m_btRaceServer;
            if (btRaceServer == grobal2.RC_PLAYOBJECT)
            {
                m_nHumCount++;
            }
            if (btRaceServer >= grobal2.RC_ANIMAL)
            {
                m_nMonCount++;
            }
        }

        public void DelObjectCount(object BaseObject)
        {
            var btRaceServer = ((TBaseObject)BaseObject).m_btRaceServer;
            if (btRaceServer == grobal2.RC_PLAYOBJECT)
            {
                m_nHumCount++;
            }
            if (btRaceServer >= grobal2.RC_ANIMAL)
            {
                m_nMonCount++;
            }
        }
    }
}

