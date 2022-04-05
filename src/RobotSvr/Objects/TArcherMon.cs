using System;
using SystemModule;

namespace RobotSvr
{
    public class TArcherMon: TCatMon
    {
        // ============================= TArcherMon =============================
        public override void Run()
        {
            int prv;
            long m_dwFrameTimetime;
            TFlyingAxe meff;
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
                        this.m_boUseEffect = false;
                    }
                    if ((this.m_nCurrentAction == Grobal2.SM_FLYAXE) && (this.m_nCurrentFrame - this.m_nStartFrame == 4))
                    {
                        meff = ((TFlyingArrow)(ClMain.g_PlayScene.NewFlyObject(this, this.m_nCurrX, this.m_nCurrY, this.m_nTargetX, this.m_nTargetY, this.m_nTargetRecog, magiceff.TMagicType.mtFlyArrow)));
                        if (meff != null)
                        {
                            meff.ImgLib = WMFile.Units.WMFile.g_WEffectImg;
                            meff.NextFrameTime = 30;
                            meff.FlyImageBase = magiceff.Units.magiceff.ARCHERBASE2;
                        }
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
                this.LoadSurface();
            }
        }

    } // end TArcherMon

    } // end TBanyaGuardMon

