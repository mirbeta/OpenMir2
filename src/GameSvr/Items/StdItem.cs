using SystemModule;
using SystemModule.Data;
using SystemModule.Packet.ClientPackets;

namespace GameSvr.Items
{
    public class StdItem
    {
        public GoodType ItemType = 0;
        public string Name;
        public byte StdMode = 0;
        public byte Shape = 0;
        public byte Weight = 0;
        public byte AniCount = 0;
        public short Source = 0;
        public byte Reserved = 0;
        public byte NeedIdentify = 0;
        public ushort Looks = 0;
        public ushort DuraMax = 0;
        public ushort Ac = 0;
        public ushort Ac2 = 0;
        public ushort Mac = 0;
        public ushort Mac2 = 0;
        public ushort Dc = 0;
        public ushort Dc2 = 0;
        public ushort Mc = 0;
        public ushort Mc2 = 0;
        public ushort Sc = 0;
        public ushort Sc2 = 0;
        public byte Need = 0;
        public byte NeedLevel = 0;
        public int Price = 0;
        public bool Light = false;

        private int GetRandomRange(int nCount, int nRate)
        {
            var result = 0;
            for (var i = 0; i < nCount; i++)
            {
                if (M2Share.RandomNumber.Random(nRate) == 0)
                {
                    result++;
                }
            }
            return result;
        }

        public void GetStandardItem(ref ClientStdItem stdItem)
        {
            if (stdItem == null)
            {
                stdItem = new ClientStdItem();
            }
            stdItem.Name = M2Share.FilterShowName(Name);
            stdItem.StdMode = StdMode;
            stdItem.Shape = Shape;
            stdItem.Weight = Weight;
            stdItem.AniCount = AniCount;
            //stdItem.reserved = Reserved;
            //stdItem.Source = (sbyte)Source;
            stdItem.NeedIdentify = NeedIdentify;
            stdItem.Looks = Looks;
            stdItem.DuraMax = DuraMax;
            stdItem.Need = Need;
            stdItem.NeedLevel = NeedLevel;
            stdItem.Price = Price;
        }

        public void GetItemAddValue(UserItem userItem, ref ClientStdItem stdItem)
        {
            switch (ItemType)
            {
                case GoodType.ITEM_WEAPON:
                    stdItem.DC = HUtil32.MakeLong(Dc, Dc2 + userItem.btValue[0]);
                    stdItem.MC = HUtil32.MakeLong(Mc, Mc2 + userItem.btValue[1]);
                    stdItem.SC = HUtil32.MakeLong(Sc, Sc2 + userItem.btValue[2]);
                    stdItem.AC = HUtil32.MakeLong(Ac + userItem.btValue[3], Ac2 + userItem.btValue[5]);
                    stdItem.MAC = HUtil32.MakeLong(Mac + userItem.btValue[4], Mac2 + userItem.btValue[6]);
                    //if (userItem.btValue[7] - 1 < 10)
                    //{
                    //    stdItem.Source = (sbyte)userItem.btValue[7];
                    //}
                    //if (userItem.btValue[ItemAttr.WeaponUpgrade] != 0)
                    //{
                    //    stdItem.reserved = (byte)(stdItem.reserved | 1);
                    //}
                    break;
                case GoodType.ITEM_ARMOR:
                    stdItem.AC = HUtil32.MakeLong(Ac, Ac2 + userItem.btValue[0]);
                    stdItem.MAC = HUtil32.MakeLong(Mac, Mac2 + userItem.btValue[1]);
                    stdItem.DC = HUtil32.MakeLong(Dc, Dc2 + userItem.btValue[2]);
                    stdItem.MC = HUtil32.MakeLong(Mc, Mc2 + userItem.btValue[3]);
                    stdItem.SC = HUtil32.MakeLong(Sc, Sc2 + userItem.btValue[4]);
                    break;
                case GoodType.ITEM_ACCESSORY:
                    stdItem.AC = HUtil32.MakeLong(Ac, Ac2 + userItem.btValue[0]);
                    stdItem.MAC = HUtil32.MakeLong(Mac, Mac2 + userItem.btValue[1]);
                    stdItem.DC = HUtil32.MakeLong(Dc, Dc2 + userItem.btValue[2]);
                    stdItem.MC = HUtil32.MakeLong(Mc, Mc2 + userItem.btValue[3]);
                    stdItem.SC = HUtil32.MakeLong(Sc, Sc2 + userItem.btValue[4]);
                    if (userItem.btValue[5] > 0)
                    {
                        stdItem.Need = userItem.btValue[5];
                    }
                    if (userItem.btValue[6] > 0)
                    {
                        stdItem.NeedLevel = userItem.btValue[6];
                    }
                    break;
                case GoodType.ITEM_LEECHDOM:
                    stdItem.AC = HUtil32.MakeLong(Ac, Ac2);
                    stdItem.MAC = HUtil32.MakeLong(Mac, Mac2);
                    stdItem.DC = HUtil32.MakeLong(Dc, Dc2);
                    stdItem.MC = HUtil32.MakeLong(Mc, Mc2);
                    stdItem.SC = HUtil32.MakeLong(Sc, Sc2);
                    break;
                default:
                    stdItem.AC = 0;
                    stdItem.MAC = 0;
                    stdItem.DC = 0;
                    stdItem.MC = 0;
                    stdItem.SC = 0;
                    //stdItem.Source = 0;
                    //stdItem.reserved = 0;
                    break;
            }
        }

