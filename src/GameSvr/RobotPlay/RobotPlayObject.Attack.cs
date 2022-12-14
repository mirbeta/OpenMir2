using GameSvr.Magic;
using SystemModule;
using SystemModule.Consts;
using SystemModule.Enums;
using SystemModule.Packets.ClientPackets;

namespace GameSvr.RobotPlay
{
    public partial class RobotPlayObject
    {
        /// <summary>
        /// 物理攻击
        /// </summary>
        /// <returns></returns>
        private bool WarrAttackTarget1(short wHitMode)
        {
            bool result = false;
            byte bt06 = 0;
            try
            {
                if (TargetCret != null)
                {
                    var boHit = GetAttackDir(TargetCret, ref bt06);
                    if (!boHit && (wHitMode == 4 || wHitMode == 15))
                    {
                        boHit = GetAttackDir(TargetCret, 2, ref bt06);// 防止隔位刺杀无效果
                    }
                    if (boHit)
                    {
                        TargetFocusTick = HUtil32.GetTickCount();
                        AttackDir(TargetCret, wHitMode, bt06);
                        MDwActionTick = HUtil32.GetTickCount();
                        BreakHolySeizeMode();
                        result = true;
                    }
                    else
                    {
                        if (TargetCret.Envir == Envir)
                        {
                            SetTargetXY(TargetCret.CurrX, TargetCret.CurrY);
                        }
                        else
                        {
                            DelTargetCreat();
                        }
                    }
                }
            }
            catch
            {
                M2Share.Log.Error("RobotPlayObject.WarrAttackTarget");
            }
            return result;
        }

        /// <summary>
        /// 战士攻击
        /// </summary>
        /// <returns></returns>
        private bool WarrorAttackTarget1()
        {
            UserMagic UserMagic;
            bool result = false;
            try
            {
                m_wHitMode = 0;
                if (WAbil.MP > 0)
                {
                    if (TargetCret != null)
                    {
                        if (WAbil.HP <= Math.Round(WAbil.MaxHP * 0.25) || TargetCret.CrazyMode)
                        {
                            // 注释,战不躲避
                            if (AllowUseMagic(12))
                            {
                                // 血少时或目标疯狂模式时，做隔位刺杀 
                                if (!(Math.Abs(TargetCret.CurrX - CurrX) == 2 && Math.Abs(TargetCret.CurrY - CurrY) == 0 || Math.Abs(TargetCret.CurrX - CurrX) == 1 && Math.Abs(TargetCret.CurrY - CurrY) == 0 || Math.Abs(TargetCret.CurrX - CurrX) == 1 && Math.Abs(TargetCret.CurrY - CurrY) == 1 || Math.Abs(TargetCret.CurrX - CurrX) == 2 && Math.Abs(TargetCret.CurrY - CurrY) == 2 || Math.Abs(TargetCret.CurrX - CurrX) == 0 && Math.Abs(TargetCret.CurrY - CurrY) == 1 || Math.Abs(TargetCret.CurrX - CurrX) == 0 && Math.Abs(TargetCret.CurrY - CurrY) == 2))
                                {
                                    GetGotoXY(TargetCret, 2);
                                    GotoTargetXY(TargetX, TargetY, 0);
                                }
                            }
                        }
                    }
                    SearchMagic();
                    if (m_nSelectMagic > 0)
                    {
                        UserMagic = FindMagic(m_nSelectMagic);
                        if (UserMagic != null)
                        {
                            if (UserMagic.Key == 0)
                            {
                                switch (m_nSelectMagic)
                                {
                                    // 技能打开状态才能使用
                                    // Modify the A .. B: 27, 39, 41, 60 .. 65, 68, 75, SKILL_101, SKILL_102
                                    case 27:
                                    case 39:
                                    case 41:
                                    case 60:
                                    case 68:
                                    case 75:
                                        if (TargetCret != null)
                                        {
                                            result = UseSpell(UserMagic, TargetCret.CurrX, TargetCret.CurrY, TargetCret); // 战士魔法
                                            AttackTick = HUtil32.GetTickCount();
                                            return result;
                                        }
                                        break;
                                    case 7:
                                        m_wHitMode = 3;
                                        break;
                                    case 12:
                                        m_wHitMode = 4;
                                        break;
                                    case 25: // 四级刺杀
                                        m_wHitMode = 5;
                                        break;
                                    case 26: // 圆月弯刀(四级半月弯刀)
                                        if (UseSpell(UserMagic, CurrX, CurrY, TargetCret))
                                        {
                                            m_wHitMode = 7;
                                        }
                                        break;
                                    case 40: // 使用烈火
                                        m_wHitMode = 8;
                                        break;
                                    case 43: // 抱月刀法
                                        if (UseSpell(UserMagic, CurrX, CurrY, TargetCret))
                                        {
                                            m_wHitMode = 9;
                                        }
                                        break;
                                    case 42: // 开天斩
                                        if (UseSpell(UserMagic, CurrX, CurrY, TargetCret))
                                        {
                                            m_wHitMode = 12;
                                        }
                                        break;
                                }
                            }
                        }
                    }
                }

                result = WarrAttackTarget1(m_wHitMode);
                if (result)
                {
                    AttackTick = HUtil32.GetTickCount();
                }
            }
            catch
            {
                // M2Share.MainOutMessage(format("{%s} RobotPlayObject.WarrorAttackTarget Code:%d", new byte[] { nCode }));
            }
            return result;
        }

