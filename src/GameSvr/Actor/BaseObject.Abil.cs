using GameSvr.Items;
using GameSvr.Player;
using SystemModule;
using SystemModule.Data;
using SystemModule.Packet.ClientPackets;

namespace GameSvr.Actor
{
    public partial class BaseObject
    {
        /// <summary>
        /// 计算自身属性
        /// </summary>
        public virtual void RecalcAbilitysOld()
        {
            bool[] boRecallSuite = new bool[4] { false, false, false, false };
            bool[] boMoXieSuite = new bool[3] { false, false, false };
            bool[] boSpirit = new bool[4] { false, false, false, false };
            AddAbil = new AddAbility();
            ushort wOldHP = Abil.HP;
            ushort wOldMP = Abil.MP;
            WAbil = Abil;
            WAbil.HP = wOldHP;
            WAbil.MP = wOldMP;
            WAbil.Weight = 0;
            WAbil.WearWeight = 0;
            WAbil.HandWeight = 0;
            AntiPoison = 0;
            PoisonRecover = 0;
            HealthRecover = 0;
            SpellRecover = 0;
            AntiMagic = 1;
            Luck = 0;
            HitSpeed = 0;
            BoExpItem = false;
            ExpItem = 0;
            BoPowerItem = false;
            PowerItem = 0;
            HideMode = false;
            Teleport = false;
            Paralysis = false;
            Revival = false;
            UnRevival = false;
            FlameRing = false;
            RecoveryRing = false;
            AngryRing = false;
            MagicShield = false;
            UnMagicShield = false;
            MuscleRing = false;
            FastTrain = false;
            ProbeNecklace = false;
            SuperManItem = false;
            GuildMove = false;
            UnParalysis = false;
            BoExpItem = false;
            BoPowerItem = false;
            NoDropItem = false;
            NoDropUseItem = false;
            MBopirit = false;
            HorseType = 0;
            DressEffType = 0;
            AutoAddHpmpMode = 0;
            // 气血石
            MoXieSuite = 0;
            SuckupEnemyHealth = 0;
            SuckupEnemyHealthRate = 0;
            bool boHongMoSuite1 = false;
            bool boHongMoSuite2 = false;
            bool boHongMoSuite3 = false;
            RecallSuite = false;
            SmashSet = false;
            bool boSmash1 = false;
            bool boSmash2 = false;
            bool boSmash3 = false;
            HwanDevilSet = false;
            bool boHwanDevil1 = false;
            bool boHwanDevil2 = false;
            bool boHwanDevil3 = false;
            PuritySet = false;
            bool boPurity1 = false;
            bool boPurity2 = false;
            bool boPurity3 = false;
            MundaneSet = false;
            bool boMundane1 = false;
            bool boMundane2 = false;
            NokChiSet = false;
            bool boNokChi1 = false;
            bool boNokChi2 = false;
            TaoBuSet = false;
            bool boTaoBu1 = false;
            bool boTaoBu2 = false;
            FiveStringSet = false;
            bool boFiveString1 = false;
            bool boFiveString2 = false;
            bool boFiveString3 = false;
            bool boOldHideMode = HideMode;
            PkDieLostExp = 0;
            PkDieLostLevel = 0;
            if (Race == Grobal2.RC_PLAYOBJECT || Race == Grobal2.RC_PLAYCLONE)
            {
                for (var i = 0; i < UseItems.Length; i++)
                {
                    if (UseItems[i] == null || (UseItems[i].Index <= 0) || (UseItems[i].Dura <= 0))
                    {
                        continue;
                    }
                    var stdItem = M2Share.WorldEngine.GetStdItem(UseItems[i].Index);
                    if (stdItem == null)
                    {
                        continue;
                    }
                    ApplyItemParameters(UseItems[i], stdItem, ref AddAbil);
                    ApplyItemParametersEx(UseItems[i], ref WAbil);
                    if ((i == Grobal2.U_WEAPON) || (i == Grobal2.U_RIGHTHAND) || (i == Grobal2.U_DRESS))
                    {
                        if (i == Grobal2.U_DRESS)
                        {
                            Abil.WearWeight += stdItem.Weight;
                        }
                        else
                        {
                            Abil.HandWeight += stdItem.Weight;
                        }
                        switch (stdItem.AniCount)
                        {
                            case 120:
                                FastTrain = true;
                                break;
                            case 121:
                                ProbeNecklace = true;
                                break;
                            case 145:
                                GuildMove = true;
                                break;
                            case 111:
                                StatusArr[Grobal2.STATE_TRANSPARENT] = 6 * 10 * 1000;
                                HideMode = true;
                                break;
                            case 112:
                                Teleport = true;
                                break;
                            case 113:
                                Paralysis = true;
                                break;
                            case 114:
                                Revival = true;
                                break;
                            case 115:
                                FlameRing = true;
                                break;
                            case 116:
                                RecoveryRing = true;
                                break;
                            case 117:
                                AngryRing = true;
                                break;
                            case 118:
                                MagicShield = true;
                                break;
                            case 119:
                                MuscleRing = true;
                                break;
                            case 135:
                                boMoXieSuite[0] = true;
                                MoXieSuite += stdItem.Weight / 10;
                                break;
                            case 138:
                                SuckupEnemyHealthRate += stdItem.Weight;
                                break;
                            case 139:
                                UnParalysis = true;
                                break;
                            case 140:
                                SuperManItem = true;
                                break;
                            case 141:
                                BoExpItem = true;
                                ExpItem = ExpItem + (UseItems[i].Dura / M2Share.Config.ItemExpRate);
                                break;
                            case 142:
                                BoPowerItem = true;
                                PowerItem = PowerItem + (UseItems[i].Dura / M2Share.Config.ItemPowerRate);
                                break;
                            case 182:
                                BoExpItem = true;
                                ExpItem = ExpItem + (UseItems[i].DuraMax / M2Share.Config.ItemExpRate);
                                break;
                            case 183:
                                BoPowerItem = true;
                                PowerItem = PowerItem + (UseItems[i].DuraMax / M2Share.Config.ItemPowerRate);
                                break;
                            case 143:
                                UnMagicShield = true;
                                break;
                            case 144:
                                UnRevival = true;
                                break;
                            case 170:
                                AngryRing = true;
                                break;
                            case 171:
                                NoDropItem = true;
                                break;
                            case 172:
                                NoDropUseItem = true;
                                break;
                            case 150:// 麻痹护身
                                Paralysis = true;
                                MagicShield = true;
                                break;
                            case 151:// 麻痹火球
                                Paralysis = true;
                                FlameRing = true;
                                break;
                            case 152:// 麻痹防御
                                Paralysis = true;
                                RecoveryRing = true;
                                break;
                            case 153:// 麻痹负载
                                Paralysis = true;
                                MuscleRing = true;
                                break;
                        }
                        if (stdItem.Shape == 154)
                        {
                            // 护身火球
                            MagicShield = true;
                            FlameRing = true;
                        }
                        switch (stdItem.AniCount)
                        {
                            case 155:// 护身防御
                                MagicShield = true;
                                RecoveryRing = true;
                                break;
                            case 156:// 护身负载
                                MagicShield = true;
                                MuscleRing = true;
                                break;
                            case 157:// 传送麻痹
                                Teleport = true;
                                Paralysis = true;
                                break;
                            case 158:// 传送护身
                                Teleport = true;
                                MagicShield = true;
                                break;
                            case 159:// 传送探测
                                Teleport = true;
                                ProbeNecklace = true;
                                break;
                            case 160:// 传送复活
                                Teleport = true;
                                Revival = true;
                                break;
                            case 161:// 麻痹复活
                                Paralysis = true;
                                Revival = true;
                                break;
                            case 162:// 护身复活
                                MagicShield = true;
                                Revival = true;
                                break;
                            case 180:// PK 死亡掉经验
                                PkDieLostExp = stdItem.DuraMax * M2Share.Config.dwPKDieLostExpRate;
                                break;
                            case 181:// PK 死亡掉等级
                                PkDieLostLevel = stdItem.DuraMax / M2Share.Config.nPKDieLostLevelRate;
                                break;
                        }
                    }
                    else
                    {
                        Abil.WearWeight += stdItem.Weight;
                    }
                    Abil.Weight += stdItem.Weight;
                    switch (i)
                    {
                        case Grobal2.U_WEAPON:
                            {
                                if ((stdItem.SpecialPwr - 1 - 10) < 0)
                                {
                                    AddAbil.WeaponStrong = (byte)stdItem.SpecialPwr;// 强度+
                                }
                                if ((stdItem.SpecialPwr <= -1) && (stdItem.SpecialPwr >= -50))  // -1 to -50
                                {
                                    AddAbil.UndeadPower = (byte)(AddAbil.UndeadPower + -stdItem.SpecialPwr);// Holy+
                                }
                                if ((stdItem.SpecialPwr <= -51) && (stdItem.SpecialPwr >= -100))// -51 to -100
                                {
                                    AddAbil.UndeadPower = (byte)(AddAbil.UndeadPower + (stdItem.SpecialPwr + 50));// Holy-
                                }
                                continue;
                            }
                        case Grobal2.U_RIGHTHAND:
                            {
                                if (stdItem.Shape >= 1 && stdItem.Shape <= 50)
                                {
                                    DressEffType = stdItem.Shape;
                                }
                                if (stdItem.Shape >= 51 && stdItem.Shape <= 100)
                                {
                                    HorseType = (byte)(stdItem.Shape - 50);
                                }
                                continue;
                            }
                        case Grobal2.U_DRESS:
                            {
                                if (UseItems[i].Desc[5] > 0)
                                {
                                    DressEffType = UseItems[i].Desc[5];
                                }
                                if (stdItem.AniCount > 0)
                                {
                                    DressEffType = stdItem.AniCount;
                                }
                                if (stdItem.Light > 0)
                                {
                                    Light = 3;
                                }
                                continue;
                            }
                    }
                    switch (stdItem.Shape)
                    {
                        case 139:
                            UnParalysis = true;
                            break;
                        case 140:
                            SuperManItem = true;
                            break;
                        case 141:
                            BoExpItem = true;
                            ExpItem = ExpItem + (UseItems[i].Dura / M2Share.Config.ItemExpRate);
                            break;
                        case 142:
                            BoPowerItem = true;
                            PowerItem = PowerItem + (UseItems[i].Dura / M2Share.Config.ItemPowerRate);
                            break;
                        case 182:
                            BoExpItem = true;
                            ExpItem = ExpItem + (UseItems[i].DuraMax / M2Share.Config.ItemExpRate);
                            break;
                        case 183:
                            BoPowerItem = true;
                            PowerItem = PowerItem + (UseItems[i].DuraMax / M2Share.Config.ItemPowerRate);
                            break;
                        case 143:
                            UnMagicShield = true;
                            break;
                        case 144:
                            UnRevival = true;
                            break;
                        case 170:
                            AngryRing = true;
                            break;
                        case 171:
                            NoDropItem = true;
                            break;
                        case 172:
                            NoDropUseItem = true;
                            break;
                        case 150:// 麻痹护身
                            Paralysis = true;
                            MagicShield = true;
                            break;
                        case 151:// 麻痹火球
                            Paralysis = true;
                            FlameRing = true;
                            break;
                        case 152:// 麻痹防御
                            Paralysis = true;
                            RecoveryRing = true;
                            break;
                        case 153:// 麻痹负载
                            Paralysis = true;
                            MuscleRing = true;
                            break;
                        case 154:// 护身火球
                            MagicShield = true;
                            FlameRing = true;
                            break;
                        case 155:// 护身防御
                            MagicShield = true;
                            RecoveryRing = true;
                            break;
                        case 156:// 护身负载
                            MagicShield = true;
                            MuscleRing = true;
                            break;
                        case 157:// 传送麻痹
                            Teleport = true;
                            Paralysis = true;
                            break;
                        case 158:// 传送护身
                            Teleport = true;
                            MagicShield = true;
                            break;
                        case 159:// 传送探测
                            Teleport = true;
                            ProbeNecklace = true;
                            break;
                        case 160:// 传送复活
                            Teleport = true;
                            Revival = true;
                            break;
                        case 161:// 麻痹复活
                            Paralysis = true;
                            Revival = true;
                            break;
                        case 162:// 护身复活
                            MagicShield = true;
                            Revival = true;
                            break;
                        case 180:// PK 死亡掉经验
                            PkDieLostExp = stdItem.DuraMax * M2Share.Config.dwPKDieLostExpRate;
                            break;
                        case 181:// PK 死亡掉等级
                            PkDieLostLevel = stdItem.DuraMax / M2Share.Config.nPKDieLostLevelRate;
                            break;
                        case 120:
                            FastTrain = true;
                            break;
                        case 121:
                            ProbeNecklace = true;
                            break;
                        case 123:
                            boRecallSuite[0] = true;
                            break;
                        case 145:
                            GuildMove = true;
                            break;
                        case 127:
                            boSpirit[0] = true;
                            break;
                        case 135:
                            boMoXieSuite[0] = true;
                            MoXieSuite += stdItem.AniCount;
                            break;
                        case 138:
                            boHongMoSuite1 = true;
                            SuckupEnemyHealthRate += stdItem.AniCount;
                            break;
                        case 200:
                            boSmash1 = true;
                            break;
                        case 203:
                            boHwanDevil1 = true;
                            break;
                        case 206:
                            boPurity1 = true;
                            break;
                        case 216:
                            boFiveString1 = true;
                            break;
                        case 111:
                            StatusArr[Grobal2.STATE_TRANSPARENT] = 6 * 10 * 1000;
                            HideMode = true;
                            break;
                        case 112:
                            Teleport = true;
                            break;
                        case 113:
                            Paralysis = true;
                            break;
                        case 114:
                            Revival = true;
                            break;
                        case 115:
                            FlameRing = true;
                            break;
                        case 116:
                            RecoveryRing = true;
                            break;
                        case 117:
                            AngryRing = true;
                            break;
                        case 118:
                            MagicShield = true;
                            break;
                        case 119:
                            MuscleRing = true;
                            break;
                        case 122:
                            boRecallSuite[1] = true;
                            break;
                        case 128:
                            boSpirit[1] = true;
                            break;
                        case 133:
                            boMoXieSuite[1] = true;
                            MoXieSuite += stdItem.AniCount;
                            break;
                        case 136:
                            boHongMoSuite2 = true;
                            SuckupEnemyHealthRate += stdItem.AniCount;
                            break;
                        case 201:
                            boSmash2 = true;
                            break;
                        case 204:
                            boHwanDevil2 = true;
                            break;
                        case 207:
                            boPurity2 = true;
                            break;
                        case 210:
                            boMundane1 = true;
                            break;
                        case 212:
                            boNokChi1 = true;
                            break;
                        case 214:
                            boTaoBu1 = true;
                            break;
                        case 217:
                            boFiveString2 = true;
                            break;
                    }
                    if ((stdItem.SpecialPwr <= -1) && (stdItem.SpecialPwr >= -50))
                    {
                        // -1 to -50
                        AddAbil.UndeadPower = (byte)(AddAbil.UndeadPower + -stdItem.SpecialPwr);
                        // Holy+
                    }
                    if ((stdItem.SpecialPwr <= -51) && (stdItem.SpecialPwr >= -100))
                    {
                        // -51 to -100
                        AddAbil.UndeadPower = (byte)(AddAbil.UndeadPower + (stdItem.SpecialPwr + 50));
                        // Holy-
                    }
                    switch (stdItem.Shape)
                    {
                        case 124:
                            boRecallSuite[2] = true;
                            break;
                        case 126:
                            boSpirit[2] = true;
                            break;
                        case 145:
                            GuildMove = true;
                            break;
                        case 134:
                            boMoXieSuite[2] = true;
                            MoXieSuite += stdItem.AniCount;
                            break;
                        case 137:
                            boHongMoSuite3 = true;
                            SuckupEnemyHealthRate += stdItem.AniCount;
                            break;
                        case 202:
                            boSmash3 = true;
                            break;
                        case 205:
                            boHwanDevil3 = true;
                            break;
                        case 208:
                            boPurity3 = true;
                            break;
                        case 211:
                            boMundane2 = true;
                            break;
                        case 213:
                            boNokChi2 = true;
                            break;
                        case 215:
                            boTaoBu2 = true;
                            break;
                        case 218:
                            boFiveString3 = true;
                            break;
                        case 125:
                            boRecallSuite[3] = true;
                            break;
                        case 129:
                            boSpirit[3] = true;
                            break;
                    }
                }
                if (boRecallSuite[0] && boRecallSuite[1] && boRecallSuite[2] && boRecallSuite[3])
                {
                    RecallSuite = true;
                }
                if (boMoXieSuite[0] && boMoXieSuite[1] && boMoXieSuite[2])
                {
                    MoXieSuite += 50;
                }
                if (boHongMoSuite1 && boHongMoSuite2 && boHongMoSuite3)
                {
                    AddAbil.HIT += 2;
                }
                if (boSpirit[0] && boSpirit[1] && boSpirit[2] && boSpirit[3])
                {
                    MBopirit = true;
                }
                if (boSmash1 && boSmash2 && boSmash3)
                {
                    SmashSet = true;
                }
                if (boHwanDevil1 && boHwanDevil2 && boHwanDevil3)
                {
                    HwanDevilSet = true;
                }
                if (boPurity1 && boPurity2 && boPurity3)
                {
                    PuritySet = true;
                }
                if (boMundane1 && boMundane2)
                {
                    MundaneSet = true;
                }
                if (boNokChi1 && boNokChi2)
                {
                    NokChiSet = true;
                }
                if (boTaoBu1 && boTaoBu2)
                {
                    TaoBuSet = true;
                }
                if (boFiveString1 && boFiveString2 && boFiveString3)
                {
                    FiveStringSet = true;
                }
                Abil.Weight = RecalcBagWeight();
            }
            if (Transparent && (StatusArr[Grobal2.STATE_TRANSPARENT] > 0))
            {
                HideMode = true;
            }
            if (HideMode)
            {
                if (!boOldHideMode)
                {
                    CharStatus = GetCharStatus();
                    StatusChanged();
                }
            }
            else
            {
                if (boOldHideMode)
                {
                    StatusArr[Grobal2.STATE_TRANSPARENT] = 0;
                    CharStatus = GetCharStatus();
                    StatusChanged();
                }
            }
            if (Race == Grobal2.RC_PLAYOBJECT)
            {
                RecalcHitSpeed();//增加此行，只有类型为人物的角色才重新计算攻击敏捷
            }
            int nOldLight = Light;
            if ((UseItems[Grobal2.U_RIGHTHAND] != null) && (UseItems[Grobal2.U_RIGHTHAND].Index > 0) && (UseItems[Grobal2.U_RIGHTHAND].Dura > 0))
            {
                Light = 3;
            }
            else
            {
                Light = 0;
            }
            if (nOldLight != Light)
            {
                SendRefMsg(Grobal2.RM_CHANGELIGHT, 0, 0, 0, 0, "");
            }
            SpeedPoint += (byte)AddAbil.SPEED;
            HitPoint += (byte)AddAbil.HIT;
            AntiPoison += (byte)AddAbil.AntiPoison;
            PoisonRecover += AddAbil.PoisonRecover;
            HealthRecover += AddAbil.HealthRecover;
            SpellRecover += AddAbil.SpellRecover;
            AntiMagic += AddAbil.AntiMagic;
            Luck += AddAbil.Luck;
            Luck -= AddAbil.UnLuck;
            HitSpeed = AddAbil.HitSpeed;
            //Abil.MaxWeight += AddAbil.Weight;
            //Abil.MaxWearWeight += (byte)AddAbil.WearWeight;
            //Abil.MaxHandWeight += (byte)AddAbil.HandWeight;
            Abil.MaxHP = (ushort)HUtil32._MIN(ushort.MaxValue, Abil.MaxHP + AddAbil.HP);
            Abil.MaxMP = (ushort)HUtil32._MIN(ushort.MaxValue, Abil.MaxMP + AddAbil.MP);
            Abil.AC = HUtil32.MakeLong(HUtil32.LoWord(AddAbil.AC) + HUtil32.LoWord(Abil.AC), HUtil32.HiWord(AddAbil.AC) + HUtil32.HiWord(Abil.AC));
            Abil.MAC = HUtil32.MakeLong(HUtil32.LoWord(AddAbil.MAC) + HUtil32.LoWord(Abil.MAC), HUtil32.HiWord(AddAbil.MAC) + HUtil32.HiWord(Abil.MAC));
            Abil.DC = HUtil32.MakeLong(HUtil32.LoWord(AddAbil.DC) + HUtil32.LoWord(Abil.DC), HUtil32.HiWord(AddAbil.DC) + HUtil32.HiWord(Abil.DC));
            Abil.MC = HUtil32.MakeLong(HUtil32.LoWord(AddAbil.MC) + HUtil32.LoWord(Abil.MC), HUtil32.HiWord(AddAbil.MC) + HUtil32.HiWord(Abil.MC));
            Abil.SC = HUtil32.MakeLong(HUtil32.LoWord(AddAbil.SC) + HUtil32.LoWord(Abil.SC), HUtil32.HiWord(AddAbil.SC) + HUtil32.HiWord(Abil.SC));
            if (StatusArr[Grobal2.STATE_DEFENCEUP] > 0)
            {
                Abil.AC = (ushort)HUtil32.MakeLong(HUtil32.LoWord(Abil.AC), HUtil32.HiWord(Abil.AC) + 2 + (Abil.Level / 7));
            }
            if (StatusArr[Grobal2.STATE_MAGDEFENCEUP] > 0)
            {
                Abil.MAC = (ushort)HUtil32.MakeLong(HUtil32.LoWord(Abil.MAC), HUtil32.HiWord(Abil.MAC) + 2 + (Abil.Level / 7));
            }
            if (ExtraAbil[0] > 0)
            {
                Abil.DC = (ushort)HUtil32.MakeLong(HUtil32.LoWord(Abil.DC), HUtil32.HiWord(Abil.DC) + 2 + ExtraAbil[0]);
            }
            if (ExtraAbil[1] > 0)
            {
                Abil.MC = (ushort)HUtil32.MakeLong(HUtil32.LoWord(Abil.MC), HUtil32.HiWord(Abil.MC) + 2 + ExtraAbil[1]);
            }
            if (ExtraAbil[2] > 0)
            {
                Abil.SC = (ushort)HUtil32.MakeLong(HUtil32.LoWord(Abil.SC), HUtil32.HiWord(Abil.SC) + 2 + ExtraAbil[2]);
            }
            if (ExtraAbil[3] > 0)
            {
                HitSpeed += ExtraAbil[3];
            }
            if (ExtraAbil[4] > 0)
            {
                Abil.MaxHP = (ushort)HUtil32._MIN(ushort.MaxValue, Abil.MaxHP + ExtraAbil[4]);
            }
            if (ExtraAbil[5] > 0)
            {
                Abil.MaxMP = (ushort)HUtil32._MIN(ushort.MaxValue, Abil.MaxMP + ExtraAbil[5]);
            }
            if (FlameRing)
            {
                AddItemSkill(1);
            }
            else
            {
                DelItemSkill(1);
            }
            if (RecoveryRing)
            {
                AddItemSkill(2);
            }
            else
            {
                DelItemSkill(2);
            }
            if (MuscleRing)
            {
                Abil.MaxWeight += Abil.MaxWeight;
                Abil.MaxWearWeight += Abil.MaxWearWeight;
                Abil.MaxHandWeight += Abil.MaxHandWeight;
            }
            if (MoXieSuite > 0)
            {
                // 魔血
                if (Abil.MaxMP <= MoXieSuite)
                {
                    MoXieSuite = Abil.MaxMP - 1;
                }
                Abil.MaxMP -= (ushort)MoXieSuite;
                Abil.MaxHP = (ushort)HUtil32._MIN(ushort.MaxValue, Abil.MaxHP + MoXieSuite);
            }
            if (MBopirit)
            {
                // Bonus DC Min +2,DC Max +5,A.Speed + 2
                Abil.DC = HUtil32.MakeLong(HUtil32.LoWord(Abil.DC) + 2, HUtil32.HiWord(Abil.DC) + 2 + 5);
                HitSpeed += 2;
            }
            if (SmashSet)
            {
                // Attack Speed +1, DC1-3
                Abil.DC = HUtil32.MakeLong(HUtil32.LoWord(Abil.DC) + 1, HUtil32.HiWord(Abil.DC) + 2 + 3);
                HitSpeed++;
            }
            if (HwanDevilSet)
            {
                // Hand Carrying Weight Increase +5, Bag Weight Limit Increase +20, +MC 1-2
                Abil.MaxHandWeight += 5;
                Abil.MaxWeight += 20;
                Abil.MC = HUtil32.MakeLong(HUtil32.LoWord(Abil.MC) + 1, HUtil32.HiWord(Abil.MC) + 2 + 2);
            }
            if (PuritySet)
            {
                // Holy +3, Sc 1-2
                AddAbil.UndeadPower = (byte)(AddAbil.UndeadPower + -3);
                Abil.SC = HUtil32.MakeLong(HUtil32.LoWord(Abil.SC) + 1, HUtil32.HiWord(Abil.SC) + 2 + 2);
            }
            if (MundaneSet)
            {
                // Bonus of Hp+50
                Abil.MaxHP = (ushort)HUtil32._MIN(ushort.MaxValue, Abil.MaxHP + 50);
            }
            if (NokChiSet)
            {
                // Bonus of Mp+50
                Abil.MaxMP = (ushort)HUtil32._MIN(ushort.MaxValue, Abil.MaxMP + 50);
            }
            if (TaoBuSet)
            {
                // Bonus of Hp+30, Mp+30
                Abil.MaxHP = (ushort)HUtil32._MIN(ushort.MaxValue, Abil.MaxHP + 30);
                Abil.MaxMP = (ushort)HUtil32._MIN(ushort.MaxValue, Abil.MaxMP + 30);
            }
            if (FiveStringSet)
            {
                // Bonus of Hp +30%, Ac+2
                Abil.MaxHP = (ushort)HUtil32._MIN(ushort.MaxValue, Abil.MaxHP / 100 * 30);
                HitPoint += 2;
            }
            if (Race == Grobal2.RC_PLAYOBJECT)
            {
                SendUpdateMsg(this, Grobal2.RM_CHARSTATUSCHANGED, HitSpeed, CharStatus, 0, 0, "");
            }
            if (Race >= Grobal2.RC_ANIMAL)
            {
                MonsterRecalcAbilitys();
            }
            // 限制最高属性
            Abil.AC = HUtil32.MakeLong(HUtil32._MIN(M2Share.MAXHUMPOWER, HUtil32.LoWord(Abil.AC)), HUtil32._MIN(M2Share.MAXHUMPOWER, HUtil32.HiWord(Abil.AC)));
            Abil.MAC = HUtil32.MakeLong(HUtil32._MIN(M2Share.MAXHUMPOWER, HUtil32.LoWord(Abil.MAC)), HUtil32._MIN(M2Share.MAXHUMPOWER, HUtil32.HiWord(Abil.MAC)));
            Abil.DC = HUtil32.MakeLong(HUtil32._MIN(M2Share.MAXHUMPOWER, HUtil32.LoWord(Abil.DC)), HUtil32._MIN(M2Share.MAXHUMPOWER, HUtil32.HiWord(Abil.DC)));
            Abil.MC = HUtil32.MakeLong(HUtil32._MIN(M2Share.MAXHUMPOWER, HUtil32.LoWord(Abil.MC)), HUtil32._MIN(M2Share.MAXHUMPOWER, HUtil32.HiWord(Abil.MC)));
            Abil.SC = HUtil32.MakeLong(HUtil32._MIN(M2Share.MAXHUMPOWER, HUtil32.LoWord(Abil.SC)), HUtil32._MIN(M2Share.MAXHUMPOWER, HUtil32.HiWord(Abil.SC)));
            if (M2Share.Config.HungerSystem && M2Share.Config.HungerDecPower)
            {
                if (HUtil32.RangeInDefined(HungerStatus, 0, 999))
                {
                    Abil.DC = HUtil32.MakeLong(HUtil32.Round(HUtil32.LoWord(Abil.DC) * 0.2), HUtil32.Round(HUtil32.HiWord(Abil.DC) * 0.2));
                    Abil.MC = HUtil32.MakeLong(HUtil32.Round(HUtil32.LoWord(Abil.MC) * 0.2), HUtil32.Round(HUtil32.HiWord(Abil.MC) * 0.2));
                    Abil.SC = HUtil32.MakeLong(HUtil32.Round(HUtil32.LoWord(Abil.SC) * 0.2), HUtil32.Round(HUtil32.HiWord(Abil.SC) * 0.2));
                }
                else if (HUtil32.RangeInDefined(HungerStatus, 1000, 1999))
                {
                    Abil.DC = HUtil32.MakeLong(HUtil32.Round(HUtil32.LoWord(Abil.DC) * 0.4), HUtil32.Round(HUtil32.HiWord(Abil.DC) * 0.4));
                    Abil.MC = HUtil32.MakeLong(HUtil32.Round(HUtil32.LoWord(Abil.MC) * 0.4), HUtil32.Round(HUtil32.HiWord(Abil.MC) * 0.4));
                    Abil.SC = HUtil32.MakeLong(HUtil32.Round(HUtil32.LoWord(Abil.SC) * 0.4), HUtil32.Round(HUtil32.HiWord(Abil.SC) * 0.4));
                }
                else if (HUtil32.RangeInDefined(HungerStatus, 2000, 2999))
                {
                    Abil.DC = HUtil32.MakeLong(HUtil32.Round(HUtil32.LoWord(Abil.DC) * 0.6), HUtil32.Round(HUtil32.HiWord(Abil.DC) * 0.6));
                    Abil.MC = HUtil32.MakeLong(HUtil32.Round(HUtil32.LoWord(Abil.MC) * 0.6), HUtil32.Round(HUtil32.HiWord(Abil.MC) * 0.6));
                    Abil.SC = HUtil32.MakeLong(HUtil32.Round(HUtil32.LoWord(Abil.SC) * 0.6), HUtil32.Round(HUtil32.HiWord(Abil.SC) * 0.6));
                }
                else if (HUtil32.RangeInDefined(HungerStatus, 3000, 3000))
                {
                    Abil.DC = HUtil32.MakeLong(HUtil32.Round(HUtil32.LoWord(Abil.DC) * 0.9), HUtil32.Round(HUtil32.HiWord(Abil.DC) * 0.9));
                    Abil.MC = HUtil32.MakeLong(HUtil32.Round(HUtil32.LoWord(Abil.MC) * 0.9), HUtil32.Round(HUtil32.HiWord(Abil.MC) * 0.9));
                    Abil.SC = HUtil32.MakeLong(HUtil32.Round(HUtil32.LoWord(Abil.SC) * 0.9), HUtil32.Round(HUtil32.HiWord(Abil.SC) * 0.9));
                }
            }
        }

