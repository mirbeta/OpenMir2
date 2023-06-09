using SystemModule;
using SystemModule.Data;
using SystemModule.Packets.ClientPackets;

namespace M2Server.Items
{
    public class ItemSystem : IItemSystem
    {
        /// <summary>
        /// 物品列表
        /// </summary>
        public readonly IList<StdItem> StdItemList;

       public ItemSystem()
        {
            StdItemList = new List<StdItem>();
        }

        public StdItem GetStdItem(ushort nItemIdx)
        {
            StdItem result = null;
            nItemIdx -= 1;
            if (nItemIdx >= 0 && StdItemList.Count > nItemIdx)
            {
                result = StdItemList[nItemIdx];
                if (string.IsNullOrEmpty(result.Name)) result = null;
            }
            return result;
        }

        public StdItem GetStdItem(string sItemName)
        {
            StdItem result = null;
            if (string.IsNullOrEmpty(sItemName)) return null;
            for (var i = 0; i < StdItemList.Count; i++)
            {
                var stdItem = StdItemList[i];
                if (string.Compare(stdItem.Name, sItemName, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    result = stdItem;
                    break;
                }
            }
            return result;
        }

        public int GetStdItemWeight(int nItemIdx)
        {
            var result = 0;
            nItemIdx -= 1;
            if (nItemIdx >= 0 && StdItemList.Count > nItemIdx)
            {
                result = StdItemList[nItemIdx].Weight;
            }
            return result;
        }

        public void AddItem(StdItem stdItem)
        {
            StdItemList.Add(stdItem);
        }

        public void Clear()
        {
            for (var i = 0; i < StdItemList.Count; i++)
            {
                StdItemList[i] = null;
            }
            StdItemList.Clear();
        }

        public int ItemCount => StdItemList.Count;

        public string GetStdItemName(int nItemIdx)
        {
            var result = string.Empty;
            nItemIdx -= 1;
            if (nItemIdx >= 0 && StdItemList.Count > nItemIdx)
            {
                result = StdItemList[nItemIdx].Name;
            }
            return result;
        }

        public ushort GetStdItemIdx(string sItemName)
        {
            ushort result = 0;
            if (string.IsNullOrEmpty(sItemName)) return result;
            for (var i = 0; i < StdItemList.Count; i++)
            {
                var stdItem = StdItemList[i];
                if (stdItem.Name.Equals(sItemName, StringComparison.OrdinalIgnoreCase))
                {
                    result = (ushort)(i + 1);
                    break;
                }
            }
            return result;
        }

        public bool CopyToUserItemFromName(string sItemName, ref UserItem item)
        {
            if (string.IsNullOrEmpty(sItemName)) return false;
            for (var i = 0; i < StdItemList.Count; i++)
            {
                var stdItem = StdItemList[i];
                if (!stdItem.Name.Equals(sItemName, StringComparison.OrdinalIgnoreCase)) continue;
                if (item == null) item = new UserItem();
                item.Index = (ushort)(i + 1);
                item.MakeIndex = M2Share.GetItemNumber();
                item.Dura = stdItem.DuraMax;
                item.DuraMax = stdItem.DuraMax;
                return true;
            }
            return false;
        }

        private int GetUpgrade(int count, int ran)
        {
            var result = 0;
            for (var i = 0; i < count; i++)
            {
                if (M2Share.RandomNumber.Random(ran) == 0)
                {
                    result = result + 1;
                }
                else
                {
                    break;
                }
            }
            return result;
        }

        public int GetUpgrade2(int x, int a)
        {
            int iProb;
            var result = 0;
            for (var i = x; i >= 1; i--)
            {
                if (i > x / 2)
                {
                    iProb = Convert.ToInt32(Math.Sqrt(Math.Pow(a, 2.0) - Math.Pow(i, 2.0)) / (a * i + Math.Pow(i, 2.0)) * 100);
                }
                else
                {
                    iProb = Convert.ToInt32(Math.Sqrt(1 - Math.Pow(i, 2.0) / Math.Pow(a, 2.0)) * 100 / Math.Sqrt(i));
                }
                if (M2Share.RandomNumber.Random(100) < iProb)
                {
                    result = i / 3;
                    break;
                }
            }
            return result;
        }

        private void UpgradeRandomWeapon(UserItem pu)
        {
            var up = GetUpgrade(12, 15);
            if (M2Share.RandomNumber.Random(15) == 0)
            {
                pu.Desc[0] = (byte)(1 + up);//DC
            }
            up = GetUpgrade(12, 15);
            if (M2Share.RandomNumber.Random(20) == 0)
            {
                var incp = (1 + up) / 3;
                if (incp > 0)
                {
                    if (M2Share.RandomNumber.Random(3) != 0)
                    {
                        pu.Desc[6] = (byte)incp;
                    }
                    else
                    {
                        pu.Desc[6] = (byte)(10 + incp);
                    }
                }
            }
            up = GetUpgrade(12, 15);
            if (M2Share.RandomNumber.Random(15) == 0)
            {
                pu.Desc[1] = (byte)(1 + up);//MC
            }
            up = GetUpgrade(12, 15);
            if (M2Share.RandomNumber.Random(15) == 0)
            {
                pu.Desc[2] = (byte)(1 + up);//SC
            }
            up = GetUpgrade(12, 15);
            if (M2Share.RandomNumber.Random(24) == 0)
            {
                pu.Desc[5] = (byte)(1 + up / 2);
            }
            up = GetUpgrade(12, 12);
            if (M2Share.RandomNumber.Random(3) < 2)
            {
                var n = (1 + up) * 2000;
                pu.DuraMax = (ushort)HUtil32._MIN(65000, pu.DuraMax + n);
                pu.Dura = (ushort)HUtil32._MIN(65000, pu.Dura + n);
            }
            up = GetUpgrade(12, 15);
            if (M2Share.RandomNumber.Random(10) == 0)
            {
                pu.Desc[7] = (byte)(1 + up / 2);
            }
        }

        private void UpgradeRandomDress(UserItem pu)
        {
            var up = GetUpgrade(6, 15);
            if (M2Share.RandomNumber.Random(30) == 0)
            {
                pu.Desc[0] = (byte)(1 + up);// AC
            }
            up = GetUpgrade(6, 15);
            if (M2Share.RandomNumber.Random(30) == 0)
            {
                pu.Desc[1] = (byte)(1 + up); // MAC
            }
            up = GetUpgrade(6, 20);
            if (M2Share.RandomNumber.Random(40) == 0)
            {
                pu.Desc[2] = (byte)(1 + up);// DC
            }
            up = GetUpgrade(6, 20);
            if (M2Share.RandomNumber.Random(40) == 0)
            {
                pu.Desc[3] = (byte)(1 + up); // MC
            }
            up = GetUpgrade(6, 20);
            if (M2Share.RandomNumber.Random(40) == 0)
            {
                pu.Desc[4] = (byte)(1 + up);// SC
            }
            up = GetUpgrade(6, 10);
            if (M2Share.RandomNumber.Random(8) < 6)
            {
                var n = (1 + up) * 2000;
                pu.DuraMax = (ushort)HUtil32._MIN(65000, pu.DuraMax + n);
                pu.Dura = (ushort)HUtil32._MIN(65000, pu.Dura + n);
            }
        }

        private void UpgradeRandomNecklace(UserItem pu)
        {
            var up = GetUpgrade(6, 30);
            if (M2Share.RandomNumber.Random(60) == 0)
            {
                pu.Desc[0] = (byte)(1 + up);// AC(HIT)
            }
            up = GetUpgrade(6, 30);
            if (M2Share.RandomNumber.Random(60) == 0)
            {
                pu.Desc[1] = (byte)(1 + up);// MAC(SPEED)
            }
            up = GetUpgrade(6, 20);
            if (M2Share.RandomNumber.Random(30) == 0)
            {
                pu.Desc[2] = (byte)(1 + up);// DC
            }
            up = GetUpgrade(6, 20);
            if (M2Share.RandomNumber.Random(30) == 0)
            {
                pu.Desc[3] = (byte)(1 + up);// MC
            }
            up = GetUpgrade(6, 20);
            if (M2Share.RandomNumber.Random(30) == 0)
            {
                pu.Desc[4] = (byte)(1 + up);// SC
            }
            up = GetUpgrade(6, 12);
            if (M2Share.RandomNumber.Random(20) < 15)
            {
                var n = (1 + up) * 1000;
                pu.DuraMax = (ushort)HUtil32._MIN(65000, pu.DuraMax + n);
                pu.Dura = (ushort)HUtil32._MIN(65000, pu.Dura + n);
            }
        }

        private void UpgradeRandomBarcelet(UserItem pu)
        {
            var up = GetUpgrade(6, 20);
            if (M2Share.RandomNumber.Random(20) == 0)
            {
                pu.Desc[0] = (byte)(1 + up);// AC
            }
            up = GetUpgrade(6, 20);
            if (M2Share.RandomNumber.Random(20) == 0)
            {
                pu.Desc[1] = (byte)(1 + up);// MAC
            }
            up = GetUpgrade(6, 20);
            if (M2Share.RandomNumber.Random(30) == 0)
            {
                pu.Desc[2] = (byte)(1 + up);// DC
            }
            up = GetUpgrade(6, 20);
            if (M2Share.RandomNumber.Random(30) == 0)
            {
                pu.Desc[3] = (byte)(1 + up);// MC
            }
            up = GetUpgrade(6, 20);
            if (M2Share.RandomNumber.Random(30) == 0)
            {
                pu.Desc[4] = (byte)(1 + up);// SC
            }
            up = GetUpgrade(6, 12);
            if (M2Share.RandomNumber.Random(20) < 15)
            {
                var n = (1 + up) * 1000;
                pu.DuraMax = (ushort)HUtil32._MIN(65000, pu.DuraMax + n);
                pu.Dura = (ushort)HUtil32._MIN(65000, pu.Dura + n);
            }
        }

        private void UpgradeRandomNecklace19(UserItem pu)
        {
            var up = GetUpgrade(6, 20);
            if (M2Share.RandomNumber.Random(40) == 0)
            {
                pu.Desc[0] = (byte)(1 + up);
            }
            up = GetUpgrade(6, 20);
            if (M2Share.RandomNumber.Random(40) == 0)
            {
                pu.Desc[1] = (byte)(1 + up);
            }
            up = GetUpgrade(6, 20);
            if (M2Share.RandomNumber.Random(30) == 0)
            {
                pu.Desc[2] = (byte)(1 + up); // DC
            }
            up = GetUpgrade(6, 20);
            if (M2Share.RandomNumber.Random(30) == 0)
            {
                pu.Desc[3] = (byte)(1 + up);// MC
            }
            up = GetUpgrade(6, 20);
            if (M2Share.RandomNumber.Random(30) == 0)
            {
                pu.Desc[4] = (byte)(1 + up);// SC
            }
            up = GetUpgrade(6, 10);
            if (M2Share.RandomNumber.Random(4) < 3)
            {
                var n = (1 + up) * 1000;
                pu.DuraMax = (ushort)HUtil32._MIN(65000, pu.DuraMax + n);
                pu.Dura = (ushort)HUtil32._MIN(65000, pu.Dura + n);
            }
        }

        private void UpgradeRandomRings(UserItem pu)
        {
            var up = GetUpgrade(6, 20);
            if (M2Share.RandomNumber.Random(30) == 0)
            {
                pu.Desc[2] = (byte)(1 + up); // DC
            }
            up = GetUpgrade(6, 20);
            if (M2Share.RandomNumber.Random(30) == 0)
            {
                pu.Desc[3] = (byte)(1 + up);// MC
            }
            up = GetUpgrade(6, 20);
            if (M2Share.RandomNumber.Random(30) == 0)
            {
                pu.Desc[4] = (byte)(1 + up);// SC
            }
            up = GetUpgrade(6, 12);
            if (M2Share.RandomNumber.Random(4) < 3)
            {
                var n = (1 + up) * 1000;
                pu.DuraMax = (ushort)HUtil32._MIN(65000, pu.DuraMax + n);
                pu.Dura = (ushort)HUtil32._MIN(65000, pu.Dura + n);
            }
        }

        private void UpgradeRandomRings23(UserItem pu)
        {
            var up = GetUpgrade(6, 20);
            if (M2Share.RandomNumber.Random(40) == 0)
            {
                pu.Desc[0] = (byte)(1 + up);
            }
            up = GetUpgrade(6, 20);
            if (M2Share.RandomNumber.Random(40) == 0)
            {
                pu.Desc[1] = (byte)(1 + up);
            }
            up = GetUpgrade(6, 20);
            if (M2Share.RandomNumber.Random(30) == 0)
            {
                pu.Desc[2] = (byte)(1 + up);// DC
            }
            up = GetUpgrade(6, 20);
            if (M2Share.RandomNumber.Random(30) == 0)
            {
                pu.Desc[3] = (byte)(1 + up);// MC
            }
            up = GetUpgrade(6, 20);
            if (M2Share.RandomNumber.Random(30) == 0)
            {
                pu.Desc[4] = (byte)(1 + up); // SC
            }
            up = GetUpgrade(6, 12);
            if (M2Share.RandomNumber.Random(4) < 3)
            {
                var n = (1 + up) * 1000;
                pu.DuraMax = (ushort)HUtil32._MIN(65000, pu.DuraMax + n);
                pu.Dura = (ushort)HUtil32._MIN(65000, pu.Dura + n);
            }
        }

        private void UpgradeRandomHelmet(UserItem pu)
        {
            var up = GetUpgrade(6, 20);
            if (M2Share.RandomNumber.Random(40) == 0)
            {
                pu.Desc[0] = (byte)(1 + up);// AC
            }
            up = GetUpgrade(6, 20);
            if (M2Share.RandomNumber.Random(30) == 0)
            {
                pu.Desc[1] = (byte)(1 + up);// MAC
            }
            up = GetUpgrade(6, 20);
            if (M2Share.RandomNumber.Random(30) == 0)
            {
                pu.Desc[2] = (byte)(1 + up);// DC
            }
            up = GetUpgrade(6, 20);
            if (M2Share.RandomNumber.Random(30) == 0)
            {
                pu.Desc[3] = (byte)(1 + up);// MC
            }
            up = GetUpgrade(6, 20);
            if (M2Share.RandomNumber.Random(30) == 0)
            {
                pu.Desc[4] = (byte)(1 + up);// SC
            }
            up = GetUpgrade(6, 12);
            if (M2Share.RandomNumber.Random(4) < 3)
            {
                var n = (1 + up) * 1000;
                pu.DuraMax = (ushort)HUtil32._MIN(65000, pu.DuraMax + n);
                pu.Dura = (ushort)HUtil32._MIN(65000, pu.Dura + n);
            }
        }

        private void RandomSetUnknownHelmet(UserItem pu)
        {
            var up = GetUpgrade(4, 3) + GetUpgrade(4, 8) + GetUpgrade(4, 20);
            if (up > 0)
            {
                pu.Desc[0] = (byte)up;// AC
            }
            var sum = up;
            up = GetUpgrade(4, 3) + GetUpgrade(4, 8) + GetUpgrade(4, 20);
            if (up > 0)
            {
                pu.Desc[1] = (byte)up;// MAC
            }
            sum = sum + up;
            up = GetUpgrade(3, 15) + GetUpgrade(3, 30);
            if (up > 0)
            {
                pu.Desc[2] = (byte)up;// DC
            }
            sum = sum + up;
            up = GetUpgrade(3, 15) + GetUpgrade(3, 30);
            if (up > 0)
            {
                pu.Desc[3] = (byte)up;// MC
            }
            sum = sum + up;
            up = GetUpgrade(3, 15) + GetUpgrade(3, 30);
            if (up > 0)
            {
                pu.Desc[4] = (byte)up;// SC
            }
            sum = sum + up;
            up = GetUpgrade(6, 30);
            if (up > 0)
            {
                var n = (1 + up) * 1000;
                pu.DuraMax = (ushort)HUtil32._MIN(65000, pu.DuraMax + n);
                pu.Dura = (ushort)HUtil32._MIN(65000, pu.Dura + n);
            }
            if (M2Share.RandomNumber.Random(30) == 0)
            {
                pu.Desc[7] = 1;
            }
            pu.Desc[8] = 1;
            if (sum >= 3)
            {
                if (pu.Desc[0] >= 5)
                {
                    pu.Desc[5] = 1;
                    pu.Desc[6] = (byte)(25 + pu.Desc[0] * 3);
                    return;
                }
                if (pu.Desc[2] >= 2)
                {
                    pu.Desc[5] = 1;
                    pu.Desc[6] = (byte)(35 + pu.Desc[2] * 4);
                    return;
                }
                if (pu.Desc[3] >= 2)
                {
                    pu.Desc[5] = 2;
                    pu.Desc[6] = (byte)(18 + pu.Desc[3] * 2);
                    return;
                }
                if (pu.Desc[4] >= 2)
                {
                    pu.Desc[5] = 3;
                    pu.Desc[6] = (byte)(18 + pu.Desc[4] * 2);
                    return;
                }
                pu.Desc[6] = (byte)(18 + sum * 2);
            }
        }

        private void RandomSetUnknownRing(UserItem pu)
        {
            var up = GetUpgrade(3, 4) + GetUpgrade(3, 8) + GetUpgrade(6, 20);
            if (up > 0)
            {
                pu.Desc[2] = (byte)up;// DC
            }
            var sum = up;
            up = GetUpgrade(3, 4) + GetUpgrade(3, 8) + GetUpgrade(6, 20);
            if (up > 0)
            {
                pu.Desc[3] = (byte)up;// MC
            }
            sum = sum + up;
            up = GetUpgrade(3, 4) + GetUpgrade(3, 8) + GetUpgrade(6, 20);
            if (up > 0)
            {
                pu.Desc[4] = (byte)up;// SC
            }
            sum = sum + up;
            up = GetUpgrade(6, 30);
            if (up > 0)
            {
                var n = (1 + up) * 1000;
                pu.DuraMax = (ushort)HUtil32._MIN(65000, pu.DuraMax + n);
                pu.Dura = (ushort)HUtil32._MIN(65000, pu.Dura + n);
            }
            if (M2Share.RandomNumber.Random(30) == 0)
            {
                pu.Desc[7] = 1;
            }
            pu.Desc[8] = 1;
            if (sum >= 3)
            {
                if (pu.Desc[2] >= 3)
                {
                    pu.Desc[5] = 1;
                    pu.Desc[6] = (byte)(25 + pu.Desc[2] * 3);
                    return;
                }
                if (pu.Desc[3] >= 3)
                {
                    pu.Desc[5] = 2;
                    pu.Desc[6] = (byte)(18 + pu.Desc[3] * 2);
                    return;
                }
                if (pu.Desc[4] >= 3)
                {
                    pu.Desc[5] = 3;
                    pu.Desc[6] = (byte)(18 + pu.Desc[4] * 2);
                    return;
                }
                pu.Desc[6] = (byte)(18 + sum * 2);
            }
        }

        private void RandomSetUnknownBracelet(UserItem pu)
        {
            var up = GetUpgrade(3, 5) + GetUpgrade(5, 20);
            if (up > 0)
            {
                pu.Desc[0] = (byte)up; // AC
            }
            var sum = up;
            up = GetUpgrade(3, 5) + GetUpgrade(5, 20);
            if (up > 0)
            {
                pu.Desc[1] = (byte)up;// MAC
            }
            sum = sum + up;
            up = GetUpgrade(3, 15) + GetUpgrade(5, 30);
            if (up > 0)
            {
                pu.Desc[2] = (byte)up;// DC
            }
            sum = sum + up;
            up = GetUpgrade(3, 15) + GetUpgrade(5, 30);
            if (up > 0)
            {
                pu.Desc[3] = (byte)up;// MC
            }
            sum = sum + up;
            up = GetUpgrade(3, 15) + GetUpgrade(5, 30);
            if (up > 0)
            {
                pu.Desc[4] = (byte)up;// SC
            }
            sum = sum + up;
            up = GetUpgrade(6, 30);
            if (up > 0)
            {
                var n = (1 + up) * 1000;
                pu.DuraMax = (ushort)HUtil32._MIN(65000, pu.DuraMax + n);
                pu.Dura = (ushort)HUtil32._MIN(65000, pu.Dura + n);
            }
            if (M2Share.RandomNumber.Random(30) == 0)
            {
                pu.Desc[7] = 1;
            }
            pu.Desc[8] = 1;
            if (sum >= 2)
            {
                if (pu.Desc[0] >= 3)
                {
                    pu.Desc[5] = 1;
                    pu.Desc[6] = (byte)(25 + pu.Desc[0] * 3);
                    return;
                }
                if (pu.Desc[2] >= 2)
                {
                    pu.Desc[5] = 1;
                    pu.Desc[6] = (byte)(30 + pu.Desc[2] * 3);
                    return;
                }
                if (pu.Desc[3] >= 2)
                {
                    pu.Desc[5] = 2;
                    pu.Desc[6] = (byte)(20 + pu.Desc[3] * 2);
                    return;
                }
                if (pu.Desc[4] >= 2)
                {
                    pu.Desc[5] = 3;
                    pu.Desc[6] = (byte)(20 + pu.Desc[4] * 2);
                    return;
                }
                pu.Desc[6] = (byte)(18 + sum * 2);
            }
        }

        private void CopyItemToClientItem(StdItem stdItem, ref ClientItem clientStdItem)
        {
            if (clientStdItem == null)
            {
                clientStdItem = new ClientItem();
            }
            clientStdItem.Item.Name = stdItem.Name;
            clientStdItem.Item.StdMode = stdItem.StdMode;
            clientStdItem.Item.Shape = stdItem.Shape;
            clientStdItem.Item.Weight = stdItem.Weight;
            clientStdItem.Item.AniCount = stdItem.AniCount;
            clientStdItem.Item.SpecialPwr = stdItem.SpecialPwr;
            clientStdItem.Item.ItemDesc = stdItem.ItemDesc;
            clientStdItem.Item.Looks = stdItem.Looks;
            clientStdItem.Item.DuraMax = stdItem.DuraMax;
            clientStdItem.Item.AC = stdItem.AC;
            clientStdItem.Item.MAC = stdItem.MAC;
            clientStdItem.Item.DC = stdItem.DC;
            clientStdItem.Item.MC = stdItem.MC;
            clientStdItem.Item.SC = stdItem.SC;
            clientStdItem.Item.Need = stdItem.Need;
            clientStdItem.Item.NeedLevel = stdItem.NeedLevel;
            clientStdItem.Item.NeedIdentify = stdItem.NeedIdentify;
            clientStdItem.Item.Price = stdItem.Price;
            clientStdItem.Item.Stock = stdItem.Stock;
            clientStdItem.Item.AtkSpd = stdItem.AtkSpd;
            clientStdItem.Item.Agility = stdItem.Agility;
            clientStdItem.Item.Accurate = stdItem.Accurate;
            clientStdItem.Item.MgAvoid = stdItem.MgAvoid;
            clientStdItem.Item.Strong = stdItem.Strong;
            clientStdItem.Item.Undead = stdItem.Undead;
            clientStdItem.Item.HpAdd = stdItem.HpAdd;
            clientStdItem.Item.MpAdd = stdItem.MpAdd;
            clientStdItem.Item.ExpAdd = stdItem.ExpAdd;
            clientStdItem.Item.EffType1 = stdItem.EffType1;
            clientStdItem.Item.EffRate1 = stdItem.EffRate1;
            clientStdItem.Item.EffValue1 = stdItem.EffValue1;
            clientStdItem.Item.EffType2 = stdItem.EffType2;
            clientStdItem.Item.EffRate2 = stdItem.EffRate2;
            clientStdItem.Item.EffValue2 = stdItem.EffValue2;
            clientStdItem.Item.Slowdown = stdItem.Slowdown;
            clientStdItem.Item.Tox = stdItem.Tox;
            clientStdItem.Item.ToxAvoid = stdItem.ToxAvoid;
            clientStdItem.Item.UniqueItem = stdItem.UniqueItem;
            clientStdItem.Item.OverlapItem = stdItem.OverlapItem;
            clientStdItem.Item.Light = stdItem.Light;
            clientStdItem.Item.ItemType = stdItem.ItemType;
            clientStdItem.Item.ItemSet = stdItem.ItemSet;
            clientStdItem.Item.Reference = stdItem.Reference;
        }

        public int GetUpgradeStdItem(StdItem stdItem, UserItem userItem, ref ClientItem clientItem)
        {
            CopyItemToClientItem(stdItem, ref clientItem);
            var count = 0;
            switch (stdItem.StdMode)
            {
                case 5:
                case 6:
                    clientItem.Item.DC = HUtil32.MakeWord(HUtil32.LoByte(stdItem.DC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(stdItem.DC) + userItem.Desc[0]));
                    clientItem.Item.MC = HUtil32.MakeWord(HUtil32.LoByte(stdItem.MC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(stdItem.MC) + userItem.Desc[1]));
                    clientItem.Item.SC = HUtil32.MakeWord(HUtil32.LoByte(stdItem.SC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(stdItem.SC) + userItem.Desc[2]));
                    clientItem.Item.AC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(stdItem.AC) + userItem.Desc[3]), (ushort)(HUtil32.HiByte(stdItem.AC) + userItem.Desc[5]));
                    clientItem.Item.MAC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(stdItem.MAC) + userItem.Desc[4]), HUtil32.HiByte(stdItem.MAC));
                    clientItem.Item.MAC = HUtil32.MakeWord(HUtil32.LoByte(stdItem.MAC), GetAttackSpeed(HUtil32.HiByte(stdItem.MAC), userItem.Desc[6]));
                    if (userItem.Desc[7] >= 1 && userItem.Desc[7] <= 10)
                    {
                        if (clientItem.Item.SpecialPwr >= 0)
                        {
                            clientItem.Item.SpecialPwr = (sbyte)userItem.Desc[7];
                        }
                    }
                    if (userItem.Desc[10] != 0)
                    {
                        clientItem.Item.ItemDesc = (byte)(stdItem.ItemDesc | 0x01);
                    }
                    clientItem.Item.Slowdown = (byte)(stdItem.Slowdown + userItem.Desc[12]);
                    clientItem.Item.Tox = (byte)(stdItem.Tox + userItem.Desc[13]);
                    if (userItem.Desc[0] > 0)
                    {
                        count++;
                    }
                    if (userItem.Desc[1] > 0)
                    {
                        count++;
                    }
                    if (userItem.Desc[2] > 0)
                    {
                        count++;
                    }
                    if (userItem.Desc[3] > 0)
                    {
                        count++;
                    }
                    if (userItem.Desc[4] > 0)
                    {
                        count++;
                    }
                    if (userItem.Desc[5] > 0)
                    {
                        count++;
                    }
                    if (userItem.Desc[6] > 0)
                    {
                        count++;
                    }
                    if (userItem.Desc[7] > 0)
                    {
                        count++;
                    }
                    if (userItem.Desc[12] > 0)
                    {
                        count++;
                    }
                    if (userItem.Desc[13] > 0)
                    {
                        count++;
                    }
                    break;
                case 10:
                case 11:
                    clientItem.Item.AC = HUtil32.MakeWord(HUtil32.LoByte(stdItem.AC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(stdItem.AC) + userItem.Desc[0]));
                    clientItem.Item.MAC = HUtil32.MakeWord(HUtil32.LoByte(stdItem.MAC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(stdItem.MAC) + userItem.Desc[1]));
                    clientItem.Item.DC = HUtil32.MakeWord(HUtil32.LoByte(stdItem.DC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(stdItem.DC) + userItem.Desc[2]));
                    clientItem.Item.MC = HUtil32.MakeWord(HUtil32.LoByte(stdItem.MC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(stdItem.MC) + userItem.Desc[3]));
                    clientItem.Item.SC = HUtil32.MakeWord(HUtil32.LoByte(stdItem.SC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(stdItem.SC) + userItem.Desc[4]));
                    clientItem.Item.Agility = (byte)(stdItem.Agility + userItem.Desc[11]);
                    clientItem.Item.MgAvoid = (byte)(stdItem.MgAvoid + userItem.Desc[12]);
                    clientItem.Item.ToxAvoid = (byte)(stdItem.ToxAvoid + userItem.Desc[13]);
                    if (userItem.Desc[0] > 0)
                    {
                        count++;
                    }
                    if (userItem.Desc[1] > 0)
                    {
                        count++;
                    }
                    if (userItem.Desc[2] > 0)
                    {
                        count++;
                    }
                    if (userItem.Desc[3] > 0)
                    {
                        count++;
                    }
                    if (userItem.Desc[4] > 0)
                    {
                        count++;
                    }
                    if (userItem.Desc[11] > 0)
                    {
                        count++;
                    }
                    if (userItem.Desc[12] > 0)
                    {
                        count++;
                    }
                    if (userItem.Desc[13] > 0)
                    {
                        count++;
                    }
                    break;
                case 15:
                    clientItem.Item.AC = HUtil32.MakeWord(HUtil32.LoByte(stdItem.AC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(stdItem.AC) + userItem.Desc[0]));
                    clientItem.Item.MAC = HUtil32.MakeWord(HUtil32.LoByte(stdItem.MAC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(stdItem.MAC) + userItem.Desc[1]));
                    clientItem.Item.DC = HUtil32.MakeWord(HUtil32.LoByte(stdItem.DC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(stdItem.DC) + userItem.Desc[2]));
                    clientItem.Item.MC = HUtil32.MakeWord(HUtil32.LoByte(stdItem.MC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(stdItem.MC) + userItem.Desc[3]));
                    clientItem.Item.SC = HUtil32.MakeWord(HUtil32.LoByte(stdItem.SC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(stdItem.SC) + userItem.Desc[4]));
                    clientItem.Item.Accurate = (byte)(stdItem.Accurate + userItem.Desc[11]);
                    clientItem.Item.MgAvoid = (byte)(stdItem.MgAvoid + userItem.Desc[12]);
                    clientItem.Item.ToxAvoid = (byte)(stdItem.ToxAvoid + userItem.Desc[13]);
                    if (userItem.Desc[5] > 0)
                    {
                        clientItem.Item.Need = userItem.Desc[5];
                    }
                    if (userItem.Desc[6] > 0)
                    {
                        clientItem.Item.NeedLevel = userItem.Desc[6];
                    }
                    if (userItem.Desc[0] > 0)
                    {
                        count++;
                    }
                    if (userItem.Desc[1] > 0)
                    {
                        count++;
                    }
                    if (userItem.Desc[2] > 0)
                    {
                        count++;
                    }
                    if (userItem.Desc[3] > 0)
                    {
                        count++;
                    }
                    if (userItem.Desc[4] > 0)
                    {
                        count++;
                    }
                    if (userItem.Desc[11] > 0)
                    {
                        count++;
                    }
                    if (userItem.Desc[12] > 0)
                    {
                        count++;
                    }
                    if (userItem.Desc[13] > 0)
                    {
                        count++;
                    }
                    break;
                case 19:
                case 20:
                case 21:
                    clientItem.Item.AC = HUtil32.MakeWord(HUtil32.LoByte(stdItem.AC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(stdItem.AC) + userItem.Desc[0]));
                    clientItem.Item.MAC = HUtil32.MakeWord(HUtil32.LoByte(stdItem.MAC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(stdItem.MAC) + userItem.Desc[1]));
                    clientItem.Item.DC = HUtil32.MakeWord(HUtil32.LoByte(stdItem.DC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(stdItem.DC) + userItem.Desc[2]));
                    clientItem.Item.MC = HUtil32.MakeWord(HUtil32.LoByte(stdItem.MC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(stdItem.MC) + userItem.Desc[3]));
                    clientItem.Item.SC = HUtil32.MakeWord(HUtil32.LoByte(stdItem.SC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(stdItem.SC) + userItem.Desc[4]));
                    clientItem.Item.AtkSpd = (byte)(stdItem.AtkSpd + userItem.Desc[9]);
                    clientItem.Item.Slowdown = (byte)(stdItem.Slowdown + userItem.Desc[12]);
                    clientItem.Item.Tox = (byte)(stdItem.Tox + userItem.Desc[13]);
                    if (stdItem.StdMode == 19)
                    {
                        clientItem.Item.Accurate = (byte)(stdItem.Accurate + userItem.Desc[11]);
                    }
                    else if (stdItem.StdMode == 20)
                    {
                        clientItem.Item.MgAvoid = (byte)(stdItem.MgAvoid + userItem.Desc[11]);
                    }
                    else if (stdItem.StdMode == 21)
                    {
                        clientItem.Item.Accurate = (byte)(stdItem.Accurate + userItem.Desc[11]);
                        clientItem.Item.MgAvoid = (byte)(stdItem.MgAvoid + userItem.Desc[7]);
                    }
                    if (userItem.Desc[5] > 0)
                    {
                        clientItem.Item.Need = userItem.Desc[5];
                    }
                    if (userItem.Desc[6] > 0)
                    {
                        clientItem.Item.NeedLevel = userItem.Desc[6];
                    }
                    if (userItem.Desc[0] > 0)
                    {
                        count++;
                    }
                    if (userItem.Desc[1] > 0)
                    {
                        count++;
                    }
                    if (userItem.Desc[2] > 0)
                    {
                        count++;
                    }
                    if (userItem.Desc[3] > 0)
                    {
                        count++;
                    }
                    if (userItem.Desc[4] > 0)
                    {
                        count++;
                    }
                    if (userItem.Desc[9] > 0)
                    {
                        count++;
                    }
                    if (userItem.Desc[11] > 0)
                    {
                        count++;
                    }
                    if (userItem.Desc[12] > 0)
                    {
                        count++;
                    }
                    if (userItem.Desc[13] > 0)
                    {
                        count++;
                    }
                    break;
                case 22:
                case 23:
                    clientItem.Item.AC = HUtil32.MakeWord(HUtil32.LoByte(stdItem.AC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(stdItem.AC) + userItem.Desc[0]));
                    clientItem.Item.MAC = HUtil32.MakeWord(HUtil32.LoByte(stdItem.MAC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(stdItem.MAC) + userItem.Desc[1]));
                    clientItem.Item.DC = HUtil32.MakeWord(HUtil32.LoByte(stdItem.DC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(stdItem.DC) + userItem.Desc[2]));
                    clientItem.Item.MC = HUtil32.MakeWord(HUtil32.LoByte(stdItem.MC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(stdItem.MC) + userItem.Desc[3]));
                    clientItem.Item.SC = HUtil32.MakeWord(HUtil32.LoByte(stdItem.SC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(stdItem.SC) + userItem.Desc[4]));
                    clientItem.Item.AtkSpd = (byte)(stdItem.AtkSpd + userItem.Desc[9]);
                    clientItem.Item.Slowdown = (byte)(stdItem.Slowdown + userItem.Desc[12]);
                    clientItem.Item.Tox = (byte)(stdItem.Tox + userItem.Desc[13]);
                    if (userItem.Desc[5] > 0)
                    {
                        clientItem.Item.Need = userItem.Desc[5];
                    }
                    if (userItem.Desc[6] > 0)
                    {
                        clientItem.Item.NeedLevel = userItem.Desc[6];
                    }
                    if (userItem.Desc[0] > 0)
                    {
                        count++;
                    }
                    if (userItem.Desc[1] > 0)
                    {
                        count++;
                    }
                    if (userItem.Desc[2] > 0)
                    {
                        count++;
                    }
                    if (userItem.Desc[3] > 0)
                    {
                        count++;
                    }
                    if (userItem.Desc[4] > 0)
                    {
                        count++;
                    }
                    if (userItem.Desc[9] > 0)
                    {
                        count++;
                    }
                    if (userItem.Desc[12] > 0)
                    {
                        count++;
                    }
                    if (userItem.Desc[13] > 0)
                    {
                        count++;
                    }
                    break;
                case 24:
                    clientItem.Item.AC = HUtil32.MakeWord(HUtil32.LoByte(stdItem.AC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(stdItem.AC) + userItem.Desc[0]));
                    clientItem.Item.MAC = HUtil32.MakeWord(HUtil32.LoByte(stdItem.MAC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(stdItem.MAC) + userItem.Desc[1]));
                    clientItem.Item.DC = HUtil32.MakeWord(HUtil32.LoByte(stdItem.DC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(stdItem.DC) + userItem.Desc[2]));
                    clientItem.Item.MC = HUtil32.MakeWord(HUtil32.LoByte(stdItem.MC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(stdItem.MC) + userItem.Desc[3]));
                    clientItem.Item.SC = HUtil32.MakeWord(HUtil32.LoByte(stdItem.SC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(stdItem.SC) + userItem.Desc[4]));
                    if (userItem.Desc[5] > 0)
                    {
                        clientItem.Item.Need = userItem.Desc[5];
                    }
                    if (userItem.Desc[6] > 0)
                    {
                        clientItem.Item.NeedLevel = userItem.Desc[6];
                    }
                    if (userItem.Desc[0] > 0)
                    {
                        count++;
                    }
                    if (userItem.Desc[1] > 0)
                    {
                        count++;
                    }
                    if (userItem.Desc[2] > 0)
                    {
                        count++;
                    }
                    if (userItem.Desc[3] > 0)
                    {
                        count++;
                    }
                    if (userItem.Desc[4] > 0)
                    {
                        count++;
                    }
                    break;
                case 26:
                    clientItem.Item.AC = HUtil32.MakeWord(HUtil32.LoByte(stdItem.AC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(stdItem.AC) + userItem.Desc[0]));
                    clientItem.Item.MAC = HUtil32.MakeWord(HUtil32.LoByte(stdItem.MAC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(stdItem.MAC) + userItem.Desc[1]));
                    clientItem.Item.DC = HUtil32.MakeWord(HUtil32.LoByte(stdItem.DC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(stdItem.DC) + userItem.Desc[2]));
                    clientItem.Item.MC = HUtil32.MakeWord(HUtil32.LoByte(stdItem.MC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(stdItem.MC) + userItem.Desc[3]));
                    clientItem.Item.SC = HUtil32.MakeWord(HUtil32.LoByte(stdItem.SC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(stdItem.SC) + userItem.Desc[4]));
                    clientItem.Item.Accurate = (byte)(stdItem.Accurate + userItem.Desc[11]);
                    clientItem.Item.Agility = (byte)(stdItem.Agility + userItem.Desc[12]);
                    if (userItem.Desc[5] > 0)
                    {
                        clientItem.Item.Need = userItem.Desc[5];
                    }
                    if (userItem.Desc[6] > 0)
                    {
                        clientItem.Item.NeedLevel = userItem.Desc[6];
                    }
                    if (userItem.Desc[0] > 0)
                    {
                        count++;
                    }
                    if (userItem.Desc[1] > 0)
                    {
                        count++;
                    }
                    if (userItem.Desc[2] > 0)
                    {
                        count++;
                    }
                    if (userItem.Desc[3] > 0)
                    {
                        count++;
                    }
                    if (userItem.Desc[4] > 0)
                    {
                        count++;
                    }
                    if (userItem.Desc[11] > 0)
                    {
                        count++;
                    }
                    if (userItem.Desc[12] > 0)
                    {
                        count++;
                    }
                    break;
                case 52:
                    clientItem.Item.AC = HUtil32.MakeWord(HUtil32.LoByte(stdItem.AC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(stdItem.AC) + userItem.Desc[0]));
                    clientItem.Item.MAC = HUtil32.MakeWord(HUtil32.LoByte(stdItem.MAC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(stdItem.MAC) + userItem.Desc[1]));
                    clientItem.Item.Agility = (byte)(stdItem.Agility + userItem.Desc[3]);
                    if (userItem.Desc[0] > 0)
                    {
                        count++;
                    }
                    if (userItem.Desc[1] > 0)
                    {
                        count++;
                    }
                    if (userItem.Desc[3] > 0)
                    {
                        count++;
                    }
                    break;
                case 54:
                    clientItem.Item.AC = HUtil32.MakeWord(HUtil32.LoByte(stdItem.AC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(stdItem.AC) + userItem.Desc[0]));
                    clientItem.Item.MAC = HUtil32.MakeWord(HUtil32.LoByte(stdItem.MAC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(stdItem.MAC) + userItem.Desc[1]));
                    clientItem.Item.Accurate = (byte)(stdItem.Accurate + userItem.Desc[2]);
                    clientItem.Item.Agility = (byte)(stdItem.Agility + userItem.Desc[3]);
                    clientItem.Item.ToxAvoid = (byte)(stdItem.ToxAvoid + userItem.Desc[13]);
                    if (userItem.Desc[0] > 0)
                    {
                        count++;
                    }
                    if (userItem.Desc[1] > 0)
                    {
                        count++;
                    }
                    if (userItem.Desc[2] > 0)
                    {
                        count++;
                    }
                    if (userItem.Desc[3] > 0)
                    {
                        count++;
                    }
                    if (userItem.Desc[13] > 0)
                    {
                        count++;
                    }
                    break;
            }
            return count;
        }

        public int RealAttackSpeed(short wAtkSpd)
        {
            int result;
            if (wAtkSpd <= 10)
            {
                result = -wAtkSpd;
            }
            else
            {
                result = wAtkSpd - 10;
            }
            return result;
        }

        private static short NaturalAttackSpeed(int iAtkSpd)
        {
            short result;
            if (iAtkSpd <= 0)
            {
                result = (short)-iAtkSpd;
            }
            else
            {
                result = (short)(iAtkSpd + 10);
            }
            return result;
        }

        private byte GetAttackSpeed(byte bStdAtkSpd, byte bUserAtkSpd)
        {
            var iTemp = RealAttackSpeed(bStdAtkSpd) + RealAttackSpeed(bUserAtkSpd);
            return (byte)NaturalAttackSpeed(iTemp);
        }

        /// <summary>
        /// 攻击速度升级
        /// </summary>
        /// <returns></returns>
        public byte UpgradeAttackSpeed(byte bUserAtkSpd, int iUpValue)
        {
            var iTemp = RealAttackSpeed(bUserAtkSpd) + iUpValue;
            return (byte)NaturalAttackSpeed(iTemp);
        }

        public void RandomUpgradeItem(StdItem stdItem, UserItem pu)
        {
            if (stdItem != null)
            {
                switch (stdItem.StdMode)
                {
                    case 5:
                    case 6:
                        UpgradeRandomWeapon(pu);
                        break;
                    case 10:
                    case 11:
                        UpgradeRandomDress(pu);
                        break;
                    case 19:
                        UpgradeRandomNecklace19(pu);
                        break;
                    case 20:
                    case 21:
                    case 24:
                        UpgradeRandomNecklace(pu);
                        break;
                    case 26:
                        UpgradeRandomBarcelet(pu);
                        break;
                    case 22:
                        UpgradeRandomRings(pu);
                        break;
                    case 23:
                        UpgradeRandomRings23(pu);
                        break;
                    case 15:
                        UpgradeRandomHelmet(pu);
                        break;
                }
            }
        }

        public void RandomSetUnknownItem(StdItem stdItem, UserItem pu)
        {
            if (stdItem != null)
            {
                switch (stdItem.StdMode)
                {
                    case 15:
                        RandomSetUnknownHelmet(pu);
                        break;
                    case 22:
                    case 23:
                        RandomSetUnknownRing(pu);
                        break;
                    case 24:
                    case 26:
                        RandomSetUnknownBracelet(pu);
                        break;
                }
            }
        }
    }
}