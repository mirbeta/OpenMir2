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

        protected override bool Operate(ProcessMessage ProcessMsg)
        {
            var result = false;
            if (ProcessMsg.wIdent == Grobal2.RM_STRUCK || ProcessMsg.wIdent == Grobal2.RM_MAGSTRUCK || ProcessMsg.wIdent == Grobal2.RM_SPELL)
            {
                if (Master != null)
                {
                    if (Master.Abil.MP <= 0)
                    {
                        Abil.HP = 0;
                    }
                    if (ProcessMsg.wIdent == Grobal2.RM_SPELL)
                    {
                        Master.Abil.MP -= (ushort)ProcessMsg.nParam3;
                    }
                    else
                    {
                        Master.Abil.MP -= (ushort)ProcessMsg.wParam;
                    }
                }
            }
            return result;
        }

        private void LightingAttack(byte nDir)
        {
            short nSX = 0;
            short nSY = 0;
            short nTX = 0;
            short nTY = 0;
            Direction = nDir;
            SendRefMsg(Grobal2.RM_LIGHTING, 1, CurrX, CurrY, TargetCret.ObjectId, "");
            if (Envir.GetNextPosition(CurrX, CurrY, nDir, 1, ref nSX, ref nSY))
            {
                Envir.GetNextPosition(CurrX, CurrY, nDir, 9, ref nTX, ref nTY);
                var WAbil = Abil;
                var nPwr = M2Share.RandomNumber.Random(HUtil32.HiWord(WAbil.DC) - HUtil32.LoWord(WAbil.DC) + 1) + HUtil32.LoWord(WAbil.DC);
                MagPassThroughMagic(nSX, nSY, nTX, nTY, nDir, nPwr, true);
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

