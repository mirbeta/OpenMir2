using GameSvr.Actor;
using GameSvr.Items;
using GameSvr.Maps;
using GameSvr.Player;
using GameSvr.Script;
using SystemModule;
using SystemModule.Common;
using SystemModule.Data;
using SystemModule.Packet.ClientPackets;

namespace GameSvr.Npc
{
    public partial class NormNpc
    {
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
            }
            if (string.Compare(sLabel, "@main", StringComparison.OrdinalIgnoreCase) == 0)
            {
                for (var i = 0; i < m_ScriptList.Count; i++)
                {
                    Script3C = m_ScriptList[i];
                    if (Script3C.RecordList.TryGetValue(sLabel, out SayingRecord))
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
                if (Script.RecordList.TryGetValue(sLabel, out SayingRecord))
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
                        if (GotoLableQuestCheckCondition(PlayObject, SayingProcedure.ConditionList, ref sC, ref UserItem))
                        {
                            sSendMsg = sSendMsg + SayingProcedure.sSayMsg;
                            if (!GotoLableQuestActionProcess(PlayObject, SayingProcedure.ActionList, ref sC, ref UserItem, ref bo11))
                            {
                                break;
                            }
                            if (bo11)
                            {
                                GotoLableSendMerChantSayMsg(PlayObject, sSendMsg, true);
                            }
                        }
                        else
                        {
                            sSendMsg = sSendMsg + SayingProcedure.sElseSayMsg;
                            if (!GotoLableQuestActionProcess(PlayObject, SayingProcedure.ElseActionList, ref sC, ref UserItem, ref bo11))
                            {
                                break;
                            }
                            if (bo11)
                            {
                                GotoLableSendMerChantSayMsg(PlayObject, sSendMsg, true);
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(sSendMsg))
                    {
                        GotoLableSendMerChantSayMsg(PlayObject, sSendMsg, false);
                    }
                }
            }
        }

        public void GotoLable(TPlayObject PlayObject, string sLabel, bool boExtJmp)
        {
            GotoLable(PlayObject, sLabel, boExtJmp, "");
        }

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

        private TUserItem GotoLable_CheckItemW(TPlayObject PlayObject, string sItemType, int nParam)
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
            result = PlayObject.CheckItemCount(sItemType, ref nCount);
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
                    if (string.Compare(LoadList[i].Trim(), sHumName, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        result = true;
                        break;
                    }
                }
                LoadList = null;
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
                if (HUtil32.RangeInDefined(n14, 0, 99))
                {
                    PlayObject.m_nVal[n14] = nCount;
                }
                else if (HUtil32.RangeInDefined(n14, 100, 119))
                {
                    M2Share.g_Config.GlobalVal[n14 - 100] = nCount;
                }
                else if (HUtil32.RangeInDefined(n14, 200, 299))
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

        private bool GotoLable_QuestCheckCondition_CheckDieMon(TPlayObject PlayObject, string MonName)
        {
            bool result = string.IsNullOrEmpty(MonName);
            if ((PlayObject.m_LastHiter != null) && (PlayObject.m_LastHiter.m_sCharName == MonName))
            {
                result = true;
            }
            return result;
        }

