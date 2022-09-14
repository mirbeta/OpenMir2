using GameSvr.Actor;
using GameSvr.Monster;
using GameSvr.Npc;
using GameSvr.Script;
using System.Collections;
using SystemModule;
using SystemModule.Common;
using SystemModule.Data;
using SystemModule.Packet.ClientPackets;

namespace GameSvr
{
    public class TDefineInfo
    {
        public string sName;
        public string sText;
    }

    public class TQDDinfo
    {
        public int n00;
        public string s04;
        public ArrayList sList;
    }

    public class LocalDB
    {
        public bool LoadAdminList()
        {
            var sLineText = string.Empty;
            var sIPaddr = string.Empty;
            var sCharName = string.Empty;
            var sData = string.Empty;
            var sfilename = Path.Combine(M2Share.BasePath, M2Share.Config.EnvirDir, "AdminList.txt");
            if (!File.Exists(sfilename))
            {
                return false;
            }
            M2Share.WorldEngine.AdminList.Clear();
            using var LoadList = new StringList();
            LoadList.LoadFromFile(sfilename);
            for (var i = 0; i < LoadList.Count; i++)
            {
                sLineText = LoadList[i];
                var nLv = -1;
                if (sLineText != "" && sLineText[0] != ';')
                {
                    if (sLineText[0] == '*')
                    {
                        nLv = 10;
                    }
                    else if (sLineText[0] == '1')
                    {
                        nLv = 9;
                    }
                    else if (sLineText[0] == '2')
                    {
                        nLv = 8;
                    }
                    else if (sLineText[0] == '3')
                    {
                        nLv = 7;
                    }
                    else if (sLineText[0] == '4')
                    {
                        nLv = 6;
                    }
                    else if (sLineText[0] == '5')
                    {
                        nLv = 5;
                    }
                    else if (sLineText[0] == '6')
                    {
                        nLv = 4;
                    }
                    else if (sLineText[0] == '7')
                    {
                        nLv = 3;
                    }
                    else if (sLineText[0] == '8')
                    {
                        nLv = 2;
                    }
                    else if (sLineText[0] == '9')
                    {
                        nLv = 1;
                    }

                    if (nLv > 0)
                    {
                        sLineText = HUtil32.GetValidStrCap(sLineText, ref sData, new[] { "/", "\\", " ", "\t" });
                        sLineText = HUtil32.GetValidStrCap(sLineText, ref sCharName, new[] { "/", "\\", " ", "\t" });
                        sLineText = HUtil32.GetValidStrCap(sLineText, ref sIPaddr, new[] { "/", "\\", " ", "\t" });
                        if (string.IsNullOrEmpty(sCharName) || sIPaddr == "")
                        {
                            continue;
                        }
                        var AdminInfo = new TAdminInfo
                        {
                            nLv = nLv,
                            sChrName = sCharName,
                            sIPaddr = sIPaddr
                        };
                        M2Share.WorldEngine.AdminList.Add(AdminInfo);
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 读取守卫配置
        /// </summary>
        public void LoadGuardList()
        {
            try
            {
                var sLine = string.Empty;
                var monName = string.Empty;
                var mapName = string.Empty;
                var cX = string.Empty;
                var cY = string.Empty;
                var direction = string.Empty;
                var sFileName = Path.Combine(M2Share.BasePath, M2Share.Config.EnvirDir, "GuardList.txt");
                if (File.Exists(sFileName))
                {
                    var tGuardList = new StringList();
                    tGuardList.LoadFromFile(sFileName);
                    for (var i = 0; i < tGuardList.Count; i++)
                    {
                        sLine = tGuardList[i];
                        if (!string.IsNullOrEmpty(sLine) && sLine[0] != ';')
                        {
                            sLine = HUtil32.GetValidStrCap(sLine, ref monName, new[] { " " });
                            if (!string.IsNullOrEmpty(monName) && monName[0] == '\"')
                            {
                                HUtil32.ArrestStringEx(monName, "\"", "\"", ref monName);
                            }
                            sLine = HUtil32.GetValidStr3(sLine, ref mapName, new[] { ' ' });
                            sLine = HUtil32.GetValidStr3(sLine, ref cX, new[] { ' ', ',' });
                            sLine = HUtil32.GetValidStr3(sLine, ref cY, new[] { ' ', ',', ':' });
                            sLine = HUtil32.GetValidStr3(sLine, ref direction, new[] { ' ', ':' });
                            if (!string.IsNullOrEmpty(monName) && !string.IsNullOrEmpty(mapName) && !string.IsNullOrEmpty(direction))
                            {
                                var tGuard = M2Share.WorldEngine.RegenMonsterByName(mapName, (short)HUtil32.Str_ToInt(cX, 0), (short)HUtil32.Str_ToInt(cY, 0), monName);
                                if (tGuard != null)
                                {
                                    tGuard.Direction = (byte)HUtil32.Str_ToInt(direction, 0);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                M2Share.Log.Error(ex.StackTrace);
            }
        }

        /// <summary>
        /// 炼药列表
        /// </summary>
        public void LoadMakeItem()
        {
            int nItemCount;
            var sSubName = string.Empty;
            var sItemName = string.Empty;
            IList<MakeItem> List28 = null;
            var sFileName = Path.Combine(M2Share.BasePath, M2Share.Config.EnvirDir, "MakeItem.txt");
            if (File.Exists(sFileName))
            {
                using var LoadList = new StringList();
                LoadList.LoadFromFile(sFileName);
                for (var i = 0; i < LoadList.Count; i++)
                {
                    var sLine = LoadList[i].Trim();
                    if (string.IsNullOrEmpty(sLine) || sLine.StartsWith(";"))
                    {
                        continue;
                    }
                    if (sLine.StartsWith("["))
                    {
                        if (List28 != null)
                        {
                            M2Share.MakeItemList.Add(sItemName, List28);
                        }
                        List28 = new List<MakeItem>();
                        HUtil32.ArrestStringEx(sLine, "[", "]", ref sItemName);
                    }
                    else
                    {
                        if (List28 != null)
                        {
                            sLine = HUtil32.GetValidStr3(sLine, ref sSubName, new[] { " ", "\t" });
                            nItemCount = HUtil32.Str_ToInt(sLine.Trim(), 1);
                            List28.Add(new MakeItem() { ItemName = sSubName, ItemCount = nItemCount });
                        }
                    }
                }
                if (List28 != null)
                {
                    M2Share.MakeItemList.Add(sItemName, List28);
                }
            }
        }

        private void QFunctionNPC()
        {
            try
            {
                var sScriptFile = Path.Combine(M2Share.BasePath, M2Share.Config.EnvirDir, ScriptConst.sMarket_Def, "QFunction-0.txt");
                var sScritpDir = Path.Combine(M2Share.BasePath, M2Share.Config.EnvirDir, ScriptConst.sMarket_Def);
                if (!Directory.Exists(sScritpDir))
                {
                    Directory.CreateDirectory(sScritpDir);
                }
                if (!File.Exists(sScriptFile))
                {
                    var SaveList = new StringList();
                    SaveList.Add(";此脚为功能脚本，用于实现各种与脚本有关的功能");
                    SaveList.SaveToFile(sScriptFile);
                    SaveList = null;
                }
                if (File.Exists(sScriptFile))
                {
                    M2Share.g_FunctionNPC = new Merchant
                    {
                        MapName = "0",
                        CurrX = 0,
                        CurrY = 0,
                        CharName = "QFunction",
                        m_nFlag = 0,
                        Appr = 0,
                        m_sFilePath = ScriptConst.sMarket_Def,
                        m_sScript = "QFunction",
                        m_boIsHide = true,
                        m_boIsQuest = false
                    };
                    M2Share.WorldEngine.AddMerchant(M2Share.g_FunctionNPC);
                }
                else
                {
                    M2Share.g_FunctionNPC = null;
                }
            }
            catch
            {
                M2Share.g_FunctionNPC = null;
            }
        }

        private void QMangeNPC()
        {
            try
            {
                var sScriptFile = Path.Combine(M2Share.BasePath, M2Share.Config.EnvirDir, "MapQuest_def", "QManage.txt");
                var sScritpDir = Path.Combine(M2Share.BasePath, M2Share.Config.EnvirDir, "MapQuest_def");
                if (!Directory.Exists(sScritpDir))
                {
                    Directory.CreateDirectory(sScritpDir);
                }
                if (!File.Exists(sScriptFile))
                {
                    var sShowFile = HUtil32.ReplaceChar(sScriptFile, '\\', '/');
                    var SaveList = new StringList();
                    SaveList.Add(";此脚为登录脚本，人物每次登录时都会执行此脚本，所有人物初始设置都可以放在此脚本中。");
                    SaveList.Add(";修改脚本内容，可用@ReloadManage命令重新加载该脚本，不须重启程序。");
                    SaveList.Add("[@Login]");
                    SaveList.Add("#if");
                    SaveList.Add("#act");
                    SaveList.Add(";设置10倍杀怪经验");
                    SaveList.Add(";CANGETEXP 1 10");
                    SaveList.Add("#say");
                    SaveList.Add("游戏登录脚本运行成功，欢迎进入本游戏!!!\\ \\");
                    SaveList.Add("<关闭/@exit> \\ \\");
                    SaveList.Add("登录脚本文件位于: \\");
                    SaveList.Add(sShowFile + '\\');
                    SaveList.Add("脚本内容请自行按自己的要求修改。");
                    SaveList.SaveToFile(sScriptFile);
                    SaveList = null;
                }
                if (File.Exists(sScriptFile))
                {
                    M2Share.g_ManageNPC = new Merchant
                    {
                        MapName = "0",
                        CurrX = 0,
                        CurrY = 0,
                        CharName = "QManage",
                        m_nFlag = 0,
                        Appr = 0,
                        m_sFilePath = "MapQuest_def",
                        m_boIsHide = true,
                        m_boIsQuest = false
                    };
                    M2Share.WorldEngine.QuestNpcList.Add(M2Share.g_ManageNPC);
                }
                else
                {
                    M2Share.g_ManageNPC = null;
                }
            }
            catch
            {
                M2Share.g_ManageNPC = null;
            }
        }

        private void RobotNPC()
        {
            try
            {
                var sScriptFile = Path.Combine(M2Share.BasePath, M2Share.Config.EnvirDir, "Robot_def", "RobotManage.txt");
                var sScritpDir = Path.Combine(M2Share.BasePath, M2Share.Config.EnvirDir, "Robot_def");
                if (!Directory.Exists(sScritpDir))
                {
                    Directory.CreateDirectory(sScritpDir);
                }
                if (!File.Exists(sScriptFile))
                {
                    var tSaveList = new StringList();
                    tSaveList.Add(";此脚为机器人专用脚本，用于机器人处理功能用的脚本。");
                    tSaveList.SaveToFile(sScriptFile);
                    tSaveList = null;
                }
                if (File.Exists(sScriptFile))
                {
                    M2Share.g_RobotNPC = new Merchant
                    {
                        MapName = "0",
                        CurrX = 0,
                        CurrY = 0,
                        CharName = "RobotManage",
                        m_nFlag = 0,
                        Appr = 0,
                        m_sFilePath = "Robot_def",
                        m_boIsHide = true,
                        m_boIsQuest = false
                    };
                    M2Share.WorldEngine.QuestNpcList.Add(M2Share.g_RobotNPC);
                }
                else
                {
                    M2Share.g_RobotNPC = null;
                }
            }
            catch
            {
                M2Share.g_RobotNPC = null;
            }
        }

        /// <summary>
        /// 读取地图任务配置
        /// </summary>
        /// <returns></returns>
        public int LoadMapQuest()
        {
            var result = 1;
            var sMap = string.Empty;
            var s1C = string.Empty;
            var s20 = string.Empty;
            var sMonName = string.Empty;
            var sItem = string.Empty;
            var sQuest = string.Empty;
            var s30 = string.Empty;
            var s34 = string.Empty;
            var sFileName = Path.Combine(M2Share.BasePath, M2Share.Config.EnvirDir, "MapQuest.txt");
            if (File.Exists(sFileName))
            {
                var tMapQuestList = new StringList();
                tMapQuestList.LoadFromFile(sFileName);
                for (var i = 0; i < tMapQuestList.Count; i++)
                {
                    var tStr = tMapQuestList[i];
                    if (!string.IsNullOrEmpty(tStr) && tStr[0] != ';')
                    {
                        tStr = HUtil32.GetValidStr3(tStr, ref sMap, new[] { " ", "\t" });
                        tStr = HUtil32.GetValidStr3(tStr, ref s1C, new[] { " ", "\t" });
                        tStr = HUtil32.GetValidStr3(tStr, ref s20, new[] { " ", "\t" });
                        tStr = HUtil32.GetValidStr3(tStr, ref sMonName, new[] { " ", "\t" });
                        if (!string.IsNullOrEmpty(sMonName) && sMonName[0] == '\"')
                        {
                            HUtil32.ArrestStringEx(sMonName, "\"", "\"", ref sMonName);
                        }
                        tStr = HUtil32.GetValidStr3(tStr, ref sItem, new[] { " ", "\t" });
                        if (!string.IsNullOrEmpty(sItem) && sItem[0] == '\"')
                        {
                            HUtil32.ArrestStringEx(sItem, "\"", "\"", ref sItem);
                        }
                        tStr = HUtil32.GetValidStr3(tStr, ref sQuest, new[] { " ", "\t" });
                        tStr = HUtil32.GetValidStr3(tStr, ref s30, new[] { " ", "\t" });
                        if (!string.IsNullOrEmpty(sMap) && !string.IsNullOrEmpty(sMonName) && !string.IsNullOrEmpty(sQuest))
                        {
                            var Map = M2Share.MapMgr.FindMap(sMap);
                            if (Map != null)
                            {
                                HUtil32.ArrestStringEx(s1C, "[", "]", ref s34);
                                var n38 = HUtil32.Str_ToInt(s34, 0);
                                var n3C = HUtil32.Str_ToInt(s20, 0);
                                var boGrouped = HUtil32.CompareLStr(s30, "GROUP", "GROUP".Length);
                                if (!Map.CreateQuest(n38, n3C, sMonName, sItem, sQuest, boGrouped))
                                {
                                    result = -i;
                                }
                            }
                            else
                            {
                                result = -i;
                            }
                        }
                        else
                        {
                            result = -i;
                        }
                    }
                }
            }
            QMangeNPC();
            QFunctionNPC();
            RobotNPC();
            return result;
        }

        /// <summary>
        /// 读取交易商人配置
        /// </summary>
        public void LoadMerchant()
        {
            var sScript = string.Empty;
            var sMapName = string.Empty;
            var sX = string.Empty;
            var sY = string.Empty;
            var sName = string.Empty;
            var sFlag = string.Empty;
            var sAppr = string.Empty;
            var sIsCalste = string.Empty;
            var sCanMove = string.Empty;
            var sMoveTime = string.Empty;
            var sFileName = Path.Combine(M2Share.BasePath, M2Share.Config.EnvirDir, "Merchant.txt");
            if (File.Exists(sFileName))
            {
                var tMerchantList = new StringList();
                tMerchantList.LoadFromFile(sFileName);
                for (var i = 0; i < tMerchantList.Count; i++)
                {
                    var sLineText = tMerchantList[i].Trim();
                    if (!string.IsNullOrEmpty(sLineText) && sLineText[0] != ';')
                    {
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sScript, new[] { " ", "\t" });
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sMapName, new[] { " ", "\t" });
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sX, new[] { " ", "\t" });
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sY, new[] { " ", "\t" });
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sName, new[] { " ", "\t" });
                        if (!string.IsNullOrEmpty(sName) && sName[0] == '\"')
                        {
                            HUtil32.ArrestStringEx(sName, "\"", "\"", ref sName);
                        }
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sFlag, new[] { " ", "\t" });
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sAppr, new[] { " ", "\t" });
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sIsCalste, new[] { " ", "\t" });
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sCanMove, new[] { " ", "\t" });
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sMoveTime, new[] { " ", "\t" });
                        if (!string.IsNullOrEmpty(sScript) && !string.IsNullOrEmpty(sMapName) && !string.IsNullOrEmpty(sAppr))
                        {
                            var merchantNpc = new Merchant
                            {
                                m_sScript = sScript,
                                MapName = sMapName,
                                CurrX = (short)HUtil32.Str_ToInt(sX, 0),
                                CurrY = (short)HUtil32.Str_ToInt(sY, 0),
                                CharName = sName,
                                m_nFlag = (short)HUtil32.Str_ToInt(sFlag, 0),
                                Appr = (ushort)HUtil32.Str_ToInt(sAppr, 0),
                                m_dwMoveTime = HUtil32.Str_ToInt(sMoveTime, 0)
                            };
                            if (HUtil32.Str_ToInt(sIsCalste, 0) != 0)
                            {
                                merchantNpc.m_boCastle = true;
                            }
                            if (HUtil32.Str_ToInt(sCanMove, 0) != 0 && merchantNpc.m_dwMoveTime > 0)
                            {
                                merchantNpc.m_boCanMove = true;
                            }
                            M2Share.WorldEngine.AddMerchant(merchantNpc);
                        }
                    }
                }
            }
        }

