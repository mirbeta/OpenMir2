using BotSrv.Player;
using System.Collections;
using System.Collections.Generic;
using OpenMir2;
using OpenMir2.Consts;
using OpenMir2.Enums;
using OpenMir2.Packets.ClientPackets;
using SystemModule;
using SystemModule.Packets.ClientPackets;

namespace BotSrv.Objects
{
    public class Actor
    {
        public RobotPlayer robotClient;
        public string UserName = string.Empty;
        public byte NameColor;
        public int m_nNameColor;
        public byte m_btDir;
        public byte m_btDress = 0; // 衣服类型
        public byte m_btEffect = 0;
        public byte m_btHair = 0;
        public byte m_btHairEx = 0;
        public byte m_btHorse = 0;
        public byte m_btIsHero;
        public byte Job = 0;
        public byte m_btPoisonDecHealth;
        public byte Race = ActorRace.Play;
        public byte m_btSex = 0;
        public byte m_btTitleIndex;
        public byte m_btWeapon = 0; // 武器类型
        public byte m_btWeaponEffect; // 武器特效类型
        /// <summary>
        /// 基础属性
        /// </summary>
        public Ability Abil;
        public TMonsterAction m_Action;
        public bool m_boAttackSlow; // 腕力不够时慢动作攻击.
        public bool Death;
        public bool m_boDelActionAfterFinished;
        public bool DelActor;
        public bool m_boDigFragment = false;
        public bool m_boFisrShopItem;
        public bool m_boGrouped;
        public bool m_boHeroLongHit = false;
        public bool m_boHeroLongHit2 = false;
        public bool m_boHitEffect;
        public bool m_boHoldPlace;
        public bool m_boItemExplore;
        public bool m_boLockEndFrame;
        public bool m_boMoveSlow; // 负重不够时慢动作跑
        protected bool m_boMsgMuch;
        public bool m_boNewMagic;
        public bool OpenHealth;
        public bool m_boReverseFrame;
        public bool m_boRunSound = false;
        public bool m_boSkeleton;
        public bool m_boSmiteHit = false;
        public byte m_boSmiteLongHit;
        public bool m_boSmiteLongHit2;
        public byte m_boSmiteLongHitS2;
        public bool m_boSmiteWideHit2;
        public byte m_boSmiteWideHitS2;
        public bool m_boThrow = false;
        public bool m_boUseCboLib;
        public bool m_boUseEffect;
        public bool m_boUseMagic;
        public bool Visible;
        public bool m_boWarMode;
        public bool m_btAFilter;
        public byte m_btAttribute;
        public byte m_btDeathState = 0;
        public TChrMsg m_ChrMsg;
        public TUseMagicInfo m_CurMagic = new TUseMagicInfo();
        public long m_dwAutoTecHeroTick = 0;
        public long m_dwAutoTecTick = 0;
        protected int m_dwDefFrameTime;
        public long m_dwDeleteTime = 0;
        protected int m_dwEffectFrameTime = 0;
        protected int m_dwEffectStartTime = 0;
        public long m_dwFocusFrameTick = 0;
        protected int m_dwFrameTime;
        protected int m_dwGenAnicountTime;
        public long m_dwHealthBK = 0;
        public long m_dwHealthHP = 0;
        public long m_dwHealthMP = 0;
        public long m_dwHealthSP = 0;
        public long m_dwLastStruckTime;
        public long m_dwLoadSurfaceTime;
        public long m_dwMsgHint = 0;
        public long m_dwOpenHealthStart = 0;
        public long m_dwOpenHealthTime = 0;
        public long m_dwPracticeTick = 0;
        public long m_dwSayTime = 0;
        public long m_dwSendQueryUserNameTime;
        protected int m_dwSmoothMoveTime;
        protected int m_dwStartTime;
        protected int m_dwStruckFrameTime;
        public long m_dwWaitMagicRequest = 0;
        public long m_dwWarModeTime;
        public bool m_fHideMode;
        public IList<TChrMsg> m_MsgList;
        public short m_nActBeforeX;
        public short m_nActBeforeY;
        public int m_nAppearSound = 0;
        public int m_nAttackSound;
        public int m_nBagSize = 0;
        public int m_nBodyOffset;
        public int m_nCboHairOffset = 0;
        public int m_nCboHumWinOffSet = 0;
        public int m_nChrLight;
        public int m_nCurEffFrame;
        public int m_nCurFocusFrame;
        public int m_nCurrentAction;
        protected int m_nCurrentDefFrame;
        public int m_nCurrentEvent = 0;
        public int m_nCurrentFrame;
        /// <summary>
        /// 当前所在坐标X
        /// </summary>
        public short CurrX;
        /// <summary>
        /// 当前所在坐标Y
        /// </summary>
        public short CurrY;
        protected int m_nCurTick;
        protected int m_nDefFrameCount;
        public int m_nDie2Sound;
        public int m_nDieSound;
        public int m_nDownDrawLevel;
        protected int m_nEffectEnd = 0;
        protected int m_nEffectFrame;
        protected int m_nEffectStart = 0;
        protected int m_nEndFrame;
        public int m_nFeature;
        public int m_nFeatureEx = 0;
        public int m_nFootStepSound;
        public ushort m_nGameDiamd = 0;
        public ushort m_nGameGird = 0;
        public int m_nGameGold = 0;
        public int m_nGamePoint = 0;
        public int m_nGenAniCount = 0;
        public int m_nGold;
        public int m_nHairOffset = 0;
        public int m_nHairOffsetEx = 0;
        public int m_nHeroEnergy = 0;
        public int m_nHeroEnergyType = 0;
        public int m_nHitEffectNumber = 0;
        public int HiterCode;
        public ushort HitSpeed = 0;
        public int m_nHpx = 0;
        public int m_nHpy = 0;
        public int m_nHumWinOffset = 0;
        public int m_nIPower;
        public long m_nIPowerExp;
        public int m_nIPowerLvl;
        public int m_nMagicExplosionSound = 0;
        public int m_nMagicFireSound = 0;
        public int m_nMagicNum = 0;
        public int m_nMagicStartSound = 0;
        public int m_nMagicStruckSound;
        public int m_nMagLight;
        public int m_nMaxHeroEnergy = 0;
        protected int m_nMaxTick;
        public ArrayList m_nMoveHpList;
        public int m_nMoveSlowLevel;
        protected int m_nMoveStep;
        public int m_nNormalSound;
        public bool m_noInstanceOpenHealth;
        protected int m_nOldDir;
        protected short m_nOldx;
        protected short m_nOldy;
        public short m_nPx = 0;
        public short m_nPy = 0;
        public int RecogId;
        public int m_nRushDir = 0;
        public short m_nRx;
        public short m_nRy;
        public int m_nSayLineCount = 0;
        public short m_nSayX = 0;
        public short m_nSayY = 0;
        public int m_nScreamSound;
        public short m_nShiftX;
        public short m_nShiftY;
        protected int m_nSkipTick;
        public int m_nSpellFrame;
        public short m_nSpx = 0;
        public short m_nSpy = 0;
        protected int m_nStartFrame;
        public int m_nState;
        public int m_nStruckSound = 0;
        public int m_nStruckWeaponSound;
        public short m_nTagX = 0;
        public short m_nTagY = 0;
        public int m_nTargetRecog = 0;
        public int m_nTargetX = 0;
        public int m_nTargetY = 0;
        public byte m_nTempState;
        public int m_nWaitForFeature = 0;
        public int m_nWaitForRecogId;
        public int m_nWaitForStatus = 0;
        public int m_nWeaponOffset = 0;
        public int m_nWeaponSound;
        public int m_nWpeX = 0;
        public int m_nWpeY = 0;
        protected int m_nWpord = 0;
        public int m_nWpx = 0;
        public int m_nWpy = 0;
        public int m_nXxI = 0;
        public byte m_RushStep;
        public string m_sAutoSayMsg;
        public string m_sDescUserName;
        public string m_sLoyaly;
        public ArrayList m_StruckDamage;
        public short m_wAppearance = 0;
        public short m_wAppr;
        public short m_wGloryPoint;
        public bool n_boState;
        public TChrMsg RealActionMsg = new TChrMsg();

