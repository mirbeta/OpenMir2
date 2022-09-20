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
            Race = Grobal2.RC_ARCHERGUARD;
        }

        private void AttackTarger(BaseObject targeTBaseObject)
        {
            Direction = M2Share.GetNextDirection(CurrX, CurrY, targeTBaseObject.CurrX, targeTBaseObject.CurrY);
            Ability wAbil = Abil;
            var nDamage = M2Share.RandomNumber.Random(Math.Abs(HUtil32.HiWord(wAbil.DC) - HUtil32.LoWord(wAbil.DC) + 1)) + HUtil32.LoWord(wAbil.DC);
            if (nDamage > 0)
            {
                nDamage = targeTBaseObject.GetHitStruckDamage(this, nDamage);
            }
            if (nDamage > 0)
            {
                targeTBaseObject.SetLastHiter(this);
                targeTBaseObject.ExpHitter = null;
                targeTBaseObject.StruckDamage(nDamage);
                targeTBaseObject.SendDelayMsg(Grobal2.RM_STRUCK, Grobal2.RM_10101, nDamage, targeTBaseObject.Abil.HP, targeTBaseObject.Abil.MaxHP, ActorId, "", HUtil32._MAX(Math.Abs(CurrX - targeTBaseObject.CurrX), Math.Abs(CurrY - targeTBaseObject.CurrY)) * 50 + 600);
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
                        AttackTarger(TargetCret);
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

