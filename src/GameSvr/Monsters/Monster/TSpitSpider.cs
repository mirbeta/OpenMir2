using SystemModule;

namespace GameSvr
{
    public class TSpitSpider : TATMonster
    {
        public bool m_boUsePoison;

        public TSpitSpider() : base()
        {
            this.m_dwSearchTime = M2Share.RandomNumber.Random(1500) + 1500;
            this.m_boAnimal = true;
            m_boUsePoison = true;
        }

        private void SpitAttack(byte btDir)
        {
            short nX = 0;
            short nY = 0;
            TBaseObject BaseObject;
            this.m_btDirection = btDir;
            var WAbil = this.m_WAbil;
            var nDamage = M2Share.RandomNumber.Random(HUtil32.HiWord(WAbil.DC) - HUtil32.LoWord(WAbil.DC) + 1) + HUtil32.LoWord(WAbil.DC);
            if (nDamage <= 0)
            {
                return;
            }
            this.SendRefMsg(Grobal2.RM_HIT, this.m_btDirection, this.m_nCurrX, this.m_nCurrY, 0, "");
            for (var i = 0; i <= 4; i++)
            {
                for (var k = 0; k <= 4; k++)
                {
                    if (M2Share.g_Config.SpitMap[btDir, i, k] == 1)
                    {
                        nX = (short)(this.m_nCurrX - 2 + k);
                        nY = (short)(this.m_nCurrY - 2 + i);
                        BaseObject = (TBaseObject)this.m_PEnvir.GetMovingObject(nX, nY, true);
                        if (BaseObject != null && BaseObject != this && this.IsProperTarget(BaseObject) && M2Share.RandomNumber.Random(BaseObject.m_btSpeedPoint) < this.m_btHitPoint)
                        {
                            nDamage = BaseObject.GetMagStruckDamage(this, nDamage);
                            if (nDamage > 0)
                            {
                                BaseObject.StruckDamage(nDamage);
                                BaseObject.SendDelayMsg(Grobal2.RM_STRUCK, Grobal2.RM_10101, (short)nDamage, this.m_WAbil.HP, this.m_WAbil.MaxHP, this.ObjectId, "", 300);
                                if (m_boUsePoison)
                                {
                                    if (M2Share.RandomNumber.Random(this.m_btAntiPoison + 20) == 0)
                                    {
                                        BaseObject.MakePosion(Grobal2.POISON_DECHEALTH, 30, 1);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        protected override bool AttackTarget()
        {
            byte btDir = 0;
            if (this.m_TargetCret == null)
            {
                return false;
            }
            if (this.TargetInSpitRange(this.m_TargetCret, ref btDir))
            {
                if ((HUtil32.GetTickCount() - this.m_dwHitTick) > this.m_nNextHitTime)
                {
                    this.m_dwHitTick = HUtil32.GetTickCount();
                    this.m_dwTargetFocusTick = HUtil32.GetTickCount();
                    SpitAttack(btDir);
                    this.BreakHolySeizeMode();
                }
                return true;
            }
            if (this.m_TargetCret.m_PEnvir == this.m_PEnvir)
            {
                this.SetTargetXY(this.m_TargetCret.m_nCurrX, this.m_TargetCret.m_nCurrY);
            }
            else
            {
                this.DelTargetCreat();
            }
            return false;
        }
    }
}

