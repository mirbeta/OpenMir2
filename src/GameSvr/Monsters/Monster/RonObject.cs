using System;
using System.Collections.Generic;
using SystemModule;

namespace GameSvr
{
    public class RonObject : Monster
    {
        public RonObject()
            : base()
        {
            m_dwSearchTime = M2Share.RandomNumber.Random(1500) + 1500;
        }

        private void AroundAttack()
        {
            TBaseObject BaseObject;
            short wHitMode = 0;
            GetAttackDir(m_TargetCret, ref m_btDirection);
            IList<TBaseObject> xTargetList = new List<TBaseObject>();
            GetMapBaseObjects(m_PEnvir, m_nCurrX, m_nCurrY, 1, xTargetList);
            if (xTargetList.Count > 0)
            {
                for (var i = xTargetList.Count - 1; i >= 0; i--)
                {
                    BaseObject = xTargetList[i];
                    if (BaseObject != null)
                    {
                        _Attack(ref wHitMode, BaseObject);
                        xTargetList.RemoveAt(i);
                    }
                }
            }
            SendRefMsg(Grobal2.RM_HIT, m_btDirection, m_nCurrX, m_nCurrY, 0, "");
        }

        public override void Run()
        {
            if (!m_boDeath && !m_boGhost && m_wStatusTimeArr[Grobal2.POISON_STONE] == 0)
            {
                if ((HUtil32.GetTickCount() - m_dwSearchEnemyTick) > 8000 || (HUtil32.GetTickCount() - m_dwSearchEnemyTick) > 1000 && m_TargetCret == null)
                {
                    m_dwSearchEnemyTick = HUtil32.GetTickCount();
                    SearchTarget();
                }
                if (m_TargetCret != null && Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) < 6 && Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) < 6 && (HUtil32.GetTickCount() - m_dwHitTick) > m_nNextHitTime)
                {
                    m_dwHitTick = HUtil32.GetTickCount();
                    AroundAttack();
                }
            }
            base.Run();
        }
    }
}