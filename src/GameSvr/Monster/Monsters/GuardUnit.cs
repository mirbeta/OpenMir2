using GameSvr.Actor;
using SystemModule;

namespace GameSvr.Monster.Monsters
{
    public class GuardUnit : AnimalObject
    {
        public short m_nX550;
        public short m_nY554;
        
        public override void Struck(TBaseObject hiter)
        {
            base.Struck(hiter);
            if (m_Castle != null)
            {
                bo2B0 = true;
                m_dw2B4Tick = HUtil32.GetTickCount();
            }
        }

        public override bool IsProperTarget(TBaseObject BaseObject)
        {
            var result = false;
            if (m_Castle != null)
            {
                if (m_LastHiter == BaseObject)
                {
                    result = true;
                }
                if (BaseObject.bo2B0)
                {
                    if ((HUtil32.GetTickCount() - BaseObject.m_dw2B4Tick) < (2 * 60 * 1000))
                    {
                        result = true;
                    }
                    else
                    {
                        BaseObject.bo2B0 = false;
                    }
                    if (BaseObject.m_Castle != null)
                    {
                        BaseObject.bo2B0 = false;
                        result = false;
                    }
                }
                if (m_Castle.m_boUnderWar)
                {
                    result = true;
                }
                if (m_Castle.m_MasterGuild != null)
                {
                    if (BaseObject.m_Master == null)
                    {
                        if (m_Castle.m_MasterGuild == BaseObject.m_MyGuild || m_Castle.m_MasterGuild.IsAllyGuild(BaseObject.m_MyGuild))
                        {
                            if (m_LastHiter != BaseObject)
                            {
                                result = false;
                            }
                        }
                    }
                    else
                    {
                        if (m_Castle.m_MasterGuild == BaseObject.m_Master.m_MyGuild || m_Castle.m_MasterGuild.IsAllyGuild(BaseObject.m_Master.m_MyGuild))
                        {
                            if (m_LastHiter != BaseObject.m_Master && m_LastHiter != BaseObject)
                            {
                                result = false;
                            }
                        }
                    }
                }
                if (BaseObject.m_boAdminMode || BaseObject.m_boStoneMode || BaseObject.m_btRaceServer >= Grobal2.RC_NPC && BaseObject.m_btRaceServer < Grobal2.RC_ANIMAL || BaseObject == this || BaseObject.m_Castle == m_Castle)
                {
                    result = false;
                }
                return result;
            }
            if (m_LastHiter == BaseObject)
            {
                result = true;
            }
            if (BaseObject.m_TargetCret != null && BaseObject.m_TargetCret.m_btRaceServer == 112)
            {
                result = true;
            }
            if (BaseObject.PKLevel() >= 2)
            {
                result = true;
            }
            if (BaseObject.m_boAdminMode || BaseObject.m_boStoneMode || BaseObject == this)
            {
                result = false;
            }
            return result;
        }
    }
}