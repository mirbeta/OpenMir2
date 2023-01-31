using SystemModule.Packets.ClientPackets;

namespace GameSvr.Items
{
    public class StdItemAttrConst
    {
        public const byte AC = 0;
        public const byte MAC = 1;
        public const byte DC = 2;
        public const byte MC = 3;
        public const byte SC = 3;
    }

    public class StdItem
    {
        public string Name;
        public byte StdMode;
        public byte Shape;
        /// <summary>
        /// 物品重量
        /// </summary>
        public byte Weight;
        public byte AniCount;
        public sbyte SpecialPwr;
        public byte ItemDesc;
        public ushort Looks;
        public ushort DuraMax;
        public ushort AC;
        public ushort MAC;
        public ushort DC;
        public ushort MC;
        public ushort SC;
        public byte Need;
        public byte NeedLevel;
        public byte NeedIdentify;
        public int Price;
        public int Stock;
        public byte AtkSpd;
        public byte Agility;
        public byte Accurate;
        public byte MgAvoid;
        public byte Strong;
        public byte Undead;
        public int HpAdd;
        public int MpAdd;
        public int ExpAdd;
        public byte EffType1;
        public byte EffRate1;
        public byte EffValue1;
        public byte EffType2;
        public byte EffRate2;
        public byte EffValue2;
        public byte Slowdown;
        public byte Tox;
        public byte ToxAvoid;
        public byte UniqueItem;
        public byte OverlapItem;
        public byte Light;
        public byte ItemType;
        public ushort ItemSet;
        public string Reference;

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

        private void CopyItemToClientItem(ref ClientItem clientStdItem)
        {
            if (clientStdItem == null)
            {
                clientStdItem = new ClientItem();
            }
            clientStdItem.Item.Name = Name;
            clientStdItem.Item.StdMode = StdMode;
            clientStdItem.Item.Shape = Shape;
            clientStdItem.Item.Weight = Weight;
            clientStdItem.Item.AniCount = AniCount;
            clientStdItem.Item.SpecialPwr = SpecialPwr;
            clientStdItem.Item.ItemDesc = ItemDesc;
            clientStdItem.Item.Looks = Looks;
            clientStdItem.Item.DuraMax = DuraMax;
            clientStdItem.Item.AC = AC;
            clientStdItem.Item.MAC = MAC;
            clientStdItem.Item.DC = DC;
            clientStdItem.Item.MC = MC;
            clientStdItem.Item.SC = SC;
            clientStdItem.Item.Need = Need;
            clientStdItem.Item.NeedLevel = NeedLevel;
            clientStdItem.Item.NeedIdentify = NeedIdentify;
            clientStdItem.Item.Price = Price;
            clientStdItem.Item.Stock = Stock;
            clientStdItem.Item.AtkSpd = AtkSpd;
            clientStdItem.Item.Agility = Agility;
            clientStdItem.Item.Accurate = Accurate;
            clientStdItem.Item.MgAvoid = MgAvoid;
            clientStdItem.Item.Strong = Strong;
            clientStdItem.Item.Undead = Undead;
            clientStdItem.Item.HpAdd = HpAdd;
            clientStdItem.Item.MpAdd = MpAdd;
            clientStdItem.Item.ExpAdd = ExpAdd;
            clientStdItem.Item.EffType1 = EffType1;
            clientStdItem.Item.EffRate1 = EffRate1;
            clientStdItem.Item.EffValue1 = EffValue1;
            clientStdItem.Item.EffType2 = EffType2;
            clientStdItem.Item.EffRate2 = EffRate2;
            clientStdItem.Item.EffValue2 = EffValue2;
            clientStdItem.Item.Slowdown = Slowdown;
            clientStdItem.Item.Tox = Tox;
            clientStdItem.Item.ToxAvoid = ToxAvoid;
            clientStdItem.Item.UniqueItem = UniqueItem;
            clientStdItem.Item.OverlapItem = OverlapItem;
            clientStdItem.Item.Light = Light;
            clientStdItem.Item.ItemType = ItemType;
            clientStdItem.Item.ItemSet = ItemSet;
            clientStdItem.Item.Reference = Reference;
        }

