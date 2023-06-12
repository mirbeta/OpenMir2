using SystemModule;
using SystemModule.Consts;
using SystemModule.Data;
using SystemModule.Enums;

namespace M2Server.Actor
{
    public class AnimalObject : BaseObject, IMonsterActor
    {
        /// <summary>
        /// 经验值
        /// </summary>
        public int FightExp { get; set; }
        internal short TargetX;
        internal short TargetY;
        public bool RunAwayMode;
        public int RunAwayStart;
        public int RunAwayTime;
        protected int SearchEnemyTick = 0;
        /// <summary>
        /// 行走步伐
        /// </summary>
        public int WalkStep { get; set; }
        public int WalkWait { get; set; }
        protected int WalkCount;
        protected int WalkWaitTick;
        /// <summary>
        /// 步行等待锁定
        /// </summary>
        protected bool WalkWaitLocked;
        /// <summary>
        /// 肉的品质
        /// </summary>
        public ushort MeatQuality;
        /// <summary>
        /// 当前处理数量
        /// </summary>
        public int ProcessRunCount { get; set; }
        /// <summary>
        /// 非攻击模式 F-可攻击 T-不攻击
        /// </summary>
        public bool NoAttackMode;
        /// <summary>
        /// 是否属于召唤怪物(宝宝)
        /// </summary>
        public bool IsSlave { get; set; }
        /// <summary>
        /// 说话信息列表
        /// </summary>
        public IList<MonsterSayMsg> SayMsgList;
        /// <summary>
        /// 不能走动模式(困魔咒)
        /// </summary>
        public bool HolySeize;
        /// <summary>
        /// 不能走动时间(困魔咒)
        /// </summary>
        private int HolySeizeTick;
        /// <summary>
        /// 不能走动时长(困魔咒)
        /// </summary>
        private int HolySeizeInterval;

        public AnimalObject()
        {
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

        public override void Initialize()
        {
            base.Initialize();
            LoadSayMsg();
            MonsterSayMessage(null, MonStatus.MonGen);
        }

        public override void Die()
        {
            base.Die();
            if (!SuperMan)
            {
                return;
            }
            if (LastHiter != null && Race != ActorRace.Play)
            {
                MonsterSayMessage(LastHiter, MonStatus.Die);
            }
            else if (LastHiter != null && Race == ActorRace.Play)
            {
                //LastHiter.MonsterSayMessage(this, MonStatus.KillHuman);
                MonsterSayMessage(LastHiter, MonStatus.KillHuman);
            }
        }

        protected override bool Operate(ProcessMessage processMsg)
        {
            switch (processMsg.wIdent)
            {
                case Messages.RM_STRUCK:
                    {
                        var struckObject = M2Share.ActorMgr.Get(processMsg.nParam3);
                        if (processMsg.ActorId == ActorId && struckObject != null)
                        {
                            SetLastHiter(struckObject);
                            Struck(struckObject);
                            BreakHolySeizeMode();
                            if (Master != null && struckObject != Master && struckObject.Race == ActorRace.Play)
                            {
                                ((IPlayerActor)Master).SetPkFlag(struckObject);
                            }
                            MonsterSayMessage(struckObject, MonStatus.UnderFire);
                        }
                        return true;
                    }
                case Messages.RM_MAKEHOLYSEIZEMODE:
                    OpenHolySeizeMode(processMsg.wParam);
                    return true;
                default:
                    return base.Operate(processMsg);
            }
        }

        public override void Run()
        {
            if (HolySeize && ((HUtil32.GetTickCount() - HolySeizeTick) > HolySeizeInterval))
            {
                BreakHolySeizeMode();
            }
            base.Run();
        }

        protected override void DelTargetCreat()
        {
            base.DelTargetCreat();
            TargetX = -1;
            TargetY = -1;
        }

        /// <summary>
        /// 是否可以走动
        /// </summary>
        /// <returns></returns>
        protected bool CanMove()
        {
            return !Ghost && !Death && StatusTimeArr[PoisonState.STONE] == 0;
        }

        protected virtual void Attack(IActor targetObject, byte nDir)
        {
            var nPower = GetBaseAttackPoewr();
            AttackDir(targetObject, nPower, nDir);
            SendAttackMsg(Messages.RM_HIT, Dir, CurrX, CurrY);
        }

        protected virtual void GotoTargetXY()
        {
            if (CurrX != TargetX || CurrY != TargetY)
            {
                int n10 = TargetX;
                int n14 = TargetY;
                var nDir = Direction.Down;
                if (n10 > CurrX)
                {
                    nDir = Direction.Right;
                    if (n14 > CurrY)
                    {
                        nDir = Direction.DownRight;
                    }
                    if (n14 < CurrY)
                    {
                        nDir = Direction.UpRight;
                    }
                }
                else
                {
                    if (n10 < CurrX)
                    {
                        nDir = Direction.Left;
                        if (n14 > CurrY)
                        {
                            nDir = Direction.DownLeft;
                        }
                        if (n14 < CurrY)
                        {
                            nDir = Direction.UpLeft;
                        }
                    }
                    else
                    {
                        if (n14 > CurrY)
                        {
                            nDir = Direction.Down;
                        }
                        else if (n14 < CurrY)
                        {
                            nDir = Direction.Up;
                        }
                    }
                }
                int nOldX = CurrX;
                int nOldY = CurrY;
                WalkTo(nDir, false);
                var n20 = M2Share.RandomNumber.Random(3);
                for (var i = Direction.Up; i <= Direction.UpLeft; i++)
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
                            nDir = Direction.UpLeft;
                        }
                        if (nDir > Direction.UpLeft)
                        {
                            nDir = Direction.Up;
                        }
                        WalkTo(nDir, false);
                    }
                }
            }
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

