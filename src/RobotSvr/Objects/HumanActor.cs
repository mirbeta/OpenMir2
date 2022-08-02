using System;
using System.Collections;
using SystemModule;

namespace RobotSvr
{
    public struct THumanAction
    {
        public TActionInfo ActStand;
        public TActionInfo ActWalk;
        public TActionInfo ActRun;
        public TActionInfo ActRushLeft;
        public TActionInfo ActRushRight;
        public TActionInfo ActWarMode;
        public TActionInfo ActHit;
        public TActionInfo ActUnitHit;
        public TActionInfo ActHeavyHit;
        public TActionInfo ActBigHit;
        public TActionInfo ActFireHitReady;
        public TActionInfo ActSpell;
        public TActionInfo ActSitdown;
        public TActionInfo ActStruck;
        public TActionInfo ActDie;
        public TActionInfo ActRush2;
        public TActionInfo ActSmiteHit;
        public TActionInfo ActSmiteLongHit;
        public TActionInfo ActSmiteLongHit2;
        public TActionInfo ActSmiteLongHit3;
        public TActionInfo ActSmiteWideHit;
        public TActionInfo ActMagic_104;
        public TActionInfo ActMagic_105;
        public TActionInfo ActMagic_106;
        public TActionInfo ActMagic_107;
        public TActionInfo ActMagic_108;
        public TActionInfo ActMagic_109;
        public TActionInfo ActMagic_110;
        public TActionInfo ActMagic_111;
        public TActionInfo ActMagic_112;
        public TActionInfo ActMagic_113;
        public TActionInfo ActMagic_114;
    }

    public class THumAction
    {

        public static THumanAction HA = new THumanAction()
        {
            ActStand = new TActionInfo { start = 0, frame = 4, skip = 4, ftime = 200, usetick = 0 },
            ActWalk = new TActionInfo { start = 64, frame = 6, skip = 2, ftime = 90, usetick = 2 },
            ActRun = new TActionInfo { start = 128, frame = 6, skip = 2, ftime = 115, usetick = 3 },
            ActRushLeft = new TActionInfo { start = 128, frame = 3, skip = 5, ftime = 120, usetick = 3 },
            ActRushRight = new TActionInfo { start = 131, frame = 3, skip = 5, ftime = 120, usetick = 3 },
            ActWarMode = new TActionInfo { start = 192, frame = 1, skip = 0, ftime = 200, usetick = 0 },
            ActHit = new TActionInfo { start = 200, frame = 6, skip = 2, ftime = 85, usetick = 0 },
            ActUnitHit = new TActionInfo { start = 200, frame = 17, skip = 2, ftime = 85, usetick = 0 },
            ActHeavyHit = new TActionInfo { start = 264, frame = 6, skip = 2, ftime = 90, usetick = 0 },
            ActBigHit = new TActionInfo { start = 328, frame = 8, skip = 0, ftime = 70, usetick = 0 },
            ActFireHitReady = new TActionInfo { start = 192, frame = 1, skip = 0, ftime = 100, usetick = 0 },
            ActSpell = new TActionInfo { start = 392, frame = 6, skip = 2, ftime = 60, usetick = 0 },
            ActSitdown = new TActionInfo { start = 456, frame = 2, skip = 0, ftime = 300, usetick = 0 },
            ActStruck = new TActionInfo { start = 472, frame = 3, skip = 5, ftime = 70, usetick = 0 },
            ActDie = new TActionInfo { start = 536, frame = 4, skip = 4, ftime = 120, usetick = 0 },
            ActRush2 = new TActionInfo { start = 080, frame = 8, skip = 2, ftime = 77, usetick = 3 },
            ActSmiteHit = new TActionInfo { start = 160, skip = 15, frame = 5, ftime = 56, usetick = 0 },
            ActSmiteLongHit = new TActionInfo { start = 1920, frame = 5, skip = 5, ftime = 45, usetick = 0 },
            ActSmiteLongHit2 = new TActionInfo { start = 320, frame = 6, skip = 4, ftime = 80, usetick = 0 },
            ActSmiteLongHit3 = new TActionInfo { start = 320, frame = 6, skip = 4, ftime = 100, usetick = 0 },
            ActSmiteWideHit = new TActionInfo { start = 560, frame = 10, skip = 0, ftime = 78, usetick = 0 },
            ActMagic_104 = new TActionInfo { start = 640, frame = 6, skip = 4, ftime = 92, usetick = 0 },
            ActMagic_105 = new TActionInfo { start = 880, frame = 10, skip = 0, ftime = 88, usetick = 0 },
            ActMagic_106 = new TActionInfo { start = 800, frame = 8, skip = 2, ftime = 88, usetick = 0 },
            ActMagic_107 = new TActionInfo { start = 1040, frame = 13, skip = 7, ftime = 72, usetick = 0 },
            ActMagic_108 = new TActionInfo { start = 1200, frame = 6, skip = 4, ftime = 95, usetick = 0 },
            ActMagic_109 = new TActionInfo { start = 1440, frame = 12, skip = 8, ftime = 78, usetick = 0 },
            ActMagic_110 = new TActionInfo { start = 1600, frame = 12, skip = 8, ftime = 78, usetick = 0 },
            ActMagic_111 = new TActionInfo { start = 1760, frame = 14, skip = 6, ftime = 65, usetick = 0 },
            ActMagic_112 = new TActionInfo { start = 720, frame = 6, skip = 4, ftime = 95, usetick = 0 },
            ActMagic_113 = new TActionInfo { start = 400, frame = 12, skip = 8, ftime = 70, usetick = 0 },
            ActMagic_114 = new TActionInfo { start = 400, frame = 12, skip = 8, ftime = 85, usetick = 0 }
        };
    }

    // ActFireHitReady: (start: 192; frame: 6; skip: 4; ftime: 70; usetick: 0);
    public class THumActor : TActor
    {
        private bool m_boSSkill = false;
        private bool m_boWeaponEffect = false;
        private int m_nCurWeaponEffect = 0;
        private int m_nCurBubbleStruck = 0;
        private long m_dwWeaponpEffectTime = 0;
        private bool m_boHideWeapon = false;
        public TStallMgr m_StallMgr = null;
        public int m_nFrame = 0;
        public long m_dwFrameTick = 0;
        public long m_dwFrameTime = 0;
        public ArrayList m_SlaveObject = null;
        public TActor m_HeroObject = null;
        //Constructor  Create()
        public THumActor() : base()
        {
            m_SlaveObject = new ArrayList();
            m_HeroObject = null;
            m_boWeaponEffect = false;
            m_boSSkill = false;
            m_dwFrameTime = 150;
            m_dwFrameTick = MShare.GetTickCount();
            m_nFrame = 0;
            this.m_nHumWinOffset = 0;
            this.m_nCboHumWinOffSet = 0;
            m_StallMgr = new TStallMgr();
        }

        protected void CalcActorWinFrame()
        {
            if ((this.m_btEffect == 50))
            {
                this.m_nCboHumWinOffSet = 352;
            }
            else if (this.m_btEffect != 0)
            {
                this.m_nCboHumWinOffSet = (this.m_btEffect - 1) * 2000;
            }
        }

        public override void ActionEnded()
        {
            if (MShare.g_SeriesSkillFire)
            {
                if ((MShare.g_MagicLockActor == null) || MShare.g_MagicLockActor.m_boDeath)
                {
                    MShare.g_SeriesSkillFire = false;
                    MShare.g_SeriesSkillStep = 0;
                }
                if (this.m_boUseMagic && (this == MShare.g_MySelf) && (MShare.g_MagicLockActor != null) && (!MShare.g_MagicLockActor.m_boDeath) && (MShare.g_nCurrentMagic <= 3))
                {
                    if ((this.m_nCurrentFrame - this.m_nStartFrame) >= (this.m_nSpellFrame - 1))
                    {
                        if (MShare.g_MagicArr[0][MShare.g_SeriesSkillArr[MShare.g_nCurrentMagic]] != null)
                        {
                            ClMain.frmMain.UseMagic(MShare.g_nMouseX, MShare.g_nMouseY, MShare.g_MagicArr[0][MShare.g_SeriesSkillArr[MShare.g_nCurrentMagic]], false, true);
                        }
                        MShare.g_nCurrentMagic++;
                        if (MShare.g_nCurrentMagic > HUtil32._MIN(3, MShare.g_SeriesSkillStep))
                        {
                            MShare.g_SeriesSkillFire = false;
                            MShare.g_SeriesSkillStep = 0;
                        }
                    }
                }
            }
        }

