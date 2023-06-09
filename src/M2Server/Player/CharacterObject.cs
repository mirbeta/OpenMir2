using M2Server.Actor;
using M2Server.Items;
using SystemModule;
using SystemModule.Consts;
using SystemModule.Data;
using SystemModule.Enums;
using SystemModule.Packets.ClientPackets;

namespace M2Server.Player
{
    public class CharacterObject : AnimalObject
    {
        /// <summary>
        /// 攻击速度
        /// </summary>
        internal ushort HitSpeed;
        /// <summary>
        /// HP恢复点数
        /// </summary>
        protected ushort HealthRecover;
        /// <summary>
        /// MP恢复点数
        /// </summary>
        protected ushort SpellRecover;
        /// <summary>
        /// 中毒恢复点数
        /// </summary>
        protected ushort PoisonRecover;
        protected byte PerHealth;
        protected byte PerHealing;
        protected byte PerSpell;
        /// <summary>
        /// 增加血量的间隔
        /// </summary>
        protected int IncHealthSpellTick;
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
        /// 骑马
        /// </summary>
        public bool OnHorse;
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
        public ushort IncHealth;
        public ushort IncSpell;
        public ushort IncHealing;
        /// <summary>
        /// 死亡是不是掉装备
        /// </summary>
        public bool NoDropUseItem = false;

        public CharacterObject()
        {
            PerHealth = 5;
            PerHealing = 5;
            PerSpell = 5;
            IncHealthSpellTick = HUtil32.GetTickCount();
            IncHpStoneTime = HUtil32.GetTickCount();
            IncMpStoneTime = HUtil32.GetTickCount();
            UseItems = new UserItem[13];
        }

