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
            GuardDirection = -1;
            Race = 112;
        }

        private void sub_4A6B30(BaseObject targeTBaseObject)
        {
            Direction = M2Share.GetNextDirection(CurrX, CurrY, targeTBaseObject.CurrX, targeTBaseObject.CurrY);
            TAbility wAbil = Abil;
            var nPower = M2Share.RandomNumber.Random(HUtil32.HiWord(wAbil.DC) - HUtil32.LoWord(wAbil.DC) + 1) + HUtil32.LoWord(wAbil.DC);
            if (nPower > 0)
            {
                nPower = targeTBaseObject.GetHitStruckDamage(this, nPower);
            }
            if (nPower > 0)
            {
                targeTBaseObject.SetLastHiter(this);
                targeTBaseObject.ExpHitter = null;
                targeTBaseObject.StruckDamage(nPower);
                targeTBaseObject.SendDelayMsg(Grobal2.RM_STRUCK, Grobal2.RM_10101, (short)nPower, targeTBaseObject.Abil.HP, targeTBaseObject.Abil.MaxHP, ActorId, "", HUtil32._MAX(Math.Abs(CurrX - targeTBaseObject.CurrX), Math.Abs(CurrY - targeTBaseObject.CurrY)) * 50 + 600);
            }
            SendRefMsg(Grobal2.RM_FLYAXE, Direction, CurrX, CurrY, targeTBaseObject.ActorId, "");
        }

        public override void Run()
        {
            int nRage = 9999;
            BaseObject targetBaseObject = null;
            if (CanWalk())
            {
                if ((HUtil32.GetTickCount() - WalkTick) >= WalkSpeed)
                {
                    WalkTick = HUtil32.GetTickCount();
                    for (var i = 0; i < VisibleActors.Count; i++)
                    {
                        var baseObject = VisibleActors[i].BaseObject;
                        if (baseObject.Death)
                        {
                            continue;
                        }
                        if (IsProperTarget(baseObject))
                        {
                            var nAbs = Math.Abs(CurrX - baseObject.CurrX) + Math.Abs(CurrY - baseObject.CurrY);
                            if (nAbs < nRage)
                            {
                                nRage = nAbs;
                                targetBaseObject = baseObject;
                            }
                        }
                    }
                    if (targetBaseObject != null)
                    {
                        SetTargetCreat(targetBaseObject);
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
                    if (GuardDirection > 0 && Direction != GuardDirection)
                    {
                        TurnTo((byte)GuardDirection);
                    }
                }
            }
            base.Run();
        }
    }
}

