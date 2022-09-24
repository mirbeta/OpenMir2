using GameSvr.Maps;
using SystemModule;
using SystemModule.Consts;
using SystemModule.Data;

namespace GameSvr.Actor
{
    public class AnimalObject : BaseObject
    {
        /// <summary>
        /// 未被处理次数，用于怪物处理循环
        /// </summary>
        public int m_nNotProcessCount;
        internal short TargetX;
        internal short TargetY;
        public bool m_boRunAwayMode;
        public int m_dwRunAwayStart;
        public int m_dwRunAwayTime;

        public AnimalObject() : base()
        {
            m_nNotProcessCount = 0;
            TargetX = -1;
            this.Race = Grobal2.RC_ANIMAL;
            this.AttackTick = HUtil32.GetTickCount() - M2Share.RandomNumber.Random(3000);
            this.WalkTick = HUtil32.GetTickCount() - M2Share.RandomNumber.Random(3000);
            this.SearchEnemyTick = HUtil32.GetTickCount();
            m_boRunAwayMode = false;
            m_dwRunAwayStart = HUtil32.GetTickCount();
            m_dwRunAwayTime = 0;
            MapCell = CellType.Monster;
        }

        /// <summary>
        /// 是否可以走动
        /// </summary>
        /// <returns></returns>
        protected bool CanMove()
        {
            return !Ghost && !Death && StatusArr[StatuStateConst.POISON_STONE] == 0;
        }

        protected virtual void Attack(BaseObject TargeTBaseObject, byte nDir)
        {
            base.AttackDir(TargeTBaseObject, 0, nDir);
        }

        protected void GotoTargetXY()
        {
            if (this.CurrX != TargetX || this.CurrY != TargetY)
            {
                int n10 = TargetX;
                int n14 = TargetY;
                byte nDir = Grobal2.DR_DOWN;
                if (n10 > this.CurrX)
                {
                    nDir = Grobal2.DR_RIGHT;
                    if (n14 > this.CurrY)
                    {
                        nDir = Grobal2.DR_DOWNRIGHT;
                    }
                    if (n14 < this.CurrY)
                    {
                        nDir = Grobal2.DR_UPRIGHT;
                    }
                }
                else
                {
                    if (n10 < this.CurrX)
                    {
                        nDir = Grobal2.DR_LEFT;
                        if (n14 > this.CurrY)
                        {
                            nDir = Grobal2.DR_DOWNLEFT;
                        }
                        if (n14 < this.CurrY)
                        {
                            nDir = Grobal2.DR_UPLEFT;
                        }
                    }
                    else
                    {
                        if (n14 > this.CurrY)
                        {
                            nDir = Grobal2.DR_DOWN;
                        }
                        else if (n14 < this.CurrY)
                        {
                            nDir = Grobal2.DR_UP;
                        }
                    }
                }
                int nOldX = this.CurrX;
                int nOldY = this.CurrY;
                this.WalkTo(nDir, false);
                int n20 = M2Share.RandomNumber.Random(3);
                for (var i = Grobal2.DR_UP; i <= Grobal2.DR_UPLEFT; i++)
                {
                    if (nOldX == this.CurrX && nOldY == this.CurrY)
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
                        this.WalkTo(nDir, false);
                    }
                }
            }
        }

        protected override bool Operate(ProcessMessage ProcessMsg)
        {
            if (ProcessMsg.wIdent == Grobal2.RM_STRUCK)
            {
                var struckObject = M2Share.ActorMgr.Get(ProcessMsg.nParam3);
                if (ProcessMsg.BaseObject == this.ActorId && struckObject != null)
                {
                    this.SetLastHiter(struckObject);
                    Struck(struckObject);
                    this.BreakHolySeizeMode();
                    if (this.Master != null && struckObject != this.Master && struckObject.Race == Grobal2.RC_PLAYOBJECT)
                    {
                        this.Master.SetPkFlag(struckObject);
                    }
                    if (M2Share.Config.MonSayMsg)
                    {
                        this.MonsterSayMsg(struckObject, MonStatus.UnderFire);
                    }
                }
                return true;
            }
            return base.Operate(ProcessMsg);
        }

        public override void Run()
        {
            base.Run();
        }

