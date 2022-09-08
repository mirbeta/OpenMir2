using GameSvr.Actor;
using SystemModule;

namespace GameSvr.Monster.Monsters
{
    public class BoneKingMonster : MonsterObject
    {
        private short _dangerLevel;
        private readonly IList<BaseObject> _slaveObjectList;

        public BoneKingMonster() : base()
        {
            SearchTime = M2Share.RandomNumber.Random(1500) + 1500;
            ViewRange = 8;
            Direction = 5;
            _dangerLevel = 5;
            _slaveObjectList = new List<BaseObject>();
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
                if (_slaveObjectList.Count >= 30)
                {
                    break;
                }
                var baseObject = M2Share.UserEngine.RegenMonsterByName(MapName, n10, n14, sMonName[M2Share.RandomNumber.Random(3)]);
                if (baseObject != null)
                {
                    _slaveObjectList.Add(baseObject);
                }
            }
        }

        public override void Attack(BaseObject targeTBaseObject, byte nDir)
        {
            var wAbil = Abil;
            var nPower = GetAttackPower(HUtil32.LoWord(wAbil.DC), HUtil32.HiWord(wAbil.DC) - HUtil32.LoWord(wAbil.DC));
            HitMagAttackTarget(targeTBaseObject, 0, nPower, true);
        }

        public override void Run()
        {
            if (!Ghost && !Death && StatusTimeArr[Grobal2.POISON_STONE] == 0 && HUtil32.GetTickCount() - WalkTick >= WalkSpeed)
            {
                if ((HUtil32.GetTickCount() - SearchEnemyTick) > 8000 || (HUtil32.GetTickCount() - SearchEnemyTick) > 1000 && TargetCret == null)
                {
                    SearchEnemyTick = HUtil32.GetTickCount();
                    SearchTarget();
                    if (_dangerLevel > Abil.HP / Abil.MaxHP * 5 && _dangerLevel > 0)
                    {
                        _dangerLevel -= 1;
                        CallSlave();
                    }
                    if (Abil.HP == Abil.MaxHP)
                    {
                        _dangerLevel = 5;
                    }
                }
                for (var i = _slaveObjectList.Count - 1; i >= 0; i--)
                {
                    var baseObject = _slaveObjectList[i];
                    if (baseObject.Death || baseObject.Ghost)
                    {
                        _slaveObjectList.RemoveAt(i);
                    }
                }
            }
            base.Run();
        }
    }
}

