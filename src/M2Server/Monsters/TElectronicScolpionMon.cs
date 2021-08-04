using System;
using SystemModule;

namespace M2Server
{
    public class TElectronicScolpionMon : TMonster
    {
        private bool m_boUseMagic = false;

        public TElectronicScolpionMon() : base()
        {
            m_dwSearchTime = M2Share.RandomNumber.Random(1500) + 1500;
            m_boUseMagic = false;
        }

        private void LightingAttack(byte nDir)
        {
            TAbility WAbil;
            int nPower;
            int nDamage;
            int btGetBackHP;
            m_btDirection = nDir;
            WAbil = m_WAbil;
            nPower = GetAttackPower(HUtil32.LoWord(WAbil.MC), HUtil32.HiWord(WAbil.MC) - HUtil32.LoWord(WAbil.MC));
            nDamage = m_TargetCret.GetMagStruckDamage(this, nPower);
            if (nDamage > 0)
            {
                btGetBackHP = HUtil32.LoByte(m_WAbil.MP);
                if (btGetBackHP != 0)
                {
                    m_WAbil.HP += (ushort)(nDamage / btGetBackHP);
                }
                m_TargetCret.StruckDamage(nDamage);
                m_TargetCret.SendDelayMsg(grobal2.RM_STRUCK, grobal2.RM_10101, (short)nDamage, m_TargetCret.m_WAbil.HP, m_TargetCret.m_WAbil.MaxHP, ObjectId, "", 200);
            }
            SendRefMsg(grobal2.RM_LIGHTING, 1, m_nCurrX, m_nCurrY, m_TargetCret.ObjectId, "");
        }

        public override void Run()
        {
            if (!m_boDeath && !bo554 && !m_boGhost && m_wStatusTimeArr[grobal2.POISON_STONE] == 0)
            {
                // 血量低于一半时开始用魔法攻击
                if (m_WAbil.HP < m_WAbil.MaxHP / 2)
                {
                    m_boUseMagic = true;
                }
                else
                {
                    m_boUseMagic = false;
                }
                if (HUtil32.GetTickCount() - m_dwSearchEnemyTick > 1000 && m_TargetCret == null)
                {
                    m_dwSearchEnemyTick = HUtil32.GetTickCount();
                    SearchTarget();
                }
                if (m_TargetCret == null)
                {
                    return;
                }
                var nX = Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX);
                var nY = Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY);
                if (nX <= 2 && nY <= 2)
                {
                    if (m_boUseMagic || nX == 2 || nY == 2)
                    {
                        if (HUtil32.GetTickCount() - m_dwHitTick > m_nNextHitTime)
                        {
                            m_dwHitTick = HUtil32.GetTickCount();
                            int nAttackDir = M2Share.GetNextDirection(m_nCurrX, m_nCurrY, m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY);
                            LightingAttack((byte)nAttackDir);
                        }
                    }
                }
            }
            base.Run();
        }
    }
}