        public Actor(RobotPlayer robotClient)
        {
            this.robotClient = robotClient;
            m_btTitleIndex = 0;
            m_nCurFocusFrame = 0;
            m_nWaitForRecogId = 0;
            m_MsgList = new List<TChrMsg>();
            RecogId = 0;
            m_wAppr = 0;
            m_btPoisonDecHealth = 0;
            n_boState = false;
            m_btAFilter = false;
            m_nIPower = -1;
            m_nIPowerLvl = 0;
            m_nIPowerExp = 0;
            m_btAttribute = 0;
            m_nGold = 0;
            Visible = true;
            m_boHoldPlace = true;
            m_wGloryPoint = 0;
            m_nCurrentAction = 0;
            m_boReverseFrame = false;
            m_nShiftX = 0;
            m_nShiftY = 0;
            m_nDownDrawLevel = 0;
            m_nCurrentFrame = -1;
            m_nEffectFrame = -1;
            UserName = "";
            m_sAutoSayMsg = "";
            NameColor = 255;
            m_nNameColor = 255;
            m_dwSendQueryUserNameTime = MShare.GetTickCount();
            m_boWarMode = false;
            m_dwWarModeTime = 0;
            Death = false;
            m_boSkeleton = false;
            m_boItemExplore = false;
            DelActor = false;
            m_boDelActionAfterFinished = false;
            m_nChrLight = 0;
            m_nMagLight = 0;
            m_boLockEndFrame = false;
            m_dwSmoothMoveTime = 0;
            m_dwGenAnicountTime = 0;
            m_dwDefFrameTime = 0;
            m_dwLoadSurfaceTime = MShare.GetTickCount();
            m_boGrouped = false;
            OpenHealth = false;
            m_noInstanceOpenHealth = false;
            m_CurMagic.ServerMagicCode = 0;
            m_nSpellFrame = ActorConst.DEFSPELLFRAME;
            m_nNormalSound = -1;
            m_nFootStepSound = -1;
            m_nAttackSound = -1;
            m_nWeaponSound = -1;
            m_nStruckWeaponSound = -1;
            m_nScreamSound = -1;
            m_nDieSound = -1;
            m_nDie2Sound = -1;
            m_btIsHero = 0;
            m_boFisrShopItem = true;
            m_boSmiteLongHit = 0;
            m_boSmiteLongHit2 = false;
            m_boSmiteLongHitS2 = 0;
            m_boSmiteWideHit2 = false;
            m_boSmiteWideHitS2 = 0;
            m_boAttackSlow = false;
            m_boMoveSlow = false;
            m_nMoveSlowLevel = 0;
            m_btWeaponEffect = 0;
            m_StruckDamage = new ArrayList();
            m_nMoveHpList = new ArrayList();
            m_nTempState = 1;
            m_fHideMode = false;
            Abil = new Ability();
        }

