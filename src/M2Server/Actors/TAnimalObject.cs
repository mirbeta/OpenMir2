using System;
using System.Collections.Generic;
using SystemModule;

namespace M2Server
{
    public class TAnimalObject: TBaseObject
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

        public TAnimalObject() : base()
        {
            m_nNotProcessCount = 0;
            m_nTargetX =  -1;
            this.m_btRaceServer = Grobal2.RC_ANIMAL;
            this.m_dwHitTick = HUtil32.GetTickCount() - M2Share.RandomNumber.Random(3000);
            this.m_dwWalkTick = HUtil32.GetTickCount() - M2Share.RandomNumber.Random(3000);
            this.m_dwSearchEnemyTick = HUtil32.GetTickCount();
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
            if (this.m_nCurrX != m_nTargetX || this.m_nCurrY != m_nTargetY)
            {
                n10 = m_nTargetX;
                n14 = m_nTargetY;
                nDir = Grobal2.DR_DOWN;
                if (n10 > this.m_nCurrX)
                {
                    nDir = Grobal2.DR_RIGHT;
                    if (n14 > this.m_nCurrY)
                    {
                        nDir = Grobal2.DR_DOWNRIGHT;
                    }
                    if (n14 < this.m_nCurrY)
                    {
                        nDir = Grobal2.DR_UPRIGHT;
                    }
                }
                else
                {
                    if (n10 < this.m_nCurrX)
                    {
                        nDir = Grobal2.DR_LEFT;
                        if (n14 > this.m_nCurrY)
                        {
                            nDir = Grobal2.DR_DOWNLEFT;
                        }
                        if (n14 < this.m_nCurrY)
                        {
                            nDir = Grobal2.DR_UPLEFT;
                        }
                    }
                    else
                    {
                        if (n14 > this.m_nCurrY)
                        {
                            nDir = Grobal2.DR_DOWN;
                        }
                        else if (n14 < this.m_nCurrY)
                        {
                            nDir = Grobal2.DR_UP;
                        }
                    }
                }
                nOldX = this.m_nCurrX;
                nOldY = this.m_nCurrY;
                this.WalkTo(nDir, false);
                n20 = M2Share.RandomNumber.Random(3);
                for (var i = Grobal2.DR_UP; i <= Grobal2.DR_UPLEFT; i ++ )
                {
                    if (nOldX == this.m_nCurrX && nOldY == this.m_nCurrY)
                    {
                        if (n20 != 0)
                        {
                            nDir ++;
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

        public override bool Operate(TProcessMessage ProcessMsg)
        {
            if (ProcessMsg.wIdent == Grobal2.RM_STRUCK)
            {
                if (ProcessMsg.BaseObject == this.ObjectId && M2Share.ObjectSystem.Get(ProcessMsg.nParam3) != null)
                {
                    this.SetLastHiter(M2Share.ObjectSystem.Get(ProcessMsg.nParam3));
                    Struck(M2Share.ObjectSystem.Get(ProcessMsg.nParam3));
                    this.BreakHolySeizeMode();
                    if (this.m_Master != null && M2Share.ObjectSystem.Get(ProcessMsg.nParam3) != this.m_Master && M2Share.ObjectSystem.Get(ProcessMsg.nParam3).m_btRaceServer == Grobal2.RC_PLAYOBJECT)
                    {
                        this.m_Master.SetPKFlag(M2Share.ObjectSystem.Get(ProcessMsg.nParam3));
                    }
                    if (M2Share.g_Config.boMonSayMsg)
                    {
                        this.MonsterSayMsg(M2Share.ObjectSystem.Get(ProcessMsg.nParam3), TMonStatus.s_UnderFire);
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
            this.m_dwStruckTick = HUtil32.GetTickCount();
            if (Hiter != null)
            {
                if (this.m_TargetCret == null || this.GetAttackDir(this.m_TargetCret, ref btDir) || M2Share.RandomNumber.Random(6) == 0)
                {
                    if (this.IsProperTarget(Hiter))
                    {
                        this.SetTargetCreat(Hiter);
                    }
                }
            }
            if (this.m_boAnimal)
            {
                this.m_nMeatQuality = (ushort)(this.m_nMeatQuality - M2Share.RandomNumber.Random(300));
                if (this.m_nMeatQuality < 0)
                {
                    this.m_nMeatQuality = 0;
                }
            }
            this.m_dwHitTick = this.m_dwHitTick + (150 - HUtil32._MIN(130, this.m_Abil.Level * 4));
        }

        protected void HitMagAttackTarget(TBaseObject TargeTBaseObject, int nHitPower, int nMagPower, bool boFlag)
        {
            int nDamage;
            TBaseObject BaseObject;
            IList<TBaseObject> BaseObjectList = new List<TBaseObject>();
            this.m_btDirection = M2Share.GetNextDirection(this.m_nCurrX, this.m_nCurrY, TargeTBaseObject.m_nCurrX, TargeTBaseObject.m_nCurrY);
            this.m_PEnvir.GetBaseObjects(TargeTBaseObject.m_nCurrX, TargeTBaseObject.m_nCurrY, false, BaseObjectList);
            for (var i = 0; i < BaseObjectList.Count; i ++ )
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
            this.SendRefMsg(Grobal2.RM_HIT, this.m_btDirection, this.m_nCurrX, this.m_nCurrY, 0, "");
        }

        public override void DelTargetCreat()
        {
            base.DelTargetCreat();
            m_nTargetX =  -1;
            m_nTargetY =  -1;
        }

        protected virtual void SearchTarget()
        {
            TBaseObject BaseObject = null;
            TBaseObject BaseObject18 = null;
            int nC;
            var n10 = 999;
            for (var i = 0; i < this.m_VisibleActors.Count; i ++ )
            {
                BaseObject = this.m_VisibleActors[i].BaseObject;
                if (!BaseObject.m_boDeath)
                {
                    if (this.IsProperTarget(BaseObject) && (!BaseObject.m_boHideMode || this.m_boCoolEye))
                    {
                        nC = Math.Abs(this.m_nCurrX - BaseObject.m_nCurrX) + Math.Abs(this.m_nCurrY - BaseObject.m_nCurrY);
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
            for (var i = 0; i < this.m_VisibleActors.Count; i ++ )
            {
                BaseObject = this.m_VisibleActors[i].BaseObject;
                if (BaseObject.m_boDeath)
                {
                    continue;
                }
                if (!this.IsProperTarget(BaseObject)) continue;
                var nC = Math.Abs(this.m_nCurrX - BaseObject.m_nCurrX) + Math.Abs(this.m_nCurrY - BaseObject.m_nCurrY);
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
                this.TurnTo((byte)M2Share.RandomNumber.Random(8));
            }
            else
            {
                this.WalkTo(this.m_btDirection, false);
            }
        }
    }
}

