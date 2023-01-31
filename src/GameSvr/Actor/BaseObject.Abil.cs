using GameSvr.Items;
using GameSvr.Player;
using SystemModule.Consts;
using SystemModule.Data;
using SystemModule.Enums;
using SystemModule.Packets.ClientPackets;

namespace GameSvr.Actor
{
    public partial class BaseObject
    {
        /// <summary>
        /// 计算自身属性
        /// </summary>
        public virtual void RecalcAbilitys()
        {
            AddAbil = new AddAbility();
            Ability temp = WAbil;
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
            bool oldhmode = HideMode;
            HideMode = false;
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
            if (AddAbil.HitSpeed > 0)
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
            Luck = (byte)(Luck + AddAbil.Luck);
            Luck = (byte)(Luck - AddAbil.UnLuck);
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
            if (Race >= ActorRace.Animal)
            {
                ApplySlaveLevelAbilitys();
            }
        }

        public void RecalcLevelAbilitys()
        {
            if (Race == ActorRace.Play)
            {
                int n;
                byte nLevel = Abil.Level;
                switch (((PlayObject)this).Job)
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
        protected virtual void RecalcHitSpeed()
        {
            HitPlus = 0;
            HitDouble = 0;
        }

        protected static void ChangeItemWithLevel(ref ClientItem citem, int level)
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
                for (byte i = Grobal2.U_DRESS; i <= Grobal2.U_CHARM; i++)
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