        public virtual void RecalcAbilitys()
        {
            bool[] cghi = new bool[4]{false,false,false,false};
            StdItem stdItem;
            AddAbil = new AddAbility();
            var temp = WAbil;
            WAbil = Abil;
            WAbil.HP = temp.HP;
            WAbil.MP = temp.MP;
            WAbil.Weight = 0;
            WAbil.WearWeight = 0;
            WAbil.HandWeight = 0;
            AntiPoison = 0;
            PoisonRecover = 0;
            HealthRecover = 0;
            SpellRecover = 0;
            AntiMagic = 1;
            Luck = 0;
            HitSpeed = 0;
            var oldhmode = HideMode;
            HideMode = false;
            Teleport = false;
            Paralysis = false;
            Revival = false;
            FlameRing = false;
            RecoveryRing = false;
            AngryRing = false;
            MagicShield = false;
            MuscleRing = false;
            FastTrain = false;
            ProbeNecklace = false;
            MoXieSuite = 0;
            var mh_ring = false;
            var mh_bracelet = false;
            var mh_necklace = false;
            SuckupEnemyHealthRate = 0;
            SuckupEnemyHealth = 0;
            var sh_ring = false;
            var sh_bracelet = false;
            var sh_necklace = false;
            var hp_ring = false;
            var hp_bracelet = false;
            var mp_ring = false;
            var mp_bracelet = false;
            var hpmp_ring = false;
            var hpmp_bracelet = false;
            var hpp_necklace = false;
            var hpp_bracelet = false;
            var hpp_ring = false;
            var cho_weapon = false;
            var cho_necklace = false;
            var cho_ring = false;
            var cho_helmet = false;
            var cho_bracelet = false;
            var pset_necklace = false;
            var pset_bracelet = false;
            var pset_ring = false;
            var hset_necklace = false;
            var hset_bracelet = false;
            var hset_ring = false;
            var yset_necklace = false;
            var yset_bracelet = false;
            var yset_ring = false;
            var boneset_weapon = false;
            var boneset_helmet = false;
            var boneset_dress = false;
            var bugset_necklace = false;
            var bugset_ring = false;
            var bugset_bracelet = false;
            var ptset_belt = false;
            var ptset_boots = false;
            var ptset_necklace = false;
            var ptset_bracelet = false;
            var ptset_ring = false;
            var ksset_belt = false;
            var ksset_boots = false;
            var ksset_necklace = false;
            var ksset_bracelet = false;
            var ksset_ring = false;
            var rubyset_belt = false;
            var rubyset_boots = false;
            var rubyset_necklace = false;
            var rubyset_bracelet = false;
            var rubyset_ring = false;
            var strong_ptset_belt = false;
            var strong_ptset_boots = false;
            var strong_ptset_necklace = false;
            var strong_ptset_bracelet = false;
            var strong_ptset_ring = false;
            var strong_ksset_belt = false;
            var strong_ksset_boots = false;
            var strong_ksset_necklace = false;
            var strong_ksset_bracelet = false;
            var strong_ksset_ring = false;
            var strong_rubyset_belt = false;
            var strong_rubyset_boots = false;
            var strong_rubyset_necklace = false;
            var strong_rubyset_bracelet = false;
            var strong_rubyset_ring = false;
            var dragonset_ring_left = false;
            var dragonset_ring_right = false;
            var dragonset_bracelet_left = false;
            var dragonset_bracelet_right = false;
            var dragonset_necklace = false;
            var dragonset_dress = false;
            var dragonset_helmet = false;
            var dragonset_weapon = false;
            var dragonset_boots = false;
            var dragonset_belt = false;
            RecallSuite = false;
            var dset_wingdress = false;
            if ((Race == Grobal2.RC_PLAYOBJECT))
            {
                for (var i = 0; i <= 12; i++)
                {
                    if ((UseItems[i].Index > 0))
                    {
                        if (UseItems[i].Dura == 0)
                        {
                            stdItem = M2Share.WorldEngine.GetStdItem(UseItems[i].Index);
                            if (stdItem != null)
                            {
                                if ((i == Grobal2.U_WEAPON) || (i == Grobal2.U_RIGHTHAND))
                                {
                                    WAbil.HandWeight = (byte)(WAbil.HandWeight + stdItem.Weight);
                                }
                                else
                                {
                                    WAbil.WearWeight = (byte)(WAbil.WearWeight + stdItem.Weight);
                                }
                            }
                            continue;
                        }
                        stdItem = M2Share.WorldEngine.GetStdItem(UseItems[i].Index);
                        ApplyItemParameters(UseItems[i], stdItem, ref AddAbil);
                        ApplyItemParametersEx(UseItems[i], ref WAbil);
                        if (stdItem != null)
                        {
                            if ((i == Grobal2.U_WEAPON) || (i == Grobal2.U_RIGHTHAND))
                            {
                                WAbil.HandWeight = (byte)(WAbil.HandWeight + stdItem.Weight);
                            }
                            else
                            {
                                WAbil.WearWeight = (byte)(WAbil.WearWeight + stdItem.Weight);
                            }
                            switch (i)
                            {
                                case Grobal2.U_WEAPON:
                                case Grobal2.U_ARMRINGL:
                                case Grobal2.U_ARMRINGR:
                                    {
                                        if ((stdItem.SpecialPwr <= -1) && (stdItem.SpecialPwr >= -50))
                                        {
                                            AddAbil.UndeadPower = (byte)(AddAbil.UndeadPower + (-stdItem.SpecialPwr));
                                        }
                                        if ((stdItem.SpecialPwr <= -51) && (stdItem.SpecialPwr >= -100))
                                        {
                                            AddAbil.UndeadPower = (byte)(AddAbil.UndeadPower + (stdItem.SpecialPwr + 50));
                                        }
                                        if (stdItem.Shape == ItemShapeConst.CCHO_WEAPON)
                                        {
                                            cho_weapon = true;
                                        }
                                        if ((stdItem.Shape == ItemShapeConst.BONESET_WEAPON_SHAPE) && (stdItem.StdMode == 6))
                                        {
                                            boneset_weapon = true;
                                        }
                                        if (stdItem.Shape == DragonConst.DRAGON_WEAPON_SHAPE)
                                        {
                                            dragonset_weapon = true;
                                        }
                                        break;
                                    }
                                case Grobal2.U_NECKLACE:
                                    {
                                        if (stdItem.Shape == ItemShapeConst.NECTLACE_FASTTRAINING_ITEM)
                                        {
                                            FastTrain = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.NECTLACE_SEARCH_ITEM)
                                        {
                                            ProbeNecklace = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.NECKLACE_GI_ITEM)
                                        {
                                            cghi[1] = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.NECKLACE_OF_MANATOHEALTH)
                                        {
                                            mh_necklace = true;
                                            MoXieSuite = MoXieSuite + stdItem.AniCount;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.NECKLACE_OF_SUCKHEALTH)
                                        {
                                            sh_necklace = true;
                                            SuckupEnemyHealthRate = SuckupEnemyHealthRate + stdItem.AniCount;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.NECKLACE_OF_HPPUP)
                                        {
                                            hpp_necklace = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.CCHO_NECKLACE)
                                        {
                                            cho_necklace = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.PSET_NECKLACE_SHAPE)
                                        {
                                            pset_necklace = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.HSET_NECKLACE_SHAPE)
                                        {
                                            hset_necklace = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.YSET_NECKLACE_SHAPE)
                                        {
                                            yset_necklace = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.BUGSET_NECKLACE_SHAPE)
                                        {
                                            bugset_necklace = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.PTSET_NECKLACE_SHAPE)
                                        {
                                            ptset_necklace = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.KSSET_NECKLACE_SHAPE)
                                        {
                                            ksset_necklace = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.RUBYSET_NECKLACE_SHAPE)
                                        {
                                            rubyset_necklace = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.STRONG_PTSET_NECKLACE_SHAPE)
                                        {
                                            strong_ptset_necklace = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.STRONG_KSSET_NECKLACE_SHAPE)
                                        {
                                            strong_ksset_necklace = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.STRONG_RUBYSET_NECKLACE_SHAPE)
                                        {
                                            strong_rubyset_necklace = true;
                                        }
                                        if (stdItem.Shape == DragonConst.DRAGON_NECKLACE_SHAPE)
                                        {
                                            dragonset_necklace = true;
                                        }
                                        break;
                                    }
                                case Grobal2.U_RINGR:
                                case Grobal2.U_RINGL:
                                    {
                                        if (stdItem.Shape == ItemShapeConst.RING_TRANSPARENT_ITEM)
                                        {
                                            StatusArr[Grobal2.STATE_TRANSPARENT] = 60000;
                                            HideMode = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.RING_SPACEMOVE_ITEM)
                                        {
                                            Teleport = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.RING_MAKESTONE_ITEM)
                                        {
                                            Paralysis = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.RING_REVIVAL_ITEM)
                                        {
                                            Revival = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.RING_FIREBALL_ITEM)
                                        {
                                            FlameRing = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.RING_HEALING_ITEM)
                                        {
                                            RecoveryRing = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.RING_ANGERENERGY_ITEM)
                                        {
                                            AngryRing = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.RING_MAGICSHIELD_ITEM)
                                        {
                                            MagicShield = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.RING_SUPERSTRENGTH_ITEM)
                                        {
                                            MuscleRing = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.RING_CHUN_ITEM)
                                        {
                                            cghi[0] = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.RING_OF_MANATOHEALTH)
                                        {
                                            mh_ring = true;
                                            MoXieSuite = MoXieSuite + stdItem.AniCount;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.RING_OF_SUCKHEALTH)
                                        {
                                            sh_ring = true;
                                            SuckupEnemyHealthRate = SuckupEnemyHealthRate + stdItem.AniCount;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.RING_OF_HPUP)
                                        {
                                            hp_ring = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.RING_OF_MPUP)
                                        {
                                            mp_ring = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.RING_OF_HPMPUP)
                                        {
                                            hpmp_ring = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.RING_OH_HPPUP)
                                        {
                                            hpp_ring = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.CCHO_RING)
                                        {
                                            cho_ring = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.PSET_RING_SHAPE)
                                        {
                                            pset_ring = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.HSET_RING_SHAPE)
                                        {
                                            hset_ring = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.YSET_RING_SHAPE)
                                        {
                                            yset_ring = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.BUGSET_RING_SHAPE)
                                        {
                                            bugset_ring = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.PTSET_RING_SHAPE)
                                        {
                                            ptset_ring = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.KSSET_RING_SHAPE)
                                        {
                                            ksset_ring = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.RUBYSET_RING_SHAPE)
                                        {
                                            rubyset_ring = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.STRONG_PTSET_RING_SHAPE)
                                        {
                                            strong_ptset_ring = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.STRONG_KSSET_RING_SHAPE)
                                        {
                                            strong_ksset_ring = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.STRONG_RUBYSET_RING_SHAPE)
                                        {
                                            strong_rubyset_ring = true;
                                        }
                                        if (stdItem.Shape == DragonConst.DRAGON_RING_SHAPE)
                                        {
                                            if ((i == Grobal2.U_RINGL))
                                            {
                                                dragonset_ring_left = true;
                                            }
                                            if ((i == Grobal2.U_RINGR))
                                            {
                                                dragonset_ring_right = true;
                                            }
                                        }
                                        break;
                                    }
                            }
                            switch (i)
                            {
                                case Grobal2.U_ARMRINGL:
                                case Grobal2.U_ARMRINGR:
                                    {
                                        if (stdItem.Shape == ItemShapeConst.ARMRING_HAP_ITEM)
                                        {
                                            cghi[2] = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.BRACELET_OF_MANATOHEALTH)
                                        {
                                            mh_bracelet = true;
                                            MoXieSuite = MoXieSuite + stdItem.AniCount;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.BRACELET_OF_SUCKHEALTH)
                                        {
                                            sh_bracelet = true;
                                            SuckupEnemyHealthRate = SuckupEnemyHealthRate + stdItem.AniCount;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.BRACELET_OF_HPUP)
                                        {
                                            hp_bracelet = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.BRACELET_OF_MPUP)
                                        {
                                            mp_bracelet = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.BRACELET_OF_HPMPUP)
                                        {
                                            hpmp_bracelet = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.BRACELET_OF_HPPUP)
                                        {
                                            hpp_bracelet = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.CCHO_BRACELET)
                                        {
                                            cho_bracelet = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.PSET_BRACELET_SHAPE)
                                        {
                                            pset_bracelet = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.HSET_BRACELET_SHAPE)
                                        {
                                            hset_bracelet = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.YSET_BRACELET_SHAPE)
                                        {
                                            yset_bracelet = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.BUGSET_BRACELET_SHAPE)
                                        {
                                            bugset_bracelet = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.PTSET_BRACELET_SHAPE)
                                        {
                                            ptset_bracelet = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.KSSET_BRACELET_SHAPE)
                                        {
                                            ksset_bracelet = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.RUBYSET_BRACELET_SHAPE)
                                        {
                                            rubyset_bracelet = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.STRONG_PTSET_BRACELET_SHAPE)
                                        {
                                            strong_ptset_bracelet = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.STRONG_KSSET_BRACELET_SHAPE)
                                        {
                                            strong_ksset_bracelet = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.STRONG_RUBYSET_BRACELET_SHAPE)
                                        {
                                            strong_rubyset_bracelet = true;
                                        }
                                        if (stdItem.Shape == DragonConst.DRAGON_BRACELET_SHAPE)
                                        {
                                            if ((i == Grobal2.U_ARMRINGL))
                                            {
                                                dragonset_bracelet_left = true;
                                            }
                                            if ((i == Grobal2.U_ARMRINGR))
                                            {
                                                dragonset_bracelet_right = true;
                                            }
                                        }
                                        break;
                                    }
                                case Grobal2.U_HELMET:
                                    {
                                        if (stdItem.Shape == ItemShapeConst.HELMET_IL_ITEM)
                                        {
                                            cghi[3] = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.CCHO_HELMET)
                                        {
                                            cho_helmet = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.BONESET_HELMET_SHAPE)
                                        {
                                            boneset_helmet = true;
                                        }
                                        if (stdItem.Shape == DragonConst.DRAGON_HELMET_SHAPE)
                                        {
                                            dragonset_helmet = true;
                                        }
                                        break;
                                    }
                                case Grobal2.U_DRESS:
                                    {
                                        if (stdItem.Shape == ItemShapeConst.DRESS_SHAPE_WING)
                                        {
                                            dset_wingdress = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.BONESET_DRESS_SHAPE)
                                        {
                                            boneset_dress = true;
                                        }
                                        if (stdItem.Shape == DragonConst.DRAGON_DRESS_SHAPE)
                                        {
                                            dragonset_dress = true;
                                        }
                                        break;
                                    }
                                case Grobal2.U_BELT:
                                    {
                                        if (stdItem.Shape == ItemShapeConst.PTSET_BELT_SHAPE)
                                        {
                                            ptset_belt = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.KSSET_BELT_SHAPE)
                                        {
                                            ksset_belt = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.RUBYSET_BELT_SHAPE)
                                        {
                                            rubyset_belt = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.STRONG_PTSET_BELT_SHAPE)
                                        {
                                            strong_ptset_belt = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.STRONG_KSSET_BELT_SHAPE)
                                        {
                                            strong_ksset_belt = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.STRONG_RUBYSET_BELT_SHAPE)
                                        {
                                            strong_rubyset_belt = true;
                                        }
                                        if (stdItem.Shape == DragonConst.DRAGON_BELT_SHAPE)
                                        {
                                            dragonset_belt = true;
                                        }
                                        break;
                                    }
                                case Grobal2.U_BOOTS:
                                    {
                                        if (stdItem.Shape == ItemShapeConst.PTSET_BOOTS_SHAPE)
                                        {
                                            ptset_boots = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.KSSET_BOOTS_SHAPE)
                                        {
                                            ksset_boots = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.RUBYSET_BOOTS_SHAPE)
                                        {
                                            rubyset_boots = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.STRONG_PTSET_BOOTS_SHAPE)
                                        {
                                            strong_ptset_boots = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.STRONG_KSSET_BOOTS_SHAPE)
                                        {
                                            strong_ksset_boots = true;
                                        }
                                        if (stdItem.Shape == ItemShapeConst.STRONG_RUBYSET_BOOTS_SHAPE)
                                        {
                                            strong_rubyset_boots = true;
                                        }
                                        if (stdItem.Shape == DragonConst.DRAGON_BOOTS_SHAPE)
                                        {
                                            dragonset_boots = true;
                                        }
                                        break;
                                    }
                                case Grobal2.U_CHARM:
                                    {
                                        if ((stdItem.StdMode == 53) && (stdItem.Shape == ItemShapeConst.SHAPE_OF_LUCKYLADLE))
                                        {
                                            AddAbil.Luck = (byte)(HUtil32._MIN(255, AddAbil.Luck + 1));
                                        }
                                        break;
                                    }
                            }
                        }
                    }
                }
                if (cghi[0] && cghi[1] && cghi[2] && cghi[3])
                {
                    RecallSuite = true;
                }
                if (mh_necklace && mh_bracelet && mh_ring)
                {
                    MoXieSuite = MoXieSuite + 50;
                }
                if (sh_necklace && sh_bracelet && sh_ring)
                {
                    AddAbil.HIT = (ushort)(AddAbil.HIT + 2);
                }
                if (hp_bracelet && hp_ring)
                {
                    AddAbil.HP = (ushort)(AddAbil.HP + 50);
                }
                if (mp_bracelet && mp_ring)
                {
                    AddAbil.MP = (ushort)(AddAbil.MP + 50);
                }
                if (hpmp_bracelet && hpmp_ring)
                {
                    AddAbil.HP = (ushort)(AddAbil.HP + 30);
                    AddAbil.MP = (ushort)(AddAbil.MP + 30);
                }
                if (hpp_necklace && hpp_bracelet && hpp_ring)
                {
                    AddAbil.HP = (ushort)(AddAbil.HP + ((WAbil.MaxHP * 30) / 100));
                    AddAbil.AC = (ushort)(AddAbil.AC + HUtil32.MakeWord(2, 2));
                }
                if (cho_weapon && cho_necklace && cho_ring && cho_helmet && cho_bracelet)
                {
                    AddAbil.HitSpeed = (ushort)(AddAbil.HitSpeed + 4);
                    AddAbil.DC = (ushort)(AddAbil.DC + HUtil32.MakeWord(2, 5));
                }
                if (pset_bracelet && pset_ring)
                {
                    AddAbil.HitSpeed = (ushort)(AddAbil.HitSpeed + 2);
                    if (pset_necklace)
                    {
                        AddAbil.DC = (ushort)(AddAbil.DC + HUtil32.MakeWord(1, 3));
                    }
                }
                if (hset_bracelet && hset_ring)
                {
                    WAbil.MaxWeight = (ushort)(WAbil.MaxWeight + 20);
                    WAbil.MaxWearWeight = (byte)(HUtil32._MIN(255, WAbil.MaxWearWeight + 5));
                    if (hset_necklace)
                    {
                        AddAbil.MC = (ushort)(AddAbil.MC + HUtil32.MakeWord(1, 2));
                    }
                }
                if (yset_bracelet && yset_ring)
                {
                    AddAbil.UndeadPower = (byte)(AddAbil.UndeadPower + 3);
                    if (yset_necklace)
                    {
                        AddAbil.SC = (ushort)(AddAbil.SC + HUtil32.MakeWord(1, 2));
                    }
                }
                if (boneset_weapon && boneset_helmet && boneset_dress)
                {
                    AddAbil.AC = (ushort)(AddAbil.AC + HUtil32.MakeWord(0, 2));
                    AddAbil.MC = (ushort)(AddAbil.MC + HUtil32.MakeWord(0, 1));
                    AddAbil.SC = (ushort)(AddAbil.SC + HUtil32.MakeWord(0, 1));
                }
                if (bugset_necklace && bugset_ring && bugset_bracelet)
                {
                    AddAbil.DC = (ushort)(AddAbil.DC + HUtil32.MakeWord(0, 1));
                    AddAbil.MC = (ushort)(AddAbil.MC + HUtil32.MakeWord(0, 1));
                    AddAbil.SC = (ushort)(AddAbil.SC + HUtil32.MakeWord(0, 1));
                    AddAbil.AntiMagic = (ushort)(AddAbil.AntiMagic + 1);
                    AddAbil.AntiPoison = (ushort)(AddAbil.AntiPoison + 1);
                }
                if (ptset_belt && ptset_boots && ptset_necklace && ptset_bracelet && ptset_ring)
                {
                    AddAbil.DC = (ushort)(AddAbil.DC + HUtil32.MakeWord(0, 2));
                    AddAbil.AC = (ushort)(AddAbil.AC + HUtil32.MakeWord(0, 2));
                    WAbil.MaxHandWeight = (byte)(HUtil32._MIN(255, WAbil.MaxHandWeight + 1));
                    WAbil.MaxWearWeight = (byte)(HUtil32._MIN(255, WAbil.MaxWearWeight + 2));
                }
                if (ksset_belt && ksset_boots && ksset_necklace && ksset_bracelet && ksset_ring)
                {
                    AddAbil.SC = (ushort)(AddAbil.SC + HUtil32.MakeWord(0, 2));
                    AddAbil.AC = (ushort)(AddAbil.AC + HUtil32.MakeWord(0, 1));
                    AddAbil.MAC = (ushort)(AddAbil.MAC + HUtil32.MakeWord(0, 1));
                    AddAbil.SPEED = (ushort)(AddAbil.SPEED + 1);
                    WAbil.MaxHandWeight = (byte)(HUtil32._MIN(255, WAbil.MaxHandWeight + 1));
                    WAbil.MaxWearWeight = (byte)(HUtil32._MIN(255, WAbil.MaxWearWeight + 2));
                }
                if (rubyset_belt && rubyset_boots && rubyset_necklace && rubyset_bracelet && rubyset_ring)
                {
                    AddAbil.MC = (ushort)(AddAbil.MC + HUtil32.MakeWord(0, 2));
                    AddAbil.MAC = (ushort)(AddAbil.MAC + HUtil32.MakeWord(0, 2));
                    WAbil.MaxHandWeight = (byte)(HUtil32._MIN(255, WAbil.MaxHandWeight + 1));
                    WAbil.MaxWearWeight = (byte)(HUtil32._MIN(255, WAbil.MaxWearWeight + 2));
                }
                if (strong_ptset_belt && strong_ptset_boots && strong_ptset_necklace && strong_ptset_bracelet && strong_ptset_ring)
                {
                    AddAbil.DC = (ushort)(AddAbil.DC + HUtil32.MakeWord(0, 3));
                    AddAbil.HP = (ushort)(AddAbil.HP + 30);
                    AddAbil.HitSpeed = (ushort)(AddAbil.HitSpeed + 2);
                    WAbil.MaxWearWeight = (byte)(HUtil32._MIN(255, WAbil.MaxWearWeight + 2));
                }
                if (strong_ksset_belt && strong_ksset_boots && strong_ksset_necklace && strong_ksset_bracelet && strong_ksset_ring)
                {
                    AddAbil.SC = (ushort)(AddAbil.SC + HUtil32.MakeWord(0, 2));
                    AddAbil.HP = (ushort)(AddAbil.HP + 15);
                    AddAbil.MP = (ushort)(AddAbil.MP + 20);
                    AddAbil.UndeadPower = (byte)(AddAbil.UndeadPower + 1);
                    AddAbil.HIT = (ushort)(AddAbil.HIT + 1);
                    AddAbil.SPEED = (ushort)(AddAbil.SPEED + 1);
                    WAbil.MaxWearWeight = (byte)(HUtil32._MIN(255, WAbil.MaxWearWeight + 2));
                }
                if (strong_rubyset_belt && strong_rubyset_boots && strong_rubyset_necklace && strong_rubyset_bracelet && strong_rubyset_ring)
                {
                    AddAbil.MC = (ushort)(AddAbil.MC + HUtil32.MakeWord(0, 2));
                    AddAbil.MP = (ushort)(AddAbil.MP + 40);
                    AddAbil.SPEED = (ushort)(AddAbil.SPEED + 2);
                    WAbil.MaxWearWeight = (byte)(HUtil32._MIN(255, WAbil.MaxWearWeight + 2));
                }
                if (dragonset_ring_left && dragonset_ring_right && dragonset_bracelet_left && dragonset_bracelet_right && dragonset_necklace && dragonset_dress && dragonset_helmet && dragonset_weapon && dragonset_boots && dragonset_belt)
                {
                    AddAbil.AC = HUtil32.MakeWord(HUtil32.LoByte(AddAbil.AC) + 1, HUtil32._MIN(255, HUtil32.HiByte(AddAbil.AC) + 4));
                    AddAbil.MAC = HUtil32.MakeWord(HUtil32.LoByte(AddAbil.MAC) + 1, HUtil32._MIN(255, HUtil32.HiByte(AddAbil.MAC) + 4));
                    AddAbil.Luck = (byte)(HUtil32._MIN(255, AddAbil.Luck + 2));
                    AddAbil.HitSpeed = (ushort)(AddAbil.HitSpeed + 2);
                    AddAbil.AntiMagic = (ushort)(AddAbil.AntiMagic + 6);
                    AddAbil.AntiPoison = (ushort)(AddAbil.AntiPoison + 6);
                    WAbil.MaxHandWeight = (byte)(HUtil32._MIN(255, WAbil.MaxHandWeight + 34));
                    WAbil.MaxWearWeight = (byte)(HUtil32._MIN(255, WAbil.MaxWearWeight + 27));
                    WAbil.MaxWeight = (ushort)(WAbil.MaxWeight + 120);
                    WAbil.MaxHP = (ushort)(WAbil.MaxHP + 70);
                    WAbil.MaxMP = (ushort)(WAbil.MaxMP + 80);
                    AddAbil.SPEED = (ushort)(AddAbil.SPEED + 1);
                    AddAbil.DC = HUtil32.MakeWord(HUtil32.LoByte(AddAbil.DC) + 1, HUtil32._MIN(255, HUtil32.HiByte(AddAbil.DC) + 4));
                    AddAbil.MC = HUtil32.MakeWord(HUtil32.LoByte(AddAbil.MC) + 1, HUtil32._MIN(255, HUtil32.HiByte(AddAbil.MC) + 3));
                    AddAbil.SC = HUtil32.MakeWord(HUtil32.LoByte(AddAbil.SC) + 1, HUtil32._MIN(255, HUtil32.HiByte(AddAbil.SC) + 3));
                }
                else
                {
                    if (dragonset_dress && dragonset_helmet && dragonset_weapon && dragonset_boots && dragonset_belt)
                    {
                        WAbil.MaxHandWeight = (byte)(HUtil32._MIN(255, WAbil.MaxHandWeight + 34));
                        WAbil.MaxWeight = (ushort)(WAbil.MaxWeight + 50);
                        AddAbil.SPEED = (ushort)(AddAbil.SPEED + 1);
                        AddAbil.DC = HUtil32.MakeWord(HUtil32.LoByte(AddAbil.DC) + 1, HUtil32._MIN(255, HUtil32.HiByte(AddAbil.DC) + 4));
                        AddAbil.MC = HUtil32.MakeWord(HUtil32.LoByte(AddAbil.MC) + 1, HUtil32._MIN(255, HUtil32.HiByte(AddAbil.MC) + 3));
                        AddAbil.SC = HUtil32.MakeWord(HUtil32.LoByte(AddAbil.SC) + 1, HUtil32._MIN(255, HUtil32.HiByte(AddAbil.SC) + 3));
                    }
                    else if (dragonset_dress && dragonset_boots && dragonset_belt)
                    {
                        WAbil.MaxHandWeight = (byte)(HUtil32._MIN(255, WAbil.MaxHandWeight + 17));
                        WAbil.MaxWeight = (ushort)(WAbil.MaxWeight + 30);
                        AddAbil.DC = HUtil32.MakeWord(HUtil32.LoByte(AddAbil.DC), HUtil32._MIN(255, HUtil32.HiByte(AddAbil.DC) + 1));
                        AddAbil.MC = HUtil32.MakeWord(HUtil32.LoByte(AddAbil.MC), HUtil32._MIN(255, HUtil32.HiByte(AddAbil.MC) + 1));
                        AddAbil.SC = HUtil32.MakeWord(HUtil32.LoByte(AddAbil.SC), HUtil32._MIN(255, HUtil32.HiByte(AddAbil.SC) + 1));
                    }
                    else if (dragonset_dress && dragonset_helmet && dragonset_weapon)
                    {
                        AddAbil.DC = HUtil32.MakeWord(HUtil32.LoByte(AddAbil.DC), HUtil32._MIN(255, HUtil32.HiByte(AddAbil.DC) + 2));
                        AddAbil.MC = HUtil32.MakeWord(HUtil32.LoByte(AddAbil.MC), HUtil32._MIN(255, HUtil32.HiByte(AddAbil.MC) + 1));
                        AddAbil.SC = HUtil32.MakeWord(HUtil32.LoByte(AddAbil.SC), HUtil32._MIN(255, HUtil32.HiByte(AddAbil.SC) + 1));
                        AddAbil.SPEED = (ushort)(AddAbil.SPEED + 1);
                    }
                    if (dragonset_ring_left && dragonset_ring_right && dragonset_bracelet_left && dragonset_bracelet_right && dragonset_necklace)
                    {
                        WAbil.MaxWearWeight = (byte)(HUtil32._MIN(255, WAbil.MaxWearWeight + 27));
                        WAbil.MaxWeight = (ushort)(WAbil.MaxWeight + 50);
                        AddAbil.AC = HUtil32.MakeWord(HUtil32.LoByte(AddAbil.AC) + 1, HUtil32._MIN(255, HUtil32.HiByte(AddAbil.AC) + 3));
                        AddAbil.MAC = HUtil32.MakeWord(HUtil32.LoByte(AddAbil.MAC) + 1, HUtil32._MIN(255, HUtil32.HiByte(AddAbil.MAC) + 3));
                    }
                    else if ((dragonset_ring_left || dragonset_ring_right) && dragonset_bracelet_left && dragonset_bracelet_right && dragonset_necklace)
                    {
                        WAbil.MaxWearWeight = (byte)(HUtil32._MIN(255, WAbil.MaxWearWeight + 17));
                        WAbil.MaxWeight = (ushort)(WAbil.MaxWeight + 30);
                        AddAbil.AC = HUtil32.MakeWord(HUtil32.LoByte(AddAbil.AC) + 1, HUtil32._MIN(255, HUtil32.HiByte(AddAbil.AC) + 1));
                        AddAbil.MAC = HUtil32.MakeWord(HUtil32.LoByte(AddAbil.MAC) + 1, HUtil32._MIN(255, HUtil32.HiByte(AddAbil.MAC) + 1));
                    }
                    else if (dragonset_ring_left && dragonset_ring_right && (dragonset_bracelet_left || dragonset_bracelet_right) && dragonset_necklace)
                    {
                        WAbil.MaxWearWeight = (byte)(HUtil32._MIN(255, WAbil.MaxWearWeight + 17));
                        WAbil.MaxWeight = (ushort)(WAbil.MaxWeight + 30);
                        AddAbil.AC = HUtil32.MakeWord(HUtil32.LoByte(AddAbil.AC), HUtil32._MIN(255, HUtil32.HiByte(AddAbil.AC) + 2));
                        AddAbil.MAC = HUtil32.MakeWord(HUtil32.LoByte(AddAbil.MAC), HUtil32._MIN(255, HUtil32.HiByte(AddAbil.MAC) + 2));
                    }
                    else if ((dragonset_ring_left || dragonset_ring_right) && (dragonset_bracelet_left || dragonset_bracelet_right) && dragonset_necklace)
                    {
                        WAbil.MaxWearWeight = (byte)(HUtil32._MIN(255, WAbil.MaxWearWeight + 17));
                        WAbil.MaxWeight = (ushort)(WAbil.MaxWeight + 30);
                        AddAbil.AC = HUtil32.MakeWord(HUtil32.LoByte(AddAbil.AC), HUtil32._MIN(255, HUtil32.HiByte(AddAbil.AC) + 1));
                        AddAbil.MAC = HUtil32.MakeWord(HUtil32.LoByte(AddAbil.MAC), HUtil32._MIN(255, HUtil32.HiByte(AddAbil.MAC) + 1));
                    }
                    else
                    {
                        if (dragonset_bracelet_left && dragonset_bracelet_right)
                        {
                            AddAbil.AC = HUtil32.MakeWord(HUtil32.LoByte(AddAbil.AC) + 1, HUtil32._MIN(255, HUtil32.HiByte(AddAbil.AC)));
                            AddAbil.MAC = HUtil32.MakeWord(HUtil32.LoByte(AddAbil.MAC) + 1, HUtil32._MIN(255, HUtil32.HiByte(AddAbil.MAC)));
                        }
                        if (dragonset_ring_left && dragonset_ring_right)
                        {
                            AddAbil.AC = HUtil32.MakeWord(HUtil32.LoByte(AddAbil.AC), HUtil32._MIN(255, HUtil32.HiByte(AddAbil.AC) + 1));
                            AddAbil.MAC = HUtil32.MakeWord(HUtil32.LoByte(AddAbil.MAC), HUtil32._MIN(255, HUtil32.HiByte(AddAbil.MAC) + 1));
                        }
                    }
                }
                if (dset_wingdress && (Abil.Level >= 20))
                {
                    if ((Abil.Level < 30))
                    {

                    }
                    else if ((Abil.Level < 40))
                    {
                        AddAbil.DC = (ushort)(AddAbil.DC + HUtil32.MakeWord(0, 1));
                        AddAbil.MC = (ushort)(AddAbil.MC + HUtil32.MakeWord(0, 2));
                        AddAbil.SC = (ushort)(AddAbil.SC + HUtil32.MakeWord(0, 2));
                        AddAbil.AC = (ushort)(AddAbil.AC + HUtil32.MakeWord(2, 3));
                        AddAbil.MAC = (ushort)(AddAbil.MAC + HUtil32.MakeWord(0, 2));
                    }
                    else if ((Abil.Level < 50))
                    {
                        AddAbil.DC = (ushort)(AddAbil.DC + HUtil32.MakeWord(0, 3));
                        AddAbil.MC = (ushort)(AddAbil.MC + HUtil32.MakeWord(0, 4));
                        AddAbil.SC = (ushort)(AddAbil.SC + HUtil32.MakeWord(0, 4));
                        AddAbil.AC = (ushort)(AddAbil.AC + HUtil32.MakeWord(5, 5));
                        AddAbil.MAC = (ushort)(AddAbil.MAC + HUtil32.MakeWord(1, 2));
                    }
                    else
                    {
                        AddAbil.DC = (ushort)(AddAbil.DC + HUtil32.MakeWord(0, 5));
                        AddAbil.MC = (ushort)(AddAbil.MC + HUtil32.MakeWord(0, 6));
                        AddAbil.SC = (ushort)(AddAbil.SC + HUtil32.MakeWord(0, 6));
                        AddAbil.AC = (ushort)(AddAbil.AC + HUtil32.MakeWord(9, 7));
                        AddAbil.MAC = (ushort)(AddAbil.MAC + HUtil32.MakeWord(2, 4));
                    }
                }
                WAbil.Weight = RecalcBagWeight();
            }
            if (Transparent && (StatusArr[Grobal2.STATE_TRANSPARENT] > 0))
            {
                HideMode = true;
            }
            if (HideMode)
            {
                if (!oldhmode)
                {
                    CharStatus = GetCharStatus();
                    StatusChanged();
                }
            }
            else
            {
                if (oldhmode)
                {
                    StatusArr[Grobal2.STATE_TRANSPARENT] = 0;
                    CharStatus = GetCharStatus();
                    StatusChanged();
                }
            }
            RecalcHitSpeed();
            if (AddAbil.HitSpeed >= 0)
            {
                AddAbil.HitSpeed = ((ushort)(AddAbil.HitSpeed / 2));
            }
            else
            {
                AddAbil.HitSpeed = (ushort)((AddAbil.HitSpeed - 1) / 2);
            }
            AddAbil.HitSpeed = (ushort)HUtil32._MIN(15, AddAbil.HitSpeed);
            var oldlight = Light;
            Light = GetMyLight();
            if (oldlight != Light)
            {
                SendRefMsg(Grobal2.RM_CHANGELIGHT, 0, 0, 0, 0, "");
            }
            SpeedPoint = (byte)(SpeedPoint + AddAbil.SPEED);
            HitPoint = (byte)(HitPoint + AddAbil.HIT);
            AntiPoison = (byte)(AntiPoison + AddAbil.AntiPoison);
            PoisonRecover = (ushort)(PoisonRecover + AddAbil.PoisonRecover);
            HealthRecover = (ushort)(HealthRecover + AddAbil.HealthRecover);
            SpellRecover = (ushort)(SpellRecover + AddAbil.SpellRecover);
            AntiMagic = (ushort)(AntiMagic + AddAbil.AntiMagic);
            Luck = Luck + AddAbil.Luck;
            Luck = Luck - AddAbil.UnLuck;
            HitSpeed = AddAbil.HitSpeed;
            WAbil.MaxHP = (ushort)(Abil.MaxHP + AddAbil.HP);
            WAbil.MaxMP = (ushort)(Abil.MaxMP + AddAbil.MP);
            WAbil.AC = HUtil32.MakeWord(HUtil32._MIN(255, HUtil32.LoByte(AddAbil.AC) + HUtil32.LoByte(Abil.AC)), HUtil32._MIN(255, HUtil32.HiByte(AddAbil.AC) + HUtil32.HiByte(Abil.AC)));
            WAbil.MAC = HUtil32.MakeWord(HUtil32._MIN(255, HUtil32.LoByte(AddAbil.MAC) + HUtil32.LoByte(Abil.MAC)), HUtil32._MIN(255, HUtil32.HiByte(AddAbil.MAC) + HUtil32.HiByte(Abil.MAC)));
            WAbil.DC = HUtil32.MakeWord(HUtil32._MIN(255, HUtil32.LoByte(AddAbil.DC) + HUtil32.LoByte(Abil.DC)), HUtil32._MIN(255, HUtil32.HiByte(AddAbil.DC) + HUtil32.HiByte(Abil.DC)));
            WAbil.MC = HUtil32.MakeWord(HUtil32._MIN(255, HUtil32.LoByte(AddAbil.MC) + HUtil32.LoByte(Abil.MC)), HUtil32._MIN(255, HUtil32.HiByte(AddAbil.MC) + HUtil32.HiByte(Abil.MC)));
            WAbil.SC = HUtil32.MakeWord(HUtil32._MIN(255, HUtil32.LoByte(AddAbil.SC) + HUtil32.LoByte(Abil.SC)), HUtil32._MIN(255, HUtil32.HiByte(AddAbil.SC) + HUtil32.HiByte(Abil.SC)));
            if (StatusArr[Grobal2.STATE_DEFENCEUP] > 0)
            {
                WAbil.AC = HUtil32.MakeWord(HUtil32.LoByte(WAbil.AC), HUtil32._MIN(255, HUtil32.HiByte(WAbil.AC) + (Abil.Level / 7) + StatusArrTick[Grobal2.STATE_DEFENCEUP]));
            }
            if (StatusArr[Grobal2.STATE_MAGDEFENCEUP] > 0)
            {
                WAbil.MAC = HUtil32.MakeWord(HUtil32.LoByte(WAbil.MAC), HUtil32._MIN(255, HUtil32.HiByte(WAbil.MAC) + (Abil.Level / 7) + StatusArrTick[Grobal2.STATE_MAGDEFENCEUP]));
            }
            if (ExtraAbil[AbilConst.EABIL_DCUP] > 0)
            {
                WAbil.DC = HUtil32.MakeWord(HUtil32.LoByte(WAbil.DC), HUtil32.HiByte(WAbil.DC) + ExtraAbil[AbilConst.EABIL_DCUP]);
            }
            if (ExtraAbil[AbilConst.EABIL_MCUP] > 0)
            {
                WAbil.MC = HUtil32.MakeWord(HUtil32.LoByte(WAbil.MC), HUtil32.HiByte(WAbil.MC) + ExtraAbil[AbilConst.EABIL_MCUP]);
            }
            if (ExtraAbil[AbilConst.EABIL_SCUP] > 0)
            {
                WAbil.SC = HUtil32.MakeWord(HUtil32.LoByte(WAbil.SC), HUtil32.HiByte(WAbil.SC) + ExtraAbil[AbilConst.EABIL_SCUP]);
            }
            if (ExtraAbil[AbilConst.EABIL_HITSPEEDUP] > 0)
            {
                HitSpeed = (ushort)(HitSpeed + ExtraAbil[AbilConst.EABIL_HITSPEEDUP]);
            }
            if (ExtraAbil[AbilConst.EABIL_HPUP] > 0)
            {
                WAbil.MaxHP = (ushort)(WAbil.MaxHP + ExtraAbil[AbilConst.EABIL_HPUP]);
            }
            if (ExtraAbil[AbilConst.EABIL_MPUP] > 0)
            {
                WAbil.MaxMP = (ushort)(WAbil.MaxMP + ExtraAbil[AbilConst.EABIL_MPUP]);
            }
            if (ExtraAbil[AbilConst.EABIL_PWRRATE] > 0)
            {
                WAbil.DC = HUtil32.MakeWord((HUtil32.LoByte(WAbil.DC) * ExtraAbil[AbilConst.EABIL_PWRRATE]) / 100, (HUtil32.HiByte(WAbil.DC) * ExtraAbil[AbilConst.EABIL_PWRRATE]) / 100);
                WAbil.MC = HUtil32.MakeWord((HUtil32.LoByte(WAbil.MC) * ExtraAbil[AbilConst.EABIL_PWRRATE]) / 100, (HUtil32.HiByte(WAbil.MC) * ExtraAbil[AbilConst.EABIL_PWRRATE]) / 100);
                WAbil.SC = HUtil32.MakeWord((HUtil32.LoByte(WAbil.SC) * ExtraAbil[AbilConst.EABIL_PWRRATE]) / 100, (HUtil32.HiByte(WAbil.SC) * ExtraAbil[AbilConst.EABIL_PWRRATE]) / 100);
            }
            if (FlameRing)
            {
                AddItemSkill(M2Share.AM_FIREBALL);
            }
            else
            {
                DelItemSkill(M2Share.AM_FIREBALL);
            }
            if (RecoveryRing)
            {
                AddItemSkill(M2Share.AM_HEALING);
            }
            else
            {
                DelItemSkill(M2Share.AM_HEALING);
            }
            if (MuscleRing)
            {
                WAbil.MaxWeight = (ushort)(WAbil.MaxWeight * 2);
                WAbil.MaxWearWeight = (byte)HUtil32._MIN(255, WAbil.MaxWearWeight * 2);
                if ((WAbil.MaxHandWeight * 2 > 255))
                {
                    WAbil.MaxHandWeight = 255;
                }
                else
                {
                    WAbil.MaxHandWeight = (byte)(WAbil.MaxHandWeight * 2);
                }
            }
            if (MoXieSuite > 0)
            {
                if (MoXieSuite >= WAbil.MaxMP)
                {
                    MoXieSuite = WAbil.MaxMP - 1;
                }
                WAbil.MaxMP = (ushort)(WAbil.MaxMP - MoXieSuite);
                WAbil.MaxHP = (ushort)(WAbil.MaxHP + MoXieSuite);
                if ((Race == Grobal2.RC_PLAYOBJECT) && (WAbil.HP > WAbil.MaxHP))
                {
                    WAbil.HP = WAbil.MaxHP;
                }
            }
            if ((Race == Grobal2.RC_PLAYOBJECT) && (WAbil.HP > WAbil.MaxHP) && (!mh_necklace && !mh_bracelet && !mh_ring))
            {
                WAbil.HP = WAbil.MaxHP;
            }
            if ((Race == Grobal2.RC_PLAYOBJECT) && (WAbil.MP > WAbil.MaxMP))
            {
                WAbil.MP = WAbil.MaxMP;
            }
            if (Race == Grobal2.RC_PLAYOBJECT)
            {
                var fastmoveflag = false;
                if ((UseItems[Grobal2.U_BOOTS].Dura > 0) && (UseItems[Grobal2.U_BOOTS].Index == M2Share.INDEX_MIRBOOTS))
                {
                    fastmoveflag = true;
                }
                if (fastmoveflag)
                {
                    StatusArr[Grobal2.STATE_FASTMOVE] = 60000;
                }
                else
                {
                    StatusArr[Grobal2.STATE_FASTMOVE] = 0;
                }
                //if ((Abil.Level >= EfftypeConst.EFFECTIVE_HIGHLEVEL))
                //{
                //    if (BoHighLevelEffect)
                //    {
                //        StatusArr[Grobal2.STATE_50LEVELEFFECT] = 60000;
                //    }
                //    else
                //    {
                //        StatusArr[Grobal2.STATE_50LEVELEFFECT] = 0;
                //    }
                //}
                //else
                //{
                //    StatusArr[Grobal2.STATE_50LEVELEFFECT] = 0;
                //}
                CharStatus = GetCharStatus();
                StatusChanged();
            }
            if (Race == Grobal2.RC_PLAYOBJECT)
            {
                SendUpdateMsg(this, Grobal2.RM_CHARSTATUSCHANGED, HitSpeed, CharStatus, 0, 0, "");
            }
            if (Race >= Grobal2.RC_ANIMAL)
            {
                ApplySlaveLevelAbilitys();
            }
        }