        public override void ReadyNextAction()
        {
            if (this.m_boUseCboLib && this.m_boHitEffect && (this == MShare.g_MySelf) && (MShare.g_nCurrentMagic2 < 4))
            {
                if ((this.m_nCurrentFrame - this.m_nStartFrame) == 2)
                {
                    if (MShare.g_MagicArr[0][MShare.g_SeriesSkillArr[MShare.g_nCurrentMagic2]] != null)
                    {
                        ClMain.frmMain.UseMagic(MShare.g_nMouseX, MShare.g_nMouseY, MShare.g_MagicArr[0][MShare.g_SeriesSkillArr[MShare.g_nCurrentMagic2]], false, true);
                    }
                    MShare.g_nCurrentMagic2++;
                    if (MShare.g_nCurrentMagic2 > HUtil32._MIN(4, MShare.g_SeriesSkillStep))
                    {
                        MShare.g_SeriesSkillFire = false;
                    }
                }
            }
        }

        public override void CalcActorFrame()
        {
            int nHairEx;
            int haircount;
            TMagicEff Effect;
            this.m_boUseMagic = false;
            this.m_boNewMagic = false;
            this.m_boUseCboLib = false;
            this.m_boHitEffect = false;
            this.m_nHitEffectNumber = 0;
            this.m_nCurrentFrame = -1;
            this.m_btHair = Grobal2.HAIRfeature(this.m_nFeature);
            this.m_btDress = Grobal2.DRESSfeature(this.m_nFeature);
            if (this.m_btDress >= 24 && this.m_btDress <= 27)
            {
                this.m_btDress = 18 + this.m_btSex;
            }
            this.m_btWeapon = Grobal2.WEAPONfeature(this.m_nFeature);
            this.m_btHorse = Grobal2.Horsefeature(this.m_nFeatureEx);
            this.m_btWeaponEffect = this.m_btHorse;
            this.m_btHorse = 0;
            this.m_btEffect = Grobal2.Effectfeature(this.m_nFeatureEx);
            this.m_nBodyOffset = Actor.Units.Actor.HUMANFRAME * (this.m_btDress);
            this.m_nCboHairOffset = -1;
            if (this.m_btHair >= 10)
            {
                this.m_btHairEx = this.m_btHair / 10;
                this.m_btHair = this.m_btHair % 10;
            }
            else
            {
                this.m_btHairEx = 0;
            }
            if (this.m_btHairEx == 0)
            {
                // 头发兼容模式
                if (this.m_btHair > haircount - 1)
                {
                    this.m_btHair = haircount - 1;
                }
                nHairEx = (this.m_btHair - this.m_btSex) >> 1 + 1;
                haircount = WMFile.Units.WMFile.g_cbohair.ImageCount / 2000 / 2;
                if (nHairEx > haircount)
                {
                    nHairEx = haircount;
                }
                this.m_nCboHairOffset = 2000 * ((nHairEx - 1) * 2 + this.m_btSex);
            }
            else if (this.m_btHairEx > 0)
            {
                if (this.m_btHairEx > haircount)
                {
                    this.m_btHairEx = haircount;
                }
                this.m_nHairOffsetEx = Actor.Units.Actor.HUMANFRAME * ((this.m_btHairEx - 1) * 2 + this.m_btSex);
                // nHairEx := m_btHairEx + m_btSex + (m_btHairEx mod 4);
                nHairEx = (this.m_btHairEx - 1) * 2 + this.m_btSex;
                haircount = WMFile.Units.WMFile.g_cbohair.ImageCount / 2000;
                if (nHairEx > haircount)
                {
                    nHairEx = haircount;
                }
                this.m_nCboHairOffset = 2000 * nHairEx;
            }
            else
            {
                this.m_nHairOffsetEx = -1;
            }
            if (this.m_btHair > haircount - 1)
            {
                this.m_btHair = haircount - 1;
            }
            this.m_btHair = this.m_btHair * 2;
            if (this.m_btHair > 1)
            {
                this.m_nHairOffset = Actor.Units.Actor.HUMANFRAME * (this.m_btHair + this.m_btSex);
            }
            else
            {
                this.m_nHairOffset = -1;
            }
            this.m_nWeaponOffset = Actor.Units.Actor.HUMANFRAME * this.m_btWeapon;
            if ((this.m_btEffect == 50))
            {
                this.m_nHumWinOffset = 352;
            }
            else if (this.m_btEffect != 0)
            {
                this.m_nHumWinOffset = (this.m_btEffect - 1) * Actor.Units.Actor.HUMANFRAME;
            }
            switch (this.m_nCurrentAction)
            {
                case Grobal2.SM_TURN:
                    this.m_nStartFrame = THumAction.HA.ActStand.start + this.m_btDir * (THumAction.HA.ActStand.frame + THumAction.HA.ActStand.skip);
                    this.m_nEndFrame = this.m_nStartFrame + THumAction.HA.ActStand.frame - 1;
                    m_dwFrameTime = THumAction.HA.ActStand.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    this.m_nDefFrameCount = THumAction.HA.ActStand.frame;
                    this.Shift(this.m_btDir, 0, 0, this.m_nEndFrame - this.m_nStartFrame + 1);
                    if (this.m_fHideMode)
                    {
                        this.m_fHideMode = false;
                        this.m_dwSmoothMoveTime = 0;
                        this.m_nCurrentAction = 0;
                    }
                    break;
                case Grobal2.SM_WALK:
                case Grobal2.SM_BACKSTEP:
                    this.m_nStartFrame = THumAction.HA.ActWalk.start + this.m_btDir * (THumAction.HA.ActWalk.frame + THumAction.HA.ActWalk.skip);
                    this.m_nEndFrame = this.m_nStartFrame + THumAction.HA.ActWalk.frame - 1;
                    m_dwFrameTime = THumAction.HA.ActWalk.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    this.m_nMaxTick = THumAction.HA.ActWalk.usetick;
                    this.m_nCurTick = 0;
                    this.m_nMoveStep = 1;
                    if (this.m_nCurrentAction == Grobal2.SM_BACKSTEP)
                    {
                        this.Shift(ClFunc.GetBack(this.m_btDir), this.m_nMoveStep, 0, this.m_nEndFrame - this.m_nStartFrame + 1);
                    }
                    else
                    {
                        this.Shift(this.m_btDir, this.m_nMoveStep, 0, this.m_nEndFrame - this.m_nStartFrame + 1);
                    }
                    break;
                case Grobal2.SM_RUSH:
                    if (this.m_nRushDir == 0)
                    {
                        this.m_nRushDir = 1;
                        this.m_nStartFrame = THumAction.HA.ActRushLeft.start + this.m_btDir * (THumAction.HA.ActRushLeft.frame + THumAction.HA.ActRushLeft.skip);
                        this.m_nEndFrame = this.m_nStartFrame + THumAction.HA.ActRushLeft.frame - 1;
                        m_dwFrameTime = THumAction.HA.ActRushLeft.ftime;
                        this.m_dwStartTime = MShare.GetTickCount();
                        this.m_nMaxTick = THumAction.HA.ActRushLeft.usetick;
                        this.m_nCurTick = 0;
                        this.m_nMoveStep = 1;
                        this.Shift(this.m_btDir, 1, 0, this.m_nEndFrame - this.m_nStartFrame + 1);
                    }
                    else
                    {
                        this.m_nRushDir = 0;
                        this.m_nStartFrame = THumAction.HA.ActRushRight.start + this.m_btDir * (THumAction.HA.ActRushRight.frame + THumAction.HA.ActRushRight.skip);
                        this.m_nEndFrame = this.m_nStartFrame + THumAction.HA.ActRushRight.frame - 1;
                        m_dwFrameTime = THumAction.HA.ActRushRight.ftime;
                        this.m_dwStartTime = MShare.GetTickCount();
                        this.m_nMaxTick = THumAction.HA.ActRushRight.usetick;
                        this.m_nCurTick = 0;
                        this.m_nMoveStep = 1;
                        this.Shift(this.m_btDir, 1, 0, this.m_nEndFrame - this.m_nStartFrame + 1);
                    }
                    break;
                case Grobal2.SM_RUSHEX:
                    CalcActorWinFrame();
                    this.m_nSpellFrame = THumAction.HA.ActRush2.frame;
                    this.m_nStartFrame = THumAction.HA.ActRush2.start + this.m_btDir * (THumAction.HA.ActRush2.frame + THumAction.HA.ActRush2.skip);
                    this.m_nEndFrame = this.m_nStartFrame + THumAction.HA.ActRush2.frame - 1;
                    m_dwFrameTime = THumAction.HA.ActRush2.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    this.m_nMaxTick = THumAction.HA.ActRush2.usetick;
                    this.m_nCurTick = 0;
                    this.m_nMoveStep = this.m_RushStep;
                    this.Shift(this.m_btDir, this.m_nMoveStep, 0, this.m_nEndFrame - this.m_nStartFrame + 1);
                    this.m_boWarMode = true;
                    this.m_dwWarModeTime = MShare.GetTickCount();
                    this.m_boUseCboLib = true;
                    this.m_boHitEffect = true;
                    break;
                case Grobal2.SM_SMITEHIT:
                    CalcActorWinFrame();
                    this.m_nSpellFrame = THumAction.HA.ActSmiteHit.frame;
                    this.m_nStartFrame = THumAction.HA.ActSmiteHit.start + this.m_btDir * (THumAction.HA.ActSmiteHit.frame + THumAction.HA.ActSmiteHit.skip);
                    this.m_nEndFrame = this.m_nStartFrame + THumAction.HA.ActSmiteHit.frame - 1;
                    m_dwFrameTime = THumAction.HA.ActSmiteHit.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    this.m_nMaxTick = THumAction.HA.ActSmiteHit.usetick;
                    this.m_nCurTick = 0;
                    this.Shift(this.m_btDir, 0, 0, 1);
                    this.m_boWarMode = true;
                    this.m_dwWarModeTime = MShare.GetTickCount();
                    this.m_boUseCboLib = true;
                    this.m_boSmiteHit = true;
                    this.m_boHitEffect = true;
                    break;
                case Grobal2.SM_SMITELONGHIT:
                    CalcActorWinFrame();
                    this.m_nSpellFrame = THumAction.HA.ActSmiteLongHit.frame;
                    this.m_nStartFrame = THumAction.HA.ActSmiteLongHit.start + this.m_btDir * (THumAction.HA.ActSmiteLongHit.frame + THumAction.HA.ActSmiteLongHit.skip);
                    this.m_nEndFrame = this.m_nStartFrame + THumAction.HA.ActSmiteLongHit.frame - 1;
                    m_dwFrameTime = THumAction.HA.ActSmiteLongHit.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    this.m_nMaxTick = THumAction.HA.ActSmiteLongHit.usetick;
                    this.m_nCurTick = 0;
                    this.Shift(this.m_btDir, 0, 0, 1);
                    this.m_boWarMode = true;
                    this.m_dwWarModeTime = MShare.GetTickCount();
                    this.m_boUseCboLib = true;
                    this.m_boSmiteLongHit = 1;
                    this.m_boHitEffect = true;
                    break;
                case Grobal2.SM_SMITELONGHIT3:
                    CalcActorWinFrame();
                    this.m_nSpellFrame = THumAction.HA.ActSmiteLongHit3.frame;
                    this.m_nStartFrame = THumAction.HA.ActSmiteLongHit3.start + this.m_btDir * (THumAction.HA.ActSmiteLongHit3.frame + THumAction.HA.ActSmiteLongHit3.skip);
                    this.m_nEndFrame = this.m_nStartFrame + THumAction.HA.ActSmiteLongHit3.frame - 1;
                    m_dwFrameTime = THumAction.HA.ActSmiteLongHit3.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    this.m_nMaxTick = THumAction.HA.ActSmiteLongHit3.usetick;
                    this.m_nCurTick = 0;
                    this.Shift(this.m_btDir, 0, 0, 1);
                    this.m_boWarMode = true;
                    this.m_dwWarModeTime = MShare.GetTickCount();
                    this.m_boUseCboLib = true;
                    this.m_boHitEffect = true;
                    break;
                case Grobal2.SM_SMITELONGHIT2:
                    CalcActorWinFrame();
                    this.m_nSpellFrame = THumAction.HA.ActMagic_113.frame;
                    this.m_nStartFrame = THumAction.HA.ActMagic_113.start + this.m_btDir * (THumAction.HA.ActMagic_113.frame + THumAction.HA.ActMagic_113.skip);
                    this.m_nEndFrame = this.m_nStartFrame + THumAction.HA.ActMagic_113.frame - 1;
                    m_dwFrameTime = THumAction.HA.ActMagic_113.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    this.m_nMaxTick = THumAction.HA.ActMagic_113.usetick;
                    this.m_nCurTick = 0;
                    this.Shift(this.m_btDir, 0, 0, 1);
                    this.m_boWarMode = true;
                    this.m_dwWarModeTime = MShare.GetTickCount();
                    this.m_boUseCboLib = true;
                    this.m_boSmiteLongHit2 = true;
                    this.m_boSmiteLongHitS2 = 2;
                    this.m_boHitEffect = true;
                    break;
                case Grobal2.SM_SMITEWIDEHIT:
                    CalcActorWinFrame();
                    this.m_nSpellFrame = THumAction.HA.ActSmiteWideHit.frame;
                    this.m_nStartFrame = THumAction.HA.ActSmiteWideHit.start + this.m_btDir * (THumAction.HA.ActSmiteWideHit.frame + THumAction.HA.ActSmiteWideHit.skip);
                    this.m_nEndFrame = this.m_nStartFrame + THumAction.HA.ActSmiteWideHit.frame - 1;
                    m_dwFrameTime = THumAction.HA.ActSmiteWideHit.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    this.m_nMaxTick = THumAction.HA.ActSmiteWideHit.usetick;
                    this.m_nCurTick = 0;
                    this.Shift(this.m_btDir, 0, 0, 1);
                    this.m_boWarMode = true;
                    this.m_dwWarModeTime = MShare.GetTickCount();
                    this.m_boUseCboLib = true;
                    this.m_boHitEffect = true;
                    break;
                case Grobal2.SM_SMITEWIDEHIT2:
                    CalcActorWinFrame();
                    this.m_nSpellFrame = THumAction.HA.ActMagic_114.frame;
                    this.m_nStartFrame = THumAction.HA.ActMagic_114.start + this.m_btDir * (THumAction.HA.ActMagic_114.frame + THumAction.HA.ActMagic_114.skip);
                    this.m_nEndFrame = this.m_nStartFrame + THumAction.HA.ActMagic_114.frame - 1;
                    m_dwFrameTime = THumAction.HA.ActMagic_114.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    this.m_nMaxTick = THumAction.HA.ActMagic_114.usetick;
                    this.m_nCurTick = 0;
                    this.Shift(this.m_btDir, 0, 0, 1);
                    this.m_boWarMode = true;
                    this.m_dwWarModeTime = MShare.GetTickCount();
                    this.m_boUseCboLib = true;
                    this.m_boSmiteWideHit2 = true;
                    this.m_boSmiteWideHitS2 = 9;
                    this.m_boHitEffect = true;
                    break;
                case Grobal2.SM_RUSHKUNG:
                    this.m_nStartFrame = THumAction.HA.ActRun.start + this.m_btDir * (THumAction.HA.ActRun.frame + THumAction.HA.ActRun.skip);
                    this.m_nEndFrame = this.m_nStartFrame + THumAction.HA.ActRun.frame - 1;
                    m_dwFrameTime = THumAction.HA.ActRun.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    this.m_nMaxTick = THumAction.HA.ActRun.usetick;
                    this.m_nCurTick = 0;
                    this.m_nMoveStep = 1;
                    this.Shift(this.m_btDir, this.m_nMoveStep, 0, this.m_nEndFrame - this.m_nStartFrame + 1);
                    break;
                case Grobal2.SM_SITDOWN:
                    this.m_nStartFrame = THumAction.HA.ActSitdown.start + this.m_btDir * (THumAction.HA.ActSitdown.frame + THumAction.HA.ActSitdown.skip);
                    this.m_nEndFrame = this.m_nStartFrame + THumAction.HA.ActSitdown.frame - 1;
                    m_dwFrameTime = THumAction.HA.ActSitdown.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    break;
                case Grobal2.SM_RUN:
                    this.m_nStartFrame = THumAction.HA.ActRun.start + this.m_btDir * (THumAction.HA.ActRun.frame + THumAction.HA.ActRun.skip);
                    this.m_nEndFrame = this.m_nStartFrame + THumAction.HA.ActRun.frame - 1;
                    m_dwFrameTime = THumAction.HA.ActRun.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    this.m_nMaxTick = THumAction.HA.ActRun.usetick;
                    this.m_nCurTick = 0;
                    if (this.m_nCurrentAction == Grobal2.SM_RUN)
                    {
                        this.m_nMoveStep = 2;
                    }
                    else
                    {
                        this.m_nMoveStep = 1;
                    }
                    this.Shift(this.m_btDir, this.m_nMoveStep, 0, this.m_nEndFrame - this.m_nStartFrame + 1);
                    break;
                case Grobal2.SM_HORSERUN:
                    this.m_nStartFrame = THumAction.HA.ActRun.start + this.m_btDir * (THumAction.HA.ActRun.frame + THumAction.HA.ActRun.skip);
                    this.m_nEndFrame = this.m_nStartFrame + THumAction.HA.ActRun.frame - 1;
                    m_dwFrameTime = THumAction.HA.ActRun.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    this.m_nMaxTick = THumAction.HA.ActRun.usetick;
                    this.m_nCurTick = 0;
                    if (this.m_nCurrentAction == Grobal2.SM_HORSERUN)
                    {
                        this.m_nMoveStep = 3;
                    }
                    else
                    {
                        this.m_nMoveStep = 1;
                    }
                    this.Shift(this.m_btDir, this.m_nMoveStep, 0, this.m_nEndFrame - this.m_nStartFrame + 1);
                    break;
                case Grobal2.SM_THROW:
                    this.m_nStartFrame = THumAction.HA.ActHit.start + this.m_btDir * (THumAction.HA.ActHit.frame + THumAction.HA.ActHit.skip);
                    this.m_nEndFrame = this.m_nStartFrame + THumAction.HA.ActHit.frame - 1;
                    m_dwFrameTime = THumAction.HA.ActHit.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    this.m_boWarMode = true;
                    this.m_dwWarModeTime = MShare.GetTickCount();
                    this.m_boThrow = true;
                    this.Shift(this.m_btDir, 0, 0, 1);
                    break;
                case Grobal2.SM_HIT:
                case Grobal2.SM_POWERHIT:
                case Grobal2.SM_LONGHIT:
                case Grobal2.SM_WIDEHIT:
                case Grobal2.SM_FIREHIT:
                case Grobal2.SM_HERO_LONGHIT:
                case Grobal2.SM_HERO_LONGHIT2:
                case Grobal2.SM_SQUHIT:
                case Grobal2.SM_CRSHIT:
                case Grobal2.SM_TWNHIT:
                    this.m_nStartFrame = THumAction.HA.ActHit.start + this.m_btDir * (THumAction.HA.ActHit.frame + THumAction.HA.ActHit.skip);
                    this.m_nEndFrame = this.m_nStartFrame + THumAction.HA.ActHit.frame - 1;
                    m_dwFrameTime = THumAction.HA.ActHit.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    this.m_boWarMode = true;
                    this.m_dwWarModeTime = MShare.GetTickCount();
                    if ((this.m_nCurrentAction == Grobal2.SM_POWERHIT))
                    {
                        this.m_boHitEffect = true;
                        this.m_nMagLight = 2;
                        this.m_nHitEffectNumber = 1;
                        switch (this.m_CurMagic.magfirelv / 4)
                        {
                            case 1:
                                this.m_nHitEffectNumber += 101;
                                break;
                            case 2:
                                this.m_nHitEffectNumber += 201;
                                break;
                            case 3:
                                this.m_nHitEffectNumber += 301;
                                break;
                        }
                    }
                    if ((this.m_nCurrentAction == Grobal2.SM_LONGHIT))
                    {
                        this.m_boHitEffect = true;
                        this.m_nMagLight = 2;
                        this.m_nHitEffectNumber = 2;
                        switch (this.m_CurMagic.magfirelv / 4)
                        {
                            case 1:
                                this.m_nHitEffectNumber += 101;
                                break;
                            case 2:
                                this.m_nHitEffectNumber += 201;
                                break;
                            case 3:
                                this.m_nHitEffectNumber += 301;
                                break;
                        }
                    }
                    if ((this.m_nCurrentAction == Grobal2.SM_WIDEHIT))
                    {
                        this.m_boHitEffect = true;
                        this.m_nMagLight = 2;
                        this.m_nHitEffectNumber = 3;
                        switch (this.m_CurMagic.magfirelv / 4)
                        {
                            case 1:
                                this.m_nHitEffectNumber += 101;
                                break;
                            case 2:
                                this.m_nHitEffectNumber += 201;
                                break;
                            case 3:
                                this.m_nHitEffectNumber += 301;
                                break;
                        }
                    }
                    if ((this.m_nCurrentAction == Grobal2.SM_FIREHIT))
                    {
                        this.m_boHitEffect = true;
                        this.m_nMagLight = 2;
                        this.m_nHitEffectNumber = 4;
                        switch (this.m_CurMagic.magfirelv / 4)
                        {
                            case 1:
                                this.m_nHitEffectNumber += 101;
                                break;
                            case 2:
                                this.m_nHitEffectNumber += 201;
                                break;
                            case 3:
                                this.m_nHitEffectNumber += 301;
                                break;
                        }
                    }
                    if ((this.m_nCurrentAction == Grobal2.SM_SQUHIT))
                    {
                        Effect = new THeroCharEffect(WMFile.Units.WMFile.g_WMagic2Images, this.m_btDir * 20 + 740, 15, 50, this);
                        if (Effect != null)
                        {
                            ClMain.g_PlayScene.m_EffectList.Add(Effect);
                        }
                    }
                    if ((this.m_nCurrentAction == Grobal2.SM_CRSHIT))
                    {
                        this.m_boHitEffect = true;
                        this.m_nMagLight = 2;
                        this.m_nHitEffectNumber = 5;
                    }
                    if ((this.m_nCurrentAction == Grobal2.SM_TWNHIT))
                    {
                        // 35
                        Effect = new THeroCharEffect(WMFile.Units.WMFile.g_WMagic2Images, this.m_btDir * 20 + 226, 6, 72, this);
                        if (Effect != null)
                        {
                            ClMain.g_PlayScene.m_EffectList.Add(Effect);
                        }
                    }
                    if ((this.m_nCurrentAction == Grobal2.SM_HERO_LONGHIT2))
                    {
                        this.m_boHitEffect = true;
                        this.m_nMagLight = 2;
                        this.m_nHitEffectNumber = 6;
                        this.m_boHeroLongHit2 = true;
                    }
                    if ((this.m_nCurrentAction == Grobal2.SM_HERO_LONGHIT))
                    {
                        this.m_boHitEffect = true;
                        this.m_nMagLight = 2;
                        this.m_nHitEffectNumber = 8;
                        this.m_boHeroLongHit = true;
                    }
                    this.Shift(this.m_btDir, 0, 0, 1);
                    break;
                case Grobal2.SM_WWJATTACK:
                case Grobal2.SM_WSJATTACK:
                case Grobal2.SM_WTJATTACK:
                    this.m_nStartFrame = THumAction.HA.ActHit.start + this.m_btDir * (THumAction.HA.ActHit.frame + THumAction.HA.ActHit.skip);
                    this.m_nEndFrame = this.m_nStartFrame + THumAction.HA.ActHit.frame - 1;
                    m_dwFrameTime = THumAction.HA.ActHit.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    this.m_boWarMode = true;
                    this.m_dwWarModeTime = MShare.GetTickCount();
                    this.Shift(this.m_btDir, 0, 0, 1);
                    break;
                case Grobal2.SM_HEAVYHIT:
                    this.m_nStartFrame = THumAction.HA.ActHeavyHit.start + this.m_btDir * (THumAction.HA.ActHeavyHit.frame + THumAction.HA.ActHeavyHit.skip);
                    this.m_nEndFrame = this.m_nStartFrame + THumAction.HA.ActHeavyHit.frame - 1;
                    m_dwFrameTime = THumAction.HA.ActHeavyHit.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    this.m_boWarMode = true;
                    this.m_dwWarModeTime = MShare.GetTickCount();
                    this.Shift(this.m_btDir, 0, 0, 1);
                    break;
                case Grobal2.SM_BIGHIT:
                case Grobal2.SM_PURSUEHIT:
                    this.m_nStartFrame = THumAction.HA.ActBigHit.start + this.m_btDir * (THumAction.HA.ActBigHit.frame + THumAction.HA.ActBigHit.skip);
                    this.m_nEndFrame = this.m_nStartFrame + THumAction.HA.ActBigHit.frame - 1;
                    m_dwFrameTime = THumAction.HA.ActBigHit.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    this.m_boWarMode = true;
                    this.m_dwWarModeTime = MShare.GetTickCount();
                    this.Shift(this.m_btDir, 0, 0, 1);
                    break;
                case Grobal2.SM_SPELL:
                    if (this.m_CurMagic.EffectNumber >= 104 && this.m_CurMagic.EffectNumber <= 114)
                    {
                        // DScreen.AddChatBoardString(format('EffectNumber=%d m_nEndFrame=%d', [m_CurMagic.EffectNumber, 1]), clWhite, clRed);
                        CalcActorWinFrame();
                        switch (this.m_CurMagic.EffectNumber)
                        {
                            case 104:
                                this.m_nSpellFrame = THumAction.HA.ActMagic_104.frame;
                                this.m_nStartFrame = THumAction.HA.ActMagic_104.start + this.m_btDir * (THumAction.HA.ActMagic_104.frame + THumAction.HA.ActMagic_104.skip);
                                this.m_nEndFrame = this.m_nStartFrame + THumAction.HA.ActMagic_104.frame - 1;
                                m_dwFrameTime = THumAction.HA.ActMagic_104.ftime;
                                this.m_dwStartTime = MShare.GetTickCount();
                                m_boSSkill = true;
                                break;
                            case 105:
                                this.m_nSpellFrame = THumAction.HA.ActMagic_105.frame;
                                this.m_nStartFrame = THumAction.HA.ActMagic_105.start + this.m_btDir * (THumAction.HA.ActMagic_105.frame + THumAction.HA.ActMagic_105.skip);
                                this.m_nEndFrame = this.m_nStartFrame + THumAction.HA.ActMagic_105.frame - 1;
                                m_dwFrameTime = THumAction.HA.ActMagic_105.ftime;
                                this.m_dwStartTime = MShare.GetTickCount();
                                m_boSSkill = true;
                                this.m_boNewMagic = true;
                                break;
                            case 106:
                                this.m_nSpellFrame = THumAction.HA.ActMagic_106.frame;
                                this.m_nStartFrame = THumAction.HA.ActMagic_106.start + this.m_btDir * (THumAction.HA.ActMagic_106.frame + THumAction.HA.ActMagic_106.skip);
                                this.m_nEndFrame = this.m_nStartFrame + THumAction.HA.ActMagic_106.frame - 1;
                                m_dwFrameTime = THumAction.HA.ActMagic_106.ftime;
                                this.m_dwStartTime = MShare.GetTickCount();
                                m_boSSkill = true;
                                this.m_boNewMagic = true;
                                break;
                            case 107:
                                this.m_nSpellFrame = THumAction.HA.ActMagic_107.frame;
                                this.m_nStartFrame = THumAction.HA.ActMagic_107.start + this.m_btDir * (THumAction.HA.ActMagic_107.frame + THumAction.HA.ActMagic_107.skip);
                                this.m_nEndFrame = this.m_nStartFrame + THumAction.HA.ActMagic_107.frame - 1;
                                m_dwFrameTime = THumAction.HA.ActMagic_107.ftime;
                                this.m_dwStartTime = MShare.GetTickCount();
                                m_boSSkill = true;
                                this.m_boNewMagic = true;
                                break;
                            case 108:
                                this.m_nSpellFrame = THumAction.HA.ActMagic_108.frame;
                                this.m_nStartFrame = THumAction.HA.ActMagic_108.start + this.m_btDir * (THumAction.HA.ActMagic_108.frame + THumAction.HA.ActMagic_108.skip);
                                this.m_nEndFrame = this.m_nStartFrame + THumAction.HA.ActMagic_108.frame - 1;
                                m_dwFrameTime = THumAction.HA.ActMagic_108.ftime;
                                this.m_dwStartTime = MShare.GetTickCount();
                                m_boSSkill = true;
                                break;
                            case 109:
                                this.m_nSpellFrame = THumAction.HA.ActMagic_109.frame;
                                this.m_nStartFrame = THumAction.HA.ActMagic_109.start + this.m_btDir * (THumAction.HA.ActMagic_109.frame + THumAction.HA.ActMagic_109.skip);
                                this.m_nEndFrame = this.m_nStartFrame + THumAction.HA.ActMagic_109.frame - 1;
                                m_dwFrameTime = THumAction.HA.ActMagic_109.ftime;
                                this.m_dwStartTime = MShare.GetTickCount();
                                m_boSSkill = true;
                                this.m_boNewMagic = true;
                                break;
                            case 110:
                                this.m_nSpellFrame = THumAction.HA.ActMagic_110.frame;
                                this.m_nStartFrame = THumAction.HA.ActMagic_110.start + this.m_btDir * (THumAction.HA.ActMagic_110.frame + THumAction.HA.ActMagic_110.skip);
                                this.m_nEndFrame = this.m_nStartFrame + THumAction.HA.ActMagic_110.frame - 1;
                                m_dwFrameTime = THumAction.HA.ActMagic_110.ftime;
                                this.m_dwStartTime = MShare.GetTickCount();
                                m_boSSkill = true;
                                this.m_boNewMagic = true;
                                break;
                            case 111:
                                this.m_nSpellFrame = THumAction.HA.ActMagic_111.frame;
                                this.m_nStartFrame = THumAction.HA.ActMagic_111.start + this.m_btDir * (THumAction.HA.ActMagic_111.frame + THumAction.HA.ActMagic_111.skip);
                                this.m_nEndFrame = this.m_nStartFrame + THumAction.HA.ActMagic_111.frame - 1;
                                m_dwFrameTime = THumAction.HA.ActMagic_111.ftime;
                                this.m_dwStartTime = MShare.GetTickCount();
                                this.m_boNewMagic = true;
                                break;
                            case 112:
                                this.m_nSpellFrame = THumAction.HA.ActMagic_112.frame;
                                this.m_nStartFrame = THumAction.HA.ActMagic_112.start + this.m_btDir * (THumAction.HA.ActMagic_112.frame + THumAction.HA.ActMagic_112.skip);
                                this.m_nEndFrame = this.m_nStartFrame + THumAction.HA.ActMagic_112.frame - 1;
                                m_dwFrameTime = THumAction.HA.ActMagic_112.ftime;
                                this.m_dwStartTime = MShare.GetTickCount();
                                m_boSSkill = true;
                                break;
                            case 113:
                                this.m_nSpellFrame = THumAction.HA.ActMagic_113.frame;
                                this.m_nStartFrame = THumAction.HA.ActMagic_113.start + this.m_btDir * (THumAction.HA.ActMagic_113.frame + THumAction.HA.ActMagic_113.skip);
                                this.m_nEndFrame = this.m_nStartFrame + THumAction.HA.ActMagic_113.frame - 1;
                                m_dwFrameTime = THumAction.HA.ActMagic_113.ftime;
                                this.m_dwStartTime = MShare.GetTickCount();
                                m_boSSkill = true;
                                break;
                            case 114:
                                this.m_nSpellFrame = THumAction.HA.ActMagic_114.frame;
                                this.m_nStartFrame = THumAction.HA.ActMagic_114.start + this.m_btDir * (THumAction.HA.ActMagic_114.frame + THumAction.HA.ActMagic_114.skip);
                                this.m_nEndFrame = this.m_nStartFrame + THumAction.HA.ActMagic_114.frame - 1;
                                m_dwFrameTime = THumAction.HA.ActMagic_114.ftime;
                                this.m_dwStartTime = MShare.GetTickCount();
                                m_boSSkill = true;
                                break;
                        }
                        this.m_nCurEffFrame = 0;
                        this.m_boUseMagic = true;
                        this.m_boUseCboLib = true;
                    }
                    else
                    {
                        this.m_nStartFrame = THumAction.HA.ActSpell.start + this.m_btDir * (THumAction.HA.ActSpell.frame + THumAction.HA.ActSpell.skip);
                        this.m_nEndFrame = this.m_nStartFrame + THumAction.HA.ActSpell.frame - 1;
                        m_dwFrameTime = THumAction.HA.ActSpell.ftime;
                        this.m_dwStartTime = MShare.GetTickCount();
                        this.m_nCurEffFrame = 0;
                        this.m_boUseMagic = true;
                        // DScreen.AddChatBoardString(format('EffectNumber=%d m_nEndFrame=%d', [m_CurMagic.EffectNumber, 1]), clWhite, clRed);
                        this.m_nSpellFrame = Actor.Units.Actor.DEFSPELLFRAME;
                        switch (this.m_CurMagic.EffectNumber)
                        {
                            case 10:
                                // 灵魂火符
                                this.m_nMagLight = 2;
                                if (this.m_CurMagic.spelllv > MShare.MAXMAGICLV)
                                {
                                    this.m_nSpellFrame = 10;
                                }
                                break;
                            case 15:
                                if (this.m_CurMagic.spelllv > 3)
                                {
                                    this.m_nMagLight = 2;
                                    this.m_nSpellFrame = 10;
                                }
                                break;
                            case 22:// 地狱雷光
                                this.m_nMagLight = 4;
                                this.m_nSpellFrame = 10;
                                break;
                            case 26:// 心灵启示
                                this.m_nMagLight = 2;
                                break;
                            case 34:// 灭天火
                                this.m_nMagLight = 2;
                                if (this.m_CurMagic.spelllv > MShare.MAXMAGICLV)
                                {
                                    this.m_nSpellFrame = 10;
                                }
                                break;
                            case 35:// 无极真气
                                this.m_nMagLight = 2;
                                break;
                            case 43:// 狮子吼
                                this.m_nMagLight = 3;
                                break;
                            case 121:
                                this.m_nMagLight = 3;
                                this.m_nSpellFrame = 10;
                                break;
                            case 120:
                                this.m_nMagLight = 3;
                                this.m_nSpellFrame = 12;
                                break;
                            case 122:
                                this.m_nMagLight = 3;
                                this.m_nSpellFrame = 8;
                                break;
                            case 116:
                            case 117:
                                // 狮子吼
                                this.m_nMagLight = 3;
                                this.m_nSpellFrame = 10;
                                break;
                            case 124:
                                this.m_nMagLight = 4;
                                // FSpellFrame := 40;
                                break;
                            case 127:
                                this.m_nMagLight = 3;
                                break;
                            default:
                                this.m_nMagLight = 2;
                                this.m_nSpellFrame = Actor.Units.Actor.DEFSPELLFRAME;
                                break;
                        }
                    }
                    this.m_dwWaitMagicRequest = MShare.GetTickCount();
                    this.m_boWarMode = true;
                    this.m_dwWarModeTime = MShare.GetTickCount();
                    this.Shift(this.m_btDir, 0, 0, 1);
                    break;
                case Grobal2.SM_STRUCK:
                    this.m_nStartFrame = THumAction.HA.ActStruck.start + this.m_btDir * (THumAction.HA.ActStruck.frame + THumAction.HA.ActStruck.skip);
                    this.m_nEndFrame = this.m_nStartFrame + THumAction.HA.ActStruck.frame - 1;
                    m_dwFrameTime = this.m_dwStruckFrameTime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    this.Shift(this.m_btDir, 0, 0, 1);
                    this.m_dwGenAnicountTime = MShare.GetTickCount();
                    m_nCurBubbleStruck = 0;
                    break;
                case Grobal2.SM_NOWDEATH:
                    this.m_nStartFrame = THumAction.HA.ActDie.start + this.m_btDir * (THumAction.HA.ActDie.frame + THumAction.HA.ActDie.skip);
                    this.m_nEndFrame = this.m_nStartFrame + THumAction.HA.ActDie.frame - 1;
                    m_dwFrameTime = THumAction.HA.ActDie.ftime;
                    this.m_dwStartTime = MShare.GetTickCount();
                    break;
            }
        }

