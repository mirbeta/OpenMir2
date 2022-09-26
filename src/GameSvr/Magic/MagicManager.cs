using GameSvr.Actor;
using GameSvr.Event.Events;
using GameSvr.Player;
using SystemModule;
using SystemModule.Consts;
using SystemModule.Data;
using SystemModule.Packet.ClientPackets;

namespace GameSvr.Magic
{
    public class MagicManager
    {
        private int MagPushArround(BaseObject PlayObject, int nPushLevel)
        {
            var result = 0;
            for (var i = 0; i < PlayObject.VisibleActors.Count; i++)
            {
                var BaseObject = PlayObject.VisibleActors[i].BaseObject;
                if (Math.Abs(PlayObject.CurrX - BaseObject.CurrX) <= 1 && Math.Abs(PlayObject.CurrY - BaseObject.CurrY) <= 1)
                {
                    if (!BaseObject.Death && BaseObject != PlayObject)
                    {
                        if (PlayObject.Abil.Level > BaseObject.Abil.Level && !BaseObject.StickMode)
                        {
                            var levelgap = PlayObject.Abil.Level - BaseObject.Abil.Level;
                            if (M2Share.RandomNumber.Random(20) < 6 + nPushLevel * 3 + levelgap)
                            {
                                if (PlayObject.IsProperTarget(BaseObject))
                                {
                                    var push = 1 + HUtil32._MAX(0, nPushLevel - 1) + M2Share.RandomNumber.Random(2);
                                    var nDir = M2Share.GetNextDirection(PlayObject.CurrX, PlayObject.CurrY, BaseObject.CurrX, BaseObject.CurrY);
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

        private bool MagBigHealing(BaseObject PlayObject, int nPower, int nX, int nY)
        {
            var result = false;
            IList<BaseObject> BaseObjectList = new List<BaseObject>();
            PlayObject.GetMapBaseObjects(PlayObject.Envir, nX, nY, 1, BaseObjectList);
            for (var i = 0; i < BaseObjectList.Count; i++)
            {
                var BaseObject = BaseObjectList[i];
                if (PlayObject.IsProperFriend(BaseObject))
                {
                    if (BaseObject.WAbil.HP < BaseObject.WAbil.MaxHP)
                    {
                        BaseObject.SendDelayMsg(PlayObject, Grobal2.RM_MAGHEALING, 0, nPower, 0, 0, "", 800);
                        result = true;
                    }
                    if (PlayObject.AbilSeeHealGauge)
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
        /// <returns></returns>
        public bool IsWarrSkill(int wMagIdx)
        {
            var result = false;
            switch (wMagIdx)
            {
                case MagicConst.SKILL_ONESWORD:
                case MagicConst.SKILL_ILKWANG:
                case MagicConst.SKILL_YEDO:
                case MagicConst.SKILL_ERGUM:
                case MagicConst.SKILL_BANWOL:
                case MagicConst.SKILL_FIRESWORD:
                case MagicConst.SKILL_MOOTEBO:
                case MagicConst.SKILL_CROSSMOON:
                case MagicConst.SKILL_TWINBLADE:
                    result = true;
                    break;
            }
            return result;
        }

        private ushort DoSpell_MPow(UserMagic UserMagic)
        {
            return (ushort)(UserMagic.Magic.Power + M2Share.RandomNumber.Random(UserMagic.Magic.MaxPower - UserMagic.Magic.Power));
        }

        private ushort DoSpell_GetPower(UserMagic UserMagic, ushort nPower)
        {
            return (ushort)(HUtil32.Round(nPower / (UserMagic.Magic.TrainLv + 1) * (UserMagic.Level + 1)) + UserMagic.Magic.DefPower +
                            M2Share.RandomNumber.Random(UserMagic.Magic.DefMaxPower - UserMagic.Magic.DefPower));
        }

        private ushort DoSpell_GetPower13(UserMagic UserMagic, int nInt)
        {
            var d10 = nInt / 3.0;
            var d18 = nInt - d10;
            var result = (ushort)HUtil32.Round(d18 / (UserMagic.Magic.TrainLv + 1) * (UserMagic.Level + 1) + d10 + (UserMagic.Magic.DefPower + M2Share.RandomNumber.Random(UserMagic.Magic.DefMaxPower - UserMagic.Magic.DefPower)));
            return result;
        }

        private ushort DoSpell_GetRPow(int wInt)
        {
            ushort result;
            if (HUtil32.HiByte(wInt) > HUtil32.LoByte(wInt))
            {
                result = (ushort)(M2Share.RandomNumber.Random(HUtil32.HiByte(wInt) - HUtil32.LoByte(wInt) + 1) + HUtil32.LoByte(wInt));
            }
            else
            {
                result = HUtil32.LoByte(wInt);
            }
            return result;
        }

        public void DoSpell_sub_4934B4(PlayObject PlayObject)
        {
            if (PlayObject.UseItems[Grobal2.U_ARMRINGL].Dura < 100)
            {
                PlayObject.UseItems[Grobal2.U_ARMRINGL].Dura = 0;
                PlayObject.SendDelItems(PlayObject.UseItems[Grobal2.U_ARMRINGL]);
                PlayObject.UseItems[Grobal2.U_ARMRINGL].Index = 0;
            }
        }

        public bool DoSpell(PlayObject PlayObject, UserMagic UserMagic, short nTargetX, short nTargetY, BaseObject TargeTBaseObject)
        {
            short n14 = 0;
            short n18 = 0;
            int n1C;
            short nAmuletIdx = 0;
            if (IsWarrSkill(UserMagic.MagIdx))
            {
                return false;
            }
            if ((Math.Abs(PlayObject.CurrX - nTargetX) > M2Share.Config.MagicAttackRage) || (Math.Abs(PlayObject.CurrY - nTargetY) > M2Share.Config.MagicAttackRage))
            {
                return false;
            }
            PlayObject.SendRefMsg(Grobal2.RM_SPELL, UserMagic.Magic.Effect, nTargetX, nTargetY, UserMagic.Magic.MagicId, "");
            if (TargeTBaseObject != null && TargeTBaseObject.Death)
            {
                return false;
            }
            var boTrain = false;
            var boSpellFail = false;
            var boSpellFire = true;
            if (PlayObject.m_nSoftVersionDateEx == 0 && PlayObject.m_dwClientTick == 0 && UserMagic.Magic.MagicId > 40)
            {
                return false;
            }
            ushort nPower;
            switch (UserMagic.Magic.MagicId)
            {
                case MagicConst.SKILL_FIREBALL:
                case MagicConst.SKILL_FIREBALL2:
                    if (PlayObject.MagCanHitTarget(PlayObject.CurrX, PlayObject.CurrY, TargeTBaseObject))
                    {
                        if (PlayObject.IsProperTarget(TargeTBaseObject))
                        {
                            if (TargeTBaseObject.AntiMagic <= M2Share.RandomNumber.Random(10) && Math.Abs(TargeTBaseObject.CurrX - nTargetX) <= 1 && Math.Abs(TargeTBaseObject.CurrY - nTargetY) <= 1)
                            {
                                nPower = PlayObject.GetAttackPower(DoSpell_GetPower(UserMagic, DoSpell_MPow(UserMagic)) + HUtil32.LoByte(PlayObject.WAbil.MC), HUtil32.HiByte(PlayObject.WAbil.MC) - HUtil32.LoByte(PlayObject.WAbil.MC) + 1);
                                PlayObject.SendDelayMsg(PlayObject, Grobal2.RM_DELAYMAGIC, nPower, HUtil32.MakeLong(nTargetX, nTargetY), 2, TargeTBaseObject.ActorId, "", 600);
                                if (TargeTBaseObject.Race >= ActorRace.Animal)
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
                case MagicConst.SKILL_HEALLING:
                    if (TargeTBaseObject == null)
                    {
                        TargeTBaseObject = PlayObject;
                        nTargetX = PlayObject.CurrX;
                        nTargetY = PlayObject.CurrY;
                    }
                    if (PlayObject.IsProperFriend(TargeTBaseObject))
                    {
                        nPower = PlayObject.GetAttackPower(DoSpell_GetPower(UserMagic, DoSpell_MPow(UserMagic)) + HUtil32.LoByte(PlayObject.WAbil.SC) * 2, (HUtil32.HiByte(PlayObject.WAbil.SC) - HUtil32.LoByte(PlayObject.WAbil.SC)) * 2 + 1);
                        if (TargeTBaseObject.WAbil.HP < TargeTBaseObject.WAbil.MaxHP)
                        {
                            TargeTBaseObject.SendDelayMsg(PlayObject, Grobal2.RM_MAGHEALING, 0, nPower, 0, 0, "", 800);
                            boTrain = true;
                        }
                        if (PlayObject.AbilSeeHealGauge)
                        {
                            PlayObject.SendMsg(TargeTBaseObject, Grobal2.RM_10414, 0, 0, 0, 0, "");
                        }
                    }
                    break;
                case MagicConst.SKILL_AMYOUNSUL:
                    boSpellFail = true;
                    if (PlayObject.IsProperTarget(TargeTBaseObject))
                    {
                        if (MagicBase.CheckAmulet(PlayObject, 1, 2, ref nAmuletIdx))
                        {
                            var StdItem = M2Share.WorldEngine.GetStdItem(PlayObject.UseItems[nAmuletIdx].Index);
                            if (StdItem != null)
                            {
                                MagicBase.UseAmulet(PlayObject, 1, 2, ref nAmuletIdx);
                                if (M2Share.RandomNumber.Random(TargeTBaseObject.AntiPoison + 7) <= 6)
                                {
                                    switch (StdItem.Shape)
                                    {
                                        case 1:
                                            nPower = (ushort)(DoSpell_GetPower13(UserMagic, 40) + DoSpell_GetRPow(PlayObject.WAbil.SC) * 2);// 中毒类型 - 绿毒
                                            TargeTBaseObject.SendDelayMsg(PlayObject, Grobal2.RM_POISON, StatuStateConst.POISON_DECHEALTH, nPower, PlayObject.ActorId, HUtil32.Round(UserMagic.Level / 3 * (nPower / M2Share.Config.AmyOunsulPoint)), "", 1000);
                                            break;
                                        case 2:
                                            nPower = (ushort)(DoSpell_GetPower13(UserMagic, 30) + DoSpell_GetRPow(PlayObject.WAbil.SC) * 2);// 中毒类型 - 红毒
                                            TargeTBaseObject.SendDelayMsg(PlayObject, Grobal2.RM_POISON, StatuStateConst.POISON_DAMAGEARMOR, nPower, PlayObject.ActorId, HUtil32.Round(UserMagic.Level / 3 * (nPower / M2Share.Config.AmyOunsulPoint)), "", 1000);
                                            break;
                                    }
                                    if (TargeTBaseObject.Race == ActorRace.Play || TargeTBaseObject.Race >= ActorRace.Animal)
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
                case MagicConst.SKILL_FIREWIND:
                    if (MagPushArround(PlayObject, UserMagic.Level) > 0)
                    {
                        boTrain = true;
                    }
                    break;
                case MagicConst.SKILL_FIRE:
                    n1C = M2Share.GetNextDirection(PlayObject.CurrX, PlayObject.CurrY, nTargetX, nTargetY);
                    if (PlayObject.Envir.GetNextPosition(PlayObject.CurrX, PlayObject.CurrY, n1C, 1, ref n14, ref n18))
                    {
                        PlayObject.Envir.GetNextPosition(PlayObject.CurrX, PlayObject.CurrY, n1C, 5, ref nTargetX, ref nTargetY);
                        nPower = PlayObject.GetAttackPower(DoSpell_GetPower(UserMagic, DoSpell_MPow(UserMagic)) + HUtil32.LoByte(PlayObject.WAbil.MC), HUtil32.HiByte(PlayObject.WAbil.MC) - HUtil32.LoByte(PlayObject.WAbil.MC) + 1);
                        if (PlayObject.MagPassThroughMagic(n14, n18, nTargetX, nTargetY, n1C, nPower, false) > 0)
                        {
                            boTrain = true;
                        }
                    }
                    break;
                case MagicConst.SKILL_SHOOTLIGHTEN:
                    n1C = M2Share.GetNextDirection(PlayObject.CurrX, PlayObject.CurrY, nTargetX, nTargetY);
                    if (PlayObject.Envir.GetNextPosition(PlayObject.CurrX, PlayObject.CurrY, n1C, 1, ref n14, ref n18))
                    {
                        PlayObject.Envir.GetNextPosition(PlayObject.CurrX, PlayObject.CurrY, n1C, 8, ref nTargetX, ref nTargetY);
                        nPower = PlayObject.GetAttackPower(DoSpell_GetPower(UserMagic, DoSpell_MPow(UserMagic)) + HUtil32.LoByte(PlayObject.WAbil.MC), (ushort)(HUtil32.HiByte(PlayObject.WAbil.MC) - HUtil32.LoByte(PlayObject.WAbil.MC) + 1));
                        if (PlayObject.MagPassThroughMagic(n14, n18, nTargetX, nTargetY, n1C, nPower, true) > 0)
                        {
                            boTrain = true;
                        }
                    }
                    break;
                case MagicConst.SKILL_LIGHTENING:
                    if (PlayObject.IsProperTarget(TargeTBaseObject))
                    {
                        if (M2Share.RandomNumber.Random(10) >= TargeTBaseObject.AntiMagic)
                        {
                            nPower = PlayObject.GetAttackPower(DoSpell_GetPower(UserMagic, DoSpell_MPow(UserMagic)) + HUtil32.LoByte(PlayObject.WAbil.MC), HUtil32.HiByte(PlayObject.WAbil.MC) - HUtil32.LoByte(PlayObject.WAbil.MC) + 1);
                            if (TargeTBaseObject.LifeAttrib == Grobal2.LA_UNDEAD)
                            {
                                nPower = (ushort)HUtil32.Round(nPower * 1.5);
                            }
                            PlayObject.SendDelayMsg(PlayObject, Grobal2.RM_DELAYMAGIC, nPower, HUtil32.MakeLong(nTargetX, nTargetY), 2, TargeTBaseObject.ActorId, "", 600);
                            if (TargeTBaseObject.Race >= ActorRace.Animal)
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
                case MagicConst.SKILL_FIRECHARM:
                case MagicConst.SKILL_HANGMAJINBUB:
                case MagicConst.SKILL_DEJIWONHO:
                case MagicConst.SKILL_HOLYSHIELD:
                case MagicConst.SKILL_SKELLETON:
                case MagicConst.SKILL_CLOAK:
                case MagicConst.SKILL_BIGCLOAK:
                    boSpellFail = true;
                    if (MagicBase.CheckAmulet(PlayObject, 1, 1, ref nAmuletIdx))
                    {
                        MagicBase.UseAmulet(PlayObject, 1, 1, ref nAmuletIdx);
                        switch (UserMagic.Magic.MagicId)
                        {
                            case MagicConst.SKILL_FIRECHARM:
                                if (PlayObject.MagCanHitTarget(PlayObject.CurrX, PlayObject.CurrY, TargeTBaseObject))
                                {
                                    if (PlayObject.IsProperTarget(TargeTBaseObject))
                                    {
                                        if (M2Share.RandomNumber.Random(10) >= TargeTBaseObject.AntiMagic)
                                        {
                                            if (Math.Abs(TargeTBaseObject.CurrX - nTargetX) <= 1 && Math.Abs(TargeTBaseObject.CurrY - nTargetY) <= 1)
                                            {
                                                nPower = PlayObject.GetAttackPower(DoSpell_GetPower(UserMagic, DoSpell_MPow(UserMagic)) + HUtil32.LoByte(PlayObject.WAbil.SC), HUtil32.HiByte(PlayObject.WAbil.SC) - HUtil32.LoByte(PlayObject.WAbil.SC) + 1);
                                                PlayObject.SendDelayMsg(PlayObject, Grobal2.RM_DELAYMAGIC, nPower, HUtil32.MakeLong(nTargetX, nTargetY), 2, TargeTBaseObject.ActorId, "", 1200);
                                                if (TargeTBaseObject.Race >= ActorRace.Animal)
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
                            case MagicConst.SKILL_HANGMAJINBUB:
                                nPower = PlayObject.GetAttackPower(DoSpell_GetPower13(UserMagic, 60) + HUtil32.LoByte(PlayObject.WAbil.SC) * 10, HUtil32.HiByte(PlayObject.WAbil.SC) - HUtil32.LoByte(PlayObject.WAbil.SC) + 1);
                                if (PlayObject.MagMakeDefenceArea(nTargetX, nTargetY, 3, nPower, 1) > 0)
                                {
                                    boTrain = true;
                                }
                                break;
                            case MagicConst.SKILL_DEJIWONHO:
                                nPower = PlayObject.GetAttackPower(DoSpell_GetPower13(UserMagic, 60) + HUtil32.LoByte(PlayObject.WAbil.SC) * 10, HUtil32.HiByte(PlayObject.WAbil.SC) - HUtil32.LoByte(PlayObject.WAbil.SC) + 1);
                                if (PlayObject.MagMakeDefenceArea(nTargetX, nTargetY, 3, nPower, 0) > 0)
                                {
                                    boTrain = true;
                                }
                                break;
                            case MagicConst.SKILL_HOLYSHIELD:
                                if (MagMakeHolyCurtain(PlayObject, DoSpell_GetPower13(UserMagic, 40) + DoSpell_GetRPow(PlayObject.WAbil.SC) * 3, nTargetX, nTargetY) > 0)
                                {
                                    boTrain = true;
                                }
                                break;
                            case MagicConst.SKILL_SKELLETON:
                                if (MagMakeSlave(PlayObject, UserMagic))
                                {
                                    boTrain = true;
                                }
                                break;
                            case MagicConst.SKILL_CLOAK:
                                if (MagMakePrivateTransparent(PlayObject, (ushort)(DoSpell_GetPower13(UserMagic, 30) + DoSpell_GetRPow(PlayObject.WAbil.SC) * 3)))
                                {
                                    boTrain = true;
                                }
                                break;
                            case MagicConst.SKILL_BIGCLOAK:
                                if (MagMakeGroupTransparent(PlayObject, nTargetX, nTargetY, DoSpell_GetPower13(UserMagic, 30) + DoSpell_GetRPow(PlayObject.WAbil.SC) * 3))
                                {
                                    boTrain = true;
                                }
                                break;
                        }
                        boSpellFail = false;
                    }
                    break;
                case MagicConst.SKILL_TAMMING:
                    if (PlayObject.IsProperTarget(TargeTBaseObject))
                    {
                        if (MagTamming(PlayObject, TargeTBaseObject, nTargetX, nTargetY, UserMagic.Level))
                        {
                            boTrain = true;
                        }
                    }
                    break;
                case MagicConst.SKILL_SPACEMOVE:
                    var targerActors = TargeTBaseObject == null ? 0 : TargeTBaseObject.ActorId;
                    PlayObject.SendRefMsg(Grobal2.RM_MAGICFIRE, 0, HUtil32.MakeWord(UserMagic.Magic.EffectType, UserMagic.Magic.Effect), HUtil32.MakeLong(nTargetX, nTargetY), targerActors, "");
                    boSpellFire = false;
                    if (MagSaceMove(PlayObject, UserMagic.Level))
                    {
                        boTrain = true;
                    }
                    break;
                case MagicConst.SKILL_EARTHFIRE:
                    if (MagMakeFireCross(PlayObject, PlayObject.GetAttackPower(DoSpell_GetPower(UserMagic, DoSpell_MPow(UserMagic)) + HUtil32.LoByte(PlayObject.WAbil.MC), HUtil32.HiByte(PlayObject.WAbil.MC) - HUtil32.LoByte(PlayObject.WAbil.MC) + 1), (ushort)(DoSpell_GetPower(UserMagic, 10) + (DoSpell_GetRPow(PlayObject.WAbil.MC) >> 1)), nTargetX, nTargetY))
                    {
                        boTrain = true;
                    }
                    break;
                case MagicConst.SKILL_FIREBOOM:
                    if (MagBigExplosion(PlayObject, PlayObject.GetAttackPower(DoSpell_GetPower(UserMagic, DoSpell_MPow(UserMagic)) + HUtil32.LoByte(PlayObject.WAbil.MC), HUtil32.HiByte(PlayObject.WAbil.MC) - HUtil32.LoByte(PlayObject.WAbil.MC) + 1), nTargetX, nTargetY, M2Share.Config.FireBoomRage))
                    {
                        boTrain = true;
                    }
                    break;
                case MagicConst.SKILL_LIGHTFLOWER:
                    if (MagElecBlizzard(PlayObject, PlayObject.GetAttackPower(DoSpell_GetPower(UserMagic, DoSpell_MPow(UserMagic)) + HUtil32.LoByte(PlayObject.WAbil.MC),
                        HUtil32.HiByte(PlayObject.WAbil.MC) - HUtil32.LoByte(PlayObject.WAbil.MC) + 1)))
                    {
                        boTrain = true;
                    }
                    break;
                case MagicConst.SKILL_SHOWHP:
                    if (TargeTBaseObject != null && !TargeTBaseObject.ShowHp)
                    {
                        if (M2Share.RandomNumber.Random(6) <= UserMagic.Level + 3)
                        {
                            TargeTBaseObject.ShowHpTick = HUtil32.GetTickCount();
                            TargeTBaseObject.ShowHpInterval = DoSpell_GetPower13(UserMagic, DoSpell_GetRPow(PlayObject.WAbil.SC) * 2 + 30) * 1000;
                            TargeTBaseObject.SendDelayMsg(TargeTBaseObject, Grobal2.RM_DOOPENHEALTH, 0, 0, 0, 0, "", 1500);
                            boTrain = true;
                        }
                    }
                    break;
                case MagicConst.SKILL_BIGHEALLING:
                    nPower = PlayObject.GetAttackPower(DoSpell_GetPower(UserMagic, DoSpell_MPow(UserMagic)) + HUtil32.LoByte(PlayObject.WAbil.SC) * 2, (HUtil32.HiByte(PlayObject.WAbil.SC) - HUtil32.LoByte(PlayObject.WAbil.SC)) * 2 + 1);
                    if (MagBigHealing(PlayObject, nPower, nTargetX, nTargetY))
                    {
                        boTrain = true;
                    }
                    break;
                case MagicConst.SKILL_SINSU:
                    boSpellFail = true;
                    if (MagicBase.CheckAmulet(PlayObject, 5, 1, ref nAmuletIdx))
                    {
                        MagicBase.UseAmulet(PlayObject, 5, 1, ref nAmuletIdx);
                        if (MagMakeSinSuSlave(PlayObject, UserMagic))
                        {
                            boTrain = true;
                        }
                        boSpellFail = false;
                    }
                    break;
                case MagicConst.SKILL_ANGEL:
                    boSpellFail = true;
                    if (MagicBase.CheckAmulet(PlayObject, 2, 1, ref nAmuletIdx))
                    {
                        MagicBase.UseAmulet(PlayObject, 2, 1, ref nAmuletIdx);
                        if (MagMakeAngelSlave(PlayObject, UserMagic))
                        {
                            boTrain = true;
                        }
                        boSpellFail = false;
                    }
                    break;
                case MagicConst.SKILL_SHIELD:
                    if (PlayObject.MagBubbleDefenceUp(UserMagic.Level, DoSpell_GetPower(UserMagic, (ushort)(DoSpell_GetRPow(PlayObject.WAbil.MC) + 15))))
                    {
                        boTrain = true;
                    }
                    break;
                case MagicConst.SKILL_KILLUNDEAD:
                    if (PlayObject.IsProperTarget(TargeTBaseObject))
                    {
                        if (MagTurnUndead(PlayObject, TargeTBaseObject, nTargetX, nTargetY, UserMagic.Level))
                        {
                            boTrain = true;
                        }
                    }
                    break;
                case MagicConst.SKILL_SNOWWIND:
                    if (MagBigExplosion(PlayObject, PlayObject.GetAttackPower(DoSpell_GetPower(UserMagic, DoSpell_MPow(UserMagic)) + HUtil32.LoByte(PlayObject.WAbil.MC), HUtil32.HiByte(PlayObject.WAbil.MC) - HUtil32.LoByte(PlayObject.WAbil.MC) + 1), nTargetX, nTargetY, M2Share.Config.SnowWindRange))
                    {
                        boTrain = true;
                    }
                    break;
                case MagicConst.SKILL_UNAMYOUNSUL:
                    if (TargeTBaseObject == null)
                    {
                        TargeTBaseObject = PlayObject;
                        nTargetX = PlayObject.CurrX;
                        nTargetY = PlayObject.CurrY;
                    }
                    if (PlayObject.IsProperFriend(TargeTBaseObject))
                    {
                        if (M2Share.RandomNumber.Random(7) - (UserMagic.Level + 1) < 0)
                        {
                            if (TargeTBaseObject.StatusArr[StatuStateConst.POISON_DECHEALTH] != 0)
                            {
                                TargeTBaseObject.StatusArr[StatuStateConst.POISON_DECHEALTH] = 1;
                                boTrain = true;
                            }
                            if (TargeTBaseObject.StatusArr[StatuStateConst.POISON_DAMAGEARMOR] != 0)
                            {
                                TargeTBaseObject.StatusArr[StatuStateConst.POISON_DAMAGEARMOR] = 1;
                                boTrain = true;
                            }
                            if (TargeTBaseObject.StatusArr[StatuStateConst.POISON_STONE] != 0)
                            {
                                TargeTBaseObject.StatusArr[StatuStateConst.POISON_STONE] = 1;
                                boTrain = true;
                            }
                        }
                    }
                    break;
                case MagicConst.SKILL_WINDTEBO:
                    if (MagWindTebo(PlayObject, UserMagic))
                    {
                        boTrain = true;
                    }
                    break;
                case MagicConst.SKILL_MABE:
                    nPower = PlayObject.GetAttackPower(DoSpell_GetPower(UserMagic, DoSpell_MPow(UserMagic)) + HUtil32.LoByte(PlayObject.WAbil.MC), HUtil32.HiByte(PlayObject.WAbil.MC) - HUtil32.LoByte(PlayObject.WAbil.MC) + 1);
                    if (MabMabe(PlayObject, TargeTBaseObject, nPower, UserMagic.Level, nTargetX, nTargetY))
                    {
                        boTrain = true;
                    }
                    break;
                case MagicConst.SKILL_GROUPLIGHTENING:
                    if (MagGroupLightening(PlayObject, UserMagic, nTargetX, nTargetY, TargeTBaseObject, ref boSpellFire))
                    {
                        boTrain = true;
                    }
                    break;
                case MagicConst.SKILL_GROUPAMYOUNSUL:
                    if (MagGroupAmyounsul(PlayObject, UserMagic, nTargetX, nTargetY, TargeTBaseObject))
                    {
                        boTrain = true;
                    }
                    break;
                case MagicConst.SKILL_GROUPDEDING:
                    if (MagGroupDeDing(PlayObject, UserMagic, nTargetX, nTargetY, TargeTBaseObject))
                    {
                        boTrain = true;
                    }
                    break;
                case MagicConst.SKILL_43:
                    break;
                case MagicConst.SKILL_44:
                    if (MagHbFireBall(PlayObject, UserMagic, nTargetX, nTargetY, ref TargeTBaseObject))
                    {
                        boTrain = true;
                    }
                    break;
                case MagicConst.SKILL_45:
                    if (PlayObject.IsProperTarget(TargeTBaseObject))
                    {
                        if (M2Share.RandomNumber.Random(10) >= TargeTBaseObject.AntiMagic)
                        {
                            nPower = PlayObject.GetAttackPower(DoSpell_GetPower(UserMagic, DoSpell_MPow(UserMagic)) + HUtil32.LoByte(PlayObject.WAbil.MC), HUtil32.HiByte(PlayObject.WAbil.MC) - HUtil32.LoByte(PlayObject.WAbil.MC) + 1);
                            if (TargeTBaseObject.LifeAttrib == Grobal2.LA_UNDEAD)
                            {
                                nPower = (ushort)HUtil32.Round(nPower * 1.5);
                            }
                            PlayObject.SendDelayMsg(PlayObject, Grobal2.RM_DELAYMAGIC, nPower, HUtil32.MakeLong(nTargetX, nTargetY), 2, TargeTBaseObject.ActorId, "", 600);
                            if (TargeTBaseObject.Race >= ActorRace.Animal)
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
                case MagicConst.SKILL_46:
                    if (MagMakeClone(PlayObject, UserMagic))
                    {
                        boTrain = true;
                    }
                    break;
                case MagicConst.SKILL_47:
                    if (MagBigExplosion(PlayObject, PlayObject.GetAttackPower(DoSpell_GetPower(UserMagic, DoSpell_MPow(UserMagic)) + HUtil32.LoByte(PlayObject.WAbil.MC), HUtil32.HiByte(PlayObject.WAbil.MC) - HUtil32.LoByte(PlayObject.WAbil.MC) + 1), nTargetX, nTargetY, M2Share.Config.FireBoomRage))
                    {
                        boTrain = true;
                    }
                    break;
                case MagicConst.SKILL_ENERGYREPULSOR:
                    if (MagPushArround(PlayObject, UserMagic.Level) > 0)
                    {
                        boTrain = true;
                    }
                    break;
                case MagicConst.SKILL_49:
                    boTrain = true;
                    break;
                case MagicConst.SKILL_UENHANCER:
                    boSpellFail = true;
                    if (TargeTBaseObject == null)
                    {
                        TargeTBaseObject = PlayObject;
                        nTargetX = PlayObject.CurrX;
                        nTargetY = PlayObject.CurrY;
                    }
                    if (PlayObject.IsProperFriend(TargeTBaseObject))
                    {
                        if (MagicBase.CheckAmulet(PlayObject, 1, 1, ref nAmuletIdx))
                        {
                            MagicBase.UseAmulet(PlayObject, 1, 1, ref nAmuletIdx);
                            nPower = (ushort)(UserMagic.Level + 1 + M2Share.RandomNumber.Random(UserMagic.Level));
                            n14 = (short)PlayObject.GetAttackPower(DoSpell_GetPower13(UserMagic, 60) + HUtil32.LoByte(PlayObject.WAbil.SC) * 10, HUtil32.HiByte(PlayObject.WAbil.SC) - HUtil32.LoByte(PlayObject.WAbil.SC) + 1);
                            if (TargeTBaseObject.AttPowerUp(nPower, n14))
                            {
                                boTrain = true;
                            }
                            boSpellFail = false;
                        }
                    }
                    break;
                case MagicConst.SKILL_51:// 灵魂召唤术
                    boTrain = true;
                    break;
                case MagicConst.SKILL_52:// 诅咒术
                    boTrain = true;
                    break;
                case MagicConst.SKILL_53:// 灵魂召唤术
                    boTrain = true;
                    break;
                case MagicConst.SKILL_54:// 诅咒术
                    boTrain = true;
                    break;
            }
            if (boSpellFail)
            {
                return false;
            }
            if (boSpellFire)
            {
                if (TargeTBaseObject == null)
                {
                    PlayObject.SendRefMsg(Grobal2.RM_MAGICFIRE, 0, HUtil32.MakeWord(UserMagic.Magic.EffectType, UserMagic.Magic.Effect), HUtil32.MakeLong(nTargetX, nTargetY), 0, "");
                }
                else
                {
                    PlayObject.SendRefMsg(Grobal2.RM_MAGICFIRE, 0, HUtil32.MakeWord(UserMagic.Magic.EffectType, UserMagic.Magic.Effect), HUtil32.MakeLong(nTargetX, nTargetY), TargeTBaseObject.ActorId, "");
                }
            }
            if (UserMagic.Level < 3 && boTrain)
            {
                if (UserMagic.Magic.TrainLevel[UserMagic.Level] <= PlayObject.Abil.Level)
                {
                    PlayObject.TrainSkill(UserMagic, M2Share.RandomNumber.Random(3) + 1);
                    if (!PlayObject.CheckMagicLevelup(UserMagic))
                    {
                        PlayObject.SendDelayMsg(PlayObject, Grobal2.RM_MAGIC_LVEXP, 0, UserMagic.Magic.MagicId, UserMagic.Level, UserMagic.TranPoint, "", 1000);
                    }
                }
            }
            return true;
        }

        public bool MagMakePrivateTransparent(BaseObject BaseObject, ushort nHTime)
        {
            if (BaseObject.StatusArr[StatuStateConst.STATE_TRANSPARENT] > 0)
            {
                return false;
            }
            IList<BaseObject> BaseObjectList = new List<BaseObject>();
            BaseObject.GetMapBaseObjects(BaseObject.Envir, BaseObject.CurrX, BaseObject.CurrY, 9, BaseObjectList);
            for (var i = 0; i < BaseObjectList.Count; i++)
            {
                var TargeTBaseObject = BaseObjectList[i];
                if (TargeTBaseObject.Race >= ActorRace.Animal && TargeTBaseObject.TargetCret == BaseObject)
                {
                    if (Math.Abs(TargeTBaseObject.CurrX - BaseObject.CurrX) > 1 || Math.Abs(TargeTBaseObject.CurrY - BaseObject.CurrY) > 1 || M2Share.RandomNumber.Random(2) == 0)
                    {
                        TargeTBaseObject.TargetCret = null;
                    }
                }
            }
            BaseObjectList.Clear();
            BaseObject.StatusArr[StatuStateConst.STATE_TRANSPARENT] = nHTime;
            BaseObject.CharStatus = BaseObject.GetCharStatus();
            BaseObject.StatusChanged();
            BaseObject.HideMode = true;
            BaseObject.Transparent = true;
            return true;
        }

        private bool MagTamming(BaseObject BaseObject, BaseObject TargeTBaseObject, int nTargetX, int nTargetY, int nMagicLevel)
        {
            var result = false;
            if (TargeTBaseObject.Race != ActorRace.Play && M2Share.RandomNumber.Random(4 - nMagicLevel) == 0)
            {
                TargeTBaseObject.TargetCret = null;
                if (TargeTBaseObject.Master == BaseObject)
                {
                    TargeTBaseObject.OpenHolySeizeMode((nMagicLevel * 5 + 10) * 1000);
                    result = true;
                }
                else
                {
                    if (M2Share.RandomNumber.Random(2) == 0)
                    {
                        if (TargeTBaseObject.Abil.Level <= BaseObject.Abil.Level + 2)
                        {
                            if (M2Share.RandomNumber.Random(3) == 0)
                            {
                                if (M2Share.RandomNumber.Random(BaseObject.Abil.Level + 20 + nMagicLevel * 5) > TargeTBaseObject.Abil.Level + M2Share.Config.MagTammingTargetLevel)
                                {
                                    if (!TargeTBaseObject.NoTame && TargeTBaseObject.LifeAttrib != Grobal2.LA_UNDEAD && TargeTBaseObject.Abil.Level < M2Share.Config.MagTammingLevel && BaseObject.SlaveList.Count < M2Share.Config.MagTammingCount)
                                    {
                                        int n14 = TargeTBaseObject.Abil.MaxHP / M2Share.Config.MagTammingHPRate;
                                        if (n14 <= 2)
                                        {
                                            n14 = 2;
                                        }
                                        else
                                        {
                                            n14 += n14;
                                        }
                                        if (TargeTBaseObject.Master != BaseObject && M2Share.RandomNumber.Random(n14) == 0)
                                        {
                                            TargeTBaseObject.BreakCrazyMode();
                                            if (TargeTBaseObject.Master != null)
                                            {
                                                TargeTBaseObject.WAbil.HP = (ushort)(TargeTBaseObject.WAbil.HP / 10);
                                            }

                                            if (TargeTBaseObject.CanReAlive && TargeTBaseObject.Master == null)
                                            {
                                                TargeTBaseObject.CanReAlive = false;
                                                if (TargeTBaseObject.MonGen != null)
                                                {
                                                    if (TargeTBaseObject.MonGen.ActiveCount > 0)
                                                    {
                                                        TargeTBaseObject.MonGen.ActiveCount--;
                                                    }
                                                    else
                                                    {
                                                        TargeTBaseObject.MonGen = null;
                                                    }
                                                }
                                            }
                                            TargeTBaseObject.Master = BaseObject;
                                            TargeTBaseObject.MasterRoyaltyTick = (M2Share.RandomNumber.Random(BaseObject.Abil.Level * 2) + (nMagicLevel << 2) * 5 + 20) * 60 * 1000 + HUtil32.GetTickCount();
                                            TargeTBaseObject.SlaveMakeLevel = (byte)nMagicLevel;
                                            if (TargeTBaseObject.MasterTick == 0)
                                            {
                                                TargeTBaseObject.MasterTick = HUtil32.GetTickCount();
                                            }
                                            TargeTBaseObject.BreakHolySeizeMode();
                                            if (1500 - nMagicLevel * 200 < TargeTBaseObject.WalkSpeed)
                                            {
                                                TargeTBaseObject.WalkSpeed = 1500 - nMagicLevel * 200;
                                            }
                                            if (2000 - nMagicLevel * 200 < TargeTBaseObject.NextHitTime)
                                            {
                                                TargeTBaseObject.NextHitTime = 2000 - nMagicLevel * 200;
                                            }
                                            TargeTBaseObject.RefShowName();
                                            BaseObject.SlaveList.Add(TargeTBaseObject);
                                        }
                                        else
                                        {
                                            if (M2Share.RandomNumber.Random(14) == 0)
                                            {
                                                TargeTBaseObject.WAbil.HP = 0;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (TargeTBaseObject.LifeAttrib == Grobal2.LA_UNDEAD && M2Share.RandomNumber.Random(20) == 0)
                                        {
                                            TargeTBaseObject.WAbil.HP = 0;
                                        }
                                    }
                                }
                                else
                                {
                                    if (TargeTBaseObject.LifeAttrib != Grobal2.LA_UNDEAD && M2Share.RandomNumber.Random(20) == 0)
                                    {
                                        TargeTBaseObject.OpenCrazyMode(M2Share.RandomNumber.Random(20) + 10);
                                    }
                                }
                            }
                            else
                            {
                                if (TargeTBaseObject.LifeAttrib != Grobal2.LA_UNDEAD)
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

        private bool MagTurnUndead(BaseObject BaseObject, BaseObject TargeTBaseObject, int nTargetX, int nTargetY, int nLevel)
        {
            var result = false;
            if (TargeTBaseObject.SuperMan || TargeTBaseObject.LifeAttrib != Grobal2.LA_UNDEAD)
            {
                return result;
            }
            ((AnimalObject)TargeTBaseObject).Struck(BaseObject);
            if (TargeTBaseObject.TargetCret == null)
            {
                ((AnimalObject)TargeTBaseObject).m_boRunAwayMode = true;
                ((AnimalObject)TargeTBaseObject).m_dwRunAwayStart = HUtil32.GetTickCount();
                ((AnimalObject)TargeTBaseObject).m_dwRunAwayTime = 10 * 1000;
            }
            BaseObject.SetTargetCreat(TargeTBaseObject);
            if (M2Share.RandomNumber.Random(2) + (BaseObject.Abil.Level - 1) > TargeTBaseObject.Abil.Level)
            {
                if (TargeTBaseObject.Abil.Level < M2Share.Config.MagTurnUndeadLevel)
                {
                    var n14 = BaseObject.Abil.Level - TargeTBaseObject.Abil.Level;
                    if (M2Share.RandomNumber.Random(100) < (nLevel << 3) - nLevel + 15 + n14)
                    {
                        TargeTBaseObject.SetLastHiter(BaseObject);
                        TargeTBaseObject.WAbil.HP = 0;
                        result = true;
                    }
                }
            }
            return result;
        }

        private bool MagWindTebo(PlayObject PlayObject, UserMagic UserMagic)
        {
            var result = false;
            var PoseBaseObject = PlayObject.GetPoseCreate();
            if (PoseBaseObject != null && PoseBaseObject != PlayObject && !PoseBaseObject.Death && !PoseBaseObject.Ghost && PlayObject.IsProperTarget(PoseBaseObject) && !PoseBaseObject.StickMode)
            {
                if (Math.Abs(PlayObject.CurrX - PoseBaseObject.CurrX) <= 1 && Math.Abs(PlayObject.CurrY - PoseBaseObject.CurrY) <= 1 && PlayObject.Abil.Level > PoseBaseObject.Abil.Level)
                {
                    if (M2Share.RandomNumber.Random(20) < UserMagic.Level * 6 + 6 + (PlayObject.Abil.Level - PoseBaseObject.Abil.Level))
                    {
                        PoseBaseObject.CharPushed(M2Share.GetNextDirection(PlayObject.CurrX, PlayObject.CurrY, PoseBaseObject.CurrX, PoseBaseObject.CurrY), HUtil32._MAX(0, UserMagic.Level - 1) + 1);
                        result = true;
                    }
                }
            }
            return result;
        }

        private bool MagSaceMove(PlayObject playObject, int nLevel)
        {
            var result = false;
            if (M2Share.RandomNumber.Random(11) < nLevel * 2 + 4)
            {
                playObject.SendRefMsg(Grobal2.RM_SPACEMOVE_FIRE2, 0, 0, 0, 0, "");
                var Envir = playObject.Envir;
                playObject.MapRandomMove(playObject.HomeMap, 1);
                if (Envir != playObject.Envir && playObject.Race == ActorRace.Play)
                {
                    playObject.m_boTimeRecall = false;
                }
                result = true;
            }
            return result;
        }

        private bool MagGroupAmyounsul(PlayObject PlayObject, UserMagic UserMagic, int nTargetX, int nTargetY, BaseObject TargeTBaseObject)
        {
            short nAmuletIdx = 0;
            var result = false;
            IList<BaseObject> BaseObjectList = new List<BaseObject>();
            PlayObject.GetMapBaseObjects(PlayObject.Envir, nTargetX, nTargetY, HUtil32._MAX(1, UserMagic.Level), BaseObjectList);
            for (var i = 0; i < BaseObjectList.Count; i++)
            {
                var BaseObject = BaseObjectList[i];
                if (BaseObject.Death || BaseObject.Ghost || PlayObject == BaseObject)
                {
                    continue;
                }
                if (PlayObject.IsProperTarget(BaseObject))
                {
                    if (MagicBase.CheckAmulet(PlayObject, 1, 2, ref nAmuletIdx))
                    {
                        var StdItem = M2Share.WorldEngine.GetStdItem(PlayObject.UseItems[nAmuletIdx].Index);
                        if (StdItem != null)
                        {
                            MagicBase.UseAmulet(PlayObject, 1, 2, ref nAmuletIdx);
                            if (M2Share.RandomNumber.Random(BaseObject.AntiPoison + 7) <= 6)
                            {
                                int nPower;
                                switch (StdItem.Shape)
                                {
                                    case 1:
                                        nPower = MagicBase.GetPower13(40, UserMagic) + MagicBase.GetRPow(PlayObject.WAbil.SC) * 2;// 中毒类型 - 绿毒
                                        BaseObject.SendDelayMsg(PlayObject, Grobal2.RM_POISON, StatuStateConst.POISON_DECHEALTH, nPower, PlayObject.ActorId, HUtil32.Round(UserMagic.Level / 3 * (nPower / M2Share.Config.AmyOunsulPoint)), "", 1000);
                                        break;
                                    case 2:
                                        nPower = MagicBase.GetPower13(30, UserMagic) + MagicBase.GetRPow(PlayObject.WAbil.SC) * 2;// 中毒类型 - 红毒
                                        BaseObject.SendDelayMsg(PlayObject, Grobal2.RM_POISON, StatuStateConst.POISON_DAMAGEARMOR, nPower, PlayObject.ActorId, HUtil32.Round(UserMagic.Level / 3 * (nPower / M2Share.Config.AmyOunsulPoint)), "", 1000);
                                        break;
                                }
                                if (BaseObject.Race == ActorRace.Play || BaseObject.Race >= ActorRace.Animal)
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
            return result;
        }

        private bool MagGroupDeDing(PlayObject PlayObject, UserMagic UserMagic, int nTargetX, int nTargetY, BaseObject TargeTBaseObject)
        {
            BaseObject BaseObject;
            var result = false;
            IList<BaseObject> BaseObjectList = new List<BaseObject>();
            PlayObject.GetMapBaseObjects(PlayObject.Envir, nTargetX, nTargetY, HUtil32._MAX(1, UserMagic.Level), BaseObjectList);
            for (var i = 0; i < BaseObjectList.Count; i++)
            {
                BaseObject = BaseObjectList[i];
                if (BaseObject.Death || BaseObject.Ghost || PlayObject == BaseObject)
                {
                    continue;
                }
                if (PlayObject.IsProperTarget(BaseObject))
                {
                    var nPower = PlayObject.GetAttackPower(HUtil32.LoByte(PlayObject.WAbil.DC), HUtil32.HiByte(PlayObject.WAbil.DC) - HUtil32.LoByte(PlayObject.WAbil.DC));
                    if (M2Share.RandomNumber.Random(BaseObject.SpeedPoint) >= PlayObject.HitPoint)
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
                        PlayObject.SendDelayMsg(PlayObject, Grobal2.RM_DELAYMAGIC, nPower, HUtil32.MakeLong(BaseObject.CurrX, BaseObject.CurrY), 1, BaseObject.ActorId, "", 200);
                    }
                    if (BaseObject.Race >= ActorRace.Animal)
                    {
                        result = true;
                    }
                }
                PlayObject.SendRefMsg(Grobal2.RM_10205, 0, BaseObject.CurrX, BaseObject.CurrY, 1, "");
            }
            BaseObjectList.Clear();
            return result;
        }

        private bool MagGroupLightening(PlayObject PlayObject, UserMagic UserMagic, int nTargetX, int nTargetY, BaseObject TargeTBaseObject, ref bool boSpellFire)
        {
            var result = false;
            boSpellFire = false;
            IList<BaseObject> BaseObjectList = new List<BaseObject>();
            PlayObject.GetMapBaseObjects(PlayObject.Envir, nTargetX, nTargetY, HUtil32._MAX(1, UserMagic.Level), BaseObjectList);
            PlayObject.SendRefMsg(Grobal2.RM_MAGICFIRE, 0, HUtil32.MakeWord(UserMagic.Magic.EffectType, UserMagic.Magic.Effect), HUtil32.MakeLong(nTargetX, nTargetY), TargeTBaseObject.ActorId, "");
            for (var i = 0; i < BaseObjectList.Count; i++)
            {
                var BaseObject = BaseObjectList[i];
                if (BaseObject.Death || BaseObject.Ghost || PlayObject == BaseObject)
                {
                    continue;
                }
                if (PlayObject.IsProperTarget(BaseObject))
                {
                    if (M2Share.RandomNumber.Random(10) >= BaseObject.AntiMagic)
                    {
                        var nPower = PlayObject.GetAttackPower(MagicBase.GetPower(MagicBase.MPow(UserMagic), UserMagic) + HUtil32.LoByte(PlayObject.WAbil.MC), HUtil32.HiByte(PlayObject.WAbil.MC) - HUtil32.LoByte(PlayObject.WAbil.MC) + 1);
                        if (BaseObject.LifeAttrib == Grobal2.LA_UNDEAD)
                        {
                            nPower = (ushort)HUtil32.Round(nPower * 1.5);
                        }
                        PlayObject.SendDelayMsg(PlayObject, Grobal2.RM_DELAYMAGIC, nPower, HUtil32.MakeLong(BaseObject.CurrX, BaseObject.CurrY), 2, BaseObject.ActorId, "", 600);
                        if (BaseObject.Race >= ActorRace.Animal)
                        {
                            result = true;
                        }
                    }
                    if (BaseObject.CurrX != nTargetX || BaseObject.CurrY != nTargetY)
                    {
                        PlayObject.SendRefMsg(Grobal2.RM_10205, 0, BaseObject.CurrX, BaseObject.CurrY, 4, "");
                    }
                }
            }
            BaseObjectList.Clear();
            return result;
        }

        private bool MagHbFireBall(PlayObject PlayObject, UserMagic UserMagic, short nTargetX, short nTargetY, ref BaseObject TargetBaseObject)
        {
            var result = false;
            if (!PlayObject.MagCanHitTarget(PlayObject.CurrX, PlayObject.CurrY, TargetBaseObject))
            {
                TargetBaseObject = null;
                return false;
            }
            if (!PlayObject.IsProperTarget(TargetBaseObject))
            {
                TargetBaseObject = null;
                return false;
            }
            if (TargetBaseObject.AntiMagic > M2Share.RandomNumber.Random(10) || Math.Abs(TargetBaseObject.CurrX - nTargetX) > 1 || Math.Abs(TargetBaseObject.CurrY - nTargetY) > 1)
            {
                TargetBaseObject = null;
                return false;
            }
            var nPower = PlayObject.GetAttackPower(MagicBase.GetPower(MagicBase.MPow(UserMagic), UserMagic) + HUtil32.LoByte(PlayObject.WAbil.MC), HUtil32.HiByte(PlayObject.WAbil.MC) - HUtil32.LoByte(PlayObject.WAbil.MC) + 1);
            PlayObject.SendDelayMsg(PlayObject, Grobal2.RM_DELAYMAGIC, nPower, HUtil32.MakeLong(nTargetX, nTargetY), 2, TargetBaseObject.ActorId, "", 600);
            if (TargetBaseObject.Race >= ActorRace.Animal)
            {
                result = true;
            }
            if (PlayObject.Abil.Level > TargetBaseObject.Abil.Level && !TargetBaseObject.StickMode)
            {
                var levelgap = PlayObject.Abil.Level - TargetBaseObject.Abil.Level;
                if (M2Share.RandomNumber.Random(20) < 6 + UserMagic.Level * 3 + levelgap)
                {
                    var push = M2Share.RandomNumber.Random(UserMagic.Level) - 1;
                    if (push > 0)
                    {
                        byte nDir = M2Share.GetNextDirection(PlayObject.CurrX, PlayObject.CurrY, TargetBaseObject.CurrX, TargetBaseObject.CurrY);
                        PlayObject.SendDelayMsg(PlayObject, Grobal2.RM_DELAYPUSHED, nDir, HUtil32.MakeLong(nTargetX, nTargetY), push, TargetBaseObject.ActorId, "", 600);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// 火墙
        /// </summary>
        /// <returns></returns>
        private bool MagMakeFireCross(PlayObject PlayObject, int nDamage, ushort time, short nX, short nY)
        {
            const string sDisableInSafeZoneFireCross = "安全区不允许使用...";
            if (M2Share.Config.DisableInSafeZoneFireCross && PlayObject.InSafeZone(PlayObject.Envir, nX, nY))
            {
                PlayObject.SysMsg(sDisableInSafeZoneFireCross, MsgColor.Red, MsgType.Notice);
                return false;
            }
            if (PlayObject.Envir.GetEvent(nX, nY - 1) == null)
            {
                FireBurnEvent FireBurnEvent = new FireBurnEvent(PlayObject, nX, nY - 1, Grobal2.ET_FIRE, time * 1000, nDamage);
                M2Share.EventMgr.AddEvent(FireBurnEvent);
            }
            if (PlayObject.Envir.GetEvent(nX - 1, nY) == null)
            {
                FireBurnEvent FireBurnEvent = new FireBurnEvent(PlayObject, nX - 1, nY, Grobal2.ET_FIRE, time * 1000, nDamage);
                M2Share.EventMgr.AddEvent(FireBurnEvent);
            }
            if (PlayObject.Envir.GetEvent(nX, nY) == null)
            {
                FireBurnEvent FireBurnEvent = new FireBurnEvent(PlayObject, nX, nY, Grobal2.ET_FIRE, time * 1000, nDamage);
                M2Share.EventMgr.AddEvent(FireBurnEvent);
            }
            if (PlayObject.Envir.GetEvent(nX + 1, nY) == null)
            {
                FireBurnEvent FireBurnEvent = new FireBurnEvent(PlayObject, nX + 1, nY, Grobal2.ET_FIRE, time * 1000, nDamage);
                M2Share.EventMgr.AddEvent(FireBurnEvent);
            }
            if (PlayObject.Envir.GetEvent(nX, nY + 1) == null)
            {
                FireBurnEvent FireBurnEvent = new FireBurnEvent(PlayObject, nX, nY + 1, Grobal2.ET_FIRE, time * 1000, nDamage);
                M2Share.EventMgr.AddEvent(FireBurnEvent);
            }
            return true;
        }

        private bool MagBigExplosion(BaseObject BaseObject, int nPower, int nX, int nY, int nRage)
        {
            var result = false;
            IList<BaseObject> BaseObjectList = new List<BaseObject>();
            BaseObject.GetMapBaseObjects(BaseObject.Envir, nX, nY, nRage, BaseObjectList);
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
            return result;
        }

        private bool MagElecBlizzard(BaseObject BaseObject, int nPower)
        {
            var result = false;
            IList<BaseObject> BaseObjectList = new List<BaseObject>();
            BaseObject.GetMapBaseObjects(BaseObject.Envir, BaseObject.CurrX, BaseObject.CurrY, M2Share.Config.ElecBlizzardRange, BaseObjectList);
            for (var i = 0; i < BaseObjectList.Count; i++)
            {
                var TargeTBaseObject = BaseObjectList[i];
                int nPowerPoint;
                if (TargeTBaseObject.LifeAttrib != Grobal2.LA_UNDEAD)
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
            return result;
        }

        private int MagMakeHolyCurtain(BaseObject BaseObject, int nPower, short nX, short nY)
        {
            var result = 0;
            if (BaseObject.Envir.CanWalk(nX, nY, true))
            {
                IList<BaseObject> BaseObjectList = new List<BaseObject>();
                MagicEvent MagicEvent = null;
                BaseObject.GetMapBaseObjects(BaseObject.Envir, nX, nY, 1, BaseObjectList);
                for (var i = 0; i < BaseObjectList.Count; i++)
                {
                    var TargeTBaseObject = BaseObjectList[i];
                    if (TargeTBaseObject.Race >= ActorRace.Animal && M2Share.RandomNumber.Random(4) + (BaseObject.Abil.Level - 1) > TargeTBaseObject.Abil.Level && TargeTBaseObject.Master == null)
                    {
                        TargeTBaseObject.OpenHolySeizeMode(nPower * 1000);
                        if (MagicEvent == null)
                        {
                            MagicEvent = new MagicEvent
                            {
                                BaseObjectList = new List<BaseObject>(),
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

                if (result > 0 && MagicEvent != null)
                {
                    var HolyCurtainEvent = new HolyCurtainEvent(BaseObject.Envir, nX - 1, nY - 2, Grobal2.ET_HOLYCURTAIN, nPower * 1000);
                    M2Share.EventMgr.AddEvent(HolyCurtainEvent);
                    MagicEvent.Events[0] = HolyCurtainEvent;
                    HolyCurtainEvent = new HolyCurtainEvent(BaseObject.Envir, nX + 1, nY - 2, Grobal2.ET_HOLYCURTAIN, nPower * 1000);
                    M2Share.EventMgr.AddEvent(HolyCurtainEvent);
                    MagicEvent.Events[1] = HolyCurtainEvent;
                    HolyCurtainEvent = new HolyCurtainEvent(BaseObject.Envir, nX - 2, nY - 1, Grobal2.ET_HOLYCURTAIN, nPower * 1000);
                    M2Share.EventMgr.AddEvent(HolyCurtainEvent);
                    MagicEvent.Events[2] = HolyCurtainEvent;
                    HolyCurtainEvent = new HolyCurtainEvent(BaseObject.Envir, nX + 2, nY - 1, Grobal2.ET_HOLYCURTAIN, nPower * 1000);
                    M2Share.EventMgr.AddEvent(HolyCurtainEvent);
                    MagicEvent.Events[3] = HolyCurtainEvent;
                    HolyCurtainEvent = new HolyCurtainEvent(BaseObject.Envir, nX - 2, nY + 1, Grobal2.ET_HOLYCURTAIN, nPower * 1000);
                    M2Share.EventMgr.AddEvent(HolyCurtainEvent);
                    MagicEvent.Events[4] = HolyCurtainEvent;
                    HolyCurtainEvent = new HolyCurtainEvent(BaseObject.Envir, nX + 2, nY + 1, Grobal2.ET_HOLYCURTAIN, nPower * 1000);
                    M2Share.EventMgr.AddEvent(HolyCurtainEvent);
                    MagicEvent.Events[5] = HolyCurtainEvent;
                    HolyCurtainEvent = new HolyCurtainEvent(BaseObject.Envir, nX - 1, nY + 2, Grobal2.ET_HOLYCURTAIN, nPower * 1000);
                    M2Share.EventMgr.AddEvent(HolyCurtainEvent);
                    MagicEvent.Events[6] = HolyCurtainEvent;
                    HolyCurtainEvent = new HolyCurtainEvent(BaseObject.Envir, nX + 1, nY + 2, Grobal2.ET_HOLYCURTAIN, nPower * 1000);
                    M2Share.EventMgr.AddEvent(HolyCurtainEvent);
                    MagicEvent.Events[7] = HolyCurtainEvent;
                    M2Share.WorldEngine.MagicEventList.Add(MagicEvent);
                }
                else
                {
                    if (MagicEvent == null) return result;
                    MagicEvent.BaseObjectList = null;
                }
            }
            return result;
        }

        private bool MagMakeGroupTransparent(BaseObject BaseObject, int nX, int nY, int nHTime)
        {
            var result = false;
            IList<BaseObject> BaseObjectList = new List<BaseObject>();
            BaseObject.GetMapBaseObjects(BaseObject.Envir, nX, nY, 1, BaseObjectList);
            for (var i = 0; i < BaseObjectList.Count; i++)
            {
                var TargeTBaseObject = BaseObjectList[i];
                if (BaseObject.IsProperFriend(TargeTBaseObject))
                {
                    if (TargeTBaseObject.StatusArr[StatuStateConst.STATE_TRANSPARENT] == 0)
                    {
                        TargeTBaseObject.SendDelayMsg(TargeTBaseObject, Grobal2.RM_TRANSPARENT, 0, nHTime, 0, 0, "", 800);
                        result = true;
                    }
                }
            }
            BaseObjectList.Clear();
            return result;
        }

        private bool MabMabe(BaseObject BaseObject, BaseObject TargeTBaseObject, int nPower, int nLevel, int nTargetX, int nTargetY)
        {
            var result = false;
            if (BaseObject.MagCanHitTarget(BaseObject.CurrX, BaseObject.CurrY, TargeTBaseObject))
            {
                if (BaseObject.IsProperTarget(TargeTBaseObject))
                {
                    if (TargeTBaseObject.AntiMagic <= M2Share.RandomNumber.Random(10) && Math.Abs(TargeTBaseObject.CurrX - nTargetX) <= 1 && Math.Abs(TargeTBaseObject.CurrY - nTargetY) <= 1)
                    {
                        BaseObject.SendDelayMsg(BaseObject, Grobal2.RM_DELAYMAGIC, nPower / 3, HUtil32.MakeLong(nTargetX, nTargetY), 2, TargeTBaseObject.ActorId, "", 600);
                        if (M2Share.RandomNumber.Random(2) + (BaseObject.Abil.Level - 1) > TargeTBaseObject.Abil.Level)
                        {
                            var nLv = BaseObject.Abil.Level - TargeTBaseObject.Abil.Level;
                            if (M2Share.RandomNumber.Random(M2Share.Config.MabMabeHitRandRate) < HUtil32._MAX(M2Share.Config.MabMabeHitMinLvLimit, nLevel * 8 - nLevel + 15 + nLv))
                            {
                                if (M2Share.RandomNumber.Random(M2Share.Config.MabMabeHitSucessRate) < nLevel * 2 + 4)
                                {
                                    if (TargeTBaseObject.Race == ActorRace.Play)
                                    {
                                        BaseObject.SetPkFlag(BaseObject);
                                        BaseObject.SetTargetCreat(TargeTBaseObject);
                                    }
                                    TargeTBaseObject.SetLastHiter(BaseObject);
                                    nPower = TargeTBaseObject.GetMagStruckDamage(BaseObject, nPower);
                                    BaseObject.SendDelayMsg(BaseObject, Grobal2.RM_DELAYMAGIC, nPower, HUtil32.MakeLong(nTargetX, nTargetY), 2, TargeTBaseObject.ActorId, "", 600);
                                    if (!TargeTBaseObject.UnParalysis)
                                    {
                                        // 中毒类型 - 麻痹
                                        TargeTBaseObject.SendDelayMsg(BaseObject, Grobal2.RM_POISON, StatuStateConst.POISON_STONE, nPower / M2Share.Config.MabMabeHitMabeTimeRate + M2Share.RandomNumber.Random(nLevel), BaseObject.ActorId, nLevel, "", 650);
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

        private bool MagMakeSinSuSlave(PlayObject PlayObject, UserMagic UserMagic)
        {
            var result = false;
            if (!PlayObject.CheckServerMakeSlave())
            {
                var sMonName = M2Share.Config.Dragon;
                int nExpLevel = UserMagic.Level;
                var nCount = M2Share.Config.DragonCount;
                for (var i = 0; i < M2Share.Config.DragonArray.Length; i++)
                {
                    if (M2Share.Config.DragonArray[i].nHumLevel == 0)
                    {
                        break;
                    }
                    if (PlayObject.Abil.Level >= M2Share.Config.DragonArray[i].nHumLevel)
                    {
                        sMonName = M2Share.Config.DragonArray[i].sMonName;
                        nExpLevel = M2Share.Config.DragonArray[i].nLevel;
                        nCount = M2Share.Config.DragonArray[i].nCount;
                    }
                }
                if (PlayObject.MakeSlave(sMonName, UserMagic.Level, nExpLevel, nCount, dwRoyaltySec) != null)
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

        /// <summary>
        /// 宠物叛变时间
        /// </summary>
        const int dwRoyaltySec = 10 * 24 * 60 * 60;

        private bool MagMakeSlave(PlayObject PlayObject, UserMagic UserMagic)
        {
            var result = false;
            if (!PlayObject.CheckServerMakeSlave())
            {
                var sMonName = M2Share.Config.Skeleton;
                int nExpLevel = UserMagic.Level;
                var nCount = M2Share.Config.SkeletonCount;
                for (var i = 0; i < M2Share.Config.SkeletonArray.Length; i++)
                {
                    if (M2Share.Config.SkeletonArray[i].nHumLevel == 0)
                    {
                        break;
                    }
                    if (PlayObject.Abil.Level >= M2Share.Config.SkeletonArray[i].nHumLevel)
                    {
                        sMonName = M2Share.Config.SkeletonArray[i].sMonName;
                        nExpLevel = M2Share.Config.SkeletonArray[i].nLevel;
                        nCount = M2Share.Config.SkeletonArray[i].nCount;
                    }
                }
                if (PlayObject.MakeSlave(sMonName, UserMagic.Level, nExpLevel, nCount, dwRoyaltySec) != null)
                {
                    result = true;
                }
            }
            return result;
        }

        private bool MagMakeClone(PlayObject PlayObject, UserMagic UserMagic)
        {
            var playCloneObject = new PlayCloneObject(PlayObject);
            return true;
        }

        private bool MagMakeAngelSlave(PlayObject PlayObject, UserMagic UserMagic)
        {
            var result = false;
            if (!PlayObject.CheckServerMakeSlave())
            {
                var sMonName = M2Share.Config.Angel;
                if (PlayObject.MakeSlave(sMonName, UserMagic.Level, UserMagic.Level, 1, dwRoyaltySec) != null)
                {
                    result = true;
                }
            }
            return result;
        }
    }
}