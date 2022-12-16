using GameSvr.Actor;
using GameSvr.Items;
using GameSvr.Magic;
using GameSvr.RobotPlay;
using SystemModule;
using SystemModule.Consts;
using SystemModule.Data;
using SystemModule.Enums;
using SystemModule.Packets.ClientPackets;

namespace GameSvr.Player
{
    public partial class PlayObject
    {

        protected void AttackDir(BaseObject targetObject, short wHitMode, byte nDir)
        {
            var attackTarget = targetObject ?? GetPoseCreate();
            if (UseItems[Grobal2.U_WEAPON] != null && (UseItems[Grobal2.U_WEAPON].Index > 0) && UseItems[Grobal2.U_WEAPON].Desc[ItemAttr.WeaponUpgrade] > 0)
            {
                if (attackTarget != null)
                {
                    CheckWeaponUpgrade();
                }
            }

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

            var nBasePower = GetBaseAttackPoewr();//基础攻击力
            var canHit = false;
            var nPower = GetAttackPowerHit(wHitMode, nBasePower, attackTarget, ref canHit);
            SkillAttackDamage(wHitMode, nPower);
            AttackDir(attackTarget, nPower, nDir);
            SendAttackMsg(GetHitMode(wHitMode), Direction, CurrX, CurrY);
            AttackSuccess(wHitMode, nPower, canHit, attackTarget);
        }

        private int GetHitMode(short wHitMode)
        {
            var wIdent = Grobal2.RM_HIT;
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
                    if (PowerHit)
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
                    if (FireHitSkill)
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
                    if (TwinHitSkill)
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
            return wIdent;
        }

        private void SkillAttackDamage(short wHitMode, ushort nPower)
        {
            var nSecPwr = 0;
            if (wHitMode > 0)
            {
                switch (wHitMode)
                {
                    case 4:// 刺杀
                        if (MagicArr[MagicConst.SKILL_ERGUM] != null)
                        {
                            nSecPwr = HUtil32.Round(nPower / (MagicArr[MagicConst.SKILL_ERGUM].Magic.TrainLv + 2) * (MagicArr[MagicConst.SKILL_ERGUM].Level + 2));
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
                            if (MagicArr[MagicConst.SKILL_BANWOL] != null)
                            {
                                nSecPwr = HUtil32.Round(nPower / (MagicArr[MagicConst.SKILL_BANWOL].Magic.TrainLv + 10) * (MagicArr[MagicConst.SKILL_BANWOL].Level + 2));
                            }
                            if (nSecPwr > 0)
                            {
                                SwordWideAttack(ref nSecPwr);
                            }
                            break;
                        }
                    case 12:
                        {
                            if (MagicArr[MagicConst.SKILL_REDBANWOL] != null)
                            {
                                nSecPwr = HUtil32.Round(nPower / (MagicArr[MagicConst.SKILL_REDBANWOL].Magic.TrainLv + 10) * (MagicArr[MagicConst.SKILL_REDBANWOL].Level + 2));
                            }
                            if (nSecPwr > 0)
                            {
                                SwordWideAttack(ref nSecPwr);
                            }
                            break;
                        }
                    case 8:
                        {
                            if (MagicArr[MagicConst.SKILL_CROSSMOON] != null)
                            {
                                nSecPwr = HUtil32.Round(nPower / (MagicArr[MagicConst.SKILL_CROSSMOON].Magic.TrainLv + 10) * (MagicArr[MagicConst.SKILL_CROSSMOON].Level + 2));
                            }
                            if (nSecPwr > 0)
                            {
                                CrsWideAttack(nSecPwr);
                            }
                            break;
                        }
                }
            }
        }

        private void CheckSkillProficiency(int nTranPoint, UserMagic attackMagic)
        {
            if (attackMagic != null)
            {
                if ((attackMagic.Level < 3) && (attackMagic.Magic.TrainLevel[attackMagic.Level] <= Abil.Level))
                {
                    TrainSkill(attackMagic, nTranPoint);
                    if (!CheckMagicLevelup(attackMagic))
                    {
                        SendDelayMsg(this, Grobal2.RM_MAGIC_LVEXP, 0, attackMagic.Magic.MagicId, attackMagic.Level, attackMagic.TranPoint, "", 3000);
                    }
                }
            }
        }

