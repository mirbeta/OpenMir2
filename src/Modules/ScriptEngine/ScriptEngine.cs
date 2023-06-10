using SystemModule;
using SystemModule.Common;
using SystemModule.Data;
using SystemModule.Enums;
using SystemModule.Packets.ClientPackets;

namespace ScriptSystem
{
    /// <summary>
    /// 脚本执行引擎
    /// </summary>
    public class ScriptEngine
    {
        private ConditionProcessingSys ConditionScript;
        private ExecutionProcessingSys ExecutionProcessing;

        public ScriptEngine()
        {
            ConditionScript = new ConditionProcessingSys("", "", "", 0, 0);
            ConditionScript.Initialize();
            ExecutionProcessing = new ExecutionProcessingSys();
            ExecutionProcessing.Initialize();
        }

        private void GotoLable(IPlayerActor playerActor, int npcId, string sLabel, bool boExtJmp, string sMsg)
        {
            var currentNpc = SystemShare.ActorMgr.Get<INormNpc>(npcId);
            if (currentNpc == null)
            {
                return;   
            }
            if (playerActor.LastNpc != npcId)
            {
                playerActor.LastNpc = 0;
            }
            ScriptInfo script = null;
            SayingRecord sayingRecord;
            UserItem userItem = null;
            string sC = string.Empty;
            /*var scriptList = currentNpc.GetScriptList();
            if (string.Compare("@main", sLabel, StringComparison.OrdinalIgnoreCase) == 0)
            {
                for (int i = 0; i < scriptList.Count; i++)
                {
                    ScriptInfo script3C = scriptList[i];
                    if (script3C.RecordList.TryGetValue(sLabel, out _))
                    {
                        script = script3C;
                        playerActor.MScript = script;
                        playerActor.LastNpc = npcId;
                        break;
                    }
                }
            }
            if (script == null)
            {
                if (playerActor.MScript != null)
                {
                    for (int i = scriptList.Count - 1; i >= 0; i--)
                    {
                        if (scriptList[i] == playerActor.MScript)
                        {
                            script = scriptList[i];
                        }
                    }
                }
                if (script == null)
                {
                    for (int i = scriptList.Count - 1; i >= 0; i--)
                    {
                        if (CheckGotoLableQuestStatus(playerActor, scriptList[i]))
                        {
                            script = scriptList[i];
                            playerActor.MScript = script;
                            playerActor.LastNpc = npcId;
                        }
                    }
                }
            }*/
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
                        if (GotoLableQuestCheckCondition(playerActor,sayingProcedure.ConditionList, ref sC, ref userItem))
                        {
                            sSendMsg = sSendMsg + sayingProcedure.sSayMsg;
                            if (!GotoLableQuestActionProcess(playerActor,sayingProcedure.ActionList, ref sC, ref userItem, ref bo11))
                            {
                                break;
                            }
                            if (bo11)
                            {
                                GotoLableSendMerChantSayMsg(playerActor,sSendMsg, true);
                            }
                        }
                        else
                        {
                            sSendMsg = sSendMsg + sayingProcedure.sElseSayMsg;
                            if (!GotoLableQuestActionProcess(playerActor,sayingProcedure.ElseActionList, ref sC, ref userItem, ref bo11))
                            {
                                break;
                            }
                            if (bo11)
                            {
                                GotoLableSendMerChantSayMsg(playerActor,sSendMsg, true);
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(sSendMsg))
                    {
                        GotoLableSendMerChantSayMsg(playerActor,sSendMsg, false);
                    }
                }
            }
        }

        public void GotoLable(IActor playerActor, int npcId, string sLabel, bool boExtJmp)
        {
            GotoLable((IPlayerActor)playerActor, npcId, sLabel, boExtJmp, string.Empty);
        }