        /// <summary>
        /// 魔法盾
        /// </summary>
        /// <returns></returns>
        public bool MagBubbleDefenceUp(byte nLevel, ushort nSec)
        {
            if (StatusTimeArr[PoisonState.BubbleDefenceUP] != 0)
            {
                return false;
            }
            int nOldStatus = CharStatus;
            StatusTimeArr[PoisonState.BubbleDefenceUP] = nSec;
            StatusArrTick[PoisonState.BubbleDefenceUP] = HUtil32.GetTickCount();
            CharStatus = GetCharStatus();
            if (nOldStatus != CharStatus)
            {
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
        public override ushort GetHitStruckDamage(IActor target, int nDamage)
        {
            ushort damage = base.GetHitStruckDamage(target, nDamage);
            if (nDamage > 0 && AbilMagBubbleDefence)
            {
                damage = (ushort)HUtil32.Round(damage / 100.0 * (MagBubbleDefenceLevel + 2) * 8);
                DamageBubbleDefence(damage);
            }
            return damage;
        }

        /// <summary>
        /// 获取魔法伤害点数
        /// </summary>
        /// <returns></returns>
        public override int GetMagStruckDamage(IActor baseObject, int nDamage)
        {
            var damage = base.GetMagStruckDamage(baseObject, nDamage);
            if ((damage > 0) && AbilMagBubbleDefence)
            {
                damage = (ushort)HUtil32.Round(damage / 1.0e2 * (MagBubbleDefenceLevel + 2) * 8.0);//魔法盾减伤
                DamageBubbleDefence(damage);
            }
            return damage;
        }

        public override ushort GetFeatureEx()
        {
            return HUtil32.MakeWord(OnHorse ? HorseType : (byte)0, DressEffType);
        }

        /// <summary>
        /// 气血石和魔血石
        /// </summary>
        internal void PlaySuperRock()
        {
            if (!Death && Race == ActorRace.Play || Race == ActorRace.PlayClone)
            {
                if (UseItems.Length >= ItemLocation.Charm && UseItems[ItemLocation.Charm] != null && UseItems[ItemLocation.Charm].Index > 0)
                {
                    StdItem StdItem = ModuleShare.ItemSystem.GetStdItem(UseItems[ItemLocation.Charm].Index);
                    if ((StdItem.StdMode == 7) && (StdItem.Shape == 2 || StdItem.Shape == 3))
                    {
                        ushort stoneDura;
                        ushort dCount;
                        ushort bCount;
                        // 加HP
                        if ((IncHealth == 0) && (UseItems[ItemLocation.Charm].Index > 0) && ((HUtil32.GetTickCount() - IncHpStoneTime) > ModuleShare.Config.HPStoneIntervalTime) && ((WAbil.HP / WAbil.MaxHP * 100) < ModuleShare.Config.HPStoneStartRate))
                        {
                            IncHpStoneTime = HUtil32.GetTickCount();
                            stoneDura = (ushort)(UseItems[ItemLocation.Charm].Dura * 10);
                            bCount = (ushort)(stoneDura / ModuleShare.Config.HPStoneAddRate);
                            dCount = (ushort)(WAbil.MaxHP - WAbil.HP);
                            if (dCount > bCount)
                            {
                                dCount = bCount;
                            }
                            if (stoneDura > dCount)
                            {
                                IncHealth += dCount;
                                UseItems[ItemLocation.Charm].Dura -= (ushort)HUtil32.Round(dCount / 10.0);
                            }
                            else
                            {
                                stoneDura = 0;
                                IncHealth += stoneDura;
                                UseItems[ItemLocation.Charm].Dura = 0;
                            }
                            if (UseItems[ItemLocation.Charm].Dura >= 1000)
                            {
                                if (Race == ActorRace.Play)
                                {
                                    SendMsg(Messages.RM_DURACHANGE, ItemLocation.Charm, UseItems[ItemLocation.Charm].Dura, UseItems[ItemLocation.Charm].DuraMax, 0);
                                }
                            }
                            else
                            {
                                UseItems[ItemLocation.Charm].Dura = 0;
                                if (Race == ActorRace.Play)
                                {
                                    ((PlayObject)this).SendDelItems(UseItems[ItemLocation.Charm]);
                                }
                                UseItems[ItemLocation.Charm].Index = 0;
                            }
                        }
                        // 加MP
                        if ((IncSpell == 0) && (UseItems[ItemLocation.Charm].Index > 0) && ((HUtil32.GetTickCount() - IncMpStoneTime) > ModuleShare.Config.MpStoneIntervalTime) && ((WAbil.MP / WAbil.MaxMP * 100) < ModuleShare.Config.MPStoneStartRate))
                        {
                            IncMpStoneTime = HUtil32.GetTickCount();
                            stoneDura = (ushort)(UseItems[ItemLocation.Charm].Dura * 10);
                            bCount = (ushort)(stoneDura / ModuleShare.Config.MPStoneAddRate);
                            dCount = (ushort)(WAbil.MaxMP - WAbil.MP);
                            if (dCount > bCount)
                            {
                                dCount = bCount;
                            }
                            if (stoneDura > dCount)
                            {
                                IncSpell += dCount;
                                UseItems[ItemLocation.Charm].Dura -= (ushort)HUtil32.Round(dCount / 10.0);
                            }
                            else
                            {
                                stoneDura = 0;
                                IncSpell += stoneDura;
                                UseItems[ItemLocation.Charm].Dura = 0;
                            }
                            if (UseItems[ItemLocation.Charm].Dura >= 1000)
                            {
                                if (Race == ActorRace.Play)
                                {
                                    SendMsg(Messages.RM_DURACHANGE, ItemLocation.Charm, UseItems[ItemLocation.Charm].Dura, UseItems[ItemLocation.Charm].DuraMax, 0);
                                }
                            }
                            else
                            {
                                UseItems[ItemLocation.Charm].Dura = 0;
                                if (Race == ActorRace.Play)
                                {
                                    ((PlayObject)this).SendDelItems(UseItems[ItemLocation.Charm]);
                                }
                                UseItems[ItemLocation.Charm].Index = 0;
                            }
                        }
                    }
                }
            }
        }
    }
}