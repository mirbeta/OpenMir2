using System;
using System.Collections;
using System.Collections.Generic;
using SystemModule;

namespace GameSvr
{
    public partial class TBaseObject
    {
        public virtual void Run()
        {
            TProcessMessage ProcessMsg = null;
            const string sExceptionMsg0 = "[Exception] TBaseObject::Run 0";
            const string sExceptionMsg1 = "[Exception] TBaseObject::Run 1";
            const string sExceptionMsg2 = "[Exception] TBaseObject::Run 2";
            const string sExceptionMsg3 = "[Exception] TBaseObject::Run 3";
            const string sExceptionMsg4 = "[Exception] TBaseObject::Run 4 Code:{0}";
            const string sExceptionMsg5 = "[Exception] TBaseObject::Run 5";
            const string sExceptionMsg6 = "[Exception] TBaseObject::Run 6";
            const string sExceptionMsg7 = "[Exception] TBaseObject::Run 7";
            var dwRunTick = HUtil32.GetTickCount();
            try
            {
                while (GetMessage(ref ProcessMsg))
                {
                    Operate(ProcessMsg);
                }
            }
            catch (Exception e)
            {
                M2Share.ErrorMessage(sExceptionMsg0);
                M2Share.ErrorMessage(e.StackTrace);
            }
            try
            {
                if (m_boSuperMan)
                {
                    m_WAbil.HP = m_WAbil.MaxHP;
                    m_WAbil.MP = m_WAbil.MaxMP;
                }
                int dwC = (HUtil32.GetTickCount() - m_dwHPMPTick) / 20;
                m_dwHPMPTick = HUtil32.GetTickCount();
                m_nHealthTick += dwC;
                m_nSpellTick += dwC;
                if (!m_boDeath)
                {
                    ushort n18;
                    if ((m_WAbil.HP < m_WAbil.MaxHP) && (m_nHealthTick >= M2Share.g_Config.nHealthFillTime))
                    {
                        n18 = (ushort)((m_WAbil.MaxHP / 75) + 1);
                        if ((m_WAbil.HP + n18) < m_WAbil.MaxHP)
                        {
                            m_WAbil.HP += n18;
                        }
                        else
                        {
                            m_WAbil.HP = m_WAbil.MaxHP;
                        }
                        HealthSpellChanged();
                    }
                    if ((m_WAbil.MP < m_WAbil.MaxMP) && (m_nSpellTick >= M2Share.g_Config.nSpellFillTime))
                    {
                        n18 = (ushort)((m_WAbil.MaxMP / 18) + 1);
                        if ((m_WAbil.MP + n18) < m_WAbil.MaxMP)
                        {
                            m_WAbil.MP += n18;
                        }
                        else
                        {
                            m_WAbil.MP = m_WAbil.MaxMP;
                        }
                        HealthSpellChanged();
                    }
                    if (m_WAbil.HP == 0)
                    {
                        if (((m_LastHiter == null) || !m_LastHiter.m_boUnRevival) && m_boRevival && ((HUtil32.GetTickCount() - m_dwRevivalTick) > M2Share.g_Config.dwRevivalTime))// 60 * 1000
                        {
                            m_dwRevivalTick = HUtil32.GetTickCount();
                            ItemDamageRevivalRing();
                            m_WAbil.HP = m_WAbil.MaxHP;
                            HealthSpellChanged();
                            SysMsg(M2Share.g_sRevivalRecoverMsg, MsgColor.Green, MsgType.Hint);
                        }
                        if (m_WAbil.HP == 0)
                        {
                            Die();
                        }
                    }
                    if (m_nHealthTick >= M2Share.g_Config.nHealthFillTime)
                    {
                        m_nHealthTick = 0;
                    }
                    if (m_nSpellTick >= M2Share.g_Config.nSpellFillTime)
                    {
                        m_nSpellTick = 0;
                    }
                }
                else
                {
                    if (m_boCanReAlive && m_pMonGen != null)
                    {
                        var dwMakeGhostTime = HUtil32._MAX(10 * 1000, M2Share.UserEngine.ProcessMonsters_GetZenTime(m_pMonGen.dwZenTime) - 20 * 1000);
                        if (dwMakeGhostTime > M2Share.g_Config.dwMakeGhostTime)
                        {
                            dwMakeGhostTime = M2Share.g_Config.dwMakeGhostTime;
                        }
                        if ((HUtil32.GetTickCount() - m_dwDeathTick > dwMakeGhostTime))
                        {
                            MakeGhost();
                        }
                    }
                    else
                    {
                        if ((HUtil32.GetTickCount() - m_dwDeathTick) > M2Share.g_Config.dwMakeGhostTime)// 3 * 60 * 1000
                        {
                            MakeGhost();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                M2Share.ErrorMessage(sExceptionMsg1);
                M2Share.ErrorMessage(e.Message);
            }
            try
            {
                if (!m_boDeath && ((m_nIncSpell > 0) || (m_nIncHealth > 0) || (m_nIncHealing > 0)))
                {
                    int dwInChsTime = 600 - HUtil32._MIN(400, m_Abil.Level * 10);
                    if (((HUtil32.GetTickCount() - m_dwIncHealthSpellTick) >= dwInChsTime) && !m_boDeath)
                    {
                        int dwC = HUtil32._MIN(200, HUtil32.GetTickCount() - m_dwIncHealthSpellTick - dwInChsTime);
                        m_dwIncHealthSpellTick = HUtil32.GetTickCount() + dwC;
                        if ((m_nIncSpell > 0) || (m_nIncHealth > 0) || (m_nPerHealing > 0))
                        {
                            if (m_nPerHealth <= 0)
                            {
                                m_nPerHealth = 1;
                            }
                            if (m_nPerSpell <= 0)
                            {
                                m_nPerSpell = 1;
                            }
                            if (m_nPerHealing <= 0)
                            {
                                m_nPerHealing = 1;
                            }
                            int nHP;
                            if (m_nIncHealth < m_nPerHealth)
                            {
                                nHP = m_nIncHealth;
                                m_nIncHealth = 0;
                            }
                            else
                            {
                                nHP = m_nPerHealth;
                                m_nIncHealth -= m_nPerHealth;
                            }
                            int nMP;
                            if (m_nIncSpell < m_nPerSpell)
                            {
                                nMP = m_nIncSpell;
                                m_nIncSpell = 0;
                            }
                            else
                            {
                                nMP = m_nPerSpell;
                                m_nIncSpell -= m_nPerSpell;
                            }
                            if (m_nIncHealing < m_nPerHealing)
                            {
                                nHP += m_nIncHealing;
                                m_nIncHealing = 0;
                            }
                            else
                            {
                                nHP += m_nPerHealing;
                                m_nIncHealing -= m_nPerHealing;
                            }
                            m_nPerHealth = m_Abil.Level / 10 + 5;
                            m_nPerSpell = m_Abil.Level / 10 + 5;
                            m_nPerHealing = 5;
                            IncHealthSpell(nHP, nMP);
                            if (m_WAbil.HP == m_WAbil.MaxHP)
                            {
                                m_nIncHealth = 0;
                                m_nIncHealing = 0;
                            }
                            if (m_WAbil.MP == m_WAbil.MaxMP)
                            {
                                m_nIncSpell = 0;
                            }
                        }
                    }
                }
                else
                {
                    m_dwIncHealthSpellTick = HUtil32.GetTickCount();
                }
                if ((m_nHealthTick < -M2Share.g_Config.nHealthFillTime) && (m_WAbil.HP > 1))
                {
                    m_WAbil.HP -= 1;
                    m_nHealthTick += M2Share.g_Config.nHealthFillTime;
                    HealthSpellChanged();
                }
                // 检查HP/MP值是否大于最大值，大于则降低到正常大小
                bool boNeedRecalc = false;
                if (m_WAbil.HP > m_WAbil.MaxHP)
                {
                    boNeedRecalc = true;
                    m_WAbil.HP = (ushort)(m_WAbil.MaxHP - 1);
                }
                if (m_WAbil.MP > m_WAbil.MaxMP)
                {
                    boNeedRecalc = true;
                    m_WAbil.MP = (ushort)(m_WAbil.MaxMP - 1);
                }
                if (boNeedRecalc)
                {
                    HealthSpellChanged();
                }
            }
            catch
            {
                M2Share.ErrorMessage(sExceptionMsg2);
            }
            // 血气石处理开始
            try
            {
                if ( m_UseItems.Length>=Grobal2.U_CHARM && m_UseItems[Grobal2.U_CHARM]!=null)
                {
                    if (!m_boDeath && new ArrayList(new byte[] { Grobal2.RC_PLAYOBJECT, Grobal2.RC_PLAYCLONE }).Contains(m_btRaceServer))
                    {
                        int nCount;
                        int dCount;
                        int bCount;
                        GoodItem StdItem;
                        // 加HP
                        if ((m_nIncHealth == 0) && (m_UseItems[Grobal2.U_CHARM].wIndex > 0) && ((HUtil32.GetTickCount() - m_nIncHPStoneTime) > M2Share.g_Config.HPStoneIntervalTime) && ((m_WAbil.HP / m_WAbil.MaxHP * 100) < M2Share.g_Config.HPStoneStartRate))
                        {
                            m_nIncHPStoneTime = HUtil32.GetTickCount();
                            StdItem = M2Share.UserEngine.GetStdItem(m_UseItems[Grobal2.U_CHARM].wIndex);
                            if ((StdItem.StdMode == 7) && new ArrayList(new byte[] { 1, 3 }).Contains(StdItem.Shape))
                            {
                                nCount = m_UseItems[Grobal2.U_CHARM].Dura * 10;
                                bCount = Convert.ToInt32(nCount / M2Share.g_Config.HPStoneAddRate);
                                dCount = m_WAbil.MaxHP - m_WAbil.HP;
                                if (dCount > bCount)
                                {
                                    dCount = bCount;
                                }
                                if (nCount > dCount)
                                {
                                    m_nIncHealth += dCount;
                                    m_UseItems[Grobal2.U_CHARM].Dura -= (ushort)HUtil32.Round(dCount / 10);
                                }
                                else
                                {
                                    nCount = 0;
                                    m_nIncHealth += nCount;
                                    m_UseItems[Grobal2.U_CHARM].Dura = 0;
                                }
                                if (m_UseItems[Grobal2.U_CHARM].Dura >= 1000)
                                {
                                    if (m_btRaceServer == Grobal2.RC_PLAYOBJECT)
                                    {
                                        SendMsg(this, Grobal2.RM_DURACHANGE, Grobal2.U_CHARM, m_UseItems[Grobal2.U_CHARM].Dura, m_UseItems[Grobal2.U_CHARM].DuraMax, 0, "");
                                    }
                                }
                                else
                                {
                                    m_UseItems[Grobal2.U_CHARM].Dura = 0;
                                    if (m_btRaceServer == Grobal2.RC_PLAYOBJECT)
                                    {
                                        (this as TPlayObject).SendDelItems(m_UseItems[Grobal2.U_CHARM]);
                                    }
                                    m_UseItems[Grobal2.U_CHARM].wIndex = 0;
                                }
                            }
                        }
                        // 加MP
                        if ((m_nIncSpell == 0) && (m_UseItems[Grobal2.U_CHARM].wIndex > 0) && ((HUtil32.GetTickCount() - m_nIncMPStoneTime) > M2Share.g_Config.MPStoneIntervalTime) && ((m_WAbil.MP / m_WAbil.MaxMP * 100) < M2Share.g_Config.MPStoneStartRate))
                        {
                            m_nIncMPStoneTime = HUtil32.GetTickCount();
                            StdItem = M2Share.UserEngine.GetStdItem(m_UseItems[Grobal2.U_CHARM].wIndex);
                            if ((StdItem.StdMode == 7) && new ArrayList(new byte[] { 2, 3 }).Contains(StdItem.Shape))
                            {
                                nCount = m_UseItems[Grobal2.U_CHARM].Dura * 10;
                                bCount = Convert.ToInt32(nCount / M2Share.g_Config.MPStoneAddRate);
                                dCount = m_WAbil.MaxMP - m_WAbil.MP;
                                if (dCount > bCount)
                                {
                                    dCount = bCount;
                                }
                                if (nCount > dCount)
                                {
                                    // Dec(nCount,dCount);
                                    m_nIncSpell += dCount;
                                    m_UseItems[Grobal2.U_CHARM].Dura -= (ushort)HUtil32.Round(dCount / 10);
                                }
                                else
                                {
                                    nCount = 0;
                                    m_nIncSpell += nCount;
                                    m_UseItems[Grobal2.U_CHARM].Dura = 0;
                                }
                                if (m_UseItems[Grobal2.U_CHARM].Dura >= 1000)
                                {
                                    if (m_btRaceServer == Grobal2.RC_PLAYOBJECT)
                                    {
                                        SendMsg(this, Grobal2.RM_DURACHANGE, Grobal2.U_CHARM, m_UseItems[Grobal2.U_CHARM].Dura, m_UseItems[Grobal2.U_CHARM].DuraMax, 0, "");
                                    }
                                }
                                else
                                {
                                    m_UseItems[Grobal2.U_CHARM].Dura = 0;
                                    if (m_btRaceServer == Grobal2.RC_PLAYOBJECT)
                                    {
                                        (this as TPlayObject).SendDelItems(m_UseItems[Grobal2.U_CHARM]);
                                    }
                                    m_UseItems[Grobal2.U_CHARM].wIndex = 0;
                                }
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                M2Share.ErrorMessage(sExceptionMsg7);
            }
            // 血气石处理结束
            // TBaseObject.Run 3 清理目标对象
            try
            {
                if (m_TargetCret != null)
                {
                    //修复弓箭卫士在人物进入房间后再出来，还会攻击人物(人物的攻击目标没清除)
                    if (((HUtil32.GetTickCount() - m_dwTargetFocusTick) > 30000) || m_TargetCret.m_boDeath || m_TargetCret.m_boGhost || (m_TargetCret.m_PEnvir != m_PEnvir) || (Math.Abs(m_TargetCret.m_nCurrX - m_nCurrX) > 15) || (Math.Abs(m_TargetCret.m_nCurrY - m_nCurrY) > 15))
                    {
                        m_TargetCret = null;
                    }
                }
                if (m_LastHiter != null)
                {
                    if (((HUtil32.GetTickCount() - m_LastHiterTick) > 30000) || m_LastHiter.m_boDeath || m_LastHiter.m_boGhost)
                    {
                        m_LastHiter = null;
                    }
                }
                if (m_ExpHitter != null)
                {
                    if (((HUtil32.GetTickCount() - m_ExpHitterTick) > 6000) || m_ExpHitter.m_boDeath || m_ExpHitter.m_boGhost)
                    {
                        m_ExpHitter = null;
                    }
                }
                if (m_Master != null)
                {
                    m_boNoItem = true;
                    // 宝宝变色
                    int nInteger;
                    if (m_boAutoChangeColor && (HUtil32.GetTickCount() - m_dwAutoChangeColorTick > M2Share.g_Config.dwBBMonAutoChangeColorTime))
                    {
                        m_dwAutoChangeColorTick = HUtil32.GetTickCount();
                        switch (m_nAutoChangeIdx)
                        {
                            case 0:
                                nInteger = Grobal2.STATE_TRANSPARENT;
                                break;
                            case 1:
                                nInteger = Grobal2.POISON_STONE;
                                break;
                            case 2:
                                nInteger = Grobal2.POISON_DONTMOVE;
                                break;
                            case 3:
                                nInteger = Grobal2.POISON_68;
                                break;
                            case 4:
                                nInteger = Grobal2.POISON_DECHEALTH;
                                break;
                            case 5:
                                nInteger = Grobal2.POISON_LOCKSPELL;
                                break;
                            case 6:
                                nInteger = Grobal2.POISON_DAMAGEARMOR;
                                break;
                            default:
                                m_nAutoChangeIdx = 0;
                                nInteger = Grobal2.STATE_TRANSPARENT;
                                break;
                        }
                        m_nAutoChangeIdx++;
                        m_nCharStatus = (int)((m_nCharStatusEx & 0xFFFFF) | ((0x80000000 >> nInteger) | 0));
                        StatusChanged();
                    }
                    if (m_boFixColor && (m_nFixStatus != m_nCharStatus))
                    {
                        switch (m_nFixColorIdx)
                        {
                            case 0:
                                nInteger = Grobal2.STATE_TRANSPARENT;
                                break;
                            case 1:
                                nInteger = Grobal2.POISON_STONE;
                                break;
                            case 2:
                                nInteger = Grobal2.POISON_DONTMOVE;
                                break;
                            case 3:
                                nInteger = Grobal2.POISON_68;
                                break;
                            case 4:
                                nInteger = Grobal2.POISON_DECHEALTH;
                                break;
                            case 5:
                                nInteger = Grobal2.POISON_LOCKSPELL;
                                break;
                            case 6:
                                nInteger = Grobal2.POISON_DAMAGEARMOR;
                                break;
                            default:
                                m_nFixColorIdx = 0;
                                nInteger = Grobal2.STATE_TRANSPARENT;
                                break;
                        }
                        m_nCharStatus = (int)((m_nCharStatusEx & 0xFFFFF) | ((0x80000000 >> nInteger) | 0));
                        m_nFixStatus = m_nCharStatus;
                        StatusChanged();
                    }
                    // 宝宝在主人死亡后死亡处理
                    if (m_Master.m_boDeath && ((HUtil32.GetTickCount() - m_Master.m_dwDeathTick) > 1000))
                    {
                        if (M2Share.g_Config.boMasterDieMutiny && (m_Master.m_LastHiter != null) && (M2Share.RandomNumber.Random(M2Share.g_Config.nMasterDieMutinyRate) == 0))
                        {
                            m_Master = null;
                            m_btSlaveExpLevel = (byte)M2Share.g_Config.SlaveColor.GetUpperBound(0);
                            RecalcAbilitys();
                            m_WAbil.DC = HUtil32.MakeLong(HUtil32.LoWord(m_WAbil.DC) * M2Share.g_Config.nMasterDieMutinyPower, HUtil32.HiWord(m_WAbil.DC) * M2Share.g_Config.nMasterDieMutinyPower);
                            m_nWalkSpeed = m_nWalkSpeed / M2Share.g_Config.nMasterDieMutinySpeed;
                            RefNameColor();
                            RefShowName();
                        }
                        else
                        {
                            m_WAbil.HP = 0;
                        }
                    }
                    if (m_Master.m_boGhost && ((HUtil32.GetTickCount() - m_Master.m_dwGhostTick) > 1000))
                    {
                        MakeGhost();
                    }
                }
                // 清除宝宝列表中已经死亡及叛变的宝宝信息
                for (var i = m_SlaveList.Count - 1; i >= 0; i--)
                {
                    if (m_SlaveList[i].m_boDeath || m_SlaveList[i].m_boGhost || (m_SlaveList[i].m_Master != this))
                    {
                        m_SlaveList.RemoveAt(i);
                    }
                }
                if (m_boHolySeize && ((HUtil32.GetTickCount() - m_dwHolySeizeTick) > m_dwHolySeizeInterval))
                {
                    BreakHolySeizeMode();
                }
                if (m_boCrazyMode && ((HUtil32.GetTickCount() - m_dwCrazyModeTick) > m_dwCrazyModeInterval))
                {
                    BreakCrazyMode();
                }
                if (m_boShowHP && ((HUtil32.GetTickCount() - m_dwShowHPTick) > m_dwShowHPInterval))
                {
                    BreakOpenHealth();
                }
            }
            catch
            {
                M2Share.ErrorMessage(sExceptionMsg3);
            }
            try
            {
                // 减少PK值开始
                if ((HUtil32.GetTickCount() - m_dwDecPkPointTick) > M2Share.g_Config.dwDecPkPointTime)// 120000
                {
                    m_dwDecPkPointTick = HUtil32.GetTickCount();
                    if (m_nPkPoint > 0)
                    {
                        DecPKPoint(M2Share.g_Config.nDecPkPointCount);
                    }
                }
                if ((HUtil32.GetTickCount() - m_DecLightItemDrugTick) > M2Share.g_Config.dwDecLightItemDrugTime)
                {
                    m_DecLightItemDrugTick += M2Share.g_Config.dwDecLightItemDrugTime;
                    if (m_btRaceServer == Grobal2.RC_PLAYOBJECT)
                    {
                        UseLamp();
                        CheckPKStatus();
                    }
                }
                if ((HUtil32.GetTickCount() - m_dwCheckRoyaltyTick) > 10000)
                {
                    m_dwCheckRoyaltyTick = HUtil32.GetTickCount();
                    if (m_Master != null)
                    {
                        if ((M2Share.g_dwSpiritMutinyTick > HUtil32.GetTickCount()) && (m_btSlaveExpLevel < 5))
                        {
                            m_dwMasterRoyaltyTick = 0;
                        }
                        if (HUtil32.GetTickCount() > m_dwMasterRoyaltyTick)
                        {
                            for (var i = 0; i < m_Master.m_SlaveList.Count; i++)
                            {
                                if (m_Master.m_SlaveList[i] == this)
                                {
                                    m_Master.m_SlaveList.RemoveAt(i);
                                    break;
                                }
                            }
                            m_Master = null;
                            m_WAbil.HP = (ushort)(m_WAbil.HP / 10);
                            RefShowName();
                        }
                        if (m_dwMasterTick != 0)
                        {
                            if ((HUtil32.GetTickCount() - m_dwMasterTick) > 12 * 60 * 60 * 1000)
                            {
                                m_WAbil.HP = 0;
                            }
                        }
                    }
                }

                if ((HUtil32.GetTickCount() - m_dwVerifyTick) > 30 * 1000)
                {
                    m_dwVerifyTick = HUtil32.GetTickCount();
                    // 清组队已死亡成员
                    if (m_GroupOwner != null)
                    {
                        if (m_GroupOwner.m_boDeath || m_GroupOwner.m_boGhost)
                        {
                            m_GroupOwner = null;
                        }
                    }

                    if (m_GroupOwner == this)
                    {
                        for (var i = m_GroupMembers.Count - 1; i >= 0; i--)
                        {
                            TBaseObject BaseObject = m_GroupMembers[i];
                            if (BaseObject.m_boDeath || BaseObject.m_boGhost)
                            {
                                m_GroupMembers.RemoveAt(i);
                            }
                        }
                    }
                    // 检查交易双方 状态
                    if ((m_DealCreat != null) && m_DealCreat.m_boGhost)
                    {
                        m_DealCreat = null;
                    }
                    if (!m_boDenyRefStatus)
                    {
                        m_PEnvir.VerifyMapTime(m_nCurrX, m_nCurrY, this);// 刷新在地图上位置的时间
                    }
                }
            }
            catch (Exception e)
            {
                M2Share.ErrorMessage(sExceptionMsg4);
                M2Share.ErrorMessage(e.Message);
            }
            try
            {
                bool boChg = false;
                bool boNeedRecalc = false;
                for (var i = m_dwStatusArrTick.GetLowerBound(0); i <= m_dwStatusArrTick.GetUpperBound(0); i++)
                {
                    if ((m_wStatusTimeArr[i] > 0) && (m_wStatusTimeArr[i] < 60000))
                    {
                        if ((HUtil32.GetTickCount() - m_dwStatusArrTick[i]) > 1000)
                        {
                            m_wStatusTimeArr[i] -= 1;
                            m_dwStatusArrTick[i] += 1000;
                            if (m_wStatusTimeArr[i] == 0)
                            {
                                boChg = true;
                                switch (i)
                                {
                                    case Grobal2.STATE_TRANSPARENT:
                                        m_boHideMode = false;
                                        break;
                                    case Grobal2.STATE_DEFENCEUP:
                                        boNeedRecalc = true;
                                        SysMsg("Defense strength is back to normal.", MsgColor.Green, MsgType.Hint);
                                        break;
                                    case Grobal2.STATE_MAGDEFENCEUP:
                                        boNeedRecalc = true;
                                        SysMsg("Magical defense strength is back to normal.", MsgColor.Green, MsgType.Hint);
                                        break;
                                    case Grobal2.STATE_BUBBLEDEFENCEUP:
                                        m_boAbilMagBubbleDefence = false;
                                        break;
                                }
                            }
                        }
                    }
                }
                for (var i = m_wStatusArrValue.GetLowerBound(0); i <= m_wStatusArrValue.GetUpperBound(0); i++)
                {
                    if (m_wStatusArrValue[i] > 0)
                    {
                        if (HUtil32.GetTickCount() > m_dwStatusArrTimeOutTick[i])
                        {
                            m_wStatusArrValue[i] = 0;
                            boNeedRecalc = true;
                            switch (i)
                            {
                                case 0:
                                    SysMsg("Removed temporarily increased destructive power.", MsgColor.Green, MsgType.Hint);
                                    break;
                                case 1:
                                    SysMsg("Removed temporarily increased magic power.", MsgColor.Green, MsgType.Hint);
                                    break;
                                case 2:
                                    SysMsg("Removed temporarily increased zen power.", MsgColor.Green, MsgType.Hint);
                                    break;
                                case 3:
                                    SysMsg("Removed temporarily increased hitting speed.", MsgColor.Green, MsgType.Hint);
                                    break;
                                case 4:
                                    SysMsg("Removed temporarily increased HP.", MsgColor.Green, MsgType.Hint);
                                    break;
                                case 5:
                                    SysMsg("Removed temporarily increased MP.", MsgColor.Green, MsgType.Hint);
                                    break;
                                case 6:
                                    SysMsg("Removed temporarily decreased attack ability.", MsgColor.Green, MsgType.Hint);
                                    break;
                            }
                        }
                    }
                }
                if (boChg)
                {
                    m_nCharStatus = GetCharStatus();
                    StatusChanged();
                }
                if (boNeedRecalc)
                {
                    RecalcAbilitys();
                    SendMsg(this, Grobal2.RM_ABILITY, 0, 0, 0, 0, "");
                }
            }
            catch (Exception)
            {
                M2Share.ErrorMessage(sExceptionMsg5);
            }
            try
            {
                if ((HUtil32.GetTickCount() - m_dwPoisoningTick) > M2Share.g_Config.dwPosionDecHealthTime)
                {
                    m_dwPoisoningTick = HUtil32.GetTickCount();
                    if (m_wStatusTimeArr[Grobal2.POISON_DECHEALTH] > 0)
                    {
                        if (m_boAnimal)
                        {
                            m_nMeatQuality -= 1000;
                        }
                        DamageHealth(m_btGreenPoisoningPoint + 1);
                        m_nHealthTick = 0;
                        m_nSpellTick = 0;
                        HealthSpellChanged();
                    }
                }
            }
            catch
            {
                M2Share.ErrorMessage(sExceptionMsg6);
            }
            M2Share.g_nBaseObjTimeMin = HUtil32.GetTickCount() - dwRunTick;
            if (M2Share.g_nBaseObjTimeMax < M2Share.g_nBaseObjTimeMin)
            {
                M2Share.g_nBaseObjTimeMax = M2Share.g_nBaseObjTimeMin;
            }
        }

        public virtual void Die()
        {
            int tExp;
            bool tCheck;
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

            if (m_boCanReAlive)
            {
                if ((m_pMonGen != null) && (m_pMonGen.Envir != m_PEnvir))
                {
                    m_boCanReAlive = false;
                    if (m_pMonGen.nActiveCount > 0)
                    {
                        m_pMonGen.nActiveCount--;
                    }
                    m_pMonGen = null;
                }
            }

            m_nIncSpell = 0;
            m_nIncHealth = 0;
            m_nIncHealing = 0;
            KillFunc();
            try
            {
                if (m_btRaceServer != Grobal2.RC_PLAYOBJECT && m_LastHiter != null)
                {
                    if (M2Share.g_Config.boMonSayMsg)
                    {
                        MonsterSayMsg(m_LastHiter, MonStatus.Die);
                    }
                    if (m_ExpHitter != null)
                    {
                        if (m_ExpHitter.m_btRaceServer == Grobal2.RC_PLAYOBJECT)
                        {
                            if (M2Share.g_FunctionNPC != null)
                            {
                                M2Share.g_FunctionNPC.GotoLable(m_ExpHitter as TPlayObject, "@PlayKillMob", false);
                            }
                            tExp = m_ExpHitter.CalcGetExp(m_Abil.Level, m_dwFightExp);
                            if (!M2Share.g_Config.boVentureServer)
                            {
                                if (m_ExpHitter.m_boAI)
                                {
                                    (m_ExpHitter as TAIPlayObject).GainExp(tExp);
                                }
                                else
                                {
                                    (m_ExpHitter as TPlayObject).GainExp(tExp);
                                }
                            }
                            // 是否执行任务脚本
                            if (m_PEnvir.IsCheapStuff())
                            {
                                Merchant QuestNPC;
                                if (m_ExpHitter.m_GroupOwner != null)
                                {
                                    for (var i = 0; i < m_ExpHitter.m_GroupOwner.m_GroupMembers.Count; i++)
                                    {
                                        TPlayObject GroupHuman = m_ExpHitter.m_GroupOwner.m_GroupMembers[i];
                                        if (!GroupHuman.m_boDeath && m_ExpHitter.m_PEnvir == GroupHuman.m_PEnvir && Math.Abs(m_ExpHitter.m_nCurrX - GroupHuman.m_nCurrX) <= 12 && Math.Abs(m_ExpHitter.m_nCurrX - GroupHuman.m_nCurrX) <= 12 && m_ExpHitter == GroupHuman)
                                        {
                                            tCheck = false;
                                        }
                                        else
                                        {
                                            tCheck = true;
                                        }
                                        QuestNPC = (Merchant)m_PEnvir.GetQuestNPC(GroupHuman, m_sCharName, "", tCheck);
                                        if (QuestNPC != null)
                                        {
                                            QuestNPC.Click(GroupHuman);
                                        }
                                    }
                                }
                                QuestNPC = (Merchant)m_PEnvir.GetQuestNPC(m_ExpHitter, m_sCharName, "", false);
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
                                    if (m_ExpHitter.m_Master.m_boAI)
                                    {
                                        (m_ExpHitter.m_Master as TAIPlayObject).GainExp(tExp);
                                    }
                                    else
                                    {
                                        (m_ExpHitter.m_Master as TPlayObject).GainExp(tExp);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (m_LastHiter.m_btRaceServer == Grobal2.RC_PLAYOBJECT)
                        {
                            if (M2Share.g_FunctionNPC != null)
                            {
                                M2Share.g_FunctionNPC.GotoLable(m_LastHiter as TPlayObject, "@PlayKillMob", false);
                            }
                            tExp = m_LastHiter.CalcGetExp(m_Abil.Level, m_dwFightExp);
                            if (!M2Share.g_Config.boVentureServer)
                            {
                                if (m_LastHiter.m_boAI)
                                {
                                    (m_LastHiter as TAIPlayObject).GainExp(tExp);
                                }
                                else
                                {
                                    (m_LastHiter as TPlayObject).GainExp(tExp);
                                }
                            }
                        }
                    }
                }
                if (M2Share.g_Config.boMonSayMsg && m_btRaceServer == Grobal2.RC_PLAYOBJECT && m_LastHiter != null)
                {
                    m_LastHiter.MonsterSayMsg(this, MonStatus.KillHuman);
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
                var boPK = false;
                if (!M2Share.g_Config.boVentureServer && !m_PEnvir.Flag.boFightZone && !m_PEnvir.Flag.boFight3Zone)
                {
                    if (m_btRaceServer == Grobal2.RC_PLAYOBJECT && m_LastHiter != null && PKLevel() < 2)
                    {
                        if ((m_LastHiter.m_btRaceServer == Grobal2.RC_PLAYOBJECT) || (m_LastHiter.m_btRaceServer == Grobal2.RC_NPC))//允许NPC杀死人物
                        {
                            boPK = true;
                        }
                        if (m_LastHiter.m_Master != null)
                        {
                            if (m_LastHiter.m_Master.m_btRaceServer == Grobal2.RC_PLAYOBJECT)
                            {
                                m_LastHiter = m_LastHiter.m_Master;
                                boPK = true;
                            }
                        }
                    }
                }
                if (boPK && m_LastHiter != null)
                {
                    var guildwarkill = false;
                    if (m_MyGuild != null && m_LastHiter.m_MyGuild != null)
                    {
                        if (GetGuildRelation(this, m_LastHiter) == 2)
                        {
                            guildwarkill = true;
                        }
                    }
                    var Castle = M2Share.CastleManager.InCastleWarArea(this);
                    if (Castle != null && Castle.m_boUnderWar || m_boInFreePKArea)
                    {
                        guildwarkill = true;
                    }
                    if (!guildwarkill)
                    {
                        if ((M2Share.g_Config.boKillHumanWinLevel || M2Share.g_Config.boKillHumanWinExp || m_PEnvir.Flag.boPKWINLEVEL || m_PEnvir.Flag.boPKWINEXP) && m_LastHiter.m_btRaceServer == Grobal2.RC_PLAYOBJECT)
                        {
                            (this as TPlayObject).PKDie(m_LastHiter as TPlayObject);
                        }
                        else
                        {
                            if (!m_LastHiter.IsGoodKilling(this))
                            {
                                m_LastHiter.IncPkPoint(M2Share.g_Config.nKillHumanAddPKPoint);
                                m_LastHiter.SysMsg(M2Share.g_sYouMurderedMsg, MsgColor.Red, MsgType.Hint);
                                SysMsg(format(M2Share.g_sYouKilledByMsg, m_LastHiter.m_sCharName), MsgColor.Red, MsgType.Hint);
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
                                m_LastHiter.SysMsg(M2Share.g_sYouProtectedByLawOfDefense, MsgColor.Green, MsgType.Hint);
                            }
                        }
                        // 检查攻击人是否用了着经验或等级装备
                        if (m_LastHiter.m_btRaceServer == Grobal2.RC_PLAYOBJECT)
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
                    var AttackBaseObject = m_ExpHitter;
                    if (m_ExpHitter != null && m_ExpHitter.m_Master != null)
                    {
                        AttackBaseObject = m_ExpHitter.m_Master;
                    }
                    if (m_btRaceServer != Grobal2.RC_PLAYOBJECT)
                    {
                        DropUseItems(AttackBaseObject);
                        if (m_Master == null && (!m_boNoItem || !m_PEnvir.Flag.boNODROPITEM))
                        {
                            ScatterBagItems(AttackBaseObject);
                        }
                        if (m_btRaceServer >= Grobal2.RC_ANIMAL && m_Master == null && (!m_boNoItem || !m_PEnvir.Flag.boNODROPITEM))
                        {
                            ScatterGolds(AttackBaseObject);
                        }
                    }
                    else
                    {
                        if (!m_boNoItem || !m_PEnvir.Flag.boNODROPITEM)//允许设置 m_boNoItem 后人物死亡不掉物品
                        {
                            if (AttackBaseObject != null)
                            {
                                if (M2Share.g_Config.boKillByHumanDropUseItem && AttackBaseObject.m_btRaceServer == Grobal2.RC_PLAYOBJECT || M2Share.g_Config.boKillByMonstDropUseItem && AttackBaseObject.m_btRaceServer != Grobal2.RC_PLAYOBJECT)
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
                string tStr;
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
                            tStr = m_LastHiter.m_MyGuild.sGuildName + ':' + m_LastHiter.m_MyGuild.nContestPoint + "  " + m_MyGuild.sGuildName + ':' + m_MyGuild.nContestPoint;
                            M2Share.UserEngine.CryCry(Grobal2.RM_CRY, m_PEnvir, m_nCurrX, m_nCurrY, 1000, M2Share.g_Config.btCryMsgFColor, M2Share.g_Config.btCryMsgBColor, "- " + tStr);
                        }
                    }
                }
                if (m_btRaceServer == Grobal2.RC_PLAYOBJECT)
                {
                    if (m_GroupOwner != null)
                    {
                        m_GroupOwner.DelMember(this);// 人物死亡立即退组，以防止组队刷经验
                    }
                    if (m_LastHiter != null)
                    {
                        if (m_LastHiter.m_btRaceServer == Grobal2.RC_PLAYOBJECT)
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
                SendRefMsg(Grobal2.RM_DEATH, m_btDirection, m_nCurrX, m_nCurrY, 1, "");
            }
            catch
            {
                M2Share.ErrorMessage(sExceptionMsg3);
            }
        }

        internal virtual void ReAlive()
        {
            m_boDeath = false;
            SendRefMsg(Grobal2.RM_ALIVE, m_btDirection, m_nCurrX, m_nCurrY, 0, "");
        }

        protected virtual bool IsProtectTarget(TBaseObject BaseObject)
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
                    if (m_Abil.Level <= M2Share.g_Config.nPKProtectLevel)// 如果小于指定等级
                    {
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
                if (((HUtil32.GetTickCount() - m_dwMapMoveTick) < 3000) || ((HUtil32.GetTickCount() - BaseObject.m_dwMapMoveTick) < 3000))
                {
                    result = false;
                }
            }
            return result;
        }

        protected virtual void ProcessSayMsg(string sMsg)
        {
            string sCharName;
            if (m_btRaceServer == Grobal2.RC_PLAYOBJECT)
            {
                sCharName = m_sCharName;
            }
            else
            {
                sCharName = M2Share.FilterShowName(m_sCharName);
            }
            SendRefMsg(Grobal2.RM_HEAR, 0, M2Share.g_Config.btHearMsgFColor, M2Share.g_Config.btHearMsgBColor, 0, sCharName + ':' + sMsg);
        }

        public virtual void MakeGhost()
        {
            if (m_boCanReAlive)
            {
                m_boInvisible = true;
                m_dwGhostTick = HUtil32.GetTickCount();
                m_PEnvir.DeleteFromMap(m_nCurrX, m_nCurrY, CellType.OS_MOVINGOBJECT, this);
                SendRefMsg(Grobal2.RM_DISAPPEAR, 0, 0, 0, 0, "");
            }
            else
            {
                m_boGhost = true;
                m_dwGhostTick = HUtil32.GetTickCount();
                DisappearA();
            }
        }

        /// <summary>
        /// 散落包裹物品
        /// </summary>
        /// <param name="ItemOfCreat"></param>
        protected virtual void ScatterBagItems(TBaseObject ItemOfCreat)
        {
            TUserItem UserItem;
            GoodItem StdItem;
            const string sExceptionMsg = "[Exception] TBaseObject::ScatterBagItems";
            try
            {
                var DropWide = HUtil32._MIN(M2Share.g_Config.nDropItemRage, 7);
                if ((m_btRaceServer == Grobal2.RC_PLAYCLONE) && (m_Master != null))
                {
                    return;
                }
                for (var i = m_ItemList.Count - 1; i >= 0; i--)
                {
                    UserItem = m_ItemList[i];
                    StdItem = M2Share.UserEngine.GetStdItem(UserItem.wIndex);
                    var boCanNotDrop = false;
                    if (StdItem != null)
                    {
                        TMonDrop MonDrop = null;
                        if (M2Share.g_MonDropLimitLIst.TryGetValue(StdItem.Name, out MonDrop))
                        {
                            if (MonDrop.nDropCount < MonDrop.nCountLimit)
                            {
                                MonDrop.nDropCount++;
                                M2Share.g_MonDropLimitLIst[StdItem.Name] = MonDrop;
                            }
                            else
                            {
                                MonDrop.nNoDropCount++;
                                boCanNotDrop = true;
                            }
                            break;
                        }
                    }
                    if (boCanNotDrop)
                    {
                        continue;
                    }
                    if (DropItemDown(UserItem, DropWide, true, ItemOfCreat, this))
                    {
                        Dispose(UserItem);
                        m_ItemList.RemoveAt(i);
                    }
                }
            }
            catch
            {
                M2Share.ErrorMessage(sExceptionMsg);
            }
        }

        protected virtual void DropUseItems(TBaseObject BaseObject)
        {
            int nC;
            int nRate;
            GoodItem StdItem;
            IList<TDeleteItem> DropItemList = null;
            const string sExceptionMsg = "[Exception] TBaseObject::DropUseItems";
            try
            {
                if (m_boNoDropUseItem)
                {
                    return;
                }
                if (m_btRaceServer == Grobal2.RC_PLAYOBJECT)
                {
                    nC = 0;
                    while (true)
                    {
                        if (m_UseItems[nC] == null)
                        {
                            nC++;
                            continue;
                        }
                        StdItem = M2Share.UserEngine.GetStdItem(m_UseItems[nC].wIndex);
                        if (StdItem != null)
                        {
                            if ((StdItem.Reserved & 8) != 0)
                            {
                                if (DropItemList == null)
                                {
                                    DropItemList = new List<TDeleteItem>();
                                }
                                DropItemList.Add(new TDeleteItem()
                                {
                                    MakeIndex = m_UseItems[nC].MakeIndex
                                });
                                if (StdItem.NeedIdentify == 1)
                                {
                                    M2Share.AddGameDataLog("16" + "\t" + m_sMapName + "\t" + m_nCurrX + "\t" + m_nCurrY + "\t" + m_sCharName + "\t" + StdItem.Name + "\t" + m_UseItems[nC].MakeIndex + "\t" + HUtil32.BoolToIntStr(m_btRaceServer == Grobal2.RC_PLAYOBJECT) + "\t" + '0');
                                }
                                m_UseItems[nC].wIndex = 0;
                            }
                        }
                        nC++;
                        if (nC >= 9)
                        {
                            break;
                        }
                    }
                }
                if (PKLevel() > 2)
                {
                    nRate = 15;
                }
                else
                {
                    nRate = 30;
                }
                nC = 0;
                while (true)
                {
                    if (M2Share.RandomNumber.Random(nRate) == 0)
                    {
                        if (m_UseItems[nC] == null)
                        {
                            nC++;
                            continue;
                        }
                        if (DropItemDown(m_UseItems[nC], 2, true, BaseObject, this))
                        {
                            StdItem = M2Share.UserEngine.GetStdItem(m_UseItems[nC].wIndex);
                            if (StdItem != null)
                            {
                                if ((StdItem.Reserved & 10) == 0)
                                {
                                    if (m_btRaceServer == Grobal2.RC_PLAYOBJECT)
                                    {
                                        if (DropItemList == null)
                                        {
                                            DropItemList = new List<TDeleteItem>();
                                        }
                                        DropItemList.Add(new TDeleteItem()
                                        {
                                            sItemName = M2Share.UserEngine.GetStdItemName(m_UseItems[nC].wIndex),
                                            MakeIndex = m_UseItems[nC].MakeIndex
                                        });
                                    }
                                    m_UseItems[nC].wIndex = 0;
                                }
                            }
                        }
                    }
                    nC++;
                    if (nC >= 9)
                    {
                        break;
                    }
                }
                if (DropItemList != null)
                {
                    var ObjectId = HUtil32.Sequence();
                    M2Share.ObjectManager.AddOhter(ObjectId, DropItemList);
                    SendMsg(this, Grobal2.RM_SENDDELITEMLIST, 0, ObjectId, 0, 0, "");
                }
            }
            catch
            {
                M2Share.ErrorMessage(sExceptionMsg);
            }
        }

        public virtual void SetTargetCreat(TBaseObject BaseObject)
        {
            m_TargetCret = BaseObject;
            m_dwTargetFocusTick = HUtil32.GetTickCount();
        }

        protected virtual void DelTargetCreat()
        {
            m_TargetCret = null;
        }

        public virtual bool IsProperFriend(TBaseObject BaseObject)
        {
            bool result = false;
            if (BaseObject == null)
            {
                return result;
            }
            if (m_btRaceServer >= Grobal2.RC_ANIMAL)
            {
                if (BaseObject.m_btRaceServer >= Grobal2.RC_ANIMAL)
                {
                    result = true;
                }
                if (BaseObject.m_Master != null)
                {
                    result = false;
                }
                return result;
            }
            if (m_btRaceServer == Grobal2.RC_PLAYOBJECT)
            {
                result = IsProperFriend_IsFriend(BaseObject);
                if (BaseObject.m_btRaceServer < Grobal2.RC_ANIMAL)
                {
                    return result;
                }
                if (BaseObject.m_Master == this)
                {
                    result = true;
                    return result;
                }
                if (BaseObject.m_Master != null)
                {
                    result = IsProperFriend_IsFriend(BaseObject.m_Master);
                    return result;
                }
            }
            else
            {
                result = true;
            }
            return result;
        }

        public virtual bool Operate(TProcessMessage ProcessMsg)
        {
            int nDamage;
            int nTargetX;
            int nTargetY;
            int nPower;
            int nRage;
            TBaseObject TargetBaseObject;
            const string sExceptionMsg = "[Exception] TBaseObject::Operate ";
            bool result = false;
            try
            {
                switch (ProcessMsg.wIdent)
                {
                    case Grobal2.RM_MAGSTRUCK:
                    case Grobal2.RM_MAGSTRUCK_MINE:
                        if ((ProcessMsg.wIdent == Grobal2.RM_MAGSTRUCK) && (m_btRaceServer >= Grobal2.RC_ANIMAL) && !bo2BF && (m_Abil.Level < 50))
                        {
                            m_dwWalkTick = m_dwWalkTick + 800 + M2Share.RandomNumber.Random(1000);
                        }
                        nDamage = GetMagStruckDamage(null, ProcessMsg.nParam1);
                        if (nDamage > 0)
                        {
                            StruckDamage(nDamage);
                            HealthSpellChanged();
                            SendRefMsg(Grobal2.RM_STRUCK_MAG, (short)nDamage, m_WAbil.HP, m_WAbil.MaxHP, ProcessMsg.BaseObject, "");
                            TargetBaseObject = M2Share.ObjectManager.Get(ProcessMsg.BaseObject);
                            if (M2Share.g_Config.boMonDelHptoExp)
                            {
                                if (TargetBaseObject.m_btRaceServer == Grobal2.RC_PLAYOBJECT)
                                {
                                    if ((TargetBaseObject as TPlayObject).m_WAbil.Level <= M2Share.g_Config.MonHptoExpLevel)
                                    {
                                        if (!M2Share.GetNoHptoexpMonList(m_sCharName))
                                        {
                                            if (TargetBaseObject.m_boAI)
                                            {
                                                (TargetBaseObject as TAIPlayObject).GainExp(GetMagStruckDamage(TargetBaseObject, nDamage) * M2Share.g_Config.MonHptoExpmax);
                                            }
                                            else
                                            {
                                                (TargetBaseObject as TPlayObject).GainExp(GetMagStruckDamage(TargetBaseObject, nDamage) * M2Share.g_Config.MonHptoExpmax);
                                            }
                                        }
                                    }
                                }
                                if (TargetBaseObject.m_btRaceServer == Grobal2.RC_PLAYCLONE)
                                {
                                    if (TargetBaseObject.m_Master != null)
                                    {
                                        if ((TargetBaseObject.m_Master as TPlayObject).m_WAbil.Level <= M2Share.g_Config.MonHptoExpLevel)
                                        {
                                            if (!M2Share.GetNoHptoexpMonList(m_sCharName))
                                            {
                                                if (TargetBaseObject.m_Master.m_boAI)
                                                {
                                                    (TargetBaseObject.m_Master as TAIPlayObject).GainExp(GetMagStruckDamage(TargetBaseObject, nDamage) * M2Share.g_Config.MonHptoExpmax);
                                                }
                                                else
                                                {
                                                    (TargetBaseObject.m_Master as TPlayObject).GainExp(GetMagStruckDamage(TargetBaseObject, nDamage) * M2Share.g_Config.MonHptoExpmax);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (m_btRaceServer != Grobal2.RC_PLAYOBJECT)
                            {
                                if (m_boAnimal)
                                {
                                    m_nMeatQuality -= (ushort)(nDamage * 1000);
                                }
                                SendMsg(this, Grobal2.RM_STRUCK, nDamage, m_WAbil.HP, m_WAbil.MaxHP, ProcessMsg.BaseObject, "");
                            }
                        }
                        if (m_boFastParalysis)
                        {
                            m_wStatusTimeArr[Grobal2.POISON_STONE] = 1;
                            m_boFastParalysis = false;
                        }
                        break;
                    case Grobal2.RM_MAGHEALING:
                        if ((m_nIncHealing + ProcessMsg.nParam1) < 300)
                        {
                            if (m_btRaceServer == Grobal2.RC_PLAYOBJECT)
                            {
                                m_nIncHealing += ProcessMsg.nParam1;
                                m_nPerHealing = 5;
                            }
                            else
                            {
                                m_nIncHealing += ProcessMsg.nParam1;
                                m_nPerHealing = 5;
                            }
                        }
                        else
                        {
                            m_nIncHealing = 300;
                        }
                        break;
                    case Grobal2.RM_10101:
                        SendRefMsg(ProcessMsg.BaseObject, ProcessMsg.wParam, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, ProcessMsg.sMsg);
                        if ((ProcessMsg.BaseObject == Grobal2.RM_STRUCK) && (m_btRaceServer != Grobal2.RC_PLAYOBJECT))
                        {
                            SendMsg(this, ProcessMsg.BaseObject, ProcessMsg.wParam, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, ProcessMsg.sMsg);
                        }
                        if (m_boFastParalysis)
                        {
                            m_wStatusTimeArr[Grobal2.POISON_STONE] = 1;
                            m_boFastParalysis = false;
                        }
                        break;
                    case Grobal2.RM_DELAYMAGIC:
                        nPower = ProcessMsg.wParam;
                        nTargetX = HUtil32.LoWord(ProcessMsg.nParam1);
                        nTargetY = HUtil32.HiWord(ProcessMsg.nParam1);
                        nRage = ProcessMsg.nParam2;
                        TargetBaseObject = M2Share.ObjectManager.Get(ProcessMsg.nParam3);
                        if ((TargetBaseObject != null) && (TargetBaseObject.GetMagStruckDamage(this, nPower) > 0))
                        {
                            SetTargetCreat(TargetBaseObject);
                            if (TargetBaseObject.m_btRaceServer >= Grobal2.RC_ANIMAL)
                            {
                                nPower = HUtil32.Round(nPower / 1.2);
                            }
                            if ((Math.Abs(nTargetX - TargetBaseObject.m_nCurrX) <= nRage) && (Math.Abs(nTargetY - TargetBaseObject.m_nCurrY) <= nRage))
                            {
                                TargetBaseObject.SendMsg(this, Grobal2.RM_MAGSTRUCK, 0, nPower, 0, 0, "");
                            }
                        }
                        break;
                    case Grobal2.RM_10155:
                        MapRandomMove(ProcessMsg.sMsg, ProcessMsg.wParam);
                        break;
                    case Grobal2.RM_DELAYPUSHED:
                        nPower = ProcessMsg.wParam;
                        nTargetX = HUtil32.LoWord(ProcessMsg.nParam1);
                        nTargetY = HUtil32.HiWord(ProcessMsg.nParam1);
                        nRage = ProcessMsg.nParam2;
                        TargetBaseObject = M2Share.ObjectManager.Get(ProcessMsg.nParam3);// M2Share.ObjectSystem.Get(ProcessMsg.nParam3);
                        if (TargetBaseObject != null)
                        {
                            TargetBaseObject.CharPushed((byte)nPower, nRage);
                        }
                        break;
                    case Grobal2.RM_POISON:
                        TargetBaseObject = M2Share.ObjectManager.Get(ProcessMsg.nParam2);// ((ProcessMsg.nParam2) as TBaseObject);
                        if (TargetBaseObject != null)
                        {
                            if (IsProperTarget(TargetBaseObject))
                            {
                                SetTargetCreat(TargetBaseObject);
                                if ((m_btRaceServer == Grobal2.RC_PLAYOBJECT) && (TargetBaseObject.m_btRaceServer == Grobal2.RC_PLAYOBJECT))
                                {
                                    SetPKFlag(TargetBaseObject);
                                }
                                SetLastHiter(TargetBaseObject);
                            }
                            MakePosion(ProcessMsg.wParam, ProcessMsg.nParam1, ProcessMsg.nParam3);// 中毒类型
                        }
                        else
                        {
                            MakePosion(ProcessMsg.wParam, ProcessMsg.nParam1, ProcessMsg.nParam3);// 中毒类型
                        }
                        break;
                    case Grobal2.RM_TRANSPARENT:
                        M2Share.MagicManager.MagMakePrivateTransparent(this, ProcessMsg.nParam1);
                        break;
                    case Grobal2.RM_DOOPENHEALTH:
                        MakeOpenHealth();
                        break;
                        /*default:
                            Debug.WriteLine(format("人物: {0} 消息: Ident {1} Param {2} P1 {3} P2 {3} P3 {4} Msg {5}", m_sCharName, ProcessMsg.wIdent, ProcessMsg.wParam, ProcessMsg.nParam1, ProcessMsg.nParam2, ProcessMsg.nParam3, ProcessMsg.sMsg));
                            break;*/
                }
            }
            catch (Exception e)
            {
                M2Share.ErrorMessage(sExceptionMsg);
                M2Share.ErrorMessage(e.Message);
            }
            return result;
        }

        public virtual string GetShowName()
        {
            var sShowName = m_sCharName;
            var result = M2Share.FilterShowName(sShowName);
            if ((m_Master != null) && !m_Master.m_boObMode)
            {
                result = result + '(' + m_Master.m_sCharName + ')';
            }
            return result;
        }

        /// <summary>
        /// 计算自身属性
        /// </summary>
        public virtual void RecalcAbilitys()
        {
            GoodItem StdItem;
            bool[] boRecallSuite = new bool[4] { false, false, false, false };
            bool[] boMoXieSuite = new bool[3] { false, false, false };
            bool[] boSpirit = new bool[4] { false, false, false, false };
            m_AddAbil = new TAddAbility();
            ushort wOldHP = m_WAbil.HP;
            ushort wOldMP = m_WAbil.MP;
            m_WAbil = m_Abil;
            m_WAbil.HP = wOldHP;
            m_WAbil.MP = wOldMP;
            m_WAbil.Weight = 0;
            m_WAbil.WearWeight = 0;
            m_WAbil.HandWeight = 0;
            m_btAntiPoison = 0;
            m_nPoisonRecover = 0;
            m_nHealthRecover = 0;
            m_nSpellRecover = 0;
            m_nAntiMagic = 1;
            m_nLuck = 0;
            m_nHitSpeed = 0;
            m_boExpItem = false;
            m_rExpItem = 0;
            m_boPowerItem = false;
            m_rPowerItem = 0;
            m_boHideMode = false;
            m_boTeleport = false;
            m_boParalysis = false;
            m_boRevival = false;
            m_boUnRevival = false;
            m_boFlameRing = false;
            m_boRecoveryRing = false;
            m_boAngryRing = false;
            m_boMagicShield = false;
            m_boUnMagicShield = false;
            m_boMuscleRing = false;
            m_boFastTrain = false;
            m_boProbeNecklace = false;
            m_boSupermanItem = false;
            m_boGuildMove = false;
            m_boUnParalysis = false;
            m_boExpItem = false;
            m_boPowerItem = false;
            m_boNoDropItem = false;
            m_boNoDropUseItem = false;
            m_bopirit = false;
            m_btHorseType = 0;
            m_btDressEffType = 0;
            m_nAutoAddHPMPMode = 0;
            // 气血石
            m_nMoXieSuite = 0;
            m_db3B0 = 0;
            m_nHongMoSuite = 0;
            bool boHongMoSuite1 = false;
            bool boHongMoSuite2 = false;
            bool boHongMoSuite3 = false;
            m_boRecallSuite = false;
            m_boSmashSet = false;
            bool boSmash1 = false;
            bool boSmash2 = false;
            bool boSmash3 = false;
            m_boHwanDevilSet = false;
            bool boHwanDevil1 = false;
            bool boHwanDevil2 = false;
            bool boHwanDevil3 = false;
            m_boPuritySet = false;
            bool boPurity1 = false;
            bool boPurity2 = false;
            bool boPurity3 = false;
            m_boMundaneSet = false;
            bool boMundane1 = false;
            bool boMundane2 = false;
            m_boNokChiSet = false;
            bool boNokChi1 = false;
            bool boNokChi2 = false;
            m_boTaoBuSet = false;
            bool boTaoBu1 = false;
            bool boTaoBu2 = false;
            m_boFiveStringSet = false;
            bool boFiveString1 = false;
            bool boFiveString2 = false;
            bool boFiveString3 = false;
            bool boOldHideMode = m_boHideMode;
            m_dwPKDieLostExp = 0;
            m_nPKDieLostLevel = 0;
            for (var i = m_UseItems.GetLowerBound(0); i <= m_UseItems.GetUpperBound(0); i++)
            {
                if (m_UseItems[i] == null)
                {
                    continue;
                }
                if ((m_UseItems[i].wIndex <= 0) || (m_UseItems[i].Dura <= 0))
                {
                    continue;
                }
                StdItem = M2Share.UserEngine.GetStdItem(m_UseItems[i].wIndex);
                if (StdItem == null)
                {
                    continue;
                }
                StdItem.ApplyItemParameters(ref m_AddAbil);
                if ((i == Grobal2.U_WEAPON) || (i == Grobal2.U_RIGHTHAND) || (i == Grobal2.U_DRESS))
                {
                    if (i == Grobal2.U_DRESS)
                    {
                        m_WAbil.WearWeight += StdItem.Weight;
                    }
                    else
                    {
                        m_WAbil.HandWeight += StdItem.Weight;
                    }
                    // 新增开始
                    if (StdItem.AniCount == 120)
                    {
                        m_boFastTrain = true;
                    }
                    if (StdItem.AniCount == 121)
                    {
                        m_boProbeNecklace = true;
                    }
                    if (StdItem.AniCount == 145)
                    {
                        m_boGuildMove = true;
                    }
                    if (StdItem.AniCount == 111)
                    {
                        m_wStatusTimeArr[Grobal2.STATE_TRANSPARENT] = 6 * 10 * 1000;
                        m_boHideMode = true;
                    }
                    if (StdItem.AniCount == 112)
                    {
                        m_boTeleport = true;
                    }
                    if (StdItem.AniCount == 113)
                    {
                        m_boParalysis = true;
                    }
                    if (StdItem.AniCount == 114)
                    {
                        m_boRevival = true;
                    }
                    if (StdItem.AniCount == 115)
                    {
                        m_boFlameRing = true;
                    }
                    if (StdItem.AniCount == 116)
                    {
                        m_boRecoveryRing = true;
                    }
                    if (StdItem.AniCount == 117)
                    {
                        m_boAngryRing = true;
                    }
                    if (StdItem.AniCount == 118)
                    {
                        m_boMagicShield = true;
                    }
                    if (StdItem.AniCount == 119)
                    {
                        m_boMuscleRing = true;
                    }
                    if (StdItem.AniCount == 135)
                    {
                        boMoXieSuite[0] = true;
                        m_nMoXieSuite += StdItem.Weight / 10;
                    }
                    if (StdItem.AniCount == 138)
                    {
                        m_nHongMoSuite += StdItem.Weight;
                    }
                    if (StdItem.AniCount == 139)
                    {
                        m_boUnParalysis = true;
                    }
                    if (StdItem.AniCount == 140)
                    {
                        m_boSupermanItem = true;
                    }
                    if (StdItem.AniCount == 141)
                    {
                        m_boExpItem = true;
                        m_rExpItem = m_rExpItem + (m_UseItems[i].Dura / M2Share.g_Config.nItemExpRate);
                    }
                    if (StdItem.AniCount == 142)
                    {
                        m_boPowerItem = true;
                        m_rPowerItem = m_rPowerItem + (m_UseItems[i].Dura / M2Share.g_Config.nItemPowerRate);
                    }
                    if (StdItem.AniCount == 182)
                    {
                        m_boExpItem = true;
                        m_rExpItem = m_rExpItem + (m_UseItems[i].DuraMax / M2Share.g_Config.nItemExpRate);
                    }
                    if (StdItem.AniCount == 183)
                    {
                        m_boPowerItem = true;
                        m_rPowerItem = m_rPowerItem + (m_UseItems[i].DuraMax / M2Share.g_Config.nItemPowerRate);
                    }
                    if (StdItem.AniCount == 143)
                    {
                        m_boUnMagicShield = true;
                    }
                    if (StdItem.AniCount == 144)
                    {
                        m_boUnRevival = true;
                    }
                    if (StdItem.AniCount == 170)
                    {
                        m_boAngryRing = true;
                    }
                    if (StdItem.AniCount == 171)
                    {
                        m_boNoDropItem = true;
                    }
                    if (StdItem.AniCount == 172)
                    {
                        m_boNoDropUseItem = true;
                    }
                    if (StdItem.AniCount == 150)
                    {
                        // 麻痹护身
                        m_boParalysis = true;
                        m_boMagicShield = true;
                    }
                    if (StdItem.AniCount == 151)
                    {
                        // 麻痹火球
                        m_boParalysis = true;
                        m_boFlameRing = true;
                    }
                    if (StdItem.AniCount == 152)
                    {
                        // 麻痹防御
                        m_boParalysis = true;
                        m_boRecoveryRing = true;
                    }
                    if (StdItem.AniCount == 153)
                    {
                        // 麻痹负载
                        m_boParalysis = true;
                        m_boMuscleRing = true;
                    }
                    if (StdItem.Shape == 154)
                    {
                        // 护身火球
                        m_boMagicShield = true;
                        m_boFlameRing = true;
                    }
                    if (StdItem.AniCount == 155)
                    {
                        // 护身防御
                        m_boMagicShield = true;
                        m_boRecoveryRing = true;
                    }
                    if (StdItem.AniCount == 156)
                    {
                        // 护身负载
                        m_boMagicShield = true;
                        m_boMuscleRing = true;
                    }
                    if (StdItem.AniCount == 157)
                    {
                        // 传送麻痹
                        m_boTeleport = true;
                        m_boParalysis = true;
                    }
                    if (StdItem.AniCount == 158)
                    {
                        // 传送护身
                        m_boTeleport = true;
                        m_boMagicShield = true;
                    }
                    if (StdItem.AniCount == 159)
                    {
                        // 传送探测
                        m_boTeleport = true;
                        m_boProbeNecklace = true;
                    }
                    if (StdItem.AniCount == 160)
                    {
                        // 传送复活
                        m_boTeleport = true;
                        m_boRevival = true;
                    }
                    if (StdItem.AniCount == 161)
                    {
                        // 麻痹复活
                        m_boParalysis = true;
                        m_boRevival = true;
                    }
                    if (StdItem.AniCount == 162)
                    {
                        // 护身复活
                        m_boMagicShield = true;
                        m_boRevival = true;
                    }
                    if (StdItem.AniCount == 180)
                    {
                        // PK 死亡掉经验
                        m_dwPKDieLostExp = StdItem.DuraMax * M2Share.g_Config.dwPKDieLostExpRate;
                    }
                    if (StdItem.AniCount == 181)
                    {
                        // PK 死亡掉等级
                        m_nPKDieLostLevel = StdItem.DuraMax / M2Share.g_Config.nPKDieLostLevelRate;
                    }
                    // 新增结束
                }
                else
                {
                    m_WAbil.WearWeight += StdItem.Weight;
                }
                m_WAbil.Weight += StdItem.Weight;
                if (i == Grobal2.U_WEAPON)
                {
                    if ((StdItem.Source - 1 - 10) < 0)
                    {
                        m_AddAbil.btWeaponStrong = (byte)StdItem.Source;// 强度+
                    }
                    if ((StdItem.Source <= -1) && (StdItem.Source >= -50))  // -1 to -50
                    {
                        m_AddAbil.btUndead = (byte)(m_AddAbil.btUndead + -StdItem.Source);// Holy+
                    }
                    if ((StdItem.Source <= -51) && (StdItem.Source >= -100))// -51 to -100
                    {
                        m_AddAbil.btUndead = (byte)(m_AddAbil.btUndead + (StdItem.Source + 50));// Holy-
                    }
                    continue;
                }
                if (i == Grobal2.U_RIGHTHAND)
                {
                    if (StdItem.Shape >= 1 && StdItem.Shape <= 50)
                    {
                        m_btDressEffType = StdItem.Shape;
                    }
                    if (StdItem.Shape >= 51 && StdItem.Shape <= 100)
                    {
                        m_btHorseType = (byte)(StdItem.Shape - 50);
                    }
                    continue;
                }
                if (i == Grobal2.U_DRESS)
                {
                    if (m_UseItems[i].btValue[5] > 0)
                    {
                        m_btDressEffType = m_UseItems[i].btValue[5];
                    }
                    if (StdItem.AniCount > 0)
                    {
                        m_btDressEffType = StdItem.AniCount;
                    }
                    if (StdItem.Light)
                    {
                        m_nLight = 3;
                    }
                    continue;
                }
                // 新增开始
                if (StdItem.Shape == 139)
                {
                    m_boUnParalysis = true;
                }
                if (StdItem.Shape == 140)
                {
                    m_boSupermanItem = true;
                }
                if (StdItem.Shape == 141)
                {
                    m_boExpItem = true;
                    m_rExpItem = m_rExpItem + (m_UseItems[i].Dura / M2Share.g_Config.nItemExpRate);
                }
                if (StdItem.Shape == 142)
                {
                    m_boPowerItem = true;
                    m_rPowerItem = m_rPowerItem + (m_UseItems[i].Dura / M2Share.g_Config.nItemPowerRate);
                }
                if (StdItem.Shape == 182)
                {
                    m_boExpItem = true;
                    m_rExpItem = m_rExpItem + (m_UseItems[i].DuraMax / M2Share.g_Config.nItemExpRate);
                }
                if (StdItem.Shape == 183)
                {
                    m_boPowerItem = true;
                    m_rPowerItem = m_rPowerItem + (m_UseItems[i].DuraMax / M2Share.g_Config.nItemPowerRate);
                }
                if (StdItem.Shape == 143)
                {
                    m_boUnMagicShield = true;
                }
                if (StdItem.Shape == 144)
                {
                    m_boUnRevival = true;
                }
                if (StdItem.Shape == 170)
                {
                    m_boAngryRing = true;
                }
                if (StdItem.Shape == 171)
                {
                    m_boNoDropItem = true;
                }
                if (StdItem.Shape == 172)
                {
                    m_boNoDropUseItem = true;
                }
                if (StdItem.Shape == 150)
                {
                    // 麻痹护身
                    m_boParalysis = true;
                    m_boMagicShield = true;
                }
                if (StdItem.Shape == 151)
                {
                    // 麻痹火球
                    m_boParalysis = true;
                    m_boFlameRing = true;
                }
                if (StdItem.Shape == 152)
                {
                    // 麻痹防御
                    m_boParalysis = true;
                    m_boRecoveryRing = true;
                }
                if (StdItem.Shape == 153)
                {
                    // 麻痹负载
                    m_boParalysis = true;
                    m_boMuscleRing = true;
                }
                if (StdItem.Shape == 154)
                {
                    // 护身火球
                    m_boMagicShield = true;
                    m_boFlameRing = true;
                }
                if (StdItem.Shape == 155)
                {
                    // 护身防御
                    m_boMagicShield = true;
                    m_boRecoveryRing = true;
                }
                if (StdItem.Shape == 156)
                {
                    // 护身负载
                    m_boMagicShield = true;
                    m_boMuscleRing = true;
                }
                if (StdItem.Shape == 157)
                {
                    // 传送麻痹
                    m_boTeleport = true;
                    m_boParalysis = true;
                }
                if (StdItem.Shape == 158)
                {
                    // 传送护身
                    m_boTeleport = true;
                    m_boMagicShield = true;
                }
                if (StdItem.Shape == 159)
                {
                    // 传送探测
                    m_boTeleport = true;
                    m_boProbeNecklace = true;
                }
                if (StdItem.Shape == 160)
                {
                    // 传送复活
                    m_boTeleport = true;
                    m_boRevival = true;
                }
                if (StdItem.Shape == 161)
                {
                    // 麻痹复活
                    m_boParalysis = true;
                    m_boRevival = true;
                }
                if (StdItem.Shape == 162)
                {
                    // 护身复活
                    m_boMagicShield = true;
                    m_boRevival = true;
                }
                if (StdItem.Shape == 180)
                {
                    // PK 死亡掉经验
                    m_dwPKDieLostExp = StdItem.DuraMax * M2Share.g_Config.dwPKDieLostExpRate;
                }
                if (StdItem.Shape == 181)
                {
                    // PK 死亡掉等级
                    m_nPKDieLostLevel = StdItem.DuraMax / M2Share.g_Config.nPKDieLostLevelRate;
                }
                // 新增结束
                if (StdItem.Shape == 120)
                {
                    m_boFastTrain = true;
                }
                if (StdItem.Shape == 121)
                {
                    m_boProbeNecklace = true;
                }
                if (StdItem.Shape == 123)
                {
                    boRecallSuite[0] = true;
                }
                if (StdItem.Shape == 145)
                {
                    m_boGuildMove = true;
                }
                if (StdItem.Shape == 127)
                {
                    boSpirit[0] = true;
                }
                if (StdItem.Shape == 135)
                {
                    boMoXieSuite[0] = true;
                    m_nMoXieSuite += StdItem.AniCount;
                }
                if (StdItem.Shape == 138)
                {
                    boHongMoSuite1 = true;
                    m_nHongMoSuite += StdItem.AniCount;
                }
                if (StdItem.Shape == 200)
                {
                    boSmash1 = true;
                }
                if (StdItem.Shape == 203)
                {
                    boHwanDevil1 = true;
                }
                if (StdItem.Shape == 206)
                {
                    boPurity1 = true;
                }
                if (StdItem.Shape == 216)
                {
                    boFiveString1 = true;
                }
                if (StdItem.Shape == 111)
                {
                    m_wStatusTimeArr[Grobal2.STATE_TRANSPARENT] = 6 * 10 * 1000;
                    m_boHideMode = true;
                }
                if (StdItem.Shape == 112)
                {
                    m_boTeleport = true;
                }
                if (StdItem.Shape == 113)
                {
                    m_boParalysis = true;
                }
                if (StdItem.Shape == 114)
                {
                    m_boRevival = true;
                }
                if (StdItem.Shape == 115)
                {
                    m_boFlameRing = true;
                }
                if (StdItem.Shape == 116)
                {
                    m_boRecoveryRing = true;
                }
                if (StdItem.Shape == 117)
                {
                    m_boAngryRing = true;
                }
                if (StdItem.Shape == 118)
                {
                    m_boMagicShield = true;
                }
                if (StdItem.Shape == 119)
                {
                    m_boMuscleRing = true;
                }
                if (StdItem.Shape == 122)
                {
                    boRecallSuite[1] = true;
                }
                if (StdItem.Shape == 128)
                {
                    boSpirit[1] = true;
                }
                if (StdItem.Shape == 133)
                {
                    boMoXieSuite[1] = true;
                    m_nMoXieSuite += StdItem.AniCount;
                }
                if (StdItem.Shape == 136)
                {
                    boHongMoSuite2 = true;
                    m_nHongMoSuite += StdItem.AniCount;
                }
                if (StdItem.Shape == 201)
                {
                    boSmash2 = true;
                }
                if (StdItem.Shape == 204)
                {
                    boHwanDevil2 = true;
                }
                if (StdItem.Shape == 207)
                {
                    boPurity2 = true;
                }
                if (StdItem.Shape == 210)
                {
                    boMundane1 = true;
                }
                if (StdItem.Shape == 212)
                {
                    boNokChi1 = true;
                }
                if (StdItem.Shape == 214)
                {
                    boTaoBu1 = true;
                }
                if (StdItem.Shape == 217)
                {
                    boFiveString2 = true;
                }
                if ((StdItem.Source <= -1) && (StdItem.Source >= -50))
                {
                    // -1 to -50
                    m_AddAbil.btUndead = (byte)(m_AddAbil.btUndead + -StdItem.Source);
                    // Holy+
                }
                if ((StdItem.Source <= -51) && (StdItem.Source >= -100))
                {
                    // -51 to -100
                    m_AddAbil.btUndead = (byte)(m_AddAbil.btUndead + (StdItem.Source + 50));
                    // Holy-
                }
                if (StdItem.Shape == 124)
                {
                    boRecallSuite[2] = true;
                }
                if (StdItem.Shape == 126)
                {
                    boSpirit[2] = true;
                }
                if (StdItem.Shape == 145)
                {
                    m_boGuildMove = true;
                }
                if (StdItem.Shape == 134)
                {
                    boMoXieSuite[2] = true;
                    m_nMoXieSuite += StdItem.AniCount;
                }
                if (StdItem.Shape == 137)
                {
                    boHongMoSuite3 = true;
                    m_nHongMoSuite += StdItem.AniCount;
                }
                if (StdItem.Shape == 202)
                {
                    boSmash3 = true;
                }
                if (StdItem.Shape == 205)
                {
                    boHwanDevil3 = true;
                }
                if (StdItem.Shape == 208)
                {
                    boPurity3 = true;
                }
                if (StdItem.Shape == 211)
                {
                    boMundane2 = true;
                }
                if (StdItem.Shape == 213)
                {
                    boNokChi2 = true;
                }
                if (StdItem.Shape == 215)
                {
                    boTaoBu2 = true;
                }
                if (StdItem.Shape == 218)
                {
                    boFiveString3 = true;
                }
                if (StdItem.Shape == 125)
                {
                    boRecallSuite[3] = true;
                }
                if (StdItem.Shape == 129)
                {
                    boSpirit[3] = true;
                }
            }
            if (boRecallSuite[0] && boRecallSuite[1] && boRecallSuite[2] && boRecallSuite[3])
            {
                m_boRecallSuite = true;
            }
            if (boMoXieSuite[0] && boMoXieSuite[1] && boMoXieSuite[2])
            {
                m_nMoXieSuite += 50;
            }
            if (boHongMoSuite1 && boHongMoSuite2 && boHongMoSuite3)
            {
                m_AddAbil.wHitPoint += 2;
            }
            if (boSpirit[0] && boSpirit[1] && boSpirit[2] && boSpirit[3])
            {
                m_bopirit = true;
            }
            if (boSmash1 && boSmash2 && boSmash3)
            {
                m_boSmashSet = true;
            }
            if (boHwanDevil1 && boHwanDevil2 && boHwanDevil3)
            {
                m_boHwanDevilSet = true;
            }
            if (boPurity1 && boPurity2 && boPurity3)
            {
                m_boPuritySet = true;
            }
            if (boMundane1 && boMundane2)
            {
                m_boMundaneSet = true;
            }
            if (boNokChi1 && boNokChi2)
            {
                m_boNokChiSet = true;
            }
            if (boTaoBu1 && boTaoBu2)
            {
                m_boTaoBuSet = true;
            }
            if (boFiveString1 && boFiveString2 && boFiveString3)
            {
                m_boFiveStringSet = true;
            }
            m_WAbil.Weight = RecalcBagWeight();
            if (m_boTransparent && (m_wStatusTimeArr[Grobal2.STATE_TRANSPARENT] > 0))
            {
                m_boHideMode = true;
            }
            if (m_boHideMode)
            {
                if (!boOldHideMode)
                {
                    m_nCharStatus = GetCharStatus();
                    StatusChanged();
                }
            }
            else
            {
                if (boOldHideMode)
                {
                    m_wStatusTimeArr[Grobal2.STATE_TRANSPARENT] = 0;
                    m_nCharStatus = GetCharStatus();
                    StatusChanged();
                }
            }
            if (m_btRaceServer == Grobal2.RC_PLAYOBJECT)
            {
                RecalcHitSpeed();//增加此行，只有类型为人物的角色才重新计算攻击敏捷
            }
            int nOldLight = m_nLight;
            if ((m_UseItems[Grobal2.U_RIGHTHAND] != null) && (m_UseItems[Grobal2.U_RIGHTHAND].wIndex > 0) && (m_UseItems[Grobal2.U_RIGHTHAND].Dura > 0))
            {
                m_nLight = 3;
            }
            else
            {
                m_nLight = 0;
            }
            if (nOldLight != m_nLight)
            {
                SendRefMsg(Grobal2.RM_CHANGELIGHT, 0, 0, 0, 0, "");
            }
            m_btSpeedPoint += (byte)m_AddAbil.wSpeedPoint;
            m_btHitPoint += (byte)m_AddAbil.wHitPoint;
            m_btAntiPoison += (byte)m_AddAbil.wAntiPoison;
            m_nPoisonRecover += m_AddAbil.wPoisonRecover;
            m_nHealthRecover += m_AddAbil.wHealthRecover;
            m_nSpellRecover += m_AddAbil.wSpellRecover;
            m_nAntiMagic += m_AddAbil.wAntiMagic;
            m_nLuck += m_AddAbil.btLuck;
            m_nLuck -= m_AddAbil.btUnLuck;
            m_nHitSpeed = m_AddAbil.nHitSpeed;
            m_WAbil.MaxWeight += m_AddAbil.Weight;
            m_WAbil.MaxWearWeight += m_AddAbil.WearWeight;
            m_WAbil.MaxHandWeight += m_AddAbil.HandWeight;
            m_WAbil.MaxHP = (ushort)HUtil32._MIN(short.MaxValue, m_Abil.MaxHP + m_AddAbil.wHP);
            m_WAbil.MaxMP = (ushort)HUtil32._MIN(short.MaxValue, m_Abil.MaxMP + m_AddAbil.wMP);
            m_WAbil.AC = HUtil32.MakeLong(HUtil32.LoWord(m_AddAbil.wAC) + HUtil32.LoWord(m_Abil.AC), HUtil32.HiWord(m_AddAbil.wAC) + HUtil32.HiWord(m_Abil.AC));
            m_WAbil.MAC = HUtil32.MakeLong(HUtil32.LoWord(m_AddAbil.wMAC) + HUtil32.LoWord(m_Abil.MAC), HUtil32.HiWord(m_AddAbil.wMAC) + HUtil32.HiWord(m_Abil.MAC));
            m_WAbil.DC = HUtil32.MakeLong(HUtil32.LoWord(m_AddAbil.wDC) + HUtil32.LoWord(m_Abil.DC), HUtil32.HiWord(m_AddAbil.wDC) + HUtil32.HiWord(m_Abil.DC));
            m_WAbil.MC = HUtil32.MakeLong(HUtil32.LoWord(m_AddAbil.wMC) + HUtil32.LoWord(m_Abil.MC), HUtil32.HiWord(m_AddAbil.wMC) + HUtil32.HiWord(m_Abil.MC));
            m_WAbil.SC = HUtil32.MakeLong(HUtil32.LoWord(m_AddAbil.wSC) + HUtil32.LoWord(m_Abil.SC), HUtil32.HiWord(m_AddAbil.wSC) + HUtil32.HiWord(m_Abil.SC));
            if (m_wStatusTimeArr[Grobal2.STATE_DEFENCEUP] > 0)
            {
                m_WAbil.AC = HUtil32.MakeLong(HUtil32.LoWord(m_WAbil.AC), HUtil32.HiWord(m_WAbil.AC) + 2 + (m_Abil.Level / 7));
            }
            if (m_wStatusTimeArr[Grobal2.STATE_MAGDEFENCEUP] > 0)
            {
                m_WAbil.MAC = HUtil32.MakeLong(HUtil32.LoWord(m_WAbil.MAC), HUtil32.HiWord(m_WAbil.MAC) + 2 + (m_Abil.Level / 7));
            }
            if (m_wStatusArrValue[0] > 0)
            {
                m_WAbil.DC = HUtil32.MakeLong(HUtil32.LoWord(m_WAbil.DC), HUtil32.HiWord(m_WAbil.DC) + 2 + m_wStatusArrValue[0]);
            }
            if (m_wStatusArrValue[1] > 0)
            {
                m_WAbil.MC = HUtil32.MakeLong(HUtil32.LoWord(m_WAbil.MC), HUtil32.HiWord(m_WAbil.MC) + 2 + m_wStatusArrValue[1]);
            }
            if (m_wStatusArrValue[2] > 0)
            {
                m_WAbil.SC = HUtil32.MakeLong(HUtil32.LoWord(m_WAbil.SC), HUtil32.HiWord(m_WAbil.SC) + 2 + m_wStatusArrValue[2]);
            }
            if (m_wStatusArrValue[3] > 0)
            {
                m_nHitSpeed += m_wStatusArrValue[3];
            }
            if (m_wStatusArrValue[4] > 0)
            {
                m_WAbil.MaxHP = (ushort)HUtil32._MIN(short.MaxValue, m_WAbil.MaxHP + m_wStatusArrValue[4]);
            }
            if (m_wStatusArrValue[5] > 0)
            {
                m_WAbil.MaxMP = (ushort)HUtil32._MIN(short.MaxValue, m_WAbil.MaxMP + m_wStatusArrValue[5]);
            }
            if (m_boFlameRing)
            {
                AddItemSkill(1);
            }
            else
            {
                DelItemSkill(1);
            }
            if (m_boRecoveryRing)
            {
                AddItemSkill(2);
            }
            else
            {
                DelItemSkill(2);
            }
            if (m_boMuscleRing)
            {
                m_WAbil.MaxWeight += m_WAbil.MaxWeight;
                m_WAbil.MaxWearWeight += m_WAbil.MaxWearWeight;
                m_WAbil.MaxHandWeight += m_WAbil.MaxHandWeight;
            }
            if (m_nMoXieSuite > 0)
            {
                // 魔血
                if (m_WAbil.MaxMP <= m_nMoXieSuite)
                {
                    m_nMoXieSuite = m_WAbil.MaxMP - 1;
                }
                m_WAbil.MaxMP -= (ushort)m_nMoXieSuite;
                m_WAbil.MaxHP = (ushort)HUtil32._MIN(short.MaxValue, m_WAbil.MaxHP + m_nMoXieSuite);
            }
            if (m_bopirit)
            {
                // Bonus DC Min +2,DC Max +5,A.Speed + 2
                m_WAbil.DC = HUtil32.MakeLong(HUtil32.LoWord(m_WAbil.DC) + 2, HUtil32.HiWord(m_WAbil.DC) + 2 + 5);
                m_nHitSpeed += 2;
            }
            if (m_boSmashSet)
            {
                // Attack Speed +1, DC1-3
                m_WAbil.DC = HUtil32.MakeLong(HUtil32.LoWord(m_WAbil.DC) + 1, HUtil32.HiWord(m_WAbil.DC) + 2 + 3);
                m_nHitSpeed++;
            }
            if (m_boHwanDevilSet)
            {
                // Hand Carrying Weight Increase +5, Bag Weight Limit Increase +20, +MC 1-2
                m_WAbil.MaxHandWeight += 5;
                m_WAbil.MaxWeight += 20;
                m_WAbil.MC = HUtil32.MakeLong(HUtil32.LoWord(m_WAbil.MC) + 1, HUtil32.HiWord(m_WAbil.MC) + 2 + 2);
            }
            if (m_boPuritySet)
            {
                // Holy +3, Sc 1-2
                m_AddAbil.btUndead = (byte)(m_AddAbil.btUndead + -3);
                m_WAbil.SC = HUtil32.MakeLong(HUtil32.LoWord(m_WAbil.SC) + 1, HUtil32.HiWord(m_WAbil.SC) + 2 + 2);
            }
            if (m_boMundaneSet)
            {
                // Bonus of Hp+50
                m_WAbil.MaxHP = (ushort)HUtil32._MIN(short.MaxValue, m_WAbil.MaxHP + 50);
            }
            if (m_boNokChiSet)
            {
                // Bonus of Mp+50
                m_WAbil.MaxMP = (ushort)HUtil32._MIN(short.MaxValue, m_WAbil.MaxMP + 50);
            }
            if (m_boTaoBuSet)
            {
                // Bonus of Hp+30, Mp+30
                m_WAbil.MaxHP = (ushort)HUtil32._MIN(short.MaxValue, m_WAbil.MaxHP + 30);
                m_WAbil.MaxMP = (ushort)HUtil32._MIN(short.MaxValue, m_WAbil.MaxMP + 30);
            }
            if (m_boFiveStringSet)
            {
                // Bonus of Hp +30%, Ac+2
                m_WAbil.MaxHP = (ushort)HUtil32._MIN(short.MaxValue, m_WAbil.MaxHP / 100 * 30);
                m_btHitPoint += 2;
            }
            if (m_btRaceServer == Grobal2.RC_PLAYOBJECT)
            {
                SendUpdateMsg(this, Grobal2.RM_CHARSTATUSCHANGED, m_nHitSpeed, (int)m_nCharStatus, 0, 0, "");
            }
            if (m_btRaceServer >= Grobal2.RC_ANIMAL)
            {
                MonsterRecalcAbilitys();
            }
            // 限制最高属性
            m_WAbil.AC = HUtil32.MakeLong(HUtil32._MIN(M2Share.MAXHUMPOWER, HUtil32.LoWord(m_WAbil.AC)), HUtil32._MIN(M2Share.MAXHUMPOWER, HUtil32.HiWord(m_WAbil.AC)));
            m_WAbil.MAC = HUtil32.MakeLong(HUtil32._MIN(M2Share.MAXHUMPOWER, HUtil32.LoWord(m_WAbil.MAC)), HUtil32._MIN(M2Share.MAXHUMPOWER, HUtil32.HiWord(m_WAbil.MAC)));
            m_WAbil.DC = HUtil32.MakeLong(HUtil32._MIN(M2Share.MAXHUMPOWER, HUtil32.LoWord(m_WAbil.DC)), HUtil32._MIN(M2Share.MAXHUMPOWER, HUtil32.HiWord(m_WAbil.DC)));
            m_WAbil.MC = HUtil32.MakeLong(HUtil32._MIN(M2Share.MAXHUMPOWER, HUtil32.LoWord(m_WAbil.MC)), HUtil32._MIN(M2Share.MAXHUMPOWER, HUtil32.HiWord(m_WAbil.MC)));
            m_WAbil.SC = HUtil32.MakeLong(HUtil32._MIN(M2Share.MAXHUMPOWER, HUtil32.LoWord(m_WAbil.SC)), HUtil32._MIN(M2Share.MAXHUMPOWER, HUtil32.HiWord(m_WAbil.SC)));
            if (M2Share.g_Config.boHungerSystem && M2Share.g_Config.boHungerDecPower)
            {
                if (HUtil32.RangeInDefined(m_nHungerStatus, 0, 999))
                {
                    m_WAbil.DC = HUtil32.MakeLong(HUtil32.Round(HUtil32.LoWord(m_WAbil.DC) * 0.2), HUtil32.Round(HUtil32.HiWord(m_WAbil.DC) * 0.2));
                    m_WAbil.MC = HUtil32.MakeLong(HUtil32.Round(HUtil32.LoWord(m_WAbil.MC) * 0.2), HUtil32.Round(HUtil32.HiWord(m_WAbil.MC) * 0.2));
                    m_WAbil.SC = HUtil32.MakeLong(HUtil32.Round(HUtil32.LoWord(m_WAbil.SC) * 0.2), HUtil32.Round(HUtil32.HiWord(m_WAbil.SC) * 0.2));
                }
                else if (HUtil32.RangeInDefined(m_nHungerStatus, 1000, 1999))
                {
                    m_WAbil.DC = HUtil32.MakeLong(HUtil32.Round(HUtil32.LoWord(m_WAbil.DC) * 0.4), HUtil32.Round(HUtil32.HiWord(m_WAbil.DC) * 0.4));
                    m_WAbil.MC = HUtil32.MakeLong(HUtil32.Round(HUtil32.LoWord(m_WAbil.MC) * 0.4), HUtil32.Round(HUtil32.HiWord(m_WAbil.MC) * 0.4));
                    m_WAbil.SC = HUtil32.MakeLong(HUtil32.Round(HUtil32.LoWord(m_WAbil.SC) * 0.4), HUtil32.Round(HUtil32.HiWord(m_WAbil.SC) * 0.4));
                }
                else if (HUtil32.RangeInDefined(m_nHungerStatus, 2000, 2999))
                {
                    m_WAbil.DC = HUtil32.MakeLong(HUtil32.Round(HUtil32.LoWord(m_WAbil.DC) * 0.6), HUtil32.Round(HUtil32.HiWord(m_WAbil.DC) * 0.6));
                    m_WAbil.MC = HUtil32.MakeLong(HUtil32.Round(HUtil32.LoWord(m_WAbil.MC) * 0.6), HUtil32.Round(HUtil32.HiWord(m_WAbil.MC) * 0.6));
                    m_WAbil.SC = HUtil32.MakeLong(HUtil32.Round(HUtil32.LoWord(m_WAbil.SC) * 0.6), HUtil32.Round(HUtil32.HiWord(m_WAbil.SC) * 0.6));
                }
                else if (HUtil32.RangeInDefined(m_nHungerStatus, 3000, 3000))
                {
                    m_WAbil.DC = HUtil32.MakeLong(HUtil32.Round(HUtil32.LoWord(m_WAbil.DC) * 0.9), HUtil32.Round(HUtil32.HiWord(m_WAbil.DC) * 0.9));
                    m_WAbil.MC = HUtil32.MakeLong(HUtil32.Round(HUtil32.LoWord(m_WAbil.MC) * 0.9), HUtil32.Round(HUtil32.HiWord(m_WAbil.MC) * 0.9));
                    m_WAbil.SC = HUtil32.MakeLong(HUtil32.Round(HUtil32.LoWord(m_WAbil.SC) * 0.9), HUtil32.Round(HUtil32.HiWord(m_WAbil.SC) * 0.9));
                }
            }
        }
    }
}