        private void ApplyItemParameters(UserItem uitem, StdItem stdItem, ref AddAbility aabil)
        {
            var item = M2Share.WorldEngine.GetStdItem(uitem.Index);
            if (item != null)
            {
                var clientItem = new ClientItem();
                item.GetUpgradeStdItem(uitem, ref clientItem);
                ApplyItemParametersByJob(uitem, ref clientItem);
                switch (item.StdMode)
                {
                    case 5:
                    case 6:
                        aabil.HIT = (ushort)(aabil.HIT + HUtil32.HiByte(clientItem.Item.AC));
                        aabil.HitSpeed = (ushort)(aabil.HitSpeed + stdItem.RealAttackSpeed(HUtil32.HiByte(clientItem.Item.MAC)));
                        aabil.Luck = (byte)(aabil.Luck + HUtil32.LoByte(clientItem.Item.AC));
                        aabil.UnLuck = (byte)(aabil.UnLuck + HUtil32.LoByte(clientItem.Item.MAC));
                        aabil.Slowdown = (byte)(aabil.Slowdown + clientItem.Item.Slowdown);
                        aabil.Poison = (byte)(aabil.Poison + clientItem.Item.Tox);
                        if (clientItem.Item.SpecialPwr >= 1 && clientItem.Item.SpecialPwr <= 10)
                        {
                            aabil.WeaponStrong = (byte)clientItem.Item.SpecialPwr;
                        }
                        break;
                    case 10:
                    case 11:
                        aabil.AC = HUtil32.MakeWord(HUtil32.LoByte(aabil.AC) + HUtil32.LoByte(clientItem.Item.AC), HUtil32.HiByte(aabil.AC) + HUtil32.HiByte(clientItem.Item.AC));
                        aabil.MAC = HUtil32.MakeWord(HUtil32.LoByte(aabil.MAC) + HUtil32.LoByte(clientItem.Item.MAC), HUtil32.HiByte(aabil.MAC) + HUtil32.HiByte(clientItem.Item.MAC));
                        aabil.SPEED = (byte)(aabil.SPEED + clientItem.Item.Agility);
                        aabil.AntiMagic = (byte)(aabil.AntiMagic + clientItem.Item.MgAvoid);
                        aabil.AntiPoison = (byte)(aabil.AntiPoison + clientItem.Item.ToxAvoid);
                        aabil.HP = (byte)(aabil.HP + clientItem.Item.HpAdd);
                        aabil.MP = (byte)(aabil.MP + clientItem.Item.MpAdd);
                        if (clientItem.Item.EffType1 > 0)
                        {
                            switch (clientItem.Item.EffType1)
                            {
                                case EfftypeConst.EFFTYPE_HP_MP_ADD:
                                    if ((aabil.HealthRecover + clientItem.Item.EffRate1 > 65000))
                                    {
                                        aabil.HealthRecover = 65000;
                                    }
                                    else
                                    {
                                        aabil.HealthRecover = (ushort)(aabil.HealthRecover + clientItem.Item.EffRate1);
                                    }
                                    if ((aabil.SpellRecover + clientItem.Item.EffValue1 > 65000))
                                    {
                                        aabil.SpellRecover = 65000;
                                    }
                                    else
                                    {
                                        aabil.SpellRecover = (ushort)(aabil.SpellRecover + clientItem.Item.EffValue1);
                                    }
                                    break;
                            }
                        }
                        if (clientItem.Item.EffType2 > 0)
                        {
                            switch (clientItem.Item.EffType2)
                            {
                                case EfftypeConst.EFFTYPE_HP_MP_ADD:
                                    if ((aabil.HealthRecover + clientItem.Item.EffRate2 > 65000))
                                    {
                                        aabil.HealthRecover = 65000;
                                    }
                                    else
                                    {
                                        aabil.HealthRecover = (ushort)(aabil.HealthRecover + clientItem.Item.EffRate2);
                                    }
                                    if ((aabil.SpellRecover + clientItem.Item.EffValue2 > 65000))
                                    {
                                        aabil.SpellRecover = 65000;
                                    }
                                    else
                                    {
                                        aabil.SpellRecover = (ushort)(aabil.SpellRecover + clientItem.Item.EffValue2);
                                    }
                                    break;
                            }
                        }
                        if (clientItem.Item.EffType1 == EfftypeConst.EFFTYPE_LUCK_ADD)
                        {
                            if (aabil.Luck + clientItem.Item.EffValue1 > 255)
                            {
                                aabil.Luck = 255;
                            }
                            else
                            {
                                aabil.Luck = (byte)(aabil.Luck + clientItem.Item.EffValue1);
                            }
                        }
                        else if (clientItem.Item.EffType2 == EfftypeConst.EFFTYPE_LUCK_ADD)
                        {
                            if (aabil.Luck + clientItem.Item.EffValue2 > 255)
                            {
                                aabil.Luck = 255;
                            }
                            else
                            {
                                aabil.Luck = (byte)(aabil.Luck + clientItem.Item.EffValue2);
                            }
                        }
                        break;
                    case 15:
                        aabil.AC = HUtil32.MakeWord(HUtil32.LoByte(aabil.AC) + HUtil32.LoByte(clientItem.Item.AC), HUtil32.HiByte(aabil.AC) + HUtil32.HiByte(clientItem.Item.AC));
                        aabil.MAC = HUtil32.MakeWord(HUtil32.LoByte(aabil.MAC) + HUtil32.LoByte(clientItem.Item.MAC), HUtil32.HiByte(aabil.MAC) + HUtil32.HiByte(clientItem.Item.MAC));
                        aabil.HIT = (byte)(aabil.HIT + clientItem.Item.Accurate);
                        aabil.AntiMagic = (byte)(aabil.AntiMagic + clientItem.Item.MgAvoid);
                        aabil.AntiPoison = (byte)((byte)(aabil.AntiPoison + clientItem.Item.ToxAvoid));
                        break;
                    case 19:
                        aabil.AntiMagic = (ushort)(aabil.AntiMagic + HUtil32.HiByte(clientItem.Item.AC));
                        aabil.UnLuck = (byte)(aabil.UnLuck + HUtil32.LoByte(clientItem.Item.MAC));
                        aabil.Luck = (byte)(aabil.Luck + HUtil32.HiByte(clientItem.Item.MAC));
                        aabil.HitSpeed = (byte)(aabil.HitSpeed + clientItem.Item.AtkSpd);
                        aabil.HIT = (byte)(aabil.HIT + clientItem.Item.Accurate);
                        aabil.Slowdown = (byte)(aabil.Slowdown + clientItem.Item.Slowdown);
                        aabil.Poison = (byte)(aabil.Poison + clientItem.Item.Tox);
                        break;
                    case 20:
                        aabil.HIT = (byte)(aabil.HIT + HUtil32.HiByte(clientItem.Item.AC));
                        aabil.SPEED = (byte)(aabil.SPEED + HUtil32.HiByte(clientItem.Item.MAC));
                        aabil.HitSpeed = (byte)(aabil.HitSpeed + clientItem.Item.AtkSpd);
                        aabil.AntiMagic = (byte)(aabil.AntiMagic + clientItem.Item.MgAvoid);
                        aabil.Slowdown = (byte)(aabil.Slowdown + clientItem.Item.Slowdown);
                        aabil.Poison = (byte)(aabil.Poison + clientItem.Item.Tox);
                        break;
                    case 21:
                        aabil.HealthRecover = (byte)(aabil.HealthRecover + HUtil32.HiByte(clientItem.Item.AC));
                        aabil.SpellRecover = (byte)(aabil.SpellRecover + HUtil32.HiByte(clientItem.Item.MAC));
                        aabil.HitSpeed = (byte)(aabil.HitSpeed + HUtil32.LoByte(clientItem.Item.AC));
                        aabil.HitSpeed = (byte)(aabil.HitSpeed - HUtil32.LoByte(clientItem.Item.MAC));
                        aabil.HitSpeed = (byte)(aabil.HitSpeed + clientItem.Item.AtkSpd);
                        aabil.HIT = (byte)(aabil.HIT + clientItem.Item.Accurate);
                        aabil.AntiMagic = (byte)(aabil.AntiMagic + clientItem.Item.MgAvoid);
                        aabil.Slowdown = (byte)(aabil.Slowdown + clientItem.Item.Slowdown);
                        aabil.Poison = (byte)(aabil.Poison + clientItem.Item.Tox);
                        break;
                    case 22:
                        aabil.AC = HUtil32.MakeWord(HUtil32.LoByte(aabil.AC) + HUtil32.LoByte(clientItem.Item.AC), HUtil32.HiByte(aabil.AC) + HUtil32.HiByte(clientItem.Item.AC));
                        aabil.MAC = HUtil32.MakeWord(HUtil32.LoByte(aabil.MAC) + HUtil32.LoByte(clientItem.Item.MAC), HUtil32.HiByte(aabil.MAC) + HUtil32.HiByte(clientItem.Item.MAC));
                        aabil.HitSpeed = (byte)(aabil.HitSpeed + clientItem.Item.AtkSpd);
                        aabil.Slowdown = (byte)(aabil.Slowdown + clientItem.Item.Slowdown);
                        aabil.Poison = (byte)(aabil.Poison + clientItem.Item.Tox);
                        aabil.HIT = (byte)(aabil.HIT + clientItem.Item.Accurate);
                        aabil.HP = (byte)(aabil.HP + clientItem.Item.HpAdd);
                        break;
                    case 23:
                        aabil.AntiPoison = (byte)(aabil.AntiPoison + HUtil32.HiByte(clientItem.Item.AC));
                        aabil.PoisonRecover = (byte)(aabil.PoisonRecover + HUtil32.HiByte(clientItem.Item.MAC));
                        aabil.HitSpeed = (byte)(aabil.HitSpeed + HUtil32.LoByte(clientItem.Item.AC));
                        aabil.HitSpeed = (byte)(aabil.HitSpeed - HUtil32.LoByte(clientItem.Item.MAC));
                        aabil.HitSpeed = (byte)(aabil.HitSpeed + clientItem.Item.AtkSpd);
                        aabil.Slowdown = (byte)(aabil.Slowdown + clientItem.Item.Slowdown);
                        aabil.Poison = (byte)(aabil.Poison + clientItem.Item.Tox);
                        break;
                    case 24:
                    case 26:
                        if (clientItem.Item.SpecialPwr >= 1 && clientItem.Item.SpecialPwr <= 10)
                        {
                            aabil.WeaponStrong = (byte)clientItem.Item.SpecialPwr;
                        }
                        switch (item.StdMode)
                        {
                            case 24:
                                aabil.HIT = (byte)(aabil.HIT + HUtil32.HiByte(clientItem.Item.AC));
                                aabil.SPEED = (byte)(aabil.SPEED + HUtil32.HiByte(clientItem.Item.MAC));
                                break;
                            case 26:
                                aabil.AC = HUtil32.MakeWord(HUtil32.LoByte(aabil.AC) + HUtil32.LoByte(clientItem.Item.AC), HUtil32.HiByte(aabil.AC) + HUtil32.HiByte(clientItem.Item.AC));
                                aabil.MAC = HUtil32.MakeWord(HUtil32.LoByte(aabil.MAC) + HUtil32.LoByte(clientItem.Item.MAC), HUtil32.HiByte(aabil.MAC) + HUtil32.HiByte(clientItem.Item.MAC));
                                aabil.HIT = (byte)(aabil.HIT + clientItem.Item.Accurate);
                                aabil.SPEED = (byte)(aabil.SPEED + clientItem.Item.Agility);
                                aabil.MP = (byte)(aabil.MP + clientItem.Item.MpAdd);
                                break;
                        }
                        break;
                    case 52:
                        aabil.AC = HUtil32.MakeWord(HUtil32.LoByte(aabil.AC) + HUtil32.LoByte(clientItem.Item.AC), HUtil32.HiByte(aabil.AC) + HUtil32.HiByte(clientItem.Item.AC));
                        aabil.MAC = HUtil32.MakeWord(HUtil32.LoByte(aabil.MAC) + HUtil32.LoByte(clientItem.Item.MAC), HUtil32.HiByte(aabil.MAC) + HUtil32.HiByte(clientItem.Item.MAC));
                        aabil.SPEED = (byte)(aabil.SPEED + clientItem.Item.Agility);
                        break;
                    case 54:
                        aabil.AC = HUtil32.MakeWord(HUtil32.LoByte(aabil.AC) + HUtil32.LoByte(clientItem.Item.AC), HUtil32.HiByte(aabil.AC) + HUtil32.HiByte(clientItem.Item.AC));
                        aabil.MAC = HUtil32.MakeWord(HUtil32.LoByte(aabil.MAC) + HUtil32.LoByte(clientItem.Item.MAC), HUtil32.HiByte(aabil.MAC) + HUtil32.HiByte(clientItem.Item.MAC));
                        aabil.HIT = (byte)(aabil.HIT + clientItem.Item.Accurate);
                        aabil.SPEED = (byte)(aabil.SPEED + clientItem.Item.Agility);
                        aabil.AntiPoison = (byte)(aabil.AntiPoison + clientItem.Item.ToxAvoid);
                        break;
                    case 53:
                        aabil.HP = (byte)(aabil.HP + clientItem.Item.HpAdd);
                        aabil.MP = (byte)(aabil.MP + clientItem.Item.MpAdd);
                        break;
                    default:
                        aabil.AC = HUtil32.MakeWord(HUtil32.LoByte(aabil.AC) + HUtil32.LoByte(clientItem.Item.AC), HUtil32.HiByte(aabil.AC) + HUtil32.HiByte(clientItem.Item.AC));
                        aabil.MAC = HUtil32.MakeWord(HUtil32.LoByte(aabil.MAC) + HUtil32.LoByte(clientItem.Item.MAC), HUtil32.HiByte(aabil.MAC) + HUtil32.HiByte(clientItem.Item.MAC));
                        break;
                }
                aabil.DC = HUtil32.MakeWord(HUtil32.LoByte(aabil.DC) + HUtil32.LoByte(clientItem.Item.DC), HUtil32._MIN(255, HUtil32.HiByte(aabil.DC) + HUtil32.HiByte(clientItem.Item.DC)));
                aabil.MC = HUtil32.MakeWord(HUtil32.LoByte(aabil.MC) + HUtil32.LoByte(clientItem.Item.MC), HUtil32._MIN(255, HUtil32.HiByte(aabil.MC) + HUtil32.HiByte(clientItem.Item.MC)));
                aabil.SC = HUtil32.MakeWord(HUtil32.LoByte(aabil.SC) + HUtil32.LoByte(clientItem.Item.SC), HUtil32._MIN(255, HUtil32.HiByte(aabil.SC) + HUtil32.HiByte(clientItem.Item.SC)));
            }
        }

