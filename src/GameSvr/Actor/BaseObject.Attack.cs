using GameSvr.Items;
using GameSvr.Magic;
using GameSvr.Player;
using GameSvr.RobotPlay;
using SystemModule;
using SystemModule.Consts;
using SystemModule.Data;
using SystemModule.Enums;
using SystemModule.Packets.ClientPackets;

namespace GameSvr.Actor
{
    public partial class BaseObject
    {
        protected virtual void AttackDir(BaseObject TargeBaseObject, short wHitMode, byte nDir)
        {
            const string sExceptionMsg = "[Exception] TBaseObject::AttackDir";
            try
            {
                switch (wHitMode)
                {
                    case 5 when (MagicArr[MagicConst.SKILL_BANWOL] != null):
                        if (WAbil.MP > 0)
                        {
                            DamageSpell((ushort)(MagicArr[MagicConst.SKILL_BANWOL].Magic.DefSpell + GetMagicSpell(MagicArr[MagicConst.SKILL_BANWOL])));
                            HealthSpellChanged();
                        }
                        else
                        {
                            wHitMode = Grobal2.RM_HIT;
                        }
                        break;
                    case 8 when (MagicArr[MagicConst.SKILL_CROSSMOON] != null):
                        if (WAbil.MP > 0)
                        {
                            DamageSpell((ushort)(MagicArr[MagicConst.SKILL_CROSSMOON].Magic.DefSpell + GetMagicSpell(MagicArr[MagicConst.SKILL_CROSSMOON])));
                            HealthSpellChanged();
                        }
                        else
                        {
                            wHitMode = Grobal2.RM_HIT;
                        }
                        break;
                    case 12 when (MagicArr[MagicConst.SKILL_REDBANWOL] != null):
                        if (WAbil.MP > 0)
                        {
                            DamageSpell((ushort)(MagicArr[MagicConst.SKILL_REDBANWOL].Magic.DefSpell + GetMagicSpell(MagicArr[MagicConst.SKILL_REDBANWOL])));
                            HealthSpellChanged();
                        }
                        else
                        {
                            wHitMode = Grobal2.RM_HIT;
                        }
                        break;
                }
                Direction = nDir;
                BaseObject AttackTarget;
                if (TargeBaseObject == null)
                {
                    AttackTarget = GetPoseCreate();
                }
                else
                {
                    AttackTarget = TargeBaseObject;
                }
                if (UseItems[Grobal2.U_WEAPON] != null && (UseItems[Grobal2.U_WEAPON].Index > 0) && UseItems[Grobal2.U_WEAPON].Desc[ItemAttr.WeaponUpgrade] > 0)
                {
                    if (AttackTarget != null)
                    {
                        CheckWeaponUpgrade();
                    }
                }
                var boPowerHit = PowerHit;
                var boFireHit = FireHitSkill;
                var boTwinHit = TwinHitSkill;
                if (_Attack(ref wHitMode, AttackTarget))
                {
                    SetTargetCreat(AttackTarget);
                }
                int wIdent = Grobal2.RM_HIT;
                if (Race == ActorRace.Play)
                {
                    switch (wHitMode)
                    {
                        case 0:
                            wIdent = Grobal2.RM_HIT;
                            break;
                        case 1:
                            wIdent = Grobal2.RM_HEAVYHIT;
                            break;
                        case 2:
                            wIdent = Grobal2.RM_BIGHIT;
                            break;
                        case 3:
                            if (boPowerHit)
                            {
                                wIdent = Grobal2.RM_SPELL2;
                            }
                            break;
                        case 4:
                            if (MagicArr[MagicConst.SKILL_ERGUM] != null)
                            {
                                wIdent = Grobal2.RM_LONGHIT;
                            }
                            break;
                        case 5:
                            if (MagicArr[MagicConst.SKILL_BANWOL] != null)
                            {
                                wIdent = Grobal2.RM_WIDEHIT;
                            }
                            break;
                        case 7:
                            if (boFireHit)
                            {
                                wIdent = Grobal2.RM_FIREHIT;
                            }
                            break;
                        case 8:
                            if (MagicArr[MagicConst.SKILL_CROSSMOON] != null)
                            {
                                wIdent = Grobal2.RM_CRSHIT;
                            }
                            break;
                        case 9:
                            if (boTwinHit)
                            {
                                wIdent = Grobal2.RM_TWINHIT;
                            }
                            break;
                        case 12:
                            if (MagicArr[MagicConst.SKILL_REDBANWOL] != null)
                            {
                                wIdent = Grobal2.RM_WIDEHIT;
                            }
                            break;
                    }
                }
                SendAttackMsg(wIdent, Direction, CurrX, CurrY);
            }
            catch (Exception e)
            {
                M2Share.Log.Error(sExceptionMsg);
                M2Share.Log.Error(e.Message);
            }
        }