        public int GetUpgradeStdItem(UserItem userItem, ref ClientItem clientItem)
        {
            CopyItemToClientItem(ref clientItem);
            var UCount = 0;
            switch (StdMode)
            {
                case 5:
                case 6:
                    clientItem.Item.DC = HUtil32.MakeWord(HUtil32.LoByte(DC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(DC) + userItem.Desc[0]));
                    clientItem.Item.MC = HUtil32.MakeWord(HUtil32.LoByte(MC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(MC) + userItem.Desc[1]));
                    clientItem.Item.SC = HUtil32.MakeWord(HUtil32.LoByte(SC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(SC) + userItem.Desc[2]));
                    clientItem.Item.AC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(AC) + userItem.Desc[3]), (ushort)(HUtil32.HiByte(AC) + userItem.Desc[5]));
                    clientItem.Item.MAC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(MAC) + userItem.Desc[4]), HUtil32.HiByte(MAC));
                    clientItem.Item.MAC = HUtil32.MakeWord(HUtil32.LoByte(MAC), GetAttackSpeed(HUtil32.HiByte(MAC), userItem.Desc[6]));
                    if (userItem.Desc[7] >= 1 && userItem.Desc[7] <= 10)
                    {
                        if (clientItem.Item.SpecialPwr >= 0)
                        {
                            clientItem.Item.SpecialPwr = (sbyte)userItem.Desc[7];
                        }
                    }
                    if (userItem.Desc[10] != 0)
                    {
                        clientItem.Item.ItemDesc = (byte)(ItemDesc | 0x01);
                    }
                    clientItem.Item.Slowdown = (byte)(Slowdown + userItem.Desc[12]);
                    clientItem.Item.Tox = (byte)(Tox + userItem.Desc[13]);
                    if (userItem.Desc[0] > 0)
                    {
                        UCount++;
                    }
                    if (userItem.Desc[1] > 0)
                    {
                        UCount++;
                    }
                    if (userItem.Desc[2] > 0)
                    {
                        UCount++;
                    }
                    if (userItem.Desc[3] > 0)
                    {
                        UCount++;
                    }
                    if (userItem.Desc[4] > 0)
                    {
                        UCount++;
                    }
                    if (userItem.Desc[5] > 0)
                    {
                        UCount++;
                    }
                    if (userItem.Desc[6] > 0)
                    {
                        UCount++;
                    }
                    if (userItem.Desc[7] > 0)
                    {
                        UCount++;
                    }
                    if (userItem.Desc[12] > 0)
                    {
                        UCount++;
                    }
                    if (userItem.Desc[13] > 0)
                    {
                        UCount++;
                    }
                    break;
                case 10:
                case 11:
                    clientItem.Item.AC = HUtil32.MakeWord(HUtil32.LoByte(AC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(AC) + userItem.Desc[0]));
                    clientItem.Item.MAC = HUtil32.MakeWord(HUtil32.LoByte(MAC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(MAC) + userItem.Desc[1]));
                    clientItem.Item.DC = HUtil32.MakeWord(HUtil32.LoByte(DC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(DC) + userItem.Desc[2]));
                    clientItem.Item.MC = HUtil32.MakeWord(HUtil32.LoByte(MC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(MC) + userItem.Desc[3]));
                    clientItem.Item.SC = HUtil32.MakeWord(HUtil32.LoByte(SC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(SC) + userItem.Desc[4]));
                    clientItem.Item.Agility = (byte)(Agility + userItem.Desc[11]);
                    clientItem.Item.MgAvoid = (byte)(MgAvoid + userItem.Desc[12]);
                    clientItem.Item.ToxAvoid = (byte)(ToxAvoid + userItem.Desc[13]);
                    if (userItem.Desc[0] > 0)
                    {
                        UCount++;
                    }
                    if (userItem.Desc[1] > 0)
                    {
                        UCount++;
                    }
                    if (userItem.Desc[2] > 0)
                    {
                        UCount++;
                    }
                    if (userItem.Desc[3] > 0)
                    {
                        UCount++;
                    }
                    if (userItem.Desc[4] > 0)
                    {
                        UCount++;
                    }
                    if (userItem.Desc[11] > 0)
                    {
                        UCount++;
                    }
                    if (userItem.Desc[12] > 0)
                    {
                        UCount++;
                    }
                    if (userItem.Desc[13] > 0)
                    {
                        UCount++;
                    }
                    break;
                case 15:
                    clientItem.Item.AC = HUtil32.MakeWord(HUtil32.LoByte(AC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(AC) + userItem.Desc[0]));
                    clientItem.Item.MAC = HUtil32.MakeWord(HUtil32.LoByte(MAC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(MAC) + userItem.Desc[1]));
                    clientItem.Item.DC = HUtil32.MakeWord(HUtil32.LoByte(DC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(DC) + userItem.Desc[2]));
                    clientItem.Item.MC = HUtil32.MakeWord(HUtil32.LoByte(MC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(MC) + userItem.Desc[3]));
                    clientItem.Item.SC = HUtil32.MakeWord(HUtil32.LoByte(SC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(SC) + userItem.Desc[4]));
                    clientItem.Item.Accurate = (byte)(Accurate + userItem.Desc[11]);
                    clientItem.Item.MgAvoid = (byte)(MgAvoid + userItem.Desc[12]);
                    clientItem.Item.ToxAvoid = (byte)(ToxAvoid + userItem.Desc[13]);
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
                        UCount++;
                    }
                    if (userItem.Desc[1] > 0)
                    {
                        UCount++;
                    }
                    if (userItem.Desc[2] > 0)
                    {
                        UCount++;
                    }
                    if (userItem.Desc[3] > 0)
                    {
                        UCount++;
                    }
                    if (userItem.Desc[4] > 0)
                    {
                        UCount++;
                    }
                    if (userItem.Desc[11] > 0)
                    {
                        UCount++;
                    }
                    if (userItem.Desc[12] > 0)
                    {
                        UCount++;
                    }
                    if (userItem.Desc[13] > 0)
                    {
                        UCount++;
                    }
                    break;
                case 19:
                case 20:
                case 21:
                    clientItem.Item.AC = HUtil32.MakeWord(HUtil32.LoByte(AC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(AC) + userItem.Desc[0]));
                    clientItem.Item.MAC = HUtil32.MakeWord(HUtil32.LoByte(MAC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(MAC) + userItem.Desc[1]));
                    clientItem.Item.DC = HUtil32.MakeWord(HUtil32.LoByte(DC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(DC) + userItem.Desc[2]));
                    clientItem.Item.MC = HUtil32.MakeWord(HUtil32.LoByte(MC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(MC) + userItem.Desc[3]));
                    clientItem.Item.SC = HUtil32.MakeWord(HUtil32.LoByte(SC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(SC) + userItem.Desc[4]));
                    clientItem.Item.AtkSpd = (byte)(AtkSpd + userItem.Desc[9]);
                    clientItem.Item.Slowdown = (byte)(Slowdown + userItem.Desc[12]);
                    clientItem.Item.Tox = (byte)(Tox + userItem.Desc[13]);
                    if (StdMode == 19)
                    {
                        clientItem.Item.Accurate = (byte)(Accurate + userItem.Desc[11]);
                    }
                    else if (StdMode == 20)
                    {
                        clientItem.Item.MgAvoid = (byte)(MgAvoid + userItem.Desc[11]);
                    }
                    else if (StdMode == 21)
                    {
                        clientItem.Item.Accurate = (byte)(Accurate + userItem.Desc[11]);
                        clientItem.Item.MgAvoid = (byte)(MgAvoid + userItem.Desc[7]);
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
                        UCount++;
                    }
                    if (userItem.Desc[1] > 0)
                    {
                        UCount++;
                    }
                    if (userItem.Desc[2] > 0)
                    {
                        UCount++;
                    }
                    if (userItem.Desc[3] > 0)
                    {
                        UCount++;
                    }
                    if (userItem.Desc[4] > 0)
                    {
                        UCount++;
                    }
                    if (userItem.Desc[9] > 0)
                    {
                        UCount++;
                    }
                    if (userItem.Desc[11] > 0)
                    {
                        UCount++;
                    }
                    if (userItem.Desc[12] > 0)
                    {
                        UCount++;
                    }
                    if (userItem.Desc[13] > 0)
                    {
                        UCount++;
                    }
                    break;
                case 22:
                case 23:
                    clientItem.Item.AC = HUtil32.MakeWord(HUtil32.LoByte(AC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(AC) + userItem.Desc[0]));
                    clientItem.Item.MAC = HUtil32.MakeWord(HUtil32.LoByte(MAC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(MAC) + userItem.Desc[1]));
                    clientItem.Item.DC = HUtil32.MakeWord(HUtil32.LoByte(DC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(DC) + userItem.Desc[2]));
                    clientItem.Item.MC = HUtil32.MakeWord(HUtil32.LoByte(MC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(MC) + userItem.Desc[3]));
                    clientItem.Item.SC = HUtil32.MakeWord(HUtil32.LoByte(SC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(SC) + userItem.Desc[4]));
                    clientItem.Item.AtkSpd = (byte)(AtkSpd + userItem.Desc[9]);
                    clientItem.Item.Slowdown = (byte)(Slowdown + userItem.Desc[12]);
                    clientItem.Item.Tox = (byte)(Tox + userItem.Desc[13]);
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
                        UCount++;
                    }
                    if (userItem.Desc[1] > 0)
                    {
                        UCount++;
                    }
                    if (userItem.Desc[2] > 0)
                    {
                        UCount++;
                    }
                    if (userItem.Desc[3] > 0)
                    {
                        UCount++;
                    }
                    if (userItem.Desc[4] > 0)
                    {
                        UCount++;
                    }
                    if (userItem.Desc[9] > 0)
                    {
                        UCount++;
                    }
                    if (userItem.Desc[12] > 0)
                    {
                        UCount++;
                    }
                    if (userItem.Desc[13] > 0)
                    {
                        UCount++;
                    }
                    break;
                case 24:
                    clientItem.Item.AC = HUtil32.MakeWord(HUtil32.LoByte(AC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(AC) + userItem.Desc[0]));
                    clientItem.Item.MAC = HUtil32.MakeWord(HUtil32.LoByte(MAC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(MAC) + userItem.Desc[1]));
                    clientItem.Item.DC = HUtil32.MakeWord(HUtil32.LoByte(DC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(DC) + userItem.Desc[2]));
                    clientItem.Item.MC = HUtil32.MakeWord(HUtil32.LoByte(MC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(MC) + userItem.Desc[3]));
                    clientItem.Item.SC = HUtil32.MakeWord(HUtil32.LoByte(SC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(SC) + userItem.Desc[4]));
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
                        UCount++;
                    }
                    if (userItem.Desc[1] > 0)
                    {
                        UCount++;
                    }
                    if (userItem.Desc[2] > 0)
                    {
                        UCount++;
                    }
                    if (userItem.Desc[3] > 0)
                    {
                        UCount++;
                    }
                    if (userItem.Desc[4] > 0)
                    {
                        UCount++;
                    }
                    break;
                case 26:
                    clientItem.Item.AC = HUtil32.MakeWord(HUtil32.LoByte(AC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(AC) + userItem.Desc[0]));
                    clientItem.Item.MAC = HUtil32.MakeWord(HUtil32.LoByte(MAC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(MAC) + userItem.Desc[1]));
                    clientItem.Item.DC = HUtil32.MakeWord(HUtil32.LoByte(DC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(DC) + userItem.Desc[2]));
                    clientItem.Item.MC = HUtil32.MakeWord(HUtil32.LoByte(MC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(MC) + userItem.Desc[3]));
                    clientItem.Item.SC = HUtil32.MakeWord(HUtil32.LoByte(SC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(SC) + userItem.Desc[4]));
                    clientItem.Item.Accurate = (byte)(Accurate + userItem.Desc[11]);
                    clientItem.Item.Agility = (byte)(Agility + userItem.Desc[12]);
                    if (userItem.Desc[5] > 0)
                    {
                        // 鞘夸(肪,颇鲍,付过,档仿)
                        clientItem.Item.Need = userItem.Desc[5];
                    }
                    if (userItem.Desc[6] > 0)
                    {
                        clientItem.Item.NeedLevel = userItem.Desc[6];
                    }
                    if (userItem.Desc[0] > 0)
                    {
                        UCount++;
                    }
                    if (userItem.Desc[1] > 0)
                    {
                        UCount++;
                    }
                    if (userItem.Desc[2] > 0)
                    {
                        UCount++;
                    }
                    if (userItem.Desc[3] > 0)
                    {
                        UCount++;
                    }
                    if (userItem.Desc[4] > 0)
                    {
                        UCount++;
                    }
                    if (userItem.Desc[11] > 0)
                    {
                        UCount++;
                    }
                    if (userItem.Desc[12] > 0)
                    {
                        UCount++;
                    }
                    break;
                case 52:
                    clientItem.Item.AC = HUtil32.MakeWord(HUtil32.LoByte(AC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(AC) + userItem.Desc[0]));
                    clientItem.Item.MAC = HUtil32.MakeWord(HUtil32.LoByte(MAC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(MAC) + userItem.Desc[1]));
                    clientItem.Item.Agility = (byte)(Agility + userItem.Desc[3]);
                    if (userItem.Desc[0] > 0)
                    {
                        UCount++;
                    }
                    if (userItem.Desc[1] > 0)
                    {
                        UCount++;
                    }
                    if (userItem.Desc[3] > 0)
                    {
                        UCount++;
                    }
                    break;
                case 54:
                    clientItem.Item.AC = HUtil32.MakeWord(HUtil32.LoByte(AC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(AC) + userItem.Desc[0]));
                    clientItem.Item.MAC = HUtil32.MakeWord(HUtil32.LoByte(MAC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(MAC) + userItem.Desc[1]));
                    clientItem.Item.Accurate = (byte)(Accurate + userItem.Desc[2]);
                    clientItem.Item.Agility = (byte)(Agility + userItem.Desc[3]);
                    clientItem.Item.ToxAvoid = (byte)(ToxAvoid + userItem.Desc[13]);
                    if (userItem.Desc[0] > 0)
                    {
                        UCount++;
                    }
                    if (userItem.Desc[1] > 0)
                    {
                        UCount++;
                    }
                    if (userItem.Desc[2] > 0)
                    {
                        UCount++;
                    }
                    if (userItem.Desc[3] > 0)
                    {
                        UCount++;
                    }
                    if (userItem.Desc[13] > 0)
                    {
                        UCount++;
                    }
                    break;
            }
            return UCount;
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

        private short NaturalAttackSpeed(int iAtkSpd)
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
            int iTemp = RealAttackSpeed(bStdAtkSpd) + RealAttackSpeed(bUserAtkSpd);
            return (byte)NaturalAttackSpeed(iTemp);
        }

        public byte UpgradeAttackSpeed(byte bUserAtkSpd, int iUpValue)
        {
            int iTemp = RealAttackSpeed(bUserAtkSpd) + iUpValue;
            return (byte)NaturalAttackSpeed(iTemp);
        }

        public void RandomUpgradeItem(UserItem pu)
        {
            StdItem stdItem = M2Share.WorldEngine.GetStdItem(pu.Index);
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

        public void RandomSetUnknownItem(UserItem pu)
        {
            StdItem stdItem = M2Share.WorldEngine.GetStdItem(pu.Index);
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