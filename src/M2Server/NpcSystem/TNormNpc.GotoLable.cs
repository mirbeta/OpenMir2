using System;
using System.Collections.Generic;
using System.IO;
using SystemModule;
using SystemModule.Common;

namespace M2Server
{
    public partial class TNormNpc
    {
        private bool GotoLable_CheckQuestStatus(TPlayObject PlayObject, TScript ScriptInfo)
        {
            bool result = true;
            int I;
            if (!ScriptInfo.boQuest)
            {
                return result;
            }
            I = 0;
            while (true)
            {
                if ((ScriptInfo.QuestInfo[I].nRandRage > 0) && (M2Share.RandomNumber.Random(ScriptInfo.QuestInfo[I].nRandRage) != 0))
                {
                    result = false;
                    break;
                }
                if (PlayObject.GetQuestFalgStatus(ScriptInfo.QuestInfo[I].wFlag) != ScriptInfo.QuestInfo[I].btValue)
                {
                    result = false;
                    break;
                }
                I++;
                if (I >= 10)
                {
                    break;
                }
            }
            return result;
        }

        public TUserItem GotoLable_CheckItemW(TPlayObject PlayObject, string sItemType, int nParam)
        {
            TUserItem result = null;
            int nCount = 0;
            if (HUtil32.CompareLStr(sItemType, "[NECKLACE]", 4))
            {
                if (PlayObject.m_UseItems[Grobal2.U_NECKLACE].wIndex > 0)
                {
                    result = PlayObject.m_UseItems[Grobal2.U_NECKLACE];
                }
                return result;
            }
            if (HUtil32.CompareLStr(sItemType, "[RING]", 4))
            {
                if (PlayObject.m_UseItems[Grobal2.U_RINGL].wIndex > 0)
                {
                    result = PlayObject.m_UseItems[Grobal2.U_RINGL];
                }
                if (PlayObject.m_UseItems[Grobal2.U_RINGR].wIndex > 0)
                {
                    result = PlayObject.m_UseItems[Grobal2.U_RINGR];
                }
                return result;
            }
            if (HUtil32.CompareLStr(sItemType, "[ARMRING]", 4))
            {
                if (PlayObject.m_UseItems[Grobal2.U_ARMRINGL].wIndex > 0)
                {
                    result = PlayObject.m_UseItems[Grobal2.U_ARMRINGL];
                }
                if (PlayObject.m_UseItems[Grobal2.U_ARMRINGR].wIndex > 0)
                {
                    result = PlayObject.m_UseItems[Grobal2.U_ARMRINGR];
                }
                return result;
            }
            if (HUtil32.CompareLStr(sItemType, "[WEAPON]", 4))
            {
                if (PlayObject.m_UseItems[Grobal2.U_WEAPON].wIndex > 0)
                {
                    result = PlayObject.m_UseItems[Grobal2.U_WEAPON];
                }
                return result;
            }
            if (HUtil32.CompareLStr(sItemType, "[HELMET]", 4))
            {
                if (PlayObject.m_UseItems[Grobal2.U_HELMET].wIndex > 0)
                {
                    result = PlayObject.m_UseItems[Grobal2.U_HELMET];
                }
                return result;
            }
            if (HUtil32.CompareLStr(sItemType, "[BUJUK]", 4))
            {
                if (PlayObject.m_UseItems[Grobal2.U_BUJUK].wIndex > 0)
                {
                    result = PlayObject.m_UseItems[Grobal2.U_BUJUK];
                }
                return result;
            }
            if (HUtil32.CompareLStr(sItemType, "[BELT]", 4))
            {
                if (PlayObject.m_UseItems[Grobal2.U_BELT].wIndex > 0)
                {
                    result = PlayObject.m_UseItems[Grobal2.U_BELT];
                }
                return result;
            }
            if (HUtil32.CompareLStr(sItemType, "[BOOTS]", 4))
            {
                if (PlayObject.m_UseItems[Grobal2.U_BOOTS].wIndex > 0)
                {
                    result = PlayObject.m_UseItems[Grobal2.U_BOOTS];
                }
                return result;
            }
            if (HUtil32.CompareLStr(sItemType, "[CHARM]", 4))
            {
                if (PlayObject.m_UseItems[Grobal2.U_CHARM].wIndex > 0)
                {
                    result = PlayObject.m_UseItems[Grobal2.U_CHARM];
                }
                return result;
            }
            result = PlayObject.sub_4C4CD4(sItemType, ref nCount);
            if (nCount < nParam)
            {
                result = null;
            }
            return result;
        }

        private bool GotoLable_CheckStringList(string sHumName, string sListFileName)
        {
            bool result = false;
            StringList LoadList;
            sListFileName = M2Share.g_Config.sEnvirDir + sListFileName;
            if (File.Exists(sListFileName))
            {
                LoadList = new StringList();
                try
                {
                    LoadList.LoadFromFile(sListFileName);
                }
                catch
                {
                    M2Share.MainOutMessage("loading fail.... => " + sListFileName);
                }
                for (var i = 0; i < LoadList.Count; i++)
                {
                    if (LoadList[i].Trim().ToLower().CompareTo(sHumName.ToLower()) == 0)
                    {
                        result = true;
                        break;
                    }
                }
                //LoadList.Free;
            }
            else
            {
                M2Share.MainOutMessage("file not found => " + sListFileName);
            }
            return result;
        }

        private void GotoLable_QuestCheckCondition_SetVal(TPlayObject PlayObject, string sIndex, int nCount)
        {
            var n14 = M2Share.GetValNameNo(sIndex);
            if (n14 >= 0)
            {
                //根据不同的索引进行赋值
                if (HUtil32.RangeInDefined(n14, 0, 9))
                {
                    PlayObject.m_nVal[n14] = nCount;
                }
                else if (HUtil32.RangeInDefined(n14, 100, 119))
                {
                    M2Share.g_Config.GlobalVal[n14 - 100] = nCount;
                }
                else  if (HUtil32.RangeInDefined(n14, 200, 209))
                {
                    PlayObject.m_DyVal[n14 - 200] = nCount;
                }
                else if (HUtil32.RangeInDefined(n14, 300, 399))
                {
                    PlayObject.m_nMval[n14 - 300] = nCount;
                }
                else if (HUtil32.RangeInDefined(n14, 400, 499))
                {
                    M2Share.g_Config.GlobaDyMval[n14 - 400] = (short)nCount;
                }
                else if (HUtil32.RangeInDefined(n14, 500, 599))
                {
                    PlayObject.m_nSval[n14 - 600] = nCount.ToString();
                }
            }
        }

        public bool GotoLable_QuestCheckCondition_CheckDieMon(TPlayObject PlayObject, string MonName)
        {
            bool result = false;
            if (string.IsNullOrEmpty(MonName))
            {
                result = true;
            }
            if ((PlayObject.m_LastHiter != null) && (PlayObject.m_LastHiter.m_sCharName == MonName))
            {
                result = true;
            }
            return result;
        }

        public bool GotoLable_QuestCheckCondition_CheckKillMon(TPlayObject PlayObject, string MonName)
        {
            bool result = false;
            if (MonName == "")
            {
                result = true;
            }
            if ((PlayObject.m_TargetCret != null) && (PlayObject.m_TargetCret.m_sCharName == MonName))
            {
                result = true;
            }
            return result;
        }

        public bool GotoLable_QuestCheckCondition_CheckRandomNo(TPlayObject PlayObject, string sNumber)
        {
            return PlayObject.m_sRandomNo == sNumber;
        }

        public bool GotoLable_QuestCheckCondition_CheckUserDateType(TPlayObject PlayObject, string charName, string sListFileName, string sDay, string param1, string param2)
        {
            bool result;
            int nDay;
            int UseDay;
            int LastDay;
            DateTime nnday;
            int i;
            StringList LoadList;
            string sText = string.Empty;
            string Name = string.Empty;
            string ssDay = string.Empty;
            result = false;
            sListFileName = M2Share.g_Config.sEnvirDir + sListFileName;
            LoadList = new StringList();
            try
            {
                if (File.Exists(sListFileName))
                {
                    try
                    {
                        LoadList.LoadFromFile(sListFileName);
                    }
                    catch
                    {
                        M2Share.MainOutMessage("loading fail.... => " + sListFileName);
                    }
                }
                nDay = HUtil32.Str_ToInt(sDay, 0);
                for (i = 0; i < LoadList.Count; i++)
                {
                    sText = LoadList[i].Trim();
                    sText = HUtil32.GetValidStrCap(sText, ref Name, new string[] { " ", "\t" });
                    Name = Name.Trim();
                    if (charName == Name)
                    {
                        ssDay = sText.Trim();
                        nnday = HUtil32.Str_ToDate(ssDay);
                        UseDay = HUtil32.Round(DateTime.Today.ToOADate() - nnday.ToOADate());
                        LastDay = nDay - UseDay;
                        if (LastDay < 0)
                        {
                            result = true;
                            LastDay = 0;
                        }
                        GotoLable_QuestCheckCondition_SetVal(PlayObject, param1, UseDay);
                        GotoLable_QuestCheckCondition_SetVal(PlayObject, param2, LastDay);
                        return result;
                    }
                }
            }
            finally
            {
                //LoadList.Free;
            }
            return result;
        }

