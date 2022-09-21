using SystemModule;
using SystemModule.Data;
using SystemModule.Packet.ClientPackets;

namespace GameSvr.Items
{
    public class StdItem
    {
        public EquipmentType ItemType = 0;
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

        public void GetStandardItem(ref ClientStdItem clientStdItem)
        {
            if (clientStdItem == null)
            {
                clientStdItem = new ClientStdItem();
            }
            clientStdItem.Name = M2Share.FilterShowName(Name);
            clientStdItem.StdMode = StdMode;
            clientStdItem.Shape = Shape;
            clientStdItem.Weight = Weight;
            clientStdItem.AniCount = AniCount;
            //stdItem.reserved = Reserved;
            //stdItem.Source = (sbyte)Source;
            clientStdItem.NeedIdentify = NeedIdentify;
            clientStdItem.Looks = Looks;
            clientStdItem.DuraMax = DuraMax;
            clientStdItem.Need = Need;
            clientStdItem.NeedLevel = NeedLevel;
            clientStdItem.Price = Price;
        }

        public void GetItemAddValue(UserItem userItem, ref ClientStdItem clientStdItem)
        {
            switch (ItemType)
            {
                case EquipmentType.ITEM_WEAPON:
                    clientStdItem.DC = (ushort)HUtil32.MakeLong(Dc, Dc2 + userItem.Desc[0]);
                    clientStdItem.MC = (ushort)HUtil32.MakeLong(Mc, Mc2 + userItem.Desc[1]);
                    clientStdItem.SC = (ushort)HUtil32.MakeLong(Sc, Sc2 + userItem.Desc[2]);
                    clientStdItem.AC = HUtil32.MakeLong(Ac + userItem.Desc[3], Ac2 + userItem.Desc[5]);
                    clientStdItem.MAC = HUtil32.MakeLong(Mac + userItem.Desc[4], Mac2 + userItem.Desc[6]);
                    //if (userItem.btValue[7] - 1 < 10)
                    //{
                    //    stdItem.Source = (sbyte)userItem.btValue[7];
                    //}
                    //if (userItem.btValue[ItemAttr.WeaponUpgrade] != 0)
                    //{
                    //    stdItem.reserved = (byte)(stdItem.reserved | 1);
                    //}
                    break;
                case EquipmentType.ITEM_ARMOR:
                    clientStdItem.AC = (ushort)HUtil32.MakeLong(Ac, Ac2 + userItem.Desc[0]);
                    clientStdItem.MAC =(ushort) HUtil32.MakeLong(Mac, Mac2 + userItem.Desc[1]);
                    clientStdItem.DC = (ushort)HUtil32.MakeLong(Dc, Dc2 + userItem.Desc[2]);
                    clientStdItem.MC = (ushort)HUtil32.MakeLong(Mc, Mc2 + userItem.Desc[3]);
                    clientStdItem.SC =(ushort) HUtil32.MakeLong(Sc, Sc2 + userItem.Desc[4]);
                    break;
                case EquipmentType.ITEM_ACCESSORY:
                    clientStdItem.AC =(ushort) HUtil32.MakeLong(Ac, Ac2 + userItem.Desc[0]);
                    clientStdItem.MAC =(ushort) HUtil32.MakeLong(Mac, Mac2 + userItem.Desc[1]);
                    clientStdItem.DC = (ushort)HUtil32.MakeLong(Dc, Dc2 + userItem.Desc[2]);
                    clientStdItem.MC = (ushort)HUtil32.MakeLong(Mc, Mc2 + userItem.Desc[3]);
                    clientStdItem.SC = (ushort)HUtil32.MakeLong(Sc, Sc2 + userItem.Desc[4]);
                    if (userItem.Desc[5] > 0)
                    {
                        clientStdItem.Need = userItem.Desc[5];
                    }
                    if (userItem.Desc[6] > 0)
                    {
                        clientStdItem.NeedLevel = userItem.Desc[6];
                    }
                    break;
                case EquipmentType.ITEM_LEECHDOM:
                    clientStdItem.AC = (ushort)HUtil32.MakeLong(Ac, Ac2);
                    clientStdItem.MAC = (ushort)HUtil32.MakeLong(Mac, Mac2);
                    clientStdItem.DC = (ushort)HUtil32.MakeLong(Dc, Dc2);
                    clientStdItem.MC = (ushort)HUtil32.MakeLong(Mc, Mc2);
                    clientStdItem.SC =(ushort) HUtil32.MakeLong(Sc, Sc2);
                    break;
                default:
                    clientStdItem.AC = 0;
                    clientStdItem.MAC = 0;
                    clientStdItem.DC = 0;
                    clientStdItem.MC = 0;
                    clientStdItem.SC = 0;
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
                case EquipmentType.ITEM_WEAPON:
                    nUpgrade = GetRandomRange(M2Share.Config.WeaponDCAddValueMaxLimit, M2Share.Config.WeaponDCAddValueRate);
                    if (M2Share.RandomNumber.Random(15) == 0)
                    {
                        userItem.Desc[0] = (byte)(nUpgrade + 1);
                    }
                    nUpgrade = GetRandomRange(12, 15);
                    if (M2Share.RandomNumber.Random(20) == 0)
                    {
                        nIncp = (nUpgrade + 1) / 3;
                        if (nIncp > 0)
                        {
                            if (M2Share.RandomNumber.Random(3) != 0)
                            {
                                userItem.Desc[6] = (byte)nIncp;
                            }
                            else
                            {
                                userItem.Desc[6] = (byte)(nIncp + 10);
                            }
                        }
                    }
                    nUpgrade = GetRandomRange(12, 15);
                    if (M2Share.RandomNumber.Random(15) == 0)
                    {
                        userItem.Desc[1] = (byte)(nUpgrade + 1);
                    }
                    nUpgrade = GetRandomRange(12, 15);
                    if (M2Share.RandomNumber.Random(15) == 0)
                    {
                        userItem.Desc[2] = (byte)(nUpgrade + 1);
                    }
                    nUpgrade = GetRandomRange(12, 15);
                    if (M2Share.RandomNumber.Random(24) == 0)
                    {
                        userItem.Desc[5] = (byte)(nUpgrade / 2 + 1);
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
                        userItem.Desc[7] = (byte)(nUpgrade / 2 + 1);
                    }
                    break;
                case EquipmentType.ITEM_ARMOR:
                    nUpgrade = GetRandomRange(6, 15);
                    if (M2Share.RandomNumber.Random(30) == 0)
                    {
                        userItem.Desc[0] = (byte)(nUpgrade + 1);
                    }
                    nUpgrade = GetRandomRange(6, 15);
                    if (M2Share.RandomNumber.Random(30) == 0)
                    {
                        userItem.Desc[1] = (byte)(nUpgrade + 1);
                    }
                    nUpgrade = GetRandomRange(M2Share.Config.DressDCAddValueMaxLimit, M2Share.Config.DressDCAddValueRate);
                    if (M2Share.RandomNumber.Random(M2Share.Config.DressDCAddRate) == 0)
                    {
                        userItem.Desc[2] = (byte)(nUpgrade + 1);
                    }
                    nUpgrade = GetRandomRange(M2Share.Config.DressMCAddValueMaxLimit, M2Share.Config.DressMCAddValueRate);
                    if (M2Share.RandomNumber.Random(M2Share.Config.DressMCAddRate) == 0)
                    {
                        userItem.Desc[3] = (byte)(nUpgrade + 1);
                    }
                    nUpgrade = GetRandomRange(M2Share.Config.DressSCAddValueMaxLimit, M2Share.Config.nDressSCAddValueRate);
                    if (M2Share.RandomNumber.Random(M2Share.Config.DressSCAddRate) == 0)
                    {
                        userItem.Desc[4] = (byte)(nUpgrade + 1);
                    }
                    nUpgrade = GetRandomRange(6, 10);
                    if (M2Share.RandomNumber.Random(8) < 6)
                    {
                        nVal = (nUpgrade + 1) * 2000;
                        userItem.DuraMax = (byte)HUtil32._MIN(65000, userItem.DuraMax + nVal);
                        userItem.Dura = (byte)HUtil32._MIN(65000, userItem.Dura + nVal);
                    }
                    break;
                case EquipmentType.ITEM_ACCESSORY:
                    switch (StdMode)
                    {
                        case 20:
                        case 21:
                        case 24:
                            nUpgrade = GetRandomRange(6, 30);
                            if (M2Share.RandomNumber.Random(60) == 0)
                            {
                                userItem.Desc[0] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(6, 30);
                            if (M2Share.RandomNumber.Random(60) == 0)
                            {
                                userItem.Desc[1] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(M2Share.Config.NeckLace202124DCAddValueMaxLimit, M2Share.Config.NeckLace202124DCAddValueRate);
                            if (M2Share.RandomNumber.Random(M2Share.Config.NeckLace202124DCAddRate) == 0)
                            {
                                userItem.Desc[2] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(M2Share.Config.NeckLace202124MCAddValueMaxLimit, M2Share.Config.NeckLace202124MCAddValueRate);
                            if (M2Share.RandomNumber.Random(M2Share.Config.NeckLace202124MCAddRate) == 0)
                            {
                                userItem.Desc[3] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(M2Share.Config.NeckLace202124SCAddValueMaxLimit, M2Share.Config.NeckLace202124SCAddValueRate);
                            if (M2Share.RandomNumber.Random(M2Share.Config.NeckLace202124SCAddRate) == 0)
                            {
                                userItem.Desc[4] = (byte)(nUpgrade + 1);
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
                                userItem.Desc[0] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(6, 20);
                            if (M2Share.RandomNumber.Random(20) == 0)
                            {
                                userItem.Desc[1] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(M2Share.Config.ArmRing26DCAddValueMaxLimit, M2Share.Config.ArmRing26DCAddValueRate);
                            if (M2Share.RandomNumber.Random(M2Share.Config.ArmRing26DCAddRate) == 0)
                            {
                                userItem.Desc[2] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(M2Share.Config.ArmRing26MCAddValueMaxLimit, M2Share.Config.ArmRing26MCAddValueRate);
                            if (M2Share.RandomNumber.Random(M2Share.Config.ArmRing26MCAddRate) == 0)
                            {
                                userItem.Desc[3] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(M2Share.Config.ArmRing26SCAddValueMaxLimit, M2Share.Config.ArmRing26SCAddValueRate);
                            if (M2Share.RandomNumber.Random(M2Share.Config.ArmRing26SCAddRate) == 0)
                            {
                                userItem.Desc[4] = (byte)(nUpgrade + 1);
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
                                userItem.Desc[0] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(6, 20);
                            if (M2Share.RandomNumber.Random(40) == 0)
                            {
                                userItem.Desc[1] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(M2Share.Config.NeckLace19DCAddValueMaxLimit, M2Share.Config.NeckLace19DCAddValueRate);
                            if (M2Share.RandomNumber.Random(M2Share.Config.NeckLace19DCAddRate) == 0)
                            {
                                userItem.Desc[2] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(M2Share.Config.NeckLace19MCAddValueMaxLimit, M2Share.Config.NeckLace19MCAddValueRate);
                            if (M2Share.RandomNumber.Random(M2Share.Config.NeckLace19MCAddRate) == 0)
                            {
                                userItem.Desc[3] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(M2Share.Config.NeckLace19SCAddValueMaxLimit, M2Share.Config.NeckLace19SCAddValueRate);
                            if (M2Share.RandomNumber.Random(M2Share.Config.NeckLace19SCAddRate) == 0)
                            {
                                userItem.Desc[4] = (byte)(nUpgrade + 1);
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
                                userItem.Desc[2] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(M2Share.Config.Ring22MCAddValueMaxLimit, M2Share.Config.Ring22MCAddValueRate);
                            if (M2Share.RandomNumber.Random(M2Share.Config.Ring22MCAddRate) == 0)
                            {
                                userItem.Desc[3] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(M2Share.Config.Ring22SCAddValueMaxLimit, M2Share.Config.Ring22SCAddValueRate);
                            if (M2Share.RandomNumber.Random(M2Share.Config.Ring22SCAddRate) == 0)
                            {
                                userItem.Desc[4] = (byte)(nUpgrade + 1);
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
                                userItem.Desc[0] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(6, 20);
                            if (M2Share.RandomNumber.Random(40) == 0)
                            {
                                userItem.Desc[1] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(M2Share.Config.Ring23DCAddValueMaxLimit, M2Share.Config.Ring23DCAddValueRate);
                            if (M2Share.RandomNumber.Random(M2Share.Config.Ring23DCAddRate) == 0)
                            {
                                userItem.Desc[2] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(M2Share.Config.Ring23MCAddValueMaxLimit, M2Share.Config.Ring23MCAddValueRate);
                            if (M2Share.RandomNumber.Random(M2Share.Config.Ring23MCAddRate) == 0)
                            {
                                userItem.Desc[3] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(M2Share.Config.Ring23SCAddValueMaxLimit, M2Share.Config.Ring23SCAddValueRate);
                            if (M2Share.RandomNumber.Random(M2Share.Config.Ring23SCAddRate) == 0)
                            {
                                userItem.Desc[4] = (byte)(nUpgrade + 1);
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
                                userItem.Desc[0] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(6, 20);
                            if (M2Share.RandomNumber.Random(30) == 0)
                            {
                                userItem.Desc[1] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(M2Share.Config.HelMetDCAddValueMaxLimit, M2Share.Config.HelMetDCAddValueRate);
                            if (M2Share.RandomNumber.Random(M2Share.Config.HelMetDCAddRate) == 0)
                            {
                                userItem.Desc[2] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(M2Share.Config.HelMetMCAddValueMaxLimit, M2Share.Config.HelMetMCAddValueRate);
                            if (M2Share.RandomNumber.Random(M2Share.Config.HelMetMCAddRate) == 0)
                            {
                                userItem.Desc[3] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(M2Share.Config.HelMetSCAddValueMaxLimit, M2Share.Config.HelMetSCAddValueRate);
                            if (M2Share.RandomNumber.Random(M2Share.Config.HelMetSCAddRate) == 0)
                            {
                                userItem.Desc[4] = (byte)(nUpgrade + 1);
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
                case EquipmentType.ITEM_WEAPON:
                    break;
                case EquipmentType.ITEM_ARMOR:
                    break;
                case EquipmentType.ITEM_ACCESSORY:
                    int nUpgrade;
                    int nRandPoint;
                    int nVal;
                    switch (StdMode)
                    {
                        case 15:
                            nRandPoint = GetRandomRange(M2Share.Config.UnknowHelMetACAddValueMaxLimit, M2Share.Config.UnknowHelMetACAddRate);
                            if (nRandPoint > 0)
                            {
                                userItem.Desc[0] = (byte)nRandPoint;
                            }
                            nUpgrade = nRandPoint;
                            nRandPoint = GetRandomRange(M2Share.Config.UnknowHelMetMACAddValueMaxLimit, M2Share.Config.UnknowHelMetMACAddRate);
                            if (nRandPoint > 0)
                            {
                                userItem.Desc[1] = (byte)nRandPoint;
                            }
                            nUpgrade += nRandPoint;
                            nRandPoint = GetRandomRange(M2Share.Config.UnknowHelMetDCAddValueMaxLimit, M2Share.Config.UnknowHelMetDCAddRate);
                            if (nRandPoint > 0)
                            {
                                userItem.Desc[2] = (byte)nRandPoint;
                            }
                            nUpgrade += nRandPoint;
                            nRandPoint = GetRandomRange(M2Share.Config.UnknowHelMetMCAddValueMaxLimit, M2Share.Config.UnknowHelMetMCAddRate);
                            if (nRandPoint > 0)
                            {
                                userItem.Desc[3] = (byte)nRandPoint;
                            }
                            nUpgrade += nRandPoint;
                            nRandPoint = GetRandomRange(M2Share.Config.UnknowHelMetSCAddValueMaxLimit, M2Share.Config.UnknowHelMetSCAddRate);
                            if (nRandPoint > 0)
                            {
                                userItem.Desc[4] = (byte)nRandPoint;
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
                                userItem.Desc[7] = 1;
                            }
                            userItem.Desc[8] = 1;
                            if (nUpgrade >= 3)
                            {
                                if (userItem.Desc[0] >= 5)
                                {
                                    userItem.Desc[5] = 1;
                                    userItem.Desc[6] = (byte)(userItem.Desc[0] * 3 + 25);
                                    return;
                                }
                                if (userItem.Desc[2] >= 2)
                                {
                                    userItem.Desc[5] = 1;
                                    userItem.Desc[6] = (byte)(userItem.Desc[2] * 4 + 35);
                                    return;
                                }
                                if (userItem.Desc[3] >= 2)
                                {
                                    userItem.Desc[5] = 2;
                                    userItem.Desc[6] = (byte)(userItem.Desc[3] * 2 + 18);
                                    return;
                                }
                                if (userItem.Desc[4] >= 2)
                                {
                                    userItem.Desc[5] = 3;
                                    userItem.Desc[6] = (byte)(userItem.Desc[4] * 2 + 18);
                                    return;
                                }
                                userItem.Desc[6] = (byte)(nUpgrade * 2 + 18);
                            }
                            break;
                        case 22:
                        case 23:
                            nRandPoint = GetRandomRange(M2Share.Config.UnknowRingACAddValueMaxLimit, M2Share.Config.UnknowRingACAddRate);
                            if (nRandPoint > 0)
                            {
                                userItem.Desc[0] = (byte)nRandPoint;
                            }
                            nUpgrade = nRandPoint;
                            nRandPoint = GetRandomRange(M2Share.Config.UnknowRingMACAddValueMaxLimit, M2Share.Config.UnknowRingMACAddRate);
                            if (nRandPoint > 0)
                            {
                                userItem.Desc[1] = (byte)nRandPoint;
                            }
                            nUpgrade += nRandPoint; // 以上二项为增加项，增加防，及魔防
                            nRandPoint = GetRandomRange(M2Share.Config.UnknowRingDCAddValueMaxLimit, M2Share.Config.UnknowRingDCAddRate);
                            if (nRandPoint > 0)
                            {
                                userItem.Desc[2] = (byte)nRandPoint;
                            }
                            nUpgrade += nRandPoint;
                            nRandPoint = GetRandomRange(M2Share.Config.UnknowRingMCAddValueMaxLimit, M2Share.Config.UnknowRingMCAddRate);
                            if (nRandPoint > 0)
                            {
                                userItem.Desc[3] = (byte)nRandPoint;
                            }
                            nUpgrade += nRandPoint;
                            nRandPoint = GetRandomRange(M2Share.Config.UnknowRingSCAddValueMaxLimit, M2Share.Config.UnknowRingSCAddRate);
                            if (nRandPoint > 0)
                            {
                                userItem.Desc[4] = (byte)nRandPoint;
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
                                userItem.Desc[7] = 1;
                            }
                            userItem.Desc[8] = 1;
                            if (nUpgrade >= 3)
                            {
                                if (userItem.Desc[2] >= 3)
                                {
                                    userItem.Desc[5] = 1;
                                    userItem.Desc[6] = (byte)(userItem.Desc[2] * 3 + 25);
                                    return;
                                }
                                if (userItem.Desc[3] >= 3)
                                {
                                    userItem.Desc[5] = 2;
                                    userItem.Desc[6] = (byte)(userItem.Desc[3] * 2 + 18);
                                    return;
                                }
                                if (userItem.Desc[4] >= 3)
                                {
                                    userItem.Desc[5] = 3;
                                    userItem.Desc[6] = (byte)(userItem.Desc[4] * 2 + 18);
                                    return;
                                }
                                userItem.Desc[6] = (byte)(nUpgrade * 2 + 18);
                            }
                            break;
                        case 24:
                        case 26:
                            nRandPoint = GetRandomRange(M2Share.Config.UnknowNecklaceACAddValueMaxLimit, M2Share.Config.UnknowNecklaceACAddRate);
                            if (nRandPoint > 0)
                            {
                                userItem.Desc[0] = (byte)nRandPoint;
                            }
                            nUpgrade = nRandPoint;
                            nRandPoint = GetRandomRange(M2Share.Config.UnknowNecklaceMACAddValueMaxLimit, M2Share.Config.UnknowNecklaceMACAddRate);
                            if (nRandPoint > 0)
                            {
                                userItem.Desc[1] = (byte)nRandPoint;
                            }
                            nUpgrade += nRandPoint;
                            nRandPoint = GetRandomRange(M2Share.Config.UnknowNecklaceDCAddValueMaxLimit, M2Share.Config.UnknowNecklaceDCAddRate);
                            if (nRandPoint > 0)
                            {
                                userItem.Desc[2] = (byte)nRandPoint;
                            }
                            nUpgrade += nRandPoint;
                            nRandPoint = GetRandomRange(M2Share.Config.UnknowNecklaceMCAddValueMaxLimit, M2Share.Config.UnknowNecklaceMCAddRate);
                            if (nRandPoint > 0)
                            {
                                userItem.Desc[3] = (byte)nRandPoint;
                            }
                            nUpgrade += nRandPoint;
                            nRandPoint = GetRandomRange(M2Share.Config.UnknowNecklaceSCAddValueMaxLimit, M2Share.Config.UnknowNecklaceSCAddRate);
                            if (nRandPoint > 0)
                            {
                                userItem.Desc[4] = (byte)nRandPoint;
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
                                userItem.Desc[7] = 1;
                            }
                            userItem.Desc[8] = 1;
                            if (nUpgrade >= 2)
                            {
                                if (userItem.Desc[0] >= 3)
                                {
                                    userItem.Desc[5] = 1;
                                    userItem.Desc[6] = (byte)(userItem.Desc[0] * 3 + 25);
                                    return;
                                }
                                if (userItem.Desc[2] >= 2)
                                {
                                    userItem.Desc[5] = 1;
                                    userItem.Desc[6] = (byte)(userItem.Desc[2] * 3 + 30);
                                    return;
                                }
                                if (userItem.Desc[3] >= 2)
                                {
                                    userItem.Desc[5] = 2;
                                    userItem.Desc[6] = (byte)(userItem.Desc[3] * 2 + 20);
                                    return;
                                }
                                if (userItem.Desc[4] >= 2)
                                {
                                    userItem.Desc[5] = 3;
                                    userItem.Desc[6] = (byte)(userItem.Desc[4] * 2 + 20);
                                    return;
                                }
                                userItem.Desc[6] = (byte)(nUpgrade * 2 + 18);
                            }
                            break;
                    }
                    break;
            }
        }

        public void ApplyItemParameters(ref AddAbility addAbility)
        {
            switch (ItemType)
            {
                case EquipmentType.ITEM_WEAPON:
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
                case EquipmentType.ITEM_ARMOR:
                    addAbility.AC = HUtil32.MakeLong(HUtil32.LoWord(addAbility.AC) + Ac, HUtil32.HiWord(addAbility.AC) + Ac2);
                    addAbility.MAC = HUtil32.MakeLong(HUtil32.LoWord(addAbility.MAC) + Mac, HUtil32.HiWord(addAbility.MAC) + Mac2);
                    addAbility.btLuck += HUtil32.LoByte(Source);
                    addAbility.btUnLuck += HUtil32.HiByte(Source);
                    break;
                case EquipmentType.ITEM_ACCESSORY:
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
                                addAbility.AC = HUtil32.MakeLong(HUtil32.LoWord(addAbility.AC) + Ac, HUtil32.HiWord(addAbility.AC) + Ac2);
                                addAbility.MAC = HUtil32.MakeLong(HUtil32.LoWord(addAbility.MAC) + Mac, HUtil32.HiWord(addAbility.MAC) + Mac2);
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
                                addAbility.AC = HUtil32.MakeLong(HUtil32.LoWord(addAbility.AC) + Ac, HUtil32.HiWord(addAbility.AC) + Ac2);
                                addAbility.MAC = HUtil32.MakeLong(HUtil32.LoWord(addAbility.MAC) + Mac, HUtil32.HiWord(addAbility.MAC) + Mac2);
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
                                addAbility.AC = HUtil32.MakeLong(HUtil32.LoWord(addAbility.AC) + Ac, HUtil32.HiWord(addAbility.AC) + Ac2);
                                addAbility.MAC = HUtil32.MakeLong(HUtil32.LoWord(addAbility.MAC) + Mac, HUtil32.HiWord(addAbility.MAC) + Mac2);
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
            addAbility.DC = HUtil32.MakeLong(HUtil32.LoWord(addAbility.DC) + Dc, HUtil32.HiWord(addAbility.DC) + Dc2);
            addAbility.MC = HUtil32.MakeLong(HUtil32.LoWord(addAbility.MC) + Mc, HUtil32.HiWord(addAbility.MC) + Mc2);
            addAbility.SC = HUtil32.MakeLong(HUtil32.LoWord(addAbility.SC) + Sc, HUtil32.HiWord(addAbility.SC) + Sc2);
        }
    }

    public enum EquipmentType : byte
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