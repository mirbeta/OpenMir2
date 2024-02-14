using GameSrv.DB;
using M2Server.Items;
using M2Server.Npc;
using OpenMir2.Enums;
using ScriptSystem.Consts;
using SystemModule.Actors;
using SystemModule.Const;
using SystemModule.ModuleEvent;

namespace GameSrv.Npc
{
    /// <summary>
    /// 交易NPC类
    /// 普通商人 如：药店和杂货店都在此实现
    /// </summary>
    public class Merchant : NormNpc, IMerchant
    {
        /// <summary>
        /// 脚本路径
        /// </summary>
        public string ScriptName { get; set; }
        /// <summary>
        /// 物品价格倍率 默认为 100%
        /// </summary>
        public int PriceRate { get; set; }
        /// <summary>
        /// 沙巴克城堡商人
        /// </summary>
        public bool CastleMerchant { get; set; }
        /// <summary>
        /// 刷新在售商品时间
        /// </summary>
        public int RefillGoodsTick { get; set; }
        /// <summary>
        /// 清理武器升级过期时间
        /// </summary>
        public int ClearExpreUpgradeTick { get; set; }
        /// <summary>
        /// NPC买卖物品类型列表，脚本中前面的 +1 +30 之类的
        /// </summary>
        public IList<int> ItemTypeList { get; set; }
        public IList<Goods> RefillGoodsList { get; set; }
        /// <summary>
        /// 商品列表
        /// </summary>
        public IList<IList<UserItem>> GoodsList { get; set; }
        /// <summary>
        /// 物品价格列表
        /// </summary>
        public IList<ItemPrice> ItemPriceList { get; set; }
        /// <summary>
        /// 物品升级列表
        /// </summary>
        public IList<WeaponUpgradeInfo> UpgradeWeaponList { get; set; }
        public bool BoCanMove { get; set; }
        public int MoveTime { get; set; }
        public int MoveTick { get; set; }
        /// <summary>
        /// 是否购买物品
        /// </summary>
        public bool IsBuy { get; set; }
        /// <summary>
        /// 是否交易物品
        /// </summary>
        public bool IsSell { get; set; }
        public bool IsMakeDrug { get; set; }
        public bool IsPrices { get; set; }
        public bool IsStorage { get; set; }
        public bool IsGetback { get; set; }
        public bool IsUpgradenow { get; set; }
        public bool IsGetBackupgnow { get; set; }
        public bool IsRepair { get; set; }
        public bool IsSupRepair { get; set; }
        public bool IsSendMsg { get; set; }
        public bool IsGetMarry { get; set; }
        public bool IsGetMaster { get; set; }
        public bool IsUseItemName { get; set; }
        public bool IsOffLineMsg { get; set; }
        public bool IsYbDeal { get; set; }
        public bool CanItemMarket { get; set; }

        public Merchant() : base()
        {
            RaceImg = ActorRace.Merchant;
            Appr = 0;
            PriceRate = 100;
            CastleMerchant = false;
            ItemTypeList = new List<int>();
            RefillGoodsList = new List<Goods>();
            GoodsList = new List<IList<UserItem>>();
            ItemPriceList = new List<ItemPrice>();
            UpgradeWeaponList = new List<WeaponUpgradeInfo>();
            RefillGoodsTick = HUtil32.GetTickCount();
            ClearExpreUpgradeTick = HUtil32.GetTickCount();
            IsBuy = false;
            IsSell = false;
            IsMakeDrug = false;
            IsPrices = false;
            IsStorage = false;
            IsGetback = false;
            IsUpgradenow = false;
            IsGetBackupgnow = false;
            IsRepair = false;
            IsSupRepair = false;
            IsGetMarry = false;
            IsGetMaster = false;
            IsUseItemName = false;
            MoveTick = HUtil32.GetTickCount();
            CellType = CellType.Merchant;
        }