        public void SendMsg(int wIdent, ushort nX, ushort nY, int ndir, int nFeature, int nState, string sStr, int nSound, int dwDelay = 0)
        {
            TChrMsg Msg = new TChrMsg();
            Msg.Ident = wIdent;
            Msg.X = (short)nX;
            Msg.Y = (short)nY;
            Msg.Dir = ndir;
            Msg.Feature = nFeature;
            Msg.State = nState;
            Msg.Saying = HUtil32.StrToInt(sStr, 0);
            Msg.Sound = nSound;
            if (dwDelay > 0)
            {
                Msg.dwDelay = MShare.GetTickCount() + dwDelay;
            }
            else
            {
                Msg.dwDelay = 0;
            }
            m_MsgList.Add(Msg);
        }

        public void UpdateMsg(short wIdent, ushort nX, ushort nY, int ndir, int nFeature, int nState, string sStr, int nSound)
        {
            TChrMsg Msg;
            var i = 0;
            while (true)
            {
                if (i >= m_MsgList.Count) break;
                Msg = m_MsgList[i];
                if (this == MShare.MySelf && Msg.Ident >= 3000 && Msg.Ident <= 3099 || Msg.Ident == wIdent)
                {
                    m_MsgList.RemoveAt(i);
                    continue;
                }
                i++;
            }
            SendMsg(wIdent, nX, nY, ndir, nFeature, nState, sStr, nSound);
        }

        public void CleanUserMsgs()
        {
            TChrMsg Msg;
            int i = 0;
            while (true)
            {
                if (i >= m_MsgList.Count) break;
                Msg = m_MsgList[i];
                if (Msg.Ident >= 3000 && Msg.Ident <= 3099)
                {
                    m_MsgList.RemoveAt(i);
                    continue;
                }
                i++;
            }
        }

