using GameSvr.Actor;
using GameSvr.Player;
using SystemModule;
using SystemModule.Enums;

namespace GameSvr.Monster.Monsters
{
    /// <summary>
    /// 守卫类
    /// </summary>
    public class GuardUnit : AnimalObject
    {
        public sbyte GuardDirection;
        public bool BoCrimeforCastle;
        public int CrimeforCastleTime = 0;

        public GuardUnit()
        {
            Race = ActorRace.Guard;
        }

        public override void Struck(BaseObject hiter)
        {
            base.Struck(hiter);
            if (Castle != null)
            {
                BoCrimeforCastle = true;
                CrimeforCastleTime = HUtil32.GetTickCount();
            }
        }

        public override bool IsProperTarget(BaseObject baseObject)
        {
            var result = false;
            if (Castle != null)
            {
                if (LastHiter == baseObject)
                {
                    result = true;
                }
                if (Castle.UnderWar)
                {
                    result = true;
                }
                if (baseObject.Race == ActorRace.Guard)
                {
                    var guardObject = (GuardUnit)baseObject;
                    if (guardObject.BoCrimeforCastle)
                    {
                        if ((HUtil32.GetTickCount() - guardObject.CrimeforCastleTime) < (2 * 60 * 1000))
                        {
                            result = true;
                        }
                        else
                        {
                            guardObject.BoCrimeforCastle = false;
                        }
                        if (guardObject.Castle != null)
                        {
                            guardObject.BoCrimeforCastle = false;
                            result = false;
                        }
                    }
                }
                if (Castle.MasterGuild != null)
                {
                    if (baseObject.Master == null)
                    {
                        if (baseObject.Race == ActorRace.Play)
                        {
                            if (Castle.MasterGuild == ((PlayObject)baseObject).MyGuild || Castle.MasterGuild.IsAllyGuild(((PlayObject)baseObject).MyGuild))
                            {
                                if (LastHiter != baseObject)
                                {
                                    result = false;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (baseObject.Master.Race == ActorRace.Play)
                        {
                            if (Castle.MasterGuild == ((PlayObject)baseObject.Master).MyGuild || Castle.MasterGuild.IsAllyGuild(((PlayObject)baseObject.Master).MyGuild))
                            {
                                if (LastHiter != baseObject.Master && LastHiter != baseObject)
                                {
                                    result = false;
                                }
                            }
                        }
                    }
                }
                if (baseObject.AdminMode || baseObject.StoneMode || baseObject.Race >= ActorRace.NPC && baseObject.Race < ActorRace.Animal || baseObject == this || baseObject.Castle == Castle)
                {
                    result = false;
                }
                return result;
            }
            if (LastHiter == baseObject)
            {
                return true;
            }
            if (baseObject.TargetCret != null && baseObject.TargetCret.Race == 112)
            {
                return true;
            }
            if (baseObject.Race == ActorRace.Play)
            {
                if (((PlayObject)baseObject).PvpLevel() >= 2)
                {
                    return true;
                }
            }
            if (baseObject.AdminMode || baseObject.StoneMode || baseObject == this)
            {
                return false;
            }
            return result;
        }
    }
}