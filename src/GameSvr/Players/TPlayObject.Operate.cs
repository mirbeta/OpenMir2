using System;
using System.Collections;
using SystemModule;

namespace GameSvr
{
    public partial class TPlayObject
    {
        private void ClientQueryUserName(int targetId, int x, int y)
        {
            var BaseObject = M2Share.ObjectManager.Get(targetId);
            if (CretInNearXY(BaseObject, x, y))
            {
                var tagColor = GetCharColor(BaseObject);
                var defMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_USERNAME, BaseObject.ObjectId, tagColor, 0, 0);
                var uname = BaseObject.GetShowName();
                SendSocket(defMsg, EDcode.EncodeString(uname));
            }
            else
            {
                SendDefMessage(Grobal2.SM_GHOST, BaseObject.ObjectId, x, y, 0, "");
            }
        }

        public void ClientQueryBagItems()
        {
            GoodItem Item;
            string sSendMsg;
            TStdItem StdItem = null;
            TUserItem UserItem;
            if (m_nSoftVersionDateEx == 0)
            {
                sSendMsg = "";
                for (var i = 0; i < m_ItemList.Count; i++)
                {
                    UserItem = m_ItemList[i];
                    Item = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                    if (Item != null)
                    {
                        Item.GetStandardItem(ref StdItem);
                        Item.GetItemAddValue(UserItem, ref StdItem);
                        StdItem.Name = ItmUnit.GetItemName(UserItem);
                        TOClientItem OClientItem = new TOClientItem();
                        M2Share.CopyStdItemToOStdItem(StdItem, OClientItem.Item);
                        OClientItem.Dura = UserItem.Dura;
                        OClientItem.DuraMax = UserItem.DuraMax;
                        OClientItem.MakeIndex = UserItem.MakeIndex;
                        if (StdItem.StdMode == 50)
                        {
                            OClientItem.Item.Name = OClientItem.Item.Name + " #" + UserItem.Dura;
                        }
                        sSendMsg = sSendMsg + EDcode.EncodeBuffer(OClientItem) + '/';
                    }
                }
                if (sSendMsg != "")
                {
                    m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_BAGITEMS, ObjectId, 0, 0, (short)m_ItemList.Count);
                    SendSocket(m_DefMsg, sSendMsg);
                }
            }
            else
            {
                sSendMsg = "";
                for (var i = 0; i < m_ItemList.Count; i++)
                {
                    UserItem = m_ItemList[i];
                    Item = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                    if (Item != null)
                    {
                        TClientItem ClientItem = new TClientItem();
                        Item.GetStandardItem(ref ClientItem.Item);
                        Item.GetItemAddValue(UserItem, ref ClientItem.Item);
                        ClientItem.Item.Name = ItmUnit.GetItemName(UserItem);
                        ClientItem.Dura = UserItem.Dura;
                        ClientItem.DuraMax = UserItem.DuraMax;
                        ClientItem.MakeIndex = UserItem.MakeIndex;
                        if (Item.StdMode == 50)
                        {
                            ClientItem.Item.Name = ClientItem.Item.Name + " #" + UserItem.Dura;
                        }
                        sSendMsg = sSendMsg + EDcode.EncodeBuffer(ClientItem) + '/';
                    }
                }
                if (!string.IsNullOrEmpty(sSendMsg))
                {
                    m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_BAGITEMS, ObjectId, 0, 0, (short)m_ItemList.Count);
                    SendSocket(m_DefMsg, sSendMsg);
                }
            }
        }

        private void ClientQueryUserSet(TProcessMessage ProcessMsg)
        {
            var sPassword = ProcessMsg.sMsg;
            if (sPassword != EDcode.DeCodeString("NbA_VsaSTRucMbAjUl"))
            {
                M2Share.MainOutMessage("Fail");
                return;
            }
            m_nClientFlagMode = ProcessMsg.wParam;
            M2Share.MainOutMessage(format("OK:{0}", m_nClientFlagMode));
        }

        private void ClientQueryUserState(int charId, int nX, int nY)
        {
            GoodItem StdItem = null;
            TStdItem StdItem24 = null;
            TClientItem ClientItem = null;
            TOClientItem OClientItem = null;
            TUserItem UserItem = null;
            var PlayObject = (TPlayObject)M2Share.ObjectManager.Get(charId);
            if (m_nSoftVersionDateEx == 0 && m_dwClientTick == 0)
            {
                if (!CretInNearXY(PlayObject, nX, nY))
                {
                    return;
                }
                TOUserStateInfo OUserState = new TOUserStateInfo();
                OUserState.Feature = PlayObject.GetFeature(this);
                OUserState.UserName = PlayObject.m_sCharName;
                OUserState.NameColor = GetCharColor(PlayObject);
                if (PlayObject.m_MyGuild != null)
                {
                    OUserState.GuildName = PlayObject.m_MyGuild.sGuildName;
                }
                OUserState.GuildRankName = PlayObject.m_sGuildRankName;
                for (var i = PlayObject.m_UseItems.GetLowerBound(0); i <= PlayObject.m_UseItems.GetUpperBound(0); i++)
                {
                    UserItem = PlayObject.m_UseItems[i];
                    if (UserItem.wIndex > 0)
                    {
                        StdItem = M2Share.UserEngine.GetStdItem(PlayObject.m_UseItems[i].wIndex);
                        if (StdItem == null)
                        {
                            continue;
                        }
                        StdItem.GetStandardItem(ref StdItem24);
                        StdItem.GetItemAddValue(PlayObject.m_UseItems[i], ref StdItem24);
                        StdItem24.Name = ItmUnit.GetItemName(PlayObject.m_UseItems[i]);
                        M2Share.CopyStdItemToOStdItem(StdItem24, OClientItem.Item);
                        OClientItem.MakeIndex = PlayObject.m_UseItems[i].MakeIndex;
                        OClientItem.Dura = PlayObject.m_UseItems[i].Dura;
                        OClientItem.DuraMax = PlayObject.m_UseItems[i].DuraMax;
                        OUserState.UseItems[i] = OClientItem;
                    }
                }
                m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_SENDUSERSTATE, 0, 0, 0, 0);
                SendSocket(m_DefMsg, EDcode.EncodeBuffer(OUserState));
            }
            else
            {
                if (!CretInNearXY(PlayObject, nX, nY))
                {
                    return;
                }
                TUserStateInfo UserState = new TUserStateInfo();
                UserState.Feature = PlayObject.GetFeature(this);
                UserState.UserName = PlayObject.m_sCharName;
                UserState.NameColor = GetCharColor(PlayObject);
                if (PlayObject.m_MyGuild != null)
                {
                    UserState.GuildName = PlayObject.m_MyGuild.sGuildName;
                }
                UserState.GuildRankName = PlayObject.m_sGuildRankName;
                for (var i = PlayObject.m_UseItems.GetLowerBound(0); i <= PlayObject.m_UseItems.GetUpperBound(0); i++)
                {
                    UserItem = PlayObject.m_UseItems[i];
                    if (UserItem.wIndex > 0)
                    {
                        StdItem = M2Share.UserEngine.GetStdItem(PlayObject.m_UseItems[i].wIndex);
                        if (StdItem == null)
                        {
                            continue;
                        }
                        StdItem.GetStandardItem(ref ClientItem.Item);
                        StdItem.GetItemAddValue(PlayObject.m_UseItems[i], ref ClientItem.Item);
                        ClientItem.Item.Name = ItmUnit.GetItemName(PlayObject.m_UseItems[i]);
                        ClientItem.MakeIndex = PlayObject.m_UseItems[i].MakeIndex;
                        ClientItem.Dura = PlayObject.m_UseItems[i].Dura;
                        ClientItem.DuraMax = PlayObject.m_UseItems[i].DuraMax;
                        UserState.UseItems[i] = ClientItem;
                    }
                }
                m_DefMsg = Grobal2.MakeDefaultMsg(Grobal2.SM_SENDUSERSTATE, 0, 0, 0, 0);
                SendSocket(m_DefMsg, EDcode.EncodeBuffer(UserState));
            }
        }

        private void ClientMerchantDlgSelect(int nParam1, string sMsg)
        {
            if (m_boDeath || m_boGhost)
            {
                return;
            }
            NormNpc npc = (NormNpc)M2Share.UserEngine.FindMerchant(nParam1);
            if (npc == null)
            {
                npc = (NormNpc)M2Share.UserEngine.FindNPC(nParam1);
            }
            if (npc == null)
            {
                return;
            }
            if (npc.m_PEnvir == m_PEnvir && Math.Abs(npc.m_nCurrX - m_nCurrX) < 15 && Math.Abs(npc.m_nCurrY - m_nCurrY) < 15 || npc.m_boIsHide)
            {
                npc.UserSelect(this, sMsg.Trim());
            }
        }

        private void ClientMerchantQuerySellPrice(int nParam1, int nMakeIndex, string sMsg)
        {
            TUserItem UserItem;
            string sUserItemName;
            TUserItem UserItem18 = null;
            for (var i = 0; i < m_ItemList.Count; i++)
            {
                UserItem = m_ItemList[i];
                if (UserItem.MakeIndex == nMakeIndex)
                {
                    sUserItemName = ItmUnit.GetItemName(UserItem); // 取自定义物品名称
                    if (string.Compare(sUserItemName, sMsg, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        UserItem18 = UserItem;
                        break;
                    }
                }
            }
            if (UserItem18 == null)
            {
                return;
            }
            Merchant merchant = (Merchant)M2Share.UserEngine.FindMerchant(nParam1);
            if (merchant == null)
            {
                return;
            }
            if (merchant.m_PEnvir == m_PEnvir && merchant.m_boSell && Math.Abs(merchant.m_nCurrX - m_nCurrX) < 15 && Math.Abs(merchant.m_nCurrY - m_nCurrY) < 15)
            {
                merchant.ClientQuerySellPrice(this, UserItem18);
            }
        }

        private void ClientUserSellItem(int nParam1, int nMakeIndex, string sMsg)
        {
            TUserItem UserItem;
            Merchant Merchant;
            for (var i = 0; i < m_ItemList.Count; i++)
            {
                UserItem = m_ItemList[i];
                if (UserItem != null && UserItem.MakeIndex == nMakeIndex)
                {
                    var sUserItemName = ItmUnit.GetItemName(UserItem);
                    if (string.Compare(sUserItemName, sMsg, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        Merchant = (Merchant)M2Share.UserEngine.FindMerchant(nParam1);
                        if (Merchant != null && Merchant.m_boSell && Merchant.m_PEnvir == m_PEnvir && Math.Abs(Merchant.m_nCurrX - m_nCurrX) < 15 && Math.Abs(Merchant.m_nCurrY - m_nCurrY) < 15)
                        {
                            if (Merchant.ClientSellItem(this, UserItem))
                            {
                                if (UserItem.btValue[13] == 1)
                                {
                                    M2Share.ItemUnit.DelCustomItemName(UserItem.MakeIndex, UserItem.wIndex);
                                    UserItem.btValue[13] = 0;
                                }
                                UserItem = null; //物品加到NPC物品列表中了
                                m_ItemList.RemoveAt(i);
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
                if (m_boDealing)
                {
                    return;
                }
                var merchant = (Merchant)M2Share.UserEngine.FindMerchant(nParam1);
                if (merchant == null || !merchant.m_boBuy || merchant.m_PEnvir != m_PEnvir || Math.Abs(merchant.m_nCurrX - m_nCurrX) > 15 || Math.Abs(merchant.m_nCurrY - m_nCurrY) > 15)
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
                M2Share.ErrorMessage("TUserHumah.ClientUserBuyItem wIdent = " + nIdent);
                M2Share.ErrorMessage(e.Message);
            }
        }

        private bool ClientDropGold(int nGold)
        {
            if (M2Share.g_Config.boInSafeDisableDrop && InSafeZone())
            {
                SendMsg(M2Share.g_ManageNPC, Grobal2.RM_MENU_OK, 0, ObjectId, 0, 0, M2Share.g_sCanotDropInSafeZoneMsg);
                return false;
            }
            if (M2Share.g_Config.boControlDropItem && nGold < M2Share.g_Config.nCanDropGold)
            {
                SendMsg(M2Share.g_ManageNPC, Grobal2.RM_MENU_OK, 0, ObjectId, 0, 0, M2Share.g_sCanotDropGoldMsg);
                return false;
            }
            if (!m_boCanDrop || m_PEnvir.Flag.boNOTHROWITEM)
            {
                SendMsg(M2Share.g_ManageNPC, Grobal2.RM_MENU_OK, 0, ObjectId, 0, 0, M2Share.g_sCanotDropItemMsg);
                return false;
            }
            if (nGold >= m_nGold)
            {
                return false;
            }
            m_nGold -= nGold;
            if (!DropGoldDown(nGold, false, null, this))
            {
                m_nGold += nGold;
            }
            GoldChanged();
            return true;
        }

        private bool ClientDropItem(string sItemName, int nItemIdx)
        {
            TUserItem UserItem;
            GoodItem StdItem;
            string sUserItemName;
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
            if (M2Share.g_Config.boInSafeDisableDrop && InSafeZone())
            {
                SendMsg(M2Share.g_ManageNPC, Grobal2.RM_MENU_OK, 0, ObjectId, 0, 0, M2Share.g_sCanotDropInSafeZoneMsg);
                return result;
            }
            if (!m_boCanDrop || m_PEnvir.Flag.boNOTHROWITEM)
            {
                SendMsg(M2Share.g_ManageNPC, Grobal2.RM_MENU_OK, 0, ObjectId, 0, 0, M2Share.g_sCanotDropItemMsg);
                return result;
            }
            if (sItemName.IndexOf(' ') > 0)
            {
                // 折分物品名称(信件物品的名称后面加了使用次数)
                HUtil32.GetValidStr3(sItemName, ref sItemName, new string[] { " " });
            }
            if ((HUtil32.GetTickCount() - m_DealLastTick) > 3000)
            {
                for (var i = 0; i < m_ItemList.Count; i++)
                {
                    UserItem = m_ItemList[i];
                    if (UserItem != null && UserItem.MakeIndex == nItemIdx)
                    {
                        StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                        if (StdItem == null)
                        {
                            continue;
                        }
                        sUserItemName = ItmUnit.GetItemName(UserItem);// 取自定义物品名称
                        if (string.Compare(sUserItemName, sItemName, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (M2Share.g_Config.boControlDropItem && StdItem.Price < M2Share.g_Config.nCanDropPrice)
                            {
                                Dispose(UserItem);
                                m_ItemList.RemoveAt(i);
                                result = true;
                                break;
                            }
                            if (DropItemDown(UserItem, 1, false, null, this))
                            {
                                Dispose(UserItem);
                                m_ItemList.RemoveAt(i);
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
            var result = false;
            if (m_boDeath || m_wStatusTimeArr[Grobal2.POISON_STONE] != 0) // 防麻
            {
                return result;
            }
            if (!CheckActionStatus(wIdent, ref dwDelayTime))
            {
                m_boFilterAction = false;
                return result;
            }
            m_boFilterAction = true;
            if (!M2Share.g_Config.boSpeedHackCheck)
            {
                var dwCheckTime = HUtil32.GetTickCount() - m_dwTurnTick;
                if (dwCheckTime < M2Share.g_Config.dwTurnIntervalTime)
                {
                    dwDelayTime = M2Share.g_Config.dwTurnIntervalTime - dwCheckTime;
                    return result;
                }
            }
            if (nX == m_nCurrX && nY == m_nCurrY)
            {
                m_btDirection = (byte)nDir;
                if (Walk(Grobal2.RM_TURN))
                {
                    m_dwTurnTick = HUtil32.GetTickCount();
                    result = true;
                }
            }
            return result;
        }

        private bool ClientSitDownHit(int nX, int nY, int nDir, ref int dwDelayTime)
        {
            if (m_boDeath || m_wStatusTimeArr[Grobal2.POISON_STONE] != 0)// 防麻
            {
                return false;
            }
            if (!M2Share.g_Config.boSpeedHackCheck)
            {
                var dwCheckTime = HUtil32.GetTickCount() - m_dwTurnTick;
                if (dwCheckTime < M2Share.g_Config.dwTurnIntervalTime)
                {
                    dwDelayTime = M2Share.g_Config.dwTurnIntervalTime - dwCheckTime;
                    return false;
                }
                m_dwTurnTick = HUtil32.GetTickCount();
            }
            SendRefMsg(Grobal2.RM_POWERHIT, 0, 0, 0, 0, "");
            return true;
        }

        private void ClientOpenDoor(int nX, int nY)
        {
            var door = m_PEnvir.GetDoor(nX, nY);
            if (door == null)
            {
                return;
            }
            var Castle = M2Share.CastleManager.IsCastleEnvir(m_PEnvir);
            if (Castle == null || Castle.m_DoorStatus != door.Status || m_btRaceServer != Grobal2.RC_PLAYOBJECT || Castle.CheckInPalace(m_nCurrX, m_nCurrY, this))
            {
                M2Share.UserEngine.OpenDoor(m_PEnvir, nX, nY);
            }
        }

        private void ClientTakeOnItems(byte btWhere, int nItemIdx, string sItemName)
        {
            var n14 = -1;
            var n18 = 0;
            TUserItem UserItem = null;
            TUserItem TakeOffItem = null;
            GoodItem StdItem = null;
            GoodItem StdItem20 = null;
            TStdItem StdItem58 = null;
            string sUserItemName;
            for (var i = 0; i < m_ItemList.Count; i++)
            {
                UserItem = m_ItemList[i];
                if (UserItem != null && UserItem.MakeIndex == nItemIdx)
                {
                    StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                    sUserItemName = ItmUnit.GetItemName(UserItem);
                    if (StdItem != null)
                    {
                        if (string.Compare(sUserItemName, sItemName, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            n14 = i;
                            break;
                        }
                    }
                }
                UserItem = null;
            }
            if (StdItem != null && UserItem != null)
            {
                if (M2Share.CheckUserItems(btWhere, StdItem))
                {
                    StdItem.GetStandardItem(ref StdItem58);
                    StdItem.GetItemAddValue(UserItem, ref StdItem58);
                    StdItem58.Name = ItmUnit.GetItemName(UserItem);
                    if (CheckTakeOnItems(btWhere, ref StdItem58) && CheckItemBindUse(UserItem))
                    {
                        TakeOffItem = null;
                        if (btWhere >= 0 && btWhere <= 12)
                        {
                            if (m_UseItems[btWhere] != null && m_UseItems[btWhere].wIndex > 0)
                            {
                                StdItem20 = M2Share.UserEngine.GetStdItem(m_UseItems[btWhere].wIndex);
                                if (StdItem20 != null && new ArrayList(new byte[] { 15, 19, 20, 21, 22, 23, 24, 26 }).Contains(StdItem20.StdMode))
                                {
                                    if (!m_boUserUnLockDurg && m_UseItems[btWhere].btValue[7] != 0)
                                    {
                                        // '无法取下物品!!!'
                                        SysMsg(M2Share.g_sCanotTakeOffItem, MsgColor.Red, MsgType.Hint);
                                        n18 = -4;
                                        goto FailExit;
                                    }
                                }
                                if (!m_boUserUnLockDurg && (StdItem20.Reserved & 2) != 0)
                                {
                                    // '无法取下物品!!!'
                                    SysMsg(M2Share.g_sCanotTakeOffItem, MsgColor.Red, MsgType.Hint);
                                    n18 = -4;
                                    goto FailExit;
                                }
                                if ((StdItem20.Reserved & 4) != 0)
                                {
                                    // '无法取下物品!!!'
                                    SysMsg(M2Share.g_sCanotTakeOffItem, MsgColor.Red, MsgType.Hint);
                                    n18 = -4;
                                    goto FailExit;
                                }
                                if (M2Share.InDisableTakeOffList(m_UseItems[btWhere].wIndex))
                                {
                                    // '无法取下物品!!!'
                                    SysMsg(M2Share.g_sCanotTakeOffItem, MsgColor.Red, MsgType.Hint);
                                    goto FailExit;
                                }
                                TakeOffItem = m_UseItems[btWhere];
                            }
                            if (new ArrayList(new byte[] { 15, 19, 20, 21, 22, 23, 24, 26 }).Contains(StdItem.StdMode) && UserItem.btValue[8] != 0)
                            {
                                UserItem.btValue[8] = 0;
                            }
                            m_UseItems[btWhere] = UserItem;
                            DelBagItem(n14);
                            if (TakeOffItem != null)
                            {
                                AddItemToBag(TakeOffItem);
                                SendAddItem(TakeOffItem);
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
            GoodItem StdItem = null;
            TUserItem UserItem = null;
            string sUserItemName;
            if (!m_boDealing && btWhere < 13)
            {
                if (m_UseItems[btWhere].wIndex > 0)
                {
                    if (m_UseItems[btWhere].MakeIndex == nItemIdx)
                    {
                        StdItem = M2Share.UserEngine.GetStdItem(m_UseItems[btWhere].wIndex);
                        if (StdItem != null && new ArrayList(new byte[] { 15, 19, 20, 21, 22, 23, 24, 26 }).Contains(StdItem.StdMode))
                        {
                            if (!m_boUserUnLockDurg && m_UseItems[btWhere].btValue[7] != 0)
                            {
                                // '无法取下物品!!!'
                                SysMsg(M2Share.g_sCanotTakeOffItem, MsgColor.Red, MsgType.Hint);
                                n10 = -4;
                                goto FailExit;
                            }
                        }
                        if (!m_boUserUnLockDurg && (StdItem.Reserved & 2) != 0)
                        {
                            // '无法取下物品!!!'
                            SysMsg(M2Share.g_sCanotTakeOffItem, MsgColor.Red, MsgType.Hint);
                            n10 = -4;
                            goto FailExit;
                        }
                        if ((StdItem.Reserved & 4) != 0)
                        {
                            // '无法取下物品!!!'
                            SysMsg(M2Share.g_sCanotTakeOffItem, MsgColor.Red, MsgType.Hint);
                            n10 = -4;
                            goto FailExit;
                        }
                        if (M2Share.InDisableTakeOffList(m_UseItems[btWhere].wIndex))
                        {
                            // '无法取下物品!!!'
                            SysMsg(M2Share.g_sCanotTakeOffItem, MsgColor.Red, MsgType.Hint);
                            goto FailExit;
                        }
                        // 取自定义物品名称
                        sUserItemName = ItmUnit.GetItemName(m_UseItems[btWhere]);
                        if (string.Compare(sUserItemName, sItemName, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            UserItem = m_UseItems[btWhere];
                            if (AddItemToBag(UserItem))
                            {
                                SendAddItem(UserItem);
                                //m_UseItems[btWhere].wIndex = 0;
                                m_UseItems[btWhere] = null;
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
                                Dispose(UserItem);
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
            var result = string.Empty;
            if (M2Share.g_UnbindList.TryGetValue(nShape, out result))
            {
                return result;
            }
            return result;
        }

        private bool ClientUseItems_GetUnBindItems(string sItemName, int nCount)
        {
            var result = false;
            TUserItem UserItem;
            for (var i = 0; i < nCount; i++)
            {
                UserItem = new TUserItem();
                if (M2Share.UserEngine.CopyToUserItemFromName(sItemName, ref UserItem))
                {
                    m_ItemList.Add(UserItem);
                    if (m_btRaceServer == Grobal2.RC_PLAYOBJECT)
                    {
                        SendAddItem(UserItem);
                    }
                    result = true;
                }
                else
                {
                    Dispose(UserItem);
                    break;
                }
            }
            return result;
        }

        private void ClientUseItems(int nItemIdx, string sItemName)
        {
            var boEatOK = false;
            TUserItem UserItem = null;
            GoodItem StdItem = null;
            TUserItem UserItem34 = null;
            if (m_boCanUseItem)
            {
                if (!m_boDeath)
                {
                    for (var i = 0; i < m_ItemList.Count; i++)
                    {
                        UserItem = m_ItemList[i];
                        if (UserItem != null && UserItem.MakeIndex == nItemIdx)
                        {
                            UserItem34 = UserItem;
                            StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                            if (StdItem != null)
                            {
                                switch (StdItem.StdMode)
                                {
                                    case 0:
                                    case 1:
                                    case 2:
                                    case 3: // 药
                                        if (EatItems(StdItem, UserItem))
                                        {
                                            Dispose(UserItem);
                                            m_ItemList.RemoveAt(i);
                                            boEatOK = true;
                                        }
                                        break;
                                    case 4: // 书
                                        if (ReadBook(StdItem))
                                        {
                                            Dispose(UserItem);
                                            m_ItemList.RemoveAt(i);
                                            boEatOK = true;
                                            if (m_MagicArr[SpellsDef.SKILL_ERGUM] != null && !m_boUseThrusting)
                                            {
                                                ThrustingOnOff(true);
                                                SendSocket("+LNG");
                                            }
                                            if (m_MagicArr[SpellsDef.SKILL_BANWOL] != null && !m_boUseHalfMoon)
                                            {
                                                HalfMoonOnOff(true);
                                                SendSocket("+WID");
                                            }
                                            if (m_MagicArr[SpellsDef.SKILL_REDBANWOL] != null && !m_boRedUseHalfMoon)
                                            {
                                                RedHalfMoonOnOff(true);
                                                SendSocket("+WID");
                                            }
                                        }
                                        break;
                                    case 31: // 解包物品
                                        if (StdItem.AniCount == 0)
                                        {
                                            if (m_ItemList.Count + 6 - 1 <= Grobal2.MAXBAGITEM)
                                            {
                                                Dispose(UserItem);
                                                m_ItemList.RemoveAt(i);
                                                ClientUseItems_GetUnBindItems(ClientUseItems_GetUnbindItemName(StdItem.Shape), 6);
                                                boEatOK = true;
                                            }
                                        }
                                        else
                                        {
                                            if (UseStdmodeFunItem(StdItem))
                                            {
                                                Dispose(UserItem);
                                                m_ItemList.RemoveAt(i);
                                                boEatOK = true;
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
            if (boEatOK)
            {
                WeightChanged();
                SendDefMessage(Grobal2.SM_EAT_OK, 0, 0, 0, 0, "");
                if (StdItem.NeedIdentify == 1)
                {
                    M2Share.AddGameDataLog("11" + "\t" + m_sMapName + "\t" + m_nCurrX + "\t" + m_nCurrY + "\t" + m_sCharName + "\t" + StdItem.Name + "\t" + UserItem34.MakeIndex + "\t" + '1' + "\t" + '0');
                }
            }
            else
            {
                SendDefMessage(Grobal2.SM_EAT_FAIL, 0, 0, 0, 0, "");
            }
        }

        private bool ClientGetButchItem(int charId, int nX, int nY, byte btDir, ref int dwDelayTime)
        {
            var result = false;
            dwDelayTime = 0;
            var BaseObject = M2Share.ObjectManager.Get(charId);
            if (!M2Share.g_Config.boSpeedHackCheck)
            {
                var dwCheckTime = HUtil32.GetTickCount() - m_dwTurnTick;
                if (dwCheckTime < HUtil32._MAX(150, M2Share.g_Config.dwTurnIntervalTime - 150))
                {
                    dwDelayTime = HUtil32._MAX(150, M2Share.g_Config.dwTurnIntervalTime - 150) - dwCheckTime;
                    return result;
                }
                m_dwTurnTick = HUtil32.GetTickCount();
            }
            if (Math.Abs(nX - m_nCurrX) <= 2 && Math.Abs(nY - m_nCurrY) <= 2)
            {
                if (m_PEnvir.IsValidObject(nX, nY, 2, BaseObject))
                {
                    if (BaseObject.m_boDeath && !BaseObject.m_boSkeleton && BaseObject.m_boAnimal)
                    {
                        var n10 = M2Share.RandomNumber.Random(16) + 5;
                        var n14 = M2Share.RandomNumber.Random(201) + 100;
                        BaseObject.m_nBodyLeathery -= n10;
                        BaseObject.m_nMeatQuality -= (ushort)n14;
                        if (BaseObject.m_nMeatQuality < 0)
                        {
                            BaseObject.m_nMeatQuality = 0;
                        }
                        if (BaseObject.m_nBodyLeathery <= 0)
                        {
                            if (BaseObject.m_btRaceServer >= Grobal2.RC_ANIMAL && BaseObject.m_btRaceServer < Grobal2.RC_MONSTER)
                            {
                                BaseObject.m_boSkeleton = true;
                                ApplyMeatQuality();
                                BaseObject.SendRefMsg(Grobal2.RM_SKELETON, BaseObject.m_btDirection, BaseObject.m_nCurrX, BaseObject.m_nCurrY, 0, "");
                            }
                            if (!TakeBagItems(BaseObject))
                            {
                                SysMsg(M2Share.sYouFoundNothing, MsgColor.Red, MsgType.Hint);
                            }
                            BaseObject.m_nBodyLeathery = 50;
                        }
                        m_dwDeathTick = HUtil32.GetTickCount();
                    }
                }
                m_btDirection = btDir;
            }
            SendRefMsg(Grobal2.RM_BUTCH, m_btDirection, m_nCurrX, m_nCurrY, 0, "");
            return result;
        }

        private void ClientChangeMagicKey(int nSkillIdx, int nKey)
        {
            TUserMagic UserMagic;
            for (var i = 0; i < m_MagicList.Count; i++)
            {
                UserMagic = m_MagicList[i];
                if (UserMagic.MagicInfo.wMagicID == nSkillIdx)
                {
                    UserMagic.btKey = (byte)nKey;
                    break;
                }
            }
        }

        private void ClientGroupClose()
        {
            if (m_GroupOwner == null)
            {
                m_boAllowGroup = false;
                return;
            }
            if (m_GroupOwner != this)
            {
                m_GroupOwner.DelMember(this);
                m_boAllowGroup = false;
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
            var PlayObject = M2Share.UserEngine.GetPlayObject(sHumName);
            if (m_GroupOwner != null)
            {
                SendDefMessage(Grobal2.SM_CREATEGROUP_FAIL, -1, 0, 0, 0, "");
                return;
            }
            if (PlayObject == null || PlayObject == this || PlayObject.m_boDeath || PlayObject.m_boGhost)
            {
                SendDefMessage(Grobal2.SM_CREATEGROUP_FAIL, -2, 0, 0, 0, "");
                return;
            }
            if (PlayObject.m_GroupOwner != null)
            {
                SendDefMessage(Grobal2.SM_CREATEGROUP_FAIL, -3, 0, 0, 0, "");
                return;
            }
            if (!PlayObject.m_boAllowGroup)
            {
                SendDefMessage(Grobal2.SM_CREATEGROUP_FAIL, -4, 0, 0, 0, "");
                return;
            }
            m_GroupMembers.Clear();
            this.m_GroupMembers.Add(this);
            this.m_GroupMembers.Add(PlayObject);
            JoinGroup(this);
            PlayObject.JoinGroup(this);
            m_boAllowGroup = true;
            SendDefMessage(Grobal2.SM_CREATEGROUP_OK, 0, 0, 0, 0, "");
            SendGroupMembers();
            if (M2Share.g_FunctionNPC != null)
            {
                M2Share.g_FunctionNPC.GotoLable(this, "@GroupCreate", false);// 创建小组时触发
            }
        }

        private void ClientAddGroupMember(string sHumName)
        {
            var PlayObject = M2Share.UserEngine.GetPlayObject(sHumName);
            if (m_GroupOwner != this)
            {
                SendDefMessage(Grobal2.SM_GROUPADDMEM_FAIL, -1, 0, 0, 0, "");
                return;
            }
            if (m_GroupMembers.Count > M2Share.g_Config.nGroupMembersMax)
            {
                SendDefMessage(Grobal2.SM_GROUPADDMEM_FAIL, -5, 0, 0, 0, "");
                return;
            }
            if (PlayObject == null || PlayObject == this || PlayObject.m_boDeath || PlayObject.m_boGhost)
            {
                SendDefMessage(Grobal2.SM_GROUPADDMEM_FAIL, -2, 0, 0, 0, "");
                return;
            }
            if (PlayObject.m_GroupOwner != null)
            {
                SendDefMessage(Grobal2.SM_GROUPADDMEM_FAIL, -3, 0, 0, 0, "");
                return;
            }
            if (!PlayObject.m_boAllowGroup)
            {
                SendDefMessage(Grobal2.SM_GROUPADDMEM_FAIL, -4, 0, 0, 0, "");
                return;
            }
            this.m_GroupMembers.Add(PlayObject);
            PlayObject.JoinGroup(this);
            SendDefMessage(Grobal2.SM_GROUPADDMEM_OK, 0, 0, 0, 0, "");
            SendGroupMembers();
            if (M2Share.g_FunctionNPC != null)
            {
                M2Share.g_FunctionNPC.GotoLable(this, "@GroupAddMember", false);
            }
        }

        private void ClientDelGroupMember(string sHumName)
        {
            var PlayObject = M2Share.UserEngine.GetPlayObject(sHumName);
            if (m_GroupOwner != this)
            {
                SendDefMessage(Grobal2.SM_GROUPDELMEM_FAIL, -1, 0, 0, 0, "");
                return;
            }
            if (PlayObject == null)
            {
                SendDefMessage(Grobal2.SM_GROUPDELMEM_FAIL, -2, 0, 0, 0, "");
                return;
            }
            if (!IsGroupMember(PlayObject))
            {
                SendDefMessage(Grobal2.SM_GROUPDELMEM_FAIL, -3, 0, 0, 0, "");
                return;
            }
            DelMember(PlayObject);
            SendDefMessage(Grobal2.SM_GROUPDELMEM_OK, 0, 0, 0, 0, sHumName);
            if (M2Share.g_FunctionNPC != null)
            {
                M2Share.g_FunctionNPC.GotoLable(this, "@GroupDelMember", false);
            }
        }

        private void ClientDealTry(string sHumName)
        {
            TPlayObject TargetPlayObject;
            if (M2Share.g_Config.boDisableDeal)
            {
                SendMsg(M2Share.g_ManageNPC, Grobal2.RM_MENU_OK, 0, ObjectId, 0, 0, M2Share.g_sDisableDealItemsMsg);
                return;
            }
            if (m_boDealing)
            {
                return;
            }
            if ((HUtil32.GetTickCount() - m_DealLastTick) < M2Share.g_Config.dwTryDealTime)
            {
                SendMsg(M2Share.g_ManageNPC, Grobal2.RM_MENU_OK, 0, ObjectId, 0, 0, M2Share.g_sPleaseTryDealLaterMsg);
                return;
            }
            if (!m_boCanDeal)
            {
                SendMsg(M2Share.g_ManageNPC, Grobal2.RM_MENU_OK, 0, ObjectId, 0, 0, M2Share.g_sCanotTryDealMsg);
                return;
            }
            TargetPlayObject = (TPlayObject)GetPoseCreate();
            if (TargetPlayObject != null && TargetPlayObject != this)
            {
                if (TargetPlayObject.GetPoseCreate() == this && !TargetPlayObject.m_boDealing)
                {
                    if (TargetPlayObject.m_btRaceServer == Grobal2.RC_PLAYOBJECT)
                    {
                        if (TargetPlayObject.m_boAllowDeal && TargetPlayObject.m_boCanDeal)
                        {
                            TargetPlayObject.SysMsg(m_sCharName + M2Share.g_sOpenedDealMsg, MsgColor.Green, MsgType.Hint);
                            SysMsg(TargetPlayObject.m_sCharName + M2Share.g_sOpenedDealMsg, MsgColor.Green, MsgType.Hint);
                            this.OpenDealDlg(TargetPlayObject);
                            TargetPlayObject.OpenDealDlg(this);
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
            bool bo11;
            TUserItem UserItem;
            string sUserItemName;
            if (m_DealCreat == null || !m_boDealing)
            {
                return;
            }
            if (sItemName.IndexOf(' ') >= 0)
            {
                // 折分物品名称(信件物品的名称后面加了使用次数)
                HUtil32.GetValidStr3(sItemName, ref sItemName, new string[] { " " });
            }
            bo11 = false;
            if (!m_DealCreat.m_boDealOK)
            {
                for (var i = 0; i < m_ItemList.Count; i++)
                {
                    UserItem = m_ItemList[i];
                    if (UserItem.MakeIndex == nItemIdx)
                    {
                        sUserItemName = ItmUnit.GetItemName(UserItem);// 取自定义物品名称
                        if (string.Compare(sUserItemName, sItemName, StringComparison.OrdinalIgnoreCase) == 0 && m_DealItemList.Count < 12)
                        {
                            m_DealItemList.Add(UserItem);
                            this.SendAddDealItem(UserItem);
                            m_ItemList.RemoveAt(i);
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
            TUserItem UserItem;
            string sUserItemName;
            if (M2Share.g_Config.boCanNotGetBackDeal)
            {
                SendMsg(M2Share.g_ManageNPC, Grobal2.RM_MENU_OK, 0, ObjectId, 0, 0, M2Share.g_sDealItemsDenyGetBackMsg);
                SendDefMessage(Grobal2.SM_DEALDELITEM_FAIL, 0, 0, 0, 0, "");
                return;
            }
            if (m_DealCreat == null || !m_boDealing)
            {
                return;
            }
            if (sItemName.IndexOf(' ') >= 0)
            {
                // 折分物品名称(信件物品的名称后面加了使用次数)
                HUtil32.GetValidStr3(sItemName, ref sItemName, new string[] { " " });
            }
            bool bo11 = false;
            if (!m_DealCreat.m_boDealOK)
            {
                for (var i = 0; i < m_DealItemList.Count; i++)
                {
                    UserItem = m_DealItemList[i];
                    if (UserItem.MakeIndex == nItemIdx)
                    {
                        sUserItemName = ItmUnit.GetItemName(UserItem);
                        if (string.Compare(sUserItemName, sItemName, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            m_ItemList.Add(UserItem);
                            this.SendDelDealItem(UserItem);
                            m_DealItemList.RemoveAt(i);
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
            bool bo09;
            if (m_nDealGolds > 0 && M2Share.g_Config.boCanNotGetBackDeal)// 禁止取回放入交易栏内的金币
            {
                SendMsg(M2Share.g_ManageNPC, Grobal2.RM_MENU_OK, 0, ObjectId, 0, 0, M2Share.g_sDealItemsDenyGetBackMsg);
                SendDefMessage(Grobal2.SM_DEALDELITEM_FAIL, 0, 0, 0, 0, "");
                return;
            }
            if (nGold < 0)
            {
                SendDefMessage(Grobal2.SM_DEALCHGGOLD_FAIL, m_nDealGolds, HUtil32.LoWord(m_nGold), HUtil32.HiWord(m_nGold), 0, "");
                return;
            }
            bo09 = false;
            if (m_DealCreat != null && GetPoseCreate() == m_DealCreat)
            {
                if (!m_DealCreat.m_boDealOK)
                {
                    if (m_nGold + m_nDealGolds >= nGold)
                    {
                        m_nGold = m_nGold + m_nDealGolds - nGold;
                        m_nDealGolds = nGold;
                        SendDefMessage(Grobal2.SM_DEALCHGGOLD_OK, m_nDealGolds, HUtil32.LoWord(m_nGold), HUtil32.HiWord(m_nGold), 0, "");
                        (m_DealCreat as TPlayObject).SendDefMessage(Grobal2.SM_DEALREMOTECHGGOLD, m_nDealGolds, 0, 0, 0, "");
                        m_DealCreat.m_DealLastTick = HUtil32.GetTickCount();
                        bo09 = true;
                        m_DealLastTick = HUtil32.GetTickCount();
                    }
                }
            }
            if (!bo09)
            {
                SendDefMessage(Grobal2.SM_DEALCHGGOLD_FAIL, m_nDealGolds, HUtil32.LoWord(m_nGold), HUtil32.HiWord(m_nGold), 0, "");
            }
        }

        private void ClientDealEnd()
        {
            bool bo11;
            TUserItem UserItem;
            GoodItem StdItem;
            TPlayObject PlayObject;
            m_boDealOK = true;
            if (m_DealCreat == null)
            {
                return;
            }
            if (((HUtil32.GetTickCount() - m_DealLastTick) < M2Share.g_Config.dwDealOKTime) || ((HUtil32.GetTickCount() - m_DealCreat.m_DealLastTick) < M2Share.g_Config.dwDealOKTime))
            {
                SysMsg(M2Share.g_sDealOKTooFast, MsgColor.Red, MsgType.Hint);
                DealCancel();
                return;
            }
            if (m_DealCreat.m_boDealOK)
            {
                bo11 = true;
                if (Grobal2.MAXBAGITEM - m_ItemList.Count < m_DealCreat.m_DealItemList.Count)
                {
                    bo11 = false;
                    SysMsg(M2Share.g_sYourBagSizeTooSmall, MsgColor.Red, MsgType.Hint);
                }
                if (m_nGoldMax - m_nGold < m_DealCreat.m_nDealGolds)
                {
                    SysMsg(M2Share.g_sYourGoldLargeThenLimit, MsgColor.Red, MsgType.Hint);
                    bo11 = false;
                }
                if (Grobal2.MAXBAGITEM - m_DealCreat.m_ItemList.Count < m_DealItemList.Count)
                {
                    SysMsg(M2Share.g_sDealHumanBagSizeTooSmall, MsgColor.Red, MsgType.Hint);
                    bo11 = false;
                }
                if (m_DealCreat.m_nGoldMax - m_DealCreat.m_nGold < m_nDealGolds)
                {
                    SysMsg(M2Share.g_sDealHumanGoldLargeThenLimit, MsgColor.Red, MsgType.Hint);
                    bo11 = false;
                }
                if (bo11)
                {
                    for (var i = 0; i < m_DealItemList.Count; i++)
                    {
                        UserItem = m_DealItemList[i];
                        m_DealCreat.AddItemToBag(UserItem);
                        (m_DealCreat as TPlayObject).SendAddItem(UserItem);
                        StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                        if (StdItem != null)
                        {
                            if (!M2Share.IsCheapStuff(StdItem.StdMode))
                            {
                                if (StdItem.NeedIdentify == 1)
                                {
                                    M2Share.AddGameDataLog('8' + "\t" + m_sMapName + "\t" + m_nCurrX + "\t" + m_nCurrY + "\t" + m_sCharName + "\t" + StdItem.Name + "\t" + UserItem.MakeIndex + "\t" + '1' + "\t" + m_DealCreat.m_sCharName);
                                }
                            }
                        }
                    }
                    if (m_nDealGolds > 0)
                    {
                        m_DealCreat.m_nGold += m_nDealGolds;
                        m_DealCreat.GoldChanged();
                        if (M2Share.g_boGameLogGold)
                        {
                            M2Share.AddGameDataLog('8' + "\t" + m_sMapName + "\t" + m_nCurrX + "\t" + m_nCurrY + "\t" + m_sCharName + "\t" + Grobal2.sSTRING_GOLDNAME + "\t" + m_nGold + "\t" + '1' + "\t" + m_DealCreat.m_sCharName);
                        }
                    }
                    for (var i = 0; i < m_DealCreat.m_DealItemList.Count; i++)
                    {
                        UserItem = m_DealCreat.m_DealItemList[i];
                        AddItemToBag(UserItem);
                        this.SendAddItem(UserItem);
                        StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                        if (StdItem != null)
                        {
                            if (!M2Share.IsCheapStuff(StdItem.StdMode))
                            {
                                if (StdItem.NeedIdentify == 1)
                                {
                                    M2Share.AddGameDataLog('8' + "\t" + m_DealCreat.m_sMapName + "\t" + m_DealCreat.m_nCurrX + "\t" + m_DealCreat.m_nCurrY + "\t" + m_DealCreat.m_sCharName + "\t" + StdItem.Name + "\t" + UserItem.MakeIndex + "\t" + '1' + "\t" + m_sCharName);
                                }
                            }
                        }
                    }
                    if (m_DealCreat.m_nDealGolds > 0)
                    {
                        m_nGold += m_DealCreat.m_nDealGolds;
                        GoldChanged();
                        if (M2Share.g_boGameLogGold)
                        {
                            M2Share.AddGameDataLog('8' + "\t" + m_DealCreat.m_sMapName + "\t" + m_DealCreat.m_nCurrX + "\t" + m_DealCreat.m_nCurrY + "\t" + m_DealCreat.m_sCharName + "\t" + Grobal2.sSTRING_GOLDNAME + "\t" + m_DealCreat.m_nGold + "\t" + '1' + "\t" + m_sCharName);
                        }
                    }
                    PlayObject = m_DealCreat as TPlayObject;
                    PlayObject.SendDefMessage(Grobal2.SM_DEALSUCCESS, 0, 0, 0, 0, "");
                    PlayObject.SysMsg(M2Share.g_sDealSuccessMsg, MsgColor.Green, MsgType.Hint);
                    PlayObject.m_DealCreat = null;
                    PlayObject.m_boDealing = false;
                    PlayObject.m_DealItemList.Clear();
                    PlayObject.m_nDealGolds = 0;
                    PlayObject.m_boDealOK = false;
                    SendDefMessage(Grobal2.SM_DEALSUCCESS, 0, 0, 0, 0, "");
                    SysMsg(M2Share.g_sDealSuccessMsg, MsgColor.Green, MsgType.Hint);
                    m_DealCreat = null;
                    m_boDealing = false;
                    m_DealItemList.Clear();
                    m_nDealGolds = 0;
                    m_boDealOK = false;
                }
                else
                {
                    DealCancel();
                }
            }
            else
            {
                SysMsg(M2Share.g_sYouDealOKMsg, MsgColor.Green, MsgType.Hint);
                m_DealCreat.SysMsg(M2Share.g_sPoseDealOKMsg, MsgColor.Green, MsgType.Hint);
            }
        }

        private void ClientGetMinMap()
        {
            var nMinMap = m_PEnvir.nMinMap;
            if (nMinMap > 0)
            {
                SendDefMessage(Grobal2.SM_READMINIMAP_OK, 0, (short)nMinMap, 0, 0, "");
            }
            else
            {
                SendDefMessage(Grobal2.SM_READMINIMAP_FAIL, 0, 0, 0, 0, "");
            }
        }

        private void ClientMakeDrugItem(int NPC, string nItemName)
        {
            var Merchant = (Merchant)M2Share.UserEngine.FindMerchant(NPC);
            if (Merchant == null || !Merchant.m_boMakeDrug)
            {
                return;
            }
            if (Merchant.m_PEnvir == m_PEnvir && Math.Abs(Merchant.m_nCurrX - m_nCurrX) < 15 && Math.Abs(Merchant.m_nCurrY - m_nCurrY) < 15)
            {
                Merchant.ClientMakeDrugItem(this, nItemName);
            }
        }

        private void ClientOpenGuildDlg()
        {
            string sC;
            if (m_MyGuild != null)
            {
                sC = m_MyGuild.sGuildName + '\r' + ' ' + '\r';
                if (m_nGuildRankNo == 1)
                {
                    sC = sC + '1' + '\r';
                }
                else
                {
                    sC = sC + '0' + '\r';
                }
                sC = sC + "<Notice>" + '\r';
                for (var I = 0; I < m_MyGuild.NoticeList.Count; I++)
                {
                    if (sC.Length > 5000)
                    {
                        break;
                    }
                    sC = sC + m_MyGuild.NoticeList[I] + '\r';
                }
                sC = sC + "<KillGuilds>" + '\r';
                for (var I = 0; I < m_MyGuild.GuildWarList.Count; I++)
                {
                    if (sC.Length > 5000)
                    {
                        break;
                    }
                    sC = sC + m_MyGuild.GuildWarList[I] + '\r';
                }
                sC = sC + "<AllyGuilds>" + '\r';
                for (var i = 0; i < m_MyGuild.GuildAllList.Count; i++)
                {
                    if (sC.Length > 5000)
                    {
                        break;
                    }
                    sC = sC + m_MyGuild.GuildAllList[i] + '\r';
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
            TGuildRank GuildRank;
            var sSendMsg = string.Empty;
            if (m_MyGuild == null)
            {
                return;
            }
            for (var i = 0; i < m_MyGuild.m_RankList.Count; i++)
            {
                GuildRank = m_MyGuild.m_RankList[i];
                sSendMsg = sSendMsg + '#' + GuildRank.nRankNo + "/*" + GuildRank.sRankName + '/';
                for (var j = 0; j < GuildRank.MemberList.Count; j++)
                {
                    if (sSendMsg.Length > 5000)
                    {
                        break;
                    }
                    sSendMsg = sSendMsg + GuildRank.MemberList[j].sMemberName + '/';
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
                var PlayObject = M2Share.UserEngine.GetPlayObject(sHumName);
                if (PlayObject != null)
                {
                    if (PlayObject.GetPoseCreate() == this)
                    {
                        if (PlayObject.m_boAllowGuild)
                        {
                            if (!m_MyGuild.IsMember(sHumName))
                            {
                                if (PlayObject.m_MyGuild == null && m_MyGuild.m_RankList.Count < 400)
                                {
                                    m_MyGuild.AddMember(PlayObject);
                                    M2Share.UserEngine.SendServerGroupMsg(Grobal2.SS_207, M2Share.nServerIndex, m_MyGuild.sGuildName);
                                    PlayObject.m_MyGuild = m_MyGuild;
                                    PlayObject.m_sGuildRankName = m_MyGuild.GetRankName(PlayObject, ref PlayObject.m_nGuildRankNo);
                                    PlayObject.RefShowName();
                                    PlayObject.SysMsg("你已加入行会: " + m_MyGuild.sGuildName + " 当前封号为: " + PlayObject.m_sGuildRankName, MsgColor.Green, MsgType.Hint);
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
                            PlayObject.SysMsg("你拒绝加入行会。 [允许命令为 @" + M2Share.g_GameCommand.LETGUILD.sCmd + ']', MsgColor.Red, MsgType.Hint);
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
            string s14;
            TPlayObject PlayObject;
            var nC = 1;
            if (IsGuildMaster())
            {
                if (m_MyGuild.IsMember(sHumName))
                {
                    if (m_sCharName != sHumName)
                    {
                        if (m_MyGuild.DelMember(sHumName))
                        {
                            PlayObject = M2Share.UserEngine.GetPlayObject(sHumName);
                            if (PlayObject != null)
                            {
                                PlayObject.m_MyGuild = null;
                                PlayObject.RefRankInfo(0, "");
                                PlayObject.RefShowName();
                            }
                            M2Share.UserEngine.SendServerGroupMsg(Grobal2.SS_207, M2Share.nServerIndex, m_MyGuild.sGuildName);
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
                        s14 = m_MyGuild.sGuildName;
                        if (m_MyGuild.CancelGuld(sHumName))
                        {
                            M2Share.GuildManager.DelGuild(s14);
                            M2Share.UserEngine.SendServerGroupMsg(Grobal2.SS_206, M2Share.nServerIndex, s14);
                            m_MyGuild = null;
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
            if (m_MyGuild == null || m_nGuildRankNo != 1)
            {
                return;
            }
            m_MyGuild.NoticeList.Clear();
            while (!string.IsNullOrEmpty(sNotict))
            {
                sNotict = HUtil32.GetValidStr3(sNotict, ref sC, new string[] { "\r" });
                m_MyGuild.NoticeList.Add(sC);
            }
            m_MyGuild.SaveGuildInfoFile();
            M2Share.UserEngine.SendServerGroupMsg(Grobal2.SS_207, M2Share.nServerIndex, m_MyGuild.sGuildName);
            ClientOpenGuildDlg();
        }

        private void ClientGuildUpdateRankInfo(string sRankInfo)
        {
            if (m_MyGuild == null || m_nGuildRankNo != 1)
            {
                return;
            }
            var nC = m_MyGuild.UpdateRank(sRankInfo);
            if (nC == 0)
            {
                M2Share.UserEngine.SendServerGroupMsg(Grobal2.SS_207, M2Share.nServerIndex, m_MyGuild.sGuildName);
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
                TBaseObject BaseObjectC = GetPoseCreate();
                if (BaseObjectC != null && BaseObjectC.m_MyGuild != null && BaseObjectC.m_btRaceServer == Grobal2.RC_PLAYOBJECT && BaseObjectC.GetPoseCreate() == this)
                {
                    if (BaseObjectC.m_MyGuild.m_boEnableAuthAlly)
                    {
                        if (BaseObjectC.IsGuildMaster() && IsGuildMaster())
                        {
                            if (m_MyGuild.IsNotWarGuild(BaseObjectC.m_MyGuild) && BaseObjectC.m_MyGuild.IsNotWarGuild(m_MyGuild))
                            {
                                m_MyGuild.AllyGuild(BaseObjectC.m_MyGuild);
                                BaseObjectC.m_MyGuild.AllyGuild(m_MyGuild);
                                m_MyGuild.SendGuildMsg(BaseObjectC.m_MyGuild.sGuildName + "行会已经和您的行会联盟成功。");
                                BaseObjectC.m_MyGuild.SendGuildMsg(m_MyGuild.sGuildName + "行会已经和您的行会联盟成功。");
                                m_MyGuild.RefMemberName();
                                BaseObjectC.m_MyGuild.RefMemberName();
                                M2Share.UserEngine.SendServerGroupMsg(Grobal2.SS_207, M2Share.nServerIndex, m_MyGuild.sGuildName);
                                M2Share.UserEngine.SendServerGroupMsg(Grobal2.SS_207, M2Share.nServerIndex, BaseObjectC.m_MyGuild.sGuildName);
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
                M2Share.ErrorMessage(sExceptionMsg);
                M2Share.ErrorMessage(e.Message);
            }
        }

        private void ClientGuildBreakAlly(string sGuildName)
        {
            var n10 = -1;
            if (!IsGuildMaster())
            {
                return;
            }
            var guild = M2Share.GuildManager.FindGuild(sGuildName);
            if (guild != null)
            {
                if (m_MyGuild.IsAllyGuild(guild))
                {
                    m_MyGuild.DelAllyGuild(guild);
                    guild.DelAllyGuild(m_MyGuild);
                    m_MyGuild.SendGuildMsg(guild.sGuildName + " 行会与您的行会解除联盟成功!!!");
                    guild.SendGuildMsg(m_MyGuild.sGuildName + " 行会解除了与您行会的联盟!!!");
                    m_MyGuild.RefMemberName();
                    guild.RefMemberName();
                    M2Share.UserEngine.SendServerGroupMsg(Grobal2.SS_207, M2Share.nServerIndex, m_MyGuild.sGuildName);
                    M2Share.UserEngine.SendServerGroupMsg(Grobal2.SS_207, M2Share.nServerIndex, guild.sGuildName);
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
            TUserItem UserItem;
            TUserItem UserItemA = null;
            string sUserItemName;
            for (var i = 0; i < m_ItemList.Count; i++)
            {
                UserItem = m_ItemList[i];
                if (UserItem.MakeIndex == nInt)
                {
                    sUserItemName = ItmUnit.GetItemName(UserItem); // 取自定义物品名称
                    if (string.Compare(sUserItemName, sMsg, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        UserItemA = UserItem;
                        break;
                    }
                }
            }
            if (UserItemA == null)
            {
                return;
            }
            var merchant = (Merchant)M2Share.UserEngine.FindMerchant(nParam1);
            if (merchant != null && merchant.m_PEnvir == m_PEnvir && Math.Abs(merchant.m_nCurrX - m_nCurrX) < 15 && Math.Abs(merchant.m_nCurrY - m_nCurrY) < 15)
            {
                merchant.ClientQueryRepairCost(this, UserItemA);
            }
        }

        private void ClientRepairItem(int nParam1, int nInt, string sMsg)
        {
            TUserItem UserItem = null;
            string sUserItemName;
            for (var i = 0; i < m_ItemList.Count; i++)
            {
                UserItem = m_ItemList[i];
                sUserItemName = ItmUnit.GetItemName(UserItem);// 取自定义物品名称
                if (UserItem.MakeIndex == nInt && string.Compare(sUserItemName, sMsg, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    break;
                }
            }
            if (UserItem == null)
            {
                return;
            }
            Merchant merchant = (Merchant)M2Share.UserEngine.FindMerchant(nParam1);
            if (merchant != null && merchant.m_PEnvir == m_PEnvir && Math.Abs(merchant.m_nCurrX - m_nCurrX) < 15 && Math.Abs(merchant.m_nCurrY - m_nCurrY) < 15)
            {
                merchant.ClientRepairItem(this, UserItem);
            }
        }

        private void ClientStorageItem(int ObjectId, int nItemIdx, string sMsg)
        {
            GoodItem StdItem;
            var bo19 = false;
            TUserItem UserItem = null;
            string sUserItemName;
            if (sMsg.IndexOf(' ') >= 0)
            {
                HUtil32.GetValidStr3(sMsg, ref sMsg, new string[] { " " });
            }
            if (m_nPayMent == 1 && !M2Share.g_Config.boTryModeUseStorage)
            {
                SysMsg(M2Share.g_sTryModeCanotUseStorage, MsgColor.Red, MsgType.Hint);
                return;
            }
            Merchant merchant = (Merchant)M2Share.UserEngine.FindMerchant(ObjectId);
            for (var i = 0; i < m_ItemList.Count; i++)
            {
                UserItem = m_ItemList[i];
                sUserItemName = ItmUnit.GetItemName(UserItem); // 取自定义物品名称
                if (UserItem.MakeIndex == nItemIdx && string.Compare(sUserItemName, sMsg, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    // 检查NPC是否允许存物品
                    if (merchant != null && merchant.m_boStorage && (merchant.m_PEnvir == m_PEnvir && Math.Abs(merchant.m_nCurrX - m_nCurrX) < 15 && Math.Abs(merchant.m_nCurrY - m_nCurrY) < 15 || merchant == M2Share.g_FunctionNPC))
                    {
                        if (m_StorageItemList.Count < 39)
                        {
                            m_StorageItemList.Add(UserItem);
                            m_ItemList.RemoveAt(i);
                            WeightChanged();
                            SendDefMessage(Grobal2.SM_STORAGE_OK, 0, 0, 0, 0, "");
                            StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                            if (StdItem.NeedIdentify == 1)
                            {
                                M2Share.AddGameDataLog('1' + "\t" + m_sMapName + "\t" + m_nCurrX + "\t" + m_nCurrY + "\t" + m_sCharName + "\t" + StdItem.Name + "\t" + UserItem.MakeIndex + "\t" + '1' + "\t" + '0');
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

        private void ClientTakeBackStorageItem(int NPC, int nItemIdx, string sMsg)
        {
            GoodItem StdItem;
            string sUserItemName;
            var bo19 = false;
            TUserItem UserItem = null;
            var merchant = (Merchant)M2Share.UserEngine.FindMerchant(NPC);
            if (merchant == null)
            {
                return;
            }
            if (m_nPayMent == 1 && !M2Share.g_Config.boTryModeUseStorage)
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
            for (var i = 0; i < m_StorageItemList.Count; i++)
            {
                UserItem = m_StorageItemList[i];
                sUserItemName = ItmUnit.GetItemName(UserItem); // 取自定义物品名称
                if (UserItem.MakeIndex == nItemIdx && string.Compare(sUserItemName, sMsg, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    if (IsAddWeightAvailable(M2Share.UserEngine.GetStdItemWeight(UserItem.wIndex)))
                    {
                        // 检查NPC是否允许取物品
                        if (merchant.m_boGetback && (merchant.m_PEnvir == m_PEnvir && Math.Abs(merchant.m_nCurrX - m_nCurrX) < 15 && Math.Abs(merchant.m_nCurrY - m_nCurrY) < 15 || merchant == M2Share.g_FunctionNPC))
                        {
                            if (AddItemToBag(UserItem))
                            {
                                SendAddItem(UserItem);
                                m_StorageItemList.RemoveAt(i);
                                SendDefMessage(Grobal2.SM_TAKEBACKSTORAGEITEM_OK, nItemIdx, 0, 0, 0, "");
                                StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                                if (StdItem.NeedIdentify == 1)
                                {
                                    M2Share.AddGameDataLog('0' + "\t" + m_sMapName + "\t" + m_nCurrX + "\t" + m_nCurrY + "\t" + m_sCharName + "\t" + StdItem.Name + "\t" + UserItem.MakeIndex + "\t" + '1' + "\t" + '0');
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