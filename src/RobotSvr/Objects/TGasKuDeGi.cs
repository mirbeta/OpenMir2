using System;
using System.Collections;
using SystemModule;

namespace RobotSvr
{
    public class TGasKuDeGi: TActor
    {
        protected TDirectDrawSurface AttackEffectSurface = null;
        protected TDirectDrawSurface DieEffectSurface = null;
        protected bool BoUseDieEffect = false;
        // 0x258
        protected int firedir = 0;
        // 0x25C
        protected int fire16dir = 0;
        // 0c260
        protected int ax = 0;
        // 0x264
        protected int ay = 0;
        // 0x268
        protected int bx = 0;
        protected int by = 0;
        // ============================== TGasKuDeGi =============================
        //Constructor  Create()
        public TGasKuDeGi() : base()
        {
            AttackEffectSurface = null;
            DieEffectSurface = null;
            this.m_boUseEffect = false;
            BoUseDieEffect = false;
        }
        public override void CalcActorFrame()
        {
            TMonsterAction pm;
            TActor Actor;
            int scx;
            int scy;
            int stx;
            int sty;
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
                    this.m_nStartFrame = pm.ActStand.start + this.m_btDir * (pm.ActStand.frame + pm.ActStand.skip);
                    this.m_nEndFrame = this.m_nStartFrame + pm.ActStand.frame - 1;
                    this.m_dwFrameTime = pm.ActStand.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    this.m_nDefFrameCount = pm.ActStand.frame;
                    this.Shift(this.m_btDir, 0, 0, 1);
                    break;
                case Grobal2.SM_WALK:
                    this.m_nStartFrame = pm.ActWalk.start + this.m_btDir * (pm.ActWalk.frame + pm.ActWalk.skip);
                    this.m_nEndFrame = this.m_nStartFrame + pm.ActWalk.frame - 1;
                    this.m_dwFrameTime = pm.ActWalk.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    this.m_nMaxTick = pm.ActWalk.usetick;
                    this.m_nCurTick = 0;
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
                case Grobal2.SM_HIT:
                case Grobal2.SM_LIGHTING:
                    this.m_nStartFrame = pm.ActAttack.start + this.m_btDir * (pm.ActAttack.frame + pm.ActAttack.skip);
                    this.m_nEndFrame = this.m_nStartFrame + pm.ActAttack.frame - 1;
                    this.m_dwFrameTime = pm.ActAttack.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    this.m_dwWarModeTime = MShare.GetTickCount();
                    this.Shift(this.m_btDir, 0, 0, 1);
                    this.m_boUseEffect = true;
                    firedir = this.m_btDir;
                    this.m_nEffectFrame = this.m_nStartFrame;
                    this.m_nEffectStart = this.m_nStartFrame;
                    if (this.m_btRace == 20)
                    {
                        this.m_nEffectEnd = this.m_nEndFrame + 1;
                    }
                    else
                    {
                        this.m_nEffectEnd = this.m_nEndFrame;
                    }
                    this.m_dwEffectStartTime = MShare.GetTickCount();
                    this.m_dwEffectFrameTime = this.m_dwFrameTime;
                    Actor =ClMain.g_PlayScene.FindActor(this.m_nTargetRecog);
                    if (Actor != null)
                    {
                       ClMain.g_PlayScene.ScreenXYfromMCXY(this.m_nCurrX, this.m_nCurrY, ref scx, ref scy);
                       ClMain.g_PlayScene.ScreenXYfromMCXY(Actor.m_nCurrX, Actor.m_nCurrY, ref stx, ref sty);
                        fire16dir = ClFunc.GetFlyDirection16(scx, scy, stx, sty);
                    }
                    else
                    {
                        fire16dir = firedir * 2;
                    }
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
                    if ((new ArrayList(new int[] {40, 65}).Contains(this.m_btRace)))
                    {
                        BoUseDieEffect = true;
                    }
                    break;
                case Grobal2.SM_SKELETON:
                    this.m_nStartFrame = pm.ActDeath.start;
                    this.m_nEndFrame = this.m_nStartFrame + pm.ActDeath.frame - 1;
                    this.m_dwFrameTime = pm.ActDeath.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
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
                result = pm.ActStand.start + this.m_btDir * (pm.ActStand.frame + pm.ActStand.skip) + cf;
            }
            return result;
        }