        private bool GotoLable_QuestCheckCondition(TPlayObject PlayObject, IList<TQuestConditionInfo> ConditionList, ref string sC, ref TUserItem UserItem)
        {
            bool result = true;
            TQuestConditionInfo QuestConditionInfo;
            int n10 = 0;
            int n14 = 0;
            int n18 = 0;
            int n1C = 0;
            int nMaxDura = 0;
            int nDura = 0;
            int Hour = 0;
            int Min = 0;
            int Sec = 0;
            int MSec = 0;
            TEnvirnoment Envir;
            MirItem StdItem;
            for (var i = 0; i < ConditionList.Count; i++)
            {
                QuestConditionInfo = ConditionList[i];
                switch (QuestConditionInfo.nCmdCode)
                {
                    case M2Share.nCHECKUSERDATE:
                        result = GotoLable_QuestCheckCondition_CheckUserDateType(PlayObject, PlayObject.m_sCharName, m_sPath + QuestConditionInfo.sParam1, QuestConditionInfo.sParam3, QuestConditionInfo.sParam4, QuestConditionInfo.sParam5);
                        break;
                    case M2Share.nSC_CHECKRANDOMNO:
                        Console.WriteLine("TODO nSC_CHECKRANDOMNO...");
                        //result = GotoLable_QuestCheckCondition_CheckRandomNo(PlayObject, sMsg);
                        break;
                    case M2Share.nCheckDiemon:
                        result = GotoLable_QuestCheckCondition_CheckDieMon(PlayObject, QuestConditionInfo.sParam1);
                        break;
                    case M2Share.ncheckkillplaymon:
                        result = GotoLable_QuestCheckCondition_CheckKillMon(PlayObject, QuestConditionInfo.sParam1);
                        break;
                    case M2Share.nCHECK:
                        n14 = HUtil32.Str_ToInt(QuestConditionInfo.sParam1, 0);
                        n18 = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, 0);
                        n10 = PlayObject.GetQuestFalgStatus(n14);
                        if (n10 == 0)
                        {
                            if (n18 != 0)
                            {
                                result = false;
                            }
                        }
                        else
                        {
                            if (n18 == 0)
                            {
                                result = false;
                            }
                        }
                        break;
                    case M2Share.nRANDOM:
                        if (M2Share.RandomNumber.Random(QuestConditionInfo.nParam1) != 0)
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nGENDER:
                        if (QuestConditionInfo.sParam1.ToLower().CompareTo(M2Share.sMAN.ToLower()) == 0)
                        {
                            if (PlayObject.m_btGender != ObjBase.gMan)
                            {
                                result = false;
                            }
                        }
                        else
                        {
                            if (PlayObject.m_btGender != ObjBase.gWoMan)
                            {
                                result = false;
                            }
                        }
                        break;
                    case M2Share.nDAYTIME:
                        if (QuestConditionInfo.sParam1.ToLower().CompareTo(M2Share.sSUNRAISE.ToLower()) == 0)
                        {
                            if (M2Share.g_nGameTime != 0)
                            {
                                result = false;
                            }
                        }
                        if (QuestConditionInfo.sParam1.ToLower().CompareTo(M2Share.sDAY.ToLower()) == 0)
                        {
                            if (M2Share.g_nGameTime != 1)
                            {
                                result = false;
                            }
                        }
                        if (QuestConditionInfo.sParam1.ToLower().CompareTo(M2Share.sSUNSET.ToLower()) == 0)
                        {
                            if (M2Share.g_nGameTime != 2)
                            {
                                result = false;
                            }
                        }
                        if (QuestConditionInfo.sParam1.ToLower().CompareTo(M2Share.sNIGHT.ToLower()) == 0)
                        {
                            if (M2Share.g_nGameTime != 3)
                            {
                                result = false;
                            }
                        }
                        break;
                    case M2Share.nCHECKOPEN:
                        n14 = HUtil32.Str_ToInt(QuestConditionInfo.sParam1, 0);
                        n18 = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, 0);
                        n10 = PlayObject.GetQuestUnitOpenStatus(n14);
                        if (n10 == 0)
                        {
                            if (n18 != 0)
                            {
                                result = false;
                            }
                        }
                        else
                        {
                            if (n18 == 0)
                            {
                                result = false;
                            }
                        }
                        break;
                    case M2Share.nCHECKUNIT:
                        n14 = HUtil32.Str_ToInt(QuestConditionInfo.sParam1, 0);
                        n18 = HUtil32.Str_ToInt(QuestConditionInfo.sParam2, 0);
                        n10 = PlayObject.GetQuestUnitStatus(n14);
                        if (n10 == 0)
                        {
                            if (n18 != 0)
                            {
                                result = false;
                            }
                        }
                        else
                        {
                            if (n18 == 0)
                            {
                                result = false;
                            }
                        }
                        break;
                    case M2Share.nCHECKLEVEL:
                        if (PlayObject.m_Abil.Level < QuestConditionInfo.nParam1)
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nCHECKJOB:
                        if (HUtil32.CompareLStr(QuestConditionInfo.sParam1, M2Share.sWarrior, M2Share.sWarrior.Length))
                        {
                            if (PlayObject.m_btJob != M2Share.jWarr)
                            {
                                result = false;
                            }
                        }
                        if (HUtil32.CompareLStr(QuestConditionInfo.sParam1, M2Share.sWizard, M2Share.sWizard.Length))
                        {
                            if (PlayObject.m_btJob != M2Share.jWizard)
                            {
                                result = false;
                            }
                        }
                        if (HUtil32.CompareLStr(QuestConditionInfo.sParam1, M2Share.sTaos, M2Share.sTaos.Length))
                        {
                            if (PlayObject.m_btJob != M2Share.jTaos)
                            {
                                result = false;
                            }
                        }
                        break;
                    case M2Share.nCHECKBBCOUNT:
                        if (PlayObject.m_SlaveList.Count < QuestConditionInfo.nParam1)
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nCHECKCREDITPOINT:
                        break;
                    case M2Share.nCHECKITEM:
                        UserItem = PlayObject.QuestCheckItem(QuestConditionInfo.sParam1, ref n1C, ref nMaxDura, ref nDura);
                        if (n1C < QuestConditionInfo.nParam2)
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nCHECKITEMW:
                        UserItem = GotoLable_CheckItemW(PlayObject, QuestConditionInfo.sParam1, QuestConditionInfo.nParam2);
                        if (UserItem == null)
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nCHECKGOLD:
                        if (PlayObject.m_nGold < QuestConditionInfo.nParam1)
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nISTAKEITEM:
                        if (sC != QuestConditionInfo.sParam1)
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nCHECKDURA:
                        UserItem = PlayObject.QuestCheckItem(QuestConditionInfo.sParam1, ref n1C, ref nMaxDura, ref nDura);
                        if (HUtil32.Round(nDura / 1000) < QuestConditionInfo.nParam2)
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nCHECKDURAEVA:
                        UserItem = PlayObject.QuestCheckItem(QuestConditionInfo.sParam1, ref n1C, ref nMaxDura, ref nDura);
                        if (n1C > 0)
                        {
                            if (HUtil32.Round(nMaxDura / n1C / 1000) < QuestConditionInfo.nParam2)
                            {
                                result = false;
                            }
                        }
                        else
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nDAYOFWEEK:
                        if (HUtil32.CompareLStr(QuestConditionInfo.sParam1, M2Share.sSUN, M2Share.sSUN.Length))
                        {
                            if ((int)DateTime.Now.DayOfWeek != 1)
                            {
                                result = false;
                            }
                        }
                        if (HUtil32.CompareLStr(QuestConditionInfo.sParam1, M2Share.sMON, M2Share.sMON.Length))
                        {
                            if ((int)DateTime.Now.DayOfWeek != 2)
                            {
                                result = false;
                            }
                        }
                        if (HUtil32.CompareLStr(QuestConditionInfo.sParam1, M2Share.sTUE, M2Share.sTUE.Length))
                        {
                            if ((int)DateTime.Now.DayOfWeek != 3)
                            {
                                result = false;
                            }
                        }
                        if (HUtil32.CompareLStr(QuestConditionInfo.sParam1, M2Share.sWED, M2Share.sWED.Length))
                        {
                            if ((int)DateTime.Now.DayOfWeek != 4)
                            {
                                result = false;
                            }
                        }
                        if (HUtil32.CompareLStr(QuestConditionInfo.sParam1, M2Share.sTHU, M2Share.sTHU.Length))
                        {
                            if ((int)DateTime.Now.DayOfWeek != 5)
                            {
                                result = false;
                            }
                        }
                        if (HUtil32.CompareLStr(QuestConditionInfo.sParam1, M2Share.sFRI, M2Share.sFRI.Length))
                        {
                            if ((int)DateTime.Now.DayOfWeek != 6)
                            {
                                result = false;
                            }
                        }
                        if (HUtil32.CompareLStr(QuestConditionInfo.sParam1, M2Share.sSAT, M2Share.sSAT.Length))
                        {
                            if ((int)DateTime.Now.DayOfWeek != 7)
                            {
                                result = false;
                            }
                        }
                        break;
                    case M2Share.nHOUR:
                        if ((QuestConditionInfo.nParam1 != 0) && (QuestConditionInfo.nParam2 == 0))
                        {
                            QuestConditionInfo.nParam2 = QuestConditionInfo.nParam1;
                        }
                        Hour = DateTime.Now.Hour;
                        Min = DateTime.Now.Minute;
                        Sec = DateTime.Now.Second;
                        MSec = DateTime.Now.Millisecond;
                        if ((Hour < QuestConditionInfo.nParam1) || (Hour > QuestConditionInfo.nParam2))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nMIN:
                        if ((QuestConditionInfo.nParam1 != 0) && (QuestConditionInfo.nParam2 == 0))
                        {
                            QuestConditionInfo.nParam2 = QuestConditionInfo.nParam1;
                        }
                        Hour = DateTime.Now.Hour;
                        Min = DateTime.Now.Minute;
                        Sec = DateTime.Now.Second;
                        MSec = DateTime.Now.Millisecond;
                        if ((Min < QuestConditionInfo.nParam1) || (Min > QuestConditionInfo.nParam2))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nCHECKPKPOINT:
                        if (PlayObject.PKLevel() < QuestConditionInfo.nParam1)
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nCHECKLUCKYPOINT:
                        if (PlayObject.m_nBodyLuckLevel < QuestConditionInfo.nParam1)
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nCHECKMONMAP:
                        Envir = M2Share.g_MapManager.FindMap(QuestConditionInfo.sParam1);
                        if (Envir != null)
                        {
                            if (M2Share.UserEngine.GetMapMonster(Envir, null) < QuestConditionInfo.nParam2)
                            {
                                result = false;
                            }
                        }
                        break;
                    case M2Share.nCHECKMONAREA:
                        break;
                    case M2Share.nCHECKHUM:
                        if (M2Share.UserEngine.GetMapHuman(QuestConditionInfo.sParam1) < QuestConditionInfo.nParam2)
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nCHECKBAGGAGE:
                        if (PlayObject.IsEnoughBag())
                        {
                            if (QuestConditionInfo.sParam1 != "")
                            {
                                result = false;
                                StdItem = M2Share.UserEngine.GetStdItem(QuestConditionInfo.sParam1);
                                if (StdItem != null)
                                {
                                    if (PlayObject.IsAddWeightAvailable(StdItem.Weight))
                                    {
                                        result = true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nCHECKNAMELIST:
                        if (!GotoLable_CheckStringList(PlayObject.m_sCharName, m_sPath + QuestConditionInfo.sParam1))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nCHECKACCOUNTLIST:
                        if (!GotoLable_CheckStringList(PlayObject.m_sUserID, m_sPath + QuestConditionInfo.sParam1))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nCHECKIPLIST:
                        if (!GotoLable_CheckStringList(PlayObject.m_sIPaddr, m_sPath + QuestConditionInfo.sParam1))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nEQUAL:
                        n10 = M2Share.GetValNameNo(QuestConditionInfo.sParam1);
                        if (n10 >= 0)
                        {
                            if (HUtil32.RangeInDefined(n10, 0, 9))
                            {
                                if (PlayObject.m_nVal[n10] != QuestConditionInfo.nParam2)
                                {
                                    result = false;
                                }
                            }
                            else if (HUtil32.RangeInDefined(n10, 100, 119))
                            {
                                if (M2Share.g_Config.GlobalVal[n10 - 100] != QuestConditionInfo.nParam2)
                                {
                                    result = false;
                                }
                            }
                            else if (HUtil32.RangeInDefined(n10, 200, 209))
                            {
                                if (PlayObject.m_DyVal[n10 - 200] != QuestConditionInfo.nParam2)
                                {
                                    result = false;
                                }
                            }
                            else if (HUtil32.RangeInDefined(n10, 300, 399))
                            {
                                if (PlayObject.m_nMval[n10 - 300] != QuestConditionInfo.nParam2)
                                {
                                    result = false;
                                }
                            }
                            else if (HUtil32.RangeInDefined(n10, 400, 499))
                            {
                                if (M2Share.g_Config.GlobaDyMval[n10 - 400] != QuestConditionInfo.nParam2)
                                {
                                    result = false;
                                }
                            }
                        }
                        else
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nLARGE:
                        n10 = M2Share.GetValNameNo(QuestConditionInfo.sParam1);
                        if (n10 >= 0)
                        {
                            if (HUtil32.RangeInDefined(n10,0,9))
                            {
                                if (PlayObject.m_nVal[n10] <= QuestConditionInfo.nParam2)
                                {
                                    result = false;
                                }
                            }
                            else if (HUtil32.RangeInDefined(n10, 100, 119))
                            {
                                if (M2Share.g_Config.GlobalVal[n10 - 100] <= QuestConditionInfo.nParam2)
                                {
                                    result = false;
                                }
                            }
                            else if (HUtil32.RangeInDefined(n10, 200, 209))
                            {
                                if (PlayObject.m_DyVal[n10 - 200] <= QuestConditionInfo.nParam2)
                                {
                                    result = false;
                                }
                            }
                            else if (HUtil32.RangeInDefined(n10, 300, 399))
                            {
                                if (PlayObject.m_nMval[n10 - 300] <= QuestConditionInfo.nParam2)
                                {
                                    result = false;
                                }
                            }
                            else if (HUtil32.RangeInDefined(n10, 400, 499))
                            {
                                if (M2Share.g_Config.GlobaDyMval[n10 - 400] <= QuestConditionInfo.nParam2)
                                {
                                    result = false;
                                }
                            }
                        }
                        else
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSMALL:
                        n10 = M2Share.GetValNameNo(QuestConditionInfo.sParam1);
                        if (n10 >= 0)
                        {
                            if (HUtil32.RangeInDefined(n10, 0, 9))
                            {
                                if (PlayObject.m_nVal[n10] >= QuestConditionInfo.nParam2)
                                {
                                    result = false;
                                }
                            }
                            else if (HUtil32.RangeInDefined(n10, 100, 119))
                            {
                                if (M2Share.g_Config.GlobalVal[n10 - 100] >= QuestConditionInfo.nParam2)
                                {
                                    result = false;
                                }
                            }
                            else if (HUtil32.RangeInDefined(n10, 200, 209))
                            {
                                if (PlayObject.m_DyVal[n10 - 200] >= QuestConditionInfo.nParam2)
                                {
                                    result = false;
                                }
                            }
                            else if (HUtil32.RangeInDefined(n10, 300, 399))
                            {
                                if (PlayObject.m_nMval[n10 - 300] >= QuestConditionInfo.nParam2)
                                {
                                    result = false;
                                }
                            }
                            else if (HUtil32.RangeInDefined(n10, 400, 499))
                            {
                                if (M2Share.g_Config.GlobaDyMval[n10 - 400] >= QuestConditionInfo.nParam2)
                                {
                                    result = false;
                                }
                            }
                        }
                        else
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_ISSYSOP:
                        if (!(PlayObject.m_btPermission >= 4))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_ISADMIN:
                        if (!(PlayObject.m_btPermission >= 6))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_CHECKGROUPCOUNT:
                        if (!ConditionOfCheckGroupCount(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_CHECKPOSEDIR:
                        if (!ConditionOfCheckPoseDir(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_CHECKPOSELEVEL:
                        if (!ConditionOfCheckPoseLevel(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_CHECKPOSEGENDER:
                        if (!ConditionOfCheckPoseGender(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_CHECKLEVELEX:
                        if (!ConditionOfCheckLevelEx(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_CHECKBONUSPOINT:
                        if (!ConditionOfCheckBonusPoint(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_CHECKMARRY:
                        if (!ConditionOfCheckMarry(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_CHECKPOSEMARRY:
                        if (!ConditionOfCheckPoseMarry(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_CHECKMARRYCOUNT:
                        if (!ConditionOfCheckMarryCount(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_CHECKMASTER:
                        if (!ConditionOfCheckMaster(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_HAVEMASTER:
                        if (!ConditionOfHaveMaster(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_CHECKPOSEMASTER:
                        if (!ConditionOfCheckPoseMaster(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_POSEHAVEMASTER:
                        if (!ConditionOfPoseHaveMaster(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_CHECKISMASTER:
                        if (!ConditionOfCheckIsMaster(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_HASGUILD:
                        if (!ConditionOfCheckHaveGuild(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_ISGUILDMASTER:
                        if (!ConditionOfCheckIsGuildMaster(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_CHECKCASTLEMASTER:
                        if (!ConditionOfCheckIsCastleMaster(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_ISCASTLEGUILD:
                        if (!ConditionOfCheckIsCastleaGuild(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_ISATTACKGUILD:
                        if (!ConditionOfCheckIsAttackGuild(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_ISDEFENSEGUILD:
                        if (!ConditionOfCheckIsDefenseGuild(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_CHECKCASTLEDOOR:
                        if (!ConditionOfCheckCastleDoorStatus(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_ISATTACKALLYGUILD:
                        if (!ConditionOfCheckIsAttackAllyGuild(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_ISDEFENSEALLYGUILD:
                        if (!ConditionOfCheckIsDefenseAllyGuild(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_CHECKPOSEISMASTER:
                        if (!ConditionOfCheckPoseIsMaster(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_CHECKNAMEIPLIST:
                        if (!ConditionOfCheckNameIPList(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_CHECKACCOUNTIPLIST:
                        if (!ConditionOfCheckAccountIPList(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_CHECKSLAVECOUNT:
                        if (!ConditionOfCheckSlaveCount(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_ISNEWHUMAN:
                        if (!PlayObject.m_boNewHuman)
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_CHECKMEMBERTYPE:
                        if (!ConditionOfCheckMemberType(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_CHECKMEMBERLEVEL:
                        if (!ConditionOfCheckMemBerLevel(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_CHECKGAMEGOLD:
                        if (!ConditionOfCheckGameGold(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_CHECKGAMEPOINT:
                        if (!ConditionOfCheckGamePoint(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_CHECKNAMELISTPOSITION:
                        if (!ConditionOfCheckNameListPostion(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_CHECKGUILDLIST:
                        if (PlayObject.m_MyGuild != null)
                        {
                            if (!GotoLable_CheckStringList(PlayObject.m_MyGuild.sGuildName, m_sPath + QuestConditionInfo.sParam1))
                            {
                                result = false;
                            }
                        }
                        else
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_CHECKRENEWLEVEL:
                        if (!ConditionOfCheckReNewLevel(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_CHECKSLAVELEVEL:
                        if (!ConditionOfCheckSlaveLevel(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_CHECKSLAVENAME:
                        if (!ConditionOfCheckSlaveName(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_CHECKCREDITPOINT:
                        if (!ConditionOfCheckCreditPoint(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_CHECKOFGUILD:
                        if (!ConditionOfCheckOfGuild(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_CHECKPAYMENT:
                        if (!ConditionOfCheckPayMent(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_CHECKUSEITEM:
                        if (!ConditionOfCheckUseItem(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_CHECKBAGSIZE:
                        if (!ConditionOfCheckBagSize(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_CHECKLISTCOUNT:
                        if (!ConditionOfCheckListCount(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_CHECKDC:
                        if (!ConditionOfCheckDC(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_CHECKMC:
                        if (!ConditionOfCheckMC(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_CHECKSC:
                        if (!ConditionOfCheckSC(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_CHECKHP:
                        if (!ConditionOfCheckHP(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_CHECKMP:
                        if (!ConditionOfCheckMP(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_CHECKITEMTYPE:
                        if (!ConditionOfCheckItemType(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_CHECKEXP:
                        if (!ConditionOfCheckExp(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_CHECKCASTLEGOLD:
                        if (!ConditionOfCheckCastleGold(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_PASSWORDERRORCOUNT:
                        if (!ConditionOfCheckPasswordErrorCount(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_ISLOCKPASSWORD:
                        if (!ConditionOfIsLockPassword(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_ISLOCKSTORAGE:
                        if (!ConditionOfIsLockStorage(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_CHECKBUILDPOINT:
                        if (!ConditionOfCheckGuildBuildPoint(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_CHECKAURAEPOINT:
                        if (!ConditionOfCheckGuildAuraePoint(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_CHECKSTABILITYPOINT:
                        if (!ConditionOfCheckStabilityPoint(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_CHECKFLOURISHPOINT:
                        if (!ConditionOfCheckFlourishPoint(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_CHECKCONTRIBUTION:
                        if (!ConditionOfCheckContribution(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_CHECKRANGEMONCOUNT:
                        if (!ConditionOfCheckRangeMonCount(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_CHECKITEMADDVALUE:
                        if (!ConditionOfCheckItemAddValue(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_CHECKINMAPRANGE:
                        if (!ConditionOfCheckInMapRange(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_CASTLECHANGEDAY:
                        if (!ConditionOfCheckCastleChageDay(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_CASTLEWARDAY:
                        if (!ConditionOfCheckCastleWarDay(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_ONLINELONGMIN:
                        if (!ConditionOfCheckOnlineLongMin(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_CHECKGUILDCHIEFITEMCOUNT:
                        if (!ConditionOfCheckChiefItemCount(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_CHECKNAMEDATELIST:
                        if (!ConditionOfCheckNameDateList(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_CHECKMAPHUMANCOUNT:
                        if (!ConditionOfCheckMapHumanCount(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_CHECKMAPMONCOUNT:
                        if (!ConditionOfCheckMapMonCount(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_CHECKVAR:
                        if (!ConditionOfCheckVar(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_CHECKSERVERNAME:
                        if (!ConditionOfCheckServerName(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_CHECKISONMAP:
                        if (!ConditionOfCheckIsOnMap(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_KILLBYHUM:
                        if ((PlayObject.m_LastHiter != null) && (PlayObject.m_LastHiter.m_btRaceServer != Grobal2.RC_PLAYOBJECT))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_KILLBYMON:
                        if ((PlayObject.m_LastHiter != null) && (PlayObject.m_LastHiter.m_btRaceServer == Grobal2.RC_PLAYOBJECT))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_CHECKINSAFEZONE:
                        if (!PlayObject.InSafeZone())
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_CHECKMAP:
                        if (!ConditionOfCheckMap(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_CHECKPOS:
                        if (!ConditionOfCheckPos(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_REVIVESLAVE:
                        if (!ConditionOfReviveSlave(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_CHECKMAGICLVL:
                        if (!ConditionOfCheckMagicLvl(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_CHECKGROUPCLASS:
                        if (!ConditionOfCheckGroupClass(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_ISGROUPMASTER:
                        if (PlayObject.m_GroupOwner != null)
                        {
                            if (PlayObject.m_GroupOwner != PlayObject)
                            {
                                result = false;
                            }
                        }
                        else
                        {
                            result = false;
                        }
                        break;
                    case M2Share.nSC_ISHIGH:
                        result = ConditionOfIsHigh(PlayObject, QuestConditionInfo);
                        break;
                }
                if (!result)
                {
                    break;
                }
            }
            return result;
        }

        private bool GotoLable_JmpToLable(TPlayObject PlayObject, string sLabel)
        {
            PlayObject.m_nScriptGotoCount++;
            if (PlayObject.m_nScriptGotoCount > M2Share.g_Config.nScriptGotoCountLimit)
            {
                return false;
            }
            GotoLable(PlayObject, sLabel, false);
            return true;
        }

        private void GotoLable_GoToQuest(TPlayObject PlayObject, int nQuest)
        {
            TScript Script;
            for (var i = 0; i < m_ScriptList.Count; i++)
            {
                Script = m_ScriptList[i];
                if (Script.nQuest == nQuest)
                {
                    PlayObject.m_Script = Script;
                    PlayObject.m_NPC = this;
                    GotoLable(PlayObject, M2Share.sMAIN, false);
                    break;
                }
            }
        }

        private void GotoLable_AddUseDateList(string sHumName, string sListFileName)
        {
            StringList LoadList;
            string s10 = string.Empty;
            string sText;
            bool bo15;
            sListFileName = M2Share.g_Config.sEnvirDir + sListFileName;
            LoadList = new StringList();
            if (File.Exists(sListFileName))
            {
                try
                {
                    LoadList.LoadFromFile(sListFileName);
                }
                catch
                {
                    M2Share.MainOutMessage("loading fail.... => " + sListFileName);
                }
            }
            bo15 = false;
            for (var i = 0; i < LoadList.Count; i++)
            {
                sText = LoadList[i].Trim();
                sText = HUtil32.GetValidStrCap(sText, ref s10, new string[] { " ", "\t" });
                if (sHumName.ToLower().CompareTo(s10.ToLower()) == 0)
                {
                    bo15 = true;
                    break;
                }
            }
            if (!bo15)
            {
                s10 = string.Format("%s    %s", new string[] { sHumName, DateTime.Today.ToString() });
                LoadList.Add(s10);
                try
                {
                    LoadList.SaveToFile(sListFileName);
                }
                catch
                {
                    M2Share.MainOutMessage("saving fail.... => " + sListFileName);
                }
            }
            //LoadList.Free;
        }

        private void GotoLable_AddList(string sHumName, string sListFileName)
        {
            string s10 = string.Empty;
            sListFileName = M2Share.g_Config.sEnvirDir + sListFileName;
            var LoadList = new StringList();
            if (File.Exists(sListFileName))
            {
                try
                {
                    LoadList.LoadFromFile(sListFileName);
                }
                catch
                {
                    M2Share.MainOutMessage("loading fail.... => " + sListFileName);
                }
            }
            var bo15 = false;
            for (var i = 0; i < LoadList.Count; i++)
            {
                s10 = LoadList[i].Trim();
                if (sHumName.ToLower().CompareTo(s10.ToLower()) == 0)
                {
                    bo15 = true;
                    break;
                }
            }
            if (!bo15)
            {
                LoadList.Add(sHumName);
                try
                {
                    LoadList.SaveToFile(sListFileName);
                }
                catch
                {
                    M2Share.MainOutMessage("saving fail.... => " + sListFileName);
                }
            }
            // LoadList.Free;
        }

        private void GotoLable_DELUseDateList(string sHumName, string sListFileName)
        {
            string s10 = string.Empty;
            string sText;
            sListFileName = M2Share.g_Config.sEnvirDir + sListFileName;
            var LoadList = new StringList();
            if (File.Exists(sListFileName))
            {
                LoadList.LoadFromFile(sListFileName);
            }
            var bo15 = false;
            for (var i = 0; i < LoadList.Count; i++)
            {
                sText = LoadList[i].Trim();
                sText = HUtil32.GetValidStrCap(sText, ref s10, new string[] { " ", "\t" });
                if (sHumName.ToLower().CompareTo(s10.ToLower()) == 0)
                {
                    bo15 = true;
                    LoadList.RemoveAt(i);
                    break;
                }
            }
            if (bo15)
            {
                LoadList.SaveToFile(sListFileName);
            }
            //LoadList.Free;
        }

        private void GotoLable_DelList(string sHumName, string sListFileName)
        {
            string s10 = string.Empty;
            bool bo15;
            sListFileName = M2Share.g_Config.sEnvirDir + sListFileName;
            var LoadList = new StringList();
            if (File.Exists(sListFileName))
            {
                try
                {
                    LoadList.LoadFromFile(sListFileName);
                }
                catch
                {
                    M2Share.MainOutMessage("loading fail.... => " + sListFileName);
                }
            }
            bo15 = false;
            for (var i = 0; i < LoadList.Count; i++)
            {
                s10 = LoadList[i].Trim();
                if (sHumName.ToLower().CompareTo(s10.ToLower()) == 0)
                {
                    LoadList.RemoveAt(i);
                    bo15 = true;
                    break;
                }
            }
            if (bo15)
            {
                LoadList.SaveToFile(sListFileName);
            }
            // LoadList.Free;
        }

        private void GotoLable_TakeItem(TPlayObject PlayObject, string sItemName, int nItemCount, ref string sC)
        {
            TUserItem UserItem;
            MirItem StdItem;
            if (sItemName.ToLower().CompareTo(Grobal2.sSTRING_GOLDNAME.ToLower()) == 0)
            {
                PlayObject.DecGold(nItemCount);
                PlayObject.GoldChanged();
                if (M2Share.g_boGameLogGold)
                {
                    M2Share.AddGameDataLog("10" + "\t" + PlayObject.m_sMapName + "\t" + PlayObject.m_nCurrX + "\t" + PlayObject.m_nCurrY + "\t" + PlayObject.m_sCharName + "\t" + Grobal2.sSTRING_GOLDNAME + "\t" + nItemCount + "\t" + '1' + "\t" + this.m_sCharName);
                }
                return;
            }
            for (var i = PlayObject.m_ItemList.Count - 1; i >= 0; i--)
            {
                if (nItemCount <= 0)
                {
                    break;
                }
                UserItem = PlayObject.m_ItemList[i];
                if (M2Share.UserEngine.GetStdItemName(UserItem.wIndex).ToLower().CompareTo(sItemName.ToLower()) == 0)
                {
                    StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                    if (StdItem.NeedIdentify == 1)
                    {
                        M2Share.AddGameDataLog("10" + "\t" + PlayObject.m_sMapName + "\t" + PlayObject.m_nCurrX + "\t" + PlayObject.m_nCurrY + "\t" + PlayObject.m_sCharName + "\t" + sItemName + "\t" + UserItem.MakeIndex + "\t" + '1' + "\t" + this.m_sCharName);
                    }
                    PlayObject.SendDelItems(UserItem);
                    sC = M2Share.UserEngine.GetStdItemName(UserItem.wIndex);
                    Dispose(UserItem);
                    PlayObject.m_ItemList.RemoveAt(i);
                    nItemCount -= 1;
                }
            }
        }

        public void GotoLable_GiveItem(TPlayObject PlayObject, string sItemName, int nItemCount)
        {
            TUserItem UserItem;
            MirItem StdItem;
            if (sItemName.ToLower().CompareTo(Grobal2.sSTRING_GOLDNAME.ToLower()) == 0)
            {
                PlayObject.IncGold(nItemCount);
                PlayObject.GoldChanged();
                if (M2Share.g_boGameLogGold)
                {
                    M2Share.AddGameDataLog('9' + "\t" + PlayObject.m_sMapName + "\t" + PlayObject.m_nCurrX + "\t" + PlayObject.m_nCurrY + "\t" + PlayObject.m_sCharName + "\t" + Grobal2.sSTRING_GOLDNAME + "\t" + nItemCount + "\t" + '1' + "\t" + this.m_sCharName);
                }
                return;
            }
            if (M2Share.UserEngine.GetStdItemIdx(sItemName) > 0)
            {
                if (!(nItemCount >= 1 && nItemCount <= 50))
                {
                    nItemCount = 1;
                }
                // 12.28 改上一条
                for (var i = 0; i < nItemCount; i++)
                {
                    if (PlayObject.IsEnoughBag())
                    {
                        UserItem = new TUserItem();
                        if (M2Share.UserEngine.CopyToUserItemFromName(sItemName, ref UserItem))
                        {
                            PlayObject.m_ItemList.Add(UserItem);
                            PlayObject.SendAddItem(UserItem);
                            StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                            if (StdItem.NeedIdentify == 1)
                            {
                                M2Share.AddGameDataLog('9' + "\t" + PlayObject.m_sMapName + "\t" + PlayObject.m_nCurrX + "\t" + PlayObject.m_nCurrY + "\t" + PlayObject.m_sCharName + "\t" + sItemName + "\t" + UserItem.MakeIndex + "\t" + '1' + "\t" + this.m_sCharName);
                            }
                        }
                        else
                        {
                            Dispose(UserItem);
                        }
                    }
                    else
                    {
                        UserItem = new TUserItem();
                        if (M2Share.UserEngine.CopyToUserItemFromName(sItemName, ref UserItem))
                        {
                            StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                            if (StdItem.NeedIdentify == 1)
                            {
                                M2Share.AddGameDataLog('9' + "\t" + PlayObject.m_sMapName + "\t" + PlayObject.m_nCurrX + "\t" + PlayObject.m_nCurrY + "\t" + PlayObject.m_sCharName + "\t" + sItemName + "\t" + UserItem.MakeIndex + "\t" + '1' + "\t" + this.m_sCharName);
                            }
                            PlayObject.DropItemDown(UserItem, 3, false, PlayObject, null);
                        }
                        Dispose(UserItem);
                    }
                }
            }
        }

        private void GotoLable_TakeWItem(TPlayObject PlayObject, string sItemName, int nItemCount)
        {
            string sName = string.Empty;
            string sC = string.Empty;
            if (HUtil32.CompareLStr(sItemName, "[NECKLACE]", 4))
            {
                if (PlayObject.m_UseItems[Grobal2.U_NECKLACE].wIndex > 0)
                {
                    PlayObject.SendDelItems(PlayObject.m_UseItems[Grobal2.U_NECKLACE]);
                    sC = M2Share.UserEngine.GetStdItemName(PlayObject.m_UseItems[Grobal2.U_NECKLACE].wIndex);
                    PlayObject.m_UseItems[Grobal2.U_NECKLACE].wIndex = 0;
                    return;
                }
            }
            if (HUtil32.CompareLStr(sItemName, "[RING]", 4))
            {
                if (PlayObject.m_UseItems[Grobal2.U_RINGL].wIndex > 0)
                {
                    PlayObject.SendDelItems(PlayObject.m_UseItems[Grobal2.U_RINGL]);
                    sC = M2Share.UserEngine.GetStdItemName(PlayObject.m_UseItems[Grobal2.U_RINGL].wIndex);
                    PlayObject.m_UseItems[Grobal2.U_RINGL].wIndex = 0;
                    return;
                }
                if (PlayObject.m_UseItems[Grobal2.U_RINGR].wIndex > 0)
                {
                    PlayObject.SendDelItems(PlayObject.m_UseItems[Grobal2.U_RINGR]);
                    sC = M2Share.UserEngine.GetStdItemName(PlayObject.m_UseItems[Grobal2.U_RINGR].wIndex);
                    PlayObject.m_UseItems[Grobal2.U_RINGR].wIndex = 0;
                    return;
                }
            }
            if (HUtil32.CompareLStr(sItemName, "[ARMRING]", 4))
            {
                if (PlayObject.m_UseItems[Grobal2.U_ARMRINGL].wIndex > 0)
                {
                    PlayObject.SendDelItems(PlayObject.m_UseItems[Grobal2.U_ARMRINGL]);
                    sC = M2Share.UserEngine.GetStdItemName(PlayObject.m_UseItems[Grobal2.U_ARMRINGL].wIndex);
                    PlayObject.m_UseItems[Grobal2.U_ARMRINGL].wIndex = 0;
                    return;
                }
                if (PlayObject.m_UseItems[Grobal2.U_ARMRINGR].wIndex > 0)
                {
                    PlayObject.SendDelItems(PlayObject.m_UseItems[Grobal2.U_ARMRINGR]);
                    sC = M2Share.UserEngine.GetStdItemName(PlayObject.m_UseItems[Grobal2.U_ARMRINGR].wIndex);
                    PlayObject.m_UseItems[Grobal2.U_ARMRINGR].wIndex = 0;
                    return;
                }
            }
            if (HUtil32.CompareLStr(sItemName, "[WEAPON]", 4))
            {
                if (PlayObject.m_UseItems[Grobal2.U_WEAPON].wIndex > 0)
                {
                    PlayObject.SendDelItems(PlayObject.m_UseItems[Grobal2.U_WEAPON]);
                    sC = M2Share.UserEngine.GetStdItemName(PlayObject.m_UseItems[Grobal2.U_WEAPON].wIndex);
                    PlayObject.m_UseItems[Grobal2.U_WEAPON].wIndex = 0;
                    return;
                }
            }
            if (HUtil32.CompareLStr(sItemName, "[HELMET]", 4))
            {
                if (PlayObject.m_UseItems[Grobal2.U_HELMET].wIndex > 0)
                {
                    PlayObject.SendDelItems(PlayObject.m_UseItems[Grobal2.U_HELMET]);
                    sC = M2Share.UserEngine.GetStdItemName(PlayObject.m_UseItems[Grobal2.U_HELMET].wIndex);
                    PlayObject.m_UseItems[Grobal2.U_HELMET].wIndex = 0;
                    return;
                }
            }
            if (HUtil32.CompareLStr(sItemName, "[DRESS]", 4))
            {
                if (PlayObject.m_UseItems[Grobal2.U_DRESS].wIndex > 0)
                {
                    PlayObject.SendDelItems(PlayObject.m_UseItems[Grobal2.U_DRESS]);
                    sC = M2Share.UserEngine.GetStdItemName(PlayObject.m_UseItems[Grobal2.U_DRESS].wIndex);
                    PlayObject.m_UseItems[Grobal2.U_DRESS].wIndex = 0;
                    return;
                }
            }
            if (HUtil32.CompareLStr(sItemName, "[U_BUJUK]", 4))
            {
                if (PlayObject.m_UseItems[Grobal2.U_BUJUK].wIndex > 0)
                {
                    PlayObject.SendDelItems(PlayObject.m_UseItems[Grobal2.U_BUJUK]);
                    sC = M2Share.UserEngine.GetStdItemName(PlayObject.m_UseItems[Grobal2.U_BUJUK].wIndex);
                    PlayObject.m_UseItems[Grobal2.U_BUJUK].wIndex = 0;
                    return;
                }
            }
            if (HUtil32.CompareLStr(sItemName, "[U_BELT]", 4))
            {
                if (PlayObject.m_UseItems[Grobal2.U_BELT].wIndex > 0)
                {
                    PlayObject.SendDelItems(PlayObject.m_UseItems[Grobal2.U_BELT]);
                    sC = M2Share.UserEngine.GetStdItemName(PlayObject.m_UseItems[Grobal2.U_BELT].wIndex);
                    PlayObject.m_UseItems[Grobal2.U_BELT].wIndex = 0;
                    return;
                }
            }
            if (HUtil32.CompareLStr(sItemName, "[U_BOOTS]", 4))
            {
                if (PlayObject.m_UseItems[Grobal2.U_BOOTS].wIndex > 0)
                {
                    PlayObject.SendDelItems(PlayObject.m_UseItems[Grobal2.U_BOOTS]);
                    sC = M2Share.UserEngine.GetStdItemName(PlayObject.m_UseItems[Grobal2.U_BOOTS].wIndex);
                    PlayObject.m_UseItems[Grobal2.U_BOOTS].wIndex = 0;
                    return;
                }
            }
            if (HUtil32.CompareLStr(sItemName, "[U_CHARM]", 4))
            {
                if (PlayObject.m_UseItems[Grobal2.U_CHARM].wIndex > 0)
                {
                    PlayObject.SendDelItems(PlayObject.m_UseItems[Grobal2.U_CHARM]);
                    sC = M2Share.UserEngine.GetStdItemName(PlayObject.m_UseItems[Grobal2.U_CHARM].wIndex);
                    PlayObject.m_UseItems[Grobal2.U_CHARM].wIndex = 0;
                    return;
                }
            }
            for (var i = PlayObject.m_UseItems.GetLowerBound(0); i <= PlayObject.m_UseItems.GetUpperBound(0); i++)
            {
                if (nItemCount <= 0)
                {
                    return;
                }
                if (PlayObject.m_UseItems[i].wIndex > 0)
                {
                    sName = M2Share.UserEngine.GetStdItemName(PlayObject.m_UseItems[i].wIndex);
                    if (sName.ToLower().CompareTo(sItemName.ToLower()) == 0)
                    {
                        PlayObject.SendDelItems(PlayObject.m_UseItems[i]);
                        PlayObject.m_UseItems[i].wIndex = 0;
                        nItemCount -= 1;
                    }
                }
            }
        }

        private bool GotoLable_QuestActionProcess(TPlayObject PlayObject, IList<TQuestActionInfo> ActionList, ref string sC, ref TUserItem UserItem,ref bool bo11)
        {
            bool result= true;
            TQuestActionInfo QuestActionInfo;
            int n14;
            int n1C;
            int n28;
            int n2C;
            int n20X;
            int n24Y;
            string s4C = string.Empty;
            string s50 = string.Empty;
            string s34 = string.Empty;
            string s44 = string.Empty;
            string s48 = string.Empty;
            TEnvirnoment Envir;
            IList<TBaseObject> List58;
            TPlayObject User;
            var n18 = 0;
            var n34 = 0;
            var n38 = 0;
            var n3C = 0;
            var n40 = 0;
            for (var i = 0; i < ActionList.Count; i++)
            {
                QuestActionInfo = ActionList[i];
                switch (QuestActionInfo.nCmdCode)
                {
                    case M2Share.nSET:
                        n28 = HUtil32.Str_ToInt(QuestActionInfo.sParam1, 0);
                        n2C = HUtil32.Str_ToInt(QuestActionInfo.sParam2, 0);
                        PlayObject.SetQuestFlagStatus(n28, n2C);
                        break;
                    case M2Share.nTAKE:
                        GotoLable_TakeItem(PlayObject, QuestActionInfo.sParam1, QuestActionInfo.nParam2, ref sC);
                        break;
                    case M2Share.nSC_GIVE:
                        ActionOfGiveItem(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nTAKEW:
                        GotoLable_TakeWItem(PlayObject, QuestActionInfo.sParam1, QuestActionInfo.nParam2);
                        break;
                    case M2Share.nCLOSE:
                        PlayObject.SendMsg(this, Grobal2.RM_MERCHANTDLGCLOSE, 0, this.ObjectId, 0, 0, "");
                        break;
                    case M2Share.nRESET:
                        for (var k = 0; k < QuestActionInfo.nParam2; k++)
                        {
                            PlayObject.SetQuestFlagStatus(QuestActionInfo.nParam1 + k, 0);
                        }
                        break;
                    case M2Share.nSETOPEN:
                        n28 = HUtil32.Str_ToInt(QuestActionInfo.sParam1, 0);
                        n2C = HUtil32.Str_ToInt(QuestActionInfo.sParam2, 0);
                        PlayObject.SetQuestUnitOpenStatus(n28, n2C);
                        break;
                    case M2Share.nSETUNIT:
                        n28 = HUtil32.Str_ToInt(QuestActionInfo.sParam1, 0);
                        n2C = HUtil32.Str_ToInt(QuestActionInfo.sParam2, 0);
                        PlayObject.SetQuestUnitStatus(n28, n2C);
                        break;
                    case M2Share.nRESETUNIT:
                        for (var k = 0; k < QuestActionInfo.nParam2; k++)
                        {
                            PlayObject.SetQuestUnitStatus(QuestActionInfo.nParam1 + k, 0);
                        }
                        break;
                    case M2Share.nBREAK:
                        result = false;
                        break;
                    case M2Share.nTIMERECALL:
                        PlayObject.m_boTimeRecall = true;
                        PlayObject.m_sMoveMap = PlayObject.m_sMapName;
                        PlayObject.m_nMoveX = PlayObject.m_nCurrX;
                        PlayObject.m_nMoveY = PlayObject.m_nCurrY;
                        PlayObject.m_dwTimeRecallTick = HUtil32.GetTickCount() + (QuestActionInfo.nParam1 * 60 * 1000);
                        break;
                    case M2Share.nSC_PARAM1:
                        n34 = QuestActionInfo.nParam1;
                        s44 = QuestActionInfo.sParam1;
                        break;
                    case M2Share.nSC_PARAM2:
                        n38 = QuestActionInfo.nParam1;
                        s48 = QuestActionInfo.sParam1;
                        break;
                    case M2Share.nSC_PARAM3:
                        n3C = QuestActionInfo.nParam1;
                        s4C = QuestActionInfo.sParam1;
                        break;
                    case M2Share.nSC_PARAM4:
                        n40 = QuestActionInfo.nParam1;
                        s50 = QuestActionInfo.sParam1;
                        break;
                    case M2Share.nSC_EXEACTION:
                        n40 = QuestActionInfo.nParam1;
                        s50 = QuestActionInfo.sParam1;
                        ExeAction(PlayObject, QuestActionInfo.sParam1, QuestActionInfo.sParam2, QuestActionInfo.sParam3, QuestActionInfo.nParam1, QuestActionInfo.nParam2, QuestActionInfo.nParam3);
                        break;
                    case M2Share.nMAPMOVE:
                        PlayObject.SendRefMsg(Grobal2.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
                        PlayObject.SpaceMove(QuestActionInfo.sParam1, (short)QuestActionInfo.nParam2, (short)QuestActionInfo.nParam3, 0);
                        bo11 = true;
                        break;
                    case M2Share.nMAP:
                        PlayObject.SendRefMsg(Grobal2.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
                        PlayObject.MapRandomMove(QuestActionInfo.sParam1, 0);
                        bo11 = true;
                        break;
                    case M2Share.nTAKECHECKITEM:
                        if (UserItem != null)
                        {
                            PlayObject.QuestTakeCheckItem(UserItem);
                        }
                        else
                        {
                            ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sTAKECHECKITEM);
                        }
                        break;
                    case M2Share.nMONGEN:
                        for (var k = 0; k < QuestActionInfo.nParam2; k++)
                        {
                            n20X = M2Share.RandomNumber.Random(QuestActionInfo.nParam3 * 2 + 1) + (n38 - QuestActionInfo.nParam3);
                            n24Y = M2Share.RandomNumber.Random(QuestActionInfo.nParam3 * 2 + 1) + (n3C - QuestActionInfo.nParam3);
                            M2Share.UserEngine.RegenMonsterByName(s44, (short)n20X, (short)n24Y, QuestActionInfo.sParam1);
                        }
                        break;
                    case M2Share.nMONCLEAR:
                        List58 = new List<TBaseObject>();
                        M2Share.UserEngine.GetMapMonster(M2Share.g_MapManager.FindMap(QuestActionInfo.sParam1), List58);
                        for (var k = 0; k < List58.Count; k++)
                        {
                            List58[k].m_boNoItem = true;
                            List58[k].m_WAbil.HP = 0;
                        }
                        //List58.Free;
                        break;
                    case M2Share.nMOV:
                        n14 = M2Share.GetValNameNo(QuestActionInfo.sParam1);
                        if (n14 >= 0)
                        {
                            if (HUtil32.RangeInDefined(n14, 0, 10))
                            {
                                PlayObject.m_nVal[n14] = QuestActionInfo.nParam2;
                            }
                            else if (HUtil32.RangeInDefined(n14, 100, 120))
                            {
                                M2Share.g_Config.GlobalVal[n14 - 100] = QuestActionInfo.nParam2;
                            }
                            else if (HUtil32.RangeInDefined(n14, 200, 210))
                            {
                                PlayObject.m_DyVal[n14 - 200] = QuestActionInfo.nParam2;
                            }
                            else if (HUtil32.RangeInDefined(n14, 300, 400))
                            {
                                PlayObject.m_nMval[n14 - 300] = QuestActionInfo.nParam2;
                            }
                            else if (HUtil32.RangeInDefined(n14, 400, 500))
                            {
                                M2Share.g_Config.GlobaDyMval[n14 - 400] = (short)QuestActionInfo.nParam2;
                            }
                            else
                            {
                                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sMOV);
                            }
                        }
                        else
                        {
                            ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sMOV);
                        }
                        break;
                    case M2Share.nINC:
                        n14 = M2Share.GetValNameNo(QuestActionInfo.sParam1);
                        if (n14 >= 0)
                        {
                            if (HUtil32.RangeInDefined(n14, 0, 9))
                            {
                                if (QuestActionInfo.nParam2 > 1)
                                {
                                    PlayObject.m_nVal[n14] += QuestActionInfo.nParam2;
                                }
                                else
                                {
                                    PlayObject.m_nVal[n14]++;
                                }
                            }
                            else if(HUtil32.RangeInDefined(n14,100,119))
                            {
                                if (QuestActionInfo.nParam2 > 1)
                                {
                                    M2Share.g_Config.GlobalVal[n14 - 100] += QuestActionInfo.nParam2;
                                }
                                else
                                {
                                    M2Share.g_Config.GlobalVal[n14 - 100]++;
                                }
                            }
                            else if(HUtil32.RangeInDefined(n14,200,209))
                            {
                                if (QuestActionInfo.nParam2 > 1)
                                {
                                    PlayObject.m_DyVal[n14 - 200] += QuestActionInfo.nParam2;
                                }
                                else
                                {
                                    PlayObject.m_DyVal[n14 - 200]++;
                                }
                            }
                            else if(HUtil32.RangeInDefined(n14,300,399))
                            {
                                if (QuestActionInfo.nParam2 > 1)
                                {
                                    PlayObject.m_nMval[n14 - 300] += QuestActionInfo.nParam2;
                                }
                                else
                                {
                                    PlayObject.m_nMval[n14 - 300]++;
                                }
                            }
                            else if(HUtil32.RangeInDefined(n14,400,499))
                            {
                                if (QuestActionInfo.nParam2 > 1)
                                {
                                    M2Share.g_Config.GlobaDyMval[n14 - 400] += (short)QuestActionInfo.nParam2;
                                }
                                else
                                {
                                    M2Share.g_Config.GlobaDyMval[n14 - 400]++;
                                }
                            }
                            else
                            {
                                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sINC);
                            }
                        }
                        else
                        {
                            ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sINC);
                        }
                        break;
                    case M2Share.nDEC:
                        n14 = M2Share.GetValNameNo(QuestActionInfo.sParam1);
                        if (n14 >= 0)
                        {
                            if (HUtil32.RangeInDefined(n14,0,9))
                            {
                                if (QuestActionInfo.nParam2 > 1)
                                {
                                    PlayObject.m_nVal[n14] -= QuestActionInfo.nParam2;
                                }
                                else
                                {
                                    PlayObject.m_nVal[n14] -= 1;
                                }
                            }
                            else if (HUtil32.RangeInDefined(n14, 100, 119))
                            {
                                if (QuestActionInfo.nParam2 > 1)
                                {
                                    M2Share.g_Config.GlobalVal[n14 - 100] -= QuestActionInfo.nParam2;
                                }
                                else
                                {
                                    M2Share.g_Config.GlobalVal[n14 - 100] -= 1;
                                }
                            }
                            else if (HUtil32.RangeInDefined(n14, 200, 209))
                            {
                                if (QuestActionInfo.nParam2 > 1)
                                {
                                    PlayObject.m_DyVal[n14 - 200] -= QuestActionInfo.nParam2;
                                }
                                else
                                {
                                    PlayObject.m_DyVal[n14 - 200] -= 1;
                                }
                            }
                            else if (HUtil32.RangeInDefined(n14, 300, 399))
                            {
                                if (QuestActionInfo.nParam2 > 1)
                                {
                                    PlayObject.m_nMval[n14 - 300] -= QuestActionInfo.nParam2;
                                }
                                else
                                {
                                    PlayObject.m_nMval[n14 - 300] -= 1;
                                }
                            }
                            else if (HUtil32.RangeInDefined(n14, 400, 499))
                            {
                                if (QuestActionInfo.nParam2 > 1)
                                {
                                    M2Share.g_Config.GlobaDyMval[n14 - 400] -= (short)QuestActionInfo.nParam2;
                                }
                                else
                                {
                                    M2Share.g_Config.GlobaDyMval[n14 - 400] -= 1;
                                }
                            }
                            else
                            {
                                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sDEC);
                            }
                        }
                        else
                        {
                            ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sDEC);
                        }
                        break;
                    case M2Share.nSUM:
                        n18 = 0;
                        n14 = M2Share.GetValNameNo(QuestActionInfo.sParam1);
                        if (n14 >= 0)
                        {
                            if (HUtil32.RangeInDefined(n14, 0, 9))
                            {
                                n18 = PlayObject.m_nVal[n14];
                            }
                            else if (HUtil32.RangeInDefined(n14, 100, 119))
                            {
                                n18 = M2Share.g_Config.GlobalVal[n14 - 100];
                            }
                            else if (HUtil32.RangeInDefined(n14, 200, 209))
                            {
                                n18 = PlayObject.m_DyVal[n14 - 200];
                            }
                            else if (HUtil32.RangeInDefined(n14, 300, 399))
                            {
                                n18 = PlayObject.m_nMval[n14 - 300];
                            }
                            else if (HUtil32.RangeInDefined(n14, 400, 499))
                            {
                                n18 = M2Share.g_Config.GlobaDyMval[n14 - 400];
                            }
                            else
                            {
                                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSUM);
                            }
                        }
                        else
                        {
                            ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSUM);
                        }
                        n1C = 0;
                        n14 = M2Share.GetValNameNo(QuestActionInfo.sParam2);
                        if (n14 >= 0)
                        {
                            if (HUtil32.RangeInDefined(n14, 0, 9))
                            {
                                n1C = PlayObject.m_nVal[n14];
                            }
                            else if (HUtil32.RangeInDefined(n14, 100, 119))
                            {
                                n1C = M2Share.g_Config.GlobalVal[n14 - 100];
                            }
                            else if (HUtil32.RangeInDefined(n14, 200, 209))
                            {
                                n1C = PlayObject.m_DyVal[n14 - 200];
                            }
                            else if (HUtil32.RangeInDefined(n14, 300, 399))
                            {
                                n1C = PlayObject.m_nMval[n14 - 300];
                            }
                            else if (HUtil32.RangeInDefined(n14, 400, 499))
                            {
                                n1C = M2Share.g_Config.GlobaDyMval[n14 - 400];
                            }
                            else
                            {
                                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSUM);
                            }
                        }
                        else
                        {
                            ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSUM);
                        }
                        n14 = M2Share.GetValNameNo(QuestActionInfo.sParam1);
                        if (n14 >= 0)
                        {
                            if (HUtil32.RangeInDefined(n14, 0, 9))
                            {
                                PlayObject.m_nVal[9] = PlayObject.m_nVal[9] + n18 + n1C;
                            }
                            else if (HUtil32.RangeInDefined(n14, 100, 119))
                            {
                                M2Share.g_Config.GlobalVal[9] = M2Share.g_Config.GlobalVal[9] + n18 + n1C;
                            }
                            else if (HUtil32.RangeInDefined(n14, 200, 209))
                            {
                                PlayObject.m_DyVal[9] = PlayObject.m_DyVal[9] + n18 + n1C;
                            }
                            else if (HUtil32.RangeInDefined(n14, 300, 399))
                            {
                                PlayObject.m_nMval[99] = PlayObject.m_nMval[99] + n18 + n1C;
                            }
                            else if (HUtil32.RangeInDefined(n14, 400, 499))
                            {
                                M2Share.g_Config.GlobaDyMval[99] = (short)(M2Share.g_Config.GlobaDyMval[99] + n18 + n1C);
                            }
                        }
                        break;
                    case M2Share.nBREAKTIMERECALL:
                        PlayObject.m_boTimeRecall = false;
                        break;
                    case M2Share.nCHANGEMODE:
                        switch (QuestActionInfo.nParam1)
                        {
                            case 1:
                                PlayObject.CmdChangeAdminMode("", 10, "", HUtil32.Str_ToInt(QuestActionInfo.sParam2, 0) == 1);
                                break;
                            case 2:
                                PlayObject.CmdChangeSuperManMode("", 10, "", HUtil32.Str_ToInt(QuestActionInfo.sParam2, 0) == 1);
                                break;
                            case 3:
                                PlayObject.CmdChangeObMode("", 10, "", HUtil32.Str_ToInt(QuestActionInfo.sParam2, 0) == 1);
                                break;
                            default:
                                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sCHANGEMODE);
                                break;
                        }
                        break;
                    case M2Share.nPKPOINT:
                        if (QuestActionInfo.nParam1 == 0)
                        {
                            PlayObject.m_nPkPoint = 0;
                        }
                        else
                        {
                            if (QuestActionInfo.nParam1 < 0)
                            {
                                if ((PlayObject.m_nPkPoint + QuestActionInfo.nParam1) >= 0)
                                {
                                    PlayObject.m_nPkPoint += QuestActionInfo.nParam1;
                                }
                                else
                                {
                                    PlayObject.m_nPkPoint = 0;
                                }
                            }
                            else
                            {
                                if ((PlayObject.m_nPkPoint + QuestActionInfo.nParam1) > 10000)
                                {
                                    PlayObject.m_nPkPoint = 10000;
                                }
                                else
                                {
                                    PlayObject.m_nPkPoint += QuestActionInfo.nParam1;
                                }
                            }
                        }
                        PlayObject.RefNameColor();
                        break;
                    case M2Share.nCHANGEXP:
                        break;
                    case M2Share.nSC_RECALLMOB:
                        ActionOfRecallmob(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nKICK:
                        PlayObject.m_boReconnection = true;
                        PlayObject.m_boSoftClose = true;
                        break;
                    case M2Share.nMOVR:
                        n14 = M2Share.GetValNameNo(QuestActionInfo.sParam1);
                        if (n14 >= 0)
                        {
                            if (HUtil32.RangeInDefined(n14, 0, 9))
                            {
                                PlayObject.m_nVal[n14] = M2Share.RandomNumber.Random(QuestActionInfo.nParam2);
                            }
                            else if (HUtil32.RangeInDefined(n14, 100, 119))
                            {
                                M2Share.g_Config.GlobalVal[n14 - 100] = M2Share.RandomNumber.Random(QuestActionInfo.nParam2);
                            }
                            else if (HUtil32.RangeInDefined(n14, 200, 209))
                            {
                                PlayObject.m_DyVal[n14 - 200] = M2Share.RandomNumber.Random(QuestActionInfo.nParam2);
                            }
                            else if (HUtil32.RangeInDefined(n14, 300, 399))
                            {
                                PlayObject.m_nMval[n14 - 300] = M2Share.RandomNumber.Random(QuestActionInfo.nParam2);
                            }
                            else if (HUtil32.RangeInDefined(n14, 400, 499))
                            {
                                M2Share.g_Config.GlobaDyMval[n14 - 400] = (short)M2Share.RandomNumber.Random(QuestActionInfo.nParam2);
                            }
                            else
                            {
                                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sMOVR);
                            }
                        }
                        else
                        {
                            ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sMOVR);
                        }
                        break;
                    case M2Share.nEXCHANGEMAP:
                        Envir = M2Share.g_MapManager.FindMap(QuestActionInfo.sParam1);
                        if (Envir != null)
                        {
                            List58 = new List<TBaseObject>();
                            M2Share.UserEngine.GetMapRageHuman(Envir, 0, 0, 1000, List58);
                            if (List58.Count > 0)
                            {
                                User = (TPlayObject)List58[0];
                                User.MapRandomMove(this.m_sMapName, 0);
                            }
                            PlayObject.MapRandomMove(QuestActionInfo.sParam1, 0);
                        }
                        else
                        {
                            ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sEXCHANGEMAP);
                        }
                        break;
                    case M2Share.nRECALLMAP:
                        Envir = M2Share.g_MapManager.FindMap(QuestActionInfo.sParam1);
                        if (Envir != null)
                        {
                            List58 = new List<TBaseObject>();
                            M2Share.UserEngine.GetMapRageHuman(Envir, 0, 0, 1000, List58);
                            for (var k = 0; k < List58.Count; k++)
                            {
                                User = (TPlayObject)List58[k];
                                User.MapRandomMove(this.m_sMapName, 0);
                                if (k > 20)
                                {
                                    break;
                                }
                            }
                            //List58.Free;
                        }
                        else
                        {
                            ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sRECALLMAP);
                        }
                        break;
                    case M2Share.nADDBATCH:
                        if (BatchParamsList == null)
                        {
                            BatchParamsList = new List<TScriptParams>();
                        }
                        BatchParamsList.Add(new TScriptParams()
                        {
                            sParams = QuestActionInfo.sParam1,
                            nParams = n18
                        });
                        break;
                    case M2Share.nBATCHDELAY:
                        n18 = QuestActionInfo.nParam1 * 1000;
                        break;
                    case M2Share.nBATCHMOVE:
                        var n20 = 0;
                        for (var k = 0; k < BatchParamsList.Count; k ++ )
                        {
                            var batchParam = BatchParamsList[k];
                            PlayObject.SendDelayMsg(this.ObjectId, Grobal2.RM_10155, 0, 0, 0, 0, BatchParamsList[k].sParams, batchParam.nParams + n20);
                            n20 += batchParam.nParams;
                        }
                        break;
                    case M2Share.nPLAYDICE:
                        PlayObject.m_sPlayDiceLabel = QuestActionInfo.sParam2;
                        PlayObject.SendMsg(this, Grobal2.RM_PLAYDICE, (short)QuestActionInfo.nParam1, HUtil32.MakeLong(HUtil32.MakeWord(PlayObject.m_DyVal[0], PlayObject.m_DyVal[1]), HUtil32.MakeWord(PlayObject.m_DyVal[2], PlayObject.m_DyVal[3])), HUtil32.MakeLong(HUtil32.MakeWord(PlayObject.m_DyVal[4], PlayObject.m_DyVal[5]), HUtil32.MakeWord(PlayObject.m_DyVal[6], PlayObject.m_DyVal[7])), HUtil32.MakeLong(HUtil32.MakeWord(PlayObject.m_DyVal[8], PlayObject.m_DyVal[9]), 0), QuestActionInfo.sParam2);
                        bo11 = true;
                        break;
                    case M2Share.nADDNAMELIST:
                        GotoLable_AddList(PlayObject.m_sCharName, m_sPath + QuestActionInfo.sParam1);
                        break;
                    case M2Share.nDELNAMELIST:
                        GotoLable_DelList(PlayObject.m_sCharName, m_sPath + QuestActionInfo.sParam1);
                        break;
                    case M2Share.nADDUSERDATE:
                        GotoLable_AddUseDateList(PlayObject.m_sCharName, m_sPath + QuestActionInfo.sParam1);
                        break;
                    case M2Share.nDELUSERDATE:
                        GotoLable_DELUseDateList(PlayObject.m_sCharName, m_sPath + QuestActionInfo.sParam1);
                        break;
                    case M2Share.nADDGUILDLIST:
                        if (PlayObject.m_MyGuild != null)
                        {
                            GotoLable_AddList(PlayObject.m_MyGuild.sGuildName, m_sPath + QuestActionInfo.sParam1);
                        }
                        break;
                    case M2Share.nDELGUILDLIST:
                        if (PlayObject.m_MyGuild != null)
                        {
                            GotoLable_DelList(PlayObject.m_MyGuild.sGuildName, m_sPath + QuestActionInfo.sParam1);
                        }
                        break;
                    case M2Share.nSC_LINEMSG:
                    case M2Share.nSENDMSG:
                        ActionOfLineMsg(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nADDACCOUNTLIST:
                        GotoLable_AddList(PlayObject.m_sUserID, m_sPath + QuestActionInfo.sParam1);
                        break;
                    case M2Share.nDELACCOUNTLIST:
                        GotoLable_DelList(PlayObject.m_sUserID, m_sPath + QuestActionInfo.sParam1);
                        break;
                    case M2Share.nADDIPLIST:
                        GotoLable_AddList(PlayObject.m_sIPaddr, m_sPath + QuestActionInfo.sParam1);
                        break;
                    case M2Share.nDELIPLIST:
                        GotoLable_DelList(PlayObject.m_sIPaddr, m_sPath + QuestActionInfo.sParam1);
                        break;
                    case M2Share.nGOQUEST:
                        GotoLable_GoToQuest(PlayObject, QuestActionInfo.nParam1);
                        break;
                    case M2Share.nENDQUEST:
                        PlayObject.m_Script = null;
                        break;
                    case M2Share.nGOTO:
                        if (!GotoLable_JmpToLable(PlayObject, QuestActionInfo.sParam1))
                        {
                            // ScriptActionError(PlayObject,'',QuestActionInfo,sGOTO);
                            M2Share.MainOutMessage("[脚本死循环] NPC:" + this.m_sCharName + " 位置:" + this.m_sMapName + '(' + this.m_nCurrX + ':' + this.m_nCurrY + ')' + " 命令:" + M2Share.sGOTO + ' ' + QuestActionInfo.sParam1);
                            result = false;
                            return result;
                        }
                        break;
                    case M2Share.nSC_HAIRCOLOR:
                        break;
                    case M2Share.nSC_WEARCOLOR:
                        break;
                    case M2Share.nSC_HAIRSTYLE:
                        ActionOfChangeHairStyle(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_MONRECALL:
                        break;
                    case M2Share.nSC_HORSECALL:
                        break;
                    case M2Share.nSC_HAIRRNDCOL:
                        break;
                    case M2Share.nSC_KILLHORSE:
                        break;
                    case M2Share.nSC_RANDSETDAILYQUEST:
                        break;
                    case M2Share.nSC_RECALLGROUPMEMBERS:
                        ActionOfRecallGroupMembers(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_CLEARNAMELIST:
                        ActionOfClearNameList(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_MAPTING:
                        ActionOfMapTing(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_CHANGELEVEL:
                        ActionOfChangeLevel(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_MARRY:
                        ActionOfMarry(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_MASTER:
                        ActionOfMaster(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_UNMASTER:
                        ActionOfUnMaster(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_UNMARRY:
                        ActionOfUnMarry(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_GETMARRY:
                        ActionOfGetMarry(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_GETMASTER:
                        ActionOfGetMaster(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_CLEARSKILL:
                        ActionOfClearSkill(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_DELNOJOBSKILL:
                        ActionOfDelNoJobSkill(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_DELSKILL:
                        ActionOfDelSkill(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_ADDSKILL:
                        ActionOfAddSkill(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_SKILLLEVEL:
                        ActionOfSkillLevel(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_CHANGEPKPOINT:
                        ActionOfChangePkPoint(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_CHANGEEXP:
                        ActionOfChangeExp(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_CHANGEJOB:
                        ActionOfChangeJob(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_MISSION:
                        ActionOfMission(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_MOBPLACE:
                        ActionOfMobPlace(PlayObject, QuestActionInfo, n34, n38, n3C, n40);
                        break;
                    case M2Share.nSC_SETMEMBERTYPE:
                        ActionOfSetMemberType(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_SETMEMBERLEVEL:
                        ActionOfSetMemberLevel(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_GAMEGOLD:
                        // nSC_SETMEMBERTYPE:   PlayObject.m_nMemberType:=Str_ToInt(QuestActionInfo.sParam1,0);
                        // nSC_SETMEMBERLEVEL:  PlayObject.m_nMemberType:=Str_ToInt(QuestActionInfo.sParam1,0);
                        ActionOfGameGold(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_GAMEPOINT:
                        ActionOfGamePoint(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_OffLine:
                        ActionOfOffLine(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_AUTOADDGAMEGOLD: // 增加挂机
                        ActionOfAutoAddGameGold(PlayObject, QuestActionInfo, n34, n38);
                        break;
                    case M2Share.nSC_AUTOSUBGAMEGOLD:
                        ActionOfAutoSubGameGold(PlayObject, QuestActionInfo, n34, n38);
                        break;
                    case M2Share.nSC_CHANGENAMECOLOR:
                        ActionOfChangeNameColor(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_CLEARPASSWORD:
                        ActionOfClearPassword(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_RENEWLEVEL:
                        ActionOfReNewLevel(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_KILLSLAVE:
                        ActionOfKillSlave(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_CHANGEGENDER:
                        ActionOfChangeGender(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_KILLMONEXPRATE:
                        ActionOfKillMonExpRate(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_POWERRATE:
                        ActionOfPowerRate(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_CHANGEMODE:
                        ActionOfChangeMode(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_CHANGEPERMISSION:
                        ActionOfChangePerMission(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_KILL:
                        ActionOfKill(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_KICK:
                        ActionOfKick(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_BONUSPOINT:
                        ActionOfBonusPoint(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_RESTRENEWLEVEL:
                        ActionOfRestReNewLevel(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_DELMARRY:
                        ActionOfDelMarry(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_DELMASTER:
                        ActionOfDelMaster(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_CREDITPOINT:
                        ActionOfChangeCreditPoint(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_CLEARNEEDITEMS:
                        ActionOfClearNeedItems(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_CLEARMAEKITEMS:
                        ActionOfClearMakeItems(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_SETSENDMSGFLAG:
                        PlayObject.m_boSendMsgFlag = true;
                        break;
                    case M2Share.nSC_UPGRADEITEMS:
                        ActionOfUpgradeItems(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_UPGRADEITEMSEX:
                        ActionOfUpgradeItemsEx(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_MONGENEX:
                        ActionOfMonGenEx(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_CLEARMAPMON:
                        ActionOfClearMapMon(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_SETMAPMODE:
                        ActionOfSetMapMode(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_PKZONE:
                        ActionOfPkZone(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_RESTBONUSPOINT:
                        ActionOfRestBonusPoint(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_TAKECASTLEGOLD:
                        ActionOfTakeCastleGold(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_HUMANHP:
                        ActionOfHumanHP(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_HUMANMP:
                        ActionOfHumanMP(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_BUILDPOINT:
                        ActionOfGuildBuildPoint(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_DELAYGOTO:
                        ActionOfDelayCall(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_AURAEPOINT:
                        ActionOfGuildAuraePoint(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_STABILITYPOINT:
                        ActionOfGuildstabilityPoint(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_FLOURISHPOINT:
                        ActionOfGuildFlourishPoint(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_OPENMAGICBOX:
                        ActionOfOpenMagicBox(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_SETRANKLEVELNAME:
                        ActionOfSetRankLevelName(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_GMEXECUTE:
                        ActionOfGmExecute(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_GUILDCHIEFITEMCOUNT:
                        ActionOfGuildChiefItemCount(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_ADDNAMEDATELIST:
                        ActionOfAddNameDateList(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_DELNAMEDATELIST:
                        ActionOfDelNameDateList(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_MOBFIREBURN:
                        ActionOfMobFireBurn(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_MESSAGEBOX:
                        ActionOfMessageBox(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_SETSCRIPTFLAG:
                        ActionOfSetScriptFlag(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_SETAUTOGETEXP:
                        ActionOfAutoGetExp(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_VAR:
                        ActionOfVar(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_LOADVAR:
                        ActionOfLoadVar(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_SAVEVAR:
                        ActionOfSaveVar(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_CALCVAR:
                        ActionOfCalcVar(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_GUILDRECALL:
                        ActionOfGuildRecall(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_GROUPADDLIST:
                        ActionOfGroupAddList(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_CLEARLIST:
                        ActionOfClearList(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_GROUPRECALL:
                        ActionOfGroupRecall(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_GROUPMOVEMAP:
                        ActionOfGroupMoveMap(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_REPAIRALL:
                        ActionOfRepairAllItem(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nSC_QUERYBAGITEMS:// 刷新包裹
                        if ((HUtil32.GetTickCount() - PlayObject.m_dwQueryBagItemsTick) > M2Share.g_Config.dwQueryBagItemsTick)
                        {
                            PlayObject.m_dwQueryBagItemsTick = HUtil32.GetTickCount();
                            PlayObject.ClientQueryBagItems();
                        }
                        else
                        {
                            PlayObject.SysMsg(M2Share.g_sQUERYBAGITEMS, TMsgColor.c_Red, TMsgType.t_Hint);
                        }
                        break;
                    case M2Share.nSC_SETRANDOMNO:
                        while (true)
                        {
                            n2C = M2Share.RandomNumber.Random(999999);
                            if ((n2C >= 1000) && (n2C.ToString() != PlayObject.m_sRandomNo))
                            {
                                PlayObject.m_sRandomNo = n2C.ToString();
                                break;
                            }
                        }
                        break;
                    case M2Share.nOPENYBDEAL:
                        ActionOfOPENYBDEAL(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nQUERYYBSELL:
                        ActionOfQUERYYBSELL(PlayObject, QuestActionInfo);
                        break;
                    case M2Share.nQUERYYBDEAL:
                        ActionOfQUERYYBDEAL(PlayObject, QuestActionInfo);
                        break;
                }
            }
            return result;
        }

        private void GotoLable_SendMerChantSayMsg(TPlayObject PlayObject, string sMsg, bool boFlag)
        {
            string s10 = string.Empty;
            string s14 = sMsg;
            int nC = 0;
            while (true)
            {
                if (HUtil32.TagCount(s14, '>') < 1)
                {
                    break;
                }
                s14 = HUtil32.ArrestStringEx(s14, '<', '>', ref s10);
                GetVariableText(PlayObject, ref sMsg, s10);
                nC++;
                if (nC >= 101)
                {
                    break;
                }
            }
            PlayObject.GetScriptLabel(sMsg);
            if (boFlag)
            {
                PlayObject.SendFirstMsg(this, Grobal2.RM_MERCHANTSAY, 0, 0, 0, 0, this.m_sCharName + '/' + sMsg);
            }
            else
            {
                PlayObject.SendMsg(this, Grobal2.RM_MERCHANTSAY, 0, 0, 0, 0, this.m_sCharName + '/' + sMsg);
            }
        }

        private void GotoLable(TPlayObject PlayObject, string sLabel, bool boExtJmp, string sMsg)
        {
            bool bo11;
            string sSendMsg;
            TScript Script = null;
            TScript Script3C = null;
            TSayingRecord SayingRecord;
            TSayingProcedure SayingProcedure;
            TUserItem UserItem = null;
            string sC = string.Empty;
            if (PlayObject.m_NPC != this)
            {
                PlayObject.m_NPC = null;
                PlayObject.m_Script = null;
                //FillChar(PlayObject.m_nVal, sizeof(PlayObject.m_nVal), '\0');
            }
            if (string.Compare(sLabel, "@main", StringComparison.OrdinalIgnoreCase) == 0)
            {
                for (var i = 0; i < m_ScriptList.Count; i++)
                {
                    Script3C = m_ScriptList[i];
                    if (Script3C.RecordList.TryGetValue(sLabel.ToLower(), out SayingRecord))
                    {
                        Script = Script3C;
                        PlayObject.m_Script = Script;
                        PlayObject.m_NPC = this;
                        break;
                    }
                }
            }
            if (Script == null)
            {
                if (PlayObject.m_Script != null)
                {
                    for (var i = m_ScriptList.Count - 1; i >= 0; i--)
                    {
                        if (m_ScriptList[i] == PlayObject.m_Script)
                        {
                            Script = m_ScriptList[i];
                        }
                    }
                }
                if (Script == null)
                {
                    for (var i = m_ScriptList.Count - 1; i >= 0; i--)
                    {
                        if (GotoLable_CheckQuestStatus(PlayObject, m_ScriptList[i]))
                        {
                            Script = m_ScriptList[i];
                            PlayObject.m_Script = Script;
                            PlayObject.m_NPC = this;
                        }
                    }
                }
            }
            // 跳转到指定示签，执行
            if (Script != null)
            {
                if (Script.RecordList.TryGetValue(sLabel.ToLower(), out SayingRecord))
                {
                    if (boExtJmp && SayingRecord.boExtJmp == false)
                    {
                        return;
                    }
                    sSendMsg = "";
                    for (var i = 0; i < SayingRecord.ProcedureList.Count; i++)
                    {
                        SayingProcedure = SayingRecord.ProcedureList[i];
                        bo11 = false;
                        if (GotoLable_QuestCheckCondition(PlayObject, SayingProcedure.ConditionList, ref sC, ref UserItem))
                        {
                            sSendMsg = sSendMsg + SayingProcedure.sSayMsg;
                            if (!GotoLable_QuestActionProcess(PlayObject, SayingProcedure.ActionList, ref sC, ref UserItem,ref bo11))
                            {
                                break;
                            }
                            if (bo11)
                            {
                                GotoLable_SendMerChantSayMsg(PlayObject, sSendMsg, true);
                            }
                        }
                        else
                        {
                            sSendMsg = sSendMsg + SayingProcedure.sElseSayMsg;
                            if (!GotoLable_QuestActionProcess(PlayObject, SayingProcedure.ElseActionList, ref sC, ref UserItem,ref bo11))
                            {
                                break;
                            }
                            if (bo11)
                            {
                                GotoLable_SendMerChantSayMsg(PlayObject, sSendMsg, true);
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(sSendMsg))
                    {
                        GotoLable_SendMerChantSayMsg(PlayObject, sSendMsg, false);
                    }
                }
            }
        }

        public void GotoLable(TPlayObject PlayObject, string sLabel, bool boExtJmp)
        {
            GotoLable(PlayObject, sLabel, boExtJmp, "");
        }
    }
}