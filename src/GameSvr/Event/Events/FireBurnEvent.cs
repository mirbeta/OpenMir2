﻿using GameSvr.Actor;
using SystemModule;

namespace GameSvr.Event.Events
{
    /// <summary>
    /// 火墙
    /// </summary>
    public class FireBurnEvent : MirEvent
    {
        /// <summary>
        /// 火墙运行时间
        /// </summary>
        private int m_fireRunTick;

        public FireBurnEvent(BaseObject Creat, int nX, int nY, int nType, int nTime, int nDamage) : base(Creat.Envir, nX, nY, nType, nTime, true)
        {
            Damage = nDamage;
            OwnBaseObject = Creat;
        }

        public override void Run()
        {
            if ((HUtil32.GetTickCount() - m_fireRunTick) > 3000)
            {
                m_fireRunTick = HUtil32.GetTickCount();
                IList<BaseObject> BaseObjectList = new List<BaseObject>();
                if (m_Envir != null)
                {
                    m_Envir.GetBaseObjects(m_nX, m_nY, true, BaseObjectList);
                    for (var i = 0; i < BaseObjectList.Count; i++)
                    {
                        var targeTBaseObject = BaseObjectList[i];
                        if (targeTBaseObject != null && OwnBaseObject != null && OwnBaseObject.IsProperTarget(targeTBaseObject))
                        {
                            targeTBaseObject.SendMsg(OwnBaseObject, Grobal2.RM_MAGSTRUCK_MINE, 0, Damage, 0, 0, "");
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

