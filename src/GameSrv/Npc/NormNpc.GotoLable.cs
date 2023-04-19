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
                    case (int)ExecutionCode.CheckUserDate:
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
                if (ExecutionProcessing.IsRegister(questActionInfo.nCmdCode))
                {
                    ExecutionProcessing.Execute(this, playObject, questActionInfo, ref result);
                    return result;
                }
                M2Share.Logger.Info($"[QuestActionProcess] 未处理命令:[{questActionInfo.nCmdCode}]");

                ExecutionCode executionCode = (ExecutionCode)questActionInfo.nCmdCode;
                switch (executionCode)
                {
                    case ExecutionCode.Take:
                        GotoLableTakeItem(playObject, questActionInfo.sParam1, questActionInfo.nParam2, ref sC);
                        break;
                    case ExecutionCode.Takew:
                        GotoLableTakeWItem(playObject, questActionInfo.sParam1, questActionInfo.nParam2);
                        break;
                    case ExecutionCode.TakecheckItem:
                        if (userItem != null)
                        {
                            playObject.QuestTakeCheckItem(userItem);
                        }
                        else
                        {
                            ScriptActionError(playObject, "", questActionInfo, ExecutionCode.TakecheckItem);
                        }
                        break;
                    case ExecutionCode.Break:
                        result = false;
                        break;
                    case ExecutionCode.Param1:
                        n34 = questActionInfo.nParam1;
                        s44 = questActionInfo.sParam1;
                        break;
                    case ExecutionCode.Param2:
                        n38 = questActionInfo.nParam1;
                        string s48 = questActionInfo.sParam1;
                        break;
                    case ExecutionCode.Param3:
                        n3C = questActionInfo.nParam1;
                        string s4C = questActionInfo.sParam1;
                        break;
                    case ExecutionCode.Param4:
                        n40 = questActionInfo.nParam1;
                        break;
                    case ExecutionCode.Map:
                    case ExecutionCode.MapMove:
                        bo11 = true;
                        break;
                    case ExecutionCode.Mov:
                        MovData(playObject, questActionInfo);
                        break;
                    case ExecutionCode.Inc:
                        IncInteger(playObject, questActionInfo);
                        break;
                    case ExecutionCode.Dec:
                        DecInteger(playObject, questActionInfo);
                        break;
                    case ExecutionCode.Sum:
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
                    case ExecutionCode.Movr:
                        MovrData(playObject, questActionInfo);
                        break;
                    case ExecutionCode.AddBatch:
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
                    case ExecutionCode.BatchDelay:
                        n18 = questActionInfo.nParam1 * 1000;
                        break;
                    case ExecutionCode.BatchMove:
                        int n20 = 0;
                        for (int k = 0; k < BatchParamsList.Count; k++)
                        {
                            ScriptParams batchParam = BatchParamsList[k];
                            playObject.SendSelfDelayMsg(Messages.RM_RANDOMSPACEMOVE, 0, 0, 0, 0, BatchParamsList[k].sParams, batchParam.nParams + n20);
                            n20 += batchParam.nParams;
                        }
                        break;
                    case ExecutionCode.PlayDice:
                       bo11 = true;
                        break;
                    case ExecutionCode.GoQuest:
                        GoToQuest(playObject, questActionInfo.nParam1);
                        break;
                    case ExecutionCode.EndQuest:
                        playObject.MScript = null;
                        break;
                    case ExecutionCode.Goto:
                        if (!JmpToLable(playObject, questActionInfo.sParam1))
                        {
                            M2Share.Logger.Error("[脚本死循环] NPC:" + ChrName + " 位置:" + MapName + '(' + CurrX + ':' + CurrY + ')' + " 命令:" + ExecutionCode.Goto + ' ' + questActionInfo.sParam1);
                            result = false;
                            return result;
                        }
                        break;
                    case ExecutionCode.GetDlgItemValue:

                        break;
                    case ExecutionCode.TakeDlgItem:

                        break;
                }
            }
            return result;
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