using GameSrv.Actor;
using GameSrv.GameCommand;
using GameSrv.Items;
using GameSrv.Maps;
using GameSrv.Player;
using GameSrv.Script;
using SystemModule.Common;
using SystemModule.Enums;
using SystemModule.Packets.ClientPackets;

namespace GameSrv.Npc {
    public partial class NormNpc {
        private void GotoLable(PlayObject playObject, string sLabel, bool boExtJmp, string sMsg) {
            if (playObject.LastNpc != this.ActorId) {
                playObject.LastNpc = 0;
                playObject.MScript = null;
            }
            ScriptInfo script = null;
            SayingRecord sayingRecord;
            UserItem userItem = null;
            string sC = string.Empty;
            if (string.Compare("@main", sLabel, StringComparison.OrdinalIgnoreCase) == 0) {
                for (int i = 0; i < m_ScriptList.Count; i++) {
                    ScriptInfo script3C = m_ScriptList[i];
                    if (script3C.RecordList.TryGetValue(sLabel, out _)) {
                        script = script3C;
                        playObject.MScript = script;
                        playObject.LastNpc = this.ActorId;
                        break;
                    }
                }
            }
            if (script == null) {
                if (playObject.MScript != null) {
                    for (int i = m_ScriptList.Count - 1; i >= 0; i--) {
                        if (m_ScriptList[i] == playObject.MScript) {
                            script = m_ScriptList[i];
                        }
                    }
                }
                if (script == null) {
                    for (int i = m_ScriptList.Count - 1; i >= 0; i--) {
                        if (CheckGotoLableQuestStatus(playObject, m_ScriptList[i])) {
                            script = m_ScriptList[i];
                            playObject.MScript = script;
                            playObject.LastNpc = this.ActorId;
                        }
                    }
                }
            }
            // 跳转到指定示签，执行
            if (script != null) {
                if (script.RecordList.TryGetValue(sLabel, out sayingRecord)) {
                    if (boExtJmp && sayingRecord.boExtJmp == false) {
                        return;
                    }
                    string sSendMsg = string.Empty;
                    for (int i = 0; i < sayingRecord.ProcedureList.Count; i++) {
                        SayingProcedure sayingProcedure = sayingRecord.ProcedureList[i];
                        bool bo11 = false;
                        if (GotoLableQuestCheckCondition(playObject, sayingProcedure.ConditionList, ref sC, ref userItem)) {
                            sSendMsg = sSendMsg + sayingProcedure.sSayMsg;
                            if (!GotoLableQuestActionProcess(playObject, sayingProcedure.ActionList, ref sC, ref userItem, ref bo11)) {
                                break;
                            }
                            if (bo11) {
                                GotoLableSendMerChantSayMsg(playObject, sSendMsg, true);
                            }
                        }
                        else {
                            sSendMsg = sSendMsg + sayingProcedure.sElseSayMsg;
                            if (!GotoLableQuestActionProcess(playObject, sayingProcedure.ElseActionList, ref sC, ref userItem, ref bo11)) {
                                break;
                            }
                            if (bo11) {
                                GotoLableSendMerChantSayMsg(playObject, sSendMsg, true);
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(sSendMsg)) {
                        GotoLableSendMerChantSayMsg(playObject, sSendMsg, false);
                    }
                }
            }
        }

        public void GotoLable(PlayObject playObject, string sLabel, bool boExtJmp) {
            GotoLable(playObject, sLabel, boExtJmp, string.Empty);
        }

        private static bool CheckGotoLableQuestStatus(PlayObject playObject, ScriptInfo scriptInfo) {
            bool result = true;
            if (!scriptInfo.IsQuest) {
                return true;
            }
            int nIndex = 0;
            while (true) {
                if ((scriptInfo.QuestInfo[nIndex].nRandRage > 0) && (M2Share.RandomNumber.Random(scriptInfo.QuestInfo[nIndex].nRandRage) != 0)) {
                    result = false;
                    break;
                }
                if (playObject.GetQuestFalgStatus(scriptInfo.QuestInfo[nIndex].wFlag) != scriptInfo.QuestInfo[nIndex].btValue) {
                    result = false;
                    break;
                }
                nIndex++;
                if (nIndex >= 10) {
                    break;
                }
            }
            return result;
        }

        private static UserItem CheckGotoLableItemW(PlayObject playObject, string sItemType, int nParam) {
            UserItem result = null;
            int nCount = 0;
            if (HUtil32.CompareLStr(sItemType, "[NECKLACE]", 4)) {
                if (playObject.UseItems[ItemLocation.Necklace].Index > 0) {
                    result = playObject.UseItems[ItemLocation.Necklace];
                }
                return result;
            }
            if (HUtil32.CompareLStr(sItemType, "[RING]", 4)) {
                if (playObject.UseItems[ItemLocation.Ringl].Index > 0) {
                    result = playObject.UseItems[ItemLocation.Ringl];
                }
                if (playObject.UseItems[ItemLocation.Ringr].Index > 0) {
                    result = playObject.UseItems[ItemLocation.Ringr];
                }
                return result;
            }
            if (HUtil32.CompareLStr(sItemType, "[ARMRING]", 4)) {
                if (playObject.UseItems[ItemLocation.ArmRingl].Index > 0) {
                    result = playObject.UseItems[ItemLocation.ArmRingl];
                }
                if (playObject.UseItems[ItemLocation.ArmRingr].Index > 0) {
                    result = playObject.UseItems[ItemLocation.ArmRingr];
                }
                return result;
            }
            if (HUtil32.CompareLStr(sItemType, "[WEAPON]", 4)) {
                if (playObject.UseItems[ItemLocation.Weapon].Index > 0) {
                    result = playObject.UseItems[ItemLocation.Weapon];
                }
                return result;
            }
            if (HUtil32.CompareLStr(sItemType, "[HELMET]", 4)) {
                if (playObject.UseItems[ItemLocation.Helmet].Index > 0) {
                    result = playObject.UseItems[ItemLocation.Helmet];
                }
                return result;
            }
            if (HUtil32.CompareLStr(sItemType, "[BUJUK]", 4)) {
                if (playObject.UseItems[ItemLocation.Bujuk].Index > 0) {
                    result = playObject.UseItems[ItemLocation.Bujuk];
                }
                return result;
            }
            if (HUtil32.CompareLStr(sItemType, "[BELT]", 4)) {
                if (playObject.UseItems[ItemLocation.Belt].Index > 0) {
                    result = playObject.UseItems[ItemLocation.Belt];
                }
                return result;
            }
            if (HUtil32.CompareLStr(sItemType, "[BOOTS]", 4)) {
                if (playObject.UseItems[ItemLocation.Boots].Index > 0) {
                    result = playObject.UseItems[ItemLocation.Boots];
                }
                return result;
            }
            if (HUtil32.CompareLStr(sItemType, "[CHARM]", 4)) {
                if (playObject.UseItems[ItemLocation.Charm].Index > 0) {
                    result = playObject.UseItems[ItemLocation.Charm];
                }
                return result;
            }
            result = playObject.CheckItemCount(sItemType, ref nCount);
            if (nCount < nParam) {
                result = null;
            }
            return result;
        }

        private static bool CheckGotoLableStringList(string sHumName, string sListFileName) {
            bool result = false;
            sListFileName = M2Share.GetEnvirFilePath(sListFileName);
            if (File.Exists(sListFileName)) {
                using StringList loadList = new StringList();
                try {
                    loadList.LoadFromFile(sListFileName);
                }
                catch {
                    M2Share.Logger.Error("loading fail.... => " + sListFileName);
                }
                for (int i = 0; i < loadList.Count; i++) {
                    if (string.Compare(loadList[i].Trim(), sHumName, StringComparison.OrdinalIgnoreCase) == 0) {
                        result = true;
                        break;
                    }
                }
            }
            else {
                M2Share.Logger.Error("file not found => " + sListFileName);
            }
            return result;
        }

        private static void GotoLableQuestCheckConditionSetVal(PlayObject playObject, string sIndex, int nCount) {
            int n14 = M2Share.GetValNameNo(sIndex);
            if (n14 >= 0) {
                if (HUtil32.RangeInDefined(n14, 0, 99)) {
                    playObject.MNVal[n14] = nCount;
                }
                else if (HUtil32.RangeInDefined(n14, 100, 119)) {
                    M2Share.Config.GlobalVal[n14 - 100] = nCount;
                }
                else if (HUtil32.RangeInDefined(n14, 200, 299)) {
                    playObject.MDyVal[n14 - 200] = nCount;
                }
                else if (HUtil32.RangeInDefined(n14, 300, 399)) {
                    playObject.MNMval[n14 - 300] = nCount;
                }
                else if (HUtil32.RangeInDefined(n14, 400, 499)) {
                    M2Share.Config.GlobaDyMval[n14 - 400] = (short)nCount;
                }
                else if (HUtil32.RangeInDefined(n14, 500, 599)) {
                    playObject.MNSval[n14 - 600] = nCount.ToString();
                }
            }
        }

        private static bool GotoLable_QuestCheckCondition_CheckDieMon(PlayObject playObject, string monName) {
            bool result = string.IsNullOrEmpty(monName);
            if ((playObject.LastHiter != null) && (playObject.LastHiter.ChrName == monName)) {
                result = true;
            }
            return result;
        }

        private static bool GotoLable_QuestCheckCondition_CheckKillMon(PlayObject playObject, string monName) {
            bool result = string.IsNullOrEmpty(monName);
            if ((playObject.TargetCret != null) && (playObject.TargetCret.ChrName == monName)) {
                result = true;
            }
            return result;
        }

        public static bool GotoLable_QuestCheckCondition_CheckRandomNo(PlayObject playObject, string sNumber) {
            return playObject.RandomNo == sNumber;
        }

        private bool QuestCheckConditionCheckUserDateType(PlayObject playObject, string chrName, string sListFileName, string sDay, string param1, string param2) {
            string name = string.Empty;
            bool result = false;
            sListFileName = M2Share.GetEnvirFilePath(sListFileName);
            using StringList loadList = new StringList();
            if (File.Exists(sListFileName)) {
                try {
                    loadList.LoadFromFile(sListFileName);
                }
                catch {
                    M2Share.Logger.Error("loading fail.... => " + sListFileName);
                }
            }
            int nDay = HUtil32.StrToInt(sDay, 0);
            for (int i = 0; i < loadList.Count; i++) {
                string sText = loadList[i].Trim();
                sText = HUtil32.GetValidStrCap(sText, ref name, new[] { ' ', '\t' });
                name = name.Trim();
                if (chrName == name) {
                    string ssDay = sText.Trim();
                    DateTime nnday = HUtil32.StrToDate(ssDay);
                    int useDay = HUtil32.Round(DateTime.Today.ToOADate() - nnday.ToOADate());
                    int lastDay = nDay - useDay;
                    if (lastDay < 0) {
                        result = true;
                        lastDay = 0;
                    }
                    GotoLableQuestCheckConditionSetVal(playObject, param1, useDay);
                    GotoLableQuestCheckConditionSetVal(playObject, param2, lastDay);
                    return result;
                }
            }
            return false;
        }

        private bool GotoLableQuestCheckCondition(PlayObject playObject, IList<QuestConditionInfo> conditionList, ref string sC, ref UserItem userItem) {
            bool result = true;
            int n1C = 0;
            int nMaxDura = 0;
            int nDura = 0;
            for (int i = 0; i < conditionList.Count; i++) {
                var questConditionInfo = conditionList[i];
                if (!string.IsNullOrEmpty(questConditionInfo.sParam1)) {
                    if (questConditionInfo.sParam1[0] == '$') {
                        string s50 = questConditionInfo.sParam1;
                        questConditionInfo.sParam1 = '<' + questConditionInfo.sParam1 + '>';
                        GetVariableText(playObject, s50, ref questConditionInfo.sParam1);
                    }
                    else if (questConditionInfo.sParam1.IndexOf(">", StringComparison.OrdinalIgnoreCase) > -1) {
                        questConditionInfo.sParam1 = GetLineVariableText(playObject, questConditionInfo.sParam1);
                    }
                }
                if (!string.IsNullOrEmpty(questConditionInfo.sParam2)) {
                    if (questConditionInfo.sParam2[0] == '$') {
                        string s50 = questConditionInfo.sParam2;
                        questConditionInfo.sParam2 = '<' + questConditionInfo.sParam2 + '>';
                        GetVariableText(playObject, s50, ref questConditionInfo.sParam2);
                    }
                    else if (questConditionInfo.sParam2.IndexOf(">", StringComparison.OrdinalIgnoreCase) > -1) {
                        questConditionInfo.sParam2 = GetLineVariableText(playObject, questConditionInfo.sParam2);
                    }
                }
                if (!string.IsNullOrEmpty(questConditionInfo.sParam3)) {
                    if (questConditionInfo.sParam3[0] == '$') {
                        string s50 = questConditionInfo.sParam3;
                        questConditionInfo.sParam3 = '<' + questConditionInfo.sParam3 + '>';
                        GetVariableText(playObject, s50, ref questConditionInfo.sParam3);
                    }
                    else if (questConditionInfo.sParam3.IndexOf(">", StringComparison.OrdinalIgnoreCase) > -1) {
                        questConditionInfo.sParam3 = GetLineVariableText(playObject, questConditionInfo.sParam3);
                    }
                }
                if (!string.IsNullOrEmpty(questConditionInfo.sParam4)) {
                    if (questConditionInfo.sParam4[0] == '$') {
                        string s50 = questConditionInfo.sParam4;
                        questConditionInfo.sParam4 = '<' + questConditionInfo.sParam4 + '>';
                        GetVariableText(playObject, s50, ref questConditionInfo.sParam4);
                    }
                    else if (questConditionInfo.sParam4.IndexOf(">", StringComparison.OrdinalIgnoreCase) > -1) {
                        questConditionInfo.sParam4 = GetLineVariableText(playObject, questConditionInfo.sParam4);
                    }
                }
                if (!string.IsNullOrEmpty(questConditionInfo.sParam5)) {
                    if (questConditionInfo.sParam5[0] == '$') {
                        string s50 = questConditionInfo.sParam5;
                        questConditionInfo.sParam5 = '<' + questConditionInfo.sParam5 + '>';
                        GetVariableText(playObject, s50, ref questConditionInfo.sParam5);
                    }
                    else if (questConditionInfo.sParam5.IndexOf(">", StringComparison.OrdinalIgnoreCase) > -1) {
                        questConditionInfo.sParam5 = GetLineVariableText(playObject, questConditionInfo.sParam5);
                    }
                }
                if (!string.IsNullOrEmpty(questConditionInfo.sParam6)) {
                    if (questConditionInfo.sParam6[0] == '$') {
                        string s50 = questConditionInfo.sParam6;
                        questConditionInfo.sParam6 = '<' + questConditionInfo.sParam6 + '>';
                        GetVariableText(playObject, s50, ref questConditionInfo.sParam6);
                    }
                    else if (questConditionInfo.sParam6.IndexOf(">", StringComparison.OrdinalIgnoreCase) > -1) {
                        questConditionInfo.sParam6 = GetLineVariableText(playObject, questConditionInfo.sParam6);
                    }
                }

                //参数变量解释以主执行人物为依据
                if (!string.IsNullOrEmpty(questConditionInfo.sOpName)) {
                    if (questConditionInfo.sOpName.Length > 2) {
                        if (questConditionInfo.sOpName[1] == '$') {
                            string s50 = questConditionInfo.sOpName;
                            questConditionInfo.sOpName = '<' + questConditionInfo.sOpName + '>';
                            GetVariableText(playObject, s50, ref questConditionInfo.sOpName);
                        }
                        else if (questConditionInfo.sOpName.IndexOf(">", StringComparison.OrdinalIgnoreCase) > -1) {
                            questConditionInfo.sOpName = GetLineVariableText(playObject, questConditionInfo.sOpName);
                        }
                    }
                    PlayObject human = M2Share.WorldEngine.GetPlayObject(questConditionInfo.sOpName);
                    if (human != null) {
                        playObject = human;
                        if (!string.IsNullOrEmpty(questConditionInfo.sOpHName) && string.Compare(questConditionInfo.sOpHName, "H", StringComparison.OrdinalIgnoreCase) == 0) {
                            //todo 英雄
                        }
                    }
                }

                if (HUtil32.IsStringNumber(questConditionInfo.sParam1))
                    questConditionInfo.nParam1 = HUtil32.StrToInt(questConditionInfo.sParam1, 0);
                if (HUtil32.IsStringNumber(questConditionInfo.sParam2))
                    questConditionInfo.nParam2 = HUtil32.StrToInt(questConditionInfo.sParam2, 1);
                if (HUtil32.IsStringNumber(questConditionInfo.sParam3))
                    questConditionInfo.nParam3 = HUtil32.StrToInt(questConditionInfo.sParam3, 1);
                if (HUtil32.IsStringNumber(questConditionInfo.sParam4))
                    questConditionInfo.nParam4 = HUtil32.StrToInt(questConditionInfo.sParam4, 0);
                if (HUtil32.IsStringNumber(questConditionInfo.sParam5))
                    questConditionInfo.nParam5 = HUtil32.StrToInt(questConditionInfo.sParam5, 0);
                if (HUtil32.IsStringNumber(questConditionInfo.sParam6))
                    questConditionInfo.nParam6 = HUtil32.StrToInt(questConditionInfo.sParam6, 0);

                int n10;
                int n14;
                int n18;
                int hour;
                int min;
                int sec;
                int mSec;
                switch (questConditionInfo.CmdCode) {
                    case ScriptConst.nCHECKUSERDATE:
                        result = QuestCheckConditionCheckUserDateType(playObject, playObject.ChrName, m_sPath + questConditionInfo.sParam1, questConditionInfo.sParam3, questConditionInfo.sParam4, questConditionInfo.sParam5);
                        break;
                    case ScriptConst.nSC_CHECKRANDOMNO:
                        M2Share.Logger.Error("TODO nSC_CHECKRANDOMNO...");
                        //result = GotoLable_QuestCheckCondition_CheckRandomNo(PlayObject, sMsg);
                        break;
                    case ScriptConst.nCheckDiemon:
                        result = GotoLable_QuestCheckCondition_CheckDieMon(playObject, questConditionInfo.sParam1);
                        break;
                    case ScriptConst.ncheckkillplaymon:
                        result = GotoLable_QuestCheckCondition_CheckKillMon(playObject, questConditionInfo.sParam1);
                        break;
                    case ScriptConst.nCHECK:
                        n14 = HUtil32.StrToInt(questConditionInfo.sParam1, 0);
                        n18 = HUtil32.StrToInt(questConditionInfo.sParam2, 0);
                        n10 = playObject.GetQuestFalgStatus(n14);
                        if (n10 == 0) {
                            if (n18 != 0) {
                                result = false;
                            }
                        }
                        else {
                            if (n18 == 0) {
                                result = false;
                            }
                        }
                        break;
                    case ScriptConst.nRANDOM:
                        if (M2Share.RandomNumber.Random(questConditionInfo.nParam1) != 0) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nGENDER:
                        if (string.Compare(questConditionInfo.sParam1, ScriptConst.sMAN, StringComparison.OrdinalIgnoreCase) == 0) {
                            if (playObject.Gender != PlayGender.Man) {
                                result = false;
                            }
                        }
                        else {
                            if (playObject.Gender != PlayGender.WoMan) {
                                result = false;
                            }
                        }
                        break;
                    case ScriptConst.nDAYTIME:
                        if (string.Compare(questConditionInfo.sParam1, ScriptConst.sSUNRAISE, StringComparison.OrdinalIgnoreCase) == 0) {
                            if (M2Share.GameTime != 0) {
                                result = false;
                            }
                        }
                        if (string.Compare(questConditionInfo.sParam1, ScriptConst.sDAY, StringComparison.OrdinalIgnoreCase) == 0) {
                            if (M2Share.GameTime != 1) {
                                result = false;
                            }
                        }
                        if (string.Compare(questConditionInfo.sParam1, ScriptConst.sSUNSET, StringComparison.OrdinalIgnoreCase) == 0) {
                            if (M2Share.GameTime != 2) {
                                result = false;
                            }
                        }
                        if (string.Compare(questConditionInfo.sParam1, ScriptConst.sNIGHT, StringComparison.OrdinalIgnoreCase) == 0) {
                            if (M2Share.GameTime != 3) {
                                result = false;
                            }
                        }
                        break;
                    case ScriptConst.nCHECKOPEN:
                        n14 = HUtil32.StrToInt(questConditionInfo.sParam1, 0);
                        n18 = HUtil32.StrToInt(questConditionInfo.sParam2, 0);
                        n10 = playObject.GetQuestUnitOpenStatus(n14);
                        if (n10 == 0) {
                            if (n18 != 0) {
                                result = false;
                            }
                        }
                        else {
                            if (n18 == 0) {
                                result = false;
                            }
                        }
                        break;
                    case ScriptConst.nCHECKUNIT:
                        n14 = HUtil32.StrToInt(questConditionInfo.sParam1, 0);
                        n18 = HUtil32.StrToInt(questConditionInfo.sParam2, 0);
                        n10 = playObject.GetQuestUnitStatus(n14);
                        if (n10 == 0) {
                            if (n18 != 0) {
                                result = false;
                            }
                        }
                        else {
                            if (n18 == 0) {
                                result = false;
                            }
                        }
                        break;
                    case ScriptConst.nCHECKLEVEL:
                        if (playObject.Abil.Level < questConditionInfo.nParam1) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nCHECKJOB:
                        if (HUtil32.CompareLStr(questConditionInfo.sParam1, ScriptConst.sWarrior)) {
                            if (playObject.Job != PlayJob.Warrior) {
                                result = false;
                            }
                        }
                        if (HUtil32.CompareLStr(questConditionInfo.sParam1, ScriptConst.sWizard)) {
                            if (playObject.Job != PlayJob.Wizard) {
                                result = false;
                            }
                        }
                        if (HUtil32.CompareLStr(questConditionInfo.sParam1, ScriptConst.sTaos)) {
                            if (playObject.Job != PlayJob.Taoist) {
                                result = false;
                            }
                        }
                        break;
                    case ScriptConst.nCHECKBBCOUNT:
                        if (playObject.SlaveList.Count < questConditionInfo.nParam1) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nCHECKITEM:
                        userItem = playObject.QuestCheckItem(questConditionInfo.sParam1, ref n1C, ref nMaxDura, ref nDura);
                        if (n1C < questConditionInfo.nParam2) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nCHECKITEMW:
                        userItem = CheckGotoLableItemW(playObject, questConditionInfo.sParam1, questConditionInfo.nParam2);
                        if (userItem == null) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nCHECKGOLD:
                        if (playObject.Gold < questConditionInfo.nParam1) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nISTAKEITEM:
                        if (sC != questConditionInfo.sParam1) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nCHECKDURA:
                        userItem = playObject.QuestCheckItem(questConditionInfo.sParam1, ref n1C, ref nMaxDura, ref nDura);
                        if (HUtil32.Round(nDura / 1000) < questConditionInfo.nParam2) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nCHECKDURAEVA:
                        userItem = playObject.QuestCheckItem(questConditionInfo.sParam1, ref n1C, ref nMaxDura, ref nDura);
                        if (n1C > 0) {
                            if (HUtil32.Round(nMaxDura / n1C / 1000) < questConditionInfo.nParam2) {
                                result = false;
                            }
                        }
                        else {
                            result = false;
                        }
                        break;
                    case ScriptConst.nDAYOFWEEK:
                        if (HUtil32.CompareLStr(questConditionInfo.sParam1, ScriptConst.sSUN)) {
                            if ((int)DateTime.Now.DayOfWeek != 1) {
                                result = false;
                            }
                        }
                        if (HUtil32.CompareLStr(questConditionInfo.sParam1, ScriptConst.sMON)) {
                            if ((int)DateTime.Now.DayOfWeek != 2) {
                                result = false;
                            }
                        }
                        if (HUtil32.CompareLStr(questConditionInfo.sParam1, ScriptConst.sTUE)) {
                            if ((int)DateTime.Now.DayOfWeek != 3) {
                                result = false;
                            }
                        }
                        if (HUtil32.CompareLStr(questConditionInfo.sParam1, ScriptConst.sWED)) {
                            if ((int)DateTime.Now.DayOfWeek != 4) {
                                result = false;
                            }
                        }
                        if (HUtil32.CompareLStr(questConditionInfo.sParam1, ScriptConst.sTHU)) {
                            if ((int)DateTime.Now.DayOfWeek != 5) {
                                result = false;
                            }
                        }
                        if (HUtil32.CompareLStr(questConditionInfo.sParam1, ScriptConst.sFRI)) {
                            if ((int)DateTime.Now.DayOfWeek != 6) {
                                result = false;
                            }
                        }
                        if (HUtil32.CompareLStr(questConditionInfo.sParam1, ScriptConst.sSAT)) {
                            if ((int)DateTime.Now.DayOfWeek != 7) {
                                result = false;
                            }
                        }
                        break;
                    case ScriptConst.nHOUR:
                        if ((questConditionInfo.nParam1 != 0) && (questConditionInfo.nParam2 == 0)) {
                            questConditionInfo.nParam2 = questConditionInfo.nParam1;
                        }
                        hour = DateTime.Now.Hour;
                        min = DateTime.Now.Minute;
                        sec = DateTime.Now.Second;
                        mSec = DateTime.Now.Millisecond;
                        if ((hour < questConditionInfo.nParam1) || (hour > questConditionInfo.nParam2)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nMIN:
                        if ((questConditionInfo.nParam1 != 0) && (questConditionInfo.nParam2 == 0)) {
                            questConditionInfo.nParam2 = questConditionInfo.nParam1;
                        }
                        hour = DateTime.Now.Hour;
                        min = DateTime.Now.Minute;
                        sec = DateTime.Now.Second;
                        mSec = DateTime.Now.Millisecond;
                        if ((min < questConditionInfo.nParam1) || (min > questConditionInfo.nParam2)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nCHECKPKPOINT:
                        if (playObject.PvpLevel() < questConditionInfo.nParam1) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nCHECKLUCKYPOINT:
                        if (playObject.BodyLuckLevel < questConditionInfo.nParam1) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nCHECKMONMAP:
                        Envirnoment envir = M2Share.MapMgr.FindMap(questConditionInfo.sParam1);
                        if (envir != null) {
                            if (M2Share.WorldEngine.GetMapMonster(envir, null) < questConditionInfo.nParam2) {
                                result = false;
                            }
                        }
                        break;
                    case ScriptConst.nCHECKMONAREA:
                        break;
                    case ScriptConst.nCHECKHUM:
                        if (M2Share.WorldEngine.GetMapHuman(questConditionInfo.sParam1) < questConditionInfo.nParam2) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nCHECKBAGGAGE:
                        if (playObject.IsEnoughBag()) {
                            if ((!string.IsNullOrEmpty(questConditionInfo.sParam1))) {
                                result = false;
                                StdItem stdItem = M2Share.WorldEngine.GetStdItem(questConditionInfo.sParam1);
                                if (stdItem != null) {
                                    if (playObject.IsAddWeightAvailable(stdItem.Weight)) {
                                        result = true;
                                    }
                                }
                            }
                        }
                        else {
                            result = false;
                        }
                        break;
                    case ScriptConst.nCHECKNAMELIST:
                        if (!CheckGotoLableStringList(playObject.ChrName, m_sPath + questConditionInfo.sParam1)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nCHECKACCOUNTLIST:
                        if (!CheckGotoLableStringList(playObject.UserAccount, m_sPath + questConditionInfo.sParam1)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nCHECKIPLIST:
                        if (!CheckGotoLableStringList(playObject.LoginIpAddr, m_sPath + questConditionInfo.sParam1)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nEQUAL:
                        result = EqualData(playObject, questConditionInfo);
                        break;
                    case ScriptConst.nLARGE:
                        result = LargeData(playObject, questConditionInfo);
                        break;
                    case ScriptConst.nSMALL:
                        result = Smalldata(playObject, questConditionInfo);
                        break;
                    case ScriptConst.nSC_ISSYSOP:
                        if (!(playObject.Permission >= 4)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_ISADMIN:
                        if (!(playObject.Permission >= 6)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKGROUPCOUNT:
                        if (!ConditionOfCheckGroupCount(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKPOSEDIR:
                        if (!ConditionOfCheckPoseDir(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKPOSELEVEL:
                        if (!ConditionOfCheckPoseLevel(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKPOSEGENDER:
                        if (!ConditionOfCheckPoseGender(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKLEVELEX:
                        if (!ConditionOfCheckLevelEx(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKBONUSPOINT:
                        if (!ConditionOfCheckBonusPoint(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKMARRY:
                        if (!ConditionOfCheckMarry(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKPOSEMARRY:
                        if (!ConditionOfCheckPoseMarry(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKMARRYCOUNT:
                        if (!ConditionOfCheckMarryCount(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKMASTER:
                        if (!ConditionOfCheckMaster(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_HAVEMASTER:
                        if (!ConditionOfHaveMaster(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKPOSEMASTER:
                        if (!ConditionOfCheckPoseMaster(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_POSEHAVEMASTER:
                        if (!ConditionOfPoseHaveMaster(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKISMASTER:
                        if (!ConditionOfCheckIsMaster(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_HASGUILD:
                        if (!ConditionOfCheckHaveGuild(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_ISGUILDMASTER:
                        if (!ConditionOfCheckIsGuildMaster(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKCASTLEMASTER:
                        if (!ConditionOfCheckIsCastleMaster(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_ISCASTLEGUILD:
                        if (!ConditionOfCheckIsCastleaGuild(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_ISATTACKGUILD:
                        if (!ConditionOfCheckIsAttackGuild(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_ISDEFENSEGUILD:
                        if (!ConditionOfCheckIsDefenseGuild(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKCASTLEDOOR:
                        if (!ConditionOfCheckCastleDoorStatus(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_ISATTACKALLYGUILD:
                        if (!ConditionOfCheckIsAttackAllyGuild(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_ISDEFENSEALLYGUILD:
                        if (!ConditionOfCheckIsDefenseAllyGuild(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKPOSEISMASTER:
                        if (!ConditionOfCheckPoseIsMaster(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKNAMEIPLIST:
                        if (!ConditionOfCheckNameIPList(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKACCOUNTIPLIST:
                        if (!ConditionOfCheckAccountIPList(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKSLAVECOUNT:
                        if (!ConditionOfCheckSlaveCount(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_ISNEWHUMAN:
                        if (!playObject.IsNewHuman) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKMEMBERTYPE:
                        if (!ConditionOfCheckMemberType(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKMEMBERLEVEL:
                        if (!ConditionOfCheckMemBerLevel(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKGAMEGOLD:
                        if (!ConditionOfCheckGameGold(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKGAMEPOINT:
                        if (!ConditionOfCheckGamePoint(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKNAMELISTPOSITION:
                        if (!ConditionOfCheckNameListPostion(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKGUILDLIST:
                        if (playObject.MyGuild != null) {
                            if (!CheckGotoLableStringList(playObject.MyGuild.GuildName, m_sPath + questConditionInfo.sParam1)) {
                                result = false;
                            }
                        }
                        else {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKRENEWLEVEL:
                        if (!ConditionOfCheckReNewLevel(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKSLAVELEVEL:
                        if (!ConditionOfCheckSlaveLevel(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKSLAVENAME:
                        if (!ConditionOfCheckSlaveName(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKCREDITPOINT:
                        if (!ConditionOfCheckCreditPoint(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKOFGUILD:
                        if (!ConditionOfCheckOfGuild(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKPAYMENT:
                        if (!ConditionOfCheckPayMent(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKUSEITEM:
                        if (!ConditionOfCheckUseItem(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKBAGSIZE:
                        if (!ConditionOfCheckBagSize(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKLISTCOUNT:
                        if (!ConditionOfCheckListCount(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKDC:
                        if (!ConditionOfCheckDC(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKMC:
                        if (!ConditionOfCheckMC(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKSC:
                        if (!ConditionOfCheckSC(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKHP:
                        if (!ConditionOfCheckHP(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKMP:
                        if (!ConditionOfCheckMP(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKITEMTYPE:
                        if (!ConditionOfCheckItemType(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKEXP:
                        if (!ConditionOfCheckExp(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKCASTLEGOLD:
                        if (!ConditionOfCheckCastleGold(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_PASSWORDERRORCOUNT:
                        if (!ConditionOfCheckPasswordErrorCount(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_ISLOCKPASSWORD:
                        if (!ConditionOfIsLockPassword(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_ISLOCKSTORAGE:
                        if (!ConditionOfIsLockStorage(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKBUILDPOINT:
                        if (!ConditionOfCheckGuildBuildPoint(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKAURAEPOINT:
                        if (!ConditionOfCheckGuildAuraePoint(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKSTABILITYPOINT:
                        if (!ConditionOfCheckStabilityPoint(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKFLOURISHPOINT:
                        if (!ConditionOfCheckFlourishPoint(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKCONTRIBUTION:
                        if (!ConditionOfCheckContribution(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKRANGEMONCOUNT:
                        if (!ConditionOfCheckRangeMonCount(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKITEMADDVALUE:
                        if (!ConditionOfCheckItemAddValue(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKINMAPRANGE:
                        if (!ConditionOfCheckInMapRange(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CASTLECHANGEDAY:
                        if (!ConditionOfCheckCastleChageDay(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CASTLEWARDAY:
                        if (!ConditionOfCheckCastleWarDay(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_ONLINELONGMIN:
                        if (!ConditionOfCheckOnlineLongMin(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKGUILDCHIEFITEMCOUNT:
                        if (!ConditionOfCheckChiefItemCount(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKNAMEDATELIST:
                        if (!ConditionOfCheckNameDateList(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKMAPHUMANCOUNT:
                        if (!ConditionOfCheckMapHumanCount(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKMAPMONCOUNT:
                        if (!ConditionOfCheckMapMonCount(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKVAR:
                        if (!ConditionOfCheckVar(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKSERVERNAME:
                        if (!ConditionOfCheckServerName(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKISONMAP:
                        if (!ConditionOfCheckIsOnMap(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_KILLBYHUM:
                        if ((playObject.LastHiter != null) && (playObject.LastHiter.Race != ActorRace.Play)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_KILLBYMON:
                        if ((playObject.LastHiter != null) && (playObject.LastHiter.Race == ActorRace.Play)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKINSAFEZONE:
                        if (!playObject.InSafeZone()) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKMAP:
                        if (!ConditionOfCheckMap(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKPOS:
                        if (!ConditionOfCheckPos(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_REVIVESLAVE:
                        if (!ConditionOfReviveSlave(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKMAGICLVL:
                        if (!ConditionOfCheckMagicLvl(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKGROUPCLASS:
                        if (!ConditionOfCheckGroupClass(playObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_ISGROUPMASTER:
                        if (playObject.GroupOwner != 0) {
                            if (playObject.GroupOwner != playObject.ActorId) {
                                result = false;
                            }
                        }
                        else {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_ISHIGH:
                        result = ConditionOfIsHigh(playObject, questConditionInfo);
                        break;
                    case ScriptConst.nSCHECKDEATHPLAYMON:
                        string s01 = string.Empty;
                        if (!GetValValue(playObject, questConditionInfo.sParam1, ref s01)) {
                            s01 = GetLineVariableText(playObject, questConditionInfo.sParam1);
                        }
                        result = CheckKillMon2(playObject, s01);
                        break;
                }
                if (!result) {
                    break;
                }
            }
            return result;
        }

        private static bool CheckKillMon2(PlayObject playObject, string sMonName) {
            return true;
        }

        private bool JmpToLable(PlayObject playObject, string sLabel) {
            playObject.ScriptGotoCount++;
            if (playObject.ScriptGotoCount > M2Share.Config.ScriptGotoCountLimit) {
                return false;
            }
            GotoLable(playObject, sLabel, false);
            return true;
        }

        private void GoToQuest(PlayObject playObject, int nQuest) {
            for (int i = 0; i < m_ScriptList.Count; i++) {
                ScriptInfo script = m_ScriptList[i];
                if (script.QuestCount == nQuest) {
                    playObject.MScript = script;
                    playObject.LastNpc = this.ActorId;
                    GotoLable(playObject, ScriptConst.sMAIN, false);
                    break;
                }
            }
        }

        private static void GotoLableAddUseDateList(string sHumName, string sListFileName) {
            string s10 = string.Empty;
            string sText;
            bool bo15;
            sListFileName = M2Share.GetEnvirFilePath(sListFileName);
            using StringList loadList = new StringList();
            if (File.Exists(sListFileName)) {
                loadList.LoadFromFile(sListFileName);
            }
            bo15 = false;
            for (int i = 0; i < loadList.Count; i++) {
                sText = loadList[i].Trim();
                sText = HUtil32.GetValidStrCap(sText, ref s10, new[] { ' ', '\t' });
                if (string.Compare(sHumName, s10, StringComparison.OrdinalIgnoreCase) == 0) {
                    bo15 = true;
                    break;
                }
            }
            if (!bo15)
            {
                s10 = $"{sHumName}    {DateTime.Today}";
                loadList.Add(s10);
                try {
                    loadList.SaveToFile(sListFileName);
                }
                catch {
                    M2Share.Logger.Error("saving fail.... => " + sListFileName);
                }
            }
        }

        private static void GotoLableAddList(string sHumName, string sListFileName) {
            sListFileName = M2Share.GetEnvirFilePath(sListFileName);
            using StringList loadList = new StringList();
            if (File.Exists(sListFileName)) {
                loadList.LoadFromFile(sListFileName);
            }
            bool bo15 = false;
            for (int i = 0; i < loadList.Count; i++) {
                string s10 = loadList[i].Trim();
                if (string.Compare(sHumName, s10, StringComparison.OrdinalIgnoreCase) == 0) {
                    bo15 = true;
                    break;
                }
            }
            if (!bo15) {
                loadList.Add(sHumName);
                try {
                    loadList.SaveToFile(sListFileName);
                }
                catch {
                    M2Share.Logger.Error("saving fail.... => " + sListFileName);
                }
            }
        }

        private static void GotoLableDelUseDateList(string sHumName, string sListFileName) {
            string s10 = string.Empty;
            string sText;
            sListFileName = M2Share.GetEnvirFilePath(sListFileName);
            using StringList loadList = new StringList();
            if (File.Exists(sListFileName)) {
                loadList.LoadFromFile(sListFileName);
            }
            bool bo15 = false;
            for (int i = 0; i < loadList.Count; i++) {
                sText = loadList[i].Trim();
                sText = HUtil32.GetValidStrCap(sText, ref s10, new[] { ' ', '\t' });
                if (string.Compare(sHumName, s10, StringComparison.OrdinalIgnoreCase) == 0) {
                    bo15 = true;
                    loadList.RemoveAt(i);
                    break;
                }
            }
            if (bo15) {
                loadList.SaveToFile(sListFileName);
            }
        }

        private static void GotoLableDelList(string playName, string sListFileName) {
            bool bo15;
            sListFileName = M2Share.GetEnvirFilePath(sListFileName);
            using StringList loadList = new StringList();
            if (File.Exists(sListFileName)) {
                loadList.LoadFromFile(sListFileName);
            }
            bo15 = false;
            for (int i = 0; i < loadList.Count; i++) {
                string s10 = loadList[i].Trim();
                if (string.Compare(playName, s10, StringComparison.OrdinalIgnoreCase) == 0) {
                    loadList.RemoveAt(i);
                    bo15 = true;
                    break;
                }
            }
            if (bo15) {
                loadList.SaveToFile(sListFileName);
            }
        }

        private void GotoLableTakeItem(PlayObject playObject, string sItemName, int nItemCount, ref string sC) {
            UserItem userItem;
            StdItem stdItem;
            if (string.Compare(sItemName, Grobal2.StringGoldName, StringComparison.OrdinalIgnoreCase) == 0) {
                playObject.DecGold(nItemCount);
                playObject.GoldChanged();
                if (M2Share.GameLogGold) {
                    M2Share.EventSource.AddEventLog(10, playObject.MapName + "\t" + playObject.CurrX + "\t" + playObject.CurrY + "\t" + playObject.ChrName + "\t" + Grobal2.StringGoldName + "\t" + nItemCount + "\t" + '1' + "\t" + ChrName);
                }
                return;
            }
            for (int i = playObject.ItemList.Count - 1; i >= 0; i--) {
                if (nItemCount <= 0) {
                    break;
                }
                userItem = playObject.ItemList[i];
                if (string.Compare(M2Share.WorldEngine.GetStdItemName(userItem.Index), sItemName, StringComparison.OrdinalIgnoreCase) == 0) {
                    stdItem = M2Share.WorldEngine.GetStdItem(userItem.Index);
                    if (stdItem.NeedIdentify == 1) {
                        M2Share.EventSource.AddEventLog(10, playObject.MapName + "\t" + playObject.CurrX + "\t" + playObject.CurrY + "\t" + playObject.ChrName + "\t" + sItemName + "\t" + userItem.MakeIndex + "\t" + '1' + "\t" + ChrName);
                    }
                    playObject.SendDelItems(userItem);
                    sC = M2Share.WorldEngine.GetStdItemName(userItem.Index);
                    Dispose(userItem);
                    playObject.ItemList.RemoveAt(i);
                    nItemCount -= 1;
                }
            }
        }

        public void GotoLableGiveItem(PlayObject playObject, string sItemName, int nItemCount) {
            UserItem userItem;
            StdItem stdItem;
            if (string.Compare(sItemName, Grobal2.StringGoldName, StringComparison.OrdinalIgnoreCase) == 0) {
                playObject.IncGold(nItemCount);
                playObject.GoldChanged();
                if (M2Share.GameLogGold) {
                    M2Share.EventSource.AddEventLog(9, playObject.MapName + "\t" + playObject.CurrX + "\t" + playObject.CurrY + "\t" + playObject.ChrName + "\t" + Grobal2.StringGoldName + "\t" + nItemCount + "\t" + '1' + "\t" + ChrName);
                }
                return;
            }
            if (M2Share.WorldEngine.GetStdItemIdx(sItemName) > 0) {
                if (!(nItemCount >= 1 && nItemCount <= 50)) {
                    nItemCount = 1;
                }
                for (int i = 0; i < nItemCount; i++) {
                    if (playObject.IsEnoughBag()) {
                        userItem = new UserItem();
                        if (M2Share.WorldEngine.CopyToUserItemFromName(sItemName, ref userItem)) {
                            playObject.ItemList.Add(userItem);
                            playObject.SendAddItem(userItem);
                            stdItem = M2Share.WorldEngine.GetStdItem(userItem.Index);
                            if (stdItem.NeedIdentify == 1) {
                                M2Share.EventSource.AddEventLog(9, playObject.MapName + "\t" + playObject.CurrX + "\t" + playObject.CurrY + "\t" + playObject.ChrName + "\t" + sItemName + "\t" + userItem.MakeIndex + "\t" + '1' + "\t" + ChrName);
                            }
                        }
                        else {
                            Dispose(userItem);
                        }
                    }
                    else {
                        userItem = new UserItem();
                        if (M2Share.WorldEngine.CopyToUserItemFromName(sItemName, ref userItem)) {
                            stdItem = M2Share.WorldEngine.GetStdItem(userItem.Index);
                            if (stdItem.NeedIdentify == 1) {
                                M2Share.EventSource.AddEventLog(9, playObject.MapName + "\t" + playObject.CurrX + "\t" + playObject.CurrY + "\t" + playObject.ChrName + "\t" + sItemName + "\t" + userItem.MakeIndex + "\t" + '1' + "\t" + ChrName);
                            }
                            playObject.DropItemDown(userItem, 3, false, playObject.ActorId, 0);
                        }
                        Dispose(userItem);
                    }
                }
            }
        }

        private static void GotoLableTakeWItem(PlayObject playObject, string sItemName, int nItemCount) {
            string sC;
            if (HUtil32.CompareLStr(sItemName, "[NECKLACE]", 4)) {
                if (playObject.UseItems[ItemLocation.Necklace].Index > 0) {
                    playObject.SendDelItems(playObject.UseItems[ItemLocation.Necklace]);
                    sC = M2Share.WorldEngine.GetStdItemName(playObject.UseItems[ItemLocation.Necklace].Index);
                    playObject.UseItems[ItemLocation.Necklace].Index = 0;
                    return;
                }
            }
            if (HUtil32.CompareLStr(sItemName, "[RING]", 4)) {
                if (playObject.UseItems[ItemLocation.Ringl].Index > 0) {
                    playObject.SendDelItems(playObject.UseItems[ItemLocation.Ringl]);
                    sC = M2Share.WorldEngine.GetStdItemName(playObject.UseItems[ItemLocation.Ringl].Index);
                    playObject.UseItems[ItemLocation.Ringl].Index = 0;
                    return;
                }
                if (playObject.UseItems[ItemLocation.Ringr].Index > 0) {
                    playObject.SendDelItems(playObject.UseItems[ItemLocation.Ringr]);
                    sC = M2Share.WorldEngine.GetStdItemName(playObject.UseItems[ItemLocation.Ringr].Index);
                    playObject.UseItems[ItemLocation.Ringr].Index = 0;
                    return;
                }
            }
            if (HUtil32.CompareLStr(sItemName, "[ARMRING]", 4)) {
                if (playObject.UseItems[ItemLocation.ArmRingl].Index > 0) {
                    playObject.SendDelItems(playObject.UseItems[ItemLocation.ArmRingl]);
                    sC = M2Share.WorldEngine.GetStdItemName(playObject.UseItems[ItemLocation.ArmRingl].Index);
                    playObject.UseItems[ItemLocation.ArmRingl].Index = 0;
                    return;
                }
                if (playObject.UseItems[ItemLocation.ArmRingr].Index > 0) {
                    playObject.SendDelItems(playObject.UseItems[ItemLocation.ArmRingr]);
                    sC = M2Share.WorldEngine.GetStdItemName(playObject.UseItems[ItemLocation.ArmRingr].Index);
                    playObject.UseItems[ItemLocation.ArmRingr].Index = 0;
                    return;
                }
            }
            if (HUtil32.CompareLStr(sItemName, "[WEAPON]", 4)) {
                if (playObject.UseItems[ItemLocation.Weapon].Index > 0) {
                    playObject.SendDelItems(playObject.UseItems[ItemLocation.Weapon]);
                    sC = M2Share.WorldEngine.GetStdItemName(playObject.UseItems[ItemLocation.Weapon].Index);
                    playObject.UseItems[ItemLocation.Weapon].Index = 0;
                    return;
                }
            }
            if (HUtil32.CompareLStr(sItemName, "[HELMET]", 4)) {
                if (playObject.UseItems[ItemLocation.Helmet].Index > 0) {
                    playObject.SendDelItems(playObject.UseItems[ItemLocation.Helmet]);
                    sC = M2Share.WorldEngine.GetStdItemName(playObject.UseItems[ItemLocation.Helmet].Index);
                    playObject.UseItems[ItemLocation.Helmet].Index = 0;
                    return;
                }
            }
            if (HUtil32.CompareLStr(sItemName, "[DRESS]", 4)) {
                if (playObject.UseItems[ItemLocation.Dress].Index > 0) {
                    playObject.SendDelItems(playObject.UseItems[ItemLocation.Dress]);
                    sC = M2Share.WorldEngine.GetStdItemName(playObject.UseItems[ItemLocation.Dress].Index);
                    playObject.UseItems[ItemLocation.Dress].Index = 0;
                    return;
                }
            }
            if (HUtil32.CompareLStr(sItemName, "[U_BUJUK]", 4)) {
                if (playObject.UseItems[ItemLocation.Bujuk].Index > 0) {
                    playObject.SendDelItems(playObject.UseItems[ItemLocation.Bujuk]);
                    sC = M2Share.WorldEngine.GetStdItemName(playObject.UseItems[ItemLocation.Bujuk].Index);
                    playObject.UseItems[ItemLocation.Bujuk].Index = 0;
                    return;
                }
            }
            if (HUtil32.CompareLStr(sItemName, "[U_BELT]", 4)) {
                if (playObject.UseItems[ItemLocation.Belt].Index > 0) {
                    playObject.SendDelItems(playObject.UseItems[ItemLocation.Belt]);
                    sC = M2Share.WorldEngine.GetStdItemName(playObject.UseItems[ItemLocation.Belt].Index);
                    playObject.UseItems[ItemLocation.Belt].Index = 0;
                    return;
                }
            }
            if (HUtil32.CompareLStr(sItemName, "[U_BOOTS]", 4)) {
                if (playObject.UseItems[ItemLocation.Boots].Index > 0) {
                    playObject.SendDelItems(playObject.UseItems[ItemLocation.Boots]);
                    sC = M2Share.WorldEngine.GetStdItemName(playObject.UseItems[ItemLocation.Boots].Index);
                    playObject.UseItems[ItemLocation.Boots].Index = 0;
                    return;
                }
            }
            if (HUtil32.CompareLStr(sItemName, "[U_CHARM]", 4)) {
                if (playObject.UseItems[ItemLocation.Charm].Index > 0) {
                    playObject.SendDelItems(playObject.UseItems[ItemLocation.Charm]);
                    sC = M2Share.WorldEngine.GetStdItemName(playObject.UseItems[ItemLocation.Charm].Index);
                    playObject.UseItems[ItemLocation.Charm].Index = 0;
                    return;
                }
            }
            for (int i = 0; i < playObject.UseItems.Length; i++) {
                if (nItemCount <= 0) {
                    return;
                }
                if (playObject.UseItems[i].Index > 0) {
                    string sName = M2Share.WorldEngine.GetStdItemName(playObject.UseItems[i].Index);
                    if (string.Compare(sName, sItemName, StringComparison.OrdinalIgnoreCase) == 0) {
                        playObject.SendDelItems(playObject.UseItems[i]);
                        playObject.UseItems[i].Index = 0;
                        nItemCount -= 1;
                    }
                }
            }
        }

        private bool GotoLableQuestActionProcess(PlayObject playObject, IList<QuestActionInfo> actionList, ref string sC, ref UserItem userItem, ref bool bo11) {
            bool result = true;
            string s44 = string.Empty;
            int n18 = 0;
            int n34 = 0;
            int n38 = 0;
            int n3C = 0;
            int n40 = 0;
            for (int i = 0; i < actionList.Count; i++) {
                QuestActionInfo questActionInfo = actionList[i];
                switch (questActionInfo.nCmdCode) {
                    case ScriptConst.nSET:
                        var n28 = HUtil32.StrToInt(questActionInfo.sParam1, 0);
                        var n2C = HUtil32.StrToInt(questActionInfo.sParam2, 0);
                        playObject.SetQuestFlagStatus(n28, n2C);
                        break;
                    case ScriptConst.nTAKE:
                        GotoLableTakeItem(playObject, questActionInfo.sParam1, questActionInfo.nParam2, ref sC);
                        break;
                    case ScriptConst.nSC_GIVE:
                        ActionOfGiveItem(playObject, questActionInfo);
                        break;
                    case ScriptConst.nTAKEW:
                        GotoLableTakeWItem(playObject, questActionInfo.sParam1, questActionInfo.nParam2);
                        break;
                    case ScriptConst.nCLOSE:
                        playObject.SendMsg(this, Messages.RM_MERCHANTDLGCLOSE, 0, ActorId, 0, 0, "");
                        break;
                    case ScriptConst.nRESET:
                        for (int k = 0; k < questActionInfo.nParam2; k++) {
                            playObject.SetQuestFlagStatus(questActionInfo.nParam1 + k, 0);
                        }
                        break;
                    case ScriptConst.nSETOPEN:
                        n28 = HUtil32.StrToInt(questActionInfo.sParam1, 0);
                        n2C = HUtil32.StrToInt(questActionInfo.sParam2, 0);
                        playObject.SetQuestUnitOpenStatus(n28, n2C);
                        break;
                    case ScriptConst.nSETUNIT:
                        n28 = HUtil32.StrToInt(questActionInfo.sParam1, 0);
                        n2C = HUtil32.StrToInt(questActionInfo.sParam2, 0);
                        playObject.SetQuestUnitStatus(n28, n2C);
                        break;
                    case ScriptConst.nRESETUNIT:
                        for (int k = 0; k < questActionInfo.nParam2; k++) {
                            playObject.SetQuestUnitStatus(questActionInfo.nParam1 + k, 0);
                        }
                        break;
                    case ScriptConst.nBREAK:
                        result = false;
                        break;
                    case ScriptConst.nTIMERECALL:
                        playObject.IsTimeRecall = true;
                        playObject.TimeRecallMoveMap = playObject.MapName;
                        playObject.TimeRecallMoveX = playObject.CurrX;
                        playObject.TimeRecallMoveY = playObject.CurrY;
                        playObject.TimeRecallTick = HUtil32.GetTickCount() + (questActionInfo.nParam1 * 60 * 1000);
                        break;
                    case ScriptConst.nSC_PARAM1:
                        n34 = questActionInfo.nParam1;
                        s44 = questActionInfo.sParam1;
                        break;
                    case ScriptConst.nSC_PARAM2:
                        n38 = questActionInfo.nParam1;
                        string s48 = questActionInfo.sParam1;
                        break;
                    case ScriptConst.nSC_PARAM3:
                        n3C = questActionInfo.nParam1;
                        string s4C = questActionInfo.sParam1;
                        break;
                    case ScriptConst.nSC_PARAM4:
                        n40 = questActionInfo.nParam1;
                        break;
                    case ScriptConst.nSC_EXEACTION:
                        n40 = questActionInfo.nParam1;
                        ExeAction(playObject, questActionInfo.sParam1, questActionInfo.sParam2, questActionInfo.sParam3, questActionInfo.nParam1, questActionInfo.nParam2, questActionInfo.nParam3);
                        break;
                    case ScriptConst.nMAPMOVE:
                        playObject.SendRefMsg(Messages.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
                        playObject.SpaceMove(questActionInfo.sParam1, (short)questActionInfo.nParam2, (short)questActionInfo.nParam3, 0);
                        bo11 = true;
                        break;
                    case ScriptConst.nMAP:
                        playObject.SendRefMsg(Messages.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
                        playObject.MapRandomMove(questActionInfo.sParam1, 0);
                        bo11 = true;
                        break;
                    case ScriptConst.nTAKECHECKITEM:
                        if (userItem != null) {
                            playObject.QuestTakeCheckItem(userItem);
                        }
                        else {
                            ScriptActionError(playObject, "", questActionInfo, ScriptConst.sTAKECHECKITEM);
                        }
                        break;
                    case ScriptConst.nMONGEN:
                        for (int k = 0; k < questActionInfo.nParam2; k++)
                        {
                            var n20X = M2Share.RandomNumber.Random(questActionInfo.nParam3 * 2 + 1) + (n38 - questActionInfo.nParam3);
                            var n24Y = M2Share.RandomNumber.Random(questActionInfo.nParam3 * 2 + 1) + (n3C - questActionInfo.nParam3);
                            M2Share.WorldEngine.RegenMonsterByName(s44, (short)n20X, (short)n24Y, questActionInfo.sParam1);
                        }
                        break;
                    case ScriptConst.nMONCLEAR:
                        var list58 = new List<BaseObject>();
                        M2Share.WorldEngine.GetMapMonster(M2Share.MapMgr.FindMap(questActionInfo.sParam1), list58);
                        for (int k = 0; k < list58.Count; k++) {
                            list58[k].NoItem = true;
                            list58[k].WAbil.HP = 0;
                            list58[k].MakeGhost();
                        }
                        list58.Clear();
                        break;
                    case ScriptConst.nMOV:
                        MovData(playObject, questActionInfo);
                        break;
                    case ScriptConst.nINC:
                        IncInteger(playObject, questActionInfo);
                        break;
                    case ScriptConst.nDEC:
                        DecInteger(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSUM:
                        SumData(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_DIV:
                        DivData(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_MUL:
                        MulData(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_PERCENT:
                        PercentData(playObject, questActionInfo);
                        break;
                    case ScriptConst.nBREAKTIMERECALL:
                        playObject.IsTimeRecall = false;
                        break;
                    case ScriptConst.nCHANGEMODE:
                        switch (questActionInfo.nParam1)
                        {
                            case 1:
                                CommandMgr.Execute(playObject, "ChangeAdminMode");
                                break;
                            case 2:
                                CommandMgr.Execute(playObject, "ChangeSuperManMode");
                                break;
                            case 3:
                                CommandMgr.Execute(playObject, "ChangeObMode");
                                break;
                            default:
                                ScriptActionError(playObject, "", questActionInfo, ScriptConst.sCHANGEMODE);
                                break;
                        }
                        break;
                    case ScriptConst.nPKPOINT:
                        if (questActionInfo.nParam1 == 0) {
                            playObject.PkPoint = 0;
                        }
                        else {
                            if (questActionInfo.nParam1 < 0) {
                                if ((playObject.PkPoint + questActionInfo.nParam1) >= 0) {
                                    playObject.PkPoint += questActionInfo.nParam1;
                                }
                                else {
                                    playObject.PkPoint = 0;
                                }
                            }
                            else {
                                if ((playObject.PkPoint + questActionInfo.nParam1) > 10000) {
                                    playObject.PkPoint = 10000;
                                }
                                else {
                                    playObject.PkPoint += questActionInfo.nParam1;
                                }
                            }
                        }
                        playObject.RefNameColor();
                        break;
                    case ScriptConst.nCHANGEXP:
                        break;
                    case ScriptConst.nSC_RECALLMOB:
                        ActionOfRecallmob(playObject, questActionInfo);
                        break;
                    case ScriptConst.nKICK:
                        playObject.BoReconnection = true;
                        playObject.BoSoftClose = true;
                        break;
                    case ScriptConst.nTHROWITEM://将指定物品刷新到指定地图坐标范围内
                        ActionOfThrowitem(playObject, questActionInfo);
                        break;
                    case ScriptConst.nMOVR:
                        MovrData(playObject, questActionInfo);
                        break;
                    case ScriptConst.nEXCHANGEMAP:
                        var envir = M2Share.MapMgr.FindMap(questActionInfo.sParam1);
                        if (envir != null)
                        {
                            IList<BaseObject> exchangeList = new List<BaseObject>();
                            M2Share.WorldEngine.GetMapRageHuman(envir, 0, 0, 1000, ref exchangeList);
                            if (exchangeList.Count > 0)
                            {
                                var user = (PlayObject)exchangeList[0];
                                user.MapRandomMove(MapName, 0);
                            }
                            playObject.MapRandomMove(questActionInfo.sParam1, 0);
                        }
                        else {
                            ScriptActionError(playObject, "", questActionInfo, ScriptConst.sEXCHANGEMAP);
                        }
                        break;
                    case ScriptConst.nRECALLMAP:
                        var recallEnvir = M2Share.MapMgr.FindMap(questActionInfo.sParam1);
                        if (recallEnvir != null)
                        {
                            IList<BaseObject> recallList = new List<BaseObject>();
                            M2Share.WorldEngine.GetMapRageHuman(recallEnvir, 0, 0, 1000, ref recallList);
                            for (int k = 0; k < recallList.Count; k++)
                            {
                                var user = (PlayObject)recallList[k];
                                user.MapRandomMove(MapName, 0);
                                if (k > 20)
                                {
                                    break;
                                }
                            }
                        }
                        else
                        {
                            ScriptActionError(playObject, "", questActionInfo, ScriptConst.sRECALLMAP);
                        }
                        break;
                    case ScriptConst.nADDBATCH:
                        if (BatchParamsList == null) {
                            BatchParamsList = new List<ScriptParams>();
                        }
                        BatchParamsList.Add(new ScriptParams() {
                            sParams = questActionInfo.sParam1,
                            nParams = n18
                        });
                        break;
                    case ScriptConst.nBATCHDELAY:
                        n18 = questActionInfo.nParam1 * 1000;
                        break;
                    case ScriptConst.nBATCHMOVE:
                        int n20 = 0;
                        for (int k = 0; k < BatchParamsList.Count; k++) {
                            ScriptParams batchParam = BatchParamsList[k];
                            playObject.SendDelayMsg(ActorId, Messages.RM_RANDOMSPACEMOVE, 0, 0, 0, 0, BatchParamsList[k].sParams, batchParam.nParams + n20);
                            n20 += batchParam.nParams;
                        }
                        break;
                    case ScriptConst.nPLAYDICE:
                        playObject.PlayDiceLabel = questActionInfo.sParam2;
                        playObject.SendMsg(this, Messages.RM_PLAYDICE, (short)questActionInfo.nParam1, HUtil32.MakeLong(HUtil32.MakeWord((ushort)playObject.MDyVal[0], (ushort)playObject.MDyVal[1]), HUtil32.MakeWord((ushort)playObject.MDyVal[2], (ushort)playObject.MDyVal[3])), HUtil32.MakeLong(HUtil32.MakeWord((ushort)playObject.MDyVal[4], (ushort)playObject.MDyVal[5]), HUtil32.MakeWord((ushort)playObject.MDyVal[6], (ushort)playObject.MDyVal[7])), HUtil32.MakeLong(HUtil32.MakeWord((ushort)playObject.MDyVal[8], (ushort)playObject.MDyVal[9]), 0), questActionInfo.sParam2);
                        bo11 = true;
                        break;
                    case ScriptConst.nADDNAMELIST:
                        GotoLableAddList(playObject.ChrName, m_sPath + questActionInfo.sParam1);
                        break;
                    case ScriptConst.nDELNAMELIST:
                        GotoLableDelList(playObject.ChrName, m_sPath + questActionInfo.sParam1);
                        break;
                    case ScriptConst.nADDUSERDATE:
                        GotoLableAddUseDateList(playObject.ChrName, m_sPath + questActionInfo.sParam1);
                        break;
                    case ScriptConst.nDELUSERDATE:
                        GotoLableDelUseDateList(playObject.ChrName, m_sPath + questActionInfo.sParam1);
                        break;
                    case ScriptConst.nADDGUILDLIST:
                        if (playObject.MyGuild != null) {
                            GotoLableAddList(playObject.MyGuild.GuildName, m_sPath + questActionInfo.sParam1);
                        }
                        break;
                    case ScriptConst.nDELGUILDLIST:
                        if (playObject.MyGuild != null) {
                            GotoLableDelList(playObject.MyGuild.GuildName, m_sPath + questActionInfo.sParam1);
                        }
                        break;
                    case ScriptConst.nSC_LINEMSG:
                    case ScriptConst.nSENDMSG:
                        ActionOfLineMsg(playObject, questActionInfo);
                        break;
                    case ScriptConst.nADDACCOUNTLIST:
                        GotoLableAddList(playObject.UserAccount, m_sPath + questActionInfo.sParam1);
                        break;
                    case ScriptConst.nDELACCOUNTLIST:
                        GotoLableDelList(playObject.UserAccount, m_sPath + questActionInfo.sParam1);
                        break;
                    case ScriptConst.nADDIPLIST:
                        GotoLableAddList(playObject.LoginIpAddr, m_sPath + questActionInfo.sParam1);
                        break;
                    case ScriptConst.nDELIPLIST:
                        GotoLableDelList(playObject.LoginIpAddr, m_sPath + questActionInfo.sParam1);
                        break;
                    case ScriptConst.nGOQUEST:
                        GoToQuest(playObject, questActionInfo.nParam1);
                        break;
                    case ScriptConst.nENDQUEST:
                        playObject.MScript = null;
                        break;
                    case ScriptConst.nGOTO:
                        if (!JmpToLable(playObject, questActionInfo.sParam1)) {
                            // ScriptActionError(PlayObject,'',QuestActionInfo,sGOTO);
                            M2Share.Logger.Error("[脚本死循环] NPC:" + ChrName + " 位置:" + MapName + '(' + CurrX + ':' + CurrY + ')' + " 命令:" + ScriptConst.sGOTO + ' ' + questActionInfo.sParam1);
                            result = false;
                            return result;
                        }
                        break;
                    case ScriptConst.nSC_HAIRCOLOR:
                        break;
                    case ScriptConst.nSC_WEARCOLOR:
                        break;
                    case ScriptConst.nSC_HAIRSTYLE:
                        ActionOfChangeHairStyle(playObject, questActionInfo);
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
                        ActionOfRecallGroupMembers(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_CLEARNAMELIST:
                        ActionOfClearList(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_MAPTING:
                        ActionOfMapTing(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_CHANGELEVEL:
                        ActionOfChangeLevel(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_MARRY:
                        ActionOfMarry(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_MASTER:
                        ActionOfMaster(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_UNMASTER:
                        ActionOfUnMaster(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_UNMARRY:
                        ActionOfUnMarry(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_GETMARRY:
                        ActionOfGetMarry(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_GETMASTER:
                        ActionOfGetMaster(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_CLEARSKILL:
                        ActionOfClearSkill(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_DELNOJOBSKILL:
                        ActionOfDelNoJobSkill(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_DELSKILL:
                        ActionOfDelSkill(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_ADDSKILL:
                        ActionOfAddSkill(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_SKILLLEVEL:
                        ActionOfSkillLevel(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_CHANGEPKPOINT:
                        ActionOfChangePkPoint(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_CHANGEEXP:
                        ActionOfChangeExp(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_CHANGEJOB:
                        ActionOfChangeJob(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_MISSION:
                        ActionOfMission(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_MOBPLACE:
                        ActionOfMobPlace(playObject, questActionInfo, n34, n38, n3C, n40);
                        break;
                    case ScriptConst.nSC_SETMEMBERTYPE:
                        ActionOfSetMemberType(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_SETMEMBERLEVEL:
                        ActionOfSetMemberLevel(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_GAMEGOLD:
                        // nSC_SETMEMBERTYPE:   PlayObject.m_nMemberType:=Str_ToInt(QuestActionInfo.sParam1,0);
                        // nSC_SETMEMBERLEVEL:  PlayObject.m_nMemberType:=Str_ToInt(QuestActionInfo.sParam1,0);
                        ActionOfGameGold(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_GAMEPOINT:
                        ActionOfGamePoint(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_OffLine:
                        ActionOfOffLine(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_AUTOADDGAMEGOLD: // 增加挂机
                        ActionOfAutoAddGameGold(playObject, questActionInfo, n34, n38);
                        break;
                    case ScriptConst.nSC_AUTOSUBGAMEGOLD:
                        ActionOfAutoSubGameGold(playObject, questActionInfo, n34, n38);
                        break;
                    case ScriptConst.nSC_CHANGENAMECOLOR:
                        ActionOfChangeNameColor(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_CLEARPASSWORD:
                        ActionOfClearPassword(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_RENEWLEVEL:
                        ActionOfReNewLevel(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_KILLSLAVE:
                        ActionOfKillSlave(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_CHANGEGENDER:
                        ActionOfChangeGender(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_KILLMONEXPRATE:
                        ActionOfKillMonExpRate(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_POWERRATE:
                        ActionOfPowerRate(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_CHANGEMODE:
                        ActionOfChangeMode(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_CHANGEPERMISSION:
                        ActionOfChangePerMission(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_KILL:
                        ActionOfKill(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_KICK:
                        ActionOfKick(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_BONUSPOINT:
                        ActionOfBonusPoint(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_RESTRENEWLEVEL:
                        ActionOfRestReNewLevel(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_DELMARRY:
                        ActionOfDelMarry(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_DELMASTER:
                        ActionOfDelMaster(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_CREDITPOINT:
                        ActionOfChangeCreditPoint(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_CLEARNEEDITEMS:
                        ActionOfClearNeedItems(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_CLEARMAEKITEMS:
                        ActionOfClearMakeItems(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_SETSENDMSGFLAG:
                        playObject.BoSendMsgFlag = true;
                        break;
                    case ScriptConst.nSC_UPGRADEITEMS:
                        ActionOfUpgradeItems(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_UPGRADEITEMSEX:
                        ActionOfUpgradeItemsEx(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_MONGENEX:
                        ActionOfMonGenEx(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_CLEARMAPMON:
                        ActionOfClearMapMon(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_SETMAPMODE:
                        ActionOfSetMapMode(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_PKZONE:
                        ActionOfPkZone(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_RESTBONUSPOINT:
                        ActionOfRestBonusPoint(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_TAKECASTLEGOLD:
                        ActionOfTakeCastleGold(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_HUMANHP:
                        ActionOfHumanHp(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_HUMANMP:
                        ActionOfHumanMp(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_BUILDPOINT:
                        ActionOfGuildBuildPoint(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_DELAYGOTO:
                        ActionOfDelayCall(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_AURAEPOINT:
                        ActionOfGuildAuraePoint(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_STABILITYPOINT:
                        ActionOfGuildstabilityPoint(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_FLOURISHPOINT:
                        ActionOfGuildFlourishPoint(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_OPENMAGICBOX:
                        ActionOfOpenMagicBox(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_SETRANKLEVELNAME:
                        ActionOfSetRankLevelName(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_GMEXECUTE:
                        ActionOfGmExecute(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_GUILDCHIEFITEMCOUNT:
                        ActionOfGuildChiefItemCount(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_ADDNAMEDATELIST:
                        ActionOfAddNameDateList(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_DELNAMEDATELIST:
                        ActionOfDelNameDateList(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_MOBFIREBURN:
                        ActionOfMobFireBurn(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_MESSAGEBOX:
                        ActionOfMessageBox(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_SETSCRIPTFLAG:
                        ActionOfSetScriptFlag(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_SETAUTOGETEXP:
                        ActionOfAutoGetExp(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_VAR:
                        ActionOfVar(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_LOADVAR:
                        ActionOfLoadVar(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_SAVEVAR:
                        ActionOfSaveVar(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_CALCVAR:
                        ActionOfCalcVar(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_GUILDRECALL:
                        ActionOfGuildRecall(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_GROUPADDLIST:
                        ActionOfGroupAddList(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_CLEARLIST:
                        ActionOfClearList(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_GROUPRECALL:
                        ActionOfGroupRecall(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_GROUPMOVEMAP:
                        ActionOfGroupMoveMap(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_REPAIRALL:
                        ActionOfRepairAllItem(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_QUERYBAGITEMS:// 刷新包裹
                        if ((HUtil32.GetTickCount() - playObject.QueryBagItemsTick) > M2Share.Config.QueryBagItemsTick) {
                            playObject.QueryBagItemsTick = HUtil32.GetTickCount();
                            playObject.ClientQueryBagItems();
                        }
                        else {
                            playObject.SysMsg(Settings.QUERYBAGITEMS, MsgColor.Red, MsgType.Hint);
                        }
                        break;
                    case ScriptConst.nSC_SETRANDOMNO:
                        while (true) {
                            n2C = M2Share.RandomNumber.Random(999999);
                            if ((n2C >= 1000) && (n2C.ToString() != playObject.RandomNo)) {
                                playObject.RandomNo = n2C.ToString();
                                break;
                            }
                        }
                        break;
                    case ScriptConst.nOPENYBDEAL:
                        ActionOfOpenSaleDeal(playObject, questActionInfo);
                        break;
                    case ScriptConst.nQUERYYBSELL:
                        ActionOfQuerySaleSell(playObject, questActionInfo);
                        break;
                    case ScriptConst.nQUERYYBDEAL:
                        ActionOfQueryTrustDeal(playObject, questActionInfo);
                        break;
                    case ScriptConst.nDELAYGOTO:
                        playObject.IsTimeGoto = true;
                        int mDelayGoto = HUtil32.StrToInt(GetLineVariableText(playObject, questActionInfo.sParam1), 0);//变量操作
                        if (mDelayGoto == 0) {
                            int delayCount = 0;
                            GetValValue(playObject, questActionInfo.sParam1, ref delayCount);
                            mDelayGoto = delayCount;
                        }
                        if (mDelayGoto > 0) {
                            playObject.TimeGotoTick = HUtil32.GetTickCount() + mDelayGoto;
                        }
                        else {
                            playObject.TimeGotoTick = HUtil32.GetTickCount() + questActionInfo.nParam1;//毫秒
                        }
                        playObject.TimeGotoLable = questActionInfo.sParam2;
                        playObject.TimeGotoNpc = this;
                        break;
                    case ScriptConst.nCLEARDELAYGOTO:
                        playObject.IsTimeGoto = false;
                        playObject.TimeGotoLable = "";
                        playObject.TimeGotoNpc = null;
                        break;
                    case ScriptConst.nSC_QUERYVALUE:
                        ActionOfQueryValue(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_KILLSLAVENAME:
                        ActionOfKillSlaveName(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_QUERYITEMDLG:
                        ActionOfQueryItemDlg(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_UPGRADEDLGITEM:
                        ActionOfUpgradeDlgItem(playObject, questActionInfo);
                        break;
                    case ScriptConst.nSC_GETDLGITEMVALUE:

                        break;
                    case ScriptConst.nSC_TAKEDLGITEM:

                        break;
                }
            }
            return result;
        }

        private static void ActionOfUpgradeDlgItem(PlayObject playObject, QuestActionInfo questActionInfo) {

        }

        private void ActionOfQueryItemDlg(PlayObject playObject, QuestActionInfo questActionInfo) {
            playObject.TakeDlgItem = questActionInfo.nParam3 != 0;
            playObject.GotoNpcLabel = questActionInfo.sParam2;
            string sHint = questActionInfo.sParam1;
            if (string.IsNullOrEmpty(sHint)) sHint = "请输入:";
            playObject.SendDefMessage(Messages.SM_QUERYITEMDLG, ActorId, 0, 0, 0, sHint);
        }

        private void ActionOfKillSlaveName(PlayObject playObject, QuestActionInfo questActionInfo) {
            string sSlaveName = questActionInfo.sParam1;
            if (string.IsNullOrEmpty(sSlaveName)) {
                ScriptActionError(playObject, "", questActionInfo, ScriptConst.sSC_KILLSLAVENAME);
                return;
            }
            if (sSlaveName.Equals("*") || string.Compare(sSlaveName, "ALL", StringComparison.OrdinalIgnoreCase) == 0) {
                for (int i = 0; i < playObject.SlaveList.Count; i++) {
                    playObject.SlaveList[i].WAbil.HP = 0;
                }
                return;
            }
            for (int i = 0; i < playObject.SlaveList.Count; i++) {
                BaseObject baseObject = playObject.SlaveList[i];
                if (!Death && (string.Compare(sSlaveName, baseObject.ChrName, StringComparison.OrdinalIgnoreCase) == 0)) {
                    baseObject.WAbil.HP = 0;
                }
            }
        }

        private static void ActionOfQueryValue(PlayObject playObject, QuestActionInfo questActionInfo) {
            int btStrLabel = questActionInfo.nParam1;
            if (btStrLabel < 100) {
                btStrLabel = 0;
            }
            playObject.ValLabel = (byte)btStrLabel;
            byte btType = (byte)questActionInfo.nParam2;
            if (btType > 3) {
                btType = 0;
            }
            playObject.ValType = btType;
            int btLen = HUtil32._MAX(1, questActionInfo.nParam3);
            playObject.GotoNpcLabel = questActionInfo.sParam4;
            string sHint = questActionInfo.sParam5;
            playObject.ValNpcType = 0;
            if (string.Compare(questActionInfo.sParam6, "QF", StringComparison.OrdinalIgnoreCase) == 0) {
                playObject.ValNpcType = 1;
            }
            else if (string.Compare(questActionInfo.sParam6, "QM", StringComparison.OrdinalIgnoreCase) == 0) {
                playObject.ValNpcType = 2;
            }
            if (string.IsNullOrEmpty(sHint)) {
                sHint = "请输入：";
            }
            playObject.SendDefMessage(Messages.SM_QUERYVALUE, 0, HUtil32.MakeWord(btType, (ushort)btLen), 0, 0, sHint);
        }

        private void GotoLableSendMerChantSayMsg(PlayObject playObject, string sMsg, bool boFlag) {
            sMsg = GetLineVariableText(playObject, sMsg);
            playObject.GetScriptLabel(sMsg);
            if (boFlag) {
                playObject.SendPriorityMsg(this, Messages.RM_MERCHANTSAY, 0, 0, 0, 0, ChrName + '/' + sMsg, MessagePriority.High);
            }
            else {
                playObject.SendMsg(this, Messages.RM_MERCHANTSAY, 0, 0, 0, 0, ChrName + '/' + sMsg);
            }
        }
    }
}