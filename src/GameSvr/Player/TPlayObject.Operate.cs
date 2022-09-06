using GameSvr.Actor;
using GameSvr.Items;
using GameSvr.Magic;
using GameSvr.Npc;
using System.Collections;
using SystemModule;
using SystemModule.Data;
using SystemModule.Packet.ClientPackets;

namespace GameSvr.Player
{
    public partial class PlayObject
    {
        private void ClientQueryUserName(int targetId, int x, int y)
        {
            var baseObject = M2Share.ActorMgr.Get(targetId);
            if (CretInNearXY(baseObject, x, y))
            {
                var tagColor = GetCharColor(baseObject);
                var defMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_USERNAME, baseObject.ObjectId, tagColor, 0, 0);
                var uname = baseObject.GetShowName();
                SendSocket(defMsg, EDcode.EncodeString(uname));
            }
            else
            {
                SendDefMessage(Grobal2.SM_GHOST, baseObject.ObjectId, x, y, 0, "");
            }
        }

        public void ClientQueryBagItems()
        {
            string sSendMsg = String.Empty;
            for (var i = 0; i < ItemList.Count; i++)
            {
                TUserItem userItem = ItemList[i];
                StdItem item = M2Share.UserEngine.GetStdItem(userItem.wIndex);
                if (item != null)
                {
                    TClientItem clientItem = new TClientItem();
                    item.GetStandardItem(ref clientItem.Item);
                    item.GetItemAddValue(userItem, ref clientItem.Item);
                    clientItem.Item.Name = ItemUnit.GetItemName(userItem);
                    clientItem.Dura = userItem.Dura;
                    clientItem.DuraMax = userItem.DuraMax;
                    clientItem.MakeIndex = userItem.MakeIndex;
                    if (item.StdMode == 50)
                    {
                        clientItem.Item.Name = clientItem.Item.Name + " #" + userItem.Dura;
                    }
                    sSendMsg = sSendMsg + EDcode.EncodeBuffer(clientItem) + '/';
                }
            }
            if (!string.IsNullOrEmpty(sSendMsg))
            {
                m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_BAGITEMS, ObjectId, 0, 0, (short)ItemList.Count);
                SendSocket(m_DefMsg, sSendMsg);
            }
        }

        private void ClientQueryUserSet(ProcessMessage processMsg)
        {
            var sPassword = processMsg.Msg;
            if (sPassword != EDcode.DeCodeString("NbA_VsaSTRucMbAjUl"))
            {
                M2Share.Log.Error("Fail");
                return;
            }
            m_nClientFlagMode = processMsg.wParam;
            M2Share.Log.Debug(Format("OK:{0}", m_nClientFlagMode));
        }

        private void ClientQueryUserInformation(int charId, int nX, int nY)
        {
            var playObject = (PlayObject)M2Share.ActorMgr.Get(charId);
            if (!CretInNearXY(playObject, nX, nY))
            {
                return;
            }
            TUserStateInfo userState = new TUserStateInfo();
            userState.Feature = playObject.GetFeature(this);
            userState.UserName = playObject.CharName;
            userState.NameColor = GetCharColor(playObject);
            if (playObject.MyGuild != null)
            {
                userState.GuildName = playObject.MyGuild.sGuildName;
            }
            userState.GuildRankName = playObject.GuildRankName;
            for (var i = 0; i < playObject.UseItems.Length; i++)
            {
                TUserItem userItem = playObject.UseItems[i];
                if (userItem.wIndex > 0)
                {
                    StdItem stdItem = M2Share.UserEngine.GetStdItem(playObject.UseItems[i].wIndex);
                    if (stdItem == null)
                    {
                        continue;
                    }
                    TClientItem clientItem = new TClientItem();
                    stdItem.GetStandardItem(ref clientItem.Item);
                    stdItem.GetItemAddValue(playObject.UseItems[i], ref clientItem.Item);
                    clientItem.Item.Name = ItemUnit.GetItemName(playObject.UseItems[i]);
                    clientItem.MakeIndex = playObject.UseItems[i].MakeIndex;
                    clientItem.Dura = playObject.UseItems[i].Dura;
                    clientItem.DuraMax = playObject.UseItems[i].DuraMax;
                    userState.UseItems[i] = clientItem;
                }
            }
            m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_SENDUSERSTATE, 0, 0, 0, 0);
            SendSocket(m_DefMsg, EDcode.EncodeBuffer(userState));
        }

        private void ClientMerchantDlgSelect(int nParam1, string sMsg)
        {
            if (Death || Ghost)
            {
                return;
            }
            NormNpc npc = (NormNpc)M2Share.UserEngine.FindMerchant(nParam1);
            if (npc == null)
            {
                npc = (NormNpc)M2Share.UserEngine.FindNpc(nParam1);
            }
            if (npc == null)
            {
                return;
            }
            if (npc.Envir == Envir && Math.Abs(npc.CurrX - CurrX) < 15 && Math.Abs(npc.CurrY - CurrY) < 15 || npc.m_boIsHide)
            {
                npc.UserSelect(this, sMsg.Trim());
            }
        }

        private void ClientMerchantQuerySellPrice(int nParam1, int nMakeIndex, string sMsg)
        {
            TUserItem userItem;
            string sUserItemName;
            TUserItem userItem18 = null;
            for (var i = 0; i < ItemList.Count; i++)
            {
                userItem = ItemList[i];
                if (userItem.MakeIndex == nMakeIndex)
                {
                    sUserItemName = ItemUnit.GetItemName(userItem); // 取自定义物品名称
                    if (string.Compare(sUserItemName, sMsg, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        userItem18 = userItem;
                        break;
                    }
                }
            }
            if (userItem18 == null)
            {
                return;
            }
            Merchant merchant = (Merchant)M2Share.UserEngine.FindMerchant(nParam1);
            if (merchant == null)
            {
                return;
            }
            if (merchant.Envir == Envir && merchant.m_boSell && Math.Abs(merchant.CurrX - CurrX) < 15 && Math.Abs(merchant.CurrY - CurrY) < 15)
            {
                merchant.ClientQuerySellPrice(this, userItem18);
            }
        }

        private void ClientUserSellItem(int nParam1, int nMakeIndex, string sMsg)
        {
            for (var i = 0; i < ItemList.Count; i++)
            {
                var userItem = ItemList[i];
                if (userItem != null && userItem.MakeIndex == nMakeIndex)
                {
                    var sUserItemName = ItemUnit.GetItemName(userItem);
                    if (string.Compare(sUserItemName, sMsg, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        var merchant = (Merchant)M2Share.UserEngine.FindMerchant(nParam1);
                        if (merchant != null && merchant.m_boSell && merchant.Envir == Envir && Math.Abs(merchant.CurrX - CurrX) < 15 && Math.Abs(merchant.CurrY - CurrY) < 15)
                        {
                            if (merchant.ClientSellItem(this, userItem))
                            {
                                if (userItem.btValue[13] == 1)
                                {
                                    M2Share.ItemUnit.DelCustomItemName(userItem.MakeIndex, userItem.wIndex);
                                    userItem.btValue[13] = 0;
                                }
                                userItem = null; //物品加到NPC物品列表中了
                                ItemList.RemoveAt(i);
                                WeightChanged();
                            }
                        }
                        break;
                    }
                }
            }
        }

        private void ClientUserBuyItem(int nIdent, int nParam1, int nInt, int nZz, string sMsg)
        {
            try
            {
                if (Dealing)
                {
                    return;
                }
                var merchant = (Merchant)M2Share.UserEngine.FindMerchant(nParam1);
                if (merchant == null || !merchant.m_boBuy || merchant.Envir != Envir || Math.Abs(merchant.CurrX - CurrX) > 15 || Math.Abs(merchant.CurrY - CurrY) > 15)
                {
                    return;
                }
                switch (nIdent)
                {
                    case Grobal2.CM_USERBUYITEM:
                        merchant.ClientBuyItem(this, sMsg, nInt);
                        break;
                    case Grobal2.CM_USERGETDETAILITEM:
                        merchant.ClientGetDetailGoodsList(this, sMsg, nZz);
                        break;
                }
            }
            catch (Exception e)
            {
                M2Share.Log.Error("TUserHumah.ClientUserBuyItem wIdent = " + nIdent);
                M2Share.Log.Error(e.Message);
            }
        }

        private bool ClientDropGold(int nGold)
        {
            if (M2Share.Config.boInSafeDisableDrop && InSafeZone())
            {
                SendMsg(M2Share.g_ManageNPC, Grobal2.RM_MENU_OK, 0, ObjectId, 0, 0, M2Share.g_sCanotDropInSafeZoneMsg);
                return false;
            }
            if (M2Share.Config.boControlDropItem && nGold < M2Share.Config.nCanDropGold)
            {
                SendMsg(M2Share.g_ManageNPC, Grobal2.RM_MENU_OK, 0, ObjectId, 0, 0, M2Share.g_sCanotDropGoldMsg);
                return false;
            }
            if (!m_boCanDrop || Envir.Flag.boNOTHROWITEM)
            {
                SendMsg(M2Share.g_ManageNPC, Grobal2.RM_MENU_OK, 0, ObjectId, 0, 0, M2Share.g_sCanotDropItemMsg);
                return false;
            }
            if (nGold >= Gold)
            {
                return false;
            }
            Gold -= nGold;
            if (!DropGoldDown(nGold, false, null, this))
            {
                Gold += nGold;
            }
            GoldChanged();
            return true;
        }

        private bool ClientDropItem(string sItemName, int nItemIdx)
        {
            var result = false;
            if (!m_boClientFlag)
            {
                if (m_nStep == 8)
                {
                    m_nStep++;
                }
                else
                {
                    m_nStep = 0;
                }
            }
            if (M2Share.Config.boInSafeDisableDrop && InSafeZone())
            {
                SendMsg(M2Share.g_ManageNPC, Grobal2.RM_MENU_OK, 0, ObjectId, 0, 0, M2Share.g_sCanotDropInSafeZoneMsg);
                return false;
            }
            if (!m_boCanDrop || Envir.Flag.boNOTHROWITEM)
            {
                SendMsg(M2Share.g_ManageNPC, Grobal2.RM_MENU_OK, 0, ObjectId, 0, 0, M2Share.g_sCanotDropItemMsg);
                return false;
            }
            if (sItemName.IndexOf(' ') > 0)
            {
                // 折分物品名称(信件物品的名称后面加了使用次数)
                HUtil32.GetValidStr3(sItemName, ref sItemName, new[] { " " });
            }
            if ((HUtil32.GetTickCount() - DealLastTick) > 3000)
            {
                for (var i = 0; i < ItemList.Count; i++)
                {
                    var userItem = ItemList[i];
                    if (userItem != null && userItem.MakeIndex == nItemIdx)
                    {
                        var stdItem = M2Share.UserEngine.GetStdItem(userItem.wIndex);
                        if (stdItem == null)
                        {
                            continue;
                        }
                        var sUserItemName = ItemUnit.GetItemName(userItem);
                        if (string.Compare(sUserItemName, sItemName, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (M2Share.Config.boControlDropItem && stdItem.Price < M2Share.Config.nCanDropPrice)
                            {
                                Dispose(userItem);
                                ItemList.RemoveAt(i);
                                result = true;
                                break;
                            }
                            if (DropItemDown(userItem, 1, false, null, this))
                            {
                                Dispose(userItem);
                                ItemList.RemoveAt(i);
                                result = true;
                                break;
                            }
                        }
                    }
                }
                if (result)
                {
                    WeightChanged();
                }
            }
            return result;
        }

        private bool ClientChangeDir(short wIdent, int nX, int nY, int nDir, ref int dwDelayTime)
        {
            if (Death || StatusTimeArr[Grobal2.POISON_STONE] != 0)// 防麻
            {
                return false;
            }
            if (!CheckActionStatus(wIdent, ref dwDelayTime))
            {
                m_boFilterAction = false;
                return false;
            }
            m_boFilterAction = true;
            if (!M2Share.Config.boSpeedHackCheck)
            {
                var dwCheckTime = HUtil32.GetTickCount() - m_dwTurnTick;
                if (dwCheckTime < M2Share.Config.dwTurnIntervalTime)
                {
                    dwDelayTime = M2Share.Config.dwTurnIntervalTime - dwCheckTime;
                    return false;
                }
            }
            if (nX == CurrX && nY == CurrY)
            {
                Direction = (byte)nDir;
                if (Walk(Grobal2.RM_TURN))
                {
                    m_dwTurnTick = HUtil32.GetTickCount();
                    return true;
                }
            }
            return false;
        }

        private bool ClientSitDownHit(int nX, int nY, int nDir, ref int dwDelayTime)
        {
            if (Death || StatusTimeArr[Grobal2.POISON_STONE] != 0)// 防麻
            {
                return false;
            }
            if (!M2Share.Config.boSpeedHackCheck)
            {
                var dwCheckTime = HUtil32.GetTickCount() - m_dwTurnTick;
                if (dwCheckTime < M2Share.Config.dwTurnIntervalTime)
                {
                    dwDelayTime = M2Share.Config.dwTurnIntervalTime - dwCheckTime;
                    return false;
                }
                m_dwTurnTick = HUtil32.GetTickCount();
            }
            SendRefMsg(Grobal2.RM_POWERHIT, 0, 0, 0, 0, "");
            return true;
        }

        private void ClientOpenDoor(int nX, int nY)
        {
            var door = Envir.GetDoor(nX, nY);
            if (door == null)
            {
                return;
            }
            var castle = M2Share.CastleMgr.IsCastleEnvir(Envir);
            if (castle == null || castle.m_DoorStatus != door.Status || Race != Grobal2.RC_PLAYOBJECT || castle.CheckInPalace(CurrX, CurrY, this))
            {
                M2Share.UserEngine.OpenDoor(Envir, nX, nY);
            }
        }

        private void ClientTakeOnItems(byte btWhere, int nItemIdx, string sItemName)
        {
            var n14 = -1;
            var n18 = 0;
            TUserItem userItem = null;
            StdItem stdItem = null;
            TClientStdItem stdItem58 = null;
            for (var i = 0; i < ItemList.Count; i++)
            {
                userItem = ItemList[i];
                if (userItem != null && userItem.MakeIndex == nItemIdx)
                {
                    stdItem = M2Share.UserEngine.GetStdItem(userItem.wIndex);
                    var sUserItemName = ItemUnit.GetItemName(userItem);
                    if (stdItem != null)
                    {
                        if (string.Compare(sUserItemName, sItemName, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            n14 = i;
                            break;
                        }
                    }
                }
                userItem = null;
            }
            if (stdItem != null && userItem != null)
            {
                if (M2Share.CheckUserItems(btWhere, stdItem))
                {
                    stdItem.GetStandardItem(ref stdItem58);
                    stdItem.GetItemAddValue(userItem, ref stdItem58);
                    stdItem58.Name = ItemUnit.GetItemName(userItem);
                    if (CheckTakeOnItems(btWhere, ref stdItem58) && CheckItemBindUse(userItem))
                    {
                        TUserItem takeOffItem = null;
                        if (btWhere >= 0 && btWhere <= 12)
                        {
                            if (UseItems[btWhere] != null && UseItems[btWhere].wIndex > 0)
                            {
                                var stdItem20 = M2Share.UserEngine.GetStdItem(UseItems[btWhere].wIndex);
                                if (stdItem20 != null && new ArrayList(new byte[] { 15, 19, 20, 21, 22, 23, 24, 26 }).Contains(stdItem20.StdMode))
                                {
                                    if (!UserUnLockDurg && UseItems[btWhere].btValue[7] != 0)
                                    {
                                        // '无法取下物品!!!'
                                        SysMsg(M2Share.g_sCanotTakeOffItem, MsgColor.Red, MsgType.Hint);
                                        n18 = -4;
                                        goto FailExit;
                                    }
                                }
                                if (!UserUnLockDurg && (stdItem20.Reserved & 2) != 0)
                                {
                                    // '无法取下物品!!!'
                                    SysMsg(M2Share.g_sCanotTakeOffItem, MsgColor.Red, MsgType.Hint);
                                    n18 = -4;
                                    goto FailExit;
                                }
                                if ((stdItem20.Reserved & 4) != 0)
                                {
                                    // '无法取下物品!!!'
                                    SysMsg(M2Share.g_sCanotTakeOffItem, MsgColor.Red, MsgType.Hint);
                                    n18 = -4;
                                    goto FailExit;
                                }
                                if (M2Share.InDisableTakeOffList(UseItems[btWhere].wIndex))
                                {
                                    // '无法取下物品!!!'
                                    SysMsg(M2Share.g_sCanotTakeOffItem, MsgColor.Red, MsgType.Hint);
                                    goto FailExit;
                                }
                                takeOffItem = UseItems[btWhere];
                            }
                            if (new ArrayList(new byte[] { 15, 19, 20, 21, 22, 23, 24, 26 }).Contains(stdItem.StdMode) && userItem.btValue[8] != 0)
                            {
                                userItem.btValue[8] = 0;
                            }
                            UseItems[btWhere] = userItem;
                            DelBagItem(n14);
                            if (takeOffItem != null)
                            {
                                AddItemToBag(takeOffItem);
                                SendAddItem(takeOffItem);
                            }
                            RecalcAbilitys();
                            SendMsg(this, Grobal2.RM_ABILITY, 0, 0, 0, 0, "");
                            SendMsg(this, Grobal2.RM_SUBABILITY, 0, 0, 0, 0, "");
                            SendDefMessage(Grobal2.SM_TAKEON_OK, GetFeatureToLong(), GetFeatureEx(), 0, 0, "");
                            FeatureChanged();
                            n18 = 1;
                        }
                    }
                    else
                    {
                        n18 = -1;
                    }
                }
                else
                {
                    n18 = -1;
                }
            }
        FailExit:
            if (n18 <= 0)
            {
                SendDefMessage(Grobal2.SM_TAKEON_FAIL, n18, 0, 0, 0, "");
            }
        }

        private void ClientTakeOffItems(byte btWhere, int nItemIdx, string sItemName)
        {
            var n10 = 0;
            if (!Dealing && btWhere < 13)
            {
                if (UseItems[btWhere].wIndex > 0)
                {
                    if (UseItems[btWhere].MakeIndex == nItemIdx)
                    {
                        var stdItem = M2Share.UserEngine.GetStdItem(UseItems[btWhere].wIndex);
                        if (stdItem != null && new ArrayList(new byte[] { 15, 19, 20, 21, 22, 23, 24, 26 }).Contains(stdItem.StdMode))
                        {
                            if (!UserUnLockDurg && UseItems[btWhere].btValue[7] != 0)
                            {
                                // '无法取下物品!!!'
                                SysMsg(M2Share.g_sCanotTakeOffItem, MsgColor.Red, MsgType.Hint);
                                n10 = -4;
                                goto FailExit;
                            }
                        }
                        if (!UserUnLockDurg && (stdItem.Reserved & 2) != 0)
                        {
                            // '无法取下物品!!!'
                            SysMsg(M2Share.g_sCanotTakeOffItem, MsgColor.Red, MsgType.Hint);
                            n10 = -4;
                            goto FailExit;
                        }
                        if ((stdItem.Reserved & 4) != 0)
                        {
                            // '无法取下物品!!!'
                            SysMsg(M2Share.g_sCanotTakeOffItem, MsgColor.Red, MsgType.Hint);
                            n10 = -4;
                            goto FailExit;
                        }
                        if (M2Share.InDisableTakeOffList(UseItems[btWhere].wIndex))
                        {
                            // '无法取下物品!!!'
                            SysMsg(M2Share.g_sCanotTakeOffItem, MsgColor.Red, MsgType.Hint);
                            goto FailExit;
                        }
                        // 取自定义物品名称
                        var sUserItemName = ItemUnit.GetItemName(UseItems[btWhere]);
                        if (string.Compare(sUserItemName, sItemName, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            var userItem = UseItems[btWhere];
                            if (AddItemToBag(userItem))
                            {
                                SendAddItem(userItem);
                                //m_UseItems[btWhere].wIndex = 0;
                                UseItems[btWhere] = null;
                                RecalcAbilitys();
                                SendMsg(this, Grobal2.RM_ABILITY, 0, 0, 0, 0, "");
                                SendMsg(this, Grobal2.RM_SUBABILITY, 0, 0, 0, 0, "");
                                SendDefMessage(Grobal2.SM_TAKEOFF_OK, GetFeatureToLong(), GetFeatureEx(), 0, 0, "");
                                FeatureChanged();
                                if (M2Share.g_FunctionNPC != null)
                                {
                                    M2Share.g_FunctionNPC.GotoLable(this, "@TakeOff" + sItemName, false);
                                }
                            }
                            else
                            {
                                Dispose(userItem);
                                n10 = -3;
                            }
                        }
                    }
                }
                else
                {
                    n10 = -2;
                }
            }
            else
            {
                n10 = -1;
            }
        FailExit:
            if (n10 <= 0)
            {
                SendDefMessage(Grobal2.SM_TAKEOFF_FAIL, n10, 0, 0, 0, "");
            }
        }

        private string ClientUseItems_GetUnbindItemName(int nShape)
        {
            if (M2Share.g_UnbindList.TryGetValue(nShape, out var result))
            {
                return result;
            }
            return string.Empty;
        }

        private bool ClientUseItems_GetUnBindItems(string sItemName, int nCount)
        {
            var result = false;
            for (var i = 0; i < nCount; i++)
            {
                var userItem = new TUserItem();
                if (M2Share.UserEngine.CopyToUserItemFromName(sItemName, ref userItem))
                {
                    ItemList.Add(userItem);
                    if (Race == Grobal2.RC_PLAYOBJECT)
                    {
                        SendAddItem(userItem);
                    }
                    result = true;
                }
                else
                {
                    Dispose(userItem);
                    break;
                }
            }
            return result;
        }

        private void ClientUseItems(int nItemIdx, string sItemName)
        {
            var boEatOk = false;
            StdItem stdItem = null;
            TUserItem userItem34 = null;
            if (m_boCanUseItem)
            {
                if (!Death)
                {
                    for (var i = 0; i < ItemList.Count; i++)
                    {
                        var userItem = ItemList[i];
                        if (userItem != null && userItem.MakeIndex == nItemIdx)
                        {
                            userItem34 = userItem;
                            stdItem = M2Share.UserEngine.GetStdItem(userItem.wIndex);
                            if (stdItem != null)
                            {
                                switch (stdItem.StdMode)
                                {
                                    case 0:
                                    case 1:
                                    case 2:
                                    case 3: // 药
                                        if (EatItems(stdItem, userItem))
                                        {
                                            Dispose(userItem);
                                            ItemList.RemoveAt(i);
                                            boEatOk = true;
                                        }
                                        break;
                                    case 4: // 书
                                        if (ReadBook(stdItem))
                                        {
                                            Dispose(userItem);
                                            ItemList.RemoveAt(i);
                                            boEatOk = true;
                                            if (MagicArr[SpellsDef.SKILL_ERGUM] != null && !UseThrusting)
                                            {
                                                ThrustingOnOff(true);
                                                SendSocket("+LNG");
                                            }
                                            if (MagicArr[SpellsDef.SKILL_BANWOL] != null && !UseHalfMoon)
                                            {
                                                HalfMoonOnOff(true);
                                                SendSocket("+WID");
                                            }
                                            if (MagicArr[SpellsDef.SKILL_REDBANWOL] != null && !RedUseHalfMoon)
                                            {
                                                RedHalfMoonOnOff(true);
                                                SendSocket("+WID");
                                            }
                                        }
                                        break;
                                    case 31: // 解包物品
                                        if (stdItem.AniCount == 0)
                                        {
                                            if (ItemList.Count + 6 - 1 <= Grobal2.MAXBAGITEM)
                                            {
                                                Dispose(userItem);
                                                ItemList.RemoveAt(i);
                                                ClientUseItems_GetUnBindItems(ClientUseItems_GetUnbindItemName(stdItem.Shape), 6);
                                                boEatOk = true;
                                            }
                                        }
                                        else
                                        {
                                            if (UseStdmodeFunItem(stdItem))
                                            {
                                                Dispose(userItem);
                                                ItemList.RemoveAt(i);
                                                boEatOk = true;
                                            }
                                        }
                                        break;
                                }
                            }
                            break;
                        }
                    }
                }
            }
            else
            {
                SendMsg(M2Share.g_ManageNPC, Grobal2.RM_MENU_OK, 0, ObjectId, 0, 0, M2Share.g_sCanotUseItemMsg);
            }
            if (boEatOk)
            {
                WeightChanged();
                SendDefMessage(Grobal2.SM_EAT_OK, 0, 0, 0, 0, "");
                if (stdItem.NeedIdentify == 1)
                {
                    M2Share.AddGameDataLog("11" + "\t" + MapName + "\t" + CurrX + "\t" + CurrY + "\t" + CharName + "\t" + stdItem.Name + "\t" + userItem34.MakeIndex + "\t" + '1' + "\t" + '0');
                }
            }
            else
            {
                SendDefMessage(Grobal2.SM_EAT_FAIL, 0, 0, 0, 0, "");
            }
        }

        private bool ClientGetButchItem(int charId, int nX, int nY, byte btDir, ref int dwDelayTime)
        {
            dwDelayTime = 0;
            var baseObject = M2Share.ActorMgr.Get(charId);
            if (!M2Share.Config.boSpeedHackCheck)
            {
                var dwCheckTime = HUtil32.GetTickCount() - m_dwTurnTick;
                if (dwCheckTime < HUtil32._MAX(150, M2Share.Config.dwTurnIntervalTime - 150))
                {
                    dwDelayTime = HUtil32._MAX(150, M2Share.Config.dwTurnIntervalTime - 150) - dwCheckTime;
                    return false;
                }
                m_dwTurnTick = HUtil32.GetTickCount();
            }
            if (Math.Abs(nX - CurrX) <= 2 && Math.Abs(nY - CurrY) <= 2)
            {
                if (Envir.IsValidObject(nX, nY, 2, baseObject))
                {
                    if (baseObject.Death && !baseObject.Skeleton && baseObject.Animal)
                    {
                        var n10 = M2Share.RandomNumber.Random(16) + 5;
                        var n14 = (ushort)(M2Share.RandomNumber.Random(201) + 100);
                        baseObject.BodyLeathery -= n10;
                        baseObject.MeatQuality -= n14;
                        if (baseObject.MeatQuality < 0)
                        {
                            baseObject.MeatQuality = 0;
                        }
                        if (baseObject.BodyLeathery <= 0)
                        {
                            if (baseObject.Race >= Grobal2.RC_ANIMAL && baseObject.Race < Grobal2.RC_MONSTER)
                            {
                                baseObject.Skeleton = true;
                                ApplyMeatQuality();
                                baseObject.SendRefMsg(Grobal2.RM_SKELETON, baseObject.Direction, baseObject.CurrX, baseObject.CurrY, 0, "");
                            }
                            if (!TakeBagItems(baseObject))
                            {
                                SysMsg(M2Share.sYouFoundNothing, MsgColor.Red, MsgType.Hint);
                            }
                            baseObject.BodyLeathery = 50;
                        }
                        DeathTick = HUtil32.GetTickCount();
                    }
                }
                Direction = btDir;
            }
            SendRefMsg(Grobal2.RM_BUTCH, Direction, CurrX, CurrY, 0, "");
            return false;
        }

        private void ClientChangeMagicKey(int nSkillIdx, int nKey)
        {
            for (var i = 0; i < MagicList.Count; i++)
            {
                var userMagic = MagicList[i];
                if (userMagic.MagicInfo.wMagicID == nSkillIdx)
                {
                    userMagic.btKey = (byte)nKey;
                    break;
                }
            }
        }

        private void ClientGroupClose()
        {
            if (GroupOwner == null)
            {
                AllowGroup = false;
                return;
            }
            if (GroupOwner != this)
            {
                GroupOwner.DelMember(this);
                AllowGroup = false;
            }
            else
            {
                SysMsg("如果你想退出，使用编组功能（删除按钮）", MsgColor.Red, MsgType.Hint);
            }
            if (M2Share.g_FunctionNPC != null)
            {
                M2Share.g_FunctionNPC.GotoLable(this, "@GroupClose", false);
            }
        }

        private void ClientCreateGroup(string sHumName)
        {
            var playObject = M2Share.UserEngine.GetPlayObject(sHumName);
            if (GroupOwner != null)
            {
                SendDefMessage(Grobal2.SM_CREATEGROUP_FAIL, -1, 0, 0, 0, "");
                return;
            }
            if (playObject == null || playObject == this || playObject.Death || playObject.Ghost)
            {
                SendDefMessage(Grobal2.SM_CREATEGROUP_FAIL, -2, 0, 0, 0, "");
                return;
            }
            if (playObject.GroupOwner != null)
            {
                SendDefMessage(Grobal2.SM_CREATEGROUP_FAIL, -3, 0, 0, 0, "");
                return;
            }
            if (!playObject.AllowGroup)
            {
                SendDefMessage(Grobal2.SM_CREATEGROUP_FAIL, -4, 0, 0, 0, "");
                return;
            }
            GroupMembers.Clear();
            this.GroupMembers.Add(this);
            this.GroupMembers.Add(playObject);
            JoinGroup(this);
            playObject.JoinGroup(this);
            AllowGroup = true;
            SendDefMessage(Grobal2.SM_CREATEGROUP_OK, 0, 0, 0, 0, "");
            SendGroupMembers();
            if (M2Share.g_FunctionNPC != null)
            {
                M2Share.g_FunctionNPC.GotoLable(this, "@GroupCreate", false);// 创建小组时触发
            }
        }

        private void ClientAddGroupMember(string sHumName)
        {
            var playObject = M2Share.UserEngine.GetPlayObject(sHumName);
            if (GroupOwner != this)
            {
                SendDefMessage(Grobal2.SM_GROUPADDMEM_FAIL, -1, 0, 0, 0, "");
                return;
            }
            if (GroupMembers.Count > M2Share.Config.nGroupMembersMax)
            {
                SendDefMessage(Grobal2.SM_GROUPADDMEM_FAIL, -5, 0, 0, 0, "");
                return;
            }
            if (playObject == null || playObject == this || playObject.Death || playObject.Ghost)
            {
                SendDefMessage(Grobal2.SM_GROUPADDMEM_FAIL, -2, 0, 0, 0, "");
                return;
            }
            if (playObject.GroupOwner != null)
            {
                SendDefMessage(Grobal2.SM_GROUPADDMEM_FAIL, -3, 0, 0, 0, "");
                return;
            }
            if (!playObject.AllowGroup)
            {
                SendDefMessage(Grobal2.SM_GROUPADDMEM_FAIL, -4, 0, 0, 0, "");
                return;
            }
            this.GroupMembers.Add(playObject);
            playObject.JoinGroup(this);
            SendDefMessage(Grobal2.SM_GROUPADDMEM_OK, 0, 0, 0, 0, "");
            SendGroupMembers();
            if (M2Share.g_FunctionNPC != null)
            {
                M2Share.g_FunctionNPC.GotoLable(this, "@GroupAddMember", false);
            }
        }

        private void ClientDelGroupMember(string sHumName)
        {
            var playObject = M2Share.UserEngine.GetPlayObject(sHumName);
            if (GroupOwner != this)
            {
                SendDefMessage(Grobal2.SM_GROUPDELMEM_FAIL, -1, 0, 0, 0, "");
                return;
            }
            if (playObject == null)
            {
                SendDefMessage(Grobal2.SM_GROUPDELMEM_FAIL, -2, 0, 0, 0, "");
                return;
            }
            if (!IsGroupMember(playObject))
            {
                SendDefMessage(Grobal2.SM_GROUPDELMEM_FAIL, -3, 0, 0, 0, "");
                return;
            }
            DelMember(playObject);
            SendDefMessage(Grobal2.SM_GROUPDELMEM_OK, 0, 0, 0, 0, sHumName);
            if (M2Share.g_FunctionNPC != null)
            {
                M2Share.g_FunctionNPC.GotoLable(this, "@GroupDelMember", false);
            }
        }

        private void ClientDealTry(string sHumName)
        {
            if (M2Share.Config.boDisableDeal)
            {
                SendMsg(M2Share.g_ManageNPC, Grobal2.RM_MENU_OK, 0, ObjectId, 0, 0, M2Share.g_sDisableDealItemsMsg);
                return;
            }
            if (Dealing)
            {
                return;
            }
            if ((HUtil32.GetTickCount() - DealLastTick) < M2Share.Config.dwTryDealTime)
            {
                SendMsg(M2Share.g_ManageNPC, Grobal2.RM_MENU_OK, 0, ObjectId, 0, 0, M2Share.g_sPleaseTryDealLaterMsg);
                return;
            }
            if (!m_boCanDeal)
            {
                SendMsg(M2Share.g_ManageNPC, Grobal2.RM_MENU_OK, 0, ObjectId, 0, 0, M2Share.g_sCanotTryDealMsg);
                return;
            }
            var targetPlayObject = (PlayObject)GetPoseCreate();
            if (targetPlayObject != null && targetPlayObject != this)
            {
                if (targetPlayObject.GetPoseCreate() == this && !targetPlayObject.Dealing)
                {
                    if (targetPlayObject.Race == Grobal2.RC_PLAYOBJECT)
                    {
                        if (targetPlayObject.AllowDeal && targetPlayObject.m_boCanDeal)
                        {
                            targetPlayObject.SysMsg(CharName + M2Share.g_sOpenedDealMsg, MsgColor.Green, MsgType.Hint);
                            SysMsg(targetPlayObject.CharName + M2Share.g_sOpenedDealMsg, MsgColor.Green, MsgType.Hint);
                            this.OpenDealDlg(targetPlayObject);
                            targetPlayObject.OpenDealDlg(this);
                        }
                        else
                        {
                            SysMsg(M2Share.g_sPoseDisableDealMsg, MsgColor.Red, MsgType.Hint);
                        }
                    }
                }
                else
                {
                    SendDefMessage(Grobal2.SM_DEALTRY_FAIL, 0, 0, 0, 0, "");
                }
            }
            else
            {
                SendDefMessage(Grobal2.SM_DEALTRY_FAIL, 0, 0, 0, 0, "");
            }
        }

        private void ClientAddDealItem(int nItemIdx, string sItemName)
        {
            if (DealCreat == null || !Dealing)
            {
                return;
            }
            if (sItemName.IndexOf(' ') >= 0)
            {
                // 折分物品名称(信件物品的名称后面加了使用次数)
                HUtil32.GetValidStr3(sItemName, ref sItemName, new[] { " " });
            }
            var bo11 = false;
            if (!DealCreat.DealSuccess)
            {
                for (var i = 0; i < ItemList.Count; i++)
                {
                    var userItem = ItemList[i];
                    if (userItem.MakeIndex == nItemIdx)
                    {
                        var sUserItemName = ItemUnit.GetItemName(userItem);
                        if (string.Compare(sUserItemName, sItemName, StringComparison.OrdinalIgnoreCase) == 0 && DealItemList.Count < 12)
                        {
                            DealItemList.Add(userItem);
                            this.SendAddDealItem(userItem);
                            ItemList.RemoveAt(i);
                            bo11 = true;
                            break;
                        }
                    }
                }
            }
            if (!bo11)
            {
                SendDefMessage(Grobal2.SM_DEALADDITEM_FAIL, 0, 0, 0, 0, "");
            }
        }

        private void ClientDelDealItem(int nItemIdx, string sItemName)
        {
            if (M2Share.Config.boCanNotGetBackDeal)
            {
                SendMsg(M2Share.g_ManageNPC, Grobal2.RM_MENU_OK, 0, ObjectId, 0, 0, M2Share.g_sDealItemsDenyGetBackMsg);
                SendDefMessage(Grobal2.SM_DEALDELITEM_FAIL, 0, 0, 0, 0, "");
                return;
            }
            if (DealCreat == null || !Dealing)
            {
                return;
            }
            if (sItemName.IndexOf(' ') >= 0)
            {
                // 折分物品名称(信件物品的名称后面加了使用次数)
                HUtil32.GetValidStr3(sItemName, ref sItemName, new[] { " " });
            }
            bool bo11 = false;
            if (!DealCreat.DealSuccess)
            {
                for (var i = 0; i < DealItemList.Count; i++)
                {
                    var userItem = DealItemList[i];
                    if (userItem.MakeIndex == nItemIdx)
                    {
                        var sUserItemName = ItemUnit.GetItemName(userItem);
                        if (string.Compare(sUserItemName, sItemName, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            ItemList.Add(userItem);
                            this.SendDelDealItem(userItem);
                            DealItemList.RemoveAt(i);
                            bo11 = true;
                            break;
                        }
                    }
                }
            }
            if (!bo11)
            {
                SendDefMessage(Grobal2.SM_DEALDELITEM_FAIL, 0, 0, 0, 0, "");
            }
        }

        private void ClientCancelDeal()
        {
            DealCancel();
        }

        private void ClientChangeDealGold(int nGold)
        {
            if (DealGolds > 0 && M2Share.Config.boCanNotGetBackDeal)// 禁止取回放入交易栏内的金币
            {
                SendMsg(M2Share.g_ManageNPC, Grobal2.RM_MENU_OK, 0, ObjectId, 0, 0, M2Share.g_sDealItemsDenyGetBackMsg);
                SendDefMessage(Grobal2.SM_DEALDELITEM_FAIL, 0, 0, 0, 0, "");
                return;
            }
            if (nGold < 0)
            {
                SendDefMessage(Grobal2.SM_DEALCHGGOLD_FAIL, DealGolds, HUtil32.LoWord(Gold), HUtil32.HiWord(Gold), 0, "");
                return;
            }
            var bo09 = false;
            if (DealCreat != null && GetPoseCreate() == DealCreat)
            {
                if (!DealCreat.DealSuccess)
                {
                    if (Gold + DealGolds >= nGold)
                    {
                        Gold = Gold + DealGolds - nGold;
                        DealGolds = nGold;
                        SendDefMessage(Grobal2.SM_DEALCHGGOLD_OK, DealGolds, HUtil32.LoWord(Gold), HUtil32.HiWord(Gold), 0, "");
                        (DealCreat as PlayObject).SendDefMessage(Grobal2.SM_DEALREMOTECHGGOLD, DealGolds, 0, 0, 0, "");
                        DealCreat.DealLastTick = HUtil32.GetTickCount();
                        bo09 = true;
                        DealLastTick = HUtil32.GetTickCount();
                    }
                }
            }
            if (!bo09)
            {
                SendDefMessage(Grobal2.SM_DEALCHGGOLD_FAIL, DealGolds, HUtil32.LoWord(Gold), HUtil32.HiWord(Gold), 0, "");
            }
        }

        private void ClientDealEnd()
        {
            DealSuccess = true;
            if (DealCreat == null)
            {
                return;
            }
            if (((HUtil32.GetTickCount() - DealLastTick) < M2Share.Config.dwDealOKTime) || ((HUtil32.GetTickCount() - DealCreat.DealLastTick) < M2Share.Config.dwDealOKTime))
            {
                SysMsg(M2Share.g_sDealOKTooFast, MsgColor.Red, MsgType.Hint);
                DealCancel();
                return;
            }
            if (DealCreat.DealSuccess)
            {
                var bo11 = true;
                if (Grobal2.MAXBAGITEM - ItemList.Count < DealCreat.DealItemList.Count)
                {
                    bo11 = false;
                    SysMsg(M2Share.g_sYourBagSizeTooSmall, MsgColor.Red, MsgType.Hint);
                }
                if (GoldMax - Gold < DealCreat.DealGolds)
                {
                    SysMsg(M2Share.g_sYourGoldLargeThenLimit, MsgColor.Red, MsgType.Hint);
                    bo11 = false;
                }
                if (Grobal2.MAXBAGITEM - DealCreat.ItemList.Count < DealItemList.Count)
                {
                    SysMsg(M2Share.g_sDealHumanBagSizeTooSmall, MsgColor.Red, MsgType.Hint);
                    bo11 = false;
                }
                if (DealCreat.GoldMax - DealCreat.Gold < DealGolds)
                {
                    SysMsg(M2Share.g_sDealHumanGoldLargeThenLimit, MsgColor.Red, MsgType.Hint);
                    bo11 = false;
                }
                if (bo11)
                {
                    TUserItem userItem;
                    StdItem stdItem;
                    for (var i = 0; i < DealItemList.Count; i++)
                    {
                        userItem = DealItemList[i];
                        DealCreat.AddItemToBag(userItem);
                        (DealCreat as PlayObject).SendAddItem(userItem);
                        stdItem = M2Share.UserEngine.GetStdItem(userItem.wIndex);
                        if (stdItem != null)
                        {
                            if (!M2Share.IsCheapStuff(stdItem.StdMode))
                            {
                                if (stdItem.NeedIdentify == 1)
                                {
                                    M2Share.AddGameDataLog('8' + "\t" + MapName + "\t" + CurrX + "\t" + CurrY + "\t" + CharName + "\t" + stdItem.Name + "\t" + userItem.MakeIndex + "\t" + '1' + "\t" + DealCreat.CharName);
                                }
                            }
                        }
                    }
                    if (DealGolds > 0)
                    {
                        DealCreat.Gold += DealGolds;
                        DealCreat.GoldChanged();
                        if (M2Share.g_boGameLogGold)
                        {
                            M2Share.AddGameDataLog('8' + "\t" + MapName + "\t" + CurrX + "\t" + CurrY + "\t" + CharName + "\t" + Grobal2.sSTRING_GOLDNAME + "\t" + Gold + "\t" + '1' + "\t" + DealCreat.CharName);
                        }
                    }
                    for (var i = 0; i < DealCreat.DealItemList.Count; i++)
                    {
                        userItem = DealCreat.DealItemList[i];
                        AddItemToBag(userItem);
                        this.SendAddItem(userItem);
                        stdItem = M2Share.UserEngine.GetStdItem(userItem.wIndex);
                        if (stdItem != null)
                        {
                            if (!M2Share.IsCheapStuff(stdItem.StdMode))
                            {
                                if (stdItem.NeedIdentify == 1)
                                {
                                    M2Share.AddGameDataLog('8' + "\t" + DealCreat.MapName + "\t" + DealCreat.CurrX + "\t" + DealCreat.CurrY + "\t" + DealCreat.CharName + "\t" + stdItem.Name + "\t" + userItem.MakeIndex + "\t" + '1' + "\t" + CharName);
                                }
                            }
                        }
                    }
                    if (DealCreat.DealGolds > 0)
                    {
                        Gold += DealCreat.DealGolds;
                        GoldChanged();
                        if (M2Share.g_boGameLogGold)
                        {
                            M2Share.AddGameDataLog('8' + "\t" + DealCreat.MapName + "\t" + DealCreat.CurrX + "\t" + DealCreat.CurrY + "\t" + DealCreat.CharName + "\t" + Grobal2.sSTRING_GOLDNAME + "\t" + DealCreat.Gold + "\t" + '1' + "\t" + CharName);
                        }
                    }
                    var playObject = DealCreat as PlayObject;
                    playObject.SendDefMessage(Grobal2.SM_DEALSUCCESS, 0, 0, 0, 0, "");
                    playObject.SysMsg(M2Share.g_sDealSuccessMsg, MsgColor.Green, MsgType.Hint);
                    playObject.DealCreat = null;
                    playObject.Dealing = false;
                    playObject.DealItemList.Clear();
                    playObject.DealGolds = 0;
                    playObject.DealSuccess = false;
                    SendDefMessage(Grobal2.SM_DEALSUCCESS, 0, 0, 0, 0, "");
                    SysMsg(M2Share.g_sDealSuccessMsg, MsgColor.Green, MsgType.Hint);
                    DealCreat = null;
                    Dealing = false;
                    DealItemList.Clear();
                    DealGolds = 0;
                    DealSuccess = false;
                }
                else
                {
                    DealCancel();
                }
            }
            else
            {
                SysMsg(M2Share.g_sYouDealOKMsg, MsgColor.Green, MsgType.Hint);
                DealCreat.SysMsg(M2Share.g_sPoseDealOKMsg, MsgColor.Green, MsgType.Hint);
            }
        }

        private void ClientGetMinMap()
        {
            var nMinMap = Envir.MinMap;
            if (nMinMap > 0)
            {
                SendDefMessage(Grobal2.SM_READMINIMAP_OK, 0, (short)nMinMap, 0, 0, "");
            }
            else
            {
                SendDefMessage(Grobal2.SM_READMINIMAP_FAIL, 0, 0, 0, 0, "");
            }
        }

        private void ClientMakeDrugItem(int objectId, string nItemName)
        {
            var merchant = (Merchant)M2Share.UserEngine.FindMerchant(objectId);
            if (merchant == null || !merchant.m_boMakeDrug)
            {
                return;
            }
            if (merchant.Envir == Envir && Math.Abs(merchant.CurrX - CurrX) < 15 && Math.Abs(merchant.CurrY - CurrY) < 15)
            {
                merchant.ClientMakeDrugItem(this, nItemName);
            }
        }

        private void ClientOpenGuildDlg()
        {
            string sC;
            if (MyGuild != null)
            {
                sC = MyGuild.sGuildName + '\r' + ' ' + '\r';
                if (GuildRankNo == 1)
                {
                    sC = sC + '1' + '\r';
                }
                else
                {
                    sC = sC + '0' + '\r';
                }
                sC = sC + "<Notice>" + '\r';
                for (var I = 0; I < MyGuild.NoticeList.Count; I++)
                {
                    if (sC.Length > 5000)
                    {
                        break;
                    }
                    sC = sC + MyGuild.NoticeList[I] + '\r';
                }
                sC = sC + "<KillGuilds>" + '\r';
                for (var I = 0; I < MyGuild.GuildWarList.Count; I++)
                {
                    if (sC.Length > 5000)
                    {
                        break;
                    }
                    sC = sC + MyGuild.GuildWarList[I] + '\r';
                }
                sC = sC + "<AllyGuilds>" + '\r';
                for (var i = 0; i < MyGuild.GuildAllList.Count; i++)
                {
                    if (sC.Length > 5000)
                    {
                        break;
                    }
                    sC = sC + MyGuild.GuildAllList[i] + '\r';
                }
                m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_OPENGUILDDLG, 0, 0, 0, 1);
                SendSocket(m_DefMsg, EDcode.EncodeString(sC));
            }
            else
            {
                SendDefMessage(Grobal2.SM_OPENGUILDDLG_FAIL, 0, 0, 0, 0, "");
            }
        }

        private void ClientGuildHome()
        {
            ClientOpenGuildDlg();
        }

        private void ClientGuildMemberList()
        {
            var sSendMsg = string.Empty;
            if (MyGuild == null)
            {
                return;
            }
            for (var i = 0; i < MyGuild.m_RankList.Count; i++)
            {
                var guildRank = MyGuild.m_RankList[i];
                sSendMsg = sSendMsg + '#' + guildRank.nRankNo + "/*" + guildRank.sRankName + '/';
                for (var j = 0; j < guildRank.MemberList.Count; j++)
                {
                    if (sSendMsg.Length > 5000)
                    {
                        break;
                    }
                    sSendMsg = sSendMsg + guildRank.MemberList[j].sMemberName + '/';
                }
            }
            m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_SENDGUILDMEMBERLIST, 0, 0, 0, 1);
            SendSocket(m_DefMsg, EDcode.EncodeString(sSendMsg));
        }

        private void ClientGuildAddMember(string sHumName)
        {
            var nC = 1; // '你没有权利使用这个命令。'
            if (IsGuildMaster())
            {
                var playObject = M2Share.UserEngine.GetPlayObject(sHumName);
                if (playObject != null)
                {
                    if (playObject.GetPoseCreate() == this)
                    {
                        if (playObject.AllowGuild)
                        {
                            if (!MyGuild.IsMember(sHumName))
                            {
                                if (playObject.MyGuild == null && MyGuild.m_RankList.Count < 400)
                                {
                                    MyGuild.AddMember(playObject);
                                    M2Share.UserEngine.SendServerGroupMsg(Grobal2.SS_207, M2Share.ServerIndex, MyGuild.sGuildName);
                                    playObject.MyGuild = MyGuild;
                                    playObject.GuildRankName = MyGuild.GetRankName(playObject, ref playObject.GuildRankNo);
                                    playObject.RefShowName();
                                    playObject.SysMsg("你已加入行会: " + MyGuild.sGuildName + " 当前封号为: " + playObject.GuildRankName, MsgColor.Green, MsgType.Hint);
                                    nC = 0;
                                }
                                else
                                {
                                    nC = 4; // '对方已经加入其他行会。'
                                }
                            }
                            else
                            {
                                nC = 3; // '对方已经加入我们的行会。'
                            }
                        }
                        else
                        {
                            nC = 5; // '对方不允许加入行会。'
                            playObject.SysMsg("你拒绝加入行会。 [允许命令为 @" + M2Share.g_GameCommand.LETGUILD.sCmd + ']', MsgColor.Red, MsgType.Hint);
                        }
                    }
                    else
                    {
                        nC = 2; // '想加入进来的成员应该来面对掌门人。'
                    }
                }
                else
                {
                    nC = 2;
                }
            }
            if (nC == 0)
            {
                SendDefMessage(Grobal2.SM_GUILDADDMEMBER_OK, 0, 0, 0, 0, "");
            }
            else
            {
                SendDefMessage(Grobal2.SM_GUILDADDMEMBER_FAIL, nC, 0, 0, 0, "");
            }
        }

        private void ClientGuildDelMember(string sHumName)
        {
            var nC = 1;
            if (IsGuildMaster())
            {
                if (MyGuild.IsMember(sHumName))
                {
                    if (CharName != sHumName)
                    {
                        if (MyGuild.DelMember(sHumName))
                        {
                            var playObject = M2Share.UserEngine.GetPlayObject(sHumName);
                            if (playObject != null)
                            {
                                playObject.MyGuild = null;
                                playObject.RefRankInfo(0, "");
                                playObject.RefShowName();
                            }
                            M2Share.UserEngine.SendServerGroupMsg(Grobal2.SS_207, M2Share.ServerIndex, MyGuild.sGuildName);
                            nC = 0;
                        }
                        else
                        {
                            nC = 4;
                        }
                    }
                    else
                    {
                        nC = 3;
                        var s14 = MyGuild.sGuildName;
                        if (MyGuild.CancelGuld(sHumName))
                        {
                            M2Share.GuildMgr.DelGuild(s14);
                            M2Share.UserEngine.SendServerGroupMsg(Grobal2.SS_206, M2Share.ServerIndex, s14);
                            MyGuild = null;
                            RefRankInfo(0, "");
                            RefShowName();
                            SysMsg("行会" + s14 + "已被取消!!!", MsgColor.Red, MsgType.Hint);
                            nC = 0;
                        }
                    }
                }
                else
                {
                    nC = 2;
                }
            }
            if (nC == 0)
            {
                SendDefMessage(Grobal2.SM_GUILDDELMEMBER_OK, 0, 0, 0, 0, "");
            }
            else
            {
                SendDefMessage(Grobal2.SM_GUILDDELMEMBER_FAIL, nC, 0, 0, 0, "");
            }
        }

        private void ClientGuildUpdateNotice(string sNotict)
        {
            var sC = string.Empty;
            if (MyGuild == null || GuildRankNo != 1)
            {
                return;
            }
            MyGuild.NoticeList.Clear();
            while (!string.IsNullOrEmpty(sNotict))
            {
                sNotict = HUtil32.GetValidStr3(sNotict, ref sC, new[] { "\r" });
                MyGuild.NoticeList.Add(sC);
            }
            MyGuild.SaveGuildInfoFile();
            M2Share.UserEngine.SendServerGroupMsg(Grobal2.SS_207, M2Share.ServerIndex, MyGuild.sGuildName);
            ClientOpenGuildDlg();
        }

        private void ClientGuildUpdateRankInfo(string sRankInfo)
        {
            if (MyGuild == null || GuildRankNo != 1)
            {
                return;
            }
            var nC = MyGuild.UpdateRank(sRankInfo);
            if (nC == 0)
            {
                M2Share.UserEngine.SendServerGroupMsg(Grobal2.SS_207, M2Share.ServerIndex, MyGuild.sGuildName);
                ClientGuildMemberList();
            }
            else
            {
                if (nC <= -2)
                {
                    SendDefMessage(Grobal2.SM_GUILDRANKUPDATE_FAIL, nC, 0, 0, 0, "");
                }
            }
        }

        private void ClientGuildAlly()
        {
            const string sExceptionMsg = "[Exception] TPlayObject::ClientGuildAlly";
            try
            {
                var n8 = -1;
                BaseObject baseObjectC = GetPoseCreate();
                if (baseObjectC != null && baseObjectC.MyGuild != null && baseObjectC.Race == Grobal2.RC_PLAYOBJECT && baseObjectC.GetPoseCreate() == this)
                {
                    if (baseObjectC.MyGuild.m_boEnableAuthAlly)
                    {
                        if (baseObjectC.IsGuildMaster() && IsGuildMaster())
                        {
                            if (MyGuild.IsNotWarGuild(baseObjectC.MyGuild) && baseObjectC.MyGuild.IsNotWarGuild(MyGuild))
                            {
                                MyGuild.AllyGuild(baseObjectC.MyGuild);
                                baseObjectC.MyGuild.AllyGuild(MyGuild);
                                MyGuild.SendGuildMsg(baseObjectC.MyGuild.sGuildName + "行会已经和您的行会联盟成功。");
                                baseObjectC.MyGuild.SendGuildMsg(MyGuild.sGuildName + "行会已经和您的行会联盟成功。");
                                MyGuild.RefMemberName();
                                baseObjectC.MyGuild.RefMemberName();
                                M2Share.UserEngine.SendServerGroupMsg(Grobal2.SS_207, M2Share.ServerIndex, MyGuild.sGuildName);
                                M2Share.UserEngine.SendServerGroupMsg(Grobal2.SS_207, M2Share.ServerIndex, baseObjectC.MyGuild.sGuildName);
                                n8 = 0;
                            }
                            else
                            {
                                n8 = -2;
                            }
                        }
                        else
                        {
                            n8 = -3;
                        }
                    }
                    else
                    {
                        n8 = -4;
                    }
                }
                if (n8 == 0)
                {
                    SendDefMessage(Grobal2.SM_GUILDMAKEALLY_OK, 0, 0, 0, 0, "");
                }
                else
                {
                    SendDefMessage(Grobal2.SM_GUILDMAKEALLY_FAIL, n8, 0, 0, 0, "");
                }
            }
            catch (Exception e)
            {
                M2Share.Log.Error(sExceptionMsg);
                M2Share.Log.Error(e.Message);
            }
        }

        private void ClientGuildBreakAlly(string sGuildName)
        {
            int n10;
            if (!IsGuildMaster())
            {
                return;
            }
            var guild = M2Share.GuildMgr.FindGuild(sGuildName);
            if (guild != null)
            {
                if (MyGuild.IsAllyGuild(guild))
                {
                    MyGuild.DelAllyGuild(guild);
                    guild.DelAllyGuild(MyGuild);
                    MyGuild.SendGuildMsg(guild.sGuildName + " 行会与您的行会解除联盟成功!!!");
                    guild.SendGuildMsg(MyGuild.sGuildName + " 行会解除了与您行会的联盟!!!");
                    MyGuild.RefMemberName();
                    guild.RefMemberName();
                    M2Share.UserEngine.SendServerGroupMsg(Grobal2.SS_207, M2Share.ServerIndex, MyGuild.sGuildName);
                    M2Share.UserEngine.SendServerGroupMsg(Grobal2.SS_207, M2Share.ServerIndex, guild.sGuildName);
                    n10 = 0;
                }
                else
                {
                    n10 = -2;
                }
            }
            else
            {
                n10 = -3;
            }
            if (n10 == 0)
            {
                SendDefMessage(Grobal2.SM_GUILDBREAKALLY_OK, 0, 0, 0, 0, "");
            }
            else
            {
                SendDefMessage(Grobal2.SM_GUILDMAKEALLY_FAIL, 0, 0, 0, 0, "");
            }
        }

        private void ClientQueryRepairCost(int nParam1, int nInt, string sMsg)
        {
            TUserItem userItemA = null;
            string sUserItemName;
            for (var i = 0; i < ItemList.Count; i++)
            {
                var userItem = ItemList[i];
                if (userItem.MakeIndex == nInt)
                {
                    sUserItemName = ItemUnit.GetItemName(userItem); // 取自定义物品名称
                    if (string.Compare(sUserItemName, sMsg, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        userItemA = userItem;
                        break;
                    }
                }
            }
            if (userItemA == null)
            {
                return;
            }
            var merchant = (Merchant)M2Share.UserEngine.FindMerchant(nParam1);
            if (merchant != null && merchant.Envir == Envir && Math.Abs(merchant.CurrX - CurrX) < 15 && Math.Abs(merchant.CurrY - CurrY) < 15)
            {
                merchant.ClientQueryRepairCost(this, userItemA);
            }
        }

        private void ClientRepairItem(int nParam1, int nInt, string sMsg)
        {
            TUserItem userItem = null;
            for (var i = 0; i < ItemList.Count; i++)
            {
                userItem = ItemList[i];
                var sUserItemName = ItemUnit.GetItemName(userItem);
                if (userItem.MakeIndex == nInt && string.Compare(sUserItemName, sMsg, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    break;
                }
            }
            if (userItem == null)
            {
                return;
            }
            Merchant merchant = (Merchant)M2Share.UserEngine.FindMerchant(nParam1);
            if (merchant != null && merchant.Envir == Envir && Math.Abs(merchant.CurrX - CurrX) < 15 && Math.Abs(merchant.CurrY - CurrY) < 15)
            {
                merchant.ClientRepairItem(this, userItem);
            }
        }

        private void ClientStorageItem(int objectId, int nItemIdx, string sMsg)
        {
            var bo19 = false;
            if (sMsg.IndexOf(' ') >= 0)
            {
                HUtil32.GetValidStr3(sMsg, ref sMsg, new[] { " " });
            }
            if (m_nPayMent == 1 && !M2Share.Config.TryModeUseStorage)
            {
                SysMsg(M2Share.g_sTryModeCanotUseStorage, MsgColor.Red, MsgType.Hint);
                return;
            }
            Merchant merchant = (Merchant)M2Share.UserEngine.FindMerchant(objectId);
            for (var i = 0; i < ItemList.Count; i++)
            {
                var userItem = ItemList[i];
                var sUserItemName = ItemUnit.GetItemName(userItem);// 取自定义物品名称
                if (userItem.MakeIndex == nItemIdx && string.Compare(sUserItemName, sMsg, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    // 检查NPC是否允许存物品
                    if (merchant != null && merchant.m_boStorage && (merchant.Envir == Envir && Math.Abs(merchant.CurrX - CurrX) < 15 && Math.Abs(merchant.CurrY - CurrY) < 15 || merchant == M2Share.g_FunctionNPC))
                    {
                        if (StorageItemList.Count < 39)
                        {
                            StorageItemList.Add(userItem);
                            ItemList.RemoveAt(i);
                            WeightChanged();
                            SendDefMessage(Grobal2.SM_STORAGE_OK, 0, 0, 0, 0, "");
                            var stdItem = M2Share.UserEngine.GetStdItem(userItem.wIndex);
                            if (stdItem.NeedIdentify == 1)
                            {
                                M2Share.AddGameDataLog('1' + "\t" + MapName + "\t" + CurrX + "\t" + CurrY + "\t" + CharName + "\t" + stdItem.Name + "\t" + userItem.MakeIndex + "\t" + '1' + "\t" + '0');
                            }
                        }
                        else
                        {
                            SendDefMessage(Grobal2.SM_STORAGE_FULL, 0, 0, 0, 0, "");
                        }
                        bo19 = true;
                    }
                    break;
                }
            }
            if (!bo19)
            {
                SendDefMessage(Grobal2.SM_STORAGE_FAIL, 0, 0, 0, 0, "");
            }
        }

        private void ClientTakeBackStorageItem(int npc, int nItemIdx, string sMsg)
        {
            var bo19 = false;
            var merchant = (Merchant)M2Share.UserEngine.FindMerchant(npc);
            if (merchant == null)
            {
                return;
            }
            if (m_nPayMent == 1 && !M2Share.Config.TryModeUseStorage)
            {
                // '试玩模式不可以使用仓库功能!!!'
                SysMsg(M2Share.g_sTryModeCanotUseStorage, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (!m_boCanGetBackItem)
            {
                SendMsg(merchant, Grobal2.RM_MENU_OK, 0, ObjectId, 0, 0, M2Share.g_sStorageIsLockedMsg + "\\ \\" + "仓库开锁命令: @" + M2Share.g_GameCommand.UNLOCKSTORAGE.sCmd + '\\' + "仓库加锁命令: @" + M2Share.g_GameCommand.__LOCK.sCmd + '\\' + "设置密码命令: @" + M2Share.g_GameCommand.SETPASSWORD.sCmd + '\\' + "修改密码命令: @" + M2Share.g_GameCommand.CHGPASSWORD.sCmd);
                return;
            }
            for (var i = 0; i < StorageItemList.Count; i++)
            {
                var userItem = StorageItemList[i];
                var sUserItemName = ItemUnit.GetItemName(userItem);
                if (userItem.MakeIndex == nItemIdx && string.Compare(sUserItemName, sMsg, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    if (IsAddWeightAvailable(M2Share.UserEngine.GetStdItemWeight(userItem.wIndex)))
                    {
                        // 检查NPC是否允许取物品
                        if (merchant.m_boGetback && (merchant.Envir == Envir && Math.Abs(merchant.CurrX - CurrX) < 15 && Math.Abs(merchant.CurrY - CurrY) < 15 || merchant == M2Share.g_FunctionNPC))
                        {
                            if (AddItemToBag(userItem))
                            {
                                SendAddItem(userItem);
                                StorageItemList.RemoveAt(i);
                                SendDefMessage(Grobal2.SM_TAKEBACKSTORAGEITEM_OK, nItemIdx, 0, 0, 0, "");
                                var stdItem = M2Share.UserEngine.GetStdItem(userItem.wIndex);
                                if (stdItem.NeedIdentify == 1)
                                {
                                    M2Share.AddGameDataLog('0' + "\t" + MapName + "\t" + CurrX + "\t" + CurrY + "\t" + CharName + "\t" + stdItem.Name + "\t" + userItem.MakeIndex + "\t" + '1' + "\t" + '0');
                                }
                            }
                            else
                            {
                                SendDefMessage(Grobal2.SM_TAKEBACKSTORAGEITEM_FULLBAG, 0, 0, 0, 0, "");
                            }
                            bo19 = true;
                        }
                    }
                    else
                    {
                        // '无法携带更多的东西!!!'
                        SysMsg(M2Share.g_sCanotGetItems, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                }
            }
            if (!bo19)
            {
                SendDefMessage(Grobal2.SM_TAKEBACKSTORAGEITEM_FAIL, 0, 0, 0, 0, "");
            }
        }
    }
}