        /// <summary>
        /// 法师攻击
        /// </summary>
        /// <returns></returns>
        private bool WizardAttackTarget1()
        {
            bool result = false;
            try
            {
                m_wHitMode = 0;
                SearchMagic(); // 查询魔法
                if (m_nSelectMagic == 0)
                {
                    m_boIsUseMagic = true;// 是否能躲避
                }
                if (m_nSelectMagic > 0)
                {
                    if (TargetCret != null)
                    {
                        if (!MagCanHitTarget(CurrX, CurrY, TargetCret) || Math.Abs(TargetCret.CurrX - CurrX) > 7 || Math.Abs(TargetCret.CurrY - CurrY) > 7)
                        {
                            // 魔法不能打到怪
                            if (m_nSelectMagic != 10)// 除疾光电影外
                            {
                                GetGotoXY(TargetCret, 3);// 法只走向目标3格范围
                                GotoTargetXY(TargetX, TargetY, 0);
                            }
                        }
                    }
                    var UserMagic = FindMagic(m_nSelectMagic);
                    if (UserMagic != null)
                    {
                        if (UserMagic.Key == 0)// 技能打开状态才能使用
                        {
                            AttackTick = HUtil32.GetTickCount();
                            return UseSpell(UserMagic, TargetCret.CurrX, TargetCret.CurrY, TargetCret);
                        }
                    }
                }
                AttackTick = HUtil32.GetTickCount();
                if (M2Share.Config.boHeroAttackTarget && Abil.Level < 22)
                {
                    m_boIsUseMagic = false;// 是否能躲避
                    result = WarrAttackTarget1(m_wHitMode);
                }
            }
            catch
            {
                M2Share.Log.Error("RobotPlayObject.WizardAttackTarget");
            }
            return result;
        }

