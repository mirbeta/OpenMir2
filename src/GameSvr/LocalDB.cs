using SystemModule;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SystemModule.Common;
using System.Text.Json;

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

    public struct TGoodFileHeader
    {
        public int nItemCount;
        public int[] Resv;
    }

    public class LocalDB
    {
        public bool LoadAdminList()
        {
            bool result = false;
            var sLineText = string.Empty;
            var sIPaddr = string.Empty;
            var sCharName = string.Empty;
            var sData = string.Empty;
            StringList LoadList;
            TAdminInfo AdminInfo;
            string sfilename = Path.Combine(M2Share.sConfigPath, M2Share.g_Config.sEnvirDir, "AdminList.txt");
            if (!File.Exists(sfilename))
            {
                return result;
            }
            M2Share.UserEngine.m_AdminList.Clear();
            LoadList = new StringList();
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
                        AdminInfo = new TAdminInfo
                        {
                            nLv = nLv,
                            sChrName = sCharName,
                            sIPaddr = sIPaddr
                        };
                        M2Share.UserEngine.m_AdminList.Add(AdminInfo);
                    }
                }
            }
            result = true;
            return result;
        }

        public bool SaveAdminList()
        {
            var sPermission = string.Empty;
            int nPermission;
            TAdminInfo AdminInfo;
            string sfilename = Path.Combine(M2Share.sConfigPath, M2Share.g_Config.sEnvirDir, "AdminList.txt");
            StringList Savelist = new StringList();
            for (var i = 0; i < M2Share.UserEngine.m_AdminList.Count; i++)
            {
                AdminInfo = M2Share.UserEngine.m_AdminList[i];
                nPermission = AdminInfo.nLv;
                if (nPermission == 10)
                {
                    sPermission = "*";
                }

                if (nPermission == 9)
                {
                    sPermission = "1";
                }

                if (nPermission == 8)
                {
                    sPermission = "2";
                }

                if (nPermission == 7)
                {
                    sPermission = "3";
                }

                if (nPermission == 6)
                {
                    sPermission = "4";
                }

                if (nPermission == 5)
                {
                    sPermission = "5";
                }

                if (nPermission == 4)
                {
                    sPermission = "6";
                }

                if (nPermission == 3)
                {
                    sPermission = "7";
                }

                if (nPermission == 2)
                {
                    sPermission = "8";
                }

                if (nPermission == 1)
                {
                    sPermission = "9";
                }

                Savelist.Add(sPermission + "\t" + AdminInfo.sChrName + "\t" + AdminInfo.sIPaddr);
            }
            Savelist.SaveToFile(sfilename);
            return true;
        }

        public void LoadGuardList()
        {
            try
            {
                var s14 = string.Empty;
                var s1C = string.Empty;
                var s20 = string.Empty;
                var s24 = string.Empty;
                var s28 = string.Empty;
                var s2C = string.Empty;
                StringList tGuardList;
                TBaseObject tGuard;
                var sfilename = Path.Combine(M2Share.sConfigPath, M2Share.g_Config.sEnvirDir, "GuardList.txt");
                if (File.Exists(sfilename))
                {
                    tGuardList = new StringList();
                    tGuardList.LoadFromFile(sfilename);
                    for (var i = 0; i < tGuardList.Count; i++)
                    {
                        s14 = tGuardList[i];
                        if (s14 != "" && s14[0] != ';')
                        {
                            s14 = HUtil32.GetValidStrCap(s14, ref s1C, new[] { " " });
                            if (s1C != "" && s1C[0] == '\"')
                            {
                                HUtil32.ArrestStringEx(s1C, '\"', '\"', ref s1C);
                            }
                            s14 = HUtil32.GetValidStr3(s14, ref s20, new[] { ' ' });
                            s14 = HUtil32.GetValidStr3(s14, ref s24, new[] { ' ', ',' });
                            s14 = HUtil32.GetValidStr3(s14, ref s28, new[] { ' ', ',', ':' });
                            s14 = HUtil32.GetValidStr3(s14, ref s2C, new[] { ' ', ':' });
                            if (s1C != "" && s20 != "" && s2C != "")
                            {
                                tGuard = M2Share.UserEngine.RegenMonsterByName(s20, (short)HUtil32.Str_ToInt(s24, 0), (short)HUtil32.Str_ToInt(s28, 0), s1C);
                                if (tGuard != null)
                                {
                                    tGuard.m_btDirection = (byte)HUtil32.Str_ToInt(s2C, 0);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
        }

        /// <summary>
        /// 炼药列表
        /// </summary>
        public void LoadMakeItem()
        {
            int nItemCount;
            var sLine = string.Empty;
            var sSubName = string.Empty;
            var sItemName = string.Empty;
            StringList LoadList;
            IList<TMakeItem> List28 = null;
            var sFileName = Path.Combine(M2Share.sConfigPath, M2Share.g_Config.sEnvirDir, "MakeItem.txt");
            if (File.Exists(sFileName))
            {
                LoadList = new StringList();
                LoadList.LoadFromFile(sFileName);
                for (var i = 0; i < LoadList.Count; i++)
                {
                    sLine = LoadList[i].Trim();
                    if (string.IsNullOrEmpty(sLine) || sLine.StartsWith(";"))
                    {
                        continue;
                    }
                    if (sLine.StartsWith("["))
                    {
                        if (List28 != null)
                        {
                            M2Share.g_MakeItemList.Add(sItemName, List28);
                        }
                        List28 = new List<TMakeItem>();
                        HUtil32.ArrestStringEx(sLine, '[', ']', ref sItemName);
                    }
                    else
                    {
                        if (List28 != null)
                        {
                            sLine = HUtil32.GetValidStr3(sLine, ref sSubName, new[] { " ", "\t" });
                            nItemCount = HUtil32.Str_ToInt(sLine.Trim(), 1);
                            List28.Add(new TMakeItem() { ItemName = sSubName, ItemCount = nItemCount });
                        }
                    }
                }
                if (List28 != null)
                {
                    M2Share.g_MakeItemList.Add(sItemName, List28);
                }
            }
        }

        private void QFunctionNPC()
        {
            try
            {
                var sScriptFile = Path.Combine(M2Share.sConfigPath, M2Share.g_Config.sEnvirDir, M2Share.sMarket_Def, "QFunction-0.txt");
                var sScritpDir = Path.Combine(M2Share.sConfigPath, M2Share.g_Config.sEnvirDir, M2Share.sMarket_Def);
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
                    M2Share.g_FunctionNPC = new TMerchant
                    {
                        m_sMapName = "0",
                        m_nCurrX = 0,
                        m_nCurrY = 0,
                        m_sCharName = "QFunction",
                        m_nFlag = 0,
                        m_wAppr = 0,
                        m_sFilePath = M2Share.sMarket_Def,
                        m_sScript = "QFunction",
                        m_boIsHide = true,
                        m_boIsQuest = false
                    };
                    M2Share.UserEngine.AddMerchant(M2Share.g_FunctionNPC);
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
                var sScriptFile = Path.Combine(M2Share.sConfigPath, M2Share.g_Config.sEnvirDir, "MapQuest_def", "QManage.txt");
                var sShowFile = HUtil32.ReplaceChar(sScriptFile, '\\', '/');
                var sScritpDir = Path.Combine(M2Share.sConfigPath, M2Share.g_Config.sEnvirDir, "MapQuest_def");
                if (!Directory.Exists(sScritpDir))
                {
                    Directory.CreateDirectory(sScritpDir);
                }
                if (!File.Exists(sScriptFile))
                {
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
                    M2Share.g_ManageNPC = new TMerchant
                    {
                        m_sMapName = "0",
                        m_nCurrX = 0,
                        m_nCurrY = 0,
                        m_sCharName = "QManage",
                        m_nFlag = 0,
                        m_wAppr = 0,
                        m_sFilePath = "MapQuest_def",
                        m_boIsHide = true,
                        m_boIsQuest = false
                    };
                    M2Share.UserEngine.QuestNPCList.Add(M2Share.g_ManageNPC);
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
                var sScriptFile = Path.Combine(M2Share.sConfigPath, M2Share.g_Config.sEnvirDir, "Robot_def", "RobotManage.txt");
                var sScritpDir = Path.Combine(M2Share.sConfigPath, M2Share.g_Config.sEnvirDir, "Robot_def");
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
                    M2Share.g_RobotNPC = new TMerchant
                    {
                        m_sMapName = "0",
                        m_nCurrX = 0,
                        m_nCurrY = 0,
                        m_sCharName = "RobotManage",
                        m_nFlag = 0,
                        m_wAppr = 0,
                        m_sFilePath = "Robot_def",
                        m_boIsHide = true,
                        m_boIsQuest = false
                    };
                    M2Share.UserEngine.QuestNPCList.Add(M2Share.g_RobotNPC);
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
            var sFileName = Path.Combine(M2Share.sConfigPath, M2Share.g_Config.sEnvirDir, "MapQuest.txt");
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
                        if (sMonName != "" && sMonName[0] == '\"')
                        {
                            HUtil32.ArrestStringEx(sMonName, "\"", "\"", ref sMonName);
                        }
                        tStr = HUtil32.GetValidStr3(tStr, ref sItem, new[] { " ", "\t" });
                        if (sItem != "" && sItem[0] == '\"')
                        {
                            HUtil32.ArrestStringEx(sItem, "\"", "\"", ref sItem);
                        }
                        tStr = HUtil32.GetValidStr3(tStr, ref sQuest, new[] { " ", "\t" });
                        tStr = HUtil32.GetValidStr3(tStr, ref s30, new[] { " ", "\t" });
                        if (!string.IsNullOrEmpty(sMap) && !string.IsNullOrEmpty(sMonName) && !string.IsNullOrEmpty(sQuest))
                        {
                            var Map = M2Share.g_MapManager.FindMap(sMap);
                            if (Map != null)
                            {
                                HUtil32.ArrestStringEx(s1C, "[", "]", ref s34);
                                var n38 = HUtil32.Str_ToInt(s34, 0);
                                var n3C = HUtil32.Str_ToInt(s20, 0);
                                bool boGrouped = HUtil32.CompareLStr(s30, "GROUP", "GROUP".Length);
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

        public void LoadMerchant()
        {
            var sLineText = string.Empty;
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
            var sFileName = Path.Combine(M2Share.sConfigPath, M2Share.g_Config.sEnvirDir, "Merchant.txt");
            if (File.Exists(sFileName))
            {
                var tMerchantList = new StringList();
                tMerchantList.LoadFromFile(sFileName);
                for (var i = 0; i < tMerchantList.Count; i++)
                {
                    sLineText = tMerchantList[i].Trim();
                    if (sLineText != "" && sLineText[0] != ';')
                    {
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sScript, new[] { " ", "\t" });
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sMapName, new[] { " ", "\t" });
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sX, new[] { " ", "\t" });
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sY, new[] { " ", "\t" });
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sName, new[] { " ", "\t" });
                        if (sName != "" && sName[0] == '\"')
                        {
                            HUtil32.ArrestStringEx(sName, '\"', '\"', ref sName);
                        }
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sFlag, new[] { " ", "\t" });
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sAppr, new[] { " ", "\t" });
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sIsCalste, new[] { " ", "\t" });
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sCanMove, new[] { " ", "\t" });
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sMoveTime, new[] { " ", "\t" });
                        if (sScript != "" && sMapName != "" && sAppr != "")
                        {
                            var tMerchantNPC = new TMerchant
                            {
                                m_sScript = sScript,
                                m_sMapName = sMapName,
                                m_nCurrX = (short)HUtil32.Str_ToInt(sX, 0),
                                m_nCurrY = (short)HUtil32.Str_ToInt(sY, 0),
                                m_sCharName = sName,
                                m_nFlag = (short)HUtil32.Str_ToInt(sFlag, 0),
                                m_wAppr = (ushort)HUtil32.Str_ToInt(sAppr, 0),
                                m_dwMoveTime = HUtil32.Str_ToInt(sMoveTime, 0)
                            };
                            if (HUtil32.Str_ToInt(sIsCalste, 0) != 0)
                            {
                                tMerchantNPC.m_boCastle = true;
                            }
                            if (HUtil32.Str_ToInt(sCanMove, 0) != 0 && tMerchantNPC.m_dwMoveTime > 0)
                            {
                                tMerchantNPC.m_boCanMove = true;
                            }
                            M2Share.UserEngine.AddMerchant(tMerchantNPC);
                        }
                    }
                }
                //tMerchantList.Free;
            }
        }

        private void LoadMonGen_LoadMapGen(StringList MonGenList, string sFileName)
        {
            var sFileDir = Path.Combine(M2Share.sConfigPath, M2Share.g_Config.sEnvirDir, "MonGen");
            if (!Directory.Exists(sFileDir))
            {
                Directory.CreateDirectory(sFileDir);
            }
            var sFilePatchName = sFileDir + sFileName;
            if (!File.Exists(sFilePatchName)) return;
            var LoadList = new StringList();
            LoadList.LoadFromFile(sFilePatchName);
            for (var i = 0; i < LoadList.Count; i++)
            {
                MonGenList.Add(LoadList[i]);
            }
            //LoadList.Free;
        }

        public int LoadMonGen()
        {
            var sLineText = string.Empty;
            var sData = string.Empty;
            int i;
            var result = 0;
            var sFileName = Path.Combine(M2Share.sConfigPath, M2Share.g_Config.sEnvirDir, "MonGen.txt");
            if (File.Exists(sFileName))
            {
                var LoadList = new StringList();
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
                        if (sMapGenFile != "")
                        {
                            LoadMonGen_LoadMapGen(LoadList, sMapGenFile);
                        }
                    }
                    i++;
                }
                TMonGenInfo MonGenInfo;
                for (i = 0; i < LoadList.Count; i++)
                {
                    sLineText = LoadList[i];
                    if (sLineText != "" && sLineText[0] != ';')
                    {
                        MonGenInfo = new TMonGenInfo();
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sData, new[] { " ", "\t" });
                        MonGenInfo.sMapName = sData;
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sData, new[] { " ", "\t" });
                        MonGenInfo.nX = HUtil32.Str_ToInt(sData, 0);
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sData, new[] { " ", "\t" });
                        MonGenInfo.nY = HUtil32.Str_ToInt(sData, 0);
                        sLineText = HUtil32.GetValidStrCap(sLineText, ref sData, new[] { " ", "\t" });
                        if (sData != "" && sData[0] == '\"')
                        {
                            HUtil32.ArrestStringEx(sData, "\"", "\"", ref sData);
                        }
                        MonGenInfo.sMonName = sData;
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sData, new[] { " ", "\t" });
                        MonGenInfo.nRange = HUtil32.Str_ToInt(sData, 0);
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sData, new[] { " ", "\t" });
                        MonGenInfo.nCount = HUtil32.Str_ToInt(sData, 0);
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sData, new[] { " ", "\t" });
                        MonGenInfo.dwZenTime = HUtil32.Str_ToInt(sData, -1) * 60 * 1000;
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sData, new[] { " ", "\t" });
                        MonGenInfo.nMissionGenRate = HUtil32.Str_ToInt(sData, 0);
                        // 集中座标刷新机率 1 -100
                        if (MonGenInfo.sMapName != "" && MonGenInfo.sMonName != "" && MonGenInfo.dwZenTime != 0 &&
                            M2Share.g_MapManager.GetMapInfo(M2Share.nServerIndex, MonGenInfo.sMapName) != null)
                        {
                            MonGenInfo.CertList = new List<TBaseObject>();
                            MonGenInfo.Envir = M2Share.g_MapManager.FindMap(MonGenInfo.sMapName);
                            if (MonGenInfo.Envir != null)
                            {
                                M2Share.UserEngine.m_MonGenList.Add(MonGenInfo);
                            }
                            else
                            {
                                MonGenInfo = null;
                            }
                        }
                    }
                }
                MonGenInfo = new TMonGenInfo
                {
                    sMapName = "",
                    sMonName = "",
                    CertList = new List<TBaseObject>(),
                    Envir = null
                };
                M2Share.UserEngine.m_MonGenList.Add(MonGenInfo);
                LoadList = null;
                result = 1;
            }
            return result;
        }

        public int LoadMonitems(string MonName, ref IList<TMonItem> ItemList)
        {
            StringList LoadList;
            var s30 = string.Empty;
            var result = 0;
            var s24 = Path.Combine(M2Share.sConfigPath, M2Share.g_Config.sEnvirDir, "MonItems", $"{MonName}.txt");
            if (File.Exists(s24))
            {
                if (ItemList != null)
                {
                    for (var i = 0; i < ItemList.Count; i++)
                    {
                        ItemList[i] = null;
                    }
                    ItemList.Clear();
                }
                LoadList = new StringList();
                LoadList.LoadFromFile(s24);
                for (var i = 0; i < LoadList.Count; i++)
                {
                    var s28 = LoadList[i];
                    if (s28 != "" && s28[0] != ';')
                    {
                        s28 = HUtil32.GetValidStr3(s28, ref s30, new[] { " ", "/", "\t" });
                        var n18 = HUtil32.Str_ToInt(s30, -1);
                        s28 = HUtil32.GetValidStr3(s28, ref s30, new[] { " ", "/", "\t" });
                        var n1C = HUtil32.Str_ToInt(s30, -1);
                        s28 = HUtil32.GetValidStr3(s28, ref s30, new[] { " ", "\t" });
                        if (s30 != "")
                        {
                            if (s30[0] == '\"')
                            {
                                HUtil32.ArrestStringEx(s30, "\"", "\"", ref s30);
                            }
                        }
                        var s2C = s30;
                        s28 = HUtil32.GetValidStr3(s28, ref s30, new[] { " ", "\t" });
                        var n20 = HUtil32.Str_ToInt(s30, 1);
                        if (n18 > 0 && n1C > 0 && s2C != "")
                        {
                            if (ItemList == null)
                            {
                                ItemList = new List<TMonItem>();
                            }
                            var MonItem = new TMonItem
                            {
                                SelPoint = n18 - 1,
                                MaxPoint = n1C,
                                ItemName = s2C,
                                Count = n20
                            };
                            ItemList.Add(MonItem);
                            result++;
                        }
                    }
                }
                //LoadList.Free;
            }
            return result;
        }

        public void LoadNpcs()
        {
            var s10 = string.Empty;
            var s18 = string.Empty;
            var s1C = string.Empty;
            var s20 = string.Empty;
            var s24 = string.Empty;
            var s28 = string.Empty;
            var s2C = string.Empty;
            var s30 = string.Empty;
            var s34 = string.Empty;
            var s38 = string.Empty;
            StringList LoadList;
            TNormNpc NPC;
            string sFileName = Path.Combine(M2Share.sConfigPath, M2Share.g_Config.sEnvirDir, "Npcs.txt");
            if (File.Exists(sFileName))
            {
                LoadList = new StringList();
                LoadList.LoadFromFile(sFileName);
                for (var i = 0; i < LoadList.Count; i++)
                {
                    s18 = LoadList[i].Trim();
                    if (s18 != "" && s18[0] != ';')
                    {
                        s18 = HUtil32.GetValidStrCap(s18, ref s20, new[] { " ", "\t" });
                        if (s20 != "" && s20[0] == '\"')
                        {
                            HUtil32.ArrestStringEx(s20, "\"", "\"", ref s20);
                        }
                        s18 = HUtil32.GetValidStr3(s18, ref s24, new[] { " ", "\t" });
                        s18 = HUtil32.GetValidStr3(s18, ref s28, new[] { " ", "\t" });
                        s18 = HUtil32.GetValidStr3(s18, ref s2C, new[] { " ", "\t" });
                        s18 = HUtil32.GetValidStr3(s18, ref s30, new[] { " ", "\t" });
                        s18 = HUtil32.GetValidStr3(s18, ref s34, new[] { " ", "\t" });
                        s18 = HUtil32.GetValidStr3(s18, ref s38, new[] { " ", "\t" });
                        if (s20 != "" && s28 != "" && s38 != "")
                        {
                            NPC = null;
                            switch (HUtil32.Str_ToInt(s24, 0))
                            {
                                case 0:
                                    NPC = new TMerchant();
                                    break;
                                case 1:
                                    NPC = new TGuildOfficial();
                                    break;
                                case 2:
                                    NPC = new TCastleOfficial();
                                    break;
                            }
                            if (NPC != null)
                            {
                                NPC.m_sMapName = s28;
                                NPC.m_nCurrX = (short)HUtil32.Str_ToInt(s2C, 0);
                                NPC.m_nCurrY = (short)HUtil32.Str_ToInt(s30, 0);
                                NPC.m_sCharName = s20;
                                NPC.m_nFlag = (short)HUtil32.Str_ToInt(s34, 0);
                                NPC.m_wAppr = (ushort)HUtil32.Str_ToInt(s38, 0);
                                M2Share.UserEngine.QuestNPCList.Add(NPC);

                            }
                        }
                    }
                }
                LoadList = null;
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
            int result= 1;
            IList<TQDDinfo> QDDinfoList;
            TQDDinfo QDDinfo;
            var s14 = string.Empty;
            var s18 = string.Empty;
            var s1C = string.Empty;
            var s20 = string.Empty;
            StringList LoadList;
            var bo2D = false;
            var nC = 1;
            M2Share.QuestDiaryList.Clear();
            while (true)
            {
                QDDinfoList = null;
                s14 = Path.Combine(M2Share.sConfigPath, M2Share.g_Config.sEnvirDir, "QuestDiary", LoadQuestDiary_sub_48978C(nC) + ".txt");
                if (File.Exists(s14))
                {
                    s18 = "";
                    QDDinfo = null;
                    LoadList = new StringList();
                    LoadList.LoadFromFile(s14);
                    for (var i = 0; i < LoadList.Count; i++)
                    {
                        s1C = LoadList[i];
                        if (s1C != "" && s1C[0] != ';')
                        {
                            if (s1C[0] == '[' && s1C.Length > 2)
                            {
                                if (s18 == "")
                                {
                                    HUtil32.ArrestStringEx(s1C, '[', ']', ref s18);
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
                    LoadList = null;
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
            var tStr = string.Empty;
            var s18 = string.Empty;
            var s1C = string.Empty;
            var s20 = string.Empty;
            var s22 = string.Empty;
            var s24 = string.Empty;
            var s26 = string.Empty;
            var s28 = string.Empty;
            var s30 = string.Empty;
            StringList LoadList;
            TStartPoint StartPoint;
            var sFileName = Path.Combine(M2Share.sConfigPath, M2Share.g_Config.sEnvirDir, "StartPoint.txt");
            if (File.Exists(sFileName))
            {
                M2Share.StartPointList.Clear();
                LoadList = new StringList();
                LoadList.LoadFromFile(sFileName);
                for (var i = 0; i < LoadList.Count; i++)
                {
                    tStr = LoadList[i].Trim();
                    if (tStr != "" && tStr[0] != ';')
                    {
                        tStr = HUtil32.GetValidStr3(tStr, ref s18, new[] { " ", "\t" });
                        tStr = HUtil32.GetValidStr3(tStr, ref s1C, new[] { " ", "\t" });
                        tStr = HUtil32.GetValidStr3(tStr, ref s20, new[] { " ", "\t" });
                        tStr = HUtil32.GetValidStr3(tStr, ref s22, new[] { " ", "\t" });
                        tStr = HUtil32.GetValidStr3(tStr, ref s24, new[] { " ", "\t" });
                        tStr = HUtil32.GetValidStr3(tStr, ref s26, new[] { " ", "\t" });
                        tStr = HUtil32.GetValidStr3(tStr, ref s28, new[] { " ", "\t" });
                        tStr = HUtil32.GetValidStr3(tStr, ref s30, new[] { " ", "\t" });
                        if (s18 != "" && s1C != "" && s20 != "")
                        {
                            StartPoint = new TStartPoint
                            {
                                m_sMapName = s18,
                                m_nCurrX = (short)HUtil32.Str_ToInt(s1C, 0),
                                m_nCurrY = (short)HUtil32.Str_ToInt(s20, 0),
                                m_boNotAllowSay = Convert.ToBoolean(HUtil32.Str_ToInt(s22, 0)),
                                m_nRange = HUtil32.Str_ToInt(s24, 0),
                                m_nType = HUtil32.Str_ToInt(s26, 0),
                                m_nPkZone = HUtil32.Str_ToInt(s28, 0),
                                m_nPkFire = HUtil32.Str_ToInt(s30, 0)
                            };
                            M2Share.StartPointList.Add(StartPoint);
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
            StringList LoadList;
            int n10;
            var sFileName = Path.Combine(M2Share.sConfigPath, M2Share.g_Config.sEnvirDir, "UnbindList.txt");
            if (File.Exists(sFileName))
            {
                LoadList = new StringList();
                LoadList.LoadFromFile(sFileName);
                for (var i = 0; i < LoadList.Count; i++)
                {
                    tStr = LoadList[i];
                    if (tStr != "" && tStr[0] != ';')
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
                                Console.WriteLine("重复解包物品[{0}]...", sItemName);
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

        public int SaveGoodRecord(TMerchant NPC, string sFile)
        {
            int result;
            string sFileName;
            result = -1;
            sFileName = ".\\Envir\\Market_Saved\\" + sFile + ".sav";
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
            //    FileHandle.Close();
            //    result = 1;
            //}
            return result;
        }

        public int SaveGoodPriceRecord(TMerchant NPC, string sFile)
        {
            int result;
            string sFileName;
            result = -1;
            sFileName = ".\\Envir\\Market_Prices\\" + sFile + ".prc";
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
            //    FileHandle.Close();
            //    result = 1;
            //}
            return result;
        }

        public void ReLoadNpc()
        {

        }

        public void ReLoadMerchants()
        {
            int nX;
            int nY;
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
            TMerchant Merchant;
            StringList LoadList;
            bool boNewNpc;
            var sFileName = Path.Combine(M2Share.sConfigPath, M2Share.g_Config.sEnvirDir, "Merchant.txt");
            if (!File.Exists(sFileName))
            {
                return;
            }
                for (var i = 0; i < M2Share.UserEngine.m_MerchantList.Count; i++)
                {
                    Merchant = M2Share.UserEngine.m_MerchantList[i];
                    if (Merchant != M2Share.g_FunctionNPC)
                    {
                        Merchant.m_nFlag = -1;
                    }
                }
                LoadList = new StringList();
                LoadList.LoadFromFile(sFileName);
                for (var i = 0; i < LoadList.Count; i++)
                {
                    var sLineText = LoadList[i].Trim();
                    if (sLineText != "" && sLineText[0] != ';')
                    {
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sScript, new[] { " ", "\t" });
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sMapName, new[] { " ", "\t" });
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sX, new[] { " ", "\t" });
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sY, new[] { " ", "\t" });
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sCharName, new[] { " ", "\t" });
                        if (sCharName != "" && sCharName[0] == '\"')
                        {
                            HUtil32.ArrestStringEx(sCharName, '\"', '\"', ref sCharName);
                        }
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sFlag, new[] { " ", "\t" });
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sAppr, new[] { " ", "\t" });
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sCastle, new[] { " ", "\t" });
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sCanMove, new[] { " ", "\t" });
                        sLineText = HUtil32.GetValidStr3(sLineText, ref sMoveTime, new[] { " ", "\t" });
                        nX = HUtil32.Str_ToInt(sX, 0);
                        nY = HUtil32.Str_ToInt(sY, 0);
                        boNewNpc = true;
                        for (var j = 0; j < M2Share.UserEngine.m_MerchantList.Count; j++)
                        {
                            Merchant = M2Share.UserEngine.m_MerchantList[j];
                            if (Merchant.m_sMapName == sMapName && Merchant.m_nCurrX == nX && Merchant.m_nCurrY == nY)
                            {
                                boNewNpc = false;
                                Merchant.m_sScript = sScript;
                                Merchant.m_sCharName = sCharName;
                                Merchant.m_nFlag = (short)HUtil32.Str_ToInt(sFlag, 0);
                                Merchant.m_wAppr = (ushort)HUtil32.Str_ToInt(sAppr, 0);
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
                            Merchant = new TMerchant
                            {
                                m_sMapName = sMapName
                            };
                            Merchant.m_PEnvir = M2Share.g_MapManager.FindMap(Merchant.m_sMapName);
                            if (Merchant.m_PEnvir != null)
                            {
                                Merchant.m_sScript = sScript;
                                Merchant.m_nCurrX = (short)nX;
                                Merchant.m_nCurrY = (short)nY;
                                Merchant.m_sCharName = sCharName;
                                Merchant.m_nFlag = (short)HUtil32.Str_ToInt(sFlag, 0);
                                Merchant.m_wAppr = (ushort)HUtil32.Str_ToInt(sAppr, 0);
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
                                M2Share.UserEngine.m_MerchantList.Add(Merchant);
                                Merchant.Initialize();
                            }
                        }
                    }
                }
                for (var i = M2Share.UserEngine.m_MerchantList.Count - 1; i >= 0; i--)
                {
                    Merchant = M2Share.UserEngine.m_MerchantList[i];
                    if (Merchant.m_nFlag == -1)
                    {
                        Merchant.m_boGhost = true;
                        Merchant.m_dwGhostTick = HUtil32.GetTickCount();
                        M2Share.UserEngine.m_MerchantList.RemoveAt(i);
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

        public int LoadGoodRecord(TMerchant NPC, string sFile)
        {
            int result;
            string sFileName;
            result = -1;
            sFileName = ".\\Envir\\Market_Saved\\" + sFile + ".sav";
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

        public int LoadGoodPriceRecord(TMerchant NPC, string sFile)
        {
            int result;
            string sFileName;
            result = -1;
            sFileName = ".\\Envir\\Market_Prices\\" + sFile + ".prc";
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
