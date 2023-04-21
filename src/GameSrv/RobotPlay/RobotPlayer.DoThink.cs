using System.Collections;
using GameSrv.Magic;
using SystemModule.Enums;

namespace GameSrv.RobotPlay
{
    public partial class RobotPlayer
    {
        private int CheckTargetXYCount(int nX, int nY, int nRange)
        {
            int n10 = nRange;
            int result = 0;
            if (VisibleActors.Count > 0)
            {
                for (int i = 0; i < VisibleActors.Count; i++)
                {
                    var baseObject = VisibleActors[i].BaseObject;
                    if (baseObject != null)
                    {
                        if (!baseObject.Death)
                        {
                            if (IsProperTarget(baseObject) && (!baseObject.HideMode || CoolEye))
                            {
                                var nC = Math.Abs(nX - baseObject.CurrX) + Math.Abs(nY - baseObject.CurrY);
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

        private bool TargetNeedRunPos()
        {
            return TargetCret.Race == ActorRace.Play || TargetCret.Race == 108;
        }

        private bool CanRunPos(int nAttackCount)
        {
            return MRunPos.AttackCount >= nAttackCount;
        }

        /// <summary>
        /// 使用野蛮冲撞技能时，判断目标是否需要移动
        /// </summary>
        /// <returns></returns>
        private bool DoThinkMotaeboPos(short magicId)
        {
            bool result = false;
            short nTargetX = 0;
            short nTargetY = 0;
            if (magicId == MagicConst.SKILL_MOOTEBO && Master != null && TargetCret != null && AllowUseMagic(MagicConst.SKILL_MOOTEBO) && TargetCret.Abil.Level < Abil.Level && CheckMagicInterval(27,  1000 * 10))
            {
                var btNewDir = GameShare.GetNextDirection(TargetCret.CurrX, TargetCret.CurrY, Master.CurrX, Master.CurrY);
                if (Envir.GetNextPosition(TargetCret.CurrX, TargetCret.CurrY, GetBackDir(btNewDir), 1, ref nTargetX, ref nTargetY))
                {
                    result = Envir.CanWalk(nTargetX, nTargetY, true);
                }
            }
            return result;
        }

        private bool MagPushArround(int magicId, short wMagicId)
        {
            bool result = false;
            byte btNewDir;
            short nTargetX = 0;
            short nTargetY = 0;
            if (TargetCret != null && Abil.Level > TargetCret.Abil.Level && Math.Abs(CurrX - TargetCret.CurrX) <= 1 && Math.Abs(CurrY - TargetCret.CurrY) <= 1)
            {
                btNewDir = GameShare.GetNextDirection(TargetCret.CurrX, TargetCret.CurrY, CurrX, CurrY);
                if (Envir.GetNextPosition(TargetCret.CurrX, TargetCret.CurrY, GetBackDir(btNewDir), 1, ref nTargetX, ref nTargetY))
                {
                    result = Envir.CanWalk(nTargetX, nTargetY, true);
                }
                if (result)
                {
                    return result;
                }
            }
            if (wMagicId == magicId)
            {
                for (int i = 0; i < VisibleActors.Count; i++)
                {
                    var targetObject = VisibleActors[i].BaseObject;
                    if (Math.Abs(CurrX - targetObject.CurrX) <= 1 && Math.Abs(CurrY - targetObject.CurrY) <= 1)
                    {
                        if (!targetObject.Death && targetObject != this && IsProperTarget(targetObject))
                        {
                            if (Abil.Level > targetObject.Abil.Level && !targetObject.StickMode)
                            {
                                btNewDir = GameShare.GetNextDirection(targetObject.CurrX, targetObject.CurrY, CurrX, CurrY);
                                if (Envir.GetNextPosition(targetObject.CurrX, targetObject.CurrY, GetBackDir(btNewDir), 1, ref nTargetX, ref nTargetY))
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

        private int DoThink(short magicId)
        {
            byte btDir = 0;
            int result = -1;
            switch (Job)
            {
                case PlayJob.Warrior: // 1=野蛮冲撞 2=无法攻击到目标需要移动 3=走位
                    if (DoThinkMotaeboPos(magicId))
                    {
                        result = 1;
                    }
                    else
                    {
                        var nRange = 1;
                        if (magicId == 43)
                        {
                            nRange = 4;
                        }
                        if (magicId == 12)
                        {
                            nRange = 2;
                        }
                        if (magicId == 60)
                        {
                            nRange = 6;
                        }
                        result = 2;
                        if (magicId == 61 || magicId == 62 || CanAttack(TargetCret, nRange, ref btDir))
                        {
                            result = 0;
                        }
                        if (Math.Abs(TargetCret.CurrX - CurrX) > 2 || Math.Abs(TargetCret.CurrY - CurrY) > 2)
                        {
                            if (result == 0 && !new ArrayList(new short[] { 60, 61, 62 }).Contains(magicId))
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
                    if (magicId == 8 && MagPushArround(magicId, magicId))
                    {
                        return result;
                    }
                    if (IsUseAttackMagic()) // 1=躲避 2=追击 3=魔法直线攻击不到目标 4=无法攻击到目标需要移动 5=走位
                    {
                        if (CheckTargetXYCount(CurrX, CurrY, 2) > 0)
                        {
                            result = 1;
                        }
                        else if (Math.Abs(TargetCret.CurrX - CurrX) > 6 || Math.Abs(TargetCret.CurrY - CurrY) > 6)
                        {
                            result = 2;
                        }
                        else if (new ArrayList(new short[] { MagicConst.SKILL_FIREBALL, MagicConst.SKILL_FIREBALL2 }).Contains(magicId) && !CanAttack(TargetCret, 10, ref btDir))
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
                    if (magicId == 48 && MagPushArround(magicId, magicId))
                    {
                        return result;
                    }
                    if (IsUseAttackMagic())// 1=躲避 2=追击 3=魔法直线攻击不到目标 4=无法攻击到目标需要移动 5=走位
                    {
                        if (CheckTargetXYCount(CurrX, CurrY, 2) > 0)
                        {
                            result = 1;
                        }
                        else if (Math.Abs(TargetCret.CurrX - CurrX) > 6 || Math.Abs(TargetCret.CurrY - CurrY) > 6)
                        {
                            result = 2;
                        }
                        else if (magicId == MagicConst.SKILL_FIRECHARM && !CanAttack(TargetCret, 10, ref btDir))
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