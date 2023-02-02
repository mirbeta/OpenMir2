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
            string sCommand = string.Empty;
            string sLine = string.Empty;
            string sReConnectMap = string.Empty;
            int result = -1;
            string sFileName = M2Share.GetEnvirFilePath("MapInfo.txt");
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
                            SafeArea = false
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
                        MapFlag.RequestLevel = 1;
                        Merchant QuestNPC = null;
                        MapFlag.SafeArea = false;
                        MapFlag.NeedSetonFlag = -1;
                        MapFlag.NeedOnOff = -1;
                        MapFlag.MusicId = -1;
                        while (true)
                        {
                            if (sFlag == "")
                            {
                                break;
                            }
                            sFlag = HUtil32.GetValidStr3(sFlag, ref sCommand, HUtil32.Separator);
                            if (string.IsNullOrEmpty(sCommand))
                            {
                                break;
                            }
                            if (sCommand.Equals("SAFE", StringComparison.OrdinalIgnoreCase))
                            {
                                MapFlag.SafeArea = true;
                                continue;
                            }
                            if (string.Compare(sCommand, "DARK", StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                MapFlag.boDarkness = true;
                                continue;
                            }
                            if (string.Compare(sCommand, "FIGHT", StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                MapFlag.FightZone = true;
                                continue;
                            }
                            if (string.Compare(sCommand, "FIGHT3", StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                MapFlag.Fight3Zone = true;
                                continue;
                            }
                            if (string.Compare(sCommand, "DAY", StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                MapFlag.DayLight = true;
                                continue;
                            }
                            if (string.Compare(sCommand, "QUIZ", StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                MapFlag.boQUIZ = true;
                                continue;
                            }
                            if (HUtil32.CompareLStr(sCommand, "NORECONNECT"))
                            {
                                MapFlag.boNORECONNECT = true;
                                HUtil32.ArrestStringEx(sCommand, "(", ")", ref sReConnectMap);
                                MapFlag.sNoReConnectMap = sReConnectMap;
                                if (MapFlag.sNoReConnectMap == "")
                                {
                                }
                                continue;
                            }
                            if (HUtil32.CompareLStr(sCommand, "CHECKQUEST"))
                            {
                                HUtil32.ArrestStringEx(sCommand, "(", ")", ref sLine);
                                QuestNPC = LoadMapQuest(sLine);
                                continue;
                            }
                            if (HUtil32.CompareLStr(sCommand, "NEEDSET_ON"))
                            {
                                MapFlag.NeedOnOff = 1;
                                HUtil32.ArrestStringEx(sCommand, "(", ")", ref sLine);
                                MapFlag.NeedSetonFlag = HUtil32.StrToInt(sLine, -1);
                                continue;
                            }
                            if (HUtil32.CompareLStr(sCommand, "NEEDSET_OFF"))
                            {
                                MapFlag.NeedOnOff = 0;
                                HUtil32.ArrestStringEx(sCommand, "(", ")", ref sLine);
                                MapFlag.NeedSetonFlag = HUtil32.StrToInt(sLine, -1);
                                continue;
                            }
                            if (HUtil32.CompareLStr(sCommand, "MUSIC"))
                            {
                                MapFlag.Music = true;
                                HUtil32.ArrestStringEx(sCommand, "(", ")", ref sLine);
                                MapFlag.MusicId = HUtil32.StrToInt(sLine, -1);
                                continue;
                            }
                            if (HUtil32.CompareLStr(sCommand, "EXPRATE"))
                            {
                                MapFlag.boEXPRATE = true;
                                HUtil32.ArrestStringEx(sCommand, "(", ")", ref sLine);
                                MapFlag.ExpRate = HUtil32.StrToInt(sLine, -1);
                                continue;
                            }
                            if (HUtil32.CompareLStr(sCommand, "PKWINLEVEL"))
                            {
                                MapFlag.boPKWINLEVEL = true;
                                HUtil32.ArrestStringEx(sCommand, "(", ")", ref sLine);
                                MapFlag.nPKWINLEVEL = HUtil32.StrToInt(sLine, -1);
                                continue;
                            }
                            if (HUtil32.CompareLStr(sCommand, "PKWINEXP"))
                            {
                                MapFlag.boPKWINEXP = true;
                                HUtil32.ArrestStringEx(sCommand, "(", ")", ref sLine);
                                MapFlag.nPKWINEXP = HUtil32.StrToInt(sLine, -1);
                                continue;
                            }
                            if (HUtil32.CompareLStr(sCommand, "PKLOSTLEVEL"))
                            {
                                MapFlag.boPKLOSTLEVEL = true;
                                HUtil32.ArrestStringEx(sCommand, "(", ")", ref sLine);
                                MapFlag.nPKLOSTLEVEL = HUtil32.StrToInt(sLine, -1);
                                continue;
                            }
                            if (HUtil32.CompareLStr(sCommand, "PKLOSTEXP"))
                            {
                                MapFlag.boPKLOSTEXP = true;
                                HUtil32.ArrestStringEx(sCommand, "(", ")", ref sLine);
                                MapFlag.nPKLOSTEXP = HUtil32.StrToInt(sLine, -1);
                                continue;
                            }
                            if (HUtil32.CompareLStr(sCommand, "DECHP"))
                            {
                                MapFlag.boDECHP = true;
                                HUtil32.ArrestStringEx(sCommand, "(", ")", ref sLine);
                                MapFlag.nDECHPPOINT = HUtil32.StrToInt(HUtil32.GetValidStr3(sLine, ref sLine, HUtil32.Backslash), -1);
                                MapFlag.nDECHPTIME = HUtil32.StrToInt(sLine, -1);
                                continue;
                            }
                            if (HUtil32.CompareLStr(sCommand, "INCHP"))
                            {
                                MapFlag.boINCHP = true;
                                HUtil32.ArrestStringEx(sCommand, "(", ")", ref sLine);
                                MapFlag.nINCHPPOINT = HUtil32.StrToInt(HUtil32.GetValidStr3(sLine, ref sLine, HUtil32.Backslash), -1);
                                MapFlag.nINCHPTIME = HUtil32.StrToInt(sLine, -1);
                                continue;
                            }
                            if (HUtil32.CompareLStr(sCommand, "DECGAMEGOLD"))
                            {
                                MapFlag.boDECGAMEGOLD = true;
                                HUtil32.ArrestStringEx(sCommand, "(", ")", ref sLine);
                                MapFlag.nDECGAMEGOLD = HUtil32.StrToInt(HUtil32.GetValidStr3(sLine, ref sLine, HUtil32.Backslash), -1);
                                MapFlag.nDECGAMEGOLDTIME = HUtil32.StrToInt(sLine, -1);
                                continue;
                            }
                            if (HUtil32.CompareLStr(sCommand, "DECGAMEPOINT"))
                            {
                                MapFlag.boDECGAMEPOINT = true;
                                HUtil32.ArrestStringEx(sCommand, "(", ")", ref sLine);
                                MapFlag.nDECGAMEPOINT = HUtil32.StrToInt(HUtil32.GetValidStr3(sLine, ref sLine, HUtil32.Backslash), -1);
                                MapFlag.nDECGAMEPOINTTIME = HUtil32.StrToInt(sLine, -1);
                                continue;
                            }
                            if (HUtil32.CompareLStr(sCommand, "INCGAMEGOLD"))
                            {
                                MapFlag.boINCGAMEGOLD = true;
                                HUtil32.ArrestStringEx(sCommand, "(", ")", ref sLine);
                                MapFlag.nINCGAMEGOLD = HUtil32.StrToInt(HUtil32.GetValidStr3(sLine, ref sLine, HUtil32.Backslash), -1);
                                MapFlag.nINCGAMEGOLDTIME = HUtil32.StrToInt(sLine, -1);
                                continue;
                            }
                            if (HUtil32.CompareLStr(sCommand, "INCGAMEPOINT"))
                            {
                                MapFlag.boINCGAMEPOINT = true;
                                HUtil32.ArrestStringEx(sCommand, "(", ")", ref sLine);
                                MapFlag.nINCGAMEPOINT = HUtil32.StrToInt(HUtil32.GetValidStr3(sLine, ref sLine, HUtil32.Backslash), -1);
                                MapFlag.nINCGAMEPOINTTIME = HUtil32.StrToInt(sLine, -1);
                                continue;
                            }
                            if (sCommand.Equals("RUNHUMAN", StringComparison.OrdinalIgnoreCase))
                            {
                                MapFlag.RunHuman = true;
                                continue;
                            }
                            if (sCommand.Equals("RUNMON", StringComparison.OrdinalIgnoreCase))
                            {
                                MapFlag.RunMon = true;
                                continue;
                            }
                            if (sCommand.Equals("NEEDHOLE", StringComparison.OrdinalIgnoreCase))
                            {
                                MapFlag.boNEEDHOLE = true;
                                continue;
                            }
                            if (sCommand.Equals("NORECALL", StringComparison.OrdinalIgnoreCase))
                            {
                                MapFlag.NoReCall = true;
                                continue;
                            }
                            if (sCommand.Equals("NOGUILDRECALL", StringComparison.OrdinalIgnoreCase))
                            {
                                MapFlag.NoGuildReCall = true;
                                continue;
                            }
                            if (sCommand.Equals("NODEARRECALL", StringComparison.OrdinalIgnoreCase))
                            {
                                MapFlag.boNODEARRECALL = true;
                                continue;
                            }
                            if (sCommand.Equals("NOMASTERRECALL", StringComparison.OrdinalIgnoreCase))
                            {
                                MapFlag.MasterReCall = true;
                                continue;
                            }
                            if (sCommand.Equals("NORANDOMMOVE", StringComparison.OrdinalIgnoreCase))
                            {
                                MapFlag.boNORANDOMMOVE = true;
                                continue;
                            }
                            if (sCommand.Equals("NODRUG", StringComparison.OrdinalIgnoreCase))
                            {
                                MapFlag.boNODRUG = true;
                                continue;
                            }
                            if (sCommand.Equals("MINE", StringComparison.OrdinalIgnoreCase))
                            {
                                MapFlag.Mine = true;
                                continue;
                            }
                            if (sCommand.Equals("MINE2", StringComparison.OrdinalIgnoreCase))
                            {
                                MapFlag.boMINE2 = true;
                                continue;
                            }
                            if (sCommand.Equals("NOTHROWITEM", StringComparison.OrdinalIgnoreCase))
                            {
                                MapFlag.NoThrowItem = true;
                                continue;
                            }
                            if (sCommand.Equals("NODROPITEM", StringComparison.OrdinalIgnoreCase))
                            {
                                MapFlag.NoDropItem = true;
                                continue;
                            }
                            if (sCommand.Equals("NOPOSITIONMOVE", StringComparison.OrdinalIgnoreCase))
                            {
                                MapFlag.boNOPOSITIONMOVE = true;
                                continue;
                            }
                            if (sCommand.Equals("NOHORSE", StringComparison.OrdinalIgnoreCase))
                            {
                                MapFlag.boNOPOSITIONMOVE = true;
                                continue;
                            }
                            if (sCommand.Equals("NOCHAT", StringComparison.OrdinalIgnoreCase))
                            {
                                MapFlag.boNOCHAT = true;
                                continue;
                            }
                            if (HUtil32.CompareLStr(sCommand, "KILLFUNC"))
                            {
                                MapFlag.boKILLFUNC = true;
                                HUtil32.ArrestStringEx(sCommand, "(", ")", ref sLine);
                                MapFlag.nKILLFUNCNO = HUtil32.StrToInt(sLine, -1);
                                continue;
                            }
                            if (HUtil32.CompareLStr(sCommand, "NOHUMNOMON"))
                            {
                                // 有人才开始刷怪
                                MapFlag.boNOHUMNOMON = true;
                                continue;
                            }
                            if (sCommand[0] == 'L')
                            {
                                MapFlag.RequestLevel = HUtil32.StrToInt(sCommand[1..], 1);
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
                        sFlag = HUtil32.GetValidStr3(sFlag, ref sCommand, HUtil32.Separator);
                        sMapName = sCommand;
                        sFlag = HUtil32.GetValidStr3(sFlag, ref sCommand, HUtil32.Separator);
                        int nX = HUtil32.StrToInt(sCommand, 0);
                        sFlag = HUtil32.GetValidStr3(sFlag, ref sCommand, HUtil32.Separator);
                        int n18 = HUtil32.StrToInt(sCommand, 0);
                        sFlag = HUtil32.GetValidStr3(sFlag, ref sCommand, new[] { ' ', ',', '-', '>', '\t' });
                        string s44 = sCommand;
                        sFlag = HUtil32.GetValidStr3(sFlag, ref sCommand, HUtil32.Separator);
                        int n1C = HUtil32.StrToInt(sCommand, 0);
                        sFlag = HUtil32.GetValidStr3(sFlag, ref sCommand, new[] { ' ', ',', ';', '\t' });
                        int n20 = HUtil32.StrToInt(sCommand, 0);
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
            string sFileName = M2Share.GetEnvirFilePath("MiniMap.txt");
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
                                M2Share.Logger.Error($"重复小地图配置信息[{sMapNO}]");
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
                NpcFlag = 0,
                Appr = 0,
                FilePath = "MapQuest_def",
                IsHide = true,
                IsQuest = false
            };
            M2Share.WorldEngine.QuestNpcList.Add(questNPC);
            return questNPC;
        }

        private static void LoadSubMapInfo(StringList loadList, string sFileName)
        {
            string sFileDir = M2Share.GetEnvirFilePath("MapInfo");
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