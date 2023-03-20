using GameSrv.DataSource;
using GameSrv.Items;
using GameSrv.Maps;
using GameSrv.Player;
using GameSrv.Script;
using GameSrv.World.Managers;
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
        private readonly IList<IList<UserItem>> _goodsList;
        /// <summary>
        /// 物品价格列表
        /// </summary>
        private readonly IList<ItemPrice> _itemPriceList;
        /// <summary>
        /// 物品升级列表
        /// </summary>
        private readonly IList<WeaponUpgradeInfo> _upgradeWeaponList;
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
        public bool IsYbDeal = false;
        public bool CanItemMarket = false;

        public Merchant() : base() {
            RaceImg = ActorRace.Merchant;
            Appr = 0;
            PriceRate = 100;
            CastleMerchant = false;
            ItemTypeList = new List<int>();
            RefillGoodsList = new List<Goods>();
            _goodsList = new List<IList<UserItem>>();
            _itemPriceList = new List<ItemPrice>();
            _upgradeWeaponList = new List<WeaponUpgradeInfo>();
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
                var dwCurrentTick = HUtil32.GetTickCount();
                var dwDelayTick = ProcessRefillIndex * 500;
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

        protected override bool Operate(ProcessMessage processMsg) {
            return base.Operate(processMsg);
        }

        private void AddItemPrice(ushort nIndex, double nPrice) {
            var itemPrice = new ItemPrice {
                wIndex = nIndex,
                nPrice = nPrice
            };
            _itemPriceList.Add(itemPrice);
            LocalDb.SaveGoodPriceRecord(this, ScriptName + '-' + MapName);
        }

        private void CheckItemPrice(ushort nIndex) {
            for (var i = 0; i < _itemPriceList.Count; i++) {
                var itemPrice = _itemPriceList[i];
                if (itemPrice.wIndex == nIndex) {
                    var n10 = itemPrice.nPrice;
                    if (Math.Round(n10 * 1.1) > n10) {
                        n10 = HUtil32.Round(n10 * 1.1);
                    }
                    return;
                }
            }
            var stdItem = M2Share.WorldEngine.GetStdItem(nIndex);
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
                Goods goods;
                for (var i = 0; i < RefillGoodsList.Count; i++) {
                    goods = RefillGoodsList[i];
                    var goodCout = goods.Count;
                    if ((HUtil32.GetTickCount() - goods.RefillTick) > (goods.RefillTime * 60 * 1000)) {
                        goods.RefillTick = HUtil32.GetTickCount();
                        nIndex = M2Share.WorldEngine.GetStdItemIdx(goods.ItemName);
                        if (nIndex > 0) {
                            IList<UserItem> refillList = GetRefillList(nIndex);
                            var nRefillCount = refillList?.Count ?? 0;
                            if (goodCout > nRefillCount) {
                                CheckItemPrice(nIndex);
                                RefillGoodsItems(ref refillList, goods.ItemName, goods.Count - nRefillCount);
                                LocalDb.SaveGoodRecord(this, ScriptName + '-' + MapName);
                                LocalDb.SaveGoodPriceRecord(this, ScriptName + '-' + MapName);
                                return;
                            }
                            if (goodCout < nRefillCount) {
                                RefillDelReFillItem(ref refillList, nRefillCount - goods.Count);
                                LocalDb.SaveGoodRecord(this, ScriptName + '-' + MapName);
                                LocalDb.SaveGoodPriceRecord(this, ScriptName + '-' + MapName);
                            }
                        }
                    }
                }
                for (var i = 0; i < _goodsList.Count; i++) {
                    IList<UserItem> refillList20 = _goodsList[i];
                    if (refillList20.Count > 1000) {
                        var bo21 = false;
                        for (var j = 0; j < RefillGoodsList.Count; j++) {
                            goods = RefillGoodsList[j];
                            nIndex = M2Share.WorldEngine.GetStdItemIdx(goods.ItemName);
                            if (refillList20[0].Index == nIndex) {
                                bo21 = true;
                                break;
                            }
                        }
                        if (!bo21) {
                            RefillDelReFillItem(ref refillList20, refillList20.Count - 1000);
                        }
                        else {
                            RefillDelReFillItem(ref refillList20, refillList20.Count - 5000);
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
            for (var i = 0; i < _goodsList.Count; i++) {
                IList<UserItem> goods = _goodsList[i];
                if (goods.Count > 0) {
                    if (goods[0].Index == nIndex) {
                        return goods;
                    }
                }
            }
            return null;
        }

        private void RefillGoodsItems(ref IList<UserItem> list, string sItemName, int nInt) {
            if (list == null) {
                list = new List<UserItem>();
                _goodsList.Add(list);
            }
            for (var i = 0; i < nInt; i++) {
                var goodItem = new UserItem();
                if (M2Share.WorldEngine.CopyToUserItemFromName(sItemName, ref goodItem)) {
                    list.Insert(0, goodItem);
                }
                else {
                    Dispose(goodItem);
                }
            }
        }

        private static void RefillDelReFillItem(ref IList<UserItem> list, int nInt) {
            for (var i = list.Count - 1; i >= 0; i--) {
                if (nInt <= 0) {
                    break;
                }
                Dispose(list[i]);
                list.RemoveAt(i);
                nInt -= 1;
            }
        }

        private bool CheckItemType(int nStdMode) {
            var result = false;
            for (var i = 0; i < ItemTypeList.Count; i++) {
                if (ItemTypeList[i] == nStdMode) {
                    result = true;
                    break;
                }
            }
            return result;
        }

        private double GetItemPrice(ushort nIndex) {
            double result = -1;
            for (var i = 0; i < _itemPriceList.Count; i++) {
                var itemPrice = _itemPriceList[i];
                if (itemPrice.wIndex == nIndex) {
                    result = itemPrice.nPrice;
                    break;
                }
            }
            if (result < 0) {
                var stdItem = M2Share.WorldEngine.GetStdItem(nIndex);
                if (stdItem != null) {
                    if (CheckItemType(stdItem.StdMode)) {
                        result = stdItem.Price;
                    }
                }
            }
            return result;
        }

        private void UpgradeWaponAddValue(PlayObject user, IList<UserItem> itemList, ref byte btDc, ref byte btSc, ref byte btMc, ref byte btDura) {
            ClientItem stdItem80 = null;
            IList<DeleteItem> delItemList = null;
            int nDc;
            int nSc;
            int nMc;
            var nDcMin = 0;
            var nDcMax = 0;
            var nScMin = 0;
            var nScMax = 0;
            var nMcMin = 0;
            var nMcMax = 0;
            var nDura = 0;
            var nItemCount = 0;
            IList<double> duraList = new List<double>();
            for (var i = itemList.Count - 1; i >= 0; i--) {
                var userItem = itemList[i];
                if (M2Share.WorldEngine.GetStdItemName(userItem.Index) == M2Share.Config.BlackStone) {
                    duraList.Add(Math.Round(userItem.Dura / 1.0e3));
                    if (delItemList == null) {
                        delItemList = new List<DeleteItem>();
                    }
                    delItemList.Add(new DeleteItem() {
                        MakeIndex = userItem.MakeIndex,
                        ItemName = M2Share.Config.BlackStone
                    });
                    DisPose(userItem);
                    itemList.RemoveAt(i);
                }
                else {
                    if (M2Share.IsAccessory(userItem.Index)) {
                        var stdItem = M2Share.WorldEngine.GetStdItem(userItem.Index);
                        if (stdItem != null) {
                            stdItem.GetUpgradeStdItem(userItem, ref stdItem80);
                            //StdItem.GetItemAddValue(UserItem, ref StdItem80);
                            nDc = 0;
                            nSc = 0;
                            nMc = 0;
                            switch (stdItem80.Item.StdMode) {
                                case 19:
                                case 20:
                                case 21:
                                    nDc = HUtil32.HiWord(stdItem80.Item.DC) + HUtil32.LoWord(stdItem80.Item.DC);
                                    nSc = HUtil32.HiWord(stdItem80.Item.SC) + HUtil32.LoWord(stdItem80.Item.SC);
                                    nMc = HUtil32.HiWord(stdItem80.Item.MC) + HUtil32.LoWord(stdItem80.Item.MC);
                                    break;
                                case 22:
                                case 23:
                                    nDc = HUtil32.HiWord(stdItem80.Item.DC) + HUtil32.LoWord(stdItem80.Item.DC);
                                    nSc = HUtil32.HiWord(stdItem80.Item.SC) + HUtil32.LoWord(stdItem80.Item.SC);
                                    nMc = HUtil32.HiWord(stdItem80.Item.MC) + HUtil32.LoWord(stdItem80.Item.MC);
                                    break;
                                case 24:
                                case 26:
                                    nDc = HUtil32.HiWord(stdItem80.Item.DC) + HUtil32.LoWord(stdItem80.Item.DC) + 1;
                                    nSc = HUtil32.HiWord(stdItem80.Item.SC) + HUtil32.LoWord(stdItem80.Item.SC) + 1;
                                    nMc = HUtil32.HiWord(stdItem80.Item.MC) + HUtil32.LoWord(stdItem80.Item.MC) + 1;
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
                            if (delItemList == null) {
                                delItemList = new List<DeleteItem>();
                            }
                            delItemList.Add(new DeleteItem() {
                                ItemName = stdItem.Name,
                                MakeIndex = userItem.MakeIndex
                            });
                            if (stdItem.NeedIdentify == 1) {
                                M2Share.EventSource.AddEventLog(26, user.MapName + "\t" + user.CurrX + "\t" + user.CurrY + "\t" + user.ChrName + "\t" + stdItem.Name + "\t" + userItem.MakeIndex + "\t" + '1' + "\t" + '0');
                            }
                            DisPose(userItem);
                            itemList.RemoveAt(i);
                        }
                    }
                }
            }
            for (var i = 0; i < duraList.Count; i++) {
                for (var j = duraList.Count - 1; j > i; j--) {
                    if (duraList[j] > duraList[j - 1]) {
                        duraList.Reverse();
                    }
                }
            }
            for (var i = 0; i < duraList.Count; i++) {
                nDura = nDura + (int)duraList[i];
                nItemCount++;
                if (nItemCount >= 5) {
                    break;
                }
            }
            btDura = (byte)HUtil32.Round(HUtil32._MIN(5, nItemCount) + HUtil32._MIN(5, nItemCount) * (nDura / nItemCount / 5.0));
            btDc = (byte)(nDcMin / 5 + nDcMax / 3);
            btSc = (byte)(nScMin / 5 + nScMax / 3);
            btMc = (byte)(nMcMin / 5 + nMcMax / 3);
            if (delItemList != null) {
                var objectId = HUtil32.Sequence();
                M2Share.ActorMgr.AddOhter(objectId, delItemList);
                user.SendMsg(this, Messages.RM_SENDDELITEMLIST, 0, objectId, 0, 0, "");
            }
        }

        private void UpgradeWapon(PlayObject user) {
            var upgradeSuccess = false;
            WeaponUpgradeInfo upgradeInfo;
            for (var i = 0; i < _upgradeWeaponList.Count; i++) {
                upgradeInfo = _upgradeWeaponList[i];
                if (upgradeInfo.UserName == user.ChrName) {
                    GotoLable(user, ScriptConst.sUPGRADEING, false);
                    return;
                }
            }
            if (user.UseItems[ItemLocation.Weapon] != null && user.UseItems[ItemLocation.Weapon].Index != 0 && user.Gold >= M2Share.Config.UpgradeWeaponPrice
                && user.CheckItems(M2Share.Config.BlackStone) != null) {
                user.DecGold(M2Share.Config.UpgradeWeaponPrice);
                if (CastleMerchant || M2Share.Config.GetAllNpcTax) {
                    if (Castle != null) {
                        Castle.IncRateGold(M2Share.Config.UpgradeWeaponPrice);
                    }
                    else if (M2Share.Config.GetAllNpcTax) {
                        M2Share.CastleMgr.IncRateGold(M2Share.Config.UpgradeWeaponPrice);
                    }
                }
                user.GoldChanged();
                upgradeInfo = new WeaponUpgradeInfo {
                    UserName = user.ChrName,
                    UserItem = new UserItem(user.UseItems[ItemLocation.Weapon])
                };
                var stdItem = M2Share.WorldEngine.GetStdItem(user.UseItems[ItemLocation.Weapon].Index);
                if (stdItem.NeedIdentify == 1) {
                    M2Share.EventSource.AddEventLog(25, user.MapName + "\t" + user.CurrX + "\t" + user.CurrY + "\t" + user.ChrName + "\t" + stdItem.Name + "\t" + user.UseItems[ItemLocation.Weapon].MakeIndex + "\t" + '1' + "\t" + '0');
                }
                user.SendDelItems(user.UseItems[ItemLocation.Weapon]);
                user.UseItems[ItemLocation.Weapon].Index = 0;
                user.RecalcAbilitys();
                user.FeatureChanged();
                user.SendMsg(user, Messages.RM_ABILITY, 0, 0, 0, 0, "");
                UpgradeWaponAddValue(user, user.ItemList, ref upgradeInfo.Dc, ref upgradeInfo.Sc, ref upgradeInfo.Mc, ref upgradeInfo.Dura);
                upgradeInfo.UpgradeTime = DateTime.Now;
                upgradeInfo.GetBackTick = HUtil32.GetTickCount();
                _upgradeWeaponList.Add(upgradeInfo);
                SaveUpgradingList();
                upgradeSuccess = true;
            }
            if (upgradeSuccess) {
                GotoLable(user, ScriptConst.sUPGRADEOK, false);
            }
            else {
                GotoLable(user, ScriptConst.sUPGRADEFAIL, false);
            }
        }

        /// <summary>
        /// 取回升级武器
        /// </summary>
        /// <param name="user"></param>
        private void GetBackupgWeapon(PlayObject user) {
            WeaponUpgradeInfo upgradeInfo = default;
            var nFlag = 0;
            if (!user.IsEnoughBag()) {
                GotoLable(user, ScriptConst.sGETBACKUPGFULL, false);
                return;
            }
            for (var i = 0; i < _upgradeWeaponList.Count; i++) {
                if (string.Compare(_upgradeWeaponList[i].UserName, user.ChrName, StringComparison.OrdinalIgnoreCase) == 0) {
                    nFlag = 1;
                    if (((HUtil32.GetTickCount() - _upgradeWeaponList[i].GetBackTick) > M2Share.Config.UPgradeWeaponGetBackTime) || user.Permission >= 4) {
                        upgradeInfo = _upgradeWeaponList[i];
                        _upgradeWeaponList.RemoveAt(i);
                        SaveUpgradingList();
                        nFlag = 2;
                        break;
                    }
                }
            }
            if (!string.IsNullOrEmpty(upgradeInfo.UserName)) {
                if (HUtil32.RangeInDefined(upgradeInfo.Dura, 0, 8)) {
                    if (upgradeInfo.UserItem.DuraMax > 3000) {
                        upgradeInfo.UserItem.DuraMax -= 3000;
                    }
                    else {
                        upgradeInfo.UserItem.DuraMax = (ushort)(upgradeInfo.UserItem.DuraMax >> 1);
                    }
                    if (upgradeInfo.UserItem.Dura > upgradeInfo.UserItem.DuraMax) {
                        upgradeInfo.UserItem.Dura = upgradeInfo.UserItem.DuraMax;
                    }
                }
                else if (HUtil32.RangeInDefined(upgradeInfo.Dura, 9, 15)) {
                    if (M2Share.RandomNumber.Random(upgradeInfo.Dura) < 6) {
                        if (upgradeInfo.UserItem.DuraMax > 1000) {
                            upgradeInfo.UserItem.DuraMax -= 1000;
                        }
                        if (upgradeInfo.UserItem.Dura > upgradeInfo.UserItem.DuraMax) {
                            upgradeInfo.UserItem.Dura = upgradeInfo.UserItem.DuraMax;
                        }
                    }
                }
                else if (HUtil32.RangeInDefined(upgradeInfo.Dura, 18, 255)) {
                    var r = M2Share.RandomNumber.Random(upgradeInfo.Dura - 18);
                    if (HUtil32.RangeInDefined(r, 1, 4)) {
                        upgradeInfo.UserItem.DuraMax += 1000;
                    }
                    else if (HUtil32.RangeInDefined(r, 5, 7)) {
                        upgradeInfo.UserItem.DuraMax += 2000;
                    }
                    else if (HUtil32.RangeInDefined(r, 8, 255)) {
                        upgradeInfo.UserItem.DuraMax += 4000;
                    }
                }
                int n1C;
                if (upgradeInfo.Dc == upgradeInfo.Mc && upgradeInfo.Mc == upgradeInfo.Sc) {
                    n1C = M2Share.RandomNumber.Random(3);
                }
                else {
                    n1C = -1;
                }
                int n90;
                int n10;
                if (upgradeInfo.Dc >= upgradeInfo.Mc && upgradeInfo.Dc >= upgradeInfo.Sc || (n1C == 0)) {
                    n90 = HUtil32._MIN(11, upgradeInfo.Dc);
                    n10 = HUtil32._MIN(85, (n90 << 3 - n90) + 10 + upgradeInfo.UserItem.Desc[3] - upgradeInfo.UserItem.Desc[4] + user.BodyLuckLevel);
                    if (M2Share.RandomNumber.Random(M2Share.Config.UpgradeWeaponDCRate) < n10) {
                        upgradeInfo.UserItem.Desc[ItemAttr.WeaponUpgrade] = 10;
                        if (n10 > 63 && M2Share.RandomNumber.Random(M2Share.Config.UpgradeWeaponDCTwoPointRate) == 0) {
                            upgradeInfo.UserItem.Desc[ItemAttr.WeaponUpgrade] = 11;
                        }
                        if (n10 > 79 && M2Share.RandomNumber.Random(M2Share.Config.UpgradeWeaponDCThreePointRate) == 0) {
                            upgradeInfo.UserItem.Desc[ItemAttr.WeaponUpgrade] = 12;
                        }
                    }
                    else {
                        upgradeInfo.UserItem.Desc[ItemAttr.WeaponUpgrade] = 1;
                    }
                }
                if (upgradeInfo.Mc >= upgradeInfo.Dc && upgradeInfo.Mc >= upgradeInfo.Sc || n1C == 1) {
                    n90 = HUtil32._MIN(11, upgradeInfo.Mc);
                    n10 = HUtil32._MIN(85, (n90 << 3 - n90) + 10 + upgradeInfo.UserItem.Desc[3] - upgradeInfo.UserItem.Desc[4] + user.BodyLuckLevel);
                    if (M2Share.RandomNumber.Random(M2Share.Config.UpgradeWeaponMCRate) < n10) {
                        upgradeInfo.UserItem.Desc[ItemAttr.WeaponUpgrade] = 20;
                        if (n10 > 63 && M2Share.RandomNumber.Random(M2Share.Config.UpgradeWeaponMCTwoPointRate) == 0) {
                            upgradeInfo.UserItem.Desc[ItemAttr.WeaponUpgrade] = 21;
                        }
                        if (n10 > 79 && M2Share.RandomNumber.Random(M2Share.Config.UpgradeWeaponMCThreePointRate) == 0) {
                            upgradeInfo.UserItem.Desc[ItemAttr.WeaponUpgrade] = 22;
                        }
                    }
                    else {
                        upgradeInfo.UserItem.Desc[ItemAttr.WeaponUpgrade] = 1;
                    }
                }
                if (upgradeInfo.Sc >= upgradeInfo.Mc && upgradeInfo.Sc >= upgradeInfo.Dc || n1C == 2) {
                    n90 = HUtil32._MIN(11, upgradeInfo.Mc);
                    n10 = HUtil32._MIN(85, (n90 << 3 - n90) + 10 + upgradeInfo.UserItem.Desc[3] - upgradeInfo.UserItem.Desc[4] + user.BodyLuckLevel);
                    if (M2Share.RandomNumber.Random(M2Share.Config.UpgradeWeaponSCRate) < n10) {
                        upgradeInfo.UserItem.Desc[ItemAttr.WeaponUpgrade] = 30;
                        if (n10 > 63 && M2Share.RandomNumber.Random(M2Share.Config.UpgradeWeaponSCTwoPointRate) == 0) {
                            upgradeInfo.UserItem.Desc[ItemAttr.WeaponUpgrade] = 31;
                        }
                        if (n10 > 79 && M2Share.RandomNumber.Random(M2Share.Config.UpgradeWeaponSCThreePointRate) == 0) {
                            upgradeInfo.UserItem.Desc[ItemAttr.WeaponUpgrade] = 32;
                        }
                    }
                    else {
                        upgradeInfo.UserItem.Desc[ItemAttr.WeaponUpgrade] = 1;
                    }
                }
                var userItem = upgradeInfo.UserItem;
                var stdItem = M2Share.WorldEngine.GetStdItem(userItem.Index);
                if (stdItem.NeedIdentify == 1) {
                    M2Share.EventSource.AddEventLog(24, user.MapName + "\t" + user.CurrX + "\t" + user.CurrY + "\t" + user.ChrName + "\t" + stdItem.Name + "\t" + userItem.MakeIndex + "\t" + '1' + "\t" + '0');
                }
                user.AddItemToBag(userItem);
                user.SendAddItem(userItem);
                DisPose(upgradeInfo);
            }
            switch (nFlag) {
                case 0:
                    GotoLable(user, ScriptConst.sGETBACKUPGFAIL, false);
                    break;
                case 1:
                    GotoLable(user, ScriptConst.sGETBACKUPGING, false);
                    break;
                case 2:
                    GotoLable(user, ScriptConst.sGETBACKUPGOK, false);
                    break;
            }
        }

        /// <summary>
        /// 获取物品售卖价格
        /// </summary>
        /// <returns></returns>
        private int GetUserPrice(PlayObject playObject, double nPrice) {
            int result;
            if (CastleMerchant) {
                if (Castle != null && Castle.IsMasterGuild(playObject.MyGuild)) //沙巴克成员修复物品打折
                {
                    var n14 = HUtil32._MAX(60, HUtil32.Round(PriceRate * (M2Share.Config.CastleMemberPriceRate / 100)));//80%
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

        public override void UserSelect(PlayObject playObject, string sData) {
            var sLabel = string.Empty;
            const string sExceptionMsg = "[Exception] TMerchant::UserSelect... Data: {0}";
            base.UserSelect(playObject, sData);
            try {
                if (!CastleMerchant || !(Castle != null && Castle.UnderWar)) {
                    if (!playObject.Death && !string.IsNullOrEmpty(sData) && sData[0] == '@') {
                        var sMsg = HUtil32.GetValidStr3(sData, ref sLabel, '\r');
                        playObject.ScriptLable = sData;
                        var boCanJmp = playObject.LableIsCanJmp(sLabel);
                        if (string.Compare(sLabel, ScriptConst.sSL_SENDMSG, StringComparison.OrdinalIgnoreCase) == 0) {
                            if (string.IsNullOrEmpty(sMsg)) {
                                return;
                            }
                        }
                        GotoLable(playObject, sLabel, !boCanJmp);
                        if (!boCanJmp) {
                            return;
                        }
                        if (string.Compare(sLabel, ScriptConst.sOFFLINEMSG, StringComparison.OrdinalIgnoreCase) == 0)// 增加挂机
                        {
                            if (IsOffLineMsg) {
                                SetOffLineMsg(playObject, sMsg);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptConst.sSL_SENDMSG, StringComparison.OrdinalIgnoreCase) == 0) {
                            if (IsSendMsg) {
                                SendCustemMsg(playObject, sMsg);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptConst.SuperRepair, StringComparison.OrdinalIgnoreCase) == 0) {
                            if (IsSupRepair) {
                                UserSelectSuperRepairItem(playObject);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptConst.sBUY, StringComparison.OrdinalIgnoreCase) == 0) {
                            if (IsBuy) {
                                UserSelectBuyItem(playObject, 0);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptConst.sSELL, StringComparison.OrdinalIgnoreCase) == 0) {
                            if (IsSell) {
                                UserSelectSellItem(playObject);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptConst.sREPAIR, StringComparison.OrdinalIgnoreCase) == 0) {
                            if (IsRepair) {
                                UserSelectRepairItem(playObject);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptConst.sMAKEDURG, StringComparison.OrdinalIgnoreCase) == 0) {
                            if (IsMakeDrug) {
                                UserSelectMakeDurg(playObject);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptConst.sPRICES, StringComparison.OrdinalIgnoreCase) == 0) {
                            if (IsPrices) {
                                UserSelectItemPrices(playObject);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptConst.sSTORAGE, StringComparison.OrdinalIgnoreCase) == 0) {
                            if (IsStorage) {
                                UserSelectStorage(playObject);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptConst.sGETBACK, StringComparison.OrdinalIgnoreCase) == 0) {
                            if (IsGetback) {
                                UserSelectGetBack(playObject);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptConst.sUPGRADENOW, StringComparison.OrdinalIgnoreCase) == 0) {
                            if (IsUpgradenow) {
                                UpgradeWapon(playObject);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptConst.sGETBACKUPGNOW, StringComparison.OrdinalIgnoreCase) == 0) {
                            if (IsGetBackupgnow) {
                                GetBackupgWeapon(playObject);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptConst.sGETMARRY, StringComparison.OrdinalIgnoreCase) == 0) {
                            if (IsGetMarry) {
                                GetBackupgWeapon(playObject);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptConst.sGETMASTER, StringComparison.OrdinalIgnoreCase) == 0) {
                            if (IsGetMaster) {
                                GetBackupgWeapon(playObject);
                            }
                        }
                        else if (HUtil32.CompareLStr(sLabel, ScriptConst.UseItemName)) {
                            if (IsUseItemName) {
                                ChangeUseItemName(playObject, sLabel, sMsg);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptConst.sEXIT, StringComparison.OrdinalIgnoreCase) == 0) {
                            playObject.SendMsg(this, Messages.RM_MERCHANTDLGCLOSE, 0, ActorId, 0, 0, "");
                        }
                        else if (string.Compare(sLabel, ScriptConst.sBACK, StringComparison.OrdinalIgnoreCase) == 0) {
                            if (string.IsNullOrEmpty(playObject.ScriptGoBackLable)) {
                                playObject.ScriptGoBackLable = ScriptConst.sMAIN;
                            }
                            GotoLable(playObject, playObject.ScriptGoBackLable, false);
                        }
                        else if (string.Compare(sLabel, ScriptConst.sDealYBme, StringComparison.OrdinalIgnoreCase) == 0) // 元宝寄售:出售物品 
                        {
                            if (IsYbDeal) {
                                UserSelectOpenDealOffForm(playObject); // 打开出售物品窗口
                            }
                        }
                        else if (CanItemMarket) //拍卖行
                        {
                            if (string.Compare(sLabel, "market_0", StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                SendUserMarket(playObject, MarketConst.USERMARKET_TYPE_ALL, MarketConst.USERMARKET_MODE_BUY);
                            }
                            else if (string.Compare(sLabel, "@market_sell", StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                SendUserMarket(playObject, MarketConst.USERMARKET_TYPE_ALL, MarketConst.USERMARKET_MODE_SELL);
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

        private void SendUserMarket(PlayObject user, short ItemType, byte UserMode)
        {
            switch (UserMode)
            {
                case MarketConst.USERMARKET_MODE_BUY:
                case MarketConst.USERMARKET_MODE_INQUIRY:
                    RequireLoadUserMarket(user, M2Share.Config.ServerName + '_' + this.ChrName, ItemType, UserMode, "", "");
                    break;
                case MarketConst.USERMARKET_MODE_SELL:
                    SendUserMarketSellReady(user);
                    break;
            }
        }
        
        private void RequireLoadUserMarket(PlayObject user,string MarketName, short ItemType, byte UserMode, string OtherName, string ItemName)
        {
            var IsOk = false;
            var marKetReqInfo = new MarKetReqInfo();
            marKetReqInfo.UserName = ChrName;
            marKetReqInfo.MarketName = MarketName;
            marKetReqInfo.SearchWho = OtherName;
            marKetReqInfo.SearchItem = ItemName;
            marKetReqInfo.ItemType = ItemType;
            marKetReqInfo.ItemSet = 0;
            marKetReqInfo.UserMode = UserMode;
            
            switch (ItemType)
            {
                case MarketConst.USERMARKET_TYPE_ALL:
                case MarketConst.USERMARKET_TYPE_WEAPON:
                case MarketConst.USERMARKET_TYPE_NECKLACE:
                case MarketConst.USERMARKET_TYPE_RING:
                case MarketConst.USERMARKET_TYPE_BRACELET:
                case MarketConst.USERMARKET_TYPE_CHARM:
                case MarketConst.USERMARKET_TYPE_HELMET:
                case MarketConst.USERMARKET_TYPE_BELT:
                case MarketConst.USERMARKET_TYPE_SHOES:
                case MarketConst.USERMARKET_TYPE_ARMOR:
                case MarketConst.USERMARKET_TYPE_DRINK:
                case MarketConst.USERMARKET_TYPE_JEWEL:
                case MarketConst.USERMARKET_TYPE_BOOK:
                case MarketConst.USERMARKET_TYPE_MINERAL:
                case MarketConst.USERMARKET_TYPE_QUEST:
                case MarketConst.USERMARKET_TYPE_ETC:
                case MarketConst.USERMARKET_TYPE_OTHER:
                case MarketConst.USERMARKET_TYPE_ITEMNAME:
                    IsOk = true;
                    break;
                case MarketConst.USERMARKET_TYPE_SET:
                    marKetReqInfo.ItemSet = 1;
                    IsOk = true;
                    break;
                case MarketConst.USERMARKET_TYPE_MINE:
                    marKetReqInfo.SearchWho = ChrName;
                    IsOk = true;
                    break;
            }
            if (IsOk)
            {
                if (M2Share.MarketService.RequestLoadPageUserMarket(user.ActorId, marKetReqInfo))
                {
                    SendUserMarketCloseMsg(user);
                }
            }
        }

        private void SendUserMarketCloseMsg(PlayObject user)
        {
            user.SendMsg(this,  Messages.RM_MARKET_RESULT, 0, 0, MarketConst.UMResult_MarketNotReady, 0, "");
            user.SendMsg(this, Messages.RM_MENU_OK, 0, ActorId, 0, 0, "你不能使用寄售商人功能。");
        }
        
        private void SendUserMarketSellReady(PlayObject user)
        {
            if (!M2Share.Config.EnableMarket)
            {
                SysMsg("寄售商人功能无法使用。", MsgColor.Red, MsgType.Hint);
            }
            //M2Share.MarketService.SendUserMarketSellReady(this.ActorId, user.ActorId,);
        }
        
        /// <summary>
        /// 特殊修理物品
        /// </summary>
        private void UserSelectSuperRepairItem(PlayObject user) {
            user.SendMsg(this, Messages.RM_SENDUSERSREPAIR, 0, ActorId, 0, 0, "");
        }

        /// <summary>
        /// 物品详情列表
        /// </summary>
        private void UserSelectBuyItem(PlayObject user, int nInt) {
            var sSendMsg = string.Empty;
            var n10 = 0;
            for (var i = 0; i < _goodsList.Count; i++) {
                IList<UserItem> goods = _goodsList[i];
                var userItem = goods[0];
                var stdItem = M2Share.WorldEngine.GetStdItem(userItem.Index);
                if (stdItem != null) {
                    var sName = CustomItem.GetItemName(userItem);
                    var nPrice = GetUserPrice(user, GetItemPrice(userItem.Index));
                    var nStock = goods.Count;
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
            user.SendMsg(this, Messages.RM_SENDGOODSLIST, 0, ActorId, n10, 0, sSendMsg);
        }

        private void UserSelectSellItem(PlayObject user) {
            user.SendMsg(this, Messages.RM_SENDUSERSELL, 0, ActorId, 0, 0, "");
        }

        private void UserSelectRepairItem(PlayObject user) {
            user.SendMsg(this, Messages.RM_SENDUSERREPAIR, 0, ActorId, 0, 0, "");
        }

        private void UserSelectMakeDurg(PlayObject user) {
            var sSendMsg = string.Empty;
            for (var i = 0; i < _goodsList.Count; i++) {
                var stdItem = M2Share.WorldEngine.GetStdItem(_goodsList[i][0].Index);
                if (stdItem != null) {
                    sSendMsg = sSendMsg + stdItem.Name + '/' + 0 + '/' + M2Share.Config.MakeDurgPrice + '/' + 1 + '/';
                }
            }
            if (!string.IsNullOrEmpty(sSendMsg)) {
                user.SendMsg(this, Messages.RM_USERMAKEDRUGITEMLIST, 0, ActorId, 0, 0, sSendMsg);
            }
        }

        private static void UserSelectItemPrices(PlayObject user) {

        }

        private void UserSelectStorage(PlayObject user) {
            user.SendMsg(this, Messages.RM_USERSTORAGEITEM, 0, ActorId, 0, 0, "");
        }

        private void UserSelectGetBack(PlayObject user) {
            user.SendMsg(this, Messages.RM_USERGETBACKITEM, 0, ActorId, 0, 0, "");
        }

        /// <summary>
        /// 打开出售物品窗口
        /// </summary>
        /// <param name="user"></param>
        private void UserSelectOpenDealOffForm(PlayObject user) {
            if (user.SaleDeal) {
                if (!user.SellOffInTime(0)) {
                    user.SendMsg(this, Messages.RM_SENDDEALOFFFORM, 0, ActorId, 0, 0, "");
                    user.GetBackSellOffItems();
                }
                else {
                    user.SendMsg(this, Messages.RM_MERCHANTSAY, 0, 0, 0, 0, ChrName + "/您还有元宝服务正在进行!!\\ \\<返回/@main>");
                }
            }
            else {
                user.SendMsg(this, Messages.RM_MERCHANTSAY, 0, 0, 0, 0, ChrName + "/您未开通元宝服务,请先开通元宝服务!!\\ \\<返回/@main>");
            }
        }

        public void LoadNpcData() {
            var sFile = ScriptName + '-' + MapName;
            LocalDb.LoadGoodRecord(this, sFile);
            LocalDb.LoadGoodPriceRecord(this, sFile);
            LoadUpgradeList();
        }

        private void SaveNpcData() {
            var sFile = ScriptName + '-' + MapName;
            LocalDb.SaveGoodRecord(this, sFile);
            LocalDb.SaveGoodPriceRecord(this, sFile);
        }

        /// <summary>
        /// 清理武器升级过期数据
        /// </summary>
        private void ClearExpreUpgradeListData() {
            for (var i = _upgradeWeaponList.Count - 1; i >= 0; i--) {
                var upgradeInfo = _upgradeWeaponList[i];
                if ((DateTime.Now - upgradeInfo.UpgradeTime).TotalDays >= M2Share.Config.ClearExpireUpgradeWeaponDays) {
                    Dispose(upgradeInfo);
                    _upgradeWeaponList.RemoveAt(i);
                }
            }
        }

        public void LoadMerchantScript() {
            ItemTypeList.Clear();
            m_sPath = ScriptConst.sMarket_Def;
            var scriptPath = ScriptName + '-' + MapName;
            M2Share.ScriptSystem.LoadScriptFile(this, ScriptConst.sMarket_Def, scriptPath, true);
        }

        public override void Click(PlayObject playObject) {
            base.Click(playObject);
        }

        protected override void GetVariableText(PlayObject playObject, string sVariable, ref string sMsg) {
            string sText;
            base.GetVariableText(playObject, sVariable, ref sMsg);
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
                        if (playObject.UseItems[ItemLocation.Weapon] != null && playObject.UseItems[ItemLocation.Weapon].Index != 0)
                        {
                            sText = M2Share.WorldEngine.GetStdItemName(playObject.UseItems[ItemLocation.Weapon].Index);
                        }
                        else {
                            sText = "无";
                        }
                        sMsg = ReplaceVariableText(sMsg, "<$USERWEAPON>", sText);
                        break;
                    }
            }
        }

        private double GetUserItemPrice(UserItem userItem) {
            var itemPrice = GetItemPrice(userItem.Index);
            if (itemPrice > 0) {
                var stdItem = M2Share.WorldEngine.GetStdItem(userItem.Index);
                if (stdItem != null && stdItem.StdMode > 4 && stdItem.DuraMax > 0 && userItem.DuraMax > 0) {
                    double n20;
                    switch (stdItem.StdMode)
                    {
                        case 40:// 肉
                            {
                                if (userItem.Dura <= userItem.DuraMax) {
                                    n20 = itemPrice / 2.0 / userItem.DuraMax * (userItem.DuraMax - userItem.Dura);
                                    itemPrice = HUtil32._MAX(2, HUtil32.Round(itemPrice - n20));
                                }
                                else {
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
                                var n14 = 0;
                                var nC = 0;
                                while (true) {
                                    if (stdItem.StdMode == 5 || stdItem.StdMode == 6) {
                                        if (nC != 4 || nC != 9) {
                                            if (nC == 6) {
                                                if (userItem.Desc[nC] > 10) {
                                                    n14 = n14 + (userItem.Desc[nC] - 10) * 2;
                                                }
                                            }
                                            else {
                                                n14 = n14 + userItem.Desc[nC];
                                            }
                                        }
                                    }
                                    else {
                                        n14 += userItem.Desc[nC];
                                    }
                                    nC++;
                                    if (nC >= 8) {
                                        break;
                                    }
                                }
                                if (n14 > 0) {
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

        public void ClientBuyItem(PlayObject playObject, string sItemName, int nInt) {
            var bo29 = false;
            var n1C = 1;
            for (var i = 0; i < _goodsList.Count; i++) {
                if (bo29) {
                    break;
                }
                IList<UserItem> list20 = _goodsList[i];
                var userItem = list20[0];
                var stdItem = M2Share.WorldEngine.GetStdItem(userItem.Index);
                if (stdItem != null) {
                    var sUserItemName = CustomItem.GetItemName(userItem);
                    if (playObject.IsAddWeightAvailable(stdItem.Weight)) {
                        if (sUserItemName == sItemName) {
                            for (var j = 0; j < list20.Count; j++) {
                                userItem = list20[j];
                                if (stdItem.StdMode <= 4 || stdItem.StdMode == 42 || stdItem.StdMode == 31 || userItem.MakeIndex == nInt) {
                                    var nPrice = GetUserPrice(playObject, GetUserItemPrice(userItem));
                                    if (playObject.Gold >= nPrice && nPrice > 0) {
                                        if (playObject.AddItemToBag(userItem)) {
                                            playObject.Gold -= nPrice;
                                            if (CastleMerchant || M2Share.Config.GetAllNpcTax) {
                                                if (Castle != null) {
                                                    Castle.IncRateGold(nPrice);
                                                }
                                                else if (M2Share.Config.GetAllNpcTax) {
                                                    M2Share.CastleMgr.IncRateGold(M2Share.Config.UpgradeWeaponPrice);
                                                }
                                            }
                                            playObject.SendAddItem(userItem);
                                            if (stdItem.NeedIdentify == 1) {
                                                M2Share.EventSource.AddEventLog(9, playObject.MapName + "\t" + playObject.CurrX + "\t" + playObject.CurrY + "\t" + playObject.ChrName + "\t" + stdItem.Name + "\t" + userItem.MakeIndex + "\t" + '1' + "\t" + ChrName);
                                            }
                                            list20.RemoveAt(j);
                                            if (list20.Count <= 0) {
                                                _goodsList.RemoveAt(i);
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
                playObject.SendMsg(this, Messages.SM_BUYITEM_SUCCESS, 0, playObject.Gold, nInt, 0, "");
            }
            else {
                playObject.SendMsg(this, Messages.SM_BUYITEM_FAIL, 0, n1C, 0, 0, "");
            }
        }

        public void ClientGetDetailGoodsList(PlayObject playObject, string sItemName, int nInt) {
            var sSendMsg = string.Empty;
            var nItemCount = 0;
            for (var i = 0; i < _goodsList.Count; i++) {
                IList<UserItem> list20 = _goodsList[i];
                if (list20.Count <= 0) {
                    continue;
                }
                var userItem = list20[0];
                var item = M2Share.WorldEngine.GetStdItem(userItem.Index);
                if (item != null && item.Name == sItemName) {
                    if (list20.Count - 1 < nInt) {
                        nInt = HUtil32._MAX(0, list20.Count - 10);
                    }
                    for (var j = list20.Count - 1; j >= 0; j--) {
                        userItem = list20[j];
                        var clientItem = new ClientItem();
                        item.GetUpgradeStdItem(userItem, ref clientItem);
                        //Item.GetItemAddValue(UserItem, ref ClientItem.Item);
                        clientItem.Dura = userItem.Dura;
                        clientItem.DuraMax = (ushort)GetUserPrice(playObject, GetUserItemPrice(userItem));
                        clientItem.MakeIndex = userItem.MakeIndex;
                        sSendMsg = sSendMsg + EDCode.EncodeBuffer(clientItem) + "/";
                        nItemCount++;
                        if (nItemCount >= 10) {
                            break;
                        }
                    }
                    break;
                }
            }
            playObject.SendMsg(this, Messages.RM_SENDDETAILGOODSLIST, 0, ActorId, nItemCount, nInt, sSendMsg);
        }

        public void ClientQuerySellPrice(PlayObject playObject, UserItem userItem) {
            var nC = GetSellItemPrice(GetUserItemPrice(userItem));
            if (nC >= 0) {
                playObject.SendMsg(this, Messages.RM_SENDBUYPRICE, 0, nC, 0, 0, "");
            }
            else {
                playObject.SendMsg(this, Messages.RM_SENDBUYPRICE, 0, 0, 0, 0, "");
            }
        }

        private static int GetSellItemPrice(double nPrice) {
            return HUtil32.Round(nPrice / 2.0);
        }

        private static bool ClientSellItem_sub_4A1C84(UserItem userItem) {
            var result = true;
            var stdItem = M2Share.WorldEngine.GetStdItem(userItem.Index);
            if (stdItem != null && (stdItem.StdMode == 25 || stdItem.StdMode == 30)) {
                if (userItem.Dura < 4000) {
                    result = false;
                }
            }
            return result;
        }

        public bool ClientSellItem(PlayObject playObject, UserItem userItem) {
            var result = false;
            var nPrice = GetSellItemPrice(GetUserItemPrice(userItem));
            if (nPrice > 0 && ClientSellItem_sub_4A1C84(userItem)) {
                if (playObject.IncGold(nPrice)) {
                    if (CastleMerchant || M2Share.Config.GetAllNpcTax) {
                        if (Castle != null) {
                            Castle.IncRateGold(nPrice);
                        }
                        else if (M2Share.Config.GetAllNpcTax) {
                            M2Share.CastleMgr.IncRateGold(M2Share.Config.UpgradeWeaponPrice);
                        }
                    }
                    playObject.SendMsg(this, Messages.RM_USERSELLITEM_OK, 0, playObject.Gold, 0, 0, "");
                    AddItemToGoodsList(userItem);
                    var stdItem = M2Share.WorldEngine.GetStdItem(userItem.Index);
                    if (stdItem.NeedIdentify == 1) {
                        M2Share.EventSource.AddEventLog(10, playObject.MapName + "\t" + playObject.CurrX + "\t" + playObject.CurrY + "\t" + playObject.ChrName + "\t" + stdItem.Name + "\t" + userItem.MakeIndex + "\t" + '1' + "\t" + ChrName);
                    }
                    result = true;
                }
                else {
                    playObject.SendMsg(this, Messages.RM_USERSELLITEM_FAIL, 0, 0, 0, 0, "");
                }
            }
            else {
                playObject.SendMsg(this, Messages.RM_USERSELLITEM_FAIL, 0, 0, 0, 0, "");
            }
            return result;
        }

        private void AddItemToGoodsList(UserItem userItem) {
            if (userItem.Dura <= 0)
            {
                return;
            }
            IList<UserItem> itemList = GetRefillList(userItem.Index);
            if (itemList == null) {
                itemList = new List<UserItem>();
                _goodsList.Add(itemList);
            }
            itemList.Insert(0, userItem);
        }

        private bool ClientMakeDrugItem_sub_4A28FC(PlayObject playObject, string sItemName) {
            var result = false;
            IList<MakeItem> list10 = M2Share.GetMakeItemInfo(sItemName);
            IList<DeleteItem> list28;
            if (list10 == null) {
                return result;
            }
            result = true;
            string s20;
            int n1C;
            for (var i = 0; i < list10.Count; i++) {
                s20 = list10[i].ItemName;
                n1C = list10[i].ItemCount;
                for (var j = 0; j < playObject.ItemList.Count; j++) {
                    if (M2Share.WorldEngine.GetStdItemName(playObject.ItemList[j].Index) == s20) {
                        n1C -= 1;
                    }
                }
                if (n1C > 0) {
                    result = false;
                    break;
                }
            }
            if (result) {
                list28 = null;
                for (var i = 0; i < list10.Count; i++) {
                    s20 = list10[i].ItemName;
                    n1C = list10[i].ItemCount;
                    for (var j = playObject.ItemList.Count - 1; j >= 0; j--) {
                        if (n1C <= 0) {
                            break;
                        }
                        var userItem = playObject.ItemList[j];
                        if (M2Share.WorldEngine.GetStdItemName(userItem.Index) == s20) {
                            if (list28 == null) {
                                list28 = new List<DeleteItem>();
                            }
                            list28.Add(new DeleteItem() {
                                ItemName = s20,
                                MakeIndex = userItem.MakeIndex
                            });
                            Dispose(userItem);
                            playObject.ItemList.RemoveAt(j);
                            n1C -= 1;
                        }
                    }
                }
                if (list28 != null) {
                    var objectId = HUtil32.Sequence();
                    M2Share.ActorMgr.AddOhter(objectId, list28);
                    playObject.SendMsg(this, Messages.RM_SENDDELITEMLIST, 0, objectId, 0, 0, "");
                }
            }
            return result;
        }

        public void ClientMakeDrugItem(PlayObject playObject, string sItemName) {
            byte n14 = 1;
            for (var i = 0; i < _goodsList.Count; i++)
            {
                IList<UserItem> list1C = _goodsList[i];
                var makeItem = list1C[0];
                var stdItem = M2Share.WorldEngine.GetStdItem(makeItem.Index);
                if (stdItem != null && string.Compare(stdItem.Name, sItemName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    if (playObject.Gold >= M2Share.Config.MakeDurgPrice)
                    {
                        if (ClientMakeDrugItem_sub_4A28FC(playObject, sItemName))
                        {
                            var userItem = new UserItem();
                            M2Share.WorldEngine.CopyToUserItemFromName(sItemName, ref userItem);
                            if (playObject.AddItemToBag(userItem))
                            {
                                playObject.Gold -= M2Share.Config.MakeDurgPrice;
                                playObject.SendAddItem(userItem);
                                stdItem = M2Share.WorldEngine.GetStdItem(userItem.Index);
                                if (stdItem.NeedIdentify == 1)
                                {
                                    M2Share.EventSource.AddEventLog(2, playObject.MapName + "\t" + playObject.CurrX + "\t" + playObject.CurrY + "\t" + playObject.ChrName + "\t" + stdItem.Name + "\t" + userItem.MakeIndex + "\t" + '1' + "\t" + ChrName);
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
            if (n14 == 0) {
                playObject.SendMsg(this, Messages.RM_MAKEDRUG_SUCCESS, 0, playObject.Gold, 0, 0, "");
            }
            else {
                playObject.SendMsg(this, Messages.RM_MAKEDRUG_FAIL, 0, n14, 0, 0, "");
            }
        }

        /// <summary>
        /// 客户查询修复所需成本
        /// </summary>
        public void ClientQueryRepairCost(PlayObject playObject, UserItem userItem) {
            int nRepairPrice;
            var nPrice = GetUserPrice(playObject, GetUserItemPrice(userItem));
            if (nPrice > 0 && userItem.DuraMax > userItem.Dura) {
                if (userItem.DuraMax > 0) {
                    nRepairPrice = HUtil32.Round((double)(nPrice / 3) / userItem.DuraMax * (userItem.DuraMax - userItem.Dura));
                }
                else {
                    nRepairPrice = nPrice;
                }
                if (string.Compare(playObject.ScriptLable, ScriptConst.SuperRepair, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    if (IsSupRepair)
                    {
                        nRepairPrice = nRepairPrice * M2Share.Config.SuperRepairPriceRate;
                    }
                    else
                    {
                        nRepairPrice = -1;
                    }
                }
                else {
                    if (!IsRepair) {
                        nRepairPrice = -1;
                    }
                }
                playObject.SendMsg(this, Messages.RM_SENDREPAIRCOST, 0, nRepairPrice, 0, 0, "");
            }
            else {
                playObject.SendMsg(this, Messages.RM_SENDREPAIRCOST, 0, -1, 0, 0, "");
            }
        }

        /// <summary>
        /// 修理物品
        /// </summary>
        /// <returns></returns>
        public void ClientRepairItem(PlayObject playObject, UserItem userItem) {
            var supRepair = string.Compare(playObject.ScriptLable, ScriptConst.SuperRepair, StringComparison.OrdinalIgnoreCase) == 0;
            if (supRepair && !IsSupRepair)
            {
                return;//不支持特殊修理
            }
            if (!supRepair && !IsRepair)
            {
                return; //不支持修理和特殊修理
            }
            if (string.Compare(playObject.ScriptLable, ScriptConst.Superrepairfail, StringComparison.OrdinalIgnoreCase) == 0)
            {
                SendMsgToUser(playObject, "对不起!我不能帮你修理这个物品。\\ \\ \\<返回/@main>");
                playObject.SendMsg(this, Messages.RM_USERREPAIRITEM_FAIL, 0, 0, 0, 0, "");
                return;
            }
            var nPrice = GetUserPrice(playObject, GetUserItemPrice(userItem));
            if (supRepair) {
                nPrice = nPrice * M2Share.Config.SuperRepairPriceRate;
            }
            var stdItem = M2Share.WorldEngine.GetStdItem(userItem.Index);
            if (stdItem != null) {
                if (nPrice > 0 && userItem.DuraMax > userItem.Dura && stdItem.StdMode != 43) {
                    int nRepairPrice;
                    if (userItem.DuraMax > 0) {
                        nRepairPrice = HUtil32.Round(nPrice / 3 / userItem.DuraMax * (userItem.DuraMax - userItem.Dura));
                    }
                    else {
                        nRepairPrice = nPrice;
                    }
                    if (playObject.DecGold(nRepairPrice)) {
                        if (CastleMerchant || M2Share.Config.GetAllNpcTax) {
                            if (Castle != null) {
                                Castle.IncRateGold(nRepairPrice);
                            }
                            else if (M2Share.Config.GetAllNpcTax) {
                                M2Share.CastleMgr.IncRateGold(M2Share.Config.UpgradeWeaponPrice);
                            }
                        }
                        if (supRepair) {
                            userItem.Dura = userItem.DuraMax;
                            playObject.SendMsg(this, Messages.RM_USERREPAIRITEM_OK, 0, playObject.Gold, userItem.Dura, userItem.DuraMax, "");
                            GotoLable(playObject, ScriptConst.sSUPERREPAIROK, false);
                        }
                        else {
                            userItem.DuraMax -= (ushort)((userItem.DuraMax - userItem.Dura) / M2Share.Config.RepairItemDecDura);
                            userItem.Dura = userItem.DuraMax;
                            playObject.SendMsg(this, Messages.RM_USERREPAIRITEM_OK, 0, playObject.Gold, userItem.Dura, userItem.DuraMax, "");
                            GotoLable(playObject, ScriptConst.sREPAIROK, false);
                        }
                    }
                    else {
                        playObject.SendMsg(this, Messages.RM_USERREPAIRITEM_FAIL, 0, 0, 0, 0, "");
                    }
                }
                else {
                    playObject.SendMsg(this, Messages.RM_USERREPAIRITEM_FAIL, 0, 0, 0, 0, "");
                }
            }
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
            _upgradeWeaponList.Clear();
            try {
                CommonDB.LoadUpgradeWeaponRecord(ScriptName + '-' + MapName, _upgradeWeaponList);
            }
            catch {
                M2Share.Logger.Error("Failure in loading upgradinglist - " + ChrName);
            }
        }

        private void SaveUpgradingList() {
            try {
                CommonDB.SaveUpgradeWeaponRecord(ScriptName + '-' + MapName, _upgradeWeaponList);
            }
            catch {
                M2Share.Logger.Error("Failure in saving upgradinglist - " + ChrName);
            }
        }

        /// <summary>
        /// 设置挂机留言信息
        /// </summary>
        /// <param name="playObject"></param>
        /// <param name="sMsg"></param>
        protected static void SetOffLineMsg(PlayObject playObject, string sMsg) {
            playObject.OffLineLeaveWord = sMsg;
        }

        protected override void SendCustemMsg(PlayObject playObject, string sMsg) {
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
                for (var i = 0; i < _goodsList.Count; i++)
                {
                    IList<UserItem> itemList = _goodsList[i];
                    for (var j = 0; j < itemList.Count; j++)
                    {
                        var userItem = itemList[j];
                        Dispose(userItem);
                    }
                }
                _goodsList.Clear();
                for (var i = 0; i < _itemPriceList.Count; i++)
                {
                    var itemPrice = _itemPriceList[i];
                    Dispose(itemPrice);
                }
                _itemPriceList.Clear();
                SaveNpcData();
            }
            catch (Exception e)
            {
                M2Share.Logger.Error(sExceptionMsg);
                M2Share.Logger.Error(e.Message);
            }
        }

        private void ChangeUseItemName(PlayObject playObject, string sLabel, string sItemName) {
            if (!playObject.BoChangeItemNameFlag) {
                return;
            }
            playObject.BoChangeItemNameFlag = false;
            var sWhere = sLabel[ScriptConst.UseItemName.Length..];
            var btWhere = (byte)HUtil32.StrToInt(sWhere, -1);
            if (btWhere >= 0 && btWhere <= playObject.UseItems.Length) {
                var userItem = playObject.UseItems[btWhere];
                if (userItem.Index == 0) {
                    var sMsg = Format(Settings.YourUseItemIsNul, M2Share.GetUseItemName(btWhere));
                    playObject.SendMsg(this, Messages.RM_MENU_OK, 0, playObject.ActorId, 0, 0, sMsg);
                    return;
                }
                if (userItem.Desc[13] == 1) {
                    M2Share.CustomItemMgr.DelCustomItemName(userItem.MakeIndex, userItem.Index);
                }
                if (!string.IsNullOrEmpty(sItemName)) {
                    M2Share.CustomItemMgr.AddCustomItemName(userItem.MakeIndex, userItem.Index, sItemName);
                    userItem.Desc[13] = 1;
                }
                else {
                    M2Share.CustomItemMgr.DelCustomItemName(userItem.MakeIndex, userItem.Index);
                    userItem.Desc[13] = 0;
                }
                M2Share.CustomItemMgr.SaveCustomItemName();
                playObject.SendMsg(playObject, Messages.RM_SENDUSEITEMS, 0, 0, 0, 0, "");
                playObject.SendMsg(this, Messages.RM_MENU_OK, 0, playObject.ActorId, 0, 0, "");
            }
        }

        private static void DisPose(object obj) {
            obj = null;
        }
    }
}