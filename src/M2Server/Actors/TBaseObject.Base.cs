using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace M2Server
{
    public partial class TBaseObject
    {
        public virtual void Die()
        {
            bool boPK;
            bool guildwarkill;
            string tStr;
            int tExp;
            TPlayObject GroupHuman;
            TMerchant QuestNPC;
            bool tCheck;
            TBaseObject AttackBaseObject;
            TUserCastle Castle;
            const string sExceptionMsg1 = "[Exception] TBaseObject::Die 1";
            const string sExceptionMsg2 = "[Exception] TBaseObject::Die 2";
            const string sExceptionMsg3 = "[Exception] TBaseObject::Die 3";
            if (m_boSuperMan)
            {
                return;
            }
            if (m_boSupermanItem)
            {
                return;
            }
            m_boDeath = true;
            m_dwDeathTick = HUtil32.GetTickCount();
            if (m_Master != null)
            {
                m_ExpHitter = null;
                m_LastHiter = null;
            }
            m_nIncSpell = 0;
            m_nIncHealth = 0;
            m_nIncHealing = 0;
            KillFunc();
            try
            {
                if (m_btRaceServer != grobal2.RC_PLAYOBJECT && m_LastHiter != null)
                {
                    if (M2Share.g_Config.boMonSayMsg)
                    {
                        MonsterSayMsg(m_LastHiter, TMonStatus.s_Die);
                    }
                    if (m_ExpHitter != null)
                    {
                        if (m_ExpHitter.m_btRaceServer == grobal2.RC_PLAYOBJECT)
                        {
                            if (M2Share.g_FunctionNPC != null)
                            {
                                M2Share.g_FunctionNPC.GotoLable(m_ExpHitter as TPlayObject, "@PlayKillMob", false);
                            }
                            tExp = m_ExpHitter.CalcGetExp(m_Abil.Level, m_dwFightExp);
                            if (!M2Share.g_Config.boVentureServer)
                            {
                                (m_ExpHitter as TPlayObject).GainExp(tExp);
                            }
                            // 是否执行任务脚本
                            if (m_PEnvir.IsCheapStuff())
                            {
                                if (m_ExpHitter.m_GroupOwner != null)
                                {
                                    for (var i = 0; i < m_ExpHitter.m_GroupOwner.m_GroupMembers.Count; i++)
                                    {
                                        GroupHuman = m_ExpHitter.m_GroupOwner.m_GroupMembers[i];
                                        if (!GroupHuman.m_boDeath && m_ExpHitter.m_PEnvir == GroupHuman.m_PEnvir && Math.Abs(m_ExpHitter.m_nCurrX - GroupHuman.m_nCurrX) <= 12 && Math.Abs(m_ExpHitter.m_nCurrX - GroupHuman.m_nCurrX) <= 12 && m_ExpHitter == GroupHuman)
                                        {
                                            tCheck = false;
                                        }
                                        else
                                        {
                                            tCheck = true;
                                        }
                                        QuestNPC = (TMerchant)m_PEnvir.GetQuestNPC(GroupHuman, m_sCharName, "", tCheck);
                                        if (QuestNPC != null)
                                        {
                                            QuestNPC.Click(GroupHuman);
                                        }
                                    }
                                }
                                QuestNPC = (TMerchant)m_PEnvir.GetQuestNPC(m_ExpHitter, m_sCharName, "", false);
                                if (QuestNPC != null)
                                {
                                    QuestNPC.Click(m_ExpHitter as TPlayObject);
                                }
                            }
                        }
                        else
                        {
                            if (m_ExpHitter.m_Master != null)
                            {
                                m_ExpHitter.GainSlaveExp(m_Abil.Level);
                                tExp = m_ExpHitter.m_Master.CalcGetExp(m_Abil.Level, m_dwFightExp);
                                if (!M2Share.g_Config.boVentureServer)
                                {
                                    (m_ExpHitter.m_Master as TPlayObject).GainExp(tExp);
                                }
                            }
                        }
                    }
                    else
                    {
                        if (m_LastHiter.m_btRaceServer == grobal2.RC_PLAYOBJECT)
                        {
                            if (M2Share.g_FunctionNPC != null)
                            {
                                M2Share.g_FunctionNPC.GotoLable(m_LastHiter as TPlayObject, "@PlayKillMob", false);
                            }
                            tExp = m_LastHiter.CalcGetExp(m_Abil.Level, m_dwFightExp);
                            if (!M2Share.g_Config.boVentureServer)
                            {
                                (m_LastHiter as TPlayObject).GainExp(tExp);
                            }
                        }
                    }
                }
                if (M2Share.g_Config.boMonSayMsg && m_btRaceServer == grobal2.RC_PLAYOBJECT && m_LastHiter != null)
                {
                    m_LastHiter.MonsterSayMsg(this, TMonStatus.s_KillHuman);
                }
                m_Master = null;
            }
            catch (Exception e)
            {
                M2Share.ErrorMessage(sExceptionMsg1);
                M2Share.ErrorMessage(e.Message);
            }
            try
            {
                boPK = false;
                if (!M2Share.g_Config.boVentureServer && !m_PEnvir.Flag.boFightZone && !m_PEnvir.Flag.boFight3Zone)
                {
                    if (m_btRaceServer == grobal2.RC_PLAYOBJECT && m_LastHiter != null && PKLevel() < 2)
                    {
                        // if (m_LastHiter.m_btRaceServer = RC_PLAYOBJECT) then
                        if (m_LastHiter.m_btRaceServer == grobal2.RC_PLAYOBJECT || m_LastHiter.m_btRaceServer == grobal2.RC_NPC)
                        {
                            // 修改日期2004/07/21，允许NPC杀死人物
                            boPK = true;
                        }
                        if (m_LastHiter.m_Master != null)
                        {
                            if (m_LastHiter.m_Master.m_btRaceServer == grobal2.RC_PLAYOBJECT)
                            {
                                m_LastHiter = m_LastHiter.m_Master;
                                boPK = true;
                            }
                        }
                    }
                }
                if (boPK && m_LastHiter != null)
                {
                    guildwarkill = false;
                    if (m_MyGuild != null && m_LastHiter.m_MyGuild != null)
                    {
                        if (GetGuildRelation(this, m_LastHiter) == 2)
                        {
                            guildwarkill = true;    
                        }
                    }
                    Castle = M2Share.CastleManager.InCastleWarArea(this);
                    if (Castle != null && Castle.m_boUnderWar || m_boInFreePKArea)
                    {
                        guildwarkill = true;
                    }
                    // =================================================================
                    if (!guildwarkill)
                    {
                        if ((M2Share.g_Config.boKillHumanWinLevel || M2Share.g_Config.boKillHumanWinExp || m_PEnvir.Flag.boPKWINLEVEL || m_PEnvir.Flag.boPKWINEXP) && m_LastHiter.m_btRaceServer == grobal2.RC_PLAYOBJECT)
                        {
                            (this as TPlayObject).PKDie(m_LastHiter as TPlayObject);
                        }
                        else
                        {
                            if (!m_LastHiter.IsGoodKilling(this))
                            {
                                m_LastHiter.IncPkPoint(M2Share.g_Config.nKillHumanAddPKPoint);
                                m_LastHiter.SysMsg(M2Share.g_sYouMurderedMsg, TMsgColor.c_Red, TMsgType.t_Hint);
                                SysMsg(format(M2Share.g_sYouKilledByMsg, m_LastHiter.m_sCharName), TMsgColor.c_Red, TMsgType.t_Hint);
                                m_LastHiter.AddBodyLuck(-M2Share.g_Config.nKillHumanDecLuckPoint);
                                if (PKLevel() < 1)
                                {
                                    if (M2Share.RandomNumber.Random(5) == 0)
                                    {
                                        m_LastHiter.MakeWeaponUnlock();
                                    }
                                }
                            }
                            else
                            {
                                m_LastHiter.SysMsg(M2Share.g_sYouProtectedByLawOfDefense, TMsgColor.c_Green, TMsgType.t_Hint);
                            }
                        }
                        // 检查攻击人是否用了着经验或等级装备
                        if (m_LastHiter.m_btRaceServer == grobal2.RC_PLAYOBJECT)
                        {
                            if (m_LastHiter.m_dwPKDieLostExp > 0)
                            {
                                if (m_Abil.Exp >= m_LastHiter.m_dwPKDieLostExp)
                                {
                                    m_Abil.Exp -= (short)m_LastHiter.m_dwPKDieLostExp;
                                }
                                else
                                {
                                    m_Abil.Exp = 0;
                                }
                            }
                            if (m_LastHiter.m_nPKDieLostLevel > 0)
                            {
                                if (m_Abil.Level >= m_LastHiter.m_nPKDieLostLevel)
                                {
                                    m_Abil.Level -= (ushort)m_LastHiter.m_nPKDieLostLevel;
                                }
                                else
                                {
                                    m_Abil.Level = 0;
                                }
                            }
                        }
                    }
                    // =================================================================
                }
            }
            catch
            {
                M2Share.ErrorMessage(sExceptionMsg2);
            }
            try
            {
                if (!m_PEnvir.Flag.boFightZone && !m_PEnvir.Flag.boFight3Zone && !m_boAnimal)
                {
                    AttackBaseObject = m_ExpHitter;
                    if (m_ExpHitter != null && m_ExpHitter.m_Master != null)
                    {
                        AttackBaseObject = m_ExpHitter.m_Master;
                    }
                    if (m_btRaceServer != grobal2.RC_PLAYOBJECT)
                    {
                        DropUseItems(AttackBaseObject);
                        if (m_Master == null && (!m_boNoItem || !m_PEnvir.Flag.boNODROPITEM))
                        {
                            ScatterBagItems(AttackBaseObject);
                        }
                        if (m_btRaceServer >= grobal2.RC_ANIMAL && m_Master == null && (!m_boNoItem || !m_PEnvir.Flag.boNODROPITEM))
                        {
                            ScatterGolds(AttackBaseObject);
                        }
                    }
                    else
                    {
                        if (!m_boNoItem || !m_PEnvir.Flag.boNODROPITEM)
                        {
                            // 修改日期2004/07/21，增加此行，允许设置 m_boNoItem 后人物死亡不掉物品
                            if (AttackBaseObject != null)
                            {
                                if (M2Share.g_Config.boKillByHumanDropUseItem && AttackBaseObject.m_btRaceServer == grobal2.RC_PLAYOBJECT || M2Share.g_Config.boKillByMonstDropUseItem && AttackBaseObject.m_btRaceServer != grobal2.RC_PLAYOBJECT)
                                {
                                    DropUseItems(null);
                                }
                            }
                            else
                            {
                                DropUseItems(null);
                            }
                            if (M2Share.g_Config.boDieScatterBag)
                            {
                                ScatterBagItems(null);
                            }
                            if (M2Share.g_Config.boDieDropGold)
                            {
                                ScatterGolds(null);
                            }
                        }
                        AddBodyLuck(-(50 - (50 - m_Abil.Level * 5)));
                    }
                }
                if (m_PEnvir.Flag.boFight3Zone)
                {
                    m_nFightZoneDieCount++;
                    if (m_MyGuild != null)
                    {
                        m_MyGuild.TeamFightWhoDead(m_sCharName);
                    }
                    if (m_LastHiter != null)
                    {
                        if (m_LastHiter.m_MyGuild != null && m_MyGuild != null)
                        {
                            m_LastHiter.m_MyGuild.TeamFightWhoWinPoint(m_LastHiter.m_sCharName, 100);
                            // matchpoint 刘啊, 俺牢己利 扁废
                            tStr = m_LastHiter.m_MyGuild.sGuildName + ':' + m_LastHiter.m_MyGuild.nContestPoint + "  " + m_MyGuild.sGuildName + ':' + m_MyGuild.nContestPoint;
                            M2Share.UserEngine.CryCry(grobal2.RM_CRY, m_PEnvir, m_nCurrX, m_nCurrY, 1000, M2Share.g_Config.btCryMsgFColor, M2Share.g_Config.btCryMsgBColor, "- " + tStr);
                        }
                    }
                }
                if (m_btRaceServer == grobal2.RC_PLAYOBJECT)
                {
                    // Jacky 2004/09/05
                    // 人物死亡立即退组，以防止组队刷经验
                    if (m_GroupOwner != null)
                    {
                        m_GroupOwner.DelMember(this);
                    }
                    if (m_LastHiter != null)
                    {
                        if (m_LastHiter.m_btRaceServer == grobal2.RC_PLAYOBJECT)
                        {
                            tStr = m_LastHiter.m_sCharName;
                        }
                        else
                        {
                            tStr = '#' + m_LastHiter.m_sCharName;
                        }
                    }
                    else
                    {
                        tStr = "####";
                    }
                    M2Share.AddGameDataLog("19" + "\t" + m_sMapName + "\t" + m_nCurrX + "\t" + m_nCurrY + "\t" + m_sCharName + "\t" + "FZ-" + HUtil32.BoolToIntStr(m_PEnvir.Flag.boFightZone) + "_F3-" + HUtil32.BoolToIntStr(m_PEnvir.Flag.boFight3Zone) + "\t" + '0' + "\t" + '1' + "\t" + tStr);
                }
                // 减少地图上怪物计数
                if (m_Master == null && !m_boDelFormMaped)
                {
                    m_PEnvir.DelObjectCount(this);
                    m_boDelFormMaped = true;
                }
                SendRefMsg(grobal2.RM_DEATH, m_btDirection, m_nCurrX, m_nCurrY, 1, "");
            }
            catch
            {
                M2Share.ErrorMessage(sExceptionMsg3);
            }
        }

        internal virtual void ReAlive()
        {
            m_boDeath = false;
            SendRefMsg(grobal2.RM_ALIVE, m_btDirection, m_nCurrX, m_nCurrY, 0, "");
        }

        public virtual bool IsProtectTarget(TBaseObject BaseObject)
        {
            var result = true;
            if (BaseObject == null)
            {
                return result;
            }
            if (InSafeZone() || BaseObject.InSafeZone())
            {
                result = false;
            }
            if (!BaseObject.m_boInFreePKArea)
            {
                if (M2Share.g_Config.boPKLevelProtect)// 新人保护
                {
                    if (m_Abil.Level > M2Share.g_Config.nPKProtectLevel)// 如果大于指定等级
                    {
                        if (!BaseObject.m_boPKFlag && BaseObject.m_Abil.Level <= M2Share.g_Config.nPKProtectLevel && 
                            BaseObject.PKLevel() < 2)// 被攻击的人物小指定等级没有红名，则不可以攻击。
                        {
                            result = false;
                            return result;
                        }
                    }
                    if (m_Abil.Level <= M2Share.g_Config.nPKProtectLevel)
                    {
                        // 如果小于指定等级
                        if (!BaseObject.m_boPKFlag && BaseObject.m_Abil.Level > M2Share.g_Config.nPKProtectLevel && BaseObject.PKLevel() < 2)
                        {
                            result = false;
                            return result;
                        }
                    }
                }
                // 大于指定级别的红名人物不可以杀指定级别未红名的人物。
                if (PKLevel() >= 2 && m_Abil.Level > M2Share.g_Config.nRedPKProtectLevel)
                {
                    if (BaseObject.m_Abil.Level <= M2Share.g_Config.nRedPKProtectLevel && BaseObject.PKLevel() < 2)
                    {
                        result = false;
                        return result;
                    }
                }
                // 小于指定级别的非红名人物不可以杀指定级别红名人物。
                if (m_Abil.Level <= M2Share.g_Config.nRedPKProtectLevel && PKLevel() < 2)
                {
                    if (BaseObject.PKLevel() >= 2 && BaseObject.m_Abil.Level > M2Share.g_Config.nRedPKProtectLevel)
                    {
                        result = false;
                        return result;
                    }
                }
                if (HUtil32.GetTickCount() - m_dwMapMoveTick < 3000 || HUtil32.GetTickCount() - BaseObject.m_dwMapMoveTick < 3000)
                {
                    result = false;
                }
            }
            return result;
        }

    }
}
