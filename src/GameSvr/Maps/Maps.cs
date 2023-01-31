using GameSvr.Event.Events;
using GameSvr.Npc;
using NLog;
using System.Diagnostics;
using SystemModule.Common;
using SystemModule.Data;

namespace GameSvr.Maps
{
    public class Map
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        private static Thread _makeStoneMinesThread;

        public static void StartMakeStoneThread()
        {
            if (_makeStoneMinesThread == null)
            {
                _makeStoneMinesThread = new Thread(MakeStoneMines) { IsBackground = true };
            }
            _makeStoneMinesThread.Start();
        }

        public static int LoadMapInfo()
        {
            logger.Info("正在加载地图数据...");
            string sFlag = string.Empty;
            string s34 = string.Empty;
            string sLine = string.Empty;
            string sReConnectMap = string.Empty;
            int result = -1;
            string sFileName = Path.Combine(M2Share.BasePath, M2Share.Config.EnvirDir, "MapInfo.txt");
            if (File.Exists(sFileName))
            {
                StringList LoadList = new StringList();
                LoadList.LoadFromFile(sFileName);
                if (LoadList.Count < 0)
                {
                    return result;
                }
                int count = 0;
                while (true)
                {
                    if (count >= LoadList.Count)
                    {
                        break;
                    }
                    if (HUtil32.CompareLStr("ConnectMapInfo", LoadList[count]))
                    {
                        string sMapInfoFile = HUtil32.GetValidStr3(LoadList[count], ref sFlag, new[] { ' ', '\t' });
                        LoadList.RemoveAt(count);
                        if (sMapInfoFile != "")
                        {
                            LoadSubMapInfo(LoadList, sMapInfoFile);
                        }
                    }
                    count++;
                }
                result = 1;
                // 加载地图设置
                string sMapName;
                for (int i = 0; i < LoadList.Count; i++)
                {
                    sFlag = LoadList[i];
                    if (!string.IsNullOrEmpty(sFlag) && sFlag[0] == '[')
                    {
                        sMapName = "";
                        MapInfoFlag MapFlag = new MapInfoFlag
                        {
                            boSAFE = false
                        };
                        sFlag = HUtil32.ArrestStringEx(sFlag, "[", "]", ref sMapName);
                        string sMapDesc = HUtil32.GetValidStrCap(sMapName, ref sMapName, HUtil32.Separator);
                        if (!string.IsNullOrEmpty(sMapDesc) && sMapDesc[0] == '\"')
                        {
                            HUtil32.ArrestStringEx(sMapDesc, "\"", "\"", ref sMapDesc);
                        }
                        string s4C = HUtil32.GetValidStr3(sMapDesc, ref sMapDesc, HUtil32.Separator).Trim();
                        byte nServerIndex = (byte)HUtil32.StrToInt(s4C, 0);
                        if (sMapName == "")
                        {
                            continue;
                        }
                        MapFlag.nL = 1;
                        Merchant QuestNPC = null;
                        MapFlag.boSAFE = false;
                        MapFlag.nNEEDSETONFlag = -1;
                        MapFlag.nNeedONOFF = -1;
                        MapFlag.nMUSICID = -1;
                        while (true)
                        {
                            if (sFlag == "")
                            {
                                break;
                            }
                            sFlag = HUtil32.GetValidStr3(sFlag, ref s34, HUtil32.Separator);
                            if (s34 == "")
                            {
                                break;
                            }
                            if (s34.Equals("SAFE", StringComparison.OrdinalIgnoreCase))
                            {
                                MapFlag.boSAFE = true;
                                continue;
                            }
                            if (string.Compare(s34, "DARK", StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                MapFlag.boDarkness = true;
                                continue;
                            }
                            if (string.Compare(s34, "FIGHT", StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                MapFlag.boFightZone = true;
                                continue;
                            }
                            if (string.Compare(s34, "FIGHT3", StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                MapFlag.boFight3Zone = true;
                                continue;
                            }
                            if (string.Compare(s34, "DAY", StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                MapFlag.boDayLight = true;
                                continue;
                            }
                            if (string.Compare(s34, "QUIZ", StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                MapFlag.boQUIZ = true;
                                continue;
                            }
                            if (HUtil32.CompareLStr(s34, "NORECONNECT"))
                            {
                                MapFlag.boNORECONNECT = true;
                                HUtil32.ArrestStringEx(s34, "(", ")", ref sReConnectMap);
                                MapFlag.sNoReConnectMap = sReConnectMap;
                                if (MapFlag.sNoReConnectMap == "")
                                {
                                }
                                continue;
                            }
                            if (HUtil32.CompareLStr(s34, "CHECKQUEST"))
                            {
                                HUtil32.ArrestStringEx(s34, "(", ")", ref sLine);
                                QuestNPC = LoadMapQuest(sLine);
                                continue;
                            }
                            if (HUtil32.CompareLStr(s34, "NEEDSET_ON"))
                            {
                                MapFlag.nNeedONOFF = 1;
                                HUtil32.ArrestStringEx(s34, "(", ")", ref sLine);
                                MapFlag.nNEEDSETONFlag = HUtil32.StrToInt(sLine, -1);
                                continue;
                            }
                            if (HUtil32.CompareLStr(s34, "NEEDSET_OFF"))
                            {
                                MapFlag.nNeedONOFF = 0;
                                HUtil32.ArrestStringEx(s34, "(", ")", ref sLine);
                                MapFlag.nNEEDSETONFlag = HUtil32.StrToInt(sLine, -1);
                                continue;
                            }
                            if (HUtil32.CompareLStr(s34, "MUSIC"))
                            {
                                MapFlag.boMUSIC = true;
                                HUtil32.ArrestStringEx(s34, "(", ")", ref sLine);
                                MapFlag.nMUSICID = HUtil32.StrToInt(sLine, -1);
                                continue;
                            }
                            if (HUtil32.CompareLStr(s34, "EXPRATE"))
                            {
                                MapFlag.boEXPRATE = true;
                                HUtil32.ArrestStringEx(s34, "(", ")", ref sLine);
                                MapFlag.nEXPRATE = HUtil32.StrToInt(sLine, -1);
                                continue;
                            }
                            if (HUtil32.CompareLStr(s34, "PKWINLEVEL"))
                            {
                                MapFlag.boPKWINLEVEL = true;
                                HUtil32.ArrestStringEx(s34, "(", ")", ref sLine);
                                MapFlag.nPKWINLEVEL = HUtil32.StrToInt(sLine, -1);
                                continue;
                            }
                            if (HUtil32.CompareLStr(s34, "PKWINEXP"))
                            {
                                MapFlag.boPKWINEXP = true;
                                HUtil32.ArrestStringEx(s34, "(", ")", ref sLine);
                                MapFlag.nPKWINEXP = HUtil32.StrToInt(sLine, -1);
                                continue;
                            }
                            if (HUtil32.CompareLStr(s34, "PKLOSTLEVEL"))
                            {
                                MapFlag.boPKLOSTLEVEL = true;
                                HUtil32.ArrestStringEx(s34, "(", ")", ref sLine);
                                MapFlag.nPKLOSTLEVEL = HUtil32.StrToInt(sLine, -1);
                                continue;
                            }
                            if (HUtil32.CompareLStr(s34, "PKLOSTEXP"))
                            {
                                MapFlag.boPKLOSTEXP = true;
                                HUtil32.ArrestStringEx(s34, "(", ")", ref sLine);
                                MapFlag.nPKLOSTEXP = HUtil32.StrToInt(sLine, -1);
                                continue;
                            }
                            if (HUtil32.CompareLStr(s34, "DECHP"))
                            {
                                MapFlag.boDECHP = true;
                                HUtil32.ArrestStringEx(s34, "(", ")", ref sLine);
                                MapFlag.nDECHPPOINT = HUtil32.StrToInt(HUtil32.GetValidStr3(sLine, ref sLine, HUtil32.Backslash), -1);
                                MapFlag.nDECHPTIME = HUtil32.StrToInt(sLine, -1);
                                continue;
                            }
                            if (HUtil32.CompareLStr(s34, "INCHP"))
                            {
                                MapFlag.boINCHP = true;
                                HUtil32.ArrestStringEx(s34, "(", ")", ref sLine);
                                MapFlag.nINCHPPOINT = HUtil32.StrToInt(HUtil32.GetValidStr3(sLine, ref sLine, HUtil32.Backslash), -1);
                                MapFlag.nINCHPTIME = HUtil32.StrToInt(sLine, -1);
                                continue;
                            }
                            if (HUtil32.CompareLStr(s34, "DECGAMEGOLD"))
                            {
                                MapFlag.boDECGAMEGOLD = true;
                                HUtil32.ArrestStringEx(s34, "(", ")", ref sLine);
                                MapFlag.nDECGAMEGOLD = HUtil32.StrToInt(HUtil32.GetValidStr3(sLine, ref sLine, HUtil32.Backslash), -1);
                                MapFlag.nDECGAMEGOLDTIME = HUtil32.StrToInt(sLine, -1);
                                continue;
                            }
                            if (HUtil32.CompareLStr(s34, "DECGAMEPOINT"))
                            {
                                MapFlag.boDECGAMEPOINT = true;
                                HUtil32.ArrestStringEx(s34, "(", ")", ref sLine);
                                MapFlag.nDECGAMEPOINT = HUtil32.StrToInt(HUtil32.GetValidStr3(sLine, ref sLine, HUtil32.Backslash), -1);
                                MapFlag.nDECGAMEPOINTTIME = HUtil32.StrToInt(sLine, -1);
                                continue;
                            }
                            if (HUtil32.CompareLStr(s34, "INCGAMEGOLD"))
                            {
                                MapFlag.boINCGAMEGOLD = true;
                                HUtil32.ArrestStringEx(s34, "(", ")", ref sLine);
                                MapFlag.nINCGAMEGOLD = HUtil32.StrToInt(HUtil32.GetValidStr3(sLine, ref sLine, HUtil32.Backslash), -1);
                                MapFlag.nINCGAMEGOLDTIME = HUtil32.StrToInt(sLine, -1);
                                continue;
                            }
                            if (HUtil32.CompareLStr(s34, "INCGAMEPOINT"))
                            {
                                MapFlag.boINCGAMEPOINT = true;
                                HUtil32.ArrestStringEx(s34, "(", ")", ref sLine);
                                MapFlag.nINCGAMEPOINT = HUtil32.StrToInt(HUtil32.GetValidStr3(sLine, ref sLine, HUtil32.Backslash), -1);
                                MapFlag.nINCGAMEPOINTTIME = HUtil32.StrToInt(sLine, -1);
                                continue;
                            }
                            if (s34.Equals("RUNHUMAN", StringComparison.OrdinalIgnoreCase))
                            {
                                MapFlag.boRUNHUMAN = true;
                                continue;
                            }
                            if (s34.Equals("RUNMON", StringComparison.OrdinalIgnoreCase))
                            {
                                MapFlag.boRUNMON = true;
                                continue;
                            }
                            if (s34.Equals("NEEDHOLE", StringComparison.OrdinalIgnoreCase))
                            {
                                MapFlag.boNEEDHOLE = true;
                                continue;
                            }
                            if (s34.Equals("NORECALL", StringComparison.OrdinalIgnoreCase))
                            {
                                MapFlag.boNORECALL = true;
                                continue;
                            }
                            if (s34.Equals("NOGUILDRECALL", StringComparison.OrdinalIgnoreCase))
                            {
                                MapFlag.boNOGUILDRECALL = true;
                                continue;
                            }
                            if (s34.Equals("NODEARRECALL", StringComparison.OrdinalIgnoreCase))
                            {
                                MapFlag.boNODEARRECALL = true;
                                continue;
                            }
                            if (s34.Equals("NOMASTERRECALL", StringComparison.OrdinalIgnoreCase))
                            {
                                MapFlag.boNOMASTERRECALL = true;
                                continue;
                            }
                            if (s34.Equals("NORANDOMMOVE", StringComparison.OrdinalIgnoreCase))
                            {
                                MapFlag.boNORANDOMMOVE = true;
                                continue;
                            }
                            if (s34.Equals("NODRUG", StringComparison.OrdinalIgnoreCase))
                            {
                                MapFlag.boNODRUG = true;
                                continue;
                            }
                            if (s34.Equals("MINE", StringComparison.OrdinalIgnoreCase))
                            {
                                MapFlag.boMINE = true;
                                continue;
                            }
                            if (s34.Equals("MINE2", StringComparison.OrdinalIgnoreCase))
                            {
                                MapFlag.boMINE2 = true;
                                continue;
                            }
                            if (s34.Equals("NOTHROWITEM", StringComparison.OrdinalIgnoreCase))
                            {
                                MapFlag.boNOTHROWITEM = true;
                                continue;
                            }
                            if (s34.Equals("NODROPITEM", StringComparison.OrdinalIgnoreCase))
                            {
                                MapFlag.boNODROPITEM = true;
                                continue;
                            }
                            if (s34.Equals("NOPOSITIONMOVE", StringComparison.OrdinalIgnoreCase))
                            {
                                MapFlag.boNOPOSITIONMOVE = true;
                                continue;
                            }
                            if (s34.Equals("NOHORSE", StringComparison.OrdinalIgnoreCase))
                            {
                                MapFlag.boNOPOSITIONMOVE = true;
                                continue;
                            }
                            if (s34.Equals("NOCHAT", StringComparison.OrdinalIgnoreCase))
                            {
                                MapFlag.boNOCHAT = true;
                                continue;
                            }
                            if (HUtil32.CompareLStr(s34, "KILLFUNC"))
                            {
                                MapFlag.boKILLFUNC = true;
                                HUtil32.ArrestStringEx(s34, "(", ")", ref sLine);
                                MapFlag.nKILLFUNCNO = HUtil32.StrToInt(sLine, -1);
                                continue;
                            }
                            if (HUtil32.CompareLStr(s34, "NOHUMNOMON"))
                            {
                                // 有人才开始刷怪
                                MapFlag.boNOHUMNOMON = true;
                                continue;
                            }
                            if (s34[0] == 'L')
                            {
                                MapFlag.nL = HUtil32.StrToInt(s34[1..], 1);
                            }
                        }
                        if (M2Share.MapMgr.AddMapInfo(sMapName, sMapDesc, nServerIndex, MapFlag, QuestNPC) == null)
                        {

                        }
                        result = 1;
                    }
                }

                // 加载地图连接点
                for (int i = 0; i < LoadList.Count; i++)
                {
                    sFlag = LoadList[i];
                    if (!string.IsNullOrEmpty(sFlag) && sFlag[0] != '[' && sFlag[0] != ';')
                    {
                        sFlag = HUtil32.GetValidStr3(sFlag, ref s34, HUtil32.Separator);
                        sMapName = s34;
                        sFlag = HUtil32.GetValidStr3(sFlag, ref s34, HUtil32.Separator);
                        int nX = HUtil32.StrToInt(s34, 0);
                        sFlag = HUtil32.GetValidStr3(sFlag, ref s34, HUtil32.Separator);
                        int n18 = HUtil32.StrToInt(s34, 0);
                        sFlag = HUtil32.GetValidStr3(sFlag, ref s34, new[] { ' ', ',', '-', '>', '\t' });
                        string s44 = s34;
                        sFlag = HUtil32.GetValidStr3(sFlag, ref s34, HUtil32.Separator);
                        int n1C = HUtil32.StrToInt(s34, 0);
                        sFlag = HUtil32.GetValidStr3(sFlag, ref s34, new[] { ' ', ',', ';', '\t' });
                        int n20 = HUtil32.StrToInt(s34, 0);
                        M2Share.MapMgr.AddMapRoute(sMapName, nX, n18, s44, n1C, n20);
                    }
                }
            }
            logger.Info($"地图数据加载成功...[{M2Share.MapMgr.Maps.Count}]");
            return result;
        }

        public static int LoadMinMap()
        {
            logger.Info("正在加载数据图文件...");
            string sMapNO = string.Empty;
            string sMapIdx = string.Empty;
            int result = 0;
            string sFileName = Path.Combine(M2Share.BasePath, M2Share.Config.EnvirDir, "MiniMap.txt");
            if (File.Exists(sFileName))
            {
                M2Share.MiniMapList.Clear();
                StringList tMapList = new StringList();
                tMapList.LoadFromFile(sFileName);
                for (int i = 0; i < tMapList.Count; i++)
                {
                    string tStr = tMapList[i];
                    if (tStr != "" && tStr[0] != ';')
                    {
                        tStr = HUtil32.GetValidStr3(tStr, ref sMapNO, new[] { ' ', '\t' });
                        tStr = HUtil32.GetValidStr3(tStr, ref sMapIdx, new[] { ' ', '\t' });
                        int nIdx = HUtil32.StrToInt(sMapIdx, 0);
                        if (nIdx > 0)
                        {
                            if (M2Share.MiniMapList.ContainsKey(sMapNO))
                            {
                                M2Share.Log.Error($"重复小地图配置信息[{sMapNO}]");
                                continue;
                            }
                            M2Share.MiniMapList.TryAdd(sMapNO, nIdx);
                        }
                    }
                }
            }
            logger.Info("小地图数据加载成功...");
            return result;
        }

        private static void MakeStoneMines()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            IList<Envirnoment> mineMapList = M2Share.MapMgr.GetMineMaps();
            logger.Info($"初始化地图矿物数据...[{mineMapList.Count}]");
            for (int i = 0; i < mineMapList.Count; i++)
            {
                Envirnoment envir = mineMapList[i];
                for (int nW = 0; nW < envir.Width; nW++)
                {
                    for (int nH = 0; nH < envir.Height; nH++)
                    {
                        StoneMineEvent mine = new StoneMineEvent(envir, (short)nW, (short)nH, Grobal2.ET_MINE);
                        if (!mine.AddToMap)
                        {
                            mine.Dispose();
                        }
                    }
                }
            }
            sw.Stop();
            logger.Debug($"地图矿物数据初始化完成. 耗时:{sw.Elapsed}");
        }

        private static Merchant LoadMapQuest(string sName)
        {
            Merchant questNPC = new Merchant
            {
                MapName = "0",
                CurrX = 0,
                CurrY = 0,
                ChrName = sName,
                m_nFlag = 0,
                Appr = 0,
                m_sFilePath = "MapQuest_def",
                m_boIsHide = true,
                m_boIsQuest = false
            };
            M2Share.WorldEngine.QuestNpcList.Add(questNPC);
            return questNPC;
        }

        private static void LoadSubMapInfo(StringList loadList, string sFileName)
        {
            string sFileDir = Path.Combine(M2Share.BasePath, M2Share.Config.EnvirDir, "MapInfo");
            if (!Directory.Exists(sFileDir))
            {
                Directory.CreateDirectory(sFileDir);
            }
            string sFilePatchName = sFileDir + sFileName;
            if (File.Exists(sFilePatchName))
            {
                StringList loadMapList = new StringList();
                loadMapList.LoadFromFile(sFilePatchName);
                for (int i = 0; i < loadMapList.Count; i++)
                {
                    loadList.Add(loadMapList[i]);
                }
            }
        }

    }
}
