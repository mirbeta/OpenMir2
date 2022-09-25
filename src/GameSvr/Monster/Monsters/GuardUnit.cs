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
                Bo2B0 = true;
                MDw2B4Tick = HUtil32.GetTickCount();
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
                if (baseObject.Bo2B0)
                {
                    if ((HUtil32.GetTickCount() - baseObject.MDw2B4Tick) < (2 * 60 * 1000))
                    {
                        result = true;
                    }
                    else
                    {
                        baseObject.Bo2B0 = false;
                    }
                    if (baseObject.Castle != null)
                    {
                        baseObject.Bo2B0 = false;
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