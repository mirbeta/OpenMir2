using GameSvr.Actor;
using GameSvr.Magic;
using System.Collections;
using SystemModule;

namespace GameSvr.RobotPlay
{
    public partial class RobotPlayObject
    {
        public int DoThink_CheckTargetXYCount(int nX, int nY, int nRange)
        {
            TBaseObject BaseObject;
            int nC;
            int n10 = nRange;
            int result = 0;
            if (m_VisibleActors.Count > 0)
            {
                for (var i = 0; i < m_VisibleActors.Count; i++)
                {
                    BaseObject = m_VisibleActors[i].BaseObject;
                    if (BaseObject != null)
                    {
                        if (!BaseObject.m_boDeath)
                        {
                            if (IsProperTarget(BaseObject) && (!BaseObject.m_boHideMode || m_boCoolEye))
                            {
                                nC = Math.Abs(nX - BaseObject.m_nCurrX) + Math.Abs(nY - BaseObject.m_nCurrY);
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

        public bool DoThink_TargetNeedRunPos()
        {
            return m_TargetCret.m_btRaceServer == Grobal2.RC_PLAYOBJECT || m_TargetCret.m_btRaceServer == 108;
        }

        public bool DoThink_CanRunPos(int nAttackCount)
        {
            return m_RunPos.nAttackCount >= nAttackCount;
        }

        public bool DoThink_MotaeboPos(short wMagicID)
        {
            bool result = false;
            short nTargetX = 0;
            short nTargetY = 0;
            byte btNewDir;
            if (wMagicID == 27 && m_Master != null && m_TargetCret != null && AllowUseMagic(27) && m_TargetCret.m_Abil.Level < m_Abil.Level && HUtil32.GetTickCount() - m_SkillUseTick[27] > 1000 * 10)
            {
                btNewDir = M2Share.GetNextDirection(m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY, m_Master.m_nCurrX, m_Master.m_nCurrY);
                if (m_PEnvir.GetNextPosition(m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY, GetBackDir(btNewDir), 1, ref nTargetX, ref nTargetY))
                {
                    result = m_PEnvir.CanWalk(nTargetX, nTargetY, true);
                }
            }
            return result;
        }

        public bool DoThink_MagPushArround(int MagicID, short wMagicID)
        {
            bool result = false;
            TBaseObject ActorObject;
            byte btNewDir;
            short nTargetX = 0;
            short nTargetY = 0;
            if (m_TargetCret != null && m_Abil.Level > m_TargetCret.m_Abil.Level && Math.Abs(m_nCurrX - m_TargetCret.m_nCurrX) <= 1 && Math.Abs(m_nCurrY - m_TargetCret.m_nCurrY) <= 1)
            {
                btNewDir = M2Share.GetNextDirection(m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY, m_nCurrX, m_nCurrY);
                if (m_PEnvir.GetNextPosition(m_TargetCret.m_nCurrX, m_TargetCret.m_nCurrY, GetBackDir(btNewDir), 1, ref nTargetX, ref nTargetY))
                {
                    result = m_PEnvir.CanWalk(nTargetX, nTargetY, true);
                }
                if (result)
                {
                    return result;
                }
            }
            if (wMagicID == MagicID)
            {
                for (var i = 0; i < m_VisibleActors.Count; i++)
                {
                    ActorObject = m_VisibleActors[i].BaseObject;
                    if (Math.Abs(m_nCurrX - ActorObject.m_nCurrX) <= 1 && Math.Abs(m_nCurrY - ActorObject.m_nCurrY) <= 1)
                    {
                        if (!ActorObject.m_boDeath && ActorObject != this && IsProperTarget(ActorObject))
                        {
                            if (m_Abil.Level > ActorObject.m_Abil.Level && !ActorObject.m_boStickMode)
                            {
                                btNewDir = M2Share.GetNextDirection(ActorObject.m_nCurrX, ActorObject.m_nCurrY, m_nCurrX, m_nCurrY);
                                if (m_PEnvir.GetNextPosition(ActorObject.m_nCurrX, ActorObject.m_nCurrY, GetBackDir(btNewDir), 1, ref nTargetX, ref nTargetY))
                                {
                                    if (m_PEnvir.CanWalk(nTargetX, nTargetY, true))
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
            switch (m_btJob)
            {
                case 0: // 1=野蛮冲撞 2=无法攻击到目标需要移动 3=走位
                    if (DoThink_MotaeboPos(wMagicID))
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
                        if (wMagicID == 61 || wMagicID == 62 || CanAttack(m_TargetCret, nRange, ref btDir))
                        {
                            result = 0;
                        }
                        if (Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) > 2 || Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) > 2)
                        {
                            if (result == 0 && !new ArrayList(new short[] { 60, 61, 62 }).Contains(wMagicID))
                            {
                                if (DoThink_TargetNeedRunPos())
                                {
                                    if (DoThink_CanRunPos(5))
                                    {
                                        result = 5;
                                    }
                                }
                                else
                                {
                                    if (DoThink_CanRunPos(20))
                                    {
                                        result = 5;
                                    }
                                }
                            }
                            if (Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) > 6 || Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) > 6)
                            {
                                result = 2;
                            }
                        }
                    }
                    break;
                case 1:
                    if (wMagicID == 8 && DoThink_MagPushArround(wMagicID, wMagicID))
                    {
                        return result;
                    }
                    // 1=躲避 2=追击 3=魔法直线攻击不到目标 4=无法攻击到目标需要移动 5=走位
                    if (IsUseAttackMagic())
                    {
                        if (DoThink_CheckTargetXYCount(m_nCurrX, m_nCurrY, 2) > 0)
                        {
                            result = 1;
                        }
                        else if (Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) > 6 || Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) > 6)
                        {
                            result = 2;
                        }
                        else if (new ArrayList(new short[] { SpellsDef.SKILL_FIREBALL, SpellsDef.SKILL_FIREBALL2 }).Contains(wMagicID) && !CanAttack(m_TargetCret, 10, ref btDir))
                        {
                            result = 3;
                        }
                        else if (DoThink_TargetNeedRunPos() && DoThink_CanRunPos(5) && DoThink_CheckTargetXYCount(m_nCurrX, m_nCurrY, 2) > 0)
                        {
                            result = 5;
                        }
                    }
                    else
                    {
                        if (!GetAttackDir(m_TargetCret, 1, ref btDir))
                        {
                            result = 4;
                        }
                    }
                    break;
                case 2:
                    if (wMagicID == 48 && DoThink_MagPushArround(wMagicID, wMagicID))
                    {
                        return result;
                    }
                    // 1=躲避 2=追击 3=魔法直线攻击不到目标 4=无法攻击到目标需要移动 5=走位
                    if (IsUseAttackMagic())
                    {
                        if (DoThink_CheckTargetXYCount(m_nCurrX, m_nCurrY, 2) > 0)
                        {
                            result = 1;
                        }
                        else if (Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) > 6 || Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) > 6)
                        {
                            result = 2;
                        }
                        else if (wMagicID == SpellsDef.SKILL_FIRECHARM && !CanAttack(m_TargetCret, 10, ref btDir))
                        {
                            result = 3;
                        }
                        else if (DoThink_TargetNeedRunPos() && DoThink_CanRunPos(5) && DoThink_CheckTargetXYCount(m_nCurrX, m_nCurrY, 2) > 0)
                        {
                            result = 5;
                        }
                    }
                    else
                    {
                        if (!GetAttackDir(m_TargetCret, 1, ref btDir))
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