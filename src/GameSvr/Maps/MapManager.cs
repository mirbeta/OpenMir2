using GameSvr.Event.Events;
using GameSvr.Npc;
using NLog;
using SystemModule;
using SystemModule.Data;

namespace GameSvr.Maps
{
    public class MapManager
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly Dictionary<string, Envirnoment> _mapList = new Dictionary<string, Envirnoment>(StringComparer.OrdinalIgnoreCase);
        /// <summary>
        /// 地图上门列表
        /// </summary>
        private readonly IList<Envirnoment> _mapDoorList = new List<Envirnoment>();
        /// <summary>
        /// 矿物地图列表
        /// </summary>
        private readonly IList<Envirnoment> _mapMineList = new List<Envirnoment>();

        public IList<Envirnoment> Maps => _mapList.Values.ToList();

        public void MakeSafePkZone()
        {
            for (var i = 0; i < M2Share.StartPointList.Count; i++)
            {
                var StartPoint = M2Share.StartPointList[i];
                if (StartPoint != null && StartPoint.m_nType > 0)
                {
                    var Envir = FindMap(StartPoint.m_sMapName);
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
                                    var SafeEvent = new SafeEvent(Envir, nX, nY, StartPoint.m_nType);
                                    M2Share.EventMgr.AddEvent(SafeEvent);
                                }
                            }
                        }
                    }
                }
            }
        }

        public IList<Envirnoment> GetMineMaps()
        {
            return _mapMineList;
        }

        public IList<Envirnoment> GetDoorMapList()
        {
            return _mapDoorList;
        }

        public Envirnoment AddMapInfo(string sMapName, string sMapDesc, int nServerNumber, MapInfoFlag MapFlag, Merchant QuestNPC)
        {
            var sMapFileName = string.Empty;
            var sTempName = sMapName;
            if (sTempName.IndexOf('|') > -1)
            {
                sMapFileName = HUtil32.GetValidStr3(sTempName, ref sMapName, '|');
            }
            else
            {
                sTempName = HUtil32.ArrestStringEx(sTempName, "<", ">", ref sMapFileName);
                if (sMapFileName == "")
                {
                    sMapFileName = sMapName;
                }
                else
                {
                    sMapName = sTempName;
                }
            }
            var envirnoment = new Envirnoment
            {
                MapName = sMapName,
                MapFileName = sMapFileName,
                MapDesc = sMapDesc,
                ServerIndex = nServerNumber,
                Flag = MapFlag,
                QuestNpc = QuestNPC
            };
            if (M2Share.MiniMapList.TryGetValue(envirnoment.MapName, out var minMap))
            {
                envirnoment.MinMap = minMap;
            }
            if (envirnoment.LoadMapData(Path.Combine(M2Share.BasePath, M2Share.Config.MapDir, sMapFileName + ".map")))
            {
                if (!_mapList.ContainsKey(sMapName))
                {
                    _mapList.Add(sMapName, envirnoment);
                }
                else
                {
                    M2Share.Log.LogError("地图名称重复 [" + sMapName + "]，请确认配置文件是否正确.");
                }
                if (envirnoment.DoorList.Count > 0)
                {
                    _mapDoorList.Add(envirnoment);
                }
                if (envirnoment.Flag.boMINE || envirnoment.Flag.boMINE2)
                {
                    _mapMineList.Add(envirnoment);
                }
            }
            else
            {
                M2Share.Log.LogError("地图文件: " + M2Share.Config.MapDir + sMapName + ".map" + "未找到,或者加载出错!!!");
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
                var GateObj = new GateObject
                {
                    boFlag = false,
                    Envir = DEnvir,
                    nX = (short)nDMapX,
                    nY = (short)nDMapY
                };
                SEnvir.AddToMap(nSMapX, nSMapY, CellType.Route, GateObj);
                result = true;
            }
            return result;
        }

        public Envirnoment FindMap(string sMapName)
        {
            return _mapList.TryGetValue(sMapName, out var map) ? map : null;
        }

        public Envirnoment GetMapInfo(int nServerIdx, string sMapName)
        {
            Envirnoment result = null;
            if (_mapList.TryGetValue(sMapName, out var envirnoment))
            {
                if (envirnoment.ServerIndex == nServerIdx)
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
            if (_mapList.TryGetValue(sMapName, out var envirnoment))
            {
                return envirnoment.ServerIndex;
            }
            return 0;
        }

        public void LoadMapDoor()
        {
            for (var i = 0; i < Maps.Count; i++)
            {
                this.Maps[i].AddDoorToMap();
            }
            _logger.Info("地图环境加载成功...");
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