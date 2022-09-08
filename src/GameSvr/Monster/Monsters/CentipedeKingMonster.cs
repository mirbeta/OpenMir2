using SystemModule;

namespace GameSvr.Monster.Monsters
{
    public class CentipedeKingMonster : StickMonster
    {
        private int _attickTick;

        public CentipedeKingMonster() : base()
        {
            ViewRange = 6;
            ComeOutValue = 4;
            AttackRange = 6;
            Animal = false;
            _attickTick = HUtil32.GetTickCount();
        }

        private bool CheckAttackTarget()
        {
            var result = false;
            for (var i = 0; i < VisibleActors.Count; i++)
            {
                var baseObject = VisibleActors[i].BaseObject;
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
                var wAbil = Abil;
                var nPower = M2Share.RandomNumber.Random(HUtil32.HiWord(wAbil.DC) - HUtil32.LoWord(wAbil.DC) + 1) + HUtil32.LoWord(wAbil.DC);
                for (var i = 0; i < VisibleActors.Count; i++)
                {
                    var baseObject = VisibleActors[i].BaseObject;
                    if (baseObject.Death)
                    {
                        continue;
                    }
                    if (IsProperTarget(baseObject))
                    {
                        if (Math.Abs(CurrX - baseObject.CurrX) < ViewRange && Math.Abs(CurrY - baseObject.CurrY) < ViewRange)
                        {
                            TargetFocusTick = HUtil32.GetTickCount();
                            SendDelayMsg(this, Grobal2.RM_DELAYMAGIC, (short)nPower, HUtil32.MakeLong(baseObject.CurrX, baseObject.CurrY), 2, baseObject.ObjectId, "", 600);
                            if (M2Share.RandomNumber.Random(4) == 0)
                            {
                                if (M2Share.RandomNumber.Random(3) != 0)
                                {
                                    baseObject.MakePosion(Grobal2.POISON_DECHEALTH, 60, 3);
                                }
                                else
                                {
                                    baseObject.MakePosion(Grobal2.POISON_STONE, 5, 0);
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
            Abil.HP = Abil.MaxHP;
        }

        public override void Run()
        {
            if (CanWalk())
            {
                if ((HUtil32.GetTickCount() - WalkTick) > WalkSpeed)
                {
                    WalkTick = HUtil32.GetTickCount();
                    if (FixedHideMode)
                    {
                        if ((HUtil32.GetTickCount() - _attickTick) > 10000)
                        {
                            for (var i = 0; i < VisibleActors.Count; i++)
                            {
                                var baseObject = VisibleActors[i].BaseObject;
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
                                            _attickTick = HUtil32.GetTickCount();
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if ((HUtil32.GetTickCount() - _attickTick) > 3000)
                        {
                            if (AttackTarget())
                            {
                                base.Run();
                                return;
                            }
                            if ((HUtil32.GetTickCount() - _attickTick) > 10000)
                            {
                                ComeDown();
                                _attickTick = HUtil32.GetTickCount();
                            }
                        }
                    }
                }
            }
            base.Run();
        }
    }
}