        public override void DefaultMotion()
        {
            int ndir;
            int MaxIdx;
            int frame;
            base.DefaultMotion();
            if (this.m_boUseCboLib)
            {
                if ((this.m_btEffect == 50))
                {
                    if ((this.m_nCurrentFrame <= 536))
                    {
                        if ((MShare.GetTickCount() - m_dwFrameTick) > 100)
                        {
                            if (m_nFrame < 19)
                            {
                                m_nFrame++;
                            }
                            else
                            {
                                m_nFrame = 0;
                            }
                            m_dwFrameTick = MShare.GetTickCount();
                        }
                    }
                }
                else if ((this.m_btEffect != 0))
                {
                    MaxIdx = 0;
                    ndir = 0;
                    frame = 0;
                    switch (this.m_nCurrentAction)
                    {
                        case Grobal2.SM_RUSHEX:
                            frame = 8;
                            ndir = 10;
                            MaxIdx = 80;
                            break;
                        case Grobal2.SM_SMITEHIT:
                            frame = 15;
                            ndir = 20;
                            MaxIdx = 160;
                            break;
                        case Grobal2.SM_SMITELONGHIT:
                            if (this.m_boSmiteLongHit == 2)
                            {
                                frame = 6;
                                ndir = 10;
                                MaxIdx = 320;
                            }
                            break;
                        case Grobal2.SM_SMITELONGHIT3:
                            frame = 6;
                            ndir = 10;
                            MaxIdx = 320;
                            break;
                        case Grobal2.SM_SMITELONGHIT2:
                        case Grobal2.SM_SMITEWIDEHIT2:
                            frame = 12;
                            ndir = 20;
                            MaxIdx = 400;
                            break;
                        case Grobal2.SM_SMITEWIDEHIT:
                            frame = 10;
                            ndir = 10;
                            MaxIdx = 560;
                            break;
                        case Grobal2.SM_SPELL:
                            switch (this.m_CurMagic.EffectNumber)
                            {
                                case 104:
                                    frame = 6;
                                    ndir = 10;
                                    MaxIdx = 640;
                                    break;
                                case 112:
                                    frame = 6;
                                    ndir = 10;
                                    MaxIdx = 720;
                                    break;
                                case 106:
                                    frame = 8;
                                    ndir = 10;
                                    MaxIdx = 800;
                                    break;
                                case 107:
                                    frame = 13;
                                    ndir = 10;
                                    MaxIdx = 1040;
                                    break;
                                case 108:
                                    frame = 6;
                                    ndir = 10;
                                    MaxIdx = 1200;
                                    break;
                                case 109:
                                    frame = 12;
                                    ndir = 20;
                                    MaxIdx = 1440;
                                    break;
                                case 110:
                                    frame = 12;
                                    ndir = 20;
                                    MaxIdx = 1600;
                                    break;
                                case 111:
                                    frame = 14;
                                    ndir = 20;
                                    MaxIdx = 1760;
                                    break;
                                case 105:
                                    // 112
                                    frame = 10;
                                    ndir = 10;
                                    MaxIdx = 880;
                                    break;
                            }
                            break;
                    }
                    // dscreen.AddChatBoardString(inttostr(m_nCboHumWinOffSet), clBlue, clWhite);
                    if (this.m_nCurrentFrame < MaxIdx)
                    {
                        if ((MShare.GetTickCount() - m_dwFrameTick) > MShare.HUMWINEFFECTTICK)
                        {
                            if (m_nFrame < (frame - 1))
                            {
                                m_nFrame++;
                            }
                            else
                            {
                                m_nFrame = 0;
                            }
                            m_dwFrameTick = MShare.GetTickCount();
                        }
                    }
                }
                else
                {
                    if ((this.m_btEffect == 50))
                    {
                        if ((this.m_nCurrentFrame <= 536))
                        {
                            if ((MShare.GetTickCount() - m_dwFrameTick) > 100)
                            {
                                if (m_nFrame < 19)
                                {
                                    m_nFrame++;
                                }
                                else
                                {
                                    m_nFrame = 0;
                                }
                                m_dwFrameTick = MShare.GetTickCount();
                            }
                        }
                    }
                    else if ((this.m_btEffect != 0))
                    {
                        if (this.m_nCurrentFrame < 64)
                        {
                            if ((MShare.GetTickCount() - m_dwFrameTick) > MShare.HUMWINEFFECTTICK)
                            {
                                // Blue
                                if (m_nFrame < 7)
                                {
                                    m_nFrame++;
                                }
                                else
                                {
                                    m_nFrame = 0;
                                }
                                m_dwFrameTick = MShare.GetTickCount();
                            }
                        }
                    }
                }
            }
        }

