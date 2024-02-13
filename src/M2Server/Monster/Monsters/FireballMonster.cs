using OpenMir2;
using SystemModule.Actors;

namespace M2Server.Monster.Monsters
{
    public class FireballMonster : MagicMonster
    {
        public FireballMonster() : base()
        {
            SpellTick = HUtil32.GetTickCount();
            SearchTime = M2Share.RandomNumber.Random(1500) + 1500;
        }

        public override void Run()
        {
            if (!Death && !Ghost)
            {
                if (TargetCret != null)
                {
                    if (MagCanHitTarget(TargetCret.CurrX, TargetCret.CurrY, TargetCret))
                    {
                        if (IsProperTarget(TargetCret))
                        {
                            if (Math.Abs(TargetX - CurrX) <= 8 && Math.Abs(TargetY - CurrY) <= 8)
                            {
                                int nPower = HUtil32.LoByte(WAbil.DC) + M2Share.RandomNumber.Random(Math.Abs(HUtil32.HiByte(WAbil.DC) - HUtil32.LoByte(WAbil.DC)) + 1);
                                if (nPower > 0)
                                {
                                    IActor baseObject = GetPoseCreate();
                                    if (baseObject != null && IsProperTarget(baseObject) && AntiMagic > 0)
                                    {
                                        nPower = baseObject.GetMagStruckDamage(this, nPower);
                                        if (nPower > 0)
                                        {
                                            baseObject.StruckDamage(nPower);
                                            if ((HUtil32.GetTickCount() - SpellTick) > NextHitTime)
                                            {
                                                SpellTick = HUtil32.GetTickCount();
                                                SendRefMsg(Messages.RM_SPELL, 48, TargetCret.CurrX, TargetCret.CurrY, 48, "");
                                                SendRefMsg(Messages.RM_MAGICFIRE, 0, HUtil32.MakeWord(2, 48), HUtil32.MakeLong(TargetCret.CurrX, TargetCret.CurrY), TargetCret.ActorId, "");
                                                SendStruckDelayMsg(Messages.RM_DELAYMAGIC, nPower, HUtil32.MakeLong(TargetCret.CurrX, TargetCret.CurrY), 2, TargetCret.ActorId, "", 600);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    BreakHolySeizeMode();
                }
                else
                {
                    TargetCret = null;
                }
                if ((HUtil32.GetTickCount() - SearchEnemyTick) > 8000 || (HUtil32.GetTickCount() - SearchEnemyTick) > 1000 && TargetCret == null)
                {
                    SearchEnemyTick = HUtil32.GetTickCount();
                    SearchTarget();
                }
            }
            base.Run();
        }
    }
}