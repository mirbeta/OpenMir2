using GameSrv.Monster;
using SystemModule.Consts;
using SystemModule.Enums;
using SystemModule.Packets.ClientPackets;

namespace GameSrv.Actor
{
    public partial class BaseObject {
        /// <summary>
        /// 计算自身属性
        /// </summary>
        public virtual void RecalcAbilitys()
        {
            AddAbil = default;
            Ability temp = WAbil;
            WAbil = (Ability)Abil.Clone();
            WAbil.HP = temp.HP;
            WAbil.MP = temp.MP;
            WAbil.Weight = 0;
            WAbil.WearWeight = 0;
            WAbil.HandWeight = 0;
            AntiPoison = 0;
            AntiMagic = 1;
            HitSpeed = 0;
            bool oldhmode = HideMode;
            HideMode = false;
            if (Transparent && (StatusTimeArr[PoisonState.STATETRANSPARENT] > 0)) {
                HideMode = true;
            }
            if (HideMode) {
                if (!oldhmode) {
                    CharStatus = GetCharStatus();
                    StatusChanged();
                }
            }
            else {
                if (oldhmode) {
                    StatusTimeArr[PoisonState.STATETRANSPARENT] = 0;
                    CharStatus = GetCharStatus();
                    StatusChanged();
                }
            }
            RecalcHitSpeed();
            if (AddAbil.HitSpeed > 0) {
                AddAbil.HitSpeed = ((ushort)(AddAbil.HitSpeed / 2));
            }
            else {
                AddAbil.HitSpeed = (ushort)((AddAbil.HitSpeed - 1) / 2);
            }
            AddAbil.HitSpeed = (ushort)HUtil32._MIN(15, AddAbil.HitSpeed);
            SpeedPoint = (byte)(SpeedPoint + AddAbil.SPEED);
            HitPoint = (byte)(HitPoint + AddAbil.HIT);
            AntiPoison = (byte)(AntiPoison + AddAbil.AntiPoison);
            AntiMagic = (ushort)(AntiMagic + AddAbil.AntiMagic);
            HitSpeed = AddAbil.HitSpeed;
            WAbil.MaxHP = (ushort)(Abil.MaxHP + AddAbil.HP);
            WAbil.MaxMP = (ushort)(Abil.MaxMP + AddAbil.MP);
            WAbil.AC = HUtil32.MakeWord((ushort)HUtil32._MIN(255, HUtil32.LoByte(AddAbil.AC) + HUtil32.LoByte(Abil.AC)), (ushort)HUtil32._MIN(255, HUtil32.HiByte(AddAbil.AC) + HUtil32.HiByte(Abil.AC)));
            WAbil.MAC = HUtil32.MakeWord((ushort)HUtil32._MIN(255, HUtil32.LoByte(AddAbil.MAC) + HUtil32.LoByte(Abil.MAC)), (ushort)HUtil32._MIN(255, HUtil32.HiByte(AddAbil.MAC) + HUtil32.HiByte(Abil.MAC)));
            WAbil.DC = HUtil32.MakeWord((ushort)HUtil32._MIN(255, HUtil32.LoByte(AddAbil.DC) + HUtil32.LoByte(Abil.DC)), (ushort)HUtil32._MIN(255, HUtil32.HiByte(AddAbil.DC) + HUtil32.HiByte(Abil.DC)));
            WAbil.MC = HUtil32.MakeWord((ushort)HUtil32._MIN(255, HUtil32.LoByte(AddAbil.MC) + HUtil32.LoByte(Abil.MC)), (ushort)HUtil32._MIN(255, HUtil32.HiByte(AddAbil.MC) + HUtil32.HiByte(Abil.MC)));
            WAbil.SC = HUtil32.MakeWord((ushort)HUtil32._MIN(255, HUtil32.LoByte(AddAbil.SC) + HUtil32.LoByte(Abil.SC)), (ushort)HUtil32._MIN(255, HUtil32.HiByte(AddAbil.SC) + HUtil32.HiByte(Abil.SC)));
            if (StatusTimeArr[PoisonState.DefenceUP] > 0) {
                WAbil.AC = HUtil32.MakeWord(HUtil32.LoByte(WAbil.AC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(WAbil.AC) + (Abil.Level / 7) + StatusArrTick[PoisonState.DefenceUP]));
            }
            if (StatusTimeArr[PoisonState.MagDefenceUP] > 0) {
                WAbil.MAC = HUtil32.MakeWord(HUtil32.LoByte(WAbil.MAC), (ushort)HUtil32._MIN(255, HUtil32.HiByte(WAbil.MAC) + (Abil.Level / 7) + StatusArrTick[PoisonState.MagDefenceUP]));
            }
            if (Race >= ActorRace.Animal) {
                ApplySlaveLevelAbilitys();
            }
        }
        