        private void AttackSuccess(short wHitMode, ushort nPower, bool canHit, BaseObject AttackTarget)
        {
            if (AttackTarget.Race == ActorRace.Play && Race == ActorRace.Play)
            {
                if (!((PlayObject)AttackTarget).UnParalysis && Paralysis && (M2Share.RandomNumber.Random(AttackTarget.AntiPoison + M2Share.Config.AttackPosionRate) == 0))
                {
                    AttackTarget.MakePosion(PoisonState.STONE, M2Share.Config.AttackPosionTime, 0);
                }
            }
            var nWeaponDamage = (ushort)(M2Share.RandomNumber.Random(5) + 2 - AddAbil.WeaponStrong);
            if ((nWeaponDamage > 0) && (UseItems[Grobal2.U_WEAPON] != null) && (UseItems[Grobal2.U_WEAPON].Index > 0))
            {
                DoDamageWeapon(nWeaponDamage);
            }
            if (SuckupEnemyHealthRate > 0)// 虹魔，吸血
            {
                SuckupEnemyHealth = nPower / 100 * SuckupEnemyHealthRate;
                if (SuckupEnemyHealth >= 2.0)
                {
                    var n20 = Convert.ToUInt16(SuckupEnemyHealth);
                    SuckupEnemyHealth = n20;
                    DamageHealth((ushort)-n20);
                }
            }
            UserMagic attackMagic;
            if (MagicArr[MagicConst.SKILL_ILKWANG] != null)
            {
                attackMagic = GetAttrackMagic(MagicConst.SKILL_ILKWANG);
                if (attackMagic.Level < 3)
                {
                    CheckSkillProficiency(M2Share.RandomNumber.Random(3) + 1, attackMagic);
                }
            }
            if (canHit && (MagicArr[MagicConst.SKILL_YEDO] != null))
            {
                attackMagic = GetAttrackMagic(MagicConst.SKILL_YEDO);
                if (attackMagic.Level < 3)
                {
                    CheckSkillProficiency(M2Share.RandomNumber.Random(3) + 1, attackMagic);
                }
            }
            switch (wHitMode)
            {
                case 4:
                    attackMagic = GetAttrackMagic(MagicConst.SKILL_ERGUM);
                    CheckSkillProficiency(1, attackMagic);
                    break;
                case 5:
                    attackMagic = GetAttrackMagic(MagicConst.SKILL_BANWOL);
                    CheckSkillProficiency(1, attackMagic);
                    break;
                case 12:
                    attackMagic = GetAttrackMagic(MagicConst.SKILL_REDBANWOL);
                    CheckSkillProficiency(1, attackMagic);
                    break;
                case 7:
                    attackMagic = GetAttrackMagic(MagicConst.SKILL_FIRESWORD);
                    CheckSkillProficiency(1, attackMagic);
                    break;
                case 8:
                    attackMagic = GetAttrackMagic(MagicConst.SKILL_CROSSMOON);
                    CheckSkillProficiency(1, attackMagic);
                    break;
                case 9:
                    attackMagic = GetAttrackMagic(MagicConst.SKILL_TWINBLADE);
                    CheckSkillProficiency(1, attackMagic);
                    break;
            }
            if (M2Share.Config.MonDelHptoExp)
            {
                switch (Race)
                {
                    case ActorRace.Play:
                        if (IsRobot)
                        {
                            if (((RobotPlayObject)this).Abil.Level <= M2Share.Config.MonHptoExpLevel)
                            {
                                if (!M2Share.GetNoHptoexpMonList(AttackTarget.ChrName))
                                {
                                    ((RobotPlayObject)this).GainExp(nPower * M2Share.Config.MonHptoExpmax);
                                }
                            }
                        }
                        else
                        {
                            if (Abil.Level <= M2Share.Config.MonHptoExpLevel)
                            {
                                if (!M2Share.GetNoHptoexpMonList(AttackTarget.ChrName))
                                {
                                    GainExp(nPower * M2Share.Config.MonHptoExpmax);
                                }
                            }
                        }
                        break;
                    case ActorRace.PlayClone:
                        if (Master != null)
                        {
                            if (Master.IsRobot)
                            {
                                if (((RobotPlayObject)Master).Abil.Level <= M2Share.Config.MonHptoExpLevel)
                                {
                                    if (!M2Share.GetNoHptoexpMonList(AttackTarget.ChrName))
                                    {
                                        ((RobotPlayObject)Master).GainExp(nPower * M2Share.Config.MonHptoExpmax);
                                    }
                                }
                            }
                            else
                            {
                                if (((PlayObject)Master).Abil.Level <= M2Share.Config.MonHptoExpLevel)
                                {
                                    if (!M2Share.GetNoHptoexpMonList(AttackTarget.ChrName))
                                    {
                                        ((PlayObject)Master).GainExp(nPower * M2Share.Config.MonHptoExpmax);
                                    }
                                }
                            }
                        }
                        break;
                }
            }
            TrainCurrentSkill(wHitMode);
        }

        public bool IsTrainingSkill(int nIndex)
        {
            for (var i = 0; i < MagicList.Count; i++)
            {
                var userMagic = MagicList[i];
                if ((userMagic != null) && (userMagic.MagIdx == nIndex))
                {
                    return true;
                }
            }
            return false;
        }