        private bool GotoLable_QuestCheckCondition_CheckKillMon(TPlayObject PlayObject, string MonName)
        {
            bool result = string.IsNullOrEmpty(MonName);
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

        private bool GotoLable_QuestCheckCondition_CheckUserDateType(TPlayObject PlayObject, string charName, string sListFileName, string sDay, string param1, string param2)
        {
            string sText = string.Empty;
            string Name = string.Empty;
            string ssDay = string.Empty;
            var result = false;
            sListFileName = M2Share.g_Config.sEnvirDir + sListFileName;
            var LoadList = new StringList();
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
                var nDay = HUtil32.Str_ToInt(sDay, 0);
                for (var i = 0; i < LoadList.Count; i++)
                {
                    sText = LoadList[i].Trim();
                    sText = HUtil32.GetValidStrCap(sText, ref Name, new string[] { " ", "\t" });
                    Name = Name.Trim();
                    if (charName == Name)
                    {
                        ssDay = sText.Trim();
                        var nnday = HUtil32.Str_ToDate(ssDay);
                        var UseDay = HUtil32.Round(DateTime.Today.ToOADate() - nnday.ToOADate());
                        var LastDay = nDay - UseDay;
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

        private bool GotoLableQuestCheckCondition(TPlayObject PlayObject, IList<TQuestConditionInfo> ConditionList, ref string sC, ref TUserItem UserItem)
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
            Envirnoment Envir;
            StdItem StdItem;
            for (var i = 0; i < ConditionList.Count; i++)
            {
                QuestConditionInfo = ConditionList[i];
                switch (QuestConditionInfo.nCmdCode)
                {
                    case ScriptConst.nCHECKUSERDATE:
                        result = GotoLable_QuestCheckCondition_CheckUserDateType(PlayObject, PlayObject.m_sCharName, m_sPath + QuestConditionInfo.sParam1, QuestConditionInfo.sParam3, QuestConditionInfo.sParam4, QuestConditionInfo.sParam5);
                        break;
                    case ScriptConst.nSC_CHECKRANDOMNO:
                        Console.WriteLine("TODO nSC_CHECKRANDOMNO...");
                        //result = GotoLable_QuestCheckCondition_CheckRandomNo(PlayObject, sMsg);
                        break;
                    case ScriptConst.nCheckDiemon:
                        result = GotoLable_QuestCheckCondition_CheckDieMon(PlayObject, QuestConditionInfo.sParam1);
                        break;
                    case ScriptConst.ncheckkillplaymon:
                        result = GotoLable_QuestCheckCondition_CheckKillMon(PlayObject, QuestConditionInfo.sParam1);
                        break;
                    case ScriptConst.nCHECK:
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
                    case ScriptConst.nRANDOM:
                        if (M2Share.RandomNumber.Random(QuestConditionInfo.nParam1) != 0)
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nGENDER:
                        if (String.Compare(QuestConditionInfo.sParam1, ScriptConst.sMAN, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (PlayObject.Gender != PlayGender.Man)
                            {
                                result = false;
                            }
                        }
                        else
                        {
                            if (PlayObject.Gender != PlayGender.WoMan)
                            {
                                result = false;
                            }
                        }
                        break;
                    case ScriptConst.nDAYTIME:
                        if (String.Compare(QuestConditionInfo.sParam1, ScriptConst.sSUNRAISE, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (M2Share.g_nGameTime != 0)
                            {
                                result = false;
                            }
                        }
                        if (String.Compare(QuestConditionInfo.sParam1, ScriptConst.sDAY, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (M2Share.g_nGameTime != 1)
                            {
                                result = false;
                            }
                        }
                        if (String.Compare(QuestConditionInfo.sParam1, ScriptConst.sSUNSET, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (M2Share.g_nGameTime != 2)
                            {
                                result = false;
                            }
                        }
                        if (String.Compare(QuestConditionInfo.sParam1, ScriptConst.sNIGHT, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (M2Share.g_nGameTime != 3)
                            {
                                result = false;
                            }
                        }
                        break;
                    case ScriptConst.nCHECKOPEN:
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
                    case ScriptConst.nCHECKUNIT:
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
                    case ScriptConst.nCHECKLEVEL:
                        if (PlayObject.m_Abil.Level < QuestConditionInfo.nParam1)
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nCHECKJOB:
                        if (HUtil32.CompareLStr(QuestConditionInfo.sParam1, ScriptConst.sWarrior, ScriptConst.sWarrior.Length))
                        {
                            if (PlayObject.m_btJob != PlayJob.Warrior)
                            {
                                result = false;
                            }
                        }
                        if (HUtil32.CompareLStr(QuestConditionInfo.sParam1, ScriptConst.sWizard, ScriptConst.sWizard.Length))
                        {
                            if (PlayObject.m_btJob != PlayJob.Wizard)
                            {
                                result = false;
                            }
                        }
                        if (HUtil32.CompareLStr(QuestConditionInfo.sParam1, ScriptConst.sTaos, ScriptConst.sTaos.Length))
                        {
                            if (PlayObject.m_btJob != PlayJob.Taoist)
                            {
                                result = false;
                            }
                        }
                        break;
                    case ScriptConst.nCHECKBBCOUNT:
                        if (PlayObject.m_SlaveList.Count < QuestConditionInfo.nParam1)
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nCHECKCREDITPOINT:
                        break;
                    case ScriptConst.nCHECKITEM:
                        UserItem = PlayObject.QuestCheckItem(QuestConditionInfo.sParam1, ref n1C, ref nMaxDura, ref nDura);
                        if (n1C < QuestConditionInfo.nParam2)
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nCHECKITEMW:
                        UserItem = GotoLable_CheckItemW(PlayObject, QuestConditionInfo.sParam1, QuestConditionInfo.nParam2);
                        if (UserItem == null)
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nCHECKGOLD:
                        if (PlayObject.m_nGold < QuestConditionInfo.nParam1)
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nISTAKEITEM:
                        if (sC != QuestConditionInfo.sParam1)
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nCHECKDURA:
                        UserItem = PlayObject.QuestCheckItem(QuestConditionInfo.sParam1, ref n1C, ref nMaxDura, ref nDura);
                        if (HUtil32.Round(nDura / 1000) < QuestConditionInfo.nParam2)
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nCHECKDURAEVA:
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
                    case ScriptConst.nDAYOFWEEK:
                        if (HUtil32.CompareLStr(QuestConditionInfo.sParam1, ScriptConst.sSUN, ScriptConst.sSUN.Length))
                        {
                            if ((int)DateTime.Now.DayOfWeek != 1)
                            {
                                result = false;
                            }
                        }
                        if (HUtil32.CompareLStr(QuestConditionInfo.sParam1, ScriptConst.sMON, ScriptConst.sMON.Length))
                        {
                            if ((int)DateTime.Now.DayOfWeek != 2)
                            {
                                result = false;
                            }
                        }
                        if (HUtil32.CompareLStr(QuestConditionInfo.sParam1, ScriptConst.sTUE, ScriptConst.sTUE.Length))
                        {
                            if ((int)DateTime.Now.DayOfWeek != 3)
                            {
                                result = false;
                            }
                        }
                        if (HUtil32.CompareLStr(QuestConditionInfo.sParam1, ScriptConst.sWED, ScriptConst.sWED.Length))
                        {
                            if ((int)DateTime.Now.DayOfWeek != 4)
                            {
                                result = false;
                            }
                        }
                        if (HUtil32.CompareLStr(QuestConditionInfo.sParam1, ScriptConst.sTHU, ScriptConst.sTHU.Length))
                        {
                            if ((int)DateTime.Now.DayOfWeek != 5)
                            {
                                result = false;
                            }
                        }
                        if (HUtil32.CompareLStr(QuestConditionInfo.sParam1, ScriptConst.sFRI, ScriptConst.sFRI.Length))
                        {
                            if ((int)DateTime.Now.DayOfWeek != 6)
                            {
                                result = false;
                            }
                        }
                        if (HUtil32.CompareLStr(QuestConditionInfo.sParam1, ScriptConst.sSAT, ScriptConst.sSAT.Length))
                        {
                            if ((int)DateTime.Now.DayOfWeek != 7)
                            {
                                result = false;
                            }
                        }
                        break;
                    case ScriptConst.nHOUR:
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
                    case ScriptConst.nMIN:
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
                    case ScriptConst.nCHECKPKPOINT:
                        if (PlayObject.PKLevel() < QuestConditionInfo.nParam1)
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nCHECKLUCKYPOINT:
                        if (PlayObject.m_nBodyLuckLevel < QuestConditionInfo.nParam1)
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nCHECKMONMAP:
                        Envir = M2Share.MapManager.FindMap(QuestConditionInfo.sParam1);
                        if (Envir != null)
                        {
                            if (M2Share.UserEngine.GetMapMonster(Envir, null) < QuestConditionInfo.nParam2)
                            {
                                result = false;
                            }
                        }
                        break;
                    case ScriptConst.nCHECKMONAREA:
                        break;
                    case ScriptConst.nCHECKHUM:
                        if (M2Share.UserEngine.GetMapHuman(QuestConditionInfo.sParam1) < QuestConditionInfo.nParam2)
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nCHECKBAGGAGE:
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
                    case ScriptConst.nCHECKNAMELIST:
                        if (!GotoLable_CheckStringList(PlayObject.m_sCharName, m_sPath + QuestConditionInfo.sParam1))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nCHECKACCOUNTLIST:
                        if (!GotoLable_CheckStringList(PlayObject.m_sUserID, m_sPath + QuestConditionInfo.sParam1))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nCHECKIPLIST:
                        if (!GotoLable_CheckStringList(PlayObject.m_sIPaddr, m_sPath + QuestConditionInfo.sParam1))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nEQUAL:
                        result = EqualData(PlayObject, QuestConditionInfo);
                        break;
                    case ScriptConst.nLARGE:
                        result = LargeData(PlayObject, QuestConditionInfo);
                        break;
                    case ScriptConst.nSMALL:
                        result = Smalldata(PlayObject, QuestConditionInfo);
                        break;
                    case ScriptConst.nSC_ISSYSOP:
                        if (!(PlayObject.m_btPermission >= 4))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_ISADMIN:
                        if (!(PlayObject.m_btPermission >= 6))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKGROUPCOUNT:
                        if (!ConditionOfCheckGroupCount(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKPOSEDIR:
                        if (!ConditionOfCheckPoseDir(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKPOSELEVEL:
                        if (!ConditionOfCheckPoseLevel(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKPOSEGENDER:
                        if (!ConditionOfCheckPoseGender(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKLEVELEX:
                        if (!ConditionOfCheckLevelEx(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKBONUSPOINT:
                        if (!ConditionOfCheckBonusPoint(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKMARRY:
                        if (!ConditionOfCheckMarry(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKPOSEMARRY:
                        if (!ConditionOfCheckPoseMarry(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKMARRYCOUNT:
                        if (!ConditionOfCheckMarryCount(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKMASTER:
                        if (!ConditionOfCheckMaster(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_HAVEMASTER:
                        if (!ConditionOfHaveMaster(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKPOSEMASTER:
                        if (!ConditionOfCheckPoseMaster(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_POSEHAVEMASTER:
                        if (!ConditionOfPoseHaveMaster(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKISMASTER:
                        if (!ConditionOfCheckIsMaster(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_HASGUILD:
                        if (!ConditionOfCheckHaveGuild(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_ISGUILDMASTER:
                        if (!ConditionOfCheckIsGuildMaster(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKCASTLEMASTER:
                        if (!ConditionOfCheckIsCastleMaster(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_ISCASTLEGUILD:
                        if (!ConditionOfCheckIsCastleaGuild(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_ISATTACKGUILD:
                        if (!ConditionOfCheckIsAttackGuild(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_ISDEFENSEGUILD:
                        if (!ConditionOfCheckIsDefenseGuild(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKCASTLEDOOR:
                        if (!ConditionOfCheckCastleDoorStatus(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_ISATTACKALLYGUILD:
                        if (!ConditionOfCheckIsAttackAllyGuild(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_ISDEFENSEALLYGUILD:
                        if (!ConditionOfCheckIsDefenseAllyGuild(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKPOSEISMASTER:
                        if (!ConditionOfCheckPoseIsMaster(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKNAMEIPLIST:
                        if (!ConditionOfCheckNameIPList(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKACCOUNTIPLIST:
                        if (!ConditionOfCheckAccountIPList(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKSLAVECOUNT:
                        if (!ConditionOfCheckSlaveCount(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_ISNEWHUMAN:
                        if (!PlayObject.m_boNewHuman)
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKMEMBERTYPE:
                        if (!ConditionOfCheckMemberType(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKMEMBERLEVEL:
                        if (!ConditionOfCheckMemBerLevel(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKGAMEGOLD:
                        if (!ConditionOfCheckGameGold(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKGAMEPOINT:
                        if (!ConditionOfCheckGamePoint(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKNAMELISTPOSITION:
                        if (!ConditionOfCheckNameListPostion(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKGUILDLIST:
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
                    case ScriptConst.nSC_CHECKRENEWLEVEL:
                        if (!ConditionOfCheckReNewLevel(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKSLAVELEVEL:
                        if (!ConditionOfCheckSlaveLevel(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKSLAVENAME:
                        if (!ConditionOfCheckSlaveName(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKCREDITPOINT:
                        if (!ConditionOfCheckCreditPoint(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKOFGUILD:
                        if (!ConditionOfCheckOfGuild(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKPAYMENT:
                        if (!ConditionOfCheckPayMent(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKUSEITEM:
                        if (!ConditionOfCheckUseItem(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKBAGSIZE:
                        if (!ConditionOfCheckBagSize(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKLISTCOUNT:
                        if (!ConditionOfCheckListCount(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKDC:
                        if (!ConditionOfCheckDC(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKMC:
                        if (!ConditionOfCheckMC(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKSC:
                        if (!ConditionOfCheckSC(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKHP:
                        if (!ConditionOfCheckHP(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKMP:
                        if (!ConditionOfCheckMP(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKITEMTYPE:
                        if (!ConditionOfCheckItemType(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKEXP:
                        if (!ConditionOfCheckExp(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKCASTLEGOLD:
                        if (!ConditionOfCheckCastleGold(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_PASSWORDERRORCOUNT:
                        if (!ConditionOfCheckPasswordErrorCount(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_ISLOCKPASSWORD:
                        if (!ConditionOfIsLockPassword(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_ISLOCKSTORAGE:
                        if (!ConditionOfIsLockStorage(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKBUILDPOINT:
                        if (!ConditionOfCheckGuildBuildPoint(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKAURAEPOINT:
                        if (!ConditionOfCheckGuildAuraePoint(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKSTABILITYPOINT:
                        if (!ConditionOfCheckStabilityPoint(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKFLOURISHPOINT:
                        if (!ConditionOfCheckFlourishPoint(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKCONTRIBUTION:
                        if (!ConditionOfCheckContribution(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKRANGEMONCOUNT:
                        if (!ConditionOfCheckRangeMonCount(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKITEMADDVALUE:
                        if (!ConditionOfCheckItemAddValue(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKINMAPRANGE:
                        if (!ConditionOfCheckInMapRange(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CASTLECHANGEDAY:
                        if (!ConditionOfCheckCastleChageDay(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CASTLEWARDAY:
                        if (!ConditionOfCheckCastleWarDay(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_ONLINELONGMIN:
                        if (!ConditionOfCheckOnlineLongMin(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKGUILDCHIEFITEMCOUNT:
                        if (!ConditionOfCheckChiefItemCount(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKNAMEDATELIST:
                        if (!ConditionOfCheckNameDateList(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKMAPHUMANCOUNT:
                        if (!ConditionOfCheckMapHumanCount(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKMAPMONCOUNT:
                        if (!ConditionOfCheckMapMonCount(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKVAR:
                        if (!ConditionOfCheckVar(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKSERVERNAME:
                        if (!ConditionOfCheckServerName(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKISONMAP:
                        if (!ConditionOfCheckIsOnMap(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_KILLBYHUM:
                        if ((PlayObject.m_LastHiter != null) && (PlayObject.m_LastHiter.m_btRaceServer != Grobal2.RC_PLAYOBJECT))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_KILLBYMON:
                        if ((PlayObject.m_LastHiter != null) && (PlayObject.m_LastHiter.m_btRaceServer == Grobal2.RC_PLAYOBJECT))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKINSAFEZONE:
                        if (!PlayObject.InSafeZone())
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKMAP:
                        if (!ConditionOfCheckMap(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKPOS:
                        if (!ConditionOfCheckPos(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_REVIVESLAVE:
                        if (!ConditionOfReviveSlave(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKMAGICLVL:
                        if (!ConditionOfCheckMagicLvl(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKGROUPCLASS:
                        if (!ConditionOfCheckGroupClass(PlayObject, QuestConditionInfo))
                        {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_ISGROUPMASTER:
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
                    case ScriptConst.nSC_ISHIGH:
                        result = ConditionOfIsHigh(PlayObject, QuestConditionInfo);
                        break;
                    case ScriptConst.nSCHECKDEATHPLAYMON:
                        var s01 = string.Empty;
                        if (!GetValValue(PlayObject, QuestConditionInfo.sParam1, ref s01))
                        {
                            s01 = GetLineVariableText(PlayObject, QuestConditionInfo.sParam1);
                        }
                        result = CheckKillMon2(PlayObject, s01);
                        break;
                }
                if (!result)
                {
                    break;
                }
            }
            return result;
        }

        private bool CheckKillMon2(TPlayObject PlayObject, string sMonName)
        {
            return true;
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
                    GotoLable(PlayObject, ScriptConst.sMAIN, false);
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
                if (String.Compare(sHumName, s10, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    bo15 = true;
                    break;
                }
            }
            if (!bo15)
            {
                s10 = string.Format("{0}    {1}", new object[] { sHumName, DateTime.Today });
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
                if (string.Compare(sHumName, s10, StringComparison.OrdinalIgnoreCase) == 0)
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
                if (String.Compare(sHumName, s10, StringComparison.OrdinalIgnoreCase) == 0)
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
                if (String.Compare(sHumName, s10, StringComparison.OrdinalIgnoreCase) == 0)
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
        }

        private void GotoLable_TakeItem(TPlayObject PlayObject, string sItemName, int nItemCount, ref string sC)
        {
            TUserItem UserItem;
            StdItem StdItem;
            if (String.Compare(sItemName, Grobal2.sSTRING_GOLDNAME, StringComparison.OrdinalIgnoreCase) == 0)
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
                if (String.Compare(M2Share.UserEngine.GetStdItemName(UserItem.wIndex), sItemName, StringComparison.OrdinalIgnoreCase) == 0)
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
            StdItem StdItem;
            if (String.Compare(sItemName, Grobal2.sSTRING_GOLDNAME, StringComparison.OrdinalIgnoreCase) == 0)
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
            for (var i = 0; i < PlayObject.m_UseItems.Length; i++)
            {
                if (nItemCount <= 0)
                {
                    return;
                }
                if (PlayObject.m_UseItems[i].wIndex > 0)
                {
                    sName = M2Share.UserEngine.GetStdItemName(PlayObject.m_UseItems[i].wIndex);
                    if (String.Compare(sName, sItemName, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        PlayObject.SendDelItems(PlayObject.m_UseItems[i]);
                        PlayObject.m_UseItems[i].wIndex = 0;
                        nItemCount -= 1;
                    }
                }
            }
        }

        private bool GotoLableQuestActionProcess(TPlayObject PlayObject, IList<TQuestActionInfo> ActionList, ref string sC, ref TUserItem UserItem, ref bool bo11)
        {
            bool result = true;
            TQuestActionInfo QuestActionInfo;
            int n28;
            int n2C;
            int n20X;
            int n24Y;
            string s4C = string.Empty;
            string s50 = string.Empty;
            string s34 = string.Empty;
            string s44 = string.Empty;
            string s48 = string.Empty;
            Envirnoment Envir;
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
                    case ScriptConst.nSET:
                        n28 = HUtil32.Str_ToInt(QuestActionInfo.sParam1, 0);
                        n2C = HUtil32.Str_ToInt(QuestActionInfo.sParam2, 0);
                        PlayObject.SetQuestFlagStatus(n28, n2C);
                        break;
                    case ScriptConst.nTAKE:
                        GotoLable_TakeItem(PlayObject, QuestActionInfo.sParam1, QuestActionInfo.nParam2, ref sC);
                        break;
                    case ScriptConst.nSC_GIVE:
                        ActionOfGiveItem(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nTAKEW:
                        GotoLable_TakeWItem(PlayObject, QuestActionInfo.sParam1, QuestActionInfo.nParam2);
                        break;
                    case ScriptConst.nCLOSE:
                        PlayObject.SendMsg(this, Grobal2.RM_MERCHANTDLGCLOSE, 0, this.ObjectId, 0, 0, "");
                        break;
                    case ScriptConst.nRESET:
                        for (var k = 0; k < QuestActionInfo.nParam2; k++)
                        {
                            PlayObject.SetQuestFlagStatus(QuestActionInfo.nParam1 + k, 0);
                        }
                        break;
                    case ScriptConst.nSETOPEN:
                        n28 = HUtil32.Str_ToInt(QuestActionInfo.sParam1, 0);
                        n2C = HUtil32.Str_ToInt(QuestActionInfo.sParam2, 0);
                        PlayObject.SetQuestUnitOpenStatus(n28, n2C);
                        break;
                    case ScriptConst.nSETUNIT:
                        n28 = HUtil32.Str_ToInt(QuestActionInfo.sParam1, 0);
                        n2C = HUtil32.Str_ToInt(QuestActionInfo.sParam2, 0);
                        PlayObject.SetQuestUnitStatus(n28, n2C);
                        break;
                    case ScriptConst.nRESETUNIT:
                        for (var k = 0; k < QuestActionInfo.nParam2; k++)
                        {
                            PlayObject.SetQuestUnitStatus(QuestActionInfo.nParam1 + k, 0);
                        }
                        break;
                    case ScriptConst.nBREAK:
                        result = false;
                        break;
                    case ScriptConst.nTIMERECALL:
                        PlayObject.m_boTimeRecall = true;
                        PlayObject.m_sMoveMap = PlayObject.m_sMapName;
                        PlayObject.m_nMoveX = PlayObject.m_nCurrX;
                        PlayObject.m_nMoveY = PlayObject.m_nCurrY;
                        PlayObject.m_dwTimeRecallTick = HUtil32.GetTickCount() + (QuestActionInfo.nParam1 * 60 * 1000);
                        break;
                    case ScriptConst.nSC_PARAM1:
                        n34 = QuestActionInfo.nParam1;
                        s44 = QuestActionInfo.sParam1;
                        break;
                    case ScriptConst.nSC_PARAM2:
                        n38 = QuestActionInfo.nParam1;
                        s48 = QuestActionInfo.sParam1;
                        break;
                    case ScriptConst.nSC_PARAM3:
                        n3C = QuestActionInfo.nParam1;
                        s4C = QuestActionInfo.sParam1;
                        break;
                    case ScriptConst.nSC_PARAM4:
                        n40 = QuestActionInfo.nParam1;
                        s50 = QuestActionInfo.sParam1;
                        break;
                    case ScriptConst.nSC_EXEACTION:
                        n40 = QuestActionInfo.nParam1;
                        s50 = QuestActionInfo.sParam1;
                        ExeAction(PlayObject, QuestActionInfo.sParam1, QuestActionInfo.sParam2, QuestActionInfo.sParam3, QuestActionInfo.nParam1, QuestActionInfo.nParam2, QuestActionInfo.nParam3);
                        break;
                    case ScriptConst.nMAPMOVE:
                        PlayObject.SendRefMsg(Grobal2.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
                        PlayObject.SpaceMove(QuestActionInfo.sParam1, (short)QuestActionInfo.nParam2, (short)QuestActionInfo.nParam3, 0);
                        bo11 = true;
                        break;
                    case ScriptConst.nMAP:
                        PlayObject.SendRefMsg(Grobal2.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
                        PlayObject.MapRandomMove(QuestActionInfo.sParam1, 0);
                        bo11 = true;
                        break;
                    case ScriptConst.nTAKECHECKITEM:
                        if (UserItem != null)
                        {
                            PlayObject.QuestTakeCheckItem(UserItem);
                        }
                        else
                        {
                            ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sTAKECHECKITEM);
                        }
                        break;
                    case ScriptConst.nMONGEN:
                        for (var k = 0; k < QuestActionInfo.nParam2; k++)
                        {
                            n20X = M2Share.RandomNumber.Random(QuestActionInfo.nParam3 * 2 + 1) + (n38 - QuestActionInfo.nParam3);
                            n24Y = M2Share.RandomNumber.Random(QuestActionInfo.nParam3 * 2 + 1) + (n3C - QuestActionInfo.nParam3);
                            M2Share.UserEngine.RegenMonsterByName(s44, (short)n20X, (short)n24Y, QuestActionInfo.sParam1);
                        }
                        break;
                    case ScriptConst.nMONCLEAR:
                        List58 = new List<TBaseObject>();
                        M2Share.UserEngine.GetMapMonster(M2Share.MapManager.FindMap(QuestActionInfo.sParam1), List58);
                        for (var k = 0; k < List58.Count; k++)
                        {
                            List58[k].m_boNoItem = true;
                            List58[k].m_WAbil.HP = 0;
                        }
                        //List58.Free;
                        break;
                    case ScriptConst.nMOV:
                        MovData(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nINC:
                        IncInteger(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nDEC:
                        DecInteger(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSUM:
                        SumData(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_DIV:
                        DivData(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_MUL:
                        MulData(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_PERCENT:
                        PercentData(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nBREAKTIMERECALL:
                        PlayObject.m_boTimeRecall = false;
                        break;
                    case ScriptConst.nCHANGEMODE:
                        switch (QuestActionInfo.nParam1)
                        {
                            case 1:
                                M2Share.CommandSystem.ExecCmd("ChangeAdminMode", PlayObject);
                                break;
                            case 2:
                                M2Share.CommandSystem.ExecCmd("ChangeSuperManMode", PlayObject);
                                break;
                            case 3:
                                M2Share.CommandSystem.ExecCmd("ChangeObMode", PlayObject);
                                break;
                            default:
                                ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sCHANGEMODE);
                                break;
                        }
                        break;
                    case ScriptConst.nPKPOINT:
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
                    case ScriptConst.nCHANGEXP:
                        break;
                    case ScriptConst.nSC_RECALLMOB:
                        ActionOfRecallmob(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nKICK:
                        PlayObject.m_boReconnection = true;
                        PlayObject.m_boSoftClose = true;
                        break;
                    case ScriptConst.nTHROWITEM://将指定物品刷新到指定地图坐标范围内
                        ActionOfTHROWITEM(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nMOVR:
                        MovrData(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nEXCHANGEMAP:
                        Envir = M2Share.MapManager.FindMap(QuestActionInfo.sParam1);
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
                            ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sEXCHANGEMAP);
                        }
                        break;
                    case ScriptConst.nRECALLMAP:
                        Envir = M2Share.MapManager.FindMap(QuestActionInfo.sParam1);
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
                            ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sRECALLMAP);
                        }
                        break;
                    case ScriptConst.nADDBATCH:
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
                    case ScriptConst.nBATCHDELAY:
                        n18 = QuestActionInfo.nParam1 * 1000;
                        break;
                    case ScriptConst.nBATCHMOVE:
                        var n20 = 0;
                        for (var k = 0; k < BatchParamsList.Count; k++)
                        {
                            var batchParam = BatchParamsList[k];
                            PlayObject.SendDelayMsg(this.ObjectId, Grobal2.RM_10155, 0, 0, 0, 0, BatchParamsList[k].sParams, batchParam.nParams + n20);
                            n20 += batchParam.nParams;
                        }
                        break;
                    case ScriptConst.nPLAYDICE:
                        PlayObject.m_sPlayDiceLabel = QuestActionInfo.sParam2;
                        PlayObject.SendMsg(this, Grobal2.RM_PLAYDICE, (short)QuestActionInfo.nParam1, HUtil32.MakeLong(HUtil32.MakeWord(PlayObject.m_DyVal[0], PlayObject.m_DyVal[1]), HUtil32.MakeWord(PlayObject.m_DyVal[2], PlayObject.m_DyVal[3])), HUtil32.MakeLong(HUtil32.MakeWord(PlayObject.m_DyVal[4], PlayObject.m_DyVal[5]), HUtil32.MakeWord(PlayObject.m_DyVal[6], PlayObject.m_DyVal[7])), HUtil32.MakeLong(HUtil32.MakeWord(PlayObject.m_DyVal[8], PlayObject.m_DyVal[9]), 0), QuestActionInfo.sParam2);
                        bo11 = true;
                        break;
                    case ScriptConst.nADDNAMELIST:
                        GotoLable_AddList(PlayObject.m_sCharName, m_sPath + QuestActionInfo.sParam1);
                        break;
                    case ScriptConst.nDELNAMELIST:
                        GotoLable_DelList(PlayObject.m_sCharName, m_sPath + QuestActionInfo.sParam1);
                        break;
                    case ScriptConst.nADDUSERDATE:
                        GotoLable_AddUseDateList(PlayObject.m_sCharName, m_sPath + QuestActionInfo.sParam1);
                        break;
                    case ScriptConst.nDELUSERDATE:
                        GotoLable_DELUseDateList(PlayObject.m_sCharName, m_sPath + QuestActionInfo.sParam1);
                        break;
                    case ScriptConst.nADDGUILDLIST:
                        if (PlayObject.m_MyGuild != null)
                        {
                            GotoLable_AddList(PlayObject.m_MyGuild.sGuildName, m_sPath + QuestActionInfo.sParam1);
                        }
                        break;
                    case ScriptConst.nDELGUILDLIST:
                        if (PlayObject.m_MyGuild != null)
                        {
                            GotoLable_DelList(PlayObject.m_MyGuild.sGuildName, m_sPath + QuestActionInfo.sParam1);
                        }
                        break;
                    case ScriptConst.nSC_LINEMSG:
                    case ScriptConst.nSENDMSG:
                        ActionOfLineMsg(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nADDACCOUNTLIST:
                        GotoLable_AddList(PlayObject.m_sUserID, m_sPath + QuestActionInfo.sParam1);
                        break;
                    case ScriptConst.nDELACCOUNTLIST:
                        GotoLable_DelList(PlayObject.m_sUserID, m_sPath + QuestActionInfo.sParam1);
                        break;
                    case ScriptConst.nADDIPLIST:
                        GotoLable_AddList(PlayObject.m_sIPaddr, m_sPath + QuestActionInfo.sParam1);
                        break;
                    case ScriptConst.nDELIPLIST:
                        GotoLable_DelList(PlayObject.m_sIPaddr, m_sPath + QuestActionInfo.sParam1);
                        break;
                    case ScriptConst.nGOQUEST:
                        GotoLable_GoToQuest(PlayObject, QuestActionInfo.nParam1);
                        break;
                    case ScriptConst.nENDQUEST:
                        PlayObject.m_Script = null;
                        break;
                    case ScriptConst.nGOTO:
                        if (!GotoLable_JmpToLable(PlayObject, QuestActionInfo.sParam1))
                        {
                            // ScriptActionError(PlayObject,'',QuestActionInfo,sGOTO);
                            M2Share.MainOutMessage("[脚本死循环] NPC:" + this.m_sCharName + " 位置:" + this.m_sMapName + '(' + this.m_nCurrX + ':' + this.m_nCurrY + ')' + " 命令:" + ScriptConst.sGOTO + ' ' + QuestActionInfo.sParam1);
                            result = false;
                            return result;
                        }
                        break;
                    case ScriptConst.nSC_HAIRCOLOR:
                        break;
                    case ScriptConst.nSC_WEARCOLOR:
                        break;
                    case ScriptConst.nSC_HAIRSTYLE:
                        ActionOfChangeHairStyle(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_MONRECALL:
                        break;
                    case ScriptConst.nSC_HORSECALL:
                        break;
                    case ScriptConst.nSC_HAIRRNDCOL:
                        break;
                    case ScriptConst.nSC_KILLHORSE:
                        break;
                    case ScriptConst.nSC_RANDSETDAILYQUEST:
                        break;
                    case ScriptConst.nSC_RECALLGROUPMEMBERS:
                        ActionOfRecallGroupMembers(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_CLEARNAMELIST:
                        ActionOfClearNameList(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_MAPTING:
                        ActionOfMapTing(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_CHANGELEVEL:
                        ActionOfChangeLevel(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_MARRY:
                        ActionOfMarry(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_MASTER:
                        ActionOfMaster(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_UNMASTER:
                        ActionOfUnMaster(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_UNMARRY:
                        ActionOfUnMarry(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_GETMARRY:
                        ActionOfGetMarry(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_GETMASTER:
                        ActionOfGetMaster(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_CLEARSKILL:
                        ActionOfClearSkill(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_DELNOJOBSKILL:
                        ActionOfDelNoJobSkill(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_DELSKILL:
                        ActionOfDelSkill(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_ADDSKILL:
                        ActionOfAddSkill(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_SKILLLEVEL:
                        ActionOfSkillLevel(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_CHANGEPKPOINT:
                        ActionOfChangePkPoint(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_CHANGEEXP:
                        ActionOfChangeExp(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_CHANGEJOB:
                        ActionOfChangeJob(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_MISSION:
                        ActionOfMission(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_MOBPLACE:
                        ActionOfMobPlace(PlayObject, QuestActionInfo, n34, n38, n3C, n40);
                        break;
                    case ScriptConst.nSC_SETMEMBERTYPE:
                        ActionOfSetMemberType(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_SETMEMBERLEVEL:
                        ActionOfSetMemberLevel(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_GAMEGOLD:
                        // nSC_SETMEMBERTYPE:   PlayObject.m_nMemberType:=Str_ToInt(QuestActionInfo.sParam1,0);
                        // nSC_SETMEMBERLEVEL:  PlayObject.m_nMemberType:=Str_ToInt(QuestActionInfo.sParam1,0);
                        ActionOfGameGold(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_GAMEPOINT:
                        ActionOfGamePoint(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_OffLine:
                        ActionOfOffLine(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_AUTOADDGAMEGOLD: // 增加挂机
                        ActionOfAutoAddGameGold(PlayObject, QuestActionInfo, n34, n38);
                        break;
                    case ScriptConst.nSC_AUTOSUBGAMEGOLD:
                        ActionOfAutoSubGameGold(PlayObject, QuestActionInfo, n34, n38);
                        break;
                    case ScriptConst.nSC_CHANGENAMECOLOR:
                        ActionOfChangeNameColor(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_CLEARPASSWORD:
                        ActionOfClearPassword(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_RENEWLEVEL:
                        ActionOfReNewLevel(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_KILLSLAVE:
                        ActionOfKillSlave(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_CHANGEGENDER:
                        ActionOfChangeGender(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_KILLMONEXPRATE:
                        ActionOfKillMonExpRate(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_POWERRATE:
                        ActionOfPowerRate(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_CHANGEMODE:
                        ActionOfChangeMode(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_CHANGEPERMISSION:
                        ActionOfChangePerMission(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_KILL:
                        ActionOfKill(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_KICK:
                        ActionOfKick(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_BONUSPOINT:
                        ActionOfBonusPoint(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_RESTRENEWLEVEL:
                        ActionOfRestReNewLevel(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_DELMARRY:
                        ActionOfDelMarry(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_DELMASTER:
                        ActionOfDelMaster(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_CREDITPOINT:
                        ActionOfChangeCreditPoint(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_CLEARNEEDITEMS:
                        ActionOfClearNeedItems(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_CLEARMAEKITEMS:
                        ActionOfClearMakeItems(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_SETSENDMSGFLAG:
                        PlayObject.m_boSendMsgFlag = true;
                        break;
                    case ScriptConst.nSC_UPGRADEITEMS:
                        ActionOfUpgradeItems(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_UPGRADEITEMSEX:
                        ActionOfUpgradeItemsEx(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_MONGENEX:
                        ActionOfMonGenEx(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_CLEARMAPMON:
                        ActionOfClearMapMon(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_SETMAPMODE:
                        ActionOfSetMapMode(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_PKZONE:
                        ActionOfPkZone(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_RESTBONUSPOINT:
                        ActionOfRestBonusPoint(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_TAKECASTLEGOLD:
                        ActionOfTakeCastleGold(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_HUMANHP:
                        ActionOfHumanHP(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_HUMANMP:
                        ActionOfHumanMP(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_BUILDPOINT:
                        ActionOfGuildBuildPoint(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_DELAYGOTO:
                        ActionOfDelayCall(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_AURAEPOINT:
                        ActionOfGuildAuraePoint(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_STABILITYPOINT:
                        ActionOfGuildstabilityPoint(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_FLOURISHPOINT:
                        ActionOfGuildFlourishPoint(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_OPENMAGICBOX:
                        ActionOfOpenMagicBox(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_SETRANKLEVELNAME:
                        ActionOfSetRankLevelName(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_GMEXECUTE:
                        ActionOfGmExecute(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_GUILDCHIEFITEMCOUNT:
                        ActionOfGuildChiefItemCount(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_ADDNAMEDATELIST:
                        ActionOfAddNameDateList(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_DELNAMEDATELIST:
                        ActionOfDelNameDateList(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_MOBFIREBURN:
                        ActionOfMobFireBurn(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_MESSAGEBOX:
                        ActionOfMessageBox(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_SETSCRIPTFLAG:
                        ActionOfSetScriptFlag(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_SETAUTOGETEXP:
                        ActionOfAutoGetExp(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_VAR:
                        ActionOfVar(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_LOADVAR:
                        ActionOfLoadVar(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_SAVEVAR:
                        ActionOfSaveVar(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_CALCVAR:
                        ActionOfCalcVar(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_GUILDRECALL:
                        ActionOfGuildRecall(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_GROUPADDLIST:
                        ActionOfGroupAddList(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_CLEARLIST:
                        ActionOfClearList(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_GROUPRECALL:
                        ActionOfGroupRecall(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_GROUPMOVEMAP:
                        ActionOfGroupMoveMap(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_REPAIRALL:
                        ActionOfRepairAllItem(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_QUERYBAGITEMS:// 刷新包裹
                        if ((HUtil32.GetTickCount() - PlayObject.m_dwQueryBagItemsTick) > M2Share.g_Config.dwQueryBagItemsTick)
                        {
                            PlayObject.m_dwQueryBagItemsTick = HUtil32.GetTickCount();
                            PlayObject.ClientQueryBagItems();
                        }
                        else
                        {
                            PlayObject.SysMsg(M2Share.g_sQUERYBAGITEMS, MsgColor.Red, MsgType.Hint);
                        }
                        break;
                    case ScriptConst.nSC_SETRANDOMNO:
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
                    case ScriptConst.nOPENYBDEAL:
                        ActionOfOPENYBDEAL(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nQUERYYBSELL:
                        ActionOfQUERYYBSELL(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nQUERYYBDEAL:
                        ActionOfQUERYYBDEAL(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nDELAYGOTO:
                        PlayObject.m_boTimeGoto = true;
                        var m_DelayGoto = HUtil32.Str_ToInt(GetLineVariableText(PlayObject, QuestActionInfo.sParam1), 0);//变量操作
                        if (m_DelayGoto == 0)
                        {
                            var delayCount = 0;
                            GetValValue(PlayObject, QuestActionInfo.sParam1, ref delayCount);
                            m_DelayGoto = delayCount;
                        }
                        if (m_DelayGoto > 0)
                        {
                            PlayObject.m_dwTimeGotoTick = HUtil32.GetTickCount() + m_DelayGoto;
                        }
                        else
                        {
                            PlayObject.m_dwTimeGotoTick = HUtil32.GetTickCount() + QuestActionInfo.nParam1;//毫秒
                        }
                        PlayObject.m_sTimeGotoLable = QuestActionInfo.sParam2;
                        PlayObject.m_TimeGotoNPC = this;
                        break;
                    case ScriptConst.nCLEARDELAYGOTO:
                        PlayObject.m_boTimeGoto = false;
                        PlayObject.m_sTimeGotoLable = "";
                        PlayObject.m_TimeGotoNPC = null;
                        break;
                }
            }
            return result;
        }

        private void GotoLableSendMerChantSayMsg(TPlayObject PlayObject, string sMsg, bool boFlag)
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
                s14 = HUtil32.ArrestStringEx(s14, "<", ">", ref s10);
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
    }
}