        private void LoadMonGen_LoadMapGen(StringList MonGenList, string sFileName)
        {
            var sFileDir = Path.Combine(M2Share.BasePath, M2Share.Config.EnvirDir, "MonGen");
            if (!Directory.Exists(sFileDir))
            {
                Directory.CreateDirectory(sFileDir);
            }
            var sFilePatchName = sFileDir + sFileName;
            if (!File.Exists(sFilePatchName)) return;
            using var LoadList = new StringList();
            LoadList.LoadFromFile(sFilePatchName);
            for (var i = 0; i < LoadList.Count; i++)
            {
                MonGenList.Add(LoadList[i]);
            }
        }

        public int LoadMonGen()
        {
            var sLineText = string.Empty;
            var sData = string.Empty;
            int i;
            var result = 0;
            var sFileName = Path.Combine(M2Share.BasePath, M2Share.Config.EnvirDir, "MonGen.txt");
            if (File.Exists(sFileName))
            {
                using var LoadList = new StringList();
                LoadList.LoadFromFile(sFileName);
                i = 0;
                while (true)
                {
                    if (i >= LoadList.Count)
                    {
                        break;
                    }
                    if (HUtil32.CompareLStr("loadgen", LoadList[i], "loadgen".Length))
                    {
                        var sMapGenFile = HUtil32.GetValidStr3(LoadList[i], ref sLineText, new[] { " ", "\t" });
                        LoadList.RemoveAt(i);
                        if (!string.IsNullOrEmpty(sMapGenFile))
                        {
                            LoadMonGen_LoadMapGen(LoadList, sMapGenFile);
                        }
                    }
                    i++;
                }
                MonGenInfo MonGenInfo = null;
                for (i = 0; i < LoadList.Count; i++)
                {
                    sLineText = LoadList[i];
                    if (!string.IsNullOrEmpty(sLineText) && sLineText[0] != ';')
                    {
                        MonGenInfo = new MonGenInfo();
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sData, new[] { " ", "\t" });
                        MonGenInfo.sMapName = sData;
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sData, new[] { " ", "\t" });
                        MonGenInfo.nX = HUtil32.Str_ToInt(sData, 0);
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sData, new[] { " ", "\t" });
                        MonGenInfo.nY = HUtil32.Str_ToInt(sData, 0);
                        sLineText = HUtil32.GetValidStrCap(sLineText, ref sData, new[] { " ", "\t" });
                        if (!string.IsNullOrEmpty(sData) && sData[0] == '\"')
                        {
                            HUtil32.ArrestStringEx(sData, "\"", "\"", ref sData);
                        }
                        MonGenInfo.sMonName = sData;
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sData, new[] { " ", "\t" });
                        MonGenInfo.nRange = HUtil32.Str_ToInt(sData, 0);
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sData, new[] { " ", "\t" });
                        MonGenInfo.nCount = HUtil32.Str_ToInt(sData, 0);
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sData, new[] { " ", "\t" });
                        MonGenInfo.ZenTime = HUtil32.Str_ToInt(sData, -1) * 60 * 1000;
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sData, new[] { " ", "\t" });
                        MonGenInfo.nMissionGenRate = HUtil32.Str_ToInt(sData, 0);// 集中座标刷新机率 1 -100
                        if (!string.IsNullOrEmpty(MonGenInfo.sMapName) && !string.IsNullOrEmpty(MonGenInfo.sMonName) && MonGenInfo.ZenTime != 0 && 
                            M2Share.MapMgr.GetMapInfo(M2Share.ServerIndex, MonGenInfo.sMapName) != null)
                        {
                            MonGenInfo.CertList = new List<BaseObject>();
                            MonGenInfo.Envir = M2Share.MapMgr.FindMap(MonGenInfo.sMapName);
                            if (MonGenInfo.Envir != null)
                            {
                                M2Share.WorldEngine.MonGenList.Add(MonGenInfo);
                            }
                            else
                            {
                                MonGenInfo = null;
                            }
                        }
                    }
                }
                //MonGenInfo = new MonGenInfo
                //{
                //    CertList = new List<TBaseObject>(),
                //    Envir = null
                //};
                M2Share.WorldEngine.MonGenList.Add(MonGenInfo);
                result = 1;
            }
            return result;
        }

        /// <summary>
        /// 读取怪物物品掉落配置
        /// </summary>
        /// <returns></returns>
        public void LoadMonitems(string MonName, ref IList<TMonItem> ItemList)
        {
            var sData = string.Empty;
            var monFileName = Path.Combine(M2Share.BasePath, M2Share.Config.EnvirDir, "MonItems", $"{MonName}.txt");
            if (File.Exists(monFileName))
            {
                if (ItemList != null)
                {
                    for (var i = 0; i < ItemList.Count; i++)
                    {
                        ItemList[i] = null;
                    }
                    ItemList.Clear();
                }
                using var LoadList = new StringList();
                LoadList.LoadFromFile(monFileName);
                for (var i = 0; i < LoadList.Count; i++)
                {
                    var s28 = LoadList[i];
                    if (!string.IsNullOrEmpty(s28) && s28[0] != ';')
                    {
                        s28 = HUtil32.GetValidStr3(s28, ref sData, new[] { " ", "/", "\t" });
                        var n18 = HUtil32.Str_ToInt(sData, -1);
                        s28 = HUtil32.GetValidStr3(s28, ref sData, new[] { " ", "/", "\t" });
                        var n1C = HUtil32.Str_ToInt(sData, -1);
                        s28 = HUtil32.GetValidStr3(s28, ref sData, new[] { " ", "\t" });
                        if (sData != "")
                        {
                            if (sData[0] == '\"')
                            {
                                HUtil32.ArrestStringEx(sData, "\"", "\"", ref sData);
                            }
                        }
                        var itemName = sData;
                        s28 = HUtil32.GetValidStr3(s28, ref sData, new[] { " ", "\t" });
                        var itemCount = HUtil32.Str_ToInt(sData, 1);
                        if (n18 > 0 && n1C > 0 && !string.IsNullOrEmpty(itemName))
                        {
                            if (ItemList == null)
                            {
                                ItemList = new List<TMonItem>();
                            }
                            var MonItem = new TMonItem
                            {
                                SelPoint = n18 - 1,
                                MaxPoint = n1C,
                                ItemName = itemName,
                                Count = itemCount
                            };
                            ItemList.Add(MonItem);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 读取管理NPC配置
        /// </summary>
        public void LoadNpcs()
        {
            var sData = string.Empty;
            var charName = string.Empty;
            var type = string.Empty;
            var mapName = string.Empty;
            var cX = string.Empty;
            var cY = string.Empty;
            var flag = string.Empty;
            var appr = string.Empty;
            var sFileName = Path.Combine(M2Share.BasePath, M2Share.Config.EnvirDir, "Npcs.txt");
            if (File.Exists(sFileName))
            {
                using var LoadList = new StringList();
                LoadList.LoadFromFile(sFileName);
                for (var i = 0; i < LoadList.Count; i++)
                {
                    sData = LoadList[i].Trim();
                    if (!string.IsNullOrEmpty(sData) && sData[0] != ';')
                    {
                        sData = HUtil32.GetValidStrCap(sData, ref charName, new[] { " ", "\t" });
                        if (!string.IsNullOrEmpty(charName) && charName[0] == '\"')
                        {
                            HUtil32.ArrestStringEx(charName, "\"", "\"", ref charName);
                        }
                        sData = HUtil32.GetValidStr3(sData, ref type, new[] { " ", "\t" });
                        sData = HUtil32.GetValidStr3(sData, ref mapName, new[] { " ", "\t" });
                        sData = HUtil32.GetValidStr3(sData, ref cX, new[] { " ", "\t" });
                        sData = HUtil32.GetValidStr3(sData, ref cY, new[] { " ", "\t" });
                        sData = HUtil32.GetValidStr3(sData, ref flag, new[] { " ", "\t" });
                        sData = HUtil32.GetValidStr3(sData, ref appr, new[] { " ", "\t" });
                        if (!string.IsNullOrEmpty(charName) && !string.IsNullOrEmpty(mapName) && !string.IsNullOrEmpty(appr))
                        {
                            NormNpc NPC = null;
                            switch (HUtil32.Str_ToInt(type, 0))
                            {
                                case 0:
                                    NPC = new Merchant();
                                    break;
                                case 1:
                                    NPC = new GuildOfficial();
                                    break;
                                case 2:
                                    NPC = new CastleOfficial();
                                    break;
                            }
                            if (NPC != null)
                            {
                                NPC.MapName = mapName;
                                NPC.CurrX = (short)HUtil32.Str_ToInt(cX, 0);
                                NPC.CurrY = (short)HUtil32.Str_ToInt(cY, 0);
                                NPC.CharName = charName;
                                NPC.m_nFlag = (short)HUtil32.Str_ToInt(flag, 0);
                                NPC.Appr = (ushort)HUtil32.Str_ToInt(appr, 0);
                                M2Share.WorldEngine.QuestNpcList.Add(NPC);
                            }
                        }
                    }
                }
            }
        }

        private string LoadQuestDiary_sub_48978C(int nIndex)
        {
            string result;
            if (nIndex >= 1000)
            {
                result = nIndex.ToString();
                return result;
            }
            if (nIndex >= 100)
            {
                result = nIndex.ToString() + '0';
                return result;
            }
            result = nIndex + "00";
            return result;
        }

        public int LoadQuestDiary()
        {
            var result = 1;
            var s18 = string.Empty;
            var s1C = string.Empty;
            var s20 = string.Empty;
            var bo2D = false;
            var nC = 1;
            M2Share.QuestDiaryList.Clear();
            while (true)
            {
                IList<TQDDinfo> QDDinfoList = null;
                var sFileName = Path.Combine(M2Share.BasePath, M2Share.Config.EnvirDir, "QuestDiary", LoadQuestDiary_sub_48978C(nC) + ".txt");
                if (File.Exists(sFileName))
                {
                    s18 = string.Empty;
                    TQDDinfo QDDinfo = null;
                    using var LoadList = new StringList();
                    LoadList.LoadFromFile(sFileName);
                    for (var i = 0; i < LoadList.Count; i++)
                    {
                        s1C = LoadList[i];
                        if (!string.IsNullOrEmpty(s1C) && s1C[0] != ';')
                        {
                            if (s1C[0] == '[' && s1C.Length > 2)
                            {
                                if (string.IsNullOrEmpty(s18))
                                {
                                    HUtil32.ArrestStringEx(s1C, "[", "]", ref s18);
                                    QDDinfoList = new List<TQDDinfo>();
                                    QDDinfo = new TQDDinfo
                                    {
                                        n00 = nC,
                                        s04 = s18,
                                        sList = new ArrayList()
                                    };
                                    QDDinfoList.Add(QDDinfo);
                                    bo2D = true;
                                }
                                else
                                {
                                    if (s1C[0] != '@')
                                    {
                                        s1C = HUtil32.GetValidStr3(s1C, ref s20, new[] { " ", "\t" });
                                        HUtil32.ArrestStringEx(s20, "[", "]", ref s20);
                                        QDDinfo = new TQDDinfo
                                        {
                                            n00 = HUtil32.Str_ToInt(s20, 0),
                                            s04 = s1C,
                                            sList = new ArrayList()
                                        };
                                        QDDinfoList.Add(QDDinfo);
                                        bo2D = true;
                                    }
                                    else
                                    {
                                        bo2D = false;
                                    }
                                }
                            }
                            else
                            {
                                if (bo2D)
                                {
                                    QDDinfo.sList.Add(s1C);
                                }
                            }
                        }
                    }
                }
                if (QDDinfoList != null)
                {
                    M2Share.QuestDiaryList.Add(QDDinfoList);
                }
                else
                {
                    M2Share.QuestDiaryList.Add(null);
                }
                nC++;
                if (nC >= 105)
                {
                    break;
                }
            }
            return result;
        }

        public void LoadStartPoint()
        {
            var mapName = string.Empty;
            var cX = string.Empty;
            var cY = string.Empty;
            var allSay = string.Empty;
            var range = string.Empty;
            var type = string.Empty;
            var zone = string.Empty;
            var fire = string.Empty;
            var sFileName = Path.Combine(M2Share.BasePath, M2Share.Config.EnvirDir, "StartPoint.txt");
            if (File.Exists(sFileName))
            {
                M2Share.StartPointList.Clear();
                using var LoadList = new StringList();
                LoadList.LoadFromFile(sFileName);
                for (var i = 0; i < LoadList.Count; i++)
                {
                    var sLine = LoadList[i].Trim();
                    if (!string.IsNullOrEmpty(sLine) && sLine[0] != ';')
                    {
                        sLine = HUtil32.GetValidStr3(sLine, ref mapName, new[] { " ", "\t" });
                        sLine = HUtil32.GetValidStr3(sLine, ref cX, new[] { " ", "\t" });
                        sLine = HUtil32.GetValidStr3(sLine, ref cY, new[] { " ", "\t" });
                        sLine = HUtil32.GetValidStr3(sLine, ref allSay, new[] { " ", "\t" });
                        sLine = HUtil32.GetValidStr3(sLine, ref range, new[] { " ", "\t" });
                        sLine = HUtil32.GetValidStr3(sLine, ref type, new[] { " ", "\t" });
                        sLine = HUtil32.GetValidStr3(sLine, ref zone, new[] { " ", "\t" });
                        sLine = HUtil32.GetValidStr3(sLine, ref fire, new[] { " ", "\t" });
                        if (!string.IsNullOrEmpty(mapName) && !string.IsNullOrEmpty(cX) && cY != "")
                        {
                            var startPoint = new StartPoint
                            {
                                m_sMapName = mapName,
                                m_nCurrX = (short)HUtil32.Str_ToInt(cX, 0),
                                m_nCurrY = (short)HUtil32.Str_ToInt(cY, 0),
                                m_boNotAllowSay = Convert.ToBoolean(HUtil32.Str_ToInt(allSay, 0)),
                                m_nRange = HUtil32.Str_ToInt(range, 0),
                                m_nType = HUtil32.Str_ToInt(type, 0),
                                m_nPkZone = HUtil32.Str_ToInt(zone, 0),
                                m_nPkFire = HUtil32.Str_ToInt(fire, 0)
                            };
                            M2Share.StartPointList.Add(startPoint);
                        }
                    }
                }
            }
        }

        public int LoadUnbindList()
        {
            var result = 0;
            var tStr = string.Empty;
            var sData = string.Empty;
            var sItemName = string.Empty;
            int n10;
            var sFileName = Path.Combine(M2Share.BasePath, M2Share.Config.EnvirDir, "UnbindList.txt");
            if (File.Exists(sFileName))
            {
                using var LoadList = new StringList();
                LoadList.LoadFromFile(sFileName);
                for (var i = 0; i < LoadList.Count; i++)
                {
                    tStr = LoadList[i];
                    if (!string.IsNullOrEmpty(tStr) && tStr[0] != ';')
                    {
                        tStr = HUtil32.GetValidStr3(tStr, ref sData, new[] { " ", "\t" });
                        tStr = HUtil32.GetValidStrCap(tStr, ref sItemName, new[] { " ", "\t" });
                        if (!string.IsNullOrEmpty(sItemName) && sItemName[0] == '\"')
                        {
                            HUtil32.ArrestStringEx(sItemName, "\"", "\"", ref sItemName);
                        }
                        n10 = HUtil32.Str_ToInt(sData, 0);
                        if (n10 > 0)
                        {
                            if (M2Share.g_UnbindList.ContainsKey(n10))
                            {
                                M2Share.Log.Warn(string.Format("重复解包物品[{0}]...", sItemName));
                                continue;
                            }
                            M2Share.g_UnbindList.Add(n10, sItemName);
                        }
                        else
                        {
                            result = -i;// 需要取负数
                            break;
                        }
                    }
                }
            }
            return result;
        }

        public int SaveGoodRecord(Merchant NPC, string sFile)
        {
            var result = -1;
            var sFileName = ".\\Envir\\Market_Saved\\" + sFile + ".sav";
            //if (File.Exists(sFileName))
            //{
            //    FileHandle = File.Open(sFileName, (FileMode) FileAccess.Write | FileShare.ReadWrite);
            //}
            //else
            //{
            //    FileHandle = File.Create(sFileName);
            //}
            //if (FileHandle > 0)
            //{
            //    
            //    FillChar(Header420, sizeof(TGoodFileHeader), '\0');
            //    for (I = 0; I < NPC.m_GoodsList.Count; I ++ )
            //    {
            //        List = ((NPC.m_GoodsList[I]) as ArrayList);
            //        Header420.nItemCount += List.Count;
            //    }
            //    
            //    FileWrite(FileHandle, Header420, sizeof(TGoodFileHeader));
            //    for (I = 0; I < NPC.m_GoodsList.Count; I ++ )
            //    {
            //        List = ((NPC.m_GoodsList[I]) as ArrayList);
            //        for (II = 0; II < List.Count; II ++ )
            //        {
            //            UserItem = List[II];
            //            
            //            FileWrite(FileHandle, UserItem, sizeof(TUserItem));
            //        }
            //    }
            //    result = 1;
            //}
            return result;
        }

        public int SaveGoodPriceRecord(Merchant NPC, string sFile)
        {
            var result = -1;
            var sFileName = ".\\Envir\\Market_Prices\\" + sFile + ".prc";
            //if (File.Exists(sFileName))
            //{
            //    FileHandle = File.Open(sFileName, (FileMode) FileAccess.Write | FileShare.ReadWrite);
            //}
            //else
            //{
            //    FileHandle = File.Create(sFileName);
            //}
            //if (FileHandle > 0)
            //{
            //    
            //    FillChar(Header420, sizeof(TGoodFileHeader), '\0');
            //    Header420.nItemCount = NPC.m_ItemPriceList.Count;
            //    
            //    FileWrite(FileHandle, Header420, sizeof(TGoodFileHeader));
            //    for (I = 0; I < NPC.m_ItemPriceList.Count; I ++ )
            //    {
            //        ItemPrice = NPC.m_ItemPriceList[I];
            //        
            //        FileWrite(FileHandle, ItemPrice, sizeof(TItemPrice));
            //    }
            //    result = 1;
            //}
            return result;
        }

        public void ReLoadNpc()
        {

        }

        public void ReLoadMerchants()
        {
            var sScript = string.Empty;
            var sMapName = string.Empty;
            var sX = string.Empty;
            var sY = string.Empty;
            var sCharName = string.Empty;
            var sFlag = string.Empty;
            var sAppr = string.Empty;
            var sCastle = string.Empty;
            var sCanMove = string.Empty;
            var sMoveTime = string.Empty;
            Merchant Merchant;
            var sFileName = Path.Combine(M2Share.BasePath, M2Share.Config.EnvirDir, "Merchant.txt");
            if (!File.Exists(sFileName))
            {
                return;
            }
            for (var i = 0; i < M2Share.WorldEngine.MerchantList.Count; i++)
            {
                Merchant = M2Share.WorldEngine.MerchantList[i];
                if (Merchant != M2Share.g_FunctionNPC)
                {
                    Merchant.m_nFlag = -1;
                }
            }
            using var LoadList = new StringList();
            LoadList.LoadFromFile(sFileName);
            for (var i = 0; i < LoadList.Count; i++)
            {
                var sLineText = LoadList[i].Trim();
                if (!string.IsNullOrEmpty(sLineText) && sLineText[0] != ';')
                {
                    sLineText = HUtil32.GetValidStr3(sLineText, ref sScript, new[] { " ", "\t" });
                    sLineText = HUtil32.GetValidStr3(sLineText, ref sMapName, new[] { " ", "\t" });
                    sLineText = HUtil32.GetValidStr3(sLineText, ref sX, new[] { " ", "\t" });
                    sLineText = HUtil32.GetValidStr3(sLineText, ref sY, new[] { " ", "\t" });
                    sLineText = HUtil32.GetValidStr3(sLineText, ref sCharName, new[] { " ", "\t" });
                    if (sCharName != "" && sCharName[0] == '\"')
                    {
                        HUtil32.ArrestStringEx(sCharName, "\"", "\"", ref sCharName);
                    }
                    sLineText = HUtil32.GetValidStr3(sLineText, ref sFlag, new[] { " ", "\t" });
                    sLineText = HUtil32.GetValidStr3(sLineText, ref sAppr, new[] { " ", "\t" });
                    sLineText = HUtil32.GetValidStr3(sLineText, ref sCastle, new[] { " ", "\t" });
                    sLineText = HUtil32.GetValidStr3(sLineText, ref sCanMove, new[] { " ", "\t" });
                    sLineText = HUtil32.GetValidStr3(sLineText, ref sMoveTime, new[] { " ", "\t" });
                    var nX = HUtil32.Str_ToInt(sX, 0);
                    var nY = HUtil32.Str_ToInt(sY, 0);
                    var boNewNpc = true;
                    for (var j = 0; j < M2Share.WorldEngine.MerchantList.Count; j++)
                    {
                        Merchant = M2Share.WorldEngine.MerchantList[j];
                        if (Merchant.MapName == sMapName && Merchant.CurrX == nX && Merchant.CurrY == nY)
                        {
                            boNewNpc = false;
                            Merchant.m_sScript = sScript;
                            Merchant.CharName = sCharName;
                            Merchant.m_nFlag = (short)HUtil32.Str_ToInt(sFlag, 0);
                            Merchant.Appr = (ushort)HUtil32.Str_ToInt(sAppr, 0);
                            Merchant.m_dwMoveTime = HUtil32.Str_ToInt(sMoveTime, 0);
                            if (HUtil32.Str_ToInt(sCastle, 0) != 1)
                            {
                                Merchant.m_boCastle = true;
                            }
                            else
                            {
                                Merchant.m_boCastle = false;
                            }
                            if (HUtil32.Str_ToInt(sCanMove, 0) != 0 && Merchant.m_dwMoveTime > 0)
                            {
                                Merchant.m_boCanMove = true;
                            }
                            break;
                        }
                    }
                    if (boNewNpc)
                    {
                        Merchant = new Merchant
                        {
                            MapName = sMapName
                        };
                        Merchant.Envir = M2Share.MapMgr.FindMap(Merchant.MapName);
                        if (Merchant.Envir != null)
                        {
                            Merchant.m_sScript = sScript;
                            Merchant.CurrX = (short)nX;
                            Merchant.CurrY = (short)nY;
                            Merchant.CharName = sCharName;
                            Merchant.m_nFlag = (short)HUtil32.Str_ToInt(sFlag, 0);
                            Merchant.Appr = (ushort)HUtil32.Str_ToInt(sAppr, 0);
                            Merchant.m_dwMoveTime = HUtil32.Str_ToInt(sMoveTime, 0);
                            if (HUtil32.Str_ToInt(sCastle, 0) != 1)
                            {
                                Merchant.m_boCastle = true;
                            }
                            else
                            {
                                Merchant.m_boCastle = false;
                            }
                            if (HUtil32.Str_ToInt(sCanMove, 0) != 0 && Merchant.m_dwMoveTime > 0)
                            {
                                Merchant.m_boCanMove = true;
                            }
                            M2Share.WorldEngine.MerchantList.Add(Merchant);
                            Merchant.Initialize();
                        }
                    }
                }
            }
            for (var i = M2Share.WorldEngine.MerchantList.Count - 1; i >= 0; i--)
            {
                Merchant = M2Share.WorldEngine.MerchantList[i];
                if (Merchant.m_nFlag == -1)
                {
                    Merchant.Ghost = true;
                    Merchant.GhostTick = HUtil32.GetTickCount();
                    M2Share.WorldEngine.MerchantList.RemoveAt(i);
                }
            }
        }

        public int LoadUpgradeWeaponRecord(string sNPCName, IList<TUpgradeInfo> DataList)
        {
            //todo 加载武器升级数据
            return -1;
        }

        public int SaveUpgradeWeaponRecord(string sNPCName, IList<TUpgradeInfo> DataList)
        {
            //todo 保存武器升级数据
            return -1;
        }

        public int LoadGoodRecord(Merchant NPC, string sFile)
        {
            var result = -1;
            var sFileName = ".\\Envir\\Market_Saved\\" + sFile + ".sav";
            //if (File.Exists(sFileName))
            //{
            //    FileHandle = File.Open(sFileName, (FileMode) FileAccess.Read | FileShare.ReadWrite);
            //    List = null;
            //    if (FileHandle > 0)
            //    {
            //        
            //        if (FileRead(FileHandle, Header420, sizeof(TGoodFileHeader)) == sizeof(TGoodFileHeader))
            //        {
            //            for (I = 0; I < Header420.nItemCount; I ++ )
            //            {
            //                UserItem = new TUserItem();
            //                
            //                if (FileRead(FileHandle, UserItem, sizeof(TUserItem)) == sizeof(TUserItem))
            //                {
            //                    if (List == null)
            //                    {
            //                        List = new ArrayList();
            //                        List.Add(UserItem);
            //                    }
            //                    else
            //                    {
            //                        if (((TUserItem)(List[0])).wIndex == UserItem.wIndex)
            //                        {
            //                            List.Add(UserItem);
            //                        }
            //                        else
            //                        {
            //                            NPC.m_GoodsList.Add(List);
            //                            List = new ArrayList();
            //                            List.Add(UserItem);
            //                        }
            //                    }
            //                }
            //            }
            //            if (List != null)
            //            {
            //                NPC.m_GoodsList.Add(List);
            //            }
            //            FileHandle.Close();
            //            result = 1;
            //        }
            //    }
            //}
            return result;
        }

        public int LoadGoodPriceRecord(Merchant NPC, string sFile)
        {
            var result = -1;
            var sFileName = ".\\Envir\\Market_Prices\\" + sFile + ".prc";
            //if (File.Exists(sFileName))
            //{
            //    FileHandle = File.Open(sFileName, (FileMode)FileAccess.Read | FileShare.ReadWrite);
            //    if (FileHandle > 0)
            //    {
            //        @ Unsupported function or procedure: 'FileRead'
            //        if (FileRead(FileHandle, Header420, sizeof(TGoodFileHeader)) == sizeof(TGoodFileHeader))
            //        {
            //            for (I = 0; I < Header420.nItemCount; I++)
            //            {
            //                ItemPrice = new TItemPrice();
            //                @ Unsupported function or procedure: 'FileRead'
            //                if (FileRead(FileHandle, ItemPrice, sizeof(TItemPrice)) == sizeof(TItemPrice))
            //                {
            //                    NPC.m_ItemPriceList.Add(ItemPrice);
            //                }
            //                else
            //                {
            //                    @ Unsupported function or procedure: 'Dispose'
            //                    Dispose(ItemPrice);
            //                    break;
            //                }
            //            }
            //        }
            //        FileHandle.Close();
            //        result = 1;
            //    }
            //}
            return result;
        }
    }
}