        private void TrainCurrentSkill(int wHitMode)
        {
            if (Race != ActorRace.Play)
            {
                return;
            }
            int nCLevel = Abil.Level;
            if ((MagicArr[MagicConst.SKILL_ONESWORD] != null))
            {
                if ((MagicArr[MagicConst.SKILL_ONESWORD].Level < MagicArr[MagicConst.SKILL_ONESWORD].Magic.TrainLv) && (MagicArr[MagicConst.SKILL_ONESWORD].Magic.TrainLevel[MagicArr[MagicConst.SKILL_ONESWORD].Level] <= nCLevel))
                {
                    TrainSkill(MagicArr[MagicConst.SKILL_ONESWORD], M2Share.RandomNumber.Random(3) + 1);
                    if (!CheckMagicLevelup(MagicArr[MagicConst.SKILL_ONESWORD]))
                    {
                        SendDelayMsg(this, Grobal2.RM_MAGIC_LVEXP, 0, MagicArr[MagicConst.SKILL_ONESWORD].Magic.MagicId, MagicArr[MagicConst.SKILL_ONESWORD].Level, MagicArr[MagicConst.SKILL_ONESWORD].TranPoint, "", 3000);
                    }
                }
            }
            if ((MagicArr[MagicConst.SKILL_ILKWANG] != null))
            {
                if ((MagicArr[MagicConst.SKILL_ILKWANG].Level < MagicArr[MagicConst.SKILL_ILKWANG].Magic.TrainLv) && (MagicArr[MagicConst.SKILL_ILKWANG].Magic.TrainLevel[MagicArr[MagicConst.SKILL_ILKWANG].Level] <= nCLevel))
                {
                    TrainSkill(MagicArr[MagicConst.SKILL_ILKWANG], M2Share.RandomNumber.Random(3) + 1);
                    if (!CheckMagicLevelup(MagicArr[MagicConst.SKILL_ILKWANG]))
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
                            TrainSkill(MagicArr[MagicConst.SKILL_YEDO], M2Share.RandomNumber.Random(3) + 1);
                            if (!CheckMagicLevelup(MagicArr[MagicConst.SKILL_YEDO]))
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
                            TrainSkill(MagicArr[MagicConst.SKILL_ERGUM], 1);
                            if (!CheckMagicLevelup(MagicArr[MagicConst.SKILL_ERGUM]))
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
                            TrainSkill(MagicArr[MagicConst.SKILL_BANWOL], 1);
                            if (!CheckMagicLevelup(MagicArr[MagicConst.SKILL_BANWOL]))
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
                            TrainSkill(MagicArr[MagicConst.SKILL_FIRESWORD], 1);
                            if (!CheckMagicLevelup(MagicArr[MagicConst.SKILL_FIRESWORD]))
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
                            TrainSkill(MagicArr[43], 1);
                            if (!CheckMagicLevelup(MagicArr[43]))
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
                            TrainSkill(MagicArr[56], 1);
                            if (!CheckMagicLevelup(MagicArr[56]))
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
                            TrainSkill(MagicArr[40], 1);
                            if (!CheckMagicLevelup(MagicArr[40]))
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
                            TrainSkill(MagicArr[42], 1);
                            if (!CheckMagicLevelup(MagicArr[42]))
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
                            TrainSkill(MagicArr[66], 1);
                            if (!CheckMagicLevelup(MagicArr[66]))
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
                            TrainSkill(MagicArr[61], 1);
                            if (!CheckMagicLevelup(MagicArr[61]))
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
                            TrainSkill(MagicArr[101], 1);
                            if (!CheckMagicLevelup(MagicArr[101]))
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
                            TrainSkill(MagicArr[102], 1);
                            if (!CheckMagicLevelup(MagicArr[102]))
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
                            TrainSkill(MagicArr[103], 1);
                            if (!CheckMagicLevelup(MagicArr[103]))
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
                            TrainSkill(MagicArr[114], 1);
                            if (!CheckMagicLevelup(MagicArr[114]))
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
                            TrainSkill(MagicArr[113], 1);
                            if (!CheckMagicLevelup(MagicArr[113]))
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
                            TrainSkill(MagicArr[115], 1);
                            if (!CheckMagicLevelup(MagicArr[115]))
                            {
                                SendDelayMsg(this, Grobal2.RM_MAGIC_LVEXP, 0, MagicArr[115].Magic.MagicId, MagicArr[115].Level, MagicArr[115].TranPoint, "", 3000);
                            }
                        }
                        break;
                    }
            }
        }

