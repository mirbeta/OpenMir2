using SystemModule;

namespace GameSvr.Monster.Monsters
{
    /// <summary>
    /// 神兽普通跟随形态
    /// </summary>
    public class ElfMonster : MonsterObject
    {
        public bool boIsFirst;

        public ElfMonster() : base()
        {
            ViewRange = 6;
            FixedHideMode = true;
            NoAttackMode = true;
            boIsFirst = true;
        }

        public void AppearNow()
        {
            FixedHideMode = false;
            RecalcAbilitys();
            WalkTick = WalkTick + 800;
        }

        public override void RecalcAbilitys()
        {
            base.RecalcAbilitys();
            ResetElfMon();
        }

        private void ResetElfMon()
        {
            WalkSpeed = 500 - SlaveMakeLevel * 50;
            WalkTick = HUtil32.GetTickCount() + 2000;
        }

        public override void Run()
        {
            bool boChangeFace = false;
            if (boIsFirst)
            {
                boIsFirst = false;
                FixedHideMode = false;
                SendRefMsg(Grobal2.RM_DIGUP, Direction, CurrX, CurrY, 0, "");
                ResetElfMon();
            }
            if (Death)
            {
                if ((HUtil32.GetTickCount() - DeathTick) > (2 * 1000))
                {
                    MakeGhost();
                }
            }
            else
            {
                if (TargetCret != null)
                {
                    boChangeFace = true;
                }
                if (Master != null && (Master.TargetCret != null || Master.LastHiter != null))
                {
                    boChangeFace = true;
                }
                if (boChangeFace)
                {
                    var ElfMon = MakeClone(M2Share.Config.Dragon1, this);
                    if (ElfMon != null)
                    {
                        ElfMon.AutoChangeColor = AutoChangeColor;
                        if (ElfMon is ElfWarriorMonster)
                        {
                            (ElfMon as ElfWarriorMonster).AppearNow();
                        }
                        Master = null;
                        KickException();
                    }
                }
            }
            base.Run();
        }
    }
}