        public virtual void Struck(BaseObject Hiter)
        {
            byte btDir = 0;
            this.StruckTick = HUtil32.GetTickCount();
            if (Hiter != null)
            {
                if (this.TargetCret == null || this.GetAttackDir(this.TargetCret, ref btDir) || M2Share.RandomNumber.Random(6) == 0)
                {
                    if (this.IsProperTarget(Hiter))
                    {
                        this.SetTargetCreat(Hiter);
                    }
                }
            }
            if (this.Animal)
            {
                this.MeatQuality = (ushort)(this.MeatQuality - M2Share.RandomNumber.Random(300));
                if (this.MeatQuality < 0)
                {
                    this.MeatQuality = 0;
                }
            }
            this.AttackTick = this.AttackTick + (150 - HUtil32._MIN(130, this.Abil.Level * 4));
        }

        protected void HitMagAttackTarget(BaseObject TargeTBaseObject, int nHitPower, int nMagPower, bool boFlag)
        {
            IList<BaseObject> BaseObjectList = new List<BaseObject>();
            this.Direction = M2Share.GetNextDirection(this.CurrX, this.CurrY, TargeTBaseObject.CurrX, TargeTBaseObject.CurrY);
            this.Envir.GetBaseObjects(TargeTBaseObject.CurrX, TargeTBaseObject.CurrY, false, BaseObjectList);
            for (var i = 0; i < BaseObjectList.Count; i++)
            {
                var BaseObject = BaseObjectList[i];
                if (this.IsProperTarget(BaseObject))
                {
                    var nDamage = 0;
                    nDamage += BaseObject.GetHitStruckDamage(this, nHitPower);
                    nDamage += BaseObject.GetMagStruckDamage(this, nMagPower);
                    if (nDamage > 0)
                    {
                        BaseObject.StruckDamage((ushort)nDamage);
                        BaseObject.SendDelayMsg(Grobal2.RM_STRUCK, Grobal2.RM_REFMESSAGE, nDamage, BaseObject.WAbil.HP, BaseObject.WAbil.MaxHP, this.ActorId, "", 200);
                    }
                }
            }
            BaseObjectList.Clear();
            BaseObjectList = null;
            this.SendRefMsg(Grobal2.RM_HIT, this.Direction, this.CurrX, this.CurrY, 0, "");
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
            BaseObject BaseObject18 = null;
            var n10 = 999;
            for (var i = 0; i < this.VisibleActors.Count; i++)
            {
                var baseObject = this.VisibleActors[i].BaseObject;
                if (baseObject.Death || baseObject.Ghost || (baseObject.Envir != Envir) || (Math.Abs(baseObject.CurrX - CurrX) > 15) || (Math.Abs(baseObject.CurrY - CurrY) > 15))
                {
                    ClearTargetCreat(baseObject);
                    continue;
                }
                if (!baseObject.Death)
                {
                    if (this.IsProperTarget(baseObject) && (!baseObject.HideMode || this.CoolEye))
                    {
                        var nC = Math.Abs(this.CurrX - baseObject.CurrX) + Math.Abs(this.CurrY - baseObject.CurrY);
                        if (nC < n10)
                        {
                            n10 = nC;
                            BaseObject18 = baseObject;
                        }
                    }
                }
            }
            if (BaseObject18 != null)
            {
                this.SetTargetCreat(BaseObject18);
            }
        }

        protected void sub_4C959C()
        {
            BaseObject Creat = null;
            var n10 = 999;
            for (var i = 0; i < this.VisibleActors.Count; i++)
            {
                var BaseObject = this.VisibleActors[i].BaseObject;
                if (BaseObject.Death)
                {
                    continue;
                }
                if (!this.IsProperTarget(BaseObject)) continue;
                var nC = Math.Abs(this.CurrX - BaseObject.CurrX) + Math.Abs(this.CurrY - BaseObject.CurrY);
                if (nC >= n10) continue;
                n10 = nC;
                Creat = BaseObject;
            }
            if (Creat != null)
            {
                this.SetTargetCreat(Creat);
            }
        }

        protected virtual void SetTargetXY(short nX, short nY)
        {
            TargetX = nX;
            TargetY = nY;
        }

        protected virtual void Wondering()
        {
            if (M2Share.RandomNumber.Random(20) != 0) return;
            if (M2Share.RandomNumber.Random(4) == 1)
            {
                this.TurnTo(M2Share.RandomNumber.RandomByte(8));
            }
            else
            {
                this.WalkTo(this.Direction, false);
            }
        }
    }
}

