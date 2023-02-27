using GameSrv.Items;
using GameSrv.Maps;
using GameSrv.Player;
using GameSrv.Script;
using SystemModule.Data;
using SystemModule.Enums;
using SystemModule.Packets.ClientPackets;

namespace GameSrv.Npc {
    /// <summary>
    /// 交易NPC类
    /// 普通商人 如：药店和杂货店都在此实现
    /// </summary>
    public class Merchant : NormNpc {
        /// <summary>
        /// 脚本路径
        /// </summary>
        public string ScriptName = string.Empty;
        /// <summary>
        /// 物品价格倍率 默认为 100%
        /// </summary>
        public int PriceRate;
        /// <summary>
        /// 沙巴克城堡商人
        /// </summary>
        public bool CastleMerchant;
        /// <summary>
        /// 刷新在售商品时间
        /// </summary>
        internal int RefillGoodsTick;
        /// <summary>
        /// 清理武器升级过期时间
        /// </summary>
        internal int ClearExpreUpgradeTick;
        /// <summary>
        /// NPC买卖物品类型列表，脚本中前面的 +1 +30 之类的
        /// </summary>
        public IList<int> ItemTypeList;
        public IList<Goods> RefillGoodsList;
        /// <summary>
        /// 商品列表
        /// </summary>
        private readonly IList<IList<UserItem>> GoodsList;
        /// <summary>
        /// 物品价格列表
        /// </summary>
        private readonly IList<ItemPrice> ItemPriceList;
        /// <summary>
        /// 物品升级列表
        /// </summary>
        private readonly IList<WeaponUpgradeInfo> UpgradeWeaponList;
        public bool BoCanMove = false;
        public int MoveTime = 0;
        public int MoveTick;
        /// <summary>
        /// 是否购买物品
        /// </summary>
        public bool IsBuy;
        /// <summary>
        /// 是否交易物品
        /// </summary>
        public bool IsSell;
        public bool IsMakeDrug;
        public bool IsPrices;
        public bool IsStorage;
        public bool IsGetback;
        public bool IsUpgradenow;
        public bool IsGetBackupgnow;
        public bool IsRepair;
        public bool IsSupRepair;
        public bool IsSendMsg = false;
        public bool IsGetMarry;
        public bool IsGetMaster;
        public bool IsUseItemName;
        public bool IsOffLineMsg = false;
        public bool IsYBDeal = false;