        private void ApplyItemParametersEx(UserItem uitem, ref Ability AWabil)
        {
            StdItem item = M2Share.WorldEngine.GetStdItem(uitem.Index);
            if (item != null)
            {
                var clientItem = new ClientItem();
                item.GetUpgradeStdItem(uitem, ref clientItem);
                switch (item.StdMode)
                {
                    case 52:
                        if (clientItem.Item.EffType1 > 0)
                        {
                            switch (clientItem.Item.EffType1)
                            {
                                case EfftypeConst.EFFTYPE_TWOHAND_WEHIGHT_ADD:
                                    if ((AWabil.MaxHandWeight + clientItem.Item.EffValue1 > 255))
                                    {
                                        AWabil.MaxHandWeight = 255;
                                    }
                                    else
                                    {
                                        AWabil.MaxHandWeight = (byte)(AWabil.MaxHandWeight + clientItem.Item.EffValue1);
                                    }
                                    break;
                                case EfftypeConst.EFFTYPE_EQUIP_WHEIGHT_ADD:
                                    if ((AWabil.MaxWearWeight + clientItem.Item.EffValue1 > 255))
                                    {
                                        AWabil.MaxWearWeight = 255;
                                    }
                                    else
                                    {
                                        AWabil.MaxWearWeight = (byte)(AWabil.MaxWearWeight + clientItem.Item.EffValue1);
                                    }
                                    break;
                            }
                        }
                        if (clientItem.Item.EffType2 > 0)
                        {
                            switch (clientItem.Item.EffType2)
                            {
                                case EfftypeConst.EFFTYPE_TWOHAND_WEHIGHT_ADD:
                                    if ((AWabil.MaxHandWeight + clientItem.Item.EffValue2 > 255))
                                    {
                                        AWabil.MaxHandWeight = 255;
                                    }
                                    else
                                    {
                                        AWabil.MaxHandWeight = (byte)(AWabil.MaxHandWeight + clientItem.Item.EffValue2);
                                    }
                                    break;
                                case EfftypeConst.EFFTYPE_EQUIP_WHEIGHT_ADD:
                                    if ((AWabil.MaxWearWeight + clientItem.Item.EffValue2 > 255))
                                    {
                                        AWabil.MaxWearWeight = 255;
                                    }
                                    else
                                    {
                                        AWabil.MaxWearWeight = (byte)(AWabil.MaxWearWeight + clientItem.Item.EffValue2);
                                    }
                                    break;
                            }
                        }
                        break;
                    case 54:
                        if (clientItem.Item.EffType1 > 0)
                        {
                            switch (clientItem.Item.EffType1)
                            {
                                case EfftypeConst.EFFTYPE_BAG_WHIGHT_ADD:
                                    if ((AWabil.MaxWeight + clientItem.Item.EffValue1 > 65000))
                                    {
                                        AWabil.MaxWeight = 65000;
                                    }
                                    else
                                    {
                                        AWabil.MaxWeight = (byte)(AWabil.MaxWeight + clientItem.Item.EffValue1);
                                    }
                                    break;
                            }
                        }
                        if (clientItem.Item.EffType2 > 0)
                        {
                            switch (clientItem.Item.EffType2)
                            {
                                case EfftypeConst.EFFTYPE_BAG_WHIGHT_ADD:
                                    if ((AWabil.MaxWeight + clientItem.Item.EffValue2 > 65000))
                                    {
                                        AWabil.MaxWeight = 65000;
                                    }
                                    else
                                    {
                                        AWabil.MaxWeight = (byte)(AWabil.MaxWeight + clientItem.Item.EffValue2);
                                    }
                                    break;
                            }
                        }
                        break;
                }
            }
        }