        public override void LoadSurface()
        {
            base.LoadSurface();
            switch(this.m_btRace)
            {
                case 16:
                    // 攻击效果
                    // //洞蛆
                    if (this.m_boUseEffect)
                    {
                        AttackEffectSurface = WMFile.Units.WMFile.g_WMons[3].GetCachedImage(Units.AxeMon.KUDEGIGASBASE - 1 + (firedir * 10) + this.m_nEffectFrame - this.m_nEffectStart, ref ax, ref ay);
                    }
                    break;
                case 20:
                    // //火焰沃玛
                    if (this.m_boUseEffect)
                    {
                        AttackEffectSurface = WMFile.Units.WMFile.g_WMons[4].GetCachedImage(Units.AxeMon.COWMONFIREBASE + (firedir * 10) + this.m_nEffectFrame - this.m_nEffectStart, ref ax, ref ay);
                    }
                    break;
                case 21:
                    // //沃玛教主
                    if (this.m_boUseEffect)
                    {
                        AttackEffectSurface = WMFile.Units.WMFile.g_WMons[4].GetCachedImage(Units.AxeMon.COWMONLIGHTBASE + (firedir * 10) + this.m_nEffectFrame - this.m_nEffectStart, ref ax, ref ay);
                    }
                    break;
                case 24:
                    if (this.m_boUseEffect)
                    {
                        AttackEffectSurface = WMFile.Units.WMFile.g_WMons[1].GetCachedImage(Units.AxeMon.SUPERIORGUARDBASE + (this.m_btDir * 8) + this.m_nEffectFrame - this.m_nEffectStart, ref ax, ref ay);
                    }
                    break;
                case 40:
                    // //僵尸1
                    if (this.m_boUseEffect)
                    {
                        AttackEffectSurface = WMFile.Units.WMFile.g_WMons[5].GetCachedImage(Units.AxeMon.ZOMBILIGHTINGBASE + (fire16dir * 10) + this.m_nEffectFrame - this.m_nEffectStart, ref ax, ref ay);
                    }
                    if (BoUseDieEffect)
                    {
                        DieEffectSurface = WMFile.Units.WMFile.g_WMons[5].GetCachedImage(Units.AxeMon.ZOMBIDIEBASE + this.m_nCurrentFrame - this.m_nStartFrame, ref bx, ref by);
                    }
                    break;
                case 52:
                    // //楔蛾
                    if (this.m_boUseEffect)
                    {
                        AttackEffectSurface = WMFile.Units.WMFile.g_WMons[4].GetCachedImage(Units.AxeMon.MOTHPOISONGASBASE + (firedir * 10) + this.m_nEffectFrame - this.m_nEffectStart, ref ax, ref ay);
                    }
                    break;
                case 53:
                    // //粪虫
                    if (this.m_boUseEffect)
                    {
                        AttackEffectSurface = WMFile.Units.WMFile.g_WMons[3].GetCachedImage(Units.AxeMon.DUNGPOISONGASBASE + (firedir * 10) + this.m_nEffectFrame - this.m_nEffectStart, ref ax, ref ay);
                    }
                    break;
                case 64:
                    if (this.m_boUseEffect)
                    {
                        AttackEffectSurface = WMFile.Units.WMFile.g_WMons[20].GetCachedImage(720 + (firedir * 10) + this.m_nEffectFrame - this.m_nEffectStart, ref ax, ref ay);
                    }
                    break;
                case 65:
                    if (BoUseDieEffect)
                    {
                        DieEffectSurface = WMFile.Units.WMFile.g_WMons[20].GetCachedImage(350 + this.m_nCurrentFrame - this.m_nStartFrame, ref bx, ref by);
                    }
                    break;
                case 66:
                    if (BoUseDieEffect)
                    {
                        DieEffectSurface = WMFile.Units.WMFile.g_WMons[20].GetCachedImage(1600 + this.m_nCurrentFrame - this.m_nStartFrame, ref bx, ref by);
                    }
                    break;
                case 67:
                    if (BoUseDieEffect)
                    {
                        DieEffectSurface = WMFile.Units.WMFile.g_WMons[20].GetCachedImage(1160 + (this.m_btDir * 10) + this.m_nCurrentFrame - this.m_nStartFrame, ref bx, ref by);
                    }
                    break;
                case 68:
                    if (BoUseDieEffect)
                    {
                        DieEffectSurface = WMFile.Units.WMFile.g_WMons[20].GetCachedImage(1600 + this.m_nCurrentFrame - this.m_nStartFrame, ref bx, ref by);
                    }
                    break;
            }
        }

