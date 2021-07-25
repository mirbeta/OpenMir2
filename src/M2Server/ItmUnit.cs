using SystemModule;
using System.Collections.Generic;
using System.IO;

namespace M2Server
{
    public class ItemUnit
    {
        private IList<TItemName> m_ItemNameList = null;

        public ItemUnit()
        {
            m_ItemNameList = new List<TItemName>();
        }

        ~ItemUnit()
        {
            for (var i = 0; i < m_ItemNameList.Count; i ++ )
            {
                m_ItemNameList[i] = null;
            }
            m_ItemNameList = null;
        }

        public string GetCustomItemName(int nMakeIndex, int nItemIndex)
        {
            var result = string.Empty;
            TItemName ItemName;
            for (var i = m_ItemNameList.Count - 1; i >= 0; i--)
            {
                ItemName = m_ItemNameList[i];
                if (ItemName.nMakeIndex != nMakeIndex || ItemName.nItemIndex != nItemIndex) continue;
                result = ItemName.sItemName;
                break;
            }
            return result;
        }

        public bool AddCustomItemName(int nMakeIndex, int nItemIndex, string sItemName)
        {
            var result = false;
            TItemName ItemName;
            for (var i = m_ItemNameList.Count - 1; i >= 0; i--)
            {
                ItemName = m_ItemNameList[i];
                if (ItemName.nMakeIndex == nMakeIndex && ItemName.nItemIndex == nItemIndex)
                {
                    return result;
                }
            }
            ItemName = new TItemName();
            ItemName.nMakeIndex = nMakeIndex;
            ItemName.nItemIndex = nItemIndex;
            ItemName.sItemName = sItemName;
            m_ItemNameList.Add(ItemName);
            result = true;
            return result;
        }

        public void DelCustomItemName(int nMakeIndex, int nItemIndex)
        {
            for (var i = 0; i < m_ItemNameList.Count; i++)
            {
                var itemName = m_ItemNameList[i];
                if (itemName.nMakeIndex == nMakeIndex && itemName.nItemIndex == nItemIndex)
                {
                    itemName = null;
                    m_ItemNameList.RemoveAt(i);
                    return;
                }
            }
        }

        public void LoadCustomItemName()
        {
            string sLineText;
            var sMakeIndex = string.Empty;
            var sItemIndex = string.Empty;
            var sItemName = string.Empty;
            var sFileName = M2Share.g_Config.sEnvirDir + "ItemNameList.txt";
            var LoadList = new StringList();
            if (File.Exists(sFileName))
            {
                m_ItemNameList.Clear();
                LoadList.LoadFromFile(sFileName);
                for (var i = 0; i < LoadList.Count; i++)
                {
                    sLineText = LoadList[i].Trim();
                    sLineText = HUtil32.GetValidStr3(sLineText, ref sMakeIndex, new string[] { " ", "\t" });
                    sLineText = HUtil32.GetValidStr3(sLineText, ref sItemIndex, new string[] { " ", "\t" });
                    sLineText = HUtil32.GetValidStr3(sLineText, ref sItemName, new string[] { " ", "\t" });
                    var nMakeIndex = HUtil32.Str_ToInt(sMakeIndex, -1);
                    var nItemIndex = HUtil32.Str_ToInt(sItemIndex, -1);
                    if (nMakeIndex < 0 || nItemIndex < 0) continue;
                    var itemName = new TItemName();
                    itemName.nMakeIndex = nMakeIndex;
                    itemName.nItemIndex = nItemIndex;
                    itemName.sItemName = sItemName;
                    m_ItemNameList.Add(itemName);
                }
            }
            else
            {
                LoadList.SaveToFile(sFileName);
            }
            LoadList = null;
        }

        public void SaveCustomItemName()
        {
            var sFileName = M2Share.g_Config.sEnvirDir + "ItemNameList.txt";
            var SaveList = new StringList();
            for (var i = m_ItemNameList.Count - 1; i >= 0; i--)
            {
                var ItemName = m_ItemNameList[i];
                SaveList.Add(ItemName.nMakeIndex + "\t" + ItemName.nItemIndex + "\t" + ItemName.sItemName);
            }
            SaveList.SaveToFile(sFileName);
            SaveList = null;
        }
    } 

    public class TItem
    {
        public byte ItemType = 0;
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
        public ushort AC = 0;
        public ushort AC2 = 0;
        public ushort MAC = 0;
        public ushort MAC2 = 0;
        public ushort DC = 0;
        public ushort DC2 = 0;
        public ushort MC = 0;
        public ushort MC2 = 0;
        public ushort SC = 0;
        public ushort SC2 = 0;
        public int Need = 0;
        public int NeedLevel = 0;
        public int Price = 0;
        public bool Light = false;

        private int GetRandomRange(int nCount, int nRate)
        {
            var result = 0;
            for (var i = 0; i < nCount; i ++ )
            {
                if (M2Share.RandomNumber.Random(nRate) == 0)
                {
                    result ++;
                }
            }
            return result;
        }

