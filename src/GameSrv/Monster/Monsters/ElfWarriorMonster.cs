using GameSrv.Actor;
using SystemModule.Enums;

namespace GameSrv.Monster.Monsters {
    /// <summary>
    /// 神兽攻击形态
    /// </summary>
    public class ElfWarriorMonster : SpitSpider {
        public bool BoIsFirst;
        private int DigDownTick;

        public void AppearNow() {
            BoIsFirst = false;
            FixedHideMode = false;
            SendRefMsg(Messages.RM_DIGUP, Dir, CurrX, CurrY, 0, "");
            RecalcAbilitys();
            WalkTick = WalkTick + 800;
            DigDownTick = HUtil32.GetTickCount();
            Race = ActorRace.ElfWarriormon;
        }

        public ElfWarriorMonster()
            : base() {
            ViewRange = 6;
            FixedHideMode = true;
            BoIsFirst = true;
            UsePoison = false;
        }

        public override void RecalcAbilitys() {
            base.RecalcAbilitys();
            ResetElfMon();
        }

        private void SpitAttack(byte dir)
        {
            Dir = dir;
            var wAbil = WAbil;
            var nDamage = (GameShare.RandomNumber.Random(Math.Abs(HUtil32.HiWord(wAbil.DC) - HUtil32.LoWord(wAbil.DC)) + 1) + HUtil32.LoWord(wAbil.DC));
            if (nDamage <= 0)
            {
                return;
            }
            SendRefMsg(Messages.RM_HIT, Dir, CurrX, CurrY, 0, "");
            nDamage = 1;
            short nX = 0;
            short nY = 0;
            while (true)
            {
                if (Envir.GetNextPosition(CurrX, CurrY, Dir, nDamage, ref nX, ref nY))
                {
                    var baseObject = Envir.GetMovingObject(nX, nY, true);
                    if (baseObject != null && baseObject != this && IsProperTarget(baseObject) && (GameShare.RandomNumber.Random(baseObject.SpeedPoint) < HitPoint))
                    {
                        nDamage = baseObject.GetMagStruckDamage(this, nDamage);
                        if (nDamage > 0)
                        {
                            baseObject.StruckDamage(nDamage);
                            baseObject.SendStruckDelayMsg(Messages.RM_STRUCKEFFECT, nDamage, WAbil.HP, WAbil.MaxHP, this.ActorId, "", 300);
                        }
                    }
                }
            }
        }

        protected override bool AttackTarget()
        {
            if (TargetCret == null)
            {
                return false;
            }
            if (Appr != 704 && Appr != 706 && Appr != 708) //不是强化神兽则用普通攻击
            {
                return base.AttackTarget();
            }
            byte btDir = 0;
            if (TargetInSpitRange(TargetCret, ref btDir) || GetLongAttackDirDis(TargetCret, 3, ref btDir))
            {
                if (HUtil32.GetTickCount() - AttackTick > NextHitTime)
                {
                    AttackTick = HUtil32.GetTickCount();
                    TargetFocusTick = HUtil32.GetTickCount();
                    SpitAttack(btDir);
                    BreakHolySeizeMode();
                }
                return true;
            }
            if (TargetCret.Envir == Envir)
            {
                SetTargetXy(TargetCret.CurrX, TargetCret.CurrY);
            }
            else
            {
                DelTargetCreat();
            }
            return false;
        }

        private void ResetElfMon() {
            NextHitTime = 1500 - SlaveMakeLevel * 100;
            WalkSpeed = 500 - SlaveMakeLevel * 50;
            WalkTick = HUtil32.GetTickCount() + 2000;
        }

        public override void Run() {
            if (BoIsFirst) {
                BoIsFirst = false;
                FixedHideMode = false;
                SendRefMsg(Messages.RM_DIGUP, Dir, CurrX, CurrY, 0, "");
                ResetElfMon();
            }
            if (Death) {
                if ((HUtil32.GetTickCount() - DeathTick) > (2 * 1000)) {
                    MakeGhost();
                }
            }
            else {
                bool boChangeFace = TargetCret == null;
                if (Master != null && (Master.TargetCret != null || Master.LastHiter != null)) {
                    boChangeFace = false;
                }
                if (boChangeFace) {
                    if ((HUtil32.GetTickCount() - DigDownTick) > (6 * 10 * 1000)) {
                        BaseObject elfMon = null;
                        string elfName = ChrName;
                        if (elfName[^1] == '1') {
                            elfName = elfName[..^1];
                            elfMon = MakeClone(elfName, this);
                        }
                        if (elfMon != null) {
                            SendRefMsg(Messages.RM_DIGDOWN, Dir, CurrX, CurrY, 0, "");
                            SendRefMsg(Messages.RM_CHANGEFACE, 0, ActorId, elfMon.ActorId, 0, "");
                            elfMon.AutoChangeColor = AutoChangeColor;
                            if (elfMon is ElfMonster monster) {
                                monster.AppearNow();
                            }
                            Master = null;
                            KickException();
                        }
                    }
                }
                else {
                    DigDownTick = HUtil32.GetTickCount();
                }
            }
            base.Run();
        }
    }
}