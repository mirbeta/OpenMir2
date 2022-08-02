using System;
using System.Collections;
using SystemModule;

namespace RobotSvr
{
    public class TNpcActor: TActor
    {
        private int m_nEffX = 0;
        private int m_nEffY = 0;
        private bool m_boDigUp = false;
        private long m_dwUseEffectTick = 0;

        public override void CalcActorFrame()
        {
            this.m_boUseMagic = false;
            this.m_boNewMagic = false;
            this.m_boUseCboLib = false;
            this.m_nCurrentFrame =  -1;
            this.m_nBodyOffset = Actor.GetNpcOffset(this.m_wAppearance);
            TMonsterAction pm = Actor.GetRaceByPM(this.m_btRace, this.m_wAppearance);
            if (pm == null)
            {
                return;
            }
            this.m_btDir = this.m_btDir % 3;
            switch(this.m_nCurrentAction)
            {
                case Grobal2.SM_TURN:
                    switch(this.m_wAppearance)
                    {
                        // Modify the A .. B: 54 .. 58, 112 .. 117
                        case 54:
                        case 112:
                            break;
                        // Modify the A .. B: 59, 70 .. 75, 81 .. 85, 90 .. 92, 94 .. 98, 118 .. 123, 130, 131, 132
                        case 59:
                        case 70:
                        case 81:
                        case 90:
                        case 94:
                        case 118:
                        case 130:
                        case 131:
                        case 132:
                            this.m_nStartFrame = pm.ActStand.start;
                            this.m_nEndFrame = this.m_nStartFrame + pm.ActStand.frame - 1;
                            this.m_dwFrameTime = pm.ActStand.ftime;
                            this.m_dwStartTime = MShare.GetTickCount();
                            this.m_nDefFrameCount = pm.ActStand.frame;
                            this.Shift(this.m_btDir, 0, 0, 1);
                            break;
                        default:
                            this.m_nStartFrame = pm.ActStand.start + this.m_btDir * (pm.ActStand.frame + pm.ActStand.skip);
                            this.m_nEndFrame = this.m_nStartFrame + pm.ActStand.frame - 1;
                            this.m_dwFrameTime = pm.ActStand.ftime;
                            this.m_dwStartTime = MShare.GetTickCount();
                            this.m_nDefFrameCount = pm.ActStand.frame;
                            this.Shift(this.m_btDir, 0, 0, 1);
                            break;
                    }
                    if (!this.m_boUseEffect)
                    {
                        if ((new ArrayList(new int[] {33, 34}).Contains(this.m_wAppearance)))
                        {
                            this.m_boUseEffect = true;
                            this.m_nEffectFrame = this.m_nEffectStart;
                            this.m_nEffectEnd = this.m_nEffectStart + 9;
                            this.m_dwEffectStartTime = MShare.GetTickCount();
                            this.m_dwEffectFrameTime = 300;
                        }
                        else if (new ArrayList(new int[] {54, 94}).Contains(this.m_wAppearance))
                        {
                            // m_nStartFrame := 0;
                            // m_nEndFrame := 0;
                            this.m_boUseEffect = true;
                            this.m_nEffectStart = 0;
                            this.m_nEffectEnd = 8;
                            this.m_nEffectFrame = 0;
                            this.m_dwEffectStartTime = MShare.GetTickCount();
                            this.m_dwEffectFrameTime = 150;
                        }
                        else if (this.m_wAppearance >= 42 && this.m_wAppearance<= 47)
                        {
                            this.m_nStartFrame = 20;
                            this.m_nEndFrame = 10;
                            this.m_boUseEffect = true;
                            this.m_nEffectStart = 0;
                            this.m_nEffectFrame = 0;
                            this.m_nEffectEnd = 19;
                            this.m_dwEffectStartTime = MShare.GetTickCount();
                            this.m_dwEffectFrameTime = 100;
                        }
                        else if (this.m_wAppearance >= 118 && this.m_wAppearance<= 120)
                        {
                            this.m_boUseEffect = true;
                            this.m_nEffectStart = 10;
                            this.m_nEffectEnd = 10 + 16 - 1;
                            this.m_nEffectFrame = 16;
                            this.m_dwEffectStartTime = MShare.GetTickCount();
                            this.m_dwEffectFrameTime = 200;
                        }
                        else if (this.m_wAppearance >= 122 && this.m_wAppearance<= 123)
                        {
                            this.m_boUseEffect = true;
                            this.m_nEffectStart = 20;
                            this.m_nEffectEnd = 20 + 9 - 1;
                            this.m_nEffectFrame = 9;
                            this.m_dwEffectStartTime = MShare.GetTickCount();
                            this.m_dwEffectFrameTime = 200;
                        }
                        else if (this.m_wAppearance == 131)
                        {
                            this.m_boUseEffect = true;
                            this.m_nEffectStart = 10;
                            this.m_nEffectEnd = 21;
                            this.m_nEffectFrame = 12;
                            this.m_dwEffectStartTime = MShare.GetTickCount();
                            this.m_dwEffectFrameTime = 100;
                        }
                        else if (this.m_wAppearance == 132)
                        {
                            this.m_boUseEffect = true;
                            this.m_nEffectStart = 20;
                            this.m_nEffectEnd = 39;
                            this.m_nEffectFrame = 20;
                            this.m_dwEffectStartTime = MShare.GetTickCount();
                            this.m_dwEffectFrameTime = 100;
                        }
                        else if (this.m_wAppearance == 51)
                        {
                            this.m_boUseEffect = true;
                            this.m_nEffectStart = 60;
                            this.m_nEffectFrame = this.m_nEffectStart;
                            this.m_nEffectEnd = this.m_nEffectStart + 7;
                            this.m_dwEffectStartTime = MShare.GetTickCount();
                            this.m_dwEffectFrameTime = 150;
                        }
                        else if (this.m_wAppearance >= 60 && this.m_wAppearance<= 67)
                        {
                            this.m_boUseEffect = true;
                            this.m_nEffectStart = 0;
                            this.m_nEffectFrame = this.m_nEffectStart;
                            this.m_nEffectEnd = this.m_nEffectStart + 3;
                            this.m_dwEffectStartTime = MShare.GetTickCount();
                            this.m_dwEffectFrameTime = 500;
                        }
                        else if (new ArrayList(new int[] {68}).Contains(this.m_wAppearance))
                        {
                            this.m_boUseEffect = true;
                            this.m_nEffectStart = 60;
                            this.m_nEffectFrame = this.m_nEffectStart;
                            this.m_nEffectEnd = this.m_nEffectStart + 3;
                            this.m_dwEffectStartTime = MShare.GetTickCount();
                            this.m_dwEffectFrameTime = 500;
                        }
                        else if (new ArrayList(new int[] {70, 90}).Contains(this.m_wAppearance))
                        {
                            this.m_boUseEffect = true;
                            this.m_nEffectStart = 4;
                            this.m_nEffectFrame = this.m_nEffectStart;
                            this.m_nEffectEnd = this.m_nEffectStart + 3;
                            this.m_dwEffectStartTime = MShare.GetTickCount();
                            this.m_dwEffectFrameTime = 500;
                        }
                    }
                    break;
                case Grobal2.SM_HIT:
                    switch(this.m_wAppearance)
                    {
                        // Modify the A .. B: 54 .. 58, 104 .. 106, 110, 112 .. 117, 121, 132, 133
                        case 54:
                        case 104:
                        case 110:
                        case 112:
                        case 121:
                        case 132:
                        case 133:
                            break;
                        case 33:
                        case 34:
                        case 52:
                            // 0710
                            this.m_nStartFrame = pm.ActStand.start + this.m_btDir * (pm.ActStand.frame + pm.ActStand.skip);
                            this.m_nEndFrame = this.m_nStartFrame + pm.ActStand.frame - 1;
                            this.m_dwStartTime = MShare.GetTickCount();
                            this.m_nDefFrameCount = pm.ActStand.frame;
                            break;
                        // Modify the A .. B: 59, 70 .. 75, 81 .. 85, 90 .. 92, 94 .. 98, 111, 130, 131, 118 .. 120, 122, 123
                        case 59:
                        case 70:
                        case 81:
                        case 90:
                        case 94:
                        case 111:
                        case 130:
                        case 131:
                        case 118:
                        case 122:
                        case 123:
                            this.m_nStartFrame = pm.ActAttack.start;
                            this.m_nEndFrame = this.m_nStartFrame + pm.ActAttack.frame - 1;
                            this.m_dwFrameTime = pm.ActAttack.ftime;
                            this.m_dwStartTime = MShare.GetTickCount();
                            if (this.m_wAppearance == 84)
                            {
                                m_boDigUp = true;
                                this.m_boUseEffect = true;
                                this.m_nEffectStart = 14;
                                this.m_nEffectFrame = this.m_nEffectStart;
                                this.m_nEffectEnd = this.m_nEffectStart + 7;
                                this.m_dwEffectStartTime = MShare.GetTickCount();
                                this.m_dwEffectFrameTime = this.m_dwFrameTime;
                            }
                            break;
                        default:
                            this.m_nStartFrame = pm.ActAttack.start + this.m_btDir * (pm.ActAttack.frame + pm.ActAttack.skip);
                            this.m_nEndFrame = this.m_nStartFrame + pm.ActAttack.frame - 1;
                            this.m_dwFrameTime = pm.ActAttack.ftime;
                            this.m_dwStartTime = MShare.GetTickCount();
                            if (this.m_wAppearance == 51)
                            {
                                this.m_boUseEffect = true;
                                this.m_nEffectStart = 60;
                                this.m_nEffectFrame = this.m_nEffectStart;
                                this.m_nEffectEnd = this.m_nEffectStart + 7;
                                this.m_dwEffectStartTime = MShare.GetTickCount();
                                this.m_dwEffectFrameTime = 200;
                            }
                            break;
                    }
                    break;
                case Grobal2.SM_DIGUP:
                    if (this.m_wAppearance == 52)
                    {
                        m_boDigUp = true;
                        m_dwUseEffectTick = MShare.GetTickCount() + 23000;
                        this.m_boUseEffect = true;
                        this.m_nEffectStart = 60;
                        this.m_nEffectFrame = this.m_nEffectStart;
                        this.m_nEffectEnd = this.m_nEffectStart + 11;
                        this.m_dwEffectStartTime = MShare.GetTickCount();
                        this.m_dwEffectFrameTime = 100;
                    }
                    if (new ArrayList(new int[] {84, 85}).Contains(this.m_wAppearance))
                    {
                        this.m_nStartFrame = pm.ActCritical.start;
                        this.m_nEndFrame = this.m_nStartFrame + pm.ActCritical.frame - 1;
                        this.m_dwFrameTime = pm.ActCritical.ftime;
                        this.m_dwStartTime = MShare.GetTickCount();
                    }
                    if (this.m_wAppearance == 85)
                    {
                        m_boDigUp = true;
                        this.m_boUseEffect = true;
                        this.m_nEffectStart = 127;
                        this.m_nEffectFrame = this.m_nEffectStart;
                        this.m_nEffectEnd = this.m_nEffectStart + 34;
                        this.m_dwEffectStartTime = MShare.GetTickCount();
                        this.m_dwEffectFrameTime = this.m_dwFrameTime;
                    }
                    break;
            }
        }

