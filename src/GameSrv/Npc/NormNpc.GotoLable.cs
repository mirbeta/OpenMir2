using GameSrv.Actor;
using GameSrv.GameCommand;
using GameSrv.Items;
using GameSrv.Maps;
using GameSrv.Player;
using GameSrv.Script;
using SystemModule.Common;
using SystemModule.Enums;
using SystemModule.Packets.ClientPackets;

namespace GameSrv.Npc
{
    public partial class NormNpc
    {
        private void GotoLable(PlayObject playObject, string sLabel, bool boExtJmp, string sMsg)
        {
            if (playObject.LastNpc != this.ActorId)
            {
                playObject.LastNpc = 0;
                playObject.MScript = null;
            }
            ScriptInfo script = null;
            SayingRecord sayingRecord;
            UserItem userItem = null;
            string sC = string.Empty;
            if (string.Compare("@main", sLabel, StringComparison.OrdinalIgnoreCase) == 0)
            {
                for (int i = 0; i < m_ScriptList.Count; i++)
                {
                    ScriptInfo script3C = m_ScriptList[i];
                    if (script3C.RecordList.TryGetValue(sLabel, out _))
                    {
                        script = script3C;
                        playObject.MScript = script;
                        playObject.LastNpc = this.ActorId;
                        break;
                    }
                }
            }
            if (script == null)
            {
                if (playObject.MScript != null)
                {
                    for (int i = m_ScriptList.Count - 1; i >= 0; i--)
                    {
                        if (m_ScriptList[i] == playObject.MScript)
                        {
                            script = m_ScriptList[i];
                        }
                    }
                }
                if (script == null)
                {
                    for (int i = m_ScriptList.Count - 1; i >= 0; i--)
                    {
                        if (CheckGotoLableQuestStatus(playObject, m_ScriptList[i]))
                        {
                            script = m_ScriptList[i];
                            playObject.MScript = script;
                            playObject.LastNpc = this.ActorId;
                        }
                    }
                }
            }
            // 跳转到指定示签，执行
            if (script != null)
            {
                if (script.RecordList.TryGetValue(sLabel, out sayingRecord))
                {
                    if (boExtJmp && sayingRecord.boExtJmp == false)
                    {
                        return;
                    }
                    string sSendMsg = string.Empty;
                    for (int i = 0; i < sayingRecord.ProcedureList.Count; i++)
                    {
                        SayingProcedure sayingProcedure = sayingRecord.ProcedureList[i];
                        bool bo11 = false;
                        if (GotoLableQuestCheckCondition(playObject, sayingProcedure.ConditionList, ref sC, ref userItem))
                        {
                            sSendMsg = sSendMsg + sayingProcedure.sSayMsg;
                            if (!GotoLableQuestActionProcess(playObject, sayingProcedure.ActionList, ref sC, ref userItem, ref bo11))
                            {
                                break;
                            }
                            if (bo11)
                            {
                                GotoLableSendMerChantSayMsg(playObject, sSendMsg, true);
                            }
                        }
                        else
                        {
                            sSendMsg = sSendMsg + sayingProcedure.sElseSayMsg;
                            if (!GotoLableQuestActionProcess(playObject, sayingProcedure.ElseActionList, ref sC, ref userItem, ref bo11))
                            {
                                break;
                            }
                            if (bo11)
                            {
                                GotoLableSendMerChantSayMsg(playObject, sSendMsg, true);
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(sSendMsg))
                    {
                        GotoLableSendMerChantSayMsg(playObject, sSendMsg, false);
                    }
                }
            }
        }

        public void GotoLable(PlayObject playObject, string sLabel, bool boExtJmp)
        {
            GotoLable(playObject, sLabel, boExtJmp, string.Empty);
        }

        private static bool CheckGotoLableQuestStatus(PlayObject playObject, ScriptInfo scriptInfo)
        {
            bool result = true;
            if (!scriptInfo.IsQuest)
            {
                return true;
            }
            int nIndex = 0;
            while (true)
            {
                if ((scriptInfo.QuestInfo[nIndex].nRandRage > 0) && (M2Share.RandomNumber.Random(scriptInfo.QuestInfo[nIndex].nRandRage) != 0))
                {
                    result = false;
                    break;
                }
                if (playObject.GetQuestFalgStatus(scriptInfo.QuestInfo[nIndex].wFlag) != scriptInfo.QuestInfo[nIndex].btValue)
                {
                    result = false;
                    break;
                }
                nIndex++;
                if (nIndex >= 10)
                {
                    break;
                }
            }
            return result;
        }

        private static UserItem CheckGotoLableItemW(PlayObject playObject, string sItemType, int nParam)
        {
            UserItem result = null;
            int nCount = 0;
            if (HUtil32.CompareLStr(sItemType, "[NECKLACE]", 4))
            {
                if (playObject.UseItems[ItemLocation.Necklace].Index > 0)
                {
                    result = playObject.UseItems[ItemLocation.Necklace];
                }
                return result;
            }
            if (HUtil32.CompareLStr(sItemType, "[RING]", 4))
            {
                if (playObject.UseItems[ItemLocation.Ringl].Index > 0)
                {
                    result = playObject.UseItems[ItemLocation.Ringl];
                }
                if (playObject.UseItems[ItemLocation.Ringr].Index > 0)
                {
                    result = playObject.UseItems[ItemLocation.Ringr];
                }
                return result;
            }
            if (HUtil32.CompareLStr(sItemType, "[ARMRING]", 4))
            {
                if (playObject.UseItems[ItemLocation.ArmRingl].Index > 0)
                {
                    result = playObject.UseItems[ItemLocation.ArmRingl];
                }
                if (playObject.UseItems[ItemLocation.ArmRingr].Index > 0)
                {
                    result = playObject.UseItems[ItemLocation.ArmRingr];
                }
                return result;
            }
            if (HUtil32.CompareLStr(sItemType, "[WEAPON]", 4))
            {
                if (playObject.UseItems[ItemLocation.Weapon].Index > 0)
                {
                    result = playObject.UseItems[ItemLocation.Weapon];
                }
                return result;
            }
            if (HUtil32.CompareLStr(sItemType, "[HELMET]", 4))
            {
                if (playObject.UseItems[ItemLocation.Helmet].Index > 0)
                {
                    result = playObject.UseItems[ItemLocation.Helmet];
                }
                return result;
            }
            if (HUtil32.CompareLStr(sItemType, "[BUJUK]", 4))
            {
                if (playObject.UseItems[ItemLocation.Bujuk].Index > 0)
                {
                    result = playObject.UseItems[ItemLocation.Bujuk];
                }
                return result;
            }
            if (HUtil32.CompareLStr(sItemType, "[BELT]", 4))
            {
                if (playObject.UseItems[ItemLocation.Belt].Index > 0)
                {
                    result = playObject.UseItems[ItemLocation.Belt];
                }
                return result;
            }
            if (HUtil32.CompareLStr(sItemType, "[BOOTS]", 4))
            {
                if (playObject.UseItems[ItemLocation.Boots].Index > 0)
                {
                    result = playObject.UseItems[ItemLocation.Boots];
                }
                return result;
            }
            if (HUtil32.CompareLStr(sItemType, "[CHARM]", 4))
            {
                if (playObject.UseItems[ItemLocation.Charm].Index > 0)
                {
                    result = playObject.UseItems[ItemLocation.Charm];
                }
                return result;
            }
            result = playObject.CheckItemCount(sItemType, ref nCount);
            if (nCount < nParam)
            {
                result = null;
            }
            return result;
        }

        private static bool CheckGotoLableStringList(string sHumName, string sListFileName)
        {
            bool result = false;
            sListFileName = M2Share.GetEnvirFilePath(sListFileName);
            if (File.Exists(sListFileName))
            {
                using StringList loadList = new StringList();
                try
                {
                    loadList.LoadFromFile(sListFileName);
                }
                catch
                {
                    M2Share.Logger.Error("loading fail.... => " + sListFileName);
                }
                for (int i = 0; i < loadList.Count; i++)
                {
                    if (string.Compare(loadList[i].Trim(), sHumName, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        result = true;
                        break;
                    }
                }
            }
            else
            {
                M2Share.Logger.Error("file not found => " + sListFileName);
            }
            return result;
        }

        private static void GotoLableQuestCheckConditionSetVal(PlayObject playObject, string sIndex, int nCount)
        {
            int n14 = M2Share.GetValNameNo(sIndex);
            if (n14 >= 0)
            {
                if (HUtil32.RangeInDefined(n14, 0, 99))
                {
                    playObject.MNVal[n14] = nCount;
                }
                else if (HUtil32.RangeInDefined(n14, 100, 119))
                {
                    M2Share.Config.GlobalVal[n14 - 100] = nCount;
                }
                else if (HUtil32.RangeInDefined(n14, 200, 299))
                {
                    playObject.MDyVal[n14 - 200] = nCount;
                }
                else if (HUtil32.RangeInDefined(n14, 300, 399))
                {
                    playObject.MNMval[n14 - 300] = nCount;
                }
                else if (HUtil32.RangeInDefined(n14, 400, 499))
                {
                    M2Share.Config.GlobaDyMval[n14 - 400] = (short)nCount;
                }
                else if (HUtil32.RangeInDefined(n14, 500, 599))
                {
                    playObject.MNSval[n14 - 600] = nCount.ToString();
                }
            }
        }

        private static bool GotoLable_QuestCheckCondition_CheckDieMon(PlayObject playObject, string monName)
        {
            bool result = string.IsNullOrEmpty(monName);
            if ((playObject.LastHiter != null) && (playObject.LastHiter.ChrName == monName))
            {
                result = true;
            }
            return result;
        }

        private static bool GotoLable_QuestCheckCondition_CheckKillMon(PlayObject playObject, string monName)
        {
            bool result = string.IsNullOrEmpty(monName);
            if ((playObject.TargetCret != null) && (playObject.TargetCret.ChrName == monName))
            {
                result = true;
            }
            return result;
        }

        public static bool GotoLable_QuestCheckCondition_CheckRandomNo(PlayObject playObject, string sNumber)
        {
            return playObject.RandomNo == sNumber;
        }

        private bool QuestCheckConditionCheckUserDateType(PlayObject playObject, string chrName, string sListFileName, string sDay, string param1, string param2)
        {
            string name = string.Empty;
            bool result = false;
            sListFileName = M2Share.GetEnvirFilePath(sListFileName);
            using StringList loadList = new StringList();
            if (File.Exists(sListFileName))
            {
                try
                {
                    loadList.LoadFromFile(sListFileName);
                }
                catch
                {
                    M2Share.Logger.Error("loading fail.... => " + sListFileName);
                }
            }
            int nDay = HUtil32.StrToInt(sDay, 0);
            for (int i = 0; i < loadList.Count; i++)
            {
                string sText = loadList[i].Trim();
                sText = HUtil32.GetValidStrCap(sText, ref name, new[] { ' ', '\t' });
                name = name.Trim();
                if (chrName == name)
                {
                    string ssDay = sText.Trim();
                    DateTime nnday = HUtil32.StrToDate(ssDay);
                    int useDay = HUtil32.Round(DateTime.Today.ToOADate() - nnday.ToOADate());
                    int lastDay = nDay - useDay;
                    if (lastDay < 0)
                    {
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

        private bool GotoLableQuestCheckCondition(PlayObject playObject, IList<QuestConditionInfo> conditionList, ref string sC, ref UserItem userItem)
        {
            bool result = true;
            int n1C = 0;
            int nMaxDura = 0;
            int nDura = 0;
            for (int i = 0; i < conditionList.Count; i++)
            {
                var questConditionInfo = conditionList[i];
                if (ConditionScript.IsRegister(questConditionInfo.CmdCode))
                {
                    ConditionScript.Execute(playObject, questConditionInfo, ref result);
                    return result;
                } 
                M2Share.Logger.Info($"[QuestCheckCondition] 未处理命令:[{questConditionInfo.CmdCode}]");

                if (!string.IsNullOrEmpty(questConditionInfo.sParam1))
                {
                    if (questConditionInfo.sParam1[0] == '$')
                    {
                        string s50 = questConditionInfo.sParam1;
                        questConditionInfo.sParam1 = '<' + questConditionInfo.sParam1 + '>';
                        GetVariableText(playObject, s50, ref questConditionInfo.sParam1);
                    }
                    else if (questConditionInfo.sParam1.IndexOf(">", StringComparison.OrdinalIgnoreCase) > -1)
                    {
                        questConditionInfo.sParam1 = GetLineVariableText(playObject, questConditionInfo.sParam1);
                    }
                }
                if (!string.IsNullOrEmpty(questConditionInfo.sParam2))
                {
                    if (questConditionInfo.sParam2[0] == '$')
                    {
                        string s50 = questConditionInfo.sParam2;
                        questConditionInfo.sParam2 = '<' + questConditionInfo.sParam2 + '>';
                        GetVariableText(playObject, s50, ref questConditionInfo.sParam2);
                    }
                    else if (questConditionInfo.sParam2.IndexOf(">", StringComparison.OrdinalIgnoreCase) > -1)
                    {
                        questConditionInfo.sParam2 = GetLineVariableText(playObject, questConditionInfo.sParam2);
                    }
                }
                if (!string.IsNullOrEmpty(questConditionInfo.sParam3))
                {
                    if (questConditionInfo.sParam3[0] == '$')
                    {
                        string s50 = questConditionInfo.sParam3;
                        questConditionInfo.sParam3 = '<' + questConditionInfo.sParam3 + '>';
                        GetVariableText(playObject, s50, ref questConditionInfo.sParam3);
                    }
                    else if (questConditionInfo.sParam3.IndexOf(">", StringComparison.OrdinalIgnoreCase) > -1)
                    {
                        questConditionInfo.sParam3 = GetLineVariableText(playObject, questConditionInfo.sParam3);
                    }
                }
                if (!string.IsNullOrEmpty(questConditionInfo.sParam4))
                {
                    if (questConditionInfo.sParam4[0] == '$')
                    {
                        string s50 = questConditionInfo.sParam4;
                        questConditionInfo.sParam4 = '<' + questConditionInfo.sParam4 + '>';
                        GetVariableText(playObject, s50, ref questConditionInfo.sParam4);
                    }
                    else if (questConditionInfo.sParam4.IndexOf(">", StringComparison.OrdinalIgnoreCase) > -1)
                    {
                        questConditionInfo.sParam4 = GetLineVariableText(playObject, questConditionInfo.sParam4);
                    }
                }
                if (!string.IsNullOrEmpty(questConditionInfo.sParam5))
                {
                    if (questConditionInfo.sParam5[0] == '$')
                    {
                        string s50 = questConditionInfo.sParam5;
                        questConditionInfo.sParam5 = '<' + questConditionInfo.sParam5 + '>';
                        GetVariableText(playObject, s50, ref questConditionInfo.sParam5);
                    }
                    else if (questConditionInfo.sParam5.IndexOf(">", StringComparison.OrdinalIgnoreCase) > -1)
                    {
                        questConditionInfo.sParam5 = GetLineVariableText(playObject, questConditionInfo.sParam5);
                    }
                }
                if (!string.IsNullOrEmpty(questConditionInfo.sParam6))
                {
                    if (questConditionInfo.sParam6[0] == '$')
                    {
                        string s50 = questConditionInfo.sParam6;
                        questConditionInfo.sParam6 = '<' + questConditionInfo.sParam6 + '>';
                        GetVariableText(playObject, s50, ref questConditionInfo.sParam6);
                    }
                    else if (questConditionInfo.sParam6.IndexOf(">", StringComparison.OrdinalIgnoreCase) > -1)
                    {
                        questConditionInfo.sParam6 = GetLineVariableText(playObject, questConditionInfo.sParam6);
                    }
                }

                //参数变量解释以主执行人物为依据
                if (!string.IsNullOrEmpty(questConditionInfo.sOpName))
                {
                    if (questConditionInfo.sOpName.Length > 2)
                    {
                        if (questConditionInfo.sOpName[1] == '$')
                        {
                            string s50 = questConditionInfo.sOpName;
                            questConditionInfo.sOpName = '<' + questConditionInfo.sOpName + '>';
                            GetVariableText(playObject, s50, ref questConditionInfo.sOpName);
                        }
                        else if (questConditionInfo.sOpName.IndexOf(">", StringComparison.OrdinalIgnoreCase) > -1)
                        {
                            questConditionInfo.sOpName = GetLineVariableText(playObject, questConditionInfo.sOpName);
                        }
                    }
                    PlayObject human = M2Share.WorldEngine.GetPlayObject(questConditionInfo.sOpName);
                    if (human != null)
                    {
                        playObject = human;
                        if (!string.IsNullOrEmpty(questConditionInfo.sOpHName) && string.Compare(questConditionInfo.sOpHName, "H", StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            
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

                switch (questConditionInfo.CmdCode)
                {
                    case (int)ExecutionCode.CHECKUSERDATE:
                        result = QuestCheckConditionCheckUserDateType(playObject, playObject.ChrName, m_sPath + questConditionInfo.sParam1, questConditionInfo.sParam3, questConditionInfo.sParam4, questConditionInfo.sParam5);
                        break;
                    case (int)ConditionCode.CHECKRANDOMNO:
                        M2Share.Logger.Error("TODO nSC_CHECKRANDOMNO...");
                        //result = GotoLable_QuestCheckCondition_CheckRandomNo(PlayObject, sMsg);
                        break;
                    case (int)ConditionCode.CHECKDIEMON:
                        result = GotoLable_QuestCheckCondition_CheckDieMon(playObject, questConditionInfo.sParam1);
                        break;
                    case (int)ConditionCode.CHECKKILLPLAYMON:
                        result = GotoLable_QuestCheckCondition_CheckKillMon(playObject, questConditionInfo.sParam1);
                        break;
                    case (int)ConditionCode.CHECKITEMW:
                        userItem = CheckGotoLableItemW(playObject, questConditionInfo.sParam1, questConditionInfo.nParam2);
                        if (userItem == null)
                        {
                            result = false;
                        }
                        break;
                    case (int)ConditionCode.ISTAKEITEM:
                        if (sC != questConditionInfo.sParam1)
                        {
                            result = false;
                        }
                        break;
                    case (int)ConditionCode.CHECKDURAEVA:
                        userItem = playObject.QuestCheckItem(questConditionInfo.sParam1, ref n1C, ref nMaxDura, ref nDura);
                        if (n1C > 0)
                        {
                            if (HUtil32.Round(nMaxDura / n1C / 1000.0) < questConditionInfo.nParam2)
                            {
                                result = false;
                            }
                        }
                        else
                        {
                            result = false;
                        }
                        break;
                    case (int)ConditionCode.KILLBYHUM:
                        if ((playObject.LastHiter != null) && (playObject.LastHiter.Race != ActorRace.Play))
                        {
                            result = false;
                        }
                        break;
                    case (int)ConditionCode.KILLBYMON:
                        if ((playObject.LastHiter != null) && (playObject.LastHiter.Race == ActorRace.Play))
                        {
                            result = false;
                        }
                        break;
                    case (int)ConditionCode.CHECKINSAFEZONE:
                        if (!playObject.InSafeZone())
                        {
                            result = false;
                        }
                        break;
                    case (int)ConditionCode.SCHECKDEATHPLAYMON:
                        string s01 = string.Empty;
                        if (!GetValValue(playObject, questConditionInfo.sParam1, ref s01))
                        {
                            s01 = GetLineVariableText(playObject, questConditionInfo.sParam1);
                        }
                        result = CheckKillMon2(playObject, s01);
                        break;
                }
                if (!result)
                {
                    break;
                }
            }
            return result;
        }

        private static bool CheckKillMon2(PlayObject playObject, string sMonName)
        {
            return true;
        }

        private bool JmpToLable(PlayObject playObject, string sLabel)
        {
            playObject.ScriptGotoCount++;
            if (playObject.ScriptGotoCount > M2Share.Config.ScriptGotoCountLimit)
            {
                return false;
            }
            GotoLable(playObject, sLabel, false);
            return true;
        }

        private void GoToQuest(PlayObject playObject, int nQuest)
        {
            for (int i = 0; i < m_ScriptList.Count; i++)
            {
                ScriptInfo script = m_ScriptList[i];
                if (script.QuestCount == nQuest)
                {
                    playObject.MScript = script;
                    playObject.LastNpc = this.ActorId;
                    GotoLable(playObject, ScriptConst.sMAIN, false);
                    break;
                }
            }
        }

        private static void GotoLableAddUseDateList(string sHumName, string sListFileName)
        {
            string s10 = string.Empty;
            string sText;
            bool bo15;
            sListFileName = M2Share.GetEnvirFilePath(sListFileName);
            using StringList loadList = new StringList();
            if (File.Exists(sListFileName))
            {
                loadList.LoadFromFile(sListFileName);
            }
            bo15 = false;
            for (int i = 0; i < loadList.Count; i++)
            {
                sText = loadList[i].Trim();
                sText = HUtil32.GetValidStrCap(sText, ref s10, new[] { ' ', '\t' });
                if (string.Compare(sHumName, s10, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    bo15 = true;
                    break;
                }
            }
            if (!bo15)
            {
                s10 = $"{sHumName}    {DateTime.Today}";
                loadList.Add(s10);
                try
                {
                    loadList.SaveToFile(sListFileName);
                }
                catch
                {
                    M2Share.Logger.Error("saving fail.... => " + sListFileName);
                }
            }
        }

        private static void GotoLableAddList(string sHumName, string sListFileName)
        {
            sListFileName = M2Share.GetEnvirFilePath(sListFileName);
            using StringList loadList = new StringList();
            if (File.Exists(sListFileName))
            {
                loadList.LoadFromFile(sListFileName);
            }
            bool bo15 = false;
            for (int i = 0; i < loadList.Count; i++)
            {
                string s10 = loadList[i].Trim();
                if (string.Compare(sHumName, s10, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    bo15 = true;
                    break;
                }
            }
            if (!bo15)
            {
                loadList.Add(sHumName);
                try
                {
                    loadList.SaveToFile(sListFileName);
                }
                catch
                {
                    M2Share.Logger.Error("saving fail.... => " + sListFileName);
                }
            }
        }

        private static void GotoLableDelUseDateList(string sHumName, string sListFileName)
        {
            string s10 = string.Empty;
            string sText;
            sListFileName = M2Share.GetEnvirFilePath(sListFileName);
            using StringList loadList = new StringList();
            if (File.Exists(sListFileName))
            {
                loadList.LoadFromFile(sListFileName);
            }
            bool bo15 = false;
            for (int i = 0; i < loadList.Count; i++)
            {
                sText = loadList[i].Trim();
                sText = HUtil32.GetValidStrCap(sText, ref s10, new[] { ' ', '\t' });
                if (string.Compare(sHumName, s10, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    bo15 = true;
                    loadList.RemoveAt(i);
                    break;
                }
            }
            if (bo15)
            {
                loadList.SaveToFile(sListFileName);
            }
        }

        private static void GotoLableDelList(string playName, string sListFileName)
        {
            bool bo15;
            sListFileName = M2Share.GetEnvirFilePath(sListFileName);
            using StringList loadList = new StringList();
            if (File.Exists(sListFileName))
            {
                loadList.LoadFromFile(sListFileName);
            }
            bo15 = false;
            for (int i = 0; i < loadList.Count; i++)
            {
                string s10 = loadList[i].Trim();
                if (string.Compare(playName, s10, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    loadList.RemoveAt(i);
                    bo15 = true;
                    break;
                }
            }
            if (bo15)
            {
                loadList.SaveToFile(sListFileName);
            }
        }

        private void GotoLableTakeItem(PlayObject playObject, string sItemName, int nItemCount, ref string sC)
        {
            UserItem userItem;
            StdItem stdItem;
            if (string.Compare(sItemName, Grobal2.StringGoldName, StringComparison.OrdinalIgnoreCase) == 0)
            {
                playObject.DecGold(nItemCount);
                playObject.GoldChanged();
                if (M2Share.GameLogGold)
                {
                    M2Share.EventSource.AddEventLog(10, playObject.MapName + "\t" + playObject.CurrX + "\t" + playObject.CurrY + "\t" + playObject.ChrName + "\t" + Grobal2.StringGoldName + "\t" + nItemCount + "\t" + '1' + "\t" + ChrName);
                }
                return;
            }
            for (int i = playObject.ItemList.Count - 1; i >= 0; i--)
            {
                if (nItemCount <= 0)
                {
                    break;
                }
                userItem = playObject.ItemList[i];
                if (string.Compare(M2Share.WorldEngine.GetStdItemName(userItem.Index), sItemName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    stdItem = M2Share.WorldEngine.GetStdItem(userItem.Index);
                    if (stdItem.NeedIdentify == 1)
                    {
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

        public void GotoLableGiveItem(PlayObject playObject, string sItemName, int nItemCount)
        {
            UserItem userItem;
            StdItem stdItem;
            if (string.Compare(sItemName, Grobal2.StringGoldName, StringComparison.OrdinalIgnoreCase) == 0)
            {
                playObject.IncGold(nItemCount);
                playObject.GoldChanged();
                if (M2Share.GameLogGold)
                {
                    M2Share.EventSource.AddEventLog(9, playObject.MapName + "\t" + playObject.CurrX + "\t" + playObject.CurrY + "\t" + playObject.ChrName + "\t" + Grobal2.StringGoldName + "\t" + nItemCount + "\t" + '1' + "\t" + ChrName);
                }
                return;
            }
            if (M2Share.WorldEngine.GetStdItemIdx(sItemName) > 0)
            {
                if (!(nItemCount >= 1 && nItemCount <= 50))
                {
                    nItemCount = 1;
                }
                for (int i = 0; i < nItemCount; i++)
                {
                    if (playObject.IsEnoughBag())
                    {
                        userItem = new UserItem();
                        if (M2Share.WorldEngine.CopyToUserItemFromName(sItemName, ref userItem))
                        {
                            playObject.ItemList.Add(userItem);
                            playObject.SendAddItem(userItem);
                            stdItem = M2Share.WorldEngine.GetStdItem(userItem.Index);
                            if (stdItem.NeedIdentify == 1)
                            {
                                M2Share.EventSource.AddEventLog(9, playObject.MapName + "\t" + playObject.CurrX + "\t" + playObject.CurrY + "\t" + playObject.ChrName + "\t" + sItemName + "\t" + userItem.MakeIndex + "\t" + '1' + "\t" + ChrName);
                            }
                        }
                        else
                        {
                            Dispose(userItem);
                        }
                    }
                    else
                    {
                        userItem = new UserItem();
                        if (M2Share.WorldEngine.CopyToUserItemFromName(sItemName, ref userItem))
                        {
                            stdItem = M2Share.WorldEngine.GetStdItem(userItem.Index);
                            if (stdItem.NeedIdentify == 1)
                            {
                                M2Share.EventSource.AddEventLog(9, playObject.MapName + "\t" + playObject.CurrX + "\t" + playObject.CurrY + "\t" + playObject.ChrName + "\t" + sItemName + "\t" + userItem.MakeIndex + "\t" + '1' + "\t" + ChrName);
                            }
                            playObject.DropItemDown(userItem, 3, false, playObject.ActorId, 0);
                        }
                        Dispose(userItem);
                    }
                }
            }
        }

        private static void GotoLableTakeWItem(PlayObject playObject, string sItemName, int nItemCount)
        {
            string sC;
            if (HUtil32.CompareLStr(sItemName, "[NECKLACE]", 4))
            {
                if (playObject.UseItems[ItemLocation.Necklace].Index > 0)
                {
                    playObject.SendDelItems(playObject.UseItems[ItemLocation.Necklace]);
                    sC = M2Share.WorldEngine.GetStdItemName(playObject.UseItems[ItemLocation.Necklace].Index);
                    playObject.UseItems[ItemLocation.Necklace].Index = 0;
                    return;
                }
            }
            if (HUtil32.CompareLStr(sItemName, "[RING]", 4))
            {
                if (playObject.UseItems[ItemLocation.Ringl].Index > 0)
                {
                    playObject.SendDelItems(playObject.UseItems[ItemLocation.Ringl]);
                    sC = M2Share.WorldEngine.GetStdItemName(playObject.UseItems[ItemLocation.Ringl].Index);
                    playObject.UseItems[ItemLocation.Ringl].Index = 0;
                    return;
                }
                if (playObject.UseItems[ItemLocation.Ringr].Index > 0)
                {
                    playObject.SendDelItems(playObject.UseItems[ItemLocation.Ringr]);
                    sC = M2Share.WorldEngine.GetStdItemName(playObject.UseItems[ItemLocation.Ringr].Index);
                    playObject.UseItems[ItemLocation.Ringr].Index = 0;
                    return;
                }
            }
            if (HUtil32.CompareLStr(sItemName, "[ARMRING]", 4))
            {
                if (playObject.UseItems[ItemLocation.ArmRingl].Index > 0)
                {
                    playObject.SendDelItems(playObject.UseItems[ItemLocation.ArmRingl]);
                    sC = M2Share.WorldEngine.GetStdItemName(playObject.UseItems[ItemLocation.ArmRingl].Index);
                    playObject.UseItems[ItemLocation.ArmRingl].Index = 0;
                    return;
                }
                if (playObject.UseItems[ItemLocation.ArmRingr].Index > 0)
                {
                    playObject.SendDelItems(playObject.UseItems[ItemLocation.ArmRingr]);
                    sC = M2Share.WorldEngine.GetStdItemName(playObject.UseItems[ItemLocation.ArmRingr].Index);
                    playObject.UseItems[ItemLocation.ArmRingr].Index = 0;
                    return;
                }
            }
            if (HUtil32.CompareLStr(sItemName, "[WEAPON]", 4))
            {
                if (playObject.UseItems[ItemLocation.Weapon].Index > 0)
                {
                    playObject.SendDelItems(playObject.UseItems[ItemLocation.Weapon]);
                    sC = M2Share.WorldEngine.GetStdItemName(playObject.UseItems[ItemLocation.Weapon].Index);
                    playObject.UseItems[ItemLocation.Weapon].Index = 0;
                    return;
                }
            }
            if (HUtil32.CompareLStr(sItemName, "[HELMET]", 4))
            {
                if (playObject.UseItems[ItemLocation.Helmet].Index > 0)
                {
                    playObject.SendDelItems(playObject.UseItems[ItemLocation.Helmet]);
                    sC = M2Share.WorldEngine.GetStdItemName(playObject.UseItems[ItemLocation.Helmet].Index);
                    playObject.UseItems[ItemLocation.Helmet].Index = 0;
                    return;
                }
            }
            if (HUtil32.CompareLStr(sItemName, "[DRESS]", 4))
            {
                if (playObject.UseItems[ItemLocation.Dress].Index > 0)
                {
                    playObject.SendDelItems(playObject.UseItems[ItemLocation.Dress]);
                    sC = M2Share.WorldEngine.GetStdItemName(playObject.UseItems[ItemLocation.Dress].Index);
                    playObject.UseItems[ItemLocation.Dress].Index = 0;
                    return;
                }
            }
            if (HUtil32.CompareLStr(sItemName, "[U_BUJUK]", 4))
            {
                if (playObject.UseItems[ItemLocation.Bujuk].Index > 0)
                {
                    playObject.SendDelItems(playObject.UseItems[ItemLocation.Bujuk]);
                    sC = M2Share.WorldEngine.GetStdItemName(playObject.UseItems[ItemLocation.Bujuk].Index);
                    playObject.UseItems[ItemLocation.Bujuk].Index = 0;
                    return;
                }
            }
            if (HUtil32.CompareLStr(sItemName, "[U_BELT]", 4))
            {
                if (playObject.UseItems[ItemLocation.Belt].Index > 0)
                {
                    playObject.SendDelItems(playObject.UseItems[ItemLocation.Belt]);
                    sC = M2Share.WorldEngine.GetStdItemName(playObject.UseItems[ItemLocation.Belt].Index);
                    playObject.UseItems[ItemLocation.Belt].Index = 0;
                    return;
                }
            }
            if (HUtil32.CompareLStr(sItemName, "[U_BOOTS]", 4))
            {
                if (playObject.UseItems[ItemLocation.Boots].Index > 0)
                {
                    playObject.SendDelItems(playObject.UseItems[ItemLocation.Boots]);
                    sC = M2Share.WorldEngine.GetStdItemName(playObject.UseItems[ItemLocation.Boots].Index);
                    playObject.UseItems[ItemLocation.Boots].Index = 0;
                    return;
                }
            }
            if (HUtil32.CompareLStr(sItemName, "[U_CHARM]", 4))
            {
                if (playObject.UseItems[ItemLocation.Charm].Index > 0)
                {
                    playObject.SendDelItems(playObject.UseItems[ItemLocation.Charm]);
                    sC = M2Share.WorldEngine.GetStdItemName(playObject.UseItems[ItemLocation.Charm].Index);
                    playObject.UseItems[ItemLocation.Charm].Index = 0;
                    return;
                }
            }
            for (int i = 0; i < playObject.UseItems.Length; i++)
            {
                if (nItemCount <= 0)
                {
                    return;
                }
                if (playObject.UseItems[i].Index > 0)
                {
                    string sName = M2Share.WorldEngine.GetStdItemName(playObject.UseItems[i].Index);
                    if (string.Compare(sName, sItemName, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        playObject.SendDelItems(playObject.UseItems[i]);
                        playObject.UseItems[i].Index = 0;
                        nItemCount -= 1;
                    }
                }
            }
        }

        private bool GotoLableQuestActionProcess(PlayObject playObject, IList<QuestActionInfo> actionList, ref string sC, ref UserItem userItem, ref bool bo11)
        {
            bool result = true;
            string s44 = string.Empty;
            int n18 = 0;
            int n34 = 0;
            int n38 = 0;
            int n3C = 0;
            int n40 = 0;
            for (int i = 0; i < actionList.Count; i++)
            {
                QuestActionInfo questActionInfo = actionList[i];
                ExecutionCode executionCode = (ExecutionCode)questActionInfo.nCmdCode;
                switch (executionCode)
                {
                    case ExecutionCode.SET:
                        var n28 = HUtil32.StrToInt(questActionInfo.sParam1, 0);
                        var n2C = HUtil32.StrToInt(questActionInfo.sParam2, 0);
                        playObject.SetQuestFlagStatus(n28, n2C);
                        break;
                    case ExecutionCode.TAKE:
                        GotoLableTakeItem(playObject, questActionInfo.sParam1, questActionInfo.nParam2, ref sC);
                        break;
                    case ExecutionCode.GIVE:
                        ActionOfGiveItem(playObject, questActionInfo);
                        break;
                    case ExecutionCode.TAKEW:
                        GotoLableTakeWItem(playObject, questActionInfo.sParam1, questActionInfo.nParam2);
                        break;
                    case ExecutionCode.CLOSE:
                        playObject.SendMsg(this, Messages.RM_MERCHANTDLGCLOSE, 0, ActorId, 0, 0);
                        break;
                    case ExecutionCode.RESET:
                        for (int k = 0; k < questActionInfo.nParam2; k++)
                        {
                            playObject.SetQuestFlagStatus(questActionInfo.nParam1 + k, 0);
                        }
                        break;
                    case ExecutionCode.SETOPEN:
                        n28 = HUtil32.StrToInt(questActionInfo.sParam1, 0);
                        n2C = HUtil32.StrToInt(questActionInfo.sParam2, 0);
                        playObject.SetQuestUnitOpenStatus(n28, n2C);
                        break;
                    case ExecutionCode.SETUNIT:
                        n28 = HUtil32.StrToInt(questActionInfo.sParam1, 0);
                        n2C = HUtil32.StrToInt(questActionInfo.sParam2, 0);
                        playObject.SetQuestUnitStatus(n28, n2C);
                        break;
                    case ExecutionCode.RESETUNIT:
                        for (int k = 0; k < questActionInfo.nParam2; k++)
                        {
                            playObject.SetQuestUnitStatus(questActionInfo.nParam1 + k, 0);
                        }
                        break;
                    case ExecutionCode.BREAK:
                        result = false;
                        break;
                    case ExecutionCode.TIMERECALL:
                        playObject.IsTimeRecall = true;
                        playObject.TimeRecallMoveMap = playObject.MapName;
                        playObject.TimeRecallMoveX = playObject.CurrX;
                        playObject.TimeRecallMoveY = playObject.CurrY;
                        playObject.TimeRecallTick = HUtil32.GetTickCount() + (questActionInfo.nParam1 * 60 * 1000);
                        break;
                    case ExecutionCode.PARAM1:
                        n34 = questActionInfo.nParam1;
                        s44 = questActionInfo.sParam1;
                        break;
                    case ExecutionCode.PARAM2:
                        n38 = questActionInfo.nParam1;
                        string s48 = questActionInfo.sParam1;
                        break;
                    case ExecutionCode.PARAM3:
                        n3C = questActionInfo.nParam1;
                        string s4C = questActionInfo.sParam1;
                        break;
                    case ExecutionCode.PARAM4:
                        n40 = questActionInfo.nParam1;
                        break;
                    case ExecutionCode.EXEACTION:
                        n40 = questActionInfo.nParam1;
                        ExeAction(playObject, questActionInfo.sParam1, questActionInfo.sParam2, questActionInfo.sParam3, questActionInfo.nParam1, questActionInfo.nParam2, questActionInfo.nParam3);
                        break;
                    case ExecutionCode.MAPMOVE:
                        playObject.SendRefMsg(Messages.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
                        playObject.SpaceMove(questActionInfo.sParam1, (short)questActionInfo.nParam2, (short)questActionInfo.nParam3, 0);
                        bo11 = true;
                        break;
                    case ExecutionCode.MAP:
                        playObject.SendRefMsg(Messages.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
                        playObject.MapRandomMove(questActionInfo.sParam1, 0);
                        bo11 = true;
                        break;
                    case ExecutionCode.TAKECHECKITEM:
                        if (userItem != null)
                        {
                            playObject.QuestTakeCheckItem(userItem);
                        }
                        else
                        {
                            ScriptActionError(playObject, "", questActionInfo, ExecutionCode.TAKECHECKITEM);
                        }
                        break;
                    case ExecutionCode.MONGEN:
                        for (int k = 0; k < questActionInfo.nParam2; k++)
                        {
                            var n20X = M2Share.RandomNumber.Random(questActionInfo.nParam3 * 2 + 1) + (n38 - questActionInfo.nParam3);
                            var n24Y = M2Share.RandomNumber.Random(questActionInfo.nParam3 * 2 + 1) + (n3C - questActionInfo.nParam3);
                            M2Share.WorldEngine.RegenMonsterByName(s44, (short)n20X, (short)n24Y, questActionInfo.sParam1);
                        }
                        break;
                    case ExecutionCode.MONCLEAR:
                        var list58 = new List<BaseObject>();
                        M2Share.WorldEngine.GetMapMonster(M2Share.MapMgr.FindMap(questActionInfo.sParam1), list58);
                        for (int k = 0; k < list58.Count; k++)
                        {
                            list58[k].NoItem = true;
                            list58[k].WAbil.HP = 0;
                            list58[k].MakeGhost();
                        }
                        list58.Clear();
                        break;
                    case ExecutionCode.MOV:
                        MovData(playObject, questActionInfo);
                        break;
                    case ExecutionCode.INC:
                        IncInteger(playObject, questActionInfo);
                        break;
                    case ExecutionCode.DEC:
                        DecInteger(playObject, questActionInfo);
                        break;
                    case ExecutionCode.SUM:
                        SumData(playObject, questActionInfo);
                        break;
                    case ExecutionCode.Div:
                        DivData(playObject, questActionInfo);
                        break;
                    case ExecutionCode.Mul:
                        MulData(playObject, questActionInfo);
                        break;
                    case ExecutionCode.Percent:
                        PercentData(playObject, questActionInfo);
                        break;
                    case ExecutionCode.BREAKTIMERECALL:
                        playObject.IsTimeRecall = false;
                        break;
                    case ExecutionCode.PKPOINT:
                        if (questActionInfo.nParam1 == 0)
                        {
                            playObject.PkPoint = 0;
                        }
                        else
                        {
                            if (questActionInfo.nParam1 < 0)
                            {
                                if ((playObject.PkPoint + questActionInfo.nParam1) >= 0)
                                {
                                    playObject.PkPoint += questActionInfo.nParam1;
                                }
                                else
                                {
                                    playObject.PkPoint = 0;
                                }
                            }
                            else
                            {
                                if ((playObject.PkPoint + questActionInfo.nParam1) > 10000)
                                {
                                    playObject.PkPoint = 10000;
                                }
                                else
                                {
                                    playObject.PkPoint += questActionInfo.nParam1;
                                }
                            }
                        }
                        playObject.RefNameColor();
                        break;
                    case ExecutionCode.CHANGEXP:
                        break;
                    case ExecutionCode.RECALLMOB:
                        ActionOfRecallmob(playObject, questActionInfo);
                        break;
                    case ExecutionCode.THROWITEM://将指定物品刷新到指定地图坐标范围内
                        ActionOfThrowitem(playObject, questActionInfo);
                        break;
                    case ExecutionCode.MOVR:
                        MovrData(playObject, questActionInfo);
                        break;
                    case ExecutionCode.EXCHANGEMAP:
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
                        else
                        {
                            ScriptActionError(playObject, "", questActionInfo, ExecutionCode.EXCHANGEMAP);
                        }
                        break;
                    case ExecutionCode.RECALLMAP:
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
                            ScriptActionError(playObject, "", questActionInfo, ExecutionCode.RECALLMAP);
                        }
                        break;
                    case ExecutionCode.ADDBATCH:
                        if (BatchParamsList == null)
                        {
                            BatchParamsList = new List<ScriptParams>();
                        }
                        BatchParamsList.Add(new ScriptParams()
                        {
                            sParams = questActionInfo.sParam1,
                            nParams = n18
                        });
                        break;
                    case ExecutionCode.BATCHDELAY:
                        n18 = questActionInfo.nParam1 * 1000;
                        break;
                    case ExecutionCode.BATCHMOVE:
                        int n20 = 0;
                        for (int k = 0; k < BatchParamsList.Count; k++)
                        {
                            ScriptParams batchParam = BatchParamsList[k];
                            playObject.SendSelfDelayMsg(Messages.RM_RANDOMSPACEMOVE, 0, 0, 0, 0, BatchParamsList[k].sParams, batchParam.nParams + n20);
                            n20 += batchParam.nParams;
                        }
                        break;
                    case ExecutionCode.PLAYDICE:
                        playObject.PlayDiceLabel = questActionInfo.sParam2;
                        playObject.SendMsg(this, Messages.RM_PLAYDICE, (short)questActionInfo.nParam1, HUtil32.MakeLong(HUtil32.MakeWord((ushort)playObject.MDyVal[0], (ushort)playObject.MDyVal[1]), HUtil32.MakeWord((ushort)playObject.MDyVal[2], (ushort)playObject.MDyVal[3])), HUtil32.MakeLong(HUtil32.MakeWord((ushort)playObject.MDyVal[4], (ushort)playObject.MDyVal[5]), HUtil32.MakeWord((ushort)playObject.MDyVal[6], (ushort)playObject.MDyVal[7])), HUtil32.MakeLong(HUtil32.MakeWord((ushort)playObject.MDyVal[8], (ushort)playObject.MDyVal[9]), 0), questActionInfo.sParam2);
                        bo11 = true;
                        break;
                    case ExecutionCode.ADDNAMELIST:
                        GotoLableAddList(playObject.ChrName, m_sPath + questActionInfo.sParam1);
                        break;
                    case ExecutionCode.DELNAMELIST:
                        GotoLableDelList(playObject.ChrName, m_sPath + questActionInfo.sParam1);
                        break;
                    case ExecutionCode.ADDUSERDATE:
                        GotoLableAddUseDateList(playObject.ChrName, m_sPath + questActionInfo.sParam1);
                        break;
                    case ExecutionCode.DELUSERDATE:
                        GotoLableDelUseDateList(playObject.ChrName, m_sPath + questActionInfo.sParam1);
                        break;
                    case ExecutionCode.ADDGUILDLIST:
                        if (playObject.MyGuild != null)
                        {
                            GotoLableAddList(playObject.MyGuild.GuildName, m_sPath + questActionInfo.sParam1);
                        }
                        break;
                    case ExecutionCode.DELGUILDLIST:
                        if (playObject.MyGuild != null)
                        {
                            GotoLableDelList(playObject.MyGuild.GuildName, m_sPath + questActionInfo.sParam1);
                        }
                        break;
                    case ExecutionCode.LINEMSG:
                    case ExecutionCode.SENDMSG:
                        ActionOfLineMsg(playObject, questActionInfo);
                        break;
                    case ExecutionCode.ADDACCOUNTLIST:
                        GotoLableAddList(playObject.UserAccount, m_sPath + questActionInfo.sParam1);
                        break;
                    case ExecutionCode.DELACCOUNTLIST:
                        GotoLableDelList(playObject.UserAccount, m_sPath + questActionInfo.sParam1);
                        break;
                    case ExecutionCode.ADDIPLIST:
                        GotoLableAddList(playObject.LoginIpAddr, m_sPath + questActionInfo.sParam1);
                        break;
                    case ExecutionCode.DELIPLIST:
                        GotoLableDelList(playObject.LoginIpAddr, m_sPath + questActionInfo.sParam1);
                        break;
                    case ExecutionCode.GOQUEST:
                        GoToQuest(playObject, questActionInfo.nParam1);
                        break;
                    case ExecutionCode.ENDQUEST:
                        playObject.MScript = null;
                        break;
                    case ExecutionCode.GOTO:
                        if (!JmpToLable(playObject, questActionInfo.sParam1))
                        {
                            M2Share.Logger.Error("[脚本死循环] NPC:" + ChrName + " 位置:" + MapName + '(' + CurrX + ':' + CurrY + ')' + " 命令:" + ExecutionCode.GOTO + ' ' + questActionInfo.sParam1);
                            result = false;
                            return result;
                        }
                        break;
                    case ExecutionCode.HAIRCOLOR:
                        break;
                    case ExecutionCode.WEARCOLOR:
                        break;
                    case ExecutionCode.HAIRSTYLE:
                        ActionOfChangeHairStyle(playObject, questActionInfo);
                        break;
                    case ExecutionCode.MONRECALL:
                        break;
                    case ExecutionCode.HORSECALL:
                        break;
                    case ExecutionCode.HAIRRNDCOL:
                        break;
                    case ExecutionCode.KILLHORSE:
                        break;
                    case ExecutionCode.RANDSETDAILYQUEST:
                        break;
                    case ExecutionCode.RECALLGROUPMEMBERS:
                        ActionOfRecallGroupMembers(playObject, questActionInfo);
                        break;
                    case ExecutionCode.CLEARNAMELIST:
                        ActionOfClearList(playObject, questActionInfo);
                        break;
                    case ExecutionCode.MAPTING:
                        ActionOfMapTing(playObject, questActionInfo);
                        break;
                    case ExecutionCode.CHANGELEVEL:
                        ActionOfChangeLevel(playObject, questActionInfo);
                        break;
                    case ExecutionCode.MARRY:
                        ActionOfMarry(playObject, questActionInfo);
                        break;
                    case ExecutionCode.MASTER:
                        ActionOfMaster(playObject, questActionInfo);
                        break;
                    case ExecutionCode.UNMASTER:
                        ActionOfUnMaster(playObject, questActionInfo);
                        break;
                    case ExecutionCode.UNMARRY:
                        ActionOfUnMarry(playObject, questActionInfo);
                        break;
                    case ExecutionCode.GETMARRY:
                        ActionOfGetMarry(playObject, questActionInfo);
                        break;
                    case ExecutionCode.GETMASTER:
                        ActionOfGetMaster(playObject, questActionInfo);
                        break;
                    case ExecutionCode.CLEARSKILL:
                        ActionOfClearSkill(playObject, questActionInfo);
                        break;
                    case ExecutionCode.DELNOJOBSKILL:
                        ActionOfDelNoJobSkill(playObject, questActionInfo);
                        break;
                    case ExecutionCode.DELSKILL:
                        ActionOfDelSkill(playObject, questActionInfo);
                        break;
                    case ExecutionCode.ADDSKILL:
                        ActionOfAddSkill(playObject, questActionInfo);
                        break;
                    case ExecutionCode.SKILLLEVEL:
                        ActionOfSkillLevel(playObject, questActionInfo);
                        break;
                    case ExecutionCode.CHANGEPKPOINT:
                        ActionOfChangePkPoint(playObject, questActionInfo);
                        break;
                    case ExecutionCode.CHANGEEXP:
                        ActionOfChangeExp(playObject, questActionInfo);
                        break;
                    case ExecutionCode.CHANGEJOB:
                        ActionOfChangeJob(playObject, questActionInfo);
                        break;
                    case ExecutionCode.MISSION:
                        ActionOfMission(playObject, questActionInfo);
                        break;
                    case ExecutionCode.MOBPLACE:
                        ActionOfMobPlace(playObject, questActionInfo, n34, n38, n3C, n40);
                        break;
                    case ExecutionCode.SETMEMBERTYPE:
                        ActionOfSetMemberType(playObject, questActionInfo);
                        break;
                    case ExecutionCode.SETMEMBERLEVEL:
                        ActionOfSetMemberLevel(playObject, questActionInfo);
                        break;
                    case ExecutionCode.GAMEGOLD:
                        ActionOfGameGold(playObject, questActionInfo);
                        break;
                    case ExecutionCode.GAMEPOINT:
                        ActionOfGamePoint(playObject, questActionInfo);
                        break;
                    case ExecutionCode.OffLine:
                        ActionOfOffLine(playObject, questActionInfo);
                        break;
                    case ExecutionCode.AUTOADDGAMEGOLD: // 增加挂机
                        ActionOfAutoAddGameGold(playObject, questActionInfo, n34, n38);
                        break;
                    case ExecutionCode.AUTOSUBGAMEGOLD:
                        ActionOfAutoSubGameGold(playObject, questActionInfo, n34, n38);
                        break;
                    case ExecutionCode.CHANGENAMECOLOR:
                        ActionOfChangeNameColor(playObject, questActionInfo);
                        break;
                    case ExecutionCode.CLEARPASSWORD:
                        ActionOfClearPassword(playObject, questActionInfo);
                        break;
                    case ExecutionCode.RENEWLEVEL:
                        ActionOfReNewLevel(playObject, questActionInfo);
                        break;
                    case ExecutionCode.KILLSLAVE:
                        ActionOfKillSlave(playObject, questActionInfo);
                        break;
                    case ExecutionCode.CHANGEGENDER:
                        ActionOfChangeGender(playObject, questActionInfo);
                        break;
                    case ExecutionCode.KILLMONEXPRATE:
                        ActionOfKillMonExpRate(playObject, questActionInfo);
                        break;
                    case ExecutionCode.POWERRATE:
                        ActionOfPowerRate(playObject, questActionInfo);
                        break;
                    case ExecutionCode.CHANGEPERMISSION:
                        ActionOfChangePerMission(playObject, questActionInfo);
                        break;
                    case ExecutionCode.BONUSPOINT:
                        ActionOfBonusPoint(playObject, questActionInfo);
                        break;
                    case ExecutionCode.RESTRENEWLEVEL:
                        ActionOfRestReNewLevel(playObject, questActionInfo);
                        break;
                    case ExecutionCode.DELMARRY:
                        ActionOfDelMarry(playObject, questActionInfo);
                        break;
                    case ExecutionCode.DELMASTER:
                        ActionOfDelMaster(playObject, questActionInfo);
                        break;
                    case ExecutionCode.CREDITPOINT:
                        ActionOfChangeCreditPoint(playObject, questActionInfo);
                        break;
                    case ExecutionCode.CLEARNEEDITEMS:
                        ActionOfClearNeedItems(playObject, questActionInfo);
                        break;
                    case ExecutionCode.CLEARMAKEITEMS:
                        ActionOfClearMakeItems(playObject, questActionInfo);
                        break;
                    case ExecutionCode.SETSENDMSGFLAG:
                        playObject.BoSendMsgFlag = true;
                        break;
                    case ExecutionCode.UPGRADEITEMS:
                        ActionOfUpgradeItems(playObject, questActionInfo);
                        break;
                    case ExecutionCode.UPGRADEITEMSEX:
                        ActionOfUpgradeItemsEx(playObject, questActionInfo);
                        break;
                    case ExecutionCode.MONGENEX:
                        ActionOfMonGenEx(playObject, questActionInfo);
                        break;
                    case ExecutionCode.CLEARMAPMON:
                        ActionOfClearMapMon(playObject, questActionInfo);
                        break;
                    case ExecutionCode.SETMAPMODE:
                        ActionOfSetMapMode(playObject, questActionInfo);
                        break;
                    case ExecutionCode.PKZONE:
                        ActionOfPkZone(playObject, questActionInfo);
                        break;
                    case ExecutionCode.RESTBONUSPOINT:
                        ActionOfRestBonusPoint(playObject, questActionInfo);
                        break;
                    case ExecutionCode.TAKECASTLEGOLD:
                        ActionOfTakeCastleGold(playObject, questActionInfo);
                        break;
                    case ExecutionCode.HUMANHP:
                        ActionOfHumanHp(playObject, questActionInfo);
                        break;
                    case ExecutionCode.HUMANMP:
                        ActionOfHumanMp(playObject, questActionInfo);
                        break;
                    case ExecutionCode.BUILDPOINT:
                        ActionOfGuildBuildPoint(playObject, questActionInfo);
                        break;
                    case ExecutionCode.AURAEPOINT:
                        ActionOfGuildAuraePoint(playObject, questActionInfo);
                        break;
                    case ExecutionCode.STABILITYPOINT:
                        ActionOfGuildstabilityPoint(playObject, questActionInfo);
                        break;
                    case ExecutionCode.FLOURISHPOINT:
                        ActionOfGuildFlourishPoint(playObject, questActionInfo);
                        break;
                    case ExecutionCode.OPENMAGICBOX:
                        ActionOfOpenMagicBox(playObject, questActionInfo);
                        break;
                    case ExecutionCode.SETRANKLEVELNAME:
                        ActionOfSetRankLevelName(playObject, questActionInfo);
                        break;
                    case ExecutionCode.GMEXECUTE:
                        ActionOfGmExecute(playObject, questActionInfo);
                        break;
                    case ExecutionCode.GUILDCHIEFITEMCOUNT:
                        ActionOfGuildChiefItemCount(playObject, questActionInfo);
                        break;
                    case ExecutionCode.ADDNAMEDATELIST:
                        ActionOfAddNameDateList(playObject, questActionInfo);
                        break;
                    case ExecutionCode.DELNAMEDATELIST:
                        ActionOfDelNameDateList(playObject, questActionInfo);
                        break;
                    case ExecutionCode.MOBFIREBURN:
                        ActionOfMobFireBurn(playObject, questActionInfo);
                        break;
                    case ExecutionCode.MESSAGEBOX:
                        ActionOfMessageBox(playObject, questActionInfo);
                        break;
                    case ExecutionCode.SETSCRIPTFLAG:
                        ActionOfSetScriptFlag(playObject, questActionInfo);
                        break;
                    case ExecutionCode.SETAUTOGETEXP:
                        ActionOfAutoGetExp(playObject, questActionInfo);
                        break;
                    case ExecutionCode.VAR:
                        ActionOfVar(playObject, questActionInfo);
                        break;
                    case ExecutionCode.LOADVAR:
                        ActionOfLoadVar(playObject, questActionInfo);
                        break;
                    case ExecutionCode.SAVEVAR:
                        ActionOfSaveVar(playObject, questActionInfo);
                        break;
                    case ExecutionCode.CALCVAR:
                        ActionOfCalcVar(playObject, questActionInfo);
                        break;
                    case ExecutionCode.GUILDRECALL:
                        ActionOfGuildRecall(playObject, questActionInfo);
                        break;
                    case ExecutionCode.GROUPADDLIST:
                        ActionOfGroupAddList(playObject, questActionInfo);
                        break;
                    case ExecutionCode.CLEARLIST:
                        ActionOfClearList(playObject, questActionInfo);
                        break;
                    case ExecutionCode.GROUPRECALL:
                        ActionOfGroupRecall(playObject, questActionInfo);
                        break;
                    case ExecutionCode.GROUPMOVEMAP:
                        ActionOfGroupMoveMap(playObject, questActionInfo);
                        break;
                    case ExecutionCode.REPAIRALL:
                        ActionOfRepairAllItem(playObject, questActionInfo);
                        break;
                    case ExecutionCode.QUERYBAGITEMS:// 刷新包裹
                        if ((HUtil32.GetTickCount() - playObject.QueryBagItemsTick) > M2Share.Config.QueryBagItemsTick)
                        {
                            playObject.QueryBagItemsTick = HUtil32.GetTickCount();
                            playObject.ClientQueryBagItems();
                        }
                        else
                        {
                            playObject.SysMsg(Settings.QUERYBAGITEMS, MsgColor.Red, MsgType.Hint);
                        }
                        break;
                    case ExecutionCode.SETRANDOMNO:
                        while (true)
                        {
                            n2C = M2Share.RandomNumber.Random(999999);
                            if ((n2C >= 1000) && (n2C.ToString() != playObject.RandomNo))
                            {
                                playObject.RandomNo = n2C.ToString();
                                break;
                            }
                        }
                        break;
                    case ExecutionCode.OPENYBDEAL:
                        ActionOfOpenSaleDeal(playObject, questActionInfo);
                        break;
                    case ExecutionCode.QUERYYBSELL:
                        ActionOfQuerySaleSell(playObject, questActionInfo);
                        break;
                    case ExecutionCode.QUERYYBDEAL:
                        ActionOfQueryTrustDeal(playObject, questActionInfo);
                        break;
                    case ExecutionCode.CLEARDELAYGOTO:
                        playObject.IsTimeGoto = false;
                        playObject.TimeGotoLable = "";
                        playObject.TimeGotoNpc = null;
                        break;
                    case ExecutionCode.QUERYVALUE:
                        ActionOfQueryValue(playObject, questActionInfo);
                        break;
                    case ExecutionCode.KILLSLAVENAME:
                        ActionOfKillSlaveName(playObject, questActionInfo);
                        break;
                    case ExecutionCode.QUERYITEMDLG:
                        ActionOfQueryItemDlg(playObject, questActionInfo);
                        break;
                    case ExecutionCode.UPGRADEDLGITEM:
                        ActionOfUpgradeDlgItem(playObject, questActionInfo);
                        break;
                    case ExecutionCode.CHANGEMODE:
                        ActionOfChangeMode(playObject, questActionInfo);
                        break;
                    case ExecutionCode.KILL:
                        ActionOfKill(playObject, questActionInfo);
                        break;
                    case ExecutionCode.KICK:
                        ActionOfKick(playObject, questActionInfo);
                        break;
                    case ExecutionCode.DELAYGOTO:
                        ActionOfDelayCall(playObject, questActionInfo);
                        break;
                    case ExecutionCode.GETDLGITEMVALUE:

                        break;
                    case ExecutionCode.TakeDlgItem:

                        break;
                }
            }
            return result;
        }

        private static void ActionOfUpgradeDlgItem(PlayObject playObject, QuestActionInfo questActionInfo)
        {

        }

        private void ActionOfQueryItemDlg(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            playObject.TakeDlgItem = questActionInfo.nParam3 != 0;
            playObject.GotoNpcLabel = questActionInfo.sParam2;
            string sHint = questActionInfo.sParam1;
            if (string.IsNullOrEmpty(sHint)) sHint = "请输入:";
            playObject.SendDefMessage(Messages.SM_QUERYITEMDLG, ActorId, 0, 0, 0, sHint);
        }

        private void ActionOfKillSlaveName(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            string sSlaveName = questActionInfo.sParam1;
            if (string.IsNullOrEmpty(sSlaveName))
            {
                ScriptActionError(playObject, "", questActionInfo, ExecutionCode.KILLSLAVENAME);
                return;
            }
            if (sSlaveName.Equals("*") || string.Compare(sSlaveName, "ALL", StringComparison.OrdinalIgnoreCase) == 0)
            {
                for (int i = 0; i < playObject.SlaveList.Count; i++)
                {
                    playObject.SlaveList[i].WAbil.HP = 0;
                }
                return;
            }
            for (int i = 0; i < playObject.SlaveList.Count; i++)
            {
                BaseObject baseObject = playObject.SlaveList[i];
                if (!Death && (string.Compare(sSlaveName, baseObject.ChrName, StringComparison.OrdinalIgnoreCase) == 0))
                {
                    baseObject.WAbil.HP = 0;
                }
            }
        }

        private static void ActionOfQueryValue(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            int btStrLabel = questActionInfo.nParam1;
            if (btStrLabel < 100)
            {
                btStrLabel = 0;
            }
            playObject.ValLabel = (byte)btStrLabel;
            byte btType = (byte)questActionInfo.nParam2;
            if (btType > 3)
            {
                btType = 0;
            }
            playObject.ValType = btType;
            int btLen = HUtil32._MAX(1, questActionInfo.nParam3);
            playObject.GotoNpcLabel = questActionInfo.sParam4;
            string sHint = questActionInfo.sParam5;
            playObject.ValNpcType = 0;
            if (string.Compare(questActionInfo.sParam6, "QF", StringComparison.OrdinalIgnoreCase) == 0)
            {
                playObject.ValNpcType = 1;
            }
            else if (string.Compare(questActionInfo.sParam6, "QM", StringComparison.OrdinalIgnoreCase) == 0)
            {
                playObject.ValNpcType = 2;
            }
            if (string.IsNullOrEmpty(sHint))
            {
                sHint = "请输入：";
            }
            playObject.SendDefMessage(Messages.SM_QUERYVALUE, 0, HUtil32.MakeWord(btType, (ushort)btLen), 0, 0, sHint);
        }

        private void GotoLableSendMerChantSayMsg(PlayObject playObject, string sMsg, bool boFlag)
        {
            sMsg = GetLineVariableText(playObject, sMsg);
            playObject.GetScriptLabel(sMsg);
            if (boFlag)
            {
                playObject.SendPriorityMsg(Messages.RM_MERCHANTSAY, 0, 0, 0, 0, ChrName + '/' + sMsg, MessagePriority.High);
            }
            else
            {
                playObject.SendMsg(this, Messages.RM_MERCHANTSAY, 0, 0, 0, 0, ChrName + '/' + sMsg);
            }
        }
    }
}