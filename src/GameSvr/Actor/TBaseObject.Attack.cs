using GameSvr.Items;
using GameSvr.Magic;
using GameSvr.Player;
using GameSvr.RobotPlay;
using SystemModule;
using SystemModule.Data;
using SystemModule.Packet.ClientPackets;

namespace GameSvr.Actor
{
    public partial class TBaseObject
    {
        protected virtual void AttackDir(TBaseObject TargeTBaseObject, short wHitMode, byte nDir)
        {
            TBaseObject AttackTarget;
            const string sExceptionMsg = "[Exception] TBaseObject::AttackDir";
            try
            {
                if ((wHitMode == 5) && (MagicArr[SpellsDef.SKILL_BANWOL] != null)) // 半月
                {
                    if (m_WAbil.MP > 0)
                    {
                        DamageSpell((ushort)(MagicArr[SpellsDef.SKILL_BANWOL].MagicInfo.btDefSpell + GetMagicSpell(MagicArr[SpellsDef.SKILL_BANWOL])));
                        HealthSpellChanged();
                    }
                    else
                    {
                        wHitMode = Grobal2.RM_HIT;
                    }
                }
                if ((wHitMode == 12) && (MagicArr[SpellsDef.SKILL_REDBANWOL] != null))
                {
                    if (m_WAbil.MP > 0)
                    {
                        DamageSpell((ushort)(MagicArr[SpellsDef.SKILL_REDBANWOL].MagicInfo.btDefSpell + GetMagicSpell(MagicArr[SpellsDef.SKILL_REDBANWOL])));
                        HealthSpellChanged();
                    }
                    else
                    {
                        wHitMode = Grobal2.RM_HIT;
                    }
                }
                if ((wHitMode == 8) && (MagicArr[SpellsDef.SKILL_CROSSMOON] != null))
                {
                    if (m_WAbil.MP > 0)
                    {
                        DamageSpell((ushort)(MagicArr[SpellsDef.SKILL_CROSSMOON].MagicInfo.btDefSpell + GetMagicSpell(MagicArr[SpellsDef.SKILL_CROSSMOON])));
                        HealthSpellChanged();
                    }
                    else
                    {
                        wHitMode = Grobal2.RM_HIT;
                    }
                }
                Direction = nDir;
                if (TargeTBaseObject == null)
                {
                    AttackTarget = GetPoseCreate();
                }
                else
                {
                    AttackTarget = TargeTBaseObject;
                }
                if (UseItems[Grobal2.U_WEAPON] != null && UseItems[Grobal2.U_WEAPON].btValue[ItemAttr.WeaponUpgrade] > 0)
                {
                    if ((AttackTarget != null) && (UseItems[Grobal2.U_WEAPON].wIndex > 0))
                    {
                        CheckWeaponUpgrade();
                    }
                }
                var boPowerHit = PowerHit;
                var boFireHit = FireHitSkill;
                var bo41 = m_bo41kill;
                var boTwinHit = m_boTwinHitSkill;
                if (_Attack(ref wHitMode, AttackTarget))
                {
                    SetTargetCreat(AttackTarget);
                }
                short wIdent = Grobal2.RM_HIT;
                if (Race == Grobal2.RC_PLAYOBJECT)
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
                            if (MagicArr[SpellsDef.SKILL_ERGUM] != null)
                            {
                                wIdent = Grobal2.RM_LONGHIT;
                            }
                            break;
                        case 5:
                            if (MagicArr[SpellsDef.SKILL_BANWOL] != null)
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
                            if (MagicArr[SpellsDef.SKILL_CROSSMOON] != null)
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
                            if (MagicArr[SpellsDef.SKILL_REDBANWOL] != null)
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
                M2Share.LogSystem.Error(sExceptionMsg);
                M2Share.LogSystem.Error(e.Message);
            }
        }

        protected void SendAttackMsg(short wIdent, byte btDir, int nX, int nY)
        {
            SendRefMsg(wIdent, btDir, nX, nY, 0, "");
        }

        /// <summary>
        /// 检查武器升级状态
        /// </summary>
        /// <param name="UserItem"></param>
        private void CheckWeaponUpgradeStatus(ref TUserItem UserItem)
        {
            if ((UserItem.btValue[0] + UserItem.btValue[1] + UserItem.btValue[2]) < M2Share.Config.nUpgradeWeaponMaxPoint)
            {
                if (UserItem.btValue[ItemAttr.WeaponUpgrade] == 1)
                {
                    UserItem.wIndex = 0;
                }
                if (HUtil32.RangeInDefined(UserItem.btValue[ItemAttr.WeaponUpgrade], 10, 13))
                {
                    UserItem.btValue[0] = (byte)(UserItem.btValue[0] + UserItem.btValue[ItemAttr.WeaponUpgrade] - 9);
                }
                if (HUtil32.RangeInDefined(UserItem.btValue[ItemAttr.WeaponUpgrade], 20, 23))
                {
                    UserItem.btValue[1] = (byte)(UserItem.btValue[1] + UserItem.btValue[ItemAttr.WeaponUpgrade] - 19);
                }
                if (HUtil32.RangeInDefined(UserItem.btValue[ItemAttr.WeaponUpgrade], 30, 33))
                {
                    UserItem.btValue[2] = (byte)(UserItem.btValue[2] + UserItem.btValue[ItemAttr.WeaponUpgrade] - 29);
                }
            }
            else
            {
                UserItem.wIndex = 0;
            }
            UserItem.btValue[ItemAttr.WeaponUpgrade] = 0;
        }

