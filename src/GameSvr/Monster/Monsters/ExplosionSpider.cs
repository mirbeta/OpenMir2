using GameSvr.Actor;
using SystemModule;

namespace GameSvr.Monster.Monsters
{
    public class ExplosionSpider : MonsterObject
    {
        public int dw558 = 0;

        public ExplosionSpider() : base()
        {
            m_nViewRange = 5;
            m_nRunTime = 250;
            m_dwSearchTime = M2Share.RandomNumber.Random(1500) + 2500;
            m_dwSearchTick = 0;
            dw558 = HUtil32.GetTickCount();
        }

        private void sub_4A65C4()
        {
            m_WAbil.HP = 0;
            var nPower = M2Share.RandomNumber.Random(HUtil32.HiWord(m_WAbil.DC) - HUtil32.LoWord(m_WAbil.DC) + 1) + HUtil32.LoWord(m_WAbil.DC);
            for (var i = 0; i < m_VisibleActors.Count; i++)
            {
                var BaseObject = m_VisibleActors[i].BaseObject;
                if (BaseObject.m_boDeath)
                {
                    continue;
                }
                if (IsProperTarget(BaseObject))
                {
                    if (Math.Abs(m_nCurrX - BaseObject.m_nCurrX) <= 1 && Math.Abs(m_nCurrY - BaseObject.m_nCurrY) <= 1)
                    {
                        var damage = 0;
                        damage += BaseObject.GetHitStruckDamage(this, nPower / 2);
                        damage += BaseObject.GetMagStruckDamage(this, nPower / 2);
                        if (damage > 0)
                        {
                            BaseObject.StruckDamage(damage);
                            BaseObject.SendDelayMsg(Grobal2.RM_STRUCK, Grobal2.RM_10101, (short)damage, BaseObject.m_WAbil.HP, BaseObject.m_WAbil.MaxHP, ObjectId, "", 700);
                        }
                    }
                }
            }
        }

        protected override bool AttackTarget()
        {
            var result = false;
            byte btDir = 0;
            if (m_TargetCret == null)
            {
                return result;
            }
            if (GetAttackDir(m_TargetCret, ref btDir))
            {
                if ((HUtil32.GetTickCount() - m_dwHitTick) > m_nNextHitTime)
                {
                    m_dwHitTick = HUtil32.GetTickCount();
                    m_dwTargetFocusTick = HUtil32.GetTickCount();
                    sub_4A65C4();
                }
                result = true;
            }
            else
            {
                if (m_TargetCret.m_PEnvir == m_PEnvir)
                {
                    SetTargetXY(m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY);
                }
                else
                {
                    DelTargetCreat();
                }
            }
            return result;
        }

        public override void Run()
        {
            if (!m_boDeath && !m_boGhost)
            {
                if ((HUtil32.GetTickCount() - dw558) > (60 * 1000))
                {
                    dw558 = HUtil32.GetTickCount();
                    sub_4A65C4();
                }
            }
            base.Run();
        }
    }
}

