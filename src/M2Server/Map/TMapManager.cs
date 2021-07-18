using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace M2Server
{
    public class TMapManager
    {
        private readonly Dictionary<string, TEnvirnoment> m_MapList = new Dictionary<string, TEnvirnoment>();

        public IList<TEnvirnoment> Maps
        {
            get
            {
                return m_MapList.Values.ToList();
            }
        }

        public void MakeSafePkZone()
        {
            int nX;
            int nY;
            TSafeEvent SafeEvent;
            int nMinX;
            int nMaxX;
            int nMinY;
            int nMaxY;
            TStartPoint StartPoint;
            TEnvirnoment Envir;
            for (var i = 0; i < M2Share.StartPointList.Count; i++)
            {
                StartPoint = M2Share.StartPointList[i];
                if (StartPoint != null && StartPoint.m_nType > 0)
                {
                    Envir = FindMap(StartPoint.m_sMapName);
                    if (Envir != null)
                    {
                        nMinX = StartPoint.m_nCurrX - StartPoint.m_nRange;
                        nMaxX = StartPoint.m_nCurrX + StartPoint.m_nRange;
                        nMinY = StartPoint.m_nCurrY - StartPoint.m_nRange;
                        nMaxY = StartPoint.m_nCurrY + StartPoint.m_nRange;
                        for (nX = nMinX; nX <= nMaxX; nX++)
                        {
                            for (nY = nMinY; nY <= nMaxY; nY++)
                            {
                                if (nX < nMaxX && nY == nMinY || nY < nMaxY && nX == nMinX || nX == nMaxX || nY == nMaxY)
                                {
                                    SafeEvent = new TSafeEvent(Envir, nX, nY, StartPoint.m_nType);
                                    M2Share.EventManager.AddEvent(SafeEvent);
                                }
                            }
                        }
                    }
                }
            }
        }

        public IList<TEnvirnoment> GetMineMaps()
        {
            var list = new List<TEnvirnoment>();
            foreach (var item in m_MapList.Values)
            {
                if (item.Flag.boMINE || item.Flag.boMINE2)
                {
                    list.Add(item);
                }
            }
            return list;
        }

        public IList<TEnvirnoment> GetDoorMapList()
        {
            var list = new List<TEnvirnoment>();
            foreach (var item in m_MapList.Values)
            {
                if (item.m_DoorList.Count > 0)
                {
                    list.Add(item);
                }
            }
            return list;
        }

        public TEnvirnoment AddMapInfo(string sMapName, string sMapDesc, int nServerNumber, TMapFlag MapFlag, object QuestNPC)
        {
            var m_sMapFileName = string.Empty;
            TEnvirnoment result = null;
            var Envir = new TEnvirnoment();
            var sTempName = sMapName;
            if (sTempName.IndexOf('|') > 0)
            {
                m_sMapFileName = HUtil32.GetValidStr3(sTempName, ref sMapName, new char[] { '|' });
            }
            else
            {
                sTempName = HUtil32.ArrestStringEx(sTempName, '<', '>', ref m_sMapFileName);
                if (m_sMapFileName == "")
                {
                    m_sMapFileName = sMapName;
                }
                else
                {
                    sMapName = sTempName;
                }
            }
            Envir.sMapName = sMapName;
            Envir.m_sMapFileName = m_sMapFileName;
            Envir.sMapDesc = sMapDesc;
            Envir.nServerIndex = nServerNumber;
            Envir.Flag = MapFlag;
            Envir.QuestNPC = QuestNPC;
            var minMap = 0;
            if (M2Share.MiniMapList.TryGetValue(Envir.sMapName.ToLower(), out minMap))
            {
                Envir.nMinMap = minMap;
            }
            if (Envir.LoadMapData(Path.Combine(M2Share.g_Config.sMapDir, m_sMapFileName + ".map")))
            {
                result = Envir;
                if (!m_MapList.ContainsKey(sMapName.ToLower()))
                {
                    m_MapList.Add(sMapName.ToLower(), Envir);
                }
                else
                {
                    M2Share.ErrorMessage("地图名称重复 [" + sMapName + "]，请确认配置文件是否正确.");
                }
            }
            else
            {
                M2Share.ErrorMessage("地图文件: " + M2Share.g_Config.sMapDir + sMapName + ".map" + "未找到,或者加载出错！！！");
            }
            return result;
        }

        public bool AddMapRoute(string sSMapNO, int nSMapX, int nSMapY, string sDMapNO, int nDMapX, int nDMapY)
        {
            bool result = false;
            TEnvirnoment SEnvir = FindMap(sSMapNO);
            TEnvirnoment DEnvir = FindMap(sDMapNO);
            if (SEnvir != null && DEnvir != null)
            {
                var GateObj = new TGateObj
                {
                    boFlag = false,
                    DEnvir = DEnvir,
                    nDMapX = (short)nDMapX,
                    nDMapY = (short)nDMapY
                };
                SEnvir.AddToMap(nSMapX, nSMapY, grobal2.OS_GATEOBJECT, GateObj);
                result = true;
            }
            return result;
        }


        public TMapManager()
            : base()
        {

        }

        public TEnvirnoment FindMap(string sMapName)
        {
            TEnvirnoment result = null;
            TEnvirnoment Map = null;
            if (m_MapList.TryGetValue(sMapName.ToLower(), out Map))
            {
                return Map;
            }
            return result;
        }

        public TEnvirnoment GetMapInfo(int nServerIdx, string sMapName)
        {
            TEnvirnoment result = null;
            TEnvirnoment Envir = null;
            if (m_MapList.TryGetValue(sMapName.ToLower(), out Envir))
            {
                if (Envir.nServerIndex == nServerIdx)
                {
                    result = Envir;
                }
            }
            return result;
        }

        public int GetMapOfServerIndex(string sMapName)
        {
            var result = 0;
            TEnvirnoment Envir = null;
            if (m_MapList.TryGetValue(sMapName.ToLower(), out Envir))
            {
                return Envir.nServerIndex;
            }
            return result;
        }

        public void LoadMapDoor()
        {
            for (var i = 0; i < Maps.Count; i++)
            {
                this.Maps[i].AddDoorToMap();
            }
        }

        public void ProcessMapDoor()
        {
        }

        public void ReSetMinMap()
        {
            //for (I = 0; I < this.Count; I ++ )
            //{
            //    Envirnoment = ((this.Items[I]) as TEnvirnoment);
            //    for (II = 0; II < M2Share.MiniMapList.Count; II ++ )
            //    {
            //        if ((M2Share.MiniMapList[II]).ToLower().CompareTo((Envirnoment.sMapName).ToLower()) == 0)
            //        {
            //            Envirnoment.nMinMap = ((int)M2Share.MiniMapList.Values[II]);
            //            break;
            //        }
            //    }
            //}
        }

        public void Run()
        {

        }
    }
}