        public override void Run()
        {
            int prv;
            long m_dwEffectFrameTimetime;
            long m_dwFrameTimetime;
            if ((this.m_nCurrentAction == Grobal2.SM_WALK) || (this.m_nCurrentAction == Grobal2.SM_BACKSTEP) || (this.m_nCurrentAction == Grobal2.SM_RUN) || (this.m_nCurrentAction == Grobal2.SM_HORSERUN))
            {
                return;
            }
            this.m_boMsgMuch = false;
            if (this.m_MsgList.Count >= MShare.MSGMUCH)
            {
                this.m_boMsgMuch = true;
            }
            this.RunActSound(this.m_nCurrentFrame - this.m_nStartFrame);
            this.RunFrameAction(this.m_nCurrentFrame - this.m_nStartFrame);
            if (this.m_boUseEffect)
            {
                if (this.m_boMsgMuch)
                {
                    m_dwEffectFrameTimetime = Math.Round(this.m_dwEffectFrameTime * 2 / 3);
                }
                else
                {
                    m_dwEffectFrameTimetime = this.m_dwEffectFrameTime;
                }
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
            prv = this.m_nCurrentFrame;
            if (this.m_nCurrentAction != 0)
            {
                if ((this.m_nCurrentFrame < this.m_nStartFrame) || (this.m_nCurrentFrame > this.m_nEndFrame))
                {
                    this.m_nCurrentFrame = this.m_nStartFrame;
                }
                if (this.m_boMsgMuch)
                {
                    m_dwFrameTimetime = Math.Round(this.m_dwFrameTime * 2 / 3);
                }
                else
                {
                    m_dwFrameTimetime = this.m_dwFrameTime;
                }
                if (MShare.GetTickCount() - this.m_dwStartTime > m_dwFrameTimetime)
                {
                    if (this.m_nCurrentFrame < this.m_nEndFrame)
                    {
                        this.m_nCurrentFrame ++;
                        this.m_dwStartTime = MShare.GetTickCount();
                    }
                    else
                    {
                        this.m_nCurrentAction = 0;
                        BoUseDieEffect = false;
                    }
                }
                this.m_nCurrentDefFrame = 0;
                this.m_dwDefFrameTime = MShare.GetTickCount();
            }
            else
            {
                if (((int)MShare.GetTickCount() - this.m_dwSmoothMoveTime) > 200)
                {
                    if (MShare.GetTickCount() - this.m_dwDefFrameTime > 500)
                    {
                        this.m_dwDefFrameTime = MShare.GetTickCount();
                        this.m_nCurrentDefFrame ++;
                        if (this.m_nCurrentDefFrame >= this.m_nDefFrameCount)
                        {
                            this.m_nCurrentDefFrame = 0;
                        }
                    }
                    this.DefaultMotion();
                }
            }
            if (prv != this.m_nCurrentFrame)
            {
                this.m_dwLoadSurfaceTime = MShare.GetTickCount();
                LoadSurface();
            }
        }

        public override void DrawChr(TDirectDrawSurface dsurface, int dx, int dy, bool blend, bool boFlag, bool DrawOnSale)
        {
            TColorEffect ceff;
            if (!(this.m_btDir >= 0 && this.m_btDir<= 7))
            {
                return;
            }
                        if (MShare.GetTickCount() - this.m_dwLoadSurfaceTime > g_dwLoadSurfaceTime)
            {
                this.m_dwLoadSurfaceTime = MShare.GetTickCount();
                LoadSurface();
            }
            ceff = this.GetDrawEffectValue();
            if (this.m_BodySurface != null)
            {
                this.DrawEffSurface(dsurface, this.m_BodySurface, dx + this.m_nPx + this.m_nShiftX, dy + this.m_nPy + this.m_nShiftY, blend, ceff);
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
            if (BoUseDieEffect)
            {
                if (DieEffectSurface != null)
                {
                    cliUtil.Units.cliUtil.DrawBlend(dsurface, dx + bx + this.m_nShiftX, dy + by + this.m_nShiftY, DieEffectSurface, 1);
                }
            }
        }

    } // end TGasKuDeGi

    } // end TBanyaGuardMon

