using GameSvr.Actor;
using SystemModule;

namespace GameSvr.Monster.Monsters
{
    public class BoneKingMonster : MonsterObject
    {
        private short DangerLevel;
        private readonly IList<TBaseObject> SlaveObjectList;

        public BoneKingMonster() : base()
        {
            SearchTime = M2Share.RandomNumber.Random(1500) + 1500;
            ViewRange = 8;
            Direction = 5;
            DangerLevel = 5;
            SlaveObjectList = new List<TBaseObject>();
        }

        private void CallSlave()
        {
            string[] sMonName = { "BoneCaptain", "BoneArcher", "BoneSpearman" };
            short n10 = 0;
            short n14 = 0;
            var nC = M2Share.RandomNumber.Random(6) + 6;
            GetFrontPosition(ref n10, ref n14);
            for (var i = 0; i < nC; i++)
            {
                if (SlaveObjectList.Count >= 30)
                {
                    break;
                }
                var BaseObject = M2Share.UserEngine.RegenMonsterByName(MapName, n10, n14, sMonName[M2Share.RandomNumber.Random(3)]);
                if (BaseObject != null)
                {
                    SlaveObjectList.Add(BaseObject);
                }
            }
        }

        public override void Attack(TBaseObject TargeTBaseObject, byte nDir)
        {
            var WAbil = m_WAbil;
            var nPower = GetAttackPower(HUtil32.LoWord(WAbil.DC), HUtil32.HiWord(WAbil.DC) - HUtil32.LoWord(WAbil.DC));
            HitMagAttackTarget(TargeTBaseObject, 0, nPower, true);
        }

        public override void Run()
        {
            if (!Ghost && !Death && m_wStatusTimeArr[Grobal2.POISON_STONE] == 0 && HUtil32.GetTickCount() - WalkTick >= WalkSpeed)
            {
                if ((HUtil32.GetTickCount() - SearchEnemyTick) > 8000 || (HUtil32.GetTickCount() - SearchEnemyTick) > 1000 && TargetCret == null)
                {
                    SearchEnemyTick = HUtil32.GetTickCount();
                    SearchTarget();
                    if (DangerLevel > m_WAbil.HP / m_WAbil.MaxHP * 5 && DangerLevel > 0)
                    {
                        DangerLevel -= 1;
                        CallSlave();
                    }
                    if (m_WAbil.HP == m_WAbil.MaxHP)
                    {
                        DangerLevel = 5;
                    }
                }
                for (var i = SlaveObjectList.Count - 1; i >= 0; i--)
                {
                    var BaseObject = SlaveObjectList[i];
                    if (BaseObject.Death || BaseObject.Ghost)
                    {
                        SlaveObjectList.RemoveAt(i);
                    }
                }
            }
            base.Run();
        }
    }
}

