using GameSvr.Actor;
using GameSvr.Event;
using SystemModule;

namespace GameSvr.Monster.Monsters
{
    public class ScultureKingMonster : MonsterObject
    {
        private int m_nDangerLevel = 0;
        private readonly IList<TBaseObject> m_SlaveObjectList = null;

        public ScultureKingMonster() : base()
        {
            SearchTime = M2Share.RandomNumber.Random(1500) + 1500;
            ViewRange = 8;
            StoneMode = true;
            m_nCharStatusEx = Grobal2.STATE_STONE_MODE;
            Direction = 5;
            m_nDangerLevel = 5;
            m_SlaveObjectList = new List<TBaseObject>();
        }

        private void MeltStone()
        {
            m_nCharStatusEx = 0;
            CharStatus = GetCharStatus();
            SendRefMsg(Grobal2.RM_DIGUP, Direction, CurrX, CurrY, 0, "");
            StoneMode = false;
            var stoneEvent = new MirEvent(Envir, CurrX, CurrY, 6, 5 * 60 * 1000, true);
            M2Share.EventManager.AddEvent(stoneEvent);
        }

        private void CallSlave()
        {
            short nX = 0;
            short nY = 0;
            var nCount = M2Share.RandomNumber.Random(6) + 6;
            GetFrontPosition(ref nX, ref nY);
            for (var i = 0; i < nCount; i++)
            {
                if (m_SlaveObjectList.Count >= 30)
                {
                    break;
                }
                var baseObject = M2Share.UserEngine.RegenMonsterByName(MapName, nX, nY, M2Share.g_Config.sZuma[M2Share.RandomNumber.Random(4)]);
                if (baseObject != null)
                {
                    m_SlaveObjectList.Add(baseObject);
                }
            }
        }

        public override void Attack(TBaseObject TargeTBaseObject, byte nDir)
        {
            int nPower = GetAttackPower(HUtil32.LoWord(m_WAbil.DC), HUtil32.HiWord(m_WAbil.DC) - HUtil32.LoWord(m_WAbil.DC));
            HitMagAttackTarget(TargeTBaseObject, 0, nPower, true);
        }

        public override void Run()
        {
            TBaseObject BaseObject;
            if (!Ghost && !Death && m_wStatusTimeArr[Grobal2.POISON_STONE] == 0 && (HUtil32.GetTickCount() - WalkTick) >= WalkSpeed)
            {
                if (StoneMode)
                {
                    for (var i = 0; i < VisibleActors.Count; i++)
                    {
                        BaseObject = VisibleActors[i].BaseObject;
                        if (BaseObject.Death)
                        {
                            continue;
                        }
                        if (IsProperTarget(BaseObject))
                        {
                            if (!BaseObject.HideMode || CoolEye)
                            {
                                if (Math.Abs(CurrX - BaseObject.CurrX) <= 2 && Math.Abs(CurrY - BaseObject.CurrY) <= 2)
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