        private void CheckWeaponUpgrade()
        {
            if (UseItems[Grobal2.U_WEAPON] != null && UseItems[Grobal2.U_WEAPON].btValue[ItemAttr.WeaponUpgrade] > 0) //检车武器是否升级
            {
                var useItems = new TUserItem(UseItems[Grobal2.U_WEAPON]);
                CheckWeaponUpgradeStatus(ref UseItems[Grobal2.U_WEAPON]);
                PlayObject PlayObject = null;
                StdItem StdItem = null;
                if (UseItems[Grobal2.U_WEAPON].wIndex == 0)
                {
                    SysMsg(M2Share.g_sTheWeaponBroke, MsgColor.Red, MsgType.Hint);
                    PlayObject = this as PlayObject;
                    PlayObject.SendDelItems(useItems);
                    SendRefMsg(Grobal2.RM_BREAKWEAPON, 0, 0, 0, 0, "");
                    StdItem = M2Share.UserEngine.GetStdItem(useItems.wIndex);
                    if (StdItem != null)
                    {
                        if (StdItem.NeedIdentify == 1)
                        {
                            M2Share.AddGameDataLog("21" + "\t" + MapName + "\t" + CurrX + "\t" + CurrY + "\t" + CharName + "\t" + StdItem.Name + "\t" + useItems.MakeIndex + "\t" + '1' + "\t" + '0');
                        }
                    }
                    FeatureChanged();
                }
                else
                {
                    SysMsg(M2Share.sTheWeaponRefineSuccessfull, MsgColor.Red, MsgType.Hint);
                    PlayObject = this as PlayObject;
                    PlayObject.SendUpdateItem(UseItems[Grobal2.U_WEAPON]);
                    StdItem = M2Share.UserEngine.GetStdItem(useItems.wIndex);
                    if (StdItem.NeedIdentify == 1)
                    {
                        M2Share.AddGameDataLog("20" + "\t" + MapName + "\t" + CurrX + "\t" + CurrY + "\t" + CharName + "\t" + StdItem.Name + "\t" + useItems.MakeIndex + "\t" + '1' + "\t" + '0');
                    }
                    RecalcAbilitys();
                    SendMsg(this, Grobal2.RM_ABILITY, 0, 0, 0, 0, "");
                    SendMsg(this, Grobal2.RM_SUBABILITY, 0, 0, 0, 0, "");
                }
            }
        }