        protected override bool IsAttackTarget(BaseObject baseObject)
        {
            var result = base.IsAttackTarget(baseObject);
            if (result)
            {
                return true;
            }
            switch (AttatckMode)
            {
                case AttackMode.HAM_ALL:
                    if ((baseObject.Race < ActorRace.NPC) || (baseObject.Race > ActorRace.PeaceNpc))
                    {
                        result = true;
                    }
                    if (M2Share.Config.PveServer)
                    {
                        result = true;
                    }
                    break;
                case AttackMode.HAM_PEACE:
                    if (baseObject.Race >= ActorRace.Animal)
                    {
                        result = true;
                    }
                    break;
                case AttackMode.HAM_DEAR:
                    if (baseObject != MDearHuman)
                    {
                        result = true;
                    }
                    break;
                case AttackMode.HAM_MASTER:
                    if (baseObject.Race == ActorRace.Play)
                    {
                        result = true;
                        if (MBoMaster)
                        {
                            for (var i = 0; i < MMasterList.Count; i++)
                            {
                                if (MMasterList[i] == baseObject)
                                {
                                    result = false;
                                    break;
                                }
                            }
                        }
                        if (((PlayObject)baseObject).MBoMaster)
                        {
                            for (var i = 0; i < ((PlayObject)baseObject).MMasterList.Count; i++)
                            {
                                if (((PlayObject)baseObject).MMasterList[i] == this)
                                {
                                    result = false;
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        result = true;
                    }
                    break;
                case AttackMode.HAM_GROUP:
                    if ((baseObject.Race < ActorRace.NPC) || (baseObject.Race > ActorRace.PeaceNpc))
                    {
                        result = true;
                    }
                    if (baseObject.Race == ActorRace.Play)
                    {
                        if (IsGroupMember(baseObject))
                        {
                            result = false;
                        }
                    }
                    if (M2Share.Config.PveServer)
                    {
                        result = true;
                    }
                    break;
                case AttackMode.HAM_GUILD:
                    if ((baseObject.Race < ActorRace.NPC) || (baseObject.Race > ActorRace.PeaceNpc))
                    {
                        result = true;
                    }
                    if (baseObject.Race == ActorRace.Play)
                    {
                        if (MyGuild != null)
                        {
                            if (MyGuild.IsMember(baseObject.ChrName))
                            {
                                result = false;
                            }
                            if (GuildWarArea && (((PlayObject)baseObject).MyGuild != null))
                            {
                                if (MyGuild.IsAllyGuild(((PlayObject)baseObject).MyGuild))
                                {
                                    result = false;
                                }
                            }
                        }
                    }
                    if (M2Share.Config.PveServer)
                    {
                        result = true;
                    }
                    break;
                case AttackMode.HAM_PKATTACK:
                    if ((baseObject.Race < ActorRace.NPC) || (baseObject.Race > ActorRace.PeaceNpc))
                    {
                        result = true;
                    }
                    if (baseObject.Race == ActorRace.Play)
                    {
                        if (PvpLevel() >= 2)
                        {
                            result = ((PlayObject)baseObject).PvpLevel() < 2;
                        }
                        else
                        {
                            result = ((PlayObject)baseObject).PvpLevel() >= 2;
                        }
                    }
                    if (M2Share.Config.PveServer)
                    {
                        result = true;
                    }
                    break;
            }
            return result;
        }

        public override bool IsProperFriend(BaseObject attackTarget)
        {
            if (attackTarget.Race == ActorRace.Play)
            {
                var result = IsProperIsFriend(attackTarget);
                if (attackTarget.Race < ActorRace.Animal)
                {
                    return result;
                }
                if (attackTarget.Master == this)
                {
                    return true;
                }
                if (attackTarget.Master != null)
                {
                    return IsProperIsFriend(attackTarget.Master);
                }
                if (attackTarget.Race > ActorRace.Play) return result;
                var targetObject = (PlayObject)attackTarget;
                if (!targetObject.InGuildWarArea)
                {
                    if (M2Share.Config.boPKLevelProtect)// 新人保护
                    {
                        if (Abil.Level > M2Share.Config.nPKProtectLevel)// 如果大于指定等级
                        {
                            if (!targetObject.PvpFlag && targetObject.WAbil.Level <= M2Share.Config.nPKProtectLevel && targetObject.PvpLevel() < 2)// 被攻击的人物小指定等级没有红名，则不可以攻击。
                            {
                                return false;
                            }
                        }
                        if (Abil.Level <= M2Share.Config.nPKProtectLevel)// 如果小于指定等级
                        {
                            if (!targetObject.PvpFlag && targetObject.WAbil.Level > M2Share.Config.nPKProtectLevel && targetObject.PvpLevel() < 2)
                            {
                                return false;
                            }
                        }
                    }
                    // 大于指定级别的红名人物不可以杀指定级别未红名的人物。
                    if (PvpLevel() >= 2 && Abil.Level > M2Share.Config.nRedPKProtectLevel)
                    {
                        if (targetObject.Abil.Level <= M2Share.Config.nRedPKProtectLevel && targetObject.PvpLevel() < 2)
                        {
                            return false;
                        }
                    }
                    // 小于指定级别的非红名人物不可以杀指定级别红名人物。
                    if (Abil.Level <= M2Share.Config.nRedPKProtectLevel && PvpLevel() < 2)
                    {
                        if (targetObject.PvpLevel() >= 2 && targetObject.Abil.Level > M2Share.Config.nRedPKProtectLevel)
                        {
                            return false;
                        }
                    }
                    if (((HUtil32.GetTickCount() - MapMoveTick) < 3000) || ((HUtil32.GetTickCount() - targetObject.MapMoveTick) < 3000))
                    {
                        result = false;
                    }
                }
                return result;
            }
            return base.IsProperFriend(attackTarget);
        }

        private bool ClientHitXY(int wIdent, int nX, int nY, byte nDir, bool boLateDelivery, ref int dwDelayTime)
        {
            var result = false;
            short n14 = 0;
            short n18 = 0;
            const string sExceptionMsg = "[Exception] TPlayObject::ClientHitXY";
            dwDelayTime = 0;
            try
            {
                if (!BoCanHit)
                {
                    return false;
                }
                if (Death || StatusArr[PoisonState.STONE] != 0)// 防麻
                {
                    return false;
                }
                if (!M2Share.Config.CloseSpeedHackCheck)
                {
                    if (!boLateDelivery)
                    {
                        if (!CheckActionStatus(wIdent, ref dwDelayTime))
                        {
                            MBoFilterAction = false;
                            return false;
                        }
                        MBoFilterAction = true;
                        int dwAttackTime = HUtil32._MAX(0, M2Share.Config.HitIntervalTime - HitSpeed * M2Share.Config.ItemSpeed);
                        int dwCheckTime = HUtil32.GetTickCount() - MDwAttackTick;
                        if (dwCheckTime < dwAttackTime)
                        {
                            MDwAttackCount++;
                            dwDelayTime = dwAttackTime - dwCheckTime;
                            if (dwDelayTime > M2Share.Config.DropOverSpeed)
                            {
                                if (MDwAttackCount >= 4)
                                {
                                    MDwAttackTick = HUtil32.GetTickCount();
                                    MDwAttackCount = 0;
                                    dwDelayTime = M2Share.Config.DropOverSpeed;
                                    if (TestSpeedMode)
                                    {
                                        SysMsg($"攻击忙!!!{dwDelayTime}", MsgColor.Red, MsgType.Hint);
                                    }
                                }
                                else
                                {
                                    MDwAttackCount = 0;
                                }
                                return false;
                            }
                            if (TestSpeedMode)
                            {
                                SysMsg($"攻击步忙!!!{dwDelayTime}", MsgColor.Red, MsgType.Hint);
                            }
                            return false;
                        }

                    }
                }
                if (nX == CurrX && nY == CurrY)
                {
                    result = true;
                    MDwAttackTick = HUtil32.GetTickCount();
                    if (wIdent == Grobal2.CM_HEAVYHIT && UseItems[Grobal2.U_WEAPON] != null && UseItems[Grobal2.U_WEAPON].Dura > 0)// 挖矿
                    {
                        if (GetFrontPosition(ref n14, ref n18) && !Envir.CanWalk(n14, n18, false))
                        {
                            var StdItem = M2Share.WorldEngine.GetStdItem(UseItems[Grobal2.U_WEAPON].Index);
                            if (StdItem != null && StdItem.Shape == 19)
                            {
                                if (PileStones(n14, n18))
                                {
                                    SendSocket("=DIG");
                                }
                                HealthTick -= 30;
                                SpellTick -= 50;
                                SpellTick = HUtil32._MAX(0, SpellTick);
                                PerHealth -= 2;
                                PerSpell -= 2;
                                return result;
                            }
                        }
                    }
                    switch (wIdent)
                    {
                        case Grobal2.CM_HIT:
                            AttackDir(null, 0, nDir);
                            break;
                        case Grobal2.CM_HEAVYHIT:
                            AttackDir(null, 1, nDir);
                            break;
                        case Grobal2.CM_BIGHIT:
                            AttackDir(null, 2, nDir);
                            break;
                        case Grobal2.CM_POWERHIT:
                            AttackDir(null, 3, nDir);
                            break;
                        case Grobal2.CM_LONGHIT:
                            AttackDir(null, 4, nDir);
                            break;
                        case Grobal2.CM_WIDEHIT:
                            AttackDir(null, 5, nDir);
                            break;
                        case Grobal2.CM_FIREHIT:
                            AttackDir(null, 7, nDir);
                            break;
                        case Grobal2.CM_CRSHIT:
                            AttackDir(null, 8, nDir);
                            break;
                        case Grobal2.CM_TWINHIT:
                            AttackDir(null, 9, nDir);
                            break;
                        case Grobal2.CM_42HIT:
                            AttackDir(null, 10, nDir);
                            AttackDir(null, 11, nDir);
                            break;
                    }
                    if (MagicArr[MagicConst.SKILL_YEDO] != null && UseItems[Grobal2.U_WEAPON].Dura > 0)
                    {
                        AttackSkillCount -= 1;
                        if (AttackSkillPointCount == AttackSkillCount)
                        {
                            PowerHit = true;
                            SendSocket("+PWR");
                        }
                        if (AttackSkillCount <= 0)
                        {
                            AttackSkillCount = (byte)(7 - MagicArr[MagicConst.SKILL_YEDO].Level);
                            AttackSkillPointCount = M2Share.RandomNumber.RandomByte(AttackSkillCount);
                        }
                    }
                    HealthTick -= 30;
                    SpellTick -= 100;
                    SpellTick = HUtil32._MAX(0, SpellTick);
                    PerHealth -= 2;
                    PerSpell -= 2;
                }
            }
            catch (Exception e)
            {
                M2Share.Log.Error(sExceptionMsg);
                M2Share.Log.Error(e.StackTrace);
            }
            return result;
        }

        private bool ClientHorseRunXY(int wIdent, short nX, short nY, bool boLateDelivery, ref int dwDelayTime)
        {
            var result = false;
            byte n14;
            int dwCheckTime;
            dwDelayTime = 0;
            if (!BoCanRun)
            {
                return result;
            }
            if (Death || StatusArr[PoisonState.STONE] != 0)// 防麻
            {
                return result;
            }
            if (!M2Share.Config.CloseSpeedHackCheck)
            {
                if (!boLateDelivery)
                {
                    if (!CheckActionStatus(wIdent, ref dwDelayTime))
                    {
                        MBoFilterAction = false;
                        return result;
                    }
                    MBoFilterAction = true;
                    dwCheckTime = HUtil32.GetTickCount() - MDwMoveTick;
                    if (dwCheckTime < M2Share.Config.RunIntervalTime)
                    {
                        MDwMoveCount++;
                        dwDelayTime = M2Share.Config.RunIntervalTime - dwCheckTime;
                        if (dwDelayTime > M2Share.Config.DropOverSpeed)
                        {
                            if (MDwMoveCount >= 4)
                            {
                                MDwMoveTick = HUtil32.GetTickCount();
                                MDwMoveCount = 0;
                                dwDelayTime = M2Share.Config.DropOverSpeed;
                                if (TestSpeedMode)
                                {
                                    SysMsg("马跑步忙复位!!!" + dwDelayTime, MsgColor.Red, MsgType.Hint);
                                }
                            }
                            else
                            {
                                MDwMoveCount = 0;
                            }
                            return result;
                        }
                        else
                        {
                            if (TestSpeedMode)
                            {
                                SysMsg("马跑步忙!!!" + dwDelayTime, MsgColor.Red, MsgType.Hint);
                            }
                            return result;
                        }
                    }
                }
            }
            MDwMoveTick = HUtil32.GetTickCount();
            SpaceMoved = false;
            n14 = M2Share.GetNextDirection(CurrX, CurrY, nX, nY);
            if (HorseRunTo(n14, false))
            {
                if (Transparent && HideMode)
                {
                    StatusArr[PoisonState.STATE_TRANSPARENT] = 1;
                }
                if (SpaceMoved || CurrX == nX && CurrY == nY)
                {
                    result = true;
                }
                HealthTick -= 60;
                SpellTick -= 10;
                SpellTick = HUtil32._MAX(0, SpellTick);
                PerHealth -= 1;
                PerSpell -= 1;
            }
            else
            {
                MDwMoveCount = 0;
            }
            return result;
        }

        private bool ClientSpellXY(int wIdent, int nKey, short nTargetX, short nTargetY, BaseObject TargeTBaseObject, bool boLateDelivery, ref int dwDelayTime)
        {
            dwDelayTime = 0;
            if (!BoCanSpell)
            {
                return false;
            }
            if (Death || StatusArr[PoisonState.STONE] != 0)// 防麻
            {
                return false;
            }
            var UserMagic = GetMagicInfo(nKey);
            if (UserMagic == null)
            {
                return false;
            }
            var boIsWarrSkill = M2Share.MagicMgr.IsWarrSkill(UserMagic.MagIdx);
            if (!boLateDelivery && !boIsWarrSkill && (!M2Share.Config.CloseSpeedHackCheck))
            {
                if (!CheckActionStatus(wIdent, ref dwDelayTime))
                {
                    MBoFilterAction = false;
                    return false;
                }
                MBoFilterAction = true;
                var dwCheckTime = HUtil32.GetTickCount() - MDwMagicAttackTick;
                if (dwCheckTime < MDwMagicAttackInterval)
                {
                    MDwMagicAttackCount++;
                    dwDelayTime = MDwMagicAttackInterval - dwCheckTime;
                    if (dwDelayTime > M2Share.Config.MagicHitIntervalTime / 3)
                    {
                        if (MDwMagicAttackCount >= 4)
                        {
                            MDwMagicAttackTick = HUtil32.GetTickCount();
                            MDwMagicAttackCount = 0;
                            dwDelayTime = M2Share.Config.MagicHitIntervalTime / 3;
                            if (TestSpeedMode)
                            {
                                SysMsg("魔法忙复位!!!" + dwDelayTime, MsgColor.Red, MsgType.Hint);
                            }
                        }
                        else
                        {
                            MDwMagicAttackCount = 0;
                        }
                        return false;
                    }
                    if (TestSpeedMode)
                    {
                        SysMsg("魔法忙!!!" + dwDelayTime, MsgColor.Red, MsgType.Hint);
                    }
                    return false;
                }
            }
            SpellTick -= 450;
            SpellTick = HUtil32._MAX(0, SpellTick);
            if (!boIsWarrSkill)
            {
                MDwMagicAttackInterval = UserMagic.Magic.DelayTime + M2Share.Config.MagicHitIntervalTime;
            }
            MDwMagicAttackTick = HUtil32.GetTickCount();
            ushort nSpellPoint;
            bool result;
            switch (UserMagic.MagIdx)
            {
                case MagicConst.SKILL_ERGUM:
                    if (MagicArr[MagicConst.SKILL_ERGUM] != null)
                    {
                        if (!UseThrusting)
                        {
                            ThrustingOnOff(true);
                            SendSocket("+LNG");
                        }
                        else
                        {
                            ThrustingOnOff(false);
                            SendSocket("+ULNG");
                        }
                    }
                    result = true;
                    break;
                case MagicConst.SKILL_BANWOL:
                    if (MagicArr[MagicConst.SKILL_BANWOL] != null)
                    {
                        if (!UseHalfMoon)
                        {
                            HalfMoonOnOff(true);
                            SendSocket("+WID");
                        }
                        else
                        {
                            HalfMoonOnOff(false);
                            SendSocket("+UWID");
                        }
                    }
                    result = true;
                    break;
                case MagicConst.SKILL_REDBANWOL:
                    if (MagicArr[MagicConst.SKILL_REDBANWOL] != null)
                    {
                        if (!RedUseHalfMoon)
                        {
                            RedHalfMoonOnOff(true);
                            SendSocket("+WID");
                        }
                        else
                        {
                            RedHalfMoonOnOff(false);
                            SendSocket("+UWID");
                        }
                    }
                    result = true;
                    break;
                case MagicConst.SKILL_FIRESWORD:
                    if (MagicArr[MagicConst.SKILL_FIRESWORD] != null)
                    {
                        if (AllowFireHitSkill())
                        {
                            nSpellPoint = GetSpellPoint(UserMagic);
                            if (WAbil.MP >= nSpellPoint)
                            {
                                if (nSpellPoint > 0)
                                {
                                    DamageSpell(nSpellPoint);
                                    HealthSpellChanged();
                                }
                                SendSocket("+FIR");
                            }
                        }
                    }
                    result = true;
                    break;
                case MagicConst.SKILL_MOOTEBO:
                    result = true;
                    if ((HUtil32.GetTickCount() - DoMotaeboTick) > 3 * 1000)
                    {
                        DoMotaeboTick = HUtil32.GetTickCount();
                        Direction = (byte)nTargetX;
                        nSpellPoint = GetSpellPoint(UserMagic);
                        if (WAbil.MP >= nSpellPoint)
                        {
                            if (nSpellPoint > 0)
                            {
                                DamageSpell(nSpellPoint);
                                HealthSpellChanged();
                            }
                            if (DoMotaebo(Direction, UserMagic.Level))
                            {
                                if (UserMagic.Level < 3)
                                {
                                    if (UserMagic.Magic.TrainLevel[UserMagic.Level] < Abil.Level)
                                    {
                                        TrainSkill(UserMagic, M2Share.RandomNumber.Random(3) + 1);
                                        if (!CheckMagicLevelup(UserMagic))
                                        {
                                            SendDelayMsg(this, Grobal2.RM_MAGIC_LVEXP, 0, UserMagic.Magic.MagicId, UserMagic.Level, UserMagic.TranPoint, "", 1000);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    break;
                case MagicConst.SKILL_CROSSMOON:
                    if (MagicArr[MagicConst.SKILL_CROSSMOON] != null)
                    {
                        if (!CrsHitkill)
                        {
                            SkillCrsOnOff(true);
                            SendSocket("+CRS");
                        }
                        else
                        {
                            SkillCrsOnOff(false);
                            SendSocket("+UCRS");
                        }
                    }
                    result = true;
                    break;
                case MagicConst.SKILL_TWINBLADE:// 狂风斩
                    if (MagicArr[MagicConst.SKILL_TWINBLADE] != null)
                    {
                        if (AllowTwinHitSkill())
                        {
                            nSpellPoint = GetSpellPoint(UserMagic);
                            if (WAbil.MP >= nSpellPoint)
                            {
                                if (nSpellPoint > 0)
                                {
                                    DamageSpell(nSpellPoint);
                                    HealthSpellChanged();
                                }
                                SendSocket("+TWN");
                            }
                        }
                    }
                    result = true;
                    break;
                case MagicConst.SKILL_43:// 破空剑
                    if (MagicArr[MagicConst.SKILL_43] != null)
                    {
                        if (!MBo43Kill)
                        {
                            Skill43OnOff(true);
                            SendSocket("+CID");
                        }
                        else
                        {
                            Skill43OnOff(false);
                            SendSocket("+UCID");
                        }
                    }
                    result = true;
                    break;
                default:
                    Direction = M2Share.GetNextDirection(CurrX, CurrY, nTargetX, nTargetY); ;
                    BaseObject BaseObject = null;
                    if (CretInNearXy(TargeTBaseObject, nTargetX, nTargetY)) // 检查目标角色，与目标座标误差范围，如果在误差范围内则修正目标座标
                    {
                        BaseObject = TargeTBaseObject;
                        nTargetX = BaseObject.CurrX;
                        nTargetY = BaseObject.CurrY;
                    }
                    if (!DoSpell(UserMagic, nTargetX, nTargetY, BaseObject))
                    {
                        SendRefMsg(Grobal2.RM_MAGICFIREFAIL, 0, 0, 0, 0, "");
                    }
                    result = true;
                    break;
            }
            return result;
        }

        private bool ClientRunXY(int wIdent, short nX, short nY, int nFlag, ref int dwDelayTime)
        {
            bool result = false;
            byte nDir;
            dwDelayTime = 0;
            if (!BoCanRun)
            {
                return false;
            }
            if (Death || StatusArr[PoisonState.STONE] != 0)
            {
                return false;
            }
            if (nFlag != wIdent && (!M2Share.Config.CloseSpeedHackCheck))
            {
                if (!CheckActionStatus(wIdent, ref dwDelayTime))
                {
                    MBoFilterAction = false;
                    return false;
                }
                MBoFilterAction = true;
                var dwCheckTime = HUtil32.GetTickCount() - MDwMoveTick;
                if (dwCheckTime < M2Share.Config.RunIntervalTime)
                {
                    MDwMoveCount++;
                    dwDelayTime = M2Share.Config.RunIntervalTime - dwCheckTime;
                    if (dwDelayTime > M2Share.Config.RunIntervalTime / 3)
                    {
                        if (MDwMoveCount >= 4)
                        {
                            MDwMoveTick = HUtil32.GetTickCount();
                            MDwMoveCount = 0;
                            dwDelayTime = M2Share.Config.RunIntervalTime / 3;
                            if (TestSpeedMode)
                            {
                                SysMsg("跑步忙复位!!!" + dwDelayTime, MsgColor.Red, MsgType.Hint);
                            }
                        }
                        else
                        {
                            MDwMoveCount = 0;
                        }
                        return result;
                    }
                    if (TestSpeedMode)
                    {
                        SysMsg("跑步忙!!!" + dwDelayTime, MsgColor.Red, MsgType.Hint);
                    }
                    return result;
                }
            }
            MDwMoveTick = HUtil32.GetTickCount();
            SpaceMoved = false;
            nDir = M2Share.GetNextDirection(CurrX, CurrY, nX, nY);
            if (RunTo(nDir, false, nX, nY))
            {
                if (Transparent && HideMode)
                {
                    StatusArr[PoisonState.STATE_TRANSPARENT] = 1;
                }
                if (SpaceMoved || CurrX == nX && CurrY == nY)
                {
                    result = true;
                }
                HealthTick -= 60;
                SpellTick -= 10;
                SpellTick = HUtil32._MAX(0, SpellTick);
                PerHealth -= 1;
                PerSpell -= 1;
            }
            else
            {
                MDwMoveCount = 0;
            }
            return result;
        }

        private bool ClientWalkXY(int wIdent, short nX, short nY, bool boLateDelivery, ref int dwDelayTime)
        {
            bool result = false;
            int n14;
            dwDelayTime = 0;
            if (!BoCanWalk)
            {
                return false;
            }
            if (Death || StatusArr[PoisonState.STONE] != 0)
            {
                return false; // 防麻
            }
            if (!boLateDelivery && (!M2Share.Config.CloseSpeedHackCheck))
            {
                if (!CheckActionStatus(wIdent, ref dwDelayTime))
                {
                    MBoFilterAction = false;
                    return false;
                }
                MBoFilterAction = true;
                int dwCheckTime = HUtil32.GetTickCount() - MDwMoveTick;
                if (dwCheckTime < M2Share.Config.WalkIntervalTime)
                {
                    MDwMoveCount++;
                    dwDelayTime = M2Share.Config.WalkIntervalTime - dwCheckTime;
                    if (dwDelayTime > M2Share.Config.WalkIntervalTime / 3)
                    {
                        if (MDwMoveCount >= 4)
                        {
                            MDwMoveTick = HUtil32.GetTickCount();
                            MDwMoveCount = 0;
                            dwDelayTime = M2Share.Config.WalkIntervalTime / 3;
                            if (TestSpeedMode)
                            {
                                SysMsg("走路忙复位!!!" + dwDelayTime, MsgColor.Red, MsgType.Hint);
                            }
                        }
                        else
                        {
                            MDwMoveCount = 0;
                        }
                        return false;
                    }
                    if (TestSpeedMode)
                    {
                        SysMsg("走路忙!!!" + dwDelayTime, MsgColor.Red, MsgType.Hint);
                    }
                    return false;
                }
            }
            MDwMoveTick = HUtil32.GetTickCount();
            SpaceMoved = false;
            n14 = M2Share.GetNextDirection(CurrX, CurrY, nX, nY);
            if (WalkTo((byte)n14, false))
            {
                if (SpaceMoved || CurrX == nX && CurrY == nY)
                {
                    result = true;
                }
                HealthTick -= 10;
            }
            else
            {
                MDwMoveCount = 0;
            }
            return result;
        }
    }
}