using GameSvr.Actor;
using SystemModule;

namespace GameSvr.Monster.Monsters
{
    public class GuardUnit : AnimalObject
    {
        public override void Struck(BaseObject hiter)
        {
            base.Struck(hiter);
            if (Castle != null)
            {
                Bo2B0 = true;
                MDw2B4Tick = HUtil32.GetTickCount();
            }
        }

        public override bool IsProperTarget(BaseObject BaseObject)
        {
            var result = false;
            if (Castle != null)
            {
                if (LastHiter == BaseObject)
                {
                    result = true;
                }
                if (BaseObject.Bo2B0)
                {
                    if ((HUtil32.GetTickCount() - BaseObject.MDw2B4Tick) < (2 * 60 * 1000))
                    {
                        result = true;
                    }
                    else
                    {
                        BaseObject.Bo2B0 = false;
                    }
                    if (BaseObject.Castle != null)
                    {
                        BaseObject.Bo2B0 = false;
                        result = false;
                    }
                }
                if (Castle.m_boUnderWar)
                {
                    result = true;
                }
                if (Castle.m_MasterGuild != null)
                {
                    if (BaseObject.Master == null)
                    {
                        if (Castle.m_MasterGuild == BaseObject.MyGuild || Castle.m_MasterGuild.IsAllyGuild(BaseObject.MyGuild))
                        {
                            if (LastHiter != BaseObject)
                            {
                                result = false;
                            }
                        }
                    }
                    else
                    {
                        if (Castle.m_MasterGuild == BaseObject.Master.MyGuild || Castle.m_MasterGuild.IsAllyGuild(BaseObject.Master.MyGuild))
                        {
                            if (LastHiter != BaseObject.Master && LastHiter != BaseObject)
                            {
                                result = false;
                            }
                        }
                    }
                }
                if (BaseObject.AdminMode || BaseObject.StoneMode || BaseObject.Race >= Grobal2.RC_NPC && BaseObject.Race < Grobal2.RC_ANIMAL || BaseObject == this || BaseObject.Castle == Castle)
                {
                    result = false;
                }
                return result;
            }
            if (LastHiter == BaseObject)
            {
                result = true;
            }
            if (BaseObject.TargetCret != null && BaseObject.TargetCret.Race == 112)
            {
                result = true;
            }
            if (BaseObject.PvpLevel() >= 2)
            {
                result = true;
            }
            if (BaseObject.AdminMode || BaseObject.StoneMode || BaseObject == this)
            {
                result = false;
            }
            return result;
        }
    }
}