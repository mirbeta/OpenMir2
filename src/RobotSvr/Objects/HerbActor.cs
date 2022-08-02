namespace RobotSvr
{
    public class TKillingHerb: TActor
    {
        public TKillingHerb() : base()
        {

        }

        public override void CalcActorFrame()
        {
            TMonsterAction pm;
            this.m_boUseMagic = false;
            this.m_nCurrentFrame =  -1;
            this.m_nBodyOffset = Actor.Units.Actor.GetOffset(this.m_wAppearance);
            pm = Actor.Units.Actor.GetRaceByPM(this.m_btRace, this.m_wAppearance);
            if (pm == null)
            {
                return;
            }
            switch(this.m_nCurrentAction)
            {
                case Grobal2.SM_TURN:
                    // //
                    this.m_nStartFrame = pm.ActStand.start;
                    // + Dir * (pm.ActStand.frame + pm.ActStand.skip);
                    this.m_nEndFrame = this.m_nStartFrame + pm.ActStand.frame - 1;
                    this.m_dwFrameTime = pm.ActStand.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    this.m_nDefFrameCount = pm.ActStand.frame;
                    this.Shift(this.m_btDir, 0, 0, 1);
                    break;
                case Grobal2.SM_DIGUP:
                    // //, SM_DIGUP, .
                    this.m_nStartFrame = pm.ActWalk.start;
                    // + Dir * (pm.ActWalk.frame + pm.ActWalk.skip);
                    this.m_nEndFrame = this.m_nStartFrame + pm.ActWalk.frame - 1;
                    this.m_dwFrameTime = pm.ActWalk.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    this.m_nMaxTick = pm.ActWalk.usetick;
                    this.m_nCurTick = 0;
                    this.m_nMoveStep = 1;
                    this.Shift(this.m_btDir, 0, 0, 1);
                    break;
                case Grobal2.SM_HIT:
                    // m_nMoveStep, 0, m_nEndFrame-startframe+1);
                    this.m_nStartFrame = pm.ActAttack.start + this.m_btDir * (pm.ActAttack.frame + pm.ActAttack.skip);
                    this.m_nEndFrame = this.m_nStartFrame + pm.ActAttack.frame - 1;
                    this.m_dwFrameTime = pm.ActAttack.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    // WarMode := TRUE;
                    this.m_dwWarModeTime = MShare.GetTickCount();
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
                case Grobal2.SM_DIGDOWN:
                    this.m_nStartFrame = pm.ActDeath.start;
                    this.m_nEndFrame = this.m_nStartFrame + pm.ActDeath.frame - 1;
                    this.m_dwFrameTime = pm.ActDeath.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    this.m_boDelActionAfterFinished = true;
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
                if (this.m_boSkeleton)
                {
                    result = pm.ActDeath.start;
                }
                else
                {
                    result = pm.ActDie.start + this.m_btDir * (pm.ActDie.frame + pm.ActDie.skip) + (pm.ActDie.frame - 1);
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
                result = pm.ActStand.start + cf;
            }
            return result;
        }

    } // end TKillingHerb

    public class TMineMon: TKillingHerb
    {
        // TMineMon
        public override void CalcActorFrame()
        {
            base.CalcActorFrame();
        }

        //Constructor  Create()
        public TMineMon() : base()
        {
        }
        public override int GetDefaultFrame(bool wmode)
        {
            int result;
            result = 0;
            return result;
        }

    } // end TMineMon

    public class TBeeQueen: TActor
    {
        // ----------------------------------------------------------------------
        public override void CalcActorFrame()
        {
            TMonsterAction pm;
            this.m_boUseMagic = false;
            this.m_nCurrentFrame =  -1;
            this.m_nBodyOffset = Actor.Units.Actor.GetOffset(this.m_wAppearance);
            pm = Actor.Units.Actor.GetRaceByPM(this.m_btRace, this.m_wAppearance);
            if (pm == null)
            {
                return;
            }
            switch(this.m_nCurrentAction)
            {
                case Grobal2.SM_TURN:
                    // //방향이 없음...
                    this.m_nStartFrame = pm.ActStand.start;
                    // + Dir * (pm.ActStand.frame + pm.ActStand.skip);
                    this.m_nEndFrame = this.m_nStartFrame + pm.ActStand.frame - 1;
                    this.m_dwFrameTime = pm.ActStand.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    this.m_nDefFrameCount = pm.ActStand.frame;
                    this.Shift(this.m_btDir, 0, 0, 1);
                    break;
                case Grobal2.SM_HIT:
                    this.m_nStartFrame = pm.ActAttack.start;
                    // + Dir * (pm.ActAttack.frame + pm.ActAttack.skip);
                    this.m_nEndFrame = this.m_nStartFrame + pm.ActAttack.frame - 1;
                    this.m_dwFrameTime = pm.ActAttack.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    // WarMode := TRUE;
                    this.m_dwWarModeTime = MShare.GetTickCount();
                    this.Shift(this.m_btDir, 0, 0, 1);
                    break;
                case Grobal2.SM_STRUCK:
                    this.m_nStartFrame = pm.ActStruck.start;
                    // + Dir * (pm.ActStruck.frame + pm.ActStruck.skip);
                    this.m_nEndFrame = this.m_nStartFrame + pm.ActStruck.frame - 1;
                    this.m_dwFrameTime = this.m_dwStruckFrameTime;
                    // pm.ActStruck.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    break;
                case Grobal2.SM_DEATH:
                    this.m_nStartFrame = pm.ActDie.start;
                    // + Dir * (pm.ActDie.frame + pm.ActDie.skip);
                    this.m_nEndFrame = this.m_nStartFrame + pm.ActDie.frame - 1;
                    this.m_nStartFrame = this.m_nEndFrame;
                    this.m_dwFrameTime = pm.ActDie.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    break;
                case Grobal2.SM_NOWDEATH:
                    this.m_nStartFrame = pm.ActDie.start;
                    // + Dir * (pm.ActDie.frame + pm.ActDie.skip);
                    this.m_nEndFrame = this.m_nStartFrame + pm.ActDie.frame - 1;
                    this.m_dwFrameTime = pm.ActDie.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    break;
            }
        }

        public override int GetDefaultFrame(bool wmode)
        {
            int result;
            TMonsterAction pm;
            int cf;
            result = 0;
            // jacky
            pm = Actor.Units.Actor.GetRaceByPM(this.m_btRace, this.m_wAppearance);
            if (pm == null)
            {
                return result;
            }
            if (this.m_boDeath)
            {
                result = pm.ActDie.start + (pm.ActDie.frame - 1);
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
                result = pm.ActStand.start + cf;
            // 방향이 없음..
            }
            return result;
        }

    } // end TBeeQueen

    public class TCentipedeKingMon: TKillingHerb
    {
        private TDirectDrawSurface AttackEffectSurface = null;
        // 0x250
        private bool BoUseDieEffect = false;
        // 0x254
        private int ax = 0;
        // 0x258
        private int ay = 0;
        public override void CalcActorFrame()
        {
            TMonsterAction pm;
            this.m_boUseMagic = false;
            this.m_nCurrentFrame =  -1;
            this.m_nBodyOffset = Actor.Units.Actor.GetOffset(this.m_wAppearance);
            pm = Actor.Units.Actor.GetRaceByPM(this.m_btRace, this.m_wAppearance);
            if (pm == null)
            {
                return;
            }
            switch(this.m_nCurrentAction)
            {
                case Grobal2.SM_TURN:
                    this.m_btDir = 0;
                    base.CalcActorFrame();
                    break;
                case Grobal2.SM_HIT:
                    this.m_btDir = 0;
                    this.m_nStartFrame = pm.ActCritical.start + this.m_btDir * (pm.ActCritical.frame + pm.ActCritical.skip);
                    this.m_nEndFrame = this.m_nStartFrame + pm.ActCritical.frame - 1;
                    this.m_dwFrameTime = pm.ActCritical.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    BoUseDieEffect = true;
                    this.m_nEffectFrame = 0;
                    this.m_nEffectStart = 0;
                    this.m_nEffectEnd = this.m_nEffectStart + 9;
                    this.m_dwEffectFrameTime = 62;
                    this.Shift(this.m_btDir, 0, 0, 1);
                    break;
                case Grobal2.SM_DIGDOWN:
                    base.CalcActorFrame();
                    break;
                default:
                    this.m_btDir = 0;
                    base.CalcActorFrame();
                    break;
            }
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

        // 0x25C
        private void LoadEffect()
        {
            if (this.m_boUseEffect)
            {
                AttackEffectSurface = WMFile.Units.WMFile.g_WMons[15].GetCachedImage(100 + this.m_nEffectFrame - this.m_nEffectStart, ref ax, ref ay);
            }
        }

        public override void LoadSurface()
        {
            base.LoadSurface();
            LoadEffect();
        }

        public override void Run()
        {
            if ((this.m_nCurrentAction == Grobal2.SM_WALK) || (this.m_nCurrentAction == Grobal2.SM_BACKSTEP) || (this.m_nCurrentAction == Grobal2.SM_HORSERUN) || (this.m_nCurrentAction == Grobal2.SM_RUN))
            {
                return;
            }
            if (BoUseDieEffect)
            {
                if ((this.m_nCurrentFrame - this.m_nStartFrame) >= 5)
                {
                    BoUseDieEffect = false;
                    this.m_boUseEffect = true;
                    this.m_dwEffectStartTime = MShare.GetTickCount();
                    this.m_nEffectFrame = 0;
                    LoadEffect();
                }
            }
            if (this.m_boUseEffect)
            {
                if ((MShare.GetTickCount() - this.m_dwEffectStartTime) > this.m_dwEffectFrameTime)
                {
                    this.m_dwEffectStartTime = MShare.GetTickCount();
                    if (this.m_nEffectFrame < this.m_nEffectEnd)
                    {
                        this.m_nEffectFrame ++;
                        LoadEffect();
                    }
                    else
                    {
                        this.m_boUseEffect = false;
                    }
                }
            }
            base.Run();
        }

    } // end TCentipedeKingMon

    public class TBigHeartMon: TKillingHerb
    {
        // TBigHeartMon
        public override void CalcActorFrame()
        {
            this.m_btDir = 0;
            base.CalcActorFrame();
        }

    } // end TBigHeartMon

    public class TSpiderHouseMon: TKillingHerb
    {
        // TSpiderHouseMon
        public override void CalcActorFrame()
        {
            this.m_btDir = 0;
            base.CalcActorFrame();
        }

    } // end TSpiderHouseMon

    public class TCastleDoor: TActor
    {
        private TDirectDrawSurface EffectSurface = null;
        private int ax = 0;
        private int ay = 0;
        private int oldunitx = 0;
        private int oldunity = 0;
        public bool BoDoorOpen = false;
        // ----------------------------------------------------------------------
        //Constructor  Create()
        public TCastleDoor() : base()
        {
            this.m_btDir = 0;
            EffectSurface = null;
            this.m_nDownDrawLevel = 1;
        }
        private void ApplyDoorState(TDoorState dstate)
        {
            bool bowalk;
           ClMain.Map.MarkCanWalk(this.m_nCurrX, this.m_nCurrY - 2, true);
           ClMain.Map.MarkCanWalk(this.m_nCurrX + 1, this.m_nCurrY - 1, true);
           ClMain.Map.MarkCanWalk(this.m_nCurrX + 1, this.m_nCurrY - 2, true);
            if (dstate == TDoorState.dsClose)
            {
                bowalk = false;
            }
            else
            {
                bowalk = true;
            }
           ClMain.Map.MarkCanWalk(this.m_nCurrX, this.m_nCurrY, bowalk);
           ClMain.Map.MarkCanWalk(this.m_nCurrX, this.m_nCurrY - 1, bowalk);
           ClMain.Map.MarkCanWalk(this.m_nCurrX, this.m_nCurrY - 2, bowalk);
           ClMain.Map.MarkCanWalk(this.m_nCurrX + 1, this.m_nCurrY - 1, bowalk);
           ClMain.Map.MarkCanWalk(this.m_nCurrX + 1, this.m_nCurrY - 2, bowalk);
           ClMain.Map.MarkCanWalk(this.m_nCurrX - 1, this.m_nCurrY - 1, bowalk);
           ClMain.Map.MarkCanWalk(this.m_nCurrX - 1, this.m_nCurrY, bowalk);
           ClMain.Map.MarkCanWalk(this.m_nCurrX - 1, this.m_nCurrY + 1, bowalk);
           ClMain.Map.MarkCanWalk(this.m_nCurrX - 2, this.m_nCurrY, bowalk);
            if (dstate == TDoorState.dsOpen)
            {
               ClMain.Map.MarkCanWalk(this.m_nCurrX, this.m_nCurrY - 2, false);
               ClMain.Map.MarkCanWalk(this.m_nCurrX + 1, this.m_nCurrY - 1, false);
               ClMain.Map.MarkCanWalk(this.m_nCurrX + 1, this.m_nCurrY - 2, false);
            }
        }

        public override void LoadSurface()
        {
            TWMBaseImages mimg;
            base.LoadSurface();
            mimg = MShare.GetMonImg(this.m_wAppearance);
            if (this.m_boUseEffect)
            {
                EffectSurface = mimg.GetCachedImage(Units.HerbActor.DOORDEATHEFFECTBASE + (this.m_nCurrentFrame - this.m_nStartFrame), ref ax, ref ay);
            }
        }

        public override void CalcActorFrame()
        {
            TMonsterAction pm;
            this.m_boUseEffect = false;
            this.m_nCurrentFrame =  -1;
            this.m_nBodyOffset = Actor.Units.Actor.GetOffset(this.m_wAppearance);
            pm = Actor.Units.Actor.GetRaceByPM(this.m_btRace, this.m_wAppearance);
            if (pm == null)
            {
                return;
            }
            this.m_sUserName = " ";
            switch(this.m_nCurrentAction)
            {
                case Grobal2.SM_NOWDEATH:
                    this.m_nStartFrame = pm.ActDie.start;
                    this.m_nEndFrame = this.m_nStartFrame + pm.ActDie.frame - 1;
                    this.m_dwFrameTime = pm.ActDie.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    this.Shift(this.m_btDir, 0, 0, 1);
                    this.m_boUseEffect = true;
                    ApplyDoorState(TDoorState.dsBroken);
                    break;
                case Grobal2.SM_STRUCK:
                    this.m_nStartFrame = pm.ActStruck.start + this.m_btDir * (pm.ActStruck.frame + pm.ActStruck.skip);
                    this.m_nEndFrame = this.m_nStartFrame + pm.ActStruck.frame - 1;
                    this.m_dwFrameTime = pm.ActStand.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    this.Shift(this.m_btDir, 0, 0, 1);
                    break;
                case Grobal2.SM_DIGUP:
                    this.m_nStartFrame = pm.ActAttack.start;
                    this.m_nEndFrame = this.m_nStartFrame + pm.ActAttack.frame - 1;
                    this.m_dwFrameTime = pm.ActAttack.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    ApplyDoorState(TDoorState.dsOpen);
                    break;
                case Grobal2.SM_DIGDOWN:
                    this.m_nStartFrame = pm.ActCritical.start;
                    this.m_nEndFrame = this.m_nStartFrame + pm.ActCritical.frame - 1;
                    this.m_dwFrameTime = pm.ActCritical.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    BoDoorOpen = false;
                    this.m_boHoldPlace = true;
                    ApplyDoorState(TDoorState.dsClose);
                    break;
                case Grobal2.SM_DEATH:
                    this.m_nStartFrame = pm.ActDie.start + pm.ActDie.frame - 1;
                    this.m_nEndFrame = this.m_nStartFrame;
                    this.m_nDefFrameCount = 0;
                    ApplyDoorState(TDoorState.dsBroken);
                    break;
                default:
                    if (this.m_btDir < 3)
                    {
                        this.m_nStartFrame = pm.ActStand.start + this.m_btDir * (pm.ActStand.frame + pm.ActStand.skip);
                        this.m_nEndFrame = this.m_nStartFrame;
                        // + pm.ActStand.frame - 1;
                        this.m_dwFrameTime = pm.ActStand.ftime;
                        this.m_dwStartTime = MShare.GetTickCount();
                        this.m_nDefFrameCount = 0;
                        // pm.ActStand.frame;
                        this.Shift(this.m_btDir, 0, 0, 1);
                        BoDoorOpen = false;
                        this.m_boHoldPlace = true;
                        ApplyDoorState(TDoorState.dsClose);
                    }
                    else
                    {
                        this.m_nStartFrame = pm.ActCritical.start;
                        this.m_nEndFrame = this.m_nStartFrame;
                        this.m_nDefFrameCount = 0;
                        BoDoorOpen = true;
                        this.m_boHoldPlace = false;
                        ApplyDoorState(TDoorState.dsOpen);
                    }
                    break;
            }
        }

        public override int GetDefaultFrame(bool wmode)
        {
            int result;
            TMonsterAction pm;
            result = 0;
            // jacky
            this.m_nBodyOffset = Actor.Units.Actor.GetOffset(this.m_wAppearance);
            pm = Actor.Units.Actor.GetRaceByPM(this.m_btRace, this.m_wAppearance);
            if (pm == null)
            {
                return result;
            }
            if (this.m_boDeath)
            {
                result = pm.ActDie.start + pm.ActDie.frame - 1;
                this.m_nDownDrawLevel = 2;
            }
            else
            {
                if (BoDoorOpen)
                {
                    this.m_nDownDrawLevel = 2;
                    result = pm.ActCritical.start;
                // + Dir * (pm.ActStand.frame + pm.ActStand.skip);
                }
                else
                {
                    this.m_nDownDrawLevel = 1;
                    result = pm.ActStand.start + this.m_btDir * (pm.ActStand.frame + pm.ActStand.skip);
                }
            }
            return result;
        }

        public override void ActionEnded()
        {
            if (this.m_nCurrentAction == Grobal2.SM_DIGUP)
            {
                BoDoorOpen = true;
                this.m_boHoldPlace = false;
            }
        }

        public override void Run()
        {
            if ((ClMain.Map.m_nCurUnitX != oldunitx) || (ClMain.Map.m_nCurUnitY != oldunity))
            {
                if (this.m_boDeath)
                {
                    ApplyDoorState(TDoorState.dsBroken);
                }
                else if (BoDoorOpen)
                {
                    ApplyDoorState(TDoorState.dsOpen);
                }
                else
                {
                    ApplyDoorState(TDoorState.dsClose);
                }
            }
            oldunitx =ClMain.Map.m_nCurUnitX;
            oldunity =ClMain.Map.m_nCurUnitY;
            base.Run();
        }

        public override void DrawChr(TDirectDrawSurface dsurface, int dx, int dy, bool blend, bool boFlag, bool DrawOnSale)
        {
            base.DrawChr(dsurface, dx, dy, blend, false);
            if (this.m_boUseEffect && !blend)
            {
                if (EffectSurface != null)
                {
                    cliUtil.Units.cliUtil.DrawBlend(dsurface, dx + ax + this.m_nShiftX, dy + ay + this.m_nShiftY, EffectSurface, 1);
                }
            }
        }

    } // end TCastleDoor

    public class TWallStructure: TActor
    {
        private TDirectDrawSurface EffectSurface = null;
        private TDirectDrawSurface BrokenSurface = null;
        private int ax = 0;
        private int ay = 0;
        private int bx = 0;
        private int by = 0;
        private int deathframe = 0;
        private bool bomarkpos = false;
        // ----------------------------------------------------------------------
        //Constructor  Create()
        public TWallStructure() : base()
        {
            this.m_btDir = 0;
            EffectSurface = null;
            BrokenSurface = null;
            bomarkpos = false;
            // DownDrawLevel := 1;

        }
        public override void CalcActorFrame()
        {
            TMonsterAction pm;
            this.m_boUseEffect = false;
            this.m_nCurrentFrame =  -1;
            this.m_nBodyOffset = Actor.Units.Actor.GetOffset(this.m_wAppearance);
            pm = Actor.Units.Actor.GetRaceByPM(this.m_btRace, this.m_wAppearance);
            if (pm == null)
            {
                return;
            }
            this.m_sUserName = " ";
            deathframe = 0;
            this.m_boUseEffect = false;
            switch(this.m_nCurrentAction)
            {
                case Grobal2.SM_NOWDEATH:
                    this.m_nStartFrame = pm.ActDie.start;
                    this.m_nEndFrame = this.m_nStartFrame + pm.ActDie.frame - 1;
                    this.m_dwFrameTime = pm.ActDie.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    deathframe = pm.ActStand.start + this.m_btDir;
                    this.Shift(this.m_btDir, 0, 0, 1);
                    this.m_boUseEffect = true;
                    break;
                case Grobal2.SM_DEATH:
                    this.m_nStartFrame = pm.ActDie.start + pm.ActDie.frame - 1;
                    this.m_nEndFrame = this.m_nStartFrame;
                    this.m_nDefFrameCount = 0;
                    break;
                case Grobal2.SM_DIGUP:
                    // //모습이 변경될때 마다
                    this.m_nStartFrame = pm.ActDie.start;
                    this.m_nEndFrame = this.m_nStartFrame + pm.ActDie.frame - 1;
                    this.m_dwFrameTime = pm.ActDie.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    deathframe = pm.ActStand.start + this.m_btDir;
                    this.m_boUseEffect = true;
                    break;
                default:
                    // //방향이 없음...
                    this.m_nStartFrame = pm.ActStand.start + this.m_btDir;
                    // * (pm.ActStand.frame + pm.ActStand.skip);
                    this.m_nEndFrame = this.m_nStartFrame;
                    // + pm.ActStand.frame - 1;
                    this.m_dwFrameTime = pm.ActStand.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    this.m_nDefFrameCount = 0;
                    // pm.ActStand.frame;
                    this.Shift(this.m_btDir, 0, 0, 1);
                    this.m_boHoldPlace = true;
                    break;
            }
        }

        public override void LoadSurface()
        {
            TWMBaseImages mimg;
            mimg = MShare.GetMonImg(this.m_wAppearance);
            if (deathframe > 0)
            {
                // (CurrentAction = SM_NOWDEATH) or (CurrentAction = SM_DEATH) then begin
                this.m_BodySurface = mimg.GetCachedImage(Actor.Units.Actor.GetOffset(this.m_wAppearance) + deathframe, ref this.m_nPx, ref this.m_nPy);
            }
            else
            {
                base.LoadSurface();
            }
            if ((this.m_wAppearance >= 901) && (this.m_wAppearance <= 903))
            {
                BrokenSurface = WMFile.Units.WMFile.g_WEffectImg.GetCachedImage(Actor.Units.Actor.GetOffset(this.m_wAppearance) + 8 + this.m_btDir, ref bx, ref by);
                if (this.m_boUseEffect)
                {
                    if (this.m_wAppearance == 901)
                    {
                        EffectSurface = WMFile.Units.WMFile.g_WEffectImg.GetCachedImage(Units.HerbActor.WALLLEFTBROKENEFFECTBASE + (this.m_nCurrentFrame - this.m_nStartFrame), ref ax, ref ay);
                    }
                    else
                    {
                        EffectSurface = WMFile.Units.WMFile.g_WEffectImg.GetCachedImage(Units.HerbActor.WALLRIGHTBROKENEFFECTBASE + (this.m_nCurrentFrame - this.m_nStartFrame), ref ax, ref ay);
                    }
                }
            }
        }

        public override int GetDefaultFrame(bool wmode)
        {
            int result;
            TMonsterAction pm;
            result = 0;
            // jacky
            this.m_nBodyOffset = Actor.Units.Actor.GetOffset(this.m_wAppearance);
            pm = Actor.Units.Actor.GetRaceByPM(this.m_btRace, this.m_wAppearance);
            if (pm == null)
            {
                return result;
            }
            result = pm.ActStand.start + this.m_btDir;
            // * (pm.ActStand.frame + pm.ActStand.skip);

            return result;
        }

        public override void DrawChr(TDirectDrawSurface dsurface, int dx, int dy, bool blend, bool boFlag, bool DrawOnSale)
        {
            base.DrawChr(dsurface, dx, dy, blend, boFlag);
            if ((BrokenSurface != null) && !blend)
            {
                                                dsurface.Draw(dx + bx + this.m_nShiftX, dy + by + this.m_nShiftY, BrokenSurface.ClientRect, BrokenSurface, true);
            }
            if (this.m_boUseEffect && !blend)
            {
                if (EffectSurface != null)
                {
                    cliUtil.Units.cliUtil.DrawBlend(dsurface, dx + ax + this.m_nShiftX, dy + ay + this.m_nShiftY, EffectSurface, 1);
                }
            }
        }

        public override void Run()
        {
            if (this.m_boDeath)
            {
                if (bomarkpos)
                {
                   ClMain.Map.MarkCanWalk(this.m_nCurrX, this.m_nCurrY, true);
                    bomarkpos = false;
                }
            }
            else
            {
                if (!bomarkpos)
                {
                   ClMain.Map.MarkCanWalk(this.m_nCurrX, this.m_nCurrY, false);
                    bomarkpos = true;
                }
            }
           ClMain.g_PlayScene.SetActorDrawLevel(this, 0);
            base.Run();
        }

    } // end TWallStructure

    public class TSoccerBall: TActor
    {
    } // end TSoccerBall

    public class TDragonBody: TKillingHerb
    {
        // TDragonBody
        public override void CalcActorFrame()
        {
            TMonsterAction pm;
            this.m_btDir = 0;
            this.m_boUseMagic = false;
            this.m_nCurrentFrame =  -1;
            this.m_nBodyOffset = Actor.Units.Actor.GetOffset(this.m_wAppearance);
            pm = Actor.Units.Actor.GetRaceByPM(this.m_btRace, this.m_wAppearance);
            if (pm == null)
            {
                return;
            }
            switch(this.m_nCurrentAction)
            {
                case Grobal2.SM_DIGUP:
                    this.m_nMaxTick = pm.ActWalk.ftime;
                    this.m_nCurTick = 0;
                    this.m_nMoveStep = 1;
                    this.Shift(this.m_btDir, 0, 0, 1);
                    break;
                case Grobal2.SM_HIT:
                    AttackEff();
                    break;
            }
            this.m_nStartFrame = 0;
            this.m_nEndFrame = 1;
            this.m_dwFrameTime = 400;
            this.m_dwStartTime = MShare.GetTickCount();
        }

        public override void DrawEff(TDirectDrawSurface dsurface, int dx, int dy)
        {
            if (!(this.m_btDir >= 0 && this.m_btDir<= 7))
            {
                return;
            }
                        if (MShare.GetTickCount() - this.m_dwLoadSurfaceTime > g_dwLoadSurfaceTime)
            {
                this.m_dwLoadSurfaceTime = MShare.GetTickCount();
                LoadSurface();
            }
            // if m_BodySurface <> nil then
            // DrawBlend(dsurface, dx + m_nPx + m_nShiftX, dy + m_nPy + m_nShiftY, m_BodySurface, 1);

        }

        public override void LoadSurface()
        {
            this.m_BodySurface = null;
            // m_BodySurface := g_WDragonImg.GetCachedImage(GetOffset(m_wAppearance), m_nPx, m_nHpy);

        }

        private void AttackEff()
        {
            int n8;
            int nc;
            int n10;
            int n14;
            int n18;
            bool bo11;
            int i;
            int iCount;
            n8 = this.m_nCurrX;
            nc = this.m_nCurrY;
            iCount = (new System.Random(5)).Next();
            for (i = 0; i <= iCount; i ++ )
            {
                n10 = (new System.Random(4)).Next();
                n14 = (new System.Random(8)).Next();
                n18 = (new System.Random(8)).Next();
                switch(n10)
                {
                    case 0:
                       ClMain.g_PlayScene.NewMagic(this, 80, 80, this.m_nCurrX, this.m_nCurrY, n8 - n14 - 2, nc + n18 + 1, 0, magiceff.TMagicType.mtRedThunder, false, 30, ref bo11);
                        break;
                    case 1:
                       ClMain.g_PlayScene.NewMagic(this, 80, 80, this.m_nCurrX, this.m_nCurrY, n8 - n14, nc + n18, 0, magiceff.TMagicType.mtRedThunder, false, 30, ref bo11);
                        break;
                    case 2:
                       ClMain.g_PlayScene.NewMagic(this, 80, 80, this.m_nCurrX, this.m_nCurrY, n8 - n14, nc + n18 + 1, 0, magiceff.TMagicType.mtRedThunder, false, 30, ref bo11);
                        break;
                    case 3:
                       ClMain.g_PlayScene.NewMagic(this, 80, 80, this.m_nCurrX, this.m_nCurrY, n8 - n14 - 2, nc + n18, 0, magiceff.TMagicType.mtRedThunder, false, 30, ref bo11);
                        break;
                }
                //SoundUtil.g_SndMgr.PlaySound(8301, this.m_nCurrX, this.m_nCurrY);
            }
        }

    } // end TDragonBody

    public enum TDoorState
    {
        dsOpen,
        dsClose,
        dsBroken
    } // end TDoorState

}

namespace HerbActor.Units
{
    public class HerbActor
    {
        public const int BEEQUEENBASE = 600;
        public const int DOORDEATHEFFECTBASE = 120;
        public const int WALLLEFTBROKENEFFECTBASE = 224;
        public const int WALLRIGHTBROKENEFFECTBASE = 240;
    } // end HerbActor

}

