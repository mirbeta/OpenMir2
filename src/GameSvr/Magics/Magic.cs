using SystemModule;

namespace GameSvr
{
    public static class Magic
    {
        public static int MPow(TUserMagic UserMagic)
        {
            return UserMagic.MagicInfo.wPower + M2Share.RandomNumber.Random(UserMagic.MagicInfo.wMaxPower - UserMagic.MagicInfo.wPower);
        }

        public static int GetPower(int nPower, TUserMagic UserMagic)
        {
            return HUtil32.Round(nPower / (UserMagic.MagicInfo.btTrainLv + 1) * (UserMagic.btLevel + 1)) + UserMagic.MagicInfo.btDefPower + M2Share.RandomNumber.Random(UserMagic.MagicInfo.btDefMaxPower - UserMagic.MagicInfo.btDefPower);
        }

        public static int GetPower13(int nInt, TUserMagic UserMagic)
        {
            double d10 = nInt / 3.0;
            double  d18 = nInt - d10;
            return HUtil32.Round(d18 / (UserMagic.MagicInfo.btTrainLv + 1) * (UserMagic.btLevel + 1) + d10 + (UserMagic.MagicInfo.btDefPower + M2Share.RandomNumber.Random(UserMagic.MagicInfo.btDefMaxPower - UserMagic.MagicInfo.btDefPower)));
        }

        public static ushort GetRPow(int wInt)
        {
            ushort result;
            if (HUtil32.HiWord(wInt) > HUtil32.LoWord(wInt))
            {
                result = (ushort)(M2Share.RandomNumber.Random(HUtil32.HiWord(wInt) - HUtil32.LoWord(wInt) + 1) + HUtil32.LoWord(wInt));
            }
            else
            {
                result = HUtil32.LoWord(wInt);
            }
            return result;
        }

        /// <summary>
        /// 检查护身符
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="nCount"></param>
        /// <param name="nType"></param>
        /// <param name="Idx"></param>
        /// <returns></returns>
        public static bool CheckAmulet(TPlayObject PlayObject, int nCount, int nType, ref short Idx)
        {
            GoodItem amuletStdItem = null;
            var result = false;
            Idx = 0;
            if (PlayObject.m_UseItems[Grobal2.U_ARMRINGL]!=null && PlayObject.m_UseItems[Grobal2.U_ARMRINGL].wIndex > 0)
            {
                amuletStdItem = M2Share.UserEngine.GetStdItem(PlayObject.m_UseItems[Grobal2.U_ARMRINGL].wIndex);
                if (amuletStdItem != null && amuletStdItem.StdMode == 25)
                {
                    switch (nType)
                    {
                        case 1:
                            if (amuletStdItem.Shape == 5 && HUtil32.Round(PlayObject.m_UseItems[Grobal2.U_ARMRINGL].Dura / 100) >= nCount)
                            {
                                Idx = Grobal2.U_ARMRINGL;
                                result = true;
                                return result;
                            }
                            break;
                        case 2:
                            if (amuletStdItem.Shape <= 2 && HUtil32.Round(PlayObject.m_UseItems[Grobal2.U_ARMRINGL].Dura / 100) >= nCount)
                            {
                                Idx = Grobal2.U_ARMRINGL;
                                result = true;
                                return result;
                            }
                            break;
                    }
                }
            }
            if (PlayObject.m_UseItems[Grobal2.U_BUJUK] !=null && PlayObject.m_UseItems[Grobal2.U_BUJUK].wIndex > 0)
            {
                amuletStdItem = M2Share.UserEngine.GetStdItem(PlayObject.m_UseItems[Grobal2.U_BUJUK].wIndex);
                if (amuletStdItem != null && amuletStdItem.StdMode == 25)
                {
                    switch (nType)
                    {
                        case 1:
                            if (amuletStdItem.Shape == 5 && HUtil32.Round(PlayObject.m_UseItems[Grobal2.U_BUJUK].Dura / 100) >= nCount)
                            {
                                Idx = Grobal2.U_BUJUK;
                                result = true;
                                return result;
                            }
                            break;
                        case 2:
                            if (amuletStdItem.Shape <= 2 && HUtil32.Round(PlayObject.m_UseItems[Grobal2.U_BUJUK].Dura / 100) >= nCount)
                            {
                                Idx = Grobal2.U_BUJUK;
                                result = true;
                                return result;
                            }
                            break;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 使用护身符
        /// </summary>
        /// <param name="PlayObject"></param>
        /// <param name="nCount"></param>
        /// <param name="nType"></param>
        /// <param name="Idx"></param>
        public static void UseAmulet(TPlayObject PlayObject, int nCount, int nType, ref short Idx)
        {
            var dura = (ushort)(nCount * 100);
            if (PlayObject.m_UseItems[Idx] != null && PlayObject.m_UseItems[Idx].Dura > dura)
            {
                PlayObject.m_UseItems[Idx].Dura -= dura;//减少护身符持久即数量
                PlayObject.SendMsg(PlayObject, Grobal2.RM_DURACHANGE, Idx, PlayObject.m_UseItems[Idx].Dura, PlayObject.m_UseItems[Idx].DuraMax, 0, "");
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

