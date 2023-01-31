using GameSvr.Maps;
using GameSvr.Player;
using SystemModule.Consts;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSvr.Actor
{
    public class AnimalObject : BaseObject
    {
        /// <summary>
        /// 未被处理次数，用于怪物处理循环
        /// </summary>
        private readonly int NotProcessCount;
        internal short TargetX;
        internal short TargetY;
        public bool MBoRunAwayMode;
        public int MDwRunAwayStart;
        public int MDwRunAwayTime;
        protected int SearchEnemyTick = 0;
        /// <summary>
        /// 行走步伐
        /// </summary>
        public int WalkStep = 0;
        public int WalkWait = 0;
        protected int WalkCount;
        protected int WalkWaitTick;
        /// <summary>
        /// 步行等待锁定
        /// </summary>
        protected bool WalkWaitLocked;
        /// <summary>
        /// 当前处理数量
        /// </summary>
        public int ProcessRunCount;

        public AnimalObject()
        {
            NotProcessCount = 0;
            TargetX = -1;
            Race = ActorRace.Animal;
            AttackTick = HUtil32.GetTickCount() - M2Share.RandomNumber.Random(3000);
            WalkTick = HUtil32.GetTickCount() - M2Share.RandomNumber.Random(3000);
            SearchEnemyTick = HUtil32.GetTickCount();
            MBoRunAwayMode = false;
            MDwRunAwayStart = HUtil32.GetTickCount();
            MDwRunAwayTime = 0;
            WalkCount = 0;
            WalkWaitTick = HUtil32.GetTickCount();
            WalkWaitLocked = false;
            ProcessRunCount = 0;
            MapCell = CellType.Monster;
        }

        /// <summary>
        /// 是否可以走动
        /// </summary>
        /// <returns></returns>
        protected bool CanMove()
        {
            return !Ghost && !Death && StatusArr[PoisonState.STONE] == 0;
        }

        protected virtual void Attack(BaseObject targetObject, byte nDir)
        {
            var nPower = GetBaseAttackPoewr();
            AttackDir(targetObject, nPower, nDir);
            SendAttackMsg(Grobal2.RM_HIT, Direction, CurrX, CurrY);
        }

        protected void GotoTargetXy()
        {
            if (CurrX != TargetX || CurrY != TargetY)
            {
                int n10 = TargetX;
                int n14 = TargetY;
                byte nDir = Grobal2.DR_DOWN;
                if (n10 > CurrX)
                {
                    nDir = Grobal2.DR_RIGHT;
                    if (n14 > CurrY)
                    {
                        nDir = Grobal2.DR_DOWNRIGHT;
                    }
                    if (n14 < CurrY)
                    {
                        nDir = Grobal2.DR_UPRIGHT;
                    }
                }
                else
                {
                    if (n10 < CurrX)
                    {
                        nDir = Grobal2.DR_LEFT;
                        if (n14 > CurrY)
                        {
                            nDir = Grobal2.DR_DOWNLEFT;
                        }
                        if (n14 < CurrY)
                        {
                            nDir = Grobal2.DR_UPLEFT;
                        }
                    }
                    else
                    {
                        if (n14 > CurrY)
                        {
                            nDir = Grobal2.DR_DOWN;
                        }
                        else if (n14 < CurrY)
                        {
                            nDir = Grobal2.DR_UP;
                        }
                    }
                }
                int nOldX = CurrX;
                int nOldY = CurrY;
                WalkTo(nDir, false);
                int n20 = M2Share.RandomNumber.Random(3);
                for (var i = Grobal2.DR_UP; i <= Grobal2.DR_UPLEFT; i++)
                {
                    if (nOldX == CurrX && nOldY == CurrY)
                    {
                        if (n20 != 0)
                        {
                            nDir++;
                        }
                        else if (nDir > 0)
                        {
                            nDir -= 1;
                        }
                        else
                        {
                            nDir = Grobal2.DR_UPLEFT;
                        }
                        if (nDir > Grobal2.DR_UPLEFT)
                        {
                            nDir = Grobal2.DR_UP;
                        }
                        WalkTo(nDir, false);
                    }
                }
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            LoadSayMsg();
            MonsterSayMsg(null, MonStatus.MonGen);
        }

        protected override bool Operate(ProcessMessage processMsg)
        {
            if (processMsg.wIdent == Grobal2.RM_STRUCK)
            {
                var struckObject = M2Share.ActorMgr.Get(processMsg.nParam3);
                if (processMsg.BaseObject == ActorId && struckObject != null)
                {
                    SetLastHiter(struckObject);
                    Struck(struckObject);
                    BreakHolySeizeMode();
                    if (Master != null && struckObject != Master && struckObject.Race == ActorRace.Play)
                    {
                        ((PlayObject)Master).SetPkFlag(struckObject);
                    }
                    MonsterSayMsg(struckObject, MonStatus.UnderFire);
                }
                return true;
            }
            return base.Operate(processMsg);
        }

        public virtual void Struck(BaseObject hiter)
        {
            byte btDir = 0;
            StruckTick = HUtil32.GetTickCount();
            if (hiter != null)
            {
                if (TargetCret == null || GetAttackDir(TargetCret, ref btDir) || M2Share.RandomNumber.Random(6) == 0)
                {
                    if (IsProperTarget(hiter))
                    {
                        SetTargetCreat(hiter);
                    }
                }
            }
            if (Animal)
            {
                MeatQuality = (ushort)(MeatQuality - M2Share.RandomNumber.Random(300));
                if (MeatQuality < 0)
                {
                    MeatQuality = 0;
                }
            }
            AttackTick = AttackTick + (150 - HUtil32._MIN(130, Abil.Level * 4));
        }

        protected void HitMagAttackTarget(BaseObject targetBaseObject, int nHitPower, int nMagPower, bool boFlag)
        {
            IList<BaseObject> baseObjectList = new List<BaseObject>();
            Direction = M2Share.GetNextDirection(CurrX, CurrY, targetBaseObject.CurrX, targetBaseObject.CurrY);
            Envir.GetBaseObjects(targetBaseObject.CurrX, targetBaseObject.CurrY, false, baseObjectList);
            for (var i = 0; i < baseObjectList.Count; i++)
            {
                var baseObject = baseObjectList[i];
                if (IsProperTarget(baseObject))
                {
                    var nDamage = 0;
                    nDamage += baseObject.GetHitStruckDamage(this, nHitPower);
                    nDamage += baseObject.GetMagStruckDamage(this, (ushort)nMagPower);
                    if (nDamage > 0)
                    {
                        baseObject.StruckDamage((ushort)nDamage);
                        baseObject.SendDelayMsg(Grobal2.RM_STRUCK, Grobal2.RM_REFMESSAGE, nDamage, baseObject.WAbil.HP, baseObject.WAbil.MaxHP, ActorId, "", 200);
                    }
                }
            }
            baseObjectList.Clear();
            SendRefMsg(Grobal2.RM_HIT, Direction, CurrX, CurrY, 0, "");
        }

        protected override void DelTargetCreat()
        {
            base.DelTargetCreat();
            TargetX = -1;
            TargetY = -1;
        }

        /// <summary>
        /// 搜索目标
        /// </summary>
        protected virtual void SearchTarget()
        {
            BaseObject searchTarget = null;
            var n10 = 999;
            for (var i = 0; i < VisibleActors.Count; i++)
            {
                var baseObject = VisibleActors[i].BaseObject;
                if (baseObject.Death || baseObject.Ghost || (baseObject.Envir != Envir) || (Math.Abs(baseObject.CurrX - CurrX) > 15) || (Math.Abs(baseObject.CurrY - CurrY) > 15))
                {
                    ClearTargetCreat(baseObject);
                    continue;
                }
                if (!baseObject.Death)
                {
                    if (IsProperTarget(baseObject) && (!baseObject.HideMode || CoolEye))
                    {
                        var nC = Math.Abs(CurrX - baseObject.CurrX) + Math.Abs(CurrY - baseObject.CurrY);
                        if (nC < n10)
                        {
                            n10 = nC;
                            searchTarget = baseObject;
                        }
                    }
                }
            }
            if (searchTarget != null)
            {
                SetTargetCreat(searchTarget);
            }
        }

        protected virtual void SetTargetXy(short nX, short nY)
        {
            TargetX = nX;
            TargetY = nY;
        }

        protected virtual void Wondering()
        {
            if (M2Share.RandomNumber.Random(20) != 0) return;
            if (M2Share.RandomNumber.Random(4) == 1)
            {
                TurnTo(M2Share.RandomNumber.RandomByte(8));
            }
            else
            {
                WalkTo(Direction, false);
            }
        }
    }
}