        private void ReadyAction(TChrMsg Msg)
        {
            m_nActBeforeX = CurrX;
            m_nActBeforeY = CurrY;
            if (Msg.Ident == Messages.SM_ALIVE)
            {
                Death = false;
                m_boSkeleton = false;
                m_boItemExplore = false;
            }

            if (!Death)
            {
                switch (Msg.Ident)
                {
                    case Messages.SM_TURN:
                    case Messages.SM_WALK:
                    case Messages.SM_BACKSTEP:
                    case Messages.SM_RUSH:
                    case Messages.SM_RUSHKUNG:
                    case Messages.SM_RUN:
                    case Messages.SM_HORSERUN:
                    case Messages.SM_DIGUP:
                    case Messages.SM_ALIVE:
                        m_nFeature = Msg.Feature;
                        m_nState = Msg.State;
                        m_RushStep = HUtil32.HiByte(Msg.Dir);
                        if ((m_nState & PoisonState.OPENHEATH) != 0)
                        {
                            OpenHealth = true;
                        }
                        else
                        {
                            OpenHealth = false;
                        }
                        break;
                }
                if (MShare.MySelf == this)
                {
                    if (Msg.Ident == Messages.CM_WALK)
                    {
                        if (!robotClient.PlayScene.CanWalk(Msg.X, Msg.Y))
                        {
                            return;
                        }
                    }
                    if (Msg.Ident == Messages.CM_RUN)
                    {
                        if (!robotClient.PlayScene.CanRun(MShare.MySelf.CurrX, MShare.MySelf.CurrY, Msg.X, Msg.Y))
                        {
                            return;
                        }
                    }
                    if (Msg.Ident == Messages.CM_HORSERUN)
                    {
                        if (!robotClient.PlayScene.CanRun(MShare.MySelf.CurrX, MShare.MySelf.CurrY, Msg.X, Msg.Y))
                        {
                            return;
                        }
                    }
                    switch (Msg.Ident)
                    {
                        case Messages.CM_TURN:
                        case Messages.CM_WALK:
                        case Messages.CM_SITDOWN:
                        case Messages.CM_RUN:
                        case Messages.CM_HIT:
                        case Messages.CM_HEAVYHIT:
                        case Messages.CM_BIGHIT:
                        case Messages.CM_POWERHIT:
                        case Messages.CM_LONGHIT:
                        case Messages.CM_WIDEHIT:
                        case Messages.CM_CRSHIT:
                            if (Msg.Ident == Messages.CM_POWERHIT)
                            {
                                Msg.Saying = robotClient.GetMagicLv(this, 7);
                            }
                            else if (Msg.Ident == Messages.CM_LONGHIT)
                            {
                                Msg.Saying = robotClient.GetMagicLv(this, 12);
                            }
                            else if (Msg.Ident == Messages.CM_WIDEHIT)
                            {
                                Msg.Saying = robotClient.GetMagicLv(this, 25);
                            }
                            RealActionMsg = Msg;
                            Msg.Ident = Msg.Ident - 3000;
                            break;
                        case Messages.CM_HORSERUN:
                            RealActionMsg = Msg;
                            Msg.Ident = Messages.SM_HORSERUN;
                            break;
                        case Messages.CM_THROW:
                            if (m_nFeature != 0)
                            {
                                //m_nTargetX = (Msg.Feature as TActor).CurrX;
                                //m_nTargetY = (Msg.Feature as TActor).CurrY;
                                //m_nTargetRecog = (Msg.Feature as TActor).m_nRecogId;
                            }
                            RealActionMsg = Msg;
                            Msg.Ident = Messages.SM_THROW;
                            break;
                        case Messages.CM_FIREHIT:
                            Msg.Saying = robotClient.GetMagicLv(this, 26);
                            RealActionMsg = Msg;
                            Msg.Ident = Messages.SM_FIREHIT;
                            break;
                        case Messages.CM_SPELL:
                            if (MShare.MagicTarget != null)
                                Msg.Dir = BotHelper.GetFlyDirection(CurrX, CurrY, MShare.MagicTarget.CurrX,
                                    MShare.MagicTarget.CurrY);
                            RealActionMsg = Msg;
                            //UseMagic = (TUseMagicInfo)Msg.Feature;
                            //RealActionMsg.Dir = UseMagic.MagicSerial;
                            //Msg.Ident = Msg.Ident - 3000;
                            //Msg.X = robotClient.GetMagicLv(this, UseMagic.MagicSerial);
                            //Msg.Y = m_btPoisonDecHealth;
                            break;
                    }
                    m_nOldx = CurrX;
                    m_nOldy = CurrY;
                    m_nOldDir = m_btDir;
                }
                switch (Msg.Ident)
                {
                    case Messages.SM_STRUCK:
                        m_nMagicStruckSound = Msg.X;
                        m_dwLastStruckTime = MShare.GetTickCount();
                        break;
                    case Messages.SM_SPELL:
                        m_btDir = (byte)Msg.Dir;
                        //UseMagic = (TUseMagicInfo) Msg.Feature;
                        //if (UseMagic != null)
                        //{
                        //    m_CurMagic = UseMagic;
                        //    m_CurMagic.ServerMagicCode = -1;
                        //    m_CurMagic.targx = Msg.X;
                        //    m_CurMagic.targy = Msg.Y;
                        //    m_CurMagic.spelllv = Msg.X;
                        //    m_CurMagic.Poison = Msg.Y;
                        //    UseMagic = null;
                        //}
                        break;
                    case Messages.SM_FIREHIT:
                    case Messages.SM_POWERHIT:
                    case Messages.SM_LONGHIT:
                    case Messages.SM_WIDEHIT:
                        CurrX = Msg.X;
                        CurrY = Msg.Y;
                        m_btDir = (byte)Msg.Dir;
                        //m_CurMagic.magfirelv = Msg.Saying;
                        break;
                    default:
                        CurrX = Msg.X;
                        CurrY = Msg.Y;
                        m_btDir = (byte)Msg.Dir;
                        break;
                }
                m_nCurrentAction = Msg.Ident;
                CalcActorFrame();
            }
            else if (Msg.Ident == Messages.SM_SKELETON)
            {
                m_nCurrentAction = Msg.Ident;
                CalcActorFrame();
                m_boSkeleton = true;
            }
            if (Msg.Ident == Messages.SM_DEATH || Msg.Ident == Messages.SM_NOWDEATH)
            {
                Death = true;
                if (HUtil32.HiByte(Msg.Dir) != 0) m_boItemExplore = true;
                robotClient.PlayScene.ActorDied(this);
            }
        }

        private bool GetMessage(ref TChrMsg chrMsg)
        {
            bool result = false;
            int i = 0;
            while (m_MsgList.Count > i)
            {
                TChrMsg Msg = m_MsgList[i];
                if (Msg.dwDelay != 0 && MShare.GetTickCount() < Msg.dwDelay)
                {
                    i++;
                    continue;
                }
                chrMsg = Msg;
                m_MsgList.RemoveAt(i);
                result = true;
                break;
            }
            return result;
        }

        public void ProcMsg()
        {
            m_ChrMsg = default(TChrMsg);
            while (m_nCurrentAction == 0 && GetMessage(ref m_ChrMsg))
            {
                switch (m_ChrMsg.Ident)
                {
                    case Messages.SM_STRUCK:
                        HiterCode = m_ChrMsg.Sound;
                        ReadyAction(m_ChrMsg);
                        break;
                    case Messages.SM_DEATH:
                    case Messages.SM_NOWDEATH:
                    case Messages.SM_SKELETON:
                    case Messages.SM_ALIVE:
                    case Messages.SM_ACTION_MIN:
                    case Messages.SM_ACTION2_MIN:
                        ReadyAction(m_ChrMsg);
                        break;
                    case Messages.SM_SPACEMOVE_SHOW:
                        m_ChrMsg.Ident = Messages.SM_TURN;
                        ReadyAction(m_ChrMsg);
                        break;
                    case Messages.SM_SPACEMOVE_SHOW2:
                        m_ChrMsg.Ident = Messages.SM_TURN;
                        ReadyAction(m_ChrMsg);
                        break;
                    default:
                        if (HUtil32.RangeInDefined(m_ChrMsg.Ident, 3000, 3099))
                        {
                            ReadyAction(m_ChrMsg);
                        }
                        break;
                }
            }
        }

