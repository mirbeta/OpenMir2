using OpenMir2;
using OpenMir2.Enums;
using SystemModule;
using SystemModule.Actors;
using SystemModule.Enums;

namespace M2Server.Monster.Monsters
{
    /// <summary>
    /// 弓箭守卫
    /// </summary>
    public class ArcherGuard : GuardUnit
    {
        public ArcherGuard() : base()
        {
            ViewRange = 12;
            WantRefMsg = true;
            Castle = null;
            GuardDirection = -1;
            Race = ActorRace.ArcherGuard;
        }

        private void AttackTarger(IActor targetObject)
        {
            Dir = M2Share.GetNextDirection(CurrX, CurrY, targetObject.CurrX, targetObject.CurrY);
            int nDamage = HUtil32.LoByte(WAbil.DC) + M2Share.RandomNumber.Random(Math.Abs(HUtil32.HiByte(WAbil.DC) - HUtil32.LoByte(WAbil.DC)) + 1);
            if (nDamage > 0)
            {
                nDamage = targetObject.GetHitStruckDamage(this, nDamage);
            }
            if (nDamage > 0)
            {
                targetObject.SetLastHiter(this);
                targetObject.ExpHitter = null;
                targetObject.StruckDamage(nDamage);
                targetObject.SendStruckDelayMsg(Messages.RM_REFMESSAGE, nDamage, targetObject.WAbil.HP, targetObject.WAbil.MaxHP, ActorId, "", HUtil32._MAX(Math.Abs(CurrX - targetObject.CurrX), Math.Abs(CurrY - targetObject.CurrY)) * 50 + 600);
            }
            SendRefMsg(Messages.RM_FLYAXE, Dir, CurrX, CurrY, targetObject.ActorId, "");
        }

        public override void Run()
        {
            int nRage = 9999;
            IActor targetBaseObject = null;
            if (CanMove())
            {
                if ((HUtil32.GetTickCount() - WalkTick) >= WalkSpeed)
                {
                    WalkTick = HUtil32.GetTickCount();
                    for (int i = 0; i < VisibleActors.Count; i++)
                    {
                        IActor baseObject = VisibleActors[i].BaseObject;
                        if (baseObject.Death)
                        {
                            continue;
                        }
                        if (IsProperTarget(baseObject))
                        {
                            int nAbs = Math.Abs(CurrX - baseObject.CurrX) + Math.Abs(CurrY - baseObject.CurrY);
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
                    if (GuardDirection > 0 && Dir != GuardDirection)
                    {
                        TurnTo((byte)GuardDirection);
                    }
                }
            }
            base.Run();
        }
    }
}