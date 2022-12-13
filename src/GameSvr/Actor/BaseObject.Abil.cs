using GameSvr.Items;
using GameSvr.Magic;
using GameSvr.Player;
using SystemModule;
using SystemModule.Consts;
using SystemModule.Data;
using SystemModule.Enums;
using SystemModule.Packets.ClientPackets;

namespace GameSvr.Actor
{
    public partial class BaseObject
    {
        public void RecalcLevelAbilitys()
        {
            int n;
            var nLevel = Abil.Level;
            switch (Job)
            {
                case PlayJob.Taoist:
                    Abil.MaxHP = (ushort)HUtil32._MIN(ushort.MaxValue, 14 + HUtil32.Round(((nLevel / M2Share.Config.nLevelValueOfTaosHP) + M2Share.Config.nLevelValueOfTaosHPRate) * nLevel));
                    Abil.MaxMP = (ushort)HUtil32._MIN(ushort.MaxValue, 13 + HUtil32.Round(nLevel / M2Share.Config.nLevelValueOfTaosMP * 2.2 * nLevel));
                    Abil.MaxWeight = (ushort)(50 + HUtil32.Round(nLevel / 4 * nLevel));
                    Abil.MaxWearWeight = (byte)HUtil32._MIN(byte.MaxValue, (15 + HUtil32.Round(nLevel / 50 * nLevel)));
                    if ((12 + HUtil32.Round(Abil.Level / 13 * Abil.Level)) > 255)
                    {
                        Abil.MaxHandWeight = byte.MaxValue;
                    }
                    else
                    {
                        Abil.MaxHandWeight = (byte)(12 + HUtil32.Round(nLevel / 42 * nLevel));
                    }
                    n = nLevel / 7;
                    Abil.DC = HUtil32.MakeWord((ushort)HUtil32._MAX(n - 1, 0), (ushort)HUtil32._MAX(1, n));
                    Abil.MC = 0;
                    Abil.SC = HUtil32.MakeWord((ushort)HUtil32._MAX(n - 1, 0), (ushort)HUtil32._MAX(1, n));
                    Abil.AC = 0;
                    n = HUtil32.Round(nLevel / 6);
                    Abil.MAC = HUtil32.MakeWord((ushort)(n / 2), (ushort)(n + 1));
                    break;
                case PlayJob.Wizard:
                    Abil.MaxHP = (ushort)HUtil32._MIN(ushort.MaxValue, 14 + HUtil32.Round(((nLevel / M2Share.Config.nLevelValueOfWizardHP) + M2Share.Config.nLevelValueOfWizardHPRate) * nLevel));
                    Abil.MaxMP = (ushort)HUtil32._MIN(ushort.MaxValue, 13 + HUtil32.Round(((nLevel / 5) + 2) * 2.2 * nLevel));
                    Abil.MaxWeight = (ushort)(50 + HUtil32.Round(nLevel / 5 * nLevel));
                    Abil.MaxWearWeight = (byte)HUtil32._MIN(byte.MaxValue, 15 + HUtil32.Round(nLevel / 100 * nLevel));
                    Abil.MaxHandWeight = (byte)(12 + HUtil32.Round(nLevel / 90 * nLevel));
                    n = nLevel / 7;
                    Abil.DC = HUtil32.MakeWord((ushort)HUtil32._MAX(n - 1, 0), (ushort)HUtil32._MAX(1, n));
                    Abil.MC = HUtil32.MakeWord((ushort)HUtil32._MAX(n - 1, 0), (ushort)HUtil32._MAX(1, n));
                    Abil.SC = 0;
                    Abil.AC = 0;
                    Abil.MAC = 0;
                    break;
                case PlayJob.Warrior:
                    Abil.MaxHP = (ushort)HUtil32._MIN(ushort.MaxValue, 14 + HUtil32.Round((nLevel / M2Share.Config.nLevelValueOfWarrHP + M2Share.Config.nLevelValueOfWarrHPRate + nLevel / 20) * nLevel));
                    Abil.MaxMP = (ushort)HUtil32._MIN(ushort.MaxValue, 11 + HUtil32.Round(nLevel * 3.5));
                    Abil.MaxWeight = (ushort)(50 + HUtil32.Round(nLevel / 3 * nLevel));
                    Abil.MaxWearWeight = (byte)HUtil32._MIN(byte.MaxValue, (15 + HUtil32.Round(nLevel / 20 * nLevel)));
                    Abil.MaxHandWeight = (byte)(12 + HUtil32.Round(nLevel / 13 * nLevel));
                    Abil.DC = HUtil32.MakeWord((ushort)HUtil32._MAX(nLevel / 5 - 1, 1), (ushort)HUtil32._MAX(1, nLevel / 5));
                    Abil.SC = 0;
                    Abil.MC = 0;
                    Abil.AC = HUtil32.MakeWord(0, (ushort)(nLevel / 7));
                    Abil.MAC = 0;
                    break;
                case PlayJob.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            if (Abil.HP > Abil.MaxHP)
            {
                Abil.HP = Abil.MaxHP;
            }
            if (Abil.MP > Abil.MaxMP)
            {
                Abil.MP = Abil.MaxMP;
            }
        }

        /// <summary>
        /// 计算攻击速度
        /// </summary>
        private void RecalcHitSpeed()
        {
            NakedAbility bonusTick = null;
            switch (Job)
            {
                case PlayJob.Warrior:
                    bonusTick = M2Share.Config.BonusAbilofWarr;
                    break;
                case PlayJob.Wizard:
                    bonusTick = M2Share.Config.BonusAbilofWizard;
                    break;
                case PlayJob.Taoist:
                    bonusTick = M2Share.Config.BonusAbilofTaos;
                    break;
            }

            HitPoint = (byte)(M2Share.DEFHIT + BonusAbil.Hit / bonusTick.Hit);
            switch (Job)
            {
                case PlayJob.Taoist:
                    SpeedPoint = (byte)(M2Share.DEFSPEED + BonusAbil.Speed / bonusTick.Speed + 3);
                    break;
                default:
                    SpeedPoint = (byte)(M2Share.DEFSPEED + BonusAbil.Speed / bonusTick.Speed);
                    break;
            }

            HitPlus = 0;
            HitDouble = 0;
            for (var i = 0; i < MagicList.Count; i++)
            {
                var userMagic = MagicList[i];
                MagicArr[userMagic.MagIdx] = userMagic;
                switch (userMagic.MagIdx)
                {
                    case MagicConst.SKILL_ONESWORD: // 基本剑法
                        if (userMagic.Level > 0)
                        {
                            HitPoint = (byte)(HitPoint + HUtil32.Round(9 / 3 * userMagic.Level));
                        }
                        break;
                    case MagicConst.SKILL_ILKWANG: // 精神力战法
                        if (userMagic.Level > 0)
                        {
                            HitPoint = (byte)(HitPoint + HUtil32.Round(8 / 3 * userMagic.Level));
                        }
                        break;
                    case MagicConst.SKILL_YEDO: // 攻杀剑法
                        if (userMagic.Level > 0)
                        {
                            HitPoint = (byte)(HitPoint + HUtil32.Round(3 / 3 * userMagic.Level));
                        }
                        HitPlus = (byte)(M2Share.DEFHIT + userMagic.Level);
                        AttackSkillCount = (byte)(7 - userMagic.Level);
                        AttackSkillPointCount = M2Share.RandomNumber.RandomByte(AttackSkillCount);
                        break;
                    case MagicConst.SKILL_FIRESWORD: // 烈火剑法
                        HitDouble = (byte)(4 + userMagic.Level * 4);
                        break;
                }
            }
        }

        /// <summary>
        /// 计算自身属性
        /// </summary>
        public virtual void RecalcAbilitys()
        {
            AddAbil = new AddAbility();
            var temp = WAbil;
            WAbil = (Ability)Abil.Clone();
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
            MoXieSuite = 0;
            SuckupEnemyHealthRate = 0;
            SuckupEnemyHealth = 0;
            if (Transparent && (StatusArr[PoisonState.STATE_TRANSPARENT] > 0))
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
                    StatusArr[PoisonState.STATE_TRANSPARENT] = 0;
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
            WAbil.AC = HUtil32.MakeWord((ushort)HUtil32._MIN(255, HUtil32.LoByte(AddAbil.AC) + HUtil32.LoByte(Abil.AC)), (ushort)HUtil32._MIN(255, HUtil32.HiByte(AddAbil.AC) + HUtil32.HiByte(Abil.AC)));
            WAbil.MAC = HUtil32.MakeWord((ushort)HUtil32._MIN(255, HUtil32.LoByte(AddAbil.MAC) + HUtil32.LoByte(Abil.MAC)), (ushort)HUtil32._MIN(255, HUtil32.HiByte(AddAbil.MAC) + HUtil32.HiByte(Abil.MAC)));
            WAbil.DC = HUtil32.MakeWord((ushort)HUtil32._MIN(255, HUtil32.LoByte(AddAbil.DC) + HUtil32.LoByte(Abil.DC)), (ushort)HUtil32._MIN(255, HUtil32.HiByte(AddAbil.DC) + HUtil32.HiByte(Abil.DC)));
            WAbil.MC = HUtil32.MakeWord((ushort)HUtil32._MIN(255, HUtil32.LoByte(AddAbil.MC) + HUtil32.LoByte(Abil.MC)), (ushort)HUtil32._MIN(255, HUtil32.HiByte(AddAbil.MC) + HUtil32.HiByte(Abil.MC)));
            WAbil.SC = HUtil32.MakeWord((ushort)HUtil32._MIN(255, HUtil32.LoByte(AddAbil.SC) + HUtil32.LoByte(Abil.SC)), (ushort)HUtil32._MIN(255, HUtil32.HiByte(AddAbil.SC) + HUtil32.HiByte(Abil.SC)));
            if (StatusArr[PoisonState.DEFENCEUP] > 0)
            {
                WAbil.AC = HUtil32.MakeWord(HUtil32.LoByte(WAbil.AC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(WAbil.AC) + (Abil.Level / 7) + StatusArrTick[PoisonState.DEFENCEUP]));
            }
            if (StatusArr[PoisonState.MAGDEFENCEUP] > 0)
            {
                WAbil.MAC = HUtil32.MakeWord(HUtil32.LoByte(WAbil.MAC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(WAbil.MAC) + (Abil.Level / 7) + StatusArrTick[PoisonState.MAGDEFENCEUP]));
            }
            if (MoXieSuite > 0)
            {
                if (MoXieSuite >= WAbil.MaxMP)
                {
                    MoXieSuite = WAbil.MaxMP - 1;
                }
                WAbil.MaxMP = (ushort)(WAbil.MaxMP - MoXieSuite);
                WAbil.MaxHP = (ushort)(WAbil.MaxHP + MoXieSuite);
                if ((Race == ActorRace.Play) && (WAbil.HP > WAbil.MaxHP))
                {
                    WAbil.HP = WAbil.MaxHP;
                }
            }
            if (Race >= ActorRace.Animal)
            {
                ApplySlaveLevelAbilitys();
            }
        }

        internal void ApplyItemParameters(UserItem uitem, StdItem stdItem, ref AddAbility aabil)
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
                        aabil.AC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(aabil.AC) + HUtil32.LoByte(clientItem.Item.AC)), (ushort)(HUtil32.HiByte(aabil.AC) + HUtil32.HiByte(clientItem.Item.AC)));
                        aabil.MAC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(aabil.MAC) + HUtil32.LoByte(clientItem.Item.MAC)), (ushort)(HUtil32.HiByte(aabil.MAC) + HUtil32.HiByte(clientItem.Item.MAC)));
                        aabil.SPEED = (ushort)(aabil.SPEED + clientItem.Item.Agility);
                        aabil.AntiMagic = (ushort)(aabil.AntiMagic + clientItem.Item.MgAvoid);
                        aabil.AntiPoison = (ushort)(aabil.AntiPoison + clientItem.Item.ToxAvoid);
                        aabil.HP = (ushort)(aabil.HP + clientItem.Item.HpAdd);
                        aabil.MP = (ushort)(aabil.MP + clientItem.Item.MpAdd);
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
                                aabil.Luck = byte.MaxValue;
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
                                aabil.Luck = byte.MaxValue;
                            }
                            else
                            {
                                aabil.Luck = (byte)(aabil.Luck + clientItem.Item.EffValue2);
                            }
                        }
                        break;
                    case 15:
                        aabil.AC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(aabil.AC) + HUtil32.LoByte(clientItem.Item.AC)), (ushort)(HUtil32.HiByte(aabil.AC) + HUtil32.HiByte(clientItem.Item.AC)));
                        aabil.MAC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(aabil.MAC) + HUtil32.LoByte(clientItem.Item.MAC)), (ushort)(HUtil32.HiByte(aabil.MAC) + HUtil32.HiByte(clientItem.Item.MAC)));
                        aabil.HIT = (ushort)(aabil.HIT + clientItem.Item.Accurate);
                        aabil.AntiMagic = (ushort)(aabil.AntiMagic + clientItem.Item.MgAvoid);
                        aabil.AntiPoison = (ushort)(aabil.AntiPoison + clientItem.Item.ToxAvoid);
                        break;
                    case 19:
                        aabil.AntiMagic = (ushort)(aabil.AntiMagic + HUtil32.HiByte(clientItem.Item.AC));
                        aabil.UnLuck = (byte)(aabil.UnLuck + HUtil32.LoByte(clientItem.Item.MAC));
                        aabil.Luck = (byte)(aabil.Luck + HUtil32.HiByte(clientItem.Item.MAC));
                        aabil.HitSpeed = (ushort)(aabil.HitSpeed + clientItem.Item.AtkSpd);
                        aabil.HIT = (ushort)(aabil.HIT + clientItem.Item.Accurate);
                        aabil.Slowdown = (byte)(aabil.Slowdown + clientItem.Item.Slowdown);
                        aabil.Poison = (byte)(aabil.Poison + clientItem.Item.Tox);
                        break;
                    case 20:
                        aabil.HIT = (ushort)(aabil.HIT + HUtil32.HiByte(clientItem.Item.AC));
                        aabil.SPEED = (ushort)(aabil.SPEED + HUtil32.HiByte(clientItem.Item.MAC));
                        aabil.HitSpeed = (ushort)(aabil.HitSpeed + clientItem.Item.AtkSpd);
                        aabil.AntiMagic = (ushort)(aabil.AntiMagic + clientItem.Item.MgAvoid);
                        aabil.Slowdown = (byte)(aabil.Slowdown + clientItem.Item.Slowdown);
                        aabil.Poison = (byte)(aabil.Poison + clientItem.Item.Tox);
                        break;
                    case 21:
                        aabil.HealthRecover = (ushort)(aabil.HealthRecover + HUtil32.HiByte(clientItem.Item.AC));
                        aabil.SpellRecover = (ushort)(aabil.SpellRecover + HUtil32.HiByte(clientItem.Item.MAC));
                        aabil.HitSpeed = (ushort)(aabil.HitSpeed + HUtil32.LoByte(clientItem.Item.AC));
                        aabil.HitSpeed = (ushort)(aabil.HitSpeed - HUtil32.LoByte(clientItem.Item.MAC));
                        aabil.HitSpeed = (ushort)(aabil.HitSpeed + clientItem.Item.AtkSpd);
                        aabil.HIT = (ushort)(aabil.HIT + clientItem.Item.Accurate);
                        aabil.AntiMagic = (ushort)(aabil.AntiMagic + clientItem.Item.MgAvoid);
                        aabil.Slowdown = (byte)(aabil.Slowdown + clientItem.Item.Slowdown);
                        aabil.Poison = (byte)(aabil.Poison + clientItem.Item.Tox);
                        break;
                    case 22:
                        aabil.AC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(aabil.AC) + HUtil32.LoByte(clientItem.Item.AC)), (ushort)(HUtil32.HiByte(aabil.AC) + HUtil32.HiByte(clientItem.Item.AC)));
                        aabil.MAC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(aabil.MAC) + HUtil32.LoByte(clientItem.Item.MAC)), (ushort)(HUtil32.HiByte(aabil.MAC) + HUtil32.HiByte(clientItem.Item.MAC)));
                        aabil.HitSpeed = (ushort)(aabil.HitSpeed + clientItem.Item.AtkSpd);
                        aabil.Slowdown = (byte)(aabil.Slowdown + clientItem.Item.Slowdown);
                        aabil.Poison = (byte)(aabil.Poison + clientItem.Item.Tox);
                        aabil.HIT = (ushort)(aabil.HIT + clientItem.Item.Accurate);
                        aabil.HP = (ushort)(aabil.HP + clientItem.Item.HpAdd);
                        break;
                    case 23:
                        aabil.AntiPoison = (ushort)(aabil.AntiPoison + HUtil32.HiByte(clientItem.Item.AC));
                        aabil.PoisonRecover = (ushort)(aabil.PoisonRecover + HUtil32.HiByte(clientItem.Item.MAC));
                        aabil.HitSpeed = (ushort)(aabil.HitSpeed + HUtil32.LoByte(clientItem.Item.AC));
                        aabil.HitSpeed = (ushort)(aabil.HitSpeed - HUtil32.LoByte(clientItem.Item.MAC));
                        aabil.HitSpeed = (ushort)(aabil.HitSpeed + clientItem.Item.AtkSpd);
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
                                aabil.HIT = (ushort)(aabil.HIT + HUtil32.HiByte(clientItem.Item.AC));
                                aabil.SPEED = (ushort)(aabil.SPEED + HUtil32.HiByte(clientItem.Item.MAC));
                                break;
                            case 26:
                                aabil.AC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(aabil.AC) + HUtil32.LoByte(clientItem.Item.AC)), (ushort)(HUtil32.HiByte(aabil.AC) + HUtil32.HiByte(clientItem.Item.AC)));
                                aabil.MAC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(aabil.MAC) + HUtil32.LoByte(clientItem.Item.MAC)), (ushort)(HUtil32.HiByte(aabil.MAC) + HUtil32.HiByte(clientItem.Item.MAC)));
                                aabil.HIT = (ushort)(aabil.HIT + clientItem.Item.Accurate);
                                aabil.SPEED = (ushort)(aabil.SPEED + clientItem.Item.Agility);
                                aabil.MP = (ushort)(aabil.MP + clientItem.Item.MpAdd);
                                break;
                        }
                        break;
                    case 52:
                        aabil.AC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(aabil.AC) + HUtil32.LoByte(clientItem.Item.AC)), (ushort)(HUtil32.HiByte(aabil.AC) + HUtil32.HiByte(clientItem.Item.AC)));
                        aabil.MAC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(aabil.MAC) + HUtil32.LoByte(clientItem.Item.MAC)), (ushort)(HUtil32.HiByte(aabil.MAC) + HUtil32.HiByte(clientItem.Item.MAC)));
                        aabil.SPEED = (ushort)(aabil.SPEED + clientItem.Item.Agility);
                        break;
                    case 54:
                        aabil.AC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(aabil.AC) + HUtil32.LoByte(clientItem.Item.AC)), (ushort)(HUtil32.HiByte(aabil.AC) + HUtil32.HiByte(clientItem.Item.AC)));
                        aabil.MAC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(aabil.MAC) + HUtil32.LoByte(clientItem.Item.MAC)), (ushort)(HUtil32.HiByte(aabil.MAC) + HUtil32.HiByte(clientItem.Item.MAC)));
                        aabil.HIT = (ushort)(aabil.HIT + clientItem.Item.Accurate);
                        aabil.SPEED = (ushort)(aabil.SPEED + clientItem.Item.Agility);
                        aabil.AntiPoison = (ushort)(aabil.AntiPoison + clientItem.Item.ToxAvoid);
                        break;
                    case 53:
                        aabil.HP = (ushort)(aabil.HP + clientItem.Item.HpAdd);
                        aabil.MP = (ushort)(aabil.MP + clientItem.Item.MpAdd);
                        break;
                    default:
                        aabil.AC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(aabil.AC) + HUtil32.LoByte(clientItem.Item.AC)), (ushort)(HUtil32.HiByte(aabil.AC) + HUtil32.HiByte(clientItem.Item.AC)));
                        aabil.MAC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(aabil.MAC) + HUtil32.LoByte(clientItem.Item.MAC)), (ushort)(HUtil32.HiByte(aabil.MAC) + HUtil32.HiByte(clientItem.Item.MAC)));
                        break;
                }
                aabil.DC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(aabil.DC) + HUtil32.LoByte(clientItem.Item.DC)), (ushort)HUtil32._MIN(255, HUtil32.HiByte(aabil.DC) + HUtil32.HiByte(clientItem.Item.DC)));
                aabil.MC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(aabil.MC) + HUtil32.LoByte(clientItem.Item.MC)), (ushort)HUtil32._MIN(255, HUtil32.HiByte(aabil.MC) + HUtil32.HiByte(clientItem.Item.MC)));
                aabil.SC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(aabil.SC) + HUtil32.LoByte(clientItem.Item.SC)), (ushort)HUtil32._MIN(255, HUtil32.HiByte(aabil.SC) + HUtil32.HiByte(clientItem.Item.SC)));
            }
        }

        internal void ApplyItemParametersEx(UserItem uitem, ref Ability aWabil)
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
                                    if ((aWabil.MaxHandWeight + clientItem.Item.EffValue1 > 255))
                                    {
                                        aWabil.MaxHandWeight = byte.MaxValue;
                                    }
                                    else
                                    {
                                        aWabil.MaxHandWeight = (byte)(aWabil.MaxHandWeight + clientItem.Item.EffValue1);
                                    }
                                    break;
                                case EfftypeConst.EFFTYPE_EQUIP_WHEIGHT_ADD:
                                    if ((aWabil.MaxWearWeight + clientItem.Item.EffValue1 > 255))
                                    {
                                        aWabil.MaxWearWeight = byte.MaxValue;
                                    }
                                    else
                                    {
                                        aWabil.MaxWearWeight = (byte)(aWabil.MaxWearWeight + clientItem.Item.EffValue1);
                                    }
                                    break;
                            }
                        }
                        if (clientItem.Item.EffType2 > 0)
                        {
                            switch (clientItem.Item.EffType2)
                            {
                                case EfftypeConst.EFFTYPE_TWOHAND_WEHIGHT_ADD:
                                    if ((aWabil.MaxHandWeight + clientItem.Item.EffValue2 > 255))
                                    {
                                        aWabil.MaxHandWeight = byte.MaxValue;
                                    }
                                    else
                                    {
                                        aWabil.MaxHandWeight = (byte)(aWabil.MaxHandWeight + clientItem.Item.EffValue2);
                                    }
                                    break;
                                case EfftypeConst.EFFTYPE_EQUIP_WHEIGHT_ADD:
                                    if ((aWabil.MaxWearWeight + clientItem.Item.EffValue2 > 255))
                                    {
                                        aWabil.MaxWearWeight = byte.MaxValue;
                                    }
                                    else
                                    {
                                        aWabil.MaxWearWeight = (byte)(aWabil.MaxWearWeight + clientItem.Item.EffValue2);
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
                                    if ((aWabil.MaxWeight + clientItem.Item.EffValue1 > 65000))
                                    {
                                        aWabil.MaxWeight = 65000;
                                    }
                                    else
                                    {
                                        aWabil.MaxWeight = (ushort)(aWabil.MaxWeight + clientItem.Item.EffValue1);
                                    }
                                    break;
                            }
                        }
                        if (clientItem.Item.EffType2 > 0)
                        {
                            switch (clientItem.Item.EffType2)
                            {
                                case EfftypeConst.EFFTYPE_BAG_WHIGHT_ADD:
                                    if ((aWabil.MaxWeight + clientItem.Item.EffValue2 > 65000))
                                    {
                                        aWabil.MaxWeight = 65000;
                                    }
                                    else
                                    {
                                        aWabil.MaxWeight = (ushort)(aWabil.MaxWeight + clientItem.Item.EffValue2);
                                    }
                                    break;
                            }
                        }
                        break;
                }
            }
        }

        protected void ChangeItemWithLevel(ref ClientItem citem, int level)
        {
            if (citem.Item.Shape == ItemShapeConst.DRESS_SHAPE_WING && (citem.Item.StdMode == ItemShapeConst.DRESS_STDMODE_MAN || citem.Item.StdMode == ItemShapeConst.DRESS_STDMODE_WOMAN))
            {
                if (level > 20)
                {
                    if (level < 40)
                    {
                        citem.Item.DC = (ushort)(citem.Item.DC + HUtil32.MakeWord(0, 1));
                        citem.Item.MC = (ushort)(citem.Item.MC + HUtil32.MakeWord(0, 2));
                        citem.Item.SC = (ushort)(citem.Item.SC + HUtil32.MakeWord(0, 2));
                        citem.Item.AC = (ushort)(citem.Item.AC + HUtil32.MakeWord(2, 3));
                        citem.Item.MAC = (ushort)(citem.Item.MAC + HUtil32.MakeWord(0, 2));
                    }
                    else if (level < 50)
                    {
                        citem.Item.DC = (ushort)(citem.Item.DC + HUtil32.MakeWord(0, 3));
                        citem.Item.MC = (ushort)(citem.Item.MC + HUtil32.MakeWord(0, 4));
                        citem.Item.SC = (ushort)(citem.Item.SC + HUtil32.MakeWord(0, 4));
                        citem.Item.AC = (ushort)(citem.Item.AC + HUtil32.MakeWord(5, 5));
                        citem.Item.MAC = (ushort)(citem.Item.MAC + HUtil32.MakeWord(1, 2));
                    }
                    else
                    {
                        citem.Item.DC = (ushort)(citem.Item.DC + HUtil32.MakeWord(0, 5));
                        citem.Item.MC = (ushort)(citem.Item.MC + HUtil32.MakeWord(0, 6));
                        citem.Item.SC = (ushort)(citem.Item.SC + HUtil32.MakeWord(0, 6));
                        citem.Item.AC = (ushort)(citem.Item.AC + HUtil32.MakeWord(9, 7));
                        citem.Item.MAC = (ushort)(citem.Item.MAC + HUtil32.MakeWord(2, 4));
                    }
                }
            }
        }

        protected void ChangeItemByJob(ref ClientItem citem, int level)
        {
            if ((citem.Item.StdMode == 22) && (citem.Item.Shape == DragonConst.DRAGON_RING_SHAPE))
            {
                switch (Job)
                {
                    case PlayJob.Warrior:
                        citem.Item.DC = HUtil32.MakeWord(HUtil32.LoByte(citem.Item.DC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(citem.Item.DC) + 4));
                        citem.Item.MC = 0;
                        citem.Item.SC = 0;
                        break;
                    case PlayJob.Wizard:
                        citem.Item.DC = 0;
                        citem.Item.SC = 0;
                        break;
                    case PlayJob.Taoist:
                        citem.Item.MC = 0;
                        break;
                }
            }
            else if ((citem.Item.StdMode == 26) && (citem.Item.Shape == DragonConst.DRAGON_BRACELET_SHAPE))
            {
                switch (Job)
                {
                    case PlayJob.Warrior:
                        citem.Item.DC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(citem.Item.DC) + 1), (ushort)HUtil32._MIN(255, HUtil32.HiByte(citem.Item.DC) + 2));
                        citem.Item.MC = 0;
                        citem.Item.SC = 0;
                        citem.Item.AC = HUtil32.MakeWord(HUtil32.LoByte(citem.Item.AC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(citem.Item.AC) + 1));
                        break;
                    case PlayJob.Wizard:
                        citem.Item.DC = 0;
                        citem.Item.SC = 0;
                        citem.Item.AC = HUtil32.MakeWord(HUtil32.LoByte(citem.Item.AC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(citem.Item.AC) + 1));
                        break;
                    case PlayJob.Taoist:
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
                    case PlayJob.Wizard:
                        citem.Item.DC = 0;
                        citem.Item.SC = 0;
                        break;
                    case PlayJob.Taoist:
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
                    case PlayJob.Wizard:
                        citem.Item.DC = 0;
                        citem.Item.SC = 0;
                        break;
                    case PlayJob.Taoist:
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
                    case PlayJob.Wizard:
                        citem.Item.DC = 0;
                        citem.Item.SC = 0;
                        break;
                    case PlayJob.Taoist:
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
                        citem.Item.DC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(citem.Item.DC) + 1), (ushort)HUtil32._MIN(255, HUtil32.HiByte(citem.Item.DC) + 28));
                        citem.Item.MC = 0;
                        citem.Item.SC = 0;
                        citem.Item.AC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(citem.Item.AC) - 2), HUtil32.HiByte(citem.Item.AC));
                        break;
                    case PlayJob.Wizard:
                        citem.Item.SC = 0;
                        if (HUtil32.HiByte(citem.Item.MAC) > 12)
                        {
                            citem.Item.MAC = HUtil32.MakeWord(HUtil32.LoByte(citem.Item.MAC), (ushort)(HUtil32.HiByte(citem.Item.MAC) - 12));
                        }
                        else
                        {
                            citem.Item.MAC = HUtil32.MakeWord(HUtil32.LoByte(citem.Item.MAC), 0);
                        }
                        break;
                    case PlayJob.Taoist:
                        citem.Item.DC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(citem.Item.DC) + 2), (ushort)HUtil32._MIN(255, HUtil32.HiByte(citem.Item.DC) + 10));
                        citem.Item.MC = 0;
                        citem.Item.AC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(citem.Item.AC) - 2), HUtil32.HiByte(citem.Item.AC));
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
                            citem.Item.DC = HUtil32.MakeWord(HUtil32.LoByte(citem.Item.DC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(citem.Item.DC) + 2));
                            citem.Item.MC = 0;
                            citem.Item.SC = 0;
                            break;
                        case PlayJob.Wizard:
                            citem.Item.DC = 0;
                            citem.Item.MC = HUtil32.MakeWord(HUtil32.LoByte(citem.Item.MC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(citem.Item.MC) + 2));
                            citem.Item.SC = 0;
                            break;
                        case PlayJob.Taoist:
                            citem.Item.DC = 0;
                            citem.Item.MC = 0;
                            citem.Item.SC = HUtil32.MakeWord(HUtil32.LoByte(citem.Item.SC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(citem.Item.SC) + 2));
                            break;
                    }
                }
                else if ((citem.Item.Shape == ShapeConst.GOLDMEDAL_SHAPE) || (citem.Item.Shape == ShapeConst.SILVERMEDAL_SHAPE) || (citem.Item.Shape == ShapeConst.BRONZEMEDAL_SHAPE))
                {
                    switch (Job)
                    {
                        case PlayJob.Warrior:
                            citem.Item.DC = HUtil32.MakeWord(HUtil32.LoByte(citem.Item.DC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(citem.Item.DC)));
                            citem.Item.MC = 0;
                            citem.Item.SC = 0;
                            break;
                        case PlayJob.Wizard:
                            citem.Item.DC = 0;
                            citem.Item.MC = HUtil32.MakeWord(HUtil32.LoByte(citem.Item.MC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(citem.Item.MC)));
                            citem.Item.SC = 0;
                            break;
                        case PlayJob.Taoist:
                            citem.Item.DC = 0;
                            citem.Item.MC = 0;
                            citem.Item.SC = HUtil32.MakeWord(HUtil32.LoByte(citem.Item.SC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(citem.Item.SC)));
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
                            std.Item.DC = HUtil32.MakeWord(HUtil32.LoByte(item.DC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(item.DC) + 4));
                            std.Item.MC = 0;
                            std.Item.SC = 0;
                            break;
                        case PlayJob.Wizard:
                            std.Item.DC = 0;
                            std.Item.SC = 0;
                            break;
                        case PlayJob.Taoist:
                            std.Item.MC = 0;
                            break;
                    }
                }
                else if ((item.StdMode == 26) && (item.Shape == DragonConst.DRAGON_BRACELET_SHAPE))
                {
                    switch (Job)
                    {
                        case PlayJob.Warrior:
                            std.Item.DC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(item.DC) + 1), (ushort)HUtil32._MIN(255, HUtil32.HiByte(item.DC) + 2));
                            std.Item.MC = 0;
                            std.Item.SC = 0;
                            std.Item.AC = HUtil32.MakeWord(HUtil32.LoByte(item.AC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(item.AC) + 1));
                            break;
                        case PlayJob.Wizard:
                            std.Item.DC = 0;
                            std.Item.SC = 0;
                            std.Item.AC = HUtil32.MakeWord(HUtil32.LoByte(item.AC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(item.AC) + 1));
                            break;
                        case PlayJob.Taoist:
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
                        case PlayJob.Wizard:
                            std.Item.DC = 0;
                            std.Item.SC = 0;
                            break;
                        case PlayJob.Taoist:
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
                        case PlayJob.Wizard:
                            std.Item.DC = 0;
                            std.Item.SC = 0;
                            break;
                        case PlayJob.Taoist:
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
                        case PlayJob.Wizard:
                            std.Item.DC = 0;
                            std.Item.SC = 0;
                            break;
                        case PlayJob.Taoist:
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
                            std.Item.DC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(item.DC) + 1), (ushort)HUtil32._MIN(255, HUtil32.HiByte(item.DC) + 28));
                            std.Item.MC = 0;
                            std.Item.SC = 0;
                            std.Item.AC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(item.AC) - 2), HUtil32.HiByte(item.AC));
                            break;
                        case PlayJob.Wizard:
                            std.Item.SC = 0;
                            if (HUtil32.HiByte(item.MAC) > 12)
                            {
                                std.Item.MAC = HUtil32.MakeWord(HUtil32.LoByte(item.MAC), (ushort)(HUtil32.HiByte(item.MAC) - 12));
                            }
                            else
                            {
                                std.Item.MAC = HUtil32.MakeWord(HUtil32.LoByte(item.MAC), 0);
                            }
                            break;
                        case PlayJob.Taoist:
                            std.Item.DC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(item.DC) + 2), (ushort)HUtil32._MIN(255, HUtil32.HiByte(item.DC) + 10));
                            std.Item.MC = 0;
                            std.Item.AC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(item.AC) - 2), HUtil32.HiByte(item.AC));
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
                                std.Item.DC = HUtil32.MakeWord(HUtil32.LoByte(item.DC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(item.DC) + 2));
                                std.Item.MC = 0;
                                std.Item.SC = 0;
                                break;
                            case PlayJob.Wizard:
                                std.Item.DC = 0;
                                std.Item.MC = HUtil32.MakeWord(HUtil32.LoByte(item.MC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(item.MC) + 2));
                                std.Item.SC = 0;
                                break;
                            case PlayJob.Taoist:
                                std.Item.DC = 0;
                                std.Item.MC = 0;
                                std.Item.SC = HUtil32.MakeWord(HUtil32.LoByte(item.SC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(item.SC) + 2));
                                break;
                        }
                    }
                    else if ((item.Shape == ShapeConst.GOLDMEDAL_SHAPE) || (item.Shape == ShapeConst.SILVERMEDAL_SHAPE) || (item.Shape == ShapeConst.BRONZEMEDAL_SHAPE))
                    {
                        switch (Job)
                        {
                            case PlayJob.Warrior:
                                std.Item.DC = HUtil32.MakeWord(HUtil32.LoByte(item.DC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(item.DC)));
                                std.Item.MC = 0;
                                std.Item.SC = 0;
                                break;
                            case PlayJob.Wizard:
                                std.Item.DC = 0;
                                std.Item.MC = HUtil32.MakeWord(HUtil32.LoByte(item.MC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(item.MC)));
                                std.Item.SC = 0;
                                break;
                            case PlayJob.Taoist:
                                std.Item.DC = 0;
                                std.Item.MC = 0;
                                std.Item.SC = HUtil32.MakeWord(HUtil32.LoByte(item.SC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(item.SC)));
                                break;
                        }
                    }
                }
                if (((item.StdMode == 10) || (item.StdMode == 11)) && (item.Shape == ItemShapeConst.DRESS_SHAPE_PBKING))
                {
                    switch (Job)
                    {
                        case PlayJob.Warrior:
                            std.Item.DC = HUtil32.MakeWord(HUtil32.LoByte(item.DC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(item.DC) + 2));
                            std.Item.MC = 0;
                            std.Item.SC = 0;
                            std.Item.AC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(item.AC) + 2), (ushort)HUtil32._MIN(255, HUtil32.HiByte(item.AC) + 4));
                            std.Item.MpAdd = item.MpAdd + 30;
                            break;
                        case PlayJob.Wizard:
                            std.Item.DC = 0;
                            std.Item.SC = 0;
                            std.Item.MAC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(item.MAC) + 1), (ushort)HUtil32._MIN(255, HUtil32.HiByte(item.MAC) + 2));
                            std.Item.HpAdd = item.HpAdd + 30;
                            break;
                        case PlayJob.Taoist:
                            std.Item.DC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(item.DC) + 1), (ushort)HUtil32._MIN(255, HUtil32.HiByte(item.DC)));
                            std.Item.MC = 0;
                            std.Item.AC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(item.AC) + 1), (ushort)HUtil32._MIN(255, HUtil32.HiByte(item.AC)));
                            std.Item.MAC = HUtil32.MakeWord((ushort)(HUtil32.LoByte(item.MAC) + 1), (ushort)HUtil32._MIN(255, HUtil32.HiByte(item.MAC)));
                            std.Item.HpAdd = item.HpAdd + 20;
                            std.Item.MpAdd = item.MpAdd + 10;
                            break;
                    }
                }
            }
        }

        private void ApplySlaveLevelAbilitys()
        {
            //if ((Race == Grobal2.RC_ANGEL) || (Race == Grobal2.RC_CLONE))
            //{
            //    return;
            //}
            ushort chp = 0;
            if ((Race == ActorRace.WhiteSkeleton) || (Race == ActorRace.ElfMonster) || (Race == ActorRace.ElfWarriormon))
            {
                WAbil.DC = HUtil32.MakeWord(HUtil32.LoByte(WAbil.DC), HUtil32.HiByte(Abil.DC));
                WAbil.DC = HUtil32.MakeWord(HUtil32.LoByte(WAbil.DC), (ushort)(int)Math.Round(HUtil32.HiByte(WAbil.DC) + (3 * (0.3 + SlaveExpLevel * 0.1) * SlaveExpLevel)));
                chp = (ushort)(chp + Math.Round(Abil.MaxHP * (0.3 + SlaveExpLevel * 0.1)) * SlaveExpLevel);
                chp = (ushort)(Abil.MaxHP + chp);
                if (SlaveExpLevel > 0)
                {
                    WAbil.MaxHP = chp;
                }
                else
                {
                    WAbil.MaxHP = Abil.MaxHP;
                }
                //WAbil.DC = HUtil32.MakeWord(HUtil32.LoByte(WAbil.DC), (ushort)(HUtil32.HiByte(WAbil.DC) + ExtraAbil[AbilConst.EABIL_DCUP]));
            }
            else
            {
                if (Master != null)
                {
                    chp = Abil.MaxHP;
                    WAbil.DC = HUtil32.MakeWord(HUtil32.LoByte(WAbil.DC), HUtil32.HiByte(Abil.DC));
                    WAbil.DC = HUtil32.MakeWord(HUtil32.LoByte(WAbil.DC), (ushort)Math.Abs(HUtil32.HiByte(WAbil.DC) + (2 * SlaveExpLevel)));
                    chp = (ushort)(chp + Math.Round(Abil.MaxHP * 0.15) * SlaveExpLevel);
                    WAbil.MaxHP = (ushort)HUtil32._MIN(Math.Abs(Abil.MaxHP + 60 * SlaveExpLevel), chp);
                    WAbil.MAC = 0;
                }
            }
            //AccuracyPoint = 15;
        }

        internal byte GetMyLight()
        {
            byte currentLight = 0;
            if (Race == ActorRace.Play)
            {
                if (this is PlayObject)
                {
                    if (true)//BoHighLevelEffect
                    {
                        if (Abil.Level >= EfftypeConst.EFFECTIVE_HIGHLEVEL)
                        {
                            currentLight = 1;
                        }
                    }
                }
                for (var i = Grobal2.U_DRESS; i <= Grobal2.U_CHARM; i++)
                {
                    if (UseItems[i] == null)
                    {
                        continue;
                    }
                    if ((UseItems[i].Index > 0) && (UseItems[i].Dura > 0))
                    {
                        StdItem stdItem = M2Share.WorldEngine.GetStdItem(UseItems[i].Index);
                        if (stdItem != null)
                        {
                            if (currentLight < stdItem.Light)
                            {
                                currentLight = stdItem.Light;
                            }
                        }
                    }
                }
            }
            return currentLight;
        }
    }
}