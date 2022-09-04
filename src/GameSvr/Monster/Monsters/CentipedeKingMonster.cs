using GameSvr.Actor;
using SystemModule;
using SystemModule.Packet.ClientPackets;

namespace GameSvr.Monster.Monsters
{
    public class CentipedeKingMonster : StickMonster
    {
        public int m_dwAttickTick = 0;

        public CentipedeKingMonster() : base()
        {
            ViewRange = 6;
            nComeOutValue = 4;
            nAttackRange = 6;
            Animal = false;
            m_dwAttickTick = HUtil32.GetTickCount();
        }

        private bool sub_4A5B0C()
        {
            var result = false;
            TBaseObject BaseObject;
            for (var i = 0; i < VisibleActors.Count; i++)
            {
                BaseObject = VisibleActors[i].BaseObject;
                if (BaseObject.Death)
                {
                    continue;
                }
                if (IsProperTarget(BaseObject))
                {
                    if (Math.Abs(CurrX - BaseObject.CurrX) <= ViewRange && Math.Abs(CurrY - BaseObject.CurrY) <= ViewRange)
                    {
                        result = true;
                        break;
                    }
                }
            }
            return result;
        }

        protected override bool AttackTarget()
        {
            var result = false;
            TAbility WAbil;
            int nPower;
            TBaseObject BaseObject;
            if (!sub_4A5B0C())
            {
                return result;
            }
            if ((HUtil32.GetTickCount() - AttackTick) > NextHitTime)
            {
                AttackTick = HUtil32.GetTickCount();
                SendAttackMsg(Grobal2.RM_HIT, Direction, CurrX, CurrY);
                WAbil = m_WAbil;
                nPower = M2Share.RandomNumber.Random(HUtil32.HiWord(WAbil.DC) - HUtil32.LoWord(WAbil.DC) + 1) + HUtil32.LoWord(WAbil.DC);
                for (var i = 0; i < VisibleActors.Count; i++)
                {
                    BaseObject = VisibleActors[i].BaseObject;
                    if (BaseObject.Death)
                    {
                        continue;
                    }
                    if (IsProperTarget(BaseObject))
                    {
                        if (Math.Abs(CurrX - BaseObject.CurrX) < ViewRange && Math.Abs(CurrY - BaseObject.CurrY) < ViewRange)
                        {
                            TargetFocusTick = HUtil32.GetTickCount();
                            SendDelayMsg(this, Grobal2.RM_DELAYMAGIC, (short)nPower, HUtil32.MakeLong(BaseObject.CurrX, BaseObject.CurrY), 2, BaseObject.ObjectId, "", 600);
                            if (M2Share.RandomNumber.Random(4) == 0)
                            {
                                if (M2Share.RandomNumber.Random(3) != 0)
                                {
                                    BaseObject.MakePosion(Grobal2.POISON_DECHEALTH, 60, 3);
                                }
                                else
                                {
                                    BaseObject.MakePosion(Grobal2.POISON_STONE, 5, 0);
                                }
                                TargetCret = BaseObject;
                            }
                        }
                    }
                }
            }
            result = true;
            return result;
        }

        protected override void ComeOut()
        {
            base.ComeOut();
            m_WAbil.HP = m_WAbil.MaxHP;
        }

        public override void Run()
        {
            TBaseObject BaseObject;
            if (!Ghost && !Death && m_wStatusTimeArr[Grobal2.POISON_STONE] == 0)
            {
                if ((HUtil32.GetTickCount() - WalkTick) > WalkSpeed)
                {
                    WalkTick = HUtil32.GetTickCount();
                    if (FixedHideMode)
                    {
                        if ((HUtil32.GetTickCount() - m_dwAttickTick) > 10000)
                        {
                            for (var i = 0; i < VisibleActors.Count; i++)
                            {
                                BaseObject = VisibleActors[i].BaseObject;
                                if (BaseObject.Death)
                                {
                                    continue;
                                }
                                if (IsProperTarget(BaseObject))
                                {
                                    if (!BaseObject.HideMode || CoolEye)
                                    {
                                        if (Math.Abs(CurrX - BaseObject.CurrX) < nComeOutValue && Math.Abs(CurrY - BaseObject.CurrY) < nComeOutValue)
                                        {
                                            ComeOut();
                                            m_dwAttickTick = HUtil32.GetTickCount();
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if ((HUtil32.GetTickCount() - m_dwAttickTick) > 3000)
                        {
                            if (AttackTarget())
                            {
                                base.Run();
                                return;
                            }
                            if ((HUtil32.GetTickCount() - m_dwAttickTick) > 10000)
                            {
                                ComeDown();
                                m_dwAttickTick = HUtil32.GetTickCount();
                            }
                        }
                    }
                }
            }
            base.Run();
        }
    }
}

