using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using SystemModule;
using SystemModule.Common;

namespace M2Server
{
    public partial class TNormNpc
    {
        /// <summary>
        /// 开通元宝交易
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="QuestActionInfo"></param>
        private void ActionOfOPENYBDEAL(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            int nGameGold = 0;
            try
            {
                if (PlayObject.bo_YBDEAL)
                {
                    PlayObject.SendMsg(this, Grobal2.RM_MERCHANTSAY, 0, 0, 0, 0, this.m_sCharName + "/您已开通寄售服务,不需要再开通！！！\\ \\<返回/@main>");
                    return;// 如已开通元宝服务则退出
                }
                //if (!GetValValue(PlayObject, QuestActionInfo.sParam1, ref nGameGold))
                //{
                //    // 增加变量支持
                nGameGold = HUtil32.Str_ToInt(GetLineVariableText(PlayObject, QuestActionInfo.sParam1), 0);
                //}
                if (PlayObject.m_nGameGold >= nGameGold)
                {
                    // 玩家的元宝数大于或等于开通所需的元宝数
                    PlayObject.m_nGameGold -= nGameGold;
                    PlayObject.bo_YBDEAL = true;
                    PlayObject.SendMsg(this, Grobal2.RM_MERCHANTSAY, 0, 0, 0, 0, this.m_sCharName + "/开通寄售服务成功！！！\\ \\<返回/@main>");
                }
                else
                {
                    PlayObject.SendMsg(this, Grobal2.RM_MERCHANTSAY, 0, 0, 0, 0, this.m_sCharName + "/您身上没有" + M2Share.g_Config.sGameGoldName + ",或" + M2Share.g_Config.sGameGoldName + "数不够！！！\\ \\<返回/@main>");
                }
            }
            catch
            {
                M2Share.MainOutMessage("{异常} TNormNpc.ActionOfOPENYBDEAL");
            }
        }

