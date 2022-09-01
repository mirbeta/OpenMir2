using GameSvr.Items;
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
        public int m_nPriceRate = 0;
        public bool m_boCastle = false;
        public int dwRefillGoodsTick = 0;
        public int dwClearExpreUpgradeTick = 0;
        /// <summary>
        /// NPC买卖物品类型列表，脚本中前面的 +1 +30 之类的
        /// </summary>
        public IList<int> m_ItemTypeList = null;
        public IList<TGoods> m_RefillGoodsList = null;
        /// <summary>
        /// 商品列表
        /// </summary>
        private readonly IList<IList<TUserItem>> m_GoodsList = null;
        /// <summary>
        /// 物品价格列表
        /// </summary>
        private readonly IList<TItemPrice> m_ItemPriceList = null;
        /// <summary>
        /// 物品升级列表
        /// </summary>
        private readonly IList<TUpgradeInfo> m_UpgradeWeaponList = null;
        public bool m_boCanMove = false;
        public int m_dwMoveTime = 0;
        public int m_dwMoveTick = 0;
        /// <summary>
        /// 是否购买物品
        /// </summary>
        public bool m_boBuy = false;
        /// <summary>
        /// 是否交易物品
        /// </summary>
        public bool m_boSell = false;
        public bool m_boMakeDrug = false;
        public bool m_boPrices = false;
        public bool m_boStorage = false;
        public bool m_boGetback = false;
        public bool m_boUpgradenow = false;
        public bool m_boGetBackupgnow = false;
        public bool m_boRepair = false;
        public bool m_boS_repair = false;
        public bool m_boSendmsg = false;
        public bool m_boGetMarry = false;
        public bool m_boGetMaster = false;
        public bool m_boUseItemName = false;
        public bool m_boOffLineMsg = false;
        public bool m_boYBDeal = false;

        private void AddItemPrice(int nIndex, int nPrice)
        {
            TItemPrice ItemPrice;
            ItemPrice = new TItemPrice
            {
                wIndex = (short)nIndex,
                nPrice = nPrice
            };
            m_ItemPriceList.Add(ItemPrice);
            M2Share.LocalDB.SaveGoodPriceRecord(this, m_sScript + '-' + m_sMapName);
        }

        private void CheckItemPrice(int nIndex)
        {
            TItemPrice ItemPrice;
            double n10;
            StdItem StdItem;
            for (var i = 0; i < m_ItemPriceList.Count; i++)
            {
                ItemPrice = m_ItemPriceList[i];
                if (ItemPrice.wIndex == nIndex)
                {
                    n10 = ItemPrice.nPrice;
                    if (Math.Round(n10 * 1.1) > n10)
                    {
                        n10 = HUtil32.Round(n10 * 1.1);
                    }
                    else
                    {
                        n10++;
                    }
                    return;
                }
            }
            StdItem = M2Share.UserEngine.GetStdItem(nIndex);
            if (StdItem != null)
            {
                AddItemPrice(nIndex, HUtil32.Round(StdItem.Price * 1.1));
            }
        }

        private IList<TUserItem> GetRefillList(int nIndex)
        {
            IList<TUserItem> result = null;
            IList<TUserItem> List;
            if (nIndex <= 0)
            {
                return result;
            }
            for (var i = 0; i < m_GoodsList.Count; i++)
            {
                List = m_GoodsList[i];
                if (List.Count > 0)
                {
                    if (List[0].wIndex == nIndex)
                    {
                        result = List;
                        break;
                    }
                }
            }
            return result;
        }

        private void RefillGoods_RefillItems(ref IList<TUserItem> List, string sItemName, int nInt)
        {
            TUserItem UserItem;
            if (List == null)
            {
                List = new List<TUserItem>();
                m_GoodsList.Add(List);
            }
            for (var i = 0; i < nInt; i++)
            {
                UserItem = new TUserItem();
                if (M2Share.UserEngine.CopyToUserItemFromName(sItemName, ref UserItem))
                {
                    List.Insert(0, UserItem);
                }
                else
                {
                    Dispose(UserItem);
                }
            }
        }

        private void RefillGoods_DelReFillItem(ref IList<TUserItem> List, int nInt)
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

        private void RefillGoods()
        {
            TGoods Goods;
            int nIndex;
            int nRefillCount;
            IList<TUserItem> RefillList;
            IList<TUserItem> RefillList20;
            bool bo21;
            const string sExceptionMsg = "[Exception] TMerchant::RefillGoods {0}/{1}:{2} [{3}] Code:{4}";
            try
            {
                for (var i = 0; i < m_RefillGoodsList.Count; i++)
                {
                    Goods = m_RefillGoodsList[i];
                    if ((HUtil32.GetTickCount() - Goods.dwRefillTick) > (Goods.dwRefillTime * 60 * 1000))
                    {
                        Goods.dwRefillTick = HUtil32.GetTickCount();
                        nIndex = M2Share.UserEngine.GetStdItemIdx(Goods.sItemName);
                        if (nIndex >= 0)
                        {
                            RefillList = GetRefillList(nIndex);
                            nRefillCount = 0;
                            if (RefillList != null)
                            {
                                nRefillCount = RefillList.Count;
                            }
                            if (Goods.nCount > nRefillCount)
                            {
                                CheckItemPrice(nIndex);
                                RefillGoods_RefillItems(ref RefillList, Goods.sItemName, Goods.nCount - nRefillCount);
                                M2Share.LocalDB.SaveGoodRecord(this, m_sScript + '-' + m_sMapName);
                                M2Share.LocalDB.SaveGoodPriceRecord(this, m_sScript + '-' + m_sMapName);
                            }
                            if (Goods.nCount < nRefillCount)
                            {
                                RefillGoods_DelReFillItem(ref RefillList, nRefillCount - Goods.nCount);
                                M2Share.LocalDB.SaveGoodRecord(this, m_sScript + '-' + m_sMapName);
                                M2Share.LocalDB.SaveGoodPriceRecord(this, m_sScript + '-' + m_sMapName);
                            }
                        }
                    }
                }
                for (var i = 0; i < m_GoodsList.Count; i++)
                {
                    RefillList20 = m_GoodsList[i];
                    if (RefillList20.Count > 1000)
                    {
                        bo21 = false;
                        for (var j = 0; j < m_RefillGoodsList.Count; j++)
                        {
                            Goods = m_RefillGoodsList[j];
                            nIndex = M2Share.UserEngine.GetStdItemIdx(Goods.sItemName);
                            if (RefillList20[0].wIndex == nIndex)
                            {
                                bo21 = true;
                                break;
                            }
                        }
                        if (!bo21)
                        {
                            RefillGoods_DelReFillItem(ref RefillList20, RefillList20.Count - 1000);
                        }
                        else
                        {
                            RefillGoods_DelReFillItem(ref RefillList20, RefillList20.Count - 5000);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                M2Share.MainOutMessage(format(sExceptionMsg, m_sCharName, m_nCurrX, m_nCurrY, e.Message, ScriptConst.nCHECK));
            }
        }

        private bool CheckItemType(int nStdMode)
        {
            var result = false;
            for (var i = 0; i < m_ItemTypeList.Count; i++)
            {
                if (m_ItemTypeList[i] == nStdMode)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        private double GetItemPrice(int nIndex)
        {
            double result = -1;
            TItemPrice ItemPrice;
            StdItem StdItem;
            for (var i = 0; i < m_ItemPriceList.Count; i++)
            {
                ItemPrice = m_ItemPriceList[i];
                if (ItemPrice.wIndex == nIndex)
                {
                    result = ItemPrice.nPrice;
                    break;
                }
            }
            if (result < 0)
            {
                StdItem = M2Share.UserEngine.GetStdItem(nIndex);
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

        private void SaveUpgradingList()
        {
            try
            {
                M2Share.LocalDB.SaveUpgradeWeaponRecord(m_sScript + '-' + m_sMapName, m_UpgradeWeaponList);
            }
            catch
            {
                M2Share.MainOutMessage("Failure in saving upgradinglist - " + m_sCharName);
            }
        }

        private void UpgradeWaponAddValue(TPlayObject User, IList<TUserItem> ItemList, ref byte btDc, ref byte btSc, ref byte btMc, ref byte btDura)
        {
            TUserItem UserItem;
            StdItem StdItem;
            TClientStdItem StdItem80 = null;
            IList<TDeleteItem> DelItemList = null;
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
                UserItem = ItemList[i];
                if (M2Share.UserEngine.GetStdItemName(UserItem.wIndex) == M2Share.g_Config.sBlackStone)
                {
                    DuraList.Add(Math.Round(UserItem.Dura / 1.0e3));
                    if (DelItemList == null)
                    {
                        DelItemList = new List<TDeleteItem>();
                    }
                    DelItemList.Add(new TDeleteItem()
                    {
                        MakeIndex = UserItem.MakeIndex,
                        sItemName = M2Share.g_Config.sBlackStone
                    });
                    DisPose(UserItem);
                    ItemList.RemoveAt(i);
                }
                else
                {
                    if (M2Share.IsAccessory(UserItem.wIndex))
                    {
                        StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                        if (StdItem != null)
                        {
                            StdItem.GetStandardItem(ref StdItem80);
                            StdItem.GetItemAddValue(UserItem, ref StdItem80);
                            nDc = 0;
                            nSc = 0;
                            nMc = 0;
                            switch (StdItem80.StdMode)
                            {
                                case 19:
                                case 20:
                                case 21:
                                    nDc = HUtil32.HiWord(StdItem80.DC) + HUtil32.LoWord(StdItem80.DC);
                                    nSc = HUtil32.HiWord(StdItem80.SC) + HUtil32.LoWord(StdItem80.SC);
                                    nMc = HUtil32.HiWord(StdItem80.MC) + HUtil32.LoWord(StdItem80.MC);
                                    break;
                                case 22:
                                case 23:
                                    nDc = HUtil32.HiWord(StdItem80.DC) + HUtil32.LoWord(StdItem80.DC);
                                    nSc = HUtil32.HiWord(StdItem80.SC) + HUtil32.LoWord(StdItem80.SC);
                                    nMc = HUtil32.HiWord(StdItem80.MC) + HUtil32.LoWord(StdItem80.MC);
                                    break;
                                case 24:
                                case 26:
                                    nDc = HUtil32.HiWord(StdItem80.DC) + HUtil32.LoWord(StdItem80.DC) + 1;
                                    nSc = HUtil32.HiWord(StdItem80.SC) + HUtil32.LoWord(StdItem80.SC) + 1;
                                    nMc = HUtil32.HiWord(StdItem80.MC) + HUtil32.LoWord(StdItem80.MC) + 1;
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
                                DelItemList = new List<TDeleteItem>();
                            }
                            DelItemList.Add(new TDeleteItem()
                            {
                                sItemName = StdItem.Name,
                                MakeIndex = UserItem.MakeIndex
                            });
                            if (StdItem.NeedIdentify == 1)
                            {
                                M2Share.AddGameDataLog("26" + "\t" + User.m_sMapName + "\t" + User.m_nCurrX + "\t" + User.m_nCurrY + "\t" + User.m_sCharName + "\t" + StdItem.Name + "\t" + UserItem.MakeIndex + "\t" + '1' + "\t" + '0');
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
                M2Share.ActorManager.AddOhter(objectId, DelItemList);
                User.SendMsg(this, Grobal2.RM_SENDDELITEMLIST, 0, objectId, 0, 0, "");
            }
            if (DuraList != null)
            {
                DuraList = null;
            }
        }

        private void UpgradeWapon(TPlayObject User)
        {
            var bo0D = false;
            TUpgradeInfo upgradeInfo;
            for (var i = 0; i < m_UpgradeWeaponList.Count; i++)
            {
                upgradeInfo = m_UpgradeWeaponList[i];
                if (upgradeInfo.sUserName == User.m_sCharName)
                {
                    GotoLable(User, ScriptConst.sUPGRADEING, false);
                    return;
                }
            }
            if (User.m_UseItems[Grobal2.U_WEAPON] != null && User.m_UseItems[Grobal2.U_WEAPON].wIndex != 0 && User.m_nGold >= M2Share.g_Config.nUpgradeWeaponPrice
                && User.CheckItems(M2Share.g_Config.sBlackStone) != null)
            {
                User.DecGold(M2Share.g_Config.nUpgradeWeaponPrice);
                if (m_boCastle || M2Share.g_Config.boGetAllNpcTax)
                {
                    if (m_Castle != null)
                    {
                        m_Castle.IncRateGold(M2Share.g_Config.nUpgradeWeaponPrice);
                    }
                    else if (M2Share.g_Config.boGetAllNpcTax)
                    {
                        M2Share.CastleManager.IncRateGold(M2Share.g_Config.nUpgradeWeaponPrice);
                    }
                }
                User.GoldChanged();
                var userItem = new TUserItem(User.m_UseItems[Grobal2.U_WEAPON]);
                upgradeInfo = new TUpgradeInfo
                {
                    sUserName = User.m_sCharName,
                    UserItem = userItem
                };
                var StdItem = M2Share.UserEngine.GetStdItem(User.m_UseItems[Grobal2.U_WEAPON].wIndex);
                if (StdItem.NeedIdentify == 1)
                {
                    M2Share.AddGameDataLog("25" + "\t" + User.m_sMapName + "\t" + User.m_nCurrX + "\t" + User.m_nCurrY + "\t" + User.m_sCharName + "\t" + StdItem.Name + "\t" + User.m_UseItems[Grobal2.U_WEAPON].MakeIndex + "\t" + '1' + "\t" + '0');
                }
                User.SendDelItems(User.m_UseItems[Grobal2.U_WEAPON]);
                User.m_UseItems[Grobal2.U_WEAPON].wIndex = 0;
                User.RecalcAbilitys();
                User.FeatureChanged();
                User.SendMsg(User, Grobal2.RM_ABILITY, 0, 0, 0, 0, "");
                UpgradeWaponAddValue(User, User.m_ItemList, ref upgradeInfo.btDc, ref upgradeInfo.btSc, ref upgradeInfo.btMc, ref upgradeInfo.btDura);
                upgradeInfo.dtTime = DateTime.Now;
                upgradeInfo.dwGetBackTick = HUtil32.GetTickCount();
                m_UpgradeWeaponList.Add(upgradeInfo);
                SaveUpgradingList();
                bo0D = true;
                userItem = null;
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
        private void GetBackupgWeapon(TPlayObject User)
        {
            TUpgradeInfo UpgradeInfo = null;
            int n18 = 0;
            if (!User.IsEnoughBag())
            {
                GotoLable(User, ScriptConst.sGETBACKUPGFULL, false);
                return;
            }
            for (var i = 0; i < m_UpgradeWeaponList.Count; i++)
            {
                if (m_UpgradeWeaponList[i].sUserName == User.m_sCharName)
                {
                    n18 = 1;
                    if (((HUtil32.GetTickCount() - m_UpgradeWeaponList[i].dwGetBackTick) > M2Share.g_Config.dwUPgradeWeaponGetBackTime) || User.m_btPermission >= 4)
                    {
                        UpgradeInfo = m_UpgradeWeaponList[i];
                        m_UpgradeWeaponList.RemoveAt(i);
                        SaveUpgradingList();
                        n18 = 2;
                        break;
                    }
                }
            }
            if (UpgradeInfo != null)
            {
                if (HUtil32.RangeInDefined(UpgradeInfo.btDura, 0, 8))
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
                else if (HUtil32.RangeInDefined(UpgradeInfo.btDura, 9, 15))
                {
                    if (M2Share.RandomNumber.Random(UpgradeInfo.btDura) < 6)
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
                else if (HUtil32.RangeInDefined(UpgradeInfo.btDura, 18, 255))
                {
                    var r = M2Share.RandomNumber.Random(UpgradeInfo.btDura - 18);
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
                if (UpgradeInfo.btDc == UpgradeInfo.btMc && UpgradeInfo.btMc == UpgradeInfo.btSc)
                {
                    n1C = M2Share.RandomNumber.Random(3);
                }
                else
                {
                    n1C = -1;
                }
                int n90;
                int n10;
                if (UpgradeInfo.btDc >= UpgradeInfo.btMc && (UpgradeInfo.btDc >= UpgradeInfo.btSc) || (n1C == 0))
                {
                    n90 = HUtil32._MIN(11, UpgradeInfo.btDc);
                    n10 = HUtil32._MIN(85, (n90 << 3 - n90) + 10 + UpgradeInfo.UserItem.btValue[3] - UpgradeInfo.UserItem.btValue[4] + User.m_nBodyLuckLevel);
                    if (M2Share.RandomNumber.Random(M2Share.g_Config.nUpgradeWeaponDCRate) < n10)
                    {
                        UpgradeInfo.UserItem.btValue[ItemAttr.WeaponUpgrade] = 10;
                        if (n10 > 63 && M2Share.RandomNumber.Random(M2Share.g_Config.nUpgradeWeaponDCTwoPointRate) == 0)
                        {
                            UpgradeInfo.UserItem.btValue[ItemAttr.WeaponUpgrade] = 11;
                        }
                        if (n10 > 79 && M2Share.RandomNumber.Random(M2Share.g_Config.nUpgradeWeaponDCThreePointRate) == 0)
                        {
                            UpgradeInfo.UserItem.btValue[ItemAttr.WeaponUpgrade] = 12;
                        }
                    }
                    else
                    {
                        UpgradeInfo.UserItem.btValue[ItemAttr.WeaponUpgrade] = 1;
                    }
                }
                if (UpgradeInfo.btMc >= UpgradeInfo.btDc && UpgradeInfo.btMc >= UpgradeInfo.btSc || n1C == 1)
                {
                    n90 = HUtil32._MIN(11, UpgradeInfo.btMc);
                    n10 = HUtil32._MIN(85, (n90 << 3 - n90) + 10 + UpgradeInfo.UserItem.btValue[3] - UpgradeInfo.UserItem.btValue[4] + User.m_nBodyLuckLevel);
                    if (M2Share.RandomNumber.Random(M2Share.g_Config.nUpgradeWeaponMCRate) < n10)
                    {
                        UpgradeInfo.UserItem.btValue[ItemAttr.WeaponUpgrade] = 20;
                        if (n10 > 63 && M2Share.RandomNumber.Random(M2Share.g_Config.nUpgradeWeaponMCTwoPointRate) == 0)
                        {
                            UpgradeInfo.UserItem.btValue[ItemAttr.WeaponUpgrade] = 21;
                        }
                        if (n10 > 79 && M2Share.RandomNumber.Random(M2Share.g_Config.nUpgradeWeaponMCThreePointRate) == 0)
                        {
                            UpgradeInfo.UserItem.btValue[ItemAttr.WeaponUpgrade] = 22;
                        }
                    }
                    else
                    {
                        UpgradeInfo.UserItem.btValue[ItemAttr.WeaponUpgrade] = 1;
                    }
                }
                if (UpgradeInfo.btSc >= UpgradeInfo.btMc && UpgradeInfo.btSc >= UpgradeInfo.btDc || n1C == 2)
                {
                    n90 = HUtil32._MIN(11, UpgradeInfo.btMc);
                    n10 = HUtil32._MIN(85, (n90 << 3 - n90) + 10 + UpgradeInfo.UserItem.btValue[3] - UpgradeInfo.UserItem.btValue[4] + User.m_nBodyLuckLevel);
                    if (M2Share.RandomNumber.Random(M2Share.g_Config.nUpgradeWeaponSCRate) < n10)
                    {
                        UpgradeInfo.UserItem.btValue[ItemAttr.WeaponUpgrade] = 30;
                        if (n10 > 63 && M2Share.RandomNumber.Random(M2Share.g_Config.nUpgradeWeaponSCTwoPointRate) == 0)
                        {
                            UpgradeInfo.UserItem.btValue[ItemAttr.WeaponUpgrade] = 31;
                        }
                        if (n10 > 79 && M2Share.RandomNumber.Random(M2Share.g_Config.nUpgradeWeaponSCThreePointRate) == 0)
                        {
                            UpgradeInfo.UserItem.btValue[ItemAttr.WeaponUpgrade] = 32;
                        }
                    }
                    else
                    {
                        UpgradeInfo.UserItem.btValue[ItemAttr.WeaponUpgrade] = 1;
                    }
                }
                var UserItem = UpgradeInfo.UserItem;
                var StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                if (StdItem.NeedIdentify == 1)
                {
                    M2Share.AddGameDataLog("24" + "\t" + User.m_sMapName + "\t" + User.m_nCurrX + "\t" + User.m_nCurrY + "\t" + User.m_sCharName + "\t" + StdItem.Name + "\t" + UserItem.MakeIndex + "\t" + '1' + "\t" + '0');
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
        /// <param name="PlayObject"></param>
        /// <param name="nPrice"></param>
        /// <returns></returns>
        private int GetUserPrice(TPlayObject PlayObject, double nPrice)
        {
            int result;
            if (m_boCastle)
            {
                if (m_Castle != null && m_Castle.IsMasterGuild(PlayObject.m_MyGuild)) //沙巴克成员修复物品打折
                {
                    var n14 = HUtil32._MAX(60, HUtil32.Round(m_nPriceRate * (M2Share.g_Config.nCastleMemberPriceRate / 100)));//80%
                    result = HUtil32.Round(nPrice / 100 * n14);
                }
                else
                {
                    result = HUtil32.Round(nPrice / 100 * m_nPriceRate);
                }
            }
            else
            {
                result = HUtil32.Round(nPrice / 100 * m_nPriceRate);
            }
            return result;
        }

        private void UserSelect_SuperRepairItem(TPlayObject User)
        {
            User.SendMsg(this, Grobal2.RM_SENDUSERSREPAIR, 0, ObjectId, 0, 0, "");
        }

        private void UserSelect_BuyItem(TPlayObject User, int nInt)
        {
            var sSendMsg = string.Empty;
            var n10 = 0;
            for (var i = 0; i < m_GoodsList.Count; i++)
            {
                var List14 = m_GoodsList[i];
                var UserItem = List14[0];
                var StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                if (StdItem != null)
                {
                    var sName = ItmUnit.GetItemName(UserItem);
                    var nPrice = GetUserPrice(User, GetItemPrice(UserItem.wIndex));
                    var nStock = List14.Count;
                    short nSubMenu;
                    if (StdItem.StdMode <= 4 || StdItem.StdMode == 42 || StdItem.StdMode == 31)
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
            User.SendMsg(this, Grobal2.RM_SENDGOODSLIST, 0, ObjectId, n10, 0, sSendMsg);
        }

        private void UserSelect_SellItem(TPlayObject User)
        {
            User.SendMsg(this, Grobal2.RM_SENDUSERSELL, 0, ObjectId, 0, 0, "");
        }

        private void UserSelect_RepairItem(TPlayObject User)
        {
            User.SendMsg(this, Grobal2.RM_SENDUSERREPAIR, 0, ObjectId, 0, 0, "");
        }

        private void UserSelect_MakeDurg(TPlayObject User)
        {
            IList<TUserItem> List14;
            TUserItem UserItem;
            StdItem StdItem;
            var sSendMsg = string.Empty;
            for (var i = 0; i < m_GoodsList.Count; i++)
            {
                List14 = m_GoodsList[i];
                UserItem = List14[0];
                StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                if (StdItem != null)
                {
                    sSendMsg = sSendMsg + StdItem.Name + '/' + 0 + '/' + M2Share.g_Config.nMakeDurgPrice + '/' + 1 + '/';
                }
            }
            if (sSendMsg != "")
            {
                User.SendMsg(this, Grobal2.RM_USERMAKEDRUGITEMLIST, 0, ObjectId, 0, 0, sSendMsg);
            }
        }

        private void UserSelect_ItemPrices(TPlayObject User)
        {
        }

        private void UserSelect_Storage(TPlayObject User)
        {
            User.SendMsg(this, Grobal2.RM_USERSTORAGEITEM, 0, ObjectId, 0, 0, "");
        }

        private void UserSelect_GetBack(TPlayObject User)
        {
            User.SendMsg(this, Grobal2.RM_USERGETBACKITEM, 0, ObjectId, 0, 0, "");
        }

        /// <summary>
        /// 打开出售物品窗口
        /// </summary>
        /// <param name="User"></param>
        private void UserSelect_OpenDealOffForm(TPlayObject User)
        {
            if (User.bo_YBDEAL)
            {
                if (!User.SellOffInTime(0))
                {
                    User.SendMsg(this, Grobal2.RM_SENDDEALOFFFORM, 0, this.ObjectId, 0, 0, "");
                    User.GetBackSellOffItems();
                }
                else
                {
                    User.SendMsg(this, Grobal2.RM_MERCHANTSAY, 0, 0, 0, 0, this.m_sCharName + "/您还有元宝服务正在进行!!\\ \\<返回/@main>");
                }
            }
            else
            {
                User.SendMsg(this, Grobal2.RM_MERCHANTSAY, 0, 0, 0, 0, this.m_sCharName + "/您未开通元宝服务,请先开通元宝服务!!\\ \\<返回/@main>");
            }
        }

        public override void UserSelect(TPlayObject PlayObject, string sData)
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
                if (!m_boCastle || !(m_Castle != null && m_Castle.m_boUnderWar))
                {
                    if (!PlayObject.m_boDeath && sData != "" && sData[0] == '@')
                    {
                        string sMsg = HUtil32.GetValidStr3(sData, ref sLabel, new char[] { '\r' });
                        PlayObject.m_sScriptLable = sData;
                        bool boCanJmp = PlayObject.LableIsCanJmp(sLabel);
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
                                UserSelect_SuperRepairItem(PlayObject);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptConst.sBUY, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (m_boBuy)
                            {
                                UserSelect_BuyItem(PlayObject, 0);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptConst.sSELL, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (m_boSell)
                            {
                                UserSelect_SellItem(PlayObject);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptConst.sREPAIR, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (m_boRepair)
                            {
                                UserSelect_RepairItem(PlayObject);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptConst.sMAKEDURG, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (m_boMakeDrug)
                            {
                                UserSelect_MakeDurg(PlayObject);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptConst.sPRICES, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (m_boPrices)
                            {
                                UserSelect_ItemPrices(PlayObject);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptConst.sSTORAGE, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (m_boStorage)
                            {
                                UserSelect_Storage(PlayObject);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptConst.sGETBACK, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            if (m_boGetback)
                            {
                                UserSelect_GetBack(PlayObject);
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
                        else if (HUtil32.CompareLStr(sLabel, ScriptConst.sUSEITEMNAME, ScriptConst.sUSEITEMNAME.Length))
                        {
                            if (m_boUseItemName)
                            {
                                ChangeUseItemName(PlayObject, sLabel, sMsg);
                            }
                        }
                        else if (string.Compare(sLabel, ScriptConst.sEXIT, StringComparison.OrdinalIgnoreCase) == 0)
                        {
                            PlayObject.SendMsg(this, Grobal2.RM_MERCHANTDLGCLOSE, 0, ObjectId, 0, 0, "");
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
                                UserSelect_OpenDealOffForm(PlayObject); // 打开出售物品窗口
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                M2Share.MainOutMessage(format(sExceptionMsg, sData));
                M2Share.MainOutMessage(ex.StackTrace);
            }
        }

        public override void Run()
        {
            try
            {
                if ((HUtil32.GetTickCount() - dwRefillGoodsTick) > 30000)
                {
                    dwRefillGoodsTick = HUtil32.GetTickCount();
                    RefillGoods();
                }
                if ((HUtil32.GetTickCount() - dwClearExpreUpgradeTick) > 10 * 60 * 1000)
                {
                    dwClearExpreUpgradeTick = HUtil32.GetTickCount();
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
                        SendRefMsg(Grobal2.RM_HIT, Direction, m_nCurrX, m_nCurrY, 0, "");
                    }
                }
                if (m_boCastle && m_Castle != null && m_Castle.m_boUnderWar)
                {
                    if (!m_boFixedHideMode)
                    {
                        SendRefMsg(Grobal2.RM_DISAPPEAR, 0, 0, 0, 0, "");
                        m_boFixedHideMode = true;
                    }
                }
                else
                {
                    if (m_boFixedHideMode)
                    {
                        m_boFixedHideMode = false;
                        SendRefMsg(Grobal2.RM_HIT, Direction, m_nCurrX, m_nCurrY, 0, "");
                    }
                }
                if (m_boCanMove && (HUtil32.GetTickCount() - m_dwMoveTick) > m_dwMoveTime * 1000)
                {
                    m_dwMoveTick = HUtil32.GetTickCount();
                    SendRefMsg(Grobal2.RM_SPACEMOVE_FIRE, 0, 0, 0, 0, "");
                    MapRandomMove(m_sMapName, 0);
                }
            }
            catch (Exception e)
            {
                M2Share.ErrorMessage(e.Message);
            }
            base.Run();
        }

        protected override bool Operate(TProcessMessage ProcessMsg)
        {
            return base.Operate(ProcessMsg);
        }

        public void LoadNPCData()
        {
            var sFile = m_sScript + '-' + m_sMapName;
            M2Share.LocalDB.LoadGoodRecord(this, sFile);
            M2Share.LocalDB.LoadGoodPriceRecord(this, sFile);
            LoadUpgradeList();
        }

        private void SaveNPCData()
        {
            var sFile = m_sScript + '-' + m_sMapName;
            M2Share.LocalDB.SaveGoodRecord(this, sFile);
            M2Share.LocalDB.SaveGoodPriceRecord(this, sFile);
        }

        public Merchant() : base()
        {
            m_btRaceImg = Grobal2.RCC_MERCHANT;
            m_wAppr = 0;
            m_nPriceRate = 100;
            m_boCastle = false;
            m_ItemTypeList = new List<int>();
            m_RefillGoodsList = new List<TGoods>();
            m_GoodsList = new List<IList<TUserItem>>();
            m_ItemPriceList = new List<TItemPrice>();
            m_UpgradeWeaponList = new List<TUpgradeInfo>();
            dwRefillGoodsTick = HUtil32.GetTickCount();
            dwClearExpreUpgradeTick = HUtil32.GetTickCount();
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
        }

        /// <summary>
        /// 清理武器升级过期数据
        /// </summary>
        private void ClearExpreUpgradeListData()
        {
            TUpgradeInfo UpgradeInfo;
            for (var i = m_UpgradeWeaponList.Count - 1; i >= 0; i--)
            {
                UpgradeInfo = m_UpgradeWeaponList[i];
                if ((int)Math.Round(DateTime.Now.ToOADate() - UpgradeInfo.dtTime.ToOADate()) >= M2Share.g_Config.nClearExpireUpgradeWeaponDays)
                {
                    Dispose(UpgradeInfo);
                    m_UpgradeWeaponList.RemoveAt(i);
                }
            }
        }

        public void LoadMerchantScript()
        {
            m_ItemTypeList.Clear();
            m_sPath = ScriptConst.sMarket_Def;
            var sC = m_sScript + '-' + m_sMapName;
            M2Share.ScriptSystem.LoadScriptFile(this, ScriptConst.sMarket_Def, sC, true);
        }

        public override void Click(TPlayObject PlayObject)
        {
            base.Click(PlayObject);
        }

        protected override void GetVariableText(TPlayObject PlayObject, ref string sMsg, string sVariable)
        {
            string sText;
            base.GetVariableText(PlayObject, ref sMsg, sVariable);
            switch (sVariable)
            {
                case "$PRICERATE":
                    sText = m_nPriceRate.ToString();
                    sMsg = ReplaceVariableText(sMsg, "<$PRICERATE>", sText);
                    break;
                case "$UPGRADEWEAPONFEE":
                    sText = M2Share.g_Config.nUpgradeWeaponPrice.ToString();
                    sMsg = ReplaceVariableText(sMsg, "<$UPGRADEWEAPONFEE>", sText);
                    break;
                case "$USERWEAPON":
                    {
                        if (PlayObject.m_UseItems[Grobal2.U_WEAPON].wIndex != 0)
                        {
                            sText = M2Share.UserEngine.GetStdItemName(PlayObject.m_UseItems[Grobal2.U_WEAPON].wIndex);
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

        private double GetUserItemPrice(TUserItem UserItem)
        {
            double result;
            StdItem StdItem;
            double n20;
            int nC;
            int n14;
            var n10 = GetItemPrice(UserItem.wIndex);
            if (n10 > 0)
            {
                StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                if (StdItem != null && StdItem.StdMode > 4 && StdItem.DuraMax > 0 && UserItem.DuraMax > 0)
                {
                    if (StdItem.StdMode == 40)// 肉
                    {
                        if (UserItem.Dura <= UserItem.DuraMax)
                        {
                            n20 = n10 / 2.0 / UserItem.DuraMax * (UserItem.DuraMax - UserItem.Dura);
                            n10 = HUtil32._MAX(2, HUtil32.Round(n10 - n20));
                        }
                        else
                        {
                            n10 = n10 + HUtil32.Round(n10 / UserItem.DuraMax * 2.0 * (UserItem.DuraMax - UserItem.Dura));
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
                            n20 = n10 / 2.0 / UserItem.DuraMax * (UserItem.DuraMax - UserItem.Dura);
                            n10 = HUtil32._MAX(2, HUtil32.Round(n10 - n20));
                        }
                        else
                        {
                            n10 = n10 + HUtil32.Round(n10 / UserItem.DuraMax * 1.3 * (UserItem.DuraMax - UserItem.Dura));
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
                                        if (UserItem.btValue[nC] > 10)
                                        {
                                            n14 = n14 + (UserItem.btValue[nC] - 10) * 2;
                                        }
                                    }
                                    else
                                    {
                                        n14 = n14 + UserItem.btValue[nC];
                                    }
                                }
                            }
                            else
                            {
                                n14 += UserItem.btValue[nC];
                            }
                            nC++;
                            if (nC >= 8)
                            {
                                break;
                            }
                        }
                        if (n14 > 0)
                        {
                            n10 = n10 / 5 * n14;
                        }
                        n10 = HUtil32.Round(n10 / StdItem.DuraMax * UserItem.DuraMax);
                        n20 = n10 / 2.0 / UserItem.DuraMax * (UserItem.DuraMax - UserItem.Dura);
                        n10 = HUtil32._MAX(2, HUtil32.Round(n10 - n20));
                    }
                }
            }
            result = n10;
            return result;
        }

        public void ClientBuyItem(TPlayObject PlayObject, string sItemName, int nInt)
        {
            IList<TUserItem> List20;
            TUserItem UserItem;
            StdItem StdItem;
            int nPrice;
            string sUserItemName;
            var bo29 = false;
            var n1C = 1;
            for (var i = 0; i < m_GoodsList.Count; i++)
            {
                if (bo29)
                {
                    break;
                }
                List20 = m_GoodsList[i];
                UserItem = List20[0];
                StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                if (StdItem != null)
                {
                    sUserItemName = ItmUnit.GetItemName(UserItem);
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
                                    if (PlayObject.m_nGold >= nPrice && nPrice > 0)
                                    {
                                        if (PlayObject.AddItemToBag(UserItem))
                                        {
                                            PlayObject.m_nGold -= nPrice;
                                            if (m_boCastle || M2Share.g_Config.boGetAllNpcTax)
                                            {
                                                if (m_Castle != null)
                                                {
                                                    m_Castle.IncRateGold(nPrice);
                                                }
                                                else if (M2Share.g_Config.boGetAllNpcTax)
                                                {
                                                    M2Share.CastleManager.IncRateGold(M2Share.g_Config.nUpgradeWeaponPrice);
                                                }
                                            }
                                            PlayObject.SendAddItem(UserItem);
                                            if (StdItem.NeedIdentify == 1)
                                            {
                                                M2Share.AddGameDataLog('9' + "\t" + PlayObject.m_sMapName + "\t" + PlayObject.m_nCurrX + "\t" + PlayObject.m_nCurrY + "\t" + PlayObject.m_sCharName + "\t" + StdItem.Name + "\t" + UserItem.MakeIndex + "\t" + '1' + "\t" + m_sCharName);
                                            }
                                            List20.RemoveAt(j);
                                            if (List20.Count <= 0)
                                            {
                                                m_GoodsList.RemoveAt(i);
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
                PlayObject.SendMsg(this, Grobal2.RM_BUYITEM_SUCCESS, 0, PlayObject.m_nGold, nInt, 0, "");
            }
            else
            {
                PlayObject.SendMsg(this, Grobal2.RM_BUYITEM_FAIL, 0, n1C, 0, 0, "");
            }
        }

        public void ClientGetDetailGoodsList(TPlayObject PlayObject, string sItemName, int nInt)
        {
            IList<TUserItem> List20;
            var sSendMsg = string.Empty;
            int nItemCount = 0;
            for (var i = 0; i < m_GoodsList.Count; i++)
            {
                List20 = m_GoodsList[i];
                if (List20.Count <= 0)
                {
                    continue;
                }
                TUserItem UserItem = List20[0];
                StdItem Item = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                if (Item != null && Item.Name == sItemName)
                {
                    if (List20.Count - 1 < nInt)
                    {
                        nInt = HUtil32._MAX(0, List20.Count - 10);
                    }
                    for (var j = List20.Count - 1; j >= 0; j--)
                    {
                        UserItem = List20[j];
                        TClientItem ClientItem = new TClientItem();
                        Item.GetStandardItem(ref ClientItem.Item);
                        Item.GetItemAddValue(UserItem, ref ClientItem.Item);
                        ClientItem.Dura = UserItem.Dura;
                        ClientItem.DuraMax = (ushort)GetUserPrice(PlayObject, GetUserItemPrice(UserItem));
                        ClientItem.MakeIndex = UserItem.MakeIndex;
                        sSendMsg = sSendMsg + EDcode.EncodeBuffer(ClientItem) + "/";
                        nItemCount++;
                        if (nItemCount >= 10)
                        {
                            break;
                        }
                    }
                    break;
                }
            }
            PlayObject.SendMsg(this, Grobal2.RM_SENDDETAILGOODSLIST, 0, ObjectId, nItemCount, nInt, sSendMsg);
        }

        public void ClientQuerySellPrice(TPlayObject PlayObject, TUserItem UserItem)
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

        private bool ClientSellItem_sub_4A1C84(TUserItem UserItem)
        {
            var result = true;
            var StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
            if (StdItem != null && (StdItem.StdMode == 25 || StdItem.StdMode == 30))
            {
                if (UserItem.Dura < 4000)
                {
                    result = false;
                }
            }
            return result;
        }

        public bool ClientSellItem(TPlayObject PlayObject, TUserItem UserItem)
        {
            var result = false;
            StdItem StdItem;
            var nPrice = GetSellItemPrice(GetUserItemPrice(UserItem));
            if (nPrice > 0 && ClientSellItem_sub_4A1C84(UserItem))
            {
                if (PlayObject.IncGold(nPrice))
                {
                    if (m_boCastle || M2Share.g_Config.boGetAllNpcTax)
                    {
                        if (m_Castle != null)
                        {
                            m_Castle.IncRateGold(nPrice);
                        }
                        else if (M2Share.g_Config.boGetAllNpcTax)
                        {
                            M2Share.CastleManager.IncRateGold(M2Share.g_Config.nUpgradeWeaponPrice);
                        }
                    }
                    PlayObject.SendMsg(this, Grobal2.RM_USERSELLITEM_OK, 0, PlayObject.m_nGold, 0, 0, "");
                    AddItemToGoodsList(UserItem);
                    StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                    if (StdItem.NeedIdentify == 1)
                    {
                        M2Share.AddGameDataLog("10" + "\t" + PlayObject.m_sMapName + "\t" + PlayObject.m_nCurrX + "\t" + PlayObject.m_nCurrY + "\t" + PlayObject.m_sCharName + "\t" + StdItem.Name + "\t" + UserItem.MakeIndex + "\t" + '1' + "\t" + m_sCharName);
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

        private bool AddItemToGoodsList(TUserItem UserItem)
        {
            var result = false;
            if (UserItem.Dura <= 0)
            {
                return result;
            }
            var ItemList = GetRefillList(UserItem.wIndex);
            if (ItemList == null)
            {
                ItemList = new List<TUserItem>();
                m_GoodsList.Add(ItemList);
            }
            ItemList.Insert(0, UserItem);
            result = true;
            return result;
        }

        private bool ClientMakeDrugItem_sub_4A28FC(TPlayObject PlayObject, string sItemName)
        {
            bool result = false;
            IList<TMakeItem> List10 = M2Share.GetMakeItemInfo(sItemName);
            TUserItem UserItem = null;
            IList<TDeleteItem> List28;
            string s20 = string.Empty;
            int n1C = 0;
            if (List10 == null)
            {
                return result;
            }
            result = true;
            for (var i = 0; i < List10.Count; i++)
            {
                s20 = List10[i].ItemName;
                n1C = List10[i].ItemCount;
                for (var j = 0; j < PlayObject.m_ItemList.Count; j++)
                {
                    if (M2Share.UserEngine.GetStdItemName(PlayObject.m_ItemList[j].wIndex) == s20)
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
                    for (var j = PlayObject.m_ItemList.Count - 1; j >= 0; j--)
                    {
                        if (n1C <= 0)
                        {
                            break;
                        }
                        UserItem = PlayObject.m_ItemList[j];
                        if (M2Share.UserEngine.GetStdItemName(UserItem.wIndex) == s20)
                        {
                            if (List28 == null)
                            {
                                List28 = new List<TDeleteItem>();
                            }
                            List28.Add(new TDeleteItem()
                            {
                                sItemName = s20,
                                MakeIndex = UserItem.MakeIndex
                            });
                            Dispose(UserItem);
                            PlayObject.m_ItemList.RemoveAt(j);
                            n1C -= 1;
                        }
                    }
                }
                if (List28 != null)
                {
                    var ObjectId = HUtil32.Sequence();
                    M2Share.ActorManager.AddOhter(ObjectId, List28);
                    PlayObject.SendMsg(this, Grobal2.RM_SENDDELITEMLIST, 0, ObjectId, 0, 0, "");
                }
            }
            return result;
        }

        public void ClientMakeDrugItem(TPlayObject PlayObject, string sItemName)
        {
            IList<TUserItem> List1C;
            TUserItem MakeItem;
            TUserItem UserItem;
            StdItem StdItem;
            var n14 = 1;
            for (var i = 0; i < m_GoodsList.Count; i++)
            {
                List1C = m_GoodsList[i];
                MakeItem = List1C[0];
                StdItem = M2Share.UserEngine.GetStdItem(MakeItem.wIndex);
                if (StdItem != null && StdItem.Name == sItemName)
                {
                    if (PlayObject.m_nGold >= M2Share.g_Config.nMakeDurgPrice)
                    {
                        if (ClientMakeDrugItem_sub_4A28FC(PlayObject, sItemName))
                        {
                            UserItem = new TUserItem();
                            M2Share.UserEngine.CopyToUserItemFromName(sItemName, ref UserItem);
                            if (PlayObject.AddItemToBag(UserItem))
                            {
                                PlayObject.m_nGold -= M2Share.g_Config.nMakeDurgPrice;
                                PlayObject.SendAddItem(UserItem);
                                StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                                if (StdItem.NeedIdentify == 1)
                                {
                                    M2Share.AddGameDataLog('2' + "\t" + PlayObject.m_sMapName + "\t" + PlayObject.m_nCurrX + "\t" + PlayObject.m_nCurrY + "\t" + PlayObject.m_sCharName + "\t" + StdItem.Name + "\t" + UserItem.MakeIndex + "\t" + '1' + "\t" + m_sCharName);
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
                PlayObject.SendMsg(this, Grobal2.RM_MAKEDRUG_SUCCESS, 0, PlayObject.m_nGold, 0, 0, "");
            }
            else
            {
                PlayObject.SendMsg(this, Grobal2.RM_MAKEDRUG_FAIL, 0, n14, 0, 0, "");
            }
        }

        /// <summary>
        /// 客户查询修复所需成本
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="UserItem"></param>
        public void ClientQueryRepairCost(TPlayObject PlayObject, TUserItem UserItem)
        {
            int nRepairPrice;
            var nPrice = GetUserPrice(PlayObject, GetUserItemPrice(UserItem));
            if (nPrice > 0 && UserItem.DuraMax > UserItem.Dura)
            {
                if (UserItem.DuraMax > 0)
                {
                    nRepairPrice = HUtil32.Round(((double)(nPrice / 3) / UserItem.DuraMax) * (UserItem.DuraMax - UserItem.Dura));
                }
                else
                {
                    nRepairPrice = nPrice;
                }
                if (PlayObject.m_sScriptLable == ScriptConst.sSUPERREPAIR)
                {
                    if (m_boS_repair)
                    {
                        nRepairPrice = nRepairPrice * M2Share.g_Config.nSuperRepairPriceRate;
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
        public bool ClientRepairItem(TPlayObject PlayObject, TUserItem UserItem)
        {
            int nRepairPrice;
            var result = false;
            var boCanRepair = true;
            if (PlayObject.m_sScriptLable == ScriptConst.sSUPERREPAIR && !m_boS_repair)
            {
                boCanRepair = false;
            }
            if (PlayObject.m_sScriptLable != ScriptConst.sSUPERREPAIR && !m_boRepair)
            {
                boCanRepair = false;
            }
            if (PlayObject.m_sScriptLable == "@fail_s_repair")
            {
                SendMsgToUser(PlayObject, "对不起!我不能帮你修理这个物品。\\ \\ \\<返回/@main>");
                PlayObject.SendMsg(this, Grobal2.RM_USERREPAIRITEM_FAIL, 0, 0, 0, 0, "");
                return result;
            }
            var nPrice = GetUserPrice(PlayObject, GetUserItemPrice(UserItem));
            if (PlayObject.m_sScriptLable == ScriptConst.sSUPERREPAIR)
            {
                nPrice = nPrice * M2Share.g_Config.nSuperRepairPriceRate;
            }
            StdItem StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
            if (StdItem != null)
            {
                if (boCanRepair && nPrice > 0 && UserItem.DuraMax > UserItem.Dura && StdItem.StdMode != 43)
                {
                    if (UserItem.DuraMax > 0)
                    {
                        nRepairPrice = HUtil32.Round(((nPrice / 3) / UserItem.DuraMax) * (UserItem.DuraMax - UserItem.Dura));
                    }
                    else
                    {
                        nRepairPrice = nPrice;
                    }
                    if (PlayObject.DecGold(nRepairPrice))
                    {
                        if (m_boCastle || M2Share.g_Config.boGetAllNpcTax)
                        {
                            if (m_Castle != null)
                            {
                                m_Castle.IncRateGold(nRepairPrice);
                            }
                            else if (M2Share.g_Config.boGetAllNpcTax)
                            {
                                M2Share.CastleManager.IncRateGold(M2Share.g_Config.nUpgradeWeaponPrice);
                            }
                        }
                        if (PlayObject.m_sScriptLable == ScriptConst.sSUPERREPAIR)
                        {
                            UserItem.Dura = UserItem.DuraMax;
                            PlayObject.SendMsg(this, Grobal2.RM_USERREPAIRITEM_OK, 0, PlayObject.m_nGold, UserItem.Dura, UserItem.DuraMax, "");
                            GotoLable(PlayObject, ScriptConst.sSUPERREPAIROK, false);
                        }
                        else
                        {
                            UserItem.DuraMax -= (ushort)((UserItem.DuraMax - UserItem.Dura) / M2Share.g_Config.nRepairItemDecDura);
                            UserItem.Dura = UserItem.DuraMax;
                            PlayObject.SendMsg(this, Grobal2.RM_USERREPAIRITEM_OK, 0, PlayObject.m_nGold, UserItem.Dura, UserItem.DuraMax, "");
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
            for (var i = 0; i < m_UpgradeWeaponList.Count; i++)
            {
                m_UpgradeWeaponList[i] = null;
            }
            m_UpgradeWeaponList.Clear();
            try
            {
                M2Share.LocalDB.LoadUpgradeWeaponRecord(m_sScript + '-' + m_sMapName, m_UpgradeWeaponList);
            }
            catch
            {
                M2Share.MainOutMessage("Failure in loading upgradinglist - " + m_sCharName);
            }
        }

        /// <summary>
        /// 设置挂机留言信息
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="sMsg"></param>
        protected void SetOffLineMsg(TPlayObject PlayObject, string sMsg)
        {
            PlayObject.m_sOffLineLeaveword = sMsg;
        }

        protected override void SendCustemMsg(TPlayObject PlayObject, string sMsg)
        {
            base.SendCustemMsg(PlayObject, sMsg);
        }

        /// <summary>
        /// 清除临时文件，包括交易库存，价格表
        /// </summary>
        public void ClearData()
        {
            TUserItem UserItem;
            IList<TUserItem> ItemList;
            TItemPrice ItemPrice;
            const string sExceptionMsg = "[Exception] TMerchant::ClearData";
            try
            {
                for (var i = 0; i < m_GoodsList.Count; i++)
                {
                    ItemList = m_GoodsList[i];
                    for (var j = 0; j < ItemList.Count; j++)
                    {
                        UserItem = ItemList[j];
                        Dispose(UserItem);
                    }
                }
                m_GoodsList.Clear();
                for (var i = 0; i < m_ItemPriceList.Count; i++)
                {
                    ItemPrice = m_ItemPriceList[i];
                    Dispose(ItemPrice);
                }
                m_ItemPriceList.Clear();
                SaveNPCData();
            }
            catch (Exception e)
            {
                M2Share.ErrorMessage(sExceptionMsg);
                M2Share.ErrorMessage(e.Message);
            }
        }

        private void ChangeUseItemName(TPlayObject PlayObject, string sLabel, string sItemName)
        {
            if (!PlayObject.m_boChangeItemNameFlag)
            {
                return;
            }
            PlayObject.m_boChangeItemNameFlag = false;
            var sWhere = sLabel.Substring(ScriptConst.sUSEITEMNAME.Length, sLabel.Length - ScriptConst.sUSEITEMNAME.Length);
            var btWhere = (byte)HUtil32.Str_ToInt(sWhere, -1);
            if (btWhere >= 0 && btWhere <= PlayObject.m_UseItems.Length)
            {
                var UserItem = PlayObject.m_UseItems[btWhere];
                if (UserItem.wIndex == 0)
                {
                    var sMsg = format(M2Share.g_sYourUseItemIsNul, M2Share.GetUseItemName(btWhere));
                    PlayObject.SendMsg(this, Grobal2.RM_MENU_OK, 0, PlayObject.ObjectId, 0, 0, sMsg);
                    return;
                }
                if (UserItem.btValue[13] == 1)
                {
                    M2Share.ItemUnit.DelCustomItemName(UserItem.MakeIndex, UserItem.wIndex);
                }
                if (!string.IsNullOrEmpty(sItemName))
                {
                    M2Share.ItemUnit.AddCustomItemName(UserItem.MakeIndex, UserItem.wIndex, sItemName);
                    UserItem.btValue[13] = 1;
                }
                else
                {
                    M2Share.ItemUnit.DelCustomItemName(UserItem.MakeIndex, UserItem.wIndex);
                    UserItem.btValue[13] = 0;
                }
                M2Share.ItemUnit.SaveCustomItemName();
                PlayObject.SendMsg(PlayObject, Grobal2.RM_SENDUSEITEMS, 0, 0, 0, 0, "");
                PlayObject.SendMsg(this, Grobal2.RM_MENU_OK, 0, PlayObject.ObjectId, 0, 0, "");
            }
        }

        private void DisPose(object obj)
        {
            obj = null;
        }
    }
}