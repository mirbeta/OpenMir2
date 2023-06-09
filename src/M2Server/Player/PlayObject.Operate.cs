using M2Server.Actor;
using M2Server.Items;
using M2Server.Magic;
using SystemModule;
using SystemModule.Consts;
using SystemModule.Data;
using SystemModule.Enums;
using SystemModule.Packets.ClientPackets;

namespace M2Server.Player
{
    public partial class PlayObject
    {
        private void ClientQueryUserName(int targetId, int x, int y)
        {
            var baseObject = M2Share.ActorMgr.Get(targetId);
            if (baseObject == null)
            {
                return;
            }
            if (CretInNearXy(baseObject, x, y))
            {
                var nameColor = GetChrColor(baseObject);
                var defMsg = Messages.MakeMessage(Messages.SM_USERNAME, targetId, nameColor, 0, 0);
                SendSocket(defMsg, EDCode.EncodeString(baseObject.GetShowName()));
            }
            else
            {
                SendDefMessage(Messages.SM_GHOST, baseObject.ActorId, x, y, 0);
            }
        }

        public void ClientQueryBagItems()
        {
            var sSendMsg = string.Empty;
            for (var i = 0; i < ItemList.Count; i++)
            {
                var userItem = ItemList[i];
                var item = ModuleShare.ItemSystem.GetStdItem(userItem.Index);
                if (item != null)
                {
                    var clientItem = new ClientItem();
                    ModuleShare.ItemSystem.GetUpgradeStdItem(item, userItem, ref clientItem);
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
                ClientMsg = Messages.MakeMessage(Messages.SM_BAGITEMS, ActorId, 0, 0, ItemList.Count);
                SendSocket(ClientMsg, sSendMsg);
            }
        }

        private static void ClientQueryUserSet(ProcessMessage processMsg)
        {

        }

        private void ClientQueryUserInformation(int charId, int nX, int nY)
        {
            var playObject = (PlayObject)M2Share.ActorMgr.Get(charId);
            if (!CretInNearXy(playObject, nX, nY))
            {
                return;
            }
            var userState = new UserStateInfo();
            userState.Feature = playObject.GetFeature(this);
            userState.UserName = playObject.ChrName;
            userState.NameColor = GetChrColor(playObject);
            if (playObject.MyGuild != null)
            {
                userState.GuildName = playObject.MyGuild.GuildName;
            }
            userState.GuildRankName = playObject.GuildRankName;
            for (var i = 0; i < playObject.UseItems.Length; i++)
            {
                if (playObject.UseItems[i].Index > 0)
                {
                    var stdItem = ModuleShare.ItemSystem.GetStdItem(playObject.UseItems[i].Index);
                    if (stdItem == null)
                    {
                        continue;
                    }
                    var clientItem = new ClientItem();
                    ModuleShare.ItemSystem.GetUpgradeStdItem(stdItem, playObject.UseItems[i], ref clientItem);
                    clientItem.Item.Name = CustomItem.GetItemName(playObject.UseItems[i]);
                    clientItem.MakeIndex = playObject.UseItems[i].MakeIndex;
                    clientItem.Dura = playObject.UseItems[i].Dura;
                    clientItem.DuraMax = playObject.UseItems[i].DuraMax;
                    if (i == ItemLocation.Dress)
                    {
                        ChangeItemWithLevel(ref clientItem, playObject.Abil.Level);
                    }
                    playObject.ChangeItemByJob(ref clientItem, playObject.Abil.Level);
                    userState.UseItems[i] = clientItem;
                }
            }
            ClientMsg = Messages.MakeMessage(Messages.SM_SENDUSERSTATE, 0, 0, 0, 0);
            SendSocket(ClientMsg, EDCode.EncodeBuffer(userState));
        }

        private void ClientMerchantDlgSelect(int nParam1, string sMsg)
        {
            if (Death || Ghost)
            {
                return;
            }
            var npc = M2Share.WorldEngine.FindMerchant(nParam1) ?? M2Share.WorldEngine.FindNpc(nParam1);
            if (npc == null)
            {
                return;
            }
            if (npc.Envir == Envir && IsWithinSight(npc) || npc.IsHide)
            {
                npc.UserSelect((IPlayerActor)this, sMsg.Trim());
            }
        }

        private void ClientMerchantQuerySellPrice(int nParam1, int nMakeIndex, string sMsg)
        {
            UserItem userItem18 = null;
            for (var i = 0; i < ItemList.Count; i++)
            {
                var userItem = ItemList[i];
                if (userItem.MakeIndex == nMakeIndex)
                {
                    var sUserItemName = CustomItem.GetItemName(userItem);
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
            var merchant = M2Share.WorldEngine.FindMerchant(nParam1);
            if (merchant == null)
            {
                return;
            }
            if (merchant.Envir == Envir && merchant.IsSell && IsWithinSight(merchant))
            {
                // merchant.ClientQuerySellPrice(this, userItem18);
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
                    //if (string.Compare(sUserItemName, sMsg, StringComparison.OrdinalIgnoreCase) == 0)
                    //{
                    //    var merchant = M2Share.ActorMgr.FindMerchant(nParam1);
                    //    if (merchant != null && merchant.IsSell && merchant.Envir == Envir && IsWithinSight(merchant))
                    //    {
                    //        if (merchant.ClientSellItem(this, userItem))
                    //        {
                    //            if (userItem.Desc[13] == 1)
                    //            {
                    //                M2Share.CustomItemMgr.DelCustomItemName(userItem.MakeIndex, userItem.Index);
                    //                userItem.Desc[13] = 0;
                    //            }
                    //            ItemList.RemoveAt(i);
                    //            WeightChanged();
                    //        }
                    //    }
                    //    break;
                    //}
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
                var merchant = M2Share.WorldEngine.FindMerchant(nParam1);
                if (merchant == null || !merchant.IsBuy || merchant.Envir != Envir || Math.Abs(merchant.CurrX - CurrX) > 15 || Math.Abs(merchant.CurrY - CurrY) > 15)
                {
                    return;
                }
                //switch (nIdent)
                //{
                //    case Messages.CM_USERBUYITEM:
                //        merchant.ClientBuyItem(this, sMsg, nInt);
                //        break;
                //    case Messages.CM_USERGETDETAILITEM:
                //        merchant.ClientGetDetailGoodsList(this, sMsg, nZz);
                //        break;
                //}
            }
            catch (Exception e)
            {
                M2Share.Logger.Error("TUserHumah.ClientUserBuyItem wIdent = " + nIdent);
                M2Share.Logger.Error(e.Message);
            }
        }

        private void ClientDropGold(int nGold)
        {
            if (ModuleShare.Config.InSafeDisableDrop && InSafeZone())
            {
                SendMsg(ModuleShare.ManageNPC, Messages.RM_MENU_OK, 0, ActorId, 0, 0, Settings.CanotDropInSafeZoneMsg);
                return;
            }
            if (ModuleShare.Config.ControlDropItem && nGold < ModuleShare.Config.CanDropGold)
            {
                SendMsg(ModuleShare.ManageNPC, Messages.RM_MENU_OK, 0, ActorId, 0, 0, Settings.CanotDropGoldMsg);
                return;
            }
            if (!IsCanDrop || Envir.Flag.NoThrowItem)
            {
                SendMsg(ModuleShare.ManageNPC, Messages.RM_MENU_OK, 0, ActorId, 0, 0, Settings.CanotDropItemMsg);
                return;
            }
            if (nGold >= Gold)
            {
                return;
            }
            Gold -= nGold;
            if (!DropGoldDown(nGold, false, 0, this.ActorId))
            {
                Gold += nGold;
            }
            GoldChanged();
        }

        private bool ClientDropItem(string sItemName, int nItemIdx)
        {
            var result = false;
            if (ModuleShare.Config.InSafeDisableDrop && InSafeZone())
            {
                SendMsg(ModuleShare.ManageNPC, Messages.RM_MENU_OK, 0, ActorId, 0, 0, Settings.CanotDropInSafeZoneMsg);
                return false;
            }
            if (!IsCanDrop || Envir.Flag.NoThrowItem)
            {
                SendMsg(ModuleShare.ManageNPC, Messages.RM_MENU_OK, 0, ActorId, 0, 0, Settings.CanotDropItemMsg);
                return false;
            }
            if (sItemName.IndexOf(' ') > 0)
            {
                // 折分物品名称(信件物品的名称后面加了使用次数)
                HUtil32.GetValidStr3(sItemName, ref sItemName, ' ');
            }
            if ((HUtil32.GetTickCount() - DealLastTick) > 3000)
            {
                for (var i = 0; i < ItemList.Count; i++)
                {
                    var userItem = ItemList[i];
                    if (userItem != null && userItem.MakeIndex == nItemIdx)
                    {
                        var stdItem = ModuleShare.ItemSystem.GetStdItem(userItem.Index);
                        if (stdItem == null)
                        {
                            continue;
                        }
                        var sUserItemName = CustomItem.GetItemName(userItem);
                        if (string.Compare(sUserItemName, sItemName, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (ModuleShare.Config.ControlDropItem && stdItem.Price < ModuleShare.Config.CanDropPrice)
                            {
                                Dispose(userItem);
                                ItemList.RemoveAt(i);
                                result = true;
                                break;
                            }
                            int dropWide = HUtil32._MIN(ModuleShare.Config.DropItemRage, 3);
                            if (DropItemDown(userItem, dropWide, false, 0, this.ActorId))
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

        private bool ClientChangeDir(int wIdent, int nX, int nY, byte nDir, ref int dwDelayTime)
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
            if (!ModuleShare.Config.CloseSpeedHackCheck)
            {
                var dwCheckTime = HUtil32.GetTickCount() - TurnTick;
                if (dwCheckTime < ModuleShare.Config.TurnIntervalTime)
                {
                    dwDelayTime = ModuleShare.Config.TurnIntervalTime - dwCheckTime;
                    return false;
                }
            }
            if (nX == CurrX && nY == CurrY)
            {
                Dir = nDir;
                if (Walk(Messages.RM_TURN))
                {
                    TurnTick = HUtil32.GetTickCount();
                    return true;
                }
            }
            return false;
        }

        private bool ClientSitDownHit(int nX, int nY, byte nDir, ref int dwDelayTime)
        {
            if (Death || StatusTimeArr[PoisonState.STONE] != 0)// 防麻
            {
                return false;
            }
            if (!ModuleShare.Config.CloseSpeedHackCheck)
            {
                var dwCheckTime = HUtil32.GetTickCount() - TurnTick;
                if (dwCheckTime < ModuleShare.Config.TurnIntervalTime)
                {
                    dwDelayTime = ModuleShare.Config.TurnIntervalTime - dwCheckTime;
                    return false;
                }
                TurnTick = HUtil32.GetTickCount();
            }
            SendRefMsg(Messages.RM_POWERHIT, 0, 0, 0, 0, "");
            return true;
        }

        private void ClientOpenDoor(int nX, int nY)
        {
            MapDoor door = default;
            if (Envir.GetDoor(nX, nY, ref door))
            {
                return;
            }
            var castle = M2Share.CastleMgr.IsCastleEnvir(Envir);
            if (castle == null || castle.DoorStatus != door.Status || Race != ActorRace.Play || castle.CheckInPalace(CurrX, CurrY, this))
            {
                // M2Share.WorldEngine.OpenDoor(Envir, nX, nY);
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
                    stdItem = ModuleShare.ItemSystem.GetStdItem(userItem.Index);
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
                    ModuleShare.ItemSystem.GetUpgradeStdItem(stdItem, userItem, ref clientItem);
                    clientItem.Item.Name = CustomItem.GetItemName(userItem);
                    if (CheckTakeOnItems(btWhere, ref clientItem) && CheckItemBindUse(userItem))
                    {
                        UserItem takeOffItem = null;
                        if (btWhere <= 12)
                        {
                            if (UseItems[btWhere] != null && UseItems[btWhere].Index > 0)
                            {
                                var stdItem20 = ModuleShare.ItemSystem.GetStdItem(UseItems[btWhere].Index);
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
                            SendMsg(Messages.RM_ABILITY, 0, 0, 0, 0);
                            SendMsg(Messages.RM_SUBABILITY, 0, 0, 0, 0);
                            SendDefMessage(Messages.SM_TAKEON_OK, GetFeatureToLong(), GetFeatureEx(), 0, 0);
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
                SendDefMessage(Messages.SM_TAKEON_FAIL, n18, 0, 0, 0);
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
                        var stdItem = ModuleShare.ItemSystem.GetStdItem(UseItems[btWhere].Index);
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
                        var sUserItemName = CustomItem.GetItemName(UseItems[btWhere]);// 取自定义物品名称
                        if (string.Compare(sUserItemName, sItemName, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            var userItem = UseItems[btWhere];
                            if (AddItemToBag(userItem))
                            {
                                SendAddItem(userItem);
                                //UseItems[btWhere].wIndex = 0;
                                UseItems[btWhere] = null;
                                RecalcAbilitys();
                                SendMsg(Messages.RM_ABILITY, 0, 0, 0, 0);
                                SendMsg(Messages.RM_SUBABILITY, 0, 0, 0, 0);
                                SendDefMessage(Messages.SM_TAKEOFF_OK, GetFeatureToLong(), GetFeatureEx(), 0, 0);
                                FeatureChanged();
                                if (ModuleShare.FunctionNPC != null)
                                {
                                    //  ModuleShare.FunctionNPC.GotoLable(this, "@TakeOff" + sItemName, false);
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
                SendDefMessage(Messages.SM_TAKEOFF_FAIL, n10, 0, 0, 0);
            }
        }

        private static string ClientUseItemsGetUnbindItemName(int nShape)
        {
            return M2Share.UnbindList.TryGetValue(nShape, out var result) ? result : string.Empty;
        }

        private void ClientUseItemsGetUnBindItems(string sItemName, int nCount)
        {
            for (var i = 0; i < nCount; i++)
            {
                var userItem = new UserItem();
                if (ModuleShare.ItemSystem.CopyToUserItemFromName(sItemName, ref userItem))
                {
                    ItemList.Add(userItem);
                    SendAddItem(userItem);
                }
                else
                {
                    Dispose(userItem);
                    break;
                }
            }
        }

        private void ClientUseItems(int nItemIdx, string sItemName)
        {
            var eatSuccess = false;
            StdItem stdItem = null;
            var itemIndex = 0;
            if (BoCanUseItem)
            {
                if (!Death)
                {
                    for (var i = 0; i < ItemList.Count; i++)
                    {
                        var userItem = ItemList[i];
                        if (userItem != null && userItem.MakeIndex == nItemIdx)
                        {
                            itemIndex = userItem.MakeIndex;
                            stdItem = ModuleShare.ItemSystem.GetStdItem(userItem.Index);
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
                                            eatSuccess = true;
                                        }
                                        break;
                                    case 4: // 书
                                        if (ReadBook(stdItem))
                                        {
                                            Dispose(userItem);
                                            ItemList.RemoveAt(i);
                                            eatSuccess = true;
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
                                            //if (MagicArr[MagicConst.SKILL_REDBANWOL] != null && !RedUseHalfMoon)
                                            //{
                                            //    RedHalfMoonOnOff(true);
                                            //    SendSocket("+WID");
                                            //}
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
                                                eatSuccess = true;
                                            }
                                        }
                                        else
                                        {
                                            if (UseStdModeFunItem(stdItem))
                                            {
                                                Dispose(userItem);
                                                ItemList.RemoveAt(i);
                                                eatSuccess = true;
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
                SendMsg(ModuleShare.ManageNPC, Messages.RM_MENU_OK, 0, ActorId, 0, 0, Settings.CanotUseItemMsg);
            }
            if (eatSuccess)
            {
                WeightChanged();
                SendDefMessage(Messages.SM_EAT_OK, 0, 0, 0, 0);
                if (stdItem.NeedIdentify == 1)
                {
                    // M2Share.EventSource.AddEventLog(11, MapName + "\t" + CurrX + "\t" + CurrY + "\t" + ChrName + "\t" + stdItem.Name + "\t" + itemIndex + "\t" + '1' + "\t" + '0');
                }
            }
            else
            {
                SendDefMessage(Messages.SM_EAT_FAIL, 0, 0, 0, 0);
            }
        }

        private bool ClientGetButchItem(int charId, int nX, int nY, byte btDir, ref int dwDelayTime)
        {
            dwDelayTime = 0;
            var baseObject = M2Share.ActorMgr.Get(charId);
            if (!ModuleShare.Config.CloseSpeedHackCheck)
            {
                var dwCheckTime = HUtil32.GetTickCount() - TurnTick;
                if (dwCheckTime < HUtil32._MAX(150, ModuleShare.Config.TurnIntervalTime - 150))
                {
                    dwDelayTime = HUtil32._MAX(150, ModuleShare.Config.TurnIntervalTime - 150) - dwCheckTime;
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
                        var n10 = (byte)(M2Share.RandomNumber.Random(16) + 5);
                        var meatQuality = (ushort)(M2Share.RandomNumber.Random(201) + 100);
                        baseObject.BodyLeathery -= n10;
                        ((AnimalObject)baseObject).MeatQuality -= meatQuality;//随机降低肉的品质
                        if (((AnimalObject)baseObject).MeatQuality <= 0)
                        {
                            ((AnimalObject)baseObject).MeatQuality = 0;
                        }
                        if (baseObject.BodyLeathery <= 0)
                        {
                            if (baseObject.Race >= ActorRace.Animal && baseObject.Race < ActorRace.Monster)
                            {
                                baseObject.Skeleton = true;
                                baseObject.ApplyMeatQuality();
                                baseObject.SendRefMsg(Messages.RM_SKELETON, baseObject.Dir, baseObject.CurrX, baseObject.CurrY, 0, "");
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
                Dir = btDir;
            }
            SendRefMsg(Messages.RM_BUTCH, Dir, CurrX, CurrY, 0, "");
            return false;
        }

        private bool TakeBagItems(IActor baseObject)
        {
            var result = false;
            while (true)
            {
                if (baseObject.ItemList.Count <= 0)
                {
                    break;
                }
                var userItem = baseObject.ItemList[0];
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

        private void ClientChangeMagicKey(ushort nSkillIdx, char nKey)
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
            if (GroupOwner == 0)
            {
                AllowGroup = false;
                return;
            }
            if (GroupOwner != this.ActorId)
            {
                var groupOwnerPlay = (PlayObject)M2Share.ActorMgr.Get(GroupOwner);
                groupOwnerPlay.DelMember((IPlayerActor)this);
                AllowGroup = false;
            }
            else
            {
                SysMsg("如果你想退出，使用编组功能（删除按钮）", MsgColor.Red, MsgType.Hint);
            }
            if (ModuleShare.FunctionNPC != null)
            {
                //ModuleShare.FunctionNPC.GotoLable(this, "@GroupClose", false);
            }
        }

        private void ClientCreateGroup(string sHumName)
        {
            var playObject = M2Share.WorldEngine.GetPlayObject(sHumName);
            if (GroupOwner != 0)
            {
                SendDefMessage(Messages.SM_CREATEGROUP_FAIL, -1, 0, 0, 0);
                return;
            }
            if (playObject == null || playObject == this || playObject.Death || playObject.Ghost)
            {
                SendDefMessage(Messages.SM_CREATEGROUP_FAIL, -2, 0, 0, 0);
                return;
            }
            if (playObject.GroupOwner != 0)
            {
                SendDefMessage(Messages.SM_CREATEGROUP_FAIL, -3, 0, 0, 0);
                return;
            }
            if (!playObject.AllowGroup)
            {
                SendDefMessage(Messages.SM_CREATEGROUP_FAIL, -4, 0, 0, 0);
                return;
            }
            GroupMembers.Clear();
            this.GroupMembers.Add((IPlayerActor)this);
            this.GroupMembers.Add(playObject);
            JoinGroup(this);
            playObject.JoinGroup((IPlayerActor)this);
            AllowGroup = true;
            SendDefMessage(Messages.SM_CREATEGROUP_OK, 0, 0, 0, 0);
            SendGroupMembers();
            if (ModuleShare.FunctionNPC != null)
            {
                // ModuleShare.FunctionNPC.GotoLable(this, "@GroupCreate", false);// 创建小组时触发
            }
        }

        private void ClientAddGroupMember(string sHumName)
        {
            var playObject = M2Share.WorldEngine.GetPlayObject(sHumName);
            if (GroupOwner != this.ActorId)
            {
                SendDefMessage(Messages.SM_GROUPADDMEM_FAIL, -1, 0, 0, 0);
                return;
            }
            if (GroupMembers.Count > ModuleShare.Config.GroupMembersMax)
            {
                SendDefMessage(Messages.SM_GROUPADDMEM_FAIL, -5, 0, 0, 0);
                return;
            }
            if (playObject == null || playObject == this || playObject.Death || playObject.Ghost)
            {
                SendDefMessage(Messages.SM_GROUPADDMEM_FAIL, -2, 0, 0, 0);
                return;
            }
            if (playObject.GroupOwner != 0)
            {
                SendDefMessage(Messages.SM_GROUPADDMEM_FAIL, -3, 0, 0, 0);
                return;
            }
            if (!playObject.AllowGroup)
            {
                SendDefMessage(Messages.SM_GROUPADDMEM_FAIL, -4, 0, 0, 0);
                return;
            }
            this.GroupMembers.Add(playObject);
            playObject.JoinGroup((IPlayerActor)this);
            SendDefMessage(Messages.SM_GROUPADDMEM_OK, 0, 0, 0, 0);
            SendGroupMembers();
            if (ModuleShare.FunctionNPC != null)
            {
                //ModuleShare.FunctionNPC.GotoLable(this, "@GroupAddMember", false);
            }
        }

        private void ClientDelGroupMember(string sHumName)
        {
            var playObject = M2Share.WorldEngine.GetPlayObject(sHumName);
            if (GroupOwner != this.ActorId)
            {
                SendDefMessage(Messages.SM_GROUPDELMEM_FAIL, -1, 0, 0, 0);
                return;
            }
            if (playObject == null)
            {
                SendDefMessage(Messages.SM_GROUPDELMEM_FAIL, -2, 0, 0, 0);
                return;
            }
            if (!IsGroupMember(playObject))
            {
                SendDefMessage(Messages.SM_GROUPDELMEM_FAIL, -3, 0, 0, 0);
                return;
            }
            DelMember(playObject);
            SendDefMessage(Messages.SM_GROUPDELMEM_OK, 0, 0, 0, 0, sHumName);
            if (ModuleShare.FunctionNPC != null)
            {
                //  ModuleShare.FunctionNPC.GotoLable(this, "@GroupDelMember", false);
            }
        }

        private void ClientDealTry(string sHumName)
        {
            if (ModuleShare.Config.DisableDeal)
            {
                SendMsg(ModuleShare.ManageNPC, Messages.RM_MENU_OK, 0, ActorId, 0, 0, Settings.DisableDealItemsMsg);
                return;
            }
            if (Dealing)
            {
                return;
            }
            if ((HUtil32.GetTickCount() - DealLastTick) < ModuleShare.Config.TryDealTime)
            {
                SendMsg(ModuleShare.ManageNPC, Messages.RM_MENU_OK, 0, ActorId, 0, 0, Settings.PleaseTryDealLaterMsg);
                return;
            }
            if (!IsCanDeal)
            {
                SendMsg(ModuleShare.ManageNPC, Messages.RM_MENU_OK, 0, ActorId, 0, 0, Settings.CanotTryDealMsg);
                return;
            }
            var poseObject = GetPoseCreate();
            if (poseObject.Race == ActorRace.Play)
            {
                var targetPlayObject = (PlayObject)poseObject;
                if (targetPlayObject != this)
                {
                    if (targetPlayObject.GetPoseCreate() == this && !targetPlayObject.Dealing)
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
                    else
                    {
                        SendDefMessage(Messages.SM_DEALTRY_FAIL, 0, 0, 0, 0);
                    }
                }
            }
            else
            {
                SendDefMessage(Messages.SM_DEALTRY_FAIL, 0, 0, 0, 0);
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
                HUtil32.GetValidStr3(sItemName, ref sItemName, ' ');
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
                SendDefMessage(Messages.SM_DEALADDITEM_FAIL, 0, 0, 0, 0);
            }
        }

        private void ClientDelDealItem(int nItemIdx, string sItemName)
        {
            if (ModuleShare.Config.CanNotGetBackDeal)
            {
                SendMsg(ModuleShare.ManageNPC, Messages.RM_MENU_OK, 0, ActorId, 0, 0, Settings.DealItemsDenyGetBackMsg);
                SendDefMessage(Messages.SM_DEALDELITEM_FAIL, 0, 0, 0, 0);
                return;
            }
            if (DealCreat == null || !Dealing)
            {
                return;
            }
            if (sItemName.IndexOf(' ') >= 0)
            {
                // 折分物品名称(信件物品的名称后面加了使用次数)
                HUtil32.GetValidStr3(sItemName, ref sItemName, ' ');
            }
            var bo11 = false;
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
                SendDefMessage(Messages.SM_DEALDELITEM_FAIL, 0, 0, 0, 0);
            }
        }

        private void ClientCancelDeal()
        {
            DealCancel();
        }

        private void ClientChangeDealGold(int nGold)
        {
            if (DealGolds > 0 && ModuleShare.Config.CanNotGetBackDeal)// 禁止取回放入交易栏内的金币
            {
                SendMsg(ModuleShare.ManageNPC, Messages.RM_MENU_OK, 0, ActorId, 0, 0, Settings.DealItemsDenyGetBackMsg);
                SendDefMessage(Messages.SM_DEALDELITEM_FAIL, 0, 0, 0, 0);
                return;
            }
            if (nGold < 0)
            {
                SendDefMessage(Messages.SM_DEALCHGGOLD_FAIL, DealGolds, HUtil32.LoWord(Gold), HUtil32.HiWord(Gold), 0);
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
                        SendDefMessage(Messages.SM_DEALCHGGOLD_OK, DealGolds, HUtil32.LoWord(Gold), HUtil32.HiWord(Gold), 0);
                        DealCreat.SendDefMessage(Messages.SM_DEALREMOTECHGGOLD, DealGolds, 0, 0, 0);
                        DealCreat.DealLastTick = HUtil32.GetTickCount();
                        bo09 = true;
                        DealLastTick = HUtil32.GetTickCount();
                    }
                }
            }
            if (!bo09)
            {
                SendDefMessage(Messages.SM_DEALCHGGOLD_FAIL, DealGolds, HUtil32.LoWord(Gold), HUtil32.HiWord(Gold), 0);
            }
        }

        private void ClientDealEnd()
        {
            DealSuccess = true;
            if (DealCreat == null)
            {
                return;
            }
            if (((HUtil32.GetTickCount() - DealLastTick) < ModuleShare.Config.DealOKTime) || ((HUtil32.GetTickCount() - DealCreat.DealLastTick) < ModuleShare.Config.DealOKTime))
            {
                SysMsg(Settings.DealOKTooFast, MsgColor.Red, MsgType.Hint);
                DealCancel();
                return;
            }
            if (DealCreat.DealSuccess)
            {
                var bo11 = true;
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
                    for (var i = 0; i < DealItemList.Count; i++)
                    {
                        userItem = DealItemList[i];
                        DealCreat.AddItemToBag(userItem);
                        DealCreat.SendAddItem(userItem);
                        stdItem = ModuleShare.ItemSystem.GetStdItem(userItem.Index);
                        if (stdItem != null)
                        {
                            if (!M2Share.IsCheapStuff(stdItem.StdMode))
                            {
                                if (stdItem.NeedIdentify == 1)
                                {
                                    //  M2Share.EventSource.AddEventLog(8, MapName + "\t" + CurrX + "\t" + CurrY + "\t" + ChrName + "\t" + stdItem.Name + "\t" + userItem.MakeIndex + "\t" + '1' + "\t" + DealCreat.ChrName);
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
                            //  M2Share.EventSource.AddEventLog(8, MapName + "\t" + CurrX + "\t" + CurrY + "\t" + ChrName + "\t" + Grobal2.StringGoldName + "\t" + Gold + "\t" + '1' + "\t" + DealCreat.ChrName);
                        }
                    }
                    for (var i = 0; i < DealCreat.DealItemList.Count; i++)
                    {
                        userItem = DealCreat.DealItemList[i];
                        AddItemToBag(userItem);
                        this.SendAddItem(userItem);
                        stdItem = ModuleShare.ItemSystem.GetStdItem(userItem.Index);
                        if (stdItem != null)
                        {
                            if (!M2Share.IsCheapStuff(stdItem.StdMode))
                            {
                                if (stdItem.NeedIdentify == 1)
                                {
                                    //   M2Share.EventSource.AddEventLog(8, DealCreat.MapName + "\t" + DealCreat.CurrX + "\t" + DealCreat.CurrY + "\t" + DealCreat.ChrName + "\t" + stdItem.Name + "\t" + userItem.MakeIndex + "\t" + '1' + "\t" + ChrName);
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
                            //  M2Share.EventSource.AddEventLog(8, DealCreat.MapName + "\t" + DealCreat.CurrX + "\t" + DealCreat.CurrY + "\t" + DealCreat.ChrName + "\t" + Grobal2.StringGoldName + "\t" + DealCreat.Gold + "\t" + '1' + "\t" + ChrName);
                        }
                    }
                    DealCreat.SendDefMessage(Messages.SM_DEALSUCCESS, 0, 0, 0, 0);
                    DealCreat.SysMsg(Settings.DealSuccessMsg, MsgColor.Green, MsgType.Hint);
                    DealCreat.DealCreat = null;
                    DealCreat.Dealing = false;
                    DealCreat.DealItemList.Clear();
                    DealCreat.DealGolds = 0;
                    DealCreat.DealSuccess = false;
                    SendDefMessage(Messages.SM_DEALSUCCESS, 0, 0, 0, 0);
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
            if (Envir.MinMap > 0)
            {
                SendDefMessage(Messages.SM_READMINIMAP_OK, 0, Envir.MinMap, 0, 0);
            }
            else
            {
                SendDefMessage(Messages.SM_READMINIMAP_FAIL, 0, 0, 0, 0);
            }
        }

        private void ClientMakeDrugItem(int actorId, string nItemName)
        {
            var merchant = M2Share.WorldEngine.FindMerchant(actorId);
            if (merchant == null || !merchant.IsMakeDrug)
            {
                return;
            }
            if (merchant.Envir == Envir && IsWithinSight(merchant))
            {
                //  merchant.ClientMakeDrugItem(this, nItemName);
            }
        }

        private void ClientOpenGuildDlg()
        {
            if (MyGuild != null)
            {
                var sSendStr = MyGuild.GuildName + '\r' + ' ' + '\r';
                if (GuildRankNo == 1)
                {
                    sSendStr = sSendStr + '1' + '\r';
                }
                else
                {
                    sSendStr = sSendStr + '0' + '\r';
                }
                sSendStr = sSendStr + "<Notice>" + '\r';
                for (var i = 0; i < MyGuild.NoticeList.Count; i++)
                {
                    if (sSendStr.Length > 5000)
                    {
                        break;
                    }
                    sSendStr = sSendStr + MyGuild.NoticeList[i] + '\r';
                }
                sSendStr = sSendStr + "<KillGuilds>" + '\r';
                for (var i = 0; i < MyGuild.GuildWarList.Count; i++)
                {
                    if (sSendStr.Length > 5000)
                    {
                        break;
                    }
                    sSendStr = sSendStr + MyGuild.GuildWarList[i] + '\r';
                }
                sSendStr = sSendStr + "<AllyGuilds>" + '\r';
                for (var i = 0; i < MyGuild.GuildAllList.Count; i++)
                {
                    if (sSendStr.Length > 5000)
                    {
                        break;
                    }
                    sSendStr = sSendStr + MyGuild.GuildAllList[i] + '\r';
                }
                ClientMsg = Messages.MakeMessage(Messages.SM_OPENGUILDDLG, 0, 0, 0, 1);
                SendSocket(ClientMsg, EDCode.EncodeString(sSendStr));
            }
            else
            {
                SendDefMessage(Messages.SM_OPENGUILDDLG_FAIL, 0, 0, 0, 0);
            }
        }

        private void ClientGuildHome()
        {
            ClientOpenGuildDlg();
        }

        private void ClientGuildMemberList()
        {
            if (MyGuild == null)
            {
                return;
            }
            var sSendMsg = string.Empty;
            for (var i = 0; i < MyGuild.RankList.Count; i++)
            {
                var guildRank = MyGuild.RankList[i];
                sSendMsg = sSendMsg + '#' + guildRank.RankNo + "/*" + guildRank.RankName + '/';
                for (var j = 0; j < guildRank.MemberList.Count; j++)
                {
                    if (sSendMsg.Length > 5000)
                    {
                        break;
                    }
                    sSendMsg = sSendMsg + guildRank.MemberList[j].MemberName + '/';
                }
            }
            ClientMsg = Messages.MakeMessage(Messages.SM_SENDGUILDMEMBERLIST, 0, 0, 0, 1);
            SendSocket(ClientMsg, EDCode.EncodeString(sSendMsg));
        }

        private void ClientGuildAddMember(string sHumName)
        {
            byte nC = 1; // '你没有权利使用这个命令。'
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
                                if (playObject.MyGuild == null && MyGuild.RankList.Count < 400)
                                {
                                    MyGuild.AddMember(playObject);
                                    M2Share.WorldEngine.SendServerGroupMsg(Messages.SS_207, M2Share.ServerIndex, MyGuild.GuildName);
                                    playObject.MyGuild = MyGuild;
                                    short rankNo = 0;
                                    playObject.GuildRankName = MyGuild.GetRankName(playObject, ref rankNo);
                                    playObject.GuildRankNo = rankNo;
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
                                    // playObject.SysMsg("你拒绝加入行会。 [允许命令为 @" + CommandMgr.GameCommands.LetGuild.CmdName + ']', MsgColor.Red, MsgType.Hint);
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
                SendDefMessage(Messages.SM_GUILDADDMEMBER_OK, 0, 0, 0, 0);
            }
            else
            {
                SendDefMessage(Messages.SM_GUILDADDMEMBER_FAIL, nC, 0, 0, 0);
            }
        }

        private void ClientGuildDelMember(string sHumName)
        {
            byte nC = 1;
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
                            M2Share.WorldEngine.SendServerGroupMsg(Messages.SS_207, M2Share.ServerIndex, MyGuild.GuildName);
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
                        var s14 = MyGuild.GuildName;
                        if (MyGuild.CancelGuld(sHumName))
                        {
                            M2Share.GuildMgr.DelGuild(s14);
                            M2Share.WorldEngine.SendServerGroupMsg(Messages.SS_206, M2Share.ServerIndex, s14);
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
                SendDefMessage(Messages.SM_GUILDDELMEMBER_OK, 0, 0, 0, 0);
            }
            else
            {
                SendDefMessage(Messages.SM_GUILDDELMEMBER_FAIL, nC, 0, 0, 0);
            }
        }

        private void ClientGuildUpdateNotice(string sNotict)
        {
            if (MyGuild == null || GuildRankNo != 1)
            {
                return;
            }
            var sNoticeStr = string.Empty;
            MyGuild.NoticeList.Clear();
            while (!string.IsNullOrEmpty(sNotict))
            {
                sNotict = HUtil32.GetValidStr3(sNotict, ref sNoticeStr, '\r');
                MyGuild.NoticeList.Add(sNoticeStr);
            }
            MyGuild.SaveGuildInfoFile();
            M2Share.WorldEngine.SendServerGroupMsg(Messages.SS_207, M2Share.ServerIndex, MyGuild.GuildName);
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
                M2Share.WorldEngine.SendServerGroupMsg(Messages.SS_207, M2Share.ServerIndex, MyGuild.GuildName);
                ClientGuildMemberList();
            }
            else
            {
                if (nC <= -2)
                {
                    SendDefMessage(Messages.SM_GUILDRANKUPDATE_FAIL, nC, 0, 0, 0);
                }
            }
        }

        public void ClientGuildAlly()
        {
            const string sExceptionMsg = "[Exception] PlayObject::ClientGuildAlly";
            try
            {
                var n8 = -1;
                var poseObject = GetPoseCreate();
                if (poseObject != null && poseObject.Race == ActorRace.Play)
                {
                    var posePlayer = (PlayObject)poseObject;
                    if (posePlayer.MyGuild != null && posePlayer.GetPoseCreate() == this)
                    {
                        if (posePlayer.MyGuild.EnableAuthAlly)
                        {
                            if (posePlayer.IsGuildMaster() && IsGuildMaster())
                            {
                                if (MyGuild.IsNotWarGuild(posePlayer.MyGuild) && posePlayer.MyGuild.IsNotWarGuild(MyGuild))
                                {
                                    MyGuild.AllyGuild(posePlayer.MyGuild);
                                    posePlayer.MyGuild.AllyGuild(MyGuild);
                                    MyGuild.SendGuildMsg(posePlayer.MyGuild.GuildName + "行会已经和您的行会联盟成功。");
                                    posePlayer.MyGuild.SendGuildMsg(MyGuild.GuildName + "行会已经和您的行会联盟成功。");
                                    MyGuild.RefMemberName();
                                    posePlayer.MyGuild.RefMemberName();
                                    M2Share.WorldEngine.SendServerGroupMsg(Messages.SS_207, M2Share.ServerIndex, MyGuild.GuildName);
                                    M2Share.WorldEngine.SendServerGroupMsg(Messages.SS_207, M2Share.ServerIndex, posePlayer.MyGuild.GuildName);
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
                    SendDefMessage(Messages.SM_GUILDMAKEALLY_OK, 0, 0, 0, 0);
                }
                else
                {
                    SendDefMessage(Messages.SM_GUILDMAKEALLY_FAIL, n8, 0, 0, 0);
                }
            }
            catch (Exception e)
            {
                M2Share.Logger.Error(sExceptionMsg);
                M2Share.Logger.Error(e.Message);
            }
        }

        public void ClientGuildBreakAlly(string sGuildName)
        {
            if (!IsGuildMaster())
            {
                return;
            }
            var guildsuccess = false;
            var guild = M2Share.GuildMgr.FindGuild(sGuildName);
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
                    M2Share.WorldEngine.SendServerGroupMsg(Messages.SS_207, M2Share.ServerIndex, MyGuild.GuildName);
                    M2Share.WorldEngine.SendServerGroupMsg(Messages.SS_207, M2Share.ServerIndex, guild.GuildName);
                    guildsuccess = true;
                }
            }
            if (guildsuccess)
            {
                SendDefMessage(Messages.SM_GUILDBREAKALLY_OK, 0, 0, 0, 0);
            }
            else
            {
                SendDefMessage(Messages.SM_GUILDMAKEALLY_FAIL, 0, 0, 0, 0);
            }
        }

        private void ClientQueryRepairCost(int actorId, int nInt, string sMsg)
        {
            UserItem userItemA = null;
            for (var i = 0; i < ItemList.Count; i++)
            {
                var userItem = ItemList[i];
                if (userItem.MakeIndex == nInt)
                {
                    var sUserItemName = CustomItem.GetItemName(userItem);
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
            var merchant = M2Share.WorldEngine.FindMerchant(actorId);
            if (merchant != null && merchant.Envir == Envir && IsWithinSight(merchant))
            {
                // merchant.ClientQueryRepairCost(this, userItemA);
            }
        }

        private void ClientRepairItem(int actorId, int nInt, string sMsg)
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
            var merchant = M2Share.WorldEngine.FindMerchant(actorId);
            if (merchant != null && merchant.Envir == Envir && IsWithinSight(merchant))
            {
                // merchant.ClientRepairItem(this, userItem);
            }
        }

        private void ClientStorageItem(int actorId, int nItemIdx, string sMsg)
        {
            var bo19 = false;
            if (sMsg.Contains(' '))
            {
                HUtil32.GetValidStr3(sMsg, ref sMsg, ' ');
            }
            if (PayMent == 1 && !ModuleShare.Config.TryModeUseStorage)
            {
                SysMsg(Settings.TryModeCanotUseStorage, MsgColor.Red, MsgType.Hint);
                return;
            }
            var merchant = M2Share.WorldEngine.FindMerchant(actorId);
            for (var i = 0; i < ItemList.Count; i++)
            {
                var userItem = ItemList[i];
                var sUserItemName = CustomItem.GetItemName(userItem);// 取自定义物品名称
                if (userItem.MakeIndex == nItemIdx && string.Compare(sUserItemName, sMsg, StringComparison.OrdinalIgnoreCase) == 0) // 检查NPC是否允许存物品
                {
                    if (merchant != null && merchant.IsStorage && (merchant.Envir == Envir && IsWithinSight(merchant) || merchant == ModuleShare.FunctionNPC))
                    {
                        if (StorageItemList.Count < 39)
                        {
                            StorageItemList.Add(userItem);
                            ItemList.RemoveAt(i);
                            WeightChanged();
                            SendDefMessage(Messages.SM_STORAGE_OK, 0, 0, 0, 0);
                            var stdItem = ModuleShare.ItemSystem.GetStdItem(userItem.Index);
                            if (stdItem.NeedIdentify == 1)
                            {
                                // M2Share.EventSource.AddEventLog(1, MapName + "\t" + CurrX + "\t" + CurrY + "\t" + ChrName + "\t" + stdItem.Name + "\t" + userItem.MakeIndex + "\t" + '1' + "\t" + '0');
                            }
                        }
                        else
                        {
                            SendDefMessage(Messages.SM_STORAGE_FULL, 0, 0, 0, 0);
                        }
                        bo19 = true;
                    }
                    break;
                }
            }
            if (!bo19)
            {
                SendDefMessage(Messages.SM_STORAGE_FAIL, 0, 0, 0, 0);
            }
        }

        private void ClientTakeBackStorageItem(int actorId, int nItemIdx, string sMsg)
        {
            var bo19 = false;
            var merchant = M2Share.WorldEngine.FindMerchant(actorId);
            if (merchant == null)
            {
                return;
            }
            if (PayMent == 1 && !ModuleShare.Config.TryModeUseStorage)// '试玩模式不可以使用仓库功能!!!'
            {
                SysMsg(Settings.TryModeCanotUseStorage, MsgColor.Red, MsgType.Hint);
                return;
            }
            if (!IsCanGetBackItem)
            {
                //SendMsg(merchant, Messages.RM_MENU_OK, 0, ActorId, 0, 0, Settings.StorageIsLockedMsg + "\\ \\" + "仓库开锁命令: @" + CommandMgr.GameCommands.UnlockStorage.CmdName + '\\' + "仓库加锁命令: @" + CommandMgr.GameCommands.Lock.CmdName + '\\' + "设置密码命令: @" + CommandMgr.GameCommands.SetPassword.CmdName + '\\' + "修改密码命令: @" + CommandMgr.GameCommands.ChgPassword.CmdName);
                return;
            }
            for (var i = 0; i < StorageItemList.Count; i++)
            {
                var userItem = StorageItemList[i];
                var sUserItemName = CustomItem.GetItemName(userItem);
                if (userItem.MakeIndex == nItemIdx && string.Compare(sUserItemName, sMsg, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    if (IsAddWeightAvailable(ModuleShare.ItemSystem.GetStdItemWeight(userItem.Index)))// 检查NPC是否允许取物品
                    {
                        if (merchant.IsGetback && (merchant.Envir == Envir && IsWithinSight(merchant) || merchant == ModuleShare.FunctionNPC))
                        {
                            if (AddItemToBag(userItem))
                            {
                                SendAddItem(userItem);
                                StorageItemList.RemoveAt(i);
                                SendDefMessage(Messages.SM_TAKEBACKSTORAGEITEM_OK, nItemIdx, 0, 0, 0);
                                var stdItem = ModuleShare.ItemSystem.GetStdItem(userItem.Index);
                                if (stdItem.NeedIdentify == 1)
                                {
                                    // M2Share.EventSource.AddEventLog(0, MapName + "\t" + CurrX + "\t" + CurrY + "\t" + ChrName + "\t" + stdItem.Name + "\t" + userItem.MakeIndex + "\t" + '1' + "\t" + '0');
                                }
                            }
                            else
                            {
                                SendDefMessage(Messages.SM_TAKEBACKSTORAGEITEM_FULLBAG, 0, 0, 0, 0);
                            }
                            bo19 = true;
                        }
                    }
                    else
                    {
                        SysMsg(Settings.CanotGetItems, MsgColor.Red, MsgType.Hint);// '无法携带更多的东西!!!'
                    }
                    break;
                }
            }
            if (!bo19)
            {
                SendDefMessage(Messages.SM_TAKEBACKSTORAGEITEM_FAIL, 0, 0, 0, 0);
            }
        }

        private bool IsWithinSight(IActor merchant)
        {
            return Math.Abs(merchant.CurrX - CurrX) < 15 && Math.Abs(merchant.CurrY - CurrY) < 15;
        }
    }
}