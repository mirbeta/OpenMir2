using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SystemModule;

namespace GameSvr
{
    public class MapManager
    {
        private readonly Dictionary<string, Envirnoment> m_MapList = new Dictionary<string, Envirnoment>(StringComparer.OrdinalIgnoreCase);

        public IList<Envirnoment> Maps => m_MapList.Values.ToList();

        public void MakeSafePkZone()
        {
            SafeEvent SafeEvent;
            TStartPoint StartPoint;
            Envirnoment Envir;
            for (var i = 0; i < M2Share.StartPointList.Count; i++)
            {
                StartPoint = M2Share.StartPointList[i];
                if (StartPoint != null && StartPoint.m_nType > 0)
                {
                    Envir = FindMap(StartPoint.m_sMapName);
                    if (Envir != null)
                    {
                        int nMinX = StartPoint.m_nCurrX - StartPoint.m_nRange;
                        int nMaxX = StartPoint.m_nCurrX + StartPoint.m_nRange;
                        int nMinY = StartPoint.m_nCurrY - StartPoint.m_nRange;
                        int nMaxY = StartPoint.m_nCurrY + StartPoint.m_nRange;
                        for (var nX = nMinX; nX <= nMaxX; nX++)
                        {
                            for (var nY = nMinY; nY <= nMaxY; nY++)
                            {
                                if (nX < nMaxX && nY == nMinY || nY < nMaxY && nX == nMinX || nX == nMaxX || nY == nMaxY)
                                {
                                    SafeEvent = new SafeEvent(Envir, nX, nY, StartPoint.m_nType);
                                    M2Share.EventManager.AddEvent(SafeEvent);
                                }
                            }
                        }
                    }
                }
            }
        }

        public IList<Envirnoment> GetMineMaps()
        {
            var list = new List<Envirnoment>();
            foreach (var item in m_MapList.Values)
            {
                if (item.Flag.boMINE || item.Flag.boMINE2)
                {
                    list.Add(item);
                }
            }
            return list;
        }

        public IList<Envirnoment> GetDoorMapList()
        {
            var list = new List<Envirnoment>();
            foreach (var item in m_MapList.Values)
            {
                if (item.m_DoorList.Count > 0)
                {
                    list.Add(item);
                }
            }
            return list;
        }

        public Envirnoment AddMapInfo(string sMapName, string sMapDesc, int nServerNumber, TMapFlag MapFlag, object QuestNPC)
        {
            var m_sMapFileName = string.Empty;
            var sTempName = sMapName;
            if (sTempName.IndexOf('|') > -1)
            {
                m_sMapFileName = HUtil32.GetValidStr3(sTempName, ref sMapName, new[] { '|' });
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
            var envirnoment = new Envirnoment
            {
                sMapName = sMapName,
                m_sMapFileName = m_sMapFileName,
                sMapDesc = sMapDesc,
                nServerIndex = nServerNumber,
                Flag = MapFlag,
                QuestNPC = QuestNPC
            };
            if (M2Share.MiniMapList.TryGetValue(envirnoment.sMapName, out var minMap))
            {
                envirnoment.nMinMap = minMap;
            }
            if (envirnoment.LoadMapData(Path.Combine(M2Share.sConfigPath, M2Share.g_Config.sMapDir, m_sMapFileName + ".map")))
            {
                if (!m_MapList.ContainsKey(sMapName))
                {
                    m_MapList.Add(sMapName, envirnoment);
                }
                else
                {
                    M2Share.ErrorMessage("地图名称重复 [" + sMapName + "]，请确认配置文件是否正确.");
                }
            }
            else
            {
                M2Share.ErrorMessage("地图文件: " + M2Share.g_Config.sMapDir + sMapName + ".map" + "未找到,或者加载出错!!!");
            }
            return envirnoment;
        }

        public bool AddMapRoute(string sSMapNO, int nSMapX, int nSMapY, string sDMapNO, int nDMapX, int nDMapY)
        {
            bool result = false;
            Envirnoment SEnvir = FindMap(sSMapNO);
            Envirnoment DEnvir = FindMap(sDMapNO);
            if (SEnvir != null && DEnvir != null)
            {
                var GateObj = new TGateObj
                {
                    boFlag = false,
                    DEnvir = DEnvir,
                    nDMapX = (short)nDMapX,
                    nDMapY = (short)nDMapY
                };
                SEnvir.AddToMap(nSMapX, nSMapY, CellType.OS_GATEOBJECT, GateObj);
                result = true;
            }
            return result;
        }

        public Envirnoment FindMap(string sMapName)
        {
            Envirnoment Map = null;
            return m_MapList.TryGetValue(sMapName, out Map) ? Map : null;
        }

        public Envirnoment GetMapInfo(int nServerIdx, string sMapName)
        {
            Envirnoment result = null;
            if (m_MapList.TryGetValue(sMapName, out var envirnoment))
            {
                if (envirnoment.nServerIndex == nServerIdx)
                {
                    result = envirnoment;
                }
            }
            return result;
        }

        /// <summary>
        /// 取地图编号服务器
        /// </summary>
        /// <param name="sMapName"></param>
        /// <returns></returns>
        public int GetMapOfServerIndex(string sMapName)
        {
            if (m_MapList.TryGetValue(sMapName, out var envirnoment))
            {
                return envirnoment.nServerIndex;
            }
            return 0;
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
            // for (var I = 0; I < this.Count; I ++ )
            // {
            //     var Envirnoment = ((this.Items[I]) as TEnvirnoment);
            //     for (var II = 0; II < M2Share.MiniMapList.Count; II ++ )
            //     {
            //         if ((M2Share.MiniMapList[II]).CompareTo((Envirnoment.sMapName)) == 0)
            //         {
            //             Envirnoment.nMinMap = ((int)M2Share.MiniMapList.Values[II]);
            //             break;
            //         }
            //     }
            // }
        }

        public void Run()
        {

        }
    }
}