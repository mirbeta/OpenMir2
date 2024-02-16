using M2Server.Actor;
using OpenMir2;
using OpenMir2.Consts;
using OpenMir2.Data;
using OpenMir2.Enums;
using OpenMir2.Packets.ClientPackets;
using SystemModule;
using SystemModule.Actors;

namespace M2Server.Player
{
    public class CharacterObject : AnimalObject
    {
        /// <summary>
        /// 攻击速度
        /// </summary>
        public ushort HitSpeed { get; set; }
        /// <summary>
        /// HP恢复点数
        /// </summary>
        protected ushort HealthRecover { get; set; }
        /// <summary>
        /// MP恢复点数
        /// </summary>
        protected ushort SpellRecover { get; set; }
        /// <summary>
        /// 中毒恢复点数
        /// </summary>
        protected ushort PoisonRecover { get; set; }
        protected byte PerHealth { get; set; }
        protected byte PerHealing { get; set; }
        protected byte PerSpell { get; set; }
        /// <summary>
        /// 增加血量的间隔
        /// </summary>
        protected int IncHealthSpellTick { get; set; }
        /// <summary>
        /// 身上物品
        /// </summary>
        public UserItem[] UseItems { get; set; }
        /// <summary>
        /// 攻击状态
        /// </summary>
        public AttackMode AttatckMode { get; set; }
        /// <summary>
        /// 衣服特效(如天外飞仙衣服效果)
        /// </summary>
        public byte DressEffType { get; set; }
        /// <summary>
        /// 马类型
        /// </summary>
        public byte HorseType { get; set; }
        /// <summary>
        /// 骑马
        /// </summary>
        public bool OnHorse { get; set; }
        /// <summary>
        /// 魔法盾
        /// </summary>
        public bool AbilMagBubbleDefence { get; set; }
        /// <summary>
        /// 魔法盾等级
        /// </summary>
        public byte MagBubbleDefenceLevel { get; set; }
        /// <summary>
        /// 气血石时间
        /// </summary>
        public int IncHpStoneTime { get; set; }
        /// <summary>
        /// 魔血石时间
        /// </summary>
        public int IncMpStoneTime { get; set; }
        public ushort IncHealth { get; set; }
        public ushort IncSpell { get; set; }
        public ushort IncHealing { get; set; }
        /// <summary>
        /// 死亡是不是掉装备
        /// </summary>
        public bool NoDropUseItem { get; set; }

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
            M2Share.ActorBuffSystem.AddBuff(this, BuffType.MagicShield, nSec, nSec);
            //StatusTimeArr[PoisonState.BubbleDefenceUP] = nSec;
            //StatusArrTick[PoisonState.BubbleDefenceUP] = HUtil32.GetTickCount();
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
            int damage = base.GetMagStruckDamage(baseObject, nDamage);
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
        public void PlaySuperRock()
        {
            if (!Death && Race == ActorRace.Play || Race == ActorRace.PlayClone)
            {
                if (UseItems.Length >= ItemLocation.Charm && UseItems[ItemLocation.Charm] != null && UseItems[ItemLocation.Charm].Index > 0)
                {
                    StdItem StdItem = SystemShare.EquipmentSystem.GetStdItem(UseItems[ItemLocation.Charm].Index);
                    if ((StdItem.StdMode == 7) && (StdItem.Shape == 2 || StdItem.Shape == 3))
                    {
                        ushort stoneDura;
                        ushort dCount;
                        ushort bCount;
                        // 加HP
                        if ((IncHealth == 0) && (UseItems[ItemLocation.Charm].Index > 0) && ((HUtil32.GetTickCount() - IncHpStoneTime) > SystemShare.Config.HPStoneIntervalTime) && ((WAbil.HP / WAbil.MaxHP * 100) < SystemShare.Config.HPStoneStartRate))
                        {
                            IncHpStoneTime = HUtil32.GetTickCount();
                            stoneDura = (ushort)(UseItems[ItemLocation.Charm].Dura * 10);
                            bCount = (ushort)(stoneDura / SystemShare.Config.HPStoneAddRate);
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
                                    ((IPlayerActor)this).SendDelItems(UseItems[ItemLocation.Charm]);
                                }
                                UseItems[ItemLocation.Charm].Index = 0;
                            }
                        }
                        // 加MP
                        if ((IncSpell == 0) && (UseItems[ItemLocation.Charm].Index > 0) && ((HUtil32.GetTickCount() - IncMpStoneTime) > SystemShare.Config.MpStoneIntervalTime) && ((WAbil.MP / WAbil.MaxMP * 100) < SystemShare.Config.MPStoneStartRate))
                        {
                            IncMpStoneTime = HUtil32.GetTickCount();
                            stoneDura = (ushort)(UseItems[ItemLocation.Charm].Dura * 10);
                            bCount = (ushort)(stoneDura / SystemShare.Config.MPStoneAddRate);
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
                                    ((IPlayerActor)this).SendDelItems(UseItems[ItemLocation.Charm]);
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