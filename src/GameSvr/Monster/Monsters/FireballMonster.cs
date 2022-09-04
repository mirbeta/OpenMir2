using SystemModule;

namespace GameSvr.Monster.Monsters
{
    public class FireballMonster : MagicMonster
    {
        public FireballMonster() : base()
        {
            m_dwSpellTick = HUtil32.GetTickCount();
            SearchTime = M2Share.RandomNumber.Random(1500) + 1500;
        }

        public override void Run()
        {
            if (!Death && !bo554 && !Ghost)
            {
                if (TargetCret != null)
                {
                    if (MagCanHitTarget(TargetCret.CurrX, TargetCret.CurrY, TargetCret))
                    {
                        if (IsProperTarget(TargetCret))
                        {
                            if (Math.Abs(m_nTargetX - CurrX) <= 8 && Math.Abs(m_nTargetY - CurrY) <= 8)
                            {
                                var nPower = M2Share.RandomNumber.Random(HUtil32.HiWord(m_WAbil.MC) - HUtil32.LoWord(m_WAbil.MC) + 1) + HUtil32.LoWord(m_WAbil.MC);
                                if (nPower > 0)
                                {
                                    var BaseObject = GetPoseCreate();
                                    if (BaseObject != null && IsProperTarget(BaseObject) && m_nAntiMagic > 0)
                                    {
                                        nPower = BaseObject.GetMagStruckDamage(this, nPower);
                                        if (nPower > 0)
                                        {
                                            BaseObject.StruckDamage(nPower);
                                            if ((HUtil32.GetTickCount() - m_dwSpellTick) > NextHitTime)
                                            {
                                                m_dwSpellTick = HUtil32.GetTickCount();
                                                SendRefMsg(Grobal2.RM_SPELL, 48, TargetCret.CurrX, TargetCret.CurrY, 48, "");
                                                SendRefMsg(Grobal2.RM_MAGICFIRE, 0, HUtil32.MakeWord(2, 48), HUtil32.MakeLong(TargetCret.CurrX, TargetCret.CurrY), TargetCret.ObjectId, "");
                                                SendDelayMsg(Grobal2.RM_STRUCK, Grobal2.RM_DELAYMAGIC, (short)nPower, HUtil32.MakeLong(TargetCret.CurrX, TargetCret.CurrY), 2, TargetCret.ObjectId, "", 600);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    BreakHolySeizeMode();
                }
                else
                {
                    TargetCret = null;
                }
                if ((HUtil32.GetTickCount() - SearchEnemyTick) > 8000 || (HUtil32.GetTickCount() - SearchEnemyTick) > 1000 && TargetCret == null)
                {
                    SearchEnemyTick = HUtil32.GetTickCount();
                    SearchTarget();
                }
            }
            base.Run();
        }
    }
}

