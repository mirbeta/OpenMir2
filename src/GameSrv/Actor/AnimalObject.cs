using GameSrv.Maps;
using GameSrv.Player;
using SystemModule.Consts;
using SystemModule.Data;
using SystemModule.Enums;

namespace GameSrv.Actor {
    public class AnimalObject : BaseObject {
        internal short TargetX;
        internal short TargetY;
        public bool RunAwayMode;
        public int RunAwayStart;
        public int RunAwayTime;
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

        public AnimalObject() {
            TargetX = -1;
            Race = ActorRace.Animal;
            AttackTick = HUtil32.GetTickCount() - M2Share.RandomNumber.Random(3000);
            WalkTick = HUtil32.GetTickCount() - M2Share.RandomNumber.Random(3000);
            SearchEnemyTick = HUtil32.GetTickCount();
            RunAwayMode = false;
            RunAwayStart = HUtil32.GetTickCount();
            RunAwayTime = 0;
            WalkCount = 0;
            WalkWaitTick = HUtil32.GetTickCount();
            WalkWaitLocked = false;
            ProcessRunCount = 0;
            CellType = CellType.Monster;
        }

        /// <summary>
        /// 是否可以走动
        /// </summary>
        /// <returns></returns>
        protected bool CanMove() {
            return !Ghost && !Death && StatusTimeArr[PoisonState.STONE] == 0;
        }

        protected virtual void Attack(BaseObject targetObject, byte nDir) {
            var nPower = GetBaseAttackPoewr();
            AttackDir(targetObject, nPower, nDir);
            SendAttackMsg(Messages.RM_HIT, Direction, CurrX, CurrY);
        }

        protected void GotoTargetXy() {
            if (CurrX != TargetX || CurrY != TargetY) {
                int n10 = TargetX;
                int n14 = TargetY;
                var nDir = SystemModule.Enums.Direction.Down;
                if (n10 > CurrX) {
                    nDir = SystemModule.Enums.Direction.Right;
                    if (n14 > CurrY) {
                        nDir = SystemModule.Enums.Direction.DownRight;
                    }
                    if (n14 < CurrY) {
                        nDir = SystemModule.Enums.Direction.UpRight;
                    }
                }
                else {
                    if (n10 < CurrX) {
                        nDir = SystemModule.Enums.Direction.Left;
                        if (n14 > CurrY) {
                            nDir = SystemModule.Enums.Direction.DownLeft;
                        }
                        if (n14 < CurrY) {
                            nDir = SystemModule.Enums.Direction.UpLeft;
                        }
                    }
                    else {
                        if (n14 > CurrY) {
                            nDir = SystemModule.Enums.Direction.Down;
                        }
                        else if (n14 < CurrY) {
                            nDir = SystemModule.Enums.Direction.Up;
                        }
                    }
                }
                int nOldX = CurrX;
                int nOldY = CurrY;
                WalkTo(nDir, false);
                var n20 = M2Share.RandomNumber.Random(3);
                for (var i = SystemModule.Enums.Direction.Up; i <= SystemModule.Enums.Direction.UpLeft; i++) {
                    if (nOldX == CurrX && nOldY == CurrY) {
                        if (n20 != 0) {
                            nDir++;
                        }
                        else if (nDir > 0) {
                            nDir -= 1;
                        }
                        else {
                            nDir = SystemModule.Enums.Direction.UpLeft;
                        }
                        if (nDir > SystemModule.Enums.Direction.UpLeft) {
                            nDir = SystemModule.Enums.Direction.Up;
                        }
                        WalkTo(nDir, false);
                    }
                }
            }
        }

        public override void Initialize() {
            base.Initialize();
            LoadSayMsg();
            MonsterSayMsg(null, MonStatus.MonGen);
        }
        
        /// <summary>
        /// 取怪物说话信息列表
        /// </summary>
        internal void LoadSayMsg()
        {
            for (int i = 0; i < M2Share.MonSayMsgList.Count; i++)
            {
                if (M2Share.MonSayMsgList.TryGetValue(ChrName, out SayMsgList))
                {
                    break;
                }
            }
        }

