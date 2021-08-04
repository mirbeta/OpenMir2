using System;
using SystemModule;

namespace M2Server
{
    public class TBigHeartMonster : TAnimalObject
    {
        public TBigHeartMonster() : base()
        {
            m_nViewRange = 16;
            m_boAnimal = false;
        }

        protected virtual bool AttackTarget()
        {
            var result = false;
            TBaseObject BaseObject;
            int nPower;
            TAbility WAbil;
            if (HUtil32.GetTickCount() - m_dwHitTick > m_nNextHitTime)
            {
                m_dwHitTick = HUtil32.GetTickCount();
                SendRefMsg(grobal2.RM_HIT, m_btDirection, m_nCurrX, m_nCurrY, 0, "");
                WAbil = m_WAbil;
                nPower = M2Share.RandomNumber.Random(HUtil32.HiWord(WAbil.DC) - HUtil32.LoWord(WAbil.DC) + 1) + HUtil32.LoWord(WAbil.DC);
                for (var i = 0; i < m_VisibleActors.Count; i++)
                {
                    BaseObject = m_VisibleActors[i].BaseObject;
                    if (BaseObject.m_boDeath)
                    {
                        continue;
                    }
                    if (IsProperTarget(BaseObject))
                    {
                        if (Math.Abs(m_nCurrX - BaseObject.m_nCurrX) <= m_nViewRange && Math.Abs(m_nCurrY - BaseObject.m_nCurrY) <= m_nViewRange)
                        {
                            SendDelayMsg(this, grobal2.RM_DELAYMAGIC, (short)nPower, HUtil32.MakeLong(BaseObject.m_nCurrX, BaseObject.m_nCurrY), 1, BaseObject.ObjectId, "", 200);
                            SendRefMsg(grobal2.RM_10205, 0, BaseObject.m_nCurrX, BaseObject.m_nCurrY, 1, "");
                        }
                    }
                }
                result = true;
            }
            return result;
        }

        public override void Run()
        {
            if (!m_boGhost && !m_boDeath && m_wStatusTimeArr[grobal2.POISON_STONE] == 0)
            {
                if (m_VisibleActors.Count > 0)
                {
                    AttackTarget();
                }
            }
            base.Run();
        }
    }
}

