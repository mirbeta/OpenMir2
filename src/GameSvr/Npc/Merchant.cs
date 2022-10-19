using GameSvr.Items;
using GameSvr.Maps;
using GameSvr.Player;
using GameSvr.Script;
using SystemModule;
using SystemModule.Data;
using SystemModule.Packet.ClientPackets;

namespace GameSvr.Npc
{
    /// <summary>
    /// 交易NPC类
    /// 普通商人 如：药店和杂货店都在此实现
    /// </summary>
    public class Merchant : NormNpc
    {
        public string m_sScript = string.Empty;
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
        public bool m_boCanMove = false;
        public int m_dwMoveTime = 0;
        public int m_dwMoveTick;
        /// <summary>
        /// 是否购买物品
        /// </summary>
        public bool m_boBuy;
        /// <summary>
        /// 是否交易物品
        /// </summary>
        public bool m_boSell;
        public bool m_boMakeDrug;
        public bool m_boPrices;
        public bool m_boStorage;
        public bool m_boGetback;
        public bool m_boUpgradenow;
        public bool m_boGetBackupgnow;
        public bool m_boRepair;
        public bool m_boS_repair;
        public bool m_boSendmsg = false;
        public bool m_boGetMarry;
        public bool m_boGetMaster;
        public bool m_boUseItemName;
        public bool m_boOffLineMsg = false;
        public bool m_boYBDeal = false;

        public Merchant() : base()
        {
            RaceImg = Grobal2.RCC_MERCHANT;
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
            m_boBuy = false;
            m_boSell = false;
            m_boMakeDrug = false;
            m_boPrices = false;
            m_boStorage = false;
            m_boGetback = false;
            m_boUpgradenow = false;
            m_boGetBackupgnow = false;
            m_boRepair = false;
            m_boS_repair = false;
            m_boGetMarry = false;
            m_boGetMaster = false;
            m_boUseItemName = false;
            m_dwMoveTick = HUtil32.GetTickCount();
            MapCell = CellType.Merchant;
        }
        
        public override void Run()
        {
            try
            {
                var dwCurrentTick = HUtil32.GetTickCount();
                var dwDelayTick = ProcessRefillIndex * 500;
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
                        SendRefMsg(Grobal2.RM_HIT, Direction, CurrX, CurrY, 0, "");
                    }
                }
                if (CastleMerchant && Castle != null && Castle.UnderWar)
                {
                    if (!FixedHideMode)
                    {
                        SendRefMsg(Grobal2.RM_DISAPPEAR, 0, 0, 0, 0, "");
                        FixedHideMode = true;
                    }
                }
                else
                {
                    if (FixedHideMode)
                    {
                        FixedHideMode = false;
                        SendRefMsg(Grobal2.RM_HIT, Direction, CurrX, CurrY, 0, "");
                    }
                }
                if (m_boCanMove && (HUtil32.GetTickCount() - m_dwMoveTick) > m_dwMoveTime * 1000)
                {
                    m_dwMoveTick = HUtil32.GetTickCount();
                    SendRefMsg(Grobal2.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
                    MapRandomMove(MapName, 0);
                }
            }
            catch (Exception e)
            {
                M2Share.Log.Error(e.Message);
            }
            base.Run();
        }

        protected override bool Operate(ProcessMessage ProcessMsg)
        {
            return base.Operate(ProcessMsg);
        }
        
        private void AddItemPrice(ushort nIndex, double nPrice)
        {
            var itemPrice = new ItemPrice
            {
                wIndex = nIndex,
                nPrice = nPrice
            };
            ItemPriceList.Add(itemPrice);
            M2Share.LocalDb.SaveGoodPriceRecord(this, m_sScript + '-' + MapName);
        }

