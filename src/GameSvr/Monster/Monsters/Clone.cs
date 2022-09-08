using GameSvr.Actor;
using SystemModule;
using SystemModule.Data;
using SystemModule.Packet.ClientPackets;

namespace GameSvr.Monster.Monsters
{
    public class CloneMonster : MonsterObject
    {
        public CloneMonster() : base()
        {
            SearchTime = M2Share.RandomNumber.Random(1500) + 1500;
        }

        protected override bool Operate(ProcessMessage processMsg)
        {
            var result = false;
            if (processMsg.wIdent == Grobal2.RM_STRUCK || processMsg.wIdent == Grobal2.RM_MAGSTRUCK || processMsg.wIdent == Grobal2.RM_SPELL)
            {
                if (Master != null)
                {
                    if (Master.Abil.MP <= 0)
                    {
                        Abil.HP = 0;
                    }
                    if (processMsg.wIdent == Grobal2.RM_SPELL)
                    {
                        Master.Abil.MP -= (ushort)processMsg.nParam3;
                    }
                    else
                    {
                        Master.Abil.MP -= (ushort)processMsg.wParam;
                    }
                }
            }
            return result;
        }

        private void LightingAttack(byte nDir)
        {
            short nSx = 0;
            short nSy = 0;
            short nTx = 0;
            short nTy = 0;
            Direction = nDir;
            SendRefMsg(Grobal2.RM_LIGHTING, 1, CurrX, CurrY, TargetCret.ObjectId, "");
            if (Envir.GetNextPosition(CurrX, CurrY, nDir, 1, ref nSx, ref nSy))
            {
                Envir.GetNextPosition(CurrX, CurrY, nDir, 9, ref nTx, ref nTy);
                var wAbil = Abil;
                var nPwr = M2Share.RandomNumber.Random(HUtil32.HiWord(wAbil.DC) - HUtil32.LoWord(wAbil.DC) + 1) + HUtil32.LoWord(wAbil.DC);
                MagPassThroughMagic(nSx, nSy, nTx, nTy, nDir, nPwr, true);
            }
            BreakHolySeizeMode();
        }

        public override void Struck(BaseObject hiter)
        {
            if (hiter == null)
            {
                return;
            }
        }

        public override void Run()
        {
            if (!Death && !Ghost && StatusTimeArr[Grobal2.POISON_STONE] == 0 && HUtil32.GetTickCount() - SearchEnemyTick > 8000)
            {
                if ((HUtil32.GetTickCount() - SearchEnemyTick) > 1000 && TargetCret == null)
                {
                    SearchEnemyTick = HUtil32.GetTickCount();
                    SearchTarget();
                }
                if ((HUtil32.GetTickCount() - WalkTick) > WalkSpeed && TargetCret != null && Math.Abs(CurrX - TargetCret.CurrX) <= 4 && Math.Abs(CurrY - TargetCret.CurrY) <= 4)
                {
                    if (Math.Abs(CurrX - TargetCret.CurrX) <= 2 && Math.Abs(CurrY - TargetCret.CurrY) <= 2 && M2Share.RandomNumber.Random(3) != 0)
                    {
                        base.Run();
                        return;
                    }
                    GetBackPosition(ref TargetX, ref TargetY);
                }
                if (TargetCret != null && Math.Abs(CurrX - TargetCret.CurrX) < 6 && Math.Abs(CurrY - TargetCret.CurrY) < 6 && (HUtil32.GetTickCount() - AttackTick) > NextHitTime)
                {
                    AttackTick = HUtil32.GetTickCount();
                    byte nAttackDir = M2Share.GetNextDirection(CurrX, CurrY, TargetCret.CurrX, TargetCret.CurrY);
                    LightingAttack(nAttackDir);
                }
            }
            base.Run();
        }
    }
}

