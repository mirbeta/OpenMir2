using GameSvr.Actor;
using SystemModule;
using SystemModule.Packet.ClientPackets;

namespace GameSvr.Monster.Monsters
{
    public class ArcherGuard : GuardUnit
    {
        public ArcherGuard() : base()
        {
            ViewRange = 12;
            WantRefMsg = true;
            Castle = null;
            Direction = 0;
        }

        private void sub_4A6B30(TBaseObject TargeTBaseObject)
        {
            Direction = M2Share.GetNextDirection(CurrX, CurrY, TargeTBaseObject.CurrX, TargeTBaseObject.CurrY);
            TAbility WAbil = m_WAbil;
            var nPower = M2Share.RandomNumber.Random(HUtil32.HiWord(WAbil.DC) - HUtil32.LoWord(WAbil.DC) + 1) + HUtil32.LoWord(WAbil.DC);
            if (nPower > 0)
            {
                nPower = TargeTBaseObject.GetHitStruckDamage(this, nPower);
            }
            if (nPower > 0)
            {
                TargeTBaseObject.SetLastHiter(this);
                TargeTBaseObject.ExpHitter = null;
                TargeTBaseObject.StruckDamage(nPower);
                TargeTBaseObject.SendDelayMsg(Grobal2.RM_STRUCK, Grobal2.RM_10101, (short)nPower, TargeTBaseObject.m_WAbil.HP, TargeTBaseObject.m_WAbil.MaxHP, ObjectId, "", HUtil32._MAX(Math.Abs(CurrX - TargeTBaseObject.CurrX), Math.Abs(CurrY - TargeTBaseObject.CurrY)) * 50 + 600);
            }
            SendRefMsg(Grobal2.RM_FLYAXE, Direction, CurrX, CurrY, TargeTBaseObject.ObjectId, "");
        }

        public override void Run()
        {
            int nRage = 9999;
            TBaseObject BaseObject = null;
            TBaseObject TargetBaseObject = null;
            if (!Death && !Ghost && m_wStatusTimeArr[Grobal2.POISON_STONE] == 0)
            {
                if ((HUtil32.GetTickCount() - WalkTick) >= WalkSpeed)
                {
                    WalkTick = HUtil32.GetTickCount();
                    for (var i = 0; i < VisibleActors.Count; i++)
                    {
                        BaseObject = VisibleActors[i].BaseObject;
                        if (BaseObject.Death)
                        {
                            continue;
                        }
                        if (IsProperTarget(BaseObject))
                        {
                            var nAbs = Math.Abs(CurrX - BaseObject.CurrX) + Math.Abs(CurrY - BaseObject.CurrY);
                            if (nAbs < nRage)
                            {
                                nRage = nAbs;
                                TargetBaseObject = BaseObject;
                            }
                        }
                    }
                    if (TargetBaseObject != null)
                    {
                        SetTargetCreat(TargetBaseObject);
                    }
                    else
                    {
                        DelTargetCreat();
                    }
                }
                if (TargetCret != null)
                {
                    if ((HUtil32.GetTickCount() - AttackTick) >= NextHitTime)
                    {
                        AttackTick = HUtil32.GetTickCount();
                        sub_4A6B30(TargetCret);
                    }
                }
                else
                {
                    if (Direction > 0 && Direction != Direction)
                    {
                        TurnTo(Direction);
                    }
                }
            }
            base.Run();
        }
    }
}

