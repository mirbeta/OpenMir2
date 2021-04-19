namespace M2Server
{
    public class Magic
    {
        public static int MPow(TUserMagic UserMagic)
        {
            int result;
            result = UserMagic.MagicInfo.wPower + M2Share.RandomNumber.Random(UserMagic.MagicInfo.wMaxPower - UserMagic.MagicInfo.wPower);
            return result;
        }

        public static int GetPower(int nPower, TUserMagic UserMagic)
        {
            int result;
            result = HUtil32.Round(nPower / (UserMagic.MagicInfo.btTrainLv + 1) * (UserMagic.btLevel + 1)) + UserMagic.MagicInfo.btDefPower + M2Share.RandomNumber.Random(UserMagic.MagicInfo.btDefMaxPower - UserMagic.MagicInfo.btDefPower);
            return result;
        }

        public static int GetPower13(int nInt, TUserMagic UserMagic)
        {
            int result;
            double d10;
            double d18;
            d10 = nInt / 3.0;
            d18 = nInt - d10;
            result = HUtil32.Round(d18 / (UserMagic.MagicInfo.btTrainLv + 1) * (UserMagic.btLevel + 1) + d10 + (UserMagic.MagicInfo.btDefPower + M2Share.RandomNumber.Random(UserMagic.MagicInfo.btDefMaxPower - UserMagic.MagicInfo.btDefPower)));
            return result;
        }

        public static short GetRPow(int wInt)
        {
            short result;
            if (HUtil32.HiWord(wInt) > HUtil32.LoWord(wInt))
            {
                result = (short)(M2Share.RandomNumber.Random(HUtil32.HiWord(wInt) - HUtil32.LoWord(wInt) + 1) + HUtil32.LoWord(wInt));
            }
            else
            {
                result = HUtil32.LoWord(wInt);
            }
            return result;
        }

        // nType 为指定类型 1 为护身符 2 为毒药
        public static bool CheckAmulet(TPlayObject PlayObject, int nCount, int nType, ref short Idx)
        {
            bool result;
            TItem AmuletStdItem;
            result = false;
            Idx = 0;
            if (PlayObject.m_UseItems[grobal2.U_ARMRINGL].wIndex > 0)
            {
                AmuletStdItem = M2Share.UserEngine.GetStdItem(PlayObject.m_UseItems[grobal2.U_ARMRINGL].wIndex);
                if ((AmuletStdItem != null) && (AmuletStdItem.StdMode == 25))
                {
                    switch (nType)
                    {
                        case 1:
                            if ((AmuletStdItem.Shape == 5) && (HUtil32.Round(PlayObject.m_UseItems[grobal2.U_ARMRINGL].Dura / 100) >= nCount))
                            {
                                Idx = grobal2.U_ARMRINGL;
                                result = true;
                                return result;
                            }
                            break;
                        case 2:
                            if ((AmuletStdItem.Shape <= 2) && (HUtil32.Round(PlayObject.m_UseItems[grobal2.U_ARMRINGL].Dura / 100) >= nCount))
                            {
                                Idx = grobal2.U_ARMRINGL;
                                result = true;
                                return result;
                            }
                            break;
                    }
                }
            }
            if (PlayObject.m_UseItems[grobal2.U_BUJUK].wIndex > 0)
            {
                AmuletStdItem = M2Share.UserEngine.GetStdItem(PlayObject.m_UseItems[grobal2.U_BUJUK].wIndex);
                if ((AmuletStdItem != null) && (AmuletStdItem.StdMode == 25))
                {
                    switch (nType)
                    {
                        case 1:
                            if ((AmuletStdItem.Shape == 5) && (HUtil32.Round(PlayObject.m_UseItems[grobal2.U_BUJUK].Dura / 100) >= nCount))
                            {
                                Idx = grobal2.U_BUJUK;
                                result = true;
                                return result;
                            }
                            break;
                        case 2:
                            if ((AmuletStdItem.Shape <= 2) && (HUtil32.Round(PlayObject.m_UseItems[grobal2.U_BUJUK].Dura / 100) >= nCount))
                            {
                                Idx = grobal2.U_BUJUK;
                                result = true;
                                return result;
                            }
                            break;
                    }
                }
            }
            return result;
        }

        // nType 为指定类型 1 为护身符 2 为毒药
        public static void UseAmulet(TPlayObject PlayObject, int nCount, int nType, ref short Idx)
        {
            if (PlayObject.m_UseItems[Idx].Dura > nCount * 100)
            {
                PlayObject.m_UseItems[Idx].Dura -= (short)(nCount * 100);
                PlayObject.SendMsg(PlayObject, grobal2.RM_DURACHANGE, Idx, PlayObject.m_UseItems[Idx].Dura, PlayObject.m_UseItems[Idx].DuraMax, 0, "");
            }
            else
            {
                PlayObject.m_UseItems[Idx].Dura = 0;
                PlayObject.SendDelItems(PlayObject.m_UseItems[Idx]);
                PlayObject.m_UseItems[Idx].wIndex = 0;
            }
        }
    }
}