        public void GetStandardItem(ref TStdItem StdItem)
        {
            StdItem = new TStdItem();
            StdItem.Name = M2Share.FilterShowName(Name);
            StdItem.StdMode = StdMode;
            StdItem.Shape = Shape;
            StdItem.Weight = Weight;
            StdItem.AniCount = AniCount;
            StdItem.reserved = Reserved;
            StdItem.Source = (sbyte)Source;
            StdItem.NeedIdentify = NeedIdentify;
            StdItem.Looks = Looks;
            StdItem.DuraMax = DuraMax;
            StdItem.Need = Need;
            StdItem.NeedLevel = NeedLevel;
            StdItem.Price = (uint)Price;
        }

        public void GetItemAddValue(TUserItem UserItem, ref TStdItem StdItem)
        {
            switch(ItemType)
            {
                case grobal2.ITEM_WEAPON:
                    StdItem.DC = HUtil32.MakeLong(DC, DC2 + UserItem.btValue[0]);
                    StdItem.MC = HUtil32.MakeLong(MC, MC2 + UserItem.btValue[1]);
                    StdItem.SC = HUtil32.MakeLong(SC, SC2 + UserItem.btValue[2]);
                    StdItem.AC = HUtil32.MakeLong(AC + UserItem.btValue[3], AC2 + UserItem.btValue[5]);
                    StdItem.MAC = HUtil32.MakeLong(MAC + UserItem.btValue[4], MAC2 + UserItem.btValue[6]);
                    if (UserItem.btValue[7] - 1 < 10)
                    {
                        StdItem.Source = (sbyte)UserItem.btValue[7];
                    }
                    if (UserItem.btValue[10] != 0)
                    {
                        StdItem.reserved = (byte)(StdItem.reserved | 1);
                    }
                    break;
                case grobal2.ITEM_ARMOR:
                    StdItem.AC = HUtil32.MakeLong(AC, AC2 + UserItem.btValue[0]);
                    StdItem.MAC = HUtil32.MakeLong(MAC, MAC2 + UserItem.btValue[1]);
                    StdItem.DC = HUtil32.MakeLong(DC, DC2 + UserItem.btValue[2]);
                    StdItem.MC = HUtil32.MakeLong(MC, MC2 + UserItem.btValue[3]);
                    StdItem.SC = HUtil32.MakeLong(SC, SC2 + UserItem.btValue[4]);
                    break;
                case grobal2.ITEM_ACCESSORY:
                    StdItem.AC = HUtil32.MakeLong(AC, AC2 + UserItem.btValue[0]);
                    StdItem.MAC = HUtil32.MakeLong(MAC, MAC2 + UserItem.btValue[1]);
                    StdItem.DC = HUtil32.MakeLong(DC, DC2 + UserItem.btValue[2]);
                    StdItem.MC = HUtil32.MakeLong(MC, MC2 + UserItem.btValue[3]);
                    StdItem.SC = HUtil32.MakeLong(SC, SC2 + UserItem.btValue[4]);
                    if (UserItem.btValue[5] > 0)
                    {
                        StdItem.Need = UserItem.btValue[5];
                    }
                    if (UserItem.btValue[6] > 0)
                    {
                        StdItem.NeedLevel = UserItem.btValue[6];
                    }
                    break;
                case grobal2.ITEM_LEECHDOM:
                    StdItem.AC = HUtil32.MakeLong(AC, AC2);
                    StdItem.MAC = HUtil32.MakeLong(MAC, MAC2);
                    StdItem.DC = HUtil32.MakeLong(DC, DC2);
                    StdItem.MC = HUtil32.MakeLong(MC, MC2);
                    StdItem.SC = HUtil32.MakeLong(SC, SC2);
                    break;
                default:
                    StdItem.AC = 0;
                    StdItem.MAC = 0;
                    StdItem.DC = 0;
                    StdItem.MC = 0;
                    StdItem.SC = 0;
                    StdItem.Source = 0;
                    StdItem.reserved = 0;
                    break;
            }
        }
        
