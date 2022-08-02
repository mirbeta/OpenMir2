using SystemModule;

public class TFrostTiger: TSkeletonOma
    {
        public bool boActive = false;
        public bool boCasted = false;
        //Constructor  Create()
        public TFrostTiger() : base()
        {
            boActive = false;
            boCasted = false;
        }
        public override void CalcActorFrame()
        {
            TMonsterAction pm;
            this.m_nCurrentFrame =  -1;
            this.m_nBodyOffset = Actor.Units.Actor.GetOffset(this.m_wAppearance);
            pm = Actor.Units.Actor.GetRaceByPM(this.m_btRace, this.m_wAppearance);
            if (pm == null)
            {
                return;
            }
            switch(this.m_nCurrentAction)
            {
                case Grobal2.SM_LIGHTING:
                    this.m_nStartFrame = pm.ActCritical.start + this.m_btDir * (pm.ActCritical.frame + pm.ActCritical.skip);
                    this.m_nEndFrame = this.m_nStartFrame + pm.ActCritical.frame - 1;
                    this.m_dwFrameTime = pm.ActCritical.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    this.m_dwWarModeTime = MShare.GetTickCount();
                    boCasted = true;
                    this.Shift(this.m_btDir, 0, 0, 1);
                    break;
                case Grobal2.SM_DIGDOWN:
                    this.m_nStartFrame = pm.ActDeath.start + this.m_btDir * (pm.ActDeath.frame + pm.ActDeath.skip);
                    this.m_nEndFrame = this.m_nStartFrame + pm.ActDeath.frame - 1;
                    this.m_dwFrameTime = pm.ActDeath.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    this.m_dwWarModeTime = MShare.GetTickCount();
                    boActive = false;
                    break;
                case Grobal2.SM_DIGUP:
                    boActive = true;
                    break;
                case Grobal2.SM_WALK:
                    boActive = true;
                    base.CalcActorFrame();
                    break;
                default:
                    base.CalcActorFrame();
                    break;
            }
        }

        public override void Run()
        {
            bool bofly;
            if ((this.m_nCurrentAction == Grobal2.SM_LIGHTING) && (boCasted == true))
            {
                boCasted = false;
               ClMain.g_PlayScene.NewMagic(this, 1, 39, this.m_nCurrX, this.m_nCurrY, this.m_nTargetX, this.m_nTargetY, this.m_nTargetRecog, magiceff.TMagicType.mtFly, false, 30, ref bofly);
                            }
            base.Run();
        }

        public override int GetDefaultFrame(bool wmode)
        {
            int result;
            int cf;
            TMonsterAction pm;
            result = 0;
            if (boActive == false)
            {
                pm = Actor.Units.Actor.GetRaceByPM(this.m_btRace, this.m_wAppearance);
                if (pm == null)
                {
                    return result;
                }
                if (this.m_boDeath)
                {
                    base.GetDefaultFrame(wmode);
                    return result;
                }
                this.m_nDefFrameCount = pm.ActDeath.frame;
                if (this.m_nCurrentDefFrame < 0)
                {
                    cf = 0;
                }
                else if (this.m_nCurrentDefFrame >= pm.ActDeath.frame)
                {
                    cf = 0;
                }
                else
                {
                    cf = this.m_nCurrentDefFrame;
                }
                result = pm.ActDeath.start + this.m_btDir * (pm.ActDeath.frame + pm.ActDeath.skip) + cf;
            }
            else
            {
                result = base.GetDefaultFrame(wmode);
            }
            return result;
        }

    } // end TFrostTiger