        private static bool CheckGotoLableQuestStatus(IPlayerActor playerActor, ScriptInfo scriptInfo)
        {
            bool result = true;
            if (!scriptInfo.IsQuest)
            {
                return true;
            }
            int nIndex = 0;
            while (true)
            {
                if ((scriptInfo.QuestInfo[nIndex].nRandRage > 0) && (SystemShare.RandomNumber.Random(scriptInfo.QuestInfo[nIndex].nRandRage) != 0))
                {
                    result = false;
                    break;
                }
                if (playerActor.GetQuestFalgStatus(scriptInfo.QuestInfo[nIndex].wFlag) != scriptInfo.QuestInfo[nIndex].btValue)
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

        private static UserItem CheckGotoLableItemW(IPlayerActor playerActor, string sItemType, int nParam)
        {
            UserItem result = null;
            int nCount = 0;
            if (HUtil32.CompareLStr(sItemType, "[NECKLACE]", 4))
            {
                if (playerActor.UseItems[ItemLocation.Necklace].Index > 0)
                {
                    result = playerActor.UseItems[ItemLocation.Necklace];
                }
                return result;
            }
            if (HUtil32.CompareLStr(sItemType, "[RING]", 4))
            {
                if (playerActor.UseItems[ItemLocation.Ringl].Index > 0)
                {
                    result = playerActor.UseItems[ItemLocation.Ringl];
                }
                if (playerActor.UseItems[ItemLocation.Ringr].Index > 0)
                {
                    result = playerActor.UseItems[ItemLocation.Ringr];
                }
                return result;
            }
            if (HUtil32.CompareLStr(sItemType, "[ARMRING]", 4))
            {
                if (playerActor.UseItems[ItemLocation.ArmRingl].Index > 0)
                {
                    result = playerActor.UseItems[ItemLocation.ArmRingl];
                }
                if (playerActor.UseItems[ItemLocation.ArmRingr].Index > 0)
                {
                    result = playerActor.UseItems[ItemLocation.ArmRingr];
                }
                return result;
            }
            if (HUtil32.CompareLStr(sItemType, "[WEAPON]", 4))
            {
                if (playerActor.UseItems[ItemLocation.Weapon].Index > 0)
                {
                    result = playerActor.UseItems[ItemLocation.Weapon];
                }
                return result;
            }
            if (HUtil32.CompareLStr(sItemType, "[HELMET]", 4))
            {
                if (playerActor.UseItems[ItemLocation.Helmet].Index > 0)
                {
                    result = playerActor.UseItems[ItemLocation.Helmet];
                }
                return result;
            }
            if (HUtil32.CompareLStr(sItemType, "[BUJUK]", 4))
            {
                if (playerActor.UseItems[ItemLocation.Bujuk].Index > 0)
                {
                    result = playerActor.UseItems[ItemLocation.Bujuk];
                }
                return result;
            }
            if (HUtil32.CompareLStr(sItemType, "[BELT]", 4))
            {
                if (playerActor.UseItems[ItemLocation.Belt].Index > 0)
                {
                    result = playerActor.UseItems[ItemLocation.Belt];
                }
                return result;
            }
            if (HUtil32.CompareLStr(sItemType, "[BOOTS]", 4))
            {
                if (playerActor.UseItems[ItemLocation.Boots].Index > 0)
                {
                    result = playerActor.UseItems[ItemLocation.Boots];
                }
                return result;
            }
            if (HUtil32.CompareLStr(sItemType, "[CHARM]", 4))
            {
                if (playerActor.UseItems[ItemLocation.Charm].Index > 0)
                {
                    result = playerActor.UseItems[ItemLocation.Charm];
                }
                return result;
            }
            result = playerActor.CheckItemCount(sItemType, ref nCount);
            if (nCount < nParam)
            {
                result = null;
            }
            return result;
        }

        private static bool CheckGotoLableStringList(string sHumName, string sListFileName)
        {
            bool result = false;
            sListFileName = SystemShare.GetEnvirFilePath(sListFileName);
            if (File.Exists(sListFileName))
            {
                using StringList loadList = new StringList();
                try
                {
                    loadList.LoadFromFile(sListFileName);
                }
                catch
                {
                    SystemShare.Logger.Error("loading fail.... => " + sListFileName);
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
                SystemShare.Logger.Error("file not found => " + sListFileName);
            }
            return result;
        }

        private static void GotoLableQuestCheckConditionSetVal(IPlayerActor playerActor, string sIndex, int nCount)
        {
            int n14 = SystemShare.GetValNameNo(sIndex);
            if (n14 >= 0)
            {
                if (HUtil32.RangeInDefined(n14, 0, 99))
                {
                    playerActor.MNVal[n14] = nCount;
                }
                else if (HUtil32.RangeInDefined(n14, 100, 119))
                {
                    SystemShare.Config.GlobalVal[n14 - 100] = nCount;
                }
                else if (HUtil32.RangeInDefined(n14, 200, 299))
                {
                    playerActor.MDyVal[n14 - 200] = nCount;
                }
                else if (HUtil32.RangeInDefined(n14, 300, 399))
                {
                    playerActor.MNMval[n14 - 300] = nCount;
                }
                else if (HUtil32.RangeInDefined(n14, 400, 499))
                {
                    SystemShare.Config.GlobaDyMval[n14 - 400] = (short)nCount;
                }
                else if (HUtil32.RangeInDefined(n14, 500, 599))
                {
                    playerActor.MNSval[n14 - 600] = nCount.ToString();
                }
            }
        }

        private static bool GotoLable_QuestCheckCondition_CheckDieMon(IPlayerActor playerActor, string monName)
        {
            bool result = string.IsNullOrEmpty(monName);
            if ((playerActor.LastHiter != null) && (playerActor.LastHiter.ChrName == monName))
            {
                result = true;
            }
            return result;
        }

        private static bool GotoLable_QuestCheckCondition_CheckKillMon(IPlayerActor playerActor, string monName)
        {
            bool result = string.IsNullOrEmpty(monName);
            if ((playerActor.TargetCret != null) && (playerActor.TargetCret.ChrName == monName))
            {
                result = true;
            }
            return result;
        }

        public static bool GotoLable_QuestCheckCondition_CheckRandomNo(IPlayerActor playerActor, string sNumber)
        {
            return playerActor.RandomNo == sNumber;
        }

        private bool QuestCheckConditionCheckUserDateType(IPlayerActor playerActor, string chrName, string sListFileName, string sDay, string param1, string param2)
        {
            string name = string.Empty;
            bool result = false;
            sListFileName = SystemShare.GetEnvirFilePath(sListFileName);
            using StringList loadList = new StringList();
            if (File.Exists(sListFileName))
            {
                try
                {
                    loadList.LoadFromFile(sListFileName);
                }
                catch
                {
                    SystemShare.Logger.Error("loading fail.... => " + sListFileName);
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
                    GotoLableQuestCheckConditionSetVal(playerActor,param1, useDay);
                    GotoLableQuestCheckConditionSetVal(playerActor,param2, lastDay);
                    return result;
                }
            }
            return false;
        }

        private bool GotoLableQuestCheckCondition(IPlayerActor playerActor, IList<QuestConditionInfo> conditionList, ref string sC, ref UserItem userItem)
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
                    ConditionScript.Execute(playerActor,questConditionInfo, ref result);
                    return result;
                }
                if (!string.IsNullOrEmpty(questConditionInfo.sParam1))
                {
                    if (questConditionInfo.sParam1[0] == '$')
                    {
                        string s50 = questConditionInfo.sParam1;
                        questConditionInfo.sParam1 = '<' + questConditionInfo.sParam1 + '>';
                        ConditionScript.GetVariableText(playerActor,ref s50, questConditionInfo.sParam1);
                    }
                    else if (questConditionInfo.sParam1.IndexOf(">", StringComparison.OrdinalIgnoreCase) > -1)
                    {
                        questConditionInfo.sParam1 = ConditionScript.GetLineVariableText(playerActor,questConditionInfo.sParam1);
                    }
                }
                if (!string.IsNullOrEmpty(questConditionInfo.sParam2))
                {
                    if (questConditionInfo.sParam2[0] == '$')
                    {
                        string s50 = questConditionInfo.sParam2;
                        questConditionInfo.sParam2 = '<' + questConditionInfo.sParam2 + '>';
                        ConditionScript.GetVariableText(playerActor,ref s50, questConditionInfo.sParam2);
                    }
                    else if (questConditionInfo.sParam2.IndexOf(">", StringComparison.OrdinalIgnoreCase) > -1)
                    {
                        questConditionInfo.sParam2 = ConditionScript.GetLineVariableText(playerActor,questConditionInfo.sParam2);
                    }
                }
                if (!string.IsNullOrEmpty(questConditionInfo.sParam3))
                {
                    if (questConditionInfo.sParam3[0] == '$')
                    {
                        string s50 = questConditionInfo.sParam3;
                        questConditionInfo.sParam3 = '<' + questConditionInfo.sParam3 + '>';
                        ConditionScript.GetVariableText(playerActor,ref s50, questConditionInfo.sParam3);
                    }
                    else if (questConditionInfo.sParam3.IndexOf(">", StringComparison.OrdinalIgnoreCase) > -1)
                    {
                        questConditionInfo.sParam3 = ConditionScript.GetLineVariableText(playerActor,questConditionInfo.sParam3);
                    }
                }
                if (!string.IsNullOrEmpty(questConditionInfo.sParam4))
                {
                    if (questConditionInfo.sParam4[0] == '$')
                    {
                        string s50 = questConditionInfo.sParam4;
                        questConditionInfo.sParam4 = '<' + questConditionInfo.sParam4 + '>';
                        ConditionScript.GetVariableText(playerActor,ref s50, questConditionInfo.sParam4);
                    }
                    else if (questConditionInfo.sParam4.IndexOf(">", StringComparison.OrdinalIgnoreCase) > -1)
                    {
                        questConditionInfo.sParam4 = ConditionScript.GetLineVariableText(playerActor,questConditionInfo.sParam4);
                    }
                }
                if (!string.IsNullOrEmpty(questConditionInfo.sParam5))
                {
                    if (questConditionInfo.sParam5[0] == '$')
                    {
                        string s50 = questConditionInfo.sParam5;
                        questConditionInfo.sParam5 = '<' + questConditionInfo.sParam5 + '>';
                        ConditionScript.GetVariableText(playerActor,ref s50, questConditionInfo.sParam5);
                    }
                    else if (questConditionInfo.sParam5.IndexOf(">", StringComparison.OrdinalIgnoreCase) > -1)
                    {
                        questConditionInfo.sParam5 = ConditionScript.GetLineVariableText(playerActor,questConditionInfo.sParam5);
                    }
                }
                if (!string.IsNullOrEmpty(questConditionInfo.sParam6))
                {
                    if (questConditionInfo.sParam6[0] == '$')
                    {
                        string s50 = questConditionInfo.sParam6;
                        questConditionInfo.sParam6 = '<' + questConditionInfo.sParam6 + '>';
                        ConditionScript.GetVariableText(playerActor,ref s50, questConditionInfo.sParam6);
                    }
                    else if (questConditionInfo.sParam6.IndexOf(">", StringComparison.OrdinalIgnoreCase) > -1)
                    {
                        questConditionInfo.sParam6 = ConditionScript.GetLineVariableText(playerActor,questConditionInfo.sParam6);
                    }
                }
                if (!string.IsNullOrEmpty(questConditionInfo.sOpName))
                {
                    if (questConditionInfo.sOpName.Length > 2)
                    {
                        if (questConditionInfo.sOpName[1] == '$')
                        {
                            string s50 = questConditionInfo.sOpName;
                            questConditionInfo.sOpName = '<' + questConditionInfo.sOpName + '>';
                            ConditionScript.GetVariableText(playerActor,ref s50, questConditionInfo.sOpName);
                        }
                        else if (questConditionInfo.sOpName.IndexOf(">", StringComparison.OrdinalIgnoreCase) > -1)
                        {
                            questConditionInfo.sOpName = ConditionScript.GetLineVariableText(playerActor,questConditionInfo.sOpName);
                        }
                    }
                    /*IPlayerActor human = M2Share.WorldEngine.GetIPlayerActor(questConditionInfo.sOpName);
                    if (human != null)
                    {
                        IPlayerActor = human;
                        if (!string.IsNullOrEmpty(questConditionInfo.sOpHName) && string.Compare(questConditionInfo.sOpHName, "H", StringComparison.OrdinalIgnoreCase) == 0)
                        {

                        }
                    }*/
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
                      //  result = QuestCheckConditionCheckUserDateType(playerActor,playerActor.ChrName, m_sPath + questConditionInfo.sParam1, questConditionInfo.sParam3, questConditionInfo.sParam4, questConditionInfo.sParam5);
                        break;
                    case (int)ConditionCode.CHECKRANDOMNO:
                        SystemShare.Logger.Error("TODO nSC_CHECKRANDOMNO...");
                        //result = GotoLable_QuestCheckCondition_CheckRandomNo(playerActor,sMsg);
                        break;
                    case (int)ConditionCode.CHECKDIEMON:
                        result = GotoLable_QuestCheckCondition_CheckDieMon(playerActor,questConditionInfo.sParam1);
                        break;
                    case (int)ConditionCode.CHECKKILLPLAYMON:
                        result = GotoLable_QuestCheckCondition_CheckKillMon(playerActor,questConditionInfo.sParam1);
                        break;
                    case (int)ConditionCode.CHECKITEMW:
                        userItem = CheckGotoLableItemW(playerActor,questConditionInfo.sParam1, questConditionInfo.nParam2);
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
                        userItem = playerActor.QuestCheckItem(questConditionInfo.sParam1, ref n1C, ref nMaxDura, ref nDura);
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
                        if ((playerActor.LastHiter != null) && (playerActor.LastHiter.Race != ActorRace.Play))
                        {
                            result = false;
                        }
                        break;
                    case (int)ConditionCode.KILLBYMON:
                        if ((playerActor.LastHiter != null) && (playerActor.LastHiter.Race == ActorRace.Play))
                        {
                            result = false;
                        }
                        break;
                    case (int)ConditionCode.CHECKINSAFEZONE:
                        if (!playerActor.InSafeZone())
                        {
                            result = false;
                        }
                        break;
                    case (int)ConditionCode.SCHECKDEATHPLAYMON:
                        string s01 = string.Empty;
                        //if (!GetValValue(playerActor,questConditionInfo.sParam1, ref s01))
                        //{
                        //    s01 = ConditionScript.GetLineVariableText(playerActor,questConditionInfo.sParam1);
                        //}
                        result = CheckKillMon2(playerActor,s01);
                        break;
                }
                if (!result)
                {
                    break;
                }
            }
            return result;
        }

