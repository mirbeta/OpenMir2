using SystemModule.Consts;

namespace GameSvr.Monster.Monsters
{
    public class CentipedeKingMonster : StickMonster
    {
        private int _attackTick;

        public CentipedeKingMonster() : base()
        {
            ViewRange = 6;
            ComeOutValue = 4;
            AttackRange = 6;
            Animal = false;
            _attackTick = HUtil32.GetTickCount();
        }

        private bool CheckAttackTarget()
        {
            bool result = false;
            for (int i = 0; i < VisibleActors.Count; i++)
            {
                Actor.BaseObject baseObject = VisibleActors[i].BaseObject;
                if (baseObject.Death)
                {
                    continue;
                }
                if (IsProperTarget(baseObject))
                {
                    if (Math.Abs(CurrX - baseObject.CurrX) <= ViewRange && Math.Abs(CurrY - baseObject.CurrY) <= ViewRange)
                    {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }

        protected override bool AttackTarget()
        {
            if (!CheckAttackTarget())
            {
                return false;
            }
            if ((HUtil32.GetTickCount() - AttackTick) > NextHitTime)
            {
                AttackTick = HUtil32.GetTickCount();
                SendAttackMsg(Grobal2.RM_HIT, Direction, CurrX, CurrY);
                int nPower = HUtil32._MAX(0, HUtil32.LoByte(WAbil.DC) + M2Share.RandomNumber.Random(Math.Abs(HUtil32.HiByte(WAbil.DC) - HUtil32.LoByte(WAbil.DC)) + 1));
                for (int i = 0; i < VisibleActors.Count; i++)
                {
                    Actor.BaseObject baseObject = VisibleActors[i].BaseObject;
                    if (baseObject.Death)
                    {
                        continue;
                    }
                    if (IsProperTarget(baseObject))
                    {
                        if (Math.Abs(CurrX - baseObject.CurrX) < ViewRange && Math.Abs(CurrY - baseObject.CurrY) < ViewRange)
                        {
                            TargetFocusTick = HUtil32.GetTickCount();
                            SendDelayMsg(this, Grobal2.RM_DELAYMAGIC, nPower, HUtil32.MakeLong(baseObject.CurrX, baseObject.CurrY), 2, baseObject.ActorId, "", 600);
                            if (M2Share.RandomNumber.Random(4) == 0)
                            {
                                if (M2Share.RandomNumber.Random(3) != 0)
                                {
                                    baseObject.MakePosion(PoisonState.DECHEALTH, 60, 3);
                                }
                                else
                                {
                                    baseObject.MakePosion(PoisonState.STONE, 5, 0);
                                }
                                TargetCret = baseObject;
                            }
                        }
                    }
                }
            }
            return true;
        }

        protected override void FindAttackTarget()
        {
            base.FindAttackTarget();
            WAbil.HP = WAbil.MaxHP;
        }

        public override void Run()
        {
            if (CanMove())
            {
                if ((HUtil32.GetTickCount() - WalkTick) > WalkSpeed)
                {
                    WalkTick = HUtil32.GetTickCount();
                    if (FixedHideMode)
                    {
                        if ((HUtil32.GetTickCount() - _attackTick) > 10000)
                        {
                            for (int i = 0; i < VisibleActors.Count; i++)
                            {
                                Actor.BaseObject baseObject = VisibleActors[i].BaseObject;
                                if (baseObject.Death)
                                {
                                    continue;
                                }
                                if (IsProperTarget(baseObject))
                                {
                                    if (!baseObject.HideMode || CoolEye)
                                    {
                                        if (Math.Abs(CurrX - baseObject.CurrX) < ComeOutValue && Math.Abs(CurrY - baseObject.CurrY) < ComeOutValue)
                                        {
                                            FindAttackTarget();
                                            _attackTick = HUtil32.GetTickCount();
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if ((HUtil32.GetTickCount() - _attackTick) > 3000)
                        {
                            if (AttackTarget())
                            {
                                base.Run();
                                return;
                            }
                            if ((HUtil32.GetTickCount() - _attackTick) > 10000)
                            {
                                ComeDown();
                                _attackTick = HUtil32.GetTickCount();
                            }
                        }
                    }
                }
            }
            base.Run();
        }
    }
}

