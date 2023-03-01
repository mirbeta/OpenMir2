using System.Collections;
using System.Collections.Generic;
using BotSrv.Player;
using SystemModule;
using SystemModule.Enums;
using SystemModule.Packets.ClientPackets;

namespace BotSrv.Objects
{
    public class THumActor : Actor
    {
        private readonly bool m_boHideWeapon = false;
        private readonly bool m_boSSkill;
        public int m_nFrame;
        public IList<Actor> m_SlaveObject;
        public TStallMgr m_StallMgr;

        public THumActor(RobotPlayer robotClient) : base(robotClient)
        {
            m_StallMgr = new TStallMgr();
            m_SlaveObject = new List<Actor>();
            m_boSSkill = false;
            m_dwFrameTime = 150;
            m_nFrame = 0;
            m_nHumWinOffset = 0;
            m_nCboHumWinOffSet = 0;
        }

        protected void CalcActorWinFrame()
        {
            if (m_btEffect == 50)
                m_nCboHumWinOffSet = 352;
            else if (m_btEffect != 0) m_nCboHumWinOffSet = (m_btEffect - 1) * 2000;
        }

        public override void ActionEnded()
        {
            //if (MShare.g_SeriesSkillFire)
            //{
            //    if (MShare.MagicLockActor == null || MShare.MagicLockActor.m_boDeath)
            //    {
            //        MShare.g_SeriesSkillFire = false;
            //        MShare.g_SeriesSkillStep = 0;
            //    }

            //    if (m_boUseMagic && this == MShare.MySelf && MShare.MagicLockActor != null &&
            //        !MShare.MagicLockActor.m_boDeath && MShare.g_nCurrentMagic <= 3)
            //        if (m_nCurrentFrame - m_nStartFrame >= m_nSpellFrame - 1)
            //        {
            //            if (MShare.g_MagicArr[MShare.g_SeriesSkillArr[MShare.g_nCurrentMagic]] != null)
            //            {
            //                // ClMain.frmMain.UseMagic(MShare.g_nMouseX, MShare.g_nMouseY, MShare.g_MagicArr[MShare.g_SeriesSkillArr[MShare.g_nCurrentMagic]], false, true);
            //            }

            //            MShare.g_nCurrentMagic++;
            //            if (MShare.g_nCurrentMagic > HUtil32._MIN(3, MShare.g_SeriesSkillStep))
            //            {
            //                MShare.g_SeriesSkillFire = false;
            //                MShare.g_SeriesSkillStep = 0;
            //            }
            //        }
            //}
        }

        public override void ReadyNextAction()
        {
            //if (m_boUseCboLib && m_boHitEffect && this == MShare.MySelf && MShare.g_nCurrentMagic2 < 4)
            //    if (m_nCurrentFrame - m_nStartFrame == 2)
            //    {
            //        if (MShare.g_MagicArr[MShare.g_SeriesSkillArr[MShare.g_nCurrentMagic2]] != null)
            //        {
            //            // ClMain.frmMain.UseMagic(MShare.g_nMouseX, MShare.g_nMouseY, MShare.g_MagicArr[MShare.g_SeriesSkillArr[MShare.g_nCurrentMagic2]], false, true);
            //        }

            //        MShare.g_nCurrentMagic2++;
            //        if (MShare.g_nCurrentMagic2 > HUtil32._MIN(4, MShare.g_SeriesSkillStep))
            //            MShare.g_SeriesSkillFire = false;
            //    }
        }

        public override void DefaultMotion()
        {

        }

        public override int GetDefaultFrame(bool wmode)
        {
            int result;
            int cf;
            if (Death)
            {
                result = THumAction.HA.ActDie.start + m_btDir * (THumAction.HA.ActDie.frame + THumAction.HA.ActDie.skip) +
                         (THumAction.HA.ActDie.frame - 1);
            }
            else if (wmode)
            {
                result = THumAction.HA.ActWarMode.start +
                         m_btDir * (THumAction.HA.ActWarMode.frame + THumAction.HA.ActWarMode.skip);
            }
            else
            {
                m_nDefFrameCount = THumAction.HA.ActStand.frame;
                if (m_nCurrentDefFrame < 0)
                    cf = 0;
                else if (m_nCurrentDefFrame >= THumAction.HA.ActStand.frame)
                    cf = 0;
                else
                    cf = m_nCurrentDefFrame;
                result = THumAction.HA.ActStand.start +
                         m_btDir * (THumAction.HA.ActStand.frame + THumAction.HA.ActStand.skip) + cf;
            }

            return result;
        }

