using GameSvr.Actor;
using GameSvr.Event.Events;
using GameSvr.GameCommand;
using GameSvr.Items;
using GameSvr.Maps;
using GameSvr.Player;
using GameSvr.Script;
using GameSvr.Services;
using SystemModule;
using SystemModule.Common;
using SystemModule.Data;
using SystemModule.Enums;
using SystemModule.Packets.ClientPackets;

namespace GameSvr.Npc
{
    public partial class NormNpc
    {
        /// <summary>
        /// 开通元宝交易
        /// </summary>
        private void ActionOfOPENYBDEAL(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            int nGameGold = 0;
            try
            {
                if (PlayObject.bo_YBDEAL)
                {
                    PlayObject.SendMsg(this, Grobal2.RM_MERCHANTSAY, 0, 0, 0, 0, ChrName + "/您已开通寄售服务,不需要再开通!!!\\ \\<返回/@main>");
                    return;// 如已开通元宝服务则退出
                }
                if (!GetValValue(PlayObject, QuestActionInfo.sParam1, ref nGameGold))
                {
                    nGameGold = HUtil32.StrToInt(GetLineVariableText(PlayObject, QuestActionInfo.sParam1), 0);
                }
                if (PlayObject.m_nGameGold >= nGameGold)
                {
                    // 玩家的元宝数大于或等于开通所需的元宝数
                    PlayObject.m_nGameGold -= nGameGold;
                    PlayObject.bo_YBDEAL = true;
                    PlayObject.SendMsg(this, Grobal2.RM_MERCHANTSAY, 0, 0, 0, 0, ChrName + "/开通寄售服务成功!!!\\ \\<返回/@main>");
                }
                else
                {
                    PlayObject.SendMsg(this, Grobal2.RM_MERCHANTSAY, 0, 0, 0, 0, ChrName + "/您身上没有" + M2Share.Config.GameGoldName + ",或" + M2Share.Config.GameGoldName + "数不够!!!\\ \\<返回/@main>");
                }
            }
            catch
            {
                M2Share.Log.LogError("{异常} TNormNpc.ActionOfOPENYBDEAL");
            }
        }

        /// <summary>
        /// 查询正在出售的物品
        /// </summary>
        private void ActionOfQUERYYBSELL(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            DealOffInfo DealOffInfo;
            string sSendStr;
            string sUserItemName;
            StdItem StdItem80;
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
                            ClientDealOffInfo sClientDealOffInfo = new ClientDealOffInfo();
                            sClientDealOffInfo.UseItems = new ClientItem[9];
                            for (var i = 0; i < M2Share.sSellOffItemList.Count; i++)
                            {
                                DealOffInfo = M2Share.sSellOffItemList[i];
                                if (string.Compare(DealOffInfo.sDealChrName, PlayObject.ChrName, StringComparison.OrdinalIgnoreCase) == 0 && (DealOffInfo.Flag == 0 || DealOffInfo.Flag == 3))
                                {
                                    for (var j = 0; j < 9; j++)
                                    {
                                        if (DealOffInfo.UseItems[j] == null)
                                        {
                                            continue;
                                        }

                                        StdItem StdItem = M2Share.WorldEngine.GetStdItem(DealOffInfo.UseItems[j].Index);
                                        if (StdItem == null)
                                        {
                                            // 是金刚石
                                            if (!bo12 && DealOffInfo.UseItems[j].MakeIndex > 0 && DealOffInfo.UseItems[j].Index == ushort.MaxValue && DealOffInfo.UseItems[j].Dura == ushort.MaxValue && DealOffInfo.UseItems[j].DuraMax == ushort.MaxValue)
                                            {
                                                ClientItem _wvar1 = sClientDealOffInfo.UseItems[j];// '金刚石'
                                                //_wvar1.S.Name = M2Share.g_Config.sGameDiaMond + '(' + (DealOffInfo.UseItems[K].MakeIndex).ToString() + ')';
                                                //_wvar1.S.Price = DealOffInfo.UseItems[K].MakeIndex;// 金刚石数量
                                                _wvar1.Dura = ushort.MaxValue;// 客户端金刚石特征
                                                _wvar1.Item.DuraMax = ushort.MaxValue;// 客户端金刚石特征
                                                _wvar1.Item.Looks = ushort.MaxValue;// 不显示图片
                                                bo12 = true;
                                            }
                                            else
                                            {
                                                sClientDealOffInfo.UseItems[j].Item.Name = "";
                                            }
                                            continue;
                                        }
                                        StdItem80 = StdItem;
                                        //M2Share.ItemUnit.GetItemAddValue(DealOffInfo.UseItems[K], ref StdItem80);
                                        //Move(StdItem80, sClientDealOffInfo.UseItems[K].S, sizeof(TStdItem));
                                        sClientDealOffInfo.UseItems[j] = new ClientItem();
                                        StdItem80.GetUpgradeStdItem(DealOffInfo.UseItems[j], ref sClientDealOffInfo.UseItems[j]);
                                        //sClientDealOffInfo.UseItems[j].S = StdItem80;
                                        // 取自定义物品名称
                                        sUserItemName = "";
                                        if (DealOffInfo.UseItems[j].Desc[13] == 1)
                                        {
                                            sUserItemName = M2Share.CustomItemMgr.GetCustomItemName(DealOffInfo.UseItems[j].MakeIndex, DealOffInfo.UseItems[j].Index);
                                        }
                                        if (sUserItemName != "")
                                        {
                                            sClientDealOffInfo.UseItems[j].Item.Name = sUserItemName;
                                        }
                                        sClientDealOffInfo.UseItems[j].MakeIndex = DealOffInfo.UseItems[j].MakeIndex;
                                        sClientDealOffInfo.UseItems[j].Dura = DealOffInfo.UseItems[j].Dura;
                                        sClientDealOffInfo.UseItems[j].DuraMax = DealOffInfo.UseItems[j].DuraMax;
                                        switch (StdItem.StdMode)
                                        {
                                            case 15:
                                            case 19:
                                            case 26:
                                                if (DealOffInfo.UseItems[j].Desc[8] != 0)
                                                {
                                                    sClientDealOffInfo.UseItems[j].Item.Shape = 130;
                                                }
                                                break;
                                        }
                                    }
                                    sClientDealOffInfo.DealChrName = DealOffInfo.sDealChrName;
                                    sClientDealOffInfo.BuyChrName = DealOffInfo.sBuyChrName;
                                    sClientDealOffInfo.SellDateTime = HUtil32.DateTimeToDouble(DealOffInfo.dSellDateTime);
                                    sClientDealOffInfo.SellGold = DealOffInfo.nSellGold;
                                    sClientDealOffInfo.N = DealOffInfo.Flag;
                                    sSendStr = EDCode.EncodeBuffer(sClientDealOffInfo);
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
                    PlayObject.SendMsg(PlayObject, Grobal2.RM_MENU_OK, 0, PlayObject.ActorId, 0, 0, "您未开通寄售服务,请先开通!!!");
                }
            }
            catch
            {
                M2Share.Log.LogError("{异常} TNormNpc.ActionOfQUERYYBSELL");
            }
        }

        /// <summary>
        /// 查询可以的购买物品
        /// </summary>
        private void ActionOfQueryTrustDeal(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            DealOffInfo DealOffInfo;
            string sSendStr;
            string sUserItemName;
            StdItem StdItem80;
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
                            ClientDealOffInfo sClientDealOffInfo = new ClientDealOffInfo();
                            sClientDealOffInfo.UseItems = new ClientItem[9];
                            for (var i = 0; i < M2Share.sSellOffItemList.Count; i++)
                            {
                                DealOffInfo = M2Share.sSellOffItemList[i];
                                if (string.Compare(DealOffInfo.sBuyChrName, PlayObject.ChrName, StringComparison.OrdinalIgnoreCase) == 0 && DealOffInfo.Flag == 0)
                                {
                                    for (var k = 0; k < 9; k++)
                                    {
                                        if (DealOffInfo.UseItems[k] == null)
                                        {
                                            continue;
                                        }

                                        StdItem StdItem = M2Share.WorldEngine.GetStdItem(DealOffInfo.UseItems[k].Index);
                                        if (StdItem == null)
                                        {
                                            // 是金刚石
                                            if (!bo12 && DealOffInfo.UseItems[k].MakeIndex > 0 && DealOffInfo.UseItems[k].Index == short.MaxValue && DealOffInfo.UseItems[k].Dura == short.MaxValue && DealOffInfo.UseItems[k].DuraMax == short.MaxValue)
                                            {
                                                ClientItem _wvar1 = sClientDealOffInfo.UseItems[k];// '金刚石'
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
                                                sClientDealOffInfo.UseItems[k].Item.Name = "";
                                            }
                                            continue;
                                        }
                                        StdItem80 = StdItem;
                                        //M2Share.ItemUnit.GetItemAddValue(DealOffInfo.UseItems[K], ref StdItem80);
                                        //Move(StdItem80, sClientDealOffInfo.UseItems[K].S);// 取自定义物品名称
                                        //sClientDealOffInfo.UseItems[K].S = StdItem80;
                                        sClientDealOffInfo.UseItems[k] = new ClientItem();
                                        //StdItem80.GetStandardItem(ref sClientDealOffInfo.UseItems[k].Item);
                                        sUserItemName = "";
                                        if (DealOffInfo.UseItems[k].Desc[13] == 1)
                                        {
                                            sUserItemName = M2Share.CustomItemMgr.GetCustomItemName(DealOffInfo.UseItems[k].MakeIndex, DealOffInfo.UseItems[k].Index);
                                        }
                                        if (sUserItemName != "")
                                        {
                                            sClientDealOffInfo.UseItems[k].Item.Name = sUserItemName;
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
                                                if (DealOffInfo.UseItems[k].Desc[8] != 0)
                                                {
                                                    sClientDealOffInfo.UseItems[k].Item.Shape = 130;
                                                }
                                                break;
                                        }
                                    }
                                    sClientDealOffInfo.DealChrName = DealOffInfo.sDealChrName;
                                    sClientDealOffInfo.BuyChrName = DealOffInfo.sBuyChrName;
                                    sClientDealOffInfo.SellDateTime = HUtil32.DateTimeToDouble(DealOffInfo.dSellDateTime);
                                    sClientDealOffInfo.SellGold = DealOffInfo.nSellGold;
                                    sClientDealOffInfo.N = DealOffInfo.Flag;
                                    sSendStr = EDCode.EncodeBuffer(sClientDealOffInfo);
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
                    PlayObject.SendMsg(PlayObject, Grobal2.RM_MENU_OK, 0, PlayObject.ActorId, 0, 0, "您未开通元宝寄售服务,请先开通!!!");
                }
            }
            catch
            {
                M2Share.Log.LogError("{异常} TNormNpc.ActionOfQueryTrustDeal");
            }
        }

        private void ActionOfAddNameDateList(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            string sLineText;
            var sHumName = string.Empty;
            var sDate = string.Empty;
            var sListFileName = Path.Combine(M2Share.BasePath, M2Share.Config.EnvirDir, m_sPath, QuestActionInfo.sParam1);
            using var LoadList = new StringList();
            if (File.Exists(sListFileName))
            {
                try
                {
                    LoadList.LoadFromFile(sListFileName);
                }
                catch
                {
                    M2Share.Log.LogError("loading fail.... => " + sListFileName);
                }
            }
            var boFound = false;
            for (var i = 0; i < LoadList.Count; i++)
            {
                sLineText = LoadList[i].Trim();
                sLineText = HUtil32.GetValidStr3(sLineText, ref sHumName, new[] { ' ', '\t' });
                sLineText = HUtil32.GetValidStr3(sLineText, ref sDate, new[] { ' ', '\t' });
                if (string.Compare(sHumName, PlayObject.ChrName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    LoadList[i] = PlayObject.ChrName + "\t" + DateTime.Today;
                    boFound = true;
                    break;
                }
            }
            if (!boFound)
            {
                LoadList.Add(PlayObject.ChrName + "\t" + DateTime.Today);
            }
            try
            {
                LoadList.SaveToFile(sListFileName);
            }
            catch
            {
                M2Share.Log.LogError("saving fail.... => " + sListFileName);
            }
        }

        private void ActionOfDelNameDateList(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            string sLineText;
            string sHumName = string.Empty;
            string sDate = string.Empty;
            var sListFileName = Path.Combine(M2Share.BasePath, M2Share.Config.EnvirDir, m_sPath, QuestActionInfo.sParam1);
            using var LoadList = new StringList();
            if (File.Exists(sListFileName))
            {
                try
                {
                    LoadList.LoadFromFile(sListFileName);
                }
                catch
                {
                    M2Share.Log.LogError("loading fail.... => " + sListFileName);
                }
            }
            var boFound = false;
            for (var i = 0; i < LoadList.Count; i++)
            {
                sLineText = LoadList[i].Trim();
                sLineText = HUtil32.GetValidStr3(sLineText, ref sHumName, new[] { ' ', '\t' });
                sLineText = HUtil32.GetValidStr3(sLineText, ref sDate, new[] { ' ', '\t' });
                if (string.Compare(sHumName, PlayObject.ChrName, StringComparison.OrdinalIgnoreCase) == 0)
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
                    M2Share.Log.LogError("saving fail.... => " + sListFileName);
                }
            }
        }