        protected void ChangeItemByJob(ref ClientItem citem, int lv)
        {
            if ((citem.Item.StdMode == 22) && (citem.Item.Shape == DragonConst.DRAGON_RING_SHAPE))
            {
                switch (Job)
                {
                    case PlayJob.Warrior:
                        citem.Item.DC = HUtil32.MakeWord(HUtil32.LoByte(citem.Item.DC), HUtil32._MIN(255, HUtil32.HiByte(citem.Item.DC) + 4));
                        citem.Item.MC = 0;
                        citem.Item.SC = 0;
                        break;
                    case PlayJob.Taoist:
                        citem.Item.DC = 0;
                        citem.Item.SC = 0;
                        break;
                    case PlayJob.Wizard:
                        citem.Item.MC = 0;
                        break;
                }
            }
            else if ((citem.Item.StdMode == 26) && (citem.Item.Shape == DragonConst.DRAGON_BRACELET_SHAPE))
            {
                switch (Job)
                {
                    case PlayJob.Warrior:
                        citem.Item.DC = HUtil32.MakeWord(HUtil32.LoByte(citem.Item.DC) + 1, HUtil32._MIN(255, HUtil32.HiByte(citem.Item.DC) + 2));
                        citem.Item.MC = 0;
                        citem.Item.SC = 0;
                        citem.Item.AC = HUtil32.MakeWord(HUtil32.LoByte(citem.Item.AC), HUtil32._MIN(255, HUtil32.HiByte(citem.Item.AC) + 1));
                        break;
                    case PlayJob.Taoist:
                        citem.Item.DC = 0;
                        citem.Item.SC = 0;
                        citem.Item.AC = HUtil32.MakeWord(HUtil32.LoByte(citem.Item.AC), HUtil32._MIN(255, HUtil32.HiByte(citem.Item.AC) + 1));
                        break;
                    case PlayJob.Wizard:
                        citem.Item.MC = 0;
                        break;
                }
            }
            else if ((citem.Item.StdMode == 19) && (citem.Item.Shape == DragonConst.DRAGON_NECKLACE_SHAPE))
            {
                switch (Job)
                {
                    case PlayJob.Warrior:
                        citem.Item.MC = 0;
                        citem.Item.SC = 0;
                        break;
                    case PlayJob.Taoist:
                        citem.Item.DC = 0;
                        citem.Item.SC = 0;
                        break;
                    case PlayJob.Wizard:
                        citem.Item.DC = 0;
                        citem.Item.MC = 0;
                        break;
                }
            }
            else if (((citem.Item.StdMode == 10) || (citem.Item.StdMode == 11)) && (citem.Item.Shape == DragonConst.DRAGON_DRESS_SHAPE))
            {
                switch (Job)
                {
                    case PlayJob.Warrior:
                        citem.Item.MC = 0;
                        citem.Item.SC = 0;
                        break;
                    case PlayJob.Taoist:
                        citem.Item.DC = 0;
                        citem.Item.SC = 0;
                        break;
                    case PlayJob.Wizard:
                        citem.Item.DC = 0;
                        citem.Item.MC = 0;
                        break;
                }
            }
            else if ((citem.Item.StdMode == 15) && (citem.Item.Shape == DragonConst.DRAGON_HELMET_SHAPE))
            {
                switch (Job)
                {
                    case PlayJob.Warrior:
                        citem.Item.MC = 0;
                        citem.Item.SC = 0;
                        break;
                    case PlayJob.Taoist:
                        citem.Item.DC = 0;
                        citem.Item.SC = 0;
                        break;
                    case PlayJob.Wizard:
                        citem.Item.DC = 0;
                        citem.Item.MC = 0;
                        break;
                }
            }
            else if (((citem.Item.StdMode == 5) || (citem.Item.StdMode == 6)) && (citem.Item.Shape == DragonConst.DRAGON_WEAPON_SHAPE))
            {
                switch (Job)
                {
                    case PlayJob.Warrior:
                        citem.Item.DC = HUtil32.MakeWord(HUtil32.LoByte(citem.Item.DC) + 1, HUtil32._MIN(255, HUtil32.HiByte(citem.Item.DC) + 28));
                        citem.Item.MC = 0;
                        citem.Item.SC = 0;
                        citem.Item.AC = HUtil32.MakeWord(HUtil32.LoByte(citem.Item.AC) - 2, HUtil32.HiByte(citem.Item.AC));
                        break;
                    case PlayJob.Taoist:
                        citem.Item.SC = 0;
                        if (HUtil32.HiByte(citem.Item.MAC) > 12)
                        {
                            citem.Item.MAC = HUtil32.MakeWord(HUtil32.LoByte(citem.Item.MAC), HUtil32.HiByte(citem.Item.MAC) - 12);
                        }
                        else
                        {
                            citem.Item.MAC = HUtil32.MakeWord(HUtil32.LoByte(citem.Item.MAC), 0);
                        }
                        break;
                    case PlayJob.Wizard:
                        citem.Item.DC = HUtil32.MakeWord(HUtil32.LoByte(citem.Item.DC) + 2, HUtil32._MIN(255, HUtil32.HiByte(citem.Item.DC) + 10));
                        citem.Item.MC = 0;
                        citem.Item.AC = HUtil32.MakeWord(HUtil32.LoByte(citem.Item.AC) - 2, HUtil32.HiByte(citem.Item.AC));
                        break;
                }
            }
            else if ((citem.Item.StdMode == 53))
            {
                if ((citem.Item.Shape == ShapeConst.LOLLIPOP_SHAPE))
                {
                    switch (Job)
                    {
                        case PlayJob.Warrior:
                            citem.Item.DC = HUtil32.MakeWord(HUtil32.LoByte(citem.Item.DC), HUtil32._MIN(255, HUtil32.HiByte(citem.Item.DC) + 2));
                            citem.Item.MC = 0;
                            citem.Item.SC = 0;
                            break;
                        case PlayJob.Taoist:
                            citem.Item.DC = 0;
                            citem.Item.MC = HUtil32.MakeWord(HUtil32.LoByte(citem.Item.MC), HUtil32._MIN(255, HUtil32.HiByte(citem.Item.MC) + 2));
                            citem.Item.SC = 0;
                            break;
                        case PlayJob.Wizard:
                            citem.Item.DC = 0;
                            citem.Item.MC = 0;
                            citem.Item.SC = HUtil32.MakeWord(HUtil32.LoByte(citem.Item.SC), HUtil32._MIN(255, HUtil32.HiByte(citem.Item.SC) + 2));
                            break;
                    }
                }
                else if ((citem.Item.Shape == ShapeConst.GOLDMEDAL_SHAPE) || (citem.Item.Shape == ShapeConst.SILVERMEDAL_SHAPE) || (citem.Item.Shape == ShapeConst.BRONZEMEDAL_SHAPE))
                {
                    switch (Job)
                    {
                        case PlayJob.Warrior:
                            citem.Item.DC = HUtil32.MakeWord(HUtil32.LoByte(citem.Item.DC), HUtil32._MIN(255, HUtil32.HiByte(citem.Item.DC)));
                            citem.Item.MC = 0;
                            citem.Item.SC = 0;
                            break;
                        case PlayJob.Taoist:
                            citem.Item.DC = 0;
                            citem.Item.MC = HUtil32.MakeWord(HUtil32.LoByte(citem.Item.MC), HUtil32._MIN(255, HUtil32.HiByte(citem.Item.MC)));
                            citem.Item.SC = 0;
                            break;
                        case PlayJob.Wizard:
                            citem.Item.DC = 0;
                            citem.Item.MC = 0;
                            citem.Item.SC = HUtil32.MakeWord(HUtil32.LoByte(citem.Item.SC), HUtil32._MIN(255, HUtil32.HiByte(citem.Item.SC)));
                            break;
                    }
                }
            }
        }