        public Merchant() : base() {
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

        public override void Run() {
            try {
                int dwCurrentTick = HUtil32.GetTickCount();
                int dwDelayTick = ProcessRefillIndex * 500;
                if (dwCurrentTick < dwDelayTick) {
                    dwDelayTick = 0;
                }
                if (dwCurrentTick - RefillGoodsTick > 5 * 60 * 1000 + dwDelayTick) {
                    RefillGoodsTick = dwCurrentTick + dwDelayTick;
                    RefillGoods();
                }
                if ((dwCurrentTick - ClearExpreUpgradeTick) > 10 * 60 * 1000) {
                    ClearExpreUpgradeTick = dwCurrentTick;
                    ClearExpreUpgradeListData();
                }
                if (M2Share.RandomNumber.Random(50) == 0) {
                    TurnTo(M2Share.RandomNumber.RandomByte(8));
                }
                else {
                    if (M2Share.RandomNumber.Random(50) == 0) {
                        SendRefMsg(Messages.RM_HIT, Dir, CurrX, CurrY, 0, "");
                    }
                }
                if (CastleMerchant && Castle != null && Castle.UnderWar) {
                    if (!FixedHideMode) {
                        SendRefMsg(Messages.RM_DISAPPEAR, 0, 0, 0, 0, "");
                        FixedHideMode = true;
                    }
                }
                else {
                    if (FixedHideMode) {
                        FixedHideMode = false;
                        SendRefMsg(Messages.RM_HIT, Dir, CurrX, CurrY, 0, "");
                    }
                }
                if (BoCanMove && (HUtil32.GetTickCount() - MoveTick) > MoveTime * 1000) {
                    MoveTick = HUtil32.GetTickCount();
                    SendRefMsg(Messages.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
                    MapRandomMove(MapName, 0);
                }
            }
            catch (Exception e) {
                M2Share.Logger.Error(e.Message);
            }
            base.Run();
        }

        protected override bool Operate(ProcessMessage ProcessMsg) {
            return base.Operate(ProcessMsg);
        }

        private void AddItemPrice(ushort nIndex, double nPrice) {
            ItemPrice itemPrice = new ItemPrice {
                wIndex = nIndex,
                nPrice = nPrice
            };
            ItemPriceList.Add(itemPrice);
            DataSource.LocalDb.SaveGoodPriceRecord(this, ScriptName + '-' + MapName);
        }

        private void CheckItemPrice(ushort nIndex) {
            for (int i = 0; i < ItemPriceList.Count; i++) {
                ItemPrice itemPrice = ItemPriceList[i];
                if (itemPrice.wIndex == nIndex) {
                    double n10 = itemPrice.nPrice;
                    if (Math.Round(n10 * 1.1) > n10) {
                        n10 = HUtil32.Round(n10 * 1.1);
                    }
                    return;
                }
            }
            StdItem stdItem = M2Share.WorldEngine.GetStdItem(nIndex);
            if (stdItem != null) {
                AddItemPrice(nIndex, HUtil32.Round(stdItem.Price * 1.1));
            }
        }

        /// <summary>
        /// 刷新在售商品
        /// </summary>
        private void RefillGoods() {
            const string sExceptionMsg = "[Exception] TMerchant::RefillGoods {0}/{1}:{2} [{3}] Code:{4}";
            try {
                ushort nIndex;
                Goods Goods;
                for (int i = 0; i < RefillGoodsList.Count; i++) {
                    Goods = RefillGoodsList[i];
                    if ((HUtil32.GetTickCount() - Goods.RefillTick) > (Goods.RefillTime * 60 * 1000)) {
                        Goods.RefillTick = HUtil32.GetTickCount();
                        nIndex = M2Share.WorldEngine.GetStdItemIdx(Goods.ItemName);
                        if (nIndex > 0) {
                            IList<UserItem> refillList = GetRefillList(nIndex);
                            int nRefillCount = 0;
                            if (refillList != null) {
                                nRefillCount = refillList.Count;
                            }
                            if (Goods.Count > nRefillCount) {
                                CheckItemPrice(nIndex);
                                RefillGoodsItems(ref refillList, Goods.ItemName, Goods.Count - nRefillCount);
                                DataSource.LocalDb.SaveGoodRecord(this, ScriptName + '-' + MapName);
                                DataSource.LocalDb.SaveGoodPriceRecord(this, ScriptName + '-' + MapName);
                            }
                            if (Goods.Count < nRefillCount) {
                                RefillDelReFillItem(ref refillList, nRefillCount - Goods.Count);
                                DataSource.LocalDb.SaveGoodRecord(this, ScriptName + '-' + MapName);
                                DataSource.LocalDb.SaveGoodPriceRecord(this, ScriptName + '-' + MapName);
                            }
                        }
                    }
                }
                for (int i = 0; i < GoodsList.Count; i++) {
                    IList<UserItem> RefillList20 = GoodsList[i];
                    if (RefillList20.Count > 1000) {
                        bool bo21 = false;
                        for (int j = 0; j < RefillGoodsList.Count; j++) {
                            Goods = RefillGoodsList[j];
                            nIndex = M2Share.WorldEngine.GetStdItemIdx(Goods.ItemName);
                            if (RefillList20[0].Index == nIndex) {
                                bo21 = true;
                                break;
                            }
                        }
                        if (!bo21) {
                            RefillDelReFillItem(ref RefillList20, RefillList20.Count - 1000);
                        }
                        else {
                            RefillDelReFillItem(ref RefillList20, RefillList20.Count - 5000);
                        }
                    }
                }
            }
            catch (Exception e) {
                M2Share.Logger.Error(Format(sExceptionMsg, ChrName, CurrX, CurrY, e.Message, ScriptConst.nCHECK));
            }
        }

        private IList<UserItem> GetRefillList(int nIndex) {
            if (nIndex < 0) {
                return null;
            }
            for (int i = 0; i < GoodsList.Count; i++) {
                IList<UserItem> goods = GoodsList[i];
                if (goods.Count > 0) {
                    if (goods[0].Index == nIndex) {
                        return goods;
                    }
                }
            }
            return null;
        }

        private void RefillGoodsItems(ref IList<UserItem> List, string sItemName, int nInt) {
            if (List == null) {
                List = new List<UserItem>();
                GoodsList.Add(List);
            }
            for (int i = 0; i < nInt; i++) {
                UserItem goodItem = new UserItem();
                if (M2Share.WorldEngine.CopyToUserItemFromName(sItemName, ref goodItem)) {
                    List.Insert(0, goodItem);
                }
                else {
                    Dispose(goodItem);
                }
            }
        }

        private void RefillDelReFillItem(ref IList<UserItem> List, int nInt) {
            for (int i = List.Count - 1; i >= 0; i--) {
                if (nInt <= 0) {
                    break;
                }
                Dispose(List[i]);
                List.RemoveAt(i);
                nInt -= 1;
            }
        }

        private bool CheckItemType(int nStdMode) {
            bool result = false;
            for (int i = 0; i < ItemTypeList.Count; i++) {
                if (ItemTypeList[i] == nStdMode) {
                    result = true;
                    break;
                }
            }
            return result;
        }

        private double GetItemPrice(ushort nIndex) {
            double result = -1;
            for (int i = 0; i < ItemPriceList.Count; i++) {
                ItemPrice ItemPrice = ItemPriceList[i];
                if (ItemPrice.wIndex == nIndex) {
                    result = ItemPrice.nPrice;
                    break;
                }
            }
            if (result < 0) {
                StdItem StdItem = M2Share.WorldEngine.GetStdItem(nIndex);
                if (StdItem != null) {
                    if (CheckItemType(StdItem.StdMode)) {
                        result = StdItem.Price;
                    }
                }
            }
            return result;
        }

        private void UpgradeWaponAddValue(PlayObject User, IList<UserItem> ItemList, ref byte btDc, ref byte btSc, ref byte btMc, ref byte btDura) {
            ClientItem StdItem80 = null;
            IList<DeleteItem> DelItemList = null;
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
            IList<double> DuraList = new List<double>();
            for (int i = ItemList.Count - 1; i >= 0; i--) {
                UserItem UserItem = ItemList[i];
                if (M2Share.WorldEngine.GetStdItemName(UserItem.Index) == M2Share.Config.BlackStone) {
                    DuraList.Add(Math.Round(UserItem.Dura / 1.0e3));
                    if (DelItemList == null) {
                        DelItemList = new List<DeleteItem>();
                    }
                    DelItemList.Add(new DeleteItem() {
                        MakeIndex = UserItem.MakeIndex,
                        ItemName = M2Share.Config.BlackStone
                    });
                    DisPose(UserItem);
                    ItemList.RemoveAt(i);
                }
                else {
                    if (M2Share.IsAccessory(UserItem.Index)) {
                        StdItem StdItem = M2Share.WorldEngine.GetStdItem(UserItem.Index);
                        if (StdItem != null) {
                            StdItem.GetUpgradeStdItem(UserItem, ref StdItem80);
                            //StdItem.GetItemAddValue(UserItem, ref StdItem80);
                            nDc = 0;
                            nSc = 0;
                            nMc = 0;
                            switch (StdItem80.Item.StdMode) {
                                case 19:
                                case 20:
                                case 21:
                                    nDc = HUtil32.HiWord(StdItem80.Item.DC) + HUtil32.LoWord(StdItem80.Item.DC);
                                    nSc = HUtil32.HiWord(StdItem80.Item.SC) + HUtil32.LoWord(StdItem80.Item.SC);
                                    nMc = HUtil32.HiWord(StdItem80.Item.MC) + HUtil32.LoWord(StdItem80.Item.MC);
                                    break;
                                case 22:
                                case 23:
                                    nDc = HUtil32.HiWord(StdItem80.Item.DC) + HUtil32.LoWord(StdItem80.Item.DC);
                                    nSc = HUtil32.HiWord(StdItem80.Item.SC) + HUtil32.LoWord(StdItem80.Item.SC);
                                    nMc = HUtil32.HiWord(StdItem80.Item.MC) + HUtil32.LoWord(StdItem80.Item.MC);
                                    break;
                                case 24:
                                case 26:
                                    nDc = HUtil32.HiWord(StdItem80.Item.DC) + HUtil32.LoWord(StdItem80.Item.DC) + 1;
                                    nSc = HUtil32.HiWord(StdItem80.Item.SC) + HUtil32.LoWord(StdItem80.Item.SC) + 1;
                                    nMc = HUtil32.HiWord(StdItem80.Item.MC) + HUtil32.LoWord(StdItem80.Item.MC) + 1;
                                    break;
                            }
                            if (nDcMin < nDc) {
                                nDcMax = nDcMin;
                                nDcMin = nDc;
                            }
                            else {
                                if (nDcMax < nDc) {
                                    nDcMax = nDc;
                                }
                            }
                            if (nScMin < nSc) {
                                nScMax = nScMin;
                                nScMin = nSc;
                            }
                            else {
                                if (nScMax < nSc) {
                                    nScMax = nSc;
                                }
                            }
                            if (nMcMin < nMc) {
                                nMcMax = nMcMin;
                                nMcMin = nMc;
                            }
                            else {
                                if (nMcMax < nMc) {
                                    nMcMax = nMc;
                                }
                            }
                            if (DelItemList == null) {
                                DelItemList = new List<DeleteItem>();
                            }
                            DelItemList.Add(new DeleteItem() {
                                ItemName = StdItem.Name,
                                MakeIndex = UserItem.MakeIndex
                            });
                            if (StdItem.NeedIdentify == 1) {
                                M2Share.EventSource.AddEventLog(26, User.MapName + "\t" + User.CurrX + "\t" + User.CurrY + "\t" + User.ChrName + "\t" + StdItem.Name + "\t" + UserItem.MakeIndex + "\t" + '1' + "\t" + '0');
                            }
                            DisPose(UserItem);
                            ItemList.RemoveAt(i);
                        }
                    }
                }
            }
            for (int i = 0; i < DuraList.Count; i++) {
                for (int j = DuraList.Count - 1; j > i; j--) {
                    if (DuraList[j] > DuraList[j - 1]) {
                        DuraList.Reverse();
                    }
                }
            }
            for (int i = 0; i < DuraList.Count; i++) {
                nDura = nDura + (int)DuraList[i];
                nItemCount++;
                if (nItemCount >= 5) {
                    break;
                }
            }
            btDura = (byte)HUtil32.Round(HUtil32._MIN(5, nItemCount) + HUtil32._MIN(5, nItemCount) * (nDura / nItemCount / 5.0));
            btDc = (byte)(nDcMin / 5 + nDcMax / 3);
            btSc = (byte)(nScMin / 5 + nScMax / 3);
            btMc = (byte)(nMcMin / 5 + nMcMax / 3);
            if (DelItemList != null) {
                int objectId = HUtil32.Sequence();
                M2Share.ActorMgr.AddOhter(objectId, DelItemList);
                User.SendMsg(this, Messages.RM_SENDDELITEMLIST, 0, objectId, 0, 0, "");
            }
            if (DuraList != null) {
            }
        }

        private void UpgradeWapon(PlayObject User) {
            bool upgradeSuccess = false;
            WeaponUpgradeInfo upgradeInfo;
            for (int i = 0; i < UpgradeWeaponList.Count; i++) {
                upgradeInfo = UpgradeWeaponList[i];
                if (upgradeInfo.UserName == User.ChrName) {
                    GotoLable(User, ScriptConst.sUPGRADEING, false);
                    return;
                }
            }
            if (User.UseItems[ItemLocation.Weapon] != null && User.UseItems[ItemLocation.Weapon].Index != 0 && User.Gold >= M2Share.Config.UpgradeWeaponPrice
                && User.CheckItems(M2Share.Config.BlackStone) != null) {
                User.DecGold(M2Share.Config.UpgradeWeaponPrice);
                if (CastleMerchant || M2Share.Config.GetAllNpcTax) {
                    if (Castle != null) {
                        Castle.IncRateGold(M2Share.Config.UpgradeWeaponPrice);
                    }
                    else if (M2Share.Config.GetAllNpcTax) {
                        M2Share.CastleMgr.IncRateGold(M2Share.Config.UpgradeWeaponPrice);
                    }
                }
                User.GoldChanged();
                upgradeInfo = new WeaponUpgradeInfo {
                    UserName = User.ChrName,
                    UserItem = new UserItem(User.UseItems[ItemLocation.Weapon])
                };
                StdItem StdItem = M2Share.WorldEngine.GetStdItem(User.UseItems[ItemLocation.Weapon].Index);
                if (StdItem.NeedIdentify == 1) {
                    M2Share.EventSource.AddEventLog(25, User.MapName + "\t" + User.CurrX + "\t" + User.CurrY + "\t" + User.ChrName + "\t" + StdItem.Name + "\t" + User.UseItems[ItemLocation.Weapon].MakeIndex + "\t" + '1' + "\t" + '0');
                }
                User.SendDelItems(User.UseItems[ItemLocation.Weapon]);
                User.UseItems[ItemLocation.Weapon].Index = 0;
                User.RecalcAbilitys();
                User.FeatureChanged();
                User.SendMsg(User, Messages.RM_ABILITY, 0, 0, 0, 0, "");
                UpgradeWaponAddValue(User, User.ItemList, ref upgradeInfo.Dc, ref upgradeInfo.Sc, ref upgradeInfo.Mc, ref upgradeInfo.Dura);
                upgradeInfo.UpgradeTime = DateTime.Now;
                upgradeInfo.GetBackTick = HUtil32.GetTickCount();
                UpgradeWeaponList.Add(upgradeInfo);
                SaveUpgradingList();
                upgradeSuccess = true;
            }
            if (upgradeSuccess) {
                GotoLable(User, ScriptConst.sUPGRADEOK, false);
            }
            else {
                GotoLable(User, ScriptConst.sUPGRADEFAIL, false);
            }
        }

        /// <summary>
        /// 取回升级武器
        /// </summary>
        /// <param name="User"></param>
        private void GetBackupgWeapon(PlayObject User) {
            WeaponUpgradeInfo UpgradeInfo = default;
            int nFlag = 0;
            if (!User.IsEnoughBag()) {
                GotoLable(User, ScriptConst.sGETBACKUPGFULL, false);
                return;
            }
            for (int i = 0; i < UpgradeWeaponList.Count; i++) {
                if (string.Compare(UpgradeWeaponList[i].UserName, User.ChrName, StringComparison.OrdinalIgnoreCase) == 0) {
                    nFlag = 1;
                    if (((HUtil32.GetTickCount() - UpgradeWeaponList[i].GetBackTick) > M2Share.Config.UPgradeWeaponGetBackTime) || User.Permission >= 4) {
                        UpgradeInfo = UpgradeWeaponList[i];
                        UpgradeWeaponList.RemoveAt(i);
                        SaveUpgradingList();
                        nFlag = 2;
                        break;
                    }
                }
            }
            if (!string.IsNullOrEmpty(UpgradeInfo.UserName)) {
                if (HUtil32.RangeInDefined(UpgradeInfo.Dura, 0, 8)) {
                    if (UpgradeInfo.UserItem.DuraMax > 3000) {
                        UpgradeInfo.UserItem.DuraMax -= 3000;
                    }
                    else {
                        UpgradeInfo.UserItem.DuraMax = (ushort)(UpgradeInfo.UserItem.DuraMax >> 1);
                    }
                    if (UpgradeInfo.UserItem.Dura > UpgradeInfo.UserItem.DuraMax) {
                        UpgradeInfo.UserItem.Dura = UpgradeInfo.UserItem.DuraMax;
                    }
                }
                else if (HUtil32.RangeInDefined(UpgradeInfo.Dura, 9, 15)) {
                    if (M2Share.RandomNumber.Random(UpgradeInfo.Dura) < 6) {
                        if (UpgradeInfo.UserItem.DuraMax > 1000) {
                            UpgradeInfo.UserItem.DuraMax -= 1000;
                        }
                        if (UpgradeInfo.UserItem.Dura > UpgradeInfo.UserItem.DuraMax) {
                            UpgradeInfo.UserItem.Dura = UpgradeInfo.UserItem.DuraMax;
                        }
                    }
                }
                else if (HUtil32.RangeInDefined(UpgradeInfo.Dura, 18, 255)) {
                    int r = M2Share.RandomNumber.Random(UpgradeInfo.Dura - 18);
                    if (HUtil32.RangeInDefined(r, 1, 4)) {
                        UpgradeInfo.UserItem.DuraMax += 1000;
                    }
                    else if (HUtil32.RangeInDefined(r, 5, 7)) {
                        UpgradeInfo.UserItem.DuraMax += 2000;
                    }
                    else if (HUtil32.RangeInDefined(r, 8, 255)) {
                        UpgradeInfo.UserItem.DuraMax += 4000;
                    }
                }
                int n1C;
                if (UpgradeInfo.Dc == UpgradeInfo.Mc && UpgradeInfo.Mc == UpgradeInfo.Sc) {
                    n1C = M2Share.RandomNumber.Random(3);
                }
                else {
                    n1C = -1;
                }
                int n90;
                int n10;
                if (UpgradeInfo.Dc >= UpgradeInfo.Mc && UpgradeInfo.Dc >= UpgradeInfo.Sc || (n1C == 0)) {
                    n90 = HUtil32._MIN(11, UpgradeInfo.Dc);
                    n10 = HUtil32._MIN(85, (n90 << 3 - n90) + 10 + UpgradeInfo.UserItem.Desc[3] - UpgradeInfo.UserItem.Desc[4] + User.BodyLuckLevel);
                    if (M2Share.RandomNumber.Random(M2Share.Config.UpgradeWeaponDCRate) < n10) {
                        UpgradeInfo.UserItem.Desc[ItemAttr.WeaponUpgrade] = 10;
                        if (n10 > 63 && M2Share.RandomNumber.Random(M2Share.Config.UpgradeWeaponDCTwoPointRate) == 0) {
                            UpgradeInfo.UserItem.Desc[ItemAttr.WeaponUpgrade] = 11;
                        }
                        if (n10 > 79 && M2Share.RandomNumber.Random(M2Share.Config.UpgradeWeaponDCThreePointRate) == 0) {
                            UpgradeInfo.UserItem.Desc[ItemAttr.WeaponUpgrade] = 12;
                        }
                    }
                    else {
                        UpgradeInfo.UserItem.Desc[ItemAttr.WeaponUpgrade] = 1;
                    }
                }
                if (UpgradeInfo.Mc >= UpgradeInfo.Dc && UpgradeInfo.Mc >= UpgradeInfo.Sc || n1C == 1) {
                    n90 = HUtil32._MIN(11, UpgradeInfo.Mc);
                    n10 = HUtil32._MIN(85, (n90 << 3 - n90) + 10 + UpgradeInfo.UserItem.Desc[3] - UpgradeInfo.UserItem.Desc[4] + User.BodyLuckLevel);
                    if (M2Share.RandomNumber.Random(M2Share.Config.UpgradeWeaponMCRate) < n10) {
                        UpgradeInfo.UserItem.Desc[ItemAttr.WeaponUpgrade] = 20;
                        if (n10 > 63 && M2Share.RandomNumber.Random(M2Share.Config.UpgradeWeaponMCTwoPointRate) == 0) {
                            UpgradeInfo.UserItem.Desc[ItemAttr.WeaponUpgrade] = 21;
                        }
                        if (n10 > 79 && M2Share.RandomNumber.Random(M2Share.Config.UpgradeWeaponMCThreePointRate) == 0) {
                            UpgradeInfo.UserItem.Desc[ItemAttr.WeaponUpgrade] = 22;
                        }
                    }
                    else {
                        UpgradeInfo.UserItem.Desc[ItemAttr.WeaponUpgrade] = 1;
                    }
                }
                if (UpgradeInfo.Sc >= UpgradeInfo.Mc && UpgradeInfo.Sc >= UpgradeInfo.Dc || n1C == 2) {
                    n90 = HUtil32._MIN(11, UpgradeInfo.Mc);
                    n10 = HUtil32._MIN(85, (n90 << 3 - n90) + 10 + UpgradeInfo.UserItem.Desc[3] - UpgradeInfo.UserItem.Desc[4] + User.BodyLuckLevel);
                    if (M2Share.RandomNumber.Random(M2Share.Config.UpgradeWeaponSCRate) < n10) {
                        UpgradeInfo.UserItem.Desc[ItemAttr.WeaponUpgrade] = 30;
                        if (n10 > 63 && M2Share.RandomNumber.Random(M2Share.Config.UpgradeWeaponSCTwoPointRate) == 0) {
                            UpgradeInfo.UserItem.Desc[ItemAttr.WeaponUpgrade] = 31;
                        }
                        if (n10 > 79 && M2Share.RandomNumber.Random(M2Share.Config.UpgradeWeaponSCThreePointRate) == 0) {
                            UpgradeInfo.UserItem.Desc[ItemAttr.WeaponUpgrade] = 32;
                        }
                    }
                    else {
                        UpgradeInfo.UserItem.Desc[ItemAttr.WeaponUpgrade] = 1;
                    }
                }
                UserItem UserItem = UpgradeInfo.UserItem;
                StdItem StdItem = M2Share.WorldEngine.GetStdItem(UserItem.Index);
                if (StdItem.NeedIdentify == 1) {
                    M2Share.EventSource.AddEventLog(24, User.MapName + "\t" + User.CurrX + "\t" + User.CurrY + "\t" + User.ChrName + "\t" + StdItem.Name + "\t" + UserItem.MakeIndex + "\t" + '1' + "\t" + '0');
                }
                User.AddItemToBag(UserItem);
                User.SendAddItem(UserItem);
                DisPose(UpgradeInfo);
            }
            switch (nFlag) {
                case 0:
                    GotoLable(User, ScriptConst.sGETBACKUPGFAIL, false);
                    break;
                case 1:
                    GotoLable(User, ScriptConst.sGETBACKUPGING, false);
                    break;
                case 2:
                    GotoLable(User, ScriptConst.sGETBACKUPGOK, false);
                    break;
            }
        }

        /// <summary>
        /// 获取物品售卖价格
        /// </summary>
        /// <returns></returns>
        private int GetUserPrice(PlayObject PlayObject, double nPrice) {
            int result;
            if (CastleMerchant) {
                if (Castle != null && Castle.IsMasterGuild(PlayObject.MyGuild)) //沙巴克成员修复物品打折
                {
                    int n14 = HUtil32._MAX(60, HUtil32.Round(PriceRate * (M2Share.Config.CastleMemberPriceRate / 100)));//80%
                    result = HUtil32.Round(nPrice / 100 * n14);
                }
                else {
                    result = HUtil32.Round(nPrice / 100 * PriceRate);
                }
            }
            else {
                result = HUtil32.Round(nPrice / 100 * PriceRate);
            }
            return result;
        }

        public override void UserSelect(PlayObject PlayObject, string sData) {
            string sLabel = string.Empty;
            const string sExceptionMsg = "[Exception] TMerchant::UserSelect... Data: {0}";
            base.UserSelect(PlayObject, sData);
            try {
                if (!CastleMerchant || !(Castle != null && Castle.UnderWar)) {
                    if (!PlayObject.Death && !string.IsNullOrEmpty(sData) && sData[0] == '@') {
                        string sMsg = HUtil32.GetValidStr3(sData, ref sLabel, '\r');
                        PlayObject.ScriptLable = sData;
                        bool boCanJmp = PlayObject.LableIsCanJmp(sLabel);
                        if (string.Compare(sLabel, ScriptConst.sSL_SENDMSG, StringComparison.OrdinalIgnoreCase) == 0) {
                            if (string.IsNullOrEmpty(sMsg)) {
                                return;
                            }
                        }
                        GotoLable(PlayObject, sLabel, !boCanJmp);
                        if (!boCanJmp) {
                            return;
                        }
                        if (string.Compare(sLabel, ScriptConst.sOFFLINEMSG, StringComparison.OrdinalIgnoreCase) == 0)// 增加挂机
                        {
                            if (IsOffLineMsg) {
                                SetOffLineMsg(PlayObject, sMsg);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptConst.sSL_SENDMSG, StringComparison.OrdinalIgnoreCase) == 0) {
                            if (IsSendMsg) {
                                SendCustemMsg(PlayObject, sMsg);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptConst.sSUPERREPAIR, StringComparison.OrdinalIgnoreCase) == 0) {
                            if (IsSupRepair) {
                                UserSelectSuperRepairItem(PlayObject);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptConst.sBUY, StringComparison.OrdinalIgnoreCase) == 0) {
                            if (IsBuy) {
                                UserSelectBuyItem(PlayObject, 0);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptConst.sSELL, StringComparison.OrdinalIgnoreCase) == 0) {
                            if (IsSell) {
                                UserSelectSellItem(PlayObject);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptConst.sREPAIR, StringComparison.OrdinalIgnoreCase) == 0) {
                            if (IsRepair) {
                                UserSelectRepairItem(PlayObject);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptConst.sMAKEDURG, StringComparison.OrdinalIgnoreCase) == 0) {
                            if (IsMakeDrug) {
                                UserSelectMakeDurg(PlayObject);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptConst.sPRICES, StringComparison.OrdinalIgnoreCase) == 0) {
                            if (IsPrices) {
                                UserSelectItemPrices(PlayObject);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptConst.sSTORAGE, StringComparison.OrdinalIgnoreCase) == 0) {
                            if (IsStorage) {
                                UserSelectStorage(PlayObject);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptConst.sGETBACK, StringComparison.OrdinalIgnoreCase) == 0) {
                            if (IsGetback) {
                                UserSelectGetBack(PlayObject);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptConst.sUPGRADENOW, StringComparison.OrdinalIgnoreCase) == 0) {
                            if (IsUpgradenow) {
                                UpgradeWapon(PlayObject);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptConst.sGETBACKUPGNOW, StringComparison.OrdinalIgnoreCase) == 0) {
                            if (IsGetBackupgnow) {
                                GetBackupgWeapon(PlayObject);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptConst.sGETMARRY, StringComparison.OrdinalIgnoreCase) == 0) {
                            if (IsGetMarry) {
                                GetBackupgWeapon(PlayObject);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptConst.sGETMASTER, StringComparison.OrdinalIgnoreCase) == 0) {
                            if (IsGetMaster) {
                                GetBackupgWeapon(PlayObject);
                            }
                        }
                        else if (HUtil32.CompareLStr(sLabel, ScriptConst.sUSEITEMNAME)) {
                            if (IsUseItemName) {
                                ChangeUseItemName(PlayObject, sLabel, sMsg);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptConst.sEXIT, StringComparison.OrdinalIgnoreCase) == 0) {
                            PlayObject.SendMsg(this, Messages.RM_MERCHANTDLGCLOSE, 0, ActorId, 0, 0, "");
                        }
                        else if (string.Compare(sLabel, ScriptConst.sBACK, StringComparison.OrdinalIgnoreCase) == 0) {
                            if (string.IsNullOrEmpty(PlayObject.ScriptGoBackLable)) {
                                PlayObject.ScriptGoBackLable = ScriptConst.sMAIN;
                            }
                            GotoLable(PlayObject, PlayObject.ScriptGoBackLable, false);
                        }
                        else if (string.Compare(sLabel, ScriptConst.sDealYBme, StringComparison.OrdinalIgnoreCase) == 0) // 元宝寄售:出售物品 
                        {
                            if (IsYBDeal) {
                                UserSelectOpenDealOffForm(PlayObject); // 打开出售物品窗口
                            }
                        }
                    }
                }
            }
            catch (Exception ex) {
                M2Share.Logger.Error(Format(sExceptionMsg, sData));
                M2Share.Logger.Error(ex.StackTrace);
            }
        }

        /// <summary>
        /// 特殊修理物品
        /// </summary>
        private void UserSelectSuperRepairItem(PlayObject User) {
            User.SendMsg(this, Messages.RM_SENDUSERSREPAIR, 0, ActorId, 0, 0, "");
        }

        /// <summary>
        /// 物品详情列表
        /// </summary>
        private void UserSelectBuyItem(PlayObject User, int nInt) {
            string sSendMsg = string.Empty;
            int n10 = 0;
            for (int i = 0; i < GoodsList.Count; i++) {
                IList<UserItem> goods = GoodsList[i];
                UserItem userItem = goods[0];
                StdItem stdItem = M2Share.WorldEngine.GetStdItem(userItem.Index);
                if (stdItem != null) {
                    string sName = CustomItem.GetItemName(userItem);
                    int nPrice = GetUserPrice(User, GetItemPrice(userItem.Index));
                    int nStock = goods.Count;
                    short nSubMenu;
                    if (stdItem.StdMode <= 4 || stdItem.StdMode == 42 || stdItem.StdMode == 31) {
                        nSubMenu = 0;
                    }
                    else {
                        nSubMenu = 1;
                    }
                    sSendMsg = sSendMsg + sName + '/' + nSubMenu + '/' + nPrice + '/' + nStock + '/';
                    n10++;
                }
            }
            User.SendMsg(this, Messages.RM_SENDGOODSLIST, 0, ActorId, n10, 0, sSendMsg);
        }

        private void UserSelectSellItem(PlayObject User) {
            User.SendMsg(this, Messages.RM_SENDUSERSELL, 0, ActorId, 0, 0, "");
        }

        private void UserSelectRepairItem(PlayObject User) {
            User.SendMsg(this, Messages.RM_SENDUSERREPAIR, 0, ActorId, 0, 0, "");
        }

        private void UserSelectMakeDurg(PlayObject User) {
            string sSendMsg = string.Empty;
            for (int i = 0; i < GoodsList.Count; i++) {
                IList<UserItem> goods = GoodsList[i];
                StdItem StdItem = M2Share.WorldEngine.GetStdItem(goods[0].Index);
                if (StdItem != null) {
                    sSendMsg = sSendMsg + StdItem.Name + '/' + 0 + '/' + M2Share.Config.MakeDurgPrice + '/' + 1 + '/';
                }
            }
            if (!string.IsNullOrEmpty(sSendMsg)) {
                User.SendMsg(this, Messages.RM_USERMAKEDRUGITEMLIST, 0, ActorId, 0, 0, sSendMsg);
            }
        }

        private static void UserSelectItemPrices(PlayObject User) {

        }

        private void UserSelectStorage(PlayObject User) {
            User.SendMsg(this, Messages.RM_USERSTORAGEITEM, 0, ActorId, 0, 0, "");
        }

        private void UserSelectGetBack(PlayObject User) {
            User.SendMsg(this, Messages.RM_USERGETBACKITEM, 0, ActorId, 0, 0, "");
        }

        /// <summary>
        /// 打开出售物品窗口
        /// </summary>
        /// <param name="User"></param>
        private void UserSelectOpenDealOffForm(PlayObject User) {
            if (User.BoYbDeal) {
                if (!User.SellOffInTime(0)) {
                    User.SendMsg(this, Messages.RM_SENDDEALOFFFORM, 0, ActorId, 0, 0, "");
                    User.GetBackSellOffItems();
                }
                else {
                    User.SendMsg(this, Messages.RM_MERCHANTSAY, 0, 0, 0, 0, ChrName + "/您还有元宝服务正在进行!!\\ \\<返回/@main>");
                }
            }
            else {
                User.SendMsg(this, Messages.RM_MERCHANTSAY, 0, 0, 0, 0, ChrName + "/您未开通元宝服务,请先开通元宝服务!!\\ \\<返回/@main>");
            }
        }

        public void LoadNPCData() {
            string sFile = ScriptName + '-' + MapName;
            DataSource.LocalDb.LoadGoodRecord(this, sFile);
            DataSource.LocalDb.LoadGoodPriceRecord(this, sFile);
            LoadUpgradeList();
        }

        private void SaveNPCData() {
            string sFile = ScriptName + '-' + MapName;
            DataSource.LocalDb.SaveGoodRecord(this, sFile);
            DataSource.LocalDb.SaveGoodPriceRecord(this, sFile);
        }

        /// <summary>
        /// 清理武器升级过期数据
        /// </summary>
        private void ClearExpreUpgradeListData() {
            for (int i = UpgradeWeaponList.Count - 1; i >= 0; i--) {
                WeaponUpgradeInfo upgradeInfo = UpgradeWeaponList[i];
                if ((DateTime.Now - upgradeInfo.UpgradeTime).TotalDays >= M2Share.Config.ClearExpireUpgradeWeaponDays) {
                    Dispose(upgradeInfo);
                    UpgradeWeaponList.RemoveAt(i);
                }
            }
        }

        public void LoadMerchantScript() {
            ItemTypeList.Clear();
            m_sPath = ScriptConst.sMarket_Def;
            string scriptPath = ScriptName + '-' + MapName;
            M2Share.ScriptSystem.LoadScriptFile(this, ScriptConst.sMarket_Def, scriptPath, true);
        }

        public override void Click(PlayObject PlayObject) {
            base.Click(PlayObject);
        }

        protected override void GetVariableText(PlayObject PlayObject, string sVariable, ref string sMsg) {
            string sText;
            base.GetVariableText(PlayObject, sVariable, ref sMsg);
            switch (sVariable) {
                case "$PRICERATE":
                    sText = PriceRate.ToString();
                    sMsg = ReplaceVariableText(sMsg, "<$PRICERATE>", sText);
                    break;
                case "$UPGRADEWEAPONFEE":
                    sText = M2Share.Config.UpgradeWeaponPrice.ToString();
                    sMsg = ReplaceVariableText(sMsg, "<$UPGRADEWEAPONFEE>", sText);
                    break;
                case "$USERWEAPON": {
                        if (PlayObject.UseItems[ItemLocation.Weapon].Index != 0) {
                            sText = M2Share.WorldEngine.GetStdItemName(PlayObject.UseItems[ItemLocation.Weapon].Index);
                        }
                        else {
                            sText = "无";
                        }
                        sMsg = ReplaceVariableText(sMsg, "<$USERWEAPON>", sText);
                        break;
                    }
            }
        }

        private double GetUserItemPrice(UserItem UserItem) {
            double result;
            double n20;
            int nC;
            int n14;
            double itemPrice = GetItemPrice(UserItem.Index);
            if (itemPrice > 0) {
                StdItem StdItem = M2Share.WorldEngine.GetStdItem(UserItem.Index);
                if (StdItem != null && StdItem.StdMode > 4 && StdItem.DuraMax > 0 && UserItem.DuraMax > 0) {
                    if (StdItem.StdMode == 40)// 肉
                    {
                        if (UserItem.Dura <= UserItem.DuraMax) {
                            n20 = itemPrice / 2.0 / UserItem.DuraMax * (UserItem.DuraMax - UserItem.Dura);
                            itemPrice = HUtil32._MAX(2, HUtil32.Round(itemPrice - n20));
                        }
                        else {
                            itemPrice = itemPrice + HUtil32.Round(itemPrice / UserItem.DuraMax * 2.0 * (UserItem.DuraMax - UserItem.Dura));
                        }
                    }
                    if (StdItem.StdMode == 43) {
                        if (UserItem.DuraMax < 10000) {
                            UserItem.DuraMax = 10000;
                        }
                        if (UserItem.Dura <= UserItem.DuraMax) {
                            n20 = itemPrice / 2.0 / UserItem.DuraMax * (UserItem.DuraMax - UserItem.Dura);
                            itemPrice = HUtil32._MAX(2, HUtil32.Round(itemPrice - n20));
                        }
                        else {
                            itemPrice = itemPrice + HUtil32.Round(itemPrice / UserItem.DuraMax * 1.3 * (UserItem.DuraMax - UserItem.Dura));
                        }
                    }
                    if (StdItem.StdMode > 4) {
                        n14 = 0;
                        nC = 0;
                        while (true) {
                            if (StdItem.StdMode == 5 || StdItem.StdMode == 6) {
                                if (nC != 4 || nC != 9) {
                                    if (nC == 6) {
                                        if (UserItem.Desc[nC] > 10) {
                                            n14 = n14 + (UserItem.Desc[nC] - 10) * 2;
                                        }
                                    }
                                    else {
                                        n14 = n14 + UserItem.Desc[nC];
                                    }
                                }
                            }
                            else {
                                n14 += UserItem.Desc[nC];
                            }
                            nC++;
                            if (nC >= 8) {
                                break;
                            }
                        }
                        if (n14 > 0) {
                            itemPrice = itemPrice / 5 * n14;
                        }
                        itemPrice = HUtil32.Round(itemPrice / StdItem.DuraMax * UserItem.DuraMax);
                        n20 = itemPrice / 2.0 / UserItem.DuraMax * (UserItem.DuraMax - UserItem.Dura);
                        itemPrice = HUtil32._MAX(2, HUtil32.Round(itemPrice - n20));
                    }
                }
            }
            result = itemPrice;
            return result;
        }

        public void ClientBuyItem(PlayObject PlayObject, string sItemName, int nInt) {
            bool bo29 = false;
            int n1C = 1;
            for (int i = 0; i < GoodsList.Count; i++) {
                if (bo29) {
                    break;
                }
                IList<UserItem> List20 = GoodsList[i];
                UserItem UserItem = List20[0];
                StdItem StdItem = M2Share.WorldEngine.GetStdItem(UserItem.Index);
                if (StdItem != null) {
                    string sUserItemName = CustomItem.GetItemName(UserItem);
                    if (PlayObject.IsAddWeightAvailable(StdItem.Weight)) {
                        if (sUserItemName == sItemName) {
                            for (int j = 0; j < List20.Count; j++) {
                                UserItem = List20[j];
                                if (StdItem.StdMode <= 4 || StdItem.StdMode == 42 || StdItem.StdMode == 31 || UserItem.MakeIndex == nInt) {
                                    int nPrice = GetUserPrice(PlayObject, GetUserItemPrice(UserItem));
                                    if (PlayObject.Gold >= nPrice && nPrice > 0) {
                                        if (PlayObject.AddItemToBag(UserItem)) {
                                            PlayObject.Gold -= nPrice;
                                            if (CastleMerchant || M2Share.Config.GetAllNpcTax) {
                                                if (Castle != null) {
                                                    Castle.IncRateGold(nPrice);
                                                }
                                                else if (M2Share.Config.GetAllNpcTax) {
                                                    M2Share.CastleMgr.IncRateGold(M2Share.Config.UpgradeWeaponPrice);
                                                }
                                            }
                                            PlayObject.SendAddItem(UserItem);
                                            if (StdItem.NeedIdentify == 1) {
                                                M2Share.EventSource.AddEventLog(9, PlayObject.MapName + "\t" + PlayObject.CurrX + "\t" + PlayObject.CurrY + "\t" + PlayObject.ChrName + "\t" + StdItem.Name + "\t" + UserItem.MakeIndex + "\t" + '1' + "\t" + ChrName);
                                            }
                                            List20.RemoveAt(j);
                                            if (List20.Count <= 0) {
                                                GoodsList.RemoveAt(i);
                                            }
                                            n1C = 0;
                                        }
                                        else {
                                            n1C = 2;
                                        }
                                    }
                                    else {
                                        n1C = 3;
                                    }
                                    bo29 = true;
                                    break;
                                }
                            }
                        }
                    }
                    else {
                        n1C = 2;
                    }
                }
            }
            if (n1C == 0) {
                PlayObject.SendMsg(this, Messages.SM_BUYITEM_SUCCESS, 0, PlayObject.Gold, nInt, 0, "");
            }
            else {
                PlayObject.SendMsg(this, Messages.SM_BUYITEM_FAIL, 0, n1C, 0, 0, "");
            }
        }

        public void ClientGetDetailGoodsList(PlayObject PlayObject, string sItemName, int nInt) {
            string sSendMsg = string.Empty;
            int nItemCount = 0;
            for (int i = 0; i < GoodsList.Count; i++) {
                IList<UserItem> List20 = GoodsList[i];
                if (List20.Count <= 0) {
                    continue;
                }
                UserItem UserItem = List20[0];
                StdItem Item = M2Share.WorldEngine.GetStdItem(UserItem.Index);
                if (Item != null && Item.Name == sItemName) {
                    if (List20.Count - 1 < nInt) {
                        nInt = HUtil32._MAX(0, List20.Count - 10);
                    }
                    for (int j = List20.Count - 1; j >= 0; j--) {
                        UserItem = List20[j];
                        ClientItem clientItem = new ClientItem();
                        Item.GetUpgradeStdItem(UserItem, ref clientItem);
                        //Item.GetItemAddValue(UserItem, ref ClientItem.Item);
                        clientItem.Dura = UserItem.Dura;
                        clientItem.DuraMax = (ushort)GetUserPrice(PlayObject, GetUserItemPrice(UserItem));
                        clientItem.MakeIndex = UserItem.MakeIndex;
                        sSendMsg = sSendMsg + EDCode.EncodeBuffer(clientItem) + "/";
                        nItemCount++;
                        if (nItemCount >= 10) {
                            break;
                        }
                    }
                    break;
                }
            }
            PlayObject.SendMsg(this, Messages.RM_SENDDETAILGOODSLIST, 0, ActorId, nItemCount, nInt, sSendMsg);
        }

        public void ClientQuerySellPrice(PlayObject PlayObject, UserItem UserItem) {
            int nC = GetSellItemPrice(GetUserItemPrice(UserItem));
            if (nC >= 0) {
                PlayObject.SendMsg(this, Messages.RM_SENDBUYPRICE, 0, nC, 0, 0, "");
            }
            else {
                PlayObject.SendMsg(this, Messages.RM_SENDBUYPRICE, 0, 0, 0, 0, "");
            }
        }

        private static int GetSellItemPrice(double nPrice) {
            return HUtil32.Round(nPrice / 2.0);
        }

        private static bool ClientSellItem_sub_4A1C84(UserItem UserItem) {
            bool result = true;
            StdItem StdItem = M2Share.WorldEngine.GetStdItem(UserItem.Index);
            if (StdItem != null && (StdItem.StdMode == 25 || StdItem.StdMode == 30)) {
                if (UserItem.Dura < 4000) {
                    result = false;
                }
            }
            return result;
        }

        public bool ClientSellItem(PlayObject PlayObject, UserItem UserItem) {
            bool result = false;
            int nPrice = GetSellItemPrice(GetUserItemPrice(UserItem));
            if (nPrice > 0 && ClientSellItem_sub_4A1C84(UserItem)) {
                if (PlayObject.IncGold(nPrice)) {
                    if (CastleMerchant || M2Share.Config.GetAllNpcTax) {
                        if (Castle != null) {
                            Castle.IncRateGold(nPrice);
                        }
                        else if (M2Share.Config.GetAllNpcTax) {
                            M2Share.CastleMgr.IncRateGold(M2Share.Config.UpgradeWeaponPrice);
                        }
                    }
                    PlayObject.SendMsg(this, Messages.RM_USERSELLITEM_OK, 0, PlayObject.Gold, 0, 0, "");
                    AddItemToGoodsList(UserItem);
                    StdItem StdItem = M2Share.WorldEngine.GetStdItem(UserItem.Index);
                    if (StdItem.NeedIdentify == 1) {
                        M2Share.EventSource.AddEventLog(10, PlayObject.MapName + "\t" + PlayObject.CurrX + "\t" + PlayObject.CurrY + "\t" + PlayObject.ChrName + "\t" + StdItem.Name + "\t" + UserItem.MakeIndex + "\t" + '1' + "\t" + ChrName);
                    }
                    result = true;
                }
                else {
                    PlayObject.SendMsg(this, Messages.RM_USERSELLITEM_FAIL, 0, 0, 0, 0, "");
                }
            }
            else {
                PlayObject.SendMsg(this, Messages.RM_USERSELLITEM_FAIL, 0, 0, 0, 0, "");
            }
            return result;
        }

        private bool AddItemToGoodsList(UserItem UserItem) {
            if (UserItem.Dura <= 0) {
                return false;
            }
            IList<UserItem> ItemList = GetRefillList(UserItem.Index);
            if (ItemList == null) {
                ItemList = new List<UserItem>();
                GoodsList.Add(ItemList);
            }
            ItemList.Insert(0, UserItem);
            return true;
        }

        private bool ClientMakeDrugItem_sub_4A28FC(PlayObject PlayObject, string sItemName) {
            bool result = false;
            IList<MakeItem> List10 = M2Share.GetMakeItemInfo(sItemName);
            IList<DeleteItem> List28;
            if (List10 == null) {
                return result;
            }
            result = true;
            string s20;
            int n1C;
            for (int i = 0; i < List10.Count; i++) {
                s20 = List10[i].ItemName;
                n1C = List10[i].ItemCount;
                for (int j = 0; j < PlayObject.ItemList.Count; j++) {
                    if (M2Share.WorldEngine.GetStdItemName(PlayObject.ItemList[j].Index) == s20) {
                        n1C -= 1;
                    }
                }
                if (n1C > 0) {
                    result = false;
                    break;
                }
            }
            if (result) {
                List28 = null;
                for (int i = 0; i < List10.Count; i++) {
                    s20 = List10[i].ItemName;
                    n1C = List10[i].ItemCount;
                    for (int j = PlayObject.ItemList.Count - 1; j >= 0; j--) {
                        if (n1C <= 0) {
                            break;
                        }
                        UserItem UserItem = PlayObject.ItemList[j];
                        if (M2Share.WorldEngine.GetStdItemName(UserItem.Index) == s20) {
                            if (List28 == null) {
                                List28 = new List<DeleteItem>();
                            }
                            List28.Add(new DeleteItem() {
                                ItemName = s20,
                                MakeIndex = UserItem.MakeIndex
                            });
                            Dispose(UserItem);
                            PlayObject.ItemList.RemoveAt(j);
                            n1C -= 1;
                        }
                    }
                }
                if (List28 != null) {
                    int ObjectId = HUtil32.Sequence();
                    M2Share.ActorMgr.AddOhter(ObjectId, List28);
                    PlayObject.SendMsg(this, Messages.RM_SENDDELITEMLIST, 0, ObjectId, 0, 0, "");
                }
            }
            return result;
        }

        public void ClientMakeDrugItem(PlayObject PlayObject, string sItemName) {
            int n14 = 1;
            for (int i = 0; i < GoodsList.Count; i++) {
                IList<UserItem> List1C = GoodsList[i];
                UserItem MakeItem = List1C[0];
                StdItem StdItem = M2Share.WorldEngine.GetStdItem(MakeItem.Index);
                if (StdItem != null && StdItem.Name == sItemName) {
                    if (PlayObject.Gold >= M2Share.Config.MakeDurgPrice) {
                        if (ClientMakeDrugItem_sub_4A28FC(PlayObject, sItemName)) {
                            UserItem UserItem = new UserItem();
                            M2Share.WorldEngine.CopyToUserItemFromName(sItemName, ref UserItem);
                            if (PlayObject.AddItemToBag(UserItem)) {
                                PlayObject.Gold -= M2Share.Config.MakeDurgPrice;
                                PlayObject.SendAddItem(UserItem);
                                StdItem = M2Share.WorldEngine.GetStdItem(UserItem.Index);
                                if (StdItem.NeedIdentify == 1) {
                                    M2Share.EventSource.AddEventLog(2, PlayObject.MapName + "\t" + PlayObject.CurrX + "\t" + PlayObject.CurrY + "\t" + PlayObject.ChrName + "\t" + StdItem.Name + "\t" + UserItem.MakeIndex + "\t" + '1' + "\t" + ChrName);
                                }
                                n14 = 0;
                                break;
                            }
                            else {
                                DisPose(UserItem);
                                n14 = 2;
                            }
                        }
                        else {
                            n14 = 4;
                        }
                    }
                    else {
                        n14 = 3;
                    }
                }
            }
            if (n14 == 0) {
                PlayObject.SendMsg(this, Messages.RM_MAKEDRUG_SUCCESS, 0, PlayObject.Gold, 0, 0, "");
            }
            else {
                PlayObject.SendMsg(this, Messages.RM_MAKEDRUG_FAIL, 0, n14, 0, 0, "");
            }
        }

        /// <summary>
        /// 客户查询修复所需成本
        /// </summary>
        public void ClientQueryRepairCost(PlayObject PlayObject, UserItem UserItem) {
            int nRepairPrice;
            int nPrice = GetUserPrice(PlayObject, GetUserItemPrice(UserItem));
            if (nPrice > 0 && UserItem.DuraMax > UserItem.Dura) {
                if (UserItem.DuraMax > 0) {
                    nRepairPrice = HUtil32.Round((double)(nPrice / 3) / UserItem.DuraMax * (UserItem.DuraMax - UserItem.Dura));
                }
                else {
                    nRepairPrice = nPrice;
                }
                if (PlayObject.ScriptLable == ScriptConst.sSUPERREPAIR) {
                    if (IsSupRepair) {
                        nRepairPrice = nRepairPrice * M2Share.Config.SuperRepairPriceRate;
                    }
                    else {
                        nRepairPrice = -1;
                    }
                }
                else {
                    if (!IsRepair) {
                        nRepairPrice = -1;
                    }
                }
                PlayObject.SendMsg(this, Messages.RM_SENDREPAIRCOST, 0, nRepairPrice, 0, 0, "");
            }
            else {
                PlayObject.SendMsg(this, Messages.RM_SENDREPAIRCOST, 0, -1, 0, 0, "");
            }
        }

        /// <summary>
        /// 修理物品
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="UserItem"></param>
        /// <returns></returns>
        public bool ClientRepairItem(PlayObject PlayObject, UserItem UserItem) {
            int nRepairPrice;
            bool result = false;
            bool boCanRepair = true;
            if (PlayObject.ScriptLable == ScriptConst.sSUPERREPAIR && !IsSupRepair) {
                boCanRepair = false;
            }
            if (PlayObject.ScriptLable != ScriptConst.sSUPERREPAIR && !IsRepair) {
                boCanRepair = false;
            }
            if (PlayObject.ScriptLable == "@fail_s_repair") {
                SendMsgToUser(PlayObject, "对不起!我不能帮你修理这个物品。\\ \\ \\<返回/@main>");
                PlayObject.SendMsg(this, Messages.RM_USERREPAIRITEM_FAIL, 0, 0, 0, 0, "");
                return result;
            }
            int nPrice = GetUserPrice(PlayObject, GetUserItemPrice(UserItem));
            if (PlayObject.ScriptLable == ScriptConst.sSUPERREPAIR) {
                nPrice = nPrice * M2Share.Config.SuperRepairPriceRate;
            }
            StdItem StdItem = M2Share.WorldEngine.GetStdItem(UserItem.Index);
            if (StdItem != null) {
                if (boCanRepair && nPrice > 0 && UserItem.DuraMax > UserItem.Dura && StdItem.StdMode != 43) {
                    if (UserItem.DuraMax > 0) {
                        nRepairPrice = HUtil32.Round(nPrice / 3 / UserItem.DuraMax * (UserItem.DuraMax - UserItem.Dura));
                    }
                    else {
                        nRepairPrice = nPrice;
                    }
                    if (PlayObject.DecGold(nRepairPrice)) {
                        if (CastleMerchant || M2Share.Config.GetAllNpcTax) {
                            if (Castle != null) {
                                Castle.IncRateGold(nRepairPrice);
                            }
                            else if (M2Share.Config.GetAllNpcTax) {
                                M2Share.CastleMgr.IncRateGold(M2Share.Config.UpgradeWeaponPrice);
                            }
                        }
                        if (PlayObject.ScriptLable == ScriptConst.sSUPERREPAIR) {
                            UserItem.Dura = UserItem.DuraMax;
                            PlayObject.SendMsg(this, Messages.RM_USERREPAIRITEM_OK, 0, PlayObject.Gold, UserItem.Dura, UserItem.DuraMax, "");
                            GotoLable(PlayObject, ScriptConst.sSUPERREPAIROK, false);
                        }
                        else {
                            UserItem.DuraMax -= (ushort)((UserItem.DuraMax - UserItem.Dura) / M2Share.Config.RepairItemDecDura);
                            UserItem.Dura = UserItem.DuraMax;
                            PlayObject.SendMsg(this, Messages.RM_USERREPAIRITEM_OK, 0, PlayObject.Gold, UserItem.Dura, UserItem.DuraMax, "");
                            GotoLable(PlayObject, ScriptConst.sREPAIROK, false);
                        }
                        result = true;
                    }
                    else {
                        PlayObject.SendMsg(this, Messages.RM_USERREPAIRITEM_FAIL, 0, 0, 0, 0, "");
                    }
                }
                else {
                    PlayObject.SendMsg(this, Messages.RM_USERREPAIRITEM_FAIL, 0, 0, 0, 0, "");
                }
            }
            return result;
        }

        public override void ClearScript() {
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

        private void LoadUpgradeList() {
            UpgradeWeaponList.Clear();
            try {
                DataSource.CommonDB.LoadUpgradeWeaponRecord(ScriptName + '-' + MapName, UpgradeWeaponList);
            }
            catch {
                M2Share.Logger.Error("Failure in loading upgradinglist - " + ChrName);
            }
        }

        private void SaveUpgradingList() {
            try {
                DataSource.CommonDB.SaveUpgradeWeaponRecord(ScriptName + '-' + MapName, UpgradeWeaponList);
            }
            catch {
                M2Share.Logger.Error("Failure in saving upgradinglist - " + ChrName);
            }
        }

        /// <summary>
        /// 设置挂机留言信息
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sMsg"></param>
        protected static void SetOffLineMsg(PlayObject PlayObject, string sMsg) {
            PlayObject.OffLineLeaveWord = sMsg;
        }

        protected override void SendCustemMsg(PlayObject PlayObject, string sMsg) {
            base.SendCustemMsg(PlayObject, sMsg);
        }

        /// <summary>
        /// 清除临时文件，包括交易库存，价格表
        /// </summary>
        public void ClearData() {
            UserItem UserItem;
            IList<UserItem> ItemList;
            ItemPrice ItemPrice;
            const string sExceptionMsg = "[Exception] TMerchant::ClearData";
            try {
                for (int i = 0; i < GoodsList.Count; i++) {
                    ItemList = GoodsList[i];
                    for (int j = 0; j < ItemList.Count; j++) {
                        UserItem = ItemList[j];
                        Dispose(UserItem);
                    }
                }
                GoodsList.Clear();
                for (int i = 0; i < ItemPriceList.Count; i++) {
                    ItemPrice = ItemPriceList[i];
                    Dispose(ItemPrice);
                }
                ItemPriceList.Clear();
                SaveNPCData();
            }
            catch (Exception e) {
                M2Share.Logger.Error(sExceptionMsg);
                M2Share.Logger.Error(e.Message);
            }
        }

        private void ChangeUseItemName(PlayObject PlayObject, string sLabel, string sItemName) {
            if (!PlayObject.BoChangeItemNameFlag) {
                return;
            }
            PlayObject.BoChangeItemNameFlag = false;
            string sWhere = sLabel[ScriptConst.sUSEITEMNAME.Length..];
            byte btWhere = (byte)HUtil32.StrToInt(sWhere, -1);
            if (btWhere >= 0 && btWhere <= PlayObject.UseItems.Length) {
                UserItem UserItem = PlayObject.UseItems[btWhere];
                if (UserItem.Index == 0) {
                    string sMsg = Format(Settings.YourUseItemIsNul, M2Share.GetUseItemName(btWhere));
                    PlayObject.SendMsg(this, Messages.RM_MENU_OK, 0, PlayObject.ActorId, 0, 0, sMsg);
                    return;
                }
                if (UserItem.Desc[13] == 1) {
                    M2Share.CustomItemMgr.DelCustomItemName(UserItem.MakeIndex, UserItem.Index);
                }
                if (!string.IsNullOrEmpty(sItemName)) {
                    M2Share.CustomItemMgr.AddCustomItemName(UserItem.MakeIndex, UserItem.Index, sItemName);
                    UserItem.Desc[13] = 1;
                }
                else {
                    M2Share.CustomItemMgr.DelCustomItemName(UserItem.MakeIndex, UserItem.Index);
                    UserItem.Desc[13] = 0;
                }
                M2Share.CustomItemMgr.SaveCustomItemName();
                PlayObject.SendMsg(PlayObject, Messages.RM_SENDUSEITEMS, 0, 0, 0, 0, "");
                PlayObject.SendMsg(this, Messages.RM_MENU_OK, 0, PlayObject.ActorId, 0, 0, "");
            }
        }

        private static void DisPose(object obj) {
            obj = null;
        }
    }
}