        protected override bool Operate(ProcessMessage processMsg) {
            if (processMsg.wIdent == Messages.RM_STRUCK) {
                var struckObject = M2Share.ActorMgr.Get(processMsg.nParam3);
                if (processMsg.ActorId == ActorId && struckObject != null) {
                    SetLastHiter(struckObject);
                    Struck(struckObject);
                    BreakHolySeizeMode();
                    if (Master != null && struckObject != Master && struckObject.Race == ActorRace.Play) {
                        ((PlayObject)Master).SetPkFlag(struckObject);
                    }
                    MonsterSayMsg(struckObject, MonStatus.UnderFire);
                }
                return true;
            }
            return base.Operate(processMsg);
        }

        public virtual void Struck(BaseObject hiter) {
            byte btDir = 0;
            StruckTick = HUtil32.GetTickCount();
            if (hiter != null) {
                if (TargetCret == null || GetAttackDir(TargetCret, ref btDir) || M2Share.RandomNumber.Random(6) == 0) {
                    if (IsProperTarget(hiter)) {
                        SetTargetCreat(hiter);
                    }
                }
            }
            if (Animal) {
                MeatQuality = (ushort)(MeatQuality - M2Share.RandomNumber.Random(300));
                if (MeatQuality < 0) {
                    MeatQuality = 0;
                }
            }
            AttackTick = AttackTick + (150 - HUtil32._MIN(130, Abil.Level * 4));
        }

        protected void HitMagAttackTarget(BaseObject targetBaseObject, int nHitPower, int nMagPower, bool boFlag) {
            IList<BaseObject> baseObjectList = new List<BaseObject>();
            Direction = M2Share.GetNextDirection(CurrX, CurrY, targetBaseObject.CurrX, targetBaseObject.CurrY);
            Envir.GetBaseObjects(targetBaseObject.CurrX, targetBaseObject.CurrY, false, baseObjectList);
            for (var i = 0; i < baseObjectList.Count; i++) {
                var baseObject = baseObjectList[i];
                if (IsProperTarget(baseObject)) {
                    int nDamage = 0;
                    nDamage += baseObject.GetHitStruckDamage(this, nHitPower);
                    nDamage += baseObject.GetMagStruckDamage(this, nMagPower);
                    if (nDamage > 0) {
                        baseObject.StruckDamage(nDamage);
                        baseObject.SendStruckDelayMsg(Messages.RM_STRUCK, Messages.RM_REFMESSAGE, nDamage, baseObject.WAbil.HP, baseObject.WAbil.MaxHP, ActorId, "", 200);
                    }
                }
            }
            baseObjectList.Clear();
            SendRefMsg(Messages.RM_HIT, Direction, CurrX, CurrY, 0, "");
        }

        protected override void DelTargetCreat() {
            base.DelTargetCreat();
            TargetX = -1;
            TargetY = -1;
        }

        /// <summary>
        /// 搜索目标
        /// </summary>
        protected virtual void SearchTarget() {
            BaseObject searchTarget = null;
            var n10 = 999;
            for (var i = 0; i < VisibleActors.Count; i++) {
                var baseObject = VisibleActors[i].BaseObject;
                if (baseObject.Death || baseObject.Ghost || (baseObject.Envir != Envir) || (Math.Abs(baseObject.CurrX - CurrX) > 15) || (Math.Abs(baseObject.CurrY - CurrY) > 15)) {
                    ClearTargetCreat(baseObject);
                    continue;
                }
                if (!baseObject.Death) {
                    if (IsProperTarget(baseObject) && (!baseObject.HideMode || CoolEye)) {
                        var nC = Math.Abs(CurrX - baseObject.CurrX) + Math.Abs(CurrY - baseObject.CurrY);
                        if (nC < n10) {
                            n10 = nC;
                            searchTarget = baseObject;
                        }
                    }
                }
            }
            if (searchTarget != null) {
                SetTargetCreat(searchTarget);
            }
        }

        protected virtual void SetTargetXy(short nX, short nY) {
            TargetX = nX;
            TargetY = nY;
        }

        protected virtual void Wondering() {
            if (M2Share.RandomNumber.Random(20) != 0) return;
            if (M2Share.RandomNumber.Random(4) == 1) {
                TurnTo(M2Share.RandomNumber.RandomByte(8));
            }
            else {
                WalkTo(Direction, false);
            }
        }
    }
}

