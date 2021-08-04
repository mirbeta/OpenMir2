using System.Collections.Generic;
using SystemModule;

namespace M2Server
{
    public class TFireBurnEvent : TEvent
    {
        public TFireBurnEvent(TBaseObject Creat, int nX, int nY, int nType, int nTime, int nDamage) : base(Creat.m_PEnvir, nX, nY, nType, nTime, true)
        {
            m_nDamage = nDamage;
            m_OwnBaseObject = Creat;
        }

        public override void Run()
        {
            IList<TBaseObject> BaseObjectList;
            TBaseObject TargeTBaseObject;
            if (HUtil32.GetTickCount() - m_dwRunTick > 3000)
            {
                m_dwRunTick = HUtil32.GetTickCount();
                BaseObjectList = new List<TBaseObject>();
                if (m_Envir != null)
                {
                    m_Envir.GetBaseObjects(m_nX, m_nY, true, BaseObjectList);
                    for (var i = 0; i < BaseObjectList.Count; i++)
                    {
                        TargeTBaseObject = BaseObjectList[i];
                        if (TargeTBaseObject != null && m_OwnBaseObject != null && m_OwnBaseObject.IsProperTarget(TargeTBaseObject))
                        {
                            TargeTBaseObject.SendMsg(m_OwnBaseObject, Grobal2.RM_MAGSTRUCK_MINE, 0, m_nDamage, 0, 0, "");
                        }
                    }
                }
                BaseObjectList.Clear();
                BaseObjectList = null;
            }
            base.Run();
        }
    }
}

