using SystemModule;
using SystemModule.Consts;
using SystemModule.Events;

namespace M2Server.Monster.Monsters
{
    public class ScultureKingMonster : MonsterObject
    {
        private int _mNDangerLevel;
        private readonly IList<IActor> _mSlaveObjectList;

        public ScultureKingMonster() : base()
        {
            SearchTime = M2Share.RandomNumber.Random(1500) + 1500;
            ViewRange = 8;
            StoneMode = true;
            CharStatusEx = PoisonState.STONEMODE;
            Dir = 5;
            _mNDangerLevel = 5;
            _mSlaveObjectList = new List<IActor>();
        }

        private void MeltStone()
        {
            CharStatusEx = 0;
            CharStatus = GetCharStatus();
            SendRefMsg(Messages.RM_DIGUP, Dir, CurrX, CurrY, 0, "");
            StoneMode = false;
            MapEvent stoneEvent = new MapEvent(Envir, CurrX, CurrY, 6, 5 * 60 * 1000, true);
            M2Share.EventMgr.AddEvent(stoneEvent);
        }

        private void CallSlave()
        {
            short nX = 0;
            short nY = 0;
            int nCount = M2Share.RandomNumber.Random(6) + 6;
            GetFrontPosition(ref nX, ref nY);
            for (int i = 0; i < nCount; i++)
            {
                if (_mSlaveObjectList.Count >= 30)
                {
                    break;
                }
                IActor baseObject = M2Share.WorldEngine.RegenMonsterByName(MapName, nX, nY, SystemShare.Config.Zuma[M2Share.RandomNumber.Random(4)]);
                if (baseObject != null)
                {
                    _mSlaveObjectList.Add(baseObject);
                }
            }
        }

        protected override void Attack(IActor targetBaseObject, byte nDir)
        {
            var nPower = GetAttackPower(HUtil32.LoByte(WAbil.DC), Math.Abs(HUtil32.HiByte(WAbil.DC) - HUtil32.LoByte(WAbil.DC)));
            HitMagAttackTarget(targetBaseObject, 0, nPower, true);
        }

        public override void Run()
        {
            if (CanMove() && (HUtil32.GetTickCount() - WalkTick) >= WalkSpeed)
            {
                IActor baseObject;
                if (StoneMode)
                {
                    for (int i = 0; i < VisibleActors.Count; i++)
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
                        if (_mNDangerLevel > WAbil.HP / WAbil.MaxHP * 5 && _mNDangerLevel > 0)
                        {
                            _mNDangerLevel -= 1;
                            CallSlave();
                        }
                        if (WAbil.HP == WAbil.MaxHP)
                        {
                            _mNDangerLevel = 5;
                        }
                    }
                }
                for (int i = _mSlaveObjectList.Count - 1; i >= 0; i--)
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

