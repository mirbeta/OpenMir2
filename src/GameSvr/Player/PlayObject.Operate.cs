using GameSvr.Actor;
using GameSvr.GameCommand;
using GameSvr.Items;
using GameSvr.Magic;
using GameSvr.Npc;
using GameSvr.World;
using SystemModule.Consts;
using SystemModule.Data;
using SystemModule.Enums;
using SystemModule.Packets.ClientPackets;

namespace GameSvr.Player
{
    public partial class PlayObject
    {
        private void ClientQueryUserName(int targetId, int x, int y)
        {
            BaseObject baseObject = M2Share.ActorMgr.Get(targetId);
            if (CretInNearXy(baseObject, x, y))
            {
                byte nameColor = GetChrColor(baseObject);
                CommandPacket defMsg = Grobal2.MakeDefaultMsg(Messages.SM_USERNAME, baseObject.ActorId, nameColor, 0, 0);
                SendSocket(defMsg, EDCode.EncodeString(baseObject.GetShowName()));
            }
            else
            {
                SendDefMessage(Messages.SM_GHOST, baseObject.ActorId, x, y, 0, "");
            }
        }

        public void ClientQueryBagItems()
        {
            string sSendMsg = string.Empty;
            for (int i = 0; i < ItemList.Count; i++)
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
                ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_BAGITEMS, ActorId, 0, 0, ItemList.Count);
                SendSocket(ClientMsg, sSendMsg);
            }
        }

        private static void ClientQueryUserSet(ProcessMessage processMsg)
        {

        }

        private void ClientQueryUserInformation(int charId, int nX, int nY)
        {
            PlayObject playObject = (PlayObject)M2Share.ActorMgr.Get(charId);
            if (!CretInNearXy(playObject, nX, nY))
            {
                return;
            }
            UserStateInfo userState = new UserStateInfo();
            userState.Feature = playObject.GetFeature(this);
            userState.UserName = playObject.ChrName;
            userState.NameColor = GetChrColor(playObject);
            if (playObject.MyGuild != null)
            {
                userState.GuildName = playObject.MyGuild.GuildName;
            }
            userState.GuildRankName = playObject.GuildRankName;
            for (int i = 0; i < playObject.UseItems.Length; i++)
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
                        ChangeItemWithLevel(ref clientItem, playObject.Abil.Level);
                    }
                    playObject.ChangeItemByJob(ref clientItem, playObject.Abil.Level);
                    userState.UseItems[i] = clientItem;
                }
            }
            ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_SENDUSERSTATE, 0, 0, 0, 0);
            SendSocket(ClientMsg, EDCode.EncodeBuffer(userState));
        }

        private void ClientMerchantDlgSelect(int nParam1, string sMsg)
        {
            if (Death || Ghost)
            {
                return;
            }
            NormNpc npc = WorldServer.FindMerchant<Merchant>(nParam1) ?? WorldServer.FindNpc<NormNpc>(nParam1);
            if (npc == null)
            {
                return;
            }
            if (npc.Envir == Envir && IsWithinSight(npc) || npc.IsHide)
            {
                npc.UserSelect(this, sMsg.Trim());
            }
        }

        private void ClientMerchantQuerySellPrice(int nParam1, int nMakeIndex, string sMsg)
        {
            UserItem userItem18 = null;
            for (int i = 0; i < ItemList.Count; i++)
            {
                UserItem userItem = ItemList[i];
                if (userItem.MakeIndex == nMakeIndex)
                {
                    string sUserItemName = CustomItem.GetItemName(userItem);
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
            Merchant merchant = WorldServer.FindMerchant<Merchant>(nParam1);
            if (merchant == null)
            {
                return;
            }
            if (merchant.Envir == Envir && merchant.IsSell && IsWithinSight(merchant))
            {
                merchant.ClientQuerySellPrice(this, userItem18);
            }
        }

        private void ClientUserSellItem(int nParam1, int nMakeIndex, string sMsg)
        {
            for (int i = 0; i < ItemList.Count; i++)
            {
                UserItem userItem = ItemList[i];
                if (userItem != null && userItem.MakeIndex == nMakeIndex)
                {
                    string sUserItemName = CustomItem.GetItemName(userItem);
                    if (string.Compare(sUserItemName, sMsg, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        Merchant merchant = WorldServer.FindMerchant<Merchant>(nParam1);
                        if (merchant != null && merchant.IsSell && merchant.Envir == Envir && IsWithinSight(merchant))
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
                Merchant merchant = WorldServer.FindMerchant<Merchant>(nParam1);
                if (merchant == null || !merchant.IsBuy || merchant.Envir != Envir || Math.Abs(merchant.CurrX - CurrX) > 15 || Math.Abs(merchant.CurrY - CurrY) > 15)
                {
                    return;
                }
                switch (nIdent)
                {
                    case Messages.CM_USERBUYITEM:
                        merchant.ClientBuyItem(this, sMsg, nInt);
                        break;
                    case Messages.CM_USERGETDETAILITEM:
                        merchant.ClientGetDetailGoodsList(this, sMsg, nZz);
                        break;
                }
            }
            catch (Exception e)
            {
                M2Share.Logger.Error("TUserHumah.ClientUserBuyItem wIdent = " + nIdent);
                M2Share.Logger.Error(e.Message);
            }
        }

        private bool ClientDropGold(int nGold)
        {
            if (M2Share.Config.InSafeDisableDrop && InSafeZone())
            {
                SendMsg(M2Share.ManageNPC, Messages.RM_MENU_OK, 0, ActorId, 0, 0, Settings.CanotDropInSafeZoneMsg);
                return false;
            }
            if (M2Share.Config.ControlDropItem && nGold < M2Share.Config.CanDropGold)
            {
                SendMsg(M2Share.ManageNPC, Messages.RM_MENU_OK, 0, ActorId, 0, 0, Settings.CanotDropGoldMsg);
                return false;
            }
            if (!IsCanDrop || Envir.Flag.NoThrowItem)
            {
                SendMsg(M2Share.ManageNPC, Messages.RM_MENU_OK, 0, ActorId, 0, 0, Settings.CanotDropItemMsg);
                return false;
            }
            if (nGold >= Gold)
            {
                return false;
            }
            Gold -= nGold;
            if (!DropGoldDown(nGold, false, 0, this.ActorId))
            {
                Gold += nGold;
            }
            GoldChanged();
            return true;
        }

        private bool ClientDropItem(string sItemName, int nItemIdx)
        {
            bool result = false;
            if (M2Share.Config.InSafeDisableDrop && InSafeZone())
            {
                SendMsg(M2Share.ManageNPC, Messages.RM_MENU_OK, 0, ActorId, 0, 0, Settings.CanotDropInSafeZoneMsg);
                return false;
            }
            if (!IsCanDrop || Envir.Flag.NoThrowItem)
            {
                SendMsg(M2Share.ManageNPC, Messages.RM_MENU_OK, 0, ActorId, 0, 0, Settings.CanotDropItemMsg);
                return false;
            }
            if (sItemName.IndexOf(' ') > 0)
            {
                // 折分物品名称(信件物品的名称后面加了使用次数)
                HUtil32.GetValidStr3(sItemName, ref sItemName, new[] { ' ' });
            }
            if ((HUtil32.GetTickCount() - DealLastTick) > 3000)
            {
                for (int i = 0; i < ItemList.Count; i++)
                {
                    UserItem userItem = ItemList[i];
                    if (userItem != null && userItem.MakeIndex == nItemIdx)
                    {
                        StdItem stdItem = M2Share.WorldEngine.GetStdItem(userItem.Index);
                        if (stdItem == null)
                        {
                            continue;
                        }
                        string sUserItemName = CustomItem.GetItemName(userItem);
                        if (string.Compare(sUserItemName, sItemName, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (M2Share.Config.ControlDropItem && stdItem.Price < M2Share.Config.CanDropPrice)
                            {
                                Dispose(userItem);
                                ItemList.RemoveAt(i);
                                result = true;
                                break;
                            }
                            if (DropItemDown(userItem, 1, false, 0, this.ActorId))
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
            if (Death || StatusTimeArr[PoisonState.STONE] != 0)// 防麻
            {
                return false;
            }
            if (!CheckActionStatus(wIdent, ref dwDelayTime))
            {
                IsFilterAction = false;
                return false;
            }
            IsFilterAction = true;
            if (!M2Share.Config.CloseSpeedHackCheck)
            {
                int dwCheckTime = HUtil32.GetTickCount() - TurnTick;
                if (dwCheckTime < M2Share.Config.TurnIntervalTime)
                {
                    dwDelayTime = M2Share.Config.TurnIntervalTime - dwCheckTime;
                    return false;
                }
            }
            if (nX == CurrX && nY == CurrY)
            {
                Direction = (byte)nDir;
                if (Walk(Messages.RM_TURN))
                {
                    TurnTick = HUtil32.GetTickCount();
                    return true;
                }
            }
            return false;
        }

        private bool ClientSitDownHit(int nX, int nY, int nDir, ref int dwDelayTime)
        {
            if (Death || StatusTimeArr[PoisonState.STONE] != 0)// 防麻
            {
                return false;
            }
            if (!M2Share.Config.CloseSpeedHackCheck)
            {
                int dwCheckTime = HUtil32.GetTickCount() - TurnTick;
                if (dwCheckTime < M2Share.Config.TurnIntervalTime)
                {
                    dwDelayTime = M2Share.Config.TurnIntervalTime - dwCheckTime;
                    return false;
                }
                TurnTick = HUtil32.GetTickCount();
            }
            SendRefMsg(Messages.RM_POWERHIT, 0, 0, 0, 0, "");
            return true;
        }

        private void ClientOpenDoor(int nX, int nY)
        {
            Maps.DoorInfo door = Envir.GetDoor(nX, nY);
            if (door == null)
            {
                return;
            }
            Castle.UserCastle castle = M2Share.CastleMgr.IsCastleEnvir(Envir);
            if (castle == null || castle.DoorStatus != door.Status || Race != ActorRace.Play || castle.CheckInPalace(CurrX, CurrY, this))
            {
                M2Share.WorldEngine.OpenDoor(Envir, nX, nY);
            }
        }

        private void ClientTakeOnItems(byte btWhere, int nItemIdx, string sItemName)
        {
            int itemIndex = -1;
            int n18 = 0;
            UserItem userItem = null;
            StdItem stdItem = null;
            ClientItem clientItem = null;
            for (int i = 0; i < ItemList.Count; i++)
            {
                userItem = ItemList[i];
                if (userItem != null && userItem.MakeIndex == nItemIdx)
                {
                    stdItem = M2Share.WorldEngine.GetStdItem(userItem.Index);
                    string sUserItemName = CustomItem.GetItemName(userItem);
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
                                StdItem stdItem20 = M2Share.WorldEngine.GetStdItem(UseItems[btWhere].Index);
                                if (stdItem20 != null && M2Share.StdModeMap.Contains(stdItem20.StdMode))
                                {
                                    if (!UserUnLockDurg && UseItems[btWhere].Desc[7] != 0)
                                    {
                                        SysMsg(Settings.CanotTakeOffItem, MsgColor.Red, MsgType.Hint);
                                        n18 = -4;
                                        goto FailExit;
                                    }
                                }
                                if (!UserUnLockDurg && (stdItem20.ItemDesc & 2) != 0)
                                {
                                    SysMsg(Settings.CanotTakeOffItem, MsgColor.Red, MsgType.Hint);
                                    n18 = -4;
                                    goto FailExit;
                                }
                                if ((stdItem20.ItemDesc & 4) != 0)
                                {
                                    SysMsg(Settings.CanotTakeOffItem, MsgColor.Red, MsgType.Hint);
                                    n18 = -4;
                                    goto FailExit;
                                }
                                if (M2Share.InDisableTakeOffList(UseItems[btWhere].Index))
                                {
                                    SysMsg(Settings.CanotTakeOffItem, MsgColor.Red, MsgType.Hint);
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
                            SendMsg(this, Messages.RM_ABILITY, 0, 0, 0, 0, "");
                            SendMsg(this, Messages.RM_SUBABILITY, 0, 0, 0, 0, "");
                            SendDefMessage(Messages.SM_TAKEON_OK, GetFeatureToLong(), GetFeatureEx(), 0, 0, "");
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
                SendDefMessage(Messages.SM_TAKEON_FAIL, n18, 0, 0, 0, "");
            }
        }

        private void ClientTakeOffItems(byte btWhere, int nItemIdx, string sItemName)
        {
            int n10 = 0;
            if (!Dealing && btWhere < 13)
            {
                if (UseItems[btWhere].Index > 0)
                {
                    if (UseItems[btWhere].MakeIndex == nItemIdx)
                    {
                        StdItem stdItem = M2Share.WorldEngine.GetStdItem(UseItems[btWhere].Index);
                        if (stdItem != null && M2Share.StdModeMap.Contains(stdItem.StdMode))
                        {
                            if (!UserUnLockDurg && UseItems[btWhere].Desc[7] != 0)
                            {
                                SysMsg(Settings.CanotTakeOffItem, MsgColor.Red, MsgType.Hint);
                                n10 = -4;
                                goto FailExit;
                            }
                        }
                        if (!UserUnLockDurg && (stdItem.ItemDesc & 2) != 0)
                        {
                            SysMsg(Settings.CanotTakeOffItem, MsgColor.Red, MsgType.Hint);
                            n10 = -4;
                            goto FailExit;
                        }
                        if ((stdItem.ItemDesc & 4) != 0)
                        {
                            SysMsg(Settings.CanotTakeOffItem, MsgColor.Red, MsgType.Hint);
                            n10 = -4;
                            goto FailExit;
                        }
                        if (M2Share.InDisableTakeOffList(UseItems[btWhere].Index))
                        {
                            SysMsg(Settings.CanotTakeOffItem, MsgColor.Red, MsgType.Hint);
                            goto FailExit;
                        }
                        string sUserItemName = CustomItem.GetItemName(UseItems[btWhere]);// 取自定义物品名称
                        if (string.Compare(sUserItemName, sItemName, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            UserItem userItem = UseItems[btWhere];
                            if (AddItemToBag(userItem))
                            {
                                SendAddItem(userItem);
                                //m_UseItems[btWhere].wIndex = 0;
                                UseItems[btWhere] = null;
                                RecalcAbilitys();
                                SendMsg(this, Messages.RM_ABILITY, 0, 0, 0, 0, "");
                                SendMsg(this, Messages.RM_SUBABILITY, 0, 0, 0, 0, "");
                                SendDefMessage(Messages.SM_TAKEOFF_OK, GetFeatureToLong(), GetFeatureEx(), 0, 0, "");
                                FeatureChanged();
                                if (M2Share.FunctionNPC != null)
                                {
                                    M2Share.FunctionNPC.GotoLable(this, "@TakeOff" + sItemName, false);
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
                SendDefMessage(Messages.SM_TAKEOFF_FAIL, n10, 0, 0, 0, "");
            }
        }

        private static string ClientUseItemsGetUnbindItemName(int nShape)
        {
            return M2Share.UnbindList.TryGetValue(nShape, out string result) ? result : string.Empty;
        }

        private bool ClientUseItemsGetUnBindItems(string sItemName, int nCount)
        {
            bool result = false;
            for (int i = 0; i < nCount; i++)
            {
                UserItem userItem = new UserItem();
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
            bool boEatOk = false;
            StdItem stdItem = null;
            int itemIndex = 0;
            if (MBoCanUseItem)
            {
                if (!Death)
                {
                    for (int i = 0; i < ItemList.Count; i++)
                    {
                        UserItem userItem = ItemList[i];
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
                                            if (ItemList.Count + 6 - 1 <= Grobal2.MaxBagItem)
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
                SendMsg(M2Share.ManageNPC, Messages.RM_MENU_OK, 0, ActorId, 0, 0, Settings.CanotUseItemMsg);
            }
            if (boEatOk)
            {
                WeightChanged();
                SendDefMessage(Messages.SM_EAT_OK, 0, 0, 0, 0, "");
                if (stdItem.NeedIdentify == 1)
                {
                    M2Share.EventSource.AddEventLog(11, MapName + "\t" + CurrX + "\t" + CurrY + "\t" + ChrName + "\t" + stdItem.Name + "\t" + itemIndex + "\t" + '1' + "\t" + '0');
                }
            }
            else
            {
                SendDefMessage(Messages.SM_EAT_FAIL, 0, 0, 0, 0, "");
            }
        }

        private bool ClientGetButchItem(int charId, int nX, int nY, byte btDir, ref int dwDelayTime)
        {
            dwDelayTime = 0;
            BaseObject baseObject = M2Share.ActorMgr.Get(charId);
            if (!M2Share.Config.CloseSpeedHackCheck)
            {
                int dwCheckTime = HUtil32.GetTickCount() - TurnTick;
                if (dwCheckTime < HUtil32._MAX(150, M2Share.Config.TurnIntervalTime - 150))
                {
                    dwDelayTime = HUtil32._MAX(150, M2Share.Config.TurnIntervalTime - 150) - dwCheckTime;
                    return false;
                }
                TurnTick = HUtil32.GetTickCount();
            }
            if (Math.Abs(nX - CurrX) <= 2 && Math.Abs(nY - CurrY) <= 2)
            {
                if (Envir.IsValidObject(nX, nY, 2, baseObject))
                {
                    if (baseObject.Death && !baseObject.Skeleton && baseObject.Animal)
                    {
                        byte n10 = (byte)(M2Share.RandomNumber.Random(16) + 5);
                        ushort n14 = (ushort)(M2Share.RandomNumber.Random(201) + 100);
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
                                baseObject.SendRefMsg(Messages.RM_SKELETON, baseObject.Direction, baseObject.CurrX, baseObject.CurrY, 0, "");
                            }
                            if (!TakeBagItems(baseObject))
                            {
                                SysMsg(Settings.YouFoundNothing, MsgColor.Red, MsgType.Hint);
                            }
                            baseObject.BodyLeathery = 50;
                        }
                        DeathTick = HUtil32.GetTickCount();
                    }
                }
                Direction = btDir;
            }
            SendRefMsg(Messages.RM_BUTCH, Direction, CurrX, CurrY, 0, "");
            return false;
        }

        protected bool TakeBagItems(BaseObject baseObject)
        {
            bool result = false;
            while (true)
            {
                if (baseObject.ItemList.Count <= 0)
                {
                    break;
                }
                UserItem userItem = baseObject.ItemList[0];
                if (!AddItemToBag(userItem))
                {
                    break;
                }
                SendAddItem(userItem);
                result = true;
                baseObject.ItemList.RemoveAt(0);
            }
            return result;
        }

        private void ClientChangeMagicKey(int nSkillIdx, char nKey)
        {
            for (int i = 0; i < MagicList.Count; i++)
            {
                UserMagic userMagic = MagicList[i];
                if (userMagic.Magic.MagicId == nSkillIdx)
                {
                    userMagic.Key = nKey;
                    break;
                }
            }
        }

        private void ClientGroupClose()
        {
            if (GroupOwner == 0)
            {
                AllowGroup = false;
                return;
            }
            if (GroupOwner != this.ActorId)
            {
                PlayObject groupOwnerPlay = (PlayObject)M2Share.ActorMgr.Get(GroupOwner);
                groupOwnerPlay.DelMember(this);
                AllowGroup = false;
            }
            else
            {
                SysMsg("如果你想退出，使用编组功能（删除按钮）", MsgColor.Red, MsgType.Hint);
            }
            if (M2Share.FunctionNPC != null)
            {
                M2Share.FunctionNPC.GotoLable(this, "@GroupClose", false);
            }
        }

        private void ClientCreateGroup(string sHumName)
        {
            PlayObject playObject = M2Share.WorldEngine.GetPlayObject(sHumName);
            if (GroupOwner != 0)
            {
                SendDefMessage(Messages.SM_CREATEGROUP_FAIL, -1, 0, 0, 0, "");
                return;
            }
            if (playObject == null || playObject == this || playObject.Death || playObject.Ghost)
            {
                SendDefMessage(Messages.SM_CREATEGROUP_FAIL, -2, 0, 0, 0, "");
                return;
            }
            if (playObject.GroupOwner != 0)
            {
                SendDefMessage(Messages.SM_CREATEGROUP_FAIL, -3, 0, 0, 0, "");
                return;
            }
            if (!playObject.AllowGroup)
            {
                SendDefMessage(Messages.SM_CREATEGROUP_FAIL, -4, 0, 0, 0, "");
                return;
            }
            GroupMembers.Clear();
            this.GroupMembers.Add(this);
            this.GroupMembers.Add(playObject);
            JoinGroup(this);
            playObject.JoinGroup(this);
            AllowGroup = true;
            SendDefMessage(Messages.SM_CREATEGROUP_OK, 0, 0, 0, 0, "");
            SendGroupMembers();
            if (M2Share.FunctionNPC != null)
            {
                M2Share.FunctionNPC.GotoLable(this, "@GroupCreate", false);// 创建小组时触发
            }
        }

        private void ClientAddGroupMember(string sHumName)
        {
            PlayObject playObject = M2Share.WorldEngine.GetPlayObject(sHumName);
            if (GroupOwner != this.ActorId)
            {
                SendDefMessage(Messages.SM_GROUPADDMEM_FAIL, -1, 0, 0, 0, "");
                return;
            }
            if (GroupMembers.Count > M2Share.Config.GroupMembersMax)
            {
                SendDefMessage(Messages.SM_GROUPADDMEM_FAIL, -5, 0, 0, 0, "");
                return;
            }
            if (playObject == null || playObject == this || playObject.Death || playObject.Ghost)
            {
                SendDefMessage(Messages.SM_GROUPADDMEM_FAIL, -2, 0, 0, 0, "");
                return;
            }
            if (playObject.GroupOwner != 0)
            {
                SendDefMessage(Messages.SM_GROUPADDMEM_FAIL, -3, 0, 0, 0, "");
                return;
            }
            if (!playObject.AllowGroup)
            {
                SendDefMessage(Messages.SM_GROUPADDMEM_FAIL, -4, 0, 0, 0, "");
                return;
            }
            this.GroupMembers.Add(playObject);
            playObject.JoinGroup(this);
            SendDefMessage(Messages.SM_GROUPADDMEM_OK, 0, 0, 0, 0, "");
            SendGroupMembers();
            if (M2Share.FunctionNPC != null)
            {
                M2Share.FunctionNPC.GotoLable(this, "@GroupAddMember", false);
            }
        }

        private void ClientDelGroupMember(string sHumName)
        {
            PlayObject playObject = M2Share.WorldEngine.GetPlayObject(sHumName);
            if (GroupOwner != this.ActorId)
            {
                SendDefMessage(Messages.SM_GROUPDELMEM_FAIL, -1, 0, 0, 0, "");
                return;
            }
            if (playObject == null)
            {
                SendDefMessage(Messages.SM_GROUPDELMEM_FAIL, -2, 0, 0, 0, "");
                return;
            }
            if (!IsGroupMember(playObject))
            {
                SendDefMessage(Messages.SM_GROUPDELMEM_FAIL, -3, 0, 0, 0, "");
                return;
            }
            DelMember(playObject);
            SendDefMessage(Messages.SM_GROUPDELMEM_OK, 0, 0, 0, 0, sHumName);
            if (M2Share.FunctionNPC != null)
            {
                M2Share.FunctionNPC.GotoLable(this, "@GroupDelMember", false);
            }
        }

        private void ClientDealTry(string sHumName)
        {
            if (M2Share.Config.DisableDeal)
            {
                SendMsg(M2Share.ManageNPC, Messages.RM_MENU_OK, 0, ActorId, 0, 0, Settings.DisableDealItemsMsg);
                return;
            }
            if (Dealing)
            {
                return;
            }
            if ((HUtil32.GetTickCount() - DealLastTick) < M2Share.Config.TryDealTime)
            {
                SendMsg(M2Share.ManageNPC, Messages.RM_MENU_OK, 0, ActorId, 0, 0, Settings.PleaseTryDealLaterMsg);
                return;
            }
            if (!IsCanDeal)
            {
                SendMsg(M2Share.ManageNPC, Messages.RM_MENU_OK, 0, ActorId, 0, 0, Settings.CanotTryDealMsg);
                return;
            }
            PlayObject targetPlayObject = (PlayObject)GetPoseCreate();
            if (targetPlayObject != null && targetPlayObject != this)
            {
                if (targetPlayObject.GetPoseCreate() == this && !targetPlayObject.Dealing)
                {
                    if (targetPlayObject.Race == ActorRace.Play)
                    {
                        if (targetPlayObject.AllowDeal && targetPlayObject.IsCanDeal)
                        {
                            targetPlayObject.SysMsg(ChrName + Settings.OpenedDealMsg, MsgColor.Green, MsgType.Hint);
                            SysMsg(targetPlayObject.ChrName + Settings.OpenedDealMsg, MsgColor.Green, MsgType.Hint);
                            this.OpenDealDlg(targetPlayObject);
                            targetPlayObject.OpenDealDlg(this);
                        }
                        else
                        {
                            SysMsg(Settings.PoseDisableDealMsg, MsgColor.Red, MsgType.Hint);
                        }
                    }
                }
                else
                {
                    SendDefMessage(Messages.SM_DEALTRY_FAIL, 0, 0, 0, 0, "");
                }
            }
            else
            {
                SendDefMessage(Messages.SM_DEALTRY_FAIL, 0, 0, 0, 0, "");
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
                HUtil32.GetValidStr3(sItemName, ref sItemName, new[] { ' ' });
            }
            bool dealSuccess = false;
            if (!DealCreat.DealSuccess)
            {
                for (int i = 0; i < ItemList.Count; i++)
                {
                    UserItem userItem = ItemList[i];
                    if (userItem.MakeIndex == nItemIdx)
                    {
                        string sUserItemName = CustomItem.GetItemName(userItem);
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
                SendDefMessage(Messages.SM_DEALADDITEM_FAIL, 0, 0, 0, 0, "");
            }
        }

        private void ClientDelDealItem(int nItemIdx, string sItemName)
        {
            if (M2Share.Config.CanNotGetBackDeal)
            {
                SendMsg(M2Share.ManageNPC, Messages.RM_MENU_OK, 0, ActorId, 0, 0, Settings.DealItemsDenyGetBackMsg);
                SendDefMessage(Messages.SM_DEALDELITEM_FAIL, 0, 0, 0, 0, "");
                return;
            }
            if (DealCreat == null || !Dealing)
            {
                return;
            }
            if (sItemName.IndexOf(' ') >= 0)
            {
                // 折分物品名称(信件物品的名称后面加了使用次数)
                HUtil32.GetValidStr3(sItemName, ref sItemName, new[] { ' ' });
            }
            bool bo11 = false;
            if (!DealCreat.DealSuccess)
            {
                for (int i = 0; i < DealItemList.Count; i++)
                {
                    UserItem userItem = DealItemList[i];
                    if (userItem.MakeIndex == nItemIdx)
                    {
                        string sUserItemName = CustomItem.GetItemName(userItem);
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
                SendDefMessage(Messages.SM_DEALDELITEM_FAIL, 0, 0, 0, 0, "");
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
                SendMsg(M2Share.ManageNPC, Messages.RM_MENU_OK, 0, ActorId, 0, 0, Settings.DealItemsDenyGetBackMsg);
                SendDefMessage(Messages.SM_DEALDELITEM_FAIL, 0, 0, 0, 0, "");
                return;
            }
            if (nGold < 0)
            {
                SendDefMessage(Messages.SM_DEALCHGGOLD_FAIL, DealGolds, HUtil32.LoWord(Gold), HUtil32.HiWord(Gold), 0, "");
                return;
            }
            bool bo09 = false;
            if (DealCreat != null && GetPoseCreate() == DealCreat)
            {
                if (!DealCreat.DealSuccess)
                {
                    if (Gold + DealGolds >= nGold)
                    {
                        Gold = Gold + DealGolds - nGold;
                        DealGolds = nGold;
                        SendDefMessage(Messages.SM_DEALCHGGOLD_OK, DealGolds, HUtil32.LoWord(Gold), HUtil32.HiWord(Gold), 0, "");
                        DealCreat.SendDefMessage(Messages.SM_DEALREMOTECHGGOLD, DealGolds, 0, 0, 0, "");
                        DealCreat.DealLastTick = HUtil32.GetTickCount();
                        bo09 = true;
                        DealLastTick = HUtil32.GetTickCount();
                    }
                }
            }
            if (!bo09)
            {
                SendDefMessage(Messages.SM_DEALCHGGOLD_FAIL, DealGolds, HUtil32.LoWord(Gold), HUtil32.HiWord(Gold), 0, "");
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
                SysMsg(Settings.DealOKTooFast, MsgColor.Red, MsgType.Hint);
                DealCancel();
                return;
            }
            if (DealCreat.DealSuccess)
            {
                bool bo11 = true;
                if (Grobal2.MaxBagItem - ItemList.Count < DealCreat.DealItemList.Count)
                {
                    bo11 = false;
                    SysMsg(Settings.YourBagSizeTooSmall, MsgColor.Red, MsgType.Hint);
                }
                if (GoldMax - Gold < DealCreat.DealGolds)
                {
                    SysMsg(Settings.YourGoldLargeThenLimit, MsgColor.Red, MsgType.Hint);
                    bo11 = false;
                }
                if (Grobal2.MaxBagItem - DealCreat.ItemList.Count < DealItemList.Count)
                {
                    SysMsg(Settings.DealHumanBagSizeTooSmall, MsgColor.Red, MsgType.Hint);
                    bo11 = false;
                }
                if (DealCreat.GoldMax - DealCreat.Gold < DealGolds)
                {
                    SysMsg(Settings.DealHumanGoldLargeThenLimit, MsgColor.Red, MsgType.Hint);
                    bo11 = false;
                }
                if (bo11)
                {
                    UserItem userItem;
                    StdItem stdItem;
                    for (int i = 0; i < DealItemList.Count; i++)
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
                            M2Share.EventSource.AddEventLog(8, MapName + "\t" + CurrX + "\t" + CurrY + "\t" + ChrName + "\t" + Grobal2.StringGoldName + "\t" + Gold + "\t" + '1' + "\t" + DealCreat.ChrName);
                        }
                    }
                    for (int i = 0; i < DealCreat.DealItemList.Count; i++)
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
                            M2Share.EventSource.AddEventLog(8, DealCreat.MapName + "\t" + DealCreat.CurrX + "\t" + DealCreat.CurrY + "\t" + DealCreat.ChrName + "\t" + Grobal2.StringGoldName + "\t" + DealCreat.Gold + "\t" + '1' + "\t" + ChrName);
                        }
                    }
                    PlayObject playObject = DealCreat;
                    playObject.SendDefMessage(Messages.SM_DEALSUCCESS, 0, 0, 0, 0, "");
                    playObject.SysMsg(Settings.DealSuccessMsg, MsgColor.Green, MsgType.Hint);
                    playObject.DealCreat = null;
                    playObject.Dealing = false;
                    playObject.DealItemList.Clear();
                    playObject.DealGolds = 0;
                    playObject.DealSuccess = false;
                    SendDefMessage(Messages.SM_DEALSUCCESS, 0, 0, 0, 0, "");
                    SysMsg(Settings.DealSuccessMsg, MsgColor.Green, MsgType.Hint);
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
                SysMsg(Settings.YouDealOKMsg, MsgColor.Green, MsgType.Hint);
                DealCreat.SysMsg(Settings.PoseDealOKMsg, MsgColor.Green, MsgType.Hint);
            }
        }

        private void ClientGetMinMap()
        {
            int nMinMap = Envir.MinMap;
            if (nMinMap > 0)
            {
                SendDefMessage(Messages.SM_READMINIMAP_OK, 0, (short)nMinMap, 0, 0, "");
            }
            else
            {
                SendDefMessage(Messages.SM_READMINIMAP_FAIL, 0, 0, 0, 0, "");
            }
        }

        private void ClientMakeDrugItem(int objectId, string nItemName)
        {
            Merchant merchant = WorldServer.FindMerchant<Merchant>(objectId);
            if (merchant == null || !merchant.IsMakeDrug)
            {
                return;
            }
            if (merchant.Envir == Envir && IsWithinSight(merchant))
            {
                merchant.ClientMakeDrugItem(this, nItemName);
            }
        }

        private void ClientOpenGuildDlg()
        {
            string sC;
            if (MyGuild != null)
            {
                sC = MyGuild.GuildName + '\r' + ' ' + '\r';
                if (GuildRankNo == 1)
                {
                    sC = sC + '1' + '\r';
                }
                else
                {
                    sC = sC + '0' + '\r';
                }
                sC = sC + "<Notice>" + '\r';
                for (int I = 0; I < MyGuild.NoticeList.Count; I++)
                {
                    if (sC.Length > 5000)
                    {
                        break;
                    }
                    sC = sC + MyGuild.NoticeList[I] + '\r';
                }
                sC = sC + "<KillGuilds>" + '\r';
                for (int I = 0; I < MyGuild.GuildWarList.Count; I++)
                {
                    if (sC.Length > 5000)
                    {
                        break;
                    }
                    sC = sC + MyGuild.GuildWarList[I] + '\r';
                }
                sC = sC + "<AllyGuilds>" + '\r';
                for (int i = 0; i < MyGuild.GuildAllList.Count; i++)
                {
                    if (sC.Length > 5000)
                    {
                        break;
                    }
                    sC = sC + MyGuild.GuildAllList[i] + '\r';
                }
                ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_OPENGUILDDLG, 0, 0, 0, 1);
                SendSocket(ClientMsg, EDCode.EncodeString(sC));
            }
            else
            {
                SendDefMessage(Messages.SM_OPENGUILDDLG_FAIL, 0, 0, 0, 0, "");
            }
        }

        private void ClientGuildHome()
        {
            ClientOpenGuildDlg();
        }

        private void ClientGuildMemberList()
        {
            string sSendMsg = string.Empty;
            if (MyGuild == null)
            {
                return;
            }
            for (int i = 0; i < MyGuild.MRankList.Count; i++)
            {
                Guild.GuildRank guildRank = MyGuild.MRankList[i];
                sSendMsg = sSendMsg + '#' + guildRank.RankNo + "/*" + guildRank.RankName + '/';
                for (int j = 0; j < guildRank.MemberList.Count; j++)
                {
                    if (sSendMsg.Length > 5000)
                    {
                        break;
                    }
                    sSendMsg = sSendMsg + guildRank.MemberList[j].MemberName + '/';
                }
            }
            ClientMsg = Grobal2.MakeDefaultMsg(Messages.SM_SENDGUILDMEMBERLIST, 0, 0, 0, 1);
            SendSocket(ClientMsg, EDCode.EncodeString(sSendMsg));
        }

        private void ClientGuildAddMember(string sHumName)
        {
            int nC = 1; // '你没有权利使用这个命令。'
            if (IsGuildMaster())
            {
                PlayObject playObject = M2Share.WorldEngine.GetPlayObject(sHumName);
                if (playObject != null)
                {
                    if (playObject.GetPoseCreate() == this)
                    {
                        if (playObject.AllowGuild)
                        {
                            if (!MyGuild.IsMember(sHumName))
                            {
                                if (playObject.MyGuild == null && MyGuild.MRankList.Count < 400)
                                {
                                    MyGuild.AddMember(playObject);
                                    WorldServer.SendServerGroupMsg(Messages.SS_207, M2Share.ServerIndex, MyGuild.GuildName);
                                    playObject.MyGuild = MyGuild;
                                    playObject.GuildRankName = MyGuild.GetRankName(playObject, ref playObject.GuildRankNo);
                                    playObject.RefShowName();
                                    playObject.SysMsg("你已加入行会: " + MyGuild.GuildName + " 当前封号为: " + playObject.GuildRankName, MsgColor.Green, MsgType.Hint);
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
                            playObject.SysMsg("你拒绝加入行会。 [允许命令为 @" + CommandMgr.GameCommands.LetGuild.CmdName + ']', MsgColor.Red, MsgType.Hint);
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
                SendDefMessage(Messages.SM_GUILDADDMEMBER_OK, 0, 0, 0, 0, "");
            }
            else
            {
                SendDefMessage(Messages.SM_GUILDADDMEMBER_FAIL, nC, 0, 0, 0, "");
            }
        }

        private void ClientGuildDelMember(string sHumName)
        {
            int nC = 1;
            if (IsGuildMaster())
            {
                if (MyGuild.IsMember(sHumName))
                {
                    if (ChrName != sHumName)
                    {
                        if (MyGuild.DelMember(sHumName))
                        {
                            PlayObject playObject = M2Share.WorldEngine.GetPlayObject(sHumName);
                            if (playObject != null)
                            {
                                playObject.MyGuild = null;
                                playObject.RefRankInfo(0, "");
                                playObject.RefShowName();
                            }
                            WorldServer.SendServerGroupMsg(Messages.SS_207, M2Share.ServerIndex, MyGuild.GuildName);
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
                        string s14 = MyGuild.GuildName;
                        if (MyGuild.CancelGuld(sHumName))
                        {
                            M2Share.GuildMgr.DelGuild(s14);
                            WorldServer.SendServerGroupMsg(Messages.SS_206, M2Share.ServerIndex, s14);
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
                SendDefMessage(Messages.SM_GUILDDELMEMBER_OK, 0, 0, 0, 0, "");
            }
            else
            {
                SendDefMessage(Messages.SM_GUILDDELMEMBER_FAIL, nC, 0, 0, 0, "");
            }
        }

        private void ClientGuildUpdateNotice(string sNotict)
        {
            string sC = string.Empty;
            if (MyGuild == null || GuildRankNo != 1)
            {
                return;
            }
            MyGuild.NoticeList.Clear();
            while (!string.IsNullOrEmpty(sNotict))
            {
                sNotict = HUtil32.GetValidStr3(sNotict, ref sC, '\r');
                MyGuild.NoticeList.Add(sC);
            }
            MyGuild.SaveGuildInfoFile();
            WorldServer.SendServerGroupMsg(Messages.SS_207, M2Share.ServerIndex, MyGuild.GuildName);
            ClientOpenGuildDlg();
        }

        private void ClientGuildUpdateRankInfo(string sRankInfo)
        {
            if (MyGuild == null || GuildRankNo != 1)
            {
                return;
            }
            int nC = MyGuild.UpdateRank(sRankInfo);
            if (nC == 0)
            {
                WorldServer.SendServerGroupMsg(Messages.SS_207, M2Share.ServerIndex, MyGuild.GuildName);
                ClientGuildMemberList();
            }
            else
            {
                if (nC <= -2)
                {
                    SendDefMessage(Messages.SM_GUILDRANKUPDATE_FAIL, nC, 0, 0, 0, "");
                }
            }
        }

        internal void ClientGuildAlly()
        {
            const string sExceptionMsg = "[Exception] TPlayObject::ClientGuildAlly";
            try
            {
                int n8 = -1;
                BaseObject poseObject = GetPoseCreate();
                if (poseObject != null && poseObject.Race == ActorRace.Play)
                {
                    PlayObject posePlay = poseObject as PlayObject;
                    if (posePlay.MyGuild != null && posePlay.GetPoseCreate() == this)
                    {
                        if (posePlay.MyGuild.EnableAuthAlly)
                        {
                            if (posePlay.IsGuildMaster() && IsGuildMaster())
                            {
                                if (MyGuild.IsNotWarGuild(posePlay.MyGuild) && posePlay.MyGuild.IsNotWarGuild(MyGuild))
                                {
                                    MyGuild.AllyGuild(posePlay.MyGuild);
                                    posePlay.MyGuild.AllyGuild(MyGuild);
                                    MyGuild.SendGuildMsg(posePlay.MyGuild.GuildName + "行会已经和您的行会联盟成功。");
                                    posePlay.MyGuild.SendGuildMsg(MyGuild.GuildName + "行会已经和您的行会联盟成功。");
                                    MyGuild.RefMemberName();
                                    posePlay.MyGuild.RefMemberName();
                                    WorldServer.SendServerGroupMsg(Messages.SS_207, M2Share.ServerIndex, MyGuild.GuildName);
                                    WorldServer.SendServerGroupMsg(Messages.SS_207, M2Share.ServerIndex, posePlay.MyGuild.GuildName);
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
                }
                if (n8 == 0)
                {
                    SendDefMessage(Messages.SM_GUILDMAKEALLY_OK, 0, 0, 0, 0, "");
                }
                else
                {
                    SendDefMessage(Messages.SM_GUILDMAKEALLY_FAIL, n8, 0, 0, 0, "");
                }
            }
            catch (Exception e)
            {
                M2Share.Logger.Error(sExceptionMsg);
                M2Share.Logger.Error(e.Message);
            }
        }

        internal void ClientGuildBreakAlly(string sGuildName)
        {
            int n10;
            if (!IsGuildMaster())
            {
                return;
            }
            Guild.GuildInfo guild = M2Share.GuildMgr.FindGuild(sGuildName);
            if (guild != null)
            {
                if (MyGuild.IsAllyGuild(guild))
                {
                    MyGuild.DelAllyGuild(guild);
                    guild.DelAllyGuild(MyGuild);
                    MyGuild.SendGuildMsg(guild.GuildName + " 行会与您的行会解除联盟成功!!!");
                    guild.SendGuildMsg(MyGuild.GuildName + " 行会解除了与您行会的联盟!!!");
                    MyGuild.RefMemberName();
                    guild.RefMemberName();
                    WorldServer.SendServerGroupMsg(Messages.SS_207, M2Share.ServerIndex, MyGuild.GuildName);
                    WorldServer.SendServerGroupMsg(Messages.SS_207, M2Share.ServerIndex, guild.GuildName);
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
                SendDefMessage(Messages.SM_GUILDBREAKALLY_OK, 0, 0, 0, 0, "");
            }
            else
            {
                SendDefMessage(Messages.SM_GUILDMAKEALLY_FAIL, 0, 0, 0, 0, "");
            }
        }

        private void ClientQueryRepairCost(int nParam1, int nInt, string sMsg)
        {
            UserItem userItemA = null;
            string sUserItemName;
            for (int i = 0; i < ItemList.Count; i++)
            {
                UserItem userItem = ItemList[i];
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
            Merchant merchant = WorldServer.FindMerchant<Merchant>(nParam1);
            if (merchant != null && merchant.Envir == Envir && IsWithinSight(merchant))
            {
                merchant.ClientQueryRepairCost(this, userItemA);
            }
        }

        private void ClientRepairItem(int nParam1, int nInt, string sMsg)
        {
            UserItem userItem = null;
            for (int i = 0; i < ItemList.Count; i++)
            {
                userItem = ItemList[i];
                string sUserItemName = CustomItem.GetItemName(userItem);
                if (userItem.MakeIndex == nInt && string.Compare(sUserItemName, sMsg, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    break;
                }
            }
            if (userItem == null)
            {
                return;
            }
            Merchant merchant = WorldServer.FindMerchant<Merchant>(nParam1);
            if (merchant != null && merchant.Envir == Envir && IsWithinSight(merchant))
            {
                merchant.ClientRepairItem(this, userItem);
            }
        }

        private void ClientStorageItem(int objectId, int nItemIdx, string sMsg)
        {
            bool bo19 = false;
            if (sMsg.IndexOf(' ') >= 0)
            {
                HUtil32.GetValidStr3(sMsg, ref sMsg, new[] { ' ' });
            }
            if (PayMent == 1 && !M2Share.Config.TryModeUseStorage)
            {
                SysMsg(Settings.TryModeCanotUseStorage, MsgColor.Red, MsgType.Hint);
                return;
            }
            Merchant merchant = WorldServer.FindMerchant<Merchant>(objectId);
            for (int i = 0; i < ItemList.Count; i++)
            {
                UserItem userItem = ItemList[i];
                string sUserItemName = CustomItem.GetItemName(userItem);// 取自定义物品名称
                if (userItem.MakeIndex == nItemIdx && string.Compare(sUserItemName, sMsg, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    // 检查NPC是否允许存物品
                    if (merchant != null && merchant.IsStorage && (merchant.Envir == Envir && IsWithinSight(merchant) || merchant == M2Share.FunctionNPC))
                    {
                        if (StorageItemList.Count < 39)
                        {
                            StorageItemList.Add(userItem);
                            ItemList.RemoveAt(i);
                            WeightChanged();
                            SendDefMessage(Messages.SM_STORAGE_OK, 0, 0, 0, 0, "");
                            StdItem stdItem = M2Share.WorldEngine.GetStdItem(userItem.Index);
                            if (stdItem.NeedIdentify == 1)
                            {
                                M2Share.EventSource.AddEventLog(1, MapName + "\t" + CurrX + "\t" + CurrY + "\t" + ChrName + "\t" + stdItem.Name + "\t" + userItem.MakeIndex + "\t" + '1' + "\t" + '0');
                            }
                        }
                        else
                        {
                            SendDefMessage(Messages.SM_STORAGE_FULL, 0, 0, 0, 0, "");
                        }
                        bo19 = true;
                    }
                    break;
                }
            }
            if (!bo19)
            {
                SendDefMessage(Messages.SM_STORAGE_FAIL, 0, 0, 0, 0, "");
            }
        }

        private void ClientTakeBackStorageItem(int npc, int nItemIdx, string sMsg)
        {
            bool bo19 = false;
            Merchant merchant = WorldServer.FindMerchant<Merchant>(npc);
            if (merchant == null)
            {
                return;
            }
            if (PayMent == 1 && !M2Share.Config.TryModeUseStorage)
            {
                // '试玩模式不可以使用仓库功能!!!'
                SysMsg(Settings.TryModeCanotUseStorage, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (!IsCanGetBackItem)
            {
                SendMsg(merchant, Messages.RM_MENU_OK, 0, ActorId, 0, 0, Settings.StorageIsLockedMsg + "\\ \\" + "仓库开锁命令: @" + CommandMgr.GameCommands.UnlockStorage.CmdName + '\\' + "仓库加锁命令: @" + CommandMgr.GameCommands.Lock.CmdName + '\\' + "设置密码命令: @" + CommandMgr.GameCommands.SetPassword.CmdName + '\\' + "修改密码命令: @" + CommandMgr.GameCommands.ChgPassword.CmdName);
                return;
            }
            for (int i = 0; i < StorageItemList.Count; i++)
            {
                UserItem userItem = StorageItemList[i];
                string sUserItemName = CustomItem.GetItemName(userItem);
                if (userItem.MakeIndex == nItemIdx && string.Compare(sUserItemName, sMsg, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    if (IsAddWeightAvailable(M2Share.WorldEngine.GetStdItemWeight(userItem.Index)))
                    {
                        // 检查NPC是否允许取物品
                        if (merchant.IsGetback && (merchant.Envir == Envir && IsWithinSight(merchant) || merchant == M2Share.FunctionNPC))
                        {
                            if (AddItemToBag(userItem))
                            {
                                SendAddItem(userItem);
                                StorageItemList.RemoveAt(i);
                                SendDefMessage(Messages.SM_TAKEBACKSTORAGEITEM_OK, nItemIdx, 0, 0, 0, "");
                                StdItem stdItem = M2Share.WorldEngine.GetStdItem(userItem.Index);
                                if (stdItem.NeedIdentify == 1)
                                {
                                    M2Share.EventSource.AddEventLog(0, MapName + "\t" + CurrX + "\t" + CurrY + "\t" + ChrName + "\t" + stdItem.Name + "\t" + userItem.MakeIndex + "\t" + '1' + "\t" + '0');
                                }
                            }
                            else
                            {
                                SendDefMessage(Messages.SM_TAKEBACKSTORAGEITEM_FULLBAG, 0, 0, 0, 0, "");
                            }
                            bo19 = true;
                        }
                    }
                    else
                    {
                        // '无法携带更多的东西!!!'
                        SysMsg(Settings.CanotGetItems, MsgColor.Red, MsgType.Hint);
                    }
                    break;
                }
            }
            if (!bo19)
            {
                SendDefMessage(Messages.SM_TAKEBACKSTORAGEITEM_FAIL, 0, 0, 0, 0, "");
            }
        }

        private bool IsWithinSight(NormNpc merchant)
        {
            return Math.Abs(merchant.CurrX - CurrX) < 15 && Math.Abs(merchant.CurrY - CurrY) < 15;
        }
    }
}