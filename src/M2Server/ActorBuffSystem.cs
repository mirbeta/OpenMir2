using M2Server.Actor;
using OpenMir2;
using OpenMir2.Enums;
using SystemModule;
using SystemModule.Actors;
using SystemModule.Enums;

namespace M2Server
{
    public enum BuffType : byte
    {
        /// <summary>
        /// 绿毒
        /// </summary>
        GreenPoison = 0,
        /// <summary>
        /// 红毒
        /// </summary>
        RedPoison = 1,
        Defense = 2,
        /// <summary>
        /// 不能走动
        /// </summary>
        CanNotRun = 3,
        /// <summary>
        /// 不能移动
        /// </summary>
        CanNotMove = 4,
        Paralysis = 5,
        ReduceHP = 6,
        Frozen = 7,
        /// <summary>
        /// 魔法隐身
        /// </summary>
        Transparent = 8,
        /// <summary>
        /// 防御力
        /// </summary>
        DefensePower = 9,
        /// <summary>
        /// 魔法防御力
        /// </summary>
        MagicDefensePower = 10,
        /// <summary>
        /// 魔法盾
        /// </summary>
        MagicShield = 11
    }

    /// <summary>
    /// 精灵状态系统
    /// </summary>
    public class ActorBuffSystem
    {
        private readonly Dictionary<int, IList<ActorBuffData>> ActorBuffMap = new Dictionary<int, IList<ActorBuffData>>();
        private readonly IList<IActor> ActorBuffs = new List<IActor>();

        public void DoWork(object obj)
        {
            if (ActorBuffs.Any())
            {
                Operate();
                int currentTickCount = HUtil32.GetTickCount();
                int idx = 0;
                int actorCount = ActorBuffs.Count;
                while (true)
                {
                    if (idx >= actorCount)
                    {
                        break;
                    }
                    IActor actor = ActorBuffs[idx];
                    if (ActorBuffMap.TryGetValue(actor.ActorId, out IList<ActorBuffData> actorBuffs))
                    {
                        for (int j = 0; j < actorBuffs.Count; j++)
                        {
                            if (currentTickCount >= actorBuffs[j].DestroyTick)
                            {
                                actorBuffs.RemoveAt(j);
                            }
                        }
                    }
                    if (actorBuffs != null && actorBuffs.Count == 0)
                    {
                        ActorBuffs.RemoveAt(idx);
                        actorCount--;
                    }
                    idx++;
                }
            }
        }

        /// <summary>
        /// 给对象附加效果
        /// </summary>
        /// <param name="actor">对象</param>
        /// <param name="buffType">效果类型</param>
        /// <param name="duraTime">持续时长</param>
        /// <param name="stateval">伤害效果</param>
        public void AddBuff(IActor actor, BuffType buffType, int duraTime, int stateval)
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

            if (ActorBuffMap.TryGetValue(actor.ActorId, out IList<ActorBuffData> buffs))
            {
                if (buffs.FirstOrDefault(x => x.BuffType == buffType) == null) // 不存在就直接添加,否则直接更新时间
                {
                    buffs.Add(new ActorBuffData()
                    {
                        ActorId = actor.ActorId,
                        BuffType = buffType,
                        StateVal = stateval,
                        StateTick = duraTime,
                        DestroyTick = HUtil32.GetTickCount() + 40000
                    });
                }
                else
                {
                    for (int i = 0; i < buffs.Count; i++)
                    {
                        if (buffs[i].BuffType != buffType)
                        {
                            continue;
                        }

                        buffs[i].StateVal = stateval;
                        buffs[i].StateTick = duraTime;
                        buffs[i].DestroyTick = HUtil32.GetTickCount() + 40000;
                    }
                }
            }
            else
            {
                ActorBuffData buffState = new ActorBuffData()
                {
                    ActorId = actor.ActorId,
                    BuffType = buffType,
                    StateVal = stateval,
                    StateTick = duraTime,
                    DestroyTick = HUtil32.GetTickCount() + 40000
                };
                ActorBuffMap.Add(actor.ActorId, new List<ActorBuffData>() { buffState });
            }

            if (!ActorBuffs.Contains(actor))
            {
                ActorBuffs.Add(actor);
            }
        }