        /// <summary>
        /// 道士攻击
        /// </summary>
        /// <returns></returns>
        private bool TaoistAttackTarget()
        {
            bool result = false;
            UserMagic UserMagic;
            try
            {
                m_wHitMode = 0;
                if (TargetCret != null)
                {
                    if (M2Share.Config.boHeroAttackTao && TargetCret.Race != ActorRace.Play) // 22级砍血量的怪 
                    {
                        if (TargetCret.WAbil.MaxHP >= 700)
                        {
                            SearchMagic();// 查询魔法
                        }
                        else
                        {
                            if (HUtil32.GetTickCount() - m_dwSearchMagic > 1300) // 增加查询魔法的间隔
                            {
                                SearchMagic();// 查询魔法
                                m_dwSearchMagic = HUtil32.GetTickCount();
                            }
                            else
                            {
                                m_boIsUseAttackMagic = false;// 可以走向目标
                            }
                        }
                    }
                    else
                    {
                        SearchMagic(); // 查询魔法
                    }
                }
                if (m_nSelectMagic == 0)
                {
                    m_boIsUseMagic = true;// 是否能躲避 
                }
                if (m_nSelectMagic > 0)
                {
                    if (TargetCret != null)
                    {
                        if (!MagCanHitTarget(CurrX, CurrY, TargetCret) || Math.Abs(TargetCret.CurrX - CurrX) > 7 || Math.Abs(TargetCret.CurrY - CurrY) > 7)
                        {
                            // 魔法不能打到怪
                            if (M2Share.Config.boHeroAttackTao && TargetCret.Race != ActorRace.Play)// 22级砍血量的怪
                            {
                                if (TargetCret.WAbil.MaxHP >= 700)
                                {
                                    GetGotoXY(TargetCret, 3); // 道只走向目标3格范围
                                    GotoTargetXY(TargetX, TargetY, 0);
                                }
                            }
                            else
                            {
                                GetGotoXY(TargetCret, 3); // 道只走向目标3格范围
                                GotoTargetXY(TargetX, TargetY, 0);
                            }
                        }
                    }
                    switch (m_nSelectMagic)
                    {
                        case MagicConst.SKILL_HEALLING:// 治愈术 
                            if (WAbil.HP <= Math.Round(WAbil.MaxHP * 0.7))
                            {
                                UserMagic = FindMagic(m_nSelectMagic);
                                if (UserMagic != null && UserMagic.Key == 0)// 技能打开状态才能使用
                                {
                                    UseSpell(UserMagic, CurrX, CurrY, null);
                                    AttackTick = HUtil32.GetTickCount();
                                    if (M2Share.Config.boHeroAttackTao && TargetCret.Race != ActorRace.Play)// 22级砍血量的怪
                                    {
                                        if (TargetCret.WAbil.MaxHP >= 700)
                                        {
                                            m_boIsUseMagic = true;
                                            return result;
                                        }
                                        else
                                        {
                                            m_nSelectMagic = 0;
                                        }
                                    }
                                    else
                                    {
                                        m_boIsUseMagic = true;
                                        return result;
                                    }
                                }
                            }
                            break;
                        case MagicConst.SKILL_BIGHEALLING:// 群体治疗术
                            if (WAbil.HP <= Math.Round(WAbil.MaxHP * 0.7))
                            {
                                UserMagic = FindMagic(m_nSelectMagic);
                                if (UserMagic != null && UserMagic.Key == 0)// 技能打开状态才能使用
                                {
                                    UseSpell(UserMagic, CurrX, CurrY, this);
                                    AttackTick = HUtil32.GetTickCount();
                                    if (M2Share.Config.boHeroAttackTao && TargetCret.Race != ActorRace.Play)// 22级砍血量的怪 
                                    {
                                        if (TargetCret.WAbil.MaxHP >= 700)
                                        {
                                            m_boIsUseMagic = true;// 能躲避
                                            return result;
                                        }
                                        else
                                        {
                                            m_nSelectMagic = 0;
                                        }
                                    }
                                    else
                                    {
                                        m_boIsUseMagic = true;// 能躲避
                                        return result;
                                    }
                                }
                            }
                            break;
                        case MagicConst.SKILL_FIRECHARM:// 灵符火符
                            if (!MagCanHitTarget(CurrX, CurrY, TargetCret))
                            {
                                GetGotoXY(TargetCret, 3);
                                GotoTargetXY(TargetX, TargetY, 1);
                            }
                            break;
                        case MagicConst.SKILL_AMYOUNSUL:
                        case MagicConst.SKILL_GROUPAMYOUNSUL:
                            if (TargetCret.StatusArr[PoisonState.DECHEALTH] == 0 && GetUserItemList(2, 1) >= 0)
                            {
                                n_AmuletIndx = 1;
                            }
                            else if (TargetCret.StatusArr[PoisonState.DAMAGEARMOR] == 0 && GetUserItemList(2, 2) >= 0)
                            {
                                n_AmuletIndx = 2;
                            }
                            break;
                        case MagicConst.SKILL_CLOAK:
                        case MagicConst.SKILL_BIGCLOAK: // 集体隐身术  隐身术
                            UserMagic = FindMagic(m_nSelectMagic);
                            if (UserMagic != null && UserMagic.Key == 0)// 技能打开状态才能使用
                            {
                                UseSpell(UserMagic, CurrX, CurrY, this);
                                AttackTick = HUtil32.GetTickCount();
                                if (M2Share.Config.boHeroAttackTao && TargetCret.Race != ActorRace.Play)// 22级砍血量的怪 
                                {
                                    if (TargetCret.WAbil.MaxHP >= 700)
                                    {
                                        m_boIsUseMagic = false;
                                        return result;
                                    }
                                    else
                                    {
                                        m_nSelectMagic = 0;
                                    }
                                }
                                else
                                {
                                    m_boIsUseMagic = false;
                                    return result;
                                }
                            }
                            break;
                        case MagicConst.SKILL_SKELLETON:
                        case MagicConst.SKILL_SINSU:
                            UserMagic = FindMagic(m_nSelectMagic);
                            if (UserMagic != null && UserMagic.Key == 0)
                            {
                                UseSpell(UserMagic, TargetCret.CurrX, TargetCret.CurrY, TargetCret); // 使用魔法
                                AttackTick = HUtil32.GetTickCount();
                                if (M2Share.Config.boHeroAttackTao && TargetCret.Race != ActorRace.Play)
                                {
                                    // 22级砍血量的怪
                                    if (TargetCret.WAbil.MaxHP >= 700)
                                    {
                                        m_boIsUseMagic = true; // 能躲避
                                        return result;
                                    }
                                    else
                                    {
                                        m_nSelectMagic = 0;
                                    }
                                }
                                else
                                {
                                    m_boIsUseMagic = true; // 能躲避
                                    return result;
                                }
                            }
                            break;
                    }
                    UserMagic = FindMagic(m_nSelectMagic);
                    if (UserMagic != null)
                    {
                        if (UserMagic.Key == 0)   // 技能打开状态才能使用 
                        {
                            AttackTick = HUtil32.GetTickCount();
                            result = UseSpell(UserMagic, TargetCret.CurrX, TargetCret.CurrY, TargetCret); // 使用魔法
                            if (TargetCret.WAbil.MaxHP >= 700 || !M2Share.Config.boHeroAttackTao)
                            {
                                return result;
                            }
                        }
                    }
                }
                AttackTick = HUtil32.GetTickCount();
                if (WAbil.HP <= Math.Round(WAbil.MaxHP * 0.15))
                {
                    m_boIsUseMagic = true;
                }
                // 是否能躲避 
                // 增加人形条件
                if (M2Share.Config.boHeroAttackTarget && Abil.Level < 22 || TargetCret.WAbil.MaxHP < 700 && M2Share.Config.boHeroAttackTao && TargetCret.Race != ActorRace.Play)
                {
                    // 20090106 道士22级前是否物理攻击  怪等级小于英雄时
                    if (Math.Abs(TargetCret.CurrX - CurrX) > 1 || Math.Abs(TargetCret.CurrY - CurrY) > 1)
                    {
                        // 道走近目标砍 
                        GotoTargetXY(TargetCret.CurrX, TargetCret.CurrY, 0);
                    }
                    m_boIsUseMagic = false;// 是否能躲避
                    result = WarrAttackTarget1(m_wHitMode);
                }
            }
            catch
            {
                // M2Share.MainOutMessage('{异常} RobotPlayObject.TaoistAttackTarget');
            }
            return result;
        }

