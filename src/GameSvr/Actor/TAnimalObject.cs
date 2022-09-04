using SystemModule;
using SystemModule.Data;

namespace GameSvr.Actor
{
    public class AnimalObject : TBaseObject
    {
        /// <summary>
        /// 未被处理次数，用于怪物处理循环
        /// </summary>
        public int m_nNotProcessCount = 0;
        public short m_nTargetX = 0;
        public short m_nTargetY = 0;
        public bool m_boRunAwayMode = false;
        public int m_dwRunAwayStart = 0;
        public int m_dwRunAwayTime = 0;

        public virtual void Attack(TBaseObject TargeTBaseObject, byte nDir)
        {
            base.AttackDir(TargeTBaseObject, 0, nDir);
        }

        public AnimalObject() : base()
        {
            m_nNotProcessCount = 0;
            m_nTargetX = -1;
            this.Race = Grobal2.RC_ANIMAL;
            this.AttackTick = HUtil32.GetTickCount() - M2Share.RandomNumber.Random(3000);
            this.WalkTick = HUtil32.GetTickCount() - M2Share.RandomNumber.Random(3000);
            this.SearchEnemyTick = HUtil32.GetTickCount();
            m_boRunAwayMode = false;
            m_dwRunAwayStart = HUtil32.GetTickCount();
            m_dwRunAwayTime = 0;
        }

        protected virtual void GotoTargetXY()
        {
            byte nDir;
            int n10;
            int n14;
            int n20;
            int nOldX;
            int nOldY;
            if (this.CurrX != m_nTargetX || this.CurrY != m_nTargetY)
            {
                n10 = m_nTargetX;
                n14 = m_nTargetY;
                nDir = Grobal2.DR_DOWN;
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
                nOldX = this.CurrX;
                nOldY = this.CurrY;
                this.WalkTo(nDir, false);
                n20 = M2Share.RandomNumber.Random(3);
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

        protected override bool Operate(TProcessMessage ProcessMsg)
        {
            if (ProcessMsg.wIdent == Grobal2.RM_STRUCK)
            {
                var struckObject = M2Share.ActorMgr.Get(ProcessMsg.nParam3);
                if (ProcessMsg.BaseObject == this.ObjectId && struckObject != null)
                {
                    this.SetLastHiter(struckObject);
                    Struck(struckObject);
                    this.BreakHolySeizeMode();
                    if (this.Master != null && struckObject != this.Master && struckObject.Race == Grobal2.RC_PLAYOBJECT)
                    {
                        this.Master.SetPKFlag(struckObject);
                    }
                    if (M2Share.g_Config.boMonSayMsg)
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

        public virtual void Struck(TBaseObject Hiter)
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
                this.m_nMeatQuality = (ushort)(this.m_nMeatQuality - M2Share.RandomNumber.Random(300));
                if (this.m_nMeatQuality < 0)
                {
                    this.m_nMeatQuality = 0;
                }
            }
            this.AttackTick = this.AttackTick + (150 - HUtil32._MIN(130, this.Abil.Level * 4));
        }

        protected void HitMagAttackTarget(TBaseObject TargeTBaseObject, int nHitPower, int nMagPower, bool boFlag)
        {
            int nDamage;
            TBaseObject BaseObject;
            IList<TBaseObject> BaseObjectList = new List<TBaseObject>();
            this.Direction = M2Share.GetNextDirection(this.CurrX, this.CurrY, TargeTBaseObject.CurrX, TargeTBaseObject.CurrY);
            this.Envir.GetBaseObjects(TargeTBaseObject.CurrX, TargeTBaseObject.CurrY, false, BaseObjectList);
            for (var i = 0; i < BaseObjectList.Count; i++)
            {
                BaseObject = BaseObjectList[i];
                if (this.IsProperTarget(BaseObject))
                {
                    nDamage = 0;
                    nDamage += BaseObject.GetHitStruckDamage(this, nHitPower);
                    nDamage += BaseObject.GetMagStruckDamage(this, nMagPower);
                    if (nDamage > 0)
                    {
                        BaseObject.StruckDamage(nDamage);
                        BaseObject.SendDelayMsg(Grobal2.RM_STRUCK, Grobal2.RM_10101, (ushort)nDamage, BaseObject.m_WAbil.HP, BaseObject.m_WAbil.MaxHP, this.ObjectId, "", 200);
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
            m_nTargetX = -1;
            m_nTargetY = -1;
        }

        protected virtual void SearchTarget()
        {
            TBaseObject BaseObject = null;
            TBaseObject BaseObject18 = null;
            int nC;
            var n10 = 999;
            for (var i = 0; i < this.VisibleActors.Count; i++)
            {
                BaseObject = this.VisibleActors[i].BaseObject;
                if (!BaseObject.Death)
                {
                    if (this.IsProperTarget(BaseObject) && (!BaseObject.HideMode || this.CoolEye))
                    {
                        nC = Math.Abs(this.CurrX - BaseObject.CurrX) + Math.Abs(this.CurrY - BaseObject.CurrY);
                        if (nC < n10)
                        {
                            n10 = nC;
                            BaseObject18 = BaseObject;
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
            TBaseObject BaseObject;
            TBaseObject Creat = null;
            var n10 = 999;
            for (var i = 0; i < this.VisibleActors.Count; i++)
            {
                BaseObject = this.VisibleActors[i].BaseObject;
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
            m_nTargetX = nX;
            m_nTargetY = nY;
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