        /// <summary>
        /// 消息处理
        /// </summary>
        private void Operate()
        {
            bool boChg = false;
            bool boNeedRecalc = false;
            for (int i = 0; i < ActorBuffs.Count; i++)
            {
                IActor actor = ActorBuffs[i];
                if (actor.Race == ActorRace.Play)
                {
                    if (ActorBuffMap.TryGetValue(actor.ActorId, out IList<ActorBuffData> actorBuffs))
                    {
                        for (int j = 0; j < actorBuffs.Count; j++)
                        {
                            if ((actorBuffs[j].StateTick > 0) && (actorBuffs[j].StateTick < 60000))
                            {
                                if ((HUtil32.GetTickCount() - actorBuffs[j].StatusArrTick) > 1000)
                                {
                                    actorBuffs[j].StateTick -= 1;
                                    actorBuffs[j].StatusArrTick += 1000;

                                    if (actorBuffs[j].StateTick == 0)
                                    {
                                        boChg = true;
                                        switch (actorBuffs[j].BuffType)
                                        {
                                            case BuffType.DefensePower:
                                                boNeedRecalc = true;
                                                ((IPlayerActor)actor).SysMsg("防御力回复正常.", MsgColor.Green, MsgType.Hint);
                                                break;
                                            case BuffType.MagicDefensePower:
                                                boNeedRecalc = true;
                                                ((IPlayerActor)actor).SysMsg("魔法防御力回复正常.", MsgColor.Green, MsgType.Hint);
                                                break;
                                            case BuffType.Transparent:
                                                actor.HideMode = false;
                                                break;
                                        }
                                    }
                                    else if (actorBuffs[j].StateTick == 10)
                                    {
                                        if (actorBuffs[j].BuffType == BuffType.DefensePower)
                                        {
                                            ((IPlayerActor)actor).SysMsg($"防御力{actorBuffs[j].StateTick}秒后恢复正常。", MsgColor.Green, MsgType.Hint);
                                            break;
                                        }
                                        if (actorBuffs[j].BuffType == BuffType.MagicDefensePower)
                                        {
                                            ((IPlayerActor)actor).SysMsg($"魔法防御力{actorBuffs[j].StateTick}秒后恢复正常。", MsgColor.Green, MsgType.Hint);
                                            break;
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
                }

                if (!actor.Death)
                {
                    if ((HUtil32.GetTickCount() - actor.PoisoningTick) > SystemShare.Config.PosionDecHealthTime)
                    {
                        actor.PoisoningTick = HUtil32.GetTickCount();

                        if (ActorBuffMap.TryGetValue(actor.ActorId, out IList<ActorBuffData> actorBuff))
                        {
                            for (int j = 0; j < actorBuff.Count; j++)
                            {
                                if (actorBuff[j].BuffType == BuffType.GreenPoison)
                                {
                                    if (actorBuff[j].StateTick > 0)
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
        }

        public int GetBuff(IActor actor, BuffType buffType)
        {
            if (!ActorBuffMap.TryGetValue(actor.ActorId, out IList<ActorBuffData> actorBuffs))
            {
                return 0;
            }

            for (int i = 0; i < actorBuffs.Count; i++)
            {
                if (actorBuffs[i].ActorId == actor.ActorId && actorBuffs[i].BuffType == buffType)
                {
                    return i + 1;
                }
            }
            return 0;
        }

        public int GetBuffStatus(IActor actor)
        {
            int nStatus = 0;
            if (ActorBuffMap.TryGetValue(actor.ActorId, out IList<ActorBuffData> actorBuffs))
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

        public bool HasBuff(IActor actor, BuffType buffType)
        {
            if (!ActorBuffMap.TryGetValue(actor.ActorId, out IList<ActorBuffData> actorBuffs))
            {
                return false;
            }

            for (int i = 0; i < actorBuffs.Count; i++)
            {
                if (actorBuffs[i].ActorId == actor.ActorId && actorBuffs[i].BuffType == buffType)
                {
                    return true;
                }
            }
            return false;
        }

        public bool TryUpdate(IActor actor, BuffType buffType, int duraTime)
        {
            if (!ActorBuffMap.TryGetValue(actor.ActorId, out IList<ActorBuffData> actorBuffs))
            {
                return false;
            }

            for (int i = 0; i < actorBuffs.Count; i++)
            {
                if (actorBuffs[i].ActorId == actor.ActorId && actorBuffs[i].BuffType == buffType)
                {
                    actorBuffs[i].StateTick = duraTime;
                    return true;
                }
            }
            return false;
        }
    }

    public class ActorBuffData
    {
        public int ActorId { get; set; }
        public BuffType BuffType { get; set; }
        public int StateVal { get; set; }
        public int StateTick { get; set; }
        public int StatusArrTick { get; set; }
        /// <summary>
        /// 销毁时间
        /// </summary>
        public int DestroyTick { get; set; }
    }
}