        private bool AttackTarget()
        {
            bool result = false;
            try
            {
                if (TargetCret != null)
                {
                    if (InSafeZone())// 英雄进入安全区内就不打PK目标
                    {
                        if (TargetCret.Race == ActorRace.Play)
                        {
                            TargetCret = null;
                            return result;
                        }
                    }
                    if (TargetCret == this) // 防止英雄自己打自己
                    {
                        TargetCret = null;
                        return result;
                    }
                }
                TargetFocusTick = HUtil32.GetTickCount();
                if (Death || Ghost)
                {
                    return result;
                }
                switch (Job)
                {
                    case PlayJob.Warrior:
                        if (HUtil32.GetTickCount() - AttackTick > M2Share.Config.nAIWarrorAttackTime)
                        {
                            m_boIsUseMagic = false;// 是否能躲避
                            result = WarrorAttackTarget1();
                        }
                        break;
                    case PlayJob.Wizard:
                        if (HUtil32.GetTickCount() - AttackTick > M2Share.Config.nAIWizardAttackTime)// 连击也不受间隔控制
                        {
                            AttackTick = HUtil32.GetTickCount();
                            m_boIsUseMagic = false;// 是否能躲避
                            result = WizardAttackTarget1();
                            m_nSelectMagic = 0;
                            return result;
                        }
                        m_nSelectMagic = 0;
                        break;
                    case PlayJob.Taoist:
                        if (HUtil32.GetTickCount() - AttackTick > M2Share.Config.nAITaoistAttackTime)
                        {
                            AttackTick = HUtil32.GetTickCount();
                            m_boIsUseMagic = false; // 是否能躲避
                            result = TaoistAttackTarget();
                            m_nSelectMagic = 0;
                            return result;
                        }
                        m_nSelectMagic = 0;
                        break;
                }
            }
            catch
            {
                M2Share.Log.Error("RobotPlayObject.AttackTarget");
            }
            return result;
        }
    }
}