        public override void Run()
        {
            try
            {
                int dwCurrentTick = HUtil32.GetTickCount();
                int dwDelayTick = ProcessRefillIndex * 500;
                if (dwCurrentTick < dwDelayTick)
                {
                    dwDelayTick = 0;
                }
                if (dwCurrentTick - RefillGoodsTick > 5 * 60 * 1000 + dwDelayTick)
                {
                    RefillGoodsTick = dwCurrentTick + dwDelayTick;
                    RefillGoods();
                }
                if ((dwCurrentTick - ClearExpreUpgradeTick) > 10 * 60 * 1000)
                {
                    ClearExpreUpgradeTick = dwCurrentTick;
                    ClearExpreUpgradeListData();
                }
                if (M2Share.RandomNumber.Random(50) == 0)
                {
                    TurnTo(M2Share.RandomNumber.RandomByte(8));
                }
                else
                {
                    if (M2Share.RandomNumber.Random(50) == 0)
                    {
                        SendRefMsg(Messages.RM_HIT, Dir, CurrX, CurrY, 0, "");
                    }
                }
                if (CastleMerchant && Castle != null && Castle.UnderWar)
                {
                    if (!FixedHideMode)
                    {
                        SendRefMsg(Messages.RM_DISAPPEAR, 0, 0, 0, 0, "");
                        FixedHideMode = true;
                    }
                }
                else
                {
                    if (FixedHideMode)
                    {
                        FixedHideMode = false;
                        SendRefMsg(Messages.RM_HIT, Dir, CurrX, CurrY, 0, "");
                    }
                }
                if (BoCanMove && (HUtil32.GetTickCount() - MoveTick) > MoveTime * 1000)
                {
                    MoveTick = HUtil32.GetTickCount();
                    SendRefMsg(Messages.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
                    MapRandomMove(MapName, 0);
                }
            }
            catch (Exception e)
            {
                LogService.Error(e.Message);
            }
            base.Run();
        }

        private void AddItemPrice(ushort nIndex, double nPrice)
        {
            ItemPrice itemPrice = new ItemPrice
            {
                wIndex = nIndex,
                nPrice = nPrice
            };
            ItemPriceList.Add(itemPrice);
            LocalDb.SaveGoodPriceRecord(this, ScriptName + '-' + MapName);
        }

        private void CheckItemPrice(ushort nIndex)
        {
            for (int i = 0; i < ItemPriceList.Count; i++)
            {
                ItemPrice itemPrice = ItemPriceList[i];
                if (itemPrice.wIndex == nIndex)
                {
                    double n10 = itemPrice.nPrice;
                    if (Math.Round(n10 * 1.1) > n10)
                    {
                        n10 = HUtil32.Round(n10 * 1.1);
                    }
                    return;
                }
            }
            StdItem stdItem = SystemShare.ItemSystem.GetStdItem(nIndex);
            if (stdItem != null)
            {
                AddItemPrice(nIndex, HUtil32.Round(stdItem.Price * 1.1));
            }
        }

        /// <summary>
        /// 刷新在售商品
        /// </summary>
        private void RefillGoods()
        {
            const string sExceptionMsg = "[Exception] TMerchant::RefillGoods {0}/{1}:{2} [{3}] Code:{4}";
            try
            {
                ushort nIndex;
                Goods goods;
                for (int i = 0; i < RefillGoodsList.Count; i++)
                {
                    goods = RefillGoodsList[i];
                    int goodCout = goods.Count;
                    if ((HUtil32.GetTickCount() - goods.RefillTick) > (goods.RefillTime * 60 * 1000))
                    {
                        goods.RefillTick = HUtil32.GetTickCount();
                        nIndex = SystemShare.ItemSystem.GetStdItemIdx(goods.ItemName);
                        if (nIndex > 0)
                        {
                            IList<UserItem> refillList = GetRefillList(nIndex);
                            int nRefillCount = refillList?.Count ?? 0;
                            if (goodCout > nRefillCount)
                            {
                                CheckItemPrice(nIndex);
                                RefillGoodsItems(ref refillList, goods.ItemName, goods.Count - nRefillCount);
                                LocalDb.SaveGoodRecord(this, ScriptName + '-' + MapName);
                                LocalDb.SaveGoodPriceRecord(this, ScriptName + '-' + MapName);
                                return;
                            }
                            if (goodCout < nRefillCount)
                            {
                                RefillDelReFillItem(ref refillList, nRefillCount - goods.Count);
                                LocalDb.SaveGoodRecord(this, ScriptName + '-' + MapName);
                                LocalDb.SaveGoodPriceRecord(this, ScriptName + '-' + MapName);
                            }
                        }
                    }
                }
                for (int i = 0; i < GoodsList.Count; i++)
                {
                    IList<UserItem> refillList20 = GoodsList[i];
                    if (refillList20.Count > 1000)
                    {
                        bool bo21 = false;
                        for (int j = 0; j < RefillGoodsList.Count; j++)
                        {
                            goods = RefillGoodsList[j];
                            nIndex = SystemShare.ItemSystem.GetStdItemIdx(goods.ItemName);
                            if (refillList20[0].Index == nIndex)
                            {
                                bo21 = true;
                                break;
                            }
                        }
                        if (!bo21)
                        {
                            RefillDelReFillItem(ref refillList20, refillList20.Count - 1000);
                        }
                        else
                        {
                            RefillDelReFillItem(ref refillList20, refillList20.Count - 5000);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogService.Error(Format(sExceptionMsg, ChrName, CurrX, CurrY, e.Message, ConditionCode.CHECK));
            }
        }

        private IList<UserItem> GetRefillList(int nIndex)
        {
            if (nIndex < 0)
            {
                return null;
            }
            for (int i = 0; i < GoodsList.Count; i++)
            {
                IList<UserItem> goods = GoodsList[i];
                if (goods.Count > 0)
                {
                    if (goods[0].Index == nIndex)
                    {
                        return goods;
                    }
                }
            }
            return null;
        }

        private void RefillGoodsItems(ref IList<UserItem> list, string sItemName, int nInt)
        {
            if (list == null)
            {
                list = new List<UserItem>();
                GoodsList.Add(list);
            }
            for (int i = 0; i < nInt; i++)
            {
                UserItem goodItem = new UserItem();
                if (SystemShare.ItemSystem.CopyToUserItemFromName(sItemName, ref goodItem))
                {
                    list.Insert(0, goodItem);
                }
                else
                {
                    Dispose(goodItem);
                }
            }
        }

        private static void RefillDelReFillItem(ref IList<UserItem> list, int nInt)
        {
            for (int i = list.Count - 1; i >= 0; i--)
            {
                if (nInt <= 0)
                {
                    break;
                }
                Dispose(list[i]);
                list.RemoveAt(i);
                nInt -= 1;
            }
        }

        private bool CheckItemType(int nStdMode)
        {
            bool result = false;
            for (int i = 0; i < ItemTypeList.Count; i++)
            {
                if (ItemTypeList[i] == nStdMode)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        private double GetItemPrice(ushort nIndex)
        {
            double result = -1;
            for (int i = 0; i < ItemPriceList.Count; i++)
            {
                ItemPrice itemPrice = ItemPriceList[i];
                if (itemPrice.wIndex == nIndex)
                {
                    result = itemPrice.nPrice;
                    break;
                }
            }
            if (result < 0)
            {
                StdItem stdItem = SystemShare.ItemSystem.GetStdItem(nIndex);
                if (stdItem != null)
                {
                    if (CheckItemType(stdItem.StdMode))
                    {
                        result = stdItem.Price;
                    }
                }
            }
            return result;
        }

        private void UpgradeWaponAddValue(IPlayerActor user, IList<UserItem> itemList, ref byte btDc, ref byte btSc, ref byte btMc, ref byte btDura)
        {
            ClientItem clientItem = null;
            IList<DeleteItem> delItemList = null;
            int nDc;
            int nSc;
            int nMc;
            int nDcMin = 0;
            int nDcMax = 0;
            int nScMin = 0;
            int nScMax = 0;
            int nMcMin = 0;
            int nMcMax = 0;
            int nDura = 0;
            int nItemCount = 0;
            IList<double> duraList = new List<double>();
            for (int i = itemList.Count - 1; i >= 0; i--)
            {
                UserItem userItem = itemList[i];
                if (SystemShare.ItemSystem.GetStdItemName(userItem.Index) == SystemShare.Config.BlackStone)
                {
                    duraList.Add(Math.Round(userItem.Dura / 1.0e3));
                    if (delItemList == null)
                    {
                        delItemList = new List<DeleteItem>();
                    }
                    delItemList.Add(new DeleteItem()
                    {
                        MakeIndex = userItem.MakeIndex,
                        ItemName = SystemShare.Config.BlackStone
                    });
                    DisPose(userItem);
                    itemList.RemoveAt(i);
                }
                else
                {
                    if (M2Share.IsAccessory(userItem.Index))
                    {
                        StdItem stdItem = SystemShare.ItemSystem.GetStdItem(userItem.Index);
                        if (stdItem != null)
                        {
                            SystemShare.ItemSystem.GetUpgradeStdItem(stdItem, userItem, ref clientItem);
                            //StdItem.GetItemAddValue(UserItem, ref StdItem80);
                            nDc = 0;
                            nSc = 0;
                            nMc = 0;
                            switch (clientItem.Item.StdMode)
                            {
                                case 19:
                                case 20:
                                case 21:
                                    nDc = HUtil32.HiWord(clientItem.Item.DC) + HUtil32.LoWord(clientItem.Item.DC);
                                    nSc = HUtil32.HiWord(clientItem.Item.SC) + HUtil32.LoWord(clientItem.Item.SC);
                                    nMc = HUtil32.HiWord(clientItem.Item.MC) + HUtil32.LoWord(clientItem.Item.MC);
                                    break;
                                case 22:
                                case 23:
                                    nDc = HUtil32.HiWord(clientItem.Item.DC) + HUtil32.LoWord(clientItem.Item.DC);
                                    nSc = HUtil32.HiWord(clientItem.Item.SC) + HUtil32.LoWord(clientItem.Item.SC);
                                    nMc = HUtil32.HiWord(clientItem.Item.MC) + HUtil32.LoWord(clientItem.Item.MC);
                                    break;
                                case 24:
                                case 26:
                                    nDc = HUtil32.HiWord(clientItem.Item.DC) + HUtil32.LoWord(clientItem.Item.DC) + 1;
                                    nSc = HUtil32.HiWord(clientItem.Item.SC) + HUtil32.LoWord(clientItem.Item.SC) + 1;
                                    nMc = HUtil32.HiWord(clientItem.Item.MC) + HUtil32.LoWord(clientItem.Item.MC) + 1;
                                    break;
                            }
                            if (nDcMin < nDc)
                            {
                                nDcMax = nDcMin;
                                nDcMin = nDc;
                            }
                            else
                            {
                                if (nDcMax < nDc)
                                {
                                    nDcMax = nDc;
                                }
                            }
                            if (nScMin < nSc)
                            {
                                nScMax = nScMin;
                                nScMin = nSc;
                            }
                            else
                            {
                                if (nScMax < nSc)
                                {
                                    nScMax = nSc;
                                }
                            }
                            if (nMcMin < nMc)
                            {
                                nMcMax = nMcMin;
                                nMcMin = nMc;
                            }
                            else
                            {
                                if (nMcMax < nMc)
                                {
                                    nMcMax = nMc;
                                }
                            }
                            if (delItemList == null)
                            {
                                delItemList = new List<DeleteItem>();
                            }
                            delItemList.Add(new DeleteItem()
                            {
                                ItemName = stdItem.Name,
                                MakeIndex = userItem.MakeIndex
                            });
                            if (stdItem.NeedIdentify == 1)
                            {
                                // M2Share.EventSource.AddEventLog(26, user.MapName + "\t" + user.CurrX + "\t" + user.CurrY + "\t" + user.ChrName + "\t" + stdItem.Name + "\t" + userItem.MakeIndex + "\t" + '1' + "\t" + '0');
                            }
                            DisPose(userItem);
                            itemList.RemoveAt(i);
                        }
                    }
                }
            }
            for (int i = 0; i < duraList.Count; i++)
            {
                for (int j = duraList.Count - 1; j > i; j--)
                {
                    if (duraList[j] > duraList[j - 1])
                    {
                        duraList.Reverse();
                    }
                }
            }
            for (int i = 0; i < duraList.Count; i++)
            {
                nDura = nDura + (int)duraList[i];
                nItemCount++;
                if (nItemCount >= 5)
                {
                    break;
                }
            }
            btDura = (byte)HUtil32.Round(HUtil32._MIN(5, nItemCount) + HUtil32._MIN(5, nItemCount) * (nDura / nItemCount / 5.0));
            btDc = (byte)(nDcMin / 5 + nDcMax / 3);
            btSc = (byte)(nScMin / 5 + nScMax / 3);
            btMc = (byte)(nMcMin / 5 + nMcMax / 3);
            if (delItemList != null)
            {
                int objectId = HUtil32.Sequence();
                SystemShare.ActorMgr.AddOhter(objectId, delItemList);
                user.SendMsg(user, Messages.RM_SENDDELITEMLIST, 0, objectId, 0, 0);
            }
        }

        private void UpgradeWapon(IPlayerActor user)
        {
            bool upgradeSuccess = false;
            WeaponUpgradeInfo upgradeInfo;
            for (int i = 0; i < UpgradeWeaponList.Count; i++)
            {
                upgradeInfo = UpgradeWeaponList[i];
                if (upgradeInfo.UserName == user.ChrName)
                {
                    M2Share.ScriptEngine.GotoLable(user, this.ActorId, ScriptFlagConst.sUPGRADEING);
                    return;
                }
            }
            if (user.UseItems[ItemLocation.Weapon] != null && user.UseItems[ItemLocation.Weapon].Index != 0 && user.Gold >= SystemShare.Config.UpgradeWeaponPrice
                && user.CheckItems(SystemShare.Config.BlackStone) != null)
            {
                user.DecGold(SystemShare.Config.UpgradeWeaponPrice);
                if (CastleMerchant || SystemShare.Config.GetAllNpcTax)
                {
                    if (Castle != null)
                    {
                        Castle.IncRateGold(SystemShare.Config.UpgradeWeaponPrice);
                    }
                    else if (SystemShare.Config.GetAllNpcTax)
                    {
                        SystemShare.CastleMgr.IncRateGold(SystemShare.Config.UpgradeWeaponPrice);
                    }
                }
                user.GoldChanged();
                upgradeInfo = new WeaponUpgradeInfo
                {
                    UserName = user.ChrName,
                    UserItem = new UserItem(user.UseItems[ItemLocation.Weapon])
                };
                StdItem stdItem = SystemShare.ItemSystem.GetStdItem(user.UseItems[ItemLocation.Weapon].Index);
                if (stdItem.NeedIdentify == 1)
                {
                    //  M2Share.EventSource.AddEventLog(25, user.MapName + "\t" + user.CurrX + "\t" + user.CurrY + "\t" + user.ChrName + "\t" + stdItem.Name + "\t" + user.UseItems[ItemLocation.Weapon].MakeIndex + "\t" + '1' + "\t" + '0');
                }
                user.SendDelItems(user.UseItems[ItemLocation.Weapon]);
                user.UseItems[ItemLocation.Weapon].Index = 0;
                user.RecalcAbilitys();
                user.FeatureChanged();
                user.SendMsg(Messages.RM_ABILITY, 0, 0, 0, 0);
                UpgradeWaponAddValue(user, user.ItemList, ref upgradeInfo.Dc, ref upgradeInfo.Sc, ref upgradeInfo.Mc, ref upgradeInfo.Dura);
                upgradeInfo.UpgradeTime = DateTime.Now;
                upgradeInfo.GetBackTick = HUtil32.GetTickCount();
                UpgradeWeaponList.Add(upgradeInfo);
                SaveUpgradingList();
                upgradeSuccess = true;
            }
            if (upgradeSuccess)
            {
                M2Share.ScriptEngine.GotoLable(user, this.ActorId, ScriptFlagConst.sUPGRADEOK);
            }
            else
            {
                M2Share.ScriptEngine.GotoLable(user, this.ActorId, ScriptFlagConst.sUPGRADEFAIL);
            }
        }

        /// <summary>
        /// 取回升级武器
        /// </summary>
        /// <param name="user"></param>
        private void GetBackupgWeapon(IPlayerActor user)
        {
            WeaponUpgradeInfo upgradeInfo = default;
            int nFlag = 0;
            if (!user.IsEnoughBag())
            {
                M2Share.ScriptEngine.GotoLable(user, this.ActorId, ScriptFlagConst.sGETBACKUPGFULL);
                return;
            }
            for (int i = 0; i < UpgradeWeaponList.Count; i++)
            {
                if (string.Compare(UpgradeWeaponList[i].UserName, user.ChrName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    nFlag = 1;
                    if (((HUtil32.GetTickCount() - UpgradeWeaponList[i].GetBackTick) > SystemShare.Config.UPgradeWeaponGetBackTime) || user.Permission >= 4)
                    {
                        upgradeInfo = UpgradeWeaponList[i];
                        UpgradeWeaponList.RemoveAt(i);
                        SaveUpgradingList();
                        nFlag = 2;
                        break;
                    }
                }
            }
            if (!string.IsNullOrEmpty(upgradeInfo.UserName))
            {
                if (HUtil32.RangeInDefined(upgradeInfo.Dura, 0, 8))
                {
                    if (upgradeInfo.UserItem.DuraMax > 3000)
                    {
                        upgradeInfo.UserItem.DuraMax -= 3000;
                    }
                    else
                    {
                        upgradeInfo.UserItem.DuraMax = (ushort)(upgradeInfo.UserItem.DuraMax >> 1);
                    }
                    if (upgradeInfo.UserItem.Dura > upgradeInfo.UserItem.DuraMax)
                    {
                        upgradeInfo.UserItem.Dura = upgradeInfo.UserItem.DuraMax;
                    }
                }
                else if (HUtil32.RangeInDefined(upgradeInfo.Dura, 9, 15))
                {
                    if (M2Share.RandomNumber.Random(upgradeInfo.Dura) < 6)
                    {
                        if (upgradeInfo.UserItem.DuraMax > 1000)
                        {
                            upgradeInfo.UserItem.DuraMax -= 1000;
                        }
                        if (upgradeInfo.UserItem.Dura > upgradeInfo.UserItem.DuraMax)
                        {
                            upgradeInfo.UserItem.Dura = upgradeInfo.UserItem.DuraMax;
                        }
                    }
                }
                else if (HUtil32.RangeInDefined(upgradeInfo.Dura, 18, 255))
                {
                    int r = M2Share.RandomNumber.Random(upgradeInfo.Dura - 18);
                    if (HUtil32.RangeInDefined(r, 1, 4))
                    {
                        upgradeInfo.UserItem.DuraMax += 1000;
                    }
                    else if (HUtil32.RangeInDefined(r, 5, 7))
                    {
                        upgradeInfo.UserItem.DuraMax += 2000;
                    }
                    else if (HUtil32.RangeInDefined(r, 8, 255))
                    {
                        upgradeInfo.UserItem.DuraMax += 4000;
                    }
                }
                int n1C;
                if (upgradeInfo.Dc == upgradeInfo.Mc && upgradeInfo.Mc == upgradeInfo.Sc)
                {
                    n1C = M2Share.RandomNumber.Random(3);
                }
                else
                {
                    n1C = -1;
                }
                int n90;
                int n10;
                if (upgradeInfo.Dc >= upgradeInfo.Mc && upgradeInfo.Dc >= upgradeInfo.Sc || (n1C == 0))
                {
                    n90 = HUtil32._MIN(11, upgradeInfo.Dc);
                    n10 = HUtil32._MIN(85, (n90 << 3 - n90) + 10 + upgradeInfo.UserItem.Desc[3] - upgradeInfo.UserItem.Desc[4] + user.BodyLuckLevel);
                    if (M2Share.RandomNumber.Random(SystemShare.Config.UpgradeWeaponDCRate) < n10)
                    {
                        upgradeInfo.UserItem.Desc[ItemAttr.WeaponUpgrade] = 10;
                        if (n10 > 63 && M2Share.RandomNumber.Random(SystemShare.Config.UpgradeWeaponDCTwoPointRate) == 0)
                        {
                            upgradeInfo.UserItem.Desc[ItemAttr.WeaponUpgrade] = 11;
                        }
                        if (n10 > 79 && M2Share.RandomNumber.Random(SystemShare.Config.UpgradeWeaponDCThreePointRate) == 0)
                        {
                            upgradeInfo.UserItem.Desc[ItemAttr.WeaponUpgrade] = 12;
                        }
                    }
                    else
                    {
                        upgradeInfo.UserItem.Desc[ItemAttr.WeaponUpgrade] = 1;
                    }
                }
                if (upgradeInfo.Mc >= upgradeInfo.Dc && upgradeInfo.Mc >= upgradeInfo.Sc || n1C == 1)
                {
                    n90 = HUtil32._MIN(11, upgradeInfo.Mc);
                    n10 = HUtil32._MIN(85, (n90 << 3 - n90) + 10 + upgradeInfo.UserItem.Desc[3] - upgradeInfo.UserItem.Desc[4] + user.BodyLuckLevel);
                    if (M2Share.RandomNumber.Random(SystemShare.Config.UpgradeWeaponMCRate) < n10)
                    {
                        upgradeInfo.UserItem.Desc[ItemAttr.WeaponUpgrade] = 20;
                        if (n10 > 63 && M2Share.RandomNumber.Random(SystemShare.Config.UpgradeWeaponMCTwoPointRate) == 0)
                        {
                            upgradeInfo.UserItem.Desc[ItemAttr.WeaponUpgrade] = 21;
                        }
                        if (n10 > 79 && M2Share.RandomNumber.Random(SystemShare.Config.UpgradeWeaponMCThreePointRate) == 0)
                        {
                            upgradeInfo.UserItem.Desc[ItemAttr.WeaponUpgrade] = 22;
                        }
                    }
                    else
                    {
                        upgradeInfo.UserItem.Desc[ItemAttr.WeaponUpgrade] = 1;
                    }
                }
                if (upgradeInfo.Sc >= upgradeInfo.Mc && upgradeInfo.Sc >= upgradeInfo.Dc || n1C == 2)
                {
                    n90 = HUtil32._MIN(11, upgradeInfo.Mc);
                    n10 = HUtil32._MIN(85, (n90 << 3 - n90) + 10 + upgradeInfo.UserItem.Desc[3] - upgradeInfo.UserItem.Desc[4] + user.BodyLuckLevel);
                    if (M2Share.RandomNumber.Random(SystemShare.Config.UpgradeWeaponSCRate) < n10)
                    {
                        upgradeInfo.UserItem.Desc[ItemAttr.WeaponUpgrade] = 30;
                        if (n10 > 63 && M2Share.RandomNumber.Random(SystemShare.Config.UpgradeWeaponSCTwoPointRate) == 0)
                        {
                            upgradeInfo.UserItem.Desc[ItemAttr.WeaponUpgrade] = 31;
                        }
                        if (n10 > 79 && M2Share.RandomNumber.Random(SystemShare.Config.UpgradeWeaponSCThreePointRate) == 0)
                        {
                            upgradeInfo.UserItem.Desc[ItemAttr.WeaponUpgrade] = 32;
                        }
                    }
                    else
                    {
                        upgradeInfo.UserItem.Desc[ItemAttr.WeaponUpgrade] = 1;
                    }
                }
                UserItem userItem = upgradeInfo.UserItem;
                StdItem stdItem = SystemShare.ItemSystem.GetStdItem(userItem.Index);
                if (stdItem.NeedIdentify == 1)
                {
                    //  M2Share.EventSource.AddEventLog(24, user.MapName + "\t" + user.CurrX + "\t" + user.CurrY + "\t" + user.ChrName + "\t" + stdItem.Name + "\t" + userItem.MakeIndex + "\t" + '1' + "\t" + '0');
                }
                //user.AddItemToBag(userItem);
                user.SendAddItem(userItem);
                DisPose(upgradeInfo);
            }
            switch (nFlag)
            {
                case 0:
                    M2Share.ScriptEngine.GotoLable(user, this.ActorId, ScriptFlagConst.sGETBACKUPGFAIL);
                    break;
                case 1:
                    M2Share.ScriptEngine.GotoLable(user, this.ActorId, ScriptFlagConst.sGETBACKUPGING);
                    break;
                case 2:
                    M2Share.ScriptEngine.GotoLable(user, this.ActorId, ScriptFlagConst.sGETBACKUPGOK);
                    break;
            }
        }

        /// <summary>
        /// 获取物品售卖价格
        /// </summary>
        /// <returns></returns>
        private int GetUserPrice(IPlayerActor playObject, double nPrice)
        {
            int result = 0;
            if (CastleMerchant)
            {
                if (Castle != null && Castle.IsMasterGuild(playObject.MyGuild)) //沙巴克成员修复物品打折
                {
                    int n14 = HUtil32._MAX(60, HUtil32.Round(PriceRate * (SystemShare.Config.CastleMemberPriceRate / 100.0)));//80%
                    result = HUtil32.Round(nPrice / 100.0 * n14);
                }
                else
                {
                    result = HUtil32.Round(nPrice / 100.0 * PriceRate);
                }
            }
            else
            {
                result = HUtil32.Round(nPrice / 100.0 * PriceRate);
            }
            return result;
        }

        public override void UserSelect(IPlayerActor playObject, string sData)
        {
            string sLabel = string.Empty;
            const string sExceptionMsg = "[Exception] TMerchant::UserSelect... Data: {0}";
            base.UserSelect(playObject, sData);
            try
            {
                if (!CastleMerchant || !(Castle != null && Castle.UnderWar))
                {
                    if (!playObject.Death && !string.IsNullOrEmpty(sData) && sData[0] == '@')
                    {
                        string sMsg = HUtil32.GetValidStr3(sData, ref sLabel, '\r');
                        playObject.ScriptLable = sData;
                        bool boCanJmp = playObject.LableIsCanJmp(sLabel);
                        if (string.Compare(sLabel, ScriptFlagConst.sSL_SENDMSG, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (string.IsNullOrEmpty(sMsg))
                            {
                                return;
                            }
                        }
                        M2Share.ScriptEngine.GotoLable(playObject, ActorId, sLabel, !boCanJmp);
                        if (!boCanJmp)
                        {
                            return;
                        }
                        if (string.Compare(sLabel, ScriptFlagConst.sOFFLINEMSG, StringComparison.OrdinalIgnoreCase) == 0)// 增加挂机
                        {
                            if (IsOffLineMsg)
                            {
                                SetOffLineMsg(playObject, sMsg);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptFlagConst.sSL_SENDMSG, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (IsSendMsg)
                            {
                                SendCustemMsg(playObject, sMsg);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptFlagConst.SuperRepair, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (IsSupRepair)
                            {
                                UserSelectSuperRepairItem(playObject);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptFlagConst.sBUY, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (IsBuy)
                            {
                                UserSelectBuyItem(playObject, 0);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptFlagConst.sSELL, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (IsSell)
                            {
                                UserSelectSellItem(playObject);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptFlagConst.sREPAIR, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (IsRepair)
                            {
                                UserSelectRepairItem(playObject);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptFlagConst.sMAKEDURG, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (IsMakeDrug)
                            {
                                UserSelectMakeDurg(playObject);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptFlagConst.sPRICES, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (IsPrices)
                            {
                                UserSelectItemPrices(playObject);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptFlagConst.sSTORAGE, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (IsStorage)
                            {
                                UserSelectStorage(playObject);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptFlagConst.sGETBACK, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (IsGetback)
                            {
                                UserSelectGetBack(playObject);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptFlagConst.sUPGRADENOW, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (IsUpgradenow)
                            {
                                UpgradeWapon(playObject);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptFlagConst.sGETBACKUPGNOW, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (IsGetBackupgnow)
                            {
                                GetBackupgWeapon(playObject);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptFlagConst.sGETMARRY, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (IsGetMarry)
                            {
                                GetBackupgWeapon(playObject);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptFlagConst.sGETMASTER, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (IsGetMaster)
                            {
                                GetBackupgWeapon(playObject);
                            }
                        }
                        else if (HUtil32.CompareLStr(sLabel, ScriptFlagConst.UseItemName))
                        {
                            if (IsUseItemName)
                            {
                                ChangeUseItemName(playObject, sLabel, sMsg);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptFlagConst.sEXIT, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            playObject.SendMsg(playObject, Messages.RM_MERCHANTDLGCLOSE, 0, ActorId, 0, 0);
                        }
                        else if (string.Compare(sLabel, ScriptFlagConst.sBACK, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (string.IsNullOrEmpty(playObject.ScriptGoBackLable))
                            {
                                playObject.ScriptGoBackLable = ScriptFlagConst.sMAIN;
                            }
                            M2Share.ScriptEngine.GotoLable(playObject, this.ActorId, playObject.ScriptGoBackLable);
                        }
                        else if (string.Compare(sLabel, ScriptFlagConst.sDealYBme, StringComparison.OrdinalIgnoreCase) == 0) // 元宝寄售:出售物品 
                        {
                            if (IsYbDeal)
                            {
                                UserSelectOpenDealOffForm(playObject); // 打开出售物品窗口
                            }
                        }
                        else //插件消息
                        {
                            SystemShare.Mediator.Publish(new UserSelectMessageEvent { Actor = playObject, Lable = sLabel, NormNpc = this });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogService.Error(Format(sExceptionMsg, sData));
                LogService.Error(ex.StackTrace);
            }
        }

        /// <summary>
        /// 特殊修理物品
        /// </summary>
        private void UserSelectSuperRepairItem(IPlayerActor user)
        {
            user.SendMsg(this, Messages.RM_SENDUSERSREPAIR, 0, ActorId, 0, 0);
        }

        /// <summary>
        /// 物品详情列表
        /// </summary>
        private void UserSelectBuyItem(IPlayerActor user, int nInt)
        {
            string sSendMsg = string.Empty;
            int n10 = 0;
            for (int i = 0; i < GoodsList.Count; i++)
            {
                IList<UserItem> goods = GoodsList[i];
                UserItem userItem = goods[0];
                StdItem stdItem = SystemShare.ItemSystem.GetStdItem(userItem.Index);
                if (stdItem != null)
                {
                    string sName = CustomItemSystem.GetItemName(userItem);
                    int nPrice = GetUserPrice(user, GetItemPrice(userItem.Index));
                    int nStock = goods.Count;
                    short nSubMenu;
                    if (stdItem.StdMode <= 4 || stdItem.StdMode == 42 || stdItem.StdMode == 31)
                    {
                        nSubMenu = 0;
                    }
                    else
                    {
                        nSubMenu = 1;
                    }
                    sSendMsg = sSendMsg + sName + '/' + nSubMenu + '/' + nPrice + '/' + nStock + '/';
                    n10++;
                }
            }
            user.SendMsg(Messages.RM_SENDGOODSLIST, 0, ActorId, n10, 0, sSendMsg);
        }

        private void UserSelectSellItem(IPlayerActor user)
        {
            user.SendMsg(Messages.RM_SENDUSERSELL, 0, ActorId, 0, 0);
        }

        private void UserSelectRepairItem(IPlayerActor user)
        {
            user.SendMsg(Messages.RM_SENDUSERREPAIR, 0, ActorId, 0, 0);
        }

        private void UserSelectMakeDurg(IPlayerActor user)
        {
            string sSendMsg = string.Empty;
            for (int i = 0; i < GoodsList.Count; i++)
            {
                StdItem stdItem = SystemShare.ItemSystem.GetStdItem(GoodsList[i][0].Index);
                if (stdItem != null)
                {
                    sSendMsg = sSendMsg + stdItem.Name + '/' + 0 + '/' + SystemShare.Config.MakeDurgPrice + '/' + 1 + '/';
                }
            }
            if (!string.IsNullOrEmpty(sSendMsg))
            {
                user.SendMsg(Messages.RM_USERMAKEDRUGITEMLIST, 0, ActorId, 0, 0, sSendMsg);
            }
        }

        private static void UserSelectItemPrices(IPlayerActor user)
        {

        }

        private void UserSelectStorage(IPlayerActor user)
        {
            user.SendMsg(Messages.RM_USERSTORAGEITEM, 0, ActorId, 0, 0);
        }

        private void UserSelectGetBack(IPlayerActor user)
        {
            user.SendMsg(Messages.RM_USERGETBACKITEM, 0, ActorId, 0, 0);
        }

        /// <summary>
        /// 打开出售物品窗口
        /// </summary>
        /// <param name="user"></param>
        private void UserSelectOpenDealOffForm(IPlayerActor user)
        {
            if (user.SaleDeal)
            {
                if (!user.SellOffInTime(0))
                {
                    user.SendMsg(Messages.RM_SENDDEALOFFFORM, 0, ActorId, 0, 0);
                    //user.GetBackSellOffItems();
                }
                else
                {
                    user.SendMsg(this, Messages.RM_MERCHANTSAY, 0, 0, 0, 0, ChrName + "/您还有元宝服务正在进行!!\\ \\<返回/@main>");
                }
            }
            else
            {
                user.SendMsg(this, Messages.RM_MERCHANTSAY, 0, 0, 0, 0, ChrName + "/您未开通元宝服务,请先开通元宝服务!!\\ \\<返回/@main>");
            }
        }

        public void LoadNpcData()
        {
            string sFile = ScriptName + '-' + MapName;
            LocalDb.LoadGoodRecord(this, sFile);
            LocalDb.LoadGoodPriceRecord(this, sFile);
            LoadUpgradeList();
        }

        private void SaveNpcData()
        {
            string sFile = ScriptName + '-' + MapName;
            LocalDb.SaveGoodRecord(this, sFile);
            LocalDb.SaveGoodPriceRecord(this, sFile);
        }

        /// <summary>
        /// 清理武器升级过期数据
        /// </summary>
        private void ClearExpreUpgradeListData()
        {
            for (int i = UpgradeWeaponList.Count - 1; i >= 0; i--)
            {
                WeaponUpgradeInfo upgradeInfo = UpgradeWeaponList[i];
                if ((DateTime.Now - upgradeInfo.UpgradeTime).TotalDays >= SystemShare.Config.ClearExpireUpgradeWeaponDays)
                {
                    Dispose(upgradeInfo);
                    UpgradeWeaponList.RemoveAt(i);
                }
            }
        }

        public void LoadMerchantScript()
        {
            ItemTypeList.Clear();
            Path = ScriptFlagConst.sMarket_Def;
            string scriptPath = ScriptName + '-' + MapName;
            M2Share.ScriptParsers.LoadScriptFile(this, "Market_Def", scriptPath, true);
        }

        public override void Click(IPlayerActor playObject)
        {
            base.Click(playObject);
        }

        protected override void GetVariableText(IPlayerActor playObject, string sVariable, ref string sMsg)
        {
            string sText;
            base.GetVariableText(playObject, sVariable, ref sMsg);
            switch (sVariable)
            {
                case "$PRICERATE":
                    sText = PriceRate.ToString();
                    sMsg = ReplaceVariableText(sMsg, "<$PRICERATE>", sText);
                    break;
                case "$UPGRADEWEAPONFEE":
                    sText = SystemShare.Config.UpgradeWeaponPrice.ToString();
                    sMsg = ReplaceVariableText(sMsg, "<$UPGRADEWEAPONFEE>", sText);
                    break;
                case "$USERWEAPON":
                    if (playObject.UseItems[ItemLocation.Weapon] != null && playObject.UseItems[ItemLocation.Weapon].Index != 0)
                    {
                        sText = SystemShare.ItemSystem.GetStdItemName(playObject.UseItems[ItemLocation.Weapon].Index);
                    }
                    else
                    {
                        sText = "无";
                    }
                    sMsg = ReplaceVariableText(sMsg, "<$USERWEAPON>", sText);
                    break;
            }
        }

        private double GetUserItemPrice(UserItem userItem)
        {
            double itemPrice = GetItemPrice(userItem.Index);
            if (itemPrice > 0)
            {
                StdItem stdItem = SystemShare.ItemSystem.GetStdItem(userItem.Index);
                if (stdItem != null && stdItem.StdMode > 4 && stdItem.DuraMax > 0 && userItem.DuraMax > 0)
                {
                    double n20;
                    switch (stdItem.StdMode)
                    {
                        case 40:// 肉
                            {
                                if (userItem.Dura <= userItem.DuraMax)
                                {
                                    n20 = itemPrice / 2.0 / userItem.DuraMax * (userItem.DuraMax - userItem.Dura);
                                    itemPrice = HUtil32._MAX(2, HUtil32.Round(itemPrice - n20));
                                }
                                else
                                {
                                    itemPrice = itemPrice + HUtil32.Round(itemPrice / userItem.DuraMax * 2.0 * (userItem.DuraMax - userItem.Dura));
                                }
                                return itemPrice;
                            }
                        case 43:
                            {
                                if (userItem.DuraMax < 10000)
                                {
                                    userItem.DuraMax = 10000;
                                }
                                if (userItem.Dura <= userItem.DuraMax)
                                {
                                    n20 = itemPrice / 2.0 / userItem.DuraMax * (userItem.DuraMax - userItem.Dura);
                                    itemPrice = HUtil32._MAX(2, HUtil32.Round(itemPrice - n20));
                                }
                                else
                                {
                                    itemPrice = itemPrice + HUtil32.Round(itemPrice / userItem.DuraMax * 1.3 * (userItem.DuraMax - userItem.Dura));
                                }
                                return itemPrice;
                            }
                        case > 4:
                            {
                                int n14 = 0;
                                int nC = 0;
                                while (true)
                                {
                                    if (stdItem.StdMode == 5 || stdItem.StdMode == 6)
                                    {
                                        if (nC != 4 || nC != 9)
                                        {
                                            if (nC == 6)
                                            {
                                                if (userItem.Desc[nC] > 10)
                                                {
                                                    n14 = n14 + (userItem.Desc[nC] - 10) * 2;
                                                }
                                            }
                                            else
                                            {
                                                n14 = n14 + userItem.Desc[nC];
                                            }
                                        }
                                    }
                                    else
                                    {
                                        n14 += userItem.Desc[nC];
                                    }
                                    nC++;
                                    if (nC >= 8)
                                    {
                                        break;
                                    }
                                }
                                if (n14 > 0)
                                {
                                    itemPrice = itemPrice / 5 * n14;
                                }
                                itemPrice = HUtil32.Round(itemPrice / stdItem.DuraMax * userItem.DuraMax);
                                n20 = itemPrice / 2.0 / userItem.DuraMax * (userItem.DuraMax - userItem.Dura);
                                itemPrice = HUtil32._MAX(2, HUtil32.Round(itemPrice - n20));
                                return itemPrice;
                            }
                    }
                }
            }
            return itemPrice;
        }

        public void ClientBuyItem(IPlayerActor playObject, string sItemName, int nInt)
        {
            bool bo29 = false;
            int n1C = 1;
            for (int i = 0; i < GoodsList.Count; i++)
            {
                if (bo29)
                {
                    break;
                }
                IList<UserItem> list20 = GoodsList[i];
                UserItem userItem = list20[0];
                StdItem stdItem = SystemShare.ItemSystem.GetStdItem(userItem.Index);
                if (stdItem != null)
                {
                    string sUserItemName = CustomItemSystem.GetItemName(userItem);
                    if (playObject.IsAddWeightAvailable(stdItem.Weight))
                    {
                        if (sUserItemName.Equals(sItemName))
                        {
                            for (int j = 0; j < list20.Count; j++)
                            {
                                userItem = list20[j];
                                if (stdItem.StdMode <= 4 || stdItem.StdMode == 42 || stdItem.StdMode == 31 || userItem.MakeIndex == nInt)
                                {
                                    int nPrice = GetUserPrice(playObject, GetUserItemPrice(userItem));
                                    if (playObject.Gold >= nPrice && nPrice > 0)
                                    {
                                        if (playObject.AddItemToBag(userItem))
                                        {
                                            playObject.Gold -= nPrice;
                                            if (CastleMerchant || SystemShare.Config.GetAllNpcTax)
                                            {
                                                if (Castle != null)
                                                {
                                                    Castle.IncRateGold(nPrice);
                                                }
                                                else if (SystemShare.Config.GetAllNpcTax)
                                                {
                                                    SystemShare.CastleMgr.IncRateGold(SystemShare.Config.UpgradeWeaponPrice);
                                                }
                                            }
                                            playObject.SendAddItem(userItem);
                                            if (stdItem.NeedIdentify == 1)
                                            {
                                                //  M2Share.EventSource.AddEventLog(9, playObject.MapName + "\t" + playObject.CurrX + "\t" + playObject.CurrY + "\t" + playObject.ChrName + "\t" + stdItem.Name + "\t" + userItem.MakeIndex + "\t" + '1' + "\t" + ChrName);
                                            }
                                            list20.RemoveAt(j);
                                            if (list20.Count <= 0)
                                            {
                                                GoodsList.RemoveAt(i);
                                            }
                                            n1C = 0;
                                        }
                                        else
                                        {
                                            n1C = 2;
                                        }
                                    }
                                    else
                                    {
                                        n1C = 3;
                                    }
                                    bo29 = true;
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        n1C = 2;
                    }
                }
            }
            if (n1C == 0)
            {
                playObject.SendMsg(this, Messages.RM_BUYITEM_SUCCESS, 0, playObject.Gold, nInt, 0);
            }
            else
            {
                playObject.SendMsg(this, Messages.RM_BUYITEM_FAIL, 0, n1C, 0, 0);
            }
        }

        public void ClientGetDetailGoodsList(IPlayerActor playObject, string sItemName, int nInt)
        {
            string sSendMsg = string.Empty;
            int nItemCount = 0;
            for (int i = 0; i < GoodsList.Count; i++)
            {
                IList<UserItem> list20 = GoodsList[i];
                if (list20.Count <= 0)
                {
                    continue;
                }
                UserItem userItem = list20[0];
                StdItem item = SystemShare.ItemSystem.GetStdItem(userItem.Index);
                if (item != null && item.Name == sItemName)
                {
                    if (list20.Count - 1 < nInt)
                    {
                        nInt = HUtil32._MAX(0, list20.Count - 10);
                    }
                    for (int j = list20.Count - 1; j >= 0; j--)
                    {
                        userItem = list20[j];
                        ClientItem clientItem = new ClientItem();
                        SystemShare.ItemSystem.GetUpgradeStdItem(item, userItem, ref clientItem);
                        //Item.GetItemAddValue(UserItem, ref ClientItem.Item);
                        clientItem.Dura = userItem.Dura;
                        clientItem.DuraMax = (ushort)GetUserPrice(playObject, GetUserItemPrice(userItem));
                        clientItem.MakeIndex = userItem.MakeIndex;
                        sSendMsg = sSendMsg + EDCode.EncodeBuffer(clientItem) + "/";
                        nItemCount++;
                        if (nItemCount >= 10)
                        {
                            break;
                        }
                    }
                    break;
                }
            }
            playObject.SendMsg(Messages.RM_SENDDETAILGOODSLIST, 0, ActorId, nItemCount, nInt, sSendMsg);
        }

        public void ClientQuerySellPrice(IPlayerActor playObject, UserItem userItem)
        {
            int nC = GetSellItemPrice(GetUserItemPrice(userItem));
            if (nC >= 0)
            {
                playObject.SendMsg(Messages.RM_SENDBUYPRICE, 0, nC, 0, 0);
            }
            else
            {
                playObject.SendMsg(Messages.RM_SENDBUYPRICE, 0, 0, 0, 0);
            }
        }

        private static int GetSellItemPrice(double nPrice)
        {
            return HUtil32.Round(nPrice / 2.0);
        }

        private static bool ClientSellItem_sub_4A1C84(UserItem userItem)
        {
            bool result = true;
            StdItem stdItem = SystemShare.ItemSystem.GetStdItem(userItem.Index);
            if (stdItem != null && (stdItem.StdMode == 25 || stdItem.StdMode == 30))
            {
                if (userItem.Dura < 4000)
                {
                    result = false;
                }
            }
            return result;
        }

        public bool ClientSellItem(IPlayerActor playObject, UserItem userItem)
        {
            bool result = false;
            int nPrice = GetSellItemPrice(GetUserItemPrice(userItem));
            if (nPrice > 0 && ClientSellItem_sub_4A1C84(userItem))
            {
                if (playObject.IncGold(nPrice))
                {
                    if (CastleMerchant || SystemShare.Config.GetAllNpcTax)
                    {
                        if (Castle != null)
                        {
                            Castle.IncRateGold(nPrice);
                        }
                        else if (SystemShare.Config.GetAllNpcTax)
                        {
                            SystemShare.CastleMgr.IncRateGold(SystemShare.Config.UpgradeWeaponPrice);
                        }
                    }
                    playObject.SendMsg(Messages.RM_USERSELLITEM_OK, 0, playObject.Gold, 0, 0);
                    AddItemToGoodsList(userItem);
                    StdItem stdItem = SystemShare.ItemSystem.GetStdItem(userItem.Index);
                    if (stdItem.NeedIdentify == 1)
                    {
                        // M2Share.EventSource.AddEventLog(10, playObject.MapName + "\t" + playObject.CurrX + "\t" + playObject.CurrY + "\t" + playObject.ChrName + "\t" + stdItem.Name + "\t" + userItem.MakeIndex + "\t" + '1' + "\t" + ChrName);
                    }
                    result = true;
                }
                else
                {
                    playObject.SendMsg(Messages.RM_USERSELLITEM_FAIL, 0, 0, 0, 0);
                }
            }
            else
            {
                playObject.SendMsg(Messages.RM_USERSELLITEM_FAIL, 0, 0, 0, 0);
            }
            return result;
        }

        private void AddItemToGoodsList(UserItem userItem)
        {
            if (userItem.Dura <= 0)
            {
                return;
            }
            IList<UserItem> itemList = GetRefillList(userItem.Index);
            if (itemList == null)
            {
                itemList = new List<UserItem>();
                GoodsList.Add(itemList);
            }
            itemList.Insert(0, userItem);
        }

        private bool ClientMakeDrugCheckNeedItem(IPlayerActor playObject, string sItemName)
        {
            IList<MakeItem> list10 = M2Share.GetMakeItemInfo(sItemName);
            if (list10 == null)
            {
                return false;
            }
            bool result = true;
            string s20;
            int n1C;
            for (int i = 0; i < list10.Count; i++)
            {
                s20 = list10[i].ItemName;
                n1C = list10[i].ItemCount;
                for (int j = 0; j < playObject.ItemList.Count; j++)
                {
                    if (SystemShare.ItemSystem.GetStdItemName(playObject.ItemList[j].Index) == s20)
                    {
                        n1C -= 1;
                    }
                }
                if (n1C > 0)
                {
                    result = false;
                    break;
                }
            }
            if (result)
            {
                IList<DeleteItem> list28 = null;
                for (int i = 0; i < list10.Count; i++)
                {
                    s20 = list10[i].ItemName;
                    n1C = list10[i].ItemCount;
                    for (int j = playObject.ItemList.Count - 1; j >= 0; j--)
                    {
                        if (n1C <= 0)
                        {
                            break;
                        }
                        UserItem userItem = playObject.ItemList[j];
                        if (SystemShare.ItemSystem.GetStdItemName(userItem.Index) == s20)
                        {
                            if (list28 == null)
                            {
                                list28 = new List<DeleteItem>();
                            }
                            list28.Add(new DeleteItem()
                            {
                                ItemName = s20,
                                MakeIndex = userItem.MakeIndex
                            });
                            Dispose(userItem);
                            playObject.ItemList.RemoveAt(j);
                            n1C -= 1;
                        }
                    }
                }
                if (list28 != null)
                {
                    int objectId = HUtil32.Sequence();
                    SystemShare.ActorMgr.AddOhter(objectId, list28);
                    playObject.SendMsg(this, Messages.RM_SENDDELITEMLIST, 0, objectId, 0, 0);
                }
            }
            return result;
        }

        public void ClientMakeDrugItem(IPlayerActor playObject, string sItemName)
        {
            byte n14 = 1;
            for (int i = 0; i < GoodsList.Count; i++)
            {
                IList<UserItem> list1C = GoodsList[i];
                UserItem makeItem = list1C[0];
                StdItem stdItem = SystemShare.ItemSystem.GetStdItem(makeItem.Index);
                if (stdItem != null && string.Compare(stdItem.Name, sItemName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    if (playObject.Gold >= SystemShare.Config.MakeDurgPrice)
                    {
                        if (ClientMakeDrugCheckNeedItem(playObject, sItemName))
                        {
                            UserItem userItem = new UserItem();
                            SystemShare.ItemSystem.CopyToUserItemFromName(sItemName, ref userItem);
                            if (playObject.AddItemToBag(userItem))
                            {
                                playObject.Gold -= SystemShare.Config.MakeDurgPrice;
                                playObject.SendAddItem(userItem);
                                stdItem = SystemShare.ItemSystem.GetStdItem(userItem.Index);
                                if (stdItem.NeedIdentify == 1)
                                {
                                    //  M2Share.EventSource.AddEventLog(2, playObject.MapName + "\t" + playObject.CurrX + "\t" + playObject.CurrY + "\t" + playObject.ChrName + "\t" + stdItem.Name + "\t" + userItem.MakeIndex + "\t" + '1' + "\t" + ChrName);
                                }
                                n14 = 0;
                                break;
                            }
                            DisPose(userItem);
                            n14 = 2;
                        }
                        else
                        {
                            n14 = 4;
                        }
                    }
                    else
                    {
                        n14 = 3;
                    }
                }
            }
            if (n14 == 0)
            {
                playObject.SendMsg(Messages.RM_MAKEDRUG_SUCCESS, 0, playObject.Gold, 0, 0);
            }
            else
            {
                playObject.SendMsg(Messages.RM_MAKEDRUG_FAIL, 0, n14, 0, 0);
            }
        }

        /// <summary>
        /// 客户查询修复所需成本
        /// </summary>
        public void ClientQueryRepairCost(IPlayerActor playObject, UserItem userItem)
        {
            int nPrice = GetUserPrice(playObject, GetUserItemPrice(userItem));
            if (nPrice > 0 && userItem.DuraMax > userItem.Dura)
            {
                int nRepairPrice;
                if (userItem.DuraMax > 0)
                {
                    nRepairPrice = HUtil32.Round((nPrice / 3.0) / userItem.DuraMax * (userItem.DuraMax - userItem.Dura));
                }
                else
                {
                    nRepairPrice = nPrice;
                }
                if (string.Compare(playObject.ScriptLable, ScriptFlagConst.SuperRepair, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    if (IsSupRepair)
                    {
                        nRepairPrice = nRepairPrice * SystemShare.Config.SuperRepairPriceRate;
                    }
                    else
                    {
                        nRepairPrice = -1;
                    }
                }
                else
                {
                    if (!IsRepair)
                    {
                        nRepairPrice = -1;
                    }
                }
                playObject.SendMsg(Messages.RM_SENDREPAIRCOST, 0, nRepairPrice, 0, 0);
            }
            else
            {
                playObject.SendMsg(Messages.RM_SENDREPAIRCOST, 0, -1, 0, 0);
            }
        }

        /// <summary>
        /// 修理物品
        /// </summary>
        /// <returns></returns>
        public void ClientRepairItem(IPlayerActor playObject, UserItem userItem)
        {
            bool supRepair = string.Compare(playObject.ScriptLable, ScriptFlagConst.SuperRepair, StringComparison.OrdinalIgnoreCase) == 0;
            if (supRepair && !IsSupRepair)
            {
                return;//不支持特殊修理
            }
            if (!supRepair && !IsRepair)
            {
                return; //不支持修理和特殊修理
            }
            if (string.Compare(playObject.ScriptLable, ScriptFlagConst.Superrepairfail, StringComparison.OrdinalIgnoreCase) == 0)
            {
                SendMsgToUser(playObject, "对不起!我不能帮你修理这个物品。\\ \\ \\<返回/@main>");
                playObject.SendMsg(Messages.RM_USERREPAIRITEM_FAIL, 0, 0, 0, 0);
                return;
            }
            int nPrice = GetUserPrice(playObject, GetUserItemPrice(userItem));
            if (supRepair)
            {
                nPrice = nPrice * SystemShare.Config.SuperRepairPriceRate;
            }
            StdItem stdItem = SystemShare.ItemSystem.GetStdItem(userItem.Index);
            if (stdItem != null)
            {
                if (nPrice > 0 && userItem.DuraMax > userItem.Dura && stdItem.StdMode != 43)
                {
                    int nRepairPrice;
                    if (userItem.DuraMax > 0)
                    {
                        nRepairPrice = HUtil32.Round(nPrice / 3.0 / userItem.DuraMax * (userItem.DuraMax - userItem.Dura));
                    }
                    else
                    {
                        nRepairPrice = nPrice;
                    }
                    if (playObject.DecGold(nRepairPrice))
                    {
                        if (CastleMerchant || SystemShare.Config.GetAllNpcTax)
                        {
                            if (Castle != null)
                            {
                                Castle.IncRateGold(nRepairPrice);
                            }
                            else if (SystemShare.Config.GetAllNpcTax)
                            {
                                SystemShare.CastleMgr.IncRateGold(SystemShare.Config.UpgradeWeaponPrice);
                            }
                        }
                        if (supRepair)
                        {
                            userItem.Dura = userItem.DuraMax;
                            playObject.SendMsg(Messages.RM_USERREPAIRITEM_OK, 0, playObject.Gold, userItem.Dura, userItem.DuraMax);
                            M2Share.ScriptEngine.GotoLable(playObject, ActorId, ScriptFlagConst.sSUPERREPAIROK);
                        }
                        else
                        {
                            userItem.DuraMax -= (ushort)((userItem.DuraMax - userItem.Dura) / SystemShare.Config.RepairItemDecDura);
                            userItem.Dura = userItem.DuraMax;
                            playObject.SendMsg(Messages.RM_USERREPAIRITEM_OK, 0, playObject.Gold, userItem.Dura, userItem.DuraMax);
                            M2Share.ScriptEngine.GotoLable(playObject, ActorId, ScriptFlagConst.sREPAIROK);
                        }
                    }
                    else
                    {
                        playObject.SendMsg(Messages.RM_USERREPAIRITEM_FAIL, 0, 0, 0, 0);
                    }
                }
                else
                {
                    playObject.SendMsg(Messages.RM_USERREPAIRITEM_FAIL, 0, 0, 0, 0);
                }
            }
        }

        public override void ClearScript()
        {
            IsBuy = false;
            IsSell = false;
            IsMakeDrug = false;
            IsPrices = false;
            IsStorage = false;
            IsGetback = false;
            IsUpgradenow = false;
            IsGetBackupgnow = false;
            IsRepair = false;
            IsSupRepair = false;
            IsGetMarry = false;
            IsGetMaster = false;
            IsUseItemName = false;
            base.ClearScript();
        }

        private void LoadUpgradeList()
        {
            UpgradeWeaponList.Clear();
            try
            {
                GameShare.DataSource.LoadUpgradeWeaponRecord(ScriptName + '-' + MapName, UpgradeWeaponList);
            }
            catch
            {
                LogService.Error("Failure in loading upgradinglist - " + ChrName);
            }
        }

        private void SaveUpgradingList()
        {
            try
            {
                GameShare.DataSource.SaveUpgradeWeaponRecord(ScriptName + '-' + MapName, UpgradeWeaponList);
            }
            catch
            {
                LogService.Error("Failure in saving upgradinglist - " + ChrName);
            }
        }

        /// <summary>
        /// 设置挂机留言信息
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sMsg"></param>
        protected static void SetOffLineMsg(IPlayerActor playObject, string sMsg)
        {
            playObject.OffLineLeaveWord = sMsg;
        }

        protected override void SendCustemMsg(IPlayerActor playObject, string sMsg)
        {
            base.SendCustemMsg(playObject, sMsg);
        }

        /// <summary>
        /// 清除临时文件，包括交易库存，价格表
        /// </summary>
        public void ClearData()
        {
            const string sExceptionMsg = "[Exception] TMerchant::ClearData";
            try
            {
                for (int i = 0; i < GoodsList.Count; i++)
                {
                    IList<UserItem> itemList = GoodsList[i];
                    for (int j = 0; j < itemList.Count; j++)
                    {
                        UserItem userItem = itemList[j];
                        Dispose(userItem);
                    }
                }
                GoodsList.Clear();
                for (int i = 0; i < ItemPriceList.Count; i++)
                {
                    ItemPrice itemPrice = ItemPriceList[i];
                    Dispose(itemPrice);
                }
                ItemPriceList.Clear();
                SaveNpcData();
            }
            catch (Exception e)
            {
                LogService.Error(sExceptionMsg);
                LogService.Error(e.Message);
            }
        }

        private void ChangeUseItemName(IPlayerActor playObject, string sLabel, string sItemName)
        {
            if (!playObject.BoChangeItemNameFlag)
            {
                return;
            }
            playObject.BoChangeItemNameFlag = false;
            string sWhere = sLabel[ScriptFlagConst.UseItemName.Length..];
            byte btWhere = (byte)HUtil32.StrToInt(sWhere, -1);
            if (btWhere >= 0 && btWhere <= playObject.UseItems.Length)
            {
                UserItem userItem = playObject.UseItems[btWhere];
                if (userItem.Index == 0)
                {
                    string sMsg = Format(MessageSettings.YourUseItemIsNul, SystemShare.GetUseItemName(btWhere));
                    playObject.SendMsg(this, Messages.RM_MENU_OK, 0, playObject.ActorId, 0, 0, sMsg);
                    return;
                }
                if (userItem.Desc[13] == 1)
                {
                    M2Share.CustomItemMgr.DelCustomItemName(userItem.MakeIndex, userItem.Index);
                }
                if (!string.IsNullOrEmpty(sItemName))
                {
                    M2Share.CustomItemMgr.AddCustomItemName(userItem.MakeIndex, userItem.Index, sItemName);
                    userItem.Desc[13] = 1;
                }
                else
                {
                    M2Share.CustomItemMgr.DelCustomItemName(userItem.MakeIndex, userItem.Index);
                    userItem.Desc[13] = 0;
                }
                M2Share.CustomItemMgr.SaveCustomItemName();
                playObject.SendMsg(playObject, Messages.RM_SENDUSEITEMS, 0, 0, 0, 0);
                playObject.SendMsg(this, Messages.RM_MENU_OK, 0, playObject.ActorId, 0, 0);
            }
        }

        private static void DisPose(object obj)
        {
            obj = null;
        }
    }
}