        public void RandomUpgradeItem(TUserItem UserItem)
        {
            int nUpgrade;
            int nIncp;
            int nVal;
            switch(ItemType)
            {
                case grobal2.ITEM_WEAPON:
                    nUpgrade = GetRandomRange(M2Share.g_Config.nWeaponDCAddValueMaxLimit, M2Share.g_Config.nWeaponDCAddValueRate);
                    if (M2Share.RandomNumber.Random(15) == 0)
                    {
                        UserItem.btValue[0] = (byte)(nUpgrade + 1);
                    }
                    nUpgrade = GetRandomRange(12, 15);
                    if (M2Share.RandomNumber.Random(20) == 0)
                    {
                        nIncp = (nUpgrade + 1) / 3;
                        if (nIncp > 0)
                        {
                            if (M2Share.RandomNumber.Random(3) != 0)
                            {
                                UserItem.btValue[6] = (byte)nIncp;
                            }
                            else
                            {
                                UserItem.btValue[6] = (byte)(nIncp + 10);
                            }
                        }
                    }
                    nUpgrade = GetRandomRange(12, 15);
                    if (M2Share.RandomNumber.Random(15) == 0)
                    {
                        UserItem.btValue[1] = (byte)(nUpgrade + 1);
                    }
                    nUpgrade = GetRandomRange(12, 15);
                    if (M2Share.RandomNumber.Random(15) == 0)
                    {
                        UserItem.btValue[2] = (byte)(nUpgrade + 1);
                    }
                    nUpgrade = GetRandomRange(12, 15);
                    if (M2Share.RandomNumber.Random(24) == 0)
                    {
                        UserItem.btValue[5] = (byte)(nUpgrade / 2 + 1);
                    }
                    nUpgrade = GetRandomRange(12, 12);
                    if (M2Share.RandomNumber.Random(3) < 2)
                    {
                        nVal = (nUpgrade + 1) * 2000;
                        UserItem.DuraMax = (byte)HUtil32._MIN(65000, UserItem.DuraMax + nVal);
                        UserItem.Dura = (byte)HUtil32._MIN(65000, UserItem.Dura + nVal);
                    }
                    nUpgrade = GetRandomRange(12, 15);
                    if (M2Share.RandomNumber.Random(10) == 0)
                    {
                        UserItem.btValue[7] = (byte)(nUpgrade / 2 + 1);
                    }
                    break;
                case grobal2.ITEM_ARMOR:
                    nUpgrade = GetRandomRange(6, 15);
                    if (M2Share.RandomNumber.Random(30) == 0)
                    {
                        UserItem.btValue[0] = (byte)(nUpgrade + 1);
                    }
                    nUpgrade = GetRandomRange(6, 15);
                    if (M2Share.RandomNumber.Random(30) == 0)
                    {
                        UserItem.btValue[1] = (byte)(nUpgrade + 1);
                    }
                    nUpgrade = GetRandomRange(M2Share.g_Config.nDressDCAddValueMaxLimit, M2Share.g_Config.nDressDCAddValueRate);
                    if (M2Share.RandomNumber.Random(M2Share.g_Config.nDressDCAddRate) == 0)
                    {
                        UserItem.btValue[2] = (byte)(nUpgrade + 1);
                    }
                    nUpgrade = GetRandomRange(M2Share.g_Config.nDressMCAddValueMaxLimit, M2Share.g_Config.nDressMCAddValueRate);
                    if (M2Share.RandomNumber.Random(M2Share.g_Config.nDressMCAddRate) == 0)
                    {
                        UserItem.btValue[3] = (byte)(nUpgrade + 1);
                    }
                    nUpgrade = GetRandomRange(M2Share.g_Config.nDressSCAddValueMaxLimit, M2Share.g_Config.nDressSCAddValueRate);
                    if (M2Share.RandomNumber.Random(M2Share.g_Config.nDressSCAddRate) == 0)
                    {
                        UserItem.btValue[4] = (byte)(nUpgrade + 1);
                    }
                    nUpgrade = GetRandomRange(6, 10);
                    if (M2Share.RandomNumber.Random(8) < 6)
                    {
                        nVal = (nUpgrade + 1) * 2000;
                        UserItem.DuraMax = (byte)HUtil32._MIN(65000, UserItem.DuraMax + nVal);
                        UserItem.Dura = (byte)HUtil32._MIN(65000, UserItem.Dura + nVal);
                    }
                    break;
                case grobal2.ITEM_ACCESSORY:
                    switch(StdMode)
                    {
                        case 20:
                        case 21:
                        case 24:
                            nUpgrade = GetRandomRange(6, 30);
                            if (M2Share.RandomNumber.Random(60) == 0)
                            {
                                UserItem.btValue[0] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(6, 30);
                            if (M2Share.RandomNumber.Random(60) == 0)
                            {
                                UserItem.btValue[1] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(M2Share.g_Config.nNeckLace202124DCAddValueMaxLimit, M2Share.g_Config.nNeckLace202124DCAddValueRate);
                            if (M2Share.RandomNumber.Random(M2Share.g_Config.nNeckLace202124DCAddRate) == 0)
                            {
                                UserItem.btValue[2] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(M2Share.g_Config.nNeckLace202124MCAddValueMaxLimit, M2Share.g_Config.nNeckLace202124MCAddValueRate);
                            if (M2Share.RandomNumber.Random(M2Share.g_Config.nNeckLace202124MCAddRate) == 0)
                            {
                                UserItem.btValue[3] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(M2Share.g_Config.nNeckLace202124SCAddValueMaxLimit, M2Share.g_Config.nNeckLace202124SCAddValueRate);
                            if (M2Share.RandomNumber.Random(M2Share.g_Config.nNeckLace202124SCAddRate) == 0)
                            {
                                UserItem.btValue[4] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(6, 12);
                            if (M2Share.RandomNumber.Random(20) < 15)
                            {
                                nVal = (nUpgrade + 1) * 1000;
                                UserItem.DuraMax = (byte)HUtil32._MIN(65000, UserItem.DuraMax + nVal);
                                UserItem.Dura = (byte)HUtil32._MIN(65000, UserItem.Dura + nVal);
                            }
                            break;
                        case 26:
                            nUpgrade = GetRandomRange(6, 20);
                            if (M2Share.RandomNumber.Random(20) == 0)
                            {
                                UserItem.btValue[0] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(6, 20);
                            if (M2Share.RandomNumber.Random(20) == 0)
                            {
                                UserItem.btValue[1] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(M2Share.g_Config.nArmRing26DCAddValueMaxLimit, M2Share.g_Config.nArmRing26DCAddValueRate);
                            if (M2Share.RandomNumber.Random(M2Share.g_Config.nArmRing26DCAddRate) == 0)
                            {
                                UserItem.btValue[2] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(M2Share.g_Config.nArmRing26MCAddValueMaxLimit, M2Share.g_Config.nArmRing26MCAddValueRate);
                            if (M2Share.RandomNumber.Random(M2Share.g_Config.nArmRing26MCAddRate) == 0)
                            {
                                UserItem.btValue[3] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(M2Share.g_Config.nArmRing26SCAddValueMaxLimit, M2Share.g_Config.nArmRing26SCAddValueRate);
                            if (M2Share.RandomNumber.Random(M2Share.g_Config.nArmRing26SCAddRate) == 0)
                            {
                                UserItem.btValue[4] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(6, 12);
                            if (M2Share.RandomNumber.Random(20) < 15)
                            {
                                nVal = (nUpgrade + 1) * 1000;
                                UserItem.DuraMax = (byte)HUtil32._MIN(65000, UserItem.DuraMax + nVal);
                                UserItem.Dura = (byte)HUtil32._MIN(65000, UserItem.Dura + nVal);
                            }
                            break;
                        case 19:
                            nUpgrade = GetRandomRange(6, 20);
                            if (M2Share.RandomNumber.Random(40) == 0)
                            {
                                UserItem.btValue[0] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(6, 20);
                            if (M2Share.RandomNumber.Random(40) == 0)
                            {
                                UserItem.btValue[1] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(M2Share.g_Config.nNeckLace19DCAddValueMaxLimit, M2Share.g_Config.nNeckLace19DCAddValueRate);
                            if (M2Share.RandomNumber.Random(M2Share.g_Config.nNeckLace19DCAddRate) == 0)
                            {
                                UserItem.btValue[2] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(M2Share.g_Config.nNeckLace19MCAddValueMaxLimit, M2Share.g_Config.nNeckLace19MCAddValueRate);
                            if (M2Share.RandomNumber.Random(M2Share.g_Config.nNeckLace19MCAddRate) == 0)
                            {
                                UserItem.btValue[3] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(M2Share.g_Config.nNeckLace19SCAddValueMaxLimit, M2Share.g_Config.nNeckLace19SCAddValueRate);
                            if (M2Share.RandomNumber.Random(M2Share.g_Config.nNeckLace19SCAddRate) == 0)
                            {
                                UserItem.btValue[4] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(6, 10);
                            if (M2Share.RandomNumber.Random(4) < 3)
                            {
                                nVal = (nUpgrade + 1) * 1000;
                                UserItem.DuraMax = (byte)HUtil32._MIN(65000, UserItem.DuraMax + nVal);
                                UserItem.Dura = (byte)HUtil32._MIN(65000, UserItem.Dura + nVal);
                            }
                            break;
                        case 22:
                            nUpgrade = GetRandomRange(M2Share.g_Config.nRing22DCAddValueMaxLimit, M2Share.g_Config.nRing22DCAddValueRate);
                            if (M2Share.RandomNumber.Random(M2Share.g_Config.nRing22DCAddRate) == 0)
                            {
                                UserItem.btValue[2] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(M2Share.g_Config.nRing22MCAddValueMaxLimit, M2Share.g_Config.nRing22MCAddValueRate);
                            if (M2Share.RandomNumber.Random(M2Share.g_Config.nRing22MCAddRate) == 0)
                            {
                                UserItem.btValue[3] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(M2Share.g_Config.nRing22SCAddValueMaxLimit, M2Share.g_Config.nRing22SCAddValueRate);
                            if (M2Share.RandomNumber.Random(M2Share.g_Config.nRing22SCAddRate) == 0)
                            {
                                UserItem.btValue[4] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(6, 12);
                            if (M2Share.RandomNumber.Random(4) < 3)
                            {
                                nVal = (nUpgrade + 1) * 1000;
                                UserItem.DuraMax = (byte)HUtil32._MIN(65000, UserItem.DuraMax + nVal);
                                UserItem.Dura = (byte)HUtil32._MIN(65000, UserItem.Dura + nVal);
                            }
                            break;
                        case 23:
                            nUpgrade = GetRandomRange(6, 20);
                            if (M2Share.RandomNumber.Random(40) == 0)
                            {
                                UserItem.btValue[0] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(6, 20);
                            if (M2Share.RandomNumber.Random(40) == 0)
                            {
                                UserItem.btValue[1] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(M2Share.g_Config.nRing23DCAddValueMaxLimit, M2Share.g_Config.nRing23DCAddValueRate);
                            if (M2Share.RandomNumber.Random(M2Share.g_Config.nRing23DCAddRate) == 0)
                            {
                                UserItem.btValue[2] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(M2Share.g_Config.nRing23MCAddValueMaxLimit, M2Share.g_Config.nRing23MCAddValueRate);
                            if (M2Share.RandomNumber.Random(M2Share.g_Config.nRing23MCAddRate) == 0)
                            {
                                UserItem.btValue[3] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(M2Share.g_Config.nRing23SCAddValueMaxLimit, M2Share.g_Config.nRing23SCAddValueRate);
                            if (M2Share.RandomNumber.Random(M2Share.g_Config.nRing23SCAddRate) == 0)
                            {
                                UserItem.btValue[4] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(6, 12);
                            if (M2Share.RandomNumber.Random(4) < 3)
                            {
                                nVal = (nUpgrade + 1) * 1000;
                                UserItem.DuraMax = (byte)HUtil32._MIN(65000, UserItem.DuraMax + nVal);
                                UserItem.Dura = (byte)HUtil32._MIN(65000, UserItem.Dura + nVal);
                            }
                            break;
                        case 15:
                            nUpgrade = GetRandomRange(6, 20);
                            if (M2Share.RandomNumber.Random(40) == 0)
                            {
                                UserItem.btValue[0] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(6, 20);
                            if (M2Share.RandomNumber.Random(30) == 0)
                            {
                                UserItem.btValue[1] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(M2Share.g_Config.nHelMetDCAddValueMaxLimit, M2Share.g_Config.nHelMetDCAddValueRate);
                            if (M2Share.RandomNumber.Random(M2Share.g_Config.nHelMetDCAddRate) == 0)
                            {
                                UserItem.btValue[2] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(M2Share.g_Config.nHelMetMCAddValueMaxLimit, M2Share.g_Config.nHelMetMCAddValueRate);
                            if (M2Share.RandomNumber.Random(M2Share.g_Config.nHelMetMCAddRate) == 0)
                            {
                                UserItem.btValue[3] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(M2Share.g_Config.nHelMetSCAddValueMaxLimit, M2Share.g_Config.nHelMetSCAddValueRate);
                            if (M2Share.RandomNumber.Random(M2Share.g_Config.nHelMetSCAddRate) == 0)
                            {
                                UserItem.btValue[4] = (byte)(nUpgrade + 1);
                            }
                            nUpgrade = GetRandomRange(6, 12);
                            if (M2Share.RandomNumber.Random(4) < 3)
                            {
                                nVal = (nUpgrade + 1) * 1000;
                                UserItem.DuraMax = (byte)HUtil32._MIN(65000, UserItem.DuraMax + nVal);
                                UserItem.Dura = (byte)HUtil32._MIN(65000, UserItem.Dura + nVal);
                            }
                            break;
                    }
                    break;
            }
        }

        public void RandomUpgradeUnknownItem(TUserItem UserItem)
        {
            switch (ItemType)
            {
                case grobal2.ITEM_WEAPON:
                    break;
                case grobal2.ITEM_ARMOR:
                    break;
                case grobal2.ITEM_ACCESSORY:
                    int nUpgrade;
                    int nRandPoint;
                    int nVal;
                    switch (StdMode)
                    {
                        case 15:
                            nRandPoint = GetRandomRange(M2Share.g_Config.nUnknowHelMetACAddValueMaxLimit, M2Share.g_Config.nUnknowHelMetACAddRate);
                            if (nRandPoint > 0)
                            {
                                UserItem.btValue[0] = (byte)nRandPoint;
                            }
                            nUpgrade = nRandPoint;
                            nRandPoint = GetRandomRange(M2Share.g_Config.nUnknowHelMetMACAddValueMaxLimit, M2Share.g_Config.nUnknowHelMetMACAddRate);
                            if (nRandPoint > 0)
                            {
                                UserItem.btValue[1] = (byte)nRandPoint;
                            }
                            nUpgrade += nRandPoint;
                            nRandPoint = GetRandomRange(M2Share.g_Config.nUnknowHelMetDCAddValueMaxLimit, M2Share.g_Config.nUnknowHelMetDCAddRate);
                            if (nRandPoint > 0)
                            {
                                UserItem.btValue[2] = (byte)nRandPoint;
                            }
                            nUpgrade += nRandPoint;
                            nRandPoint = GetRandomRange(M2Share.g_Config.nUnknowHelMetMCAddValueMaxLimit, M2Share.g_Config.nUnknowHelMetMCAddRate);
                            if (nRandPoint > 0)
                            {
                                UserItem.btValue[3] = (byte)nRandPoint;
                            }
                            nUpgrade += nRandPoint;
                            nRandPoint = GetRandomRange(M2Share.g_Config.nUnknowHelMetSCAddValueMaxLimit, M2Share.g_Config.nUnknowHelMetSCAddRate);
                            if (nRandPoint > 0)
                            {
                                UserItem.btValue[4] = (byte)nRandPoint;
                            }
                            nUpgrade += nRandPoint;
                            nRandPoint = GetRandomRange(6, 30);
                            if (nRandPoint > 0)
                            {
                                nVal = (nRandPoint + 1) * 1000;
                                UserItem.DuraMax = (byte)HUtil32._MIN(65000, UserItem.DuraMax + nVal);
                                UserItem.Dura = (byte)HUtil32._MIN(65000, UserItem.Dura + nVal);
                            }
                            if (M2Share.RandomNumber.Random(30) == 0)
                            {
                                UserItem.btValue[7] = 1;
                            }
                            UserItem.btValue[8] = 1;
                            if (nUpgrade >= 3)
                            {
                                if (UserItem.btValue[0] >= 5)
                                {
                                    UserItem.btValue[5] = 1;
                                    UserItem.btValue[6] = (byte)(UserItem.btValue[0] * 3 + 25);
                                    return;
                                }
                                if (UserItem.btValue[2] >= 2)
                                {
                                    UserItem.btValue[5] = 1;
                                    UserItem.btValue[6] = (byte)(UserItem.btValue[2] * 4 + 35);
                                    return;
                                }
                                if (UserItem.btValue[3] >= 2)
                                {
                                    UserItem.btValue[5] = 2;
                                    UserItem.btValue[6] = (byte)(UserItem.btValue[3] * 2 + 18);
                                    return;
                                }
                                if (UserItem.btValue[4] >= 2)
                                {
                                    UserItem.btValue[5] = 3;
                                    UserItem.btValue[6] = (byte)(UserItem.btValue[4] * 2 + 18);
                                    return;
                                }
                                UserItem.btValue[6] = (byte)(nUpgrade * 2 + 18);
                            }
                            break;
                        case 22:
                        case 23:
                            nRandPoint = GetRandomRange(M2Share.g_Config.nUnknowRingACAddValueMaxLimit, M2Share.g_Config.nUnknowRingACAddRate);
                            if (nRandPoint > 0)
                            {
                                UserItem.btValue[0] = (byte)nRandPoint;
                            }
                            nUpgrade = nRandPoint;
                            nRandPoint = GetRandomRange(M2Share.g_Config.nUnknowRingMACAddValueMaxLimit, M2Share.g_Config.nUnknowRingMACAddRate);
                            if (nRandPoint > 0)
                            {
                                UserItem.btValue[1] = (byte)nRandPoint;
                            }
                            nUpgrade += nRandPoint; // 以上二项为增加项，增加防，及魔防
                            nRandPoint = GetRandomRange(M2Share.g_Config.nUnknowRingDCAddValueMaxLimit, M2Share.g_Config.nUnknowRingDCAddRate);
                            if (nRandPoint > 0)
                            {
                                UserItem.btValue[2] = (byte)nRandPoint;
                            }
                            nUpgrade += nRandPoint;
                            nRandPoint = GetRandomRange(M2Share.g_Config.nUnknowRingMCAddValueMaxLimit, M2Share.g_Config.nUnknowRingMCAddRate);
                            if (nRandPoint > 0)
                            {
                                UserItem.btValue[3] = (byte)nRandPoint;
                            }
                            nUpgrade += nRandPoint;
                            nRandPoint = GetRandomRange(M2Share.g_Config.nUnknowRingSCAddValueMaxLimit, M2Share.g_Config.nUnknowRingSCAddRate);
                            if (nRandPoint > 0)
                            {
                                UserItem.btValue[4] = (byte)nRandPoint;
                            }
                            nUpgrade += nRandPoint;
                            nRandPoint = GetRandomRange(6, 30);
                            if (nRandPoint > 0)
                            {
                                nVal = (nRandPoint + 1) * 1000;
                                UserItem.DuraMax = (byte)HUtil32._MIN(65000, UserItem.DuraMax + nVal);
                                UserItem.Dura = (byte)HUtil32._MIN(65000, UserItem.Dura + nVal);
                            }
                            if (M2Share.RandomNumber.Random(30) == 0)
                            {
                                UserItem.btValue[7] = 1;
                            }
                            UserItem.btValue[8] = 1;
                            if (nUpgrade >= 3)
                            {
                                if (UserItem.btValue[2] >= 3)
                                {
                                    UserItem.btValue[5] = 1;
                                    UserItem.btValue[6] = (byte)(UserItem.btValue[2] * 3 + 25);
                                    return;
                                }
                                if (UserItem.btValue[3] >= 3)
                                {
                                    UserItem.btValue[5] = 2;
                                    UserItem.btValue[6] = (byte)(UserItem.btValue[3] * 2 + 18);
                                    return;
                                }
                                if (UserItem.btValue[4] >= 3)
                                {
                                    UserItem.btValue[5] = 3;
                                    UserItem.btValue[6] = (byte)(UserItem.btValue[4] * 2 + 18);
                                    return;
                                }
                                UserItem.btValue[6] = (byte)(nUpgrade * 2 + 18);
                            }
                            break;
                        case 24:
                        case 26:
                            nRandPoint = GetRandomRange(M2Share.g_Config.nUnknowNecklaceACAddValueMaxLimit, M2Share.g_Config.nUnknowNecklaceACAddRate);
                            if (nRandPoint > 0)
                            {
                                UserItem.btValue[0] = (byte)nRandPoint;
                            }
                            nUpgrade = nRandPoint;
                            nRandPoint = GetRandomRange(M2Share.g_Config.nUnknowNecklaceMACAddValueMaxLimit, M2Share.g_Config.nUnknowNecklaceMACAddRate);
                            if (nRandPoint > 0)
                            {
                                UserItem.btValue[1] = (byte)nRandPoint;
                            }
                            nUpgrade += nRandPoint;
                            nRandPoint = GetRandomRange(M2Share.g_Config.nUnknowNecklaceDCAddValueMaxLimit, M2Share.g_Config.nUnknowNecklaceDCAddRate);
                            if (nRandPoint > 0)
                            {
                                UserItem.btValue[2] = (byte)nRandPoint;
                            }
                            nUpgrade += nRandPoint;
                            nRandPoint = GetRandomRange(M2Share.g_Config.nUnknowNecklaceMCAddValueMaxLimit, M2Share.g_Config.nUnknowNecklaceMCAddRate);
                            if (nRandPoint > 0)
                            {
                                UserItem.btValue[3] = (byte)nRandPoint;
                            }
                            nUpgrade += nRandPoint;
                            nRandPoint = GetRandomRange(M2Share.g_Config.nUnknowNecklaceSCAddValueMaxLimit, M2Share.g_Config.nUnknowNecklaceSCAddRate);
                            if (nRandPoint > 0)
                            {
                                UserItem.btValue[4] = (byte)nRandPoint;
                            }
                            nUpgrade += nRandPoint;
                            nRandPoint = GetRandomRange(6, 30);
                            if (nRandPoint > 0)
                            {
                                nVal = (nRandPoint + 1) * 1000;
                                UserItem.DuraMax = (byte)HUtil32._MIN(65000, UserItem.DuraMax + nVal);
                                UserItem.Dura = (byte)HUtil32._MIN(65000, UserItem.Dura + nVal);
                            }
                            if (M2Share.RandomNumber.Random(30) == 0)
                            {
                                UserItem.btValue[7] = 1;
                            }
                            UserItem.btValue[8] = 1;
                            if (nUpgrade >= 2)
                            {
                                if (UserItem.btValue[0] >= 3)
                                {
                                    UserItem.btValue[5] = 1;
                                    UserItem.btValue[6] = (byte)(UserItem.btValue[0] * 3 + 25);
                                    return;
                                }
                                if (UserItem.btValue[2] >= 2)
                                {
                                    UserItem.btValue[5] = 1;
                                    UserItem.btValue[6] = (byte)(UserItem.btValue[2] * 3 + 30);
                                    return;
                                }
                                if (UserItem.btValue[3] >= 2)
                                {
                                    UserItem.btValue[5] = 2;
                                    UserItem.btValue[6] = (byte)(UserItem.btValue[3] * 2 + 20);
                                    return;
                                }
                                if (UserItem.btValue[4] >= 2)
                                {
                                    UserItem.btValue[5] = 3;
                                    UserItem.btValue[6] = (byte)(UserItem.btValue[4] * 2 + 20);
                                    return;
                                }
                                UserItem.btValue[6] = (byte)(nUpgrade * 2 + 18);
                            }
                            break;
                    }
                    break;
            }
        }

        public void ApplyItemParameters(ref TAddAbility AddAbility)
        {
            switch(ItemType)
            {
                case grobal2.ITEM_WEAPON:
                    AddAbility.wHitPoint += AC2;
                    if (MAC2 > 10)
                    {
                        AddAbility.nHitSpeed += (ushort)(MAC2 - 10);
                    }
                    else
                    {
                        AddAbility.nHitSpeed -= MAC2;
                    }
                    AddAbility.btLuck += (byte)AC;
                    AddAbility.btUnLuck += (byte)MAC;
                    break;
                case grobal2.ITEM_ARMOR:
                    AddAbility.wAC = HUtil32.MakeLong(HUtil32.LoWord(AddAbility.wAC) + AC, HUtil32.HiWord(AddAbility.wAC) + AC2);
                    AddAbility.wMAC = HUtil32.MakeLong(HUtil32.LoWord(AddAbility.wMAC) + MAC, HUtil32.HiWord(AddAbility.wMAC) + MAC2);
                    AddAbility.btLuck += HUtil32.LoByte(Source);
                    AddAbility.btUnLuck += HUtil32.HiByte(Source);
                    break;
                case grobal2.ITEM_ACCESSORY:
                    switch(StdMode)
                    {
                        case 19:
                            AddAbility.wAntiMagic += AC2;
                            AddAbility.btUnLuck += (byte)MAC;
                            AddAbility.btLuck += (byte)MAC2;
                            break;
                        case 53: // 新加物品属性
                            if (M2Share.g_Config.boAddUserItemNewValue)
                            {
                                AddAbility.wAntiMagic += AC2;
                                AddAbility.btUnLuck += (byte)MAC;
                                AddAbility.btLuck += (byte)MAC2;
                            }
                            else
                            {
                                AddAbility.wAC = HUtil32.MakeLong(HUtil32.LoWord(AddAbility.wAC) + AC, HUtil32.HiWord(AddAbility.wAC) + AC2);
                                AddAbility.wMAC = HUtil32.MakeLong(HUtil32.LoWord(AddAbility.wMAC) + MAC, HUtil32.HiWord(AddAbility.wMAC) + MAC2);
                            }
                            break;
                        case 63:
                            AddAbility.wHP += AC;
                            AddAbility.wMP += AC2;
                            AddAbility.btUnLuck += (byte)MAC;
                            AddAbility.btLuck += (byte)MAC2;
                            break;
                        case 20:
                        case 24:
                            AddAbility.wHitPoint += AC2;
                            AddAbility.wSpeedPoint += MAC2;
                            break;
                        case 52:// 原本与 20,24 一个属性，现在分开单独进行设置
                            if (M2Share.g_Config.boAddUserItemNewValue)
                            {
                                AddAbility.wHitPoint += AC2;
                                AddAbility.wSpeedPoint += MAC2;
                            }
                            else
                            {
                                AddAbility.wAC = HUtil32.MakeLong(HUtil32.LoWord(AddAbility.wAC) + AC, HUtil32.HiWord(AddAbility.wAC) + AC2);
                                AddAbility.wMAC = HUtil32.MakeLong(HUtil32.LoWord(AddAbility.wMAC) + MAC, HUtil32.HiWord(AddAbility.wMAC) + MAC2);
                            }
                            break;
                        case 62:
                            AddAbility.HandWeight += AC2;
                            AddAbility.Weight += MAC;
                            AddAbility.WearWeight += MAC2;
                            break;
                        case 21:
                            AddAbility.wHealthRecover += AC2;
                            AddAbility.wSpellRecover += MAC2;
                            AddAbility.nHitSpeed += AC;
                            AddAbility.nHitSpeed -= MAC;
                            break;
                        case 54:
                            if (M2Share.g_Config.boAddUserItemNewValue)
                            {
                                AddAbility.wHealthRecover += AC2;
                                AddAbility.wSpellRecover += MAC2;
                                AddAbility.nHitSpeed += AC;
                                AddAbility.nHitSpeed -= MAC;
                            }
                            else
                            {
                                AddAbility.wAC = HUtil32.MakeLong(HUtil32.LoWord(AddAbility.wAC) + AC, HUtil32.HiWord(AddAbility.wAC) + AC2);
                                AddAbility.wMAC = HUtil32.MakeLong(HUtil32.LoWord(AddAbility.wMAC) + MAC, HUtil32.HiWord(AddAbility.wMAC) + MAC2);
                            }
                            break;
                        case 64:
                            AddAbility.wHealthRecover += AC2;
                            AddAbility.wSpellRecover += MAC2;
                            AddAbility.nHitSpeed += AC;
                            AddAbility.nHitSpeed -= MAC;
                            break;
                        case 23:
                            AddAbility.wAntiPoison += AC2;
                            AddAbility.wPoisonRecover += MAC2;
                            AddAbility.nHitSpeed += AC;
                            AddAbility.nHitSpeed -= MAC;
                            break;
                    }
                    break;
            }
            AddAbility.wDC = HUtil32.MakeLong(HUtil32.LoWord(AddAbility.wDC) + DC, HUtil32.HiWord(AddAbility.wDC) + DC2);
            AddAbility.wMC = HUtil32.MakeLong(HUtil32.LoWord(AddAbility.wMC) + MC, HUtil32.HiWord(AddAbility.wMC) + MC2);
            AddAbility.wSC = HUtil32.MakeLong(HUtil32.LoWord(AddAbility.wSC) + SC, HUtil32.HiWord(AddAbility.wSC) + SC2);
        }
    }
}

namespace M2Server
{
    public class ItmUnit
    {
        /// <summary>
        /// 获取自定义装备名称
        /// </summary>
        /// <param name="UserItem"></param>
        /// <returns></returns>
        public static string GetItemName(TUserItem UserItem)
        {
            var result = string.Empty;
            if (UserItem.btValue[13] == 1)
            {
                result = M2Share.ItemUnit.GetCustomItemName(UserItem.MakeIndex, UserItem.wIndex);
            }
            if (string.IsNullOrEmpty(result))
            {
                result = M2Share.UserEngine.GetStdItemName(UserItem.wIndex);
            }
            return result;
        }
    }
}

