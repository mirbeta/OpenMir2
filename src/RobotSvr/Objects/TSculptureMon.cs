using SystemModule;

namespace RobotSvr
{
    public class TSculptureMon: TSkeletonOma
    {
        private TDirectDrawSurface AttackEffectSurface = null;
        private int ax = 0;
        private int ay = 0;
        private int firedir = 0;
        // -----------------------------------------------------------
        // procedure TZombiLighting.Run;
        // -----------------------------------------------------------
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
            this.m_boUseEffect = false;
            switch(this.m_nCurrentAction)
            {
                case Grobal2.SM_TURN:
                    if ((this.m_nState & Grobal2.STATE_STONE_MODE) != 0)
                    {
                        if ((this.m_btRace == 48) || (this.m_btRace == 49))
                        {
                            // + Dir * (pm.ActDeath.frame + pm.ActDeath.skip)
                            this.m_nStartFrame = pm.ActDeath.start;
                        }
                        else
                        {
                            this.m_nStartFrame = pm.ActDeath.start + this.m_btDir * (pm.ActDeath.frame + pm.ActDeath.skip);
                        }
                        this.m_nEndFrame = this.m_nStartFrame;
                        this.m_dwFrameTime = pm.ActDeath.ftime;
                        this.m_dwStartTime = MShare.GetTickCount();
                        this.m_nDefFrameCount = pm.ActDeath.frame;
                    }
                    else
                    {
                        this.m_nStartFrame = pm.ActStand.start + this.m_btDir * (pm.ActStand.frame + pm.ActStand.skip);
                        this.m_nEndFrame = this.m_nStartFrame + pm.ActStand.frame - 1;
                        this.m_dwFrameTime = pm.ActStand.ftime;
                        this.m_dwStartTime = MShare.GetTickCount();
                        this.m_nDefFrameCount = pm.ActStand.frame;
                    }
                    this.Shift(this.m_btDir, 0, 0, 1);
                    break;
                case Grobal2.SM_WALK:
                case Grobal2.SM_BACKSTEP:
                    this.m_nStartFrame = pm.ActWalk.start + this.m_btDir * (pm.ActWalk.frame + pm.ActWalk.skip);
                    this.m_nEndFrame = this.m_nStartFrame + pm.ActWalk.frame - 1;
                    this.m_dwFrameTime = pm.ActWalk.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    this.m_nMaxTick = pm.ActWalk.usetick;
                    this.m_nCurTick = 0;
                    // WarMode := FALSE;
                    this.m_nMoveStep = 1;
                    if (this.m_nCurrentAction == Grobal2.SM_WALK)
                    {
                        this.Shift(this.m_btDir, this.m_nMoveStep, 0, this.m_nEndFrame - this.m_nStartFrame + 1);
                    }
                    else
                    {
                        // sm_backstep
                        this.Shift(ClFunc.GetBack(this.m_btDir), this.m_nMoveStep, 0, this.m_nEndFrame - this.m_nStartFrame + 1);
                    }
                    break;
                case Grobal2.SM_DIGUP:
                    // //叭扁 绝澜, SM_DIGUP, 规氢 绝澜.
                    if ((this.m_btRace == 48) || (this.m_btRace == 49))
                    {
                        this.m_nStartFrame = pm.ActDeath.start;
                    }
                    else
                    {
                        this.m_nStartFrame = pm.ActDeath.start + this.m_btDir * (pm.ActDeath.frame + pm.ActDeath.skip);
                    }
                    this.m_nEndFrame = this.m_nStartFrame + pm.ActDeath.frame - 1;
                    this.m_dwFrameTime = pm.ActDeath.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    // WarMode := FALSE;
                    this.Shift(this.m_btDir, 0, 0, 1);
                    break;
                case Grobal2.SM_HIT:
                    this.m_nStartFrame = pm.ActAttack.start + this.m_btDir * (pm.ActAttack.frame + pm.ActAttack.skip);
                    this.m_nEndFrame = this.m_nStartFrame + pm.ActAttack.frame - 1;
                    this.m_dwFrameTime = pm.ActAttack.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    if (this.m_btRace == 49)
                    {
                        this.m_boUseEffect = true;
                        firedir = this.m_btDir;
                        this.m_nEffectFrame = 0;
                        // startframe;
                        this.m_nEffectStart = 0;
                        // startframe;
                        this.m_nEffectEnd = this.m_nEffectStart + 8;
                        this.m_dwEffectStartTime = MShare.GetTickCount();
                        this.m_dwEffectFrameTime = this.m_dwFrameTime;
                    }
                    this.Shift(this.m_btDir, 0, 0, 1);
                    break;
                case Grobal2.SM_STRUCK:
                    this.m_nStartFrame = pm.ActStruck.start + this.m_btDir * (pm.ActStruck.frame + pm.ActStruck.skip);
                    this.m_nEndFrame = this.m_nStartFrame + pm.ActStruck.frame - 1;
                    this.m_dwFrameTime = this.m_dwStruckFrameTime;
                    // pm.ActStruck.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    break;
                case Grobal2.SM_DEATH:
                    this.m_nStartFrame = pm.ActDie.start + this.m_btDir * (pm.ActDie.frame + pm.ActDie.skip);
                    this.m_nEndFrame = this.m_nStartFrame + pm.ActDie.frame - 1;
                    this.m_nStartFrame = this.m_nEndFrame;
                    this.m_dwFrameTime = pm.ActDie.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    break;
                case Grobal2.SM_NOWDEATH:
                    this.m_nStartFrame = pm.ActDie.start + this.m_btDir * (pm.ActDie.frame + pm.ActDie.skip);
                    this.m_nEndFrame = this.m_nStartFrame + pm.ActDie.frame - 1;
                    this.m_dwFrameTime = pm.ActDie.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    break;
            }
        }

