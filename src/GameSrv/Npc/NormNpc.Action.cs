using GameSrv.Actor;
using GameSrv.Event.Events;
using GameSrv.GameCommand;
using GameSrv.Items;
using GameSrv.Maps;
using GameSrv.Player;
using GameSrv.Script;
using GameSrv.Services;
using GameSrv.World;
using SystemModule.Common;
using SystemModule.Data;
using SystemModule.Enums;
using SystemModule.Packets.ClientPackets;

namespace GameSrv.Npc
{
    public partial class NormNpc
    {
        /// <summary>
        /// 开通元宝交易
        /// </summary>
        private void ActionOfOpenSaleDeal(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            var nGameGold = 0;
            try
            {
                if (playObject.SaleDeal)
                {
                    playObject.SendMsg(this, Messages.RM_MERCHANTSAY, 0, 0, 0, 0, ChrName + "/您已开通寄售服务,不需要再开通!!!\\ \\<返回/@main>");
                    return;// 如已开通元宝服务则退出
                }
                if (!GetValValue(playObject, questActionInfo.sParam1, ref nGameGold))
                {
                    nGameGold = HUtil32.StrToInt(GetLineVariableText(playObject, questActionInfo.sParam1), 0);
                }
                if (playObject.GameGold >= nGameGold)// 玩家的元宝数大于或等于开通所需的元宝数
                {
                    playObject.GameGold -= nGameGold;
                    playObject.SaleDeal = true;
                    playObject.SendMsg(this, Messages.RM_MERCHANTSAY, 0, 0, 0, 0, ChrName + "/开通寄售服务成功!!!\\ \\<返回/@main>");
                }
                else
                {
                    playObject.SendMsg(this, Messages.RM_MERCHANTSAY, 0, 0, 0, 0, ChrName + "/您身上没有" + M2Share.Config.GameGoldName + ",或" + M2Share.Config.GameGoldName + "数不够!!!\\ \\<返回/@main>");
                }
            }
            catch
            {
                M2Share.Logger.Error("{异常} TNormNpc.ActionOfOPENYBDEAL");
            }
        }