        /// <summary>
        /// 查询正在出售的物品
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="QuestActionInfo"></param>
        private void ActionOfQUERYYBSELL(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            TDealOffInfo DealOffInfo;
            string sSendStr;
            string sUserItemName;
            TClientDealOffInfo sClientDealOffInfo = null;
            MirItem StdItem;
            MirItem StdItem80;
            bool bo12;
            try
            {
                bo12 = false;
                if (PlayObject.bo_YBDEAL) // 已开通元宝服务
                {
                    if (PlayObject.SellOffInTime(0))
                    {
                        if (M2Share.sSellOffItemList.Count > 0)
                        {
                            sClientDealOffInfo = new TClientDealOffInfo();
                            sClientDealOffInfo.UseItems = new TClientItem[9];
                            for (var i = 0; i < M2Share.sSellOffItemList.Count; i++)
                            {
                                DealOffInfo = M2Share.sSellOffItemList[i];
                                if (((DealOffInfo.sDealCharName).ToLower().CompareTo((PlayObject.m_sCharName).ToLower()) == 0) && (new ArrayList(new byte[] { 0, 3 }).Contains(DealOffInfo.N)))
                                {
                                    for (var j = 0; j < 9; j++)
                                    {
                                        if (DealOffInfo.UseItems[j] == null)
                                        {
                                            continue;
                                        }
                                        StdItem = M2Share.UserEngine.GetStdItem(DealOffInfo.UseItems[j].wIndex);
                                        if ((StdItem == null))
                                        {
                                            // 是金刚石
                                            if (!bo12 && (DealOffInfo.UseItems[j].MakeIndex > 0) && (DealOffInfo.UseItems[j].wIndex == UInt16.MaxValue) && (DealOffInfo.UseItems[j].Dura == UInt16.MaxValue) && (DealOffInfo.UseItems[j].DuraMax == UInt16.MaxValue))
                                            {
                                                TClientItem _wvar1 = sClientDealOffInfo.UseItems[j];// '金刚石'
                                                //_wvar1.S.Name = M2Share.g_Config.sGameDiaMond + '(' + (DealOffInfo.UseItems[K].MakeIndex).ToString() + ')';
                                                //_wvar1.S.Price = DealOffInfo.UseItems[K].MakeIndex;// 金刚石数量
                                                _wvar1.Dura = UInt16.MaxValue;// 客户端金刚石特征
                                                _wvar1.S.DuraMax = Int16.MaxValue;// 客户端金刚石特征
                                                _wvar1.S.Looks = UInt16.MaxValue;// 不显示图片
                                                bo12 = true;
                                            }
                                            else
                                            {
                                                sClientDealOffInfo.UseItems[j].S.Name = "";
                                            }
                                            continue;
                                        }
                                        StdItem80 = StdItem;
                                        //M2Share.ItemUnit.GetItemAddValue(DealOffInfo.UseItems[K], ref StdItem80);
                                        //Move(StdItem80, sClientDealOffInfo.UseItems[K].S, sizeof(TStdItem));
                                        sClientDealOffInfo.UseItems[j] = new TClientItem();
                                        StdItem80.GetStandardItem(ref sClientDealOffInfo.UseItems[j].S);
                                        //sClientDealOffInfo.UseItems[j].S = StdItem80;
                                        // 取自定义物品名称
                                        sUserItemName = "";
                                        if (DealOffInfo.UseItems[j].btValue[13] == 1)
                                        {
                                            sUserItemName = M2Share.ItemUnit.GetCustomItemName(DealOffInfo.UseItems[j].MakeIndex, DealOffInfo.UseItems[j].wIndex);
                                        }
                                        if (sUserItemName != "")
                                        {
                                            sClientDealOffInfo.UseItems[j].S.Name = sUserItemName;
                                        }
                                        sClientDealOffInfo.UseItems[j].MakeIndex = DealOffInfo.UseItems[j].MakeIndex;
                                        sClientDealOffInfo.UseItems[j].Dura = DealOffInfo.UseItems[j].Dura;
                                        sClientDealOffInfo.UseItems[j].DuraMax = DealOffInfo.UseItems[j].DuraMax;
                                        switch (StdItem.StdMode)
                                        {
                                            // if StdItem.StdMode = 50 then 
                                            // sClientDealOffInfo.UseItems[K].s.Name := sClientDealOffInfo.UseItems[K].s.Name + ' #' + IntToStr(DealOffInfo.UseItems[K].Dura);
                                            // Modify the A .. B: 15, 19 .. 24, 26
                                            case 15:
                                            case 19:
                                            case 26:
                                                if (DealOffInfo.UseItems[j].btValue[8] != 0)
                                                {
                                                    sClientDealOffInfo.UseItems[j].S.Shape = 130;
                                                }
                                                break;
                                        }
                                    }
                                    sClientDealOffInfo.sDealCharName = DealOffInfo.sDealCharName;
                                    sClientDealOffInfo.sBuyCharName = DealOffInfo.sBuyCharName;
                                    sClientDealOffInfo.dSellDateTime = HUtil32.DateTimeToDouble(DealOffInfo.dSellDateTime);
                                    sClientDealOffInfo.nSellGold = DealOffInfo.nSellGold;
                                    sClientDealOffInfo.N = DealOffInfo.N;
                                    sSendStr = EDcode.EncodeBuffer(sClientDealOffInfo);
                                    PlayObject.SendMsg(this, Grobal2.RM_QUERYYBSELL, 0, 0, 0, 0, sSendStr);
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        GotoLable(PlayObject, "@AskYBSellFail", false);
                    }
                }
                else
                {
                    PlayObject.SendMsg(PlayObject, Grobal2.RM_MENU_OK, 0, PlayObject.ObjectId, 0, 0, "您未开通寄售服务,请先开通！！！");
                }
            }
            catch
            {
                M2Share.MainOutMessage("{异常} TNormNpc.ActionOfQUERYYBSELL");
            }
        }

        /// <summary>
        /// 查询可以的购买物品
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="QuestActionInfo"></param>
        private void ActionOfQUERYYBDEAL(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            TDealOffInfo DealOffInfo;
            string sSendStr;
            string sUserItemName;
            TClientDealOffInfo sClientDealOffInfo = null;
            MirItem StdItem;
            MirItem StdItem80;
            bool bo12;
            try
            {
                bo12 = false;
                if (PlayObject.bo_YBDEAL)
                {
                    // 已开通元宝服务
                    if (PlayObject.SellOffInTime(1))
                    {
                        if (M2Share.sSellOffItemList.Count > 0)
                        {
                            sClientDealOffInfo = new TClientDealOffInfo();
                            sClientDealOffInfo.UseItems = new TClientItem[9];
                            for (var i = 0; i < M2Share.sSellOffItemList.Count; i++)
                            {
                                DealOffInfo = M2Share.sSellOffItemList[i];
                                if (((DealOffInfo.sBuyCharName).ToLower().CompareTo((PlayObject.m_sCharName).ToLower()) == 0) && (DealOffInfo.N == 0))
                                {
                                    for (var k = 0; k < 9; k++)
                                    {
                                        if (DealOffInfo.UseItems[k] == null)
                                        {
                                            continue;
                                        }
                                        StdItem = M2Share.UserEngine.GetStdItem(DealOffInfo.UseItems[k].wIndex);
                                        if (StdItem == null)
                                        {
                                            // 是金刚石
                                            if (!bo12 && (DealOffInfo.UseItems[k].MakeIndex > 0) && (DealOffInfo.UseItems[k].wIndex == Int16.MaxValue) && (DealOffInfo.UseItems[k].Dura == Int16.MaxValue) && (DealOffInfo.UseItems[k].DuraMax == Int16.MaxValue))
                                            {
                                                TClientItem _wvar1 = sClientDealOffInfo.UseItems[k];// '金刚石'
                                                //_wvar1.S.Name = M2Share.g_Config.sGameDiaMond + '(' + (DealOffInfo.UseItems[K].MakeIndex).ToString() + ')';
                                                //_wvar1.S.Price = DealOffInfo.UseItems[K].MakeIndex;
                                                //// 金刚石数量
                                                //_wvar1.Dura = UInt16.MaxValue;// 客户端金刚石特征
                                                //_wvar1.S.DuraMax = Int16.MaxValue;// 客户端金刚石特征
                                                //_wvar1.S.Looks = UInt16.MaxValue;// 不显示图片
                                                bo12 = true;
                                            }
                                            else
                                            {
                                                sClientDealOffInfo.UseItems[k].S.Name = "";
                                            }
                                            continue;
                                        }
                                        StdItem80 = StdItem;
                                        //M2Share.ItemUnit.GetItemAddValue(DealOffInfo.UseItems[K], ref StdItem80);
                                        //Move(StdItem80, sClientDealOffInfo.UseItems[K].S);// 取自定义物品名称
                                        //sClientDealOffInfo.UseItems[K].S = StdItem80;
                                        sClientDealOffInfo.UseItems[k] = new TClientItem();
                                        StdItem80.GetStandardItem(ref sClientDealOffInfo.UseItems[k].S);
                                        sUserItemName = "";
                                        if (DealOffInfo.UseItems[k].btValue[13] == 1)
                                        {
                                            sUserItemName = M2Share.ItemUnit.GetCustomItemName(DealOffInfo.UseItems[k].MakeIndex, DealOffInfo.UseItems[k].wIndex);
                                        }
                                        if (sUserItemName != "")
                                        {
                                            sClientDealOffInfo.UseItems[k].S.Name = sUserItemName;
                                        }
                                        sClientDealOffInfo.UseItems[k].MakeIndex = DealOffInfo.UseItems[k].MakeIndex;
                                        sClientDealOffInfo.UseItems[k].Dura = DealOffInfo.UseItems[k].Dura;
                                        sClientDealOffInfo.UseItems[k].DuraMax = DealOffInfo.UseItems[k].DuraMax;
                                        switch (StdItem.StdMode)
                                        {
                                            // Modify the A .. B: 15, 19 .. 24, 26
                                            case 15:
                                            case 19:
                                            case 26:
                                                if (DealOffInfo.UseItems[k].btValue[8] != 0)
                                                {
                                                    sClientDealOffInfo.UseItems[k].S.Shape = 130;
                                                }
                                                break;
                                        }
                                    }
                                    sClientDealOffInfo.sDealCharName = DealOffInfo.sDealCharName;
                                    sClientDealOffInfo.sBuyCharName = DealOffInfo.sBuyCharName;
                                    sClientDealOffInfo.dSellDateTime = HUtil32.DateTimeToDouble(DealOffInfo.dSellDateTime);
                                    sClientDealOffInfo.nSellGold = DealOffInfo.nSellGold;
                                    sClientDealOffInfo.N = DealOffInfo.N;
                                    sSendStr = EDcode.EncodeBuffer(sClientDealOffInfo);
                                    PlayObject.SendMsg(this, Grobal2.RM_QUERYYBDEAL, 0, 0, 0, 0, sSendStr);
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        GotoLable(PlayObject, "@AskYBDealFail", false);
                    }
                }
                else
                {
                    PlayObject.SendMsg(PlayObject, Grobal2.RM_MENU_OK, 0, PlayObject.ObjectId, 0, 0, "您未开通元宝寄售服务,请先开通！！！");
                }
            }
            catch
            {
                M2Share.MainOutMessage("{异常} TNormNpc.ActionOfQUERYYBDEAL");
            }
        }

        private void ActionOfAddNameDateList(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            string sLineText;
            var sHumName = string.Empty;
            var sDate = string.Empty;
            var sListFileName = M2Share.g_Config.sEnvirDir + m_sPath + QuestActionInfo.sParam1;
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
            var boFound = false;
            for (var i = 0; i < LoadList.Count; i++)
            {
                sLineText = LoadList[i].Trim();
                sLineText = HUtil32.GetValidStr3(sLineText, ref sHumName, new string[] { " ", "\t" });
                sLineText = HUtil32.GetValidStr3(sLineText, ref sDate, new string[] { " ", "\t" });
                if (string.Compare(sHumName.ToLower(), PlayObject.m_sCharName.ToLower(), StringComparison.Ordinal) == 0)
                {
                    LoadList[i] = PlayObject.m_sCharName + "\t" + DateTime.Today;
                    boFound = true;
                    break;
                }
            }
            if (!boFound)
            {
                LoadList.Add(PlayObject.m_sCharName + "\t" + DateTime.Today);
            }
            try
            {
                LoadList.SaveToFile(sListFileName);
            }
            catch
            {
                M2Share.MainOutMessage("saving fail.... => " + sListFileName);
            }
            //LoadList.Free;
        }

        private void ActionOfDelNameDateList(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            string sLineText;
            string sHumName = string.Empty;
            string sDate = string.Empty;
            var sListFileName = M2Share.g_Config.sEnvirDir + m_sPath + QuestActionInfo.sParam1;
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
            var boFound = false;
            for (var i = 0; i < LoadList.Count; i++)
            {
                sLineText = LoadList[i].Trim();
                sLineText = HUtil32.GetValidStr3(sLineText, ref sHumName, new string[] { " ", "\t" });
                sLineText = HUtil32.GetValidStr3(sLineText, ref sDate, new string[] { " ", "\t" });
                if (sHumName.ToLower().CompareTo(PlayObject.m_sCharName.ToLower()) == 0)
                {
                    LoadList.RemoveAt(i);
                    boFound = true;
                    break;
                }
            }
            if (boFound)
            {
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

        private void ActionOfAddSkill(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            var nLevel = HUtil32._MIN(3, HUtil32.Str_ToInt(QuestActionInfo.sParam2, 0));
            var Magic = M2Share.UserEngine.FindMagic(QuestActionInfo.sParam1);
            if (Magic != null)
            {
                if (!PlayObject.IsTrainingSkill(Magic.wMagicID))
                {
                    var UserMagic = new TUserMagic();
                    UserMagic.MagicInfo = Magic;
                    UserMagic.wMagIdx = Magic.wMagicID;
                    UserMagic.btKey = 0;
                    UserMagic.btLevel = (byte)nLevel;
                    UserMagic.nTranPoint = 0;
                    PlayObject.m_MagicList.Add(UserMagic);
                    PlayObject.SendAddMagic(UserMagic);
                    PlayObject.RecalcAbilitys();
                    if (M2Share.g_Config.boShowScriptActionMsg)
                    {
                        PlayObject.SysMsg(Magic.sMagicName + "练习成功。", TMsgColor.c_Green, TMsgType.t_Hint);
                    }
                }
            }
            else
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_ADDSKILL);
            }
        }

        private void ActionOfAutoAddGameGold(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo, int nPoint, int nTime)
        {
            if (QuestActionInfo.sParam1.ToLower().CompareTo("START".ToLower()) == 0)
            {
                if (nPoint > 0 && nTime > 0)
                {
                    PlayObject.m_nIncGameGold = nPoint;
                    PlayObject.m_dwIncGameGoldTime = nTime * 1000;
                    PlayObject.m_dwIncGameGoldTick = HUtil32.GetTickCount();
                    PlayObject.m_boIncGameGold = true;
                    return;
                }
            }
            if (QuestActionInfo.sParam1.ToLower().CompareTo("STOP".ToLower()) == 0)
            {
                PlayObject.m_boIncGameGold = false;
                return;
            }
            ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_AUTOADDGAMEGOLD);
        }

        // SETAUTOGETEXP 时间 点数 是否安全区 地图号
        private void ActionOfAutoGetExp(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            TEnvirnoment Envir = null;
            var nTime = HUtil32.Str_ToInt(QuestActionInfo.sParam1, -1);
            var nPoint = HUtil32.Str_ToInt(QuestActionInfo.sParam2, -1);
            var boIsSafeZone = QuestActionInfo.sParam3[1] == '1';
            var sMap = QuestActionInfo.sParam4;
            if (sMap != "")
            {
                Envir = M2Share.g_MapManager.FindMap(sMap);
            }
            if ((nTime <= 0) || (nPoint <= 0) || ((sMap != "") && (Envir == null)))
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_SETAUTOGETEXP);
                return;
            }
            PlayObject.m_boAutoGetExpInSafeZone = boIsSafeZone;
            PlayObject.m_AutoGetExpEnvir = Envir;
            PlayObject.m_nAutoGetExpTime = nTime * 1000;
            PlayObject.m_nAutoGetExpPoint = nPoint;
        }

        /// <summary>
        /// 增加挂机
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="QuestActionInfo"></param>
        private void ActionOfOffLine(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            var sOffLineStartMsg = "系统已经为你开启了脱机泡点功能，你现在可以下线了……";
            PlayObject.m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_SYSMESSAGE, PlayObject.ObjectId, HUtil32.MakeWord(M2Share.g_Config.btCustMsgFColor, M2Share.g_Config.btCustMsgBColor), 0, 1);
            PlayObject.SendSocket(PlayObject.m_DefMsg, EDcode.EncodeString(sOffLineStartMsg));
            var nTime = HUtil32.Str_ToInt(QuestActionInfo.sParam1, 5);
            var nPoint = HUtil32.Str_ToInt(QuestActionInfo.sParam2, 500);
            var nKickOffLine = HUtil32.Str_ToInt(QuestActionInfo.sParam3, 1440 * 15);
            PlayObject.m_boAutoGetExpInSafeZone = true;
            PlayObject.m_AutoGetExpEnvir = PlayObject.m_PEnvir;
            PlayObject.m_nAutoGetExpTime = nTime * 1000;
            PlayObject.m_nAutoGetExpPoint = nPoint;
            PlayObject.m_boOffLineFlag = true;
            PlayObject.m_dwKickOffLineTick = HUtil32.GetTickCount() + (nKickOffLine * 60 * 1000);
            IdSrvClient.Instance.SendHumanLogOutMsgA(PlayObject.m_sUserID, PlayObject.m_nSessionID);
            PlayObject.SendDefMessage(Grobal2.SM_OUTOFCONNECTION, 0, 0, 0, 0, "");
        }

        private void ActionOfAutoSubGameGold(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo, int nPoint, int nTime)
        {
            if (QuestActionInfo.sParam1.ToLower().CompareTo("START".ToLower()) == 0)
            {
                if ((nPoint > 0) && (nTime > 0))
                {
                    PlayObject.m_nDecGameGold = nPoint;
                    PlayObject.m_dwDecGameGoldTime = nTime * 1000;
                    PlayObject.m_dwDecGameGoldTick = 0;
                    PlayObject.m_boDecGameGold = true;
                    return;
                }
            }
            if (QuestActionInfo.sParam1.ToLower().CompareTo("STOP".ToLower()) == 0)
            {
                PlayObject.m_boDecGameGold = false;
                return;
            }
            ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_AUTOSUBGAMEGOLD);
        }

        private void ActionOfChangeCreditPoint(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            int nCreditPoint = HUtil32.Str_ToInt(QuestActionInfo.sParam2, -1);
            if (nCreditPoint < 0)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_CREDITPOINT);
                return;
            }
            var cMethod = QuestActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (nCreditPoint >= 0)
                    {
                        if (nCreditPoint > byte.MaxValue)
                        {
                            PlayObject.m_btCreditPoint = byte.MaxValue;
                        }
                        else
                        {
                            PlayObject.m_btCreditPoint = (byte)nCreditPoint;
                        }
                    }
                    break;
                case '-':
                    if (PlayObject.m_btCreditPoint > (byte)nCreditPoint)
                    {
                        PlayObject.m_btCreditPoint -= (byte)nCreditPoint;
                    }
                    else
                    {
                        PlayObject.m_btCreditPoint = 0;
                    }
                    break;
                case '+':
                    if (PlayObject.m_btCreditPoint + (byte)nCreditPoint > byte.MaxValue)
                    {
                        PlayObject.m_btCreditPoint = byte.MaxValue;
                    }
                    else
                    {
                        PlayObject.m_btCreditPoint += (byte)nCreditPoint;
                    }
                    break;
                default:
                    ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_CREDITPOINT);
                    return;
            }
        }

        private void ActionOfChangeExp(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            int dwInt;
            var nExp = HUtil32.Str_ToInt(QuestActionInfo.sParam2, -1);
            if (nExp < 0)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_CHANGEEXP);
                return;
            }
            var cMethod = QuestActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (nExp >= 0)
                    {
                        PlayObject.m_Abil.Exp = nExp;
                        dwInt = nExp;
                    }
                    break;
                case '-':
                    if (PlayObject.m_Abil.Exp > nExp)
                    {
                        PlayObject.m_Abil.Exp -= nExp;
                    }
                    else
                    {
                        PlayObject.m_Abil.Exp = 0;
                    }
                    break;
                case '+':
                    if (PlayObject.m_Abil.Exp >= nExp)
                    {
                        if ((PlayObject.m_Abil.Exp - nExp) > (int.MaxValue - PlayObject.m_Abil.Exp))
                        {
                            dwInt = int.MaxValue - PlayObject.m_Abil.Exp;
                        }
                        else
                        {
                            dwInt = nExp;
                        }
                    }
                    else
                    {
                        if ((nExp - PlayObject.m_Abil.Exp) > (int.MaxValue - nExp))
                        {
                            dwInt = int.MaxValue - nExp;
                        }
                        else
                        {
                            dwInt = nExp;
                        }
                    }
                    PlayObject.m_Abil.Exp += dwInt;
                    // PlayObject.GetExp(dwInt);
                    PlayObject.SendMsg(PlayObject, Grobal2.RM_WINEXP, 0, dwInt, 0, 0, "");
                    break;
            }
        }

        private void ActionOfChangeHairStyle(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            var nHair = HUtil32.Str_ToInt(QuestActionInfo.sParam1, -1);
            if ((QuestActionInfo.sParam1 != "") && (nHair >= 0))
            {
                PlayObject.m_btHair = (byte)nHair;
                PlayObject.FeatureChanged();
            }
            else
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_HAIRSTYLE);
            }
        }

        private void ActionOfChangeJob(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            var nJob = -1;
            if (HUtil32.CompareLStr(QuestActionInfo.sParam1, M2Share.sWarrior, M2Share.sWarrior.Length))
            {
                nJob = M2Share.jWarr;
            }
            if (HUtil32.CompareLStr(QuestActionInfo.sParam1, M2Share.sWizard, M2Share.sWizard.Length))
            {
                nJob = M2Share.jWizard;
            }
            if (HUtil32.CompareLStr(QuestActionInfo.sParam1, M2Share.sTaos, M2Share.sTaos.Length))
            {
                nJob = M2Share.jTaos;
            }
            if (nJob < 0)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_CHANGEJOB);
                return;
            }
            if (PlayObject.m_btJob != nJob)
            {
                PlayObject.m_btJob = (byte)nJob;
                // 
                // PlayObject.RecalcLevelAbilitys();
                // PlayObject.RecalcAbilitys();
                // 
                PlayObject.HasLevelUp(0);
            }
        }

        private void ActionOfChangeLevel(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            int nLv;
            var boChgOK = false;
            int nOldLevel = PlayObject.m_Abil.Level;
            var nLevel = HUtil32.Str_ToInt(QuestActionInfo.sParam2, -1);
            if (nLevel < 0)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_CHANGELEVEL);
                return;
            }
            var cMethod = QuestActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if ((nLevel > 0) && (nLevel <= Grobal2.MAXLEVEL))
                    {
                        PlayObject.m_Abil.Level = (ushort)nLevel;
                        boChgOK = true;
                    }
                    break;
                case '-':
                    nLv = HUtil32._MAX(0, PlayObject.m_Abil.Level - nLevel);
                    nLv = HUtil32._MIN(Grobal2.MAXLEVEL, nLv);
                    PlayObject.m_Abil.Level = (ushort)nLv;
                    boChgOK = true;
                    break;
                case '+':
                    nLv = HUtil32._MAX(0, PlayObject.m_Abil.Level + nLevel);
                    nLv = HUtil32._MIN(Grobal2.MAXLEVEL, nLv);
                    PlayObject.m_Abil.Level = (ushort)nLv;
                    boChgOK = true;
                    break;
            }
            if (boChgOK)
            {
                PlayObject.HasLevelUp(nOldLevel);
            }
        }

        private void ActionOfChangePkPoint(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            int nPoint;
            var nOldPKLevel = PlayObject.PKLevel();
            var nPKPoint = HUtil32.Str_ToInt(QuestActionInfo.sParam2, -1);
            if (nPKPoint < 0)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_CHANGEPKPOINT);
                return;
            }
            var cMethod = QuestActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (nPKPoint >= 0)
                    {
                        PlayObject.m_nPkPoint = nPKPoint;
                    }
                    break;
                case '-':
                    nPoint = HUtil32._MAX(0, PlayObject.m_nPkPoint - nPKPoint);
                    PlayObject.m_nPkPoint = nPoint;
                    break;
                case '+':
                    nPoint = HUtil32._MAX(0, PlayObject.m_nPkPoint + nPKPoint);
                    PlayObject.m_nPkPoint = nPoint;
                    break;
            }
            if (nOldPKLevel != PlayObject.PKLevel())
            {
                PlayObject.RefNameColor();
            }
        }

        private void ActionOfClearMapMon(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            TBaseObject Mon;
            IList<TBaseObject> MonList = new List<TBaseObject>();
            M2Share.UserEngine.GetMapMonster(M2Share.g_MapManager.FindMap(QuestActionInfo.sParam1), MonList);
            for (var i = 0; i < MonList.Count; i++)
            {
                Mon = MonList[i];
                if (Mon.m_Master != null)
                {
                    continue;
                }
                if (M2Share.GetNoClearMonList(Mon.m_sCharName))
                {
                    continue;
                }
                Mon.m_boNoItem = true;
                Mon.MakeGhost();
            }
        }

        private void ActionOfClearNameList(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            var sListFileName = M2Share.g_Config.sEnvirDir + m_sPath + QuestActionInfo.sParam1;
            var LoadList = new StringList();
            LoadList.Clear();
            try
            {
                LoadList.SaveToFile(sListFileName);
            }
            catch
            {
                M2Share.MainOutMessage("saving fail.... => " + sListFileName);
            }
            //LoadList.Free;
        }

        private void ActionOfClearSkill(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            TUserMagic UserMagic;
            for (var i = PlayObject.m_MagicList.Count - 1; i >= 0; i--)
            {
                UserMagic = PlayObject.m_MagicList[i];
                PlayObject.SendDelMagic(UserMagic);
                Dispose(UserMagic);
                PlayObject.m_MagicList.RemoveAt(i);
            }
            PlayObject.RecalcAbilitys();
        }

        private void ActionOfDelNoJobSkill(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            TUserMagic UserMagic;
            for (var i = PlayObject.m_MagicList.Count - 1; i >= 0; i--)
            {
                UserMagic = PlayObject.m_MagicList[i];
                if (UserMagic.MagicInfo.btJob != PlayObject.m_btJob)
                {
                    PlayObject.SendDelMagic(UserMagic);
                    Dispose(UserMagic);
                    PlayObject.m_MagicList.RemoveAt(i);
                }
            }
        }

        private void ActionOfDelSkill(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            TUserMagic UserMagic;
            var sMagicName = QuestActionInfo.sParam1;
            var Magic = M2Share.UserEngine.FindMagic(sMagicName);
            if (Magic == null)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_DELSKILL);
                return;
            }
            for (var i = 0; i < PlayObject.m_MagicList.Count; i++)
            {
                UserMagic = PlayObject.m_MagicList[i];
                if (UserMagic.MagicInfo == Magic)
                {
                    PlayObject.m_MagicList.RemoveAt(i);
                    PlayObject.SendDelMagic(UserMagic);
                    Dispose(UserMagic);
                    PlayObject.RecalcAbilitys();
                    break;
                }
            }
        }

        private void ActionOfGameGold(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            var nOldGameGold = PlayObject.m_nGameGold;
            var nGameGold = HUtil32.Str_ToInt(QuestActionInfo.sParam2, -1);
            if (nGameGold < 0)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_GAMEGOLD);
                return;
            }
            var cMethod = QuestActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (nGameGold >= 0)
                    {
                        PlayObject.m_nGameGold = nGameGold;
                    }
                    break;
                case '-':
                    nGameGold = HUtil32._MAX(0, PlayObject.m_nGameGold - nGameGold);
                    PlayObject.m_nGameGold = nGameGold;
                    break;
                case '+':
                    nGameGold = HUtil32._MAX(0, PlayObject.m_nGameGold + nGameGold);
                    PlayObject.m_nGameGold = nGameGold;
                    break;
            }
            if (M2Share.g_boGameLogGameGold)
            {
                M2Share.AddGameDataLog(format(M2Share.g_sGameLogMsg1, Grobal2.LOG_GAMEGOLD, PlayObject.m_sMapName, PlayObject.m_nCurrX, PlayObject.m_nCurrY, PlayObject.m_sCharName, M2Share.g_Config.sGameGoldName, nGameGold, cMethod, this.m_sCharName));
            }
            if (nOldGameGold != PlayObject.m_nGameGold)
            {
                PlayObject.GameGoldChanged();
            }
        }

        private void ActionOfGamePoint(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            var nOldGamePoint = PlayObject.m_nGamePoint;
            var nGamePoint = HUtil32.Str_ToInt(QuestActionInfo.sParam2, -1);
            if (nGamePoint < 0)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_GAMEPOINT);
                return;
            }
            var cMethod = QuestActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (nGamePoint >= 0)
                    {
                        PlayObject.m_nGamePoint = nGamePoint;
                    }
                    break;
                case '-':
                    nGamePoint = HUtil32._MAX(0, PlayObject.m_nGamePoint - nGamePoint);
                    PlayObject.m_nGamePoint = nGamePoint;
                    break;
                case '+':
                    nGamePoint = HUtil32._MAX(0, PlayObject.m_nGamePoint + nGamePoint);
                    PlayObject.m_nGamePoint = nGamePoint;
                    break;
            }
            if (M2Share.g_boGameLogGamePoint)
            {
                M2Share.AddGameDataLog(format(M2Share.g_sGameLogMsg1, new object[] { Grobal2.LOG_GAMEPOINT, PlayObject.m_sMapName, PlayObject.m_nCurrX, PlayObject.m_nCurrY, PlayObject.m_sCharName, M2Share.g_Config.sGamePointName, nGamePoint, cMethod, this.m_sCharName }));
            }
            if (nOldGamePoint != PlayObject.m_nGamePoint)
            {
                PlayObject.GameGoldChanged();
            }
        }

        private void ActionOfGetMarry(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            var PoseBaseObject = PlayObject.GetPoseCreate();
            if ((PoseBaseObject != null) && (PoseBaseObject.m_btRaceServer == Grobal2.RC_PLAYOBJECT) && (PoseBaseObject.m_btGender != PlayObject.m_btGender))
            {
                PlayObject.m_sDearName = PoseBaseObject.m_sCharName;
                PlayObject.RefShowName();
                PoseBaseObject.RefShowName();
            }
            else
            {
                GotoLable(PlayObject, "@MarryError", false);
            }
        }

        private void ActionOfGetMaster(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            var PoseBaseObject = PlayObject.GetPoseCreate();
            if ((PoseBaseObject != null) && (PoseBaseObject.m_btRaceServer == Grobal2.RC_PLAYOBJECT) && (PoseBaseObject.m_btGender != PlayObject.m_btGender))
            {
                PlayObject.m_sMasterName = PoseBaseObject.m_sCharName;
                PlayObject.RefShowName();
                PoseBaseObject.RefShowName();
            }
            else
            {
                GotoLable(PlayObject, "@MasterError", false);
            }
        }

        private void ActionOfLineMsg(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            var sMsg = GetLineVariableText(PlayObject, QuestActionInfo.sParam2);
            sMsg = sMsg.Replace("%s", PlayObject.m_sCharName);
            sMsg = sMsg.Replace("%d", this.m_sCharName);
            switch (QuestActionInfo.nParam1)
            {
                case 0:
                    M2Share.UserEngine.SendBroadCastMsg(sMsg, TMsgType.t_System);
                    break;
                case 1:
                    M2Share.UserEngine.SendBroadCastMsg("(*) " + sMsg, TMsgType.t_System);
                    break;
                case 2:
                    M2Share.UserEngine.SendBroadCastMsg('[' + this.m_sCharName + ']' + sMsg, TMsgType.t_System);
                    break;
                case 3:
                    M2Share.UserEngine.SendBroadCastMsg('[' + PlayObject.m_sCharName + ']' + sMsg, TMsgType.t_System);
                    break;
                case 4:
                    this.ProcessSayMsg(sMsg);
                    break;
                case 5:
                    PlayObject.SysMsg(sMsg, TMsgColor.c_Red, TMsgType.t_Say);
                    break;
                case 6:
                    PlayObject.SysMsg(sMsg, TMsgColor.c_Green, TMsgType.t_Say);
                    break;
                case 7:
                    PlayObject.SysMsg(sMsg, TMsgColor.c_Blue, TMsgType.t_Say);
                    break;
                case 8:
                    PlayObject.SendGroupText(sMsg);
                    break;
                case 9:
                    if (PlayObject.m_MyGuild != null)
                    {
                        PlayObject.m_MyGuild.SendGuildMsg(sMsg);
                        M2Share.UserEngine.SendServerGroupMsg(Grobal2.SS_208, M2Share.nServerIndex, PlayObject.m_MyGuild.sGuildName + "/" + PlayObject.m_sCharName + "/" + sMsg);
                    }
                    break;
                default:
                    ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSENDMSG);
                    break;
            }
        }

        private void ActionOfMapTing(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
        }

        private void ActionOfMarry(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            string sSayMsg;
            if (PlayObject.m_sDearName != "")
            {
                return;
            }
            var PoseHuman = (TPlayObject)PlayObject.GetPoseCreate();
            if (PoseHuman == null)
            {
                GotoLable(PlayObject, "@MarryCheckDir", false);
                return;
            }
            if (QuestActionInfo.sParam1 == "")
            {
                if (PoseHuman.m_btRaceServer != Grobal2.RC_PLAYOBJECT)
                {
                    GotoLable(PlayObject, "@HumanTypeErr", false);
                    return;
                }
                if (PoseHuman.GetPoseCreate() == PlayObject)
                {
                    if (PlayObject.m_btGender != PoseHuman.m_btGender)
                    {
                        GotoLable(PlayObject, "@StartMarry", false);
                        GotoLable(PoseHuman, "@StartMarry", false);
                        if ((PlayObject.m_btGender == ObjBase.gMan) && (PoseHuman.m_btGender == ObjBase.gWoMan))
                        {
                            sSayMsg = M2Share.g_sStartMarryManMsg.Replace("%n", this.m_sCharName);
                            sSayMsg = sSayMsg.Replace("%s", PlayObject.m_sCharName);
                            sSayMsg = sSayMsg.Replace("%d", PoseHuman.m_sCharName);
                            M2Share.UserEngine.SendBroadCastMsg(sSayMsg, TMsgType.t_Say);
                            sSayMsg = M2Share.g_sStartMarryManAskQuestionMsg.Replace("%n", this.m_sCharName);
                            sSayMsg = sSayMsg.Replace("%s", PlayObject.m_sCharName);
                            sSayMsg = sSayMsg.Replace("%d", PoseHuman.m_sCharName);
                            M2Share.UserEngine.SendBroadCastMsg(sSayMsg, TMsgType.t_Say);
                        }
                        else if ((PlayObject.m_btGender == ObjBase.gWoMan) && (PoseHuman.m_btGender == ObjBase.gMan))
                        {
                            sSayMsg = M2Share.g_sStartMarryWoManMsg.Replace("%n", this.m_sCharName);
                            sSayMsg = sSayMsg.Replace("%s", PlayObject.m_sCharName);
                            sSayMsg = sSayMsg.Replace("%d", PoseHuman.m_sCharName);
                            M2Share.UserEngine.SendBroadCastMsg(sSayMsg, TMsgType.t_Say);
                            sSayMsg = M2Share.g_sStartMarryWoManAskQuestionMsg.Replace("%n", this.m_sCharName);
                            sSayMsg = sSayMsg.Replace("%s", PlayObject.m_sCharName);
                            sSayMsg = sSayMsg.Replace("%d", PoseHuman.m_sCharName);
                            M2Share.UserEngine.SendBroadCastMsg(sSayMsg, TMsgType.t_Say);
                        }
                        PlayObject.m_boStartMarry = true;
                        PoseHuman.m_boStartMarry = true;
                    }
                    else
                    {
                        GotoLable(PoseHuman, "@MarrySexErr", false);
                        GotoLable(PlayObject, "@MarrySexErr", false);
                    }
                }
                else
                {
                    GotoLable(PlayObject, "@MarryDirErr", false);
                    GotoLable(PoseHuman, "@MarryCheckDir", false);
                }
                return;
            }
            // sREQUESTMARRY
            if (QuestActionInfo.sParam1.ToLower().CompareTo("REQUESTMARRY".ToLower()) == 0)
            {
                if (PlayObject.m_boStartMarry && PoseHuman.m_boStartMarry)
                {
                    if ((PlayObject.m_btGender == ObjBase.gMan) && (PoseHuman.m_btGender == ObjBase.gWoMan))
                    {
                        sSayMsg = M2Share.g_sMarryManAnswerQuestionMsg.Replace("%n", this.m_sCharName);
                        sSayMsg = sSayMsg.Replace("%s", PlayObject.m_sCharName);
                        sSayMsg = sSayMsg.Replace("%d", PoseHuman.m_sCharName);
                        M2Share.UserEngine.SendBroadCastMsg(sSayMsg, TMsgType.t_Say);
                        sSayMsg = M2Share.g_sMarryManAskQuestionMsg.Replace("%n", this.m_sCharName);
                        sSayMsg = sSayMsg.Replace("%s", PlayObject.m_sCharName);
                        sSayMsg = sSayMsg.Replace("%d", PoseHuman.m_sCharName);
                        M2Share.UserEngine.SendBroadCastMsg(sSayMsg, TMsgType.t_Say);
                        GotoLable(PlayObject, "@WateMarry", false);
                        GotoLable(PoseHuman, "@RevMarry", false);
                    }
                }
                return;
            }
            // sRESPONSEMARRY
            if (QuestActionInfo.sParam1.ToLower().CompareTo("RESPONSEMARRY".ToLower()) == 0)
            {
                if ((PlayObject.m_btGender == ObjBase.gWoMan) && (PoseHuman.m_btGender == ObjBase.gMan))
                {
                    if (QuestActionInfo.sParam2.ToLower().CompareTo("OK".ToLower()) == 0)
                    {
                        if (PlayObject.m_boStartMarry && PoseHuman.m_boStartMarry)
                        {
                            sSayMsg = M2Share.g_sMarryWoManAnswerQuestionMsg.Replace("%n", this.m_sCharName);
                            sSayMsg = sSayMsg.Replace("%s", PlayObject.m_sCharName);
                            sSayMsg = sSayMsg.Replace("%d", PoseHuman.m_sCharName);
                            M2Share.UserEngine.SendBroadCastMsg(sSayMsg, TMsgType.t_Say);
                            sSayMsg = M2Share.g_sMarryWoManGetMarryMsg.Replace("%n", this.m_sCharName);
                            sSayMsg = sSayMsg.Replace("%s", PlayObject.m_sCharName);
                            sSayMsg = sSayMsg.Replace("%d", PoseHuman.m_sCharName);
                            M2Share.UserEngine.SendBroadCastMsg(sSayMsg, TMsgType.t_Say);
                            GotoLable(PlayObject, "@EndMarry", false);
                            GotoLable(PoseHuman, "@EndMarry", false);
                            PlayObject.m_boStartMarry = false;
                            PoseHuman.m_boStartMarry = false;
                            PlayObject.m_sDearName = PoseHuman.m_sCharName;
                            PlayObject.m_DearHuman = PoseHuman;
                            PoseHuman.m_sDearName = PlayObject.m_sCharName;
                            PoseHuman.m_DearHuman = PlayObject;
                            PlayObject.RefShowName();
                            PoseHuman.RefShowName();
                        }
                    }
                    else
                    {
                        if (PlayObject.m_boStartMarry && PoseHuman.m_boStartMarry)
                        {
                            GotoLable(PlayObject, "@EndMarryFail", false);
                            GotoLable(PoseHuman, "@EndMarryFail", false);
                            PlayObject.m_boStartMarry = false;
                            PoseHuman.m_boStartMarry = false;
                            sSayMsg = M2Share.g_sMarryWoManDenyMsg.Replace("%n", this.m_sCharName);
                            sSayMsg = sSayMsg.Replace("%s", PlayObject.m_sCharName);
                            sSayMsg = sSayMsg.Replace("%d", PoseHuman.m_sCharName);
                            M2Share.UserEngine.SendBroadCastMsg(sSayMsg, TMsgType.t_Say);
                            sSayMsg = M2Share.g_sMarryWoManCancelMsg.Replace("%n", this.m_sCharName);
                            sSayMsg = sSayMsg.Replace("%s", PlayObject.m_sCharName);
                            sSayMsg = sSayMsg.Replace("%d", PoseHuman.m_sCharName);
                            M2Share.UserEngine.SendBroadCastMsg(sSayMsg, TMsgType.t_Say);
                        }
                    }
                }
                return;
            }
        }

        private void ActionOfMaster(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            if (PlayObject.m_sMasterName != "")
            {
                return;
            }
            var PoseHuman = (TPlayObject)PlayObject.GetPoseCreate();
            if (PoseHuman == null)
            {
                GotoLable(PlayObject, "@MasterCheckDir", false);
                return;
            }
            if (QuestActionInfo.sParam1 == "")
            {
                if (PoseHuman.m_btRaceServer != Grobal2.RC_PLAYOBJECT)
                {
                    GotoLable(PlayObject, "@HumanTypeErr", false);
                    return;
                }
                if (PoseHuman.GetPoseCreate() == PlayObject)
                {
                    GotoLable(PlayObject, "@StartGetMaster", false);
                    GotoLable(PoseHuman, "@StartMaster", false);
                    PlayObject.m_boStartMaster = true;
                    PoseHuman.m_boStartMaster = true;
                }
                else
                {
                    GotoLable(PlayObject, "@MasterDirErr", false);
                    GotoLable(PoseHuman, "@MasterCheckDir", false);
                }
                return;
            }
            if (QuestActionInfo.sParam1.ToLower().CompareTo("REQUESTMASTER".ToLower()) == 0)
            {
                if (PlayObject.m_boStartMaster && PoseHuman.m_boStartMaster)
                {
                    PlayObject.m_PoseBaseObject = PoseHuman;
                    PoseHuman.m_PoseBaseObject = PlayObject;
                    GotoLable(PlayObject, "@WateMaster", false);
                    GotoLable(PoseHuman, "@RevMaster", false);
                }
                return;
            }
            if (QuestActionInfo.sParam1.ToLower().CompareTo("RESPONSEMASTER".ToLower()) == 0)
            {
                if (QuestActionInfo.sParam2.ToLower().CompareTo("OK".ToLower()) == 0)
                {
                    if ((PlayObject.m_PoseBaseObject == PoseHuman) && (PoseHuman.m_PoseBaseObject == PlayObject))
                    {
                        if (PlayObject.m_boStartMaster && PoseHuman.m_boStartMaster)
                        {
                            GotoLable(PlayObject, "@EndMaster", false);
                            GotoLable(PoseHuman, "@EndMaster", false);
                            PlayObject.m_boStartMaster = false;
                            PoseHuman.m_boStartMaster = false;
                            if (PlayObject.m_sMasterName == "")
                            {
                                PlayObject.m_sMasterName = PoseHuman.m_sCharName;
                                PlayObject.m_boMaster = true;
                            }
                            PlayObject.m_MasterList.Add(PoseHuman);
                            PoseHuman.m_sMasterName = PlayObject.m_sCharName;
                            PoseHuman.m_boMaster = false;
                            PlayObject.RefShowName();
                            PoseHuman.RefShowName();
                        }
                    }
                }
                else
                {
                    if (PlayObject.m_boStartMaster && PoseHuman.m_boStartMaster)
                    {
                        GotoLable(PlayObject, "@EndMasterFail", false);
                        GotoLable(PoseHuman, "@EndMasterFail", false);
                        PlayObject.m_boStartMaster = false;
                        PoseHuman.m_boStartMaster = false;
                    }
                }
                return;
            }
        }

        private void ActionOfMessageBox(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            PlayObject.SendMsg(this, Grobal2.RM_MENU_OK, 0, PlayObject.ObjectId, 0, 0, GetLineVariableText(PlayObject, QuestActionInfo.sParam1));
        }

        private void ActionOfMission(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            if ((QuestActionInfo.sParam1 != "") && (QuestActionInfo.nParam2 > 0) && (QuestActionInfo.nParam3 > 0))
            {
                M2Share.g_sMissionMap = QuestActionInfo.sParam1;
                M2Share.g_nMissionX = (short)QuestActionInfo.nParam2;
                M2Share.g_nMissionY = (short)QuestActionInfo.nParam3;
            }
            else
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_MISSION);
            }
        }

        // MOBFIREBURN MAP X Y TYPE TIME POINT
        private void ActionOfMobFireBurn(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            var sMap = QuestActionInfo.sParam1;
            var nX = HUtil32.Str_ToInt(QuestActionInfo.sParam2, -1);
            var nY = HUtil32.Str_ToInt(QuestActionInfo.sParam3, -1);
            var nType = HUtil32.Str_ToInt(QuestActionInfo.sParam4, -1);
            var nTime = HUtil32.Str_ToInt(QuestActionInfo.sParam5, -1);
            var nPoint = HUtil32.Str_ToInt(QuestActionInfo.sParam6, -1);
            if ((sMap == "") || (nX < 0) || (nY < 0) || (nType < 0) || (nTime < 0) || (nPoint < 0))
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_MOBFIREBURN);
                return;
            }
            var Envir = M2Share.g_MapManager.FindMap(sMap);
            if (Envir != null)
            {
                var OldEnvir = PlayObject.m_PEnvir;
                PlayObject.m_PEnvir = Envir;
                var FireBurnEvent = new TFireBurnEvent(PlayObject, nX, nY, nType, nTime * 1000, nPoint);
                M2Share.EventManager.AddEvent(FireBurnEvent);
                PlayObject.m_PEnvir = OldEnvir;
                return;
            }
            ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_MOBFIREBURN);
        }

        private void ActionOfMobPlace(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo, int nX, int nY, int nCount, int nRange)
        {
            short nRandX;
            short nRandY;
            TBaseObject Mon;
            for (var i = 0; i < nCount; i++)
            {
                nRandX = (short)(M2Share.RandomNumber.Random(nRange * 2 + 1) + (nX - nRange));
                nRandY = (short)(M2Share.RandomNumber.Random(nRange * 2 + 1) + (nY - nRange));
                Mon = M2Share.UserEngine.RegenMonsterByName(M2Share.g_sMissionMap, nRandX, nRandY, QuestActionInfo.sParam1);
                if (Mon != null)
                {
                    Mon.m_boMission = true;
                    Mon.m_nMissionX = M2Share.g_nMissionX;
                    Mon.m_nMissionY = M2Share.g_nMissionY;
                }
                else
                {
                    ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_MOBPLACE);
                    break;
                }
            }
        }

        private void ActionOfRecallGroupMembers(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
        }

        private void ActionOfSetRankLevelName(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            var sRankLevelName = QuestActionInfo.sParam1;
            if (sRankLevelName == "")
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_SKILLLEVEL);
                return;
            }
            PlayObject.m_sRankLevelName = sRankLevelName;
            PlayObject.RefShowName();
        }

        private void ActionOfSetScriptFlag(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            var nWhere = HUtil32.Str_ToInt(QuestActionInfo.sParam1, -1);
            var boFlag = HUtil32.Str_ToInt(QuestActionInfo.sParam2, -1) == 1;
            switch (nWhere)
            {
                case 0:
                    PlayObject.m_boSendMsgFlag = boFlag;
                    break;
                case 1:
                    PlayObject.m_boChangeItemNameFlag = boFlag;
                    break;
                default:
                    ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_SETSCRIPTFLAG);
                    break;
            }
        }

        private void ActionOfSkillLevel(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            TUserMagic UserMagic;
            var nLevel = HUtil32.Str_ToInt(QuestActionInfo.sParam3, 0);
            if (nLevel < 0)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_SKILLLEVEL);
                return;
            }
            var cMethod = QuestActionInfo.sParam2[0];
            var Magic = M2Share.UserEngine.FindMagic(QuestActionInfo.sParam1);
            if (Magic != null)
            {
                for (var i = 0; i < PlayObject.m_MagicList.Count; i++)
                {
                    UserMagic = PlayObject.m_MagicList[i];
                    if (UserMagic.MagicInfo == Magic)
                    {
                        switch (cMethod)
                        {
                            case '=':
                                if (nLevel >= 0)
                                {
                                    nLevel = HUtil32._MAX(3, nLevel);
                                    UserMagic.btLevel = (byte)nLevel;
                                }
                                break;
                            case '-':
                                if (UserMagic.btLevel >= nLevel)
                                {
                                    UserMagic.btLevel -= (byte)nLevel;
                                }
                                else
                                {
                                    UserMagic.btLevel = 0;
                                }
                                break;
                            case '+':
                                if (UserMagic.btLevel + nLevel <= 3)
                                {
                                    UserMagic.btLevel += (byte)nLevel;
                                }
                                else
                                {
                                    UserMagic.btLevel = 3;
                                }
                                break;
                        }
                        PlayObject.SendDelayMsg(PlayObject, Grobal2.RM_MAGIC_LVEXP, 0, UserMagic.MagicInfo.wMagicID, UserMagic.btLevel, UserMagic.nTranPoint, "", 100);
                        break;
                    }
                }
            }
        }

        private void ActionOfTakeCastleGold(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            var nGold = HUtil32.Str_ToInt(QuestActionInfo.sParam1, -1);
            if ((nGold < 0) || (this.m_Castle == null))
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_TAKECASTLEGOLD);
                return;
            }
            if (nGold <= this.m_Castle.m_nTotalGold)
            {
                this.m_Castle.m_nTotalGold -= nGold;
            }
            else
            {
                this.m_Castle.m_nTotalGold = 0;
            }
        }

        private void ActionOfUnMarry(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            if (PlayObject.m_sDearName == "")
            {
                GotoLable(PlayObject, "@ExeMarryFail", false);
                return;
            }
            var PoseHuman = (TPlayObject)PlayObject.GetPoseCreate();
            if (PoseHuman == null)
            {
                GotoLable(PlayObject, "@UnMarryCheckDir", false);
            }
            if (PoseHuman != null)
            {
                if (QuestActionInfo.sParam1 == "")
                {
                    if (PoseHuman.m_btRaceServer != Grobal2.RC_PLAYOBJECT)
                    {
                        GotoLable(PlayObject, "@UnMarryTypeErr", false);
                        return;
                    }
                    if (PoseHuman.GetPoseCreate() == PlayObject)
                    {
                        // and (PosHum.AddInfo.sDearName = Hum.sName)
                        if (PlayObject.m_sDearName == PoseHuman.m_sCharName)
                        {
                            GotoLable(PlayObject, "@StartUnMarry", false);
                            GotoLable(PoseHuman, "@StartUnMarry", false);
                            return;
                        }
                    }
                }
            }
            // sREQUESTUNMARRY
            if (QuestActionInfo.sParam1.ToLower().CompareTo("REQUESTUNMARRY".ToLower()) == 0)
            {
                if (QuestActionInfo.sParam2 == "")
                {
                    if (PoseHuman != null)
                    {
                        PlayObject.m_boStartUnMarry = true;
                        if (PlayObject.m_boStartUnMarry && PoseHuman.m_boStartUnMarry)
                        {
                            // sUnMarryMsg8
                            // sMarryMsg0
                            // sUnMarryMsg9
                            M2Share.UserEngine.SendBroadCastMsg('[' + this.m_sCharName + "]: " + "我宣布" + PoseHuman.m_sCharName + ' ' + '与' + PlayObject.m_sCharName + ' ' + ' ' + "正式脱离夫妻关系。", TMsgType.t_Say);
                            PlayObject.m_sDearName = "";
                            PoseHuman.m_sDearName = "";
                            PlayObject.m_btMarryCount++;
                            PoseHuman.m_btMarryCount++;
                            PlayObject.m_boStartUnMarry = false;
                            PoseHuman.m_boStartUnMarry = false;
                            PlayObject.RefShowName();
                            PoseHuman.RefShowName();
                            GotoLable(PlayObject, "@UnMarryEnd", false);
                            GotoLable(PoseHuman, "@UnMarryEnd", false);
                        }
                        else
                        {
                            GotoLable(PlayObject, "@WateUnMarry", false);
                            // GotoLable(PoseHuman,'@RevUnMarry',False);
                        }
                    }
                    return;
                }
                else
                {
                    // 强行离婚
                    if (QuestActionInfo.sParam2.ToLower().CompareTo("FORCE".ToLower()) == 0)
                    {
                        M2Share.UserEngine.SendBroadCastMsg('[' + this.m_sCharName + "]: " + "我宣布" + PlayObject.m_sCharName + ' ' + '与' + PlayObject.m_sDearName + ' ' + ' ' + "已经正式脱离夫妻关系！！！", TMsgType.t_Say);
                        PoseHuman = M2Share.UserEngine.GetPlayObject(PlayObject.m_sDearName);
                        if (PoseHuman != null)
                        {
                            PoseHuman.m_sDearName = "";
                            PoseHuman.m_btMarryCount++;
                            PoseHuman.RefShowName();
                        }
                        else
                        {
                            //sUnMarryFileName = M2Share.g_Config.sEnvirDir + "UnMarry.txt";
                            //LoadList = new StringList();
                            //if (File.Exists(sUnMarryFileName))
                            //{
                            //    LoadList.LoadFromFile(sUnMarryFileName);
                            //}
                            //LoadList.Add(PlayObject.m_sDearName);
                            //LoadList.SaveToFile(sUnMarryFileName);
                            //LoadList.Free;
                        }
                        PlayObject.m_sDearName = "";
                        PlayObject.m_btMarryCount++;
                        GotoLable(PlayObject, "@UnMarryEnd", false);
                        PlayObject.RefShowName();
                    }
                    return;
                }
            }
        }

        // 保存变量值
        // SAVEVAR 变量类型 变量名 文件名
        private void ActionOfSaveVar(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            string sName = string.Empty;
            TDynamicVar DynamicVar = null;
            bool boFoundVar;
            IList<TDynamicVar> DynamicVarList;
            IniFile IniFile;
            const string sVarFound = "变量%s不存在，变量类型:%s";
            const string sVarTypeError = "变量类型错误，错误类型:%s 当前支持类型(HUMAN、GUILD、GLOBAL)";
            string sType = QuestActionInfo.sParam1;
            string sVarName = QuestActionInfo.sParam2;
            string sFileName = M2Share.g_Config.sEnvirDir + m_sPath + QuestActionInfo.sParam3;
            if ((sType == "") || (sVarName == "") || !File.Exists(sFileName))
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_SAVEVAR);
                return;
            }
            boFoundVar = false;
            DynamicVarList = GetDynamicVarList(PlayObject, sType, ref sName);
            if (DynamicVarList == null)
            {
                Dispose(DynamicVar);
                ScriptActionError(PlayObject, format(sVarTypeError, new string[] { sType }), QuestActionInfo, M2Share.sSC_VAR);
                return;
            }
            IniFile = new IniFile(sFileName);
            for (var i = 0; i < DynamicVarList.Count; i++)
            {
                DynamicVar = DynamicVarList[i];
                if (DynamicVar.sName.ToLower().CompareTo(sVarName.ToLower()) == 0)
                {
                    switch (DynamicVar.VarType)
                    {
                        case TVarType.VInteger:
                            IniFile.WriteInteger(sName, DynamicVar.sName, DynamicVar.nInternet);
                            break;
                        case TVarType.VString:
                            IniFile.WriteString(sName, DynamicVar.sName, DynamicVar.sString);
                            break;
                    }
                    boFoundVar = true;
                    break;
                }
            }
            if (!boFoundVar)
            {
                ScriptActionError(PlayObject, format(sVarFound, new string[] { sVarName, sType }), QuestActionInfo, M2Share.sSC_SAVEVAR);
            }
            //IniFile.Free;
        }

        private void ActionOfDelayCall(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            PlayObject.m_nDelayCall = HUtil32._MAX(1, QuestActionInfo.nParam1);
            PlayObject.m_sDelayCallLabel = QuestActionInfo.sParam2;
            PlayObject.m_dwDelayCallTick = HUtil32.GetTickCount();
            PlayObject.m_boDelayCall = true;
            PlayObject.m_DelayCallNPC = this.ObjectId;
        }

        // 对变量进行运算(+、-、*、/)
        private void ActionOfCalcVar(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            string sName = string.Empty;
            TDynamicVar DynamicVar = null;
            IList<TDynamicVar> DynamicVarList;
            const string sVarFound = "变量%s不存在，变量类型:%s";
            const string sVarTypeError = "变量类型错误，错误类型:%s 当前支持类型(HUMAN、GUILD、GLOBAL)";
            string sType = QuestActionInfo.sParam1;
            string sVarName = QuestActionInfo.sParam2;
            string sMethod = QuestActionInfo.sParam3;
            string sVarValue = QuestActionInfo.sParam4;
            int nVarValue = HUtil32.Str_ToInt(QuestActionInfo.sParam4, 0);
            if ((sType == "") || (sVarName == "") || (sMethod == ""))
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_CALCVAR);
                return;
            }
            bool boFoundVar = false;
            char cMethod = sMethod[0];
            DynamicVarList = GetDynamicVarList(PlayObject, sType, ref sName);
            if (DynamicVarList == null)
            {
                Dispose(DynamicVar);
                ScriptActionError(PlayObject, format(sVarTypeError, new string[] { sType }), QuestActionInfo, M2Share.sSC_CALCVAR);
                return;
            }
            for (var i = 0; i < DynamicVarList.Count; i++)
            {
                DynamicVar = DynamicVarList[i];
                if (string.Compare(DynamicVar.sName, sVarName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    switch (DynamicVar.VarType)
                    {
                        case TVarType.VInteger:
                            switch (cMethod)
                            {
                                case '=':
                                    DynamicVar.nInternet = nVarValue;
                                    break;
                                case '+':
                                    DynamicVar.nInternet = DynamicVar.nInternet + nVarValue;
                                    break;
                                case '-':
                                    DynamicVar.nInternet = DynamicVar.nInternet - nVarValue;
                                    break;
                                case '*':
                                    DynamicVar.nInternet = DynamicVar.nInternet * nVarValue;
                                    break;
                                case '/':
                                    DynamicVar.nInternet = DynamicVar.nInternet / nVarValue;
                                    break;
                            }
                            break;
                        case TVarType.VString:
                            break;
                    }
                    boFoundVar = true;
                    break;
                }
            }
            if (!boFoundVar)
            {
                ScriptActionError(PlayObject, format(sVarFound, new string[] { sVarName, sType }), QuestActionInfo, M2Share.sSC_CALCVAR);
            }
        }

        private void ActionOfGuildRecall(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            if (PlayObject.m_MyGuild != null)
            {
                // PlayObject.GuildRecall('GuildRecall','');
            }
        }

        private void ActionOfGroupAddList(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            string ffile = QuestActionInfo.sParam1;
            if (PlayObject.m_GroupOwner != null)
            {
                for (var I = 0; I < PlayObject.m_GroupMembers.Count; I++)
                {
                    PlayObject = PlayObject.m_GroupMembers[I];
                    // AddListEx(PlayObject.m_sCharName,ffile);
                }
            }
        }

        private void ActionOfClearList(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            var ffile = M2Share.g_Config.sEnvirDir + QuestActionInfo.sParam1;
            if (File.Exists(ffile))
            {
                //myFile = new FileInfo(ffile);
                //_W_0 = myFile.CreateText();
                //_W_0.Close();
            }
        }

        private void ActionOfGroupRecall(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            if (PlayObject.m_GroupOwner != null)
            {
                // PlayObject.GroupRecall('GroupRecall');
            }
        }

        // 脚本特修身上所有装备命令
        private void ActionOfRepairAllItem(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            string sUserItemName;
            bool boIsHasItem = false;
            for (var i = PlayObject.m_UseItems.GetLowerBound(0); i <= PlayObject.m_UseItems.GetUpperBound(0); i++)
            {
                if (PlayObject.m_UseItems[i].wIndex <= 0)
                {
                    continue;
                }
                sUserItemName = M2Share.UserEngine.GetStdItemName(PlayObject.m_UseItems[i].wIndex);
                if (!(i != Grobal2.U_CHARM))
                {
                    PlayObject.SysMsg(sUserItemName + " 禁止修理...", TMsgColor.c_Red, TMsgType.t_Hint);
                    continue;
                }
                PlayObject.m_UseItems[i].Dura = PlayObject.m_UseItems[i].DuraMax;
                PlayObject.SendMsg(this, Grobal2.RM_DURACHANGE, (short)i, PlayObject.m_UseItems[i].Dura, PlayObject.m_UseItems[i].DuraMax, 0, "");
                boIsHasItem = true;
            }
            if (boIsHasItem)
            {
                PlayObject.SysMsg("您身上的的装备修复成功了...", TMsgColor.c_Blue, TMsgType.t_Hint);
            }
        }

        private void ActionOfGroupMoveMap(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            TPlayObject PlayObjectEx;
            bool boFlag = false;
            if (PlayObject.m_GroupOwner != null)
            {
                var Envir = M2Share.g_MapManager.FindMap(QuestActionInfo.sParam1);
                if (Envir != null)
                {
                    if (Envir.CanWalk(QuestActionInfo.nParam2, QuestActionInfo.nParam3, true))
                    {
                        for (var i = 0; i < PlayObject.m_GroupMembers.Count; i++)
                        {
                            PlayObjectEx = PlayObject.m_GroupMembers[i];
                            PlayObjectEx.SpaceMove(QuestActionInfo.sParam1, (short)QuestActionInfo.nParam2, (short)QuestActionInfo.nParam3, 0);
                        }
                        boFlag = true;
                    }
                }
            }
            if (!boFlag)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_GROUPMOVEMAP);
            }
        }


        private void ActionOfUpgradeItems(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            int nAddPoint;
            var nWhere = HUtil32.Str_ToInt(QuestActionInfo.sParam1, -1);
            var nRate = HUtil32.Str_ToInt(QuestActionInfo.sParam2, -1);
            var nPoint = HUtil32.Str_ToInt(QuestActionInfo.sParam3, -1);
            if ((nWhere < 0) || (nWhere > PlayObject.m_UseItems.GetUpperBound(0)) || (nRate < 0) || (nPoint < 0) || (nPoint > 255))
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_UPGRADEITEMS);
                return;
            }
            var UserItem = PlayObject.m_UseItems[nWhere];
            var StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
            if ((UserItem.wIndex <= 0) || (StdItem == null))
            {
                PlayObject.SysMsg("你身上没有戴指定物品！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            nRate = M2Share.RandomNumber.Random(nRate);
            nPoint = M2Share.RandomNumber.Random(nPoint);
            var nValType = M2Share.RandomNumber.Random(14);
            if (nRate != 0)
            {
                PlayObject.SysMsg("装备升级失败！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            if (nValType == 14)
            {
                nAddPoint = nPoint * 1000;
                if (UserItem.DuraMax + nAddPoint > ushort.MaxValue)
                {
                    nAddPoint = ushort.MaxValue - UserItem.DuraMax;
                }
                UserItem.DuraMax = (ushort)(UserItem.DuraMax + nAddPoint);
            }
            else
            {
                nAddPoint = nPoint;
                if (UserItem.btValue[nValType] + nAddPoint > byte.MaxValue)
                {
                    nAddPoint = byte.MaxValue - UserItem.btValue[nValType];
                }
                UserItem.btValue[nValType] = (byte)(UserItem.btValue[nValType] + nAddPoint);
            }
            PlayObject.SendUpdateItem(UserItem);
            PlayObject.SysMsg("装备升级成功", TMsgColor.c_Green, TMsgType.t_Hint);
            PlayObject.SysMsg(StdItem.Name + ": " + UserItem.Dura + '/' + UserItem.DuraMax + '/' + UserItem.btValue[0] + '/' + UserItem.btValue[1] + '/' + UserItem.btValue[2] + '/' + UserItem.btValue[3] + '/' + UserItem.btValue[4] + '/' + UserItem.btValue[5] + '/' + UserItem.btValue[6] + '/' + UserItem.btValue[7] + '/' + UserItem.btValue[8] + '/' + UserItem.btValue[9] + '/' + UserItem.btValue[10] + '/' + UserItem.btValue[11] + '/' + UserItem.btValue[12] + '/' + UserItem.btValue[13], TMsgColor.c_Blue, TMsgType.t_Hint);
        }

        private void ActionOfUpgradeItemsEx(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            int nAddPoint;
            var nWhere = HUtil32.Str_ToInt(QuestActionInfo.sParam1, -1);
            var nValType = HUtil32.Str_ToInt(QuestActionInfo.sParam2, -1);
            var nRate = HUtil32.Str_ToInt(QuestActionInfo.sParam3, -1);
            var nPoint = HUtil32.Str_ToInt(QuestActionInfo.sParam4, -1);
            var nUpgradeItemStatus = HUtil32.Str_ToInt(QuestActionInfo.sParam5, -1);
            if ((nValType < 0) || (nValType > 14) || (nWhere < 0) || (nWhere > PlayObject.m_UseItems.GetUpperBound(0)) || (nRate < 0) || (nPoint < 0) || (nPoint > 255))
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_UPGRADEITEMSEX);
                return;
            }
            var UserItem = PlayObject.m_UseItems[nWhere];
            var StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
            if ((UserItem.wIndex <= 0) || (StdItem == null))
            {
                PlayObject.SysMsg("你身上没有戴指定物品！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            var nRatePoint = M2Share.RandomNumber.Random(nRate * 10);
            nPoint = HUtil32._MAX(1, M2Share.RandomNumber.Random(nPoint));
            if (!(nRatePoint >= 0 && nRatePoint <= 10))
            {
                switch (nUpgradeItemStatus)
                {
                    case 0:
                        PlayObject.SysMsg("装备升级未成功！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                        break;
                    case 1:
                        PlayObject.SendDelItems(UserItem);
                        UserItem.wIndex = 0;
                        PlayObject.SysMsg("装备破碎！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                        break;
                    case 2:
                        PlayObject.SysMsg("装备升级失败，装备属性恢复默认！！！", TMsgColor.c_Red, TMsgType.t_Hint);
                        if (nValType != 14)
                        {
                            UserItem.btValue[nValType] = 0;
                        }
                        break;
                }
                return;
            }
            if (nValType == 14)
            {
                nAddPoint = nPoint * 1000;
                if (UserItem.DuraMax + nAddPoint > ushort.MaxValue)
                {
                    nAddPoint = ushort.MaxValue - UserItem.DuraMax;
                }
                UserItem.DuraMax = (ushort)(UserItem.DuraMax + nAddPoint);
            }
            else
            {
                nAddPoint = nPoint;
                if (UserItem.btValue[nValType] + nAddPoint > byte.MaxValue)
                {
                    nAddPoint = byte.MaxValue - UserItem.btValue[nValType];
                }
                UserItem.btValue[nValType] = (byte)(UserItem.btValue[nValType] + nAddPoint);
            }
            PlayObject.SendUpdateItem(UserItem);
            PlayObject.SysMsg("装备升级成功", TMsgColor.c_Green, TMsgType.t_Hint);
            PlayObject.SysMsg(StdItem.Name + ": " + UserItem.Dura + '/' + UserItem.DuraMax + '-' + UserItem.btValue[0] + '/' + UserItem.btValue[1] + '/' + UserItem.btValue[2] + '/' + UserItem.btValue[3] + '/' + UserItem.btValue[4] + '/' + UserItem.btValue[5] + '/' + UserItem.btValue[6] + '/' + UserItem.btValue[7] + '/' + UserItem.btValue[8] + '/' + UserItem.btValue[9] + '/' + UserItem.btValue[10] + '/' + UserItem.btValue[11] + '/' + UserItem.btValue[12] + '/' + UserItem.btValue[13], TMsgColor.c_Blue, TMsgType.t_Hint);
        }

        // 声明变量
        // VAR 数据类型(Integer String) 类型(HUMAN GUILD GLOBAL) 变量值
        private void ActionOfVar(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            string sName = string.Empty;
            TDynamicVar DynamicVar;
            bool boFoundVar;
            IList<TDynamicVar> DynamicVarList;
            const string sVarFound = "变量%s已存在，变量类型:%s";
            const string sVarTypeError = "变量类型错误，错误类型:%s 当前支持类型(HUMAN、GUILD、GLOBAL)";
            var sType = QuestActionInfo.sParam2;
            var sVarName = QuestActionInfo.sParam3;
            var sVarValue = QuestActionInfo.sParam4;
            var nVarValue = HUtil32.Str_ToInt(QuestActionInfo.sParam4, 0);
            var VarType = TVarType.vNone;
            if (QuestActionInfo.sParam1.ToLower().CompareTo("Integer".ToLower()) == 0)
            {
                VarType = TVarType.VInteger;
            }
            if (QuestActionInfo.sParam1.ToLower().CompareTo("String".ToLower()) == 0)
            {
                VarType = TVarType.VString;
            }
            if ((sType == "") || (sVarName == "") || (VarType == TVarType.vNone))
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_VAR);
                return;
            }
            DynamicVar = new TDynamicVar();
            DynamicVar.sName = sVarName;
            DynamicVar.VarType = VarType;
            DynamicVar.nInternet = nVarValue;
            DynamicVar.sString = sVarValue;
            boFoundVar = false;
            DynamicVarList = GetDynamicVarList(PlayObject, sType, ref sName);
            if (DynamicVarList == null)
            {
                Dispose(DynamicVar);
                ScriptActionError(PlayObject, format(sVarTypeError, new string[] { sType }), QuestActionInfo, M2Share.sSC_VAR);
                return;
            }
            for (var i = 0; i < DynamicVarList.Count; i++)
            {
                if (DynamicVarList[i].sName.ToLower().CompareTo(sVarName.ToLower()) == 0)
                {
                    boFoundVar = true;
                    break;
                }
            }
            if (!boFoundVar)
            {
                DynamicVarList.Add(DynamicVar);
            }
            else
            {
                ScriptActionError(PlayObject, format(sVarFound, new string[] { sVarName, sType }), QuestActionInfo, M2Share.sSC_VAR);
            }
        }

        // 读取变量值
        // LOADVAR 变量类型 变量名 文件名
        private void ActionOfLoadVar(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            string sName = string.Empty;
            TDynamicVar DynamicVar = null;
            bool boFoundVar;
            IList<TDynamicVar> DynamicVarList;
            IniFile IniFile;
            const string sVarFound = "变量%s不存在，变量类型:%s";
            const string sVarTypeError = "变量类型错误，错误类型:%s 当前支持类型(HUMAN、GUILD、GLOBAL)";
            var sType = QuestActionInfo.sParam1;
            var sVarName = QuestActionInfo.sParam2;
            var sFileName = M2Share.g_Config.sEnvirDir + m_sPath + QuestActionInfo.sParam3;
            if ((sType == "") || (sVarName == "") || !File.Exists(sFileName))
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_LOADVAR);
                return;
            }
            boFoundVar = false;
            DynamicVarList = GetDynamicVarList(PlayObject, sType, ref sName);
            if (DynamicVarList == null)
            {
                Dispose(DynamicVar);
                ScriptActionError(PlayObject, format(sVarTypeError, new string[] { sType }), QuestActionInfo, M2Share.sSC_VAR);
                return;
            }
            IniFile = new IniFile(sFileName);
            for (var i = 0; i < DynamicVarList.Count; i++)
            {
                DynamicVar = DynamicVarList[i];
                if (DynamicVar.sName.ToLower().CompareTo(sVarName.ToLower()) == 0)
                {
                    switch (DynamicVar.VarType)
                    {
                        case TVarType.VInteger:
                            DynamicVar.nInternet = IniFile.ReadInteger(sName, DynamicVar.sName, 0);
                            break;
                        case TVarType.VString:
                            DynamicVar.sString = IniFile.ReadString(sName, DynamicVar.sName, "");
                            break;
                    }
                    boFoundVar = true;
                    break;
                }
            }
            if (!boFoundVar)
            {
                ScriptActionError(PlayObject, format(sVarFound, new string[] { sVarName, sType }), QuestActionInfo, M2Share.sSC_LOADVAR);
            }
            //IniFile.Free;
        }

        private void ActionOfClearNeedItems(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            TUserItem UserItem;
            MirItem StdItem;
            var nNeed = HUtil32.Str_ToInt(QuestActionInfo.sParam1, -1);
            if (nNeed < 0)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_CLEARNEEDITEMS);
                return;
            }
            for (var i = PlayObject.m_ItemList.Count - 1; i >= 0; i--)
            {
                UserItem = PlayObject.m_ItemList[i];
                StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                if ((StdItem != null) && (StdItem.Need == nNeed))
                {
                    PlayObject.SendDelItems(UserItem);
                    Dispose(UserItem);
                    PlayObject.m_ItemList.RemoveAt(i);
                }
            }
            for (var i = PlayObject.m_StorageItemList.Count - 1; i >= 0; i--)
            {
                UserItem = PlayObject.m_StorageItemList[i];
                StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                if ((StdItem != null) && (StdItem.Need == nNeed))
                {
                    Dispose(UserItem);
                    PlayObject.m_StorageItemList.RemoveAt(i);
                }
            }
        }

        private void ActionOfClearMakeItems(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            TUserItem UserItem;
            MirItem StdItem;
            string sItemName = QuestActionInfo.sParam1;
            var nMakeIndex = QuestActionInfo.nParam2;
            var boMatchName = QuestActionInfo.sParam3 == "1";
            if ((sItemName == "") || (nMakeIndex <= 0))
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_CLEARMAKEITEMS);
                return;
            }
            for (var i = PlayObject.m_ItemList.Count - 1; i >= 0; i--)
            {
                UserItem = PlayObject.m_ItemList[i];
                if (UserItem.MakeIndex != nMakeIndex)
                {
                    continue;
                }
                StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                if (!boMatchName || ((StdItem != null) && (StdItem.Name.ToLower().CompareTo(sItemName.ToLower()) == 0)))
                {
                    PlayObject.SendDelItems(UserItem);
                    Dispose(UserItem);
                    PlayObject.m_ItemList.RemoveAt(i);
                }
            }
            for (var i = PlayObject.m_StorageItemList.Count - 1; i >= 0; i--)
            {
                UserItem = PlayObject.m_ItemList[i];
                if (UserItem.MakeIndex != nMakeIndex)
                {
                    continue;
                }
                StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                if (!boMatchName || ((StdItem != null) && (StdItem.Name.ToLower().CompareTo(sItemName.ToLower()) == 0)))
                {
                    Dispose(UserItem);
                    PlayObject.m_StorageItemList.RemoveAt(i);
                }
            }
            for (var i = PlayObject.m_UseItems.GetLowerBound(0); i <= PlayObject.m_UseItems.GetUpperBound(0); i++)
            {
                UserItem = PlayObject.m_UseItems[i];
                if (UserItem.MakeIndex != nMakeIndex)
                {
                    continue;
                }
                StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                if (!boMatchName || ((StdItem != null) && (StdItem.Name.ToLower().CompareTo(sItemName.ToLower()) == 0)))
                {
                    UserItem.wIndex = 0;
                }
            }
        }

        private void ActionOfUnMaster(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            string sMsg;
            if (PlayObject.m_sMasterName == "")
            {
                GotoLable(PlayObject, "@ExeMasterFail", false);
                return;
            }
            var PoseHuman = (TPlayObject)PlayObject.GetPoseCreate();
            if (PoseHuman == null)
            {
                GotoLable(PlayObject, "@UnMasterCheckDir", false);
            }
            if (PoseHuman != null)
            {
                if (QuestActionInfo.sParam1 == "")
                {
                    if (PoseHuman.m_btRaceServer != Grobal2.RC_PLAYOBJECT)
                    {
                        GotoLable(PlayObject, "@UnMasterTypeErr", false);
                        return;
                    }
                    if (PoseHuman.GetPoseCreate() == PlayObject)
                    {
                        if (PlayObject.m_sMasterName == PoseHuman.m_sCharName)
                        {
                            if (PlayObject.m_boMaster)
                            {
                                GotoLable(PlayObject, "@UnIsMaster", false);
                                return;
                            }
                            if (PlayObject.m_sMasterName != PoseHuman.m_sCharName)
                            {
                                GotoLable(PlayObject, "@UnMasterError", false);
                                return;
                            }
                            GotoLable(PlayObject, "@StartUnMaster", false);
                            GotoLable(PoseHuman, "@WateUnMaster", false);
                            return;
                        }
                    }
                }
            }
            // sREQUESTUNMARRY
            if (QuestActionInfo.sParam1.ToLower().CompareTo("REQUESTUNMASTER".ToLower()) == 0)
            {
                if (QuestActionInfo.sParam2 == "")
                {
                    if (PoseHuman != null)
                    {
                        PlayObject.m_boStartUnMaster = true;
                        if (PlayObject.m_boStartUnMaster && PoseHuman.m_boStartUnMaster)
                        {
                            sMsg = M2Share.g_sNPCSayUnMasterOKMsg.Replace("%n", this.m_sCharName);
                            sMsg = sMsg.Replace("%s", PlayObject.m_sCharName);
                            sMsg = sMsg.Replace("%d", PoseHuman.m_sCharName);
                            M2Share.UserEngine.SendBroadCastMsg(sMsg, TMsgType.t_Say);
                            PlayObject.m_sMasterName = "";
                            PoseHuman.m_sMasterName = "";
                            PlayObject.m_boStartUnMaster = false;
                            PoseHuman.m_boStartUnMaster = false;
                            PlayObject.RefShowName();
                            PoseHuman.RefShowName();
                            GotoLable(PlayObject, "@UnMasterEnd", false);
                            GotoLable(PoseHuman, "@UnMasterEnd", false);
                        }
                        else
                        {
                            GotoLable(PlayObject, "@WateUnMaster", false);
                            GotoLable(PoseHuman, "@RevUnMaster", false);
                        }
                    }
                    return;
                }
                else
                {
                    // 强行出师
                    if (QuestActionInfo.sParam2.ToLower().CompareTo("FORCE".ToLower()) == 0)
                    {
                        sMsg = M2Share.g_sNPCSayForceUnMasterMsg.Replace("%n", this.m_sCharName);
                        sMsg = sMsg.Replace("%s", PlayObject.m_sCharName);
                        sMsg = sMsg.Replace("%d", PlayObject.m_sMasterName);
                        M2Share.UserEngine.SendBroadCastMsg(sMsg, TMsgType.t_Say);
                        PoseHuman = M2Share.UserEngine.GetPlayObject(PlayObject.m_sMasterName);
                        if (PoseHuman != null)
                        {
                            PoseHuman.m_sMasterName = "";
                            PoseHuman.RefShowName();
                        }
                        else
                        {
                            M2Share.g_UnForceMasterList.Add(PlayObject.m_sMasterName);
                            M2Share.SaveUnForceMasterList();
                        }
                        PlayObject.m_sMasterName = "";
                        GotoLable(PlayObject, "@UnMasterEnd", false);
                        PlayObject.RefShowName();
                    }
                    return;
                }
            }
        }

        private void ActionOfSetMapMode(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            string sMapName = QuestActionInfo.sParam1;
            string sMapMode = QuestActionInfo.sParam2;
            string sParam1 = QuestActionInfo.sParam3;
            string sParam2 = QuestActionInfo.sParam4;
            TEnvirnoment Envir = M2Share.g_MapManager.FindMap(sMapName);
            if ((Envir == null) || (sMapMode == ""))
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_SETMAPMODE);
                return;
            }
            if (sMapMode.ToLower().CompareTo("SAFE".ToLower()) == 0)
            {
                if (sParam1 != "")
                {
                    Envir.Flag.boSAFE = true;
                }
                else
                {
                    Envir.Flag.boSAFE = false;
                }
            }
            else if (sMapMode.ToLower().CompareTo("DARK".ToLower()) == 0)
            {
                if (sParam1 != "")
                {
                    Envir.Flag.boDarkness = true;
                }
                else
                {
                    Envir.Flag.boDarkness = false;
                }
            }
            else if (sMapMode.ToLower().CompareTo("FIGHT".ToLower()) == 0)
            {
                if (sParam1 != "")
                {
                    Envir.Flag.boFightZone = true;
                }
                else
                {
                    Envir.Flag.boFightZone = false;
                }
            }
            else if (sMapMode.ToLower().CompareTo("FIGHT3".ToLower()) == 0)
            {
                if (sParam1 != "")
                {
                    Envir.Flag.boFight3Zone = true;
                }
                else
                {
                    Envir.Flag.boFight3Zone = false;
                }
            }
            else if (sMapMode.ToLower().CompareTo("DAY".ToLower()) == 0)
            {
                if (sParam1 != "")
                {
                    Envir.Flag.boDayLight = true;
                }
                else
                {
                    Envir.Flag.boDayLight = false;
                }
            }
            else if (sMapMode.ToLower().CompareTo("QUIZ".ToLower()) == 0)
            {
                if (sParam1 != "")
                {
                    Envir.Flag.boQUIZ = true;
                }
                else
                {
                    Envir.Flag.boQUIZ = false;
                }
            }
            else if (sMapMode.ToLower().CompareTo("NORECONNECT".ToLower()) == 0)
            {
                if (sParam1 != "")
                {
                    Envir.Flag.boNORECONNECT = true;
                    Envir.Flag.sNoReConnectMap = sParam1;
                }
                else
                {
                    Envir.Flag.boNORECONNECT = false;
                }
            }
            else if (sMapMode.ToLower().CompareTo("MUSIC".ToLower()) == 0)
            {
                if (sParam1 != "")
                {
                    Envir.Flag.boMUSIC = true;
                    Envir.Flag.nMUSICID = HUtil32.Str_ToInt(sParam1, -1);
                }
                else
                {
                    Envir.Flag.boMUSIC = false;
                }
            }
            else if (sMapMode.ToLower().CompareTo("EXPRATE".ToLower()) == 0)
            {
                if (sParam1 != "")
                {
                    Envir.Flag.boEXPRATE = true;
                    Envir.Flag.nEXPRATE = HUtil32.Str_ToInt(sParam1, -1);
                }
                else
                {
                    Envir.Flag.boEXPRATE = false;
                }
            }
            else if (sMapMode.ToLower().CompareTo("PKWINLEVEL".ToLower()) == 0)
            {
                if (sParam1 != "")
                {
                    Envir.Flag.boPKWINLEVEL = true;
                    Envir.Flag.nPKWINLEVEL = HUtil32.Str_ToInt(sParam1, -1);
                }
                else
                {
                    Envir.Flag.boPKWINLEVEL = false;
                }
            }
            else if (sMapMode.ToLower().CompareTo("PKWINEXP".ToLower()) == 0)
            {
                if (sParam1 != "")
                {
                    Envir.Flag.boPKWINEXP = true;
                    Envir.Flag.nPKWINEXP = HUtil32.Str_ToInt(sParam1, -1);
                }
                else
                {
                    Envir.Flag.boPKWINEXP = false;
                }
            }
            else if (sMapMode.ToLower().CompareTo("PKLOSTLEVEL".ToLower()) == 0)
            {
                if (sParam1 != "")
                {
                    Envir.Flag.boPKLOSTLEVEL = true;
                    Envir.Flag.nPKLOSTLEVEL = HUtil32.Str_ToInt(sParam1, -1);
                }
                else
                {
                    Envir.Flag.boPKLOSTLEVEL = false;
                }
            }
            else if (sMapMode.ToLower().CompareTo("PKLOSTEXP".ToLower()) == 0)
            {
                if (sParam1 != "")
                {
                    Envir.Flag.boPKLOSTEXP = true;
                    Envir.Flag.nPKLOSTEXP = HUtil32.Str_ToInt(sParam1, -1);
                }
                else
                {
                    Envir.Flag.boPKLOSTEXP = false;
                }
            }
            else if (sMapMode.ToLower().CompareTo("DECHP".ToLower()) == 0)
            {
                if ((sParam1 != "") && (sParam2 != ""))
                {
                    Envir.Flag.boDECHP = true;
                    Envir.Flag.nDECHPTIME = HUtil32.Str_ToInt(sParam1, -1);
                    Envir.Flag.nDECHPPOINT = HUtil32.Str_ToInt(sParam2, -1);
                }
                else
                {
                    Envir.Flag.boDECHP = false;
                }
            }
            else if (sMapMode.ToLower().CompareTo("DECGAMEGOLD".ToLower()) == 0)
            {
                if ((sParam1 != "") && (sParam2 != ""))
                {
                    Envir.Flag.boDECGAMEGOLD = true;
                    Envir.Flag.nDECGAMEGOLDTIME = HUtil32.Str_ToInt(sParam1, -1);
                    Envir.Flag.nDECGAMEGOLD = HUtil32.Str_ToInt(sParam2, -1);
                }
                else
                {
                    Envir.Flag.boDECGAMEGOLD = false;
                }
            }
            else if (sMapMode.ToLower().CompareTo("RUNHUMAN".ToLower()) == 0)
            {
                if (sParam1 != "")
                {
                    Envir.Flag.boRUNHUMAN = true;
                }
                else
                {
                    Envir.Flag.boRUNHUMAN = false;
                }
            }
            else if (sMapMode.ToLower().CompareTo("RUNMON".ToLower()) == 0)
            {
                if (sParam1 != "")
                {
                    Envir.Flag.boRUNMON = true;
                }
                else
                {
                    Envir.Flag.boRUNMON = false;
                }
            }
            else if (sMapMode.ToLower().CompareTo("NEEDHOLE".ToLower()) == 0)
            {
                if (sParam1 != "")
                {
                    Envir.Flag.boNEEDHOLE = true;
                }
                else
                {
                    Envir.Flag.boNEEDHOLE = false;
                }
            }
            else if (sMapMode.ToLower().CompareTo("NORECALL".ToLower()) == 0)
            {
                if (sParam1 != "")
                {
                    Envir.Flag.boNORECALL = true;
                }
                else
                {
                    Envir.Flag.boNORECALL = false;
                }
            }
            else if (sMapMode.ToLower().CompareTo("NOGUILDRECALL".ToLower()) == 0)
            {
                if (sParam1 != "")
                {
                    Envir.Flag.boNOGUILDRECALL = true;
                }
                else
                {
                    Envir.Flag.boNOGUILDRECALL = false;
                }
            }
            else if (sMapMode.ToLower().CompareTo("NODEARRECALL".ToLower()) == 0)
            {
                if (sParam1 != "")
                {
                    Envir.Flag.boNODEARRECALL = true;
                }
                else
                {
                    Envir.Flag.boNODEARRECALL = false;
                }
            }
            else if (sMapMode.ToLower().CompareTo("NOMASTERRECALL".ToLower()) == 0)
            {
                if (sParam1 != "")
                {
                    Envir.Flag.boNOMASTERRECALL = true;
                }
                else
                {
                    Envir.Flag.boNOMASTERRECALL = false;
                }
            }
            else if (sMapMode.ToLower().CompareTo("NORANDOMMOVE".ToLower()) == 0)
            {
                if (sParam1 != "")
                {
                    Envir.Flag.boNORANDOMMOVE = true;
                }
                else
                {
                    Envir.Flag.boNORANDOMMOVE = false;
                }
            }
            else if (sMapMode.ToLower().CompareTo("NODRUG".ToLower()) == 0)
            {
                if (sParam1 != "")
                {
                    Envir.Flag.boNODRUG = true;
                }
                else
                {
                    Envir.Flag.boNODRUG = false;
                }
            }
            else if (sMapMode.ToLower().CompareTo("MINE".ToLower()) == 0)
            {
                if (sParam1 != "")
                {
                    Envir.Flag.boMINE = true;
                }
                else
                {
                    Envir.Flag.boMINE = false;
                }
            }
            else if (sMapMode.ToLower().CompareTo("MINE2".ToLower()) == 0)
            {
                if (sParam1 != "")
                {
                    Envir.Flag.boMINE2 = true;
                }
                else
                {
                    Envir.Flag.boMINE2 = false;
                }
            }
            else if (sMapMode.ToLower().CompareTo("NOTHROWITEM".ToLower()) == 0)
            {
                if (sParam1 != "")
                {
                    Envir.Flag.boNOTHROWITEM = true;
                }
                else
                {
                    Envir.Flag.boNOTHROWITEM = false;
                }
            }
            else if (sMapMode.ToLower().CompareTo("NODROPITEM".ToLower()) == 0)
            {
                if (sParam1 != "")
                {
                    Envir.Flag.boNODROPITEM = true;
                }
                else
                {
                    Envir.Flag.boNODROPITEM = false;
                }
            }
            else if (sMapMode.ToLower().CompareTo("NOPOSITIONMOVE".ToLower()) == 0)
            {
                if (sParam1 != "")
                {
                    Envir.Flag.boNOPOSITIONMOVE = true;
                }
                else
                {
                    Envir.Flag.boNOPOSITIONMOVE = false;
                }
            }
            else if (sMapMode.ToLower().CompareTo("NOHORSE".ToLower()) == 0)
            {
                if (sParam1 != "")
                {
                    Envir.Flag.boNOHORSE = true;
                }
                else
                {
                    Envir.Flag.boNOHORSE = false;
                }
            }
            else if (sMapMode.ToLower().CompareTo("NOCHAT".ToLower()) == 0)
            {
                if (sParam1 != "")
                {
                    Envir.Flag.boNOCHAT = true;
                }
                else
                {
                    Envir.Flag.boNOCHAT = false;
                }
            }
        }

        private void ActionOfSetMemberLevel(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            int nLevel = HUtil32.Str_ToInt(QuestActionInfo.sParam2, -1);
            if (nLevel < 0)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_SETMEMBERLEVEL);
                return;
            }
            char cMethod = QuestActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    PlayObject.m_nMemberLevel = nLevel;
                    break;
                case '-':
                    PlayObject.m_nMemberLevel -= nLevel;
                    if (PlayObject.m_nMemberLevel < 0)
                    {
                        PlayObject.m_nMemberLevel = 0;
                    }
                    break;
                case '+':
                    PlayObject.m_nMemberLevel += nLevel;
                    if (PlayObject.m_nMemberLevel > 65535)
                    {
                        PlayObject.m_nMemberLevel = 65535;
                    }
                    break;
            }
            if (M2Share.g_Config.boShowScriptActionMsg)
            {
                PlayObject.SysMsg(format(M2Share.g_sChangeMemberLevelMsg, new int[] { PlayObject.m_nMemberLevel }), TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }

        private void ActionOfSetMemberType(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            int nType = HUtil32.Str_ToInt(QuestActionInfo.sParam2, -1);
            if (nType < 0)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_SETMEMBERTYPE);
                return;
            }
            char cMethod = QuestActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    PlayObject.m_nMemberType = nType;
                    break;
                case '-':
                    PlayObject.m_nMemberType -= nType;
                    if (PlayObject.m_nMemberType < 0)
                    {
                        PlayObject.m_nMemberType = 0;
                    }
                    break;
                case '+':
                    PlayObject.m_nMemberType += nType;
                    if (PlayObject.m_nMemberType > 65535)
                    {
                        PlayObject.m_nMemberType = 65535;
                    }
                    break;
            }
            if (M2Share.g_Config.boShowScriptActionMsg)
            {
                PlayObject.SysMsg(format(M2Share.g_sChangeMemberTypeMsg, new int[] { PlayObject.m_nMemberType }), TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }

        private void ActionOfGiveItem(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            TUserItem UserItem;
            MirItem StdItem;
            var sItemName = QuestActionInfo.sParam1;
            var nItemCount = QuestActionInfo.nParam2;
            if ((sItemName == "") || (nItemCount <= 0))
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_GIVE);
                return;
            }
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
                for (var I = 0; I < nItemCount; I++)
                {
                    // nItemCount 为0时出死循环
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

        private void ActionOfGmExecute(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            string sParam1 = QuestActionInfo.sParam1;
            string sParam2 = QuestActionInfo.sParam2;
            string sParam3 = QuestActionInfo.sParam3;
            string sParam4 = QuestActionInfo.sParam4;
            string sParam5 = QuestActionInfo.sParam5;
            string sParam6 = QuestActionInfo.sParam6;
            if (sParam2.ToLower().CompareTo("Self".ToLower()) == 0)
            {
                sParam2 = PlayObject.m_sCharName;
            }
            string sData = format("@{0} {1} {2} {3} {4} {5}", sParam1, sParam2, sParam3, sParam4, sParam5, sParam6);
            byte btOldPermission = PlayObject.m_btPermission;
            try
            {
                PlayObject.m_btPermission = 10;
                PlayObject.ProcessUserLineMsg(sData);
            }
            finally
            {
                PlayObject.m_btPermission = btOldPermission;
            }
        }

        private void ActionOfGuildAuraePoint(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            int nAuraePoint = HUtil32.Str_ToInt(QuestActionInfo.sParam2, -1);
            if (nAuraePoint < 0)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_AURAEPOINT);
                return;
            }
            if (PlayObject.m_MyGuild == null)
            {
                PlayObject.SysMsg(M2Share.g_sScriptGuildAuraePointNoGuild, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            var Guild = PlayObject.m_MyGuild;
            var cMethod = QuestActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    Guild.nAurae = nAuraePoint;
                    break;
                case '-':
                    if (Guild.nAurae >= nAuraePoint)
                    {
                        Guild.nAurae = Guild.nAurae - nAuraePoint;
                    }
                    else
                    {
                        Guild.nAurae = 0;
                    }
                    break;
                case '+':
                    if ((int.MaxValue - Guild.nAurae) >= nAuraePoint)
                    {
                        Guild.nAurae = Guild.nAurae + nAuraePoint;
                    }
                    else
                    {
                        Guild.nAurae = int.MaxValue;
                    }
                    break;
            }
            if (M2Share.g_Config.boShowScriptActionMsg)
            {
                PlayObject.SysMsg(format(M2Share.g_sScriptGuildAuraePointMsg, new int[] { Guild.nAurae }), TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }

        private void ActionOfGuildBuildPoint(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            var nBuildPoint = HUtil32.Str_ToInt(QuestActionInfo.sParam2, -1);
            if (nBuildPoint < 0)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_BUILDPOINT);
                return;
            }
            if (PlayObject.m_MyGuild == null)
            {
                PlayObject.SysMsg(M2Share.g_sScriptGuildBuildPointNoGuild, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            var Guild = PlayObject.m_MyGuild;
            var cMethod = QuestActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    Guild.nBuildPoint = nBuildPoint;
                    break;
                case '-':
                    if (Guild.nBuildPoint >= nBuildPoint)
                    {
                        Guild.nBuildPoint = Guild.nBuildPoint - nBuildPoint;
                    }
                    else
                    {
                        Guild.nBuildPoint = 0;
                    }
                    break;
                case '+':
                    if ((int.MaxValue - Guild.nBuildPoint) >= nBuildPoint)
                    {
                        Guild.nBuildPoint = Guild.nBuildPoint + nBuildPoint;
                    }
                    else
                    {
                        Guild.nBuildPoint = int.MaxValue;
                    }
                    break;
            }
            if (M2Share.g_Config.boShowScriptActionMsg)
            {
                PlayObject.SysMsg(format(M2Share.g_sScriptGuildBuildPointMsg, new[] { Guild.nBuildPoint }), TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }

        private void ActionOfGuildChiefItemCount(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            var nItemCount = HUtil32.Str_ToInt(QuestActionInfo.sParam2, -1);
            if (nItemCount < 0)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_GUILDCHIEFITEMCOUNT);
                return;
            }
            if (PlayObject.m_MyGuild == null)
            {
                PlayObject.SysMsg(M2Share.g_sScriptGuildFlourishPointNoGuild, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            var Guild = PlayObject.m_MyGuild;
            var cMethod = QuestActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    Guild.nChiefItemCount = nItemCount;
                    break;
                case '-':
                    if (Guild.nChiefItemCount >= nItemCount)
                    {
                        Guild.nChiefItemCount = Guild.nChiefItemCount - nItemCount;
                    }
                    else
                    {
                        Guild.nChiefItemCount = 0;
                    }
                    break;
                case '+':
                    if ((int.MaxValue - Guild.nChiefItemCount) >= nItemCount)
                    {
                        Guild.nChiefItemCount = Guild.nChiefItemCount + nItemCount;
                    }
                    else
                    {
                        Guild.nChiefItemCount = int.MaxValue;
                    }
                    break;
            }
            if (M2Share.g_Config.boShowScriptActionMsg)
            {
                PlayObject.SysMsg(format(M2Share.g_sScriptChiefItemCountMsg, new int[] { Guild.nChiefItemCount }), TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }

        private void ActionOfGuildFlourishPoint(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            var nFlourishPoint = HUtil32.Str_ToInt(QuestActionInfo.sParam2, -1);
            if (nFlourishPoint < 0)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_FLOURISHPOINT);
                return;
            }
            if (PlayObject.m_MyGuild == null)
            {
                PlayObject.SysMsg(M2Share.g_sScriptGuildFlourishPointNoGuild, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            var Guild = PlayObject.m_MyGuild;
            var cMethod = QuestActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    Guild.nFlourishing = nFlourishPoint;
                    break;
                case '-':
                    if (Guild.nFlourishing >= nFlourishPoint)
                    {
                        Guild.nFlourishing = Guild.nFlourishing - nFlourishPoint;
                    }
                    else
                    {
                        Guild.nFlourishing = 0;
                    }
                    break;
                case '+':
                    if ((int.MaxValue - Guild.nFlourishing) >= nFlourishPoint)
                    {
                        Guild.nFlourishing = Guild.nFlourishing + nFlourishPoint;
                    }
                    else
                    {
                        Guild.nFlourishing = int.MaxValue;
                    }
                    break;
            }
            if (M2Share.g_Config.boShowScriptActionMsg)
            {
                PlayObject.SysMsg(format(M2Share.g_sScriptGuildFlourishPointMsg, new int[] { Guild.nFlourishing }), TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }

        private void ActionOfGuildstabilityPoint(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            var nStabilityPoint = HUtil32.Str_ToInt(QuestActionInfo.sParam2, -1);
            if (nStabilityPoint < 0)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_STABILITYPOINT);
                return;
            }
            if (PlayObject.m_MyGuild == null)
            {
                PlayObject.SysMsg(M2Share.g_sScriptGuildStabilityPointNoGuild, TMsgColor.c_Red, TMsgType.t_Hint);
                return;
            }
            var Guild = PlayObject.m_MyGuild;
            var cMethod = QuestActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    Guild.nStability = nStabilityPoint;
                    break;
                case '-':
                    if (Guild.nStability >= nStabilityPoint)
                    {
                        Guild.nStability = Guild.nStability - nStabilityPoint;
                    }
                    else
                    {
                        Guild.nStability = 0;
                    }
                    break;
                case '+':
                    if ((int.MaxValue - Guild.nStability) >= nStabilityPoint)
                    {
                        Guild.nStability = Guild.nStability + nStabilityPoint;
                    }
                    else
                    {
                        Guild.nStability = int.MaxValue;
                    }
                    break;
            }
            if (M2Share.g_Config.boShowScriptActionMsg)
            {
                PlayObject.SysMsg(format(M2Share.g_sScriptGuildStabilityPointMsg, new int[] { Guild.nStability }), TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }

        private void ActionOfHumanHP(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            var nHP = HUtil32.Str_ToInt(QuestActionInfo.sParam2, -1);
            if (nHP < 0)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_HUMANHP);
                return;
            }
            var cMethod = QuestActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    PlayObject.m_WAbil.HP = (ushort)nHP;
                    break;
                case '-':
                    if (PlayObject.m_WAbil.HP >= nHP)
                    {
                        PlayObject.m_WAbil.HP -= (ushort)nHP;
                    }
                    else
                    {
                        PlayObject.m_WAbil.HP = 0;
                    }
                    break;
                case '+':
                    PlayObject.m_WAbil.HP += (ushort)nHP;
                    if (PlayObject.m_WAbil.HP > PlayObject.m_WAbil.MaxHP)
                    {
                        PlayObject.m_WAbil.HP = PlayObject.m_WAbil.MaxHP;
                    }
                    break;
            }
            if (M2Share.g_Config.boShowScriptActionMsg)
            {
                PlayObject.SysMsg(format(M2Share.g_sScriptChangeHumanHPMsg, new ushort[] { PlayObject.m_WAbil.MaxHP }), TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }

        private void ActionOfHumanMP(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            var nMP = HUtil32.Str_ToInt(QuestActionInfo.sParam2, -1);
            if (nMP < 0)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_HUMANMP);
                return;
            }
            var cMethod = QuestActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    PlayObject.m_WAbil.MP = (ushort)nMP;
                    break;
                case '-':
                    if (PlayObject.m_WAbil.MP >= nMP)
                    {
                        PlayObject.m_WAbil.MP -= (ushort)nMP;
                    }
                    else
                    {
                        PlayObject.m_WAbil.MP = 0;
                    }
                    break;
                case '+':
                    PlayObject.m_WAbil.MP += (ushort)nMP;
                    if (PlayObject.m_WAbil.MP > PlayObject.m_WAbil.MaxMP)
                    {
                        PlayObject.m_WAbil.MP = PlayObject.m_WAbil.MaxMP;
                    }
                    break;
            }
            if (M2Share.g_Config.boShowScriptActionMsg)
            {
                PlayObject.SysMsg(format(M2Share.g_sScriptChangeHumanMPMsg, new ushort[] { PlayObject.m_WAbil.MaxMP }), TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }

        private void ActionOfKick(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            PlayObject.m_boKickFlag = true;
        }

        private void ActionOfKill(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            var nMode = HUtil32.Str_ToInt(QuestActionInfo.sParam1, -1);
            if (nMode >= 0 && nMode <= 3)
            {
                switch (nMode)
                {
                    case 1:
                        PlayObject.m_boNoItem = true;
                        PlayObject.Die();
                        break;
                    case 2:
                        PlayObject.SetLastHiter(this);
                        PlayObject.Die();
                        break;
                    case 3:
                        PlayObject.m_boNoItem = true;
                        PlayObject.SetLastHiter(this);
                        PlayObject.Die();
                        break;
                    default:
                        PlayObject.Die();
                        break;
                }
            }
            else
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_KILL);
            }
        }

        private void ActionOfBonusPoint(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            int nBonusPoint = HUtil32.Str_ToInt(QuestActionInfo.sParam2, -1);
            if ((nBonusPoint < 0) || (nBonusPoint > 10000))
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_BONUSPOINT);
                return;
            }
            char cMethod = QuestActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    //FillChar(PlayObject.m_BonusAbil, sizeof(TNakedAbility), '\0');
                    PlayObject.HasLevelUp(0);
                    PlayObject.m_nBonusPoint = nBonusPoint;
                    PlayObject.SendMsg(PlayObject, Grobal2.RM_ADJUST_BONUS, 0, 0, 0, 0, "");
                    break;
                case '-':
                    break;
                case '+':
                    PlayObject.m_nBonusPoint += nBonusPoint;
                    PlayObject.SendMsg(PlayObject, Grobal2.RM_ADJUST_BONUS, 0, 0, 0, 0, "");
                    break;
            }
        }

        private void ActionOfDelMarry(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            PlayObject.m_sDearName = "";
            PlayObject.RefShowName();
        }

        private void ActionOfDelMaster(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            PlayObject.m_sMasterName = "";
            PlayObject.RefShowName();
        }

        private void ActionOfRestBonusPoint(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            var nTotleUsePoint = PlayObject.m_BonusAbil.DC + PlayObject.m_BonusAbil.MC + PlayObject.m_BonusAbil.SC + PlayObject.m_BonusAbil.AC + PlayObject.m_BonusAbil.MAC + PlayObject.m_BonusAbil.HP + PlayObject.m_BonusAbil.MP + PlayObject.m_BonusAbil.Hit + PlayObject.m_BonusAbil.Speed + PlayObject.m_BonusAbil.X2;
            //FillChar(PlayObject.m_BonusAbil, sizeof(TNakedAbility), '\0');
            PlayObject.m_nBonusPoint += nTotleUsePoint;
            PlayObject.SendMsg(PlayObject, Grobal2.RM_ADJUST_BONUS, 0, 0, 0, 0, "");
            PlayObject.HasLevelUp(0);
            PlayObject.SysMsg("分配点数已复位！！！", TMsgColor.c_Red, TMsgType.t_Hint);
        }

        private void ActionOfRestReNewLevel(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            PlayObject.m_btReLevel = 0;
            PlayObject.HasLevelUp(0);
        }


        private void ActionOfChangeNameColor(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            int nColor = QuestActionInfo.nParam1;
            if ((nColor < 0) || (nColor > 255))
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_CHANGENAMECOLOR);
                return;
            }
            PlayObject.m_btNameColor = (byte)nColor;
            PlayObject.RefNameColor();
        }

        private void ActionOfClearPassword(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            PlayObject.m_sStoragePwd = "";
            PlayObject.m_boPasswordLocked = false;
        }

        // 挂机的
        // RECALLMOB 怪物名称 等级 叛变时间 变色(0,1) 固定颜色(1 - 7)
        // 变色为0 时固定颜色才起作用
        private void ActionOfRecallmob(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            TBaseObject Mon;
            if (QuestActionInfo.nParam3 <= 1)
            {
                Mon = PlayObject.MakeSlave(QuestActionInfo.sParam1, 3, HUtil32.Str_ToInt(QuestActionInfo.sParam2, 0), 100, 10 * 24 * 60 * 60);
            }
            else
            {
                Mon = PlayObject.MakeSlave(QuestActionInfo.sParam1, 3, HUtil32.Str_ToInt(QuestActionInfo.sParam2, 0), 100, QuestActionInfo.nParam3 * 60);
            }
            if (Mon != null)
            {
                if ((QuestActionInfo.sParam4 != "") && (QuestActionInfo.sParam4[1] == '1'))
                {
                    Mon.m_boAutoChangeColor = true;
                }
                else if (QuestActionInfo.nParam5 > 0)
                {
                    Mon.m_boFixColor = true;
                    Mon.m_nFixColorIdx = QuestActionInfo.nParam5 - 1;
                }
            }
        }

        private void ActionOfReNewLevel(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            int nReLevel = HUtil32.Str_ToInt(QuestActionInfo.sParam1, -1);
            int nLevel = HUtil32.Str_ToInt(QuestActionInfo.sParam2, -1);
            int nBounsuPoint = HUtil32.Str_ToInt(QuestActionInfo.sParam3, -1);
            if ((nReLevel < 0) || (nLevel < 0) || (nBounsuPoint < 0))
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_RENEWLEVEL);
                return;
            }
            if ((PlayObject.m_btReLevel + nReLevel) <= 255)
            {
                PlayObject.m_btReLevel += (byte)nReLevel;
                if (nLevel > 0)
                {
                    PlayObject.m_Abil.Level = (ushort)nLevel;
                }
                if (M2Share.g_Config.boReNewLevelClearExp)
                {
                    PlayObject.m_Abil.Exp = 0;
                }
                PlayObject.m_nBonusPoint += nBounsuPoint;
                PlayObject.SendMsg(PlayObject, Grobal2.RM_ADJUST_BONUS, 0, 0, 0, 0, "");
                PlayObject.HasLevelUp(0);
                PlayObject.RefShowName();
            }
        }

        private void ActionOfChangeGender(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            var nGender = HUtil32.Str_ToInt(QuestActionInfo.sParam1, -1);
            if (nGender > 1)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_CHANGEGENDER);
                return;
            }
            PlayObject.m_btGender = (byte)nGender;
            PlayObject.FeatureChanged();
        }

        private void ActionOfKillSlave(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            TBaseObject Slave;
            for (var i = 0; i < PlayObject.m_SlaveList.Count; i++)
            {
                Slave = PlayObject.m_SlaveList[i];
                Slave.m_WAbil.HP = 0;
            }
        }

        private void ActionOfKillMonExpRate(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            int nRate = HUtil32.Str_ToInt(QuestActionInfo.sParam1, -1);
            int nTime = HUtil32.Str_ToInt(QuestActionInfo.sParam2, -1);
            if ((nRate < 0) || (nTime < 0))
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_KILLMONEXPRATE);
                return;
            }
            PlayObject.m_nKillMonExpRate = nRate;
            // PlayObject.m_dwKillMonExpRateTime:=_MIN(High(Word),nTime);
            PlayObject.m_dwKillMonExpRateTime = nTime;
            if (M2Share.g_Config.boShowScriptActionMsg)
            {
                PlayObject.SysMsg(format(M2Share.g_sChangeKillMonExpRateMsg, new object[] { PlayObject.m_nKillMonExpRate / 100, PlayObject.m_dwKillMonExpRateTime }), TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }

        private void ActionOfMonGenEx(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            string sMapName = QuestActionInfo.sParam1;
            int nMapX = HUtil32.Str_ToInt(QuestActionInfo.sParam2, -1);
            int nMapY = HUtil32.Str_ToInt(QuestActionInfo.sParam3, -1);
            string sMonName = QuestActionInfo.sParam4;
            int nRange = QuestActionInfo.nParam5;
            int nCount = QuestActionInfo.nParam6;
            if ((sMapName == "") || (nMapX <= 0) || (nMapY <= 0) || (sMapName == "") || (nRange <= 0) || (nCount <= 0))
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_MONGENEX);
                return;
            }
            for (var i = 0; i < nCount; i++)
            {
                int nRandX = M2Share.RandomNumber.Random(nRange * 2 + 1) + (nMapX - nRange);
                int nRandY = M2Share.RandomNumber.Random(nRange * 2 + 1) + (nMapY - nRange);
                if (M2Share.UserEngine.RegenMonsterByName(sMapName, (short)nRandX, (short)nRandY, sMonName) == null)
                {
                    // ScriptActionError(PlayObject,'',QuestActionInfo,sSC_MONGENEX);
                    break;
                }
            }
        }

        private void ActionOfOpenMagicBox(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            short nX = 0;
            short nY = 0;
            string sMonName = QuestActionInfo.sParam1;
            if (sMonName == "")
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_OPENMAGICBOX);
                return;
            }
            PlayObject.GetFrontPosition(ref nX, ref nY);
            var Monster = M2Share.UserEngine.RegenMonsterByName(PlayObject.m_PEnvir.sMapName, nX, nY, sMonName);
            if (Monster == null)
            {
                return;
            }
            Monster.Die();
        }

        private void ActionOfPkZone(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            TFireBurnEvent FireBurnEvent;
            int nRange = HUtil32.Str_ToInt(QuestActionInfo.sParam1, -1);
            int nType = HUtil32.Str_ToInt(QuestActionInfo.sParam2, -1);
            int nTime = HUtil32.Str_ToInt(QuestActionInfo.sParam3, -1);
            int nPoint = HUtil32.Str_ToInt(QuestActionInfo.sParam4, -1);
            if ((nRange < 0) || (nType < 0) || (nTime < 0) || (nPoint < 0))
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_PKZONE);
                return;
            }
            int nMinX = this.m_nCurrX - nRange;
            int nMaxX = this.m_nCurrX + nRange;
            int nMinY = this.m_nCurrY - nRange;
            int nMaxY = this.m_nCurrY + nRange;
            for (int nX = nMinX; nX <= nMaxX; nX++)
            {
                for (int nY = nMinY; nY <= nMaxY; nY++)
                {
                    if (((nX < nMaxX) && (nY == nMinY)) || ((nY < nMaxY) && (nX == nMinX)) || (nX == nMaxX) ||
                        (nY == nMaxY))
                    {
                        FireBurnEvent = new TFireBurnEvent(PlayObject, nX, nY, nType, nTime * 1000, nPoint);
                        M2Share.EventManager.AddEvent(FireBurnEvent);
                    }
                }
            }
        }

        private void ActionOfPowerRate(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            var nRate = HUtil32.Str_ToInt(QuestActionInfo.sParam1, -1);
            var nTime = HUtil32.Str_ToInt(QuestActionInfo.sParam2, -1);
            if ((nRate < 0) || (nTime < 0))
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_POWERRATE);
                return;
            }
            PlayObject.m_nPowerRate = nRate;
            // PlayObject.m_dwPowerRateTime:=_MIN(High(Word),nTime);
            PlayObject.m_dwPowerRateTime = nTime;
            if (M2Share.g_Config.boShowScriptActionMsg)
            {
                PlayObject.SysMsg(format(M2Share.g_sChangePowerRateMsg, new object[] { PlayObject.m_nPowerRate / 100, PlayObject.m_dwPowerRateTime }), TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }

        private void ActionOfChangeMode(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            var nMode = QuestActionInfo.nParam1;
            var boOpen = HUtil32.Str_ToInt(QuestActionInfo.sParam2, -1) == 1;
            if (nMode >= 1 && nMode <= 3)
            {
                switch (nMode)
                {
                    case 1:
                        PlayObject.m_boAdminMode = boOpen;
                        if (PlayObject.m_boAdminMode)
                        {
                            PlayObject.SysMsg(M2Share.sGameMasterMode, TMsgColor.c_Green, TMsgType.t_Hint);
                        }
                        else
                        {
                            PlayObject.SysMsg(M2Share.sReleaseGameMasterMode, TMsgColor.c_Green, TMsgType.t_Hint);
                        }
                        break;
                    case 2:
                        PlayObject.m_boSuperMan = boOpen;
                        if (PlayObject.m_boSuperMan)
                        {
                            PlayObject.SysMsg(M2Share.sSupermanMode, TMsgColor.c_Green, TMsgType.t_Hint);
                        }
                        else
                        {
                            PlayObject.SysMsg(M2Share.sReleaseSupermanMode, TMsgColor.c_Green, TMsgType.t_Hint);
                        }
                        break;
                    case 3:
                        PlayObject.m_boObMode = boOpen;
                        if (PlayObject.m_boObMode)
                        {
                            PlayObject.SysMsg(M2Share.sObserverMode, TMsgColor.c_Green, TMsgType.t_Hint);
                        }
                        else
                        {
                            PlayObject.SysMsg(M2Share.g_sReleaseObserverMode, TMsgColor.c_Green, TMsgType.t_Hint);
                        }
                        break;
                }
            }
            else
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_CHANGEMODE);
            }
        }

        private void ActionOfChangePerMission(TPlayObject PlayObject, TQuestActionInfo QuestActionInfo)
        {
            var nPermission = HUtil32.Str_ToInt(QuestActionInfo.sParam1, -1);
            if (nPermission >= 0 && nPermission <= 10)
            {
                PlayObject.m_btPermission = (byte)nPermission;
            }
            else
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, M2Share.sSC_CHANGEPERMISSION);
                return;
            }
            if (M2Share.g_Config.boShowScriptActionMsg)
            {
                PlayObject.SysMsg(format(M2Share.g_sChangePermissionMsg, new byte[] { PlayObject.m_btPermission }), TMsgColor.c_Green, TMsgType.t_Hint);
            }
        }

    }
}
