﻿using M2Server.Actor;
using SystemModule;
using SystemModule.Consts;
using SystemModule.Enums;

namespace M2Server
{
    public enum BuffStateType : byte
    {
        GreenPoison = 0,
        RedPoison = 1,
        Defense = 2,
        CanNotRun = 3,
        CanNotMove = 4,
        Paralysis = 5,
        ReduceHP = 6,
        Frozen = 7,
        Hide = 8,
        DefensePower = 9,
        MagicDefensePower = 10,
        MagicShield = 11
    }

    /// <summary>
    /// 精灵状态系统
    /// </summary>
    public class ActorStateSystem
    {
        private readonly Dictionary<int, IList<ActorBuffState>> ActorBuffMap = new Dictionary<int, IList<ActorBuffState>>();
        private readonly IList<int> ActorBuffs = new List<int>();

        public void AddBuff(IActor actor, BuffStateType buffType, int duraTime, int stateval)
        {
            //if (StatusTimeArr[nType] > 0)
            //{
            //    if (StatusTimeArr[nType] < nTime)
            //    {
            //        StatusTimeArr[nType] = nTime;
            //    }
            //}
            //else
            //{
            //    StatusTimeArr[nType] = nTime;
            //}

            if (ActorBuffMap.TryGetValue(actor.ActorId, out var buffs))
            {
                buffs.Add(new ActorBuffState()
                {
                    ActorId = actor.ActorId,
                    StateType = buffType,
                    StateVal = stateval,
                    StateTick = HUtil32.GetTickCount() + duraTime
                });
            }
            else
            {
                var buffState = new ActorBuffState()
                {
                    ActorId = actor.ActorId,
                    StateType = buffType,
                    StateVal = stateval,
                    StateTick = HUtil32.GetTickCount() + duraTime
                };
                ActorBuffMap.Add(actor.ActorId, new List<ActorBuffState>() { buffState });
            }
            ActorBuffs.Add(actor.ActorId);
        }

        /// <summary>
        /// 消息处理
        /// </summary>
        public void Operate(IActor actor)
        {
            bool boChg = false;
            var boNeedRecalc = false;
            //for (int i = 0; i < ActorBuffs.Count; i++)
            //{
            if (ActorBuffMap.TryGetValue(actor.ActorId, out var actorBuffs))
            {
                for (int i = 0; i < actorBuffs.Count; i++)
                {
                    if ((actorBuffs[i].StateTick > 0) && (actorBuffs[i].StateTick < 60000))
                    {
                        if ((HUtil32.GetTickCount() - actorBuffs[i].StatusArrTick) > 1000)
                        {
                            actorBuffs[i].StateTick -= 1;
                            actorBuffs[i].StatusArrTick += 1000;
                            if (actor.Race == ActorRace.Play)
                            {
                                if (actorBuffs[i].StateTick == 0)
                                {
                                    boChg = true;
                                    switch (i)
                                    {
                                        case PoisonState.DefenceUP:
                                            boNeedRecalc = true;
                                            ((IPlayerActor)actor).SysMsg("防御力回复正常.", MsgColor.Green, MsgType.Hint);
                                            break;
                                        case PoisonState.MagDefenceUP:
                                            boNeedRecalc = true;
                                            ((IPlayerActor)actor).SysMsg("魔法防御力回复正常.", MsgColor.Green, MsgType.Hint);
                                            break;
                                        case PoisonState.STATETRANSPARENT:
                                            actor.HideMode = false;
                                            break;
                                    }
                                }
                                else if (actorBuffs[i].StateTick == 10)
                                {
                                    if (actorBuffs[i].StateType == BuffStateType.DefensePower)
                                    {
                                        ((IPlayerActor)actor).SysMsg($"防御力{actorBuffs[i].StateTick}秒后恢复正常。", MsgColor.Green, MsgType.Hint);
                                        break;
                                    }
                                    if (actorBuffs[i].StateType == BuffStateType.MagicDefensePower)
                                    {
                                        ((IPlayerActor)actor).SysMsg($"魔法防御力{actorBuffs[i].StateTick}秒后恢复正常。", MsgColor.Green, MsgType.Hint);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            //}

            if (boChg)
            {
                actor.CharStatus = actor.GetCharStatus();
                actor.StatusChanged();
            }

            if (boNeedRecalc)
            {
                actor.RecalcAbilitys();
                actor.SendMsg(Messages.RM_ABILITY, 0, 0, 0, 0);
            }

            if ((HUtil32.GetTickCount() - actor.PoisoningTick) > SystemShare.Config.PosionDecHealthTime)
            {
                actor.PoisoningTick = HUtil32.GetTickCount();

                if (ActorBuffMap.TryGetValue(actor.ActorId, out var actorBuff))
                {
                    for (int i = 0; i < actorBuff.Count; i++)
                    {
                        if (actorBuff[i].StateType == BuffStateType.GreenPoison)
                        {
                            //if (StatusTimeArr[PoisonState.DECHEALTH] > 0)
                            if (actorBuff[i].StateVal > 0)
                            {
                                if (actor.Animal)
                                {
                                    ((AnimalObject)actor).MeatQuality -= 1000;
                                }

                                actor.DamageHealth(actor.GreenPoisoningPoint + 1);
                                actor.HealthTick = 0;
                                actor.SpellTick = 0;
                                actor.HealthSpellChanged();
                            }
                        }
                    }
                }
            }
        }

        public int GetBuff(IActor actor, BuffStateType buffState)
        {
            if (ActorBuffMap.TryGetValue(actor.ActorId, out var actorBuffs))
            {
                for (int i = 0; i < actorBuffs.Count; i++)
                {
                    if (actorBuffs[i].ActorId == actor.ActorId && actorBuffs[i].StateType == buffState)
                    {
                        return i + 1;
                    }
                }
            }
            return 0;
        }

        public void Remove()
        {

        }

        public bool HasBuff()
        {
            return true;
        }
    }

    public class ActorBuffState
    {
        public int ActorId { get; set; }
        public BuffStateType StateType { get; set; }
        public int StateVal { get; set; }
        public int StateTime { get; set; }
        public int StateTick { get; set; }
        public int StatusArrTick { get; set; }
        /// <summary>
        /// 销毁时间
        /// </summary>
        public int DestroyTick { get; set; }
    }
}