        /// <summary>
        /// 查询正在出售的物品
        /// </summary>
        private void ActionOfQuerySaleSell(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            try
            {
                var bo12 = false;
                if (playObject.SaleDeal) // 已开通元宝服务
                {
                    if (playObject.SellOffInTime(0))
                    {
                        if (M2Share.SellOffItemList.Count > 0)
                        {
                            var sClientDealOffInfo = new ClientDealOffInfo();
                            sClientDealOffInfo.UseItems = new ClientItem[9];
                            for (var i = 0; i < M2Share.SellOffItemList.Count; i++)
                            {
                                var dealOffInfo = M2Share.SellOffItemList[i];
                                if (string.Compare(dealOffInfo.sDealChrName, playObject.ChrName, StringComparison.OrdinalIgnoreCase) == 0 && (dealOffInfo.Flag == 0 || dealOffInfo.Flag == 3))
                                {
                                    for (var j = 0; j < 9; j++)
                                    {
                                        if (dealOffInfo.UseItems[j] == null)
                                        {
                                            continue;
                                        }
                                        var stdItem = M2Share.WorldEngine.GetStdItem(dealOffInfo.UseItems[j].Index);
                                        if (stdItem == null)
                                        {
                                            // 是金刚石
                                            if (!bo12 && dealOffInfo.UseItems[j].MakeIndex > 0 && dealOffInfo.UseItems[j].Index == ushort.MaxValue && dealOffInfo.UseItems[j].Dura == ushort.MaxValue && dealOffInfo.UseItems[j].DuraMax == ushort.MaxValue)
                                            {
                                                var wvar1 = sClientDealOffInfo.UseItems[j];// '金刚石'
                                                //_wvar1.S.Name = Settings.g_Config.sGameDiaMond + '(' + (DealOffInfo.UseItems[K].MakeIndex).ToString() + ')';
                                                //_wvar1.S.Price = DealOffInfo.UseItems[K].MakeIndex;// 金刚石数量
                                                wvar1.Dura = ushort.MaxValue;// 客户端金刚石特征
                                                wvar1.Item.DuraMax = ushort.MaxValue;// 客户端金刚石特征
                                                wvar1.Item.Looks = ushort.MaxValue;// 不显示图片
                                                bo12 = true;
                                            }
                                            else
                                            {
                                                sClientDealOffInfo.UseItems[j].Item.Name = "";
                                            }
                                            continue;
                                        }
                                        var stdItem80 = stdItem;
                                        //M2Share.ItemUnit.GetItemAddValue(DealOffInfo.UseItems[K], ref StdItem80);
                                        //Move(StdItem80, sClientDealOffInfo.UseItems[K].S, sizeof(TStdItem));
                                        sClientDealOffInfo.UseItems[j] = new ClientItem();
                                        stdItem80.GetUpgradeStdItem(dealOffInfo.UseItems[j], ref sClientDealOffInfo.UseItems[j]);
                                        //sClientDealOffInfo.UseItems[j].S = StdItem80;
                                        // 取自定义物品名称
                                        var sUserItemName = string.Empty;
                                        if (dealOffInfo.UseItems[j].Desc[13] == 1)
                                        {
                                            sUserItemName = M2Share.CustomItemMgr.GetCustomItemName(dealOffInfo.UseItems[j].MakeIndex, dealOffInfo.UseItems[j].Index);
                                            if (!string.IsNullOrEmpty(sUserItemName))
                                            {
                                                sClientDealOffInfo.UseItems[j].Item.Name = sUserItemName;
                                            }
                                        }
                                        sClientDealOffInfo.UseItems[j].MakeIndex = dealOffInfo.UseItems[j].MakeIndex;
                                        sClientDealOffInfo.UseItems[j].Dura = dealOffInfo.UseItems[j].Dura;
                                        sClientDealOffInfo.UseItems[j].DuraMax = dealOffInfo.UseItems[j].DuraMax;
                                        switch (stdItem.StdMode)
                                        {
                                            case 15:
                                            case 19:
                                            case 26:
                                                if (dealOffInfo.UseItems[j].Desc[8] != 0)
                                                {
                                                    sClientDealOffInfo.UseItems[j].Item.Shape = 130;
                                                }
                                                break;
                                        }
                                    }
                                    sClientDealOffInfo.DealChrName = dealOffInfo.sDealChrName;
                                    sClientDealOffInfo.BuyChrName = dealOffInfo.sBuyChrName;
                                    sClientDealOffInfo.SellDateTime = HUtil32.DateTimeToDouble(dealOffInfo.dSellDateTime);
                                    sClientDealOffInfo.SellGold = dealOffInfo.nSellGold;
                                    sClientDealOffInfo.N = dealOffInfo.Flag;
                                    var sSendStr = EDCode.EncodeBuffer(sClientDealOffInfo);
                                    playObject.SendMsg(this, Messages.RM_QUERYYBSELL, 0, 0, 0, 0, sSendStr);
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        GotoLable(playObject, "@AskYBSellFail", false);
                    }
                }
                else
                {
                    playObject.SendMsg(playObject, Messages.RM_MENU_OK, 0, playObject.ActorId, 0, 0, "您未开通寄售服务,请先开通!!!");
                }
            }
            catch
            {
                M2Share.Logger.Error("{异常} TNormNpc.ActionOfQUERYYBSELL");
            }
        }

        /// <summary>
        /// 查询可以的购买物品
        /// </summary>
        private void ActionOfQueryTrustDeal(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            try
            {
                var bo12 = false;
                if (playObject.SaleDeal)  // 已开通元宝服务
                {
                    if (playObject.SellOffInTime(1))
                    {
                        if (M2Share.SellOffItemList.Count > 0)
                        {
                            var sClientDealOffInfo = new ClientDealOffInfo();
                            sClientDealOffInfo.UseItems = new ClientItem[9];
                            for (var i = 0; i < M2Share.SellOffItemList.Count; i++)
                            {
                                var dealOffInfo = M2Share.SellOffItemList[i];
                                if (string.Compare(dealOffInfo.sBuyChrName, playObject.ChrName, StringComparison.OrdinalIgnoreCase) == 0 && dealOffInfo.Flag == 0)
                                {
                                    for (var k = 0; k < 9; k++)
                                    {
                                        if (dealOffInfo.UseItems[k] == null)
                                        {
                                            continue;
                                        }
                                        var stdItem = M2Share.WorldEngine.GetStdItem(dealOffInfo.UseItems[k].Index);
                                        if (stdItem == null)
                                        {
                                            // 是金刚石
                                            if (!bo12 && dealOffInfo.UseItems[k].MakeIndex > 0 && dealOffInfo.UseItems[k].Index == short.MaxValue && dealOffInfo.UseItems[k].Dura == short.MaxValue && dealOffInfo.UseItems[k].DuraMax == short.MaxValue)
                                            {
                                                var wvar1 = sClientDealOffInfo.UseItems[k];// '金刚石'
                                                //_wvar1.S.Name = Settings.g_Config.sGameDiaMond + '(' + (DealOffInfo.UseItems[K].MakeIndex).ToString() + ')';
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

                                        //M2Share.ItemUnit.GetItemAddValue(DealOffInfo.UseItems[K], ref StdItem80);
                                        //Move(StdItem80, sClientDealOffInfo.UseItems[K].S);// 取自定义物品名称
                                        //sClientDealOffInfo.UseItems[K].S = StdItem80;
                                        sClientDealOffInfo.UseItems[k] = new ClientItem();
                                        //StdItem80.GetStandardItem(ref sClientDealOffInfo.UseItems[k].Item);
                                        var sUserItemName = string.Empty;
                                        if (dealOffInfo.UseItems[k].Desc[13] == 1)
                                        {
                                            sUserItemName = M2Share.CustomItemMgr.GetCustomItemName(dealOffInfo.UseItems[k].MakeIndex, dealOffInfo.UseItems[k].Index);
                                        }
                                        if (!string.IsNullOrEmpty(sUserItemName))
                                        {
                                            sClientDealOffInfo.UseItems[k].Item.Name = sUserItemName;
                                        }
                                        sClientDealOffInfo.UseItems[k].MakeIndex = dealOffInfo.UseItems[k].MakeIndex;
                                        sClientDealOffInfo.UseItems[k].Dura = dealOffInfo.UseItems[k].Dura;
                                        sClientDealOffInfo.UseItems[k].DuraMax = dealOffInfo.UseItems[k].DuraMax;
                                        switch (stdItem.StdMode)
                                        {
                                            // Modify the A .. B: 15, 19 .. 24, 26
                                            case 15:
                                            case 19:
                                            case 26:
                                                if (dealOffInfo.UseItems[k].Desc[8] != 0)
                                                {
                                                    sClientDealOffInfo.UseItems[k].Item.Shape = 130;
                                                }
                                                break;
                                        }
                                    }
                                    sClientDealOffInfo.DealChrName = dealOffInfo.sDealChrName;
                                    sClientDealOffInfo.BuyChrName = dealOffInfo.sBuyChrName;
                                    sClientDealOffInfo.SellDateTime = HUtil32.DateTimeToDouble(dealOffInfo.dSellDateTime);
                                    sClientDealOffInfo.SellGold = dealOffInfo.nSellGold;
                                    sClientDealOffInfo.N = dealOffInfo.Flag;
                                    var sSendStr = EDCode.EncodeBuffer(sClientDealOffInfo);
                                    playObject.SendMsg(this, Messages.RM_QUERYYBDEAL, 0, 0, 0, 0, sSendStr);
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        GotoLable(playObject, "@AskYBDealFail", false);
                    }
                }
                else
                {
                    playObject.SendMsg(playObject, Messages.RM_MENU_OK, 0, playObject.ActorId, 0, 0, "您未开通元宝寄售服务,请先开通!!!");
                }
            }
            catch
            {
                M2Share.Logger.Error("{异常} TNormNpc.ActionOfQueryTrustDeal");
            }
        }

        private void ActionOfAddNameDateList(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            var sHumName = string.Empty;
            var sDate = string.Empty;
            var sListFileName = M2Share.GetEnvirFilePath(m_sPath, questActionInfo.sParam1);
            using var loadList = new StringList();
            if (File.Exists(sListFileName))
            {
                loadList.LoadFromFile(sListFileName);
            }
            var boFound = false;
            for (var i = 0; i < loadList.Count; i++)
            {
                var sLineText = loadList[i].Trim();
                sLineText = HUtil32.GetValidStr3(sLineText, ref sHumName, new[] { ' ', '\t' });
                sLineText = HUtil32.GetValidStr3(sLineText, ref sDate, new[] { ' ', '\t' });
                if (string.Compare(sHumName, playObject.ChrName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    loadList[i] = playObject.ChrName + "\t" + DateTime.Today;
                    boFound = true;
                    break;
                }
            }
            if (!boFound)
            {
                loadList.Add(playObject.ChrName + "\t" + DateTime.Today);
            }
            try
            {
                loadList.SaveToFile(sListFileName);
            }
            catch
            {
                M2Share.Logger.Error("saving fail.... => " + sListFileName);
            }
        }

        private void ActionOfDelNameDateList(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            var sHumName = string.Empty;
            var sDate = string.Empty;
            var sListFileName = M2Share.GetEnvirFilePath(m_sPath, questActionInfo.sParam1);
            using var loadList = new StringList();
            if (File.Exists(sListFileName))
            {
                loadList.LoadFromFile(sListFileName);
            }
            var boFound = false;
            for (var i = 0; i < loadList.Count; i++)
            {
                var sLineText = loadList[i].Trim();
                sLineText = HUtil32.GetValidStr3(sLineText, ref sHumName, new[] { ' ', '\t' });
                sLineText = HUtil32.GetValidStr3(sLineText, ref sDate, new[] { ' ', '\t' });
                if (string.Compare(sHumName, playObject.ChrName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    loadList.RemoveAt(i);
                    boFound = true;
                    break;
                }
            }
            if (boFound)
            {
                loadList.SaveToFile(sListFileName);
            }
        }

        private void ActionOfAddSkill(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            var nLevel = HUtil32._MIN(3, HUtil32.StrToInt(questActionInfo.sParam2, 0));
            var magic = M2Share.WorldEngine.FindMagic(questActionInfo.sParam1);
            if (magic != null)
            {
                if (!playObject.IsTrainingSkill(magic.MagicId))
                {
                    var userMagic = new UserMagic();
                    userMagic.Magic = magic;
                    userMagic.MagIdx = magic.MagicId;
                    userMagic.Key = (char)0;
                    userMagic.Level = (byte)nLevel;
                    userMagic.TranPoint = 0;
                    playObject.MagicList.Add(userMagic);
                    playObject.SendAddMagic(userMagic);
                    playObject.RecalcAbilitys();
                    if (M2Share.Config.ShowScriptActionMsg)
                    {
                        playObject.SysMsg(magic.MagicName + "练习成功。", MsgColor.Green, MsgType.Hint);
                    }
                }
            }
            else
            {
                ScriptActionError(playObject, "", questActionInfo, ScriptConst.sSC_ADDSKILL);
            }
        }

        private void ActionOfAutoAddGameGold(PlayObject playObject, QuestActionInfo questActionInfo, int nPoint, int nTime)
        {
            if (string.Compare(questActionInfo.sParam1, "START", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (nPoint > 0 && nTime > 0)
                {
                    playObject.IncGameGold = nPoint;
                    playObject.IncGameGoldTime = nTime * 1000;
                    playObject.IncGameGoldTick = HUtil32.GetTickCount();
                    playObject.BoIncGameGold = true;
                    return;
                }
            }
            if (string.Compare(questActionInfo.sParam1, "STOP", StringComparison.OrdinalIgnoreCase) == 0)
            {
                playObject.BoIncGameGold = false;
                return;
            }
            ScriptActionError(playObject, "", questActionInfo, ScriptConst.sSC_AUTOADDGAMEGOLD);
        }

        // SETAUTOGETEXP 时间 点数 是否安全区 地图号
        private void ActionOfAutoGetExp(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            Envirnoment envir = null;
            var nTime = HUtil32.StrToInt(questActionInfo.sParam1, -1);
            var nPoint = HUtil32.StrToInt(questActionInfo.sParam2, -1);
            var boIsSafeZone = questActionInfo.sParam3[1] == '1';
            var sMap = questActionInfo.sParam4;
            if (string.IsNullOrEmpty(sMap))
            {
                envir = M2Share.MapMgr.FindMap(sMap);
            }
            if (nTime <= 0 || nPoint <= 0 || string.IsNullOrEmpty(sMap) && envir == null)
            {
                ScriptActionError(playObject, "", questActionInfo, ScriptConst.sSC_SETAUTOGETEXP);
                return;
            }
            playObject.AutoGetExpInSafeZone = boIsSafeZone;
            playObject.AutoGetExpEnvir = envir;
            playObject.AutoGetExpTime = nTime * 1000;
            playObject.AutoGetExpPoint = nPoint;
        }

        /// <summary>
        /// 增加挂机
        /// </summary>
        private static void ActionOfOffLine(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            const string sOffLineStartMsg = "系统已经为你开启了脱机泡点功能，你现在可以下线了……";
            playObject.ClientMsg = Messages.MakeMessage(Messages.SM_SYSMESSAGE, playObject.ActorId, HUtil32.MakeWord(M2Share.Config.CustMsgFColor, M2Share.Config.CustMsgBColor), 0, 1);
            playObject.SendSocket(playObject.ClientMsg, EDCode.EncodeString(sOffLineStartMsg));
            var nTime = HUtil32.StrToInt(questActionInfo.sParam1, 5);
            var nPoint = HUtil32.StrToInt(questActionInfo.sParam2, 500);
            var nKickOffLine = HUtil32.StrToInt(questActionInfo.sParam3, 1440 * 15);
            playObject.AutoGetExpInSafeZone = true;
            playObject.AutoGetExpEnvir = playObject.Envir;
            playObject.AutoGetExpTime = nTime * 1000;
            playObject.AutoGetExpPoint = nPoint;
            playObject.OffLineFlag = true;
            playObject.KickOffLineTick = HUtil32.GetTickCount() + nKickOffLine * 60 * 1000;
            IdSrvClient.Instance.SendHumanLogOutMsgA(playObject.UserAccount, playObject.SessionId);
            playObject.SendDefMessage(Messages.SM_OUTOFCONNECTION, 0, 0, 0, 0);
        }

        private void ActionOfAutoSubGameGold(PlayObject playObject, QuestActionInfo questActionInfo, int nPoint, int nTime)
        {
            if (string.Compare(questActionInfo.sParam1, "START", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (nPoint > 0 && nTime > 0)
                {
                    playObject.DecGameGold = nPoint;
                    playObject.DecGameGoldTime = nTime * 1000;
                    playObject.DecGameGoldTick = 0;
                    playObject.BoDecGameGold = true;
                    return;
                }
            }
            if (string.Compare(questActionInfo.sParam1, "STOP", StringComparison.OrdinalIgnoreCase) == 0)
            {
                playObject.BoDecGameGold = false;
                return;
            }
            ScriptActionError(playObject, "", questActionInfo, ScriptConst.sSC_AUTOSUBGAMEGOLD);
        }

        private void ActionOfChangeCreditPoint(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            var nCreditPoint = (byte)HUtil32.StrToInt(questActionInfo.sParam2, -1);
            if (nCreditPoint < 0)
            {
                ScriptActionError(playObject, "", questActionInfo, ScriptConst.sSC_CREDITPOINT);
                return;
            }
            var cMethod = questActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (nCreditPoint >= 0)
                    {
                        if (nCreditPoint > byte.MaxValue)
                        {
                            playObject.CreditPoint = byte.MaxValue;
                        }
                        else
                        {
                            playObject.CreditPoint = nCreditPoint;
                        }
                    }
                    break;
                case '-':
                    if (playObject.CreditPoint > nCreditPoint)
                    {
                        playObject.CreditPoint -= nCreditPoint;
                    }
                    else
                    {
                        playObject.CreditPoint = 0;
                    }
                    break;
                case '+':
                    if (playObject.CreditPoint + nCreditPoint > byte.MaxValue)
                    {
                        playObject.CreditPoint = byte.MaxValue;
                    }
                    else
                    {
                        playObject.CreditPoint += nCreditPoint;
                    }
                    break;
                default:
                    ScriptActionError(playObject, "", questActionInfo, ScriptConst.sSC_CREDITPOINT);
                    return;
            }
        }

        private void ActionOfChangeExp(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            int dwInt;
            var nExp = HUtil32.StrToInt(questActionInfo.sParam2, -1);
            if (nExp < 0)
            {
                ScriptActionError(playObject, "", questActionInfo, ScriptConst.sSC_CHANGEEXP);
                return;
            }
            var cMethod = questActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (nExp > 0)
                    {
                        playObject.Abil.Exp = nExp;
                    }
                    break;
                case '-':
                    if (playObject.Abil.Exp > nExp)
                    {
                        playObject.Abil.Exp -= nExp;
                    }
                    else
                    {
                        playObject.Abil.Exp = 0;
                    }
                    break;
                case '+':
                    if (playObject.Abil.Exp >= nExp)
                    {
                        if (playObject.Abil.Exp - nExp > int.MaxValue - playObject.Abil.Exp)
                        {
                            dwInt = int.MaxValue - playObject.Abil.Exp;
                        }
                        else
                        {
                            dwInt = nExp;
                        }
                    }
                    else
                    {
                        if (nExp - playObject.Abil.Exp > int.MaxValue - nExp)
                        {
                            dwInt = int.MaxValue - nExp;
                        }
                        else
                        {
                            dwInt = nExp;
                        }
                    }
                    playObject.Abil.Exp += dwInt;
                    // PlayObject.GetExp(dwInt);
                    playObject.SendMsg(playObject, Messages.RM_WINEXP, 0, dwInt, 0, 0);
                    break;
            }
        }

        private void ActionOfChangeHairStyle(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            var nHair = HUtil32.StrToInt(questActionInfo.sParam1, -1);
            if ((!string.IsNullOrEmpty(questActionInfo.sParam1)) && nHair >= 0)
            {
                playObject.Hair = (byte)nHair;
                playObject.FeatureChanged();
            }
            else
            {
                ScriptActionError(playObject, "", questActionInfo, ScriptConst.sSC_HAIRSTYLE);
            }
        }

        private void ActionOfChangeJob(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            var nJob = PlayJob.None;
            if (HUtil32.CompareLStr(questActionInfo.sParam1, ScriptConst.sWarrior))
            {
                nJob = PlayJob.Warrior;
            }
            if (HUtil32.CompareLStr(questActionInfo.sParam1, ScriptConst.sWizard))
            {
                nJob = PlayJob.Wizard;
            }
            if (HUtil32.CompareLStr(questActionInfo.sParam1, ScriptConst.sTaos))
            {
                nJob = PlayJob.Taoist;
            }
            if (nJob == PlayJob.None)
            {
                ScriptActionError(playObject, "", questActionInfo, ScriptConst.sSC_CHANGEJOB);
                return;
            }
            if (playObject.Job != nJob)
            {
                playObject.Job = nJob;
                playObject.HasLevelUp(0);
            }
        }

        private void ActionOfChangeLevel(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            int nLv;
            var boChgOk = false;
            int nOldLevel = playObject.Abil.Level;
            var nLevel = HUtil32.StrToInt(questActionInfo.sParam2, -1);
            if (nLevel < 0)
            {
                ScriptActionError(playObject, "", questActionInfo, ScriptConst.sSC_CHANGELEVEL);
                return;
            }
            var cMethod = questActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (nLevel > 0 && nLevel <= Grobal2.MaxLevel)
                    {
                        playObject.Abil.Level = (byte)nLevel;
                        boChgOk = true;
                    }
                    break;
                case '-':
                    nLv = HUtil32._MAX(0, playObject.Abil.Level - nLevel);
                    nLv = HUtil32._MIN(Grobal2.MaxLevel, nLv);
                    playObject.Abil.Level = (byte)nLv;
                    boChgOk = true;
                    break;
                case '+':
                    nLv = HUtil32._MAX(0, playObject.Abil.Level + nLevel);
                    nLv = HUtil32._MIN(Grobal2.MaxLevel, nLv);
                    playObject.Abil.Level = (byte)nLv;
                    boChgOk = true;
                    break;
            }
            if (boChgOk)
            {
                playObject.HasLevelUp(nOldLevel);
            }
        }

        private void ActionOfChangePkPoint(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            int nPoint;
            var nOldPkLevel = playObject.PvpLevel();
            var nPkPoint = HUtil32.StrToInt(questActionInfo.sParam2, -1);
            if (nPkPoint < 0)
            {
                ScriptActionError(playObject, "", questActionInfo, ScriptConst.sSC_CHANGEPKPOINT);
                return;
            }
            var cMethod = questActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (nPkPoint >= 0)
                    {
                        playObject.PkPoint = nPkPoint;
                    }
                    break;
                case '-':
                    nPoint = HUtil32._MAX(0, playObject.PkPoint - nPkPoint);
                    playObject.PkPoint = nPoint;
                    break;
                case '+':
                    nPoint = HUtil32._MAX(0, playObject.PkPoint + nPkPoint);
                    playObject.PkPoint = nPoint;
                    break;
            }
            if (nOldPkLevel != playObject.PvpLevel())
            {
                playObject.RefNameColor();
            }
        }

        private static void ActionOfClearMapMon(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            IList<BaseObject> monList = new List<BaseObject>();
            var monsterCount = M2Share.WorldEngine.GetMapMonster(M2Share.MapMgr.FindMap(questActionInfo.sParam1), monList);
            for (var i = 0; i < monsterCount; i++)
            {
                var mon = monList[i];
                if (mon.Master != null)
                {
                    continue;
                }
                if (M2Share.GetNoClearMonList(mon.ChrName))
                {
                    continue;
                }
                mon.NoItem = true;
                mon.WAbil.HP = 0;
                mon.MakeGhost();
            }
        }

        private static void ActionOfClearList(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            var sListFileName = M2Share.GetEnvirFilePath(questActionInfo.sParam1);
            File.WriteAllBytes(sListFileName, Array.Empty<byte>());
        }

        private static void ActionOfClearSkill(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            for (var i = playObject.MagicList.Count - 1; i >= 0; i--)
            {
                var userMagic = playObject.MagicList[i];
                playObject.SendDelMagic(userMagic);
                playObject.MagicList.RemoveAt(i);
                Dispose(userMagic);
            }
            playObject.RecalcAbilitys();
        }

        private static void ActionOfDelNoJobSkill(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            for (var i = playObject.MagicList.Count - 1; i >= 0; i--)
            {
                var userMagic = playObject.MagicList[i];
                if (userMagic.Magic.Job != (byte)playObject.Job)
                {
                    playObject.SendDelMagic(userMagic);
                    playObject.MagicList.RemoveAt(i);
                    Dispose(userMagic);
                }
            }
        }

        private void ActionOfDelSkill(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            var magic = M2Share.WorldEngine.FindMagic(questActionInfo.sParam1);
            if (magic == null)
            {
                ScriptActionError(playObject, "", questActionInfo, ScriptConst.sSC_DELSKILL);
                return;
            }
            for (var i = 0; i < playObject.MagicList.Count; i++)
            {
                var userMagic = playObject.MagicList[i];
                if (string.CompareOrdinal(userMagic.Magic.MagicName, magic.MagicName) == 0)
                {
                    playObject.MagicList.RemoveAt(i);
                    playObject.SendDelMagic(userMagic);
                    Dispose(userMagic);
                    playObject.RecalcAbilitys();
                    break;
                }
            }
        }

        private void ActionOfGameGold(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            var nOldGameGold = playObject.GameGold;
            var nGameGold = HUtil32.StrToInt(questActionInfo.sParam2, -1);
            if (nGameGold < 0)
            {
                ScriptActionError(playObject, "", questActionInfo, ScriptConst.sSC_GAMEGOLD);
                return;
            }
            var cMethod = questActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (nGameGold >= 0)
                    {
                        playObject.GameGold = nGameGold;
                    }
                    break;
                case '-':
                    nGameGold = HUtil32._MAX(0, playObject.GameGold - nGameGold);
                    playObject.GameGold = nGameGold;
                    break;
                case '+':
                    nGameGold = HUtil32._MAX(0, playObject.GameGold + nGameGold);
                    playObject.GameGold = nGameGold;
                    break;
            }
            if (M2Share.GameLogGameGold)
            {
                M2Share.EventSource.AddEventLog(Grobal2.LogGameGold, Format(CommandHelp.GameLogMsg1, playObject.MapName, playObject.CurrX, playObject.CurrY, playObject.ChrName, M2Share.Config.GameGoldName, nGameGold, cMethod, ChrName));
            }
            if (nOldGameGold != playObject.GameGold)
            {
                playObject.GameGoldChanged();
            }
        }

        private void ActionOfGamePoint(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            var nOldGamePoint = playObject.GamePoint;
            var nGamePoint = HUtil32.StrToInt(questActionInfo.sParam2, -1);
            if (nGamePoint < 0)
            {
                ScriptActionError(playObject, "", questActionInfo, ScriptConst.sSC_GAMEPOINT);
                return;
            }
            var cMethod = questActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    if (nGamePoint >= 0)
                    {
                        playObject.GamePoint = nGamePoint;
                    }
                    break;
                case '-':
                    nGamePoint = HUtil32._MAX(0, playObject.GamePoint - nGamePoint);
                    playObject.GamePoint = nGamePoint;
                    break;
                case '+':
                    nGamePoint = HUtil32._MAX(0, playObject.GamePoint + nGamePoint);
                    playObject.GamePoint = nGamePoint;
                    break;
            }
            if (M2Share.GameLogGamePoint)
            {
                M2Share.EventSource.AddEventLog(Grobal2.LogGamePoint, Format(CommandHelp.GameLogMsg1, playObject.MapName, playObject.CurrX, playObject.CurrY, playObject.ChrName, M2Share.Config.GamePointName, nGamePoint, cMethod, ChrName));
            }
            if (nOldGamePoint != playObject.GamePoint)
            {
                playObject.GameGoldChanged();
            }
        }

        private void ActionOfGetMarry(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            var poseHuman = playObject.GetPoseCreate();
            if (poseHuman != null && poseHuman.Race == ActorRace.Play && ((PlayObject)poseHuman).Gender != playObject.Gender)
            {
                playObject.DearName = poseHuman.ChrName;
                playObject.RefShowName();
                poseHuman.RefShowName();
            }
            else
            {
                GotoLable(playObject, "@MarryError", false);
            }
        }

        private void ActionOfGetMaster(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            var poseHuman = playObject.GetPoseCreate();
            if (poseHuman != null && poseHuman.Race == ActorRace.Play && ((PlayObject)poseHuman).Gender != playObject.Gender)
            {
                playObject.MasterName = poseHuman.ChrName;
                playObject.RefShowName();
                poseHuman.RefShowName();
            }
            else
            {
                GotoLable(playObject, "@MasterError", false);
            }
        }

        private void ActionOfLineMsg(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            var sMsg = GetLineVariableText(playObject, questActionInfo.sParam2);
            sMsg = sMsg.Replace("%s", playObject.ChrName);
            sMsg = sMsg.Replace("%d", ChrName);
            switch (questActionInfo.nParam1)
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
                    M2Share.WorldEngine.SendBroadCastMsg('[' + playObject.ChrName + ']' + sMsg, MsgType.System);
                    break;
                case 4:
                    ProcessSayMsg(sMsg);
                    break;
                case 5:
                    playObject.SysMsg(sMsg, MsgColor.Red, MsgType.Say);
                    break;
                case 6:
                    playObject.SysMsg(sMsg, MsgColor.Green, MsgType.Say);
                    break;
                case 7:
                    playObject.SysMsg(sMsg, MsgColor.Blue, MsgType.Say);
                    break;
                case 8:
                    playObject.SendGroupText(sMsg);
                    break;
                case 9:
                    if (playObject.MyGuild != null)
                    {
                        playObject.MyGuild.SendGuildMsg(sMsg);
                        WorldServer.SendServerGroupMsg(Messages.SS_208, M2Share.ServerIndex, playObject.MyGuild.GuildName + "/" + playObject.ChrName + "/" + sMsg);
                    }
                    break;
                default:
                    ScriptActionError(playObject, "", questActionInfo, ScriptConst.sSENDMSG);
                    break;
            }
        }

        private static void ActionOfMapTing(PlayObject playObject, QuestActionInfo questActionInfo)
        {

        }

        private void ActionOfMarry(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            string sSayMsg;
            if (!string.IsNullOrEmpty(playObject.DearName))
            {
                return;
            }
            var poseHuman = (PlayObject)playObject.GetPoseCreate();
            if (poseHuman == null)
            {
                GotoLable(playObject, "@MarryCheckDir", false);
                return;
            }
            if (string.IsNullOrEmpty(questActionInfo.sParam1))
            {
                if (poseHuman.Race != ActorRace.Play)
                {
                    GotoLable(playObject, "@HumanTypeErr", false);
                    return;
                }
                if (poseHuman.GetPoseCreate() == playObject)
                {
                    if (playObject.Gender != poseHuman.Gender)
                    {
                        GotoLable(playObject, "@StartMarry", false);
                        GotoLable(poseHuman, "@StartMarry", false);
                        if (playObject.Gender == PlayGender.Man && poseHuman.Gender == PlayGender.WoMan)
                        {
                            sSayMsg = string.Format(Settings.StartMarryManMsg, ChrName, playObject.ChrName, poseHuman.ChrName);
                            M2Share.WorldEngine.SendBroadCastMsg(sSayMsg, MsgType.Say);
                            sSayMsg = string.Format(Settings.StartMarryManAskQuestionMsg, ChrName, playObject.ChrName, poseHuman.ChrName);
                            M2Share.WorldEngine.SendBroadCastMsg(sSayMsg, MsgType.Say);
                        }
                        else if (playObject.Gender == PlayGender.WoMan && poseHuman.Gender == PlayGender.Man)
                        {
                            sSayMsg = string.Format(Settings.StartMarryWoManMsg, ChrName, playObject.ChrName, poseHuman.ChrName);
                            M2Share.WorldEngine.SendBroadCastMsg(sSayMsg, MsgType.Say);
                            sSayMsg = string.Format(Settings.StartMarryWoManAskQuestionMsg, ChrName, playObject.ChrName, poseHuman.ChrName);
                            M2Share.WorldEngine.SendBroadCastMsg(sSayMsg, MsgType.Say);
                        }
                        playObject.IsStartMarry = true;
                        poseHuman.IsStartMarry = true;
                    }
                    else
                    {
                        GotoLable(poseHuman, "@MarrySexErr", false);
                        GotoLable(playObject, "@MarrySexErr", false);
                    }
                }
                else
                {
                    GotoLable(playObject, "@MarryDirErr", false);
                    GotoLable(poseHuman, "@MarryCheckDir", false);
                }
                return;
            }
            // sREQUESTMARRY
            if (string.Compare(questActionInfo.sParam1, "REQUESTMARRY", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (playObject.IsStartMarry && poseHuman.IsStartMarry)
                {
                    if (playObject.Gender == PlayGender.Man && poseHuman.Gender == PlayGender.WoMan)
                    {
                        sSayMsg = Settings.MarryManAnswerQuestionMsg.Replace("%n", ChrName);
                        sSayMsg = sSayMsg.Replace("%s", playObject.ChrName);
                        sSayMsg = sSayMsg.Replace("%d", poseHuman.ChrName);
                        M2Share.WorldEngine.SendBroadCastMsg(sSayMsg, MsgType.Say);
                        sSayMsg = Settings.MarryManAskQuestionMsg.Replace("%n", ChrName);
                        sSayMsg = sSayMsg.Replace("%s", playObject.ChrName);
                        sSayMsg = sSayMsg.Replace("%d", poseHuman.ChrName);
                        M2Share.WorldEngine.SendBroadCastMsg(sSayMsg, MsgType.Say);
                        GotoLable(playObject, "@WateMarry", false);
                        GotoLable(poseHuman, "@RevMarry", false);
                    }
                }
                return;
            }
            // sRESPONSEMARRY
            if (string.Compare(questActionInfo.sParam1, "RESPONSEMARRY", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (playObject.Gender == PlayGender.WoMan && poseHuman.Gender == PlayGender.Man)
                {
                    if (string.Compare(questActionInfo.sParam2, "OK", StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        if (playObject.IsStartMarry && poseHuman.IsStartMarry)
                        {
                            sSayMsg = string.Format(Settings.MarryWoManAnswerQuestionMsg, ChrName, playObject.ChrName, poseHuman.ChrName);
                            M2Share.WorldEngine.SendBroadCastMsg(sSayMsg, MsgType.Say);
                            sSayMsg = string.Format(Settings.MarryWoManGetMarryMsg, ChrName, playObject.ChrName, poseHuman.ChrName);
                            M2Share.WorldEngine.SendBroadCastMsg(sSayMsg, MsgType.Say);
                            GotoLable(playObject, "@EndMarry", false);
                            GotoLable(poseHuman, "@EndMarry", false);
                            playObject.IsStartMarry = false;
                            poseHuman.IsStartMarry = false;
                            playObject.DearName = poseHuman.ChrName;
                            playObject.DearHuman = poseHuman;
                            poseHuman.DearName = playObject.ChrName;
                            poseHuman.DearHuman = playObject;
                            playObject.RefShowName();
                            poseHuman.RefShowName();
                        }
                    }
                    else
                    {
                        if (playObject.IsStartMarry && poseHuman.IsStartMarry)
                        {
                            GotoLable(playObject, "@EndMarryFail", false);
                            GotoLable(poseHuman, "@EndMarryFail", false);
                            playObject.IsStartMarry = false;
                            poseHuman.IsStartMarry = false;
                            sSayMsg = string.Format(Settings.MarryWoManDenyMsg, ChrName, playObject.ChrName, poseHuman.ChrName);
                            M2Share.WorldEngine.SendBroadCastMsg(sSayMsg, MsgType.Say);
                            sSayMsg = string.Format(Settings.MarryWoManCancelMsg, ChrName, playObject.ChrName, poseHuman.ChrName);
                            M2Share.WorldEngine.SendBroadCastMsg(sSayMsg, MsgType.Say);
                        }
                    }
                }
            }
        }

        private void ActionOfMaster(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            if (!string.IsNullOrEmpty(playObject.MasterName))
            {
                return;
            }
            var poseHuman = (PlayObject)playObject.GetPoseCreate();
            if (poseHuman == null)
            {
                GotoLable(playObject, "@MasterCheckDir", false);
                return;
            }
            if ((string.IsNullOrEmpty(questActionInfo.sParam1)))
            {
                if (poseHuman.Race != ActorRace.Play)
                {
                    GotoLable(playObject, "@HumanTypeErr", false);
                    return;
                }
                if (poseHuman.GetPoseCreate() == playObject)
                {
                    GotoLable(playObject, "@StartGetMaster", false);
                    GotoLable(poseHuman, "@StartMaster", false);
                    playObject.IsStartMaster = true;
                    poseHuman.IsStartMaster = true;
                }
                else
                {
                    GotoLable(playObject, "@MasterDirErr", false);
                    GotoLable(poseHuman, "@MasterCheckDir", false);
                }
                return;
            }
            if (string.Compare(questActionInfo.sParam1, "REQUESTMASTER", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (playObject.IsStartMaster && poseHuman.IsStartMaster)
                {
                    playObject.PoseBaseObject = poseHuman.ActorId;
                    poseHuman.PoseBaseObject = playObject.ActorId;
                    GotoLable(playObject, "@WateMaster", false);
                    GotoLable(poseHuman, "@RevMaster", false);
                }
                return;
            }
            if (string.Compare(questActionInfo.sParam1, "RESPONSEMASTER", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (string.Compare(questActionInfo.sParam2, "OK", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    if (playObject.PoseBaseObject == poseHuman.ActorId && poseHuman.PoseBaseObject == playObject.ActorId)
                    {
                        if (playObject.IsStartMaster && poseHuman.IsStartMaster)
                        {
                            GotoLable(playObject, "@EndMaster", false);
                            GotoLable(poseHuman, "@EndMaster", false);
                            playObject.IsStartMaster = false;
                            poseHuman.IsStartMaster = false;
                            if (string.IsNullOrEmpty(playObject.MasterName))
                            {
                                playObject.MasterName = poseHuman.ChrName;
                                playObject.IsMaster = true;
                            }
                            playObject.MasterList.Add(poseHuman);
                            poseHuman.MasterName = playObject.ChrName;
                            poseHuman.IsMaster = false;
                            playObject.RefShowName();
                            poseHuman.RefShowName();
                        }
                    }
                }
                else
                {
                    if (playObject.IsStartMaster && poseHuman.IsStartMaster)
                    {
                        GotoLable(playObject, "@EndMasterFail", false);
                        GotoLable(poseHuman, "@EndMasterFail", false);
                        playObject.IsStartMaster = false;
                        poseHuman.IsStartMaster = false;
                    }
                }
            }
        }

        private void ActionOfMessageBox(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            playObject.SendMsg(this, Messages.RM_MENU_OK, 0, playObject.ActorId, 0, 0, GetLineVariableText(playObject, questActionInfo.sParam1));
        }

        private void ActionOfMission(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            if (!string.IsNullOrEmpty(questActionInfo.sParam1) && questActionInfo.nParam2 > 0 && questActionInfo.nParam3 > 0)
            {
                M2Share.MissionMap = questActionInfo.sParam1;
                M2Share.MissionX = (short)questActionInfo.nParam2;
                M2Share.MissionY = (short)questActionInfo.nParam3;
            }
            else
            {
                ScriptActionError(playObject, "", questActionInfo, ScriptConst.sSC_MISSION);
            }
        }

        // MOBFIREBURN MAP X Y TYPE TIME POINT
        private void ActionOfMobFireBurn(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            var sMap = questActionInfo.sParam1;
            var nX = HUtil32.StrToInt16(questActionInfo.sParam2, -1);
            var nY = HUtil32.StrToInt16(questActionInfo.sParam3, -1);
            var nType = (byte)HUtil32.StrToInt(questActionInfo.sParam4, -1);
            var nTime = HUtil32.StrToInt(questActionInfo.sParam5, -1);
            var nPoint = HUtil32.StrToInt(questActionInfo.sParam6, -1);
            if (string.IsNullOrEmpty(sMap) || nX < 0 || nY < 0 || nType < 0 || nTime < 0 || nPoint < 0)
            {
                ScriptActionError(playObject, "", questActionInfo, ScriptConst.sSC_MOBFIREBURN);
                return;
            }
            var envir = M2Share.MapMgr.FindMap(sMap);
            if (envir != null)
            {
                var oldEnvir = playObject.Envir;
                playObject.Envir = envir;
                var fireBurnEvent = new FireBurnEvent(playObject, nX, nY, nType, nTime * 1000, nPoint);
                M2Share.EventMgr.AddEvent(fireBurnEvent);
                playObject.Envir = oldEnvir;
                return;
            }
            ScriptActionError(playObject, "", questActionInfo, ScriptConst.sSC_MOBFIREBURN);
        }

        private void ActionOfMobPlace(PlayObject playObject, QuestActionInfo questActionInfo, int nX, int nY, int nCount, int nRange)
        {
            for (var i = 0; i < nCount; i++)
            {
                var nRandX = (short)(M2Share.RandomNumber.Random(nRange * 2 + 1) + (nX - nRange));
                var nRandY = (short)(M2Share.RandomNumber.Random(nRange * 2 + 1) + (nY - nRange));
                var mon = M2Share.WorldEngine.RegenMonsterByName(M2Share.MissionMap, nRandX, nRandY, questActionInfo.sParam1);
                if (mon != null)
                {
                    mon.Mission = true;
                    mon.MissionX = M2Share.MissionX;
                    mon.MissionY = M2Share.MissionY;
                }
                else
                {
                    ScriptActionError(playObject, "", questActionInfo, ScriptConst.sSC_MOBPLACE);
                    break;
                }
            }
        }

        private static void ActionOfRecallGroupMembers(PlayObject playObject, QuestActionInfo questActionInfo)
        {
        }

        private void ActionOfSetRankLevelName(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            var sRankLevelName = questActionInfo.sParam1;
            if (string.IsNullOrEmpty(sRankLevelName))
            {
                ScriptActionError(playObject, "", questActionInfo, ScriptConst.sSC_SKILLLEVEL);
                return;
            }
            playObject.RankLevelName = sRankLevelName;
            playObject.RefShowName();
        }

        private void ActionOfSetScriptFlag(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            var nWhere = HUtil32.StrToInt(questActionInfo.sParam1, -1);
            var boFlag = HUtil32.StrToInt(questActionInfo.sParam2, -1) == 1;
            switch (nWhere)
            {
                case 0:
                    playObject.BoSendMsgFlag = boFlag;
                    break;
                case 1:
                    playObject.BoChangeItemNameFlag = boFlag;
                    break;
                default:
                    ScriptActionError(playObject, "", questActionInfo, ScriptConst.sSC_SETSCRIPTFLAG);
                    break;
            }
        }

        private void ActionOfSkillLevel(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            var nLevel = HUtil32.StrToInt(questActionInfo.sParam3, 0);
            if (nLevel < 0)
            {
                ScriptActionError(playObject, "", questActionInfo, ScriptConst.sSC_SKILLLEVEL);
                return;
            }
            var cMethod = questActionInfo.sParam2[0];
            var magic = M2Share.WorldEngine.FindMagic(questActionInfo.sParam1);
            if (magic != null)
            {
                for (var i = 0; i < playObject.MagicList.Count; i++)
                {
                    var userMagic = playObject.MagicList[i];
                    if (userMagic.Magic == magic)
                    {
                        switch (cMethod)
                        {
                            case '=':
                                if (nLevel >= 0)
                                {
                                    nLevel = HUtil32._MAX(3, nLevel);
                                    userMagic.Level = (byte)nLevel;
                                }
                                break;
                            case '-':
                                if (userMagic.Level >= nLevel)
                                {
                                    userMagic.Level -= (byte)nLevel;
                                }
                                else
                                {
                                    userMagic.Level = 0;
                                }
                                break;
                            case '+':
                                if (userMagic.Level + nLevel <= 3)
                                {
                                    userMagic.Level += (byte)nLevel;
                                }
                                else
                                {
                                    userMagic.Level = 3;
                                }
                                break;
                        }
                        playObject.SendSelfDelayMsg(Messages.RM_MAGIC_LVEXP, 0, userMagic.Magic.MagicId, userMagic.Level, userMagic.TranPoint, "", 100);
                        break;
                    }
                }
            }
        }

        private void ActionOfTakeCastleGold(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            var nGold = HUtil32.StrToInt(questActionInfo.sParam1, -1);
            if (nGold < 0 || Castle == null)
            {
                ScriptActionError(playObject, "", questActionInfo, ScriptConst.sSC_TAKECASTLEGOLD);
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

        private void ActionOfUnMarry(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            if (string.IsNullOrEmpty(playObject.DearName))
            {
                GotoLable(playObject, "@ExeMarryFail", false);
                return;
            }
            var poseHuman = (PlayObject)playObject.GetPoseCreate();
            if (poseHuman == null)
            {
                GotoLable(playObject, "@UnMarryCheckDir", false);
            }
            if (poseHuman != null)
            {
                if (string.IsNullOrEmpty(questActionInfo.sParam1))
                {
                    if (poseHuman.Race != ActorRace.Play)
                    {
                        GotoLable(playObject, "@UnMarryTypeErr", false);
                        return;
                    }
                    if (poseHuman.GetPoseCreate() == playObject)
                    {
                        if (playObject.DearName == poseHuman.ChrName)
                        {
                            GotoLable(playObject, "@StartUnMarry", false);
                            GotoLable(poseHuman, "@StartUnMarry", false);
                            return;
                        }
                    }
                }
            }
            // sREQUESTUNMARRY
            if (string.Compare(questActionInfo.sParam1, "REQUESTUNMARRY", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (string.IsNullOrEmpty(questActionInfo.sParam2))
                {
                    if (poseHuman != null)
                    {
                        playObject.IsStartUnMarry = true;
                        if (playObject.IsStartUnMarry && poseHuman.IsStartUnMarry)
                        {
                            // sUnMarryMsg8
                            // sMarryMsg0
                            // sUnMarryMsg9
                            M2Share.WorldEngine.SendBroadCastMsg('[' + ChrName + "]: " + "我宣布" + poseHuman.ChrName + ' ' + '与' + playObject.ChrName + ' ' + ' ' + "正式脱离夫妻关系。", MsgType.Say);
                            playObject.DearName = "";
                            poseHuman.DearName = "";
                            playObject.MarryCount++;
                            poseHuman.MarryCount++;
                            playObject.IsStartUnMarry = false;
                            poseHuman.IsStartUnMarry = false;
                            playObject.RefShowName();
                            poseHuman.RefShowName();
                            GotoLable(playObject, "@UnMarryEnd", false);
                            GotoLable(poseHuman, "@UnMarryEnd", false);
                        }
                        else
                        {
                            GotoLable(playObject, "@WateUnMarry", false);
                            // GotoLable(PoseHuman,'@RevUnMarry',False);
                        }
                    }
                }
                else
                {
                    // 强行离婚
                    if (string.Compare(questActionInfo.sParam2, "FORCE", StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        M2Share.WorldEngine.SendBroadCastMsg('[' + ChrName + "]: " + "我宣布" + playObject.ChrName + ' ' + '与' + playObject.DearName + ' ' + ' ' + "已经正式脱离夫妻关系!!!", MsgType.Say);
                        poseHuman = M2Share.WorldEngine.GetPlayObject(playObject.DearName);
                        if (poseHuman != null)
                        {
                            poseHuman.DearName = string.Empty;
                            poseHuman.MarryCount++;
                            poseHuman.RefShowName();
                        }
                        else
                        {
                            //sUnMarryFileName = Settings.g_Config.sEnvirDir + "UnMarry.txt";
                            //LoadList = new StringList();
                            //if (File.Exists(sUnMarryFileName))
                            //{
                            //    LoadList.LoadFromFile(sUnMarryFileName);
                            //}
                            //LoadList.Add(PlayObject.m_sDearName);
                            //LoadList.SaveToFile(sUnMarryFileName);
                            //LoadList.Free;
                        }
                        playObject.DearName = string.Empty;
                        playObject.MarryCount++;
                        GotoLable(playObject, "@UnMarryEnd", false);
                        playObject.RefShowName();
                    }
                }
            }
        }

        /// <summary>
        /// 保存变量值
        /// SAVEVAR 变量类型 变量名 文件名
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="questActionInfo"></param>
        private void ActionOfSaveVar(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            var sName = string.Empty;
            DynamicVar dynamicVar = null;
            const string sVarFound = "变量{0}不存在，变量类型:{1}";
            const string sVarTypeError = "变量类型错误，错误类型:{0} 当前支持类型(HUMAN、GUILD、GLOBAL)";
            var sType = questActionInfo.sParam1;
            var sVarName = questActionInfo.sParam2;
            var sFileName = GetLineVariableText(playObject, questActionInfo.sParam3);
            if (sFileName[0] == '\\')
            {
                sFileName = sFileName[1..];
            }
            if (sFileName[1] == '\\')
            {
                sFileName = sFileName[2..];
            }
            if (sFileName[2] == '\\')
            {
                sFileName = sFileName[3..];
            }
            sFileName = M2Share.GetEnvirFilePath(sFileName);
            if (string.IsNullOrEmpty(sType) || string.IsNullOrEmpty(sVarName) || !File.Exists(sFileName))
            {
                ScriptActionError(playObject, "", questActionInfo, ScriptConst.sSC_SAVEVAR);
                return;
            }
            var boFoundVar = false;
            var dynamicVarList = GetDynamicVarMap(playObject, sType, ref sName);
            if (dynamicVarList == null)
            {
                Dispose(dynamicVar);
                ScriptActionError(playObject, Format(sVarTypeError, sType), questActionInfo, ScriptConst.sSC_VAR);
                return;
            }
            if (dynamicVarList.TryGetValue(sVarName, out dynamicVar))
            {
                var iniFile = new ConfFile(sFileName);
                iniFile.Load();
                if (dynamicVar.VarType == VarType.Integer)
                {
                    dynamicVarList[sVarName].nInternet = dynamicVar.nInternet;
                    iniFile.WriteInteger(sName, dynamicVar.sName, dynamicVar.nInternet);
                }
                else
                {
                    dynamicVarList[sVarName].sString = dynamicVar.sString;
                    iniFile.WriteString(sName, dynamicVar.sName, dynamicVar.sString);
                }
                boFoundVar = true;
            }
            if (!boFoundVar)
            {
                ScriptActionError(playObject, Format(sVarFound, sVarName, sType), questActionInfo, ScriptConst.sSC_SAVEVAR);
            }
        }

        private void ActionOfDelayCall(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            playObject.DelayCall = HUtil32._MAX(1, questActionInfo.nParam1);
            playObject.DelayCallLabel = questActionInfo.sParam2;
            playObject.DelayCallTick = HUtil32.GetTickCount();
            playObject.IsDelayCall = true;
            playObject.DelayCallNpc = ActorId;
        }

        /// <summary>
        /// 对变量进行运算(+、-、*、/)
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="questActionInfo"></param>
        private void ActionOfCalcVar(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            var sName = string.Empty;
            DynamicVar dynamicVar = null;
            var sVarValue2 = string.Empty;
            const string sVarFound = "变量{0}不存在，变量类型:{1}";
            const string sVarTypeError = "变量类型错误，错误类型:{0} 当前支持类型(HUMAN、GUILD、GLOBAL)";
            var sType = questActionInfo.sParam1;//类型
            var sVarName = questActionInfo.sParam2;//自定义变量
            var sMethod = questActionInfo.sParam3;//操作符 +-*/=
            var sVarValue = questActionInfo.sParam4;//变量
            var nVarValue = HUtil32.StrToInt(questActionInfo.sParam4, 0);
            if (string.IsNullOrEmpty(sType) || string.IsNullOrEmpty(sVarName) || string.IsNullOrEmpty(sMethod))
            {
                ScriptActionError(playObject, "", questActionInfo, ScriptConst.sSC_CALCVAR);
                return;
            }
            var boFoundVar = false;
            if (!string.IsNullOrEmpty(sVarValue) && !HUtil32.IsStringNumber(sVarValue))
            {
                if (HUtil32.CompareLStr(sVarValue, "<$HUMAN(", 8))
                {
                    HUtil32.ArrestStringEx(sVarValue, "(", ")", ref sVarValue2);
                    sVarValue = sVarValue2;
                    if (playObject.DynamicVarMap.Count > 0)
                    {
                        if (playObject.DynamicVarMap.TryGetValue(sVarValue, out dynamicVar))
                        {
                            switch (dynamicVar.VarType)
                            {
                                case VarType.Integer:
                                    nVarValue = dynamicVar.nInternet;
                                    break;
                                case VarType.String:
                                    sVarValue = dynamicVar.sString;
                                    break;
                            }
                            boFoundVar = true;
                        }
                    }
                    if (!boFoundVar)
                    {
                        ScriptActionError(playObject, string.Format(sVarFound, sVarValue, sType), questActionInfo, ScriptConst.sSC_CALCVAR);
                        return;
                    }
                }
                else
                {
                    nVarValue = HUtil32.StrToInt(GetLineVariableText(playObject, sVarValue), 0);
                    sVarValue = GetLineVariableText(playObject, sVarValue);
                }
            }
            else
            {
                nVarValue = HUtil32.StrToInt(questActionInfo.sParam4, 0);
            }
            var cMethod = sMethod[0];
            var dynamicVarList = GetDynamicVarMap(playObject, sType, ref sName);
            if (dynamicVarList == null)
            {
                Dispose(dynamicVar);
                ScriptActionError(playObject, Format(sVarTypeError, sType), questActionInfo, ScriptConst.sSC_CALCVAR);
                return;
            }

            if (playObject.DynamicVarMap.TryGetValue(sVarName, out dynamicVar))
            {
                switch (dynamicVar.VarType)
                {
                    case VarType.Integer:
                        switch (cMethod)
                        {
                            case '=':
                                dynamicVar.nInternet = nVarValue;
                                break;
                            case '+':
                                dynamicVar.nInternet = dynamicVar.nInternet + nVarValue;
                                break;
                            case '-':
                                dynamicVar.nInternet = dynamicVar.nInternet - nVarValue;
                                break;
                            case '*':
                                dynamicVar.nInternet = dynamicVar.nInternet * nVarValue;
                                break;
                            case '/':
                                dynamicVar.nInternet = dynamicVar.nInternet / nVarValue;
                                break;
                        }
                        break;
                    case VarType.String:
                        switch (cMethod)
                        {
                            case '=':
                                dynamicVar.sString = sVarValue;
                                break;
                            case '+':
                                dynamicVar.sString = dynamicVar.sString + sVarValue;
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
                ScriptActionError(playObject, Format(sVarFound, sVarName, sType), questActionInfo, ScriptConst.sSC_CALCVAR);
            }
        }

        private static void ActionOfGuildRecall(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            if (playObject.MyGuild != null)
            {
                // PlayObject.GuildRecall('GuildRecall','');
            }
        }

        private static void ActionOfGroupAddList(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            var ffile = questActionInfo.sParam1;
            if (playObject.GroupOwner != 0)
            {
                for (var i = 0; i < playObject.GroupMembers.Count; i++)
                {
                    playObject = playObject.GroupMembers[i];
                    // AddListEx(PlayObject.m_sChrName,ffile);
                }
            }
        }

        private static void ActionOfGroupRecall(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            if (playObject.GroupOwner != 0)
            {
                // PlayObject.GroupRecall('GroupRecall');
            }
        }

        /// <summary>
        /// 特修身上所有装备
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="questActionInfo"></param>
        private void ActionOfRepairAllItem(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            var boIsHasItem = false;
            for (var i = 0; i < playObject.UseItems.Length; i++)
            {
                if (playObject.UseItems[i].Index <= 0)
                {
                    continue;
                }
                var sUserItemName = M2Share.WorldEngine.GetStdItemName(playObject.UseItems[i].Index);
                if (!(i != ItemLocation.Charm))
                {
                    playObject.SysMsg(sUserItemName + " 禁止修理...", MsgColor.Red, MsgType.Hint);
                    continue;
                }
                playObject.UseItems[i].Dura = playObject.UseItems[i].DuraMax;
                playObject.SendMsg(this, Messages.RM_DURACHANGE, (short)i, playObject.UseItems[i].Dura, playObject.UseItems[i].DuraMax, 0);
                boIsHasItem = true;
            }
            if (boIsHasItem)
            {
                playObject.SysMsg("您身上的的装备修复成功了...", MsgColor.Blue, MsgType.Hint);
            }
        }

        private void ActionOfGroupMoveMap(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            var boFlag = false;
            if (playObject.GroupOwner != 0)
            {
                var envir = M2Share.MapMgr.FindMap(questActionInfo.sParam1);
                if (envir != null)
                {
                    if (envir.CanWalk(questActionInfo.nParam2, questActionInfo.nParam3, true))
                    {
                        for (var i = 0; i < playObject.GroupMembers.Count; i++)
                        {
                            var playObjectEx = playObject.GroupMembers[i];
                            playObjectEx.SpaceMove(questActionInfo.sParam1, (short)questActionInfo.nParam2, (short)questActionInfo.nParam3, 0);
                        }
                        boFlag = true;
                    }
                }
            }
            if (!boFlag)
            {
                ScriptActionError(playObject, "", questActionInfo, ScriptConst.sSC_GROUPMOVEMAP);
            }
        }

        private void ActionOfUpgradeItems(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            int nAddPoint;
            var nWhere = HUtil32.StrToInt(questActionInfo.sParam1, -1);
            var nRate = HUtil32.StrToInt(questActionInfo.sParam2, -1);
            var nPoint = HUtil32.StrToInt(questActionInfo.sParam3, -1);
            if (nWhere < 0 || nWhere > playObject.UseItems.Length || nRate < 0 || nPoint < 0 || nPoint > 255)
            {
                ScriptActionError(playObject, "", questActionInfo, ScriptConst.sSC_UPGRADEITEMS);
                return;
            }
            var userItem = playObject.UseItems[nWhere];
            var stdItem = M2Share.WorldEngine.GetStdItem(userItem.Index);
            if (userItem.Index <= 0 || stdItem == null)
            {
                playObject.SysMsg("你身上没有戴指定物品!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            nRate = M2Share.RandomNumber.Random(nRate);
            nPoint = M2Share.RandomNumber.Random(nPoint);
            var nValType = M2Share.RandomNumber.Random(14);
            if (nRate != 0)
            {
                playObject.SysMsg("装备升级失败!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            if (nValType == 14)
            {
                nAddPoint = nPoint * 1000;
                if (userItem.DuraMax + nAddPoint > ushort.MaxValue)
                {
                    nAddPoint = ushort.MaxValue - userItem.DuraMax;
                }
                userItem.DuraMax = (ushort)(userItem.DuraMax + nAddPoint);
            }
            else
            {
                nAddPoint = nPoint;
                if (userItem.Desc[nValType] + nAddPoint > byte.MaxValue)
                {
                    nAddPoint = byte.MaxValue - userItem.Desc[nValType];
                }
                userItem.Desc[nValType] = (byte)(userItem.Desc[nValType] + nAddPoint);
            }
            playObject.SendUpdateItem(userItem);
            playObject.SysMsg("装备升级成功", MsgColor.Green, MsgType.Hint);
            playObject.SysMsg(stdItem.Name + ": " + userItem.Dura + '/' + userItem.DuraMax + '/' + userItem.Desc[0] + '/' + userItem.Desc[1] + '/' + userItem.Desc[2] + '/' + userItem.Desc[3] + '/' + userItem.Desc[4] + '/' + userItem.Desc[5] + '/' + userItem.Desc[6] + '/' + userItem.Desc[7] + '/' + userItem.Desc[8] + '/' + userItem.Desc[9] + '/' + userItem.Desc[ItemAttr.WeaponUpgrade] + '/' + userItem.Desc[11] + '/' + userItem.Desc[12] + '/' + userItem.Desc[13], MsgColor.Blue, MsgType.Hint);
        }

        private void ActionOfUpgradeItemsEx(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            int nAddPoint;
            var nWhere = HUtil32.StrToInt(questActionInfo.sParam1, -1);
            var nValType = HUtil32.StrToInt(questActionInfo.sParam2, -1);
            var nRate = HUtil32.StrToInt(questActionInfo.sParam3, -1);
            var nPoint = HUtil32.StrToInt(questActionInfo.sParam4, -1);
            var nUpgradeItemStatus = HUtil32.StrToInt(questActionInfo.sParam5, -1);
            if (nValType < 0 || nValType > 14 || nWhere < 0 || nWhere > playObject.UseItems.Length || nRate < 0 || nPoint < 0 || nPoint > 255)
            {
                ScriptActionError(playObject, "", questActionInfo, ScriptConst.sSC_UPGRADEITEMSEX);
                return;
            }
            var userItem = playObject.UseItems[nWhere];
            var stdItem = M2Share.WorldEngine.GetStdItem(userItem.Index);
            if (userItem.Index <= 0 || stdItem == null)
            {
                playObject.SysMsg("你身上没有戴指定物品!!!", MsgColor.Red, MsgType.Hint);
                return;
            }
            var nRatePoint = M2Share.RandomNumber.Random(nRate * 10);
            nPoint = HUtil32._MAX(1, M2Share.RandomNumber.Random(nPoint));
            if (!(nRatePoint >= 0 && nRatePoint <= 10))
            {
                switch (nUpgradeItemStatus)
                {
                    case 0:
                        playObject.SysMsg("装备升级未成功!!!", MsgColor.Red, MsgType.Hint);
                        break;
                    case 1:
                        playObject.SendDelItems(userItem);
                        userItem.Index = 0;
                        playObject.SysMsg("装备破碎!!!", MsgColor.Red, MsgType.Hint);
                        break;
                    case 2:
                        playObject.SysMsg("装备升级失败，装备属性恢复默认!!!", MsgColor.Red, MsgType.Hint);
                        if (nValType != 14)
                        {
                            userItem.Desc[nValType] = 0;
                        }
                        break;
                }
                return;
            }
            if (nValType == 14)
            {
                nAddPoint = nPoint * 1000;
                if (userItem.DuraMax + nAddPoint > ushort.MaxValue)
                {
                    nAddPoint = ushort.MaxValue - userItem.DuraMax;
                }
                userItem.DuraMax = (ushort)(userItem.DuraMax + nAddPoint);
            }
            else
            {
                nAddPoint = nPoint;
                if (userItem.Desc[nValType] + nAddPoint > byte.MaxValue)
                {
                    nAddPoint = byte.MaxValue - userItem.Desc[nValType];
                }
                userItem.Desc[nValType] = (byte)(userItem.Desc[nValType] + nAddPoint);
            }
            playObject.SendUpdateItem(userItem);
            playObject.SysMsg("装备升级成功", MsgColor.Green, MsgType.Hint);
            playObject.SysMsg(stdItem.Name + ": " + userItem.Dura + '/' + userItem.DuraMax + '-' + userItem.Desc[0] + '/' + userItem.Desc[1] + '/' + userItem.Desc[2] + '/' + userItem.Desc[3] + '/' + userItem.Desc[4] + '/' + userItem.Desc[5] + '/' + userItem.Desc[6] + '/' + userItem.Desc[7] + '/' + userItem.Desc[8] + '/' + userItem.Desc[9] + '/' + userItem.Desc[ItemAttr.WeaponUpgrade] + '/' + userItem.Desc[11] + '/' + userItem.Desc[12] + '/' + userItem.Desc[13], MsgColor.Blue, MsgType.Hint);
        }

        /// <summary>
        /// 声明变量
        /// VAR 数据类型(Integer String) 类型(HUMAN GUILD GLOBAL) 变量值
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="questActionInfo"></param>
        private void ActionOfVar(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            var sName = string.Empty;
            const string sVarFound = "变量{0}已存在，变量类型:{1}";
            const string sVarTypeError = "变量类型错误，错误类型:{0} 当前支持类型(HUMAN、GUILD、GLOBAL)";
            var sType = questActionInfo.sParam2;
            var sVarName = questActionInfo.sParam3;
            var sVarValue = questActionInfo.sParam4;
            var nVarValue = HUtil32.StrToInt(questActionInfo.sParam4, 0);
            var varType = VarType.None;
            if (string.Compare(questActionInfo.sParam1, "Integer", StringComparison.OrdinalIgnoreCase) == 0)
            {
                varType = VarType.Integer;
            }
            if (string.Compare(questActionInfo.sParam1, "String", StringComparison.OrdinalIgnoreCase) == 0)
            {
                varType = VarType.String;
            }
            if (string.IsNullOrEmpty(sType) || string.IsNullOrEmpty(sVarName) || varType == VarType.None)
            {
                ScriptActionError(playObject, "", questActionInfo, ScriptConst.sSC_VAR);
                return;
            }
            if (string.Compare(questActionInfo.sParam1, "Integer", StringComparison.OrdinalIgnoreCase) == 0)
            {
                varType = VarType.Integer;
            }
            if (string.Compare(questActionInfo.sParam1, "String", StringComparison.OrdinalIgnoreCase) == 0)
            {
                varType = VarType.String;
            }
            if (string.IsNullOrEmpty(sType) || string.IsNullOrEmpty(sVarName) || varType == VarType.None)
            {
                ScriptActionError(playObject, "", questActionInfo, ScriptConst.sSC_VAR);
                return;
            }
            var dynamicVar = new DynamicVar();
            dynamicVar.sName = sVarName;
            dynamicVar.VarType = varType;
            dynamicVar.nInternet = nVarValue;
            dynamicVar.sString = sVarValue;
            var boFoundVar = false;
            var dynamicVarList = GetDynamicVarMap(playObject, sType, ref sName);
            if (dynamicVarList == null)
            {
                Dispose(dynamicVar);
                ScriptActionError(playObject, Format(sVarTypeError, sType), questActionInfo, ScriptConst.sSC_VAR);
                return;
            }
            if (dynamicVarList.ContainsKey(sVarName))
            {
                boFoundVar = true;
            }
            if (!boFoundVar)
            {
                dynamicVarList.Add(sVarName, dynamicVar);
            }
            else
            {
                ScriptActionError(playObject, Format(sVarFound, sVarName, sType), questActionInfo, ScriptConst.sSC_VAR);
            }
        }

        /// <summary>
        /// 读取变量值
        /// LOADVAR 变量类型 变量名 文件名
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="questActionInfo"></param>
        private void ActionOfLoadVar(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            var sName = string.Empty;
            const string sVarFound = "变量{0}不存在，变量类型:{1}";
            const string sVarTypeError = "变量类型错误，错误类型:{0} 当前支持类型(HUMAN、GUILD、GLOBAL)";
            var sType = questActionInfo.sParam1;
            var sVarName = questActionInfo.sParam2;
            var sFileName = GetLineVariableText(playObject, questActionInfo.sParam3);
            if (sFileName[0] == '\\')
            {
                sFileName = sFileName[1..];
            }
            if (sFileName[1] == '\\')
            {
                sFileName = sFileName[2..];
            }
            if (sFileName[2] == '\\')
            {
                sFileName = sFileName[3..];
            }
            sFileName = M2Share.GetEnvirFilePath(sFileName);
            if (string.IsNullOrEmpty(sType) || string.IsNullOrEmpty(sVarName) || !File.Exists(sFileName))
            {
                ScriptActionError(playObject, "", questActionInfo, ScriptConst.sSC_LOADVAR);
                return;
            }
            var boFoundVar = false;
            var dynamicVarList = GetDynamicVarMap(playObject, sType, ref sName);
            if (dynamicVarList == null)
            {
                ScriptActionError(playObject, Format(sVarTypeError, sType), questActionInfo, ScriptConst.sSC_VAR);
                return;
            }
            if (dynamicVarList.TryGetValue(sVarName, out _))
            {
                //IniFile = new ConfFile(sFileName);
                //IniFile.Load();
                /*switch (DynamicVar.VarType)
                {
                    case TVarType.Integer:
                        DynamicVar.nInternet = IniFile.ReadWriteInteger(sName, DynamicVar.sName, 0);
                        break;
                    case TVarType.String:
                        DynamicVar.sString = IniFile.ReadWriteString(sName, DynamicVar.sName, "");
                        break;
                }*/
                boFoundVar = true;
            }
            else
            {
                var iniFile = new ConfFile(sFileName);
                iniFile.Load();
                var str = iniFile.ReadString(sName, sVarName, "");
                if (!string.IsNullOrEmpty(str))
                {
                    if (!dynamicVarList.ContainsKey(sVarName))
                    {
                        dynamicVarList.Add(sVarName, new DynamicVar()
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
                ScriptActionError(playObject, Format(sVarFound, sVarName, sType), questActionInfo, ScriptConst.sSC_LOADVAR);
            }
        }

        private void ActionOfClearNeedItems(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            var nNeed = HUtil32.StrToInt(questActionInfo.sParam1, -1);
            if (nNeed < 0)
            {
                ScriptActionError(playObject, "", questActionInfo, ScriptConst.sSC_CLEARNEEDITEMS);
                return;
            }
            StdItem stdItem;
            UserItem userItem;
            for (var i = playObject.ItemList.Count - 1; i >= 0; i--)
            {
                userItem = playObject.ItemList[i];
                stdItem = M2Share.WorldEngine.GetStdItem(userItem.Index);
                if (stdItem != null && stdItem.Need == nNeed)
                {
                    playObject.SendDelItems(userItem);
                    Dispose(userItem);
                    playObject.ItemList.RemoveAt(i);
                }
            }
            for (var i = playObject.StorageItemList.Count - 1; i >= 0; i--)
            {
                userItem = playObject.StorageItemList[i];
                stdItem = M2Share.WorldEngine.GetStdItem(userItem.Index);
                if (stdItem != null && stdItem.Need == nNeed)
                {
                    Dispose(userItem);
                    playObject.StorageItemList.RemoveAt(i);
                }
            }
        }

        private void ActionOfClearMakeItems(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            UserItem userItem;
            var sItemName = questActionInfo.sParam1;
            var nMakeIndex = questActionInfo.nParam2;
            var boMatchName = questActionInfo.sParam3 == "1";
            if (string.IsNullOrEmpty(sItemName) || nMakeIndex <= 0)
            {
                ScriptActionError(playObject, "", questActionInfo, ScriptConst.sSC_CLEARMAKEITEMS);
                return;
            }
            StdItem stdItem;
            for (var i = playObject.ItemList.Count - 1; i >= 0; i--)
            {
                userItem = playObject.ItemList[i];
                if (userItem.MakeIndex != nMakeIndex)
                {
                    continue;
                }
                stdItem = M2Share.WorldEngine.GetStdItem(userItem.Index);
                if (!boMatchName || stdItem != null && string.Compare(stdItem.Name, sItemName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    playObject.SendDelItems(userItem);
                    Dispose(userItem);
                    playObject.ItemList.RemoveAt(i);
                }
            }
            for (var i = playObject.StorageItemList.Count - 1; i >= 0; i--)
            {
                userItem = playObject.ItemList[i];
                if (userItem.MakeIndex != nMakeIndex)
                {
                    continue;
                }
                stdItem = M2Share.WorldEngine.GetStdItem(userItem.Index);
                if (!boMatchName || stdItem != null && string.Compare(stdItem.Name, sItemName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    Dispose(userItem);
                    playObject.StorageItemList.RemoveAt(i);
                }
            }
            for (var i = 0; i < playObject.UseItems.Length; i++)
            {
                userItem = playObject.UseItems[i];
                if (userItem.MakeIndex != nMakeIndex)
                {
                    continue;
                }
                stdItem = M2Share.WorldEngine.GetStdItem(userItem.Index);
                if (!boMatchName || stdItem != null && string.Compare(stdItem.Name, sItemName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    userItem.Index = 0;
                }
            }
        }

        private void ActionOfUnMaster(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            if (string.IsNullOrEmpty(playObject.MasterName))
            {
                GotoLable(playObject, "@ExeMasterFail", false);
                return;
            }
            var poseHuman = (PlayObject)playObject.GetPoseCreate();
            if (poseHuman == null)
            {
                GotoLable(playObject, "@UnMasterCheckDir", false);
            }
            if (poseHuman != null)
            {
                if ((string.IsNullOrEmpty(questActionInfo.sParam1)))
                {
                    if (poseHuman.Race != ActorRace.Play)
                    {
                        GotoLable(playObject, "@UnMasterTypeErr", false);
                        return;
                    }
                    if (poseHuman.GetPoseCreate() == playObject)
                    {
                        if (playObject.MasterName == poseHuman.ChrName)
                        {
                            if (playObject.IsMaster)
                            {
                                GotoLable(playObject, "@UnIsMaster", false);
                                return;
                            }
                            if (playObject.MasterName != poseHuman.ChrName)
                            {
                                GotoLable(playObject, "@UnMasterError", false);
                                return;
                            }
                            GotoLable(playObject, "@StartUnMaster", false);
                            GotoLable(poseHuman, "@WateUnMaster", false);
                            return;
                        }
                    }
                }
            }
            // sREQUESTUNMARRY
            if (string.Compare(questActionInfo.sParam1, "REQUESTUNMASTER", StringComparison.OrdinalIgnoreCase) == 0)
            {
                string sMsg;
                if (string.IsNullOrEmpty(questActionInfo.sParam2))
                {
                    if (poseHuman != null)
                    {
                        playObject.IsStartUnMaster = true;
                        if (playObject.IsStartUnMaster && poseHuman.IsStartUnMaster)
                        {
                            sMsg = string.Format(Settings.NPCSayUnMasterOKMsg, ChrName, playObject.ChrName, poseHuman.ChrName);
                            M2Share.WorldEngine.SendBroadCastMsg(sMsg, MsgType.Say);
                            playObject.MasterName = "";
                            poseHuman.MasterName = "";
                            playObject.IsStartUnMaster = false;
                            poseHuman.IsStartUnMaster = false;
                            playObject.RefShowName();
                            poseHuman.RefShowName();
                            GotoLable(playObject, "@UnMasterEnd", false);
                            GotoLable(poseHuman, "@UnMasterEnd", false);
                        }
                        else
                        {
                            GotoLable(playObject, "@WateUnMaster", false);
                            GotoLable(poseHuman, "@RevUnMaster", false);
                        }
                    }
                    return;
                }
                // 强行出师
                if (string.Compare(questActionInfo.sParam2, "FORCE", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    sMsg = string.Format(Settings.NPCSayForceUnMasterMsg, ChrName, playObject.ChrName, playObject.MasterName);
                    M2Share.WorldEngine.SendBroadCastMsg(sMsg, MsgType.Say);
                    poseHuman = M2Share.WorldEngine.GetPlayObject(playObject.MasterName);
                    if (poseHuman != null)
                    {
                        poseHuman.MasterName = "";
                        poseHuman.RefShowName();
                    }
                    else
                    {
                        M2Share.UnForceMasterList.Add(playObject.MasterName);
                        M2Share.SaveUnForceMasterList();
                    }
                    playObject.MasterName = "";
                    GotoLable(playObject, "@UnMasterEnd", false);
                    playObject.RefShowName();
                }
            }
        }

        private void ActionOfSetMapMode(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            var sMapName = questActionInfo.sParam1;
            var sMapMode = questActionInfo.sParam2;
            var sParam1 = questActionInfo.sParam3;
            var sParam2 = questActionInfo.sParam4;
            var envir = M2Share.MapMgr.FindMap(sMapName);
            if (envir == null || string.IsNullOrEmpty(sMapMode))
            {
                ScriptActionError(playObject, "", questActionInfo, ScriptConst.sSC_SETMAPMODE);
                return;
            }
            if (string.Compare(sMapMode, "SAFE", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (!string.IsNullOrEmpty(sParam1))
                {
                    envir.Flag.SafeArea = true;
                }
                else
                {
                    envir.Flag.SafeArea = false;
                }
            }
            else if (string.Compare(sMapMode, "DARK", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (!string.IsNullOrEmpty(sParam1))
                {
                    envir.Flag.boDarkness = true;
                }
                else
                {
                    envir.Flag.boDarkness = false;
                }
            }
            else if (string.Compare(sMapMode, "FIGHT", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (!string.IsNullOrEmpty(sParam1))
                {
                    envir.Flag.FightZone = true;
                }
                else
                {
                    envir.Flag.FightZone = false;
                }
            }
            else if (string.Compare(sMapMode, "FIGHT3", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (!string.IsNullOrEmpty(sParam1))
                {
                    envir.Flag.Fight3Zone = true;
                }
                else
                {
                    envir.Flag.Fight3Zone = false;
                }
            }
            else if (string.Compare(sMapMode, "DAY", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (!string.IsNullOrEmpty(sParam1))
                {
                    envir.Flag.DayLight = true;
                }
                else
                {
                    envir.Flag.DayLight = false;
                }
            }
            else if (string.Compare(sMapMode, "QUIZ", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (!string.IsNullOrEmpty(sParam1))
                {
                    envir.Flag.boQUIZ = true;
                }
                else
                {
                    envir.Flag.boQUIZ = false;
                }
            }
            else if (string.Compare(sMapMode, "NORECONNECT", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (!string.IsNullOrEmpty(sParam1))
                {
                    envir.Flag.boNORECONNECT = true;
                    envir.Flag.sNoReConnectMap = sParam1;
                }
                else
                {
                    envir.Flag.boNORECONNECT = false;
                }
            }
            else if (string.Compare(sMapMode, "MUSIC", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (!string.IsNullOrEmpty(sParam1))
                {
                    envir.Flag.Music = true;
                    envir.Flag.MusicId = HUtil32.StrToInt(sParam1, -1);
                }
                else
                {
                    envir.Flag.Music = false;
                }
            }
            else if (string.Compare(sMapMode, "EXPRATE", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (!string.IsNullOrEmpty(sParam1))
                {
                    envir.Flag.boEXPRATE = true;
                    envir.Flag.ExpRate = HUtil32.StrToInt(sParam1, -1);
                }
                else
                {
                    envir.Flag.boEXPRATE = false;
                }
            }
            else if (string.Compare(sMapMode, "PKWINLEVEL", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (!string.IsNullOrEmpty(sParam1))
                {
                    envir.Flag.boPKWINLEVEL = true;
                    envir.Flag.nPKWINLEVEL = HUtil32.StrToInt(sParam1, -1);
                }
                else
                {
                    envir.Flag.boPKWINLEVEL = false;
                }
            }
            else if (string.Compare(sMapMode, "PKWINEXP", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (!string.IsNullOrEmpty(sParam1))
                {
                    envir.Flag.boPKWINEXP = true;
                    envir.Flag.nPKWINEXP = HUtil32.StrToInt(sParam1, -1);
                }
                else
                {
                    envir.Flag.boPKWINEXP = false;
                }
            }
            else if (string.Compare(sMapMode, "PKLOSTLEVEL", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (!string.IsNullOrEmpty(sParam1))
                {
                    envir.Flag.boPKLOSTLEVEL = true;
                    envir.Flag.nPKLOSTLEVEL = HUtil32.StrToInt(sParam1, -1);
                }
                else
                {
                    envir.Flag.boPKLOSTLEVEL = false;
                }
            }
            else if (string.Compare(sMapMode, "PKLOSTEXP", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (!string.IsNullOrEmpty(sParam1))
                {
                    envir.Flag.boPKLOSTEXP = true;
                    envir.Flag.nPKLOSTEXP = HUtil32.StrToInt(sParam1, -1);
                }
                else
                {
                    envir.Flag.boPKLOSTEXP = false;
                }
            }
            else if (string.Compare(sMapMode, "DECHP", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if ((!string.IsNullOrEmpty(sParam1)) && (!string.IsNullOrEmpty(sParam2)))
                {
                    envir.Flag.boDECHP = true;
                    envir.Flag.nDECHPTIME = HUtil32.StrToInt(sParam1, -1);
                    envir.Flag.nDECHPPOINT = HUtil32.StrToInt(sParam2, -1);
                }
                else
                {
                    envir.Flag.boDECHP = false;
                }
            }
            else if (string.Compare(sMapMode, "DECGAMEGOLD", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if ((!string.IsNullOrEmpty(sParam1)) && (!string.IsNullOrEmpty(sParam2)))
                {
                    envir.Flag.boDECGAMEGOLD = true;
                    envir.Flag.nDECGAMEGOLDTIME = HUtil32.StrToInt(sParam1, -1);
                    envir.Flag.nDECGAMEGOLD = HUtil32.StrToInt(sParam2, -1);
                }
                else
                {
                    envir.Flag.boDECGAMEGOLD = false;
                }
            }
            else if (string.Compare(sMapMode, "RUNHUMAN", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (!string.IsNullOrEmpty(sParam1))
                {
                    envir.Flag.RunHuman = true;
                }
                else
                {
                    envir.Flag.RunHuman = false;
                }
            }
            else if (string.Compare(sMapMode, "RUNMON", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (!string.IsNullOrEmpty(sParam1))
                {
                    envir.Flag.RunMon = true;
                }
                else
                {
                    envir.Flag.RunMon = false;
                }
            }
            else if (string.Compare(sMapMode, "NEEDHOLE", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (!string.IsNullOrEmpty(sParam1))
                {
                    envir.Flag.boNEEDHOLE = true;
                }
                else
                {
                    envir.Flag.boNEEDHOLE = false;
                }
            }
            else if (string.Compare(sMapMode, "NORECALL", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (!string.IsNullOrEmpty(sParam1))
                {
                    envir.Flag.NoReCall = true;
                }
                else
                {
                    envir.Flag.NoReCall = false;
                }
            }
            else if (string.Compare(sMapMode, "NOGUILDRECALL", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (!string.IsNullOrEmpty(sParam1))
                {
                    envir.Flag.NoGuildReCall = true;
                }
                else
                {
                    envir.Flag.NoGuildReCall = false;
                }
            }
            else if (string.Compare(sMapMode, "NODEARRECALL", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (!string.IsNullOrEmpty(sParam1))
                {
                    envir.Flag.boNODEARRECALL = true;
                }
                else
                {
                    envir.Flag.boNODEARRECALL = false;
                }
            }
            else if (string.Compare(sMapMode, "NOMASTERRECALL", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (!string.IsNullOrEmpty(sParam1))
                {
                    envir.Flag.MasterReCall = true;
                }
                else
                {
                    envir.Flag.MasterReCall = false;
                }
            }
            else if (string.Compare(sMapMode, "NORANDOMMOVE", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (!string.IsNullOrEmpty(sParam1))
                {
                    envir.Flag.boNORANDOMMOVE = true;
                }
                else
                {
                    envir.Flag.boNORANDOMMOVE = false;
                }
            }
            else if (string.Compare(sMapMode, "NODRUG", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (!string.IsNullOrEmpty(sParam1))
                {
                    envir.Flag.boNODRUG = true;
                }
                else
                {
                    envir.Flag.boNODRUG = false;
                }
            }
            else if (string.Compare(sMapMode, "MINE", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (!string.IsNullOrEmpty(sParam1))
                {
                    envir.Flag.Mine = true;
                }
                else
                {
                    envir.Flag.Mine = false;
                }
            }
            else if (string.Compare(sMapMode, "MINE2", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (!string.IsNullOrEmpty(sParam1))
                {
                    envir.Flag.boMINE2 = true;
                }
                else
                {
                    envir.Flag.boMINE2 = false;
                }
            }
            else if (string.Compare(sMapMode, "NOTHROWITEM", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (!string.IsNullOrEmpty(sParam1))
                {
                    envir.Flag.NoThrowItem = true;
                }
                else
                {
                    envir.Flag.NoThrowItem = false;
                }
            }
            else if (string.Compare(sMapMode, "NODROPITEM", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (!string.IsNullOrEmpty(sParam1))
                {
                    envir.Flag.NoDropItem = true;
                }
                else
                {
                    envir.Flag.NoDropItem = false;
                }
            }
            else if (string.Compare(sMapMode, "NOPOSITIONMOVE", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (!string.IsNullOrEmpty(sParam1))
                {
                    envir.Flag.boNOPOSITIONMOVE = true;
                }
                else
                {
                    envir.Flag.boNOPOSITIONMOVE = false;
                }
            }
            else if (string.Compare(sMapMode, "NOHORSE", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (!string.IsNullOrEmpty(sParam1))
                {
                    envir.Flag.NoHorse = true;
                }
                else
                {
                    envir.Flag.NoHorse = false;
                }
            }
            else if (string.Compare(sMapMode, "NOCHAT", StringComparison.OrdinalIgnoreCase) == 0)
            {
                if (!string.IsNullOrEmpty(sParam1))
                {
                    envir.Flag.boNOCHAT = true;
                }
                else
                {
                    envir.Flag.boNOCHAT = false;
                }
            }
        }

        private void ActionOfSetMemberLevel(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            var nLevel = (byte)HUtil32.StrToInt(questActionInfo.sParam2, -1);
            if (nLevel < 0)
            {
                ScriptActionError(playObject, "", questActionInfo, ScriptConst.sSC_SETMEMBERLEVEL);
                return;
            }
            var cMethod = questActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    playObject.MemberLevel = nLevel;
                    break;
                case '-':
                    playObject.MemberLevel -= nLevel;
                    if (playObject.MemberLevel <= 0)
                    {
                        playObject.MemberLevel = 0;
                    }
                    break;
                case '+':
                    playObject.MemberLevel += nLevel;
                    if (playObject.MemberLevel >= byte.MaxValue)
                    {
                        playObject.MemberLevel = 255;
                    }
                    break;
            }
            if (M2Share.Config.ShowScriptActionMsg)
            {
                playObject.SysMsg(Format(Settings.ChangeMemberLevelMsg, playObject.MemberLevel), MsgColor.Green, MsgType.Hint);
            }
        }

        private void ActionOfSetMemberType(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            var nType = HUtil32.StrToInt(questActionInfo.sParam2, -1);
            if (nType < 0)
            {
                ScriptActionError(playObject, "", questActionInfo, ScriptConst.sSC_SETMEMBERTYPE);
                return;
            }
            var cMethod = questActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    playObject.MemberType = nType;
                    break;
                case '-':
                    playObject.MemberType -= nType;
                    if (playObject.MemberType < 0)
                    {
                        playObject.MemberType = 0;
                    }
                    break;
                case '+':
                    playObject.MemberType += nType;
                    if (playObject.MemberType > 65535)
                    {
                        playObject.MemberType = 65535;
                    }
                    break;
            }
            if (M2Share.Config.ShowScriptActionMsg)
            {
                playObject.SysMsg(Format(Settings.ChangeMemberTypeMsg, playObject.MemberType), MsgColor.Green, MsgType.Hint);
            }
        }

        private void ActionOfGiveItem(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            var sItemName = questActionInfo.sParam1;
            var nItemCount = questActionInfo.nParam2;
            if (string.IsNullOrEmpty(sItemName) || nItemCount <= 0)
            {
                ScriptActionError(playObject, "", questActionInfo, ScriptConst.sSC_GIVE);
                return;
            }
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
                for (var i = 0; i < nItemCount; i++)
                {
                    StdItem stdItem;
                    // nItemCount 为0时出死循环
                    UserItem userItem;
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

        private static void ActionOfGmExecute(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            var sParam1 = questActionInfo.sParam1;
            var sParam2 = questActionInfo.sParam2;
            var sParam3 = questActionInfo.sParam3;
            var sParam4 = questActionInfo.sParam4;
            var sParam5 = questActionInfo.sParam5;
            var sParam6 = questActionInfo.sParam6;
            if (string.Compare(sParam2, "Self", StringComparison.OrdinalIgnoreCase) == 0)
            {
                sParam2 = playObject.ChrName;
            }
            var sData = Format("@{0} {1} {2} {3} {4} {5}", sParam1, sParam2, sParam3, sParam4, sParam5, sParam6);
            var btOldPermission = playObject.Permission;
            try
            {
                playObject.Permission = 10;
                playObject.ProcessUserLineMsg(sData);
            }
            finally
            {
                playObject.Permission = btOldPermission;
            }
        }

        private void ActionOfGuildAuraePoint(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            var nAuraePoint = HUtil32.StrToInt(questActionInfo.sParam2, -1);
            if (nAuraePoint < 0)
            {
                ScriptActionError(playObject, "", questActionInfo, ScriptConst.sSC_AURAEPOINT);
                return;
            }
            if (playObject.MyGuild == null)
            {
                playObject.SysMsg(Settings.ScriptGuildAuraePointNoGuild, MsgColor.Red, MsgType.Hint);
                return;
            }
            var guild = playObject.MyGuild;
            var cMethod = questActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    guild.Aurae = nAuraePoint;
                    break;
                case '-':
                    if (guild.Aurae >= nAuraePoint)
                    {
                        guild.Aurae = guild.Aurae - nAuraePoint;
                    }
                    else
                    {
                        guild.Aurae = 0;
                    }
                    break;
                case '+':
                    if (int.MaxValue - guild.Aurae >= nAuraePoint)
                    {
                        guild.Aurae = guild.Aurae + nAuraePoint;
                    }
                    else
                    {
                        guild.Aurae = int.MaxValue;
                    }
                    break;
            }
            if (M2Share.Config.ShowScriptActionMsg)
            {
                playObject.SysMsg(Format(Settings.ScriptGuildAuraePointMsg, new[] { guild.Aurae }), MsgColor.Green, MsgType.Hint);
            }
        }

        private void ActionOfGuildBuildPoint(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            var nBuildPoint = HUtil32.StrToInt(questActionInfo.sParam2, -1);
            if (nBuildPoint < 0)
            {
                ScriptActionError(playObject, "", questActionInfo, ScriptConst.sSC_BUILDPOINT);
                return;
            }
            if (playObject.MyGuild == null)
            {
                playObject.SysMsg(Settings.ScriptGuildBuildPointNoGuild, MsgColor.Red, MsgType.Hint);
                return;
            }
            var guild = playObject.MyGuild;
            var cMethod = questActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    guild.BuildPoint = nBuildPoint;
                    break;
                case '-':
                    if (guild.BuildPoint >= nBuildPoint)
                    {
                        guild.BuildPoint = guild.BuildPoint - nBuildPoint;
                    }
                    else
                    {
                        guild.BuildPoint = 0;
                    }
                    break;
                case '+':
                    if (int.MaxValue - guild.BuildPoint >= nBuildPoint)
                    {
                        guild.BuildPoint = guild.BuildPoint + nBuildPoint;
                    }
                    else
                    {
                        guild.BuildPoint = int.MaxValue;
                    }
                    break;
            }
            if (M2Share.Config.ShowScriptActionMsg)
            {
                playObject.SysMsg(Format(Settings.ScriptGuildBuildPointMsg, guild.BuildPoint), MsgColor.Green, MsgType.Hint);
            }
        }

        private void ActionOfGuildChiefItemCount(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            var nItemCount = HUtil32.StrToInt(questActionInfo.sParam2, -1);
            if (nItemCount < 0)
            {
                ScriptActionError(playObject, "", questActionInfo, ScriptConst.sSC_GUILDCHIEFITEMCOUNT);
                return;
            }
            if (playObject.MyGuild == null)
            {
                playObject.SysMsg(Settings.ScriptGuildFlourishPointNoGuild, MsgColor.Red, MsgType.Hint);
                return;
            }
            var guild = playObject.MyGuild;
            var cMethod = questActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    guild.ChiefItemCount = nItemCount;
                    break;
                case '-':
                    if (guild.ChiefItemCount >= nItemCount)
                    {
                        guild.ChiefItemCount = guild.ChiefItemCount - nItemCount;
                    }
                    else
                    {
                        guild.ChiefItemCount = 0;
                    }
                    break;
                case '+':
                    if (int.MaxValue - guild.ChiefItemCount >= nItemCount)
                    {
                        guild.ChiefItemCount = guild.ChiefItemCount + nItemCount;
                    }
                    else
                    {
                        guild.ChiefItemCount = int.MaxValue;
                    }
                    break;
            }
            if (M2Share.Config.ShowScriptActionMsg)
            {
                playObject.SysMsg(Format(Settings.ScriptChiefItemCountMsg, guild.ChiefItemCount), MsgColor.Green, MsgType.Hint);
            }
        }

        private void ActionOfGuildFlourishPoint(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            var nFlourishPoint = HUtil32.StrToInt(questActionInfo.sParam2, -1);
            if (nFlourishPoint < 0)
            {
                ScriptActionError(playObject, "", questActionInfo, ScriptConst.sSC_FLOURISHPOINT);
                return;
            }
            if (playObject.MyGuild == null)
            {
                playObject.SysMsg(Settings.ScriptGuildFlourishPointNoGuild, MsgColor.Red, MsgType.Hint);
                return;
            }
            var guild = playObject.MyGuild;
            var cMethod = questActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    guild.Flourishing = nFlourishPoint;
                    break;
                case '-':
                    if (guild.Flourishing >= nFlourishPoint)
                    {
                        guild.Flourishing = guild.Flourishing - nFlourishPoint;
                    }
                    else
                    {
                        guild.Flourishing = 0;
                    }
                    break;
                case '+':
                    if (int.MaxValue - guild.Flourishing >= nFlourishPoint)
                    {
                        guild.Flourishing = guild.Flourishing + nFlourishPoint;
                    }
                    else
                    {
                        guild.Flourishing = int.MaxValue;
                    }
                    break;
            }
            if (M2Share.Config.ShowScriptActionMsg)
            {
                playObject.SysMsg(Format(Settings.ScriptGuildFlourishPointMsg, guild.Flourishing), MsgColor.Green, MsgType.Hint);
            }
        }

        private void ActionOfGuildstabilityPoint(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            var nStabilityPoint = HUtil32.StrToInt(questActionInfo.sParam2, -1);
            if (nStabilityPoint < 0)
            {
                ScriptActionError(playObject, "", questActionInfo, ScriptConst.sSC_STABILITYPOINT);
                return;
            }
            if (playObject.MyGuild == null)
            {
                playObject.SysMsg(Settings.ScriptGuildStabilityPointNoGuild, MsgColor.Red, MsgType.Hint);
                return;
            }
            var guild = playObject.MyGuild;
            var cMethod = questActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    guild.Stability = nStabilityPoint;
                    break;
                case '-':
                    if (guild.Stability >= nStabilityPoint)
                    {
                        guild.Stability = guild.Stability - nStabilityPoint;
                    }
                    else
                    {
                        guild.Stability = 0;
                    }
                    break;
                case '+':
                    if (int.MaxValue - guild.Stability >= nStabilityPoint)
                    {
                        guild.Stability = guild.Stability + nStabilityPoint;
                    }
                    else
                    {
                        guild.Stability = int.MaxValue;
                    }
                    break;
            }
            if (M2Share.Config.ShowScriptActionMsg)
            {
                playObject.SysMsg(Format(Settings.ScriptGuildStabilityPointMsg, guild.Stability), MsgColor.Green, MsgType.Hint);
            }
        }

        private void ActionOfHumanHp(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            var nHp = HUtil32.StrToInt(questActionInfo.sParam2, -1);
            if (nHp < 0)
            {
                ScriptActionError(playObject, "", questActionInfo, ScriptConst.sSC_HUMANHP);
                return;
            }
            var cMethod = questActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    playObject.WAbil.HP = (ushort)nHp;
                    break;
                case '-':
                    if (playObject.WAbil.HP >= nHp)
                    {
                        playObject.WAbil.HP -= (ushort)nHp;
                    }
                    else
                    {
                        playObject.WAbil.HP = 0;
                    }
                    break;
                case '+':
                    playObject.WAbil.HP += (ushort)nHp;
                    if (playObject.WAbil.HP > playObject.WAbil.MaxHP)
                    {
                        playObject.WAbil.HP = playObject.WAbil.MaxHP;
                    }
                    break;
            }
            if (M2Share.Config.ShowScriptActionMsg)
            {
                playObject.SysMsg(Format(Settings.ScriptChangeHumanHPMsg, playObject.WAbil.MaxHP), MsgColor.Green, MsgType.Hint);
            }
        }

        private void ActionOfHumanMp(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            var nMp = HUtil32.StrToInt(questActionInfo.sParam2, -1);
            if (nMp < 0)
            {
                ScriptActionError(playObject, "", questActionInfo, ScriptConst.sSC_HUMANMP);
                return;
            }
            var cMethod = questActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    playObject.WAbil.MP = (ushort)nMp;
                    break;
                case '-':
                    if (playObject.WAbil.MP >= nMp)
                    {
                        playObject.WAbil.MP -= (ushort)nMp;
                    }
                    else
                    {
                        playObject.WAbil.MP = 0;
                    }
                    break;
                case '+':
                    playObject.WAbil.MP += (ushort)nMp;
                    if (playObject.WAbil.MP > playObject.WAbil.MaxMP)
                    {
                        playObject.WAbil.MP = playObject.WAbil.MaxMP;
                    }
                    break;
            }
            if (M2Share.Config.ShowScriptActionMsg)
            {
                playObject.SysMsg(Format(Settings.ScriptChangeHumanMPMsg, new[] { playObject.WAbil.MaxMP }), MsgColor.Green, MsgType.Hint);
            }
        }

        private static void ActionOfKick(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            playObject.BoKickFlag = true;
        }

        private void ActionOfKill(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            var nMode = HUtil32.StrToInt(questActionInfo.sParam1, -1);
            if (nMode >= 0 && nMode <= 3)
            {
                switch (nMode)
                {
                    case 1:
                        playObject.NoItem = true;
                        playObject.Die();
                        break;
                    case 2:
                        playObject.SetLastHiter(this);
                        playObject.Die();
                        break;
                    case 3:
                        playObject.NoItem = true;
                        playObject.SetLastHiter(this);
                        playObject.Die();
                        break;
                    default:
                        playObject.Die();
                        break;
                }
            }
            else
            {
                ScriptActionError(playObject, "", questActionInfo, ScriptConst.sSC_KILL);
            }
        }

        private void ActionOfBonusPoint(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            var nBonusPoint = HUtil32.StrToInt(questActionInfo.sParam2, -1);
            if (nBonusPoint < 0 || nBonusPoint > 10000)
            {
                ScriptActionError(playObject, "", questActionInfo, ScriptConst.sSC_BONUSPOINT);
                return;
            }
            var cMethod = questActionInfo.sParam1[0];
            switch (cMethod)
            {
                case '=':
                    playObject.HasLevelUp(0);
                    playObject.BonusPoint = nBonusPoint;
                    playObject.SendMsg(Messages.RM_ADJUST_BONUS, 0, 0, 0, 0);
                    break;
                case '-':
                    break;
                case '+':
                    playObject.BonusPoint += nBonusPoint;
                    playObject.SendMsg(Messages.RM_ADJUST_BONUS, 0, 0, 0, 0);
                    break;
            }
        }

        private static void ActionOfDelMarry(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            playObject.DearName = "";
            playObject.RefShowName();
        }

        private static void ActionOfDelMaster(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            playObject.MasterName = "";
            playObject.RefShowName();
        }

        private static void ActionOfRestBonusPoint(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            var nTotleUsePoint = playObject.BonusAbil.DC + playObject.BonusAbil.MC + playObject.BonusAbil.SC + playObject.BonusAbil.AC + playObject.BonusAbil.MAC + playObject.BonusAbil.HP + playObject.BonusAbil.MP + playObject.BonusAbil.Hit + playObject.BonusAbil.Speed + playObject.BonusAbil.Reserved;
            playObject.BonusPoint += nTotleUsePoint;
            playObject.SendMsg(Messages.RM_ADJUST_BONUS, 0, 0, 0, 0);
            playObject.HasLevelUp(0);
            playObject.SysMsg("分配点数已复位!!!", MsgColor.Red, MsgType.Hint);
        }

        private static void ActionOfRestReNewLevel(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            playObject.ReLevel = 0;
            playObject.HasLevelUp(0);
        }

        private void ActionOfChangeNameColor(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            var nColor = questActionInfo.nParam1;
            if (nColor < 0 || nColor > 255)
            {
                ScriptActionError(playObject, "", questActionInfo, ScriptConst.sSC_CHANGENAMECOLOR);
                return;
            }
            playObject.NameColor = (byte)nColor;
            playObject.RefNameColor();
        }

        private static void ActionOfClearPassword(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            playObject.StoragePwd = "";
            playObject.IsPasswordLocked = false;
        }

        // RECALLMOB 怪物名称 等级 叛变时间 变色(0,1) 固定颜色(1 - 7)
        // 变色为0 时固定颜色才起作用
        private static void ActionOfRecallmob(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            BaseObject mon;
            if (questActionInfo.nParam3 <= 1)
            {
                mon = playObject.MakeSlave(questActionInfo.sParam1, 3, HUtil32.StrToInt(questActionInfo.sParam2, 0), 100, 10 * 24 * 60 * 60);
            }
            else
            {
                mon = playObject.MakeSlave(questActionInfo.sParam1, 3, HUtil32.StrToInt(questActionInfo.sParam2, 0), 100, questActionInfo.nParam3 * 60);
            }
            if (mon != null)
            {
                if (!string.IsNullOrEmpty(questActionInfo.sParam4) && questActionInfo.sParam4[1] == '1')
                {
                    mon.AutoChangeColor = true;
                }
                else if (questActionInfo.nParam5 > 0)
                {
                    mon.FixColor = true;
                    mon.FixColorIdx = (byte)(questActionInfo.nParam5 - 1);
                }
            }
        }

        private void ActionOfReNewLevel(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            var nReLevel = (byte)HUtil32.StrToInt(questActionInfo.sParam1, -1);
            var nLevel = (byte)HUtil32.StrToInt(questActionInfo.sParam2, -1);
            var nBounsuPoint = HUtil32.StrToInt(questActionInfo.sParam3, -1);
            if (nReLevel < 0 || nLevel < 0 || nBounsuPoint < 0)
            {
                ScriptActionError(playObject, "", questActionInfo, ScriptConst.sSC_RENEWLEVEL);
                return;
            }
            if (playObject.ReLevel + nReLevel <= 255)
            {
                playObject.ReLevel += nReLevel;
                if (nLevel > 0)
                {
                    playObject.Abil.Level = nLevel;
                }
                if (M2Share.Config.ReNewLevelClearExp)
                {
                    playObject.Abil.Exp = 0;
                }
                playObject.BonusPoint += nBounsuPoint;
                playObject.SendMsg(Messages.RM_ADJUST_BONUS, 0, 0, 0, 0);
                playObject.HasLevelUp(0);
                playObject.RefShowName();
            }
        }

        private void ActionOfChangeGender(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            var nGender = HUtil32.StrToInt(questActionInfo.sParam1, -1);
            if (nGender > 1)
            {
                ScriptActionError(playObject, "", questActionInfo, ScriptConst.sSC_CHANGEGENDER);
                return;
            }
            playObject.Gender = Enum.Parse<PlayGender>(nGender.ToString());
            playObject.FeatureChanged();
        }

        private static void ActionOfKillSlave(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            for (var i = 0; i < playObject.SlaveList.Count; i++)
            {
                var slave = playObject.SlaveList[i];
                slave.WAbil.HP = 0;
            }
        }

        private void ActionOfKillMonExpRate(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            var nRate = HUtil32.StrToInt(questActionInfo.sParam1, -1);
            var nTime = HUtil32.StrToInt(questActionInfo.sParam2, -1);
            if (nRate < 0 || nTime < 0)
            {
                ScriptActionError(playObject, "", questActionInfo, ScriptConst.sSC_KILLMONEXPRATE);
                return;
            }
            playObject.KillMonExpRate = nRate;
            playObject.KillMonExpRateTime = nTime;
            if (M2Share.Config.ShowScriptActionMsg)
            {
                playObject.SysMsg(Format(Settings.ChangeKillMonExpRateMsg, playObject.KillMonExpRate / 100, playObject.KillMonExpRateTime), MsgColor.Green, MsgType.Hint);
            }
        }

        private void ActionOfMonGenEx(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            var sMapName = questActionInfo.sParam1;
            var nMapX = HUtil32.StrToInt(questActionInfo.sParam2, -1);
            var nMapY = HUtil32.StrToInt(questActionInfo.sParam3, -1);
            var sMonName = questActionInfo.sParam4;
            var nRange = questActionInfo.nParam5;
            var nCount = questActionInfo.nParam6;
            if (string.IsNullOrEmpty(sMapName) || nMapX <= 0 || nMapY <= 0 || string.IsNullOrEmpty(sMapName) || nRange <= 0 || nCount <= 0)
            {
                ScriptActionError(playObject, "", questActionInfo, ScriptConst.sSC_MONGENEX);
                return;
            }
            for (var i = 0; i < nCount; i++)
            {
                var nRandX = (short)(M2Share.RandomNumber.Random(nRange * 2 + 1) + (nMapX - nRange));
                var nRandY = (short)(M2Share.RandomNumber.Random(nRange * 2 + 1) + (nMapY - nRange));
                if (M2Share.WorldEngine.RegenMonsterByName(sMapName, nRandX, nRandY, sMonName) == null)
                {
                    break;
                }
            }
        }

        private void ActionOfOpenMagicBox(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            short nX = 0;
            short nY = 0;
            var sMonName = questActionInfo.sParam1;
            if (string.IsNullOrEmpty(sMonName))
            {
                ScriptActionError(playObject, "", questActionInfo, ScriptConst.sSC_OPENMAGICBOX);
                return;
            }
            playObject.GetFrontPosition(ref nX, ref nY);
            var monster = M2Share.WorldEngine.RegenMonsterByName(playObject.Envir.MapName, nX, nY, sMonName);
            if (monster == null)
            {
                return;
            }
            monster.Die();
        }

        private void ActionOfPkZone(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            FireBurnEvent fireBurnEvent;
            var nRange = HUtil32.StrToInt(questActionInfo.sParam1, -1);
            var nType = HUtil32.StrToInt(questActionInfo.sParam2, -1);
            var nTime = HUtil32.StrToInt(questActionInfo.sParam3, -1);
            var nPoint = HUtil32.StrToInt(questActionInfo.sParam4, -1);
            if (nRange < 0 || nType < 0 || nTime < 0 || nPoint < 0)
            {
                ScriptActionError(playObject, "", questActionInfo, ScriptConst.sSC_PKZONE);
                return;
            }
            var nMinX = CurrX - nRange;
            var nMaxX = CurrX + nRange;
            var nMinY = CurrY - nRange;
            var nMaxY = CurrY + nRange;
            for (var nX = nMinX; nX <= nMaxX; nX++)
            {
                for (var nY = nMinY; nY <= nMaxY; nY++)
                {
                    if (nX < nMaxX && nY == nMinY || nY < nMaxY && nX == nMinX || nX == nMaxX ||
                        nY == nMaxY)
                    {
                        fireBurnEvent = new FireBurnEvent(playObject, (short)nX, (short)nY, (byte)nType, nTime * 1000, nPoint);
                        M2Share.EventMgr.AddEvent(fireBurnEvent);
                    }
                }
            }
        }

        private void ActionOfPowerRate(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            var nRate = HUtil32.StrToInt(questActionInfo.sParam1, -1);
            var nTime = HUtil32.StrToInt(questActionInfo.sParam2, -1);
            if (nRate < 0 || nTime < 0)
            {
                ScriptActionError(playObject, "", questActionInfo, ScriptConst.sSC_POWERRATE);
                return;
            }
            playObject.PowerRate = nRate;
            playObject.PowerRateTime = nTime;
            if (M2Share.Config.ShowScriptActionMsg)
            {
                playObject.SysMsg(Format(Settings.ChangePowerRateMsg, playObject.PowerRate / 100, playObject.PowerRateTime), MsgColor.Green, MsgType.Hint);
            }
        }

        private void ActionOfChangeMode(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            var nMode = questActionInfo.nParam1;
            var boOpen = HUtil32.StrToInt(questActionInfo.sParam2, -1) == 1;
            if (nMode >= 1 && nMode <= 3)
            {
                switch (nMode)
                {
                    case 1:
                        playObject.AdminMode = boOpen;
                        if (playObject.AdminMode)
                        {
                            playObject.SysMsg(Settings.GameMasterMode, MsgColor.Green, MsgType.Hint);
                        }
                        else
                        {
                            playObject.SysMsg(Settings.ReleaseGameMasterMode, MsgColor.Green, MsgType.Hint);
                        }
                        break;
                    case 2:
                        playObject.SuperMan = boOpen;
                        if (playObject.SuperMan)
                        {
                            playObject.SysMsg(Settings.SupermanMode, MsgColor.Green, MsgType.Hint);
                        }
                        else
                        {
                            playObject.SysMsg(Settings.ReleaseSupermanMode, MsgColor.Green, MsgType.Hint);
                        }
                        break;
                    case 3:
                        playObject.ObMode = boOpen;
                        if (playObject.ObMode)
                        {
                            playObject.SysMsg(Settings.ObserverMode, MsgColor.Green, MsgType.Hint);
                        }
                        else
                        {
                            playObject.SysMsg(Settings.ReleaseObserverMode, MsgColor.Green, MsgType.Hint);
                        }
                        break;
                }
            }
            else
            {
                ScriptActionError(playObject, "", questActionInfo, ScriptConst.sSC_CHANGEMODE);
            }
        }

        private void ActionOfChangePerMission(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            var nPermission = (byte)HUtil32.StrToInt(questActionInfo.sParam1, -1);
            if (nPermission <= 10)
            {
                playObject.Permission = nPermission;
            }
            else
            {
                ScriptActionError(playObject, "", questActionInfo, ScriptConst.sSC_CHANGEPERMISSION);
                return;
            }
            if (M2Share.Config.ShowScriptActionMsg)
            {
                playObject.SysMsg(Format(Settings.ChangePermissionMsg, playObject.Permission), MsgColor.Green, MsgType.Hint);
            }
        }

        private void ActionOfThrowitem(PlayObject playObject, QuestActionInfo questActionInfo)
        {
            var sMap = string.Empty;
            var sItemName = string.Empty;
            var nX = 0;
            var nY = 0;
            var nRange = 0;
            var nCount = 0;
            var dX = 0;
            var dY = 0;
            UserItem userItem = null;
            try
            {
                if (!GetValValue(playObject, questActionInfo.sParam1, ref sMap))
                {
                    sMap = GetLineVariableText(playObject, questActionInfo.sParam1);
                }
                if (!GetValValue(playObject, questActionInfo.sParam2, ref nX))
                {
                    nX = HUtil32.StrToInt(GetLineVariableText(playObject, questActionInfo.sParam2), -1);
                }
                if (!GetValValue(playObject, questActionInfo.sParam3, ref nY))
                {
                    nY = HUtil32.StrToInt(GetLineVariableText(playObject, questActionInfo.sParam3), -1);
                }
                if (!GetValValue(playObject, questActionInfo.sParam4, ref nRange))
                {
                    nRange = HUtil32.StrToInt(GetLineVariableText(playObject, questActionInfo.sParam4), -1);
                }
                if (!GetValValue(playObject, questActionInfo.sParam5, ref sItemName))
                {
                    sItemName = GetLineVariableText(playObject, questActionInfo.sParam5);
                }
                if (!GetValValue(playObject, questActionInfo.sParam6, ref nCount))
                {
                    nCount = HUtil32.StrToInt(GetLineVariableText(playObject, questActionInfo.sParam6), -1);
                }
                if (string.IsNullOrEmpty(sMap) || nX < 0 || nY < 0 || nRange < 0 || string.IsNullOrEmpty(sItemName) || nCount <= 0)
                {
                    ScriptActionError(playObject, "", questActionInfo, ScriptConst.sTHROWITEM);
                    return;
                }
                var envir = M2Share.MapMgr.FindMap(sMap);
                if (envir == null)
                {
                    return;
                }
                if (nCount <= 0)
                {
                    nCount = 1;
                }
                MapItem mapItem;
                MapItem mapItemA;
                if (string.Compare(sItemName, Grobal2.StringGoldName, StringComparison.OrdinalIgnoreCase) == 0)// 金币
                {
                    if (GetActionOfThrowitemDropPosition(envir, nX, nY, nRange, ref dX, ref dY))
                    {
                        mapItem = new MapItem();
                        mapItem.Name = Grobal2.StringGoldName;
                        mapItem.Count = nCount;
                        mapItem.Looks = M2Share.GetGoldShape(nCount);
                        mapItem.OfBaseObject = playObject.ActorId;
                        mapItem.CanPickUpTick = HUtil32.GetTickCount();
                        mapItem.DropBaseObject = playObject.ActorId;
                        mapItemA = (MapItem)envir.AddToMap(dX, dY, CellType.Item, mapItem.ItemId, mapItem);
                        if (mapItemA != null)
                        {
                            if (mapItemA != mapItem)
                            {
                                Dispose(mapItem);
                                mapItem = mapItemA;
                            }
                            SendRefMsg(Messages.RM_ITEMSHOW, mapItem.Looks, mapItem.ItemId, dX, dY, mapItem.Name + "@0");
                        }
                        else
                        {
                            Dispose(mapItem);
                        }
                        return;
                    }
                }
                for (var i = 0; i < nCount; i++)
                {
                    if (GetActionOfThrowitemDropPosition(envir, nX, nY, nRange, ref dX, ref dY)) // 修正出现在一个坐标上
                    {
                        if (M2Share.WorldEngine.CopyToUserItemFromName(sItemName, ref userItem))
                        {
                            var stdItem = M2Share.WorldEngine.GetStdItem(userItem.Index);
                            if (stdItem != null)
                            {
                                if (stdItem.StdMode == 40)
                                {
                                    var idura = userItem.Dura - 2000;
                                    if (idura < 0)
                                    {
                                        idura = 0;
                                    }
                                    userItem.Dura = (ushort)idura;
                                }
                                mapItem = new MapItem();
                                mapItem.UserItem = new UserItem(userItem);
                                mapItem.Name = stdItem.Name;
                                var nameCorlr = "@" + CustomItem.GetItemAddValuePointColor(userItem); // 取自定义物品名称
                                if (userItem.Desc[13] == 1)
                                {
                                    var sUserItemName = M2Share.CustomItemMgr.GetCustomItemName(userItem.MakeIndex, userItem.Index);
                                    if (!string.IsNullOrEmpty(sUserItemName))
                                    {
                                        mapItem.Name = sUserItemName;
                                    }
                                }
                                mapItem.Looks = stdItem.Looks;
                                if (stdItem.StdMode == 45)
                                {
                                    mapItem.Looks = (ushort)M2Share.GetRandomLook(mapItem.Looks, stdItem.Shape);
                                }
                                mapItem.AniCount = stdItem.AniCount;
                                mapItem.Reserved = 0;
                                mapItem.Count = nCount;
                                mapItem.OfBaseObject = playObject.ActorId;
                                mapItem.CanPickUpTick = HUtil32.GetTickCount();
                                mapItem.DropBaseObject = playObject.ActorId;
                                // GetDropPosition(nX, nY, nRange, dx, dy);//取掉物的位置
                                mapItemA = (MapItem)envir.AddToMap(dX, dY, CellType.Item, mapItem.ItemId, mapItem);
                                if (mapItemA != null)
                                {
                                    if (mapItemA != mapItem)
                                    {
                                        Dispose(mapItem);
                                        mapItem = mapItemA;
                                    }
                                    SendRefMsg(Messages.RM_ITEMSHOW, mapItem.Looks, mapItem.ItemId, dX, dY, mapItem.Name + nameCorlr);
                                }
                                else
                                {
                                    Dispose(mapItem);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            Dispose(userItem);
                        }
                    }
                }
            }
            catch
            {
                M2Share.Logger.Error("{异常} TNormNpc.ActionOfTHROWITEM");
            }
        }

        private static bool GetActionOfThrowitemDropPosition(Envirnoment envir, int nOrgX, int nOrgY, int nRange, ref int nDx, ref int nDy)
        {
            var nItemCount = 0;
            var n24 = 999;
            var result = false;
            var n28 = 0;
            var n2C = 0;
            for (var i = 0; i < nRange; i++)
            {
                for (var j = -i; j <= i; j++)
                {
                    for (var k = -i; k <= i; k++)
                    {
                        nDx = nOrgX + k;
                        nDy = nOrgY + j;
                        if (envir.GetItemEx(nDx, nDy, ref nItemCount) == 0)
                        {
                            if (envir.ChFlag)
                            {
                                result = true;
                                break;
                            }
                        }
                        else
                        {
                            if (envir.ChFlag && n24 > nItemCount)
                            {
                                n24 = nItemCount;
                                n28 = nDx;
                                n2C = nDy;
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
                    nDx = n28;
                    nDy = n2C;
                }
                else
                {
                    nDx = nOrgX;
                    nDy = nOrgY;
                }
            }
            return result;
        }
    }
}