using GameSvr.Items;
using GameSvr.Player;
using SystemModule.Packets.ClientPackets;

namespace GameSvr.Magic
{
    public static class MagicBase
    {
        public static int MPow(UserMagic UserMagic)
        {
            return UserMagic.Magic.Power + M2Share.RandomNumber.Random(UserMagic.Magic.MaxPower - UserMagic.Magic.Power);
        }

        public static int GetPower(int nPower, UserMagic UserMagic)
        {
            return HUtil32.Round(nPower / (UserMagic.Magic.TrainLv + 1) * (UserMagic.Level + 1)) + UserMagic.Magic.DefPower + M2Share.RandomNumber.Random(UserMagic.Magic.DefMaxPower - UserMagic.Magic.DefPower);
        }

        public static int GetPower13(int nInt, UserMagic UserMagic)
        {
            double d10 = nInt / 3.0;
            double d18 = nInt - d10;
            return HUtil32.Round(d18 / (UserMagic.Magic.TrainLv + 1) * (UserMagic.Level + 1) + d10 + (UserMagic.Magic.DefPower + M2Share.RandomNumber.Random(UserMagic.Magic.DefMaxPower - UserMagic.Magic.DefPower)));
        }

        public static ushort GetRPow(int wInt)
        {
            ushort result;
            if (HUtil32.HiByte(wInt) > HUtil32.LoByte(wInt))
            {
                result = (ushort)(M2Share.RandomNumber.Random(HUtil32.HiByte(wInt) - HUtil32.LoByte(wInt) + 1) + HUtil32.LoByte(wInt));
            }
            else
            {
                result = HUtil32.LoByte(wInt);
            }
            return result;
        }

        /// <summary>
        /// 检查护身符
        /// </summary>
        /// <returns></returns>
        public static bool CheckAmulet(PlayObject PlayObject, int nCount, int nType, ref short Idx)
        {
            if (PlayObject == null)
            {
                return false;
            }

            Idx = 0;
            StdItem amuletStdItem;
            if (PlayObject.UseItems[Grobal2.U_ARMRINGL] != null && PlayObject.UseItems[Grobal2.U_ARMRINGL].Index > 0)
            {
                amuletStdItem = M2Share.WorldEngine.GetStdItem(PlayObject.UseItems[Grobal2.U_ARMRINGL].Index);
                if (amuletStdItem != null && amuletStdItem.StdMode == 25)
                {
                    switch (nType)
                    {
                        case 1:
                            if (amuletStdItem.Shape == 5 && HUtil32.Round(PlayObject.UseItems[Grobal2.U_ARMRINGL].Dura / 100) >= nCount)
                            {
                                Idx = Grobal2.U_ARMRINGL;
                                return true;
                            }
                            break;
                        case 2:
                            if (amuletStdItem.Shape <= 2 && HUtil32.Round(PlayObject.UseItems[Grobal2.U_ARMRINGL].Dura / 100) >= nCount)
                            {
                                Idx = Grobal2.U_ARMRINGL;
                                return true;
                            }
                            break;
                    }
                }
            }
            if (PlayObject.UseItems[Grobal2.U_BUJUK] != null && PlayObject.UseItems[Grobal2.U_BUJUK].Index > 0)
            {
                amuletStdItem = M2Share.WorldEngine.GetStdItem(PlayObject.UseItems[Grobal2.U_BUJUK].Index);
                if (amuletStdItem != null && amuletStdItem.StdMode == 25)
                {
                    switch (nType)
                    {
                        case 1:
                            if (amuletStdItem.Shape == 5 && HUtil32.Round(PlayObject.UseItems[Grobal2.U_BUJUK].Dura / 100) >= nCount)
                            {
                                Idx = Grobal2.U_BUJUK;
                                return true;
                            }
                            break;
                        case 2:
                            if (amuletStdItem.Shape <= 2 && HUtil32.Round(PlayObject.UseItems[Grobal2.U_BUJUK].Dura / 100) >= nCount)
                            {
                                Idx = Grobal2.U_BUJUK;
                                return true;
                            }
                            break;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 使用护身符
        /// </summary>
        public static void UseAmulet(PlayObject PlayObject, int nCount, int nType, ref short Idx)
        {
            if (PlayObject == null)
            {
                return;
            }
            ushort dura = (ushort)(nCount * 100);
            if (PlayObject.UseItems[Idx] != null && PlayObject.UseItems[Idx].Dura > dura)
            {
                PlayObject.UseItems[Idx].Dura -= dura;//减少护身符持久即数量
                PlayObject.SendMsg(PlayObject, Grobal2.RM_DURACHANGE, Idx, PlayObject.UseItems[Idx].Dura, PlayObject.UseItems[Idx].DuraMax, 0, "");
            }
            else
            {
                PlayObject.UseItems[Idx].Dura = 0;
                PlayObject.SendDelItems(PlayObject.UseItems[Idx]);
                PlayObject.UseItems[Idx].Index = 0;
            }
        }
    }
}