        public override void LoadSurface()
        {
            base.LoadSurface();
            switch(this.m_btRace)
            {
                case 48:
                case 49:
                    if (this.m_boUseEffect)
                    {
                        AttackEffectSurface = WMFile.Units.WMFile.g_WMons[7].GetCachedImage(Units.AxeMon.SCULPTUREFIREBASE + (firedir * 10) + this.m_nEffectFrame - this.m_nEffectStart, ref ax, ref ay);
                    }
                    break;
            }
        }

        public override int GetDefaultFrame(bool wmode)
        {
            int result;
            int cf;
            TMonsterAction pm;
            result = 0;
            pm = Actor.Units.Actor.GetRaceByPM(this.m_btRace, this.m_wAppearance);
            if (pm == null)
            {
                return result;
            }
            if (this.m_boDeath)
            {
                result = pm.ActDie.start + this.m_btDir * (pm.ActDie.frame + pm.ActDie.skip) + (pm.ActDie.frame - 1);
            }
            else
            {
                if ((this.m_nState & Grobal2.STATE_STONE_MODE) != 0)
                {
                    switch(this.m_btRace)
                    {
                        case 47:
                            result = pm.ActDeath.start + this.m_btDir * (pm.ActDeath.frame + pm.ActDeath.skip);
                            break;
                        case 48:
                        case 49:
                            result = pm.ActDeath.start;
                            break;
                    }
                }
                else
                {
                    this.m_nDefFrameCount = pm.ActStand.frame;
                    if (this.m_nCurrentDefFrame < 0)
                    {
                        cf = 0;
                    }
                    else if (this.m_nCurrentDefFrame >= pm.ActStand.frame)
                    {
                        cf = 0;
                    }
                    else
                    {
                        cf = this.m_nCurrentDefFrame;
                    }
                    result = pm.ActStand.start + this.m_btDir * (pm.ActStand.frame + pm.ActStand.skip) + cf;
                }
            }
            return result;
        }

        public override void DrawEff(TDirectDrawSurface dsurface, int dx, int dy)
        {
            if (this.m_boUseEffect)
            {
                if (AttackEffectSurface != null)
                {
                    cliUtil.Units.cliUtil.DrawBlend(dsurface, dx + ax + this.m_nShiftX, dy + ay + this.m_nShiftY, AttackEffectSurface, 1);
                }
            }
        }

        public override void Run()
        {
            long m_dwEffectFrameTimetime;
            if ((this.m_nCurrentAction == Grobal2.SM_WALK) || (this.m_nCurrentAction == Grobal2.SM_BACKSTEP) || (this.m_nCurrentAction == Grobal2.SM_RUN) || (this.m_nCurrentAction == Grobal2.SM_HORSERUN))
            {
                return;
            }
            if (this.m_boUseEffect)
            {
                m_dwEffectFrameTimetime = this.m_dwEffectFrameTime;
                if (MShare.GetTickCount() - this.m_dwEffectStartTime > m_dwEffectFrameTimetime)
                {
                    this.m_dwEffectStartTime = MShare.GetTickCount();
                    if (this.m_nEffectFrame < this.m_nEffectEnd)
                    {
                        this.m_nEffectFrame ++;
                    }
                    else
                    {
                        this.m_boUseEffect = false;
                    }
                }
            }
            base.Run();
        }

    } // end TSculptureMon

    } // end TBanyaGuardMon

