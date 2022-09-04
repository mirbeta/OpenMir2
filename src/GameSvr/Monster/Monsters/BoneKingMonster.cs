using GameSvr.Actor;
using SystemModule;

namespace GameSvr.Monster.Monsters
{
    public class BoneKingMonster : MonsterObject
    {
        private int m_nDangerLevel;
        private readonly IList<TBaseObject> m_SlaveObjectList;

        public BoneKingMonster() : base()
        {
            m_dwSearchTime = M2Share.RandomNumber.Random(1500) + 1500;
            ViewRange = 8;
            Direction = 5;
            m_nDangerLevel = 5;
            m_SlaveObjectList = new List<TBaseObject>();
        }

        private void CallSlave()
        {
            string[] sMonName = { "BoneCaptain", "BoneArcher", "BoneSpearman" };
            short n10 = 0;
            short n14 = 0;
            TBaseObject BaseObject;
            int nC = M2Share.RandomNumber.Random(6) + 6;
            GetFrontPosition(ref n10, ref n14);
            for (var i = 0; i < nC; i++)
            {
                if (m_SlaveObjectList.Count >= 30)
                {
                    break;
                }
                BaseObject = M2Share.UserEngine.RegenMonsterByName(MapName, n10, n14, sMonName[M2Share.RandomNumber.Random(3)]);
                if (BaseObject != null)
                {
                    m_SlaveObjectList.Add(BaseObject);
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
            TBaseObject BaseObject;
            if (!Ghost && !Death && m_wStatusTimeArr[Grobal2.POISON_STONE] == 0 && HUtil32.GetTickCount() - WalkTick >= WalkSpeed)
            {
                if ((HUtil32.GetTickCount() - SearchEnemyTick) > 8000 || (HUtil32.GetTickCount() - SearchEnemyTick) > 1000 && TargetCret == null)
                {
                    SearchEnemyTick = HUtil32.GetTickCount();
                    SearchTarget();
                    if (m_nDangerLevel > m_WAbil.HP / m_WAbil.MaxHP * 5 && m_nDangerLevel > 0)
                    {
                        m_nDangerLevel -= 1;
                        CallSlave();
                    }
                    if (m_WAbil.HP == m_WAbil.MaxHP)
                    {
                        m_nDangerLevel = 5;
                    }
                }
                for (var i = m_SlaveObjectList.Count - 1; i >= 0; i--)
                {
                    BaseObject = m_SlaveObjectList[i];
                    if (BaseObject.Death || BaseObject.Ghost)
                    {
                        m_SlaveObjectList.RemoveAt(i);
                    }
                }
            }
            base.Run();
        }
    }
}