        private void ApplyItemParametersByJob(UserItem uitem, ref ClientItem std)
        {
            var item = M2Share.WorldEngine.GetStdItem(uitem.Index);
            if (item != null)
            {
                if ((item.StdMode == 22) && (item.Shape == DragonConst.DRAGON_RING_SHAPE))
                {
                    switch (Job)
                    {
                        case PlayJob.Warrior:
                            std.Item.DC = HUtil32.MakeWord(HUtil32.LoByte(std.Item.DC), HUtil32._MIN(255, HUtil32.HiByte(std.Item.DC) + 4));
                            std.Item.MC = 0;
                            std.Item.SC = 0;
                            break;
                        case PlayJob.Taoist:
                            std.Item.DC = 0;
                            std.Item.SC = 0;
                            break;
                        case PlayJob.Wizard:
                            std.Item.MC = 0;
                            break;
                    }
                }
                else if ((item.StdMode == 26) && (item.Shape == DragonConst.DRAGON_BRACELET_SHAPE))
                {
                    switch (Job)
                    {
                        case PlayJob.Warrior:
                            std.Item.DC = HUtil32.MakeWord(HUtil32.LoByte(std.Item.DC) + 1, HUtil32._MIN(255, HUtil32.HiByte(std.Item.DC) + 2));
                            std.Item.MC = 0;
                            std.Item.SC = 0;
                            std.Item.AC = HUtil32.MakeWord(HUtil32.LoByte(std.Item.AC), HUtil32._MIN(255, HUtil32.HiByte(std.Item.AC) + 1));
                            break;
                        case PlayJob.Taoist:
                            std.Item.DC = 0;
                            std.Item.SC = 0;
                            std.Item.AC = HUtil32.MakeWord(HUtil32.LoByte(std.Item.AC), HUtil32._MIN(255, HUtil32.HiByte(std.Item.AC) + 1));
                            break;
                        case PlayJob.Wizard:
                            std.Item.MC = 0;
                            break;
                    }
                }
                else if ((item.StdMode == 19) && (item.Shape == DragonConst.DRAGON_NECKLACE_SHAPE))
                {
                    switch (Job)
                    {
                        case PlayJob.Warrior:
                            std.Item.MC = 0;
                            std.Item.SC = 0;
                            break;
                        case PlayJob.Taoist:
                            std.Item.DC = 0;
                            std.Item.SC = 0;
                            break;
                        case PlayJob.Wizard:
                            std.Item.DC = 0;
                            std.Item.MC = 0;
                            break;
                    }
                }
                else if (((item.StdMode == 10) || (item.StdMode == 11)) && (item.Shape == DragonConst.DRAGON_DRESS_SHAPE))
                {
                    switch (Job)
                    {
                        case PlayJob.Warrior:
                            std.Item.MC = 0;
                            std.Item.SC = 0;
                            break;
                        case PlayJob.Taoist:
                            std.Item.DC = 0;
                            std.Item.SC = 0;
                            break;
                        case PlayJob.Wizard:
                            std.Item.DC = 0;
                            std.Item.MC = 0;
                            break;
                    }
                }
                else if ((item.StdMode == 15) && (item.Shape == DragonConst.DRAGON_HELMET_SHAPE))
                {
                    switch (Job)
                    {
                        case PlayJob.Warrior:
                            std.Item.MC = 0;
                            std.Item.SC = 0;
                            break;
                        case PlayJob.Taoist:
                            std.Item.DC = 0;
                            std.Item.SC = 0;
                            break;
                        case PlayJob.Wizard:
                            std.Item.DC = 0;
                            std.Item.MC = 0;
                            break;
                    }
                }
                else if (((item.StdMode == 5) || (item.StdMode == 6)) && (item.Shape == DragonConst.DRAGON_WEAPON_SHAPE))
                {
                    switch (Job)
                    {
                        case PlayJob.Warrior:
                            std.Item.DC = HUtil32.MakeWord(HUtil32.LoByte(std.Item.DC) + 1, HUtil32._MIN(255, HUtil32.HiByte(std.Item.DC) + 28));
                            std.Item.MC = 0;
                            std.Item.SC = 0;
                            std.Item.AC = HUtil32.MakeWord(HUtil32.LoByte(std.Item.AC) - 2, HUtil32.HiByte(std.Item.AC));
                            break;
                        case PlayJob.Taoist:
                            std.Item.SC = 0;
                            if (HUtil32.HiByte(std.Item.MAC) > 12)
                            {
                                std.Item.MAC = HUtil32.MakeWord(HUtil32.LoByte(std.Item.MAC), HUtil32.HiByte(std.Item.MAC) - 12);
                            }
                            else
                            {
                                std.Item.MAC = HUtil32.MakeWord(HUtil32.LoByte(std.Item.MAC), 0);
                            }
                            break;
                        case PlayJob.Wizard:
                            std.Item.DC = HUtil32.MakeWord(HUtil32.LoByte(std.Item.DC) + 2, HUtil32._MIN(255, HUtil32.HiByte(std.Item.DC) + 10));
                            std.Item.MC = 0;
                            std.Item.AC = HUtil32.MakeWord(HUtil32.LoByte(std.Item.AC) - 2, HUtil32.HiByte(std.Item.AC));
                            break;
                    }
                }
                else if ((item.StdMode == 53))
                {
                    if ((item.Shape == ShapeConst.LOLLIPOP_SHAPE))
                    {
                        switch (Job)
                        {
                            case PlayJob.Warrior:
                                std.Item.DC = HUtil32.MakeWord(HUtil32.LoByte(std.Item.DC), HUtil32._MIN(255, HUtil32.HiByte(std.Item.DC) + 2));
                                std.Item.MC = 0;
                                std.Item.SC = 0;
                                break;
                            case PlayJob.Taoist:
                                std.Item.DC = 0;
                                std.Item.MC = HUtil32.MakeWord(HUtil32.LoByte(std.Item.MC), HUtil32._MIN(255, HUtil32.HiByte(std.Item.MC) + 2));
                                std.Item.SC = 0;
                                break;
                            case PlayJob.Wizard:
                                std.Item.DC = 0;
                                std.Item.MC = 0;
                                std.Item.SC = HUtil32.MakeWord(HUtil32.LoByte(std.Item.SC), HUtil32._MIN(255, HUtil32.HiByte(std.Item.SC) + 2));
                                break;
                        }
                    }
                    else if ((std.Item.Shape == ShapeConst.GOLDMEDAL_SHAPE) || (std.Item.Shape == ShapeConst.SILVERMEDAL_SHAPE) || (std.Item.Shape == ShapeConst.BRONZEMEDAL_SHAPE))
                    {
                        switch (Job)
                        {
                            case PlayJob.Warrior:
                                std.Item.DC = HUtil32.MakeWord(HUtil32.LoByte(std.Item.DC), HUtil32._MIN(255, HUtil32.HiByte(std.Item.DC)));
                                std.Item.MC = 0;
                                std.Item.SC = 0;
                                break;
                            case PlayJob.Taoist:
                                std.Item.DC = 0;
                                std.Item.MC = HUtil32.MakeWord(HUtil32.LoByte(std.Item.MC), HUtil32._MIN(255, HUtil32.HiByte(std.Item.MC)));
                                std.Item.SC = 0;
                                break;
                            case PlayJob.Wizard:
                                std.Item.DC = 0;
                                std.Item.MC = 0;
                                std.Item.SC = HUtil32.MakeWord(HUtil32.LoByte(std.Item.SC), HUtil32._MIN(255, HUtil32.HiByte(std.Item.SC)));
                                break;
                        }
                    }
                }
            }
        }

