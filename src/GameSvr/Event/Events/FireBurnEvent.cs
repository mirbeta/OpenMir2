using GameSvr.Actor;
using SystemModule;

namespace GameSvr.Event.Events
{
    /// <summary>
    /// 火墙
    /// </summary>
    public class FireBurnEvent : EventInfo
    {
        /// <summary>
        /// 火墙运行时间
        /// </summary>
        protected int FireRunTick;

        public FireBurnEvent(BaseObject Creat, int nX, int nY, int nType, int nTime, int nDamage) : base(Creat.Envir, nX, nY, nType, nTime, true)
        {
            Damage = nDamage;
            OwnBaseObject = Creat;
        }

        public override void Run()
        {
            if ((HUtil32.GetTickCount() - FireRunTick) > 3000)
            {
                FireRunTick = HUtil32.GetTickCount();
                IList<BaseObject> BaseObjectList = new List<BaseObject>();
                if (Envir != null)
                {
                    Envir.GetBaseObjects(nX, nY, true, BaseObjectList);
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
            }
            base.Run();
        }
    }
}