        public override void RunFrameAction(int frame)
        {
            //m_boHideWeapon = false;
            //if (m_boSSkill)
            //{
            //    if (frame == 1)
            //    {
            //        m_boSSkill = false;
            //    }
            //}
            //else if (this.m_nCurrentAction == Messages.SM_HEAVYHIT)
            //{
            //    if ((frame == 5) && this.m_boDigFragment)
            //    {
            //        this.m_boDigFragment = false;
            //        var __event = ClMain.EventMan.GetEvent(this.m_nCurrX, this.m_nCurrY, Grobal2.ET_PILESTONES);
            //        if (__event != null)
            //        {
            //            __event.m_nEventParam = __event.m_nEventParam + 1;
            //        }
            //    }
            //}
            //else if (this.m_nCurrentAction == Messages.SM_SITDOWN)
            //{
            //    if ((frame == 5) && this.m_boDigFragment)
            //    {
            //        this.m_boDigFragment = false;
            //        var __event = ClMain.EventMan.GetEvent(this.m_nCurrX, this.m_nCurrY, Grobal2.ET_PILESTONES);
            //        if (__event != null)
            //        {
            //            __event.m_nEventParam = __event.m_nEventParam + 1;
            //        }
            //    }
            //}
            //else if (this.m_nCurrentAction == Messages.SM_THROW)
            //{
            //    if ((frame == 3) && this.m_boThrow)
            //    {
            //        this.m_boThrow = false;
            //    }
            //    if (frame >= 3)
            //    {
            //        m_boHideWeapon = true;
            //    }
            //}
        }

        public void DoWeaponBreakEffect()
        {

        }

        //public bool Run_MagicTimeOut()
        //{
        //    bool result;
        //    if (this == MShare.g_MySelf)
        //    {
        //        result = MShare.GetTickCount() - this.m_dwWaitMagicRequest > 1800;
        //    }
        //    else
        //    {
        //        result = MShare.GetTickCount() - this.m_dwWaitMagicRequest > 850 + (byte)bss * 50;
        //    }
        //    if (!bss && result)
        //    {
        //        this.m_CurMagic.ServerMagicCode = 0;
        //    }
        //    return result;
        //}

