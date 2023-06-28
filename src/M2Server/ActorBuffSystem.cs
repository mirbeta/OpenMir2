using M2Server.Actor;
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
    public class ActorBuffSystem
    {
        private readonly Dictionary<int, IList<ActorBuffState>> ActorBuffMap = new Dictionary<int, IList<ActorBuffState>>();
        private readonly Timer _buffTimer;
        private readonly IList<IActor> ActorBuffs = new List<IActor>();

        public ActorBuffSystem()
        {
            //_buffTimer = new Timer(DoWork, null, 10000, 100);
        }

        public void DoWork(object obj)
        {
            if (ActorBuffs.Any())
            {
                //todo 检查Buff是否到期
                Operate();
                var currentTickCount = HUtil32.GetTickCount();
                var idx = 0;
                var actorCount = ActorBuffs.Count;
                while (true)
                {
                    if (idx >= actorCount)
                    {
                        break;
                    }
                    var actor = ActorBuffs[idx];
                    if (ActorBuffMap.TryGetValue(actor.ActorId, out var actorBuffs))
                    {
                        for (int j = 0; j < actorBuffs.Count; j++)
                        {
                            if (currentTickCount >= actorBuffs[j].DestroyTick)
                            {
                                actorBuffs.RemoveAt(j);
                            }
                        }
                    }
                    if (actorBuffs.Count == 0)
                    {
                        ActorBuffs.RemoveAt(idx);
                    }
                    idx++;
                }
            }
        }

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

            //todo buff不可叠加

            if (ActorBuffMap.TryGetValue(actor.ActorId, out var buffs))
            {
                buffs.Add(new ActorBuffState()
                {
                    ActorId = actor.ActorId,
                    StateType = buffType,
                    StateVal = stateval,
                    StateTick = HUtil32.GetTickCount() + duraTime,
                    DestroyTick = HUtil32.GetTickCount() + 40000
                });
            }
            else
            {
                var buffState = new ActorBuffState()
                {
                    ActorId = actor.ActorId,
                    StateType = buffType,
                    StateVal = stateval,
                    StateTick = HUtil32.GetTickCount() + duraTime,
                    DestroyTick = HUtil32.GetTickCount() + 40000
                };
                ActorBuffMap.Add(actor.ActorId, new List<ActorBuffState>() { buffState });
            }

            if (!ActorBuffs.Contains(actor))
            { 
                ActorBuffs.Add(actor);
            }
        }

        /// <summary>
        /// 消息处理
        /// </summary>
        public void Operate()
        {
            bool boChg = false;
            var boNeedRecalc = false;
            for (int i = 0; i < ActorBuffs.Count; i++)
            {
                var actor = ActorBuffs[i];
                if (ActorBuffMap.TryGetValue(actor.ActorId, out var actorBuffs))
                {
                    for (int j = 0; j < actorBuffs.Count; j++)
                    {
                        if ((actorBuffs[j].StateTick > 0) && (actorBuffs[j].StateTick < 60000))
                        {
                            if ((HUtil32.GetTickCount() - actorBuffs[j].StatusArrTick) > 1000)
                            {
                                actorBuffs[j].StateTick -= 1;
                                actorBuffs[j].StatusArrTick += 1000;
                                if (actor.Race == ActorRace.Play)
                                {
                                    if (actorBuffs[j].StateTick == 0)
                                    {
                                        boChg = true;
                                        switch (j)
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
                                    else if (actorBuffs[j].StateTick == 10)
                                    {
                                        if (actorBuffs[j].StateType == BuffStateType.DefensePower)
                                        {
                                            ((IPlayerActor)actor).SysMsg($"防御力{actorBuffs[j].StateTick}秒后恢复正常。", MsgColor.Green, MsgType.Hint);
                                            break;
                                        }
                                        if (actorBuffs[j].StateType == BuffStateType.MagicDefensePower)
                                        {
                                            ((IPlayerActor)actor).SysMsg($"魔法防御力{actorBuffs[j].StateTick}秒后恢复正常。", MsgColor.Green, MsgType.Hint);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

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
                        for (int j = 0; j < actorBuff.Count; j++)
                        {
                            if (actorBuff[j].StateType == BuffStateType.GreenPoison)
                            {
                                //if (StatusTimeArr[PoisonState.DECHEALTH] > 0)
                                if (actorBuff[j].StateVal > 0)
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

        public int GetBuffStatus(IActor actor)
        {
            int nStatus = 0;
            if (ActorBuffMap.TryGetValue(actor.ActorId, out var actorBuffs))
            {
                for (int i = 0; i < actorBuffs.Count; i++)
                {
                    if (actorBuffs[i].StateTick > 0)
                    {
                        nStatus = (int)(nStatus | (0x80000000 >> i));
                    }
                }
            }
            return nStatus;
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