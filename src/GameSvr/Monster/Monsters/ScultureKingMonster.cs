using GameSvr.Actor;
using GameSvr.Event;
using SystemModule;

namespace GameSvr.Monster.Monsters
{
    public class ScultureKingMonster : MonsterObject
    {
        private int _mNDangerLevel;
        private readonly IList<BaseObject> _mSlaveObjectList;

        public ScultureKingMonster() : base()
        {
            SearchTime = M2Share.RandomNumber.Random(1500) + 1500;
            ViewRange = 8;
            StoneMode = true;
            CharStatusEx = Grobal2.STATE_STONE_MODE;
            Direction = 5;
            _mNDangerLevel = 5;
            _mSlaveObjectList = new List<BaseObject>();
        }

        private void MeltStone()
        {
            CharStatusEx = 0;
            CharStatus = GetCharStatus();
            SendRefMsg(Grobal2.RM_DIGUP, Direction, CurrX, CurrY, 0, "");
            StoneMode = false;
            var stoneEvent = new MirEvent(Envir, CurrX, CurrY, 6, 5 * 60 * 1000, true);
            M2Share.EventMgr.AddEvent(stoneEvent);
        }

        private void CallSlave()
        {
            short nX = 0;
            short nY = 0;
            var nCount = M2Share.RandomNumber.Random(6) + 6;
            GetFrontPosition(ref nX, ref nY);
            for (var i = 0; i < nCount; i++)
            {
                if (_mSlaveObjectList.Count >= 30)
                {
                    break;
                }
                var baseObject = M2Share.UserEngine.RegenMonsterByName(MapName, nX, nY, M2Share.Config.Zuma[M2Share.RandomNumber.Random(4)]);
                if (baseObject != null)
                {
                    _mSlaveObjectList.Add(baseObject);
                }
            }
        }

        public override void Attack(BaseObject targeTBaseObject, byte nDir)
        {
            int nPower = GetAttackPower(HUtil32.LoWord(Abil.DC), HUtil32.HiWord(Abil.DC) - HUtil32.LoWord(Abil.DC));
            HitMagAttackTarget(targeTBaseObject, 0, nPower, true);
        }

        public override void Run()
        {
            if (!Ghost && !Death && StatusTimeArr[Grobal2.POISON_STONE] == 0 && (HUtil32.GetTickCount() - WalkTick) >= WalkSpeed)
            {
                BaseObject baseObject;
                if (StoneMode)
                {
                    for (var i = 0; i < VisibleActors.Count; i++)
                    {
                        baseObject = VisibleActors[i].BaseObject;
                        if (baseObject.Death)
                        {
                            continue;
                        }
                        if (IsProperTarget(baseObject))
                        {
                            if (!baseObject.HideMode || CoolEye)
                            {
                                if (Math.Abs(CurrX - baseObject.CurrX) <= 2 && Math.Abs(CurrY - baseObject.CurrY) <= 2)
                                {
                                    MeltStone();
                                    break;
                                }
                            }
                        }
                    }
                }
                else
                {
                    if ((HUtil32.GetTickCount() - SearchEnemyTick) > 8000 || (HUtil32.GetTickCount() - SearchEnemyTick) > 1000 && TargetCret == null)
                    {
                        SearchEnemyTick = HUtil32.GetTickCount();
                        SearchTarget();
                        if (_mNDangerLevel > Abil.HP / Abil.MaxHP * 5 && _mNDangerLevel > 0)
                        {
                            _mNDangerLevel -= 1;
                            CallSlave();
                        }
                        if (Abil.HP == Abil.MaxHP)
                        {
                            _mNDangerLevel = 5;
                        }
                    }
                }
                for (var i = _mSlaveObjectList.Count - 1; i >= 0; i--)
                {
                    baseObject = _mSlaveObjectList[i];
                    if (baseObject.Death || baseObject.Ghost)
                    {
                        _mSlaveObjectList.RemoveAt(i);
                    }
                }
            }
            base.Run();
        }
    }
}