        private void ActionOfAddSkill(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            var nLevel = HUtil32._MIN(3, HUtil32.StrToInt(QuestActionInfo.sParam2, 0));
            var Magic = M2Share.WorldEngine.FindMagic(QuestActionInfo.sParam1);
            if (Magic != null)
            {
                if (!PlayObject.IsTrainingSkill(Magic.MagicId))
                {
                    var UserMagic = new UserMagic();
                    UserMagic.Magic = Magic;
                    UserMagic.MagIdx = Magic.MagicId;
                    UserMagic.Key = (char)0;
                    UserMagic.Level = (byte)nLevel;
                    UserMagic.TranPoint = 0;
                    PlayObject.MagicList.Add(UserMagic);
                    PlayObject.SendAddMagic(UserMagic);
                    PlayObject.RecalcAbilitys();
                    if (M2Share.Config.ShowScriptActionMsg)
                    {
                        PlayObject.SysMsg(Magic.MagicName + "练习成功。", MsgColor.Green, MsgType.Hint);
                    }
                }
            }
            else
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSC_ADDSKILL);
            }
        }

        private void ActionOfAutoAddGameGold(PlayObject PlayObject, QuestActionInfo QuestActionInfo, int nPoint, int nTime)
        {
            if (string.Compare(QuestActionInfo.sParam1, "START", StringComparison.OrdinalIgnoreCase) == 0)
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
            if (string.Compare(QuestActionInfo.sParam1, "STOP", StringComparison.OrdinalIgnoreCase) == 0)
            {
                PlayObject.m_boIncGameGold = false;
                return;
            }
            ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSC_AUTOADDGAMEGOLD);
        }

        // SETAUTOGETEXP 时间 点数 是否安全区 地图号
        private void ActionOfAutoGetExp(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            Envirnoment Envir = null;
            var nTime = HUtil32.StrToInt(QuestActionInfo.sParam1, -1);
            var nPoint = HUtil32.StrToInt(QuestActionInfo.sParam2, -1);
            var boIsSafeZone = QuestActionInfo.sParam3[1] == '1';
            var sMap = QuestActionInfo.sParam4;
            if (sMap != "")
            {
                Envir = M2Share.MapMgr.FindMap(sMap);
            }
            if (nTime <= 0 || nPoint <= 0 || sMap != "" && Envir == null)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSC_SETAUTOGETEXP);
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
        private void ActionOfOffLine(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            var sOffLineStartMsg = "系统已经为你开启了脱机泡点功能，你现在可以下线了……";
            PlayObject.m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_SYSMESSAGE, PlayObject.ActorId, HUtil32.MakeWord(M2Share.Config.CustMsgFColor, M2Share.Config.CustMsgBColor), 0, 1);
            PlayObject.SendSocket(PlayObject.m_DefMsg, EDCode.EncodeString(sOffLineStartMsg));
            var nTime = HUtil32.StrToInt(QuestActionInfo.sParam1, 5);
            var nPoint = HUtil32.StrToInt(QuestActionInfo.sParam2, 500);
            var nKickOffLine = HUtil32.StrToInt(QuestActionInfo.sParam3, 1440 * 15);
            PlayObject.m_boAutoGetExpInSafeZone = true;
            PlayObject.m_AutoGetExpEnvir = PlayObject.Envir;
            PlayObject.m_nAutoGetExpTime = nTime * 1000;
            PlayObject.m_nAutoGetExpPoint = nPoint;
            PlayObject.OffLineFlag = true;
            PlayObject.KickOffLineTick = HUtil32.GetTickCount() + nKickOffLine * 60 * 1000;
            IdSrvClient.Instance.SendHumanLogOutMsgA(PlayObject.UserID, PlayObject.m_nSessionID);
            PlayObject.SendDefMessage(Grobal2.SM_OUTOFCONNECTION, 0, 0, 0, 0, "");
        }

        private void ActionOfAutoSubGameGold(PlayObject PlayObject, QuestActionInfo QuestActionInfo, int nPoint, int nTime)
        {
            if (string.Compare(QuestActionInfo.sParam1, "START", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (nPoint > 0 && nTime > 0)
                {
                    PlayObject.m_nDecGameGold = nPoint;
                    PlayObject.m_dwDecGameGoldTime = nTime * 1000;
                    PlayObject.m_dwDecGameGoldTick = 0;
                    PlayObject.m_boDecGameGold = true;
                    return;
                }
            }
            if (string.Compare(QuestActionInfo.sParam1, "STOP", StringComparison.OrdinalIgnoreCase) == 0)
            {
                PlayObject.m_boDecGameGold = false;
                return;
            }
            ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSC_AUTOSUBGAMEGOLD);
        }

        private void ActionOfChangeCreditPoint(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            int nCreditPoint = HUtil32.StrToInt(QuestActionInfo.sParam2, -1);
            if (nCreditPoint < 0)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSC_CREDITPOINT);
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
                    ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSC_CREDITPOINT);
                    return;
            }
        }

        private void ActionOfChangeExp(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            int dwInt;
            var nExp = HUtil32.StrToInt(QuestActionInfo.sParam2, -1);
            if (nExp < 0)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSC_CHANGEEXP);
                return;
            }
            var cMethod = QuestActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (nExp > 0)
                    {
                        PlayObject.Abil.Exp = nExp;
                    }
                    break;
                case '-':
                    if (PlayObject.Abil.Exp > nExp)
                    {
                        PlayObject.Abil.Exp -= nExp;
                    }
                    else
                    {
                        PlayObject.Abil.Exp = 0;
                    }
                    break;
                case '+':
                    if (PlayObject.Abil.Exp >= nExp)
                    {
                        if (PlayObject.Abil.Exp - nExp > int.MaxValue - PlayObject.Abil.Exp)
                        {
                            dwInt = int.MaxValue - PlayObject.Abil.Exp;
                        }
                        else
                        {
                            dwInt = nExp;
                        }
                    }
                    else
                    {
                        if (nExp - PlayObject.Abil.Exp > int.MaxValue - nExp)
                        {
                            dwInt = int.MaxValue - nExp;
                        }
                        else
                        {
                            dwInt = nExp;
                        }
                    }
                    PlayObject.Abil.Exp += dwInt;
                    // PlayObject.GetExp(dwInt);
                    PlayObject.SendMsg(PlayObject, Grobal2.RM_WINEXP, 0, dwInt, 0, 0, "");
                    break;
            }
        }

        private void ActionOfChangeHairStyle(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            var nHair = HUtil32.StrToInt(QuestActionInfo.sParam1, -1);
            if (QuestActionInfo.sParam1 != "" && nHair >= 0)
            {
                PlayObject.Hair = (byte)nHair;
                PlayObject.FeatureChanged();
            }
            else
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSC_HAIRSTYLE);
            }
        }

        private void ActionOfChangeJob(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            PlayJob nJob = PlayJob.None;
            if (HUtil32.CompareLStr(QuestActionInfo.sParam1, ScriptConst.sWarrior))
            {
                nJob = PlayJob.Warrior;
            }
            if (HUtil32.CompareLStr(QuestActionInfo.sParam1, ScriptConst.sWizard))
            {
                nJob = PlayJob.Wizard;
            }
            if (HUtil32.CompareLStr(QuestActionInfo.sParam1, ScriptConst.sTaos))
            {
                nJob = PlayJob.Taoist;
            }
            if (nJob == PlayJob.None)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSC_CHANGEJOB);
                return;
            }
            if (PlayObject.Job != nJob)
            {
                PlayObject.Job = nJob;
                PlayObject.HasLevelUp(0);
            }
        }

        private void ActionOfChangeLevel(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            int nLv;
            var boChgOK = false;
            int nOldLevel = PlayObject.Abil.Level;
            var nLevel = HUtil32.StrToInt(QuestActionInfo.sParam2, -1);
            if (nLevel < 0)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSC_CHANGELEVEL);
                return;
            }
            var cMethod = QuestActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (nLevel > 0 && nLevel <= Grobal2.MaxLevel)
                    {
                        PlayObject.Abil.Level = (byte)nLevel;
                        boChgOK = true;
                    }
                    break;
                case '-':
                    nLv = HUtil32._MAX(0, PlayObject.Abil.Level - nLevel);
                    nLv = HUtil32._MIN(Grobal2.MaxLevel, nLv);
                    PlayObject.Abil.Level = (byte)nLv;
                    boChgOK = true;
                    break;
                case '+':
                    nLv = HUtil32._MAX(0, PlayObject.Abil.Level + nLevel);
                    nLv = HUtil32._MIN(Grobal2.MaxLevel, nLv);
                    PlayObject.Abil.Level = (byte)nLv;
                    boChgOK = true;
                    break;
            }
            if (boChgOK)
            {
                PlayObject.HasLevelUp(nOldLevel);
            }
        }

        private void ActionOfChangePkPoint(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            int nPoint;
            var nOldPKLevel = PlayObject.PvpLevel();
            var nPKPoint = HUtil32.StrToInt(QuestActionInfo.sParam2, -1);
            if (nPKPoint < 0)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSC_CHANGEPKPOINT);
                return;
            }
            var cMethod = QuestActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (nPKPoint >= 0)
                    {
                        PlayObject.PkPoint = nPKPoint;
                    }
                    break;
                case '-':
                    nPoint = HUtil32._MAX(0, PlayObject.PkPoint - nPKPoint);
                    PlayObject.PkPoint = nPoint;
                    break;
                case '+':
                    nPoint = HUtil32._MAX(0, PlayObject.PkPoint + nPKPoint);
                    PlayObject.PkPoint = nPoint;
                    break;
            }
            if (nOldPKLevel != PlayObject.PvpLevel())
            {
                PlayObject.RefNameColor();
            }
        }

        private void ActionOfClearMapMon(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            IList<BaseObject> MonList = new List<BaseObject>();
            var monsterCount = M2Share.WorldEngine.GetMapMonster(M2Share.MapMgr.FindMap(QuestActionInfo.sParam1), MonList);
            for (var i = 0; i < monsterCount; i++)
            {
                BaseObject Mon = MonList[i];
                if (Mon.Master != null)
                {
                    continue;
                }
                if (M2Share.GetNoClearMonList(Mon.ChrName))
                {
                    continue;
                }
                Mon.NoItem = true;
                Mon.WAbil.HP = 0;
                Mon.MakeGhost();
            }
        }

        private void ActionOfClearNameList(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            var sListFileName = Path.Combine(M2Share.BasePath, M2Share.Config.EnvirDir, m_sPath, QuestActionInfo.sParam1);
            using var LoadList = new StringList();
            LoadList.Clear();
            try
            {
                LoadList.SaveToFile(sListFileName);
            }
            catch
            {
                M2Share.Log.LogError("saving fail.... => " + sListFileName);
            }
        }

        private void ActionOfClearSkill(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            UserMagic UserMagic;
            for (var i = PlayObject.MagicList.Count - 1; i >= 0; i--)
            {
                UserMagic = PlayObject.MagicList[i];
                PlayObject.SendDelMagic(UserMagic);
                Dispose(UserMagic);
                PlayObject.MagicList.RemoveAt(i);
            }
            PlayObject.RecalcAbilitys();
        }

        private void ActionOfDelNoJobSkill(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            for (var i = PlayObject.MagicList.Count - 1; i >= 0; i--)
            {
                var UserMagic = PlayObject.MagicList[i];
                if (UserMagic.Magic.Job != (byte)PlayObject.Job)
                {
                    PlayObject.SendDelMagic(UserMagic);
                    Dispose(UserMagic);
                    PlayObject.MagicList.RemoveAt(i);
                }
            }
        }

        private void ActionOfDelSkill(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            UserMagic UserMagic;
            var sMagicName = QuestActionInfo.sParam1;
            var Magic = M2Share.WorldEngine.FindMagic(sMagicName);
            if (Magic == null)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSC_DELSKILL);
                return;
            }
            for (var i = 0; i < PlayObject.MagicList.Count; i++)
            {
                UserMagic = PlayObject.MagicList[i];
                if (UserMagic.Magic == Magic)
                {
                    PlayObject.MagicList.RemoveAt(i);
                    PlayObject.SendDelMagic(UserMagic);
                    Dispose(UserMagic);
                    PlayObject.RecalcAbilitys();
                    break;
                }
            }
        }

        private void ActionOfGameGold(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            var nOldGameGold = PlayObject.m_nGameGold;
            var nGameGold = HUtil32.StrToInt(QuestActionInfo.sParam2, -1);
            if (nGameGold < 0)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSC_GAMEGOLD);
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
            if (M2Share.GameLogGameGold)
            {
                M2Share.EventSource.AddEventLog(Grobal2.LOG_GAMEGOLD, Format(CommandHelp.GameLogMsg1, PlayObject.MapName, PlayObject.CurrX, PlayObject.CurrY, PlayObject.ChrName, M2Share.Config.GameGoldName, nGameGold, cMethod, ChrName));
            }
            if (nOldGameGold != PlayObject.m_nGameGold)
            {
                PlayObject.GameGoldChanged();
            }
        }

        private void ActionOfGamePoint(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            var nOldGamePoint = PlayObject.m_nGamePoint;
            var nGamePoint = HUtil32.StrToInt(QuestActionInfo.sParam2, -1);
            if (nGamePoint < 0)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSC_GAMEPOINT);
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
            if (M2Share.GameLogGamePoint)
            {
                M2Share.EventSource.AddEventLog(Grobal2.LOG_GAMEPOINT, Format(CommandHelp.GameLogMsg1, PlayObject.MapName, PlayObject.CurrX, PlayObject.CurrY, PlayObject.ChrName, M2Share.Config.GamePointName, nGamePoint, cMethod, ChrName));
            }
            if (nOldGamePoint != PlayObject.m_nGamePoint)
            {
                PlayObject.GameGoldChanged();
            }
        }

        private void ActionOfGetMarry(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            var PoseBaseObject = PlayObject.GetPoseCreate();
            if (PoseBaseObject != null && PoseBaseObject.Race == ActorRace.Play && PoseBaseObject.Gender != PlayObject.Gender)
            {
                PlayObject.m_sDearName = PoseBaseObject.ChrName;
                PlayObject.RefShowName();
                PoseBaseObject.RefShowName();
            }
            else
            {
                GotoLable(PlayObject, "@MarryError", false);
            }
        }

        private void ActionOfGetMaster(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            var PoseBaseObject = PlayObject.GetPoseCreate();
            if (PoseBaseObject != null && PoseBaseObject.Race == ActorRace.Play && PoseBaseObject.Gender != PlayObject.Gender)
            {
                PlayObject.m_sMasterName = PoseBaseObject.ChrName;
                PlayObject.RefShowName();
                PoseBaseObject.RefShowName();
            }
            else
            {
                GotoLable(PlayObject, "@MasterError", false);
            }
        }

        private void ActionOfLineMsg(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            var sMsg = GetLineVariableText(PlayObject, QuestActionInfo.sParam2);
            sMsg = sMsg.Replace("%s", PlayObject.ChrName);
            sMsg = sMsg.Replace("%d", ChrName);
            switch (QuestActionInfo.nParam1)
            {
                case 0:
                    M2Share.WorldEngine.SendBroadCastMsg(sMsg, MsgType.System);
                    break;
                case 1:
                    M2Share.WorldEngine.SendBroadCastMsg("(*) " + sMsg, MsgType.System);
                    break;
                case 2:
                    M2Share.WorldEngine.SendBroadCastMsg('[' + ChrName + ']' + sMsg, MsgType.System);
                    break;
                case 3:
                    M2Share.WorldEngine.SendBroadCastMsg('[' + PlayObject.ChrName + ']' + sMsg, MsgType.System);
                    break;
                case 4:
                    ProcessSayMsg(sMsg);
                    break;
                case 5:
                    PlayObject.SysMsg(sMsg, MsgColor.Red, MsgType.Say);
                    break;
                case 6:
                    PlayObject.SysMsg(sMsg, MsgColor.Green, MsgType.Say);
                    break;
                case 7:
                    PlayObject.SysMsg(sMsg, MsgColor.Blue, MsgType.Say);
                    break;
                case 8:
                    PlayObject.SendGroupText(sMsg);
                    break;
                case 9:
                    if (PlayObject.MyGuild != null)
                    {
                        PlayObject.MyGuild.SendGuildMsg(sMsg);
                        M2Share.WorldEngine.SendServerGroupMsg(Grobal2.SS_208, M2Share.ServerIndex, PlayObject.MyGuild.sGuildName + "/" + PlayObject.ChrName + "/" + sMsg);
                    }
                    break;
                default:
                    ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSENDMSG);
                    break;
            }
        }

        private void ActionOfMapTing(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {

        }

        private void ActionOfMarry(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            string sSayMsg;
            if (PlayObject.m_sDearName != "")
            {
                return;
            }
            var PoseHuman = (PlayObject)PlayObject.GetPoseCreate();
            if (PoseHuman == null)
            {
                GotoLable(PlayObject, "@MarryCheckDir", false);
                return;
            }
            if (QuestActionInfo.sParam1 == "")
            {
                if (PoseHuman.Race != ActorRace.Play)
                {
                    GotoLable(PlayObject, "@HumanTypeErr", false);
                    return;
                }
                if (PoseHuman.GetPoseCreate() == PlayObject)
                {
                    if (PlayObject.Gender != PoseHuman.Gender)
                    {
                        GotoLable(PlayObject, "@StartMarry", false);
                        GotoLable(PoseHuman, "@StartMarry", false);
                        if (PlayObject.Gender == PlayGender.Man && PoseHuman.Gender == PlayGender.WoMan)
                        {
                            sSayMsg = string.Format(M2Share.g_sStartMarryManMsg, ChrName, PlayObject.ChrName, PoseHuman.ChrName);
                            M2Share.WorldEngine.SendBroadCastMsg(sSayMsg, MsgType.Say);
                            sSayMsg = string.Format(M2Share.g_sStartMarryManAskQuestionMsg, ChrName, PlayObject.ChrName, PoseHuman.ChrName);
                            M2Share.WorldEngine.SendBroadCastMsg(sSayMsg, MsgType.Say);
                        }
                        else if (PlayObject.Gender == PlayGender.WoMan && PoseHuman.Gender == PlayGender.Man)
                        {
                            sSayMsg = string.Format(M2Share.g_sStartMarryWoManMsg, ChrName, PlayObject.ChrName, PoseHuman.ChrName);
                            M2Share.WorldEngine.SendBroadCastMsg(sSayMsg, MsgType.Say);
                            sSayMsg = string.Format(M2Share.g_sStartMarryWoManAskQuestionMsg, ChrName, PlayObject.ChrName, PoseHuman.ChrName);
                            M2Share.WorldEngine.SendBroadCastMsg(sSayMsg, MsgType.Say);
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
            if (string.Compare(QuestActionInfo.sParam1, "REQUESTMARRY", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (PlayObject.m_boStartMarry && PoseHuman.m_boStartMarry)
                {
                    if (PlayObject.Gender == PlayGender.Man && PoseHuman.Gender == PlayGender.WoMan)
                    {
                        sSayMsg = M2Share.g_sMarryManAnswerQuestionMsg.Replace("%n", ChrName);
                        sSayMsg = sSayMsg.Replace("%s", PlayObject.ChrName);
                        sSayMsg = sSayMsg.Replace("%d", PoseHuman.ChrName);
                        M2Share.WorldEngine.SendBroadCastMsg(sSayMsg, MsgType.Say);
                        sSayMsg = M2Share.g_sMarryManAskQuestionMsg.Replace("%n", ChrName);
                        sSayMsg = sSayMsg.Replace("%s", PlayObject.ChrName);
                        sSayMsg = sSayMsg.Replace("%d", PoseHuman.ChrName);
                        M2Share.WorldEngine.SendBroadCastMsg(sSayMsg, MsgType.Say);
                        GotoLable(PlayObject, "@WateMarry", false);
                        GotoLable(PoseHuman, "@RevMarry", false);
                    }
                }
                return;
            }
            // sRESPONSEMARRY
            if (string.Compare(QuestActionInfo.sParam1, "RESPONSEMARRY", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (PlayObject.Gender == PlayGender.WoMan && PoseHuman.Gender == PlayGender.Man)
                {
                    if (string.Compare(QuestActionInfo.sParam2, "OK", StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        if (PlayObject.m_boStartMarry && PoseHuman.m_boStartMarry)
                        {
                            sSayMsg = string.Format(M2Share.g_sMarryWoManAnswerQuestionMsg, ChrName, PlayObject.ChrName, PoseHuman.ChrName);
                            M2Share.WorldEngine.SendBroadCastMsg(sSayMsg, MsgType.Say);
                            sSayMsg = string.Format(M2Share.g_sMarryWoManGetMarryMsg, ChrName, PlayObject.ChrName, PoseHuman.ChrName);
                            M2Share.WorldEngine.SendBroadCastMsg(sSayMsg, MsgType.Say);
                            GotoLable(PlayObject, "@EndMarry", false);
                            GotoLable(PoseHuman, "@EndMarry", false);
                            PlayObject.m_boStartMarry = false;
                            PoseHuman.m_boStartMarry = false;
                            PlayObject.m_sDearName = PoseHuman.ChrName;
                            PlayObject.m_DearHuman = PoseHuman;
                            PoseHuman.m_sDearName = PlayObject.ChrName;
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
                            sSayMsg = string.Format(M2Share.g_sMarryWoManDenyMsg, ChrName, PlayObject.ChrName, PoseHuman.ChrName);
                            M2Share.WorldEngine.SendBroadCastMsg(sSayMsg, MsgType.Say);
                            sSayMsg = string.Format(M2Share.g_sMarryWoManCancelMsg, ChrName, PlayObject.ChrName, PoseHuman.ChrName);
                            M2Share.WorldEngine.SendBroadCastMsg(sSayMsg, MsgType.Say);
                        }
                    }
                }
            }
        }

        private void ActionOfMaster(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            if (PlayObject.m_sMasterName != "")
            {
                return;
            }
            var PoseHuman = (PlayObject)PlayObject.GetPoseCreate();
            if (PoseHuman == null)
            {
                GotoLable(PlayObject, "@MasterCheckDir", false);
                return;
            }
            if (QuestActionInfo.sParam1 == "")
            {
                if (PoseHuman.Race != ActorRace.Play)
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
            if (string.Compare(QuestActionInfo.sParam1, "REQUESTMASTER", StringComparison.OrdinalIgnoreCase) == 0)
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
            if (string.Compare(QuestActionInfo.sParam1, "RESPONSEMASTER", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (string.Compare(QuestActionInfo.sParam2, "OK", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    if (PlayObject.m_PoseBaseObject == PoseHuman && PoseHuman.m_PoseBaseObject == PlayObject)
                    {
                        if (PlayObject.m_boStartMaster && PoseHuman.m_boStartMaster)
                        {
                            GotoLable(PlayObject, "@EndMaster", false);
                            GotoLable(PoseHuman, "@EndMaster", false);
                            PlayObject.m_boStartMaster = false;
                            PoseHuman.m_boStartMaster = false;
                            if (PlayObject.m_sMasterName == "")
                            {
                                PlayObject.m_sMasterName = PoseHuman.ChrName;
                                PlayObject.m_boMaster = true;
                            }
                            PlayObject.m_MasterList.Add(PoseHuman);
                            PoseHuman.m_sMasterName = PlayObject.ChrName;
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

        private void ActionOfMessageBox(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            PlayObject.SendMsg(this, Grobal2.RM_MENU_OK, 0, PlayObject.ActorId, 0, 0, GetLineVariableText(PlayObject, QuestActionInfo.sParam1));
        }

        private void ActionOfMission(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            if (QuestActionInfo.sParam1 != "" && QuestActionInfo.nParam2 > 0 && QuestActionInfo.nParam3 > 0)
            {
                M2Share.g_sMissionMap = QuestActionInfo.sParam1;
                M2Share.g_nMissionX = (short)QuestActionInfo.nParam2;
                M2Share.g_nMissionY = (short)QuestActionInfo.nParam3;
            }
            else
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSC_MISSION);
            }
        }

        // MOBFIREBURN MAP X Y TYPE TIME POINT
        private void ActionOfMobFireBurn(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            var sMap = QuestActionInfo.sParam1;
            var nX = (short)HUtil32.StrToInt(QuestActionInfo.sParam2, -1);
            var nY = (short)HUtil32.StrToInt(QuestActionInfo.sParam3, -1);
            var nType = (byte)HUtil32.StrToInt(QuestActionInfo.sParam4, -1);
            var nTime = HUtil32.StrToInt(QuestActionInfo.sParam5, -1);
            var nPoint = HUtil32.StrToInt(QuestActionInfo.sParam6, -1);
            if (sMap == "" || nX < 0 || nY < 0 || nType < 0 || nTime < 0 || nPoint < 0)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSC_MOBFIREBURN);
                return;
            }
            var Envir = M2Share.MapMgr.FindMap(sMap);
            if (Envir != null)
            {
                var OldEnvir = PlayObject.Envir;
                PlayObject.Envir = Envir;
                var FireBurnEvent = new FireBurnEvent(PlayObject, nX, nY, nType, nTime * 1000, nPoint);
                M2Share.EventMgr.AddEvent(FireBurnEvent);
                PlayObject.Envir = OldEnvir;
                return;
            }
            ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSC_MOBFIREBURN);
        }

        private void ActionOfMobPlace(PlayObject PlayObject, QuestActionInfo QuestActionInfo, int nX, int nY, int nCount, int nRange)
        {
            short nRandX;
            short nRandY;
            BaseObject Mon;
            for (var i = 0; i < nCount; i++)
            {
                nRandX = (short)(M2Share.RandomNumber.Random(nRange * 2 + 1) + (nX - nRange));
                nRandY = (short)(M2Share.RandomNumber.Random(nRange * 2 + 1) + (nY - nRange));
                Mon = M2Share.WorldEngine.RegenMonsterByName(M2Share.g_sMissionMap, nRandX, nRandY, QuestActionInfo.sParam1);
                if (Mon != null)
                {
                    Mon.Mission = true;
                    Mon.MissionX = M2Share.g_nMissionX;
                    Mon.MissionY = M2Share.g_nMissionY;
                }
                else
                {
                    ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSC_MOBPLACE);
                    break;
                }
            }
        }

        private void ActionOfRecallGroupMembers(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
        }

        private void ActionOfSetRankLevelName(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            var sRankLevelName = QuestActionInfo.sParam1;
            if (sRankLevelName == "")
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSC_SKILLLEVEL);
                return;
            }
            PlayObject.m_sRankLevelName = sRankLevelName;
            PlayObject.RefShowName();
        }

        private void ActionOfSetScriptFlag(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            var nWhere = HUtil32.StrToInt(QuestActionInfo.sParam1, -1);
            var boFlag = HUtil32.StrToInt(QuestActionInfo.sParam2, -1) == 1;
            switch (nWhere)
            {
                case 0:
                    PlayObject.m_boSendMsgFlag = boFlag;
                    break;
                case 1:
                    PlayObject.m_boChangeItemNameFlag = boFlag;
                    break;
                default:
                    ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSC_SETSCRIPTFLAG);
                    break;
            }
        }

        private void ActionOfSkillLevel(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            UserMagic UserMagic;
            var nLevel = HUtil32.StrToInt(QuestActionInfo.sParam3, 0);
            if (nLevel < 0)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSC_SKILLLEVEL);
                return;
            }
            var cMethod = QuestActionInfo.sParam2[0];
            var Magic = M2Share.WorldEngine.FindMagic(QuestActionInfo.sParam1);
            if (Magic != null)
            {
                for (var i = 0; i < PlayObject.MagicList.Count; i++)
                {
                    UserMagic = PlayObject.MagicList[i];
                    if (UserMagic.Magic == Magic)
                    {
                        switch (cMethod)
                        {
                            case '=':
                                if (nLevel >= 0)
                                {
                                    nLevel = HUtil32._MAX(3, nLevel);
                                    UserMagic.Level = (byte)nLevel;
                                }
                                break;
                            case '-':
                                if (UserMagic.Level >= nLevel)
                                {
                                    UserMagic.Level -= (byte)nLevel;
                                }
                                else
                                {
                                    UserMagic.Level = 0;
                                }
                                break;
                            case '+':
                                if (UserMagic.Level + nLevel <= 3)
                                {
                                    UserMagic.Level += (byte)nLevel;
                                }
                                else
                                {
                                    UserMagic.Level = 3;
                                }
                                break;
                        }
                        PlayObject.SendDelayMsg(PlayObject, Grobal2.RM_MAGIC_LVEXP, 0, UserMagic.Magic.MagicId, UserMagic.Level, UserMagic.TranPoint, "", 100);
                        break;
                    }
                }
            }
        }

        private void ActionOfTakeCastleGold(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            var nGold = HUtil32.StrToInt(QuestActionInfo.sParam1, -1);
            if (nGold < 0 || Castle == null)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSC_TAKECASTLEGOLD);
                return;
            }
            if (nGold <= Castle.TotalGold)
            {
                Castle.TotalGold -= nGold;
            }
            else
            {
                Castle.TotalGold = 0;
            }
        }

        private void ActionOfUnMarry(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            if (PlayObject.m_sDearName == "")
            {
                GotoLable(PlayObject, "@ExeMarryFail", false);
                return;
            }
            var PoseHuman = (PlayObject)PlayObject.GetPoseCreate();
            if (PoseHuman == null)
            {
                GotoLable(PlayObject, "@UnMarryCheckDir", false);
            }
            if (PoseHuman != null)
            {
                if (QuestActionInfo.sParam1 == "")
                {
                    if (PoseHuman.Race != ActorRace.Play)
                    {
                        GotoLable(PlayObject, "@UnMarryTypeErr", false);
                        return;
                    }
                    if (PoseHuman.GetPoseCreate() == PlayObject)
                    {
                        // and (PosHum.AddInfo.sDearName = Hum.sName)
                        if (PlayObject.m_sDearName == PoseHuman.ChrName)
                        {
                            GotoLable(PlayObject, "@StartUnMarry", false);
                            GotoLable(PoseHuman, "@StartUnMarry", false);
                            return;
                        }
                    }
                }
            }
            // sREQUESTUNMARRY
            if (string.Compare(QuestActionInfo.sParam1, "REQUESTUNMARRY", StringComparison.OrdinalIgnoreCase) == 0)
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
                            M2Share.WorldEngine.SendBroadCastMsg('[' + ChrName + "]: " + "我宣布" + PoseHuman.ChrName + ' ' + '与' + PlayObject.ChrName + ' ' + ' ' + "正式脱离夫妻关系。", MsgType.Say);
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
                    if (string.Compare(QuestActionInfo.sParam2, "FORCE", StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        M2Share.WorldEngine.SendBroadCastMsg('[' + ChrName + "]: " + "我宣布" + PlayObject.ChrName + ' ' + '与' + PlayObject.m_sDearName + ' ' + ' ' + "已经正式脱离夫妻关系!!!", MsgType.Say);
                        PoseHuman = M2Share.WorldEngine.GetPlayObject(PlayObject.m_sDearName);
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

        /// <summary>
        /// 保存变量值
        /// SAVEVAR 变量类型 变量名 文件名
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="QuestActionInfo"></param>
        private void ActionOfSaveVar(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            string sName = string.Empty;
            TDynamicVar DynamicVar = null;
            bool boFoundVar;
            Dictionary<string, TDynamicVar> DynamicVarList;
            ConfFile IniFile;
            const string sVarFound = "变量{0}不存在，变量类型:{1}";
            const string sVarTypeError = "变量类型错误，错误类型:{0} 当前支持类型(HUMAN、GUILD、GLOBAL)";
            var sType = QuestActionInfo.sParam1;
            var sVarName = QuestActionInfo.sParam2;
            var sFileName = GetLineVariableText(PlayObject, QuestActionInfo.sParam3);
            if (sFileName[0] == '\\')
            {
                sFileName = sFileName.Substring(1, sFileName.Length - 1);
            }
            if (sFileName[1] == '\\')
            {
                sFileName = sFileName.Substring(2, sFileName.Length - 2);
            }
            if (sFileName[2] == '\\')
            {
                sFileName = sFileName.Substring(3, sFileName.Length - 3);
            }
            sFileName = Path.Combine(M2Share.BasePath, M2Share.Config.EnvirDir, sFileName);
            if (sType == "" || sVarName == "" || !File.Exists(sFileName))
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSC_SAVEVAR);
                return;
            }
            boFoundVar = false;
            DynamicVarList = GetDynamicVarList(PlayObject, sType, ref sName);
            if (DynamicVarList == null)
            {
                Dispose(DynamicVar);
                ScriptActionError(PlayObject, Format(sVarTypeError, sType), QuestActionInfo, ScriptConst.sSC_VAR);
                return;
            }
            if (DynamicVarList.TryGetValue(sVarName, out DynamicVar))
            {
                IniFile = new ConfFile(sFileName);
                IniFile.Load();
                if (DynamicVar.VarType == VarType.Integer)
                {
                    DynamicVarList[sVarName].nInternet = DynamicVar.nInternet;
                    IniFile.WriteInteger(sName, DynamicVar.sName, DynamicVar.nInternet);
                }
                else
                {
                    DynamicVarList[sVarName].sString = DynamicVar.sString;
                    IniFile.WriteString(sName, DynamicVar.sName, DynamicVar.sString);
                }
                boFoundVar = true;
            }
            if (!boFoundVar)
            {
                ScriptActionError(PlayObject, Format(sVarFound, sVarName, sType), QuestActionInfo, ScriptConst.sSC_SAVEVAR);
            }
        }

        private void ActionOfDelayCall(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            PlayObject.m_nDelayCall = HUtil32._MAX(1, QuestActionInfo.nParam1);
            PlayObject.m_sDelayCallLabel = QuestActionInfo.sParam2;
            PlayObject.m_dwDelayCallTick = HUtil32.GetTickCount();
            PlayObject.m_boDelayCall = true;
            PlayObject.m_DelayCallNPC = ActorId;
        }

        /// <summary>
        /// 对变量进行运算(+、-、*、/)
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="QuestActionInfo"></param>
        private void ActionOfCalcVar(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            string sName = string.Empty;
            TDynamicVar DynamicVar = null;
            string sVarValue2 = string.Empty;
            Dictionary<string, TDynamicVar> DynamicVarList;
            const string sVarFound = "变量{0}不存在，变量类型:{1}";
            const string sVarTypeError = "变量类型错误，错误类型:{0} 当前支持类型(HUMAN、GUILD、GLOBAL)";
            string sType = QuestActionInfo.sParam1;//类型
            string sVarName = QuestActionInfo.sParam2;//自定义变量
            string sMethod = QuestActionInfo.sParam3;//操作符 +-*/=
            string sVarValue = QuestActionInfo.sParam4;//变量
            int nVarValue = HUtil32.StrToInt(QuestActionInfo.sParam4, 0);
            if (sType == "" || sVarName == "" || sMethod == "")
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSC_CALCVAR);
                return;
            }
            bool boFoundVar = false;
            if (!string.IsNullOrEmpty(sVarValue) && !HUtil32.IsStringNumber(sVarValue))
            {
                if (HUtil32.CompareLStr(sVarValue, "<$HUMAN(", 8))
                {
                    HUtil32.ArrestStringEx(sVarValue, "(", ")", ref sVarValue2);
                    sVarValue = sVarValue2;
                    if (PlayObject.m_DynamicVarList.Count > 0)
                    {
                        if (PlayObject.m_DynamicVarList.TryGetValue(sVarValue, out DynamicVar))
                        {
                            switch (DynamicVar.VarType)
                            {
                                case VarType.Integer:
                                    nVarValue = DynamicVar.nInternet;
                                    break;
                                case VarType.String:
                                    sVarValue = DynamicVar.sString;
                                    break;
                            }
                            boFoundVar = true;
                        }
                    }
                    if (!boFoundVar)
                    {
                        ScriptActionError(PlayObject, string.Format(sVarFound, sVarValue, sType), QuestActionInfo, ScriptConst.sSC_CALCVAR);
                        return;
                    }
                }
                else
                {
                    nVarValue = HUtil32.StrToInt(GetLineVariableText(PlayObject, sVarValue), 0);
                    sVarValue = GetLineVariableText(PlayObject, sVarValue);
                }
            }
            else
            {
                nVarValue = HUtil32.StrToInt(QuestActionInfo.sParam4, 0);
            }
            char cMethod = sMethod[0];
            DynamicVarList = GetDynamicVarList(PlayObject, sType, ref sName);
            if (DynamicVarList == null)
            {
                Dispose(DynamicVar);
                ScriptActionError(PlayObject, Format(sVarTypeError, sType), QuestActionInfo, ScriptConst.sSC_CALCVAR);
                return;
            }

            if (PlayObject.m_DynamicVarList.TryGetValue(sVarName, out DynamicVar))
            {
                switch (DynamicVar.VarType)
                {
                    case VarType.Integer:
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
                    case VarType.String:
                        switch (cMethod)
                        {
                            case '=':
                                DynamicVar.sString = sVarValue;
                                break;
                            case '+':
                                DynamicVar.sString = DynamicVar.sString + sVarValue;
                                break;
                            case '-':
                                break;
                        }
                        break;
                }
                boFoundVar = true;
            }
            if (!boFoundVar)
            {
                ScriptActionError(PlayObject, Format(sVarFound, sVarName, sType), QuestActionInfo, ScriptConst.sSC_CALCVAR);
            }
        }

        private void ActionOfGuildRecall(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            if (PlayObject.MyGuild != null)
            {
                // PlayObject.GuildRecall('GuildRecall','');
            }
        }

        private void ActionOfGroupAddList(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            string ffile = QuestActionInfo.sParam1;
            if (PlayObject.GroupOwner != null)
            {
                for (var i = 0; i < PlayObject.GroupMembers.Count; i++)
                {
                    PlayObject = PlayObject.GroupMembers[i];
                    // AddListEx(PlayObject.m_sChrName,ffile);
                }
            }
        }

        private void ActionOfClearList(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            var ffile = Path.Combine(M2Share.BasePath, M2Share.Config.EnvirDir, QuestActionInfo.sParam1);
            if (File.Exists(ffile))
            {
                //myFile = new FileInfo(ffile);
                //_W_0 = myFile.CreateText();
                //_W_0.Close();
            }
        }

        private void ActionOfGroupRecall(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            if (PlayObject.GroupOwner != null)
            {
                // PlayObject.GroupRecall('GroupRecall');
            }
        }

        /// <summary>
        /// 特修身上所有装备
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="QuestActionInfo"></param>
        private void ActionOfRepairAllItem(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            string sUserItemName;
            bool boIsHasItem = false;
            for (var i = 0; i < PlayObject.UseItems.Length; i++)
            {
                if (PlayObject.UseItems[i].Index <= 0)
                {
                    continue;
                }
                sUserItemName = M2Share.WorldEngine.GetStdItemName(PlayObject.UseItems[i].Index);
                if (!(i != Grobal2.U_CHARM))
                {
                    PlayObject.SysMsg(sUserItemName + " 禁止修理...", MsgColor.Red, MsgType.Hint);
                    continue;
                }
                PlayObject.UseItems[i].Dura = PlayObject.UseItems[i].DuraMax;
                PlayObject.SendMsg(this, Grobal2.RM_DURACHANGE, (short)i, PlayObject.UseItems[i].Dura, PlayObject.UseItems[i].DuraMax, 0, "");
                boIsHasItem = true;
            }
            if (boIsHasItem)
            {
                PlayObject.SysMsg("您身上的的装备修复成功了...", MsgColor.Blue, MsgType.Hint);
            }
        }

        private void ActionOfGroupMoveMap(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            PlayObject PlayObjectEx;
            bool boFlag = false;
            if (PlayObject.GroupOwner != null)
            {
                var Envir = M2Share.MapMgr.FindMap(QuestActionInfo.sParam1);
                if (Envir != null)
                {
                    if (Envir.CanWalk(QuestActionInfo.nParam2, QuestActionInfo.nParam3, true))
                    {
                        for (var i = 0; i < PlayObject.GroupMembers.Count; i++)
                        {
                            PlayObjectEx = PlayObject.GroupMembers[i];
                            PlayObjectEx.SpaceMove(QuestActionInfo.sParam1, (short)QuestActionInfo.nParam2, (short)QuestActionInfo.nParam3, 0);
                        }
                        boFlag = true;
                    }
                }
            }
            if (!boFlag)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSC_GROUPMOVEMAP);
            }
        }

        private void ActionOfUpgradeItems(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            int nAddPoint;
            var nWhere = HUtil32.StrToInt(QuestActionInfo.sParam1, -1);
            var nRate = HUtil32.StrToInt(QuestActionInfo.sParam2, -1);
            var nPoint = HUtil32.StrToInt(QuestActionInfo.sParam3, -1);
            if (nWhere < 0 || nWhere > PlayObject.UseItems.Length || nRate < 0 || nPoint < 0 || nPoint > 255)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSC_UPGRADEITEMS);
                return;
            }
            var UserItem = PlayObject.UseItems[nWhere];
            var StdItem = M2Share.WorldEngine.GetStdItem(UserItem.Index);
            if (UserItem.Index <= 0 || StdItem == null)
            {
                PlayObject.SysMsg("你身上没有戴指定物品!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            nRate = M2Share.RandomNumber.Random(nRate);
            nPoint = M2Share.RandomNumber.Random(nPoint);
            var nValType = M2Share.RandomNumber.Random(14);
            if (nRate != 0)
            {
                PlayObject.SysMsg("装备升级失败!!!", MsgColor.Red, MsgType.Hint);
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
                if (UserItem.Desc[nValType] + nAddPoint > byte.MaxValue)
                {
                    nAddPoint = byte.MaxValue - UserItem.Desc[nValType];
                }
                UserItem.Desc[nValType] = (byte)(UserItem.Desc[nValType] + nAddPoint);
            }
            PlayObject.SendUpdateItem(UserItem);
            PlayObject.SysMsg("装备升级成功", MsgColor.Green, MsgType.Hint);
            PlayObject.SysMsg(StdItem.Name + ": " + UserItem.Dura + '/' + UserItem.DuraMax + '/' + UserItem.Desc[0] + '/' + UserItem.Desc[1] + '/' + UserItem.Desc[2] + '/' + UserItem.Desc[3] + '/' + UserItem.Desc[4] + '/' + UserItem.Desc[5] + '/' + UserItem.Desc[6] + '/' + UserItem.Desc[7] + '/' + UserItem.Desc[8] + '/' + UserItem.Desc[9] + '/' + UserItem.Desc[ItemAttr.WeaponUpgrade] + '/' + UserItem.Desc[11] + '/' + UserItem.Desc[12] + '/' + UserItem.Desc[13], MsgColor.Blue, MsgType.Hint);
        }

        private void ActionOfUpgradeItemsEx(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            int nAddPoint;
            var nWhere = HUtil32.StrToInt(QuestActionInfo.sParam1, -1);
            var nValType = HUtil32.StrToInt(QuestActionInfo.sParam2, -1);
            var nRate = HUtil32.StrToInt(QuestActionInfo.sParam3, -1);
            var nPoint = HUtil32.StrToInt(QuestActionInfo.sParam4, -1);
            var nUpgradeItemStatus = HUtil32.StrToInt(QuestActionInfo.sParam5, -1);
            if (nValType < 0 || nValType > 14 || nWhere < 0 || nWhere > PlayObject.UseItems.Length || nRate < 0 || nPoint < 0 || nPoint > 255)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSC_UPGRADEITEMSEX);
                return;
            }
            var UserItem = PlayObject.UseItems[nWhere];
            var StdItem = M2Share.WorldEngine.GetStdItem(UserItem.Index);
            if (UserItem.Index <= 0 || StdItem == null)
            {
                PlayObject.SysMsg("你身上没有戴指定物品!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            var nRatePoint = M2Share.RandomNumber.Random(nRate * 10);
            nPoint = HUtil32._MAX(1, M2Share.RandomNumber.Random(nPoint));
            if (!(nRatePoint >= 0 && nRatePoint <= 10))
            {
                switch (nUpgradeItemStatus)
                {
                    case 0:
                        PlayObject.SysMsg("装备升级未成功!!!", MsgColor.Red, MsgType.Hint);
                        break;
                    case 1:
                        PlayObject.SendDelItems(UserItem);
                        UserItem.Index = 0;
                        PlayObject.SysMsg("装备破碎!!!", MsgColor.Red, MsgType.Hint);
                        break;
                    case 2:
                        PlayObject.SysMsg("装备升级失败，装备属性恢复默认!!!", MsgColor.Red, MsgType.Hint);
                        if (nValType != 14)
                        {
                            UserItem.Desc[nValType] = 0;
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
                if (UserItem.Desc[nValType] + nAddPoint > byte.MaxValue)
                {
                    nAddPoint = byte.MaxValue - UserItem.Desc[nValType];
                }
                UserItem.Desc[nValType] = (byte)(UserItem.Desc[nValType] + nAddPoint);
            }
            PlayObject.SendUpdateItem(UserItem);
            PlayObject.SysMsg("装备升级成功", MsgColor.Green, MsgType.Hint);
            PlayObject.SysMsg(StdItem.Name + ": " + UserItem.Dura + '/' + UserItem.DuraMax + '-' + UserItem.Desc[0] + '/' + UserItem.Desc[1] + '/' + UserItem.Desc[2] + '/' + UserItem.Desc[3] + '/' + UserItem.Desc[4] + '/' + UserItem.Desc[5] + '/' + UserItem.Desc[6] + '/' + UserItem.Desc[7] + '/' + UserItem.Desc[8] + '/' + UserItem.Desc[9] + '/' + UserItem.Desc[ItemAttr.WeaponUpgrade] + '/' + UserItem.Desc[11] + '/' + UserItem.Desc[12] + '/' + UserItem.Desc[13], MsgColor.Blue, MsgType.Hint);
        }

        /// <summary>
        /// 声明变量
        /// VAR 数据类型(Integer String) 类型(HUMAN GUILD GLOBAL) 变量值
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="QuestActionInfo"></param>
        private void ActionOfVar(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            string sName = string.Empty;
            TDynamicVar DynamicVar;
            bool boFoundVar;
            Dictionary<string, TDynamicVar> DynamicVarList;
            const string sVarFound = "变量{0}已存在，变量类型:{1}";
            const string sVarTypeError = "变量类型错误，错误类型:{0} 当前支持类型(HUMAN、GUILD、GLOBAL)";
            var sType = QuestActionInfo.sParam2;
            var sVarName = QuestActionInfo.sParam3;
            var sVarValue = QuestActionInfo.sParam4;
            var nVarValue = HUtil32.StrToInt(QuestActionInfo.sParam4, 0);
            var VarType = SystemModule.Data.VarType.None;
            if (string.Compare(QuestActionInfo.sParam1, "Integer", StringComparison.OrdinalIgnoreCase) == 0)
            {
                VarType = VarType.Integer;
            }
            if (string.Compare(QuestActionInfo.sParam1, "String", StringComparison.OrdinalIgnoreCase) == 0)
            {
                VarType = VarType.String;
            }
            if (sType == "" || sVarName == "" || VarType == VarType.None)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSC_VAR);
                return;
            }
            if (string.Compare(QuestActionInfo.sParam1, "Integer", StringComparison.OrdinalIgnoreCase) == 0)
            {
                VarType = VarType.Integer;
            }
            if (string.Compare(QuestActionInfo.sParam1, "String", StringComparison.OrdinalIgnoreCase) == 0)
            {
                VarType = VarType.String;
            }
            if (sType == "" || sVarName == "" || VarType == VarType.None)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSC_VAR);
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
                ScriptActionError(PlayObject, Format(sVarTypeError, sType), QuestActionInfo, ScriptConst.sSC_VAR);
                return;
            }
            if (DynamicVarList.ContainsKey(sVarName))
            {
                boFoundVar = true;
            }
            if (!boFoundVar)
            {
                DynamicVarList.Add(sVarName, DynamicVar);
            }
            else
            {
                ScriptActionError(PlayObject, Format(sVarFound, sVarName, sType), QuestActionInfo, ScriptConst.sSC_VAR);
            }
        }

        /// <summary>
        /// 读取变量值
        /// LOADVAR 变量类型 变量名 文件名
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="QuestActionInfo"></param>
        private void ActionOfLoadVar(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            string sName = string.Empty;
            TDynamicVar DynamicVar = null;
            Dictionary<string, TDynamicVar> DynamicVarList;
            const string sVarFound = "变量{0}不存在，变量类型:{1}";
            const string sVarTypeError = "变量类型错误，错误类型:{0} 当前支持类型(HUMAN、GUILD、GLOBAL)";
            var sType = QuestActionInfo.sParam1;
            var sVarName = QuestActionInfo.sParam2;
            var sFileName = GetLineVariableText(PlayObject, QuestActionInfo.sParam3);
            if (sFileName[0] == '\\')
            {
                sFileName = sFileName.Substring(1, sFileName.Length - 1);
            }
            if (sFileName[1] == '\\')
            {
                sFileName = sFileName.Substring(2, sFileName.Length - 2);
            }
            if (sFileName[2] == '\\')
            {
                sFileName = sFileName.Substring(3, sFileName.Length - 3);
            }
            sFileName = Path.Combine(M2Share.BasePath, M2Share.Config.EnvirDir, sFileName);
            if (sType == "" || sVarName == "" || !File.Exists(sFileName))
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSC_LOADVAR);
                return;
            }
            bool boFoundVar = false;
            DynamicVarList = GetDynamicVarList(PlayObject, sType, ref sName);
            if (DynamicVarList == null)
            {
                Dispose(DynamicVar);
                ScriptActionError(PlayObject, Format(sVarTypeError, sType), QuestActionInfo, ScriptConst.sSC_VAR);
                return;
            }
            if (DynamicVarList.TryGetValue(sVarName, out DynamicVar))
            {
                //IniFile = new ConfFile(sFileName);
                //IniFile.Load();
                /*switch (DynamicVar.VarType)
                {
                    case TVarType.Integer:
                        DynamicVar.nInternet = IniFile.ReadInteger(sName, DynamicVar.sName, 0);
                        break;
                    case TVarType.String:
                        DynamicVar.sString = IniFile.ReadString(sName, DynamicVar.sName, "");
                        break;
                }*/
                boFoundVar = true;
            }
            else
            {
                ConfFile IniFile = new ConfFile(sFileName);
                IniFile.Load();
                var str = IniFile.ReadString(sName, sVarName, "");
                if (!string.IsNullOrEmpty(str))
                {
                    if (!DynamicVarList.ContainsKey(sVarName))
                    {
                        DynamicVarList.Add(sVarName, new TDynamicVar()
                        {
                            sName = sVarName,
                            sString = str,
                            VarType = VarType.String
                        });
                    }
                    boFoundVar = true;
                }
            }
            if (!boFoundVar)
            {
                ScriptActionError(PlayObject, Format(sVarFound, sVarName, sType), QuestActionInfo, ScriptConst.sSC_LOADVAR);
            }
        }

        private void ActionOfClearNeedItems(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            ClientUserItem UserItem;
            var nNeed = HUtil32.StrToInt(QuestActionInfo.sParam1, -1);
            if (nNeed < 0)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSC_CLEARNEEDITEMS);
                return;
            }
            StdItem StdItem;
            for (var i = PlayObject.ItemList.Count - 1; i >= 0; i--)
            {
                UserItem = PlayObject.ItemList[i];
                StdItem = M2Share.WorldEngine.GetStdItem(UserItem.Index);
                if (StdItem != null && StdItem.Need == nNeed)
                {
                    PlayObject.SendDelItems(UserItem);
                    Dispose(UserItem);
                    PlayObject.ItemList.RemoveAt(i);
                }
            }
            for (var i = PlayObject.StorageItemList.Count - 1; i >= 0; i--)
            {
                UserItem = PlayObject.StorageItemList[i];
                StdItem = M2Share.WorldEngine.GetStdItem(UserItem.Index);
                if (StdItem != null && StdItem.Need == nNeed)
                {
                    Dispose(UserItem);
                    PlayObject.StorageItemList.RemoveAt(i);
                }
            }
        }

        private void ActionOfClearMakeItems(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            ClientUserItem UserItem;
            string sItemName = QuestActionInfo.sParam1;
            var nMakeIndex = QuestActionInfo.nParam2;
            var boMatchName = QuestActionInfo.sParam3 == "1";
            if (string.IsNullOrEmpty(sItemName) || nMakeIndex <= 0)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSC_CLEARMAKEITEMS);
                return;
            }
            StdItem StdItem;
            for (var i = PlayObject.ItemList.Count - 1; i >= 0; i--)
            {
                UserItem = PlayObject.ItemList[i];
                if (UserItem.MakeIndex != nMakeIndex)
                {
                    continue;
                }
                StdItem = M2Share.WorldEngine.GetStdItem(UserItem.Index);
                if (!boMatchName || StdItem != null && String.Compare(StdItem.Name, sItemName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    PlayObject.SendDelItems(UserItem);
                    Dispose(UserItem);
                    PlayObject.ItemList.RemoveAt(i);
                }
            }
            for (var i = PlayObject.StorageItemList.Count - 1; i >= 0; i--)
            {
                UserItem = PlayObject.ItemList[i];
                if (UserItem.MakeIndex != nMakeIndex)
                {
                    continue;
                }
                StdItem = M2Share.WorldEngine.GetStdItem(UserItem.Index);
                if (!boMatchName || StdItem != null && String.Compare(StdItem.Name, sItemName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    Dispose(UserItem);
                    PlayObject.StorageItemList.RemoveAt(i);
                }
            }
            for (var i = 0; i < PlayObject.UseItems.Length; i++)
            {
                UserItem = PlayObject.UseItems[i];
                if (UserItem.MakeIndex != nMakeIndex)
                {
                    continue;
                }
                StdItem = M2Share.WorldEngine.GetStdItem(UserItem.Index);
                if (!boMatchName || StdItem != null && String.Compare(StdItem.Name, sItemName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    UserItem.Index = 0;
                }
            }
        }

        private void ActionOfUnMaster(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            string sMsg;
            if (PlayObject.m_sMasterName == "")
            {
                GotoLable(PlayObject, "@ExeMasterFail", false);
                return;
            }
            var PoseHuman = (PlayObject)PlayObject.GetPoseCreate();
            if (PoseHuman == null)
            {
                GotoLable(PlayObject, "@UnMasterCheckDir", false);
            }
            if (PoseHuman != null)
            {
                if (QuestActionInfo.sParam1 == "")
                {
                    if (PoseHuman.Race != ActorRace.Play)
                    {
                        GotoLable(PlayObject, "@UnMasterTypeErr", false);
                        return;
                    }
                    if (PoseHuman.GetPoseCreate() == PlayObject)
                    {
                        if (PlayObject.m_sMasterName == PoseHuman.ChrName)
                        {
                            if (PlayObject.m_boMaster)
                            {
                                GotoLable(PlayObject, "@UnIsMaster", false);
                                return;
                            }
                            if (PlayObject.m_sMasterName != PoseHuman.ChrName)
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
            if (String.Compare(QuestActionInfo.sParam1, "REQUESTUNMASTER", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (QuestActionInfo.sParam2 == "")
                {
                    if (PoseHuman != null)
                    {
                        PlayObject.m_boStartUnMaster = true;
                        if (PlayObject.m_boStartUnMaster && PoseHuman.m_boStartUnMaster)
                        {
                            sMsg = string.Format(M2Share.g_sNPCSayUnMasterOKMsg, ChrName, PlayObject.ChrName, PoseHuman.ChrName);
                            M2Share.WorldEngine.SendBroadCastMsg(sMsg, MsgType.Say);
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
                    if (String.Compare(QuestActionInfo.sParam2, "FORCE", StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        sMsg = string.Format(M2Share.g_sNPCSayForceUnMasterMsg, ChrName, PlayObject.ChrName, PlayObject.m_sMasterName);
                        M2Share.WorldEngine.SendBroadCastMsg(sMsg, MsgType.Say);
                        PoseHuman = M2Share.WorldEngine.GetPlayObject(PlayObject.m_sMasterName);
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

        private void ActionOfSetMapMode(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            string sMapName = QuestActionInfo.sParam1;
            string sMapMode = QuestActionInfo.sParam2;
            string sParam1 = QuestActionInfo.sParam3;
            string sParam2 = QuestActionInfo.sParam4;
            Envirnoment Envir = M2Share.MapMgr.FindMap(sMapName);
            if (Envir == null || sMapMode == "")
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSC_SETMAPMODE);
                return;
            }
            if (String.Compare(sMapMode, "SAFE", StringComparison.OrdinalIgnoreCase) == 0)
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
            else if (String.Compare(sMapMode, "DARK", StringComparison.OrdinalIgnoreCase) == 0)
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
            else if (String.Compare(sMapMode, "FIGHT", StringComparison.OrdinalIgnoreCase) == 0)
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
            else if (String.Compare(sMapMode, "FIGHT3", StringComparison.OrdinalIgnoreCase) == 0)
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
            else if (String.Compare(sMapMode, "DAY", StringComparison.OrdinalIgnoreCase) == 0)
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
            else if (String.Compare(sMapMode, "QUIZ", StringComparison.OrdinalIgnoreCase) == 0)
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
            else if (String.Compare(sMapMode, "NORECONNECT", StringComparison.OrdinalIgnoreCase) == 0)
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
            else if (String.Compare(sMapMode, "MUSIC", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (sParam1 != "")
                {
                    Envir.Flag.boMUSIC = true;
                    Envir.Flag.nMUSICID = HUtil32.StrToInt(sParam1, -1);
                }
                else
                {
                    Envir.Flag.boMUSIC = false;
                }
            }
            else if (String.Compare(sMapMode, "EXPRATE", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (sParam1 != "")
                {
                    Envir.Flag.boEXPRATE = true;
                    Envir.Flag.nEXPRATE = HUtil32.StrToInt(sParam1, -1);
                }
                else
                {
                    Envir.Flag.boEXPRATE = false;
                }
            }
            else if (String.Compare(sMapMode, "PKWINLEVEL", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (sParam1 != "")
                {
                    Envir.Flag.boPKWINLEVEL = true;
                    Envir.Flag.nPKWINLEVEL = HUtil32.StrToInt(sParam1, -1);
                }
                else
                {
                    Envir.Flag.boPKWINLEVEL = false;
                }
            }
            else if (String.Compare(sMapMode, "PKWINEXP", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (sParam1 != "")
                {
                    Envir.Flag.boPKWINEXP = true;
                    Envir.Flag.nPKWINEXP = HUtil32.StrToInt(sParam1, -1);
                }
                else
                {
                    Envir.Flag.boPKWINEXP = false;
                }
            }
            else if (String.Compare(sMapMode, "PKLOSTLEVEL", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (sParam1 != "")
                {
                    Envir.Flag.boPKLOSTLEVEL = true;
                    Envir.Flag.nPKLOSTLEVEL = HUtil32.StrToInt(sParam1, -1);
                }
                else
                {
                    Envir.Flag.boPKLOSTLEVEL = false;
                }
            }
            else if (String.Compare(sMapMode, "PKLOSTEXP", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (sParam1 != "")
                {
                    Envir.Flag.boPKLOSTEXP = true;
                    Envir.Flag.nPKLOSTEXP = HUtil32.StrToInt(sParam1, -1);
                }
                else
                {
                    Envir.Flag.boPKLOSTEXP = false;
                }
            }
            else if (String.Compare(sMapMode, "DECHP", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (sParam1 != "" && sParam2 != "")
                {
                    Envir.Flag.boDECHP = true;
                    Envir.Flag.nDECHPTIME = HUtil32.StrToInt(sParam1, -1);
                    Envir.Flag.nDECHPPOINT = HUtil32.StrToInt(sParam2, -1);
                }
                else
                {
                    Envir.Flag.boDECHP = false;
                }
            }
            else if (String.Compare(sMapMode, "DECGAMEGOLD", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (sParam1 != "" && sParam2 != "")
                {
                    Envir.Flag.boDECGAMEGOLD = true;
                    Envir.Flag.nDECGAMEGOLDTIME = HUtil32.StrToInt(sParam1, -1);
                    Envir.Flag.nDECGAMEGOLD = HUtil32.StrToInt(sParam2, -1);
                }
                else
                {
                    Envir.Flag.boDECGAMEGOLD = false;
                }
            }
            else if (String.Compare(sMapMode, "RUNHUMAN", StringComparison.OrdinalIgnoreCase) == 0)
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
            else if (String.Compare(sMapMode, "RUNMON", StringComparison.OrdinalIgnoreCase) == 0)
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
            else if (String.Compare(sMapMode, "NEEDHOLE", StringComparison.OrdinalIgnoreCase) == 0)
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
            else if (String.Compare(sMapMode, "NORECALL", StringComparison.OrdinalIgnoreCase) == 0)
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
            else if (String.Compare(sMapMode, "NOGUILDRECALL", StringComparison.OrdinalIgnoreCase) == 0)
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
            else if (String.Compare(sMapMode, "NODEARRECALL", StringComparison.OrdinalIgnoreCase) == 0)
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
            else if (String.Compare(sMapMode, "NOMASTERRECALL", StringComparison.OrdinalIgnoreCase) == 0)
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
            else if (String.Compare(sMapMode, "NORANDOMMOVE", StringComparison.OrdinalIgnoreCase) == 0)
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
            else if (String.Compare(sMapMode, "NODRUG", StringComparison.OrdinalIgnoreCase) == 0)
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
            else if (String.Compare(sMapMode, "MINE", StringComparison.OrdinalIgnoreCase) == 0)
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
            else if (String.Compare(sMapMode, "MINE2", StringComparison.OrdinalIgnoreCase) == 0)
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
            else if (String.Compare(sMapMode, "NOTHROWITEM", StringComparison.OrdinalIgnoreCase) == 0)
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
            else if (String.Compare(sMapMode, "NODROPITEM", StringComparison.OrdinalIgnoreCase) == 0)
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
            else if (String.Compare(sMapMode, "NOPOSITIONMOVE", StringComparison.OrdinalIgnoreCase) == 0)
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
            else if (String.Compare(sMapMode, "NOHORSE", StringComparison.OrdinalIgnoreCase) == 0)
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
            else if (String.Compare(sMapMode, "NOCHAT", StringComparison.OrdinalIgnoreCase) == 0)
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

        private void ActionOfSetMemberLevel(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            int nLevel = HUtil32.StrToInt(QuestActionInfo.sParam2, -1);
            if (nLevel < 0)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSC_SETMEMBERLEVEL);
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
            if (M2Share.Config.ShowScriptActionMsg)
            {
                PlayObject.SysMsg(Format(M2Share.g_sChangeMemberLevelMsg, new[] { PlayObject.m_nMemberLevel }), MsgColor.Green, MsgType.Hint);
            }
        }

        private void ActionOfSetMemberType(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            int nType = HUtil32.StrToInt(QuestActionInfo.sParam2, -1);
            if (nType < 0)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSC_SETMEMBERTYPE);
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
            if (M2Share.Config.ShowScriptActionMsg)
            {
                PlayObject.SysMsg(Format(M2Share.g_sChangeMemberTypeMsg, new[] { PlayObject.m_nMemberType }), MsgColor.Green, MsgType.Hint);
            }
        }

        private void ActionOfGiveItem(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            ClientUserItem UserItem;
            var sItemName = QuestActionInfo.sParam1;
            var nItemCount = QuestActionInfo.nParam2;
            if (string.IsNullOrEmpty(sItemName) || nItemCount <= 0)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSC_GIVE);
                return;
            }
            if (String.Compare(sItemName, Grobal2.sSTRING_GOLDNAME, StringComparison.OrdinalIgnoreCase) == 0)
            {
                PlayObject.IncGold(nItemCount);
                PlayObject.GoldChanged();
                if (M2Share.GameLogGold)
                {
                    M2Share.EventSource.AddEventLog(9, PlayObject.MapName + "\t" + PlayObject.CurrX + "\t" + PlayObject.CurrY + "\t" + PlayObject.ChrName + "\t" + Grobal2.sSTRING_GOLDNAME + "\t" + nItemCount + "\t" + '1' + "\t" + ChrName);
                }
                return;
            }
            if (M2Share.WorldEngine.GetStdItemIdx(sItemName) > 0)
            {
                if (!(nItemCount >= 1 && nItemCount <= 50))
                {
                    nItemCount = 1;
                }
                for (var i = 0; i < nItemCount; i++)
                {
                    StdItem StdItem;
                    // nItemCount 为0时出死循环
                    if (PlayObject.IsEnoughBag())
                    {
                        UserItem = new ClientUserItem();
                        if (M2Share.WorldEngine.CopyToUserItemFromName(sItemName, ref UserItem))
                        {
                            PlayObject.ItemList.Add(UserItem);
                            PlayObject.SendAddItem(UserItem);
                            StdItem = M2Share.WorldEngine.GetStdItem(UserItem.Index);
                            if (StdItem.NeedIdentify == 1)
                            {
                                M2Share.EventSource.AddEventLog(9, PlayObject.MapName + "\t" + PlayObject.CurrX + "\t" + PlayObject.CurrY + "\t" + PlayObject.ChrName + "\t" + sItemName + "\t" + UserItem.MakeIndex + "\t" + '1' + "\t" + ChrName);
                            }
                        }
                        else
                        {
                            Dispose(UserItem);
                        }
                    }
                    else
                    {
                        UserItem = new ClientUserItem();
                        if (M2Share.WorldEngine.CopyToUserItemFromName(sItemName, ref UserItem))
                        {
                            StdItem = M2Share.WorldEngine.GetStdItem(UserItem.Index);
                            if (StdItem.NeedIdentify == 1)
                            {
                                M2Share.EventSource.AddEventLog(9, PlayObject.MapName + "\t" + PlayObject.CurrX + "\t" + PlayObject.CurrY + "\t" + PlayObject.ChrName + "\t" + sItemName + "\t" + UserItem.MakeIndex + "\t" + '1' + "\t" + ChrName);
                            }
                            PlayObject.DropItemDown(UserItem, 3, false, PlayObject, null);
                        }
                        Dispose(UserItem);
                    }
                }
            }
        }

        private void ActionOfGmExecute(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            string sParam1 = QuestActionInfo.sParam1;
            string sParam2 = QuestActionInfo.sParam2;
            string sParam3 = QuestActionInfo.sParam3;
            string sParam4 = QuestActionInfo.sParam4;
            string sParam5 = QuestActionInfo.sParam5;
            string sParam6 = QuestActionInfo.sParam6;
            if (String.Compare(sParam2, "Self", StringComparison.OrdinalIgnoreCase) == 0)
            {
                sParam2 = PlayObject.ChrName;
            }
            string sData = Format("@{0} {1} {2} {3} {4} {5}", sParam1, sParam2, sParam3, sParam4, sParam5, sParam6);
            byte btOldPermission = PlayObject.Permission;
            try
            {
                PlayObject.Permission = 10;
                PlayObject.ProcessUserLineMsg(sData);
            }
            finally
            {
                PlayObject.Permission = btOldPermission;
            }
        }

        private void ActionOfGuildAuraePoint(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            int nAuraePoint = HUtil32.StrToInt(QuestActionInfo.sParam2, -1);
            if (nAuraePoint < 0)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSC_AURAEPOINT);
                return;
            }
            if (PlayObject.MyGuild == null)
            {
                PlayObject.SysMsg(M2Share.g_sScriptGuildAuraePointNoGuild, MsgColor.Red, MsgType.Hint);
                return;
            }
            var Guild = PlayObject.MyGuild;
            var cMethod = QuestActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    Guild.Aurae = nAuraePoint;
                    break;
                case '-':
                    if (Guild.Aurae >= nAuraePoint)
                    {
                        Guild.Aurae = Guild.Aurae - nAuraePoint;
                    }
                    else
                    {
                        Guild.Aurae = 0;
                    }
                    break;
                case '+':
                    if (int.MaxValue - Guild.Aurae >= nAuraePoint)
                    {
                        Guild.Aurae = Guild.Aurae + nAuraePoint;
                    }
                    else
                    {
                        Guild.Aurae = int.MaxValue;
                    }
                    break;
            }
            if (M2Share.Config.ShowScriptActionMsg)
            {
                PlayObject.SysMsg(Format(M2Share.g_sScriptGuildAuraePointMsg, new[] { Guild.Aurae }), MsgColor.Green, MsgType.Hint);
            }
        }

        private void ActionOfGuildBuildPoint(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            var nBuildPoint = HUtil32.StrToInt(QuestActionInfo.sParam2, -1);
            if (nBuildPoint < 0)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSC_BUILDPOINT);
                return;
            }
            if (PlayObject.MyGuild == null)
            {
                PlayObject.SysMsg(M2Share.g_sScriptGuildBuildPointNoGuild, MsgColor.Red, MsgType.Hint);
                return;
            }
            var Guild = PlayObject.MyGuild;
            var cMethod = QuestActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    Guild.BuildPoint = nBuildPoint;
                    break;
                case '-':
                    if (Guild.BuildPoint >= nBuildPoint)
                    {
                        Guild.BuildPoint = Guild.BuildPoint - nBuildPoint;
                    }
                    else
                    {
                        Guild.BuildPoint = 0;
                    }
                    break;
                case '+':
                    if (int.MaxValue - Guild.BuildPoint >= nBuildPoint)
                    {
                        Guild.BuildPoint = Guild.BuildPoint + nBuildPoint;
                    }
                    else
                    {
                        Guild.BuildPoint = int.MaxValue;
                    }
                    break;
            }
            if (M2Share.Config.ShowScriptActionMsg)
            {
                PlayObject.SysMsg(Format(M2Share.g_sScriptGuildBuildPointMsg, Guild.BuildPoint), MsgColor.Green, MsgType.Hint);
            }
        }

        private void ActionOfGuildChiefItemCount(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            var nItemCount = HUtil32.StrToInt(QuestActionInfo.sParam2, -1);
            if (nItemCount < 0)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSC_GUILDCHIEFITEMCOUNT);
                return;
            }
            if (PlayObject.MyGuild == null)
            {
                PlayObject.SysMsg(M2Share.g_sScriptGuildFlourishPointNoGuild, MsgColor.Red, MsgType.Hint);
                return;
            }
            var Guild = PlayObject.MyGuild;
            var cMethod = QuestActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    Guild.ChiefItemCount = nItemCount;
                    break;
                case '-':
                    if (Guild.ChiefItemCount >= nItemCount)
                    {
                        Guild.ChiefItemCount = Guild.ChiefItemCount - nItemCount;
                    }
                    else
                    {
                        Guild.ChiefItemCount = 0;
                    }
                    break;
                case '+':
                    if (int.MaxValue - Guild.ChiefItemCount >= nItemCount)
                    {
                        Guild.ChiefItemCount = Guild.ChiefItemCount + nItemCount;
                    }
                    else
                    {
                        Guild.ChiefItemCount = int.MaxValue;
                    }
                    break;
            }
            if (M2Share.Config.ShowScriptActionMsg)
            {
                PlayObject.SysMsg(Format(M2Share.g_sScriptChiefItemCountMsg, Guild.ChiefItemCount), MsgColor.Green, MsgType.Hint);
            }
        }

        private void ActionOfGuildFlourishPoint(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            var nFlourishPoint = HUtil32.StrToInt(QuestActionInfo.sParam2, -1);
            if (nFlourishPoint < 0)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSC_FLOURISHPOINT);
                return;
            }
            if (PlayObject.MyGuild == null)
            {
                PlayObject.SysMsg(M2Share.g_sScriptGuildFlourishPointNoGuild, MsgColor.Red, MsgType.Hint);
                return;
            }
            var Guild = PlayObject.MyGuild;
            var cMethod = QuestActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    Guild.Flourishing = nFlourishPoint;
                    break;
                case '-':
                    if (Guild.Flourishing >= nFlourishPoint)
                    {
                        Guild.Flourishing = Guild.Flourishing - nFlourishPoint;
                    }
                    else
                    {
                        Guild.Flourishing = 0;
                    }
                    break;
                case '+':
                    if (int.MaxValue - Guild.Flourishing >= nFlourishPoint)
                    {
                        Guild.Flourishing = Guild.Flourishing + nFlourishPoint;
                    }
                    else
                    {
                        Guild.Flourishing = int.MaxValue;
                    }
                    break;
            }
            if (M2Share.Config.ShowScriptActionMsg)
            {
                PlayObject.SysMsg(Format(M2Share.g_sScriptGuildFlourishPointMsg, Guild.Flourishing), MsgColor.Green, MsgType.Hint);
            }
        }

        private void ActionOfGuildstabilityPoint(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            var nStabilityPoint = HUtil32.StrToInt(QuestActionInfo.sParam2, -1);
            if (nStabilityPoint < 0)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSC_STABILITYPOINT);
                return;
            }
            if (PlayObject.MyGuild == null)
            {
                PlayObject.SysMsg(M2Share.g_sScriptGuildStabilityPointNoGuild, MsgColor.Red, MsgType.Hint);
                return;
            }
            var Guild = PlayObject.MyGuild;
            var cMethod = QuestActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    Guild.Stability = nStabilityPoint;
                    break;
                case '-':
                    if (Guild.Stability >= nStabilityPoint)
                    {
                        Guild.Stability = Guild.Stability - nStabilityPoint;
                    }
                    else
                    {
                        Guild.Stability = 0;
                    }
                    break;
                case '+':
                    if (int.MaxValue - Guild.Stability >= nStabilityPoint)
                    {
                        Guild.Stability = Guild.Stability + nStabilityPoint;
                    }
                    else
                    {
                        Guild.Stability = int.MaxValue;
                    }
                    break;
            }
            if (M2Share.Config.ShowScriptActionMsg)
            {
                PlayObject.SysMsg(Format(M2Share.g_sScriptGuildStabilityPointMsg, Guild.Stability), MsgColor.Green, MsgType.Hint);
            }
        }

        private void ActionOfHumanHP(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            var nHP = HUtil32.StrToInt(QuestActionInfo.sParam2, -1);
            if (nHP < 0)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSC_HUMANHP);
                return;
            }
            var cMethod = QuestActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    PlayObject.WAbil.HP = (ushort)nHP;
                    break;
                case '-':
                    if (PlayObject.WAbil.HP >= nHP)
                    {
                        PlayObject.WAbil.HP -= (ushort)nHP;
                    }
                    else
                    {
                        PlayObject.WAbil.HP = 0;
                    }
                    break;
                case '+':
                    PlayObject.WAbil.HP += (ushort)nHP;
                    if (PlayObject.WAbil.HP > PlayObject.WAbil.MaxHP)
                    {
                        PlayObject.WAbil.HP = PlayObject.WAbil.MaxHP;
                    }
                    break;
            }
            if (M2Share.Config.ShowScriptActionMsg)
            {
                PlayObject.SysMsg(Format(M2Share.g_sScriptChangeHumanHPMsg, PlayObject.WAbil.MaxHP), MsgColor.Green, MsgType.Hint);
            }
        }

        private void ActionOfHumanMP(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            var nMP = HUtil32.StrToInt(QuestActionInfo.sParam2, -1);
            if (nMP < 0)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSC_HUMANMP);
                return;
            }
            var cMethod = QuestActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    PlayObject.WAbil.MP = (ushort)nMP;
                    break;
                case '-':
                    if (PlayObject.WAbil.MP >= nMP)
                    {
                        PlayObject.WAbil.MP -= (ushort)nMP;
                    }
                    else
                    {
                        PlayObject.WAbil.MP = 0;
                    }
                    break;
                case '+':
                    PlayObject.WAbil.MP += (ushort)nMP;
                    if (PlayObject.WAbil.MP > PlayObject.WAbil.MaxMP)
                    {
                        PlayObject.WAbil.MP = PlayObject.WAbil.MaxMP;
                    }
                    break;
            }
            if (M2Share.Config.ShowScriptActionMsg)
            {
                PlayObject.SysMsg(Format(M2Share.g_sScriptChangeHumanMPMsg, new[] { PlayObject.WAbil.MaxMP }), MsgColor.Green, MsgType.Hint);
            }
        }

        private void ActionOfKick(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            PlayObject.m_boKickFlag = true;
        }

        private void ActionOfKill(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            var nMode = HUtil32.StrToInt(QuestActionInfo.sParam1, -1);
            if (nMode >= 0 && nMode <= 3)
            {
                switch (nMode)
                {
                    case 1:
                        PlayObject.NoItem = true;
                        PlayObject.Die();
                        break;
                    case 2:
                        PlayObject.SetLastHiter(this);
                        PlayObject.Die();
                        break;
                    case 3:
                        PlayObject.NoItem = true;
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
                ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSC_KILL);
            }
        }

        private void ActionOfBonusPoint(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            int nBonusPoint = HUtil32.StrToInt(QuestActionInfo.sParam2, -1);
            if (nBonusPoint < 0 || nBonusPoint > 10000)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSC_BONUSPOINT);
                return;
            }
            char cMethod = QuestActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    PlayObject.HasLevelUp(0);
                    PlayObject.BonusPoint = nBonusPoint;
                    PlayObject.SendMsg(PlayObject, Grobal2.RM_ADJUST_BONUS, 0, 0, 0, 0, "");
                    break;
                case '-':
                    break;
                case '+':
                    PlayObject.BonusPoint += nBonusPoint;
                    PlayObject.SendMsg(PlayObject, Grobal2.RM_ADJUST_BONUS, 0, 0, 0, 0, "");
                    break;
            }
        }

        private void ActionOfDelMarry(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            PlayObject.m_sDearName = "";
            PlayObject.RefShowName();
        }

        private void ActionOfDelMaster(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            PlayObject.m_sMasterName = "";
            PlayObject.RefShowName();
        }

        private void ActionOfRestBonusPoint(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            var nTotleUsePoint = PlayObject.BonusAbil.DC + PlayObject.BonusAbil.MC + PlayObject.BonusAbil.SC + PlayObject.BonusAbil.AC + PlayObject.BonusAbil.MAC + PlayObject.BonusAbil.HP + PlayObject.BonusAbil.MP + PlayObject.BonusAbil.Hit + PlayObject.BonusAbil.Speed + PlayObject.BonusAbil.Reserved;
            PlayObject.BonusPoint += nTotleUsePoint;
            PlayObject.SendMsg(PlayObject, Grobal2.RM_ADJUST_BONUS, 0, 0, 0, 0, "");
            PlayObject.HasLevelUp(0);
            PlayObject.SysMsg("分配点数已复位!!!", MsgColor.Red, MsgType.Hint);
        }

        private void ActionOfRestReNewLevel(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            PlayObject.m_btReLevel = 0;
            PlayObject.HasLevelUp(0);
        }


        private void ActionOfChangeNameColor(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            int nColor = QuestActionInfo.nParam1;
            if (nColor < 0 || nColor > 255)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSC_CHANGENAMECOLOR);
                return;
            }
            PlayObject.NameColor = (byte)nColor;
            PlayObject.RefNameColor();
        }

        private void ActionOfClearPassword(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            PlayObject.m_sStoragePwd = "";
            PlayObject.m_boPasswordLocked = false;
        }

        // 挂机的
        // RECALLMOB 怪物名称 等级 叛变时间 变色(0,1) 固定颜色(1 - 7)
        // 变色为0 时固定颜色才起作用
        private void ActionOfRecallmob(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            BaseObject Mon;
            if (QuestActionInfo.nParam3 <= 1)
            {
                Mon = PlayObject.MakeSlave(QuestActionInfo.sParam1, 3, HUtil32.StrToInt(QuestActionInfo.sParam2, 0), 100, 10 * 24 * 60 * 60);
            }
            else
            {
                Mon = PlayObject.MakeSlave(QuestActionInfo.sParam1, 3, HUtil32.StrToInt(QuestActionInfo.sParam2, 0), 100, QuestActionInfo.nParam3 * 60);
            }
            if (Mon != null)
            {
                if (QuestActionInfo.sParam4 != "" && QuestActionInfo.sParam4[1] == '1')
                {
                    Mon.AutoChangeColor = true;
                }
                else if (QuestActionInfo.nParam5 > 0)
                {
                    Mon.FixColor = true;
                    Mon.FixColorIdx = QuestActionInfo.nParam5 - 1;
                }
            }
        }

        private void ActionOfReNewLevel(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            int nReLevel = HUtil32.StrToInt(QuestActionInfo.sParam1, -1);
            int nLevel = HUtil32.StrToInt(QuestActionInfo.sParam2, -1);
            int nBounsuPoint = HUtil32.StrToInt(QuestActionInfo.sParam3, -1);
            if (nReLevel < 0 || nLevel < 0 || nBounsuPoint < 0)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSC_RENEWLEVEL);
                return;
            }
            if (PlayObject.m_btReLevel + nReLevel <= 255)
            {
                PlayObject.m_btReLevel += (byte)nReLevel;
                if (nLevel > 0)
                {
                    PlayObject.Abil.Level = (byte)nLevel;
                }
                if (M2Share.Config.ReNewLevelClearExp)
                {
                    PlayObject.Abil.Exp = 0;
                }
                PlayObject.BonusPoint += nBounsuPoint;
                PlayObject.SendMsg(PlayObject, Grobal2.RM_ADJUST_BONUS, 0, 0, 0, 0, "");
                PlayObject.HasLevelUp(0);
                PlayObject.RefShowName();
            }
        }

        private void ActionOfChangeGender(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            var nGender = HUtil32.StrToInt(QuestActionInfo.sParam1, -1);
            if (nGender > 1)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSC_CHANGEGENDER);
                return;
            }
            PlayObject.Gender = Enum.Parse<PlayGender>(nGender.ToString());
            PlayObject.FeatureChanged();
        }

        private void ActionOfKillSlave(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            BaseObject Slave;
            for (var i = 0; i < PlayObject.SlaveList.Count; i++)
            {
                Slave = PlayObject.SlaveList[i];
                Slave.WAbil.HP = 0;
            }
        }

        private void ActionOfKillMonExpRate(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            int nRate = HUtil32.StrToInt(QuestActionInfo.sParam1, -1);
            int nTime = HUtil32.StrToInt(QuestActionInfo.sParam2, -1);
            if (nRate < 0 || nTime < 0)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSC_KILLMONEXPRATE);
                return;
            }
            PlayObject.m_nKillMonExpRate = nRate;
            PlayObject.m_dwKillMonExpRateTime = nTime;
            if (M2Share.Config.ShowScriptActionMsg)
            {
                PlayObject.SysMsg(Format(M2Share.g_sChangeKillMonExpRateMsg, PlayObject.m_nKillMonExpRate / 100, PlayObject.m_dwKillMonExpRateTime), MsgColor.Green, MsgType.Hint);
            }
        }

        private void ActionOfMonGenEx(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            string sMapName = QuestActionInfo.sParam1;
            int nMapX = HUtil32.StrToInt(QuestActionInfo.sParam2, -1);
            int nMapY = HUtil32.StrToInt(QuestActionInfo.sParam3, -1);
            string sMonName = QuestActionInfo.sParam4;
            int nRange = QuestActionInfo.nParam5;
            int nCount = QuestActionInfo.nParam6;
            if (sMapName == "" || nMapX <= 0 || nMapY <= 0 || sMapName == "" || nRange <= 0 || nCount <= 0)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSC_MONGENEX);
                return;
            }
            for (var i = 0; i < nCount; i++)
            {
                int nRandX = M2Share.RandomNumber.Random(nRange * 2 + 1) + (nMapX - nRange);
                int nRandY = M2Share.RandomNumber.Random(nRange * 2 + 1) + (nMapY - nRange);
                if (M2Share.WorldEngine.RegenMonsterByName(sMapName, (short)nRandX, (short)nRandY, sMonName) == null)
                {
                    break;
                }
            }
        }

        private void ActionOfOpenMagicBox(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            short nX = 0;
            short nY = 0;
            string sMonName = QuestActionInfo.sParam1;
            if (sMonName == "")
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSC_OPENMAGICBOX);
                return;
            }
            PlayObject.GetFrontPosition(ref nX, ref nY);
            var Monster = M2Share.WorldEngine.RegenMonsterByName(PlayObject.Envir.MapName, nX, nY, sMonName);
            if (Monster == null)
            {
                return;
            }
            Monster.Die();
        }

        private void ActionOfPkZone(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            FireBurnEvent FireBurnEvent;
            int nRange = HUtil32.StrToInt(QuestActionInfo.sParam1, -1);
            int nType = HUtil32.StrToInt(QuestActionInfo.sParam2, -1);
            int nTime = HUtil32.StrToInt(QuestActionInfo.sParam3, -1);
            int nPoint = HUtil32.StrToInt(QuestActionInfo.sParam4, -1);
            if (nRange < 0 || nType < 0 || nTime < 0 || nPoint < 0)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSC_PKZONE);
                return;
            }
            int nMinX = CurrX - nRange;
            int nMaxX = CurrX + nRange;
            int nMinY = CurrY - nRange;
            int nMaxY = CurrY + nRange;
            for (int nX = nMinX; nX <= nMaxX; nX++)
            {
                for (int nY = nMinY; nY <= nMaxY; nY++)
                {
                    if (nX < nMaxX && nY == nMinY || nY < nMaxY && nX == nMinX || nX == nMaxX ||
                        nY == nMaxY)
                    {
                        FireBurnEvent = new FireBurnEvent(PlayObject, (short)nX, (short)nY, (byte)nType, nTime * 1000, nPoint);
                        M2Share.EventMgr.AddEvent(FireBurnEvent);
                    }
                }
            }
        }

        private void ActionOfPowerRate(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            var nRate = HUtil32.StrToInt(QuestActionInfo.sParam1, -1);
            var nTime = HUtil32.StrToInt(QuestActionInfo.sParam2, -1);
            if (nRate < 0 || nTime < 0)
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSC_POWERRATE);
                return;
            }
            PlayObject.m_nPowerRate = nRate;
            PlayObject.m_dwPowerRateTime = nTime;
            if (M2Share.Config.ShowScriptActionMsg)
            {
                PlayObject.SysMsg(Format(M2Share.g_sChangePowerRateMsg, new object[] { PlayObject.m_nPowerRate / 100, PlayObject.m_dwPowerRateTime }), MsgColor.Green, MsgType.Hint);
            }
        }

        private void ActionOfChangeMode(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            var nMode = QuestActionInfo.nParam1;
            var boOpen = HUtil32.StrToInt(QuestActionInfo.sParam2, -1) == 1;
            if (nMode >= 1 && nMode <= 3)
            {
                switch (nMode)
                {
                    case 1:
                        PlayObject.AdminMode = boOpen;
                        if (PlayObject.AdminMode)
                        {
                            PlayObject.SysMsg(M2Share.sGameMasterMode, MsgColor.Green, MsgType.Hint);
                        }
                        else
                        {
                            PlayObject.SysMsg(M2Share.sReleaseGameMasterMode, MsgColor.Green, MsgType.Hint);
                        }
                        break;
                    case 2:
                        PlayObject.SuperMan = boOpen;
                        if (PlayObject.SuperMan)
                        {
                            PlayObject.SysMsg(M2Share.sSupermanMode, MsgColor.Green, MsgType.Hint);
                        }
                        else
                        {
                            PlayObject.SysMsg(M2Share.sReleaseSupermanMode, MsgColor.Green, MsgType.Hint);
                        }
                        break;
                    case 3:
                        PlayObject.ObMode = boOpen;
                        if (PlayObject.ObMode)
                        {
                            PlayObject.SysMsg(M2Share.sObserverMode, MsgColor.Green, MsgType.Hint);
                        }
                        else
                        {
                            PlayObject.SysMsg(M2Share.g_sReleaseObserverMode, MsgColor.Green, MsgType.Hint);
                        }
                        break;
                }
            }
            else
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSC_CHANGEMODE);
            }
        }

        private void ActionOfChangePerMission(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            var nPermission = HUtil32.StrToInt(QuestActionInfo.sParam1, -1);
            if (nPermission >= 0 && nPermission <= 10)
            {
                PlayObject.Permission = (byte)nPermission;
            }
            else
            {
                ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sSC_CHANGEPERMISSION);
                return;
            }
            if (M2Share.Config.ShowScriptActionMsg)
            {
                PlayObject.SysMsg(Format(M2Share.g_sChangePermissionMsg, PlayObject.Permission), MsgColor.Green, MsgType.Hint);
            }
        }

        private void ActionOfTHROWITEM(PlayObject PlayObject, QuestActionInfo QuestActionInfo)
        {
            string sMap = string.Empty;
            string sItemName = string.Empty;
            int idura;
            int nX = 0;
            int nY = 0;
            int nRange = 0;
            int nCount = 0;
            int dX = 0;
            int dY = 0;
            Envirnoment Envir;
            MapItem MapItem;
            MapItem MapItemA;
            ClientUserItem UserItem = null;
            try
            {
                if (!GetValValue(PlayObject, QuestActionInfo.sParam1, ref sMap))
                {
                    sMap = GetLineVariableText(PlayObject, QuestActionInfo.sParam1);
                }
                if (!GetValValue(PlayObject, QuestActionInfo.sParam2, ref nX))
                {
                    nX = HUtil32.StrToInt(GetLineVariableText(PlayObject, QuestActionInfo.sParam2), -1);
                }
                if (!GetValValue(PlayObject, QuestActionInfo.sParam3, ref nY))
                {
                    nY = HUtil32.StrToInt(GetLineVariableText(PlayObject, QuestActionInfo.sParam3), -1);
                }
                if (!GetValValue(PlayObject, QuestActionInfo.sParam4, ref nRange))
                {
                    nRange = HUtil32.StrToInt(GetLineVariableText(PlayObject, QuestActionInfo.sParam4), -1);
                }
                if (!GetValValue(PlayObject, QuestActionInfo.sParam5, ref sItemName))
                {
                    sItemName = GetLineVariableText(PlayObject, QuestActionInfo.sParam5);
                }
                if (!GetValValue(PlayObject, QuestActionInfo.sParam6, ref nCount))
                {
                    nCount = HUtil32.StrToInt(GetLineVariableText(PlayObject, QuestActionInfo.sParam6), -1);
                }
                if (sMap == "" || nX < 0 || nY < 0 || nRange < 0 || string.IsNullOrEmpty(sItemName) || nCount <= 0)
                {
                    ScriptActionError(PlayObject, "", QuestActionInfo, ScriptConst.sTHROWITEM);
                    return;
                }
                Envir = M2Share.MapMgr.FindMap(sMap); // 查找地图,地图不存在则退出
                if (Envir == null)
                {
                    return;
                }
                if (nCount <= 0)
                {
                    nCount = 1;
                }
                if (string.Compare(sItemName, Grobal2.sSTRING_GOLDNAME, StringComparison.OrdinalIgnoreCase) == 0)// 金币
                {
                    if (ActionOfTHROWITEM_RobotGetDropPosition(Envir, nX, nY, nRange, ref dX, ref dY))
                    {
                        MapItem = new MapItem();
                        MapItem.Name = Grobal2.sSTRING_GOLDNAME;
                        MapItem.Count = nCount;
                        MapItem.Looks = M2Share.GetGoldShape(nCount);
                        MapItem.OfBaseObject = PlayObject.ActorId;
                        MapItem.CanPickUpTick = HUtil32.GetTickCount();
                        MapItem.DropBaseObject = PlayObject.ActorId;
                        MapItemA = (MapItem)Envir.AddToMap(dX, dY, CellType.Item, MapItem);
                        if (MapItemA != null)
                        {
                            if (MapItemA != MapItem)
                            {
                                Dispose(MapItem);
                                MapItem = MapItemA;
                            }
                            SendRefMsg(Grobal2.RM_ITEMSHOW, MapItem.Looks, MapItem.ActorId, dX, dY, MapItem.Name + "@0");
                        }
                        else
                        {
                            Dispose(MapItem);
                        }
                        return;
                    }
                }
                for (var i = 0; i < nCount; i++)
                {
                    if (ActionOfTHROWITEM_RobotGetDropPosition(Envir, nX, nY, nRange, ref dX, ref dY)) // 修正出现在一个坐标上
                    {
                        if (M2Share.WorldEngine.CopyToUserItemFromName(sItemName, ref UserItem))
                        {
                            StdItem StdItem = M2Share.WorldEngine.GetStdItem(UserItem.Index);
                            if (StdItem != null)
                            {
                                if (StdItem.StdMode == 40)
                                {
                                    idura = UserItem.Dura;
                                    idura = idura - 2000;
                                    if (idura < 0)
                                    {
                                        idura = 0;
                                    }
                                    UserItem.Dura = (ushort)idura;
                                }
                                MapItem = new MapItem();
                                MapItem.UserItem = new ClientUserItem(UserItem);
                                MapItem.Name = StdItem.Name;
                                var NameCorlr = "@" + M2Share.CustomItemMgr.GetItemAddValuePointColor(UserItem); // 取自定义物品名称
                                var sUserItemName = "";
                                if (UserItem.Desc[13] == 1)
                                {
                                    sUserItemName = M2Share.CustomItemMgr.GetCustomItemName(UserItem.MakeIndex, UserItem.Index);
                                    if (sUserItemName != "")
                                    {
                                        MapItem.Name = sUserItemName;
                                    }
                                }
                                MapItem.Looks = StdItem.Looks;
                                if (StdItem.StdMode == 45)
                                {
                                    MapItem.Looks = (ushort)M2Share.GetRandomLook(MapItem.Looks, StdItem.Shape);
                                }
                                MapItem.AniCount = StdItem.AniCount;
                                MapItem.Reserved = 0;
                                MapItem.Count = nCount;
                                MapItem.OfBaseObject = PlayObject.ActorId;
                                MapItem.CanPickUpTick = HUtil32.GetTickCount();
                                MapItem.DropBaseObject = PlayObject.ActorId;
                                // GetDropPosition(nX, nY, nRange, dx, dy);//取掉物的位置
                                MapItemA = (MapItem)Envir.AddToMap(dX, dY, CellType.Item, MapItem);
                                if (MapItemA != null)
                                {
                                    if (MapItemA != MapItem)
                                    {
                                        Dispose(MapItem);
                                        MapItem = MapItemA;
                                    }
                                    SendRefMsg(Grobal2.RM_ITEMSHOW, MapItem.Looks, MapItem.ActorId, dX, dY, MapItem.Name + NameCorlr);
                                }
                                else
                                {
                                    Dispose(MapItem);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            Dispose(UserItem);
                        }
                    }
                }
            }
            catch
            {
                M2Share.Log.LogError("{异常} TNormNpc.ActionOfTHROWITEM");
            }
        }

        public bool ActionOfTHROWITEM_RobotGetDropPosition(Envirnoment neEnvir, int nOrgX, int nOrgY, int nRange, ref int nDX, ref int nDY)
        {
            bool result;
            int nItemCount = 0;
            int n24;
            int n28;
            int n2C;
            n24 = 999;
            result = false;
            n28 = 0;
            n2C = 0;
            for (var i = 0; i < nRange; i++)
            {
                for (var j = -i; j <= i; j++)
                {
                    for (var k = -i; k <= i; k++)
                    {
                        nDX = nOrgX + k;
                        nDY = nOrgY + j;
                        if (neEnvir.GetItemEx(nDX, nDY, ref nItemCount) == 0)
                        {
                            if (neEnvir.Bo2C)
                            {
                                result = true;
                                break;
                            }
                        }
                        else
                        {
                            if (neEnvir.Bo2C && n24 > nItemCount)
                            {
                                n24 = nItemCount;
                                n28 = nDX;
                                n2C = nDY;
                            }
                        }
                    }
                    if (result)
                    {
                        break;
                    }
                }
                if (result)
                {
                    break;
                }
            }
            if (!result)
            {
                if (n24 < 8)
                {
                    nDX = n28;
                    nDY = n2C;
                }
                else
                {
                    nDX = nOrgX;
                    nDY = nOrgY;
                }
            }
            return result;
        }
    }
}