        public override int GetDefaultFrame(bool wmode)
        {
            int result;
            int cf;
            if (this.m_boDeath)
            {
                result = THumAction.HA.ActDie.start + this.m_btDir * (THumAction.HA.ActDie.frame + THumAction.HA.ActDie.skip) + (THumAction.HA.ActDie.frame - 1);
            }
            else if (wmode)
            {
                result = THumAction.HA.ActWarMode.start + this.m_btDir * (THumAction.HA.ActWarMode.frame + THumAction.HA.ActWarMode.skip);
            }
            else
            {
                this.m_nDefFrameCount = THumAction.HA.ActStand.frame;
                if (this.m_nCurrentDefFrame < 0)
                {
                    cf = 0;
                }
                else if (this.m_nCurrentDefFrame >= THumAction.HA.ActStand.frame)
                {
                    cf = 0;
                }
                else
                {
                    cf = this.m_nCurrentDefFrame;
                }
                result = THumAction.HA.ActStand.start + this.m_btDir * (THumAction.HA.ActStand.frame + THumAction.HA.ActStand.skip) + cf;
            }
            return result;
        }

        public override void RunFrameAction(int frame)
        {
            TNormalDrawEffect neff;
            TMapEffect meff;
            TClEvent __event;
            TFlyingAxe mfly;
            TMagicEff HeroCharEffect;
            m_boHideWeapon = false;
            if (m_boSSkill)
            {
                if ((frame == 1))
                {
                    m_boSSkill = false;
                }
            }
            else if (this.m_nCurrentAction == Grobal2.SM_SMITELONGHIT)
            {
                if ((frame == 4) && (this.m_boSmiteLongHit == 2))
                {
                    this.m_boSmiteLongHit = 0;
                }
            }
            else if (this.m_nCurrentAction == Grobal2.SM_SMITELONGHIT3)
            {
 
            }
            else if (this.m_nCurrentAction == Grobal2.SM_SMITEWIDEHIT2)
            {
                if ((frame == 8) && (this.m_boSmiteWideHitS2 > 0))
                {
                    this.m_boSmiteWideHitS2 -= 1;
                }
                if ((frame == 10) && this.m_boSmiteWideHit2)
                {
                    this.m_boSmiteWideHit2 = false;
                }
            }
            else if (this.m_nCurrentAction == Grobal2.SM_SMITELONGHIT2)
            {
                if ((frame == 8) && (this.m_boSmiteLongHitS2 > 0))
                {
                    this.m_boSmiteLongHitS2 -= 1;
                }
                if ((frame == 11) && this.m_boSmiteLongHit2)
                {
                    this.m_boSmiteLongHit2 = false;
                }
            }
            else if (this.m_nCurrentAction == Grobal2.SM_SMITEHIT)
            {
                if ((frame == 14) && (this.m_boSmiteHit))
                {
                    this.m_boSmiteHit = false;
                }
            }
            else if (this.m_nCurrentAction == Grobal2.SM_HERO_LONGHIT2)
            {
                if ((frame == 4) && (this.m_boHeroLongHit2))
                {
                    this.m_boHeroLongHit2 = false;
                }
            }
            else if (this.m_nCurrentAction == Grobal2.SM_HERO_LONGHIT)
            {
                if ((frame == 4) && (this.m_boHeroLongHit))
                {
                    this.m_boHeroLongHit = false;
                }
            }
            else if (this.m_nCurrentAction == Grobal2.SM_HEAVYHIT)
            {
                if ((frame == 5) && (this.m_boDigFragment))
                {
                    this.m_boDigFragment = false;
                    __event = ClMain.EventMan.GetEvent(this.m_nCurrX, this.m_nCurrY, Grobal2.ET_PILESTONES);
                    if (__event != null)
                    {
                        __event.m_nEventParam = __event.m_nEventParam + 1;
                    }
                }
            }
            else if (this.m_nCurrentAction == Grobal2.SM_SITDOWN)
            {
                if ((frame == 5) && (this.m_boDigFragment))
                {
                    this.m_boDigFragment = false;
                    __event = ClMain.EventMan.GetEvent(this.m_nCurrX, this.m_nCurrY, Grobal2.ET_PILESTONES);
                    if (__event != null)
                    {
                        __event.m_nEventParam = __event.m_nEventParam + 1;
                    }
                }
            }
            else if (this.m_nCurrentAction == Grobal2.SM_THROW)
            {
                if ((frame == 3) && (this.m_boThrow))
                {
                    this.m_boThrow = false;
                    mfly = ((TFlyingAxe)(ClMain.g_PlayScene.NewFlyObject(this, this.m_nCurrX, this.m_nCurrY, this.m_nTargetX, this.m_nTargetY, this.m_nTargetRecog, magiceff.TMagicType.mtFlyAxe)));
                    if (mfly != null)
                    {
                        ((TFlyingAxe)(mfly)).ReadyFrame = 40;
                        mfly.ImgLib = WMFile.Units.WMFile.g_WMons[3];
                        mfly.FlyImageBase = magiceff.Units.magiceff.FLYOMAAXEBASE;
                    }
                }
                if (frame >= 3)
                {
                    m_boHideWeapon = true;
                }
            }
        }