        public void ProcHurryMsg()
        {
            TChrMsg Msg;
            int n = 0;
            while (true)
            {
                if (m_MsgList.Count <= n) break;
                Msg = m_MsgList[n];
                bool fin = false;
                switch (Msg.Ident)
                {
                    case Messages.SM_MAGICFIRE:
                        if (m_CurMagic.ServerMagicCode != 0)
                        {
                            m_CurMagic.ServerMagicCode = 255;
                            m_CurMagic.Target = Msg.X;
                            if (Msg.Y >= 0 && Msg.Y <= 16) m_CurMagic.EffectType = (MagicType)Msg.Y;
                            m_CurMagic.EffectNumber = Msg.Dir % 255;
                            m_CurMagic.targx = Msg.Feature;
                            m_CurMagic.targy = Msg.State;
                            //m_CurMagic.magfirelv = Msg.Saying;
                            m_CurMagic.Recusion = true;
                            fin = true;
                        }
                        break;
                    case Messages.SM_MAGICFIRE_FAIL:
                        if (m_CurMagic.ServerMagicCode != 0)
                        {
                            m_CurMagic.ServerMagicCode = 0;
                            fin = true;
                        }
                        break;
                }
                if (fin)
                {
                    m_MsgList.RemoveAt(n);
                }
                else
                {
                    n++;
                }
            }
        }

        public bool IsIdle()
        {
            return m_nCurrentAction == 0 && m_MsgList.Count == 0;
        }

        public bool ActionFinished()
        {
            bool result;
            if (m_nCurrentAction == 0 || m_nCurrentFrame >= m_nEndFrame)
                result = true;
            else
                result = false;
            return result;
        }

        public int CanWalk()
        {
            int result;
            if (MShare.GetTickCount() - MShare.LatestSpellTick < MShare.g_dwMagicPKDelayTime)
                result = -1;
            else
                result = 1;
            return result;
        }

        public int CanRun()
        {
            var result = 1;
            if (Abil.HP < ActorConst.RUN_MINHEALTH)
            {
                result = -1;
            }
            return result;
        }

        public bool Strucked()
        {
            var result = false;
            for (var i = 0; i < m_MsgList.Count; i++)
            {
                if (m_MsgList[i].Ident == Messages.SM_STRUCK)
                {
                    result = true;
                    break;
                }
            }
            return result;
        }

