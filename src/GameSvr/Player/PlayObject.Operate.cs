using GameSvr.Actor;
using GameSvr.GameCommand;
using GameSvr.Items;
using GameSvr.Magic;
using GameSvr.Npc;
using SystemModule;
using SystemModule.Consts;
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
                var nameColor = GetChrColor(baseObject);
                var defMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_USERNAME, baseObject.ActorId, nameColor, 0, 0);
                var uname = baseObject.GetShowName();
                SendSocket(defMsg, EDCode.EncodeString(uname));
            }
            else
            {
                SendDefMessage(Grobal2.SM_GHOST, baseObject.ActorId, x, y, 0, "");
            }
        }

        public void ClientQueryBagItems()
        {
            string sSendMsg = string.Empty;
            for (var i = 0; i < ItemList.Count; i++)
            {
                UserItem userItem = ItemList[i];
                StdItem item = M2Share.WorldEngine.GetStdItem(userItem.Index);
                if (item != null)
                {
                    ClientItem clientItem = new ClientItem();
                    item.GetUpgradeStdItem(userItem, ref clientItem);
                    clientItem.Item.Name = CustomItem.GetItemName(userItem);
                    clientItem.Dura = userItem.Dura;
                    clientItem.DuraMax = userItem.DuraMax;
                    clientItem.MakeIndex = userItem.MakeIndex;
                    if (item.StdMode == 50)
                    {
                        clientItem.Item.Name = clientItem.Item.Name + " #" + userItem.Dura;
                    }
                    sSendMsg = sSendMsg + EDCode.EncodeBuffer(clientItem) + '/';
                }
            }
            if (!string.IsNullOrEmpty(sSendMsg))
            {
                m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_BAGITEMS, ActorId, 0, 0, (short)ItemList.Count);
                SendSocket(m_DefMsg, sSendMsg);
            }
        }

        private void ClientQueryUserSet(ProcessMessage processMsg)
        {
            
        }

        private void ClientQueryUserInformation(int charId, int nX, int nY)
        {
            var playObject = (PlayObject)M2Share.ActorMgr.Get(charId);
            if (!CretInNearXY(playObject, nX, nY))
            {
                return;
            }
            UserStateInfo userState = new UserStateInfo();
            userState.Feature = playObject.GetFeature(this);
            userState.UserName = playObject.ChrName;
            userState.NameColor = GetChrColor(playObject);
            if (playObject.MyGuild != null)
            {
                userState.GuildName = playObject.MyGuild.sGuildName;
            }
            userState.GuildRankName = playObject.GuildRankName;
            for (var i = 0; i < playObject.UseItems.Length; i++)
            {
                if (playObject.UseItems[i].Index > 0)
                {
                    StdItem stdItem = M2Share.WorldEngine.GetStdItem(playObject.UseItems[i].Index);
                    if (stdItem == null)
                    {
                        continue;
                    }
                    ClientItem clientItem = new ClientItem();
                    stdItem.GetUpgradeStdItem(playObject.UseItems[i], ref clientItem);
                    clientItem.Item.Name = CustomItem.GetItemName(playObject.UseItems[i]);
                    clientItem.MakeIndex = playObject.UseItems[i].MakeIndex;
                    clientItem.Dura = playObject.UseItems[i].Dura;
                    clientItem.DuraMax = playObject.UseItems[i].DuraMax;

                    if (i == Grobal2.U_DRESS)
                    {
                        playObject.ChangeItemWithLevel(ref clientItem, playObject.Abil.Level);
                    }

                    playObject.ChangeItemByJob(ref clientItem, playObject.Abil.Level);

                    userState.UseItems[i] = clientItem;
                }
            }
            m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_SENDUSERSTATE, 0, 0, 0, 0);
            SendSocket(m_DefMsg, EDCode.EncodeBuffer(userState));
        }

        private void ClientMerchantDlgSelect(int nParam1, string sMsg)
        {
            if (Death || Ghost)
            {
                return;
            }
            NormNpc npc = (NormNpc)M2Share.WorldEngine.FindMerchant(nParam1);
            if (npc == null)
            {
                npc = (NormNpc)M2Share.WorldEngine.FindNpc(nParam1);
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
            UserItem userItem;
            string sUserItemName;
            UserItem userItem18 = null;
            for (var i = 0; i < ItemList.Count; i++)
            {
                userItem = ItemList[i];
                if (userItem.MakeIndex == nMakeIndex)
                {
                    sUserItemName = CustomItem.GetItemName(userItem); // 取自定义物品名称
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
            Merchant merchant = (Merchant)M2Share.WorldEngine.FindMerchant(nParam1);
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
                    var sUserItemName = CustomItem.GetItemName(userItem);
                    if (string.Compare(sUserItemName, sMsg, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        var merchant = (Merchant)M2Share.WorldEngine.FindMerchant(nParam1);
                        if (merchant != null && merchant.m_boSell && merchant.Envir == Envir && Math.Abs(merchant.CurrX - CurrX) < 15 && Math.Abs(merchant.CurrY - CurrY) < 15)
                        {
                            if (merchant.ClientSellItem(this, userItem))
                            {
                                if (userItem.Desc[13] == 1)
                                {
                                    M2Share.CustomItemMgr.DelCustomItemName(userItem.MakeIndex, userItem.Index);
                                    userItem.Desc[13] = 0;
                                }

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
                var merchant = (Merchant)M2Share.WorldEngine.FindMerchant(nParam1);
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
            if (M2Share.Config.InSafeDisableDrop && InSafeZone())
            {
                SendMsg(M2Share.g_ManageNPC, Grobal2.RM_MENU_OK, 0, ActorId, 0, 0, M2Share.g_sCanotDropInSafeZoneMsg);
                return false;
            }
            if (M2Share.Config.ControlDropItem && nGold < M2Share.Config.CanDropGold)
            {
                SendMsg(M2Share.g_ManageNPC, Grobal2.RM_MENU_OK, 0, ActorId, 0, 0, M2Share.g_sCanotDropGoldMsg);
                return false;
            }
            if (!m_boCanDrop || Envir.Flag.boNOTHROWITEM)
            {
                SendMsg(M2Share.g_ManageNPC, Grobal2.RM_MENU_OK, 0, ActorId, 0, 0, M2Share.g_sCanotDropItemMsg);
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
            if (M2Share.Config.InSafeDisableDrop && InSafeZone())
            {
                SendMsg(M2Share.g_ManageNPC, Grobal2.RM_MENU_OK, 0, ActorId, 0, 0, M2Share.g_sCanotDropInSafeZoneMsg);
                return false;
            }
            if (!m_boCanDrop || Envir.Flag.boNOTHROWITEM)
            {
                SendMsg(M2Share.g_ManageNPC, Grobal2.RM_MENU_OK, 0, ActorId, 0, 0, M2Share.g_sCanotDropItemMsg);
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
                        var stdItem = M2Share.WorldEngine.GetStdItem(userItem.Index);
                        if (stdItem == null)
                        {
                            continue;
                        }
                        var sUserItemName = CustomItem.GetItemName(userItem);
                        if (string.Compare(sUserItemName, sItemName, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (M2Share.Config.ControlDropItem && stdItem.Price < M2Share.Config.CanDropPrice)
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
            if (Death || StatusArr[StatuStateConst.POISON_STONE] != 0)// 防麻
            {
                return false;
            }
            if (!CheckActionStatus(wIdent, ref dwDelayTime))
            {
                m_boFilterAction = false;
                return false;
            }
            m_boFilterAction = true;
            if (!M2Share.Config.CloseSpeedHackCheck)
            {
                var dwCheckTime = HUtil32.GetTickCount() - m_dwTurnTick;
                if (dwCheckTime < M2Share.Config.TurnIntervalTime)
                {
                    dwDelayTime = M2Share.Config.TurnIntervalTime - dwCheckTime;
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
            if (Death || StatusArr[StatuStateConst.POISON_STONE] != 0)// 防麻
            {
                return false;
            }
            if (!M2Share.Config.CloseSpeedHackCheck)
            {
                var dwCheckTime = HUtil32.GetTickCount() - m_dwTurnTick;
                if (dwCheckTime < M2Share.Config.TurnIntervalTime)
                {
                    dwDelayTime = M2Share.Config.TurnIntervalTime - dwCheckTime;
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
            if (castle == null || castle.DoorStatus != door.Status || Race != ActorRace.Play || castle.CheckInPalace(CurrX, CurrY, this))
            {
                M2Share.WorldEngine.OpenDoor(Envir, nX, nY);
            }
        }

        private void ClientTakeOnItems(byte btWhere, int nItemIdx, string sItemName)
        {
            var itemIndex = -1;
            var n18 = 0;
            UserItem userItem = null;
            StdItem stdItem = null;
            ClientItem clientItem = null;
            for (var i = 0; i < ItemList.Count; i++)
            {
                userItem = ItemList[i];
                if (userItem != null && userItem.MakeIndex == nItemIdx)
                {
                    stdItem = M2Share.WorldEngine.GetStdItem(userItem.Index);
                    var sUserItemName = CustomItem.GetItemName(userItem);
                    if (stdItem != null)
                    {
                        if (string.Compare(sUserItemName, sItemName, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            itemIndex = i;
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
                    stdItem.GetUpgradeStdItem(userItem, ref clientItem);
                    clientItem.Item.Name = CustomItem.GetItemName(userItem);
                    if (CheckTakeOnItems(btWhere, ref clientItem) && CheckItemBindUse(userItem))
                    {
                        UserItem takeOffItem = null;
                        if (btWhere <= 12)
                        {
                            if (UseItems[btWhere] != null && UseItems[btWhere].Index > 0)
                            {
                                var stdItem20 = M2Share.WorldEngine.GetStdItem(UseItems[btWhere].Index);
                                if (stdItem20 != null && M2Share.StdModeMap.Contains(stdItem20.StdMode))
                                {
                                    if (!UserUnLockDurg && UseItems[btWhere].Desc[7] != 0)
                                    {
                                        SysMsg(M2Share.g_sCanotTakeOffItem, MsgColor.Red, MsgType.Hint);
                                        n18 = -4;
                                        goto FailExit;
                                    }
                                }
                                if (!UserUnLockDurg && (stdItem20.ItemDesc & 2) != 0)
                                {
                                    SysMsg(M2Share.g_sCanotTakeOffItem, MsgColor.Red, MsgType.Hint);
                                    n18 = -4;
                                    goto FailExit;
                                }
                                if ((stdItem20.ItemDesc & 4) != 0)
                                {
                                    SysMsg(M2Share.g_sCanotTakeOffItem, MsgColor.Red, MsgType.Hint);
                                    n18 = -4;
                                    goto FailExit;
                                }
                                if (M2Share.InDisableTakeOffList(UseItems[btWhere].Index))
                                {
                                    SysMsg(M2Share.g_sCanotTakeOffItem, MsgColor.Red, MsgType.Hint);
                                    goto FailExit;
                                }
                                takeOffItem = UseItems[btWhere];
                            }
                            if (M2Share.StdModeMap.Contains(stdItem.StdMode) && userItem.Desc[8] != 0)
                            {
                                userItem.Desc[8] = 0;
                            }
                            UseItems[btWhere] = userItem;
                            DelBagItem(itemIndex);
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

                            if ((stdItem.StdMode == ItemShapeConst.DRESS_STDMODE_MAN) || (stdItem.StdMode == ItemShapeConst.DRESS_STDMODE_WOMAN))
                            {
                                if (stdItem.Shape == ItemShapeConst.DRESS_SHAPE_WING)
                                {
                                    SendUpdateItemWithLevel(UseItems[btWhere], Abil.Level);
                                }
                                else if (stdItem.Shape == DragonConst.DRAGON_DRESS_SHAPE)
                                {
                                    SendUpdateItemByJob(UseItems[btWhere], Abil.Level);
                                }
                                else if (stdItem.Shape == ItemShapeConst.DRESS_SHAPE_PBKING)
                                {
                                    SendUpdateItemByJob(UseItems[btWhere], Abil.Level);
                                }
                                else
                                {
                                    SendUpdateItem(UseItems[btWhere]);
                                }
                            }
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
                if (UseItems[btWhere].Index > 0)
                {
                    if (UseItems[btWhere].MakeIndex == nItemIdx)
                    {
                        var stdItem = M2Share.WorldEngine.GetStdItem(UseItems[btWhere].Index);
                        if (stdItem != null && M2Share.StdModeMap.Contains(stdItem.StdMode))
                        {
                            if (!UserUnLockDurg && UseItems[btWhere].Desc[7] != 0)
                            {
                                SysMsg(M2Share.g_sCanotTakeOffItem, MsgColor.Red, MsgType.Hint);
                                n10 = -4;
                                goto FailExit;
                            }
                        }
                        if (!UserUnLockDurg && (stdItem.ItemDesc & 2) != 0)
                        {
                            SysMsg(M2Share.g_sCanotTakeOffItem, MsgColor.Red, MsgType.Hint);
                            n10 = -4;
                            goto FailExit;
                        }
                        if ((stdItem.ItemDesc & 4) != 0)
                        {
                            SysMsg(M2Share.g_sCanotTakeOffItem, MsgColor.Red, MsgType.Hint);
                            n10 = -4;
                            goto FailExit;
                        }
                        if (M2Share.InDisableTakeOffList(UseItems[btWhere].Index))
                        {
                            SysMsg(M2Share.g_sCanotTakeOffItem, MsgColor.Red, MsgType.Hint);
                            goto FailExit;
                        }
                        var sUserItemName = CustomItem.GetItemName(UseItems[btWhere]);// 取自定义物品名称
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

        private string ClientUseItemsGetUnbindItemName(int nShape)
        {
            if (M2Share.g_UnbindList.TryGetValue(nShape, out var result))
            {
                return result;
            }
            return string.Empty;
        }

        private bool ClientUseItemsGetUnBindItems(string sItemName, int nCount)
        {
            var result = false;
            for (var i = 0; i < nCount; i++)
            {
                var userItem = new UserItem();
                if (M2Share.WorldEngine.CopyToUserItemFromName(sItemName, ref userItem))
                {
                    ItemList.Add(userItem);
                    if (Race == ActorRace.Play)
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
            int itemIndex = 0;
            if (m_boCanUseItem)
            {
                if (!Death)
                {
                    for (var i = 0; i < ItemList.Count; i++)
                    {
                        var userItem = ItemList[i];
                        if (userItem != null && userItem.MakeIndex == nItemIdx)
                        {
                            itemIndex = userItem.MakeIndex;
                            stdItem = M2Share.WorldEngine.GetStdItem(userItem.Index);
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
                                            if (MagicArr[MagicConst.SKILL_ERGUM] != null && !UseThrusting)
                                            {
                                                ThrustingOnOff(true);
                                                SendSocket("+LNG");
                                            }
                                            if (MagicArr[MagicConst.SKILL_BANWOL] != null && !UseHalfMoon)
                                            {
                                                HalfMoonOnOff(true);
                                                SendSocket("+WID");
                                            }
                                            if (MagicArr[MagicConst.SKILL_REDBANWOL] != null && !RedUseHalfMoon)
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
                                                ClientUseItemsGetUnBindItems(ClientUseItemsGetUnbindItemName(stdItem.Shape), 6);
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
                SendMsg(M2Share.g_ManageNPC, Grobal2.RM_MENU_OK, 0, ActorId, 0, 0, M2Share.g_sCanotUseItemMsg);
            }
            if (boEatOk)
            {
                WeightChanged();
                SendDefMessage(Grobal2.SM_EAT_OK, 0, 0, 0, 0, "");
                if (stdItem.NeedIdentify == 1)
                {
                    M2Share.EventSource.AddEventLog(11, MapName + "\t" + CurrX + "\t" + CurrY + "\t" + ChrName + "\t" + stdItem.Name + "\t" + itemIndex + "\t" + '1' + "\t" + '0');
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
            if (!M2Share.Config.CloseSpeedHackCheck)
            {
                var dwCheckTime = HUtil32.GetTickCount() - m_dwTurnTick;
                if (dwCheckTime < HUtil32._MAX(150, M2Share.Config.TurnIntervalTime - 150))
                {
                    dwDelayTime = HUtil32._MAX(150, M2Share.Config.TurnIntervalTime - 150) - dwCheckTime;
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
                            if (baseObject.Race >= ActorRace.Animal && baseObject.Race < ActorRace.Monster)
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

        private void ClientChangeMagicKey(int nSkillIdx, char nKey)
        {
            for (var i = 0; i < MagicList.Count; i++)
            {
                var userMagic = MagicList[i];
                if (userMagic.Magic.MagicId == nSkillIdx)
                {
                    userMagic.Key = nKey;
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
            var playObject = M2Share.WorldEngine.GetPlayObject(sHumName);
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
            var playObject = M2Share.WorldEngine.GetPlayObject(sHumName);
            if (GroupOwner != this)
            {
                SendDefMessage(Grobal2.SM_GROUPADDMEM_FAIL, -1, 0, 0, 0, "");
                return;
            }
            if (GroupMembers.Count > M2Share.Config.GroupMembersMax)
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
            var playObject = M2Share.WorldEngine.GetPlayObject(sHumName);
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
            if (M2Share.Config.DisableDeal)
            {
                SendMsg(M2Share.g_ManageNPC, Grobal2.RM_MENU_OK, 0, ActorId, 0, 0, M2Share.g_sDisableDealItemsMsg);
                return;
            }
            if (Dealing)
            {
                return;
            }
            if ((HUtil32.GetTickCount() - DealLastTick) < M2Share.Config.TryDealTime)
            {
                SendMsg(M2Share.g_ManageNPC, Grobal2.RM_MENU_OK, 0, ActorId, 0, 0, M2Share.g_sPleaseTryDealLaterMsg);
                return;
            }
            if (!m_boCanDeal)
            {
                SendMsg(M2Share.g_ManageNPC, Grobal2.RM_MENU_OK, 0, ActorId, 0, 0, M2Share.g_sCanotTryDealMsg);
                return;
            }
            var targetPlayObject = (PlayObject)GetPoseCreate();
            if (targetPlayObject != null && targetPlayObject != this)
            {
                if (targetPlayObject.GetPoseCreate() == this && !targetPlayObject.Dealing)
                {
                    if (targetPlayObject.Race == ActorRace.Play)
                    {
                        if (targetPlayObject.AllowDeal && targetPlayObject.m_boCanDeal)
                        {
                            targetPlayObject.SysMsg(ChrName + M2Share.g_sOpenedDealMsg, MsgColor.Green, MsgType.Hint);
                            SysMsg(targetPlayObject.ChrName + M2Share.g_sOpenedDealMsg, MsgColor.Green, MsgType.Hint);
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
            var dealSuccess = false;
            if (!DealCreat.DealSuccess)
            {
                for (var i = 0; i < ItemList.Count; i++)
                {
                    var userItem = ItemList[i];
                    if (userItem.MakeIndex == nItemIdx)
                    {
                        var sUserItemName = CustomItem.GetItemName(userItem);
                        if (string.Compare(sUserItemName, sItemName, StringComparison.OrdinalIgnoreCase) == 0 && DealItemList.Count < 12)
                        {
                            DealItemList.Add(userItem);
                            this.SendAddDealItem(userItem);
                            ItemList.RemoveAt(i);
                            dealSuccess = true;
                            break;
                        }
                    }
                }
            }
            if (!dealSuccess)
            {
                SendDefMessage(Grobal2.SM_DEALADDITEM_FAIL, 0, 0, 0, 0, "");
            }
        }

        private void ClientDelDealItem(int nItemIdx, string sItemName)
        {
            if (M2Share.Config.CanNotGetBackDeal)
            {
                SendMsg(M2Share.g_ManageNPC, Grobal2.RM_MENU_OK, 0, ActorId, 0, 0, M2Share.g_sDealItemsDenyGetBackMsg);
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
                        var sUserItemName = CustomItem.GetItemName(userItem);
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
            if (DealGolds > 0 && M2Share.Config.CanNotGetBackDeal)// 禁止取回放入交易栏内的金币
            {
                SendMsg(M2Share.g_ManageNPC, Grobal2.RM_MENU_OK, 0, ActorId, 0, 0, M2Share.g_sDealItemsDenyGetBackMsg);
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
                        DealCreat.SendDefMessage(Grobal2.SM_DEALREMOTECHGGOLD, DealGolds, 0, 0, 0, "");
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
            if (((HUtil32.GetTickCount() - DealLastTick) < M2Share.Config.DealOKTime) || ((HUtil32.GetTickCount() - DealCreat.DealLastTick) < M2Share.Config.DealOKTime))
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
                    UserItem userItem;
                    StdItem stdItem;
                    for (var i = 0; i < DealItemList.Count; i++)
                    {
                        userItem = DealItemList[i];
                        DealCreat.AddItemToBag(userItem);
                        DealCreat.SendAddItem(userItem);
                        stdItem = M2Share.WorldEngine.GetStdItem(userItem.Index);
                        if (stdItem != null)
                        {
                            if (!M2Share.IsCheapStuff(stdItem.StdMode))
                            {
                                if (stdItem.NeedIdentify == 1)
                                {
                                    M2Share.EventSource.AddEventLog(8, MapName + "\t" + CurrX + "\t" + CurrY + "\t" + ChrName + "\t" + stdItem.Name + "\t" + userItem.MakeIndex + "\t" + '1' + "\t" + DealCreat.ChrName);
                                }
                            }
                        }
                    }
                    if (DealGolds > 0)
                    {
                        DealCreat.Gold += DealGolds;
                        DealCreat.GoldChanged();
                        if (M2Share.GameLogGold)
                        {
                            M2Share.EventSource.AddEventLog(8, MapName + "\t" + CurrX + "\t" + CurrY + "\t" + ChrName + "\t" + Grobal2.sSTRING_GOLDNAME + "\t" + Gold + "\t" + '1' + "\t" + DealCreat.ChrName);
                        }
                    }
                    for (var i = 0; i < DealCreat.DealItemList.Count; i++)
                    {
                        userItem = DealCreat.DealItemList[i];
                        AddItemToBag(userItem);
                        this.SendAddItem(userItem);
                        stdItem = M2Share.WorldEngine.GetStdItem(userItem.Index);
                        if (stdItem != null)
                        {
                            if (!M2Share.IsCheapStuff(stdItem.StdMode))
                            {
                                if (stdItem.NeedIdentify == 1)
                                {
                                    M2Share.EventSource.AddEventLog(8, DealCreat.MapName + "\t" + DealCreat.CurrX + "\t" + DealCreat.CurrY + "\t" + DealCreat.ChrName + "\t" + stdItem.Name + "\t" + userItem.MakeIndex + "\t" + '1' + "\t" + ChrName);
                                }
                            }
                        }
                    }
                    if (DealCreat.DealGolds > 0)
                    {
                        Gold += DealCreat.DealGolds;
                        GoldChanged();
                        if (M2Share.GameLogGold)
                        {
                            M2Share.EventSource.AddEventLog(8, DealCreat.MapName + "\t" + DealCreat.CurrX + "\t" + DealCreat.CurrY + "\t" + DealCreat.ChrName + "\t" + Grobal2.sSTRING_GOLDNAME + "\t" + DealCreat.Gold + "\t" + '1' + "\t" + ChrName);
                        }
                    }
                    var playObject = DealCreat;
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
            var merchant = (Merchant)M2Share.WorldEngine.FindMerchant(objectId);
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
                SendSocket(m_DefMsg, EDCode.EncodeString(sC));
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
            SendSocket(m_DefMsg, EDCode.EncodeString(sSendMsg));
        }

        private void ClientGuildAddMember(string sHumName)
        {
            var nC = 1; // '你没有权利使用这个命令。'
            if (IsGuildMaster())
            {
                var playObject = M2Share.WorldEngine.GetPlayObject(sHumName);
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
                                    M2Share.WorldEngine.SendServerGroupMsg(Grobal2.SS_207, M2Share.ServerIndex, MyGuild.sGuildName);
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
                            playObject.SysMsg("你拒绝加入行会。 [允许命令为 @" + CommandMgr.Commands.Letguild.CmdName + ']', MsgColor.Red, MsgType.Hint);
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
                    if (ChrName != sHumName)
                    {
                        if (MyGuild.DelMember(sHumName))
                        {
                            var playObject = M2Share.WorldEngine.GetPlayObject(sHumName);
                            if (playObject != null)
                            {
                                playObject.MyGuild = null;
                                playObject.RefRankInfo(0, "");
                                playObject.RefShowName();
                            }
                            M2Share.WorldEngine.SendServerGroupMsg(Grobal2.SS_207, M2Share.ServerIndex, MyGuild.sGuildName);
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
                            M2Share.WorldEngine.SendServerGroupMsg(Grobal2.SS_206, M2Share.ServerIndex, s14);
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
            M2Share.WorldEngine.SendServerGroupMsg(Grobal2.SS_207, M2Share.ServerIndex, MyGuild.sGuildName);
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
                M2Share.WorldEngine.SendServerGroupMsg(Grobal2.SS_207, M2Share.ServerIndex, MyGuild.sGuildName);
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

        internal void ClientGuildAlly()
        {
            const string sExceptionMsg = "[Exception] TPlayObject::ClientGuildAlly";
            try
            {
                var n8 = -1;
                BaseObject baseObjectC = GetPoseCreate();
                if (baseObjectC != null && baseObjectC.MyGuild != null && baseObjectC.Race == ActorRace.Play && baseObjectC.GetPoseCreate() == this)
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
                                M2Share.WorldEngine.SendServerGroupMsg(Grobal2.SS_207, M2Share.ServerIndex, MyGuild.sGuildName);
                                M2Share.WorldEngine.SendServerGroupMsg(Grobal2.SS_207, M2Share.ServerIndex, baseObjectC.MyGuild.sGuildName);
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

        internal void ClientGuildBreakAlly(string sGuildName)
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
                    M2Share.WorldEngine.SendServerGroupMsg(Grobal2.SS_207, M2Share.ServerIndex, MyGuild.sGuildName);
                    M2Share.WorldEngine.SendServerGroupMsg(Grobal2.SS_207, M2Share.ServerIndex, guild.sGuildName);
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
            UserItem userItemA = null;
            string sUserItemName;
            for (var i = 0; i < ItemList.Count; i++)
            {
                var userItem = ItemList[i];
                if (userItem.MakeIndex == nInt)
                {
                    sUserItemName = CustomItem.GetItemName(userItem); // 取自定义物品名称
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
            var merchant = (Merchant)M2Share.WorldEngine.FindMerchant(nParam1);
            if (merchant != null && merchant.Envir == Envir && Math.Abs(merchant.CurrX - CurrX) < 15 && Math.Abs(merchant.CurrY - CurrY) < 15)
            {
                merchant.ClientQueryRepairCost(this, userItemA);
            }
        }

        private void ClientRepairItem(int nParam1, int nInt, string sMsg)
        {
            UserItem userItem = null;
            for (var i = 0; i < ItemList.Count; i++)
            {
                userItem = ItemList[i];
                var sUserItemName = CustomItem.GetItemName(userItem);
                if (userItem.MakeIndex == nInt && string.Compare(sUserItemName, sMsg, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    break;
                }
            }
            if (userItem == null)
            {
                return;
            }
            Merchant merchant = (Merchant)M2Share.WorldEngine.FindMerchant(nParam1);
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
            Merchant merchant = (Merchant)M2Share.WorldEngine.FindMerchant(objectId);
            for (var i = 0; i < ItemList.Count; i++)
            {
                var userItem = ItemList[i];
                var sUserItemName = CustomItem.GetItemName(userItem);// 取自定义物品名称
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
                            var stdItem = M2Share.WorldEngine.GetStdItem(userItem.Index);
                            if (stdItem.NeedIdentify == 1)
                            {
                                M2Share.EventSource.AddEventLog(1, MapName + "\t" + CurrX + "\t" + CurrY + "\t" + ChrName + "\t" + stdItem.Name + "\t" + userItem.MakeIndex + "\t" + '1' + "\t" + '0');
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
            var merchant = (Merchant)M2Share.WorldEngine.FindMerchant(npc);
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
                SendMsg(merchant, Grobal2.RM_MENU_OK, 0, ActorId, 0, 0, M2Share.g_sStorageIsLockedMsg + "\\ \\" + "仓库开锁命令: @" + CommandMgr.Commands.UnlockStorage.CmdName + '\\' + "仓库加锁命令: @" + CommandMgr.Commands.Lock.CmdName + '\\' + "设置密码命令: @" + CommandMgr.Commands.SetPassword.CmdName + '\\' + "修改密码命令: @" + CommandMgr.Commands.ChgPassword.CmdName);
                return;
            }
            for (var i = 0; i < StorageItemList.Count; i++)
            {
                var userItem = StorageItemList[i];
                var sUserItemName = CustomItem.GetItemName(userItem);
                if (userItem.MakeIndex == nItemIdx && string.Compare(sUserItemName, sMsg, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    if (IsAddWeightAvailable(M2Share.WorldEngine.GetStdItemWeight(userItem.Index)))
                    {
                        // 检查NPC是否允许取物品
                        if (merchant.m_boGetback && (merchant.Envir == Envir && Math.Abs(merchant.CurrX - CurrX) < 15 && Math.Abs(merchant.CurrY - CurrY) < 15 || merchant == M2Share.g_FunctionNPC))
                        {
                            if (AddItemToBag(userItem))
                            {
                                SendAddItem(userItem);
                                StorageItemList.RemoveAt(i);
                                SendDefMessage(Grobal2.SM_TAKEBACKSTORAGEITEM_OK, nItemIdx, 0, 0, 0, "");
                                var stdItem = M2Share.WorldEngine.GetStdItem(userItem.Index);
                                if (stdItem.NeedIdentify == 1)
                                {
                                    M2Share.EventSource.AddEventLog(0, MapName + "\t" + CurrX + "\t" + CurrY + "\t" + ChrName + "\t" + stdItem.Name + "\t" + userItem.MakeIndex + "\t" + '1' + "\t" + '0');
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