        protected bool _Attack(ref short wHitMode, BaseObject AttackTarget)
        {
            int n20;
            bool result = false;
            try
            {
                bool bo21 = false;
                ushort nWeaponDamage = 0;
                ushort nPower = 0;
                int nSecPwr = 0;
                if (AttackTarget != null)
                {
                    nPower = GetAttackPower(HUtil32.LoByte(WAbil.DC), (sbyte)(HUtil32.HiByte(WAbil.DC) - HUtil32.LoByte(WAbil.DC)));
                    switch (wHitMode)
                    {
                        case 3 when PowerHit:
                            PowerHit = false;
                            nPower += HitPlus;
                            bo21 = true;
                            break;
                        case 7 when FireHitSkill:// 烈火剑法
                            FireHitSkill = false;
                            LatestFireHitTick = HUtil32.GetTickCount();// 禁止双烈火
                            nPower = (ushort)(nPower + HUtil32.Round(nPower / 100 * HitDouble * 10));
                            bo21 = true;
                            break;
                        case 9 when TwinHitSkill: // 烈火剑法
                            TwinHitSkill = false;
                            LatestTwinHitTick = HUtil32.GetTickCount();// 禁止双烈火
                            nPower = (ushort)(nPower + HUtil32.Round(nPower / 100 * HitDouble * 10));
                            bo21 = true;
                            break;
                    }
                }
                else
                {
                    nPower = GetAttackPower(HUtil32.LoByte(WAbil.DC), (sbyte)(HUtil32.HiByte(WAbil.DC) - HUtil32.LoByte(WAbil.DC)));
                    switch (wHitMode)
                    {
                        case 3 when PowerHit:
                            PowerHit = false;
                            nPower += HitPlus;
                            bo21 = true;
                            break;
                        case 7 when FireHitSkill:
                            FireHitSkill = false;
                            LatestFireHitTick = HUtil32.GetTickCount();// 禁止双烈火
                            break;
                        case 9 when TwinHitSkill:
                            TwinHitSkill = false;
                            LatestTwinHitTick = HUtil32.GetTickCount();// 禁止双烈火
                            break;
                    }
                }
                switch (wHitMode)
                {
                    case 4:// 刺杀
                        nSecPwr = 0;
                        if (Race == ActorRace.Play)
                        {
                            if (MagicArr[MagicConst.SKILL_ERGUM] != null)
                            {
                                nSecPwr = HUtil32.Round(nPower / (MagicArr[MagicConst.SKILL_ERGUM].Magic.TrainLv + 2) * (MagicArr[MagicConst.SKILL_ERGUM].Level + 2));
                            }
                        }
                        if (nSecPwr > 0)
                        {
                            if (!SwordLongAttack(ref nSecPwr) && M2Share.Config.LimitSwordLong)
                            {
                                wHitMode = 0;
                            }
                        }
                        break;
                    case 5:
                        {
                            nSecPwr = 0;
                            if (Race == ActorRace.Play)
                            {
                                if (MagicArr[MagicConst.SKILL_BANWOL] != null)
                                {
                                    nSecPwr = HUtil32.Round(nPower / (MagicArr[MagicConst.SKILL_BANWOL].Magic.TrainLv + 10) * (MagicArr[MagicConst.SKILL_BANWOL].Level + 2));
                                }
                            }
                            if (nSecPwr > 0)
                            {
                                SwordWideAttack(ref nSecPwr);
                            }
                            break;
                        }
                    case 12:
                        {
                            nSecPwr = 0;
                            if (Race == ActorRace.Play)
                            {
                                if (MagicArr[MagicConst.SKILL_REDBANWOL] != null)
                                {
                                    nSecPwr = HUtil32.Round(nPower / (MagicArr[MagicConst.SKILL_REDBANWOL].Magic.TrainLv + 10) * (MagicArr[MagicConst.SKILL_REDBANWOL].Level + 2));
                                }
                            }
                            if (nSecPwr > 0)
                            {
                                SwordWideAttack(ref nSecPwr);
                            }
                            break;
                        }
                    case 8:
                        {
                            nSecPwr = 0;
                            if (Race == ActorRace.Play)
                            {
                                if (MagicArr[MagicConst.SKILL_CROSSMOON] != null)
                                {
                                    nSecPwr = HUtil32.Round(nPower / (MagicArr[MagicConst.SKILL_CROSSMOON].Magic.TrainLv + 10) * (MagicArr[MagicConst.SKILL_CROSSMOON].Level + 2));
                                }
                            }
                            if (nSecPwr > 0)
                            {
                                CrsWideAttack(nSecPwr);
                            }
                            break;
                        }
                }
                if (AttackTarget == null)
                {
                    return false;
                }
                if (IsProperTarget(AttackTarget))
                {
                    if (AttackTarget.HitPoint > 0)
                    {
                        if (HitPoint < M2Share.RandomNumber.RandomByte(AttackTarget.SpeedPoint))
                        {
                            nPower = 0;
                        }
                    }
                }
                else
                {
                    nPower = 0;
                }
                if (nPower > 0)
                {
                    nPower = AttackTarget.GetHitStruckDamage(this, nPower);
                    nWeaponDamage = (ushort)(M2Share.RandomNumber.Random(5) + 2 - AddAbil.WeaponStrong);
                }
                if (nPower > 0)
                {
                    AttackTarget.StruckDamage(nPower);
                    AttackTarget.SendDelayMsg(Grobal2.RM_STRUCK, Grobal2.RM_REFMESSAGE, nPower, AttackTarget.WAbil.HP, AttackTarget.WAbil.MaxHP, ActorId, "", 200);
                    if (AttackTarget.Race == ActorRace.Play)
                    {
                        if (!(AttackTarget as PlayObject).UnParalysis && (this as PlayObject).Paralysis && (M2Share.RandomNumber.Random(AttackTarget.AntiPoison + M2Share.Config.AttackPosionRate) == 0))
                        {
                            AttackTarget.MakePosion(PoisonState.STONE, M2Share.Config.AttackPosionTime, 0);
                        }
                    }
                    if (SuckupEnemyHealthRate > 0)// 虹魔，吸血
                    {
                        SuckupEnemyHealth = nPower / 100 * SuckupEnemyHealthRate;
                        if (SuckupEnemyHealth >= 2.0)
                        {
                            n20 = Convert.ToInt32(SuckupEnemyHealth);
                            SuckupEnemyHealth = n20;
                            DamageHealth((ushort)-n20);
                        }
                    }
                    if (Race == ActorRace.Play)
                    {
                        UserMagic attackMagic;
                        if (MagicArr[MagicConst.SKILL_ILKWANG] != null)
                        {
                            attackMagic = GetAttrackMagic(MagicConst.SKILL_ILKWANG);
                            if ((attackMagic.Level < 3) && (attackMagic.Magic.TrainLevel[attackMagic.Level] <= Abil.Level))
                            {
                                (this as PlayObject).TrainSkill(attackMagic, M2Share.RandomNumber.Random(3) + 1);
                                if (!(this as PlayObject).CheckMagicLevelup(attackMagic))
                                {
                                    SendDelayMsg(this, Grobal2.RM_MAGIC_LVEXP, 0, attackMagic.Magic.MagicId, attackMagic.Level, attackMagic.TranPoint, "", 3000);
                                }
                            }
                        }
                        if (bo21 && (MagicArr[MagicConst.SKILL_YEDO] != null))
                        {
                            attackMagic = GetAttrackMagic(MagicConst.SKILL_YEDO);
                            if ((attackMagic.Level < 3) && (attackMagic.Magic.TrainLevel[attackMagic.Level] <= Abil.Level))
                            {
                                (this as PlayObject).TrainSkill(attackMagic, M2Share.RandomNumber.Random(3) + 1);
                                if (!(this as PlayObject).CheckMagicLevelup(attackMagic))
                                {
                                    SendDelayMsg(this, Grobal2.RM_MAGIC_LVEXP, 0, attackMagic.Magic.MagicId, attackMagic.Level, attackMagic.TranPoint, "", 3000);
                                }
                            }
                        }
                        switch (wHitMode)
                        {
                            case 4:
                                attackMagic = GetAttrackMagic(MagicConst.SKILL_ERGUM);
                                if (attackMagic != null)
                                {
                                    if (attackMagic.Level < 3 && (attackMagic.Magic.TrainLevel[attackMagic.Level] <= Abil.Level))
                                    {
                                        (this as PlayObject).TrainSkill(attackMagic, 1);
                                        if (!(this as PlayObject).CheckMagicLevelup(attackMagic))
                                        {
                                            SendDelayMsg(this.ActorId, Grobal2.RM_MAGIC_LVEXP, 0, attackMagic.Magic.MagicId, attackMagic.Level, attackMagic.TranPoint, "", 3000);
                                        }
                                    }
                                }
                                break;
                            case 5:
                                attackMagic = GetAttrackMagic(MagicConst.SKILL_BANWOL);
                                if (attackMagic != null)
                                {
                                    if ((attackMagic.Level < 3) && (attackMagic.Magic.TrainLevel[attackMagic.Level] <= Abil.Level))
                                    {
                                        (this as PlayObject).TrainSkill(attackMagic, 1);
                                        if (!(this as PlayObject).CheckMagicLevelup(attackMagic))
                                        {
                                            SendDelayMsg(this.ActorId, Grobal2.RM_MAGIC_LVEXP, 0, attackMagic.Magic.MagicId, attackMagic.Level, attackMagic.TranPoint, "", 3000);
                                        }
                                    }
                                }
                                break;
                            case 12:
                                attackMagic = GetAttrackMagic(MagicConst.SKILL_REDBANWOL);
                                if (attackMagic != null)
                                {
                                    if ((attackMagic.Level < 3) && (attackMagic.Magic.TrainLevel[attackMagic.Level] <= Abil.Level))
                                    {
                                        (this as PlayObject).TrainSkill(attackMagic, 1);
                                        if (!(this as PlayObject).CheckMagicLevelup(attackMagic))
                                        {
                                            SendDelayMsg(this.ActorId, Grobal2.RM_MAGIC_LVEXP, 0, attackMagic.Magic.MagicId, attackMagic.Level, attackMagic.TranPoint, "", 3000);
                                        }
                                    }
                                }
                                break;
                            case 7:
                                attackMagic = GetAttrackMagic(MagicConst.SKILL_FIRESWORD);
                                if (attackMagic != null)
                                {
                                    if ((attackMagic.Level < 3) && (attackMagic.Magic.TrainLevel[attackMagic.Level] <= Abil.Level))
                                    {
                                        (this as PlayObject).TrainSkill(attackMagic, 1);
                                        if (!(this as PlayObject).CheckMagicLevelup(attackMagic))
                                        {
                                            SendDelayMsg(this, Grobal2.RM_MAGIC_LVEXP, 0, attackMagic.Magic.MagicId, attackMagic.Level, attackMagic.TranPoint, "", 3000);
                                        }
                                    }
                                }
                                break;
                            case 8:
                                attackMagic = GetAttrackMagic(MagicConst.SKILL_CROSSMOON);
                                if (attackMagic != null)
                                {
                                    if ((attackMagic.Level < 3) && (attackMagic.Magic.TrainLevel[attackMagic.Level] <= Abil.Level))
                                    {
                                        (this as PlayObject).TrainSkill(attackMagic, 1);
                                        if (!(this as PlayObject).CheckMagicLevelup(attackMagic))
                                        {
                                            SendDelayMsg(this, Grobal2.RM_MAGIC_LVEXP, 0, attackMagic.Magic.MagicId, attackMagic.Level, attackMagic.TranPoint, "", 3000);
                                        }
                                    }
                                }
                                break;
                            case 9:
                                attackMagic = GetAttrackMagic(MagicConst.SKILL_TWINBLADE);
                                if (attackMagic != null)
                                {
                                    if ((attackMagic.Level < 3) && (attackMagic.Magic.TrainLevel[attackMagic.Level] <= Abil.Level))
                                    {
                                        (this as PlayObject).TrainSkill(attackMagic, 1);
                                        if (!(this as PlayObject).CheckMagicLevelup(attackMagic))
                                        {
                                            SendDelayMsg(this, Grobal2.RM_MAGIC_LVEXP, 0, attackMagic.Magic.MagicId, attackMagic.Level, attackMagic.TranPoint, "", 3000);
                                        }
                                    }
                                }
                                break;
                        }
                    }
                    result = true;
                    if (M2Share.Config.MonDelHptoExp)
                    {
                        switch (Race)
                        {
                            case ActorRace.Play:
                                if (this.IsRobot)
                                {
                                    if ((this as RobotPlayObject).Abil.Level <= M2Share.Config.MonHptoExpLevel)
                                    {
                                        if (!M2Share.GetNoHptoexpMonList(AttackTarget.ChrName))
                                        {
                                            (this as RobotPlayObject).GainExp(nPower * M2Share.Config.MonHptoExpmax);
                                        }
                                    }
                                }
                                else
                                {
                                    if ((this as PlayObject).Abil.Level <= M2Share.Config.MonHptoExpLevel)
                                    {
                                        if (!M2Share.GetNoHptoexpMonList(AttackTarget.ChrName))
                                        {
                                            (this as PlayObject).GainExp(nPower * M2Share.Config.MonHptoExpmax);
                                        }
                                    }
                                }
                                break;
                            case ActorRace.PlayClone:
                                if (Master != null)
                                {
                                    if (Master.IsRobot)
                                    {
                                        if ((Master as RobotPlayObject).Abil.Level <= M2Share.Config.MonHptoExpLevel)
                                        {
                                            if (!M2Share.GetNoHptoexpMonList(AttackTarget.ChrName))
                                            {
                                                (Master as RobotPlayObject).GainExp(nPower * M2Share.Config.MonHptoExpmax);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if ((Master as PlayObject).Abil.Level <= M2Share.Config.MonHptoExpLevel)
                                        {
                                            if (!M2Share.GetNoHptoexpMonList(AttackTarget.ChrName))
                                            {
                                                (Master as PlayObject).GainExp(nPower * M2Share.Config.MonHptoExpmax);
                                            }
                                        }
                                    }
                                }
                                break;
                        }
                    }
                }
                if (Race == ActorRace.Play)
                {
                    TrainCurrentSkill(wHitMode);
                }
                if ((nWeaponDamage > 0) && (UseItems[Grobal2.U_WEAPON] != null) && (UseItems[Grobal2.U_WEAPON].Index > 0))
                {
                    DoDamageWeapon(nWeaponDamage);
                }
                if (AttackTarget.Race != ActorRace.Play)
                {
                    AttackTarget.SendMsg(AttackTarget, Grobal2.RM_STRUCK, nPower, AttackTarget.WAbil.HP, AttackTarget.WAbil.MaxHP, ActorId, "");
                }
            }
            catch (Exception e)
            {
                M2Share.Log.Error(e.Message);
            }
            return result;
        }

        /// <summary>
        /// 检查武器升级状态
        /// </summary>
        private void CheckWeaponUpgradeStatus(ref UserItem userItem)
        {
            if ((userItem.Desc[0] + userItem.Desc[1] + userItem.Desc[2]) < M2Share.Config.UpgradeWeaponMaxPoint)
            {
                if (userItem.Desc[ItemAttr.WeaponUpgrade] == 1)
                {
                    userItem.Index = 0;
                }
                if (HUtil32.RangeInDefined(userItem.Desc[ItemAttr.WeaponUpgrade], 10, 13))
                {
                    userItem.Desc[0] = (byte)(userItem.Desc[0] + userItem.Desc[ItemAttr.WeaponUpgrade] - 9);
                }
                if (HUtil32.RangeInDefined(userItem.Desc[ItemAttr.WeaponUpgrade], 20, 23))
                {
                    userItem.Desc[1] = (byte)(userItem.Desc[1] + userItem.Desc[ItemAttr.WeaponUpgrade] - 19);
                }
                if (HUtil32.RangeInDefined(userItem.Desc[ItemAttr.WeaponUpgrade], 30, 33))
                {
                    userItem.Desc[2] = (byte)(userItem.Desc[2] + userItem.Desc[ItemAttr.WeaponUpgrade] - 29);
                }
            }
            else
            {
                userItem.Index = 0;
            }
            userItem.Desc[ItemAttr.WeaponUpgrade] = 0;
        }

        private void CheckWeaponUpgrade()
        {
            if (UseItems[Grobal2.U_WEAPON] != null && UseItems[Grobal2.U_WEAPON].Desc[ItemAttr.WeaponUpgrade] > 0) //检车武器是否升级
            {
                var useItems = new UserItem(UseItems[Grobal2.U_WEAPON]);
                CheckWeaponUpgradeStatus(ref UseItems[Grobal2.U_WEAPON]);
                PlayObject PlayObject;
                StdItem StdItem;
                if (UseItems[Grobal2.U_WEAPON].Index == 0)
                {
                    SysMsg(M2Share.TheWeaponBroke, MsgColor.Red, MsgType.Hint);
                    PlayObject = this as PlayObject;
                    PlayObject.SendDelItems(useItems);
                    SendRefMsg(Grobal2.RM_BREAKWEAPON, 0, 0, 0, 0, "");
                    StdItem = M2Share.WorldEngine.GetStdItem(useItems.Index);
                    if (StdItem != null)
                    {
                        if (StdItem.NeedIdentify == 1)
                        {
                            M2Share.EventSource.AddEventLog(21, MapName + "\t" + CurrX + "\t" + CurrY + "\t" + ChrName + "\t" + StdItem.Name + "\t" + useItems.MakeIndex + "\t" + '1' + "\t" + '0');
                        }
                    }
                    FeatureChanged();
                }
                else
                {
                    SysMsg(M2Share.TheWeaponRefineSuccessfull, MsgColor.Red, MsgType.Hint);
                    PlayObject = this as PlayObject;
                    PlayObject.SendUpdateItem(UseItems[Grobal2.U_WEAPON]);
                    StdItem = M2Share.WorldEngine.GetStdItem(useItems.Index);
                    if (StdItem.NeedIdentify == 1)
                    {
                        M2Share.EventSource.AddEventLog(20, MapName + "\t" + CurrX + "\t" + CurrY + "\t" + ChrName + "\t" + StdItem.Name + "\t" + useItems.MakeIndex + "\t" + '1' + "\t" + '0');
                    }
                    RecalcAbilitys();
                    SendMsg(this, Grobal2.RM_ABILITY, 0, 0, 0, 0, "");
                    SendMsg(this, Grobal2.RM_SUBABILITY, 0, 0, 0, 0, "");
                }
            }
        }

        // 攻击角色
        private bool _Attack_DirectAttack(BaseObject BaseObject, int nSecPwr)
        {
            bool result = false;
            if ((Race == ActorRace.Play) || (BaseObject.Race == ActorRace.Play) || !(InSafeZone() && BaseObject.InSafeZone()))
            {
                if (IsProperTarget(BaseObject))
                {
                    if (M2Share.RandomNumber.RandomByte(BaseObject.SpeedPoint) < HitPoint)
                    {
                        BaseObject.StruckDamage((ushort)nSecPwr);
                        BaseObject.SendDelayMsg(Grobal2.RM_STRUCK, Grobal2.RM_REFMESSAGE, nSecPwr, BaseObject.WAbil.HP, BaseObject.WAbil.MaxHP, ActorId, "", 500);
                        if (BaseObject.Race != ActorRace.Play)
                        {
                            BaseObject.SendMsg(BaseObject, Grobal2.RM_STRUCK, nSecPwr, BaseObject.WAbil.HP, BaseObject.WAbil.MaxHP, ActorId, "");
                        }
                        result = true;
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 刺杀前面一个位置的攻击
        /// </summary>
        /// <param name="nSecPwr"></param>
        /// <returns></returns>
        private bool SwordLongAttack(ref int nSecPwr)
        {
            bool result = false;
            short nX = 0;
            short nY = 0;
            nSecPwr = HUtil32.Round(nSecPwr * M2Share.Config.SwordLongPowerRate / 100);
            if (Envir.GetNextPosition(CurrX, CurrY, Direction, 2, ref nX, ref nY))
            {
                BaseObject baseObject = (BaseObject)Envir.GetMovingObject(nX, nY, true);
                if (baseObject != null)
                {
                    if ((nSecPwr > 0) && IsProperTarget(baseObject))
                    {
                        result = _Attack_DirectAttack(baseObject, nSecPwr);
                        SetTargetCreat(baseObject);
                    }
                    result = true;
                }
            }
            return result;
        }

        /// <summary>
        /// 半月攻击
        /// </summary>
        /// <param name="nSecPwr"></param>
        /// <returns></returns>
        private bool SwordWideAttack(ref int nSecPwr)
        {
            bool result = false;
            byte nC = 0;
            short nX = 0;
            short nY = 0;
            while (true)
            {
                var nDir = (byte)((Direction + M2Share.Config.WideAttack[nC]) % 8);
                if (Envir.GetNextPosition(CurrX, CurrY, nDir, 1, ref nX, ref nY))
                {
                    var BaseObject = (BaseObject)Envir.GetMovingObject(nX, nY, true);
                    if ((nSecPwr > 0) && (BaseObject != null) && IsProperTarget(BaseObject))
                    {
                        result = _Attack_DirectAttack(BaseObject, nSecPwr);
                        SetTargetCreat(BaseObject);
                    }
                }
                nC++;
                if (nC >= 3)
                {
                    break;
                }
            }
            return result;
        }

        private bool CrsWideAttack(int nSecPwr)
        {
            bool result = false;
            int nC = 0;
            short nX = 0;
            short nY = 0;
            while (true)
            {
                byte nDir = (byte)((Direction + M2Share.Config.CrsAttack[nC]) % 8);
                if (Envir.GetNextPosition(CurrX, CurrY, nDir, 1, ref nX, ref nY))
                {
                    var BaseObject = (BaseObject)Envir.GetMovingObject(nX, nY, true);
                    if ((nSecPwr > 0) && (BaseObject != null) && IsProperTarget(BaseObject))
                    {
                        result = _Attack_DirectAttack(BaseObject, nSecPwr);
                        SetTargetCreat(BaseObject);
                    }
                }
                nC++;
                if (nC >= 7)
                {
                    break;
                }
            }
            return result;
        }

        private void TrainCurrentSkill(int wHitMode)
        {
            int nCLevel = Abil.Level;
            if (Race != ActorRace.Play)
            {
                return;
            }
            if (this is not PlayObject playObject)
            {
                return;
            }
            if ((MagicArr[MagicConst.SKILL_ONESWORD] != null))
            {
                if ((MagicArr[MagicConst.SKILL_ONESWORD].Level < MagicArr[MagicConst.SKILL_ONESWORD].Magic.TrainLv) && (MagicArr[MagicConst.SKILL_ONESWORD].Magic.TrainLevel[MagicArr[MagicConst.SKILL_ONESWORD].Level] <= nCLevel))
                {
                    playObject.TrainSkill(MagicArr[MagicConst.SKILL_ONESWORD], M2Share.RandomNumber.Random(3) + 1);
                    if (!playObject.CheckMagicLevelup(MagicArr[MagicConst.SKILL_ONESWORD]))
                    {
                        SendDelayMsg(this, Grobal2.RM_MAGIC_LVEXP, 0, MagicArr[MagicConst.SKILL_ONESWORD].Magic.MagicId, MagicArr[MagicConst.SKILL_ONESWORD].Level, MagicArr[MagicConst.SKILL_ONESWORD].TranPoint, "", 3000);
                    }
                }
            }
            if ((MagicArr[MagicConst.SKILL_ILKWANG] != null))
            {
                if ((MagicArr[MagicConst.SKILL_ILKWANG].Level < MagicArr[MagicConst.SKILL_ILKWANG].Magic.TrainLv) && (MagicArr[MagicConst.SKILL_ILKWANG].Magic.TrainLevel[MagicArr[MagicConst.SKILL_ILKWANG].Level] <= nCLevel))
                {
                    playObject.TrainSkill(MagicArr[MagicConst.SKILL_ILKWANG], M2Share.RandomNumber.Random(3) + 1);
                    if (!playObject.CheckMagicLevelup(MagicArr[MagicConst.SKILL_ILKWANG]))
                    {
                        SendDelayMsg(this, Grobal2.RM_MAGIC_LVEXP, 0, MagicArr[MagicConst.SKILL_ILKWANG].Magic.MagicId, MagicArr[MagicConst.SKILL_ILKWANG].Level, MagicArr[MagicConst.SKILL_ILKWANG].TranPoint, "", 3000);
                    }
                }
            }
            switch (wHitMode)
            {
                case 3 when (MagicArr[MagicConst.SKILL_YEDO] != null):
                    {
                        if ((MagicArr[MagicConst.SKILL_YEDO].Level < MagicArr[MagicConst.SKILL_YEDO].Magic.TrainLv) && (MagicArr[MagicConst.SKILL_YEDO].Magic.TrainLevel[MagicArr[MagicConst.SKILL_YEDO].Level] <= nCLevel))
                        {
                            playObject.TrainSkill(MagicArr[MagicConst.SKILL_YEDO], M2Share.RandomNumber.Random(3) + 1);
                            if (!playObject.CheckMagicLevelup(MagicArr[MagicConst.SKILL_YEDO]))
                            {
                                SendDelayMsg(this, Grobal2.RM_MAGIC_LVEXP, 0, MagicArr[MagicConst.SKILL_YEDO].Magic.MagicId, MagicArr[MagicConst.SKILL_YEDO].Level, MagicArr[MagicConst.SKILL_YEDO].TranPoint, "", 3000);
                            }
                        }
                        break;
                    }
                case 4 when (MagicArr[MagicConst.SKILL_ERGUM] != null):
                    {
                        if ((MagicArr[MagicConst.SKILL_ERGUM].Level < MagicArr[MagicConst.SKILL_ERGUM].Magic.TrainLv) && (MagicArr[MagicConst.SKILL_ERGUM].Magic.TrainLevel[MagicArr[MagicConst.SKILL_ERGUM].Level] <= nCLevel))
                        {
                            playObject.TrainSkill(MagicArr[MagicConst.SKILL_ERGUM], 1);
                            if (!playObject.CheckMagicLevelup(MagicArr[MagicConst.SKILL_ERGUM]))
                            {
                                SendDelayMsg(this, Grobal2.RM_MAGIC_LVEXP, 0, MagicArr[MagicConst.SKILL_ERGUM].Magic.MagicId, MagicArr[MagicConst.SKILL_ERGUM].Level, MagicArr[MagicConst.SKILL_ERGUM].TranPoint, "", 3000);
                            }
                        }
                        break;
                    }
                case 5 when (MagicArr[MagicConst.SKILL_BANWOL] != null):
                    {
                        if ((MagicArr[MagicConst.SKILL_BANWOL].Level < MagicArr[MagicConst.SKILL_BANWOL].Magic.TrainLv) && (MagicArr[MagicConst.SKILL_BANWOL].Magic.TrainLevel[MagicArr[MagicConst.SKILL_BANWOL].Level] <= nCLevel))
                        {
                            playObject.TrainSkill(MagicArr[MagicConst.SKILL_BANWOL], 1);
                            if (!playObject.CheckMagicLevelup(MagicArr[MagicConst.SKILL_BANWOL]))
                            {
                                SendDelayMsg(this, Grobal2.RM_MAGIC_LVEXP, 0, MagicArr[MagicConst.SKILL_BANWOL].Magic.MagicId, MagicArr[MagicConst.SKILL_BANWOL].Level, MagicArr[MagicConst.SKILL_BANWOL].TranPoint, "", 3000);
                            }
                        }
                        break;
                    }
                case 7 when (MagicArr[MagicConst.SKILL_FIRESWORD] != null):
                    {
                        if ((MagicArr[MagicConst.SKILL_FIRESWORD].Level < MagicArr[MagicConst.SKILL_FIRESWORD].Magic.TrainLv) && (MagicArr[MagicConst.SKILL_FIRESWORD].Magic.TrainLevel[MagicArr[MagicConst.SKILL_FIRESWORD].Level] <= nCLevel))
                        {
                            playObject.TrainSkill(MagicArr[MagicConst.SKILL_FIRESWORD], 1);
                            if (!playObject.CheckMagicLevelup(MagicArr[MagicConst.SKILL_FIRESWORD]))
                            {
                                SendDelayMsg(this, Grobal2.RM_MAGIC_LVEXP, 0, MagicArr[MagicConst.SKILL_FIRESWORD].Magic.MagicId, MagicArr[MagicConst.SKILL_FIRESWORD].Level, MagicArr[MagicConst.SKILL_FIRESWORD].TranPoint, "", 3000);
                            }
                        }
                        break;
                    }
                case 9 when (MagicArr[43] != null):
                    {
                        if ((MagicArr[43].Level < MagicArr[43].Magic.TrainLv) && (MagicArr[43].Magic.TrainLevel[MagicArr[43].Level] <= nCLevel))
                        {
                            playObject.TrainSkill(MagicArr[43], 1);
                            if (!playObject.CheckMagicLevelup(MagicArr[43]))
                            {
                                SendDelayMsg(this, Grobal2.RM_MAGIC_LVEXP, 0, MagicArr[43].Magic.MagicId, MagicArr[43].Level, MagicArr[43].TranPoint, "", 3000);
                            }
                        }
                        break;
                    }
                case 13 when (MagicArr[56] != null):
                    {
                        if ((MagicArr[56].Level < MagicArr[56].Magic.TrainLv) && (MagicArr[56].Magic.TrainLevel[MagicArr[56].Level] <= nCLevel))
                        {
                            playObject.TrainSkill(MagicArr[56], 1);
                            if (!playObject.CheckMagicLevelup(MagicArr[56]))
                            {
                                SendDelayMsg(this, Grobal2.RM_MAGIC_LVEXP, 0, MagicArr[56].Magic.MagicId, MagicArr[56].Level, MagicArr[56].TranPoint, "", 3000);
                            }
                        }
                        break;
                    }
                case 8 when (MagicArr[40] != null):
                    {
                        if ((MagicArr[40].Level < MagicArr[40].Magic.TrainLv) && (MagicArr[40].Magic.TrainLevel[MagicArr[40].Level] <= nCLevel))
                        {
                            playObject.TrainSkill(MagicArr[40], 1);
                            if (!playObject.CheckMagicLevelup(MagicArr[40]))
                            {
                                SendDelayMsg(this, Grobal2.RM_MAGIC_LVEXP, 0, MagicArr[40].Magic.MagicId, MagicArr[40].Level, MagicArr[40].TranPoint, "", 3000);
                            }
                        }
                        break;
                    }
                case 10 when (MagicArr[42] != null):
                    {
                        if ((MagicArr[42].Level < MagicArr[42].Magic.TrainLv) && (MagicArr[42].Magic.TrainLevel[MagicArr[42].Level] <= nCLevel))
                        {
                            playObject.TrainSkill(MagicArr[42], 1);
                            if (!playObject.CheckMagicLevelup(MagicArr[42]))
                            {
                                SendDelayMsg(this, Grobal2.RM_MAGIC_LVEXP, 0, MagicArr[42].Magic.MagicId, MagicArr[42].Level, MagicArr[42].TranPoint, "", 3000);
                            }
                        }
                        break;
                    }
                case 12 when (MagicArr[66] != null):
                    {
                        if ((MagicArr[66].Level < MagicArr[66].Magic.TrainLv) && (MagicArr[66].Magic.TrainLevel[MagicArr[66].Level] <= nCLevel))
                        {
                            playObject.TrainSkill(MagicArr[66], 1);
                            if (!playObject.CheckMagicLevelup(MagicArr[66]))
                            {
                                SendDelayMsg(this, Grobal2.RM_MAGIC_LVEXP, 0, MagicArr[66].Magic.MagicId, MagicArr[66].Level, MagicArr[66].TranPoint, "", 3000);
                            }
                        }
                        break;
                    }
                case 61 when (MagicArr[61] != null):
                    {
                        if ((MagicArr[61].Level < MagicArr[61].Magic.TrainLv) && (MagicArr[61].Magic.TrainLevel[MagicArr[61].Level] <= nCLevel))
                        {
                            playObject.TrainSkill(MagicArr[61], 1);
                            if (!playObject.CheckMagicLevelup(MagicArr[61]))
                            {
                                SendDelayMsg(this, Grobal2.RM_MAGIC_LVEXP, 0, MagicArr[61].Magic.MagicId, MagicArr[61].Level, MagicArr[61].TranPoint, "", 3000);
                            }
                        }
                        break;
                    }
                case 20 when (MagicArr[101] != null):
                    {
                        if (MagicArr[101].Magic.TrainLevel[MagicArr[101].Level] <= nCLevel)
                        {
                            playObject.TrainSkill(MagicArr[101], 1);
                            if (!playObject.CheckMagicLevelup(MagicArr[101]))
                            {
                                SendDelayMsg(this, Grobal2.RM_MAGIC_LVEXP, 0, MagicArr[101].Magic.MagicId, MagicArr[101].Level, MagicArr[101].TranPoint, "", 3000);
                            }
                        }
                        break;
                    }
                case 21 when (MagicArr[102] != null):
                    {
                        if (MagicArr[102].Magic.TrainLevel[MagicArr[102].Level] <= nCLevel)
                        {
                            playObject.TrainSkill(MagicArr[102], 1);
                            if (!playObject.CheckMagicLevelup(MagicArr[102]))
                            {
                                SendDelayMsg(this, Grobal2.RM_MAGIC_LVEXP, 0, MagicArr[102].Magic.MagicId, MagicArr[102].Level, MagicArr[102].TranPoint, "", 3000);
                            }
                        }
                        break;
                    }
                case 22 when (MagicArr[103] != null):
                    {
                        if (MagicArr[103].Magic.TrainLevel[MagicArr[103].Level] <= nCLevel)
                        {
                            playObject.TrainSkill(MagicArr[103], 1);
                            if (!playObject.CheckMagicLevelup(MagicArr[103]))
                            {
                                SendDelayMsg(this, Grobal2.RM_MAGIC_LVEXP, 0, MagicArr[103].Magic.MagicId, MagicArr[103].Level, MagicArr[103].TranPoint, "", 3000);
                            }
                        }
                        break;
                    }
                case 23 when (MagicArr[114] != null):
                    {
                        if (MagicArr[114].Magic.TrainLevel[MagicArr[114].Level] <= nCLevel)
                        {
                            playObject.TrainSkill(MagicArr[114], 1);
                            if (!playObject.CheckMagicLevelup(MagicArr[114]))
                            {
                                SendDelayMsg(this, Grobal2.RM_MAGIC_LVEXP, 0, MagicArr[114].Magic.MagicId, MagicArr[114].Level, MagicArr[114].TranPoint, "", 3000);
                            }
                        }
                        break;
                    }
                case 24 when (MagicArr[113] != null):
                    {
                        if (MagicArr[113].Magic.TrainLevel[MagicArr[113].Level] <= nCLevel)
                        {
                            playObject.TrainSkill(MagicArr[113], 1);
                            if (!playObject.CheckMagicLevelup(MagicArr[113]))
                            {
                                SendDelayMsg(this, Grobal2.RM_MAGIC_LVEXP, 0, MagicArr[113].Magic.MagicId, MagicArr[113].Level, MagicArr[113].TranPoint, "", 3000);
                            }
                        }
                        break;
                    }
                case 25 when (MagicArr[115] != null):
                    {
                        if (MagicArr[115].Magic.TrainLevel[MagicArr[115].Level] <= nCLevel)
                        {
                            playObject.TrainSkill(MagicArr[115], 1);
                            if (!playObject.CheckMagicLevelup(MagicArr[115]))
                            {
                                SendDelayMsg(this, Grobal2.RM_MAGIC_LVEXP, 0, MagicArr[115].Magic.MagicId, MagicArr[115].Level, MagicArr[115].TranPoint, "", 3000);
                            }
                        }
                        break;
                    }
            }
        }

        private UserMagic GetAttrackMagic(int magicId)
        {
            return MagicArr[magicId];
        }

        protected void SendAttackMsg(int wIdent, byte btDir, short nX, short nY)
        {
            SendRefMsg(wIdent, btDir, nX, nY, 0, "");
        }
    }
}