        public void Shift(int dir, int step, int cur, int max)
        {
            int ss;
            int v;
            short funx;
            short funy;
            short unx = (short)(BotConst.UNITX * step);
            short uny = (short)(BotConst.UNITY * step);
            if (cur > max) cur = max;
            m_nRx = CurrX;
            m_nRy = CurrY;
            switch (dir)
            {
                case Direction.Up:
                    ss = HUtil32.Round((max - cur) / max) * step;
                    m_nRy = (short)(CurrY + ss);
                    if (ss == step)
                    {
                        funx = (short)-HUtil32.Round(uny / max * cur);
                        if (funx % 2 != 0) funx = (short)(funx + 1);
                        m_nShiftY = funx;
                    }
                    else
                    {
                        funx = (short)HUtil32.Round(uny / max * (max - cur));
                        if (funx % 2 != 0) funx = (short)(funx + 1);
                        m_nShiftY = funx;
                    }

                    break;
                case Direction.UpRight:
                    if (max >= 6)
                        v = 2;
                    else
                        v = 0;
                    ss = HUtil32.Round((max - cur + v) / max) * step;
                    m_nRx = (short)(CurrX - ss);
                    m_nRy = (short)(CurrY + ss);
                    if (ss == step)
                    {
                        funx = (short)HUtil32.Round(unx / max * cur);
                        if (funx % 2 != 0) funx = (short)(funx + 1);
                        m_nShiftX = funx;
                        funy = (short)-HUtil32.Round(uny / max * cur);
                        if (funy % 2 != 0) funy = (short)(funy + 1);
                        m_nShiftY = funy;
                    }
                    else
                    {
                        funx = (short)-HUtil32.Round(unx / max * (max - cur));
                        if (funx % 2 != 0) funx = (short)(funx + 1);
                        m_nShiftX = funx;
                        funy = (short)HUtil32.Round(uny / max * (max - cur));
                        if (funy % 2 != 0) funy = (short)(funy + 1);
                        m_nShiftY = funy;
                    }

                    break;
                case Direction.Right:
                    ss = HUtil32.Round((max - cur) / max) * step;
                    m_nRx = (short)(CurrX - ss);
                    if (ss == step)
                        m_nShiftX = (short)HUtil32.Round(unx / max * cur);
                    else
                        m_nShiftX = (short)-HUtil32.Round(unx / max * (max - cur));
                    m_nShiftY = 0;
                    break;
                case Direction.DownRight:
                    if (max >= 6)
                        v = 2;
                    else
                        v = 0;
                    ss = HUtil32.Round((max - cur - v) / max) * step;
                    m_nRx = (short)(CurrX - ss);
                    m_nRy = (short)(CurrY - ss);
                    if (ss == step)
                    {
                        funx = (short)HUtil32.Round(unx / max * cur);
                        if (funx % 2 != 0) funx = (short)(funx + 1);
                        m_nShiftX = funx;
                        funy = (short)HUtil32.Round(uny / max * cur);
                        if (funy % 2 != 0) funy = (short)(funy + 1);
                        m_nShiftY = funy;
                    }
                    else
                    {
                        funx = (short)-HUtil32.Round(unx / max * (max - cur));
                        if (funx % 2 != 0) funx = (short)(funx + 1);
                        m_nShiftX = funx;
                        funy = (short)-HUtil32.Round(uny / max * (max - cur));
                        if (funy % 2 != 0) funy = (short)(funy + 1);
                        m_nShiftY = funy;
                    }

                    break;
                case Direction.Down:
                    if (max >= 6)
                        v = 1;
                    else
                        v = 0;
                    ss = HUtil32.Round((max - cur - v) / max) * step;
                    m_nShiftX = 0;
                    m_nRy = (short)(CurrY - ss);
                    if (ss == step)
                    {
                        funy = (short)HUtil32.Round(uny / max * cur);
                        if (funy % 2 != 0) funy = (short)(funy + 1);
                        m_nShiftY = funy;
                    }
                    else
                    {
                        funy = (short)-HUtil32.Round(uny / max * (max - cur));
                        if (funy % 2 != 0) funy = (short)(funy + 1);
                        m_nShiftY = funy;
                    }

                    break;
                case Direction.DownLeft:
                    if (max >= 6)
                        v = 2;
                    else
                        v = 0;
                    ss = HUtil32.Round((max - cur - v) / max) * step;
                    m_nRx = (short)(CurrX + ss);
                    m_nRy = (short)(CurrY - ss);
                    if (ss == step)
                    {
                        funx = (short)-HUtil32.Round(unx / max * cur);
                        if (funx % 2 != 0) funx = (short)(funx + 1);
                        m_nShiftX = funx;
                        funy = (short)HUtil32.Round(uny / max * cur);
                        if (funy % 2 != 0) funy = (short)(funy + 1);
                        m_nShiftY = funy;
                    }
                    else
                    {
                        funx = (short)HUtil32.Round(unx / max * (max - cur));
                        if (funx % 2 != 0) funx = (short)(funx + 1);
                        m_nShiftX = funx;
                        funy = (short)-HUtil32.Round(uny / max * (max - cur));
                        if (funy % 2 != 0) funy = (short)(funy + 1);
                        m_nShiftY = funy;
                    }

                    break;
                case Direction.Left:
                    ss = HUtil32.Round((max - cur) / max) * step;
                    m_nRx = (short)(CurrX + ss);
                    if (ss == step)
                        m_nShiftX = (short)-HUtil32.Round(unx / max * cur);
                    else
                        m_nShiftX = (short)HUtil32.Round(unx / max * (max - cur));
                    m_nShiftY = 0;
                    break;
                case Direction.UpLeft:
                    if (max >= 6)
                        v = 2;
                    else
                        v = 0;
                    ss = HUtil32.Round((max - cur + v) / max) * step;
                    m_nRx = (short)(CurrX + ss);
                    m_nRy = (short)(CurrY + ss);
                    if (ss == step)
                    {
                        funx = (short)-HUtil32.Round(unx / max * cur);
                        if (funx % 2 != 0) funx = (short)(funx + 1);
                        m_nShiftX = funx;
                        funy = (short)-HUtil32.Round(uny / max * cur);
                        if (funy % 2 != 0) funy = (short)(funy + 1);
                        m_nShiftY = funy;
                    }
                    else
                    {
                        funx = (short)HUtil32.Round(unx / max * (max - cur));
                        if (funx % 2 != 0) funx = (short)(funx + 1);
                        m_nShiftX = funx;
                        funy = (short)HUtil32.Round(uny / max * (max - cur));
                        if (funy % 2 != 0) funy = (short)(funy + 1);
                        m_nShiftY = funy;
                    }

                    break;
            }
        }

        public virtual void FeatureChanged()
        {

        }

        public virtual int light()
        {
            return m_nChrLight;
        }

        public int CharWidth()
        {
            return 48;
        }

        public int CharHeight()
        {
            return 70;
        }

        public bool CheckSelect(int dx, int dy)
        {
            return false;
        }

        public virtual int GetDefaultFrame(bool wmode)
        {
            int cf;
            int result = 0;
            TMonsterAction pm = ActorConst.GetRaceByPM(Race, m_wAppearance);
            if (pm == null) return result;
            if (Death)
            {
                if (m_boSkeleton)
                    result = pm.ActDeath.start;
                else
                    result = pm.ActDie.start + m_btDir * (pm.ActDie.frame + pm.ActDie.skip) + (pm.ActDie.frame - 1);
            }
            else
            {
                m_nDefFrameCount = pm.ActStand.frame;
                if (m_nCurrentDefFrame < 0)
                    cf = 0;
                else if (m_nCurrentDefFrame >= pm.ActStand.frame)
                    cf = 0;
                else
                    cf = m_nCurrentDefFrame;
                result = pm.ActStand.start + m_btDir * (pm.ActStand.frame + pm.ActStand.skip) + cf;
            }

            return result;
        }

        public virtual void DefaultMotion()
        {
            m_boReverseFrame = false;
            if (m_boWarMode)
                if (MShare.GetTickCount() - m_dwWarModeTime > 4 * 1000)
                    m_boWarMode = false;
            m_nCurrentFrame = GetDefaultFrame(m_boWarMode);
            Shift(m_btDir, 0, 1, 1);
        }