        private static bool CheckKillMon2(IPlayerActor playerActor, string sMonName)
        {
            return true;
        }

        private bool JmpToLable(IPlayerActor playerActor, string sLabel)
        {
            playerActor.ScriptGotoCount++;
            if (playerActor.ScriptGotoCount > SystemShare.Config.ScriptGotoCountLimit)
            {
                return false;
            }
            //GotoLable(playerActor,sLabel, false);
            return true;
        }

        private void GoToQuest(IPlayerActor playerActor, int nQuest)
        {
            //for (int i = 0; i < m_ScriptList.Count; i++)
            //{
            //    ScriptInfo script = m_ScriptList[i];
            //    if (script.QuestCount == nQuest)
            //    {
            //        playerActor.MScript = script;
            //        playerActor.LastNpc = this.ActorId;
            //        GotoLable(playerActor,ScriptFlagConst.sMAIN, false);
            //        break;
            //    }
            //}
        }

        private void GotoLableTakeItem(IPlayerActor playerActor, string sItemName, int nItemCount, ref string sC)
        {
            UserItem userItem;
            StdItem stdItem;
            if (string.Compare(sItemName, Grobal2.StringGoldName, StringComparison.OrdinalIgnoreCase) == 0)
            {
                playerActor.DecGold(nItemCount);
                playerActor.GoldChanged();
                if (SystemShare.GameLogGold)
                {
                    // M2Share.EventSource.AddEventLog(10, playerActor.MapName + "\t" + playerActor.CurrX + "\t" + playerActor.CurrY + "\t" + playerActor.ChrName + "\t" + Grobal2.StringGoldName + "\t" + nItemCount + "\t" + '1' + "\t" + ChrName);
                }
                return;
            }
            for (int i = playerActor.ItemList.Count - 1; i >= 0; i--)
            {
                if (nItemCount <= 0)
                {
                    break;
                }
                userItem = playerActor.ItemList[i];
                if (string.Compare(SystemShare.ItemSystem.GetStdItemName(userItem.Index), sItemName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    stdItem = SystemShare.ItemSystem.GetStdItem(userItem.Index);
                    if (stdItem.NeedIdentify == 1)
                    {
                       // M2Share.EventSource.AddEventLog(10, playerActor.MapName + "\t" + playerActor.CurrX + "\t" + playerActor.CurrY + "\t" + playerActor.ChrName + "\t" + sItemName + "\t" + userItem.MakeIndex + "\t" + '1' + "\t" + ChrName);
                    }
                    playerActor.SendDelItems(userItem);
                    sC = SystemShare.ItemSystem.GetStdItemName(userItem.Index);
                    Dispose(userItem);
                    playerActor.ItemList.RemoveAt(i);
                    nItemCount -= 1;
                }
            }
        }

        public void GotoLableGiveItem(IPlayerActor playerActor, string sItemName, int nItemCount)
        {
            UserItem userItem;
            StdItem stdItem;
            if (string.Compare(sItemName, Grobal2.StringGoldName, StringComparison.OrdinalIgnoreCase) == 0)
            {
                playerActor.IncGold(nItemCount);
                playerActor.GoldChanged();
                if (SystemShare.GameLogGold)
                {
                   // M2Share.EventSource.AddEventLog(9, playerActor.MapName + "\t" + playerActor.CurrX + "\t" + playerActor.CurrY + "\t" + playerActor.ChrName + "\t" + Grobal2.StringGoldName + "\t" + nItemCount + "\t" + '1' + "\t" + ChrName);
                }
                return;
            }
            if (SystemShare.ItemSystem.GetStdItemIdx(sItemName) > 0)
            {
                if (!(nItemCount >= 1 && nItemCount <= 50))
                {
                    nItemCount = 1;
                }
                for (int i = 0; i < nItemCount; i++)
                {
                    if (playerActor.IsEnoughBag)
                    {
                        userItem = new UserItem();
                        if (SystemShare.ItemSystem.CopyToUserItemFromName(sItemName, ref userItem))
                        {
                            playerActor.ItemList.Add(userItem);
                            playerActor.SendAddItem(userItem);
                            stdItem = SystemShare.ItemSystem.GetStdItem(userItem.Index);
                            if (stdItem.NeedIdentify == 1)
                            {
                                //M2Share.EventSource.AddEventLog(9, playerActor.MapName + "\t" + playerActor.CurrX + "\t" + playerActor.CurrY + "\t" + playerActor.ChrName + "\t" + sItemName + "\t" + userItem.MakeIndex + "\t" + '1' + "\t" + ChrName);
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
                        if (SystemShare.ItemSystem.CopyToUserItemFromName(sItemName, ref userItem))
                        {
                            stdItem = SystemShare.ItemSystem.GetStdItem(userItem.Index);
                            if (stdItem.NeedIdentify == 1)
                            {
                               // M2Share.EventSource.AddEventLog(9, playerActor.MapName + "\t" + playerActor.CurrX + "\t" + playerActor.CurrY + "\t" + playerActor.ChrName + "\t" + sItemName + "\t" + userItem.MakeIndex + "\t" + '1' + "\t" + ChrName);
                            }
                            playerActor.DropItemDown(userItem, 3, false, playerActor.ActorId, 0);
                        }
                        Dispose(userItem);
                    }
                }
            }
        }

        private static void GotoLableTakeWItem(IPlayerActor playerActor, string sItemName, int nItemCount)
        {
            string sC;
            if (HUtil32.CompareLStr(sItemName, "[NECKLACE]", 4))
            {
                if (playerActor.UseItems[ItemLocation.Necklace].Index > 0)
                {
                    playerActor.SendDelItems(playerActor.UseItems[ItemLocation.Necklace]);
                    sC = SystemShare.ItemSystem.GetStdItemName(playerActor.UseItems[ItemLocation.Necklace].Index);
                    playerActor.UseItems[ItemLocation.Necklace].Index = 0;
                    return;
                }
            }
            if (HUtil32.CompareLStr(sItemName, "[RING]", 4))
            {
                if (playerActor.UseItems[ItemLocation.Ringl].Index > 0)
                {
                    playerActor.SendDelItems(playerActor.UseItems[ItemLocation.Ringl]);
                    sC = SystemShare.ItemSystem.GetStdItemName(playerActor.UseItems[ItemLocation.Ringl].Index);
                    playerActor.UseItems[ItemLocation.Ringl].Index = 0;
                    return;
                }
                if (playerActor.UseItems[ItemLocation.Ringr].Index > 0)
                {
                    playerActor.SendDelItems(playerActor.UseItems[ItemLocation.Ringr]);
                    sC = SystemShare.ItemSystem.GetStdItemName(playerActor.UseItems[ItemLocation.Ringr].Index);
                    playerActor.UseItems[ItemLocation.Ringr].Index = 0;
                    return;
                }
            }
            if (HUtil32.CompareLStr(sItemName, "[ARMRING]", 4))
            {
                if (playerActor.UseItems[ItemLocation.ArmRingl].Index > 0)
                {
                    playerActor.SendDelItems(playerActor.UseItems[ItemLocation.ArmRingl]);
                    sC = SystemShare.ItemSystem.GetStdItemName(playerActor.UseItems[ItemLocation.ArmRingl].Index);
                    playerActor.UseItems[ItemLocation.ArmRingl].Index = 0;
                    return;
                }
                if (playerActor.UseItems[ItemLocation.ArmRingr].Index > 0)
                {
                    playerActor.SendDelItems(playerActor.UseItems[ItemLocation.ArmRingr]);
                    sC = SystemShare.ItemSystem.GetStdItemName(playerActor.UseItems[ItemLocation.ArmRingr].Index);
                    playerActor.UseItems[ItemLocation.ArmRingr].Index = 0;
                    return;
                }
            }
            if (HUtil32.CompareLStr(sItemName, "[WEAPON]", 4))
            {
                if (playerActor.UseItems[ItemLocation.Weapon].Index > 0)
                {
                    playerActor.SendDelItems(playerActor.UseItems[ItemLocation.Weapon]);
                    sC = SystemShare.ItemSystem.GetStdItemName(playerActor.UseItems[ItemLocation.Weapon].Index);
                    playerActor.UseItems[ItemLocation.Weapon].Index = 0;
                    return;
                }
            }
            if (HUtil32.CompareLStr(sItemName, "[HELMET]", 4))
            {
                if (playerActor.UseItems[ItemLocation.Helmet].Index > 0)
                {
                    playerActor.SendDelItems(playerActor.UseItems[ItemLocation.Helmet]);
                    sC = SystemShare.ItemSystem.GetStdItemName(playerActor.UseItems[ItemLocation.Helmet].Index);
                    playerActor.UseItems[ItemLocation.Helmet].Index = 0;
                    return;
                }
            }
            if (HUtil32.CompareLStr(sItemName, "[DRESS]", 4))
            {
                if (playerActor.UseItems[ItemLocation.Dress].Index > 0)
                {
                    playerActor.SendDelItems(playerActor.UseItems[ItemLocation.Dress]);
                    sC = SystemShare.ItemSystem.GetStdItemName(playerActor.UseItems[ItemLocation.Dress].Index);
                    playerActor.UseItems[ItemLocation.Dress].Index = 0;
                    return;
                }
            }
            if (HUtil32.CompareLStr(sItemName, "[U_BUJUK]", 4))
            {
                if (playerActor.UseItems[ItemLocation.Bujuk].Index > 0)
                {
                    playerActor.SendDelItems(playerActor.UseItems[ItemLocation.Bujuk]);
                    sC = SystemShare.ItemSystem.GetStdItemName(playerActor.UseItems[ItemLocation.Bujuk].Index);
                    playerActor.UseItems[ItemLocation.Bujuk].Index = 0;
                    return;
                }
            }
            if (HUtil32.CompareLStr(sItemName, "[U_BELT]", 4))
            {
                if (playerActor.UseItems[ItemLocation.Belt].Index > 0)
                {
                    playerActor.SendDelItems(playerActor.UseItems[ItemLocation.Belt]);
                    sC = SystemShare.ItemSystem.GetStdItemName(playerActor.UseItems[ItemLocation.Belt].Index);
                    playerActor.UseItems[ItemLocation.Belt].Index = 0;
                    return;
                }
            }
            if (HUtil32.CompareLStr(sItemName, "[U_BOOTS]", 4))
            {
                if (playerActor.UseItems[ItemLocation.Boots].Index > 0)
                {
                    playerActor.SendDelItems(playerActor.UseItems[ItemLocation.Boots]);
                    sC = SystemShare.ItemSystem.GetStdItemName(playerActor.UseItems[ItemLocation.Boots].Index);
                    playerActor.UseItems[ItemLocation.Boots].Index = 0;
                    return;
                }
            }
            if (HUtil32.CompareLStr(sItemName, "[U_CHARM]", 4))
            {
                if (playerActor.UseItems[ItemLocation.Charm].Index > 0)
                {
                    playerActor.SendDelItems(playerActor.UseItems[ItemLocation.Charm]);
                    sC = SystemShare.ItemSystem.GetStdItemName(playerActor.UseItems[ItemLocation.Charm].Index);
                    playerActor.UseItems[ItemLocation.Charm].Index = 0;
                    return;
                }
            }
            for (int i = 0; i < playerActor.UseItems.Length; i++)
            {
                if (nItemCount <= 0)
                {
                    return;
                }
                if (playerActor.UseItems[i].Index > 0)
                {
                    string sName = SystemShare.ItemSystem.GetStdItemName(playerActor.UseItems[i].Index);
                    if (string.Compare(sName, sItemName, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        playerActor.SendDelItems(playerActor.UseItems[i]);
                        playerActor.UseItems[i].Index = 0;
                        nItemCount -= 1;
                    }
                }
            }
        }

        private bool GotoLableQuestActionProcess(IPlayerActor playerActor, IList<QuestActionInfo> actionList, ref string sC, ref UserItem userItem, ref bool bo11)
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
                  //  ExecutionProcessing.Execute(this, playerActor,questActionInfo, ref result);
                    return result;
                }

                ExecutionCode executionCode = (ExecutionCode)questActionInfo.nCmdCode;
                switch (executionCode)
                {
                    case ExecutionCode.Take:
                        GotoLableTakeItem(playerActor,questActionInfo.sParam1, questActionInfo.nParam2, ref sC);
                        break;
                    case ExecutionCode.Takew:
                        GotoLableTakeWItem(playerActor,questActionInfo.sParam1, questActionInfo.nParam2);
                        break;
                    case ExecutionCode.TakecheckItem:
                        if (userItem != null)
                        {
                            playerActor.QuestTakeCheckItem(userItem);
                        }
                        else
                        {
                            ScriptActionError(playerActor,"", questActionInfo, ExecutionCode.TakecheckItem);
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
                    case ExecutionCode.AddBatch:
                        //if (BatchParamsList == null)
                        //{
                        //    BatchParamsList = new List<ScriptParams>();
                        //}
                        //BatchParamsList.Add(new ScriptParams()
                        //{
                        //    sParams = questActionInfo.sParam1,
                        //    nParams = n18
                        //});
                        break;
                    case ExecutionCode.BatchDelay:
                        n18 = questActionInfo.nParam1 * 1000;
                        break;
                    case ExecutionCode.BatchMove:
                        //int n20 = 0;
                        //for (int k = 0; k < BatchParamsList.Count; k++)
                        //{
                        //    ScriptParams batchParam = BatchParamsList[k];
                        //    playerActor.SendSelfDelayMsg(Messages.RM_RANDOMSPACEMOVE, 0, 0, 0, 0, BatchParamsList[k].sParams, batchParam.nParams + n20);
                        //    n20 += batchParam.nParams;
                        //}
                        break;
                    case ExecutionCode.PlayDice:
                        bo11 = true;
                        break;
                    case ExecutionCode.GoQuest:
                        GoToQuest(playerActor,questActionInfo.nParam1);
                        break;
                    case ExecutionCode.EndQuest:
                        //playerActor.MScript = null;
                        break;
                    case ExecutionCode.Goto:
                        if (!JmpToLable(playerActor,questActionInfo.sParam1))
                        {
                            //M2Share.Logger.Error("[�ű���ѭ��] NPC:" + ChrName + " λ��:" + MapName + '(' + CurrX + ':' + CurrY + ')' + " ����:" + ExecutionCode.Goto + ' ' + questActionInfo.sParam1);
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

        private void GotoLableSendMerChantSayMsg(IPlayerActor playerActor, string sMsg, bool boFlag)
        {
            //sMsg = GetLineVariableText(playerActor,sMsg);
            //playerActor.GetScriptLabel(sMsg);
            //if (boFlag)
            //{
            //    playerActor.SendPriorityMsg(Messages.RM_MERCHANTSAY, 0, 0, 0, 0, ChrName + '/' + sMsg, MessagePriority.High);
            //}
            //else
            //{
            //    playerActor.SendMsg(this, Messages.RM_MERCHANTSAY, 0, 0, 0, 0, ChrName + '/' + sMsg);
            //}
        }
        
        private void ScriptActionError(IPlayerActor playerActor, string sErrMsg, QuestActionInfo QuestActionInfo, ExecutionCode sCmd)
        {
            const string sOutMessage = "[�ű�����] {0} �ű�����:{1} NPC����:{2} ��ͼ:{3}({4}:{5}) ����1:{6} ����2:{7} ����3:{8} ����4:{9} ����5:{10} ����6:{11}";
           // string sMsg = string.Format(sOutMessage, sErrMsg, sCmd, ChrName, MapName, CurrX, CurrY, QuestActionInfo.sParam1, QuestActionInfo.sParam2, QuestActionInfo.sParam3, QuestActionInfo.sParam4, QuestActionInfo.sParam5, QuestActionInfo.sParam6);
          //  M2Share.Logger.Error(sMsg);
        }

        private void ScriptActionError(IPlayerActor playerActor, string sErrMsg, QuestActionInfo QuestActionInfo, string sCmd)
        {
            const string sOutMessage = "[�ű�����] {0} �ű�����:{1} NPC����:{2} ��ͼ:{3}({4}:{5}) ����1:{6} ����2:{7} ����3:{8} ����4:{9} ����5:{10} ����6:{11}";
           // string sMsg = string.Format(sOutMessage, sErrMsg, sCmd, ChrName, MapName, CurrX, CurrY, QuestActionInfo.sParam1, QuestActionInfo.sParam2, QuestActionInfo.sParam3, QuestActionInfo.sParam4, QuestActionInfo.sParam5, QuestActionInfo.sParam6);
          //  M2Share.Logger.Error(sMsg);
        }

        private void ScriptConditionError(IPlayerActor playerActor, QuestConditionInfo QuestConditionInfo, ConditionCode sCmd)
        {
           // string sMsg = "Cmd:" + sCmd + " NPC����:" + ChrName + " ��ͼ:" + MapName + " ����:" + CurrX + ':' + CurrY + " ����1:" + QuestConditionInfo.sParam1 + " ����2:" + QuestConditionInfo.sParam2 + " ����3:" + QuestConditionInfo.sParam3 + " ����4:" + QuestConditionInfo.sParam4 + " ����5:" + QuestConditionInfo.sParam5;
           // M2Share.Logger.Error("[�ű���������ȷ] " + sMsg);
        }

        private void ScriptConditionError(IPlayerActor playerActor, QuestConditionInfo QuestConditionInfo, string sCmd)
        {
            //string sMsg = "Cmd:" + sCmd + " NPC����:" + ChrName + " ��ͼ:" + MapName + " ����:" + CurrX + ':' + CurrY + " ����1:" + QuestConditionInfo.sParam1 + " ����2:" + QuestConditionInfo.sParam2 + " ����3:" + QuestConditionInfo.sParam3 + " ����4:" + QuestConditionInfo.sParam4 + " ����5:" + QuestConditionInfo.sParam5;
           // M2Share.Logger.Error("[�ű���������ȷ] " + sMsg);
        }

        public void Dispose(object obj)
        {
            obj = null;
        }
    }
}