using System;
using SystemModule;

namespace RobotSvr
{
    public class TDragon : TActor
    {
        protected int ax = 0;
        protected int ax2 = 0;
        protected int ay = 0;
        protected int ay2 = 0;
        protected byte firedir = 0;

        public TDragon() : base()
        {
            this.m_boUseEffect = false;
        }
        public override void CalcActorFrame()
        {
            this.m_nCurrentFrame = -1;
            this.m_boReverseFrame = false;
            this.m_boUseEffect = false;
            this.m_nBodyOffset = Actor.Units.Actor.GetOffset(this.m_wAppearance);
            TMonsterAction pm = Actor.Units.Actor.GetRaceByPM(this.m_btRace, this.m_wAppearance);
            if (pm == null)
            {
                return;
            }
            switch (this.m_nCurrentAction)
            {
                case Grobal2.SM_HIT:
                    this.m_nStartFrame = this.m_Action.ActAttack.start + this.m_btDir * (this.m_Action.ActAttack.frame + this.m_Action.ActAttack.skip);
                    this.m_nEndFrame = this.m_nStartFrame + this.m_Action.ActAttack.frame - 1;
                    this.m_dwFrameTime = this.m_Action.ActAttack.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    this.m_dwWarModeTime = MShare.GetTickCount();
                    this.Shift(this.m_btDir, 0, 0, 1);
                    break;
                case Grobal2.SM_LIGHTING:
                    this.m_nStartFrame = this.m_Action.ActAttack.start + this.m_btDir * (this.m_Action.ActAttack.frame + this.m_Action.ActAttack.skip);
                    this.m_nEndFrame = this.m_nStartFrame + this.m_Action.ActAttack.frame - 1;
                    this.m_dwFrameTime = this.m_Action.ActAttack.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    this.m_dwWarModeTime = MShare.GetTickCount();
                    this.m_boUseEffect = true;
                    this.m_dwEffectStartTime = MShare.GetTickCount();
                    firedir = this.m_btDir;
                    this.m_nEffectFrame = this.m_nStartFrame;
                    this.m_nEffectStart = this.m_nStartFrame;
                    this.m_nEffectEnd = this.m_nEndFrame;
                    this.m_dwEffectStartTime = MShare.GetTickCount();
                    this.m_dwEffectFrameTime = this.m_dwFrameTime;
                    break;
                default:
                    base.CalcActorFrame();
                    break;
            }
        }

        private void AttackEff()
        {
            if (this.m_boDeath)
            {
                return;
            }
        }

        public override void Run()
        {
            int prv;
            int nDir;
            long m_dwEffectframetimetime;
            long m_dwFrameTimetime;
            bool bofly;
            if ((this.m_nCurrentAction == Grobal2.SM_WALK) || (this.m_nCurrentAction == Grobal2.SM_BACKSTEP) || (this.m_nCurrentAction == Grobal2.SM_RUN) || (this.m_nCurrentAction == Grobal2.SM_HORSERUN))
            {
                return;
            }
            this.m_boMsgMuch = false;
            if (this.m_MsgList.Count >= MShare.MSGMUCH)
            {
                this.m_boMsgMuch = true;
            }
            if (this.m_boRunSound)
            {
                this.m_boRunSound = false;
            }
            if (this.m_boUseEffect)
            {
                if (this.m_boMsgMuch)
                {
                    m_dwEffectframetimetime = this.m_dwEffectFrameTime * 2 / 3;
                }
                else
                {
                    m_dwEffectframetimetime = this.m_dwEffectFrameTime;
                }
                if (MShare.GetTickCount() - this.m_dwEffectStartTime > m_dwEffectframetimetime)
                {
                    this.m_dwEffectStartTime = MShare.GetTickCount();
                    if (this.m_nEffectFrame < this.m_nEffectEnd)
                    {
                        this.m_nEffectFrame++;
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
                    m_dwFrameTimetime = this.m_dwFrameTime * 2 / 3;
                }
                else
                {
                    m_dwFrameTimetime = this.m_dwFrameTime;
                }
                if (MShare.GetTickCount() - this.m_dwStartTime > m_dwFrameTimetime)
                {
                    if (this.m_nCurrentFrame < this.m_nEndFrame)
                    {
                        this.m_nCurrentFrame++;
                        this.m_dwStartTime = MShare.GetTickCount();
                    }
                    else
                    {
                        this.m_nCurrentAction = 0;
                        this.m_boUseEffect = false;
                    }
                    if ((this.m_nCurrentAction == Grobal2.SM_LIGHTING))
                    {
                        AttackEff();
                    }
                    else if ((this.m_nCurrentAction == Grobal2.SM_HIT))
                    {
                        nDir = 81;
                        if (this.m_btDir <= 4)
                        {
                            nDir = 81;
                        }
                        else if (this.m_btDir == 5)
                        {
                            nDir = 82;
                        }
                        else if (this.m_btDir >= 6)
                        {
                            nDir = 83;
                        }
                        if ((this.m_nCurrentFrame - this.m_nStartFrame) == 4)
                        {
                           // ClMain.g_PlayScene.NewMagic(this, nDir, nDir, this.m_nCurrX, this.m_nCurrY, this.m_nTargetX, this.m_nTargetY, this.m_nTargetRecog, magiceff.TMagicType.mtFly, true, 30, ref bofly);
                        }
                    }
                }
                this.m_nCurrentDefFrame = 0;
                this.m_dwDefFrameTime = MShare.GetTickCount();
            }
            else
            {
                if ((MShare.GetTickCount() - this.m_dwSmoothMoveTime) > 200)
                {
                    if (MShare.GetTickCount() - this.m_dwDefFrameTime > 500)
                    {
                        this.m_dwDefFrameTime = MShare.GetTickCount();
                        this.m_nCurrentDefFrame++;
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
    }
}