        public override void Run()
        {
            int off;
            int prv;
            int dwFrameTimetime;
            bool bss;
            bool sskill;
            bool fAddNewMagic;
            if (MShare.GetTickCount() - m_dwGenAnicountTime > 120)
            {
                m_dwGenAnicountTime = MShare.GetTickCount();
                m_nGenAniCount++;
                if (m_nGenAniCount > 100000) m_nGenAniCount = 0;
            }
            if (new ArrayList(new[] { 5, 9, 11, 13, 39 }).Contains(m_nCurrentAction)) return;
            m_boMsgMuch = this != MShare.MySelf && m_MsgList.Count >= 2;
            bss = new ArrayList(new[] { 105, 109 }).Contains(m_CurMagic.EffectNumber);
            off = m_nCurrentFrame - m_nStartFrame;
            RunFrameAction(off);
            prv = m_nCurrentFrame;
            if (m_nCurrentAction != 0)
            {
                if (m_nCurrentFrame < m_nStartFrame || m_nCurrentFrame > m_nEndFrame) m_nCurrentFrame = m_nStartFrame;
                if (m_boMsgMuch)
                {
                    if (m_boUseCboLib)
                    {
                        if (m_btIsHero == 1)
                            dwFrameTimetime = HUtil32.Round(m_dwFrameTime / 1.50);
                        else
                            dwFrameTimetime = HUtil32.Round(m_dwFrameTime / 1.55);
                    }
                    else
                    {
                        dwFrameTimetime = HUtil32.Round(m_dwFrameTime / 1.7);
                    }
                }
                else if (this != MShare.MySelf && m_boUseMagic)
                {
                    if (m_boUseCboLib)
                    {
                        if (m_btIsHero == 1)
                            dwFrameTimetime = HUtil32.Round(m_dwFrameTime / 1.28);
                        else
                            dwFrameTimetime = HUtil32.Round(m_dwFrameTime / 1.32);
                    }
                    else
                    {
                        dwFrameTimetime = HUtil32.Round(m_dwFrameTime / 1.38);
                    }
                }
                else
                {
                    dwFrameTimetime = m_dwFrameTime;
                }

                if (MShare.SpeedRate)
                    dwFrameTimetime = HUtil32._MAX(0, dwFrameTimetime - HUtil32._MIN(10, MShare.g_MoveSpeedRate));
                if (MShare.GetTickCount() - m_dwStartTime > dwFrameTimetime)
                {
                    if (m_nCurrentFrame < m_nEndFrame)
                    {
                        if (m_boUseMagic)
                        {
                            // 魔法效果...
                            if (m_nCurEffFrame == m_nSpellFrame - 2 || Run_MagicTimeOut())
                            {
                                if (m_CurMagic.ServerMagicCode >= 0 || Run_MagicTimeOut())
                                {
                                    m_nCurrentFrame++;
                                    m_nCurEffFrame++;
                                    m_dwStartTime = MShare.GetTickCount();
                                }
                            }
                            else
                            {
                                if (m_nCurrentFrame < m_nEndFrame - 1) m_nCurrentFrame++;
                                m_nCurEffFrame++;
                                m_dwStartTime = MShare.GetTickCount();
                            }
                        }
                        else
                        {
                            m_nCurrentFrame++;
                            m_dwStartTime = MShare.GetTickCount();
                        }

                        ReadyNextAction();
                    }
                    else
                    {
                        if (this == MShare.MySelf)
                        {
                            if (robotClient.ServerAcceptNextAction())
                            {
                                ActionEnded();
                                m_nCurrentAction = 0;
                                m_boUseMagic = false;
                                m_boUseCboLib = false;
                            }
                        }
                        else
                        {
                            ActionEnded();
                            m_nCurrentAction = 0;
                            m_boUseMagic = false;
                            m_boUseCboLib = false;
                        }

                        m_boHitEffect = false;
                        if (m_boSmiteLongHit == 1)
                        {
                            m_boSmiteLongHit = 2;
                            CalcActorWinFrame();
                            m_nStartFrame = THumAction.HA.ActSmiteLongHit2.start + m_btDir *
                                (THumAction.HA.ActSmiteLongHit2.frame + THumAction.HA.ActSmiteLongHit2.skip);
                            m_nEndFrame = m_nStartFrame + THumAction.HA.ActSmiteLongHit2.frame - 1;
                            m_dwFrameTime = THumAction.HA.ActSmiteLongHit2.ftime;
                            m_dwStartTime = MShare.GetTickCount();
                            m_nMaxTick = THumAction.HA.ActSmiteLongHit2.usetick;
                            m_nCurTick = 0;
                            Shift(m_btDir, 0, 0, 1);
                            m_boWarMode = true;
                            m_dwWarModeTime = MShare.GetTickCount();
                            m_boUseCboLib = true;
                            m_boHitEffect = true;
                        }
                    }

                    if (m_boUseMagic)
                    {
                        if (bss && m_CurMagic.ServerMagicCode > 0)
                        {
                            sskill = false;
                            switch (m_CurMagic.EffectNumber)
                            {
                                case 105:
                                case 106:
                                    sskill = m_nCurEffFrame == 7;
                                    break;
                                case 107:
                                case 109:
                                    sskill = m_nCurEffFrame == 9;
                                    break;
                                case 110:
                                    sskill = new ArrayList(new[] { 6, 8, 10 }).Contains(m_nCurEffFrame);
                                    break;
                                case 111:
                                    sskill = m_nCurEffFrame == 10;
                                    break;
                            }
                            if (sskill)
                                //TUseMagicInfo _wvar1 = this.m_CurMagic;
                                //ClMain.g_PlayScene.NewMagic(this, _wvar1.ServerMagicCode, _wvar1.EffectNumber, this.m_nCurrX, this.m_nCurrY, _wvar1.targx, _wvar1.targy, _wvar1.target, _wvar1.EffectType, _wvar1.Recusion, _wvar1.anitime, ref boFly, _wvar1.magfirelv, _wvar1.Poison);
                                m_boNewMagic = false;
                        }
                        switch (m_CurMagic.EffectNumber)
                        {
                            case 127:
                                fAddNewMagic = m_nCurEffFrame == 7;
                                break;
                            default:
                                fAddNewMagic = m_nCurEffFrame == m_nSpellFrame - 1;
                                break;
                        }

                        if (fAddNewMagic)
                        {
                            if (m_CurMagic.ServerMagicCode > 0 && (!bss || m_boNewMagic))
                            {
                                //TUseMagicInfo _wvar2 = this.m_CurMagic;
                                //ClMain.g_PlayScene.NewMagic(this, _wvar2.ServerMagicCode, _wvar2.EffectNumber, this.m_nCurrX, this.m_nCurrY, _wvar2.targx, _wvar2.targy, _wvar2.target, _wvar2.EffectType, _wvar2.Recusion, _wvar2.anitime, ref boFly, _wvar2.magfirelv, _wvar2.Poison);
                            }

                            if (this == MShare.MySelf) MShare.LatestSpellTick = MShare.GetTickCount();
                            m_CurMagic.ServerMagicCode = 0;
                        }
                    }
                }

                if (Race == ActorRace.Play)
                    m_nCurrentDefFrame = 0;
                else
                    m_nCurrentDefFrame = -10;
                m_dwDefFrameTime = MShare.GetTickCount();
            }
            else if (MShare.GetTickCount() - m_dwSmoothMoveTime > 200)
            {
                if (MShare.GetTickCount() - m_dwDefFrameTime > 500)
                {
                    m_dwDefFrameTime = MShare.GetTickCount();
                    m_nCurrentDefFrame++;
                    if (m_nCurrentDefFrame >= m_nDefFrameCount) m_nCurrentDefFrame = 0;
                }

                DefaultMotion();
            }

            if (prv != m_nCurrentFrame) m_dwLoadSurfaceTime = MShare.GetTickCount();
        }

        public override int light()
        {
            int L = m_nChrLight;
            if (L < m_nMagLight)
                if (m_boUseMagic || m_boHitEffect)
                    L = m_nMagLight;
            return L;
        }
    }

    public class TStallMgr
    {
        public TClientStallInfo mBlock;
        public bool OnSale;

        public TStallMgr()
        {
            mBlock = new TClientStallInfo();
        }
    }

    public class TClientStallInfo
    {
        public int ItemCount;
        public ClientItem[] Items = new ClientItem[10];
        public string StallName;
    }
}