        public void DoWeaponBreakEffect()
        {
            m_boWeaponEffect = true;
            m_nCurWeaponEffect = 0;
        }

        public bool Run_MagicTimeOut()
        {
            bool result;
            if ((this == MShare.g_MySelf))
            {
                result = MShare.GetTickCount() - this.m_dwWaitMagicRequest > 1800;
            }
            else
            {
                result = MShare.GetTickCount() - this.m_dwWaitMagicRequest > 850 + (byte)bss * 50;
            }
            if (!bss && result)
            {
                this.m_CurMagic.ServerMagicCode = 0;
            }
            return result;
        }

        public override void Run()
        {
            int off;
            int prv;
            double dwFrameTimetime;
            bool boFly;
            bool bss;
            bool sskill;
            bool fAddNewMagic;
            if (MShare.GetTickCount() - this.m_dwGenAnicountTime > 120)
            {
                this.m_dwGenAnicountTime = MShare.GetTickCount();
                this.m_nGenAniCount++;
                if (this.m_nGenAniCount > 100000)
                {
                    this.m_nGenAniCount = 0;
                }
                m_nCurBubbleStruck++;
            }
            if (m_boWeaponEffect)
            {
                if (MShare.GetTickCount() - m_dwWeaponpEffectTime > 120)
                {
                    m_dwWeaponpEffectTime = MShare.GetTickCount();
                    m_nCurWeaponEffect++;
                    if (m_nCurWeaponEffect >= Actor.Units.Actor.MAXWPEFFECTFRAME)
                    {
                        m_boWeaponEffect = false;
                    }
                }
            }
            if ((this.m_nCurrentAction == Grobal2.SM_RUSHEX))
            {
                this.RunActSound(this.m_nCurrentFrame - this.m_nStartFrame);
                return;
            }
            if ((new ArrayList(new int[] { 5, 9, 11, 13, 39 }).Contains(this.m_nCurrentAction)))
            {
                return;
            }
            this.m_boMsgMuch = (this != MShare.g_MySelf) && (this.m_MsgList.Count >= 2);
            bss = new ArrayList(new int[] { 105, 109 }).Contains(this.m_CurMagic.EffectNumber);
            off = this.m_nCurrentFrame - this.m_nStartFrame;
            this.RunActSound(off);
            RunFrameAction(off);
            prv = this.m_nCurrentFrame;
            if (this.m_nCurrentAction != 0)
            {
                // m_nCurrentAction = SM_ALIVE
                if ((this.m_nCurrentFrame < this.m_nStartFrame) || (this.m_nCurrentFrame > this.m_nEndFrame))
                {
                    this.m_nCurrentFrame = this.m_nStartFrame;
                }
                if (this.m_boMsgMuch)
                {
                    if (this.m_boUseCboLib)
                    {
                        if (this.m_btIsHero == 1)
                        {
                            dwFrameTimetime = Math.Round(m_dwFrameTime / 1.50);
                        }
                        else
                        {
                            dwFrameTimetime = Math.Round(m_dwFrameTime / 1.55);
                        }
                    }
                    else
                    {
                        dwFrameTimetime = Math.Round(m_dwFrameTime / 1.7);
                    }
                }
                else if ((this != MShare.g_MySelf) && (this.m_boUseMagic))
                {
                    if (this.m_boUseCboLib)
                    {
                        if (this.m_btIsHero == 1)
                        {
                            dwFrameTimetime = Math.Round(m_dwFrameTime / 1.28);
                        }
                        else
                        {
                            dwFrameTimetime = Math.Round(m_dwFrameTime / 1.32);
                        }
                    }
                    else
                    {
                        dwFrameTimetime = Math.Round(m_dwFrameTime / 1.38);
                    }
                }
                else
                {
                    dwFrameTimetime = m_dwFrameTime;
                }
                if (MShare.g_boSpeedRate)
                {
                    dwFrameTimetime = HUtil32._MAX(0, dwFrameTimetime - ((long)HUtil32._MIN(10, MShare.g_MoveSpeedRate)));
                }
                if (MShare.GetTickCount() - this.m_dwStartTime > dwFrameTimetime)
                {
                    if (this.m_nCurrentFrame < this.m_nEndFrame)
                    {
                        if (this.m_boUseMagic)
                        {
                            // 魔法效果...
                            if ((this.m_nCurEffFrame == this.m_nSpellFrame - 2) || Run_MagicTimeOut())
                            {
                                if ((this.m_CurMagic.ServerMagicCode >= 0) || Run_MagicTimeOut())
                                {
                                    this.m_nCurrentFrame++;
                                    this.m_nCurEffFrame++;
                                    this.m_dwStartTime = MShare.GetTickCount();
                                }
                            }
                            else
                            {
                                if (this.m_nCurrentFrame < this.m_nEndFrame - 1)
                                {
                                    this.m_nCurrentFrame++;
                                }
                                this.m_nCurEffFrame++;
                                this.m_dwStartTime = MShare.GetTickCount();
                            }
                        }
                        else
                        {
                            this.m_nCurrentFrame++;
                            this.m_dwStartTime = MShare.GetTickCount();
                        }
                        ReadyNextAction();
                    }
                    else
                    {
                        if (this == MShare.g_MySelf)
                        {
                            if (ClMain.frmMain.ServerAcceptNextAction())
                            {
                                ActionEnded();
                                this.m_nCurrentAction = 0;
                                this.m_boUseMagic = false;
                                this.m_boUseCboLib = false;
                            }
                        }
                        else
                        {
                            ActionEnded();
                            this.m_nCurrentAction = 0;
                            this.m_boUseMagic = false;
                            this.m_boUseCboLib = false;
                        }
                        this.m_boHitEffect = false;
                        if (this.m_boSmiteLongHit == 1)
                        {
                            this.m_boSmiteLongHit = 2;
                            CalcActorWinFrame();
                            this.m_nStartFrame = THumAction.HA.ActSmiteLongHit2.start + this.m_btDir * (THumAction.HA.ActSmiteLongHit2.frame + THumAction.HA.ActSmiteLongHit2.skip);
                            this.m_nEndFrame = this.m_nStartFrame + THumAction.HA.ActSmiteLongHit2.frame - 1;
                            m_dwFrameTime = THumAction.HA.ActSmiteLongHit2.ftime;
                            this.m_dwStartTime = MShare.GetTickCount();
                            this.m_nMaxTick = THumAction.HA.ActSmiteLongHit2.usetick;
                            this.m_nCurTick = 0;
                            this.Shift(this.m_btDir, 0, 0, 1);
                            this.m_boWarMode = true;
                            this.m_dwWarModeTime = MShare.GetTickCount();
                            this.m_boUseCboLib = true;
                            this.m_boHitEffect = true;
                            this.m_nCurrentAction = Grobal2.SM_SMITELONGHIT;
                        }
                    }
                    if (this.m_boUseMagic)
                    {
                        // bss := m_CurMagic.EffectNumber in [105..107, 109..111];
                        if (bss && (this.m_CurMagic.ServerMagicCode > 0))
                        {
                            // i := m_nCurrentFrame - m_nStartFrame;
                            sskill = false;
                            switch (this.m_CurMagic.EffectNumber)
                            {
                                case 105:
                                case 106:
                                    sskill = this.m_nCurEffFrame == 7;
                                    break;
                                case 107:
                                case 109:
                                    sskill = this.m_nCurEffFrame == 9;
                                    break;
                                case 110:
                                    sskill = new ArrayList(new int[] { 6, 8, 10 }).Contains(this.m_nCurEffFrame);
                                    break;
                                case 111:
                                    sskill = this.m_nCurEffFrame == 10;
                                    break;
                                    // 116, 117: sskill := m_nCurEffFrame = 6;
                            }
                            if (sskill)
                            {
                                TUseMagicInfo _wvar1 = this.m_CurMagic;
                                ClMain.g_PlayScene.NewMagic(this, _wvar1.ServerMagicCode, _wvar1.EffectNumber, this.m_nCurrX, this.m_nCurrY, _wvar1.targx, _wvar1.targy, _wvar1.target, _wvar1.EffectType, _wvar1.Recusion, _wvar1.anitime, ref boFly, _wvar1.magfirelv, _wvar1.Poison);
                                this.m_boNewMagic = false;
                            }
                        }
                        switch (this.m_CurMagic.EffectNumber)
                        {
                            case 127:
                                fAddNewMagic = this.m_nCurEffFrame == 7;
                                break;
                            default:
                                fAddNewMagic = this.m_nCurEffFrame == this.m_nSpellFrame - 1;
                                break;
                        }
                        if (fAddNewMagic)
                        {
                            if ((this.m_CurMagic.ServerMagicCode > 0) && (!bss || this.m_boNewMagic))
                            {
                                TUseMagicInfo _wvar2 = this.m_CurMagic;
                                ClMain.g_PlayScene.NewMagic(this, _wvar2.ServerMagicCode, _wvar2.EffectNumber, this.m_nCurrX, this.m_nCurrY, _wvar2.targx, _wvar2.targy, _wvar2.target, _wvar2.EffectType, _wvar2.Recusion, _wvar2.anitime, ref boFly, _wvar2.magfirelv, _wvar2.Poison);
                                if (boFly)
                                {
                                    //SoundUtil.g_SndMgr.PlaySound(this.m_nMagicFireSound, this.m_nCurrX, this.m_nCurrY);
                                }
                                else
                                {
                                    // DScreen.AddChatBoardString(inttostr(m_nMagicExplosionSound), GetRGB(219), clWhite);
                                    if (this.m_CurMagic.EffectNumber != 116)
                                    {
                                        //SoundUtil.g_SndMgr.PlaySound(this.m_nMagicExplosionSound, _wvar2.targx, _wvar2.targy);
                                    }
                                }
                            }
                            if (this == MShare.g_MySelf)
                            {
                                MShare.g_dwLatestSpellTick = MShare.GetTickCount();
                            }
                            this.m_CurMagic.ServerMagicCode = 0;
                        }
                    }
                }
                if (this.m_btRace == 0)
                {
                    this.m_nCurrentDefFrame = 0;
                }
                else
                {
                    this.m_nCurrentDefFrame = -10;
                }
                this.m_dwDefFrameTime = MShare.GetTickCount();
            }
            else if (((int)MShare.GetTickCount() - this.m_dwSmoothMoveTime) > 200)
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
                DefaultMotion();
            }
            if (prv != this.m_nCurrentFrame)
            {
                this.m_dwLoadSurfaceTime = MShare.GetTickCount();
                LoadSurface();
            }
        }

        public override int light()
        {
            int result;
            int L;
            L = this.m_nChrLight;
            if (L < this.m_nMagLight)
            {
                if (this.m_boUseMagic || this.m_boHitEffect)
                {
                    L = this.m_nMagLight;
                }
            }
            result = L;
            return result;
        }
    }
}