        public void RandomUpgradeItem(UserItem userItem)
        {
            int nUpgrade;
            int nIncp;
            int nVal;
            switch (ItemType)
            {
                case GoodType.ITEM_WEAPON:
                    nUpgrade = GetRandomRange(M2Share.Config.WeaponDCAddValueMaxLimit, M2Share.Config.WeaponDCAddValueRate);
                    if (M2Share.RandomNumber.Random(15) == 0)
                    {
                        userItem.btValue[0] = (byte)(nUpgrade + 1);
                    }
                    nUpgrade = GetRandomRange(12, 15);
                    if (M2Share.RandomNumber.Random(20) == 0)
                    {
                        nIncp = (nUpgrade + 1) / 3;
                        if (nIncp > 0)
                        {
                            if (M2Share.RandomNumber.Random(3) != 0)
                            {
                                userItem.btValue[6] = (byte)nIncp;
                            }
                            else
                            {
                                userItem.btValue[6] = (byte)(nIncp + 10);
                            }
                        }
                    }
                    nUpgrade = GetRandomRange(12, 15);
                    if (M2Share.RandomNumber.Random(15) == 0)
                    {
                        userItem.btValue[1] = (byte)(nUpgrade + 1);
                    }
                    nUpgrade = GetRandomRange(12, 15);
                    if (M2Share.RandomNumber.Random(15) == 0)
                    {
                        userItem.btValue[2] = (byte)(nUpgrade + 1);
                    }
                    nUpgrade = GetRandomRange(12, 15);
                    if (M2Share.RandomNumber.Random(24) == 0)
                    {
                        userItem.btValue[5] = (byte)(nUpgrade / 2 + 1);
                    }
                    nUpgrade = GetRandomRange(12, 12);
                    if (M2Share.RandomNumber.Random(3) < 2)
                    {
                        nVal = (nUpgrade + 1) * 2000;
                        userItem.DuraMax = (byte)HUtil32._MIN(65000, userItem.DuraMax + nVal);
                        userItem.Dura = (byte)HUtil32._MIN(65000, userItem.Dura + nVal);
                    }
                    nUpgrade = GetRandomRange(12, 15);
                    if (M2Share.RandomNumber.Random(10) == 0)
                    {
                        userItem.btValue[7] = (byte)(nUpgrade / 2 + 1);
                    }
                    break;
                case GoodType.ITEM_ARMOR:
                    nUpgrade = GetRandomRange(6, 15);
                    if (M2Share.RandomNumber.Random(30) == 0)
                    {
                        userItem.btValue[0] = (byte)(nUpgrade + 1);
                    }
                    nUpgrade = GetRandomRange(6, 15);
                    if (M2Share.RandomNumber.Random(30) == 0)
                    {
                        userItem.btValue[1] = (byte)(nUpgrade + 1);
                    }
                    nUpgrade = GetRandomRange(M2Share.Config.DressDCAddValueMaxLimit, M2Share.Config.DressDCAddValueRate);
                    if (M2Share.RandomNumber.Random(M2Share.Config.DressDCAddRate) == 0)
                    {
                        userItem.btValue[2] = (byte)(nUpgrade + 1);
                    }
                    nUpgrade = GetRandomRange(M2Share.Config.DressMCAddValueMaxLimit, M2Share.Config.DressMCAddValueRate);
                    if (M2Share.RandomNumber.Random(M2Share.Config.DressMCAddRate) == 0)
                    {
                        userItem.btValue[3] = (byte)(nUpgrade + 1);
                    }
                    nUpgrade = GetRandomRange(M2Share.Config.DressSCAddValueMaxLimit, M2Share.Config.nDressSCAddValueRate);
                    if (M2Share.RandomNumber.Random(M2Share.Config.DressSCAddRate) == 0)
                    {
                        userItem.btValue[4] = (byte)(nUpgrade + 1);
                    }
                    nUpgrade = GetRandomRange(6, 10);
                    if (M2Share.RandomNumber.Random(8) < 6)
                    {
                        nVal = (nUpgrade + 1) * 2000;
                        userItem.DuraMax = (byte)HUtil32._MIN(65000, userItem.DuraMax + nVal);
                        userItem.Dura = (byte)HUtil32._MIN(65000, userItem.Dura + nVal);
                    }
                    break;
                case GoodType.ITEM_ACCESSORY:
                    switch (StdMode)
                    {
                        case 20:
                        case 21:
                        case 24:
                            nUpgrade = GetRandomRange(6, 30);
                            if (M2Share.RandomNumber.Random(60) == 0)
                            {
                                userItem.btValue[0] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(6, 30);
                            if (M2Share.RandomNumber.Random(60) == 0)
                            {
                                userItem.btValue[1] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(M2Share.Config.NeckLace202124DCAddValueMaxLimit, M2Share.Config.NeckLace202124DCAddValueRate);
                            if (M2Share.RandomNumber.Random(M2Share.Config.NeckLace202124DCAddRate) == 0)
                            {
                                userItem.btValue[2] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(M2Share.Config.NeckLace202124MCAddValueMaxLimit, M2Share.Config.NeckLace202124MCAddValueRate);
                            if (M2Share.RandomNumber.Random(M2Share.Config.NeckLace202124MCAddRate) == 0)
                            {
                                userItem.btValue[3] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(M2Share.Config.NeckLace202124SCAddValueMaxLimit, M2Share.Config.NeckLace202124SCAddValueRate);
                            if (M2Share.RandomNumber.Random(M2Share.Config.NeckLace202124SCAddRate) == 0)
                            {
                                userItem.btValue[4] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(6, 12);
                            if (M2Share.RandomNumber.Random(20) < 15)
                            {
                                nVal = (nUpgrade + 1) * 1000;
                                userItem.DuraMax = (byte)HUtil32._MIN(65000, userItem.DuraMax + nVal);
                                userItem.Dura = (byte)HUtil32._MIN(65000, userItem.Dura + nVal);
                            }
                            break;
                        case 26:
                            nUpgrade = GetRandomRange(6, 20);
                            if (M2Share.RandomNumber.Random(20) == 0)
                            {
                                userItem.btValue[0] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(6, 20);
                            if (M2Share.RandomNumber.Random(20) == 0)
                            {
                                userItem.btValue[1] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(M2Share.Config.ArmRing26DCAddValueMaxLimit, M2Share.Config.ArmRing26DCAddValueRate);
                            if (M2Share.RandomNumber.Random(M2Share.Config.ArmRing26DCAddRate) == 0)
                            {
                                userItem.btValue[2] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(M2Share.Config.ArmRing26MCAddValueMaxLimit, M2Share.Config.ArmRing26MCAddValueRate);
                            if (M2Share.RandomNumber.Random(M2Share.Config.ArmRing26MCAddRate) == 0)
                            {
                                userItem.btValue[3] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(M2Share.Config.ArmRing26SCAddValueMaxLimit, M2Share.Config.ArmRing26SCAddValueRate);
                            if (M2Share.RandomNumber.Random(M2Share.Config.ArmRing26SCAddRate) == 0)
                            {
                                userItem.btValue[4] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(6, 12);
                            if (M2Share.RandomNumber.Random(20) < 15)
                            {
                                nVal = (nUpgrade + 1) * 1000;
                                userItem.DuraMax = (byte)HUtil32._MIN(65000, userItem.DuraMax + nVal);
                                userItem.Dura = (byte)HUtil32._MIN(65000, userItem.Dura + nVal);
                            }
                            break;
                        case 19:
                            nUpgrade = GetRandomRange(6, 20);
                            if (M2Share.RandomNumber.Random(40) == 0)
                            {
                                userItem.btValue[0] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(6, 20);
                            if (M2Share.RandomNumber.Random(40) == 0)
                            {
                                userItem.btValue[1] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(M2Share.Config.NeckLace19DCAddValueMaxLimit, M2Share.Config.NeckLace19DCAddValueRate);
                            if (M2Share.RandomNumber.Random(M2Share.Config.NeckLace19DCAddRate) == 0)
                            {
                                userItem.btValue[2] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(M2Share.Config.NeckLace19MCAddValueMaxLimit, M2Share.Config.NeckLace19MCAddValueRate);
                            if (M2Share.RandomNumber.Random(M2Share.Config.NeckLace19MCAddRate) == 0)
                            {
                                userItem.btValue[3] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(M2Share.Config.NeckLace19SCAddValueMaxLimit, M2Share.Config.NeckLace19SCAddValueRate);
                            if (M2Share.RandomNumber.Random(M2Share.Config.NeckLace19SCAddRate) == 0)
                            {
                                userItem.btValue[4] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(6, 10);
                            if (M2Share.RandomNumber.Random(4) < 3)
                            {
                                nVal = (nUpgrade + 1) * 1000;
                                userItem.DuraMax = (byte)HUtil32._MIN(65000, userItem.DuraMax + nVal);
                                userItem.Dura = (byte)HUtil32._MIN(65000, userItem.Dura + nVal);
                            }
                            break;
                        case 22:
                            nUpgrade = GetRandomRange(M2Share.Config.Ring22DCAddValueMaxLimit, M2Share.Config.Ring22DCAddValueRate);
                            if (M2Share.RandomNumber.Random(M2Share.Config.Ring22DCAddRate) == 0)
                            {
                                userItem.btValue[2] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(M2Share.Config.Ring22MCAddValueMaxLimit, M2Share.Config.Ring22MCAddValueRate);
                            if (M2Share.RandomNumber.Random(M2Share.Config.Ring22MCAddRate) == 0)
                            {
                                userItem.btValue[3] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(M2Share.Config.Ring22SCAddValueMaxLimit, M2Share.Config.Ring22SCAddValueRate);
                            if (M2Share.RandomNumber.Random(M2Share.Config.Ring22SCAddRate) == 0)
                            {
                                userItem.btValue[4] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(6, 12);
                            if (M2Share.RandomNumber.Random(4) < 3)
                            {
                                nVal = (nUpgrade + 1) * 1000;
                                userItem.DuraMax = (byte)HUtil32._MIN(65000, userItem.DuraMax + nVal);
                                userItem.Dura = (byte)HUtil32._MIN(65000, userItem.Dura + nVal);
                            }
                            break;
                        case 23:
                            nUpgrade = GetRandomRange(6, 20);
                            if (M2Share.RandomNumber.Random(40) == 0)
                            {
                                userItem.btValue[0] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(6, 20);
                            if (M2Share.RandomNumber.Random(40) == 0)
                            {
                                userItem.btValue[1] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(M2Share.Config.Ring23DCAddValueMaxLimit, M2Share.Config.Ring23DCAddValueRate);
                            if (M2Share.RandomNumber.Random(M2Share.Config.Ring23DCAddRate) == 0)
                            {
                                userItem.btValue[2] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(M2Share.Config.Ring23MCAddValueMaxLimit, M2Share.Config.Ring23MCAddValueRate);
                            if (M2Share.RandomNumber.Random(M2Share.Config.Ring23MCAddRate) == 0)
                            {
                                userItem.btValue[3] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(M2Share.Config.Ring23SCAddValueMaxLimit, M2Share.Config.Ring23SCAddValueRate);
                            if (M2Share.RandomNumber.Random(M2Share.Config.Ring23SCAddRate) == 0)
                            {
                                userItem.btValue[4] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(6, 12);
                            if (M2Share.RandomNumber.Random(4) < 3)
                            {
                                nVal = (nUpgrade + 1) * 1000;
                                userItem.DuraMax = (byte)HUtil32._MIN(65000, userItem.DuraMax + nVal);
                                userItem.Dura = (byte)HUtil32._MIN(65000, userItem.Dura + nVal);
                            }
                            break;
                        case 15:
                            nUpgrade = GetRandomRange(6, 20);
                            if (M2Share.RandomNumber.Random(40) == 0)
                            {
                                userItem.btValue[0] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(6, 20);
                            if (M2Share.RandomNumber.Random(30) == 0)
                            {
                                userItem.btValue[1] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(M2Share.Config.HelMetDCAddValueMaxLimit, M2Share.Config.HelMetDCAddValueRate);
                            if (M2Share.RandomNumber.Random(M2Share.Config.HelMetDCAddRate) == 0)
                            {
                                userItem.btValue[2] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(M2Share.Config.HelMetMCAddValueMaxLimit, M2Share.Config.HelMetMCAddValueRate);
                            if (M2Share.RandomNumber.Random(M2Share.Config.HelMetMCAddRate) == 0)
                            {
                                userItem.btValue[3] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(M2Share.Config.HelMetSCAddValueMaxLimit, M2Share.Config.HelMetSCAddValueRate);
                            if (M2Share.RandomNumber.Random(M2Share.Config.HelMetSCAddRate) == 0)
                            {
                                userItem.btValue[4] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(6, 12);
                            if (M2Share.RandomNumber.Random(4) < 3)
                            {
                                nVal = (nUpgrade + 1) * 1000;
                                userItem.DuraMax = (byte)HUtil32._MIN(65000, userItem.DuraMax + nVal);
                                userItem.Dura = (byte)HUtil32._MIN(65000, userItem.Dura + nVal);
                            }
                            break;
                    }
                    break;
            }
        }

        public void RandomUpgradeUnknownItem(UserItem userItem)
        {
            switch (ItemType)
            {
                case GoodType.ITEM_WEAPON:
                    break;
                case GoodType.ITEM_ARMOR:
                    break;
                case GoodType.ITEM_ACCESSORY:
                    int nUpgrade;
                    int nRandPoint;
                    int nVal;
                    switch (StdMode)
                    {
                        case 15:
                            nRandPoint = GetRandomRange(M2Share.Config.UnknowHelMetACAddValueMaxLimit, M2Share.Config.UnknowHelMetACAddRate);
                            if (nRandPoint > 0)
                            {
                                userItem.btValue[0] = (byte)nRandPoint;
                            }
                            nUpgrade = nRandPoint;
                            nRandPoint = GetRandomRange(M2Share.Config.UnknowHelMetMACAddValueMaxLimit, M2Share.Config.UnknowHelMetMACAddRate);
                            if (nRandPoint > 0)
                            {
                                userItem.btValue[1] = (byte)nRandPoint;
                            }
                            nUpgrade += nRandPoint;
                            nRandPoint = GetRandomRange(M2Share.Config.UnknowHelMetDCAddValueMaxLimit, M2Share.Config.UnknowHelMetDCAddRate);
                            if (nRandPoint > 0)
                            {
                                userItem.btValue[2] = (byte)nRandPoint;
                            }
                            nUpgrade += nRandPoint;
                            nRandPoint = GetRandomRange(M2Share.Config.UnknowHelMetMCAddValueMaxLimit, M2Share.Config.UnknowHelMetMCAddRate);
                            if (nRandPoint > 0)
                            {
                                userItem.btValue[3] = (byte)nRandPoint;
                            }
                            nUpgrade += nRandPoint;
                            nRandPoint = GetRandomRange(M2Share.Config.UnknowHelMetSCAddValueMaxLimit, M2Share.Config.UnknowHelMetSCAddRate);
                            if (nRandPoint > 0)
                            {
                                userItem.btValue[4] = (byte)nRandPoint;
                            }
                            nUpgrade += nRandPoint;
                            nRandPoint = GetRandomRange(6, 30);
                            if (nRandPoint > 0)
                            {
                                nVal = (nRandPoint + 1) * 1000;
                                userItem.DuraMax = (byte)HUtil32._MIN(65000, userItem.DuraMax + nVal);
                                userItem.Dura = (byte)HUtil32._MIN(65000, userItem.Dura + nVal);
                            }
                            if (M2Share.RandomNumber.Random(30) == 0)
                            {
                                userItem.btValue[7] = 1;
                            }
                            userItem.btValue[8] = 1;
                            if (nUpgrade >= 3)
                            {
                                if (userItem.btValue[0] >= 5)
                                {
                                    userItem.btValue[5] = 1;
                                    userItem.btValue[6] = (byte)(userItem.btValue[0] * 3 + 25);
                                    return;
                                }
                                if (userItem.btValue[2] >= 2)
                                {
                                    userItem.btValue[5] = 1;
                                    userItem.btValue[6] = (byte)(userItem.btValue[2] * 4 + 35);
                                    return;
                                }
                                if (userItem.btValue[3] >= 2)
                                {
                                    userItem.btValue[5] = 2;
                                    userItem.btValue[6] = (byte)(userItem.btValue[3] * 2 + 18);
                                    return;
                                }
                                if (userItem.btValue[4] >= 2)
                                {
                                    userItem.btValue[5] = 3;
                                    userItem.btValue[6] = (byte)(userItem.btValue[4] * 2 + 18);
                                    return;
                                }
                                userItem.btValue[6] = (byte)(nUpgrade * 2 + 18);
                            }
                            break;
                        case 22:
                        case 23:
                            nRandPoint = GetRandomRange(M2Share.Config.UnknowRingACAddValueMaxLimit, M2Share.Config.UnknowRingACAddRate);
                            if (nRandPoint > 0)
                            {
                                userItem.btValue[0] = (byte)nRandPoint;
                            }
                            nUpgrade = nRandPoint;
                            nRandPoint = GetRandomRange(M2Share.Config.UnknowRingMACAddValueMaxLimit, M2Share.Config.UnknowRingMACAddRate);
                            if (nRandPoint > 0)
                            {
                                userItem.btValue[1] = (byte)nRandPoint;
                            }
                            nUpgrade += nRandPoint; // 以上二项为增加项，增加防，及魔防
                            nRandPoint = GetRandomRange(M2Share.Config.UnknowRingDCAddValueMaxLimit, M2Share.Config.UnknowRingDCAddRate);
                            if (nRandPoint > 0)
                            {
                                userItem.btValue[2] = (byte)nRandPoint;
                            }
                            nUpgrade += nRandPoint;
                            nRandPoint = GetRandomRange(M2Share.Config.UnknowRingMCAddValueMaxLimit, M2Share.Config.UnknowRingMCAddRate);
                            if (nRandPoint > 0)
                            {
                                userItem.btValue[3] = (byte)nRandPoint;
                            }
                            nUpgrade += nRandPoint;
                            nRandPoint = GetRandomRange(M2Share.Config.UnknowRingSCAddValueMaxLimit, M2Share.Config.UnknowRingSCAddRate);
                            if (nRandPoint > 0)
                            {
                                userItem.btValue[4] = (byte)nRandPoint;
                            }
                            nUpgrade += nRandPoint;
                            nRandPoint = GetRandomRange(6, 30);
                            if (nRandPoint > 0)
                            {
                                nVal = (nRandPoint + 1) * 1000;
                                userItem.DuraMax = (byte)HUtil32._MIN(65000, userItem.DuraMax + nVal);
                                userItem.Dura = (byte)HUtil32._MIN(65000, userItem.Dura + nVal);
                            }
                            if (M2Share.RandomNumber.Random(30) == 0)
                            {
                                userItem.btValue[7] = 1;
                            }
                            userItem.btValue[8] = 1;
                            if (nUpgrade >= 3)
                            {
                                if (userItem.btValue[2] >= 3)
                                {
                                    userItem.btValue[5] = 1;
                                    userItem.btValue[6] = (byte)(userItem.btValue[2] * 3 + 25);
                                    return;
                                }
                                if (userItem.btValue[3] >= 3)
                                {
                                    userItem.btValue[5] = 2;
                                    userItem.btValue[6] = (byte)(userItem.btValue[3] * 2 + 18);
                                    return;
                                }
                                if (userItem.btValue[4] >= 3)
                                {
                                    userItem.btValue[5] = 3;
                                    userItem.btValue[6] = (byte)(userItem.btValue[4] * 2 + 18);
                                    return;
                                }
                                userItem.btValue[6] = (byte)(nUpgrade * 2 + 18);
                            }
                            break;
                        case 24:
                        case 26:
                            nRandPoint = GetRandomRange(M2Share.Config.UnknowNecklaceACAddValueMaxLimit, M2Share.Config.UnknowNecklaceACAddRate);
                            if (nRandPoint > 0)
                            {
                                userItem.btValue[0] = (byte)nRandPoint;
                            }
                            nUpgrade = nRandPoint;
                            nRandPoint = GetRandomRange(M2Share.Config.UnknowNecklaceMACAddValueMaxLimit, M2Share.Config.UnknowNecklaceMACAddRate);
                            if (nRandPoint > 0)
                            {
                                userItem.btValue[1] = (byte)nRandPoint;
                            }
                            nUpgrade += nRandPoint;
                            nRandPoint = GetRandomRange(M2Share.Config.UnknowNecklaceDCAddValueMaxLimit, M2Share.Config.UnknowNecklaceDCAddRate);
                            if (nRandPoint > 0)
                            {
                                userItem.btValue[2] = (byte)nRandPoint;
                            }
                            nUpgrade += nRandPoint;
                            nRandPoint = GetRandomRange(M2Share.Config.UnknowNecklaceMCAddValueMaxLimit, M2Share.Config.UnknowNecklaceMCAddRate);
                            if (nRandPoint > 0)
                            {
                                userItem.btValue[3] = (byte)nRandPoint;
                            }
                            nUpgrade += nRandPoint;
                            nRandPoint = GetRandomRange(M2Share.Config.UnknowNecklaceSCAddValueMaxLimit, M2Share.Config.UnknowNecklaceSCAddRate);
                            if (nRandPoint > 0)
                            {
                                userItem.btValue[4] = (byte)nRandPoint;
                            }
                            nUpgrade += nRandPoint;
                            nRandPoint = GetRandomRange(6, 30);
                            if (nRandPoint > 0)
                            {
                                nVal = (nRandPoint + 1) * 1000;
                                userItem.DuraMax = (byte)HUtil32._MIN(65000, userItem.DuraMax + nVal);
                                userItem.Dura = (byte)HUtil32._MIN(65000, userItem.Dura + nVal);
                            }
                            if (M2Share.RandomNumber.Random(30) == 0)
                            {
                                userItem.btValue[7] = 1;
                            }
                            userItem.btValue[8] = 1;
                            if (nUpgrade >= 2)
                            {
                                if (userItem.btValue[0] >= 3)
                                {
                                    userItem.btValue[5] = 1;
                                    userItem.btValue[6] = (byte)(userItem.btValue[0] * 3 + 25);
                                    return;
                                }
                                if (userItem.btValue[2] >= 2)
                                {
                                    userItem.btValue[5] = 1;
                                    userItem.btValue[6] = (byte)(userItem.btValue[2] * 3 + 30);
                                    return;
                                }
                                if (userItem.btValue[3] >= 2)
                                {
                                    userItem.btValue[5] = 2;
                                    userItem.btValue[6] = (byte)(userItem.btValue[3] * 2 + 20);
                                    return;
                                }
                                if (userItem.btValue[4] >= 2)
                                {
                                    userItem.btValue[5] = 3;
                                    userItem.btValue[6] = (byte)(userItem.btValue[4] * 2 + 20);
                                    return;
                                }
                                userItem.btValue[6] = (byte)(nUpgrade * 2 + 18);
                            }
                            break;
                    }
                    break;
            }
        }

        public void ApplyItemParameters(ref TAddAbility addAbility)
        {
            switch (ItemType)
            {
                case GoodType.ITEM_WEAPON:
                    addAbility.wHitPoint += Ac2;
                    if (Mac2 > 10)
                    {
                        addAbility.nHitSpeed += (ushort)(Mac2 - 10);
                    }
                    else
                    {
                        addAbility.nHitSpeed -= Mac2;
                    }
                    addAbility.btLuck += (byte)Ac;
                    addAbility.btUnLuck += (byte)Mac;
                    break;
                case GoodType.ITEM_ARMOR:
                    addAbility.wAC = HUtil32.MakeLong(HUtil32.LoWord(addAbility.wAC) + Ac, HUtil32.HiWord(addAbility.wAC) + Ac2);
                    addAbility.wMAC = HUtil32.MakeLong(HUtil32.LoWord(addAbility.wMAC) + Mac, HUtil32.HiWord(addAbility.wMAC) + Mac2);
                    addAbility.btLuck += HUtil32.LoByte(Source);
                    addAbility.btUnLuck += HUtil32.HiByte(Source);
                    break;
                case GoodType.ITEM_ACCESSORY:
                    switch (StdMode)
                    {
                        case 19:
                            addAbility.wAntiMagic += Ac2;
                            addAbility.btUnLuck += (byte)Mac;
                            addAbility.btLuck += (byte)Mac2;
                            break;
                        case 53: // 新加物品属性
                            if (M2Share.Config.AddUserItemNewValue)
                            {
                                addAbility.wAntiMagic += Ac2;
                                addAbility.btUnLuck += (byte)Mac;
                                addAbility.btLuck += (byte)Mac2;
                            }
                            else
                            {
                                addAbility.wAC = HUtil32.MakeLong(HUtil32.LoWord(addAbility.wAC) + Ac, HUtil32.HiWord(addAbility.wAC) + Ac2);
                                addAbility.wMAC = HUtil32.MakeLong(HUtil32.LoWord(addAbility.wMAC) + Mac, HUtil32.HiWord(addAbility.wMAC) + Mac2);
                            }
                            break;
                        case 63:
                            addAbility.wHP += Ac;
                            addAbility.wMP += Ac2;
                            addAbility.btUnLuck += (byte)Mac;
                            addAbility.btLuck += (byte)Mac2;
                            break;
                        case 20:
                        case 24:
                            addAbility.wHitPoint += Ac2;
                            addAbility.wSpeedPoint += Mac2;
                            break;
                        case 52:// 原本与 20,24 一个属性，现在分开单独进行设置
                            if (M2Share.Config.AddUserItemNewValue)
                            {
                                addAbility.wHitPoint += Ac2;
                                addAbility.wSpeedPoint += Mac2;
                            }
                            else
                            {
                                addAbility.wAC = HUtil32.MakeLong(HUtil32.LoWord(addAbility.wAC) + Ac, HUtil32.HiWord(addAbility.wAC) + Ac2);
                                addAbility.wMAC = HUtil32.MakeLong(HUtil32.LoWord(addAbility.wMAC) + Mac, HUtil32.HiWord(addAbility.wMAC) + Mac2);
                            }
                            break;
                        case 62:
                            addAbility.HandWeight += Ac2;
                            addAbility.Weight += Mac;
                            addAbility.WearWeight += Mac2;
                            break;
                        case 21:
                            addAbility.wHealthRecover += Ac2;
                            addAbility.wSpellRecover += Mac2;
                            addAbility.nHitSpeed += Ac;
                            addAbility.nHitSpeed -= Mac;
                            break;
                        case 54:
                            if (M2Share.Config.AddUserItemNewValue)
                            {
                                addAbility.wHealthRecover += Ac2;
                                addAbility.wSpellRecover += Mac2;
                                addAbility.nHitSpeed += Ac;
                                addAbility.nHitSpeed -= Mac;
                            }
                            else
                            {
                                addAbility.wAC = HUtil32.MakeLong(HUtil32.LoWord(addAbility.wAC) + Ac, HUtil32.HiWord(addAbility.wAC) + Ac2);
                                addAbility.wMAC = HUtil32.MakeLong(HUtil32.LoWord(addAbility.wMAC) + Mac, HUtil32.HiWord(addAbility.wMAC) + Mac2);
                            }
                            break;
                        case 64:
                            addAbility.wHealthRecover += Ac2;
                            addAbility.wSpellRecover += Mac2;
                            addAbility.nHitSpeed += Ac;
                            addAbility.nHitSpeed -= Mac;
                            break;
                        case 23:
                            addAbility.wAntiPoison += Ac2;
                            addAbility.wPoisonRecover += Mac2;
                            addAbility.nHitSpeed += Ac;
                            addAbility.nHitSpeed -= Mac;
                            break;
                    }
                    break;
            }
            addAbility.wDC = HUtil32.MakeLong(HUtil32.LoWord(addAbility.wDC) + Dc, HUtil32.HiWord(addAbility.wDC) + Dc2);
            addAbility.wMC = HUtil32.MakeLong(HUtil32.LoWord(addAbility.wMC) + Mc, HUtil32.HiWord(addAbility.wMC) + Mc2);
            addAbility.wSC = HUtil32.MakeLong(HUtil32.LoWord(addAbility.wSC) + Sc, HUtil32.HiWord(addAbility.wSC) + Sc2);
        }
    }

    public enum GoodType : byte
    {
        /// <summary>
        /// 武器
        /// </summary>
        ITEM_WEAPON = 0,
        /// <summary>
        /// 装备
        /// </summary>
        ITEM_ARMOR = 1,
        /// <summary>
        /// 辅助物品
        /// </summary>
        ITEM_ACCESSORY = 2,
        /// <summary>
        /// 其它物品
        /// </summary>
        ITEM_ETC = 3,
        /// <summary>
        /// 药水
        /// </summary>
        ITEM_LEECHDOM = 4,
        /// <summary>
        /// 金币
        /// </summary>
        ITEM_GOLD = 10
    }
}