        public virtual void RunFrameAction(int frame)
        {

        }

        public virtual void ActionEnded()
        {
        }

        public virtual void ReadyNextAction()
        {
        }

        protected virtual void CalcActorFrame()
        {
            if (m_nCurrentAction == Messages.SM_TURN)
            {
                if (m_fHideMode)
                {
                    m_fHideMode = false;
                    m_nCurrentAction = 0;
                }
            }
        }

        public bool Run_MagicTimeOut()
        {
            bool result;
            if (this == MShare.MySelf)
                result = MShare.GetTickCount() - m_dwWaitMagicRequest > 1800;
            else
                result = MShare.GetTickCount() - m_dwWaitMagicRequest > 900;
            if (result) m_CurMagic.ServerMagicCode = 0;
            return result;
        }

        public virtual void Run()
        {
            long dwFrameTimetime;
            if (m_nCurrentAction == Messages.SM_WALK || m_nCurrentAction == Messages.SM_BACKSTEP ||
                m_nCurrentAction == Messages.SM_RUN || m_nCurrentAction == Messages.SM_HORSERUN ||
                m_nCurrentAction == Messages.SM_RUSH || m_nCurrentAction == Messages.SM_RUSHKUNG) return;
            m_boMsgMuch = false;
            if (this != MShare.MySelf)
                if (m_MsgList.Count >= 2)
                    m_boMsgMuch = true;
            RunFrameAction(m_nCurrentFrame - m_nStartFrame);
            var prv = m_nCurrentFrame;
            if (m_nCurrentAction != 0)
            {
                if (m_nCurrentFrame < m_nStartFrame || m_nCurrentFrame > m_nEndFrame) m_nCurrentFrame = m_nStartFrame;
                if (this != MShare.MySelf && m_boUseMagic)
                    dwFrameTimetime = HUtil32.Round(m_dwFrameTime / 1.8);
                else if (m_boMsgMuch)
                    dwFrameTimetime = HUtil32.Round(m_dwFrameTime * 2 / 3);
                else
                    dwFrameTimetime = m_dwFrameTime;
                if (MShare.GetTickCount() - m_dwStartTime > dwFrameTimetime)
                {
                    if (m_nCurrentFrame < m_nEndFrame)
                    {
                        if (m_boUseMagic)
                        {
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
                    }
                    else
                    {
                        if (m_boDelActionAfterFinished) DelActor = true;
                        if (this == MShare.MySelf)
                        {
                            if (robotClient.ServerAcceptNextAction())
                            {
                                ActionEnded();
                                m_nCurrentAction = 0;
                                m_boUseMagic = false;
                                if (Race != 50)
                                {
                                    m_boUseEffect = false;
                                    m_boHitEffect = false;
                                }
                            }
                        }
                        else
                        {
                            ActionEnded();
                            m_nCurrentAction = 0;
                            m_boUseMagic = false;
                            if (Race != 50)
                            {
                                m_boUseEffect = false;
                                m_boHitEffect = false;
                            }
                        }
                    }
                    if (m_boUseMagic)
                    {
                        if (m_nCurEffFrame == m_nSpellFrame - 1)
                        {
                            if (m_CurMagic.ServerMagicCode > 0)
                            {
                                //robotClient.g_PlayScene.NewMagic(this, m_CurMagic.ServerMagicCode, m_CurMagic.EffectNumber, CurrX, CurrY, m_CurMagic.targx, m_CurMagic.targy, m_CurMagic.target, m_CurMagic.EffectType, m_CurMagic.Recusion, m_CurMagic.anitime, ref boFly, m_CurMagic.magfirelv);
                            }
                            m_CurMagic.ServerMagicCode = 0;
                        }
                    }
                }
                if (m_wAppearance == 0 || m_wAppearance == 1 || m_wAppearance == 43)
                {
                    m_nCurrentDefFrame = -10;
                }
                {
                    m_nCurrentDefFrame = 0;
                }
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

        public bool Move()
        {
            bool result;
            int prv;
            int curstep;
            int maxstep;
            bool fastmove;
            bool normmove;
            result = false;
            fastmove = false;
            normmove = false;
            if (m_nCurrentAction == Messages.SM_BACKSTEP) fastmove = true;
            if (m_nCurrentAction == Messages.SM_RUSH || m_nCurrentAction == Messages.SM_RUSHKUNG) normmove = true;
            if (!fastmove && !normmove)
            {
                m_boMoveSlow = false;
                m_boAttackSlow = false;
                m_nMoveSlowLevel = 0;
                if ((m_nState & 0x10000000) != 0)
                {
                    m_nMoveSlowLevel = 1;
                    m_boMoveSlow = true;
                }

                if (m_boMoveSlow && m_nSkipTick < m_nMoveSlowLevel)
                {
                    m_nSkipTick++;
                    return result;
                }

                m_nSkipTick = 0;
                if (Race == 0 && this == MShare.MySelf)
                    if (new ArrayList(new[] { 5, 9, 11, 13 }).Contains(m_nCurrentAction))
                        switch (m_nCurrentFrame - m_nStartFrame)
                        {
                            case 1:
                                break;
                            case 4:
                                break;
                        }
            }

            result = false;
            m_boMsgMuch = this != MShare.MySelf && m_MsgList.Count >= 2;
            prv = m_nCurrentFrame;
            if (m_nCurrentAction == Messages.SM_WALK || m_nCurrentAction == Messages.SM_RUN ||
                m_nCurrentAction == Messages.SM_HORSERUN || m_nCurrentAction == Messages.SM_RUSH ||
                m_nCurrentAction == Messages.SM_RUSHKUNG)
            {
                if (m_nCurrentFrame < m_nStartFrame || m_nCurrentFrame > m_nEndFrame) m_nCurrentFrame = m_nStartFrame - 1;
                if (m_nCurrentFrame < m_nEndFrame)
                {
                    m_nCurrentFrame++;
                    if (m_boMsgMuch && !normmove)
                        if (m_nCurrentFrame < m_nEndFrame)
                            m_nCurrentFrame++;
                    curstep = m_nCurrentFrame - m_nStartFrame + 1;
                    maxstep = m_nEndFrame - m_nStartFrame + 1;
                    Shift(m_btDir, m_nMoveStep, curstep, maxstep);
                }

                if (m_nCurrentFrame >= m_nEndFrame)
                {
                    if (this == MShare.MySelf)
                    {
                        if (robotClient.ServerAcceptNextAction())
                        {
                            m_nCurrentAction = 0;
                            m_boLockEndFrame = true;
                            m_dwSmoothMoveTime = MShare.GetTickCount();
                            m_boUseCboLib = false;
                        }
                    }
                    else
                    {
                        m_nCurrentAction = 0;
                        m_boLockEndFrame = true;
                        m_dwSmoothMoveTime = MShare.GetTickCount();
                        m_boUseCboLib = false;
                    }
                }

                if (m_nCurrentAction == Messages.SM_RUSH)
                    if (this == MShare.MySelf)
                    {
                        MShare.g_dwDizzyDelayStart = MShare.GetTickCount();
                        MShare.g_dwDizzyDelayTime = 300;
                    }

                if (m_nCurrentAction == Messages.SM_RUSHKUNG)
                    if (m_nCurrentFrame >= m_nEndFrame - 3)
                    {
                        CurrX = m_nActBeforeX;
                        CurrY = m_nActBeforeY;
                        m_nRx = CurrX;
                        m_nRy = CurrY;
                        m_nCurrentAction = 0;
                        m_boUseCboLib = false;
                        m_boLockEndFrame = true;
                    }

                result = true;
            }

            if (m_nCurrentAction == Messages.SM_BACKSTEP)
            {
                if (m_nCurrentFrame > m_nEndFrame || m_nCurrentFrame < m_nStartFrame) m_nCurrentFrame = m_nEndFrame + 1;
                if (m_nCurrentFrame > m_nStartFrame)
                {
                    m_nCurrentFrame -= 1;
                    if (m_boMsgMuch || fastmove)
                        if (m_nCurrentFrame > m_nStartFrame)
                            m_nCurrentFrame -= 1;
                    curstep = m_nEndFrame - m_nCurrentFrame + 1;
                    maxstep = m_nEndFrame - m_nStartFrame + 1;
                    Shift(BotHelper.GetBack(m_btDir), m_nMoveStep, curstep, maxstep);
                }

                if (m_nCurrentFrame <= m_nStartFrame)
                {
                    if (this == MShare.MySelf)
                    {
                        m_nCurrentAction = 0;
                        m_boLockEndFrame = true;
                        m_dwSmoothMoveTime = MShare.GetTickCount();
                        MShare.g_dwDizzyDelayStart = MShare.GetTickCount();
                        MShare.g_dwDizzyDelayTime = 1000;
                    }
                    else
                    {
                        m_nCurrentAction = 0;
                        m_boLockEndFrame = true;
                        m_dwSmoothMoveTime = MShare.GetTickCount();
                    }
                }

                result = true;
            }

            if (prv != m_nCurrentFrame) m_dwLoadSurfaceTime = MShare.GetTickCount();
            return result;
        }

        public void MoveFail()
        {
            m_nCurrentAction = 0;
            m_boLockEndFrame = true;
            MShare.MySelf.CurrX = m_nOldx;
            MShare.MySelf.CurrY = m_nOldy;
            MShare.MySelf.m_btDir = (byte)m_nOldDir;
            CleanUserMsgs();
        }

        public bool CanCancelAction()
        {
            var result = false;
            if (m_nCurrentAction == Messages.SM_HIT)
            {
                if (!m_boUseEffect)
                {
                    result = true;
                }
            }
            return result;
        }

        public void CancelAction()
        {
            m_nCurrentAction = 0;
            m_boLockEndFrame = true;
        }

        public void CleanCharMapSetting(short X, short Y)
        {
            MShare.MySelf.CurrX = X;
            MShare.MySelf.CurrY = Y;
            MShare.MySelf.m_nRx = X;
            MShare.MySelf.m_nRy = Y;
            m_nOldx = X;
            m_nOldy = Y;
            m_nCurrentAction = 0;
            m_nCurrentFrame = -1;
            CleanUserMsgs();
        }

        public void StruckShowDamage(string Str)
        {
            //m_StruckDamage.Add(Str, idx);
        }

        public void GetMoveHPShow(int nCount)
        {
        }

        public void Say(string Str)
        {
        }
    }
}