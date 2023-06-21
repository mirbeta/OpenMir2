using System.Collections.Concurrent;
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
    public class ActorStateSystem
    {
        private IList<ActorBuffState> Actors = new List<ActorBuffState>();

        public void AddBuff(IActor actor, BuffStateType buffState, int duraTime, int stateval)
        {
            Actors.Add(new ActorBuffState()
            {
                ActorId = actor.ActorId,
                StateType = buffState,
                StateVal = stateval,
                StateTick = HUtil32.GetTickCount() + duraTime
            });
        }

        /// <summary>
        /// 消息处理
        /// </summary>
        public void Operate(IActor actor)
        {
           /* bool boChg = false;
            var boNeedRecalc = false;
            for (int i = 0; i < Actors.Count; i++)
            {
                if ((Actors[i].StateTick > 0) && (Actors[i].StateTick < 60000))
                {
                    if ((HUtil32.GetTickCount() - StatusArrTick[i]) > 1000)
                    {
                        Actors[i].StateTick -= 1;
                        StatusArrTick[i] += 1000;
                        if (actor.Race == ActorRace.Play)
                        {
                            if (Actors[i].StateTick == 0)
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
                            else if (Actors[i].StateTick == 10)
                            {
                                if (i == PoisonState.DefenceUP)
                                {
                                    ((IPlayerActor)actor).SysMsg($"防御力{Actors[i].StateTick}秒后恢复正常。", MsgColor.Green, MsgType.Hint);
                                    break;
                                }

                                if (i == PoisonState.MagDefenceUP)
                                {
                                    ((IPlayerActor)actor).SysMsg($"魔法防御力{Actors[i].StateTick}秒后恢复正常。", MsgColor.Green, MsgType.Hint);
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

            if ((HUtil32.GetTickCount() - actor.PoisoningTick) > SystemShare.Config.PosionDecHealthTime)
            {
                actor.PoisoningTick = HUtil32.GetTickCount();
                if (StatusTimeArr[PoisonState.DECHEALTH] > 0)
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
            }*/
        }

        public int GetBuff(IActor actor, BuffStateType buffState)
        {
            for (int i = 0; i < Actors.Count; i++)
            {
                if (Actors[i].ActorId == actor.ActorId && Actors[i].StateType == buffState)
                {
                    return i + 1;
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

    public struct ActorBuffState
    {
        public int ActorId { get; set; }
        public BuffStateType StateType { get; set; }
        public int StateVal { get; set; }
        public int StateTime { get; set; }
        public int StateTick { get; set; }
        /// <summary>
        /// 销毁时间
        /// </summary>
        public int DestroyTick { get; set; }
    }
}