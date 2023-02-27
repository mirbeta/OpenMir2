using GameSrv.Actor;
using GameSrv.Magic;
using System.Collections;
using SystemModule.Enums;

namespace GameSrv.RobotPlay
{
    public partial class RobotPlayer
    {
        public int CheckTargetXYCount(int nX, int nY, int nRange)
        {
            BaseObject BaseObject;
            int nC;
            int n10 = nRange;
            int result = 0;
            if (VisibleActors.Count > 0)
            {
                for (int i = 0; i < VisibleActors.Count; i++)
                {
                    BaseObject = VisibleActors[i].BaseObject;
                    if (BaseObject != null)
                    {
                        if (!BaseObject.Death)
                        {
                            if (IsProperTarget(BaseObject) && (!BaseObject.HideMode || CoolEye))
                            {
                                nC = Math.Abs(nX - BaseObject.CurrX) + Math.Abs(nY - BaseObject.CurrY);
                                if (nC <= n10)
                                {
                                    result++;
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        public bool TargetNeedRunPos()
        {
            return TargetCret.Race == ActorRace.Play || TargetCret.Race == 108;
        }

        public bool CanRunPos(int nAttackCount)
        {
            return MRunPos.AttackCount >= nAttackCount;
        }

        public bool DoThinkMotaeboPos(short wMagicID)
        {
            bool result = false;
            short nTargetX = 0;
            short nTargetY = 0;
            byte btNewDir;
            if (wMagicID == 27 && Master != null && TargetCret != null && AllowUseMagic(27) && TargetCret.Abil.Level < Abil.Level && HUtil32.GetTickCount() - MSkillUseTick[27] > 1000 * 10)
            {
                btNewDir = M2Share.GetNextDirection(TargetCret.CurrX, TargetCret.CurrY, Master.CurrX, Master.CurrY);
                if (Envir.GetNextPosition(TargetCret.CurrX, TargetCret.CurrY, GetBackDir(btNewDir), 1, ref nTargetX, ref nTargetY))
                {
                    result = Envir.CanWalk(nTargetX, nTargetY, true);
                }
            }
            return result;
        }

        public bool MagPushArround(int MagicID, short wMagicID)
        {
            bool result = false;
            BaseObject ActorObject;
            byte btNewDir;
            short nTargetX = 0;
            short nTargetY = 0;
            if (TargetCret != null && Abil.Level > TargetCret.Abil.Level && Math.Abs(CurrX - TargetCret.CurrX) <= 1 && Math.Abs(CurrY - TargetCret.CurrY) <= 1)
            {
                btNewDir = M2Share.GetNextDirection(TargetCret.CurrX, TargetCret.CurrY, CurrX, CurrY);
                if (Envir.GetNextPosition(TargetCret.CurrX, TargetCret.CurrY, GetBackDir(btNewDir), 1, ref nTargetX, ref nTargetY))
                {
                    result = Envir.CanWalk(nTargetX, nTargetY, true);
                }
                if (result)
                {
                    return result;
                }
            }
            if (wMagicID == MagicID)
            {
                for (int i = 0; i < VisibleActors.Count; i++)
                {
                    ActorObject = VisibleActors[i].BaseObject;
                    if (Math.Abs(CurrX - ActorObject.CurrX) <= 1 && Math.Abs(CurrY - ActorObject.CurrY) <= 1)
                    {
                        if (!ActorObject.Death && ActorObject != this && IsProperTarget(ActorObject))
                        {
                            if (Abil.Level > ActorObject.Abil.Level && !ActorObject.StickMode)
                            {
                                btNewDir = M2Share.GetNextDirection(ActorObject.CurrX, ActorObject.CurrY, CurrX, CurrY);
                                if (Envir.GetNextPosition(ActorObject.CurrX, ActorObject.CurrY, GetBackDir(btNewDir), 1, ref nTargetX, ref nTargetY))
                                {
                                    if (Envir.CanWalk(nTargetX, nTargetY, true))
                                    {
                                        result = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        private int DoThink(short wMagicID)
        {
            byte btDir = 0;
            int nRange;
            int result = -1;
            switch (Job)
            {
                case PlayJob.Warrior: // 1=野蛮冲撞 2=无法攻击到目标需要移动 3=走位
                    if (DoThinkMotaeboPos(wMagicID))
                    {
                        result = 1;
                    }
                    else
                    {
                        nRange = 1;
                        if (wMagicID == 43)
                        {
                            nRange = 4;
                        }
                        if (wMagicID == 12)
                        {
                            nRange = 2;
                        }
                        if (wMagicID == 60)
                        {
                            nRange = 6;
                        }
                        result = 2;
                        if (wMagicID == 61 || wMagicID == 62 || CanAttack(TargetCret, nRange, ref btDir))
                        {
                            result = 0;
                        }
                        if (Math.Abs(TargetCret.CurrX - CurrX) > 2 || Math.Abs(TargetCret.CurrY - CurrY) > 2)
                        {
                            if (result == 0 && !new ArrayList(new short[] { 60, 61, 62 }).Contains(wMagicID))
                            {
                                if (TargetNeedRunPos())
                                {
                                    if (CanRunPos(5))
                                    {
                                        result = 5;
                                    }
                                }
                                else
                                {
                                    if (CanRunPos(20))
                                    {
                                        result = 5;
                                    }
                                }
                            }
                            if (Math.Abs(TargetCret.CurrX - CurrX) > 6 || Math.Abs(TargetCret.CurrY - CurrY) > 6)
                            {
                                result = 2;
                            }
                        }
                    }
                    break;
                case PlayJob.Wizard:
                    if (wMagicID == 8 && MagPushArround(wMagicID, wMagicID))
                    {
                        return result;
                    }
                    // 1=躲避 2=追击 3=魔法直线攻击不到目标 4=无法攻击到目标需要移动 5=走位
                    if (IsUseAttackMagic())
                    {
                        if (CheckTargetXYCount(CurrX, CurrY, 2) > 0)
                        {
                            result = 1;
                        }
                        else if (Math.Abs(TargetCret.CurrX - CurrX) > 6 || Math.Abs(TargetCret.CurrY - CurrY) > 6)
                        {
                            result = 2;
                        }
                        else if (new ArrayList(new short[] { MagicConst.SKILL_FIREBALL, MagicConst.SKILL_FIREBALL2 }).Contains(wMagicID) && !CanAttack(TargetCret, 10, ref btDir))
                        {
                            result = 3;
                        }
                        else if (TargetNeedRunPos() && CanRunPos(5) && CheckTargetXYCount(CurrX, CurrY, 2) > 0)
                        {
                            result = 5;
                        }
                    }
                    else
                    {
                        if (!GetAttackDir(TargetCret, 1, ref btDir))
                        {
                            result = 4;
                        }
                    }
                    break;
                case PlayJob.Taoist:
                    if (wMagicID == 48 && MagPushArround(wMagicID, wMagicID))
                    {
                        return result;
                    }
                    // 1=躲避 2=追击 3=魔法直线攻击不到目标 4=无法攻击到目标需要移动 5=走位
                    if (IsUseAttackMagic())
                    {
                        if (CheckTargetXYCount(CurrX, CurrY, 2) > 0)
                        {
                            result = 1;
                        }
                        else if (Math.Abs(TargetCret.CurrX - CurrX) > 6 || Math.Abs(TargetCret.CurrY - CurrY) > 6)
                        {
                            result = 2;
                        }
                        else if (wMagicID == MagicConst.SKILL_FIRECHARM && !CanAttack(TargetCret, 10, ref btDir))
                        {
                            result = 3;
                        }
                        else if (TargetNeedRunPos() && CanRunPos(5) && CheckTargetXYCount(CurrX, CurrY, 2) > 0)
                        {
                            result = 5;
                        }
                    }
                    else
                    {
                        if (!GetAttackDir(TargetCret, 1, ref btDir))
                        {
                            result = 4;
                        }
                    }
                    break;
            }
            return result;
        }
    }
}