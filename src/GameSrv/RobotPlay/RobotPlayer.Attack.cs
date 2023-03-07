using GameSrv.Magic;
using SystemModule.Consts;
using SystemModule.Enums;
using SystemModule.Packets.ClientPackets;

namespace GameSrv.RobotPlay
{
    public partial class RobotPlayer
    {
        /// <summary>
        /// 物理攻击
        /// </summary>
        /// <returns></returns>
        private bool WarrAttackTarget(short wHitMode)
        {
            bool result = false;
            byte dir = 0;
            if (TargetCret != null)
            {
                bool boHit = GetAttackDir(TargetCret, ref dir);
                if (!boHit && (wHitMode == 4 || wHitMode == 15))
                {
                    boHit = GetAttackDir(TargetCret, 2, ref dir);// 防止隔位刺杀无效果
                }
                if (boHit)
                {
                    TargetFocusTick = HUtil32.GetTickCount();
                    AttackDir(TargetCret, wHitMode, dir);
                    ActionTick = HUtil32.GetTickCount();
                    BreakHolySeizeMode();
                    result = true;
                }
                else
                {
                    if (TargetCret.Envir == Envir)
                    {
                        SetTargetXy(TargetCret.CurrX, TargetCret.CurrY);
                    }
                    else
                    {
                        DelTargetCreat();
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 战士攻击
        /// </summary>
        /// <returns></returns>
        private bool WarrorAttackTarget()
        {
            UserMagic UserMagic;
            bool result = false;
            try
            {
                HitMode = 0;
                if (WAbil.MP > 0)
                {
                    if (TargetCret != null)
                    {
                        if (WAbil.HP <= Math.Round(WAbil.MaxHP * 0.25) || TargetCret.CrazyMode)
                        {
                            if (AllowUseMagic(MagicConst.SKILL_ERGUM))
                            {
                                // 血少时或目标疯狂模式时，做隔位刺杀 
                                if (!(Math.Abs(TargetCret.CurrX - CurrX) == 2 && Math.Abs(TargetCret.CurrY - CurrY) == 0 || Math.Abs(TargetCret.CurrX - CurrX) == 1 && Math.Abs(TargetCret.CurrY - CurrY) == 0 || Math.Abs(TargetCret.CurrX - CurrX) == 1 && Math.Abs(TargetCret.CurrY - CurrY) == 1 || Math.Abs(TargetCret.CurrX - CurrX) == 2 && Math.Abs(TargetCret.CurrY - CurrY) == 2 || Math.Abs(TargetCret.CurrX - CurrX) == 0 && Math.Abs(TargetCret.CurrY - CurrY) == 1 || Math.Abs(TargetCret.CurrX - CurrX) == 0 && Math.Abs(TargetCret.CurrY - CurrY) == 2))
                                {
                                    GetGotoXy(TargetCret, 2);
                                    GotoTargetXy(TargetX, TargetY, 0);
                                }
                            }
                        }
                    }
                    SearchMagic();
                    if (AutoMagicId > 0)
                    {
                        UserMagic = FindMagic(AutoMagicId);
                        if (UserMagic != null)
                        {
                            if (UserMagic.Key == 0)
                            {
                                switch (AutoMagicId)
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
                                        HitMode = 3;
                                        break;
                                    case 12:
                                        HitMode = 4;
                                        break;
                                    case 25: // 四级刺杀
                                        HitMode = 5;
                                        break;
                                    case 26: // 圆月弯刀(四级半月弯刀)
                                        if (UseSpell(UserMagic, CurrX, CurrY, TargetCret))
                                        {
                                            HitMode = 7;
                                        }
                                        break;
                                    case 40: // 使用烈火
                                        HitMode = 8;
                                        break;
                                    case 43: // 抱月刀法
                                        if (UseSpell(UserMagic, CurrX, CurrY, TargetCret))
                                        {
                                            HitMode = 9;
                                        }
                                        break;
                                    case 42: // 开天斩
                                        if (UseSpell(UserMagic, CurrX, CurrY, TargetCret))
                                        {
                                            HitMode = 12;
                                        }
                                        break;
                                }
                            }
                        }
                    }
                }
                result = WarrAttackTarget(HitMode);
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
        private bool WizardAttackTarget()
        {
            bool result = false;
            try
            {
                HitMode = 0;
                SearchMagic(); // 查询魔法
                if (AutoMagicId == 0)
                {
                    AutoUseMagic = true;// 是否能躲避
                }
                if (AutoMagicId > 0)
                {
                    if (TargetCret != null)
                    {
                        if (!MagCanHitTarget(CurrX, CurrY, TargetCret) || Math.Abs(TargetCret.CurrX - CurrX) > 7 || Math.Abs(TargetCret.CurrY - CurrY) > 7)
                        {
                            // 魔法不能打到怪
                            if (AutoMagicId != 10)// 除疾光电影外
                            {
                                GetGotoXy(TargetCret, 3);// 法只走向目标3格范围
                                GotoTargetXy(TargetX, TargetY, 0);
                            }
                        }
                    }
                    UserMagic UserMagic = FindMagic(AutoMagicId);
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
                if (AttackLevelTarget())
                {
                    AutoUseMagic = false;// 是否能躲避
                    result = WarrAttackTarget(HitMode);
                }
            }
            catch
            {
                M2Share.Logger.Error("RobotPlayObject.WizardAttackTarget");
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
            try
            {
                HitMode = 0;
                if (TargetCret != null)
                {
                    if (TaoLevelHitAttack()) // 22级砍血量的怪 
                    {
                        if (TargetCret.WAbil.MaxHP >= 700)
                        {
                            SearchMagic();// 查询魔法
                        }
                        else
                        {
                            if (HUtil32.GetTickCount() - SearchUseMagic > 1300) // 增加查询魔法的间隔
                            {
                                SearchMagic();// 查询魔法
                                SearchUseMagic = HUtil32.GetTickCount();
                            }
                            else
                            {
                                BoUseAttackMagic = false;// 可以走向目标
                            }
                        }
                    }
                    else
                    {
                        SearchMagic(); // 查询魔法
                    }
                }
                if (AutoMagicId == 0)
                {
                    AutoUseMagic = true;// 是否能躲避 
                }
                if (AutoMagicId > 0)
                {
                    if (TargetCret != null)
                    {
                        if (!MagCanHitTarget(CurrX, CurrY, TargetCret) || Math.Abs(TargetCret.CurrX - CurrX) > 7 || Math.Abs(TargetCret.CurrY - CurrY) > 7)// 魔法不能打到怪
                        {
                            if (TaoLevelHitAttack())// 22级砍血量的怪
                            {
                                if (TargetCret.WAbil.MaxHP >= 700)
                                {
                                    GetGotoXy(TargetCret, 3); // 道只走向目标3格范围
                                    GotoTargetXy(TargetX, TargetY, 0);
                                }
                            }
                            else
                            {
                                GetGotoXy(TargetCret, 3); // 道只走向目标3格范围
                                GotoTargetXy(TargetX, TargetY, 0);
                            }
                        }
                    }
                    UserMagic UserMagic;
                    switch (AutoMagicId)
                    {
                        case MagicConst.SKILL_HEALLING:// 治愈术 
                            if (WAbil.HP <= Math.Round(WAbil.MaxHP * 0.7))
                            {
                                UserMagic = FindMagic(AutoMagicId);
                                if (UserMagic != null && UserMagic.Key == 0)// 技能打开状态才能使用
                                {
                                    UseSpell(UserMagic, CurrX, CurrY, null);
                                    AttackTick = HUtil32.GetTickCount();
                                    if (TaoLevelHitAttack())// 22级砍血量的怪
                                    {
                                        if (TargetCret.WAbil.MaxHP >= 700)
                                        {
                                            AutoUseMagic = true;
                                            return result;
                                        }
                                        else
                                        {
                                            AutoMagicId = 0;
                                        }
                                    }
                                    else
                                    {
                                        AutoUseMagic = true;
                                        return result;
                                    }
                                }
                            }
                            break;
                        case MagicConst.SKILL_BIGHEALLING:// 群体治疗术
                            if (WAbil.HP <= Math.Round(WAbil.MaxHP * 0.7))
                            {
                                UserMagic = FindMagic(AutoMagicId);
                                if (UserMagic != null && UserMagic.Key == 0)// 技能打开状态才能使用
                                {
                                    UseSpell(UserMagic, CurrX, CurrY, this);
                                    AttackTick = HUtil32.GetTickCount();
                                    if (TaoLevelHitAttack())// 22级砍血量的怪 
                                    {
                                        if (TargetCret.WAbil.MaxHP >= 700)
                                        {
                                            AutoUseMagic = true;// 能躲避
                                            return result;
                                        }
                                        else
                                        {
                                            AutoMagicId = 0;
                                        }
                                    }
                                    else
                                    {
                                        AutoUseMagic = true;// 能躲避
                                        return result;
                                    }
                                }
                            }
                            break;
                        case MagicConst.SKILL_FIRECHARM:// 灵符火符
                            if (!MagCanHitTarget(CurrX, CurrY, TargetCret))
                            {
                                GetGotoXy(TargetCret, 3);
                                GotoTargetXy(TargetX, TargetY, 1);
                            }
                            break;
                        case MagicConst.SKILL_AMYOUNSUL:
                        case MagicConst.SKILL_GROUPAMYOUNSUL:
                            if (TargetCret.StatusTimeArr[PoisonState.DECHEALTH] == 0 && GetUserItemList(2, 1) >= 0)
                            {
                                NAmuletIndx = 1;
                            }
                            else if (TargetCret.StatusTimeArr[PoisonState.DAMAGEARMOR] == 0 && GetUserItemList(2, 2) >= 0)
                            {
                                NAmuletIndx = 2;
                            }
                            break;
                        case MagicConst.SKILL_CLOAK:
                        case MagicConst.SKILL_BIGCLOAK: // 集体隐身术  隐身术
                            UserMagic = FindMagic(AutoMagicId);
                            if (UserMagic != null && UserMagic.Key == 0)// 技能打开状态才能使用
                            {
                                UseSpell(UserMagic, CurrX, CurrY, this);
                                AttackTick = HUtil32.GetTickCount();
                                if (TaoLevelHitAttack())// 22级砍血量的怪 
                                {
                                    if (TargetCret.WAbil.MaxHP >= 700)
                                    {
                                        AutoUseMagic = false;
                                        return result;
                                    }
                                    else
                                    {
                                        AutoMagicId = 0;
                                    }
                                }
                                else
                                {
                                    AutoUseMagic = false;
                                    return result;
                                }
                            }
                            break;
                        case MagicConst.SKILL_SKELLETON:
                        case MagicConst.SKILL_SINSU:
                            UserMagic = FindMagic(AutoMagicId);
                            if (UserMagic != null && UserMagic.Key == 0)
                            {
                                UseSpell(UserMagic, TargetCret.CurrX, TargetCret.CurrY, TargetCret); // 使用魔法
                                AttackTick = HUtil32.GetTickCount();
                                if (TaoLevelHitAttack())// 22级砍血量的怪
                                {
                                    if (TargetCret.WAbil.MaxHP >= 700)
                                    {
                                        AutoUseMagic = true; // 能躲避
                                        return result;
                                    }
                                    else
                                    {
                                        AutoMagicId = 0;
                                    }
                                }
                                else
                                {
                                    AutoUseMagic = true; // 能躲避
                                    return result;
                                }
                            }
                            break;
                    }
                    UserMagic = FindMagic(AutoMagicId);
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
                    AutoUseMagic = true;
                }
                // 是否能躲避 
                if (AttackLevelTarget() || TargetCret.WAbil.MaxHP < 700 && TaoLevelHitAttack())// 道士22级前是否物理攻击  怪等级小于英雄时
                {
                    if (Math.Abs(TargetCret.CurrX - CurrX) > 1 || Math.Abs(TargetCret.CurrY - CurrY) > 1)// 道走近目标砍 
                    {
                        GotoTargetXy(TargetCret.CurrX, TargetCret.CurrY, 0);
                    }
                    AutoUseMagic = false;// 是否能躲避
                    result = WarrAttackTarget(HitMode);
                }
            }
            catch
            {
                // M2Share.MainOutMessage('{异常} RobotPlayObject.TaoistAttackTarget');
            }
            return result;
        }

        /// <summary>
        /// 道法22级前是否可以攻击
        /// </summary>
        /// <returns></returns>
        private bool AttackLevelTarget()
        {
            return M2Share.Config.boHeroAttackTarget && Abil.Level < 22;
        }

        /// <summary>
        /// 道士22级后是否物理攻击
        /// </summary>
        /// <returns></returns>
        private bool TaoLevelHitAttack()
        {
            return M2Share.Config.boHeroAttackTao && TargetCret.Race != ActorRace.Play;
        }

        private bool AttackTarget()
        {
            bool result = false;
            try
            {
                if (TargetCret != null)
                {
                    if (InSafeZone())// 进入安全区内就不打PK目标
                    {
                        if (TargetCret.Race == ActorRace.Play)
                        {
                            TargetCret = null;
                            return false;
                        }
                    }
                    if (TargetCret.ActorId == ActorId) // 防止英雄自己打自己
                    {
                        TargetCret = null;
                        return false;
                    }
                }
                TargetFocusTick = HUtil32.GetTickCount();
                if (Death || Ghost)
                {
                    return false;
                }
                switch (Job)
                {
                    case PlayJob.Warrior:
                        if (HUtil32.GetTickCount() - AttackTick > M2Share.Config.nAIWarrorAttackTime)
                        {
                            AutoUseMagic = false;// 是否能躲避
                            result = WarrorAttackTarget();
                        }
                        break;
                    case PlayJob.Wizard:
                        if (HUtil32.GetTickCount() - AttackTick > M2Share.Config.nAIWizardAttackTime)// 连击也不受间隔控制
                        {
                            AttackTick = HUtil32.GetTickCount();
                            AutoUseMagic = false;// 是否能躲避
                            result = WizardAttackTarget();
                            AutoMagicId = 0;
                            return result;
                        }
                        AutoMagicId = 0;
                        break;
                    case PlayJob.Taoist:
                        if (HUtil32.GetTickCount() - AttackTick > M2Share.Config.nAITaoistAttackTime)
                        {
                            AttackTick = HUtil32.GetTickCount();
                            AutoUseMagic = false; // 是否能躲避
                            result = TaoistAttackTarget();
                            AutoMagicId = 0;
                            return result;
                        }
                        AutoMagicId = 0;
                        break;
                }
            }
            catch
            {
                M2Share.Logger.Error("RobotPlayObject.AttackTarget");
            }
            return result;
        }
    }
}