using GameSvr.Actor;
using GameSvr.Items;
using SystemModule.Consts;
using SystemModule.Enums;
using SystemModule.Packets.ClientPackets;

namespace GameSvr.Player {
    public class CharacterObject : AnimalObject {
        /// <summary>
        /// 身上物品
        /// </summary>
        public UserItem[] UseItems;
        /// <summary>
        /// 攻击状态
        /// </summary>
        public AttackMode AttatckMode;
        /// <summary>
        /// 衣服特效(如天外飞仙衣服效果)
        /// </summary>
        private readonly byte DressEffType;
        /// <summary>
        /// 马类型
        /// </summary>
        public byte HorseType;
        /// <summary>
        /// 魔法盾
        /// </summary>
        internal bool AbilMagBubbleDefence;
        /// <summary>
        /// 魔法盾等级
        /// </summary>
        protected byte MagBubbleDefenceLevel;
        /// <summary>
        /// 气血石时间
        /// </summary>
        private int IncHpStoneTime;
        /// <summary>
        /// 魔血石时间
        /// </summary>
        private int IncMpStoneTime;

        public CharacterObject() {
            IncHpStoneTime = HUtil32.GetTickCount();
            IncMpStoneTime = HUtil32.GetTickCount();
            UseItems = new UserItem[13];
        }

        /// <summary>
        /// 魔法盾
        /// </summary>
        /// <returns></returns>
        public bool MagBubbleDefenceUp(byte nLevel, ushort nSec) {
            if (StatusTimeArr[PoisonState.BubbleDefenceUP] != 0) {
                return false;
            }
            int nOldStatus = CharStatus;
            StatusTimeArr[PoisonState.BubbleDefenceUP] = nSec;
            StatusArrTick[PoisonState.BubbleDefenceUP] = HUtil32.GetTickCount();
            CharStatus = GetCharStatus();
            if (nOldStatus != CharStatus) {
                StatusChanged();
            }
            AbilMagBubbleDefence = true;
            MagBubbleDefenceLevel = nLevel;
            return true;
        }

        /// <summary>
        /// 获取攻击伤害点数
        /// </summary>
        /// <returns></returns>
        public override ushort GetHitStruckDamage(BaseObject target, int nDamage) {
            ushort damage = base.GetHitStruckDamage(target, nDamage);
            if (nDamage > 0 && AbilMagBubbleDefence) {
                damage = (ushort)HUtil32.Round(damage / 100 * (MagBubbleDefenceLevel + 2) * 8);
                DamageBubbleDefence(damage);
            }
            return damage;
        }

        /// <summary>
        /// 获取魔法伤害点数
        /// </summary>
        /// <returns></returns>
        public override ushort GetMagStruckDamage(BaseObject baseObject, ushort nDamage) {
            ushort damage = base.GetMagStruckDamage(baseObject, nDamage);
            if ((damage > 0) && AbilMagBubbleDefence) {
                damage = (ushort)HUtil32.Round(damage / 1.0e2 * (MagBubbleDefenceLevel + 2) * 8.0);//魔法盾减伤
                DamageBubbleDefence(damage);
            }
            return damage;
        }

        public override ushort GetFeatureEx() {
            return HUtil32.MakeWord(OnHorse ? HorseType : (byte)0, DressEffType);
        }

        /// <summary>
        /// 气血石和魔血石
        /// </summary>
        internal void LifeStone() {
            if (!Death && Race == ActorRace.Play || Race == ActorRace.PlayClone) {
                if (UseItems.Length >= Grobal2.U_CHARM && UseItems[Grobal2.U_CHARM] != null && UseItems[Grobal2.U_CHARM].Index > 0) {
                    StdItem StdItem = M2Share.WorldEngine.GetStdItem(UseItems[Grobal2.U_CHARM].Index);
                    if ((StdItem.StdMode == 7) && (StdItem.Shape == 2 || StdItem.Shape == 3)) {
                        ushort stoneDura;
                        ushort dCount;
                        ushort bCount;
                        // 加HP
                        if ((IncHealth == 0) && (UseItems[Grobal2.U_CHARM].Index > 0) && ((HUtil32.GetTickCount() - IncHpStoneTime) > M2Share.Config.HPStoneIntervalTime) && ((WAbil.HP / WAbil.MaxHP * 100) < M2Share.Config.HPStoneStartRate)) {
                            IncHpStoneTime = HUtil32.GetTickCount();
                            stoneDura = (ushort)(UseItems[Grobal2.U_CHARM].Dura * 10);
                            bCount = (ushort)(stoneDura / M2Share.Config.HPStoneAddRate);
                            dCount = (ushort)(WAbil.MaxHP - WAbil.HP);
                            if (dCount > bCount) {
                                dCount = bCount;
                            }
                            if (stoneDura > dCount) {
                                IncHealth += dCount;
                                UseItems[Grobal2.U_CHARM].Dura -= (ushort)HUtil32.Round(dCount / 10);
                            }
                            else {
                                stoneDura = 0;
                                IncHealth += stoneDura;
                                UseItems[Grobal2.U_CHARM].Dura = 0;
                            }
                            if (UseItems[Grobal2.U_CHARM].Dura >= 1000) {
                                if (Race == ActorRace.Play) {
                                    SendMsg(this, Messages.RM_DURACHANGE, Grobal2.U_CHARM, UseItems[Grobal2.U_CHARM].Dura, UseItems[Grobal2.U_CHARM].DuraMax, 0, "");
                                }
                            }
                            else {
                                UseItems[Grobal2.U_CHARM].Dura = 0;
                                if (Race == ActorRace.Play) {
                                    ((PlayObject)this).SendDelItems(UseItems[Grobal2.U_CHARM]);
                                }
                                UseItems[Grobal2.U_CHARM].Index = 0;
                            }
                        }
                        // 加MP
                        if ((IncSpell == 0) && (UseItems[Grobal2.U_CHARM].Index > 0) && ((HUtil32.GetTickCount() - IncMpStoneTime) > M2Share.Config.MpStoneIntervalTime) && ((WAbil.MP / WAbil.MaxMP * 100) < M2Share.Config.MPStoneStartRate)) {
                            IncMpStoneTime = HUtil32.GetTickCount();
                            stoneDura = (ushort)(UseItems[Grobal2.U_CHARM].Dura * 10);
                            bCount = (ushort)(stoneDura / M2Share.Config.MPStoneAddRate);
                            dCount = (ushort)(WAbil.MaxMP - WAbil.MP);
                            if (dCount > bCount) {
                                dCount = bCount;
                            }
                            if (stoneDura > dCount) {
                                IncSpell += dCount;
                                UseItems[Grobal2.U_CHARM].Dura -= (ushort)HUtil32.Round(dCount / 10);
                            }
                            else {
                                stoneDura = 0;
                                IncSpell += stoneDura;
                                UseItems[Grobal2.U_CHARM].Dura = 0;
                            }
                            if (UseItems[Grobal2.U_CHARM].Dura >= 1000) {
                                if (Race == ActorRace.Play) {
                                    SendMsg(this, Messages.RM_DURACHANGE, Grobal2.U_CHARM, UseItems[Grobal2.U_CHARM].Dura, UseItems[Grobal2.U_CHARM].DuraMax, 0, "");
                                }
                            }
                            else {
                                UseItems[Grobal2.U_CHARM].Dura = 0;
                                if (Race == ActorRace.Play) {
                                    ((PlayObject)this).SendDelItems(UseItems[Grobal2.U_CHARM]);
                                }
                                UseItems[Grobal2.U_CHARM].Index = 0;
                            }
                        }
                    }
                }
            }
        }
    }
}