        public void ApplySlaveLevelAbilitys()
        {
            //if ((Race == Grobal2.RC_ANGEL) || (Race == Grobal2.RC_CLONE))
            //{
            //    return;
            //}
            int chp = 0;
            if ((Race == Grobal2.RC_WHITESKELETON) || (Race == Grobal2.RC_ELFMON) || (Race == Grobal2.RC_ELFWARRIORMON))
            {
                WAbil.DC = HUtil32.MakeWord(HUtil32.LoByte(WAbil.DC), HUtil32.HiByte(Abil.DC));
                WAbil.DC = HUtil32.MakeWord(HUtil32.LoByte(WAbil.DC), (int)Math.Round(HUtil32.HiByte(WAbil.DC) + (3 * (0.3 + SlaveExpLevel * 0.1) * SlaveExpLevel)));
                chp = (ushort)(chp + Math.Round(Abil.MaxHP * (0.3 + SlaveExpLevel * 0.1)) * SlaveExpLevel);
                chp = Abil.MaxHP + chp;
                if (SlaveExpLevel > 0)
                {
                    WAbil.MaxHP = (ushort)chp;
                }
                else
                {
                    WAbil.MaxHP = Abil.MaxHP;
                }
                WAbil.DC = HUtil32.MakeWord(HUtil32.LoByte(WAbil.DC), HUtil32.HiByte(WAbil.DC) + ExtraAbil[AbilConst.EABIL_DCUP]);
            }
            else
            {
                if (Master != null)
                {
                    chp = Abil.MaxHP;
                    WAbil.DC = HUtil32.MakeWord(HUtil32.LoByte(WAbil.DC), HUtil32.HiByte(Abil.DC));
                    WAbil.DC = HUtil32.MakeWord(HUtil32.LoByte(WAbil.DC), (int)Math.Abs(HUtil32.HiByte(WAbil.DC) + (2 * SlaveExpLevel)));
                    chp = (ushort)(chp + Math.Round(Abil.MaxHP * 0.15) * SlaveExpLevel);
                    WAbil.MaxHP = (ushort)HUtil32._MIN(Math.Abs(Abil.MaxHP + 60 * SlaveExpLevel), chp);
                    WAbil.MAC = 0;
                }
            }
            //AccuracyPoint = 15;
        }

        private int GetMyLight_CheckLightValue()
        {
            StdItem ps;
            PlayObject hum;
            var CurrentLight = 0;
            if (Race == Grobal2.RC_PLAYOBJECT)
            {
                hum = ((this) as PlayObject);
                if (hum != null)
                {
                    if (true)//BoHighLevelEffect
                    {
                        if (Abil.Level >= EfftypeConst.EFFECTIVE_HIGHLEVEL)
                        {
                            CurrentLight = 1;
                        }
                    }
                }
            }
            for (var i = Grobal2.U_DRESS; i <= Grobal2.U_CHARM; i++)
            {
                if ((UseItems[i].Index > 0) && (UseItems[i].Dura > 0))
                {
                    ps = M2Share.WorldEngine.GetStdItem(UseItems[i].Index);
                    if (ps != null)
                    {
                        if (CurrentLight < ps.Light)
                        {
                            CurrentLight = ps.Light;
                        }
                    }
                }
            }
            var result = CurrentLight;
            return result;
        }

        public int GetMyLight()
        {
            return GetMyLight_CheckLightValue();
        }

    }
}