        /// <summary>
        /// 计算攻击速度
        /// </summary>
        protected virtual void RecalcHitSpeed() {

        }

        protected static void ChangeItemWithLevel(ref ClientItem citem, int level) {
            if (citem.Item.Shape == ItemShapeConst.DRESS_SHAPE_WING && (citem.Item.StdMode == ItemShapeConst.DRESS_STDMODE_MAN || citem.Item.StdMode == ItemShapeConst.DRESS_STDMODE_WOMAN)) {
                if (level > 20) {
                    if (level < 40) {
                        citem.Item.DC = (ushort)(citem.Item.DC + HUtil32.MakeWord(0, 1));
                        citem.Item.MC = (ushort)(citem.Item.MC + HUtil32.MakeWord(0, 2));
                        citem.Item.SC = (ushort)(citem.Item.SC + HUtil32.MakeWord(0, 2));
                        citem.Item.AC = (ushort)(citem.Item.AC + HUtil32.MakeWord(2, 3));
                        citem.Item.MAC = (ushort)(citem.Item.MAC + HUtil32.MakeWord(0, 2));
                    }
                    else if (level < 50) {
                        citem.Item.DC = (ushort)(citem.Item.DC + HUtil32.MakeWord(0, 3));
                        citem.Item.MC = (ushort)(citem.Item.MC + HUtil32.MakeWord(0, 4));
                        citem.Item.SC = (ushort)(citem.Item.SC + HUtil32.MakeWord(0, 4));
                        citem.Item.AC = (ushort)(citem.Item.AC + HUtil32.MakeWord(5, 5));
                        citem.Item.MAC = (ushort)(citem.Item.MAC + HUtil32.MakeWord(1, 2));
                    }
                    else {
                        citem.Item.DC = (ushort)(citem.Item.DC + HUtil32.MakeWord(0, 5));
                        citem.Item.MC = (ushort)(citem.Item.MC + HUtil32.MakeWord(0, 6));
                        citem.Item.SC = (ushort)(citem.Item.SC + HUtil32.MakeWord(0, 6));
                        citem.Item.AC = (ushort)(citem.Item.AC + HUtil32.MakeWord(9, 7));
                        citem.Item.MAC = (ushort)(citem.Item.MAC + HUtil32.MakeWord(2, 4));
                    }
                }
            }
        }

        private void ApplySlaveLevelAbilitys() {
            //if ((Race == Grobal2.RC_ANGEL) || (Race == Grobal2.RC_CLONE))
            //{
            //    return;
            //}
            ushort chp = 0;
            var slaveExpLevel = ((MonsterObject)this).SlaveExpLevel;
            if ((Race == ActorRace.WhiteSkeleton) || (Race == ActorRace.ElfMonster) || (Race == ActorRace.ElfWarriormon)) {
                WAbil.DC = HUtil32.MakeWord(HUtil32.LoByte(WAbil.DC), HUtil32.HiByte(Abil.DC));
                WAbil.DC = HUtil32.MakeWord(HUtil32.LoByte(WAbil.DC), (ushort)(int)Math.Round(HUtil32.HiByte(WAbil.DC) + (3 * (0.3 + slaveExpLevel * 0.1) * slaveExpLevel)));
                chp = (ushort)(chp + Math.Round(Abil.MaxHP * (0.3 + slaveExpLevel * 0.1)) * slaveExpLevel);
                chp = (ushort)(Abil.MaxHP + chp);
                if (slaveExpLevel > 0) {
                    WAbil.MaxHP = chp;
                }
                else {
                    WAbil.MaxHP = Abil.MaxHP;
                }
                //WAbil.DC = HUtil32.MakeWord(HUtil32.LoByte(WAbil.DC), (ushort)(HUtil32.HiByte(WAbil.DC) + ExtraAbil[AbilConst.EABIL_DCUP]));
            }
            else {
                if (Master != null) {
                    chp = Abil.MaxHP;
                    WAbil.DC = HUtil32.MakeWord(HUtil32.LoByte(WAbil.DC), HUtil32.HiByte(Abil.DC));
                    WAbil.DC = HUtil32.MakeWord(HUtil32.LoByte(WAbil.DC), (ushort)Math.Abs(HUtil32.HiByte(WAbil.DC) + (2 * slaveExpLevel)));
                    chp = (ushort)(chp + Math.Round(Abil.MaxHP * 0.15) * slaveExpLevel);
                    WAbil.MaxHP = (ushort)HUtil32._MIN(Abil.MaxHP + 60 * slaveExpLevel, chp);
                    WAbil.MAC = 0;
                }
            }
            //AccuracyPoint = 15;
        }
    }
}