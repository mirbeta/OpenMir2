using GameSvr.Actor;
using SystemModule;

namespace GameSvr.Monster.Monsters
{
    public class GuardUnit : AnimalObject
    {
        public sbyte GuardDirection;

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
                if (baseObject.BoCrimeforCastle)
                {
                    if ((HUtil32.GetTickCount() - baseObject.CrimeforCastleTime) < (2 * 60 * 1000))
                    {
                        result = true;
                    }
                    else
                    {
                        baseObject.BoCrimeforCastle = false;
                    }
                    if (baseObject.Castle != null)
                    {
                        baseObject.BoCrimeforCastle = false;
                        result = false;
                    }
                }
                if (Castle.UnderWar)
                {
                    result = true;
                }
                if (Castle.MasterGuild != null)
                {
                    if (baseObject.Master == null)
                    {
                        if (Castle.MasterGuild == baseObject.MyGuild || Castle.MasterGuild.IsAllyGuild(baseObject.MyGuild))
                        {
                            if (LastHiter != baseObject)
                            {
                                result = false;
                            }
                        }
                    }
                    else
                    {
                        if (Castle.MasterGuild == baseObject.Master.MyGuild || Castle.MasterGuild.IsAllyGuild(baseObject.Master.MyGuild))
                        {
                            if (LastHiter != baseObject.Master && LastHiter != baseObject)
                            {
                                result = false;
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
                result = true;
            }
            if (baseObject.TargetCret != null && baseObject.TargetCret.Race == 112)
            {
                result = true;
            }
            if (baseObject.PvpLevel() >= 2)
            {
                result = true;
            }
            if (baseObject.AdminMode || baseObject.StoneMode || baseObject == this)
            {
                result = false;
            }
            return result;
        }
    }
}