        // 攻击角色
        private bool _Attack_DirectAttack(TBaseObject BaseObject, int nSecPwr)
        {
            bool result = false;
            if ((Race == Grobal2.RC_PLAYOBJECT) || (BaseObject.Race == Grobal2.RC_PLAYOBJECT) || !(InSafeZone() && BaseObject.InSafeZone()))
            {
                if (IsProperTarget(BaseObject))
                {
                    if (M2Share.RandomNumber.Random(BaseObject.SpeedPoint) < m_btHitPoint)
                    {
                        BaseObject.StruckDamage(nSecPwr);
                        BaseObject.SendDelayMsg(Grobal2.RM_STRUCK, Grobal2.RM_10101, (short)nSecPwr, BaseObject.m_WAbil.HP, BaseObject.m_WAbil.MaxHP, ObjectId, "", 500);
                        if (BaseObject.Race != Grobal2.RC_PLAYOBJECT)
                        {
                            BaseObject.SendMsg(BaseObject, Grobal2.RM_STRUCK, (short)nSecPwr, BaseObject.m_WAbil.HP, BaseObject.m_WAbil.MaxHP, ObjectId, "");
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
            nSecPwr = HUtil32.Round(nSecPwr * M2Share.Config.nSwordLongPowerRate / 100);
            if (Envir.GetNextPosition(CurrX, CurrY, Direction, 2, ref nX, ref nY))
            {
                TBaseObject BaseObject = (TBaseObject)Envir.GetMovingObject(nX, nY, true);
                if (BaseObject != null)
                {
                    if ((nSecPwr > 0) && IsProperTarget(BaseObject))
                    {
                        result = _Attack_DirectAttack(BaseObject, nSecPwr);
                        SetTargetCreat(BaseObject);
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
            int nC = 0;
            short nX = 0;
            short nY = 0;
            while (true)
            {
                var n10 = (Direction + M2Share.Config.WideAttack[nC]) % 8;
                if (Envir.GetNextPosition(CurrX, CurrY, n10, 1, ref nX, ref nY))
                {
                    var BaseObject = (TBaseObject)Envir.GetMovingObject(nX, nY, true);
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
            int n10 = 0;
            short nX = 0;
            short nY = 0;
            while (true)
            {
                n10 = (Direction + M2Share.Config.CrsAttack[nC]) % 8;
                if (Envir.GetNextPosition(CurrX, CurrY, n10, 1, ref nX, ref nY))
                {
                    var BaseObject = (TBaseObject)Envir.GetMovingObject(nX, nY, true);
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

        private void _Attack_sub_4C1E5C_sub_4C1DC0(ref TBaseObject BaseObject, byte btDir, ref short nX, ref short nY, int nSecPwr)
        {
            if (Envir.GetNextPosition(CurrX, CurrY, btDir, 1, ref nX, ref nY))
            {
                BaseObject = (TBaseObject)Envir.GetMovingObject(nX, nY, true);
                if ((nSecPwr > 0) && (BaseObject != null))
                {
                    _Attack_DirectAttack(BaseObject, nSecPwr);
                }
            }
        }

        private void _Attack_sub_4C1E5C(int nSecPwr)
        {
            short nX = 0;
            short nY = 0;
            TBaseObject BaseObject = null;
            byte btDir = Direction;
            Envir.GetNextPosition(CurrX, CurrY, btDir, 1, ref nX, ref nY);
            _Attack_sub_4C1E5C_sub_4C1DC0(ref BaseObject, btDir, ref nX, ref nY, nSecPwr);
            btDir = M2Share.sub_4B2F80(Direction, 2);
            _Attack_sub_4C1E5C_sub_4C1DC0(ref BaseObject, btDir, ref nX, ref nY, nSecPwr);
            btDir = M2Share.sub_4B2F80(Direction, 6);
            _Attack_sub_4C1E5C_sub_4C1DC0(ref BaseObject, btDir, ref nX, ref nY, nSecPwr);
        }

        protected bool _Attack(ref short wHitMode, TBaseObject AttackTarget)
        {
            int n20;
            bool result = false;
            try
            {
                bool bo21 = false;
                int nWeaponDamage = 0;
                int nPower = 0;
                int nSecPwr = 0;
                if (AttackTarget != null)
                {
                    nPower = GetAttackPower(HUtil32.LoWord(m_WAbil.DC), HUtil32.HiWord(m_WAbil.DC) - HUtil32.LoWord(m_WAbil.DC));
                    if ((wHitMode == 3) && PowerHit)
                    {
                        PowerHit = false;
                        nPower += m_nHitPlus;
                        bo21 = true;
                    }
                    if ((wHitMode == 7) && FireHitSkill) // 烈火剑法
                    {
                        FireHitSkill = false;
                        m_dwLatestFireHitTick = HUtil32.GetTickCount();// Jacky 禁止双烈火
                        nPower = nPower + HUtil32.Round(nPower / 100 * (m_nHitDouble * 10));
                        bo21 = true;
                    }
                    if ((wHitMode == 9) && m_boTwinHitSkill) // 烈火剑法
                    {
                        m_boTwinHitSkill = false;
                        m_dwLatestTwinHitTick = HUtil32.GetTickCount();// Jacky 禁止双烈火
                        nPower = nPower + HUtil32.Round(nPower / 100 * (m_nHitDouble * 10));
                        bo21 = true;
                    }
                }
                else
                {
                    nPower = GetAttackPower(HUtil32.LoWord(m_WAbil.DC), HUtil32.HiWord(m_WAbil.DC) - HUtil32.LoWord(m_WAbil.DC));
                    if ((wHitMode == 3) && PowerHit)
                    {
                        PowerHit = false;
                        nPower += m_nHitPlus;
                        bo21 = true;
                    }
                    // Jacky 防止砍空刀刀烈火
                    if ((wHitMode == 7) && FireHitSkill)
                    {
                        FireHitSkill = false;
                        m_dwLatestFireHitTick = HUtil32.GetTickCount();// Jacky 禁止双烈火
                    }
                    if ((wHitMode == 9) && m_boTwinHitSkill)
                    {
                        m_boTwinHitSkill = false;
                        m_dwLatestTwinHitTick = HUtil32.GetTickCount();// Jacky 禁止双烈火
                    }
                }
                if (wHitMode == 4)
                {
                    // 刺杀
                    nSecPwr = 0;
                    if (Race == Grobal2.RC_PLAYOBJECT)
                    {
                        if (MagicArr[SpellsDef.SKILL_ERGUM] != null)
                        {
                            nSecPwr = HUtil32.Round(nPower / (MagicArr[SpellsDef.SKILL_ERGUM].MagicInfo.btTrainLv + 2) * (MagicArr[SpellsDef.SKILL_ERGUM].btLevel + 2));
                        }
                    }
                    if (nSecPwr > 0)
                    {
                        if (!SwordLongAttack(ref nSecPwr) && M2Share.Config.boLimitSwordLong)
                        {
                            wHitMode = 0;
                        }
                    }
                }
                if (wHitMode == 5)
                {
                    nSecPwr = 0;
                    if (Race == Grobal2.RC_PLAYOBJECT)
                    {
                        if (MagicArr[SpellsDef.SKILL_BANWOL] != null)
                        {
                            nSecPwr = HUtil32.Round(nPower / (MagicArr[SpellsDef.SKILL_BANWOL].MagicInfo.btTrainLv + 10) * (MagicArr[SpellsDef.SKILL_BANWOL].btLevel + 2));
                        }
                    }
                    if (nSecPwr > 0)
                    {
                        SwordWideAttack(ref nSecPwr);
                    }
                }
                if (wHitMode == 12)
                {
                    nSecPwr = 0;
                    if (Race == Grobal2.RC_PLAYOBJECT)
                    {
                        if (MagicArr[SpellsDef.SKILL_REDBANWOL] != null)
                        {
                            nSecPwr = HUtil32.Round(nPower / (MagicArr[SpellsDef.SKILL_REDBANWOL].MagicInfo.btTrainLv + 10) * (MagicArr[SpellsDef.SKILL_REDBANWOL].btLevel + 2));
                        }
                    }
                    if (nSecPwr > 0)
                    {
                        SwordWideAttack(ref nSecPwr);
                    }
                }
                if (wHitMode == 6)
                {
                    nSecPwr = 0;
                    if (nSecPwr > 0)
                    {
                        _Attack_sub_4C1E5C(nSecPwr);
                    }
                }
                if (wHitMode == 8)
                {
                    nSecPwr = 0;
                    if (Race == Grobal2.RC_PLAYOBJECT)
                    {
                        if (MagicArr[SpellsDef.SKILL_CROSSMOON] != null)
                        {
                            nSecPwr = HUtil32.Round(nPower / (MagicArr[SpellsDef.SKILL_CROSSMOON].MagicInfo.btTrainLv + 10) * (MagicArr[SpellsDef.SKILL_CROSSMOON].btLevel + 2));
                        }
                    }
                    if (nSecPwr > 0)
                    {
                        CrsWideAttack(nSecPwr);
                    }
                }
                if (AttackTarget == null)
                {
                    return result;
                }
                if (IsProperTarget(AttackTarget))
                {
                    if (AttackTarget.m_btHitPoint > 0)
                    {
                        if (m_btHitPoint < M2Share.RandomNumber.Random(AttackTarget.SpeedPoint))
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
                    nWeaponDamage = M2Share.RandomNumber.Random(5) + 2 - m_AddAbil.btWeaponStrong;
                }
                if (nPower > 0)
                {
                    AttackTarget.StruckDamage(nPower);
                    AttackTarget.SendDelayMsg(Grobal2.RM_STRUCK, Grobal2.RM_10101, nPower, AttackTarget.m_WAbil.HP, AttackTarget.m_WAbil.MaxHP, ObjectId, "", 200);
                    if (!AttackTarget.UnParalysis && Paralysis && (M2Share.RandomNumber.Random(AttackTarget.AntiPoison + M2Share.Config.AttackPosionRate) == 0))
                    {
                        AttackTarget.MakePosion(Grobal2.POISON_STONE, M2Share.Config.AttackPosionTime, 0);
                    }
                    if (m_nHongMoSuite > 0)// 虹魔，吸血
                    {
                        m_db3B0 = nPower / 100 * m_nHongMoSuite;
                        if (m_db3B0 >= 2.0)
                        {
                            n20 = Convert.ToInt32(m_db3B0);
                            m_db3B0 = n20;
                            DamageHealth(-n20);
                        }
                    }
                    if (Race == Grobal2.RC_PLAYOBJECT)
                    {
                        TUserMagic attackMagic = null;
                        if ((MagicArr[SpellsDef.SKILL_ILKWANG] != null))
                        {
                            attackMagic = GetAttrackMagic(SpellsDef.SKILL_ILKWANG);
                            if ((attackMagic.btLevel < 3) && (attackMagic.MagicInfo.TrainLevel[attackMagic.btLevel] <= Abil.Level))
                            {
                                (this as PlayObject).TrainSkill(attackMagic, M2Share.RandomNumber.Random(3) + 1);
                                if (!(this as PlayObject).CheckMagicLevelup(attackMagic))
                                {
                                    SendDelayMsg(this, Grobal2.RM_MAGIC_LVEXP, 0, attackMagic.MagicInfo.wMagicID, attackMagic.btLevel, attackMagic.nTranPoint, "", 3000);
                                }
                            }
                        }
                        if (bo21 && (MagicArr[SpellsDef.SKILL_YEDO] != null))
                        {
                            attackMagic = GetAttrackMagic(SpellsDef.SKILL_YEDO);
                            if ((attackMagic.btLevel < 3) && (attackMagic.MagicInfo.TrainLevel[attackMagic.btLevel] <= Abil.Level))
                            {
                                (this as PlayObject).TrainSkill(attackMagic, M2Share.RandomNumber.Random(3) + 1);
                                if (!(this as PlayObject).CheckMagicLevelup(attackMagic))
                                {
                                    SendDelayMsg(this, Grobal2.RM_MAGIC_LVEXP, 0, attackMagic.MagicInfo.wMagicID, attackMagic.btLevel, attackMagic.nTranPoint, "", 3000);
                                }
                            }
                        }
                        switch (wHitMode)
                        {
                            case 4:
                                attackMagic = GetAttrackMagic(SpellsDef.SKILL_ERGUM);
                                if (attackMagic != null)
                                {
                                    if (attackMagic.btLevel < 3 && (attackMagic.MagicInfo.TrainLevel[attackMagic.btLevel] <= Abil.Level))
                                    {
                                        (this as PlayObject).TrainSkill(attackMagic, 1);
                                        if (!(this as PlayObject).CheckMagicLevelup(attackMagic))
                                        {
                                            SendDelayMsg(this.ObjectId, Grobal2.RM_MAGIC_LVEXP, 0, attackMagic.MagicInfo.wMagicID, attackMagic.btLevel, attackMagic.nTranPoint, "", 3000);
                                        }
                                    }
                                }
                                break;
                            case 5:
                                attackMagic = GetAttrackMagic(SpellsDef.SKILL_BANWOL);
                                if (attackMagic != null)
                                {
                                    if ((attackMagic.btLevel < 3) && (attackMagic.MagicInfo.TrainLevel[attackMagic.btLevel] <= Abil.Level))
                                    {
                                        (this as PlayObject).TrainSkill(attackMagic, 1);
                                        if (!(this as PlayObject).CheckMagicLevelup(attackMagic))
                                        {
                                            SendDelayMsg(this.ObjectId, Grobal2.RM_MAGIC_LVEXP, 0, attackMagic.MagicInfo.wMagicID, attackMagic.btLevel, attackMagic.nTranPoint, "", 3000);
                                        }
                                    }
                                }
                                break;
                            case 12:
                                attackMagic = GetAttrackMagic(SpellsDef.SKILL_REDBANWOL);
                                if (attackMagic != null)
                                {
                                    if ((attackMagic.btLevel < 3) && (attackMagic.MagicInfo.TrainLevel[attackMagic.btLevel] <= Abil.Level))
                                    {
                                        (this as PlayObject).TrainSkill(attackMagic, 1);
                                        if (!(this as PlayObject).CheckMagicLevelup(attackMagic))
                                        {
                                            SendDelayMsg(this.ObjectId, Grobal2.RM_MAGIC_LVEXP, 0, attackMagic.MagicInfo.wMagicID, attackMagic.btLevel, attackMagic.nTranPoint, "", 3000);
                                        }
                                    }
                                }
                                break;
                            case 7:
                                attackMagic = GetAttrackMagic(SpellsDef.SKILL_FIRESWORD);
                                if (attackMagic != null)
                                {
                                    if ((attackMagic.btLevel < 3) && (attackMagic.MagicInfo.TrainLevel[attackMagic.btLevel] <= Abil.Level))
                                    {
                                        (this as PlayObject).TrainSkill(attackMagic, 1);
                                        if (!(this as PlayObject).CheckMagicLevelup(attackMagic))
                                        {
                                            SendDelayMsg(this, Grobal2.RM_MAGIC_LVEXP, 0, attackMagic.MagicInfo.wMagicID, attackMagic.btLevel, attackMagic.nTranPoint, "", 3000);
                                        }
                                    }
                                }
                                break;
                            case 8:
                                attackMagic = GetAttrackMagic(SpellsDef.SKILL_CROSSMOON);
                                if (attackMagic != null)
                                {
                                    if ((attackMagic.btLevel < 3) && (attackMagic.MagicInfo.TrainLevel[attackMagic.btLevel] <= Abil.Level))
                                    {
                                        (this as PlayObject).TrainSkill(attackMagic, 1);
                                        if (!(this as PlayObject).CheckMagicLevelup(attackMagic))
                                        {
                                            SendDelayMsg(this, Grobal2.RM_MAGIC_LVEXP, 0, attackMagic.MagicInfo.wMagicID, attackMagic.btLevel, attackMagic.nTranPoint, "", 3000);
                                        }
                                    }
                                }
                                break;
                            case 9:
                                attackMagic = GetAttrackMagic(SpellsDef.SKILL_TWINBLADE);
                                if (attackMagic != null)
                                {
                                    if ((attackMagic.btLevel < 3) && (attackMagic.MagicInfo.TrainLevel[attackMagic.btLevel] <= Abil.Level))
                                    {
                                        (this as PlayObject).TrainSkill(attackMagic, 1);
                                        if (!(this as PlayObject).CheckMagicLevelup(attackMagic))
                                        {
                                            SendDelayMsg(this, Grobal2.RM_MAGIC_LVEXP, 0, attackMagic.MagicInfo.wMagicID, attackMagic.btLevel, attackMagic.nTranPoint, "", 3000);
                                        }
                                    }
                                }
                                break;
                        }
                    }
                    result = true;
                    if (M2Share.Config.boMonDelHptoExp)
                    {
                        if (Race == Grobal2.RC_PLAYOBJECT)
                        {
                            if (this.IsRobot)
                            {
                                if ((this as RobotPlayObject).m_WAbil.Level <= M2Share.Config.MonHptoExpLevel)
                                {
                                    if (!M2Share.GetNoHptoexpMonList(AttackTarget.CharName))
                                    {
                                        (this as RobotPlayObject).GainExp(nPower * M2Share.Config.MonHptoExpmax);
                                    }
                                }
                            }
                            else
                            {
                                if ((this as PlayObject).m_WAbil.Level <= M2Share.Config.MonHptoExpLevel)
                                {
                                    if (!M2Share.GetNoHptoexpMonList(AttackTarget.CharName))
                                    {
                                        (this as PlayObject).GainExp(nPower * M2Share.Config.MonHptoExpmax);
                                    }
                                }
                            }
                        }
                        if (Race == Grobal2.RC_PLAYCLONE)
                        {
                            if (Master != null)
                            {
                                if (Master.IsRobot)
                                {
                                    if ((Master as RobotPlayObject).m_WAbil.Level <= M2Share.Config.MonHptoExpLevel)
                                    {
                                        if (!M2Share.GetNoHptoexpMonList(AttackTarget.CharName))
                                        {
                                            (Master as RobotPlayObject).GainExp(nPower * M2Share.Config.MonHptoExpmax);
                                        }
                                    }
                                }
                                else
                                {
                                    if ((Master as PlayObject).m_WAbil.Level <= M2Share.Config.MonHptoExpLevel)
                                    {
                                        if (!M2Share.GetNoHptoexpMonList(AttackTarget.CharName))
                                        {
                                            (Master as PlayObject).GainExp(nPower * M2Share.Config.MonHptoExpmax);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (Race == Grobal2.RC_PLAYOBJECT)
                {
                    TrainCurrentSkill(wHitMode);
                }
                if ((nWeaponDamage > 0) && (UseItems[Grobal2.U_WEAPON] != null) && (UseItems[Grobal2.U_WEAPON].wIndex > 0))
                {
                    DoDamageWeapon(nWeaponDamage);
                }
                if (AttackTarget.Race != Grobal2.RC_PLAYOBJECT)
                {
                    AttackTarget.SendMsg(AttackTarget, Grobal2.RM_STRUCK, (short)nPower, AttackTarget.m_WAbil.HP, AttackTarget.m_WAbil.MaxHP, ObjectId, "");
                }
            }
            catch (Exception e)
            {
                M2Share.LogSystem.Error(e.Message);
            }
            return result;
        }

        private void TrainCurrentSkill(int wHitMode)
        {
            int nCLevel = Abil.Level;
            if ((MagicArr[SpellsDef.SKILL_ONESWORD] != null) && ((Race == Grobal2.RC_PLAYOBJECT)))
            {
                if ((MagicArr[SpellsDef.SKILL_ONESWORD].btLevel < MagicArr[SpellsDef.SKILL_ONESWORD].MagicInfo.btTrainLv) && (MagicArr[SpellsDef.SKILL_ONESWORD].MagicInfo.TrainLevel[MagicArr[SpellsDef.SKILL_ONESWORD].btLevel] <= nCLevel))
                {
                    ((this) as PlayObject).TrainSkill(MagicArr[SpellsDef.SKILL_ONESWORD], M2Share.RandomNumber.Random(3) + 1);
                    if (!((this) as PlayObject).CheckMagicLevelup(MagicArr[SpellsDef.SKILL_ONESWORD]))
                    {
                        SendDelayMsg(this, Grobal2.RM_MAGIC_LVEXP, 0, MagicArr[SpellsDef.SKILL_ONESWORD].MagicInfo.wMagicID, MagicArr[SpellsDef.SKILL_ONESWORD].btLevel, MagicArr[SpellsDef.SKILL_ONESWORD].nTranPoint, "", 3000);
                    }
                }
            }
            if ((MagicArr[SpellsDef.SKILL_ILKWANG] != null) && ((Race == Grobal2.RC_PLAYOBJECT)))
            {
                if ((MagicArr[SpellsDef.SKILL_ILKWANG].btLevel < MagicArr[SpellsDef.SKILL_ILKWANG].MagicInfo.btTrainLv) && (MagicArr[SpellsDef.SKILL_ILKWANG].MagicInfo.TrainLevel[MagicArr[SpellsDef.SKILL_ILKWANG].btLevel] <= nCLevel))
                {
                    ((this) as PlayObject).TrainSkill(MagicArr[SpellsDef.SKILL_ILKWANG], M2Share.RandomNumber.Random(3) + 1);
                    if (!((this) as PlayObject).CheckMagicLevelup(MagicArr[SpellsDef.SKILL_ILKWANG]))
                    {
                        SendDelayMsg(this, Grobal2.RM_MAGIC_LVEXP, 0, MagicArr[SpellsDef.SKILL_ILKWANG].MagicInfo.wMagicID, MagicArr[SpellsDef.SKILL_ILKWANG].btLevel, MagicArr[SpellsDef.SKILL_ILKWANG].nTranPoint, "", 3000);
                    }
                }
            }
            if ((wHitMode == 3) && (MagicArr[SpellsDef.SKILL_YEDO] != null) && (Race == Grobal2.RC_PLAYOBJECT))
            {
                if ((MagicArr[SpellsDef.SKILL_YEDO].btLevel < MagicArr[SpellsDef.SKILL_YEDO].MagicInfo.btTrainLv) && (MagicArr[SpellsDef.SKILL_YEDO].MagicInfo.TrainLevel[MagicArr[SpellsDef.SKILL_YEDO].btLevel] <= nCLevel))
                {
                    ((this) as PlayObject).TrainSkill(MagicArr[SpellsDef.SKILL_YEDO], M2Share.RandomNumber.Random(3) + 1);
                    if (!((this) as PlayObject).CheckMagicLevelup(MagicArr[SpellsDef.SKILL_YEDO]))
                    {
                        SendDelayMsg(this, Grobal2.RM_MAGIC_LVEXP, 0, MagicArr[SpellsDef.SKILL_YEDO].MagicInfo.wMagicID, MagicArr[SpellsDef.SKILL_YEDO].btLevel, MagicArr[SpellsDef.SKILL_YEDO].nTranPoint, "", 3000);
                    }
                }
            }
            if ((wHitMode == 4) && (MagicArr[SpellsDef.SKILL_ERGUM] != null) && ((Race == Grobal2.RC_PLAYOBJECT)))
            {
                if ((MagicArr[SpellsDef.SKILL_ERGUM].btLevel < MagicArr[SpellsDef.SKILL_ERGUM].MagicInfo.btTrainLv) && (MagicArr[SpellsDef.SKILL_ERGUM].MagicInfo.TrainLevel[MagicArr[SpellsDef.SKILL_ERGUM].btLevel] <= nCLevel))
                {
                    ((this) as PlayObject).TrainSkill(MagicArr[SpellsDef.SKILL_ERGUM], 1);
                    if (!((this) as PlayObject).CheckMagicLevelup(MagicArr[SpellsDef.SKILL_ERGUM]))
                    {
                        SendDelayMsg(this, Grobal2.RM_MAGIC_LVEXP, 0, MagicArr[SpellsDef.SKILL_ERGUM].MagicInfo.wMagicID, MagicArr[SpellsDef.SKILL_ERGUM].btLevel, MagicArr[SpellsDef.SKILL_ERGUM].nTranPoint, "", 3000);
                    }
                }
            }
            if ((wHitMode == 5) && (MagicArr[SpellsDef.SKILL_BANWOL] != null) && ((Race == Grobal2.RC_PLAYOBJECT)))
            {
                if ((MagicArr[SpellsDef.SKILL_BANWOL].btLevel < MagicArr[SpellsDef.SKILL_BANWOL].MagicInfo.btTrainLv) && (MagicArr[SpellsDef.SKILL_BANWOL].MagicInfo.TrainLevel[MagicArr[SpellsDef.SKILL_BANWOL].btLevel] <= nCLevel))
                {
                    ((this) as PlayObject).TrainSkill(MagicArr[SpellsDef.SKILL_BANWOL], 1);
                    if (!((this) as PlayObject).CheckMagicLevelup(MagicArr[SpellsDef.SKILL_BANWOL]))
                    {
                        SendDelayMsg(this, Grobal2.RM_MAGIC_LVEXP, 0, MagicArr[SpellsDef.SKILL_BANWOL].MagicInfo.wMagicID, MagicArr[SpellsDef.SKILL_BANWOL].btLevel, MagicArr[SpellsDef.SKILL_BANWOL].nTranPoint, "", 3000);
                    }
                }
            }
            if ((wHitMode == 7) && (MagicArr[SpellsDef.SKILL_FIRESWORD] != null) && ((Race == Grobal2.RC_PLAYOBJECT)))
            {
                if ((MagicArr[SpellsDef.SKILL_FIRESWORD].btLevel < MagicArr[SpellsDef.SKILL_FIRESWORD].MagicInfo.btTrainLv) && (MagicArr[SpellsDef.SKILL_FIRESWORD].MagicInfo.TrainLevel[MagicArr[SpellsDef.SKILL_FIRESWORD].btLevel] <= nCLevel))
                {
                    ((this) as PlayObject).TrainSkill(MagicArr[SpellsDef.SKILL_FIRESWORD], 1);
                    if (!((this) as PlayObject).CheckMagicLevelup(MagicArr[SpellsDef.SKILL_FIRESWORD]))
                    {
                        SendDelayMsg(this, Grobal2.RM_MAGIC_LVEXP, 0, MagicArr[SpellsDef.SKILL_FIRESWORD].MagicInfo.wMagicID, MagicArr[SpellsDef.SKILL_FIRESWORD].btLevel, MagicArr[SpellsDef.SKILL_FIRESWORD].nTranPoint, "", 3000);
                    }
                }
            }
            if ((wHitMode == 9) && (MagicArr[43] != null) && ((Race == Grobal2.RC_PLAYOBJECT)))
            {
                if ((MagicArr[43].btLevel < MagicArr[43].MagicInfo.btTrainLv) && (MagicArr[43].MagicInfo.TrainLevel[MagicArr[43].btLevel] <= nCLevel))
                {
                    ((this) as PlayObject).TrainSkill(MagicArr[43], 1);
                    if (!((this) as PlayObject).CheckMagicLevelup(MagicArr[43]))
                    {
                        SendDelayMsg(this, Grobal2.RM_MAGIC_LVEXP, 0, MagicArr[43].MagicInfo.wMagicID, MagicArr[43].btLevel, MagicArr[43].nTranPoint, "", 3000);
                    }
                }
            }
            if ((wHitMode == 13) && (MagicArr[56] != null) && ((Race == Grobal2.RC_PLAYOBJECT)))
            {
                if ((MagicArr[56].btLevel < MagicArr[56].MagicInfo.btTrainLv) && (MagicArr[56].MagicInfo.TrainLevel[MagicArr[56].btLevel] <= nCLevel))
                {
                    ((this) as PlayObject).TrainSkill(MagicArr[56], 1);
                    if (!((this) as PlayObject).CheckMagicLevelup(MagicArr[56]))
                    {
                        SendDelayMsg(this, Grobal2.RM_MAGIC_LVEXP, 0, MagicArr[56].MagicInfo.wMagicID, MagicArr[56].btLevel, MagicArr[56].nTranPoint, "", 3000);
                    }
                }
            }
            if ((wHitMode == 8) && (MagicArr[40] != null) && ((Race == Grobal2.RC_PLAYOBJECT)))
            {
                if ((MagicArr[40].btLevel < MagicArr[40].MagicInfo.btTrainLv) && (MagicArr[40].MagicInfo.TrainLevel[MagicArr[40].btLevel] <= nCLevel))
                {
                    ((this) as PlayObject).TrainSkill(MagicArr[40], 1);
                    if (!((this) as PlayObject).CheckMagicLevelup(MagicArr[40]))
                    {
                        SendDelayMsg(this, Grobal2.RM_MAGIC_LVEXP, 0, MagicArr[40].MagicInfo.wMagicID, MagicArr[40].btLevel, MagicArr[40].nTranPoint, "", 3000);
                    }
                }
            }
            if ((wHitMode == 10) && (MagicArr[42] != null) && ((Race == Grobal2.RC_PLAYOBJECT)))
            {
                if ((MagicArr[42].btLevel < MagicArr[42].MagicInfo.btTrainLv) && (MagicArr[42].MagicInfo.TrainLevel[MagicArr[42].btLevel] <= nCLevel))
                {
                    ((this) as PlayObject).TrainSkill(MagicArr[42], 1);
                    if (!((this) as PlayObject).CheckMagicLevelup(MagicArr[42]))
                    {
                        SendDelayMsg(this, Grobal2.RM_MAGIC_LVEXP, 0, MagicArr[42].MagicInfo.wMagicID, MagicArr[42].btLevel, MagicArr[42].nTranPoint, "", 3000);
                    }
                }
            }
            if ((wHitMode == 12) && (MagicArr[66] != null) && ((Race == Grobal2.RC_PLAYOBJECT)))
            {
                if ((MagicArr[66].btLevel < MagicArr[66].MagicInfo.btTrainLv) && (MagicArr[66].MagicInfo.TrainLevel[MagicArr[66].btLevel] <= nCLevel))
                {
                    ((this) as PlayObject).TrainSkill(MagicArr[66], 1);
                    if (!((this) as PlayObject).CheckMagicLevelup(MagicArr[66]))
                    {
                        SendDelayMsg(this, Grobal2.RM_MAGIC_LVEXP, 0, MagicArr[66].MagicInfo.wMagicID, MagicArr[66].btLevel, MagicArr[66].nTranPoint, "", 3000);
                    }
                }
            }
            if ((wHitMode == 61) && (MagicArr[61] != null) && ((Race == Grobal2.RC_PLAYOBJECT)))
            {
                if ((MagicArr[61].btLevel < MagicArr[61].MagicInfo.btTrainLv) && (MagicArr[61].MagicInfo.TrainLevel[MagicArr[61].btLevel] <= nCLevel))
                {
                    ((this) as PlayObject).TrainSkill(MagicArr[61], 1);
                    if (!((this) as PlayObject).CheckMagicLevelup(MagicArr[61]))
                    {
                        SendDelayMsg(this, Grobal2.RM_MAGIC_LVEXP, 0, MagicArr[61].MagicInfo.wMagicID, MagicArr[61].btLevel, MagicArr[61].nTranPoint, "", 3000);
                    }
                }
            }
            if ((wHitMode == 20) && (MagicArr[101] != null) && ((Race == Grobal2.RC_PLAYOBJECT)))
            {
                if ((MagicArr[101].MagicInfo.TrainLevel[MagicArr[101].btLevel] <= nCLevel))
                {
                    ((this) as PlayObject).TrainSkill(MagicArr[101], 1);
                    if (!((this) as PlayObject).CheckMagicLevelup(MagicArr[101]))
                    {
                        SendDelayMsg(this, Grobal2.RM_MAGIC_LVEXP, 0, MagicArr[101].MagicInfo.wMagicID, MagicArr[101].btLevel, MagicArr[101].nTranPoint, "", 3000);
                    }
                }
            }
            if ((wHitMode == 21) && (MagicArr[102] != null) && ((Race == Grobal2.RC_PLAYOBJECT)))
            {
                if ((MagicArr[102].MagicInfo.TrainLevel[MagicArr[102].btLevel] <= nCLevel))
                {
                    ((this) as PlayObject).TrainSkill(MagicArr[102], 1);
                    if (!((this) as PlayObject).CheckMagicLevelup(MagicArr[102]))
                    {
                        SendDelayMsg(this, Grobal2.RM_MAGIC_LVEXP, 0, MagicArr[102].MagicInfo.wMagicID, MagicArr[102].btLevel, MagicArr[102].nTranPoint, "", 3000);
                    }
                }
            }
            if ((wHitMode == 22) && (MagicArr[103] != null) && ((Race == Grobal2.RC_PLAYOBJECT)))
            {
                if ((MagicArr[103].MagicInfo.TrainLevel[MagicArr[103].btLevel] <= nCLevel))
                {
                    ((this) as PlayObject).TrainSkill(MagicArr[103], 1);
                    if (!((this) as PlayObject).CheckMagicLevelup(MagicArr[103]))
                    {
                        SendDelayMsg(this, Grobal2.RM_MAGIC_LVEXP, 0, MagicArr[103].MagicInfo.wMagicID, MagicArr[103].btLevel, MagicArr[103].nTranPoint, "", 3000);
                    }
                }
            }
            if ((wHitMode == 23) && (MagicArr[114] != null) && ((Race == Grobal2.RC_PLAYOBJECT)))
            {
                if ((MagicArr[114].MagicInfo.TrainLevel[MagicArr[114].btLevel] <= nCLevel))
                {
                    ((this) as PlayObject).TrainSkill(MagicArr[114], 1);
                    if (!((this) as PlayObject).CheckMagicLevelup(MagicArr[114]))
                    {
                        SendDelayMsg(this, Grobal2.RM_MAGIC_LVEXP, 0, MagicArr[114].MagicInfo.wMagicID, MagicArr[114].btLevel, MagicArr[114].nTranPoint, "", 3000);
                    }
                }
            }
            if ((wHitMode == 24) && (MagicArr[113] != null) && ((Race == Grobal2.RC_PLAYOBJECT)))
            {
                if ((MagicArr[113].MagicInfo.TrainLevel[MagicArr[113].btLevel] <= nCLevel))
                {
                    ((this) as PlayObject).TrainSkill(MagicArr[113], 1);
                    if (!((this) as PlayObject).CheckMagicLevelup(MagicArr[113]))
                    {
                        SendDelayMsg(this, Grobal2.RM_MAGIC_LVEXP, 0, MagicArr[113].MagicInfo.wMagicID, MagicArr[113].btLevel, MagicArr[113].nTranPoint, "", 3000);
                    }
                }
            }
            if ((wHitMode == 25) && (MagicArr[115] != null) && ((Race == Grobal2.RC_PLAYOBJECT)))
            {
                if ((MagicArr[115].MagicInfo.TrainLevel[MagicArr[115].btLevel] <= nCLevel))
                {
                    ((this) as PlayObject).TrainSkill(MagicArr[115], 1);
                    if (!((this) as PlayObject).CheckMagicLevelup(MagicArr[115]))
                    {
                        SendDelayMsg(this, Grobal2.RM_MAGIC_LVEXP, 0, MagicArr[115].MagicInfo.wMagicID, MagicArr[115].btLevel, MagicArr[115].nTranPoint, "", 3000);
                    }
                }
            }
        }

        private TUserMagic GetAttrackMagic(int magicId)
        {
            return MagicArr[magicId];
        }
    }
}