        /// <summary>
        /// 怪物说话
        /// </summary>
        internal void MonsterSayMessage(IActor monsterObject, MonStatus monStatus)
        {
            if (!SystemShare.Config.MonSayMsg)
            {
                return;
            }
            if (Race == ActorRace.Play)
            {
                return;
            }
            if (SayMsgList == null)
            {
                return;
            }
            if (monsterObject == null)
            {
                return;
            }
            string sAttackName;
            if ((monsterObject.Race != ActorRace.Play) && (monsterObject.Master == null))
            {
                return;
            }
            if (monsterObject.Master != null)
            {
                sAttackName = monsterObject.Master.ChrName;
            }
            else
            {
                sAttackName = monsterObject.ChrName;
            }
            for (int i = 0; i < SayMsgList.Count; i++)
            {
                MonsterSayMsg monSayMsg = SayMsgList[i];
                string sMsg = monSayMsg.sSayMsg.Replace("%s", M2Share.FilterShowName(ChrName));
                sMsg = sMsg.Replace("%d", sAttackName);
                if ((monSayMsg.State == monStatus) && (M2Share.RandomNumber.Random(monSayMsg.nRate) == 0))
                {
                    if (monStatus == MonStatus.MonGen)
                    {
                        M2Share.WorldEngine.SendBroadCastMsg(sMsg, MsgType.Mon);
                        break;
                    }
                    if (monSayMsg.Color == MsgColor.White)
                    {
                        ProcessSayMsg(sMsg);
                    }
                    else
                    {
                        //monsterObject.SysMsg(sMsg, monSayMsg.Color, MsgType.Mon);
                    }
                    break;
                }
            }
        }

        private void OpenHolySeizeMode(int dwInterval)
        {
            HolySeize = true;
            HolySeizeTick = HUtil32.GetTickCount();
            HolySeizeInterval = dwInterval;
            RefNameColor();
        }

        public void BreakHolySeizeMode()
        {
            HolySeize = false;
            RefNameColor();
        }

        public virtual void Struck(IActor hiter)
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

        protected void HitMagAttackTarget(IActor targetObject, int nHitPower, int nMagPower, bool boFlag)
        {
            IList<IActor> baseObjectList = new List<IActor>();
            Dir = M2Share.GetNextDirection(CurrX, CurrY, targetObject.CurrX, targetObject.CurrY);
            Envir.GetBaseObjects(targetObject.CurrX, targetObject.CurrY, false, ref baseObjectList);
            for (var i = 0; i < baseObjectList.Count; i++)
            {
                var baseObject = baseObjectList[i];
                if (IsProperTarget(baseObject))
                {
                    int nDamage = 0;
                    nDamage += baseObject.GetHitStruckDamage(this, nHitPower);
                    nDamage += baseObject.GetMagStruckDamage(this, nMagPower);
                    if (nDamage > 0)
                    {
                        baseObject.StruckDamage(nDamage);
                        baseObject.SendStruckDelayMsg(Messages.RM_REFMESSAGE, nDamage, baseObject.WAbil.HP, baseObject.WAbil.MaxHP, ActorId, "", 200);
                    }
                }
            }
            baseObjectList.Clear();
            SendRefMsg(Messages.RM_HIT, Dir, CurrX, CurrY, 0, "");
        }

        /// <summary>
        /// 搜索目标
        /// </summary>
        protected virtual void SearchTarget()
        {
            IActor searchTarget = null;
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
                WalkTo(Dir, false);
            }
        }
    }
}