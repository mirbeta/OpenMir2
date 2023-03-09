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
        private void GotoLable(PlayObject PlayObject, string sLabel, bool boExtJmp, string sMsg) {
            if (PlayObject.LastNpc != this.ActorId) {
                PlayObject.LastNpc = 0;
                PlayObject.MScript = null;
            }
            ScriptInfo Script = null;
            SayingRecord SayingRecord;
            UserItem UserItem = null;
            string sC = string.Empty;
            if (string.Compare("@main", sLabel, StringComparison.OrdinalIgnoreCase) == 0) {
                for (int i = 0; i < m_ScriptList.Count; i++) {
                    ScriptInfo Script3C = m_ScriptList[i];
                    if (Script3C.RecordList.TryGetValue(sLabel, out _)) {
                        Script = Script3C;
                        PlayObject.MScript = Script;
                        PlayObject.LastNpc = this.ActorId;
                        break;
                    }
                }
            }
            if (Script == null) {
                if (PlayObject.MScript != null) {
                    for (int i = m_ScriptList.Count - 1; i >= 0; i--) {
                        if (m_ScriptList[i] == PlayObject.MScript) {
                            Script = m_ScriptList[i];
                        }
                    }
                }
                if (Script == null) {
                    for (int i = m_ScriptList.Count - 1; i >= 0; i--) {
                        if (CheckGotoLableQuestStatus(PlayObject, m_ScriptList[i])) {
                            Script = m_ScriptList[i];
                            PlayObject.MScript = Script;
                            PlayObject.LastNpc = this.ActorId;
                        }
                    }
                }
            }
            // 跳转到指定示签，执行
            if (Script != null) {
                if (Script.RecordList.TryGetValue(sLabel, out SayingRecord)) {
                    if (boExtJmp && SayingRecord.boExtJmp == false) {
                        return;
                    }
                    string sSendMsg = string.Empty;
                    for (int i = 0; i < SayingRecord.ProcedureList.Count; i++) {
                        SayingProcedure sayingProcedure = SayingRecord.ProcedureList[i];
                        bool bo11 = false;
                        if (GotoLableQuestCheckCondition(PlayObject, sayingProcedure.ConditionList, ref sC, ref UserItem)) {
                            sSendMsg = sSendMsg + sayingProcedure.sSayMsg;
                            if (!GotoLableQuestActionProcess(PlayObject, sayingProcedure.ActionList, ref sC, ref UserItem, ref bo11)) {
                                break;
                            }
                            if (bo11) {
                                GotoLableSendMerChantSayMsg(PlayObject, sSendMsg, true);
                            }
                        }
                        else {
                            sSendMsg = sSendMsg + sayingProcedure.sElseSayMsg;
                            if (!GotoLableQuestActionProcess(PlayObject, sayingProcedure.ElseActionList, ref sC, ref UserItem, ref bo11)) {
                                break;
                            }
                            if (bo11) {
                                GotoLableSendMerChantSayMsg(PlayObject, sSendMsg, true);
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(sSendMsg)) {
                        GotoLableSendMerChantSayMsg(PlayObject, sSendMsg, false);
                    }
                }
            }
        }

        public void GotoLable(PlayObject PlayObject, string sLabel, bool boExtJmp) {
            GotoLable(PlayObject, sLabel, boExtJmp, string.Empty);
        }

        private static bool CheckGotoLableQuestStatus(PlayObject PlayObject, ScriptInfo ScriptInfo) {
            bool result = true;
            if (!ScriptInfo.IsQuest) {
                return result;
            }
            int nIndex = 0;
            while (true) {
                if ((ScriptInfo.QuestInfo[nIndex].nRandRage > 0) && (M2Share.RandomNumber.Random(ScriptInfo.QuestInfo[nIndex].nRandRage) != 0)) {
                    result = false;
                    break;
                }
                if (PlayObject.GetQuestFalgStatus(ScriptInfo.QuestInfo[nIndex].wFlag) != ScriptInfo.QuestInfo[nIndex].btValue) {
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

        private static UserItem CheckGotoLableItemW(PlayObject PlayObject, string sItemType, int nParam) {
            UserItem result = null;
            int nCount = 0;
            if (HUtil32.CompareLStr(sItemType, "[NECKLACE]", 4)) {
                if (PlayObject.UseItems[ItemLocation.Necklace].Index > 0) {
                    result = PlayObject.UseItems[ItemLocation.Necklace];
                }
                return result;
            }
            if (HUtil32.CompareLStr(sItemType, "[RING]", 4)) {
                if (PlayObject.UseItems[ItemLocation.Ringl].Index > 0) {
                    result = PlayObject.UseItems[ItemLocation.Ringl];
                }
                if (PlayObject.UseItems[ItemLocation.Ringr].Index > 0) {
                    result = PlayObject.UseItems[ItemLocation.Ringr];
                }
                return result;
            }
            if (HUtil32.CompareLStr(sItemType, "[ARMRING]", 4)) {
                if (PlayObject.UseItems[ItemLocation.ArmRingl].Index > 0) {
                    result = PlayObject.UseItems[ItemLocation.ArmRingl];
                }
                if (PlayObject.UseItems[ItemLocation.ArmRingr].Index > 0) {
                    result = PlayObject.UseItems[ItemLocation.ArmRingr];
                }
                return result;
            }
            if (HUtil32.CompareLStr(sItemType, "[WEAPON]", 4)) {
                if (PlayObject.UseItems[ItemLocation.Weapon].Index > 0) {
                    result = PlayObject.UseItems[ItemLocation.Weapon];
                }
                return result;
            }
            if (HUtil32.CompareLStr(sItemType, "[HELMET]", 4)) {
                if (PlayObject.UseItems[ItemLocation.Helmet].Index > 0) {
                    result = PlayObject.UseItems[ItemLocation.Helmet];
                }
                return result;
            }
            if (HUtil32.CompareLStr(sItemType, "[BUJUK]", 4)) {
                if (PlayObject.UseItems[ItemLocation.Bujuk].Index > 0) {
                    result = PlayObject.UseItems[ItemLocation.Bujuk];
                }
                return result;
            }
            if (HUtil32.CompareLStr(sItemType, "[BELT]", 4)) {
                if (PlayObject.UseItems[ItemLocation.Belt].Index > 0) {
                    result = PlayObject.UseItems[ItemLocation.Belt];
                }
                return result;
            }
            if (HUtil32.CompareLStr(sItemType, "[BOOTS]", 4)) {
                if (PlayObject.UseItems[ItemLocation.Boots].Index > 0) {
                    result = PlayObject.UseItems[ItemLocation.Boots];
                }
                return result;
            }
            if (HUtil32.CompareLStr(sItemType, "[CHARM]", 4)) {
                if (PlayObject.UseItems[ItemLocation.Charm].Index > 0) {
                    result = PlayObject.UseItems[ItemLocation.Charm];
                }
                return result;
            }
            result = PlayObject.CheckItemCount(sItemType, ref nCount);
            if (nCount < nParam) {
                result = null;
            }
            return result;
        }

        private static bool CheckGotoLableStringList(string sHumName, string sListFileName) {
            bool result = false;
            sListFileName = M2Share.GetEnvirFilePath(sListFileName);
            if (File.Exists(sListFileName)) {
                using StringList LoadList = new StringList();
                try {
                    LoadList.LoadFromFile(sListFileName);
                }
                catch {
                    M2Share.Logger.Error("loading fail.... => " + sListFileName);
                }
                for (int i = 0; i < LoadList.Count; i++) {
                    if (string.Compare(LoadList[i].Trim(), sHumName, StringComparison.OrdinalIgnoreCase) == 0) {
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

        private static void GotoLableQuestCheckConditionSetVal(PlayObject PlayObject, string sIndex, int nCount) {
            int n14 = M2Share.GetValNameNo(sIndex);
            if (n14 >= 0) {
                if (HUtil32.RangeInDefined(n14, 0, 99)) {
                    PlayObject.MNVal[n14] = nCount;
                }
                else if (HUtil32.RangeInDefined(n14, 100, 119)) {
                    M2Share.Config.GlobalVal[n14 - 100] = nCount;
                }
                else if (HUtil32.RangeInDefined(n14, 200, 299)) {
                    PlayObject.MDyVal[n14 - 200] = nCount;
                }
                else if (HUtil32.RangeInDefined(n14, 300, 399)) {
                    PlayObject.MNMval[n14 - 300] = nCount;
                }
                else if (HUtil32.RangeInDefined(n14, 400, 499)) {
                    M2Share.Config.GlobaDyMval[n14 - 400] = (short)nCount;
                }
                else if (HUtil32.RangeInDefined(n14, 500, 599)) {
                    PlayObject.MNSval[n14 - 600] = nCount.ToString();
                }
            }
        }

        private static bool GotoLable_QuestCheckCondition_CheckDieMon(PlayObject PlayObject, string MonName) {
            bool result = string.IsNullOrEmpty(MonName);
            if ((PlayObject.LastHiter != null) && (PlayObject.LastHiter.ChrName == MonName)) {
                result = true;
            }
            return result;
        }

        private static bool GotoLable_QuestCheckCondition_CheckKillMon(PlayObject PlayObject, string MonName) {
            bool result = string.IsNullOrEmpty(MonName);
            if ((PlayObject.TargetCret != null) && (PlayObject.TargetCret.ChrName == MonName)) {
                result = true;
            }
            return result;
        }

        public static bool GotoLable_QuestCheckCondition_CheckRandomNo(PlayObject PlayObject, string sNumber) {
            return PlayObject.RandomNo == sNumber;
        }

        private bool QuestCheckConditionCheckUserDateType(PlayObject PlayObject, string ChrName, string sListFileName, string sDay, string param1, string param2) {
            string Name = string.Empty;
            bool result = false;
            sListFileName = M2Share.GetEnvirFilePath(sListFileName);
            using StringList LoadList = new StringList();
            if (File.Exists(sListFileName)) {
                try {
                    LoadList.LoadFromFile(sListFileName);
                }
                catch {
                    M2Share.Logger.Error("loading fail.... => " + sListFileName);
                }
            }
            int nDay = HUtil32.StrToInt(sDay, 0);
            for (int i = 0; i < LoadList.Count; i++) {
                string sText = LoadList[i].Trim();
                sText = HUtil32.GetValidStrCap(sText, ref Name, new[] { ' ', '\t' });
                Name = Name.Trim();
                if (ChrName == Name) {
                    string ssDay = sText.Trim();
                    DateTime nnday = HUtil32.StrToDate(ssDay);
                    int UseDay = HUtil32.Round(DateTime.Today.ToOADate() - nnday.ToOADate());
                    int LastDay = nDay - UseDay;
                    if (LastDay < 0) {
                        result = true;
                        LastDay = 0;
                    }
                    GotoLableQuestCheckConditionSetVal(PlayObject, param1, UseDay);
                    GotoLableQuestCheckConditionSetVal(PlayObject, param2, LastDay);
                    return result;
                }
            }
            return result;
        }

        private bool GotoLableQuestCheckCondition(PlayObject PlayObject, IList<QuestConditionInfo> ConditionList, ref string sC, ref UserItem UserItem) {
            bool result = true;
            int n1C = 0;
            int nMaxDura = 0;
            int nDura = 0;
            for (int i = 0; i < ConditionList.Count; i++) {
                var questConditionInfo = ConditionList[i];
                if (!string.IsNullOrEmpty(questConditionInfo.sParam1)) {
                    if (questConditionInfo.sParam1[0] == '$') {
                        string s50 = questConditionInfo.sParam1;
                        questConditionInfo.sParam1 = '<' + questConditionInfo.sParam1 + '>';
                        GetVariableText(PlayObject, s50, ref questConditionInfo.sParam1);
                    }
                    else if (questConditionInfo.sParam1.IndexOf(">", StringComparison.OrdinalIgnoreCase) > -1) {
                        questConditionInfo.sParam1 = GetLineVariableText(PlayObject, questConditionInfo.sParam1);
                    }
                }
                if (!string.IsNullOrEmpty(questConditionInfo.sParam2)) {
                    if (questConditionInfo.sParam2[0] == '$') {
                        string s50 = questConditionInfo.sParam2;
                        questConditionInfo.sParam2 = '<' + questConditionInfo.sParam2 + '>';
                        GetVariableText(PlayObject, s50, ref questConditionInfo.sParam2);
                    }
                    else if (questConditionInfo.sParam2.IndexOf(">", StringComparison.OrdinalIgnoreCase) > -1) {
                        questConditionInfo.sParam2 = GetLineVariableText(PlayObject, questConditionInfo.sParam2);
                    }
                }
                if (!string.IsNullOrEmpty(questConditionInfo.sParam3)) {
                    if (questConditionInfo.sParam3[0] == '$') {
                        string s50 = questConditionInfo.sParam3;
                        questConditionInfo.sParam3 = '<' + questConditionInfo.sParam3 + '>';
                        GetVariableText(PlayObject, s50, ref questConditionInfo.sParam3);
                    }
                    else if (questConditionInfo.sParam3.IndexOf(">", StringComparison.OrdinalIgnoreCase) > -1) {
                        questConditionInfo.sParam3 = GetLineVariableText(PlayObject, questConditionInfo.sParam3);
                    }
                }
                if (!string.IsNullOrEmpty(questConditionInfo.sParam4)) {
                    if (questConditionInfo.sParam4[0] == '$') {
                        string s50 = questConditionInfo.sParam4;
                        questConditionInfo.sParam4 = '<' + questConditionInfo.sParam4 + '>';
                        GetVariableText(PlayObject, s50, ref questConditionInfo.sParam4);
                    }
                    else if (questConditionInfo.sParam4.IndexOf(">", StringComparison.OrdinalIgnoreCase) > -1) {
                        questConditionInfo.sParam4 = GetLineVariableText(PlayObject, questConditionInfo.sParam4);
                    }
                }
                if (!string.IsNullOrEmpty(questConditionInfo.sParam5)) {
                    if (questConditionInfo.sParam5[0] == '$') {
                        string s50 = questConditionInfo.sParam5;
                        questConditionInfo.sParam5 = '<' + questConditionInfo.sParam5 + '>';
                        GetVariableText(PlayObject, s50, ref questConditionInfo.sParam5);
                    }
                    else if (questConditionInfo.sParam5.IndexOf(">", StringComparison.OrdinalIgnoreCase) > -1) {
                        questConditionInfo.sParam5 = GetLineVariableText(PlayObject, questConditionInfo.sParam5);
                    }
                }
                if (!string.IsNullOrEmpty(questConditionInfo.sParam6)) {
                    if (questConditionInfo.sParam6[0] == '$') {
                        string s50 = questConditionInfo.sParam6;
                        questConditionInfo.sParam6 = '<' + questConditionInfo.sParam6 + '>';
                        GetVariableText(PlayObject, s50, ref questConditionInfo.sParam6);
                    }
                    else if (questConditionInfo.sParam6.IndexOf(">", StringComparison.OrdinalIgnoreCase) > -1) {
                        questConditionInfo.sParam6 = GetLineVariableText(PlayObject, questConditionInfo.sParam6);
                    }
                }

                //参数变量解释以主执行人物为依据
                if (!string.IsNullOrEmpty(questConditionInfo.sOpName)) {
                    if (questConditionInfo.sOpName.Length > 2) {
                        if (questConditionInfo.sOpName[1] == '$') {
                            string s50 = questConditionInfo.sOpName;
                            questConditionInfo.sOpName = '<' + questConditionInfo.sOpName + '>';
                            GetVariableText(PlayObject, s50, ref questConditionInfo.sOpName);
                        }
                        else if (questConditionInfo.sOpName.IndexOf(">", StringComparison.OrdinalIgnoreCase) > -1) {
                            questConditionInfo.sOpName = GetLineVariableText(PlayObject, questConditionInfo.sOpName);
                        }
                    }
                    PlayObject Human = M2Share.WorldEngine.GetPlayObject(questConditionInfo.sOpName);
                    if (Human != null) {
                        PlayObject = Human;
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
                int Hour;
                int Min;
                int Sec;
                int MSec;
                switch (questConditionInfo.CmdCode) {
                    case ScriptConst.nCHECKUSERDATE:
                        result = QuestCheckConditionCheckUserDateType(PlayObject, PlayObject.ChrName, m_sPath + questConditionInfo.sParam1, questConditionInfo.sParam3, questConditionInfo.sParam4, questConditionInfo.sParam5);
                        break;
                    case ScriptConst.nSC_CHECKRANDOMNO:
                        M2Share.Logger.Error("TODO nSC_CHECKRANDOMNO...");
                        //result = GotoLable_QuestCheckCondition_CheckRandomNo(PlayObject, sMsg);
                        break;
                    case ScriptConst.nCheckDiemon:
                        result = GotoLable_QuestCheckCondition_CheckDieMon(PlayObject, questConditionInfo.sParam1);
                        break;
                    case ScriptConst.ncheckkillplaymon:
                        result = GotoLable_QuestCheckCondition_CheckKillMon(PlayObject, questConditionInfo.sParam1);
                        break;
                    case ScriptConst.nCHECK:
                        n14 = HUtil32.StrToInt(questConditionInfo.sParam1, 0);
                        n18 = HUtil32.StrToInt(questConditionInfo.sParam2, 0);
                        n10 = PlayObject.GetQuestFalgStatus(n14);
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
                            if (PlayObject.Gender != PlayGender.Man) {
                                result = false;
                            }
                        }
                        else {
                            if (PlayObject.Gender != PlayGender.WoMan) {
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
                        n10 = PlayObject.GetQuestUnitOpenStatus(n14);
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
                        n10 = PlayObject.GetQuestUnitStatus(n14);
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
                        if (PlayObject.Abil.Level < questConditionInfo.nParam1) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nCHECKJOB:
                        if (HUtil32.CompareLStr(questConditionInfo.sParam1, ScriptConst.sWarrior)) {
                            if (PlayObject.Job != PlayJob.Warrior) {
                                result = false;
                            }
                        }
                        if (HUtil32.CompareLStr(questConditionInfo.sParam1, ScriptConst.sWizard)) {
                            if (PlayObject.Job != PlayJob.Wizard) {
                                result = false;
                            }
                        }
                        if (HUtil32.CompareLStr(questConditionInfo.sParam1, ScriptConst.sTaos)) {
                            if (PlayObject.Job != PlayJob.Taoist) {
                                result = false;
                            }
                        }
                        break;
                    case ScriptConst.nCHECKBBCOUNT:
                        if (PlayObject.SlaveList.Count < questConditionInfo.nParam1) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nCHECKITEM:
                        UserItem = PlayObject.QuestCheckItem(questConditionInfo.sParam1, ref n1C, ref nMaxDura, ref nDura);
                        if (n1C < questConditionInfo.nParam2) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nCHECKITEMW:
                        UserItem = CheckGotoLableItemW(PlayObject, questConditionInfo.sParam1, questConditionInfo.nParam2);
                        if (UserItem == null) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nCHECKGOLD:
                        if (PlayObject.Gold < questConditionInfo.nParam1) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nISTAKEITEM:
                        if (sC != questConditionInfo.sParam1) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nCHECKDURA:
                        UserItem = PlayObject.QuestCheckItem(questConditionInfo.sParam1, ref n1C, ref nMaxDura, ref nDura);
                        if (HUtil32.Round(nDura / 1000) < questConditionInfo.nParam2) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nCHECKDURAEVA:
                        UserItem = PlayObject.QuestCheckItem(questConditionInfo.sParam1, ref n1C, ref nMaxDura, ref nDura);
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
                        Hour = DateTime.Now.Hour;
                        Min = DateTime.Now.Minute;
                        Sec = DateTime.Now.Second;
                        MSec = DateTime.Now.Millisecond;
                        if ((Hour < questConditionInfo.nParam1) || (Hour > questConditionInfo.nParam2)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nMIN:
                        if ((questConditionInfo.nParam1 != 0) && (questConditionInfo.nParam2 == 0)) {
                            questConditionInfo.nParam2 = questConditionInfo.nParam1;
                        }
                        Hour = DateTime.Now.Hour;
                        Min = DateTime.Now.Minute;
                        Sec = DateTime.Now.Second;
                        MSec = DateTime.Now.Millisecond;
                        if ((Min < questConditionInfo.nParam1) || (Min > questConditionInfo.nParam2)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nCHECKPKPOINT:
                        if (PlayObject.PvpLevel() < questConditionInfo.nParam1) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nCHECKLUCKYPOINT:
                        if (PlayObject.BodyLuckLevel < questConditionInfo.nParam1) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nCHECKMONMAP:
                        Envirnoment Envir = M2Share.MapMgr.FindMap(questConditionInfo.sParam1);
                        if (Envir != null) {
                            if (M2Share.WorldEngine.GetMapMonster(Envir, null) < questConditionInfo.nParam2) {
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
                        if (PlayObject.IsEnoughBag()) {
                            if ((!string.IsNullOrEmpty(questConditionInfo.sParam1))) {
                                result = false;
                                StdItem StdItem = M2Share.WorldEngine.GetStdItem(questConditionInfo.sParam1);
                                if (StdItem != null) {
                                    if (PlayObject.IsAddWeightAvailable(StdItem.Weight)) {
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
                        if (!CheckGotoLableStringList(PlayObject.ChrName, m_sPath + questConditionInfo.sParam1)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nCHECKACCOUNTLIST:
                        if (!CheckGotoLableStringList(PlayObject.UserAccount, m_sPath + questConditionInfo.sParam1)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nCHECKIPLIST:
                        if (!CheckGotoLableStringList(PlayObject.LoginIpAddr, m_sPath + questConditionInfo.sParam1)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nEQUAL:
                        result = EqualData(PlayObject, questConditionInfo);
                        break;
                    case ScriptConst.nLARGE:
                        result = LargeData(PlayObject, questConditionInfo);
                        break;
                    case ScriptConst.nSMALL:
                        result = Smalldata(PlayObject, questConditionInfo);
                        break;
                    case ScriptConst.nSC_ISSYSOP:
                        if (!(PlayObject.Permission >= 4)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_ISADMIN:
                        if (!(PlayObject.Permission >= 6)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKGROUPCOUNT:
                        if (!ConditionOfCheckGroupCount(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKPOSEDIR:
                        if (!ConditionOfCheckPoseDir(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKPOSELEVEL:
                        if (!ConditionOfCheckPoseLevel(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKPOSEGENDER:
                        if (!ConditionOfCheckPoseGender(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKLEVELEX:
                        if (!ConditionOfCheckLevelEx(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKBONUSPOINT:
                        if (!ConditionOfCheckBonusPoint(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKMARRY:
                        if (!ConditionOfCheckMarry(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKPOSEMARRY:
                        if (!ConditionOfCheckPoseMarry(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKMARRYCOUNT:
                        if (!ConditionOfCheckMarryCount(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKMASTER:
                        if (!ConditionOfCheckMaster(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_HAVEMASTER:
                        if (!ConditionOfHaveMaster(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKPOSEMASTER:
                        if (!ConditionOfCheckPoseMaster(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_POSEHAVEMASTER:
                        if (!ConditionOfPoseHaveMaster(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKISMASTER:
                        if (!ConditionOfCheckIsMaster(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_HASGUILD:
                        if (!ConditionOfCheckHaveGuild(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_ISGUILDMASTER:
                        if (!ConditionOfCheckIsGuildMaster(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKCASTLEMASTER:
                        if (!ConditionOfCheckIsCastleMaster(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_ISCASTLEGUILD:
                        if (!ConditionOfCheckIsCastleaGuild(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_ISATTACKGUILD:
                        if (!ConditionOfCheckIsAttackGuild(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_ISDEFENSEGUILD:
                        if (!ConditionOfCheckIsDefenseGuild(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKCASTLEDOOR:
                        if (!ConditionOfCheckCastleDoorStatus(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_ISATTACKALLYGUILD:
                        if (!ConditionOfCheckIsAttackAllyGuild(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_ISDEFENSEALLYGUILD:
                        if (!ConditionOfCheckIsDefenseAllyGuild(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKPOSEISMASTER:
                        if (!ConditionOfCheckPoseIsMaster(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKNAMEIPLIST:
                        if (!ConditionOfCheckNameIPList(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKACCOUNTIPLIST:
                        if (!ConditionOfCheckAccountIPList(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKSLAVECOUNT:
                        if (!ConditionOfCheckSlaveCount(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_ISNEWHUMAN:
                        if (!PlayObject.IsNewHuman) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKMEMBERTYPE:
                        if (!ConditionOfCheckMemberType(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKMEMBERLEVEL:
                        if (!ConditionOfCheckMemBerLevel(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKGAMEGOLD:
                        if (!ConditionOfCheckGameGold(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKGAMEPOINT:
                        if (!ConditionOfCheckGamePoint(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKNAMELISTPOSITION:
                        if (!ConditionOfCheckNameListPostion(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKGUILDLIST:
                        if (PlayObject.MyGuild != null) {
                            if (!CheckGotoLableStringList(PlayObject.MyGuild.GuildName, m_sPath + questConditionInfo.sParam1)) {
                                result = false;
                            }
                        }
                        else {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKRENEWLEVEL:
                        if (!ConditionOfCheckReNewLevel(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKSLAVELEVEL:
                        if (!ConditionOfCheckSlaveLevel(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKSLAVENAME:
                        if (!ConditionOfCheckSlaveName(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKCREDITPOINT:
                        if (!ConditionOfCheckCreditPoint(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKOFGUILD:
                        if (!ConditionOfCheckOfGuild(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKPAYMENT:
                        if (!ConditionOfCheckPayMent(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKUSEITEM:
                        if (!ConditionOfCheckUseItem(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKBAGSIZE:
                        if (!ConditionOfCheckBagSize(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKLISTCOUNT:
                        if (!ConditionOfCheckListCount(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKDC:
                        if (!ConditionOfCheckDC(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKMC:
                        if (!ConditionOfCheckMC(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKSC:
                        if (!ConditionOfCheckSC(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKHP:
                        if (!ConditionOfCheckHP(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKMP:
                        if (!ConditionOfCheckMP(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKITEMTYPE:
                        if (!ConditionOfCheckItemType(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKEXP:
                        if (!ConditionOfCheckExp(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKCASTLEGOLD:
                        if (!ConditionOfCheckCastleGold(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_PASSWORDERRORCOUNT:
                        if (!ConditionOfCheckPasswordErrorCount(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_ISLOCKPASSWORD:
                        if (!ConditionOfIsLockPassword(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_ISLOCKSTORAGE:
                        if (!ConditionOfIsLockStorage(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKBUILDPOINT:
                        if (!ConditionOfCheckGuildBuildPoint(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKAURAEPOINT:
                        if (!ConditionOfCheckGuildAuraePoint(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKSTABILITYPOINT:
                        if (!ConditionOfCheckStabilityPoint(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKFLOURISHPOINT:
                        if (!ConditionOfCheckFlourishPoint(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKCONTRIBUTION:
                        if (!ConditionOfCheckContribution(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKRANGEMONCOUNT:
                        if (!ConditionOfCheckRangeMonCount(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKITEMADDVALUE:
                        if (!ConditionOfCheckItemAddValue(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKINMAPRANGE:
                        if (!ConditionOfCheckInMapRange(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CASTLECHANGEDAY:
                        if (!ConditionOfCheckCastleChageDay(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CASTLEWARDAY:
                        if (!ConditionOfCheckCastleWarDay(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_ONLINELONGMIN:
                        if (!ConditionOfCheckOnlineLongMin(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKGUILDCHIEFITEMCOUNT:
                        if (!ConditionOfCheckChiefItemCount(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKNAMEDATELIST:
                        if (!ConditionOfCheckNameDateList(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKMAPHUMANCOUNT:
                        if (!ConditionOfCheckMapHumanCount(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKMAPMONCOUNT:
                        if (!ConditionOfCheckMapMonCount(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKVAR:
                        if (!ConditionOfCheckVar(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKSERVERNAME:
                        if (!ConditionOfCheckServerName(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKISONMAP:
                        if (!ConditionOfCheckIsOnMap(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_KILLBYHUM:
                        if ((PlayObject.LastHiter != null) && (PlayObject.LastHiter.Race != ActorRace.Play)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_KILLBYMON:
                        if ((PlayObject.LastHiter != null) && (PlayObject.LastHiter.Race == ActorRace.Play)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKINSAFEZONE:
                        if (!PlayObject.InSafeZone()) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKMAP:
                        if (!ConditionOfCheckMap(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKPOS:
                        if (!ConditionOfCheckPos(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_REVIVESLAVE:
                        if (!ConditionOfReviveSlave(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKMAGICLVL:
                        if (!ConditionOfCheckMagicLvl(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_CHECKGROUPCLASS:
                        if (!ConditionOfCheckGroupClass(PlayObject, questConditionInfo)) {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_ISGROUPMASTER:
                        if (PlayObject.GroupOwner != 0) {
                            if (PlayObject.GroupOwner != PlayObject.ActorId) {
                                result = false;
                            }
                        }
                        else {
                            result = false;
                        }
                        break;
                    case ScriptConst.nSC_ISHIGH:
                        result = ConditionOfIsHigh(PlayObject, questConditionInfo);
                        break;
                    case ScriptConst.nSCHECKDEATHPLAYMON:
                        string s01 = string.Empty;
                        if (!GetValValue(PlayObject, questConditionInfo.sParam1, ref s01)) {
                            s01 = GetLineVariableText(PlayObject, questConditionInfo.sParam1);
                        }
                        result = CheckKillMon2(PlayObject, s01);
                        break;
                }
                if (!result) {
                    break;
                }
            }
            return result;
        }

        private static bool CheckKillMon2(PlayObject PlayObject, string sMonName) {
            return true;
        }

        private bool JmpToLable(PlayObject PlayObject, string sLabel) {
            PlayObject.ScriptGotoCount++;
            if (PlayObject.ScriptGotoCount > M2Share.Config.ScriptGotoCountLimit) {
                return false;
            }
            GotoLable(PlayObject, sLabel, false);
            return true;
        }

        private void GoToQuest(PlayObject PlayObject, int nQuest) {
            for (int i = 0; i < m_ScriptList.Count; i++) {
                ScriptInfo Script = m_ScriptList[i];
                if (Script.QuestCount == nQuest) {
                    PlayObject.MScript = Script;
                    PlayObject.LastNpc = this.ActorId;
                    GotoLable(PlayObject, ScriptConst.sMAIN, false);
                    break;
                }
            }
        }

        private static void GotoLableAddUseDateList(string sHumName, string sListFileName) {
            string s10 = string.Empty;
            string sText;
            bool bo15;
            sListFileName = M2Share.GetEnvirFilePath(sListFileName);
            using StringList LoadList = new StringList();
            if (File.Exists(sListFileName)) {
                LoadList.LoadFromFile(sListFileName);
            }
            bo15 = false;
            for (int i = 0; i < LoadList.Count; i++) {
                sText = LoadList[i].Trim();
                sText = HUtil32.GetValidStrCap(sText, ref s10, new[] { ' ', '\t' });
                if (string.Compare(sHumName, s10, StringComparison.OrdinalIgnoreCase) == 0) {
                    bo15 = true;
                    break;
                }
            }
            if (!bo15)
            {
                s10 = $"{sHumName}    {DateTime.Today}";
                LoadList.Add(s10);
                try {
                    LoadList.SaveToFile(sListFileName);
                }
                catch {
                    M2Share.Logger.Error("saving fail.... => " + sListFileName);
                }
            }
        }

        private static void GotoLableAddList(string sHumName, string sListFileName) {
            sListFileName = M2Share.GetEnvirFilePath(sListFileName);
            using StringList LoadList = new StringList();
            if (File.Exists(sListFileName)) {
                LoadList.LoadFromFile(sListFileName);
            }
            bool bo15 = false;
            for (int i = 0; i < LoadList.Count; i++) {
                string s10 = LoadList[i].Trim();
                if (string.Compare(sHumName, s10, StringComparison.OrdinalIgnoreCase) == 0) {
                    bo15 = true;
                    break;
                }
            }
            if (!bo15) {
                LoadList.Add(sHumName);
                try {
                    LoadList.SaveToFile(sListFileName);
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
            using StringList LoadList = new StringList();
            if (File.Exists(sListFileName)) {
                LoadList.LoadFromFile(sListFileName);
            }
            bool bo15 = false;
            for (int i = 0; i < LoadList.Count; i++) {
                sText = LoadList[i].Trim();
                sText = HUtil32.GetValidStrCap(sText, ref s10, new[] { ' ', '\t' });
                if (string.Compare(sHumName, s10, StringComparison.OrdinalIgnoreCase) == 0) {
                    bo15 = true;
                    LoadList.RemoveAt(i);
                    break;
                }
            }
            if (bo15) {
                LoadList.SaveToFile(sListFileName);
            }
        }

        private static void GotoLableDelList(string playName, string sListFileName) {
            bool bo15;
            sListFileName = M2Share.GetEnvirFilePath(sListFileName);
            using StringList LoadList = new StringList();
            if (File.Exists(sListFileName)) {
                LoadList.LoadFromFile(sListFileName);
            }
            bo15 = false;
            for (int i = 0; i < LoadList.Count; i++) {
                string s10 = LoadList[i].Trim();
                if (string.Compare(playName, s10, StringComparison.OrdinalIgnoreCase) == 0) {
                    LoadList.RemoveAt(i);
                    bo15 = true;
                    break;
                }
            }
            if (bo15) {
                LoadList.SaveToFile(sListFileName);
            }
        }

        private void GotoLableTakeItem(PlayObject PlayObject, string sItemName, int nItemCount, ref string sC) {
            UserItem UserItem;
            StdItem StdItem;
            if (string.Compare(sItemName, Grobal2.StringGoldName, StringComparison.OrdinalIgnoreCase) == 0) {
                PlayObject.DecGold(nItemCount);
                PlayObject.GoldChanged();
                if (M2Share.GameLogGold) {
                    M2Share.EventSource.AddEventLog(10, PlayObject.MapName + "\t" + PlayObject.CurrX + "\t" + PlayObject.CurrY + "\t" + PlayObject.ChrName + "\t" + Grobal2.StringGoldName + "\t" + nItemCount + "\t" + '1' + "\t" + ChrName);
                }
                return;
            }
            for (int i = PlayObject.ItemList.Count - 1; i >= 0; i--) {
                if (nItemCount <= 0) {
                    break;
                }
                UserItem = PlayObject.ItemList[i];
                if (string.Compare(M2Share.WorldEngine.GetStdItemName(UserItem.Index), sItemName, StringComparison.OrdinalIgnoreCase) == 0) {
                    StdItem = M2Share.WorldEngine.GetStdItem(UserItem.Index);
                    if (StdItem.NeedIdentify == 1) {
                        M2Share.EventSource.AddEventLog(10, PlayObject.MapName + "\t" + PlayObject.CurrX + "\t" + PlayObject.CurrY + "\t" + PlayObject.ChrName + "\t" + sItemName + "\t" + UserItem.MakeIndex + "\t" + '1' + "\t" + ChrName);
                    }
                    PlayObject.SendDelItems(UserItem);
                    sC = M2Share.WorldEngine.GetStdItemName(UserItem.Index);
                    Dispose(UserItem);
                    PlayObject.ItemList.RemoveAt(i);
                    nItemCount -= 1;
                }
            }
        }

        public void GotoLableGiveItem(PlayObject PlayObject, string sItemName, int nItemCount) {
            UserItem UserItem;
            StdItem StdItem;
            if (string.Compare(sItemName, Grobal2.StringGoldName, StringComparison.OrdinalIgnoreCase) == 0) {
                PlayObject.IncGold(nItemCount);
                PlayObject.GoldChanged();
                if (M2Share.GameLogGold) {
                    M2Share.EventSource.AddEventLog(9, PlayObject.MapName + "\t" + PlayObject.CurrX + "\t" + PlayObject.CurrY + "\t" + PlayObject.ChrName + "\t" + Grobal2.StringGoldName + "\t" + nItemCount + "\t" + '1' + "\t" + ChrName);
                }
                return;
            }
            if (M2Share.WorldEngine.GetStdItemIdx(sItemName) > 0) {
                if (!(nItemCount >= 1 && nItemCount <= 50)) {
                    nItemCount = 1;
                }
                for (int i = 0; i < nItemCount; i++) {
                    if (PlayObject.IsEnoughBag()) {
                        UserItem = new UserItem();
                        if (M2Share.WorldEngine.CopyToUserItemFromName(sItemName, ref UserItem)) {
                            PlayObject.ItemList.Add(UserItem);
                            PlayObject.SendAddItem(UserItem);
                            StdItem = M2Share.WorldEngine.GetStdItem(UserItem.Index);
                            if (StdItem.NeedIdentify == 1) {
                                M2Share.EventSource.AddEventLog(9, PlayObject.MapName + "\t" + PlayObject.CurrX + "\t" + PlayObject.CurrY + "\t" + PlayObject.ChrName + "\t" + sItemName + "\t" + UserItem.MakeIndex + "\t" + '1' + "\t" + ChrName);
                            }
                        }
                        else {
                            Dispose(UserItem);
                        }
                    }
                    else {
                        UserItem = new UserItem();
                        if (M2Share.WorldEngine.CopyToUserItemFromName(sItemName, ref UserItem)) {
                            StdItem = M2Share.WorldEngine.GetStdItem(UserItem.Index);
                            if (StdItem.NeedIdentify == 1) {
                                M2Share.EventSource.AddEventLog(9, PlayObject.MapName + "\t" + PlayObject.CurrX + "\t" + PlayObject.CurrY + "\t" + PlayObject.ChrName + "\t" + sItemName + "\t" + UserItem.MakeIndex + "\t" + '1' + "\t" + ChrName);
                            }
                            PlayObject.DropItemDown(UserItem, 3, false, PlayObject.ActorId, 0);
                        }
                        Dispose(UserItem);
                    }
                }
            }
        }

        private static void GotoLableTakeWItem(PlayObject PlayObject, string sItemName, int nItemCount) {
            string sC;
            if (HUtil32.CompareLStr(sItemName, "[NECKLACE]", 4)) {
                if (PlayObject.UseItems[ItemLocation.Necklace].Index > 0) {
                    PlayObject.SendDelItems(PlayObject.UseItems[ItemLocation.Necklace]);
                    sC = M2Share.WorldEngine.GetStdItemName(PlayObject.UseItems[ItemLocation.Necklace].Index);
                    PlayObject.UseItems[ItemLocation.Necklace].Index = 0;
                    return;
                }
            }
            if (HUtil32.CompareLStr(sItemName, "[RING]", 4)) {
                if (PlayObject.UseItems[ItemLocation.Ringl].Index > 0) {
                    PlayObject.SendDelItems(PlayObject.UseItems[ItemLocation.Ringl]);
                    sC = M2Share.WorldEngine.GetStdItemName(PlayObject.UseItems[ItemLocation.Ringl].Index);
                    PlayObject.UseItems[ItemLocation.Ringl].Index = 0;
                    return;
                }
                if (PlayObject.UseItems[ItemLocation.Ringr].Index > 0) {
                    PlayObject.SendDelItems(PlayObject.UseItems[ItemLocation.Ringr]);
                    sC = M2Share.WorldEngine.GetStdItemName(PlayObject.UseItems[ItemLocation.Ringr].Index);
                    PlayObject.UseItems[ItemLocation.Ringr].Index = 0;
                    return;
                }
            }
            if (HUtil32.CompareLStr(sItemName, "[ARMRING]", 4)) {
                if (PlayObject.UseItems[ItemLocation.ArmRingl].Index > 0) {
                    PlayObject.SendDelItems(PlayObject.UseItems[ItemLocation.ArmRingl]);
                    sC = M2Share.WorldEngine.GetStdItemName(PlayObject.UseItems[ItemLocation.ArmRingl].Index);
                    PlayObject.UseItems[ItemLocation.ArmRingl].Index = 0;
                    return;
                }
                if (PlayObject.UseItems[ItemLocation.ArmRingr].Index > 0) {
                    PlayObject.SendDelItems(PlayObject.UseItems[ItemLocation.ArmRingr]);
                    sC = M2Share.WorldEngine.GetStdItemName(PlayObject.UseItems[ItemLocation.ArmRingr].Index);
                    PlayObject.UseItems[ItemLocation.ArmRingr].Index = 0;
                    return;
                }
            }
            if (HUtil32.CompareLStr(sItemName, "[WEAPON]", 4)) {
                if (PlayObject.UseItems[ItemLocation.Weapon].Index > 0) {
                    PlayObject.SendDelItems(PlayObject.UseItems[ItemLocation.Weapon]);
                    sC = M2Share.WorldEngine.GetStdItemName(PlayObject.UseItems[ItemLocation.Weapon].Index);
                    PlayObject.UseItems[ItemLocation.Weapon].Index = 0;
                    return;
                }
            }
            if (HUtil32.CompareLStr(sItemName, "[HELMET]", 4)) {
                if (PlayObject.UseItems[ItemLocation.Helmet].Index > 0) {
                    PlayObject.SendDelItems(PlayObject.UseItems[ItemLocation.Helmet]);
                    sC = M2Share.WorldEngine.GetStdItemName(PlayObject.UseItems[ItemLocation.Helmet].Index);
                    PlayObject.UseItems[ItemLocation.Helmet].Index = 0;
                    return;
                }
            }
            if (HUtil32.CompareLStr(sItemName, "[DRESS]", 4)) {
                if (PlayObject.UseItems[ItemLocation.Dress].Index > 0) {
                    PlayObject.SendDelItems(PlayObject.UseItems[ItemLocation.Dress]);
                    sC = M2Share.WorldEngine.GetStdItemName(PlayObject.UseItems[ItemLocation.Dress].Index);
                    PlayObject.UseItems[ItemLocation.Dress].Index = 0;
                    return;
                }
            }
            if (HUtil32.CompareLStr(sItemName, "[U_BUJUK]", 4)) {
                if (PlayObject.UseItems[ItemLocation.Bujuk].Index > 0) {
                    PlayObject.SendDelItems(PlayObject.UseItems[ItemLocation.Bujuk]);
                    sC = M2Share.WorldEngine.GetStdItemName(PlayObject.UseItems[ItemLocation.Bujuk].Index);
                    PlayObject.UseItems[ItemLocation.Bujuk].Index = 0;
                    return;
                }
            }
            if (HUtil32.CompareLStr(sItemName, "[U_BELT]", 4)) {
                if (PlayObject.UseItems[ItemLocation.Belt].Index > 0) {
                    PlayObject.SendDelItems(PlayObject.UseItems[ItemLocation.Belt]);
                    sC = M2Share.WorldEngine.GetStdItemName(PlayObject.UseItems[ItemLocation.Belt].Index);
                    PlayObject.UseItems[ItemLocation.Belt].Index = 0;
                    return;
                }
            }
            if (HUtil32.CompareLStr(sItemName, "[U_BOOTS]", 4)) {
                if (PlayObject.UseItems[ItemLocation.Boots].Index > 0) {
                    PlayObject.SendDelItems(PlayObject.UseItems[ItemLocation.Boots]);
                    sC = M2Share.WorldEngine.GetStdItemName(PlayObject.UseItems[ItemLocation.Boots].Index);
                    PlayObject.UseItems[ItemLocation.Boots].Index = 0;
                    return;
                }
            }
            if (HUtil32.CompareLStr(sItemName, "[U_CHARM]", 4)) {
                if (PlayObject.UseItems[ItemLocation.Charm].Index > 0) {
                    PlayObject.SendDelItems(PlayObject.UseItems[ItemLocation.Charm]);
                    sC = M2Share.WorldEngine.GetStdItemName(PlayObject.UseItems[ItemLocation.Charm].Index);
                    PlayObject.UseItems[ItemLocation.Charm].Index = 0;
                    return;
                }
            }
            for (int i = 0; i < PlayObject.UseItems.Length; i++) {
                if (nItemCount <= 0) {
                    return;
                }
                if (PlayObject.UseItems[i].Index > 0) {
                    string sName = M2Share.WorldEngine.GetStdItemName(PlayObject.UseItems[i].Index);
                    if (string.Compare(sName, sItemName, StringComparison.OrdinalIgnoreCase) == 0) {
                        PlayObject.SendDelItems(PlayObject.UseItems[i]);
                        PlayObject.UseItems[i].Index = 0;
                        nItemCount -= 1;
                    }
                }
            }
        }

        private bool GotoLableQuestActionProcess(PlayObject PlayObject, IList<QuestActionInfo> ActionList, ref string sC, ref UserItem UserItem, ref bool bo11) {
            bool result = true;
            int n28;
            int n2C;
            int n20X;
            int n24Y;
            string s44 = string.Empty;
            Envirnoment Envir;
            IList<BaseObject> List58;
            PlayObject User;
            int n18 = 0;
            int n34 = 0;
            int n38 = 0;
            int n3C = 0;
            int n40 = 0;
            string s50;
            for (int i = 0; i < ActionList.Count; i++) {
                QuestActionInfo QuestActionInfo = ActionList[i];
                switch (QuestActionInfo.nCmdCode) {
                    case ScriptConst.nSET:
                        n28 = HUtil32.StrToInt(QuestActionInfo.sParam1, 0);
                        n2C = HUtil32.StrToInt(QuestActionInfo.sParam2, 0);
                        PlayObject.SetQuestFlagStatus(n28, n2C);
                        break;
                    case ScriptConst.nTAKE:
                        GotoLableTakeItem(PlayObject, QuestActionInfo.sParam1, QuestActionInfo.nParam2, ref sC);
                        break;
                    case ScriptConst.nSC_GIVE:
                        ActionOfGiveItem(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nTAKEW:
                        GotoLableTakeWItem(PlayObject, QuestActionInfo.sParam1, QuestActionInfo.nParam2);
                        break;
                    case ScriptConst.nCLOSE:
                        PlayObject.SendMsg(this, Messages.RM_MERCHANTDLGCLOSE, 0, ActorId, 0, 0, "");
                        break;
                    case ScriptConst.nRESET:
                        for (int k = 0; k < QuestActionInfo.nParam2; k++) {
                            PlayObject.SetQuestFlagStatus(QuestActionInfo.nParam1 + k, 0);
                        }
                        break;
                    case ScriptConst.nSETOPEN:
                        n28 = HUtil32.StrToInt(QuestActionInfo.sParam1, 0);
                        n2C = HUtil32.StrToInt(QuestActionInfo.sParam2, 0);
                        PlayObject.SetQuestUnitOpenStatus(n28, n2C);
                        break;
                    case ScriptConst.nSETUNIT:
                        n28 = HUtil32.StrToInt(QuestActionInfo.sParam1, 0);
                        n2C = HUtil32.StrToInt(QuestActionInfo.sParam2, 0);
                        PlayObject.SetQuestUnitStatus(n28, n2C);
                        break;
                    case ScriptConst.nRESETUNIT:
                        for (int k = 0; k < QuestActionInfo.nParam2; k++) {
                            PlayObject.SetQuestUnitStatus(QuestActionInfo.nParam1 + k, 0);
                        }
                        break;
                    case ScriptConst.nBREAK:
                        result = false;
                        break;
                    case ScriptConst.nTIMERECALL:
                        PlayObject.IsTimeRecall = true;
                        PlayObject.TimeRecallMoveMap = PlayObject.MapName;
                        PlayObject.TimeRecallMoveX = PlayObject.CurrX;
                        PlayObject.TimeRecallMoveY = PlayObject.CurrY;
                        PlayObject.TimeRecallTick = HUtil32.GetTickCount() + (QuestActionInfo.nParam1 * 60 * 1000);
                        break;
                    case ScriptConst.nSC_PARAM1:
                        n34 = QuestActionInfo.nParam1;
                        s44 = QuestActionInfo.sParam1;
                        break;
                    case ScriptConst.nSC_PARAM2:
                        n38 = QuestActionInfo.nParam1;
                        string s48 = QuestActionInfo.sParam1;
                        break;
                    case ScriptConst.nSC_PARAM3:
                        n3C = QuestActionInfo.nParam1;
                        string s4C = QuestActionInfo.sParam1;
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
                        PlayObject.SendRefMsg(Messages.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
                        PlayObject.SpaceMove(QuestActionInfo.sParam1, (short)QuestActionInfo.nParam2, (short)QuestActionInfo.nParam3, 0);
                        bo11 = true;
                        break;
                    case ScriptConst.nMAP:
                        PlayObject.SendRefMsg(Messages.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
                        PlayObject.MapRandomMove(QuestActionInfo.sParam1, 0);
                        bo11 = true;
                        break;
                    case ScriptConst.nTAKECHECKITEM:
                        if (UserItem != null) {
                            PlayObject.QuestTakeCheckItem(UserItem);
                        }
                        else {
                            ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sTAKECHECKITEM);
                        }
                        break;
                    case ScriptConst.nMONGEN:
                        for (int k = 0; k < QuestActionInfo.nParam2; k++) {
                            n20X = M2Share.RandomNumber.Random(QuestActionInfo.nParam3 * 2 + 1) + (n38 - QuestActionInfo.nParam3);
                            n24Y = M2Share.RandomNumber.Random(QuestActionInfo.nParam3 * 2 + 1) + (n3C - QuestActionInfo.nParam3);
                            M2Share.WorldEngine.RegenMonsterByName(s44, (short)n20X, (short)n24Y, QuestActionInfo.sParam1);
                        }
                        break;
                    case ScriptConst.nMONCLEAR:
                        List58 = new List<BaseObject>();
                        M2Share.WorldEngine.GetMapMonster(M2Share.MapMgr.FindMap(QuestActionInfo.sParam1), List58);
                        for (int k = 0; k < List58.Count; k++) {
                            List58[k].NoItem = true;
                            List58[k].WAbil.HP = 0;
                            List58[k].MakeGhost();
                        }
                        List58.Clear();
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
                        PlayObject.IsTimeRecall = false;
                        break;
                    case ScriptConst.nCHANGEMODE:
                        switch (QuestActionInfo.nParam1)
                        {
                            case 1:
                                CommandMgr.Execute(PlayObject, "ChangeAdminMode");
                                break;
                            case 2:
                                CommandMgr.Execute(PlayObject, "ChangeSuperManMode");
                                break;
                            case 3:
                                CommandMgr.Execute(PlayObject, "ChangeObMode");
                                break;
                            default:
                                ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sCHANGEMODE);
                                break;
                        }
                        break;
                    case ScriptConst.nPKPOINT:
                        if (QuestActionInfo.nParam1 == 0) {
                            PlayObject.PkPoint = 0;
                        }
                        else {
                            if (QuestActionInfo.nParam1 < 0) {
                                if ((PlayObject.PkPoint + QuestActionInfo.nParam1) >= 0) {
                                    PlayObject.PkPoint += QuestActionInfo.nParam1;
                                }
                                else {
                                    PlayObject.PkPoint = 0;
                                }
                            }
                            else {
                                if ((PlayObject.PkPoint + QuestActionInfo.nParam1) > 10000) {
                                    PlayObject.PkPoint = 10000;
                                }
                                else {
                                    PlayObject.PkPoint += QuestActionInfo.nParam1;
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
                        PlayObject.BoReconnection = true;
                        PlayObject.BoSoftClose = true;
                        break;
                    case ScriptConst.nTHROWITEM://将指定物品刷新到指定地图坐标范围内
                        ActionOfThrowitem(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nMOVR:
                        MovrData(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nEXCHANGEMAP:
                        Envir = M2Share.MapMgr.FindMap(QuestActionInfo.sParam1);
                        if (Envir != null) {
                            List58 = new List<BaseObject>();
                            M2Share.WorldEngine.GetMapRageHuman(Envir, 0, 0, 1000, ref List58);
                            if (List58.Count > 0) {
                                User = (PlayObject)List58[0];
                                User.MapRandomMove(MapName, 0);
                            }
                            PlayObject.MapRandomMove(QuestActionInfo.sParam1, 0);
                        }
                        else {
                            ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sEXCHANGEMAP);
                        }
                        break;
                    case ScriptConst.nRECALLMAP:
                        Envir = M2Share.MapMgr.FindMap(QuestActionInfo.sParam1);
                        if (Envir != null) {
                            List58 = new List<BaseObject>();
                            M2Share.WorldEngine.GetMapRageHuman(Envir, 0, 0, 1000, ref List58);
                            for (int k = 0; k < List58.Count; k++) {
                                User = (PlayObject)List58[k];
                                User.MapRandomMove(MapName, 0);
                                if (k > 20) {
                                    break;
                                }
                            }
                            //List58.Free;
                        }
                        else {
                            ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sRECALLMAP);
                        }
                        break;
                    case ScriptConst.nADDBATCH:
                        if (BatchParamsList == null) {
                            BatchParamsList = new List<ScriptParams>();
                        }
                        BatchParamsList.Add(new ScriptParams() {
                            sParams = QuestActionInfo.sParam1,
                            nParams = n18
                        });
                        break;
                    case ScriptConst.nBATCHDELAY:
                        n18 = QuestActionInfo.nParam1 * 1000;
                        break;
                    case ScriptConst.nBATCHMOVE:
                        int n20 = 0;
                        for (int k = 0; k < BatchParamsList.Count; k++) {
                            ScriptParams batchParam = BatchParamsList[k];
                            PlayObject.SendDelayMsg(ActorId, Messages.RM_RANDOMSPACEMOVE, 0, 0, 0, 0, BatchParamsList[k].sParams, batchParam.nParams + n20);
                            n20 += batchParam.nParams;
                        }
                        break;
                    case ScriptConst.nPLAYDICE:
                        PlayObject.PlayDiceLabel = QuestActionInfo.sParam2;
                        PlayObject.SendMsg(this, Messages.RM_PLAYDICE, (short)QuestActionInfo.nParam1, HUtil32.MakeLong(HUtil32.MakeWord((ushort)PlayObject.MDyVal[0], (ushort)PlayObject.MDyVal[1]), HUtil32.MakeWord((ushort)PlayObject.MDyVal[2], (ushort)PlayObject.MDyVal[3])), HUtil32.MakeLong(HUtil32.MakeWord((ushort)PlayObject.MDyVal[4], (ushort)PlayObject.MDyVal[5]), HUtil32.MakeWord((ushort)PlayObject.MDyVal[6], (ushort)PlayObject.MDyVal[7])), HUtil32.MakeLong(HUtil32.MakeWord((ushort)PlayObject.MDyVal[8], (ushort)PlayObject.MDyVal[9]), 0), QuestActionInfo.sParam2);
                        bo11 = true;
                        break;
                    case ScriptConst.nADDNAMELIST:
                        GotoLableAddList(PlayObject.ChrName, m_sPath + QuestActionInfo.sParam1);
                        break;
                    case ScriptConst.nDELNAMELIST:
                        GotoLableDelList(PlayObject.ChrName, m_sPath + QuestActionInfo.sParam1);
                        break;
                    case ScriptConst.nADDUSERDATE:
                        GotoLableAddUseDateList(PlayObject.ChrName, m_sPath + QuestActionInfo.sParam1);
                        break;
                    case ScriptConst.nDELUSERDATE:
                        GotoLableDelUseDateList(PlayObject.ChrName, m_sPath + QuestActionInfo.sParam1);
                        break;
                    case ScriptConst.nADDGUILDLIST:
                        if (PlayObject.MyGuild != null) {
                            GotoLableAddList(PlayObject.MyGuild.GuildName, m_sPath + QuestActionInfo.sParam1);
                        }
                        break;
                    case ScriptConst.nDELGUILDLIST:
                        if (PlayObject.MyGuild != null) {
                            GotoLableDelList(PlayObject.MyGuild.GuildName, m_sPath + QuestActionInfo.sParam1);
                        }
                        break;
                    case ScriptConst.nSC_LINEMSG:
                    case ScriptConst.nSENDMSG:
                        ActionOfLineMsg(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nADDACCOUNTLIST:
                        GotoLableAddList(PlayObject.UserAccount, m_sPath + QuestActionInfo.sParam1);
                        break;
                    case ScriptConst.nDELACCOUNTLIST:
                        GotoLableDelList(PlayObject.UserAccount, m_sPath + QuestActionInfo.sParam1);
                        break;
                    case ScriptConst.nADDIPLIST:
                        GotoLableAddList(PlayObject.LoginIpAddr, m_sPath + QuestActionInfo.sParam1);
                        break;
                    case ScriptConst.nDELIPLIST:
                        GotoLableDelList(PlayObject.LoginIpAddr, m_sPath + QuestActionInfo.sParam1);
                        break;
                    case ScriptConst.nGOQUEST:
                        GoToQuest(PlayObject, QuestActionInfo.nParam1);
                        break;
                    case ScriptConst.nENDQUEST:
                        PlayObject.MScript = null;
                        break;
                    case ScriptConst.nGOTO:
                        if (!JmpToLable(PlayObject, QuestActionInfo.sParam1)) {
                            // ScriptActionError(PlayObject,'',QuestActionInfo,sGOTO);
                            M2Share.Logger.Error("[脚本死循环] NPC:" + ChrName + " 位置:" + MapName + '(' + CurrX + ':' + CurrY + ')' + " 命令:" + ScriptConst.sGOTO + ' ' + QuestActionInfo.sParam1);
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
                        ActionOfClearList(PlayObject, QuestActionInfo);
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
                        PlayObject.BoSendMsgFlag = true;
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
                        ActionOfHumanHp(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_HUMANMP:
                        ActionOfHumanMp(PlayObject, QuestActionInfo);
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
                        if ((HUtil32.GetTickCount() - PlayObject.QueryBagItemsTick) > M2Share.Config.QueryBagItemsTick) {
                            PlayObject.QueryBagItemsTick = HUtil32.GetTickCount();
                            PlayObject.ClientQueryBagItems();
                        }
                        else {
                            PlayObject.SysMsg(Settings.QUERYBAGITEMS, MsgColor.Red, MsgType.Hint);
                        }
                        break;
                    case ScriptConst.nSC_SETRANDOMNO:
                        while (true) {
                            n2C = M2Share.RandomNumber.Random(999999);
                            if ((n2C >= 1000) && (n2C.ToString() != PlayObject.RandomNo)) {
                                PlayObject.RandomNo = n2C.ToString();
                                break;
                            }
                        }
                        break;
                    case ScriptConst.nOPENYBDEAL:
                        ActionOfOpenSaleDeal(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nQUERYYBSELL:
                        ActionOfQuerySaleSell(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nQUERYYBDEAL:
                        ActionOfQueryTrustDeal(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nDELAYGOTO:
                        PlayObject.IsTimeGoto = true;
                        int m_DelayGoto = HUtil32.StrToInt(GetLineVariableText(PlayObject, QuestActionInfo.sParam1), 0);//变量操作
                        if (m_DelayGoto == 0) {
                            int delayCount = 0;
                            GetValValue(PlayObject, QuestActionInfo.sParam1, ref delayCount);
                            m_DelayGoto = delayCount;
                        }
                        if (m_DelayGoto > 0) {
                            PlayObject.TimeGotoTick = HUtil32.GetTickCount() + m_DelayGoto;
                        }
                        else {
                            PlayObject.TimeGotoTick = HUtil32.GetTickCount() + QuestActionInfo.nParam1;//毫秒
                        }
                        PlayObject.TimeGotoLable = QuestActionInfo.sParam2;
                        PlayObject.TimeGotoNpc = this;
                        break;
                    case ScriptConst.nCLEARDELAYGOTO:
                        PlayObject.IsTimeGoto = false;
                        PlayObject.TimeGotoLable = "";
                        PlayObject.TimeGotoNpc = null;
                        break;
                    case ScriptConst.nSC_QUERYVALUE:
                        ActionOfQueryValue(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_KILLSLAVENAME:
                        ActionOfKillSlaveName(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_QUERYITEMDLG:
                        ActionOfQueryItemDlg(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_UPGRADEDLGITEM:
                        ActionOfUpgradeDlgItem(PlayObject, QuestActionInfo);
                        break;
                    case ScriptConst.nSC_GETDLGITEMVALUE:

                        break;
                    case ScriptConst.nSC_TAKEDLGITEM:

                        break;
                }
            }
            return result;
        }

        private static void ActionOfUpgradeDlgItem(PlayObject PlayObject, QuestActionInfo QuestActionInfo) {

        }

        private void ActionOfQueryItemDlg(PlayObject PlayObject, QuestActionInfo QuestActionInfo) {
            PlayObject.TakeDlgItem = QuestActionInfo.nParam3 != 0;
            PlayObject.GotoNpcLabel = QuestActionInfo.sParam2;
            string sHint = QuestActionInfo.sParam1;
            if (string.IsNullOrEmpty(sHint)) sHint = "请输入:";
            PlayObject.SendDefMessage(Messages.SM_QUERYITEMDLG, ActorId, 0, 0, 0, sHint);
        }

        private void ActionOfKillSlaveName(PlayObject PlayObject, QuestActionInfo QuestActionInfo) {
            string sSlaveName = QuestActionInfo.sParam1;
            if (string.IsNullOrEmpty(sSlaveName)) {
                ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSC_KILLSLAVENAME);
                return;
            }
            if (sSlaveName.Equals("*") || string.Compare(sSlaveName, "ALL", StringComparison.OrdinalIgnoreCase) == 0) {
                for (int i = 0; i < PlayObject.SlaveList.Count; i++) {
                    PlayObject.SlaveList[i].WAbil.HP = 0;
                }
                return;
            }
            for (int i = 0; i < PlayObject.SlaveList.Count; i++) {
                BaseObject BaseObject = PlayObject.SlaveList[i];
                if (!Death && (string.Compare(sSlaveName, BaseObject.ChrName, StringComparison.OrdinalIgnoreCase) == 0)) {
                    BaseObject.WAbil.HP = 0;
                }
            }
        }

        private static void ActionOfQueryValue(PlayObject PlayObject, QuestActionInfo QuestActionInfo) {
            int btStrLabel = QuestActionInfo.nParam1;
            if (btStrLabel < 100) {
                btStrLabel = 0;
            }
            PlayObject.ValLabel = (byte)btStrLabel;
            byte btType = (byte)QuestActionInfo.nParam2;
            if (btType > 3) {
                btType = 0;
            }
            PlayObject.ValType = btType;
            int btLen = HUtil32._MAX(1, QuestActionInfo.nParam3);
            PlayObject.GotoNpcLabel = QuestActionInfo.sParam4;
            string sHint = QuestActionInfo.sParam5;
            PlayObject.ValNpcType = 0;
            if (string.Compare(QuestActionInfo.sParam6, "QF", StringComparison.OrdinalIgnoreCase) == 0) {
                PlayObject.ValNpcType = 1;
            }
            else if (string.Compare(QuestActionInfo.sParam6, "QM", StringComparison.OrdinalIgnoreCase) == 0) {
                PlayObject.ValNpcType = 2;
            }
            if (string.IsNullOrEmpty(sHint)) {
                sHint = "请输入：";
            }
            PlayObject.SendDefMessage(Messages.SM_QUERYVALUE, 0, HUtil32.MakeWord(btType, (ushort)btLen), 0, 0, sHint);
        }

        private void GotoLableSendMerChantSayMsg(PlayObject PlayObject, string sMsg, bool boFlag) {
            sMsg = GetLineVariableText(PlayObject, sMsg);
            PlayObject.GetScriptLabel(sMsg);
            if (boFlag) {
                PlayObject.SendPriorityMsg(this, Messages.RM_MERCHANTSAY, 0, 0, 0, 0, ChrName + '/' + sMsg, MessagePriority.High);
            }
            else {
                PlayObject.SendMsg(this, Messages.RM_MERCHANTSAY, 0, 0, 0, 0, ChrName + '/' + sMsg);
            }
        }
    }
}