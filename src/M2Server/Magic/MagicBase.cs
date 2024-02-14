using M2Server.Player;
using OpenMir2;
using OpenMir2.Data;
using OpenMir2.Enums;
using OpenMir2.Packets.ClientPackets;
using SystemModule;

namespace M2Server.Magic
{
    public static class MagicBase
    {
        public static int MPow(UserMagic UserMagic)
        {
            return UserMagic.Magic.Power + M2Share.RandomNumber.Random(UserMagic.Magic.MaxPower - UserMagic.Magic.Power);
        }

        public static int GetPower(int nPower, UserMagic UserMagic)
        {
            return HUtil32.Round(nPower / (double)(UserMagic.Magic.TrainLv + 1) * (UserMagic.Level + 1)) + UserMagic.Magic.DefPower + M2Share.RandomNumber.Random(UserMagic.Magic.DefMaxPower - UserMagic.Magic.DefPower);
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
        public static bool CheckAmulet(PlayObject playObject, byte nCount, byte nType, ref short Idx)
        {
            if (playObject == null)
            {
                return false;
            }
            Idx = 0;
            StdItem amuletStdItem;
            if (playObject.UseItems[ItemLocation.ArmRingl] != null && playObject.UseItems[ItemLocation.ArmRingl].Index > 0)
            {
                amuletStdItem = SystemShare.ItemSystem.GetStdItem(playObject.UseItems[ItemLocation.ArmRingl].Index);
                if (amuletStdItem != null && amuletStdItem.StdMode == 25)
                {
                    switch (nType)
                    {
                        case 1:
                            if (amuletStdItem.Shape == 5 && HUtil32.Round(playObject.UseItems[ItemLocation.ArmRingl].Dura / 100.0) >= nCount)
                            {
                                Idx = ItemLocation.ArmRingl;
                                return true;
                            }
                            break;
                        case 2:
                            if (amuletStdItem.Shape <= 2 && HUtil32.Round(playObject.UseItems[ItemLocation.ArmRingl].Dura / 100.0) >= nCount)
                            {
                                Idx = ItemLocation.ArmRingl;
                                return true;
                            }
                            break;
                    }
                }
            }
            if (playObject.UseItems[ItemLocation.Bujuk] != null && playObject.UseItems[ItemLocation.Bujuk].Index > 0)
            {
                amuletStdItem = SystemShare.ItemSystem.GetStdItem(playObject.UseItems[ItemLocation.Bujuk].Index);
                if (amuletStdItem != null && amuletStdItem.StdMode == 25)
                {
                    switch (nType)
                    {
                        case 1:
                            if (amuletStdItem.Shape == 5 && HUtil32.Round(playObject.UseItems[ItemLocation.Bujuk].Dura / 100.0) >= nCount)
                            {
                                Idx = ItemLocation.Bujuk;
                                return true;
                            }
                            break;
                        case 2:
                            if (amuletStdItem.Shape <= 2 && HUtil32.Round(playObject.UseItems[ItemLocation.Bujuk].Dura / 100.0) >= nCount)
                            {
                                Idx = ItemLocation.Bujuk;
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
        public static void UseAmulet(PlayObject playObject, byte nCount, byte nType, short idx)
        {
            if (playObject == null)
            {
                return;
            }
            ushort duration = (ushort)(nCount * 100);
            if (playObject.UseItems[idx] != null && playObject.UseItems[idx].Dura > duration)
            {
                playObject.UseItems[idx].Dura -= duration;//减少护身符持久即数量
                playObject.SendMsg(playObject, Messages.RM_DURACHANGE, idx, playObject.UseItems[idx].Dura, playObject.UseItems[idx].DuraMax, 0);
            }
            else
            {
                playObject.UseItems[idx].Dura = 0;
                playObject.SendDelItems(playObject.UseItems[idx]);
                playObject.UseItems[idx].Index = 0;
            }
        }
    }
}

