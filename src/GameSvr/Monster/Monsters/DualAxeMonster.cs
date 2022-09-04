using GameSvr.Actor;
using SystemModule;

namespace GameSvr.Monster.Monsters
{
    public class DualAxeMonster : MonsterObject
    {
        private int m_nAttackCount = 0;
        protected int m_nAttackMax = 0;

        private void FlyAxeAttack(TBaseObject Target)
        {
            if (Envir.CanFly(CurrX, CurrY, Target.CurrX, Target.CurrY))
            {
                Direction = M2Share.GetNextDirection(CurrX, CurrY, Target.CurrX, Target.CurrY);
                var WAbil = m_WAbil;
                var nDamage = M2Share.RandomNumber.Random(HUtil32.HiWord(WAbil.DC) - HUtil32.LoWord(WAbil.DC) + 1) + HUtil32.LoWord(WAbil.DC);
                if (nDamage > 0)
                {
                    nDamage = Target.GetHitStruckDamage(this, nDamage);
                }
                if (nDamage > 0)
                {
                    Target.StruckDamage(nDamage);
                    Target.SendDelayMsg(Grobal2.RM_STRUCK, Grobal2.RM_10101, (short)nDamage, Target.m_WAbil.HP, Target.m_WAbil.MaxHP, ObjectId, "", HUtil32._MAX(Math.Abs(CurrX - Target.CurrX), Math.Abs(CurrY - Target.CurrY)) * 50 + 600);
                }
                SendRefMsg(Grobal2.RM_FLYAXE, Direction, CurrX, CurrY, Target.ObjectId, "");
            }
        }

        protected override bool AttackTarget()
        {
            var result = false;
            if (TargetCret == null)
            {
                return result;
            }
            if ((HUtil32.GetTickCount() - AttackTick) > NextHitTime)
            {
                AttackTick = HUtil32.GetTickCount();
                if (Math.Abs(CurrX - TargetCret.CurrX) <= 7 && Math.Abs(CurrX - TargetCret.CurrX) <= 7)
                {
                    if (m_nAttackMax - 1 > m_nAttackCount)
                    {
                        m_nAttackCount++;
                        TargetFocusTick = HUtil32.GetTickCount();
                        FlyAxeAttack(TargetCret);
                    }
                    else
                    {
                        if (M2Share.RandomNumber.Random(5) == 0)
                        {
                            m_nAttackCount = 0;
                        }
                    }
                    result = true;
                    return result;
                }
                if (TargetCret.Envir == Envir)
                {
                    if (Math.Abs(CurrX - TargetCret.CurrX) <= 11 && Math.Abs(CurrX - TargetCret.CurrX) <= 11)
                    {
                        SetTargetXY(TargetCret.CurrX, TargetCret.CurrY);
                    }
                }
                else
                {
                    DelTargetCreat();
                }
            }
            return result;
        }

        public DualAxeMonster() : base()
        {
            ViewRange = 5;
            m_nRunTime = 250;
            SearchTime = 3000;
            m_nAttackCount = 0;
            m_nAttackMax = 2;
            SearchTick = HUtil32.GetTickCount();
        }

        public override void Run()
        {
            int nAbs;
            int nRage = 9999;
            TBaseObject BaseObject;
            TBaseObject TargetBaseObject = null;
            if (!Death && !Ghost && m_wStatusTimeArr[Grobal2.POISON_STONE] == 0)
            {
                if ((HUtil32.GetTickCount() - SearchEnemyTick) >= 5000)
                {
                    SearchEnemyTick = HUtil32.GetTickCount();
                    for (var i = 0; i < VisibleActors.Count; i++)
                    {
                        BaseObject = VisibleActors[i].BaseObject;
                        if (BaseObject.Death)
                        {
                            continue;
                        }
                        if (IsProperTarget(BaseObject))
                        {
                            if (!BaseObject.HideMode || CoolEye)
                            {
                                nAbs = Math.Abs(CurrX - BaseObject.CurrX) + Math.Abs(CurrY - BaseObject.CurrY);
                                if (nAbs < nRage)
                                {
                                    nRage = nAbs;
                                    TargetBaseObject = BaseObject;
                                }
                            }
                        }
                    }
                    if (TargetBaseObject != null)
                    {
                        SetTargetCreat(TargetBaseObject);
                    }
                }
                if ((HUtil32.GetTickCount() - WalkTick) > WalkSpeed && TargetCret != null)
                {
                    if (Math.Abs(CurrX - TargetCret.CurrX) <= 4 && Math.Abs(CurrX - TargetCret.CurrX) <= 4)
                    {
                        if (Math.Abs(CurrX - TargetCret.CurrX) <= 2 && Math.Abs(CurrX - TargetCret.CurrX) <= 2)
                        {
                            if (M2Share.RandomNumber.Random(5) == 0)
                            {
                                GetBackPosition(ref TargetX, ref TargetY);
                            }
                        }
                        else
                        {
                            GetBackPosition(ref TargetX, ref TargetY);
                        }
                    }
                }
            }
            base.Run();
        }
    }
}