        public TNpcActor() : base()
        {
            this.m_boHitEffect = false;
            this.m_nHitEffectNumber = 0;
            m_boDigUp = false;
        }

        public override int GetDefaultFrame(bool wmode)
        {
            int result;
            int cf;
            TMonsterAction pm;
            result = 0;
            pm = Units.Actor.GetRaceByPM(this.m_btRace, this.m_wAppearance);
            if (pm == null)
            {
                return result;
            }
            this.m_btDir = this.m_btDir % 3;
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
            if (new ArrayList(new int[] {54, 94, 70, 81, 90, 112, 130}).Contains(this.m_wAppearance))
            {
                result = pm.ActStand.start + cf;
            }
            else
            {
                result = pm.ActStand.start + this.m_btDir * (pm.ActStand.frame + pm.ActStand.skip) + cf;
            }
            return result;
        }

        public override void Run()
        {
            int nEffectFrame;
            long dwEffectFrameTime;
            base.Run();
            nEffectFrame = this.m_nEffectFrame;
            if (this.m_boUseEffect)
            {
                if (this.m_boUseMagic)
                {
                    dwEffectFrameTime = Math.Round(this.m_dwEffectFrameTime / 3);
                }
                else
                {
                    dwEffectFrameTime = this.m_dwEffectFrameTime;
                }
                if (MShare.GetTickCount() - this.m_dwEffectStartTime > dwEffectFrameTime)
                {
                    this.m_dwEffectStartTime = MShare.GetTickCount();
                    if (this.m_nEffectFrame < this.m_nEffectEnd)
                    {
                        this.m_nEffectFrame ++;
                    }
                    else
                    {
                        if (m_boDigUp)
                        {
                            if (MShare.GetTickCount() > m_dwUseEffectTick)
                            {
                                this.m_boUseEffect = false;
                                m_boDigUp = false;
                                m_dwUseEffectTick = MShare.GetTickCount();
                            }
                            this.m_nEffectFrame = this.m_nEffectStart;
                        }
                        else
                        {
                            this.m_nEffectFrame = this.m_nEffectStart;
                        }
                        this.m_dwEffectStartTime = MShare.GetTickCount();
                    }
                }
            }
            if (nEffectFrame != this.m_nEffectFrame)
            {
                this.m_dwLoadSurfaceTime = MShare.GetTickCount();
                LoadSurface();
            }
        }

    }
}
