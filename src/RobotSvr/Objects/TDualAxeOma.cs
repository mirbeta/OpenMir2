using System;
using SystemModule;

namespace RobotSvr
{
    public class TDualAxeOma: TSkeletonOma
    {
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
            // 荤款靛 瓤苞
            this.RunActSound(this.m_nCurrentFrame - this.m_nStartFrame);
            // 橇贰烙付促 秦具 且老
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
                        // 悼累捞 场巢.
                        this.m_nCurrentAction = 0;
                        // 悼累 肯丰
                        this.m_boUseEffect = false;
                    }
                    if ((this.m_nCurrentAction == Grobal2.SM_FLYAXE) && (this.m_nCurrentFrame - this.m_nStartFrame == Units.AxeMon.AXEMONATTACKFRAME - 4))
                    {
                        // 付过 惯荤
                        meff = ((TFlyingAxe)(ClMain.g_PlayScene.NewFlyObject(this, this.m_nCurrX, this.m_nCurrY, this.m_nTargetX, this.m_nTargetY, this.m_nTargetRecog, magiceff.TMagicType.mtFlyAxe)));
                        if (meff != null)
                        {
                            meff.ImgLib = WMFile.Units.WMFile.g_WMons[3];
                            switch(this.m_btRace)
                            {
                                case 15:
                                    meff.FlyImageBase = magiceff.Units.magiceff.FLYOMAAXEBASE;
                                    break;
                                case 22:
                                    meff.FlyImageBase = magiceff.Units.magiceff.THORNBASE;
                                    break;
                                case 57:
                                    meff.FlyImageBase = 3586;
                                    meff.ImgLib = WMFile.Units.WMFile.g_WMons[33];
                                    break;
                                case 58:
                                    meff.FlyImageBase = 4016;
                                    meff.ImgLib = WMFile.Units.WMFile.g_WMons[33];
                                    break;
                            }
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

    } // end TDualAxeOma

    } // end TBanyaGuardMon

