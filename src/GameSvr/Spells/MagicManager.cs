using System;
using System.Collections.Generic;
using SystemModule;

namespace GameSvr
{
    public class MagicManager
    {
        private int MagPushArround(TBaseObject PlayObject, int nPushLevel)
        {
            var result = 0;
            for (var i = 0; i < PlayObject.m_VisibleActors.Count; i++)
            {
                var BaseObject = PlayObject.m_VisibleActors[i].BaseObject;
                if (Math.Abs(PlayObject.m_nCurrX - BaseObject.m_nCurrX) <= 1 && Math.Abs(PlayObject.m_nCurrY - BaseObject.m_nCurrY) <= 1)
                {
                    if (!BaseObject.m_boDeath && BaseObject != PlayObject)
                    {
                        if (PlayObject.m_Abil.Level > BaseObject.m_Abil.Level && !BaseObject.m_boStickMode)
                        {
                            var levelgap = PlayObject.m_Abil.Level - BaseObject.m_Abil.Level;
                            if (M2Share.RandomNumber.Random(20) < 6 + nPushLevel * 3 + levelgap)
                            {
                                if (PlayObject.IsProperTarget(BaseObject))
                                {
                                    var push = 1 + HUtil32._MAX(0, nPushLevel - 1) + M2Share.RandomNumber.Random(2);
                                    var nDir = M2Share.GetNextDirection(PlayObject.m_nCurrX, PlayObject.m_nCurrY, BaseObject.m_nCurrX, BaseObject.m_nCurrY);
                                    BaseObject.CharPushed(nDir, push);
                                    result++;
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        private bool MagBigHealing(TBaseObject PlayObject, int nPower, int nX, int nY)
        {
            var result = false;
            IList<TBaseObject> BaseObjectList = new List<TBaseObject>();
            PlayObject.GetMapBaseObjects(PlayObject.m_PEnvir, nX, nY, 1, BaseObjectList);
            for (var i = 0; i < BaseObjectList.Count; i++)
            {
                var BaseObject = BaseObjectList[i];
                if (PlayObject.IsProperFriend(BaseObject))
                {
                    if (BaseObject.m_WAbil.HP < BaseObject.m_WAbil.MaxHP)
                    {
                        BaseObject.SendDelayMsg(PlayObject, Grobal2.RM_MAGHEALING, 0, nPower, 0, 0, "", 800);
                        result = true;
                    }
                    if (PlayObject.m_boAbilSeeHealGauge)
                    {
                        PlayObject.SendMsg(BaseObject, Grobal2.RM_10414, 0, 0, 0, 0, "");
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 是否战士技能
        /// </summary>
        /// <param name="wMagIdx"></param>
        /// <returns></returns>
        public bool IsWarrSkill(int wMagIdx)
        {
            var result = false;
            switch (wMagIdx)
            {
                case SpellsDef.SKILL_ONESWORD:
                case SpellsDef.SKILL_ILKWANG:
                case SpellsDef.SKILL_YEDO:
                case SpellsDef.SKILL_ERGUM:
                case SpellsDef.SKILL_BANWOL:
                case SpellsDef.SKILL_FIRESWORD:
                case SpellsDef.SKILL_MOOTEBO:
                case SpellsDef.SKILL_CROSSMOON:
                case SpellsDef.SKILL_TWINBLADE:
                    result = true;
                    break;
            }
            return result;
        }

        private ushort DoSpell_MPow(TUserMagic UserMagic)
        {
            return (ushort)(UserMagic.MagicInfo.wPower + M2Share.RandomNumber.Random(UserMagic.MagicInfo.wMaxPower - UserMagic.MagicInfo.wPower));
        }

        private ushort DoSpell_GetPower(TUserMagic UserMagic, ushort nPower)
        {
            return (ushort)(HUtil32.Round(nPower / (UserMagic.MagicInfo.btTrainLv + 1) * (UserMagic.btLevel + 1)) + UserMagic.MagicInfo.btDefPower +
                            M2Share.RandomNumber.Random(UserMagic.MagicInfo.btDefMaxPower - UserMagic.MagicInfo.btDefPower));
        }

        private ushort DoSpell_GetPower13(TUserMagic UserMagic, int nInt)
        {
            var d10 = nInt / 3.0;
            var d18 = nInt - d10;
            var result = (ushort)HUtil32.Round(d18 / (UserMagic.MagicInfo.btTrainLv + 1) * (UserMagic.btLevel + 1) + d10 + (UserMagic.MagicInfo.btDefPower + M2Share.RandomNumber.Random(UserMagic.MagicInfo.btDefMaxPower - UserMagic.MagicInfo.btDefPower)));
            return result;
        }

        private ushort DoSpell_GetRPow(int wInt)
        {
            ushort result;
            if (HUtil32.HiWord(wInt) > HUtil32.LoWord(wInt))
            {
                result = (ushort)(M2Share.RandomNumber.Random(HUtil32.HiWord(wInt) - HUtil32.LoWord(wInt) + 1) + HUtil32.LoWord(wInt));
            }
            else
            {
                result = HUtil32.LoWord(wInt);
            }
            return result;
        }

        public void DoSpell_sub_4934B4(TPlayObject PlayObject)
        {
            if (PlayObject.m_UseItems[Grobal2.U_ARMRINGL].Dura < 100)
            {
                PlayObject.m_UseItems[Grobal2.U_ARMRINGL].Dura = 0;
                PlayObject.SendDelItems(PlayObject.m_UseItems[Grobal2.U_ARMRINGL]);
                PlayObject.m_UseItems[Grobal2.U_ARMRINGL].wIndex = 0;
            }
        }

        public bool DoSpell(TPlayObject PlayObject, TUserMagic UserMagic, short nTargetX, short nTargetY, TBaseObject TargeTBaseObject)
        {
            var result = false;
            short n14 = 0;
            short n18 = 0;
            int n1C;
            ushort nPower = 0;
            short nAmuletIdx = 0;
            if (IsWarrSkill(UserMagic.wMagIdx))
            {
                return result;
            }
            if ((Math.Abs(PlayObject.m_nCurrX - nTargetX) > M2Share.g_Config.nMagicAttackRage) || (Math.Abs(PlayObject.m_nCurrY - nTargetY) > M2Share.g_Config.nMagicAttackRage))
            {
                return result;
            }
            PlayObject.SendRefMsg(Grobal2.RM_SPELL, UserMagic.MagicInfo.btEffect, nTargetX, nTargetY, UserMagic.MagicInfo.wMagicID, "");
            if (TargeTBaseObject != null && TargeTBaseObject.m_boDeath)
            {
                TargeTBaseObject = null;
            }
            var boTrain = false;
            var boSpellFail = false;
            var boSpellFire = true;
            if (PlayObject.m_nSoftVersionDateEx == 0 && PlayObject.m_dwClientTick == 0 && UserMagic.MagicInfo.wMagicID > 40)
            {
                return result;
            }
            switch (UserMagic.MagicInfo.wMagicID)
            {
                case SpellsDef.SKILL_FIREBALL:
                case SpellsDef.SKILL_FIREBALL2:
                    if (PlayObject.MagCanHitTarget(PlayObject.m_nCurrX, PlayObject.m_nCurrY, TargeTBaseObject))
                    {
                        if (PlayObject.IsProperTarget(TargeTBaseObject))
                        {
                            if (TargeTBaseObject.m_nAntiMagic <= M2Share.RandomNumber.Random(10) && Math.Abs(TargeTBaseObject.m_nCurrX - nTargetX) <= 1 && Math.Abs(TargeTBaseObject.m_nCurrY - nTargetY) <= 1)
                            {
                                nPower = PlayObject.GetAttackPower(DoSpell_GetPower(UserMagic, DoSpell_MPow(UserMagic)) + HUtil32.LoWord(PlayObject.m_WAbil.MC), HUtil32.HiWord(PlayObject.m_WAbil.MC) - HUtil32.LoWord(PlayObject.m_WAbil.MC) + 1);
                                PlayObject.SendDelayMsg(PlayObject, Grobal2.RM_DELAYMAGIC, (short)nPower, HUtil32.MakeLong(nTargetX, nTargetY), 2, TargeTBaseObject.ObjectId, "", 600);
                                if (TargeTBaseObject.m_btRaceServer >= Grobal2.RC_ANIMAL)
                                {
                                    boTrain = true;
                                }
                            }
                            else
                            {
                                TargeTBaseObject = null;
                            }
                        }
                        else
                        {
                            TargeTBaseObject = null;
                        }
                    }
                    else
                    {
                        TargeTBaseObject = null;
                    }
                    break;
                case SpellsDef.SKILL_HEALLING:
                    if (TargeTBaseObject == null)
                    {
                        TargeTBaseObject = PlayObject;
                        nTargetX = PlayObject.m_nCurrX;
                        nTargetY = PlayObject.m_nCurrY;
                    }
                    if (PlayObject.IsProperFriend(TargeTBaseObject))
                    {
                        nPower = PlayObject.GetAttackPower(DoSpell_GetPower(UserMagic, DoSpell_MPow(UserMagic)) + HUtil32.LoWord(PlayObject.m_WAbil.SC) * 2, (HUtil32.HiWord(PlayObject.m_WAbil.SC) - HUtil32.LoWord(PlayObject.m_WAbil.SC)) * 2 + 1);
                        if (TargeTBaseObject.m_WAbil.HP < TargeTBaseObject.m_WAbil.MaxHP)
                        {
                            TargeTBaseObject.SendDelayMsg(PlayObject, Grobal2.RM_MAGHEALING, 0, nPower, 0, 0, "", 800);
                            boTrain = true;
                        }
                        if (PlayObject.m_boAbilSeeHealGauge)
                        {
                            PlayObject.SendMsg(TargeTBaseObject, Grobal2.RM_10414, 0, 0, 0, 0, "");
                        }
                    }
                    break;
                case SpellsDef.SKILL_AMYOUNSUL:
                    boSpellFail = true;
                    if (PlayObject.IsProperTarget(TargeTBaseObject))
                    {
                        if (Magic.CheckAmulet(PlayObject, 1, 2, ref nAmuletIdx))
                        {
                            var StdItem = M2Share.UserEngine.GetStdItem(PlayObject.m_UseItems[nAmuletIdx].wIndex);
                            if (StdItem != null)
                            {
                                Magic.UseAmulet(PlayObject, 1, 2, ref nAmuletIdx);
                                if (M2Share.RandomNumber.Random(TargeTBaseObject.m_btAntiPoison + 7) <= 6)
                                {
                                    switch (StdItem.Shape)
                                    {
                                        case 1:
                                            nPower = (ushort)(DoSpell_GetPower13(UserMagic, 40) + DoSpell_GetRPow(PlayObject.m_WAbil.SC) * 2);// 中毒类型 - 绿毒
                                            TargeTBaseObject.SendDelayMsg(PlayObject, Grobal2.RM_POISON, Grobal2.POISON_DECHEALTH, nPower, PlayObject.ObjectId, HUtil32.Round(UserMagic.btLevel / 3 * (nPower / M2Share.g_Config.nAmyOunsulPoint)), "", 1000);
                                            break;
                                        case 2:
                                            nPower = (ushort)(DoSpell_GetPower13(UserMagic, 30) + DoSpell_GetRPow(PlayObject.m_WAbil.SC) * 2);// 中毒类型 - 红毒
                                            TargeTBaseObject.SendDelayMsg(PlayObject, Grobal2.RM_POISON, Grobal2.POISON_DAMAGEARMOR, nPower, PlayObject.ObjectId, HUtil32.Round(UserMagic.btLevel / 3 * (nPower / M2Share.g_Config.nAmyOunsulPoint)), "", 1000);
                                            break;
                                    }
                                    if (TargeTBaseObject.m_btRaceServer == Grobal2.RC_PLAYOBJECT || TargeTBaseObject.m_btRaceServer >= Grobal2.RC_ANIMAL)
                                    {
                                        boTrain = true;
                                    }
                                }
                                PlayObject.SetTargetCreat(TargeTBaseObject);
                                boSpellFail = false;
                            }
                        }
                    }
                    break;
                case SpellsDef.SKILL_FIREWIND:
                    if (MagPushArround(PlayObject, UserMagic.btLevel) > 0)
                    {
                        boTrain = true;
                    }
                    break;
                case SpellsDef.SKILL_FIRE:
                    n1C = M2Share.GetNextDirection(PlayObject.m_nCurrX, PlayObject.m_nCurrY, nTargetX, nTargetY);
                    if (PlayObject.m_PEnvir.GetNextPosition(PlayObject.m_nCurrX, PlayObject.m_nCurrY, n1C, 1, ref n14, ref n18))
                    {
                        PlayObject.m_PEnvir.GetNextPosition(PlayObject.m_nCurrX, PlayObject.m_nCurrY, n1C, 5, ref nTargetX, ref nTargetY);
                        nPower = PlayObject.GetAttackPower(DoSpell_GetPower(UserMagic, DoSpell_MPow(UserMagic)) + HUtil32.LoWord(PlayObject.m_WAbil.MC), HUtil32.HiWord(PlayObject.m_WAbil.MC) - HUtil32.LoWord(PlayObject.m_WAbil.MC) + 1);
                        if (PlayObject.MagPassThroughMagic(n14, n18, nTargetX, nTargetY, n1C, nPower, false) > 0)
                        {
                            boTrain = true;
                        }
                    }
                    break;
                case SpellsDef.SKILL_SHOOTLIGHTEN:
                    n1C = M2Share.GetNextDirection(PlayObject.m_nCurrX, PlayObject.m_nCurrY, nTargetX, nTargetY);
                    if (PlayObject.m_PEnvir.GetNextPosition(PlayObject.m_nCurrX, PlayObject.m_nCurrY, n1C, 1, ref n14, ref n18))
                    {
                        PlayObject.m_PEnvir.GetNextPosition(PlayObject.m_nCurrX, PlayObject.m_nCurrY, n1C, 8, ref nTargetX, ref nTargetY);
                        nPower = PlayObject.GetAttackPower(DoSpell_GetPower(UserMagic, DoSpell_MPow(UserMagic)) + HUtil32.LoWord(PlayObject.m_WAbil.MC), (ushort)(HUtil32.HiWord(PlayObject.m_WAbil.MC) - HUtil32.LoWord(PlayObject.m_WAbil.MC) + 1));
                        if (PlayObject.MagPassThroughMagic(n14, n18, nTargetX, nTargetY, n1C, nPower, true) > 0)
                        {
                            boTrain = true;
                        }
                    }
                    break;
                case SpellsDef.SKILL_LIGHTENING:
                    if (PlayObject.IsProperTarget(TargeTBaseObject))
                    {
                        if (M2Share.RandomNumber.Random(10) >= TargeTBaseObject.m_nAntiMagic)
                        {
                            nPower = PlayObject.GetAttackPower(DoSpell_GetPower(UserMagic, DoSpell_MPow(UserMagic)) + HUtil32.LoWord(PlayObject.m_WAbil.MC), HUtil32.HiWord(PlayObject.m_WAbil.MC) - HUtil32.LoWord(PlayObject.m_WAbil.MC) + 1);
                            if (TargeTBaseObject.m_btLifeAttrib == Grobal2.LA_UNDEAD)
                            {
                                nPower = (ushort)HUtil32.Round(nPower * 1.5);
                            }
                            PlayObject.SendDelayMsg(PlayObject, Grobal2.RM_DELAYMAGIC, (short)nPower, HUtil32.MakeLong(nTargetX, nTargetY), 2, TargeTBaseObject.ObjectId, "", 600);
                            if (TargeTBaseObject.m_btRaceServer >= Grobal2.RC_ANIMAL)
                            {
                                boTrain = true;
                            }
                        }
                        else
                        {
                            TargeTBaseObject = null;
                        }
                    }
                    else
                    {
                        TargeTBaseObject = null;
                    }
                    break;
                case SpellsDef.SKILL_FIRECHARM:
                case SpellsDef.SKILL_HANGMAJINBUB:
                case SpellsDef.SKILL_DEJIWONHO:
                case SpellsDef.SKILL_HOLYSHIELD:
                case SpellsDef.SKILL_SKELLETON:
                case SpellsDef.SKILL_CLOAK:
                case SpellsDef.SKILL_BIGCLOAK:
                    boSpellFail = true;
                    if (Magic.CheckAmulet(PlayObject, 1, 1, ref nAmuletIdx))
                    {
                        Magic.UseAmulet(PlayObject, 1, 1, ref nAmuletIdx);
                        switch (UserMagic.MagicInfo.wMagicID)
                        {
                            case SpellsDef.SKILL_FIRECHARM:
                                if (PlayObject.MagCanHitTarget(PlayObject.m_nCurrX, PlayObject.m_nCurrY, TargeTBaseObject))
                                {
                                    if (PlayObject.IsProperTarget(TargeTBaseObject))
                                    {
                                        if (M2Share.RandomNumber.Random(10) >= TargeTBaseObject.m_nAntiMagic)
                                        {
                                            if (Math.Abs(TargeTBaseObject.m_nCurrX - nTargetX) <= 1 && Math.Abs(TargeTBaseObject.m_nCurrY - nTargetY) <= 1)
                                            {
                                                nPower = PlayObject.GetAttackPower(DoSpell_GetPower(UserMagic, DoSpell_MPow(UserMagic)) + HUtil32.LoWord(PlayObject.m_WAbil.SC), HUtil32.HiWord(PlayObject.m_WAbil.SC) - HUtil32.LoWord(PlayObject.m_WAbil.SC) + 1);
                                                PlayObject.SendDelayMsg(PlayObject, Grobal2.RM_DELAYMAGIC, (short)nPower, HUtil32.MakeLong(nTargetX, nTargetY), 2, TargeTBaseObject.ObjectId, "", 1200);
                                                if (TargeTBaseObject.m_btRaceServer >= Grobal2.RC_ANIMAL)
                                                {
                                                    boTrain = true;
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    TargeTBaseObject = null;
                                }
                                break;
                            case SpellsDef.SKILL_HANGMAJINBUB:
                                nPower = PlayObject.GetAttackPower(DoSpell_GetPower13(UserMagic, 60) + HUtil32.LoWord(PlayObject.m_WAbil.SC) * 10, HUtil32.HiWord(PlayObject.m_WAbil.SC) - HUtil32.LoWord(PlayObject.m_WAbil.SC) + 1);
                                if (PlayObject.MagMakeDefenceArea(nTargetX, nTargetY, 3, nPower, 1) > 0)
                                {
                                    boTrain = true;
                                }
                                break;
                            case SpellsDef.SKILL_DEJIWONHO:
                                nPower = PlayObject.GetAttackPower(DoSpell_GetPower13(UserMagic, 60) + HUtil32.LoWord(PlayObject.m_WAbil.SC) * 10, HUtil32.HiWord(PlayObject.m_WAbil.SC) - HUtil32.LoWord(PlayObject.m_WAbil.SC) + 1);
                                if (PlayObject.MagMakeDefenceArea(nTargetX, nTargetY, 3, nPower, 0) > 0)
                                {
                                    boTrain = true;
                                }
                                break;
                            case SpellsDef.SKILL_HOLYSHIELD:
                                if (MagMakeHolyCurtain(PlayObject, DoSpell_GetPower13(UserMagic, 40) + DoSpell_GetRPow(PlayObject.m_WAbil.SC) * 3, nTargetX, nTargetY) > 0)
                                {
                                    boTrain = true;
                                }
                                break;
                            case SpellsDef.SKILL_SKELLETON:
                                if (MagMakeSlave(PlayObject, UserMagic))
                                {
                                    boTrain = true;
                                }
                                break;
                            case SpellsDef.SKILL_CLOAK:
                                if (MagMakePrivateTransparent(PlayObject, DoSpell_GetPower13(UserMagic, 30) + DoSpell_GetRPow(PlayObject.m_WAbil.SC) * 3))
                                {
                                    boTrain = true;
                                }
                                break;
                            case SpellsDef.SKILL_BIGCLOAK:
                                if (MagMakeGroupTransparent(PlayObject, nTargetX, nTargetY, DoSpell_GetPower13(UserMagic, 30) + DoSpell_GetRPow(PlayObject.m_WAbil.SC) * 3))
                                {
                                    boTrain = true;
                                }
                                break;
                        }
                        boSpellFail = false;
                    }
                    break;
                case SpellsDef.SKILL_TAMMING:
                    if (PlayObject.IsProperTarget(TargeTBaseObject))
                    {
                        if (MagTamming(PlayObject, TargeTBaseObject, nTargetX, nTargetY, UserMagic.btLevel))
                        {
                            boTrain = true;
                        }
                    }
                    break;
                case SpellsDef.SKILL_SPACEMOVE:
                    var targerActors = TargeTBaseObject == null ? 0 : TargeTBaseObject.ObjectId;
                    PlayObject.SendRefMsg(Grobal2.RM_MAGICFIRE, 0, HUtil32.MakeWord(UserMagic.MagicInfo.btEffectType, UserMagic.MagicInfo.btEffect), HUtil32.MakeLong(nTargetX, nTargetY), targerActors, "");
                    boSpellFire = false;
                    if (MagSaceMove(PlayObject, UserMagic.btLevel))
                    {
                        boTrain = true;
                    }
                    break;
                case SpellsDef.SKILL_EARTHFIRE:
                    if (MagMakeFireCross(PlayObject, PlayObject.GetAttackPower(DoSpell_GetPower(UserMagic, DoSpell_MPow(UserMagic)) + HUtil32.LoWord(PlayObject.m_WAbil.MC), HUtil32.HiWord(PlayObject.m_WAbil.MC) - HUtil32.LoWord(PlayObject.m_WAbil.MC) + 1), DoSpell_GetPower(UserMagic, 10) + (DoSpell_GetRPow(PlayObject.m_WAbil.MC) >> 1), nTargetX, nTargetY) > 0)
                    {
                        boTrain = true;
                    }
                    break;
                case SpellsDef.SKILL_FIREBOOM:
                    if (MagBigExplosion(PlayObject, PlayObject.GetAttackPower(DoSpell_GetPower(UserMagic, DoSpell_MPow(UserMagic)) + HUtil32.LoWord(PlayObject.m_WAbil.MC), HUtil32.HiWord(PlayObject.m_WAbil.MC) - HUtil32.LoWord(PlayObject.m_WAbil.MC) + 1), nTargetX, nTargetY, M2Share.g_Config.nFireBoomRage))
                    {
                        boTrain = true;
                    }
                    break;
                case SpellsDef.SKILL_LIGHTFLOWER:
                    if (MagElecBlizzard(PlayObject, PlayObject.GetAttackPower(DoSpell_GetPower(UserMagic, DoSpell_MPow(UserMagic)) + HUtil32.LoWord(PlayObject.m_WAbil.MC), 
                        HUtil32.HiWord(PlayObject.m_WAbil.MC) - HUtil32.LoWord(PlayObject.m_WAbil.MC) + 1)))
                    {
                        boTrain = true;
                    }
                    break;
                case SpellsDef.SKILL_SHOWHP:
                    if (TargeTBaseObject != null && !TargeTBaseObject.m_boShowHP)
                    {
                        if (M2Share.RandomNumber.Random(6) <= UserMagic.btLevel + 3)
                        {
                            TargeTBaseObject.m_dwShowHPTick = HUtil32.GetTickCount();
                            TargeTBaseObject.m_dwShowHPInterval = DoSpell_GetPower13(UserMagic, DoSpell_GetRPow(PlayObject.m_WAbil.SC) * 2 + 30) * 1000;
                            TargeTBaseObject.SendDelayMsg(TargeTBaseObject, Grobal2.RM_DOOPENHEALTH, 0, 0, 0, 0, "", 1500);
                            boTrain = true;
                        }
                    }
                    break;
                case SpellsDef.SKILL_BIGHEALLING:
                    nPower = PlayObject.GetAttackPower(DoSpell_GetPower(UserMagic, DoSpell_MPow(UserMagic)) + HUtil32.LoWord(PlayObject.m_WAbil.SC) * 2, (HUtil32.HiWord(PlayObject.m_WAbil.SC) - HUtil32.LoWord(PlayObject.m_WAbil.SC)) * 2 + 1);
                    if (MagBigHealing(PlayObject, nPower, nTargetX, nTargetY))
                    {
                        boTrain = true;
                    }
                    break;
                case SpellsDef.SKILL_SINSU:
                    boSpellFail = true;
                    if (Magic.CheckAmulet(PlayObject, 5, 1, ref nAmuletIdx))
                    {
                        Magic.UseAmulet(PlayObject, 5, 1, ref nAmuletIdx);
                        if (MagMakeSinSuSlave(PlayObject, UserMagic))
                        {
                            boTrain = true;
                        }
                        boSpellFail = false;
                    }
                    break;
                case SpellsDef.SKILL_ANGEL:
                    boSpellFail = true;
                    if (Magic.CheckAmulet(PlayObject, 2, 1, ref nAmuletIdx))
                    {
                        Magic.UseAmulet(PlayObject, 2, 1, ref nAmuletIdx);
                        if (MagMakeAngelSlave(PlayObject, UserMagic))
                        {
                            boTrain = true;
                        }
                        boSpellFail = false;
                    }
                    break;
                case SpellsDef.SKILL_SHIELD:
                    if (PlayObject.MagBubbleDefenceUp(UserMagic.btLevel, DoSpell_GetPower(UserMagic, (ushort)(DoSpell_GetRPow(PlayObject.m_WAbil.MC) + 15))))
                    {
                        boTrain = true;
                    }
                    break;
                case SpellsDef.SKILL_KILLUNDEAD:
                    if (PlayObject.IsProperTarget(TargeTBaseObject))
                    {
                        if (MagTurnUndead(PlayObject, TargeTBaseObject, nTargetX, nTargetY, UserMagic.btLevel))
                        {
                            boTrain = true;
                        }
                    }
                    break;
                case SpellsDef.SKILL_SNOWWIND:
                    if (MagBigExplosion(PlayObject, PlayObject.GetAttackPower(DoSpell_GetPower(UserMagic, DoSpell_MPow(UserMagic)) + HUtil32.LoWord(PlayObject.m_WAbil.MC), HUtil32.HiWord(PlayObject.m_WAbil.MC) - HUtil32.LoWord(PlayObject.m_WAbil.MC) + 1), nTargetX, nTargetY, M2Share.g_Config.nSnowWindRange))
                    {
                        boTrain = true;
                    }
                    break;
                case SpellsDef.SKILL_UNAMYOUNSUL:
                    if (TargeTBaseObject == null)
                    {
                        TargeTBaseObject = PlayObject;
                        nTargetX = PlayObject.m_nCurrX;
                        nTargetY = PlayObject.m_nCurrY;
                    }
                    if (PlayObject.IsProperFriend(TargeTBaseObject))
                    {
                        if (M2Share.RandomNumber.Random(7) - (UserMagic.btLevel + 1) < 0)
                        {
                            if (TargeTBaseObject.m_wStatusTimeArr[Grobal2.POISON_DECHEALTH] != 0)
                            {
                                TargeTBaseObject.m_wStatusTimeArr[Grobal2.POISON_DECHEALTH] = 1;
                                boTrain = true;
                            }
                            if (TargeTBaseObject.m_wStatusTimeArr[Grobal2.POISON_DAMAGEARMOR] != 0)
                            {
                                TargeTBaseObject.m_wStatusTimeArr[Grobal2.POISON_DAMAGEARMOR] = 1;
                                boTrain = true;
                            }
                            if (TargeTBaseObject.m_wStatusTimeArr[Grobal2.POISON_STONE] != 0)
                            {
                                TargeTBaseObject.m_wStatusTimeArr[Grobal2.POISON_STONE] = 1;
                                boTrain = true;
                            }
                        }
                    }
                    break;
                case SpellsDef.SKILL_WINDTEBO:
                    if (MagWindTebo(PlayObject, UserMagic))
                    {
                        boTrain = true;
                    }
                    break;
                case SpellsDef.SKILL_MABE:
                    nPower = PlayObject.GetAttackPower(DoSpell_GetPower(UserMagic, DoSpell_MPow(UserMagic)) + HUtil32.LoWord(PlayObject.m_WAbil.MC), HUtil32.HiWord(PlayObject.m_WAbil.MC) - HUtil32.LoWord(PlayObject.m_WAbil.MC) + 1);
                    if (MabMabe(PlayObject, TargeTBaseObject, nPower, UserMagic.btLevel, nTargetX, nTargetY))
                    {
                        boTrain = true;
                    }
                    break;
                case SpellsDef.SKILL_GROUPLIGHTENING:
                    if (MagGroupLightening(PlayObject, UserMagic, nTargetX, nTargetY, TargeTBaseObject, ref boSpellFire))
                    {
                        boTrain = true;
                    }
                    break;
                case SpellsDef.SKILL_GROUPAMYOUNSUL:
                    if (MagGroupAmyounsul(PlayObject, UserMagic, nTargetX, nTargetY, TargeTBaseObject))
                    {
                        boTrain = true;
                    }
                    break;
                case SpellsDef.SKILL_GROUPDEDING:
                    if (MagGroupDeDing(PlayObject, UserMagic, nTargetX, nTargetY, TargeTBaseObject))
                    {
                        boTrain = true;
                    }
                    break;
                case SpellsDef.SKILL_43:
                    break;
                case SpellsDef.SKILL_44:
                    if (MagHbFireBall(PlayObject, UserMagic, nTargetX, nTargetY, ref TargeTBaseObject))
                    {
                        boTrain = true;
                    }
                    break;
                case SpellsDef.SKILL_45:
                    if (PlayObject.IsProperTarget(TargeTBaseObject))
                    {
                        if (M2Share.RandomNumber.Random(10) >= TargeTBaseObject.m_nAntiMagic)
                        {
                            nPower = PlayObject.GetAttackPower(DoSpell_GetPower(UserMagic, DoSpell_MPow(UserMagic)) + HUtil32.LoWord(PlayObject.m_WAbil.MC), HUtil32.HiWord(PlayObject.m_WAbil.MC) - HUtil32.LoWord(PlayObject.m_WAbil.MC) + 1);
                            if (TargeTBaseObject.m_btLifeAttrib == Grobal2.LA_UNDEAD)
                            {
                                nPower = (ushort)HUtil32.Round(nPower * 1.5);
                            }
                            PlayObject.SendDelayMsg(PlayObject, Grobal2.RM_DELAYMAGIC, (short)nPower, HUtil32.MakeLong(nTargetX, nTargetY), 2, TargeTBaseObject.ObjectId, "", 600);
                            if (TargeTBaseObject.m_btRaceServer >= Grobal2.RC_ANIMAL)
                            {
                                boTrain = true;
                            }
                        }
                        else
                        {
                            TargeTBaseObject = null;
                        }
                    }
                    else
                    {
                        TargeTBaseObject = null;
                    }
                    break;
                case SpellsDef.SKILL_46:
                    if (MagMakeClone(PlayObject, UserMagic))
                    {
                        boTrain = true;
                    }
                    break;
                case SpellsDef.SKILL_47:
                    if (MagBigExplosion(PlayObject, PlayObject.GetAttackPower(DoSpell_GetPower(UserMagic, DoSpell_MPow(UserMagic)) + HUtil32.LoWord(PlayObject.m_WAbil.MC), HUtil32.HiWord(PlayObject.m_WAbil.MC) - HUtil32.LoWord(PlayObject.m_WAbil.MC) + 1), nTargetX, nTargetY, M2Share.g_Config.nFireBoomRage))
                    {
                        boTrain = true;
                    }
                    break;
                case SpellsDef.SKILL_ENERGYREPULSOR:
                    if (MagPushArround(PlayObject, UserMagic.btLevel) > 0)
                    {
                        boTrain = true;
                    }
                    break;
                case SpellsDef.SKILL_49:
                    boTrain = true;
                    break;
                case SpellsDef.SKILL_UENHANCER:
                    boSpellFail = true;
                    if (TargeTBaseObject == null)
                    {
                        TargeTBaseObject = PlayObject;
                        nTargetX = PlayObject.m_nCurrX;
                        nTargetY = PlayObject.m_nCurrY;
                    }
                    if (PlayObject.IsProperFriend(TargeTBaseObject))
                    {
                        if (Magic.CheckAmulet(PlayObject, 1, 1, ref nAmuletIdx))
                        {
                            Magic.UseAmulet(PlayObject, 1, 1, ref nAmuletIdx);
                            nPower = (ushort)(UserMagic.btLevel + 1 + M2Share.RandomNumber.Random(UserMagic.btLevel));
                            n14 = (short)PlayObject.GetAttackPower(DoSpell_GetPower13(UserMagic, 60) + HUtil32.LoWord(PlayObject.m_WAbil.SC) * 10, HUtil32.HiWord(PlayObject.m_WAbil.SC) - HUtil32.LoWord(PlayObject.m_WAbil.SC) + 1);
                            if (TargeTBaseObject.AttPowerUp(nPower, n14))
                            {
                                boTrain = true;
                            }
                            boSpellFail = false;
                        }
                    }
                    break;
                case SpellsDef.SKILL_51:// 灵魂召唤术
                    boTrain = true;
                    break;
                case SpellsDef.SKILL_52:// 诅咒术
                    boTrain = true;
                    break;
                case SpellsDef.SKILL_53:// 灵魂召唤术
                    boTrain = true;
                    break;
                case SpellsDef.SKILL_54:// 诅咒术
                    boTrain = true;
                    break;
            }
            if (boSpellFail)
            {
                return result;
            }
            if (boSpellFire)
            {
                if (TargeTBaseObject == null)
                {
                    PlayObject.SendRefMsg(Grobal2.RM_MAGICFIRE, 0, HUtil32.MakeWord(UserMagic.MagicInfo.btEffectType, UserMagic.MagicInfo.btEffect), HUtil32.MakeLong(nTargetX, nTargetY), 0, "");
                }
                else
                {
                    PlayObject.SendRefMsg(Grobal2.RM_MAGICFIRE, 0, HUtil32.MakeWord(UserMagic.MagicInfo.btEffectType, UserMagic.MagicInfo.btEffect), HUtil32.MakeLong(nTargetX, nTargetY), TargeTBaseObject.ObjectId, "");
                }
            }
            if (UserMagic.btLevel < 3 && boTrain)
            {
                if (UserMagic.MagicInfo.TrainLevel[UserMagic.btLevel] <= PlayObject.m_Abil.Level)
                {
                    PlayObject.TrainSkill(UserMagic, M2Share.RandomNumber.Random(3) + 1);
                    if (!PlayObject.CheckMagicLevelup(UserMagic))
                    {
                        PlayObject.SendDelayMsg(PlayObject, Grobal2.RM_MAGIC_LVEXP, 0, UserMagic.MagicInfo.wMagicID, UserMagic.btLevel, UserMagic.nTranPoint, "", 1000);
                    }
                }
            }
            return true;
        }

        public bool MagMakePrivateTransparent(TBaseObject BaseObject, int nHTime)
        {
            if (BaseObject.m_wStatusTimeArr[Grobal2.STATE_TRANSPARENT] > 0)
            {
                return false;
            }
            IList<TBaseObject> BaseObjectList = new List<TBaseObject>();
            BaseObject.GetMapBaseObjects(BaseObject.m_PEnvir, BaseObject.m_nCurrX, BaseObject.m_nCurrY, 9, BaseObjectList);
            for (var i = 0; i < BaseObjectList.Count; i++)
            {
                var TargeTBaseObject = BaseObjectList[i];
                if (TargeTBaseObject.m_btRaceServer >= Grobal2.RC_ANIMAL && TargeTBaseObject.m_TargetCret == BaseObject)
                {
                    if (Math.Abs(TargeTBaseObject.m_nCurrX - BaseObject.m_nCurrX) > 1 || Math.Abs(TargeTBaseObject.m_nCurrY - BaseObject.m_nCurrY) > 1 || M2Share.RandomNumber.Random(2) == 0)
                    {
                        TargeTBaseObject.m_TargetCret = null;
                    }
                }
            }
            BaseObjectList.Clear();
            BaseObjectList = null;
            BaseObject.m_wStatusTimeArr[Grobal2.STATE_TRANSPARENT] = (ushort)nHTime;
            BaseObject.m_nCharStatus = BaseObject.GetCharStatus();
            BaseObject.StatusChanged();
            BaseObject.m_boHideMode = true;
            BaseObject.m_boTransparent = true;
            return true;
        }

        private bool MagTamming(TBaseObject BaseObject, TBaseObject TargeTBaseObject, int nTargetX, int nTargetY, int nMagicLevel)
        {
            var result = false;
            if (TargeTBaseObject.m_btRaceServer != Grobal2.RC_PLAYOBJECT && M2Share.RandomNumber.Random(4 - nMagicLevel) == 0)
            {
                TargeTBaseObject.m_TargetCret = null;
                if (TargeTBaseObject.m_Master == BaseObject)
                {
                    TargeTBaseObject.OpenHolySeizeMode((nMagicLevel * 5 + 10) * 1000);
                    result = true;
                }
                else
                {
                    if (M2Share.RandomNumber.Random(2) == 0)
                    {
                        if (TargeTBaseObject.m_Abil.Level <= BaseObject.m_Abil.Level + 2)
                        {
                            if (M2Share.RandomNumber.Random(3) == 0)
                            {
                                if (M2Share.RandomNumber.Random(BaseObject.m_Abil.Level + 20 + nMagicLevel * 5) > TargeTBaseObject.m_Abil.Level + M2Share.g_Config.nMagTammingTargetLevel)
                                {
                                    if (!TargeTBaseObject.m_boNoTame && TargeTBaseObject.m_btLifeAttrib != Grobal2.LA_UNDEAD && TargeTBaseObject.m_Abil.Level < M2Share.g_Config.nMagTammingLevel && BaseObject.m_SlaveList.Count < M2Share.g_Config.nMagTammingCount)
                                    {
                                        int n14 = TargeTBaseObject.m_WAbil.MaxHP / M2Share.g_Config.nMagTammingHPRate;
                                        if (n14 <= 2)
                                        {
                                            n14 = 2;
                                        }
                                        else
                                        {
                                            n14 += n14;
                                        }
                                        if (TargeTBaseObject.m_Master != BaseObject && M2Share.RandomNumber.Random(n14) == 0)
                                        {
                                            TargeTBaseObject.BreakCrazyMode();
                                            if (TargeTBaseObject.m_Master != null)
                                            {
                                                TargeTBaseObject.m_WAbil.HP = (ushort)(TargeTBaseObject.m_WAbil.HP / 10);
                                            }

                                            if (TargeTBaseObject.m_boCanReAlive && TargeTBaseObject.m_Master == null)
                                            {
                                                TargeTBaseObject.m_boCanReAlive = false;
                                                if (TargeTBaseObject.m_pMonGen != null)
                                                {
                                                    if (TargeTBaseObject.m_pMonGen.nActiveCount > 0)
                                                    {
                                                        TargeTBaseObject.m_pMonGen.nActiveCount--;
                                                    }
                                                    else
                                                    {
                                                        TargeTBaseObject.m_pMonGen = null;
                                                    }
                                                }
                                            }
                                            TargeTBaseObject.m_Master = BaseObject;
                                            TargeTBaseObject.m_dwMasterRoyaltyTick = (M2Share.RandomNumber.Random(BaseObject.m_Abil.Level * 2) + (nMagicLevel << 2) * 5 + 20) * 60 * 1000 + HUtil32.GetTickCount();
                                            TargeTBaseObject.m_btSlaveMakeLevel = (byte)nMagicLevel;
                                            if (TargeTBaseObject.m_dwMasterTick == 0)
                                            {
                                                TargeTBaseObject.m_dwMasterTick = HUtil32.GetTickCount();
                                            }
                                            TargeTBaseObject.BreakHolySeizeMode();
                                            if (1500 - nMagicLevel * 200 < TargeTBaseObject.m_nWalkSpeed)
                                            {
                                                TargeTBaseObject.m_nWalkSpeed = 1500 - nMagicLevel * 200;
                                            }
                                            if (2000 - nMagicLevel * 200 < TargeTBaseObject.m_nNextHitTime)
                                            {
                                                TargeTBaseObject.m_nNextHitTime = 2000 - nMagicLevel * 200;
                                            }
                                            TargeTBaseObject.RefShowName();
                                            BaseObject.m_SlaveList.Add(TargeTBaseObject);
                                        }
                                        else
                                        {
                                            if (M2Share.RandomNumber.Random(14) == 0)
                                            {
                                                TargeTBaseObject.m_WAbil.HP = 0;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (TargeTBaseObject.m_btLifeAttrib == Grobal2.LA_UNDEAD && M2Share.RandomNumber.Random(20) == 0)
                                        {
                                            TargeTBaseObject.m_WAbil.HP = 0;
                                        }
                                    }
                                }
                                else
                                {
                                    if (TargeTBaseObject.m_btLifeAttrib != Grobal2.LA_UNDEAD && M2Share.RandomNumber.Random(20) == 0)
                                    {
                                        TargeTBaseObject.OpenCrazyMode(M2Share.RandomNumber.Random(20) + 10);
                                    }
                                }
                            }
                            else
                            {
                                if (TargeTBaseObject.m_btLifeAttrib != Grobal2.LA_UNDEAD)
                                {
                                    TargeTBaseObject.OpenCrazyMode(M2Share.RandomNumber.Random(20) + 10);// 变红
                                }
                            }
                        }
                    }
                    else
                    {
                        TargeTBaseObject.OpenHolySeizeMode((nMagicLevel * 5 + 10) * 1000);
                    }
                    result = true;
                }
            }
            else
            {
                if (M2Share.RandomNumber.Random(2) == 0)
                {
                    result = true;
                }
            }
            return result;
        }

        private bool MagTurnUndead(TBaseObject BaseObject, TBaseObject TargeTBaseObject, int nTargetX, int nTargetY, int nLevel)
        {
            var result = false;
            if (TargeTBaseObject.m_boSuperMan || TargeTBaseObject.m_btLifeAttrib != Grobal2.LA_UNDEAD)
            {
                return result;
            }
            ((AnimalObject)TargeTBaseObject).Struck(BaseObject);
            if (TargeTBaseObject.m_TargetCret == null)
            {
                ((AnimalObject)TargeTBaseObject).m_boRunAwayMode = true;
                ((AnimalObject)TargeTBaseObject).m_dwRunAwayStart = HUtil32.GetTickCount();
                ((AnimalObject)TargeTBaseObject).m_dwRunAwayTime = 10 * 1000;
            }
            BaseObject.SetTargetCreat(TargeTBaseObject);
            if (M2Share.RandomNumber.Random(2) + (BaseObject.m_Abil.Level - 1) > TargeTBaseObject.m_Abil.Level)
            {
                if (TargeTBaseObject.m_Abil.Level < M2Share.g_Config.nMagTurnUndeadLevel)
                {
                    var n14 = BaseObject.m_Abil.Level - TargeTBaseObject.m_Abil.Level;
                    if (M2Share.RandomNumber.Random(100) < (nLevel << 3) - nLevel + 15 + n14)
                    {
                        TargeTBaseObject.SetLastHiter(BaseObject);
                        TargeTBaseObject.m_WAbil.HP = 0;
                        result = true;
                    }
                }
            }
            return result;
        }

        private bool MagWindTebo(TPlayObject PlayObject, TUserMagic UserMagic)
        {
            var result = false;
            var PoseBaseObject = PlayObject.GetPoseCreate();
            if (PoseBaseObject != null && PoseBaseObject != PlayObject && !PoseBaseObject.m_boDeath && !PoseBaseObject.m_boGhost && PlayObject.IsProperTarget(PoseBaseObject) && !PoseBaseObject.m_boStickMode)
            {
                if (Math.Abs(PlayObject.m_nCurrX - PoseBaseObject.m_nCurrX) <= 1 && Math.Abs(PlayObject.m_nCurrY - PoseBaseObject.m_nCurrY) <= 1 && PlayObject.m_Abil.Level > PoseBaseObject.m_Abil.Level)
                {
                    if (M2Share.RandomNumber.Random(20) < UserMagic.btLevel * 6 + 6 + (PlayObject.m_Abil.Level - PoseBaseObject.m_Abil.Level))
                    {
                        PoseBaseObject.CharPushed(M2Share.GetNextDirection(PlayObject.m_nCurrX, PlayObject.m_nCurrY, PoseBaseObject.m_nCurrX, PoseBaseObject.m_nCurrY), HUtil32._MAX(0, UserMagic.btLevel - 1) + 1);
                        result = true;
                    }
                }
            }
            return result;
        }

        private bool MagSaceMove(TBaseObject BaseObject, int nLevel)
        {
            var result = false;
            if (M2Share.RandomNumber.Random(11) < nLevel * 2 + 4)
            {
                BaseObject.SendRefMsg(Grobal2.RM_SPACEMOVE_FIRE2, 0, 0, 0, 0, "");
                if (BaseObject is TPlayObject)
                {
                    var Envir = BaseObject.m_PEnvir;
                    BaseObject.MapRandomMove(BaseObject.m_sHomeMap, 1);
                    if (Envir != BaseObject.m_PEnvir && BaseObject.m_btRaceServer == Grobal2.RC_PLAYOBJECT)
                    {
                        var PlayObject = (TPlayObject)BaseObject;
                        PlayObject.m_boTimeRecall = false;
                    }
                }
                result = true;
            }
            return result;
        }

        private bool MagGroupAmyounsul(TPlayObject PlayObject, TUserMagic UserMagic, int nTargetX, int nTargetY, TBaseObject TargeTBaseObject)
        {
            short nAmuletIdx = 0;
            var result = false;
            IList<TBaseObject> BaseObjectList = new List<TBaseObject>();
            PlayObject.GetMapBaseObjects(PlayObject.m_PEnvir, nTargetX, nTargetY, HUtil32._MAX(1, UserMagic.btLevel), BaseObjectList);
            for (var i = 0; i < BaseObjectList.Count; i++)
            {
                var BaseObject = BaseObjectList[i];
                if (BaseObject.m_boDeath || BaseObject.m_boGhost || PlayObject == BaseObject)
                {
                    continue;
                }
                if (PlayObject.IsProperTarget(BaseObject))
                {
                    if (Magic.CheckAmulet(PlayObject, 1, 2, ref nAmuletIdx))
                    {
                        var StdItem = M2Share.UserEngine.GetStdItem(PlayObject.m_UseItems[nAmuletIdx].wIndex);
                        if (StdItem != null)
                        {
                            Magic.UseAmulet(PlayObject, 1, 2, ref nAmuletIdx);
                            if (M2Share.RandomNumber.Random(BaseObject.m_btAntiPoison + 7) <= 6)
                            {
                                int nPower;
                                switch (StdItem.Shape)
                                {
                                    case 1:
                                        nPower = Magic.GetPower13(40, UserMagic) + Magic.GetRPow(PlayObject.m_WAbil.SC) * 2;// 中毒类型 - 绿毒
                                        BaseObject.SendDelayMsg(PlayObject, Grobal2.RM_POISON, Grobal2.POISON_DECHEALTH, nPower, PlayObject.ObjectId, HUtil32.Round(UserMagic.btLevel / 3 * (nPower / M2Share.g_Config.nAmyOunsulPoint)), "", 1000);
                                        break;
                                    case 2:
                                        nPower = Magic.GetPower13(30, UserMagic) + Magic.GetRPow(PlayObject.m_WAbil.SC) * 2;// 中毒类型 - 红毒
                                        BaseObject.SendDelayMsg(PlayObject, Grobal2.RM_POISON, Grobal2.POISON_DAMAGEARMOR, nPower, PlayObject.ObjectId, HUtil32.Round(UserMagic.btLevel / 3 * (nPower / M2Share.g_Config.nAmyOunsulPoint)), "", 1000);
                                        break;
                                }
                                if (BaseObject.m_btRaceServer == Grobal2.RC_PLAYOBJECT || BaseObject.m_btRaceServer >= Grobal2.RC_ANIMAL)
                                {
                                    result = true;
                                }
                            }
                        }
                        PlayObject.SetTargetCreat(BaseObject);
                    }
                }
            }
            BaseObjectList.Clear();
            BaseObjectList = null;
            return result;
        }

        private bool MagGroupDeDing(TPlayObject PlayObject, TUserMagic UserMagic, int nTargetX, int nTargetY, TBaseObject TargeTBaseObject)
        {
            TBaseObject BaseObject;
            var result = false;
            IList<TBaseObject> BaseObjectList = new List<TBaseObject>();
            PlayObject.GetMapBaseObjects(PlayObject.m_PEnvir, nTargetX, nTargetY, HUtil32._MAX(1, UserMagic.btLevel), BaseObjectList);
            for (var i = 0; i < BaseObjectList.Count; i++)
            {
                BaseObject = BaseObjectList[i];
                if (BaseObject.m_boDeath || BaseObject.m_boGhost || PlayObject == BaseObject)
                {
                    continue;
                }
                if (PlayObject.IsProperTarget(BaseObject))
                {
                    var nPower = PlayObject.GetAttackPower(HUtil32.LoWord(PlayObject.m_WAbil.DC), HUtil32.HiWord(PlayObject.m_WAbil.DC) - HUtil32.LoWord(PlayObject.m_WAbil.DC));
                    if (M2Share.RandomNumber.Random(BaseObject.m_btSpeedPoint) >= PlayObject.m_btHitPoint)
                    {
                        nPower = 0;
                    }
                    if (nPower > 0)
                    {
                        nPower = BaseObject.GetHitStruckDamage(PlayObject, nPower);
                    }
                    if (nPower > 0)
                    {
                        BaseObject.StruckDamage(nPower);
                        PlayObject.SendDelayMsg(PlayObject, Grobal2.RM_DELAYMAGIC, (short)nPower, HUtil32.MakeLong(BaseObject.m_nCurrX, BaseObject.m_nCurrY), 1, BaseObject.ObjectId, "", 200);
                    }
                    if (BaseObject.m_btRaceServer >= Grobal2.RC_ANIMAL)
                    {
                        result = true;
                    }
                }
                PlayObject.SendRefMsg(Grobal2.RM_10205, 0, BaseObject.m_nCurrX, BaseObject.m_nCurrY, 1, "");
            }
            BaseObjectList.Clear();
            BaseObjectList = null;
            return result;
        }

        private bool MagGroupLightening(TPlayObject PlayObject, TUserMagic UserMagic, int nTargetX, int nTargetY, TBaseObject TargeTBaseObject, ref bool boSpellFire)
        {
            var result = false;
            boSpellFire = false;
            IList<TBaseObject> BaseObjectList = new List<TBaseObject>();
            PlayObject.GetMapBaseObjects(PlayObject.m_PEnvir, nTargetX, nTargetY, HUtil32._MAX(1, UserMagic.btLevel), BaseObjectList);
            PlayObject.SendRefMsg(Grobal2.RM_MAGICFIRE, 0, HUtil32.MakeWord(UserMagic.MagicInfo.btEffectType, UserMagic.MagicInfo.btEffect), HUtil32.MakeLong(nTargetX, nTargetY), TargeTBaseObject.ObjectId, "");
            for (var i = 0; i < BaseObjectList.Count; i++)
            {
                var BaseObject = BaseObjectList[i];
                if (BaseObject.m_boDeath || BaseObject.m_boGhost || PlayObject == BaseObject)
                {
                    continue;
                }
                if (PlayObject.IsProperTarget(BaseObject))
                {
                    if (M2Share.RandomNumber.Random(10) >= BaseObject.m_nAntiMagic)
                    {
                        var nPower = PlayObject.GetAttackPower(Magic.GetPower(Magic.MPow(UserMagic), UserMagic) + HUtil32.LoWord(PlayObject.m_WAbil.MC), HUtil32.HiWord(PlayObject.m_WAbil.MC) - HUtil32.LoWord(PlayObject.m_WAbil.MC) + 1);
                        if (BaseObject.m_btLifeAttrib == Grobal2.LA_UNDEAD)
                        {
                            nPower = (ushort)HUtil32.Round(nPower * 1.5);
                        }
                        PlayObject.SendDelayMsg(PlayObject, Grobal2.RM_DELAYMAGIC, (short)nPower, HUtil32.MakeLong(BaseObject.m_nCurrX, BaseObject.m_nCurrY), 2, BaseObject.ObjectId, "", 600);
                        if (BaseObject.m_btRaceServer >= Grobal2.RC_ANIMAL)
                        {
                            result = true;
                        }
                    }
                    if (BaseObject.m_nCurrX != nTargetX || BaseObject.m_nCurrY != nTargetY)
                    {
                        PlayObject.SendRefMsg(Grobal2.RM_10205, 0, BaseObject.m_nCurrX, BaseObject.m_nCurrY, 4, "");
                    }
                }
            }
            BaseObjectList.Clear();
            BaseObjectList = null;
            return result;
        }

        private bool MagHbFireBall(TPlayObject PlayObject, TUserMagic UserMagic, int nTargetX, int nTargetY, ref TBaseObject TargetBaseObject)
        {
            var result = false;
            if (!PlayObject.MagCanHitTarget(PlayObject.m_nCurrX, PlayObject.m_nCurrY, TargetBaseObject))
            {
                TargetBaseObject = null;
                return result;
            }
            if (!PlayObject.IsProperTarget(TargetBaseObject))
            {
                TargetBaseObject = null;
                return result;
            }
            if (TargetBaseObject.m_nAntiMagic > M2Share.RandomNumber.Random(10) || Math.Abs(TargetBaseObject.m_nCurrX - nTargetX) > 1 || Math.Abs(TargetBaseObject.m_nCurrY - nTargetY) > 1)
            {
                TargetBaseObject = null;
                return result;
            }
            var nPower = PlayObject.GetAttackPower(Magic.GetPower(Magic.MPow(UserMagic), UserMagic) + HUtil32.LoWord(PlayObject.m_WAbil.MC), HUtil32.HiWord(PlayObject.m_WAbil.MC) - HUtil32.LoWord(PlayObject.m_WAbil.MC) + 1);
            PlayObject.SendDelayMsg(PlayObject, Grobal2.RM_DELAYMAGIC, (short)nPower, HUtil32.MakeLong(nTargetX, nTargetY), 2, TargetBaseObject.ObjectId, "", 600);
            if (TargetBaseObject.m_btRaceServer >= Grobal2.RC_ANIMAL)
            {
                result = true;
            }
            if (PlayObject.m_Abil.Level > TargetBaseObject.m_Abil.Level && !TargetBaseObject.m_boStickMode)
            {
                var levelgap = PlayObject.m_Abil.Level - TargetBaseObject.m_Abil.Level;
                if (M2Share.RandomNumber.Random(20) < 6 + UserMagic.btLevel * 3 + levelgap)
                {
                    var push = M2Share.RandomNumber.Random(UserMagic.btLevel) - 1;
                    if (push > 0)
                    {
                        int nDir = M2Share.GetNextDirection(PlayObject.m_nCurrX, PlayObject.m_nCurrY, TargetBaseObject.m_nCurrX, TargetBaseObject.m_nCurrY);
                        PlayObject.SendDelayMsg(PlayObject, Grobal2.RM_DELAYPUSHED, (short)nDir, HUtil32.MakeLong(nTargetX, nTargetY), push, TargetBaseObject.ObjectId, "", 600);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 火墙
        /// </summary>
        /// <returns></returns>
        private int MagMakeFireCross(TPlayObject PlayObject, int nDamage, int nHTime, int nX, int nY)
        {
            const string sDisableInSafeZoneFireCross = "安全区不允许使用...";
            if (M2Share.g_Config.boDisableInSafeZoneFireCross && PlayObject.InSafeZone(PlayObject.m_PEnvir, nX, nY))
            {
                PlayObject.SysMsg(sDisableInSafeZoneFireCross, MsgColor.Red, MsgType.Notice);
                return 0;
            }
            if (PlayObject.m_PEnvir.GetEvent(nX, nY - 1) == null)
            {
                FireBurnEvent   FireBurnEvent = new FireBurnEvent(PlayObject, nX, nY - 1, Grobal2.ET_FIRE, nHTime * 1000, nDamage);
                M2Share.EventManager.AddEvent(FireBurnEvent);
            }
            if (PlayObject.m_PEnvir.GetEvent(nX - 1, nY) == null)
            {
                FireBurnEvent  FireBurnEvent = new FireBurnEvent(PlayObject, nX - 1, nY, Grobal2.ET_FIRE, nHTime * 1000, nDamage);
                M2Share.EventManager.AddEvent(FireBurnEvent);
            }
            if (PlayObject.m_PEnvir.GetEvent(nX, nY) == null)
            {
                FireBurnEvent   FireBurnEvent = new FireBurnEvent(PlayObject, nX, nY, Grobal2.ET_FIRE, nHTime * 1000, nDamage);
                M2Share.EventManager.AddEvent(FireBurnEvent);
            }
            if (PlayObject.m_PEnvir.GetEvent(nX + 1, nY) == null)
            {
                FireBurnEvent  FireBurnEvent = new FireBurnEvent(PlayObject, nX + 1, nY, Grobal2.ET_FIRE, nHTime * 1000, nDamage);
                M2Share.EventManager.AddEvent(FireBurnEvent);
            }
            if (PlayObject.m_PEnvir.GetEvent(nX, nY + 1) == null)
            {
                FireBurnEvent  FireBurnEvent = new FireBurnEvent(PlayObject, nX, nY + 1, Grobal2.ET_FIRE, nHTime * 1000, nDamage);
                M2Share.EventManager.AddEvent(FireBurnEvent);
            }
            return 1;
        }

        private bool MagBigExplosion(TBaseObject BaseObject, int nPower, int nX, int nY, int nRage)
        {
            var result = false;
            IList<TBaseObject> BaseObjectList = new List<TBaseObject>();
            BaseObject.GetMapBaseObjects(BaseObject.m_PEnvir, nX, nY, nRage, BaseObjectList);
            for (var i = 0; i < BaseObjectList.Count; i++)
            {
                var TargeTBaseObject = BaseObjectList[i];
                if (BaseObject.IsProperTarget(TargeTBaseObject))
                {
                    BaseObject.SetTargetCreat(TargeTBaseObject);
                    TargeTBaseObject.SendMsg(BaseObject, Grobal2.RM_MAGSTRUCK, 0, nPower, 0, 0, "");
                    result = true;
                }
            }
            BaseObjectList.Clear();
            BaseObjectList = null;
            return result;
        }

        private bool MagElecBlizzard(TBaseObject BaseObject, int nPower)
        {
            var result = false;
            IList<TBaseObject> BaseObjectList = new List<TBaseObject>();
            BaseObject.GetMapBaseObjects(BaseObject.m_PEnvir, BaseObject.m_nCurrX, BaseObject.m_nCurrY, M2Share.g_Config.nElecBlizzardRange, BaseObjectList);
            for (var i = 0; i < BaseObjectList.Count; i++)
            {
                var TargeTBaseObject = BaseObjectList[i];
                int nPowerPoint;
                if (TargeTBaseObject.m_btLifeAttrib != Grobal2.LA_UNDEAD)
                {
                    nPowerPoint = nPower / 10;
                }
                else
                {
                    nPowerPoint = nPower;
                }
                if (BaseObject.IsProperTarget(TargeTBaseObject))
                {
                    TargeTBaseObject.SendMsg(BaseObject, Grobal2.RM_MAGSTRUCK, 0, nPowerPoint, 0, 0, "");
                    result = true;
                }
            }
            BaseObjectList.Clear();
            BaseObjectList = null;
            return result;
        }

        private int MagMakeHolyCurtain(TBaseObject BaseObject, int nPower, short nX, short nY)
        {
            var result = 0;
            if (BaseObject.m_PEnvir.CanWalk(nX, nY, true))
            {
                IList<TBaseObject> BaseObjectList = new List<TBaseObject>();
                MagicEvent MagicEvent = null;
                BaseObject.GetMapBaseObjects(BaseObject.m_PEnvir, nX, nY, 1, BaseObjectList);
                for (var i = 0; i < BaseObjectList.Count; i++)
                {
                    var TargeTBaseObject = BaseObjectList[i];
                    if (TargeTBaseObject.m_btRaceServer >= Grobal2.RC_ANIMAL && M2Share.RandomNumber.Random(4) + (BaseObject.m_Abil.Level - 1) > TargeTBaseObject.m_Abil.Level && TargeTBaseObject.m_Master == null)
                    {
                        TargeTBaseObject.OpenHolySeizeMode(nPower * 1000);
                        if (MagicEvent == null)
                        {
                            MagicEvent = new MagicEvent
                            {
                                BaseObjectList = new List<TBaseObject>(),
                                dwStartTick = HUtil32.GetTickCount(),
                                dwTime = nPower * 1000
                            };
                        }
                        MagicEvent.BaseObjectList.Add(TargeTBaseObject);
                        result++;
                    }
                    else
                    {
                        result = 0;
                    }
                }
                BaseObjectList = null;
                if (result > 0 && MagicEvent != null)
                {
                    var HolyCurtainEvent = new HolyCurtainEvent(BaseObject.m_PEnvir, nX - 1, nY - 2, Grobal2.ET_HOLYCURTAIN, nPower * 1000);
                    M2Share.EventManager.AddEvent(HolyCurtainEvent);
                    MagicEvent.Events[0] = HolyCurtainEvent;
                    HolyCurtainEvent = new HolyCurtainEvent(BaseObject.m_PEnvir, nX + 1, nY - 2, Grobal2.ET_HOLYCURTAIN, nPower * 1000);
                    M2Share.EventManager.AddEvent(HolyCurtainEvent);
                    MagicEvent.Events[1] = HolyCurtainEvent;
                    HolyCurtainEvent = new HolyCurtainEvent(BaseObject.m_PEnvir, nX - 2, nY - 1, Grobal2.ET_HOLYCURTAIN, nPower * 1000);
                    M2Share.EventManager.AddEvent(HolyCurtainEvent);
                    MagicEvent.Events[2] = HolyCurtainEvent;
                    HolyCurtainEvent = new HolyCurtainEvent(BaseObject.m_PEnvir, nX + 2, nY - 1, Grobal2.ET_HOLYCURTAIN, nPower * 1000);
                    M2Share.EventManager.AddEvent(HolyCurtainEvent);
                    MagicEvent.Events[3] = HolyCurtainEvent;
                    HolyCurtainEvent = new HolyCurtainEvent(BaseObject.m_PEnvir, nX - 2, nY + 1, Grobal2.ET_HOLYCURTAIN, nPower * 1000);
                    M2Share.EventManager.AddEvent(HolyCurtainEvent);
                    MagicEvent.Events[4] = HolyCurtainEvent;
                    HolyCurtainEvent = new HolyCurtainEvent(BaseObject.m_PEnvir, nX + 2, nY + 1, Grobal2.ET_HOLYCURTAIN, nPower * 1000);
                    M2Share.EventManager.AddEvent(HolyCurtainEvent);
                    MagicEvent.Events[5] = HolyCurtainEvent;
                    HolyCurtainEvent = new HolyCurtainEvent(BaseObject.m_PEnvir, nX - 1, nY + 2, Grobal2.ET_HOLYCURTAIN, nPower * 1000);
                    M2Share.EventManager.AddEvent(HolyCurtainEvent);
                    MagicEvent.Events[6] = HolyCurtainEvent;
                    HolyCurtainEvent = new HolyCurtainEvent(BaseObject.m_PEnvir, nX + 1, nY + 2, Grobal2.ET_HOLYCURTAIN, nPower * 1000);
                    M2Share.EventManager.AddEvent(HolyCurtainEvent);
                    MagicEvent.Events[7] = HolyCurtainEvent;
                    M2Share.UserEngine.m_MagicEventList.Add(MagicEvent);
                }
                else
                {
                    if (MagicEvent == null) return result;
                    MagicEvent.BaseObjectList = null;
                    MagicEvent = null;
                }
            }
            return result;
        }

        private bool MagMakeGroupTransparent(TBaseObject BaseObject, int nX, int nY, int nHTime)
        {
            var result = false;
            IList<TBaseObject> BaseObjectList = new List<TBaseObject>();
            BaseObject.GetMapBaseObjects(BaseObject.m_PEnvir, nX, nY, 1, BaseObjectList);
            for (var i = 0; i < BaseObjectList.Count; i++)
            {
                var TargeTBaseObject = BaseObjectList[i];
                if (BaseObject.IsProperFriend(TargeTBaseObject))
                {
                    if (TargeTBaseObject.m_wStatusTimeArr[Grobal2.STATE_TRANSPARENT] == 0)
                    {
                        TargeTBaseObject.SendDelayMsg(TargeTBaseObject, Grobal2.RM_TRANSPARENT, 0, nHTime, 0, 0, "", 800);
                        result = true;
                    }
                }
            }
            BaseObjectList.Clear();
            BaseObjectList = null;
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="BaseObject">魔法发起人</param>
        /// <param name="TargeTBaseObject">受攻击角色</param>
        /// <param name="nPower">魔法力大小</param>
        /// <param name="nLevel">技能修炼等级</param>
        /// <param name="nTargetX">目标座标X</param>
        /// <param name="nTargetY">目标座标Y</param>
        /// <returns></returns>
        private bool MabMabe(TBaseObject BaseObject, TBaseObject TargeTBaseObject, int nPower, int nLevel, int nTargetX, int nTargetY)
        {
            var result = false;
            if (BaseObject.MagCanHitTarget(BaseObject.m_nCurrX, BaseObject.m_nCurrY, TargeTBaseObject))
            {
                if (BaseObject.IsProperTarget(TargeTBaseObject))
                {
                    if (TargeTBaseObject.m_nAntiMagic <= M2Share.RandomNumber.Random(10) && Math.Abs(TargeTBaseObject.m_nCurrX - nTargetX) <= 1 && Math.Abs(TargeTBaseObject.m_nCurrY - nTargetY) <= 1)
                    {
                        BaseObject.SendDelayMsg(BaseObject, Grobal2.RM_DELAYMAGIC, (short)(nPower / 3), HUtil32.MakeLong(nTargetX, nTargetY), 2, TargeTBaseObject.ObjectId, "", 600);
                        if (M2Share.RandomNumber.Random(2) + (BaseObject.m_Abil.Level - 1) > TargeTBaseObject.m_Abil.Level)
                        {
                            var nLv = BaseObject.m_Abil.Level - TargeTBaseObject.m_Abil.Level;
                            if (M2Share.RandomNumber.Random(M2Share.g_Config.nMabMabeHitRandRate) < HUtil32._MAX(M2Share.g_Config.nMabMabeHitMinLvLimit, nLevel * 8 - nLevel + 15 + nLv))
                            {
                                if (M2Share.RandomNumber.Random(M2Share.g_Config.nMabMabeHitSucessRate) < nLevel * 2 + 4)
                                {
                                    if (TargeTBaseObject.m_btRaceServer == Grobal2.RC_PLAYOBJECT)
                                    {
                                        BaseObject.SetPKFlag(BaseObject);
                                        BaseObject.SetTargetCreat(TargeTBaseObject);
                                    }
                                    TargeTBaseObject.SetLastHiter(BaseObject);
                                    nPower = TargeTBaseObject.GetMagStruckDamage(BaseObject, nPower);
                                    BaseObject.SendDelayMsg(BaseObject, Grobal2.RM_DELAYMAGIC, (short)nPower, HUtil32.MakeLong(nTargetX, nTargetY), 2, TargeTBaseObject.ObjectId, "", 600);
                                    if (!TargeTBaseObject.m_boUnParalysis)
                                    {
                                        // 中毒类型 - 麻痹
                                        TargeTBaseObject.SendDelayMsg(BaseObject, Grobal2.RM_POISON, Grobal2.POISON_STONE, nPower / M2Share.g_Config.nMabMabeHitMabeTimeRate + M2Share.RandomNumber.Random(nLevel), BaseObject.ObjectId, nLevel, "", 650);
                                    }
                                    result = true;
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }

        private bool MagMakeSinSuSlave(TPlayObject PlayObject, TUserMagic UserMagic)
        {
            var result = false;
            if (!PlayObject.CheckServerMakeSlave())
            {
                var sMonName = M2Share.g_Config.sDragon;
                int nMakelevel = UserMagic.btLevel;
                int nExpLevel = UserMagic.btLevel;
                var nCount = M2Share.g_Config.nDragonCount;
                var dwRoyaltySec = 10 * 24 * 60 * 60;
                for (var i = M2Share.g_Config.DragonArray.GetLowerBound(0); i <= M2Share.g_Config.DragonArray.GetUpperBound(0); i++)
                {
                    if (M2Share.g_Config.DragonArray[i].nHumLevel == 0)
                    {
                        break;
                    }
                    if (PlayObject.m_Abil.Level >= M2Share.g_Config.DragonArray[i].nHumLevel)
                    {
                        sMonName = M2Share.g_Config.DragonArray[i].sMonName;
                        nExpLevel = M2Share.g_Config.DragonArray[i].nLevel;
                        nCount = M2Share.g_Config.DragonArray[i].nCount;
                    }
                }
                if (PlayObject.MakeSlave(sMonName, nMakelevel, nExpLevel, nCount, dwRoyaltySec) != null)
                {
                    result = true;
                }
                else
                {
                    PlayObject.RecallSlave(sMonName);
                }
            }
            return result;
        }

        private bool MagMakeSlave(TPlayObject PlayObject, TUserMagic UserMagic)
        {
            var result = false;
            if (!PlayObject.CheckServerMakeSlave())
            {
                var sMonName = M2Share.g_Config.sSkeleton;
                int nMakeLevel = UserMagic.btLevel;
                int nExpLevel = UserMagic.btLevel;
                var nCount = M2Share.g_Config.nSkeletonCount;
                var dwRoyaltySec = 10 * 24 * 60 * 60;//叛变时间
                for (var i = M2Share.g_Config.SkeletonArray.GetLowerBound(0); i <= M2Share.g_Config.SkeletonArray.GetUpperBound(0); i++)
                {
                    if (M2Share.g_Config.SkeletonArray[i].nHumLevel == 0)
                    {
                        break;
                    }
                    if (PlayObject.m_Abil.Level >= M2Share.g_Config.SkeletonArray[i].nHumLevel)
                    {
                        sMonName = M2Share.g_Config.SkeletonArray[i].sMonName;
                        nExpLevel = M2Share.g_Config.SkeletonArray[i].nLevel;
                        nCount = M2Share.g_Config.SkeletonArray[i].nCount;
                    }
                }
                if (PlayObject.MakeSlave(sMonName, nMakeLevel, nExpLevel, nCount, dwRoyaltySec) != null)
                {
                    result = true;
                }
            }
            return result;
        }

        private bool MagMakeClone(TPlayObject PlayObject, TUserMagic UserMagic)
        {
            var playCloneObject = new TPlayCloneObject(PlayObject);
            return true;
        }

        private bool MagMakeAngelSlave(TPlayObject PlayObject, TUserMagic UserMagic)
        {
            var result = false;
            if (!PlayObject.CheckServerMakeSlave())
            {
                var sMonName = M2Share.g_Config.sAngel;
                int nMakeLevel = UserMagic.btLevel;
                int nExpLevel = UserMagic.btLevel;
                var dwRoyaltySec = 10 * 24 * 60 * 60;
                if (PlayObject.MakeSlave(sMonName, nMakeLevel, nExpLevel, 1, dwRoyaltySec) != null)
                {
                    result = true;
                }
            }
            return result;
        }
    }
}