        private void CheckItemPrice(ushort nIndex)
        {
            double n10;
            for (var i = 0; i < ItemPriceList.Count; i++)
            {
                var itemPrice = ItemPriceList[i];
                if (itemPrice.wIndex == nIndex)
                {
                    n10 = itemPrice.nPrice;
                    if (Math.Round(n10 * 1.1) > n10)
                    {
                        n10 = HUtil32.Round(n10 * 1.1);
                    }
                    return;
                }
            }
            var stdItem = M2Share.WorldEngine.GetStdItem(nIndex);
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
                Goods Goods;
                for (var i = 0; i < RefillGoodsList.Count; i++)
                {
                    Goods = RefillGoodsList[i];
                    if ((HUtil32.GetTickCount() - Goods.RefillTick) > (Goods.RefillTime * 60 * 1000))
                    {
                        Goods.RefillTick = HUtil32.GetTickCount();
                        nIndex = M2Share.WorldEngine.GetStdItemIdx(Goods.ItemName);
                        if (nIndex > 0)
                        {
                            IList<UserItem> refillList = GetRefillList(nIndex);
                            var nRefillCount = 0;
                            if (refillList != null)
                            {
                                nRefillCount = refillList.Count;
                            }
                            if (Goods.Count > nRefillCount)
                            {
                                CheckItemPrice(nIndex);
                                RefillGoodsItems(ref refillList, Goods.ItemName, Goods.Count - nRefillCount);
                                M2Share.LocalDb.SaveGoodRecord(this, m_sScript + '-' + MapName);
                                M2Share.LocalDb.SaveGoodPriceRecord(this, m_sScript + '-' + MapName);
                            }
                            if (Goods.Count < nRefillCount)
                            {
                                RefillDelReFillItem(ref refillList, nRefillCount - Goods.Count);
                                M2Share.LocalDb.SaveGoodRecord(this, m_sScript + '-' + MapName);
                                M2Share.LocalDb.SaveGoodPriceRecord(this, m_sScript + '-' + MapName);
                            }
                        }
                    }
                }
                for (var i = 0; i < GoodsList.Count; i++)
                {
                    IList<UserItem> RefillList20 = GoodsList[i];
                    if (RefillList20.Count > 1000)
                    {
                        var bo21 = false;
                        for (var j = 0; j < RefillGoodsList.Count; j++)
                        {
                            Goods = RefillGoodsList[j];
                            nIndex = M2Share.WorldEngine.GetStdItemIdx(Goods.ItemName);
                            if (RefillList20[0].Index == nIndex)
                            {
                                bo21 = true;
                                break;
                            }
                        }
                        if (!bo21)
                        {
                            RefillDelReFillItem(ref RefillList20, RefillList20.Count - 1000);
                        }
                        else
                        {
                            RefillDelReFillItem(ref RefillList20, RefillList20.Count - 5000);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                M2Share.Log.Error(Format(sExceptionMsg, ChrName, CurrX, CurrY, e.Message, ScriptConst.nCHECK));
            }
        }

        private IList<UserItem> GetRefillList(int nIndex)
        {
            if (nIndex < 0)
            {
                return null;
            }
            for (var i = 0; i < GoodsList.Count; i++)
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

        private void RefillGoodsItems(ref IList<UserItem> List, string sItemName, int nInt)
        {
            if (List == null)
            {
                List = new List<UserItem>();
                GoodsList.Add(List);
            }
            for (var i = 0; i < nInt; i++)
            {
                var goodItem = new UserItem();
                if (M2Share.WorldEngine.CopyToUserItemFromName(sItemName, ref goodItem))
                {
                    List.Insert(0, goodItem);
                }
                else
                {
                    Dispose(goodItem);
                }
            }
        }

        private void RefillDelReFillItem(ref IList<UserItem> List, int nInt)
        {
            for (var i = List.Count - 1; i >= 0; i--)
            {
                if (nInt <= 0)
                {
                    break;
                }
                Dispose(List[i]);
                List.RemoveAt(i);
                nInt -= 1;
            }
        }
        
        private bool CheckItemType(int nStdMode)
        {
            var result = false;
            for (var i = 0; i < ItemTypeList.Count; i++)
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
            for (var i = 0; i < ItemPriceList.Count; i++)
            {
                var ItemPrice = ItemPriceList[i];
                if (ItemPrice.wIndex == nIndex)
                {
                    result = ItemPrice.nPrice;
                    break;
                }
            }
            if (result < 0)
            {
                var StdItem = M2Share.WorldEngine.GetStdItem(nIndex);
                if (StdItem != null)
                {
                    if (CheckItemType(StdItem.StdMode))
                    {
                        result = StdItem.Price;
                    }
                }
            }
            return result;
        }

        private void UpgradeWaponAddValue(PlayObject User, IList<UserItem> ItemList, ref byte btDc, ref byte btSc, ref byte btMc, ref byte btDura)
        {
            ClientItem StdItem80 = null;
            IList<DeleteItem> DelItemList = null;
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
            IList<double> DuraList = new List<double>();
            for (var i = ItemList.Count - 1; i >= 0; i--)
            {
                var UserItem = ItemList[i];
                if (M2Share.WorldEngine.GetStdItemName(UserItem.Index) == M2Share.Config.BlackStone)
                {
                    DuraList.Add(Math.Round(UserItem.Dura / 1.0e3));
                    if (DelItemList == null)
                    {
                        DelItemList = new List<DeleteItem>();
                    }
                    DelItemList.Add(new DeleteItem()
                    {
                        MakeIndex = UserItem.MakeIndex,
                        ItemName = M2Share.Config.BlackStone
                    });
                    DisPose(UserItem);
                    ItemList.RemoveAt(i);
                }
                else
                {
                    if (M2Share.IsAccessory(UserItem.Index))
                    {
                        var StdItem = M2Share.WorldEngine.GetStdItem(UserItem.Index);
                        if (StdItem != null)
                        {
                            StdItem.GetUpgradeStdItem(UserItem, ref StdItem80);
                            //StdItem.GetItemAddValue(UserItem, ref StdItem80);
                            nDc = 0;
                            nSc = 0;
                            nMc = 0;
                            switch (StdItem80.Item.StdMode)
                            {
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
                            if (DelItemList == null)
                            {
                                DelItemList = new List<DeleteItem>();
                            }
                            DelItemList.Add(new DeleteItem()
                            {
                                ItemName = StdItem.Name,
                                MakeIndex = UserItem.MakeIndex
                            });
                            if (StdItem.NeedIdentify == 1)
                            {
                                M2Share.EventSource.AddEventLog(26, User.MapName + "\t" + User.CurrX + "\t" + User.CurrY + "\t" + User.ChrName + "\t" + StdItem.Name + "\t" + UserItem.MakeIndex + "\t" + '1' + "\t" + '0');
                            }
                            DisPose(UserItem);
                            ItemList.RemoveAt(i);
                        }
                    }
                }
            }
            for (var i = 0; i < DuraList.Count; i++)
            {
                for (var j = DuraList.Count - 1; j > i; j--)
                {
                    if (DuraList[j] > DuraList[j - 1])
                    {
                        DuraList.Reverse();
                    }
                }
            }
            for (var i = 0; i < DuraList.Count; i++)
            {
                nDura = nDura + (int)DuraList[i];
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
            if (DelItemList != null)
            {
                var objectId = HUtil32.Sequence();
                M2Share.ActorMgr.AddOhter(objectId, DelItemList);
                User.SendMsg(this, Grobal2.RM_SENDDELITEMLIST, 0, objectId, 0, 0, "");
            }
            if (DuraList != null)
            {
            }
        }

        private void UpgradeWapon(PlayObject User)
        {
            var bo0D = false;
            WeaponUpgradeInfo upgradeInfo;
            for (var i = 0; i < UpgradeWeaponList.Count; i++)
            {
                upgradeInfo = UpgradeWeaponList[i];
                if (upgradeInfo.UserName == User.ChrName)
                {
                    GotoLable(User, ScriptConst.sUPGRADEING, false);
                    return;
                }
            }
            if (User.UseItems[Grobal2.U_WEAPON] != null && User.UseItems[Grobal2.U_WEAPON].Index != 0 && User.Gold >= M2Share.Config.UpgradeWeaponPrice
                && User.CheckItems(M2Share.Config.BlackStone) != null)
            {
                User.DecGold(M2Share.Config.UpgradeWeaponPrice);
                if (CastleMerchant || M2Share.Config.GetAllNpcTax)
                {
                    if (Castle != null)
                    {
                        Castle.IncRateGold(M2Share.Config.UpgradeWeaponPrice);
                    }
                    else if (M2Share.Config.GetAllNpcTax)
                    {
                        M2Share.CastleMgr.IncRateGold(M2Share.Config.UpgradeWeaponPrice);
                    }
                }
                User.GoldChanged();
                upgradeInfo = new WeaponUpgradeInfo
                {
                    UserName = User.ChrName,
                    UserItem = User.UseItems[Grobal2.U_WEAPON]
                };
                var StdItem = M2Share.WorldEngine.GetStdItem(User.UseItems[Grobal2.U_WEAPON].Index);
                if (StdItem.NeedIdentify == 1)
                {
                    M2Share.EventSource.AddEventLog(25, User.MapName + "\t" + User.CurrX + "\t" + User.CurrY + "\t" + User.ChrName + "\t" + StdItem.Name + "\t" + User.UseItems[Grobal2.U_WEAPON].MakeIndex + "\t" + '1' + "\t" + '0');
                }
                User.SendDelItems(User.UseItems[Grobal2.U_WEAPON]);
                User.UseItems[Grobal2.U_WEAPON].Index = 0;
                User.RecalcAbilitys();
                User.FeatureChanged();
                User.SendMsg(User, Grobal2.RM_ABILITY, 0, 0, 0, 0, "");
                UpgradeWaponAddValue(User, User.ItemList, ref upgradeInfo.Dc, ref upgradeInfo.Sc, ref upgradeInfo.Mc, ref upgradeInfo.Dura);
                upgradeInfo.dtTime = DateTime.Now;
                upgradeInfo.GetBackTick = HUtil32.GetTickCount();
                UpgradeWeaponList.Add(upgradeInfo);
                SaveUpgradingList();
                bo0D = true;
            }
            if (bo0D)
            {
                GotoLable(User, ScriptConst.sUPGRADEOK, false);
            }
            else
            {
                GotoLable(User, ScriptConst.sUPGRADEFAIL, false);
            }
        }

        /// <summary>
        /// 取回升级武器
        /// </summary>
        /// <param name="User"></param>
        private void GetBackupgWeapon(PlayObject User)
        {
            WeaponUpgradeInfo UpgradeInfo = null;
            var n18 = 0;
            if (!User.IsEnoughBag())
            {
                GotoLable(User, ScriptConst.sGETBACKUPGFULL, false);
                return;
            }
            for (var i = 0; i < UpgradeWeaponList.Count; i++)
            {
                if (string.Compare(UpgradeWeaponList[i].UserName, User.ChrName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    n18 = 1;
                    if (((HUtil32.GetTickCount() - UpgradeWeaponList[i].GetBackTick) > M2Share.Config.UPgradeWeaponGetBackTime) || User.Permission >= 4)
                    {
                        UpgradeInfo = UpgradeWeaponList[i];
                        UpgradeWeaponList.RemoveAt(i);
                        SaveUpgradingList();
                        n18 = 2;
                        break;
                    }
                }
            }
            if (UpgradeInfo != null)
            {
                if (HUtil32.RangeInDefined(UpgradeInfo.Dura, 0, 8))
                {
                    if (UpgradeInfo.UserItem.DuraMax > 3000)
                    {
                        UpgradeInfo.UserItem.DuraMax -= 3000;
                    }
                    else
                    {
                        UpgradeInfo.UserItem.DuraMax = (ushort)(UpgradeInfo.UserItem.DuraMax >> 1);
                    }
                    if (UpgradeInfo.UserItem.Dura > UpgradeInfo.UserItem.DuraMax)
                    {
                        UpgradeInfo.UserItem.Dura = UpgradeInfo.UserItem.DuraMax;
                    }
                }
                else if (HUtil32.RangeInDefined(UpgradeInfo.Dura, 9, 15))
                {
                    if (M2Share.RandomNumber.Random(UpgradeInfo.Dura) < 6)
                    {
                        if (UpgradeInfo.UserItem.DuraMax > 1000)
                        {
                            UpgradeInfo.UserItem.DuraMax -= 1000;
                        }
                        if (UpgradeInfo.UserItem.Dura > UpgradeInfo.UserItem.DuraMax)
                        {
                            UpgradeInfo.UserItem.Dura = UpgradeInfo.UserItem.DuraMax;
                        }
                    }
                }
                else if (HUtil32.RangeInDefined(UpgradeInfo.Dura, 18, 255))
                {
                    var r = M2Share.RandomNumber.Random(UpgradeInfo.Dura - 18);
                    if (HUtil32.RangeInDefined(r, 1, 4))
                    {
                        UpgradeInfo.UserItem.DuraMax += 1000;
                    }
                    else if (HUtil32.RangeInDefined(r, 5, 7))
                    {
                        UpgradeInfo.UserItem.DuraMax += 2000;
                    }
                    else if (HUtil32.RangeInDefined(r, 8, 255))
                    {
                        UpgradeInfo.UserItem.DuraMax += 4000;
                    }
                }
                int n1C;
                if (UpgradeInfo.Dc == UpgradeInfo.Mc && UpgradeInfo.Mc == UpgradeInfo.Sc)
                {
                    n1C = M2Share.RandomNumber.Random(3);
                }
                else
                {
                    n1C = -1;
                }
                int n90;
                int n10;
                if (UpgradeInfo.Dc >= UpgradeInfo.Mc && (UpgradeInfo.Dc >= UpgradeInfo.Sc) || (n1C == 0))
                {
                    n90 = HUtil32._MIN(11, UpgradeInfo.Dc);
                    n10 = HUtil32._MIN(85, (n90 << 3 - n90) + 10 + UpgradeInfo.UserItem.Desc[3] - UpgradeInfo.UserItem.Desc[4] + User.BodyLuckLevel);
                    if (M2Share.RandomNumber.Random(M2Share.Config.UpgradeWeaponDCRate) < n10)
                    {
                        UpgradeInfo.UserItem.Desc[ItemAttr.WeaponUpgrade] = 10;
                        if (n10 > 63 && M2Share.RandomNumber.Random(M2Share.Config.UpgradeWeaponDCTwoPointRate) == 0)
                        {
                            UpgradeInfo.UserItem.Desc[ItemAttr.WeaponUpgrade] = 11;
                        }
                        if (n10 > 79 && M2Share.RandomNumber.Random(M2Share.Config.UpgradeWeaponDCThreePointRate) == 0)
                        {
                            UpgradeInfo.UserItem.Desc[ItemAttr.WeaponUpgrade] = 12;
                        }
                    }
                    else
                    {
                        UpgradeInfo.UserItem.Desc[ItemAttr.WeaponUpgrade] = 1;
                    }
                }
                if (UpgradeInfo.Mc >= UpgradeInfo.Dc && UpgradeInfo.Mc >= UpgradeInfo.Sc || n1C == 1)
                {
                    n90 = HUtil32._MIN(11, UpgradeInfo.Mc);
                    n10 = HUtil32._MIN(85, (n90 << 3 - n90) + 10 + UpgradeInfo.UserItem.Desc[3] - UpgradeInfo.UserItem.Desc[4] + User.BodyLuckLevel);
                    if (M2Share.RandomNumber.Random(M2Share.Config.UpgradeWeaponMCRate) < n10)
                    {
                        UpgradeInfo.UserItem.Desc[ItemAttr.WeaponUpgrade] = 20;
                        if (n10 > 63 && M2Share.RandomNumber.Random(M2Share.Config.UpgradeWeaponMCTwoPointRate) == 0)
                        {
                            UpgradeInfo.UserItem.Desc[ItemAttr.WeaponUpgrade] = 21;
                        }
                        if (n10 > 79 && M2Share.RandomNumber.Random(M2Share.Config.UpgradeWeaponMCThreePointRate) == 0)
                        {
                            UpgradeInfo.UserItem.Desc[ItemAttr.WeaponUpgrade] = 22;
                        }
                    }
                    else
                    {
                        UpgradeInfo.UserItem.Desc[ItemAttr.WeaponUpgrade] = 1;
                    }
                }
                if (UpgradeInfo.Sc >= UpgradeInfo.Mc && UpgradeInfo.Sc >= UpgradeInfo.Dc || n1C == 2)
                {
                    n90 = HUtil32._MIN(11, UpgradeInfo.Mc);
                    n10 = HUtil32._MIN(85, (n90 << 3 - n90) + 10 + UpgradeInfo.UserItem.Desc[3] - UpgradeInfo.UserItem.Desc[4] + User.BodyLuckLevel);
                    if (M2Share.RandomNumber.Random(M2Share.Config.UpgradeWeaponSCRate) < n10)
                    {
                        UpgradeInfo.UserItem.Desc[ItemAttr.WeaponUpgrade] = 30;
                        if (n10 > 63 && M2Share.RandomNumber.Random(M2Share.Config.UpgradeWeaponSCTwoPointRate) == 0)
                        {
                            UpgradeInfo.UserItem.Desc[ItemAttr.WeaponUpgrade] = 31;
                        }
                        if (n10 > 79 && M2Share.RandomNumber.Random(M2Share.Config.UpgradeWeaponSCThreePointRate) == 0)
                        {
                            UpgradeInfo.UserItem.Desc[ItemAttr.WeaponUpgrade] = 32;
                        }
                    }
                    else
                    {
                        UpgradeInfo.UserItem.Desc[ItemAttr.WeaponUpgrade] = 1;
                    }
                }
                var UserItem = UpgradeInfo.UserItem;
                var StdItem = M2Share.WorldEngine.GetStdItem(UserItem.Index);
                if (StdItem.NeedIdentify == 1)
                {
                    M2Share.EventSource.AddEventLog(24, User.MapName + "\t" + User.CurrX + "\t" + User.CurrY + "\t" + User.ChrName + "\t" + StdItem.Name + "\t" + UserItem.MakeIndex + "\t" + '1' + "\t" + '0');
                }
                User.AddItemToBag(UserItem);
                User.SendAddItem(UserItem);
                DisPose(UpgradeInfo);
            }
            switch (n18)
            {
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
        private int GetUserPrice(PlayObject PlayObject, double nPrice)
        {
            int result;
            if (CastleMerchant)
            {
                if (Castle != null && Castle.IsMasterGuild(PlayObject.MyGuild)) //沙巴克成员修复物品打折
                {
                    var n14 = HUtil32._MAX(60, HUtil32.Round(PriceRate * (M2Share.Config.CastleMemberPriceRate / 100)));//80%
                    result = HUtil32.Round(nPrice / 100 * n14);
                }
                else
                {
                    result = HUtil32.Round(nPrice / 100 * PriceRate);
                }
            }
            else
            {
                result = HUtil32.Round(nPrice / 100 * PriceRate);
            }
            return result;
        }
        
        public override void UserSelect(PlayObject PlayObject, string sData)
        {
            var sLabel = string.Empty;
            const string sExceptionMsg = "[Exception] TMerchant::UserSelect... Data: {0}";
            base.UserSelect(PlayObject, sData);
            if (this is not Merchant)// 如果类名不是 TMerchant 则不执行以下处理函数
            {
                return;
            }
            try
            {
                if (!CastleMerchant || !(Castle != null && Castle.UnderWar))
                {
                    if (!PlayObject.Death && sData != "" && sData[0] == '@')
                    {
                        var sMsg = HUtil32.GetValidStr3(sData, ref sLabel, new char[] { '\r' });
                        PlayObject.ScriptLable = sData;
                        var boCanJmp = PlayObject.LableIsCanJmp(sLabel);
                        if (string.Compare(sLabel, ScriptConst.sSL_SENDMSG, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (sMsg == "")
                            {
                                return;
                            }
                        }
                        GotoLable(PlayObject, sLabel, !boCanJmp);
                        if (!boCanJmp)
                        {
                            return;
                        }
                        if (string.Compare(sLabel, ScriptConst.sOFFLINEMSG, StringComparison.OrdinalIgnoreCase) == 0)// 增加挂机
                        {
                            if (m_boOffLineMsg)
                            {
                                SetOffLineMsg(PlayObject, sMsg);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptConst.sSL_SENDMSG, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (m_boSendmsg)
                            {
                                SendCustemMsg(PlayObject, sMsg);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptConst.sSUPERREPAIR, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (m_boS_repair)
                            {
                                UserSelectSuperRepairItem(PlayObject);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptConst.sBUY, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (m_boBuy)
                            {
                                UserSelectBuyItem(PlayObject, 0);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptConst.sSELL, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (m_boSell)
                            {
                                UserSelectSellItem(PlayObject);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptConst.sREPAIR, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (m_boRepair)
                            {
                                UserSelectRepairItem(PlayObject);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptConst.sMAKEDURG, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (m_boMakeDrug)
                            {
                                UserSelectMakeDurg(PlayObject);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptConst.sPRICES, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (m_boPrices)
                            {
                                UserSelectItemPrices(PlayObject);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptConst.sSTORAGE, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (m_boStorage)
                            {
                                UserSelectStorage(PlayObject);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptConst.sGETBACK, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (m_boGetback)
                            {
                                UserSelectGetBack(PlayObject);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptConst.sUPGRADENOW, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (m_boUpgradenow)
                            {
                                UpgradeWapon(PlayObject);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptConst.sGETBACKUPGNOW, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (m_boGetBackupgnow)
                            {
                                GetBackupgWeapon(PlayObject);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptConst.sGETMARRY, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (m_boGetMarry)
                            {
                                GetBackupgWeapon(PlayObject);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptConst.sGETMASTER, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (m_boGetMaster)
                            {
                                GetBackupgWeapon(PlayObject);
                            }
                        }
                        else if (HUtil32.CompareLStr(sLabel, ScriptConst.sUSEITEMNAME))
                        {
                            if (m_boUseItemName)
                            {
                                ChangeUseItemName(PlayObject, sLabel, sMsg);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptConst.sEXIT, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            PlayObject.SendMsg(this, Grobal2.RM_MERCHANTDLGCLOSE, 0, ActorId, 0, 0, "");
                        }
                        else if (string.Compare(sLabel, ScriptConst.sBACK, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (PlayObject.m_sScriptGoBackLable == "")
                            {
                                PlayObject.m_sScriptGoBackLable = ScriptConst.sMAIN;
                            }
                            GotoLable(PlayObject, PlayObject.m_sScriptGoBackLable, false);
                        }
                        else if (string.Compare(sLabel, ScriptConst.sDealYBme, StringComparison.OrdinalIgnoreCase) == 0) // 元宝寄售:出售物品 
                        {
                            if (m_boYBDeal)
                            {
                                UserSelectOpenDealOffForm(PlayObject); // 打开出售物品窗口
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                M2Share.Log.Error(Format(sExceptionMsg, sData));
                M2Share.Log.Error(ex.StackTrace);
            }
        }
        
        /// <summary>
        /// 特殊修理物品
        /// </summary>
        private void UserSelectSuperRepairItem(PlayObject User)
        {
            User.SendMsg(this, Grobal2.RM_SENDUSERSREPAIR, 0, ActorId, 0, 0, "");
        }

        /// <summary>
        /// 物品详情列表
        /// </summary>
        private void UserSelectBuyItem(PlayObject User, int nInt)
        {
            var sSendMsg = string.Empty;
            var n10 = 0;
            for (var i = 0; i < GoodsList.Count; i++)
            {
                var goods = GoodsList[i];
                var userItem = goods[0];
                var stdItem = M2Share.WorldEngine.GetStdItem(userItem.Index);
                if (stdItem != null)
                {
                    var sName = CustomItem.GetItemName(userItem);
                    var nPrice = GetUserPrice(User, GetItemPrice(userItem.Index));
                    var nStock = goods.Count;
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
            User.SendMsg(this, Grobal2.RM_SENDGOODSLIST, 0, ActorId, n10, 0, sSendMsg);
        }

        private void UserSelectSellItem(PlayObject User)
        {
            User.SendMsg(this, Grobal2.RM_SENDUSERSELL, 0, ActorId, 0, 0, "");
        }

        private void UserSelectRepairItem(PlayObject User)
        {
            User.SendMsg(this, Grobal2.RM_SENDUSERREPAIR, 0, ActorId, 0, 0, "");
        }

        private void UserSelectMakeDurg(PlayObject User)
        {
            var sSendMsg = string.Empty;
            for (var i = 0; i < GoodsList.Count; i++)
            {
                IList<UserItem> goods = GoodsList[i];
                var StdItem = M2Share.WorldEngine.GetStdItem(goods[0].Index);
                if (StdItem != null)
                {
                    sSendMsg = sSendMsg + StdItem.Name + '/' + 0 + '/' + M2Share.Config.MakeDurgPrice + '/' + 1 + '/';
                }
            }
            if (!string.IsNullOrEmpty(sSendMsg))
            {
                User.SendMsg(this, Grobal2.RM_USERMAKEDRUGITEMLIST, 0, ActorId, 0, 0, sSendMsg);
            }
        }

        private void UserSelectItemPrices(PlayObject User)
        {
            
        }

        private void UserSelectStorage(PlayObject User)
        {
            User.SendMsg(this, Grobal2.RM_USERSTORAGEITEM, 0, ActorId, 0, 0, "");
        }

        private void UserSelectGetBack(PlayObject User)
        {
            User.SendMsg(this, Grobal2.RM_USERGETBACKITEM, 0, ActorId, 0, 0, "");
        }

        /// <summary>
        /// 打开出售物品窗口
        /// </summary>
        /// <param name="User"></param>
        private void UserSelectOpenDealOffForm(PlayObject User)
        {
            if (User.bo_YBDEAL)
            {
                if (!User.SellOffInTime(0))
                {
                    User.SendMsg(this, Grobal2.RM_SENDDEALOFFFORM, 0, ActorId, 0, 0, "");
                    User.GetBackSellOffItems();
                }
                else
                {
                    User.SendMsg(this, Grobal2.RM_MERCHANTSAY, 0, 0, 0, 0, ChrName + "/您还有元宝服务正在进行!!\\ \\<返回/@main>");
                }
            }
            else
            {
                User.SendMsg(this, Grobal2.RM_MERCHANTSAY, 0, 0, 0, 0, ChrName + "/您未开通元宝服务,请先开通元宝服务!!\\ \\<返回/@main>");
            }
        }

        public void LoadNPCData()
        {
            var sFile = m_sScript + '-' + MapName;
            M2Share.LocalDb.LoadGoodRecord(this, sFile);
            M2Share.LocalDb.LoadGoodPriceRecord(this, sFile);
            LoadUpgradeList();
        }

        private void SaveNPCData()
        {
            var sFile = m_sScript + '-' + MapName;
            M2Share.LocalDb.SaveGoodRecord(this, sFile);
            M2Share.LocalDb.SaveGoodPriceRecord(this, sFile);
        }

        /// <summary>
        /// 清理武器升级过期数据
        /// </summary>
        private void ClearExpreUpgradeListData()
        {
            WeaponUpgradeInfo UpgradeInfo;
            for (var i = UpgradeWeaponList.Count - 1; i >= 0; i--)
            {
                UpgradeInfo = UpgradeWeaponList[i];
                if ((int)Math.Round(DateTime.Now.ToOADate() - UpgradeInfo.dtTime.ToOADate()) >= M2Share.Config.ClearExpireUpgradeWeaponDays)
                {
                    Dispose(UpgradeInfo);
                    UpgradeWeaponList.RemoveAt(i);
                }
            }
        }

        public void LoadMerchantScript()
        {
            ItemTypeList.Clear();
            m_sPath = ScriptConst.sMarket_Def;
            var sC = m_sScript + '-' + MapName;
            M2Share.ScriptSystem.LoadScriptFile(this, ScriptConst.sMarket_Def, sC, true);
        }

        public override void Click(PlayObject PlayObject)
        {
            base.Click(PlayObject);
        }

        protected override void GetVariableText(PlayObject PlayObject, ref string sMsg, string sVariable)
        {
            string sText;
            base.GetVariableText(PlayObject, ref sMsg, sVariable);
            switch (sVariable)
            {
                case "$PRICERATE":
                    sText = PriceRate.ToString();
                    sMsg = ReplaceVariableText(sMsg, "<$PRICERATE>", sText);
                    break;
                case "$UPGRADEWEAPONFEE":
                    sText = M2Share.Config.UpgradeWeaponPrice.ToString();
                    sMsg = ReplaceVariableText(sMsg, "<$UPGRADEWEAPONFEE>", sText);
                    break;
                case "$USERWEAPON":
                    {
                        if (PlayObject.UseItems[Grobal2.U_WEAPON].Index != 0)
                        {
                            sText = M2Share.WorldEngine.GetStdItemName(PlayObject.UseItems[Grobal2.U_WEAPON].Index);
                        }
                        else
                        {
                            sText = "无";
                        }
                        sMsg = ReplaceVariableText(sMsg, "<$USERWEAPON>", sText);
                        break;
                    }
            }
        }

        private double GetUserItemPrice(UserItem UserItem)
        {
            double result;
            double n20;
            int nC;
            int n14;
            var itemPrice = GetItemPrice(UserItem.Index);
            if (itemPrice > 0)
            {
                var StdItem = M2Share.WorldEngine.GetStdItem(UserItem.Index);
                if (StdItem != null && StdItem.StdMode > 4 && StdItem.DuraMax > 0 && UserItem.DuraMax > 0)
                {
                    if (StdItem.StdMode == 40)// 肉
                    {
                        if (UserItem.Dura <= UserItem.DuraMax)
                        {
                            n20 = itemPrice / 2.0 / UserItem.DuraMax * (UserItem.DuraMax - UserItem.Dura);
                            itemPrice = HUtil32._MAX(2, HUtil32.Round(itemPrice - n20));
                        }
                        else
                        {
                            itemPrice = itemPrice + HUtil32.Round(itemPrice / UserItem.DuraMax * 2.0 * (UserItem.DuraMax - UserItem.Dura));
                        }
                    }
                    if (StdItem.StdMode == 43)
                    {
                        if (UserItem.DuraMax < 10000)
                        {
                            UserItem.DuraMax = 10000;
                        }
                        if (UserItem.Dura <= UserItem.DuraMax)
                        {
                            n20 = itemPrice / 2.0 / UserItem.DuraMax * (UserItem.DuraMax - UserItem.Dura);
                            itemPrice = HUtil32._MAX(2, HUtil32.Round(itemPrice - n20));
                        }
                        else
                        {
                            itemPrice = itemPrice + HUtil32.Round(itemPrice / UserItem.DuraMax * 1.3 * (UserItem.DuraMax - UserItem.Dura));
                        }
                    }
                    if (StdItem.StdMode > 4)
                    {
                        n14 = 0;
                        nC = 0;
                        while (true)
                        {
                            if (StdItem.StdMode == 5 || StdItem.StdMode == 6)
                            {
                                if (nC != 4 || nC != 9)
                                {
                                    if (nC == 6)
                                    {
                                        if (UserItem.Desc[nC] > 10)
                                        {
                                            n14 = n14 + (UserItem.Desc[nC] - 10) * 2;
                                        }
                                    }
                                    else
                                    {
                                        n14 = n14 + UserItem.Desc[nC];
                                    }
                                }
                            }
                            else
                            {
                                n14 += UserItem.Desc[nC];
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
                        itemPrice = HUtil32.Round(itemPrice / StdItem.DuraMax * UserItem.DuraMax);
                        n20 = itemPrice / 2.0 / UserItem.DuraMax * (UserItem.DuraMax - UserItem.Dura);
                        itemPrice = HUtil32._MAX(2, HUtil32.Round(itemPrice - n20));
                    }
                }
            }
            result = itemPrice;
            return result;
        }

        public void ClientBuyItem(PlayObject PlayObject, string sItemName, int nInt)
        {
            IList<UserItem> List20;
            UserItem UserItem;
            int nPrice;
            string sUserItemName;
            var bo29 = false;
            var n1C = 1;
            for (var i = 0; i < GoodsList.Count; i++)
            {
                if (bo29)
                {
                    break;
                }
                List20 = GoodsList[i];
                UserItem = List20[0];
                var StdItem = M2Share.WorldEngine.GetStdItem(UserItem.Index);
                if (StdItem != null)
                {
                    sUserItemName = CustomItem.GetItemName(UserItem);
                    if (PlayObject.IsAddWeightAvailable(StdItem.Weight))
                    {
                        if (sUserItemName == sItemName)
                        {
                            for (var j = 0; j < List20.Count; j++)
                            {
                                UserItem = List20[j];
                                if (StdItem.StdMode <= 4 || StdItem.StdMode == 42 || StdItem.StdMode == 31 || UserItem.MakeIndex == nInt)
                                {
                                    nPrice = GetUserPrice(PlayObject, GetUserItemPrice(UserItem));
                                    if (PlayObject.Gold >= nPrice && nPrice > 0)
                                    {
                                        if (PlayObject.AddItemToBag(UserItem))
                                        {
                                            PlayObject.Gold -= nPrice;
                                            if (CastleMerchant || M2Share.Config.GetAllNpcTax)
                                            {
                                                if (Castle != null)
                                                {
                                                    Castle.IncRateGold(nPrice);
                                                }
                                                else if (M2Share.Config.GetAllNpcTax)
                                                {
                                                    M2Share.CastleMgr.IncRateGold(M2Share.Config.UpgradeWeaponPrice);
                                                }
                                            }
                                            PlayObject.SendAddItem(UserItem);
                                            if (StdItem.NeedIdentify == 1)
                                            {
                                                M2Share.EventSource.AddEventLog(9, PlayObject.MapName + "\t" + PlayObject.CurrX + "\t" + PlayObject.CurrY + "\t" + PlayObject.ChrName + "\t" + StdItem.Name + "\t" + UserItem.MakeIndex + "\t" + '1' + "\t" + ChrName);
                                            }
                                            List20.RemoveAt(j);
                                            if (List20.Count <= 0)
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
                PlayObject.SendMsg(this, Grobal2.RM_BUYITEM_SUCCESS, 0, PlayObject.Gold, nInt, 0, "");
            }
            else
            {
                PlayObject.SendMsg(this, Grobal2.RM_BUYITEM_FAIL, 0, n1C, 0, 0, "");
            }
        }

        public void ClientGetDetailGoodsList(PlayObject PlayObject, string sItemName, int nInt)
        {
            IList<UserItem> List20;
            var sSendMsg = string.Empty;
            var nItemCount = 0;
            for (var i = 0; i < GoodsList.Count; i++)
            {
                List20 = GoodsList[i];
                if (List20.Count <= 0)
                {
                    continue;
                }
                var UserItem = List20[0];
                var Item = M2Share.WorldEngine.GetStdItem(UserItem.Index);
                if (Item != null && Item.Name == sItemName)
                {
                    if (List20.Count - 1 < nInt)
                    {
                        nInt = HUtil32._MAX(0, List20.Count - 10);
                    }
                    for (var j = List20.Count - 1; j >= 0; j--)
                    {
                        UserItem = List20[j];
                        var clientItem = new ClientItem();
                        Item.GetUpgradeStdItem(UserItem, ref clientItem);
                        //Item.GetItemAddValue(UserItem, ref ClientItem.Item);
                        clientItem.Dura = UserItem.Dura;
                        clientItem.DuraMax = (ushort)GetUserPrice(PlayObject, GetUserItemPrice(UserItem));
                        clientItem.MakeIndex = UserItem.MakeIndex;
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
            PlayObject.SendMsg(this, Grobal2.RM_SENDDETAILGOODSLIST, 0, ActorId, nItemCount, nInt, sSendMsg);
        }

        public void ClientQuerySellPrice(PlayObject PlayObject, UserItem UserItem)
        {
            var nC = GetSellItemPrice(GetUserItemPrice(UserItem));
            if (nC >= 0)
            {
                PlayObject.SendMsg(this, Grobal2.RM_SENDBUYPRICE, 0, nC, 0, 0, "");
            }
            else
            {
                PlayObject.SendMsg(this, Grobal2.RM_SENDBUYPRICE, 0, 0, 0, 0, "");
            }
        }

        private int GetSellItemPrice(double nPrice)
        {
            return HUtil32.Round(nPrice / 2.0);
        }

        private bool ClientSellItem_sub_4A1C84(UserItem UserItem)
        {
            var result = true;
            var StdItem = M2Share.WorldEngine.GetStdItem(UserItem.Index);
            if (StdItem != null && (StdItem.StdMode == 25 || StdItem.StdMode == 30))
            {
                if (UserItem.Dura < 4000)
                {
                    result = false;
                }
            }
            return result;
        }

        public bool ClientSellItem(PlayObject PlayObject, UserItem UserItem)
        {
            var result = false;
            var nPrice = GetSellItemPrice(GetUserItemPrice(UserItem));
            if (nPrice > 0 && ClientSellItem_sub_4A1C84(UserItem))
            {
                if (PlayObject.IncGold(nPrice))
                {
                    if (CastleMerchant || M2Share.Config.GetAllNpcTax)
                    {
                        if (Castle != null)
                        {
                            Castle.IncRateGold(nPrice);
                        }
                        else if (M2Share.Config.GetAllNpcTax)
                        {
                            M2Share.CastleMgr.IncRateGold(M2Share.Config.UpgradeWeaponPrice);
                        }
                    }
                    PlayObject.SendMsg(this, Grobal2.RM_USERSELLITEM_OK, 0, PlayObject.Gold, 0, 0, "");
                    AddItemToGoodsList(UserItem);
                    var StdItem = M2Share.WorldEngine.GetStdItem(UserItem.Index);
                    if (StdItem.NeedIdentify == 1)
                    {
                        M2Share.EventSource.AddEventLog(10, PlayObject.MapName + "\t" + PlayObject.CurrX + "\t" + PlayObject.CurrY + "\t" + PlayObject.ChrName + "\t" + StdItem.Name + "\t" + UserItem.MakeIndex + "\t" + '1' + "\t" + ChrName);
                    }
                    result = true;
                }
                else
                {
                    PlayObject.SendMsg(this, Grobal2.RM_USERSELLITEM_FAIL, 0, 0, 0, 0, "");
                }
            }
            else
            {
                PlayObject.SendMsg(this, Grobal2.RM_USERSELLITEM_FAIL, 0, 0, 0, 0, "");
            }
            return result;
        }

        private bool AddItemToGoodsList(UserItem UserItem)
        {
            if (UserItem.Dura <= 0)
            {
                return false;
            }
            var ItemList = GetRefillList(UserItem.Index);
            if (ItemList == null)
            {
                ItemList = new List<UserItem>();
                GoodsList.Add(ItemList);
            }
            ItemList.Insert(0, UserItem);
            return true;
        }

        private bool ClientMakeDrugItem_sub_4A28FC(PlayObject PlayObject, string sItemName)
        {
            var result = false;
            IList<MakeItem> List10 = M2Share.GetMakeItemInfo(sItemName);
            IList<DeleteItem> List28;
            if (List10 == null)
            {
                return result;
            }
            result = true;
            string s20;
            int n1C;
            for (var i = 0; i < List10.Count; i++)
            {
                s20 = List10[i].ItemName;
                n1C = List10[i].ItemCount;
                for (var j = 0; j < PlayObject.ItemList.Count; j++)
                {
                    if (M2Share.WorldEngine.GetStdItemName(PlayObject.ItemList[j].Index) == s20)
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
                List28 = null;
                for (var i = 0; i < List10.Count; i++)
                {
                    s20 = List10[i].ItemName;
                    n1C = List10[i].ItemCount;
                    for (var j = PlayObject.ItemList.Count - 1; j >= 0; j--)
                    {
                        if (n1C <= 0)
                        {
                            break;
                        }
                        var UserItem = PlayObject.ItemList[j];
                        if (M2Share.WorldEngine.GetStdItemName(UserItem.Index) == s20)
                        {
                            if (List28 == null)
                            {
                                List28 = new List<DeleteItem>();
                            }
                            List28.Add(new DeleteItem()
                            {
                                ItemName = s20,
                                MakeIndex = UserItem.MakeIndex
                            });
                            Dispose(UserItem);
                            PlayObject.ItemList.RemoveAt(j);
                            n1C -= 1;
                        }
                    }
                }
                if (List28 != null)
                {
                    var ObjectId = HUtil32.Sequence();
                    M2Share.ActorMgr.AddOhter(ObjectId, List28);
                    PlayObject.SendMsg(this, Grobal2.RM_SENDDELITEMLIST, 0, ObjectId, 0, 0, "");
                }
            }
            return result;
        }

        public void ClientMakeDrugItem(PlayObject PlayObject, string sItemName)
        {
            var n14 = 1;
            for (var i = 0; i < GoodsList.Count; i++)
            {
                IList<UserItem> List1C = GoodsList[i];
                var MakeItem = List1C[0];
                var StdItem = M2Share.WorldEngine.GetStdItem(MakeItem.Index);
                if (StdItem != null && StdItem.Name == sItemName)
                {
                    if (PlayObject.Gold >= M2Share.Config.MakeDurgPrice)
                    {
                        if (ClientMakeDrugItem_sub_4A28FC(PlayObject, sItemName))
                        {
                            var UserItem = new UserItem();
                            M2Share.WorldEngine.CopyToUserItemFromName(sItemName, ref UserItem);
                            if (PlayObject.AddItemToBag(UserItem))
                            {
                                PlayObject.Gold -= M2Share.Config.MakeDurgPrice;
                                PlayObject.SendAddItem(UserItem);
                                StdItem = M2Share.WorldEngine.GetStdItem(UserItem.Index);
                                if (StdItem.NeedIdentify == 1)
                                {
                                    M2Share.EventSource.AddEventLog(2, PlayObject.MapName + "\t" + PlayObject.CurrX + "\t" + PlayObject.CurrY + "\t" + PlayObject.ChrName + "\t" + StdItem.Name + "\t" + UserItem.MakeIndex + "\t" + '1' + "\t" + ChrName);
                                }
                                n14 = 0;
                                break;
                            }
                            else
                            {
                                DisPose(UserItem);
                                n14 = 2;
                            }
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
                PlayObject.SendMsg(this, Grobal2.RM_MAKEDRUG_SUCCESS, 0, PlayObject.Gold, 0, 0, "");
            }
            else
            {
                PlayObject.SendMsg(this, Grobal2.RM_MAKEDRUG_FAIL, 0, n14, 0, 0, "");
            }
        }

        /// <summary>
        /// 客户查询修复所需成本
        /// </summary>
        public void ClientQueryRepairCost(PlayObject PlayObject, UserItem UserItem)
        {
            int nRepairPrice;
            var nPrice = GetUserPrice(PlayObject, GetUserItemPrice(UserItem));
            if (nPrice > 0 && UserItem.DuraMax > UserItem.Dura)
            {
                if (UserItem.DuraMax > 0)
                {
                    nRepairPrice = HUtil32.Round((double)(nPrice / 3) / UserItem.DuraMax * (UserItem.DuraMax - UserItem.Dura));
                }
                else
                {
                    nRepairPrice = nPrice;
                }
                if (PlayObject.ScriptLable == ScriptConst.sSUPERREPAIR)
                {
                    if (m_boS_repair)
                    {
                        nRepairPrice = nRepairPrice * M2Share.Config.SuperRepairPriceRate;
                    }
                    else
                    {
                        nRepairPrice = -1;
                    }
                }
                else
                {
                    if (!m_boRepair)
                    {
                        nRepairPrice = -1;
                    }
                }
                PlayObject.SendMsg(this, Grobal2.RM_SENDREPAIRCOST, 0, nRepairPrice, 0, 0, "");
            }
            else
            {
                PlayObject.SendMsg(this, Grobal2.RM_SENDREPAIRCOST, 0, -1, 0, 0, "");
            }
        }

        /// <summary>
        /// 修理物品
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="UserItem"></param>
        /// <returns></returns>
        public bool ClientRepairItem(PlayObject PlayObject, UserItem UserItem)
        {
            int nRepairPrice;
            var result = false;
            var boCanRepair = true;
            if (PlayObject.ScriptLable == ScriptConst.sSUPERREPAIR && !m_boS_repair)
            {
                boCanRepair = false;
            }
            if (PlayObject.ScriptLable != ScriptConst.sSUPERREPAIR && !m_boRepair)
            {
                boCanRepair = false;
            }
            if (PlayObject.ScriptLable == "@fail_s_repair")
            {
                SendMsgToUser(PlayObject, "对不起!我不能帮你修理这个物品。\\ \\ \\<返回/@main>");
                PlayObject.SendMsg(this, Grobal2.RM_USERREPAIRITEM_FAIL, 0, 0, 0, 0, "");
                return result;
            }
            var nPrice = GetUserPrice(PlayObject, GetUserItemPrice(UserItem));
            if (PlayObject.ScriptLable == ScriptConst.sSUPERREPAIR)
            {
                nPrice = nPrice * M2Share.Config.SuperRepairPriceRate;
            }
            var StdItem = M2Share.WorldEngine.GetStdItem(UserItem.Index);
            if (StdItem != null)
            {
                if (boCanRepair && nPrice > 0 && UserItem.DuraMax > UserItem.Dura && StdItem.StdMode != 43)
                {
                    if (UserItem.DuraMax > 0)
                    {
                        nRepairPrice = HUtil32.Round(nPrice / 3 / UserItem.DuraMax * (UserItem.DuraMax - UserItem.Dura));
                    }
                    else
                    {
                        nRepairPrice = nPrice;
                    }
                    if (PlayObject.DecGold(nRepairPrice))
                    {
                        if (CastleMerchant || M2Share.Config.GetAllNpcTax)
                        {
                            if (Castle != null)
                            {
                                Castle.IncRateGold(nRepairPrice);
                            }
                            else if (M2Share.Config.GetAllNpcTax)
                            {
                                M2Share.CastleMgr.IncRateGold(M2Share.Config.UpgradeWeaponPrice);
                            }
                        }
                        if (PlayObject.ScriptLable == ScriptConst.sSUPERREPAIR)
                        {
                            UserItem.Dura = UserItem.DuraMax;
                            PlayObject.SendMsg(this, Grobal2.RM_USERREPAIRITEM_OK, 0, PlayObject.Gold, UserItem.Dura, UserItem.DuraMax, "");
                            GotoLable(PlayObject, ScriptConst.sSUPERREPAIROK, false);
                        }
                        else
                        {
                            UserItem.DuraMax -= (ushort)((UserItem.DuraMax - UserItem.Dura) / M2Share.Config.RepairItemDecDura);
                            UserItem.Dura = UserItem.DuraMax;
                            PlayObject.SendMsg(this, Grobal2.RM_USERREPAIRITEM_OK, 0, PlayObject.Gold, UserItem.Dura, UserItem.DuraMax, "");
                            GotoLable(PlayObject, ScriptConst.sREPAIROK, false);
                        }
                        result = true;
                    }
                    else
                    {
                        PlayObject.SendMsg(this, Grobal2.RM_USERREPAIRITEM_FAIL, 0, 0, 0, 0, "");
                    }
                }
                else
                {
                    PlayObject.SendMsg(this, Grobal2.RM_USERREPAIRITEM_FAIL, 0, 0, 0, 0, "");
                }
            }
            return result;
        }

        public override void ClearScript()
        {
            m_boBuy = false;
            m_boSell = false;
            m_boMakeDrug = false;
            m_boPrices = false;
            m_boStorage = false;
            m_boGetback = false;
            m_boUpgradenow = false;
            m_boGetBackupgnow = false;
            m_boRepair = false;
            m_boS_repair = false;
            m_boGetMarry = false;
            m_boGetMaster = false;
            m_boUseItemName = false;
            base.ClearScript();
        }

        private void LoadUpgradeList()
        {
            for (var i = 0; i < UpgradeWeaponList.Count; i++)
            {
                UpgradeWeaponList[i] = null;
            }
            UpgradeWeaponList.Clear();
            try
            {
                M2Share.CommonDb.LoadUpgradeWeaponRecord(m_sScript + '-' + MapName, UpgradeWeaponList);
            }
            catch
            {
                M2Share.Log.Error("Failure in loading upgradinglist - " + ChrName);
            }
        }

        private void SaveUpgradingList()
        {
            try
            {
                M2Share.CommonDb.SaveUpgradeWeaponRecord(m_sScript + '-' + MapName, UpgradeWeaponList);
            }
            catch
            {
                M2Share.Log.Error("Failure in saving upgradinglist - " + ChrName);
            }
        }

        /// <summary>
        /// 设置挂机留言信息
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sMsg"></param>
        protected void SetOffLineMsg(PlayObject PlayObject, string sMsg)
        {
            PlayObject.MSOffLineLeaveword = sMsg;
        }

        protected override void SendCustemMsg(PlayObject PlayObject, string sMsg)
        {
            base.SendCustemMsg(PlayObject, sMsg);
        }

        /// <summary>
        /// 清除临时文件，包括交易库存，价格表
        /// </summary>
        public void ClearData()
        {
            UserItem UserItem;
            IList<UserItem> ItemList;
            ItemPrice ItemPrice;
            const string sExceptionMsg = "[Exception] TMerchant::ClearData";
            try
            {
                for (var i = 0; i < GoodsList.Count; i++)
                {
                    ItemList = GoodsList[i];
                    for (var j = 0; j < ItemList.Count; j++)
                    {
                        UserItem = ItemList[j];
                        Dispose(UserItem);
                    }
                }
                GoodsList.Clear();
                for (var i = 0; i < ItemPriceList.Count; i++)
                {
                    ItemPrice = ItemPriceList[i];
                    Dispose(ItemPrice);
                }
                ItemPriceList.Clear();
                SaveNPCData();
            }
            catch (Exception e)
            {
                M2Share.Log.Error(sExceptionMsg);
                M2Share.Log.Error(e.Message);
            }
        }

        private void ChangeUseItemName(PlayObject PlayObject, string sLabel, string sItemName)
        {
            if (!PlayObject.m_boChangeItemNameFlag)
            {
                return;
            }
            PlayObject.m_boChangeItemNameFlag = false;
            var sWhere = sLabel.Substring(ScriptConst.sUSEITEMNAME.Length, sLabel.Length - ScriptConst.sUSEITEMNAME.Length);
            var btWhere = (byte)HUtil32.StrToInt(sWhere, -1);
            if (btWhere >= 0 && btWhere <= PlayObject.UseItems.Length)
            {
                var UserItem = PlayObject.UseItems[btWhere];
                if (UserItem.Index == 0)
                {
                    var sMsg = Format(M2Share.g_sYourUseItemIsNul, M2Share.GetUseItemName(btWhere));
                    PlayObject.SendMsg(this, Grobal2.RM_MENU_OK, 0, PlayObject.ActorId, 0, 0, sMsg);
                    return;
                }
                if (UserItem.Desc[13] == 1)
                {
                    M2Share.CustomItemMgr.DelCustomItemName(UserItem.MakeIndex, UserItem.Index);
                }
                if (!string.IsNullOrEmpty(sItemName))
                {
                    M2Share.CustomItemMgr.AddCustomItemName(UserItem.MakeIndex, UserItem.Index, sItemName);
                    UserItem.Desc[13] = 1;
                }
                else
                {
                    M2Share.CustomItemMgr.DelCustomItemName(UserItem.MakeIndex, UserItem.Index);
                    UserItem.Desc[13] = 0;
                }
                M2Share.CustomItemMgr.SaveCustomItemName();
                PlayObject.SendMsg(PlayObject, Grobal2.RM_SENDUSEITEMS, 0, 0, 0, 0, "");
                PlayObject.SendMsg(this, Grobal2.RM_MENU_OK, 0, PlayObject.ActorId, 0, 0, "");
            }
        }

        private void DisPose(object obj)
        {
        }
    }
}